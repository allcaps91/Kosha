using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// Description : 선택진료 관련 
/// Author : 김민철
/// Create Date : 2017.08.09
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public class clsPmpaSel : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //ComFunc CF = new ComFunc();
        //clsPmpaFunc cPF = new clsPmpaFunc();

        /// <summary>
        /// Description : 선택의사인지 점검
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDrCode">의사코드</param>
        /// <param name="ArgBDate">기준일자</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Read_SELECT_DOCTOR_CHK"/>
        /// <returns>OK or Null</returns>
        /// <history>ComFunc 에 먼저 정의되어 있어 주석처리 함 2017.09.18 KMC</history>
        //public string Read_Select_Doctor_Chk(string ArgIO, string ArgDrCode, string ArgBDate)
        //{
        //    string rtnVal = "";

        //    DataTable Dt = new DataTable();
        //    string SQL = string.Empty;
        //    string SqlErr = string.Empty;

        //    if (ArgBDate.Equals(""))
        //    {
        //        ArgBDate = clsPublic.GstrSysDate;
        //    }

        //    //1. 외래 산부인과 김도균과장 토요일 선택진료 안물림
        //    //2. 토요일 근무하는 진료과장이 1명일경우 선택진료비 물리면 안됨.
        //    
        //    if (ArgIO.Equals("O") && ArgDrCode.Equals("3111") && clsVbfunc.GetYoIl(ArgBDate).Equals("토요일"))
        //    {
        //        return rtnVal;
        //    }

        //    try
        //    {
        //        //선택진료 기본마스터 정보 읽기
        //        SQL = "";
        //        SQL += ComNum.VBLF + " SELECT ROWID  FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
        //        SQL += ComNum.VBLF + "  WHERE DRCODE ='" + ArgDrCode + "' ";
        //        SQL += ComNum.VBLF + "    AND GBCHOICE ='Y' ";
        //        SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
        //            rtnVal = "";
        //            return rtnVal;
        //        }

        //        if (Dt.Rows.Count > 0)
        //        {
        //            rtnVal = "OK";
        //        }

        //        Dt.Dispose();
        //        Dt = null;

        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return "";
        //    }
        //}

        /// <summary>
        /// Description : RD과 선택진료 정보
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIO">입원,외래</param>
        /// <param name="ArgBDate">기준일자</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Read_RD_SEL_Chk"/>
        /// <returns>OK or Null</returns>
        public string Read_RD_Sel_CHK(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgBDate)
        {
            string rtnVal = "";

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            
            try
            {
                //선택진료 기본마스터 정보 읽기
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano, DrCode, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST";
                SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE ='RD' ";
                SQL += ComNum.VBLF + "    AND GUBUN ='" + ArgIO + "' ";
                SQL += ComNum.VBLF + "    AND SDate <=TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
                SQL += ComNum.VBLF + "  ORDER BY SDate DESC     ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
                    {
                        clsPmpaType.SCP.RD_DrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                        rtnVal = "Y";
                    }
                    else if (string.Compare(ArgBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                    {
                        clsPmpaType.SCP.RD_DrCode = Dt.Rows[0]["DrCode"].ToString().Trim();
                        rtnVal = "Y";
                    }
                    else
                    {
                        clsPmpaType.SCP.RD_DrCode = "";
                        rtnVal = "";
                    }
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        /// <summary>
        /// Description : 선택진료 대상자인지 점검
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDrCode">의사코드</param>
        /// <param name="ArgBDate">기준일자</param>
        /// <param name="ArgIpdNo">입원번호</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Read_Pano_SELECT_MST"/>
        /// <returns>OK or Null</returns>
        /// <history>ComFunc 에 먼저 정의되어 있어 주서처리 함. 2017.09.18 KMC</history>
        //public string Read_Pano_Select_Mst(string ArgPano, string ArgIO, string ArgDrCode, string ArgBDate, long ArgIpdNo)
        //{
        //    string rtnVal = "";

        //    DataTable Dt = new DataTable();
        //    string SQL = string.Empty;
        //    string SqlErr = string.Empty;

        //    clsIument clsIU = new clsIument();

        //    if (ArgIO.Equals("I"))
        //    {
        //        if (clsIU.Is_Ipd_New_Master_Jewon(ArgIpdNo) != true)
        //        {
        //            return "";
        //        }
        //    }
            
        //    try
        //    {
        //        //선택진료 기본마스터 정보 읽기
        //        SQL = "";
        //        SQL += ComNum.VBLF + " SELECT Pano, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
        //        SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
        //        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST";
        //        SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
        //        SQL += ComNum.VBLF + "    AND DRCODE ='" + ArgDrCode + "' ";
        //        SQL += ComNum.VBLF + "    AND GUBUN ='" + ArgIO + "' ";
        //        SQL += ComNum.VBLF + "    AND SDate <=TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
        //        SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
        //        SQL += ComNum.VBLF + "  ORDER BY SDate DESC     ";
        //        SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
        //            rtnVal = "";
        //            return rtnVal;
        //        }

        //        if (Dt.Rows.Count > 0)
        //        {
        //            if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
        //            {
        //                rtnVal = "OK";
        //            }
        //            else if (string.Compare(ArgBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
        //            {
        //                rtnVal = "OK";
        //            }
        //            else
        //            {
        //                rtnVal = "";
        //            }
        //        }

        //        Dt.Dispose();
        //        Dt = null;

        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return "";
        //    }
        //}

        /// <summary>
        /// Description : 선택진료 대상자 시작일자 가져오기
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDrCode">의사코드</param>
        /// <param name="ArgBDate">기준일자</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Read_Pano_SELECT_MST_BDate"/>
        /// <returns>OK or Null</returns>
        /// <history> ComFunc 에 먼저 정의되어 주석처리함 2017.09.18</history>
        //public string Read_Pano_Select_Mst_BDate(string strPano, string strIO, string strDrCode, string strBDate)
        //{
        //    string rtnVal = "";

        //    DataTable Dt = new DataTable();
        //    string SQL = string.Empty;
        //    string SqlErr = ""; //에러문 받는 변수

        //    try
        //    {
        //        SQL = "";
        //        SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
        //        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
        //        SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
        //        SQL += ComNum.VBLF + "    AND PANO = '" + strPano.Trim() + "' ";
        //        SQL += ComNum.VBLF + "    AND DRCODE ='" + strDrCode + "' ";
        //        SQL += ComNum.VBLF + "    AND GUBUN ='" + strIO + "' ";
        //        SQL += ComNum.VBLF + "    AND SDate <=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
        //        SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
        //        SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";

        //        SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return "";
        //        }

        //        if (Dt.Rows.Count == 0)
        //        {
        //            Dt.Dispose();
        //            Dt = null;
        //            return "";
        //        }
        //        if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
        //        {
        //            rtnVal = Dt.Rows[0]["SDATE"].ToString().Trim();
        //        }
        //        else if (string.Compare(strBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
        //        {
        //            rtnVal = Dt.Rows[0]["SDATE"].ToString().Trim();
        //        }

        //        Dt.Dispose();
        //        Dt = null;

        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return "";
        //    }
        //}

        /// <summary>
        /// Description : 선택진료 항목 및 요율 가져오기
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDrCode">의사코드</param>
        /// <param name="ArgBDate">기준일자</param>
        /// <param name="ArgIpdNo">입원번호</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Read_Pano_SELECT_MST_SET"/>
        /// <returns>OK or Null</returns>
        public string Read_Pano_Select_Mst_Set(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgDrCode, string ArgBDate, long ArgIpdNo)
        {
            string rtnVal = "";

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            clsPmpaType.SEL_SET.GbSet1 = "Y";
            clsPmpaType.SEL_SET.GbSet2 = "Y";
            clsPmpaType.SEL_SET.GbSet3 = "Y";
            clsPmpaType.SEL_SET.GbSet4 = "Y";
            clsPmpaType.SEL_SET.GbSet5 = "Y";
            clsPmpaType.SEL_SET.GbSet6 = "Y";
            clsPmpaType.SEL_SET.GbSet7 = "Y";
            clsPmpaType.SEL_SET.GbSet8 = "Y";
            clsPmpaType.SEL_SET.GbSet9 = "Y";
            clsPmpaType.SEL_SET.GbSet_Current = "Y";  //기본Y설정

            //선택진료 기본마스터 정보 읽기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano,Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,";
            SQL += ComNum.VBLF + "        TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
            SQL += ComNum.VBLF + "   FROM ADMIN.BAS_SELECT_MST";
            SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
            SQL += ComNum.VBLF + "    AND DRCODE ='" + ArgDrCode + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN ='" + ArgIO + "' ";
            SQL += ComNum.VBLF + "    AND SDate <=TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC     ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
                {
                    rtnVal = "OK";
                }
                else if (string.Compare(ArgBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                {
                    rtnVal = "OK";
                }
                else
                {
                    rtnVal = "";
                }
            }

            if (rtnVal.Equals("OK"))
            {
                clsPmpaType.SEL_SET.GbSet1 = Dt.Rows[0]["Set1"].ToString().Trim();        //진찰
                clsPmpaType.SEL_SET.GbSet2 = Dt.Rows[0]["Set2"].ToString().Trim();        //검사
                clsPmpaType.SEL_SET.GbSet3 = Dt.Rows[0]["Set3"].ToString().Trim();        //영상진단
                clsPmpaType.SEL_SET.GbSet4 = Dt.Rows[0]["Set4"].ToString().Trim();        //방사선치료
                clsPmpaType.SEL_SET.GbSet5 = Dt.Rows[0]["Set5"].ToString().Trim();        //방사선혈관촬영
                clsPmpaType.SEL_SET.GbSet6 = Dt.Rows[0]["Set6"].ToString().Trim();        //마취
                clsPmpaType.SEL_SET.GbSet7 = Dt.Rows[0]["Set7"].ToString().Trim();        //정신요법
                clsPmpaType.SEL_SET.GbSet8 = Dt.Rows[0]["Set8"].ToString().Trim();        //처치수술
                clsPmpaType.SEL_SET.GbSet9 = Dt.Rows[0]["Set9"].ToString().Trim();        //침 부항

                //영상진단,혈관촬영,마취만 체크함 
                switch (clsPmpaType.SEL.Suga_GbSelect)
                {
                    case "1":   //진찰
                        break;
                    case "2":   //입원료
                        break;
                    case "3":   //검사
                        break;
                    case "4":   //진찰
                        if (clsPmpaPb.GstrSuWonCode == "1447")      //혈괄촬영만
                        {
                            clsPmpaType.SEL_SET.GbSet_Current = clsPmpaType.SEL_SET.GbSet5;  //혈관촬영
                        }
                        else
                        {
                            clsPmpaType.SEL_SET.GbSet_Current = clsPmpaType.SEL_SET.GbSet3;  //영상진단
                        }
                        break;
                    case "5":   //마취
                        clsPmpaType.SEL_SET.GbSet_Current = clsPmpaType.SEL_SET.GbSet6;     //마취
                        break;
                    case "6":   //정신요법
                        break;
                    case "7":   //처치수술
                        break;
                    default:
                        break;
                }
            }
            
            Dt.Dispose();
            Dt = null;

            return rtnVal;
           
        }

        /// <summary>
        /// Description : 선택진료 정보 INSERT
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgSName">환자명</param>
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDeptCode">진료과</param>
        /// <param name="ArgDrCode">진료의사</param>
        /// <param name="ArgSDate">시작일자</param>
        /// <param name="ArgEDate">종료일자</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas  Pano_SELECT_INSERT"/>
        /// <returns></returns>
        public string Pano_Select_Insert(PsmhDb pDbCon, string ArgPano, string ArgSName, string ArgIO, string ArgDeptCode, string ArgDrCode, string ArgSDate, string ArgEDate)
        {
            string istrDelDate = string.Empty;
            string istrROWID = string.Empty;

            string rtnVal = "";

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            

            //선택의사체크
            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE DRCODE ='" + ArgDrCode + "' ";
            SQL += ComNum.VBLF + "    AND GBCHOICE ='Y' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                return rtnVal;
            }

            Dt.Dispose();
            Dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
            SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE ='" + ArgDeptCode + "' ";
            SQL += ComNum.VBLF + "    AND DRCODE ='" + ArgDrCode + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN ='" + ArgIO + "' ";
            SQL += ComNum.VBLF + "    AND ( DelDate IS NULL OR DelDate ='' )  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                istrROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;


            if (istrROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_MST ( ";
                SQL += ComNum.VBLF + "        PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,ENTDATE2,";
                SQL += ComNum.VBLF + "        Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9 ) ";
                SQL += ComNum.VBLF + " VALUES ( ";
                SQL += ComNum.VBLF + "        '" + ArgPano + "','" + ArgSName + "','" + ArgIO + "','" + ArgDeptCode + "','" + ArgDrCode + "',";
                SQL += ComNum.VBLF + "        TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),'',TO_DATE('" + istrDelDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "',SYSDATE,SYSDATE ,";
                SQL += ComNum.VBLF + "        'Y','Y','Y','Y','Y','Y','Y','Y','Y'   ) ";  //기본저장은 Y 설정
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return "";
                }

                rtnVal = "OK";
            }
            else
            {
                //현재 신규등록만 사용중
                //이전내역 백업
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_MST_HIS ( ";
                SQL += ComNum.VBLF + "        PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,BIGO ,";
                SQL += ComNum.VBLF + "        Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 ) ";
                SQL += ComNum.VBLF + " SELECT PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,BIGO, ";
                SQL += ComNum.VBLF + "        Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
                SQL += ComNum.VBLF + "  WHERE ROWID ='" + istrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return "";
                }


                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
                SQL += ComNum.VBLF + "    SET EDATE =TO_DATE('" + ArgEDate + "','YYYY-MM-DD') ,  ";
                SQL += ComNum.VBLF + "        DELDATE =TO_DATE('" + istrDelDate + "','YYYY-MM-DD') ,  ";
                SQL += ComNum.VBLF + "        ENTSABUN ='" + clsType.User.IdNumber + "', ";
                SQL += ComNum.VBLF + "        ENTDATE2 = SYSDATE ";
                SQL += ComNum.VBLF + " WHERE ROWID ='" + istrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return "";
                }

                rtnVal = "OK";

            }

            clsDB.setCommitTran(pDbCon);

            return rtnVal;
        }

        /// <summary>
        /// Description : 선택진료 신규등록 가능한지 체크
        /// Author : 김민철
        /// Create Date : 2017.09.04
        /// <param name="ArgPano">등록번호</param>
        /// <param name="ArgIO">외래,입원</param>
        /// <param name="ArgDrCode">진료의사코드</param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas : Read_SELECT_INSERT_CHK"/>
        public string Read_Select_Insert_Chk(PsmhDb pDbCon, string ArgPano, string ArgIO, string ArgDrCode)
        {
            string rtnVal = string.Empty;
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            rtnVal = "OK";

            //선택진료 기본마스터 정보 읽기 - 신규저장가능체크
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
            SQL += ComNum.VBLF + "  WHERE PANO ='" + ArgPano + "' ";
            SQL += ComNum.VBLF + "    AND DRCODE ='" + ArgDrCode + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN ='" + ArgIO + "' ";
            SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = "";
            }

            Dt.Dispose();
            Dt = null;
            
            return rtnVal;
        }

        /// <summary>
        /// Description : 선택진료의사 해당일자 스케쥴 체크
        /// Author : 김민철
        /// Create Date : 2017.09.18
        /// <param name="ArgIO"></param>
        /// <param name="ArgDrCode"></param>
        /// <param name="ArgBDate"></param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas : READ_SELECT_DOCTOR_SCH_CHK"/>
        public string Read_Select_Doctor_Sch_Chk(PsmhDb pDbCon, string ArgIO, string ArgDrCode, string ArgBDate)
        {
            ComFunc CF = new ComFunc();

            string rtnVal = string.Empty;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //입원아니면 제외
            if (ArgIO != "I")
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //휴일이면 제외
            if (CF.DATE_HUIL_CHECK(pDbCon, ArgBDate))
            {
                rtnVal = "OK";
                return rtnVal;
            }

            //스케쥴 읽어 5.학회,6.휴가,7.출장,8.기타,9.반off 이면 제외시킴
            //1,2,3,4 한개라도 있으면 진료로 봄
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DrCode,GbJin,GbJin2,GbJin3 ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL += ComNum.VBLF + " WHERE DrCode='" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "   AND SCHDate = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "   AND ( GbJin IN ('1','2','3','4') OR GbJin2 IN ('1','2','3','4') )  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

        }

        /// <summary>
        /// Description : 선택진료 가능 환자자격 체크
        /// Author : 김민철
        /// Create Date : 2017.09.18        
        /// <param name="ArgBi"></param>
        /// <param name="ArgGDate"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgIPDNO"></param>
        /// <param name="ArgDrCode"></param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas : READ_SELECT_BI_CHK"/>
        public string Read_Select_Bi_Chk(PsmhDb pDbCon, string ArgBi, string ArgGDate, string ArgIO, long ArgIPDNO, [Optional] string ArgDrCode)
        {
            string rtnVal = string.Empty;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN = '선택진료_BI' ";
                SQL += ComNum.VBLF + "   AND Code= '" + ArgBi + "' ";
                SQL += ComNum.VBLF + "   AND JDate <= TO_DATE('" + ArgGDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='') ";
                SQL += ComNum.VBLF + " ORDER BY JDate DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }

                rtnVal = "OK";
                if (Dt.Rows.Count > 0)
                {
                    rtnVal = "";
                }

                Dt.Dispose();
                Dt = null;
    
                //2016-05-01
                if ((ArgBi == "31" || ArgBi == "33" || ArgBi == "52" || ArgBi == "55") && ArgIO == "I" && string.Compare(ArgGDate, "2016-05-01") >= 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT TO_CHAR(InDate,'YYYY-MM-DD') INDATE ";
                    SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + " Where IPDNO =" + ArgIPDNO + " ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        rtnVal = "";
                        return rtnVal;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (string.Compare(Dt.Rows[0]["INDATE"].ToString().Trim(), "2016-05-01") < 0)
                        {
                            rtnVal = "";
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
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

        /// <summary>
        /// Description : 선택진료 환자중 감액코드가 제외 대상인지 체크
        /// Author : 김민철
        /// Create Date : 2017.09.18        
        /// <param name="ArgGam"></param>
        /// <param name="ArgGDate"></param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas : READ_SELECT_Gam_CHK"/>
        public string Read_Select_Gam_Chk(PsmhDb pDbCon, string ArgGam, string ArgGDate)
        {
            string rtnVal = string.Empty;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN = '선택진료_Gam' ";
                SQL += ComNum.VBLF + "   AND TRIM(Code)='" + ArgGam + "' ";
                SQL += ComNum.VBLF + "   AND JDate <= TO_DATE('" + ArgGDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='' ) ";
                SQL += ComNum.VBLF + " ORDER BY JDate DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }
                rtnVal = "OK";
                if (Dt.Rows.Count > 0)
                {
                    rtnVal = "";
                }
                Dt.Dispose();
                Dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        /// <summary>
        /// Description : 선택진료금액 수가항목정보 읽음
        /// Author : 김민철
        /// Create Date : 2017.09.18      
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgSuNext"></param>
        /// </summary>
        /// <seealso cref="Vb선택진료.bas : Suga_Read_Select_Gbn"/>
        public string Suga_Read_Select_Gbn(PsmhDb pDbCon, string ArgSuCode, string ArgSuNext)
        {
            string rtnVal = string.Empty;
            DataTable Dt = new DataTable();
            DataTable Dt2 = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            rtnVal = "0";

            clsPmpaPb.GstrSuBun = "";
            clsPmpaPb.GstrSuWonCode = "";
            clsPmpaPb.GstrSuDaiCode = "";
            clsPmpaPb.GstrSugbP = ""; 
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Bun,b.GbSelect,b.WonCode,b.DaiCode,b.sugbp  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT a,          ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b           ";
                SQL += ComNum.VBLF + "  WHERE TRIM(a.Sucode) = '" + ArgSuCode + "'      ";
                SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+)                    ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["GbSelect"].ToString().Trim();
                    clsPmpaPb.GstrSuBun = Dt.Rows[0]["Bun"].ToString().Trim(); 
                    clsPmpaPb.GstrSuWonCode = Dt.Rows[0]["WonCode"].ToString().Trim(); 
                    clsPmpaPb.GstrSuDaiCode = Dt.Rows[0]["DaiCode"].ToString().Trim();
                    clsPmpaPb.GstrSugbP = Dt.Rows[0]["sugbp"].ToString().Trim();
                }
                else
                { 
                    //2011-10-10
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT a.Bun,b.GbSelect,b.WonCode,b.DaiCode ,b.sugbp ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUH a ,     ";
                    SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b       ";
                    SQL += ComNum.VBLF + "  WHERE TRIM(a.SuNext) = '" + ArgSuNext + "'  ";
                    SQL += ComNum.VBLF + "    AND a.SuNext = b.SuNext(+)                ";
                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        rtnVal = "";
                        return rtnVal;
                    }
                    if (Dt2.Rows.Count > 0)
                    {
                        rtnVal = Dt2.Rows[0]["GbSelect"].ToString().Trim(); 
                        clsPmpaPb.GstrSuBun = Dt2.Rows[0]["Bun"].ToString().Trim(); 
                        clsPmpaPb.GstrSuWonCode = Dt2.Rows[0]["WonCode"].ToString().Trim(); 
                        clsPmpaPb.GstrSuDaiCode = Dt2.Rows[0]["DaiCode"].ToString().Trim();
                        clsPmpaPb.GstrSugbP = Dt.Rows[0]["sugbp"].ToString().Trim();
                    }
                    Dt2.Dispose();
                    Dt2 = null;
                }
                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
        }

        /// <summary>
        /// Description : 선택진료 금액산정 Main Routine
        /// Author : 김민철
        /// Create Date : 2017.09.18
        /// </summary>
        /// <param name="SMM">선택진료 금액 변수 구조체</param>
        /// <seealso cref="Vb선택진료.bas : READ_SELECT_MAIN"/>
        public string Read_Select_Main(PsmhDb pDbCon, clsPmpaType.Sel_Main_MST SMM)
        {
            clsPmpaQuery cPQ = new clsPmpaQuery();

            string rtnVal = string.Empty;
            string strOK = string.Empty;

            clsPmpaPb.GstrSuBun = "";               //수가분류
            clsPmpaPb.GstrSuWonCode = "";           //수가 원가코드
            clsPmpaPb.GstrSuDaiCode = "";           //약품 분류 코드
            clsPmpaType.SEL.Suga_GbSelect = "";     //수가의 선택진료구분

            //선택진료체크-외래만
            if (SMM.ArgIO.Equals("O") && SMM.ArgSpc != "1")
            {
                return rtnVal;                      //선택진료 ArgSpc 사용안함
            }

            //2011-06-01시행
            if (string.Compare(SMM.ArgBDate, "2011-06-01") < 0) { return rtnVal; }

            //예외사항점검 - 예외코드시 처리사항
            if (SMM.ArgETC != "")
            {
                if (SMM.ArgETC == "9")
                {
                    strOK = "OK";
                }
                else if (SMM.ArgETC == "0")
                {
                    strOK = "";
                }
                else
                {
                    return rtnVal;
                }
            }

            //개인별 변경사항체크 - Read_SELECT_CHANGE_CLEAR clear모듈포함
            if (clsPmpaType.SCP.GbIO != SMM.ArgIO || clsPmpaType.SCP.pano != SMM.ArgPano || clsPmpaType.SCP.BDate != SMM.ArgBDate || clsPmpaType.SCP.Bi != SMM.ArgBi || clsPmpaType.SCP.DeptCode != SMM.ArgDeptCode || clsPmpaType.SCP.DrCode != SMM.ArgDrCode || clsPmpaType.SCP.GAMEK != SMM.ArgGamek)
            {
                //변경사항있으면
                clsPmpaType.SCP.GbChange = "Y";
                clsPmpaType.SCP.GbIO = SMM.ArgIO;
                clsPmpaType.SCP.pano = SMM.ArgPano;
                clsPmpaType.SCP.BDate = SMM.ArgBDate;
                clsPmpaType.SCP.Bi = SMM.ArgBi;
                clsPmpaType.SCP.GAMEK = SMM.ArgGamek;
                clsPmpaType.SCP.DeptCode = SMM.ArgDeptCode;
                clsPmpaType.SCP.DrCode = SMM.ArgDrCode;
            }
            else
            {
                //변경사항없으면
                clsPmpaType.SCP.GbChange = "N";
            }

            //2014-04-28 RD과 선택진료 여부 확인
            clsPmpaType.SCP.GbRD = Read_RD_Sel_CHK(pDbCon, SMM.ArgPano, SMM.ArgIO, SMM.ArgBDate);

            //2015-02-26 RD과 선택진료 구분
            if (SMM.ArgDeptCode == "RD")
            {
                if (clsPmpaType.SCP.GbRD != "Y") { return rtnVal; }
            }

            if (strOK != "OK")
            { 
                //입원일경우
                if (SMM.ArgIO == "I")
                {
                    if (ComFunc.READ_IPD_NEW_MASTER_INDATE_CHK(pDbCon, SMM.ArgIPDNO) != "OK") { return rtnVal; }                //입원일자 2011-06-01 이후건만 해당됨
                    if (Read_Select_Doctor_Sch_Chk(pDbCon, SMM.ArgIO, SMM.ArgDrCode, SMM.ArgBDate) != "OK") { return rtnVal; }  //의사스케쥴 점검(BAS_SCHEDULE)
                }

                //선택진료 환자정보 체크,보험종류체크,감액체크
                if (ComFunc.Read_Pano_SELECT_MST(pDbCon, SMM.ArgPano, SMM.ArgIO, SMM.ArgDrCode, SMM.ArgBDate, SMM.ArgIPDNO) != "OK") { return rtnVal; }
                
                if (Read_Select_Bi_Chk(pDbCon, SMM.ArgBi, SMM.ArgBDate, SMM.ArgIO, SMM.ArgIPDNO, SMM.ArgDrCode) != "OK") { return rtnVal; }
                if (Read_Select_Gam_Chk(pDbCon, SMM.ArgGamek, SMM.ArgBDate) != "OK") { return rtnVal; }

            }
            
            if (clsPmpaType.SCP.GbRD == "Y" && SMM.ArgDeptCode == "RD")
            {
                if (Read_Select_Bi_Chk(pDbCon, SMM.ArgBi, SMM.ArgBDate, SMM.ArgIO, SMM.ArgIPDNO, SMM.ArgDrCode) != "OK") { return rtnVal; }
                if (Read_Select_Gam_Chk(pDbCon, SMM.ArgGamek, SMM.ArgBDate) != "OK") { return rtnVal; }
            }

            //선택진료 외래,입원 해당항목 및 부담율 체크(BAS_SELECT_SET)
            READ_SELECT_RATE_CHK(pDbCon, SMM.ArgDeptCode, SMM.ArgBDate, SMM.ArgDrCode);

            //수가코드 읽어 수가항목 선택항목 읽음
            clsPmpaType.SEL.Suga_GbSelect = Suga_Read_Select_Gbn(pDbCon, SMM.ArgSuCode, SMM.argSUNEXT);

            //선택진료 개인별 진료지원항목 읽기 2011-10-01 시행
            Read_Pano_Select_Mst_Set(pDbCon, SMM.ArgPano, SMM.ArgIO, SMM.ArgDrCode, SMM.ArgBDate, SMM.ArgIPDNO);
            
            if (string.Compare(SMM.ArgBDate, "2016-05-01") < 0)
            { 
                //자보 52,55 선택일부제한(해당=진찰,방사선(angio만),마취,수술만) 2011-07-19 처치제외
                if ((SMM.ArgBi.Equals("52") || SMM.ArgBi.Equals("55")) && (clsPmpaType.SEL.Suga_GbSelect == "2" || clsPmpaType.SEL.Suga_GbSelect == "3" || clsPmpaType.SEL.Suga_GbSelect == "4" || clsPmpaType.SEL.Suga_GbSelect == "7"))
                {
                    if (clsPmpaType.SEL.Suga_GbSelect == "4" && clsPmpaPb.GstrSuWonCode == "1447")
                    {
                        //방사선(angio만)
                    }
                    else if (clsPmpaType.SEL.Suga_GbSelect == "7" && clsPmpaPb.GstrSuBun == "34")
                    { 
                        //수술만
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                }
            }
            else
            {
                //자보/산재 선택진료 부분적용 (진찰료, 마취, 수술, 혈관조영만)
                if (SMM.ArgBi.Equals("31") || SMM.ArgBi.Equals("33") || SMM.ArgBi.Equals("52") || SMM.ArgBi.Equals("55"))
                {
                    if (clsPmpaType.SEL.Suga_GbSelect == "1" || clsPmpaType.SEL.Suga_GbSelect == "2" || clsPmpaType.SEL.Suga_GbSelect == "4" || clsPmpaType.SEL.Suga_GbSelect == "5" || clsPmpaType.SEL.Suga_GbSelect == "7")
                    { 
                        if (clsPmpaType.SEL.Suga_GbSelect == "4" && clsPmpaPb.GstrSuWonCode != "1447")
                        {   //방사선(angio만)
                            rtnVal = "";
                            return rtnVal;
                        }
                        else if (clsPmpaType.SEL.Suga_GbSelect == "7" && clsPmpaPb.GstrSuBun != "34")  //2011-08-18
                        { 
                            //수술만
                            rtnVal = "";
                            return rtnVal;
                        }
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                }
            }
            
            if (string.Compare(clsPmpaType.SEL.Suga_GbSelect, "0")  > 0)
            {
                switch (SMM.ArgIO)
                {
                    case "I":
                        switch (clsPmpaType.SEL.Suga_GbSelect)
                        {
                            case "1": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Jin_Rate;   break;
                            case "2": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Med_Rate;   break;
                            case "3": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Gum_Rate;   break;
                            case "4": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Xray_Rate;  break;
                            case "5": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Mach_Rate;  break;
                            case "6": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Psy_Rate;   break;
                            case "7": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.IPD_Op_Rate;    break;
                            default:  clsPmpaType.SEL.Current_Rate = 0; break;
                        }
                        break;
                    case "O":
                        switch (clsPmpaType.SEL.Suga_GbSelect)
                        {
                            case "1": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Jin_Rate;  break;
                            case "2": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Med_Rate;  break;
                            case "3": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Gum_Rate;  break;
                            case "4": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Xray_Rate; break;
                            case "5": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Mach_Rate; break;
                            case "6": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Psy_Rate;  break;
                            case "7": clsPmpaType.SEL.Current_Rate = clsPmpaType.SEL.OPD_Op_Rate;   break;
                            default:  clsPmpaType.SEL.Current_Rate = 0; break;
                        }
                        break;
                    default:
                        clsPmpaType.SEL.Current_Rate = 0;
                        break;
                }
            }

            //수가금액 체크(BAS_SUGA_AMT-selamt) 위의 수가 항목이 부담율 있을경우
            clsPmpaPb.GnSelAmt = 0; //선택금액  clear
                
            //영상의학과 선택진료비 산정
            if (clsPmpaType.SCP.GbRD == "Y" && SMM.ArgDeptCode == "RD")
            { 
                if (clsPmpaPb.GstrSuBun == "34" || (clsPmpaPb.GstrSuBun == "65" && clsPmpaPb.GstrSuWonCode == "1447"))
                {
                    //설정값없으면 100% 읽음
                    if (clsPmpaType.SEL.Current_Rate == 0) { clsPmpaType.SEL.Current_Rate = 100; }

                    cPQ.Read_Suga_Amt(pDbCon, SMM.ArgSuCode, SMM.argSUNEXT, SMM.ArgBDate);

                    if (clsPmpaPb.GnSelAmt == 0)
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "OK";
                        return rtnVal;
                    }
                }
                else
                {
                    rtnVal = "";
                    return rtnVal;
                }
            }

            //2011-10-10 처방입력시 강제선택금액입력일경우 금액산정
            if (strOK == "OK")
            {
                //설정값없으면 100% 읽음
                if (clsPmpaType.SEL.Current_Rate == 0) { clsPmpaType.SEL.Current_Rate = 100; }

                cPQ.Read_Suga_Amt(pDbCon, SMM.ArgSuCode, SMM.argSUNEXT, SMM.ArgBDate);

                if (clsPmpaPb.GnSelAmt == 0)
                {
                    rtnVal = "";
                    return rtnVal;
                }
                else
                {
                    rtnVal = "OK";
                    return rtnVal;
                }
            }

            if (string.Compare(SMM.ArgBDate, "2011-10-01") >= 0) //2011-10-01 부터 진료지원항목 체크포함
            { 
                if (clsPmpaType.SEL.Current_Rate > 0 && clsPmpaType.SEL_SET.GbSet_Current == "Y" && SMM.ArgSuCode != "" && SMM.argSUNEXT != "")
                {
                    cPQ.Read_Suga_Amt(pDbCon, SMM.ArgSuCode, SMM.argSUNEXT, SMM.ArgBDate);
                    if (clsPmpaPb.GnSelAmt == 0)
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                }
                else
                {
                    rtnVal = "";
                    return rtnVal;
                }
            }
            else
            { 
                if (clsPmpaType.SEL.Current_Rate > 0 && SMM.ArgSuCode != "" && SMM.argSUNEXT != "")
                {
                    cPQ.Read_Suga_Amt(pDbCon, SMM.ArgSuCode, SMM.argSUNEXT, SMM.ArgBDate);                    
                    if (clsPmpaPb.GnSelAmt == 0)
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                }
            }

            rtnVal = "OK";

            return rtnVal;
        }



        /// <summary>
        /// Description : 선택진료 구분체크
        /// Author : 박병규
        /// Create Date : 2017.11.03
        /// </summary>
        /// <param name="SMM">선택진료 금액 변수 구조체</param>
        /// <seealso cref="Vb선택진료.bas : Read_Select_Set"/>
        public string READ_SELECT_SET(PsmhDb pDbCon, string ArgDept, string ArgIO)
        {
            DataTable DtSel = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            rtnVal = "OK";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT JDATE, DEPTCODE, ISET0, ";
            SQL += ComNum.VBLF + "        ISET1, ISET2, ISET3, ";
            SQL += ComNum.VBLF + "        ISET4, ISET5, ISET6, ";
            SQL += ComNum.VBLF + "        ISET7, OSET0, OSET1, ";
            SQL += ComNum.VBLF + "        OSET2, OSET3, OSET4, ";
            SQL += ComNum.VBLF + "        OSET5, OSET6, OSET7, ";
            SQL += ComNum.VBLF + "        EntDate, Sabun ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_SELECT_SET ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Deptcode  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtSel, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtSel.Rows.Count > 0)
            {
                if (ArgIO == "O" && DtSel.Rows[0]["OSET0"].ToString().Trim() != "1")
                    rtnVal = "";
                else if (ArgIO == "I" && DtSel.Rows[0]["ISET0"].ToString().Trim() != "1")
                    rtnVal = "";
            }
            else
                rtnVal = "";

            DtSel.Dispose();
            DtSel = null;

            return rtnVal;
        }



        /// <summary>
        /// Description : 선택진료 입원,외래 부담율 읽기
        /// Author : 박병규
        /// Create Date : 2017.08.31
        /// </summary>
        /// <seealso cref="vb선택진료.bas:READ_SELECT_RATE_CHK"/>
        public string READ_SELECT_RATE_CHK(PsmhDb pDbCon, string ArgDeptCode, string ArgBdate, string ArgDrCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strUse = string.Empty;
            string rtnVal = string.Empty;

            clsPmpaType.SEL.Current_Rate = 0;//수가의 내용 읽어 해당율

            clsPmpaType.SEL.OPD_Gb_Select = "";//외래선택진료여부   2014-02-03
            clsPmpaType.SEL.OPD_Jin_Rate = 0;//진찰료
            clsPmpaType.SEL.OPD_Med_Rate = 0;//의학관리료
            clsPmpaType.SEL.OPD_Gum_Rate = 0;//검사료
            clsPmpaType.SEL.OPD_Xray_Rate = 0;//영상진단 및 방사선치료
            clsPmpaType.SEL.OPD_Mach_Rate = 0;//마취료
            clsPmpaType.SEL.OPD_Psy_Rate = 0;//정신요법
            clsPmpaType.SEL.OPD_Op_Rate = 0;//처치수술료

            clsPmpaType.SEL.IPD_Gb_Select = "";//입원선택진료여부   2014-02-03
            clsPmpaType.SEL.IPD_Jin_Rate = 0;//진찰료
            clsPmpaType.SEL.IPD_Med_Rate = 0;//의학관리료
            clsPmpaType.SEL.IPD_Gum_Rate = 0;//검사료
            clsPmpaType.SEL.IPD_Xray_Rate = 0;//영상진단 및 방사선치료
            clsPmpaType.SEL.IPD_Mach_Rate = 0;//마취료
            clsPmpaType.SEL.IPD_Psy_Rate = 0;//정신요법
            clsPmpaType.SEL.IPD_Op_Rate = 0;//처치수술료

            strUse = "Y";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ALL_USE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_DEPT  ";
            SQL += ComNum.VBLF + "  WHERE DeptCode  = '" + ArgDeptCode + "' ";
            SQL += ComNum.VBLF + "    AND JDate     <= TO_DATE('" + ArgBdate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND ALL_USE   = 'N' ";  //의사코드 사용여부
            SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (DtFunc.Rows.Count == 0)
            {
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                strUse = "N";

            DtFunc.Dispose();
            DtFunc = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ISET0, ISET1, ISET2, ";
            SQL += ComNum.VBLF + "        ISET3, ISET4, ISET5, ";
            SQL += ComNum.VBLF + "        ISET6, ISET7, OSET0, ";
            SQL += ComNum.VBLF + "        OSET1, OSET2, OSET3, ";
            SQL += ComNum.VBLF + "        OSET4, OSET5, OSET6, ";
            SQL += ComNum.VBLF + "        OSET7 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_SET  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDeptCode + "' ";
            SQL += ComNum.VBLF + "    AND JDate     <= TO_DATE('" + ArgBdate + "','YYYY-MM-DD')  ";

            if (strUse.Equals("N"))
                SQL += ComNum.VBLF + "   AND DrCode = '" + ArgDrCode + "' ";

            SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (DtFunc.Rows.Count == 0)
            {
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = "OK";

                if (DtFunc.Rows[0]["ISET0"].ToString().Trim() == "1")
                {
                    clsPmpaType.SEL.IPD_Gb_Select = DtFunc.Rows[0]["ISET0"].ToString().Trim();
                    clsPmpaType.SEL.IPD_Jin_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET1"].ToString().Trim()); //진찰료
                    clsPmpaType.SEL.IPD_Med_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET2"].ToString().Trim()); //의학관리료
                    clsPmpaType.SEL.IPD_Gum_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET3"].ToString().Trim()); //검사료 2012-05-25 내시경검사추가
                    clsPmpaType.SEL.IPD_Xray_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET4"].ToString().Trim()); //영상진단 및 방사선치료
                    clsPmpaType.SEL.IPD_Mach_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET5"].ToString().Trim()); //마취료
                    clsPmpaType.SEL.IPD_Psy_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET6"].ToString().Trim()); //정신요법
                    clsPmpaType.SEL.IPD_Op_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET7"].ToString().Trim()); //처치수술료
                }

                if (DtFunc.Rows[0]["OSET0"].ToString().Trim() == "1")
                {
                    clsPmpaType.SEL.OPD_Gb_Select = DtFunc.Rows[0]["OSET0"].ToString().Trim();
                    clsPmpaType.SEL.OPD_Jin_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET1"].ToString().Trim()); //진찰료
                    clsPmpaType.SEL.OPD_Med_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET2"].ToString().Trim()); //의학관리료
                    clsPmpaType.SEL.OPD_Gum_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET3"].ToString().Trim()); //검사료 2012-05-25 내시경검사추가
                    clsPmpaType.SEL.OPD_Xray_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET4"].ToString().Trim()); //영상진단 및 방사선치료
                    clsPmpaType.SEL.OPD_Mach_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET5"].ToString().Trim()); //마취료
                    clsPmpaType.SEL.OPD_Psy_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET6"].ToString().Trim()); //정신요법
                    clsPmpaType.SEL.OPD_Op_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET7"].ToString().Trim()); //처치수술료
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }




    }
}
