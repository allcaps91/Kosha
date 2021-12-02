using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.Com
{

    /// <summary>
    /// Class Name : ComSupLibB
    /// File Name : frmComSupPtInfo.cs
    /// Title or Description : 환자세부조회
    /// Author : 김홍록
    /// Create Date : 2017-06-13
    /// Update History : 
    /// </summary>
    /// <history>신규</history>
    public partial class frmComSupPtInfo : Form
    {

        Point p;
        DataTable dt;
        Control ctl;

        clsSpread methodSpd = new clsSpread();
        clsMethod method = new clsMethod();

        /// <summary>델리게이트</summary>
        /// <param name="sender"></param>
        /// <param name="ptInfo"></param>
        public delegate void ePSMH_PtInfo(object sender, string pano, string sname);  
                                            
        /// <summary>버튼을 클릭할경우 조회된 내용이 반영</summary>
        public event ePSMH_PtInfo ePSMH_PTInfo;

        public frmComSupPtInfo(DataTable dt, Point p, string[] colName, int[] size, Control ctl)
        {
            InitializeComponent();

            this.p = p;
            this.dt = dt;
            this.ctl = ctl;

            //setListView_Header(this.lv, dc);
            setEvent();
            DataSet ds = new DataSet();

            DataTable dt2 = dt.Clone();

            dt2 = dt.Copy();
            

            ds.Tables.Add(dt2);

            setSpdStyle(this.ss_Main, ds, colName, size);            

        }

        void setEvent()
        {
            this.Load                       += new EventHandler(eFormLoad);
            this.FormClosed                 += new FormClosedEventHandler(eFormClosed);

            this.ss_Main.CellDoubleClick    += new CellClickEventHandler(eSpreadDClick);
            this.ss_Main.KeyPress += new KeyPressEventHandler(eSpreadKeyPress);
        }

        void eSpreadKeyPress(object sender, KeyPressEventArgs e)
        {
            ePSMH_PTInfo += FrmSupComPtInfo_PSMH_PTInfoEvent;
            ePSMH_PTInfo(this.ctl, this.ss_Main.ActiveSheet.Cells[this.ss_Main.ActiveSheet.ActiveRow.Index, 0].Text, this.ss_Main.ActiveSheet.Cells[this.ss_Main.ActiveSheet.ActiveRow.Index, 1].Text);

            this.Close();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            this.Location = this.p;

            if (this.p.X == 0 && this.p.Y == 0)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            string s = this.ss_Main.ActiveSheet.Cells[this.ss_Main.ActiveSheet.ActiveRow.Index, 0].Text;

            if (string.IsNullOrEmpty(s))
            {
                ePSMH_PTInfo(this.ctl, null, null);
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ss_Main, e.Column);
                return;
            }
            else { 
                ePSMH_PTInfo += FrmSupComPtInfo_PSMH_PTInfoEvent;
                ePSMH_PTInfo(this.ctl, this.ss_Main.ActiveSheet.Cells[e.Row,0].Text, this.ss_Main.ActiveSheet.Cells[e.Row, 1].Text);

                this.Close();
            }
        }

        void FrmSupComPtInfo_PSMH_PTInfoEvent(object sender, string pano, string sname)
        {

        }

        string setListView_SelectIndex(ListView lvTagget, int item)
        {
            ListView.SelectedListViewItemCollection select = lvTagget.SelectedItems;
            string s = null;

            foreach (ListViewItem var in select)
            {
                if (var.SubItems.Count > 0)
                {
                    if (var.SubItems[item].Text != null && var.SubItems[item].Text.Trim().Length > 0)
                    {
                        
                        if (method.getGubunText(var.SubItems[item].Text, ".") != "")
                        {
                            s = var.SubItems[item].Text;
                        }
                    }
                }
            }

            return s;
        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            // 헤더 위치
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 35;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            for (int i = 0; i < spd.ActiveSheet.Columns.Count; i++)
            {
                spd.ActiveSheet.Columns.Get(i).Visible = true;
            }

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            methodSpd.setSpdFilter(spd, -1, AutoFilterMode.EnhancedContextMenu, true);

            //// 3. 컬럼 스타일 설정.            
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHK, clsSpread.enmSpdType.CheckBox);

            //string[] arrS = { "", "1", "2", "3", "4", "5", "6" };
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHASU, clsSpread.enmSpdType.ComboBox, arrS);

            //// 정렬
            //methodSpd.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            //// 4. hide정보.
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTTIME, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.BDATE, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.JEPCODE, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.MSG, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ROW_ID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.UNITNEW1, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.UNITNEW2, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.BI, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ORDERNO, clsSpread.enmSpdType.Hide);
            ////methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKIV, clsSpread.enmSpdType.Hide);

            //if (this.tabMain.SelectedTab == this.tabMain_Wait)
            //{
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHASU, clsSpread.enmSpdType.View);
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTDATE, clsSpread.enmSpdType.Hide);
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTNAL, clsSpread.enmSpdType.Hide);
            //}
            //else if (this.tabMain.SelectedTab == this.tabMain_Multi || this.tabMain.SelectedTab == this.tabMain_Done)
            //{
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHASU, clsSpread.enmSpdType.Hide);
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTDATE, clsSpread.enmSpdType.View);
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTTIME, clsSpread.enmSpdType.View);
            //    methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTNAL, clsSpread.enmSpdType.View);

            //    if (this.tabMain.SelectedTab == this.tabMain_Multi)
            //    {
            //        methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.BDATE, clsSpread.enmSpdType.View);
            //        methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTDATE, clsSpread.enmSpdType.Hide);
            //        methodSpd.setColStyle(spd, -1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ACTNAL, clsSpread.enmSpdType.View);
            //    }
            //}

            //// 5. 조건부 서식
            //UnaryComparisonConditionalFormattingRule unary;

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "★", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.KK059, unary);

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "불", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.SUNAP, unary);

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "후", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.SUNAP, unary);

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "필", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKVACC, unary);

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "독", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKCZ394, unary);

            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;



            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKIV, unary);
            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "수혈", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKIV, unary);
            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "수액", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;

            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.CHKIV, unary);
            //unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "수액", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;


            //UnaryComparisonConditionalFormattingRule unary2;
            //unary2 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "알", false);
            //unary2.BackColor = method.cSpdCellImpact_Back;
            //unary2.ForeColor = method.cSpdCellImpact_Fore;
            //this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ALLERGY, unary2);



            ////TODO : 김홍록 : 2017.04.07 : 머지부분은 현업화 협의 후 추가 진행 예정

            //if (this.tabMain.SelectedTab != this.tabMain_Ward)
            //{
            //    methodSpd.setColMerge(spd, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.PTNO);
            //    methodSpd.setColMerge(spd, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.SNAME);
            //}

            //methodSpd.setSpdFilter(spd, (int)clsComSupIjrmSQL.enmSel_OCS_OORDER_InjecMain.ORDERNM, AutoFilterMode.EnhancedContextMenu, true);

        }
    }
}
