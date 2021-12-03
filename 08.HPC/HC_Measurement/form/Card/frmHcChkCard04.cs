using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard04 :CommonForm
    {
        long FnWRTNO = 0;
        long FnLtdCode = 0;

        clsSpread cSpd = null;

        HicLtdService hicLtdService = null;
        HicChkMcodeService hicChkMcodeService = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukWorkerService hicChukWorkerService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HicChukDtlChemicalService hicChukDtlChemicalService = null;
        HicChukDtlSubltdService hicChukDtlSubltdService = null;
        
        public frmHcChkCard04()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard04(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetControl()
        {
            cSpd = new clsSpread();

            hicLtdService = new HicLtdService();
            hicChkMcodeService = new HicChkMcodeService();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukWorkerService = new HicChukWorkerService();
            hicChukDtlPlanService = new HicChukDtlPlanService();
            hicChukDtlChemicalService = new HicChukDtlChemicalService();
            hicChukDtlSubltdService = new HicChukDtlSubltdService();

            #region 측정사업장 협력업체 List
            ssSUBLTD.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssSUBLTD.AddColumnCheckBox("선택", nameof(HIC_CHUKDTL_SUBLTD.IsActive), 44, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false }).ButtonClick += eSSSUBLTD_ChkButtonClick;
            ssSUBLTD.AddColumn("일련번호", nameof(HIC_CHUKDTL_SUBLTD.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("측정사업장", nameof(HIC_CHUKDTL_SUBLTD.LTDCODE), 48, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("협력업체사업장코드", nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDCODE), 88, new SpreadCellTypeOption { });
            ssSUBLTD.AddColumn("협력업체명", nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDNAME), 220, new SpreadCellTypeOption { IsEditble = false });
            ssSUBLTD.AddColumn("비고", nameof(HIC_CHUKDTL_SUBLTD.REMARK), 220, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, TextMaxLength = 25 });
            ssSUBLTD.AddColumn("ROWID", nameof(HIC_CHUKDTL_SUBLTD.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

        }

        private void eSSSUBLTD_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUKDTL_SUBLTD code = ssSUBLTD.GetRowData(e.Row) as HIC_CHUKDTL_SUBLTD;

            ssSUBLTD.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnAdd.Click += new EventHandler(eBtnClick);
            this.btnSave_SubLtd.Click += new EventHandler(eBtnClick);

            this.ssSUBLTD.EditModeOff += new EventHandler(eSpdEditModeOff);
        }

        private void Spread_DelList_Display(bool bDel, FpSpread spd)
        {
            if (bDel)
            {
                for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
                {
                    if (spd.ActiveSheet.Cells[i, 1].Text == "Y")
                    {
                        TextCellType spdObj = new TextCellType();
                        spd.ActiveSheet.Cells[i, 0].CellType = spdObj;
                        spd.ActiveSheet.Cells[i, 0].Locked = true;
                        spd.ActiveSheet.Cells[i, 0].Text = "";
                        cSpd.setSpdForeColor(spd, i, 0, i, spd.ActiveSheet.ColumnCount - 1, Color.DarkRed);
                    }
                    else
                    {
                        CheckBoxCellType spdObj = new CheckBoxFlagEnumCellType<IsActive>();
                        spd.ActiveSheet.Cells[i, 0].CellType = spdObj;
                        cSpd.setSpdForeColor(spd, i, 0, i, spd.ActiveSheet.ColumnCount - 1, Color.Black);
                    }
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ssSUBLTD.DataSource = new List<HIC_CHUKDTL_SUBLTD>();

            ssSUBLTD.AddRows(1);

            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }

        private void Screen_Display(long argWRTNO)
        {
            if (argWRTNO == 0) { return; }

            //계약내용이 있다면 출력
            HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(argWRTNO);

            if (!item.IsNullOrEmpty())
            {
                FnLtdCode = item.LTDCODE;

                //협력업체 현황
                List<HIC_CHUKDTL_SUBLTD> lstHCS = hicChukDtlSubltdService.GetListByWrtno(argWRTNO);
                ssSUBLTD.DataSource = lstHCS;
            }
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            if (sender == ssSUBLTD)
            {
                int nRow = ssSUBLTD.ActiveSheet.ActiveRowIndex;
                string strLtdCode = ssSUBLTD.ActiveSheet.Cells[nRow, 3].Text.Trim();

                if (!strLtdCode.IsNullOrEmpty())
                {
                    ssSUBLTD.ActiveSheet.Cells[nRow, 4].Text = hicLtdService.GetNamebyCode(strLtdCode);
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave_SubLtd)
            {
                Data_Save_SubLtd();
            }
            else if (sender == btnAdd)
            {
                int nRow = ssSUBLTD.ActiveSheet.ActiveRowIndex + 1;
                ssSUBLTD.InsertRows(nRow);
                return;
            }
        }

        private void Set_Spread_Seqno_Sorting(FpSpread spd, int nCol)
        {
            int nSeq = 1;
            int nColCnt = spd.ActiveSheet.ColumnCount - 1;

            for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
            {
                if ((RowStatus)((IList)spd.DataSource)[i].GetPropertieValue("RowStatus") != RowStatus.Delete)
                {
                    if (spd.ActiveSheet.Cells[i, 0].Text != "Y")
                    {
                        if (spd.ActiveSheet.Cells[i, nColCnt].Text.Trim() != "")
                        {
                            spd.ActiveSheet.Cells[i, nCol].Value = nSeq;
                            ((IList)spd.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Update);
                            nSeq += 1;
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[i, nCol].Value = nSeq;
                            ((IList)spd.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                            nSeq += 1;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 협력업체 사업장 등록
        /// </summary>
        private void Data_Save_SubLtd()
        {
            if (FnWRTNO == 0) { return; }

            //협렵업체 내역
            IList<HIC_CHUKDTL_SUBLTD> list4 = ssSUBLTD.GetEditbleData<HIC_CHUKDTL_SUBLTD>();

            if (list4.Count > 0)
            {
                if (!hicChukDtlSubltdService.Save(list4, FnWRTNO, FnLtdCode))
                {
                    MessageBox.Show("협력업체 내역 등록중 오류가 발생하였습니다. ");
                    return;
                }
                else
                {
                    MessageBox.Show("저장완료. ");
                }
            }
        }

        private void Data_Save(string argMode = "")
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //협렵업체 내역
                IList<HIC_CHUKDTL_SUBLTD> list4 = ssSUBLTD.GetEditbleData<HIC_CHUKDTL_SUBLTD>();

                if (list4.Count > 0)
                {
                    if (argMode == "DEL")
                    {
                        foreach (HIC_CHUKDTL_SUBLTD code in list4)
                        {
                            code.RowStatus = ComBase.Mvc.RowStatus.Delete;
                        }
                    }

                    if (!hicChukDtlSubltdService.Save(list4, FnWRTNO, FnLtdCode))
                    {
                        MessageBox.Show("협력업체 내역 등록중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장완료. ");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

    }
}
