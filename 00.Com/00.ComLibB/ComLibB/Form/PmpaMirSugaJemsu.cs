using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : PmpaMir.dll
    /// File Name       : PmpaMirJemsu.cs
    /// Description     : 상대가치 점수
    /// Author          : 최익준
    /// Create Date     : 2017-09-01
    /// Update History  : 
    /// </summary>
    /// <vbp>
    /// default : Z:\차세대 의료정보시스템\1-0 착수단계\1-5 참고 소스\포항성모병원 VB Source(2017.01.11)_분석설계제작용\basic\busuga\BuSuga43.frm
    /// </vbp>
    /// 
    public partial class PmpaMirSugaJemsu : Form
    {
        public PmpaMirSugaJemsu()
        {
            InitializeComponent();
        }

        private void PmpaMirSugaJemsu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        private void btnJob1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int intRowAffected = 0; //변경된 Row 받는 변수

            int i = 0;
            int j = 0;
            int nREAD = 0;
            string strCODE = "";
            string strFCode = "";
            string strTCode = "";
            string strJDate = "";
            double nJemsu = 0;
            double nPrice = 0;
            string nSeqNo = "";
            string strBunno = "";
            string strRemark = "";
            string strAllCode = "";

            strJDate = "2001-01-01";

            Cursor.Current = Cursors.WaitCursor;

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = "SELECT * ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU_WORK";
                SQL = SQL + ComNum.VBLF + "WHERE BCode IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    strCODE = dt.Rows[i]["BCode"].ToString().Trim();
                    strCODE = VB.Pstr(strCODE, "(", 1);
                    strCODE = strCODE.Replace("-", " ");
                    nJemsu = VB.Val(dt.Rows[i]["Jemsu"].ToString().Trim());
                    nPrice = VB.Val(dt.Rows[i]["BAmt"].ToString());
                    nSeqNo = dt.Rows[i]["SeqNo"].ToString().Trim();
                    strBunno = dt.Rows[i]["BunNo"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    if (VB.I(strCODE, " ") > 1)
                    {
                        #region GoSub CmdJob1_Main
                        //Form, To형태로 자료가 있는것을 처리함
                        strAllCode = strCODE.Trim();

                        for (j = 0; j < VB.I(strAllCode, " "); j++)
                        {
                            strCODE = VB.Pstr(strAllCode, " ", j).Trim();

                            if (VB.I(strCODE, "~") > 1)
                            {
                                strFCode = VB.Pstr(strCODE, "~", 1);
                                strTCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Pstr(strCODE, "~", 2);
                            }
                            else
                            {
                                strFCode = strCODE;
                                strTCode = strCODE;
                            }

                            while (true)
                            {
                                if (strFCode == "")
                                {
                                    break;
                                }

                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark)";
                                SQL = SQL + ComNum.VBLF + "VALUES ('" + strFCode + "',TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + nJemsu + "," + nPrice + "," + nSeqNo + ",'" + strBunno + "','" + strRemark + "') ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(":BAS_SUGAJEMSU에 자료를 INSERT시 오류가 발생함", "확인");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //끝자리를 ADD
                                strFCode = VB.Left(strFCode, strFCode.Length - 1) + VB.Format(VB.Val(VB.Right(strFCode, 1)) + 1, "0");

                                if (VB.Val(strFCode) > VB.Val(strTCode) || VB.Right(strFCode, 1) == "0")
                                {
                                    break;
                                }
                            }
                        }
                        #endregion
                    }

                    else if (VB.I(strCODE, "~") > 1)
                    {
                        #region GoSub CmdJob1_Sub
                        //Form, To형태로 자료가 있는것을 처리함
                        strFCode = VB.Pstr(strCODE, "~", 1).Trim();
                        strTCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Pstr(strCODE, "~", 2).Trim();

                        for(;;)
                        {

                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark)";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strFCode + "',TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + nJemsu + "," + nPrice + "," + nSeqNo + ",'" + strBunno + "','" + strRemark + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(":BAS_SUGAJEMSU에 자료를 INSERT시 오류가 발생함", "확인");
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //끝자리를 ADD
                            strFCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Format(VB.Val(VB.Right(strFCode, 1)) + 1, "0");

                            if (VB.Val(strFCode) > VB.Val(strTCode) || VB.Right(strFCode, 1) == "0")
                            {
                                break;
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";
                        SQL = SQL + ComNum.VBLF + "     (BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + strCODE + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "         " + nJemsu + ", ";
                        SQL = SQL + ComNum.VBLF + "         " + nPrice + ", ";
                        SQL = SQL + ComNum.VBLF + "         " + nSeqNo + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + strBunno + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strRemark + "' ";
                        SQL = SQL + ComNum.VBLF + "     ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(":BAS_SUGAJEMSU에 자료를 INSERT시 오류가 발생함", "확인");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    txtState.Text = "( " + i + " )";
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("정상적으로 종료됨.", "확인");
                Cursor.Current = Cursors.Default;

                return;
            }

            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnXray_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (SaveXray() == true)
            {
                ComFunc.MsgBox("정상적으로 종료됨", "확인");
            }
        }

        private bool SaveXray()
        {
            int i = 0;
            int k = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            int nREAD = 0;
            string strBCode = "";
            string strNewBCode = "";
            double nJemsu = 0;
            double nPrice = 0;
            string strBunno = "";
            string strRemark = "";
            bool rtnVal = false;
            
            Cursor.Current = Cursors.WaitCursor;

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            //방사선 단순촬영 상대가치 점수를 READ

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     BCode,Jemsu1,Price1,BunNo,Remark";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";
                SQL = SQL + ComNum.VBLF + "     WHERE BCode >= 'G1001' ";
                SQL = SQL + ComNum.VBLF + "         AND BCode <= 'G9999'";
                SQL = SQL + ComNum.VBLF + "ORDER BY BCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nJemsu = VB.Val(dt.Rows[i]["Jemsu1"].ToString().Trim());
                    nPrice = VB.Val(dt.Rows[i]["Price1"].ToString().Trim());
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                    strBunno = dt.Rows[i]["Bunno"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    for (k = 0; k < 9; k++)
                    {
                        switch (k)
                        {
                            case 0:
                                strNewBCode = VB.Left(strBCode, 5) + "006";
                                break;
                            case 1:
                                strNewBCode = VB.Left(strBCode, 4) + "2";//2매
                                break;
                            case 2:
                                strNewBCode = VB.Left(strBCode, 4) + "2006";//2매, 전문의 판독 10%
                                break;
                            case 3:
                                strNewBCode = VB.Left(strBCode, 4) + "3";//3매
                                break;
                            case 4:
                                strNewBCode = VB.Left(strBCode, 4) + "3006";//3매, 전문의 판독 10%
                                break;
                            case 5:
                                strNewBCode = VB.Left(strBCode, 4) + "4";//4매
                                break;
                            case 6:
                                strNewBCode = VB.Left(strBCode, 4) + "4006";//4매, 전문의 판독 10%
                                break;
                            case 7:
                                strNewBCode = VB.Left(strBCode, 4) + "5";//5매
                                break;
                            case 8:
                                strNewBCode = VB.Left(strBCode, 4) + "5006";//5매, 전문의 판독 10%
                                break;
                        }

                        if (XRAY_SUB(nJemsu, strNewBCode, strBunno, strRemark, clsDB.DbCon) == false)      // 전문의 판독 10%
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        bool XRAY_SUB(double nJemsu, string strNewBCode, string strBunno, string strRemark, PsmhDb pDbCon)
        {
            double nNewJemsu = 0;
            double nNewPrice = 0;
            long nCovJemsu = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            
            nNewJemsu = 0;

            try
            {
                switch (ComFunc.MidH(strNewBCode, 5, 1))
                {
                    case "1":
                        nNewJemsu = nJemsu;
                        break;

                    case "2":
                        nNewJemsu = nJemsu * 1.5;
                        break;

                    case "3":
                        nNewJemsu = nJemsu * 2;
                        break;

                    case "4":
                        nNewJemsu = nJemsu * 2.5;
                        break;

                    case "5":
                        nNewJemsu = nJemsu * 3;
                        break;
                }

                if (strNewBCode.Length > 5)
                {
                    if (VB.Right(strNewBCode, 3) == "006")
                    {
                        nNewJemsu = nNewJemsu * 1.1;
                    }
                }

                nCovJemsu = VB.Fix((int)((nNewJemsu + 0.005) * 100));
                nNewJemsu = nCovJemsu / 100;

                nNewPrice = VB.Fix((int)(nNewJemsu * 55.4 + 5));
                nNewPrice = VB.Fix((int)(nNewPrice / 10));
                nNewPrice = nNewPrice * 10;


                // 자료를 INSERT
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";
                SQL = SQL + ComNum.VBLF + "     (BCode, JDate1, Jemsu1, Price1, BunNo, Remark, EntSabun) ";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         '" + strNewBCode + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('2001-01-01','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "         " + nNewJemsu + ", ";
                SQL = SQL + ComNum.VBLF + "         " + nNewPrice + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strBunno + "',";
                SQL = SQL + ComNum.VBLF + "         '" + strRemark + "', ";
                SQL = SQL + ComNum.VBLF + "         -2";
                SQL = SQL + ComNum.VBLF + "     ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
    }
}

