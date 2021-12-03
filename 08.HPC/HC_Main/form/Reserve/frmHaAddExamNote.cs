using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_Main.Dto;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaAddExamNote.cs
/// Description     : 추가검사관리노트
/// Author          : 김민철
/// Create Date     : 2020-05-20
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm선택검사관리노트(Frm추가검사관리노트.frm)" />
namespace HC_Main
{
    public partial class frmHaAddExamNote : Form
    {
        ComFunc CF = null;
        clsSpread cSpd = null;

        HeaJepsuResvExamService heaJepsuResvExamService = null;
        HeaResvMemoService heaResvMemoService = null;
        HeaResvExamService heaResvExamService = null;

        UnaryComparisonConditionalFormattingRule unary;

        public frmHaAddExamNote()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();
            heaJepsuResvExamService = new HeaJepsuResvExamService();
            heaResvMemoService = new HeaResvMemoService();
            heaResvExamService = new HeaResvExamService();
            SS1.Initialize(new SpreadOption { RowHeaderVisible = true, IsRowSelectColor = true, RowHeight = 24 });
            SS1.AddColumnDateTime("수검일자",    nameof(HEA_JEPSU_RESV_EXAM.SDATE),       92, IsReadOnly.Y, DateTimeType.YYYY_MM_DD);
            SS1.AddColumn("종류",                nameof(HEA_JEPSU_RESV_EXAM.GJJONG),      40, new SpreadCellTypeOption { });
            SS1.AddColumn("등록번호",            nameof(HEA_JEPSU_RESV_EXAM.PTNO),        84, new SpreadCellTypeOption { });
            SS1.AddColumn("수검자명",            nameof(HEA_JEPSU_RESV_EXAM.SNAME),       78, new SpreadCellTypeOption { });
            SS1.AddColumn("나이",                nameof(HEA_JEPSU_RESV_EXAM.A_SEX),       64, new SpreadCellTypeOption { });
            SS1.AddColumn("검사명",              nameof(HEA_JEPSU_RESV_EXAM.EXAMNAME),   180, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumnDateTime("선택검사일",  nameof(HEA_JEPSU_RESV_EXAM.RTIME),       92, IsReadOnly.Y, DateTimeType.YYYY_MM_DD);
            SS1.AddColumn("연락처",              nameof(HEA_JEPSU_RESV_EXAM.HPHONE),      92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumnDateTime("통보일자",    nameof(HEA_JEPSU_RESV_EXAM.TONGBODATE),  92, IsReadOnly.N, DateTimeType.YYYY_MM_DD);
            SS1.AddColumn("통보사번",            nameof(HEA_JEPSU_RESV_EXAM.TONGBOSABUN), 48, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("통보자",              nameof(HEA_JEPSU_RESV_EXAM.TONGBONAME),  84, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumnCheckBox("확인",        nameof(HEA_JEPSU_RESV_EXAM.CONFIRM),     42, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = true });
            SS1.AddColumn("RID",                 nameof(HEA_JEPSU_RESV_EXAM.RID),         44, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumnButton("메모",                                                   42, new SpreadCellTypeOption { ButtonText = "메모" }).ButtonClick += frmHaAddExamNote_ButtonClick;
            SS1.AddColumn("접수번호",            nameof(HEA_JEPSU_RESV_EXAM.WRTNO),       88, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            
            SS_Etc.Initialize(new SpreadOption { IsRowSelectColor = true, RowHeight = 24 });
            SS_Etc.AddColumnCheckBox("삭제", "", 47, new CheckBoxBooleanCellType { }).ButtonClick += frmHaAddExamNote_DelCheck; ;
            SS_Etc.AddColumnDateTime("수검일자",   nameof(HEA_RESV_MEMO.SDATE),      92, IsReadOnly.N, DateTimeType.YYYY_MM_DD);
            SS_Etc.AddColumn("수검자명",           nameof(HEA_RESV_MEMO.SNAME),      78, new SpreadCellTypeOption { });
            SS_Etc.AddColumn("검사명",             nameof(HEA_RESV_MEMO.EXAMENAME), 200, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS_Etc.AddColumn("면담사항",           nameof(HEA_RESV_MEMO.SANGDAM),   280, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS_Etc.AddColumnDateTime("검체제출일", nameof(HEA_RESV_MEMO.SUBDATE),    92, IsReadOnly.N, DateTimeType.YYYY_MM_DD);
            SS_Etc.AddColumn("전화번호",           nameof(HEA_RESV_MEMO.TEL),       180, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS_Etc.AddColumnCheckBox("통보자",     nameof(HEA_RESV_MEMO.CONFIRM),    42, new CheckBoxFlagEnumCellType<IsActive>() { });
            SS_Etc.AddColumn("ROWID",              nameof(HEA_RESV_MEMO.RID),        92, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsVisivle = false });

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.LightGray;
            SS1.ActiveSheet.SetConditionalFormatting(-1, 11, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = Color.LightGray;
            SS_Etc.ActiveSheet.SetConditionalFormatting(-1, 8, unary);
        }

        private void frmHaAddExamNote_DelCheck(object sender, EditorNotifyEventArgs e)
        {
            HEA_RESV_MEMO code = SS_Etc.GetRowData(e.Row) as HEA_RESV_MEMO;

            SS_Etc.DeleteRow(e.Row);
        }

        private void frmHaAddExamNote_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            string strPtno = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();

            frmHcMemo frm = new frmHcMemo(strPtno);
            frm.ShowDialog();
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnPrint.Click     += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string strFDate = dtpFDate.Text;
                string strTDate = CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1);

                // 1: 전체, 2: 미통보, 3: 통보
                int nCFM = rdoGbn2.Checked ? 2 : rdoGbn3.Checked ? 1 : 3;
                // 정렬> 1: 선택검사일,  2: 수검일자
                int nSort = rdoSort1.Checked ? 1 : 2;

                Screen_Display(strFDate, strTDate, nCFM, txtSName.Text, nSort);
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
        }

        private void Spread_Print()
        {
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void Screen_Display(string argFDate, string argTDate, int argCFM, string argSName = "", int nSort = 1)
        {
            //관리명단 조회
            List<HEA_JEPSU_RESV_EXAM> list = heaJepsuResvExamService.GetListBySDateCfmYN(argFDate, argTDate, argCFM, argSName, nSort);

            SS1.ActiveSheet.RowCount = 0;
            SS_Etc.ActiveSheet.RowCount = 0;

            if (list.Count > 0)
            {
                SS1.DataSource = list;
            }

            //선택검사 관리코멘트
            List<HEA_RESV_MEMO> list2 = heaResvMemoService.GetListByAll();

            if (list2.Count > 0)
            {
                SS_Etc.DataSource = list2;
            }

            SS_Etc.AddRows(5);
            SS_Etc.ActiveSheet.SetActiveCell(0, 0);
            SS_Etc.ShowActiveCell(VerticalPosition.Nearest, HorizontalPosition.Nearest);
            //SS_Etc.ActiveSheet.RowCount = SS_Etc.ActiveSheet.RowCount + 5;

        }

        private void Data_Save()
        {
            //Error Check
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                HEA_JEPSU_RESV_EXAM chk = SS1.GetRowData(i) as HEA_JEPSU_RESV_EXAM;

                if (!chk.IsNullOrEmpty())
                {
                    if (chk.CONFIRM == "Y" && chk.TONGBODATE.To<string>().Trim() == "")
                    {
                        clsPublic.GstrRetValue = (i + 1).To<string>() + " 번줄 ";
                        clsPublic.GstrRetValue += chk.SNAME + " 통보일자가 공란입니다. ";
                        MessageBox.Show(clsPublic.GstrRetValue, "확인");
                        return;
                    }
                }
            }

            //검사대상 통보일 저장
            IList<HEA_JEPSU_RESV_EXAM> list = SS1.GetEditbleData<HEA_JEPSU_RESV_EXAM>();

            if (list.Count > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (heaJepsuResvExamService.Save(list))
                {
                    MessageBox.Show("저장하였습니다");
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

            //메모변경
            IList<HEA_RESV_MEMO> list2 = SS_Etc.GetEditbleData<HEA_RESV_MEMO>();

            if (list2.Count > 0)
            {
                if (heaResvMemoService.Save(list2))
                {
                    MessageBox.Show("저장하였습니다");
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                }
            }

            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            // 1: 전체, 2: 미통보, 3: 통보
            int nCFM = rdoGbn2.Checked ? 2 : rdoGbn3.Checked ? 3 : 1;
            // 정렬> 1: 선택검사일,  2: 수검일자
            int nSort = rdoSort1.Checked ? 1 : 2;

            Screen_Display(strFDate, strTDate, nCFM, txtSName.Text, nSort);

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.AddDays(1).ToShortDateString();

            Screen_Clear();
        }

        private void Screen_Clear()
        {
            cSpd.Spread_Clear_Simple(SS1);
            cSpd.Spread_Clear_Simple(SS_Etc);
        }
    }
}
