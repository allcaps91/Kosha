using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Service;
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
    public partial class frmHcChkCard02 :CommonForm
    {
        long FnWRTNO = 0;
        string FstrComCode = "";
        string  FstrComName = "";
        string  FstrComGCode = "";
        string  FstrComGCode1 = "";

        clsSpread cSpd = null;

        HicChukMstNewService hicChukMstNewService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HcCodeService hcCodeService = null;

        public frmHcChkCard02()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcChkCard02(long nWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWRTNO;
        }

        private void SetControl()
        {
            cSpd = new clsSpread();

            hicChukMstNewService = new HicChukMstNewService();
            hicChukDtlPlanService = new HicChukDtlPlanService();
            hcCodeService = new HcCodeService();

            #region 측정사업장 공정 및 유해인자 측정계획 내역
            SpreadComboBoxData scbChkWay = hcCodeService.GetSpreadComboBoxData("WEM_MTH_CD", "WEM");
            SpreadComboBoxData scbChkJugi = hcCodeService.GetSpreadComboBoxData("OCCRRNC_CYCLE_CD", "WEM");

            ssGONG.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssGONG.AddColumnCheckBox("선택", nameof(HIC_CHUKDTL_PLAN.IsActive), 36, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = true });
            ssGONG.AddColumn("삭제", nameof(HIC_CHUKDTL_PLAN.IsDelete), 44, new SpreadCellTypeOption { IsVisivle = false });                                                              //1
            ssGONG.AddColumn("일련번호", nameof(HIC_CHUKDTL_PLAN.WRTNO), 74, new SpreadCellTypeOption { IsVisivle = false });
            ssGONG.AddColumnNumber("순번", nameof(HIC_CHUKDTL_PLAN.SEQNO), 40, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssGONG.AddColumn("공정코드", nameof(HIC_CHUKDTL_PLAN.PROCESS), 62, new SpreadCellTypeOption { IsEditble = false });
            ssGONG.AddColumn("공정명", nameof(HIC_CHUKDTL_PLAN.PROCESS_NM), 100, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });           // 5
            ssGONG.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssGONG.AddColumn("유해인자코드", nameof(HIC_CHUKDTL_PLAN.MCODE), 62, new SpreadCellTypeOption { IsEditble = false });
            ssGONG.AddColumn("유해인자명", nameof(HIC_CHUKDTL_PLAN.MCODE_NM), 130, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });          //8
            ssGONG.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssCHMCLS_HELP;
            ssGONG.AddColumnComboBox("유해인자 발생주기", nameof(HIC_CHUKDTL_PLAN.JUGI), 62, IsReadOnly.N, scbChkJugi, new SpreadCellTypeOption { });                             //10
            ssGONG.AddColumn("근로자수", nameof(HIC_CHUKDTL_PLAN.INWON), 44, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("작업시간", nameof(HIC_CHUKDTL_PLAN.JTIME), 44, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("폭로시간", nameof(HIC_CHUKDTL_PLAN.PTIME), 44, new SpreadCellTypeOption { });
            ssGONG.AddColumnComboBox("측정방법", nameof(HIC_CHUKDTL_PLAN.CHKWAY), 64, IsReadOnly.N, scbChkWay, new SpreadCellTypeOption { IsSort = false });
            ssGONG.AddColumn("채취방법코드", nameof(HIC_CHUKDTL_PLAN.CHKWAY_CD), 44, new SpreadCellTypeOption { IsVisivle = false });                                             //15
            ssGONG.AddColumn("채취방법", nameof(HIC_CHUKDTL_PLAN.CHKWAY_NM), 78, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGONG.AddColumn("분석방법", nameof(HIC_CHUKDTL_PLAN.ANALWAY_NM), 84, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGONG.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssGONG.AddColumnNumber("측정건수", nameof(HIC_CHUKDTL_PLAN.CHKCOUNT), 44, new SpreadCellTypeOption { TextMaxLength = 3 });
            ssGONG.AddColumn("ROWID", nameof(HIC_CHUKDTL_PLAN.RID), 74, new SpreadCellTypeOption { IsVisivle = false });                                 //20
            #endregion
        }

        private void ssCHMCLS_HELP(object sender, EditorNotifyEventArgs e)
        {
            string strGubun = "";
            string strKeyWord = "";

            FstrComCode = "";
            FstrComName = "";
            FstrComGCode = "";
            FstrComGCode1 = "";

            if (e.Column == 9)
            {
                strGubun = "MCODE";
                strKeyWord = ssGONG.ActiveSheet.Cells[e.Row, 8].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetMCodeValue += new frmHcChkCodeHelp.SetMCodeValue(eValueCode_Chmcls);
            frm.ShowDialog();
            frm.rSetMCodeValue -= new frmHcChkCodeHelp.SetMCodeValue(eValueCode_Chmcls);

            if (!FstrComCode.IsNullOrEmpty())
            {
                ssGONG.ActiveSheet.Cells[e.Row, 7].Text = FstrComCode.Trim();
                ssGONG.ActiveSheet.Cells[e.Row, 8].Text = FstrComName.Trim();
            }
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

            if (e.Column == 6)
            {
                strGubun = "GONG";
                strKeyWord = ssGONG.ActiveSheet.Cells[e.Row, 5].Text.Trim();
            }
            else if (e.Column == 9)
            {
                strGubun = "MCODE";
                strKeyWord = ssGONG.ActiveSheet.Cells[e.Row, 8].Text.Trim();
            }
            else if (e.Column == 18)
            {
                strGubun = "ANAL";
                strKeyWord = ssGONG.ActiveSheet.Cells[e.Row, 15].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

            if (!FstrComCode.IsNullOrEmpty())
            {
                if (strGubun == "GONG")
                {
                    ssGONG.ActiveSheet.Cells[e.Row, 4].Text = FstrComCode.Trim();
                    ssGONG.ActiveSheet.Cells[e.Row, 5].Text = FstrComName.Trim();
                }
                else if (strGubun == "MCODE")
                {
                    ssGONG.ActiveSheet.Cells[e.Row, 7].Text = FstrComCode.Trim();
                    ssGONG.ActiveSheet.Cells[e.Row, 8].Text = FstrComName.Trim();
                }
                else if (strGubun == "ANAL")
                {
                    ssGONG.ActiveSheet.Cells[e.Row, 15].Text = FstrComCode.Trim();
                    ssGONG.ActiveSheet.Cells[e.Row, 16].Text = FstrComGCode.Trim();
                    ssGONG.ActiveSheet.Cells[e.Row, 17].Text = FstrComGCode1.Trim();
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

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnAdd1_Ins.Click += new EventHandler(eBtnClick);
            this.btnAdd1_Add.Click += new EventHandler(eBtnClick);
            this.btnSave_Gong.Click += new EventHandler(eBtnClick);
            this.btnDel_Gong.Click += new EventHandler(eBtnClick);
            
            this.chkDel_Gong.CheckedChanged += new EventHandler(eChkChanged);
        }

        private void eChkChanged(object sender, EventArgs e)
        {
            if (sender == chkDel_Gong)
            {
                //유해인자별 측정계획
                List<HIC_CHUKDTL_PLAN> lstHCU = hicChukDtlPlanService.GetListByWrtno(FnWRTNO, chkDel_Gong.Checked);
                ssGONG.DataSource = null;
                ssGONG.SetDataSource(lstHCU);

                Spread_DelList_Display(chkDel_Gong.Checked, ssGONG);
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

        private void eFormLoad(object sender, EventArgs e)
        {
            ssGONG.DataSource = new List<HIC_CHUKDTL_PLAN>();
            ssGONG.AddRows(1);
         
            if (FnWRTNO > 0)
            {
                Screen_Display(FnWRTNO);
            }
        }

        private void Screen_Display(long argWRTNO)
        {
            if (argWRTNO == 0) { return; }

            ssGONG.DataSource = null;
            ssGONG.ActiveSheet.RowCount = 0;

            //계약내용이 있다면 출력
            HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(argWRTNO);

            if (!item.IsNullOrEmpty())
            {
                //유해인자별 측정계획
                List<HIC_CHUKDTL_PLAN> lstHCU = hicChukDtlPlanService.GetListByWrtno(argWRTNO);
                ssGONG.DataSource = lstHCU;
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnDel_Gong)
            {
                bool bChk = false;

                if (MessageBox.Show("선택항목을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < ssGONG.ActiveSheet.RowCount; i++)
                {
                    if (ssGONG.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        HIC_CHUKDTL_PLAN code = ssGONG.GetRowData(i) as HIC_CHUKDTL_PLAN;

                        ssGONG.DeleteRow(i);
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
            else if (sender == btnSave_Gong)
            {
                Data_Save();
            }
            else if (sender == btnAdd1_Ins || sender == btnAdd1_Add)
            {
                int nRow = -1;
                int nRowCnt = ssGONG.ActiveSheet.RowCount;

                List<HIC_CHUKDTL_PLAN> lstDto = new List<HIC_CHUKDTL_PLAN>();
                List<HIC_CHUKDTL_PLAN> lstDto_New = new List<HIC_CHUKDTL_PLAN>();

                for (int i = 0; i < ssGONG.ActiveSheet.RowCount; i++)
                {
                    if (ssGONG.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        if (nRow < 0) { nRow = i; }

                        HIC_CHUKDTL_PLAN code = ssGONG.GetRowData(i) as HIC_CHUKDTL_PLAN;
                        lstDto.Add(code);
                    }
                }

                if (nRow >= 0)
                {
                    if (sender == btnAdd1_Ins)
                    {
                        ssGONG.InsertRows(nRow + 1, lstDto.Count);
                    }
                    else
                    {
                        ssGONG.AddRows(lstDto.Count);
                    }

                    HIC_CHUKDTL_PLAN code = null;

                    for (int i = 0; i < ssGONG.ActiveSheet.RowCount; i++)
                    {
                        code = new HIC_CHUKDTL_PLAN();

                        code.IsActive = ssGONG.ActiveSheet.Cells[i, 0].Text == "Y" ? "Y" : "N";
                        code.IsDelete = ssGONG.ActiveSheet.Cells[i, 1].Text == "Y" ? "Y" : "N";
                        code.WRTNO = ssGONG.ActiveSheet.Cells[i, 2].Text.To<long>(0);
                        code.SEQNO = ssGONG.ActiveSheet.Cells[i, 3].Text.To<long>(0);
                        code.PROCESS = ssGONG.ActiveSheet.Cells[i, 4].Text;
                        code.PROCESS_NM = ssGONG.ActiveSheet.Cells[i, 5].Text;
                        code.MCODE = ssGONG.ActiveSheet.Cells[i, 7].Text;
                        code.MCODE_NM = ssGONG.ActiveSheet.Cells[i, 8].Text;
                        code.JUGI = ssGONG.ActiveSheet.Cells[i, 10].Value.To<string>("");
                        code.INWON = ssGONG.ActiveSheet.Cells[i, 11].Text;
                        code.JTIME = ssGONG.ActiveSheet.Cells[i, 12].Text.To<long>(0);
                        code.PTIME = ssGONG.ActiveSheet.Cells[i, 13].Text.To<long>(0);
                        code.CHKWAY = ssGONG.ActiveSheet.Cells[i, 14].Value.To<string>("");
                        code.CHKWAY_CD = ssGONG.ActiveSheet.Cells[i, 15].Text;
                        code.CHKWAY_NM = ssGONG.ActiveSheet.Cells[i, 16].Text;
                        code.ANALWAY_NM = ssGONG.ActiveSheet.Cells[i, 17].Text;
                        code.CHKCOUNT = ssGONG.ActiveSheet.Cells[i, 19].Text.To<long>(0);
                        code.RID = ssGONG.ActiveSheet.Cells[i, 20].Text;

                        lstDto_New.Add(code);
                    }

                    ssGONG.DataSource = null;
                    ssGONG.SetDataSource(lstDto_New);

                    Spread_DelList_Display(chkDel_Gong.Checked, ssGONG);

                    if (sender == btnAdd1_Ins)
                    {
                        for (int i = 0; i < lstDto.Count; i++)
                        {
                            lstDto[i].RID = "";
                            ssGONG.SetRowData(nRow + i + 1, lstDto[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < lstDto.Count; i++)
                        {
                            lstDto[i].RID = "";
                            ssGONG.SetRowData(nRowCnt + i, lstDto[i]);
                        }
                    }
                }
                else
                {
                    int nActRow = ssGONG.ActiveSheet.ActiveRowIndex + 1;

                    if (sender == btnAdd1_Add)
                    {
                        ssGONG.AddRows();
                    }
                    else if (sender == btnAdd1_Ins)
                    {
                        ssGONG.InsertRows(nActRow);
                    }
                }

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
                    if (ssGONG.ActiveSheet.Cells[i, 1].Text != "Y")
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
            string strMsg = ERROR_CHECK(ssGONG);

            if (!strMsg.IsNullOrEmpty())
            {
                MessageBox.Show(strMsg, "입력오류 점검");
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //SeqNo 정리
                Set_Spread_Seqno_Sorting(ssGONG, 3);

                //측정대상 공정 및 유해인자별 측정계획
                IList<HIC_CHUKDTL_PLAN> list2 = ssGONG.GetEditbleData<HIC_CHUKDTL_PLAN>();

                if (list2.Count > 0)
                {
                    if (!hicChukDtlPlanService.Save(list2, FnWRTNO))
                    {
                        MessageBox.Show("측정대상 공정별 유해인자 등록중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Screen_Display(FnWRTNO);

                Spread_DelList_Display(chkDel_Gong.Checked, ssGONG);

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

                    if (Spd.ActiveSheet.Cells[i, 7].Text.Trim() == "" || Spd.ActiveSheet.Cells[i, 8].Text.Trim() == "")
                    {
                        rtnVal.Append((i + 1) + " 번째 줄 유해물질 누락.");
                        rtnVal.Append(ComNum.VBLF);
                    }
                }
            }

            return rtnVal.ToString();
        }
    }
}
