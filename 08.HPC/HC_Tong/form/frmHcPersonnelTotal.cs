using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcPersonnelTotal.cs
/// Description     : 전년도 대비 인원 통계
/// Author          : 심명섭
/// Create Date     : 2021-05-31
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > Frm인원누계 (Frm인원누계.frm)" />

namespace HC_Tong
{
    public partial class frmHcPersonnelTotal :BaseForm
    {
        // Spread
        clsSpread cSpd = null;
        ComFunc cf = null;
        // 약한참조
        HicComHpcService hicComHpcService = null;
        HicTongamService hicTongamService = null;

        public frmHcPersonnelTotal()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        

        private void SetControl()
        {
            // Service 선언 
            // Service -> Repository
            cSpd = new clsSpread();
            cf = new ComFunc();
            hicComHpcService = new HicComHpcService();
            hicTongamService = new HicTongamService();
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
            long nYY, nMM, nYYMM = 0;
            string now = DateTime.Now.ToString("yyyy-MM");
            
            nYY = now.Substring(0, 4).To<long>(0);
            nMM = now.Substring(5, 2).To<long>(0);
            nYYMM = ( nYY.To<string>("") + nMM.To<string>("") ).To<long>(0);

            CbxDate.Clear();

            for (int i = 1; i <= 24; i++)
            {
                CbxDate.Items.Add(nYY + "-" + VB.Format(nMM, "00"));
                nMM -= 1;
                if(nMM == 0)
                {
                    nYY -= 1;
                    nMM = 12;
                }
            }
            CbxDate.SelectedIndex = 1;
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

        private void DisPlay_Screen()
        {
            long nYear          = 0;
            long nMM            = 0;
            long nJong          = 0;
            long nJobMM         = 0;
            long nChasu         = 0;
            long nJepCNT        = 0;  // 접수인원
            string strGbinwon   = "";
            string strYYMM      = "";
            string strSdate     = "";
            string strEdate     = "";
            string strJenYear   = "";
            long[,,] nTotCnt = new long[15, 4, 44];
            
            for(int i = 2; i <= 44; i++)
            {
                for(int j = 2; j <= 42; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            // 누적할 배열을 Clear
            for (int i = 0; i <= 14; i++)
            {
                for (int j = 0; j <= 43; j++)
                {
                    nTotCnt[i, 1, j] = 0;
                    nTotCnt[i, 2, j] = 0;
                    nTotCnt[i, 3, j] = 0;
                }
            }

            strSdate = VB.Format( VB.Val( VB.Left(CbxDate.Text, 4) ) - 1, "0000" ) + "-01-01";
            strEdate =  cf.READ_LASTDAY(clsDB.DbCon, CbxDate.Text + "-01");
            strJenYear = VB.Format( VB.Val( VB.Left(CbxDate.Text, 4)) - 1, "0000"); // 전년도
            //nJobMM = CbxDate.Text.Substring(CbxDate.Text.Length - 2, 2).To<long>(0);
            nJobMM = VB.Val(VB.Right(CbxDate.Text, 2)).To<long>(0);

            // 전년도/금년도 통계
            List<COMHPC> statistics = hicComHpcService.GetStatistics(strSdate, strEdate); 

            for (int i = 0; i < statistics.Count; i++)
            {
                nJepCNT = statistics[i].JepCnt;
                nChasu = statistics[i].CHASU.To<long>(0);
                strGbinwon = statistics[i].GJJONG.Trim(); // 널레퍼런스
                strYYMM = statistics[i].YYMM.Trim();
                nYear = 2; // 금년
                if(VB.Left(strYYMM, 4) == strJenYear)
                {
                    nYear = 1; // 전년
                }

                nMM = VB.Val(VB.Right(strYYMM, 2)).To<long>(0) + 2;

                switch (strGbinwon)
                {
                    // 성인병 (지역, 직장, 공교)
                    case "11":
                    case "61":
                        nJong = 2;
                        break;
                    case "12":
                    case "62":
                        nJong = 3;
                        break;
                    case "13":
                    case "63":
                        nJong = 4;
                        break;
                    // 공무원(일반, 교직, 군인)
                    case "21":
                    case "64":
                        nJong = 6;
                        break;
                    case "22":
                    case "65":
                        nJong = 7;
                        break;
                    case "23":
                    case "66":
                        nJong = 8;
                        break;
                    // 사업장(일반,일특,특수,배치전,일반채,일채배,방사선)
                    case "31":
                    case "67":
                        nJong = 10;
                        break;
                    case "32":
                    case "68":
                        nJong = 11;
                        break;
                    case "33":
                        nJong = 12;
                        break;
                    case "34":
                        nJong = 13;
                        break;
                    case "35":
                        nJong = 14;
                        break;
                    case "36":
                        nJong = 15;
                        break;
                    case "37":
                        nJong = 16;
                        break;
                    // 암검진계
                    case "49":
                    case "69":
                        nJong = 24;
                        break;
                    // 기타검진(위생, 운전, 학생, 기타)
                    case "53":
                        nJong = 25;
                        break;
                    case "54":
                        nJong = 26;
                        break;
                    case "55":
                        nJong = 27;
                        break;
                    case "56":
                        nJong = 28;
                        break;
                    case "51":
                    case "52":
                        nJong = 28;
                        break;
                    // 측정(일반, 국고)
                    case "71":
                        nJong = 30;
                        break;
                    case "72":
                        nJong = 31;
                        break;
                    // 대행(일반, 국고)
                    case "81":
                        nJong = 33;
                        break;
                    case "82":
                        nJong = 34;
                        break;
                    // 종검(개인, 단체)
                    case "91":
                        nJong = 36;
                        break;
                    case "92":
                        nJong = 37;
                        break;
                }

                // 전년도
                if(nYear == 1)
                {
                    if(nChasu == 1 || nChasu == 3)
                    {
                        nTotCnt[1, 1, nJong] += nJepCNT;
                        nTotCnt[1, 1, 1] += nJepCNT;
                        nTotCnt[nMM, 1, nJong] += nJepCNT;
                        nTotCnt[nMM, 1, 1] += nJepCNT;
                    }
                    else
                    {
                        nTotCnt[1, 2, 1] += nJepCNT;
                        nTotCnt[1, 2, nJong] += nJepCNT;
                    }
                }
                // 금년도
                else
                {
                    if(nChasu ==1 || nChasu == 3)
                    {
                        nTotCnt[nMM, 2, 1] += nJepCNT;
                        nTotCnt[nMM, 2, nJong] += nJepCNT;
                    }
                    else
                    {
                        nTotCnt[nMM, 3, 1] += nJepCNT;
                        nTotCnt[nMM, 3, nJong] += nJepCNT;
                    }
                }
            }

            // 소계, 총계를 구함
            for(int i = 2; i <= 40; i++)
            {
                int k = 0;
                switch (i)
                {
                    case 2:
                    case 3:
                    case 4:
                        k = 5;
                        break;
                    case 6:
                    case 7:
                    case 8:
                        k = 9;
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        k = 17;
                        break;
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                        k = 29;
                        break;
                    case 30:
                    case 31:
                        k = 32;
                        break;
                    case 33:
                    case 34:
                        k = 35;
                        break;
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                        k = 43;
                        break;
                }

                if(k > 0)
                {
                    for(int j = 3; j <= 14; j++)
                    {
                        // 총계에 누적
                        nTotCnt[1, 1, k] += nTotCnt[j, 1, i];
                        nTotCnt[1, 2, k] += nTotCnt[j, 2, i];
                        nTotCnt[1, 3, k] += nTotCnt[j, 3, i];
                        // 소계에 누적
                        nTotCnt[j, 1, k] += nTotCnt[j, 1, i];
                        nTotCnt[j, 2, k] += nTotCnt[j, 2, i];
                        nTotCnt[j, 3, k] += nTotCnt[j, 3, i];
                    }
                }
            }

            // 암검진 건수
            List<HIC_TONGAM> cancerCount = hicTongamService.GetCancerCount(strSdate, strEdate);

            
            for(int i = 0; i < cancerCount.Count; i++)
            {
                strYYMM = cancerCount[i].YYMM.Trim();
                nYear = 2; // 금년
                //if(strYYMM.Substring(strYYMM, 4) + 2 == strJenYear)
                if(VB.Left(strYYMM, 4) == strJenYear)
                {
                    nYear = 1; // 전년
                }
                nMM = VB.Val(VB.Right(strYYMM, 2)).To<long>(0) + 2;

                if (nYear == 1)
                {
                    nTotCnt[1, 1, 18] += cancerCount[i].CNT1; // 위1
                    nTotCnt[1, 1, 18] += cancerCount[i].CNT2; // 위2
                    nTotCnt[1, 1, 19] += cancerCount[i].CNT4; // 간
                    nTotCnt[1, 1, 20] += cancerCount[i].CNT3; // 결직장
                    nTotCnt[1, 1, 21] += cancerCount[i].CNT5; // 유방
                    nTotCnt[1, 1, 22] += cancerCount[i].CNT6; // 자궁경부
                    nTotCnt[1, 1, 23] += cancerCount[i].CNT7; // 폐

                    nTotCnt[nMM, 1, 18] += cancerCount[i].CNT1; // 위1
                    nTotCnt[nMM, 1, 18] += cancerCount[i].CNT2; // 위2
                    nTotCnt[nMM, 1, 19] += cancerCount[i].CNT4; // 간
                    nTotCnt[nMM, 1, 20] += cancerCount[i].CNT3; // 결직장
                    nTotCnt[nMM, 1, 21] += cancerCount[i].CNT5; // 유방
                    nTotCnt[nMM, 1, 22] += cancerCount[i].CNT6; // 자궁경부
                    nTotCnt[nMM, 1, 23] += cancerCount[i].CNT7; // 폐
                }
                else
                {
                    nTotCnt[nMM, 2, 18] += cancerCount[i].CNT1; // 위1
                    nTotCnt[nMM, 2, 18] += cancerCount[i].CNT2; // 위2
                    nTotCnt[nMM, 2, 19] += cancerCount[i].CNT4; // 간
                    nTotCnt[nMM, 2, 20] += cancerCount[i].CNT3; // 결직장
                    nTotCnt[nMM, 2, 21] += cancerCount[i].CNT5; // 유방
                    nTotCnt[nMM, 2, 22] += cancerCount[i].CNT6; // 자궁경부
                    nTotCnt[nMM, 2, 23] += cancerCount[i].CNT7; // 폐
                }
            }

            // 전년도 작업월까지 1차, 2차 누계를 구함
            for(int i = 1; i <= nJobMM; i++)
            {
                for(int j = 1; j <= 43; j++)
                {
                    // 전년도 1차 해당월까지 누적
                    nTotCnt[2, 1, j] += nTotCnt[i + 2, 1, j];
                    // 금년도 1차 해당월까지 누적
                    nTotCnt[2, 2, j] += nTotCnt[i + 2, 2, j];
                    // 금년도 2차 해당월까지 누적
                    nTotCnt[2, 3, j] += nTotCnt[i + 2, 3, j];
                }
            }

            // (1)전년총계
            for(int i = 1; i <= 43; i++)
            {
                SS1.ActiveSheet.Cells[i + 1, 2].Text = VB.Format(nTotCnt[1, 1, i], "#,##0 ");
                SS1.ActiveSheet.Cells[i + 1, 3].Text = VB.Format(nTotCnt[1, 2, i], "#,##0 ");
            }

            //누계, 1~12월

            for(int i = 1; i <= 13; i++)
            {
                for(int j = 1; j <= 43; j++)
                {
                    SS1.ActiveSheet.Cells[j + 1, (i * 3) + 1].Text = VB.Format(nTotCnt[i + 1, 1, j], "#,### ");
                    SS1.ActiveSheet.Cells[j + 1, (i * 3) + 2].Text = VB.Format(nTotCnt[i + 1, 2, j], "#,### ");
                    SS1.ActiveSheet.Cells[j + 1, (i * 3) + 3].Text = VB.Format(nTotCnt[i + 1, 3, j], "#,### ");

                }
            }

        }

        private void Spread_Print()
        {
            long nYY = VB.Left(CbxDate.Text.Trim(), 4).To<long>(0);
            long nMM = VB.Right(CbxDate.Text.Trim(), 2).To<long>(0);
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = nYY - 1 + "년도 대비 - " + nYY + "년" + nMM + "(月) 총계표";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("총계 인원에 종합검진 및 측정, 대행은 포함되지 않음." + " 인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);


            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 50, 50);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, true, true, true, true, false, true, 0.7f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}