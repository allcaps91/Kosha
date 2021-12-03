using ComBase;
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
/// File Name       : frmHcCodeHelp.cs
/// Description     : 검진 기초코드 찾기
/// Author          : 김민철
/// Create Date     : 2019-08-010
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcCode31.frm(FrmCodeHelp)" />
namespace ComHpcLibB
{
    public partial class frmHcCodeHelp : BaseForm
    {
        HicCodeService hicCodeService = new HicCodeService();
        HicSpcScodeService hicSpcScodeService = new HicSpcScodeService();
        HicMcodeService hicMcodeService = new HicMcodeService();
        HicExjongService hicExjongService = new HicExjongService();
        BasIllsService basIllsService = new BasIllsService();
        InsaMstService insaMstService = new InsaMstService();

        //public delegate void SetGstrValue(HIC_CODE GstrValue);
        public delegate void SetGstrValue(string GstrCode, string GstrName);
        public event SetGstrValue rSetGstrValue;

        string FstrGubun = string.Empty;
        string FstrFind = string.Empty;
        string FstrstrRetValue = string.Empty;

        public frmHcCodeHelp()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHcCodeHelp(string strGB = "", string strFind = "", string strstrRetValue = "")
        {
            InitializeComponent();

            FstrGubun = strGB;
            FstrFind = strFind;
            FstrstrRetValue = strstrRetValue;

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
            string strCode = "";
            string strName = "";
            //HIC_CODE item = SSList.GetCurrentRowData() as HIC_CODE;

            if (rSetGstrValue.IsNullOrEmpty())
            {
                this.Close();
            }
            else
            {
                strCode = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strName = SSList.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                //rSetGstrValue(item);
                rSetGstrValue(strCode, strName);
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
                default:   this.Text = "코드찾기【오류】";             break;
            }

            //Test
            //FstrGubun = "A6";

            View_Data(FstrGubun, FstrFind, FstrstrRetValue);
        }

        private void View_Data(string strGubun, string strName, string strRetValue = "")
        {
            if (string.Compare(FstrGubun, "50") < 0 || FstrGubun == "53" || FstrGubun == "84" || FstrGubun == "A2" || FstrGubun == "A6")
            {
                List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubunNameGcode(strGubun, strName, VB.Right(strRetValue, 1));
                SSList.DataSource = list;
            }
            else if (FstrGubun == "54") //특수판정소견코드
            {
                List < HIC_SPC_SCODE > list = hicSpcScodeService.GetListByCodeName(strName, VB.Right(strRetValue, 1));
                SSList.DataSource = list;
            }
            else if (FstrGubun == "51")
            {
                List<HIC_MCODE> list = hicMcodeService.GetListByCodeName(strGubun, strName);
                SSList.DataSource = list;                
            }
            else if (FstrGubun == "GJ")
            {
                List<HIC_EXJONG> list = hicExjongService.GetListByCodeName(strName);
                SSList.DataSource = list;
            }
            else if (FstrGubun == "IL")
            {
                List<BAS_ILLS> list = basIllsService.GetListByIllNameK(strName);
                SSList.DataSource = list;
            }
            else if (FstrGubun == "SA")
            {
                List<INSA_MST> list = insaMstService.GetSabunCodebyKorName(strName);
                SSList.DataSource = list;
            }
            else if (FstrGubun == "JO")
            {
                List<HIC_CODE> list = hicCodeService.GetCodeNamebyName(strName);
                SSList.DataSource = list;
            }
            else
            {
                SSList.DataSource = hicCodeService.GetListByCodeName(strGubun, strName);
            }
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
            SSList.AddColumn("코드",           nameof(HIC_LTD.CODE),  80, FpSpreadCellType.TextCellType, CTypeOot1);
            SSList.AddColumn("코드명칭",       nameof(HIC_LTD.NAME), 340, FpSpreadCellType.TextCellType, CTypeOot1);

            //SetAutoText(txtSearch, FstrGubun);
        }

        private void SetAutoText(TextBox txtSearch, string fstrGubun)
        {
            List<HIC_CODE> list = hicCodeService.AutoCompleteText(fstrGubun);

            string[] array = list.GetStringArray("NAME");
            txtSearch.SetAutoComplete(list, "NAME");
        }
    }
}
