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
/// File Name       : frmHcLtdUcodeHelp.cs
/// Description     : 취급물질(유해인자) 선택창
/// Author          : 김경동
/// Create Date     : 2020-07-14
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm회사별취급물질선택.frm(Frm회사별취급물질선택)" />


namespace ComHpcLibB
{
    public partial class frmHcLtdUcodeHelp : Form
    {
        string FstrJong = string.Empty;
        string FstrLtdCode = string.Empty;

        HicLtdService hicLtdService = null;
        HicLtdUcodeService hicLtdUcodeService = null;
        HicMcodeService hicMcodeService = null;
        HicGroupcodeService hicGroupcodeService = null;

        public delegate void SetGstrValue(List<string> lstUCodes, List<string> lstSExams);
        public static event SetGstrValue rSetGstrValue;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();

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

        public frmHcLtdUcodeHelp()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcLtdUcodeHelp(string argLtdCode, string argJong, string argUCodes)
        {
            InitializeComponent();
            SetEvent();
            FstrJong = argJong;
            FstrLtdCode = argLtdCode;
            FstrUCodes = argUCodes;
        }

        void SetEvent()
        {
            hicLtdService = new HicLtdService();
            hicLtdUcodeService = new HicLtdUcodeService();
            hicMcodeService = new HicMcodeService();
            hicGroupcodeService = new HicGroupcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.btnMenuCancel.Click += new EventHandler(eBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SSList.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            
        }

        void eFormLoad(object sender, EventArgs e)
        {

            int nRow = 0;
            string strLtdCode = "";
            string strJong = "";
            string strList = "";
            string strSCode = "";
            string strTest = "";

            List<string> strCode = new List<string>();
            IntPtr hSysMenu;

            this.Location = new Point(10, 10);

            sp.Spread_All_Clear(SS1);
            FnSeq = 0;

            //Test를 위한변수
            //strTest = "483{}24";

            strLtdCode = FstrLtdCode;

            switch (FstrJong)
            {
                case "21":
                case "22":
                case "24": strJong = "1"; break;
                case "23": strJong = "2"; break;
                case "69": strJong = "9"; break;
                default: strJong = "3"; break;
            }

            txtRemark.Text = hicLtdService.GetSpcRemarkByCode(long.Parse(strLtdCode));

            List<HIC_LTD_UCODE> list = hicLtdUcodeService.GetListByCodeJong1(long.Parse(strLtdCode), strJong);

            nRow = list.Count;
            SSList.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                nRow = nRow + 1;
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].JOBNAME;
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].UCODES + "{}" + list[i].SCODES;
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].RID;
            }

            //유해인자 코드를 SELECT
            List<HIC_MCODE> list2 = hicMcodeService.FindAll();
            nRow = 0;
            FnSeq = 0;
            for (int i = 0; i < list2.Count; i++)
            {
                if (VB.InStr(FstrUCodes, list2[i].CODE) > 0)
                {
                    nRow = nRow + 1;
                    if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }
                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "True";
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].CODE;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list2[i].NAME;
                    FnSeq = FnSeq + 1;
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = FnSeq.ToString();
                }
            }
            SS1.ActiveSheet.RowCount = nRow;

            //닫기 비활성화
            hSysMenu = GetSystemMenu(this.menuHandle, false);
            DeleteMenu(hSysMenu, SC_CLOSE, MF_BYCOMMAND);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                //rSetGstrValue(null);
                this.Close();
                return;
            }
            else if (sender == btnMenuSelect)
            {
                string strSeq = "";
                int[] nCnt = new int[100];
                string[] strData = new string[100];
                int nChkCnt = 0;
                string strLtdUcodes = "";

                List<string> lstUCodes = new List<string>();
                List<string> lstSExams = new List<string>();

                #region 이전 루틴
                //for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                //{
                //    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                //    {
                //        strSeq = SS1.ActiveSheet.Cells[i, 3].Text;
                //        nCnt[int.Parse(strSeq) - 1] = int.Parse(strSeq);
                //        strData[int.Parse(strSeq) - 1] = SS1.ActiveSheet.Cells[i, 1].Text;
                //        nChkCnt++;
                //    }
                //}

                //for (int i = 0; i < 100; i++)
                //{
                //    if (nCnt[i] > 0)
                //    {
                //        strLtdUcodes += strData[i] + ",";
                //    }
                //}

                //strLtdUcodes = strLtdUcodes + "{}";
                //for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                //{
                //    if (SS2.ActiveSheet.Cells[i, 0].Text == "True")
                //    {
                //        strLtdUcodes = strLtdUcodes + SS2.ActiveSheet.Cells[i, 1].Text + ",";
                //    }
                //}

                //if (nChkCnt == 0)
                //{
                //    MessageBox.Show("유해인자, 선택검사를 1건도 선택하지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                #endregion

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        lstUCodes.Add(SS1.ActiveSheet.Cells[i, 1].Text.Trim());
                    }
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        lstSExams.Add(SS2.ActiveSheet.Cells[i, 1].Text.Trim());
                    }
                }

                if (lstUCodes.Count == 0)
                {
                    MessageBox.Show("유해인자, 선택검사를 1건도 선택하지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if(lstUCodes.Count > 0 )
                {
                    lstSExams.Add("J224");
                }

                //2021-01-25


                if (FstrJong == "23")
                {
                    lstSExams.Add("2111");
                }
                else if (FstrJong == "22")
                {
                    lstSExams.Add("2202");
                    lstSExams.Add("2203");
                }
                else if (FstrJong == "23")
                {
                    lstSExams.Add("2311");
                }
                else if (FstrJong == "24")
                {
                    lstSExams.Add("2402");
                }
                
                //rSetGstrValue(strLtdUcodes);
                rSetGstrValue(lstUCodes, lstSExams);

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

                SS2.ActiveSheet.Cells[0, 0, SS2.ActiveSheet.RowCount - 1, 0].Text = "";
                SS2.ActiveSheet.Cells[0, 3, SS2.ActiveSheet.RowCount - 1, 3].Text = "";
                FnSeq = 0;

                //선택한것 글자색 변경
                SS2.ActiveSheet.Cells[0, 0, SS2.ActiveSheet.RowCount - 1, 3].ForeColor = Color.FromArgb(0, 0, 0);
                SS2.ActiveSheet.Cells[0, 0, SS2.ActiveSheet.RowCount - 1, 3].BackColor = Color.FromArgb(255, 255, 255);
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
                    SS1.ActiveSheet.Cells[e.Row, 3].Text = "";
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;

                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                        {
                            if (j <= FnSeq)
                            {
                                j += 1;
                                SS1.ActiveSheet.Cells[e.Row, 3].Text = j.ToString();
                            }
                        }
                    }
                }
                else if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "")
                {
                    SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                    SS1.ActiveSheet.Cells[e.Row, 3].Text = FnSeq.ToString();
                    SS1.ActiveSheet.Cells[e.Row, 0, e.Row, SS1.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;

                    j = 0;
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                        {
                            if (j <= FnSeq)
                            {
                                j += 1;
                                SS1.ActiveSheet.Cells[e.Row, 3].Text = j.ToString();
                            }
                        }
                    }
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            int nRow = 0;
            long nCNT = 0;
            string strUCodes = "";
            string strSCodes = "";
            string strCode = "";

            List<string> strList = new List<string>();

            if (sender == SSList)
            {
                strUCodes = VB.Trim(VB.Pstr(SSList.ActiveSheet.Cells[e.Row, 1].Text, "{}", 1));
                strSCodes = VB.Trim(VB.Pstr(SSList.ActiveSheet.Cells[e.Row, 1].Text, "{}", 2));

                nCNT = VB.L(strUCodes, ",");
                strList.Clear();

                for (int i = 1; i <= nCNT; i++)
                {
                    strCode = VB.Trim(VB.Pstr(strUCodes, ",", i));
                    if (strCode != "")
                    {
                        strList.Add(strCode);
                    }
                }

                nRow = 0;
                FnSeq = 0;
                SS1.ActiveSheet.RowCount = 0;
                if (strList != null)
                {
                    //List<HIC_MCODE> list = hicMcodeService.GetCodeListByCodeIn(strList);
                    List<HIC_GROUPCODE> list = hicGroupcodeService.GetItemByUCodes(strList);

                    if (list.Count > 0)
                    {
                        SS1.ActiveSheet.RowCount = list.Count;
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            nRow = nRow + 1;
                            if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }
                            SS1.ActiveSheet.Cells[nRow-1, 0].Text = "1";
                            SS1.ActiveSheet.Cells[nRow-1, 1].Text = list[i].UCODE;
                            SS1.ActiveSheet.Cells[nRow-1, 2].Text = list[i].NAME;
                            FnSeq = FnSeq + 1;
                            SS1.ActiveSheet.Cells[nRow-1, 3].Text = FnSeq.ToString();
                        }
                    }
                }

                List<HIC_MCODE> list2 = hicMcodeService.GetCodeListByCodeNotIn(strList); 
                for (int i = 0; i < list2.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }
                    SS1.ActiveSheet.Cells[nRow-1, 0].Text = "";
                    SS1.ActiveSheet.Cells[nRow-1, 1].Text = list2[i].CODE;
                    SS1.ActiveSheet.Cells[nRow-1, 2].Text = list2[i].NAME;
                }
                SS1.ActiveSheet.RowCount = nRow;

                nCNT = VB.L(strSCodes, ",");
                nRow = 0;
                for (int i = 0; i < nCNT; i++)
                {
                    strCode = VB.Trim(VB.Pstr(strSCodes, ",", i));
                    if (strCode != "")
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2.ActiveSheet.RowCount) { SS2.ActiveSheet.RowCount = nRow;}
                        SS2.ActiveSheet.Cells[nRow-1, 0].Text = "1";
                        SS2.ActiveSheet.Cells[nRow-1, 1].Text = strCode;
                        SS2.ActiveSheet.Cells[nRow-1, 2].Text = hb.READ_Group_Name(strCode);
                    }
                }
                SS2.ActiveSheet.RowCount = nRow;

            }
        }
    }
}
