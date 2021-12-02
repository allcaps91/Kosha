using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSpcSCode.cs
/// Description     : 특수검진 소견코드 등록
/// Author          : 김민철
/// Create Date     : 2020-01-23
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmSpcSCode(HcCode22.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcSpcSCode : Form
    {
        HicSpcScodeService hicSpcScodeService = null;
        HicCodeService hicCodeservice = null;
        HicMcodeService hicMcodeService= null;

        frmHcPanPanjengHelp FrmHcPanPanjengHelp = null;
        frmHcPanSpcSahuCode FrmHcPanSpcSahuCode = null;

        public frmHcSpcSCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            hicSpcScodeService = new HicSpcScodeService();
            hicCodeservice = new HicCodeService();
            hicMcodeService = new HicMcodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS1.AddColumn("코드",         nameof(HIC_SPC_SCODE.CODE),     44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("소견명칭",     nameof(HIC_SPC_SCODE.NAME),    240, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("판정",         nameof(HIC_SPC_SCODE.PANJENG),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsSort = true });            
            SS1.AddColumn("질병분류코드", nameof(HIC_SPC_SCODE.DBUN),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
            SS1.AddColumn("질병분류",     nameof(HIC_SPC_SCODE.DBUN_NM), 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("물질코드",     nameof(HIC_SPC_SCODE.MCODE),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
            SS1.AddColumn("취급물질",     nameof(HIC_SPC_SCODE.MCODE_NM),100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("공단코드",     nameof(HIC_SPC_SCODE.GCODE),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("순위",         nameof(HIC_SPC_SCODE.SORT),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("삭제일자",     nameof(HIC_SPC_SCODE.DELDATE),  78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, ForceColor = System.Drawing.Color.DarkRed });
            SS1.AddColumn("판정기준",     nameof(HIC_SPC_SCODE.AUTOPANGBN),42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("ROWID",        nameof(HIC_SPC_SCODE.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });

            dtpDelDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_SPC_SCODE.DELDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });

            rdoAuto1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_SPC_SCODE.AUTOPANGBN), CheckValue = "" });
            rdoAuto2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_SPC_SCODE.AUTOPANGBN), CheckValue = "1" });
            rdoAuto3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_SPC_SCODE.AUTOPANGBN), CheckValue = "2" });

            cboDBun.SetOptions(new ComboBoxOption { DataField = nameof(HIC_SPC_SCODE.DBUN) });
            cboDBun.SetItems(hicCodeservice.FindOne("19"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboMCode.SetOptions(new ComboBoxOption { DataField = nameof(HIC_SPC_SCODE.MCODE) });
            cboMCode.SetItems(hicMcodeService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboUpmu.SetOptions(new ComboBoxOption { DataField = nameof(HIC_SPC_SCODE.WORKYN) });
            cboUpmu.SetItems(hicCodeservice.FindOne("13"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.정상A");
            cboPanjeng.Items.Add("2.정상B");
            cboPanjeng.Items.Add("3.C1(직업병 요관찰자)");
            cboPanjeng.Items.Add("4.C2(일반질병 요관찰자)");
            cboPanjeng.Items.Add("5.D1(직업병 유소견자)");
            cboPanjeng.Items.Add("6.D2(일반질병 유소견자)");
            cboPanjeng.Items.Add("7.R(2차건진대상자)");
            cboPanjeng.Items.Add("8.U(미판정자)");
            cboPanjeng.Items.Add("9.CN(야간작업)");
            cboPanjeng.Items.Add("A.DN(야간작업)");

            cboPanjeng2.Items.Clear();
            cboPanjeng2.Items.Add("*.전체");
            cboPanjeng2.Items.Add("1.정상A");
            cboPanjeng2.Items.Add("2.정상B");
            cboPanjeng2.Items.Add("3.C1(직업병 요관찰자)");
            cboPanjeng2.Items.Add("4.C2(일반질병 요관찰자)");
            cboPanjeng2.Items.Add("5.D1(직업병 유소견자)");
            cboPanjeng2.Items.Add("6.D2(일반질병 유소견자)");
            cboPanjeng2.Items.Add("7.R(2차건진대상자)");
            cboPanjeng2.Items.Add("8.U(미판정자)");
            cboPanjeng2.Items.Add("9.CN(야간작업)");
            cboPanjeng2.Items.Add("A.DN(야간작업)");

        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnDelete.Click     += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnReExam.Click     += new EventHandler(eBtnClick);
            this.btnSahu.Click       += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.txtCode.KeyPress    += new KeyPressEventHandler(eKeyPress);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strCode = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            Display_Item(strCode);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode && e.KeyChar == (char)13)
            {
                Display_Item(txtCode.Text.Trim());
            }
        }

        private void Display_Item(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            HIC_SPC_SCODE item = hicSpcScodeService.GetItembyCode(argCode);

            if (!item.IsNullOrEmpty())
            {
                panMain.SetData(item);

                for (int i = 0; i < cboPanjeng.Items.Count; i++)
                {
                    if (item.PANJENG2 == VB.Pstr(cboPanjeng.Items[i].ToString(), ".", 1))
                    {
                        cboPanjeng.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboDBun.Items.Count; i++)
                {
                    if (item.DBUN.To<string>("").Trim() == VB.Pstr(cboDBun.Items[i].ToString(), ".", 1))
                    {
                        cboDBun.SelectedIndex = i;
                        break;
                    }
                }

                if (item.AUTOPANGBN == "1")
                {
                    rdoAuto2.Checked = true;
                }
                else if (item.AUTOPANGBN == "2")
                {
                    rdoAuto3.Checked = true;
                }
                else
                {
                    rdoAuto1.Checked = true;
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                string strPan = VB.Left(cboPanjeng2.Text, 1);

                SS1.DataSource = hicSpcScodeService.FindAll(strPan, chkDel.Checked);
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSave)
            {
                Data_Save("Save");
            }
            else if (sender == btnDelete)
            {
                Data_Save("Delete");
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnReExam)
            {
                FrmHcPanPanjengHelp = new frmHcPanPanjengHelp("53");
                FrmHcPanPanjengHelp.rSetGstrValue += new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
                FrmHcPanPanjengHelp.ShowDialog();
                FrmHcPanPanjengHelp.rSetGstrValue -= new frmHcPanPanjengHelp.SetGstrValue(ReExam_value);
            }
            else if (sender == btnSahu)
            {
                FrmHcPanSpcSahuCode = new frmHcPanSpcSahuCode("70");
                FrmHcPanSpcSahuCode.rSetGstrValue += new frmHcPanSpcSahuCode.SetGstrValue(Sahu_value);
                FrmHcPanSpcSahuCode.ShowDialog();
                FrmHcPanSpcSahuCode.rSetGstrValue -= new frmHcPanSpcSahuCode.SetGstrValue(Sahu_value);
            }
        }

        private void Sahu_value(string GstrValue, string GstrName)
        {
            txtSahu.Text = GstrValue;
        }

        private void ReExam_value(string argValue, string argName)
        {
            txtReExam.Text = argValue;
            txtReExam.Text = VB.Left(txtReExam.Text, txtReExam.Text.Length - 1);
        }

        private void Data_Save(string argJob)
        {
            int result = 0;

            HIC_SPC_SCODE item = panMain.GetData<HIC_SPC_SCODE>();

            if (argJob == "Save")
            {
                item.PANJENG = VB.Pstr(cboPanjeng.Text, ".", 1);

                if (item.RID.IsNullOrEmpty())
                {
                    result = hicSpcScodeService.Insert(item);
                }
                else
                {
                    result = hicSpcScodeService.Update(item);
                }
            }
            else if (argJob == "Delete")
            {
                item.DELDATE = DateTime.Now;
                result = hicSpcScodeService.Delete(item.RID, item.DELDATE);
            }

            if (result <= 0)
            {
                MessageBox.Show("Data 처리중 에러발생", "오류");
            }
            else
            {
                MessageBox.Show("처리완료", "확인");
            }

            Screen_Clear();
        }

        private void Screen_Clear()
        {
            panMain.Initialize();
            panCode.Initialize();
            panPan.Initialize();

            dtpDelDate.Checked = false;
            dtpDelDate.CustomFormat = " ";
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            cboPanjeng.SelectedIndex = 0;
            cboPanjeng2.SelectedIndex = 0;
            cboMCode.SelectedIndex = 0;
            cboDBun.SelectedIndex = 0;
            cboUpmu.SelectedIndex = 0;
        }
    }
}
