using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using HC_Tong.Dto;
using HC_Tong.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcDailySuipTong.cs
/// Description     : 경리과수입통계
/// Author          : 심명섭
/// Create Date     : 2021-05-20
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > FrmDailySuipTong(HcTong05.frm)" />


namespace HC_Tong
{
    public partial class frmHcDailySuipTong :BaseForm
    // frm == form
    {
        // 약한참조를 위해 null로 초기화
        HicTongDailyService  hicTongDailyService = null;
        HicTongDailyService GetGjJongTotAmtSumByDate = null;
        HicTongDailyService GetMisuCashSum = null;
        HicTongDailyService GetCheckUpCash = null;
        // JOIN : ComHpcLibB -> HicMisuMstSlipService :: Hic조인하는테이블명_Service
        HicMisuMstSlipService hicMisuMstSlipService = null;

        // Spread
        clsSpread cSpd = null;

        //Variable Define
        double [,] FnAmt = new double[14, 9];

        public frmHcDailySuipTong()
        {
            InitializeComponent();
            // Event Area
            SetEvent();
            // Control Area
            SetControl();
        }

        private void SetControl()
        {
            // Service 선언 
            // Service -> Repository
            hicTongDailyService = new HicTongDailyService();
            GetGjJongTotAmtSumByDate = new HicTongDailyService();
            GetMisuCashSum = new HicTongDailyService();
            GetCheckUpCash = new HicTongDailyService();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            cSpd = new clsSpread();
        }

        private void SetEvent()
        {
            // Load
            this.Load += new EventHandler(eFormLoad);
            // 닫기
            this.btnExit.Click += new EventHandler(eBtnClick);
            // 조회
            this.btnSearch.Click += new EventHandler(eBtnClick);
            // 프린트
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
        // Click Event Area
            // 닫기
            if (sender == btnExit)
            {
                this.Close();
                // 리턴 필수
                return;
            }
            // 조회
            else if (sender == btnSearch)
            {
                DisPlay_Screen();
            }
            // 프린트
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
        // Print Btn
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "수납 집계표(건강증진센터)";
            strSign = " ▶ 재무회계팀 확인 :        ";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strHeader += cSpd.setSpdPrint_String(VB.Space(38) + "┌─┬────┬────┬────┬────┬─────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String(VB.Space(38) + "│결│ 담  당 │ 계  장 │ 팀  장 │ 부  장 │ 행정처장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String(VB.Space(38) + "│  ├────┼────┼────┼────┼─────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text + VB.Space(4)+"│  │        │        │        │        │          │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(11) + "│재│        │        │        │        │          │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String(VB.Space(38) + "└─┴────┴────┴────┴────┴─────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            
            
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void DisPlay_Screen()
        // Search Btn 
        {
            long nJong           = 0;

            long nYeyakAmt       = 0;
            long nYDaeche        = 0;

            long nCardAmt1       = 0;
            long nCardAmt2       = 0;

            long nMisu_Ipgum1    = 0; //현금입금
            long nMisu_Ipgum2    = 0; //카드입금

            // 프린트 가능여부 확인 변수
            bool PstrPrint = true;

            // 최하단 msgBox 초기화
            lblMsg.Text = "";

            // 사용할 배열 초기화 , 안하면 값 버튼클릭 시 금액 누적 됨
            for(int i = 0; i < 14; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            // 수납마감금액이 맞지 않으면 출력 불가
            if ( PstrPrint == false)
            {
                MessageBox.Show("수납마감금액이 맞지 않습니다. 집계표 인쇄불가!!", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2013년 이전 자료 조회 불가
            if(string.Compare(dtpFDate.Text, "2003-12-31") < 0)
            {
                MessageBox.Show("2014년도 이후 작업만 가능합니다.");
                return;
            }

            // 최종작업시간 : ENTTIME , 사번 : JOBSABUN
            // 사번과 작업시간을 가져와 item에 저장
            HIC_TONGDAILY item = hicTongDailyService.GetEntTimeJobSabunByDate(dtpFDate.Text, dtpTDate.Text);
            
            // 널이 아닐 때
            if (!item.IsNullOrEmpty())
            {
                lblMsg.Text = "최종통계 형성 시각 : " + item.ENTTIME + " [" + clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, item.JOBSABUN.ToString()) + "]";
            }
           
            
            // 각 칼럼명이 DTO에 프로퍼티로 선언이 되어있는지 확인 필요, 여러 자료를 가지고오는 List생성
            // 자료를 SELECT
            List<HIC_TONGDAILY> dailyData = hicTongDailyService.GetSumDailyData(dtpFDate.Text, dtpTDate.Text, "1");

            for (int i = 0; i < dailyData.Count; i++)
            {
                if (string.Compare(dtpFDate.Text, "2003-12-31") >= 0)
                {
                    switch (dailyData[i].GJJONG.To<string>(""))
                    {
                        case "13":
                        case "18":
                        case "43":
                        case "46":
                            nJong = 1;      // 공단검진
                            break;

                        case "12":
                        case "17":
                        case "42":
                        case "45":
                            nJong = 1;      // 공무원
                            break;

                        case "81":
                            nJong = 6;      // 작업환경측정
                            break;

                        case "82":
                            nJong = 7;      // 보건관리대행
                            break;

                        case "83":
                            nJong = 9;      // 종합검진
                            break;

                        case "31":
                        case "35":
                            nJong = 2;      // 암검진
                            break;

                        case "56":
                            nJong = 3;      // 학생검진
                            break;

                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "63":
                        case "64":
                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "70":
                        case "71":
                        case "72":
                        case "73":
                        case "74":
                        case "75":
                        case "76":
                        case "77":
                        case "78":
                        case "79":
                        case "80":
                            nJong = 4;      // 기타
                            break;
                        default:
                            nJong = 1;      // 사업장
                            break;
                    }
                }
                else
                {
                    switch (dailyData[i].GJJONG.To<string>(""))
                    {
                        case "13":
                        case "18":
                            nJong = 1;      // 공단검진
                            break;

                        case "12":
                        case "17":
                            nJong = 1;      // 공무원
                            break;

                        case "81":
                            nJong = 6;      // 작업환경측정
                            break;

                        case "82":
                            nJong = 7;      // 보건관리대행
                            break;

                        case "83":
                            nJong = 9;      // 종합검진
                            break;

                        case "31":
                            nJong = 2;      // 암검진
                            break;

                        case "56":
                            nJong = 3;      // 학생검진
                            break;

                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                        case "38":
                        case "39":
                        case "40":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                        case "51":
                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "63":
                        case "64":
                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "69":
                        case "70":
                        case "71":
                        case "72":
                        case "73":
                        case "74":
                        case "75":
                        case "76":
                        case "77":
                        case "78":
                        case "79":
                        case "80":
                            nJong = 4;      // 기타
                            break;
                        default:
                            nJong = 1;      // 사업장
                            break;
                    }
                        
                }

                //종류별 계
                // 각 배열에 값 저장
                FnAmt[nJong, 1] = FnAmt[nJong, 1] + dailyData[i].TOTAMT;
                FnAmt[nJong, 2] = FnAmt[nJong, 2] + dailyData[i].JOHAPAMT;
                FnAmt[nJong, 3] = FnAmt[nJong, 3] + dailyData[i].LTDAMT;
                FnAmt[nJong, 4] = FnAmt[nJong, 4] + dailyData[i].BONINAMT;
                FnAmt[nJong, 5] = FnAmt[nJong, 5] + dailyData[i].MISUAMT;
                FnAmt[nJong, 6] = FnAmt[nJong, 6] + dailyData[i].HALINAMT;
                FnAmt[nJong, 7] = FnAmt[nJong, 7] + dailyData[i].SUNAPAMT;

                //종검 예약선수금,선수금 대체액
                nYeyakAmt += dailyData[i].YEYAKAMT;
                nYDaeche += dailyData[i].YDAECHE;
                    
                // 소계
                if(nJong < 5)
                {
                    FnAmt[5, 1] = FnAmt[5, 1] + dailyData[i].TOTAMT;
                    FnAmt[5, 2] = FnAmt[5, 2] + dailyData[i].JOHAPAMT;
                    FnAmt[5, 3] = FnAmt[5, 3] + dailyData[i].LTDAMT;
                    FnAmt[5, 4] = FnAmt[5, 4] + dailyData[i].BONINAMT;
                    FnAmt[5, 5] = FnAmt[5, 5] + dailyData[i].MISUAMT;
                    FnAmt[5, 6] = FnAmt[5, 6] + dailyData[i].HALINAMT;
                    FnAmt[5, 7] = FnAmt[5, 7] + dailyData[i].SUNAPAMT;
                }
                else if(nJong < 8)
                {
                    FnAmt[8, 1] = FnAmt[8, 1] + dailyData[i].TOTAMT;
                    FnAmt[8, 2] = FnAmt[8, 2] + dailyData[i].JOHAPAMT;
                    FnAmt[8, 3] = FnAmt[8, 3] + dailyData[i].LTDAMT;
                    FnAmt[8, 4] = FnAmt[8, 4] + dailyData[i].BONINAMT;
                    FnAmt[8, 5] = FnAmt[8, 5] + dailyData[i].MISUAMT;
                    FnAmt[8, 6] = FnAmt[8, 6] + dailyData[i].HALINAMT;
                    FnAmt[8, 7] = FnAmt[8, 7] + dailyData[i].SUNAPAMT;
                }

                // 전체합계
                FnAmt[10, 1] = FnAmt[10, 1] + dailyData[i].TOTAMT;
                FnAmt[10, 2] = FnAmt[10, 2] + dailyData[i].JOHAPAMT;
                FnAmt[10, 3] = FnAmt[10, 3] + dailyData[i].LTDAMT;
                FnAmt[10, 4] = FnAmt[10, 4] + dailyData[i].BONINAMT;
                FnAmt[10, 5] = FnAmt[10, 5] + dailyData[i].MISUAMT;
                FnAmt[10, 6] = FnAmt[10, 6] + dailyData[i].HALINAMT;
                FnAmt[10, 7] = FnAmt[10, 7] + dailyData[i].SUNAPAMT;

                // 카드금액 SUM , 혹시 모를 공백이 있을 수 있으니 비교 전 Trim
                if (dailyData[i].GJJONG.To<string>("").Trim() != "83" )
                {
                    nCardAmt1 = nCardAmt1 + dailyData[i].SUNAPAMT2;   // 일검
                }
                else
                {
                    nCardAmt2 = nCardAmt2 + dailyData[i].SUNAPAMT2;   // 종검
                }

                // 본인부담금액과 수납금액이 서로 맞지 않으면 인쇄불가!!
                // 2013-02-19 이양재 과장님요청 김민철 작업
                if(dailyData[i].BONINAMT + dailyData[i].MISUAMT + dailyData[i].YEYAKAMT + dailyData[i].YDAECHE != dailyData[i].SUNAPAMT )
                {
                    PstrPrint = false;
                }
            }

            // Join 테이블
            //미수입금액을 누적(현금)
            List<HIC_MISU_MST_SLIP> misuCashSum = hicMisuMstSlipService.GetMisuCashSum(dtpFDate.Text, dtpTDate.Text);

            for (int i = 0; i < misuCashSum.Count; i++)
            {
                if (string.Compare(dtpFDate.Text, "2020-03-01") >= 0)
                {
                    switch (misuCashSum[i].GJONG.To<string>(""))
                    {
                        case "13":
                        case "18":
                            nJong = 1;      // 공단검진
                            break;

                        case "12":
                        case "17":
                            nJong = 1;      // 공무원
                            break;

                        case "81":
                            nJong = 6;      // 작업환경측정
                            break;

                        case "82":
                            nJong = 7;      // 보건관리대행
                            break;

                        case "83":
                            nJong = 9;      // 종합검진
                            break;

                        case "31":
                            nJong = 2;      // 암검진
                            break;

                        case "56":
                            nJong = 3;      // 학생검진
                            break;

                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "62":
                        case "63":
                        case "64":
                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "70":
                        case "71":
                        case "72":
                        case "73":
                        case "74":
                        case "75":
                        case "76":
                        case "77":
                        case "78":
                        case "79":
                        case "80":
                            nJong = 4;      // 기타
                            break;
                        default:
                            nJong = 1;      // 사업장
                            break;
                    }
                }
                else
                {
                    switch (misuCashSum[i].GJONG.To<string>(""))
                    {
                        case "13":
                        case "18":
                            nJong = 1;      // 공단검진
                            break;

                        case "12":
                        case "17":
                            nJong = 1;      // 공무원
                            break;

                        case "81":
                            nJong = 6;      // 작업환경측정
                            break;

                        case "82":
                            nJong = 7;      // 보건관리대행
                            break;

                        case "83":
                            nJong = 9;      // 종합검진
                            break;

                        case "31":
                            nJong = 2;      // 암검진
                            break;

                        case "56":
                            nJong = 3;      // 학생검진
                            break;

                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                        case "38":
                        case "39":
                        case "40":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                        case "51":
                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "63":
                        case "64":
                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "69":
                        case "70":
                        case "71":
                        case "72":
                        case "73":
                        case "74":
                        case "75":
                        case "76":
                        case "77":
                        case "78":
                        case "79":
                        case "80":
                            nJong = 4;      // 기타
                            break;
                        default:
                            nJong = 1;      // 사업장
                            break;
                    }
                }

                // 종류별 계
                FnAmt[nJong, 8] = FnAmt[nJong, 8] + misuCashSum[i].SLIPAMT;

                // 소계
                if(nJong < 5)
                {
                    FnAmt[5, 8] = FnAmt[5, 8] + misuCashSum[i].SLIPAMT;
                }
                else if (nJong < 8)
                {
                    FnAmt[8, 8] = FnAmt[8, 8] + misuCashSum[i].SLIPAMT;
                }
               
                // 전체 합계
                FnAmt[10, 8] = FnAmt[10, 8] + misuCashSum[i].SLIPAMT;
                
                // 미수현금입금, 카드입금
                if(misuCashSum[i].GEACODE == "21")
                {
                    nMisu_Ipgum1 += misuCashSum[i].SLIPAMT;
                }
                else if (misuCashSum[i].GEACODE == "55")
                {
                    nMisu_Ipgum2 += misuCashSum[i].SLIPAMT;
                }
            }


            // 금액을 Display 
            // 스프레드에 가지고 온 값을 뿌려 줌
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    SS1.ActiveSheet.Cells[i - 1, j].Text = VB.Format(FnAmt[i, j], "###,###,###,##0 ");
                }
            }

            //일반건진, 종합검진 카드수납액 표시
            for (int i = 12; i <= 12; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    SS1.ActiveSheet.Cells[i - 1, j].Text = VB.Format(0, "###,###,###,##0 ");
                }
            }

            // 종검예약대체 현금
            long nCard_Tot = 0;
            List<HIC_TONGDAILY> CheckUpCash = hicTongDailyService.GetCheckUpCash(dtpFDate.Text, dtpTDate.Text, "3");
            
            if (CheckUpCash.Count > 0)
            {
                for(int i = 0; i < CheckUpCash.Count; i++)
                {
                    nCard_Tot = CheckUpCash[i].SUNAPAMT;
                }
            }

            //카드 및 현금

            SS1.ActiveSheet.Cells[10, 7].Text = VB.Format((nCardAmt1 + nCardAmt2) + nYDaeche + nCard_Tot, "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[10, 8].Text = VB.Format(nMisu_Ipgum2, "###,###,###,##0 ");

            SS1.ActiveSheet.Cells[11, 7].Text = VB.Format( FnAmt[10, 7] - (nCardAmt1 + nCardAmt2) + nYDaeche + nCard_Tot, "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[11, 8].Text = VB.Format(nMisu_Ipgum1, "###,###,###,##0 ");

            // 종합검진 예약선수금, 선수금대체액 표시
            SS1.ActiveSheet.Cells[12, 1].Text = "▶종합건진 예약선수금: " + VB.Format(nYeyakAmt, "###,###,###,##0 ");
            SS1.ActiveSheet.Cells[12, 5].Text = "▶예약선수금대체액: " + VB.Format(nYDaeche * -1, "###,###,###,##0 ");
        }


        private void eFormLoad(object sender, EventArgs e)
        // Form Load 후 설정
        {
            // 하루전 날짜로 셋팅
            dtpFDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            dtpTDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();

            // 하단 메세지 초기화
            lblMsg.Text = "";
        }

        private void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text;
        }
    }
}
