using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkCard03 :CommonForm
    {
        long FnWRTNO = 0;
        string FstrComCode = "";
        string  FstrComName = "";
        string  FstrComGCode = "";
        string  FstrComGCode1 = "";

        clsSpread cSpd = null;
        clsHcSpd cHSpd = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukDtlChemicalService hicChukDtlChemicalService = null;

        public frmHcChkCard03()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard03(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cHSpd = new clsHcSpd();

            hicChukMstNewService = new HicChukMstNewService();
            hicChukDtlChemicalService = new HicChukDtlChemicalService();
            
            #region 측정사업장 공정별 유해화학물질 사용 실태
            SpreadComboBoxData TREATMENT_UNIT = hiccodeService.GetSpreadComboBoxData("C2");

            ssCHEMICAL.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssCHEMICAL.AddColumnCheckBox("선택", nameof(HIC_CHUKDTL_CHEMICAL.IsActive), 44, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false });
            ssCHEMICAL.AddColumn("삭제", nameof(HIC_CHUKDTL_CHEMICAL.IsDelete), 44, new SpreadCellTypeOption { IsVisivle = false });
            ssCHEMICAL.AddColumn("일련번호", nameof(HIC_CHUKDTL_CHEMICAL.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssCHEMICAL.AddColumnNumber("순번", nameof(HIC_CHUKDTL_CHEMICAL.SEQNO), 48, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssCHEMICAL.AddColumn("공정코드", nameof(HIC_CHUKDTL_CHEMICAL.PROCESS), 62, new SpreadCellTypeOption { IsEditble = false });
            ssCHEMICAL.AddColumn("공정명", nameof(HIC_CHUKDTL_CHEMICAL.PROCESS_NM), 130, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssCHEMICAL.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssCHEMICAL.AddColumn("화학물질명(상품명)", nameof(HIC_CHUKDTL_CHEMICAL.PRODUCT_NM), 150, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("제조 또는 사용 여부", nameof(HIC_CHUKDTL_CHEMICAL.GBUSE), 72, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("사용 용도", nameof(HIC_CHUKDTL_CHEMICAL.USAGE), 130, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnNumber("월 취급량", nameof(HIC_CHUKDTL_CHEMICAL.TREATMENT), 48, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnComboBox("Kg/톤", nameof(HIC_CHUKDTL_CHEMICAL.UNIT), 52, IsReadOnly.N, TREATMENT_UNIT, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("비고", nameof(HIC_CHUKDTL_CHEMICAL.REMARK), 84, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("ROWID", nameof(HIC_CHUKDTL_CHEMICAL.RID), 74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

        }

        private void eValueCode_Chmcls(string strCode, string strName, string strGCode, string strGCode1, string TWA_PPM, string TWA_MG, string STEL_PPM, string STEL_MG, string UNIT)
        {
            FstrComCode = strCode;
            FstrComName = strName;
            FstrComGCode = strGCode;
            FstrComGCode1 = strGCode1;
        }

        private void ssGONG_HELP(object sender, EditorNotifyEventArgs e)
        {
            string strGubun = "";
            string strKeyWord = "";

            FstrComCode = "";
            FstrComName = "";
            FstrComGCode = "";
            FstrComGCode1 = "";

            if (sender == ssCHEMICAL)
            {
                strGubun = "GONG";
                strKeyWord = ssCHEMICAL.ActiveSheet.Cells[e.Row, 5].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

            if (!FstrComCode.IsNullOrEmpty())
            {
                if (sender == ssCHEMICAL)
                {
                    ssCHEMICAL.ActiveSheet.Cells[e.Row, 3].Text = FstrComCode.Trim();
                    ssCHEMICAL.ActiveSheet.Cells[e.Row, 4].Text = FstrComCode.Trim();
                    ssCHEMICAL.ActiveSheet.Cells[e.Row, 5].Text = FstrComName.Trim();
                }
            }
        }

        private void eValueCode(string strCode, string strName, string strGCode, string strGCode1)
        {
            FstrComCode = strCode;
            FstrComName = strName;
            FstrComGCode = strGCode;
            FstrComGCode1 = strGCode1;
        }

        private void eSS_Delete(object sender, EditorNotifyEventArgs e)
        {
            if (sender == ssCHEMICAL)
            {
                HIC_CHUKDTL_CHEMICAL code = ssCHEMICAL.GetRowData(e.Row) as HIC_CHUKDTL_CHEMICAL;

                ssCHEMICAL.DeleteRow(e.Row);
            }

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnIns.Click += new EventHandler(eBtnClick);
            this.btnAdd.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnSave_Chemical.Click += new EventHandler(eBtnClick);
            this.chkDel_Gong.CheckedChanged += new EventHandler(eChkChanged);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ssCHEMICAL.DataSource = new List<HIC_CHUKDTL_CHEMICAL>();
            ssCHEMICAL.AddRows(1);
         
            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }

        private void eChkChanged(object sender, EventArgs e)
        {
            if (sender == chkDel_Gong)
            {
                //유해인자별 측정계획
                List<HIC_CHUKDTL_CHEMICAL> lstHCU = hicChukDtlChemicalService.GetListByWrtno(FnWRTNO, chkDel_Gong.Checked);
                ssCHEMICAL.DataSource = null;
                ssCHEMICAL.SetDataSource(lstHCU);

                Spread_DelList_Display(chkDel_Gong.Checked, ssCHEMICAL);
            }
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

        private void Screen_Display(long argWRTNO)
        {
            if (argWRTNO == 0) { return; }

            ssCHEMICAL.DataSource = null;
            ssCHEMICAL.ActiveSheet.RowCount = 0;

            //계약내용이 있다면 출력
            HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(argWRTNO);

            if (!item.IsNullOrEmpty())
            {
                //화학물질 사용실태
                List<HIC_CHUKDTL_CHEMICAL> lstHCC = hicChukDtlChemicalService.GetListByWrtno(argWRTNO, chkDel_Gong.Checked);
                ssCHEMICAL.DataSource = lstHCC;
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave_Chemical)
            {
                Data_Save();
            }
            else if (sender == btnAdd || sender == btnIns)
            {
                Spread_AddInsert(sender, ssCHEMICAL);
                return;
            }
            else if (sender == btnDelete)
            {
                bool bChk = false;   
                if (MessageBox.Show("선택항목을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < ssCHEMICAL.ActiveSheet.RowCount; i++)
                {
                    if (ssCHEMICAL.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        HIC_CHUKDTL_PLAN code = ssCHEMICAL.GetRowData(i) as HIC_CHUKDTL_PLAN;

                        ssCHEMICAL.DeleteRow(i);
                        bChk = true;
                    }
                }

                if (!bChk)
                {
                    MessageBox.Show("삭제대상을 선택하여 주십시오.", "작업불가");
                    return;
                }

                Data_Save();
            }
        }

        private void Spread_AddInsert(object sender, FpSpread Spd)
        {
            int nRow = -1;
            int nRowCnt = Spd.ActiveSheet.RowCount;

            List<HIC_CHUKDTL_CHEMICAL> lstDto = new List<HIC_CHUKDTL_CHEMICAL>();
            List<HIC_CHUKDTL_CHEMICAL> lstDto_New = new List<HIC_CHUKDTL_CHEMICAL>();

            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if (Spd.ActiveSheet.Cells[i, 0].Text == "Y")
                {
                    if (nRow < 0) { nRow = i; }

                    HIC_CHUKDTL_CHEMICAL code = Spd.GetRowData(i) as HIC_CHUKDTL_CHEMICAL;
                    lstDto.Add(code);
                }
            }

            if (nRow >= 0)
            {
                if (sender == btnIns)
                {
                    Spd.InsertRows(nRow + 1, lstDto.Count);
                }
                else
                {
                    Spd.AddRows(lstDto.Count);
                }

                HIC_CHUKDTL_CHEMICAL code = null;

                for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
                {
                    code = new HIC_CHUKDTL_CHEMICAL();

                    code.IsDelete = Spd.ActiveSheet.Cells[i, 0].Text == "Y" ? "Y" : "N";
                    code.WRTNO = Spd.ActiveSheet.Cells[i, 2].Text.To<long>(0);
                    code.SEQNO = Spd.ActiveSheet.Cells[i, 3].Text.To<long>(0);
                    code.PROCESS = Spd.ActiveSheet.Cells[i, 4].Text;
                    code.PROCESS_NM = Spd.ActiveSheet.Cells[i, 5].Text;
                    code.PRODUCT_NM = Spd.ActiveSheet.Cells[i, 7].Text;
                    code.GBUSE = Spd.ActiveSheet.Cells[i, 8].Text;
                    code.USAGE = Spd.ActiveSheet.Cells[i, 9].Text;
                    code.TREATMENT = Spd.ActiveSheet.Cells[i, 10].Value.To<long>();
                    code.UNIT = Spd.ActiveSheet.Cells[i, 11].Value.To<string>("");
                    code.REMARK = Spd.ActiveSheet.Cells[i, 12].Text;
                    code.RID = Spd.ActiveSheet.Cells[i, 13].Text;

                    lstDto_New.Add(code);
                }

                Spd.DataSource = null;
                Spd.SetDataSource(lstDto_New);

                if (sender == btnIns)
                {
                    for (int i = 0; i < lstDto.Count; i++)
                    {
                        lstDto[i].RID = "";
                        Spd.SetRowData(nRow + i + 1, lstDto[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < lstDto.Count; i++)
                    {
                        lstDto[i].RID = "";
                        Spd.SetRowData(nRowCnt + i, lstDto[i]);
                    }
                }
            }
            else
            {
                int nActRow = Spd.ActiveSheet.ActiveRowIndex + 1;

                if (sender == btnAdd)
                {
                    Spd.AddRows();
                }
                else if (sender == btnIns)
                {
                    Spd.InsertRows(nActRow);
                }
            }

            return;
        }
    

        private void Set_Spread_Seqno_Sorting(FpSpread spd, int nCol)
        {
            int nSeq = 1;
            int nColCnt = spd.ActiveSheet.ColumnCount - 1;

            for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
            {
                if ((RowStatus)((IList)spd.DataSource)[i].GetPropertieValue("RowStatus") != RowStatus.Delete)
                {
                    if (spd.ActiveSheet.Cells[i, 1].Text != "Y")
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

        private void Data_Save(string argMode = "")
        {
            string strMsg = ERROR_CHECK(ssCHEMICAL);

            if (!strMsg.IsNullOrEmpty())
            {
                MessageBox.Show(strMsg, "입력오류 점검");
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //SeqNo 정리
                Set_Spread_Seqno_Sorting(ssCHEMICAL, 3);

                //화학물질 사용실태
                IList<HIC_CHUKDTL_CHEMICAL> list3 = ssCHEMICAL.GetEditbleData<HIC_CHUKDTL_CHEMICAL>();

                if (list3.Count > 0)
                {
                    if (!hicChukDtlChemicalService.Save(list3, FnWRTNO))
                    {
                        MessageBox.Show("유해화학물질 등록중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Screen_Display(FnWRTNO);

                Spread_DelList_Display(chkDel_Gong.Checked, ssCHEMICAL);

                MessageBox.Show("저장완료. ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private string ERROR_CHECK(FpSpread Spd)
        {
            StringBuilder rtnVal = new StringBuilder();

            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if ((RowStatus)((IList)Spd.DataSource)[i].GetPropertieValue("RowStatus") != RowStatus.Delete)
                {
                    if (Spd.ActiveSheet.Cells[i, 4].Text.Trim() == "" || Spd.ActiveSheet.Cells[i, 5].Text.Trim() == "")
                    {
                        rtnVal.Append((i + 1) + " 번째 줄 공정 누락.");
                        rtnVal.Append(ComNum.VBLF);
                    }

                    if (Spd.ActiveSheet.Cells[i, 7].Text.Trim() == "")
                    {
                        rtnVal.Append((i + 1) + " 번째 줄 화학물질명 누락.");
                        rtnVal.Append(ComNum.VBLF);
                    }
                }
            }

            return rtnVal.ToString();
        }
    }
}
