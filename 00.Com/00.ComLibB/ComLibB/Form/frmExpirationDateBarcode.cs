using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmExpirationDateBarcode : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmExpirationDateBarcode.cs
        /// Description     : 유효기간 바코드 출력
        /// Author          : 김효성
        /// Create Date     : 2017-06-26
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\nurse\nrinfo\Frm유효기간바코드 => frmExpirationDateBarcode.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\nurse\nrinfo\Frm유효기간바코드(Frm유효기간바코드)
        /// </seealso>
        /// <vbp>
        /// default : VB\nurse\nrinfo\nrinfo.vbp
        /// </vbp>

        int nPrint = 0;
        DateTime date;

        public frmExpirationDateBarcode()
        {
            InitializeComponent();
        }

        private string SetDate(string StrTemp)
        {
            string SETDate = "";

            return SETDate;
        }


        private void frmExpirationDateBarcode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Read_ComboSet();

            SCREEN_CLEAR();

            btnPrintExp.Visible = clsType.User.BuseCode.Equals("076010");


            chkOAuto.Checked = true;
        }

        private void chkOAuto_CheckedChanged(object sender, EventArgs e)
        {
            lblPODete.Text = "";
            lblPDate0.Text = "";

            date = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            if (chkOAuto.Checked == true)
            {
                lblPODete.Text = date.ToString("yyyy-MM-dd HH:mm");
                lblPDate0.Text = date.ToString("yy.MM.dd. HH시mm분");

                dtpODate.Enabled = false;
                dtpOTime0.Enabled = false;
            }
            else
            {
                lblPODete.Text = "";



                dtpODate.Enabled = true;
                dtpOTime0.Enabled = true;

                dtpODate.Value = date;
                dtpOTime0.Value = date;
                dtpOTime0.Value = date;
            }

            ComboCSet_Click();
        }

        private void BarcodePrintTeam() //바코드 프린트 인쇄 모듈
        {
            //'바코드 프린트 드리이브로 인쇄하는 모듈

            PrintDocument pd = new PrintDocument();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();

            string mstrPrintName = "혈액환자정보";
            string strPrintName1 = "";
            string strPrintName2 = "";

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(mstrPrintName.ToUpper());

                if (strPrintName2 == "")
                {
                    strPrintName2 = CP.getPrinter_Chk("S_LABEL".ToUpper());
                }
                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 의료정보과에 연락바랍니다.");
                    return;
                }
                pd.PrintController = pc;
                pd.PrinterSettings.PrinterName = strPrintName2;
                pd.PrintPage += new PrintPageEventHandler(ePrintPage);

                pd.Print();    //프린트             

                pd.PrinterSettings.PrinterName = strPrintName1;

                nPrint = 0;

            }
            catch (Exception Ex)
            {
                nPrint = 0;
                pd.PrinterSettings.PrinterName = strPrintName1;
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }

        }

        private void ePrintPage(object sender, PrintPageEventArgs e)
        {
            nPrint = nPrint + 1;

            e.Graphics.DrawString("개봉일시:" + lblPDate0.Text.Trim(), new Font("맑은 고딕", 9, FontStyle.Bold), Brushes.Black, 10, 20, new StringFormat());

            e.Graphics.DrawString("폐기일시:" + lblPDate1.Text.Trim(), new Font("맑은 고딕", 9, FontStyle.Bold), Brushes.Black, 10, 40, new StringFormat());

            e.Graphics.DrawString("유효기간:" + VB.Split(cboCSet.Text.Trim(), ".")[1], new Font("맑은 고딕", 9, FontStyle.Bold), Brushes.Black, 10, 60, new StringFormat());

            e.Graphics.DrawString("한달은 4주로 계산됨.", new Font("맑은 고딕", 9, FontStyle.Bold), Brushes.Black, 10, 80, new StringFormat());

            if (nPrint >= VB.Val(txtCnt.Text.Trim()))
            {
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
            }

        }

        private void MaxyyyymmddBarcodeLotation() //유효기간_바코드_위치정보
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            BarcodePrintTeam();
        }

        private void Read_ComboSet()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboCSet.Items.Clear();
                cboCSet.Items.Add("999. 임의유효기간");

                SQL = "SELECT CODE,NAME,CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='ETC_유효기간바코드설정값' ";
                SQL = SQL + ComNum.VBLF + "   AND ( DELDATE IS NULL OR DELDATE ='' ) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboCSet.SelectedIndex = -1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboCSet.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim() + "." + dt.Rows[i]["Cnt"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboCSet.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SCREEN_CLEAR()
        {
            date = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            lblPDate0.Text = "";

            dtpODate.Value = date;
            dtpOTime0.Value = date;
            dtpOTime1.Value = date;

            lblPCdate.Text = "";

            //'임의유효일시
            dtpCdate.Enabled = true;
            dtpCTime0.Enabled = true;
            dtpCTime1.Enabled = true;

            dtpCdate.Value = date;
            dtpCTime0.Value = date;
            dtpCTime1.Value = date;

            dtpODate.Enabled = false;
            dtpOTime0.Enabled = false;
            dtpCdate.Enabled = false;
            dtpCTime0.Enabled = false;

            lblPDate1.Text = dtpCdate.Value.ToString("yy.MM.dd. ") + dtpCTime0.Text.Trim() + "시" + dtpCTime1.Text.Trim() + "분";

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpODate_ValueChanged(object sender, EventArgs e)
        {
            lblPDate0.Text = dtpODate.Value.ToString("yy.MM.dd. ") + dtpOTime0.Text.Trim() + "시" + dtpOTime1.Text.Trim() + "분";
            ComboCSet_Click();
        }

        private void ComboCSet_Click()
        {
            date = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            //Call READ_SYSDATE

            lblPCdate.Text = "";
            lblPDate1.Text = "";

            if (VB.Left(cboCSet.Text.Trim(), 3).Trim() == "999")
            {
                //'임의유효일시
                dtpCdate.Enabled = true;
                dtpCTime0.Enabled = true;
                dtpCTime1.Enabled = true;

                dtpCdate.Value = date;
                dtpCTime0.Value = date;
                dtpCTime1.Value = date;

            }
            else
            {
                //'설정값 유효일시

                if (chkOAuto.Checked == true)
                {
                    lblPDate1.Text = Convert.ToDateTime(lblPODete.Text.Trim())
                        .AddDays(Convert.ToInt32(VB.Val(VB.Split(cboCSet.Text.Trim(), ".")[2])) / 24).ToString("yy.MM.dd. HH시mm분");
                }
                else
                {
                    lblPDate1.Text = Convert.ToDateTime(dtpODate.Text.Trim() + " " + dtpOTime0.Text.Trim() + ":" + dtpOTime1.Text.Trim())
                        .AddDays(Convert.ToInt32(VB.Val(VB.Split(cboCSet.Text.Trim(), ".")[2])) / 24).ToString("yy.MM.dd. HH시mm분");
                }

                dtpCdate.Enabled = false;
                dtpCTime0.Enabled = false;
                dtpCTime1.Enabled = false;

            }

            lblPSet.Text = VB.Split(cboCSet.Text.Trim(), ".")[1];
        }

        private void dtpCdate_ValueChanged(object sender, EventArgs e)
        {
            lblPDate1.Text = dtpCdate.Value.ToString("yy.MM.dd. ") + dtpCTime0.Text.Trim() + "시" + dtpCTime1.Text.Trim() + "분";
        }

        private void cboCSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboCSet_Click();
        }

        private void btnPrintExp_Click(object sender, EventArgs e)
        {
            BarcodePrintTeam_Exp();
        }

        private void BarcodePrintTeam_Exp() //바코드 프린트 인쇄 모듈
        {
            //'바코드 프린트 드리이브로 인쇄하는 모듈

            PrintDocument pd = new PrintDocument();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();

            string mstrPrintName = "혈액환자정보";
            string strPrintName1 = "";
            string strPrintName2 = "";

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(mstrPrintName.ToUpper());

                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 의료정보과에 연락바랍니다.");
                    return;
                }

                pd.PrintController = pc;
                pd.PrinterSettings.PrinterName = strPrintName2;
                pd.PrintPage += new PrintPageEventHandler(ePrintPage_Exp);

                pd.Print();    //프린트             

                pd.PrinterSettings.PrinterName = strPrintName1;

                nPrint = 0;

            }
            catch (Exception Ex)
            {
                nPrint = 0;
                pd.PrinterSettings.PrinterName = strPrintName1;
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }

        }

        private void ePrintPage_Exp(object sender, PrintPageEventArgs e)
        {
            nPrint = nPrint + 1;

            e.Graphics.DrawString("EXP:", new Font("굴림체", 20, FontStyle.Bold), Brushes.Black, 10, 20, new StringFormat());

            e.Graphics.DrawString(dtpCdate.Value.ToString("yyyy년MM월dd일"), new Font("굴림체", 18, FontStyle.Bold), Brushes.Black, 10, 60, new StringFormat());

            if (nPrint >= VB.Val(txtCnt.Text.Trim()))
            {
                e.HasMorePages = false;
            }
            else
            {
                e.HasMorePages = true;
            }

        }

        private void btnGaeC_Click(object sender, EventArgs e)
        {
            lblPDate0.Text = "────────";
        }
    }
}
