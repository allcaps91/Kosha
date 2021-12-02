using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaVIPMgmt.cs
/// Description     : VIP 관리
/// Author          : 이상훈
/// Create Date     : 2019-10-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmVip관리.frm(FrmVip관리)" />

namespace ComHpcLibB
{
    public partial class frmHaVIPMgmt : Form
    {
        HeaJepsuPatientSunapService heaJepsuPatientSunapService = null;

        frmHcMemo FrmHcMemo = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        clsHaBase ha = new clsHaBase();
        ComFunc cf = new ComFunc();



        public frmHaVIPMgmt()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            heaJepsuPatientSunapService = new HeaJepsuPatientSunapService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-30).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            cboAmt.Items.Clear();
            cboAmt.Items.Add("1.100만원~199만원");
            cboAmt.Items.Add("2.200만원 이상");
            cboAmt.Items.Add("3.전체");

            rdoJob1.Checked = true;
            cboAmt.SelectedIndex = 2;
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                cboAmt.Visible = false;
            }
            else if (sender == rdoJob2)
            {
                cboAmt.Visible = true;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nAmt = 0;
                string strOK = "";

                string strFrDate = "";
                string strToDate = "";
                string strJob = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob1.Checked == true)
                {
                    strJob = "2";
                }

                List<HEA_JEPSU_PATIENT_SUNAP> list = heaJepsuPatientSunapService.GetItemsbySDate(strFrDate, strToDate, strJob);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    nAmt = list[i].TOTAMT;
                    strOK = "OK";
                    if (rdoJob1.Checked == true)
                    {
                        if (list[i].VIPREMARK.Trim() == "")
                        {
                            strOK = "";
                        }
                    }
                    else
                    {
                        if (VB.Left(cboAmt.Text, 1) == "1")
                        {
                            if (nAmt < 1000000 || nAmt >= 2000000)
                            {
                                strOK = "";
                            }
                        }
                        else if (VB.Left(cboAmt.Text, 1) == "2")
                        {
                            if (nAmt < 2000000)
                            {
                                strOK = "";
                            }
                        }
                        else
                        {
                            if (nAmt < 1000000)
                            {
                                strOK = "";
                            }
                        }
                    }

                    if (strOK == "OK")
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = list[i].SDATE;
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].AGE + "/" + list[i].SEX;
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].LTDNAME;
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].VIPREMARK;
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].HPHONE;
                        SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:N0}", nAmt);
                        SS1.ActiveSheet.Cells[i, 8].Text = list[i].WRTNO.ToString();
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "종합건진 VIP 수검자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("수검일자:" + dtpFrDate.Text + "~" + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                //strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            long nWrtNo = 0;

            if (e.Column >= 0 && e.Row >= 0)
            {
                if (sender == SS1)
                {
                    if (SS1.ActiveSheet.Cells[e.Row, 7].Text == "True")
                    {
                        SS1.ActiveSheet.Cells[e.Row, 7].Text = "";
                        nWrtNo = long.Parse(SS1.ActiveSheet.Cells[e.Row, 8].Text);
                        FrmHcMemo = new frmHcMemo(nWrtNo.To<string>());
                        FrmHcMemo.ShowDialog(this);
                        FrmHcMemo.Dispose();
                        FrmHcMemo = null;
                    }
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.Column != 7)
                {
                    //clsHcVariable.GstrPanWRTNO = SS1.ActiveSheet.Cells[e.Row, 8].Text;
                    //frmHaExamResultReg_New f = new frmHaExamResultReg_New();
                    //f.ShowDialog(this);
                    //this.Hide();
                }
            }
        }
    }
}
