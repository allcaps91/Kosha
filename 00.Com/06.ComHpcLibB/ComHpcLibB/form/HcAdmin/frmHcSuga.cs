using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using ComLibB;
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
/// File Name       : frmHcSuga.cs
/// Description     : 검진수가 변환 및 변경
/// Author          : 김민철
/// Create Date     : 2020-01-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSuga(HcCode17.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcSuga : Form
    {
        private enum     enmHcSuga {  CHK01,  CODE,       HNAME,      SUCODE,     EXCODE,     BSUDATE,              SUGBA,       SUGBE,        BAMT,       NAMT,       SUDATE,         AMT1,       AMT2,       AMT3,       AMT4,       AMT5,       ERR1,   ERR2,   DELDATE,    RID }
        private string[] sHcSuga = { "선택", "검사코드", "검사명칭", "수가코드", "검사코드", "기준일자(본관기준)", "그룹수가",  "기술료가산", "보험수가", "변경수가", "수가변경일자", "종검수가", "공단수가", "특검수가", "조정수가", "임의수가", "수가", "검사", "삭제일자", "ROWID" };
        private int[]    nHcSuga = {  45,     42,         160,         82,         82,        82,                   44,          44,           82,         82,         82,             82,         82,         82,         82,         82,         44,     44,     92,         44 };

        clsSpread cSpd = null;
        HicExcodeService hicExcodeService = null;
        ViewSugaCodeService viewSugaCodeService = null;
        BasSugaAmtService basSugaAmtService = null;

        public frmHcSuga()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();

            hicExcodeService = new HicExcodeService();
            viewSugaCodeService = new ViewSugaCodeService();
            basSugaAmtService = new BasSugaAmtService();

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 42 });
            
            SS1.AddColumnCheckBox(sHcSuga[(int)enmHcSuga.CHK01], "", nHcSuga[(int)enmHcSuga.CHK01], new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false }).ButtonClick += SS1_BtnCliked;
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.CODE],     nameof(HIC_EXCODE.CODE),    nHcSuga[(int)enmHcSuga.CODE],    FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.HNAME],    nameof(HIC_EXCODE.HNAME),   nHcSuga[(int)enmHcSuga.HNAME],   FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.SUCODE],   nameof(HIC_EXCODE.SUCODE),  nHcSuga[(int)enmHcSuga.SUCODE],  FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.EXCODE],   nameof(HIC_EXCODE.EXCODE),  nHcSuga[(int)enmHcSuga.EXCODE],  FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.BSUDATE],  nameof(HIC_EXCODE.BSUDATE), nHcSuga[(int)enmHcSuga.BSUDATE], FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.SUGBA],    nameof(HIC_EXCODE.SUGBA),   nHcSuga[(int)enmHcSuga.SUGBE],   FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.SUGBE],    nameof(HIC_EXCODE.SUGBE),   nHcSuga[(int)enmHcSuga.SUGBE],   FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.BAMT],     nameof(HIC_EXCODE.BAMT),    nHcSuga[(int)enmHcSuga.BAMT],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.NAMT],     nameof(HIC_EXCODE.NAMT),    nHcSuga[(int)enmHcSuga.NAMT],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.SUDATE],   nameof(HIC_EXCODE.SUDATE),  nHcSuga[(int)enmHcSuga.SUDATE],  FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.AMT1],     nameof(HIC_EXCODE.AMT1),    nHcSuga[(int)enmHcSuga.AMT1],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.AMT2],     nameof(HIC_EXCODE.AMT2),    nHcSuga[(int)enmHcSuga.AMT2],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.AMT3],     nameof(HIC_EXCODE.AMT3),    nHcSuga[(int)enmHcSuga.AMT3],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.AMT4],     nameof(HIC_EXCODE.AMT4),    nHcSuga[(int)enmHcSuga.AMT4],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.AMT5],     nameof(HIC_EXCODE.AMT5),    nHcSuga[(int)enmHcSuga.AMT5],    FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.ERR1],     nameof(HIC_EXCODE.ERR1),    nHcSuga[(int)enmHcSuga.ERR1],    FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.ERR2],     nameof(HIC_EXCODE.ERR2),    nHcSuga[(int)enmHcSuga.ERR2],    FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.DELDATE],  nameof(HIC_EXCODE.DELDATE), nHcSuga[(int)enmHcSuga.DELDATE], FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn(sHcSuga[(int)enmHcSuga.RID],      nameof(HIC_EXCODE.RID),     nHcSuga[(int)enmHcSuga.RID],     FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            UnaryComparisonConditionalFormattingRule unary;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.Khaki;
            unary.ForeColor = Color.Blue;
            SS1.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcSuga.NAMT, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.LightCoral;
            SS1.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcSuga.ERR1, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.LightCoral;
            SS1.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcSuga.ERR2, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
            unary.ForeColor = Color.DarkRed;
            SS1.ActiveSheet.SetConditionalFormatting(-1, (int)enmHcSuga.DELDATE, unary);

        }

        private void SS1_BtnCliked(object sender, EditorNotifyEventArgs e)
        {
            if (SS1.ActiveSheet.Cells[e.Row, (int)enmHcSuga.CHK01].Text == "Y")
            {
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.PaleTurquoise;
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            }
            
        }

        private void ExCodeDelete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_EXCODE code = SS1.GetRowData(e.Row) as HIC_EXCODE;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnRun.Click       += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            if (e.Column == (int)enmHcSuga.SUCODE)
            {
                string strCode = SS1.ActiveSheet.Cells[e.Row, (int)enmHcSuga.SUCODE].Text.Trim();
                
                frmSugaEntry frm = new frmSugaEntry(strCode);
                frm.ShowDialog();
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnRun)
            {
                Accept_New_Amt();
            }
            else if (sender == btnSave)
            {
                if (MessageBox.Show(dtpBDate.Text + " 일자로 수가를 변경하시겠습니까?", "수가변경", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                Data_Save();
            }
        }

        private void Accept_New_Amt()
        {
            string strChk = string.Empty;
            string strOK = string.Empty;

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strOK = "NO";

                strChk = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.CHK01].Text;

                if (rdoJob1.Checked) { strOK = "OK"; }
                if (rdoJob2.Checked && strChk == "Y") { strOK = "OK"; }
                if (rdoJob3.Checked && strChk != "Y") { strOK = "OK"; }

                if (strOK.Equals("OK"))
                {
                    SS1.ActiveSheet.Cells[i, (int)enmHcSuga.NAMT].Text = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.BAMT].Text;
                }
            }
        }

        private void Data_Save()
        {
            int result = 0;
            string strChk = string.Empty;
            string strOK = string.Empty;
            string strCode = string.Empty;
            string strRowid = string.Empty;

            long nNSuga = 0;
            long nSuga_TO = 0, nSuga_HR = 0, nSuga_SPC = 0, nSuga_CHA = 0, nSuga_IMSI = 0;
            long nOldAmt1 = 0, nOldAmt2 = 0, nOldAmt3 = 0, nOldAmt4 = 0, nOldAmt5 = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                //변수초기화
                nSuga_TO = 0; nSuga_HR = 0; nSuga_SPC = 0; nSuga_CHA = 0; nSuga_IMSI = 0;
                nOldAmt1 = 0; nOldAmt2 = 0; nOldAmt3 = 0; nOldAmt4 = 0; nOldAmt5 = 0;

                strOK = "NO";

                strChk = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.CHK01].Text;

                //SS1.ActiveSheet.Cells[i, (int)enmHcSuga.CHK01].Text = "N";

                if (rdoJob1.Checked) { strOK = "OK"; }
                if (rdoJob2.Checked && strChk == "Y") { strOK = "OK"; }
                if (rdoJob3.Checked && strChk != "Y") { strOK = "OK"; }

                if (strOK.Equals("OK"))
                {
                    strCode = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.CODE].Text.Trim();
                    strRowid = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.RID].Text.Trim();

                    if (!strRowid.IsNullOrEmpty())
                    {
                        if (!hicExcodeService.SelectInsertHicExCodeByRid(strRowid))
                        {
                            MessageBox.Show("History 저장 실패!");
                        }
                    }

                    nNSuga      = VB.Replace(SS1.ActiveSheet.Cells[i, (int)enmHcSuga.NAMT].Text, ",", "").To<long>(0);  //New수가
                    //nSuga_TO    = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.AMT1].Text.To<long>(0);  //종검수가
                    //nSuga_HR    = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.AMT2].Text.To<long>(0);  //공단수가
                    //nSuga_SPC   = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.AMT3].Text.To<long>(0);  //특검수가
                    //nSuga_CHA   = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.AMT4].Text.To<long>(0);  //조정수가
                    //nSuga_IMSI  = SS1.ActiveSheet.Cells[i, (int)enmHcSuga.AMT5].Text.To<long>(0);  //임의수가

                    HIC_EXCODE item = hicExcodeService.FindOne(strCode);

                    if (!item.IsNullOrEmpty())
                    {
                        item.SUDATE5 = item.SUDATE4;
                        item.JAMT3 = item.JAMT2;
                        item.GAMT3 = item.GAMT2;
                        item.SAMT3 = item.SAMT2;
                        item.OAMT3 = item.OAMT2;
                        item.IAMT3 = item.IAMT2;

                        item.SUDATE4 = item.SUDATE3;
                        item.JAMT2 = item.JAMT1;
                        item.GAMT2 = item.GAMT1;
                        item.SAMT2 = item.SAMT1;
                        item.OAMT2 = item.OAMT1;
                        item.IAMT2 = item.IAMT1;

                        item.SUDATE3 = item.SUDATE2;
                        item.JAMT1 = item.OLDAMT1;
                        item.GAMT1 = item.OLDAMT2;
                        item.SAMT1 = item.OLDAMT3;
                        item.OAMT1 = item.OLDAMT4;
                        item.IAMT1 = item.OLDAMT5;

                        item.SUDATE2 = item.SUDATE;
                        item.OLDAMT1 = item.AMT1;
                        item.OLDAMT2 = item.AMT2;
                        item.OLDAMT3 = item.AMT3;
                        item.OLDAMT4 = item.AMT4;
                        item.OLDAMT5 = item.AMT5;

                        item.SUDATE = dtpBDate.Text;
                        item.AMT1 = nNSuga;
                        //item.AMT2 = nSuga_HR;
                        //item.AMT3 = nSuga_SPC;
                        //item.AMT4 = nSuga_CHA;
                        //item.AMT5 = nSuga_IMSI;

                        item.ENTSABUN = clsType.User.IdNumber.To<long>();
                    }

                    result = hicExcodeService.UpdateAmt(item);
                    if (result <= 0)
                    {
                        MessageBox.Show("수가 UPDATE중 오류발생.", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }
            }

            if (ComFunc.MsgBoxQ("수가를 정말로 변경하시겠습니까?", "수가변경", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                MessageBox.Show("수가를 변경을 취소했습니다.", "취소");
                clsDB.setRollbackTran(clsDB.DbCon);
            }
            else
            {
                MessageBox.Show("수가를 변경했습니다.", "변경");
                clsDB.setCommitTran(clsDB.DbCon);
            }

            Screen_Clear();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpBDate.Text = clsPublic.GstrSysDate;

            prgBar.Value = 0;

            //Screen_Display();
        }

        private void Screen_Clear()
        {
            txtSearch.Text = "";

            cSpd.Spread_Clear_Simple(SS1);
        }

        private void Screen_Display()
        {
            string strChkSuga = string.Empty;
            string strSuCode = string.Empty;
            string strExamCode = string.Empty;
            string strSuDateCode = string.Empty;
            bool bDel = false;

            long nBAmt = 0;
            long nSuAmt = 0;

            if (rdoSuga1.Checked)
            {
                strChkSuga = "HEA";
            }
            else if (rdoSuga2.Checked)
            {
                strChkSuga = "HIC";
            }
            else if (rdoSuga3.Checked)
            {
                strChkSuga = "SPC";
            }
            else if (rdoSuga4.Checked)
            {
                strChkSuga = "MED";
            }
            else if (rdoSuga5.Checked)
            {
                strChkSuga = "TMP";
            }
            else if (rdoSuga6.Checked)
            {
                strChkSuga = "ALL";
            }

            //수가코드매칭건
            if (chkSuCode.Checked) { strSuCode = "Y"; }
            //검사실코드매칭건
            if (chkExamSuga.Checked) { strExamCode = "Y"; }
            //적용일자이전코드
            if (chkSuDate.Checked) { strSuDateCode = dtpBDate.Text; }
            //삭제코드포함여부
            if (chkDel.Checked) { bDel = true; }

            Cursor.Current = Cursors.WaitCursor;

            List<HIC_EXCODE> list = hicExcodeService.GetSugaListByChkSuga(strChkSuga, strSuCode, strExamCode, strSuDateCode, bDel, txtSearch.Text.Trim());

            if (list.Count == 0)
            {
                MessageBox.Show("자료가 없습니다.", "확인");
                Cursor.Current = Cursors.Default;
                return;
            }

            Cursor.Current = Cursors.Default;

            SS1.ActiveSheet.RowCount = 0;
            SS1.DataSource = list;


        }

    }
}
