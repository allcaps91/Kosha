using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaSabunSunap.cs
/// Description     : 수납자별 수납집계표
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain13.frm(FrmSabunSunap)" />

namespace HC_Main
{
    public partial class frmHaSabunSunap : Form
    {
        HeaSunapService heaSunapService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHaSabunSunap()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            heaSunapService = new HeaSunapService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            SS1.ActiveSheet.SetRowHeight(-1, 15);
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nRow = 0;
                int nREAD = 0;
                long[] nTotAmt = new long[7];

                if (string.Compare(dtpToDate.Text, dtpFrDate.Text) < 0)
                {
                    MessageBox.Show("종료일자가 시작일보다 적음", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //누적할 배열을 Clear
                for (int i = 0; i < 7; i++)
                {
                    nTotAmt[i] = 0;
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 20;

                //수납금액을 SELECT
                List<HEA_SUNAP> list = heaSunapService.GetSumbySuDate(dtpFrDate.Text, dtpToDate.Text);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;

                if (nREAD > 0)
                {
                    SS1_Sheet1.Rows.Get(-1).Height = 24;

                    for (int i = 0; i < nREAD; i++)
                    {   
                        SS1.ActiveSheet.Cells[i, 0].Text = list[i].JOBSABUN.ToString();
                        SS1.ActiveSheet.Cells[i, 1].Text = cf.Read_SabunName(clsDB.DbCon, list[i].JOBSABUN.ToString());
                        SS1.ActiveSheet.Cells[i, 2].Text = string.Format("{0:N0}", list[i].SUNAPAMT1);
                        SS1.ActiveSheet.Cells[i, 3].Text = string.Format("{0:N0}", list[i].SUNAPAMT2);
                        SS1.ActiveSheet.Cells[i, 4].Text = string.Format("{0:N0}", list[i].TOTAMT);
                        SS1.ActiveSheet.Cells[i, 5].Text = string.Format("{0:N0}", list[i].LTDAMT);
                        SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:N0}", list[i].BONINAMT);
                        SS1.ActiveSheet.Cells[i, 7].Text = string.Format("{0:N0}", list[i].HALINAMT);
                        SS1.ActiveSheet.Cells[i, 8].Text = string.Format("{0:N0}", list[i].MISUAMT);

                        nTotAmt[0] += list[i].SUNAPAMT1;
                        nTotAmt[1] += list[i].SUNAPAMT2;
                        nTotAmt[2] += list[i].TOTAMT;
                        nTotAmt[3] += list[i].LTDAMT;
                        nTotAmt[4] += list[i].BONINAMT;
                        nTotAmt[5] += list[i].HALINAMT;
                        nTotAmt[6] += list[i].MISUAMT;
                    }
                    SS1.ActiveSheet.RowCount += 1;
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount- 1, 1].Text = "합계";
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 1].BackColor = Color.Aqua;
                    for (int i = 0; i < 7; i++)
                    {
                        SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i + 2].Text = string.Format("{0:N0}", nTotAmt[i]);
                        SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i + 2].BackColor = Color.Bisque;
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "수납자별 수납집계표";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간:" + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }
    }
}
