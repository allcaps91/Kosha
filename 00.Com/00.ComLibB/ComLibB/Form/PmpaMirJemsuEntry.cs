using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{

    /// Class Name      : PmpaMir.dll
    /// File Name       : PmpaMirJemsuEntry.cs
    /// Description     : 상대가치 점수 등록
    /// Author          : 최익준
    /// Create Date     : 2017-09-06
    /// Update History  : 
    /// </summary>
    /// <vbp>           : PSMH\basic\busuga\BuSuga45.frm
    /// default
    /// </vbp>
    /// 
    public partial class frmPmpaMirJemsuEntry : Form
    {
        string GstrROWID = "";
        string[] FstrJDATE = new string[5];
        double[] GdblJemsu = new double[5];
        double[] GdblPrince = new double[5];

        public frmPmpaMirJemsuEntry()
        {
            InitializeComponent();
        }


        private void Screen_Display(string ArgCode)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgCode == "")
            {
                return;
            }

            GstrROWID = "";
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnDelete.Enabled = false;
            panCode.Enabled = true;

            //자료를 SELECT
            SQL = "";
            SQL = SQL + "SELECT";
            SQL = SQL + ComNum.VBLF + "     BCode, Remark, BunNo, EntSabun, ROWID,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(DelDate,'YYYY-MM-DD') AS DelDate,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') AS EntTime,";

            SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDate1,'YYYY-MM-DD') AS JDate1, Jemsu1, Price1,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDate2,'YYYY-MM-DD') AS JDate2, Jemsu2, Price2,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDate3,'YYYY-MM-DD') AS JDate3, Jemsu3, Price3,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDate4,'YYYY-MM-DD') AS JDate4, Jemsu4, Price4,";
            SQL = SQL + ComNum.VBLF + "     TO_CHAR(JDate5,'YYYY-MM-DD') AS JDate5, Jemsu5, Price5";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";
            //            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "  WHERE BCode='" + ArgCode + "' ";

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

            txtName.Text = dt.Rows[0]["Remark"].ToString().Trim();
            txtHCode.Text = dt.Rows[0]["BunNo"].ToString().Trim();
            dtpDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
            lblEntDate.Text = dt.Rows[0]["EntTime"].ToString().Trim() + clsVbfunc.GetPassName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());
            GstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            for (i = 0; i < 5; i++)
            {
                ssCode_Sheet1.Cells[0, i + 1].Text = dt.Rows[0]["JDate" + VB.Format(i + 1, "0")].ToString().Trim();
                ssCode_Sheet1.Cells[1, i + 1].Value = VB.Val(dt.Rows[0]["Jemsu" + VB.Format(i + 1, "0")].ToString().Trim());
                ssCode_Sheet1.Cells[2, i + 1].Value = VB.Val(dt.Rows[0]["Price" + VB.Format(i + 1, "0")].ToString().Trim());

                FstrJDATE[i] = dt.Rows[0]["JDate" + VB.Format(i + 1, "0")].ToString().Trim();
                GdblJemsu[i] = VB.Val(dt.Rows[0]["Jemsu" + VB.Format(i + 1, "0")].ToString().Trim());
                GdblPrince[i] = VB.Val(dt.Rows[0]["Price" + VB.Format(i + 1, "0")].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            btnDelete.Enabled = true;
        }

        private void Screen_Clear()
        {
            int i = 0;

            txtCode.Text = "";
            txtName.Text = "";
            txtHCode.Text = "";
            dtpDelDate.Text = "";
            lblEntDate.Text = "";

            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnDelete.Enabled = false;
            txtCode.Enabled = true;
            panCode.Enabled = false;

            GstrROWID = "";

            for (i = 0; i < 5; i++)
            {
                FstrJDATE[i] = "";
                GdblJemsu[i] = 0;
                GdblPrince[i] = 0;
            }
        }

        private void PmpaMirJemsuEntry_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssCode_Sheet1.Columns[6].Visible = false;
            //ssView_Sheet1.Columns[5].Visible = false;

            Screen_Clear();

            txtData.Text = "";

            cboBlank.Items.Add("(1)번");
            cboBlank.Items.Add("(2)번");
            cboBlank.Items.Add("(3)번");
            cboBlank.Items.Add("(4)번");

            cboBlank.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCode.Text = (txtCode.Text.Trim()).ToUpper();

                if (txtCode.Text == "")
                {
                    return;
                }

                txtCode.Text.ToString();

                if (GstrROWID != "")
                {
                    ssCode.Focus();
                }
                else
                {
                    txtName.Focus();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strName = "";
            string strHCode = "";
            string strDeldate = "";

            string[] strJDate = new string[5];
            double[] dblJemsu = new double[5];
            double[] dblPrice = new double[5];

            strName = clsVbfunc.QuotationChange(txtName.Text.Trim());
            strHCode = clsVbfunc.QuotationChange(txtHCode.Text.Trim());
            strDeldate = clsVbfunc.QuotationChange(dtpDelDate.Text.Trim());

            for (i = 0; i < 5; i++)
            {
                strJDate[i] = ssCode_Sheet1.Cells[i, 0].Text;
                dblJemsu[i] = VB.Val(ssCode_Sheet1.Cells[i, 1].Text);
                dblPrice[i] = VB.Val(ssCode_Sheet1.Cells[i, 2].Text);
            }

            Cursor.Current = Cursors.WaitCursor;

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (GstrROWID == "")        // 신규등록
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SUGAJEMSU (";
                    SQL = SQL + ComNum.VBLF + "BCode,BunNo,DelDate,Remark,";
                    SQL = SQL + ComNum.VBLF + "JDate1,Jemsu1,Price1,JDate2,Jemsu2,Price2,JDate3,Jemsu3,Price3,";
                    SQL = SQL + ComNum.VBLF + "JDate4,Jemsu4,Price4,JDate5,Jemsu5,Price5,EntSabun,EntTime";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES ('";
                    SQL = SQL + ComNum.VBLF + txtCode.Text + "','" + strHCode + "',TO_DATE('" + strDeldate + "','YYYY-MM-DD'),'";
                    SQL = SQL + ComNum.VBLF + strName + "', ";

                    for (i = 0; i < 5; i++)
                    {
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate[i].ToString().Trim() + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + dblJemsu[i] + "," + dblPrice[i] + ",";
                    }

                    SQL = SQL + clsType.User.Sabun + ",SYSDATE)";

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SUGAJEMSU SET BunNo='" + strHCode + "',";
                    SQL = SQL + ComNum.VBLF + "Remark = '" + strName + "',";
                    SQL = SQL + ComNum.VBLF + "DelDate = TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";

                    for (i = 0; i < 5; i++)
                    {
                        SQL = SQL + ComNum.VBLF + "JDate" + VB.Format(i, "0") + " = TO_DATE('" + strJDate[i].Trim() + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "Jemsu" + VB.Format(i, "0") + " = " + dblJemsu[i] + ",";
                        SQL = SQL + ComNum.VBLF + "Price" + VB.Format(i, "0") + " = " + dblPrice[i] + ",";
                    }

                    SQL = SQL + ComNum.VBLF + "EntSabun='" + clsType.User.Sabun + "', EntTime=SYSDATE";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + GstrROWID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox("BAS_EDIJENSU에 UPDATE중 오류 발생", "오류");
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Clear();

                txtCode.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (GstrROWID == "")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("상대가치점수를 정말로 삭제를 하시겠습니까?", "삭제", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            //clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "DELETE FROM BAS_SUGAJEMSU";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + GstrROWID + "'";


                clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox("BAS_SUGAJEMSU 삭제시 오류가 발생함", "RollBack");

                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                Screen_Clear();

                txtCode.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                Screen_Clear();
            }

        }

        private void txtHCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpDelDate.Focus();
            }
        }

        private void dtpDelDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ssCode.Focus();
            }
        }

        private void btnBlankInsert_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int NCol = 0;
            string strData = "";

            NCol = Convert.ToInt32((VB.Mid(cboBlank.Text, 2, 1)));
            //공란을 만듬
            for (i = 4; i < NCol; i++)
            {
                for (j = 1; j < 4; j++)
                {
                    strData = ssCode_Sheet1.Cells[j, i + 1].Text.Trim();
                    ssCode_Sheet1.Cells[j, i + 2].Text = strData.Trim();
                }
            }
            ssCode_Sheet1.Cells[0, NCol].Text = "";
            ssCode_Sheet1.Cells[1, NCol].Text = "";
            ssCode_Sheet1.Cells[2, NCol].Text = "";
        }

        private void rdoView0_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                rdoView1.Focus();
            }
        }

        private void rdoView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                rdoView2.Focus();
            }
        }

        private void rdoView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                rdoView0.Focus();
            }
        }


        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (txtData.Text == "")
            {
                ComFunc.MsgBox("찾으실 자료가 공란입니다.", "오류발생");
                txtData.Focus();
                return;
            }
            btnPrint.Enabled = false;

            // 해당자료를 검색

            try
            {
                SQL = "";
                SQL = SQL + "SELECT BCode, Jemsu1, Price1, BunNo, Remark";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUGAJEMSU";

                if (rdoView0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BCode LIKE '" + txtData.Text.Trim().ToUpper() + "%' ";
                }
                else if (rdoView1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BunNo LIKE '" + txtData.Text + "%' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Remark LIKE '%" + txtData.Text + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY BCode";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.", "오류발생");
                    txtData.Focus();
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = VB.Val(dt.Rows[i]["Jemsu1"].ToString().Trim()).ToString("###,###,##0.00");
                    ssView_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["Price1"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BunNo"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                btnPrint.Enabled = true;
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
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            string strttp = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            // Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/c/f1" + "상대가치 점수 코드집" + "/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + strttp + "";
            strHead2 = strHead2 + "/r/f2" + "PAGE : /p";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;

            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;

            ssView.PrintSheet(0);
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strCODE = "";

            strCODE = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (strCODE == "")
            {
                return;
            }
            
            Screen_Clear();

            txtCode.Text = strCODE;

            Screen_Display(strCODE);
            ssCode.Focus();
        }


    }
}
