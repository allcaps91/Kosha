using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmMcrtJobBas02 : Form
    {
  
        private string fstrROWID = "";
        string FAVCLASS_CODE = "00";

        public frmMcrtJobBas02()
        {
            InitializeComponent();
        }

        private void frmMcrtJobBas02_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.SetIMEMODE(this, "K");
            lblKorE.Text = "한글";
            SetTextBoxEvent();

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetFavData();
        }

        private void SetTextBoxEvent()
        {
            Control[] Controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in Controls)
            {
                if (ctl is TextBox)
                {
                    ((TextBox)ctl).ImeModeChanged += new System.EventHandler(TextBox_ImeModeChanged);
                }
                //ClearAllTextBoxes(ctl);
            }
        }

        private void TextBox_ImeModeChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).ImeMode == ImeMode.Hangul)
            {
                lblKorE.Text = "한글";
            }
            else
            {
                lblKorE.Text = "영어";
            }
        }

        private void GetFavData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";


            DataTable dt = null;

            txtFav1.Text = "";
            txtFav2.Text = "";
            txtFav3.Text = "";

            ssFav_Sheet1.RowCount = 0;
            try
            {
                SQL = " SELECT MCCLASS, TITLENAME ";
                SQL = SQL + ComNum.VBLF + " FROM  " + ComNum.DB_MED + "OCS_MCTITLE ";
                SQL = SQL + ComNum.VBLF + " WHERE FAVCLASS = '" + FAVCLASS_CODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND MCCLASS IN ('01','02','03','04','05','10', '12','18','22', '26', '27','28', '29' ,'31') ";
                SQL = SQL + ComNum.VBLF + " ORDER   BY MCCLASS";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 진단서 코드가 없습니다.");
                    return;
                }

                ssMc1_Sheet1.RowCount = dt.Rows.Count;
                ssMc1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssMc1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MCCLASS"].ToString().Trim() + " " + dt.Rows[i]["TITLENAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                ssMc1_Sheet1.SetActiveCell(0, 0);
                AddFav();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
            }
        }

        private void AddFav(int row = 0)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssMc2_Sheet1.RowCount = 0;
            cboMc2.Items.Clear();
            try
            {
                SQL = " SELECT  FAVCLASS, TITLENAME ";
                SQL = SQL + ComNum.VBLF + " FROM    " + ComNum.DB_MED + "OCS_MCTITLE ";
                SQL = SQL + ComNum.VBLF + " WHERE   MCCLASS = '" + VB.Left(ssMc1_Sheet1.Cells[row, 0].Text, 2) + "'";
                SQL = SQL + ComNum.VBLF + " AND     FAVCLASS > '" + FAVCLASS_CODE + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 진단서 코드가 없습니다.");
                    return;
                }

                ssMc2_Sheet1.RowCount = dt.Rows.Count;
                ssMc2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssMc2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FAVCLASS"].ToString().Trim() + " " + dt.Rows[i]["TITLENAME"].ToString().Trim();
                    cboMc2.Items.Add(dt.Rows[i]["FAVCLASS"].ToString().Trim() + " " + dt.Rows[i]["TITLENAME"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
            }
        }

        private bool Delete_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_MED + "OCS_MCFAV ";
                SQL = SQL + ComNum.VBLF + " WHERE  MCCLASS   = '" + VB.Left(ssMc1_Sheet1.ActiveCell.Text, 2) +"' ";
                SQL = SQL + ComNum.VBLF + " AND    FAVCLASS  = '" + VB.Left(ssMc2_Sheet1.ActiveCell.Text, 2) +"' ";
                SQL = SQL + ComNum.VBLF + " AND    FAVSEQ    = '" + VB.Val(txtFav1.Text) + "'";
                SQL = SQL + ComNum.VBLF + " AND DRSABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return false;
            }
        }

        private void GetDataList()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (cboMc2.SelectedIndex == -1)
            {
                ComFunc.MsgBox("먼저 상용구종류를 선택하십시오.");
                cboMc2.Focus();
                return;
            }

            ssFav_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT FAVSEQ, FAVTITLE, FAVTEXT, ROWID " + ComNum.VBLF;
                SQL = SQL + "  FROM " + ComNum.DB_MED + "OCS_MCFAV " + ComNum.VBLF;
                SQL = SQL + " WHERE MCCLASS  = '" + VB.Left(ssMc1_Sheet1.ActiveCell.Text, 2) + "' " + ComNum.VBLF;
                SQL = SQL + "   AND FAVCLASS = '" + VB.Left(cboMc2.Text, 2) + "' " + ComNum.VBLF;
                SQL = SQL + "   AND DRSABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' " + ComNum.VBLF;
                SQL = SQL + " ORDER   BY FAVSEQ " + ComNum.VBLF;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 상용구가 없습니다.","상용구 조회");
                    Cursor.Current = Cursors.Default;
                    cboMc2.Focus();
                    return;
                }

                ssFav_Sheet1.RowCount = dt.Rows.Count;
                ssFav_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssFav_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FAVSEQ"].ToString().Trim();
                    ssFav_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FAVTITLE"].ToString().Trim();
                    ssFav_Sheet1.Cells[i, 2].Tag  = dt.Rows[i]["FAVTEXT"].ToString().Trim();
                    ssFav_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private bool Regist_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (fstrROWID != "")
                {
                    if (ComFunc.MsgBoxQ("이미 데이타가 들어있습니다. 수정하겠습니까?","데이타 수정확인") == DialogResult.Yes)
                    {
                        SQL = SQL + " UPDATE  " + ComNum.DB_MED + "OCS_MCFAV ";
                        SQL = SQL + ComNum.VBLF + " SET     FAVTITLE  = '" + txtFav2.Text.Trim().Replace("'", "`") + "',";
                        SQL = SQL + ComNum.VBLF + "         FAVTEXT   = '" + txtFav3.Text.Trim().Replace("'", "`") + "'";
                        SQL = SQL + ComNum.VBLF + " WHERE   ROWID  = '" + fstrROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            return false;
                        }
                    }
                }
                else
                {
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_MCFAV ";
                    SQL = SQL + ComNum.VBLF + " (MCCLASS, FAVCLASS, FAVSEQ, FAVTITLE, FAVTEXT, DRSABUN)";
                    SQL = SQL + ComNum.VBLF + " VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(ssMc1_Sheet1.ActiveCell.Text, 2) +"', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(ssMc2_Sheet1.ActiveCell.Text, 2) +"', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Val(txtFav1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + txtFav2.Text.Trim().Replace( "'", "`") + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + txtFav3.Text.Trim().Replace("'", "`") + "', '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return false;
            }
        }

        private void SelectText(TextBox txt)
        {
            txt.SelectionStart = 0;
            txt.SelectionLength = txt.Text.Length;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (cboMc2.Text.Trim() == "")
            {
                ComFunc.MsgBox("상용구 종류를 먼저 선택 하세요.", "확인");
                return;
            }
            fstrROWID = "";
            txtFav1.Text = "";
            txtFav2.Text = "";
            txtFav3.Text = "";
            txtFav1.Focus();
            txtFav2.Focus();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;
            }

            if (txtFav1.Text == "")
            {
                ComFunc.MsgBox("상용구 번호를 입력하세요", "번호오류");
                txtFav1.Focus();
                return;
            }
            if (txtFav2.Text == "")
            {
                ComFunc.MsgBox("상용구 Title을 입력하세요", "번호오류");
                txtFav2.Focus();
                return;
            }

            if (Regist_Data() == false)
            {
                return;
            }

            txtFav1.Text = "";
            txtFav2.Text = "";
            txtFav3.Text = "";
            txtFav1.Focus();
            GetDataList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return;
            }

            if ( txtFav1.Text == "")
            {
                ComFunc.MsgBox("상용구 번호를 입력하세요", "번호오류");
                txtFav1.Focus();
                return;
            }

            if (Delete_Data() == false)
            {
                return;
            }

            txtFav1.Text = "";
            txtFav2.Text = "";
            txtFav3.Text = "";
            txtFav1.Focus();
            GetDataList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }
            GetDataList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFav1_Enter(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = SQL + " SELECT MAX(FAVSEQ) SEQ ";
                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_MED + "OCS_MCFAV ";
                SQL = SQL + ComNum.VBLF + " WHERE  MCCLASS   = '" + VB.Left(ssMc1_Sheet1.ActiveCell.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  FAVCLASS  = '" + VB.Left(ssMc2_Sheet1.ActiveCell.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  DRSABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    txtFav1.Text = "1";
                    return;
                }

                txtFav1.Text = dt.Rows[0]["SEQ"].ToString().Trim();
                dt.Dispose();
                dt = null;
                SelectText(txtFav1);
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
            }
        }

        private void txtFav1_Leave(object sender, EventArgs e)
        {

            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtFav1.Text.Trim() == "")
            {
                return;
            }

            try
            {
                SQL = SQL + " SELECT  * ";
                SQL = SQL + ComNum.VBLF + " FROM    " + ComNum.DB_MED + "OCS_MCFAV ";
                SQL = SQL + ComNum.VBLF + " WHERE  MCCLASS   = '" + VB.Left(ssMc1_Sheet1.ActiveCell.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + " AND     FAVCLASS = '" + VB.Left(ssMc2_Sheet1.ActiveCell.Text, 2) + "' ";
                SQL = SQL + ComNum.VBLF + " AND     FAVSEQ   = " + VB.Val(txtFav1.Text);
                SQL = SQL + ComNum.VBLF + " AND  DRSABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    txtFav2.Text = "";
                    txtFav3.Text = "";
                    return;
                }

                txtFav2.Text = dt.Rows[0]["FAVTITLE"].ToString().Trim();
                txtFav3.Text = dt.Rows[0]["FAVTEXT"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
            }
        }

        private void ssFav_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == -1 || ssFav_Sheet1.RowCount == 0)
            {
                return;
            }

            ssFav_Sheet1.Cells[0, 0, ssFav_Sheet1.RowCount - 1, ssFav_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssFav_Sheet1.Cells[e.Row, 0, e.Row, ssFav_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            fstrROWID = ssFav_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtTitle.Text = ssFav_Sheet1.Cells[e.Row, 2].Tag.ToString().Trim();
        }

        private void ssFav_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == -1 || ssFav_Sheet1.RowCount == 0)
            {
                return;
            }

            txtFav1.Text = ssFav_Sheet1.Cells[e.Row, 0].Text.Trim();
            txtFav2.Text = ssFav_Sheet1.Cells[e.Row, 1].Text.Trim();
            txtFav3.Text = ssFav_Sheet1.Cells[e.Row, 2].Tag.ToString().Trim();
        }

        private void txtFav2_Enter(object sender, EventArgs e)
        {
            SelectText(txtFav2);
        }

        private void txtFav3_Enter(object sender, EventArgs e)
        {
            SelectText(txtFav3);
        }

        private void txtFav1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtFav2.Focus();
            }
        }

        private void txtFav2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtFav3.Focus();
            }
        }

        private void ssMc1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //clsMcrt.ClearAllTextBoxes(this);
            ssFav_Sheet1.RowCount = 0;

            AddFav(e.Row);
        }

        private void ssMc2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtFav1.Text = "";
            txtFav2.Text = "";
            txtFav3.Text = "";
        }

        private void ssMc2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            cboMc2.SelectedIndex = e.Row;
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetDataList();
        }

        private void lblKorE_Click(object sender, EventArgs e)
        {
            if (lblKorE.Text == "영어")
            {
                ComFunc.SetIMEMODE(this, "K");
                lblKorE.Text = "한글";
            }
            else
            {
                ComFunc.SetIMEMODE(this, "E");
                lblKorE.Text = "영어";
            }
        }

        private void txtFav3_KeyDown(object sender, KeyEventArgs e)
        {
            string strText = "";
            if (e.KeyCode == Keys.F2)
            {
                //strText = clsEngToKor.UDF_Eng2Han(strText);
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Eng2Kor(strText);
                ((TextBox)sender).SelectedText = strText;
            }
            else if (e.KeyCode == Keys.F3)
            {
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Kor2Eng(strText);
                ((TextBox)sender).SelectedText = strText;
            }
        }
    }
}
