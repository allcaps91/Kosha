using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaLtdExamCompare.cs
/// Description     : 회사별 종검 그룹코드 비교
/// Author          : 김민철
/// Create Date     : 2020-05-25
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm회사별묶음코드(Frm회사별묶음코드.frm)" />
namespace HC_Main
{
    public partial class frmHaLtdExamCompare : Form
    {
        string fstrLtdName = "";
        string fstrGjjong = "";

        public delegate void SetGstrItem(HEA_GROUPCODE argGrpCD);
        public static event SetGstrItem rSetGstrItem;

        HIC_LTD LtdHelpItem = null;
        ComFunc CF = null;
        clsSpread cSpd = null;
        HeaGroupcodeService heaGroupCodeService = null;
        HeaGroupexamService heaGroupexamService = null;
        HeaGroupexamExcodeService heaGroupexamExcodeService = null;
        
        UnaryComparisonConditionalFormattingRule unary = null;

        public frmHaLtdExamCompare()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHaLtdExamCompare(string strLtdName,string strGjjong)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            fstrLtdName = strLtdName;
            fstrGjjong = strGjjong;
        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
            CF = new ComFunc();
            cSpd = new clsSpread();
            heaGroupCodeService = new HeaGroupcodeService();
            heaGroupexamService = new HeaGroupexamService();
            heaGroupexamExcodeService = new HeaGroupexamExcodeService();

            cboSex.Items.Clear();
            cboSex.Items.Add("전체");
            cboSex.Items.Add("남");
            cboSex.Items.Add("여");

            SS1.Initialize(new SpreadOption { RowHeight = 24 });
            SS1.AddColumnCheckBox("선택",     "", 47, new CheckBoxBooleanCellType { IsHeaderCheckBox = false });
            SS1.AddColumn("코드",       nameof(HEA_GROUPCODE.CODE),  58, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("그룹코드명", nameof(HEA_GROUPCODE.NAME), 180, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("금액",       nameof(HEA_GROUPCODE.AMT),   88, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });

            //SS3.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 24 });
            //SS3.AddColumn("코드",       nameof(HEA_GROUPEXAM_EXCODE.EXCODE),  58, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            //SS3.AddColumn("그룹코드명", nameof(HEA_GROUPEXAM_EXCODE.HNAME),  180, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.DarkSeaGreen;
            SS1.ActiveSheet.SetConditionalFormatting(-1, 1, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.DarkSeaGreen;
            SS3.ActiveSheet.SetConditionalFormatting(-1, 0, unary);
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnLtdHelp.Click   += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnClose.Click     += new EventHandler(eBtnClick);
            this.btnRemark.Click    += new EventHandler(eBtnClick);
            this.btnClear.Click     += new EventHandler(eBtnClick);
            this.btnCom.Click       += new EventHandler(eBtnClick);
            this.btnSel.Click       += new EventHandler(eBtnClick);
            this.btnSetting.Click   += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                Screen_Clear();
                Screen_Display();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnClose)
            {
                panRemark.Visible = false;
            }
            else if (sender == btnRemark)
            {
                panRemark.Visible = true;
            }
            else if (sender == btnClear)
            {
                Screen_Clear();
            }
            else if (sender == btnCom)
            {
                GroupCode_Compare();
            }
            else if (sender == btnSel)
            {
                SelectLtdGroupCode();
            }
            else if( sender == btnSetting)
            {
                btnSearch_Setting();
            }
        }

        private void btnSearch_Setting()
        {
            txtLtdName.Text = "0000.개인종검";
            SS1.ActiveSheet.RowCount = 0;
            SS1.ActiveSheet.RowCount = 20;
        }

        private void SelectLtdGroupCode()
        {
            int nCNT = 0;
            HEA_GROUPCODE item = new HEA_GROUPCODE();

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    item.CODE = SS1.ActiveSheet.Cells[i, 1].Text;
                    nCNT += 1;
                }
            }

            if (!item.CODE.IsNullOrEmpty())
            {
                HEA_GROUPCODE rHGRP = heaGroupCodeService.GetItemByCode(item.CODE);

                if (!rHGRP.IsNullOrEmpty())
                {
                    item.GBSEX = rHGRP.GBSEX;
                    item.JONG = rHGRP.JONG;
                    item.LTDCODE = rHGRP.LTDCODE;
                }
            }

            if (nCNT > 1)
            {
                MessageBox.Show("기본코드 1개만 선택 가능합니다.", "기본코드 선택오류");
                item = null;
            }
            else
            {
                rSetGstrItem(item);
            }

