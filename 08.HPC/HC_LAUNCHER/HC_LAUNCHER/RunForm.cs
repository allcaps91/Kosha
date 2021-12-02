using CefSharp;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Repository;
using HC_Core;
using HC_OSHA;
using HC_OSHA.StatusReport;
using HC_OSHA.Visit;
using System;
using System.Windows.Forms;

namespace HC_LAUNCHER
{
    public partial class RunForm : Form
    {
        public RunForm()
        {
            InitializeComponent();
        }



        private void 사용자관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 코드관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CodeManaerForm().Show();
        }

        private void 사용자관리ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            new UserManagerForm().Show();
        }


       

        private void 사업장관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SiteManageForm().Show();
        }

        private void 그룹코드관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GroupCodeForm().Show();
        }

        private void 일정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CalendarForm().Show();
        }

        private void mSDS기초코드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MsdsForm().Show();
        }



        private void 기타업무대시보드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Dashboard().Show();
        }

        private void 근로자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new IndustrialAccidentForm().Show();
        }

        private void RunForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }

        private void 보건교육지원대장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EducationReportForm().Show();
        }

        private void 사업장관리카드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SiteManagementCardForm().Show();
        }

        private void 상태보고서간호사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StatusReportByNurseForm().Show();
        }

        private void 상태보고서산업위생기사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StatusReportByEngineer().Show();
        }

        private void 상태보고서의사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StatusReportByDoctor().Show();
        }

        private void MSDS제품관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DailyReportForm().Show();
        }

        private void 사업자마스터ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ddddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HicResBohum1Repository repo = new HicResBohum1Repository();
            HIC_RES_BOHUM1 ss =      repo.GetItemByWrtno(1014515);
        }
    }
}
