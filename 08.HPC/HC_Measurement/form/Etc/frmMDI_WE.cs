using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Model;
using HC.Core.Dto;
using HC_Core;
using HC_Core.Service;
using System;
using System.IO;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmMDI_WE : CommonForm
    {
        public delegate void ExitDelegate();
        public event ExitDelegate exitDelegate;
        public bool IsTest = false;

        clsHcFunc cHF = null;

        /// <summary>
        /// 실행중인 폼
        /// </summary>
        private CommonForm ActiveForm { get; set; }

        public frmMDI_WE()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmMDI_WE(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (exitDelegate != null)
            {
                this.exitDelegate();
            }
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.Job1_1.Click   += new EventHandler(eMenuClick);
            this.Job1_2.Click   += new EventHandler(eMenuClick);
            this.Job1_3.Click   += new EventHandler(eMenuClick);
            this.Job1_4.Click   += new EventHandler(eMenuClick);
            this.Job1_5.Click   += new EventHandler(eMenuClick);
            this.Job1_6.Click   += new EventHandler(eMenuClick);
            this.Job1_7.Click   += new EventHandler(eMenuClick);
            this.Job1_8.Click   += new EventHandler(eMenuClick);
            this.Job1_10.Click  += new EventHandler(eMenuClick);
            this.Job1_11.Click  += new EventHandler(eMenuClick);
            this.Job1_12.Click  += new EventHandler(eMenuClick);
            this.Job1_13.Click  += new EventHandler(eMenuClick);
            this.Job1_14.Click  += new EventHandler(eMenuClick);

            this.Job2_1.Click   += new EventHandler(eMenuClick);
            this.Job2_3.Click   += new EventHandler(eMenuClick);
            this.Job2_6.Click   += new EventHandler(eMenuClick);
            this.Job3_1.Click   += new EventHandler(eMenuClick);
            this.Job4_2.Click   += new EventHandler(eMenuClick);
            this.Job5_1.Click   += new EventHandler(eMenuClick);

            this.Job_Exit.Click += new EventHandler(eMenuClick);

            this.tabControl1.MouseUp += new MouseEventHandler(eMouseUp);
            this.tabControl1.SelectedIndexChanged += new EventHandler(eTabCtrlSelected);

            this.Mnu_TabClose.Click += new EventHandler(Mnu_TabClose_Click);
            this.Mnu_AllTabClose.Click += new EventHandler(Mnu_AllTabClose_Click);
            this.Mnu_OtherAllClose.Click += new EventHandler(Mnu_OtherAllClose_Click);
            this.Mnu_TabPopup.Click += new EventHandler(Mnu_TabPopup_Click);
        }

        private void eTabCtrlSelected(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count < 1)
            {
                return;
            }

            TabPage page = tabControl1.TabPages[tabControl1.SelectedIndex];

            foreach (Control ctrl in page.Controls)
            {
                if (ctrl is CommonForm)
                {
                    ActiveForm = ctrl as CommonForm;
                    return;
                }
            }
        }

        private void eMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (tabControl1.GetTabRect(tabControl1.SelectedIndex).Contains(e.X, e.Y))
                {
                    TabContextMenu.Show(tabControl1, e.X, e.Y);
                }
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            cHF.SET_자료사전_VALUE();

            //출장 노트북
            if (DataSyncService.Instance.IsLocalDB == true)
            {
                //MenuBaseCode.Enabled = false;
                //MenuChargeGroup.Enabled = false;
                //MenuNotebookManager.Enabled = true;

                this.Text = "보건관리전문 오프라인(로컬 DATABASE 사용중)";
            }
            else
            {
                //MenuNotebookManager.Enabled = false;
            }

            if (HC.Core.Service.CommonService.Instance.Session.UserId == "800594")
            {
                //MenuMig.Visible = true;
                //MenuBuild.Visible = true;
                //MenuNotebookManager.Visible = true;
                //MenuNotebookManager.Enabled = true;
            }

            HC_CODE hicCode = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
            if (hicCode.CodeName.NotEmpty())
            {
                DirectoryInfo di = new DirectoryInfo(hicCode.CodeName);
                if (di.Exists == false)
                {
                    di.Create();
                }
            }

            //SiteManageForm form = new SiteManageForm();
            ////SiteWorkerForm form = new SiteWorkerForm();
            //form.TopLevel = false;
            //form.Dock = DockStyle.Fill;
            //form.FormBorderStyle = FormBorderStyle.None;
            //tabControl1.TabPages[0].Controls.Clear();
            //tabControl1.TabPages[0].Controls.Add(form);
            //tabControl1.TabPages[0].Tag = form.Name;
            //form.BringToFront();
            //form.Show();

            //ActiveForm = form;
            //CreateForm(form, false);
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            //종료
            if (sender == Job_Exit)
            {
                this.Close();
                return;
            }
            //기초코드
            else if (sender == Job1_1)
            {
                new CodeManaerForm().Show();
            }
            //그룹코드 관리
            else if (sender == Job1_1)
            {
                new GroupCodeForm().Show();
            }
            //사용자관리
            else if (sender == Job1_2)
            {
                new UserManagerForm().Show();
            }
            //분석분류
            else if (sender == Job1_3)
            {
                new frmHcCode("HIC_CODE", "C0.작업환경측정 분석분류").Show();
            }
            //중심단어
            else if (sender == Job1_4)
            {
                new frmHcCode("HIC_CODE", "C1.작업환경측정 중심단어").Show();
            }
            //측정금액 (측정수수료와 통합)
            else if (sender == Job1_5)
            {
                new frmHcCode("HIC_CODE", "C3.작업환경측정 기초금액 2021년도").Show();
            }
            //측정수수료
            else if (sender == Job1_6)
            {
                frmHcChkSuga form = new frmHcChkSuga();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;
                
                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //유해요인
            else if (sender == Job1_7)
            {
                new frmHcChkUCode().Show();
            }
            //유해인자
            else if (sender == Job1_8)
            {
                frmHcChkMCode form = new frmHcChkMCode();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //사업장 관리
            else if (sender == Job1_10)
            {
                new frmHcLtdCode().Show();
            }
            //그룹코드 관리
            else if (sender == Job1_11)
            {
                new GroupCodeForm().Show();
            }
            //검진기초코드
            else if (sender == Job1_12)
            {
                new frmHcCode("HIC_CODE", "").Show();
            }
            //사업장 공장 구분
            else if (sender == Job1_13)
            {
                new frmHcCode("HIC_CODE", "C5.작업환경측정 공장명칭").Show();
            }
            //사업장 단위작업 구분
            else if (sender == Job1_14)
            {
                new frmHcCode("HIC_CODE", "C6.작업환경측정 단위작업명칭").Show();
            }
            //측정사업장 등록
            else if (sender == Job2_1)
            {
                frmHcChkLtd form = new frmHcChkLtd();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //예비조사표 등록
            else if (sender == Job2_3)
            {
                frmHcPreChkLtd form = new frmHcPreChkLtd();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //측정사업장 List
            else if (sender == Job2_6)
            {
                frmHcChkMain form = new frmHcChkMain();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //예비조사 일정등록
            else if (sender == Job3_1)
            {
                frmHcChkSchedule form = new frmHcChkSchedule();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text); 
            }
            //견적서 등록
            else if (sender == Job4_2)
            {
                frmHcChkEstimate form = new frmHcChkEstimate();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
            //측정입력
            else if (sender == Job5_1)
            {
                frmHcChkResult form = new frmHcChkResult();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            }
        }

        public void CreateForm(CommonForm form, bool isShowSiteTree = true, string menuName = "")
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Tag.To<string>("") == form.Name)
                {
                    tabControl1.SelectedTab = tabPage;
                    return;
                }
            }

            TabPage newTabPage = new TabPage();
            newTabPage.Text = menuName;
            newTabPage.Tag = form.Name;
            tabControl1.TabPages.Add(newTabPage);

            newTabPage.ControlRemoved += (obj, evt) =>
            {
                tabControl1.TabPages.Remove(obj as TabPage);
            };

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(form);
            form.Show();

            tabControl1.SelectedTab = newTabPage;
        }

        private void Mnu_TabClose_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

        private void Mnu_AllTabClose_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
        }

        private void Mnu_OtherAllClose_Click(object sender, EventArgs e)
        {
            for (int i = tabControl1.TabPages.Count - 1; i >= 0; i--)
            {
                if (tabControl1.SelectedTab != tabControl1.TabPages[i])
                {
                    tabControl1.TabPages.Remove(tabControl1.TabPages[i]);
                }
            }
        }

        private void Mnu_TabPopup_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count < 1)
            {
                return;
            }

            TabPage page = tabControl1.TabPages[tabControl1.SelectedIndex];

            CommonForm form = page.Controls[0] as CommonForm;
            form.Parent = null;
            form.TopLevel = true;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            form.Show();
        }
    }
}
