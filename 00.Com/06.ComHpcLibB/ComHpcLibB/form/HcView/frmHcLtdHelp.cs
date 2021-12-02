using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcLtdHelp.cs
/// Description     : 사업장 코드 찾기
/// Author          : 김민철
/// Create Date     : 2019-08-09
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcLtd00.frm(FrmLtdHelp)" />
namespace ComHpcLibB
{
    public partial class frmHcLtdHelp : BaseForm
    {
        HicLtdService hicLtdService = new HicLtdService();
        ComHpcLibBService comHpcLibBService = null;

        public delegate void SetGstrValue(HIC_LTD GstrValue);
        public event SetGstrValue rSetGstrValue;

        private string FstrFindKey = string.Empty;
        private string FstrChkGbn = string.Empty;

        public frmHcLtdHelp()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHcLtdHelp(string argFindKey, string argChk = "")
        {
            InitializeComponent();
            //SetControl();
            FstrFindKey = argFindKey;
            FstrChkGbn = argChk;

            if (!argChk.IsNullOrEmpty())
            {
                SetControl_Chk();
            }
            else
            {
                SetControl();
            }

            SetEvents();
        }

        private void SetEvents()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.txtSearch.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.ssChkList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            grpChkDtl.Initialize();

            panBottom.Visible = false;
            //전회치 사용여부 임시로 막음
            ////if (FstrChkGbn.IsNullOrEmpty())
            ////{
            ////    panBottom.Visible = false;                
            ////}

            if (!FstrFindKey.IsNullOrEmpty())
            {
                txtSearch.Text = FstrFindKey;
                Search_Data(txtSearch.Text.Trim(), FstrChkGbn);
            }

            this.ActiveControl = txtSearch;

            txtSearch.SelectAll();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                if (!txtSearch.Text.Trim().IsNullOrEmpty()) { Search_Data(txtSearch.Text.Trim(), FstrChkGbn); }
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                HIC_LTD item = SSList.GetCurrentRowData() as HIC_LTD;

                rSetGstrValue(item);
                this.Close();

                //임시로 막음
                //if (FstrChkGbn.IsNullOrEmpty())
                //{
                //    rSetGstrValue(item);
                //    this.Close();
                //}
                //else
                //{
                //    ssChkList.DataSource = null;

                //    List<COMHPC> cHPC = comHpcLibBService.GetListChukMstByLtdCode(item.CODE);

