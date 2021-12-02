using ComBase;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using HC_Tong.Model;
using HC_Tong.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcDailySummary.cs
/// Description     : 일별 검진종류별 수입통계
/// Author          : 심명섭
/// Create Date     : 2021-05-25
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > FrmDailySummary(HcTong02.frm)" />
namespace HC_Tong
{
    public partial class frmHcDailySummary :BaseForm
    {
        // 합계를 담을 배열
        double [] nTotAmt = new double[8];
        // Spread
        clsSpread cSpd = null;
        // 약한참조
        HicTongDailyOtherService hicTongDailyOtherService = null;

        public frmHcDailySummary()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            // Service 선언 
            // Service -> Repository
            // 프로퍼티 == Model
            hicTongDailyOtherService = new HicTongDailyOtherService();
            cSpd = new clsSpread();
        }

        
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }
        private void eFormLoad(object sender, EventArgs e)
        {
            // 이전 달로 설정
            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if(sender == btnExit)
            {
                this.Close();
                return;
            }
            else if(sender == btnSearch)
            {
                DisPlay_Screen();
            }
            else if(sender == btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Spread_Print();
            }
        }

        private void DisPlay_Screen()
        {
            if (string.Compare(dtpTDate.Text, dtpFDate.Text) < 0)
            {
                MessageBox.Show("종료일자가 시작일자보다 적음", "확인");
            }

            // 누적할 배열을 Clear
            for (int i = 0; i <= 7; i++)
            {
                nTotAmt[i] = 0;
            }

            // 자료를 SELECT
            List<HIC_TONGDAILY_OTHER> dailyData = hicTongDailyOtherService.GetTongData(dtpFDate.Text, dtpTDate.Text, rdoGubun1.Checked);
            
            for (int i = 0; i < dailyData.Count; i++)
            {
                // 열 갯수 설정
                SS1.Sheets[0].RowCount = dailyData.Count;
                
                // 첫번째 두번째 cell에는 글자 삽입
                SS1.ActiveSheet.Cells[i, 0].Text = VB.Format(dailyData[i].NAME);
                SS1.ActiveSheet.Cells[i, 1].Text = VB.Format(dailyData[i].CHASU);
            
                if (VB.Format(dailyData[i].GBCHUL) == "Y")
                {
                    SS1.ActiveSheet.Cells[i, 2].Text = "출장";
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 2].Text = "내원";
                }
            
                SS1.ActiveSheet.Cells[i, 3].Text = VB.Format(dailyData[i].JEPCNT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 4].Text = VB.Format(dailyData[i].TOTAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 5].Text = VB.Format(dailyData[i].JOHAPAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 6].Text = VB.Format(dailyData[i].LTDAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 7].Text = VB.Format(dailyData[i].BONINAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 8].Text = VB.Format(dailyData[i].HALINAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 9].Text = VB.Format(dailyData[i].MISUAMT, "###,###,###,##0 ");
                SS1.ActiveSheet.Cells[i, 10].Text = VB.Format(dailyData[i].SUNAPAMT, "###,###,###,##0 ");

                // 합계에 금액을 Add
                nTotAmt[0] += dailyData[i].JEPCNT;
                nTotAmt[1] += dailyData[i].TOTAMT;
                nTotAmt[2] += dailyData[i].JOHAPAMT;
                nTotAmt[3] += dailyData[i].LTDAMT;
                nTotAmt[4] += dailyData[i].BONINAMT;
                nTotAmt[5] += dailyData[i].HALINAMT;
                nTotAmt[6] += dailyData[i].MISUAMT;
                nTotAmt[7] += dailyData[i].SUNAPAMT;

            }

            // Row 제일 마지막 열 추가
            SS1.Sheets[0].RowCount = dailyData.Count + 1;
            // 합계금액을 마지막 cell에 뿌려 줌
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 0].Text = "합   계";
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 1].Text = "";
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 2].Text = "";
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 3].Text = VB.Format(nTotAmt[0], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 4].Text = VB.Format(nTotAmt[1], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 5].Text = VB.Format(nTotAmt[2], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 6].Text = VB.Format(nTotAmt[3], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 7].Text = VB.Format(nTotAmt[4], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 8].Text = VB.Format(nTotAmt[5], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 9].Text = VB.Format(nTotAmt[6], "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[SS1.Sheets[0].RowCount - 1, 10].Text = VB.Format(nTotAmt[7], "###,###,###,##0 ");

        }


        private void Spread_Print()
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "검 진 종 류 별 수 입 통 계";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += cSpd.setSpdPrint_String("┌─┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 과  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  ├────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│재│        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("└─┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
