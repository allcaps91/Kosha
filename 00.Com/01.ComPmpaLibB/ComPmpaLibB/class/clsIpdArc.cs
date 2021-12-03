using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsIpdArc : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(cPF != null)
                {
                    cPF.Dispose();
                    cPF = null;
                }
                if (cPSel != null)
                {
                    cPSel.Dispose();
                    cPSel = null;
                }
                if (cIA != null)
                {
                    cIA.Dispose();
                    cIA = null;
                }
                if (cBAcct != null)
                {
                    cBAcct.Dispose();
                    cBAcct = null;
                }
            }
            base.Dispose(disposing);
        }

        private string Old_Stat = string.Empty;
        private string FstrFoodSuCode = string.Empty;
        private string FstrFoodSuCode2 = string.Empty;
        private string FstrFoodSuCode3 = string.Empty;
        private int A_Bi1 = 0;
        private bool nTime18FLAG = false;
        private bool nBed150FLAG = false;
        private bool FbKekli = false;

        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaSel cPSel = new clsPmpaSel();
        clsIpdAcct cIA = new clsIpdAcct();
        clsBasAcct cBAcct = new clsBasAcct();

        public struct Ins_Diet_Slip
        {
            public PsmhDb iDbCon;
            public string strIpdSuCode;
            public string ArgdietDate;
            public string strGubun;
            public string strBi;
            public string strBun;
            public double nQty;
            public string strActDate;
            public string strDeptCode;
            public string strDrcode;
            public string strWardcode;

            //public Ins_Diet_Slip(PsmhDb pDbCon, string SuCode, string DietDate, string Gbn, string Bi, string Bun, double Qty, string ActDate, string Dept, string DrCode, string Ward)
            //{
            //    iDbCon = pDbCon;
            //    strIpdSuCode = SuCode;
            //    ArgdietDate = DietDate;
            //    strGubun = Gbn;
            //    strBi = Bi;
            //    strBun = Bun;
            //    nQty = Qty;
            //    strActDate = ActDate;
            //    strDeptCode = Dept;
            //    strDrcode = DrCode;
            //    strWardcode = Ward;
            //}
        } 

        public bool ARC_MAIN_PROCESS(PsmhDb pDbCon, string ArgDate)
        {
            bool rtnVal = true;

            try
            {
                // 병실료 계산에 필요한 기본정보를 SET

                //Old_Stat = "NO";
                A_Bi1 = Convert.ToInt16(VB.Mid(clsPmpaType.TIT.Bi, 1, 1));
                if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55") { A_Bi1 = 6; }//자보

                clsPmpaPb.GstrGbChild = "0";   //정맥주사수기료 소아가산 GbChild 기본 Setting
                                               //clsPmpaPb.GstatWRITE = "OK";

                clsPmpaType.TIT.ArcDate = ArgDate;
                clsPmpaType.TIT.Age = ComFunc.AgeCalcEx(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin2, ArgDate);
                clsPmpaType.ARC.Nal = 1;

                clsPmpaType.IA.Date = ArgDate; //수가 Old,New 산정시 필요함
                clsPmpaType.IA.Bi = Convert.ToInt16(clsPmpaType.TIT.Bi);    //자격을 넣어줌 - 수가세팅에서 사용
                clsPmpaType.IA.GbSpc = clsPmpaType.TIT.GbSpc;
                clsPmpaType.IA.Gbilban2 = clsPmpaType.TIT.Gbilban2;     //외국new
                clsPmpaType.IA.pano = clsPmpaType.TIT.Pano;
                clsPmpaType.IA.DrCode = clsPmpaType.TIT.DrCode;
                clsPmpaType.IA.IPDNO = clsPmpaType.TIT.Ipdno;

                FbKekli = false;

                clsPmpaPb.GstrDietDate = ArgDate;       //ARC일자와동일

                #region 퇴원계산시 당일분의 식대 계산(아침,점심만)
                if (clsPmpaPb.GstrARC == "ARC" || clsPmpaPb.GstrARC == "" || clsPmpaPb.GstrARC == "OUT" || clsPmpaPb.GstrARC == "OUTGEN")
                {
                    if (Food_Gesan3(pDbCon, clsPmpaPb.GstrDietDate, "") == false)                    
                        return false;
                }
                #endregion

                #region 재원환자 주사수기료 자동발생
                //퇴원계산시는 주사수기료 계산안함
                if (clsPmpaPb.GstrARC != "OUT" && clsPmpaPb.GstrARC != "OUTGEN")
                {
                    if (OCS_JOJE_WRITE(pDbCon) == false)       //주사조제료 산정(2001-12-12일부터)
                        return false;
                    if (OCS_JOJE_WRITE_02(pDbCon) == false)    //주사조제료 KK010 산정(2012-07-09일부터)
                        return false;
                    if (OCS_JOJE_WRITE_02_1(pDbCon) == false)
                        return false;
                    if (OCS_JOJE_WRITE_03(pDbCon) == false)    //주사조제료 KK045 산정(2012-09-05일부터)
                        return false;
                    if (OCS_JOJE_WRITE_04(pDbCon) == false)   //주사조제료 KK042 산정(2012-09-05일부터)
                        return false;
                }
                #endregion

                #region 재원기간 앞부분을 구분변경시 마지막일자는 당일분은 식대,의약품관리료,복약지도료를 계산함
                if (clsPmpaPb.GstrARC == "ICUPDT")
                {
                    clsPmpaPb.GstrDietDate = ArgDate;
                    if (Food_Gesan3(pDbCon, clsPmpaPb.GstrDietDate, "") == false)  //식대 3끼 계산                    
                        return false;

                    clsPmpaType.TIT.ArcDate = ArgDate;
                    if (R04_Tuyak_Write(pDbCon) == false)                //의약품관리료,복약지도료를 계산함)
                        return false;

                    clsPmpaPb.GstrARC = "";
                }
                #endregion


                if (clsPmpaPb.GstrARC == "OUT" || clsPmpaPb.GstrARC == "OUTGEN")
                {
                    if ((A_Bi1 == 1 || A_Bi1 == 2) && clsPmpaType.TIT.RoomCode == "359" && clsPmpaType.TIT.DeptCode == "RM")
                    {
                        //낯병동 환자는 퇴원일도 AF200 관리료 산정함.
                    }
                    else if (!(A_Bi1 == 2 && clsPmpaType.TIT.DeptCode == "NP"))
                    {
                        if (clsPmpaType.TIT.InDate != ArgDate)
                        {
                            clsPmpaType.TIT.ArcDate = ArgDate;
                            if (clsPmpaPb.GstrARC != "OUTGEN")
                            {
                                rtnVal = R99_Tim_Update(pDbCon);
                            }
                            return rtnVal;
                        }
                    }
                }

                //2010-11-07 윤조연 소아가산없이해줌
                clsPmpaPb.GstrGbChild_Temp = clsPmpaPb.GstrGbChild;
                clsPmpaPb.GstrGbChild = "0";
                clsPmpaType.TIT.ArcDate = ArgDate;    //ARC일자의 당일(ARC때는 -1일, 퇴원시는 퇴원일)

                if (R04_Tuyak_Write(pDbCon) == false)
                    return false;

                //2010-11-07 소아가산원상복원
                clsPmpaPb.GstrGbChild = clsPmpaPb.GstrGbChild_Temp;

                //-------------------------------------------------------------
                //    ARC일자의 입원료,병실차액을 IPD_NEW_SLIP에 INSERT
                //-------------------------------------------------------------

                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                clsPmpaType.TIT.ArcDate = ArgDate;
                nBed150FLAG = false;
                if (clsPmpaType.TIT.InDate == clsPmpaType.TIT.OutDate)
                {
                    nBed150FLAG = false;
                }
                else if (clsPmpaType.TIT.InDate == clsPmpaType.TIT.ArcDate)  //2016-02-18
                {
                    //2016-03-31 응급실 입원자는 병실입원 시각 기준
                    if (clsPmpaType.TIT.AmSet7 == "3" || clsPmpaType.TIT.AmSet7 == "4" || clsPmpaType.TIT.AmSet7 == "5")
                    {
                        if (string.Compare(clsPmpaType.TIT.InTime3, "0600") <= 0 && string.Compare(clsPmpaType.TIT.InTime3, "0000") >= 0)
                            nBed150FLAG = true;
                        else
                            nBed150FLAG = false;
                    }
                    else
                    {
                        if (string.Compare(clsPmpaType.TIT.InTime2, "0600") <= 0 && string.Compare(clsPmpaType.TIT.InTime2, "0000") >= 0)
                            nBed150FLAG = true;
                    }
                }
                if ((clsPmpaType.TIT.AmSet7 == "3" || clsPmpaType.TIT.AmSet7 == "4" || clsPmpaType.TIT.AmSet7 == "5") && clsPmpaType.TIT.WardCode == "33" )
                {
                    if (IPD_NEW_SLIP_V5200_INSERT(pDbCon) == false)  // 'kyo 권역 응급전용 중환자실 관리료 산정  2017-06-20
                    return false;
                }
                if (Run_Arc_AcCode(pDbCon) == false)                //2017-10-01 안전관리료
                    return false;

                if (Run_Arc_Ac302Code(pDbCon) == false)                //소아 진정관리료
                    return false;


                if (R02_Arc_Ilsu_Gesan(pDbCon) == false)
                    return false;

                if (R03_Bas_Room_Get(pDbCon, ArgDate) == false)
                    return false;

                #region 사용하지 않는 함수로 제외시킴 KMC 2017-10-10
                //if (R03_Old_Room_Read() == false)
                //    return false;

                //clsPmpaPb.GnWARD = clsPmpaPb.GnWard_AMT;
                //clsPmpaPb.GnPANT = clsPmpaPb.GnPant_AMT;
                //if (string.Compare(ArgDate, GnWard_DATE) < 0)
                //{
                //    GnWARD = GnWard_OLD;
                //    GnPANT = GnPant_OLD;
                //}
                #endregion

                //2002-08-10 외국인환자 는 자격에 상관없이 무조건 적용
                if (clsPmpaType.TIT.AmSet8 == "1")        //외국인 환자 병실차액 20,000원 증가
                    clsPmpaType.TBR.OverAmt = clsPmpaType.TBR.OverAmt + 20000;


                //2010-03-05 윤조연 소아가산없이해줌
                clsPmpaPb.GstrGbChild_Temp = clsPmpaPb.GstrGbChild;
                clsPmpaPb.GstrGbChild = "0";

                if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                {
                    if (CALL_BO(pDbCon) == false)
                        return false;
                }
                else if (string.Compare(clsPmpaType.TIT.Bi, "11") >= 0 && string.Compare(clsPmpaType.TIT.Bi, "46") < 0)
                {
                    if (CALL_BO(pDbCon) == false)
                        return false;
                }
                else
                {
                    if (CALL_IL(pDbCon) == false)
                        return false;
                }

                //2010-03-05 소아가산원상복원
                clsPmpaPb.GstrGbChild = clsPmpaPb.GstrGbChild_Temp;

                if (clsPmpaPb.GstrARC != "OUTGEN")
                {
                    if (R99_Tim_Update(pDbCon) == false)
                        return false;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 식이수가 계산 및 발생 IPD_NEW_SLIP WRITE        
        /// </summary>
        /// <param name="ArgdietDate"></param>
        /// <param name="ArgDietGbu"></param>
        /// <seealso cref="IARCACCT.bas : Food_Gesan3()"/>
        private bool Food_Gesan3(PsmhDb pDbCon, string ArgdietDate, string ArgDietGbu)
        {
            #region 변수 선언부
            DataTable Dt = new DataTable();
            DataTable Dt2 = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            int i = 0, nRead = 0;
            double nQty = 0;
            int nGubun1 = 0;                    //일반식 갯수
            int nGubun2 = 0;                    //산모식 갯수
            int nGubun3 = 0;                    //치료식 갯수
            int nGubun4 = 0;                    //경관유동식(완제품) 갯수
            int nGuBun8 = 0;
            string strBi = string.Empty;
            string strActDate = string.Empty;
            string strPano = string.Empty;
            string strDeptCode = string.Empty;
            string strDrcode = string.Empty;
            string strWardcode = string.Empty;
            string strSuCode = string.Empty;
            string strIpdSuCode = string.Empty;
            string strBun = string.Empty;
            string strRoom = string.Empty;
            string strBohun = string.Empty;
            string strPart = string.Empty;
            string strSuDate = string.Empty;
            string strOGPDBun = string.Empty;
            string strVCode = string.Empty;
            string strGubun = string.Empty;
            string strFlag = string.Empty; 
            #endregion

            if (string.Compare(ArgdietDate, clsPmpaType.TIT.InDate) < 0)
            {
                return true;
            }

            //Ins_Diet_Slip IDS = new Ins_Diet_Slip(pDbCon, strIpdSuCode, ArgdietDate, strGubun, strBi, strBun, nQty, strActDate, strDeptCode, strDrcode, strWardcode);
            Ins_Diet_Slip IDS = new Ins_Diet_Slip();

            #region 해당일자 식이오더 조회 및 SLIP 생성
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.PANO, C.BI, A.ROOMCODE,  ";
            SQL += ComNum.VBLF + "        C.DEPTCODE, C.DRCODE, B.WARDCODE, A.SUCODE, A.BUN, B.BOHUN,         ";
            SQL += ComNum.VBLF + "        SUM(CASE WHEN A.QTY >= 5 THEN 1 ELSE A.QTY END) QTY, D.GUBUN        ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A,                              ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B,                             ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS C,                                  ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "DIET_NEWCODE D                                ";
            SQL += ComNum.VBLF + "  WHERE A.PANO = '" + clsPmpaType.TIT.Pano + "'                             ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE = TO_DATE('" + ArgdietDate + "','YYYY-MM-DD')             ";
            SQL += ComNum.VBLF + "    AND A.SUCODE NOT IN ('########')                                        ";
            SQL += ComNum.VBLF + "    AND A.SUCODE = D.SUCODE                                                 ";
            SQL += ComNum.VBLF + "    AND a.DietCode =d.DietCode                                              ";
            SQL += ComNum.VBLF + "    AND a.Bun =d.Bun                                                        ";
            if (ArgDietGbu == "OUT")    //당일 퇴원일 경우 아침. 점심 계산함.
            { 
                if (clsPmpaType.TIT.OutDate == clsPublic.GstrSysDate)
                    SQL += ComNum.VBLF + "         AND A.DIETDAY IN ('1','2')       ";
                else if (string.Compare(clsPmpaType.TIT.OutDate, clsPublic.GstrSysDate) < 0)
                    SQL += ComNum.VBLF + "         AND A.DIETDAY IN ('1','2','3')   ";
            }
            SQL += ComNum.VBLF + "    AND C.TRSNO = " + clsPmpaType.TIT.Trsno + "                             ";
            SQL += ComNum.VBLF + "    AND A.PANO = B.PANO                                                     ";
            SQL += ComNum.VBLF + "    AND C.ACTDATE IS NULL                                                   ";
            SQL += ComNum.VBLF + "    AND C.GBIPD NOT IN ('9','D')                                            ";
            SQL += ComNum.VBLF + "    AND C.GBSTS NOT IN ('1','7')                                            ";
            SQL += ComNum.VBLF + "    AND B.IPDNO = C.IPDNO                                                   ";
            SQL += ComNum.VBLF + "  GROUP BY A.ACTDATE, A.PANO, C.BI, A.ROOMCODE, C.DEPTCODE, C.DRCODE,       ";
            SQL += ComNum.VBLF + "           B.WARDCODE, A.SUCODE, A.BUN, B.BOHUN, D.GUBUN                    ";
            SQL += ComNum.VBLF + "  ORDER BY C.BI                                                             ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    strActDate = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                    strPano = Dt.Rows[i]["PANO"].ToString().Trim();
                    strBi = Dt.Rows[i]["BI"].ToString().Trim();
                    strDeptCode = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strDrcode = Dt.Rows[i]["DRCODE"].ToString().Trim();
                    strWardcode = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strSuCode = Dt.Rows[i]["SUCODE"].ToString().Trim();
                    strRoom = Dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    nQty = VB.Val(Dt.Rows[i]["QTY"].ToString());
                    strBun = Dt.Rows[i]["BUN"].ToString().Trim();
                    strBohun = Dt.Rows[i]["BOHUN"].ToString().Trim();
                    strGubun = Dt.Rows[i]["GUBUN"].ToString().Trim();

                    #region Ins_Diet_Slip Set
                    IDS.ArgdietDate = ArgdietDate;
                    IDS.strActDate = strActDate;
                    IDS.strIpdSuCode = strIpdSuCode;
                    IDS.nQty = nQty;
                    IDS.strBi = strBi;
                    IDS.strBun = strBun;
                    IDS.strDeptCode = strDeptCode;
                    IDS.strDrcode = strDrcode;
                    IDS.strGubun = strGubun;
                    IDS.strWardcode = strWardcode;
                    #endregion

                    if (strBun == "01")           //식대 정규식
                    {
                        //특수병미음
                        if (strSuCode == "FD09" || strSuCode == "FD010" || strSuCode == "FA04" || strSuCode == "FA05")
                        {
                            strIpdSuCode = strSuCode;
                        }
                        else
                        {
                            strIpdSuCode = strSuCode;
                            //금식, 사식이 아닌경우
                            if (strSuCode != "FD020" && strSuCode != "FD021")
                            {
                                switch (strBi)
                                {
                                    case "21": strIpdSuCode = "F02T";  break;
                                    case "22":
                                        if (strBohun == "2" || strBohun == "3")   //보호2종환자면서 장애자
                                            strIpdSuCode = "F02T";
                                        else
                                            strIpdSuCode = "F02T";                  //보호2종환자
                                        break;
                                    case "24": strIpdSuCode = "FO1T";  break;
                                    case "31": strIpdSuCode = "F01S";  break;
                                    case "32": strIpdSuCode = "F01V";  break;
                                    case "33": strIpdSuCode = "F01S";  break;
                                    case "52": strIpdSuCode = "F01U";  break;
                                    default: strIpdSuCode = strSuCode; break;
                                }
                            }
                        }

                        IDS.strIpdSuCode = strIpdSuCode;
                        if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                            return false;

                        //금식, 사식이 아닌경우
                        if (strSuCode != "FD020" && strSuCode != "FD021")
                        {
                            if (strBi == "21" || strBi == "22")
                            {
                                //보호환자는 특수병미음에서 본인부담액이 없음.
                                if (strSuCode != "FD09" && strSuCode != "FD010" && strSuCode != "FA04" && strSuCode != "FA05")
                                {
                                    if (strBi == "21")              //21종,
                                    { 
                                        strIpdSuCode = "F03T";
                                        IDS.strIpdSuCode = strIpdSuCode;
                                        if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                            return false;
                                    }
                                    else if (strBi == "22")
                                    { 
                                        strIpdSuCode = "F03T";
                                        IDS.strIpdSuCode = strIpdSuCode;
                                        if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                            return false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strIpdSuCode = strSuCode;
                        IDS.strIpdSuCode = strIpdSuCode;
                        if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                            return false;
                    }
                }
            }
            
            Dt.Dispose();
            Dt = null;

            #endregion

            #region 식대 변경작업.(일반식, 산모식, 치료식)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT X.DIETDAY, SUM(X.GUBUN1) GUBUN1, SUM(X.GUBUN2) GUBUN2, SUM(X.GUBUN3) GUBUN3, SUM(X.GUBUN4) GUBUN4, SUM(X.GUBUN8) GUBUN8 ";
            SQL += ComNum.VBLF + "         FROM (";
            SQL += ComNum.VBLF + "          SELECT A.DIETDAY, CASE WHEN B.GUBUN = '1' THEN TO_NUMBER(GUBUN)  END GUBUN1, ";
            SQL += ComNum.VBLF + "                 CASE WHEN B.GUBUN = '2' THEN TO_NUMBER(GUBUN)  END GUBUN2, ";
            SQL += ComNum.VBLF + "                 CASE WHEN B.GUBUN = '3' THEN TO_NUMBER(GUBUN)  END GUBUN3, ";
            SQL += ComNum.VBLF + "                 CASE WHEN B.GUBUN = '4' THEN TO_NUMBER(GUBUN)  END GUBUN4, ";
            SQL += ComNum.VBLF + "                 CASE WHEN B.GUBUN = '8' THEN TO_NUMBER(GUBUN)  END GUBUN8 ";
            SQL += ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A,   ";
            SQL += ComNum.VBLF + "                 " + ComNum.DB_PMPA + "DIET_NEWCODE B     ";
            SQL += ComNum.VBLF + "           WHERE A.PANO = '" + clsPmpaType.TIT.Pano + "'  ";
            SQL += ComNum.VBLF + "             AND A.ACTDATE = TO_DATE('" + ArgdietDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "             AND A.SUCODE = B.SUCODE ";
            SQL += ComNum.VBLF + "            AND a.DietCode =b.DietCode                                              ";
            SQL += ComNum.VBLF + "            AND a.Bun = b.Bun                                                       ";
            if (ArgDietGbu == "OUT")  //당일 퇴원일 경우 아침. 점심 계산함.
            { 
                if (clsPmpaType.TIT.OutDate == clsPublic.GstrSysDate)
                    SQL += ComNum.VBLF + "         AND A.DIETDAY IN ('1','2') ";
                else if (string.Compare(clsPmpaType.TIT.OutDate, clsPublic.GstrSysDate) < 0)
                    SQL += ComNum.VBLF + "         AND A.DIETDAY IN ('1','2','3') ";
            }
            else
            {
                SQL += ComNum.VBLF + "         AND A.DIETDAY IN ('1','2','3') ";
            }

            SQL += ComNum.VBLF + "             AND B.GUBUN <= '8' ) X ";
            SQL += ComNum.VBLF + "  GROUP BY X.DIETDAY ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    if (Convert.ToInt16(VB.Val(Dt.Rows[i]["GUBUN8"].ToString())) > 0)
                        nGuBun8 += 1;
                    else if (Convert.ToInt16(VB.Val(Dt.Rows[i]["GUBUN4"].ToString())) > 0)
                        nGubun4 += 1;
                    else if (Convert.ToInt16(VB.Val(Dt.Rows[i]["GUBUN3"].ToString())) > 0) 
                        nGubun3 += 1;
                    else if (Convert.ToInt16(VB.Val(Dt.Rows[i]["GUBUN2"].ToString())) > 0) 
                        nGubun2 += 1;
                    else if (Convert.ToInt16(VB.Val(Dt.Rows[i]["GUBUN1"].ToString())) > 0)
                        nGubun1 += 1;
                }
                
                //경관급식
                if (nGuBun8 > 0)
                {
                    IDS.nQty = nGuBun8;
                    IDS.strActDate = ArgdietDate;
                    strFlag = "4";
                    if (Ipd_Diet_Slip_Insert(pDbCon, IDS, strFlag) == false)
                        return false;
                }

                //경관급식 완제품
                if (nGubun4 > 0)
                {
                    IDS.nQty = nGubun4;
                    IDS.strActDate = ArgdietDate;
                    strFlag = "4";
                    if (Ipd_Diet_Slip_Insert(pDbCon, IDS, strFlag) == false)
                        return false;
                }
                
                //치료식
                if (nGubun3 > 0)
                {
                    IDS.nQty = nGubun3;
                    IDS.strActDate = ArgdietDate;
                    strFlag = "3";
                    if (Ipd_Diet_Slip_Insert(pDbCon, IDS, strFlag) == false)
                        return false;
                }
                
                //산모식
                if (nGubun2 > 0)
                {
                    IDS.nQty = nGubun2;
                    IDS.strActDate = ArgdietDate;
                    strFlag = "2";

                    //산모간식 조회 간식때문에 1을 추가함
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT FROM " + ComNum.DB_PMPA + "DIET_NEWORDER ";
                    SQL += ComNum.VBLF + "  WHERE SUCODE  = 'FA03' ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND ACTDATE = TO_DATE('" + ArgdietDate + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        if (Convert.ToInt16(Dt2.Rows[0]["CNT"].ToString()) >= 2)
                        {
                            IDS.nQty += 1;
                        }
                    }
                    Dt2.Dispose();
                    Dt2 = null;

                    if (Ipd_Diet_Slip_Insert(pDbCon, IDS, strFlag) == false)
                        return false;
                }
                
                //일반식
                if (nGubun1 > 0)
                {
                    IDS.nQty = nGubun1;
                    IDS.strActDate = ArgdietDate;
                    strFlag = "1";
                    if (Ipd_Diet_Slip_Insert(pDbCon, IDS, strFlag) == false)
                        return false;
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 식대에서 산재,자보,보호 => QTY때문에 합산함. 
            if (clsPmpaPb.GstrARC != "OUTGEN")
            { 
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                        ";
                SQL += ComNum.VBLF + "       (IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,GBSPC,GBNGT,           ";
                SQL += ComNum.VBLF + "        GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,PART,AMT1 ,               ";
                SQL += ComNum.VBLF + "        AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,                                                               ";
                SQL += ComNum.VBLF + "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,gbsugbs)                   ";
                SQL += ComNum.VBLF + " SELECT IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,SUM(QTY) QTY, NAL,BASEAMT,GBSPC,GBNGT, ";
                SQL += ComNum.VBLF + "        GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,PART,                     ";
                SQL += ComNum.VBLF + "        SUM(AMT1) AMT1,SUM(AMT2) AMT2 ,SEQNO,YYMM,'',ORDERNO,                                          ";
                SQL += ComNum.VBLF + "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,gbsugbs                    ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                             ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'                                                          ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + clsPmpaType.TIT.Trsno + "                                                          ";
                SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD')                                             ";
                SQL += ComNum.VBLF + "    AND BUN  IN ('74')                                                                                 ";
                SQL += ComNum.VBLF + "    AND DRGSELF = '@'                                                                                  ";   //ARS구분 하기위해 사용함.
                SQL += ComNum.VBLF + "  GROUP BY IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,NAL,BASEAMT,GBSPC,GBNGT,            ";
                SQL += ComNum.VBLF + "           GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,PART,                  ";
                SQL += ComNum.VBLF + "           SEQNO,YYMM,DRGSELF,ORDERNO,                                                                 ";
                SQL += ComNum.VBLF + "           ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,gbsugbs                 ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
                
                SQL = "";
                SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "IPD_NEW_SLIP                 ";
                SQL += ComNum.VBLF + "  WHERE BUN IN ('74')                                      ";
                SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + strActDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DRGSELF = '@'                                      ";
                SQL += ComNum.VBLF + "    AND PANO  = '" + clsPmpaType.TIT.Pano + "'             ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + clsPmpaType.TIT.Trsno + "              ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
            }

            #endregion

            #region 영양관리료(일당)
            //의료급여는 대상에서 제외 2015-11-07
            if (clsPmpaType.TIT.Bi != "21" && clsPmpaType.TIT.Bi != "22")
            { 
                nQty = 1;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "DIET_MED_INTERVIEW ";
                SQL += ComNum.VBLF + "  Where PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SQL += ComNum.VBLF + "    AND BDATE = TO_DATE('" + ArgdietDate +"','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND OK = '1' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    strIpdSuCode = "Z0020";
                    IDS.nQty = nQty;
                    IDS.strIpdSuCode = strIpdSuCode;
                    if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "2") == false)
                        return false;
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region 2016-07-13 HD투석후 당일 입원자는 HD실에서 점심,저녁까지 다입력하므로 병동에서 식이를 넣지 않음
            //점심은 무료고 저녁부터 식이계산 함
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.PANO, A.BI, A.SUCODE, A.BUN, ";
            SQL += ComNum.VBLF + "        SUM(CASE WHEN A.QTY >= 5 THEN 1 ELSE A.QTY END) QTY, B.GUBUN            ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A,                                  ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "DIET_NEWCODE B                                    ";
            SQL += ComNum.VBLF + "  WHERE A.PANO = '" + clsPmpaType.TIT.Pano + "'                                 ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE = TO_DATE('" + ArgdietDate + "','YYYY-MM-DD')                 ";
            SQL += ComNum.VBLF + "    AND A.SUCODE = B.SUCODE                                                     ";
            SQL += ComNum.VBLF + "    AND a.DietCode =b.DietCode                                                  ";
            SQL += ComNum.VBLF + "    AND a.Bun =b.Bun                                                            ";
            SQL += ComNum.VBLF + "    AND a.DEPTCODE = 'HD'                                                       ";
            SQL += ComNum.VBLF + "    AND A.DIETDAY = '3'                                                         ";
            SQL += ComNum.VBLF + " GROUP By A.ACTDATE,A.PANO, A.BI, A.SUCODE, A.BUN,B.GUBUN                       ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT b.BI,b.DeptCode,b.DrCode,a.RoomCode,a.WardCode,a.Bohun ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS b ";
                    SQL += ComNum.VBLF + "  Where a.Pano='" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND b.Trsno=" + clsPmpaType.TIT.Trsno + " ";
                    SQL += ComNum.VBLF + "    AND a.Pano=b.Pano(+) ";
                    SQL += ComNum.VBLF + "    AND a.IPDNO = b.IPDNO ";
                    SQL += ComNum.VBLF + "    AND b.GBIPD = '1' ";
                    SQL += ComNum.VBLF + "    AND b.ACTDATE IS NULL ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        strActDate = Dt.Rows[i]["ACTDATE"].ToString().Trim();
                        strPano = Dt.Rows[i]["PANO"].ToString().Trim(); 
                        strSuCode = Dt.Rows[i]["SUCODE"].ToString().Trim();
                        nQty = Convert.ToDouble(Dt.Rows[i]["QTY"].ToString());
                        strBun = Dt.Rows[i]["BUN"].ToString().Trim(); 
                        strGubun = Dt.Rows[i]["GUBUN"].ToString().Trim(); 
                        strBi = Dt2.Rows[0]["BI"].ToString().Trim(); 
                        strDeptCode = Dt2.Rows[0]["DEPTCODE"].ToString().Trim(); 
                        strDrcode = Dt2.Rows[0]["DRCODE"].ToString().Trim(); 
                        strWardcode = Dt2.Rows[0]["WARDCODE"].ToString().Trim(); 
                        strRoom = Dt2.Rows[0]["ROOMCODE"].ToString().Trim(); 
                        strBohun = Dt2.Rows[0]["BOHUN"].ToString().Trim();

                        #region Ins_Diet_Slip Set
                        IDS.ArgdietDate = ArgdietDate;
                        IDS.strActDate = strActDate;
                        IDS.strIpdSuCode = strIpdSuCode;
                        IDS.nQty = nQty;
                        IDS.strBi = strBi;
                        IDS.strBun = strBun;
                        IDS.strDeptCode = strDeptCode;
                        IDS.strDrcode = strDrcode;
                        IDS.strGubun = strGubun;
                        IDS.strWardcode = strWardcode;
                        #endregion

                        if (strBun == "01")            //식대 정규식
                        {
                            //특수병미음
                            if (strSuCode == "FD09" || strSuCode == "FD010" || strSuCode == "FA04" || strSuCode == "FA05")
                            {
                                strIpdSuCode = strSuCode;
                            }
                            else
                            {
                                strIpdSuCode = strSuCode;
                                //금식, 사식이 아닌경우
                                if (strSuCode != "FD020" && strSuCode != "FD021")
                                {
                                    switch (strBi)
                                    {
                                        case "21": strIpdSuCode = "F02T"; break;
                                        case "22":
                                            if (strBohun == "2" || strBohun == "3")      //보호2종환자면서 장애자
                                                strIpdSuCode = "F02T";
                                            else
                                                strIpdSuCode = "F02T";                  //보호2종환자
                                            break;
                                        case "24": strIpdSuCode = "FO1T"; break;
                                        case "31": strIpdSuCode = "F01S"; break;
                                        case "32": strIpdSuCode = "F01V"; break;
                                        case "33": strIpdSuCode = "F01S"; break;
                                        case "52": strIpdSuCode = "F01U"; break;   
                                        default:   strIpdSuCode = strSuCode; break;
                                    }
                                }
                            }

                            IDS.strIpdSuCode = strIpdSuCode;
                            if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                return false;

                            //금식, 사식이 아닌경우
                            if (strSuCode != "FD020" && strSuCode != "FD021")
                            { 
                                if (strBi == "21" || strBi == "22")
                                { 
                                    //보호환자는 특수병미음에서 본인부담액이 없음.
                                    if (strSuCode != "FD09" && strSuCode != "FD010" && strSuCode != "FA04" && strSuCode != "FA05")
                                    {
                                        if (strBi == "21")            //21종,
                                        {
                                            strIpdSuCode = "F03T";
                                            IDS.strIpdSuCode = strIpdSuCode;
                                            if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                                return false;
                                        }
                                        else if (strBi == "22")
                                        { 
                                            strIpdSuCode = "F03T";
                                            IDS.strIpdSuCode = strIpdSuCode;
                                            if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                                return false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        { 
                            strIpdSuCode = strSuCode;
                            if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "1") == false)
                                return false;
                        }
                    }
                    Dt2.Dispose();
                    Dt2 = null;
                }
            }

            Dt.Dispose();
            Dt = null;

            #endregion

            return true;
        }

        public bool Arc_Ipd_Slip_Insert(PsmhDb pDbCon, Ins_Diet_Slip IDS, string strNew)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            string strNu = string.Empty;
            string strGBSELF = string.Empty;
            double nAmt = 0;
            double nBase = 0;

            clsPmpaPb cPb = new clsPmpaPb();
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NU, SUGBF,                                      ";
            SQL += ComNum.VBLF + "       Bamt,   TO_CHAR(Sudate, 'yyyy-mm-dd') GUDATE,    ";
            SQL += ComNum.VBLF + "       OldBamt,TO_CHAR(Sudate3, 'yyyy-mm-dd') GUDATE3,  ";
            SQL += ComNum.VBLF + "       Bamt3,  TO_CHAR(Sudate4, 'yyyy-mm-dd') GUDATE4,  ";
            SQL += ComNum.VBLF + "       Bamt4,  TO_CHAR(Sudate5, 'yyyy-mm-dd') GUDATE5,  ";
            SQL += ComNum.VBLF + "       Bamt5                                            ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT                   ";
            SQL += ComNum.VBLF + "  WHERE SUCODE  = '" + IDS.strIpdSuCode + "'                ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }
            if (Dt.Rows.Count > 0)
            {
                strNu = Dt.Rows[0]["NU"].ToString().Trim();                                      //누적코드

                if (string.Compare(Dt.Rows[0]["GUDATE"].ToString(), IDS.ArgdietDate) <= 0)
                    nBase = Convert.ToInt64(Dt.Rows[0]["BAMT"].ToString());                     //단가
                else if (string.Compare(Dt.Rows[0]["GUDATE3"].ToString(), IDS.ArgdietDate) <= 0)
                    nBase = Convert.ToInt64(Dt.Rows[0]["OLDBAMT"].ToString());                  //단가 
                else if (string.Compare(Dt.Rows[0]["GUDATE4"].ToString(), IDS.ArgdietDate) <= 0)
                    nBase = Convert.ToInt64(Dt.Rows[0]["BAMT3"].ToString());                    //단가 
                else if (string.Compare(Dt.Rows[0]["GUDATE5"].ToString(), IDS.ArgdietDate) <= 0)
                    nBase = Convert.ToInt64(Dt.Rows[0]["BAMT4"].ToString());                    //단가 
                else
                    nBase = Convert.ToInt64(Dt.Rows[0]["BAMT5"].ToString());                    //단가 

                if (strNew == "2")
                {
                    if (VB.Left(clsPmpaType.TIT.Bi, 1) == "2" && clsPmpaType.TIT.DeptCode == "NP")  //NP단일 단가는 0으로 함.2008-12-11 윤조연
                    {
                        nBase = 0;
                    }
                }
                else
                {
                    if (IDS.strGubun != "7") { nBase = 0; }     //2006-06-01부터 단가를 강제로 0 임.
                    if (IDS.strIpdSuCode == "F03T") { strNu = "34"; }
                    if (IDS.strBi == "21" || IDS.strBi == "22" || IDS.strBi == "24")     //보호환자는 저염식 치료식은 단가가 0임 심사계 요청
                    {
                        if (IDS.strBun == "02" && IDS.strIpdSuCode == "FL01")     //저염식만 단가 0, 저염식+치료식이 있으면 수가단가로한다. 김순옥샘 요청
                            nBase = 0;
                    }
                }
                
                strGBSELF = Dt.Rows[0]["SUGBF"].ToString().Trim();                  //급여.비급여
                
                nAmt = IDS.nQty * nBase;                     //금액
                           
                //외국new
                if (clsPmpaType.TIT.Bi == "51" && clsPmpaType.TIT.Gbilban2 == "Y")
                    nAmt = IDS.nQty * (nBase * 2);                    //금액

                switch (strNu)
                {
                    case "16":  clsPmpaType.TIT.Amt[16] += (long)nAmt; break;
                    case "34":  clsPmpaType.TIT.Amt[34] += (long)nAmt; break;
                    default:
                        break;
                }
            }

            Dt.Dispose();
            Dt = null;

            clsPmpaType.TIT.Amt[50] += (long)nAmt;

            if (clsPmpaPb.GstrARC != "OUTGEN")
            {
                #region Ipd_New_Slip Data Set
                cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrActDate;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = IDS.strActDate;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = IDS.strIpdSuCode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = "74";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = strNu;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = IDS.nQty.ToString();
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = "1";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nBase.ToString();
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = strGBSELF;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = IDS.strDeptCode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = IDS.strDrcode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = IDS.strWardcode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = IDS.strIpdSuCode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = "A";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "4";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = "?";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nAmt.ToString();
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "@";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "1";
                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                #endregion
                SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, IDS.iDbCon, ref intRowCnt);
                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(IDS.pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
            }
            return true;
        }

        private bool Ipd_Diet_Slip_Insert(PsmhDb pDbCon, Ins_Diet_Slip IDS, string strFlag)
        {
            Food_SuCode2(clsPmpaType.TIT.Bi, strFlag);

            IDS.strIpdSuCode = FstrFoodSuCode;

            if (FstrFoodSuCode != "")
            { 
                if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "2") == false)
                return false;
            }

            IDS.strIpdSuCode = FstrFoodSuCode2;

            if (FstrFoodSuCode2 != "")
            {
                if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "2") == false)
                    return false;
            }
            
            if (FstrFoodSuCode3 != "")
            {
                IDS.strIpdSuCode = FstrFoodSuCode3;
                if (Arc_Ipd_Slip_Insert(pDbCon, IDS, "2") == false)
                    return false;
            }
            
            return true;
        }

        private void Food_SuCode2(string ArgBi, string ArgGubun)
        {
            //2015-10-01 식대 변경작업.
            FstrFoodSuCode = "";
            FstrFoodSuCode2 = "";
            FstrFoodSuCode3 = "";

            if (ArgBi.Equals("21") || ArgBi.Equals("22"))
            {
                switch (ArgGubun)
                {
                    case "1": FstrFoodSuCode = "AS510"; FstrFoodSuCode2 = "";  break;    //일반신
                    case "2": FstrFoodSuCode = "AS550"; FstrFoodSuCode2 = "";  break;    //산모식
                    case "3": FstrFoodSuCode = "AS520"; FstrFoodSuCode2 = "";  break;    //치료식
                    case "4": FstrFoodSuCode = "AS560"; FstrFoodSuCode2 = "";  break;    //경관유동식(완제품)
                    case "8": FstrFoodSuCode = "AS520"; FstrFoodSuCode2 = "";  break;    //경관급식 4030원
                    default:
                        break;
                }
            }
            else
            {
                switch (ArgGubun)
                {
                    case "1": FstrFoodSuCode = "Y2200"; FstrFoodSuCode2 = "Z0010";  FstrFoodSuCode3 = "Z0011"; break;  //일반식
                    case "2": FstrFoodSuCode = "Y6200"; FstrFoodSuCode2 = "";       FstrFoodSuCode3 = "";      break;  //산모식
                    case "3": FstrFoodSuCode = "Y3200"; FstrFoodSuCode2 = "";       FstrFoodSuCode3 = "";      break;  //치료식
                    case "4": FstrFoodSuCode = "Y7001"; FstrFoodSuCode2 = "";       FstrFoodSuCode3 = "";      break;  //치료식'경관유동식(완제품)
                    case "8": FstrFoodSuCode = "Y7001"; FstrFoodSuCode2 = "";       FstrFoodSuCode3 = "";      break;  //경관급식 4030원
                    default:
                        break;
                }
            }
        }

        private bool OCS_JOJE_WRITE(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            double[,] nDATA = new double[3, 7];
            int i = 0, j = 0;
            
            for (i = 0; i < 7; i++)
            {
                nDATA[1, i] = 0;
                nDATA[2, i] = 0;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT B.SUGBB,                                                           ";
                SQL += ComNum.VBLF + "        SUM(DECODE(SUBSTR(A.SUNEXT,1,2) ,'C-' , A.QTY * A.NAL))   SQTY,    ";
                SQL += ComNum.VBLF + "        SUM(DECODE(SUBSTR(A.SUNEXT,1,2) ,'C-', 0 ,A.QTY * A.NAL)) NSQTY    ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,                              ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT B                                    ";
                SQL += ComNum.VBLF + "  WHERE A.Pano = '" + clsPmpaType.TIT.Pano + "'                            ";
                SQL += ComNum.VBLF + "    AND A.BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "    AND A.BI = '" + clsPmpaType.TIT.Bi + "'                                ";
                SQL += ComNum.VBLF + "    AND A.BUN ='20'                                                        "; //내복약, 외용약, 주사
                SQL += ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT                                                ";
                SQL += ComNum.VBLF + "  GROUP BY A.BUN, B.SUGBB                                                  ";
                SQL += ComNum.VBLF + "  ORDER BY A.BUN ,B.SUGBB                                                  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        switch (Dt.Rows[i]["SUGBB"].ToString().Trim())
                        {
                            case "2":         
                            case "3":
                                nDATA[1, 2] += VB.Val(Dt.Rows[i]["Sqty"].ToString());  //항암주사
                                nDATA[2, 2] += VB.Val(Dt.Rows[i]["NSqty"].ToString()); //일반주사
                                break;
                            case "4":
                                nDATA[1, 3] = VB.Val(Dt.Rows[i]["Sqty"].ToString());   //항암주사
                                nDATA[2, 3] = VB.Val(Dt.Rows[i]["NSqty"].ToString());  //일반주사
                                break;
                            case "5":
                                nDATA[1, 4] = VB.Val(Dt.Rows[i]["Sqty"].ToString());   //항암주사
                                nDATA[2, 4] = VB.Val(Dt.Rows[i]["NSqty"].ToString());  //일반주사
                                break;
                            case "6":
                                nDATA[1, 5] = VB.Val(Dt.Rows[i]["Sqty"].ToString());   //항암주사
                                nDATA[2, 5] = VB.Val(Dt.Rows[i]["NSqty"].ToString());  //일반주사
                                break;
                            default:
                                break;
                        }
                    }
                    
                    Dt.Dispose();
                    Dt = null;

                    for (j = 1; j < 3; j++)
                    {
                        for (i = 1; i < 7; i++)
                        {
                            if (nDATA[j, i] != 0)
                            {
                                if (SUGA_READ_JUSA(pDbCon, i, j, nDATA) == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                
                return true;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
               
                return false;
            }
        }

        private bool SUGA_READ_JUSA(PsmhDb pDbCon, int x, int y, double[,] nData)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            double nQty = 0;
            string strKKCode = string.Empty;
            string strKK058 = string.Empty;
            string strXanb = string.Empty;

            try
            {
                if (y == 1 && nData[1, x] != 0)   //항암주사
                {
                    switch (x)
                    {
                        case 1:
                            strKKCode = "KK156";
                            nQty = nData[1, x];    //IM 수기료
                            break;
                        case 2:
                            strKKCode = "KK158";
                            nQty = nData[1, x];    //IV 수기료
                            break;
                        default:
                            break;
                    }
                }
                else if (y == 2 && nData[2, x] != 0)
                {
                    switch (x)
                    {
                        case 1:
                            strKKCode = "KK010";
                            nQty = nData[2, x];    //IM 수기료
                            break;
                        case 2:
                            if (nData[2, 3] == 0 && nData[2, 4] == 0 && nData[2, 5] == 0)   //IV 수기료
                            {
                                strKKCode = "KK020";
                                nQty = nData[2, x];
                            }
                            else
                            {
                                strKKCode = "KK054";
                                nQty = nData[2, x];
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (strKKCode == "KK158" || strKKCode == "KK151" || strKKCode == "KK020")       //1일당
                {
                    nQty = 1;
                }
                else if (strKKCode == "KK054")
                {
                    if (nQty > 2) { nQty = 2; }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Pano, SuNext, SUM(Qty * Nal) Qty                                  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                ";
                    SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                             ";
                    SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')   ";
                    SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "'                                 ";
                    SQL += ComNum.VBLF + "    AND SuNext = 'KKO54'                                                  ";
                    SQL += ComNum.VBLF + "  GROUP BY Pano, SuNext                                                   ";

                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }

                    if (Dt.Rows.Count > 0)
                    {
                        nQty -= VB.Val(Dt.Rows[0]["QTY"].ToString()); 
                    }

                    Dt.Dispose();
                    Dt = null;

                    if (nQty <= 0) { return true; }
                }

                if (strKKCode != "")
                {
                    if (cPF.Suga_Read(pDbCon, strKKCode) == false)
                        return false;
                }

                cIA.Move_RS_TO_ISG();
                
                //KK158+J0041 루틴 뺌(2021-07-06 의뢰서)
                //2012-06-12 KK158 항암제 발생시 + J0041 자동발생
                //if (strKKCode == "KK158")
                //{
                //    clsPmpaType.ARC.Amt2 = 0;
                //    clsPmpaType.ARC.GbSpc = "0";
                //    clsPmpaType.ARC.GbHost = "4";
                //    clsPmpaType.ISG.SugbF = "0";
                //    if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                //    {
                //        cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                //        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                //        clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                //    }
                //    else
                //    {
                //        clsPmpaPb.GstrGbChild = "0";
                //    }
                //    clsPmpaType.ARC.Qty = nQty;
                //    clsPmpaType.ARC.Nal = 1;
                //    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                //    //2010-02-02 심경순의뢰서 일단 주석
                //    //적용 2010-02-16
                //    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                //    {
                //        clsPmpaType.ARC.Amt1 = 0;
                //        clsPmpaType.ISG.BaseAmt = 0;
                //    }

                //    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                //    {
                //        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                //        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                //        {
                //            if (clsPmpaType.ARC.Qty > 0)
                //            {
                //                if (Arc_Slip_Write(pDbCon) == false)
                //                    return false;
                //            }
                //        }
                //        else
                //        {
                //            if (clsPmpaType.ARC.Amt1 > 0)
                //            {
                //                if (Arc_Slip_Write(pDbCon) == false)
                //                    return false;
                //            }
                //        }
                //    }

                //    if (cPF.Suga_Read(pDbCon, "J0041") == false)
                //        return false;

                //    cIA.Move_RS_TO_ISG();

                //    clsPmpaType.ARC.Amt2 = 0;
                //    clsPmpaType.ARC.GbSpc = "0";
                //    clsPmpaType.ARC.GbHost = "4";
                //    clsPmpaType.ISG.SugbF = "0";
                //    if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                //    {
                //        cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                //        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                //        clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                //    }
                //    else
                //    {
                //        clsPmpaPb.GstrGbChild = "0";
                //    }

                //    clsPmpaType.ARC.Qty = nQty;
                //    clsPmpaType.ARC.Nal = 1;
                //    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                   
                //    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                //    {
                //        clsPmpaType.ARC.Amt1 = 0;
                //        clsPmpaType.ISG.BaseAmt = 0;
                //    }

                //    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                //    {
                //        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);
                    
                //        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                //        {
                //            if (clsPmpaType.ARC.Qty > 0)
                //            {
                //                if (Arc_Slip_Write(pDbCon) == false)
                //                    return false;
                //            }
                //        }
                //        else
                //        {
                //            if (clsPmpaType.ARC.Amt1 > 0)
                //            {
                //                if (Arc_Slip_Write(pDbCon) == false)
                //                    return false;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbSpc = "0";
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ISG.SugbF = "0";
                    if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                    {
                        cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                        clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                    }
                    else
                    {
                        clsPmpaPb.GstrGbChild = "0";
                    }
                    clsPmpaType.ARC.Qty = nQty;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    //2010-02-02 심경순의뢰서 일단 주석
                    //적용 2010-02-16
                    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                    {
                        clsPmpaType.ARC.Amt1 = 0;
                        clsPmpaType.ISG.BaseAmt = 0;
                    }

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                    {
                        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                        {
                            if (clsPmpaType.ARC.Qty > 0)
                            {
                                if (Arc_Slip_Write(pDbCon) == false)
                                    return false;
                            }
                        }
                        else
                        {
                            if (clsPmpaType.ARC.Amt1 > 0)
                            {
                                if (Arc_Slip_Write(pDbCon) == false)
                                    return false;
                            }
                        }
                    }

                //}

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private bool OCS_JOJE_WRITE_02(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0;
            double nQty = 0;
            string strKKCode = string.Empty;
            string strKK058 = string.Empty;
            string strXanb = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SuCode,QTY,NAL                                                    ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                             ";
                SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "'                                 ";
                SQL += ComNum.VBLF + "    AND BUN ='20'                                             ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'                                           ";
                SQL += ComNum.VBLF + "    AND TRIM(SUCODE) IN (                                           ";
                SQL += ComNum.VBLF + "        SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE      ";
                SQL += ComNum.VBLF + "         WHERE GUBUN = '주사수기료자동발생_KK010'             ";
                SQL += ComNum.VBLF + "        ) ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false ;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Convert.ToInt16(Dt.Rows[i]["NAL"].ToString()) >= 0)
                        {
                            nQty += Convert.ToDouble(Dt.Rows[i]["QTY"].ToString());
                        }
                        else
                        {
                            nQty += Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) * -1;
                        }
                    }
                    if (SUGA_READ_JUSA_02(pDbCon, nQty) == false)
                    //if (SUGA_READ_JUSA_02(pDbCon) == false)
                    {
                        Dt.Dispose();
                        Dt = null;
                        return false;
                    }
                }

                Dt.Dispose();
                Dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool SUGA_READ_JUSA_02(PsmhDb pDbCon, double  Qty )
        //private bool SUGA_READ_JUSA_02(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            double nQty = Qty;
            //double nQty = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano, SuNext, SUM(Qty * Nal) Qty ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "' ";
                SQL += ComNum.VBLF + "    AND SuNext = 'KK010' ";
                SQL += ComNum.VBLF + "  GROUP BY Pano, SuNext ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    nQty -= Convert.ToDouble(Dt.Rows[0]["QTY"].ToString());
                }

                Dt.Dispose();
                Dt = null;

                if (nQty <= 0) { return true; }

                if (cPF.Suga_Read(pDbCon, "KK010") == false)
                    return false;

                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                {
                    cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                    clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                }
                else
                {
                    clsPmpaPb.GstrGbChild = "0";
                }
                clsPmpaType.ARC.Qty = nQty;
                clsPmpaType.ARC.Nal = 1;
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                
                if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                {
                    clsPmpaType.ARC.Amt1 = 0;
                    clsPmpaType.ISG.BaseAmt = 0;
                }

                if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                {
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                    {
                        if (clsPmpaType.ARC.Qty > 0)
                        {
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                    }
                    else
                    {
                        if (clsPmpaType.ARC.Amt1 > 0)
                        {
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                    }
                }
                
                return true;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool OCS_JOJE_WRITE_02_1(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            double nQty = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(QTY*NAL) CNT                                                  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                             ";
                SQL += ComNum.VBLF + "    AND ACTDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "'                                 ";
                SQL += ComNum.VBLF + "    AND BUN ='20'                                                         ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'                                                       ";
                SQL += ComNum.VBLF + "    AND SuCode = 'ELCA10'                                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    nQty = Convert.ToDouble(VB.Val(Dt.Rows[0]["CNT"].ToString()));
                }

                Dt.Dispose();
                Dt = null;

                if (nQty > 0)
                {
                    if (cPF.Suga_Read(pDbCon, "KK010") == false)
                        return false;

                    cIA.Move_RS_TO_ISG();

                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbSpc = "0";
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ISG.SugbF = "0";

                    if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                    {
                        cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                        clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                    }
                    else
                    {
                        clsPmpaPb.GstrGbChild = "0";
                    }

                    clsPmpaType.ARC.Qty = nQty;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                    //적용 2010-02-16
                    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                    {
                        clsPmpaType.ARC.Amt1 = 0;
                        clsPmpaType.ISG.BaseAmt = 0;
                    }

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                    {
                        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                        if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                        {
                            if (clsPmpaType.ARC.Qty > 0)
                            {
                                if (Arc_Slip_Write(pDbCon) == false)
                                    return false;
                            }
                        }
                        else
                        {
                            if (clsPmpaType.ARC.Amt1 > 0)
                            {
                                if (Arc_Slip_Write(pDbCon) == false)
                                    return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool OCS_JOJE_WRITE_03(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, j = 0, nNal = 0, nRead = 0, nRead2= 0;
            double nQty = 0;
            string strBDate = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE,SUM(QTY*NAL) QNAL               ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                             ";
                SQL += ComNum.VBLF + "    AND ActDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "'                                 ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'                                                       ";
                SQL += ComNum.VBLF + "    AND SuCode IN ('TETA','IG-TETA')                                      ";
                SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD')                                    ";
                SQL += ComNum.VBLF + " HAVING SUM(QTY*NAL) <> 0                                                 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE,QTY,NAL ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                        SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "' ";
                        SQL += ComNum.VBLF + "    AND ActDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND bDate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "' ";
                        SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                        SQL += ComNum.VBLF + "     AND SuCode IN ('TETA','IG-TETA')  ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }
                        nRead2 = Dt2.Rows.Count;

                        nQty = 0;
                        nNal = 0;

                        for (j = 0; j < nRead2; j++)
                        {
                            nQty = Convert.ToDouble(Dt2.Rows[j]["QTY"].ToString());
                            nNal = Convert.ToInt16(Dt2.Rows[j]["Nal"].ToString());

                            if (nQty > 0)
                            {
                                if (cPF.Suga_Read(pDbCon, "KK045") == false)
                                    return false;

                                cIA.Move_RS_TO_ISG();

                                clsPmpaType.ARC.Amt2 = 0;
                                clsPmpaType.ARC.GbSpc = "0";
                                clsPmpaType.ARC.GbHost = "4";
                                clsPmpaType.ISG.SugbF = "0";

                                if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                                {
                                    cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                                    clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                                }
                                else
                                {
                                    clsPmpaPb.GstrGbChild = "0";
                                }
                                clsPmpaType.ARC.Qty = nQty;
                                clsPmpaType.ARC.Nal = 1;
                                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                                if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                                {
                                    clsPmpaType.ARC.Amt1 = 0;
                                    clsPmpaType.ISG.BaseAmt = 0;
                                }

                                if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                                {
                                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                                    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                                    {
                                        if (clsPmpaType.ARC.Qty > 0)
                                        {
                                            if (Arc_Slip_Write(pDbCon) == false)
                                                return false;
                                        }
                                    }
                                    else
                                    {
                                        if (clsPmpaType.ARC.Amt1 > 0)
                                        {
                                            if (Arc_Slip_Write(pDbCon) == false)
                                                return false;
                                        }
                                    }
                                }
                            }
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;
                
                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool OCS_JOJE_WRITE_04(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, j = 0, nNal = 0, nRead = 0, nRead2 = 0;
            double nQty = 0;
            string strBDate = string.Empty;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE,SUM(QTY*NAL) QNAL               ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                             ";
                SQL += ComNum.VBLF + "    AND ActDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "'                                 ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0'                                                       ";
                SQL += ComNum.VBLF + "    AND TRIM(SuCode) IN ( SELECT TRIM(SUNEXT) FROM ADMIN.BAS_SUN WHERE DAICODE ='633' ) ";
                SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(BDate,'YYYY-MM-DD')                                          ";
                SQL += ComNum.VBLF + "    HAVING SUM(QTY*NAL) <> 0 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                nRead = Dt.Rows.Count;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE,QTY,NAL ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                        SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "' ";
                        SQL += ComNum.VBLF + "    AND ActDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "    AND bDate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND BI = '" + clsPmpaType.TIT.Bi + "' ";
                        SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                        SQL += ComNum.VBLF + "    AND TRIM(SuCode) IN ( SELECT TRIM(SUNEXT) FROM ADMIN.BAS_SUN WHERE DAICODE ='633' ) ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }
                        nRead2 = Dt2.Rows.Count;

                        nQty = 0;
                        nNal = 0;

                        for (j = 0; j < nRead2; j++)
                        {
                            nQty = Convert.ToDouble(Dt2.Rows[j]["QTY"].ToString());
                            nNal = Convert.ToInt16(Dt2.Rows[j]["Nal"].ToString());

                            if (nQty > 0)
                            {
                                if (cPF.Suga_Read(pDbCon, "KK042") == false)
                                    return false;

                                cIA.Move_RS_TO_ISG();

                                clsPmpaType.ARC.Amt2 = 0;
                                clsPmpaType.ARC.GbSpc = "0";
                                clsPmpaType.ARC.GbHost = "4";
                                clsPmpaType.ISG.SugbF = "0";

                                if (clsPmpaType.TIT.Age < 8 && clsPmpaType.ISG.SugbB != "0")
                                {
                                    cBAcct.Bas_PED_Rate(clsPmpaType.TIT.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.TIT.ArcDate);
                                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (long)Math.Truncate((100 + clsPmpaPb.GnPedRate) / 100.0);
                                    clsPmpaPb.GstrGbChild = "1"; //소아가산인 경우 GBCHild = '1' 함 2006-04-20
                                }
                                else
                                {
                                    clsPmpaPb.GstrGbChild = "0";
                                }
                                clsPmpaType.ARC.Qty = nQty;
                                clsPmpaType.ARC.Nal = 1;
                                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                                if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                                {
                                    clsPmpaType.ARC.Amt1 = 0;
                                    clsPmpaType.ISG.BaseAmt = 0;
                                }

                                if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                                {
                                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);

                                    if ((clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22") && clsPmpaType.TIT.DeptCode == "NP")
                                    {
                                        if (clsPmpaType.ARC.Qty > 0)
                                        {
                                            if (Arc_Slip_Write(pDbCon) == false)
                                                return false;
                                        }
                                    }
                                    else
                                    {
                                        if (clsPmpaType.ARC.Amt1 > 0)
                                        {
                                            if (Arc_Slip_Write(pDbCon) == false)
                                                return false;
                                        }
                                    }
                                }
                            }
                        }

                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool R04_Tuyak_Write(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            long R04_B11Amt = 0, R04_B20Amt = 0;
            long R04_MayakAmt = 0;
            try
            {
                //보호정신과는 의약품관리료 계산 안함
                if (VB.Mid(clsPmpaType.TIT.Bi, 1, 1) == "2" && clsPmpaType.TIT.DeptCode == "NP")
                return true;

                //입원일자 이전인 경우 의약품관리료 계산 안함
                if (string.Compare(clsPmpaType.TIT.ArcDate, clsPmpaType.TIT.InDate) < 0)
                    return true;
                
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Fee1) Fee1 ,SUM(Fee2) Fee2, SUM(Fee3) Fee3,";
                SQL += ComNum.VBLF + "        SUM(Fee4) Fee4, SUM(Fee6) Fee6 ,SUM(nvl(AL010,0)) AL010";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NOTE ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return true;
                }
                else
                {
                    R04_B11Amt = Convert.ToInt64(VB.Val(Dt.Rows[0]["Fee4"].ToString()));
                    R04_B20Amt = Convert.ToInt64(VB.Val(Dt.Rows[0]["Fee1"].ToString()));
                    R04_B20Amt += Convert.ToInt64(VB.Val(Dt.Rows[0]["Fee2"].ToString()));
                    R04_B20Amt += Convert.ToInt64(VB.Val(Dt.Rows[0]["Fee3"].ToString()));
                    R04_B20Amt += Convert.ToInt64(VB.Val(Dt.Rows[0]["Fee6"].ToString()));
                    R04_MayakAmt += Convert.ToInt64(VB.Val(Dt.Rows[0]["AL010"].ToString()));
                }

                Dt.Dispose();
                Dt = null;

                if (R04_B11Amt > 0)
                {
                    if (R04_J2000_Write(pDbCon) == false) //입원 복약지도료
                        return false;
                }

                if (R04_B11Amt > 0 || R04_B20Amt > 0)
                { 
                    if (R04_AL201_Write(pDbCon) == false) //의약품관리료
                        return false;
                }

                if (R04_MayakAmt > 0 )
                {
                    if (R04_AL010_Write(pDbCon) == false) //마약의약품관리료
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private bool R04_J2000_Write(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            clsPmpaPb.GstrPowder = "";

            try
            {
                //해당일자에 복약지도료가 있는지 Check (중복계산 방지)
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(Amt1+Amt2) J2000Amt                                ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                  ";
                SQL += ComNum.VBLF + "    AND Bi   = '" + clsPmpaType.TIT.Bi + "'                    ";
                SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND SuNext = 'J2000'                                       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count == 1)
                {
                    if (Convert.ToInt64(VB.Val(Dt.Rows[0]["J2000Amt"].ToString())) > 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }

                Dt.Dispose();
                Dt = null;

                
                //파우더 대상자 
                if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-01-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(b.Qty*b.NAL) POW   ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ocs_Iorder a , " + ComNum.DB_PMPA + "ipd_new_slip b                     ";
                    SQL += ComNum.VBLF + "  WHERE a.Ptno    = '" + clsPmpaType.TIT.Pano + "'             ";
                    SQL += ComNum.VBLF + "    AND a.BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')            ";
                    SQL += ComNum.VBLF + "    AND a.powder ='1'                                          ";
                    SQL += ComNum.VBLF + "    AND a.ptno=b.pano                                            ";
                    SQL += ComNum.VBLF + "    AND a.bdate=b.bdate                                          ";
                    SQL += ComNum.VBLF + "    AND a.orderno=b.orderno                                      ";

                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt.Rows.Count >= 1)
                    {
                        if (Convert.ToInt64(VB.Val(Dt.Rows[0]["POW"].ToString())) > 0)
                        {
                            clsPmpaPb.GstrPowder = "1";
                            Dt.Dispose();
                            Dt = null;
                          
                        }
                    }
                    else
                    {
                        Dt.Dispose();
                        Dt = null;
                    }
                   

                }
                



                //입원 복약지도료
                if (cPF.Suga_Read(pDbCon, "J2000") == false)
                    return false;

                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                if (clsPmpaPb.GstrPowder == "1")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.3);
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal );  //파우더 가산
                }
                else
                {
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                }
                

                if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                {
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);
                    if (clsPmpaType.ARC.Amt1 > 0)
                    {
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;
                    }
                    clsPmpaPb.GstrPowder = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        private bool R04_AL010_Write(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
               

                    //해당일자에 의약품관리료가 있는지 Check (중복계산 방지)                
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT nvl(SUM(Amt1+Amt2),0) AL010Amt                                  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                    SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                  ";
                    SQL += ComNum.VBLF + "    AND Bi   = '" + clsPmpaType.TIT.Bi + "'                    ";
                    SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND SuNext = 'AL010'                                       ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt.Rows.Count == 1)
                    {
                        if (Convert.ToInt64(Dt.Rows[0]["AL010Amt"].ToString()) > 0)
                        {
                            Dt.Dispose();
                            Dt = null;
                            return true;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;

                    if (cPF.Suga_Read(pDbCon, "AL010") == false)
                        return false;

                    cIA.Move_RS_TO_ISG();

                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbSpc = "0";
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ISG.SugbF = "0";

                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                    {
                        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);
                        if (clsPmpaType.ARC.Amt1 > 0)
                        {
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                    }
               

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }
        private bool Run_Arc_AcCode(PsmhDb pDbCon)  //안전 . 감염예방 관리료 
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {

                if (string.Compare(clsPmpaType.TIT.ArcDate, "2020-10-01") >= 0)
                {
                    if (cPF.Suga_Read(pDbCon, "AC321") == false)
                    {
                        return false;
                    }
                }
                else
                {
                    if (cPF.Suga_Read(pDbCon, "AC421") == false)
                    {
                        return false;
                    }
                }


                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                if (clsPmpaType.ARC.Amt1 > 0)
                {
                    if (Arc_Slip_Write(pDbCon) == false)

                        return false;
                }

                if ( string.Compare(clsPmpaType.TIT.ArcDate, "2020-10-01") >= 0)
                {
                    if (cPF.Suga_Read(pDbCon, "AH011") == false)
                    {
                        return false;
                    }
                }
                else
                {
                    if (cPF.Suga_Read(pDbCon, "AH013") == false)
                    {
                        return false;
                    }
                }
                    

                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                if (clsPmpaType.ARC.Amt1 > 0)
                {
                    if (Arc_Slip_Write(pDbCon) == false)

                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }
        private bool Run_Arc_Ac302Code(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {


                //해당일자에 의약품관리료가 있는지 Check (중복계산 방지)                
                SQL = "";
                SQL += ComNum.VBLF + " SELECT nvl(SUM(Qty*Nal),0)    QNty                                    ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a                    ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                  ";
                SQL += ComNum.VBLF + "    AND IPDNO   = '" + clsPmpaType.TIT.Ipdno + "'                    ";
                SQL += ComNum.VBLF + "    AND trsno   = '" + clsPmpaType.TIT.Trsno + "'                    ";
                SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND orderno in  ( select orderno from ADMIN.ocs_iorder b where bdate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD')  and a.pano=b.ptno  and sedation ='1'  )   ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt64(Dt.Rows[0]["QNty"].ToString()) == 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;

                    }
                }

                Dt.Dispose();
                Dt = null;

                if (cPF.Suga_Read(pDbCon, "AC302") == false)
                    return false;

                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;

                if (clsPmpaType.TIT.Age == 0)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.5);
                }
                else if (clsPmpaType.TIT.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.3);
                }

                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                if (clsPmpaType.ARC.Amt1 > 0)
                {
                    if (Arc_Slip_Write(pDbCon) == false)

                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }
        private bool IPD_NEW_SLIP_V5200_INSERT(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {


                //해당일자에 중환자관리료 있는지 Check (중복계산 방지)                
                SQL = "";
                //SQL += ComNum.VBLF + " SELECT nvl(SUM(Qty+Nal),0) QNty                                  ";
                SQL += ComNum.VBLF + " SELECT nvl(SUM(Qty*Nal),0) QNty                                  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                  ";
                SQL += ComNum.VBLF + "    AND IPDNO   = '" + clsPmpaType.TIT.Ipdno + "'                    ";
                SQL += ComNum.VBLF + "    AND BDate >= TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND SuNext = 'V5200'                                       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count == 1)
                {
                    if (Convert.ToInt64(Dt.Rows[0]["QNty"].ToString()) > 2 )
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
             
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (cPF.Suga_Read(pDbCon, "V5200") == false)
                    return false;

                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbSpc = "0";
                clsPmpaType.ARC.GbHost = "4";
                clsPmpaType.ISG.SugbF = "0";

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                if (clsPmpaType.ARC.Amt1 > 0)
                {
                    if (Arc_Slip_Write(pDbCon) == false)

                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }
        private bool R04_AL201_Write(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                if (clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "32")
                {
                    //해당일자에 의약품관리료가 있는지 Check (중복계산 방지)                
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT NVL(SUM(Amt1+Amt2),0) AL651Amt                                ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                    SQL += ComNum.VBLF + "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'                  ";
                    SQL += ComNum.VBLF + "    AND Bi   = '" + clsPmpaType.TIT.Bi + "'                    ";
                    SQL += ComNum.VBLF + "    AND BDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND SuNext = 'AL651'                                       ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt.Rows.Count == 1)
                    {
                        if (Convert.ToInt64(Dt.Rows[0]["AL651Amt"].ToString()) > 0)
                        {
                            Dt.Dispose();
                            Dt = null;
                            return true;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;

                    if (cPF.Suga_Read(pDbCon, "AL651") == false)
                        return false;

                    cIA.Move_RS_TO_ISG();

                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbSpc = "0";
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ISG.SugbF = "0";

                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") < 0 || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
                    {
                        clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 * clsPmpaPb.GISUL[A_Bi1] / 100.0);
                        if (clsPmpaType.ARC.Amt1 > 0)
                        {
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }

        private bool R99_Tim_Update(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            try
            {
                if (clsPmpaType.TIT.ArcQty > 999)
                    clsPmpaType.TIT.ArcQty = 900;
                if (clsPmpaType.TIT.IcuQty > 999)
                    clsPmpaType.TIT.IcuQty = 900;
                if (clsPmpaType.TIT.Ilsu > 999)
                    clsPmpaType.TIT.Ilsu = 900;

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                  ";
                SQL += ComNum.VBLF + "    SET ArcDate = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD'),    ";
                SQL += ComNum.VBLF + "        Age     =  " + clsPmpaType.TIT.Age + ",                               ";
                SQL += ComNum.VBLF + "        Ilsu    =  " + clsPmpaType.TIT.Ilsu + ",                              ";
                SQL += ComNum.VBLF + "        ArcQty  =  " + clsPmpaType.TIT.ArcQty + ",                            ";
                SQL += ComNum.VBLF + "        IcuQty2  =  " + clsPmpaType.TIT.IcuQty2 + ",                          ";
                SQL += ComNum.VBLF + "        MiIlsu  =  " + clsPmpaType.TIT.MiIlsu + ",                            ";
                SQL += ComNum.VBLF + "        MiArcDate = TO_DATE('" + clsPmpaType.TIT.MiArcDate + "','YYYY-MM-DD'),";
                SQL += ComNum.VBLF + "        IcuQty  =  " + clsPmpaType.TIT.IcuQty + "                             ";
                SQL += ComNum.VBLF + "  WHERE IPDNO   =  " + clsPmpaType.TIT.Ipdno + "                              ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);

                    clsPublic.GstrMsgTitle = "재원자 Master Error";
                    clsPublic.GstrMsgList = "병록번호 : " + clsPmpaType.TIT.Pano;
                    clsPublic.GstrMsgList += ComNum.VBLF + "수진자명 : " + clsPmpaType.TIT.Sname;
                    clsPublic.GstrMsgList += ComNum.VBLF + "호실번호 : " + clsPmpaType.TIT.RoomCode;
                    clsPublic.GstrMsgList += ComNum.VBLF + "재원자 Master UpDate 에 문제가 발생되었습니다 ";
                    clsPublic.GstrMsgList += ComNum.VBLF + "전산실 연락후 문제를 해결 하셔야 합니다 !!";
                    clsPublic.GstrMsgList += ComNum.VBLF + " ";
                    clsPublic.GstrMsgList += ComNum.VBLF + "재작업을 안하면 큰문제가 야기됩니다 !!";

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool R02_Arc_Ilsu_Gesan(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nQty = 0, nTempCnt = 0;
            string strDate = string.Empty;
            string strDate2 = string.Empty;
            
            ComFunc CF = new ComFunc();

            try
            {
                //2016-09-09 응급실 경유 입원환자는 실제병동일자부터 계산함
                if (clsPmpaType.TIT.AmSet7 == "3" || clsPmpaType.TIT.AmSet7 == "4" || clsPmpaType.TIT.AmSet7 == "5")
                    clsPmpaType.TIT.ArcQty = CF.DATE_ILSU(pDbCon, clsPmpaType.TIT.ArcDate, VB.Left(clsPmpaType.TIT.WardDate, 10)) + 1;
                else
                    clsPmpaType.TIT.ArcQty = CF.DATE_ILSU(pDbCon, clsPmpaType.TIT.ArcDate, VB.Left(clsPmpaType.TIT.M_InDate, 10)) + 1;

                //TRANS의 재원일수는 Trans의 입원일자를 기준으로 다시 계산함
                clsPmpaType.TIT.Ilsu = CF.DATE_ILSU(pDbCon, clsPmpaType.IA.Date, clsPmpaType.TIT.InDate) + 1;

                //중환자실 재원일수를 읽음
                //ipd_bm 생성 새벽 3시경 - 보통 일일병실료 12시 ~ 1시 사이에 빌드함 윤조연 참조
                SQL = "";
                SQL += ComNum.VBLF + " SELECT COUNT(a.Pano) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_BM a,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b ";
                SQL += ComNum.VBLF + "  WHERE a.Pano='" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode ";
                SQL += ComNum.VBLF + "    AND a.JobDate>=TO_DATE('" + VB.Left(clsPmpaType.TIT.M_InDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.JobDate<=TO_DATE('" + clsPmpaType.IA.Date + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.GbBackup='J' ";
                SQL += ComNum.VBLF + "    AND b.RoomClass = 'U' ";      //ICU 병실만
                SQL += ComNum.VBLF + "    AND b.TBED > 0 ";             //가동병상만
                //SQL += ComNum.VBLF + "    AND a.RoomCode IN (233,234,398) ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                clsPmpaType.TIT.IcuQty = 0;

                if (Dt.Rows.Count > 0)
                {
                    //clsPmpaType.TIT.IcuQty = AdoGetNumber(RsArc, "CNT", 0)
                    //2009-10-22 윤조연 수정함 ipd_bm 생성 새벽 3시경 - 보통 일일병실료 12시 ~ 1시 사이에 빌드함 윤조연 참조
                    //IcuQty는 ipd_bm을 읽어서 하기때문에 1을 더해서 계산함
                    //clsPmpaType.TIT.IcuQty = AdoGetNumber(RsArc, "CNT", 0) + 1  
                    clsPmpaType.TIT.IcuQty = Convert.ToInt32(VB.Val(Dt.Rows[0]["CNT"].ToString()));

                    if ((string.Compare(clsPublic.GstrSysTime, "00:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "03:00") < 0) && clsPmpaPb.GstrARC == "ARC")
                    {
                        //IPD_BM 03 빌드되기에 병실료 자정 빌드시 IPD_NEW_MASTER 읽어 더해줌 2010-04-20
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
                        SQL += ComNum.VBLF + "                  " + ComNum.DB_PMPA + "BAS_ROOM b ";
                        SQL += ComNum.VBLF + "  WHERE (a.ActDate IS NULL OR a.ActDate>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "    AND a.IpwonTime < TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND (a.GatewonTime IS NULL OR a.GatewonTime>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "    AND (a.PrintTime IS NULL OR a.PrintTime>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode ";
                        SQL += ComNum.VBLF + "    AND a.Amset6 <> '*'";
                        SQL += ComNum.VBLF + "    AND a.Amset4 <> '3'";
                        SQL += ComNum.VBLF + "    AND a.PANO = '" + clsPmpaType.TIT.Pano + "' ";
                        SQL += ComNum.VBLF + "    AND b.RoomClass = 'U' ";      //ICU 병실만
                        SQL += ComNum.VBLF + "    AND b.TBED > 0 ";             //가동병상만
                        //SQL += ComNum.VBLF + "    AND ( ROOMCODE IN ( 233,234 ) OR WardCode IN ('32','33')  )  ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }
                        if (Dt2.Rows.Count > 0)
                        {
                            clsPmpaType.TIT.IcuQty = clsPmpaType.TIT.IcuQty + 1;
                        }
                        Dt2.Dispose();
                        Dt2 = null;
                    }
                }

                Dt.Dispose();
                Dt = null;

                //기존 중환자 건수 icuqty2 로 이전  2013-03-15
                clsPmpaType.TIT.IcuQty2 = clsPmpaType.TIT.IcuQty;

                //2013-03-15
                //중환자실 체감제 계산 변경 - 입원기준 2013-03-05 부터

                nTempCnt = 0;
                clsPmpaType.TIT.IcuQty = 0;

                //2010-04-20 윤조연 추가
                if ((string.Compare(clsPublic.GstrSysTime, "00:01") >= 0 && string.Compare(clsPublic.GstrSysTime, "03:00") < 0) && clsPmpaPb.GstrARC == "ARC")
                {
                    //IPD_BM 03 빌드되기에 병실료 자정 빌드시 IPD_NEW_MASTER 읽어 더해줌 2010-04-20
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.Pano  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
                    SQL += ComNum.VBLF + "                     " + ComNum.DB_PMPA + "BAS_ROOM b ";
                    SQL += ComNum.VBLF + "  WHERE (a.ActDate IS NULL OR a.ActDate>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                    SQL += ComNum.VBLF + "    AND a.IpwonTime < TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND (a.GatewonTime IS NULL OR a.GatewonTime>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                    SQL += ComNum.VBLF + "    AND (a.PrintTime IS NULL OR a.PrintTime>=TO_DATE('" + CF.DATE_ADD(pDbCon, clsPmpaType.TIT.ArcDate, 1) + "','YYYY-MM-DD')) ";
                    SQL += ComNum.VBLF + "    AND a.Amset6 <> '*'";
                    SQL += ComNum.VBLF + "    AND a.Amset4 <> '3'";
                    SQL += ComNum.VBLF + "    AND a.PANO = '" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode ";
                    SQL += ComNum.VBLF + "    AND b.RoomClass = 'U' ";      //ICU 병실만
                    SQL += ComNum.VBLF + "    AND b.TBED > 0 ";             //가동병상만
                    //SQL += ComNum.VBLF + "    AND ( ROOMCODE IN ( 233,234 ) OR WardCode IN ('32','33')  )  ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        nTempCnt = 1;  //arc 일자
                    }
                    Dt2.Dispose();
                    Dt2 = null;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(a.JOBDATE,'YYYY-MM-DD') JOBDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_BM a,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b ";
                SQL += ComNum.VBLF + "  WHERE a.Pano='" + clsPmpaType.TIT.Pano + "' ";
                SQL += ComNum.VBLF + "    AND a.JobDate>=TO_DATE('" + VB.Left(clsPmpaType.TIT.M_InDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.JobDate<=TO_DATE('" + clsPmpaType.IA.Date + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND a.GbBackup='J' ";
                SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode ";
                SQL += ComNum.VBLF + "    AND b.RoomClass = 'U' ";      //ICU 병실만
                SQL += ComNum.VBLF + "    AND b.TBED > 0 ";             //가동병상만
                //SQL += ComNum.VBLF + "    AND ( ROOMCODE IN ( 233,234,398 ) OR WardCode IN ('32','33')  )  ";
                SQL += ComNum.VBLF + "  ORDER BY a.JOBDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                clsPmpaType.TIT.IcuQty = 0;
                nQty = 0;
                strDate2 = clsPmpaType.TIT.ArcDate;  //arc일자

                if (Dt2.Rows.Count > 0)
                {
                    for (i = 0; i < Dt2.Rows.Count; i++)
                    {
                        strDate = Dt2.Rows[i]["JobDate"].ToString().Trim();

                        //ARC 형성전 체크
                        if (i == 0 && nTempCnt == 1)
                        {
                            //ipd_bm 형성되기전 중환자재실이면서 하루전 재원체크하여 맞으면
                            if (CF.DATE_ILSU(pDbCon, strDate2, strDate) == 1)
                            {
                                nQty += 1;
                            }
                            else
                            {
                                //하루전 재원 중환자실 아니면 빠져나옴
                                break;
                            }

                            strDate2 = CF.DATE_ADD(pDbCon, strDate2, -1);
                        }

                        if (i != 0)
                        {
                            strDate2 = CF.DATE_ADD(pDbCon, strDate2, -1);
                        }

                        //일수계산 위해 하루뺌
                        strDate = CF.DATE_ADD(pDbCon, strDate, -1);

                        if (CF.DATE_ILSU(pDbCon, strDate2, strDate) == 1)
                        {
                            nQty += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Dt2.Dispose();
                Dt2 = null;

                //중환자실 재원일수 재설정
                clsPmpaType.TIT.IcuQty = nQty;

                //-----(1996/11/12 일일병실료 계산시 자리수 한계로(999이상) 900로 강제로 Seting)-----
                if (clsPmpaType.TIT.ArcQty > 999) { clsPmpaType.TIT.ArcQty = 900; }
                if (clsPmpaType.TIT.Ilsu > 999) { clsPmpaType.TIT.Ilsu = 900; }
                if (clsPmpaType.TIT.IcuQty > 999) { clsPmpaType.TIT.IcuQty = 900; }
                if (clsPmpaType.TIT.IcuQty2 > 999) { clsPmpaType.TIT.IcuQty2 = 900; }
                
                return true;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                if (Dt2 != null)
                {
                    Dt2.Dispose();
                    Dt2 = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private bool R03_Bas_Room_Get(PsmhDb pDbCon, string ArgDate)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            string strTdate1 = string.Empty;
            string strTdate2 = string.Empty;

            try
            {    
                SQL = "";
                SQL += ComNum.VBLF + " SELECT WardCode,RoomCode,RoomClass,Sex,TBed,HBed,GBed,    ";
                SQL += ComNum.VBLF + "        BBed,WardAmt,PantAmt,RondAmt,FoodAmt,OverAmt,      ";
                SQL += ComNum.VBLF + "        TO_CHAR(TransDate1,'YYYY-MM-DD') Tdate1,           ";
                SQL += ComNum.VBLF + "        RoomClass1,IlbanAmt1,OverAmt1,DEPTCODE,            ";
                SQL += ComNum.VBLF + "        TO_CHAR(TransDate2,'YYYY-MM-DD') Tdate2,           ";
                SQL += ComNum.VBLF + "        RoomClass2,IlbanAmt2,OverAmt2                      ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM                     ";
                SQL += ComNum.VBLF + "  WHERE RoomCode = '" + clsPmpaType.TIT.RoomCode + "'      ";
                SQL += ComNum.VBLF + "    AND TBED > 0 ";         //가동중인 병실만
                SQL += ComNum.VBLF + "";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count == 0)
                {

                    clsPublic.GstrMsgTitle = "경고";
                    clsPublic.GstrMsgList = "병록번호 : " + clsPmpaType.TIT.Pano;
                    clsPublic.GstrMsgList += ComNum.VBLF + "수진자명 : " + clsPmpaType.TIT.Sname;
                    clsPublic.GstrMsgList += ComNum.VBLF + "호실번호 : " + clsPmpaType.TIT.RoomCode;
                    clsPublic.GstrMsgList += ComNum.VBLF + "";
                    clsPublic.GstrMsgList += ComNum.VBLF + "병실 MASTER에 없는 병실입니다 !";
                    clsPublic.GstrMsgList += ComNum.VBLF + "전실작업후 재작업 하십시요 !!";
                    clsPublic.GstrMsgList += ComNum.VBLF + "재작업을 안하면 큰문제가 야기됩니다 !!";

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                    //clsPmpaPb.GstatWRITE = "NO";
                    Dt.Dispose();
                    Dt = null;
                    
                    return false;
                }
                else
                {
                    clsPmpaType.TBR.WardCode = Dt.Rows[0]["WardCode"].ToString().Trim();
                    clsPmpaType.TBR.RoomCode = VB.Format(Convert.ToInt16(Dt.Rows[0]["RoomCode"].ToString()), "0000"); 
                    clsPmpaType.TBR.Sex = Dt.Rows[0]["Sex"].ToString().Trim(); 
                    clsPmpaType.TBR.Tbed = Convert.ToInt16(Dt.Rows[0]["Tbed"].ToString());
                    clsPmpaType.TBR.Hbed = Convert.ToInt16(Dt.Rows[0]["Hbed"].ToString()); 
                    clsPmpaType.TBR.Bbed = Convert.ToInt16(Dt.Rows[0]["Bbed"].ToString());
                    clsPmpaType.TBR.Gbed = Convert.ToInt16(Dt.Rows[0]["Gbed"].ToString());
                    clsPmpaType.TBR.PantAmt = Convert.ToInt64(Dt.Rows[0]["PantAmt"].ToString()); 
                    clsPmpaType.TBR.RondAmt = Convert.ToInt64(Dt.Rows[0]["RondAmt"].ToString()); 
                    clsPmpaType.TBR.FoodAMT = Convert.ToInt64(Dt.Rows[0]["FoodAmt"].ToString());       
                    clsPmpaType.TBR.DeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                    
                    // 병실료 계산시 적용일자를 관리함 (97.10.03)
                    strTdate1 = Dt.Rows[0]["Tdate1"].ToString().Trim();
                    strTdate2 = Dt.Rows[0]["Tdate2"].ToString().Trim();

                    if (string.Compare(ArgDate, strTdate1) >= 0 || strTdate1 == "")
                    {
                        clsPmpaType.TBR.RoomClass = Dt.Rows[0]["RoomClass"].ToString().Trim();
                        clsPmpaType.TBR.WardAmt = Convert.ToInt64(Dt.Rows[0]["WardAmt"].ToString());
                        clsPmpaType.TBR.OverAmt = Convert.ToInt64(Dt.Rows[0]["OverAmt"].ToString());
                    }
                    else if (string.Compare(ArgDate, strTdate2) >= 0 || strTdate2 == "")
                    {
                        clsPmpaType.TBR.RoomClass = Dt.Rows[0]["RoomClass1"].ToString().Trim();
                        clsPmpaType.TBR.WardAmt = Convert.ToInt64(Dt.Rows[0]["IlbanAmt1"].ToString());
                        clsPmpaType.TBR.OverAmt = Convert.ToInt64(Dt.Rows[0]["OverAmt1"].ToString());
                    }
                    else
                    {
                        clsPmpaType.TBR.RoomClass = Dt.Rows[0]["RoomClass2"].ToString().Trim();
                        clsPmpaType.TBR.WardAmt = Convert.ToInt64(Dt.Rows[0]["IlbanAmt2"].ToString());
                        clsPmpaType.TBR.OverAmt = Convert.ToInt64(Dt.Rows[0]["OverAmt2"].ToString());
                    }
                   
                    if (clsPmpaType.TBR.WardAmt == 0)
                    { 
                        clsPmpaType.TBR.RoomClass = Dt.Rows[0]["RoomClass"].ToString().Trim();
                        clsPmpaType.TBR.WardAmt = Convert.ToInt64(Dt.Rows[0]["WardAmt"].ToString());
                        clsPmpaType.TBR.OverAmt = Convert.ToInt64(Dt.Rows[0]["OverAmt"].ToString());
                    }

                    Dt.Dispose();
                    Dt = null;

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private bool CALL_BO(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            
            try
            {
                nTime18FLAG = false;

                if (VB.Mid(clsPmpaType.TIT.Bi, 1, 1) == "2" && clsPmpaType.TIT.DeptCode == "NP")      //보호정신과
                {
                    if (Run_Arc_Boho_NP(pDbCon) == false)
                        return false;
                }

                //2014-08-25 격리병실 사용여부
                if (Run_Arc_KEKLI(pDbCon) == false)
                    return false;

                #region Old_Stat 변수 사용안함으로 아래로직 제외 2017-10-10 K.M.C
                //If Old_Stat = "OK" And TOR.OverAmt > TBR.OverAmt Then
                //    TBR.OverAmt = TOR.OverAmt
                //End If

                //If Old_Stat = "OK" And(TOR.RoomCode = "565" Or TOR.WardCode = "33" Or TOR.WardCode = "35") Then   '2013-07-19
                //    TBR.RoomCode = TIT.RoomCode                               ' 전실전 중환자실 발생
                //    TBR.WardCode = TIT.WardCode
                //    TIT.RoomCode = TOR.RoomCode
                //    TIT.WardCode = TOR.WardCode
                //    Call Run_Arc_ICU
                //    If TBR.OverAmt > 0 Then                                   ' 전실후 실료차 발생
                //        TIT.RoomCode = TBR.RoomCode
                //        TIT.WardCode = TBR.WardCode
                //        Call Room_Cha_Gesan


                //    End If
                //    Return
                //End If
                #endregion

                if (clsPmpaType.TIT.RoomCode == "565")
                {
                    if (Run_Arc_INQ(pDbCon) == false)            //신생아 Incubator 2009-12-19 369->640 12/12 바뀜 640->565 13/06/25 바뀜
                        return false;
                }
                else if (clsPmpaType.TIT.RoomCode == "563" || clsPmpaType.TIT.RoomCode == "564")
                {
                    if (Run_Arc_NEW(pDbCon) == false)            //신생아 기존 563호실만 적용하다가 564호 신생아ICU 도 일반신생아 입원료로 적용
                        return false;
                }
                //낮병동 시행않함으로 제외시킴  2017-10-10 KMC
                //else if (clsPmpaType.TIT.RoomCode == "355")
                //{
                //    if (Run_Arc_DayTime_Room(pDbCon) == false)   //낮병동
                //        return false;
                //}
                //else if (clsPmpaType.TIT.RoomCode == "408" || clsPmpaType.TIT.RoomCode == "409")
                //{
                //    if (Run_Arc_HosPice(pDbCon) == false)        //호스피스병동
                //        return false;
                //}
                else
                {
                    //''산재포함 심사팀정희정 2021-03-05 add
                    if ((clsPmpaType.TIT.WardCode == "40" || clsPmpaType.TIT.WardCode == "73" || clsPmpaType.TIT.WardCode == "75") && clsPmpaType.TIT.T_CARE == "Y" && (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2" || VB.Left(clsPmpaType.TIT.Bi, 1) == "3"))
                    {
                        if (Run_Arc_Total(pDbCon) == false)  //입원관리료
                            return false;
                        if (Run_Arc_Nurse(pDbCon) == false)  //간호간병료
                            return false;
                    }
                    else if (clsPmpaType.TIT.WardCode == "4H" )   //2013-06-18
                    {
                        if (Run_Arc_HosPice(pDbCon) == false)                //ICU
                            return false;
                    }
                    else if (clsPmpaType.TIT.WardCode == "33" || clsPmpaType.TIT.WardCode == "35")   //2013-06-18
                    {
                        if (Run_Arc_ICU(pDbCon) == false)                //ICU
                            return false;
                    }
                    else
                    {
                        if (VB.Left(clsPmpaType.TIT.DeptCode, 1) == "M" || clsPmpaType.TIT.DeptCode == "NE" || clsPmpaType.TIT.DeptCode == "NP" || clsPmpaType.TIT.DeptCode == "PD")
                        {
                            if (Run_Arc_SPC(pDbCon) == false)  //내, 소,정신과 
                                return false;
                        }
                        else
                        {
                            if (clsPmpaType.TIT.GbCancer == "1") //// 항암요법 환자 //GS,OG 지만 내소정으로 처리 JJY:2001-03-29
                            {
                                if (Run_Arc_SPC(pDbCon) == false)  //내, 소,정신과 
                                    return false;
                            }
                            else
                            {
                                if (clsPmpaType.TIT.Age >= 0 && clsPmpaType.TIT.Age <= 7)
                                {
                                    if (Run_Arc_SPC(pDbCon) == false)  //8세 미만
                                        return false;
                                }
                                else
                                {
                                    if (Run_Arc_Gen(pDbCon) == false)  
                                        return false;
                                }
                            }
                        }
                    }
                }

                
                #region Old_Stat 변수 사용안함으로 아래로직 제외 2017-10-10 K.M.C
                //If Old_Stat = "OK" And(TIT.RoomCode = "233" Or TIT.RoomCode = "234" Or TIT.RoomCode = "641" Or TIT.RoomCode = "564") And TBR.OverAmt > 0 Then '2009-12-19 368->641 12/12 바뀜
                //    TIT.RoomCode = TOR.RoomCode
                //    TIT.WardCode = TOR.WardCode
                //    Call Room_Cha_Gesan
                //End If
                #endregion

                //2015-08-31
                if (Run_Arc_AuCode(pDbCon) == false)
                    return false;

                #region 일반병실에 보육기 사용료가 책정되어 현재시점으로 기준병실을 다시 정의함 2013-08-07 
                if (clsPmpaType.TIT.RoomCode != "564" && clsPmpaType.TIT.RoomCode != "565" && clsPmpaType.TIT.RoomCode != "0564" && clsPmpaType.TIT.RoomCode != "0565")
                { 
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT PANO FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR ";
                    SQL += ComNum.VBLF + " WHERE FRROOM IN ( 564,565 )";
                    SQL += ComNum.VBLF + "   AND TOROOM NOT IN ( 564,565 )";
                    SQL += ComNum.VBLF + "   AND FRWARD <> TOWARD ";
                    SQL += ComNum.VBLF + "   AND TRUNC(TRSDATE) = TO_DATE('" + clsPmpaType.TIT.ArcDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND IPDNO =" + clsPmpaType.TIT.Ipdno + " ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (cPF.Suga_Read(pDbCon, "AM100") == false)    //보육기 사용료
                            return false;

                        cIA.Move_RS_TO_ISG();

                        clsPmpaType.ARC.Qty = 1;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "4";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;
                    }

                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool CALL_IL(PsmhDb pDbCon)
        {
            try
            {
                if (Run_Arc_ILBAN(pDbCon) == false)
                    return false;
                //2015-08-31
                if (Run_Arc_AuCode(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_Boho_NP(PsmhDb pDbCon)
        {
            
            //2000.1.1 보호NP단일수가 체감제(1-180,181-360,361일이상)
            //보호NP 단일수가코드 변경2008-12-11
            //날짜를 잘못입력함 2008-12-31 로 해야함..

            try
            {
                if (clsPmpaType.TIT.ArcQty < 181)
                {
                    if (cPF.Suga_Read(pDbCon, "AR312") == false)   //1-180일
                        return false;
                    cIA.Move_RS_TO_ISG();

                }
                else if (clsPmpaType.TIT.ArcQty < 361)
                {
                    if (cPF.Suga_Read(pDbCon, "AR322") == false)   //181-360일
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else
                {
                    if (cPF.Suga_Read(pDbCon, "AR333") == false)   //361일이상
                        return false;
                    cIA.Move_RS_TO_ISG();
                }

                //보호 NP단일수가 체감체는 QTY는 무조건 = "1" 임
                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbHost = "0";

                //선택진료비산정 2011-06-01
                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }

                if (Arc_Slip_Write(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_INQ(PsmhDb pDbCon)
        {
           
            try
            {
                if (FbKekli == false)
                { 
                    if (Run_Arc_SPC(pDbCon) == false)         //2014-09-01 격리병실료 대상이면 입원료 산정안함
                        return false;
                }

                if (cPF.Suga_Read(pDbCon, "AM100") == false)    //보육기 사용료
                    return false;
                cIA.Move_RS_TO_ISG();

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbHost = "4";
                if (Arc_Slip_Write(pDbCon) == false)
                    return false;
                
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_SPC(PsmhDb pDbCon)
        {
           
            string strBi = string.Empty;

            try
            {
                if (FbKekli) { return true; }       //2014-09-01 격리병실료 대상이면 입원료 산정안함

                if (nTime18FLAG) { clsPmpaType.TIT.Ilsu -= 1; }  //* 18:00이후 입원자

                if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)     //6인실 기준
                {
                    if (cPF.Suga_Read(pDbCon, "AB2001") == false)  
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H" )     //2인실 기준
                {
                    if (cPF.Suga_Read(pDbCon, "AB2701") == false)
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else if (clsPmpaType.TBR.RoomClass == "K" )                                         //4인실 기준
                {
                    if (cPF.Suga_Read(pDbCon, "AB2401") == false)
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else

                { 
                    //예외사항 적용
                    if (clsPmpaType.TBR.WardCode == "NR" || clsPmpaType.TBR.WardCode == "IQ")
                    {
                        if (cPF.Suga_Read(pDbCon, "AG221") == false)        //6인실 기준
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else
                    {
                        if (cPF.Suga_Read(pDbCon, "AB2201") == false)        //5인실 기준
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                }

                //보호 정신과는 내역만 IPD_NEW_SLIP 추가 시킴. 단가는 0임 윤조연
                if (VB.Left(clsPmpaType.TIT.Bi, 1) == "2" && clsPmpaType.TIT.DeptCode == "NP")
                { 
                    clsPmpaType.ISG.BaseAmt = 0;
                }

                strBi = clsPmpaType.TIT.Bi;

                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                { 
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }
                    else if (clsPmpaType.TIT.ArcQty > 30)
                    {
                        clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    }
                }


                //2016-12-20 신생아실 입원료 체감제 적용안함
                if (clsPmpaType.ISG.Sunext == "AG221")
                    clsPmpaType.ARC.Qty = 1;

                if (nTime18FLAG)
                    clsPmpaType.TIT.Ilsu += 1;   //18:00이후 입원자

                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }
                if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && (clsPmpaType.TBR.RoomClass == "A" || clsPmpaType.TBR.RoomClass == "B" || clsPmpaType.TBR.RoomClass == "C") && (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2"))
                {
                    //  1인실 비급여로 인한 산정은 병실차액에서 발생함
                }
                else
                {
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }

                if ((string.Compare(clsPmpaType.TIT.ArcDate, "2019-10-01") >= 0) && (clsPmpaType.TIT.WardCode != "NR" && clsPmpaType.TIT.WardCode != "IQ" ))
                {
                    if (cPF.Suga_Read(pDbCon, "AI120") == false)        //야간간호료 시행 사용료
                        return false;
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "4";
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }



                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_NEW(PsmhDb pDbCon)
        {
            if (FbKekli) { return true; }
            if (Run_Arc_SPC(pDbCon) == false)
                return false;

            return true;
        }

        private bool Run_Arc_Gen(PsmhDb pDbCon)
        {
            
            string strBi = string.Empty;

            try
            {
                if (FbKekli) { return true; }       //2014-09-01 격리병실료 대상이면 입원료 산정안함

                if (nTime18FLAG) { clsPmpaType.TIT.Ilsu -= 1; }  //* 18:00이후 입원자

                if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)     //6인실 기준
                {
                    if (cPF.Suga_Read(pDbCon, "AB200") == false)
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H" )     //2인실 기준
                {
                    if (cPF.Suga_Read(pDbCon, "AB270") == false)
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else
                {
                    //예외사항 적용
                    if (clsPmpaType.TBR.WardCode == "NR" || clsPmpaType.TBR.WardCode == "IQ")
                    {
                        if (cPF.Suga_Read(pDbCon, "AG221") == false)        //6인실 기준
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (clsPmpaType.TBR.RoomClass == "K")
                    {
                        if (cPF.Suga_Read(pDbCon, "AB240") == false)        //4인실 기준
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else
                    {
                        if (cPF.Suga_Read(pDbCon, "AB220") == false)        //5인실 기준
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                }
                
                strBi = clsPmpaType.TIT.Bi;

                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }
                    else if (clsPmpaType.TIT.ArcQty > 30)
                    {
                        clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    }
                }
                
                //2016-12-20 신생아실 입원료 체감제 적용안함
                if (clsPmpaType.ISG.Sunext == "AG221")
                    clsPmpaType.ARC.Qty = 1;

                if (nTime18FLAG)
                    clsPmpaType.TIT.Ilsu += 1;   //18:00이후 입원자

                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }

                if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && (clsPmpaType.TBR.RoomClass == "A" || clsPmpaType.TBR.RoomClass == "B" || clsPmpaType.TBR.RoomClass == "C") && (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2"))
                {
                    //  1인실 비급여로 인한 산정은 병실차액에서 발생함
                }
                else
                {
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }

                if ((string.Compare(clsPmpaType.TIT.ArcDate, "2019-10-01") >= 0) && (clsPmpaType.TIT.WardCode != "NR" && clsPmpaType.TIT.WardCode != "IQ" ))
                {
                    if (cPF.Suga_Read(pDbCon, "AI120") == false)        //보육기 사용료
                        return false;
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "4";
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }


                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_HosPice(PsmhDb pDbCon)
        {
            string strBi = string.Empty;
            
            try
            {
                if (FbKekli) { return true; }

                if (nTime18FLAG) { clsPmpaType.TIT.Ilsu -= 1; }  //* 18:00이후 입원자

                //if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)     //6인실 기준
                //{
                //    if (cPF.Suga_Read(pDbCon, "AB200") == false)
                //        return false;
                //    cIA.Move_RS_TO_ISG();
                //}
                //else if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")     //2인실 기준
                //{
                //    if (cPF.Suga_Read(pDbCon, "AB270") == false)
                //        return false;
                //    cIA.Move_RS_TO_ISG();
                //}
                //else if(clsPmpaType.TBR.RoomClass == "K")       //4인실 기준
                //{
                //    if (cPF.Suga_Read(pDbCon, "AB240") == false)
                //        return false;
                //    cIA.Move_RS_TO_ISG();
                //}
                //else
                //{
                //    if (cPF.Suga_Read(pDbCon, "AB220") == false)    //5인실 기준
                //        return false;
                //    cIA.Move_RS_TO_ISG();
                //}

                //내소정 적용
                //if (VB.Left(clsPmpaType.TIT.DeptCode, 1) == "M" || clsPmpaType.TIT.DeptCode == "NE" || clsPmpaType.TIT.DeptCode == "NP" || clsPmpaType.TIT.DeptCode == "PD")
                //{
                //    if (cPF.Suga_Read(pDbCon, "AB2401") == false)  //내, 소,정신과 2010-09-30
                //        return false;
                //}
                //else
                //{
                //    if (clsPmpaType.TIT.Age <= 7)
                //    {
                //        if (cPF.Suga_Read(pDbCon, "AB2401") == false)       //8세 미만
                //            return false;
                //    }
                //}
                if (cPF.Suga_Read(pDbCon, "AB240") == false) { return false; }  //2019-07-01 호스피스는 모든 병실을 AB240행위로 발생한다 

                cIA.Move_RS_TO_ISG();

                strBi = clsPmpaType.TIT.Bi;
                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }
                    else if (clsPmpaType.TIT.ArcQty > 30)
                    {
                        clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    }
                }

                if (nTime18FLAG)
                    clsPmpaType.TIT.Ilsu += 1;   //18:00이후 입원자

                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }

                if (Arc_Slip_Write(pDbCon) == false)
                    return false;

                if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-03-08") >= 0)
                {
                    if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-07-01") >= 0)
                    {
                        //1등급으로 변경(2021-06-28)
                        if (clsPmpaType.TBR.OverAmt == 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM211") == false) { return false; } //4인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                        else if (clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM271") == false) { return false; } //1인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    else if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-04-01") >= 0)
                    {
                        if (clsPmpaType.TBR.OverAmt == 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM221") == false) { return false; } //4인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                        else if (clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM281") == false) { return false; } //1인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    else if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0)
                    {
                        if (clsPmpaType.TBR.OverAmt == 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM211") == false) { return false; } //4인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                        else if (clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM271") == false) { return false; } //1인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    else if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-04-01") >= 0)
                    {
                        if (clsPmpaType.TBR.OverAmt == 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM221") == false) { return false; } //4인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                        else if (clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM281") == false) { return false; } //1인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    else
                    {
                        if (clsPmpaType.TBR.OverAmt == 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM202") == false) { return false; } //4인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                        else if (clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "WM262") == false) { return false; } //1인실 기준
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                    if (nBed150FLAG)
                    {
                        clsPmpaType.ARC.Qty = 1.5;
                        clsPmpaType.ARC.Nal = 1;
                    }
                    else
                    {
                        clsPmpaType.ARC.Qty = 1;
                        clsPmpaType.ARC.Nal = 1;
                        if (clsPmpaType.TIT.ArcQty >= 61)
                        {
                            clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                        }
                    }

                    clsPmpaType.ARC.GbHost = "0";
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                    clsPmpaPb.G7TAMT = 0;
                    clsPmpaType.ARC.Amt2 = 0;

                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;

                }
                
                
                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_Total(PsmhDb pDbCon)
        {
            string strBi = string.Empty;
            
            try
            {
                if (FbKekli) { return true; }       //'2014-09-01 격리병실료 대상이면 입원료 산정안함

                if (nTime18FLAG) { clsPmpaType.TIT.Ilsu -= 1; }  //* 18:00이후 입원자

                if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)       
                {
                    if (cPF.Suga_Read(pDbCon, "AO200") == false)    //6인실 기준
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else if(clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")
                {
                    if (cPF.Suga_Read(pDbCon, "AO280") == false)    //2인실 기준
                        return false;
                    cIA.Move_RS_TO_ISG();
                }
                else if (clsPmpaType.TBR.RoomClass == "K" )
                {
                    if (cPF.Suga_Read(pDbCon, "AO240") == false)    //4인실 기준
                        return false;
                    cIA.Move_RS_TO_ISG();
                }

                else
                {
                    if (cPF.Suga_Read(pDbCon, "AO220") == false)    //5인실 기준
                        return false;
                    cIA.Move_RS_TO_ISG();
                }

                //내소정 적용
                if (VB.Left(clsPmpaType.TIT.DeptCode, 1) == "M" || clsPmpaType.TIT.DeptCode == "NE" || clsPmpaType.TIT.DeptCode == "NP" || clsPmpaType.TIT.DeptCode == "PD")
                {
                    if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)
                    { 
                        if (cPF.Suga_Read(pDbCon, "AO2001") == false)  //내, 소,정신과
                            return false;
                    }
                    else if(clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")
                    {
                        if (cPF.Suga_Read(pDbCon, "AO2801") == false)  //내, 소,정신과
                            return false;
                    }
                    else if (clsPmpaType.TBR.RoomClass == "K" )
                    {
                        if (cPF.Suga_Read(pDbCon, "AO2401") == false)  //내, 소,정신과
                            return false;
                    }
                    else
                    {
                        if (cPF.Suga_Read(pDbCon, "AO2201") == false)  //내, 소,정신과
                            return false;
                    }
                }
                else
                {
                    if (clsPmpaType.TIT.Age <= 7)
                    {
                        if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "AO2001") == false)  //내, 소,정신과
                                return false;
                        }
                        else if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")
                        {
                            if (cPF.Suga_Read(pDbCon, "AO2801") == false)  //내, 소,정신과
                                return false;
                        }
                        else if (clsPmpaType.TBR.RoomClass == "K" )
                        {
                            if (cPF.Suga_Read(pDbCon, "AO2401") == false)  //내, 소,정신과
                                return false;
                        }
                        else
                        {
                            if (cPF.Suga_Read(pDbCon, "AO2201") == false)  //내, 소,정신과
                                return false;
                        }
                    }
                }

                cIA.Move_RS_TO_ISG();

                strBi = clsPmpaType.TIT.Bi;
                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }
                    else if (clsPmpaType.TIT.ArcQty > 30)
                    {
                        clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    }
                }

                if (nTime18FLAG)
                    clsPmpaType.TIT.Ilsu += 1;   //18:00이후 입원자

                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }
                if ( string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && (clsPmpaType.TBR.RoomClass == "A" || clsPmpaType.TBR.RoomClass == "B" || clsPmpaType.TBR.RoomClass == "C"))
                { 

                }
                else
                {
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }
                

                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_Nurse(PsmhDb pDbCon)
        {
            string strBi = string.Empty;
            
            try
            {
                if (FbKekli) { return true; }

                if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H" )
                {
                    if (cPF.Suga_Read(pDbCon, "AV820") == false)
                        return false;
                }
                else
                {
                    if (cPF.Suga_Read(pDbCon, "AV222") == false)
                        return false;
                }
                
                cIA.Move_RS_TO_ISG();
                
                //내소정 적용
                if (VB.Left(clsPmpaType.TIT.DeptCode, 1) == "M" || clsPmpaType.TIT.DeptCode == "NE" || clsPmpaType.TIT.DeptCode == "NP" || clsPmpaType.TIT.DeptCode == "PD")
                {
                    if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")
                    {
                        if (cPF.Suga_Read(pDbCon, "AV8201") == false)
                            return false;
                    }
                    else
                    {
                        if (cPF.Suga_Read(pDbCon, "AV2221") == false) //내, 소,정신과 2010-09-30
                            return false;
                    }
              
                }
                else
                {
                    if (clsPmpaType.TIT.Age <= 7)
                    {
                        if (clsPmpaType.TBR.RoomClass == "G" || clsPmpaType.TBR.RoomClass == "H")
                        {
                            if (cPF.Suga_Read(pDbCon, "AV8201") == false)
                                return false;
                        }
                        else
                        {
                            if (cPF.Suga_Read(pDbCon, "AV2221") == false) //내, 소,정신과 2010-09-30
                                return false;
                        }
                    }
                }

                cIA.Move_RS_TO_ISG();

                strBi = clsPmpaType.TIT.Bi;
                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }
                    else if (clsPmpaType.TIT.ArcQty > 30)
                    {
                        clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    }
                }
                
                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }
                if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && (clsPmpaType.TBR.RoomClass == "A" || clsPmpaType.TBR.RoomClass == "B" || clsPmpaType.TBR.RoomClass == "C"))
                {

                }
                else
                {
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_ICU(PsmhDb pDbCon)
        {
            string strCode = string.Empty;
            string strBi = string.Empty;
            
            try
            {
                //중환자 격리관리료 나와도 산정함 2016-10-11
                //2015-06-18 간호등급 변경시 입원수속 프로그램의 FrmRoomEntry에도 코드를 변경해주어야 함.
                if (clsPmpaType.TIT.Age > 0)
                {
                    
                    //2021-09-30  2021년 10월 1일부터 33병동, 35병동 간호등급 차등됨.
                    //BAS_BCODE.GUBUN = 'BAS_간호등급수가코드' 사용 불가
                    //다음번 변경 때 또 차등일 경우 별도 클래스로 작성 예정
                    if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-10-01") >= 0)
                    {
                        #region 2021년 10월 1일부터 중환자실 간호등급 병동별 차등 
                        if (clsPmpaType.TIT.WardCode == "33")
                        {
                            strCode = "AJ220";      //2등급
                        }
                        else if (clsPmpaType.TIT.WardCode == "35")
                        {
                            strCode = "AJ210";      //1등급
                        }

                        if (strCode == "")
                        {
                            strCode = "AJ210";      //예외처리 1등급 처리
                        } 
                        #endregion
                    }
                    else
                    {
                        strCode = clsVbfunc.GetBCodeCODE(pDbCon, "BAS_간호등급수가코드", clsPmpaType.TIT.ArcDate);
                    }

                    if (strCode != "")
                    {
                        if (cPF.Suga_Read(pDbCon, strCode) == false)
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    
                }
                else
                {
                    //if (clsPmpaType.TIT.RoomCode == "564")
                    if (clsPmpaType.TBR.RoomClass == "U" && clsPmpaType.TIT.DeptCode == "NR")
                    {
                        cPF.Suga_Read(pDbCon, "AJ201");   //I.C.U 입원료(신생아)
                        cIA.Move_RS_TO_ISG();
                    }
                    else
                    {
                        if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-10-01") >= 0)
                        {
                            #region 2021년 10월 1일부터 중환자실 간호등급 병동별 차등 
                            if (clsPmpaType.TIT.WardCode == "33")
                            {
                                strCode = "AJ220";      //2등급
                            }
                            else if (clsPmpaType.TIT.WardCode == "35")
                            {
                                strCode = "AJ210";      //1등급
                            }

                            if (strCode == "")
                            {
                                strCode = "AJ210";      //예외처리 1등급 처리
                            }
                            #endregion
                        }
                        else
                        {
                            strCode = clsVbfunc.GetBCodeCODE(pDbCon, "BAS_간호등급수가코드", clsPmpaType.TIT.ArcDate);
                        }

                        if (strCode != "")
                        {
                            if (cPF.Suga_Read(pDbCon, strCode) == false)
                                return false;
                            cIA.Move_RS_TO_ISG();
                        }
                        
                    }
                }
                
                strBi = clsPmpaType.TIT.Bi;
                //1999.5.8일 입원시각이 00:00-06:00은 입원료에 0.5가산
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;
                    clsPmpaType.ARC.Nal = 1;
                }
                else if ((string.Compare(strBi, "40") > 0 && string.Compare(strBi, "44") < 0) || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "32" || strBi == "33")    //기타보험 체감적용 제외
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                }
                else
                {
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    //if (clsPmpaType.TIT.ArcQty >= 16 && clsPmpaType.TIT.ArcQty <= 30)
                    //{
                    //    clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    //}
                    //else if (clsPmpaType.TIT.ArcQty > 30)
                    //{
                    //    clsPmpaType.ARC.Qty = 0.85;              //보험  30일 이상 85%
                    //}
                }

                if (nTime18FLAG)
                { 
                    clsPmpaType.TIT.Ilsu += 1;      //18:00이후 입원자
                    clsPmpaType.TIT.IcuQty += 1;    //18:00이후 입원자
                }

                clsPmpaType.ARC.GbHost = "0";
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }

                if (Arc_Slip_Write(pDbCon) == false)
                    return false;

                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_KEKLI(PsmhDb pDbCon)
        {
            
            string strMsg = string.Empty;

            try
            {
              
                strMsg = RTN_KEKLI_ROOM(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.ArcDate, clsPmpaType.TBR.Tbed, clsPmpaType.TIT.Ipdno);

                if (clsPmpaType.TIT.WardCode == "33" || clsPmpaType.TIT.WardCode == "35")
                {
                 
                    if (strMsg == "1" || strMsg == "2" || strMsg == "3")
                    {
                        if (cPF.Suga_Read(pDbCon, "AJ010") == false)   //일반격리 
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "4" || strMsg == "5" || strMsg == "6")
                    {
                        if (cPF.Suga_Read(pDbCon, "AJ020") == false)   //음압격리 
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                   
                    else
                    {
                        return true;
                    }

                }
               else if (clsPmpaType.TIT.WardCode == "4H" ) //'2019-07-01 add 호스피스는 모든병실을  AB240 행위료로 산정
                {

                    if (strMsg == "1" || strMsg == "2" || strMsg == "3")
                    {
                        if (cPF.Suga_Read(pDbCon, "AB240") == false)   //일반격리 
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "4" || strMsg == "5" || strMsg == "6")
                    {
                        if (cPF.Suga_Read(pDbCon, "AB240") == false)   //음압격리 
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }

                    else
                    {
                        return true;
                    }

                }
                else
                {
                    if (strMsg == "1" )
                    {
                        if (cPF.Suga_Read(pDbCon, "AK200") == false)   //일반격리 1인실
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "2" )
                    {
                        if (cPF.Suga_Read(pDbCon, "AK201") == false)   //일반격리 다인실
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "4")
                    {
                        if (cPF.Suga_Read(pDbCon, "AK210") == false)   //음압격리 1인실
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "5")
                    {
                        if (cPF.Suga_Read(pDbCon, "AK211") == false)   //음압격리 다인실
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else if (strMsg == "3" || strMsg == "6")
                    {
                        if (cPF.Suga_Read(pDbCon, "AK202") == false)   //음압격리 2인실
                            return false;
                        cIA.Move_RS_TO_ISG();
                    }
                    else
                    {
                        return true;
                    }
                    
                }

                clsPmpaType.ARC.Qty = 1;
                clsPmpaType.ARC.Nal = 1;
                if (nBed150FLAG)
                {
                    clsPmpaType.ARC.Qty = 1.5;               //입원시각이 00시에서 06시 사이 1.5인거 확인해보기
                }
                
                clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                clsPmpaType.ARC.Amt2 = 0;
                clsPmpaType.ARC.GbHost = "0";

                #region 선택진료 정보 Setting
                clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                SMM.ArgIO = "I";
                SMM.ArgPano = clsPmpaType.TIT.Pano;
                SMM.ArgBDate = clsPmpaType.IA.Date;
                SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                SMM.ArgBun = clsPmpaType.ISG.Bun;
                SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                SMM.ArgETC = "";
                #endregion

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ARC.Amt2 = 0;
                if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                {
                    clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                }
                
                if (Arc_Slip_Write(pDbCon) == false)
                    return false;
                
                if (nTime18FLAG)
                    clsPmpaType.TIT.Ilsu -= 1;

                FbKekli = true;
                if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-07-01") >= 0 && clsPmpaType.TIT.WardCode == "4H" && FbKekli == true)
                {
                    //격리실 1등급
                    if (cPF.Suga_Read(pDbCon, "WN211") == false)   //음압격리 2인실
                    {
                        return false;
                    }
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (nBed150FLAG)
                    {
                        clsPmpaType.ARC.Qty = 1.5;               //입원시각이 00시에서 06시 사이 1.5인거 확인해보기
                    }

                    if (clsPmpaType.TIT.ArcQty >= 61)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }

                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "0";

                    if (Arc_Slip_Write(pDbCon) == false)
                    {
                        return false;
                    }
                }
                else if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-04-01") >= 0 && clsPmpaType.TIT.WardCode == "4H" && FbKekli == true)
                {
                    if (cPF.Suga_Read(pDbCon, "WN221") == false)   //음압격리 2인실
                    {
                        return false;
                    }
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (nBed150FLAG)
                    {
                        clsPmpaType.ARC.Qty = 1.5;               //입원시각이 00시에서 06시 사이 1.5인거 확인해보기
                    }

                    if (clsPmpaType.TIT.ArcQty >= 61)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }

                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "0";

                    if (Arc_Slip_Write(pDbCon) == false)
                    {
                        return false;
                    }
                }
                else if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && clsPmpaType.TIT.WardCode == "4H" && FbKekli == true)
                {
                    if (cPF.Suga_Read(pDbCon, "WN211") == false)   //음압격리 2인실
                    {
                        return false;
                    }
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (nBed150FLAG)
                    {
                        clsPmpaType.ARC.Qty = 1.5;               //입원시각이 00시에서 06시 사이 1.5인거 확인해보기
                    }

                    if (clsPmpaType.TIT.ArcQty >= 61)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }

                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "0";

                    if (Arc_Slip_Write(pDbCon) == false)
                    {
                        return false;
                    }
                }
                else if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-03-08") >= 0 && clsPmpaType.TIT.WardCode == "4H" && FbKekli == true)
                {
                    if (cPF.Suga_Read(pDbCon, "WN221") == false)   //음압격리 2인실
                    {
                        return false;
                    }
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    if (nBed150FLAG)
                    {
                        clsPmpaType.ARC.Qty = 1.5;               //입원시각이 00시에서 06시 사이 1.5인거 확인해보기
                    }

                    if (clsPmpaType.TIT.ArcQty >= 61)
                    {
                        clsPmpaType.ARC.Qty = 0.9;              //보험  16일 이상 90%
                    }

                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "0";

                    if (Arc_Slip_Write(pDbCon) == false)
                    {
                        return false;
                    }

                }

                //2014-09-01 일부 격리병실은 병실차액료 생성안됨(각 해당로직안으로 들어감)
                if (FbKekli)
                    clsPmpaType.TBR.OverAmt = 0;

                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_ILBAN(PsmhDb pDbCon)
        {
            string strCode = string.Empty;
            string strBi = string.Empty;
            
            try
            {
                if (FbKekli == false)       //2014-09-01 격리병실료 대상이면 입원료 산정안함
                {
                    //일반은 50% 가산된 입원료
                    //2013-06-18
                    if (clsPmpaType.TBR.RoomClass == "U" && clsPmpaType.TBR.WardCode != "NR")
                    {
                        if (string.Compare(clsPmpaType.TIT.ArcDate, "2021-10-01") >= 0)
                        {
                            #region 2021년 10월 1일부터 중환자실 간호등급 병동별 차등 
                            if (clsPmpaType.TIT.WardCode == "33")
                            {
                                strCode = "AJ220";      //2등급
                            }
                            else if (clsPmpaType.TIT.WardCode == "35")
                            {
                                strCode = "AJ210";      //1등급
                            }

                            if (strCode == "")
                            {
                                strCode = "AJ210";      //예외처리 1등급 처리
                            }
                            #endregion
                        }
                        else
                        {
                            strCode = clsVbfunc.GetBCodeCODE(pDbCon, "BAS_간호등급수가코드", clsPmpaType.TIT.ArcDate);
                        }
                        if (strCode != "")
                        {
                            if (cPF.Suga_Read(pDbCon, strCode) == false)
                                return false;
                            cIA.Move_RS_TO_ISG();
                        }
                    }
                    else
                    { 
                        
                        if (clsPmpaType.TBR.RoomClass == "O" || clsPmpaType.TBR.OverAmt > 0)     //6인실 기준
                        {
                            if (cPF.Suga_Read(pDbCon, "AB2001") == false)
                                return false;
                            cIA.Move_RS_TO_ISG();
                        }
                        else
                        { 
                            //예외사항 적용
                            if (clsPmpaType.TBR.WardCode == "NR" || clsPmpaType.TBR.WardCode == "IQ")
                            {
                                if (cPF.Suga_Read(pDbCon, "AG221") == false)        //6인실 기준
                                    return false;
                                cIA.Move_RS_TO_ISG();
                            }
                            else
                            {
                                if (cPF.Suga_Read(pDbCon, "AB2201") == false)        //5인실 기준
                                    return false;
                                cIA.Move_RS_TO_ISG();
                            }
                        }
                    }

                    clsPmpaType.ARC.Qty = 1.5; //일반은 50% 가산함
                    clsPmpaType.ARC.Nal = 1;
                    //2013-09-05 일반자격환자의 경우 입원료와 병실차액을 분리함(심사과장요청)
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);
                    
                    //외국new
                    if (clsPmpaType.TIT.Bi == "51" && clsPmpaType.TIT.Gbilban2 == "Y")
                    {
                        //ARC.Amt1 = ((SG.BaseAmt * 2) * ARC.Qty * ARC.Nal) + TBR.OverAmt   '(기본 * 1.5) + 병실차액더해줌
                        //2013-09-05 일반자격환자의 경우 입원료와 병실차액을 분리함(심사과장요청)
                        clsPmpaType.ARC.Amt1 = (long)Math.Truncate((clsPmpaType.ISG.BaseAmt * 2) * clsPmpaType.ARC.Qty * clsPmpaType.ARC.Nal);   //(기본*1.5)+ 병실차액더해줌
                    }
                    
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "0";

                    //100단위 사사오입절사 적용(13400 = 13000, 13500 = 14000)
                    clsPmpaType.ARC.Amt1 = (long)Math.Truncate(clsPmpaType.ARC.Amt1 / 1000.0 + 0.5) * 1000;

                    //선택진료비산정 2011-06-01
                    #region 선택진료 정보 Setting
                    clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                    SMM.ArgSpc = clsPmpaType.TIT.GbSpc;
                    SMM.ArgIO = "I";
                    SMM.ArgPano = clsPmpaType.TIT.Pano;
                    SMM.ArgBDate = clsPmpaType.IA.Date;
                    SMM.ArgBi = clsPmpaType.TIT.Bi.ToString();
                    SMM.ArgGamek = clsPmpaType.TIT.GbGameK;
                    SMM.ArgDeptCode = clsPmpaType.TIT.DeptCode;
                    SMM.ArgDrCode = clsPmpaType.TIT.DrCode;
                    SMM.ArgBun = clsPmpaType.ISG.Bun;
                    SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                    SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                    SMM.ArgIPDNO = clsPmpaType.TIT.Ipdno;
                    SMM.ArgETC = "";
                    #endregion

                    clsPmpaPb.G7TAMT = 0;
                    clsPmpaType.ARC.Amt2 = 0;
                    if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                    {
                        clsPmpaType.ARC.Amt2 = clsPmpaPb.GnSelAmt * clsPmpaType.ARC.Nal;  //선택진료는 arc.qty = > 무조건1
                    }

                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;

                    if ((string.Compare(clsPmpaType.TIT.ArcDate, "2019-10-01") >= 0) && (clsPmpaType.TIT.WardCode != "NR" && clsPmpaType.TIT.WardCode != "IQ" && clsPmpaType.TIT.WardCode != "33" && clsPmpaType.TIT.WardCode != "35" ))
                    {
                        if (cPF.Suga_Read(pDbCon, "AI120") == false)        //보육기 사용료
                            return false;
                        cIA.Move_RS_TO_ISG();
                        clsPmpaType.ARC.Qty = 1;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "4";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;
                    }

                }

                //2012-05-23 51종 보육기사용료 의뢰
                //2013-08-07 일반병실에 보육기사용료가 적용되어 현재시점으로 병실기준을 다시 정의함 (민철)
                //If TIT.Bi = "51" And (TIT.RoomCode = "0640" Or TIT.RoomCode = "640") Then
                if (clsPmpaType.TIT.Bi == "51" && clsPmpaType.TIT.RoomCode == "565")
                {
                    if (cPF.Suga_Read(pDbCon, "AM100") == false)        //보육기 사용료
                        return false;
                    cIA.Move_RS_TO_ISG();
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ARC.Nal = 1;
                    clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "4";
                    if (Arc_Slip_Write(pDbCon) == false)
                        return false;
                }

                if (Room_Cha_Gesan(pDbCon) == false)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private bool Run_Arc_AuCode(PsmhDb pDbCon)
        {
            int nQty = 0;
            
            try
            {
                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                    return true;

                nQty = 1;
                if (nBed150FLAG) { nQty = 2; }

                //2015-08-31
                if (clsPmpaType.TIT.GbDRG == "D")
                {
                    if (string.Compare(clsPmpaType.TIT.InDate, clsPmpaPb.GstrMedSupDay) >= 0)
                    {
                        if (cPF.Suga_Read(pDbCon, "AU204") == false)   //의료질평가지원금(입원)
                            return false;
                        cIA.Move_RS_TO_ISG();
                        clsPmpaType.ARC.Qty = nQty;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "0";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;

                        if (cPF.Suga_Read(pDbCon, "AU302") == false)   //교육수련분야(입원)
                            return false;
                        cIA.Move_RS_TO_ISG();
                        clsPmpaType.ARC.Qty = nQty;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "0";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;
                        if (string.Compare(clsPmpaType.TIT.ArcDate, "2018-09-01") >= 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "AU403") == false)   //교육수련분야(입원)
                                return false;
                            cIA.Move_RS_TO_ISG();
                            clsPmpaType.ARC.Qty = nQty;
                            clsPmpaType.ARC.Nal = 1;
                            clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                            clsPmpaType.ARC.Amt2 = 0;
                            clsPmpaType.ARC.GbHost = "0";
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                          
                    }
                }
                else
                {
                    if (string.Compare(clsPmpaType.TIT.InDate, clsPmpaPb.GstrMedSupDay) >= 0)
                    {
                        if (cPF.Suga_Read(pDbCon, "AU204") == false)   //의료질평가지원금(입원)
                            return false;
                        cIA.Move_RS_TO_ISG();
                        clsPmpaType.ARC.Qty = nQty;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "0";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;

                        if (cPF.Suga_Read(pDbCon, "AU302") == false)   //교육수련분야(입원)
                            return false;
                        cIA.Move_RS_TO_ISG();
                        clsPmpaType.ARC.Qty = nQty;
                        clsPmpaType.ARC.Nal = 1;
                        clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                        clsPmpaType.ARC.Amt2 = 0;
                        clsPmpaType.ARC.GbHost = "0";
                        if (Arc_Slip_Write(pDbCon) == false)
                            return false;
                     
                        if (string.Compare(clsPmpaType.TIT.ArcDate, "2018-09-01") >= 0)
                        {
                            if (cPF.Suga_Read(pDbCon, "AU403") == false)   //교육수련분야(입원)
                                return false;
                            cIA.Move_RS_TO_ISG();
                            clsPmpaType.ARC.Qty = nQty;
                            clsPmpaType.ARC.Nal = 1;
                            clsPmpaType.ARC.Amt1 = clsPmpaType.ISG.BaseAmt;
                            clsPmpaType.ARC.Amt2 = 0;
                            clsPmpaType.ARC.GbHost = "0";
                            if (Arc_Slip_Write(pDbCon) == false)
                                return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        //2016-09-21 격리병실 사용여부 1인, 다인, 2인 추가 (음압구분은 있고, 음압수가는 없음)
        private string RTN_KEKLI_ROOM(PsmhDb pDbCon, string ArgPano, string ArgDate, int ArgBed, long ArgIpdNo)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            string rtnVal = string.Empty;

            ComFunc CF = new ComFunc();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT INFECT,AIR_INFECT FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR_INFECT ";
                SQL += ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "   AND IPDNO = " + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "   AND TRUNC(TRSDATE) < TO_DATE('" + CF.DATE_ADD(pDbCon, ArgDate, 1) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ORDER By TRSDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["INFECT"].ToString().Trim() == "1")
                    {
                        if (ArgBed == 1)
                            rtnVal = "1";        //1인실
                        else if (ArgBed == 2)
                            rtnVal = "3";        //2인실
                        else
                            rtnVal = "2";        //다인실
                    }
                    else if (Dt.Rows[0]["AIR_INFECT"].ToString().Trim() == "1")
                    {
                        if (ArgBed == 1)
                            rtnVal = "4";        //1인실
                        else if (ArgBed == 2)
                            rtnVal = "6";        //2인실
                        else
                            rtnVal = "5";        //다인실
                    }
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return "";
            }
        }

        private bool Room_Cha_Gesan(PsmhDb pDbCon)
        {
            
            try
            {
                if (clsPmpaType.TBR.RoomClass == "U") { return true; }      //중환자실 제외
                if (clsPmpaType.TBR.WardCode == "NR") { return true; }      //신생아실 제외
                if (clsPmpaType.TBR.OverAmt == 0) { return true; }          //병실료차액 없음

                if ( string.Compare(clsPmpaType.TIT.ArcDate, "2019-07-01") >= 0 && clsPmpaType.TIT.WardCode!= "4H" && (clsPmpaType.TBR.RoomClass == "A" || clsPmpaType.TBR.RoomClass == "B" || clsPmpaType.TBR.RoomClass == "C") &&  ( VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2")   )         
                {
                    if (cPF.Suga_Read(pDbCon, "AB901") == false)   //1인실 비급여 차액 + 60000
                        return false;
                    cIA.Move_RS_TO_ISG();

                    clsPmpaType.ISG.BaseAmt = clsPmpaType.TBR.OverAmt + 60000 ;
                    clsPmpaType.ARC.Amt1 = clsPmpaType.TBR.OverAmt + 60000 ;
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ARC.Qty = 1;
                    clsPmpaType.ISG.Nu = "21";

                }
                else
                {
                    if (cPF.Suga_Read(pDbCon, "AZ900") == false)   //중환자실 일반격리관리료
                        return false;
                    cIA.Move_RS_TO_ISG();

                    clsPmpaType.ISG.BaseAmt = clsPmpaType.TBR.OverAmt;
                    clsPmpaType.ARC.Amt1 = clsPmpaType.TBR.OverAmt;
                    clsPmpaType.ARC.Amt2 = 0;
                    clsPmpaType.ARC.GbHost = "4";
                    clsPmpaType.ARC.Qty = 1;
                }
               

                if (Arc_Slip_Write(pDbCon) == false)
                    return false;
                
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public bool Arc_Slip_Write(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            clsPmpaPb cPb = new clsPmpaPb();
           
            try
            {
                if (clsPmpaPb.GstrARC != "OUTGEN")  //퇴원계산서 발행시 조회만 할 경우
                {
                    //RS 정보를 ISG정보에 담아서 사용해야 함

                    #region Ipd_New_Slip Data Set
                    cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrActDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.ArcDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = clsPmpaType.ARC.Qty.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = clsPmpaType.ARC.Nal.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = clsPmpaType.ISG.BaseAmt.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = clsPmpaType.ISG.SugbE;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = clsPmpaType.ISG.SugbF;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = clsPmpaPb.GstrGbChild;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = "A";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = clsPmpaType.ARC.GbHost;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = "?";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = clsPmpaType.ARC.Amt1.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = clsPmpaType.ARC.Amt2.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = clsPmpaType.TIT.RoomCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "1";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.POWDER] = clsPmpaPb.GstrPowder;
                    #endregion
                    SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowCnt);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return false;
                    }
                }

                switch (clsPmpaType.ISG.Nu)
                {
                    case "01": clsPmpaType.TIT.Amt[1] += clsPmpaType.ARC.Amt1; break;
                    case "02": clsPmpaType.TIT.Amt[2] += clsPmpaType.ARC.Amt1; break;
                    case "03": clsPmpaType.TIT.Amt[3] += clsPmpaType.ARC.Amt1; break;
                    case "04": clsPmpaType.TIT.Amt[4] += clsPmpaType.ARC.Amt1; break;
                    case "05": clsPmpaType.TIT.Amt[5] += clsPmpaType.ARC.Amt1; break;
                    case "07": clsPmpaType.TIT.Amt[7] += clsPmpaType.ARC.Amt1; break;
                    case "16": clsPmpaType.TIT.Amt[16] += clsPmpaType.ARC.Amt1; break;
                    case "21": clsPmpaType.TIT.Amt[21] += clsPmpaType.ARC.Amt1; break;
                    case "34": clsPmpaType.TIT.Amt[34] += clsPmpaType.ARC.Amt1; break;
                    case "35": clsPmpaType.TIT.Amt[35] += clsPmpaType.ARC.Amt1; break;
                    default:
                        return false;
                        //break;
                }

                clsPmpaType.TIT.Amt[44] += clsPmpaType.ARC.Amt2;
                clsPmpaType.TIT.Amt[50] += clsPmpaType.ARC.Amt1 + clsPmpaType.ARC.Amt2;

                if (clsPmpaType.TIT.GbSpc == "") { clsPmpaType.TIT.GbSpc = "0"; }

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public bool Run_Arc_AcCode_Auto(PsmhDb pDbCon, string ArgPano, string ArgArcDate)
        {
            bool rtnVal = true;
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSuCode = string.Empty;
            
            try
            {
                if (string.Compare(clsPmpaType.TIT.OutDate, "2020-10-01") >= 0)
                {
                    strSuCode = "AC321";
                }
                else
                {
                    strSuCode = "AC421";
                }

             
                
                SQL = "";
                SQL += " SELECT SUM(Amt1+Amt2) AC421Amt                                ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                SQL += "  WHERE Pano = '" + ArgPano + "'                               ";
                SQL += "    AND BDate = TO_DATE('" + ArgArcDate + "','YYYY-MM-DD')     ";
                SQL += "    AND SuNext = '" + strSuCode + "'                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt64(VB.Val(dt.Rows[0]["AC421Amt"].ToString())) > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                }
                
                if (cPF.Ins_IpdSlip_SuCode(pDbCon, strSuCode, ArgArcDate) == false)
                { 
                    ComFunc.MsgBox(strSuCode + " 수가입력 오류!", "작업불가");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
        public bool Run_Arc_AHCode_Auto(PsmhDb pDbCon, string ArgPano, string ArgArcDate)  //감염예방관리료
        {
            bool rtnVal = true;
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSuCode = string.Empty;

            try
            {
                if ( string.Compare(clsPmpaType.TIT.OutDate, "2020-10-01") >= 0)
                {
                    strSuCode = "AH011";
                }
                else
                {
                    strSuCode = "AH013";
                }

             

                SQL = "";
                SQL += " SELECT SUM(Amt1+Amt2) AH013Amt                                ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                SQL += "  WHERE Pano = '" + ArgPano + "'                               ";
                SQL += "    AND BDate = TO_DATE('" + ArgArcDate + "','YYYY-MM-DD')     ";
                SQL += "    AND SuNext = '" + strSuCode + "'                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt64(VB.Val(dt.Rows[0]["AH013Amt"].ToString())) > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                }

                if (cPF.Ins_IpdSlip_SuCode(pDbCon, strSuCode, ArgArcDate) == false)
                {
                    ComFunc.MsgBox(strSuCode + " 수가입력 오류!", "작업불가");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
        public bool Run_Arc_AICode_Auto(PsmhDb pDbCon, string ArgPano, string ArgArcDate)  //야간간호료
        {
            bool rtnVal = true;
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSuCode = string.Empty;
            if (string.Compare(clsPmpaType.TIT.ArcDate, "2019-10-01") < 0) { return rtnVal; }
            try
            {
                strSuCode = "AI120";

                SQL = "";
                SQL += " SELECT SUM(Amt1+Amt2) AI120Amt                                ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                     ";
                SQL += "  WHERE Pano = '" + ArgPano + "'                               ";
                SQL += "    AND BDate = TO_DATE('" + ArgArcDate + "','YYYY-MM-DD')     ";
                SQL += "    AND SuNext = '" + strSuCode + "'                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt64(VB.Val(dt.Rows[0]["AI120Amt"].ToString())) > 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                }

                if (clsPmpaType.TIT.WardCode == "NR" || clsPmpaType.TIT.WardCode == "IQ" || clsPmpaType.TIT.WardCode == "33" || clsPmpaType.TIT.WardCode == "35" || clsPmpaType.TIT.WardCode == "4H" || clsPmpaType.TIT.WardCode == "40" || clsPmpaType.TIT.WardCode == "73" || clsPmpaType.TIT.WardCode == "75")

                {
                    return rtnVal;
                }
 
                if (cPF.Ins_IpdSlip_SuCode(pDbCon, strSuCode, ArgArcDate) == false)
                {
                    ComFunc.MsgBox(strSuCode + " 수가입력 오류!", "작업불가");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
    }
}
