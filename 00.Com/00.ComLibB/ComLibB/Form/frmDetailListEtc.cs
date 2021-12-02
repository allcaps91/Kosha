using System;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmDetailListEtc : Form
    {
        public frmDetailListEtc()
        {
            InitializeComponent();
        }


        void frmDetailListEtc_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lblMsg.Text = "";
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strHead = "";

            string strFont1 = "";
            string strFont2 = "";


            strHead = "/l/f1" + VB.Space(35) + "약, 주사 표준수가 와 비교 LIST " + "/n/n";

            strFont1 = "/l/fn\"굴림체\" /fz\"10\"";
            strFont2 = "/n/fn\"굴림체\" /fb0/fu0/fz\"11\"";


            ssList_Sheet1.PrintInfo.Header = strFont1 + "/n/c" + lblMsg.Text + " 상세명단";
            ssList_Sheet1.PrintInfo.Header += strFont2 + "/n";

            ssList_Sheet1.PrintInfo.Margin.Top = 250;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 0;
            ssList_Sheet1.PrintInfo.Margin.Left = 350;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;

            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = true;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = true;
            ssList_Sheet1.PrintInfo.UseMax = false;
            ssList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.Preview = true;
            ssList.PrintSheet(0);
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strTemp = "";

            if (e.Column == 3)
            {
                strTemp = ssList_Sheet1.Cells[e.Row, 0].Text;
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.InitialDirectory = @"C:\cmc\ocsviewer\ocsviewer.exe";
                if (OpenFile.InitialDirectory != "")
                {
                    //TODO
                    //EXECUTE_EMR(strTemp)
                }
            }
            else
            {
                //FrameDetail.Visible = False
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
