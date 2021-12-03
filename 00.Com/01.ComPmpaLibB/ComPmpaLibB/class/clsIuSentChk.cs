using ComLibB;
using System;
using System.Data;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ComPmpaLibB
{
    /// <summary>
    /// 입원로직에서 사용하는 각종 점검용 함수 모음
    /// </summary>
    public class clsIuSentChk
    {
        public static int nX;
        public static int nY;

       

        # region 원내수가 EDI 코드 조회 // RTN_BAS_SUN_BCODE
        public string Rtn_Bas_Sun_BCode(PsmhDb pDbCon, string strCode, string strCurDate)
        {
            string strBCode = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  BCODE,  TO_CHAR(EDIDATE,  'YYYY-MM-DD') EDIDATE, ";                
                SQL = SQL + ComNum.VBLF + "         BCODE3, TO_CHAR(EDIDATE3, 'YYYY-MM-DD') EDIDATE3, ";
                SQL = SQL + ComNum.VBLF + "         BCODE4, TO_CHAR(EDIDATE4, 'YYYY-MM-DD') EDIDATE4, ";
                SQL = SQL + ComNum.VBLF + "         BCODE5, TO_CHAR(EDIDATE5, 'YYYY-MM-DD') EDIDATE5  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + strCode + "'       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                if (string.Compare(strCurDate, Dt.Rows[0]["EDIDATE"].ToString().Trim()) >= 0)
                {
                    strBCode = Dt.Rows[0]["BCODE"].ToString().Trim();
                }
                else if (string.Compare(strCurDate, Dt.Rows[0]["EDIDATE3"].ToString().Trim()) >= 0)
                {
                    strBCode = Dt.Rows[0]["BCODE3"].ToString().Trim();
                }
                else if (string.Compare(strCurDate, Dt.Rows[0]["EDIDATE4"].ToString().Trim()) >= 0)
                {
                    strBCode = Dt.Rows[0]["BCODE4"].ToString().Trim();
                }
                else if (string.Compare(strCurDate, Dt.Rows[0]["EDIDATE5"].ToString().Trim()) >= 0)
                {
                    strBCode = Dt.Rows[0]["BCODE5"].ToString().Trim();
                }
                else
                {
                    strBCode = "";
                }
                
                Dt.Dispose();
                Dt = null;

                return strBCode;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }

        }
        #endregion
        #region 원내수가 EDI 코드 조회 신경외과 가산 항목 SUGBAE
        public string Rtn_Bas_Sun_SUGBAE(PsmhDb pDbCon, string strCode, string strCurDate)
        {
            string strSUGBAE = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  nvl(SUGBAE,'0') SUGBAE ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + strCode + "'       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return "";
                }


                strSUGBAE = Dt.Rows[0]["SUGBAE"].ToString().Trim();
                

                Dt.Dispose();
                Dt = null;

                return strSUGBAE;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }

        }
        #endregion


        public long Rtn_Bas_Sun_GBTAHPSUGA(PsmhDb pDbCon, string strCode, string strDate)
        {
            long nAmt = 0;
            double dQty = 0.0;  //표준계수
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Bamt, TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE, ";
                SQL = SQL + ComNum.VBLF + "        OldBamt, TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3, ";
                SQL = SQL + ComNum.VBLF + "        Bamt3, TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4, ";
                SQL = SQL + ComNum.VBLF + "        Bamt4,Bamt5, TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5 ,SUHAM ";

                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE               ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                        ";
                SQL = SQL + ComNum.VBLF + "    AND Sucode = '" + strCode + "'                     ";
                SQL = SQL + ComNum.VBLF + "    AND GBTAHPSUGA is not null                       ";

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (Dt.Rows.Count > 0)
                {
                    if (string.Compare( Dt.Rows[0]["SUDATE"].ToString().Trim(), strDate) <= 0)
                    {
                        nAmt = (Convert.ToInt64(Dt.Rows[0]["Bamt"].ToString().Trim()));
                    }
                    else if (string.Compare( Dt.Rows[0]["SUDATE3"].ToString().Trim(), strDate) <= 0)
                    {
                        nAmt = (Convert.ToInt64(Dt.Rows[0]["OldBamt"].ToString().Trim()));
                    }
                    else if (string.Compare( Dt.Rows[0]["SUDATE4"].ToString().Trim(), strDate) <= 0)
                    {
                        nAmt = (Convert.ToInt64(Dt.Rows[0]["Bamt3"].ToString().Trim()));
                    }
                    else if (string.Compare( Dt.Rows[0]["SUDATE5"].ToString().Trim(), strDate) <= 0)
                    {
                        nAmt = (Convert.ToInt64(Dt.Rows[0]["Bamt4"].ToString().Trim()));
                    }
                    else
                    {
                        nAmt = (Convert.ToInt64(Dt.Rows[0]["Bamt5"].ToString().Trim()));
                    }

                    dQty = VB.Val(Dt.Rows[0]["SUHAM"].ToString().Trim());
                    if (dQty == 0) { dQty = 1; }

                    nAmt = Convert.ToInt64(nAmt * dQty);

                    Dt.Dispose();
                    Dt = null;

                    return nAmt;
                }
                Dt.Dispose();
                Dt = null;

                return nAmt;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return 0;
            }
        }



        #region 원내수가 EDI 코드 조회 
        public string Rtn_Bas_Sun_SUGBB(PsmhDb pDbCon, string strCode, string strCurDate)
        {
            string strSUGBB = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  SUGBB ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + strCode + "'       ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return "";
                }

                strSUGBB = Dt.Rows[0]["SUGBB"].ToString().Trim();


                Dt.Dispose();
                Dt = null;

                return strSUGBB;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }

        }
        #endregion
        #region EDI 보험수가 조회 // RTN_EDI_SUGA_AMT
        public long Rtn_Edi_Suga_Amt(PsmhDb pDbCon, string strCode, string strDate)
        {
            long nAmt = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PRICE1, TO_CHAR(JDate1,'YYYY-MM-DD') JDATE1, ";
                SQL = SQL + ComNum.VBLF + "        PRICE2, TO_CHAR(JDate2,'YYYY-MM-DD') JDATE2, ";
                SQL = SQL + ComNum.VBLF + "        PRICE3, TO_CHAR(JDate3,'YYYY-MM-DD') JDATE3, ";
                SQL = SQL + ComNum.VBLF + "        PRICE4, TO_CHAR(JDate4,'YYYY-MM-DD') JDATE4, ";
                SQL = SQL + ComNum.VBLF + "        PRICE5, TO_CHAR(JDate5,'YYYY-MM-DD') JDATE5, ";
                SQL = SQL + ComNum.VBLF + "        PRICE6, TO_CHAR(JDate6,'YYYY-MM-DD') JDATE6  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_SUGA               ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                        ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + strCode + "'                     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return 0;
                }

                if (string.Compare(strDate, Dt.Rows[0]["JDate1"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE1"].ToString().Trim()));
                }
                else if (string.Compare(strDate, Dt.Rows[0]["JDate2"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE2"].ToString().Trim()));
                }
                else if (string.Compare(strDate, Dt.Rows[0]["JDate3"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE3"].ToString().Trim()));
                }
                else if (string.Compare(strDate, Dt.Rows[0]["JDate4"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE4"].ToString().Trim()));
                }
                else if (string.Compare(strDate, Dt.Rows[0]["JDate5"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE5"].ToString().Trim()));
                }
                else if (string.Compare(strDate, Dt.Rows[0]["JDate6"].ToString().Trim()) >= 0)
                {
                    nAmt = (Convert.ToInt64(Dt.Rows[0]["PRICE6"].ToString().Trim()));
                }
                else
                {
                    nAmt = 0;
                }

                Dt.Dispose();
                Dt = null;

                return nAmt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return 0;
            }
        }
        #endregion

        # region 격리병실 사용여부 조회 (음압구분은 있으나 음압수가는 산정안됨) // RTN_KEKLI_ROOM
        public string Rtn_Kekli_Room(PsmhDb pDbCon, string strPano, string strDate, int intBed, long lngIpdno)
        {
            string strGubun = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ComFunc CF = new ComFunc();

            try
            {
                //CF.DATE_ADD(pDbCon, strDate, 1)
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT INFECT,AIR_INFECT                                                                        ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR_INFECT                                                ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                                                                 ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "                                                                 ";
                SQL = SQL + ComNum.VBLF + "    AND TRUNC(TRSDATE) < TO_DATE('" + CF.DATE_ADD(pDbCon, strDate, 1) + "','YYYY-MM-DD')         ";
                SQL = SQL + ComNum.VBLF + "  ORDER By TRSDATE DESC                                                                          ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                if (Dt.Rows[0]["INFECT"].ToString().Trim() == "1")      // 일반격리
                {
                    if (intBed == 1)
                    {
                        strGubun = "1";     // 1인실
                    }
                    else if (intBed == 2)
                    {
                        strGubun = "3";     // 2인실
                    }
                    else
                    {
                        strGubun = "2";     // 다인실
                    }
                }
                else if (Dt.Rows[0]["INFECT"].ToString().Trim() == "1") // 음압격리
                {
                    if (intBed == 1)
                    {
                        strGubun = "4";     // 1인실
                    }
                    else if (intBed == 2)
                    {
                        strGubun = "6";     // 2인실
                    }
                    else
                    {
                        strGubun = "5";     // 다인실
                    }
                }
               

                Dt.Dispose();
                Dt = null;

                return strGubun;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 입원자 입원당일 치과진료 여부 확인 // RTN_외래치과진료여부
        public string Rtn_Opd_Jin_Dent(PsmhDb pDbCon, string strPano, string strInDate, string strBi)
        {
            string strRowid = "";
            int intCnt = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE                                         ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = 'DT'                                      ";
                SQL = SQL + ComNum.VBLF + "    AND BI NOT IN ('21','22','31','32','33')                 ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                strRowid = Dt.Rows[0]["DRCODE"].ToString().Trim();
                    
                Dt.Dispose();
                Dt = null;

                //당일 치과단일 치료인 사람은 주과가 치과가 되므로 지병처리 안함
                //치과 단일치료 대상자는 이 로직에서 제외됨 (치과를 지병처리하기 위한 체크로직임)
                if (strRowid != "")      
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT COUNT(*) CNT                                         ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                             ";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')   ";
                    SQL = SQL + ComNum.VBLF + "    AND BI NOT IN ('21','22','31','32','33')                 ";

                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "";
                    }
                    if (Dt.Rows.Count == 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        //ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return "";
                    }

                    intCnt = Convert.ToInt32(Dt.Rows[0]["CNT"].ToString().Trim());
                    
                    if (intCnt == 1)
                    {
                        strRowid = "";
                    }

                    Dt.Dispose();
                    Dt = null;
                }
                
                return strRowid;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 입원자 입원당일 응급실진료 여부 확인 // RTN_응급실당일진료여부
        public string Rtn_Opd_Jin_ER(PsmhDb pDbCon, string strPano, string strInDate)
        {
            string strRowid = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID                                                ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = 'ER'                                      ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                strRowid = Dt.Rows[0]["ROWID"].ToString().Trim();

                Dt.Dispose();
                Dt = null;

                return strRowid;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion 

        # region 의료급여 입원자 알림정보 생성 // Read_Ipd_Gub_Notice
        public void Read_Ipd_Gub_Notice(PsmhDb pDbCon, string strPano, string strSName, long lngIpdno)
        {
            

            DataTable Dt = null;
            
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID                                ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_GUB_NOTICE ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'             ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = 'D'                          ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                
                if (Dt.Rows.Count > 0)
                {
                    
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO IPD_GUB_NOTICE (Pano,SName, IPDNO, GUBUN, EntDate ) Values ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strPano + "','" + strSName + "'," + lngIpdno + ",'S',SYSDATE)       ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }
        }
        #endregion

        #region 퇴원심사시 입원료 누락일자 점검 // Check_RoomCharge_Exist
        public bool Check_RoomCharge_Exist(PsmhDb pDbCon, long lngIpdno, string strInDate, string strOutDate)
        {
            string strOldDate = "";
            string strMinDate = "";
            string strMaxDate = "";
            string strBDate = "";

            long lngAmt = 0;
            int intRead = 0;
            int i;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            ComFunc CF = new ComFunc();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate, SUM(Amt1+Amt2) Amt    ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                       ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "                                 ";
                SQL = SQL + ComNum.VBLF + "    AND Bun >= '03'                                              ";
                SQL = SQL + ComNum.VBLF + "    AND Bun <= '10'                                              ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) <> 'C-'                               ";
                SQL = SQL + ComNum.VBLF + "  GROUP By BDATE                                                 ";
                SQL = SQL + ComNum.VBLF + "  ORDER By BDATE ASC                                             ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                
                intRead = Dt.Rows.Count;

                if (intRead == 0)
                {
                    ComFunc.MsgBox("입원료가 누락되었습니다.");
                    Dt.Dispose();
                    Dt = null;
                    return false;
                }
                else
                {
                    strOldDate = Dt.Rows[0]["BDate"].ToString().Trim();
                }
                
                for (i = 0; i < intRead; i++)
                {
                    strBDate = Dt.Rows[i]["BDate"].ToString().Trim();
                    lngAmt = Convert.ToInt64(Dt.Rows[i]["Amt"].ToString().Trim());

                    if(lngAmt == 0)
                    {
                        ComFunc.MsgBox(strBDate + "일 입원료가 누락되었습니다.");
                        Dt.Dispose();
                        Dt = null;
                        return false;
                    }

                    if(string.Compare(strBDate, strOldDate) > 0)
                    {
                        ComFunc.MsgBox(strOldDate + "일 입원료가 누락되었습니다.");
                        Dt.Dispose();
                        Dt = null;
                        return false;
                    }

                    if (strMinDate == "")
                    {
                        strMinDate = strBDate;
                    }

                    strMaxDate = strBDate;
                    //strOldDate = Convert.ToDateTime(strBDate).AddDays(1).ToString();
                    strOldDate = CF.DATE_ADD(pDbCon, strBDate, 1);
                }

                Dt.Dispose();
                Dt = null;

                if (string.Compare(strMinDate, strInDate) > 0)
                {
                    ComFunc.MsgBox("입원당일 입원료가 누락되었습니다.");
                    return false;
                }

                //strOldDate = Convert.ToDateTime(strOutDate).AddDays(-1).ToString();
                strOldDate = CF.DATE_ADD(pDbCon, strOutDate, -1);

                if (string.Compare(strMaxDate, strOutDate) > 0)
                {
                    ComFunc.MsgBox("퇴원전일 입원료가 누락되었습니다.");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        #region 퇴원당일 식대료 점검 // CHK_퇴원당일식대점검
        public bool Chk_Discharge_Diet(PsmhDb pDbCon, string strPano, long lngTrsno, string strOutDate)
        {
            long lngAmt = 0;
            int intRead = 0;
            double intQty1 = 0;
            double intQty2 = 0;
            int i;
            string strSuCode = "";
            string strMsg = ""; 

            DataTable Dt = null;
            DataTable Dt2 = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.PANO, C.BI, A.ROOMCODE,   ";
                SQL = SQL + ComNum.VBLF + "        C.DEPTCODE, C.DRCODE, B.WARDCODE, A.SUCODE, A.BUN, B.BOHUN,          ";
                SQL = SQL + ComNum.VBLF + "        SUM(CASE WHEN A.QTY >= 5 THEN 1 ELSE A.QTY END) QTY, D.GUBUN         ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DIET_NEWORDER A,                               ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B,                              ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS C,                                   ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "DIET_NEWCODE D                                 ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = " + strPano + "                                             ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = TO_DATE('" + strOutDate +"','YYYY-MM-DD')                ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE NOT IN ('########')                                         ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = D.SUCODE                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND A.DIETCODE =D.DIETCODE                                               ";
                SQL = SQL + ComNum.VBLF + "    AND A.BUN =D.BUN                                                         ";
                SQL = SQL + ComNum.VBLF + "    AND A.DIETDAY IN ('1','2','3')                                           ";
                SQL = SQL + ComNum.VBLF + "    AND A.DIETCODE <> '37'                                                   ";
                SQL = SQL + ComNum.VBLF + "    AND C.TRSNO = " + lngTrsno + "                                           ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND C.ACTDATE IS NULL                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE <> 'FD020'                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GBIPD NOT IN ('9','D')                                             ";
                SQL = SQL + ComNum.VBLF + "    AND C.GBSTS NOT IN ('1','7')                                             ";
                SQL = SQL + ComNum.VBLF + "    AND B.IPDNO = C.IPDNO                                                    ";
                SQL = SQL + ComNum.VBLF + "  GROUP By A.ACTDATE, A.PANO, C.BI, A.ROOMCODE, C.DEPTCODE, C.DRCODE,        ";
                SQL = SQL + ComNum.VBLF + "           B.WARDCODE, A.SUCODE, A.BUN, B.BOHUN, D.GUBUN                     ";
                SQL = SQL + ComNum.VBLF + "  ORDER By C.BI                                                              ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                intRead = Dt.Rows.Count;

                if (intRead > 0)
                {
                    for (i = 0; i <= (intRead - 1); i++)
                    {
                        strSuCode = Dt.Rows[i]["SUCODE"].ToString().Trim();
                        intQty1 = VB.Val(Dt.Rows[i]["QTY"].ToString().Trim());
                        lngAmt = 0;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SUM(Qty*Nal) SQty,SUM(Amt1+Amt2) SAmt, SUCODE    ";
                        SQL = SQL + ComNum.VBLF + "   From ADMIN.IPD_NEW_SLIP                         ";
                        SQL = SQL + ComNum.VBLF + "  Where Pano='" + strPano + "'                           ";
                        SQL = SQL + ComNum.VBLF + "    AND TRSNO=" + lngTrsno + "                           ";
                        SQL = SQL + ComNum.VBLF + "    AND SUCODE='" + strSuCode + "'                       ";
                        SQL = SQL + ComNum.VBLF + "    AND BDate=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY SUCODE                                        ";
                        SQL = SQL + ComNum.VBLF + "  HAVING SUM(AMT1+AMT2) <> 0                             ";

                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return false;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            lngAmt = Convert.ToInt64(VB.Val(Dt2.Rows[0]["SAMT"].ToString().Trim()));
                            intQty2 = VB.Val(Dt2.Rows[0]["SQTY"].ToString().Trim());
                        }

                        Dt2.Dispose();
                        Dt2 = null;

                        if (lngAmt != 0 && intQty1 != intQty2)
                        {
                            strMsg = "";
                            strMsg = strMsg + ComNum.VBLF + "퇴원당일 식대비 발생이 맞지 않습니다." + ComNum.VBLF;
                            strMsg = strMsg + ComNum.VBLF + "식이오더 코드:" + strSuCode;
                            strMsg = strMsg + ComNum.VBLF + "오더 : " + intQty1 + " 개";
                            strMsg = strMsg + ComNum.VBLF + "발생 : " + intQty2 + " 개";

                            ComFunc.MsgBox(strMsg);

                            Dt.Dispose();
                            Dt = null;
                        }
                    }
                }
                
                Dt.Dispose();
                Dt = null;
                
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 재원자 마약수가 SLIP 발생 여부 // CHK_마약처방대상자체크
        public bool Chk_Drug_IpdSlip(PsmhDb pDbCon, string strPano, long lngIpdno)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUCODE, SUM(A.QTY*A.NAL) SQTY      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_ERP + "DRUG_JEP B      ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + strPano + "'           ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE                 ";
                SQL = SQL + ComNum.VBLF + "    AND B.CHENGGU = '09'                     ";
                SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = " + lngIpdno + "           ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY A.SUCODE                          ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(a.QTY * a.NAL) > 0               ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }
                
                Dt.Dispose();
                Dt = null;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 재원자 마약수가 처방여부 // CHK_마약처방대상자체크2
        public bool Chk_Drug_Order(PsmhDb pDbCon, string strPano, string strInDate)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUCODE, SUM(A.QTY*A.NAL) SQTY                      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER A,                   ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_ERP + "DRUG_JEP B                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO = '" + strPano + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE>=TO_DATE('" + strInDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE                                 ";
                SQL = SQL + ComNum.VBLF + "    AND B.CHENGGU = '09'                                     ";
                SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE='ER'                                      ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY A.SUCODE                                          ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(a.QTY * a.NAL) > 0                               ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.SUCODE, SUM(A.QTY*A.NAL) SQTY                      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER A,                   ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_ERP + "DRUG_JEP B                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO = '" + strPano + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE>=TO_DATE('" + strInDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND A.SUCODE = B.JEPCODE                                 ";
                SQL = SQL + ComNum.VBLF + "    AND B.CHENGGU = '09'                                     ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY A.SUCODE                                          ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(a.QTY * a.NAL) > 0                               ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 도로명주소 입력 점검 // CHK_주소입력체크
        public bool Chk_ZipCode_Road(PsmhDb pDbCon, string strZip1, string strZip2, string strZip3, string strJusoDtl, string strBuildNo)
        {
            int i = 0;
            bool bReturn = false;

            ComQuery cCQ = new ComQuery();
            ComFunc CF = new ComFunc();

            for (i=1; i <= VB.Len(strJusoDtl); i++)
            {
                if(VB.IsNumeric(VB.Mid(strJusoDtl,i,1)) == true)
                {
                    bReturn = true;
                    break;
                }
            }

            if (VB.Len(strZip2) < 3)
            {
                if (cCQ.Read_RoadJuso(pDbCon, strBuildNo) == "")
                    bReturn = false;
            }
            else
            {
                if (CF.READ_BAS_Mail(pDbCon, strZip1 + strZip2).Contains("괴동") == true)
                    bReturn = false;
            }

            if (strJusoDtl.Contains("주소미상") == true)
                bReturn = false;

            if (strZip2.Length < 3)
            {
                if (cCQ.Read_RoadJuso(pDbCon, strBuildNo) == "")
                    bReturn = false;
            }
            else
            {
                if (CF.READ_BAS_Mail(pDbCon, strZip1 + strZip2) == "" || strJusoDtl == "")
                    bReturn = false;
            }
            
            if (strZip3 == "")
            { 
                if(VB.Val(strZip1) == 0 || VB.Val(strZip2) == 0)
                { 
                    bReturn = false;
                }
            }
            else
            { 
                if (VB.Val(strZip3) == 0)
                {
                    bReturn = false;
                }
            }

            if (strZip3.Length == 5 && cCQ.Read_RoadJuso(pDbCon, strBuildNo) != "")
            {
                bReturn = true;
            }
            
            return bReturn;
            
        }
        #endregion

        # region 상급병실 사용내역 조회 // Read_상급병실사용내역
        public string Chk_Senior_Ward(PsmhDb pDbCon, long lngIpdno,long lngTrsno, [Optional] long lngBaseAmt)
        {
            int i = 0;
            int intRead = 0;
            string strBDate = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            string strDate4 = "";

            string strRtMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT  a.ROOMCODE, a.BaseAmt,                 ";
                SQL = SQL + ComNum.VBLF + "  b.TBED, b.TBED1, b.TBED2, b.TBED3,             ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE,           ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(b.TransDate1,'YYYY-MM-DD') TRANSDATE1, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(b.TransDate2,'YYYY-MM-DD') TRANSDATE2, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(b.TransDate3,'YYYY-MM-DD') TRANSDATE3, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(b.TransDate4,'YYYY-MM-DD') TRANSDATE4  ";
                SQL = SQL + ComNum.VBLF + "      From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, ";
                SQL = SQL + ComNum.VBLF + "           " + ComNum.DB_PMPA + "BAS_ROOM b      ";
                SQL = SQL + ComNum.VBLF + "     Where a.IPDNO =" + lngIpdno + "             ";
                SQL = SQL + ComNum.VBLF + "       AND a.TRSNO =" + lngTrsno + "             ";
                SQL = SQL + ComNum.VBLF + "       AND a.BUN='77'                            ";
                SQL = SQL + ComNum.VBLF + "       AND a.Roomcode = b.RoomCode               ";
                if(lngBaseAmt > 0)
                {
                    SQL = SQL + ComNum.VBLF + "       AND a.BaseAmt = " + lngBaseAmt + "    ";
                }

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                intRead = Dt.Rows.Count;

                if (intRead > 0)
                {
                    for(i=0; i<=(intRead - 1); i++)
                    {
                        strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                        strDate1 = Dt.Rows[i]["TRANSDATE1"].ToString().Trim();
                        strDate2 = Dt.Rows[i]["TRANSDATE2"].ToString().Trim();
                        strDate3 = Dt.Rows[i]["TRANSDATE3"].ToString().Trim();
                        strDate4 = Dt.Rows[i]["TRANSDATE4"].ToString().Trim();

                        if (string.Compare (strBDate, strDate1) > 0)
                        {
                            strRtMsg = Dt.Rows[i]["TBED"].ToString().Trim();
                        }
                        else if(string.Compare(strBDate, strDate2) > 0)
                        {
                            strRtMsg = Dt.Rows[i]["TBED1"].ToString().Trim();
                        }
                        else if (string.Compare(strBDate, strDate3) > 0)
                        {
                            strRtMsg = Dt.Rows[i]["TBED2"].ToString().Trim();
                        }
                        else if (string.Compare(strBDate, strDate4) > 0)
                        {
                            strRtMsg = Dt.Rows[i]["TBED3"].ToString().Trim();
                        }

                        if(strRtMsg != "")
                        {
                            strRtMsg = strRtMsg + "인실";
                        }
                    }
                }
                
                return strRtMsg;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 입원 Verbal 오더 체크 // IPD_Verb_Doctror_ORDER_Chk
        public bool Chk_Ipd_Verbal_Order(PsmhDb pDbCon, string strPano, string strInDate, string strOutDate)
        {
            int i = 0;
            int intRead = 0;
            string strRtnMsg = "";
            bool rtnVal = true;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strOutDate == "")
            {
                strOutDate = DateTime.Now.ToString("yyyy-mm-dd");
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUCODE,TO_CHAR(BDATE,'YYYY-MM-DD') BDate           ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE Ptno     = '" + strPano + "'                       ";
                SQL = SQL + ComNum.VBLF + "    AND GbStatus IN (' ','D','D+')                         ";
                SQL = SQL + ComNum.VBLF + "    AND BDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND BDate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND NurseID <> ' '                                     "; //간호사처방만
                SQL = SQL + ComNum.VBLF + "    AND GbVerb ='Y'                                        "; //구두처방대상건만
                SQL = SQL + ComNum.VBLF + "    AND Bun IN ('11','12','20')                            ";
                SQL = SQL + ComNum.VBLF + "    AND DRORDERVIEW IS NULL                                ";
                SQL = SQL + ComNum.VBLF + "    AND (GbSend  = ' ' OR GbSend IS NULL)                  "; //전송된것
                SQL = SQL + ComNum.VBLF + "    AND OrderSite Not Like 'DC%'                           ";
                SQL = SQL + ComNum.VBLF + "    AND OrderSite <>  'CAN'                                ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDate, Seqno                                    ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                intRead = Dt.Rows.Count;

                for (i = 0; i <= (intRead - 1); i++)
                {
                    strRtnMsg = strRtnMsg + "수    가: " + Dt.Rows[i]["SUCODE"].ToString().Trim() + ComNum.VBLF;
                    strRtnMsg = strRtnMsg + "처방일자: " + Dt.Rows[i]["BDATE"].ToString().Trim() + ComNum.VBLF;
                }
                
                Dt.Dispose();
                Dt = null;

                if (strRtnMsg != "")
                {
                    clsPublic.GstrMsgList = "미확정된 구두처방 Data가 있습니다." + ComNum.VBLF;
                    clsPublic.GstrMsgList += "담당의사의 구두처방 확정후 심사가능합니다." + ComNum.VBLF + ComNum.VBLF;
                    clsPublic.GstrMsgList += strRtnMsg;

                    ComFunc.MsgBox(clsPublic.GstrMsgList, "심사완료 불가");

                    rtnVal = false;
                }
                
                return rtnVal;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 응급실 보호자 출입증 교부확인 // CHK_보호자출입증확인
        public string Chk_Er_Pass_Issue(PsmhDb pDbCon, string strPano, long lngIpdno)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ARTICLE                              ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + strPano + "'             ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "             ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                strRtnMsg = Dt.Rows[0]["ARTICLE"].ToString().Trim();
                
                Dt.Dispose();
                Dt = null;

                return strRtnMsg;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 추가 입원료 발생 확인 // CHK_추가입원료발생
        public bool Chk_Add_Admission_Fee(PsmhDb pDbCon, string strPano, long lngIpdno, long lngTrsno)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUCODE, SUM(QTY*NAL) SQTY            ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP    ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "'              ";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO =" + lngIpdno + "               ";
                SQL = SQL + ComNum.VBLF + "   AND TRSNO =" + lngTrsno + "               ";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE IN ('AB2401A','AB240A','AB2201A','AB220A','AB270A','AB2701A','AO280A','AO240A','AO2401A', 'AO2801A', 'AV8201A', 'AV820A',   'AO280A','AO2801A','AO2801B','AO2801B','AV820A','AV820B','AV8201A','AV8201B','AB901A')        ";
                SQL = SQL + ComNum.VBLF + " GROUP BY SUCODE                             ";
                SQL = SQL + ComNum.VBLF + "Having Sum(Qty * Nal) > 0                    ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }
                
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 이전 상한제 유무 확인 // CHK_이전상한제유무
        public bool Chk_Pre_SangHan(PsmhDb pDbCon, string strPano, string strInDate, long lngIpdno)
        {
            int i = 0;
            int intRead = 0;
            string strFDate = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            strFDate = VB.Left(strInDate, 4) + "-01-01";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SangAmt From " + ComNum.DB_PMPA + "IPD_TRANS         ";
                SQL = SQL + ComNum.VBLF + "  Where Pano='" + strPano + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND INDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO <> " + lngIpdno + "                            ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                intRead = Dt.Rows.Count;
                
                for(i = 0; i <= (intRead - 1); i++)
                {
                    if (Convert.ToInt64(Dt.Rows[i]["SangAmt"].ToString().Trim()) > 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }
                    
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 당일 DRG 대상자 확인 // CHK_당일DRG입원대상
        public bool Chk_DRG_Subject(PsmhDb pDbCon, string strPano, string strInDate, string strDept)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GBDRG                                                ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_DEPTJEPSU                  ";
                SQL = SQL + ComNum.VBLF + "  Where Pano='" + strPano + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + strDept + "'                         ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["GBDRG"].ToString().Trim() == "Y")
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }
                
                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID                                            ";
                SQL = SQL + ComNum.VBLF + "  From " + ComNum.VBLF + "IPD_RESERVED                   ";
                SQL = SQL + ComNum.VBLF + " Where Pano='" + strPano + "'                            ";
                SQL = SQL + ComNum.VBLF + "   AND REDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDept + "'                      ";
                SQL = SQL + ComNum.VBLF + "   AND GBDRG = 'Y'                                       ";
                
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (Dt.Rows.Count > 0)
                { 
                    if (Dt.Rows[0]["ROWID"].ToString().Trim() == "Y")
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }
                
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 입원당일 타과 접수여부 // CHK_당일타과접수여부
        public bool Chk_OpdJepsu_Other(PsmhDb pDbCon, string strPano, string strInDate, string strDept)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID From " + ComNum.DB_PMPA + "OPD_MASTER          ";
                SQL = SQL + ComNum.VBLF + "  Where Pano='" + strPano + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE <> " + strDept + "                          ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;

                }
                if (Dt.Rows[0]["ROWID"].ToString().Trim() == "Y")
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                if (Dt.Rows[0]["ROWID"].ToString().Trim() != "")
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }
                
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 응급실 미전송 오더 점검 // CHK_응급실미전송오더점검
        public bool Chk_ErOrder_Not_Send(PsmhDb pDbCon, string strPano, string strDate)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string ArgDate = Convert.ToDateTime(strDate).AddDays(-1).ToString("yyyy-MM-dd");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT COUNT(PTNO) CNT                                                                  ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_MED + "OCS_iORDER                                                  ";
                SQL = SQL + ComNum.VBLF + "  Where Ptno='" + strPano + "'                                                           ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                 ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strDate + "','YYYY-MM-DD')                                 ";
                SQL = SQL + ComNum.VBLF + "    AND GbSend = '*'                                                                     ";
                SQL = SQL + ComNum.VBLF + "    AND Sucode IS  NOT Null                                                              ";
                SQL = SQL + ComNum.VBLF + "    AND GBIOE ='E'                                                                       ";
                SQL = SQL + ComNum.VBLF + "  ORDER By PTNO                                                                          ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;


                }
                if (Convert.ToInt32(Dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 응급실 오더 미수납 점검 // CHK_입원전_응급실미수납점검
        public bool Chk_ErOrder_Not_Sunap(PsmhDb pDbCon, string strPano, string strDate)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strFDate = Convert.ToDateTime(strDate).AddDays(-2).ToString("yyyy-MM-dd");
            string strTDate = Convert.ToDateTime(strDate).AddDays(-1).ToString("yyyy-MM-dd"); 

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT COUNT(PTNO) CNT                                                                  ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_MED + "OCS_iORDER                                                  ";
                SQL = SQL + ComNum.VBLF + "  Where Ptno='" + strPano + "'                                                           ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND GbAct = '*'                                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND Sucode IS  NOT Null                                                              ";
                SQL = SQL + ComNum.VBLF + "    AND GBIOE ='E'                                                                       ";
                SQL = SQL + ComNum.VBLF + "  ORDER By PTNO                                                                          ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Convert.ToInt32(VB.Val(Dt.Rows[0]["CNT"].ToString().Trim())) > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 입원 예약정보 확인 // CHK_입원예약여부
        public string Rtn_Ipd_Reserved(PsmhDb pDbCon, string strPano, string strDate)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO,SNAME,TO_CHAR(REDATE,'YYYY-MM-DD') ReDate,      ";
                SQL = SQL + ComNum.VBLF + "        DEPTCODE,DRCODE,WARDCODE,ROOMCODE,INSIL,CDATE,SDATE, ";
                SQL = SQL + ComNum.VBLF + "        Remark , GbSMS, GBCHK, SMSDATE, GbDRG                ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_RESERVED                   ";
                SQL = SQL + ComNum.VBLF + "  Where PANO = '" + strPano + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND REDATE > TO_DATE('" + strDate + "','YYYY-MM-DD')     ";
                SQL = SQL + ComNum.VBLF + "    AND GBCHK <> '1'                                         ";
                
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["REDATE"].ToString().Trim();
                    strRtnMsg = strRtnMsg + "^^" + Dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }
                
                Dt.Dispose();
                Dt = null;

                return strRtnMsg;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return strRtnMsg;
            }
        }
        #endregion

        # region 외래 의료질평가 지원금 산정여부 확인 // CHK_의료질평가지원금
        public bool Chk_Med_Sup_Amt(PsmhDb pDbCon, string strPano, string strDate)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUCODE, SUM(QTY*NAL) SQTY                            ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_SLIP                       ";
                SQL = SQL + ComNum.VBLF + "  Where PANO='" + strPano + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')     ";                
                SQL = SQL + ComNum.VBLF + "    AND SUCODE IN ('AU214','AU312')                          ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY SUCODE                                            ";
                SQL = SQL + ComNum.VBLF + "  Having Sum(Qty * Nal) > 0                                  ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(VB.Val(Dt.Rows[0]["SQTY"].ToString().Trim())) > 0)
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }
                
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 병동 입실시간 조회 // RTN_IPTIME
        public string Rtn_IpTime(PsmhDb pDbCon, long lngIpdno)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  TO_CHAR(WardInTime,'YYYY-MM-DD HH24:MI') InDate     ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER                 ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "                             ";
                
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                strRtnMsg = Dt.Rows[0]["InDate"].ToString().Trim();
                
                Dt.Dispose();
                Dt = null;

                return strRtnMsg;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }

        }
        #endregion

        # region 입원 본인부담율 Set // IRPG_AMT_SET 참조안함 Ipd_RPG_Amt_Set 변경
        public void IRPG_AMT_SET(PsmhDb pDbCon, int intBonRate, string strCT)  //Ipd_RPG_Amt_Set 변경
        {
            //Ipd_RPG_Amt_Set 변경
           
                
        }
        #endregion

        # region 통합간호간병 동의서 작성여부 // CHK_TOTAL_CARE
        public bool Chk_Total_Care(PsmhDb pDbCon, long lngIpdno, string strPano)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID                                ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_TOTAL_CARE ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                  ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'             ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "             ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;

                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }
                
                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 입원, 퇴원 당일 상급병실 사용여부 체크 // CHK_IpdTewon_RoomChaGesan
        public void Chk_IpdTewon_RoomChaGesan(PsmhDb pDbCon, string strPano, long lngIpdno, string strInDate, string strOutDate)
        {
            string strRtmMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ComFunc CF = new ComFunc();

            //입원당일 상급병실 사용여부 체크
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.FrRoom,a.ToRoom                                                                    ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_TRANSFOR a,                                                ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b                                                     ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                                                             ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "                                                             ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')                               ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSDATE <  TO_DATE('" + CF.DATE_ADD(pDbCon, strInDate, 1) + "','YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "    AND a.FrRoom = b.RoomCode                                                                ";
                SQL = SQL + ComNum.VBLF + "    AND b.RoomClass IN ('A','B','C','D','E','F','G','H')                                     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtmMsg = "입원당일";
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }

            //퇴원당일 상급병실 사용여부 체크
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.FrRoom,a.ToRoom                                                                        ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_TRANSFOR a,                                                    ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b                                                         ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                                                                 ";
                SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + lngIpdno + "                                                                 ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSDATE >= TO_DATE('" + strOutDate + "','YYYY-MM-DD')                                  ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSDATE <  TO_DATE('" + CF.DATE_ADD(pDbCon, strOutDate, 1) + "','YYYY-MM-DD')          ";
                SQL = SQL + ComNum.VBLF + "    AND a.FrRoom = b.RoomCode                                                                    ";
                SQL = SQL + ComNum.VBLF + "    AND b.RoomClass IN ('A','B','C','D','E','F','G','H')                                         ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;

                }

                if (Dt.Rows.Count > 0)
                {
                    if(strRtmMsg != "")
                    {
                        strRtmMsg = "과 퇴원당일";
                    }
                    else
                    {
                        strRtmMsg = "퇴원당일";
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }

            if(strRtmMsg != "")
            {
                strRtmMsg = strRtmMsg + " 상급병실 사용내역이 있습니다.";
                ComFunc.MsgBox(strRtmMsg);
            }
        }
        #endregion

        # region 입원료 본인부담 주상병 예외대상 체크 // READ_BAS_ILLS_IPDETC
        public bool Rtn_Bas_ILLS_IpdEt(PsmhDb pDbCon, string strILLCode)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT IPDETC                               ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ILLS       ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                  ";
                SQL = SQL + ComNum.VBLF + "    AND ILLCODE = '" + strILLCode + "'       ";
                SQL = SQL + ComNum.VBLF + "    AND ILLCLASS = '1'                       ";
                SQL = SQL + ComNum.VBLF + "    AND IPDETC = 'Y'                         ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }
        }
        #endregion

        # region 후불수납대상 외래 오더 점검 // Read_Sunap_Order
        public bool Chk_Sunap_Oorder(PsmhDb pDbCon, string strPano, string strBDate, string strDept, string strBi)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM ADMIN.OCS_OORDER                     ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND Ptno = '" + strPano + "'                         ";
                //2013-11-05
                if (clsPmpaPb.GstrIpdAllDept != "OK")
                {
                    if (strDept == "MD" || strDept == "MN")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND (DeptCode = '" + strDept + "' OR DEPTCODE = 'HD')    ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + strDept + "'                         ";
                    }
                }
                else
                {
                    //2014-02-21
                    if (VB.Left(strBi, 1) == "2")
                    {
                        if (strDept == "MN")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND (DeptCode = '" + strDept + "'  OR DEPTCODE = 'HD' )  ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + strDept + "'                         ";
                        }
                    }
                    else
                    {
                        if (VB.Left(strBi, 1) == "1")
                        {
                            if (strDept == "MN")
                            {
                                SQL = SQL + ComNum.VBLF + "    AND (DeptCode = '" + strDept + "'  OR DEPTCODE = 'HD' ";
                                SQL = SQL + ComNum.VBLF + "         OR DeptCode IN ( SELECT DeptCode FROM ADMIN.OCS_OORDER WHERE BDATE=TO_DATE('" + strBDate + "','YYYY-MM-DD') AND BI IN ('11','12','13') ) ) ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "    AND (DeptCode = '" + strDept + "'                    ";
                                SQL = SQL + ComNum.VBLF + "         OR DeptCode IN ( SELECT DeptCode FROM ADMIN.OCS_OORDER WHERE BDATE=TO_DATE('" + strBDate + "','YYYY-MM-DD') AND BI IN ('11','12','13')  ) ) ";
                            }
                        }
                        else if (VB.Left(strBi, 1) == "3")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                 ";
                        }
                        else if (VB.Left(strBi, 1) == "4")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                 ";
                        }
                        else if (VB.Left(strBi, 1) == "4")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND Bi = '" + strBi + "'                 ";
                        }
                        else
                        {
                            if (strDept == "MN")
                            {
                                SQL = SQL + ComNum.VBLF + "    AND (DeptCode = '" + strDept + "'  OR DEPTCODE = 'HD')   ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + strDept + "'                         ";
                            }
                        }
                    }
                }
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')                     ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) <> '$$'                                           "; //2005-09-02 '심사계 심경순샘 요청 尹
                SQL = SQL + ComNum.VBLF + "    AND GbSunap = '0'                                                        ";
                SQL = SQL + ComNum.VBLF + "    AND GBAUTOSEND2 IN ('1','2')                                             "; //후불수납대상
                SQL = SQL + ComNum.VBLF + "    AND SUCODE NOT IN ( SELECT SUCODE FROM BAS_SUN WHERE SUCODE LIKE '@V%')  "; //JJY(2004-02-05 추가)
                SQL = SQL + ComNum.VBLF + "    AND Nal > 0                                                              "; //2006-07-31 날수 1일만 전송함 전계장님 요청
                SQL = SQL + ComNum.VBLF + "    AND ( Res IS NULL OR Res <>'1' )                                         ";
                SQL = SQL + ComNum.VBLF + "    AND SeqNo > 0                                                            ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (Dt.Rows.Count > 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return true;
                }

                Dt.Dispose();
                Dt = null;

                return false;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return false;
            }

        }
        #endregion

        # region 입원수속시 타과 오더없고 접수만 있는경우 체크 // Read_Opd_Jepsu_Order
        public string Chk_Opd_Jepsu_Order(PsmhDb pDbCon, string strPano, string strBDate, string strDept)
        {
            string strRtmMsg = "";
            string strDeptCode = "";
            int i = 0;
            int intCnt = 0;

            DataTable Dt = null;
            DataTable Dt2 = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //입원당일 상급병실 사용여부 체크
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE                                         ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER                 ";
                SQL = SQL + ComNum.VBLF + "  Where 1=1                                              ";
                SQL = SQL + ComNum.VBLF + "    AND PANO = '" + strPano + "'                         ";
                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE <> '" + strDept + "'                    ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE <> 'ER'                                 ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                intCnt = Dt.Rows.Count;

                for (i=0; i<= (intCnt - 1); i++)
                {
                    strDeptCode = Dt.Rows[i]["DEPTCODE"].ToString().Trim();

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT ROWID                                            ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER                  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + strPano + "'                         ";
                    SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + strDept + "'                     ";
                    SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) <> '$$'                       ";
                    SQL = SQL + ComNum.VBLF + "    AND SUCODE IS NOT NULL                               ";
                    SQL = SQL + ComNum.VBLF + "    AND SeqNo > 0                                        ";

                    SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "";
                    }

                    if (Dt2.Rows.Count == 0)
                    {
                        strRtmMsg = strRtmMsg + strDeptCode + ", ";
                    }

                    Dt2.Dispose();
                    Dt2 = null;
                }

                Dt.Dispose();
                Dt = null;

                return strRtmMsg;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region BAS_SUGAHIS S항 조회 // BAS_SUGA_HIS_S항
        public string Rtn_Bas_Sugahis_S(PsmhDb pDbCon, string strSuCode, string strDate)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUGBS                                                ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGAHIS                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                ";
                SQL = SQL + ComNum.VBLF + "    AND SuCode = '" + strSuCode + "'                         ";
                SQL = SQL + ComNum.VBLF + "    AND JobDate  >= TO_DATE('" + strDate + "','YYYY-MM-DD')  ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                { 
                    strRtnMsg = Dt.Rows[0]["SUGBS"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region BAS_SUN S항 조회 // READ_BAS_Sun_S항
        public string Rtn_Bas_Sun_S(PsmhDb pDbCon, string strSuNext)
        {
            string strRtnMsg = "0";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SugbS                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + strSuNext + "'     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["SugbS"].ToString().Trim();
                }
                
                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region BAS_SUN Z항 조회 // READ_BAS_Sun_Z항
        public string Rtn_Bas_Sun_Z(PsmhDb pDbCon, string strSuNext)
        {
            string strRtnMsg = "0";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SugbZ                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + strSuNext + "'     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["SugbZ"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region BAS_SUN GBNS항 조회 // READ_BAS_Sun_GBNS항
        public string Rtn_Bas_Sun_GBNS(PsmhDb pDbCon, string strSuNext)
        {
            string strRtnMsg = "0";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GBNS                             ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + strSuNext + "'     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["GBNS"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 응급가산수가 Slip 스프레드 DisPlay // DisPlay_IPD_SlipDtl_응급가산수가
        public void Rtn_Display_Ipd_ErPed(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, long lngTrsno)
        {
            int intCnt = 0;
            int intRow = 0;
            int i = 0;
            int nAge = 0;
            int intNal = 0;
            long lngAmt = 0;
            long lngTot2 = 0;
            double nSQNal = 0;
            double nMQty = 0;
            double dAgeDay = 0;
            string strBun = "";
            string strGisul = "";
            string strGbChild = "";
            string strNgt = "";
            string strSuNext = "";
            string strER = "";
            string strSugbAD = "";
            string strGbSgAdd = "";
            string strSugbAC = "";
            string strSuGbAB = "";
            string strSuGbAA = "";

            DataTable Dt = null;
            DRG DRG = new DRG();

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Sunext,SUM(A.Qty*A.Nal) SQNal, A.Qty SQTY, SUM(A.Nal) SNal,   ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE, a.Bun,                          ";
                SQL = SQL + ComNum.VBLF + "        a.GbGisul,a.GbChild, a.GbNgt,b.SuNameK,a.GbEr,                       ";
                SQL = SQL + ComNum.VBLF + "        b.SugbAA,b.SugbAB,b.SugbAC,b.SugbAD,a.GBSGADD                        "; 
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,                                ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                                                 ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSNO = " + clsPmpaType.TIT.Trsno + "                              ";
                SQL = SQL + ComNum.VBLF + "    AND a.GbEr IN ('1','2','3')                                              ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY a.Sunext,a.BDate,a.Bun,a.GbGisul,a.GbChild, a.GbNgt,b.SuNameK,    ";
                SQL = SQL + ComNum.VBLF + "           a.GbEr,b.SugbAA,b.SugbAB,b.SugbAC,b.SugbAD,a.GBSGADD ,Qty             ";
                SQL = SQL + ComNum.VBLF + " HAVING SUM(A.Nal) <> 0                                                      ";
                SQL = SQL + ComNum.VBLF + "  ORDER By a.BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i < intCnt; i++)
                {
                    lngAmt = 0;

                    nSQNal      = Convert.ToDouble(Dt.Rows[i]["SQNal"].ToString().Trim());
                    nMQty       = Convert.ToDouble(Dt.Rows[i]["SQTY"].ToString().Trim());
                    intNal      = Convert.ToInt32(Dt.Rows[i]["SNal"].ToString().Trim());
                    strBun      = Dt.Rows[i]["Bun"].ToString().Trim(); 
                    strGisul    = Dt.Rows[i]["GbGisul"].ToString().Trim();
                    strGbChild  = Dt.Rows[i]["GbChild"].ToString().Trim(); 
                    strNgt      = Dt.Rows[i]["GbNgt"].ToString().Trim();
                    strSuNext   = Dt.Rows[i]["Sunext"].ToString().Trim();
                    strER       = Dt.Rows[i]["GbEr"].ToString().Trim();

                    strSugbAD   = Dt.Rows[i]["SugbAD"].ToString().Trim();      
                    strGbSgAdd  = Dt.Rows[i]["GBSGADD"].ToString().Trim();    
                    strSugbAC   = Dt.Rows[i]["SugbAC"].ToString().Trim();           
                    strSuGbAB   = Dt.Rows[i]["SugbAB"].ToString().Trim();
                    strSuGbAA   = Dt.Rows[i]["SugbAA"].ToString().Trim();

                    nAge        = clsPmpaType.TIT.Age;
                    dAgeDay     = clsPmpaType.TIT.AgeDays;
                    
                    lngAmt = DRG.READ_DRG_ER_AMT(pDbCon, strSuNext, strBun, strGbChild, strGisul, nAge, dAgeDay, strNgt, nSQNal, intNal, clsPmpaType.TIT.InDate, nMQty, strER, strSugbAD, strGbSgAdd, strSugbAC, strSuGbAB, strSuGbAA);
                    
                    intRow = ++intRow;
                    if (intRow > Spd.RowCount)
                    {
                        Spd.RowCount = intRow;
                    }
                    
                    Spd.Cells[intRow - 1, 0].Text = Dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.Cells[intRow - 1, 1].Text = strSuNext;
                    Spd.Cells[intRow - 1, 2].Text = " " + Dt.Rows[i]["SuNameK"].ToString().Trim();
                    if (strGisul =="1")
                    {
                        Spd.Cells[intRow - 1, 3].Text = VB.Format(lngAmt*0.8, "#,##0");
                    }
                    else
                    {
                        Spd.Cells[intRow - 1, 3].Text = VB.Format(lngAmt, "#,##0");
                    }
                   
                    Spd.Cells[intRow - 1, 4].Text = VB.Format(nMQty, "#0.0");
                    Spd.Cells[intRow - 1, 5].Text = intNal.ToString(); 
                    Spd.Cells[intRow - 1, 6].Text = VB.Format(lngAmt, "#,##0");
                    Spd.Cells[intRow - 1, 7].Text = VB.Format(0, "#,##0");
                    Spd.Cells[intRow - 1, 8].Text = VB.Format(0, "#,##0");

                    lngTot2 = lngTot2 + lngAmt;
                }
                
                Dt.Dispose();
                Dt = null;

                if (Spd.RowCount > 0)
                {
                    intRow = ++intRow;
                    Spd.RowCount = intRow;
                    Spd.Cells[intRow - 1, 2].Text = " ** 합 계 ** ";
                    Spd.Cells[intRow - 1, 6].Text = VB.Format(lngTot2, "#,##0");
                }                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }

            
        }
        #endregion

        # region 부수술 Slip 스프레드 DisPlay // DisPlay_IPD_SlipDtl_부수술비용
        public void Rtn_Display_Ipd_SubOP(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, long lngTrsno)
        {
            int intCnt = 0;
            int intRow = 0;
            int i = 0;
            int intQty = 0;
            int intBi = 0;

            long lngBAmt = 0;
            long lngSAmt = 0;
            long lngTot2 = 0;

            double dbGisul = 0.0;

            string strBDate = "";
            string strSugbE = "";
            string strSuNext = "";

            DataTable Dt = null;
            DataTable Dt2 = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Sunext, a.BaseAmt, SUM(a.AMT1 + a.AMT2) SAMT,          ";
                SQL = SQL + ComNum.VBLF + "        SUM(a.Qty*a.Nal) NQty, a.GbGisul, b.SUNAMEK,             ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE                      ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                    ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                          ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.TRSNO = " + lngTrsno + "                               ";
                SQL = SQL + ComNum.VBLF + "    AND a.BUN IN ('34','35')                                     ";
                SQL = SQL + ComNum.VBLF + "    AND (a.GbNgt IN ('5','6','7') OR OPGUBUN IN ('1','2','D'))   ";
                SQL = SQL + ComNum.VBLF + "    AND a.Sunext=b.Sunext(+)                                     ";
                SQL = SQL + ComNum.VBLF + "  Group By a.Sunext,a.BaseAmt,a.GbGisul,a.BDATE,b.SUNAMEK        ";
                SQL = SQL + ComNum.VBLF + " Having SUM(a.Qty*a.Nal) <> 0                                    ";
                SQL = SQL + ComNum.VBLF + "  ORDER By a.BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i <= (intCnt - 1); i++)
                {
                    strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    strSugbE = Dt.Rows[i]["GbGisul"].ToString().Trim();
                    strSuNext = Dt.Rows[i]["Sunext"].ToString().Trim();

                    intBi = (int)VB.Val(VB.Mid(clsPmpaType.TIT.Bi, 1, 1));

                    //기술료율 계산
                    if (string.Compare(strSugbE, "0") > 0)
                    {
                        //기술료가산
                        if (string.Compare(strBDate, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[intBi] > 0)
                        {
                            dbGisul = clsPmpaPb.OLD_GISUL[intBi] / 100.0;
                        }
                        else
                        {
                            dbGisul = clsPmpaPb.GISUL[intBi] / 100.0;
                        }
                    }

                    intQty = Convert.ToInt32(VB.Val(Dt.Rows[i]["NQty"].ToString().Trim()));
                    lngBAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["BaseAmt"].ToString().Trim()));
                    lngSAmt = Convert.ToInt64(VB.Val(Dt.Rows[i]["SAMT"].ToString().Trim()));
                    
                    //SQL = "";
                    //SQL = SQL + ComNum.VBLF + " SELECT BAMT From BAS_SUGA_AMT                               ";
                    //SQL = SQL + ComNum.VBLF + "  Where SUNEXT = '" + strSuNext + "'                         ";
                    //SQL = SQL + ComNum.VBLF + "    AND SUDATE <= TO_DATE('" + strBDate + "','YYYY-MM-DD')   ";
                    //SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL                                      ";
                    //SQL = SQL + ComNum.VBLF + "  ORDER By SuDate DESC                                       ";
                    //SqlErr = clsDB.GetDataTable(ref Dt2, SQL, pDbCon);

                    //if (SqlErr != "")
                    //{
                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    //    return;
                    //}

                    //lngBAmt = (long)(Convert.ToInt64(VB.Val(Dt2.Rows[0]["BAmt"].ToString().Trim())) * intQty * dbGisul);

                    //Dt2.Dispose();
                    //Dt2 = null;

                    intRow += 1;
                    if (intRow > Spd.RowCount)
                    {
                        Spd.RowCount = intRow;
                    }

                    Spd.Cells[intRow - 1, 0].Text = strBDate;
                    Spd.Cells[intRow - 1, 1].Text = strSuNext;
                    Spd.Cells[intRow - 1, 2].Text = Dt.Rows[i]["SuNameK"].ToString().Trim();
                    Spd.Cells[intRow - 1, 3].Text = VB.Format(lngBAmt, "#,##0");
                    Spd.Cells[intRow - 1, 4].Text = VB.Format(intQty, "#0.0");
                    Spd.Cells[intRow - 1, 5].Text = VB.Format(1, "#0");
                    Spd.Cells[intRow - 1, 6].Text = VB.Format(lngBAmt, "#,##0");
                    Spd.Cells[intRow - 1, 7].Text = VB.Format(0, "#,##0");
                    Spd.Cells[intRow - 1, 8].Text = VB.Format(0, "#,##0");

                    lngTot2 = lngTot2 + lngBAmt;
                    
                }

                Dt.Dispose();
                Dt = null;

                intRow += 1;
                Spd.RowCount = intRow;
                Spd.Cells[intRow - 1, 2].Text = " ** 합 계 ** ";
                Spd.Cells[intRow - 1, 6].Text = VB.Format(lngTot2, "#,##0");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }
        }
        #endregion

        # region 추가입원료 Slip 스프레드 DisPlay // DisPlay_IPD_SlipDtl_추가입원료
        public void Rtn_Display_Ipd_AddCharge(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, long lngTrsno)
        {
            int intCnt = 0;
            int intRow = 0;
            int i = 0;
            int intQty = 0;
            int intBi = 0;

            long lngBAmt = 0;
            long lngTot2 = 0;

            //double dbGisul = 0.0;
            string[] strTrsDate = new string[3];
            string strBDate = "";
            string strSugbE = "";
            string strSuNext = "";
            string strRmCls = "";
            DataTable Dt = null;
            DataTable Dt2 = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, '1' intQty,   ";
                SQL += ComNum.VBLF + "        b.TRANSDATE1,b.RoomClass, b.OverAmt,                          ";
                SQL += ComNum.VBLF + "        b.TRANSDATE2,b.RoomClass1, b.OverAmt1,                        ";
                SQL += ComNum.VBLF + "        b.TRANSDATE3,b.RoomClass2, b.OverAmt2                         ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                         ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM b                              ";
                SQL += ComNum.VBLF + "  WHERE a.BDATE >= TO_DATE('2014-09-01','YYYY-MM-DD')                 ";
                SQL += ComNum.VBLF + "    AND a.RoomCode = b.RoomCode(+)                                    ";
                SQL += ComNum.VBLF + "    AND a.WardCode = b.WardCode(+)                                    ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + lngTrsno + "                                    ";
                SQL += ComNum.VBLF + "    AND b.RoomClass in ('M','K','G','H','A','B','C')                                   ";
                //SQL += ComNum.VBLF + "    AND a.SUCODE NOT IN ('AU312','AU214','AU204','AU302','AP601','AV222','AV820','V7000','AC421','AC321','AI700', 'IA221','AV2221','AU403','AU413','AH013','AC460','AI120','AH011','AC321','AC302') ";    
                SQL += ComNum.VBLF + "    AND a.SUCODE NOT IN ('AU312','AU214','AU204','AU302','AP601','AV222','AV820','V7000','AC421','AC321','AI700', ";
                SQL += ComNum.VBLF + "                         'IA221','AV2221','AU403','AU413','AH013','AC460','AI120','AH011','AC321','AC302','ID110','ID120','ID130') "; //결핵관리료, 상담료 2021-09-16
                SQL += ComNum.VBLF + "    AND a.BUN IN ('04','06')                                          ";   //환자관리료
                SQL += ComNum.VBLF + "  GROUP BY a.BDate, b.TRANSDATE1,b.RoomClass,b.OverAmt, b.TRANSDATE2,b.RoomClass1,b.OverAmt1, b.TRANSDATE3,b.RoomClass2,b.OverAmt2 ";
                SQL += ComNum.VBLF + "  HAVING SUM(A.QTY * A.NAL) > 0  ";
                if (clsPmpaType.TIT.InDate != clsPmpaType.TIT.OutDate)
                {
                    SQL += ComNum.VBLF + "  union all  ";

                    if (clsPmpaType.TIT.OutDate == "")
                    { SQL += ComNum.VBLF + " SELECT TO_CHAR(trunc(sysdate)  ,'YYYY-MM-DD') BDate, '1' intQty,   "; }
                    else
                    { SQL += ComNum.VBLF + " SELECT '" + clsPmpaType.TIT.OutDate + "' BDate, '1' intQty,   "; }

                    SQL += ComNum.VBLF + "        TRANSDATE1,RoomClass, OverAmt,                          ";
                    SQL += ComNum.VBLF + "        TRANSDATE2,RoomClass1, OverAmt1,                        ";
                    SQL += ComNum.VBLF + "        TRANSDATE3,RoomClass2, OverAmt2                         ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ROOM                         ";
                    SQL += ComNum.VBLF + "  WHERE RoomCode = '" + clsPmpaType.TIT.RoomCode + "'               ";

                    if (string.Compare(clsPmpaType.TIT.OutDate, "2019-07-01") >= 0)
                    { SQL += ComNum.VBLF + "    AND RoomClass in ('M','K','G','H','A','B','C')                      "; }
                    else
                    { SQL += ComNum.VBLF + "    AND RoomClass in ('M','K','G','H')                                   "; }

                }
                   
                
                SQL += ComNum.VBLF + "  ORDER BY BDATE              ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i <= (intCnt - 1); i++)
                {
                    strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    

                    intBi = (int)VB.Val(VB.Mid(clsPmpaType.TIT.Bi, 1, 1));

                
                    strTrsDate[0] = Dt.Rows[i]["TRANSDATE1"].ToString().Trim();
                    strTrsDate[1] = Dt.Rows[i]["TRANSDATE2"].ToString().Trim();
                    strTrsDate[2] = Dt.Rows[i]["TRANSDATE3"].ToString().Trim();

                    //기준일자 적용
                    if (string.Compare(strBDate, strTrsDate[0]) >= 0)
                    {
                        strRmCls = Dt.Rows[i]["RoomClass"].ToString().Trim();
                    }
                    else if (string.Compare(strBDate, strTrsDate[1]) >= 0)
                    {
                        strRmCls = Dt.Rows[i]["RoomClass1"].ToString().Trim();
                    }
                    else if (string.Compare(strBDate, strTrsDate[2]) >= 0)
                    {
                        strRmCls = Dt.Rows[i]["RoomClass2"].ToString().Trim();
                    }

                    intQty = Convert.ToInt32(VB.Val(Dt.Rows[i]["intQty"].ToString().Trim()));
                    if (strRmCls == "M")

                    { strSuNext = "AB220"; }

                    else if  (strRmCls == "K")
                    { strSuNext = "AB240"; }

                    else if (strRmCls == "G" || strRmCls == "H" )

                    { strSuNext = "AB270"; }
                    else if ((strRmCls == "A" || strRmCls == "B" || strRmCls == "C" ) && string.Compare(strBDate, "2019-07-01") >= 0)

                    { strSuNext = "AB901"; }



                    lngBAmt = ((Rtn_Edi_Suga_Amt(pDbCon, strSuNext, strBDate) * intQty) - (Rtn_Edi_Suga_Amt(pDbCon, "AB200", strBDate) * intQty));

                    intRow += 1;
                    if (intRow > Spd.RowCount)
                    {
                        Spd.RowCount = intRow;
                    }

                    Spd.Cells[intRow - 1, 0].Text = strBDate;
                    Spd.Cells[intRow - 1, 1].Text = strSuNext;
                    Spd.Cells[intRow - 1, 2].Text = Read_SunameK(pDbCon, strSuNext); ;
                    Spd.Cells[intRow - 1, 3].Text = VB.Format(lngBAmt, "#,##0");
                    Spd.Cells[intRow - 1, 4].Text = VB.Format(intQty, "#0.0");
                    Spd.Cells[intRow - 1, 5].Text = VB.Format(1, "#0");
                    Spd.Cells[intRow - 1, 6].Text = VB.Format(lngBAmt, "#,##0");
                    Spd.Cells[intRow - 1, 7].Text = VB.Format(0, "#,##0");
                    Spd.Cells[intRow - 1, 8].Text = VB.Format(0, "#,##0");

                    lngTot2 = lngTot2 + lngBAmt;

                }

                Dt.Dispose();
                Dt = null;

                intRow += 1;
                Spd.RowCount = intRow;
                Spd.Cells[intRow - 1, 2].Text = " ** 합 계 ** ";
                Spd.Cells[intRow - 1, 6].Text = VB.Format(lngTot2, "#,##0");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }
        }
        #endregion

        # region 재원 Slip 내역 스프레드 DisPlay // DisPlay_IPD_SlipDtl
        public void Rtn_Display_Ipd_SlipDtl(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, long lngTrsno, string strName)
        {
            int intCnt = 0;
            int intRow = 0;
            int i = 0;
            int intQty = 0;
            int intNal = 0;
            long lngBaseAmt = 0;
            long lngAmt = 0;
            long lngTot2 = 0;

            string strBCode = "";
            string strBDate = "";
            string strSuNext = "";
            string strSuNameK = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, a.BaseAmt, a.Sunext,b.SuNameK, b.BCODE,b.DRGF,    ";
                SQL = SQL + ComNum.VBLF + "        a.Qty SQty, SUM(a.Nal) SNal, SUM(a.Amt1+a.Amt2) SAmt, SUM(a.AMT1) * nvl(b.DRGBOSANG,0) Bosang            ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                                ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                                      ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.TRSNO = " + lngTrsno + "                                           ";
                switch (strName)
                {
                    case "의료질평가지원":
                        SQL = SQL + ComNum.VBLF + "  AND a.SUCODE IN ('AU204','AU302','AU403')                                  ";
                        break;
                    case "안전.야간.감염예방관리료":
                        SQL = SQL + ComNum.VBLF + "  AND a.SUCODE IN ('AC421','AH013','AC460','AI120','AH011','AC321')                                  ";
                        break;
                    case "간호간병료":
                        SQL = SQL + ComNum.VBLF + "  AND a.SUCODE IN ('AV222','AV2221','AV222A','AV2221A','AV820','AV820A' )              ";
                        break;
                    case "급여초음파":
                        SQL = SQL + ComNum.VBLF + "  AND a.BUN = '49' and gbself ='0'                                                 ";
                        break;
                    case "수정체제외":
                        SQL = SQL + ComNum.VBLF + "  AND a.SUCODE IN  ('OTDRG1','OTDRG2')                              ";
                        break;
                    case "별도보상(ADD)":
                        SQL = SQL + ComNum.VBLF + "  AND nvl(b.DRGBOSANG,0) >0                                          ";
                        break;
                    case "인정비급여":
                        SQL = SQL + ComNum.VBLF + "  AND (b.DRG100 = 'Y' or b.DRGF = 'Y')                               ";
                        break;
                    case "선별급여":
                        SQL = SQL + ComNum.VBLF + "  AND a.GBSUGBS IN ('2','3','4','5','6','7','8','9')  AND DRG100 IS NOT NULL    ";
                        SQL = SQL + ComNum.VBLF + "  AND a.BDate >= TO_DATE('2016-10-01','YYYY-MM-DD')                  ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "    AND a.Sunext=b.Sunext(+)                                                 ";
                SQL = SQL + ComNum.VBLF + "  Group By a.BDate, a.BaseAmt, a.Sunext,b.SuNameK, b.BCODE,a.Qty ,b.DRGF , nvl(b.DRGBOSANG,0)     ";
                SQL = SQL + ComNum.VBLF + " Having SUM(a.Qty*a.Nal) <> 0                                                ";
                SQL = SQL + ComNum.VBLF + "  ORDER By a.BDate ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                intCnt = Dt.Rows.Count;

                for (i = 0; i <= (intCnt - 1); i++)
                {
                    strBDate = Dt.Rows[i]["BDATE"].ToString().Trim();
                    strSuNext = Dt.Rows[i]["Sunext"].ToString().Trim();
                    if ( Dt.Rows[i]["DRGF"].ToString().Trim() == "Y")
                    {
                        strSuNameK = Dt.Rows[i]["SuNameK"].ToString().Trim() + "(비)";
                    }
                    else

                    {
                        strSuNameK = Dt.Rows[i]["SuNameK"].ToString().Trim();
                    }
                    strBCode = Dt.Rows[i]["BCODE"].ToString().Trim();
                    
                    lngBaseAmt = Convert.ToInt64(Dt.Rows[i]["BaseAmt"].ToString().Trim());

                    if (strName == "별도보상(ADD)")
                    {
                        lngAmt = Convert.ToInt64(Dt.Rows[i]["Bosang"].ToString().Trim());
                    }
                    else
                    {
                        lngAmt = Convert.ToInt64(Dt.Rows[i]["SAmt"].ToString().Trim());
                    }
                    
                    intQty = Convert.ToInt32(Dt.Rows[i]["SQty"].ToString().Trim());
                    intNal = Convert.ToInt32(Dt.Rows[i]["SNal"].ToString().Trim());
                    
                    intRow += 1;
                    if (intRow > Spd.RowCount)
                    {
                        Spd.RowCount = intRow;
                    }

                    Spd.Cells[intRow - 1, 0].Text = strBDate;
                    Spd.Cells[intRow - 1, 1].Text = strSuNext;
                    //Spd.Cells[intRow - 1, 2].Text = strBCode;
                    Spd.Cells[intRow - 1, 2].Text = " " + strSuNameK;
                    Spd.Cells[intRow - 1, 3].Text = VB.Format(lngBaseAmt, "#,##0");
                    Spd.Cells[intRow - 1, 4].Text = VB.Format(intQty, "#0.0");
                    Spd.Cells[intRow - 1, 5].Text = VB.Format(intNal, "#0");

                    Spd.Cells[intRow - 1, 6].Text = VB.Format(lngAmt, "#,##0");

                    lngTot2 = lngTot2 + lngAmt;

                }

                Dt.Dispose();
                Dt = null;

                intRow += 1;
                Spd.RowCount = intRow;
                Spd.Cells[intRow - 1, 2].Text = " ** 합 계 ** ";
                Spd.Cells[intRow - 1, 6].Text = VB.Format(lngTot2, "#,##0");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
            }
        }
        #endregion

        # region 마이너스 수가 조회 // CHK_MINUS_SUGA
        public string Chk_Minus_Suga(PsmhDb pDbCon, string strPano, long lngTrsno)
        {
            string strRtnMsg = "";
            int i = 0;
            int intCnt = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수 

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.BDATE, A.SUNEXT,  B.SUNAMEK,  SUM(A.AMT1) AMT  ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,            ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.PANO = '" + strPano + "'                       ";
                SQL = SQL + ComNum.VBLF + "    AND a.TRSNO = " + lngTrsno + "                       ";
                SQL = SQL + ComNum.VBLF + "    AND a.SUNEXT=b.SUNEXT                                ";
                SQL = SQL + ComNum.VBLF + "    AND a.BASEAMT <> 0                                   ";
                SQL = SQL + ComNum.VBLF + "  Group By A.BDATE, A.SUNEXT , B.SUNAMEK                 ";
                SQL = SQL + ComNum.VBLF + " Having SUM(A.AMT1) < 0                                  ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                intCnt = Dt.Rows.Count;

                for (i=0; i <= (intCnt-1); i++)
                {
                    strRtnMsg = strRtnMsg + Dt.Rows[i]["BDATE"].ToString().Trim() + " ";
                    strRtnMsg = strRtnMsg + Dt.Rows[i]["SUNEXT"].ToString().Trim() + " ";
                    strRtnMsg = strRtnMsg + Dt.Rows[i]["AMT"].ToString().Trim() + " ";
                    strRtnMsg = strRtnMsg + Dt.Rows[i]["SUNAMEK"].ToString().Trim() + " ";
                    strRtnMsg = strRtnMsg + ComNum.VBLF;
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region 병실예약 내용 조회 // READ_BAS_ROOM_RESERVED
        public string Rtn_Bas_Room_Reserved(PsmhDb pDbCon, string strWard, string strRoom, string strPano, string strSeq)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수 

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT RESERVED                                                 ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE WARDCODE = '" + strWard + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND ROOMCODE = '" + strRoom + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND ( PANO = '" + strPano + "' OR SEQNO = '" + strSeq + "' ) ";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL                                          ";

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";

                }
                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["RESERVED"].ToString().Trim();
                }
                
                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 병동코드 유효여부 RETURN BOOL
        /// <param name="strCode">병동코드</param>
        /// 2017-06-29 김민철
        /// </summary>
        public bool Chk_WardCode(PsmhDb pDbCon, string strCode)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT WardName ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL += ComNum.VBLF + "  WHERE WardCode='" + strCode + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["WARDNAME"].ToString().Trim() != "")
                    {
                        Dt.Dispose();
                        Dt = null;
                        return true;
                    }
                }

                Dt.Dispose();
                Dt = null;

                return false;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// 병실코드 유효여부 RETURN BOOL
        /// <param name="strWardCode">병동코드</param>
        /// <param name="strRoomCode">병실코드</param>
        /// 2017-06-29 김민철
        /// </summary>
        public string Chk_RoomCode(PsmhDb pDbCon, string strWardCode, string strRoomCode)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TBed ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE WardCode='" + strWardCode + "' ";
                SQL += ComNum.VBLF + "    AND RoomCode='" + strRoomCode + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["TBed"].ToString().Trim();
                    return rtnVal;
                    
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
        public string RTN_ER_AREA_INFO(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PTMIAREA ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTMIINDT  = '" + VB.Replace(ArgDate, "-", "") + "' ";
            SQL += ComNum.VBLF + "    AND PTMIIDNO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SEQNO     = (SELECT MAX(SEQNO) ";
            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI ";
            SQL += ComNum.VBLF + "                      WHERE PTMIINDT = '" + VB.Replace(ArgDate, "-", "") + "'";
            SQL += ComNum.VBLF + "                        AND PTMIIDNO = '" + ArgPtno + "') ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                switch (Dt.Rows[0]["PTMIAREA"].ToString().Trim())
                {
                    case "1":
                        rtnVal = "일반격리병상";
                        break;
                    case "2":
                        rtnVal = "음압격리병상";
                        break;
                    case "3":
                        rtnVal = "중증응급환자 진료구역";
                        break;
                    case "4":
                        rtnVal = "응급환자 진료구역";
                        break;
                    case "5":
                        rtnVal = "소생실";
                        break;
                    case "6":
                        rtnVal = "처치실";
                        break;
                    case "8":
                        rtnVal = "기타(1~6를 제외한 모든 구역)";
                        break;
                    case "9":
                        rtnVal = "미상";
                        break;
              
                }
            }
             

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        public string RTN_GBBSSiMSA_INFO(PsmhDb pDbCon, long ArgTrsno,long ArgIpdno )
        {
            //분석심사 대상자 확인 ,CP대상자 확인
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT '분석심사' SUNEXT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";

            SQL += ComNum.VBLF + "    AND trsno  = " + ArgTrsno + " ";
            SQL += ComNum.VBLF + "    AND sunext     in (select sunext from bas_sun a where GBBSSimSa is not null) ";
            SQL += ComNum.VBLF + "    GROUP By sunext  Having SUM(Qty*Nal) > 0  ";
            SQL += ComNum.VBLF + " union all  ";
            SQL += ComNum.VBLF + " SELECT 'CP대상자' SUNEXT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ocs_cp_record ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ipdno  = " + ArgIpdno + " ";
            SQL += ComNum.VBLF + "    and CANCERDATE is null       ";
            SQL += ComNum.VBLF + "    and DROPDATE is null ";

            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = Dt.Rows[0]["SUNEXT"].ToString().Trim();

            }


            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        public string Chk_VFCode_Info(PsmhDb pDbCon, long strTrsno)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE TRSNO=" + strTrsno + " ";
        
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "P" || Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "O")
                    {
                        // '2015-05-18 입원명령결핵 추가
                        if (Dt.Rows[0]["FCODE"].ToString().Trim() == "F008")
                        {
                            rtnVal = Dt.Rows[0]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "F010")
                        {
                            //'2015-06-30
                            rtnVal = Dt.Rows[0]["VCODE"].ToString().Trim() + "+F010";
                        }
                        else                        {
                            rtnVal = "면제";

                            if ( Dt.Rows[0]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V231")
                            {
                                rtnVal = " ★결핵★";
                            }
                        }
                    }
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        rtnVal = "중증E+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        rtnVal = "중증F+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        //'2013-02-15
                        rtnVal = "E+V";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        //'2013-02-15
                        rtnVal = "F+V";
                    }
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        rtnVal = "중증E+V+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        rtnVal = "중증F+V+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    //'V268 뇌출혈추가
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V191" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V192" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V193" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V194" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V268" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V275")
                    {
                        rtnVal = "중증+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if ( Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "E" && Dt.Rows[0]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V231")
                    {
                        //'2015-05-18 입원명령결핵 추가
                        if (Dt.Rows[0]["FCODE"].ToString().Trim() == "F008")
                        {
                            rtnVal = "차상위E(" + Dt.Rows[0]["VCODE"].ToString().Trim() + "+F008)";
                        }
                        else
                        {
                            rtnVal = "차상위E+★결핵★";
                        }
                    }

                    else if ( Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "F" && Dt.Rows[0]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V231")
                    {
                        //'2015-05-18 입원명령결핵 추가
                        if (Dt.Rows[0]["FCODE"].ToString().Trim() == "F008")
                        {
                            rtnVal = "차상위F(" + Dt.Rows[0]["VCODE"].ToString().Trim() + "+F008)";
                        }
                        else
                        {
                            rtnVal = "차상위F+★결핵★";
                        }

                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "E" && Dt.Rows[0]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V250")
                    {
                        rtnVal = "중증화상E+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "F" && Dt.Rows[0]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V250")
                    {
                        rtnVal = "중증화상F+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V250")
                    {
                        rtnVal = "중증화상+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "H")
                    {
                        if (Dt.Rows[0]["VCODE"].ToString().Trim() == "")
                        {
                            rtnVal = "희귀H";
                        }
                        else
                        {
                            rtnVal = "희귀H+" + Dt.Rows[0]["VCODE"].ToString().Trim();
                        }
                          
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "V")
                    {
                        rtnVal = "희귀V";

                        if (Dt.Rows[0]["FCODE"].ToString().Trim() == "F008")
                        {
                            rtnVal = Dt.Rows[0]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else
                        {
                            rtnVal = "희귀V+";

                            if ( Dt.Rows[0]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[0]["VCODE"].ToString().Trim() == "V231")
                            {
                                rtnVal = " ★결핵★";
                            }
                            else
                            {
                                rtnVal = rtnVal + Dt.Rows[0]["VCODE"].ToString().Trim();
                            }
                        }

                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "C")
                    {
                        rtnVal = "차상C";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        if (Dt.Rows[0]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            rtnVal = "차상E+" + Dt.Rows[0]["OGPDBUNdtl"].ToString().Trim() + " " + Dt.Rows[0]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal = "차상E" + Dt.Rows[0]["VCODE"].ToString().Trim();
                        }
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        if (Dt.Rows[0]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            rtnVal = "차상F+" + Dt.Rows[0]["OGPDBUNdtl"].ToString().Trim() + " " + Dt.Rows[0]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            rtnVal = "차상F" + Dt.Rows[0]["VCODE"].ToString().Trim();
                        }
                    }
                    else if ( Dt.Rows[0]["FCODE"].ToString().Trim() == "F014")
                    {
                        rtnVal = "F014";
                    }
                    else if (Dt.Rows[0]["FCODE"].ToString().Trim() == "F013")
                    {
                        rtnVal = "F013 제왕절개";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "O")
                    {
                        rtnVal = "자연분만";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "T")
                    {
                        rtnVal = "고위험임산부";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "P")
                    {
                        rtnVal = "소아P면제 F005,F019";
                    }
                    else if (Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "S"  )
                    {
                        rtnVal = "소아S";
                    }
                    else if ( Dt.Rows[0]["OGPDBUN"].ToString().Trim() == "Y")
                    {
                        rtnVal = "소아6-15세 F018,F020";
                    }

                    else if (Dt.Rows[0]["FCODE"].ToString().Trim() == "MT04")
                    {
                        rtnVal = "신종코로나";
                    }

                    //rtnVal = Dt.Rows[0]["TBed"].ToString().Trim();
                    return rtnVal;

                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }


        #region 상급병실 사용여부
        public bool Chk_RoomCha_Class(PsmhDb pDbCon, string strRoom)
        {
            bool rtnVal = false;
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TBED, ROOMCLASS ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL += ComNum.VBLF + "  WHERE RoomCode=" + strRoom + " ";
                SQL += ComNum.VBLF + "    AND TBed > 0 AND TBed < 4 ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                else
                {
                    rtnVal = false;
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Description : 항결핵약제비용 합산
        /// Author : 김민철
        /// Create Date : 2017.09.21
        /// <param name="ArgTrsNo"></param>
        /// </summary>
        /// <seealso cref="IpdTewon.bas : GESAN_항결핵약제비"/> 
        public long Gesan_AntiTubeDrug_Amt(PsmhDb pDbCon, long ArgTrsNo)
        {
            int i = 0;
            long rtnVal = 0;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT a.Sunext,SUM(a.Amt1+a.Amt2) SAMT      ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b       ";
                SQL += ComNum.VBLF + "  WHERE a.Sunext=b.Sunext(+)                  ";
                SQL += ComNum.VBLF + "    AND a.TRSNO = " + ArgTrsNo + "            ";
                SQL += ComNum.VBLF + "    AND b.GBTB ='Y'                           ";
                SQL += ComNum.VBLF + "  Group By a.Sunext                           ";
                SQL += ComNum.VBLF + " Having SUM(a.Qty*a.Nal) > 0                  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                if (Dt.Rows.Count > 0)
                {
                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        rtnVal += Convert.ToInt64(Dt.Rows[i]["SAMT"].ToString());
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
                return 0;
            }
            
        }

        /// <summary>
        /// Description : 수가명 읽어오기
        /// Author : 김민철
        /// Create Date : 2018.02.12
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuNext"></param>
        /// <returns></returns>
        public string Read_SunameK(PsmhDb pDbCon, string ArgSuNext)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수 

            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SuNameK                          ";
                SQL = SQL + ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_SUN    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + ArgSuNext + "'     ";
                
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["SuNameK"].ToString().Trim(); 
                }
                
                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }

        /// <summary>
        /// Description : 수가테이블에서 원하는 컬럼 값 읽어오기
        /// Author : 김민철
        /// Create Date : 2018.03.07
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSuNext"></param>
        /// <param name="strColName"></param>
        /// <returns></returns>
        public string Read_Bas_Sun_ColName(PsmhDb pDbCon, string strSuNext, string strColName, string strSucode)  //2018-10-29 
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT " + strColName + "                       ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE     ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                    ";
                SQL = SQL + ComNum.VBLF + "    AND SUNEXT = '" + strSuNext + "'             ";
                SQL = SQL + ComNum.VBLF + "    AND SUCODE = '" + strSucode + "'             ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0][strColName].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }

        }

        # region BAS_SUT B항 조회 
        public string Rtn_Bas_Sut_B(PsmhDb pDbCon, string strSuNext)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SugbB                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + strSuNext + "'     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["SugbB"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion

        # region BAS_SUH B항 조회 
        public string Rtn_Bas_SuH_B(PsmhDb pDbCon, string strSuNext)
        {
            string strRtnMsg = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SugbB                            ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUH    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                            ";
                SQL = SQL + ComNum.VBLF + "    AND SuNext = '" + strSuNext + "'     ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (Dt.Rows.Count > 0)
                {
                    strRtnMsg = Dt.Rows[0]["SugbB"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return strRtnMsg;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, pDbCon);
                return "";
            }
        }
        #endregion
    }
}
