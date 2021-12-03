using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBasVitalSetWard : Form
    {
        #region //생성자
        public frmEmrBasVitalSetWard()
        {
            InitializeComponent();
        }

        private void frmEmrBasVitalSetWard_Load(object sender, EventArgs e)
        {
            cboWard.Items.Clear();
            ssForm_Sheet1.RowCount = 0;
            ssFormWard_Sheet1.RowCount = 0;

            FormSearch("임상관찰", ssForm_Sheet1);

            int sIndex = 0;

            #region 병동 설정
            cboWard.Enabled = false;

            DataTable dt = null;
            string SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    if (dt.Rows[i]["WardCode"].ToString().Trim().Equals(WardCodes))
                    {
                        sIndex = i;
                    }
                }
            }

            dt.Dispose();
            #endregion

            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("OP");
            cboWard.Items.Add("AG");
            cboWard.Items.Add("ENDO");
            cboWard.Items.Add("외래수혈");
            cboWard.Items.Add("CT");
            cboWard.Items.Add("MRI");
            cboWard.Items.Add("RI");
            cboWard.Items.Add("SONO");

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;
            cboWard.Text = clsType.User.BuseCode.Equals("033124") ? "AG" : WardCodes;

            if (NURSE_Manager_Check(clsType.User.IdNumber) == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }
        }

        #endregion //생성자

        #region //함수
        /// <summary>
        /// 간호 관리자인지 체크
        /// </summary>
        /// <param name="strSABUN"></param>
        /// <returns></returns>
        private bool NURSE_Manager_Check(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
            SQL = SQL + ComNum.VBLF + "   AND Code= " + VB.Val(strSABUN) + " ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL    ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            dt.Dispose();
            dt = null;
            return true;
        }

        /// <summary>
        /// 아이템을 조회한다
        /// </summary>
        /// <param name="strWard"></param>
        /// <param name="Spd"></param>
        /// <param name="strSearch"></param>
        private void FormSearch(string strWard, FarPoint.Win.Spread.SheetView Spd, string strSearch = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.RowCount = 0;

            if (strWard == "임상관찰")
            {
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
                SQL = SQL + ComNum.VBLF + "         (SELECT BG.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BG";
                SQL = SQL + ComNum.VBLF + "             WHERE BG.BASCD = B.VFLAG1 ";
                SQL = SQL + ComNum.VBLF + "             AND BG.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "             AND BG.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "     WHERE B.BSNSCLS = '기록지관리'  ";
                SQL = SQL + ComNum.VBLF + "          AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
                SQL = SQL + ComNum.VBLF + "          AND B.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "     )";

                if (string.IsNullOrWhiteSpace(strSearch) == false)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE BASEXNAME LIKE '%" + strSearch.Replace("'", "`")  + "%'  ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY GRPSORT, GRPSEQ, NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO";
            }
            else
            {
                //SQL = "";
                //SQL = "SELECT B.BASVAL, B.BASCD, B.BASNAME, B.BASEXNAME ";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD BB";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                //SQL = SQL + ComNum.VBLF + "     ON BB.BASCD = B.BASCD ";
                //SQL = SQL + ComNum.VBLF + "     AND B.BSNSCLS = '기록지관리' ";
                //SQL = SQL + ComNum.VBLF + "     AND B.UNITCLS = '임상관찰'";
                //SQL = SQL + ComNum.VBLF + "     AND B.USECLS = '0' ";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BG";
                //SQL = SQL + ComNum.VBLF + "     ON B.VFLAG1 = BG.BASCD ";
                //SQL = SQL + ComNum.VBLF + "     AND BG.BSNSCLS = '기록지관리' ";
                //SQL = SQL + ComNum.VBLF + "     AND BG.UNITCLS = '임상관찰그룹'";
                //SQL = SQL + ComNum.VBLF + "WHERE BB.BSNSCLS = '기록지관리' ";
                //SQL = SQL + ComNum.VBLF + "     AND BB.UNITCLS = '임상관찰병동_" + strWard + "'";
                //SQL = SQL + ComNum.VBLF + "     AND BB.USECLS = '0' ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY BG.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3, B.DISSEQNO";

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
                SQL = SQL + ComNum.VBLF + "         (SELECT BG.DISSEQNO FROM " + ComNum.DB_EMR + "AEMRBASCD BG";
                SQL = SQL + ComNum.VBLF + "             WHERE BG.BASCD = B.VFLAG1 ";
                SQL = SQL + ComNum.VBLF + "             AND BG.BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "             AND BG.UNITCLS = B.UNITCLS || '그룹') AS GRPSEQ";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRBASCD B";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
                SQL = SQL + ComNum.VBLF + "          ON B.BASCD = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "          AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "          AND BB.UNITCLS = '임상관찰병동_" + strWard + "'";
                SQL = SQL + ComNum.VBLF + "          AND BB.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "     WHERE B.BSNSCLS = '기록지관리'  ";
                SQL = SQL + ComNum.VBLF + "          AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
                SQL = SQL + ComNum.VBLF + "          AND B.USECLS = '0' ";
                SQL = SQL + ComNum.VBLF + "     )";

                if (string.IsNullOrWhiteSpace(strSearch) == false)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE BASNAME LIKE '%" + strSearch.Replace("'", "`") + "%'  ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY GRPSORT, GRPSEQ, NFLAG1, BASVAL, NFLAG2, NFLAG3, DISSEQNO";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);

            string strBASEXNAME = "";
            int intS = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                Spd.Cells[i, 2].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                if (strBASEXNAME != dt.Rows[i]["BASEXNAME"].ToString().Trim())
                {
                    Spd.Cells[i, 1].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    if (i != 0)
                    {
                        Spd.AddSpanCell(intS, 1, i - intS, 1);
                    }
                    intS = i;
                }
                strBASEXNAME = dt.Rows[i]["BASEXNAME"].ToString().Trim();
            }
            Spd.AddSpanCell(intS, 1, i - intS, 1);

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 부서별 아이템을 저장한다
        /// </summary>
        /// <param name="strItemCD"></param>
        /// <param name="strItemNm"></param>
        /// <returns></returns>
        private bool SaveData(string strItemCD, string strItemNm)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            DataTable dt = null;
            try
            {
                SQL = "";
                SQL = "SELECT BASCD ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '임상관찰병동_" + (VB.Right(cboWard.Text, 20)).Trim() + "'"; //간호활동항목
                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strItemCD + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count != 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "            (BSNSCLS,     UNITCLS,    BASCD,  BASNAME, BASEXNAME, BASVAL, NFLAG1, ";
                SQL = SQL + ComNum.VBLF + "             AplFrDate,   AplEndDate, REMARK1,REMARK2, MNGCLS,    VFLAG1, DISSEQNO, USECLS ) ";
                SQL = SQL + ComNum.VBLF + " VALUES('기록지관리',  ";
                SQL = SQL + ComNum.VBLF + "              '임상관찰병동_" + (VB.Right(cboWard.Text, 20)).Trim() + "', ";
                SQL = SQL + ComNum.VBLF + "              '" + strItemCD + "', ";
                SQL = SQL + ComNum.VBLF + "              '" + strItemNm + "',";
                SQL = SQL + ComNum.VBLF + "              '" + strItemNm + "', ";
                SQL = SQL + ComNum.VBLF + "              0, "; //BASVAL
                SQL = SQL + ComNum.VBLF + "              0, "; //NFLAG1
                SQL = SQL + ComNum.VBLF + "              '20150101',";
                SQL = SQL + ComNum.VBLF + "              '99981231',";
                SQL = SQL + ComNum.VBLF + "              '', ";
                SQL = SQL + ComNum.VBLF + "              '', ";
                SQL = SQL + ComNum.VBLF + "              '0', ";
                SQL = SQL + ComNum.VBLF + "              '', ";
                SQL = SQL + ComNum.VBLF + "              999, ";
                SQL = SQL + ComNum.VBLF + "              '0') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "INSERT중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 사용자별 아이템을 삭제한다
        /// </summary>
        /// <param name="strItemCD"></param>
        /// <returns></returns>
        private bool DeleteData(string strItemCD)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "";
                SQL = "DELETE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'";
                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '임상관찰병동_" + (VB.Right(cboWard.Text, 20)).Trim() + "'"; //간호활동항목
                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + strItemCD + "'";
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
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        #endregion

        private void mbtnSet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormSearch((VB.Right(cboWard.Text, 20)).Trim(), ssFormWard_Sheet1);
        }

        private void ssForm_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssForm_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column == 1)
            {
                string sItemNm = ssForm_Sheet1.Cells[e.Row, 1].Text.Trim();
                for (int i = e.Row; i < ssForm_Sheet1.RowCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(ssForm_Sheet1.Cells[i, 1].Text.Trim()) && !sItemNm.Equals(ssForm_Sheet1.Cells[i, 1].Text.Trim()))
                    {
                        break;
                    }

                    if (ssForm_Sheet1.Cells[i, 1].Text.Trim() == sItemNm || string.IsNullOrWhiteSpace(ssForm_Sheet1.Cells[i, 1].Text.Trim()))
                    {
                        if (SaveData(ssForm_Sheet1.Cells[i, 0].Text.Trim(), ssForm_Sheet1.Cells[i, 2].Text.Trim()) == true)
                        {
                         
                        }
                    }
                }
                FormSearch((VB.Right(cboWard.Text, 20)).Trim(), ssFormWard_Sheet1);
                return;
            }

            string strItemCD = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strItemNm = ssForm_Sheet1.Cells[e.Row, 2].Text.Trim();
            if (SaveData(strItemCD, strItemNm) == true)
            {
                FormSearch((VB.Right(cboWard.Text, 20)).Trim(), ssFormWard_Sheet1);
            }
        }

        private void ssFormWard_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFormWard_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 2)
            {
                return;
            }

            string strItemCD = ssFormWard_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (DeleteData(strItemCD) == true)
            {
                FormSearch((VB.Right(cboWard.Text, 20)).Trim(), ssFormWard_Sheet1);
            }
        }


        #region 검색
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch("임상관찰", ssForm_Sheet1);
                return;
            }

            FormSearch("임상관찰", ssForm_Sheet1, txtSearch.Text.Trim());
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                FormSearch("임상관찰", ssForm_Sheet1);
                return;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                FormSearch("임상관찰", ssForm_Sheet1, txtSearch.Text.Trim());
            }
        }
        #endregion

    }
}
