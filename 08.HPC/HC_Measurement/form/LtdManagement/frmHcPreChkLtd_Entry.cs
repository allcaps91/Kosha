using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Service;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcPreChkLtd_Entry :CommonForm
    {
        long FnWRTNO = 0;
        bool FbLtdSeq_New = false;
        string FstrRowid = "";
        string FstrComCode = "";
        string  FstrComName = "";
        string  FstrComGCode = "";
        string  FstrComGCode1 = "";

        HIC_LTD     LtdHelpItem     = null;
        clsSpread cSpd = null;

        HcUserService hcUserService = null;
        HicLtdService hicLtdService = null;
        HicChkMcodeService hicChkMcodeService = null;
        HicChukMstNewService hicChukMstNewService = null;
        HicChukWorkerService hicChukWorkerService = null;
        HicChukDtlPlanService hicChukDtlPlanService = null;
        HicChukDtlChemicalService hicChukDtlChemicalService = null;
        HicChukDtlSubltdService hicChukDtlSubltdService = null;
        HcCodeService hcCodesService = null;
        ComHpcLibBService comHpcLibBService = null;
        HcCodeService hcCodeService = null;

        public frmHcPreChkLtd_Entry()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPreChkLtd_Entry(long argWRTNO)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = argWRTNO;
        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
            cSpd = new clsSpread();

            hcUserService = new HcUserService();
            hicLtdService = new HicLtdService();
            hicChkMcodeService = new HicChkMcodeService();
            hicChukMstNewService = new HicChukMstNewService();
            hicChukWorkerService = new HicChukWorkerService();
            hicChukDtlPlanService = new HicChukDtlPlanService();
            hicChukDtlChemicalService = new HicChukDtlChemicalService();
            hicChukDtlSubltdService = new HicChukDtlSubltdService();
            hcCodesService = new HcCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hcCodeService = new HcCodeService();

            txtLtdCode.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.LTDCODE), Min = 0 });
            nmrGjYear.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.CHKYEAR), Min = 0 });
            nmrLtdSeqNo.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.LTDSEQNO), Min = 0, Max = 999 });
            nmrTLimit.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T_LIMIT), Min = 0, Max = 999 });
            nmrToAccum.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.TO_ACCUM), Min = 0, Max = 999 });
            nmrT5Accum.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T5_ACCUM), Min = 0, Max = 999 });
            nmrT5Limit.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.T5_LIMIT), Min = 0, Max = 999 });

            chkGukgo.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSUPPORT), CheckValue = "1", UnCheckValue = "0" });
            chkGbTemp.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBTEMP), CheckValue = "Y", UnCheckValue = "N" });
            chkEstimate.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBEST), CheckValue = "Y", UnCheckValue = "N" });

            //신규구분
            rdoNew1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBNEW), CheckValue = "2" });
            rdoNew2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBNEW), CheckValue = "1" });
            //반기
            rdoBangi1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.BANGI), CheckValue = "1" });
            rdoBangi2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.BANGI), CheckValue = "2" });
            //조사방법
            rdoWay1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBWAY), CheckValue = "1" });
            rdoWay2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBWAY), CheckValue = "2" });
            //예비조사일
            dtpBDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.BDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD } );
            //측정예상일자 Fr - To
            dtpChkSDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.SDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            dtpChkEDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.EDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });

            //진행상태
            List<HC_CODE> lstGBSTS = hcCodesService.FindActiveCodeByGroupCode("GBSTS", "WEM");
            cboGbSTS.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSTS) });
            cboGbSTS.SetItems(lstGBSTS, "CODENAME", "CODE");

            //지정한계 및 측정실적
            rdoProcChg1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_YN), CheckValue = "0" });
            rdoProcChg2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_YN), CheckValue = "1" });
            dtpProcChgDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_NEW_CHANGE_DATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });
            rdoWemRes1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "0" });
            rdoWemRes2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "1" });
            rdoWemRes3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "2" });
            rdoWemRes4.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_PROCS_WEM_RESULT), CheckValue = "3" });
            rdoCrngnYN1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CRNGN_RDMTR_OVER_YN), CheckValue = "0" });
            rdoCrngnYN2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CRNGN_RDMTR_OVER_YN), CheckValue = "1" });
            rdoChmclsYN1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CHMCLS_RDMTR_OVER_YN), CheckValue = "0" });
            rdoChmclsYN2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_CHMCLS_RDMTR_OVER_YN), CheckValue = "1" });
            rdoFutrWemCycle1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "0" });
            rdoFutrWemCycle2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "1" });
            rdoFutrWemCycle3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_CYCLE), CheckValue = "2" });
            dtpWemPlanDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_CHUKMST_NEW.CYCLE_FUTR_WEM_PLAN_DATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.YYYY_MM_DD });

            #region 측정자, 분석자 등록내역
            SS1.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 30 });
            SS1.AddColumnCheckBox("삭제", nameof(HIC_CHUK_WORKER.IsDelete),     44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSS1_ChkButtonClick;
            SS1.AddColumn("일련번호",   nameof(HIC_CHUK_WORKER.WRTNO),          74, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("작업자사번", nameof(HIC_CHUK_WORKER.WORKER_SABUN),   74, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("작업자명",   nameof(HIC_CHUK_WORKER.WORKER_NAME),    84, new SpreadCellTypeOption { });
            SS1.AddColumn("역할",       nameof(HIC_CHUK_WORKER.ROLE),          120, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("자격번호",   nameof(HIC_CHUK_WORKER.CERTNO),        220, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("비고",       nameof(HIC_CHUK_WORKER.BIGO),          180, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID",      nameof(HIC_CHUK_WORKER.RID),            74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 측정사업장 공정 및 유해인자 측정계획 내역
            SpreadComboBoxData scbChkWay = hcCodeService.GetSpreadComboBoxData("WEM_MTH_CD", "WEM");
            SpreadComboBoxData scbChkJugi = hcCodeService.GetSpreadComboBoxData("OCCRRNC_CYCLE_CD", "WEM");
            
            ssGONG.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssGONG.AddColumnCheckBox("선택", nameof(HIC_CHUKDTL_PLAN.IsActive), 36, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = true });
            ssGONG.AddColumn("삭제", nameof(HIC_CHUKDTL_PLAN.IsDelete), 44, new SpreadCellTypeOption { IsVisivle = false });                                                              //1
            ssGONG.AddColumn("일련번호",            nameof(HIC_CHUKDTL_PLAN.WRTNO),          74, new SpreadCellTypeOption { IsVisivle = false });
            ssGONG.AddColumnNumber("순번",          nameof(HIC_CHUKDTL_PLAN.SEQNO),          40, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssGONG.AddColumn("공정코드", nameof(HIC_CHUKDTL_PLAN.PROCESS), 62, new SpreadCellTypeOption { });
            ssGONG.AddColumn("공정명", nameof(HIC_CHUKDTL_PLAN.PROCESS_NM), 100, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });           // 5
            ssGONG.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssGONG.AddColumn("유해인자코드", nameof(HIC_CHUKDTL_PLAN.MCODE), 62, new SpreadCellTypeOption { });
            ssGONG.AddColumn("유해인자명", nameof(HIC_CHUKDTL_PLAN.MCODE_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });          //8
            ssGONG.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssCHMCLS_HELP;
            ssGONG.AddColumnComboBox("유해인자 발생주기",   nameof(HIC_CHUKDTL_PLAN.JUGI), 62, IsReadOnly.N, scbChkJugi, new SpreadCellTypeOption { });                             //10
            ssGONG.AddColumn("근로자수", nameof(HIC_CHUKDTL_PLAN.INWON),       44, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("작업시간",      nameof(HIC_CHUKDTL_PLAN.JTIME),       44, new SpreadCellTypeOption { });
            ssGONG.AddColumnNumber("폭로시간",      nameof(HIC_CHUKDTL_PLAN.PTIME),       44, new SpreadCellTypeOption { });
            ssGONG.AddColumnComboBox("측정방법", nameof(HIC_CHUKDTL_PLAN.CHKWAY), 64, IsReadOnly.N, scbChkWay, new SpreadCellTypeOption { IsSort = false });                        
            ssGONG.AddColumn("채취방법코드", nameof(HIC_CHUKDTL_PLAN.CHKWAY_CD), 44, new SpreadCellTypeOption { IsVisivle = false });                                             //15
            ssGONG.AddColumn("채취방법", nameof(HIC_CHUKDTL_PLAN.CHKWAY_NM), 78, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGONG.AddColumn("분석방법", nameof(HIC_CHUKDTL_PLAN.ANALWAY_NM), 84, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGONG.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssGONG.AddColumnNumber("측정건수", nameof(HIC_CHUKDTL_PLAN.CHKCOUNT),      44, new SpreadCellTypeOption { TextMaxLength = 3 });                      
            ssGONG.AddColumn("ROWID",             nameof(HIC_CHUKDTL_PLAN.RID),        74, new SpreadCellTypeOption { IsVisivle = false });                                 //20
            #endregion

            #region 측정사업장 공정별 유해화학물질 사용 실태
            SpreadComboBoxData TREATMENT_UNIT = hiccodeService.GetSpreadComboBoxData("C2");

            ssCHEMICAL.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssCHEMICAL.AddColumnCheckBox("삭제", nameof(HIC_CHUKDTL_CHEMICAL.IsDelete), 44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSS_Delete; ;
            ssCHEMICAL.AddColumn("일련번호",                    nameof(HIC_CHUKDTL_CHEMICAL.WRTNO),       74, new SpreadCellTypeOption { IsVisivle = false });
            ssCHEMICAL.AddColumnNumber("순번",                  nameof(HIC_CHUKDTL_CHEMICAL.SEQNO),       48, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssCHEMICAL.AddColumn("공정코드",    nameof(HIC_CHUKDTL_CHEMICAL.PROCESS),     62, new SpreadCellTypeOption { IsVisivle = false });
            ssCHEMICAL.AddColumn("공단코드",    nameof(HIC_CHUKDTL_CHEMICAL.PROCESS_K2BCD), 62, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("공정명",      nameof(HIC_CHUKDTL_CHEMICAL.PROCESS_NM), 130, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssCHEMICAL.AddColumnButton("H", 24, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;
            ssCHEMICAL.AddColumn("화학물질명(상품명)",          nameof(HIC_CHUKDTL_CHEMICAL.PRODUCT_NM), 150, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("제조 또는 사용 여부",         nameof(HIC_CHUKDTL_CHEMICAL.GBUSE),       72, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("사용 용도",                   nameof(HIC_CHUKDTL_CHEMICAL.USAGE),      130, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnNumber("월 취급량",             nameof(HIC_CHUKDTL_CHEMICAL.TREATMENT),   48, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumnComboBox("Kg/톤",               nameof(HIC_CHUKDTL_CHEMICAL.UNIT),        52, IsReadOnly.N, TREATMENT_UNIT, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("비고",                        nameof(HIC_CHUKDTL_CHEMICAL.REMARK),      84, new SpreadCellTypeOption { });
            ssCHEMICAL.AddColumn("ROWID",                       nameof(HIC_CHUKDTL_CHEMICAL.RID),         74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 측정사업장 협력업체 List
            ssSUBLTD.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 38 });
            ssSUBLTD.AddColumnCheckBox("삭제", nameof(HIC_CHUKDTL_SUBLTD.IsDelete), 44, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSSSUBLTD_ChkButtonClick;
            ssSUBLTD.AddColumn("일련번호",                   nameof(HIC_CHUKDTL_SUBLTD.WRTNO),        74, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("측정사업장",           nameof(HIC_CHUKDTL_SUBLTD.LTDCODE),      48, new SpreadCellTypeOption { IsVisivle = false });
            ssSUBLTD.AddColumnNumber("협력업체사업장코드",   nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDCODE),  88, new SpreadCellTypeOption { });
            ssSUBLTD.AddColumn("협력업체명",                 nameof(HIC_CHUKDTL_SUBLTD.SUB_LTDNAME), 220, new SpreadCellTypeOption { IsEditble = false });
            ssSUBLTD.AddColumn("비고",                       nameof(HIC_CHUKDTL_SUBLTD.REMARK),      220, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, TextMaxLength = 25 });
            ssSUBLTD.AddColumn("ROWID",                      nameof(HIC_CHUKDTL_SUBLTD.RID),          74, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region Control SetOption
            nmrILSU.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.ILSU), Min = 0 });
            nmrInwon.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON), Min = 0 });
            nmrInwonS.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON_S), Min = 0 });
            nmrInwonH.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.INWON_H), Min = 0 });
            nmrDaytime.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.DAYTIME), Min = 0 });
            nmrShiftGrpCnt.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTGRPCNT), Min = 0 });
            nmrShiftQtr.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTQUARTER), Min = 0 });
            nmrShiftTime.SetOptions(new NumericUpDownOption { DataField = nameof(HIC_CHUKMST_NEW.SHIFTTIME), Min = 0 });
            chkGbDay.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBDAY), CheckValue = "1", UnCheckValue = "0" });
            chkGbShift.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBSHIFT), CheckValue = "1", UnCheckValue = "0" });
            rdoOverTime1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBOVERTIME), CheckValue = "Y" });
            rdoOverTime2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBOVERTIME), CheckValue = "N" });
            rdoEstimate1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBESTIMATE), CheckValue = "Y" });
            rdoEstimate2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBESTIMATE), CheckValue = "N" });
            rdoCorrect1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCORRECT), CheckValue = "Y" });
            rdoCorrect2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCORRECT), CheckValue = "N" });
            rdoSample1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBSAMPLE), CheckValue = "Y" });
            rdoSample2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBSAMPLE), CheckValue = "N" });
            rdoChromium1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCHROMIUM), CheckValue = "Y" });
            rdoChromium2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_CHUKMST_NEW.GBCHROMIUM), CheckValue = "N" });

            chkUcode1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE1), CheckValue = "1", UnCheckValue = "0" });
            chkUcode2.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE2), CheckValue = "1", UnCheckValue = "0" });
            chkUcode3.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE3), CheckValue = "1", UnCheckValue = "0" });
            chkUcode4.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE4), CheckValue = "1", UnCheckValue = "0" });
            chkUcode5.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE5), CheckValue = "1", UnCheckValue = "0" });
            chkUcode6.SetOptions(new CheckBoxOption { DataField = nameof(HIC_CHUKMST_NEW.GBUCODE6), CheckValue = "1", UnCheckValue = "0" });
            #endregion

            panMain.SetEnterKey();
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

            if (sender == ssGONG)
            {
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
            }
            else if (sender == ssCHEMICAL)
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
                if (sender == ssGONG)
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
                else if (sender == ssCHEMICAL)
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

        private void eSSSUBLTD_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUKDTL_SUBLTD code = ssSUBLTD.GetRowData(e.Row) as HIC_CHUKDTL_SUBLTD;

            ssSUBLTD.DeleteRow(e.Row);
        }

        private void eSS_Delete(object sender, EditorNotifyEventArgs e)
        {
            if (sender == ssCHEMICAL)
            {
                HIC_CHUKDTL_CHEMICAL code = ssCHEMICAL.GetRowData(e.Row) as HIC_CHUKDTL_CHEMICAL;

                ssCHEMICAL.DeleteRow(e.Row);
            }
            
        }

        private void eSS1_ChkButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_CHUK_WORKER code = SS1.GetRowData(e.Row) as HIC_CHUK_WORKER;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            
            this.btnAdd1_Ins.Click      += new EventHandler(eBtnClick);
            this.btnAdd1_Add.Click      += new EventHandler(eBtnClick);
            this.btnAdd2.Click          += new EventHandler(eBtnClick);
            this.btnAdd3.Click          += new EventHandler(eBtnClick);
            
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnSave_Gong.Click     += new EventHandler(eBtnClick);
            this.btnSave_Chemical.Click += new EventHandler(eBtnClick);
            this.btnSave_SubLtd.Click   += new EventHandler(eBtnClick);
            
            this.btnDelete.Click        += new EventHandler(eBtnClick);
            this.btnDel_Gong.Click      += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.btnPrint.Click         += new EventHandler(eBtnClick);
            this.SS1.EditModeOff        += new EventHandler(eSpdEditModeOff);

            this.ssSUBLTD.EditModeOff   += new EventHandler(eSpdEditModeOff);
            this.txtLtdCode.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.chkDel_Gong.CheckedChanged += new EventHandler(eChkChanged);
            this.chkNewSeq.CheckedChanged += new EventHandler(eChkChanged);
        }

        private void eChkChanged(object sender, EventArgs e)
        {
            if (sender == chkNewSeq)
            {
                if (chkNewSeq.Checked)
                {
                    FbLtdSeq_New = true;
                    MessageBox.Show("저장시 해당 사업장내 신규 순번을 부여합니다.", "신규순번 부여");
                }
                else
                {
                    FbLtdSeq_New = false;
                }
            }
            else if (sender == chkDel_Gong)
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
            this.panMain.AddRequiredControl(nmrGjYear);
            this.panMain.AddRequiredControl(txtLtdCode);

            panMain.Initialize();

            SS1.DataSource = new List<HIC_CHUK_WORKER>();
            ssGONG.DataSource = new List<HIC_CHUKDTL_PLAN>();
            ssCHEMICAL.DataSource = new List<HIC_CHUKDTL_CHEMICAL>();
            ssSUBLTD.DataSource = new List<HIC_CHUKDTL_SUBLTD>();
            
            SS1.AddRows(1);
            ssGONG.AddRows(1);
            ssCHEMICAL.AddRows(1);
            ssSUBLTD.AddRows(1);
            
            nmrGjYear.Value = DateTime.Now.Year;
            cboGbSTS.SelectedIndex = 0;

            //포항성모병원 지정한계치
            nmrTLimit.Value = 480;

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
                panMain.SetData(item);

                //기본사업장 정보 출력
                HIC_LTD dHLTD = hicLtdService.GetMailCodebyCode(item.LTDCODE.To<string>("0"));

                if (!dHLTD.IsNullOrEmpty())
                {
                    txtSangho.Text = dHLTD.NAME;
                    txtMail.Text = dHLTD.MAILCODE;
                    txtJuso.Text = dHLTD.JUSO + " " + dHLTD.JUSODETAIL;
                    txtDaepyo.Text = dHLTD.DAEPYO;
                    txtTel.Text = dHLTD.TEL;
                }

                FstrRowid = item.RID;

                //사업장 측정자, 분석자 출력
                List<HIC_CHUK_WORKER> lsthCW = hicChukWorkerService.GetListByWrtno(argWRTNO);
                SS1.DataSource = lsthCW;
                SS1.AddRows(1);

                //유해인자별 측정계획
                List<HIC_CHUKDTL_PLAN> lstHCU = hicChukDtlPlanService.GetListByWrtno(argWRTNO);
                ssGONG.DataSource = lstHCU;

                //화학물질 사용실태
                List<HIC_CHUKDTL_CHEMICAL> lstHCC = hicChukDtlChemicalService.GetListByWrtno(argWRTNO);
                ssCHEMICAL.DataSource = lstHCC;

                //협력업체 현황
                List<HIC_CHUKDTL_SUBLTD> lstHCS = hicChukDtlSubltdService.GetListByWrtno(argWRTNO);
                ssSUBLTD.DataSource = lstHCS;
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtLtdCode.Text.Trim() != "")
                {
                    //기본사업장 정보 출력
                    HIC_LTD dHLTD = hicLtdService.GetMailCodebyCode(txtLtdCode.Text);

                    if (!dHLTD.IsNullOrEmpty())
                    {
                        txtSangho.Text = dHLTD.NAME;
                        txtMail.Text = dHLTD.MAILCODE;
                        txtJuso.Text = dHLTD.JUSO + " " + dHLTD.JUSODETAIL;
                        txtDaepyo.Text = dHLTD.DAEPYO;
                        txtTel.Text = dHLTD.TEL;
                    }
                }
            }
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            if (sender == SS1)
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
            else if (sender == ssSUBLTD)
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
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnDelete)
            {
                if (MessageBox.Show("사업장 예비조사 내역을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                Data_Save("DEL");
            }
            else if (sender == btnDel_Gong)
            {
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
                    }
                }
                
                Data_Save_Gong();
            }
            else if (sender == btnSave_Gong)
            {
                Data_Save_Gong();
            }
            else if (sender == btnSave_Chemical)
            {
                Data_Save_Chemical();
            }
            else if (sender == btnSave_SubLtd)
            {
                Data_Save_SubLtd();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
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
            else if (sender == btnAdd2)
            {
                int nRow = ssCHEMICAL.ActiveSheet.ActiveRowIndex + 1;
                ssCHEMICAL.InsertRows(nRow);
                return;
            }
            else if (sender == btnAdd3)
            {
                int nRow = ssSUBLTD.ActiveSheet.ActiveRowIndex + 1;
                ssSUBLTD.InsertRows(nRow);
                return;
            }
            else if (sender == btnPrint)
            {
                if (FnWRTNO > 0)
                {
                    HIC_CHUKMST_NEW item = hicChukMstNewService.GetItemByWrtno(FnWRTNO);

                    if (!item.IsNullOrEmpty())
                    {
                        item.MAILCODE = txtMail.Text.Trim();
                        item.JUSO = txtJuso.Text.Trim();
                        item.SANGHO = txtSangho.Text.Trim();
                        item.DAEPYO = txtDaepyo.Text.Trim();

                        frmHcPreChkPrint frm = new frmHcPreChkPrint(item);
                        frm.ShowDialog();

                        MessageBox.Show("출력완료");
                        return;
                    }
                }
            }
        }

        private void Set_Spread_Seqno_Sorting(FpSpread spd, int nCol)
        {
            int nSeq = 1;
            int nColCnt = spd.ActiveSheet.ColumnCount - 1;
            string strSel = "";

            for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
            {
                if ((RowStatus)((IList)spd.DataSource)[i].GetPropertieValue("RowStatus") != RowStatus.Delete)
                {
                    if (spd == ssGONG)
                    {
                        strSel = spd.ActiveSheet.Cells[i, 1].Text;
                    }
                    else
                    {
                        strSel = spd.ActiveSheet.Cells[i, 0].Text;
                    }

                    if (strSel != "Y")
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
                long nLTDCODE = txtLtdCode.Value.To<long>(0);

                if (!hicChukDtlSubltdService.Save(list4, FnWRTNO, nLTDCODE))
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

        /// <summary>
        /// 유해화학물 등록
        /// </summary>
        private void Data_Save_Chemical()
        {
            if (FnWRTNO == 0) { return; }

            //SeqNo 정리
            Set_Spread_Seqno_Sorting(ssCHEMICAL, 2);

            //화학물질 사용실태
            IList<HIC_CHUKDTL_CHEMICAL> list3 = ssCHEMICAL.GetEditbleData<HIC_CHUKDTL_CHEMICAL>();

            if (list3.Count > 0)
            {
                if (!hicChukDtlChemicalService.Save(list3, FnWRTNO))
                {
                    MessageBox.Show("유해화학물질 등록중 오류가 발생하였습니다. ");
                    return;
                }
                else
                {
                    MessageBox.Show("저장완료. ");
                }
            }
        }

        /// <summary>
        /// 측정대상 공정 및 유해인자별 측정계획
        /// </summary>
        private void Data_Save_Gong()
        {
            if (FnWRTNO == 0) { return; }

            //SeqNo 정리
            Set_Spread_Seqno_Sorting(ssGONG, 3);

            //측정대상 공정 및 유해인자별 측정계획
            IList<HIC_CHUKDTL_PLAN> list2 = ssGONG.GetEditbleData<HIC_CHUKDTL_PLAN>();

            if (list2.Count > 0)
            {
                if (!hicChukDtlPlanService.Save(list2, FnWRTNO))
                {
                    MessageBox.Show("측정대상 공정별 유해인자 등록중 오류가 발생하였습니다. ");
                    return;
                }
                else
                {
                    //유해인자별 측정계획
                    List<HIC_CHUKDTL_PLAN> lstHCU = hicChukDtlPlanService.GetListByWrtno(FnWRTNO, chkDel_Gong.Checked);
                    ssGONG.DataSource = null;
                    ssGONG.SetDataSource(lstHCU);

                    Spread_DelList_Display(chkDel_Gong.Checked, ssGONG);

                    MessageBox.Show("저장완료. ");
                }
            }
        }

        private void Data_Save(string argMode = "")
        {
            if (!panMain.RequiredValidate())
            {
                MessageBox.Show("필수 입력항목이 누락되었습니다.");
                return;
            }

            if (txtLtdCode.Value <= 0)
            {
                MessageBox.Show("사업장 코드 입력안됨.");
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

                //사업장관리순번 신규
                if (FbLtdSeq_New)
                {
                    item.LTDSEQNO = hicChukMstNewService.GetMaxLtdSeqNoByLtdCode(item.LTDCODE);
                    if (item.LTDSEQNO > 0) { item.LTDSEQNO += 1; } 
                }

                //누적치 계산 (신규등록시 계산)
                if (FnWRTNO == 0)
                {
                    nmrToAccum.Value = hicChukMstNewService.GetTotAccumByBangiYear(item.BANGI, item.CHKYEAR);       //총누적
                    nmrT5Accum.Value = hicChukMstNewService.GetT5AccumByBangiYear(item.BANGI, item.CHKYEAR);       //5인이상 누적
                    nmrT5Limit.Value = hicChukMstNewService.GetT5LimitByBangiYear(item.BANGI, item.CHKYEAR);       //국고누적
                }

                if (argMode == "DEL")
                {
                    item.RowStatus = ComBase.Mvc.RowStatus.Delete;
                    item.DELSABUN = clsType.User.IdNumber.To<long>(0);
                }

                hicChukMstNewService.Save(item);

                if (FnWRTNO == 0) { FnWRTNO = item.WRTNO; }

                //측정자, 분석자 내역 등록
                IList<HIC_CHUK_WORKER> list = SS1.GetEditbleData<HIC_CHUK_WORKER>();

                if (list.Count > 0)
                {
                    if (argMode == "DEL")
                    {
                        foreach (HIC_CHUK_WORKER code in list)
                        {
                            code.RowStatus = ComBase.Mvc.RowStatus.Delete;
                        }
                    }

                    if (!hicChukWorkerService.Save(list, FnWRTNO))
                    {
                        MessageBox.Show("측정자 등록중 오류가 발생하였습니다. ");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                if (cboGbSTS.SelectedIndex > 0)
                {
                    //SeqNo 정리
                    Set_Spread_Seqno_Sorting(ssGONG, 3);

                    //측정대상 공정 및 유해인자별 측정계획
                    IList<HIC_CHUKDTL_PLAN> list2 = ssGONG.GetEditbleData<HIC_CHUKDTL_PLAN>();

                    if (list2.Count > 0)
                    {
                        if (argMode == "DEL")
                        {
                            foreach (HIC_CHUKDTL_PLAN code in list2)
                            {
                                code.RowStatus = ComBase.Mvc.RowStatus.Delete;
                            }
                        }

                        if (!hicChukDtlPlanService.Save(list2, FnWRTNO))
                        {
                            MessageBox.Show("측정대상 공정별 유해인자 등록중 오류가 발생하였습니다. ");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

                    //SeqNo 정리
                    Set_Spread_Seqno_Sorting(ssCHEMICAL, 2);

                    //화학물질 사용실태
                    IList<HIC_CHUKDTL_CHEMICAL> list3 = ssCHEMICAL.GetEditbleData<HIC_CHUKDTL_CHEMICAL>();

                    if (list3.Count > 0)
                    {
                        if (argMode == "DEL")
                        {
                            foreach (HIC_CHUKDTL_CHEMICAL code in list3)
                            {
                                code.RowStatus = ComBase.Mvc.RowStatus.Delete;
                            }
                        }

                        if (!hicChukDtlChemicalService.Save(list3, FnWRTNO))
                        {
                            MessageBox.Show("유해화학물질 등록중 오류가 발생하였습니다. ");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }

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

                        long nLTDCODE = txtLtdCode.Value.To<long>(0);

                        if (!hicChukDtlSubltdService.Save(list4, FnWRTNO, nLTDCODE))
                        {
                            MessageBox.Show("협력업체 내역 등록중 오류가 발생하였습니다. ");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
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

        private void Ltd_Code_Help()
        {
            string strFind = "";

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind, "측정");
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtSangho.Text = LtdHelpItem.NAME;
                txtMail.Text = LtdHelpItem.MAILCODE;
                txtJuso.Text = LtdHelpItem.JUSO + " " + LtdHelpItem.JUSODETAIL;
                txtDaepyo.Text = LtdHelpItem.DAEPYO;
                txtTel.Text = LtdHelpItem.TEL;
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

    }
}
