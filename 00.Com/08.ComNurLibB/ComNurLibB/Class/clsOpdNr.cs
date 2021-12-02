using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using ComDbB;
using ComBase;
using ComLibB;

namespace ComNurLibB
{
    //외래간호 공용 사용
    public class clsOpdNr
    {
        ComFunc cfun = new ComFunc();

        //VbOcs_Res.bas
        public static string Gstr예약오더정보 = "";
        public static string Gstr예약오더구분 = "";


        //opdwait.ini
        public static string AllDeptCode = "";
        public static string AllDrCode = "";
        public static int AllDoctCnt = 0;

        //emrprt~1.ini
        public static bool GbPrtYN = false;                 //'표준서식지 해당 PC에서 출력 여부(True/False)
        public static string GstrEmrDoct = "";              //'EMR 진료기록지를 출력할 의사 명단 (예: '1101','1102','2101')
        public static string GstrXrayDoct = "";             //'EMR 진료기록지를 조회할 의사 명단 (예: '1101','1102','2101')
        public static string GstrEmrViewDoct = "";          //'EMR 진료기록지를 조회할 의사 명단 (예: '1101','1102','2101')
        public static string GstrEmrViewDoct_New = "";
        public static string GstrEmrViewDoct_PU = "";
        
        public static string gstrFluPrt = "";               //'신종플루인쇄 여부
        public static string Gstr내과_SORT = "";

        public static string GnPrtDev1 = "";                //'EMR서식
        public static string GnPrtDev2 = "";                //'접수증
        public static string GnPrtScaleMode = "";           //'현재 기본프린터의 Scale Mode
        public static string GnPrtDeviceNo = "";

        public static string GstrMasterCode = "";

        public static string OpdWait_path = @"C:\CMC\OpdWait.INI";
        public static string EMRPRT_path = @"C:\CMC\EMRPRT.INI";
        public static string EMRPRT_path1 = @"C:\CMC\EMRPRT1.INI";
        public static string EMRPRT_path2 = @"C:\CMC\EMRPRT2.INI";

        public static string GstrLock = "";
        public static string GstrPassName = "";

        public class MedOpdNur
        {
            //public string AllDeptCode;      //'진료과(Ex:'MD','GS',...)
            //public string AllDrCode;        //'의사코드(Ex:'1101','1102',...)
            public int DoctCNT;             //'표시할 의사 인원수
            public string DeptCode;
            public string DrCode;
            public string DrName;
            public string GbJin;
            public string GbJin2;
            public string AmTime;
            public string AmJinYN;
            public string PmTime;
            public string PmJinYN;
            public string YTimeGbn;
        }
        public static MedOpdNur[] OWT = null;


        public class MedDrSchColor
        {
            public string STS01 = "1"; //진료
            public string STS02 = "2"; //수술
            public string STS03 = "3"; //특검
            public string STS04 = "4"; //휴진
            public string STS05 = "5"; //학회
            public string STS06 = "6"; //휴가
            public string STS07 = "7"; //출장
            public string STS08 = "8"; //기타
            public string STS09 = "9"; //OFF
            public string STS10 = "D"; //교육
            public string STS11 = "A"; //협진
            
        }


        public static opdNrPatientInfo ClearPatient()
        {
            opdNrPatientInfo pinfo = new opdNrPatientInfo();

            pinfo.PtNo = "";
            pinfo.PtAcpNo = "";
            pinfo.PtInOutCls = "";
            pinfo.PtMedFrDate = "";
            pinfo.PtMedFrTime = "";
            pinfo.PtMedEndDate = "";
            pinfo.PtMedEndTime = "";
            pinfo.PtMedDeptCd = "";
            pinfo.PtMedDrCd = "";
            pinfo.PtName = "";
            pinfo.PtAge = "";
            pinfo.PtSex = "";            
            pinfo.PtJumin1 = "";
            pinfo.PtJumin2 = "";
            pinfo.PtIPDNO = "";
            pinfo.PtWard = "";

            pinfo.strFlag = "";
            pinfo.strROWID = "";
            pinfo.strExam = "";

            return pinfo;
        }



        #region //.ini 파일 로드

