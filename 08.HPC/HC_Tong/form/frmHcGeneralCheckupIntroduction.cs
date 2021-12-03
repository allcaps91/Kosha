using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Tong
/// File Name       : frmHcGeneralCheckupIntroduction.cs
/// Description     : 종합검진 직원소개 현황
/// Author          : 심명섭
/// Create Date     : 2021-06-03
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > Frm종검소개현황 (Frm종검소개현황.frm)" />
/// 
namespace HC_Tong
{
    public partial class frmHcGeneralCheckupIntroduction :BaseForm
    {
        // Spread
        clsSpread cSpd = null;

        ComFunc cf = null;
        HicComHpcService hicComHpcService = null;

        string FstrBuse1, FstrBuse2, FstrBuse3;

        public frmHcGeneralCheckupIntroduction()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cf = new ComFunc();
            hicComHpcService = new HicComHpcService();
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
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            long nYY, nMM;

            nYY = CbxDate.Text.Substring(0, 4).To<long>(0);
            nMM = CbxDate.Text.Substring(5, 2).To<long>(0);

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle =  nYY + "년" + nMM + " 월 종합검진 직원소개 현황";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = "";

            strHeader += cSpd.setSpdPrint_String("┌─┬────┬────┬────┬─────┬────┐", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│결│ 담  당 │ 팀  장 │ 부  장 │ 행정처장 │ 병원장 │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  ├────┼────┼────┼─────┼────┤", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│  │        │        │        │          │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("│재│        │        │        │          │        │", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            strHeader += cSpd.setSpdPrint_String("└─┴────┴────┴────┴─────┴────┘", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void DisPlay_Screen()
        {
            string strFDate, strTDate, strBuse, strCol, strNewData, strOldData = "";
            long nRow, nCol, nCol2, nCol3, nSubTot, nCnt = 0;

            int Row;
            int Col;
            string text;

            strFDate = VB.Left(CbxDate.Text, 7) + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);

            SS1_Clear();

            // 1) 직원소개과별
            List<COMHPC> Department_list = hicComHpcService.GetDepartment(strFDate , strTDate);
            nSubTot = 0;
            for(int i = 0; i < Department_list.Count; i++)
            {
                nCol = VB.Val(VB.PP(VB.PP(VB.PP(FstrBuse1, Department_list[i].BUSE.Trim(), 2), ";", 1), ",", 2)).To<long>(0);
                if(nCol > 2)
                {
                    SS1.ActiveSheet.Cells[2, nCol.To<int>(0) - 1].Text = ( VB.Val(SS1.ActiveSheet.Cells[2, nCol.To<int>(0) - 1].Text).To<long>(0) + Department_list[i].CNT ).To<string>("0");
                }
                nSubTot += Department_list[i].CNT;
            }
            // 총계
            SS1.ActiveSheet.Cells[2, 1].Text = nSubTot.To<string>("0");

            // 직원소개월별
            List<COMHPC> MonthData = hicComHpcService.GetMonthData(VB.Left(strFDate, 4) + "-01-01" , strTDate);
            nSubTot = 0;
            for(int i = 0; i < MonthData.Count; i++)
            {
                nCol = VB.Val(VB.PP(VB.PP(VB.PP(FstrBuse1, MonthData[i].BUSE.Trim(), 2), ";", 1), ",", 2)).To<long>(0);
                if (nCol > 2)
                {
                    strCol = VB.PP(VB.PP(FstrBuse2, ( nCol.To<string>("0") + "," ).Trim(), 2), ";", 1).Trim();
                    switch(VB.Left(strCol, 1))
                    {
                        case "A":
                            nCol2 = 3;
                            Row = VB.Val(VB.PP(strCol, "A", 2)).To<int>(0) - 1;
                            Col = (VB.Val(MonthData[i].SDATE).To<long>(0) + nCol2).To<int>(0) - 1;
                            text = (VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text) + MonthData[i].CNT).To<string>("0");
                            SS1.ActiveSheet.Cells[Row, Col].Text = text;
                            break;
                        case "B":
                            nCol2 = 18;
                            Row = VB.Val(VB.PP(strCol, "B", 2)).To<int>(0) - 1;
                            Col = (VB.Val(MonthData[i].SDATE).To<long>(0) + nCol2).To<int>(0) - 1;
                            text = (VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text) + MonthData[i].CNT).To<string>("0");
                            SS1.ActiveSheet.Cells[Row, Col].Text = text;
                            break;
                    }
                }
                nSubTot += MonthData[i].CNT;
            }
            // 계
            for (int i = 6; i < 22; i++)
            {
                Row = i;
                nSubTot = 0;
                for (int j = 2; j < 15; j++)
                {
                    Col = j;
                    nSubTot += SS1.ActiveSheet.Cells[Row, Col].Text.To<long>(0);
                }
                SS1.ActiveSheet.Cells[Row, 2].Text = nSubTot.To<string>("0");
            }
            for (int i = 6; i < 18; i++)
            {
                nSubTot = 0;
                for (int j = 17; j < 30; j++)
                {
                    nSubTot += SS1.ActiveSheet.Cells[i, j].Text.To<long>(0);
                }
                SS1.ActiveSheet.Cells[i, 17].Text = nSubTot.To<string>("0");
            }

