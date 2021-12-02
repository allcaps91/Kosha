using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkLtd_Entry :CommonForm
    {
        long FnWRTNO = 0;
        string FstrRowid = "";

        HcUserService hcUserService = null;
        HicLtdService hicLtdService = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukWorkerService hicChukWorkerService = null;

        public frmHcChkLtd_Entry()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkLtd_Entry(long argWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = argWRTNO;
        }

        private void SetControl()
        {
            hcUserService = new HcUserService();
            hicLtdService = new HicLtdService();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukWorkerService = new HicChukWorkerService();

            cboBangi.Items.Clear();
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");

            cboSupport.Items.Clear();
            cboSupport.Items.Add("적용무");
            cboSupport.Items.Add("적용");

            dtpChkSDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.SDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            dtpChkEDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.EDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });

            nmrGjYear.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.CHKYEAR), Min = 0 });

            SS1.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 30 });
            SS1.AddColumnCheckBox("삭제", nameof(HIC_CHUK_WORKER.IsDelete),     45, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSS1_ChkButtonClick;
            SS1.AddColumn("일련번호",   nameof(HIC_CHUK_WORKER.WRTNO),          74, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("작업자사번", nameof(HIC_CHUK_WORKER.WORKER_SABUN),   74, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("작업자명",   nameof(HIC_CHUK_WORKER.WORKER_NAME),   100, new SpreadCellTypeOption { });
            SS1.AddColumn("역할",       nameof(HIC_CHUK_WORKER.ROLE),          140, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("자격번호",   nameof(HIC_CHUK_WORKER.CERTNO),        220, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("비고",       nameof(HIC_CHUK_WORKER.BIGO),          180, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID",      nameof(HIC_CHUK_WORKER.RID),            74, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            panMain.SetEnterKey();
        }

        private void eSS1_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUK_WORKER code = SS1.GetRowData(e.Row) as HIC_CHUK_WORKER;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnAdd.Click += new EventHandler(eBtnClick);
            this.SS1.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.txtLtdCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtLtdCode.Text.Trim() != "")
                {
                    txtSangho.Text = hicLtdService.GetNamebyCode(txtLtdCode.Text);
                }
            }
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            int nRow = SS1.ActiveSheet.ActiveRowIndex;

            string strSName = SS1.ActiveSheet.Cells[nRow, 3].Text;

            if (!strSName.IsNullOrEmpty())
            {
                HC_USER item = hcUserService.FindByName(strSName);

                if (!item.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[nRow, 1].Text = FnWRTNO.To<string>("0");
                    SS1.ActiveSheet.Cells[nRow, 2].Text = item.UserId;
                    SS1.ActiveSheet.Cells[nRow, 3].Text = item.Name;
                    SS1.ActiveSheet.Cells[nRow, 4].Text = item.Role;
                    SS1.ActiveSheet.Cells[nRow, 5].Text = item.CERTNO;

                    SS1.AddRows(1);
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnAdd)
            {
                SS1.AddRows(1);
            }
            else if (sender == btnDelete)
            {
                Data_Save("DEL");
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Data_Save(string argMode = "")
        {
            if (!panMain.RequiredValidate())
            {
                MessageBox.Show("필수 입력항목이 누락되었습니다.");
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //측정사업장 등록
                HIC_CHUKMST_NEW item = panMain.GetData<HIC_CHUKMST_NEW>();
                item.RID = FstrRowid;
                item.WRTNO = FnWRTNO;
                item.ENTSABUN = clsType.User.IdNumber.To<long>(0);

                if (cboBangi.SelectedIndex == 0)
                {
                    item.BANGI = "1";
                }
                else
                {
                    item.BANGI = "2";
                }

                if (cboSupport.SelectedIndex == 0)
                {
                    item.GBSUPPORT = "1";
                }
                else
                {
                    item.GBSUPPORT = "2";
                }

                if (argMode == "DEL")
                {
                    item.RowStatus = ComBase.Mvc.RowStatus.Delete;
                    item.DELSABUN = clsType.User.IdNumber.To<long>(0);
                }

                hicChukMstNewService.Save(item);

                //측정자, 분석자 내역 등록
                IList<HIC_CHUK_WORKER> list = SS1.GetEditbleData<HIC_CHUK_WORKER>();

                if (list.Count > 0)
                {
                    if (hicChukWorkerService.Save(list, FnWRTNO))
                    {
                        MessageBox.Show("저장하였습니다");
                    }
                    else
                    {
                        MessageBox.Show("오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            this.panMain.AddRequiredControl(nmrGjYear);
            this.panMain.AddRequiredControl(txtLtdCode);
            this.panMain.AddRequiredControl(cboBangi);
            this.panMain.AddRequiredControl(cboSupport);

            panMain.Initialize();

            cboBangi.SelectedIndex = 0;
            cboSupport.SelectedIndex = 0;
            dtpEntDate.Text = DateTime.Now.ToShortDateString();

            SS1.AddRows(1);

            if (FnWRTNO > 0)
            {
                Screen_Display();
            }
        }

        private void Screen_Display()
        {
            HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(FnWRTNO);

            if (!item.IsNullOrEmpty())
            {
                panMain.SetData(item);

                FstrRowid = item.RID;

                if (item.BANGI.To<string>("") == "1")
                {
                    cboBangi.SelectedIndex = 0;
                }
                else
                {
                    cboBangi.SelectedIndex = 1;
                }

                if (item.GBSUPPORT.To<string>("") == "1")
                {
                    cboSupport.SelectedIndex = 0;
                }
                else
                {
                    cboSupport.SelectedIndex = 1;
                }

                List<HIC_CHUK_WORKER> lsthCW = hicChukWorkerService.GetListByWrtno(FnWRTNO);

                SS1.DataSource = lsthCW;
            }
        }
    }
}
