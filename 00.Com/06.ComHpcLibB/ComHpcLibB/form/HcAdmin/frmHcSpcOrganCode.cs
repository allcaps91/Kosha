using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSpcOrganCode.cs
/// Description     : 특수검진 표적장기 코드
/// Author          : 김민철
/// Create Date     : 2020-03-16
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmSpcOrganCode(HcCode36.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcSpcOrganCode : Form
    {
        clsSpread cSpd = null;
        HicOrgancodeService hicOrgancodeService = null;

        string FstrRowid = string.Empty;

        public frmHcSpcOrganCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicOrgancodeService = new HicOrgancodeService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS1.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;
            SS1.AddColumn("코드",       nameof(HIC_ORGANCODE.CODE),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("표적장기명", nameof(HIC_ORGANCODE.NAME), 160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS1.AddColumn("ROWID",      nameof(HIC_ORGANCODE.RID),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });

            SS2.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SS2.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += eSpdDelete;
            SS2.AddColumn("사유코드",   nameof(HIC_ORGANCODE.SAYUCODE),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS2.AddColumn("선정사유",   nameof(HIC_ORGANCODE.SAYUNAME), 160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsSort = true });
            SS2.AddColumn("ROWID",      nameof(HIC_ORGANCODE.RID),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Center, IsVisivle = false });
        }

        private void eSpdDelete(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SS1)
            {
                HIC_ORGANCODE code = SS1.GetRowData(e.Row) as HIC_ORGANCODE;
                SS1.DeleteRow(e.Row);
            }
            else if (sender == SS2)
            {
                HIC_ORGANCODE code = SS2.GetRowData(e.Row) as HIC_ORGANCODE;
                SS2.DeleteRow(e.Row);
            }
        }

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.btnExit.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click     += new EventHandler(eBtnClick);
            this.btnSave.Click       += new EventHandler(eBtnClick);
            this.btnSave2.Click      += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strCode = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            string strName = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();

            lblCode.Text = strCode + "." + strName;

            cSpd.Spread_Clear_Simple(SS2);

            SS2.DataSource = hicOrgancodeService.GetListAll("2", strCode);
            SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + 10;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Data_Search();
            }
            else if (sender == btnSave)
            {
                Data_Save("SS1");
            }
            else if (sender == btnSave2)
            {
                Data_Save("SS2");
            }
        }

        private void Data_Save(string argJob)
        {
            int result = 0;
            string strChk = string.Empty;
            string strCODE = string.Empty;
            string strName = string.Empty;
            string strSayuCode = string.Empty;
            string strSayuname = string.Empty;
            string strROWID = string.Empty;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                int nRow = SS1.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Style);

                for (int i = 0; i < nRow; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    strROWID = SS1.ActiveSheet.Cells[i, 3].Text;

                    if (strChk != "True")
                    {
                        if (strROWID != "")
                        {
                            result = hicOrgancodeService.DeleteByRowid(strROWID);
                        }

                        if (result <= 0)
                        {
                            MessageBox.Show("작업도중 오류발생. 작업불가!", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    else
                    {
                        if (argJob == "SS1")
                        {
                            strCODE = SS1.ActiveSheet.Cells[i, 1].Text;
                            strName = SS1.ActiveSheet.Cells[i, 2].Text;

                            if (strCODE != "")
                            {
                                FstrRowid = hicOrgancodeService.GetRowidByCode(strCODE);

                                HIC_ORGANCODE item = new HIC_ORGANCODE
                                {
                                    CODE = strCODE,
                                    NAME = strName,
                                    GUBUN = "1"
                                };

                                if (FstrRowid == "")
                                {
                                    result = hicOrgancodeService.Data_InSert(item);
                                }
                                else
                                {
                                    result = hicOrgancodeService.Data_UpDate(item);
                                }

                                if (result <= 0)
                                {
                                    MessageBox.Show("작업도중 오류발생. 작업불가!", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    return;
                                }
                            }
                        }
                        else if (argJob == "SS2")
                        {
                            strSayuCode = SS2.ActiveSheet.Cells[i, 1].Text;
                            strSayuname = SS2.ActiveSheet.Cells[i, 2].Text;
                            strCODE = VB.Left(lblCode.Text, 2);
                            strName = VB.Pstr(lblCode.Text, ".", 2);

                            if (strCODE != "")
                            {
                                FstrRowid = hicOrgancodeService.GetRowidBySayuCode(strSayuCode, strCODE);

                                HIC_ORGANCODE item = new HIC_ORGANCODE
                                {
                                    CODE = strCODE,
                                    NAME = strName,
                                    SAYUCODE = strSayuCode,
                                    SAYUNAME = strSayuname,
                                    GUBUN = "2"
                                };

                                if (FstrRowid == "")
                                {
                                    result = hicOrgancodeService.Data_InSert(item);
                                }
                                else
                                {
                                    result = hicOrgancodeService.Data_UpDate(item);
                                }

                                if (result <= 0)
                                {
                                    MessageBox.Show("작업도중 오류발생. 작업불가!", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    return;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("작업완료!", "확인");

                if (argJob == "SS2")
                {
                    cSpd.Spread_Clear_Simple(SS2);

                    SS2.DataSource = hicOrgancodeService.GetListAll("2", VB.Pstr(lblCode.Text, ".", 1));
                    SS2.ActiveSheet.RowCount = SS2.ActiveSheet.RowCount + 10;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void Data_Search()
        {
            SS1.DataSource = hicOrgancodeService.GetListAll("1");
            SS1.ActiveSheet.RowCount = SS1.ActiveSheet.RowCount + 10;
        }
    }
}
