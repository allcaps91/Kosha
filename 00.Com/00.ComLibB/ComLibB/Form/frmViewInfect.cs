using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmViewInfect.cs
    /// Description     : 격리주의 상세내역
    /// Author          : 박창욱
    /// Create Date     : 2017-08-03
    /// <history> 
    /// 격리주의 상세내역
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\OpdOcs\ojumst\FrmViewInfect.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\OpdOcs\ojumst\ojumst.vbp
    /// </vbp>
    /// </summary>
    public partial class frmViewInfect : Form
    {
        private string GstrPANO = "";
        frmViewResult frmVResultX = null;

        public frmViewInfect()
        {
            InitializeComponent();
        }

        public frmViewInfect(string strPANO)
        {
            InitializeComponent();

            GstrPANO = strPANO;
        }

        private void frmViewInfect_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            FormClear();

            if (GstrPANO.Trim() != "")
            {
                txtPtNo.Text = GstrPANO;
                txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text.Trim(), 8, "0");

                Patient_Name_READ(txtPtNo.Text);

                //감염여부 조회
                READ_EXAM_INFECT(txtPtNo.Text);
            }
        }

        private void FormClear(string argJob = "")
        {
            panMain.Visible = true;
            panMain.Dock = DockStyle.Fill;
            panView.Visible = false;
            panView.Dock = DockStyle.None;
            ssPrint1.Visible = false;
            ssPrint2.Visible = false;
            ssPrint3.Visible = false;
            ssPrint4.Visible = false;

            ssView1_Sheet1.Columns[2].Visible = false;
            ssView2_Sheet1.Columns[2].Visible = false;
            ssView3_Sheet1.Columns[2].Visible = false;
            ssView4_Sheet1.Columns[2].Visible = false;

            if (argJob == "")
            {
                txtPtNo.Text = "";
                lblSName.Text = "";
                lblSexAge.Text = "";
                lblBI.Text = "";
            }

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 0;
            ssView4_Sheet1.RowCount = 0;
            ssView5_Sheet1.RowCount = 0;
            ssView6_Sheet1.RowCount = 0;
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text.Trim(), 8, "0");

                Patient_Name_READ(txtPtNo.Text);

                //감염여부 조회
                READ_EXAM_INFECT(txtPtNo.Text);
            }
        }

        private void Patient_Name_READ(string strPtNo)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strJumin2 = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     Pano, Sname, Jumin1, Jumin2, Jumin3, Sex ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    lblSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    lblSexAge.Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalcEx(dt.Rows[0]["JUMIN1"].ToString().Trim() + strJumin2, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                }
                else
                {
                    lblSName.Text = "";
                    lblSexAge.Text = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_EXAM_INFECT(string strPtNo)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;
            FormClear("1");

            try
            {
                for (i = 1; i <= 6; i++)
                {
                    switch(i)
                    {
                        case 1:
                            ssSpread_Sheet = ssView1_Sheet1;
                            break;
                        case 2:
                            ssSpread_Sheet = ssView2_Sheet1;
                            break;
                        case 3:
                            ssSpread_Sheet = ssView3_Sheet1;
                            break;
                        case 4:
                            ssSpread_Sheet = ssView4_Sheet1;
                            break;
                        case 5:
                            ssSpread_Sheet = ssView5_Sheet1;
                            break;
                        case 6:
                            ssSpread_Sheet = ssView6_Sheet1;
                            break;
                    }

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     EXNAME ,TO_CHAR(RDate,'YYYY-MM-DD') AS RDate, ROWID, SPECNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_INFECT_MASTER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '" + i.ToString("00") + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND ODATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "ORDER BY RDate DESC ,EXNAME ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssSpread_Sheet.RowCount = dt.Rows.Count;
                        ssSpread_Sheet.SetRowHeight(-1, ComNum.SPDROWHT * 2);

                        for(k = 0; k < dt.Rows.Count; k++)
                        {
                            ssSpread_Sheet.Cells[k, 0].Text = dt.Rows[k]["EXNAME"].ToString().Trim();
                            ssSpread_Sheet.Cells[k, 1].Text = dt.Rows[k]["RDate"].ToString().Trim();
                            ssSpread_Sheet.Cells[k, 2].Text = dt.Rows[k]["RowID"].ToString().Trim();
                            ssSpread_Sheet.Cells[k, 3].Text = "더블클릭";
                            //ssSpread_Sheet.Cells[k, 3].Text = READ_MASTERNAME(dt.Rows[k]["SPECNO"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null) 
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_MASTERNAME(string strSPECNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return ""; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     (SELECT EXAMNAME FROM " + ComNum.DB_MED + "EXAM_MASTER";
                SQL = SQL + ComNum.VBLF + "         WHERE MASTERCODE = A.MASTERCODE) EXAMNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "EXAM_RESULTC A";
                SQL = SQL + ComNum.VBLF + "     WHERE SPECNO = '" + strSPECNO + "'";
                SQL = SQL + ComNum.VBLF + "         AND ROWNUM = 1";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["EXAMNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            SheetView ssSpread_Sheet = null;
            string strROWID = "";
            string strTitle = "";

            switch(VB.Right(((FpSpread)sender).Name, 1))
            {
                case "1":
                    ssSpread_Sheet = ssView1_Sheet1;
                    strTitle = "혈액주의";
                    break;
                case "2":
                    ssSpread_Sheet = ssView2_Sheet1;
                    strTitle = "접촉주의";
                    break;
                case "3":
                    ssSpread_Sheet = ssView3_Sheet1;
                    strTitle = "공기주의";
                    break;
                case "4":
                    ssSpread_Sheet = ssView4_Sheet1;
                    strTitle = "비말주의";
                    break;
                case "5":
                    ssSpread_Sheet = ssView5_Sheet1;
                    strTitle = "보호격리";
                    break;
                case "6":
                    ssSpread_Sheet = ssView6_Sheet1;
                    strTitle = "해외경유자";
                    break;
            }

            if(e.Column == 3)
            {
                if (frmVResultX != null)
                {
                    frmVResultX.Dispose();
                    frmVResultX = null;
                }

                frmVResultX = new frmViewResult(txtPtNo.Text.Trim(), this, 1);
                frmVResultX.rEventClosed += frmVResultX_rEventClosed; 
                frmVResultX.StartPosition = FormStartPosition.CenterScreen;
                frmVResultX.Show(this);

                return;
            }

            strROWID = ssSpread_Sheet.Cells[e.Row, 2].Text.Trim();

            if (strROWID != "")
            {
                if (ComFunc.MsgBoxQ("선택하신 " + strTitle + " 항목을 삭제처리 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    if (DelData(strROWID) == true)
                    {
                        READ_EXAM_INFECT(txtPtNo.Text);
                    }
                }
            }
        }

        private void frmVResultX_rEventClosed()
        {
            if (frmVResultX != null)
            {
                frmVResultX.Dispose();
                frmVResultX = null;
            }
        }

        private bool DelData(string strROWID)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_MED + "EXAM_INFECT_MASTER";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         ODATE = SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         OSABUN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

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

        private void btnView_Click(object sender, EventArgs e)
        {
            FpSpread ssSpread = null;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            panMain.Visible = false;
            panMain.Dock = DockStyle.None;
            panView.Visible = true;
            panView.Dock = DockStyle.Fill;

            switch (VB.Right(((Button)sender).Name, 1))
            {
                case "1":
                    ssSpread = ssPrint1;
                    break;
                case "2":
                    ssSpread = ssPrint2;
                    break;
                case "3":
                    ssSpread = ssPrint3;
                    break;
                case "4":
                    ssSpread = ssPrint4;
                    break;
            }

            ssSpread.Visible = true;
            ssSpread.Dock = DockStyle.Fill;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, SNAME, WARDCODE, ROOMCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "         AND JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND OUTDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssSpread.ActiveSheet.Cells[4, 0].Text = lblSName.Text.Trim();
                    ssSpread.ActiveSheet.Cells[4, 1].Text = txtPtNo.Text.Trim();
                    ssSpread.ActiveSheet.Cells[4, 2].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            FpSpread ssSpread = null;

            if (ssPrint1.Visible == true)
            {
                ssSpread = ssPrint1;
            }
            else if (ssPrint2.Visible == true)
            {
                ssSpread = ssPrint2;
            }
            else if (ssPrint3.Visible == true)
            {
                ssSpread = ssPrint3;
            }
            else if (ssPrint4.Visible == true)
            {
                ssSpread = ssPrint4;
            }

            ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssSpread.ActiveSheet.PrintInfo.Orientation = PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = true;
            ssSpread.ActiveSheet.PrintInfo.Centering = Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.Centering = Centering.Vertical;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = true;
            ssSpread.ActiveSheet.PrintInfo.UseMax = true;
            //ssRcpPrt_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = true;
            ssSpread.PrintSheet(0);
        }

        private void btnPrintEnd_Click(object sender, EventArgs e)
        {
            ssPrint1.Visible = false;
            ssPrint1.Dock = DockStyle.None;
            ssPrint2.Visible = false;
            ssPrint2.Dock = DockStyle.None;
            ssPrint3.Visible = false;
            ssPrint3.Dock = DockStyle.None;
            ssPrint4.Visible = false;
            ssPrint4.Dock = DockStyle.None;
            
            panView.Visible = false;
            panView.Dock = DockStyle.None;
            panMain.Visible = true;
            panMain.Dock = DockStyle.Fill;
        }


        public void rGetDate(string strPato)
        {

            FormClear();
            
            txtPtNo.Text = ComFunc.LPAD(strPato, 8, "0");
            
            Patient_Name_READ(strPato);
            READ_EXAM_INFECT(strPato);
        }

        private void frmViewInfect_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmVResultX != null)
            {
                frmVResultX.Dispose();
                frmVResultX = null;
            }
        }
    }
}