            //월별전체계
            for (int i = 2; i < 15; i++)
            {
                nSubTot = 0;
                Col = i;
                for (int j = 6; j < 22; j++)
                {
                    Row = j;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text).To<long>(0);
                }
                Col = i + 15;
                for (int k = 6; k < 18; k++)
                {
                    Row = k;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row , Col].Text).To<long>(0);
                }
                SS1.ActiveSheet.Cells[21, Col].Text = nSubTot.To<string>("0");
            }

            // 3) 직원소개개별 A
            List<COMHPC> Individual_A = hicComHpcService.GetIndividual_A(strFDate , strTDate);
            nSubTot = 0;
            nRow = 0;
            nCol3 = 0;
            for(int i = 0; i < Individual_A.Count; i++)
            {
                strBuse = Individual_A[i].NAME.Trim();
                strNewData = strBuse;

                if(strBuse != strOldData)
                {
                    if(i != 0)
                    {
                        nRow += 1;
                    }
                    if(nRow >= 10)
                    {
                        nRow = 0;
                        nCol3 = 15;
                    }
                    Row = 26 + nRow.To<int>(0);
                    if(nCol3 == 15)
                    {
                        SS1.ActiveSheet.Cells[Row, 2 + nCol3.To<int>(0) - 2].Text = strBuse;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[Row, 2 + nCol3.To<int>(0) - 1].Text = strBuse;
                    }
                    SS1.ActiveSheet.Cells[Row, 2 + nCol3.To<int>(0) - 1].Text = strBuse;
                    SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text = SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text + VB.TR(Individual_A[i].KORNAME.Trim(), " ", "") + "(" + Individual_A[i].CNT.To<string>("0") + "),";
                }

                else
                {
                    Row = 26 + nRow.To<int>(0);
                    SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text = SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text + VB.TR(Individual_A[i].KORNAME.Trim(), " ", "") + "(" + Individual_A[i].CNT.To<string>("0") + "),";
                }
                strOldData = strBuse;
            }
            // 3) 직원소개개별 B
            List<COMHPC> Individual_B = hicComHpcService.GetIndividual_B(strFDate , strTDate);
            nSubTot = 0;
            nRow = 0;
            nCol3 = 0;
            for (int i = 0; i < Individual_B.Count; i++)
            {
                strBuse = Individual_B[i].NAME.Trim();
                strNewData = strBuse;

                if (strNewData != strOldData)
                {
                    if (i != 0)
                    {
                        nRow += 1;
                    }
                    if (nRow >= 12)
                    {
                        nRow = 0;
                        nCol3 = 15;
                    }
                    Row = 39 + nRow.To<int>(0);
                    if (nCol3 == 15)
                    {
                        SS1.ActiveSheet.Cells[Row, 2 + nCol3.To<int>(0) - 2].Text = strBuse;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[Row, 2 + nCol3.To<int>(0) - 1].Text = strBuse;
                    }
                    SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text = SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text + VB.TR(Individual_B[i].KORNAME.Trim(), " ", "") + "(" + Individual_B[i].CNT.To<string>("0") + "),";
                }
                else
                {
                    Row = 39 + nRow.To<int>(0);
                    SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text = SS1.ActiveSheet.Cells[Row, 4 + nCol3.To<int>(0) - 1].Text + VB.TR(Individual_B[i].KORNAME.Trim(), " ", "") + "(" + Individual_B[i].CNT.To<string>("0") + "),";
                }
                strOldData = strBuse;
            }

            // 계

            for (int i = 3; i <= 3; i++)
            {
                nSubTot = 0;
                for (int j = 27; j <= 36; j++)
                {
                    Row = j - 1;
                    Col = 4 - 1;
                    if (SS1.ActiveSheet.Cells[Row, Col].Text.Trim() != "")
                    {
                        nCnt = 0;
                        long kCount = VB.L( SS1.ActiveSheet.Cells[Row, Col].Text, "(" ) - 1;
                        for (int k = 1; k <= kCount; k++)
                        {
                            nCnt += VB.Val(VB.PP(VB.PP(SS1.ActiveSheet.Cells[Row, Col].Text, "(", k + 1), ")", 1)).To<long>(0);
                        }
                        Col = i - 1;
                        SS1.ActiveSheet.Cells[Row, Col].Text = nCnt.To<string>("0");
                    }
                    Col = i - 1;
                    Row = j - 1;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text).To<long>(0);
                }
                for (int j = 27; j <= 36; j++)
                {
                    Row = j - 1;
                    Col = 19 - 1;
                    if (SS1.ActiveSheet.Cells[Row, Col].Text.Trim() != "")
                    {
                        nCnt = 0;
                        long kCount = VB.L( SS1.ActiveSheet.Cells[Row, Col].Text, "(" ) - 1;
                        for (int k = 1; k <= kCount; k++)
                        {
                            nCnt += VB.Val(VB.PP(VB.PP(SS1.ActiveSheet.Cells[Row, Col].Text, "(", k + 1), ")", 1)).To<long>(0);
                        }
                        Col = i + 14;
                        SS1.ActiveSheet.Cells[Row, Col].Text = nCnt.To<string>("0");
                    }
                    Col = i + 14;
                    Row = j - 1;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text).To<long>(0);
                }
            }
            SS1.ActiveSheet.Cells[24, 3].Text = nSubTot.To<string>("0") + "명";
            for (int i = 3; i <= 3; i++)
            {
                nSubTot = 0;
                for (int j = 40; j <= 51; j++)
                {
                    Row = j - 1;
                    Col = 4 - 1;
                    if (SS1.ActiveSheet.Cells[Row, Col].Text.Trim() != "")
                    {
                        nCnt = 0;
                        long kCount = VB.L( SS1.ActiveSheet.Cells[Row, Col].Text, "(" ) - 1;
                        for (int k = 1; k <= kCount; k++)
                        {
                            nCnt += VB.Val(VB.PP(VB.PP(SS1.ActiveSheet.Cells[Row, Col].Text, "(", k + 1), ")", 1)).To<long>(0);
                        }
                        Col = i - 1;
                        SS1.ActiveSheet.Cells[Row, Col].Text = nCnt.To<string>("0");
                    }
                    Col = i - 1;
                    Row = j - 1;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text).To<long>(0);
                }
                for (int j = 40; j <= 51; j++)
                {
                    Row = j - 1;
                    Col = 19 - 1;
                    if (SS1.ActiveSheet.Cells[Row, Col].Text.Trim() != "")
                    {
                        nCnt = 0;
                        long kCount = VB.L( SS1.ActiveSheet.Cells[Row, Col].Text, "(" ) - 1;
                        for (int k = 1; k <= kCount; k++)
                        {
                            nCnt += VB.Val(VB.PP(VB.PP(SS1.ActiveSheet.Cells[Row, Col].Text, "(", k + 1), ")", 1)).To<long>(0);
                        }
                        Col = i + 14;
                        SS1.ActiveSheet.Cells[Row, Col].Text = nCnt.To<string>("0");
                    }
                    Col = i + 14;
                    Row = j - 1;
                    nSubTot += VB.Val(SS1.ActiveSheet.Cells[Row, Col].Text).To<long>(0);
                }
            }
            SS1.ActiveSheet.Cells[37, 3].Text = nSubTot.To<string>("0") + "명";

        }

        private void SS1_Clear()
        {
            // 1) 직원소개 과별 현황
            for(int i = 1; i < 30; i++)
            {
                SS1.ActiveSheet.Cells[2, i].Text = "";
            }

            // 2)직원소개 월별 현황
            for(int i = 6; i < 22; i++)
            {
                for(int j = 2; j < 15; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            for (int i = 6; i < 22; i++)
            {
                for (int j = 17; j < 30; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            // 3) 직원소개 개별
            // 가족
            SS1.ActiveSheet.Cells[24, 2].Text = "";
            for (int i = 26; i < 36; i++)
            {
                for(int j = 1; j < 30; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }
            // 직원
            SS1.ActiveSheet.Cells[37, 2].Text = "";
            for (int i = 39; i < 51; i++)
            {
                for (int j = 1; j < 30; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            long nYY, nMM, nYYMM = 0;

            FstrBuse1 = "";
            FstrBuse1 += "000101,3;070100,3;070101,3;077100,3;077101,3;077202,3;100040,3;";                     // 기획행정, 교환, 행정처장실
            FstrBuse1 += "077200,4;077201,4;";                                                                  // 총무
            FstrBuse1 += "077300,5;077301,5;";                                                                  // 경리
            FstrBuse1 += "077500,6;077501,6;";                                                                  // 의료증진
            FstrBuse1 += "044500,7;044501,7;044600,7;044601,7;044510,7;044520,7;011150,7;100790,7;100800,7;";   // 건강증진
            FstrBuse1 += "101772,7;101773,7;101774,7;";
            FstrBuse1 += "077400,8;077401,8;077402,8;077403,8;077404,8;";                                       // 원무과
            FstrBuse1 += "066100,9;066101,9;066102,9;066103,9;066104,9;066105,9;066106,9;066107,9;066108,9;";   // 관리과
            FstrBuse1 += "066109,9;066110,9;066200,9;066201,9;076010,9;";
            FstrBuse1 += "088100,10;088101,10;088102,10;088200,10;088201,10;";                                  // 원목실
            FstrBuse1 += "078001,11;078100,11;078101,11;077409,11;076001,11;";                                  // 감염관리, QI, 적정관리, 진료의뢰
            FstrBuse1 += "044100,12;044101,12;044102,12;044103,12;044104,12;044105,12;";                        // 약제과
            FstrBuse1 += "044300,13;044301,13;099204,13;";                                                      // 영양실
            FstrBuse1 += "077405,14;077502,14;078200,14;078201,14;";                                            // 심사
            FstrBuse1 += "055100,15;055101,15;055102,15;100570,15;";                                            // 방사
            FstrBuse1 += "077900,16;077901,16;";                                                                // 사회

            FstrBuse1 += "033100,17;033101,17;033102,17;033103,17;033104,17;033105,17;033106,17;033108,17;";    //간호부
            FstrBuse1 += "033109,17;033111,17;033112,17;033113,17;033114,17;033115,17;033116,17;033117,17;";
            FstrBuse1 += "033118,17;033119,17;033120,17;033121,17;033122,17;033123,17;033125,17;033126,17;";
            FstrBuse1 += "033127,17;033130,17;033140,17;";
            // 신규병동 추가 2013-11-02
            FstrBuse1 += "101752,17;101753,17;101753,17;101754,17;101755,17;101756,17;101743,17;101757,17;";
            FstrBuse1 += "101744,17;101745,17;101746,17;101747,17;101748,17;101749,17;101750,17;101751,17;";
            FstrBuse1 += "101776,17;";

            FstrBuse1 += "033110,18;100410,18;";                                                                  // 외래
            FstrBuse1 += "055301,19;055307,19;";                                                                  // 물리
            FstrBuse1 += "055200,20;055201,20;055202,20;";                                                        // 검사실


            FstrBuse1 += "011100,21;011101,21;011102,21;011103,21;011104,21;011105,21;011106,21;011107,21;";      // 전문의
            FstrBuse1 += "011108,21;011109,21;011110,21;011111,21;011112,21;011113,21;011114,21;011115,21;";
            FstrBuse1 += "11117,21;011118,21;011119,21;011120,21;011121,21;011122,21;011123,21;011124,21;";
            FstrBuse1 += "011125,21;011126,21;011127,21;011128,21;011129,21;011128,21;";                          // 전문의(건강증진센터 포함 -011128,7)
            FstrBuse1 += "022100,21;022101,21;022102,21;022103,21;022104,21;022105,21;022118,21;022119,21;";
            FstrBuse1 += "022124,21;022150,21;022160,21;";                                                        // 전공의
            FstrBuse1 += "011116,21;100420,21;101070,21;100251,21;";                                              // 재활의학 의국포함


            FstrBuse1 += "044200,22;044201,22;";                                                                  // 기록실
            FstrBuse1 += "077600,23;077601,23;";                                                                  // 도서실
            FstrBuse1 += "044410,24;044411,24;056301,24;";                                                        // 심리실
            FstrBuse1 += "088200,25;088201,25;099202,25;";                                                        // 장례
            FstrBuse1 += "033107,26;";                                                                            // 공급실
            FstrBuse1 += "099301,27;990301,27;";                                                                  // 매점
            FstrBuse1 += "099100,28;099101,28;";                                                                  // 노조
            FstrBuse1 += "099201,29;";                                                                            // 미화
            FstrBuse1 += "099200,30;099203,30;101500,30;099303,30;101730,30;078300,30;";                          // 기타(용역,경비,기타,고객지원과)


            FstrBuse2 = "3,A7;4,A8;5,A9;6,A10;7,A11;8,A12;9,A13;10,A14;11,A15;12,A16;13,A17;14,A18;15,A19;16,A20;17,A21;18,A22;";
            FstrBuse2 += "19,B7;20,B8;21,B9;22,B10;23,B11;24,B12;25,B13;26,B14;27,B15;28,B16;29,B17;30,B18;";


            FstrBuse3 = "3,기획행정;4,총무과;5,경리과;6,의료정보;7,건강증진센터;8,원무과;9,관리과;10,원목실;11,적정;12,약제과;";
            FstrBuse3 += "13,영양실;14,심사과;15,방사선과;16,사회사업;17,간호부;18,간호부(외래);19,재활의학;20,진단검사의학;";
            FstrBuse3 += "21,의국;22,의무기록실;23,의학도서실;24,임상심리;25,요셉관;26,중앙공급실;27,매점;28,미화부;29,기타(용역);30,기타(지산사);";


            string now = DateTime.Now.ToString("yyyy-MM");

            nYY = now.Substring(0, 4).To<long>(0);
            nMM = now.Substring(5, 2).To<long>(0);
            nYYMM = (nYY.To<string>("") + nMM.To<string>("")).To<long>(0);

            CbxDate.Clear();

            for (int i = 1; i <= 24; i++)
            {
                CbxDate.Items.Add(nYY + "-" + VB.Format(nMM, "00"));
                nMM -= 1;
                if (nMM == 0)
                {
                    nYY -= 1;
                    nMM = 12;
                }
            }
            CbxDate.SelectedIndex = 1;
        }
    }
}
