using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using System.Drawing;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB.form.PmpaView
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWJicwonmisu.cs
    /// Description     : 접수번호별 삭감 조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 조회 부분 모든 코드 주석처리 컨버전 불가
    /// </history>
    /// <seealso cref= "d:\psmh\misu\misubs\misubs.vbp\misubs64.frm(FrmReMirView4)  >> frmPmPaVIEWJicwonmisu.cs 폼이름 재정의" />	

    public partial class frmPmPaVIEWReMir : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu pm = new clsPmpaMisu();
        clsPmpaFunc CPF = new clsPmpaFunc();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string GstrSakYYMM = "";
        string GstrSakGBN = "";
        string GstrSakIO = "";
        string GstrSakJohap = "";

        public frmPmPaVIEWReMir(string strSakYYMM, string strSakGBN, string strSakIO, string strSakJohap)
        {
            GstrSakYYMM = strSakYYMM;
            GstrSakGBN = strSakGBN;
            GstrSakIO = strSakIO;
            GstrSakJohap = strSakJohap;

            InitializeComponent();
        }

        public frmPmPaVIEWReMir()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWReMir_Load(object sender, EventArgs e)
        {
            ssView_Sheet1.Columns[14].Visible = false;
            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.RowCount = 0;
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 36, "", "1");

            cboJong.Items.Clear();
            cboJong.Items.Add("0 . 전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2 .의료급여");

            cboJong.SelectedIndex = 0;

            if (GstrSakYYMM != "")
            {
                if (GstrSakGBN == "1")
                {
                    rdoJob0.Checked = true;
                }
                if (GstrSakGBN == "2")
                {
                    rdoJob1.Checked = true;
                }

                switch (GstrSakIO)
                {
                    case "I":
                        rdoIO1.Checked = true;        //'입원
                        break;
                    case "O":
                        rdoIO2.Checked = true;       //'외래
                        break;
                    default:
                        rdoIO0.Checked = true;
                        break;
                }

                switch (GstrSakJohap)
                {
                    case "1":
                        cboJong.Text = "1.건강보험";
                        break;
                    case "5":
                        cboJong.Text = "2.의료급여";
                        break;
                    default:
                        cboJong.Text = "0.전체";
                        break;
                }

                cboYYMM.Text = VB.Left(GstrSakYYMM, 4) + " 년" + VB.Mid(GstrSakYYMM, 5, 2) + "월";

            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {

        }

        private void btnReView_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            { return; }     //권한확인

            strTitle = "신 자 감 액 명 단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 27, 0);
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
