using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// 유해인자(물질) 코드관리
/// </summary>
namespace HC_Measurement
{
    public partial class frmHcChkMain :CommonForm
    {
        long FnWRTNO = 0;
        clsSpread cSpd = null;
        HicChukMstNewService hicChukMstNewService = null;
        frmHcChkCard01 frmCard01 = null;
        frmHcChkCard02 frmCard02 = null;
        frmHcChkCard03 frmCard03 = null;
        frmHcChkCard04 frmCard04 = null;
        frmHcChkCard05 frmCard05 = null;
        frmHcChkCard06 frmCard06 = null;
        frmHcChkCard07 frmCard07 = null;
        frmHcChkCard08 frmCard08 = null;
        frmHcChkCard09 frmCard09 = null;
        frmHcChkCard10 frmCard10 = null;
        frmHcChkCard11 frmCard11 = null;

        public frmHcChkMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicChukMstNewService = new HicChukMstNewService();

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30, RowHeaderVisible = true });

            //SSList.AddColumnCheckBox("선택",   nameof(HIC_CHUKMST_NEW.IsDelete),    44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSSList_ChkButtonClick;
            
            SSList.AddColumn("일련번호",       nameof(HIC_CHUKMST_NEW.WRTNO),       78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("완료여부",       nameof(HIC_CHUKMST_NEW.GBSTS),       78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("상태값",         nameof(HIC_CHUKMST_NEW.GBSTS_NM),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSList.AddColumn("작업년도",       nameof(HIC_CHUKMST_NEW.CHKYEAR),     78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("반기",           nameof(HIC_CHUKMST_NEW.BANGI),       74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("사업장코드",     nameof(HIC_CHUKMST_NEW.LTDCODE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("사업장명",       nameof(HIC_CHUKMST_NEW.LTDNAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("측정일자 Fr",    nameof(HIC_CHUKMST_NEW.SDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("측정일자 To",    nameof(HIC_CHUKMST_NEW.EDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("작성일자",       nameof(HIC_CHUKMST_NEW.ENTDATE),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("작성자사번",     nameof(HIC_CHUKMST_NEW.ENTSABUN),    92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });                       //10
            SSList.AddColumn("작성자",         nameof(HIC_CHUKMST_NEW.ENTNAME),     92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("지정한계치",     nameof(HIC_CHUKMST_NEW.T_LIMIT),     55, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("국고누적",       nameof(HIC_CHUKMST_NEW.T5_LIMIT),   55, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("총누적",         nameof(HIC_CHUKMST_NEW.TO_ACCUM),    55, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("5인누적",        nameof(HIC_CHUKMST_NEW.T5_ACCUM),    55, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("담당자",         nameof(HIC_CHUKMST_NEW.LTD_MANAGER), 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("직위",           nameof(HIC_CHUKMST_NEW.LTD_GRADE),   84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("휴대전화",       nameof(HIC_CHUKMST_NEW.LTD_HPHONE),  82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("E-MAIL",         nameof(HIC_CHUKMST_NEW.LTD_EMAIL),  140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("국고지원여부",   nameof(HIC_CHUKMST_NEW.GBSUPPORT),   48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });                                           //20
            SSList.AddColumn("진행상황",       nameof(HIC_CHUKMST_NEW.GBSTS),       48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("진행상황",       nameof(HIC_CHUKMST_NEW.GBSTSNAME),   48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("삭제일자",       nameof(HIC_CHUKMST_NEW.DELDATE),    140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("삭제사번",       nameof(HIC_CHUKMST_NEW.DELSABUN),    48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("삭제자",         nameof(HIC_CHUKMST_NEW.DELNAME),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("ROWID",          nameof(HIC_CHUKMST_NEW.RID),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
        }

        private void eSSList_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUKMST_NEW code = SSList.GetRowData(e.Row) as HIC_CHUKMST_NEW;

            code.RowStatus = ComBase.Mvc.RowStatus.Update;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            //this.tabControl1.SelectedIndexChanged += new EventHandler(eTabIndexChanged);
        }

        private void eTabIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    frmCard01.Dispose();
                    frmCard01 = new frmHcChkCard01(FnWRTNO);
                    AddForm(frmCard01, tabPage1);
                    break;
                case 1:
                    frmCard11.Dispose();
                    frmCard11 = new frmHcChkCard11(FnWRTNO);
                    AddForm(frmCard11, tabPage13);
                    break;
                case 2:
                    frmCard02.Dispose();
                    frmCard02 = new frmHcChkCard02(FnWRTNO);
                    AddForm(frmCard02, tabPage2);
                    break;
                case 3:
                    frmCard03.Dispose();
                    frmCard03 = new frmHcChkCard03(FnWRTNO);
                    AddForm(frmCard03, tabPage3);
                    break;
                case 4:
                    frmCard04.Dispose();
                    frmCard04 = new frmHcChkCard04(FnWRTNO);
                    AddForm(frmCard04, tabPage4);
                    break;
                case 5:
                    frmCard05.Dispose();
                    frmCard05 = new frmHcChkCard05(FnWRTNO);
                    AddForm(frmCard05, tabPage5);
                    break;
                case 6:
                    frmCard06.Dispose();
                    frmCard06 = new frmHcChkCard06(FnWRTNO);
                    AddForm(frmCard06, tabPage6);
                    break;
                case 7:
                    frmCard07.Dispose();
                    frmCard07 = new frmHcChkCard07(FnWRTNO);
                    AddForm(frmCard07, tabPage7);
                    break;
                case 8:
                    frmCard08.Dispose();
                    frmCard08 = new frmHcChkCard08(FnWRTNO);
                    AddForm(frmCard08, tabPage8);
                    break;
                case 9:
                    frmCard09.Dispose();
                    frmCard09 = new frmHcChkCard09(FnWRTNO);
                    AddForm(frmCard09, tabPage9);
                    break;
                default:
                    break;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            FnWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>(0);

            tabControl1.SelectedTab = tabPage1;

            frmCard01 = new frmHcChkCard01(FnWRTNO);
            AddForm(frmCard01, tabPage1);

            frmCard02 = new frmHcChkCard02(FnWRTNO);
            AddForm(frmCard02, tabPage2);

            frmCard03 = new frmHcChkCard03(FnWRTNO);
            AddForm(frmCard03, tabPage3);

            frmCard04 = new frmHcChkCard04(FnWRTNO);
            AddForm(frmCard04, tabPage4);

            frmCard05 = new frmHcChkCard05(FnWRTNO);
            AddForm(frmCard05, tabPage5);

            frmCard06 = new frmHcChkCard06(FnWRTNO);
            AddForm(frmCard06, tabPage6);

            frmCard07 = new frmHcChkCard07(FnWRTNO);
            AddForm(frmCard07, tabPage7);

            frmCard08 = new frmHcChkCard08(FnWRTNO);
            AddForm(frmCard08, tabPage8);

            frmCard09 = new frmHcChkCard09(FnWRTNO);
            AddForm(frmCard09, tabPage9);

            frmCard10 = new frmHcChkCard10(FnWRTNO);
            AddForm(frmCard10, tabPage10);

            frmCard11 = new frmHcChkCard11(FnWRTNO);
            AddForm(frmCard11, tabPage13);
            //Screen_Display();
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
                Screen_Display();
            }
        }

        private void Screen_Display()
        {
            string strKeyWard = txtKeyWord.Text.Trim();
            string strBangi = rdoBangi1.Checked ? "1" : rdoBangi2.Checked ? "2" : "";
            string strGjYear = nmrGjYear.Value.To<string>("0");

            List<HIC_CHUKMST_NEW> list = hicChukMstNewService.GetItemAll(strKeyWard, false, strBangi, strGjYear);

            SSList.DataSource = list;

            lblTotNuCnt.Text = hicChukMstNewService.GetTotAccumByBangiYear(strBangi, strGjYear, true).To<string>("");       //총누적
            lblFiveNuCnt.Text = hicChukMstNewService.GetT5AccumByBangiYear(strBangi, strGjYear, true).To<string>("");       //5인이상 누적
            lblT5Limit.Text = hicChukMstNewService.GetT5LimitByBangiYear(strBangi, strGjYear, true).To<string>("");       //국고누적
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(SSList);

            if (DateTime.Now.Month > 6) { rdoBangi2.Checked = true; }

            Screen_Display();
        }

    }
}
