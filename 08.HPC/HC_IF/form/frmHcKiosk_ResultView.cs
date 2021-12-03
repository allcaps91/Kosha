using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHcKiosk_ResultView :Form
    {
        int FnTimerCnt = 0;

        clsSpread cSpd = null;
        HicJepsuService hicJepsuService = null;
        ActingCheckService actingCheckService = null;
        HicResultActiveService hicResultActiveService = null;
        HicResultService hicResultService = null;
        HicSunapService hicSunapService = null;

        public frmHcKiosk_ResultView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicJepsuService = new HicJepsuService();
            actingCheckService = new ActingCheckService();
            hicResultActiveService = new HicResultActiveService();
            hicResultService = new HicResultService();
            hicSunapService = new HicSunapService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimer_Timer);
            this.txtWRTNO.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWRTNO)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (txtWRTNO.Text.Trim() == "")
                    {
                        return;
                    }

                    Screen_Display(txtWRTNO.Text.Trim().To<long>(0));

                    txtWRTNO.SelectionStart = 0;
                    txtWRTNO.SelectionLength = txtWRTNO.Text.Length;
                    txtWRTNO.Focus();
                }
            }
        }

        private void Screen_Display(long nWRTNO)
        {
            long nPANO = 0;
            int nCNT1 = 0;
            int nCNT2 = 0;
            bool boolSort = false;

            try
            {
                if (nWRTNO == 0) { return; }

                string strDate = DateTime.Now.ToShortDateString();

                HIC_JEPSU item = hicJepsuService.GetItemByJepDateWrtno(nWRTNO, strDate);

                if (item.IsNullOrEmpty())
                {
                    cSpd.Spread_Clear_Simple(ssChk, 1);
                    lblSName.Text = "";
                    panRemark.Text = "";
                    panMsg.Text = "오늘 접수된 번호가 아닙니다.";
                    return;
                }

                lblSName.Text = item.SNAME + "(" + item.AGE + "/" + item.SEX + ")";
                nPANO = item.PANO;

                string strTemp = "";

                //검사 참고사항을 표시함
                List<HIC_JEPSU> list1 = hicJepsuService.GetExRemarkByPanoJepDate(nPANO, strDate);
                if (list1.Count > 0)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        strTemp += list1[i].EXAMREMARK + ",";
                    }
                }

                if (!strTemp.IsNullOrEmpty())
                {
                    panRemark.Text = strTemp;
                }

                //당일 암검진이 있으면 같이 표시
                nCNT1 = 0; nCNT2 = 0;
                List<ACTING_CHECK> list2 = actingCheckService.ACTING_CHECK_AM(strDate, nPANO);
                if (list2.Count > 0)
                {
                    ssChk.ActiveSheet.RowCount = list2.Count;
                    cSpd.SetfpsRowHeight(ssChk, 32);
                    for (int i = 0; i < list2.Count; i++)
                    {
                        ssChk.ActiveSheet.Cells[i, 0].Text = list2[i].NAME;
                        ssChk.ActiveSheet.Cells[i, 3].Text = list2[i].ENTPART;

                        string strChk = hicResultActiveService.GetActiveByPanoEntPart(nPANO, list2[i].ENTPART);
                        //상태점검
                        if (strChk == "Y")
                        {
                            nCNT2 += 1;
                            ssChk.ActiveSheet.Cells[i, 1].Text = "완료";
                            ssChk.ActiveSheet.Cells[i, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            ssChk.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            nCNT1 += 1;
                            ssChk.ActiveSheet.Cells[i, 1].Text = "미검";
                            ssChk.ActiveSheet.Cells[i, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            ssChk.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        //대기점검
                        List<HIC_RESULT> list3 = hicResultService.GetWaitCountByPart(list2[i].ENTPART); 
                        if (list3.Count > 0)
                        {
                            ssChk.ActiveSheet.Cells[i, 2].Text = list3.Count.To<string>("0");
                        }
                        else
                        {
                            ssChk.ActiveSheet.Cells[i, 2].Text = "0";
                        }
                    }
                }

                //2차정밀청력 대상자 표시
                if (hicSunapService.GetSunapAmtbyWrtNo(nWRTNO) > 0)
                {
                    if (hicResultService.GetExCodebyWrtNo(nWRTNO) > 0)
                    {
                        if (hicResultService.GetExCodebyWrtNo_Second(nWRTNO) > 0)
                        {
                            ssChk.ActiveSheet.RowCount += 1;
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "2차 정밀청력 수납대상";
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 1].Text = "미검";
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }

                //미검을 상단에 표시
                clsSpread.gSpdSortRow(ssChk, 1, ref boolSort, true);

                if (nCNT1 > 0)
                {
                    panMsg.Text = "미수검 항목이 있습니다.";
                }
                else if (nCNT2 > 0)
                {
                    panMsg.Text = "수검 완료";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                timer1.Stop();
                this.Close();
                return;
            }
        }

        private void eTimer_Timer(object sender, EventArgs e)
        {
            FnTimerCnt += 1;
            if (FnTimerCnt < 20)
            {
                return;
            }

            timer1.Stop();
            this.Close();
            return;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cSpd.Spread_Clear_Simple(ssChk, 1);

            FnTimerCnt = 0;
            lblSName.Text = "";
            panMsg.Text = "";
            panRemark.Text = "";

            timer1.Start();

            txtWRTNO.Text = "";
            //txtWRTNO.Focus();

            this.ActiveControl = txtWRTNO;
        }
    }
}
