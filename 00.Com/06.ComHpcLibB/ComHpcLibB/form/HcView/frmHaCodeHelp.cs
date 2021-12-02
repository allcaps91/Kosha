using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaCodeHelp.cs
/// Description     : 종검 기초코드 찾기
/// Author          : 김민철
/// Create Date     : 2020-09-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaCode00.frm(FrmCodeHelp)" />
namespace ComHpcLibB
{
    public partial class frmHaCodeHelp : BaseForm
    {
        HeaCodeService heaCodeService = new HeaCodeService();

        public delegate void SetGstrValue(HEA_CODE GstrValue);
        public event SetGstrValue rSetGstrValue;

        string FstrGubun = string.Empty;
        string FstrFind = string.Empty;

        public frmHaCodeHelp()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHaCodeHelp(string strGB = "", string strFind = "")
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

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            HEA_CODE item = SSList.GetCurrentRowData() as HEA_CODE;

            if (rSetGstrValue.IsNullOrEmpty())
            {
                this.Close();
            }
            else
            {
                rSetGstrValue(item);
                this.Close();
            }
            
        }

        private void eKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                View_Data(FstrGubun, txtSearch.Text.Trim());
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
                View_Data(FstrGubun, txtSearch.Text.Trim());
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            switch (FstrGubun)
            { 
                case "01": this.Text = "코드찾기【업종】";             break;
                case "02": this.Text = "코드찾기【지방관서】";         break;
                case "03": this.Text = "코드찾기【지도원】";           break;
                case "04": this.Text = "코드찾기【제품코드】";         break;
                case "05": this.Text = "코드찾기【직종】";             break;
                case "06": this.Text = "코드찾기【작업공정】";         break;
                case "GJ": this.Text = "코드찾기【건진종류】";         break;
                case "08": this.Text = "코드찾기【유해물질동의어】";   break;
                case "09": this.Text = "코드찾기【유해인자】";         break;
                case "10": this.Text = "코드찾기【질병분류】";         break;
                case "11": this.Text = "코드찾기【자타각증상】";       break;
                case "12": this.Text = "코드찾기【사후관리내용】";     break;
                case "13": this.Text = "코드찾기【업무적합성】";       break;
                case "14": this.Text = "코드찾기【조치코드】";         break;
                case "15": this.Text = "코드찾기【측정방법】";         break;
                case "16": this.Text = "코드찾기【측정분석】";         break;
                case "17": this.Text = "코드찾기【결과값코드】";       break;
                case "18": this.Text = "코드찾기【사업장기호】";       break;
                case "19": this.Text = "코드찾기【질병분류코드】";     break;
                case "21": this.Text = "코드찾기【건강보험지사】";     break;
                case "22": this.Text = "코드찾기【통계분류코드】";     break;
                case "23": this.Text = "코드찾기【특정수가분류】";     break;
                case "24": this.Text = "코드찾기【인원통계분류】";     break;
                case "25": this.Text = "코드찾기【보건소(시군구)】";   break;
                case "26": this.Text = "코드찾기【군병원】";           break;
                case "27": this.Text = "코드찾기【일반소견,조치】";    break;
                case "31": this.Text = "코드찾기【직업병코드】";       break;
                case "51": this.Text = "코드찾기【취급물질명】";       break;
                case "53": this.Text = "코드찾기【특수추가검사】";     break;
                case "54": this.Text = "코드찾기【특수판정소견】";     break;
                case "IL": this.Text = "코드찾기【상병코드ICD10】";    break;
                case "A2": this.Text = "코드찾기【작업공정】";         break;
                case "A6": this.Text = "코드찾기【제품코드_New】";     break;
                case "84": this.Text = "코드찾기【자타각증상】";       break;
                case "98": this.Text = "코드찾기【조치코드】";         break;
                case "SA": this.Text = "코드찾기【추천직원】";         break;
                default:   this.Text = "코드찾기【오류】";             break;
            }

            //Test
            //FstrGubun = "A6";

            View_Data(FstrGubun, FstrFind);
        }

        private void View_Data(string strGubun, string strName)
        {
            SSList.DataSource = heaCodeService.GetListByCodeName(strGubun, strName);
        }

        private void SetControl()
        {
            SpreadCellTypeOption CTypeOot1 = null;
            CTypeOot1 = new SpreadCellTypeOption();
            CTypeOot1.IsEditble = true;
            CTypeOot1.Aligen = CellHorizontalAlignment.Left;

            SpreadOption sOP = new SpreadOption();
            sOP.RowHeight = 36;

            SSList.Initialize(sOP);
            SSList.AddColumn("코드",           nameof(HEA_CODE.CODE),    80, FpSpreadCellType.TextCellType, CTypeOot1);
            SSList.AddColumn("코드명칭",       nameof(HEA_CODE.NAME),   340, FpSpreadCellType.TextCellType, CTypeOot1);
            SSList.AddColumn("비고",           nameof(HEA_CODE.GUBUN1), 340, FpSpreadCellType.TextCellType, CTypeOot1);

            //SetAutoText(txtSearch, FstrGubun);
        }

    }
}
