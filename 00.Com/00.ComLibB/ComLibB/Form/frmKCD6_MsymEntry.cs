using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// KCD6차 상병코드 등록
    /// </summary>
    public partial class frmKCD6_MsymEntry : Form
    {
        string fstrROWID = string.Empty;

        public frmKCD6_MsymEntry()
        {
            InitializeComponent();
        }

        private void frmKCD6_MsymEntry_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssKCD6_Sheet1.RowCount = 0;

            Screen_Clear();

            cboClass.Items.Clear();
            cboClass.Items.Add("A.상병(대)분류");
            cboClass.Items.Add("B.상병(중)분류" );
            cboClass.Items.Add("C.상병(소)분류" );
            cboClass.Items.Add("1.KCD6차"     );
            cboClass.Items.Add("2.상해외인코드" );
            cboClass.Items.Add("3.국제수술코드");
            cboClass.Items.Add("4.User Define");
            cboClass.Items.Add("5.기록실수술코드");
            cboClass.SelectedIndex = 3;

            cboViewClass.Items.Clear();
            cboViewClass.Items.Add("A.상병(대)분류");
            cboViewClass.Items.Add("B.상병(중)분류" );
            cboViewClass.Items.Add("C.상병(소)분류" );
            cboViewClass.Items.Add("1.KCD6차"     );
            cboViewClass.Items.Add("2.상해외인코드" );
            cboViewClass.Items.Add("3.국제수술코드");
            cboViewClass.Items.Add("4.User Define");
            cboViewClass.Items.Add("5.기록실수술코드");
            cboViewClass.SelectedIndex = 0;

            cboSex.Items.Clear();
            cboSex.Items.Add(" ");
            cboSex.Items.Add("M");
            cboSex.Items.Add("F");
            cboSex.SelectedIndex = 0;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {

        }

        private void btnSearchA_Click(object sender, EventArgs e)
        {

        }

        private void ssKCD6_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void txtGbInfect_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Screen_Display(VB.Left(cboClass.Text, 1));
            }
        }

        void Screen_Display(string strClass = "")
        {
            txtCode.Text = txtCode.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                Screen_Clear();
                return;
            }

        }

        void Screen_Clear()
        {
            txtCode.Clear();
            cboClass.SelectedIndex = -1;
            txtNameSpacing.Text = "0";

            txtNameK.Clear();
            txtNameE.Clear();
            txtDispHeader.Clear();
            txtILLUP.Clear();
            txtILLUPName.Clear();

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void txtILLCODED_KeyUp(object sender, KeyEventArgs e)
        {
            ssList.Visible = false;

            if(txtILLCODED.TextLength > 3)
            {
                GetILLCODE();
            }
        }

        void GetILLCODE()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT ILLCODE, ILLCODED, ILLNAMEK FROM BAS_ILLS_KCD6";
                SQL += ComNum.VBLF + " WHERE ILLCODED  LIKE '" + txtILLCODED.Text + "%' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssList.Height = ssList_Sheet1.RowCount * 65;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = string.Format("{0}.{1}.{1}",
                        dt.Rows[i]["illcodeD"].ToString().Trim(),
                        dt.Rows[i]["ILLNAMEK"].ToString().Trim(),
                        dt.Rows[i]["illcode"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                ssList.Focus();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (string.IsNullOrEmpty(txtCode.Text))
            {
                ComFunc.MsgBox("상병코드가 공란입니다.");
                return;
            }

            if (string.IsNullOrEmpty(cboClass.Text))
            {
                ComFunc.MsgBox("상병분류가 공란입니다.");
                return;
            }

            if (string.IsNullOrEmpty(txtNameK.Text) && string.IsNullOrEmpty(txtNameE.Text))
            {
                ComFunc.MsgBox("한글명,영문명 모두 공란입니다.");
                return;
            }

            txtNameK.Text = txtNameK.Text.Replace("'", "`");
            txtNameE.Text = txtNameE.Text.Replace("'", "`");
            txtDispHeader.Text = txtDispHeader.Text.Replace("'", "`");

            if (Save_Data())
            {
                Screen_Clear();
                txtCode.Focus();
            }
        }

        bool Save_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if(string.IsNullOrEmpty(fstrROWID))
                {
                    SQL = "INSERT INTO BAS_ILLS_KCD6 (IllCode,IllClass,IllNameK,IllNameE,NameSpacing,";
                    SQL = SQL + "DispHeader, ILLUPCODE, NOUSE,INFECT, GUBUN, KCD6, SDATE, EDATE, ILLCODED  ) VALUES ('" + txtCode.Text + "','" + VB.Left(cboClass.Text, 1) + "','";
                    SQL = SQL + txtNameK.Text.Trim() + "','" + txtNameE.Text.Trim() + "','";
                    SQL = SQL + ComFunc.SetAutoZero(txtNameSpacing.Text, 1) + "','" + txtDispHeader.Text + "', '" + txtILLUP.Text + "', ";

                    SQL = SQL + ComNum.VBLF + "  '" + (chkNoUse.Checked  ? "N" : "")  + "' , ";
                    SQL = SQL + ComNum.VBLF + "  '" + (chkInFect.Checked ? "Y" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  '" + (chkGubun.Checked  ? "1" : "")  + "',  ";
                    SQL = SQL + ComNum.VBLF + "  '" + (chkKCD6.Checked   ? "*" : "") + "',  ";
                    SQL = SQL + ComNum.VBLF + "   TO_DATE('" + txtSDate.Text + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "   TO_DATE('" + txtDDate.Text + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "    '" + txtILLCODED.Text + "' ";


                    SQL = SQL + ComNum.VBLF + " ) ";
                }
                else
                {
                    SQL = "UPDATE BAS_ILLS_KCD6 SET IllClass='" + VB.Left(cboClass.Text, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "IllNameK='" + txtNameK.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "IllNameE='" + txtNameE.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "DispHeader='" + txtDispHeader.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "NameSpacing='" + ComFunc.SetAutoZero(txtNameSpacing.Text, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "  ILLUPCODE = '" + txtILLUP.Text + "', ";
                    SQL = SQL + ComNum.VBLF + "  NOUSE = '"  + (chkNoUse.Checked  ? "N" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  INFECT = '" + (chkInFect.Checked ? "Y" : "") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  GUBUN ='"   + (chkGubun.Checked  ? "1" : "") + "',  ";
                    SQL = SQL + ComNum.VBLF + "  KCD6 ='"    + (chkKCD6.Checked   ? "*" : "") + "' ,";

                    SQL = SQL + ComNum.VBLF + "   SDATE = TO_DATE('" + txtSDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "   DDATE = TO_DATE('" + txtDDate.Text + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + "   ILLCODED = '" + txtILLCODED.Text + "' ";

                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + fstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("정말로 삭제 하시겠습니까?", "삭제여부") == DialogResult.No)
            {
                return;
            }

            Delete_Data();
            Screen_Clear();
            txtCode.Focus();
        }

        bool Delete_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "DELETE BAS_ILLS_KCD6 WHERE IllCode='" + txtCode.Text + "' ";
                SQL = SQL + ComNum.VBLF + " AND ILLCLASS  = '" + VB.Left(cboClass.Text, 1) + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            txtCode.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            READ_BAS_ILLS_KCD6("ALL");
        }

        void READ_BAS_ILLS_KCD6(string strGbn)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT IllCode,ILLCODED, IllClass,IllNameK,IllNameE,DispHeader, SEX, NOUSE, ILLUPCODE ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_ILLS_KCD6 ";
                SQL = SQL + ComNum.VBLF + "WHERE IllClass='" + VB.Left(cboViewClass.Text, 1) + "' ";

                if (strGbn != "ALL")
                {
                    SQL = SQL + ComNum.VBLF + " AND ILLCODE LIKE '" + strGbn + "%'  ";
                } 

                if(rdoSort0.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllCodeD ";
                }
                else if(rdoSort1.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllNameK,IllCodeD ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY IllNameE,IllCodeD ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                ssKCD6_Sheet1.RowCount = 0;
                ssKCD6_Sheet1.RowCount = dt.Rows.Count;
                ssKCD6_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssKCD6_Sheet1.Cells[i, 0].Text = dt.Rows[i]["illcodeD"].ToString().Trim();
                    ssKCD6_Sheet1.Cells[i, 1].Text = dt.Rows[i]["illclass"].ToString().Trim();
                    ssKCD6_Sheet1.Cells[i, 2].Text = dt.Rows[i]["illcodeD"].ToString().Trim();
                    ssKCD6_Sheet1.Cells[i, 3].Text = string.Format(
                        string.IsNullOrEmpty(dt.Rows[i]["Nouse"].ToString().Trim()) ? " {0}" : "* {0}",
                        dt.Rows[i]["ILLNAMEK"].ToString().Trim());
                    ssKCD6_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DispHeader"].ToString().Trim();
                    ssKCD6_Sheet1.Cells[i, 5].Text = dt.Rows[i]["illcode"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnSangCode_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }

        private void txtDate_DoubleClick(object sender, EventArgs e)
        {
            using (frmCalendar1 frm = new frmCalendar1())
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
            }

            if (string.IsNullOrEmpty(clsPublic.GstrCalDate))
                return;

            (sender as TextBox).Text = clsPublic.GstrCalDate;
        }
    }
}
