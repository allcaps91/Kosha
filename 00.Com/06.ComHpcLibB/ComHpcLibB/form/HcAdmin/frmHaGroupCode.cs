using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaGroupCode.cs
/// Description     : 종검 묶음코드 관리
/// Author          : 김민철
/// Create Date     : 2020-03-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGroupCode(HaCode03.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaGroupCode : Form
    {
        HeaExjongService heaExjongService = null;
        HeaGroupcodeService heaGroupcodeService = null;
        HIC_LTD LtdHelpItem = null;

        public frmHaGroupCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            heaExjongService = new HeaExjongService();
            heaGroupcodeService = new HeaGroupcodeService();
            LtdHelpItem = new HIC_LTD();

            SS1.Initialize();
            SS1.AddColumn("회사코드",     nameof(HEA_GROUPCODE.V_LTDCODE), 54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS1.AddColumn("회사명칭",     nameof(HEA_GROUPCODE.LTDNAME),  160, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("코드",         nameof(HEA_GROUPCODE.CODE),      42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true });            
            SS1.AddColumn("묶음코드명칭", nameof(HEA_GROUPCODE.NAME),      42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("종류",         nameof(HEA_GROUPCODE.JONG),      42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true });            
            SS1.AddColumn("성별",         nameof(HEA_GROUPCODE.GBSEX),     42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("선택",         nameof(HEA_GROUPCODE.GBSELECT),  42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("부담",         nameof(HEA_GROUPCODE.BURATE),    42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("변경",         nameof(HEA_GROUPCODE.GBSELF),    42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("감액",         nameof(HEA_GROUPCODE.GBGAM),     42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("현재수가",     nameof(HEA_GROUPCODE.AMT),       42, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn("변경일자",     nameof(HEA_GROUPCODE.SUDATE),    42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("종전수가",     nameof(HEA_GROUPCODE.OLDAMT),    42, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsVisivle = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn("유형",         nameof(HEA_GROUPCODE.TYPE),      42, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsVisivle = false });

            SS1.AddColumn("회사코드",     nameof(HEA_GROUPCODE.LTDCODE),   54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("약어",         nameof(HEA_GROUPCODE.YNAME),     54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("수면코드",     nameof(HEA_GROUPCODE.ENDOCODE),  54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("계약일자",     nameof(HEA_GROUPCODE.SDATE),     54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("삭제일자",     nameof(HEA_GROUPCODE.DELDATE),   54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("접수증출력",   nameof(HEA_GROUPCODE.EXAMNAME),  54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("가셔야할곳",   nameof(HEA_GROUPCODE.GBPRINT),   54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });
            SS1.AddColumn("ROWID",        nameof(HEA_GROUPCODE.RID),       54, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = true, IsVisivle = false });

            cboJong.Items.Add("**.전체");
            cboJong.SetOptions(new ComboBoxOption { DataField = nameof(HEA_GROUPCODE.JONG) });
            cboJong.SetItems(heaExjongService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJong1.Items.Add("**.전체");
            cboJong1.SetItems(heaExjongService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboSex.Items.Clear();
            cboSex.Items.Add("M.남자");
            cboSex.Items.Add("F.여자");
            cboSex.Items.Add("*.공통");

            cboBuRate.Items.Clear();
            cboBuRate.Items.Add("1.본인100%");
            cboBuRate.Items.Add("2.회사100%");
            cboBuRate.Items.Add("3.회사,본인50%");

            cboType.Items.Clear();
            cboType.Items.Add("");
            cboType.Items.Add("01.1형");
            cboType.Items.Add("02.2형");
            cboType.Items.Add("03.3형");
            cboType.Items.Add("04.4형");
            cboType.Items.Add("05.5형");
            cboType.Items.Add("06.6형");
            cboType.Items.Add("07.7형");
            cboType.Items.Add("08.8형");
            cboType.Items.Add("09.9형");

            cboEndo.SetOptions(new ComboBoxOption { DataField = nameof(HEA_GROUPCODE.ENDOCODE) });
            cboEndo.SetItems(heaGroupcodeService.GetCodeNameByLikeName("수면"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            dtpSDate.SetOptions(new DateTimePickerOption { DataField = nameof(HEA_GROUPCODE.SDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpSuDate.SetOptions(new DateTimePickerOption { DataField = nameof(HEA_GROUPCODE.SUDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpDelDate.SetOptions(new DateTimePickerOption { DataField = nameof(HEA_GROUPCODE.DELDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });

            chkSelect.SetOptions(new CheckBoxOption { DataField = nameof(HEA_GROUPCODE.GBSELECT), CheckValue = "Y", UnCheckValue = "N" });
            chkExcHalin.SetOptions(new CheckBoxOption { DataField = nameof(HEA_GROUPCODE.GBGAM), CheckValue = "Y", UnCheckValue = "N" });
            chkGbSelf.SetOptions(new CheckBoxOption { DataField = nameof(HEA_GROUPCODE.GBSELF), CheckValue = "Y", UnCheckValue = "N" });
            
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnDelete.Click     += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click    += new EventHandler(eBtnClick);
            this.txtCode.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SS1)
            {
                HEA_GROUPCODE item = SS1.GetRowData(e.Row) as HEA_GROUPCODE;

                if (item == null)
                {
                    Screen_Clear();
                    MessageBox.Show("조회된 Data가 없음");
                    return;
                }

                Code_DisPlay(item);
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode && e.KeyChar == (char)13)
            {
                if (txtCode.Text.Trim() != "")
                {
                    HEA_GROUPCODE item = heaGroupcodeService.GetItemByCode(txtCode.Text.Trim());

                    if (item == null)
                    {
                        Screen_Clear();
                        MessageBox.Show("조회된 Data가 없음");
                        return;
                    }

                    Code_DisPlay(item);
                }
            }
            else if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void Code_DisPlay(HEA_GROUPCODE item)
        {
            Screen_Clear();

            if (item.IsNullOrEmpty()) { return; }

            panMain.SetData(item);

            if (!item.IsNullOrEmpty())
            {
                txtLtdName.Text = item.LTDCODE + "." + item.LTDNAME;
            }

            //접수증출력구분(가셔야 할곳)
            if (!item.GBPRINT.IsNullOrEmpty())
            {
                for (int i = 1; i < VB.L(item.GBPRINT, ",") - 1; i++)
                {
                    if (VB.Pstr(item.GBPRINT, ",", i + 1) == "1")
                    {
                        (Controls.Find("chkPrt" + (i + 1).ToString(), true)[0] as CheckBox).Checked = true;
                    }
                }
            }

        }

        private void Screen_Clear()
        {
            ComFunc.SetAllControlClearEx(panMain);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSearch)
            {
                Data_Search(VB.Left(cboJong1.Text, 2), txtCode.Text.To<long>(), chkDel.Checked);
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            //저장
            else if (sender == btnSave)
            {
                Data_Save("Save");
            }
            //삭제
            else if (sender == btnDelete)
            {
                Data_Save("Delete");
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
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

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Data_Save(string argJob)
        {
            int nVal = 0;
            int result = 0;
            string strChkAm = string.Empty;
            string strChkPrt = string.Empty;
            
            string strGbSangdam = "00000000000000000000";
            string[] arry_Sang = new string[] { strGbSangdam };

            HEA_GROUPCODE item = panMain.GetData<HEA_GROUPCODE>();

            if (argJob.Equals("Save"))
            {
                //접수증출력(가셔야할곳)
                for (int i = 1; i < 13; i++)
                {
                    if ((Controls.Find("chkPrt" + i.ToString(), true)[0] as CheckBox).Checked)
                    {
                        item.GBPRINT = item.GBPRINT + "1,";
                    }
                    else
                    {
                        item.GBPRINT = item.GBPRINT + "0,";
                    }
                }

                //Data Save Check Logic
                string strErrMsg = heaGroupcodeService.CheckData(item);

                if (!strErrMsg.IsNullOrEmpty())
                {
                    MessageBox.Show(strErrMsg, "오류");
                    return;
                }

                if (item.RID.IsNullOrEmpty())
                {
                    result = heaGroupcodeService.Insert(item);
                }
                else
                {
                    result = heaGroupcodeService.UpDate(item);
                }
            }
            else if (argJob.Equals("Delete"))
            {
                result = heaGroupcodeService.Delete(txtRowid.Text.Trim());
            }

            if (result < 0)
            {
                MessageBox.Show("Data 저장 시 오류발생!", "전산정보팀 연락요망");
                Screen_Clear();
                return;
            }

            MessageBox.Show("작업완료!!", "확인");
            Screen_Clear();
        }

        private void Data_Search(string argJong, long argLtdCode, bool bDel)
        {
            SS1.DataSource = heaGroupcodeService.GetListByJongLtdCode(argJong, argLtdCode, bDel);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
        }
    }
}
