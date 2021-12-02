
//TODO : 연도별 검사코드 조회 기능 추가필요 GjYear
//       HIC_EXCODE HISTORY 기록 필요

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcExCode.cs
/// Description     : 건진센터 검사코드 관리
/// Author          : 김민철
/// Create Date     : 2018-08-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmExamCode(HcCode03.frm)" />
namespace ComHpcLibB
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Service;
    using ComLibB;
    using FarPoint.Win.Spread;
    using FarPoint.Win.Spread.CellType;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public partial class frmHcExCode : BaseForm
    {
        HicCodeService          hcCodeservice   = null;
        HicExcodeService        hcExCodeservice = null;
        ExamMasterService       examMasterService = null;
        BasSunService           basSunService = null;
        HicExcodeReferService   hicExcodeReferService = null;
        clsSpread cSpd = null;

        string  FstrKey     = "";
        string  FstrRowid   = "";

        bool    FbDel       = false;
        bool    FbSend      = false;
        bool    FbSpc       = false;

        public frmHcExCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnDelete.Click     += new EventHandler(eBtnClick);
            this.btnHelp1.Click      += new EventHandler(eBtnClick);
            this.btnSugaHelp.Click   += new EventHandler(eBtnClick);
            this.btnAmt.Click        += new EventHandler(eBtnClick);
            this.txtCode.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtExCode.KeyPress  += new KeyPressEventHandler(eKeyPress);
            this.txtSuCode.KeyPress  += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtCode && e.KeyChar == (char)13)
            {
                if (!string.IsNullOrEmpty(txtCode.Text.Trim()))
                {
                    HIC_EXCODE item2 = hcExCodeservice.FindOne(txtCode.Text.Trim());
                    
                    if (item2.IsNullOrEmpty())
                    {
                        Screen_Clear();
                        MessageBox.Show("조회 된 Data가 없음");
                        return;
                    }

                    item2.CODE = txtCode.Text.Trim();

                    Display_Item(item2);
                }
            }
            else if (sender == txtExCode && e.KeyChar == (char)13)
            {
                if (txtExCode.Text.Trim() != "")
                {
                    EXAM_MASTER list = examMasterService.FindExamName(txtExCode.Text.Trim());
                    if (list != null)
                    {
                        lblExName.Text = list.EXAMFNAME;
                    }
                    else
                    {
                        lblExName.Text = "";
                    }
                }
            }
            else if (sender == txtSuCode && e.KeyChar == (char)13)
            {
                if (txtSuCode.Text.Trim() != "")
                {
                    BAS_SUN list2 = basSunService.FindSugaName(txtSuCode.Text.Trim());
                    if (list2 != null)
                    {
                        lblSuCode.Text = list2.SUNAMEK.Trim();
                    }
                    else
                    {
                        lblSuCode.Text = "";
                    }
                }
            }
        }

        void SetControl()
        {
            hcExCodeservice = new HicExcodeService();
            hcCodeservice = new HicCodeService();
            examMasterService = new ExamMasterService();
            basSunService = new BasSunService();
            hicExcodeReferService = new HicExcodeReferService();
            cSpd = new clsSpread();

            #region SS1 List Spread Set
            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS1.AddColumn("순번",             nameof(HIC_EXCODE.EXSORT),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("Act",              nameof(HIC_EXCODE.SORTA),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("검사코드",         nameof(HIC_EXCODE.CODE),          64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("결과",             nameof(HIC_EXCODE.STRRESTYPE),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("검사항목 한글명",  nameof(HIC_EXCODE.HNAME),        280, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("검사항목 영문명",  nameof(HIC_EXCODE.ENAME),        140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("공단코드",         nameof(HIC_EXCODE.GCODE),         62, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("수가코드",         nameof(HIC_EXCODE.SUCODE),        62, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("전송",             nameof(HIC_EXCODE.GBAUTOSEND),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("선택",             nameof(HIC_EXCODE.GBCODEUSE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("종검수가",         nameof(HIC_EXCODE.AMT1),          72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("공단수가",         nameof(HIC_EXCODE.AMT2),          72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("특검수가",         nameof(HIC_EXCODE.AMT3),          72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("조정수가",         nameof(HIC_EXCODE.AMT4),          72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("임의수가",         nameof(HIC_EXCODE.AMT5),          72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종전종검수가",     nameof(HIC_EXCODE.OLDAMT1),       72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종전공단수가",     nameof(HIC_EXCODE.OLDAMT2),       72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종전특검수가",     nameof(HIC_EXCODE.OLDAMT3),       72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종전조정수가",     nameof(HIC_EXCODE.OLDAMT4),       72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종전임의수가",     nameof(HIC_EXCODE.OLDAMT5),       72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("촬영명",           nameof(HIC_EXCODE.XNAME),         78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("오더코드",         nameof(HIC_EXCODE.XORDERCODE),    78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("삭제일자",         nameof(HIC_EXCODE.DELDATE),       64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("결과단위",         nameof(HIC_EXCODE.UNIT),          42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("결과타입",         nameof(HIC_EXCODE.RESULTTYPE),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("결과타입2",        nameof(HIC_EXCODE.RESULTTYPE2),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("결과코드",         nameof(HIC_EXCODE.RESCODE),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("결과코드2",        nameof(HIC_EXCODE.RESCODE2),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남)From",         nameof(HIC_EXCODE.MIN_M),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남B)From",        nameof(HIC_EXCODE.MIN_MB),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남R)From",        nameof(HIC_EXCODE.MIN_MR),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남)To",           nameof(HIC_EXCODE.MAX_M),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남B)To",          nameof(HIC_EXCODE.MAX_MB),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(남R)To",          nameof(HIC_EXCODE.MAX_MR),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여)From",         nameof(HIC_EXCODE.MIN_F),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여B)From",        nameof(HIC_EXCODE.MIN_FB),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여R)From",        nameof(HIC_EXCODE.MIN_FR),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여)To",           nameof(HIC_EXCODE.MAX_F),         42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여B)To",          nameof(HIC_EXCODE.MAX_FB),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("(여R)To",          nameof(HIC_EXCODE.MAX_FR),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("종검수가관리여부", nameof(HIC_EXCODE.CHKSUGA1),      72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("공단수가관리여부", nameof(HIC_EXCODE.CHKSUGA2),      72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("특검수가관리여부", nameof(HIC_EXCODE.CHKSUGA3),      72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("조정수가관리여부", nameof(HIC_EXCODE.CHKSUGA4),      72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("임의수가관리여부", nameof(HIC_EXCODE.CHKSUGA5),      72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("ROWID",            nameof(HIC_EXCODE.RID),           42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            //TODO : HIC_EXCODE_REFER 테이블을 이용하여 기간별 참고치 관리해야함
            #region ssMaxMin Spread Set New
            //ssMaxMin1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            //ssMaxMin1.AddColumn("구분",        nameof(HIC_EXCODE_REFER.GUBUN),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            //ssMaxMin1.AddColumn("구분명",      nameof(HIC_EXCODE_REFER.GUBUNNM),  64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ////ssMaxMin1.AddColumn("적용일자",    nameof(HIC_EXCODE_REFER.FROMDATE), 64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남)From",    nameof(HIC_EXCODE_REFER.MIN_M),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남)To",      nameof(HIC_EXCODE_REFER.MAX_M),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남B)From",   nameof(HIC_EXCODE_REFER.MIN_MB),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남B)To",     nameof(HIC_EXCODE_REFER.MAX_MB),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남R)From",   nameof(HIC_EXCODE_REFER.MIN_MR),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(남R)To",     nameof(HIC_EXCODE_REFER.MAX_MR),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여)From",    nameof(HIC_EXCODE_REFER.MIN_F),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여)To",      nameof(HIC_EXCODE_REFER.MAX_F),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여B)From",   nameof(HIC_EXCODE_REFER.MIN_FB),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여B)To",     nameof(HIC_EXCODE_REFER.MAX_FB),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여R)From",   nameof(HIC_EXCODE_REFER.MIN_FR),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            //ssMaxMin1.AddColumn("(여R)To",     nameof(HIC_EXCODE_REFER.MAX_FR),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            #endregion

            #region ssMaxMin Spread Set
            ssMaxMin.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            ssMaxMin.AddColumn("구분",        "",                          64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { });
            ssMaxMin.AddColumn("(남)From",    nameof(HIC_EXCODE.MIN_M),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { });
            ssMaxMin.AddColumn("(남)To",      nameof(HIC_EXCODE.MAX_M),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { });
            ssMaxMin.AddColumn("(여)From",    nameof(HIC_EXCODE.MIN_F),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { });
            ssMaxMin.AddColumn("(여)To",      nameof(HIC_EXCODE.MAX_F),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { });
            #endregion

            //TODO : 신규구분일자 추가시 로직 필요함
            #region ssAmt Spread Set
            ssAmt.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            ssAmt.AddColumn("구분",       "", 70, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ssAmt.AddColumn("현재수가",   "", 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ssAmt.AddColumn("변경수가1",  "", 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ssAmt.AddColumn("변경수가2",  "", 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ssAmt.AddColumn("변경수가3",  "", 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            ssAmt.AddColumn("변경수가4",  "", 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            #endregion

            #region SS10 Spread Set
            SS10.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS10.AddColumn("수정일자",     nameof(HIC_EXCODE.ENTDATE), 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS10.AddColumn("작업자",       nameof(HIC_EXCODE.JOBNAME),  72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS10.AddColumn("작업자사번",   nameof(HIC_EXCODE.ENTSABUN), 44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region Form Controls Set
            chkResType.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.GBCODEUSE), CheckValue = "Y", UnCheckValue = "N" });
            chkAuto.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.GBAUTOSEND), CheckValue = "Y", UnCheckValue = "N" });
            rdoResType1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE), CheckValue = "1" });
            rdoResType2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE), CheckValue = "2" });
            rdoResType3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE), CheckValue = "3" });
            rdoResType4.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE), CheckValue = "4" });
            rdoResType5.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE2), CheckValue = "1" });
            rdoResType6.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE2), CheckValue = "2" });
            rdoResType7.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE2), CheckValue = "3" });
            rdoResType8.SetOptions(new RadioButtonOption { DataField = nameof(HIC_EXCODE.RESULTTYPE2), CheckValue = "4" });


            List<HIC_CODE> list = hcCodeservice.FindOne("17");
            cboResCode1.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboResCode2.SetItems(list, "NAME", "CODE");

            List<HIC_CODE> list1 = hcCodeservice.FindOne("22");
            cboTongBun.SetItems(list1, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            List<HIC_CODE> list2 = hcCodeservice.FindOne("72");
            cboHicPart1.SetItems(list2, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboHicPart2.SetItems(list2, "NAME", "CODE");   //조회용

            List<HIC_CODE> list3 = hcCodeservice.FindOne("A9");
            cboHeaPart1.SetItems(list3, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboHeaPart2.SetItems(list3, "NAME", "CODE");   //조회용

            List<HIC_CODE> list4 = hcCodeservice.FindOne("A7");
            cboGoTo.SetItems(list4, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            dtpDelDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_EXCODE.DELDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });

            cboHaRoom.Items.Clear();
            cboHaRoom.Items.Add("");
            for (int i = 0; i < 12; i++)
            {
                cboHaRoom.Items.Add((i + 1).ToString("D2"));
            }

            List<HIC_CODE> list5 = hcCodeservice.FindOne("A0");
            cboPart1.SetItems(list5, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboPart2.SetItems(list5, "NAME", "CODE");  //조회용

            List<HIC_CODE> list6 = hcCodeservice.FindOne("B6");
            cboPanBun1.SetItems(list6, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            List<HIC_CODE> list7 = hcCodeservice.FindOne("B7");
            cboPanBun2.SetItems(list7, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            List<HIC_CODE> list8 = hcCodeservice.FindOne("A8");
            cboExamBun.SetItems(list8, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            chkEndo1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOGUBUN1), CheckValue = "Y", UnCheckValue = "" });
            chkEndo2.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOGUBUN2), CheckValue = "Y", UnCheckValue = "" });
            //chkEndo3.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOGUBUN3), CheckValue = "Y", UnCheckValue = "" });
            chkEndo4.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOGUBUN4), CheckValue = "Y", UnCheckValue = "" });
            //chkEndo5.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOGUBUN5), CheckValue = "Y", UnCheckValue = "" });
            chkEndo수면.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.ENDOSCOPE), CheckValue = "Y", UnCheckValue = "" });
            chkEmpty.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.GBRESEMPTY), CheckValue = "Y", UnCheckValue = "" });
            chkPanDisp.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.PANDISP), CheckValue = "Y", UnCheckValue = "N" });
            chkUrine.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.GBULINE), CheckValue = "Y", UnCheckValue = "" });
            chkSpcExam.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.GBSPCEXAM), CheckValue = "Y", UnCheckValue = "" });

            chkSuga1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.CHKSUGA1), CheckValue = "Y", UnCheckValue = "" });
            chkSuga2.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.CHKSUGA2), CheckValue = "Y", UnCheckValue = "" });
            chkSuga3.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.CHKSUGA3), CheckValue = "Y", UnCheckValue = "" });
            chkSuga4.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.CHKSUGA4), CheckValue = "Y", UnCheckValue = "" });
            chkSuga5.SetOptions(new CheckBoxOption { DataField = nameof(HIC_EXCODE.CHKSUGA5), CheckValue = "Y", UnCheckValue = "" });
            #endregion

            txtCode.SetOptions(new TextBoxOption { DataField = nameof(HIC_EXCODE.CODE)});

            panMain.SetEnterKey();
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            HIC_EXCODE item = SS1.GetRowData(e.Row) as HIC_EXCODE;
            FstrRowid = item.RID;

            HIC_EXCODE item2 = hcExCodeservice.FindOne(item.CODE);

            Display_Item(item2);
        }

        /// <summary>
        /// 단일 Item 하나를 Display
        /// </summary>
        /// <param name="item"></param>
        private void Display_Item(HIC_EXCODE item)
        {
            lblExName.Text = "";
            lblSuCode.Text = "";

            panMain.SetData(item);

            if (txtExCode.Text.Trim() != "")
            {
                lblExName.Text = examMasterService.FindExamName(txtExCode.Text.Trim()).EXAMFNAME.To<string>("");
            }

            if (txtSuCode.Text.Trim() != "")
            {
                lblSuCode.Text = basSunService.FindSugaName(txtSuCode.Text.Trim()).SUNAMEK.To<string>("");
            }

            if (!item.HEAPART.IsNullOrEmpty())
            {
                for (int i = 0; i < cboHeaPart1.Items.Count; i++)
                {
                    cboHeaPart1.SelectedIndex = i;

                    if (VB.Pstr(cboHeaPart1.Text, ".", 1).Trim() == item.HEAPART.To<string>("").Trim())
                    {
                        break;
                    }
                }
            }
            
            #region 검사참고치
            ssMaxMin.ActiveSheet.Cells[0, 1].Text = item.MIN_M;   //정상A 남자 From
            ssMaxMin.ActiveSheet.Cells[0, 2].Text = item.MAX_M;   //정상A 남자 To
            ssMaxMin.ActiveSheet.Cells[0, 3].Text = item.MIN_F;   //정상A 여자 From
            ssMaxMin.ActiveSheet.Cells[0, 4].Text = item.MAX_F;   //정상A 여자 To

            ssMaxMin.ActiveSheet.Cells[1, 1].Text = item.MIN_MB;   //정상B 남자 From
            ssMaxMin.ActiveSheet.Cells[1, 2].Text = item.MAX_MB;   //정상B 남자 From
            ssMaxMin.ActiveSheet.Cells[1, 3].Text = item.MIN_FB;   //정상B 여자 From
            ssMaxMin.ActiveSheet.Cells[1, 4].Text = item.MAX_FB;   //정상B 여자 To

            ssMaxMin.ActiveSheet.Cells[2, 1].Text = item.MIN_MR;   //의심R 남자 From
            ssMaxMin.ActiveSheet.Cells[2, 2].Text = item.MAX_MR;   //의심R 남자 To
            ssMaxMin.ActiveSheet.Cells[2, 3].Text = item.MIN_FR;   //의심R 여자 From
            ssMaxMin.ActiveSheet.Cells[2, 4].Text = item.MAX_FR;   //의심R 여자 To
            #endregion

            #region 수가변경내역
            ssAmt.ActiveSheet.Cells[0, 1].Text = item.SUDATE;
            ssAmt.ActiveSheet.Cells[1, 1].Text = item.AMT1.ToString();
            ssAmt.ActiveSheet.Cells[2, 1].Text = item.AMT2.ToString();
            ssAmt.ActiveSheet.Cells[3, 1].Text = item.AMT3.ToString();
            ssAmt.ActiveSheet.Cells[4, 1].Text = item.AMT4.ToString();
            ssAmt.ActiveSheet.Cells[5, 1].Text = item.AMT5.ToString();
            ssAmt.ActiveSheet.Cells[0, 2].Text = item.SUDATE2;
            ssAmt.ActiveSheet.Cells[1, 2].Text = item.OLDAMT1.ToString();
            ssAmt.ActiveSheet.Cells[2, 2].Text = item.OLDAMT2.ToString();
            ssAmt.ActiveSheet.Cells[3, 2].Text = item.OLDAMT3.ToString();
            ssAmt.ActiveSheet.Cells[4, 2].Text = item.OLDAMT4.ToString();
            ssAmt.ActiveSheet.Cells[5, 2].Text = item.OLDAMT5.ToString();
            ssAmt.ActiveSheet.Cells[0, 3].Text = item.SUDATE3;
            ssAmt.ActiveSheet.Cells[1, 3].Text = item.JAMT1.ToString();
            ssAmt.ActiveSheet.Cells[2, 3].Text = item.GAMT1.ToString();
            ssAmt.ActiveSheet.Cells[3, 3].Text = item.SAMT1.ToString();
            ssAmt.ActiveSheet.Cells[4, 3].Text = item.OAMT1.ToString();
            ssAmt.ActiveSheet.Cells[5, 3].Text = item.IAMT1.ToString();
            ssAmt.ActiveSheet.Cells[0, 4].Text = item.SUDATE4;
            ssAmt.ActiveSheet.Cells[1, 4].Text = item.JAMT2.ToString();
            ssAmt.ActiveSheet.Cells[2, 4].Text = item.GAMT2.ToString();
            ssAmt.ActiveSheet.Cells[3, 4].Text = item.SAMT2.ToString();
            ssAmt.ActiveSheet.Cells[4, 4].Text = item.OAMT2.ToString();
            ssAmt.ActiveSheet.Cells[5, 4].Text = item.IAMT2.ToString();
            ssAmt.ActiveSheet.Cells[0, 5].Text = item.SUDATE5;
            ssAmt.ActiveSheet.Cells[1, 5].Text = item.JAMT3.ToString();
            ssAmt.ActiveSheet.Cells[2, 5].Text = item.GAMT3.ToString();
            ssAmt.ActiveSheet.Cells[3, 5].Text = item.SAMT3.ToString();
            ssAmt.ActiveSheet.Cells[4, 5].Text = item.OAMT3.ToString();
            ssAmt.ActiveSheet.Cells[5, 5].Text = item.IAMT3.ToString();
            #endregion

            FstrRowid = item.RID;

            btnSave.Enabled = item.DELDATE == null ? true : false;
            btnDelete.Enabled = item.DELDATE == null ? true : false;

            Display_ExCode_History(SS10, item.CODE);
        }

        /// <summary>
        /// 단일 Item 하나를 Display
        /// </summary>
        /// <param name="item"></param>
        private void Display_Item_New(HIC_EXCODE item, List<HIC_EXCODE_REFER> item2, List<HIC_EXCODE_REFER> item3)
        {
            panMain.SetData(item);
            ssMaxMin.DataSource = item2;
            ssMaxMin.DataSource = item3;

            #region 검사참고치
            //ssMaxMin.ActiveSheet.Cells[0, 1].Text = item2.MIN_M;   //정상A 남자 From
            //ssMaxMin.ActiveSheet.Cells[0, 2].Text = item2.MAX_M;   //정상A 남자 To
            //ssMaxMin.ActiveSheet.Cells[0, 3].Text = item2.MIN_F;   //정상A 여자 From
            //ssMaxMin.ActiveSheet.Cells[0, 4].Text = item2.MAX_F;   //정상A 여자 To

            //ssMaxMin.ActiveSheet.Cells[1, 1].Text = item2.MIN_MB;   //정상B 남자 From
            //ssMaxMin.ActiveSheet.Cells[1, 2].Text = item2.MAX_MB;   //정상B 남자 From
            //ssMaxMin.ActiveSheet.Cells[1, 3].Text = item2.MIN_FB;   //정상B 여자 From
            //ssMaxMin.ActiveSheet.Cells[1, 4].Text = item2.MAX_FB;   //정상B 여자 To

            //ssMaxMin.ActiveSheet.Cells[2, 1].Text = item2.MIN_MR;   //의심R 남자 From
            //ssMaxMin.ActiveSheet.Cells[2, 2].Text = item2.MAX_MR;   //의심R 남자 To
            //ssMaxMin.ActiveSheet.Cells[2, 3].Text = item2.MIN_FR;   //의심R 여자 From
            //ssMaxMin.ActiveSheet.Cells[2, 4].Text = item2.MAX_FR;   //의심R 여자 To
            #endregion

            #region 수가변경내역
            //ssAmt.ActiveSheet.Cells[0, 1].Text = item.SUDATE == null ? "" : item.SUDATE?.ToString("yyyy-MM-dd");
            //ssAmt.ActiveSheet.Cells[1, 1].Text = item.GAMT1.ToString();
            //ssAmt.ActiveSheet.Cells[2, 1].Text = item.SAMT1.ToString();
            //ssAmt.ActiveSheet.Cells[3, 1].Text = item.JAMT1.ToString();
            //ssAmt.ActiveSheet.Cells[4, 1].Text = item.IAMT1.ToString();
            //ssAmt.ActiveSheet.Cells[0, 2].Text = item.SUDATE2 == null ? "" : item.SUDATE2?.ToString("yyyy-MM-dd");
            //ssAmt.ActiveSheet.Cells[1, 2].Text = item.GAMT2.ToString();
            //ssAmt.ActiveSheet.Cells[2, 2].Text = item.SAMT2.ToString();
            //ssAmt.ActiveSheet.Cells[3, 2].Text = item.JAMT2.ToString();
            //ssAmt.ActiveSheet.Cells[4, 2].Text = item.IAMT2.ToString();
            //ssAmt.ActiveSheet.Cells[0, 3].Text = item.SUDATE3 == null ? "" : item.SUDATE3?.ToString("yyyy-MM-dd");
            //ssAmt.ActiveSheet.Cells[1, 3].Text = item.GAMT3.ToString();
            //ssAmt.ActiveSheet.Cells[2, 3].Text = item.SAMT3.ToString();
            //ssAmt.ActiveSheet.Cells[3, 3].Text = item.JAMT3.ToString();
            //ssAmt.ActiveSheet.Cells[4, 3].Text = item.IAMT3.ToString();
            //ssAmt.ActiveSheet.Cells[0, 4].Text = item.SUDATE4 == null ? "" : item.SUDATE4?.ToString("yyyy-MM-dd");
            //ssAmt.ActiveSheet.Cells[1, 4].Text = item.GAMT4.ToString();
            //ssAmt.ActiveSheet.Cells[2, 4].Text = item.SAMT4.ToString();
            //ssAmt.ActiveSheet.Cells[3, 4].Text = item.JAMT4.ToString();
            //ssAmt.ActiveSheet.Cells[4, 4].Text = item.IAMT4.ToString();
            //ssAmt.ActiveSheet.Cells[0, 5].Text = item.SUDATE5 == null ? "" : item.SUDATE5?.ToString("yyyy-MM-dd");
            //ssAmt.ActiveSheet.Cells[1, 5].Text = item.GAMT5.ToString();
            //ssAmt.ActiveSheet.Cells[2, 5].Text = item.SAMT5.ToString();
            //ssAmt.ActiveSheet.Cells[3, 5].Text = item.JAMT5.ToString();
            //ssAmt.ActiveSheet.Cells[4, 5].Text = item.IAMT5.ToString();
            #endregion

            btnSave.Enabled = item.DELDATE == null ? true : false;
            btnDelete.Enabled = item.DELDATE == null ? true : false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                clsSpread cSpd = new clsSpread();
                cSpd.Spread_All_Clear(SS1);

                Search_List();

                cSpd.Dispose();

            }
            else if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSave)
            {
                HIC_EXCODE item = panMain.GetData<HIC_EXCODE>();

                if (!panMain.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                #region GetData에서 가져오지 못한 컨트롤 다시 읽음
                item.SUDATE  = VB.Left(ssAmt.ActiveSheet.Cells[0, 1].Text, 10); //적용일자1
                item.SUDATE2 = VB.Left(ssAmt.ActiveSheet.Cells[0, 2].Text, 10); //적용일자2
                item.SUDATE3 = VB.Left(ssAmt.ActiveSheet.Cells[0, 3].Text, 10); //적용일자3
                item.SUDATE4 = VB.Left(ssAmt.ActiveSheet.Cells[0, 4].Text, 10); //적용일자4
                item.SUDATE5 = VB.Left(ssAmt.ActiveSheet.Cells[0, 5].Text, 10); //적용일자5
                
                long[] nJAmt = new long[5];
                long[] nGAmt = new long[5];
                long[] nSAmt = new long[5];
                long[] nOAmt = new long[5];
                long[] nIAmt = new long[5];

                for (int i = 0; i < 5; i++)
                {
                    nJAmt[i] = Convert.ToInt32(ssAmt.ActiveSheet.Cells[1, i + 1].Value);
                    nGAmt[i] = Convert.ToInt32(ssAmt.ActiveSheet.Cells[2, i + 1].Value);
                    nSAmt[i] = Convert.ToInt32(ssAmt.ActiveSheet.Cells[3, i + 1].Value);
                    nOAmt[i] = Convert.ToInt32(ssAmt.ActiveSheet.Cells[4, i + 1].Value);
                    nIAmt[i] = Convert.ToInt32(ssAmt.ActiveSheet.Cells[5, i + 1].Value);
                }

                item.AMT1 = nJAmt[0];       //종검수가
                item.AMT2 = nGAmt[0];       //공단수가
                item.AMT3 = nSAmt[0];       //특검수가
                item.AMT4 = nOAmt[0];       //조정수가
                item.AMT5 = nIAmt[0];       //임의수가

                item.OLDAMT1 = nJAmt[1];    //종검수가
                item.OLDAMT2 = nGAmt[1];    //공단수가
                item.OLDAMT3 = nSAmt[1];    //특검수가
                item.OLDAMT4 = nOAmt[1];    //조정수가
                item.OLDAMT5 = nIAmt[1];    //임의수가

                item.GAMT1 = nJAmt[2];      //종검수가
                item.SAMT1 = nGAmt[2];      //공단수가
                item.JAMT1 = nSAmt[2];      //특검수가
                item.IAMT1 = nOAmt[2];      //조정수가
                item.OAMT1 = nIAmt[2];      //임의수가

                item.GAMT2 = nJAmt[3];      //종검수가
                item.SAMT2 = nGAmt[3];      //공단수가
                item.JAMT2 = nSAmt[3];      //특검수가
                item.IAMT2 = nOAmt[3];      //조정수가
                item.OAMT2 = nIAmt[3];      //임의수가

                item.GAMT3 = nJAmt[4];      //종검수가
                item.SAMT3 = nGAmt[4];      //공단수가
                item.JAMT3 = nSAmt[4];      //특검수가
                item.IAMT3 = nOAmt[4];      //조정수가
                item.OAMT3 = nIAmt[4];      //임의수가

                item.MIN_M = ssMaxMin.ActiveSheet.Cells[0, 1].Text;
                item.MAX_M = ssMaxMin.ActiveSheet.Cells[0, 2].Text;
                item.MIN_F = ssMaxMin.ActiveSheet.Cells[0, 3].Text;
                item.MAX_F = ssMaxMin.ActiveSheet.Cells[0, 4].Text;

                item.MIN_MB = ssMaxMin.ActiveSheet.Cells[1, 1].Text;
                item.MAX_MB = ssMaxMin.ActiveSheet.Cells[1, 2].Text;
                item.MIN_FB = ssMaxMin.ActiveSheet.Cells[1, 3].Text;
                item.MAX_FB = ssMaxMin.ActiveSheet.Cells[1, 4].Text;
                
                item.MIN_MR = ssMaxMin.ActiveSheet.Cells[2, 1].Text;
                item.MAX_MR = ssMaxMin.ActiveSheet.Cells[2, 2].Text;
                item.MIN_FR = ssMaxMin.ActiveSheet.Cells[2, 3].Text;
                item.MAX_FR = ssMaxMin.ActiveSheet.Cells[2, 4].Text;

                item.ENTDATE = DateTime.Now;
                item.ENTSABUN = Convert.ToInt32(clsType.User.IdNumber);

                #endregion

                if (!FstrRowid.IsNullOrEmpty())
                {
                    if (!hcExCodeservice.SelectInsertHicExCodeByRid(FstrRowid))
                    {
                        MessageBox.Show("History 저장 실패!");
                    }
                }

                #region 검사코드 세팅 저장
                if (item.IsNullOrEmpty())
                {
                    int result = hcExCodeservice.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("저장실패!");
                        return;
                    }
                }
                else
                {
                    int result = hcExCodeservice.Update(item);

                    if (result < 0)
                    {
                        MessageBox.Show("저장실패!");
                        return;
                    }
                }
                #endregion

                #region 검사 참고치 세팅 저장 New
                ////검사 참고치 설정값 저장(일반)
                //IList<HIC_EXCODE_REFER> list = ssMaxMin.GetEditbleData<HIC_EXCODE_REFER>();
                //if (list.Count > 0)
                //{
                //    if (hicExcodeReferService.SaveData(item.CODE, list))
                //    {
                //        MessageBox.Show("저장성공!");
                //        panMain.Initialize();
                //        Screen_Clear();
                //    }
                //    else
                //    {
                //        MessageBox.Show("트랜잭션 오류로 저장 할 수 없습니다");
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("저장할 코드가 없습니다");
                //}

                ////검사 참고치 설정값 저장(종검)
                //IList<HIC_EXCODE_REFER> list2 = ssMaxMin.GetEditbleData<HIC_EXCODE_REFER>();
                //if (list.Count > 0)
                //{
                //    if (hicExcodeReferService.SaveData(item.CODE, list))
                //    {
                //        MessageBox.Show("저장성공!");
                //        panMain.Initialize();
                //        Screen_Clear();
                //    }
                //    else
                //    {
                //        MessageBox.Show("트랜잭션 오류로 저장 할 수 없습니다");
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("저장할 코드가 없습니다");
                //}
                #endregion

                MessageBox.Show("저장완료!");
                Screen_Clear();
                return;

            }
            //코드삭제시 완전히 삭제하지 않음. DelDate 날짜를 기록함 UpDate() 로직이용
            else if (sender == btnDelete)
            {
                HIC_EXCODE item = panMain.GetData<HIC_EXCODE>();

                if (item.RID.IsNullOrEmpty())
                {
                    MessageBox.Show("검사항목을 선택하십시오.");
                    return;
                }

                if (ComFunc.MsgBoxQ("검사항목을 삭제하시겠습니까?", "작업확인",  MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                int result = hcExCodeservice.Delete(item);

                if (result > 0)
                {
                    MessageBox.Show("삭제 하였습니다.");
                    panMain.Initialize();
                    Screen_Clear();
                    return;
                }
            }
            else if (sender == btnHelp1)
            {
                new frmHcExamHelp().ShowDialog();
                if (clsPublic.GstrRetValue != "")
                {
                    txtExCode.Text = clsPublic.GstrRetValue;
                    EXAM_MASTER list = examMasterService.FindExamName(clsPublic.GstrRetValue);
                    if (list != null)
                    {
                        lblExName.Text = list.EXAMFNAME;
                    }
                    else
                    {
                        lblExName.Text = "";
                    }
                    

                    return;
                }
            }
            else if (sender == btnSugaHelp)
            {
                new frmSugaHelp().ShowDialog();
                if (clsPublic.GstrRetValue != "")
                {
                    txtSuCode.Text = clsPublic.GstrRetValue;
                    BAS_SUN list2 = basSunService.FindSugaName(clsPublic.GstrRetValue);
                    if (list2 != null)
                    {
                        lblSuCode.Text = list2.SUNAMEK.Trim();
                    }
                    else
                    {
                        lblSuCode.Text = "";
                    }
                    

                    return;
                }
            }
            else if (sender == btnAmt)
            {
                ssAmt.ActiveSheet.AddColumns(1, 1);
                ssAmt.ActiveSheet.ColumnCount = 6;
                ssAmt.ActiveSheet.Columns.Get(1).Label = "현재수가";
                ssAmt.ActiveSheet.Columns.Get(1).Width = 74;
                ssAmt.ActiveSheet.Columns.Get(2).Label = "변경수가1";
                ssAmt.ActiveSheet.Columns.Get(2).Width = 74;
                ssAmt.ActiveSheet.Columns.Get(3).Label = "변경수가2";
                ssAmt.ActiveSheet.Columns.Get(3).Width = 74;
                ssAmt.ActiveSheet.Columns.Get(4).Label = "변경수가3";
                ssAmt.ActiveSheet.Columns.Get(4).Width = 74;
                ssAmt.ActiveSheet.Columns.Get(5).Label = "변경수가4";
                ssAmt.ActiveSheet.Columns.Get(5).Width = 74;

                DateTimeCellType spdObj = new DateTimeCellType();
                spdObj.DateTimeFormat = DateTimeFormat.ShortDate;

                ssAmt.ActiveSheet.Cells[0, 1].CellType = spdObj;
                ssAmt.ActiveSheet.Cells[0, 1].Text = DateTime.Now.ToShortDateString();
                ssAmt.ActiveSheet.Cells[1, 1].Text = "0";
                ssAmt.ActiveSheet.Cells[2, 1].Text = "0";
                ssAmt.ActiveSheet.Cells[3, 1].Text = "0";
                ssAmt.ActiveSheet.Cells[4, 1].Text = "0";
                ssAmt.ActiveSheet.Cells[5, 1].Text = "0";

                NumberCellType spdObj2 = new NumberCellType();
                spdObj2.DecimalPlaces = 0;
                spdObj2.MinimumValue = -999999999999D;
                spdObj2.MaximumValue = 999999999999D;
                spdObj2.DecimalSeparator = ".";
                spdObj2.Separator = ",";

                for (int i = 1; i < ssAmt.ActiveSheet.RowCount; i++)
                {
                    ssAmt.ActiveSheet.Cells[i, 1].CellType = spdObj2;
                    ssAmt.ActiveSheet.Cells[i, 1].Renderer = spdObj2;
                    ssAmt.ActiveSheet.Cells[i, 1].HorizontalAlignment = CellHorizontalAlignment.Right;
                    ssAmt.ActiveSheet.Cells[i, 1].VerticalAlignment = CellVerticalAlignment.Center;
                }
            }
        }

        void Search_List()
        {
            Cursor.Current = Cursors.WaitCursor;

            FstrKey = txtView.Text.Trim();
            FbDel = chkDel.Checked == true ? true : false;
            FbSend = chkSend.Checked == true ? true : false;
            FbSpc = chkSpc.Checked == true ? true : false;

            tShowSpread();

            Cursor.Current = Cursors.Default;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Clear();

            #region Control ErrorProvider

            panMain.AddRequiredControl(txtCode);
            panMain.AddRequiredControl(txtHName);

            #endregion
        }

        void Screen_Clear()
        {
            ComFunc.SetAllControlClearEx(this.panMain);

            ssMaxMin.ActiveSheet.RowCount = 3;
            ssMaxMin.ActiveSheet.Cells[0, 0].Text = "정상A";
            ssMaxMin.ActiveSheet.Cells[1, 0].Text = "정상B";
            ssMaxMin.ActiveSheet.Cells[2, 0].Text = "의심R";

            ssAmt.ActiveSheet.RowCount = 6;
            ssAmt.ActiveSheet.Cells[0, 0].Text = "적용일자";
            ssAmt.ActiveSheet.Cells[1, 0].Text = "종검수가";
            ssAmt.ActiveSheet.Cells[2, 0].Text = "공단수가";
            ssAmt.ActiveSheet.Cells[3, 0].Text = "특검수가";
            ssAmt.ActiveSheet.Cells[4, 0].Text = "조정수가";
            ssAmt.ActiveSheet.Cells[5, 0].Text = "임의수가";

            DateTimeCellType spdObj = new DateTimeCellType();
            spdObj.DateTimeFormat = DateTimeFormat.ShortDate;

            ssAmt.ActiveSheet.Cells[0, 1].CellType = spdObj;
            ssAmt.ActiveSheet.Cells[0, 2].CellType = spdObj;
            ssAmt.ActiveSheet.Cells[0, 3].CellType = spdObj;
            ssAmt.ActiveSheet.Cells[0, 4].CellType = spdObj;
            ssAmt.ActiveSheet.Cells[0, 5].CellType = spdObj;

            NumberCellType spdObj2 = new NumberCellType();
            spdObj2.DecimalPlaces = 0;
            spdObj2.MinimumValue = -999999999999D;
            spdObj2.MaximumValue = 999999999999D;
            spdObj2.DecimalSeparator = ".";
            spdObj2.Separator = ",";

            for (int i = 1; i < ssAmt.ActiveSheet.RowCount; i++)
            {
                for (int j = 1; j < ssAmt.ActiveSheet.ColumnCount; j++)
                {
                    ssAmt.ActiveSheet.Cells[i, j].CellType = spdObj2;
                    ssAmt.ActiveSheet.Cells[i, j].Renderer = spdObj2;
                    ssAmt.ActiveSheet.Cells[i, j].HorizontalAlignment = CellHorizontalAlignment.Right;
                    ssAmt.ActiveSheet.Cells[i, j].VerticalAlignment = CellVerticalAlignment.Center;
                }
            }
            
            dtpDelDate.Checked = false;
            dtpDelDate.CustomFormat = " ";

            lblExName.Text = "";
            lblSuCode.Text = "";
            
            txtView.Text = "";

            FstrRowid = "";
        }
        
        void tShowSpread()
        {
            SS1.SetDataSource(hcExCodeservice.FindAll(FbDel, FbSpc, FbSend, FstrKey));
        }
     
        void Display_ExCode_History(FpSpread argSS, string argCode)
        {
            cSpd.Spread_All_Clear(argSS);

            List<HIC_EXCODE> list = hcExCodeservice.GetHistoryByCode(argCode);

            SS10.SetDataSource(list);
        }
    }
}