                //    if (!cHPC.IsNullOrEmpty())
                //    {
                //        if (cHPC.Count == 0)
                //        {
                //            rSetGstrValue(item);
                //            this.Close();
                //        }
                //        else
                //        {
                //            ssChkList.SetDataSource(cHPC);
                //        }
                //    }
                //    else
                //    {
                //        rSetGstrValue(item);
                //        this.Close();
                //    }
                //}
            }
            else if (sender == ssChkList)
            {
                
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!txtSearch.Text.Trim().IsNullOrEmpty()) { Search_Data(txtSearch.Text.Trim(), FstrChkGbn); }
            }
        }

        private void Search_Data(string argFindKey, string argGbChk)
        {
            if (!argGbChk.IsNullOrEmpty())
            {
                //작업환경측정용 사업장 검색
                Display_ChukLtd_List(argFindKey);
            }
            else
            {
                Display_Ltd_List(argFindKey);
            }
            
        }

        /// <summary>
        /// 작업환경측정용 사업장 검색
        /// </summary>
        /// <param name="argFindKey"></param>
        private void Display_ChukLtd_List(string argFindKey)
        {
            string strGubun = "NAME";

            if (rdoGB2.Checked)
            {
                strGubun = "CODE";
            }
            else if (rdoGB3.Checked)
            {
                strGubun = "DAEPYO";
            }

            SSList.DataSource = hicLtdService.ViewChukLtd(argFindKey, strGubun);
        }

        /// <summary>
        /// 일반 사업장 검색
        /// </summary>
        /// <param name="argFindKey"></param>
        private void Display_Ltd_List(string argFindKey)
        {
            string strGubun = "NAME";

            if (rdoGB2.Checked)
            {
                strGubun = "CODE";
            }
            else if (rdoGB3.Checked)
            {
                strGubun = "DAEPYO";
            }

            SSList.DataSource = hicLtdService.ViewLtd(argFindKey, strGubun);
        }

        private void SetControl()
        {
            comHpcLibBService = new ComHpcLibBService();

            SSList.Initialize(new SpreadOption { RowHeight = 28 });
            SSList.AddColumn("코드",             nameof(HIC_LTD.CODE),       64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("순번",             nameof(HIC_LTD.LTDSEQNO),   44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SSList.AddColumn("사업장명칭",       nameof(HIC_LTD.NAME),      160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("사업자등록번호",   nameof(HIC_LTD.SAUPNO),    100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("대표자명",         nameof(HIC_LTD.DAEPYO),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("지사",             nameof(HIC_LTD.JISA),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("사업장기호",       nameof(HIC_LTD.KIHO),       88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("업태",             nameof(HIC_LTD.UPTAE),      68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("종목",             nameof(HIC_LTD.JONGMOK),    68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("전화번호",         nameof(HIC_LTD.TEL),        88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("FAX번호",          nameof(HIC_LTD.FAX),        88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("보건담당",         nameof(HIC_LTD.BONAME),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("우편번호",         nameof(HIC_LTD.MAILCODE),   74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주소",             nameof(HIC_LTD.JUSO),       74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("상세주소",         nameof(HIC_LTD.JUSODETAIL), 74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnDateTime("계약일자", nameof(HIC_LTD.GYEDATE),   120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = false });
        }

        private void SetControl_Chk()
        {
            comHpcLibBService = new ComHpcLibBService();

            SSList.Initialize(new SpreadOption { RowHeight = 28 });
            SSList.AddColumn("코드",             nameof(HIC_LTD.CODE),        64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("순번",             nameof(HIC_LTD.LTDSEQNO),    44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SSList.AddColumn("사업장명칭",       nameof(HIC_LTD.NAME),       160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("사업구분",         nameof(HIC_LTD.LTDGONGNAME),160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("사업자등록번호",   nameof(HIC_LTD.SAUPNO),     100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("대표자명",         nameof(HIC_LTD.DAEPYO),      74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("지사",             nameof(HIC_LTD.JISA),        42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("사업장기호",       nameof(HIC_LTD.KIHO),        88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("업태",             nameof(HIC_LTD.UPTAE),       68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("종목",             nameof(HIC_LTD.JONGMOK),     68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("전화번호",         nameof(HIC_LTD.TEL),         88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("FAX번호",          nameof(HIC_LTD.FAX),         88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("보건담당",         nameof(HIC_LTD.BONAME),      74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("우편번호",         nameof(HIC_LTD.MAILCODE),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주소",             nameof(HIC_LTD.JUSO),        74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("상세주소",         nameof(HIC_LTD.JUSODETAIL),  74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnDateTime("계약일자", nameof(HIC_LTD.GYEDATE),    120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsShowCalendarButton = false });

            ssChkList.Initialize(new SpreadOption { RowHeight = 25 });
            ssChkList.AddColumn("회사코드",   nameof(COMHPC.LTDCODE),   74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            ssChkList.AddColumn("관리순번",   nameof(COMHPC.LTDSEQNO),  74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            ssChkList.AddColumn("년도",       nameof(COMHPC.GJYEAR),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssChkList.AddColumn("반기",       nameof(COMHPC.BANGI),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssChkList.AddColumn("측정계획",   nameof(COMHPC.COUNT1),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssChkList.AddColumn("사용실태",   nameof(COMHPC.COUNT2),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssChkList.AddColumn("소음제외",   nameof(COMHPC.COUNT3),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssChkList.AddColumn("소음",       nameof(COMHPC.COUNT4),    74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
        }
    }
}
