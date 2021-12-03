using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcMemo.cs
/// Description     : 종검수검자 메모관리
/// Author          : 김민철
/// Create Date     : 2020-03-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm메모(Frm메모.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcMemo : Form
    {
        BasPcconfigService basPcconfigService = null;
        HicMemoService hicMemoService = null;
        HicPatientService hicPatientService = null;
        ComHpcLibBService comHpcLibBService = null;
        UnaryComparisonConditionalFormattingRule unary;

        clsSpread cSpd = null;
        ComFunc CF = null;
        clsHaBase hb = new clsHaBase();

        long FnPano = 0;
        string FstrPtno = string.Empty;

        public frmHcMemo()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcMemo(string argPtno)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FstrPtno = argPtno;
        }

        private void SetControl()
        {
            basPcconfigService = new BasPcconfigService();
            hicMemoService = new HicMemoService();
            hicPatientService = new HicPatientService();
            comHpcLibBService = new ComHpcLibBService();

            cSpd = new clsSpread();
            CF = new ComFunc();

            SSList.Initialize(new SpreadOption { RowHeight = 28 });
            SSList.AddColumn("검진구분", nameof(COMHPC.GBN),       42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진년도", nameof(COMHPC.GJYEAR),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진일자", nameof(COMHPC.SDATE),     78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진코드", nameof(COMHPC.GJJONG),    64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("검진종류", nameof(COMHPC.JONGNAME),  92, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("접수번호", nameof(COMHPC.WRTNO),     74, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "일반", false);
            unary.BackColor = Color.DarkSeaGreen;
            SSList.ActiveSheet.SetConditionalFormatting(-1, 0, unary);

            SS1.Initialize(new SpreadOption { RowHeight = 28, RowHeightAuto = true });
            SS1.AddColumnButton("삭제",       48, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += HeaMemoDelete_ButtonClick;
            SS1.AddColumn("구분",         nameof(HIC_MEMO.JOBGBN),    42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("입력시각",     nameof(HIC_MEMO.ENTTIME),  120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("내용",         nameof(HIC_MEMO.MEMO),     380, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsMulti = true });
            SS1.AddColumn("작업사번",     nameof(HIC_MEMO.JOBSABUN),  64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("작업자명",     nameof(HIC_MEMO.JOBNAME),   78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("검진번호",     nameof(HIC_MEMO.PANO),      78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("등록번호",     nameof(HIC_MEMO.PTNO),      78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("변경",         "",                         48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS1.AddColumn("ROWID",        nameof(HIC_MEMO.RID),       48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
        }

        private void HeaMemoDelete_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_MEMO code = SS1.GetRowData(e.Row) as HIC_MEMO;

            SS1.DeleteRow(e.Row);
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.EditModeOff += new EventHandler(eSpdEditOff);
            this.txtPtno.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.rdoGubun1.Click += new EventHandler(eRdoClick);
            this.rdoGubun2.Click += new EventHandler(eRdoClick);
            this.rdoGubun3.Click += new EventHandler(eRdoClick);

        }

        private void eRdoClick(object sender, EventArgs e)
        {
            if (clsHcVariable.GnHicLicense > 0)
            {
                if (sender == rdoGubun1 || sender == rdoGubun2 || sender == rdoGubun3)
                {
                    Screen_Display();
                }
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                txtPtno.Text = VB.Format(VB.Val(txtPtno.Text), "00000000");
                FstrPtno = txtPtno.Text;
                Screen_List();
                Screen_Display();
            }
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            if (sender == SS1)
            {
                int nRow = SS1.ActiveSheet.ActiveRowIndex;
                int nCol = SS1.ActiveSheet.ActiveColumnIndex;

                if (nCol == 3)
                {
                    Size size = SS1.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                    SS1.ActiveSheet.Rows[nRow].Height = size.Height;

                    if (SS1.ActiveSheet.Cells[nRow, 8].Text == "")
                    {
                        SS1.ActiveSheet.Cells[nRow, 8].Text = "Y";
                        cSpd.setSpdForeColor(SS1, nRow, 0, nRow, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                    }

                    SS1.ActiveSheet.Cells[nRow, 4].Text = clsType.User.IdNumber;
                    SS1.ActiveSheet.Cells[nRow, 6].Text = FnPano.To<string>("0");
                    SS1.ActiveSheet.Cells[nRow, 7].Text = FstrPtno;
                }
            }
        }

        private void eFormload(object sender, EventArgs e)
        {

            //판정의사 여부를 읽음
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());
            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");

            if (clsHcVariable.GnHicLicense > 0)
            {
                rdoGubun2.Checked = true;
            }
            else
            {
                rdoGubun3.Checked = true;
            }


            if (clsHcVariable.GstrHicPart == "1")
            {
                rdoJob2.Checked = true;
            }
            else
            {
                rdoJob1.Checked = true;
            }

            Screen_Clear();
            Screen_List();
            Screen_Display();
        }

        private void Screen_Clear()
        {
            txtPtno.Text = "";
            txtSName.Text = "";
            txtAge.Text = "";
            txtLtdName.Text = "";
        }

        private void Screen_List()
        {
            SSList.DataSource = comHpcLibBService.GetListJepListByPtno(FstrPtno);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
        }

        private void Data_Save()
        {
            string strJob = rdoJob1.Checked ? "일반" : "종검";
            string strGubun = "";
            if (clsHcVariable.GnHicLicense > 0)
            {
                strGubun = "Y";
            }

            IList<HIC_MEMO> list = SS1.GetEditbleData<HIC_MEMO>();

            if (list.Count > 0)
            {
                if (hicMemoService.Save(list, strJob, strGubun))
                {
                    MessageBox.Show("저장하였습니다");
                    Screen_Display();
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                }

                cSpd.Spread_All_Clear(SS1);

                Screen_Display();
            }

            eBtnClick(btnExit, new EventArgs());
        }

        private void Screen_Display()
        {
            string strGubun = "";

            if (FstrPtno.IsNullOrEmpty())
            {
                return;
            }

            HIC_PATIENT item = hicPatientService.GetPatInfoByPtno(FstrPtno);

            panSub01.SetData(item);

            FnPano = item.PANO;
            FstrPtno = item.PTNO;

            if (rdoGubun2.Checked == true)
            {
                strGubun = "D";
            }
            else if (rdoGubun3.Checked == true)
            {
                strGubun = "N";
            }
           
            List<HIC_MEMO> List = hicMemoService.GetItembyPaNo(FstrPtno, strGubun);

            SS1.DataSource = List;

            for (int i = 0; i < List.Count; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 2);
                SS1.ActiveSheet.Rows[i].Height = size.Height;
            }

            SS1.AddRows(5);

            SS1.ShowRow(0, 0, VerticalPosition.Top);
        }
    }
}