        public static bool READ_OPDWAIT_DB(PsmhDb pDbCon)
        {
            bool rtnVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string[] strTemp;

            AllDeptCode = "";
            AllDoctCnt = 0;
            AllDrCode = "";
            GstrEmrViewDoct = "";
            GbPrtYN = false;
            gstrFluPrt = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "     CODE, VALUEV ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PCCONFIG ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '외래간호' ";
                //TODO
                SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.89'";     //CS,UR   흉부외과, 비뇨기과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.33.63' ";    //DM      피부과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.4.101'";     //FM,DT   가정의학과, 치과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.22.93' ";    //EN      이비인후과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.22.88'";     //GS      외과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.32.50'";     //MC      심장내과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.32.122'";    //ME      내분비내과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.32.66'";     //MG      소화기내과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.131'";    //MI     
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.32.117'";    //MN,MP   신장내과, 호흡기내과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.72'";     //MR,RA   류마티스내과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.58'";     //NE,RM   신경과, 재활의학과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.33.94'";     //NP      정신건강의학과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.61'";     //NS      신경외과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.33.66'";     //OG      산부인과   
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.31.70'";     //OS      정형외과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.22.104'";    //OT      안과
                //SQL = SQL + ComNum.VBLF + "   AND IPADDRESS = '192.168.33.112'";    //PD      소아청소년과                        

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    //ComFunc.MsgBox("프로그램 PC세팅 후 다시 실행해주세요.");
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "OPDWAIT_DEPT")
                        {
                            strTemp = dt.Rows[i]["VALUEV"].ToString().Trim().Split(',');
                            if (strTemp[0] != "")
                            {
                                foreach (string ss in strTemp)
                                {
                                    AllDeptCode += "'" + ss + "',";
                                }

                                AllDeptCode = AllDeptCode.Substring(0, AllDeptCode.Length - 1);
                            }
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT_진료의사")
                        {                            
                            strTemp = dt.Rows[i]["VALUEV"].ToString().Trim().Split(',');
                            if (strTemp[0] != "")
                            {
                                foreach (string ss in strTemp)
                                {
                                    GstrEmrDoct += "'" + ss + "',";
                                    //AllDoctCnt += 1;
                                }

                                GstrEmrDoct = GstrEmrDoct.Substring(0, GstrEmrDoct.Length - 1);
                            }
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT_인쇄")
                        {                            
                            GbPrtYN = (dt.Rows[i]["VALUEV"].ToString().Trim() == "Y" ? true : false);                            
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT_XRAY")
                        {
                            strTemp = dt.Rows[i]["VALUEV"].ToString().Trim().Split(',');
                            if (strTemp[0] != "")
                            {
                                foreach (string ss in strTemp)
                                {
                                    GstrXrayDoct += "'" + ss + "',";
                                }

                                GstrXrayDoct = GstrXrayDoct.Substring(0, GstrXrayDoct.Length - 1);
                            }
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT_INFLUE")
                        {
                            gstrFluPrt = dt.Rows[i]["VALUEV"].ToString().Trim();
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT1_진료의사")
                        {
                            strTemp = dt.Rows[i]["VALUEV"].ToString().Trim().Split(',');
                            if (strTemp[0] != "")
                            {
                                foreach (string ss in strTemp)
                                {
                                    GstrEmrViewDoct += "'" + ss + "',";
                                }

                                GstrEmrViewDoct = GstrEmrViewDoct.Substring(0, GstrEmrViewDoct.Length - 1);
                            }
                        }
                        if (dt.Rows[i]["CODE"].ToString().Trim() == "EMRPRT1_XRAY")
                        {
                            strTemp = dt.Rows[i]["VALUEV"].ToString().Trim().Split(',');
                            if (strTemp[0] != "")
                            {
                                foreach (string ss in strTemp)
                                {
                                    GstrEmrViewDoct_PU += "'" + ss + "',";
                                }

                                GstrEmrViewDoct_PU = GstrEmrViewDoct_PU.Substring(0, GstrEmrViewDoct_PU.Length - 1);
                            }
                        }
                    }
                }
                
                dt.Dispose();
                dt = null;

                if(AllDeptCode != "" && GstrEmrDoct != "")
                {
                    rtnVal = true;
                }
                                
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        
        /// <summary>
        /// OPDWAIT_INI 파일 로드
        /// </summary>
        public static void READ_OPDWAIT_INI(PsmhDb pDbCon)
        {
            string strNS_SeqChk = "";

            #region // 사용안함
            //string strREC;

            //AllDeptCode = "";
            //AllDoctCnt = 0;
            //AllDrCode = "";

            //ComFunc.ReadSysDate(pDbCon);

            //if (System.IO.File.Exists(OpdWait_path))
            //{
            //    StreamReader sr = new StreamReader(OpdWait_path, System.Text.Encoding.Default);

            //    while ((strREC = sr.ReadLine()) != null)
            //    {
            //        if (strREC.StartsWith("진료과:"))
            //        {
            //            AllDeptCode = strREC.Substring("진료과:".Length, strREC.Length - "진료과:".Length);                        

            //            //MessageBox.Show(OpdWait.AllDeptCode);

            //        }
            //    }

            //    sr.Close();
            //}
            //else
            //{
            //    MessageBox.Show(OpdWait_path + "설정파일 없음");
            //}
            #endregion


            //쿼리
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (AllDeptCode != "")
                {
                    SQL = " SELECT ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + "  WHERE DrDept1 ='NS' ";
                    SQL = SQL + ComNum.VBLF + " AND DRCODE IN ( " + AllDeptCode + " ) ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        strNS_SeqChk = "Y";
                    }

                    dt.Dispose();
                    dt = null;

                }

                if (strNS_SeqChk == "Y")
                {

                    SQL = " SELECT a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                    SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_SCHEDULE b, " + ComNum.DB_PMPA + "BAS_CLINICDEPT c ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.DrDept1 IN (" + AllDeptCode + ") ";

                    SQL = SQL + ComNum.VBLF + " AND a.DrCode NOT IN ('1109','1113','1124','1402','1403','1405')  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrDept1 = c.DeptCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode=b.DrCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode <> '0104'  ";
                    SQL = SQL + ComNum.VBLF + " AND b.SchDate=TRUNC(SYSDATE)  ";
                    SQL = SQL + ComNum.VBLF + " AND (a.TOUR <> 'Y' OR a.TOUR IS NULL) ";

                    if (string.Compare(clsPublic.GstrSysTime, "13:00") > 0)
                    {
                        if (VB.I(AllDeptCode, "EN") > 1 || VB.I(AllDeptCode, "CS") > 1)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin2 IN ('1','2')  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin2 IN ('1')  ";
                        }
                    }
                    else
                    {
                        if (VB.I(AllDeptCode, "EN") > 1 || VB.I(AllDeptCode, "CS") > 1)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin IN ('1','2')  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin IN ('1')  ";
                        }                        
                    }

                    SQL = SQL + ComNum.VBLF + "  AND substr(a.drcode ,3,2) <> '99' ";

                    SQL = SQL + ComNum.VBLF + " GROUP BY a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                    SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";

                    SQL = SQL + ComNum.VBLF + " UNION ";

                    SQL = SQL + ComNum.VBLF + " SELECT a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                    SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_SCHEDULE b, " + ComNum.DB_PMPA + "BAS_CLINICDEPT c ,  " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT d ";

                    SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1 IN (" + AllDeptCode + ")  ";

                    SQL = SQL + ComNum.VBLF + " AND a.DrCode NOT IN ('1109','1113','1124','1402','1403','1405')  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrDept1 = c.DeptCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode=b.DrCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode <> '0104'  ";
                    SQL = SQL + ComNum.VBLF + " AND b.SchDate=TRUNC(SYSDATE)  ";
                    SQL = SQL + ComNum.VBLF + " AND (a.TOUR <> 'Y' OR a.TOUR IS NULL) ";

                    SQL = SQL + ComNum.VBLF + " AND b.GbJin IN ('1')  ";
                    SQL = SQL + ComNum.VBLF + " AND substr(a.drcode ,3,2) <> '99' ";
                    SQL = SQL + ComNum.VBLF + " AND d.Gubun ='00'  ";

                    SQL = SQL + ComNum.VBLF + " GROUP BY a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon,";
                    SQL = SQL + ComNum.VBLF + " a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY 1,2,3,4 ";
                }
                else
                {
                    if (Gstr내과_SORT == "2")
                    {
                        SQL = " SELECT '1' AS GBN, a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else if (Gstr내과_SORT == "3")
                    {
                        SQL = " SELECT '1' AS GBN, a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else
                    {
                        SQL = " SELECT '1' AS GBN, a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";                        
                    }


                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_SCHEDULE b, " + ComNum.DB_PMPA + "BAS_CLINICDEPT c ";
                    SQL = SQL + ComNum.VBLF + "  WHERE a.DrDept1 IN (" + AllDeptCode + ") ";


                    SQL = SQL + ComNum.VBLF + " AND a.DrCode NOT IN ('1109','1113','1124','1402','1403','1405')  "; //'김중구,최덕호 ,  곽영균 ,가정의학2분
                    SQL = SQL + ComNum.VBLF + " AND a.DrDept1 = c.DeptCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode=b.DrCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode <> '0104'  ";  //'김중구소장님은 HR에서 근무중이므로 외래에서 제외함
                    SQL = SQL + ComNum.VBLF + " AND b.SchDate=TRUNC(SYSDATE)  ";
                    SQL = SQL + ComNum.VBLF + " AND (a.TOUR <> 'Y' OR a.TOUR IS NULL) ";    //' 퇴사자도 보이는 경우가 있어서 쿼리추가 08.09.19 K.M.C

                    if (string.Compare(clsPublic.GstrSysTime, "13:00") > 0)
                    {
                        if (VB.I(AllDeptCode, "EN") > 1 || VB.I(AllDeptCode, "CS") > 1 || VB.I(AllDeptCode, "OT") > 1)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin2 IN ('1','2')  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin2 IN ('1')  ";
                        }
                    }
                    else
                    {
                        if (VB.I(AllDeptCode, "EN") > 1 || VB.I(AllDeptCode, "CS") > 1 || VB.I(AllDeptCode, "OT") > 1)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin IN ('1','2')  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin IN ('1')  ";
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "  AND substr(a.drcode ,3,2) <> '99' ";

                    if (Gstr내과_SORT == "2")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else if (Gstr내과_SORT == "3")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }


                    SQL = SQL + ComNum.VBLF + " UNION ";


                    if (Gstr내과_SORT == "2")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '1' AS GBN,a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else if (Gstr내과_SORT == "3")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '1' AS GBN,a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '1' AS GBN,a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                        SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }


                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_SCHEDULE b, " + ComNum.DB_PMPA + "BAS_CLINICDEPT c ,  " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT d ";

                    SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1 IN (" + AllDeptCode + ")  ";

                    SQL = SQL + ComNum.VBLF + " AND a.DrCode NOT IN ('1109','1113','1124','1402','1403','1405')  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrDept1 = c.DeptCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode=b.DrCode(+)  ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode=d.DrCode ";
                    SQL = SQL + ComNum.VBLF + " AND a.DrCode <> '0104'  ";
                    SQL = SQL + ComNum.VBLF + " AND b.SchDate=TRUNC(SYSDATE)  ";
                    SQL = SQL + ComNum.VBLF + " AND (a.TOUR <> 'Y' OR a.TOUR IS NULL) ";

                    SQL = SQL + ComNum.VBLF + " AND b.GbJin IN ('1')  ";
                    SQL = SQL + ComNum.VBLF + " AND substr(a.drcode ,3,2) <> '99' ";
                    SQL = SQL + ComNum.VBLF + " AND d.Gubun ='00'  ";


                    if (Gstr내과_SORT == "2")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon,";
                        SQL = SQL + ComNum.VBLF + " a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else if (Gstr내과_SORT == "3")
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon,";
                        SQL = SQL + ComNum.VBLF + " a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon,";
                        SQL = SQL + ComNum.VBLF + " a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                    }

                    if (VB.I(AllDeptCode, "MG") > 1)
                    {
                        SQL = SQL + ComNum.VBLF + " UNION ";

                        if (Gstr내과_SORT == "2")
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT '2' AS GBN, a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }
                        else if (Gstr내과_SORT == "3")
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT '2' AS GBN, a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " SELECT '2' AS GBN, a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "  a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }

                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR a, " + ComNum.DB_PMPA + "BAS_SCHEDULE b, " + ComNum.DB_PMPA + "BAS_CLINICDEPT c ";
                        SQL = SQL + ComNum.VBLF + "  WHERE a.DrDept1 IN (" + AllDeptCode + ") ";


                        SQL = SQL + ComNum.VBLF + " AND a.DrCode NOT IN ( SELECT DRCODE FROM " + ComNum.DB_PMPA + "ETC_JINDISP_DOCT WHERE GUBUN ='01' )  ";
                        SQL = SQL + ComNum.VBLF + " AND a.DrDept1 = c.DeptCode(+)  ";
                        SQL = SQL + ComNum.VBLF + " AND a.DrCode=b.DrCode(+)  ";
                        SQL = SQL + ComNum.VBLF + " AND a.DrCode <> '0104'  ";
                        SQL = SQL + ComNum.VBLF + " AND b.SchDate=TRUNC(SYSDATE)  ";
                        SQL = SQL + ComNum.VBLF + " AND (a.TOUR <> 'Y' OR a.TOUR IS NULL) ";

                        if (string.Compare(clsPublic.GstrSysTime, "13:00") > 0)
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin2 IN ('3')  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  AND b.GbJin IN ('3')  ";
                        }

