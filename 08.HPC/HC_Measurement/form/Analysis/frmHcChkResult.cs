using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
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
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkResult :CommonForm
    {
        HicChukMstNewService hicChukMstNewService = null;
        HicChukDtlResultService hicChukDtlResultService = null;
        HcCodeService hcCodeService = null;

        clsSpread cSpd = null;
        clsHcSpd cHSpd = null;
        FpSpread Spd = null;

        string FstrComCode = "";
        string FstrComName = "";
        string FstrComGCode = "";
        string FstrComGCode1 = "";
        string FstrTWA_PPM = "";
        string FstrTWA_MG = "";
        string FstrSTEL_PPM = "";
        string FstrSTEL_MG = "";
        string FstrUNIT = "";

        bool FbNew = false;     //결과입력 자료입력 신규 구분 

        long FnWRTNO = 0;

        public frmHcChkResult()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnAdd_Add.Click += new EventHandler(eBtnClick);
            this.btnAdd_Ins.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.ssUCD.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.ssNOISE.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.ssUCD.ClipboardPasted += new ClipboardPastedEventHandler(eSpdClipBoard);
            this.ssNOISE.ClipboardPasted += new ClipboardPastedEventHandler(eSpdClipBoard);
        }

        private void eSpdClipBoard(object sender, ClipboardPastedEventArgs e)
        {
            int nRow = e.CellRange.Row;
            int nRow2 = e.CellRange.Row + e.CellRange.RowCount - 1;
            int nCol = e.CellRange.Column;
            int nCol2 = e.CellRange.Column + e.CellRange.ColumnCount - 1;

            if (sender == ssUCD)
            {
                if (nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG_ETC ||
                    nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV_ETC || 
                    nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW_ETC)
                {
                    //변경된 Cell로 마킹
                    for (int i = nRow; i <= nRow2; i++)
                    {
                        Apply_Cell_Condition((FpSpread)sender, i, nCol);
                    }
                }
            }
            else if (sender == ssNOISE)
            {
                if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG_ETC || 
                    nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV_ETC || 
                    nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW_ETC)
                {
                    //변경된 Cell로 마킹
                    for (int i = nRow; i <= nRow2; i++)
                    {
                        Apply_Cell_Condition((FpSpread)sender, i, nCol);
                    }
                }
            }
            
        }

        private void eSpdEditModeOff(object sender, EventArgs e)
        {
            int nRow = ((FpSpread)sender).ActiveSheet.ActiveRowIndex;
            int nCol = ((FpSpread)sender).ActiveSheet.ActiveColumnIndex;

            if (nRow >= 0 && nCol >= 0)
            {
                Apply_Cell_Condition(((FpSpread)sender), nRow, nCol);
            }
        }

        private void Apply_Cell_Condition(FpSpread spd, int nRow, int nCol, bool bValDisp = true)
        {
            int nCol2 = nCol;
            int nColNm = 0;
            string strVal = spd.ActiveSheet.Cells[nRow, nCol].Text.Trim();
            HC_CODE item = new HC_CODE();

            //소음제외 결과
            if (spd == ssUCD)
            {
                if (nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("NOW_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("PREV_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("NOW_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes.LABOR_TIME)
                {
                    //노출보정값 계산
                    double dTime = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.LABOR_TIME].Text.To<double>(0.0);
                    double dSTDR = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text.To<double>(0.0);

                    if (dTime > 8.0)
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = Math.Round(dSTDR * (8 / dTime), 4).To<string>("");
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = dSTDR.To<string>("");
                    }
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW)
                {
                    double dNOW = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW].Text.To<double>(0.0);
                    double dSTDR = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text.To<double>(0.0);

                    if (dNOW > dSTDR)
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEN_EVL_RESULT].Value = 1;     //초과
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEN_EVL_RESULT].Value = 0;     //미만
                    }
                }

                if (nColNm > 0)
                {
                    if (strVal == "1")
                    {
                        spd.ActiveSheet.Cells[nRow, nCol2].Locked = false;
                        if (!item.IsNullOrEmpty()) { spd.ActiveSheet.Cells[nRow, nColNm].Text = item.CodeName.To<string>(""); }
                        if (bValDisp) { spd.ActiveSheet.Cells[nRow, nCol2].Text = "0"; }
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, nCol2].Locked = true;
                        if (!item.IsNullOrEmpty()) { spd.ActiveSheet.Cells[nRow, nColNm].Text = item.CodeName.To<string>(""); }
                        if (bValDisp) { spd.ActiveSheet.Cells[nRow, nCol2].Text = ""; }                        
                    }

                }
            }
            //소음 결과
            else if (spd == ssNOISE)
            {
                if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("NOW_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("PREV_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW_ETC)
                {
                    nColNm = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW_NM;
                    nCol2 = (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW;

                    item = hcCodeService.FindActiveCodeByGroupAndCode("NOW_ETC", strVal, "WEM");
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.LABOR_TIME)
                {
                    //노출보정값 계산
                    double dTime = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.LABOR_TIME].Text.To<double>(0.0);
                    double dSTDR = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_default].Text.To<double>(0.0);

                    if (dTime > 8.0)
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_VALUE].Text = (Math.Round(16.61 * Math.Log10(100.0 / (12.5 * dTime)), 4) + 90.0).To<string>("");
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_VALUE].Text = dSTDR.To<string>("");
                    }
                }
                else if (nCol == (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW)
                {
                    double dNOW = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW].Text.To<double>(0.0);
                    double dSTDR = spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_VALUE].Text.To<double>(0.0);

                    if (dNOW > dSTDR)
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEN_EVL_RESULT].Value = 1;     //초과
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEN_EVL_RESULT].Value = 0;     //미만
                    }
                }

                if (nColNm > 0)
                {
                    if (strVal == "1")
                    {
                        spd.ActiveSheet.Cells[nRow, nCol2].Locked = false;
                        if (!item.IsNullOrEmpty()) { spd.ActiveSheet.Cells[nRow, nColNm].Text = item.CodeName.To<string>(""); }
                        if (bValDisp) { spd.ActiveSheet.Cells[nRow, nCol2].Text = "0"; }
                    }
                    else 
                    {
                        spd.ActiveSheet.Cells[nRow, nCol2].Locked = true;
                        if (!item.IsNullOrEmpty()) { spd.ActiveSheet.Cells[nRow, nColNm].Text = item.CodeName.To<string>(""); }
                        if (bValDisp) { spd.ActiveSheet.Cells[nRow, nCol2].Text = ""; }
                    }
                }
            }
        }

        private void SetControl()
        {
            hicChukMstNewService = new HicChukMstNewService();
            hicChukDtlResultService = new HicChukDtlResultService();
            hcCodeService = new HcCodeService();

            cSpd = new clsSpread();
            cHSpd = new clsHcSpd();

            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 34, RowHeight = 30, RowHeaderVisible = true });

            SSList.AddColumn("번호",           nameof(HIC_CHUKMST_NEW.WRTNO),       46, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("년도",           nameof(HIC_CHUKMST_NEW.CHKYEAR),     46, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("반기",           nameof(HIC_CHUKMST_NEW.BANGI),       52, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("사업장코드",     nameof(HIC_CHUKMST_NEW.LTDCODE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("사업장명",       nameof(HIC_CHUKMST_NEW.LTDNAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("측정일자 Fr",    nameof(HIC_CHUKMST_NEW.SDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("측정일자 To",    nameof(HIC_CHUKMST_NEW.EDATE),       92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("ROWID",          nameof(HIC_CHUKMST_NEW.RID),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            Set_Spread_Not_Noise();
            Set_Spread_Noise();
        }

        /// <summary>
        /// 소음 Spread Set
        /// </summary>
        private void Set_Spread_Noise()
        {
            SpreadComboBoxData scbChkJugi = hcCodeService.GetSpreadComboBoxData("OCCRRNC_CYCLE_CD", "WEM");
            SpreadComboBoxData scbCriGbn = hcCodeService.GetSpreadComboBoxData("CRITERIA_GBN", "WEM");
            SpreadComboBoxData scbRes = hcCodeService.GetSpreadComboBoxData("WEM_RESULT", "WEM");
            SpreadComboBoxData scbUnit = hcCodeService.GetSpreadComboBoxData("CRITERIA_UNIT", "WEM");
            
            ssNOISE.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 44 });
            ssNOISE.AddColumnCheckBox("선택",         nameof(HIC_CHUKDTL_RESULT.IsDelete), 38, new CheckBoxFlagEnumCellType<IsActive>() { });
            ssNOISE.AddColumn("일련번호",             nameof(HIC_CHUKDTL_RESULT.WRTNO), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumn("구분",                 nameof(HIC_CHUKDTL_RESULT.GUBUN), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumnNumber("순번",           nameof(HIC_CHUKDTL_RESULT.SEQNO), 38, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssNOISE.AddColumn("부서명",               nameof(HIC_CHUKDTL_RESULT.DEPT_NM), 52, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssNOISE.AddColumn("공정코드",             nameof(HIC_CHUKDTL_RESULT.PROCS_CD), 44, new SpreadCellTypeOption { });                                                                     //5
            ssNOISE.AddColumn("공정명",               nameof(HIC_CHUKDTL_RESULT.PROCS_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssNOISE.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_NOISE_HELP;       //7
            ssNOISE.AddColumn("단위작업명",           nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM), 84, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssNOISE.AddColumn("유해물질코드",         nameof(HIC_CHUKDTL_RESULT.CHMCLS_CD), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumn("그룹",                 nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_CD), 38, FpSpreadCellType.CheckBoxCellType, new SpreadCellTypeOption { IsVisivle = false, TextFalse = "" });           //10
            ssNOISE.AddColumn("그룹순번",             nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_SEQ), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumn("유해물질명",           nameof(HIC_CHUKDTL_RESULT.CHMCLS_NM), 99, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            ssNOISE.AddColumn("근로자수",             nameof(HIC_CHUKDTL_RESULT.LABRR_CD), 44, new SpreadCellTypeOption { });                                                                             
            ssNOISE.AddColumn("작업내용",             nameof(HIC_CHUKDTL_RESULT.OPERT_CN), 74, new SpreadCellTypeOption { });
            ssNOISE.AddColumn("근로형태",             nameof(HIC_CHUKDTL_RESULT.LABOR_CND), 92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                                          //15
            ssNOISE.AddColumnNumber("근로시간",       nameof(HIC_CHUKDTL_RESULT.LABOR_TIME), 40, new SpreadCellTypeOption { });
            ssNOISE.AddColumnComboBox("발생형태",     nameof(HIC_CHUKDTL_RESULT.UCODE_EXPSR_CYCLE), 52, IsReadOnly.N, scbChkJugi, new SpreadCellTypeOption { });                                                  
            ssNOISE.AddColumnNumber("발생시간(분)",   nameof(HIC_CHUKDTL_RESULT.UCODE_EXPSR_TIME), 44, new SpreadCellTypeOption { });
            ssNOISE.AddColumn("측정위치",             nameof(HIC_CHUKDTL_RESULT.WEM_LC), 44, new SpreadCellTypeOption { });
            ssNOISE.AddColumn("근로자명",             nameof(HIC_CHUKDTL_RESULT.LABRR_NM), 84, new SpreadCellTypeOption { });                                                                                 //20
            ssNOISE.AddColumn("측정시작",             nameof(HIC_CHUKDTL_RESULT.WEM_TIME_FROM), 44, new SpreadCellTypeOption { });
            ssNOISE.AddColumn("측정종료",             nameof(HIC_CHUKDTL_RESULT.WEM_TIME_TO), 44, new SpreadCellTypeOption { });
            ssNOISE.AddColumnNumber("측정횟수",       nameof(HIC_CHUKDTL_RESULT.WEM_CO), 44, new SpreadCellTypeOption { TextMaxLength = 3 });
            ssNOISE.AddColumn("측정평균",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });                                      //24
            ssNOISE.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });                                      
            ssNOISE.AddColumn("측정치",               nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });
            ssNOISE.AddColumn("측정전회",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });                                      //27      
            ssNOISE.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("전회",                 nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });   //29
            ssNOISE.AddColumn("측정금회",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });                                       //30
            ssNOISE.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });
            ssNOISE.AddColumn("금회",                 nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });     //32
            ssNOISE.AddColumn("노출기준기본",         nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_default), 44, new SpreadCellTypeOption { IsVisivle = false });                                                                         //33
            ssNOISE.AddColumn("노출기준",             nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_VALUE), 44, new SpreadCellTypeOption { });                                                                         //33
            ssNOISE.AddColumnComboBox("노출구분",     nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_SE), 72, IsReadOnly.N, scbCriGbn, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumnComboBox("노출단위",     nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT), 62, IsReadOnly.N, scbUnit, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumnComboBox("측정평가결과", nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT), 58, IsReadOnly.N, scbRes, new SpreadCellTypeOption { });                                                 
            ssNOISE.AddColumn("분석방법코드",         nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_CD), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssNOISE.AddColumn("측정방법",             nameof(HIC_CHUKDTL_RESULT.WEM_MTH_NM), 92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });                  //38
            ssNOISE.AddColumn("분석방법",             nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_NM), 92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssNOISE.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_NOISE_HELP;                                                                             //40
            ssNOISE.AddColumn("비고",                 nameof(HIC_CHUKDTL_RESULT.REMARK), 92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                                         //41
            ssNOISE.AddColumn("ROWID",                nameof(HIC_CHUKDTL_RESULT.RID), 99, new SpreadCellTypeOption { IsVisivle = false });                                                                //42
        }

        private void ssGONG_NOISE_HELP(object sender, EditorNotifyEventArgs e)
        {
            string strGubun = "";
            string strKeyWord = "";

            FstrComCode = "";
            FstrComName = "";
            FstrComGCode = "";
            FstrComGCode1 = "";

            if (e.Column == (int)clsHcSpd.enmHcWemRes_Noise.btnPROCS)
            {
                strGubun = "GONG";
                strKeyWord = ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.PROCS_NM].Text.Trim();
            }
            else if (e.Column == (int)clsHcSpd.enmHcWemRes_Noise.btnANAL)    //분석방법
            {
                strGubun = "ANAL";
                strKeyWord = ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.ANALS_MTH_CD].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

            if (!FstrComCode.IsNullOrEmpty())
            {
                if (strGubun == "GONG")
                {
                    ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.PROCS_CD].Text = FstrComCode.Trim();
                    ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.PROCS_NM].Text = FstrComName.Trim();
                }
                else if (strGubun == "ANAL")
                {
                    ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.ANALS_MTH_CD].Text = FstrComCode.Trim();
                    ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.WEM_MTH_NM].Text = FstrComGCode.Trim();
                    ssNOISE.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes_Noise.ANALS_MTH_NM].Text = FstrComGCode1.Trim();
                }
            }
        }

        /// <summary>
        /// 소음제외 Spread Set
        /// </summary>
        private void Set_Spread_Not_Noise()
        {
            SpreadComboBoxData scbCriGbn = hcCodeService.GetSpreadComboBoxData("CRITERIA_GBN", "WEM");
            SpreadComboBoxData scbRes = hcCodeService.GetSpreadComboBoxData("WEM_RESULT", "WEM");
            SpreadComboBoxData scbUnit = hcCodeService.GetSpreadComboBoxData("CRITERIA_UNIT", "WEM");
            
            ssUCD.Initialize(new SpreadOption { RowHeaderVisible = false, RowHeight = 26, ColumnHeaderHeight = 44 });
            ssUCD.AddColumnCheckBox("선택",         nameof(HIC_CHUKDTL_RESULT.IsDelete), 38, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false });
            ssUCD.AddColumn("일련번호",             nameof(HIC_CHUKDTL_RESULT.WRTNO), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD.AddColumn("구분",                 nameof(HIC_CHUKDTL_RESULT.GUBUN), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD.AddColumnNumber("순번",           nameof(HIC_CHUKDTL_RESULT.SEQNO), 38, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            ssUCD.AddColumn("부서명",               nameof(HIC_CHUKDTL_RESULT.DEPT_NM), 52, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumn("공정코드",             nameof(HIC_CHUKDTL_RESULT.PROCS_CD), 44, new SpreadCellTypeOption { });                                                                     //5
            ssUCD.AddColumn("공정명",               nameof(HIC_CHUKDTL_RESULT.PROCS_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;   //7
            ssUCD.AddColumn("단위작업명",           nameof(HIC_CHUKDTL_RESULT.UNIT_WRKRUM_NM), 84, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumn("유해물질코드",         nameof(HIC_CHUKDTL_RESULT.CHMCLS_CD), 54, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumnCheckBox("그룹",         nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_CD), 38, new CheckBoxFlagEnumCellType<IsActive>() { });           //10
            ssUCD.AddColumn("그룹순번",             nameof(HIC_CHUKDTL_RESULT.UCODE_GROUP_SEQ), 99, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD.AddColumn("유해물질명",           nameof(HIC_CHUKDTL_RESULT.CHMCLS_NM), 130, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssCHMCLS_HELP;   //13
            ssUCD.AddColumn("근로자수",             nameof(HIC_CHUKDTL_RESULT.LABRR_CD), 44, new SpreadCellTypeOption { });                                                                             
            ssUCD.AddColumn("근로형태",             nameof(HIC_CHUKDTL_RESULT.LABOR_CND), 92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                              //15
            ssUCD.AddColumnNumber("근로시간",       nameof(HIC_CHUKDTL_RESULT.LABOR_TIME), 40, new SpreadCellTypeOption { });
            ssUCD.AddColumnNumber("발생시간(분)",   nameof(HIC_CHUKDTL_RESULT.UCODE_EXPSR_TIME), 44, new SpreadCellTypeOption { });
            ssUCD.AddColumn("측정위치",             nameof(HIC_CHUKDTL_RESULT.WEM_LC), 44, new SpreadCellTypeOption { });
            ssUCD.AddColumn("근로자명",             nameof(HIC_CHUKDTL_RESULT.LABRR_NM), 84, new SpreadCellTypeOption { });
            ssUCD.AddColumn("측정시작",             nameof(HIC_CHUKDTL_RESULT.WEM_TIME_FROM), 44, new SpreadCellTypeOption { });
            ssUCD.AddColumn("측정종료",             nameof(HIC_CHUKDTL_RESULT.WEM_TIME_TO), 44, new SpreadCellTypeOption { });                                                                  //20
            ssUCD.AddColumnNumber("측정횟수",       nameof(HIC_CHUKDTL_RESULT.WEM_CO), 44, new SpreadCellTypeOption { TextMaxLength = 3 });
            ssUCD.AddColumn("측정평균",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });                                      //22                  
            ssUCD.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });                                      
            ssUCD.AddColumn("측정치",               nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_AVRG), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });
            ssUCD.AddColumn("측정전회",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });                                      //25                  
            ssUCD.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("전회",                 nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_PREV), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });   //27
            ssUCD.AddColumn("측정금회",             nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW_ETC), 38, new SpreadCellTypeOption { BackColor = System.Drawing.Color.PowderBlue, TextMaxLength = 1 });
            ssUCD.AddColumn("명칭",                 "", 84, new SpreadCellTypeOption { IsEditble = false });
            ssUCD.AddColumn("금회",                 nameof(HIC_CHUKDTL_RESULT.WEM_VALUE_NOW), 52, new SpreadCellTypeOption { IsEditble = false, DecimalPlaces = 3, ForceColor = System.Drawing.Color.DarkBlue, Aligen = CellHorizontalAlignment.Right });     //30
            ssUCD.AddColumn("노출기준기본",         nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_default), 44, new SpreadCellTypeOption { IsVisivle = false });                                                                         //33
            ssUCD.AddColumn("노출기준",             nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_VALUE), 44, new SpreadCellTypeOption { });
            ssUCD.AddColumnComboBox("노출구분",     nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_SE), 72, IsReadOnly.N, scbCriGbn, new SpreadCellTypeOption { IsVisivle = false });
            ssUCD.AddColumnComboBox("노출단위",     nameof(HIC_CHUKDTL_RESULT.EXPSR_STDR_UNIT), 62, IsReadOnly.N, scbUnit, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });    //33
            ssUCD.AddColumnComboBox("측정평가결과", nameof(HIC_CHUKDTL_RESULT.WEN_EVL_RESULT), 58, IsReadOnly.N, scbRes, new SpreadCellTypeOption { });                                                 
            ssUCD.AddColumn("분석방법코드",         nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_CD), 99, new SpreadCellTypeOption { IsVisivle = false });                                     //35
            ssUCD.AddColumn("채취방법",             nameof(HIC_CHUKDTL_RESULT.WEM_MTH_NM), 92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumn("분석방법",             nameof(HIC_CHUKDTL_RESULT.ANALS_MTH_NM), 92, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssUCD.AddColumnButton("H", 22, new SpreadCellTypeOption { ButtonText = "?" }).ButtonClick += ssGONG_HELP;                                                                         //38
            ssUCD.AddColumn("비고",                 nameof(HIC_CHUKDTL_RESULT.REMARK), 92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });                                     
            ssUCD.AddColumn("ROWID",                nameof(HIC_CHUKDTL_RESULT.RID), 99, new SpreadCellTypeOption { IsVisivle = false });                                                      //40
        }

        private void ssCHMCLS_HELP(object sender, EditorNotifyEventArgs e)
        {
            string strGubun = "";
            string strKeyWord = "";

            FstrComCode = "";
            FstrComName = "";
            FstrComGCode = "";
            FstrComGCode1 = "";
            FstrTWA_PPM = "";
            FstrTWA_MG = "";
            FstrSTEL_PPM = "";
            FstrSTEL_MG = "";
            FstrUNIT = "";

            if (e.Column == (int)clsHcSpd.enmHcWemRes.btnCHMCLS)
            {
                strGubun = "MCODE";
                strKeyWord = ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.CHMCLS_NM].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetMCodeValue += new frmHcChkCodeHelp.SetMCodeValue(eValueCode_Chmcls);
            frm.ShowDialog();
            frm.rSetMCodeValue -= new frmHcChkCodeHelp.SetMCodeValue(eValueCode_Chmcls);

            
            if (!FstrComCode.IsNullOrEmpty())
            {
                ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.CHMCLS_CD].Text = FstrComCode.Trim();
                ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.CHMCLS_NM].Text = FstrComName.Trim();

                //노출기준 선택한 경우
                if (!FstrComGCode1.IsNullOrEmpty())
                {
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text = VB.Replace(FstrComGCode1, "C", "").Trim();
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = VB.Replace(FstrComGCode1, "C", "").Trim();

                    if (FstrUNIT == "ppm")
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 1;
                    }
                    else if (FstrUNIT == "mg")
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 2;
                    }
                }
                //노출기준 선택 안한 경우
                else
                {
                    if (!FstrTWA_PPM.IsNullOrEmpty())
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text = VB.Replace(FstrTWA_PPM, "C", "").Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = VB.Pstr(FstrTWA_PPM, "C", 2).Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 1;
                    }
                    else if (!FstrTWA_MG.IsNullOrEmpty())
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text = VB.Replace(FstrTWA_MG, "C", "").Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = VB.Pstr(FstrTWA_MG, "C", 2).Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 2;
                    }
                    else if (!FstrSTEL_PPM.IsNullOrEmpty())
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text = VB.Replace(FstrSTEL_PPM, "C", "").Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = VB.Pstr(FstrSTEL_PPM, "C", 2).Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 1;
                    }
                    else if (!FstrSTEL_MG.IsNullOrEmpty())
                    {
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text = VB.Replace(FstrSTEL_MG, "C", "").Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text = VB.Pstr(FstrSTEL_MG, "C", 2).Trim();
                        ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value = 2;
                    }
                }
            }
        }

        private void ssGONG_HELP(object sender, EditorNotifyEventArgs e)
        {
            string strGubun = "";
            string strKeyWord = "";

            FstrComCode = "";
            FstrComName = "";
            FstrComGCode = "";
            FstrComGCode1 = "";

            if (e.Column == (int)clsHcSpd.enmHcWemRes.btnPROCS)
            {
                strGubun = "GONG";
                strKeyWord = ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.PROCS_NM].Text.Trim();
            }
            else if (e.Column == (int)clsHcSpd.enmHcWemRes.btnANAL)    //분석방법
            {
                strGubun = "ANAL";
                strKeyWord = ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.ANALS_MTH_CD].Text.Trim();
            }

            frmHcChkCodeHelp frm = new frmHcChkCodeHelp(strGubun, strKeyWord);
            frm.rSetGstrValue += new frmHcChkCodeHelp.SetGstrValue(eValueCode);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcChkCodeHelp.SetGstrValue(eValueCode);

            if (!FstrComCode.IsNullOrEmpty())
            {
                if (strGubun == "GONG")
                {
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.PROCS_CD].Text = FstrComCode.Trim();
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.PROCS_NM].Text = FstrComName.Trim();
                }
                else if (strGubun == "ANAL")
                {
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.ANALS_MTH_CD].Text = FstrComCode.Trim();
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.WEM_MTH_NM].Text = FstrComGCode.Trim();
                    ssUCD.ActiveSheet.Cells[e.Row, (int)clsHcSpd.enmHcWemRes.ANALS_MTH_NM].Text = FstrComGCode1.Trim();
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

        private void eValueCode_Chmcls(string strCode, string strName, string strGCode, string strGCode1, string TWA_PPM, string TWA_MG, string STEL_PPM, string STEL_MG, string UNIT)
        {
            FstrComCode = strCode;
            FstrComName = strName;
            FstrComGCode = strGCode;
            FstrComGCode1 = strGCode1;
            FstrTWA_PPM = TWA_PPM;
            FstrTWA_MG = TWA_MG;
            FstrSTEL_PPM = STEL_PPM;
            FstrSTEL_MG = STEL_MG;
            FstrUNIT = UNIT;
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                Screen_Clear();

                long nWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>(0);
                FnWRTNO = nWRTNO;

                Screen_Display(FnWRTNO);
            }
        }

        void Screen_Display(long nWRTNO)
        {
            //최종작업내역
            HIC_CHUKMST_NEW hCMN = hicChukMstNewService.GetItemByWrtno(nWRTNO);

            if (!hCMN.IsNullOrEmpty())
            {
                List<HIC_CHUKDTL_RESULT> lstUCD = hicChukDtlResultService.GetListByWrtno(hCMN.WRTNO, "1");      //소음제외
                List<HIC_CHUKDTL_RESULT> lstNOISE = hicChukDtlResultService.GetListByWrtno(hCMN.WRTNO, "2");    //소음

                if (lstUCD.Count > 0 || lstNOISE.Count > 0)
                {
                    ssUCD.SetDataSource(lstUCD);
                    ssNOISE.SetDataSource(lstNOISE);
                    FbNew = false;
                }
                else
                {
                    if (MessageBox.Show("측정계획에서 입력한 내용을 가져오시겠습니까?", "신규 측정결과입력", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        FbNew = true;
                        return;
                    }

                    List<HIC_CHUKDTL_RESULT> lstUCD2 = hicChukDtlResultService.GetPlanListByWrtno(hCMN.WRTNO, "1");
                    List<HIC_CHUKDTL_RESULT> lstNOISE2 = hicChukDtlResultService.GetPlanListByWrtno(hCMN.WRTNO, "2");

                    ssUCD.SetDataSource(lstUCD2);
                    ssNOISE.SetDataSource(lstNOISE2);
                    FbNew = true;
                }

                //변경된 Cell로 마킹
                for (int i = 0; i < ssUCD.ActiveSheet.RowCount; i++)
                {
                    Apply_Cell_Condition(ssUCD, i, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG_ETC, false);
                    Apply_Cell_Condition(ssUCD, i, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV_ETC, false);
                    Apply_Cell_Condition(ssUCD, i, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW_ETC, false);
                }

                for (int i = 0; i < ssNOISE.ActiveSheet.RowCount; i++)
                {
                    Apply_Cell_Condition(ssNOISE, i, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG_ETC, false);
                    Apply_Cell_Condition(ssNOISE, i, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV_ETC, false);
                    Apply_Cell_Condition(ssNOISE, i, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW_ETC, false);
                }
            }
        }

        private void Screen_Clear()
        {
            FbNew = false;
            ssUCD.DataSource = null;
            ssNOISE.DataSource = null;
            cSpd.Spread_Clear_Simple(ssUCD);
            cSpd.Spread_Clear_Simple(ssNOISE);
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
            else if (sender == btnAdd_Add || sender == btnAdd_Ins)
            {
                if (tabControl1.SelectedTab == tabPage1)
                {
                    Spd = ssUCD;
                }
                else
                {
                    Spd = ssNOISE;
                }

                Set_Spread_Add(Spd, sender);

                Spd = null;
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnDelete)
            {
                Spd = tabControl1.SelectedTab == tabPage1 ? ssUCD : ssNOISE;

                if (MessageBox.Show("선택항목을 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
                {
                    if (Spd.ActiveSheet.Cells[i, 0].Text == "Y")
                    {
                        HIC_CHUKDTL_RESULT code = Spd.GetRowData(i) as HIC_CHUKDTL_RESULT;

                        Spd.DeleteRow(i);
                    }
                }

                Data_Save();

                Screen_Clear();
                Screen_Display(FnWRTNO);
            }
        }

        private void Set_Spread_Add(FpSpread Spd, object sender)
        {
            int nRow = -1;
            int nRowCnt = Spd.ActiveSheet.RowCount;

            List<HIC_CHUKDTL_RESULT> lstDto = new List<HIC_CHUKDTL_RESULT>();
            List<HIC_CHUKDTL_RESULT> lstDto_New = new List<HIC_CHUKDTL_RESULT>();

            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if (Spd.ActiveSheet.Cells[i, 0].Text == "Y")
                {
                    nRow = i;

                    HIC_CHUKDTL_RESULT code = new HIC_CHUKDTL_RESULT();
                    code = GetSpreadRowData(Spd, i, false);

                    lstDto.Add(code);
                }
            }

            if (nRow >= 0)
            {
                if (sender == btnAdd_Ins)
                {
                    Spd.InsertRows(nRow + 1, lstDto.Count);
                }
                else
                {
                    Spd.AddRows(lstDto.Count);
                }

                for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
                {
                    HIC_CHUKDTL_RESULT code = new HIC_CHUKDTL_RESULT();
                    code = GetSpreadRowData(Spd, i);

                    lstDto_New.Add(code);
                }

                Spd.DataSource = null;
                Spd.SetDataSource(lstDto_New);

                if (sender == btnAdd_Ins)
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

                for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
                {
                    HIC_CHUKDTL_RESULT code = new HIC_CHUKDTL_RESULT();
                    code = GetSpreadRowData(Spd, i);

                    if (code.RID.IsNullOrEmpty())
                    {
                        ((IList)Spd.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Insert);
                    }
                    else
                    {
                        ((IList)Spd.DataSource)[i].SetPropertieValue("RowStatus", RowStatus.Update);
                    }
                }

            }
            else
            {
                int nActRow = Spd.ActiveSheet.ActiveRowIndex + 1;

                if (sender == btnAdd_Add)
                {
                    Spd.AddRows();
                }
                else if (sender == btnAdd_Ins)
                {
                    Spd.InsertRows(nActRow);
                }
            }

        }

        private HIC_CHUKDTL_RESULT GetSpreadRowData(FpSpread Spd, int nRow, bool bRID = true)
        {
            HIC_CHUKDTL_RESULT hCR = new HIC_CHUKDTL_RESULT();

            if (Spd == ssUCD)
            {
                hCR.WRTNO               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WRTNO].Text.To<long>(0);
                hCR.GUBUN               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.GUBUN].Text;
                hCR.SEQNO               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.SEQNO].Text.To<long>(0);
                hCR.DEPT_NM             = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.DEPT_NM].Text;
                hCR.PROCS_CD            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.PROCS_CD].Text;
                hCR.PROCS_NM            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.PROCS_NM].Text;
                hCR.UNIT_WRKRUM_NM      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.UNIT_WRKRUM_NM].Text;
                hCR.CHMCLS_CD           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.CHMCLS_CD].Text;
                hCR.UCODE_GROUP_CD      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.UCODE_GROUP_CD].Value.To<string>("N");
                hCR.UCODE_GROUP_SEQ     = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.UCODE_GROUP_SEQ].Text.To<long>(0);
                hCR.CHMCLS_NM           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.CHMCLS_NM].Text;
                hCR.LABRR_CD            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.LABRR_CD].Text;
                hCR.LABOR_CND           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.LABOR_CND].Text;
                hCR.LABOR_TIME          = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.LABOR_TIME].Text.To<long>(0);
                hCR.UCODE_EXPSR_TIME    = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.UCODE_EXPSR_TIME].Text.To<long>(0);
                hCR.WEM_LC              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_LC].Text;
                hCR.LABRR_NM            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.LABRR_NM].Text;
                hCR.WEM_TIME_FROM       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_TIME_FROM].Text;
                hCR.WEM_TIME_TO         = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_TIME_TO].Text;
                hCR.WEM_CO              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_CO].Text.To<long>(0);
                hCR.WEM_VALUE_AVRG_ETC  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG_ETC].Text;
                hCR.WEM_VALUE_AVRG      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_AVRG].Text.To<decimal>();
                hCR.WEM_VALUE_PREV_ETC  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV_ETC].Text;
                hCR.WEM_VALUE_PREV      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_PREV].Text.To<decimal>();
                hCR.WEM_VALUE_NOW_ETC   = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW_ETC].Text;
                hCR.WEM_VALUE_NOW       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_VALUE_NOW].Text.To<decimal>();
                hCR.EXPSR_STDR_default  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_default].Text;
                hCR.EXPSR_STDR_VALUE    = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_VALUE].Text;
                hCR.EXPSR_STDR_SE       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_SE].Text;
                hCR.EXPSR_STDR_UNIT     = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.EXPSR_STDR_UNIT].Value.To<string>();
                hCR.WEN_EVL_RESULT      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEN_EVL_RESULT].Value.To<string>();
                hCR.ANALS_MTH_CD        = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.ANALS_MTH_CD].Text;
                hCR.WEM_MTH_NM          = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.WEM_MTH_NM].Text;
                hCR.ANALS_MTH_NM        = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.ANALS_MTH_NM].Text;
                hCR.REMARK              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.REMARK].Text;
                hCR.RID                 = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes.RID].Text;
                if (!bRID) { hCR.RowStatus = ComBase.Mvc.RowStatus.Insert; }
            }
            else if (Spd == ssNOISE)
            {
                hCR.WRTNO               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WRTNO].Text.To<long>(0);
                hCR.GUBUN               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.GUBUN].Text;
                hCR.SEQNO               = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.SEQNO].Text.To<long>(0);
                hCR.DEPT_NM             = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.DEPT_NM].Text;
                hCR.PROCS_CD            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.PROCS_CD].Text;
                hCR.PROCS_NM            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.PROCS_NM].Text;
                hCR.UNIT_WRKRUM_NM      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.UNIT_WRKRUM_NM].Text;
                hCR.CHMCLS_CD           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.CHMCLS_CD].Text;
                hCR.UCODE_GROUP_CD      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.UCODE_GROUP_CD].Value.To<string>("N");
                hCR.UCODE_GROUP_SEQ     = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.UCODE_GROUP_SEQ].Text.To<long>(0);
                hCR.CHMCLS_NM           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.CHMCLS_NM].Text;
                hCR.LABRR_CD            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.LABRR_CD].Text;
                hCR.OPERT_CN            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.OPERT_CN].Text;
                hCR.LABOR_CND           = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.LABOR_CND].Text;
                hCR.LABOR_TIME          = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.LABOR_TIME].Text.To<long>(0);
                hCR.UCODE_EXPSR_CYCLE   = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.UCODE_EXPSR_CYCLE].Value.To<string>();
                hCR.UCODE_EXPSR_TIME    = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.UCODE_EXPSR_TIME].Text.To<long>(0);
                hCR.WEM_LC              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_LC].Text;
                hCR.LABRR_NM            = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.LABRR_NM].Text;
                hCR.WEM_TIME_FROM       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_TIME_FROM].Text;
                hCR.WEM_TIME_TO         = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_TIME_TO].Text;
                hCR.WEM_CO              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_CO].Text.To<long>(0);
                hCR.WEM_VALUE_AVRG_ETC  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG_ETC].Text;
                hCR.WEM_VALUE_AVRG      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_AVRG].Text.To<decimal>();
                hCR.WEM_VALUE_PREV_ETC  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV_ETC].Text;
                hCR.WEM_VALUE_PREV      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_PREV].Text.To<decimal>();
                hCR.WEM_VALUE_NOW_ETC   = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW_ETC].Text;
                hCR.WEM_VALUE_NOW       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_VALUE_NOW].Text.To<decimal>();
                hCR.EXPSR_STDR_default  = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_default].Text;
                hCR.EXPSR_STDR_VALUE    = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_VALUE].Text;
                hCR.EXPSR_STDR_SE       = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_SE].Value.To<string>();
                hCR.EXPSR_STDR_UNIT     = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.EXPSR_STDR_UNIT].Value.To<string>();
                hCR.WEN_EVL_RESULT      = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEN_EVL_RESULT].Value.To<string>();
                hCR.ANALS_MTH_CD        = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.ANALS_MTH_CD].Text;
                hCR.WEM_MTH_NM          = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.WEM_MTH_NM].Text;
                hCR.ANALS_MTH_NM        = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.ANALS_MTH_NM].Text;
                hCR.REMARK              = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.REMARK].Text;
                hCR.RID                 = Spd.ActiveSheet.Cells[nRow, (int)clsHcSpd.enmHcWemRes_Noise.RID].Text;
                if (!bRID) { hCR.RowStatus = ComBase.Mvc.RowStatus.Insert; }
            }

            return hCR;
        }

        private void Data_Save()
        {
            try
            {
                if (FnWRTNO == 0) { return; }

                //SeqNo 정리
                Set_Spread_Seqno_Sorting(ssUCD, 3);
                Set_Spread_Seqno_Sorting(ssNOISE, 3);

                List<HIC_CHUKDTL_RESULT> lstUCD = new List<HIC_CHUKDTL_RESULT>();
                List<HIC_CHUKDTL_RESULT> lstNOISE = new List<HIC_CHUKDTL_RESULT>();

                //측정대상 공정 및 유해인자별 측정계획
                if (FbNew)
                {
                    //소음제외
                    for (int i = 0; i < ssUCD.ActiveSheet.RowCount; i++)
                    {
                        HIC_CHUKDTL_RESULT sss = ssUCD.GetRowData(i) as HIC_CHUKDTL_RESULT;
                        sss.GUBUN = "1";
                        lstUCD.Add(sss);
                    }

                    //소음
                    for (int i = 0; i < ssNOISE.ActiveSheet.RowCount; i++)
                    {
                        HIC_CHUKDTL_RESULT sss = ssNOISE.GetRowData(i) as HIC_CHUKDTL_RESULT;
                        sss.GUBUN = "2";
                        lstNOISE.Add(sss);
                    }
                }
                else
                {
                    lstUCD = ssUCD.GetEditbleData<HIC_CHUKDTL_RESULT>();
                    lstNOISE = ssNOISE.GetEditbleData<HIC_CHUKDTL_RESULT>();
                }

                if (lstUCD.Count > 0 || lstNOISE.Count > 0)
                {
                    if (lstUCD.Count > 0)
                    {
                        if (!hicChukDtlResultService.Save(lstUCD, FnWRTNO))
                        {
                            MessageBox.Show("측정결과입력(소음제외) 등록중 오류가 발생하였습니다. ");
                            return;
                        }
                    }

                    if (lstNOISE.Count > 0)
                    {
                        if (!hicChukDtlResultService.Save(lstNOISE, FnWRTNO))
                        {
                            MessageBox.Show("측정결과입력(소음) 등록중 오류가 발생하였습니다. ");
                            return;
                        }
                    }

                    MessageBox.Show("저장완료. ");
                    Screen_Clear();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("저장실패. ", "오류");
                return;
            }
        }

        private void Display_List()
        {
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strKeyward = txtKeyWord.Text.Trim();

            List<HIC_CHUKMST_NEW> lstHCMN = hicChukMstNewService.GetListEstimateByResultSTS(strFDate, strTDate, strKeyward);

            SSList.DataSource = lstHCMN;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            
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
    }
}
