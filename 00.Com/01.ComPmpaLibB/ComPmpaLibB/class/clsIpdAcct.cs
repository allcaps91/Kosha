using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsIpdAcct : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //수술실 처방입력용 Flag 2018.08.09 KMC
        public static string GstrOpFlag = "";
        public static string FstrCheck = "";        //JUSA_CHECK
        double FnQty = 0;
        int FnNal = 0;

        bool IsDataFlag = true;
        //clsBasAcct cBAcct = new clsBasAcct();
        //ComFunc CF = new ComFunc();
        int Count = 0;

        public string Acc_Proc_Main_Send(PsmhDb pDbCon, string ArgPtno, string ArgSex, int ArgAge, string ArgBi, string argGbSpc, string ArgWardCode, string ArgJumin1, string ArgJumin2, string ArgGbOcs, string argRoomCode, string ArgRowid, string ArgGBSELNOT, string ArgBDate)
        {
            string strOK = "OK";
            ComFunc CF = new ComFunc();

            try
            {
                #region Argument_Move
                clsPmpaType.IA.pano     = ArgPtno;
                clsPmpaType.IA.Bi       = Convert.ToInt16(ArgBi);
                clsPmpaType.IA.Sex      = ArgSex;
                clsPmpaType.IA.Age      = ArgAge;
                clsPmpaType.IA.Age2     = ComFunc.AgeCalcEx_Zero2(ArgJumin1 + ArgJumin2, ArgBDate);
                clsPmpaType.IA.AgeiLsu  = CF.DATE_ILSU(pDbCon, ArgBDate, "20" + VB.Left(ArgJumin1, 2) + "-" + VB.Mid(ArgJumin1, 3, 2) + "-" + VB.Right(ArgJumin1, 2));
                clsPmpaType.IA.Jumin1   = ArgJumin1;
                clsPmpaType.IA.Jumin2   = ArgJumin2;
                clsPmpaType.IA.GbSpc    = argGbSpc;
                clsPmpaType.IA.WardCode = ArgWardCode;
                clsPmpaType.IA.RoomCode = argRoomCode;
                clsPmpaType.IA.GBSelNot = ArgGBSELNOT;
                clsPmpaType.IA.Bi1      = Convert.ToInt16(clsPmpaType.IA.Bi.ToString().Substring(0, 1));
                clsPmpaType.IA.Date     = ArgBDate;

                if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55) { clsPmpaType.IA.Bi1 = 6; }

                clsPmpaType.IA.Fee6     = clsPmpaPb.GnFee6;

                if (clsPmpaType.IA.Fee6 < 0) { clsPmpaType.IA.Fee6 = 0; }
                #endregion

                Area_Clear_Send();

                clsPmpaType.IA.KTASLEVEL = clsPmpaPb.GstrErKTAS;

                if (Account_Check_Send(pDbCon, ArgRowid) == false)
                {
                    return "";
                }

                if (READ_TRSNO(pDbCon) == false) { return ""; }

                if (Account_Process_Send(pDbCon) == false)
                {
                    return "";
                }

                if (Last_Ipd_Note_Send(pDbCon) == false)     //복약지도료,주사 수기료 Insert
                {
                    return "";
                }

                if (Last_Ipd_Slip(pDbCon) == false)     //재원 Slip Insert
                {
                    return "";
                }

                // 재원 MASTER 누적 제외시킴 여러 로직에서 중복실행되고 있음.
                //if (Last_Ipd_Mast(pDbCon) == false)     //재원MASTER 금액 누적  Update
                //{
                //    return "";
                //}

                return strOK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
            
        }

        bool Last_Ipd_Note_Send(PsmhDb pDbCon)
        {
            //입원 복약지도료, 근육주사 수기료, 정맥주사 수기료 (IPD_NOTE TABLE)
            bool rtnVal = true;

            int i = 0;
            string strConvDate = string.Empty;
            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            try
            {
                ComFunc CF = new ComFunc();

                if (clsPmpaType.IA.DisCharge == false)
                {
                    for (i = 0; i < 60; i++)
                    {
                        if (clsPmpaPb.G7QTY11[i] == 0 && clsPmpaPb.G7QTY20A[i] == 0 && clsPmpaPb.G7QTY20B[i] == 0 && clsPmpaPb.G7AL201[i] == 0 && clsPmpaPb.G7AL010[i] == 0)
                            break;

                        strConvDate = CF.DATE_ADD(pDbCon, clsPmpaType.IA.Date, i);

                        SQL = "";
                        SQL += " SELECT * FROM " + ComNum.DB_PMPA + "IPD_NOTE                \r\n";
                        SQL += "  WHERE  Pano = '" + clsPmpaType.IA.pano + "'                \r\n";
                        SQL += "    AND  Bdate = TO_DATE('" + strConvDate + "','YYYY-MM-DD') \r\n";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            IsDataFlag = false;
                            return false;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_NOTE                       \r\n";
                            SQL += "   SET Fee1 = Fee1 + " + clsPmpaPb.G7QTY20A[i] + ",         \r\n";
                            SQL += "       Fee2 = Fee2 + " + clsPmpaPb.G7QTY20B[i] + ",         \r\n";
                            SQL += "       Fee4 = Fee4 + " + clsPmpaPb.G7QTY11[i] + ",          \r\n";
                            SQL += "       AL010 = nvl(AL010,0) + " + clsPmpaPb.G7AL010[i] + ",          \r\n";
                            SQL += "       AL201 = AL201 + " + clsPmpaPb.G7AL201[i] + "         \r\n";
                            SQL += " WHERE Pano = '" + clsPmpaType.IA.pano + "'                 \r\n";
                            SQL += "   AND Bdate = TO_DATE('" + strConvDate + "','YYYY-MM-DD')  \r\n";
                        }
                        else
                        {
                            SQL = "";
                            SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NOTE (Pano,Bdate,Fee1,Fee2,Fee4,AL010,AL201) VALUES (";
                            SQL += " '" + clsPmpaType.IA.pano + "', ";
                            SQL += " TO_DATE('" + strConvDate + "','YYYY-MM-DD'), ";
                            SQL += " " + clsPmpaPb.G7QTY20A[i] + ", ";
                            SQL += " " + clsPmpaPb.G7QTY20B[i] + ", ";
                            SQL += " " + clsPmpaPb.G7QTY11[i] + ", ";
                            SQL += " " + clsPmpaPb.G7AL010[i] + ", ";
                            SQL += " " + clsPmpaPb.G7AL201[i] + ") ";
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            IsDataFlag = false;
                            return false;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                IsDataFlag = false;
                return false;
            }
        }

        public void Area_Clear_Send()
        {
            clsPmpaPb.G7SMAcount = 0;
            clsPmpaPb.G7WRTcount = 0;

            clsPmpaPb.GstatQmgrIPD = "";
            clsPmpaPb.GstatQmgrBAS = "";
            clsPmpaPb.GstrB20STAT = "";
            clsPmpaPb.GstrB11STAT = "";
            clsPmpaPb.GstrB65STAT = "";

            clsPmpaType.IA.KTASLEVEL = "";
            clsPmpaType.IA.ErJDate = "";
            clsPmpaType.IA.SlipCount = 0;

            clsPmpaType.ISA = new clsPmpaType.Slip_Accept_Table_IPD[1];
            clsPmpaType.ISW = new clsPmpaType.Slip_Write_Table_IPD[1];
           
            clsPmpaType.IA.Amt  = new long[61];
            clsPmpaPb.G7QTY11 = new double[61];
            clsPmpaPb.G7QTY20A = new double[61];
            clsPmpaPb.G7QTY20B = new double[61];
            clsPmpaPb.G7AL201 = new int[61];
            clsPmpaPb.G7AL010 = new double[61];

            Array.Clear(clsPmpaType.ISA, 0,     clsPmpaType.ISA.Length);
            Array.Clear(clsPmpaType.ISW, 0,     clsPmpaType.ISW.Length);
            
        }

        public bool Account_Check_Send(PsmhDb pDbCon, string ArgRowid)
        {
            DataTable Dt            = new DataTable();
            DataTable Dt2           = new DataTable();
            string SQL              = string.Empty;
            string SqlErr           = string.Empty;

            string strSlipNo        = string.Empty;
            string strOrderCode     = string.Empty;
            string strGbInfo        = string.Empty;
            string strBDATE         = string.Empty;
            string strInDate        = string.Empty;
            string strInTime        = string.Empty;
            string strGbioe         = string.Empty;
            string strBi            = string.Empty;
            string strGbInfo2       = string.Empty;
            string strER_39Chk      = string.Empty;
            string strErKTASChk     = string.Empty;
            string strErV4300       = string.Empty;
            string strOK_2          = string.Empty;
            string strDayChk        = string.Empty;
            string strSuCode        = string.Empty;
            string strDept          = string.Empty;
            string strSuCode_Temp   = string.Empty;

            int nDays = 0, nSubRead = 0, nCNT = 0;

            bool rtnVal = true;
            int i = 0, j = 0, OCNT = -1, nREAD = 0;

            clsIuSent cISNT = new clsIuSent();
            ComFunc CF = new ComFunc();

            clsOrdFunction OF = new clsOrdFunction();

            try
            {
                Dt = cISNT.sel_IOrder_AccCheck(pDbCon, ArgRowid, clsPmpaType.IA.pano, clsPmpaPb.GstrBdate);

                if (Dt == null)
                {
                    return false;
                }
                else
                {
                    nREAD = Dt.Rows.Count;

                    if (nREAD > 0)
                    {
                        for (i = 0; i < nREAD; i++)
                        {
                            strSlipNo       = Dt.Rows[i]["SlipNo"].ToString().Trim();
                            strOrderCode    = Dt.Rows[i]["OrderCode"].ToString().Trim();
                            strGbInfo       = Dt.Rows[i]["GbInfo"].ToString().Trim();
                            strBDATE        = Dt.Rows[i]["BDate"].ToString().Trim();
                            strGbioe        = Dt.Rows[i]["GBioe"].ToString().Trim();    //2013-06-21
                            strBi           = Dt.Rows[i]["BI"].ToString().Trim();
                            strInDate       = clsPmpaPb.GstrInDate39;                   //2013-06-21
                            strInTime       = clsPmpaPb.GstrInTime39;                   //2013-06-21

                            nSubRead        = 0;

                            strGbInfo2      = "";
                            strER_39Chk     = "";
                            strErKTASChk    = "";
                            strErV4300      = "";
                            strOK_2         = "OK";
                            strSuCode_Temp  = "";

                            //2014-01-04 응급실 입원대상자만 GBIOE = EI- D오더 체크
                            if (Dt.Rows[i]["ORDERSITE"].ToString().Trim() == "ERO" && Dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D-" && Dt.Rows[i]["GBIOE"].ToString().Trim() == "EI")
                            {
                                #region //slip발생건 없으면 전송안함 strOK_2 = ""
                                SQL = "";
                                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                                SQL += "  WHERE PANO  = '" + Dt.Rows[i]["Ptno"].ToString().Trim() + "' ";
                                SQL += "    AND ORDERNO = " + VB.Val(Dt.Rows[i]["OrderNo"].ToString()) + " ";
                                SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return false;
                                }

                                if (Dt2.Rows.Count == 0)
                                {
                                    strOK_2 = "";
                                }

                                Dt2.Dispose();
                                Dt2 = null;
                                #endregion

                                #region //오더에 D +처방 있으면 전송하게함 2014-01-14 strOK_2 = "OK"
                                SQL = "";
                                SQL += " SELECT ROWID FROM " + ComNum.DB_MED + "OCS_iorder ";
                                SQL += "  WHERE PtNO  = '" + Dt.Rows[i]["Ptno"].ToString().Trim() + "'";
                                SQL += "    AND ORDERNO = " + VB.Val(Dt.Rows[i]["OrderNo"].ToString()) + " ";
                                SQL += "    AND Gbioe ='EI' ";
                                SQL += "    AND gbstatus ='D' ";
                                SQL += "    AND ACCSEND ='Y' ";
                                SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return false;
                                }
                                if (strOK_2 !="" && Dt2.Rows.Count > 0)
                                {
                                    strOK_2 = "OK";
                                }

                                Dt2.Dispose();
                                Dt2 = null;
                                #endregion
                            }

                            //2014-01-04
                            #region Main For Loop
                            if (strOK_2 == "OK")
                            {
                                if (strOrderCode == "$$39" && strGbioe == "EI" && VB.Left(strGbInfo, 2) != "")
                                {
                                    #region //2013-06-21 응급실입원후 $$39오더발생시
                                    strDayChk = "0"; //평일

                                    #region //2015-12-30
                                    if (string.Compare(strInDate, "2016-01-01") >= 0 && (clsPmpaType.IA.KTASLEVEL == "1" || clsPmpaType.IA.KTASLEVEL == "2" || clsPmpaType.IA.KTASLEVEL == "3"))
                                    {
                                        strErKTASChk = "Y";

                                        //slip 체크
                                        SQL = "";
                                        SQL += " SELECT SUM(AMT1+AMT2) SAMT FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                                        SQL += "  WHERE PANO  = '" + clsPmpaType.IA.pano + "' ";
                                        SQL += "    AND BDate >=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                                        SQL += "    AND BDate <=TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                                        if (string.Compare(strInDate, clsPmpaPb.GstrGlobalErFdate) >= 0)
                                        {
                                            SQL += "   AND SuCode IN ('V4203','V3203','AH3B3')  ";
                                        }
                                        else
                                        {
                                            SQL += "   AND SuCode ='V4300'               ";
                                        }
                                        SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return false;
                                        }

                                        if (VB.Val(Dt2.Rows[0]["SAMT"].ToString()) > 0)
                                        {
                                            strErV4300 = "N";
                                        }

                                        Dt2.Dispose();
                                        Dt2 = null;
                                    }
                                    #endregion

                                    if (string.Compare(strInTime, "18:00") >= 0 && string.Compare(strInTime, "23:59") <= 0) { strDayChk = "1"; }
                                    if (string.Compare(strInTime, "00:01") >= 0 && string.Compare(strInTime, "07:00") <= 0) { strDayChk = "1"; }

                                    if (CF.DATE_HUIL_CHECK(pDbCon, strInDate) == true) { strDayChk = "2"; } //휴일
                                    if (clsPublic.GstrTempHoliday == "*") { strDayChk = "2"; } //휴일 2015-09-25

                                    strER_39Chk = "Y";

                                    strGbInfo2 = strGbInfo;
                                    strGbInfo = "";

                                    strDept = VB.Left(strGbInfo2, 2);

                                    if (strErKTASChk == "Y")
                                    {
                                        #region 응급중증 대상자 관리료
                                        if (string.Compare(strInDate, clsPublic.GstrZoneEmergencyStartDate) >= 0)
                                        {
                                            strOrderCode = "V2200";
                                            strSuCode = "V2200";
                                        }
                                        else
                                        {
                                            strOrderCode = "V2300";
                                            strSuCode = "V2300";
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 일수에 따라 진찰료코드 산정
                                        SQL = "";
                                        SQL += " SELECT TO_CHAR(LastDate,'YYYY-MM-DD') LastDate ";
                                        SQL += "   FROM " + ComNum.DB_PMPA + "BAS_LASTEXAM ";
                                        SQL += "  WHERE Pano = '" + clsPmpaType.IA.pano + "' ";
                                        SQL += "    AND DeptCode = '" + strDept + "' ";
                                        SQL += "  ORDER BY LastDate DESC ";
                                        SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, clsDB.DbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return false;
                                        }
                                        if (Dt2.Rows.Count > 0)
                                        {
                                            nDays = CF.DATE_ILSU(pDbCon, clsPublic.GstrSysDate, Dt2.Rows[0]["LastDate"].ToString().Trim());

                                            if (nDays > 90)
                                            {
                                                if (strDayChk == "2")        //초진공휴
                                                {
                                                    strOrderCode = "AA1762";
                                                    strSuCode = "AA1762";
                                                }
                                                else if (strDayChk == "1")    //초진야간
                                                {
                                                    strOrderCode = "AA1761";
                                                    strSuCode = "AA1761";
                                                }
                                                else                           //초진일반
                                                {
                                                    strOrderCode = "AA176";
                                                    strSuCode = "AA176";
                                                }
                                            }
                                            else
                                            {
                                                if (strDayChk == "2")        //재진공휴
                                                {
                                                    strOrderCode = "AA2762";
                                                    strSuCode = "AA2762";
                                                }
                                                else if (strDayChk == "1")    //재진야간
                                                {
                                                    strOrderCode = "AA2761";
                                                    strSuCode = "AA2761";
                                                }
                                                else                            //재진일반
                                                {
                                                    strOrderCode = "AA276";
                                                    strSuCode = "AA276";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (strDayChk == "2")       //초진공휴
                                            {
                                                strOrderCode = "AA1762";
                                                strSuCode = "AA1762";
                                            }
                                            else if (strDayChk == "1")    //초진야간
                                            {
                                                strOrderCode = "AA1761";
                                                strSuCode = "AA1761";
                                            }
                                            else                            //초진일반
                                            {
                                                strOrderCode = "AA176";
                                                strSuCode = "AA176";
                                            }
                                        }
                                        Dt2.Dispose();
                                        Dt2 = null;
                                        #endregion
                                    }
                                    #endregion
                                }

                                if (strER_39Chk == "Y")
                                {
                                    #region 타과진찰료 
                                    nCNT = 0;
                                    // 2016-04-05 09:00 보험심사과장과 업무정리함
                                    //1) $$39 + KTAS 1~3
                                    //   ※발생수가 : V2300, AU214, AU312, V4300
                                    //   ※참고사항 : V4300 은 1회만 발생
                                    //2) $$39 + KTAS 4~5
                                    //   ※발생수가 : 기본진찰료, AU214, AU312
                                    //3) 참고사항
                                    //    2016-03-31 12:14 이현정수녀님과 통화함.
                                    //    의료급여이면 'V2300, AU214, AU312, V4300 모두 안받는다.

                                    //nCNT = 3;
                                    nCNT = 4;           //2018-09-14 add
                                    //  0   ,   1  ,   2  ,   3
                                    //V2300 , AU214, AU312, V4300 수가
                                    if (clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22)
                                    {
                                        //의료급여이면 질평가지원금(AU214, AU312), 관찰료(V4300) 발생안함
                                        nCNT = 0;
                                    }
                                    else
                                    {
                                        if (strErKTASChk == "Y")
                                        {
                                            if (strErV4300 == "N")
                                                //nCNT = 2;
                                                nCNT = 3;           //'2018-09-14
                                            else
                                                //nCNT = 3;
                                                nCNT = 4;           //'2018-09-14
                                        }
                                        else
                                        {
                                            //nCNT = 2;
                                            nCNT = 3;               //'2018-09-14
                                        }
                                    }
                                    #endregion

                                    for (j = 0; j <= nCNT; j++)
                                    {
                                        #region 진찰료 Suga Setting
                                        if (string.Compare(strInDate, clsPublic.GstrZoneEmergencyStartDate) >= 0)
                                        {
                                            switch (j)
                                            {
                                                case 1:
                                                    strOrderCode = "AU214";
                                                    strSuCode = "AU214";
                                                    break;
                                                case 2:
                                                    strOrderCode = "AU312";
                                                    strSuCode = "AU312";
                                                    break;
                                                case 3:
                                                    strOrderCode = "AU413";     //''2018-09-14 add
                                                    strSuCode = "AU413";
                                                    break;
                                                case 4:
                                                    strOrderCode = "V4203";
                                                    strSuCode = "V4203";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (j)
                                            {
                                                case 1:
                                                    strOrderCode = "AU214";
                                                    strSuCode = "AU214";
                                                    break;
                                                case 2:
                                                    strOrderCode = "AU312";
                                                    strSuCode = "AU312";
                                                    break;
                                                case 3:
                                                    strOrderCode = "V4300";
                                                    strSuCode = "V4300";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        #endregion

                                        #region For Loop
                                        OCNT += 1;
                                        if (clsPmpaType.ISA.Length < OCNT + 1) { Array.Resize(ref clsPmpaType.ISA, OCNT + 1); }
                                        clsPmpaType.ISA[OCNT].SlipNo = strSlipNo;
                                        clsPmpaType.ISA[OCNT].OrderCode = strOrderCode;
                                        clsPmpaType.ISA[OCNT].GbInfo = strGbInfo;
                                        clsPmpaType.ISA[OCNT].DeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();

                                        if (VB.Right(Dt.Rows[i]["DrCode"].ToString().Trim(), 2) == "99" && Dt.Rows[i]["DrCode"].ToString().Trim() != "")
                                        {
                                            clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode2"].ToString().Trim();
                                        }
                                        else
                                        {
                                            clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode"].ToString().Trim();
                                        }
                                        //2013-06-21
                                        if (strGbInfo2 != "" && strER_39Chk == "Y")
                                        {
                                            clsPmpaType.ISA[OCNT].Sucode = strSuCode;
                                            clsPmpaType.ISA[OCNT].GbInfo = strGbInfo2;
                                        }
                                        //소스 누락 2018-10-29 add
                                        clsPmpaType.ISA[OCNT].RealQty = VB.Val(Dt.Rows[i]["RealQty"].ToString());
                                        clsPmpaType.ISA[OCNT].Qty = VB.Val(Dt.Rows[i]["Qty"].ToString());
                                        clsPmpaType.ISA[OCNT].Nal = (int)VB.Val(Dt.Rows[i]["Nal"].ToString());
                                        clsPmpaType.ISA[OCNT].Div = (int)VB.Val(Dt.Rows[i]["GBDIV"].ToString());
                                        clsPmpaType.ISA[OCNT].DosCode = Dt.Rows[i]["DosCode"].ToString().Trim();

                                        

                                        //'2016-08-30
                                        if (string.Compare(strBDATE, "2016-09-01") >= 0)
                                        {
                                            if (Check_Change_GbSelf2(pDbCon, strOrderCode, strBi, strBDATE))
                                            {
                                                clsPmpaType.ISA[OCNT].GbSelfSource = clsPmpaType.ISA[OCNT].GbSelf;
                                                if (clsPmpaType.ISA[OCNT].GbSelfSource.Trim() == "") { clsPmpaType.ISA[OCNT].GbSelfSource = READ_SUGBF(pDbCon, strOrderCode); }
                                                clsPmpaType.ISA[OCNT].GbSelf = "0";
                                            }
                                        }

                                        clsPmpaType.ISA[OCNT].GbTFlag = Dt.Rows[i]["GbTFlag"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GbNgt = Dt.Rows[i]["GbNgt"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GbNgt2 = "";

                                        #region 주석처리 야간체크 추가
                                        //'2012-10-08 수납없이 응급실 입원수속시 - 야간체크추가
                                        //If Trim(AdoGetString(AdoRs, "SugbC", i)) = "1" And Trim(AdoGetString(AdoRs, "SugbE", i)) = "1" Then
                                        //    If Trim(OST.GbNgt) = "" And Trim(AdoGetString(AdoRs, "GbNgt2", i)) <> "" And Trim(AdoGetString(AdoRs, "GbIoe", i)) = "EI" Then
                                        //        'OST.GbNgt = Trim(AdoGetString(AdoRs, "GbNgt2", i))
                                        //        'OST.GbNgt2 = Trim(AdoGetString(AdoRs, "GbNgt2", i))  '2012 - 10 - 11
                                        //    End If
                                        //End If
                                        #endregion

                                        //2013-05-30
                                        if (clsPmpaType.ISA[OCNT].GbNgt == "1" || clsPmpaType.ISA[OCNT].GbNgt == "2")
                                        {
                                            if (Dt.Rows[i]["GBIOE"].ToString().Trim() == "EI" && Dt.Rows[i]["SugbC"].ToString().Trim() != "1")
                                            {
                                                clsPmpaType.ISA[OCNT].GbNgt = "0";  //야간가산부분 - 가산없으면 0
                                            }
                                            else if (Dt.Rows[i]["GBIOE"].ToString().Trim() == "EI" && Dt.Rows[i]["SugbC"].ToString().Trim() == "1")
                                            {
                                                //2013-06-07
                                                if (clsPmpaType.ISA[OCNT].GbNgt == "1")
                                                {
                                                    clsPmpaType.ISA[OCNT].GbNgt = "2";
                                                }
                                                else if (clsPmpaType.ISA[OCNT].GbNgt == "2")
                                                {
                                                    clsPmpaType.ISA[OCNT].GbNgt = "1";
                                                }
                                            }
                                        }

                                        clsPmpaType.ISA[OCNT].Bun = Dt.Rows[i]["Bun"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].Nu = Dt.Rows[i]["Nu"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbB = Dt.Rows[i]["SugbB"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbD = Dt.Rows[i]["SugbD"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbE = Dt.Rows[i]["SugbE"].ToString().Trim();  //2013-06-17
                                        clsPmpaType.ISA[OCNT].SugbF = Dt.Rows[i]["SugbF"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbI = Dt.Rows[i]["SugbI"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].TotMax = Convert.ToInt32(VB.Val(Dt.Rows[i]["TotMax"].ToString()));
                                        clsPmpaType.ISA[OCNT].OrderNo = Convert.ToInt64(VB.Val(Dt.Rows[i]["OrderNo"].ToString()));
                                        clsPmpaType.ISA[OCNT].RoomCode = Convert.ToInt32(VB.Val(Dt.Rows[i]["RoomCode"].ToString()));
                                        clsPmpaType.ISA[OCNT].GbSelNot = Dt.Rows[i]["ORDERSITE"].ToString().Trim() == "ERO" ? "1" : "";
                                        clsPmpaType.ISA[OCNT].CONSULT = Dt.Rows[i]["CONSULT"].ToString().Trim();
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (string.Compare(strSlipNo, "0010") >= 0 && string.Compare(strSlipNo, "0050") <= 0 && strGbInfo != "")
                                    {
                                        #region //처방종류가 검사이고 추가정보가 있으면  (동일 연속감사)
                                        //2021-07-28 매핑수가 적용
                                        SQL = "";
                                        SQL += " SELECT A.SuCode SuCode1,B.Bun Bun1,B.Nu Nu1,                                        ";
                                        SQL += "        B.SugbB SugbB1,B.SugbC SugbC1,B.SugbD SugbD1,B.SugbE SugbE1,B.TotMax TotMax1,";
                                        SQL += "        B.SugbI SugbI1,B.SugbF SugbF1,B.SugbG SugbG1                                 ";
                                        SQL += "   FROM " + ComNum.DB_MED + "OCS_SUBCODE A,                                          ";
                                        SQL += "        " + ComNum.DB_PMPA + "BAS_SUT B                                              ";
                                        SQL += "  WHERE A.OrderCode = '" + strOrderCode + "'                                         ";
                                        SQL += "    AND A.SubName = '" + strGbInfo + "'                                              ";
                                        SQL += "    AND A.SuCode  = B.SuCode                                                         ";
                                        SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);
                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            return false;
                                        }

                                        nSubRead = Dt2.Rows.Count;

                                        #endregion
                                        // 2000.12.31 수가코드를 약속처방,IORDER의 내역을 사용하지 않고
                                        //            오더판넬에 등록된 현재의 수가코드를 사용하여 IPD_SLIP에 전송함
                                    }
                                    else
                                    {
                                        #region ORDERCODE , 조회
                                        if (Dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D-" || Dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D+")
                                        {
                                            nSubRead = 0;
                                        }
                                        else
                                        {
                                            //2013-06-21
                                            if (strGbInfo2 != "" && strER_39Chk == "Y")
                                            {
                                                nSubRead = 0;
                                            }
                                            else if (strGbInfo == "")
                                            {
                                                //2021-07-28 매핑수가 적용
                                                SQL = "";
                                                SQL += " SELECT A.SuCode SuCode1,B.Bun Bun1,B.Nu Nu1,                                        ";
                                                SQL += "        B.SugbB SugbB1,B.SugbC SugbC1,B.SugbD SugbD1,B.SugbE SugbE1,B.TotMax TotMax1,";
                                                SQL += "        B.SugbI SugbI1,B.SugbF SugbF1,B.SugbG SugbG1                                 ";
                                                SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE A,                                        ";
                                                SQL += "        " + ComNum.DB_PMPA + "BAS_SUT B                                              ";
                                                SQL += "  WHERE A.OrderCode = '" + strOrderCode + "'                                         ";
                                                SQL += "    AND A.SuCode  = B.SuCode                                                         ";
                                                SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);

                                                if (SqlErr != "")
                                                {
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                    return false;
                                                }

                                                nSubRead = Dt2.Rows.Count;

                                            }
                                            else
                                            {
                                                //2021-07-28 매핑수가 적용
                                                SQL = "";
                                                SQL += " SELECT A.SuCode SuCode1,B.Bun Bun1,B.Nu Nu1,                                        ";
                                                SQL += "        B.SugbB SugbB1,B.SugbC SugbC1,B.SugbD SugbD1,B.SugbE SugbE1,B.TotMax TotMax1,";
                                                SQL += "        B.SugbI SugbI1,B.SugbF SugbF1,B.SugbG SugbG1                                 ";
                                                SQL += "   FROM " + ComNum.DB_MED + "OCS_SUBCODE A,                                          ";
                                                SQL += "        " + ComNum.DB_PMPA + "BAS_SUT B                                              ";
                                                SQL += "  WHERE A.OrderCode = '" + strOrderCode + "'                                         ";
                                                SQL += "    AND A.SubName = '" + strGbInfo + "'                                              ";
                                                SQL += "    AND A.SuCode  = B.SuCode                                                         ";
                                                SqlErr = clsDB.GetDataTableEx(ref Dt2, SQL, pDbCon);

                                                if (SqlErr != "")
                                                {
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                    return false;
                                                }

                                                nSubRead = Dt2.Rows.Count;

                                            }
                                        }
                                        #endregion
                                    }

                                    if (nSubRead > 0)
                                    {
                                        for (j = 0; j < nSubRead; j++)
                                        {
                                            #region Move Order To ISA
                                            OCNT += 1;
                                            if (clsPmpaType.ISA.Length < OCNT + 1) { Array.Resize(ref clsPmpaType.ISA, OCNT + 1); }

                                            clsPmpaType.ISA[OCNT].SlipNo = strSlipNo;
                                            clsPmpaType.ISA[OCNT].OrderCode = strOrderCode;
                                            clsPmpaType.ISA[OCNT].GbInfo = strGbInfo;
                                            clsPmpaType.ISA[OCNT].DeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();

                                            //2013-01-04
                                            if (VB.Right(Dt.Rows[i]["DrCode"].ToString().Trim(), 2) == "99" && Dt.Rows[i]["DrCode2"].ToString().Trim() != "")
                                            {
                                                clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode2"].ToString().Trim();
                                            }
                                            else
                                            {
                                                clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode"].ToString().Trim();
                                            }

                                            clsPmpaType.ISA[OCNT].RealQty = VB.Val(Dt.Rows[i]["RealQty"].ToString());
                                            clsPmpaType.ISA[OCNT].Qty = VB.Val(Dt.Rows[i]["Qty"].ToString());
                                            clsPmpaType.ISA[OCNT].Nal = (int)VB.Val(Dt.Rows[i]["Nal"].ToString());
                                            clsPmpaType.ISA[OCNT].Div = (int)VB.Val(Dt.Rows[i]["GBDIV"].ToString());
                                            clsPmpaType.ISA[OCNT].DosCode = Dt.Rows[i]["DosCode"].ToString().Trim();

                                            strSuCode_Temp = "";

                                            if (Dt2.Rows.Count > 0)
                                            {
                                                strSuCode_Temp = Dt2.Rows[j]["SuCode1"].ToString().Trim();
                                                //if (clsType.User.Sabun == "21403")
                                                if (CF.Read_Bcode_Name(clsDB.DbCon, "SUNAP_수가매핑시행여부", "USE") == "Y")
                                                {
                                                    strSuCode_Temp =  OF.Mapping_SuCode(clsDB.DbCon, strOrderCode, Dt2.Rows[j]["SuCode1"].ToString().Trim(), 
                                                                                    strGbInfo, strBDATE, Dt.Rows[i]["DeptCode"].ToString().Trim());
                                                }

                                            }

                                            switch (strSuCode_Temp)
                                            {
                                                case "PT-FT25":
                                                case "PT-FT50":
                                                    clsPmpaType.ISA[OCNT].GbSelf = "2"; //2002/09/07 jjy 삼사계요청
                                                    break;
                                                default:
                                                    clsPmpaType.ISA[OCNT].GbSelf = Dt.Rows[i]["GbSelf"].ToString().Trim(); //2002/09/07 jjy 삼사계요청
                                                    break;
                                            }

                                            //2016-08-30
                                            if (string.Compare(strBDATE, "2016-09-01") >= 0)
                                            {
                                                if (Check_Change_GbSelf2(pDbCon, strOrderCode, strBi, strBDATE))
                                                {
                                                    clsPmpaType.ISA[OCNT].GbSelfSource = clsPmpaType.ISA[OCNT].GbSelf;
                                                    if (clsPmpaType.ISA[OCNT].GbSelfSource.Trim() == "") { clsPmpaType.ISA[OCNT].GbSelfSource = READ_SUGBF(pDbCon, strOrderCode); }
                                                    clsPmpaType.ISA[OCNT].GbSelf = "0";
                                                }
                                            }

                                            clsPmpaType.ISA[OCNT].GbTFlag = Dt.Rows[i]["GbTFlag"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].GbNgt = Dt.Rows[i]["GbNgt"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].GbNgt2 = "";
                                            clsPmpaType.ISA[OCNT].GbHighRisk = Dt.Rows[i]["HIGHRISK"].ToString().Trim();
                                            if (clsPmpaType.ISA[OCNT].GbHighRisk != "" && clsPmpaType.ISA[OCNT].GbHighRisk != null && clsPmpaType.ISA[OCNT].GbHighRisk != "00")

                                            {
                                                clsPmpaType.ISA[OCNT].GbHighRisk = "1";
                                            }
                                            else
                                            {
                                                clsPmpaType.ISA[OCNT].GbHighRisk = "";
                                            }
                                            clsPmpaType.ISA[OCNT].GbER24H = Dt.Rows[i]["ER24"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].GbGSADD = Dt.Rows[i]["GSADD"].ToString().Trim();

                                            #region 주석처리
                                            //'2012-10-08 수납없이 응급실 입원수속시 - 야간체크추가
                                            //If Trim(AdoGetString(rs2, "SugbC1", j)) = "1" And Trim(AdoGetString(rs2, "SugbE1", j)) = "1" Then
                                            //    If Trim(clsPmpaType.ISA[OCNT].GbNgt) = "" And Trim(AdoGetString(AdoRs, "GbNgt2", i)) <> "" And Trim(AdoGetString(AdoRs, "GbIoe", i)) = "EI" Then
                                            //        'clsPmpaType.ISA[OCNT].GbNgt = Trim(AdoGetString(AdoRs, "GbNgt2", i))
                                            //        'clsPmpaType.ISA[OCNT].GbNgt2 = Trim(AdoGetString(AdoRs, "GbNgt2", i)) '2012 - 10 - 11
                                            //    End If
                                            //End If
                                            #endregion

                                            //2013-05-30
                                            if (clsPmpaType.ISA[OCNT].GbNgt == "1" || clsPmpaType.ISA[OCNT].GbNgt == "2")
                                            {
                                                if (Dt.Rows[i]["GbIoe"].ToString().Trim() == "EI" && Dt2.Rows[j]["SugbC1"].ToString().Trim() != "1")
                                                {
                                                    clsPmpaType.ISA[OCNT].GbNgt = "0";  //야간가산부분 - 가산없으면 0
                                                }
                                                else if (Dt.Rows[i]["GbIoe"].ToString().Trim() == "EI" && Dt2.Rows[j]["SugbC1"].ToString().Trim() == "1")
                                                {
                                                    //2013-06-07
                                                    if (clsPmpaType.ISA[OCNT].GbNgt == "1")
                                                        clsPmpaType.ISA[OCNT].GbNgt = "2";
                                                    else if (clsPmpaType.ISA[OCNT].GbNgt == "2")
                                                        clsPmpaType.ISA[OCNT].GbNgt = "1";
                                                }
                                                else if (Dt.Rows[i]["BUN"].ToString().Trim() == "35" && Dt2.Rows[j]["SugbC1"].ToString().Trim() == "1")
                                                {
                                                    //2013-06-07
                                                    if (clsPmpaType.ISA[OCNT].GbNgt == "1")
                                                        clsPmpaType.ISA[OCNT].GbNgt = "2";
                                                    else if (clsPmpaType.ISA[OCNT].GbNgt == "2")
                                                        clsPmpaType.ISA[OCNT].GbNgt = "1";
                                                }
                                            }

                                            clsPmpaType.ISA[OCNT].Sucode = strSuCode_Temp;//Dt2.Rows[j]["SuCode1"].ToString().Trim();

                                            //2013-06-21
                                            if (strGbInfo2 != "" && strER_39Chk == "Y")
                                            {
                                                clsPmpaType.ISA[OCNT].Sucode = strSuCode;
                                                clsPmpaType.ISA[OCNT].GbInfo = strGbInfo2;
                                            }


                                            clsPmpaType.ISA[OCNT].Bun = Dt2.Rows[j]["Bun1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].Nu = Dt2.Rows[j]["Nu1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbB = Dt2.Rows[j]["SugbB1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbD = Dt2.Rows[j]["SugbD1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbE = Dt2.Rows[j]["SugbE1"].ToString().Trim();   //2013-06-17                                        
                                            clsPmpaType.ISA[OCNT].SugbF = Dt2.Rows[j]["SugbF1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbI = Dt2.Rows[j]["SugbI1"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].TotMax = Convert.ToInt32(VB.Val(Dt2.Rows[j]["TotMax1"].ToString()));
                                            clsPmpaType.ISA[OCNT].OrderNo = Convert.ToInt64(VB.Val(Dt.Rows[i]["OrderNo"].ToString()));
                                            clsPmpaType.ISA[OCNT].RoomCode = Convert.ToInt32(VB.Val(Dt.Rows[i]["RoomCode"].ToString()));
                                            clsPmpaType.ISA[OCNT].GbSelNot = Dt.Rows[i]["ORDERSITE"].ToString().Trim() == "ERO" ? "1" : "";
                                            clsPmpaType.ISA[OCNT].CONSULT = Dt.Rows[i]["CONSULT"].ToString().Trim();

                                            //2015-12-30
                                            clsPmpaType.ISA[OCNT].GbErChk = "";

                                            if (strGbioe == "EI")
                                                clsPmpaType.ISA[OCNT].GbErChk = "Y";

                                            clsPmpaType.ISA[OCNT].BURN = Dt.Rows[i]["BURNADD"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].OPGBN = Dt.Rows[i]["OPGUBUN"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbAA = Dt.Rows[i]["SugbAA"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbAB = Dt.Rows[i]["SugbAB"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbAC = Dt.Rows[i]["SugbAC"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].SugbAD = Dt.Rows[i]["SugbAD"].ToString().Trim();
                                            clsPmpaType.ISA[OCNT].GBNS = Dt.Rows[i]["GBNS"].ToString().Trim();
                                            #endregion
                                        }
                                        Dt2.Dispose();
                                        Dt2 = null;
                                    }
                                    else
                                    {
                                        #region 동일연속검사가 없을 경우
                                        OCNT += 1;
                                        if (clsPmpaType.ISA.Length < OCNT + 1) { Array.Resize(ref clsPmpaType.ISA, OCNT + 1); }
                                        clsPmpaType.ISA[OCNT].SlipNo = strSlipNo;
                                        clsPmpaType.ISA[OCNT].OrderCode = strOrderCode;
                                        clsPmpaType.ISA[OCNT].GbInfo = strGbInfo;
                                        clsPmpaType.ISA[OCNT].DeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();

                                        //2013-01-04
                                        if (VB.Right(Dt.Rows[i]["DrCode"].ToString().Trim(), 2) == "99" && Dt.Rows[i]["DrCode2"].ToString().Trim() != "")
                                            clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode2"].ToString().Trim();
                                        else
                                            clsPmpaType.ISA[OCNT].DrCode = Dt.Rows[i]["DrCode"].ToString().Trim();

                                        clsPmpaType.ISA[OCNT].Sucode = Dt.Rows[i]["SuCode"].ToString().Trim();

                                        //2013-06-21
                                        if (strGbInfo2 != "" && strER_39Chk == "Y")
                                        {
                                            clsPmpaType.ISA[OCNT].Sucode = strSuCode;
                                            clsPmpaType.ISA[OCNT].GbInfo = strGbInfo2;
                                        }

                                        clsPmpaType.ISA[OCNT].RealQty = VB.Val(Dt.Rows[i]["RealQty"].ToString());
                                        clsPmpaType.ISA[OCNT].Qty = VB.Val(Dt.Rows[i]["Qty"].ToString());
                                        clsPmpaType.ISA[OCNT].Nal = Convert.ToInt32(VB.Val(Dt.Rows[i]["Nal"].ToString()));
                                        clsPmpaType.ISA[OCNT].Div = Convert.ToInt32(VB.Val(Dt.Rows[i]["GBDIV"].ToString()));
                                        clsPmpaType.ISA[OCNT].DosCode = Dt.Rows[i]["DosCode"].ToString().Trim();

                                        strSuCode_Temp = "";

                                        if (Dt2 != null && Dt2.Rows.Count > 0 )
                                        {
                                            strSuCode_Temp = Dt2.Rows[j]["SuCode1"].ToString().Trim();

                                        }

                                        switch (strSuCode_Temp)
                                        {
                                            case "PT-FT25":
                                            case "PT-FT50":
                                                clsPmpaType.ISA[OCNT].GbSelf = "2"; //2002/09/07 jjy 삼사계요청
                                                break;
                                            default:
                                                clsPmpaType.ISA[OCNT].GbSelf = Dt.Rows[i]["GbSelf"].ToString().Trim(); //2002/09/07 jjy 삼사계요청
                                                break;
                                        }

                                        //2016-08-30
                                        if (string.Compare(strBDATE, "2016-09-01") >= 0)
                                        {
                                            if (Check_Change_GbSelf2(pDbCon, strOrderCode, strBi, strBDATE))
                                            {
                                                clsPmpaType.ISA[OCNT].GbSelfSource = clsPmpaType.ISA[OCNT].GbSelf;
                                                clsPmpaType.ISA[OCNT].GbSelf = "0";
                                            }
                                        }

                                        clsPmpaType.ISA[OCNT].GbTFlag = Dt.Rows[i]["GbTFlag"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GbNgt = Dt.Rows[i]["GbNgt"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GbNgt2 = ""; //2012-10-11
                                        clsPmpaType.ISA[OCNT].GbHighRisk = Dt.Rows[i]["HIGHRISK"].ToString().Trim();
                                        if (clsPmpaType.ISA[OCNT].GbHighRisk != "" && clsPmpaType.ISA[OCNT].GbHighRisk != null && clsPmpaType.ISA[OCNT].GbHighRisk != "00")

                                        {
                                            clsPmpaType.ISA[OCNT].GbHighRisk = "1";
                                        }
                                        else
                                        {
                                            clsPmpaType.ISA[OCNT].GbHighRisk = "";
                                        }
                                        clsPmpaType.ISA[OCNT].GbER24H = Dt.Rows[i]["ER24"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GbGSADD = Dt.Rows[i]["GSADD"].ToString().Trim();

                                        #region 주석처리 부분
                                        //'2012-10-08 수납없이 응급실 입원수속시 - 야간체크추가
                                        //If Trim(AdoGetString(AdoRs, "SugbC", i)) = "1" And Trim(AdoGetString(AdoRs, "SugbE", i)) = "1" Then
                                        //    If Trim(clsPmpaType.ISA[OCNT].GbNgt) = "" And Trim(AdoGetString(AdoRs, "GbNgt2", i)) <> "" And Trim(AdoGetString(AdoRs, "GbIoe", i)) = "EI" Then
                                        //        'clsPmpaType.ISA[OCNT].GbNgt = Trim(AdoGetString(AdoRs, "GbNgt2", i))
                                        //        'clsPmpaType.ISA[OCNT].GbNgt2 = Trim(AdoGetString(AdoRs, "GbNgt2", i))  '2012-10-11
                                        //    End If
                                        //End If
                                        #endregion

                                        //2013-05-30
                                        if (clsPmpaType.ISA[OCNT].GbNgt == "1" || clsPmpaType.ISA[OCNT].GbNgt == "2")
                                        {
                                            if (Dt.Rows[i]["GBIOE"].ToString().Trim() == "EI" && Dt.Rows[i]["SugbC"].ToString().Trim() != "1")
                                            {
                                                clsPmpaType.ISA[OCNT].GbNgt = "0";  //야간가산부분 - 가산없으면 0
                                            }
                                            else if (Dt.Rows[i]["GBIOE"].ToString().Trim() == "EI" && Dt.Rows[i]["SugbC"].ToString().Trim() == "1")
                                            {
                                                //2013-06-07
                                                if (clsPmpaType.ISA[OCNT].GbNgt == "1")
                                                {
                                                    clsPmpaType.ISA[OCNT].GbNgt = "2";
                                                }
                                                else if (clsPmpaType.ISA[OCNT].GbNgt == "2")
                                                {
                                                    clsPmpaType.ISA[OCNT].GbNgt = "1";
                                                }
                                            }
                                        }

                                        clsPmpaType.ISA[OCNT].Bun = Dt.Rows[i]["Bun"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].Nu = Dt.Rows[i]["Nu"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbB = Dt.Rows[i]["SugbB"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbD = Dt.Rows[i]["SugbD"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbE = Dt.Rows[i]["SugbE"].ToString().Trim(); //2013-06-17
                                        clsPmpaType.ISA[OCNT].SugbF = Dt.Rows[i]["SugbF"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbI = Dt.Rows[i]["SugbI"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].TotMax = Convert.ToInt32(VB.Val(Dt.Rows[i]["TotMax"].ToString()));
                                        clsPmpaType.ISA[OCNT].OrderNo = Convert.ToInt64(VB.Val(Dt.Rows[i]["OrderNo"].ToString()));
                                        clsPmpaType.ISA[OCNT].RoomCode = Convert.ToInt32(VB.Val(Dt.Rows[i]["RoomCode"].ToString()));
                                        clsPmpaType.ISA[OCNT].GbSelNot = Dt.Rows[i]["ORDERSITE"].ToString().Trim() == "ERO" ? "1" : "";
                                        clsPmpaType.ISA[OCNT].CONSULT = Dt.Rows[i]["CONSULT"].ToString().Trim();

                                        //2015-12-30
                                        clsPmpaType.ISA[OCNT].GbErChk = "";
                                        if (strGbioe == "EI")
                                            clsPmpaType.ISA[OCNT].GbErChk = "Y";

                                        clsPmpaType.ISA[OCNT].BURN = Dt.Rows[i]["BURNADD"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].OPGBN = Dt.Rows[i]["OPGUBUN"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbAA = Dt.Rows[i]["SugbAA"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbAB = Dt.Rows[i]["SugbAB"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbAC = Dt.Rows[i]["SugbAC"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].SugbAD = Dt.Rows[i]["SugbAD"].ToString().Trim();
                                        clsPmpaType.ISA[OCNT].GBNS = Dt.Rows[i]["GBNS"].ToString().Trim();
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                        }
                        clsPmpaType.IA.SlipCount = OCNT + 1;
                    }
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            
        }
        
        public bool Account_Process_Send(PsmhDb pDbCon)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIuSentChk cISCHK = new clsIuSentChk();

            bool rtnVal = true;

            try
            {
                for (Count = 0; Count < clsPmpaType.IA.SlipCount; Count++)
                {
                    clsPmpaPb.G7AMT = 0;
                    clsPmpaPb.G7TAMT = 0;
                    clsPmpaPb.GstatPED = "0";
                    clsPmpaPb.GnWrtSeqNo = 0;
                    clsPmpaPb.GstatER = "0";  //2015-12-30


                    clsPmpaType.IA.Dept = clsPmpaType.ISA[Count].DeptCode;
                    if (cPF.Suga_Read(pDbCon, clsPmpaType.ISA[Count].Sucode) == false)
                    {
                        return false;
                    }

                    Move_RS_TO_ISG();

                    #region IORDER Send Suga_Read 부분


                    clsPmpaType.ISG.GbNgt = clsPmpaType.ISA[Count].GbNgt;
                    if (clsPmpaType.ISG.GbNgt.Trim() == "") { clsPmpaType.ISG.GbNgt = "0"; }

                    if (clsPmpaType.ISG.Bun == "35")
                    {
                        if (clsPmpaType.ISG.GbNgt == "4") { clsPmpaType.ISG.GbNgt = "D"; }
                    }

                    clsPmpaType.ISG.GbSelf = clsPmpaType.ISA[Count].GbSelf;
                    if (clsPmpaType.ISG.GbSelf == "1") { clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt; }
                    //'2013-10-25 자보산재 관련 X항1 + E항 0 인경우 자보수가세팅
                    if ((clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55") && clsPmpaType.ISG.GbSelf == "0" && clsPmpaType.ISG.SugbE == "0" && clsPmpaType.ISG.SugbX == "1" && clsPmpaType.ISG.Bun == "71")
                    {
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Tamt;
                    }
                    //외국인 계약 일경우 일반수가 * 2  즉 2배처리 한다. 
                    if (clsPmpaType.TIT.Gbilban2 == "Y")
                    {
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt * 2;
                    }

                    clsPmpaType.ISG.GbSpc = "0";
                    if (clsPmpaType.ISG.Bun == "22" && clsPmpaType.ISG.GbNgt != "1") { clsPmpaType.ISG.GbNgt = "0"; }
                    if (string.Compare(clsPmpaType.ISG.Bun, "28") < 0 && clsPmpaType.ISG.GbNgt != "0") { clsPmpaType.ISG.GbNgt = "0"; }

                    //처치 금액 의사가 직접 지정할때 2018-11-22
                    if (VB.Left(clsPmpaType.ISA[Count].GbInfo,3).ToUpper() == "AMT")
                    {
                        clsPmpaType.ISG.BaseAmt = Convert.ToInt64(VB.Replace(VB.Mid(clsPmpaType.ISA[Count].GbInfo.Trim(), 4, clsPmpaType.ISA[Count].GbInfo.Length - 3), ",", ""));  
                        clsPmpaType.ISG.SugbA = "1";
                        clsPmpaType.ISG.SugbF = "1";
                        clsPmpaType.ISG.SugbE = "0";
                        clsPmpaType.ISG.SugbG = "6";

                    }


                    #endregion

                    //2015-12-30 응급오더 액팅체크
                    clsPmpaType.ISG.GbErActTime = "";
                    clsPmpaType.ISG.GbErChk = "";
                    clsPmpaType.ISG.GbHighRisk = "";
                    clsPmpaType.ISG.GbER24H = "";
                    clsPmpaType.ISG.GSADD = "";   //2017-07-13

                    //Acting 시간 반영 응급가산 구현
                    if (clsPmpaType.IA.KTASLEVEL == "1" || clsPmpaType.IA.KTASLEVEL == "2" || clsPmpaType.IA.KTASLEVEL == "3")
                    {
                        clsPmpaType.ISG.GbErActTime = Read_ER_ORDER_ACT_Time(pDbCon, clsPmpaType.IA.pano, clsPmpaType.ISA[Count].OrderNo);
                    }

                    if (string.Compare(clsPmpaPb.GstrBdate, clsPublic.GstrZoneEmergencyStartDate) >= 0)
                        clsPmpaType.ISG.GSADD = READ_ORDER_GBADD(pDbCon, clsPmpaType.IA.pano, clsPmpaType.ISA[Count].OrderNo);

                    Move_SA_TO_SG_Send(Count);

                    Pre_Main_Account_Process(pDbCon, Count);

                    if (Main_Account_Process(pDbCon, Count) == false)
                    {
                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            
        }

        void Pre_Main_Account_Process(PsmhDb pDbCon, int nCNT)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.ISA[nCNT].DeptCode.Trim() == "NP" && string.Compare(clsPmpaType.ISG.Bun, "03") > 0)    //보호정신과 단일수가
                {
                  //  if (clsPmpaType.ISG.SugbF == "0" && clsPmpaType.ISG.GbSelf != "2" && clsPmpaType.ISG.Sunext.Trim() != "C-NP")    // 이외  급여분 금액 Zero ( 오더전송시 보험총액 제외, C-NP 제외)
                  //  {
                  //      if (clsPmpaType.IA.Dept == "NP") { clsPmpaType.ISG.BaseAmt = 0; }   //심사과 이경숙샘 요청(2011-06-17)  'TRANS 의 진료과가 NP가아니면 금액 발생
                  //  }
                }

                //21.특정과만 급여"
                //22.특정과는 비급여"
                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_MSELF_I        ";
                SQL += "  WHERE SUCODE = '" + clsPmpaType.ISA[nCNT].Sucode + "'     ";
                SQL += "    AND FIELDA = '" + clsPmpaType.ISA[nCNT].DeptCode + "'   ";
                SQL += "    AND GUBUNA ='2'                                         ";
                SQL += "    AND GUBUNB= '2'                                         ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPmpaType.ISG.GbSelf = "2";
                }
                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_MSELF_I ";
                SQL += "  WHERE SUCODE = '" + clsPmpaType.ISA[nCNT].Sucode + "' ";
                SQL += "    AND FIELDA = '" + clsPmpaType.ISA[nCNT].DeptCode + "' ";
                SQL += "    AND GUBUNA ='2'  ";
                SQL += "    AND GUBUNB= '1' ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPmpaType.ISG.SugbF = "0";
                    clsPmpaType.ISG.GbSelf = "0";
                }
                Dt.Dispose();
                Dt = null;

                //심사과 요청 2011-10-07
                //11: xx세 이하 비급여
                //12: xx세 이상 비급여
                if (VB.Val(clsPmpaType.ISG.GbSelf).ToString() == "0")
                {
                    SQL = "";
                    SQL += " SELECT FIELDA FROM " + ComNum.DB_PMPA + "BAS_MSELF_I ";
                    SQL += "  WHERE SUCODE = '" + clsPmpaType.ISA[nCNT].Sucode + "' ";
                    SQL += "    AND GUBUNA ='1'  ";
                    SQL += "    AND GUBUNB= '1' ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(VB.Val(Dt.Rows[0]["FIELDA"].ToString())) >= clsPmpaType.IA.Age2)   //2012-11-21
                            clsPmpaType.ISG.GbSelf = "2";
                    }
                    Dt.Dispose();
                    Dt = null;

                    SQL = "";
                    SQL += " SELECT FIELDA FROM " + ComNum.DB_PMPA + "BAS_MSELF_I ";
                    SQL += "  WHERE SUCODE = '" + clsPmpaType.ISA[nCNT].Sucode + "' ";
                    SQL += "    AND GUBUNA ='1'  ";
                    SQL += "    AND GUBUNB= '2' ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(VB.Val(Dt.Rows[0]["FIELDA"].ToString())) <= clsPmpaType.IA.Age2)   //2012-11-21
                            clsPmpaType.ISG.GbSelf = "2";
                    }
                    Dt.Dispose();
                    Dt = null;
                }

                //-----------------------------------------------------------------------
                //심사과 요청 2012-12-11
                //71: 남자특정과 비급여
                //72: 여자특정과 비급여
                if (clsPmpaType.ISG.GbSelf == "0")
                {
                    SQL = "";
                    SQL += " SELECT FIELDA,GUBUNB FROM " + ComNum.DB_PMPA + "BAS_MSELF ";
                    SQL += "  WHERE SUCODE = '" + clsPmpaType.ISA[nCNT].Sucode.Trim() + "' ";
                    SQL += "    AND GUBUNA ='7'  ";
                    if (clsPmpaType.IA.Sex == "M")
                    {
                        SQL += "   AND GUBUNB ='1'  ";
                    }
                    else
                    {
                        SQL += "   AND GUBUNB ='2'  ";
                    }
                    SQL += "   AND ( FIELDA='**' OR FIELDA ='" + clsPmpaType.ISA[nCNT].DeptCode.Trim() + "' ) ";
                    SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        clsPmpaType.ISG.GbSelf = "2";
                    }
                    Dt.Dispose();
                    Dt = null;

                }

                //2002-05-02 (jjy)
                if ((clsPmpaType.IA.Bi == 31 || clsPmpaType.IA.Bi == 33) && VB.Val(clsPmpaType.ISG.SugbQ) > 0) { clsPmpaType.ISG.SugbF = "0"; }

                if (clsPmpaType.ISG.SugbF == "2") { clsPmpaType.ISG.SugbF = clsPmpaType.ISG.GbSelf; }

                //2016-08-31
                if (string.Compare(clsPmpaPb.GstrBdate, "2016-09-01") >= 0)
                {
                    if (Check_Change_GbSelf(clsPmpaType.ISG.SugbS, clsPmpaType.IA.Bi.ToString()))
                        clsPmpaType.ISG.GbSelf = "0";
                }

                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            
        }

        /// <summary>
        /// Description : 외과,흉부외과 가산 구분
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgOrderNo"></param>
        /// <returns></returns>
        string READ_ORDER_GBADD(PsmhDb pDbCon, string ArgPtno, long ArgOrderNo)
        {
            string rtnVal = "";
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " SELECT GSADD ";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL += "  WHERE PTNO = '" + ArgPtno + "'        ";
            SQL += "    AND ORDERNO = " + ArgOrderNo + "    ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = Dt.Rows[0]["GSADD"].ToString().Trim();
            }

            return rtnVal;
        }
        
        string READ_SUGBF(PsmhDb pDbCon, string argOrderCode)
        {
            string rtnVal = "";
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " SELECT SUGBF ";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUT A, ";
            SQL += "        " + ComNum.DB_MED + "OCS_ORDERCODE B ";
            SQL += "  WHERE A.SuNext = B.SuCode ";
            SQL += "    AND B.ORDERCODE = '" + argOrderCode + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = Dt.Rows[0]["SUGBF"].ToString().Trim();
            }

            return rtnVal;   
        }

        string READ_SUGBF2(PsmhDb pDbCon, string ArgSuCode)
        {
            string rtnVal = "";
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " SELECT SUGBF ";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUT  ";
            SQL += "  WHERE A.SuNext = '" + ArgSuCode + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = Dt.Rows[0]["SUGBF"].ToString().Trim();
            }

            return rtnVal;
        }

        bool Check_Change_GbSelf(string ArgSugnS, string ArgBi)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = false;
            string strOK = "NO";

            //자격이 건보, 차상위, 의료급여 일경우
            switch (ArgSugnS)
            {
                case "3":
                case "6":
                case "7":
                case "8":
                    strOK = "OK"; break;
                default:
                    strOK = "NO"; break;
            }

            if (strOK != "OK") { return rtnVal; }

            //자격이 건보, 차상위, 의료급여 일경우
            switch (ArgBi)
            {
                case "11":
                case "12":
                case "13":
                case "21":
                case "22":
                    strOK = "OK";
                    break;
                default:
                    strOK = "NO";
                    break;
            }
            if (strOK != "OK") { return rtnVal; }

            rtnVal = true;

            return rtnVal;
        }

        bool Check_Change_GbSelf2(PsmhDb pDbCon, string argOrderCode, string ArgBi, string ArgBDate)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = false;
            string strOK = "NO";

            //자격이 건보, 차상위, 의료급여 일경우
            switch (ArgBi)
            {
                case "11":
                case "12":
                case "13":
                case "21":
                case "22":
                    strOK = "OK"; break;
                default:
                    strOK = "NO"; break;
            }

            if (strOK != "OK") { return rtnVal; }

            //SugbS가 6, 7일 경우
            SQL = "";
            SQL += " SELECT B.ORDERCODE, A.SUNEXT                   ";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUN A,        ";
            SQL += "        " + ComNum.DB_MED + "OCS_ORDERCODE B    ";
            SQL += "  WHERE a.SuNext = b.SuCode                     ";
            SQL += "    AND A.SUGBS IN ('3','6','7','8')                ";
            SQL += "    AND B.ORDERCODE = '" + argOrderCode + "'    ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (Dt.Rows.Count > 0)
            {
                strOK = "OK";
            }
            else
            {
                strOK = "NO";
            }

            Dt.Dispose();
            Dt = null;

            if (strOK != "OK") { return rtnVal; }

            rtnVal = true;
            
            return rtnVal;
        }

        bool Check_Change_GbSelf3(PsmhDb pDbCon, string arSucode, int ArgBi)
        {
            //2017-02-01 계장 김현욱
            //SugbS가 6, 7일 경우    '2017 - 12 - 27 8 추가
            //자격이 건보, 차상위, 의료급여 일경우
            //GbSelf = "0" 으로 치환

            //묶음 코드에 사용할 내용입니다.
            //처방코드가 아닌 수가코드로 조건 들어옴.
            //argordercode 는 수가코드
            
            bool rtnVal = false;

            clsIuSentChk cISC = new clsIuSentChk();

            //자격이 건보, 차상위, 의료급여 일경우
            if (ArgBi == 11 || ArgBi == 12 || ArgBi == 13 || ArgBi == 21 || ArgBi == 22)
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

            if (rtnVal == false) { return rtnVal; }

            string strS = cISC.Rtn_Bas_Sun_S(pDbCon, arSucode);

            //SugbS가 6, 7일 경우
            if (  strS == "3" || strS == "6" || strS == "7" || strS == "8")
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }
            
            return rtnVal;
        }

        bool READ_TRSNO(PsmhDb pDbCon)
        {
            bool rtnVal = true;
            int i = 0;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //-----------------------------------------------------------------------------
            // 允(2006-02-20) ipd_new_master 의 입원일자(INDATE)와 처방일자(GSTRBDATE)를
            // 비교하여 처방일자에 맞는 IPDNO,LASTTRS번호을 읽음
            // 문제점: 입원일자가 틀릴경우 수납에 문제 발생: 원무담당자에게 문제점통보
            // 원무프로그램에 퇴원계산시확인 루틴 처리요망
            //-----------------------------------------------------------------------------

            SQL = "";
            SQL += " SELECT A.IPDNO, A.LASTTRS, TO_CHAR(B.INDATE,'YYYY-MM-DD') INDATE,      ";
            SQL += "        TO_CHAR(B.OUTDATE,'YYYY-MM-DD') OUTDATE, A.WARDCODE,            ";
            SQL += "        a.roomcode, b.GBILBAN2, B.TRSNO, B.GBGAMEK, B.GBSPC, B.DEPTCODE ";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A,                         ";
            SQL += "        " + ComNum.DB_PMPA + "IPD_TRANS B                               ";
            SQL += "  WHERE b.Pano='" + clsPmpaType.IA.pano + "'                            ";       //재원자
            SQL += "    AND b.GBSTS IN ('0','1','2','3','4')                                ";       //추후 구분추가가능
            SQL += "    AND A.IPDNO= B.IPDNO                                                ";       //항상 1:1
            SQL += "    AND B.GbIPD = '1'                                                   ";       //지병,삭제는 제외
            SQL += "  ORDER BY INDATE DESC                                                  ";       //입원일자가 가장 큰일자부터 읽기위함
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (Dt.Rows.Count == 1)
            {
                clsPmpaType.IA.IPDNO    = Convert.ToInt64(VB.Val(Dt.Rows[0]["IPDNO"].ToString())); 
                clsPmpaType.IA.TRSNO    = Convert.ToInt64(VB.Val(Dt.Rows[0]["LASTTRS"].ToString()));
                clsPmpaType.IA.Gbilban2 = Dt.Rows[0]["GBILBAN2"].ToString().Trim(); 
                clsPmpaType.IA.GbGameK  = Dt.Rows[0]["GBGAMEK"].ToString().Trim(); 
                clsPmpaType.IA.GbSpc    = Dt.Rows[0]["GBSPC"].ToString().Trim();
                clsPmpaType.IA.Dept     = Dt.Rows[0]["DEPTCODE"].ToString().Trim();

                if (clsPmpaType.IA.WardCode == "") { clsPmpaType.IA.WardCode = Dt.Rows[0]["WARDCODE"].ToString().Trim(); }  //JJY 병동코드 NULL 방지
                if (clsPmpaType.IA.RoomCode == "") { clsPmpaType.IA.RoomCode = Dt.Rows[0]["ROOMCODE"].ToString().Trim(); }  //JJY 병동코드 NULL 방지
            }
            else if (Dt.Rows.Count > 1)
            {
                if (clsPmpaType.IA.WardCode == "") { clsPmpaType.IA.WardCode = Dt.Rows[0]["WARDCODE"].ToString().Trim(); } //JJY 병동코드 NULL 방지
                if (clsPmpaType.IA.RoomCode == "") { clsPmpaType.IA.RoomCode = Dt.Rows[0]["ROOMCODE"].ToString().Trim(); } //JJY 병동코드 NULL 방지

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    if (string.Compare(clsPmpaPb.GstrBdate, Dt.Rows[i]["INDATE"].ToString().Trim()) >= 0)
                    { 
                        clsPmpaType.IA.IPDNO    = Convert.ToInt64(VB.Val(Dt.Rows[i]["IPDNO"].ToString()));
                        clsPmpaType.IA.TRSNO    = Convert.ToInt64(VB.Val(Dt.Rows[i]["TRSNO"].ToString()));
                        clsPmpaType.IA.Gbilban2 = Dt.Rows[i]["GBILBAN2"].ToString().Trim();
                        clsPmpaType.IA.GbGameK  = Dt.Rows[i]["GBGAMEK"].ToString().Trim();
                        clsPmpaType.IA.GbSpc    = Dt.Rows[i]["GBSPC"].ToString().Trim(); 
                        clsPmpaType.IA.Dept     = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        break;
                    }
                }
            }

            Dt.Dispose();
            Dt = null;

            //응급실접수체크 2015-12-30
            if (clsPmpaType.IA.IPDNO > 0)
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(JTIME,'YYYY-MM-DD HH24:MI') JTime2                                  ";
                SQL += "   FROM " + ComNum.DB_PMPA + "OPD_MASTER                                            ";
                SQL += "  WHERE BDate  = TO_DATE('" + clsPmpaPb.GstrInDate39 + "','YYYY-MM-DD')             ";
                SQL += "    AND DeptCode IN ('EM' ,'ER')                                                    ";
                SQL += "    AND OcsJin ='#'                                                                 "; //퇴실자
                SQL += "    AND Pano IN ( SELECT Pano FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER             ";
                SQL += "        WHERE GBSTS IN ('0','1','2','3','4','5')                                    ";
                SQL += "          AND TRUNC(InDate) =TO_DATE('" + clsPmpaPb.GstrInDate39 + "','YYYY-MM-DD') ";
                SQL += "          AND AmSet7 IN ( '3','4','5' )                                             ";  //응급실 경유입원만
                SQL += "          AND IPDNO = " + clsPmpaType.IA.IPDNO + " )                                ";  //재원자
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    clsPmpaType.IA.ErJDate = Dt.Rows[0]["JTime2"].ToString().Trim(); 
                }

                Dt.Dispose();
                Dt = null;
            }


            if (clsPmpaType.IA.IPDNO == 0 || clsPmpaType.IA.TRSNO == 0)
                rtnVal = false;

            return rtnVal;
        }

        bool Last_Ipd_Slip(PsmhDb pDbCon)
        {
            bool rtnVal = true;

            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            int i = 0;

            clsPmpaPb cPb = new clsPmpaPb();
            clsPmpaFunc cPF = new clsPmpaFunc();

            try
            {
                if (clsPmpaType.IA.WardCode == "ER" || clsPmpaType.IA.WardCode == "")
                {
                    SQL = "";
                    SQL += " SELECT WARDCODE, ROOMCODE ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL += "  WHERE IPDNO = " + clsPmpaType.IA.IPDNO + " ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        IsDataFlag = false;
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        clsPmpaType.IA.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                        clsPmpaType.IA.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                for (i = 1; i < clsPmpaType.ISW.Length; i++)
                {
                    ComFunc.ReadSysDate(pDbCon);

                    clsPmpaPb.GstrSysDate = clsPublic.GstrSysDate;

                    #region Ipd_New_Slip Data Set
                    cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO]    = clsPmpaType.IA.IPDNO.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO]    = clsPmpaType.IA.TRSNO.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE]  = clsPmpaPb.GstrSysDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO]     = clsPmpaType.IA.pano;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI]       = clsPmpaType.IA.Bi.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE]    = clsPmpaPb.GstrBdate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT]   = clsPmpaType.ISW[i].Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN]      = clsPmpaType.ISW[i].Bun;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU]       = clsPmpaType.ISW[i].Nu;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY]      = clsPmpaType.ISW[i].Qty.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL]      = clsPmpaType.ISW[i].Nal.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT]  = clsPmpaType.ISW[i].BaseAmt.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC]    = clsPmpaType.ISW[i].GbSpc;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT]    = clsPmpaType.ISW[i].GbNgt;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL]  = clsPmpaType.ISW[i].GbGisul;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF]   = clsPmpaType.ISW[i].GbSelf;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD]  = clsPmpaType.ISW[i].GbChild;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.ISW[i].DeptCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE]   = clsPmpaType.ISW[i].DrCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.IA.WardCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE]   = clsPmpaType.ISW[i].Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP]   = clsPmpaType.ISW[i].GbSlip;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST]   = clsPmpaType.ISW[i].GbHost;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART]     = GstrOpFlag == "1" ? clsType.User.IdNumber : "!";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1]     = clsPmpaType.ISW[i].Amt1.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2]     = clsPmpaType.ISW[i].Amt2.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO]    = clsPmpaType.ISW[i].SEQNO.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM]     = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF]  = clsPmpaType.ISW[i].DrgSelf;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO]  = clsPmpaType.ISW[i].OrderNo.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE]      = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT]    = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT]     = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT]   = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT]    = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO]   = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.IA.RoomCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV]      = clsPmpaType.ISW[i].Div == 0 ? "1" : clsPmpaType.ISW[i].Div.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = clsPmpaType.ISW[i].GbSelNot;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS]  = clsPmpaType.ISW[i].SugbS;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER]     = clsPmpaType.ISW[i].GbEr;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN]     = clsPmpaType.ISW[i].Bun + "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE]  = clsPmpaType.ISW[i].Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT]  = clsPmpaType.ISW[i].Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD]  = clsPmpaType.ISW[i].GbGSADD;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = clsPmpaType.ISW[i].GBSUGBAB;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = clsPmpaType.ISW[i].GBSUGBAC;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = clsPmpaType.ISW[i].BURN;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE]    = clsPmpaType.ISW[i].BCODE;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPGUBUN]  = clsPmpaType.ISW[i].OPGBN;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.HIGHRISK] = clsPmpaType.ISW[i].HIGHRISK;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT2]   = clsPmpaType.ISW[i].GBNGT2;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ASADD]    = clsPmpaType.ISW[i].GbASADD;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBOP]     = GstrOpFlag;
                    #endregion
                    //TEST
                    if (clsPmpaPb.GstrIOSend_Test == "OK")
                    {
                        SqlErr = cPF.Ins_IpdNewSlip_Test(cPb.ArgV, pDbCon, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                    }
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        IsDataFlag = false;
                        return false;
                    }
                }
                    
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                IsDataFlag = false;
                return false;
            }
        }
        bool Data_Slip_Insert(PsmhDb pDbCon)
        {
            bool rtnVal = true;

            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            int i = 0;

            clsPmpaPb cPb = new clsPmpaPb();
            clsPmpaFunc cPF = new clsPmpaFunc();

            try
            {
                if (clsPmpaType.IA.WardCode == "ER" || clsPmpaType.IA.WardCode == "")
                {
                    SQL = "";
                    SQL += " SELECT WARDCODE, ROOMCODE ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL += "  WHERE IPDNO = " + clsPmpaType.IA.IPDNO + " ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        IsDataFlag = false;
                        return false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        clsPmpaType.IA.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                        clsPmpaType.IA.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                    ComFunc.ReadSysDate(pDbCon);

                    clsPmpaPb.GstrSysDate = clsPublic.GstrSysDate;

                    #region Ipd_New_Slip Data Set
                    cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.IA.IPDNO.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.IA.TRSNO.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrSysDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.IA.pano;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.IA.Bi.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaPb.GstrBdate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext; 
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = clsPmpaType.ISG.Qty.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = clsPmpaType.ISG.Nal.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = clsPmpaType.ISG.BaseAmt.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = clsPmpaType.ISG.GbSpc;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = clsPmpaType.ISG.GbNgt;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = clsPmpaType.ISG.SugbE;
                    //cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = clsPmpaType.ISG.GbSelf;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = clsPmpaType.ISG.SugbF;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = clsPmpaPb.GstatPED;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.ISG.DeptCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.ISG.DrCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.IA.WardCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = clsPmpaType.ISG.GbSlip;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = clsPmpaPb.GhostDAEPYO;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = GstrOpFlag == "1" ? clsType.User.IdNumber : "!";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = clsPmpaPb.G7AMT.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0" ;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = clsPmpaType.ISG.DrgSelf;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = clsPmpaType.ISG.OrderNo.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.IA.RoomCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = clsPmpaType.ISG.Div == 0 ? "1" : clsPmpaType.ISG.Div.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = clsPmpaType.ISG.GbSelNot;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = clsPmpaType.ISG.SugbS;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = clsPmpaType.ISG.GBKTAS;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = clsPmpaType.ISG.Bun + "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = clsPmpaType.ISG.Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = clsPmpaType.ISG.Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] = clsPmpaType.ISG.GSADD;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ASADD]   = clsPmpaType.ISG.ASADD;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = clsPmpaType.ISG.SugbAB;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = clsPmpaType.ISG.SugbAC;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = clsPmpaType.ISG.BURN;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE] = clsPmpaType.ISG.BCODE;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPGUBUN] = clsPmpaType.ISG.OPGBN;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.HIGHRISK] = clsPmpaType.ISG.GbHighRisk;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT2] = clsPmpaType.ISG.GBNGT2;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBOP] = GstrOpFlag;
                    #endregion
                    //TEST
                    if (clsPmpaPb.GstrIOSend_Test == "OK")
                    {
                        SqlErr = cPF.Ins_IpdNewSlip_Test(cPb.ArgV, pDbCon, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                    }
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        IsDataFlag = false;
                        return false;
                    }
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                IsDataFlag = false;
                return false;
            }
        }

        bool Last_Ipd_Mast(PsmhDb pDbCon)
        {
            bool rtnVal = true;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            string strAMT = string.Empty;
            string strAMTup = string.Empty;
            int i = 0;

            if (clsPmpaType.IA.Fee6 > 99)
            {
                clsPmpaType.IA.Fee6 = 99;
            }

            try
            {
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER    ";
                SQL += "    SET Fee6 = " + clsPmpaType.IA.Fee6 + "      ";
                SQL += "  WHERE IPDNO = " + clsPmpaType.IA.IPDNO + "    ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    IsDataFlag = false;
                    return false;
                }
                
                //IPD_MASTER에 금액을 UPDATE
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET              ";
                for (i = 1; i < 50; i++)
                {
                    if (clsPmpaType.IA.Amt.Length < i + 1) { Array.Resize(ref clsPmpaType.IA.Amt, i + 1); }
                    if (clsPmpaType.IA.Amt[i] != 0)
                    { 
                        strAMT = "AMT" + VB.Format(i, "00");
                        strAMTup = strAMT + " = " + clsPmpaType.IA.Amt[i] + ", ";
                        SQL += strAMTup;
                    }
                    else
                    {
                        strAMT = "AMT" + VB.Format(i, "00");
                        strAMTup = strAMT + " = 0, ";
                        SQL += strAMTup;
                    }

                }
                
                SQL += "        Amt50 = " + clsPmpaType.IA.Amt[50] + "  ";
                SQL += " WHERE TRSNO = " + clsPmpaType.IA.TRSNO + "     ";

                if (strAMT.Trim() != "")
                {
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        IsDataFlag = false;
                        return false;
                    }
                }
                else
                {
                    SQL = "";
                }
                

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                IsDataFlag = false;
                return false;
            }
      
        }
    
        public bool Acc_Proc_Main(PsmhDb pDbCon, string strBDate, string strDept)
        {
            bool rtnVal = true;
            
            //변경시 Acc_Proc_Main_OP(수술방에서 사용)  꼭 변경해 주세요

            Argument_Move(pDbCon, strBDate, strDept);       //금액계산시 필요한 기준 Set
            Area_Clear();                           //공용변수 Clear
            Account_Check();                        //수가점검
            
            if (clsPmpaPb.GstrAcctJob != "ICUPDT")  //구분변경이 아닐경우만
            {
                clsDB.setBeginTran(pDbCon);

                Account_Process(pDbCon);
                
                Last_Ipd_Slip(pDbCon);
                Last_Ipd_Mast(pDbCon);
                Last_Ipd_Note(pDbCon);

                //if (clsPmpaPb.GstatQmgrIPD == "OK") { Last_Qmgr_IPD(); }  //재원기간관리누적
                //if (clsPmpaPb.GstatQmgrBAS == "OK") { Last_Qmgr_BAS(); }  //평생관리    누적

                if (IsDataFlag)
                {
                    clsDB.setCommitTran(pDbCon);
                }
                else
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("재원진료비 계산 Main Logic Error", "전산팀 연락요망");
                    return false;
                }
            }
            else
            {
                Account_Process_ICUPDT(pDbCon);
            }

            return rtnVal;

        }

        //수술방에서 사용
        public bool Acc_Proc_Main_OP(PsmhDb pDbCon, string strBDate, string strDept)
        {
            bool rtnVal = true;

            try
            {
                Argument_Move(pDbCon, strBDate, strDept);       //금액계산시 필요한 기준 Set
                Area_Clear();                           //공용변수 Clear
                Account_Check();                        //수가점검

                if (clsPmpaPb.GstrAcctJob != "ICUPDT")  //구분변경이 아닐경우만
                {
                    //clsDB.setBeginTran(pDbCon);

                    Account_Process(pDbCon);

                    Last_Ipd_Slip(pDbCon);
                    Last_Ipd_Mast(pDbCon);
                    Last_Ipd_Note(pDbCon);

                    //if (clsPmpaPb.GstatQmgrIPD == "OK") { Last_Qmgr_IPD(); }  //재원기간관리누적
                    //if (clsPmpaPb.GstatQmgrBAS == "OK") { Last_Qmgr_BAS(); }  //평생관리    누적

                    if (IsDataFlag)
                    {
                        //clsDB.setCommitTran(pDbCon);
                        return true;
                    }
                    else
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("재원진료비 계산 Main Logic Error", "전산팀 연락요망");
                        return false;
                    }
                }
                else
                {
                    Account_Process_ICUPDT(pDbCon);
                }

                return rtnVal;
            }
            catch
            {
                return false;
            }
        }

        void Last_Ipd_Note(PsmhDb pDbCon)
        {
            int i = 0;
            string strConvDate = string.Empty;
            ComFunc CF = new ComFunc();
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            if (clsPmpaType.IA.DisCharge == false)
            {
                for (i = 0; i < 60; i++)
                {
                    if (clsPmpaPb.G7QTY11[i] == 0 && clsPmpaPb.G7QTY20A[i] == 0 && clsPmpaPb.G7QTY20B[i] == 0 && clsPmpaPb.G7AL201[i] == 0 && clsPmpaPb.G7AL010[i] == 0)
                        break;
                    strConvDate = CF.DATE_ADD(pDbCon, clsPmpaType.IA.Date, i);

                    SQL = "";
                    SQL += " SELECT * FROM " + ComNum.DB_PMPA + "IPD_NOTE                \r\n";
                    SQL += "  WHERE  Pano = '" + clsPmpaType.IA.pano + "'                \r\n";
                    SQL += "    AND  Bdate = TO_DATE('" + strConvDate + "','YYYY-MM-DD') \r\n";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                   
                    }

                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_NOTE                       \r\n";
                        SQL += "   SET Fee1 = Fee1 + " + clsPmpaPb.G7QTY20A[i] + ",         \r\n";
                        SQL += "       Fee2 = Fee2 + " + clsPmpaPb.G7QTY20B[i] + ",         \r\n";
                        SQL += "       Fee4 = Fee4 + " + clsPmpaPb.G7QTY11[i] + ",          \r\n";
                        SQL += "       AL010 = AL010 + " + clsPmpaPb.G7AL010[i] + ",          \r\n";
                        SQL += "       AL201 = AL201 + " + clsPmpaPb.G7AL201[i] + "         \r\n";
                        SQL += " WHERE Pano = '" + clsPmpaType.IA.pano + "'                 \r\n";
                        SQL += "   AND Bdate = TO_DATE('" + strConvDate + "','YYYY-MM-DD')  \r\n";
                    }
                    else
                    {
                        SQL = "";
                        SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NOTE (Pano,Bdate,Fee1,Fee2,Fee4,AL010,AL201) VALUES (";
                        SQL += " '" + clsPmpaType.IA.pano + "', ";
                        SQL += " TO_DATE('" + strConvDate + "','YYYY-MM-DD'), ";
                        SQL += " " + clsPmpaPb.G7QTY20A[i] + ", ";
                        SQL += " " + clsPmpaPb.G7QTY20B[i] + ", ";
                        SQL += " " + clsPmpaPb.G7QTY11[i] + ", ";
                        SQL += " " + clsPmpaPb.G7AL010[i] + ", ";
                        SQL += " " + clsPmpaPb.G7AL201[i] + ") ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    
                    }

                }
            }

        }

        public void Argument_Move(PsmhDb pDbCon, string strBDate, string strDept)
        {
            ComFunc CF = new ComFunc();

            clsPmpaType.IA.Date     = strBDate;
            clsPmpaType.IA.Dept     = strDept;
            clsPmpaType.IA.Sex      = clsPmpaType.TIT.Sex;
            clsPmpaType.IA.GbGameK  = clsPmpaType.TIT.GbGameK;
            clsPmpaType.IA.Retn     = 0;
            clsPmpaType.IA.Bi       = Convert.ToUInt16(clsPmpaType.TIT.Bi);
            clsPmpaType.IA.Bi1      = Convert.ToUInt16(VB.Mid(clsPmpaType.TIT.Bi, 1, 1));
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55)
            {
                clsPmpaType.IA.Bi1 = 6;     //자보
            }
            clsPmpaType.IA.Jumin1 = clsPmpaType.TIT.Jumin1;
            clsPmpaType.IA.Jumin2 = clsPmpaType.TIT.Jumin3;
         
            //처방일자 기준의 나이계산 및 적용이 필요
            clsPmpaType.IA.Age = clsPmpaType.TIT.Age;
            clsPmpaType.IA.Fee6 = clsPmpaType.TIT.Fee6;

            //신생아 마취료 계산 때문 일수 구함. 30일이전은 60%가산 이후 30% 가산 2006-05-17
            clsPmpaType.IA.AgeiLsu = 0;

            //2014-08-14 기존 0살만 생후일수 COUNT 하였지만 기준을 해제함
            clsPmpaType.IA.AgeiLsu = CF.DATE_ILSU(pDbCon, strBDate, "20" + VB.Left(clsPmpaType.TIT.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TIT.Jumin1, 3, 2) + "-" + VB.Right(clsPmpaType.TIT.Jumin1, 2));

            if (clsPmpaType.IA.Fee6 < 0)
            {
                clsPmpaType.IA.Fee6 = 0;
            }

            clsPmpaType.IA.Gbilban2 = clsPmpaType.TIT.Gbilban2;     //외국일반 2배
            clsPmpaType.IA.GbSpc = clsPmpaType.TIT.GbSpc;
            clsPmpaType.IA.pano = clsPmpaType.TIT.Pano;
            clsPmpaType.IA.DrCode = clsPmpaType.TIT.DrCode;
            clsPmpaType.IA.IPDNO = clsPmpaType.TIT.Ipdno;

            //2018-08-06 추가분 KMC
            clsPmpaType.IA.TRSNO = clsPmpaType.TIT.Trsno;
            clsPmpaType.IA.WardCode = clsPmpaType.TIT.WardCode;
            clsPmpaType.IA.RoomCode = clsPmpaType.TIT.RoomCode;

            clsPmpaType.IA.KTASLEVEL = clsPmpaType.TIT.KTASLEVL;
        }

        void Area_Clear()
        {
            clsPmpaPb.G7WRTcount = 0;

            clsPmpaPb.GstatQmgrIPD = "";
            clsPmpaPb.GstatQmgrBAS = "";
            clsPmpaPb.GstrB20STAT = "";
            clsPmpaPb.GstrB11STAT = "";
            clsPmpaPb.GstrB65STAT = "";

            //clsPmpaType.ISA = new clsPmpaType.Slip_Accept_Table_IPD[1];
            clsPmpaType.ISW = new clsPmpaType.Slip_Write_Table_IPD[1];

            clsPmpaType.IA.Amt  = new long[61];
            clsPmpaPb.G7QTY11   = new double[61];
            clsPmpaPb.G7QTY20A  = new double[61];
            clsPmpaPb.G7QTY20B  = new double[61];
            clsPmpaPb.G7AL201   = new int[61];
            clsPmpaPb.G7AL010 = new double[61];

            //if (clsPmpaType.ISA != null) { Array.Clear(clsPmpaType.ISA, 0, clsPmpaType.ISA.Length); }
            if (clsPmpaType.ISW != null) { Array.Clear(clsPmpaType.ISW, 0, clsPmpaType.ISW.Length); }

        }

        void Account_Check()
        {
            int nCNT = 0;

            for (nCNT = 0; nCNT < clsPmpaType.ISA.Length; nCNT++)
            {
                if (clsPmpaType.ISA[nCNT].Sucode.Trim() != ""|| clsPmpaType.ISA[nCNT].Sucode.Trim() != null)
                {
                    //SMA_Check     1999-11-15 일부 기준이라서 제외시킴
                    Data_Send_Check(nCNT);
                    Qmgr_Data_Check(nCNT);
                    if (clsPmpaType.IA.Bi != 52 && clsPmpaType.IA.Bi != 55)
                    {
                        //Xray_CT_Check     CT 2개이상 동일부위 Check      내용없음
                        //Xray_MRI_Check    MRI 2개이상 동일부위 Check     내용없음
                        //Xray_GOL_Check    골밀도 2개이상 동일부위 Check  내용없음
                        //Xray_SONO_Check   초음파 2개이상 동일부위 Check  내용없음
                    }
                }
            }
        }

        void Data_Send_Check(int nCNT)
        {
            if (clsPmpaType.ISA[nCNT].Bun == "65" || clsPmpaType.ISA[nCNT].Bun == "71" || clsPmpaType.ISA[nCNT].Bun == "72" || clsPmpaType.ISA[nCNT].Bun == "73" || clsPmpaType.ISA[nCNT].Bun == "78")
            {
                clsPmpaPb.GstrB65STAT = "OK";
            }

            if (clsPmpaType.ISA[nCNT].Bun == "11" || clsPmpaType.ISA[nCNT].Bun == "12")
            {
                clsPmpaPb.GstrB11STAT = "OK";
            }

            if (clsPmpaType.ISA[nCNT].Bun == "20" && string.Compare(clsPmpaType.ISA[nCNT].SugbB, "3") > 0 && string.Compare(clsPmpaType.ISA[nCNT].SugbB, "7") < 0)
            {
                clsPmpaPb.GstrB20STAT = "OK";       //수액 입력 Check 주사수기료 IM 변환
            }

        }

        void Qmgr_Data_Check(int nCNT)
        {
            if (clsPmpaType.IA.Bi < 40 && clsPmpaType.ISA[nCNT].TotMax > 0 && clsPmpaType.ISA[nCNT].GbSelf == "0")
            {
                if (clsPmpaType.ISA[nCNT].SugbI == "1" || clsPmpaType.ISA[nCNT].SugbI == "2")
                {
                    clsPmpaPb.GstatQmgrIPD = "OK";
                }
                else if (clsPmpaType.ISA[nCNT].SugbI == "3" || clsPmpaType.ISA[nCNT].SugbI == "4")
                {
                    clsPmpaPb.GstatQmgrBAS = "OK";
                }
            }
        }

        void Account_Process(PsmhDb pDbCon)
        {
            int nInx = 0;

            for (nInx = 0; nInx < clsPmpaType.ISA.Length; nInx++)
            {
                if (clsPmpaType.ISA[nInx].Sucode.Trim() != "")
                {
                    if (clsPmpaType.ISA[nInx].Sunext.Trim() != "")
                    {
                        clsPmpaPb.G7AMT = 0;
                        clsPmpaPb.G7TAMT = 0;
                        clsPmpaPb.GstatPED = "0";
                        clsPmpaPb.GnWrtSeqNo = 0;
                        Move_SA_TO_SG(nInx);
                        if (Main_Account_Process(pDbCon, nInx) == false)
                        {
                            IsDataFlag = false;
                        }
                    }
                }
            }

        }
        
        /// <summary>
        /// SA => SG 변수 이관 (배열에서 낱개변수로 변환해서 계산하기 위함)
        /// </summary>
        /// <param name="nInx"></param>
        void Move_SA_TO_SG(int nInx)
        {
            clsPmpaType.ISG.BCODE = "";
            clsPmpaType.ISG.Qty      = clsPmpaType.ISA[nInx].Qty;
            clsPmpaType.ISG.Nal      = clsPmpaType.ISA[nInx].Nal;
            clsPmpaType.ISG.Imiv     = clsPmpaType.ISA[nInx].Imiv;
            clsPmpaType.ISG.Dev      = clsPmpaType.ISA[nInx].Dev;
            if (clsPmpaType.ISA[nInx].SugbC != "0")
            {
                clsPmpaType.ISG.GbNgt = clsPmpaType.ISA[nInx].GbNgt;
            }
            else
            {
                clsPmpaType.ISG.GbNgt = "0";
            }
            clsPmpaType.ISG.GbSpc    = clsPmpaType.ISA[nInx].GbSpc;
            clsPmpaType.ISG.GbSelf   = clsPmpaType.ISA[nInx].GbSelf;
            clsPmpaType.ISG.BaseAmt  = clsPmpaType.ISA[nInx].BaseAmt;
            clsPmpaType.ISG.Sucode   = clsPmpaType.ISA[nInx].Sucode;
            clsPmpaType.ISG.Sunext   = clsPmpaType.ISA[nInx].Sunext;
            clsPmpaType.ISG.Bun      = clsPmpaType.ISA[nInx].Bun;
            clsPmpaType.ISG.Nu       = clsPmpaType.ISA[nInx].Nu;
            clsPmpaType.ISG.SugbA    = clsPmpaType.ISA[nInx].SugbA;
            clsPmpaType.ISG.SugbB    = clsPmpaType.ISA[nInx].SugbB;
            clsPmpaType.ISG.SugbC    = clsPmpaType.ISA[nInx].SugbC;
            clsPmpaType.ISG.SugbD    = clsPmpaType.ISA[nInx].SugbD;
            clsPmpaType.ISG.SugbE    = clsPmpaType.ISA[nInx].SugbE;
            clsPmpaType.ISG.SugbF    = clsPmpaType.ISA[nInx].SugbF;
            clsPmpaType.ISG.SugbG    = clsPmpaType.ISA[nInx].SugbG;
            clsPmpaType.ISG.SugbH    = clsPmpaType.ISA[nInx].SugbH;
            clsPmpaType.ISG.SugbI    = clsPmpaType.ISA[nInx].SugbI;
            clsPmpaType.ISG.SugbJ    = clsPmpaType.ISA[nInx].SugbJ;
            clsPmpaType.ISG.SugbP    = clsPmpaType.ISA[nInx].SugbP;
            clsPmpaType.ISG.SugbQ    = clsPmpaType.ISA[nInx].SugbQ;
            clsPmpaType.ISG.SugbR    = clsPmpaType.ISA[nInx].SugbR;
            clsPmpaType.ISG.SugbS    = clsPmpaType.ISA[nInx].SugbS;
            clsPmpaType.ISG.SugbX    = clsPmpaType.ISA[nInx].SugbX;
            clsPmpaType.ISG.SugbY    = clsPmpaType.ISA[nInx].SugbY;
            clsPmpaType.ISG.SugbZ    = clsPmpaType.ISA[nInx].SugbZ;
            clsPmpaType.ISG.SugbAA   = clsPmpaType.ISA[nInx].SugbAA;
            clsPmpaType.ISG.SugbAB   = clsPmpaType.ISA[nInx].SugbAB;
            clsPmpaType.ISG.SugbAC   = clsPmpaType.ISA[nInx].SugbAC;
            clsPmpaType.ISG.SugbAD   = clsPmpaType.ISA[nInx].SugbAD;
            clsPmpaType.ISG.SugbAG   = clsPmpaType.ISA[nInx].SugbAG;
            clsPmpaType.ISG.Iamt     = clsPmpaType.ISA[nInx].Iamt;
            clsPmpaType.ISG.Tamt     = clsPmpaType.ISA[nInx].Tamt;
            clsPmpaType.ISG.Bamt     = clsPmpaType.ISA[nInx].Bamt;
            clsPmpaType.ISG.Selamt   = clsPmpaType.ISA[nInx].Selamt;
            clsPmpaType.ISG.Sudate   = clsPmpaType.ISA[nInx].Sudate;
            clsPmpaType.ISG.OldIamt  = clsPmpaType.ISA[nInx].OldIamt;
            clsPmpaType.ISG.OldTamt  = clsPmpaType.ISA[nInx].OldTamt;
            clsPmpaType.ISG.OldBamt  = clsPmpaType.ISA[nInx].OldBamt;
            clsPmpaType.ISG.DrgSelf  = clsPmpaType.ISA[nInx].DrgSelf;
            clsPmpaType.ISG.OrderNo  = clsPmpaType.ISA[nInx].OrderNo;
            clsPmpaType.ISG.Div      = clsPmpaType.ISA[nInx].Div;
            clsPmpaType.ISG.DrCode   = clsPmpaType.ISA[nInx].DrCode;
            clsPmpaType.ISG.DeptCode = clsPmpaType.ISA[nInx].DeptCode;
            clsPmpaType.ISG.GbSelNot = "";
            if (clsPmpaType.ISA[nInx].GbSelNot != "")
            {
                clsPmpaType.ISG.GbSelNot = clsPmpaType.ISA[nInx].GbSelNot;
            }
            clsPmpaType.ISG.PART2 = "";
            clsPmpaType.ISG.GBKTAS = clsPmpaType.ISA[nInx].GBKTAS;
            clsPmpaType.ISG.GbChildZ = clsPmpaType.ISA[nInx].GbChildZ;
            clsPmpaType.ISG.GbErChk = clsPmpaType.ISA[nInx].GbErChk;
            clsPmpaType.ISG.GbHighRisk = clsPmpaType.ISA[nInx].GbHighRisk;
            clsPmpaType.ISG.GbER24H = clsPmpaType.ISA[nInx].GbER24H;
            clsPmpaType.ISG.BURN = clsPmpaType.ISA[nInx].BURN;
            clsPmpaType.ISG.OPGBN = clsPmpaType.ISA[nInx].OPGBN;
            clsPmpaType.ISG.GSADD = clsPmpaType.ISA[nInx].GbGSADD;
            clsPmpaType.ISG.ASADD = clsPmpaType.ISA[nInx].GbASADD;
            clsPmpaType.ISG.GBNS = clsPmpaType.ISA[nInx].GBNS;

            //환자 구분 변경용 
            if (clsPmpaPb.GstrAcctJob == "ICUPDT")
            {
                clsPmpaType.ISG.SAno = nInx;
            }
        }

        void Move_SA_TO_SG_Send(int nInx)
        {
            clsPmpaType.ISG.RealQty     = clsPmpaType.ISA[nInx].Qty;
            clsPmpaType.ISG.Qty         = clsPmpaType.ISA[nInx].Qty;
            clsPmpaType.ISG.Nal         = clsPmpaType.ISA[nInx].Nal;
            clsPmpaType.ISG.Div         = clsPmpaType.ISA[nInx].Div;
            //2013-06-17
            if (clsPmpaType.ISA[nInx].Bun == "22" && clsPmpaType.ISA[nInx].SugbE == "1")
                clsPmpaType.ISG.GbNgt = clsPmpaType.ISA[nInx].GbNgt;
            
            //2015-12-30
            clsPmpaType.ISG.GbErChk     = clsPmpaType.ISA[nInx].GbErChk;
            clsPmpaType.ISG.GbHighRisk  = clsPmpaType.ISA[nInx].GbHighRisk;
            clsPmpaType.ISG.GbER24H     = clsPmpaType.ISA[nInx].GbER24H;
            clsPmpaType.ISG.BURN        = clsPmpaType.ISA[nInx].BURN;
            clsPmpaType.ISG.OPGBN       = clsPmpaType.ISA[nInx].OPGBN;

            clsPmpaType.ISG.SugbAA  = clsPmpaType.ISA[nInx].SugbAA;
            clsPmpaType.ISG.SugbAB  = clsPmpaType.ISA[nInx].SugbAB;
            clsPmpaType.ISG.SugbAC  = clsPmpaType.ISA[nInx].SugbAC;
            clsPmpaType.ISG.SugbAD  = clsPmpaType.ISA[nInx].SugbAD;
            clsPmpaType.ISG.BCODE   = clsPmpaType.ISA[nInx].BCODE == null ? "" : clsPmpaType.ISA[nInx].BCODE.Trim();

        }

        bool Main_Account_Process(PsmhDb pDbCon, int nCNT)
        {
            bool rtnVal = true;
            clsBasAcct cBAcct = new clsBasAcct();

            try
            {
                clsPmpaPb.GhostDAEPYO = "0";        //GBHOST

                #region 주사수기료 관련 B항 세팅
                if (clsPmpaType.ISG.Bun == "20" && clsPmpaType.ISG.SugbB == "3")
                {
                    clsPmpaType.ISG.SugbB = clsPmpaType.ISG.Imiv;
                }
                #endregion

                #region 보험 총액부분 처리
                if (clsPmpaType.ISG.SugbF == "2")
                {
                    clsPmpaType.ISG.SugbF = clsPmpaType.ISG.GbSelf;
                }
                #endregion

                #region 산재환자 Q항관련 F항 급여처리
                if (clsPmpaType.IA.Bi == 31 && string.Compare(clsPmpaType.ISG.SugbQ, "0") > 0)
                {
                    clsPmpaType.ISG.SugbF = "0";
                }
                #endregion

                #region 그룹수가 처리 부분
                if (string.Compare(clsPmpaType.ISG.SugbA, "1") > 0 && clsPmpaType.IA.Bi < 50)
                {
                    if ((clsPmpaType.ISG.SugbF != "1" && clsPmpaType.ISG.GbSelf != "1") || clsPmpaType.ISG.Bun == "22" || clsPmpaType.ISG.Bun == "23")
                    {
                        //Account_Process_Host("BO");
                        Account_Process_Host_ONE(pDbCon, "BO", "");
                        return rtnVal;
                    }
                }

                if (string.Compare(clsPmpaType.ISG.SugbA, "1") > 0 && (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55))   //TA 보험 환자
                {
                    if (clsPmpaType.ISG.SugbF != "1" && clsPmpaType.ISG.GbSelf != "1")
                    {
                        //Account_Process_Host("TA");
                        Account_Process_Host_ONE(pDbCon, "TA", "");
                        return rtnVal;
                    }
                }

                if (string.Compare(clsPmpaType.ISG.SugbA, "2") > 0 && clsPmpaType.IA.Bi > 50)
                {
                    //Account_Process_Host("IL");
                    Account_Process_Host_ONE(pDbCon, "IL", "");
                    return rtnVal;
                }

                if (string.Compare(clsPmpaType.ISG.SugbA, "2") > 0 && clsPmpaType.IA.Bi < 50 && clsPmpaType.ISG.GbSelf == "1")
                {
                    //Account_Process_Host("IL");
                    Account_Process_Host_ONE(pDbCon, "IL", "");
                    return rtnVal;
                }
                #endregion

                #region //2013-10-25 산재환자중 초음파수가는 자보수가를 읽음 (심사과 의뢰서 작업)
                if ((clsPmpaType.IA.Bi == 31 || clsPmpaType.IA.Bi == 33 || clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55) && clsPmpaType.ISG.SugbX == "1" && clsPmpaType.ISG.GbSelf == "0" && clsPmpaType.ISG.Bun == "71")
                {
                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Tamt;
                }
                #endregion

                #region //2015-09-17 외국인일반 2배수가 적용
                if (clsPmpaType.IA.Bi == 51 && clsPmpaType.IA.Gbilban2 == "Y")
                {
                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt * 2;  //외국new
                }
                #endregion

                #region 기준일자가 과거범위 이므로 로직구현에서 제외시킴
                //' 97/01/01부터 보호이원검사 가산율 3% 신설
                //' 99/01/01부터 이원검사는 10%가산됨
                //  If clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.ISG.SugbJ = "9" Then            '보호 외부검사 기술료 가산
                //      If clsPmpaType.ISG.Nu = "13" || clsPmpaType.ISG.Nu = "14" Then
                //          If clsPmpaType.IA.Date < "1998-07-01" Then
                //              clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt / 1.1 * 1.05
                //          ElseIf clsPmpaType.IA.Date < "1999-01-01" Then
                //              clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt / 1.13 * 1.05
                //          End If
                //      End If
                //  End If
                #endregion

               

                #region //보호식대 2013-04-03
                if ((clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22) && (clsPmpaType.ISG.Sucode.Trim() == "F01T" || clsPmpaType.ISG.Sucode.Trim() == "F02T" || clsPmpaType.ISG.Sucode.Trim() == "F03T"))
                {
                    clsPmpaType.ISG.BaseAmt = 0;
                }
                #endregion

                #region 보호정신과 NP 처리
                if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30)
                {
                   // if (clsPmpaType.IA.Dept == "NP" && clsPmpaType.ISG.BaseAmt == 0) 
                   // {
                   //     clsPmpaPb.GhostDAEPYO = "0";
                   //     Slip_Write_Process(pDbCon);
                   //     return rtnVal;
                   // }
                }
                #endregion
                #region //보호정신과 단일수가
                //정신과 입원이 생길시 다시 열어야 됨
                if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.IA.Dept == "NP" && (string.Compare(clsPmpaType.ISG.Bun, "03") > 0 || clsPmpaType.ISG.Sucode.Trim() == "AC101"))
                {
                    //  if (clsPmpaType.ISG.SugbF == "0" && clsPmpaType.ISG.Sunext.Trim() != "C-NP") 
                    //  {
                    //      clsPmpaType.ISG.BaseAmt = 0;
                    //  }
                }
                #endregion

                #region //보호정신과 단일수가-진찰료관련 2010-06-03
                if ((clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22) && clsPmpaType.IA.Dept == "NP" && (clsPmpaType.ISG.Bun == "01" || clsPmpaType.ISG.Bun == "02"))
                {
                    if (clsPmpaType.ISG.Sucode.Equals("AA176") ||
                        clsPmpaType.ISG.Sucode.Equals("AA276") ||
                        clsPmpaType.ISG.Sucode.Equals("AA1761") ||
                        clsPmpaType.ISG.Sucode.Equals("AA2761") ||
                        clsPmpaType.ISG.Sucode.Equals("AA1762") ||
                        clsPmpaType.ISG.Sucode.Equals("AA2762"))
                    {
                        clsPmpaType.ISG.BaseAmt = 0;

                    }
                }
                #endregion

                #region 선택진료 정보 Setting (주석처리)
                //clsPmpaType.Sel_Main_MST SMM = new clsPmpaType.Sel_Main_MST();
                //SMM.ArgSpc = clsPmpaType.IA.GbSpc;
                //SMM.ArgIO = "I";
                //SMM.ArgPano = clsPmpaType.IA.pano;
                //SMM.ArgBDate = clsPmpaType.IA.Date;
                //SMM.ArgBi = clsPmpaType.IA.Bi.ToString();
                //SMM.ArgGamek = clsPmpaType.IA.GbGameK;
                //SMM.ArgDeptCode = clsPmpaType.IA.Dept;
                //SMM.ArgDrCode = clsPmpaType.IA.DrCode;
                //SMM.ArgBun = clsPmpaType.ISG.Bun;
                //SMM.ArgSuCode = clsPmpaType.ISG.Sucode;
                //SMM.argSUNEXT = clsPmpaType.ISG.Sunext;
                //SMM.ArgIPDNO = clsPmpaType.IA.IPDNO;
                //SMM.ArgETC = clsPmpaType.ISG.GbSelNot;

                //clsPmpaPb.G7TAMT = 0;
                //if (cPSel.Read_Select_Main(pDbCon, SMM) == "OK")
                //{
                //    if (string.Compare(clsPmpaType.SEL.Suga_GbSelect, "0") > 0 && clsPmpaPb.GnSelAmt > 0)
                //    {
                //        if (clsPmpaType.SEL.Suga_GbSelect == "5")
                //        {
                //            clsPmpaPb.G7AMT = (long)(clsPmpaPb.GnSelAmt * clsPmpaType.SEL.Current_Rate / 100);
                //            clsPmpaPb.G7TAMT = cBAcct.BAS_MACH_AMT(1, clsPmpaType.ISG.Sunext, 0, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
                //            if (clsPmpaType.ISG.Nal < 0) { clsPmpaPb.G7TAMT = clsPmpaPb.G7TAMT * -1; }
                //        }
                //        else
                //        {
                //            clsPmpaPb.G7TAMT = (long)(clsPmpaPb.GnSelAmt * clsPmpaType.SEL.Current_Rate / 100) * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal;
                //        }
                //    }
                //}
                #endregion

                clsPmpaPb.GhostDAEPYO = "0";

                #region //소아 및 응급가산 공통부분
                //ER 중증환자 가산 
                cBAcct.Bas_ER_Rate("", "", clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.ISG.GBKTAS, "I", "OK"); //GnErRate
                                                                                                                            //소아가간율 SET
                cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date); //GnPedRate
                #endregion

                #region 분류별 계산로직
                if (string.Compare(clsPmpaType.ISG.Bun, "11") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "15") <= 0)
                {
                    BUN_11_Account(pDbCon, nCNT);
                }
                else if (string.Compare(clsPmpaType.ISG.Bun, "16") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "21") <= 0)
                {
                    BUN_20_Account(pDbCon);
                }
                else if (string.Compare(clsPmpaType.ISG.Bun, "22") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "23") <= 0)
                {
                    BUN_22_Account(pDbCon);
                }
                else if (clsPmpaType.ISG.Bun == "28" || clsPmpaType.ISG.Bun == "34" || clsPmpaType.ISG.Bun == "35")
                {
                    BUN_34_Account(pDbCon);
                }
                else if (clsPmpaType.ISG.Bun == "37")
                {
                    BUN_37_Account(pDbCon);
                }
                else if (clsPmpaType.ISG.Bun == "75")
                {
                    BUN_75_Account(pDbCon);
                }
                else
                {
                    BUN_99_Account(pDbCon);
                }
                #endregion

                // 1999.11.15 내복,외용,주사약 의약품관리료 신설
                if (clsPmpaType.ISG.Bun == "11" || clsPmpaType.ISG.Bun == "12" || clsPmpaType.ISG.Bun == "20" || clsPmpaType.ISG.Bun == "23")
                {
                    BUN_AL201_Account();   //의약품관리료
                }

                // 2019-01-01 마약관리료
                if (Read_DRUG_MAYAK_CHK(pDbCon ,clsPmpaType.ISG.Sunext ) =="OK")
                {
                    BUN_AL010_Account();   //마약의약품관리료
                }

                rtnVal = IsDataFlag;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
        
        public void Account_Process_ICUPDT(PsmhDb pDbCon)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIuSentChk cISCHK = new clsIuSentChk();
            int nInx = 0;

            for (nInx = 0; nInx < clsPmpaType.ISA.Length; nInx++)
            {
                if (clsPmpaType.ISA[nInx].Sucode.Trim() != "")
                {
                    if (clsPmpaType.ISA[nInx].Sunext.Trim() != "")
                    {
                        clsPmpaPb.G7AMT = 0;
                        clsPmpaPb.G7TAMT = 0;
                        clsPmpaPb.GstatPED = "0";
                        clsPmpaPb.GnWrtSeqNo = 0;
                        Move_SA_TO_SG(nInx);

                        if (cPF.Suga_Read(pDbCon, clsPmpaType.ISA[nInx].Sucode) == false)
                        {
                            return;
                        }
                        Move_RS_TO_ISG_ICUPDT();

                        if (Main_Account_Process(pDbCon, nInx) == false)
                        {
                            IsDataFlag = false;
                        }
                    }
                }
            }

            
          
        }

        public void Slip_Write_Process(PsmhDb pDbCon)
        {
            clsPmpaPb.G7WRTcount += 1;
            if (clsPmpaPb.GstrAcctJob == "ICUPDT")
            {
                Data_Convert();       //지정진료 누적 Set
                clsPmpaPb.GstrBdate = clsPmpaType.IA.Date;
                Data_Slip_Insert(pDbCon);
                Data_Amt_Add();
                // Last_Qmgr_IPD(pDbCon);
                return;
            }
            Data_Convert();       //지정진료 누적 Set

            DATA_MOVE(pDbCon, clsPmpaPb.G7WRTcount);
           
            Data_Amt_Add();
            
         


            if (clsPmpaType.IA.Bi > 40 && clsPmpaType.ISA[Count].TotMax > 0 && clsPmpaType.ISG.GbSelf == "0")
            {
                if (clsPmpaType.ISA[Count].SugbI == "1" || clsPmpaType.ISA[Count].SugbI == "2")
                {
                    Last_Qmgr_IPD(pDbCon);
                }
            }
            
        }

        void Data_Convert()
        {
            if (string.Compare(clsPmpaType.ISG.GbSelf, "0") > 0)
                clsPmpaType.ISG.SugbF = clsPmpaType.ISG.GbSelf;

            if (clsPmpaType.IA.Bi < 40)
            {
                if ((string.Compare(clsPmpaType.ISG.SugbF, "0") > 0 || string.Compare(clsPmpaType.ISG.GbSelf, "0") > 0) 
                    && (clsPmpaType.ISG.SugbS != "3" &&  clsPmpaType.ISG.SugbS != "6" && clsPmpaType.ISG.SugbS != "7" && clsPmpaType.ISG.SugbS != "8"))     //2017-12-27 sugbs = "8" 추가
                {
                    switch (clsPmpaType.ISG.Nu)
                    {
                        case "01":
                        case "02":
                        case "03": clsPmpaType.ISG.Nu = "21"; break;
                        case "04":
                        case "05":
                        case "06":
                        case "07":
                        case "08":
                        case "09":
                        case "10":
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15": clsPmpaType.ISG.Nu = (VB.Val(clsPmpaType.ISG.Nu) + 18).ToString(); break;
                        case "16": clsPmpaType.ISG.Nu = "34"; break;
                        case "17": clsPmpaType.ISG.Nu = "42"; break;
                        case "18": clsPmpaType.ISG.Nu = "47"; break;
                        case "19": clsPmpaType.ISG.Nu = "37"; break;
                        case "20": clsPmpaType.ISG.Nu = "27"; break;
                        default:
                            break;
                    }
                }
                
                if (clsPmpaType.IA.Bi == 31 && string.Compare(clsPmpaType.ISG.SugbQ, "0") > 0)
                {
                    if (clsPmpaType.ISG.Nu == "38" || clsPmpaType.ISG.Nu == "40")
                    {
                        clsPmpaType.ISG.Nu = "20";
                    }
                }
            }
        }

        void DATA_MOVE(PsmhDb pDbCon, int nCNT)
        {
            try
            {
                if (clsPmpaType.ISW.Length < nCNT + 1) { Array.Resize(ref clsPmpaType.ISW, nCNT + 1); }

                clsPmpaType.ISW[nCNT].Sucode        = clsPmpaType.ISG.Sucode;
                clsPmpaType.ISW[nCNT].Sunext        = clsPmpaType.ISG.Sunext;
                clsPmpaType.ISW[nCNT].Bun           = clsPmpaType.ISG.Bun;
                clsPmpaType.ISW[nCNT].Nu            = clsPmpaType.ISG.Nu;
                clsPmpaType.ISW[nCNT].Qty           = clsPmpaType.ISG.Qty;
                clsPmpaType.ISW[nCNT].Nal           = clsPmpaType.ISG.Nal;
                clsPmpaType.ISW[nCNT].Div           = clsPmpaType.ISG.Div;
                clsPmpaType.ISW[nCNT].SugbS         = clsPmpaType.ISG.SugbS;    //2017-02-01
                clsPmpaType.ISW[nCNT].BaseAmt       = clsPmpaType.ISG.BaseAmt;
                clsPmpaType.ISW[nCNT].GbSpc         = clsPmpaType.ISG.GbSpc == "" ? "0" : clsPmpaType.ISG.GbSpc;
                clsPmpaType.ISW[nCNT].GbNgt         = clsPmpaType.ISG.GbNgt == "" ? "0" : clsPmpaType.ISG.GbNgt;
                clsPmpaType.ISW[nCNT].GbGisul       = clsPmpaType.ISG.SugbE;
                clsPmpaType.ISW[nCNT].GbSelf        = clsPmpaType.ISG.SugbF;
                clsPmpaType.ISW[nCNT].GbSelfSource  = clsPmpaType.ISG.GbSelfSource;
                clsPmpaType.ISW[nCNT].GbChild       = clsPmpaPb.GstatPED;

                if (string.Compare(clsPmpaType.ISG.GbSelf, "0") > 0)
                    clsPmpaType.ISW[nCNT].GbSelf = clsPmpaType.ISG.GbSelf;

                if (string.Compare(clsPmpaPb.GstrBdate, "2016-09-01") >= 0)
                {
                    if (Check_Change_GbSelf3(pDbCon, clsPmpaType.ISW[nCNT].Sunext, clsPmpaType.IA.Bi) == true)
                    {
                        clsPmpaType.ISW[nCNT].GbSelfSource = clsPmpaType.ISW[nCNT].GbSelf;
                        if (clsPmpaType.ISW[nCNT].GbSelfSource.Trim() == "")
                            clsPmpaType.ISW[nCNT].GbSelfSource = READ_SUGBF2(pDbCon, clsPmpaType.ISW[nCNT].Sunext);
                        clsPmpaType.ISW[nCNT].GbSelf = "0";
                    }
                }

                clsPmpaType.ISW[nCNT].GbHost        = clsPmpaPb.GhostDAEPYO;
                clsPmpaType.ISW[nCNT].GbSlip        = clsPmpaType.ISA[Count].GbTFlag == "" ? " " : clsPmpaType.ISA[Count].GbTFlag;
                clsPmpaType.ISW[nCNT].Amt1          = Convert.ToInt64(clsPmpaPb.G7AMT);
                clsPmpaType.ISW[nCNT].Amt2          = Convert.ToInt64(clsPmpaPb.G7TAMT);
                clsPmpaType.ISW[nCNT].SEQNO         = clsPmpaPb.GnWrtSeqNo;
                clsPmpaType.ISW[nCNT].DrCode        = clsPmpaType.ISA[Count].DrCode;
                clsPmpaType.ISW[nCNT].DeptCode      = clsPmpaType.ISA[Count].DeptCode;
                clsPmpaType.ISW[nCNT].OrderNo       = clsPmpaType.ISA[Count].OrderNo;
                clsPmpaType.ISW[nCNT].RoomCode      = clsPmpaType.ISA[Count].RoomCode;
                clsPmpaType.ISW[nCNT].GbSelNot      = clsPmpaType.ISA[Count].GbSelNot;

                if (string.Compare(clsPmpaPb.GstrBdate, "2016-09-01") >= 0)
                {
                    if (Check_Change_GbSelf(clsPmpaType.ISW[nCNT].SugbS, clsPmpaType.IA.Bi.ToString()) && clsPmpaType.ISW[nCNT].GbSelfSource == "0")
                        clsPmpaType.ISW[nCNT].SugbS = "0";
                    else
                        clsPmpaType.ISW[nCNT].SugbS = clsPmpaType.ISG.SugbS;        //2012-02-01
                }
                else
                {
                    clsPmpaType.ISW[nCNT].SugbS = clsPmpaType.ISG.SugbS;        //2012-02-01
                }

                //수술방 전송구분 2018-10-16
                if (GstrOpFlag == "1")
                {
                    clsPmpaType.ISW[nCNT].GbEr = clsPmpaType.ISG.GBKTAS;  //2018-09-04
                }
                else
                {
                    clsPmpaType.ISW[nCNT].GbEr = clsPmpaPb.GstatER;  //2015-12-30
                }
                //clsPmpaType.ISW[nCNT].GbEr = clsPmpaPb.GstatER;  //2015-12-30
                //clsPmpaType.ISW[nCNT].GbEr          = clsPmpaType.ISG.GBKTAS;  //2018-09-04
                clsPmpaType.ISW[nCNT].GbGSADD       = clsPmpaType.ISG.GSADD;  //2015-12-30
                clsPmpaType.ISW[nCNT].GbASADD       = clsPmpaType.ISG.SugbAG;  //2015-12-30
                clsPmpaType.ISW[nCNT].BCODE         = clsPmpaType.ISG.BCODE;
                clsPmpaType.ISW[nCNT].OPGBN         = clsPmpaType.ISG.OPGBN;
                clsPmpaType.ISW[nCNT].BURN          = clsPmpaType.ISG.BURN;
                clsPmpaType.ISW[nCNT].GBSUGBAC      = clsPmpaType.ISG.SugbAC;           //마취 가산
                clsPmpaType.ISW[nCNT].HIGHRISK      = clsPmpaType.ISG.GbHighRisk;       //산모가산
                clsPmpaType.ISW[nCNT].GBSUGBAB      = clsPmpaType.ISG.SugbAB;           //판독 가산
                clsPmpaType.ISW[nCNT].GBNGT2        = clsPmpaType.ISG.GBNGT2;           //청구용 GBNGT2  2018-09-18

               

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        void Data_Amt_Add()
        {
            int nNu = (int)VB.Val(clsPmpaType.ISG.Nu);
            
            clsPmpaType.IA.Amt[nNu] = Convert.ToInt64(clsPmpaType.IA.Amt[nNu] + clsPmpaPb.G7AMT);
            clsPmpaType.IA.Amt[44] = Convert.ToInt64(clsPmpaType.IA.Amt[44] + clsPmpaPb.G7TAMT);
            
            if (string.Compare(clsPmpaType.ISG.Nu, "50") < 0)
            {
                clsPmpaType.IA.Amt[50] = Convert.ToInt64(clsPmpaType.IA.Amt[50] + clsPmpaPb.G7AMT);
                clsPmpaType.IA.Amt[50] = Convert.ToInt64(clsPmpaType.IA.Amt[50] + clsPmpaPb.G7TAMT);
            }
        }

        void Last_Qmgr_IPD(PsmhDb pDbCon)
        {
            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strRD = string.Empty;

            int nAddQty = 0;

            try
            {
                strRD = "NO";

                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "IPD_QMGR   ";
                SQL += "  WHERE Pano   = '" + clsPmpaType.IA.pano + "'      ";
                SQL += "    AND Sucode = '" + clsPmpaType.ISG.Sucode + "'   ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    IsDataFlag = false;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRD = "OK";
                }

                dt.Dispose();
                dt = null;


                if (strRD == "OK")
                {
                    if (clsPmpaType.ISA[Count].SugbI == "1")
                    {
                        nAddQty = Convert.ToInt32(clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal); 
                    }
                    else
                    {
                        nAddQty = clsPmpaType.ISG.Nal;
                    }

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_QMGR              ";
                    SQL += "    SET MgrQty = MgrQty + " + nAddQty;
                    SQL += "  WHERE Pano   = '" + clsPmpaType.IA.pano + "'      ";
                    SQL += "    AND Sucode = '" + clsPmpaType.ISG.Sucode + "'   ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        IsDataFlag = false;
                        return;
                        
                    }

                    //If result = -1 Then
                    //    Beep
                    //    FrmOrdersSend.SS1.Row = FrmOrdersSend.SS1.DataRowCnt + 1
                    //    FrmOrdersSend.SS1.Col = 1
                    //    FrmOrdersSend.SS1.Text = TIM_Ptno
                    //    FrmOrdersSend.SS1.Col = 2
                    //    FrmOrdersSend.SS1.Text = "전산실"
                    //    FrmOrdersSend.SS1.Col = 3
                    //    FrmOrdersSend.SS1.Text = GstrTime & " " & "IPD_QMGR UPDATE ERROR !" & GstrErrorMessage
                    //End If
                }
                else
                {
                    if (clsPmpaType.ISA[Count].SugbI == "1")
                    {
                        nAddQty = Convert.ToInt32(clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
                    }
                    else
                    {
                        nAddQty = clsPmpaType.ISG.Nal;
                    }

                    SQL = "";
                    SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_QMGR  (  ";
                    SQL += "        PANO, SUCODE, SUNEXT, MgtQty) VALUES (  ";
                    SQL += "        '" + clsPmpaType.IA.pano + "',          ";
                    SQL += "        '" + clsPmpaType.ISG.Sucode + "',       ";
                    SQL += "        '" + clsPmpaType.ISG.Sunext + "',       ";
                    SQL += "        " + nAddQty + " )                       ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        IsDataFlag = false;
                        return;

                    }
                    
                    //If result = -1 Then
                    //    Beep
                    //    FrmOrdersSend.SS1.Row = FrmOrdersSend.SS1.DataRowCnt + 1
                    //    FrmOrdersSend.SS1.Col = 1
                    //    FrmOrdersSend.SS1.Text = TIM_Ptno
                    //    FrmOrdersSend.SS1.Col = 2
                    //    FrmOrdersSend.SS1.Text = "전산실"
                    //    FrmOrdersSend.SS1.Col = 3
                    //    FrmOrdersSend.SS1.Text = GstrTime & " " & "IPD_QMGR INSERT ERROR !" & GstrErrorMessage
                    //End If
                }
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                IsDataFlag = false;
            }

        }

        void BUN_AL201_Account()
        {
            int i = 0;
            int nQty = 0;

            nQty = (int)clsPmpaType.ISG.Qty;
            if (clsPmpaType.ISA[Count].GbTFlag == "T") { return ; }
            if (clsPmpaType.ISG.Nal < 0)
            {
                nQty = nQty * -1;
            }

            for (i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
            {
                clsPmpaPb.G7AL201[i] = clsPmpaPb.G7AL201[i] + nQty;
            }
        }

        void BUN_AL010_Account()
        {
            int i = 0;
            double nQty = 0;

            nQty = (int)clsPmpaType.ISG.Qty;

            if (clsPmpaType.ISA[Count].GbTFlag == "T") { return; }
            if (clsPmpaType.ISG.Qty < 1) { nQty = 1; }
            if (clsPmpaType.ISG.Nal < 0) { nQty = nQty * -1; }

            for (i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
            {
                clsPmpaPb.G7AL010[i] = clsPmpaPb.G7AL010[i] + nQty;
            }
        }

        void BUN_11_Account(PsmhDb pDbCon, int nCNT)
        {
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55)
                BUN_11_TA(pDbCon, nCNT);
            else if (clsPmpaType.IA.Bi > 50 || clsPmpaType.ISG.GbSelf == "1" || string.Compare(clsPmpaType.ISG.SugbF, "0") > 0)
                BUN_11_ILBAN(pDbCon, nCNT);
            else
                BUN_11_BOHUM(pDbCon, nCNT);
        }

        void BUN_11_TA(PsmhDb pDbCon, int nCNT)
        {
            //자보 투약료 계산
            double nQty = 0;
            int i = 0;

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
            
            Slip_Write_Process(pDbCon);

            if (string.Compare(clsPmpaType.ISG.SugbB, "0") > 0 && VB.Val(clsPmpaType.ISG.GbSelf).ToString() == "0" && clsPmpaType.ISA[Count].GbTFlag != "T") //복약지도료 날수누적
            {
                nQty = clsPmpaType.ISG.Qty;
                if (clsPmpaType.ISG.Qty < 1) { nQty = 1; }
                if (clsPmpaType.ISG.Nal < 0) { nQty = nQty * -1; }
                for (i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
                {
                    clsPmpaPb.G7QTY11[i] += nQty;
                }
            }
        }

        void BUN_11_ILBAN(PsmhDb pDbCon, int nCNT)
        {
            //일반 투약료 계산
            double nQty = 0;
            int i = 0;

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            Slip_Write_Process(pDbCon);

            if (clsPmpaType.IA.Bi > 50 && string.Compare(clsPmpaType.ISG.SugbB, "0") > 0 && clsPmpaType.ISA[Count].GbTFlag != "T")    //복약지도료 날수누적
            {
                nQty = clsPmpaType.ISG.Qty;
                if (clsPmpaType.ISG.Qty < 1) { nQty = 1; }
                if (clsPmpaType.ISG.Nal < 0) { nQty = nQty * -1; }
                for (i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
                {
                    clsPmpaPb.G7QTY11[i] += nQty;
                }
            }
        }

        void BUN_11_BOHUM(PsmhDb pDbCon, int nCNT)
        {
            //보험 투약료 계산
            double nQty = 0;
            int i = 0;

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            Slip_Write_Process(pDbCon);

            if (string.Compare(clsPmpaType.ISG.SugbB, "0") > 0 && VB.Val(clsPmpaType.ISG.GbSelf).ToString() == "0" && clsPmpaType.ISA[Count].GbTFlag != "T") //복약지도료 날수누적
            {
                nQty = clsPmpaType.ISG.Qty;
                if (clsPmpaType.ISG.Qty < 1) { nQty = 1; }
                if (clsPmpaType.ISG.Nal < 0) { nQty = nQty * -1; }
                for (i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
                {
                    clsPmpaPb.G7QTY11[i] += nQty;
                }
            }
        }

        void BUN_20_Account(PsmhDb pDbCon)
        {
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55)
                BUN_20_TA(pDbCon);
            else if (clsPmpaType.IA.Bi > 50 || clsPmpaType.ISG.GbSelf == "1")
                BUN_20_ILBAN(pDbCon);
            else
                BUN_20_BOHUM(pDbCon);
        }

        void BUN_20_ILBAN(PsmhDb pDbCon)
        {
            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
            Slip_Write_Process(pDbCon);
        }

        void BUN_20_TA(PsmhDb pDbCon)
        {
            clsBasAcct cBAcct = new clsBasAcct();
            string strC = string.Empty;
            string strF = string.Empty;

            #region 이전기준
            ////2015-12-28
            //if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            //{ 
            //    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
            //    {
            //        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
            //        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
            //        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //     }
            //}
            //else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            //{ 
            //    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
            //    {
            //        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
            //        if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //    }
            //} 
            #endregion

            #region 바뀐기준
            if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            {
                if (clsPmpaType.IA.Age == 0)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (150 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
                else if (clsPmpaType.IA.Age >= 1 && clsPmpaType.IA.Age < 6)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (130 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
            }
            else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            {
                if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                }
            }
            #endregion

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
                
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            if (clsPmpaPb.G7AMT != 0)
            {
                Slip_Write_Process(pDbCon);
            }
            else
            {
                if (GstrOpFlag == "1")      //수술실 일때
                {
                    if (clsPmpaType.ISG.Sucode != null)
                    {
                        if (clsPmpaType.ISG.Sucode == "PRS2")
                        {
                            ComFunc.MsgBox("BUN_20_TA : " + clsPmpaType.ISG.Sucode);
                        }
                    }
                }
            }

            strC = clsPmpaType.ISG.SugbC;

            switch (clsPmpaType.ISG.SugbB)
            {
                case "4":
                case "5":
                case "6": B20_TA_FLUID(pDbCon); break;    //수액주사 수기료
                case "9": B20_DEPO(pDbCon);  break;    //관절강내 주사수기료
                default:
                    break;
            }

            clsPmpaType.ISG.SugbC = strC;
            if (string.Compare(clsPmpaType.ISG.SugbC, "0") > 0) { B20_AQUA(pDbCon); }        //증류수 발생

            return;
        }

        void B20_TA_FLUID(PsmhDb pDbCon)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsBasAcct cBAcct = new clsBasAcct();
            double nQty = clsPmpaType.ISG.Qty;
            int nNal = clsPmpaType.ISG.Nal;

            if (VB.Left(clsPmpaType.ISG.Sucode, 2).Trim() == "C-")
            {
                switch (clsPmpaType.ISG.SugbB)
                {
                    case "4":
                        if (cPF.Suga_Read(pDbCon, "KK152") == false)
                            IsDataFlag = false;
                        break;
                    case "5":
                        if (cPF.Suga_Read(pDbCon, "KK153") == false)
                            IsDataFlag = false;
                        break;
                    default:
                        if (cPF.Suga_Read(pDbCon, "KK154") == false)
                            IsDataFlag = false;
                        break;
                }
            }
            else
            {
                switch (clsPmpaType.ISG.SugbB)
                {
                    case "4":
                        if (cPF.Suga_Read(pDbCon, "KK051") == false)
                            IsDataFlag = false;
                        break;
                    case "5":
                        if (cPF.Suga_Read(pDbCon, "KK052") == false)
                            IsDataFlag = false;
                        break;
                    default:
                        if (cPF.Suga_Read(pDbCon, "KK053") == false)
                            IsDataFlag = false;
                        break;
                }
            }
            
            
            Move_RS_TO_ISG();

            clsPmpaPb.G7TAMT = 0;
            clsPmpaType.ISG.GbNgt = "0";
            clsPmpaType.ISG.GbSpc = "0";
            clsPmpaPb.GhostDAEPYO = "3";

            clsPmpaType.ISG.Qty = nQty;
            clsPmpaType.ISG.Nal = nNal;

            if (clsPmpaType.ISG.Qty < 1) { clsPmpaType.ISG.Qty = 1; }

            #region 이전기준
            //if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
            //    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //}
            //else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //} 
            #endregion

            #region 바뀐 기준
            if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            {
                if (clsPmpaType.IA.Age == 0)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (150 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
                else if (clsPmpaType.IA.Age >= 1 && clsPmpaType.IA.Age < 6)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (130 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
            }
            else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            {
                if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                }
            } 
            #endregion
            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            if (clsPmpaPb.G7AMT != 0)
            {
                Slip_Write_Process(pDbCon);
            }
            else
            {
                if (GstrOpFlag == "1")      //수술실 일때
                {
                    if (clsPmpaType.ISG.Sucode != null)
                    {
                        if (clsPmpaType.ISG.Sucode == "PRS2")
                        {
                            ComFunc.MsgBox("B20_TA_FLUID : " + clsPmpaType.ISG.Sucode);
                        }
                    }
                }
            }
        }

        void B20_DEPO(PsmhDb pDbCon)
        {
            double nQty = clsPmpaType.ISG.Qty;
            int nNal = clsPmpaType.ISG.Nal;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsBasAcct cBAcct = new clsBasAcct();

            if (cPF.Suga_Read(pDbCon, "KK090") == false)
                IsDataFlag = false;
            
            Move_RS_TO_ISG();
            
            clsPmpaPb.G7TAMT = 0;
            clsPmpaType.ISG.GbNgt = "0";
            clsPmpaType.ISG.GbSpc = "0";
            clsPmpaPb.GhostDAEPYO = "3";

            clsPmpaType.ISG.Qty = nQty;
            clsPmpaType.ISG.Nal = nNal;

            if (clsPmpaType.ISG.Qty < 1) { clsPmpaType.ISG.Qty = 1; }

            #region 이전 기준
            //if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
            //    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //}
            //else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //}
            #endregion

            #region 바뀐 기준
            if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            {
                if (clsPmpaType.IA.Age == 0)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (150 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
                else if (clsPmpaType.IA.Age >= 1 && clsPmpaType.IA.Age < 6)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (130 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
            }
            else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            {
                if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                }
            } 
            #endregion

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            if (clsPmpaPb.G7AMT != 0)
            {
                Slip_Write_Process(pDbCon);
            }
            else
            {
                if (GstrOpFlag == "1")      //수술실 일때
                {
                    if (clsPmpaType.ISG.Sucode != null)
                    {
                        if (clsPmpaType.ISG.Sucode == "PRS2")
                        {
                            ComFunc.MsgBox("B20_DEPO : " + clsPmpaType.ISG.Sucode);
                        }
                    }
                }
            }
        }

        void B20_AQUA(PsmhDb pDbCon)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();

            double nQty = clsPmpaType.ISG.Qty;
            int nNal = clsPmpaType.ISG.Nal;

            string SGSugbC = string.Empty;
            
            switch (clsPmpaType.ISG.SugbC)
            {
                case "1":
                    if (cPF.Suga_Read(pDbCon, "WT5") == false)
                        IsDataFlag = false;
                    Move_RS_TO_ISG(); break;    // 5cc 증류수
                case "2":
                    if (cPF.Suga_Read(pDbCon, "WT10") == false)
                        IsDataFlag = false;
                    Move_RS_TO_ISG(); break;    // 3cc 증류수
                case "3":
                    if (cPF.Suga_Read(pDbCon, "WT20") == false)
                        IsDataFlag = false;
                    Move_RS_TO_ISG(); break;    //20cc 증류수
                case "7":
                    if (cPF.Suga_Read(pDbCon, "NS10") == false)
                        IsDataFlag = false;
                        Move_RS_TO_ISG(); break;    //20cc 증류수
                case "8":   //2021-11-11
                    SGSugbC = clsPmpaType.ISG.SugbC;
                    if (cPF.Suga_Read(pDbCon, "NS10") == false)
                        IsDataFlag = false;
                    Move_RS_TO_ISG();     //20cc 증류수
                    clsPmpaType.ISG.SugbC = SGSugbC;
                    break;

                default:
                    SGSugbC = clsPmpaType.ISG.SugbC;
                    if (cPF.Suga_Read(pDbCon, "NSA") == false)
                        IsDataFlag = false;
                    Move_RS_TO_ISG();
                    clsPmpaType.ISG.SugbC = SGSugbC;
                    break;
            }
            
            clsPmpaPb.G7TAMT = 0;
            clsPmpaType.ISG.GbNgt = "0";
            clsPmpaType.ISG.GbSpc = "0";
            clsPmpaPb.GhostDAEPYO = "3";
            if (clsPmpaType.IA.Bi == 52)
            {
                clsPmpaType.ISG.GbSelf = "0";
                clsPmpaType.ISG.SugbF = "0";
            }
            switch (clsPmpaType.ISG.SugbC)
            {
                case "1":
                case "2":
                case "3": clsPmpaType.ISG.Qty = nQty;        break; // WT5,WT10,WT20
                case "4": clsPmpaType.ISG.Qty = nQty * 0.25; break; // 5ccNSA
                case "5": clsPmpaType.ISG.Qty = nQty * 0.5 ; break; // 10ccNSA
                case "6": clsPmpaType.ISG.Qty = nQty * 1;    break; // 20ccNSA
                case "8": clsPmpaType.ISG.Qty = nQty * 2;    break; // 20ccNSA  //2021-11-11 C항이 8일 경우 *2
                default:                                            // 기타
                    clsPmpaType.ISG.Qty = nQty;
                    break;
            }

            clsPmpaType.ISG.Nal = nNal;
            if (clsPmpaType.ISG.Qty < 1) { clsPmpaType.ISG.Qty = 1; }

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (clsPmpaPb.G7AMT != 0)
            {
                Slip_Write_Process(pDbCon);
            }
            else
            {
                if (GstrOpFlag == "1")      //수술실 일때
                {
                    if (clsPmpaType.ISG.Sucode != null)
                    {
                        if (clsPmpaType.ISG.Sucode == "PRS2")
                        {
                            ComFunc.MsgBox("B20_AQUA : " + clsPmpaType.ISG.Sucode);
                        }
                    }
                }
            }
        }

        void BUN_20_BOHUM(PsmhDb pDbCon)
        {
            string strC = string.Empty;
            string strF = string.Empty;
            
            try
            {
                clsBasAcct cBAcct = new clsBasAcct();

                //2015-12-28
                #region 이전 기준
                //if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
                //{
                //    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                //    {
                //        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                //        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                //        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                //    }
                //}
                //else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
                //{
                //    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                //    {
                //        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
                //        if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                //    }
                //} 
                #endregion

                #region 바뀐 기준
                if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
                {
                    if (clsPmpaType.IA.Age == 0)
                    {
                        if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                        {
                            //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                            clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (150 / 100.0));
                            clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                            if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                        }
                    }
                    else if (clsPmpaType.IA.Age >= 1 && clsPmpaType.IA.Age < 6)
                    {
                        if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                        {
                            //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                            clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (130 / 100.0));
                            clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                            if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                        }
                    }
                }
                else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
                #endregion

                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    else
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }

                //'NP 정액은 처방 전송
                if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.IA.Dept == "NP")
                {
                    Slip_Write_Process(pDbCon);
                }
                else
                {
                    if (clsPmpaPb.G7AMT != 0)
                    {
                        Slip_Write_Process(pDbCon);
                    }
                    else
                    {
                        if (GstrOpFlag == "1")      //수술실 일때
                        {
                            if (clsPmpaType.ISG.Sucode != null)
                            {
                                if (clsPmpaType.ISG.Sucode == "PRS2")
                                {
                                    ComFunc.MsgBox("BUN_20_BOHUM : " + clsPmpaType.ISG.Sucode);
                                }
                            }
                        }
                    }
                }


                strC = clsPmpaType.ISG.SugbC;
                strF = clsPmpaType.ISG.SugbF; //jjy(2003-03-17) 추가
                FnQty = clsPmpaType.ISG.Qty;
                FnNal = clsPmpaType.ISG.Nal;

                switch (clsPmpaType.ISG.SugbB)
                {
                    case "1":
                        B20_BO_IMADD();
                        B20_IM_WRITE(pDbCon);
                        break;      //수량 ADD (IM)
                    case "2": B20_BO_IVADD(); break;      //수량 ADD (IV)
                    case "4":
                    case "5":
                    case "6": B20_BO_FLUID(pDbCon, strF); break;  //수액주사 수기료
                    case "9": B20_DEPO(pDbCon); break;      //관절강내 주사수기료
                    default:
                        break;
                }

                clsPmpaType.ISG.SugbC = strC;
                
                if (string.Compare(clsPmpaType.ISG.SugbC, "0") > 0) { B20_AQUA(pDbCon); }        //증류수 발생
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void SUGA_READ_JUSA(PsmhDb pDbCon)  //'주사수기료 READ(2001-12-07일부터적용)
        {
            try
            {
                //'기본으로 사용되는 주사료만자동 발생되도록 처리 되었습니다.
                string strKKCode = "";
                string strCancel = "";

                clsPmpaFunc cFP = new clsPmpaFunc();

                //JUSA_CHECK(pDbCon, strKKCode);

                strCancel = "NO";
                strKKCode = "";

                if (VB.Left(clsPmpaType.ISG.Sunext, 2) == "C-") { strCancel = "OK"; } //항암제 주사

                switch (clsPmpaType.ISG.SugbB)
                {
                    case "1":   // 'IM 수기료
                        if (strCancel == "OK")
                        {
                            strKKCode = "KK156";
                        }
                        else
                        {
                            strKKCode = "KK010";
                        }
                        break;
                    case "2":               //IV 수기료
                    case "3":
                        if (strCancel == "OK")
                        {
                            strKKCode = "KK151";
                            JUSA_CHECK(pDbCon, strKKCode);
                        }
                        else
                        {
                            Jusa_Find(pDbCon, ref strKKCode);
                            JUSA_CHECK(pDbCon, strKKCode);
                        }
                        break;
                    case "4":
                    case "5":
                    case "6":
                        if (strCancel == "OK")       //수액주사 수기료
                        {
                            switch (clsPmpaType.ISG.SugbB)
                            {
                                case "4":
                                    strKKCode = "KK152";
                                    break;
                                case "5":
                                    strKKCode = "KK153";
                                    break;
                                default:
                                    strKKCode = "KK154";
                                    break;
                            }
                        }
                        else
                        {
                            switch (clsPmpaType.ISG.SugbB)
                            {
                                case "4":
                                    strKKCode = "KK051";
                                    break;
                                case "5":
                                    strKKCode = "KK052";
                                    break;
                                default:
                                    strKKCode = "KK053";
                                    break;
                            }
                        }
                        break;
                    case "9":
                        strKKCode = "KK090";      //관절강내 주사수기료
                        break;
                    default:
                        break;
                }

                if (strKKCode != "")
                {
                    cFP.Suga_Read(pDbCon, strKKCode);
                    Move_RS_TO_ISG();
                }

                cFP = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        void Jusa_Find(PsmhDb pDbCon, ref string strKKCode) //병당 수기료가 있는지 체크
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += " SELECT SUM(A.QTY * A.NAL) nCnt                                         \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER A,                              \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUT B                                 \r\n";
                SQL += " WHERE A.PTNO ='" + clsPmpaType.IA.pano + "'                            \r\n";
                SQL += "   AND A.BDATE = TO_DATE('" + clsPmpaPb.GstrBdate + "','YYYY-MM-DD')    \r\n";
                SQL += "   AND A.BI = '" + clsPmpaType.IA.Bi.ToString() + "'                    \r\n";
                SQL += "   AND A.BUN ='20'                                                      \r\n";
                SQL += "   AND A.SUCODE = B.SUNEXT                                              \r\n";
                SQL += "   AND B.SUGBB IN('3','4','5')                                          \r\n";   //병당수기료 발생되는 코드            
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["nCNT"].ToString()) == 0)
                    {
                        strKKCode = "KK020";
                    }
                    else
                    {
                        strKKCode = "KK054";
                    }
                }
                else
                {
                    strKKCode = "KK020";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        void JUSA_CHECK(PsmhDb pDbCon, string strKKCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strCheck = "";
            int nCNT = 0;

            try
            {
                FstrCheck = "OK";

                SQL = "";
                SQL += " SELECT SUNEXT, SUM(QTY * NAL) nCnt                                 \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                          \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.IA.pano + "'                        \r\n";
                SQL += "    AND BDATE = TO_DATE('" + clsPmpaPb.GstrBdate + "','YYYY-MM-DD') \r\n";
                SQL += "    AND SUCODE = '" + strKKCode + "'                                \r\n";  //수기료 코드
                SQL += "    AND BI = '" + clsPmpaType.IA.Bi.ToString() + "'                 \r\n";
                SQL += "  GROUP BY SUNEXT                                                   \r\n";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nCNT = 0;
                if (dt.Rows.Count > 0)
                {
                    nCNT = (int)VB.Val(dt.Rows[0]["nCnt"].ToString());
                }

                if (clsPmpaType.ISW[0].Sucode != null)
                {
                    for (int j = 0; j < clsPmpaType.ISW.Length; j++)
                    {
                        if (clsPmpaType.ISW[j].Sucode.ToString().Trim() == strKKCode) { nCNT = nCNT + 1; }
                    }
                }
                

                if (nCNT == 0)
                {
                    FstrCheck = "OK";
                }
                else if ((strKKCode == "KK151" || strKKCode == "KK020") && nCNT >= 1) //1일당
                {
                    FstrCheck = "NO";
                }
                else if (strKKCode == "KK054")
                {
                    if (clsPmpaType.IA.Bi == 52 && nCNT >= 4)       //자보는 1일당 4회까지만.
                    {
                        FstrCheck = "NO";
                    }
                    else if (nCNT >= 2)                    //보험은 1일당 2회까지만.
                    {
                        FstrCheck = "NO";
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void B20_IM_WRITE(PsmhDb pDbCon)
        {
            try
            {
                SUGA_READ_JUSA(pDbCon);

                clsPmpaPb.G7TAMT = 0;
                clsPmpaType.ISG.GbNgt = "0";
                clsPmpaType.ISG.GbSpc = "0";

                clsPmpaPb.GhostDAEPYO = "3";

                clsPmpaType.ISG.Qty = FnQty;
                clsPmpaType.ISG.Nal = FnNal;

                if (clsPmpaType.ISG.Nal < 0)
                    clsPmpaType.ISG.Qty = 0;
                else
                    clsPmpaType.ISG.Qty = 1;

                //보호 정신과는 금액발생 없습니다.
                if (clsPmpaType.ISG.GbSelf == "0" && (clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22) && (clsPmpaType.IA.Dept == "NP" || clsPmpaType.IA.Dept == "HD"))
                    clsPmpaType.ISG.BaseAmt = 0;


                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    else
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }

                if (clsPmpaType.ISG.GbSelf == "0" && (clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22) && (clsPmpaType.IA.Dept == "NP"))
                {
                    Slip_Write_Process(pDbCon); //변수 STRCHEK 는Jusa_Check에서 체크 단가 0도 발생함 심사과 요청
                }
                else
                {
                    if (clsPmpaPb.G7AMT != 0 && FstrCheck == "OK")
                    {
                        Slip_Write_Process(pDbCon); //변수 STRCHEK 는Jusa_Check에서 체크
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        
        void B20_BO_IMADD()
        {
            double nQTY = 0;

            nQTY = clsPmpaType.ISG.Qty;
            if (clsPmpaType.ISG.Nal < 0) { nQTY = nQTY * -1; }

            for (int i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
            {
                clsPmpaPb.G7QTY20A[i] = clsPmpaPb.G7QTY20A[i] + nQTY;
            }
        }

        void B20_BO_IVADD()
        {
            double nQty = 0;

            if (clsPmpaType.IA.Dept == "PD" || clsPmpaType.IA.Age < 4)
            {
                B20_BO_IMADD();
                return;
            }

            if (clsPmpaPb.GstrB20STAT == "OK")
            {
                B20_BO_IMADD();
                return;
            }

            nQty = clsPmpaType.ISG.Qty;
            if (clsPmpaType.ISG.Nal < 0) { nQty = nQty * -1; }

            for (int i = 0; i < Math.Abs(clsPmpaType.ISG.Nal); i++)
            {
                clsPmpaPb.G7QTY20B[i] = clsPmpaPb.G7QTY20B[i] + nQty;
            }
            
        }

        void B20_BO_FLUID(PsmhDb pDbCon, string strF)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsBasAcct cBAcct = new clsBasAcct();

            double nQty = clsPmpaType.ISG.Qty;
            int nNal = clsPmpaType.ISG.Nal;

            if (VB.Left(clsPmpaType.ISG.Sucode, 2).Trim() == "C-")
            {
                switch (clsPmpaType.ISG.SugbB)
                {
                    case "4":
                        if (cPF.Suga_Read(pDbCon, "KK152") == false)
                            IsDataFlag = false;
                        break;
                    case "5":
                        if (cPF.Suga_Read(pDbCon, "KK153") == false)
                            IsDataFlag = false;
                        break;
                    default:
                        if (cPF.Suga_Read(pDbCon, "KK154") == false)
                            IsDataFlag = false;
                        break;
                }
            }
            else
            {
                switch (clsPmpaType.ISG.SugbB)
                {
                    case "4":
                        if (cPF.Suga_Read(pDbCon, "KK051") == false)
                            IsDataFlag = false;
                        break;
                    case "5":
                        if (cPF.Suga_Read(pDbCon, "KK052") == false)
                            IsDataFlag = false;
                        break;
                    default:
                        if (cPF.Suga_Read(pDbCon, "KK053") == false)
                            IsDataFlag = false;
                        break;
                }
            }
            
            Move_RS_TO_ISG();

            clsPmpaType.ISG.SugbF = strF;
            clsPmpaPb.G7TAMT = 0;
            clsPmpaType.ISG.GbNgt = "0";
            clsPmpaType.ISG.GbSpc = "0";
            clsPmpaPb.GhostDAEPYO = "3";

            clsPmpaType.ISG.Qty = nQty;
            clsPmpaType.ISG.Nal = nNal;

            if (clsPmpaType.ISG.Qty < 1) { clsPmpaType.ISG.Qty = 1; }

            #region 이전 기준
            //if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
            //    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //}
            //else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            //{
            //    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
            //    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
            //}
            #endregion

            #region 바뀐기준
            if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            {
                if (clsPmpaType.IA.Age == 0)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (150 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
                else if (clsPmpaType.IA.Age >= 1 && clsPmpaType.IA.Age < 6)
                {
                    if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                    {
                        //clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                        clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (130 / 100.0));
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                    }
                }
            }
            else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            {
                if ((clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18" || clsPmpaType.ISG.Bun == "19") && clsPmpaType.ISG.Sucode.Trim() != "KK041")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }
                }
            }
            #endregion

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            if (clsPmpaPb.G7AMT != 0)
            {
                Slip_Write_Process(pDbCon);
            }
            else
            {
                if (GstrOpFlag == "1")      //수술실 일때
                {
                    if (clsPmpaType.ISG.Sucode != null)
                    {
                        if (clsPmpaType.ISG.Sucode == "PRS2")
                        {
                            ComFunc.MsgBox("B20_BO_FLUID : " + clsPmpaType.ISG.Sucode);
                        }
                    }
                }
            }

        }

        void BUN_22_Account(PsmhDb pDbCon)
        {
            string strNgt = string.Empty;
            string strPCode = string.Empty;
            int nsAdd1 = 0;
            int nNgt = 0, nOldNgt = 0;

            clsBasAcct cBAcct       = new clsBasAcct();
            clsIuSentChk cISentChk  = new clsIuSentChk();
            clsPmpaFunc cPF         = new clsPmpaFunc();

            clsPmpaType.cBas_Add_Arg cBArg = new clsPmpaType.cBas_Add_Arg();
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();

            try
            {
               
                strPCode = cISentChk.Rtn_Bas_Sun_BCode(pDbCon, clsPmpaType.ISG.Sunext, clsPmpaType.IA.Date);

                //이미 가산된 코드이거나 인정비급여, 병원 임의수가 인경우 기존대로 금액 설정
                //마취, 치료 재료대 인경우 기존금액 사용
                if (strPCode == "JJJJJJ" || strPCode == "999999" || clsPmpaType.ISG.SugbP == "1" || clsPmpaType.ISG.Bun == "23"  || Convert.ToInt16(clsPmpaType.TIT.Bi) > 50 )
                {
                    #region 기존 AS-IS 가산내용
                    //개두 ,일측 ,개흉마취일경우
                    switch (clsPmpaType.ISG.SugbAC)
                    {
                        case "1":
                            nsAdd1 = 50; break;
                        case "2":
                            nsAdd1 = 50; break;
                        case "3":
                            nsAdd1 = 50; break;
                        default:
                            break;
                    }
                    //VB6 야간/심야 소아/노인 가산 부분 고도화 하면서 현재 누락되어 있음(2021-08-06)
                    //해당 루틴은 EDI코드가 없는 경우 포함되게 되어 있는데. 50종 이상일 경우도 타게 되어 있음. 
                    //ㅆ.
                    //switch (clsPmpaType.ISG.GbNgt)
                    //{
                    //    case "1":
                    //    case "4":
                    //    case "7":  //야간 일반, 야간 소아/노인, 야간 신생아
                    //        nOldNgt = clsPmpaPb.OLD_NIGHT_22[1];
                    //        nNgt    = clsPmpaPb.NIGHT_22[1];
                    //        break;
                    //    case "2":
                    //    case "5":
                    //    case "8":  //심야공휴 일반, 심야공휴 소아/노인, 심야공휴 신생아
                    //        nOldNgt = clsPmpaPb.OLD_NIGHT_22[2];
                    //        nNgt    = clsPmpaPb.NIGHT_22[2];
                    //        break;
                    //    case "3":
                    //    case "6":   //마취 소아/노인, 신생아
                    //        nOldNgt = 100;
                    //        nNgt    = 100;
                    //        break;
                    //    default:        //기타
                    //        nOldNgt = 100;
                    //        nNgt    = 100;
                    //        break;       
                    //}

                    #region 가산항목 세팅
                    cBArg           = new clsPmpaType.cBas_Add_Arg();
                    cBArg.AGE       = Convert.ToInt16(clsPmpaType.IA.Age);
                    cBArg.AGEILSU   = ComFunc.AgeCalcEx_Zero(clsPmpaType.IA.Jumin1 + clsPmpaType.IA.Jumin2, clsPmpaType.IA.Date);
                    cBArg.SUNEXT    = clsPmpaType.ISG.Sunext;           //수가코드
                    cBArg.BUN       = clsPmpaType.ISG.Bun;              //수가분류
                    cBArg.SUGBE     = clsPmpaType.ISG.SugbE;            //수가 E항(기술료)
                    cBArg.Bi        = clsPmpaType.IA.Bi1;               //자격
                    cBArg.BDATE     = clsPmpaType.IA.Date;              //처방일자
                    if (GstrOpFlag == "1")      //수술실 일때
                    {
                        cBArg.GBER  = clsPmpaType.ISG.GBKTAS;         //응급 가산
                    }
                    else
                    {
                        if ((clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                        {
                            cBArg.GBER = clsPmpaType.IA.KTASLEVEL;         //응급 가산
                        }
                    }
                    cBArg.NIGHT     = clsPmpaType.ISG.GbNgt;            //공휴, 야간 가산
                    if (clsPmpaType.ISG.GbNgt == "3")
                    {
                        clsPmpaType.ISG.GbNgt = "2";
                        cBArg.NIGHT = "2";
                    }
                    cBArg.AN1       = clsPmpaType.ISG.SugbAC;           //마취 가산
                    if (clsPmpaType.ISG.ASADD != "1" || clsPmpaType.ISG.SugbAG != "1") { clsPmpaType.ISG.SugbAG = ""; }

                    cBArg.AN2 = clsPmpaType.ISG.SugbAG;            //ASA마취 가산
                    
                    cBArg.OP1       = clsPmpaType.ISG.GSADD;            //외과 / 흉부외과 가산
                    clsPmpaType.ISG.BURN = clsPmpaType.ISG.SugbAD;
                    cBArg.OP2       = clsPmpaType.ISG.BURN;             //화상 가산   
                    //cBArg.OP2       = clsPmpaType.ISG.SugbAD;             //화상 가산           
                    if (clsPmpaType.ISG.OPGBN == "D")
                    {
                        cBArg.OP3 = "";            //수술, 부수술, 제2수술   
                    }
                    else
                    {
                        cBArg.OP3 = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
                    }
                    cBArg.OP4       = clsPmpaType.ISG.GbHighRisk;       //산모가산
                    cBArg.XRAY1     = clsPmpaType.ISG.SugbAB;           //판독 가산
                    #endregion

                    // 청구용 GBNGT2 컬럼정리
                    string strHang = cBAcct.Bas_Acct_Hang_Set(cBArg.BUN);
                    bool bOG = cBAcct.Rtn_OG_Suga(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //분만수가여부
                    clsPmpaType.ISG.GBNGT2 = cPF.Convert_GbNgt2(cBArg, strHang, bOG);

                    if (clsPmpaType.IA.Bi == 52)
                    {
                        //52종은 GBNGT2 로 되도록
                        switch (clsPmpaType.ISG.GBNGT2)
                        {
                            case "1":
                            case "4":
                            case "7":  //야간 일반, 야간 소아/노인, 야간 신생아
                                nOldNgt = clsPmpaPb.OLD_NIGHT_22[1];
                                nNgt = clsPmpaPb.NIGHT_22[1];
                                break;
                            case "2":
                            case "5":
                            case "8":  //심야공휴 일반, 심야공휴 소아/노인, 심야공휴 신생아
                                nOldNgt = clsPmpaPb.OLD_NIGHT_22[2];
                                nNgt = clsPmpaPb.NIGHT_22[2];
                                break;
                            case "3":
                            case "6":   //마취 소아/노인, 신생아
                                nOldNgt = 100;
                                nNgt = 100;
                                break;
                            default:        //기타
                                nOldNgt = 100;
                                nNgt = 100;
                                break;
                        }
                    }
                    else
                    {
                        //기존 자격은 그대로...
                        switch (clsPmpaType.ISG.GbNgt)
                        {
                            case "1":
                            case "4":
                            case "7":  //야간 일반, 야간 소아/노인, 야간 신생아
                                nOldNgt = clsPmpaPb.OLD_NIGHT_22[1];
                                nNgt = clsPmpaPb.NIGHT_22[1];
                                break;
                            case "2":
                            case "5":
                            case "8":  //심야공휴 일반, 심야공휴 소아/노인, 심야공휴 신생아
                                nOldNgt = clsPmpaPb.OLD_NIGHT_22[2];
                                nNgt = clsPmpaPb.NIGHT_22[2];
                                break;
                            case "3":
                            case "6":   //마취 소아/노인, 신생아
                                nOldNgt = 100;
                                nNgt = 100;
                                break;
                            default:        //기타
                                nOldNgt = 100;
                                nNgt = 100;
                                break;
                        }
                    }

                    //clsPmpaType.ISG.BCODE = "";
                    if (clsPmpaType.ISG.Bun == "23")
                    {
                        clsPmpaType.ISG.GbNgt = "0";
                        clsPmpaType.ISG.GBNGT2 = "0";
                    }

                    BUN_22_BOHUM(pDbCon, nsAdd1, nNgt, nOldNgt);

                    #endregion
                }
                else
                {
                    #region 마취 가산부분 원복 EDI 수가에서 가산조건에 나이 포함되어있음
                    //신경차단술 경우는 소아 노인가산 않함
                    //신경차단술 이외 수가는 GBNGT 에서 가산구분을 해주었으므로 원복해준다
                    if (clsPmpaType.ISG.GBNS != "Y")
                    {
                        if (clsPmpaType.IA.Age < 6 || clsPmpaType.IA.Age > 69)
                        {
                            if (clsPmpaType.IA.Age == 0 && clsPmpaType.IA.AgeiLsu <= 30)
                            {
                                if (VB.Val(clsPmpaType.ISG.GbNgt) >= 6)
                                {
                                    clsPmpaType.ISG.GbNgt = (VB.Val(clsPmpaType.ISG.GbNgt) - 6).ToString();
                                }
                                else if (VB.Val(clsPmpaType.ISG.GbNgt) >= 3)
                                {
                                    //2015-02-05 아래 두코드는 노인,소아가산 안함 (의뢰서작업)
                                    if (clsPmpaType.ISG.Sunext.Trim() != "L1211900" && clsPmpaType.ISG.Sunext.Trim() != "L1211600" && clsPmpaType.ISG.Sunext.Trim() != "L1211800")
                                        clsPmpaType.ISG.GbNgt = (VB.Val(clsPmpaType.ISG.GbNgt) - 3).ToString();                             
                                }
                            }
                        }
                    }

                    //2015-09-21 AP601 수가에 가산 안물리도록 조치 수녀님과 통화
                    if (clsPmpaType.ISG.Sunext.Trim() != "AP601" && (VB.Val(clsPmpaType.ISG.GbNgt) >= 3))
                    {
                        clsPmpaType.ISG.GbNgt = "0";
                    }
                    #endregion

                    #region 가산항목 세팅
                    cBArg           = new clsPmpaType.cBas_Add_Arg();
                    cBArg.AGE       = Convert.ToInt16(clsPmpaType.IA.Age);
                    cBArg.AGEILSU   = ComFunc.AgeCalcEx_Zero(clsPmpaType.IA.Jumin1 + clsPmpaType.IA.Jumin2, clsPmpaType.IA.Date);
                    cBArg.SUNEXT    = clsPmpaType.ISG.Sunext;           //수가코드
                    cBArg.BUN       = clsPmpaType.ISG.Bun;              //수가분류
                    cBArg.SUGBE     = clsPmpaType.ISG.SugbE;            //수가 E항(기술료)
                    
                    cBArg.Bi        = clsPmpaType.IA.Bi1;               //자격
                    cBArg.BDATE     = clsPmpaType.IA.Date;              //처방일자
                    if (GstrOpFlag == "1")      //수술실 일때
                    {
                        cBArg.GBER  = clsPmpaType.ISG.GBKTAS;         //응급 가산                       
                    }
                    else
                    {
                        if ((clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                        {
                            cBArg.GBER = clsPmpaType.IA.KTASLEVEL;         //응급 가산
                        }
                    }
                    cBArg.NIGHT     = clsPmpaType.ISG.GbNgt;            //공휴, 야간 가산
                    if (clsPmpaType.ISG.GbNgt == "3")
                    {
                        clsPmpaType.ISG.GbNgt = "2";
                        cBArg.NIGHT = "2";
                    }
                    cBArg.AN1       = clsPmpaType.ISG.SugbAC;           //마취 가산

                    if (clsPmpaType.ISG.ASADD != "1" || clsPmpaType.ISG.SugbAG != "1") { clsPmpaType.ISG.SugbAG = ""; }

                    cBArg.AN2       = clsPmpaType.ISG.SugbAG;            //ASA마취 가산
                    cBArg.OP1       = clsPmpaType.ISG.GSADD;            //외과 / 흉부외과 가산
                    clsPmpaType.ISG.BURN = clsPmpaType.ISG.SugbAD;
                    cBArg.OP2       = clsPmpaType.ISG.BURN;             //화상 가산    
                    if (clsPmpaType.ISG.OPGBN == "D")
                    {
                        cBArg.OP3 = "";            //수술, 부수술, 제2수술   
                    }
                    else
                    {
                        cBArg.OP3 = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
                    }
                    cBArg.OP4       = clsPmpaType.ISG.GbHighRisk;       //산모가산
                    cBArg.XRAY1     = clsPmpaType.ISG.SugbAB;           //판독 가산
                    #endregion
                    
                    //EDI 수가 금액 
                    cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

                    clsPmpaType.ISG.BaseAmt = cBAR.BASEAMT;
                    clsPmpaType.ISG.BCODE   = cBAR.PCODE;

                    //clsPmpaPb.G7AMT         = cBAR.AMT;
                    //clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaPb.G7AMT * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                    #region 마취 단수 계산
                    if (clsPmpaType.ISG.SugbG != "4")       //시간, 분 단수가 아닌 수가
                    {
                        clsPmpaPb.G7AMT = (long)Math.Truncate(cBAR.AMT * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
                    }
                    else
                    {
                        if (cBArg.BUN == "23" || cBArg.SUGBE == "0")
                        {
                            //마취 재료대 계산
                            cBAR.AMT = cBAcct.BAS_MACH_AMT(2, cBArg.SUNEXT, cBAR.BASEAMT, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
                        }
                        else
                        {
                            //마취 행위료 계산
                            cBAR.AMT = cBAcct.BAS_MACH_AMT(1, cBArg.SUNEXT, cBAR.BASEAMT, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
                        }

                        if (clsPmpaType.ISG.Nal < 0)
                        {
                            cBAR.AMT = cBAR.AMT * -1;
                        }

                        #region 기술료 가산
                        // 기술료가산c
                        if (cBArg.SUGBE == "1")
                        {
                            if (string.Compare(cBArg.BDATE, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[cBArg.Bi] > 0)
                            {
                                clsPmpaPb.G7AMT = (long)Math.Truncate(cBAR.AMT * (clsPmpaPb.OLD_GISUL[cBArg.Bi] / 100.0));       //발생금액
                            }
                            else
                            {
                                clsPmpaPb.G7AMT = (long)Math.Truncate(cBAR.AMT * (clsPmpaPb.GISUL[cBArg.Bi] / 100.0));       //발생금액
                            }
                        }
                        else
                        {
                            clsPmpaPb.G7AMT = cBAR.AMT;
                        }
                        #endregion
                    }
                    #endregion
                    
                    //청구용 GBNGT2 컬럼정리
                    string strHang = cBAcct.Bas_Acct_Hang_Set(cBArg.BUN);
                    bool bOG = cBAcct.Rtn_OG_Suga(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //분만수가여부
                    clsPmpaType.ISG.GBNGT2 = cPF.Convert_GbNgt2(cBArg, strHang, bOG);

                    if (clsPmpaPb.G7AMT != 0)
                    {
                        Slip_Write_Process(pDbCon);
                    }
                    else
                    {
                        if (GstrOpFlag == "1")      //수술실 일때
                        {
                            if (clsPmpaType.ISG.Sucode != null)
                            {
                                if (clsPmpaType.ISG.Sucode == "PRS2")
                                {
                                    ComFunc.MsgBox("BUN_22_Account : " + clsPmpaType.ISG.Sucode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
            
        }

        void BUN_22_BOHUM(PsmhDb pDbCon, int nsAdd1, int nNgt, int nOldNgt)
        {
            string strErrP = "0";

            try
            {
                strErrP = "0";
                clsBasAcct cBAcct = new clsBasAcct();

            

                strErrP = "1";
                #region //심야가산율 Check
                if (string.Compare(clsPmpaPb.GstrBdate, clsPmpaPb.NGT22_DATE) < 0 && clsPmpaPb.OLD_NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
                    nNgt = clsPmpaPb.OLD_NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
                else
                    nNgt = clsPmpaType.ISG.GbNgt == "D" ? clsPmpaPb.NIGHT_22[8] : clsPmpaPb.NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)];

                if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0) { nNgt = 100; }
                #endregion

                #region //응급가산 구분자 , 수가 AA구분이 1일 경우 KTAS 값을 'A'로 치환 2016-03-18(원무수납에서 구분자 사용함)
                if ((clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (clsPmpaType.ISG.SugbAA == "1")
                    {
                        clsPmpaPb.GstatER = "A";
                        cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, "A", "I");
                    }
                    else
                    {
                        cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.IA.KTASLEVEL, "I");
                    }

                    if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.IA.KTASLEVEL; }
                    if (clsPmpaType.ISG.SugbAA == "1") { clsPmpaPb.GstatER = "A"; }
                }
                #endregion

                //2021-08-06
                if (clsPmpaPb.GnPedRate == 0)
                {
                    cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date); //GnPedRate
                }



                strErrP = "2";
                //개두, 일측패환기, 개흉 마취의 경우 나이가산 없음
                if (nsAdd1 > 0) { clsPmpaPb.GnPedRate = 0; }
                strErrP = "3";
                // 마취 행위료, 재료대 계산
                if (clsPmpaType.ISG.Bun == "23" || clsPmpaType.ISG.SugbE == "0")
                    clsPmpaPb.G7AMT = cBAcct.BAS_MACH_AMT(2, clsPmpaType.ISG.Sunext, clsPmpaType.ISG.BaseAmt, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
                else                                          //마취 재료대 계산
                    clsPmpaPb.G7AMT = cBAcct.BAS_MACH_AMT(1, clsPmpaType.ISG.Sunext, clsPmpaType.ISG.BaseAmt, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);

                strErrP = "4";
                if (clsPmpaType.ISG.Nal < 0) { clsPmpaPb.G7AMT = clsPmpaPb.G7AMT * -1; }
                strErrP = "5";
                //AS-IS 가산 구분 (EDI 수가적용이 안된 수가는 기존 가산 루틴을 탄다
                //2017-06-20 권역가산대상일 경우 (심야가산등)
                //  if (clsPmpaType.ISG.BCODE.Trim() != "" && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                //{
                if (string.Compare(clsPmpaPb.GstrBdate, clsPmpaPb.NGT22_DATE) < 0 && clsPmpaPb.OLD_NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
                    clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaPb.G7AMT * (nOldNgt + nsAdd1 + +clsPmpaPb.GnErRate  + clsPmpaPb.GnErRateK + clsPmpaPb.GnPedRate) / 100.0);
                else
                    clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaPb.G7AMT * (nNgt + nsAdd1 + +clsPmpaPb.GnErRate  + clsPmpaPb.GnErRateK + clsPmpaPb.GnPedRate) / 100.0);
                //}
                strErrP = "6";
                //기술료가산
                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    else
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                strErrP = "7";
                //소아, 산모 가산구분자
                if (clsPmpaPb.GnPedRate > 0)
                {
                    if (clsPmpaType.ISG.SugbZ == "Z")
                        clsPmpaPb.GstatPED = "Z";
                    else
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
                strErrP = "8";
                if (clsPmpaPb.G7AMT != 0)
                {
                    Slip_Write_Process(pDbCon);
                }
                else
                {
                    if (GstrOpFlag == "1")      //수술실 일때
                    {
                        if (clsPmpaType.ISG.Sucode != null)
                        {
                            if (clsPmpaType.ISG.Sucode == "PRS2")
                            {
                                ComFunc.MsgBox("BUN_22_BOHUM : " + clsPmpaType.ISG.Sucode);
                            }
                        }
                    }
                }
                strErrP = "9";
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("에러부분 : " + strErrP + ComNum.VBLF + ex.ToString());
            }
        }

        void BUN_34_Account(PsmhDb pDbCon)
        {
            string strPCode = string.Empty;
            int nsAdd1 = 0;  //화상가산 '2017-06-20
            int nsAdd2 = 0;  //외과가산 '2017-06-20
            int nsAdd3 = 0;  //흉부가산 '2017-06-20
            int nsAdd4 = 0;  //신경외과가산 '2019-02-20 
            int nsAddT = 0;  //각종 추가가산 합계

            clsBasAcct cBAcct       = new clsBasAcct();
            clsIuSentChk cISentChk  = new clsIuSentChk();
            clsPmpaFunc cPF         = new clsPmpaFunc();

            clsPmpaType.cBas_Add_Arg cBArg = new clsPmpaType.cBas_Add_Arg();
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();

            strPCode = cISentChk.Rtn_Bas_Sun_BCode(pDbCon, clsPmpaType.ISG.Sunext, clsPmpaType.IA.Date);

            //이미 가산된 코드이거나 인정비급여, 병원 임의수가 인경우 기존대로 금액 설정


            //if (strPCode == "JJJJJJ" || strPCode == "999999" || clsPmpaType.ISG.SugbP == "1" || clsPmpaType.ISG.Bun == "35" || clsPmpaType.IA.Bi > 50)
            if (strPCode == "JJJJJJ" || strPCode == "999999" || clsPmpaType.ISG.SugbP == "1" || clsPmpaType.IA.Bi > 50)
            {
                #region 기존 AS-IS 가산내용
                //화상 가산
                switch (clsPmpaType.ISG.SugbAD)
                {
                    case "1": nsAdd1 = 30; break;
                    default:  nsAdd1 = 0;  break;
                }

                //외과 가산
                if (clsPmpaType.ISG.GSADD == "1")
                { 
                    switch (clsPmpaType.ISG.SugbY)
                    {
                        case "1": nsAdd2 = 20; break;
                        case "2": nsAdd2 = 30; break;
                        default:  nsAdd2 = 0; break;
                    }
                }

                //흉부외과 가산
                if (clsPmpaType.ISG.GSADD == "2")
                {
                    switch (clsPmpaType.ISG.SugbZ)
                    {
                        case "1": nsAdd3 = 100; break;
                        case "2": nsAdd3 = 70;  break;
                        case "3": nsAdd3 = 30;  break;
                        case "4": nsAdd3 = 20;  break;
                        default:  nsAdd3 = 0;   break;
                    }
                }

                //신경외과 가산 코드에 가산이 붙는형식이라 이위치에서 작업함
                if ( cISentChk.Rtn_Bas_Sun_SUGBAE(pDbCon, clsPmpaType.ISG.Sunext, clsPmpaType.IA.Date) != "0")
                {
                    switch (cISentChk.Rtn_Bas_Sun_SUGBAE(pDbCon, clsPmpaType.ISG.Sunext, clsPmpaType.IA.Date))
                    {
                        case "1": nsAdd4 = 5; break;
                        case "2": nsAdd4 = 10; break;
                        case "3": nsAdd4 = 15; break;
                        default: nsAdd4 = 0; break;
                    }
                }

           
                nsAddT = nsAdd1 + nsAdd2 + nsAdd3 + nsAdd4 ;

                BUN_34_BOHUM(pDbCon, nsAddT);
                #endregion
            }
            else
            {
                #region 가산항목 세팅
                cBArg           = new clsPmpaType.cBas_Add_Arg();
                cBArg.AGE       = Convert.ToInt16(clsPmpaType.IA.Age);

                //주민번호상 생일이 유효한지 점검
                //if (cPF.Chk_JuminNo_BirthDay(clsPmpaType.TIT.Jumin1) == "NO")
                //{
                //    strBirthDay = clsPmpaType.TIT.BirthDay;
                //}
                //else
                //{
                //    strBirthDay = clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3;
                //}
                //cBArg.AGEILSU = ComFunc.AgeCalcEx_Zero(strBirthDay, strCurDate);

                cBArg.AGEILSU   = ComFunc.AgeCalcEx_Zero(clsPmpaType.IA.Jumin1 + clsPmpaType.IA.Jumin2, clsPmpaType.IA.Date);
                cBArg.SUNEXT    = clsPmpaType.ISG.Sunext;           //수가코드
                cBArg.BUN       = clsPmpaType.ISG.Bun;              //수가분류
                cBArg.SUGBE     = clsPmpaType.ISG.SugbE;            //수가 E항(기술료)
                
                cBArg.Bi        = clsPmpaType.IA.Bi1;               //자격
                cBArg.BDATE     = clsPmpaType.IA.Date;              //처방일자
                if (GstrOpFlag == "1")          //수술실 일때
                {
                    cBArg.GBER = clsPmpaType.ISG.GBKTAS;         //응급 가산
                }
                else
                {
                    if ((clsPmpaType.ISG.GbErChk == "Y"  || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                    {
                       
                        //2018-11-21  add
                        if (clsPmpaType.ISG.SugbAA == "1")
                        {
                            cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, "A", "I");
                        }
                        else
                        {
                            cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.IA.KTASLEVEL, "I");
                        } 

                        if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.IA.KTASLEVEL; }
                        if (clsPmpaType.ISG.SugbAA == "1") { clsPmpaPb.GstatER = "A"; }
                        if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0) { cBArg.GBER = clsPmpaType.IA.KTASLEVEL; }       //응급 가산
                    }
                }
                //cBArg.GBER      = clsPmpaType.IA.KTASLEVEL;         //응급 가산
                cBArg.NIGHT     = clsPmpaType.ISG.GbNgt;            //공휴, 야간 가산
                if (clsPmpaType.ISG.GbNgt == "3")
                {
                    clsPmpaType.ISG.GbNgt = "D";
                    cBArg.NIGHT = "";
                    cBArg.MIDNIGHT = "Y";
                }
                cBArg.AN1       = clsPmpaType.ISG.SugbAC;           //마취 가산
                cBArg.OP1       = clsPmpaType.ISG.GSADD;            //외과 / 흉부외과 가산
                clsPmpaType.ISG.BURN = clsPmpaType.ISG.SugbAD;
                cBArg.OP2       = clsPmpaType.ISG.BURN;             //화상 가산    
                if (clsPmpaType.ISG.OPGBN == "D")
                {
                    cBArg.OP3 = "";            //수술, 부수술, 제2수술   
                    cBArg.AGE = 20;            //나이가산 안되야됨 
                }
                else
                {
                    cBArg.OP3 = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
                }
                //cBArg.OP3       = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
                cBArg.OP4       = clsPmpaType.ISG.GbHighRisk;       //산모가산
                cBArg.XRAY1     = clsPmpaType.ISG.SugbAB;           //판독 가산
                #endregion

                //EDI 수가 금액 
                cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

                clsPmpaType.ISG.BaseAmt = cBAR.BASEAMT;
                clsPmpaType.ISG.BCODE   = cBAR.PCODE;
               // clsPmpaPb.G7AMT         = cBAR.AMT;

                clsPmpaPb.G7AMT = (long)Math.Truncate(cBAR.BASEAMT * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                #region 기술료 가산
                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                }
                #endregion


                //청구용 GBNGT2 컬럼정리
                string strHang = cBAcct.Bas_Acct_Hang_Set(cBArg.BUN);
                bool bOG = cBAcct.Rtn_OG_Suga(pDbCon, cBArg.SUNEXT, cBArg.BDATE);  //분만수가여부
                clsPmpaType.ISG.GBNGT2 = cPF.Convert_GbNgt2(cBArg, strHang, bOG);

                Slip_Write_Process(pDbCon);
            }
        }

        void BUN_34_BOHUM(PsmhDb pDbCon, int nsAddT)
        {
            int nNgt = 0;
            int nAdd = 0;
            double nRem = 0;
            clsBasAcct cBAcct = new clsBasAcct();

            if (GstrOpFlag == "1")
            {
                clsPmpaType.IA.KTASLEVEL = clsPmpaType.ISG.GBKTAS;  //2018-09-04
            }

            #region //심야가산율 Check
            if (string.Compare(clsPmpaPb.GstrBdate, clsPmpaPb.NIGHT_DATE) < 0 && clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)  
                nNgt = clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            else
                nNgt = clsPmpaType.ISG.GbNgt == "D" ? clsPmpaPb.NIGHT[8] : clsPmpaPb.NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];

            if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0) { nNgt = 100; }
            #endregion

            #region //응급가산 구분자 , 수가 AA구분이 1일 경우 KTAS 값을 'A'로 치환 2016-03-18(원무수납에서 구분자 사용함)
            if ((clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            { 
                if (clsPmpaType.ISG.SugbAA == "1")
                {
                    clsPmpaPb.GstatER = "A";
                    cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, "A", "I");
                }
                else
                {
                    cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.IA.KTASLEVEL, "I");
                }

                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.IA.KTASLEVEL; }
                if (clsPmpaType.ISG.SugbAA == "1") { clsPmpaPb.GstatER = "A"; }
            }
            #endregion

            #region //고위험 임산부
            if (clsPmpaType.IA.Age >= 35 || (clsPmpaType.ISG.GbHighRisk != "" && clsPmpaType.ISG.GbHighRisk != "00" && clsPmpaType.ISG.GbHighRisk != null))
            {
                cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaPb.GstrBdate, clsPmpaType.IA.AgeiLsu, "I", "", "Z");
            }
            else
            {
                cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaPb.GstrBdate, clsPmpaType.IA.AgeiLsu, "I");
            }
            #endregion

            #region//특정수가 강제 가산
            //switch (clsPmpaType.ISG.Sucode)
            //{
            //    case "O1510":  nsAdd = 70; break;
            //    case "Q2481":  nsAdd = 30; break;
            //    case "N0011A": nsAdd = 30; break;
            //    case "N0012A": nsAdd = 30; break;
            //    case "N0053A": nsAdd = 30; break;
            //    case "N0054A": nsAdd = 30; break;
            //    case "N0057A": nsAdd = 30; break;
            //    case "N0058A": nsAdd = 30; break;
            //    default: nsAdd = 0; break;
            //}
            #endregion

            #region//B항 가산에 따른 구분자
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (clsPmpaType.ISG.SugbB != "0" && clsPmpaType.IA.Age < 6 && string.Compare(clsPmpaType.ISG.GbNgt, "0") > 0)
                {
                    if (clsPmpaType.ISG.BCODE.Trim() != "")
                        clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (nNgt + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAddT) / 100.0, 2, MidpointRounding.AwayFromZero);

                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
                else if (clsPmpaType.ISG.SugbB != "0" && clsPmpaType.IA.Age < 6)                 //소아가산
                {
                    if (clsPmpaType.ISG.BCODE.Trim() != "")
                        clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAddT) / 100.0, 2, MidpointRounding.AwayFromZero);

                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
                else if (clsPmpaType.ISG.SugbB == "Z")                  //산모가산 2013-02-08
                {
                    if (clsPmpaType.IA.Age >= 35 || (clsPmpaType.ISG.GbHighRisk != "" && clsPmpaType.ISG.GbHighRisk != "00"))
                    {
                        if (clsPmpaPb.GnPedRate > 0) { clsPmpaPb.GstatPED = "Z"; }
                    }

                    if (string.Compare(clsPmpaType.ISG.GbNgt, "0") > 0)
                    {
                        switch (clsPmpaType.ISG.SugbJ)                                  //단가에기가산여부Check
                        {
                            case "1": nRem = 1.1; nAdd = 10; break;
                            case "2": nRem = 1.2; nAdd = 20; break;
                            case "3": nRem = 1.3; nAdd = 30; break;
                            case "4": nRem = 1.4; nAdd = 40; break;
                            case "5": nRem = 1.5; nAdd = 50; break;
                            case "6": nRem = 1.6; nAdd = 60; break;
                            case "7": nRem = 1.7; nAdd = 70; break;
                            case "8": nRem = 1.8; nAdd = 80; break;
                            case "9": nRem = 1.9; nAdd = 90; break;
                            default: nRem = 1; nAdd = 0; break;
                        }

                      //  if (clsPmpaType.ISG.BCODE.Trim() != "")
                            clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt / nRem * (nNgt + nAdd + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAddT) / 100.0, 2, MidpointRounding.AwayFromZero);

                    }
                    else
                    {
                       // if (clsPmpaType.ISG.BCODE.Trim() != "")
                            clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate+ clsPmpaPb.GnErRateK + nsAddT) / 100.0, 2, MidpointRounding.AwayFromZero);
                    }

                    if (clsPmpaPb.GstatPED != "Z")
                    {
                        if (clsPmpaPb.GnPedRate == 0)
                            clsPmpaPb.GstatPED = "0";
                        else
                            clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                    }
                }
                else
                {
                    //2015-12-30
                    if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0 || nsAddT > 0)
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt * (100 + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAddT + (nNgt-100)) / 100;
                    
                }

            }
            #endregion
            
            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            Slip_Write_Process(pDbCon);
            
        }

        void BUN_37_Account(PsmhDb pDbCon)
        {
            clsBasAcct cBAcct = new clsBasAcct();
            //심사과 요청으로 변경 무조건 수가 금액으로 계산 감산 처리 로직 삭제
            if (string.Compare(clsPmpaPb.GstrBdate, "2017-07-01") >= 0)
            { 
                if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB == "2")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.2);
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
                else if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbE == "1" && string.Compare(clsPmpaType.ISG.SugbB, "A") >= 0)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (100 + clsPmpaPb.GnPedRate) / 100.0);
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
            }
            else
            { 
                if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB == "2")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.2);
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                }
            }

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)     //기술료가산
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                else
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
            }

            Slip_Write_Process(pDbCon);
            
        }

        void BUN_75_Account(PsmhDb pDbCon)
        {
            switch (clsPmpaType.ISG.SugbB)
            { 
                case "1":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 1000)); break;
                case "2":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 2000)); break;
                case "3":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 3000)); break;
                case "4":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * (clsPmpaType.ISG.BaseAmt * 0.1))); break;
                default:    clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * Math.Abs(clsPmpaType.ISG.Nal)); break;
            }

            if (clsPmpaType.ISG.Nal < 0) { clsPmpaPb.G7AMT = clsPmpaPb.G7AMT * -1; }

            Slip_Write_Process(pDbCon);
            
        }

        void BUN_99_Account(PsmhDb pDbCon)
        {
            string strPCode = string.Empty;
            int nsAdd1 = 0;  //판독가산 '2017-07-01

            #region 기타분류는 EDI수가 미적용 (심사팀에서 준비가 안되었음)
            //clsBasAcct cBAcct = new clsBasAcct();
            //clsIuSentChk cISentChk = new clsIuSentChk();
            //clsPmpaType.cBas_Add_Arg cBArg = new clsPmpaType.cBas_Add_Arg();
            //clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();

            //strPCode = cISentChk.Rtn_Bas_Sun_BCode(pDbCon, clsPmpaType.ISG.Sunext, clsPmpaType.IA.Date);

            ////이미 가산된 코드이거나 인정비급여, 병원 임의수가 인경우 기존대로 금액 설정
            //if (strPCode == "JJJJJJ" || strPCode == "999999" || clsPmpaType.ISG.SugbP == "1" || clsPmpaType.ISG.Bun == "29" || clsPmpaType.ISG.Bun == "36" || clsPmpaType.ISG.Bun == "72" || clsPmpaType.ISG.Bun == "73")
            //{
            //    #region 기존 AS-IS 가산내용
            //    //판독가산
            //    switch (clsPmpaType.ISG.SugbAB)
            //    {
            //        case "1": nsAdd1 = 10; break;
            //        default: nsAdd1 = 0; break;
            //    }
            //    #endregion

            //    if (clsPmpaType.ISG.Bun.Equals("01") || clsPmpaType.ISG.Bun.Equals("02"))
            //    {
            //        BUN_99_JIN(pDbCon);            //진찰료 세부계산
            //    }
            //    else
            //    {
            //        BUN_99_BOHUM(pDbCon, nsAdd1);  //기타항목 세부계산
            //    }
            //}
            //else
            //{
            //    #region 가산항목 세팅
            //    cBArg = new clsPmpaType.cBas_Add_Arg();
            //    cBArg.AGE = Convert.ToInt16(clsPmpaType.IA.Age);
            //    cBArg.AGEILSU = ComFunc.AgeCalcEx_Zero(clsPmpaType.IA.Jumin1 + clsPmpaType.IA.Jumin2, clsPmpaType.IA.Date);
            //    cBArg.SUNEXT = clsPmpaType.ISG.Sunext;           //수가코드
            //    cBArg.BUN = clsPmpaType.ISG.Bun;              //수가분류
            //    cBArg.SUGBE = clsPmpaType.ISG.SugbE;            //수가 E항(기술료)
            //    cBArg.Bi = clsPmpaType.IA.Bi1;               //자격
            //    cBArg.BDATE = clsPmpaType.IA.Date;
            //    if (GstrOpFlag == "1")          //수술실 일때
            //    {
            //        cBArg.GBER = clsPmpaType.ISG.GBKTAS;         //응급 가산
            //    }
            //    else
            //    {
            //        if ((clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            //        {
            //            cBArg.GBER = clsPmpaType.IA.KTASLEVEL;         //응급 가산
            //        }
            //    }
            //    //cBArg.GBER      = clsPmpaType.IA.KTASLEVEL;         //응급 가산
            //    cBArg.NIGHT = clsPmpaType.ISG.GbNgt;            //공휴, 야간 가산
            //    cBArg.AN1 = clsPmpaType.ISG.SugbAC;           //마취 가산
            //    cBArg.OP1 = clsPmpaType.ISG.GSADD;            //외과 / 흉부외과 가산
            //    cBArg.OP2 = clsPmpaType.ISG.BURN;             //화상 가산          
            //    if (clsPmpaType.ISG.OPGBN == "D")
            //    {
            //        cBArg.OP3 = "";            //수술, 부수술, 제2수술   
            //    }
            //    else
            //    {
            //        cBArg.OP3 = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
            //    }
            //    //cBArg.OP3       = clsPmpaType.ISG.OPGBN;            //수술, 부수술, 제2수술   
            //    cBArg.OP4 = clsPmpaType.ISG.GbHighRisk;       //산모가산
            //    cBArg.XRAY1 = clsPmpaType.ISG.SugbAB;           //판독 가산
            //    #endregion

            //    //EDI 수가 금액 
            //    cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

            //    clsPmpaType.ISG.BaseAmt = cBAR.BASEAMT;
            //    clsPmpaType.ISG.BCODE = cBAR.PCODE;

            //    clsPmpaPb.G7AMT = cBAR.AMT;
            //    //수량, 날수를 여기에 기술한 이유는 그룹코드에서 표준계수로 지정된 수량을 계산해주기 위함.
            //    //그 외 디자인 Layer에서 수량, 날수를 바꾸면 해당 모듈에서 직접 계산됨
            //    clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaPb.G7AMT * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            //    Slip_Write_Process(pDbCon);
            //} 
            #endregion

            //기타분류는 기존 루틴 적용
            //판독가산
            switch (clsPmpaType.ISG.SugbAB)
            {
                case "1": nsAdd1 = 10; break;
                default: nsAdd1 = 0; break;
            }

            if (clsPmpaType.ISG.Bun.Equals("01") || clsPmpaType.ISG.Bun.Equals("02"))
            {
                BUN_99_JIN(pDbCon);            //진찰료 세부계산
            }
            else
            {
                BUN_99_BOHUM(pDbCon, nsAdd1);  //기타항목 세부계산
            }
        }

        void BUN_99_JIN(PsmhDb pDbCon)
        {
            #region 나이불문 치과 급여, 장애인은 500원 가산
            if (clsPmpaType.IA.Dept == "DT" && VB.Left(clsPmpaType.TIT.Bi, 2) == "2" && clsPmpaType.TIT.Bohun == "3")
            {
                clsPmpaType.ISG.BaseAmt += 500;     //나이불문 500원 가산
            } 
            #endregion

            #region 진찰료 계산
            else if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.Sucode != "AA702" && clsPmpaType.ISG.Sucode != "V3203" && clsPmpaType.ISG.Sucode != "V4300"  && clsPmpaType.ISG.Sucode != "AH3B3" && clsPmpaType.ISG.Sucode != "V2300" && clsPmpaType.ISG.Sucode != "V4203" && clsPmpaType.ISG.Sucode != "V2300" && clsPmpaType.ISG.Sucode != "V2200")
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.PedAdd_Date) >= 0)   //2017-07-01 소아가산 0에,1-6세 구분 추가
                {
                    if (clsPmpaType.ISG.Bun == "01")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAddYg1;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAdd1;     //초진소아가산
                        }
                    }

                    if (clsPmpaType.ISG.Bun == "02")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAddYg3;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAdd3;     //초진소아가산
                        }
                    }
                }
                else
                {
                    if (clsPmpaType.ISG.Bun == "01")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAddYg1;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAdd1;     //초진소아가산
                        }
                    }

                    if (clsPmpaType.ISG.Bun == "02")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAddYg2;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAdd2;
                        }
                    }
                }
            } 
            #endregion

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            Slip_Write_Process(pDbCon);
            
        }

        void BUN_99_BOHUM(PsmhDb pDbCon, int nsAdd1)
        {
            clsPmpaPb.GnErRate = 0;
            clsPmpaPb.GnErRateK = 0;
            double nRem = 1.0;

            clsBasAcct cBAcct = new clsBasAcct();
            int nNgt = 0;
            String StrKtas = "";
            
            if ( clsPmpaType.IA.KTASLEVEL !=null && GstrOpFlag != "1")  { StrKtas = clsPmpaType.IA.KTASLEVEL; }
            else { StrKtas = clsPmpaType.ISG.GBKTAS; } 

            #region //심야가산율 Check
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.NIGHT_DATE) < 0 && clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
                nNgt = clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            else
                nNgt = clsPmpaPb.NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];

            if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0) { nNgt = 100; }
            #endregion

            #region //응급가산 구분자
            if ((clsPmpaType.ISG.GBKTAS == "A" || clsPmpaType.ISG.GBKTAS == "1" || clsPmpaType.ISG.GBKTAS == "2" || clsPmpaType.ISG.GBKTAS == "3" ||    //재원
                clsPmpaType.ISG.GbErChk == "Y" || clsPmpaType.ISG.SugbAA == "3") && string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)         //센드포함
            {
                if (clsPmpaType.ISG.SugbAA == "1")
                {
                    clsPmpaPb.GstatER = "A";
                    cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, "A", "I");
                }
                else
                {
                    cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, StrKtas , "I"); //2018-10-29  clsPmpaType.IA.KTASLEVE-> StrKtas
                }

                
            }
            #endregion

            #region //소아가산율
            //cBAcct.Bas_ER_Rate(clsPmpaType.IA.ErJDate, clsPmpaType.ISG.GbErActTime, clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, StrKtas, "I"); //2018-10-29  clsPmpaType.IA.KTASLEVE-> StrKtas
            cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date, clsPmpaType.IA.AgeiLsu, "I", "", "Z");

            if (clsPmpaPb.GnErRate > 0 || clsPmpaPb.GnErRateK > 0 ) { clsPmpaPb.GstatER = StrKtas; }   //2018-10-29  clsPmpaType.IA.KTASLEVE-> StrKtas
            if (clsPmpaType.ISG.SugbAA == "1" && clsPmpaPb.GnErRate > 0 ) { clsPmpaPb.GstatER = "A"; }
            #endregion

            #region 단가계산 및 가산구분자 세팅
            //2017-06-20 권역응급가산 수가 추가(GnErRateK)
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                //권역응급수가일 경우 야간가산 없음
                if (clsPmpaPb.GnErRateK > 0 && clsPmpaType.ISG.SugbAA == "3" && (string.Compare(clsPmpaType.ISG.Bun, "41") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "51") <= 0))
                    clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAdd1) / 100.0, 2, MidpointRounding.AwayFromZero);
                else
                    clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (nNgt + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + clsPmpaPb.GnErRateK + nsAdd1) / 100.0, 2, MidpointRounding.AwayFromZero);


                if (string.Compare(clsPmpaType.IA.Date, "2017-02-01") >= 0)
                {
                    if (clsPmpaType.ISG.SugbB == "Y" && clsPmpaPb.GnPedRate > 0)
                    {
                        if (clsPmpaType.IA.Age < 6)
                            clsPmpaPb.GstatPED = "Y";
                        else if (clsPmpaType.IA.Age >= 70)
                            clsPmpaPb.GstatPED = "Y";
                    }
                    else
                    {
                        if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbB != "0")
                        {
                            clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                        }
                    }
                }
                else
                {
                    if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.SugbB != "0")
                    {
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaPb.GstrBdate, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu);
                       
                    }
                }

                clsPmpaType.ISG.GbChildZ = clsPmpaPb.GstatPED;
            }

            if (clsPmpaType.ISG.Sucode == "AC302")
            {
                if (clsPmpaType.IA.AgeiLsu <= 28 && clsPmpaType.IA.Age == 0)
                {
                    nRem += 1.0;
                }
                else if (clsPmpaType.IA.Age == 0)
                {
                    nRem += 0.5;
                }
                else if (clsPmpaType.IA.Age < 6)
                {
                    nRem += 0.3;
                }
                if (clsPmpaType.ISG.GbNgt != "0" && clsPmpaType.ISG.GbNgt != "")
                {
                    nRem += 0.5;
                }
            }

            clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt *  nRem );

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal );

         
            #endregion

            #region 기술료 가산
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
            }
            #endregion

            Slip_Write_Process(pDbCon);

            switch (clsPmpaType.ISG.SugbB)
            {
                case "9": B20_DEPO(pDbCon); break;      //관절강내 주사수기료 재료대에도 발생
                default:
                break;
            }


        }


        string Read_DRUG_MAYAK_CHK(PsmhDb pDbCon, string ArgSucode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {

                    SQL = "";
                    SQL += " SELECT SuNext                                    \r\n";
                    SQL += "   FROM " + ComNum.DB_PMPA + "VIEW_BAS_SUN_MAYAK         \r\n";
                    SQL += "  WHERE trim(SuNext)  = trim('" + ArgSucode + "')                 \r\n";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                    rtnVal = "OK";
                    }

                    dt.Dispose();
                    dt = null;

                    return rtnVal;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }
        string Read_ER_ORDER_ACT_Time(PsmhDb pDbCon, string ArgPano, long ArgOrdNo)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                if (clsPmpaType.ISG.SugbAA == "3")
                {
                    SQL = "";
                    SQL += " SELECT ER24                                    \r\n";
                    SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER         \r\n";
                    SQL += "  WHERE PTNO ='" + ArgPano + "'                 \r\n";
                    SQL += "    AND ORDERNO = " + ArgOrdNo + "              \r\n";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rtnVal = dt.Rows[0]["ER24"].ToString().Trim();
                    }
                    
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL += " SELECT TO_CHAR(ACTTIME,'YYYY-MM-DD HH24:MI') ACTTIME   \r\n";
                    SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER_ACT_ER          \r\n";
                    SQL += "  WHERE PTNO ='" + ArgPano + "'                         \r\n";
                    SQL += "    AND ORDERNO = " + ArgOrdNo + "                      \r\n";
                    SQL += "  ORDER BY ACTTIME ASC                                  \r\n";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        rtnVal = dt.Rows[0]["ACTTIME"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// IPD_NEW_SLIP 읽어 IPD_TRANS 금액 UPDATE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgGbIPD"></param>
        /// <returns></returns>
        public bool IPD_Trans_Amt_Update(PsmhDb pDbCon, long ArgTrsNo, string ArgGbIPD)
        {
            int i = 0, nNu = 0, nBi = 0;
            long[] nAmt = new long[61];
            int nRead = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            SQL = "";
            SQL += " SELECT NU,SUM(Amt1) Amt1,SUM(Amt2) Amt2      \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP    \r\n";
            SQL += "  WHERE TrsNo=" + ArgTrsNo + "                \r\n";
            SQL += "  GROUP BY Nu                                 \r\n";
            SQL += "  ORDER BY Nu                                     ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false; ;
            }

            nRead = Dt.Rows.Count;

            if (Dt.Rows.Count > 0)
            {
                for (i = 0; i < nRead; i++)
                {
                    nNu = Convert.ToInt32(VB.Val(Dt.Rows[i]["NU"].ToString()));
                    nAmt[nNu] += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt1"].ToString())); 
                    nAmt[44] += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString()));  //선택진료 집계
                    nAmt[50] += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt1"].ToString())) + Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString())); 
                }
            }

            Dt.Dispose();
            Dt = null;


            //DB에 금액을 UPDATE
            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
            SQL += "         GbIPD='" + ArgGbIPD + "',";
            if (ArgGbIPD == "D")
            {
                for (i = 1; i < 60; i++)
                {
                    SQL += "Amt" + VB.Format(i, "00") + "=" + nAmt[i] + ",";
                }
                SQL += " Amt64 =0, ";  //2011-04-06 약제상한차액 0 - 삭제시
                SQL += " Amt60=" + nAmt[60] + " ";
            }
            else
            {
                for (i = 1; i < 50; i++)
                {
                    SQL += "Amt" + VB.Format(i, "00") + "=" + nAmt[i] + ",";
                }
                SQL += " Amt50=" + nAmt[50] + " ";
            }
            SQL += " WHERE TrsNo = " + ArgTrsNo + " ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return false;
            }

            return true;
        }

        public bool Suga_Read_YN_Check(string ArgOldBi, string ArgNewBi, string ArgNewGbilban2, string ArgOldGbilban2, string ArgNewSPC, string ArgOldSPC)
        {
            bool rtnVal = true;
            string strOldBi_1 = string.Empty;
            string strNewBi_1 = string.Empty;

            strOldBi_1 = VB.Left(ArgOldBi, 1);
            if (ArgOldBi == "52" || ArgOldBi == "55")
            {
                strOldBi_1 = "6"; //자보
            }

            strNewBi_1 = VB.Left(ArgNewBi, 1);
            if (ArgNewBi == "52" || ArgNewBi == "55")
            {
                strNewBi_1 = "6"; //자보
            }

            if (strNewBi_1 == strOldBi_1)
            {
                rtnVal = false;
                
                if (ArgNewBi == "51" && ArgNewGbilban2 != ArgOldGbilban2)  //외국new
                {
                    rtnVal = true;
                }
            }
            else
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public bool Create_Process_BB(PsmhDb pDbCon, long ArgOldTRSNO, long ArgNewTRSNO, string ArgNewBi, string ArgLange, string ArgFDay, string ArgTDay)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nCreNal = 0;
            int nIlsu = 0;
            long nCreAmt1 = 0, nCreAmt2 = 0;
            string strRowid = string.Empty;
            string strNewNu = string.Empty;
            string strNewGbSelf = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;

            // 재원 SLIP 취소발생 (보험 : 보험) Process  =====>
           
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ActDate,PANO,BI,                          \r\n";
                SQL += "        TO_CHAR(BDATE,'YYYY-MM-DD') BDate,                                      \r\n";
                SQL += "        TO_CHAR(ENTDATE,'YYYY-MM-DD') EntDate,SUNEXT,                           \r\n";
                SQL += "        BUN,NU,QTY,NAL,BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,                      \r\n";
                SQL += "        GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,                         \r\n";
                SQL += "        GBHOST,PART,AMT1,AMT2,SEQNO,YYMM,ORDERNO,                               \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT, DRGSELF,                         \r\n";
                SQL += "        ORDER_DCT,EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD , POWDER,ASADD,OPGUBUN ,BCODE    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                      \r\n";
                SQL += "  WHERE TrsNo = " + ArgOldTRSNO + "                                             \r\n";
                SQL += "    AND Part    = '-'                                                           \r\n";
                if (ArgLange == "3")  //뒷부분
                {
                    SQL += "    AND Bdate >= TO_DATE('" + ArgFDay + "','YYYY-MM-DD')                    \r\n";
                    if (ArgTDay != "")
                    {
                        SQL += "    AND Bdate <= TO_DATE('" + ArgTDay + "','YYYY-MM-DD')                \r\n";
                    }
                }
                SQL += "    AND ActDate = TRUNC(SYSDATE)                                                \r\n";
                SQL += "  ORDER BY Bdate, DeptCode                                                          ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        nCreNal = Convert.ToInt32(VB.Val(dt.Rows[i]["NAL"].ToString()));
                        nCreAmt1 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT1"].ToString()));
                        nCreAmt2 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT2"].ToString()));
                        strNewNu = dt.Rows[i]["Nu"].ToString().Trim();
                        strNewNu = dt.Rows[i]["GbSelf"].ToString().Trim();

                        if (string.Compare(clsPmpaType.TIT.Bi, "40") < 0)
                        {
                            if (string.Compare(strNewGbSelf, "0") > 0)
                            {
                                switch (strNewNu)
                                {
                                    case "01":
                                    case "02":
                                    case "03": strNewNu = "21"; break;
                                    case "04":
                                    case "05":
                                    case "06":
                                    case "07":
                                    case "08":
                                    case "09":
                                    case "10":
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "14":
                                    case "15":  strNewNu = (VB.Val(strNewNu) + 18).ToString(); break;
                                    case "16":  strNewNu = "34"; break;
                                    case "17":  strNewNu = "42"; break;
                                    case "18":  strNewNu = "47"; break;
                                    case "19":  strNewNu = "37"; break;
                                    case "20":  strNewNu = "27"; break;
                                    default:    strNewNu = "";   break;
                                }
                            }
                        }
            
                        nCreNal = nCreNal * -1;
                        nCreAmt1 = nCreAmt1 * -1;
                        nCreAmt2 = nCreAmt2 * -1;
                        
                        //----------------------------------------
                        //       취소 SLIP을 INSERT
                        //----------------------------------------
                        SQL = "";
                        SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                         ";
                        SQL += "        (IPDNO,TrsNo,ActDate, Pano, Bi, Bdate, EntDate, Sunext, Bun,                                    ";
                        SQL += "        Nu, Qty, Nal,  BaseAmt, GbSpc, GbNgt, GbGisul, GbSelf,                                          ";
                        SQL += "        GbChild, DeptCode, DrCode, WardCode, Sucode, GbSlip,                                            ";
                        SQL += "        GbHost, Part, Amt1, Amt2, SeqNo, yymm, DRGSELF, ORDERNO,                                        ";
                        SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBSGADD , BCODE ,POWDER,ASADD,OPGUBUN, GBER)  ";
                        SQL += " VALUES (                                                                                               ";
                        SQL += "        " + clsPmpaType.TIT.Ipdno + ",                                                                  ";
                        SQL += "        " + ArgNewTRSNO + ",                                                                            ";
                        SQL += "        TRUNC(SYSDATE),'" + clsPmpaType.TIT.Pano + "',                                                  ";
                        SQL += "        '" + ArgNewBi + "',                                                                             ";
                        SQL += "        TO_DATE('" + dt.Rows[i]["Bdate"].ToString() + "','YYYY-MM-DD'),                                 ";
                        SQL += "        SYSDATE,                                                                                        ";
                        SQL += "        '" + dt.Rows[i]["Sunext"].ToString() + "',  '" + dt.Rows[i]["Bun"].ToString() + "',             ";
                        SQL += "        '" + dt.Rows[i]["Nu"].ToString() + "',                                                          ";
                        SQL += "         " + VB.Val(dt.Rows[i]["Qty"].ToString()) + ",     '" + nCreNal + "',                           ";
                        SQL += "         " + VB.Val(dt.Rows[i]["BaseAmt"].ToString()) + ", '" + dt.Rows[i]["GbSpc"].ToString() + "',    ";
                        SQL += "        '" + dt.Rows[i]["GbNgt"].ToString() + "',   '" + dt.Rows[i]["GbGisul"].ToString() + "',         ";
                        SQL += "        '" + dt.Rows[i]["GbSelf"].ToString() + "',  '" + dt.Rows[i]["GbChild"].ToString() + "',         ";
                        SQL += "        '" + dt.Rows[i]["DeptCode"].ToString() + "','" + dt.Rows[i]["DrCode"].ToString() + "',          ";
                        SQL += "        '" + dt.Rows[i]["WardCode"].ToString() + "','" + dt.Rows[i]["Sucode"].ToString() + "',          ";
                        SQL += "        '" + dt.Rows[i]["GbSlip"].ToString() + "', '" + dt.Rows[i]["GbHost"].ToString() + "', '+',     ";
                        SQL += "         " + nCreAmt1 + ", " + nCreAmt2 + ", '" + VB.Val(dt.Rows[i]["SeqNo"].ToString()) + "',          ";
                        SQL += "        '" + dt.Rows[i]["YYMM"].ToString() + "' ,'" + dt.Rows[i]["DRGSELF"].ToString() + "',            ";
                        SQL += "        '" + VB.Val(dt.Rows[i]["ORDERNO"].ToString()) + "',                                             ";
                        SQL += "        '" + dt.Rows[i]["ABCDATE"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["OPER_DEPT"].ToString() + "',                                                   ";
                        SQL += "        '" + dt.Rows[i]["OPER_DCT"].ToString() + "',                                                    ";
                        SQL += "        '" + dt.Rows[i]["ORDER_DEPT"].ToString() + "',                                                  ";
                        SQL += "        '" + dt.Rows[i]["ORDER_DCT"].ToString() + "',                                                   ";
                        SQL += "        '" + VB.Val(dt.Rows[i]["EXAM_WRTNO"].ToString()) + "',                                          ";
                        SQL += "        '" + VB.Val(dt.Rows[i]["RoomCode"].ToString()) + "',                                            ";
                        SQL += "         " + VB.Val(dt.Rows[i]["DIV"].ToString()) + ",                                                  ";
                        SQL += "        '" + dt.Rows[i]["GBSELNOT"].ToString().Trim() + "',                                             ";
                        SQL += "        '" + dt.Rows[i]["GBSUGBS"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["GBSGADD"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["BCODE"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["POWDER"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["ASADD"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["OPGUBUN"].ToString() + "',                                                     ";
                        SQL += "        '" + dt.Rows[i]["GBER"].ToString().Trim() + "'                                                  ";
                        SQL += "          )                                                                                             ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return false;
                        }

                        
                    }
                }

                dt.Dispose();
                dt = null;
                
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        public bool Create_Process(PsmhDb pDbCon, long ArgIpdNo, string ArgTRSNO, string ArgLange, string ArgFdate, string ArgTdate, string ArgGbIPD, string ArgSelect)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = true;
            int i = 0;
            string strOldData = string.Empty;

            clsPmpaType.ISA = new clsPmpaType.Slip_Accept_Table_IPD[1];
            //재원 SLIP 취소발생 (보험 : 일반) Process  =====>

            //배열을 Clear
            Array.Clear(clsPmpaType.ISA, 0, clsPmpaType.ISA.Length);

            clsPmpaPb.G7WRTcount = 0;

            try
            {
                if (Create_Main_Process(pDbCon, ArgIpdNo, ArgTRSNO) == false)
                {
                    return false;
                }

                //지병은 병실료,식대 계산 안함
                if (ArgGbIPD != "9")
                {
                    if (Create_ARC_Process(pDbCon, ArgLange, ArgFdate, ArgTdate) == false)
                    {
                        ComFunc.MsgBox("IPD_SLIP (발생취소 SLIP) INSERT ERROR", "오류");
                        return false;
                    }
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        bool Create_Main_Process(PsmhDb pDbCon, long ArgIpdNo, string ArgTRSNO)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = true;
            int i = 0, nREAD = 0, nCount = 0;
            string strNewData = string.Empty;
            string strOldData = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT PANO,BI,TO_CHAR(BDATE,'YYYY-MM-DD') BDate,SuCode,SUNEXT,    \r\n";
                SQL += "        BUN,NU,QTY,BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,              \r\n";
                SQL += "        GBCHILD,DEPTCODE,DRCODE,WARDCODE,GBSLIP,                    \r\n";
                SQL += "        GBHOST,SEQNO,YYMM, DRGSELF, ORDERNO,SUM(Nal) Nal,           \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,DIV,GBSELNOT,GbSUGBS,GBER ,GBSGADD , BCODE       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                          \r\n";
                SQL += "  WHERE IPDNO   = " + ArgIpdNo + "                                  \r\n";
                SQL += "    AND TRSNO  IN (" + ArgTRSNO + ")                                \r\n";
                SQL += "    AND Part     = '-'                                              \r\n";
                SQL += "    AND (GbSlip  <> 'A' or   GbSlip  is null)                       \r\n";
                SQL += "    AND ActDate  = TRUNC(SYSDATE)                                   \r\n";
                SQL += "    AND GbHost   < '2'                                              \r\n";
                SQL += "  GROUP BY Pano,Bi,BDate,SuCode,SuNext,Bun,Nu,Qty,BaseAmt,          \r\n";
                SQL += "           GbSpc,GbNgt,GbGisul,GbSelf,GbChild,DeptCode,             \r\n";
                SQL += "           DrCode,WardCode,GbSlip,GbHost,SeqNo,YYMM,                \r\n";
                SQL += "           DrgSelf,OrderNo, ABCDATE,OPER_DEPT,OPER_DCT,             \r\n";
                SQL += "           DIV,GBSELNOT,GbSUGBS,GBER ,DECODE(Bun,22,Nal,23,Nal) ,GBSGADD , BCODE     \r\n";     //2017-07-11 마취분류는 날수도 그룹 추가
                SQL += "  ORDER BY Bdate,DeptCode,DrCode,GbSlip,Bun                             ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;
                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        if (VB.Val(dt.Rows[i]["NAL"].ToString()) != 0)
                        {
                            //일자,진료과,의사 변경 여부 Check
                            strNewData = dt.Rows[i]["Bdate"].ToString().Trim();
                            strNewData += dt.Rows[i]["DeptCode"].ToString().Trim();
                            strNewData += dt.Rows[i]["DrCode"].ToString().Trim();

                            if (strOldData == "") { strOldData = strNewData; }

                            if (strOldData != strNewData) { Call_Acc_Proc_Main(pDbCon, ref nCount, ref strNewData, ref strOldData); }

                            Array.Resize(ref clsPmpaType.ISA, nCount + 1); 
                          


                            if (nCount > clsPmpaType.ISA.Length)
                            {
                                Call_Acc_Proc_Main(pDbCon, ref nCount, ref strNewData, ref strOldData);
                                nCount = 1;
                            }

                            clsPmpaType.ISA[nCount ].Qty         = VB.Val(dt.Rows[i]["Qty"].ToString());
                            clsPmpaType.ISA[nCount ].Nal         = (int)VB.Val(dt.Rows[i]["Nal"].ToString()) * -1;
                            clsPmpaType.ISA[nCount ].Imiv        = "2";
                            clsPmpaType.ISA[nCount ].GbNgt       = dt.Rows[i]["GbNgt"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].GbSpc       = dt.Rows[i]["GbSpc"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].GbSelf      = dt.Rows[i]["GbSelf"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].GbSlip      = dt.Rows[i]["GbSlip"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].BaseAmt     = Convert.ToInt64(VB.Val(dt.Rows[i]["BaseAmt"].ToString()));
                            clsPmpaType.ISA[nCount ].DeptCode    = dt.Rows[i]["DeptCode"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].DrCode      = dt.Rows[i]["DrCode"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].WardCode    = dt.Rows[i]["WardCode"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].Sucode      = dt.Rows[i]["Sucode"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].Sunext      = dt.Rows[i]["SuNext"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].Bun         = dt.Rows[i]["Bun"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].Nu          = dt.Rows[i]["Nu"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].SEQNO       = (int)VB.Val(dt.Rows[i]["SeqNo"].ToString());
                            clsPmpaType.ISA[nCount ].SugbD       = "0";
                            clsPmpaType.ISA[nCount ].YYMM        = dt.Rows[i]["YYMM"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].WardCode    = dt.Rows[i]["WardCode"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].DrgSelf     = dt.Rows[i]["DRGSELF"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].OrderNo     = Convert.ToInt64(VB.Val(dt.Rows[i]["ORDERNO"].ToString()));
                            clsPmpaType.ISA[nCount ].ABCDATE     = dt.Rows[i]["ABCDATE"].ToString().Trim();          //2006-10-30 ABC원가 추가
                            clsPmpaType.ISA[nCount ].OPER_DEPT   = dt.Rows[i]["OPER_DEPT"].ToString().Trim();        //2006-10-30 ABC원가 추가
                            clsPmpaType.ISA[nCount ].OPER_DCT    = dt.Rows[i]["OPER_DCT"].ToString().Trim();         //2006-10-30 ABC원가 추가
                            clsPmpaType.ISA[nCount ].Div         = (int)VB.Val(dt.Rows[i]["DIV"].ToString());
                            clsPmpaType.ISA[nCount ].GbSelNot    = dt.Rows[i]["GBSELNOT"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].PART2       = ""; //2012-11-15
                            clsPmpaType.ISA[nCount ].SugbS       = dt.Rows[i]["GBSUGBS"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].GBKTAS      = dt.Rows[i]["GBER"].ToString().Trim();
                            clsPmpaType.ISA[nCount ].GbGSADD     = dt.Rows[i]["GBSGADD"].ToString().Trim();
                            nCount += 1;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                Call_Acc_Proc_Main(pDbCon, ref nCount, ref strNewData, ref strOldData);

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        bool Call_Acc_Proc_Main(PsmhDb pDbCon, ref int nCount, ref string strNewData, ref string strOldData)
        {
            bool rtnVal = true;

            if (nCount < 1)
            {
                strOldData = strNewData;
                return rtnVal;
            }

            string strBDate = VB.Left(strOldData, 10);
            string strDeptCode = VB.Mid(strOldData, 11, 2);

            if (Acc_Proc_Main(pDbCon, strBDate, strDeptCode) == false)
            {
                return false;
            }

            strOldData = strNewData;

            //배열을 Clear
            Array.Clear(clsPmpaType.ISA, 0, clsPmpaType.ISA.Length);

            nCount = 0;

            return rtnVal;
        }

        bool Create_ARC_Process(PsmhDb pDbCon, string ArgLange, string strFDate, string strTDate)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            bool rtnVal = true;
            bool bExit = true;
            string strDate = string.Empty;
            string strInTime = string.Empty;
            int nRange = 0;

            clsIpdArc cIARC = new clsIpdArc();
            ComFunc CF = new ComFunc();

            strDate = strFDate;
            nRange = (int)VB.Val(ArgLange);

            do
            {
                //재원기간 전체,재원기간 뒷부분은 마지막전일자까지 ARC작업을 함
                //마지막일자의 ARC는 퇴원계산서 발부시 처리

                if (nRange == 1 || nRange == 3)
                {
                    if (strDate == strTDate) { break; }
                }

                //ARC BACKUP을 READ
                SQL = "";
                SQL += " SELECT DeptCode,DrCode,WardCode,RoomCode                   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_BM                        \r\n";
                SQL += "  WHERE Pano = '" + clsPmpaType.TIT.Pano + "'               \r\n";
                SQL += "    AND JobDate = TO_DATE('" + strDate + "','YYYY-MM-DD')   \r\n";
                SQL += "    AND GbBackup = 'J'                                          ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["ROOMCODE"].ToString()) > 0)
                    {
                        clsPmpaType.TIT.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                        clsPmpaType.TIT.WardCode = dt.Rows[0]["WardCode"].ToString().Trim(); 
                        clsPmpaType.TIT.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();    
                        clsPmpaType.TIT.DrCode = dt.Rows[0]["DrCode"].ToString().Trim(); 
                    }
                }

                dt.Dispose();
                dt = null;

                clsPmpaPb.GstrARC = "";
                //재원기간 앞부분의 마지막일자인 경우
                if (nRange == 2 && strDate == strTDate) { clsPmpaPb.GstrARC = "ICUPDT"; }

                //병실료,식대,의약품관리료,복약지도료 SLIP 발생
                clsPmpaType.IA.Date = strDate;

                if (cIARC.ARC_MAIN_PROCESS(pDbCon, strDate) == false)
                {
                    ComFunc.MsgBox("ARC 작업 ERROR", "오류");
                    return false;
                }

                strDate = CF.DATE_ADD(pDbCon, strDate, 1);

                if (string.Compare(strDate, strTDate) > 0) { bExit = false; }

            } while (bExit == true);

            return rtnVal;
        }

        /// <summary>
        /// IPD_NEW_SLIP 을 읽어 IPD_TRANS 테이블에 금액 누적
        /// <param name="ArgTrsNo"></param>
        /// 2017-07-11 김민철
        /// </summary>
        /// <seealso cref="ipdsim.bas  : IPD_TRANS_Amt_ReBuild"/>
        /// <seealso cref="IPDACCT.bas : IPD_TRANS_Amt_ReBuild"/>
        /// <history>2017.09.19 IPDACCT.bas 의 함수 같은 내용으로 통합</history>
        public bool Ipd_Trans_Amt_ReBuild(PsmhDb pDbCon, long ArgTrsNo, string strTemp)
        {
            int i = 0, nNu = 0, nBi = 0;
            long[] nAmt = new long[51];
            long nTaxAmt = 0;
            string strTax = string.Empty;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (strTemp != "임시자격")
            {
                SQL = " SELECT Bi,GbTax  From " + ComNum.DB_PMPA + "IPD_TRANS Where TRSNO =" + ArgTrsNo + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false; ;
                }
                if (Dt.Rows.Count > 0)
                {
                    nBi = Convert.ToUInt16(Dt.Rows[0]["BI"].ToString());
                    strTax = Dt.Rows[0]["GbTax"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
            

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NU,SUM(Amt1) Amt,SUM(Amt2) Amt2    ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTrsNo + "           ";
            SQL += ComNum.VBLF + "  GROUP BY Nu                              ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false; ;
            }
            if (Dt.Rows.Count > 0)
            {
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    nNu = Convert.ToUInt16(Dt.Rows[i]["NU"].ToString());
                    nAmt[nNu] += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                    nAmt[44] += Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString()));  //2011-06-01 선택진료금액 재형성
                    nAmt[50] += (Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString())) + Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt2"].ToString())));
                }
            }

            Dt.Dispose();
            Dt = null;

            if (strTemp != "임시자격")
            {
                //2014-02-24
                if (strTax == "1") { nTaxAmt = (long)Math.Truncate((nAmt[50] * (clsPmpaPb.BON_TAX[nBi] / 100.0))); }
            }

            //IPD_MASTER에 금액을 UPDATE
            SQL = "";
            if (strTemp == "임시자격")
            {
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM SET      ";
            }
            else
            {
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET                ";
            }
            
            for (i = 1; i < 50; i++)
            {
                SQL += ComNum.VBLF + "        Amt" + VB.Format(i, "00") + "=" + nAmt[i] + ", ";
            }
            //2014-02-24
            if (strTax == "1")
                SQL += ComNum.VBLF + "        Amt67 = " + nTaxAmt + ",                       ";

            SQL += ComNum.VBLF + "        Amt50 = " + nAmt[50] + "                       ";
            SQL += ComNum.VBLF + "WHERE TRSNO = " + ArgTrsNo + "                         ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 본인부담,조합부담액,할인액을 계산함
        /// 2017-09-19 김민철
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgSDate"></param>
        /// </summary>
        /// <seealso cref="IpdTewon.bas : Ipd_Tewon_Amt_Process"/>
        public bool Ipd_Tewon_Amt_Process(PsmhDb pDbCon, long ArgTrsNo, string strTemp, [Optional] string ArgSDate)
        {

            #region // 변수선언부
            clsIument cIuM = new clsIument();
            clsIuSentChk cISC = new clsIuSentChk();
            clsPmpaFunc cPF = new clsPmpaFunc();

            int i = 0, nNu = 0, nRead = 0, intRowCnt = 0;
            long nTaxAmt = 0, nKekliAmt = 0, nKekliAmt_Bon = 0;
            long nTotGubyo = 0, nTotBiGubyo = 0, nBonBiGubyo = 0, nBoninAmt = 0;
            long nAmt = 0, nFoodAmt = 0, nFoodSumAmt = 0, nFoodAmtBonin = 0, nFoodGaAmt = 0, nFoodGaAmtBonin = 0, nBohoFood = 0;
            long nAMT09 = 0, nAMT85 = 0, nAMT09_H = 0, nAMT85_H = 0;  //nAMT09_H = 0, nAMT85_H 2인실 40% ->45% 50%
            long nCTMRAmt = 0, nCTMRBonin = 0;
            long nToothAmt = 0, nToothBonin = 0;
            long nTot100Amt = 0, n100Amt = 0, nRtnAmt = 0;
            long nTuberEduAmt = 0; //결핵관리료, 상담료 2021-09-16
            long nBonGubyo = 0;
            long nHRoomAmt = 0;      //2인실 추가 2018-07-01
            long nHRoomBonin = 0;    //2인실 추가 2018-07-01

            string strBDate = string.Empty;
            string strBun = string.Empty;
            string strOutDate = string.Empty;
            string strSugbs = string.Empty;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";
            #endregion
           
            try
            {
                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, ArgTrsNo, strTemp);

                #region 특정일자부터 항목별 금액을 재계산(의료급여 승인관련)
                if (ArgSDate != "")
                {
                    for (i = 0; i < 51; i++)
                    {
                        clsPmpaType.TIT.Amt[i] = 0;
                    }
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT NU,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  GROUP BY Nu ";
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
                            nNu = Convert.ToUInt16(Dt.Rows[i]["NU"].ToString());
                            clsPmpaType.TIT.Amt[nNu] += Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());
                            clsPmpaType.TIT.Amt[44] += Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString());  //선택진료금액 재형성
                            clsPmpaType.TIT.Amt[50] += (Convert.ToInt64(Dt.Rows[i]["Amt"].ToString()) + Convert.ToInt64(Dt.Rows[i]["Amt2"].ToString()));
                        }
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                if (strTemp != "임시자격")
                {
                    //부가세
                    if (clsPmpaType.TIT.GbTax == "1") { nTaxAmt = (long)Math.Truncate((clsPmpaType.TIT.Amt[50] * (clsPmpaPb.BON_TAX[Convert.ToUInt16(clsPmpaType.TIT.Bi)] / 100.0))); }
                }
                
                //총진료비중 급여총액, 비급여 총액을 계산함
                for (i = 0; i < 50; i++)
                {
                    if (i > 0 && i < 21)
                    {
                        nTotGubyo += clsPmpaType.TIT.Amt[i];        //급여분 합계
                    }
                    else
                    {
                        nTotBiGubyo += clsPmpaType.TIT.Amt[i];       //비급여분 합계
                    }
                }

                //'약제상한금액 급여총액- 2011-03-30
                if (clsPmpaType.TIT.Amt[64] != 0)
                    nTotGubyo -= clsPmpaType.TIT.Amt[64];
                #endregion

                #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
                //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
                if (clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt                    ";
                    SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,    ";
                    SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b          ";
                    SQL += ComNum.VBLF + "   Where a.TRSNO = " + ArgTrsNo + "               ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)                   ";
                    SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100'                        ";    //수가상세분류
                    SQL += ComNum.VBLF + "     AND a.QTY <= 0.9                             ";
                    SQL += ComNum.VBLF + "     AND a.QTY >= 0.85                            ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT not in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                    SQL += ComNum.VBLF + "   GROUP By a.QTY                                 ";
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
                            nAmt = Convert.ToInt64(Dt.Rows[i]["AMT"].ToString());
                            if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                            {
                                nAMT09 = Convert.ToInt64(Dt.Rows[i]["AMT"].ToString());    //본인부담 25%
                            }
                            else
                            {
                                nAMT85 = Convert.ToInt64(Dt.Rows[i]["AMT"].ToString());    //본인부담 30%
                            }

                            nTotGubyo -= nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region 장기입원 대상자 입원료 본인부담 선별대상 구분 
                //선별급여와 마찬가지로 SLIP, 수가별 본인부담율이 틀린것에 대한 구분자 표시 본인부담율 구하는 방법 강구할것.
                if (string.Compare(clsPmpaType.TIT.InDate, "2020-01-01") >= 0 && clsPmpaType.TIT.FCode != "F014" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.VCode.Trim() == "" && clsPmpaType.TIT.OgPdBun.Trim() == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT a.QTY,SUM(a.AMT1) Amt                    ";
                    SQL += ComNum.VBLF + "    From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,    ";
                    SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN b          ";
                    SQL += ComNum.VBLF + "   Where a.TRSNO = " + ArgTrsNo + "               ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT = b.SUNEXT(+)                   ";
                    SQL += ComNum.VBLF + "     AND b.DTLBUN = '1100'                        ";    //수가상세분류
                    SQL += ComNum.VBLF + "     AND a.QTY <= 0.9                             ";
                    SQL += ComNum.VBLF + "     AND a.QTY >= 0.85                            ";
                    SQL += ComNum.VBLF + "     AND a.SUNEXT  in ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B') ";
                    SQL += ComNum.VBLF + "   GROUP By a.QTY                                 ";
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
                            nAmt = Convert.ToInt64(Dt.Rows[i]["AMT"].ToString());
                            if (Convert.ToDouble(Dt.Rows[i]["QTY"].ToString()) == 0.9)
                            {
                                nAMT09_H = (long)Math.Truncate(nAmt * 5 / 100.0);   //본인부담 5%가산   
                            }
                            else
                            {
                                nAMT85_H = (long)Math.Truncate(nAmt * 10 / 100.0);   //본인부담 5%가산   
                            }

                      
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region 식대계산
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUM(AMT1) FoodAmt                      ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                        ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + "                                                    ";
                SQL += ComNum.VBLF + "    AND BUN IN ('74')                                                             ";
                SQL += ComNum.VBLF + "    AND NU  = '16'                                                                "; //식대 급여
                SQL += ComNum.VBLF + "    AND SUCODE NOT IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020')   ";
                SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE, 1,1) NOT IN ('F')                                          "; //기존에 식대코드가 F로 시작한것 제외함."
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')                          ";
                SQL += ComNum.VBLF + "  GROUP BY BDATE                                                                  ";
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
                    nFoodAmt = Convert.ToInt64(Dt.Rows[0]["FoodAmt"].ToString());

                    for (i = 0; i < nRead; i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                        nFoodAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                        nFoodSumAmt += nFoodAmt;

                        if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                        {
                            nFoodAmtBonin += (long)Math.Truncate(nFoodAmt * clsPmpaType.IBR.Food / 100.0);
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,SUM(Amt1) FoodAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND SUCODE = 'F02T' ";
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')                          ";
                    SQL += ComNum.VBLF + "  GROUP BY BDATE ";
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
                            nAmt = Convert.ToInt64(Dt.Rows[i]["FoodAmt"].ToString());
                            if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V268" || clsPmpaType.TIT.VCode == "V275")
                            {
                                nBohoFood = nBohoFood;            //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 0%
                            }
                            else
                            { 
                                nBohoFood += nAmt;
                            }
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }

                #endregion

                #region 가산식대 계산
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUM(AMT1) FoodGaAmt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                SQL += ComNum.VBLF + "    AND BUN IN ('74') ";
                SQL += ComNum.VBLF + "    AND NU  = '16' "; //급여 식대
                SQL += ComNum.VBLF + "    AND SUCODE  IN ('Y1110','T1110','Z4200','Z0100','Z0011','Z0010','Z0020') ";
                if (ArgSDate != "") { SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') "; }
                SQL += ComNum.VBLF + "  GROUP BY  BUN ";
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
                    if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "21")
                        ComFunc.MsgBox("의료급여 환자에 가산식대가 발생하였습니다. 식대수가를 변경해주세요.", "확인");

                    nFoodGaAmt = Convert.ToInt64(Dt.Rows[0]["FoodGaAmt"].ToString());

                    if (clsPmpaType.TIT.Bi == "51" || clsPmpaType.TIT.Bi == "43" || clsPmpaType.TIT.Bi == "55" || clsPmpaType.TIT.Bi == "41" || clsPmpaType.TIT.Bi == "42")
                    {
                        nFoodGaAmtBonin = (long)Math.Truncate(((nFoodGaAmt * 100 / 100) * 0.1) * 10);                     //식대가산금액
                    }
                    else if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "33" || clsPmpaType.TIT.Bi == "32")
                    {
                        nFoodGaAmtBonin = (long)Math.Truncate(((nFoodGaAmt * (clsPmpaType.IBR.Food / 100.0)) * 0.1) * 10);           //식대가산금액
                    }
                    else
                    {
                        if (clsPmpaType.TIT.OgPdBun == "P")
                        { nFoodGaAmtBonin = 0; }                                    //식대가산금액
                        else if (clsPmpaType.TIT.OgPdBun == "C")             //차상위계층환자는 가산식대 없음
                        { nFoodGaAmtBonin = 0; }                                     //식대가산금액
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                        { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "E")   //차상위계층2 만성질환자는 가산식대 (전액청구) 본인0%'2009 - 04 - 01
                        { nFoodGaAmtBonin = 0; }                                      //식대가산금액
                        else if ((clsPmpaType.TIT.Bi == "13" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "11") && clsPmpaType.TIT.OgPdBun == "F")   //차상위계층2 장애인 만성질환자는 가산식대(전액청구) 본인0%'2009 - 04 - 01
                        { nFoodGaAmtBonin = 0; }                                        //식대가산금액
                        else
                        {
                            nFoodGaAmtBonin = (long)Math.Truncate(nFoodGaAmt * 0.5);    //식대가산금액
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
                #endregion

                #region CT,MRI 보험,보호는 외래 부담율을 적용함
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
                {
                    strOutDate = clsPmpaType.TIT.OutDate;

                    if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22" && string.Compare(clsPmpaType.TIT.InDate, "2012-01-01") >= 0)
                    {
                        //의료급여 환자인 경우 CT/MRI 본인부담이 없고, 재원자의 경우 퇴원일자가 없으면 정상로직을 타지 않아 조회당일을 퇴원일자로 임시로 세팅함. 
                        if (strOutDate == "") { strOutDate = clsPublic.GstrSysDate; }
                    }
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) CTMRIAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND BUN IN ('72','73') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND GBSUGBS ='0' ";
                    if (ArgSDate != "") { SQL += ComNum.VBLF + " && BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') "; }
                    SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
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
                            strBun = Dt.Rows[i]["Bun"].ToString().Trim();
                            nAmt = Convert.ToInt64(Dt.Rows[i]["CTMRIAmt"].ToString());
                            nCTMRAmt += nAmt;
                            
                            if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                            {
                                nCTMRBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.CTMRI / 100.0);
                            }
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }

                #endregion

                #region 의뢰회신서 100% 급여
                //의뢰회신서 100%급여 2018-05-01
                if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1) AMT100 ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND SUCODE IN ( 'IA221','IA231' ) ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                    SQL += ComNum.VBLF + "  GROUP BY BDate ";
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
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                            nRtnAmt += nAmt;

                            nTotGubyo = nTotGubyo - nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion


                #region 결핵관리료, 상담료 100% 급여
                //결핵관리료, 상담료 100%급여 2021-09-16
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate, SUM(Amt1) AMT100 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                SQL += ComNum.VBLF + "    AND SUCODE IN ( 'ID110','ID120','ID130' ) ";
                SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                if (ArgSDate != "")
                    SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD')         ";
                SQL += ComNum.VBLF + "  GROUP BY BDate ";
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
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["AMT100"].ToString()));
                        nTuberEduAmt += nAmt;

                        nTotGubyo = nTotGubyo - nAmt;
                    }
                }
                Dt.Dispose();
                Dt = null;
                #endregion

                #region 퇴장방지 약제 가산단가 본인부담 제외 

                if ((VB.Left(clsPmpaType.TIT.Bi, 1) == "1" || VB.Left(clsPmpaType.TIT.Bi, 1) == "2") && (clsPmpaType.TIT.DrgCode.Trim() == ""))
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT round(SUM(a.AMT1) -  (SUM(a.AMT1) / 1.1) )   Amt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ," + ComNum.DB_PMPA + "BAS_SUN b";
                    SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT(+)";
                    SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND b.sugbm = '1' ";
                    SQL += ComNum.VBLF + "    AND a.BDate>=TO_DATE('2020-03-01','YYYY-MM-DD')         ";
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
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                            nTotGubyo -= nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion

                #region 노인틀니 보험,보호 
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 && clsPmpaType.TIT.DeptCode == "DT")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT b.DTLBUN,TO_CHAR(a.BDATE,'YYYY-MM-DD') BDate,a.Bun,SUM(a.Amt1) ToothAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
                    SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND a.Pano = '" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND a.SUNEXT = b.SUNEXT ";
                    SQL += ComNum.VBLF + "    AND b.DTLBUN in ('4004','4003')  ";   //2017-07-12 기존 노인틀니에 임플란트('4003') 추가 (조건이 같음)
                    SQL += ComNum.VBLF + "    AND a.GbSelf ='0' ";
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND a.BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  GROUP BY b.DTLBUN, a.BDATE, a.BUN ";
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
                            strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                            nAmt = Convert.ToInt64(Dt.Rows[i]["ToothAmt"].ToString());
                            nToothAmt += nAmt;
                            
                            if (string.Compare(strBDate, clsPmpaType.IBR.SDate) >= 0)
                            {
                                if (Dt.Rows[i]["DTLBUN"].ToString() == "4004")
                                {
                                    nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt1 / 100.0);
                                }
                                else if (Dt.Rows[i]["DTLBUN"].ToString() == "4003")
                                {
                                    nToothBonin += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Dt2 / 100.0);
                                }   
                            }
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region 2인실 병실료  
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0 )
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDate,Bun,SUM(Amt1) nHRoomAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a ";
                    SQL += ComNum.VBLF + "  WHERE a.TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND a.SUCode in  ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B')   ";  
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND a.BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  GROUP BY BDATE, BUN ";
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
                            strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                            nAmt = Convert.ToInt64(Dt.Rows[i]["nHRoomAmt"].ToString());
                            nHRoomAmt += nAmt;
                            nHRoomBonin += (long)Math.Truncate(nAmt * 40 / 100.0);
                        }
                    }

                    nHRoomBonin += nAMT09_H + nAMT85_H;    //40% 본인부담에서 가산된 금액 추가

                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                #region TA환자 때문에 CT,MRI,SONO 변수에 저장함(비급여)
                if (clsPmpaType.TIT.Bi == "52")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(Amt1) CTMRIAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B ";
                    SQL += ComNum.VBLF + "  WHERE A.TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND A.BUN IN ('71','72','73') ";
                    SQL += ComNum.VBLF + "    AND A.GbSelf ='1' ";
                    SQL += ComNum.VBLF + "    AND A.NU > '20' ";
                    SQL += ComNum.VBLF + "    AND A.BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND A.BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND A.SUCODE = B.SUNEXT(+) ";
                    SQL += ComNum.VBLF + "    AND SUGBR = '1' ";
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
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                            nCTMRAmt += nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                    //보철료       
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(Amt1) CTMRIAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND BUN IN ('40') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND NU > '20' ";
                    if (ArgSDate != "")
                        SQL += ComNum.VBLF + " AND BDate>=TO_DATE('" + ArgSDate + "','YYYY-MM-DD') ";
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
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["CTMRIAmt"].ToString()));
                            nCTMRAmt += nAmt;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                }
                #endregion
                
                #region 100/100/80/50 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
                {
                    //100/100 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2014-03-26
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(Amt1) Amt,GbSuGbs ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('2','4','5','9') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2014-12-01','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
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
                            strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                            nTot100Amt += nAmt;

                            if (strSugbs == "4")
                                n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                            else if (strSugbs == "2")
                                n100Amt += (long)Math.Truncate(nAmt * 0.2);       //본인부담 20%
                            else if (strSugbs == "5")
                                n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%\
                            else if (strSugbs == "9")
                                n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                    //100/80, 100/50 본인일부 부담금중 조합부담금액 계산(급여본인부담금액에서 제외됨) 2016-08-30
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt,GbSuGbs ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7', '8') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
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
                            strSugbs = Dt.Rows[i]["GbSuGbs"].ToString().Trim();
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));
                            nTot100Amt += nAmt;

                            if (strSugbs == "6")
                                n100Amt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                            else if (strSugbs == "3")
                                n100Amt += (long)Math.Truncate(nAmt * 0.3);       //본인부담 50%
                            else if (strSugbs == "7")
                                n100Amt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                            else if (strSugbs == "8")
                                n100Amt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                        }
                    }
                    Dt.Dispose();
                    Dt = null;

                    //급여총액에서 100/80, 50 총액을 제외함
                    nTotGubyo = nTotGubyo - nTot100Amt;
                }
                #endregion

                #region 격리병실료 산정
                if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) KekliAmt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + ArgTrsNo + " ";
                    SQL += ComNum.VBLF + "    AND PANO ='" + clsPmpaType.TIT.Pano + "' ";
                    SQL += ComNum.VBLF + "    AND SUCode IN ('AJ010','AJ020','AK200','AK201','AK202','AK210','AK211','V6001','V6002','AK200A','AH001','AH002') "; //  AK200A  2017-07-18 ADD
                    SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2016-09-23','YYYY-MM-DD') ";
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
                            nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["KekliAmt"].ToString()));

                            nKekliAmt += nAmt;

                            if (clsPmpaType.TIT.VCode != "")
                            {
                                if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                                {
                                    if (clsPmpaType.TIT.VCode == "V000" || clsPmpaType.TIT.VCode == "V010")
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.0);
                                    }
                                    else
                                    {
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * clsPmpaType.IBR.Bohum / 100.0);   
                                    }
                                }
                            }
                            else
                            {
                                if (clsPmpaType.TIT.Bi == "11" || clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                                {

                                    if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age < 6 )
                                        nKekliAmt_Bon += 0 ;
                                    else if (clsPmpaType.TIT.OgPdBun == "E" && clsPmpaType.TIT.Age <= 15)
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                                    else if (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2" || clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H" || clsPmpaType.TIT.OgPdBun == "S" || clsPmpaType.TIT.OgPdBun == "Y")
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                                    else if (clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "C" || clsPmpaType.TIT.OgPdBun == "P")
                                        nKekliAmt_Bon += nKekliAmt_Bon;
                                    else
                                        nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "P")
                                {
                                 //   nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.0);
                                }
                                else if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.OgPdBun == "Y")
                                {
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.03);
                                }
                                else if (clsPmpaType.TIT.Bi == "22")
                                {
                                    //nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.05);
                                    //2021-06-17 의뢰서. 
                                    nKekliAmt_Bon += (long)Math.Truncate(nAmt * 0.1);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region //항결핵약제비
                clsPmpaPb.GnAntiTubeDrug_Amt = cISC.Gesan_AntiTubeDrug_Amt(pDbCon, ArgTrsNo);
                #endregion

                #region 본인부담액을 계산함
                //진료비 본인부담율
                nBonGubyo = (long)Math.Truncate((nTotGubyo - nCTMRAmt - nBohoFood - nFoodSumAmt - nFoodGaAmt - nToothAmt - nKekliAmt - nHRoomAmt ) * clsPmpaType.IBR.Bohum / 100.0);

                //+항목별 본인부담율

                if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "31" || clsPmpaType.TIT.Bi == "32"   )
                {
                    nFoodGaAmtBonin = 0;
                }

                if (clsPmpaType.TIT.Bi == "22" && clsPmpaType.TIT.Bohun == "3" )
                {
                    nBonGubyo += nFoodAmtBonin + nHRoomBonin;
                }
                else
                {
                    nBonGubyo += nCTMRBonin + nFoodAmtBonin + nFoodGaAmtBonin + nToothBonin + nKekliAmt_Bon + nHRoomBonin;
                }

                
                                
                nBonGubyo += (long)Math.Truncate(nAMT09 * 25 / 100.0);
                nBonGubyo += (long)Math.Truncate(nAMT85 * 30 / 100.0);

                //본인부담 상한제 계산
                if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0)
                {
                    Gesan_Upper_Limit(pDbCon, ref nBonGubyo , ref nHRoomBonin);
                }

                #endregion
                
                #region 비급여 본인부담액을 계산
                switch (clsPmpaType.TIT.Bi)         //비급여분 본인 부담액 (자보: SONO, C/T, MRI,보철료는 제외)
                {
                    case "52":
                        nBonBiGubyo = nTotBiGubyo - nCTMRAmt - clsPmpaType.TIT.Amt[36] - clsPmpaType.TIT.Amt[37] - clsPmpaType.TIT.Amt[38] - clsPmpaType.TIT.Amt[39] - clsPmpaType.TIT.Amt[44];
                        nBoninAmt = nBonGubyo + nBonBiGubyo;
                        break;
                    default:
                        nBonBiGubyo = nTotBiGubyo;
                        nBoninAmt = nBonGubyo + nBonBiGubyo + n100Amt;
                        break;
                }

                // TODO : DRG 계산 재확인
                //DRG 자격은 각 부담금 재계산함
                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "")
                {
                    nBoninAmt = (DRG.GnDrgBonAmt + DRG.GnDrgBiTAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0] + DRG.GnGs100Amt + DRG.GnDrg열외군금액_Bon + DRG.GnGs80Amt_B + DRG.GnGs50Amt_B);   //조합부담금 add kyo 2017-02-02

                    nBonGubyo = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0];
                    nBonBiGubyo = DRG.GnDrgBiTAmt + DRG.GnGs100Amt;
                }
 
                clsPmpaType.TIT.Amt[51] = 0;
                clsPmpaType.TIT.Amt[52] = 0;

                //DRG 자격은 각 부담금 재계산함
                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "") { clsPmpaType.TIT.Amt[50] = nTotGubyo + nTotBiGubyo; }   //KYO 2017-03-29 전산팀장 요청 TIT.Amt(53) DRG환자 로직 변경

                if (clsPmpaType.TIT.Amt[64] != 0)
                    clsPmpaType.TIT.Amt[53] = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64]) - nBoninAmt; //조합부담금
                else
                    clsPmpaType.TIT.Amt[53] = clsPmpaType.TIT.Amt[50] - nBoninAmt;  //조합부담금

                clsPmpaType.TIT.Amt[61] = nBonGubyo;     //퇴원계산서
                clsPmpaType.TIT.Amt[62] = nBonBiGubyo;   //발부시 사용
                #endregion

                #region //할인금액 계산
                clsPmpaType.TIT.Amt[54] = 0;
                clsPmpaType.TIT.Amt[55] = nBoninAmt;     //차인납부액

                if (string.Compare(clsPmpaType.TIT.GbGameK, "00") > 0)
                {
                    if (clsPmpaType.TIT.OutDate == "")
                    {
                        if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
                        {
                            clsPublic.GstrMsgList = "";
                            clsPublic.GstrMsgList += ComNum.VBLF + "계약처 감액인데 계약처 코드가 없습니다.";
                            clsPublic.GstrMsgList += ComNum.VBLF + "등록번호 : " + clsPmpaType.TIT.Pano;
                            clsPublic.GstrMsgList += ComNum.VBLF + "  진료과 : " + clsPmpaType.TIT.DeptCode;
                            ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                        }

                        //2013-12-23 소방처전문치료 협약관련 감액기준
                        if (clsPmpaType.TIT.GelCode == "H911")
                        {
                            if (IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                                return false;

                            clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                            clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                            clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                        }
                        else
                        {
                            if (IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                                return false;
                        }
                    }
                    else
                    {
                        //2013-12-23 소방처전문치료 협약관련 감액기준
                        if (clsPmpaType.TIT.GelCode == "H911")
                        {
                            if (IPD_Gamek_Account_H119(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPublic.GstrSysDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt) == false)
                                return false;

                            clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                            clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                            clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                        }
                        else
                        {
                            if (IPD_Gamek_Account_Main(pDbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.GbGameK, clsPmpaType.TIT.Bi, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.GelCode, clsPmpaType.TIT.BonRate, nBoninAmt, nCTMRBonin) == false)
                                return false;
                        }
                    }
                    clsPmpaType.TIT.Amt[54] = clsPmpaType.GAM.Halin_Tot;
                    clsPmpaType.TIT.DtGamek = clsPmpaType.GAM.DTHalin_Tot;
                    clsPmpaType.TIT.Amt[55] = nBoninAmt - clsPmpaType.GAM.Halin_Tot;
                }

                // TODO : DRG 계산 재확인
                //DRG 자격은 각 부담금 재계산함
                DRG.GnAmt1 = 0;
                DRG.GnAmt2 = 0;

                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "")
                {
                    DRG.GnAmt1 = nTotGubyo;
                    DRG.GnAmt2 = nTotBiGubyo;
                }

                #endregion

                //2016-08-12 수가부담율이 개별적인 금액은 따로 계산후 더해줌
                //=========================================================================================
                //nTotGubyo = nTotGubyo + nTot100Amt + nRtnAmt;   //100/80, 100/50
                nTotGubyo = nTotGubyo + nTot100Amt + nRtnAmt + nTuberEduAmt;   //100/80, 100/50         //결핵상담료, 관리료 2021-09-16
                nTotGubyo = nTotGubyo + nAMT09 + nAMT85;        //장기입원대상자 본인부담 25%, 30%

                DRG.GnAmt1 = 0;
                DRG.GnAmt2 = 0;
                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "")
                {
                    DRG.GnAmt1 = nTotGubyo;
                    DRG.GnAmt2 = nTotBiGubyo;
                }

                #region IPD_TRANS 금액 UPDATE
                //ArgSDate <> "" 이면 의료급여승인용 계산입니다.
                if (ArgSDate == "")
                {
                    SQL = "";
                    if (strTemp == "임시자격")
                    {
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                    }
                    SQL += ComNum.VBLF + "   SET SangAmt = " + clsPmpaType.TIT.SangAmt + ",";   //상한제 대상 금액
                    SQL += ComNum.VBLF + "       Amt50 = " + clsPmpaType.TIT.Amt[50] + ",";     //총진료비
                    SQL += ComNum.VBLF + "       Amt51 = " + clsPmpaType.TIT.Amt[51] + ",";     //보증금,중간납 대체액
                    SQL += ComNum.VBLF + "       Amt52 = " + clsPmpaType.TIT.Amt[52] + ",";     //사용안함
                    SQL += ComNum.VBLF + "       Amt53 = " + clsPmpaType.TIT.Amt[53] + ",";     //조합부담
                    SQL += ComNum.VBLF + "       Amt54 = " + clsPmpaType.TIT.Amt[54] + ",";     //할인액
                    SQL += ComNum.VBLF + "       Amt55 = " + clsPmpaType.TIT.Amt[55] + ",";     //차인납부
                    SQL += ComNum.VBLF + "       Amt56 = " + clsPmpaType.TIT.Amt[56] + ",";     //개인미수
                    SQL += ComNum.VBLF + "       Amt57 = " + clsPmpaType.TIT.Amt[57] + ",";     //퇴원금
                    SQL += ComNum.VBLF + "       Amt58 = " + clsPmpaType.TIT.Amt[58] + ",";     //환불금
                    SQL += ComNum.VBLF + "       Amt59 = " + clsPmpaType.TIT.Amt[59] + ",";
                    SQL += ComNum.VBLF + "       Amt60 = " + clsPmpaType.TIT.Amt[60] + ",";
                    if (strTemp != "임시자격")
                    {
                        SQL += ComNum.VBLF + "       Amt67 = " + nTaxAmt + ",";                     //부가세 2014-02-24
                    }
                    SQL += ComNum.VBLF + "       GBSANG = '" + clsPmpaType.TIT.GbSang + "',";
                    SQL += ComNum.VBLF + "       Amt61 = " + nTotGubyo + ", ";                  //급여총액
                    SQL += ComNum.VBLF + "       Amt62 = " + nBonGubyo + ", ";                  //급여 본인부담금
                    SQL += ComNum.VBLF + "       Amt63 = " + nTotBiGubyo + " ";                 //비급여
                    SQL += ComNum.VBLF + " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 상한제 마감일자 구하기
        /// </summary>
        /// <param name="nBonGubyo"></param>
        public void Gesan_Upper_Limit(PsmhDb pDbCon, ref long nBonGubyo, ref long nOtherAmt )
        {            
            string[] SangFDate_New = new string[21];
            string[] SangTDate_New = new string[21];
            string strToYear = string.Empty;
            string strSangDate = string.Empty;
            string strGbSang = string.Empty;

            int i = 0, kk = 0, jj = 0, x = 0, y = 0, nRead = 0;

            long nSangAmt = 0, nSangAmt_New = 0;
            long nBoninAmt_Temp = 0;
            long nOtherAmt_Temp = 0;
            long nTot100Amt = 0, n100BonAmt = 0;

            DataTable Dt = new DataTable();
            DataTable Dt2 = new DataTable();

            ComFunc CF = new ComFunc();

            string SQL = "";
            string SqlErr = "";
            
            //희귀 지원금 상한제 제외
            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && clsPmpaType.TIT.OgPdBun == "H") { return; }

            //코로나검사 상한제 제외
            if (clsPmpaType.TIT.FCode == "MT04")
            {
                return ;

            }

            nOtherAmt_Temp = nOtherAmt ;  //'2인실 병실료 금액은 본인부담 상한제서 제외

            #region 건강보험으로 최초입원일자 구함 - 상한제 처음날짜 
            strToYear = VB.Left(clsPmpaType.TIT.InDate, 4) + "-01-01";
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE                 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a,              ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS b                     ";
            SQL += ComNum.VBLF + "  WHERE a.Pano = '" + clsPmpaType.TIT.Pano + "'               ";
            SQL += ComNum.VBLF + "    AND a.IPDNO = b.IPDNO                                     ";
            SQL += ComNum.VBLF + "    AND b.InDate >= TO_DATE('" + strToYear + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND b.BI IN ('11','12','13')                              ";
            SQL += ComNum.VBLF + "    AND ( b.GBIPD NOT IN ('D') OR b.GBIPD IS NULL)            ";
            SQL += ComNum.VBLF + "  ORDER BY b.INDATE                                           ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                SangFDate_New[0] = Dt.Rows[0]["INDATE"].ToString().Trim();

                for (kk = 1; kk < 21; kk++)
                {
                    if (kk == 1)
                    {
                        SangFDate_New[kk] = SangFDate_New[kk - 1];
                        SangTDate_New[kk] = VB.Left(SangFDate_New[kk], 4) + "-12-31";   //상한제 마감일자
                    }
                    else
                    {
                        SangFDate_New[kk] = CF.DATE_ADD(pDbCon, SangTDate_New[kk - 1], 1);
                        SangTDate_New[kk] = VB.Left(SangFDate_New[kk], 4) + "-12-31";   //상한제 마감일자
                    }
                }
            }
            else
            {
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;
            
            #endregion

            for (kk = 1; kk < 21; kk++)
            {
                if (string.Compare(SangFDate_New[kk], clsPmpaType.TIT.InDate) <= 0 && string.Compare(SangTDate_New[kk], clsPmpaType.TIT.InDate) >= 0)
                    break;
            }

            if (kk == 21) { kk = 20; }

            #region 선별급여중 본인부담금은 제외
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TRSNO From " + ComNum.DB_PMPA + "IPD_TRANS A ";
            SQL += ComNum.VBLF + "  Where A.INDATE >= TO_DATE('" + SangFDate_New[kk] + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.OUTDATE <= TO_DATE('" + SangTDate_New[kk] + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.ACTDATE IS NOT NULL ";
            SQL += ComNum.VBLF + "    AND A.OUTDATE <> TRUNC(SYSDATE) "; //당일 자신의 금액은 제외하고 계산해야함.
            SQL += ComNum.VBLF + "    AND A.GBIPD NOT IN ('D') ";
            SQL += ComNum.VBLF + "    AND A.BI IN ('11','12','13') ";
            SQL += ComNum.VBLF + "    AND A.Pano = '" + clsPmpaType.TIT.Pano + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            nRead = Dt.Rows.Count;
            if (nRead > 0)
            {
                for (x = 0; x < nRead; x++)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(Amt1) Amt,GbSuGbs ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + Convert.ToUInt64(Dt.Rows[x]["TRSNO"].ToString()) + " ";
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('2','4','5','9') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "    AND BDate>=TO_DATE('2014-12-01','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                    SQL += ComNum.VBLF + " HAVING SUM(Amt1) <> 0 ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        for (y = 0; y < Dt2.Rows.Count; y++)
                        {
                            string strSugbs = Dt2.Rows[y]["GBSUGBS"].ToString().Trim();
                            long nAmt = Convert.ToInt64(Dt2.Rows[y]["Amt"].ToString());

                            if (strSugbs == "4")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                            else if (strSugbs == "5")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                            else if (strSugbs == "2")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.2);       //본인부담 20%
                            else if (strSugbs == "9")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                        }
                    }
                    Dt2.Dispose();
                    Dt2 = null;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt,GbSuGbs ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + Convert.ToUInt64(Dt.Rows[x]["TRSNO"].ToString()) + " ";
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','6','7','8') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "  GROUP BY GbSuGbs ";
                    SQL += ComNum.VBLF + "  HAVING SUM(Amt1) <> 0 ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        for (y = 0; y < Dt2.Rows.Count; y++)
                        {
                            string strSugbs = Dt2.Rows[y]["GBSUGBS"].ToString().Trim();
                            long nAmt = Convert.ToInt64(Dt2.Rows[y]["Amt"].ToString());
                            nTot100Amt += nAmt;

                            if (strSugbs == "6")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.8);       //본인부담 80%
                            else if (strSugbs == "3")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.3);       //본인부담 30%
                            else if (strSugbs == "7")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.5);       //본인부담 50%
                            else if (strSugbs == "8")
                                n100BonAmt += (long)Math.Truncate(nAmt * 0.9);       //본인부담 90%
                        }
                    }
                    Dt2.Dispose();
                    Dt2 = null;
                    //2인실 본인부담금액은 상한제에서 제외
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT /*+index(A INDEX_IPDNEWSL4)*/ SUM(Amt1) Amt ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO=" + Convert.ToUInt64(Dt.Rows[x]["TRSNO"].ToString()) + " ";
                    SQL += ComNum.VBLF + "    AND SUCODE IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B') ";
                    SQL += ComNum.VBLF + "    AND GbSelf ='0' ";
                    SQL += ComNum.VBLF + "  HAVING SUM(Amt1) <> 0 ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        for (y = 0; y < Dt2.Rows.Count; y++)
                        {
                            long nAmt = Convert.ToInt64(Dt2.Rows[y]["Amt"].ToString());
                            nTot100Amt += nAmt;

                            n100BonAmt += (long)Math.Truncate(nAmt * 0.4);       //본인부담 40%
                            
                        }
                    }
                    Dt2.Dispose();
                    Dt2 = null;


                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion
            //상한제 관련 쿼리
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(";
            for (jj = 21; jj < 49; jj++)
                SQL += ComNum.VBLF + " A.AMT" + jj + "+";
            SQL += ComNum.VBLF + " A.AMT49) BIAMT, ";
            SQL += ComNum.VBLF + "      SUM(AMT51+AMT57) AMT, SUM(AMT50-AMT64) AMT50, SUM(AMT53) AMT53 ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS A";
            SQL += ComNum.VBLF + " WHERE A.INDATE  >= TO_DATE('" + SangFDate_New[kk] + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.OUTDATE <= TO_DATE('" + SangTDate_New[kk] + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.ACTDATE IS NOT NULL ";
            SQL += ComNum.VBLF + "   AND A.OUTDATE <> TRUNC(SYSDATE) "; //당일 자신의 금액은 제외하고 계산해야함.
            //SQL += ComNum.VBLF + "   AND A.TRSNO <> " + clsPmpaType.TIT.Trsno + " ";
            SQL += ComNum.VBLF + "   AND A.GBIPD NOT IN ('D') ";
            SQL += ComNum.VBLF + "   AND NVL(A.FCODE,' ') <> 'MT04' ";
            SQL += ComNum.VBLF + "   AND A.BI IN ('11','12','13') ";
            SQL += ComNum.VBLF + "   AND A.Pano = '" + clsPmpaType.TIT.Pano + "' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                nSangAmt_New = Convert.ToInt64(VB.Val(Dt.Rows[0]["AMT50"].ToString())) - Convert.ToInt64(VB.Val(Dt.Rows[0]["BIAMT"].ToString())) - Convert.ToInt64(VB.Val(Dt.Rows[0]["AMT53"].ToString())) - n100BonAmt;

                clsPmpaType.TIT.SangAmt = 0;
                strGbSang = "";
                clsPmpaType.TIT.GbSang = "";

                //상한금액계산시 상한제 제외 금액은 계산후 처리
                nBoninAmt_Temp = nBonGubyo - nOtherAmt_Temp;      //2인실본인부담  제외

                //상한제 기준일자 및 금액 세팅 2015-01-13
                if (string.Compare(clsPmpaType.TIT.Bi, "20") <= 0)
                {
                    for (i = 0; i < 11; i++)
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, clsPmpaPb.GstrSangBdate[i]) >= 0)
                        {
                            strSangDate = clsPmpaPb.GstrSangBdate[i];
                            nSangAmt = clsPmpaPb.GnSangAmt[i];
                            break;
                        }
                    }

                    strGbSang = "8";

                    if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2018-01-01") >= 0)
                    {
                        strGbSang = "8";
                    }
                    else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2017-01-01") >= 0)
                    {
                        strGbSang = "7";
                    }
                    else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2016-01-01") >= 0)
                    {
                        strGbSang = "6";
                    }
                    else if (string.Compare(clsPmpaPb.GstrSangBdate[i], "2015-01-01") >= 0)
                    {
                        strGbSang = "5";
                    }

                    if (nBoninAmt_Temp + nSangAmt_New > nSangAmt)
                    {
                        clsPmpaType.TIT.SangAmt = nSangAmt_New + nBoninAmt_Temp - nSangAmt;

                        if (nSangAmt_New >= nSangAmt)
                        {
                            nBonGubyo = nOtherAmt_Temp;
                            if (clsPmpaType.TIT.SangAmt > 0) { clsPmpaType.TIT.GbSang = strGbSang; }
                        }
                        else
                        {
                            nBonGubyo = (nSangAmt - nSangAmt_New) + nOtherAmt_Temp;
                            clsPmpaType.TIT.GbSang = strGbSang;  //상한제 구분
                        }
                    }
                }
            }

   
            Dt.Dispose();
            Dt = null;
        }

        public bool IPD_Gamek_Account_Main(PsmhDb pDbCon, long ArgTRSNO, string ArgGbGamek, string ArgBi, string argOUTDATE, string ArgLtdCode, int ArgBonRate, long ArgBoninAmt, long ArgCtMrBonAmt)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();

            IPD_Gamek_Variable_Clear();

            //감액율을 찾아 변수에 저장함
            if (cPF.READ_GAMEK_RATE(pDbCon, ArgGbGamek, ArgBi, argOUTDATE, ArgLtdCode, "I") == false)
                return false;

            IPD_Gamek_Account_SUB(pDbCon, ArgTRSNO, ArgGbGamek, ArgBi, ArgBonRate, ArgBoninAmt, ArgCtMrBonAmt);

            return true;
        }

        public bool IPD_Gamek_Account_H119(PsmhDb pDbCon, long ArgTRSNO, string ArgGbGamek, string ArgBi, string argOUTDATE, string ArgLtdCode, int ArgBonRate, long ArgBoninAmt)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();

            IPD_Gamek_Variable_Clear();

            //감액율을 찾아 변수에 저장함
            cPF.READ_GAMEK_RATE_H911(pDbCon, ArgGbGamek, ArgBi, argOUTDATE, ArgLtdCode, ArgLtdCode);

            IPD_Gamek_Account_SUB_H119(pDbCon, ArgTRSNO, ArgGbGamek, ArgBi, ArgBonRate, ArgBoninAmt);

            return true;
        }

        void IPD_Gamek_Variable_Clear()
        {
            //감액율을 보관하는 변수
            clsPmpaType.GAM.GbGameK = ""; clsPmpaType.GAM.Gam_Rate = 0; clsPmpaType.GAM.Jin_Rate = 0;
            clsPmpaType.GAM.SONO_Rate = 0; clsPmpaType.GAM.MRI_Rate = 0; clsPmpaType.GAM.FOOD_Rate = 0;
            clsPmpaType.GAM.ROOM_Rate = 0; clsPmpaType.GAM.ER_Rate = 0;
            clsPmpaType.GAM.DTJin_Rate = 0; clsPmpaType.GAM.DTGam_Rate = 0;
            clsPmpaType.GAM.DT1_Rate = 0; clsPmpaType.GAM.DT2_Rate = 0; clsPmpaType.GAM.DT3_Rate = 0;
            clsPmpaType.GAM.Amt50_Rate = 0;

            //할인 계산금액
            clsPmpaType.GAM.Jin_Halin = 0; clsPmpaType.GAM.Gam_Halin = 0; clsPmpaType.GAM.SONO_Halin = 0;
            clsPmpaType.GAM.MRI_Halin = 0; clsPmpaType.GAM.FOOD_Halin = 0; clsPmpaType.GAM.ROOM_Halin = 0;
            clsPmpaType.GAM.ER_Halin = 0; clsPmpaType.GAM.DTJin_Halin = 0; clsPmpaType.GAM.DTGam_Halin = 0;
            clsPmpaType.GAM.DT1_Halin = 0; clsPmpaType.GAM.DT2_Halin = 0; clsPmpaType.GAM.DT3_Halin = 0;
            clsPmpaType.GAM.Halin_Tot = 0; clsPmpaType.GAM.DTHalin_Tot = 0;
        }

        void IPD_Gamek_Account_SUB(PsmhDb pDbCon, long ArgTRSNO, string ArgGbGamek, string ArgBi, int ArgBonRate, long ArgBoninAmt, long ArgCtMrBonAmt)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            string strDeptCode = string.Empty;
            string strNu = string.Empty;
            string strSugbF = string.Empty;
            string strSugbK = string.Empty;
            string strDtlBun = string.Empty;
          
            long nAmt = 0, nGub = 0, nBigub = 0, nDT_Bigub = 0, nDT_Gub = 0;
            long nCtMriOldAmt = 0, nCtMriAmt = 0;
            long nFoodAmt = 0, nRoomAmt = 0, nSonoAmt = 0, nMriAmt = 0;
            long nJin1Amt = 0, nJin2Amt = 0, nEr1Amt = 0, nEr2Amt = 0;
            long nDTAmt_TO = 0, nDT_Jin1Amt = 0, nDT_Jin2Amt = 0, nDT_Er1Amt = 0, nDT_Er2Amt = 0;
            long nDT1_Amt = 0, nDT2_Amt = 0, nDT3_Amt = 0;
            long nNotHalin = 0, nNotSHalin = 0;
            long nSangAmt = 0, nSangAmt_Temp = 0;
            long nHal = 0, nHal2 = 0, nHal3 = 0, nHal4 = 0;

            //계약처 일반 및 산재공상 할인액 계산
            if (ArgGbGamek == "55" && (ArgBi == "33" || ArgBi == "51"))
            {
                #region Gamek_Amt_Account_LTD
                //계약처가 일반,산재공상 할인 대상이 아니면
                if (clsPmpaType.GAM.Amt50_Rate == 0) { return; }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun,SUM(a.Amt1+a.Amt2) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
                SQL += ComNum.VBLF + "  ORDER BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strDeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();
                        strNu = Dt.Rows[i]["Nu"].ToString().Trim();
                        strSugbF = Dt.Rows[i]["SugbF"].ToString().Trim();
                        strSugbK = Dt.Rows[i]["SugbK"].ToString().Trim();
                        strDtlBun = Dt.Rows[i]["DtlBun"].ToString().Trim();
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));

                        if (ArgBi == "33")  //산재공상
                        {
                            if (strDtlBun != "3301" && strDtlBun != "3302" && strDtlBun != "4701" && strDtlBun != "4702")       //필름복사,CD복사,진단서,소견서
                            {
                                if (strDeptCode == "DT")
                                    nDT_Bigub += nAmt;
                                else
                                    nBigub += nAmt;
                            }
                        }
                        else
                        {
                            if (strDeptCode == "DT")
                                nDT_Bigub += nAmt;
                            else
                                nBigub += nAmt;
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                clsPmpaType.GAM.Gam_Halin = (long)Math.Truncate(nBigub * clsPmpaType.GAM.Amt50_Rate / 100.0);
                clsPmpaType.GAM.DTGam_Halin = (long)Math.Truncate(nDT_Bigub * clsPmpaType.GAM.Amt50_Rate / 100.0);
                clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Gam_Halin + clsPmpaType.GAM.DTGam_Halin;
                #endregion
                return;
            }

            //공단,직장,지역 CT,MRI 급여액 계산
            if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
            {
                #region Gamek_Amt_Account_CTMRI
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate,SUM(a.Amt1+a.Amt2) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND b.SugbK <> '1' ";
                SQL += ComNum.VBLF + "  GROUP BY a.BDate ";
                SQL += ComNum.VBLF + "  ORDER BY a.BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));

                        if (string.Compare(Dt.Rows[i]["BDate"].ToString().Trim(), clsPmpaPb.OBON_DATE) < 0)
                            nCtMriOldAmt += nAmt;
                        else
                            nCtMriAmt += nAmt;
                    }
                }
                #endregion
            }

            #region IPD_SLIP에서 감액 대상 금액을 읽음
            if (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun,SUM(a.Amt1+a.Amt2) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                //공단,직장,지역 CT,MRI 급여액 계산
                if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
                    SQL += ComNum.VBLF + " AND NOT (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND A.GBSELF <> '0' ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL += ComNum.VBLF + "    AND A.SUNEXT NOT IN ('DRG001','DRG002') ";
                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun ";
                SQL += ComNum.VBLF + "  ORDER BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun,SUM(a.Amt1+a.Amt2) Amt";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                //공단,직장,지역 CT,MRI 급여액 계산
                if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
                    SQL += ComNum.VBLF + " AND NOT (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL += ComNum.VBLF + "    AND A.SUNEXT NOT IN ('US31','DRG001','DRG002') ";
                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun ";
                SQL += ComNum.VBLF + "  ORDER BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
            }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    //자격이 51종인 총 진료비에서 20% 감액임
                    strDeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();
                    strNu = Dt.Rows[i]["Nu"].ToString().Trim();
                    strSugbF = Dt.Rows[i]["SugbF"].ToString().Trim();
                    strSugbK = Dt.Rows[i]["SugbK"].ToString().Trim();
                    strDtlBun = Dt.Rows[i]["DtlBun"].ToString().Trim();
                    nAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["Amt"].ToString()));

                    if (ArgBi == "51") //2007-08-06 수정함.
                    {
                        nGub += nAmt;
                    }
                    else
                    {
                        if (strNu == "34")     //비급여 식대
                        { nFoodAmt += nAmt; }
                        else if (strNu == "35") //병실차액
                        { nRoomAmt += nAmt; }
                        else if (strNu == "21" && strDtlBun =="1100") //병실차액 + 1인실비급여병실료
                        { nRoomAmt += nAmt; }
                        else if (strNu == "36") //초음파
                        { nSonoAmt += nAmt; }
                        else if (strNu == "38") //MRI
                        { nMriAmt += nAmt; }
                        //일반과(치과제외) 할인 대상금액을 누적
                        else if (clsPmpaType.TIT.DeptCode != "DT")
                        {
                            if (strDtlBun == "0101") //진찰료 kyo
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nJin2Amt += nAmt;
                                else
                                    nJin1Amt += nAmt;
                            }
                            else if (strDtlBun == "0103") //응급관리료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nEr2Amt += nAmt;      //급여
                                else
                                    nEr1Amt += nAmt;      //비급여
                            }
                            //재단성직자 감액(본인부담전액 감액)
                            else if (clsPmpaType.GAM.GbGameK == "11")
                            {
                                if (string.Compare(strNu, "20") > 0)
                                {
                                    nBigub += nAmt;
                                }
                                else
                                {
                                    nGub += nAmt;
                                }
                            }
                            else if (strSugbK != "1")
                            {
                                if (string.Compare(strNu, "20") > 0)
                                {
                                    nBigub += nAmt;
                                }
                                else
                                {

                                    nGub += nAmt;

                                }
                                  
                            }
                        }
                        else
                        {
                            //DT 치과 할인대상 금액 누적
                            nDTAmt_TO += nAmt;
                            if (strDtlBun == "0101") //진찰료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nDT_Jin2Amt += nAmt;
                                else
                                    nDT_Jin1Amt += nAmt;
                            }
                            else if (strDtlBun == "0103")   //응급관리료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nDT_Er2Amt += nAmt;
                                else
                                    nDT_Er1Amt += nAmt;
                            }
                            else if (strSugbF == "1")           //치과 비급여
                            {
                                if (strDtlBun == "4002")        //보철
                                    nDT2_Amt += nAmt;
                                else if (strDtlBun == "4003")   //임플란트
                                    nDT3_Amt += nDT3_Amt;
                                else
                                    nDT1_Amt += nDT1_Amt;
                            }
                            else //치과 급여
                            {
                                if (strSugbK != "1")    //수가코드에 할인대상인 수가만
                                {
                                    nNotHalin += nAmt;
                                    if (string.Compare(strNu, "20") > 0)
                                        nDT_Bigub += nAmt; //치과비급여
                                    else
                                        nDT_Gub += nAmt;     //치과급여
                                }
                                else if (clsPmpaType.GAM.GbGameK == "11")   //재단성직자 감액제외 금액을 누적함
                                {
                                    nNotHalin += nAmt;
                                    nNotSHalin += nAmt;   //2012-05-11
                                }
                            }
                        }
                    }
                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion

            #region 2006-06-01일부터 식대 비급여부분도 감액에서 제외됨.
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1+AMT2) FOODAMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "    AND BUN  = '74' ";
            SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('2006-06-01','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SUCODE IN ('F01D','F20A','F20B','FD019') ";    //보호자식대, 공추, 공추(HD), 서양식
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["FOODAMT"].ToString() != "")
                {
                    nFoodAmt -= Convert.ToInt64(VB.Val(Dt.Rows[0]["FOODAMT"].ToString()));
                }
            }

            Dt.Dispose();

            Dt = null;
            #endregion

            #region 2인실 병실료 산정은 따로 2018-10-10

            long Temp_nHRoomAmt = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1+AMT2) RoomAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "    AND SUCODE IN ('AB270','AB2701','AB270A','AB2701A','AO280', 'AO2801', 'AV8201', 'AV820', 'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B')  ";    //보호자식대, 공추, 공추(HD), 서양식
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["RoomAmt"].ToString() != "")
                {
                    nGub -= Convert.ToInt64(VB.Val(Dt.Rows[0]["RoomAmt"].ToString()));
                }
                Temp_nHRoomAmt = Convert.ToInt64(VB.Val(Dt.Rows[0]["RoomAmt"].ToString()));
                Temp_nHRoomAmt = (long)Math.Truncate(Temp_nHRoomAmt * 40 / 100.0);
            }

            Dt.Dispose();

            Dt = null;
            #endregion


            #region 할인 금액을 항목별로 계산함

            #region 일반 진료과 감액 2017-08-05 대불금있을시 제외하고 할인 적용수정

           // nJin1Amt = (long)Math.Truncate(nJin1Amt     * ArgBonRate / 100.0);
           // nEr1Amt      = (long)Math.Truncate(nEr1Amt      * ArgBonRate / 100.0);
           // nGub         = (long)Math.Truncate(nGub         * ArgBonRate / 100.0);
           // nGub         =  nGub + Temp_nHRoomAmt;
           // nCtMriAmt = (long)Math.Truncate(nCtMriAmt * clsPmpaPb.OLD_OBON[Convert.ToUInt16(ArgBi)] / 100.0);
           // nCtMriOldAmt = (long)Math.Truncate(nCtMriOldAmt * clsPmpaPb.OLD_OBON[Convert.ToUInt16(ArgBi)] / 100.0);
           // nDT_Jin1Amt  = (long)Math.Truncate(nDT_Jin1Amt  * ArgBonRate / 100.0);
           // nDT_Gub      = (long)Math.Truncate(nDT_Gub      * ArgBonRate / 100.0);


            nJin1Amt = (long)Math.Truncate(nJin1Amt * clsPmpaType.IBR.Bohum / 100.0);
            nEr1Amt = (long)Math.Truncate(nEr1Amt * clsPmpaType.IBR.Bohum / 100.0);
            nGub = (long)Math.Truncate(nGub * clsPmpaType.IBR.Bohum / 100.0);
            nGub = nGub + Temp_nHRoomAmt;
            nCtMriAmt = (long)Math.Truncate(nCtMriAmt * clsPmpaType.IBR.CTMRI / 100.0);
            nCtMriOldAmt = (long)Math.Truncate(nCtMriOldAmt * clsPmpaType.IBR.CTMRI / 100.0);
            nDT_Jin1Amt = (long)Math.Truncate(nDT_Jin1Amt * clsPmpaType.IBR.Bohum / 100.0);
            nDT_Gub = (long)Math.Truncate(nDT_Gub * clsPmpaType.IBR.Bohum / 100.0);

            nSangAmt = clsPmpaType.TIT.SangAmt;
            if (clsPmpaType.TIT.OgPdBun == "P")
            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nJin1Amt;
                if (nJin1Amt - nSangAmt >= 0)
                {
                    nJin1Amt = nJin1Amt - nSangAmt;
                }
                else
                {
                    nJin1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nEr1Amt;
                if (nEr1Amt - nSangAmt >= 0)
                {
                    nEr1Amt = nEr1Amt - nSangAmt;
                }
                else
                {
                    nEr1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nGub;
                if (nGub - nSangAmt >= 0)
                {
                    nGub = nGub - nSangAmt;
                }
                else
                {
                    nGub = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = ArgCtMrBonAmt;
                if (ArgCtMrBonAmt - nSangAmt >= 0)
                {
                    ArgCtMrBonAmt = ArgCtMrBonAmt - nSangAmt;
                }
                else
                {
                    ArgCtMrBonAmt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nCtMriOldAmt;
                if (nCtMriOldAmt - nSangAmt >= 0)
                {
                    nCtMriOldAmt = nCtMriOldAmt - nSangAmt;
                }
                else
                {
                    nCtMriOldAmt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nDT_Jin1Amt;
                if (nDT_Jin1Amt - nSangAmt >= 0)
                {
                    nDT_Jin1Amt = nDT_Jin1Amt - nSangAmt;
                }
                else
                {
                    nDT_Jin1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nDT_Gub;
                if (nDT_Gub - nSangAmt >= 0)
                {
                    nDT_Gub = nDT_Gub - nSangAmt;
                }
                else
                {
                    nDT_Gub = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }
            #endregion

            if (clsPmpaType.GAM.Jin_Rate > 0) { clsPmpaType.GAM.Jin_Halin = (long)Math.Truncate((nJin1Amt + nJin2Amt) * clsPmpaType.GAM.Jin_Rate / 100.0); }
            if (clsPmpaType.GAM.ER_Rate > 0) { clsPmpaType.GAM.ER_Halin = (long)Math.Truncate((nEr1Amt + nEr2Amt) * clsPmpaType.GAM.ER_Rate / 100.0); }
            if (clsPmpaType.GAM.Gam_Rate > 0)
            {
                clsPmpaType.GAM.Gam_Halin = (long)Math.Truncate((nGub + nBigub) * clsPmpaType.GAM.Gam_Rate / 100.0);
            }

            if (clsPmpaType.GAM.SONO_Rate > 0) { clsPmpaType.GAM.SONO_Halin = (long)Math.Truncate(nSonoAmt * clsPmpaType.GAM.SONO_Rate / 100.0); }
            if (clsPmpaType.GAM.MRI_Rate > 0) { clsPmpaType.GAM.MRI_Halin = (long)Math.Truncate(nMriAmt * clsPmpaType.GAM.MRI_Rate / 100.0); }
            if (clsPmpaType.GAM.FOOD_Rate > 0) { clsPmpaType.GAM.FOOD_Halin = (long)Math.Truncate(nFoodAmt * clsPmpaType.GAM.FOOD_Rate / 100.0); }
            if (clsPmpaType.GAM.ROOM_Rate > 0) { clsPmpaType.GAM.ROOM_Halin = (long)Math.Truncate(nRoomAmt * clsPmpaType.GAM.ROOM_Rate / 100.0); }

            //이미 급여수가(nGub)에 포함되므로 로직에서 제외시킴 KMC 2018-06-18
            //급여 CT,MRI 감액 계산
            if (clsPmpaType.GAM.Gam_Rate > 0 && (nCtMriAmt != 0 || nCtMriOldAmt != 0))
            {
                clsPmpaType.GAM.Gam_Halin += (long)Math.Truncate(ArgCtMrBonAmt * clsPmpaType.GAM.Gam_Rate / 100.0);
                clsPmpaType.GAM.Gam_Halin += (long)Math.Truncate(nCtMriOldAmt * clsPmpaType.GAM.Gam_Rate / 100.0);
            }

            //치과 감액
            if (clsPmpaType.GAM.DTJin_Rate > 0) { clsPmpaType.GAM.DTJin_Halin = (long)Math.Truncate((nDT_Jin1Amt + nDT_Jin2Amt) * clsPmpaType.GAM.DTGam_Rate / 100.0); }
            if (clsPmpaType.GAM.DTGam_Rate > 0) { clsPmpaType.GAM.DTGam_Halin = (long)Math.Truncate((nDT_Gub + nDT_Bigub) * clsPmpaType.GAM.DTGam_Rate / 100.0); }
            if (clsPmpaType.GAM.DT1_Rate > 0) { clsPmpaType.GAM.DT1_Halin = (long)Math.Truncate(nDT1_Amt * clsPmpaType.GAM.DT1_Rate / 100.0); }
            if (clsPmpaType.GAM.DT2_Rate > 0) { clsPmpaType.GAM.DT2_Halin = (long)Math.Truncate(nDT2_Amt * clsPmpaType.GAM.DT2_Rate / 100.0); }
            if (clsPmpaType.GAM.DT3_Rate > 0) { clsPmpaType.GAM.DT3_Halin = (long)Math.Truncate(nDT3_Amt * clsPmpaType.GAM.DT3_Rate / 100.0); }

            //치과 감액 합계액을 계산
            clsPmpaType.GAM.DTHalin_Tot = clsPmpaType.GAM.DTJin_Halin + clsPmpaType.GAM.DTGam_Halin + clsPmpaType.GAM.DT1_Halin;
            clsPmpaType.GAM.DTHalin_Tot = clsPmpaType.GAM.DTHalin_Tot + clsPmpaType.GAM.DT2_Halin + clsPmpaType.GAM.DT3_Halin;

            //전체감액 합계금액을 계산
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Jin_Halin + clsPmpaType.GAM.Gam_Halin + clsPmpaType.GAM.SONO_Halin + clsPmpaType.GAM.MRI_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.FOOD_Halin + clsPmpaType.GAM.ROOM_Halin + clsPmpaType.GAM.ER_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.DTJin_Halin + clsPmpaType.GAM.DTGam_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.DT1_Halin + clsPmpaType.GAM.DT2_Halin + clsPmpaType.GAM.DT3_Halin;

            //재단성직자 할인제외가 없으면 본인부담액을 감액으로 처리
            if (clsPmpaType.GAM.GbGameK == "11")
            {
                SQL = " SELECT AMT54 From " + ComNum.DB_PMPA + "IPD_TRANS Where Trsno = " + ArgTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    nHal = Convert.ToInt64(VB.Val(Dt.Rows[0]["AMT54"].ToString().Trim()));
                }

                Dt.Dispose();
                Dt = null;

                //치과 본인부담금 총액
                nHal2 = (nDT_Jin1Amt ) + nDT_Jin2Amt;         //치과 진찰료
                nHal2 = nHal2 + (nDT_Gub  + nDT_Bigub);     //치과 진료비
                nHal2 = nHal2 + nDT1_Amt + nDT2_Amt + nDT3_Amt + nNotSHalin;    //비급여 + 보철 + 임플란트 + 할인안됨

                //치과제외 할인 금액(사실상 치과제외한 총금액 이므로 100.0% 감액)
                nHal3 = ArgBoninAmt - nHal2;

                //최종 치과할인 금액과 치과제외한 금액을 더함 (원래 감액되어야 할 금액)
                nHal4 = nHal3 + clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.GAM.Halin_Tot = nHal4;

                ComFunc.MsgBox("재단성직자입니다. 치과진료비 발생시 감액여부를 반드시 확인해주세요.", "확인");
            }

            //할인액이 본인부담액을 초과하면 본인부담액을 할인액으로 처리
            if (clsPmpaType.GAM.Halin_Tot > ArgBoninAmt)
                clsPmpaType.GAM.Halin_Tot = ArgBoninAmt;

            #endregion
        }

        void IPD_Gamek_Account_SUB_H119(PsmhDb pDbCon, long ArgTRSNO, string ArgGbGamek, string ArgBi, int ArgBonRate, long ArgBoninAmt)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            string strDeptCode = string.Empty;
            string strNu = string.Empty;
            string strSugbF = string.Empty;
            string strSugbK = string.Empty;
            string strDtlBun = string.Empty;
            long nAmt = 0, nGub = 0, nBigub = 0, nDT_Bigub = 0, nDT_Gub = 0;
            long nCtMriOldAmt = 0, nCtMriAmt = 0;
            long nFoodAmt = 0, nRoomAmt = 0, nSonoAmt = 0, nMriAmt = 0;
            long nJin1Amt = 0, nJin2Amt = 0, nEr1Amt = 0, nEr2Amt = 0;
            long nDTAmt_TO = 0, nDT_Jin1Amt = 0, nDT_Jin2Amt = 0, nDT_Er1Amt = 0, nDT_Er2Amt = 0;
            long nDT1_Amt = 0, nDT2_Amt = 0, nDT3_Amt = 0;
            long nNotHalin = 0, nNotSHalin = 0;
            long nSangAmt = 0, nSangAmt_Temp = 0;
            long nHal = 0, nHal2 = 0, nHal3 = 0, nHal4 = 0;

            //공단,직장,지역 CT,MRI 급여액 계산
            if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
            {
                #region Gamek_Amt_Account_CTMRI
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate,SUM(a.Amt1+a.Amt2) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                SQL += ComNum.VBLF + "    AND (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND b.SugbK <> '1' ";
                SQL += ComNum.VBLF + "  GROUP BY a.BDate ";
                SQL += ComNum.VBLF + "  ORDER BY a.BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());

                        if (string.Compare(Dt.Rows[i]["BDate"].ToString().Trim(), clsPmpaPb.OBON_DATE) < 0)
                            nCtMriOldAmt += nAmt;
                        else
                            nCtMriAmt += nAmt;
                    }
                }
                #endregion
            }

            #region IPD_SLIP에서 감액 대상 금액을 읽음
            if (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.OgPdBun == "P")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun,SUM(a.Amt1+a.Amt2) Amt ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                //공단,직장,지역 CT,MRI 급여액 계산
                if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
                    SQL += ComNum.VBLF + " AND NOT (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND A.GBSELF <> '0' ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL += ComNum.VBLF + "    AND A.SUNEXT NOT IN ('DRG001','DRG002') ";
                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun ";
                SQL += ComNum.VBLF + "  ORDER BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun,SUM(a.Amt1+a.Amt2) Amt";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT b,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN c ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO = " + ArgTRSNO + " ";
                //공단,직장,지역 CT,MRI 급여액 계산
                if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13")
                    SQL += ComNum.VBLF + " AND NOT (a.Bun IN ('72','73') And a.Nu <= '20') ";
                SQL += ComNum.VBLF + "    AND a.Sucode = b.Sucode(+) ";
                SQL += ComNum.VBLF + "    AND a.SuNext = c.SuNext(+) ";
                SQL += ComNum.VBLF + "    AND A.SUNEXT NOT IN ('US31','DRG001','DRG002') ";
                SQL += ComNum.VBLF + "  GROUP BY a.DeptCode,a.Nu, b.SugbK,b.SugbF,c.DtlBun ";
                SQL += ComNum.VBLF + "  ORDER BY a.DeptCode,a.Nu,b.SugbK,b.SugbF,c.DtlBun ";
            }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    //자격이 51종인 총 진료비에서 20% 감액임
                    strDeptCode = Dt.Rows[i]["DeptCode"].ToString().Trim();
                    strNu = Dt.Rows[i]["Nu"].ToString().Trim();
                    strSugbF = Dt.Rows[i]["SugbF"].ToString().Trim();
                    strSugbK = Dt.Rows[i]["SugbK"].ToString().Trim();
                    strDtlBun = Dt.Rows[i]["DtlBun"].ToString().Trim();
                    nAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString());

                    if (ArgBi == "51") //2007-08-06 수정함.
                    {
                        nGub += nAmt;
                    }
                    else
                    {
                        if (strNu == "34")     //비급여 식대
                        { nFoodAmt += nAmt; }
                        else if (strNu == "35") //병실차액
                        { nRoomAmt += nAmt; }
                        else if (strNu == "21" && strDtlBun == "1100") //병실차액 + 1인실비급여병실료
                        { nRoomAmt += nAmt; }
                        else if (strNu == "36") //초음파
                        { nSonoAmt += nAmt; }
                        else if (strNu == "38") //MRI
                        { nMriAmt += nAmt; }
                        //일반과(치과제외) 할인 대상금액을 누적
                        else if (strDeptCode != "DT")
                        {
                            if (strDtlBun == "0101") //진찰료 kyo
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nJin2Amt += nAmt;
                                else
                                    nJin1Amt += nAmt;
                            }
                            else if (strDtlBun == "0103") //응급관리료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nEr2Amt += nAmt;      //급여
                                else
                                    nEr1Amt += nAmt;      //비급여
                            }
                            //재단성직자 감액(본인부담전액 감액)
                            else if (clsPmpaType.GAM.GbGameK == "11")
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nBigub += nAmt;
                                else
                                    nGub += nAmt;
                            }
                            else if (strSugbK != "1")
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nBigub += nAmt;
                                else
                                    nGub += nAmt;
                            }
                        }
                        else
                        {
                            //DT 치과 할인대상 금액 누적
                            nDTAmt_TO += nAmt;
                            if (strDtlBun == "0101") //진찰료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nDT_Jin2Amt += nAmt;
                                else
                                    nDT_Jin1Amt += nAmt;
                            }
                            else if (strDtlBun == "0103")   //응급관리료
                            {
                                if (string.Compare(strNu, "20") > 0)
                                    nDT_Er2Amt += nAmt;
                                else
                                    nDT_Er1Amt += nAmt;
                            }
                            else if (strSugbF == "1")           //치과 비급여
                            {
                                if (strDtlBun == "4002")        //보철
                                    nDT2_Amt += nAmt;
                                else if (strDtlBun == "4003")   //임플란트
                                    nDT3_Amt += nDT3_Amt;
                                else
                                    nDT1_Amt += nDT1_Amt;
                            }
                            else //치과 급여
                            {
                                if (strSugbK != "1")    //수가코드에 할인대상인 수가만
                                {
                                    nNotHalin += nAmt;
                                    if (string.Compare(strNu, "20") > 0)
                                        nDT_Bigub += nAmt; //치과비급여
                                    else
                                        nDT_Gub += nAmt;     //치과급여
                                }
                                else if (clsPmpaType.GAM.GbGameK == "11")   //재단성직자 감액제외 금액을 누적함
                                {
                                    nNotHalin += nAmt;
                                    nNotSHalin += nAmt;   //2012-05-11
                                }
                            }
                        }
                    }
                }
            }
            Dt.Dispose();
            Dt = null;
            #endregion

            #region 2006-06-01일부터 식대 비급여부분도 감액에서 제외됨.
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(AMT1+AMT2) FOODAMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE TRSNO = " + ArgTRSNO + " ";
            SQL += ComNum.VBLF + "    AND BUN  = '74' ";
            SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('2006-06-01','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SUCODE IN ('F01D','F20A','F20B','FD019') ";    //보호자식대, 공추, 공추(HD), 서양식
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                nFoodAmt -= Convert.ToInt64(VB.Val(Dt.Rows[0]["FOODAMT"].ToString()));
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region 할인 금액을 항목별로 계산함

            #region 일반 진료과 감액 2017-08-05 대불금있을시 제외하고 할인 적용수정
            nJin1Amt =  (long)Math.Truncate(nJin1Amt * ArgBonRate / 100.0);
            nEr1Amt = (long)Math.Truncate(nEr1Amt * ArgBonRate / 100.0);
            nGub =  (long)Math.Truncate(nGub * ArgBonRate / 100.0);
            nCtMriOldAmt =  (long)Math.Truncate(nCtMriOldAmt * clsPmpaPb.OLD_OBON[Convert.ToUInt16(ArgBi)] / 100.0);
            nCtMriAmt = (long)Math.Truncate(nCtMriAmt * clsPmpaPb.OLD_OBON[Convert.ToUInt16(ArgBi)] / 100.0);
            nDT_Jin1Amt =  (long)Math.Truncate(nDT_Jin1Amt * ArgBonRate / 100.0);
            nDT_Gub =  (long)Math.Truncate(nDT_Gub * ArgBonRate / 100.0);
            nSangAmt = clsPmpaType.TIT.SangAmt;

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nJin1Amt;
                if (nJin1Amt - nSangAmt >= 0)
                {
                    nJin1Amt = nJin1Amt - nSangAmt;
                }
                else
                {
                    nJin1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nEr1Amt;
                if (nEr1Amt - nSangAmt >= 0)
                {
                    nEr1Amt = nEr1Amt - nSangAmt;
                }
                else
                {
                    nEr1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nGub;
                if (nGub - nSangAmt >= 0)
                {
                    nGub = nGub - nSangAmt;
                }
                else
                {
                    nGub = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nCtMriOldAmt;
                if (nCtMriOldAmt - nSangAmt >= 0)
                {
                    nCtMriOldAmt = nCtMriOldAmt - nSangAmt;
                }
                else
                {
                    nCtMriOldAmt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nDT_Jin1Amt;
                if (nDT_Jin1Amt - nSangAmt >= 0)
                {
                    nDT_Jin1Amt = nDT_Jin1Amt - nSangAmt;
                }
                else
                {
                    nDT_Jin1Amt = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }

            if (nSangAmt > 0)
            {
                nSangAmt_Temp = nDT_Gub;
                if (nDT_Gub - nSangAmt >= 0)
                {
                    nDT_Gub = nDT_Gub - nSangAmt;
                }
                else
                {
                    nDT_Gub = 0;
                }
                nSangAmt = nSangAmt - nSangAmt_Temp;
            }
            #endregion

            if (clsPmpaType.GAM.Jin_Rate > 0) { clsPmpaType.GAM.Jin_Halin = (long)Math.Truncate((nJin1Amt + nJin2Amt) * clsPmpaType.GAM.Jin_Rate / 100.0); }
            if (clsPmpaType.GAM.ER_Rate > 0) { clsPmpaType.GAM.ER_Halin = (long)Math.Truncate((nEr1Amt + nEr2Amt) * clsPmpaType.GAM.ER_Rate / 100.0); }
            if (clsPmpaType.GAM.Gam_Rate > 0)
            {
                clsPmpaType.GAM.Gam_Halin = (long)Math.Truncate((nGub + nBigub) * clsPmpaType.GAM.Gam_Rate / 100.0);
            }

            if (clsPmpaType.GAM.SONO_Rate > 0) { clsPmpaType.GAM.SONO_Halin = (long)Math.Truncate(nSonoAmt * clsPmpaType.GAM.SONO_Rate / 100.0); }
            if (clsPmpaType.GAM.MRI_Rate > 0) { clsPmpaType.GAM.MRI_Halin = (long)Math.Truncate(nMriAmt * clsPmpaType.GAM.MRI_Rate / 100.0); }
            if (clsPmpaType.GAM.FOOD_Rate > 0) { clsPmpaType.GAM.FOOD_Halin = (long)Math.Truncate(nFoodAmt * clsPmpaType.GAM.FOOD_Rate / 100.0); }
            if (clsPmpaType.GAM.ROOM_Rate > 0) { clsPmpaType.GAM.ROOM_Halin = (long)Math.Truncate(nRoomAmt * clsPmpaType.GAM.ROOM_Rate / 100.0); }

            //급여 CT,MRI 감액 계산
            if (clsPmpaType.GAM.Gam_Rate > 0 && (nCtMriAmt != 0 || nCtMriOldAmt != 0))
            {
                clsPmpaType.GAM.Gam_Halin += (long)Math.Truncate(nCtMriAmt * clsPmpaType.GAM.Gam_Rate / 100.0);
                clsPmpaType.GAM.Gam_Halin += (long)Math.Truncate(nCtMriOldAmt * clsPmpaType.GAM.Gam_Rate / 100.0);
            }

            //치과 감액
            if (clsPmpaType.GAM.DTJin_Rate > 0) { clsPmpaType.GAM.DTJin_Halin = (long)Math.Truncate((nDT_Jin1Amt + nDT_Jin2Amt) * clsPmpaType.GAM.DTGam_Rate / 100.0); }
            if (clsPmpaType.GAM.DTGam_Rate > 0) { clsPmpaType.GAM.DTGam_Halin = (long)Math.Truncate((nDT_Gub + nDT_Bigub) * clsPmpaType.GAM.DTGam_Rate / 100.0); }
            if (clsPmpaType.GAM.DT1_Rate > 0) { clsPmpaType.GAM.DT1_Halin = (long)Math.Truncate(nDT1_Amt * clsPmpaType.GAM.DT1_Rate / 100.0); }
            if (clsPmpaType.GAM.DT2_Rate > 0) { clsPmpaType.GAM.DT2_Halin = (long)Math.Truncate(nDT2_Amt * clsPmpaType.GAM.DT2_Rate / 100.0); }
            if (clsPmpaType.GAM.DT3_Rate > 0) { clsPmpaType.GAM.DT3_Halin = (long)Math.Truncate(nDT3_Amt * clsPmpaType.GAM.DT3_Rate / 100.0); }

            //치과 감액 합계액을 계산
            clsPmpaType.GAM.DTHalin_Tot = clsPmpaType.GAM.DTJin_Halin + clsPmpaType.GAM.DTGam_Halin + clsPmpaType.GAM.DT1_Halin;
            clsPmpaType.GAM.DTHalin_Tot = clsPmpaType.GAM.DTHalin_Tot + clsPmpaType.GAM.DT2_Halin + clsPmpaType.GAM.DT3_Halin;

            //전체감액 합계금액을 계산
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Jin_Halin + clsPmpaType.GAM.Gam_Halin + clsPmpaType.GAM.SONO_Halin + clsPmpaType.GAM.MRI_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.FOOD_Halin + clsPmpaType.GAM.ROOM_Halin + clsPmpaType.GAM.ER_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.DTJin_Halin + clsPmpaType.GAM.DTGam_Halin;
            clsPmpaType.GAM.Halin_Tot = clsPmpaType.GAM.Halin_Tot + clsPmpaType.GAM.DT1_Halin + clsPmpaType.GAM.DT2_Halin + clsPmpaType.GAM.DT3_Halin;

            //재단성직자 할인제외가 없으면 본인부담액을 감액으로 처리
            if (clsPmpaType.GAM.GbGameK == "11")
            {
                SQL = " SELECT AMT54 From " + ComNum.DB_PMPA + "IPD_TRANS Where Trsno = " + ArgTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    nHal = Convert.ToInt64(Dt.Rows[0]["AMT54"].ToString().Trim());
                }

                Dt.Dispose();
                Dt = null;

                //치과 본인부담금 총액
                nHal2 = (long)Math.Truncate(nDT_Jin1Amt * ArgBonRate / 100.0) + nDT_Jin2Amt;         //치과 진찰료
                nHal2 = nHal2 + ((long)Math.Truncate(nDT_Gub * ArgBonRate / 100.0) + nDT_Bigub);     //치과 진료비
                nHal2 = nHal2 + nDT1_Amt + nDT2_Amt + nDT3_Amt + nNotSHalin;    //비급여 + 보철 + 임플란트 + 할인안됨

                //치과제외 할인 금액(사실상 치과제외한 총금액 이므로 100.0% 감액)
                nHal3 = ArgBoninAmt - nHal2;

                //최종 치과할인 금액과 치과제외한 금액을 더함 (원래 감액되어야 할 금액)
                nHal4 = nHal3 + clsPmpaType.GAM.DTHalin_Tot;
                clsPmpaType.GAM.Halin_Tot = nHal4;

                ComFunc.MsgBox("재단성직자입니다. 치과진료비 발생시 감액여부를 반드시 확인해주세요.", "확인");
            }

            //할인액이 본인부담액을 초과하면 본인부담액을 할인액으로 처리
            if (clsPmpaType.GAM.Halin_Tot > ArgBoninAmt)
                clsPmpaType.GAM.Halin_Tot = ArgBoninAmt;

            #endregion
        }

        public void Move_RS_TO_ISG()
        {
            clsPmpaType.ISG.Qty = 0;
            clsPmpaType.ISG.Nal = 0;
            clsPmpaType.ISG.Imiv = "";
            clsPmpaType.ISG.Dev = "";
            clsPmpaType.ISG.GbNgt = "";
            clsPmpaType.ISG.GBNGT2 = "";
            clsPmpaType.ISG.GbSpc = "0";
            clsPmpaType.ISG.GbSelf = "0";
            clsPmpaType.ISG.Sucode = clsPmpaType.RS.SuCode;
            clsPmpaType.ISG.Sunext = clsPmpaType.RS.SuNext;
            clsPmpaType.ISG.Bun = clsPmpaType.RS.Bun;
            clsPmpaType.ISG.Nu = clsPmpaType.RS.Nu;
            clsPmpaType.ISG.SugbA = clsPmpaType.RS.SugbA;
            clsPmpaType.ISG.SugbB = clsPmpaType.RS.SugbB;
            clsPmpaType.ISG.SugbC = clsPmpaType.RS.SugbC;
            clsPmpaType.ISG.SugbD = clsPmpaType.RS.SugbD;
            clsPmpaType.ISG.SugbE = clsPmpaType.RS.SugbE;
            clsPmpaType.ISG.SugbF = clsPmpaType.RS.SugbF;
            clsPmpaType.ISG.SugbG = clsPmpaType.RS.SugbG;
            clsPmpaType.ISG.SugbH = clsPmpaType.RS.SugbH;
            clsPmpaType.ISG.SugbI = clsPmpaType.RS.SugbI;
            clsPmpaType.ISG.SugbJ = clsPmpaType.RS.SugbJ;
            clsPmpaType.ISG.SugbP = clsPmpaType.RS.SugbP;
            clsPmpaType.ISG.SugbQ = clsPmpaType.RS.SugbQ;
            clsPmpaType.ISG.SugbR = clsPmpaType.RS.SugbR;
            clsPmpaType.ISG.SugbS = clsPmpaType.RS.SugbS;
            if (clsPmpaType.ISG.SugbS == "6" || clsPmpaType.ISG.SugbS == "7")
            {
                if (string.Compare(clsPublic.GstrSysDate, "2016-09-01") < 0)
                {
                    clsPmpaType.ISG.SugbS = "1";
                }
            }
            clsPmpaType.ISG.SugbX = clsPmpaType.RS.SugbX;
            clsPmpaType.ISG.SugbAA = clsPmpaType.RS.SugbAA;
            clsPmpaType.ISG.SugbAB = clsPmpaType.RS.SugbAB;
            clsPmpaType.ISG.SugbAC = clsPmpaType.RS.SugbAC;
            clsPmpaType.ISG.SugbAD = clsPmpaType.RS.SugbAD;
            clsPmpaType.ISG.GBNS = clsPmpaType.RS.GBNS;
            clsPmpaType.ISG.Sudate = clsPmpaType.RS.SuDate;
            clsPmpaType.ISG.Sudate3 = clsPmpaType.RS.SuDate3;
            clsPmpaType.ISG.Sudate4 = clsPmpaType.RS.SuDate4;
            clsPmpaType.ISG.Sudate5 = clsPmpaType.RS.SuDate5;
            clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt;
            clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt;
            clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt;
            clsPmpaType.ISG.Selamt = 0;
            clsPmpaType.ISG.OldIamt = clsPmpaType.RS.OldIAmt;
            clsPmpaType.ISG.OldTamt = clsPmpaType.RS.OldTAmt;
            clsPmpaType.ISG.OldBamt = clsPmpaType.RS.OldBAmt;
            if (string.Compare(clsPmpaType.ISG.Sudate, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate3, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.OldIAmt;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.OldTAmt;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.OldBAmt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate4, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt3;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt3;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt3;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate5, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt4;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt4;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt4;
            }
            else
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt5;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt5;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt5;
            }
            clsPmpaType.ISG.DrgSelf = "";
            clsPmpaType.ISG.OrderNo = 0;
            clsPmpaType.ISG.Div = 0;
            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Bamt;

            if (clsPmpaType.IA.Bi > 50) { clsPmpaType.ISG.BaseAmt = clsPmpaType.RS.IAmt; }
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55) { clsPmpaType.ISG.BaseAmt = clsPmpaType.RS.TAmt; }
            clsPmpaType.ISG.PART2 = "";
            clsPmpaType.ISG.GBKTAS = "";
            clsPmpaType.ISG.GbChildZ = "";
        }


        public void Move_RS_TO_ISG_ICUPDT()
        {
           
            clsPmpaType.ISG.Sunext = clsPmpaType.RS.SuNext;
            clsPmpaType.ISG.Bun = clsPmpaType.RS.Bun;
            clsPmpaType.ISG.Nu = clsPmpaType.RS.Nu;
            clsPmpaType.ISG.SugbA = clsPmpaType.RS.SugbA;
            clsPmpaType.ISG.SugbB = clsPmpaType.RS.SugbB;
            clsPmpaType.ISG.SugbC = clsPmpaType.RS.SugbC;
            clsPmpaType.ISG.SugbD = clsPmpaType.RS.SugbD;
            clsPmpaType.ISG.SugbE = clsPmpaType.RS.SugbE;
            clsPmpaType.ISG.SugbF = clsPmpaType.RS.SugbF;
            clsPmpaType.ISG.SugbG = clsPmpaType.RS.SugbG;
            clsPmpaType.ISG.SugbH = clsPmpaType.RS.SugbH;
            clsPmpaType.ISG.SugbI = clsPmpaType.RS.SugbI;
            clsPmpaType.ISG.SugbJ = clsPmpaType.RS.SugbJ;
            clsPmpaType.ISG.SugbP = clsPmpaType.RS.SugbP;
            clsPmpaType.ISG.SugbQ = clsPmpaType.RS.SugbQ;
            clsPmpaType.ISG.SugbR = clsPmpaType.RS.SugbR;
            clsPmpaType.ISG.SugbS = clsPmpaType.RS.SugbS;
            if (clsPmpaType.ISG.SugbS == "6" || clsPmpaType.ISG.SugbS == "7")
            {
                if (string.Compare(clsPublic.GstrSysDate, "2016-09-01") < 0)
                {
                    clsPmpaType.ISG.SugbS = "1";
                }
            }
            clsPmpaType.ISG.SugbX = clsPmpaType.RS.SugbX;
            clsPmpaType.ISG.SugbAA = clsPmpaType.RS.SugbAA;
            clsPmpaType.ISG.SugbAB = clsPmpaType.RS.SugbAB;
            clsPmpaType.ISG.SugbAC = clsPmpaType.RS.SugbAC;
            clsPmpaType.ISG.SugbAD = clsPmpaType.RS.SugbAD;
            clsPmpaType.ISG.GBNS = clsPmpaType.RS.GBNS;
            clsPmpaType.ISG.Sudate = clsPmpaType.RS.SuDate;
            clsPmpaType.ISG.Sudate3 = clsPmpaType.RS.SuDate3;
            clsPmpaType.ISG.Sudate4 = clsPmpaType.RS.SuDate4;
            clsPmpaType.ISG.Sudate5 = clsPmpaType.RS.SuDate5;
            clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt;
            clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt;
            clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt;
            clsPmpaType.ISG.Selamt = 0;
            clsPmpaType.ISG.OldIamt = clsPmpaType.RS.OldIAmt;
            clsPmpaType.ISG.OldTamt = clsPmpaType.RS.OldTAmt;
            clsPmpaType.ISG.OldBamt = clsPmpaType.RS.OldBAmt;
            if (string.Compare(clsPmpaType.ISG.Sudate, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate3, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.OldIAmt;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.OldTAmt;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.OldBAmt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate4, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt3;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt3;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt3;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate5, clsPmpaType.IA.Date) <= 0)
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt4;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt4;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt4;
            }
            else
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.RS.IAmt5;
                clsPmpaType.ISG.Tamt = clsPmpaType.RS.TAmt5;
                clsPmpaType.ISG.Bamt = clsPmpaType.RS.BAmt5;
            }


            if (clsPmpaType.IA.Bi > 50) { clsPmpaType.ISG.BaseAmt = clsPmpaType.RS.IAmt; }
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55) { clsPmpaType.ISG.BaseAmt = clsPmpaType.RS.TAmt; }
        }

        /// <summary>
        /// Description : 입원 진료비 본인부담율 저장
        /// Author : 김민철
        /// Create Date : 2017.11.13
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strBi"></param>
        /// <param name="strIO"></param>
        /// <param name="strChild"></param>
        /// <param name="strMCode"></param>
        /// <param name="strVCode"></param>
        /// <param name="strSDate"></param>
        /// <history>strDept 과별 부담율 추가함 2018.04.11</history>
        public bool Read_IBon_Rate(PsmhDb pDbCon, clsPmpaType.BonRate BON)
        {
            DataTable Dt        = null;
            clsBasAcct cBAcct = new clsBasAcct();

            string SQL          = "";
            string SqlErr       = "";
            string strDeptCode  = string.Empty;
            
            bool rtnVal         = false;
            
            //본인부담율 초기화
            IBon_Rate_Variable_Clear();

            #region 이전 세팅부분 주석처리 2018-05-03 KMC
            //if (BON.DEPT != "NP" && BON.DEPT != "DT")
            //{
            //    strDeptCode = "**";
            //}
            //else
            //{
            //    strDeptCode = BON.DEPT;
            //}

            ////65세이상 본인부담율은 DT의 임플란트 틀니 부담율 때문에 구분하였으므로 DT인 경우 65세이상은 0.성인으로 분류함
            //if (BON.DEPT != "DT")
            //{
            //    if (BON.CHILD == "4")
            //        BON.CHILD = "0";
            //}

            ////건강보험은 11종으로 통합설정됨
            //if (VB.Left(BON.BI, 1) == "1")
            //    BON.BI = "11";
            ////건강보험100%은 41종, 0.성인으로 통합설정됨
            //else if (VB.Left(BON.BI, 1) == "4")
            //    BON.BI = "41";
            #endregion

            //부담율 Argument 치환(외래/입원 공통)
            cBAcct.Convert_Rate_Argument(BON);

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " SELECT BI,MCODE,GBCHILD,VCODE_NAME,VCODE,JIN,BOHUM,CTMRI,FOOD,GBIO,ENTSABUN,";
                SQL += ComNum.VBLF + "        TO_CHAR(SDATE, 'YYYY-MM-DD') SDate, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') EntDate,";
                SQL += ComNum.VBLF + "        DT1,DT2,TO_CHAR(DELDATE, 'YYYY-MM-DD') DelDate,WRTNO,FAMT1,FAMT2,FCODE,DEPT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_BON ";
                SQL += ComNum.VBLF + "  WHERE BI = '" + BON.BI + "' ";
                SQL += ComNum.VBLF + "    AND GBIO = '" + BON.IO + "' ";
                SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE > TRUNC(SYSDATE)) ";
                SQL += ComNum.VBLF + "    AND SDATE <= TO_DATE('" + BON.SDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GBCHILD = '" + BON.CHILD + "' ";
                SQL += ComNum.VBLF + "    AND DEPT = '" + BON.DEPT + "' "; 
                if (BON.MCODE.Trim() != "")
                {
                    SQL += ComNum.VBLF + "    AND MCODE = '" + BON.MCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (MCODE IS NULL OR MCODE = '' OR MCODE = ' ') ";
                }

                if (BON.VCODE.Trim() != "")
                {
                    SQL += ComNum.VBLF + "    AND VCODE = '" + BON.VCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (VCODE IS NULL OR VCODE = '' OR VCODE = ' ') ";
                }

                if (BON.OGPDBUN.Trim() != "")
                {
                    SQL += ComNum.VBLF + "    AND OGPDBUN = '" + BON.OGPDBUN + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (OGPDBUN IS NULL OR OGPDBUN = '' OR OGPDBUN = ' ') ";
                }

                if (BON.FCODE.Trim() != "")
                {
                    SQL += ComNum.VBLF + "    AND FCODE = '" + BON.FCODE + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND (FCODE IS NULL OR FCODE = '' OR FCODE = ' ') ";
                }

                SQL += ComNum.VBLF + "  ORDER By SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.", "전산실 연락요망");
                    return rtnVal;
                }
                else
                {
                    clsPmpaType.IBR.Jin = Convert.ToInt32(Dt.Rows[0]["JIN"].ToString());        //진찰료
                    clsPmpaType.IBR.Bohum = Convert.ToInt32(Dt.Rows[0]["BOHUM"].ToString());    //진료비
                    clsPmpaType.IBR.CTMRI = Convert.ToInt32(Dt.Rows[0]["CTMRI"].ToString());    //CT/MRI
                    clsPmpaType.IBR.Food = Convert.ToInt32(Dt.Rows[0]["FOOD"].ToString());      //식대료
                    clsPmpaType.IBR.Dt1 = Convert.ToInt32(Dt.Rows[0]["DT1"].ToString());        //틀니
                    clsPmpaType.IBR.Dt2 = Convert.ToInt32(Dt.Rows[0]["DT2"].ToString());        //임플란트
                    clsPmpaType.IBR.FAmt1 = Convert.ToInt64(Dt.Rows[0]["FAMT1"].ToString());    //진료비 정액
                    clsPmpaType.IBR.FAmt2 = Convert.ToInt64(Dt.Rows[0]["FAMT2"].ToString());    //진료비 정액(직접조제시)
                    clsPmpaType.IBR.SDate = Dt.Rows[0]["SDATE"].ToString().Trim();              //적용일자
                }

                Dt.Dispose();
                Dt = null;

                rtnVal = true;
                return rtnVal;

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

                return rtnVal;
            }
        }

        public void IBon_Rate_Variable_Clear()
        {
            //기본적으로 본인 100% 부담율로 세팅
            clsPmpaType.IBR.Jin = 100;
            clsPmpaType.IBR.Bohum = 100;
            clsPmpaType.IBR.CTMRI = 100;
            clsPmpaType.IBR.Food = 100;
            clsPmpaType.IBR.Dt1 = 100;
            clsPmpaType.IBR.Dt2 = 100;
            clsPmpaType.IBR.FAmt1 = 0;
            clsPmpaType.IBR.FAmt2 = 0;
            clsPmpaType.IBR.SDate = "";
        }

        #region JSIM Account_Process
        /// <summary>
        /// 재원심사용 수가 낱개별 계산 로직
        /// </summary>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgSuNext"></param>
        /// <param name="ArgNgt"></param>
        /// <param name="ArgF"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgQty"></param>
        /// <param name="ArgNal"></param>
        /// <param name="ArgDiv"></param>
        /// <param name="ArgGbSelNot"></param>
        /// <param name="ArgKTAS"></param>
        /// <param name="ArgChildZ"></param>
        public bool Account_Process_JSIM(PsmhDb pDbCon, string ArgSuCode, string ArgSuNext, string ArgNgt, string ArgF, string ArgBDate, double ArgQty, int ArgNal, int ArgDiv, string ArgGbSelNot, string ArgKTAS, string ArgChildZ, string ArgPowder)
        {
            bool rtnVal = true;

            long nAMT1 = 0;                         //계산금액
            long nTAHPSUGA = 0;                      //타병원단가


            int nInx = 0, j = 0;

            Argument_Move_JSIM(pDbCon, ArgBDate);
            Area_Clear_JSIM();

            if (ArgSuCode.Trim() != "")
            {
                clsPmpaPb.G7AMT = 0;
                clsPmpaPb.G7TAMT = 0;
                clsPmpaPb.GstatPED = "0";
                clsPmpaPb.GnWrtSeqNo = 0;
                clsPmpaType.IA.KTASLEVEL = ArgKTAS;

                if (Move_SA_TO_SG_ONE(pDbCon, ArgSuCode, ArgBDate, ArgNgt, ArgF, ArgQty, ArgNal, ArgDiv, ArgGbSelNot, ArgKTAS, ArgChildZ, ArgPowder) == false)
                {
                    ComFunc.MsgBox("수가항목을 읽어 오는데 실패하였습니다.", "수가확인");
                    return false;
                }

                if (Main_Account_Process_ONE(pDbCon, ArgSuNext) == false)
                {
                    ComFunc.MsgBox("수가항목 계산실패.", "오류발생");
                    return false;
                }

                #region 타병원 수가 관련(2021-06-02)(재원심사에서만 사용)
                nAMT1 = 0;                         //계산금액
                nTAHPSUGA = 0;                      //타병원단가

                clsIuSentChk cISentChk = new clsIuSentChk();

                nTAHPSUGA = cISentChk.Rtn_Bas_Sun_GBTAHPSUGA(clsDB.DbCon, ArgSuCode, ArgBDate);  //GBTAHPSUGA 타병원수가 확인

                if (nTAHPSUGA > 0)
                {
                    nAMT1 = (long)Math.Truncate(nTAHPSUGA * ArgQty * ArgNal);
                    clsPmpaPb.G7AMT = nAMT1;
                    clsPmpaType.ISG.BaseAmt = nTAHPSUGA;
                }
                cISentChk = null;
                
                #endregion

            }

            return rtnVal;
        }

        //계산 Routine에서 사용할 Item Move
        void Argument_Move_JSIM(PsmhDb pDbCon, string ArgBDate)
        {
            ComFunc CF = new ComFunc();

            clsPmpaType.IA.Date = ArgBDate;
            clsPmpaType.IA.Dept = clsPmpaType.TIT.DeptCode;
            clsPmpaType.IA.Sex = clsPmpaType.TIT.Sex;
            clsPmpaType.IA.GbGameK = clsPmpaType.TIT.GbGameK;
            clsPmpaType.IA.Retn = 0;
            clsPmpaType.IA.Bi = Convert.ToInt16(clsPmpaType.TIT.Bi);
            clsPmpaType.IA.Bi1 = Convert.ToInt16(clsPmpaType.TIT.Bi.Substring(0, 1));

            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55)
            {
                clsPmpaType.IA.Bi1 = 6;  //자보
            }

            clsPmpaType.IA.Age = clsPmpaType.TIT.Age;
            clsPmpaType.IA.Fee6 = clsPmpaType.TIT.Fee6;
            //신생아 마취료 계산 때문 일수 구함. 30일이전은 60%가산 이후 30% 가산 2006-05-17
            clsPmpaType.IA.AgeiLsu = 0;
            //신생아 가산관련 IF 해제 2014-10-25
            clsPmpaType.IA.AgeiLsu = CF.DATE_ILSU(pDbCon, ArgBDate, "20" + VB.Left(clsPmpaType.TIT.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TIT.Jumin1, 3, 2) + "-" + VB.Right(clsPmpaType.TIT.Jumin1, 2));

            if (clsPmpaType.IA.Fee6 < 0)
            {
                clsPmpaType.IA.Fee6 = 0;
            }

            clsPmpaType.IA.Gbilban2 = clsPmpaType.TIT.Gbilban2;  //외국new
            clsPmpaType.IA.GbSpc = clsPmpaType.TIT.GbSpc;
            clsPmpaType.IA.pano = clsPmpaType.TIT.Pano;
            clsPmpaType.IA.DrCode = clsPmpaType.TIT.DrCode;
            clsPmpaType.IA.IPDNO = clsPmpaType.TIT.Ipdno;
        
        }

        void Area_Clear_JSIM()
        {
            clsPmpaPb.G7SMAcount = 0;
            clsPmpaPb.G7WRTcount = 0;

            clsPmpaPb.GstatQmgrIPD = "";
            clsPmpaPb.GstatQmgrBAS = "";
            clsPmpaPb.GstrB20STAT = "";
            clsPmpaPb.GstrB11STAT = "";
            clsPmpaPb.GstrB65STAT = "";

            clsPmpaType.ISA = new clsPmpaType.Slip_Accept_Table_IPD[1];
            clsPmpaType.ISW = new clsPmpaType.Slip_Write_Table_IPD[1];

            clsPmpaType.IA.Amt  = new long[61];
            clsPmpaPb.G7QTY11   = new double[61];
            clsPmpaPb.G7QTY20A  = new double[61];
            clsPmpaPb.G7QTY20B  = new double[61];
            clsPmpaPb.G7AL201   = new int[61];
            clsPmpaPb.G7AL010 = new double[61];

            Array.Clear(clsPmpaType.ISA, 0, clsPmpaType.ISA.Length);
            Array.Clear(clsPmpaType.ISW, 0, clsPmpaType.ISW.Length);
        }

        bool Move_SA_TO_SG_ONE(PsmhDb pDbCon, string ArgSuCode, string ArgBDate, string ArgNgt, string ArgF, double ArgQty, int ArgNal, int ArgDiv, string ArgGbSelNot, string ArgKTAS, string ArgChildZ, string ArgPowder)
        {
            bool rtnVal = true;
            DataTable dt = null; 

            string strDelDate = string.Empty;
            string strSugbF = string.Empty;
            string strSugbG = string.Empty;
            string strBun = string.Empty;
            long nIAMT = 0;
            long nTAMT = 0;
            long nBAMT = 0;
            long nPrice = 0;

            clsPmpaFunc cPF = new clsPmpaFunc();
            clsPmpaQuery cPQ = new clsPmpaQuery();
            clsOumsadChk cOCHK = new clsOumsadChk();

            try
            {
                dt = cPF.get_Bas_Sut_Sun(pDbCon, ArgSuCode);
                if (dt == null)
                {
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strDelDate = dt.Rows[0]["DelDay"].ToString().Trim(); 
                    strSugbF = dt.Rows[0]["SugbF"].ToString().Trim(); 
                    strSugbG = dt.Rows[0]["SugbG"].ToString().Trim();
                    strBun = dt.Rows[0]["Bun"].ToString().Trim();

                    if (string.Compare(dt.Rows[0]["SuDay"].ToString().Trim(), ArgBDate) <= 0)
                    {
                        nIAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Iamt"].ToString())); 
                        nTAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Tamt"].ToString())); 
                        nBAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Bamt"].ToString()));     
                    }
                    else if (string.Compare(dt.Rows[0]["Sudate3"].ToString().Trim(), ArgBDate) <= 0)
                    {
                        nIAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["OldIamt"].ToString()));
                        nTAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["OldTamt"].ToString()));
                        nBAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["OldBamt"].ToString()));
                    }
                    else if (string.Compare(dt.Rows[0]["Sudate4"].ToString().Trim(), ArgBDate) <= 0)
                    {
                        nIAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Iamt3"].ToString()));
                        nTAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Tamt3"].ToString()));
                        nBAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Bamt3"].ToString()));
                    }
                    else if (string.Compare(dt.Rows[0]["Sudate5"].ToString().Trim(), ArgBDate) <= 0)
                    {
                        nIAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Iamt4"].ToString()));
                        nTAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Tamt4"].ToString()));
                        nBAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Bamt4"].ToString()));
                    }
                    else 
                    {
                        nIAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Iamt5"].ToString()));
                        nTAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Tamt5"].ToString()));
                        nBAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["Bamt5"].ToString()));
                    }

                    nPrice = nBAMT;

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") > 0) { nPrice = nIAMT; }
                    if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55") { nPrice = nTAMT; }

                   
                    clsPmpaType.ISG.Iamt = nIAMT;
                    clsPmpaType.ISG.Tamt = nTAMT;
                    clsPmpaType.ISG.Bamt = nBAMT;

                    clsPmpaType.ISG.Qty     = ArgQty;
                    clsPmpaType.ISG.Nal     = ArgNal;
                    clsPmpaType.ISG.Imiv    = "0";
                    clsPmpaType.ISG.Dev     = "";
                    clsPmpaType.ISG.GbNgt   = "0";
                    clsPmpaType.ISG.GbSpc   = "0";
                    clsPmpaType.ISG.GbSelf  = "0";
                    clsPmpaType.ISG.GbNgt   = ArgNgt;
                    clsPmpaType.ISG.GbSelf  = ArgF;
                    clsPmpaType.ISG.Sucode  = dt.Rows[0]["Sucode"].ToString().Trim(); 
                    clsPmpaType.ISG.Sunext  = dt.Rows[0]["Sunext"].ToString().Trim(); 
                    clsPmpaType.ISG.Bun     = dt.Rows[0]["Bun"].ToString().Trim(); 
                    clsPmpaType.ISG.Nu      = dt.Rows[0]["Nu"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbA   = dt.Rows[0]["SugbA"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbB   = dt.Rows[0]["SugbB"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbC   = dt.Rows[0]["SugbC"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbD   = dt.Rows[0]["SugbD"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbE   = dt.Rows[0]["SugbE"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbF   = dt.Rows[0]["SugbF"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbG   = dt.Rows[0]["SugbG"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbH   = dt.Rows[0]["SugbH"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbI   = dt.Rows[0]["SugbI"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbJ   = dt.Rows[0]["SugbJ"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbQ   = dt.Rows[0]["SugbQ"].ToString().Trim(); 
                    clsPmpaType.ISG.SugbR   = dt.Rows[0]["SugbR"].ToString().Trim();     
                    clsPmpaType.ISG.SugbS   = dt.Rows[0]["SugbS"].ToString().Trim();    
                    if (clsPmpaType.ISG.SugbS == "6" || clsPmpaType.ISG.SugbS == "7")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2016-09-01") < 0)
                        {
                            clsPmpaType.ISG.SugbS = "1";
                        }
                    }
                    clsPmpaType.ISG.SugbAA = dt.Rows[0]["SugbAA"].ToString().Trim();

                    clsPmpaType.ISG.Sudate = dt.Rows[0]["Suday"].ToString().Trim(); 
                    clsPmpaType.ISG.OldIamt = Convert.ToInt64(VB.Val(dt.Rows[0]["OldIamt"].ToString()));
                    clsPmpaType.ISG.OldTamt = Convert.ToInt64(VB.Val(dt.Rows[0]["OldTamt"].ToString()));
                    clsPmpaType.ISG.OldBamt = Convert.ToInt64(VB.Val(dt.Rows[0]["OldBamt"].ToString()));
                    //clsPmpaType.ISG.DayMax  = Convert.ToInt64(VB.Val(dt.Rows[0]["DayMax"].ToString()));
                    //clsPmpaType.ISG.TotMax  = Convert.ToInt64(VB.Val(dt.Rows[0]["TotMax"].ToString()));    
                    //clsPmpaType.ISG.Sunamek = dt.Rows[0]["SunameK"].ToString().Trim(); 
                    //clsPmpaType.ISG.Hcode = dt.Rows[0]["Hcode"].ToString().Trim();

                    if (clsPmpaPb.GstrSugaNewUseFlag == "OK" && cOCHK.Check_LowDrug_Suga(pDbCon, ArgSuCode) == "OK")
                    {
                        cPQ.Read_Suga_Amt(pDbCon, ArgSuCode, clsPmpaType.ISG.Sunext, ArgBDate);

                        if (clsPmpaPb.GstrSugaNewReadOK == "OK")
                        {
                            clsPmpaType.ISG.Iamt = clsPmpaPb.GnIAmt;
                            clsPmpaType.ISG.Tamt = clsPmpaPb.GnTAmt;
                            clsPmpaType.ISG.Bamt = clsPmpaPb.GnBAmt;
                        }
                    }

                    clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Bamt;

                    if (string.Compare(clsPmpaType.TIT.Bi, "50") > 0) { clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt; }
                    if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55") { clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Tamt; }
                    if (clsPmpaType.TIT.Bi == "51" && clsPmpaType.TIT.Gbilban2 == "Y")
                    {
                        clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt * 2;
                    }
                    if (clsPmpaType.ISG.Bun == "74")
                    {
                        string[] strA = new string[3];
                        strA = Chk_Diet_Code_Amt(pDbCon, ArgSuCode);

                        if (strA[0] != "" && strA[1] != "7")
                        {
                            clsPmpaType.ISG.Iamt = 0;
                            clsPmpaType.ISG.Tamt = 0;
                            clsPmpaType.ISG.Bamt = 0;
                            clsPmpaType.ISG.BaseAmt = 0;
                        }
                        
                        if (strA[0] == "02" && clsPmpaType.ISG.Sucode == "FL01")
                        { 
                            if (clsPmpaType.TIT.Bi == "21" || clsPmpaType.TIT.Bi == "22")    //보호환자는 저염식 치료식은 단가가 0임 심사계 요청
                            {
                                //저염식만 단가 0, 저염식+치료식이 있으면 수가단가로한다. 김순옥샘 요청
                                clsPmpaType.ISG.Iamt = 0;
                                clsPmpaType.ISG.Tamt = 0;
                                clsPmpaType.ISG.Bamt = 0;
                                clsPmpaType.ISG.BaseAmt = 0;
                            }
                        }
                    }

                    if (clsPmpaType.TIT.Bi == "31" && string.Compare(clsPmpaType.ISG.SugbQ, "0") > 0)
                    {
                        clsPmpaType.ISG.SugbF = "0";
                    }

                    if (clsPmpaType.ISG.SugbG == "6") { clsPmpaType.ISG.Qty = 1; clsPmpaType.ISG.Nal = 1; }
                    //마취수가의 경우 수량 날수 별도 계산
                    //if (clsPmpaType.ISG.SugbG == "4") { clsPmpaType.ISG.Qty = 0; clsPmpaType.ISG.Nal = 30; }
                    if (clsPmpaType.ISG.Bun == "20" && clsPmpaType.ISG.SugbB != "3") { clsPmpaType.ISG.Imiv = clsPmpaType.ISG.SugbB; }
                    if (clsPmpaType.ISG.SugbF == "2" && clsPmpaType.ISG.GbSelf != "1") { clsPmpaType.ISG.GbSelf = clsPmpaType.ISG.SugbF; }

                    clsPmpaType.ISG.Div = ArgDiv;
                    clsPmpaType.ISG.GbSelNot = ArgGbSelNot;
                    clsPmpaType.ISG.GBKTAS = ArgKTAS;
                    clsPmpaType.ISG.GbChildZ = ArgChildZ;
                    clsPmpaType.ISG.POWDER = ArgPowder;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
        
        bool Main_Account_Process_ONE(PsmhDb pDbCon, string ArgSuNext)
        {
            bool rtnVal = true;

            if (clsPmpaType.IA.Retn == 1) { clsPmpaType.ISG.Nal = clsPmpaType.ISG.Nal * -1; }
            if (clsPmpaType.ISG.Bun == "20" && clsPmpaType.ISG.SugbB == "3") { clsPmpaType.ISG.SugbB = clsPmpaType.ISG.Imiv; }
            if (clsPmpaType.ISG.SugbF == "2") { clsPmpaType.ISG.SugbF = clsPmpaType.ISG.GbSelf; }
            if (clsPmpaType.IA.Bi == 31 && string.Compare(clsPmpaType.ISG.SugbQ, "0") > 0) { clsPmpaType.ISG.SugbF = "0"; }
                
            //선별급여관련 GBSelf항 작업
            if (string.Compare(clsPmpaType.IA.Date, "2016-09-01") >= 0 && (clsPmpaType.ISG.SugbS == "3" || clsPmpaType.ISG.SugbS == "6" || clsPmpaType.ISG.SugbS == "7" || clsPmpaType.ISG.SugbS == "8"))
            {
                if (clsPmpaType.ISG.GbSelf == "0") { clsPmpaType.ISG.SugbS = "0"; }
                clsPmpaType.ISG.GbSelf = "0";
            }


            if (string.Compare(clsPmpaType.ISG.SugbA, "1") > 0 && clsPmpaType.IA.Bi < 50)
            { 
                //L7010EI ,L1201EI 단수가산 오류로 수정함(LYJ)
                if ((clsPmpaType.ISG.SugbF != "1" && clsPmpaType.ISG.GbSelf != "1") || clsPmpaType.ISG.Bun == "22" || clsPmpaType.ISG.Bun == "23")
                {
                    if (Account_Process_Host_ONE(pDbCon, "BO", ArgSuNext) == false)
                    {
                        return false;
                    }
                    return rtnVal;
                }
            }

            if (string.Compare(clsPmpaType.ISG.SugbA, "1") > 0 && (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55))    //TA 보험 환자
            { 
                if (clsPmpaType.ISG.SugbF != "1" && clsPmpaType.ISG.GbSelf != "1")
                {
                    if (Account_Process_Host_ONE(pDbCon, "TA", ArgSuNext) == false)
                    {
                        return false;
                    }
                    return rtnVal;
                }
            }

            if (string.Compare(clsPmpaType.ISG.SugbA, "2") > 0 && clsPmpaType.IA.Bi > 50)
            {
                if (Account_Process_Host_ONE(pDbCon, "IL", ArgSuNext) == false)
                {
                    return false;
                }
                return rtnVal;
            }

            if (string.Compare(clsPmpaType.ISG.SugbA, "2") > 0 && clsPmpaType.IA.Bi < 50 && clsPmpaType.ISG.GbSelf == "1")
            {
                if (Account_Process_Host_ONE(pDbCon, "IL", ArgSuNext) == false)
                {
                    return false;
                }
                return rtnVal;
            }

            if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.IA.Dept == "NP" && (string.Compare(clsPmpaType.ISG.Bun, "03") > 0 || clsPmpaType.ISG.Sucode.Trim() == "AC101"))   //보호정신과 단일수가
            { 
                if (clsPmpaType.ISG.SugbF == "0" && clsPmpaType.ISG.Sunext.Trim() != "C-NP")                                            //이외 급여분 금액 Zero
                {
                    clsPmpaType.ISG.BaseAmt = 0;
                }
            }
            
            if ((clsPmpaType.IA.Bi == 21 || clsPmpaType.IA.Bi == 22) && clsPmpaType.IA.Dept == "NP" && (clsPmpaType.ISG.Bun == "01" || clsPmpaType.ISG.Bun == "02"))     //보호정신과 단일수가-진찰료관련 
            {
                string strCode = clsPmpaType.ISG.Sucode.Trim();

                if (strCode == "AA176" || strCode == "AA276" || strCode == "AA1761" || strCode == "AA2761" || strCode == "AA1762" || strCode == "AA2762")
                {
                    clsPmpaType.ISG.BaseAmt = 0;
                }
            }

            if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30)
            { 
                if (clsPmpaType.IA.Dept == "NP" && clsPmpaType.ISG.BaseAmt == 0)
                {
                    clsPmpaPb.GhostDAEPYO = "0";
                    return rtnVal;
                }
            }

            clsPmpaPb.GhostDAEPYO = "0";

            if (string.Compare(clsPmpaType.ISG.Bun, "11") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "15") <= 0)
            {
                BUN_11_Account_ONE();
            }
            else if (string.Compare(clsPmpaType.ISG.Bun, "16") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "21") <= 0)
            {
                BUN_20_Account_ONE();
            }
            else if (string.Compare(clsPmpaType.ISG.Bun, "22") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "23") <= 0)
            {
                BUN_22_Account_ONE();
            }
            else if (clsPmpaType.ISG.Bun == "28" || clsPmpaType.ISG.Bun == "34" || clsPmpaType.ISG.Bun == "35")
            {
                BUN_34_Account_ONE();
            }
            else if (clsPmpaType.ISG.Bun == "37")
            {
                BUN_37_Account_ONE();
            }
            else if (clsPmpaType.ISG.Bun == "75")
            {
                BUN_75_Account_ONE();
            }
            else
            {
                BUN_99_Account_ONE();
            }
            
            return rtnVal;

        }

        /// <summary>
        /// Account_Process_JSIM > Account_Process_Host_ONE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgSuNext"></param>
        /// <returns></returns>
        bool Account_Process_Host_ONE(PsmhDb pDbCon, string ArgGubun, string ArgSuNext)
        {
            bool rtnVal = true;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strHHMMSS = string.Empty;
            string strMMSS = string.Empty;
            string strChkflag = string.Empty;
            string strBun = string.Empty;
            string strSugbSS = string.Empty;

            int i = 0;
            int nREAD = 0;

            #region // Move_SG_TO_HOST
            clsPmpaType.ISH.Qty = clsPmpaType.ISG.Qty;
            clsPmpaType.ISH.Nal = clsPmpaType.ISG.Nal;
            clsPmpaType.ISH.Dev = clsPmpaType.ISG.Dev;
            clsPmpaType.ISH.Imiv = clsPmpaType.ISG.Imiv;
            clsPmpaType.ISH.GbSelf = clsPmpaType.ISG.GbSelf;
            clsPmpaType.ISH.GbNgt = clsPmpaType.ISG.GbNgt;
            clsPmpaType.ISH.GbSpc = clsPmpaType.ISG.GbSpc;
            clsPmpaType.ISH.BaseAmt = clsPmpaType.ISG.BaseAmt;
            clsPmpaType.ISH.Sucode = clsPmpaType.ISG.Sucode;
            clsPmpaType.ISH.SugbB = clsPmpaType.ISG.SugbB;
            #endregion

            try
            {
                #region BAS_SUT, BAS_SUN Table Join Query
                SQL = "";
                SQL += " SELECT a.Bun,a.Nu,a.SugbA,a.SugbB,a.SugbC,a.SugbD,a.SugbE,a.SugbF,b.SugbS,     \r\n";
                SQL += "        a.SugbG,a.SugbH,a.SugbI,a.SugbJ,a.SugbSS,a.SugbBI,a.SuQty,a.Iamt,       \r\n";
                SQL += "        a.Tamt,a.Bamt,TO_CHAR(a.Sudate, 'yyyy-mm-dd') Sdate,                    \r\n";
                SQL += "        a.OldIamt,a.OldTamt,a.OldBamt,a.Sunext,                                 \r\n";
                SQL += "        TO_CHAR(a.Sudate3, 'yyyy-mm-dd') Sudate3,                               \r\n";
                SQL += "        a.Iamt3, a.Tamt3, a.Bamt3, TO_CHAR(a.Sudate4, 'yyyy-mm-dd') Sudate4,    \r\n";
                SQL += "        a.Iamt4, a.Tamt4, a.Bamt4, TO_CHAR(a.Sudate5, 'yyyy-mm-dd') Sudate5,    \r\n";
                SQL += "        a.Iamt5, a.Tamt5, a.Bamt5, b.SugbQ, b.SugbR,b.SugbAA ,b.SugbAB,b.SugbAg \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUH a,                                        \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUN b                                         \r\n";
                SQL += "  WHERE a.Sucode = '" + clsPmpaType.ISG.Sucode + "'                             \r\n";
                if (ArgSuNext.ToString().Trim() != "")
                {
                    SQL += "    AND a.SuNext = '" + ArgSuNext + "'                                          \r\n";  //묶음코드 한개만
           
                }
                SQL += "    AND a.SuNext =b.SuNext(+)                                                   \r\n";
                if (ArgGubun == "IL") //일반
                {
                    SQL += "   AND (a.SugbBI = '0' OR a.SugbBI = '2')                                   \r\n";
                }
                else if (ArgGubun == "TA") //교통
                {
                    SQL += "   AND (a.SugbBI = '0' OR a.SugbBI = '1' OR a.SugbBI = '3')                 \r\n";
                }
                else //보험,보호등
                {
                    SQL += "   AND (a.SugbBI = '0' OR a.SugbBI = '1')                                   \r\n";
                }
                SQL += " ORDER BY a.Sort,a.SuNext                                                       ";
                #endregion

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                

                nREAD = dt.Rows.Count;

                clsPmpaPb.GhostDAEPYO = "0";
                strHHMMSS = DateTime.Now.ToString("HH:MM:ss");
                strMMSS = VB.Mid(strHHMMSS, 4, 2) + VB.Mid(strHHMMSS, 7, 2);
                clsPmpaPb.GnWrtSeqNo = Convert.ToInt32(strMMSS);

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        strChkflag = "NO";
                        strBun = dt.Rows[i]["BUN"].ToString().Trim();
                        strSugbSS = dt.Rows[i]["SUGBSS"].ToString().Trim();

                        if (strBun == "65" || strBun == "66" || strBun == "72")
                        {
                            SugbSS_Check_65_ONE(ref strChkflag, strSugbSS);
                        }
                        else
                        {
                            SugbSS_Check_ONE(ref strChkflag, strSugbSS);
                        }

                        if (strChkflag == "OK")
                        {
                            clsPmpaPb.G7AMT = 0;
                            clsPmpaPb.G7TAMT = 0;
                            clsPmpaPb.GstatPED = "0";
                            clsPmpaPb.GstatER = "0"; //2015-12-30
                            Host_Proc_SGset_ONE(pDbCon, dt, i, ArgSuNext);
                            Host_Proc_Gesan_ONE(pDbCon, i);
                        }

                        #region // Move_HOST_TO_SG
                        //입원 send 에서 그룹으로 전송시 표준계수 적용
                        clsPmpaType.ISG.Qty = clsPmpaType.ISH.Qty;
                        clsPmpaType.ISG.Nal = clsPmpaType.ISH.Nal;
                        clsPmpaType.ISG.Dev = clsPmpaType.ISH.Dev;
                        clsPmpaType.ISG.Imiv = clsPmpaType.ISH.Imiv;
                        clsPmpaType.ISG.GbSelf = clsPmpaType.ISH.GbSelf;
                        if (clsPmpaType.ISG.SugbC != "0")
                        {
                            clsPmpaType.ISG.GbNgt = clsPmpaType.ISH.GbNgt;
                        }
                        else
                        {
                            clsPmpaType.ISG.GbNgt = "0";
                        }
                        //clsPmpaType.ISG.GbNgt = clsPmpaType.ISH.GbNgt;
                        clsPmpaType.ISG.GbSpc = clsPmpaType.ISH.GbSpc;
                        #endregion
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 식이코드 금액 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        /// <seealso cref="Iusent2.bas :  Check_식이코드금액"/>
        string[] Chk_Diet_Code_Amt(PsmhDb pDbCon, string ArgCode)
        {
            string[] Arg = new string[3];

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            Arg[0] = ""; //null 인식 오류 2018-08-18
            Arg[1] = "";  //null 인식 오류 2018-08-18
            try
            {
                SQL = "";
                SQL += " SELECT Bun,Gubun                               ";
                SQL += "   FROM " + ComNum.DB_PMPA + "DIET_NEWCODE      ";
                SQL += "  WHERE SuCode='" + ArgCode + "'                ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (dt.Rows.Count > 0)
                {
                    Arg[0] = dt.Rows[0]["BUN"].ToString().Trim();
                    Arg[1] = dt.Rows[0]["GUBUN"].ToString().Trim(); 
                }

                dt.Dispose();
                dt = null;

                return Arg;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        void SugbSS_Check_65_ONE(ref string strChkflag, string strSugbSS)
        {

            if (strSugbSS == "0") { strChkflag = "OK"; }
            
            if (clsPmpaType.IA.Age < 9)
            {
                if (strSugbSS == "4") { strChkflag = "OK"; }             // 8세이하
            }
            else if (clsPmpaType.IA.Age > 8 && clsPmpaType.IA.Age < 13 && clsPmpaType.IA.Sex == "M")
            {
                if (strSugbSS == "3") { strChkflag = "OK"; }             // 9 - 12 세남아
            }
            else if (clsPmpaType.IA.Sex == "M")
            {
                if (strSugbSS == "1") { strChkflag = "OK"; }
            }
            else
            {
                if (strSugbSS == "2") { strChkflag = "OK"; }
            }
        }

        void SugbSS_Check_ONE(ref string strChkflag, string strSugbSS)
        {
            if (strSugbSS == "0") { strChkflag = "OK"; }

            if (clsPmpaType.IA.Age < 6 && strSugbSS == "2") { strChkflag = "OK"; }
            if (clsPmpaType.IA.Age > 5 && strSugbSS == "1") { strChkflag = "OK"; }
            //2017-07-01 이전기준
            //if (clsPmpaType.IA.Age < 8 && strSugbSS == "2") { strChkflag = "OK"; }
            //if (clsPmpaType.IA.Age > 7 && strSugbSS == "1") { strChkflag = "OK"; }
        }

        /// <summary>
        /// HOST Variable Move
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="dt"></param>
        /// <param name="nCNT"></param>
        /// <seealso cref="Iusent2.bas : Account_Process_Host_ONE > Host_Proc_SGset "/>
        void Host_Proc_SGset_ONE(PsmhDb pDbCon, DataTable dt, int nCNT, string ArgSuNext)
        {
            clsPmpaQuery cPQ = new clsPmpaQuery();
            clsOumsadChk cOCHK = new clsOumsadChk();

            clsPmpaType.ISG.Sunext  = dt.Rows[nCNT]["SUNEXT"].ToString().Trim(); 
            clsPmpaType.ISG.Bun     = dt.Rows[nCNT]["Bun"].ToString().Trim();  
            clsPmpaType.ISG.Nu      = dt.Rows[nCNT]["Nu"].ToString().Trim();   
            clsPmpaType.ISG.SugbA   = dt.Rows[nCNT]["SugbA"].ToString().Trim();
            clsPmpaType.ISG.SugbB   = dt.Rows[nCNT]["SugbB"].ToString().Trim();
            clsPmpaType.ISG.SugbC   = dt.Rows[nCNT]["SugbC"].ToString().Trim();
            clsPmpaType.ISG.SugbD   = dt.Rows[nCNT]["SugbD"].ToString().Trim();
            clsPmpaType.ISG.SugbE   = dt.Rows[nCNT]["SugbE"].ToString().Trim();
            clsPmpaType.ISG.SugbF   = dt.Rows[nCNT]["SugbF"].ToString().Trim();
            clsPmpaType.ISG.SugbG   = dt.Rows[nCNT]["SugbG"].ToString().Trim();
            clsPmpaType.ISG.SugbH   = dt.Rows[nCNT]["SugbH"].ToString().Trim();
            clsPmpaType.ISG.SugbI   = dt.Rows[nCNT]["SugbI"].ToString().Trim();
            clsPmpaType.ISG.SugbJ   = dt.Rows[nCNT]["SugbJ"].ToString().Trim();
            clsPmpaType.ISG.SugbQ   = dt.Rows[nCNT]["SugbQ"].ToString().Trim();
            clsPmpaType.ISG.SugbR   = dt.Rows[nCNT]["SugbR"].ToString().Trim();
            clsPmpaType.ISG.SugbAA  = dt.Rows[nCNT]["SugbAA"].ToString().Trim();
            clsPmpaType.ISG.SugbAB = dt.Rows[nCNT]["SugbAB"].ToString().Trim();
            clsPmpaType.ISG.SugbS   = dt.Rows[nCNT]["SugbS"].ToString().Trim();
            clsPmpaType.ISG.SugbAG  = dt.Rows[nCNT]["SugbAG"].ToString().Trim();

            if (clsPmpaType.ISG.SugbS == "6" || clsPmpaType.ISG.SugbS == "7")
            {
                if (string.Compare(clsPmpaType.IA.Date, "2016-09-01") < 0) { clsPmpaType.ISG.SugbS = "1"; }
            }

            clsPmpaType.ISG.Iamt    = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Iamt"].ToString()));
            clsPmpaType.ISG.Tamt    = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Tamt"].ToString()));
            clsPmpaType.ISG.Bamt    = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Bamt"].ToString())); 
            clsPmpaType.ISG.Sudate  = dt.Rows[nCNT]["Sdate"].ToString().Trim(); 
            clsPmpaType.ISG.OldIamt = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["OldIamt"].ToString())); 
            clsPmpaType.ISG.OldTamt = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["OldTamt"].ToString())); 
            clsPmpaType.ISG.OldBamt = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["OldBamt"].ToString()));

            //입원 send에서 전송시 그룹수가로 들어오면 표준계수 적용함
            //if (clsPmpaType.ISG.SugbG != "4")
            if (clsPmpaType.ISG.SugbG != "4" && ArgSuNext == "")
            {
                clsPmpaType.ISG.Qty = VB.Val(dt.Rows[nCNT]["SuQty"].ToString()) * clsPmpaType.ISG.Qty;
            }

            clsPmpaType.ISG.Sudate3 = dt.Rows[nCNT]["Sudate3"].ToString().Trim();
            clsPmpaType.ISG.Iamt3   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Iamt3"].ToString()));
            clsPmpaType.ISG.Tamt3   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Tamt3"].ToString()));
            clsPmpaType.ISG.Bamt3   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Bamt3"].ToString()));    
            clsPmpaType.ISG.Sudate4 = dt.Rows[nCNT]["Sudate4"].ToString().Trim(); 
            clsPmpaType.ISG.Iamt4   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Iamt4"].ToString()));
            clsPmpaType.ISG.Tamt4   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Tamt4"].ToString()));
            clsPmpaType.ISG.Bamt4   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Bamt4"].ToString())); 
            clsPmpaType.ISG.Sudate5 = dt.Rows[nCNT]["Sudate5"].ToString().Trim(); 
            clsPmpaType.ISG.Iamt5   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Iamt5"].ToString()));
            clsPmpaType.ISG.Tamt5   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Tamt5"].ToString()));
            clsPmpaType.ISG.Bamt5   = Convert.ToInt64(VB.Val(dt.Rows[nCNT]["Bamt5"].ToString()));

            if (string.Compare(clsPmpaType.ISG.Sudate, clsPmpaType.IA.Date) <= 0 && clsPmpaType.ISG.Sudate != "")
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.ISG.Iamt;
                clsPmpaType.ISG.Tamt = clsPmpaType.ISG.Tamt;
                clsPmpaType.ISG.Bamt = clsPmpaType.ISG.Bamt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate3, clsPmpaType.IA.Date) <= 0 && clsPmpaType.ISG.Sudate != "")
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.ISG.OldIamt;
                clsPmpaType.ISG.Tamt = clsPmpaType.ISG.OldTamt;
                clsPmpaType.ISG.Bamt = clsPmpaType.ISG.OldBamt;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate4, clsPmpaType.IA.Date) <= 0 && clsPmpaType.ISG.Sudate != "")
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.ISG.Iamt3;
                clsPmpaType.ISG.Tamt = clsPmpaType.ISG.Tamt3;
                clsPmpaType.ISG.Bamt = clsPmpaType.ISG.Bamt3;
            }
            else if (string.Compare(clsPmpaType.ISG.Sudate5, clsPmpaType.IA.Date) <= 0 && clsPmpaType.ISG.Sudate != "")
            { 
                clsPmpaType.ISG.Iamt = clsPmpaType.ISG.Iamt4;
                clsPmpaType.ISG.Tamt = clsPmpaType.ISG.Tamt4;
                clsPmpaType.ISG.Bamt = clsPmpaType.ISG.Bamt4;
            }
            else
            {
                clsPmpaType.ISG.Iamt = clsPmpaType.ISG.Iamt5;
                clsPmpaType.ISG.Tamt = clsPmpaType.ISG.Tamt5;
                clsPmpaType.ISG.Bamt = clsPmpaType.ISG.Bamt5;
            }

            if (clsPmpaPb.GstrSugaNewUseFlag == "OK" && cOCHK.Check_LowDrug_Suga(pDbCon, clsPmpaType.ISG.Sucode) == "OK")
            {
                cPQ.Read_Suga_Amt(pDbCon, clsPmpaType.ISG.Sucode, clsPmpaType.ISG.Sunext, clsPmpaType.ISG.Sudate);

                if (clsPmpaPb.GstrSugaNewReadOK == "OK")
                {
                    clsPmpaType.ISG.Iamt = clsPmpaPb.GnIAmt;
                    clsPmpaType.ISG.Tamt = clsPmpaPb.GnTAmt;
                    clsPmpaType.ISG.Bamt = clsPmpaPb.GnBAmt;
                }
            }

            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Bamt;

            if (clsPmpaType.IA.Bi > 50 && clsPmpaType.ISG.GbSelf == "1")
            {
                clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt;
            }
            else if (clsPmpaType.IA.Bi == 51)
            {
                clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt;
            }

            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55) { clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Tamt; }
            if (clsPmpaType.IA.Bi == 31 && string.Compare(clsPmpaType.ISG.SugbQ, "0") > 0) { clsPmpaType.ISG.SugbF = "0"; }
            if (clsPmpaType.IA.Bi == 51 && clsPmpaType.IA.Gbilban2 == "Y") { clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.Iamt * 2; } //외국new
            if (string.Compare(clsPmpaType.ISG.GbSpc, "0") > 0) { clsPmpaType.ISG.GbSpc = clsPmpaType.ISG.SugbH; }
                
            //묶음코드는 내복약, 외복약, 주사약 "1"로 강제 SETTING함.
            if (clsPmpaType.ISG.Bun == "11" || clsPmpaType.ISG.Bun == "12" || clsPmpaType.ISG.Bun == "20")
            {
                clsPmpaType.ISG.Div = 1;
            }

        }

        /// <summary>
        /// HOST Routine 계산 Call
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <seealso cref="Iusent2.bas : Account_Process_Host_ONE > Host_Proc_Gesan"/>
        void Host_Proc_Gesan_ONE(PsmhDb pDbCon, int nCnt)
        {
            if (clsPmpaType.ISG.SugbG != "4" && clsPmpaType.ISG.Qty == 0) { return; }
            if (clsPmpaType.ISG.BaseAmt == 0) { return; }
            if (clsPmpaPb.GhostDAEPYO == "1") { clsPmpaPb.GhostDAEPYO = "2"; }
            if (clsPmpaPb.GhostDAEPYO == "0") { clsPmpaPb.GhostDAEPYO = "1"; }
                
            if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30 && clsPmpaType.IA.Dept == "NP" && string.Compare(clsPmpaType.ISG.Bun, "02") > 0)   //보호정신과 단일수가
            { 
                if (clsPmpaType.ISG.SugbF == "0")   //이외 급여분 금액 Zero
                {
                    clsPmpaType.ISG.BaseAmt = 0;
                }
            }

            if (clsPmpaType.IA.Bi > 20 && clsPmpaType.IA.Bi < 30)
            { 
                if (clsPmpaType.IA.Dept == "NP" && clsPmpaType.ISG.BaseAmt == 0)
                {
                    return;
                }
            }

            if (string.Compare(clsPmpaType.ISG.Bun, "11") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "15") <= 0)
            {
                //BUN_11_Account_ONE();
                BUN_11_Account(pDbCon, nCnt);
            }
            else if (string.Compare(clsPmpaType.ISG.Bun, "16") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "21") <= 0)
            {
                //BUN_20_Account_ONE();
                BUN_20_Account(pDbCon);
            }
            else if (string.Compare(clsPmpaType.ISG.Bun, "22") >= 0 && string.Compare(clsPmpaType.ISG.Bun, "23") <= 0)
            {
                //BUN_22_Account_ONE();
                BUN_22_Account(pDbCon);
            }
            else if (clsPmpaType.ISG.Bun == "28" || clsPmpaType.ISG.Bun == "34" || clsPmpaType.ISG.Bun == "35")
            {
                //BUN_34_Account_ONE();
                BUN_34_Account(pDbCon);
            }
            else if (clsPmpaType.ISG.Bun == "37")
            {
                //BUN_37_Account_ONE();
                BUN_37_Account(pDbCon);
            }
            else if (clsPmpaType.ISG.Bun == "75")
            {
                //BUN_75_Account_ONE();
                BUN_75_Account(pDbCon);
            }
            else
            {
                //BUN_99_Account_ONE();
                BUN_99_Account(pDbCon);
            }
        }

        void BUN_11_Account_ONE()
        {
            double Rem = 0 ;
            if (clsPmpaType.ISG.POWDER == "1") { Rem = 0.3; }
            if (clsPmpaType.IA.Bi == 52 || clsPmpaType.IA.Bi == 55)
            {
                #region //BUN_11_TA_ONE
                // 6세미만 소아 내복약조제료 20%가산 -> 30/ 50 가산 30% 가루약가산 
                

                if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age == 0)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.5 + Rem));
                }
                else if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.3 + Rem));
                }
                else if(clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.0 + Rem));
                }

                if (clsPmpaType.ISG.Bun == "13" && clsPmpaType.ISG.Sucode == "J2000" )
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1 + Rem));
                }
              

                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                }
                #endregion
            }
            else if (clsPmpaType.IA.Bi > 50 || clsPmpaType.ISG.GbSelf == "1" || string.Compare(clsPmpaType.ISG.SugbF, "0") > 0)
            {
                #region //BUN_11_ILBAN_ONE
                // 6세미만 소아 조제료 20%가산
                if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age == 0)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.5 + Rem));
                }
                else if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.3 + Rem));
                }
                else if(clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.0 + Rem));
                }

                if (clsPmpaType.ISG.Bun == "13" && clsPmpaType.ISG.Sucode == "J2000")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1 + Rem));
                }

                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
                #endregion
            }
            else
            {
                #region //BUN_11_BOHUM_ONE
                // 6세미만 소아 내복약조제료 20%가산 
                if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age == 0)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.5 + Rem));
                }
                else if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" && clsPmpaType.IA.Age < 6)
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.3 + Rem));
                }
                else if (clsPmpaType.ISG.Bun == "13" && VB.Left(clsPmpaType.ISG.Sucode, 2) == "J1" )
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1.0 + Rem));
                }

                if (clsPmpaType.ISG.Bun == "13" && clsPmpaType.ISG.Sucode == "J2000")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * (1 + Rem));
                }

                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                }
                #endregion
            }

        }

        void BUN_20_Account_ONE()
        {
            double nQty = clsPmpaType.ISG.Qty;
            int nNal = clsPmpaType.ISG.Nal;
            string strC = clsPmpaType.ISG.SugbC;

            if (clsPmpaType.IA.Bi == 51 || clsPmpaType.ISG.GbSelf == "1")
            {
                #region // BUN_20_ILBAN_ONE (일반 주사료 계산)
                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
                #endregion
            }
            else
            {
                BUN_20_BOHUM_ONE();
            }
        }

        /// <summary>
        /// 보험 주사료 계산
        /// </summary>
        void BUN_20_BOHUM_ONE()
        {
            clsBasAcct cBAcct = new clsBasAcct();
            //ER 중증환자 가산 
            cBAcct.Bas_ER_Rate("", "", clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.ISG.GBKTAS, "I", "OK");
            //소아가간율 SET
            cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date); //GnPedRate

            if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB != "0")
            { 
                if (clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0)
                    {
                        clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
                    }
                }

                if (clsPmpaType.ISG.Bun == "19" && clsPmpaType.ISG.Sucode != "KK041")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0));
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    if (clsPmpaPb.GnErRate > 0)
                    {
                        clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
                    }
                }
            }
            else if (clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbAA != "0")
            { 
                if (clsPmpaType.ISG.Bun == "17" || clsPmpaType.ISG.Bun == "18")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
                    if (clsPmpaPb.GnErRate > 0)
                    {
                        clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
                    }
                }
                
                if (clsPmpaType.ISG.Bun == "19" && clsPmpaType.ISG.Sucode != "KK041")
                { 
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * ((100 + clsPmpaPb.GnErRate) / 100.0));
                    if (clsPmpaPb.GnErRate > 0)
                    {
                        clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
                    }
                }
            }

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            //기술료 가산
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
            }
        }

        void BUN_22_Account_ONE()
        {
            int nNgt = 150;
            clsBasAcct cBAcct = new clsBasAcct();

            //ER 중증환자 가산 
            cBAcct.Bas_ER_Rate("", "", clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.ISG.GBKTAS, "I", "OK");
            //소아가간율 SET
            cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date, clsPmpaType.IA.AgeiLsu, "I", "", clsPmpaType.ISG.GbChildZ);

            //2014-11-25 소아가산인경우만 GbChild에 구분 표시함
            if (clsPmpaPb.GnPedRate > 0)
            {
                clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
            }
            if (clsPmpaPb.GnErRate > 0)
            {
                clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
            }

            if (clsPmpaType.ISG.Bun == "23" || clsPmpaType.ISG.SugbE == "0")
            {
                // 마취 행위료 계산
                clsPmpaPb.G7AMT = cBAcct.BAS_MACH_AMT(2, clsPmpaType.ISG.Sunext, clsPmpaType.ISG.BaseAmt, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
            }
            else
            {
                //마취 재료대 계산
                clsPmpaPb.G7AMT = cBAcct.BAS_MACH_AMT(1, clsPmpaType.ISG.Sunext, clsPmpaType.ISG.BaseAmt, clsPmpaType.ISG.Qty, clsPmpaType.ISG.Nal);
            }

            if (clsPmpaType.ISG.Nal < 0)
            {
                clsPmpaPb.G7AMT = clsPmpaPb.G7AMT * -1;
            }

            //심야가산율 Check
            if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.NGT22_DATE) < 0 && clsPmpaPb.OLD_NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
            {
                nNgt = clsPmpaPb.OLD_NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }
            else
            {
                nNgt = clsPmpaType.ISG.GbNgt == "D" ? clsPmpaPb.NIGHT_22[8] : clsPmpaPb.NIGHT_22[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }

            if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0)
            {
                nNgt = 100;
            }

            //야간, 응급, 기술료 가산
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * (nNgt + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0, 2, MidpointRounding.AwayFromZero);
                
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
            }
        }

        void BUN_34_Account_ONE()
        {
            clsBasAcct cBAcct = new clsBasAcct();
            //ER 중증환자 가산 
            cBAcct.Bas_ER_Rate("", "", clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.ISG.GBKTAS, "I", "OK");
            //소아가간율 SET
            cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date, clsPmpaType.IA.AgeiLsu, "I", "", clsPmpaType.ISG.GbChildZ);

            BUN_34_BOHUM_ONE();
        }

        void BUN_34_BOHUM_ONE()
        {
            clsBasAcct cBAcct = new clsBasAcct();

            int nNgt = 150;
            int nSAdd = 0;
            int nAdd = 0;
            double nRem = 1.0;

            //심야가산율 Check
            if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.NIGHT_DATE) < 0 && clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
            {
                nNgt = clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }
            else
            {
                nNgt = clsPmpaType.ISG.GbNgt == "D" ? clsPmpaPb.NIGHT[8] : clsPmpaPb.NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }

            if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0)
            {
                nNgt = 100;
            }

            if (clsPmpaType.ISG.Sucode.Trim() == "O1510") { nSAdd = 70; }        //강제작업 가산 70%
            if (clsPmpaType.ISG.Sucode.Trim() == "Q2481") { nSAdd = 30; }        //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0011A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0012A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0053A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0054A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "NA055A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "NA056A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0057A") { nSAdd = 30; }       //강제작업 가산 30%
            if (clsPmpaType.ISG.Sucode.Trim() == "N0058A") { nSAdd = 30; }       //강제작업 가산 30%
            
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            { 
                if (clsPmpaType.ISG.SugbB == "Z" && (clsPmpaType.IA.Age >= 35 || clsPmpaType.ISG.GbChildZ == "Z"))
                {
                    if (clsPmpaPb.GnPedRate > 0) { clsPmpaPb.GstatPED = "Z"; }
                }

                if (string.Compare(clsPmpaType.ISG.GbNgt, "0") > 0)
                {
                    //단가에 기가산여부 Check
                    switch (clsPmpaType.ISG.SugbJ)
                    {
                        case "1":   nRem = 1.1; nAdd = 10; break;
                        case "2":   nRem = 1.2; nAdd = 20; break;
                        case "3":   nRem = 1.3; nAdd = 30; break;
                        case "4":   nRem = 1.4; nAdd = 40; break;
                        case "5":   nRem = 1.5; nAdd = 50; break;
                        case "6":   nRem = 1.6; nAdd = 60; break;
                        case "7":   nRem = 1.7; nAdd = 70; break;
                        case "8":   nRem = 1.8; nAdd = 80; break;
                        case "9":   nRem = 1.9; nAdd = 90; break;
                        default:  nRem = 1;   nAdd = 0; break;
                    }
                }

                clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt / nRem * (nSAdd + nNgt + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate + nAdd) / 100.0, 2, MidpointRounding.AwayFromZero);
                
                if (clsPmpaPb.GstatPED != "Z")
                { 
                    if (clsPmpaPb.GnPedRate == 0)
                    {
                        clsPmpaPb.GstatPED = "0";
                    }
                    else
                    {
                        clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                    }
                }

                if (clsPmpaPb.GnErRate > 0) { clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS; }

            }

            //2014-07-24 clsPmpaType.ISG.GbNgt A: 제2의수술  B:제2의수술 휴일  C:제2의수술 야간 추가
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0 && (clsPmpaType.ISG.GbNgt == "A" || clsPmpaType.ISG.GbNgt == "B" || clsPmpaType.ISG.GbNgt == "C"))
            {
                //2015-02-03 DRG 부수술은 개수 조정안함
                if (clsPmpaType.TIT.GbDRG != "D") { clsPmpaType.ISG.Qty = clsPmpaType.ISG.Qty * 0.7; }
            }
            else if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0 && string.Compare(clsPmpaType.ISG.GbNgt, "4") > 0 && clsPmpaType.ISG.GbNgt != "9" && clsPmpaType.ISG.GbNgt != "D")
            {
                //2015-02-03 DRG 부수술은 개수 조정안함
                if (clsPmpaType.TIT.GbDRG != "D") { clsPmpaType.ISG.Qty = clsPmpaType.ISG.Qty * 0.5; }
            }

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

            //기술료 가산
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
            }
             
        }

        void BUN_37_Account_ONE()
        {
            clsBasAcct cBAcct = new clsBasAcct();

            if (clsPmpaType.ISG.SugbB != "1")
            { 
                if (clsPmpaType.IA.Age < 8 && clsPmpaType.ISG.SugbE == "1" && clsPmpaType.ISG.SugbB == "2")
                {
                    clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * 1.2);
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                }

                clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);

                //기술료 가산
                if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                {
                    if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                    }
                }

            }
        }

        void BUN_75_Account_ONE()
        {
            switch (clsPmpaType.ISG.SugbB)
            {
                case "1":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 1000)); break;
                case "2":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 2000)); break;
                case "3":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * 3000)); break;
                case "4":   clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt + ((clsPmpaType.ISG.Qty - 1) * (clsPmpaType.ISG.BaseAmt * 0.1))); break;
                default:    clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * Math.Abs(clsPmpaType.ISG.Nal)); break;
            }

            if (clsPmpaType.ISG.Nal < 0) { clsPmpaPb.G7AMT = clsPmpaPb.G7AMT * -1; }
        }

        void BUN_99_Account_ONE()
        {
            if (clsPmpaType.ISG.Bun == "01" || clsPmpaType.ISG.Bun == "02")
            {
                BUN_99_JIN_ONE();
            }
            else
            {
                BUN_99_BOHUM_ONE();
            }
        }

        void BUN_99_JIN_ONE()
        {
            //진찰료 소아가산 프로그램 변경
            if (clsPmpaType.IA.Dept == "DT" && clsPmpaType.TIT.Bi.Substring(0, 1) == "2" && clsPmpaType.TIT.Bohun == "3")
            {
                clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + 500;                   //나이불문 500원 가산
            }
            else if (clsPmpaType.IA.Age < 6 && clsPmpaType.ISG.Sucode.Trim() != "AA256" && clsPmpaType.ISG.Sucode.Trim() != "AA25710Z" && clsPmpaType.ISG.Sucode.Trim() != "V4203" && clsPmpaType.ISG.Sucode.Trim() != "AH3B3" && clsPmpaType.ISG.Sucode.Trim() != "V4300" && clsPmpaType.ISG.Sucode.Trim() != "AA702" && clsPmpaType.ISG.Sucode.Trim() != "V2300" && clsPmpaType.ISG.Sucode.Trim() != "V2200" && clsPmpaType.ISG.Sucode.Trim() != "V3203")
            { 
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.PedAdd_Date) >= 0) //소아가산 0세 구분 추가 2017-07-01
                { 
                    if (clsPmpaType.ISG.Bun == "01")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAddYg1;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAdd1;      //초진소아가산
                        }    
                    }
                    
                    if (clsPmpaType.ISG.Bun == "02")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAddYg3;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.PedAdd3;     //재진소아가산
                        }
                    }
                }
                else
                { 
                    if (clsPmpaType.ISG.Bun == "01")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAddYg1;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAdd1; //초진소아가산
                        }
                    }
                    
                    if (clsPmpaType.ISG.Bun == "02")
                    {
                        if (string.Compare(clsPmpaType.IA.Date, "2017-07-01") >= 0 && clsPmpaType.IA.Age == 0)
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAddYg2;
                        }
                        else
                        {
                            clsPmpaType.ISG.BaseAmt = clsPmpaType.ISG.BaseAmt + clsPmpaPb.Old_PedAdd2; //재진소아가산
                        }
                    }
                }
            }

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal);
        }

        void BUN_99_BOHUM_ONE()
        {
            clsBasAcct cBAcct = new clsBasAcct();
            int nNgt = 150;
            double nRem = 1.0;

            //심야가산율 Check
            if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.NIGHT_DATE) < 0 && clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)] > 0)
            {
                nNgt = clsPmpaPb.OLD_NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }
            else
            {
                nNgt = clsPmpaPb.NIGHT[(int)VB.Val(clsPmpaType.ISG.GbNgt)];
            }

            //2015-12-28
            if (string.Compare(clsPmpaType.ISG.GbNgt, "0") <= 0 || string.Compare(clsPmpaType.ISG.SugbC, "0") <= 0)
            {
                nNgt = 100;
            }


            //2015-12-28 ER 중증환자 가산  vb60_new\BaseFile\WERACCT.bas
            cBAcct.Bas_ER_Rate("", "", clsPmpaType.ISG.SugbAA, clsPmpaType.IA.Dept, clsPmpaType.ISG.GBKTAS, "I", "OK");

            //소아가간율 SET
            cBAcct.Bas_PED_Rate(clsPmpaType.IA.Age, clsPmpaType.ISG.SugbB, clsPmpaType.ISG.SugbE, clsPmpaType.ISG.Bun, clsPmpaType.IA.Date);

            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                clsPmpaType.ISG.BaseAmt = (long)Math.Round(clsPmpaType.ISG.BaseAmt * (nNgt + clsPmpaPb.GnPedRate + clsPmpaPb.GnErRate) / 100.0, 2, MidpointRounding.AwayFromZero);

                //2014-11-25 소아가산인경우만 GbChild에 구분 표시함
                if (clsPmpaPb.GnPedRate > 0)
                {
                    clsPmpaPb.GstatPED = cBAcct.Bas_PED_Slip(clsPmpaType.IA.Date, clsPmpaType.IA.Age, clsPmpaType.IA.AgeiLsu); //2017-07-01 add
                }
                if (clsPmpaPb.GnErRate > 0)
                {
                    clsPmpaPb.GstatER = clsPmpaType.ISG.GBKTAS;
                }
                clsPmpaType.ISG.GbChildZ = clsPmpaPb.GstatPED;
            }
            if  (clsPmpaType.ISG.Sucode =="AC302")
            {
                if (clsPmpaType.IA.AgeiLsu <= 28 && clsPmpaType.IA.Age == 0)
                {
                    nRem += 1.0;
                }
                else if (clsPmpaType.IA.Age == 0 )
                {
                    nRem += 0.5;
                }
                else if (clsPmpaType.IA.Age < 6 )
                {
                    nRem += 0.3;
                }
                if (clsPmpaType.ISG.GbNgt != "0" && clsPmpaType.ISG.GbNgt != "")
                {
                    nRem += 0.5;
                }
            }
            clsPmpaType.ISG.BaseAmt = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * nRem);

            clsPmpaPb.G7AMT = (long)Math.Truncate(clsPmpaType.ISG.BaseAmt * clsPmpaType.ISG.Qty * clsPmpaType.ISG.Nal );
            
            //기술료 가산
            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
            {
                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)    //기술료가산
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    clsPmpaPb.G7AMT = (long)Math.Round(clsPmpaPb.G7AMT * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                }
            }
        }

        #endregion
    }
}
