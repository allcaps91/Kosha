using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcSAmtSelect.cs
/// Description     : 선택검사 선택
/// Author          : 이상훈
/// Create Date     : 2020-06-26
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmSAmtSelect(HaMain85.frm)" />

namespace ComHpcLibB
{
    public partial class frmHcSAmtSelect : Form
    {
        HicGroupcodeService hicGroupcodeService = null;

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        ComFunc cf = new ComFunc();

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        frmAcheDetail FrmAcheDetail = null;

        List<string> FstrJong;
        string FstrFlag;
        Form FstrCallId;
        string FstrCallName;

        IntPtr menuHandle;
        const int MF_BYCOMMAND = 0x0;
        /// <summary>
        /// Represents the Move menu item.
        /// </summary>
        const int SC_MOVE = 0xF010;
        /// <summary>
        /// Represents the Close menu item.
        /// </summary>
        const int SC_CLOSE = 0xF060;

        public frmHcSAmtSelect()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcSAmtSelect(List<string> strJong, Form strCallId, string strCallName)
        {
            InitializeComponent();
            FstrJong = strJong;
            FstrCallId = strCallId;
            FstrCallName = strCallName;
            this.menuHandle = GetSystemMenu(strCallId.Handle, false);
            SetEvent();
        }

        void SetEvent()
        {
            hicGroupcodeService = new HicGroupcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS3.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS2.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS3.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.chkAllSel1.Click += new EventHandler(eChkBoxClick);
            this.chkAllSel2.Click += new EventHandler(eChkBoxClick);
            this.chkAllSel3.Click += new EventHandler(eChkBoxClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            IntPtr hSysMenu;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);

            lblGCode.Text = "";
            fn_Select_Clear();
            fn_Select_Sheet_SET();
            
            FstrFlag = "Y";

            //닫기 비활성화
            hSysMenu = GetSystemMenu(this.menuHandle, false);
            DeleteMenu(hSysMenu, SC_CLOSE, MF_BYCOMMAND);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                fn_Select_Clear();
                rSetGstrValue(null);
                this.Close();
                return;
            }
            else if (sender == btnMenuSelect)
            {
                string strCode = "";

                for (int i = 1; i <= 3; i++)
                {
                    FpSpread SS = (Controls.Find("SS" + (i).ToString(), true)[0] as FpSpread);
                    for (int j = 0; j < SS.ActiveSheet.RowCount; j++)
                    {
                        if (SS.ActiveSheet.Cells[j, 0].Text == "True")
                        {
                            strCode += SS.ActiveSheet.Cells[j, 1].Text + ",";
                            //선택한것 글자색 원래복귀
                            SS.ActiveSheet.Cells[j, 0, j, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SS.ActiveSheet.Cells[j, 0, j, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
                
                if (strCode.IsNullOrEmpty())
                {
                    MessageBox.Show("선택검사를 1건도 선택하지 안았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    strCode = VB.Left(strCode, strCode.Length - 1);
                }

                rSetGstrValue(strCode.Trim());
                this.Close();
            }
            else if (sender == btnMenuCancel)
            {
                fn_Select_Clear();
            }
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            int k = 0;

            if (sender == chkAllSel1 || sender == chkAllSel2 || sender == chkAllSel3)
            {
                if (sender == chkAllSel1)
                {
                    k = 1;
                }
                else if (sender == chkAllSel2)
                {
                    k = 2;
                }
                else if (sender == chkAllSel3)
                {
                    k = 3;
                }
                
                CheckBox chkAllSel = (Controls.Find("chkAllSel" + (k).ToString(), true)[0] as CheckBox);

                FpSpread SS = (Controls.Find("SS" + (k).ToString(), true)[0] as FpSpread);

                if (chkAllSel.Checked == true)
                {   
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, 0].Text = "True";
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(255, 0, 0), 1, true, true, true, true);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    fn_GroupCode_Select();
                }
                else
                {
                    //lblGCode.Text = "";
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, 0].Text = "";
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(0, 0, 0), 1, true, true, true, true);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    Application.DoEvents();
                    lblGCode.Text = "";
                    fn_GroupCode_Select();
                    
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            int k = 0;

            if (sender == SS1 || sender == SS2 || sender == SS3)
            {
                if (e.RowHeader == false)
                {
                    if (sender == SS1)
                    {
                        k = 1;
                    }
                    else if (sender == SS2)
                    {
                        k = 2;
                    }
                    else if (sender == SS3)
                    {
                        k = 3;
                    }
                    
                    FpSpread SS = (Controls.Find("SS" + (k).ToString(), true)[0] as FpSpread);

                    if (SS.ActiveSheet.Cells[e.Row, 0].Text == "True")
                    {
                        SS.ActiveSheet.Cells[e.Row, 0].Text = "";
                    }
                    else
                    {
                        SS.ActiveSheet.Cells[e.Row, 0].Text = "True";
                    }

                    if (SS.ActiveSheet.Cells[e.Row, 0].Text == "True")
                    {
                        fn_GroupCode_Select();
                        //선택한것 글자색 변경
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(255, 0, 0), 1, true, true, true, true);
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    }
                    else
                    {
                        fn_GroupCode_Select();
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(0, 0, 0), 1, true, true, true, true);
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    if (e.Row == -1 && e.Column == -1)
                    {
                        if (FstrFlag == "Y")
                        {
                            for (int j = 0; j < SS.ActiveSheet.RowCount; j++)
                            {
                                SS.ActiveSheet.Cells[j, 0].Text = "True";
                            }
                            FstrFlag = "";
                        }
                        else
                        {
                            for (int j = 0; j < SS.ActiveSheet.RowCount; j++)
                            {
                                SS.ActiveSheet.Cells[j, 0].Text = "";
                            }
                            FstrFlag = "Y";
                        }
                    }
                }
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            int k = 0;

            if (sender == SS1 || sender == SS2 || sender == SS3)
            {
                if (sender == SS1)
                {
                    k = 1;
                }
                else if (sender == SS2)
                {
                    k = 2;
                }
                else if (sender == SS3)
                {
                    k = 3;
                }

                FpSpread SS = (Controls.Find("SS" + (k).ToString(), true)[0] as FpSpread);

                lblGCode.Text = "";
                if (SS.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    fn_GroupCode_Select();
                    //선택한것 글자색 변경
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(255, 0, 0), 1, true, true, true, true);
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    if (SS.ActiveSheet.Cells[e.Row, 1].Text == "3111" || SS.ActiveSheet.Cells[e.Row, 1].Text == "9141")
                    {
                        MessageBox.Show("수면내시경을 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    fn_GroupCode_Select();
                    //선택한것 글자색 변경
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(0, 0, 0), 1, true, true, true, true);
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                    SS.ActiveSheet.Cells[e.Row, 0, e.Row, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    if (SS.ActiveSheet.Cells[e.Row, 1].Text == "3111" || SS.ActiveSheet.Cells[e.Row, 1].Text == "9141")
                    {
                        MessageBox.Show("수면내시경을 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        void fn_GroupCode_Select()
        {
            lblGCode.Text = "";

            for (int i = 1; i <= 3; i++)
            {
                FpSpread SS = (Controls.Find("SS" + (i).ToString(), true)[0] as FpSpread);
                for (int j = 0; j < SS.ActiveSheet.RowCount; j++)
                {
                    if (SS.ActiveSheet.Cells[j, 0].Text == "True")
                    {
                        lblGCode.Text += SS.ActiveSheet.Cells[j, 1].Text;
                    }
                }
            }
        }

        void fn_Select_Sheet_SET()
        {
            int inx = 0;
            int[] nRow = new int[3];
            int nRead = 0;
            string strHang = "";
            string strCode = "";
            string strName = "";
            string strGbSelectYN = "";
            int nSpdRow = 0;

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);

            if (FstrJong.IsNullOrEmpty())
            {
                return;
            }

            for (int i = 0; i <= 2; i++)
            {
                nRow[i] = 0;
            }

            if (FstrCallName == "종합검진")
            {
                strGbSelectYN = "";
            }
            else
            {
                strGbSelectYN = "Y";
            }

            List<HIC_GROUPCODE> list = hicGroupcodeService.GetHangCode(FstrJong, strGbSelectYN);

            nRead = list.Count;

            if (nRead > 0)
            {
                for (int i = 0; i < nRead; i++)
                {
                    strHang = list[i].HANG;
                    strCode = list[i].CODE;
                    strName = list[i].NAME;

                    switch (strHang)
                    {
                        case "I":
                            inx = 1; //특수선택
                            break;
                        case "H":
                            inx = 2; //공통선택
                            break;
                        default:
                            inx = 0;
                            break;
                    }

                    FpSpread SS = (Controls.Find("SS" + (inx + 1).ToString(), true)[0] as FpSpread);
                    //SS.ActiveSheet.RowCount = nRead;
                    nRow[inx] += 1;                    
                    nSpdRow = nRow[inx];
                    SS.ActiveSheet.RowCount += 1;
                    SS.ActiveSheet.Cells[nSpdRow - 1, 0].Text = "";
                    SS.ActiveSheet.Cells[nSpdRow - 1, 1].Text = strCode;
                    SS.ActiveSheet.Cells[nSpdRow - 1, 2].Text = strName;

                    //기존항목 체크
                    if (VB.L(clsHcVariable.GstrSExams, strCode) > 1)
                    {
                        SS.ActiveSheet.Cells[nSpdRow - 1, 0].Text = "True";
                    }
                }

                for (int i = 0; i <= 1; i++)
                {
                    FpSpread SS = (Controls.Find("SS" + (i + 1).ToString(), true)[0] as FpSpread);
                    SS.ActiveSheet.RowCount = nRow[i];
                }
            }
        }

        void fn_Select_Clear()
        {
            for (int i = 1; i <= 3; i++)
            {
                FpSpread SS = (Controls.Find("SS" + (i).ToString(), true)[0] as FpSpread);

                if (SS.ActiveSheet.RowCount > 0)
                {
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, 0].Text = "";
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.FromArgb(0, 0, 0), 1, true, true, true, true);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                    SS.ActiveSheet.Cells[0, 0, SS.ActiveSheet.RowCount - 1, SS.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);

                    CheckBox chkAllSel = (Controls.Find("chkAllSel" + (i).ToString(), true)[0] as CheckBox);

                    chkAllSel.Checked = false;
                }
            }
            lblGCode.Text = "";
        }
    }
}
