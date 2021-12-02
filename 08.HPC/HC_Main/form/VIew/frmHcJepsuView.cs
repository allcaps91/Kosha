using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

// TODO : frmHaJepsuView 와 합치기

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcJepsuView.cs
/// Description     : 검진접수 List 조회
/// Author          : 김민철
/// Create Date     : 2019-09-02
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmJepsuView(HcMain84.frm)" />
namespace HC_Main
{
    public partial class frmHcJepsuView : Form
    {
        clsHcMain       cHM             = null;
        clsHaBase       cHB             = null;
        clsSpread       cSpd            = null;
        HIC_LTD         LtdHelpItem     = null;

        HicLtdService   hicLtdService   = null;
        HicJepsuService hicJepsuService = null;

        Thread          thread          = null;
        FpSpread        spd             = null;

        public delegate void SetGstrValue(HIC_JEPSU GstrValue);
        public static event SetGstrValue rSetGstrValue;

        long nLtdCode = 0;
        string FstrJong = string.Empty;
        string FstrSName = string.Empty;
        string FstrFDate = string.Empty;
        string FstrTDate = string.Empty;
        bool FbChul = false;
        bool FbJongGum = false;
        bool FbKaTalk = false;
        bool FbDel = false;
        bool FnEndo = false;
        bool bShow = false;
        bool FnHabit = false;

