using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Main
{
    public partial class frmHcLiverCMunjin : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();

        long FnWRTNO = 0;
        long FnAge = 0;
        long FnPano = 0;
        string FstrJepDate = "";
        string fstrOK = "";

        public frmHcLiverCMunjin()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.chkResult52.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYEAR = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);
            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            //검진년도 Combo Set
            cboYear.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYEAR));
                nYEAR -= 1;
            }
            cboYear.SelectedIndex = 0;

            //결과Setting
            cboResult1.Items.Clear();
            cboResult1.Items.Add("1.예");
            cboResult1.Items.Add("2.아니오");
            cboResult1.SelectedIndex = -1;

            cboResult2.Items.Clear();
            cboResult2.Items.Add("1.예");
            cboResult2.Items.Add("2.아니오");
            cboResult2.Items.Add("3.모름");
            cboResult2.SelectedIndex = -1;

            cboResult3.Items.Clear();
            cboResult3.Items.Add("1.예");
            cboResult3.Items.Add("2.아니오");
            cboResult3.SelectedIndex = -1;

            cboResult4.Items.Clear();
            cboResult4.Items.Add("1.예");
            cboResult4.Items.Add("2.아니오");
            cboResult4.SelectedIndex = -1;

            cboResult6.Items.Clear();
            cboResult6.Items.Add("1.99만원 이하");
            cboResult6.Items.Add("2.100-199만원");
            cboResult6.Items.Add("3.200-299만원");
            cboResult6.Items.Add("4.300-399만원");
            cboResult6.Items.Add("5.400-499만원");
            cboResult6.Items.Add("6.500-599만원");
            cboResult6.Items.Add("7.600-699만원");
            cboResult6.Items.Add("8.700-799만원");
            cboResult6.Items.Add("9.800-899만원");
            cboResult6.Items.Add("10.900-999만원");
            cboResult6.Items.Add("11.1000만원 이상");
            cboResult6.SelectedIndex = -1;

            cboResult7.Items.Clear();
            cboResult7.Items.Add("1.전혀 필요하지 않다");
            cboResult7.Items.Add("2.필요하지 않다");
            cboResult7.Items.Add("3.필요하다");
            cboResult7.Items.Add("4.매우필요하다");
            cboResult7.SelectedIndex = -1;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }

            else if (sender == btnSave)
            {
                fstrOK = "OK";
                Data_Error_Check();
                if (fstrOK == "OK")
                {
                    Data_Save_Process();    //저장루틴
                }
               
            }

            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display()
        {
            int nRow = 0;
            int nREAD = 0;

            long nWrtno = 0;
            string strOK = "";
            string strJong = "";
            string strROWID = "";
            string strFrDate = "";
            string strToDate = "";
            string strSName = "";

            sp.Spread_All_Clear(ssList);

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;
            if (txtWrtNo.Text != "")
            {
                nWrtno = Convert.ToInt32(txtWrtNo.Text);
            }
            if (txtSName.Text.Trim() != "")
            {
                strSName = txtSName.Text.Trim();
            }

            List<HIC_JEPSU> list = hicJepsuService.GetItemByJepdateLiverC(strFrDate, strToDate, strSName, nWrtno);

            nREAD = list.Count;
            progressBar1.Maximum = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                nWrtno = list[i].WRTNO;
                strOK = "OK";

                strROWID = hicResBohum1Service.GetItemBywrtno(nWrtno);

                if (rdoJob1.Checked == true)
                {
                    if (!strROWID.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                }
                else
                {
                    if (strROWID.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                }
                strJong = list[i].GJJONG;

                if (strOK == "OK")
                {
                    nRow = nRow + 1;
                    ssList.ActiveSheet.RowCount = nRow;
                    ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJJONG;
                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;
                    ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].WRTNO.ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE;
                    ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].SEX;
                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list[i].JUMIN2.Trim())).ToString();
                    ssList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].PANO.ToString();

                }

                progressBar1.Value = i + 1;

            }
        }
        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {

                string strMsg = "";

                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 2].Text;
                FnWRTNO = long.Parse(ssList.ActiveSheet.Cells[e.Row, 2].Text);
                strMsg = "[성명:" + ssList.ActiveSheet.Cells[e.Row, 1].Text + "]";
                strMsg += " 접수번호:" + ssList.ActiveSheet.Cells[e.Row, 2].Text;
                strMsg += " 접수일자:" + ssList.ActiveSheet.Cells[e.Row, 3].Text;
                FstrJepDate = ssList.ActiveSheet.Cells[e.Row, 3].Text;
                strMsg += " 성별:" + ssList.ActiveSheet.Cells[e.Row, 4].Text;
                FnAge = long.Parse(ssList.ActiveSheet.Cells[e.Row, 5].Text);
                strMsg = strMsg + " 나이:" + FnAge + "세";
                if (!ssList.ActiveSheet.Cells[e.Row, 6].Text.IsNullOrEmpty())
                {
                    FnPano = long.Parse(ssList.ActiveSheet.Cells[e.Row, 6].Text);
                }
                else
                {
                    FnPano = 0;
                }
                lblMsg.Text = strMsg;

                fn_Display_Munjin();
            }
        }

        private void Data_Error_Check()
        {
            if (VB.Pstr(cboResult1.Text, ".", 1) == "") { MessageBox.Show("1번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }
            if (VB.Pstr(cboResult2.Text, ".", 1) == "") { MessageBox.Show("2번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }
            if (VB.Pstr(cboResult3.Text, ".", 1) == "") { MessageBox.Show("3번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }
            if (VB.Pstr(cboResult4.Text, ".", 1) == "") { MessageBox.Show("4번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }
            if (VB.Pstr(cboResult6.Text, ".", 1) == "") { MessageBox.Show("6번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }
            if (VB.Pstr(cboResult7.Text, ".", 1) == "") { MessageBox.Show("7번 선택값을 확인해주세요.", "저장불가"); fstrOK = ""; return; }

        }
        private void Data_Save_Process()
        {
            string strResult1 = ""; string strResult2 = ""; string strResult3 = ""; string strResult4 = ""; string strResult5 = "";
            string strResult6 = ""; string strResult7 = ""; string strResult8 = ""; string strResult9 = ""; string strResult10 = "";
            string strResult11 = ""; string strResult12 = ""; string strResult13 = ""; string strResult14 = ""; string strResult15 = "";
            string strResult16 = ""; string strResult17 = ""; string strResult18 = ""; string strResult19 = ""; string strResult20 = "";

            strResult1 = VB.Pstr(cboResult1.Text, ".", 1).Trim();
            strResult2 = VB.Pstr(cboResult2.Text, ".", 1).Trim();
            strResult3 = VB.Pstr(cboResult3.Text, ".", 1).Trim();
            strResult4 = txtResult3.Text;
            strResult5 = VB.Pstr(cboResult4.Text, ".", 1).Trim();
            if (chkResult41.Checked)
            {
                strResult6 = "1";
            }
            else
            {
                strResult6 = "2";
            }

            if (chkResult42.Checked)
            {
                strResult7 = "1";
            }
            else
            {
                strResult7 = "2";
            }

            if (chkResult43.Checked)
            {
                strResult8 = "1";
            }
            else
            {
                strResult8 = "2";
            }

            if (chkResult44.Checked)
            {
                strResult9 = "1";
            }
            else
            {
                strResult9 = "2";
            }

            if (chkResult51.Checked)
            {
                strResult10 = "1";
            }
            else
            {
                strResult10 = "2";
            }

            if (chkResult52.Checked)
            {
                strResult11 = "1";
            }
            else
            {
                strResult11 = "2";
            }

            if (chkResult53.Checked)
            {
                strResult12 = "1";
            }
            else
            {
                strResult12 = "2";
            }

            if (chkResult54.Checked)
            {
                strResult13 = "1";
            }
            else
            {
                strResult13 = "2";
            }

            if (chkResult55.Checked)
            {
                strResult14 = "1";
            }
            else
            {
                strResult14 = "2";
            }

            strResult15 = txtResult5.Text;

            if (chkResult56.Checked)
            {
                strResult16 = "1";
            }
            else
            {
                strResult16 = "2";
            }

            if (chkResult57.Checked)
            {
                strResult17 = "1";
            }
            else
            {
                strResult17 = "2";
            }

            if (chkResult58.Checked)
            {
                strResult18 = "1";
            }
            else
            {
                strResult18 = "2";
            }

            strResult19 = VB.Pstr(cboResult6.Text, ".", 1).Trim();
            strResult20 = VB.Pstr(cboResult7.Text, ".", 1).Trim();

            HIC_RES_BOHUM1 HLC = new HIC_RES_BOHUM1
            {
                WRTNO = FnWRTNO,
                TMUN0105 = strResult1,
                TMUN0106 = strResult2,
                TMUN0107 = strResult3,
                TMUN0108 = strResult4,
                TMUN0109 = strResult5,
                TMUN0110 = strResult6,
                TMUN0111 = strResult7,
                TMUN0112 = strResult8,
                TMUN0113 = strResult9,
                TMUN0114 = strResult10,
                TMUN0115 = strResult11,
                TMUN0116 = strResult12,
                TMUN0117 = strResult13,
                TMUN0118 = strResult14,
                TMUN0119 = strResult15,
                TMUN0120 = strResult16,
                TMUN0121 = strResult17,
                TMUN0122 = strResult18,
                TMUN0123 = strResult19,
                TMUN0124 = strResult20
            };

            hicResBohum1Service.UpdateLiverCbyWrtNo(HLC);

            MessageBox.Show("저장완료.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            fn_Screen_Clear();

        }


        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == chkResult52)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void fn_Screen_Clear()
        {
            cboResult1.SelectedIndex = -1;
            cboResult2.SelectedIndex = -1;
            cboResult3.SelectedIndex = -1;
            cboResult4.SelectedIndex = -1;
            cboResult6.SelectedIndex = -1;
            cboResult7.SelectedIndex = -1;

            txtResult3.Text = "";
            txtResult5.Text = "";

            chkResult41.Checked = false;
            chkResult42.Checked = false;
            chkResult43.Checked = false;
            chkResult44.Checked = false;

            chkResult51.Checked = false;
            chkResult52.Checked = false;
            chkResult53.Checked = false;
            chkResult54.Checked = false;
            chkResult55.Checked = false;
            chkResult56.Checked = false;
            chkResult57.Checked = false;
            chkResult58.Checked = false;
        }

        void fn_Display_Munjin()
        {

            fn_Screen_Clear();

            HIC_RES_BOHUM1 item = hicResBohum1Service.GetItemByWrtno(FnWRTNO);

            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult1.SelectedIndex = Convert.ToInt32(item.TMUN0105)-1; }
            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult2.SelectedIndex = Convert.ToInt32(item.TMUN0106)-1; }
            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult3.SelectedIndex = Convert.ToInt32(item.TMUN0107)-1; }
            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult4.SelectedIndex = Convert.ToInt32(item.TMUN0109)-1; }
            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult6.SelectedIndex = Convert.ToInt32(item.TMUN0123)-1; }
            if (!item.TMUN0105.IsNullOrEmpty()) { cboResult7.SelectedIndex = Convert.ToInt32(item.TMUN0124)-1; }

            txtResult3.Text = item.TMUN0108;
            txtResult5.Text = item.TMUN0119;

            if (item.TMUN0110 == "1") { chkResult41.Checked = true; }
            if (item.TMUN0111 == "1") { chkResult42.Checked = true; }
            if (item.TMUN0112 == "1") { chkResult43.Checked = true; }
            if (item.TMUN0113 == "1") { chkResult44.Checked = true; }

            if (item.TMUN0114 == "1") { chkResult51.Checked = true; }
            if (item.TMUN0115 == "1") { chkResult52.Checked = true; }
            if (item.TMUN0116 == "1") { chkResult53.Checked = true; }
            if (item.TMUN0117 == "1") { chkResult54.Checked = true; }
            if (item.TMUN0118 == "1") { chkResult55.Checked = true; }
            if (item.TMUN0120 == "1") { chkResult56.Checked = true; }
            if (item.TMUN0121 == "1") { chkResult57.Checked = true; }
            if (item.TMUN0122 == "1") { chkResult58.Checked = true; }


        }
    }
}
