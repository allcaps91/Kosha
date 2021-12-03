using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{

    /// <summary>
    ///Class Name       : ComLibB
    /// File Name       : frmSugaJemsu.cs
    /// Description     : 상대가치점수
    /// Author          : 최익준
    /// Create Date     : 2017-09-26
    /// <seealso> 
    /// PSMH\basic\busuga\frmSugaJemsu.frm
    /// </seealso>
    /// </summary>

    public partial class frmSugaJemsu : Form
    {
        public frmSugaJemsu()
        {
            InitializeComponent();
        }

        void frmSugaJemsu_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        void btnInsert_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int nREAD = 0;

            string strCode = "";
            string strFCode = "";
            string strTCode = "";
            string strJDate = "";
            double dblJemsu = 0;
            double dblPrice = 0;
            string strSeqNo = "";
            string strBunno = "";
            string strRemark = "";

            strJDate = "2001-01-01";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //자료를 SELECT            
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT *";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU_WORK";
                SQL = SQL + ComNum.VBLF + "WHERE BCode IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("자료가 등록되지 않았습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    strCode = dt.Rows[i]["BCode"].ToString().Trim();
                    strCode = VB.Pstr(strCode, "(", 1);
                    strCode = VB.Replace(strCode, "-", " ");
                    dblJemsu = VB.Val(dt.Rows[i]["Jemsu"].ToString().Trim());
                    dblPrice = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());
                    strSeqNo = dt.Rows[i]["SeqNo"].ToString().Trim();
                    strBunno = dt.Rows[i]["BunNo"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();


                    if (VB.I(strCode, " ") > 1)
                    {
                        string strAllCode = "";

                        strAllCode = strCode.Trim();

                        for (j = 1; j < VB.I(strAllCode, " "); j++)
                        {
                            strCode = VB.Pstr(strAllCode, " ", j).Trim();

                            if (VB.I(strCode, "~") > 1)
                            {
                                strFCode = VB.Pstr(strCode, "~", 1);
                                strTCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Pstr(strCode, "~", 2);
                            }
                            else
                            {
                                strFCode = strCode;
                                strTCode = strCode;
                            }

                            while (true)
                            {
                                if (strCode == "")
                                {
                                    break;
                                }
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark)";
                                SQL = SQL + ComNum.VBLF + "VALUES ('" + strFCode + "',TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + dblJemsu + "," + dblPrice + "," + strSeqNo + ",'" + strBunno + "','" + strRemark + "')";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                strFCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Format(VB.Val(VB.Right(strFCode, 1)) + 1, "0");

                                if (VB.Val(strFCode) > VB.Val(strTCode) || VB.Right(strFCode, 1) == "0")
                                {
                                    break;
                                }
                            }
                        }
                    }

                    else if (VB.I(strCode, "~") > 1)
                    {
                        strFCode = VB.Pstr(strCode, "~", 1).Trim();
                        strTCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Pstr(strCode, "~", 2).Trim();

                        while (true)
                        {
                            if (strFCode == "")
                            {
                                break;
                            }
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strFCode + "',TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + dblJemsu + "," + dblPrice + "," + strSeqNo + ",'" + strBunno + "','" + strRemark + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            strFCode = VB.Left(strFCode, VB.Len(strFCode) - 1) + VB.Format(VB.Val(VB.Right(strFCode, 1)) + 1, "0");
                            if (VB.Val(strFCode) > VB.Val(strTCode) || VB.Right(strFCode, 1) == "0")
                            {
                                break;
                            }
                        }
                    }

                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "INSERT INTO";
                        SQL = SQL + ComNum.VBLF + "" + ComNum.DB_PMPA + "BAS_SUGAJEMSU (";
                        SQL = SQL + ComNum.VBLF + "BCode,JDate1,Jemsu1,Price1,SeqNo,BunNo,Remark)";
                        SQL = SQL + ComNum.VBLF + "VALUES ('" + strCode + "',TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "" + dblJemsu + "," + dblPrice + "," + strSeqNo + ",'" + strBunno + "','" + strRemark + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(strSeqNo + ":BAS_SUGAJEMSU에 자료를 INSERT시 오류가 발생함", "확인");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    lblState.Text = "( " + i + " )";

                }
                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnXRay_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int nREAD = 0;
            string strBCode = "";
            string strNewBCode = "";
            double dblJemsu = 0;
            double dblNewJemsu = 0;
            double dblPrice = 0;
            double dblNewPrice = 0;
            string strBunno = "";
            string strRemark = "";
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";    //Query문

            DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);
            
            Cursor.Current = Cursors.WaitCursor;

            //방사선 단순촬영 상대가치 점수를 READ
            try
            {
                SQL = "";
                SQL = "SELECT BCode,Jemsu1,Price1,BunNo,Remark ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU ";
                SQL = SQL + ComNum.VBLF + "WHERE BCode >='G1001' ";
                SQL = SQL + ComNum.VBLF + "  AND BCode <='G9999' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    dblNewJemsu = VB.Val(dt.Rows[i]["Jemsu1"].ToString().Trim());
                    dblPrice = VB.Val(dt.Rows[i]["Price1"].ToString().Trim());
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();
                    strBunno = dt.Rows[i]["Bunno"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    if (VB.Right(strBCode, 3) != "006")
                    {
                        strNewBCode = strBCode + "006";                     //전문의판독 10%
                    }
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "2";           //2매
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "2006";        //2매, 전문의 판독10%
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "3";           //3매
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "3006";        //3매, 전문의 판독10%
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "4";           //4매
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "4006";        //4매, 전문의 판독10%
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "5";           //5매
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                    strNewBCode = VB.Left(strBCode, 4) + "5006";        //5매, 전문의 판독10%
                    XRAY_SUB(strNewBCode, dblNewJemsu, dblJemsu, dblNewPrice, strBunno, strRemark);
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("정상적으로 종료됨", "확인");

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void XRAY_SUB(string strNewBCode, double dblNewJemsu, double dblJemsu, double dblNewPrice, string strBunno, string strRemark)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            string SQL = "";
            double dblCovJemsu = 0;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            dblNewJemsu = 0;

            switch (VB.Mid(strNewBCode, 5, 1))
            {
                case "1":
                    dblNewJemsu = dblJemsu;
                    break;
                case "2":
                    dblNewJemsu = dblJemsu * 1.5;
                    break;
                case "3":
                    dblNewJemsu = dblJemsu * 2;
                    break;
                case "4":
                    dblNewJemsu = dblJemsu * 2.5;
                    break;
                case "5":
                    dblNewJemsu = dblJemsu * 3;
                    break;
            }

            if (strNewBCode.Length > 5)
            {
                if (VB.Right(strNewBCode, 3) == "006")
                {
                    dblNewJemsu = dblNewJemsu * 1.1;
                }
            }

            dblCovJemsu = Convert.ToDouble(VB.Fix(Convert.ToInt32(dblNewJemsu + 0.005) * 100));
            dblNewJemsu = dblCovJemsu / 100;

            dblNewPrice = Convert.ToDouble(VB.Fix(Convert.ToInt32(dblNewJemsu * 55.4 + 5)));
            dblNewPrice = Convert.ToDouble(VB.Fix(Convert.ToInt32(dblNewPrice / 10)));
            dblNewPrice = dblNewPrice * 10;

            //자료를 INSERT
            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (BCode,JDate1,Jemsu1,Price1,BunNo,Remark,EntSabun) ";
            SQL = SQL + ComNum.VBLF + "VALUES ('" + strNewBCode + "',TO_DATE('2001-01-01','YYYY-MM-DD'),";
            SQL = SQL + ComNum.VBLF + dblNewJemsu + "," + dblNewPrice + ",'" + strBunno + "','";
            SQL = SQL + ComNum.VBLF + strRemark + "',-2) ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("자료를 INSERT도중 오류가 발생함", "확인");
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
