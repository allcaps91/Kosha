using ComBase;
using ComDbB;
using ComHpcLibB;
using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Bill
{
    public partial class frmMDI_Admin_Old :Form, MainFormMessage
    {
        string mPara1 = "";

        clsHcFunc cHF = null;

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

        public frmMDI_Admin_Old()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmMDI_Admin_Old(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmMDI_Admin_Old(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        private void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();

            //tabForm.TabCount = 0;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.Job1_1.NodeClick += new EventHandler(eNodeClick);
            this.Job1_2.NodeClick += new EventHandler(eNodeClick);
            this.Job1_3.NodeClick += new EventHandler(eNodeClick);
            this.Job1_4_1.NodeClick += new EventHandler(eNodeClick);
            this.Job1_4_2.NodeClick += new EventHandler(eNodeClick);
            this.Job1_5.NodeClick += new EventHandler(eNodeClick);
            this.Job1_6_1.NodeClick += new EventHandler(eNodeClick);
            this.Job1_6_2.NodeClick += new EventHandler(eNodeClick);
            this.Job1_7_1.NodeClick += new EventHandler(eNodeClick);
            this.Job1_7_2.NodeClick += new EventHandler(eNodeClick);
            this.Job1_7_3.NodeClick += new EventHandler(eNodeClick);
            this.Job1_8.NodeClick += new EventHandler(eNodeClick);
            this.Job1_9.NodeClick += new EventHandler(eNodeClick);
            this.Job1_10.NodeClick += new EventHandler(eNodeClick);
            this.Job1_11.NodeClick += new EventHandler(eNodeClick);
            this.Job1_12.NodeClick += new EventHandler(eNodeClick);

            this.Job_Exit.NodeClick += new EventHandler(eNodeClick);
        }

        private void eNodeClick(object sender, EventArgs e)
        {
            string pForm = "";

            if (sender == Job1_1)
            {
                frmHcCode frm = new frmHcCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_2)
            {
                frmHcExCode frm = new frmHcExCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_3)
            {
                frmHcLtdCode frm = new frmHcLtdCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_4_1)
            {
                frmHcGJong frm = new frmHcGJong();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_4_2)
            {
                frmHaGJong frm = new frmHaGJong();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_5)
            {
                frmHaGamCode frm = new frmHaGamCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_6_1)
            {
                frmHcGroupCode frm = new frmHcGroupCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_6_2)
            {
                frmHaGroupCode frm = new frmHaGroupCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_7_1)
            {
                frmHcGroupExam frm = new frmHcGroupExam();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_7_2)
            {
                frmHaGroupExam frm = new frmHaGroupExam();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_7_3)
            {
                frmHcMCodeExam frm = new frmHcMCodeExam();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_8)
            {
                frmHcResCode frm = new frmHcResCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_9)
            {
                frmHcDoctor frm = new frmHcDoctor();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_10)
            {
                frmHcLtdMCode frm = new frmHcLtdMCode();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_11)
            {
                frmHcMatterEntry frm = new frmHcMatterEntry();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job1_12)
            {
                frmHcSuga frm = new frmHcSuga();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job4_1)
            {
                frmHcSecondList frm = new frmHcSecondList();
                themTabForm(frm);
                pForm = frm.Name;
                frm.btnExit.Click += tabClose_Click;
            }
            else if (sender == Job_Exit)
            {
                this.Close();
                return;
            }
            else
            {
                SetMDIFormTitle(clsDB.DbCon, pForm); //폼 기본값 세팅 등
            }
        }

        public void themTabForm(Form frm)
        {
            SuperTabControlPanel tabPanel = new SuperTabControlPanel();
            SuperTabItem tabItem = new SuperTabItem();

            int index = kitemratontai(tabForm, frm);

            if (index >= 0)
                tabForm.SelectedTabIndex = index;
            else
            {
                // Loading Form 설정
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Dock = DockStyle.Fill;

                // Panel에 Form추가
                tabPanel.Controls.Add(frm);

                // Item에 Panel 추가
                tabItem.AttachedControl = tabPanel;

                tabForm.Tabs.Add(tabItem);
                tabForm.Controls.Add(tabPanel);
                tabItem.Text = frm.Text;
                tabItem.Name = frm.Name;
                tabItem.FixedTabSize = new System.Drawing.Size(tabItem.Text.Length * 16, 0);

                tabItem.Click += new EventHandler(eTabClick);

                tabItem.MouseDown += new MouseEventHandler(tab_MouseClick);

                frm.Show();

                // 추가된 Tab 선택.
                tabForm.SelectedTabIndex = tabForm.Tabs.Count - 1;
            }
        }

        private void eTabClick(object sender, EventArgs e)
        {
            if (((SuperTabItem)sender).Name != "")
            {
                SetMDIFormTitle(clsDB.DbCon, ((SuperTabItem)sender).Name); //폼 기본값 세팅 등
            }
        }

        private void tab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                //마우스 오른쪽 메뉴를 생성
                ContextMenu m = new ContextMenu();
                //메뉴에 들어갈 아이템 생성
                MenuItem m1 = new MenuItem();
                MenuItem m2 = new MenuItem();

                m1.Text = "닫기";
                m2.Text = "이 창을 제외하고 모두닫기";

                m1.Click += new EventHandler(tabClose_Click);
                m2.Click += new EventHandler(tabAllOtherClose_Click);


                m.MenuItems.Add(m1);
                m.MenuItems.Add(m2);

                m.Show(tabForm, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void tabClose_Click(object sender, EventArgs e)
        {
            tabForm.Tabs.Remove(tabForm.SelectedTab);
        }


        private void tabAllOtherClose_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tabForm.Tabs.Count; i++)
            {
                if (i != tabForm.SelectedTabIndex)
                    tabForm.Tabs.RemoveAt(i--);
            }
        }

        /// <summary>
        /// Description : MDI내 폼 활성화시 기본값 세팅
        /// Author : 박병규
        /// Create Date : 2017.11.24
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        private void SetMDIFormTitle(PsmhDb pDbCon, string pForm)
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

            lblTitle.Text = "원무현황조회 : ";
            lblTitle.Text += strFORMNAME + " (" + strFORMAUTH + " ☎ " + strFORMAUTHTEL + ")"
                        + VB.Space(6) + " (" + strPROJECTNAME + " : Ver " + dblVerClt.ToString() + ")";
            lblTitle.ForeColor = System.Drawing.Color.White;
            return;

        }

        private int kitemratontai(SuperTabControl tabForm, Form Frm)
        {
            for (int i = 0; i < tabForm.Tabs.Count; i++)
            {
                if (tabForm.Tabs[i].Text == Frm.Text.Trim())
                {
                    return i;
                }
            }
            return -1;
        }

        private void eFormload(object sender, EventArgs e)
        {
            cHF.SET_자료사전_VALUE();

            menuTree.ExpandAll();
        }
    }
}
