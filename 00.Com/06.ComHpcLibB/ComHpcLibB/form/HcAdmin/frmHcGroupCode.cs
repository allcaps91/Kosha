using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcGroupCode.cs
/// Description     : 건진센터 묶음코드 관리
/// Author          : 김민철
/// Create Date     : 2019-12-17
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmGroupCode(HcCode08.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcGroupCode : Form
    {
        HicGroupcodeService hicGroupcodeService = null;
        HicCodeService hicCodeservice = null;
        HicExjongService hicExjongService = null;
        HicMcodeService hicMcodeService = null;
        HicBcodeService hicBcodeService = null;

        public frmHcGroupCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            hicGroupcodeService = new HicGroupcodeService();
            hicCodeservice = new HicCodeService();
            hicExjongService = new HicExjongService();
            hicMcodeService = new HicMcodeService();
            hicBcodeService = new HicBcodeService();

            SS1.Initialize();
            SS1.AddColumn("코드",         nameof(HIC_GROUPCODE.CODE),     54, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS1.AddColumn("그룹코드명칭", nameof(HIC_GROUPCODE.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS1.AddColumn("종류",         nameof(HIC_GROUPCODE.JONG),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });            
            SS1.AddColumn("항목",         nameof(HIC_GROUPCODE.HANG),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("선택",         nameof(HIC_GROUPCODE.GBSELECT), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            //SS1.AddColumn("수가",         nameof(HIC_GROUPCODE.GBSUGA),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("물질",         nameof(HIC_GROUPCODE.UCODE),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            SS1.AddColumn("부담율",       nameof(HIC_GROUPCODE.GBSELF),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });

            dtpSDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_GROUPCODE.SDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpDelDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_GROUPCODE.DELDATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });

            //부담율
            cboBuRate.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.GBSELF) });
            cboBuRate.SetItems(hicCodeservice.FindOne("B4"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            //검진종류
            cboJong.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.JONG) });
            cboJong.SetItems(hicExjongService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJong1.Items.Add("**.전체");
            cboJong1.SetItems(hicExjongService.FindAll(), "NAME", "CODE");

            //항목
            cboHang.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.HANG) });
            cboHang.SetItems(hicCodeservice.FindOne("B9"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            //취급물질
            cboMCode.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.UCODE) });
            cboMCode.SetItems(hicMcodeService.FindAll(), "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            
            //2차재검사유코드
            List<HIC_CODE> list4 = hicCodeservice.FindOne("53");
            cboReExam1.SetItems(list4, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboReExam2.SetItems(list4, "NAME", "CODE");
            cboReExam3.SetItems(list4, "NAME", "CODE");
            cboReExam4.SetItems(list4, "NAME", "CODE");
            cboReExam5.SetItems(list4, "NAME", "CODE");
            cboReExam6.SetItems(list4, "NAME", "CODE");

            chkSelect.SetOptions(new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBSELECT), CheckValue = "Y", UnCheckValue = "" });
            chkDental.SetOptions(new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBDENT), CheckValue = "Y", UnCheckValue = "" });
            chkAddPan.SetOptions(new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBNOTADDPAN), CheckValue = "Y", UnCheckValue = "" });
            chkGbSunap.SetOptions(new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBSUNAP), CheckValue = "Y", UnCheckValue = "" });
            chkGubun1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_GROUPCODE.GBGUBUN1), CheckValue = "Y", UnCheckValue = "" });

            SS2.Initialize();
            SS2.AddColumn("S",            "",                       42, FpSpreadCellType.CheckBoxCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            SS2.AddColumn("문진항목명",   nameof(HIC_BCODE.NAME),  120, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS2.AddColumn("코드",         nameof(HIC_BCODE.CODE),   52, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Center });
            
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

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SS1)
            {
                HIC_GROUPCODE item = SS1.GetRowData(e.Row) as HIC_GROUPCODE;

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
                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(txtCode.Text.Trim());

                    if (item == null)
                    {
                        Screen_Clear();
                        MessageBox.Show("조회된 Data가 없음");
                        return;
                    }

                    Code_DisPlay(item);
                    
                }
            }
        }

        private void Code_DisPlay(HIC_GROUPCODE item)
        {
            Screen_Clear();

            if (item.IsNullOrEmpty()) { return; }

            panMain.SetData(item);

            //0,1,0,1,1,1,
            //암구분
            if (!item.GBAM.IsNullOrEmpty())
            {
                for (int i = 0; i < VB.L(item.GBAM, ",") - 1; i++)
                {
                    if (VB.Pstr(item.GBAM, ",", i + 1) == "1")
                    {
                        (Controls.Find("chkAm" + (i + 1).ToString(), true)[0] as CheckBox).Checked = true;
                    }
                }
            }

            //2차재검사유
            if (!item.REEXAM.IsNullOrEmpty())
            {
                for (int i = 1; i < VB.L(item.REEXAM, ",") - 1; i++)
                {
                    if (VB.Pstr(item.REEXAM, ",", i + 1) != "")
                    {
                        if ((Controls.Find("cboReExam" + (i + 1).ToString(), true)[0] as ComboBox).Items.Contains(VB.Pstr(item.REEXAM, ",", i + 1)))
                        {
                            (Controls.Find("cboReExam" + (i + 1).ToString(), true)[0] as ComboBox).SelectedIndex = 
                                (Controls.Find("cboReExam" + (i + 1).ToString(), true)[0] as ComboBox).FindStringExact(VB.Pstr(item.REEXAM, ",", i + 1));
                        }
                    }
                }
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

            //표적장기별 특수상담
            if (!item.GBSANGDAM.IsNullOrEmpty())
            {
                for (int i = 0; i < 20; i++)
                {
                    if (VB.Mid(item.GBSANGDAM, i, 1) == "1")
                    {
                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 2].Text.To<int>() == i)
                            {
                                SS2.ActiveSheet.Cells[j, 0].Value = "True";
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void Screen_Clear()
        {
            ComFunc.SetAllControlClearEx(panMain);
            SS2.DataSource = hicBcodeService.GetCodeNamebyGubun("HIC_표적장기별특수상담");
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSearch)
            {
                Data_Search(VB.Left(cboJong1.Text, 2));
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
        }

        private void Data_Save(string argJob)
        {
            int nVal = 0;
            int result = 0;
            string strChkAm = string.Empty;
            string strChkPrt = string.Empty;
            
            string strGbSangdam = "00000000000000000000";
            string[] arry_Sang = new string[] { strGbSangdam };

            HIC_GROUPCODE item = panMain.GetData<HIC_GROUPCODE>();

            if (argJob.Equals("Save"))
            {
                //암구분
                for (int i = 1; i < 8; i++)
                {
                    if ((Controls.Find("chkAm" + i.ToString(), true)[0] as CheckBox).Checked)
                    {
                        item.GBAM = item.GBAM + "1,";
                    }
                    else
                    {
                        item.GBAM = item.GBAM + "0,";
                    }
                }

                //접수증출력(가셔야할곳)
                for (int i = 1; i < 13; i++)
                {
                    if ((Controls.Find("chkPrt" + i.ToString(), true)[0] as CheckBox).Checked)
                    {
                        item.GBAM = item.GBPRINT + "1,";
                    }
                    else
                    {
                        item.GBAM = item.GBPRINT + "0,";
                    }
                }

                //표적장기별 특수상담
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nVal = SS2.ActiveSheet.Cells[i, 2].Text.To<int>() - 1;
                        if (nVal >= 0)
                        {
                            arry_Sang[i] = nVal.To<string>();
                        }
                    }
                }

                item.GBSANGDAM = string.Join("", arry_Sang);


                if (item.RID.IsNullOrEmpty())
                {
                    result = hicGroupcodeService.Insert(item);
                }
                else
                {
                    result = hicGroupcodeService.UpDate(item);
                }
            }
            else if (argJob.Equals("Delete"))
            {
                result = hicGroupcodeService.Delete(txtRowid.Text.Trim());
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

        private void Data_Search(string argJong)
        {
            SS1.DataSource = hicGroupcodeService.GetListByAll(argJong);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();
            SS2.DataSource = hicBcodeService.GetCodeNamebyGubun("HIC_표적장기별특수상담");

            cboJong1.SelectedIndex = 0;
        }
    }
}