            this.Close();
        }

        private void GroupCode_Compare()
        {
            cSpd.Spread_Clear_Simple(SS3);

            //컬럼헤더 초기화
            for (int i = 2; i < SS3.ActiveSheet.ColumnCount; i++)
            {
                SS3.ActiveSheet.Columns.Get(i).Label = "";
            }

            int nCNT = 1;
            List<string> lstCode = new List<string>();

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    lstCode.Add(SS1.ActiveSheet.Cells[i, 1].Text.Trim());
                    nCNT += 1;
                }
            }

            SS3.ActiveSheet.ColumnCount = nCNT + 1;

            //컴럼헤더에 각 그룹코드명 기재
            for (int i = 0; i < lstCode.Count; i++)
            {
                SS3.ActiveSheet.Columns.Get(i + 2).Label = lstCode[i];
                cSpd.setSpdSort(SS3, i + 2, true);
            }

            cSpd.setSpdSort(SS3, 0, true);
            cSpd.setSpdSort(SS3, 1, true);

            //선택한 Group코드를 기준으로 검사항목을 다시 읽음
            if (lstCode.Count == 0)
            {
                return;
            }

            List<HEA_GROUPEXAM_EXCODE> list = heaGroupexamExcodeService.GetItemsByCodeIN(lstCode);

            if (list.Count > 0)
            {
                int nRow = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    nRow += 1;
                    if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                    SS3.ActiveSheet.Cells[i, 0].Text = list[i].EXCODE.Trim();
                    SS3.ActiveSheet.Cells[i, 1].Text = list[i].HNAME.Trim();
                }

                string strExCode = string.Empty;
                string strGrpCode = string.Empty;

                //그룹코드내에 검사코드가 포함되는지 체크
                for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
                {
                    for (int j = 2; j < SS3.ActiveSheet.ColumnCount; j++)
                    {
                        strGrpCode = SS3.ActiveSheet.Columns[j].Label.Trim();  //그룹코드
                        strExCode = SS3.ActiveSheet.Cells[i, 0].Text.Trim();        //비교 검사코드
                        //2021-01-19 컬럼크기 및 정렬추가
                        SS3.ActiveSheet.Columns[j].HorizontalAlignment = CellHorizontalAlignment.Center;
                        SS3.ActiveSheet.Columns[j].Width = 50;

                        if (!heaGroupexamService.ExistByGrpCodeExCode(strGrpCode, strExCode).IsNullOrEmpty())
                        {
                            SS3.ActiveSheet.Cells[i, j].Text = "◎";
                        }
                        else
                        {
                            SS3.ActiveSheet.Cells[i, j].BackColor = Color.LemonChiffon;
                        }
                    }
                }
            }
            
        }

        private void Screen_Display()
        {
            int nRow = 0;
            long nAmt = 0;

            if (txtLtdName.Text.Trim() == "")
            {
                MessageBox.Show("사업장코드가 공란입니다.", "조회불가!");
                return;
            }

            string strSex = string.Empty;

            if (cboSex.Text.Trim() == "남")
            {
                strSex = "M";
            }
            else if (cboSex.Text.Trim() == "여")
            {
                strSex = "F";
            }
            else
            {
                strSex = "";
            }

            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            string strDate = DateTime.Now.ToShortDateString();

            IList<HEA_GROUPCODE> lst = heaGroupCodeService.GetListByItem(nLtdCode, strSex, strDate);

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }

                    if (string.Compare(lst[i].SUDATE.ToString(), DateTime.Now.ToShortDateString()) > 0)
                    {
                        nAmt = lst[i].OLDAMT;
                    }
                    else
                    {
                        nAmt = lst[i].AMT;
                    }

                    if (nAmt > 0)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].CODE;
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = lst[i].NAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = VB.Format(nAmt, "###,###,##0");
                    }
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cboSex.SelectedIndex = 0;
            Screen_Clear();

            if (!fstrLtdName.IsNullOrEmpty())
            {
                txtLtdName.Text = fstrLtdName;
                eBtnClick(btnSearch, new EventArgs());
            }
            else if(VB.Left(fstrGjjong,2) == "11" || VB.Left(fstrGjjong, 2) == "12")
            {
                eBtnClick(btnSetting, new EventArgs());
            }       
        }

        private void Screen_Clear()
        {
            //txtLtdName.Text = "";

            panRemark.Visible = false;
            
            //txtHaRemark.Text = "";

            cSpd.Spread_Clear_Simple(SS1);
            cSpd.Spread_Clear_Simple(SS3);
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text += "." + LtdHelpItem.SANGHO;
                txtHaRemark.Text = LtdHelpItem.HAREMARK;
            }
            else
            {
                txtLtdName.Text = "";
                txtHaRemark.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
