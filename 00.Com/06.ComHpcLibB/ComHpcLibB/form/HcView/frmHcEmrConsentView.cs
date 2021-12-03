using ComBase;
using ComBase.Controls;
using ComEmrBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcEmrConsentView :Form
    {

        string fstrSname = "";

        ComHpcLibBService comHpcLibBService = null;

        Panel ctrlPan;
        EasManager easManager;
        List<frmEasViewer> lstFEView = new List<frmEasViewer>();

        public frmHcEmrConsentView()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcEmrConsentView(string strSNAME)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fstrSname = strSNAME;
        }

        private void SetControl()
        {
            comHpcLibBService = new ComHpcLibBService();

            ssList.Initialize(new SpreadOption { ColumnHeaderHeight = 30, RowHeaderVisible = true });
            ssList.AddColumn("등록번호", nameof(COMHPC.PTNO),     88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("수검자명", nameof(COMHPC.SNAME),    84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("나이",     nameof(COMHPC.AGE),      40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("검진일자", nameof(COMHPC.JEPDATE),  88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("구분",     nameof(COMHPC.DEPTCODE), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFromClosing);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eFromClosing(object sender, FormClosingEventArgs e)
        {
            if (lstFEView.Count > 0)
            {
                for (int i = 0; i < lstFEView.Count; i++)
                {
                    lstFEView[i].Close();
                    lstFEView[i].Dispose();
                }

                lstFEView.Clear();
                lstFEView = null;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (!e.RowHeader && !e.ColumnHeader)
            {
                //폼 메모리 해제 초기화
                if (lstFEView.Count > 0)
                {
                    for (int i = 0; i < lstFEView.Count; i++)
                    {
                        lstFEView[i].Close();
                        lstFEView[i].Dispose();
                    }

                    lstFEView.Clear();
                }

                tbCtrl.TabPages.Clear();

                string strPtno = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                string strFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                string strTDate = Convert.ToDateTime(strFDate).AddDays(1).ToShortDateString();
                string strDept = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                string strIO = "O";

                //strPtno = "81000004";
                //strFDate = "2020-12-29";
                //strDept = "MP";
                //strIO = "I";

                //전자동의서 EMRNO 가져오기
                List<EMR_CONSENT> lstEmrNo = comHpcLibBService.GetEmrNoByPtnoDate(strPtno, strFDate, strTDate);
                
                if (lstEmrNo.Count > 0)
                {
                    for (int i = 0; i < lstEmrNo.Count; i++)
                    {
                        if (tbCtrl.TabPages.Count < (i + 1))
                        {
                            tbCtrl.TabPages.Add(lstEmrNo[i].FORMNAME.To<string>(""));
                        }
                        else
                        {
                            tbCtrl.TabPages[i].Text = lstEmrNo[i].FORMNAME.To<string>("");
                        }

                        ctrlPan = new Panel();
                        frmEasViewer fEasViewer;

                        tbCtrl.TabPages[i].Controls.Add(ctrlPan);
                        ctrlPan.Parent = tbCtrl.TabPages[i];
                        ctrlPan.Dock = DockStyle.Fill;

                        EmrForm tForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, lstEmrNo[i].FORMNO.To<string>("")); //개인정보동의서(데이터가 없으면 NULL 반환 됩니다.)
                        EmrPatient tAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, strIO, strFDate, strDept);

                        easManager = new EasManager();
                        fEasViewer = easManager.GetEasFormViewer();
                        easManager.View(tForm, tAcp, tForm.FmFORMNO, lstEmrNo[i].ID.To<string>(""), true);
                        //easManager.View(tForm, tAcp, tForm.FmFORMNO, lstEmrNo[i].ID.To<string>(""), false );

                        // easManager.Print(tForm, tAcp, xxlstEmrNo[i].ID.To<string>(""));

                        fEasViewer.TopLevel = false;
                        fEasViewer.FormBorderStyle = FormBorderStyle.None;
                        fEasViewer.Parent = ctrlPan;
                        fEasViewer.Dock = DockStyle.Fill;

                        fEasViewer.Show();

                        //자원해제를 위하여 List에 담아두고 호출시 폼 메모리 해제
                        lstFEView.Add(fEasViewer);

                        tForm = null;
                        tAcp = null;
                    }
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            //dtpFrDate.Text = DateTime.Now.AddDays(-3).ToShortDateString();
            dtpFrDate.Text = DateTime.Now.ToShortDateString();
            dtpToDate.Text = DateTime.Now.ToShortDateString();

            txtSName.Text = "";
            if (!fstrSname.IsNullOrEmpty())
            {
                txtSName.Text = fstrSname;
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                Display_List();
            }
        }

        private void Display_List()
        {
            List<COMHPC> list = comHpcLibBService.GetListByDate(dtpFrDate.Text, dtpToDate.Text, txtSName.Text.Trim());

            if (list.Count > 0)
            {
                ssList.DataSource = list;
            }

            if (list.Count == 1 && !txtSName.Text.IsNullOrEmpty())
            {
                eSpdDblClick(ssList, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
            }
        }
    }
}
