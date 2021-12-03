using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrMacrowordProgTrs : Form
    {
        string gStrId = "";
        string gStrIdIdx = "";
        string gStrFormNo = "";
        string gStrMaxLength = "";
        //string gStrMaxRow = "";
        string gStrLabel = "";
        //string pGrpNo = "";

        public frmEmrMacrowordProgTrs()
        {
            InitializeComponent();
        }

        public frmEmrMacrowordProgTrs(string strStrId, string strStrIdIdx, string strStrFormNo, string strStrMaxLength, string strStrLabel)
        {
            InitializeComponent();

            gStrId = strStrId;
            gStrIdIdx = strStrIdIdx;
            gStrFormNo = strStrFormNo;
            gStrMaxLength = strStrMaxLength;
            gStrLabel = strStrLabel;
        }

        private void frmMacrowordProgTrs_Load(object sender, EventArgs e)
        {
            ssMacroWordLL_Sheet1.RowCount = 0;
            ssMacroWord_Sheet1.RowCount = 0;
            txtContentLL.Text = "";
            txtContent.Text = "";

            optMacroSeachUseid.Checked = true;

            optMacroSeachUseidLL.Checked = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        private void SetComboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboDoct.Items.Clear();

            cboDept.Items.Clear();

            //-->진료과 세팅
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT MEDDEPTCD, DEPTKORNAME ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "ABMEDDEPT ";
            SQL = SQL + ComNum.VBLF + " WHERE EMRUSE =  '1' ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY PRTGRD ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add((dt.Rows[i]["DEPTKORNAME"].ToString() + "").Trim() + VB.Space(50) + (dt.Rows[i]["MEDDEPTCD"].ToString() + "").Trim());
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMedDeptCd = VB.Trim(VB.Right(cboDept.Text.Trim(), 4));
            if (strMedDeptCd == "0")
            {
                strMedDeptCd = "";
            }
            ssMacroWordLL_Sheet1.RowCount = 0;
            txtContentLL.Text = "";
            chkAll.Checked = false;

            if (optMacroSeachDeptLL.Checked == true)
            {
                GetMacroList(ssMacroWordLL_Sheet1, strMedDeptCd, "", "D");
            }
            else
            {
                pSetDoctCombo(strMedDeptCd, cboDoct);
            }
        }

        private void pSetDoctCombo(string strMedDeptCd, ComboBox conCobmo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            conCobmo.Items.Clear();

            //-->진료의 세팅
            //conCobmo.Items.Add("전  체" + VB.Space(50) + "0");

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT USEID, USENAME ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AVIEWEMRUSER ";
            SQL = SQL + ComNum.VBLF + " WHERE COMUSEYN =  '1' ";
            SQL = SQL + ComNum.VBLF + " AND (MEDDEPTCD =  '" + strMedDeptCd + "') ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY MEDDEPTCD, USENAME ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count != 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    conCobmo.Items.Add((dt.Rows[i]["USENAME"].ToString() + "").Trim() + VB.Space(50) + (dt.Rows[i]["USEID"].ToString() + "").Trim());
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void cboDoct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMedDeptCd = VB.Trim(VB.Right(cboDept.Text.Trim(), 4));
            string strMedDrCd = VB.Trim(VB.Right(cboDoct.Text.Trim(), 9));
            chkAll.Checked = false;
            GetMacroList(ssMacroWordLL_Sheet1, strMedDeptCd, strMedDrCd, "U");
        }

        private void GetMacroList(FarPoint.Win.Spread.SheetView Spd, string strMedDeptCd, string strMedDrCd, string strGRPGB)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Spd.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.GRPGB ,C.FORMNAME, A.TITLE, A.CONTENT, A.USEID, A.GRPTYPE";
            SQL = SQL + ComNum.VBLF + "              , A.ORDERSEQ, A.MAXVAL, A.GRPGB, A.USEGB, A.MACRONO,A.FORMNO, A.CONTROLID, A.CONTROLIDIDX";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN (SELECT * FROM " + ComNum.DB_EMR + "AEMRFORM WHERE FORMNO IN (SELECT FORMNO FROM " + ComNum.DB_EMR + "AEMRFORM GROUP BY FORMNO ) ) C";
            SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = C.FORMNO";

            if (strGRPGB == "D")
            {
                SQL = SQL + ComNum.VBLF + "  WHERE GRPGB = 'D'";
                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + strMedDeptCd + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  WHERE GRPGB = 'U'";
                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + strMedDrCd + "'";
            }

            if (gStrFormNo.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = '" + gStrFormNo.Trim() + "'";
            }

            if (gStrId.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.CONTROLID = '" + gStrId.Trim() + "'";
                if (gStrIdIdx.Trim() == "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND (A.CONTROLIDIDX IS NULL OR A.CONTROLIDIDX = '')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.CONTROLIDIDX = '" + gStrIdIdx.Trim() + "'";
                }
            }

            SQL = SQL + ComNum.VBLF + " ORDER BY A.ORDERSEQ";
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
                return;
            }
            Spd.RowCount = dt.Rows.Count;
            Spd.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Spd.Cells[i, 1].Text = VB.Trim(dt.Rows[i]["GRPGB"].ToString());
                Spd.Cells[i, 2].Text = VB.Trim(dt.Rows[i]["TITLE"].ToString());
                Spd.Cells[i, 3].Text = VB.Trim(dt.Rows[i]["CONTENT"].ToString());
                Spd.Cells[i, 4].Text = VB.Trim(dt.Rows[i]["GRPTYPE"].ToString());
                Spd.Cells[i, 5].Text = VB.Trim(dt.Rows[i]["USEID"].ToString());
                Spd.Cells[i, 6].Text = VB.Trim(dt.Rows[i]["ORDERSEQ"].ToString());
                Spd.Cells[i, 7].Text = VB.Trim(dt.Rows[i]["MAXVAL"].ToString());
                Spd.Cells[i, 8].Text = VB.Trim(dt.Rows[i]["USEGB"].ToString());
                Spd.Cells[i, 9].Text = VB.Trim(dt.Rows[i]["MACRONO"].ToString());
                Spd.Cells[i, 10].Text = VB.Trim(dt.Rows[i]["FORMNO"].ToString());
                Spd.Cells[i, 11].Text = VB.Trim(dt.Rows[i]["FORMNAME"].ToString());
                Spd.Cells[i, 12].Text = VB.Trim(dt.Rows[i]["CONTROLID"].ToString());
                Spd.Cells[i, 13].Text = VB.Trim(dt.Rows[i]["CONTROLIDIDX"].ToString());
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void ssMacroWordLL_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMacroWordLL_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacroWordLL, e.Column);
                return;
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }

            string strMacrono = ssMacroWordLL_Sheet1.Cells[e.Row, 9].Text;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtContentLL.Text = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT CONTENT FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE WHERE MACRONO=" + VB.Val(strMacrono.Trim());
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
                return;
            }
            txtContentLL.Text = VB.Replace(VB.Replace(VB.Trim(dt.Rows[0]["CONTENT"].ToString()), "<pre>", ""), "</pre>", "");

            dt.Dispose();
            dt = null;
        }

        private void optMacroSeachDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachDept.Checked == true)
            {
                ssMacroWord_Sheet1.RowCount = 0;
                txtContent.Text = "";
                GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "D");
            }
        }

        private void optMacroSeachUseid_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachUseid.Checked == true)
            {
                ssMacroWord_Sheet1.RowCount = 0;
                txtContent.Text = "";
                GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "U");
            }
        }

        private void optMacroSeachDeptLL_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachDeptLL.Checked == true)
            {
                ssMacroWordLL_Sheet1.RowCount = 0;
                txtContentLL.Text = "";

                lblDoc.Visible = false;
                cboDoct.Visible = false;

                SetComboDept();

                chkAll.Checked = false;
            }
        }

        private void optMacroSeachUseidLL_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachUseidLL.Checked == true)
            {
                ssMacroWordLL_Sheet1.RowCount = 0;
                txtContentLL.Text = "";

                lblDoc.Visible = true;
                cboDoct.Visible = true;
                SetComboDept();
                chkAll.Checked = false;
            }
        }

        private void ssMacroWord_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMacroWord_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacroWord, e.Column);
                return;
            }

            string strMacrono = ssMacroWord_Sheet1.Cells[e.Row, 9].Text;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtContent.Text = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT CONTENT FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE WHERE MACRONO=" + VB.Val(strMacrono.Trim());
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
                return;
            }
            txtContent.Text = VB.Replace(VB.Replace(VB.Trim(dt.Rows[0]["CONTENT"].ToString()), "<pre>", ""), "</pre>", "");

            dt.Dispose();
            dt = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }

            if (DeleteData() == true)
            {
                ssMacroWord_Sheet1.RowCount = 0;
                txtContent.Text = "";
                if (optMacroSeachDept.Checked == true)
                {
                    GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "D");
                }
                else
                {
                    GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "U");
                }
            }
        }

        private bool DeleteData()
        {
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssMacroWord_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssMacroWord_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = SQL + "delete from " + ComNum.DB_EMR + "AEMRUSERSENTENCE WHERE MACRONO = " + ssMacroWord_Sheet1.Cells[i, 9].Text.Trim();
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) //권한 확인
            {
                return;
            }

            if (SaveData() == true)
            {
                ssMacroWord_Sheet1.RowCount = 0;
                txtContent.Text = "";
                if (optMacroSeachDept.Checked == true)
                {
                    GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "D");
                }
                else
                {
                    GetMacroList(ssMacroWord_Sheet1, clsType.User.DeptCode, clsType.User.IdNumber, "U");
                }
            }
        }

        private bool SaveData()
        {
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strGRPGB = "";
            string strUSEGB = "";

            if (optMacroSeachDept.Checked == true)
            {
                strGRPGB = "D";
                strUSEGB = clsType.User.DeptCode;
            }
            else
            {
                strGRPGB = "U";
                strUSEGB = clsType.User.IdNumber;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssMacroWordLL_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssMacroWordLL_Sheet1.Cells[i, 0].Value) == true)
                    {
                        double dblMacroNo = ComQuery.GetSequencesNoEx(clsDB.DbCon, "" + ComNum.DB_EMR + "SEQ_AEMRUSERSENTENCE_MACRONO");

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  INSERT INTO " + ComNum.DB_EMR + "AEMRUSERSENTENCE (";
                        SQL = SQL + ComNum.VBLF + "        MACRONO";
                        SQL = SQL + ComNum.VBLF + "        , FORMNO";
                        SQL = SQL + ComNum.VBLF + "        , CONTROLID";
                        SQL = SQL + ComNum.VBLF + "        , CONTROLIDIDX";
                        SQL = SQL + ComNum.VBLF + "        , GRPTYPE";
                        SQL = SQL + ComNum.VBLF + "        , GRPGB";
                        SQL = SQL + ComNum.VBLF + "        , USEGB";
                        SQL = SQL + ComNum.VBLF + "        , TITLE";
                        SQL = SQL + ComNum.VBLF + "        , CONTENT";
                        SQL = SQL + ComNum.VBLF + "        , USEID";
                        SQL = SQL + ComNum.VBLF + "        , WRITEDATE";
                        SQL = SQL + ComNum.VBLF + "        , WRITETIME";
                        SQL = SQL + ComNum.VBLF + "        , ORDERSEQ";
                        SQL = SQL + ComNum.VBLF + "        , MAXVAL";
                        SQL = SQL + ComNum.VBLF + "        , USETYPE)";
                        SQL = SQL + ComNum.VBLF + "    SELECT ";
                        SQL = SQL + ComNum.VBLF + "        " + dblMacroNo + " AS MACRONO";
                        SQL = SQL + ComNum.VBLF + "        , FORMNO";
                        SQL = SQL + ComNum.VBLF + "        , CONTROLID";
                        SQL = SQL + ComNum.VBLF + "        , CONTROLIDIDX";
                        SQL = SQL + ComNum.VBLF + "        , GRPTYPE";
                        SQL = SQL + ComNum.VBLF + "        , '" + strGRPGB + "' AS GRPGB";
                        SQL = SQL + ComNum.VBLF + "        , '" + strUSEGB + "' AS USEGB";
                        SQL = SQL + ComNum.VBLF + "        , TITLE";
                        SQL = SQL + ComNum.VBLF + "        , CONTENT";
                        SQL = SQL + ComNum.VBLF + "        , '" + clsType.User.IdNumber + "' AS USEID";
                        SQL = SQL + ComNum.VBLF + "        , WRITEDATE";
                        SQL = SQL + ComNum.VBLF + "        , WRITETIME";
                        SQL = SQL + ComNum.VBLF + "        , ORDERSEQ";
                        SQL = SQL + ComNum.VBLF + "        , MAXVAL";
                        SQL = SQL + ComNum.VBLF + "        , USETYPE";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE ";
                        SQL = SQL + ComNum.VBLF + "WHERE MACRONO = " + ssMacroWordLL_Sheet1.Cells[i, 9].Text.Trim();
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ssMacroWordLL_Sheet1.RowCount ; i++)
            {
                ssMacroWordLL_Sheet1.Cells[i, 0].Value = chkAll.Checked;
            }
        }
        
    }
}
