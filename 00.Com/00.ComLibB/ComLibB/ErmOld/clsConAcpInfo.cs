using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// GbTextEmrCom.bas
    /// </summary>
    public class clsConAcpInfo
    {
        /// <summary>
        /// GbTextEmrCom.bas : NEW_TextEMR_TreatInterface
        /// </summary>
        /// <param name="strPatid">등록번호</param>
        /// <param name="strBDate">발생일자</param>
        /// <param name="strDeptCode">과</param>
        /// <param name="strGubun">입원,외래</param>
        /// <param name="strSTS">정상,취소</param>
        /// <param name="strDrCode">의사코드 취소시사용</param>
        /// <returns></returns>
        public static bool NEW_TextEMR_TreatInterface(string strPatid, string strBDate, string strDeptCode, string strGubun, string strSTS, string strDrCode, PsmhDb pDbCon)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            DataTable dt2 = null;
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            bool rtnVal = false;
            int intRowAffected = 0; //변경된 Row 받는 변수
            int nREAD = 0;

            string strOutDate = "";
            string strJumin = ""; //주민 암호화
            string strDept = "";
            string strOK = "";
            
            strBDate = VB.Format(strBDate, "YYYYMMDD");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1,P.JUMIN2,P.JUMIN3, E.PATID , E.ROWID";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT  P , " + ComNum.DB_EMR + "EMR_PATIENTT E";
                SQL = SQL + ComNum.VBLF + "     WHERE E.PATID (+)=P.PANO";
                SQL = SQL + ComNum.VBLF + "         AND P.PANO ='" + strPatid.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                if (strSTS == "취소")
                {
                    dt.Dispose();
                    dt = null;

                    strOutDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "D"), "D", "");

                    if (strGubun == "HR" || strGubun == "TO")
                    {
                        SQL =                           "SELECT TREATNO, ROWID";
                        SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                        SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + strPatid + "' ";
                        SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + strBDate + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLINCODE = '" + strDeptCode + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLASS = '0'";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        nREAD = dt.Rows.Count;
                        if (nREAD > 0)
                        {
                            for (i = 0; i < nREAD; i++)
                            {
                                SQL =                       "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                                SQL = SQL + ComNum.VBLF +   "DELDATE = '" + strOutDate + "'"; //2009-09-07 윤조연 수정
                                SQL = SQL + ComNum.VBLF +   "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

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

                        dt.Dispose();
                        dt = null;

                    }
                    else
                    {
                        // 외래, 입원
                        SQL =                           "SELECT TREATNO, ROWID";
                        SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                        SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + strPatid + "' ";
                        SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + strBDate + "'";
                        if (strDeptCode == "MD" && (strDeptCode == "1107" || strDeptCode == "1125"))    //내과 오동호 과장은 RA로 2009-09-17 윤조연
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CLINCODE = 'RA'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CLINCODE = '" + strDeptCode + "'";
                        }
                        if (strGubun == "외래")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CLASS = 'O'";
                        }
                        else if (strGubun == "입원")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CLASS = 'I'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CLASS = ''";
                        }

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        nREAD = dt.Rows.Count;

                        if (nREAD > 0)
                        {
                            for (i = 0; i < nREAD; i++)
                            {
                                SQL =                       "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                                SQL = SQL + ComNum.VBLF +   "DELDATE = '" + strOutDate + "'"; //2009-09-07 윤조연 수정
                                SQL = SQL + ComNum.VBLF +   "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

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

                        dt.Dispose();
                        dt = null;

                    }
                }

                if (dt.Rows[0]["PATID"].ToString().Trim() == "")    //EMR_PATIENTT 테이블에 환자가 없다.
                {
                    strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + VB.Left(dt.Rows[0]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL =                           "INSERT INTO " + ComNum.DB_EMR + "EMR_PATIENTT(PATID, JUMINNO, NAME, SEX) ";
                    SQL = SQL + ComNum.VBLF +               "VALUES('" + dt.Rows[0]["PANO"].ToString().Trim() + "' ,";
                    SQL = SQL + ComNum.VBLF +               " '" + strJumin + "', ";
                    SQL = SQL + ComNum.VBLF +               " '" + dt.Rows[0]["sName"].ToString().Trim() + "', ";
                    SQL = SQL + ComNum.VBLF +               " '" + dt.Rows[0]["Sex"].ToString().Trim() + "') ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    return true;
                }
                else
                {
                    strJumin = dt.Rows[i]["Jumin1"] + VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";

                    SQL =                           "UPDATE " + ComNum.DB_EMR + "EMR_PATIENTT" + " ";
                    SQL = SQL + ComNum.VBLF +       "SET NAME ='" + dt.Rows[i]["sName"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF +       ", SEX  ='" + dt.Rows[i]["Sex"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF +       ", JUMINNO ='" + strJumin + "' ";
                    SQL = SQL + ComNum.VBLF +               "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    return true;
                }

                dt.Dispose();
                dt = null;

                if (strGubun == "외래")
                {
                    SQL = "";
                    SQL = SQL +                     "SELECT m.pano, TO_CHAR(M.BDATE, 'YYYYMMDD') Bdate ,m.deptcode, d.sabun, M.ROWID";
                    SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_PMPA + "opd_master m, " + ComNum.DB_MED + "ocs_doctor d ";
                    SQL = SQL + ComNum.VBLF +       "WHERE d.drcode = m.drcode";
                    SQL = SQL + ComNum.VBLF +               "AND M.BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF +               "AND  m.PANO = '" + strPatid.ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF +               "AND  m.DeptCode = '" + strDeptCode.ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF +               "AND (m.EMR ='0' OR m.EMR IS NULL )";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    nREAD = dt.Rows.Count;

                    if (nREAD == 0)
                    {
                        dt.Dispose();
                        dt = null;

                        SQL =                           "SELECT DrSabun as Sabun, DeptCode, TO_CHAR(BDATE, 'YYYYMMDD') Bdate,Pano,ROWID ";
                        SQL = SQL + ComNum.VBLF +       "From " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL = SQL + ComNum.VBLF +       "Where Pano ='" + strPatid.ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF +               "AND DeptCode = '" + strDeptCode.ToString().Trim() + "'  ";
                        SQL = SQL + ComNum.VBLF +               "AND (EMR ='0' OR EMR IS NULL )";
                        SQL = SQL + ComNum.VBLF +               "AND BDATE >= TO_DATE('2009-07-07', 'YYYY-MM-DD')";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        nREAD = dt.Rows.Count;
                    }

                    for (i = 0; i < nREAD; i++)
                    {
                        if (strDeptCode == "MD" && dt.Rows[i]["Sabun"].ToString().Trim() == "19094" || dt.Rows[i]["Sabun"].ToString().Trim() == "30322")
                        {
                            strDept = "RA";
                        }

                        else
                        {
                            strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        }

                        SQL =                           "SELECT TREATNO, ROWID  FROM " + ComNum.DB_EMR + "EMR_TREATT";
                        SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + dt.Rows[i]["Date"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLINCODE = '" + strDept + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLASS = 'O'";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            SQL =                           "INSERT INTO " + ComNum.DB_EMR + "EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                            SQL = SQL + ComNum.VBLF +       "ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED)";
                            SQL = SQL + ComNum.VBLF +               "values( " + ComNum.DB_EMR + "SEQ_TREATNO.NEXTVAL, '" + strPatid.ToString().Trim() + "' ,";
                            SQL = SQL + ComNum.VBLF +               "'O' ,";  //CLASS
                            SQL = SQL + ComNum.VBLF +               "'" + dt.Rows[i]["Date"].ToString().Trim() + "' ,"; //INDATE
                            SQL = SQL + ComNum.VBLF +               "'" + strDept + "' ,";    //CLINCODE 2009-09-17 윤조연수정
                            SQL = SQL + ComNum.VBLF +               "'' ,";   //OUTDATE
                            SQL = SQL + ComNum.VBLF +               "'" + VB.Val(dt.Rows[i]["Sabun"].ToString().Trim()) + "',"; //DOCCODE
                            SQL = SQL + ComNum.VBLF +               "'0', ";  //ERFLAG
                            SQL = SQL + ComNum.VBLF +               "'000000', "; //INITTIME
                            SQL = SQL + ComNum.VBLF +               "'" + strPatid.ToString().Trim() + "', "; //OLDPATID
                            SQL = SQL + ComNum.VBLF +               "'2', ";  //FST
                            SQL = SQL + ComNum.VBLF +               "'', ";   //WARD
                            SQL = SQL + ComNum.VBLF +               "'', ";   //ROOM
                            SQL = SQL + ComNum.VBLF +               "'1' )";  //COMPLETE

                            clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            dt2.Dispose();
                            dt2 = null;

                            SQL =                       "UPDATE " + ComNum.DB_PMPA + "opd_master SET   EMR = '1'";
                            SQL = SQL + ComNum.VBLF +   "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                    return true;
                }

                else if (strGubun == "입원")
                {
                    //입원
                    SQL =                     "SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,  D.SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ipd_new_master S, kosmos_ocs.ocs_doctor d";
                    SQL = SQL + ComNum.VBLF + "WHERE S.DrCode = d.drcode";
                    SQL = SQL + ComNum.VBLF +            "AND S.PANO = '" + strPatid + "' ";
                    SQL = SQL + ComNum.VBLF +            "AND S.DeptCode = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF +            "AND (S.EMR = '0'  OR S.EMR IS NULL)";    //나중에 적용

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (strPatid == "06734999")
                    {
                        strPatid = strPatid;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }



                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        strOK = "OK";

                        if (strDeptCode == "MD" && dt.Rows[i]["Sabun"].ToString().Trim() == "19094" || dt.Rows[i]["Sabun"].ToString().Trim() == "30322")
                        {
                            strDept = "RA";
                        }
                        else
                        {
                            strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        }

                        SQL =                           "SELECT TREATNO, ROWID  FROM " + ComNum.DB_EMR + "EMR_TREATT";
                        SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + strPatid + "'";
                        SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + dt.Rows[i]["nDate"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLINCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF +               "AND CLASS = 'I'";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            SQL =                           "INSERT INTO " + ComNum.DB_EMR + "EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE,  DOCCODE, ";
                            SQL = SQL + ComNum.VBLF +       "ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED )";
                            SQL = SQL + ComNum.VBLF +               "VALUES( " + ComNum.DB_EMR + "SEQ_TREATNO.NEXTVAL, '" + strPatid.ToString().Trim() + "' , ";
                            SQL = SQL + ComNum.VBLF +               "'I' , "; //CLASS
                            SQL = SQL + ComNum.VBLF +               "'" + dt.Rows[i]["InDate"] + "' , ";  //INDATE
                            SQL = SQL + ComNum.VBLF +               "'" + strDept + "' , ";   //CLINCODE
                            SQL = SQL + ComNum.VBLF +               "'" + VB.Val(dt.Rows[i]["Sabun"].ToString().Trim()) + "', ";    //DOCCODE
                            SQL = SQL + ComNum.VBLF +               "'0', ";  //ERFLAG
                            SQL = SQL + ComNum.VBLF +               "'000000', "; //INITTIME
                            SQL = SQL + ComNum.VBLF +               "'" + strPatid.ToString().Trim() + "', "; //OLDPATID
                            SQL = SQL + ComNum.VBLF +               "'2', ";  //FST
                            SQL = SQL + ComNum.VBLF +               "'', ";   //WARD
                            SQL = SQL + ComNum.VBLF +               "'', ";   //ROOM
                            SQL = SQL + ComNum.VBLF +               "'1')";   //COMPLETE

                            clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            SQL =                           "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                            SQL = SQL + ComNum.VBLF +       "DELDATE ='', ";  //2009-09-07 윤조연수정
                            SQL = SQL + ComNum.VBLF +       "DOCCODE = '" + VB.Val(dt.Rows[i]["Sabun"].ToString().Trim()) + "' ,";
                            SQL = SQL + ComNum.VBLF +       "OUTDATE = '" + dt.Rows[i]["OutDate"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF +       "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";

                            clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                    dt.Dispose();
                    dt = null;

                    return true;
                }

                else if (strGubun == "HR" || strGubun == "TO")
                {
                    SQL =                           "SELECT TREATNO, ROWID";
                    SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                    SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + strPatid + "' ";
                    SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + strBDate + "'";
                    SQL = SQL + ComNum.VBLF +               "AND CLINCODE = '" + strDeptCode + "'";
                    SQL = SQL + ComNum.VBLF +               "AND CLASS = 'O'";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        SQL =                           "INSERT INTO " + ComNum.DB_EMR + "EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL = SQL + ComNum.VBLF +       "ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                        SQL = SQL + ComNum.VBLF +               ") values( KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + strPatid.ToString().Trim() + "' ,";
                        SQL = SQL + ComNum.VBLF +               "'O' , "; //CLASS
                        SQL = SQL + ComNum.VBLF +               "'" + strBDate + "' ,";   //INDATE
                        SQL = SQL + ComNum.VBLF +               "'" + strDeptCode + "' ,";    //CLINCODE 2009-09-17 윤조연수정
                        SQL = SQL + ComNum.VBLF +               "'' , ";  //OUTDATE
                        SQL = SQL + ComNum.VBLF +               "'" + strDrCode + "', ";  //DOCCODE = 종검,검진은 의사사번을 받아옴
                        SQL = SQL + ComNum.VBLF +               "'0', ";  //ERFLAG
                        SQL = SQL + ComNum.VBLF +               "'000000', "; //INITTIME
                        SQL = SQL + ComNum.VBLF +               "'" + strPatid.ToString().Trim() + "', "; //OLDPATID
                        SQL = SQL + ComNum.VBLF +               "'2', ";  //FST
                        SQL = SQL + ComNum.VBLF +               "'', ";   //WARD
                        SQL = SQL + ComNum.VBLF +               "'', ";   //ROOM
                        SQL = SQL + ComNum.VBLF +               "'1' )";  //COMPLETE

                        clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    else
                    {
                        SQL =                           "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                        SQL = SQL + ComNum.VBLF +       "DELDATE ='', ";  //2009-09-07 윤조연수정
                        SQL = SQL + ComNum.VBLF +       "DOCCODE = '" + strDrCode + "'";
                        SQL = SQL + ComNum.VBLF +       "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                        clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    return true;
                }

                else if (strGubun == "접종")
                {
                    SQL =                           "SELECT TREATNO, ROWID";
                    SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_EMR + "EMR_TREATT";
                    SQL = SQL + ComNum.VBLF +       "WHERE PATID = '" + strPatid + "' ";
                    SQL = SQL + ComNum.VBLF +               "AND INDATE  ='" + strBDate + "'";
                    SQL = SQL + ComNum.VBLF +               "AND CLINCODE = '" + strDeptCode + "'";
                    SQL = SQL + ComNum.VBLF +               "AND CLASS = 'O'";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        SQL =                           "INSERT INTO " + ComNum.DB_EMR + "EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL = SQL + ComNum.VBLF +       "ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED)";
                        SQL = SQL + ComNum.VBLF +               "values( " + ComNum.DB_EMR + "SEQ_TREATNO.NEXTVAL, '" + strPatid.ToString().Trim() + "' ,";
                        SQL = SQL + ComNum.VBLF +               "'O' , "; //CLASS
                        SQL = SQL + ComNum.VBLF +               "'" + strBDate + "' , ";  //INDATE
                        SQL = SQL + ComNum.VBLF +               "'" + strDeptCode + "' , ";   //CLINCODE 2009-09-17 윤조연수정
                        SQL = SQL + ComNum.VBLF +               "'' , "; //OUTDATE
                        SQL = SQL + ComNum.VBLF +               "'" + strDrCode + "', ";  //DOCCODE = 접종은 의사사번을 받아옴
                        SQL = SQL + ComNum.VBLF +               "'0', ";  //ERFLAG
                        SQL = SQL + ComNum.VBLF +               "'000000', "; //INITTIME
                        SQL = SQL + ComNum.VBLF +               "'" + strPatid.ToString().Trim() + "', "; //OLDPATID
                        SQL = SQL + ComNum.VBLF +               "'2', ";  //FST
                        SQL = SQL + ComNum.VBLF +               "'', ";   //WARD
                        SQL = SQL + ComNum.VBLF +               "'', ";   //ROOM
                        SQL = SQL + ComNum.VBLF +               "'1' )";  //COMPLETE


                        clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    else
                    {
                        SQL =                           "UPDATE " + ComNum.DB_EMR + "EMR_TREATT SET";
                        SQL = SQL + ComNum.VBLF +       "DELDATE ='', ";  //2009-09-07 윤조연수정
                        SQL = SQL + ComNum.VBLF +       "DOCCODE = '" + strDrCode + "'";
                        SQL = SQL + ComNum.VBLF +       "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "'";

                        clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                    return true;
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
                Cursor.Current = Cursors.Default;
            }

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPatid">등록번호</param>
        /// <param name="strBDate">발생일자</param>
        /// <param name="strDeptCode">과</param>
        /// <param name="strDeptCode2">변경과</param>
        /// <param name="strDrCode">의사</param>
        /// <param name="strDrCode2">변경의사</param>
        /// <returns></returns>
        public static bool NEW_TextEMR_TRANSFOR(string strPatid, string strBDate, string strDeptCode, string strDeptCode2, string strDrCode, string strDrCode2, clsTrans TRS)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJumin = "";   //주민암호화
            string strOK = "";
            string strDrCode2Sabun = "";
            string strDept = "";

            strBDate = VB.Format(strBDate, "YYYYMMDD");

            try
            {
                SQL =                           "SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1 ,P.JUMIN2,  P.JUMIN3, E.PATID , E.ROWID";
                SQL = SQL + ComNum.VBLF +       "FROM " + ComNum.DB_PMPA + "BAS_PATIENT  P , " + ComNum.DB_EMR + "EMR_PATIENTT E";
                SQL = SQL + ComNum.VBLF +       "WHERE E.PATID (+)=P.PANO";
                SQL = SQL + ComNum.VBLF +               "AND P.PANO ='" + strPatid.ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                dt.Dispose();
                dt = null;

                // 입원
                SQL = "SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.DeptCode, S.ROWID,D.SABUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ipd_new_master S , kosmos_ocs.ocs_doctor d";
                SQL = SQL + ComNum.VBLF + "WHERE S.DrCode = d.drcode";
                SQL = SQL + ComNum.VBLF + "AND S.PANO = '" + strPatid + "'";
                SQL = SQL + ComNum.VBLF + "AND TRUNC(S.InDate) = TO_DATE('" + strBDate + "', 'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "AND S.EMR ='1'"; //처리된것만 전실전과처리

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                    SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
