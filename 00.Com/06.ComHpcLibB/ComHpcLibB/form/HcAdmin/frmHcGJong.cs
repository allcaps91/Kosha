/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcGJong.cs
/// Description     : 건진센터 검진종류 관리
/// Author          : 김민철
/// Create Date     : 2019-07-11
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmGemJongEntry(HcCode09.frm)" />

namespace ComHpcLibB
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComHpcLibB.Service;
    using FarPoint.Win.Spread;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public partial class frmHcGJong : BaseForm
    {
        HicExjongService hicExjongService   = null;
        HicCodeService   hcCodeService      = null;
        AbcBuseService   abcBuseService     = null;   

        public frmHcGJong()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            hicExjongService = new HicExjongService();
            hcCodeService    = new HicCodeService();
            abcBuseService   = new AbcBuseService();

            SS1.Initialize();
            SS1.AddColumn("코드",        nameof(HIC_EXJONG.CODE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("검진명칭",    nameof(HIC_EXJONG.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("분류",        nameof(HIC_EXJONG.BUN),      82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("차수",        nameof(HIC_EXJONG.CHASU),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("부담율",      nameof(HIC_EXJONG.BURATE),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("변경",        nameof(HIC_EXJONG.BUCHANGE), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("ABC부서코드", nameof(HIC_EXJONG.BUCODE),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });

            chkBudam.SetOptions(  new CheckBoxOption    { DataField = nameof(HIC_EXJONG.BUCHANGE),  CheckValue = "Y", UnCheckValue = "" });
            rdoChasu1.SetOptions( new RadioButtonOption { DataField = nameof(HIC_EXJONG.CHASU),     CheckValue = "1" });
            rdoChasu2.SetOptions( new RadioButtonOption { DataField = nameof(HIC_EXJONG.CHASU),     CheckValue = "2" });
            rdoChasu3.SetOptions( new RadioButtonOption { DataField = nameof(HIC_EXJONG.CHASU),     CheckValue = "3" });
            chkRep1.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT1),    CheckValue = "Y", UnCheckValue = "" });
            chkRep2.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT2),    CheckValue = "Y", UnCheckValue = "" });
            chkRep3.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT3),    CheckValue = "Y", UnCheckValue = "" });
            chkRep4.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT4),    CheckValue = "Y", UnCheckValue = "" });
            chkRep5.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT5),    CheckValue = "Y", UnCheckValue = "" });
            chkRep6.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBPRT6),    CheckValue = "Y", UnCheckValue = "" });
            chkNhic.SetOptions(   new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBNHIC),    CheckValue = "Y", UnCheckValue = "" });
            chkJin.SetOptions(    new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBJIN),     CheckValue = "Y", UnCheckValue = "" });
            chkSangDam.SetOptions(new CheckBoxOption    { DataField = nameof(HIC_EXJONG.GBSANGDAM), CheckValue = "Y", UnCheckValue = "" });

            List<HIC_CODE> list = hcCodeService.FindOne("B4");
            cboBuRate.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboSuga.Items.Clear();
            cboSuga.Items.Add("");
            cboSuga.Items.Add("1.보험80%");
            cboSuga.Items.Add("2.보험100%");
            cboSuga.Items.Add("3.보험125%");
            cboSuga.Items.Add("4.일,특수차액");
            cboSuga.Items.Add("5.임의수가");
            cboSuga.SelectedIndex = 0;

            cboMunjin.Items.Clear();
            cboMunjin.Items.Add("");
            cboMunjin.Items.Add("0.문진안함");
            cboMunjin.Items.Add("1.공단검진");
            cboMunjin.Items.Add("2.암검진");
            cboMunjin.Items.Add("3.특수검진");
            cboMunjin.Items.Add("4.공단+특수");
            cboMunjin.Items.Add("9.기타");
            cboMunjin.SelectedIndex = 0;

            List<HIC_CODE> list2 = hcCodeService.FindOne("24");
            cboInWon.SetItems(list2, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            List<ABC_BUSE> list3 = abcBuseService.BuseList();
            cboABCBuCode.SetItems(list3, "NAME", "BUCODE", "", "", AddComboBoxPosition.Top);

            cboBun.Items.Clear();
            cboBun.Items.Add("");
            cboBun.Items.Add("1.공단검진");
            cboBun.Items.Add("2.특수검진");
            cboBun.Items.Add("3.암검진");
            cboBun.Items.Add("4.작업측정");
            cboBun.Items.Add("5.보건전문");
            cboBun.Items.Add("6.기타");
            cboBun.SelectedIndex = 0;

            panMain.SetEnterKey();
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnDelete.Click     += new EventHandler(eBtnClick);
            this.txtCode.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode && e.KeyChar == (char)13)
            {
                if (txtCode.Text.Trim() != "")
                {
                    HIC_EXJONG item = hicExjongService.Read_ExJong_CodeName(txtCode.Text.Trim());

                    if (item == null)
                    {
                        Screen_Clear();
                        MessageBox.Show("조회된 Data가 없음");
                        return;
                    }

                    panMain.SetData(item);
                }
            }
        }

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SS1)
            {
                HIC_EXJONG item = SS1.GetRowData(e.Row) as HIC_EXJONG;
                panMain.SetData(item);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                clsSpread cSpd = new clsSpread();
                cSpd.Spread_All_Clear(SS1);
                cSpd.Dispose();

                SS1.DataSource = hicExjongService.FindAll();
            }
            else if (sender == btnSave)
            {
                HIC_EXJONG item = panMain.GetData<HIC_EXJONG>();
                
                if (!panMain.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (!hicExjongService.DataCheck(item))
                {
                    return;
                }

                item.ENTDATE = DateTime.Now;
                item.ENTSABUN = Convert.ToInt32(clsType.User.IdNumber);

                if (item.ROWID == null || item.ROWID == "")
                {
                    int result = hicExjongService.Insert(item);

                    if (result > 0)
                    {
                        MessageBox.Show("저장 하였습니다.");
                        panMain.Initialize();
                        Screen_Clear();
                        return;
                    }
                }
                else
                {
                    int result = hicExjongService.Update(item);

                    if (result > 0)
                    {
                        MessageBox.Show("저장 하였습니다.");
                        panMain.Initialize();
                        Screen_Clear();
                        return;
                    }
                }
                
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender ==  btnExit)
            {
                this.Close();
            }
            else if (sender == btnDelete)
            {
                HIC_EXJONG item = panMain.GetData<HIC_EXJONG>();

                if (ComFunc.MsgBoxQ("검사항목을 삭제하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                int result = hicExjongService.Delete(item);

                if (result > 0)
                {
                    MessageBox.Show("삭제 하였습니다.");
                    panMain.Initialize();
                    Screen_Clear();
                    return;
                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            #region Control ErrorProvider

            panMain.AddRequiredControl(txtCode);
            panMain.AddRequiredControl(txtName);
            panMain.AddRequiredControl(cboBun);
            panMain.AddRequiredControl(cboBuRate);
            panMain.AddRequiredControl(cboSuga);
            panMain.AddRequiredControl(cboMunjin);
            panMain.AddRequiredControl(cboABCBuCode);

            #endregion
        }

        private void Screen_Clear()
        {
            ComFunc.SetAllControlClearEx(panMain);
        }
    }
}