                        SQL = SQL + ComNum.VBLF + "  AND substr(a.drcode ,3,2) <> '99' ";

                        if (Gstr내과_SORT == "2")
                        {
                            SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts2,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }
                        else if (Gstr내과_SORT == "3")
                        {
                            SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts3,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " GROUP BY a.emrprts,a.DrDept1,a.emrprts3,a.PrintRanking,a.DrCode,a.DrName,a.YTimeGbn,a.YInwon, ";
                            SQL = SQL + ComNum.VBLF + "   a.TelCNT,a.TotJin,a.AmTime,a.PmTime,a.Bunya,b.GbJin,c.DeptNameK, b.GbJin2 ";
                        }

                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY 1,2,3,4 ";


                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                //Spd.RowCount = 50;

                AllDoctCnt = dt.Rows.Count;

                //클래스 배열 생성
                OWT = new MedOpdNur[0];
                AllDrCode = "";

                if (dt.Rows.Count == 0)
                {
                    Array.Resize<clsOpdNr.MedOpdNur>(ref OWT, OWT.Length + 1);
                    OWT[i] = new clsOpdNr.MedOpdNur(); //초기화
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //strNewData = (dt.Rows[i]["DrCode"].ToString() + "").Trim();

                        AllDrCode = AllDrCode + "'" + (dt.Rows[i]["DrCode"].ToString() + "").Trim() + "',";

                        Array.Resize<clsOpdNr.MedOpdNur>(ref OWT, OWT.Length + 1);
                        OWT[i] = new clsOpdNr.MedOpdNur(); //초기화

                        OWT[i].DeptCode = (dt.Rows[i]["DrDept1"].ToString() + "").Trim();
                        OWT[i].DrCode = (dt.Rows[i]["DrCode"].ToString() + "").Trim();
                        OWT[i].DrName = (dt.Rows[i]["DrName"].ToString() + "").Trim();
                        OWT[i].YTimeGbn = (dt.Rows[i]["YTimeGbn"].ToString() + "").Trim();

                        OWT[i].GbJin = (dt.Rows[i]["GbJin"].ToString() + "").Trim();
                        OWT[i].GbJin2 = (dt.Rows[i]["GbJin2"].ToString() + "").Trim();

                        if (string.Compare(clsPublic.GstrSysTime, "13:00") > 0)
                        {
                            OWT[i].GbJin = OWT[i].GbJin2;
                        }


                        OWT[i].AmTime = (dt.Rows[i]["AmTime"].ToString() + "").Trim();
                        OWT[i].PmTime = (dt.Rows[i]["PmTime"].ToString() + "").Trim();

                        OWT[i].AmJinYN = "N";
                        if ((dt.Rows[i]["GbJin"].ToString() + "").Trim() == "1") OWT[i].AmJinYN = "Y";
                        OWT[i].PmJinYN = "N";
                        if ((dt.Rows[i]["GbJin2"].ToString() + "").Trim() == "1") OWT[i].AmJinYN = "Y";
                    }
                }
                

                dt.Dispose();
                dt = null;

