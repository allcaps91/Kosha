
using CefSharp.Structs;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Repository;
using HC.Core.Dto;
using HC.OSHA.Dto;
using HC_Core;
using HC_Core.Service;
using HC_OSHA.form.Etc;
using HC_OSHA.form.Report;
using HC_OSHA.form.Schedule;
using HC_OSHA.form.Visit;
using HC_OSHA.StatusReport;
using HC_OSHA.Visit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class Dashboard : CommonForm
    {
        public delegate void ExitDelegate();
        public event ExitDelegate exitDelegate;
        public bool IsTest = false;
        public bool FbCodeManager = false;  //코드관리 여부
        public bool FbLtdUser = false;      //관계사 사용자 여부

        /// <summary>
        /// 실행중인 폼
        /// </summary>
        private CommonForm ActiveForm { get; set; }
     
        public Dashboard()
        {
            InitializeComponent();
            if (VB.Mid(clsType.User.Jikmu, 3, 1)=="Y") FbCodeManager = true;
            if (clsType.User.LtdUser != "") FbLtdUser = true;
            if (FbCodeManager == false)
            {
                MenuBaseCode.Visible = false;  //코드관리
            }
            if (FbLtdUser == true)
            {
                사업장관리ToolStripMenuItem.Visible = false;
                사업장등록ToolStripMenuItem.Visible = false; //거래처코드
                일정관리ToolStripMenuItem.Visible = false; //일정관리
                상태보고서의사용ToolStripMenuItem.Visible = false;
                상태보고서간호사용ToolStripMenuItem.Visible = false;
                상태보고서산업위생기사용ToolStripMenuItem.Visible = false;
                출장일지인쇄ToolStripMenuItem.Visible = false;
                수입일보인쇄ToolStripMenuItem.Visible = false;
                검진결과ToolStripMenuItem.Visible = false;
                MenuChargeGroup.Visible = false;
                MenuBaseCode.Visible = false;  //코드관리
                toolStripMenuItem1.Visible = false;  //비밀번호변경
            }
            보건교육지원대장ToolStripMenuItem1.Enabled = true;
            //검진결과ToolStripMenuItem.Visible = false;

            //비밀번호가 1234이면 업무 제한
            if (clsType.User.PassWord=="1234")
            {
                사업장ToolStripMenuItem.Visible = false;
                일정관리ToolStripMenuItem.Visible = false;
                상태보고서ToolStripMenuItem.Visible = false;
                toolStripMenuItem2.Visible = false;
                검진결과ToolStripMenuItem.Visible = false;
                통계및대장ToolStripMenuItem.Visible = false;
                MenuChargeGroup.Visible = false;
                MenuBaseCode.Visible = false;  //코드관리
                toolStripMenuItem1.Visible = true;  //비밀번호변경

                ComFunc.MsgBox("비밀번호를 변경 후 사용하세요!", "알림");
            }
            //미완성 프로그램 감추기
            if (clsType.User.IdNumber != "1")
            {
                검진결과ToolStripMenuItem.Visible = false;
                작업환경측정관리ToolStripMenuItem.Visible = false;
                //사업장현황ToolStripMenuItem.Visible = false;
                //사업장별상담대장ToolStripMenuItem.Visible = false;
                //수령증발급대장ToolStripMenuItem.Visible = false;
                근골격계지원대장ToolStripMenuItem.Visible = false;
                //정보자료제공ToolStripMenuItem.Visible = false;

                건강증진프로그램ToolStripMenuItem.Visible = false;
                직무스트레스평가ToolStripMenuItem.Visible = false;
                근골격계유해요인조사ToolStripMenuItem.Visible = false;
                청력보존프로그램ToolStripMenuItem1.Visible = false;
                밀폐공간프로그램ToolStripMenuItem.Visible = false;
                기업건강증진지수EHPToolStripMenuItem.Visible = false;
            }
        }

        public Dashboard(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }
        
        private void Dashboard_Load(object sender, EventArgs e)
        {

            HC_CODE hicCode = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
            if(hicCode.CodeName.NotEmpty()){
                DirectoryInfo di = new DirectoryInfo(hicCode.CodeName);
                if(di.Exists == false)
                {
                    di.Create();
                }
            }
            string ss = clsType.HosInfo.SwLicense; 
            SiteManageForm form = new SiteManageForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            tabControl1.TabPages[0].Controls.Clear();
            tabControl1.TabPages[0].Controls.Add(form);
            tabControl1.TabPages[0].Tag = form.Name;
            form.BringToFront();
            form.Show();

            ActiveForm = form;
            //CreateForm(form, false);
        }

        private void OshaSiteEstimateList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ActiveForm != null)
            {
                if(ActiveForm is ISelectEstimate)
                {
                    (ActiveForm as ISelectEstimate).Select(oshaSiteEstimateList.GetEstimateModel);
                }
            }
            base.SelectedEstimate = oshaSiteEstimateList.GetEstimateModel;
        }

        private bool SiteValidate()
        {
            bool result = true;
            if (base.SelectedSite == null)
            {
                result = false;
                MessageUtil.Info("사업장을 선택하세요");
            }
            return result;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count < 1)
            {
                return;
            }
            TabPage page = tabControl1.TabPages[tabControl1.SelectedIndex];
            foreach(Control ctrl in page.Controls)
            {
                if(ctrl is CommonForm)
                {
                    ActiveForm = ctrl as CommonForm;
                    return;
                }
            }
        }

        public void CreateForm(CommonForm form, bool isShowSiteTree = true, string menuName = "")
        {
            //if (ActiveForm != null)
            //{
            //    ActiveForm.Close();
            //    ActiveForm.Dispose();
            //}

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Tag.Equals(form.Name))
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

            if (isShowSiteTree)
            {
                Panel panel = new Panel();
                panel.Width = 200;
                panel.Dock = DockStyle.Left;
                panel.Padding = new Padding(0, 0, 5, 0);
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(panel);

                OshaSiteLastTree tree = new OshaSiteLastTree();
                tree.NodeClick += OshaSiteLastTree_NodeClick;
                tree.Dock = DockStyle.Left;
                //tree.BringToFront();
                tree.Dock = DockStyle.Top;
                tree.Height = 540;
                tree.Show();

                panel.Controls.Add(tree);

                OshaSiteEstimateList siteEstimateList = new OshaSiteEstimateList();
                siteEstimateList.Name = "siteEstimateList";
                panel.Controls.Add(siteEstimateList);
                siteEstimateList.Dock = DockStyle.Fill;
                siteEstimateList.Show();
                siteEstimateList.BringToFront();

                siteEstimateList.CellDoubleClick += OshaSiteEstimateList_CellDoubleClick;

                form.Shown += (o, s) =>
                {
                    if (base.SelectedSite != null)
                    {
                        if (form is ISelectSite)
                        {
                            (form as ISelectSite).Select(base.SelectedSite);
                            siteEstimateList.SearhAndDoubleClik(base.SelectedSite.ID, false);
                        }
                    }
                };
            }

            tabControl1.SelectedTab = newTabPage;
            //ActiveForm = form;

            //this.ActiveForm = AddForm(form, panFrame);
            //if (isShowSiteTree)
            //{

            //    LeftPanel.Visible = true;
            //    LeftPanel.Width = 199;
            //}
            //else
            //{

            //    LeftPanel.Visible = false;
            //    LeftPanel.Width = 0;
            //}
        }

        /// <summary>
        /// 사업장 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OshaSiteLastTree_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OshaSiteLastTree tree = (sender as TreeView).Parent.Parent as OshaSiteLastTree;
            if (ActiveForm != null)
            {
                if(ActiveForm is ISelectSite)
                {
                    //(ActiveForm as ISelectSite).Select(oshaSiteLastTree.GetSite);
                    (ActiveForm as ISelectSite).Select(tree.GetSite);
                }
            }

            //base.SelectedSite = oshaSiteLastTree.GetSite;
            //oshaSiteEstimateList.SearhAndDoubleClik(oshaSiteLastTree.GetSite.ID, false);
            
            base.SelectedSite = tree.GetSite;

            Panel panel = tree.Parent as Panel;
            Control[] controls = panel.Controls.Find("siteEstimateList", true);
            if(controls.Length > 0 && controls[0] is OshaSiteEstimateList)
            {
                (controls[0] as OshaSiteEstimateList).SearhAndDoubleClik(tree.GetSite.ID, false);
            }
            else
            {
                oshaSiteEstimateList.SearhAndDoubleClik(tree.GetSite.ID, false);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            HC_OSHA_CONTRACT dto = panFrame.GetData<HC_OSHA_CONTRACT>();
            long xxxx = dto.WORKERTOTALCOUNT;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            HC_OSHA_CONTRACT dto = new HC_OSHA_CONTRACT();
            dto.WORKERTOTALCOUNT = 11;
            panFrame.SetData(dto);

            long  xxxx = dto.WORKERTOTALCOUNT;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            panFrame.Initialize();
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
         
        }

        private void 근로자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteWorkerForm form = new SiteWorkerForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
        }

        private void 대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void 화학물질ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void 월별일정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new CalendarForm().Show();
            ScheduleRegisterForm form = new ScheduleRegisterForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            //form.Show();
            CreateForm(form, false, (sender as ToolStripMenuItem).Text);

            ISelectSite site = (form as ISelectSite);
            if (site != null)
            {
                site.Select(oshaSiteLastTree.GetSite);
            }
            
        }

        private void 사업장관리카드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SiteManagementCardForm().Show();
        }

        private void 사업장관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteManageForm form = new SiteManageForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            //form.Show();
            CreateForm(form, false, (sender as ToolStripMenuItem).Text);
            ISelectSite site = (form as ISelectSite);
            if (site != null)
            {
                site.Select(oshaSiteLastTree.GetSite);
            }
        }

        private void 출장일지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DailyReportForm().Show();
        }

        private void 보건교육지원대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmEduReport().Show();
        }

        private void 상태보고서의사용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //   new StatusReportByDoctor().Show();


            StatusReportByDoctor form = new StatusReportByDoctor();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            form.Show();
            //ISelectSite site = (form as ISelectSite);
            //if (site != null)
            //{
            //   // site.Select(oshaSiteLastTree.GetSite);
            //}

        }

        private void 상태보고서간호사용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   new StatusReportByNurseForm().Show();

            StatusReportByNurseForm form = new StatusReportByNurseForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            //  CreateForm(form);
            form.Show();
            ISelectSite site = (form as ISelectSite);
            if (site != null)
            {
               // site.Select(oshaSiteLastTree.GetSite);
            }
        }

        private void 상태보고서산업위생기사용ToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   new StatusReportByEngineer().Show();
            StatusReportByEngineer form = new StatusReportByEngineer();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            //   CreateForm(form);
            form.Show();
            ISelectSite site = (form as ISelectSite);
            if (site != null)
            {
               // site.Select(oshaSiteLastTree.GetSite);
            }
        }

        private void mSD코드관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MsdsForm().Show();
        }

        private void 코그관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CodeManaerForm().Show();
        }

        private void 그룹코드관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GroupCodeForm().Show();
        }

        private void 사용자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UsermstForm().Show();
        }

        private void 방문일정공문ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VIsitDocumentBatchForm form = new VIsitDocumentBatchForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            form.Show();
            //CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
        }

        private void 사업장관리카드ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SiteManagementCardForm form = new SiteManagementCardForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            form.Show();
            ISelectSite site = (form as ISelectSite);
            if (site != null)
            {
                site.Select(oshaSiteLastTree.GetSite);
            }

        }

        private void mSDS관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //    if (SiteValidate())
            {
                SiteMSDSListForm form = new SiteMSDSListForm();
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;
                CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
                //(form as ISelectSite).Select(oshaSiteLastTree.GetSite);
                //    CommonService.Instance.AddForm(panFrame, form);
            }
        }

        private void 출장일지인쇄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DailyReportForm().Show();
        }

        private void 일반질병유소견자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcPanOpinionAfterMgmtGen().Show();
        }

        private void 보건교육지원대장ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new FrmEduReport().Show();
        }

        private void 일반검진결과표ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcPanGenMedExamResult_New().Show();
        }

        private void 특수검진결과표ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcPanSpcDiagnosisResultReport().Show();
        }

        private void 대행질병유소견자사후관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcPanOpinionAfterMgmtGenSpc().Show();
        }

        private void 특수질병유소견자사후관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcPanOpinionAfterMgmtSpc().Show();

        }

        private void 사업장등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 장비사용대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmJangbiReport().Show();
        }

        private void 뇌심혈관발병위험도평가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrainRiskEvalution form = new frmBrainRiskEvalution();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            (form as ISelectSite).Select(oshaSiteLastTree.GetSite);
        }

        private void 업무관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 마이그레이션ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 자동미수형성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmCharge().Show();
        }

        private void 청구빌드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OshaChargeForm().Show();
        }

        private void 청구공문인쇄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ChargeDocumentBatchForm().Show();
        }

        private void 대행빌드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void 대행빌드ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
      
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (DataSyncService.Instance.IsLocalDB == true)
            //{
            //    this.exitDelegate();
            //}
            if (exitDelegate != null)
            {
                //this.exitDelegate();
                this.Close();
            }
        }

    

        private void 일반검진표빌드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HcPanGenMedExamResultBuildForm().Show();
        }

        private void 특수검진결과표ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new frmHcPanSpcDiagnosisResultReport().Show();
        }

        private void 특수검진표빌드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HcPanSpcDiagnosisResultReportBuildForm().Show();
        }

        private void 질병유소견자대행빌드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanjengBuildForm form = new PanjengBuildForm();
            form.Show();
        }

        private void 일반검진가접수자명단ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcGaJepsuVIew().Show();
        }

        private void 수입일보인쇄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IncomeReport form = new IncomeReport();
            //form.SelectedSite = base.SelectedSite;
            //form.SelectedEstimate = base.SelectedEstimate;
            //CreateForm(form);
            //(form as ISelectSite).Select(oshaSiteLastTree.GetSite);
            new IncomeReport().Show();

        }

        private void ryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HC_Main 참조시 순환참조 오류
            //new frmHa_HcAmtHalin().Show();
            
            HicGroupcodeRepository r = new HicGroupcodeRepository();
            //List<HIC_GROUPCODE> = r.GetHalinListByItems("2020 - 10 - 20", "", );
        }

        private void 사업장등록ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new frmHcLtdCode().Show();
        }

        private void 노트북으로DB가져오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StartExport().ShowDialog();
        }

        private void 원내서버로DB올리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StartImport().ShowDialog();
        }

        private void 검진출장스케쥴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcSchedule().Show();
        }

        private void 일반검진접수명단ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HC_Main 참조시 순환참조 오류
            //new frmHcJepsuView().Show();
        }

        private void 차검진대상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmHcSecondList().Show();
        }

        private void 검진결과ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 사업장원하청관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SiteRelationForm().Show();
        }

        private void 사업장DB가져오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 방문주기조회ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SCheduleRegularForm().Show();
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(tabControl1.GetTabRect(tabControl1.SelectedIndex).Contains(e.X, e.Y))
                {
                    TabContextMenu.Show(tabControl1, e.X, e.Y);
                }
            }
        }

        private void tabControl1_ControlRemoved(object sender, ControlEventArgs e)
        {

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
            for(int i = tabControl1.TabPages.Count - 1; i>=0; i--)
            {
                if(tabControl1.SelectedTab != tabControl1.TabPages[i])
                {
                    tabControl1.TabPages.Remove(tabControl1.TabPages[i]);
                }
            }
        }

        private void Mnu_TabPopup_Click(object sender, EventArgs e)
        {
            if(tabControl1.TabPages.Count < 1)
            {
                return;
            }

            TabPage page = tabControl1.TabPages[tabControl1.SelectedIndex];

            CommonForm form = page.Controls[0] as CommonForm;
            form.Parent = null;
            form.TopLevel = true;
            form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            if(page.Controls.Count > 0 && page.Controls[0] is Panel)
            {
                Panel panel = page.Controls[0] as Panel;
                form.Controls.Add(panel);
                panel.Dock = DockStyle.Left;

                OshaSiteEstimateList list = null;
                if (panel.Controls[0] is OshaSiteEstimateList)
                {
                    list = panel.Controls[0] as OshaSiteEstimateList;
                    list.CellDoubleClick -= OshaSiteEstimateList_CellDoubleClick;
                    list.CellDoubleClick += (o, evt) =>
                    {
                        if (ActiveForm != null)
                        {
                            if (list.TopLevelControl is ISelectEstimate)
                            {
                                (list.TopLevelControl as ISelectEstimate).Select(list.GetEstimateModel);
                            }
                        }
                    };
                }

                if(panel.Controls[1] is OshaSiteLastTree)
                {
                    OshaSiteLastTree tree = panel.Controls[1] as OshaSiteLastTree;
                    tree.NodeClick -= OshaSiteLastTree_NodeClick;
                    tree.NodeClick += (o, evt) =>
                    {
                        OshaSiteLastTree t = (o as TreeView).Parent.Parent as OshaSiteLastTree;

                        if(t.TopLevelControl is ISelectSite)
                        {
                            (t.TopLevelControl as ISelectSite).Select(t.GetSite);
                        }

                        list.SearhAndDoubleClik(t.GetSite.ID, false);
                    };
                }
            }
            form.Show();
        }

        private void panFrame_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new FrmPassChange().Show();
        }

        private void 산재현황대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSanjeReport().Show();
        }

        private void 사업장현황ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSaupjang form = new FrmSaupjang();
            form.Show();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 뇌심혈관계발병위험도평가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmExcelUpload2 form = new FrmExcelUpload2();
            form.Show();
        }

        private void 질병유소견자사후관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmExcelUpload3 form = new FrmExcelUpload3();
            form.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FrmExcelUpload1 form = new FrmExcelUpload1();
            form.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FrmSahusogen form = new FrmSahusogen();
            form.Show();
            //CreateForm(form, menuName: (sender as ToolStripMenuItem).Text);
            //(form as ISelectSite).Select(oshaSiteLastTree.GetSite);
        }

        private void oshaSiteLastTree_Load(object sender, EventArgs e)
        {

        }

        private void 사업장계약금액ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 사업장계약금액ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SitePriceManageForm form = new SitePriceManageForm();
            form.Show();
        }

        private void 사업장별상담대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSangdamReport().Show();
        }

        private void 방문날짜조회ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmVisitCheck().Show();
        }

        private void 수령증발급대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSuryengReport().Show();
        }

        private void 정보자료제공ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmJengboReport().Show();
        }

        private void 위탁업무수행일지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSuhangReport().Show();
        }

        private void 네트웍상태점검ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmPingTest().Show();
        }
    }
}
