using ComBase;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcCreditCardReport.cs
/// Description     : 신용카드레포트
/// Author          : 심명섭
/// Create Date     : 2021-05-27
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > Frm신용카드레포트1(Frm신용카드레포트1.frm)" />

namespace HC_Tong
{
    public partial class frmHcCreditCardReport :BaseForm
    {
        // Spread
        clsSpread cSpd = null;
        // 약한참조

        CardApprovCenterService cardApprovCenterService = null;


        public frmHcCreditCardReport()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            // Service 선언 
            // Service -> Repository

            cardApprovCenterService = new CardApprovCenterService();
            cSpd = new clsSpread();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                DisPlay_Screen();
            }
            else if (sender == btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Spread_Print();
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            
        }

        private void DisPlay_Screen()
        {
            int nRow = 0;
            long nTotAmt = 0;
            long nSoAmt = 0;
            string strInPut_New = "";
            string strInPut_Old = "";
            string strSaBun = "";
            int rdoBtnCheck = 0;

            strSaBun = txtSabun.Text.Trim();

            if (rdoGubun1.Checked)
            {
                rdoBtnCheck = 1;
            }
            else if (rdoGubun2.Checked)
            {
                rdoBtnCheck = 2;
            }
            else
            {
                rdoBtnCheck = 3;
            }

            cSpd.Spread_All_Clear(SS1);

            List<CARD_APPROV_CENTER> cardReportData = cardApprovCenterService.GetCardReportData(dtpDate.Text, rdoBtnCheck, strSaBun);

            if(cardReportData.Count == 0)
            {   
                MessageBox.Show("데이터가 없습니다.");
            }

            for(int i = 0; i < cardReportData.Count; i++)
            {
                nRow += 1;
                if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }
           
                
                strInPut_New = cardReportData[i].INPUTMETHOD;

                if(strInPut_New != strInPut_Old)
                {
                    if(strInPut_New == "S")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "PC 승인";
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "소계";
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Format(nSoAmt, "###,###,###,##0 ");

                        nSoAmt = 0;

                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }

                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "단말기 승인";
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                    }

                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }

                    strInPut_Old = strInPut_New;
                }

                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = cardReportData[i].ACCEPTERNAME;
                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Format(cardReportData[i].TAMT, "###,###,###,##0 ");
                nTotAmt += cardReportData[i].TAMT;
                nSoAmt += cardReportData[i].TAMT;
                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "";
            }

            nRow += 1;
            if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }
            
            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "소계";
            //SS1.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SS1.ActiveSheet.ColumnCount - 1].BackColor = Color.Orange;
            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Format(nSoAmt, "###,###,###,##0 ");

            nRow += 1;
            if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }
            SS1.ActiveSheet.Cells[nRow - 1 , 0].Text = "총계";
            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = VB.Format(nTotAmt, "###,###,###,##0 ");
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

            strTitle = "카드사별 수입집계표";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += cSpd.setSpdPrint_String("┌─┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 과  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  ├────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│재│        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("└─┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}