                if (AllDrCode != "")
                {
                    AllDrCode = VB.Left(AllDrCode, VB.Len(AllDrCode) - 1);
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        
        /// <summary>
        /// EMRPRT_INI 파일 로드
        /// </summary>
        public static void READ_EMRPRT_INI()
        {
            string strREC;

            //
            GbPrtYN = false;
            gstrFluPrt = "";

            if (System.IO.File.Exists(EMRPRT_path))
            {
                StreamReader sr = new StreamReader(EMRPRT_path, System.Text.Encoding.Default);

                while ((strREC = sr.ReadLine()) != null)
                {
                    if (strREC.StartsWith("의사:"))
                    {
                        GstrEmrDoct = strREC.Substring("의사:".Length, strREC.Length - "의사:".Length);

                    }
                    else if (strREC.StartsWith("인쇄:"))
                    {
                        GbPrtYN = strREC.Substring("인쇄:".Length, strREC.Length - "인쇄:".Length) == "Y" ? true : false;

                    }
                    else if (strREC.StartsWith("XRAY:"))
                    {
                        GstrXrayDoct = strREC.Substring("XRAY:".Length, strREC.Length - "XRAY:".Length);

                    }
                    else if (strREC.StartsWith("신종플루:"))
                    {
                        gstrFluPrt = strREC.Substring("신종플루:".Length, strREC.Length - "신종플루:".Length);
                    }
                    
                }

                sr.Close();

                if (!System.IO.File.Exists(EMRPRT_path1))
                {
                    System.IO.File.Copy(EMRPRT_path1, EMRPRT_path2, false);
                }


            }
            else
            {
                MessageBox.Show(EMRPRT_path + "설정파일 없음");
            }


            //
            GstrEmrViewDoct = "";
            if (System.IO.File.Exists(EMRPRT_path1))
            {
                StreamReader sr = new StreamReader(EMRPRT_path1, System.Text.Encoding.Default);

                while ((strREC = sr.ReadLine()) != null)
                {
                    if (strREC.StartsWith("의사:"))
                    {
                        GstrEmrViewDoct = strREC.Substring("의사:".Length, strREC.Length - "의사:".Length);
                    }

                }

                sr.Close();


            }

            //
            Gstr내과_SORT = "";
            if (System.IO.File.Exists(EMRPRT_path2))
            {
                StreamReader sr = new StreamReader(EMRPRT_path2, System.Text.Encoding.Default);

                while ((strREC = sr.ReadLine()) != null)
                {
                    if (strREC.StartsWith("내과2:"))
                    {
                        Gstr내과_SORT = strREC.Substring("내과2:".Length, strREC.Length - "내과2:".Length);
                    }
                    else if (strREC.StartsWith("내과3:"))
                    {
                        Gstr내과_SORT = strREC.Substring("내과3:".Length, strREC.Length - "내과3:".Length);
                    }

                }

                sr.Close();

            }


        }

        #endregion


        // 특정진료과 체크
        public static bool chkDeptYN(string[] strAllowDept)
        {
            bool rtnVal = false;
            string[] strSetDept = AllDeptCode.Replace("'", "").Split(',');

            foreach (string s1 in strAllowDept)
            {
                foreach (string s2 in strSetDept)
                {
                    if (s1 == s2) rtnVal = true;
                }
            }

            return rtnVal;
        }



        /// <summary>
        /// 동명이인 체크
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public static DataTable getSameNameJupSu(PsmhDb pDbCon, string strDrCode)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SNAME, COUNT(SName)CNT ";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "     AND DEPTCODE NOT IN('ER', 'HR', 'TO', 'R6', 'HD') ";
                SQL = SQL + ComNum.VBLF + "     AND DRCODE IN(" + strDrCode + ") ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SNAME ";
                SQL = SQL + ComNum.VBLF + " HAVING COUNT(SName) > 1 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
        

        // EMRNO 생성
        public static double GetSequencesNo(PsmhDb pDbCon, string FunSeqName)
        {
            double rtnVal = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT " + FunSeqName + "() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
        
        // 과초진 체크
        public static string OPD_GwaChojin_Check(PsmhDb pDbCon, string argPano, string argDept, string argDrCode, string argDate)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE FROM " + ComNum.DB_PMPA + "BAS_LASTEXAM  ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO='" + argPano + "'  ";
            SQL = SQL + ComNum.VBLF + "  AND DEPTCODE NOT IN ('HR','TO','R6','II','AN','PT') ";
            SQL = SQL + ComNum.VBLF + "  AND LASTDATE<TO_DATE('" + argDate + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "  AND NOT (PANO>='04600000' AND LASTDATE=TO_DATE('1995-01-01','YYYY-MM-DD'))";
            SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (dt == null)
            {
                rtnVal = "1";
            }
            if (dt.Rows.Count == 0)
            {
                if (string.Compare(argPano, "05000000") < 0)
                {
                    rtnVal = "5";       // 구환
                }
                else
                {
                    rtnVal = "1";       // 신환
                }                
            }
            else
            {
                rtnVal = "5";       // 구환
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        // 외래접수테이블 바이탈 업데이트
        public static bool updateVital_OpdDeptJepsu(PsmhDb pDbCon, string strROWID)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = " UPDATE " + ComNum.DB_PMPA + "OPD_DEPTJEPSU SET ";
                SQL = SQL + ComNum.VBLF + " VITAL ='Y' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        // 외래접수 테이블 미시행 업데이트
        public static bool updateNoExe_OpdDeptJepsu(PsmhDb pDbCon, string strROWID, string strValue)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = " UPDATE " + ComNum.DB_PMPA + "OPD_DEPTJEPSU SET ";
                SQL = SQL + ComNum.VBLF + " GB_NOEXE ='" + strValue + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        // 진료접수증, 과접수에 UPDATE
        public static bool UPDATE_OPD_WORK_DEPTJEPSU(PsmhDb pDbCon, string argPano, string argDept, string argDrCode, string argDate, string strResult)
        {
            bool rtVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                //진료접수증 업데이트
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE  " + ComNum.DB_PMPA + "OPD_WORK SET EmrSingu='" + strResult + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate=TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + argDept + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                //과접수 업데이트
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_DEPTJEPSU SET EmrSingu='" + strResult + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ActDate=TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + argDept + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                //clsDB.setRollbackTran(pDbCon);  // TEST
                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        // 환자 참고사항
        public static bool READ_DEPT_MEMO(PsmhDb pDbCon, string strPANO, string strDeptCode, ref string strMemo)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Memo,ROWID, MEMO2 FROM KOSMOS_OCS.OCS_MEMO ";
            SQL = SQL + ComNum.VBLF + " WHERE PtNo='" + strPANO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND DeptCode='" + strDeptCode + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {                
                strMemo = dt.Rows[0]["Memo"].ToString().Trim();
                rtnVal = true;
            }            
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        // 타과메모
        public static bool READ_T_DEPT_MEMO(PsmhDb pDbCon, string strPANO, string strDeptCode, ref string strDeptCode2, ref string strMemo)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strDeptCode2 = "";
            strMemo = "";

            //'기존에 자료가 있으면 읽음
            SQL = "SELECT Memo,DeptCode2,ROWID FROM " + ComNum.DB_MED + "OCS_MEMO2 ";
            SQL = SQL + ComNum.VBLF + "WHERE PtNo='" + strPANO + "' ";
            SQL = SQL + ComNum.VBLF + "  AND DeptCode='" + strDeptCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (dt.Rows.Count > 0)
            {
                strDeptCode2 = dt.Rows[0]["DeptCode2"].ToString().Trim();
                strMemo = dt.Rows[0]["Memo"].ToString().Trim();
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

            dt.Dispose();
            dt = null;
            
            return rtnVal;
        }

        // 후불수납
        public static string READ_A_SUNAP_CHK(PsmhDb pDbCon, string ArgPano, string ArgBi, string ArgDept, string strBDate = "")
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                SQL += ComNum.VBLF + "  WHERE Pano ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND (DELDATE ='' OR DELDATE IS NULL) ";
                SQL += ComNum.VBLF + "    AND GUBUN ='1'";
                SQL += ComNum.VBLF + "  ORDER BY SDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (strBDate.Equals(""))
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
                        {
                            rtnVal = "OK";
                        }
                        else if (string.Compare(strBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                        {
                            rtnVal = "OK";
                        }
                        else
                        {
                            rtnVal = "";
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (ArgBi != "")
                {
                    if (ArgBi.Equals("21") || ArgBi.Equals("22") || ArgBi.Equals("31") || ArgBi.Equals("32") || ArgBi.Equals("33") || ArgBi.Equals("52") || ArgBi.Equals("55"))
                    {
                        rtnVal = "";
                    }
                }

                if (ArgDept != "")
                {
                    if (ArgDept.Equals("HD"))
                    {
                        rtnVal = "";
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        // 낙상
        public static void READ_FALL(PsmhDb pDbCon, string strPANO, ref bool chkFall, ref string strText)
        {            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            chkFall = false;
            strText = "";

            try
            {
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_FALL_SCALE_OPD ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    chkFall = true;
                }

                SQL = " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, WARDCODE ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strText = dt.Rows[0]["ACTDATE"].ToString().Trim() + "(" + dt.Rows[0]["WARDCODE"].ToString().Trim() + ")";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
            }
        }
        
        // 도착시간 세팅
        public static string JEPSU_SeqRTime_NS_SET()
        {
            string strYTIME = "";

            if (Convert.ToDateTime(clsPublic.GstrSysTime) >= Convert.ToDateTime("12:00") && Convert.ToDateTime(clsPublic.GstrSysTime) <= Convert.ToDateTime("13:59"))
            {
                strYTIME = "14:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("08:50") && Convert.ToDateTime(clsOpdNr.OWT[0].AmTime) == Convert.ToDateTime("09:00"))
            {
                strYTIME = "09:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("09:20") && Convert.ToDateTime(clsOpdNr.OWT[0].AmTime) <= Convert.ToDateTime("09:30"))
            {
                strYTIME = "09:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) >= Convert.ToDateTime("12:00") && Convert.ToDateTime(clsPublic.GstrSysTime) <= Convert.ToDateTime(clsOpdNr.OWT[0].AmTime))
            {
                strYTIME = VB.Left(clsOpdNr.OWT[0].AmTime, 4) + "1";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("09:50"))
            {
                strYTIME = "10:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("10:20"))
            {
                strYTIME = "10:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("10:50"))
            {
                strYTIME = "11:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("11:20"))
            {
                strYTIME = "11:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("12:00"))
            {
                strYTIME = "11:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("13:50"))
            {
                strYTIME = "14:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("14:20"))
            {
                strYTIME = "14:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("14:50"))
            {
                strYTIME = "15:01";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("15:20"))
            {
                strYTIME = "15:31";
            }
            else if (Convert.ToDateTime(clsPublic.GstrSysTime) < Convert.ToDateTime("15:50"))
            {
                strYTIME = "16:01";
            }
            else
            {
                strYTIME = "16:32";
            }
            
            return strYTIME;
        }

        // 2018-02-24 
        // 접수 의사 대기환자중 마지막환자의 도착시간을 디폴트로 설정해달고 외래에서 요청함.        
        public static string JEPSU_SeqRTime__SET_NEW(PsmhDb pDbCon, string argDeptCode, string argDrCode)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";                                
                SQL += ComNum.VBLF + " SELECT                                                      ";
                SQL += ComNum.VBLF + "     MAX(A.SEQ_RTIME) AS SEQ_RTIME                           ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU A, KOSMOS_PMPA.OPD_MASTER B  ";
                SQL += ComNum.VBLF + " WHERE A.ACTDATE = TRUNC(SYSDATE)                            ";
                SQL += ComNum.VBLF + "    AND A.PANO = B.PANO(+)                                   ";
                SQL += ComNum.VBLF + "   AND A.ACTDATE = B.ACTDATE(+)                              ";
                SQL += ComNum.VBLF + "   AND A.DEPTCODE = B.DEPTCODE(+)                            ";
                SQL += ComNum.VBLF + "   AND A.DRCODE IN ('" + argDrCode + "')                     ";
                SQL += ComNum.VBLF + "   AND A.DEPTJTIME IS NOT NULL                               ";
                if (argDeptCode != "OS" && argDeptCode != "NS")
                {
                    SQL += ComNum.VBLF + "   AND A.GB_CALL IS NULL-- 부재중 제외                   ";
                }
                SQL += ComNum.VBLF + "   AND A.JINTIME IS NULL                                     ";
                SQL += ComNum.VBLF + "   AND B.SNAME IS NOT NULL                                   ";
                SQL += ComNum.VBLF + "   AND(A.SEQ_NO IS NULL OR A.SEQ_NO < 'B')                   ";
                SQL += ComNum.VBLF + "   AND B.SNAME IS NOT NULL                                   ";
                SQL += ComNum.VBLF + "   AND(B.GBFLU <> 'Y' OR B.GBFLU IS NULL)                    ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SEQ_RTIME"].ToString().Trim();                    
                }
                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장            
            }

            return rtnVal;
        }

        // 암표지자검사
        public static bool READ_CHK_CANCER(PsmhDb pDbCon, string strPANO, string argDate)
        {
            bool rtnVal = false;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strMsg = "";
            string strROWID = "";
            string strChk = "";

            DateTime date1 = Convert.ToDateTime(argDate).AddDays(-120);
            DateTime date2 = Convert.ToDateTime(argDate);

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {                                
                SQL = " SELECT ROWID,CHK FROM " + ComNum.DB_PMPA + "ETC_EXAM_CHK";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE =TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND PANO ='" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='01' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    if (dt.Rows[0]["CHK"].ToString().Trim() == "Y")
                    {
                        strChk = "OK";
                    }
                }
                dt.Dispose();
                dt = null;

                if (strChk != "OK")
                {
                    SQL = "  SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,TO_CHAR(RECEIVEDATE,'YYYY-MM-DD') SEEKDATE,TO_CHAR(RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                    SQL = SQL + ComNum.VBLF + "   PANO,SNAME,SEX,DRCODE,'' AS GBSUNAP, '' AS SUCODE,SPECNO  ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "EXAM_SPECMST ";
                    SQL = SQL + ComNum.VBLF + "  WHERE  (PANO,ORDERNO) IN ( SELECT PTNO,ORDERNO   FROM " + ComNum.DB_MED + "OCS_IORDER    ";
                    SQL = SQL + ComNum.VBLF + "                              WHERE  BDATE >=TO_DATE('" + date1.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                               AND  BDATE <=TO_DATE('" + date2.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                               AND TRIM(SUCODE) IN ( SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE  WHERE GUBUN ='ETC_암표지자검사수가' )";
                    SQL = SQL + ComNum.VBLF + "                           ) ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE >=TO_DATE('" + date1.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE <=TO_DATE('" + date2.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND PANO ='" + strPANO + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SQL = " SELECT R.STATUS,R.MASTERCODE,R.SUBCODE, R.RESULT, R.REFER, R.PANIC, R.IMGWRTNO,   R.DELTA, R.UNIT, R.SEQNO, M.EXAMNAME,";
                            SQL = SQL + ComNum.VBLF + " TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') RESULTDATE  ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "EXAM_RESULTC R, " + ComNum.DB_MED + "EXAM_MASTER M";
                            SQL = SQL + ComNum.VBLF + "   WHERE SPECNO='" + dt.Rows[i]["SPECNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND R.SUBCODE = M.MASTERCODE(+) ORDER BY R.SEQNO";
                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                if (dt2.Rows[0]["Refer"].ToString().Trim() != "")
                                {
                                    //'저장
                                    if (strROWID == "")
                                    {
                                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "ETC_EXAM_CHK ( ";
                                        SQL = SQL + ComNum.VBLF + " PANO,BDATE,REMARK,GUBUN,ENTDATE,ENTPART,CHK ";
                                        SQL = SQL + ComNum.VBLF + " ) VALUES ( ";
                                        SQL = SQL + ComNum.VBLF + " '" + strPANO + "', TRUNC(SYSDATE),'암표지가검사','01',SYSDATE, " + clsType.User.Sabun + ",''  )";
                                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                        if (SqlErr != "")
                                        {
                                            clsDB.setRollbackTran(pDbCon);
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                            Cursor.Current = Cursors.Default;
                                            return rtnVal;
                                        }

                                        strMsg = "암표지자 검사명 : " + dt2.Rows[0]["ExamName"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + "검사결과 : " + dt2.Rows[0]["Result"].ToString().Trim();
                                        ComFunc.MsgBox(strMsg, "암표지가 이상결과확인");                                        
                                    }
                                    else
                                    {
                                        strMsg = "암표지자 검사명 : " + dt2.Rows[0]["ExamName"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + "검사결과 : " + dt2.Rows[0]["Result"].ToString().Trim();
                                        strMsg = strMsg + ComNum.VBLF + ComNum.VBLF + "해당 결과 팝업 정보를 그만 보시겠습니까?";
                                        if (ComFunc.MsgBoxQ(strMsg, "암표지가 이상결과 확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            if (strROWID != "")
                                            {
                                                SQL = " UPDATE " + ComNum.DB_PMPA + "ETC_EXAM_CHK SET CHK ='Y' ";
                                                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                                                SQL = SQL + ComNum.VBLF + "  AND PANO ='" + strPANO + "' ";
                                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                                if (SqlErr != "")
                                                {
                                                    clsDB.setRollbackTran(pDbCon);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                                    Cursor.Current = Cursors.Default;
                                                    return rtnVal;
                                                }
                                            }
                                        }                                            
                                    }
                                    break;                                    
                                }
                            }

                            dt2.Dispose();
                            dt2 = null;                            
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        
        public static int Read_Wait_JinTime(PsmhDb pDbCon, int argIndex, string argDrCode)
        {
            int rtnVal = 0;
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double nAM = 0;
            double nPM = 0;
            string strAMSTIME = "";
            string strPMSTIME = "";
            double nMINADD = 0;

            try
            {
                //'기본 5분으로 설정
                rtnVal = argIndex * 5;
                nAM = 5;
                nPM = 5;

                //'평균대기시간 읽기
                SQL = "";
                SQL = "SELECT  AMJIN, PMJIN , AMJINSTIME, PMJINSTIME";
                SQL = SQL + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + "  WHERE  DRCODE ='" + argDrCode + "' ";                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    nAM = VB.Val(dt.Rows[0]["AmJin"].ToString().Trim());
                    nPM = VB.Val(dt.Rows[0]["PmJin"].ToString().Trim());

                    strAMSTIME = dt.Rows[0]["AmJinsTime"].ToString().Trim();
                    if (strAMSTIME == "") strAMSTIME = "0930";
                    
                    strPMSTIME = dt.Rows[0]["PmJinsTime"].ToString().Trim();
                    if (strPMSTIME == "") strPMSTIME = "1330";
                }

                dt.Dispose();
                dt = null;


                if (nAM == 0) nAM = 5;
                if (nPM == 0) nPM = 5;

                SQL = "SELECT  TO_CHAR(SYSDATE,'PM') AMPM, TO_CHAR(SYSDATE,'HH24MI') TIME   FROM DUAL ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows[0]["AMPM"].ToString().Trim() == "AM")
                {
                    nMINADD = 0;
                    if (string.Compare(strAMSTIME, dt.Rows[0]["Time"].ToString().Trim()) > 0)
                    {
                        nMINADD = Convert.ToDouble(ComFunc.TimeDiffMin(clsPublic.GstrSysDate + " " + VB.Left(dt.Rows[0]["Time"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[0]["Time"].ToString().Trim(), 2), clsPublic.GstrSysDate + " " + VB.Left(strAMSTIME, 2) + ":" + VB.Right(strAMSTIME, 2)));
                    }

                    rtnVal = Convert.ToInt32(argIndex * nAM);
                    rtnVal = Convert.ToInt32(rtnVal + nMINADD);
                }
                else
                {
                    nMINADD = 0;
                    if (string.Compare(dt.Rows[0]["Time"].ToString().Trim(), "1230") > 0 && string.Compare(strPMSTIME, dt.Rows[0]["Time"].ToString().Trim()) > 0)
                    {
                        nMINADD = Convert.ToDouble(ComFunc.TimeDiffMin(clsPublic.GstrSysDate + " " + VB.Left(dt.Rows[0]["Time"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[0]["Time"].ToString().Trim(), 2), clsPublic.GstrSysDate + " " + VB.Left(strPMSTIME, 2) + ":" + VB.Right(strPMSTIME, 2)));
                    }
                    
                    rtnVal = Convert.ToInt32(argIndex * nPM);
                    rtnVal = Convert.ToInt32(rtnVal + nMINADD);
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                return rtnVal;
            }
        }

        // 처음 2명은 순서가 변동되지 안게 고정하는 작업
        public static bool Fix_SeqNo_UpDate(PsmhDb pDbCon)
        {
            bool rtnVal = false;
            int i=0;            
            int nSeqNo = 0;
            string strOldData = "";
            string strNewData = "";
            //string strROWID = "";
            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {                
                SQL = "";
                SQL = "SELECT  /*+LEADING(A)*/  A.PANO,B.SNAME,A.DEPTCODE,A.DRCODE,A.GUBUN,A.RTIME,A.SEQ_RTIME,A.SEQ_NO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.JEPTIME,'YYYY-MM-DD HH24:MI') JTIME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DEPTJTIME,'YYYY-MM-DD HH24:MI') DEPTJTIME,";
                SQL = SQL + ComNum.VBLF + " A.ROWID,B.OCSJIN,C.REMARK,A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM OPD_DEPTJEPSU A,OPD_MASTER B," + ComNum.DB_MED + "OCS_OLOCK C ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACTDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DRCODE IN (" + AllDrCode + ") ";
                SQL = SQL + ComNum.VBLF + "  AND A.JINTIME IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND (A.SEQ_NO IS NULL OR A.SEQ_NO < 'B') ";
                SQL = SQL + ComNum.VBLF + "  AND A.DEPTJTIME IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO=B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE=B.ACTDATE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE=B.DEPTCODE(+) ";
                SQL = SQL + ComNum.VBLF + "  AND B.SNAME IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "  AND (B.OCSJIN IS NULL OR B.OCSJIN<>'*') ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO=C.PTNO(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.DEPTCODE,A.DRCODE,A.SEQ_NO,A.SEQ_RTIME,A.DEPTJTIME,B.SNAME ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNewData = dt.Rows[i]["DeptCode"].ToString().Trim() + dt.Rows[i]["DrCode"].ToString().Trim();
                        if (strOldData != strNewData)
                        {
                            strOldData = strNewData;
                            nSeqNo = 0;
                        }

                        nSeqNo = nSeqNo + 1;
                        if (nSeqNo <= 2)
                        {
                            SQL = "UPDATE OPD_DEPTJEPSU SET SEQ_NO='" + VB.Format(nSeqNo, "0") + "' ";
                            SQL = SQL + "WHERE ROWID='" + dt.Rows[i]["ROWID"].ToString().Trim()  + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }                        
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        
        // 미확인 약품회신
        public static string READ_HOIMST_COUNT(PsmhDb pDbCon, string argIO, string argTemp)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = SQL + " SELECT BUN, TO_CHAR(BDATE, 'YYYY-MM-DD HH24:MI') BDATE,   ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(JDATE, 'YYYY-MM-DD HH24:MI') JDATE, TO_CHAR(HDATE, 'YYYY-MM-DD HH24:MI') HDATE,  ";
                SQL = SQL + ComNum.VBLF + " IPDOPD, DEPTCODE, WARDCODE, ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " DRCODE , PANO, DRSABUN, DRUGGIST, WRTNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_HOIMST MST ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE >= TRUNC(SYSDATE - 10) ";

                if (argTemp != "전체")
                {
                    if (argIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DEPTCODE IN (" + argTemp + ")";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND DEPTCODE ='" + argTemp + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + " AND IPDOPD = 'O'";
                }

                SQL = SQL + ComNum.VBLF + "  AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + "    (SELECT * FROM " + ComNum.DB_ERP + "DRUG_HOIMST_CONFIRM SUB";
                SQL = SQL + ComNum.VBLF + "      WHERE MST.WRTNO = SUB.WRTNO)";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = "미확인 약품회신:0건";
                }
                else
                {
                    rtnVal = "미확인 약품회신:" + dt.Rows.Count + "건";
                }
                
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }

            
        }
        
        public static bool NEW_TextEMR_TRANSFOR(PsmhDb pDbCon, string strPatid, string strBDate, string strDeptCode, string strDeptCode2, string strDrCode, string strDrCode2)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJumin = "";   //주민암호화
            //string strOK = "";
            string strDrCode2Sabun = "";
            string strDept = "";

            strBDate = VB.Format(strBDate, "YYYYMMDD");

            try
            {
                SQL = "SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1 ,P.JUMIN2,  P.JUMIN3, E.PATID , E.ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT  P , " + ComNum.DB_EMR + "EMR_PATIENTT E";
                SQL = SQL + ComNum.VBLF + "WHERE E.PATID (+)=P.PANO";
                SQL = SQL + ComNum.VBLF + "AND P.PANO ='" + strPatid.ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows[0]["PATID"].ToString().Trim() == "")    //EMR_PATIENTT 테이블에 환자가 없다.
                {
                    strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL = "INSERT INTO " + ComNum.DB_EMR + "EMR_PATIENTT(PATID, JUMINNO, NAME, SEX) ";
                    SQL = SQL + ComNum.VBLF + "VALUES('" + dt.Rows[0]["Pano"].ToString().Trim() + ", ";
                    SQL = SQL + ComNum.VBLF + "'" + strJumin + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + dt.Rows[0]["sName"].ToString().Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + dt.Rows[0]["Sex"].ToString().Trim() + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {
                    strJumin = dt.Rows[i]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL = "UPDATE " + ComNum.DB_EMR + "EMR_PATIENTT";
                    SQL = SQL + ComNum.VBLF + "SET NAME ='" + dt.Rows[i]["sName"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ", SEX  ='" + dt.Rows[i]["Sex"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + ", JUMINNO ='" + strJumin + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                dt.Dispose();
                dt = null;

                // 입원
                SQL = "SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,D.SABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ipd_new_master S , " + ComNum.DB_MED + "ocs_doctor d";
                SQL = SQL + ComNum.VBLF + "WHERE S.DrCode = d.drcode";
                SQL = SQL + ComNum.VBLF + "AND S.PANO = '" + strPatid + "'";
                SQL = SQL + ComNum.VBLF + "AND TRUNC(S.InDate) = TO_DATE('" + strBDate + "', 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "AND S.EMR ='1'"; //처리된것만 전실전과처리

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "SELECT TREATNO, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                SQL = SQL + ComNum.VBLF + "WHERE PATID = '" + strPatid + "'";
                SQL = SQL + ComNum.VBLF + "AND INDATE  ='" + strBDate + "'";
                SQL = SQL + ComNum.VBLF + "AND CLINCODE = '" + strDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "AND CLASS = 'I'";

                SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    // 의사코드를 사번 읽기
                    SQL = "SELECT SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + "WHERE DRCODE ='" + strDrCode2 + "'"; //바뀔의사코드를 사번으로 읽음

                    SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    strDrCode2 = "";
                    if (dt.Rows.Count > 0)
                    {
                        strDrCode2Sabun = dt3.Rows[0]["SABUN"].ToString().Trim();

                        if (strDeptCode == "MD" && dt3.Rows[0]["SABUN"].ToString().Trim() == "19094" || dt.Rows[0]["SABUN"].ToString().Trim() == "30322")
                        {
                            strDept = "RA";
                        }
                        else
                        {
                            strDept = strDeptCode2;
                        }
                    }
                    else
                    {
                        dt3.Dispose();
                        dt3 = null;
                    }
                    if (strDrCode2Sabun != "")
                    {
                        SQL = "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                        SQL = SQL + ComNum.VBLF + "DOCCODE = '" + VB.Val(strDrCode2Sabun) + "', ";
                        SQL = SQL + ComNum.VBLF + "ClinCode = '" + strDept + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                dt2.Dispose();
                dt2 = null;

                dt.Dispose();
                dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        // SMS동의여부
        public static string Read_BasPatient_SMS(PsmhDb pDbCon, string strPano)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT GBSMS ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["GBSMS"].ToString().Trim();
                }

                if (rtnVal == "")
                {
                    rtnVal = "N";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        // 당일 SMS동의서 출력여부
        public static string READ_SMS_ARGREE_CHK(PsmhDb pDbCon, string strPano, string strDate, string strFormNo)
        {
            string rtnVal = "출력없음";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";                
                SQL = " SELECT PTNO,MEDDEPTCD ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_AGREE_PRINT ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND FORMNO ='" + strFormNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE ='" + VB.Trim(VB.Replace(strDate, "-", "")) + "' ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["MEDDEPTCD"].ToString().Trim() + "출력함";
                }
                
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static bool DeleteEtcReturn(PsmhDb pDbCon, string strRowId)
        {
            bool rtVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "DELETE KOSMOS_PMPA.ETC_RETURN ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strRowId + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }
        
        //외래 해피콜 조회
        public static bool READ_OPD_HAPPYCALL(PsmhDb pDbCon, string strGubun, string strPano, string strDept, string strBDate)
        {
            bool rtnval = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID,CONTEXT,GUBUN2 FROM KOSMOS_PMPA.NUR_HAPPYCALL_OPD ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN2 > ' '";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnval;
                }
                
                if (dt.Rows.Count > 0)
                {
                    rtnval = true;
                }

                dt.Dispose();
                dt = null;

                return rtnval;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnval;
            }
        }
        
        //외래 해피콜 저장    
        public static bool INSERT_HappyCall_OPD(PsmhDb pDbCon, string strGubun, string strPano, string strRet, string strDept, string strBDate)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.NUR_HAPPYCALL_OPD ( ";
                SQL = SQL + ComNum.VBLF + " GUBUN, PANO, WRITEDATE, WRITESABUN,  ";
                SQL = SQL + ComNum.VBLF + " GUBUN2, DEPTCODE, BDATE ) VALUES ( ";
                SQL = SQL + ComNum.VBLF + " '" + strGubun + "','" + strPano + "', SYSDATE, " + clsType.User.Sabun + ",";
                SQL = SQL + ComNum.VBLF + " '" + strRet + "','" + strDept + "',TO_DATE('" + strBDate + "','YYYY-MM-DD') ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Happy Call 내역이 저장되었습니다.");
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }            
        }

        //외래 해피콜 갱신
        public static bool UPDATE_HappyCall_OPD(PsmhDb pDbCon, string strGubun, string strPano, string strRet, string strDept, string strBDate)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.NUR_HAPPYCALL_OPD SET ";
                SQL = SQL + ComNum.VBLF + "    GUBUN2 ='" + strRet + "' ,";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE = SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "    WRITESABUN = " + clsType.User.Sabun + "  ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN ='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Happy Call 내역이 수정되었습니다.");
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        /// <summary>
        /// 사망체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano"></param>
        /// <returns></returns>
        public static bool READ_PATIENT_DIE(PsmhDb pDbCon, string strPano)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID FROM MID_SUMMARY ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND TMODEL ='5' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return rtnVal;
        }

        /// <summary>
        /// Description : 해당 월(MM) 계산 ex)201708 -> 201707 or 201709
        /// Author : 안정수
        /// Create Date : 2017.09.07
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argADD"></param>
        /// <seealso cref="VBFunction.bas : DATE_YYMM_ADD"/>
        public static string DATE_YYMM_ADD(string ArgYYMM, int argADD)
        {
            string rtnVal = "";

            //string SQL = "";
            //string SqlErr = "";
            //DataTable dt = null;

            int ArgI = 0;
            int ArgJ = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            if (ArgYYMM.Length != 6 || argADD == 0)
            {
                return ArgYYMM;
            }

            ArgYY = Convert.ToInt32(VB.Left(ArgYYMM, 4));
            ArgMM = Convert.ToInt32(VB.Right(ArgYYMM, 2));

            ArgJ = argADD;

            if (ArgJ < 0)
            {
                ArgJ = ArgJ * -1;
            }

            for (ArgI = 1; ArgI <= ArgJ; ArgI++)
            {
                if (argADD < 0)
                {
                    ArgMM -= 1;
                    if (ArgMM == 0)
                    {
                        ArgMM = 12;
                        ArgYY -= 1;
                    }
                }
                else
                {
                    ArgMM += 1;
                    if (ArgMM == 13)
                    {
                        ArgYY += 1;
                        ArgMM = 1;
                    }
                }

            }

            rtnVal = ComFunc.SetAutoZero(ArgYY.ToString(), 4) + ComFunc.SetAutoZero(ArgMM.ToString(), 2);
            return rtnVal;
        }

        /// <summary>
        /// Create : 2018-01-12
        /// Author : 안정수
        /// <seealso cref="vbfunc.bas : ComboYear_Set"/>        
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCombo"></param>
        /// <param name="ArgMonthCNT"></param>
        /// <param name="argGBDis"></param>
        public static void ComboYear_Set(PsmhDb pDbCon, ComboBox ArgCombo, int ArgMonthCNT, string argGBDis = "")
        {
            int i = 0;
            //int j = 0;
            int ArgYY = 0;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            ArgYY = Convert.ToInt32(VB.Left(CurrentDate, 4));

            ArgCombo.Items.Clear();

            for (i = 0; i < ArgMonthCNT; i++)
            {
                if (argGBDis == "1")
                {
                    ArgCombo.Items.Add(ComFunc.SetAutoZero(ArgYY.ToString(), 4));
                }

                else
                {
                    ArgCombo.Items.Add(ComFunc.SetAutoZero(ArgYY.ToString(), 4) + "년도");
                }

                ArgYY -= 1;
            }
            ArgCombo.SelectedIndex = 0;
        }

        public static string READ_BUSECODE(PsmhDb pDbCon, string argValue)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Code cCode FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND Name = '" + argValue + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {                        
                    rtnVal = dt.Rows[i]["cCode"].ToString().Trim();                 
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            return rtnVal;
        }

        public static void NUR_LOCK(PsmhDb pDbCon, string strArg1, string strArg2, string strArg3, string strArg4)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //'작업일자(yyyy-mm-dd)작업병동          프로그램구분        설명
            string strWard     = "";
            string strGubun    = "";
            string strName     = "";
            string strJob = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                strArg4 = VB.Left(strArg4, 30);
                GstrLock = "F";

                if (VB.Len(strArg1) == 0)
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                    
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  WARD,NAME,GUBUN,JOBCOMMENT,LWARD FROM NUR_LOCK ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strArg1 + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN= '" + strArg3 + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARD = '" + strArg2 + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO NUR_LOCK (ACTDATE, WARD, GUBUN, JOBCOMMENT, NAME, LWARD)";
                    SQL = SQL + ComNum.VBLF + " VALUES(TO_DATE('" + strArg1 + "','YYYY-MM-DD') , '" + strArg2 + "',";
                    SQL = SQL + ComNum.VBLF + "        '" + strArg3 + "' , '" + strArg4 + "',";
                    SQL = SQL + ComNum.VBLF + "        '" + clsType.User.UserName + "', '" + clsPublic.GstrWardCode + "') ";
                    

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    strWard = dt.Rows[0]["lward"].ToString().Trim();
                    strGubun = dt.Rows[0]["GUBUN"].ToString().Trim();
                    strJob = dt.Rows[0]["Jobcomment"].ToString().Trim();
                    strName = dt.Rows[0]["Name"].ToString().Trim();
                    //if (strWard != clsPublic.GstrWardCode)
                    if (strName.Trim() != clsType.User.UserName.Trim())
                    {
                        ComFunc.MsgBox(strName + "님이 " + strJob + "중입니다." + ComNum.VBLF + " 잠시후에 작업을 하십시오");
                        GstrLock = "T";
                    }
                    else
                    {
                        //'Result=dosql("UPDATE NUR_LOCK SET NAME = '"+ GstrPassName +"' WHERE
                    }
                }
                
                clsDB.setCommitTran(pDbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// 간호사이름
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public static string NURSE_NAME_READ(PsmhDb pDbCon, string argSabun)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + " SELECT KORNAME FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN IN ('" + argSabun.PadLeft(5, '0') + "') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KorName"].ToString().Trim();

                }
                else
                {
                    SQL = "";
                    SQL = SQL + " SELECT Name FROM " + ComNum.DB_PMPA + "NUR_CODE  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE CODE = '" + argSabun + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (dt.Rows.Count > 0)
                    {
                        rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = "";
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 인사마스터 이름조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public static string READ_INSA_NAME(PsmhDb pDbCon, string argSabun)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + " SELECT KORNAME FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN IN ('" + argSabun.PadLeft(5, '0') + "') ";
                SQL = SQL + ComNum.VBLF + "    AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KorName"].ToString().Trim();

                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_BAS_SR_Name(PsmhDb pDbCon, string strPano)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string rtnVal = "";

            string strJumin1 = "";
            string strJumin2 = "";
            string strJumin3 = "";
            string strSex = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JUMIN1,JUMIN2,JUMIN3,SEX";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin3 = dt.Rows[0]["JUMIN3"].ToString().Trim();

                    if (strJumin3 != "")
                    {
                        strJumin2 = clsAES.DeAES(strJumin3);
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    strSex = dt.Rows[0]["SEX"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     GAMJUMIN, GAMSABUN, GAMGUBUN, GAMENTER, GAMOUT, GAMEND, GAMMESSAGE, GAMNAME, GAMSOSOK, GAMCODE, GAMJUMIN3, Pano ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_GAMF ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (GAMJUMIN ='" + strJumin1 + strJumin2 + "'  OR GAMJUMIN ='" + clsAES.AES(strJumin1 + strJumin2) + "' ) ";
                    SQL = SQL + ComNum.VBLF + "         AND GamCode IN ('11','12') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        switch (strSex)
                        {
                            case "M":
                                if (VB.I(dt1.Rows[0]["GAMMESSAGE"].ToString().Trim(), "신부") > 1 || VB.I(dt1.Rows[0]["GAMNAME"].ToString().Trim(), "신부") > 1)
                                {
                                    rtnVal = "Fr";
                                }
                                break;
                            case "F":
                                if (VB.I(dt1.Rows[0]["GAMMESSAGE"].ToString().Trim(), "수녀") > 1 || VB.I(dt1.Rows[0]["GAMNAME"].ToString().Trim(), "수녀") > 1)
                                {
                                    rtnVal = "Sr";
                                }
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }
        
        /// <summary>
        /// Description : 부서명을 읽어온다.
        /// Author : 유진호
        /// Create Date : 2018.02.10
        /// <param name="strCode"></param>
        /// </summary>
        public static string Read_BuseName(PsmhDb pDbCon, string strCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Name Sname ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL += ComNum.VBLF + "  WHERE Gubun = '2' ";
                SQL += ComNum.VBLF + "    AND Code = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count > 0)
                    strVal = DtFunc.Rows[0]["Sname"].ToString().Trim();
                else
                    strVal = "";

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strVal;
            }
        }

        /// <summary>
        /// 예진표등록여부
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgDeptCode"></param>
        /// <returns></returns>
        public static string Read_MedicalInquiry_Chk(PsmhDb pDbCon, string ArgPano, string ArgBDate, string ArgDeptCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ArgDeptCode == "MR")
                {
                    SQL = " SELECT ROWID,MEDDEPTCD DEPTCODE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.EMRXMLMST ";
                    SQL += ComNum.VBLF + "  WHERE FORMNO = 2355 ";
                    SQL += ComNum.VBLF + "    AND CHARTDATE = '" + VB.Replace(ArgBDate, "-", "") + "' ";
                    SQL += ComNum.VBLF + "    AND PTNO ='" + ArgPano + "' ";
                    SQL += ComNum.VBLF + "    AND ROWNUM = 1 ";

                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + " SELECT ROWID,MEDDEPTCD DEPTCODE ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRCHARTMST ";
                    SQL += ComNum.VBLF + "  WHERE FORMNO = 2355 ";
                    SQL += ComNum.VBLF + "    AND CHARTDATE = '" + VB.Replace(ArgBDate, "-", "") + "' ";
                    SQL += ComNum.VBLF + "    AND PTNO ='" + ArgPano + "' ";
                    SQL += ComNum.VBLF + "    AND ROWNUM = 1 ";
                }
                else if(ArgDeptCode == "IC")
                {
                    SQL = " SELECT ROWID    ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN_CORONA19 ";
                    SQL += ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                    SQL += ComNum.VBLF + "  AND DEPTCODE ='" + ArgDeptCode + "' ";
                    SQL += ComNum.VBLF + "  AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";
                }
                else
                {
                    SQL = " SELECT ROWID    ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPT_MUNJIN ";
                    SQL += ComNum.VBLF + " WHERE PANO ='" + ArgPano + "' ";
                    SQL += ComNum.VBLF + "  AND DEPTCODE ='" + ArgDeptCode + "' ";
                    SQL += ComNum.VBLF + "  AND BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='')";
                }
                
                clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }
                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        /// <summary>
        /// 중증,산정특례여부 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public static bool READ_BAS_CANCER(PsmhDb pDbCon, string ArgPano)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_CANCER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";                
                SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate ='')";
                SQL = SQL + ComNum.VBLF + "  AND FDATE<=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND TDATE>=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
                
                clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        public static bool UPDATE_OPD_WORK(PsmhDb pDbCon, string strROWID)
        {
            bool rtVal = false;            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE OPD_WORK SET  EMRPRT = '2' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
                                
                clsDB.setCommitTran(pDbCon);                
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        public static bool UPDATE_OPD_MASTER_EXAM(PsmhDb pDbCon, string strPano, string strDept, string strDrcd, string strDate, string strResult, string strROWID)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE OPD_WORK SET  EMRPRT = '2' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                SQL = "UPDATE KOSMOS_PMPA.OPD_MASTER SET EXAM ='" + (strResult == "True" ? "Y" : "N") + "'  ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE =TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + strDrcd + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        /// <summary>
        /// 사용자 옵션 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strUseId"></param>
        /// <param name="strOPTCD"></param>
        /// <param name="strOPTGB"></param>
        /// <returns></returns>
        public static string GetUserOption(PsmhDb pDbCon, string strUseId, string strOPTCD, string strOPTGB)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT OPTVALUE FROM " + ComNum.DB_EMR + "EMRUSEROPTION ";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + strUseId + "'";
                SQL = SQL + ComNum.VBLF + "    AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "    AND OPTGB = '" + strOPTGB + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["OPTVALUE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }            
        }

    }
}