        public frmHcJepsuView()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetControl()
        {

            cHM             = new clsHcMain();
            cHB             = new clsHaBase();
            cSpd            = new clsSpread();
            LtdHelpItem     = new HIC_LTD();
            hicLtdService   = new HicLtdService();
            hicJepsuService = new HicJepsuService();

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong_AddItem(cboJong, true);

            //SSList.Initialize();
            SSList.Initialize(new SpreadOption { RowHeaderVisible = true });
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 38;
            SSList.AddColumn("내시경구분",       nameof(HIC_JEPSU.ENDOGBN),      92, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("수납점검",         nameof(HIC_JEPSU.CHKSUNAP),     38, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false  });
            SSList.AddColumn("내시경결과입력",   nameof(HIC_JEPSU.ENDORESULT),   84, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false  });
            SSList.AddColumn("접수일자",         nameof(HIC_JEPSU.JEPDATE),      94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true, dateTimeType = DateTimeType.YYYY_MM_DD });
            SSList.AddColumn("검진종류",         nameof(HIC_JEPSU.NAME),         94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("등록번호",         nameof(HIC_JEPSU.PTNO),         92, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("출장여부",         nameof(HIC_JEPSU.GBCHUL),       34, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, });
            SSList.AddColumn("수검자명",         nameof(HIC_JEPSU.SNAME),        84, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("나이",             nameof(HIC_JEPSU.AGE),          34, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, });
            SSList.AddColumn("성별",             nameof(HIC_JEPSU.SEX),          34, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, });
            SSList.AddColumn("접수번호",         nameof(HIC_JEPSU.WRTNO),        78, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true });
            SSList.AddColumn("그룹접수번호",     nameof(HIC_JEPSU.GWRTNO),       78, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsVisivle = false, IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("사업장명",         nameof(HIC_JEPSU.LTDNAME),      94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("보건소기호",       nameof(HIC_JEPSU.BOGUNSO),      84, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("부서명",           nameof(HIC_JEPSU.BUSENAME),     84, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("주민번호",         nameof(HIC_JEPSU.JUMINNO),     120, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("선택검사",        nameof(HIC_JEPSU.SEXAMS),       130, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("유해물질코드",     nameof(HIC_JEPSU.UCODES),      130, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumn("종검유무",         nameof(HIC_JEPSU.JONGGUMYN),    34, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, });
            SSList.AddColumn("검진코드",         nameof(HIC_JEPSU.GJJONG),       34, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("취소일자",         nameof(HIC_JEPSU.DELDATE),      84, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, dateTimeType = DateTimeType.YYYY_MM_DD });
            SSList.AddColumn("ROWID",            nameof(HIC_JEPSU.RID),          94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("카카오톡결과신청", nameof(HIC_JEPSU.WEBPRINTREQ),  94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, dateTimeType = DateTimeType.YYYY_MM_DD });
            SSList.AddColumn("개인정보동의일자", nameof(HIC_JEPSU.PRIVACY_DATE), 94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, dateTimeType = DateTimeType.YYYY_MM_DD });
            SSList.AddColumn("인터넷문진여부",   nameof(HIC_JEPSU.GBIEMUN),      44, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, });
            SSList.AddColumn("인터넷문진번호",   nameof(HIC_JEPSU.IEMUNNO),      94, FpSpreadCellType.TextCellType,  new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            UnaryComparisonConditionalFormattingRule unary;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(215, 255, 235);
            SSList.ActiveSheet.SetConditionalFormatting(-1, 0, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.FromArgb(255, 215, 235);
            SSList.ActiveSheet.SetConditionalFormatting(-1, 1, unary);
        }

        private void SetEvents()
        {
            this.Load                   += new EventHandler(eFormload);
            this.Activated              += new EventHandler(eActivated);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click       += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.SSList.CellClick       += new CellClickEventHandler(eSpdClick);
            this.txtSName.KeyPress      += new KeyPressEventHandler(eKeyPress);
        }

        private void eActivated(object sender, EventArgs e)
        {
            if (bShow) { bShow = false; return; }

            //화면갱신안함
            if (chkResv.Checked)
            {
                cboJong.SelectedIndex = 0;
                txtLtdName.Text = "";
                txtSName.Text = "";
                chkChul.Checked = false;
                chkJongGum.Checked = false;
                chkKaTalk.Checked = false;
                chkDel.Checked = false;
                chkEndo.Checked = false; 
                chkHabit.Checked = false;

                dtpFDate.Text = DateTime.Now.ToShortDateString();
                dtpTDate.Text = DateTime.Now.ToShortDateString();
                Screen_Display(SSList);
            }
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                cSpd.setSpdSort(SSList, e.Column, true);
                return;
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            foreach (Form frm2 in Application.OpenForms) //떠있는지 체크
            {
                if (frm2.Name == "frmHcJepMain")
                {
                    HIC_JEPSU item = SSList.GetCurrentRowData() as HIC_JEPSU;

                    if (rSetGstrValue.IsNullOrEmpty())
                    {
                        this.Hide();
                        return;
                    }
                    else
                    {
                        rSetGstrValue(item);
                        this.Hide();
                        return;
                    }
                }
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
            else if (sender == txtSName && e.KeyChar == (char)13)
            {
                btnSearch.Focus();
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            #endregion
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

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Screen_Display(FpSpread Spd)
        {
            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            spd = Spd;

            FstrFDate = dtpFDate.Value.ToShortDateString();
            FstrTDate = dtpTDate.Value.ToShortDateString();
            FstrJong = VB.Left(cboJong.Text, 2);
            FstrSName = txtSName.Text.Trim();
            nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            FbChul = chkChul.Checked;
            FbJongGum = chkJongGum.Checked;
            FbKaTalk = chkKaTalk.Checked;
            FbDel = chkDel.Checked;
            FnEndo = chkEndo.Checked;
            FnHabit = chkHabit.Checked;

            try
            {
                //스레드 시작
                thread = new Thread(tProcess);
                thread.Start();

                Cursor.Current = Cursors.Default;

                this.btnExit.Enabled = true;
                this.SSList.Enabled = true;

                if(chkJumin.Checked= true)
                {
                    //SSList.ActiveSheet[]
                }

                //List<HIC_JEPSU> models = SSList.DataSource as List<HIC_JEPSU>;

                //for (int i = 0; i < models.Count; i++)
                //{
                //    if (!models[i].SEXAMS.IsNullOrEmpty())
                //    {
                //        models[i].SEXAMS = cHM.SExam_Names_Display(models[i].SEXAMS);
                //    }

                //    if (!models[i].UCODES.IsNullOrEmpty())
                //    {
                //        models[i].UCODES = cHM.SExam_Names_Display(models[i].UCODES);
                //    }
                //}

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            bShow = true;
            cboJong.SelectedIndex = 0;


            //주민번호표시 권한
            if (clsType.User.IdNumber == "18551" || clsType.User.IdNumber =="39480" || clsType.User.IdNumber == "36540")
            {
                chkJumin.Visible = true;
            }

        }

        delegate void threadSpdTypeDelegate(FpSpread spd, List<HIC_JEPSU> lists);
        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        void tProcess()
        {
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            List<HIC_JEPSU> list = hicJepsuService.GetListByItems(FstrFDate, FstrTDate, FstrJong, FstrSName, nLtdCode, FbChul, FbJongGum, FbKaTalk, FbDel, FnEndo, FnHabit);


            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].SEXAMS.IsNullOrEmpty())
                {
                    list[i].SEXAMS = cHM.SExam_Names_Display(list[i].SEXAMS);
                }

                if (!list[i].UCODES.IsNullOrEmpty())
                {
                    list[i].UCODES = cHM.UCode_Names_Display(list[i].UCODES);
                }
            }

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, list);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));
        }

        private void tShowSpread(FpSpread spd, List<HIC_JEPSU> lists)
        {
            spd.DataSource = lists;
        }
    }
}
