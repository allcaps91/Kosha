using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcMCodeCode.cs
/// Description     : 취급물질별 관련 검사항목 설정(자동판정용)
/// Author          : 김민철
/// Create Date     : 2020-01-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmMCodeExam(hcCode19.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcMCodeExam : Form
    {
        clsSpread cSpd = null;
        HicCodeService hicCodeService = null;
        HicGroupexamExcodeService hicGroupexamExcodeService = null;
        HicExcodeService hicExcodeService = null;
        HicMcodeService hicMcodeService = null;
        ComHpcLibBService comHpcLibBService = null;

        string FstrCode = string.Empty;
        string FstrGbSuga = string.Empty;

        public frmHcMCodeExam()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();

            hicCodeService = new HicCodeService();
            hicGroupexamExcodeService = new HicGroupexamExcodeService();
            hicExcodeService = new HicExcodeService();
            hicMcodeService = new HicMcodeService();
            comHpcLibBService = new ComHpcLibBService();

            //검진종류
            cboPart2.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CODE.CODE) });
            cboPart2.SetItems(hicCodeService.FindOne("72"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            //유해인자
            cboUCode.SetOptions(new ComboBoxOption { DataField = nameof(HIC_CODE) });
            cboUCode.SetItems(hicCodeService.FindOne("09"), "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            //취급물질
            cboBun.Items.Clear();
            cboBun.Items.Add("01.소음");
            cboBun.Items.Add("02.이상기압");
            cboBun.Items.Add("03.분진(광물성)");
            cboBun.Items.Add("04.분진(석면)");
            cboBun.Items.Add("05.분진(기타)");
            cboBun.Items.Add("06.유기화합물");
            cboBun.Items.Add("07.금속(연)");
            cboBun.Items.Add("08.금속(수은)");
            cboBun.Items.Add("09.금속(크롬)");
            cboBun.Items.Add("10.금속(카드뮴)");
            cboBun.Items.Add("11.금속(기타금속)");
            cboBun.Items.Add("12.산,알카리,가스");
            cboBun.Items.Add("13.진동");
            cboBun.Items.Add("14.유해광선");
            cboBun.Items.Add("15.기타");
            cboBun.Items.Add("16.야간작업");
            
            SS3.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS3.AddColumn("물질코드",     nameof(HIC_MCODE.CODE),   68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS3.AddColumn("물질코드명",   nameof(HIC_MCODE.NAME),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS1.AddColumn("검사코드",       nameof(HIC_EXCODE.CODE),    68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, IsSort = true });
            SS1.AddColumn("검사명칭(한글)", nameof(HIC_EXCODE.HNAME),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });

            SS2.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            SS2.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;            
            SS2.AddColumn("검사코드",     nameof(COMHPC.EXCODE),   64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });            
            SS2.AddColumn("검사명(한글)", nameof(COMHPC.HNAME),   280, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left });
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnCancel.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
            this.SS2.ButtonClicked   += new EditorNotifyEventHandler(eSpdDelete);
            this.SS3.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        private void eSpdDelete(object sender, EditorNotifyEventArgs e)
        {
            SS2.DeleteRow(e.Row);
        }

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            if (sender == SS1)
            {
                string strExCode = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                string strCode = string.Empty;

                //묶음항목에 자료가 있는지 Check
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCode = SS2.ActiveSheet.Cells[i, 1].Text.Trim();

                    if (strExCode == strCode)
                    {
                        clsPublic.GstrMsgList = i.To<string>() + " 번줄에 이미 " + strExCode + " 코드가 있습니다.";
                        MessageBox.Show(clsPublic.GstrMsgList, "확인");
                        return;
                    }
                }

                //검사명을 READ
                HIC_EXCODE item = hicExcodeService.FindOne(strExCode);

                if (!item.IsNullOrEmpty())
                {
                    int nRow = SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Style) + 1;

                    nRow += 1;

                    if (SS2.ActiveSheet.RowCount < nRow)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }

                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = item.CODE;
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = item.HNAME;
                    SS2.ActiveSheet.SetActiveCell(nRow - 1, 1);
                }
                else
                {
                    MessageBox.Show("검사코드가 없습니다.", "오류");
                    return;
                }
            }
            else if (sender == SS3)
            {
                FstrCode = SS3.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                ComFunc.SetAllControlClearEx(panSub03);

                HIC_MCODE item = hicMcodeService.GetItemByCode(FstrCode);

                if (!item.IsNullOrEmpty())
                {
                    FstrCode = item.CODE.Trim();
                    panSub03.SetData(item);
                }

                cSpd.Spread_Clear_Simple(SS2);

                //검사항목 Display
                SS2.DataSource = null;
                SS2.DataSource = comHpcLibBService.GetExCodeHNameByMCode(FstrCode);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSearch)
            {
                Data_Search_Part(VB.Left(cboPart2.Text, 1));
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            //저장
            else if (sender == btnSave)
            {
                Data_Save();
            }
        }

        private void Data_Search_Part(string argCode)
        {
            if (argCode.IsNullOrEmpty()) { return; }

            SS1.DataSource = hicExcodeService.GetCodebyPart(argCode);
        }

        private void Data_Save()
        {
            int result = 0;
            string strGbDel = string.Empty;
            string strExCode = string.Empty;
            int nTongBun = 0;

            try
            {
                nTongBun = VB.Left(cboBun.Text.Trim(), 2).To<int>();

                //기존에 등록된 내용을 삭제함
                result = comHpcLibBService.ComDeleteByUseType("HIC_SPC_MCodeExam", "CODE", txtCode.Text.Trim());
                if (result <= 0)
                {
                    MessageBox.Show("Data 삭제 시 오류발생. 작업실패!", "오류");
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                int nRow = SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Style);

                for (int i = 0; i < nRow; i++)
                {
                    strGbDel = SS2.ActiveSheet.Cells[i, 0].Text;
                    strExCode = SS2.ActiveSheet.Cells[i, 1].Text;

                    if (strGbDel != "True")
                    {
                        result = comHpcLibBService.HicSPcMcodeExamInsert(txtCode.Text.Trim(), strExCode);

                        if (result <= 0)
                        {
                            MessageBox.Show("작업도중 오류발생. 작업불가!", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                //마스타에 코드명,유해인자,분류를 Update
                HIC_MCODE item = new HIC_MCODE
                {
                    CODE = txtCode.Text.Trim(),
                    NAME = VB.Pstr(cboUCode.Text.Trim(), ".", 2),
                    TONGBUN = nTongBun
                };

                result = hicMcodeService.UpDateSpcExamInfoByCode(item);

                if (result <= 0)
                {
                    MessageBox.Show("작업도중 오류발생. 작업불가!", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Screen_Clear();
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            cboPart2.SelectedIndex = 0;

            MCode_Display();
        }

        private void MCode_Display()
        {
            SS3.DataSource = hicMcodeService.FindAll();
        }

        private void Screen_Clear()
        {
            FstrCode = "";
            FstrGbSuga = "";

            ComFunc.SetAllControlClearEx(panSub03);
            ComFunc.SetAllControlClearEx(panSub04);
        }
    }
}
