using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcHarmfulFactor.cs
/// Description     : 취급물질(유해인자) 선택창
/// Author          : 이상훈
/// Create Date     : 2020-07-08
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmYuheSelect.frm(HcMain82)" />

namespace HC_Main
{
    public partial class frmHcHarmfulFactor : Form
    {
        HicMcodeService hicMcodeService = null;

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        clsSpread sp = new clsSpread();

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

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

        int FnSeq;
        string FstrUCodes;


        public frmHcHarmfulFactor()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcHarmfulFactor(string strUCodes)
        {
            InitializeComponent();
            FstrUCodes = strUCodes;
            SetEvent();
        }

        void SetEvent()
        {
            hicMcodeService = new HicMcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            List<string> strCode = new List<string>();
            IntPtr hSysMenu;

            this.Location = new Point(10, 10);

            sp.Spread_All_Clear(SS1);
            FnSeq = 0;

            strCode.Clear();
            strCode.Add("V11");
            strCode.Add("V12");
            strCode.Add("V13");

            //유해인자 코드를 SELECT
            List<HIC_MCODE> list = hicMcodeService.GetCodeNamebyNotInNight(strCode);

            SS1.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME;

                if (VB.L(FstrUCodes, list[i].CODE) > 1)
                {
                    //SS1.ActiveSheet.Cells[i, 0].Text = "True";
                }
            }

            //닫기 비활성화
            hSysMenu = GetSystemMenu(this.menuHandle, false);
            DeleteMenu(hSysMenu, SC_CLOSE, MF_BYCOMMAND);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                rSetGstrValue(null);
                this.Close();
                return;
            }
            else if (sender == btnMenuSelect)
            {
                string strChk = "";
                string strSeq = "";
                int[] nCnt = new int[100];
                string[] strData = new string[100];
                int nChkCnt = 0;
                string strHarmfulFactor = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strSeq = SS1.ActiveSheet.Cells[i, 3].Text;
                        nCnt[int.Parse(strSeq) - 1] = int.Parse(strSeq);
                        strData[int.Parse(strSeq) - 1] = SS1.ActiveSheet.Cells[i, 1].Text;
                        nChkCnt++;
                    }
                }

                for (int i = 0; i < 100; i++)
                {
                    if (nCnt[i] > 0)
                    {
                        strHarmfulFactor += strData[i] + ",";
                    }
                }

                if (nChkCnt == 0)
                {
                    MessageBox.Show("유해인자를 1건도 선택하지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                rSetGstrValue(strHarmfulFactor);
                this.Close();
            }
            else if (sender == btnMenuCancel)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
                SS1.ActiveSheet.Cells[0, 3, SS1.ActiveSheet.RowCount - 1, 3].Text = "";
                FnSeq = 0;

                //선택한것 글자색 변경
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 3].ForeColor = Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 3].BackColor = Color.FromArgb(255, 255, 255);
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                FnSeq += 1;
                SS1.ActiveSheet.Cells[e.Row, 3].Text = FnSeq.ToString();
                //선택한것 글자색 변경
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            }
            else
            {
                FnSeq -= 1;
                if (FnSeq < 0)
                {
                    FnSeq = 0;
                }
                SS1.ActiveSheet.Cells[e.Row, 3].Text = FnSeq.ToString();
                //선택한것 글자색 변경
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                int j = 0;

                if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {   
                    SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                    FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(SS1);
                    eSpdBtnClick(SS1, new EditorNotifyEventArgs(view, SS1, e.Row, 0));
                    SS1.ActiveSheet.Cells[e.Row, 3].Text = "";
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;

                    j = 0;
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            if (j <= FnSeq)
                            {
                                j += 1;
                                SS1.ActiveSheet.Cells[i, 3].Text = j.ToString();
                            }
                        }
                    }
                }
                else if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "")
                {   
                    SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                    FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(SS1);
                    eSpdBtnClick(SS1, new EditorNotifyEventArgs(view, SS1, e.Row, 0));
                    SS1.ActiveSheet.Cells[e.Row, 3].Text = FnSeq.ToString();
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;

                    j = 0;
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            if (j <= FnSeq)
                            {
                                j += 1;
                                SS1.ActiveSheet.Cells[i, 3].Text = j.ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
