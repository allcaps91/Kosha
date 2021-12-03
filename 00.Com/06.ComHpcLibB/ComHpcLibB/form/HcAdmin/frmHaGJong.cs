using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaGJong.cs
/// Description     : 건진센터 검진종류 관리(종검)
/// Author          : 김민철
/// Create Date     : 2019-11-22
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmGemJongEntry(HaCode02.frm)" />
/// 
namespace ComHpcLibB
{
    public partial class frmHaGJong : Form
    {
        HicCodeService hcCodeService = null;
        HeaExjongService heaExjongService = null;

        public frmHaGJong()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            heaExjongService = new HeaExjongService();
            hcCodeService = new HicCodeService();

            SS1.Initialize();
            SS1.AddColumn("코드",        nameof(HEA_EXJONG.CODE),     42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("검진명칭",    nameof(HEA_EXJONG.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("분류",        nameof(HEA_EXJONG.BUN),      82, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });            
            SS1.AddColumn("부담율",      nameof(HEA_EXJONG.BURATE),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });
            SS1.AddColumn("변경",        nameof(HEA_EXJONG.BUCHANGE), 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center });

            chkBudam.SetOptions(  new CheckBoxOption    { DataField = nameof(HEA_EXJONG.BUCHANGE),  CheckValue = "Y", UnCheckValue = "" });
            
            cboBuRate.Items.Clear();
            cboBuRate.Items.Add("1.본인 100%");
            cboBuRate.Items.Add("2.회사 100%");
            cboBuRate.Items.Add("3.회사,본인 50%");

            List<HIC_CODE> list2 = hcCodeService.FindOne("24");
            cboInWon.SetItems(list2, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            cboBun.Items.Clear();
            cboBun.Items.Add("");
            cboBun.Items.Add("1.A형");
            cboBun.Items.Add("2.B형");
            cboBun.Items.Add("3.C형");
            cboBun.Items.Add("4.D형");
            cboBun.Items.Add("5.E형");
            cboBun.Items.Add("9.기타");
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

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SS1)
            {
                HEA_EXJONG item = SS1.GetRowData(e.Row) as HEA_EXJONG;
                panMain.SetData(item);
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode && e.KeyChar == (char)13)
            {
                if (txtCode.Text.Trim() != "")
                {
                    HEA_EXJONG item = heaExjongService.Read_ExJong_CodeName(txtCode.Text.Trim());

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

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                clsSpread cSpd = new clsSpread();
                cSpd.Spread_All_Clear(SS1);
                cSpd.Dispose();

                SS1.DataSource = heaExjongService.FindAll();
            }
            else if (sender == btnSave)
            {
                HEA_EXJONG item = panMain.GetData<HEA_EXJONG>();

                if (!panMain.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (!heaExjongService.DataCheck(item))
                {
                    return;
                }

                item.ENTDATE = DateTime.Now;
                item.ENTSABUN = Convert.ToInt32(clsType.User.IdNumber);

                if (item.RID == null || item.RID == "")
                {
                    int result = heaExjongService.Insert(item);

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
                    int result = heaExjongService.Update(item);

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
            else if (sender == btnExit)
            {
                this.Close();
            }
            else if (sender == btnDelete)
            {
                HEA_EXJONG item = panMain.GetData<HEA_EXJONG>();

                if (ComFunc.MsgBoxQ("검사항목을 삭제하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                int result = heaExjongService.Delete(item);

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

            #endregion
        }

        private void Screen_Clear()
        {
            ComFunc.SetAllControlClearEx(panMain);
        }
    }
}
