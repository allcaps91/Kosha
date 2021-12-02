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
/// File Name       : frmHaRsvAdvanceReceivedList.cs
/// Description     : 종검예약선수금 관리화면
/// Author          : 이상훈
/// Create Date     : 2019-10-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain83.frm(Frm예약선수금명단)" />

namespace HC_Main
{
    public partial class frmHaRsvAdvanceReceivedList : Form
    {
        HeaJepsuService heaJepsuService = null;
        HeaSunapService heaSunapService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        clsHaBase ha = new clsHaBase();
        ComFunc cf = new ComFunc();

        public frmHaRsvAdvanceReceivedList()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            heaSunapService = new HeaSunapService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1000).ToShortDateString();
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
                int nREAD = 0;
                int nRow = 0;
                string strFDate = "";
                string strTDate = "";

                strFDate = dtpFrDate.Text;
                strTDate = dtpToDate.Text;

                List<HEA_JEPSU> list = heaJepsuService.GetItembySuDateGbSts(strFDate, strTDate);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;

                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    SS1.ActiveSheet.Cells[nRow, 0].Text = list[i].SNAME.Trim();
                    SS1.ActiveSheet.Cells[nRow, 1].Text = list[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[nRow, 2].Text = list[i].WRTNO.ToString();

                    HEA_SUNAP sunaplist = heaSunapService.GetAmtbyWrtNo(list[i].WRTNO);

                    if (sunaplist != null)
                    {
                        SS1.ActiveSheet.Cells[nRow, 3].Text = sunaplist.TOTAMT.ToString();
                        SS1.ActiveSheet.Cells[nRow, 4].Text = sunaplist.HALINAMT.ToString();
                        SS1.ActiveSheet.Cells[nRow, 5].Text = sunaplist.LTDAMT.ToString();
                        SS1.ActiveSheet.Cells[nRow, 6].Text = sunaplist.BONINAMT.ToString();
                        SS1.ActiveSheet.Cells[nRow, 7].Text = sunaplist.CASHAMT.ToString();
                        SS1.ActiveSheet.Cells[nRow, 8].Text = sunaplist.CARDAMT.ToString();
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

                strTitle = "종합검진 예약선수금액";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("수납기간:" + dtpFrDate.Text + "~" + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }
    }
}
