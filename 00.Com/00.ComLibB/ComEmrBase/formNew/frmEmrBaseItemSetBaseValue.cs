using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBaseItemSetBaseValue : Form
    {
        #region //변수선언
        string mJOBGB = "ACT";
        frmItemUserFunction frmItemUserFunctionX = null; 
        #endregion //변수선언

        #region //생성자
        public frmEmrBaseItemSetBaseValue()
        {
            InitializeComponent();
        }

        public frmEmrBaseItemSetBaseValue(string pJOBGB)
        {
            InitializeComponent();
            mJOBGB = pJOBGB;
        }

        private void frmEmrBaseItemSetBaseValue_Load(object sender, EventArgs e)
        {
            //if (mJOBGB == "IVT")
            //{
            //    panVitalFunc.Visible = true;
            //}
            //else
            //{
            //    panVitalFunc.Visible = false;
            //}

            FormSearch();
        }
        #endregion //생성자

        #region //함수

        /// <summary>
        /// 기록지 아이템조회
        /// </summary>
        private void FormSearch()
        {
            lblFunc.Text = "";
            lblFuncName.Text = "";

            if (mJOBGB == "IVT")
            {
                FormSearchIVT();
            }
            else
            {
                FormSearchAct();
            }
        }

        /// <summary>
        /// 간호활동 항목조회
        /// </summary>
        private void FormSearchAct()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssForm_Sheet1.RowCount = 0;
            ssValue_Sheet1.RowCount = 0;
            ssValue1_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "     AND BB.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '간호활동그룹'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '간호활동항목'"; //간호활동항목
            SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

            string strBASEXNAME = "";
            int intS = 0;

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();

                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 임상관찰 항목조회
        /// </summary>
        private void FormSearchIVT()
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssForm_Sheet1.RowCount = 0;
            ssValue_Sheet1.RowCount = 0;
            ssValue1_Sheet1.RowCount = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     GRPSORT, GRPSEQ, BASCD, BASNAME, BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "     NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO ";
            SQL = SQL + ComNum.VBLF + "FROM (";
            SQL = SQL + ComNum.VBLF + "     SELECT ";
            SQL = SQL + ComNum.VBLF + "         CASE ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '임상관찰' THEN 0 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '섭취배설' THEN 1 ";
            SQL = SQL + ComNum.VBLF + "             WHEN B.UNITCLS = '특수치료' THEN 2 ";
            SQL = SQL + ComNum.VBLF + "             ELSE 3 ";
            SQL = SQL + ComNum.VBLF + "         END AS GRPSORT, ";
            SQL = SQL + ComNum.VBLF + "         B.BASCD, B.BASNAME, B.BASEXNAME, ";
            SQL = SQL + ComNum.VBLF + "         B.NFLAG1, B.BASVAL, B.NFLAG2, B.NFLAG3, B.DISSEQNO, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT BB.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "             WHERE BB.BASCD = B.VFLAG1 ";
            SQL = SQL + ComNum.VBLF + "             AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "             AND BB.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "     WHERE B.BSNSCLS = '기록지관리'  ";
            SQL = SQL + ComNum.VBLF + "          AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "          AND B.USECLS = '0' ";
            SQL = SQL + ComNum.VBLF + "     )";
            //SQL = SQL + ComNum.VBLF + "ORDER BY GRPSORT, BASEXNAME, BASNAME, BASVAL";
            SQL = SQL + ComNum.VBLF + "ORDER BY GRPSORT, GRPSEQ, NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

            string strBASEXNAME = "";
            int intS = 0;

            ssForm_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                ssForm_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                ssForm_Sheet1.SetRowHeight(i, ComNum.SPDROWHT);
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            ssForm_Sheet1.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 아이템에 등록된 값을 가지고 온다
        /// </summary>
        /// <param name="strITEMCD"></param>
        private void GetValue(string strITEMCD)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssValue_Sheet1.RowCount = 0;
            ssValue1_Sheet1.RowCount = 0;
            lblFunc.Text = "";
            lblFuncName.Text = "";

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, MAX(VALUENO) AS VALUENO, ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "ORDER BY MAX(DSPSEQ)";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                ssValue_Sheet1.RowCount = dt.Rows.Count;
                ssValue_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssValue_Sheet1.Cells[i, 0].Text = dt.Rows[i]["VALUENO"].ToString().Trim();
                    ssValue_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                    ssValue_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    ssValue_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            GetItemFunc(strITEMCD);
        }

        /// <summary>
        /// 스프래드에 색을 칠한다
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Col2"></param>
        private static void SelectRowColorEx(FarPoint.Win.Spread.SheetView Spd, int Row, int Col, int Col2)
        {
            int i = 0;

            if (Spd.RowCount <= 0)
                return;

            for (i = 0; i < Spd.RowCount; i++)
            {
                Spd.Cells[i, Col, i, Col2].BackColor = ComNum.SPDESELCOLOR;
            }
            Spd.Cells[Row, Col, Row, Col2].BackColor = ComNum.SPSELCOLOR;
        }

        /// <summary>
        /// 값을 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveValue()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ssValue_Sheet1.RowCount == 0)
                {
                    string strITEMCD = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 0].Text.Trim();
                    bool isExists = false;

                    SQL = "";
                    SQL = "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     ITEMCD ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                    SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                    SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "조회중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        isExists = true;
                    }
                    dt.Dispose();
                    dt = null;

                    if (isExists == true)
                    {
                        if (ComFunc.MsgBoxQEx(this, "기존 데이타가 존재합니다." + ComNum.VBLF + "삭제하시겠습니까?") == DialogResult.No)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                    SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                    SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {
                    //ADMIN.SEQ_AEMRITEMBASEVALUE_VALUENO
                    for (int i = 0; i < ssValue_Sheet1.RowCount; i++)
                    {
                        if (ssValue_Sheet1.Cells[i, 0].Text.Trim() == "") //신규
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                            SQL = SQL + ComNum.VBLF + "     (VALUENO, JOBGB, ITEMCD, ITEMVALUE, ITEMVALUE1, DSPSEQ) ";
                            SQL = SQL + ComNum.VBLF + "VALUES (";
                            SQL = SQL + ComNum.VBLF + "     " + ComNum.DB_EMR + "SEQ_AEMRITEMBASEVALUE_VALUENO.NextVal,"; //VALUENO
                            SQL = SQL + ComNum.VBLF + "     '" + mJOBGB + "',"; //JOBGB
                            SQL = SQL + ComNum.VBLF + "     '" + ssValue_Sheet1.Cells[i, 1].Text.Trim() + "',"; //ITEMCD
                            SQL = SQL + ComNum.VBLF + "     '" + ssValue_Sheet1.Cells[i, 3].Text.Trim() + "',"; //ITEMVALUE
                            SQL = SQL + ComNum.VBLF + "     '" + "" + "',"; //ITEMVALUE
                            SQL = SQL + ComNum.VBLF + "     " + (i + 1).ToString() + ""; //DSPSEQ
                            SQL = SQL + ComNum.VBLF + ")";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRITEMBASEVAL SET ";
                            SQL = SQL + ComNum.VBLF + "     ITEMVALUE = '" + ssValue_Sheet1.Cells[i, 3].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "     DSPSEQ = " + (i + 1).ToString();
                            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                            SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + ssValue_Sheet1.Cells[i, 1].Text.Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + ssValue_Sheet1.Cells[i, 2].Text.Trim() + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 값을 삭제 한다
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private bool DeleteValue(int Row)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            bool isExists = true;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strITEMCD = ssValue_Sheet1.Cells[Row, 1].Text.Trim();
                string strITEMVALUE = ssValue_Sheet1.Cells[Row, 2].Text.Trim();

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     ITEMVALUE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + strITEMVALUE + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    isExists = true;
                }
                dt.Dispose();
                dt = null;

                if (isExists == true)
                {
                    if (ComFunc.MsgBoxQEx(this, "기존 데이타가 존재합니다." + ComNum.VBLF + "삭제하시겠습니까?") == DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + strITEMVALUE + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 서브값을 조회한다
        /// </summary>
        /// <param name="strITEMCD"></param>
        /// <param name="ITEMVALUE"></param>
        private void GetValue1(string strITEMCD, string ITEMVALUE)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssValue1_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, VALUENO, ITEMVALUE1 ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + ITEMVALUE + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ1";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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
            ssValue1_Sheet1.RowCount = dt.Rows.Count;
            ssValue1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssValue1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["VALUENO"].ToString().Trim();
                ssValue1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                ssValue1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                ssValue1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 서브값을 삭제 한다
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private bool DeleteValue1(int Row)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;
            bool isExists = true;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strVALUENO = ssValue1_Sheet1.Cells[Row, 0].Text.Trim();
                string strITEMCD = ssValue1_Sheet1.Cells[Row, 1].Text.Trim();

                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     ITEMVALUE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND VALUENO = " + VB.Val(strVALUENO);
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    isExists = true;
                }
                dt.Dispose();
                dt = null;

                if (isExists == true)
                {
                    if (ComFunc.MsgBoxQEx(this, "기존 데이타가 존재합니다." + ComNum.VBLF + "삭제하시겠습니까?") == DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND VALUENO = " + VB.Val(strVALUENO);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 서브값을 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveValue1()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string strITEMCD = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 1].Text.Trim();
            string strITEMVALUE = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 2].Text.Trim();
            string strDSPSEQ = (ssValue_Sheet1.ActiveRowIndex + 1).ToString();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     DSPSEQ ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + strITEMVALUE + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                strDSPSEQ = dt.Rows[0]["DSPSEQ"].ToString().Trim();

                dt.Dispose();
                dt = null;

                if (ssValue1_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssValue1_Sheet1.RowCount; i++)
                    {
                        if (ssValue1_Sheet1.Cells[i, 0].Text.Trim() == "") //신규
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
                            SQL = SQL + ComNum.VBLF + "     (VALUENO, JOBGB, ITEMCD, ITEMVALUE, ITEMVALUE1, DSPSEQ, DSPSEQ1) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ( ";
                            SQL = SQL + ComNum.VBLF + "     " + ComNum.DB_EMR + "SEQ_AEMRITEMBASEVALUE_VALUENO.NextVal,"; //VALUENO
                            SQL = SQL + ComNum.VBLF + "     '" + mJOBGB + "',"; //JOBGB
                            SQL = SQL + ComNum.VBLF + "     '" + strITEMCD + "',"; //ITEMCD
                            SQL = SQL + ComNum.VBLF + "     '" + strITEMVALUE + "',"; //ITEMVALUE
                            SQL = SQL + ComNum.VBLF + "     '" + ssValue1_Sheet1.Cells[i, 3].Text.Trim() + "',"; //ITEMVALUE1
                            SQL = SQL + ComNum.VBLF + "     " + strDSPSEQ + ", "; // DSPSEQ
                            SQL = SQL + ComNum.VBLF + "     " + (i + 1).ToString() + " "; // DSPSEQ1
                            SQL = SQL + ComNum.VBLF + " )";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRITEMBASEVAL SET ";
                            SQL = SQL + ComNum.VBLF + "     ITEMVALUE1 = '" + ssValue1_Sheet1.Cells[i, 3].Text.Trim() + "',";
                            SQL = SQL + ComNum.VBLF + "     DSPSEQ1 = " + (i + 1).ToString();
                            SQL = SQL + ComNum.VBLF + "WHERE VALUENO = " + VB.Val(ssValue1_Sheet1.Cells[i, 0].Text.Trim());
                            //SQL = SQL + ComNum.VBLF + "WHERE ITEMCD = '" + strITEMCD + "'";
                            //SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + strITEMVALUE + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }

                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 아이템 함수를 조회 한다
        /// </summary>
        private void GetItemFunc(string strITEMCD)
        {
            if (mJOBGB != "IVT")
            {
                return;
            }
            
            lblFunc.Text = "";
            lblFuncName.Text = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     U.ITEMCD, F.FUNCCD, F.FUNCNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEFUNC U";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRFUNCTION  F";
            SQL = SQL + ComNum.VBLF + "     ON U.FUNCCD = F.FUNCCD ";
            SQL = SQL + ComNum.VBLF + "WHERE U.JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "     AND U.ITEMCD = '" + strITEMCD + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

            lblFunc.Text = dt.Rows[0]["FUNCCD"].ToString().Trim();
            lblFuncName.Text = dt.Rows[0]["FUNCNAME"].ToString().Trim();

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 아이템 함수를 저장한다
        /// </summary>
        /// <returns></returns>
        private bool SaveItemFunc()
        {
            if (ssForm_Sheet1.Rows.Count == 0) return false;
            if (lblFunc.Text.Trim() == "") return false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strITEMCD = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 0].Text.Trim();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRITEMBASEFUNC";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRITEMBASEFUNC";
                SQL = SQL + ComNum.VBLF + "    (JOBGB, ITEMCD, FUNCCD) ";
                SQL = SQL + ComNum.VBLF + "VALUES (" ;
                SQL = SQL + ComNum.VBLF + "    '" + mJOBGB + "', ";
                SQL = SQL + ComNum.VBLF + "    '" + strITEMCD + "', ";
                SQL = SQL + ComNum.VBLF + "    '" + lblFunc.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 아이템 함수를 삭제한다
        /// </summary>
        /// <returns></returns>
        private bool DeleteItemFunc()
        {
            if (ssForm_Sheet1.Rows.Count == 0) return false;
            if (lblFunc.Text.Trim() == "") return false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strITEMCD = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 0].Text.Trim();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRITEMBASEFUNC";
                SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
                SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "'";
                SQL = SQL + ComNum.VBLF + "     AND FUNCCD = '" + lblFunc.Text.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "삭제중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                lblFunc.Text = "";
                lblFuncName.Text = "";

                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion //함수

        #region //컨트롤 이벤트
        private void ssForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }

            if (e.Column != 2)
            {
                return;
            }

            SelectRowColorEx(ssForm_Sheet1, e.Row, 2, ssForm_Sheet1.ColumnCount - 1);

            string strITEMCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

            GetValue(strITEMCD);
        }

        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            ssValue_Sheet1.RowCount = ssValue_Sheet1.RowCount + 1;
            ssValue_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssValue_Sheet1.Cells[ssValue_Sheet1.RowCount - 1, 1].Text = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 0].Text.Trim();
        }

        private void btnSaveValue_Click(object sender, EventArgs e)
        {
            if (SaveValue() == true)
            {
                string strITEMCD = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 1].Text.Trim();
                GetValue(strITEMCD);
            }
        }

        private void btnRowDelete_Click(object sender, EventArgs e)
        {
            if (ssValue_Sheet1.RowCount == 0) return;

            if (ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 0].Text.Trim() == "")
            {
                ssValue_Sheet1.RemoveRows(ssValue_Sheet1.ActiveRowIndex, 1);
                return;
            }

            if (DeleteValue(ssValue_Sheet1.ActiveRowIndex) == true)
            {
                string strITEMCD = ssForm_Sheet1.Cells[ssForm_Sheet1.ActiveRowIndex, 1].Text.Trim();
                GetValue(strITEMCD);
            }
        }

        private void ssValue_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssValue_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssValue, e.Column);
                return;
            }

            ComFunc.SelectRowColor(ssValue_Sheet1, e.Row);

            string strITEMCD = ssValue_Sheet1.Cells[e.Row, 1].Text.Trim();
            string ITEMVALUE = ssValue_Sheet1.Cells[e.Row, 2].Text.Trim();

            GetValue1(strITEMCD, ITEMVALUE);
        }

        private void btnRowAdd1_Click(object sender, EventArgs e)
        {
            ssValue1_Sheet1.RowCount = ssValue1_Sheet1.RowCount + 1;
            ssValue1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssValue1_Sheet1.Cells[ssValue1_Sheet1.RowCount - 1, 1].Text = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 1].Text.Trim();
        }

        private void btnRowDelete1_Click(object sender, EventArgs e)
        {
            if (ssValue1_Sheet1.RowCount == 0) return;

            if (ssValue1_Sheet1.Cells[ssValue1_Sheet1.ActiveRowIndex, 0].Text.Trim() == "" )
            {
                ssValue1_Sheet1.RemoveRows(ssValue1_Sheet1.ActiveRowIndex, 1);
                return;
            }

            if (DeleteValue1(ssValue1_Sheet1.ActiveRowIndex) == true)
            {
                string strITEMCD = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 1].Text.Trim();
                string strITEMVALUE = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 2].Text.Trim();
                GetValue1(strITEMCD, strITEMVALUE);
            }
        }

        private void btnSaveValue1_Click(object sender, EventArgs e)
        {
            if (ssValue1_Sheet1.RowCount == 0) return;

            string strITEMCD = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 1].Text.Trim();
            string strVALUENO = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 0].Text.Trim();

            if (strITEMCD == "" || VB.Val(strVALUENO) == 0)
            {
                ComFunc.MsgBoxEx(this, "상위 값을 먼저 저장해 주십시요.");
            }

            if (SaveValue1() == true)
            {
                strITEMCD = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 1].Text.Trim();
                string strITEMVALUE = ssValue_Sheet1.Cells[ssValue_Sheet1.ActiveRowIndex, 2].Text.Trim();
                GetValue1(strITEMCD, strITEMVALUE);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            MoveUpDownOne(ssValue, 0);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            MoveUpDownOne(ssValue, 1);
        }

        private void btnUp1_Click(object sender, EventArgs e)
        {
            MoveUpDownOne(ssValue1, 0);
        }

        private void btnDown1_Click(object sender, EventArgs e)
        {
            MoveUpDownOne(ssValue1, 1);
        }

        private void MoveUpDownOne(FarPoint.Win.Spread.FpSpread Spd, int intFlag)
        {
            int j = 0;
            int intActRow = -1;
            int intActRowMove = -1;

            if (Spd.Sheets[0].ActiveRowIndex == -1)
            {
                ComFunc.MsgBox("선택된 값이 없습니다.");
                return;
            }

            if (intFlag == 0)
            {
                intActRowMove = Spd.Sheets[0].ActiveRowIndex - 1;
                if (Spd.Sheets[0].ActiveRowIndex <= 0)
                {
                    ComFunc.MsgBox("맨윗줄입니다." + ComNum.VBLF + "이동할 수 없습니다.");
                    return;
                }
            }
            else
            {
                intActRowMove = Spd.Sheets[0].ActiveRowIndex + 1;
                if (Spd.Sheets[0].ActiveRowIndex >= Spd.Sheets[0].RowCount - 1)
                {
                    ComFunc.MsgBox("맨아랫줄입니다." + ComNum.VBLF + "이동할 수 없습니다.");
                    return;
                }
            }

            intActRow = Spd.Sheets[0].ActiveRowIndex;
            ssValue_Short_Sheet1.RowCount = 0;

            ssValue_Short_Sheet1.RowCount = ssValue_Short_Sheet1.RowCount + 1;
            for (j = 0; j < Spd.Sheets[0].ColumnCount; j++)
            {
                ssValue_Short_Sheet1.Cells[ssValue_Short_Sheet1.RowCount - 1, j].Text = Spd.Sheets[0].Cells[intActRow, j].Text;
            }

            Spd.Sheets[0].Rows[intActRow].Remove();
            if (intFlag == 0)
            {
                Spd.Sheets[0].AddRows(intActRow - 1, 1);
            }
            else
            {
                Spd.Sheets[0].AddRows(intActRow + 1, 1);
            }
            for (j = 0; j < ssValue_Short_Sheet1.ColumnCount; j++)
            {
                Spd.Sheets[0].Cells[intActRowMove, j].Text = ssValue_Short_Sheet1.Cells[0, j].Text;
            }

            Spd.Sheets[0].ActiveRowIndex = intActRowMove;
            Spd.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);

        }

        #endregion //컨트롤 이벤트

        private void btnSearchFunc_Click(object sender, EventArgs e)
        {
            if (frmItemUserFunctionX != null)
            {
                frmItemUserFunctionX.Dispose();
                frmItemUserFunctionX = null;
            }
            frmItemUserFunctionX = new frmItemUserFunction("01");
            frmItemUserFunctionX.rEventClosed += new frmItemUserFunction.EventClosed(frmItemUserFunction_EventClosed);
            frmItemUserFunctionX.rSetFuncCode += new frmItemUserFunction.SetFuncCode(frmItemUserFunction_SetFuncCode);
            frmItemUserFunctionX.StartPosition = FormStartPosition.CenterParent;
            frmItemUserFunctionX.ShowDialog(this);
        }

        private void frmItemUserFunction_EventClosed()
        {
            frmItemUserFunctionX.Dispose();
            frmItemUserFunctionX = null;
        }

        private void frmItemUserFunction_SetFuncCode(string strFUNCGB, string strFuncCode, string strFuncName)
        {
            frmItemUserFunctionX.Dispose();
            frmItemUserFunctionX = null;

            lblFunc.Text = strFuncCode;
            lblFuncName.Text = strFuncName;

            SaveItemFunc();
        }

        private void btnDeleteFunc_Click(object sender, EventArgs e)
        {
            DeleteItemFunc();
        }
    }
}
