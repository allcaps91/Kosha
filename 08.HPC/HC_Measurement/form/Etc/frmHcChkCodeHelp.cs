using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Measurement
/// File Name       : frmHcChkCodeHelp.cs
/// Description     : 작업환경측정 기초코드 찾기
/// Author          : 김민철
/// Create Date     : 2021-08-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
namespace HC_Measurement
{
    public partial class frmHcChkCodeHelp : CommonForm
    {
        HicCodeService hicCodeService = null;
        HicChkMcodeService hicChkMCodeService = null;

        public delegate void SetMCodeValue(string GstrCode, string GstrName, string GstGCode, string GstGCode1, string TWA_PPM, string TWA_MG, string STEL_PPM, string STEL_MG, string strUnit);
        public event SetMCodeValue rSetMCodeValue;

        public delegate void SetGstrValue(string GstrCode, string GstrName, string GstGCode, string GstGCode1);
        public event SetGstrValue rSetGstrValue;

        string FstrGubun = string.Empty;
        string FstrFind = string.Empty;

        public frmHcChkCodeHelp()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHcChkCodeHelp(string strGB, string strFind)
        {
            InitializeComponent();

            FstrGubun = strGB;
            FstrFind = strFind;

            SetControl();
            SetEvents();
        }

        private void SetEvents()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.txtSearch.KeyDown      += new KeyEventHandler(eKeyPress);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void SetControl()
        {
            hicCodeService = new HicCodeService();
            hicChkMCodeService = new HicChkMcodeService();

            SpreadCellTypeOption CTypeOot1 = null;
            CTypeOot1 = new SpreadCellTypeOption();
            CTypeOot1.IsEditble = true;
            CTypeOot1.Aligen = CellHorizontalAlignment.Left;

            SpreadOption sOP = new SpreadOption();
            sOP.RowHeight = 36;

            SSList.Initialize(sOP);
            SSList.AddColumn("코드",      nameof(HIC_CODE.CODE),  80, new SpreadCellTypeOption { });
            SSList.AddColumn("코드명칭",  nameof(HIC_CODE.NAME),  280, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("코드명칭2", nameof(HIC_CODE.GCODE), 280, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("코드명칭3", nameof(HIC_CODE.GCODE1), 280, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            string strCode = "";
            string strName = "";
            string strName2 = "";
            string strName3 = "";

            string strTEA_PPM = "";
            string strTEA_MG= "";
            string strSTEL_PPM = "";
            string strSTEL_MG = "";
            string strUNIT = "";

            if (FstrGubun == "MCODE")
            {
                if (rSetMCodeValue.IsNullOrEmpty())
                {
                    this.Close();
                    return;
                }
                else
                {
                    strCode = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                    strName = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    strName2 = SSList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    strName3 = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                    strTEA_PPM = SSList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                    strTEA_MG = SSList.ActiveSheet.Cells[e.Row, 5].Text.Trim();
                    strSTEL_PPM = SSList.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                    strSTEL_MG = SSList.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                    //보정값을 클릭한 경우
                    if (e.Column >= 4 && e.Column <= 7)
                    {
                        strName3 = SSList.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                        if (e.Column == 4 || e.Column == 6)
                        {
                            strUNIT = "ppm";
                        }
                        else
                        {
                            strUNIT = "mg";
                        }
                    }

                    rSetMCodeValue(strCode, strName, strName2, strName3, strTEA_PPM, strTEA_MG, strSTEL_PPM, strSTEL_MG, strUNIT);

                    this.Close();
                    return;
                }
            }
            else
            {
                if (rSetGstrValue.IsNullOrEmpty())
                {
                    this.Close();
                    return;
                }
                else
                {
                    strCode = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                    strName = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    strName2 = SSList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    strName3 = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                    rSetGstrValue(strCode, strName, strName2, strName3);

                    this.Close();
                    return;
                }
            }
        }

        private void eKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (FstrGubun == "ANAL")
                {
                    View_Data_Anal(txtSearch.Text.Trim());
                }
                else
                {
                    View_Data(FstrGubun, txtSearch.Text.Trim());
                }
            }
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
                if (FstrGubun == "ANAL")
                {
                    View_Data_Anal(txtSearch.Text.Trim());
                }
                else
                {
                    View_Data(FstrGubun, txtSearch.Text.Trim());
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string[] arrTitle = null;

            switch (FstrGubun)
            { 
                case "GONG": this.Text = "코드찾기【공정코드】";      break;
                case "MCODE": this.Text = "코드찾기【물질코드】";
                    SSList.ActiveSheet.Columns[2].Visible = false;
                    SSList.ActiveSheet.Columns[3].Visible = false;
                    SSList.AddColumn("TWA_PPM",  nameof(HIC_CODE.TWA_PPM),  78, new SpreadCellTypeOption { });
                    SSList.AddColumn("TWA_MG",   nameof(HIC_CODE.TWA_MG),   78, new SpreadCellTypeOption { });
                    SSList.AddColumn("STEL_PPM", nameof(HIC_CODE.STEL_PPM), 78, new SpreadCellTypeOption { });
                    SSList.AddColumn("STEL_MG",  nameof(HIC_CODE.STEL_MG),  78, new SpreadCellTypeOption { });
                    this.Width = 1000;
                    break;
                case "ANAL":
                    this.Text = "코드찾기【분석방법】";
                    arrTitle = new string[] { "코드", "항목", "채취방법", "분석방법" };
                    Set_Spread_Header(arrTitle);
                    break;
                default:   this.Text = "코드찾기【오류】";            break;
            }

            View_Data(FstrGubun, FstrFind);
        }

        private void Set_Spread_Header(string[] arrTitle)
        {
            for (int i = 0; i < SSList.ActiveSheet.ColumnCount; i++)
            {
                SSList.ActiveSheet.Columns.Get(i).Label = arrTitle[i];
            }
        }

        private void View_Data(string strGubun, string strFind)
        {
            if (strGubun == "MCODE") //물질코드
            {
                List<HIC_CHK_MCODE> list = hicChkMCodeService.GetItemAll(strFind);
                SSList.DataSource = list;
            }
            else if (strGubun == "JIKJONG")
            {
                List<HIC_CODE> list = hicCodeService.GetListByCodeName("05", strFind);
                SSList.DataSource = list;
            }
            else if (strGubun == "GONG")
            {
                List<HIC_CODE> list = hicCodeService.GetListByCodeName("C4", strFind);
                SSList.DataSource = list;
            }
            else if (strGubun == "ANAL")
            {
                List<HIC_CODE> list = hicCodeService.GetListByCode("15", strFind);
                SSList.DataSource = list;
            }
        }

        private void View_Data_Anal(string argKeyWard)
        {
            List<HIC_CODE> list = hicCodeService.GetItemByLikeAll("15", argKeyWard);
            SSList.DataSource = list;
        }
    }
}
