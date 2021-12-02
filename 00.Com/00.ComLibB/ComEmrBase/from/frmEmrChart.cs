using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComEmrBase
{
    public partial class frmEmrChart : Form, FormEmrMessage
    {
        private string mstrFilePath = clsType.SvrInfo.strClient + "\\FormToImage\\";


        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public string mstrFormNo = "0";
        public string mstrUpdateNo = "0";
        public string mstrFormName = "";
        public string mstrProgForm = "";
        public EmrPatient AcpEmr = null;
        public EmrForm frmFORM = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        public bool mblnMax = false;
        #endregion

        //private Form ActiveForm = null;
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string mCloseOption = "";

        private EmrChartForm ActiveFormViewChart = null;

        #region //FormEmrMessage
        public FormEmrMessage mEmrCallForm = null;
        public void MsgSave(string strSaveFlag)
        {

        }
        public void MsgDelete()
        {

        }
        public void MsgClear()
        {

        }
        public void MsgPrint()
        {

        }
        #endregion


        #region //공통함수

        private void viewFormFit()
        {
            Screen[] screens = Screen.AllScreens;
            Screen first_screen = null;
            int intWidth = 0;
            int intHeight = 0;

            foreach (Screen screen in screens)
            {
                if (screen.Primary == true)
                {
                    first_screen = screen;
                    intWidth = first_screen.Bounds.Width;
                    intHeight = first_screen.Bounds.Height;
                    break;
                }
            }

            if (this.Height < intHeight - 40)
            {
                return;
            }
            this.Height = intHeight - 40;
            this.Show();
        }

        private void pSetUserOption()
        {
            string optMcrAllFlag = "3";
            DataTable dt = null;

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag");
            if (dt == null)
            {
                optMcrUser.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                optMcrUser.Checked = true;
            }
            else
            {
                optMcrAllFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
                dt = null;
                if (optMcrAllFlag == "1")
                {
                    optMcrAll.Checked = true;
                }
                else if (optMcrAllFlag == "2")
                {
                    optMcrDept.Checked = true;
                }
                else
                {
                    optMcrUser.Checked = true;
                }
            }

            string optMcrAddFlag = "2";

            dt = clsQuery.GetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag");
            if (dt == null)
            {
                optMcrAdd.Checked = true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                optMcrAdd.Checked = true;
            }
            else
            {
                optMcrAddFlag = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                dt.Dispose();
                dt = null;
                if (optMcrAddFlag == "2")
                {
                    optMcrRpl.Checked = true;
                }
                else
                {
                    optMcrAdd.Checked = true;
                }
            }
        }

        #endregion 

        public frmEmrChart()
        {
            InitializeComponent();
        }

        public frmEmrChart(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            AcpEmr = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmEmrChart(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, bool blnMax)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            AcpEmr = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mblnMax = blnMax;
        }

        private void frmEmrChart_Load(object sender, EventArgs e)
        {
            if (mblnMax == true) this.WindowState = FormWindowState.Maximized;
            if (AcpEmr != null)
            {
                this.Text = this.Text + "      " + AcpEmr.ptName + "(" + AcpEmr.ptNo + ")";
            }

            lblFormName.Text = "";
            //폼정보를 가지고 온다.
            if (mstrFormNo != "")
            {
                frmFORM = clsEmrChart.ClearEmrForm();
                frmFORM = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);
                lblFormName.Text = frmFORM.FmFORMNAME;
                //폼정보로 옵션 표시할지 여부 판단
                ViewChart(AcpEmr, frmFORM, mstrEmrNo, mstrEmrNo, "", "0");
            }

            pSetUserOption();
            viewFormFit();
        }

        private void frmEmrChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ActiveForm != null)
                {
                    ActiveForm.Dispose();
                    //ActiveForm = null;
                }
            }
            catch { }

            if (mCloseOption != "U")
            {
                mCloseOption = "U";
                rEventClosed();
                return;
            }
        }

        private void frmEmrChart_Resize(object sender, EventArgs e)
        {
            if (ActiveForm != null)
            {
                if (frmFORM.FmALIGNGB == 1)   //Left
                {
                    ActiveForm.Height = panEmrMain.Height - 20;
                }
                else if (frmFORM.FmALIGNGB == 2)  //Top
                {
                    ActiveForm.Width = panEmrMain.Width - 20;
                }
                //ActiveForm.Refresh(); 
            }
        }

        #region //옵션 버튼

        private void optMcrAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAll.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "1") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "1";
            }
        }

        private void optMcrDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrDept.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "2";
            }
        }

        private void optMcrUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrUser.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAllFlag", "3") == true)
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }
        }

        private void optMcrAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrAdd.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag", "1") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "1";
            }
        }

        private void optMcrRpl_CheckedChanged(object sender, EventArgs e)
        {
            if (optMcrRpl.Checked == false)
            {
                return;
            }
            if (clsQuery.SetEmrUserOption(clsDB.DbCon, clsType.User.IdNumber, "ErmMain", "optMcrAddFlag", "2") == true)
            {
                clsEmrPublic.gstrMcrAddFlag = "2";
            }
        }


        #endregion

        private void ViewChart(EmrPatient tAcp, EmrForm tForm, string strEmrNo, string strTreatNo, string strSCANYN, string strFormCode)
        {
            if (ActiveForm != null)
            {
                ActiveForm.Dispose();
                ActiveForm = null;
            }

            if (strSCANYN == "S")
            {
                ActiveForm = new frmScanImageViewNew(this, strTreatNo, strFormCode, "0");
            }
            else
            {
                ActiveForm = new frmEmrChartNew(tAcp.formNo.ToString(), tAcp.updateNo.ToString(), strEmrNo, 0);
            }

            ActiveFormViewChart = (EmrChartForm)ActiveForm;
            ActiveForm.TopLevel = false;
            this.Controls.Add(ActiveForm);
            ActiveForm.Parent = panEmrMain;
            ActiveForm.Text = tForm.FmFORMNAME;
            ActiveForm.ControlBox = false;
            ActiveForm.FormBorderStyle = FormBorderStyle.None;
            ActiveForm.Top = 0;
            ActiveForm.Left = 0;
            if (strSCANYN == "S")
            {
                ActiveForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                if (tForm.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    ActiveForm.Height = panEmrMain.Height;
                }
                else if (tForm.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    ActiveForm.Width = panEmrMain.Width;
                }
                else  //None
                {
                    ActiveForm.Dock = DockStyle.Fill;
                }
            }
            ActiveForm.Show();

        }

    }
}
