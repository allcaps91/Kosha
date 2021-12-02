using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_Tong.Dto;
using HC_Tong.Service;
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcDailyilzi.cs
/// Description     : 일별 업무일지(통계기준)
/// Author          : 심명섭
/// Create Date     : 2021-06-04
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > frmHcDailyilzi (HcTong07.frm)" />
/// 
namespace HC_Tong
{
    public partial class frmHcDailyilzi :BaseForm
    {
        // Spread
        clsSpread cSpd = null;
        HicTongDailyService  hicTongDailyService = null;
        HicTongamService hicTongamService = null;
        ComFunc CF = null;
        HicCresvService hicCresvService = null;
        HicLtdService hicLtdService = null;
        HicBoresvService hicBoresvService = null;
        HicChulresvService hicChulresvService = null;
        public frmHcDailyilzi()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicTongDailyService = new HicTongDailyService();
            hicTongamService = new HicTongamService();
            CF = new ComFunc();
            hicCresvService = new HicCresvService();
            hicLtdService = new HicLtdService();
            hicBoresvService = new HicBoresvService();
            hicChulresvService = new HicChulresvService();
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

        private void Spread_Print()
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            string strYYMMDD = VB.Left(dtpFDate.Text, 4) + "년" + VB.Mid(dtpFDate.Text, 6, 2) + "월" + VB.Right(dtpFDate.Text, 2) + "일";

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (rdoGubun1.Checked)
            {
                strTitle = "업          무          일          지";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strHeader += cSpd.setSpdPrint_String("┌─┬────┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("│  ├────┼────┼────┼────┼────┤", new Font("굴림체" , 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("│  │        │        │        │ 전  결 │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("│재│        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String("└─┴────┴────┴────┴────┴────┘" , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += cSpd.setSpdPrint_String(strYYMMDD, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                strTitle = "업          무          일          지";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
                

                strHeader += cSpd.setSpdPrint_String("┌─┬────┬────┬────┬────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String("│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String("│  ├────┼────┼────┼────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String("│  │        │        │        │ 전  결 │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String("│재│        │        │        │        │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String("└─┴────┴────┴────┴────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += cSpd.setSpdPrint_String(strYYMMDD, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }


            
        }

        private void DisPlay_Screen()
        {
            int Row, Col = 0;
            int nRow = 0;
            long nCol = 0;
            long nJepCNT = 0;                       // 접수인원
            long [] nTot = new long[7];
            long nCnt1, nCnt2, nCnt3, nCnt4 = 0;
            string strNextDate = "";
            string LtdName = "";

            for (int i = 0; i <= 6; i++) {
                nTot[i] = 0;
            }

            for (int i = 4; i <= 5; i++)
            {
                for (int j = 3; j <= 27; j++)
                {
                    SS1.ActiveSheet.Cells[i - 1, j - 1].Text = "";
                }
            }

            for (int i = 6; i <= 10; i++)
            {
                for (int j = 3; j <= 19; j++)
                {
                    SS1.ActiveSheet.Cells[i - 1, j - 1].Text = "";
                }
            }

            for (int i = 3; i <= 28; i++)
            {
                Col = i - 1;
                for (int j = 13; j <= 14; j++)
                {
                    Row = j - 1;
                    SS1.ActiveSheet.Cells[Row, Col].Text = "";
                }
            }

            SS1.ActiveSheet.Cells[6, 20].Text = "";
            SS1.ActiveSheet.Cells[6, 22].Text = "";
            SS1.ActiveSheet.Cells[6, 24].Text = "";
            SS1.ActiveSheet.Cells[6, 27].Text = "";

            SS1.ActiveSheet.Cells[7, 20].Text = "";
            SS1.ActiveSheet.Cells[7, 22].Text = "";
            SS1.ActiveSheet.Cells[7, 24].Text = "";
            SS1.ActiveSheet.Cells[7, 27].Text = "";

            SS1.ActiveSheet.Cells[8, 20].Text = "";
            SS1.ActiveSheet.Cells[8, 22].Text = "";
            SS1.ActiveSheet.Cells[8, 24].Text = "";
            SS1.ActiveSheet.Cells[8, 27].Text = "";

            // 자료를 SELECT
            List<HIC_TONGDAILY> TongData = hicTongDailyService.GetTongData(dtpFDate.Text, dtpTDate.Text);

            nRow = 0;
            for (int i = 0; i < TongData.Count; i++)
            {
                nJepCNT = TongData[i].JEPCNT;

                switch (TongData[i].CHASU.Trim())
                {
                    case "1":
                    case "3":
                        SS1.ActiveSheet.Cells[7, 24].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 24].Text) + nJepCNT).To<string>("0");      // 1차
                        switch (TongData[i].GBCHUL.Trim())
                        {
                            case "N":
                                SS1.ActiveSheet.Cells[7, 20].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 20].Text) + nJepCNT).To<string>("0");    // 1차 내원
                                nTot[2] += nJepCNT;
                                nTot[4] += nJepCNT;
                                switch (TongData[i].GJJONG.Trim())
                                {
                                    case "11":
                                    case "61":
                                        SS1.ActiveSheet.Cells[3, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 2].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "12":
                                    case "62":
                                        SS1.ActiveSheet.Cells[3, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 3].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "13":
                                    case "63":
                                        SS1.ActiveSheet.Cells[3, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 4].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "21":
                                    case "64":
                                        SS1.ActiveSheet.Cells[3, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 5].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "22":
                                    case "65":
                                        SS1.ActiveSheet.Cells[3, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 6].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "23":
                                    case "66":
                                        SS1.ActiveSheet.Cells[3, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 7].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "31":
                                    case "67":
                                        SS1.ActiveSheet.Cells[3, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 8].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "32":
                                    case "68":
                                        SS1.ActiveSheet.Cells[3, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 9].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "33":
                                        SS1.ActiveSheet.Cells[3, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 10].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "34":
                                        SS1.ActiveSheet.Cells[3, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 11].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "35":
                                        SS1.ActiveSheet.Cells[3, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 12].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "36":
                                        SS1.ActiveSheet.Cells[3, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 13].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "37":
                                        SS1.ActiveSheet.Cells[3, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 14].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "49":
                                    case "69":
                                        SS1.ActiveSheet.Cells[3, 26].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 26].Text) + nJepCNT).To<string>("0");
                                        nTot[0] += nJepCNT;
                                        break;
                                    case "51":
                                        SS1.ActiveSheet.Cells[3, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "52":
                                        SS1.ActiveSheet.Cells[3, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "53":
                                        SS1.ActiveSheet.Cells[3, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 16].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "54":
                                        SS1.ActiveSheet.Cells[3, 17].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 17].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "55":
                                        SS1.ActiveSheet.Cells[3, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 15].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "56":
                                        SS1.ActiveSheet.Cells[3, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                }
                                break;
                            case "Y":
                                SS1.ActiveSheet.Cells[7, 22].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 22].Text) + nJepCNT).To<string>("0");      // 1차 출장
                                nTot[3] += nJepCNT;
                                nTot[4] += nJepCNT;
                                switch (TongData[i].GJJONG.Trim())
                                {
                                    case "11":
                                    case "61":
                                        SS1.ActiveSheet.Cells[5, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 2].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "12":
                                    case "62":
                                        SS1.ActiveSheet.Cells[5, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 3].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "13":
                                    case "63":
                                        SS1.ActiveSheet.Cells[5, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 4].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "21":
                                    case "64":
                                        SS1.ActiveSheet.Cells[5, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 5].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "22":
                                    case "65":
                                        SS1.ActiveSheet.Cells[5, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 6].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "23":
                                    case "66":
                                        SS1.ActiveSheet.Cells[5, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 7].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "31":
                                    case "67":
                                        SS1.ActiveSheet.Cells[5, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 8].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "32":
                                    case "68":
                                        SS1.ActiveSheet.Cells[5, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 9].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "33":
                                        SS1.ActiveSheet.Cells[5, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 10].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "34":
                                        SS1.ActiveSheet.Cells[5, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 11].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "35":
                                        SS1.ActiveSheet.Cells[5, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 12].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "36":
                                        SS1.ActiveSheet.Cells[5, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 13].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "37":
                                        SS1.ActiveSheet.Cells[5, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 14].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "55":
                                        SS1.ActiveSheet.Cells[5, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 15].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "56":
                                        SS1.ActiveSheet.Cells[5, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "53":
                                        SS1.ActiveSheet.Cells[5, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[5, 16].Text) + nJepCNT).To<string>("0");
                                        break;
                                }
                                break;
                        }
                        break;
                    case "2":
                        SS1.ActiveSheet.Cells[8, 24].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 24].Text) + nJepCNT).To<string>("0");      // 2차
                        switch (TongData[i].GBCHUL.Trim())
                        {
                            case "N":
                                SS1.ActiveSheet.Cells[8, 20].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 20].Text) + nJepCNT).To<string>("0");    // 2차 내원
                                nTot[2] += nJepCNT;
                                nTot[4] += nJepCNT;
                                switch (TongData[i].GJJONG.Trim())
                                {
                                    case "11":
                                    case "61":
                                        SS1.ActiveSheet.Cells[4, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[3, 2].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "12":
                                    case "62":
                                        SS1.ActiveSheet.Cells[4, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 3].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "13":
                                    case "63":
                                        SS1.ActiveSheet.Cells[4, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 4].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "21":
                                    case "64":
                                        SS1.ActiveSheet.Cells[4, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 5].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "22":
                                    case "65":
                                        SS1.ActiveSheet.Cells[4, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 6].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "23":
                                    case "66":
                                        SS1.ActiveSheet.Cells[4, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 7].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "31":
                                    case "67":
                                        SS1.ActiveSheet.Cells[4, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 8].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "32":
                                    case "68":
                                        SS1.ActiveSheet.Cells[4, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 9].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "33":
                                        SS1.ActiveSheet.Cells[4, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 10].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "34":
                                        SS1.ActiveSheet.Cells[4, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 11].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "35":
                                        SS1.ActiveSheet.Cells[4, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 12].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "36":
                                        SS1.ActiveSheet.Cells[4, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 13].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "37":
                                        SS1.ActiveSheet.Cells[4, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 14].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "49":
                                    case "69":
                                        SS1.ActiveSheet.Cells[4, 26].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 26].Text) + nJepCNT).To<string>("0");
                                        nTot[1] += nJepCNT;
                                        break;
                                    case "51":
                                        SS1.ActiveSheet.Cells[4, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "52":
                                        SS1.ActiveSheet.Cells[4, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "53":
                                        SS1.ActiveSheet.Cells[4, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 16].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "54":
                                        SS1.ActiveSheet.Cells[4, 17].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 17].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "55":
                                        SS1.ActiveSheet.Cells[4, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 15].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "56":
                                        SS1.ActiveSheet.Cells[4, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[4, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                }
                                break;
                            case "Y":
                                SS1.ActiveSheet.Cells[8, 22].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 22].Text) + nJepCNT).To<string>("0");      // 2차 출장
                                nTot[3] += nJepCNT;
                                nTot[4] += nJepCNT;
                                switch (TongData[i].GJJONG.Trim())
                                {
                                    case "11":
                                    case "61":
                                        SS1.ActiveSheet.Cells[6, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 2].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "12":
                                    case "62":
                                        SS1.ActiveSheet.Cells[6, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 3].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "13":
                                    case "63":
                                        SS1.ActiveSheet.Cells[6, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 4].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "21":
                                    case "64":
                                        SS1.ActiveSheet.Cells[6, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 5].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "22":
                                    case "65":
                                        SS1.ActiveSheet.Cells[6, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 6].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "23":
                                    case "66":
                                        SS1.ActiveSheet.Cells[6, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 7].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "31":
                                    case "67":
                                        SS1.ActiveSheet.Cells[6, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 8].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "32":
                                    case "68":
                                        SS1.ActiveSheet.Cells[6, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 9].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "33":
                                        SS1.ActiveSheet.Cells[6, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 10].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "34":
                                        SS1.ActiveSheet.Cells[6, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 11].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "35":
                                        SS1.ActiveSheet.Cells[6, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 12].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "36":
                                        SS1.ActiveSheet.Cells[6, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 13].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "37":
                                        SS1.ActiveSheet.Cells[6, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 14].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "55":
                                        SS1.ActiveSheet.Cells[6, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 15].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "56":
                                        SS1.ActiveSheet.Cells[6, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 18].Text) + nJepCNT).To<string>("0");
                                        break;
                                    case "53":
                                        SS1.ActiveSheet.Cells[6, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[6, 16].Text) + nJepCNT).To<string>("0");
                                        break;
                                }
                                break;
                        }
                        break;
                }

                switch (TongData[i].CHASU.Trim())
                {
                    case "1":                                       // 1차 (내원 및 출장)
                    case "3":
                        switch (TongData[i].GJJONG.Trim())
                        {
                            case "11":
                            case "61":
                                SS1.ActiveSheet.Cells[7, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 2].Text) + nJepCNT).To<string>("0");
                                break;
                            case "12":
                            case "62":
                                SS1.ActiveSheet.Cells[7, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 3].Text) + nJepCNT).To<string>("0");
                                break;
                            case "13":
                            case "63":
                                SS1.ActiveSheet.Cells[7, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 4].Text) + nJepCNT).To<string>("0");
                                break;
                            case "21":
                            case "64":
                                SS1.ActiveSheet.Cells[7, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 5].Text) + nJepCNT).To<string>("0");
                                break;
                            case "22":
                            case "65":
                                SS1.ActiveSheet.Cells[7, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 6].Text) + nJepCNT).To<string>("0");
                                break;
                            case "23":
                            case "66":
                                SS1.ActiveSheet.Cells[7, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 7].Text) + nJepCNT).To<string>("0");
                                break;
                            case "31":
                            case "67":
                                SS1.ActiveSheet.Cells[7, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 8].Text) + nJepCNT).To<string>("0");
                                break;
                            case "32":
                            case "68":
                                SS1.ActiveSheet.Cells[7, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 9].Text) + nJepCNT).To<string>("0");
                                break;
                            case "33":
                                SS1.ActiveSheet.Cells[7, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 10].Text) + nJepCNT).To<string>("0");
                                break;
                            case "34":
                                SS1.ActiveSheet.Cells[7, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 11].Text) + nJepCNT).To<string>("0");
                                break;
                            case "35":
                                SS1.ActiveSheet.Cells[7, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 12].Text) + nJepCNT).To<string>("0");
                                break;
                            case "36":
                                SS1.ActiveSheet.Cells[7, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 13].Text) + nJepCNT).To<string>("0");
                                break;
                            case "37":
                                SS1.ActiveSheet.Cells[7, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 14].Text) + nJepCNT).To<string>("0");
                                break;
                            case "53":
                                SS1.ActiveSheet.Cells[7, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 16].Text) + nJepCNT).To<string>("0");
                                break;
                            case "54":
                                SS1.ActiveSheet.Cells[7, 17].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 17].Text) + nJepCNT).To<string>("0");
                                break;
                            case "55":
                                SS1.ActiveSheet.Cells[7, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 15].Text) + nJepCNT).To<string>("0");
                                break;
                            case "56":
                                SS1.ActiveSheet.Cells[7, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[7, 18].Text) + nJepCNT).To<string>("0");
                                break;
                        }
                        break;
                    case "2":                                       // 2차 (내원 및 출장)
                        switch (TongData[i].GJJONG.Trim())
                        {
                            case "11":
                            case "61":
                                SS1.ActiveSheet.Cells[8, 2].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 2].Text) + nJepCNT).To<string>("0");
                                break;
                            case "12":
                            case "62":
                                SS1.ActiveSheet.Cells[8, 3].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 3].Text) + nJepCNT).To<string>("0");
                                break;
                            case "13":
                            case "63":
                                SS1.ActiveSheet.Cells[8, 4].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 4].Text) + nJepCNT).To<string>("0");
                                break;
                            case "21":
                            case "64":
                                SS1.ActiveSheet.Cells[8, 5].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 5].Text) + nJepCNT).To<string>("0");
                                break;
                            case "22":
                            case "65":
                                SS1.ActiveSheet.Cells[8, 6].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 6].Text) + nJepCNT).To<string>("0");
                                break;
                            case "23":
                            case "66":
                                SS1.ActiveSheet.Cells[8, 7].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 7].Text) + nJepCNT).To<string>("0");
                                break;
                            case "31":
                            case "67":
                                SS1.ActiveSheet.Cells[8, 8].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 8].Text) + nJepCNT).To<string>("0");
                                break;
                            case "32":
                            case "68":
                                SS1.ActiveSheet.Cells[8, 9].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 9].Text) + nJepCNT).To<string>("0");
                                break;
                            case "33":
                                SS1.ActiveSheet.Cells[8, 10].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 10].Text) + nJepCNT).To<string>("0");
                                break;
                            case "34":
                                SS1.ActiveSheet.Cells[8, 11].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 11].Text) + nJepCNT).To<string>("0");
                                break;
                            case "35":
                                SS1.ActiveSheet.Cells[8, 12].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 12].Text) + nJepCNT).To<string>("0");
                                break;
                            case "36":
                                SS1.ActiveSheet.Cells[8, 13].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 13].Text) + nJepCNT).To<string>("0");
                                break;
                            case "37":
                                SS1.ActiveSheet.Cells[8, 14].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 14].Text) + nJepCNT).To<string>("0");
                                break;
                            case "53":
                                SS1.ActiveSheet.Cells[8, 16].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 16].Text) + nJepCNT).To<string>("0");
                                break;
                            case "55":
                                SS1.ActiveSheet.Cells[8, 15].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 15].Text) + nJepCNT).To<string>("0");
                                break;
                            case "56":
                                SS1.ActiveSheet.Cells[8, 18].Text = (VB.Val(SS1.ActiveSheet.Cells[8, 18].Text) + nJepCNT).To<string>("0");
                                break;
                        }
                        break;
                }
            }

            if (nTot[2] == 0)
            {
                SS1.ActiveSheet.Cells[6, 20].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[6, 20].Text = nTot[2].To<string>("0");
            }

            if (nTot[3] == 0)
            {
                SS1.ActiveSheet.Cells[6, 22].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[6, 22].Text = nTot[3].To<string>("0");
            }

            if (nTot[4] == 0)
            {
                SS1.ActiveSheet.Cells[6, 24].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[6, 24].Text = nTot[4].To<string>("0");
            }

            // 암검진 건수
            List<HIC_TONGAM> CancerData = hicTongamService.GetCancerData(dtpFDate.Text, dtpTDate.Text);
            nRow = 0;
            Row = 3;
            for (int i = 0; i < CancerData.Count; i++)
            {
                SS1.ActiveSheet.Cells[Row, 19].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 19].Text) + CancerData[i].CNT1).To<string>("0");                  // 위1 
                SS1.ActiveSheet.Cells[Row, 20].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 20].Text) + CancerData[i].CNT2).To<string>("0");                  // 위2 
                SS1.ActiveSheet.Cells[Row, 21].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 21].Text) + CancerData[i].CNT5).To<string>("0");                  // 유방 
                SS1.ActiveSheet.Cells[Row, 22].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 22].Text) + CancerData[i].CNT3).To<string>("0");                  // 대장 
                SS1.ActiveSheet.Cells[Row, 23].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 23].Text) + CancerData[i].CNT4).To<string>("0");                  // 간 
                SS1.ActiveSheet.Cells[Row, 24].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 24].Text) + CancerData[i].CNT6).To<string>("0");                  // 자궁 
                SS1.ActiveSheet.Cells[Row, 25].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, 25].Text) + CancerData[i].CNT7).To<string>("0");                  // 폐 
            }

            // 종합건진 건수 SELECT
            List<HIC_TONGDAILY> TotalData = hicTongDailyService.GetTotalData(dtpFDate.Text, dtpTDate.Text);
            nRow = 0;
            for (int i = 0; i < TotalData.Count; i++)
            {
                nJepCNT = TotalData[i].JEPCNT;
                switch (TotalData[i].GJJONG.Trim())
                {
                    case "91":
                        Row = 8;
                        Col = 27;
                        SS1.ActiveSheet.Cells[Row, Col].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text) + nJepCNT).To<string>("0");
                        nTot[5] += nJepCNT;
                        break;
                    case "92":
                        Row = 7;
                        Col = 27;
                        SS1.ActiveSheet.Cells[Row, Col].Text = (VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text) + nJepCNT).To<string>("0");
                        nTot[5] += nJepCNT;
                        break;
                }
            }

            if (nTot[5] == 0)
            {
                Row = 6;
                Col = 27;
                SS1.ActiveSheet.Cells[Row, Col].Text = "";
            }
            else
            {
                Row = 6;
                Col = 27;
                SS1.ActiveSheet.Cells[Row, Col].Text = nTot[5].To<string>("0");
            }

            switch (VB.Left(CF.READ_YOIL(clsDB.DbCon, dtpFDate.Text), 2))
            {
                case "토":
                    strNextDate = CF.DATE_ADD(clsDB.DbCon, dtpFDate.Text, 2);
                    break;
                default:
                    strNextDate = CF.DATE_ADD(clsDB.DbCon, dtpFDate.Text, 1);
                    break;
            }

            // 현금수납금액
            List<HIC_TONGDAILY> CashData = hicTongDailyService.GetCashData(dtpFDate.Text, strNextDate);
            if (CashData.Count > 0)
            {
                if (CashData[0].SUNAPAMT > 0)
                {
                    Row = 9;
                    Col = 2;
                    SS1.ActiveSheet.Cells[Row, Col].Text = VB.Format(CashData[0].SUNAPAMT, "###,###,###,###,##0") + "원";
                }
            }
            
            // 작업환경 측정

            nCnt1 = 0;
            nCnt2 = 0;
            nCnt3 = 0;
            nCnt4 = 0;

            List<HIC_CRESV> Environment =  hicCresvService.GetEnvironment(dtpFDate.Text, strNextDate);

            for (int i = 0; i < Environment.Count; i++)
            {
                if(dtpFDate.Text == Environment[i].RDATE)
                {
                    Row = 12;
                    Col = 2;
                    nCnt1 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Environment[i].LTDCODE.To<string>("0"));
                    if(item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt1 + "." + LtdName + "[" + Environment[i].MAN + "]";
                }
                else if (strNextDate == Environment[i].RDATE)
                {
                    Row = 13;
                    Col = 2;
                    nCnt3 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Environment[i].LTDCODE.To<string>("0").Trim());
                    if (item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt3 + "." + LtdName + "[" + Environment[i].MAN + "]";
                }
            }

            // 보건관리대행

            nCnt1 = 0;
            nCnt2 = 0;
            nCnt3 = 0;
            nCnt4 = 0;

            List<HIC_BORESV> Health = hicBoresvService.GetHealth(dtpFDate.Text, strNextDate);
            for (int i = 0; i < Health.Count; i++)
            {
                if (dtpFDate.Text == Health[i].RDATE)
                {
                    Row = 12;
                    Col = 12;
                    nCnt1 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Health[i].LTDCODE.To<string>("0"));
                    if (item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt1 + "." + LtdName + "[" + Health[i].MAN + "]";
                }
                else if (strNextDate == Health[i].RDATE)
                {
                    Row = 13;
                    Col = 12;
                    nCnt3 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Health[i].LTDCODE.To<string>("0").Trim());
                    if (item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt3 + "." + LtdName + "[" + Health[i].MAN + "]";
                }
            }

            // 건강검진
            nCnt1 = 0;
            nCnt2 = 0;
            nCnt3 = 0;
            nCnt4 = 0;

            List<HIC_CHULRESV> Checkup = hicChulresvService.GetCheckup(dtpFDate.Text, strNextDate);
            for (int i = 0; i < Checkup.Count; i++)
            {
                if (dtpFDate.Value == Checkup[i].RDATE)
                {
                    Row = 12;
                    Col = 22;
                    nCnt1 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Checkup[i].LTDCODE.To<string>("0"));
                    if (item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt1 + "." + LtdName + ", ";
                }
                else if (Convert.ToDateTime(strNextDate) == Checkup[i].RDATE)
                {
                    Row = 13;
                    Col = 22;
                    nCnt3 += 1;

                    HIC_LTD item = hicLtdService.FindOne(Checkup[i].LTDCODE.To<string>("0").Trim());
                    if (item.GBGUKGO == "Y")
                    {
                        LtdName = "*" + item.NAME;
                    }
                    else
                    {
                        LtdName = item.NAME;
                    }

                    SS1.ActiveSheet.Cells[Row, Col].Text += " " + nCnt3 + "." + LtdName + ", ";
                }
            }




        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;
        }
    }
}
