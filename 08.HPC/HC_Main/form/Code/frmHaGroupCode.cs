using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.Drawing;
using System.Collections.Generic;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Enums;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaGroupCode.cs
/// Description     : 종검접수 그룹코드 선택창
/// Author          : 김민철
/// Create Date     : 2020-09-10
/// Comments        : 기존 그룹코드 선택창을 개선하여 통합된 그룹코드 목록을 보여줌
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGroupCode(FrmGroupCode.frm)" />
namespace HC_Main
{
    public partial class frmHaGroupCode : Form
    {
        public delegate void rSendMsg(List<READ_SUNAP_ITEM> argCode);
        public static event rSendMsg rSndMsg;

        bool bolSort = false;

        clsSpread cSpd = null;

        string FstrWard = string.Empty;

        List<READ_SUNAP_ITEM> FstrCodeslist = new List<READ_SUNAP_ITEM>();
        READ_SUNAP_ITEM rRSI = new READ_SUNAP_ITEM();

        HeaGroupcodeService heaGroupcodeService = null;
        ReadSunapItemService readSunapItemService = null;

        public frmHaGroupCode()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHaGroupCode(string argWard, List<READ_SUNAP_ITEM> argCodes)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            FstrWard = argWard;
            if (!argCodes.IsNullOrEmpty())
            {
                FstrCodeslist = argCodes;
            }
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            heaGroupcodeService = new HeaGroupcodeService();
            readSunapItemService = new ReadSunapItemService();

            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS1.AddColumnCheckBox("선택", "", 44, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false });
            SS1.AddColumn("묶음코드",         nameof(READ_SUNAP_ITEM.GRPCODE),  70, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(246, 234, 210) });
            SS1.AddColumn("묶음코드명",       nameof(READ_SUNAP_ITEM.GRPNAME), 280, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("금액",       nameof(READ_SUNAP_ITEM.AMT), 70, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });

        }

        void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.txtName.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.SS1.ButtonClicked      += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdClick);
            this.rdoGbn1.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn2.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn3.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn4.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn5.CheckedChanged += new EventHandler(eRdoChange);
            this.rdoGbn6.CheckedChanged += new EventHandler(eRdoChange);
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(SS1, e.Column, ref bolSort, true);
                return;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            rRSI = new READ_SUNAP_ITEM();

            if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "Y")
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;

                rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                
                if (FstrCodeslist.Count == 0) { return; }

                for (int i = 0; i < FstrCodeslist.Count; i++)
                {
                    if (FstrCodeslist[i].GRPCODE.Trim() == rRSI.GRPCODE.To<string>("").Trim())
                    {
                        FstrCodeslist.RemoveAt(i);
                    }
                }
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "Y";
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;

                rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                rRSI.GRPNAME = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                rRSI.ADDGBN = "Y";

                FstrCodeslist.Add(rRSI);
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column >= 0 && e.Row >= 0)
            {
                if (e.Column == 0)
                {
                    clsSpread cSpd = new clsSpread();

                    rRSI = new READ_SUNAP_ITEM();

                    if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "Y")
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Blue);

                        rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                        rRSI.GRPNAME = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                        rRSI.ADDGBN = "Y";

                        FstrCodeslist.Add(rRSI);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Black);

                        rRSI.GRPCODE = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                        for (int i = 0; i < FstrCodeslist.Count; i++)
                        {
                            if (FstrCodeslist[i].GRPCODE.Trim() == rRSI.GRPCODE.Trim())
                            {
                                FstrCodeslist.RemoveAt(i);
                            }
                        }
                    }

                    cSpd = null;
                }
            }
        }

        void eRdoChange(object sender, EventArgs e)
        {
            string strSDName = ((RadioButton)sender).Name;            

            if (strSDName.Substring(0, 6) == "rdoGbn")
            {
                if (((RadioButton)sender).Checked == true)
                {
                    switch (strSDName)
                    {
                        case "rdoGbn1": FstrWard = "Dust";      break;
                        case "rdoGbn2": FstrWard = "조직+CLO";  break;
                        case "rdoGbn3": FstrWard = "BMD";       break;
                        case "rdoGbn4": FstrWard = "추가";      break;
                        case "rdoGbn5": FstrWard = "수면";      break;
                        case "rdoGbn6": FstrWard = "전체";      break;
                        default: FstrWard = ""; break;
                    }
                    
                    Screen_Display(FstrWard, txtName.Text.Trim());

                    Display_Selected_Codes();

                    //SS1.ActiveSheet.SortRows(0, true, true);
                    clsSpread.gSpdSortRow(SS1, 0, ref bolSort, true);

                    txtName.Focus();
                }
            }
        }

        /// <summary>
        /// 선택된 묶음코드 체크표시
        /// </summary>
        private void Display_Selected_Codes()
        {
            string strCode = string.Empty;
            if (FstrCodeslist.IsNullOrEmpty() || FstrCodeslist.Count == 0) { return; }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();

                for (int j = 0; j < FstrCodeslist.Count; j++)
                {
                    if (VB.L(strCode, FstrCodeslist[j].GRPCODE.To<string>("").Trim()) > 1)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "Y";
                        SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                    }

                    if (VB.L(strCode, FstrCodeslist[j].UCODE.To<string>("").Trim()) > 1)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "Y";
                        SS1.ActiveSheet.Cells[i, 0, i, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                    }
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtName && e.KeyChar == (char)13)
            {
                Screen_Display(FstrWard, txtName.Text.Trim());
                btnSearch.Focus();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strCode = string.Empty;

            cSpd.Spread_Clear_Simple(SS1);

            for (int i = 0; i < FstrCodeslist.Count; i++)
            {
                FstrCodeslist[i].ADDGBN = "";
            }


            txtName.Text = FstrWard;

            rdoGbn6.Checked = true;
        }

        void Screen_Display(string argWard, string argName)
        {
            try
            {
                List<READ_SUNAP_ITEM> list = readSunapItemService.GetHeaGrpCodeListByNameLike(argWard, argName);

                SS1.SetDataSource(null);
                SS1.SetDataSource(list);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                Screen_Display(FstrWard, txtName.Text.Trim());
            }
            else if (sender == btnSave)
            {
                rSndMsg(FstrCodeslist);

                this.Close();
            }
        }
    }
}
