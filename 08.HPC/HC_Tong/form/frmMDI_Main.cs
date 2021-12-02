/// =====================================   Project Decription   ================================================
/// 폼 전역 작업 타이머 명칭 : Job_Timer
/// Timer에 의한 작업 스케쥴을 구현하도록 한다.
/// Main Form에 작업목록을 추가하여 UI에서 작업 진행상황을 알 수 있게 구현한다.
/// Main Job Routine 에서 분기하여 각 작업별로 나뉘게 구현한다.
/// 업무 구분 및 영역이 다른 경우 별도의 클래스로 나누어 관리한다. (ex: 접수업무 clsTongBuild_Jepsu,  판정업무 clsTongBuild_Pan ...)
/// 기본적으로 통계 빌드시 Slip 개념으로 Data를 빌드하여 추적이 가능하도록 구현하되, 단순 집계나 명단형성은 제외할 수 있다.
/// 각 작업별 수동으로 Data Build 가 가능하도록 구현하는것을 권장한다. (폼의 메뉴로 접근하는 방식)
/// =============================================================================================================
using ComBase;
using ComDbB;
using ComHpcLibB;
using HC_OSHA;
using HC_Core;
using System;
using System.Data;
using System.Windows.Forms;
using HC.Core.Dto;

namespace HC_Tong
{
    public partial class frmMDI_Main : Form, MainFormMessage
    {
        string mPara1 = "";

        clsHcFunc cHF = null;
        Timer Job_Timer = null;
        bool isWorking = false;

        #region //MainFormMessage

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }
        public void MsgUnloadForm(Form frm)
        {
        }
        public void MsgFormClear()
        {
        }
        public void MsgSendPara(string strPara)
        {
        }
        #endregion //MainFormMessage


        public frmMDI_Main()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();
            Job_Timer = new Timer();
            Job_Timer.Interval = 1000;      //1초 틱
            Job_Timer.Tick += Job_Timer_Tick;
        }

        private void Job_Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            if (currentDateTime.Hour == 16 && currentDateTime.Minute == 6)
            {
                if(isWorking == false)
                {
                    isWorking = true;
                    BuildMenuOsha01();

                    BuildMenuOsha02();

                    BuildMenuOsha03();
                }
                isWorking = false;
            }
                
        }

        private void SetEvent()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnExit.Click  += new EventHandler(eBtnClick);
            this.btnStart.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnStart)
            {
                Set_Job_Timer(true);
            }

        }

        private void eMenuClick(object sender, EventArgs e)
        {
            //if (sender == menuJep01)
            //{
            //    if (frmHCMain == null)
            //    {
            //        frmHCMain = new frmHcJepMain();
            //        themTabForm(frmHCMain, this.panMain);
            //    }
            //    else
            //    {
            //        if (FormIsExist(frmHCMain) == true)
            //        {
            //            FormVisiable(frmHCMain);
            //        }
            //        else
            //        {
            //            frmHCMain = new frmHcJepMain();
            //            themTabForm(frmHCMain, this.panMain);
            //        }
            //    }
            //}
           
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        /// <summary>
        /// Main 폼에서 폼이 로드된 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool FormIsExist(Form frm)
        {
            foreach (Control ctl in this.panMain.Controls)
            {
                if (ctl is Form)
                {
                    if (ctl.Name == frm.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Set_Job_Timer(false);
        }

        private void Set_Job_Timer(bool bJob)
        {
            if (bJob)
            {
                Job_Timer.Start();
                btnStart.Text = "중 지";
            }
            else
            {
                Job_Timer.Stop();
                btnStart.Text = "시 작";
            }
            
        }

        /// <summary>
        /// Description : MDI내 폼 활성화시 기본값 세팅
        /// Author : 김민철
        /// Create Date : 2020.03.02
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        void SetMDIFormTitle(PsmhDb pDbCon, string pForm)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (pForm == "frmBoard" || pForm == "frmSplash")
            {
                return;
            }

            SQL = "";
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    PROJECTNAME, FORMNAME, FORMNAME1, FORMAUTH, FORMAUTHTEL ";
            SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + pForm + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string[] arryAssName = VB.Split(dt.Rows[0]["PROJECTNAME"].ToString().Trim(), ".");
            string strAssName = arryAssName[0];
            strAssName = strAssName + ".dll";

            string strPROJECTNAME = dt.Rows[0]["PROJECTNAME"].ToString().Trim();
            string strFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
            string strFORMAUTH = dt.Rows[0]["FORMAUTH"].ToString().Trim();
            string strFORMAUTHTEL = dt.Rows[0]["FORMAUTHTEL"].ToString().Trim();
            dt.Dispose();
            dt = null;

            string strUpdateIniFile = @"C:\PSMHEXE\PSMHAutoUpdate.ini";
            clsIniFile myIniFile = new clsIniFile(strUpdateIniFile);
            double dblVerClt = myIniFile.ReadValue("DEFAULT_UPDATE_LIST", strAssName, 0);

            //lblTitle.Text = "재원심사 메인 : ";
            //lblTitle.Text += strFORMNAME + " (" + strFORMAUTH + " ☎ " + strFORMAUTHTEL + ")"
            //            + VB.Space(6) + " (" + strPROJECTNAME + " : Ver " + dblVerClt.ToString() + ")";
            //lblTitle.ForeColor = System.Drawing.Color.White;
            return;
        }

        private void menuOsha01_Click(object sender, EventArgs e)
        {
            HcPanGenMedExamResultBuildForm form = new HcPanGenMedExamResultBuildForm();
            form.ShowDialog();
        }
        private void menuOsha02_Click(object sender, EventArgs e)
        {
            HcPanSpcDiagnosisResultReportBuildForm form = new HcPanSpcDiagnosisResultReportBuildForm();
            form.ShowDialog();

        }

        private void menuOsha03_Click(object sender, EventArgs e)
        {
            PanjengBuildForm form = new PanjengBuildForm();
            form.ShowDialog();
        }

        /// <summary>
        /// 일반건강진단 유소견자수 빌드
        /// </summary>
        private void BuildMenuOsha01()
        {
            HcPanGenMedExamResultBuildForm form = new HcPanGenMedExamResultBuildForm();
            form.Show();
            form.Build(true);
            form.Close();

        }
        /// <summary>
        /// 특수건강진단 유소견자수 빌드
        /// </summary>
        private void BuildMenuOsha02()
        {
            HcPanSpcDiagnosisResultReportBuildForm form = new HcPanSpcDiagnosisResultReportBuildForm();
            form.Show();
            form.Build(true);
            form.Close();

        }
        /// <summary>
        /// 대행 질병유소견자 사후관리 빌드
        /// </summary>
        private void BuildMenuOsha03()
        {
            PanjengBuildForm form = new PanjengBuildForm();
            form.Show();
            form.Search();
            form.CheckAll();
            form.Build(true);
            form.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuildMenuOsha01();
        }
    }
}
