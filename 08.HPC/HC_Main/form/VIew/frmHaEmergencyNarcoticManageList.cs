using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaEmergencyNarcoticManageList.cs
/// Description     : 종합검진 비상 마약류 관리 리스트
/// Author          : 이상훈
/// Create Date     : 2019-09-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종합검진비상마약류관리리스트.frm(Frm종합검진비상마약류관리리스트)" />

namespace HC_Main
{
    public partial class frmHaEmergencyNarcoticManageList : Form
    {
        int fnCount = 0;

        HeaResvExamService heaResvExamService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HicCancerResv2PatientService hicCancerResv2PatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicHyangApproveService hicHyangApproveService = null;
        ComHpcLibBService comHpcLibBService = null;

        frmNarcoticPrint frmNarcoticPrint = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        ComFunc cf = new ComFunc();

        public frmHaEmergencyNarcoticManageList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            heaResvExamService = new HeaResvExamService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            hicCancerResv2PatientService = new HicCancerResv2PatientService();
            heaJepsuService = new HeaJepsuService();
            hicHyangApproveService = new HicHyangApproveService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Text = clsPublic.GstrSysDate;

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 5;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            long nWRTNO = 0;
            string strPano = "";
            string strSname = "";
            string strIO = "";
            string strDeptCode = "";
            string strDRSABUN = "";
            string strSEX = "";
            long nAge = 0;
            string strSabun = "";
            string strBDATE = "";
            string strSDate = "";
            string strNRSABUN = "";
            string strJuso = "";
            string strPtNo = "";
            string strDept = "";
            string strGBPrint = "";
            string strJumin = "";
            string strSuCode = "";
            string strChk = "";
            string strGbSite = "";
            string strQty = "";
            string strPRT = "";
            double nEntQty1 = 0;
            double nEntQty2 = 0;
            string strPTNO1 = "";

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nPano = 0;
                bool bOK = false;

                string sFrDate = "";
                string sToDate = "";
                string sGbExam = "";

                sFrDate = dtpDate.Text;
                sToDate = DateTime.Parse(dtpDate.Text).AddDays(1).ToShortDateString();
                sGbExam = "01";

                ComFunc.ReadSysDate(clsDB.DbCon);

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 0;

                //종검 내시경 예약자
                List<HEA_RESV_EXAM> list = heaResvExamService.GetItembyRTime(sFrDate, sToDate, sGbExam);

                nREAD = list.Count;

                for (int i = 0; i < nREAD; i++)
                {
                    nPano = list[i].PANO;
                    //오늘 대장내시경과 동시에 예약인지 점검
                    sGbExam = "02";
                    if (heaResvExamService.GetCountbyPano(sFrDate, sToDate, sGbExam, nPano) == 0)
                    {
                        HEA_JEPSU_PATIENT list2 = heaJepsuPatientService.GetItembyPano(nPano);
                        if (!list2.IsNullOrEmpty())
                        {
                            if (list2.SNAME.Trim() != "")
                            {
                                //if (nRow <= 40)
                                //{
                                    nRow += 1;
                                    if (SS1.ActiveSheet.RowCount < nRow)
                                    {
                                        SS1.ActiveSheet.RowCount = nRow;
                                    }
                                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = (i + 1).ToString();
                                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "향정";
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "A-ANE12G";
                                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = dtpDate.Text;
                                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "TO";
                                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "외래";
                                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list2.SNAME.Trim();
                                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list2.PTNO.Trim();
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "1";
                                //}
                            }
                        }
                    }
                }

                //건진 2층종검내시경 예약자
                List<HIC_CANCER_RESV2_PATIENT> list3 = hicCancerResv2PatientService.GetItembyRTime(dtpDate.Text);

                nREAD = list3.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strSname = list3[i].SNAME.Trim();
                    strPtNo = list3[i].PANO.Trim();

                    //종검,일반건진 동시 내시경 수검자는 제외
                    bOK = true;
                    for (int j = 0; j < nRow; j++)
                    {
                        if (SS1.ActiveSheet.Cells[j, 6].Text == strSname)
                        {
                            if (SS1.ActiveSheet.Cells[j, 7].Text == strPtNo)
                            {
                                bOK = false;
                                break;
                            }
                        }
                    }
                    //if (nRow == 41)
                    //{
                    //    bOK = false;
                    //}

                    if (bOK == true)
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = nRow.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "향정";
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "A-ANE12G";
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = dtpDate.Text;
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "HR";
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "외래";
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = strSname;
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strPtNo;
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "1";
                    }
                }

                //국가암검진 대상자
                List<HIC_CANCER_RESV2_PATIENT> list4 = hicCancerResv2PatientService.GetItembySDate(dtpDate.Text);

                nREAD = list4.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strSname = list4[i].SNAME.Trim();
                    strPtNo = list4[i].PTNO.Trim();
                    nPano = list4[i].PANO.To<long>(0);

                    //종검,일반건진 동시 내시경 수검자는 제외
                    bOK = true;

                    string strRowid = heaResvExamService.GetRowidByPanoRTimeGbExam(nPano, sFrDate, sToDate, "02");
                    if (!strRowid.IsNullOrEmpty()) { bOK = false; }

                    for (int j = 0; j < nRow; j++)
                    {
                        if (SS1.ActiveSheet.Cells[j, 6].Text == strSname)
                        {
                            if (SS1.ActiveSheet.Cells[j, 7].Text == strPtNo)
                            {
                                bOK = false;
                                break;
                            }
                        }
                    }

                    if (bOK == true)
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = nRow.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "향정";
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "A-ANE12G";
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = dtpDate.Text;
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "HR";
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "외래";
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = strSname;
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strPtNo;
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "1";
                    }
                }


                SS1.ActiveSheet.RowCount = nRow + 1;
                SS1.ActiveSheet.Cells[nRow, 1].Text = "합계";
                SS1.ActiveSheet.Cells[nRow, 8].Text = string.Format("{0:##0}", nRow);
                fnCount = 0;
                fnCount = nRow;
            }
            else if (sender == btnPrint)
            {
                string strFont1 = "";
                string strHead1 = "";
                string strFont2 = "";
                string strHead2 = "";

                strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
                strFont2 = "/fn\"맑은 고딕\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

                //strHead1 = "/n/r" + "┏━━━━━━━━━┳━━━━━━━━━┓  " + VB.Space(1) + "/n";
                //strHead1 += "/l" + VB.Space(2) + "종합검진 비상 마약류 관리 리스트";
                //strHead1 += "/r" + "┃  신    청    인  ┃  수  령   확  인 ┃  " + VB.Space(1) + "/n";
                //strHead1 += "/l" + VB.Space(2) + "================================";
                //strHead1 += "/r" + "┣━━━━┳━━━━╋━━━━┳━━━━┫  " + VB.Space(1) + "/n";
                //strHead1 += "/r" + "┃종합검진┃종합검진┃종합검진┃약 제 팀┃  " + VB.Space(1) + "/n";
                //strHead1 += "/l" + VB.Space(2) + dtpDate.Value.ToString("yyyy MM월 dd일") + " 시간: ";
                //strHead1 += "/r" + "┃전 문 의┃간호팀장┃        ┃        ┃  " + VB.Space(1) + "/n";
                //strHead1 += "/r" + "┣━━━━╋━━━━╋━━━━╋━━━━┫  " + VB.Space(1) + "/n";
                //strHead1 += "/l" + VB.Space(2) + "수령갯수 : A-ANE12G         개";
                //strHead1 += "/r" + "┃        ┃        ┃        ┃        ┃  " + VB.Space(1) + "/n";
                //strHead1 += "/r" + "┃        ┃        ┃        ┃        ┃  " + VB.Space(1) + "/n";
                //strHead1 += "/r" + "┗━━━━┻━━━━┻━━━━┻━━━━┛  " + VB.Space(1);

                strHead1 = "종합검진 비상 마약류 관리 리스트" + "/n";
                strHead1 += VB.Space(2) + "================================"+ "/n"; ;
                strHead1 += VB.Space(2) + dtpDate.Value.ToString("yyyy년 MM월 dd일") + " 시간: "+ "/n"; ;
                strHead1 += VB.Space(2) + "수령갯수 : A-ANE12G" +" " + fnCount +"개"+ "/n";
                strHead2 += VB.Space(2) +"/n" + "/n" + "/n"+ "/n"+ "/n";

                SS1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
                SS1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                SS1_Sheet1.PrintInfo.Header = strFont2 + strHead1+ strHead2;
                SS1_Sheet1.PrintInfo.Margin.Top = 100;
                SS1_Sheet1.PrintInfo.Margin.Left = 20;
                SS1_Sheet1.PrintInfo.Margin.Bottom = 10;
                SS1_Sheet1.PrintInfo.Margin.Header = 10;
                SS1_Sheet1.PrintInfo.ShowColor = false;
                SS1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                SS1_Sheet1.PrintInfo.ShowBorder = true;
                SS1_Sheet1.PrintInfo.ShowGrid = true;
                SS1_Sheet1.PrintInfo.ShowShadows = false;
                SS1_Sheet1.PrintInfo.UseMax = true;
                SS1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                SS1_Sheet1.PrintInfo.UseSmartPrint = false;
                SS1_Sheet1.PrintInfo.ShowPrintDialog = false;
                SS1_Sheet1.PrintInfo.Preview = false;
                SS1.PrintSheet(0);

                ///TODO : 이상훈(2019.09.11) 출력 Log 여부 확인
                //SQL_LOG("", SS1.PrintHeader);
            }
            else if (sender == btnPrint2)
            {   
                //종합건진 간호사만 인쇄 가능함

                strBDATE = dtpDate.Text;    //검진일
                strSDate = dtpDate.Text;    //검진일

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strPTNO1 = SS1.ActiveSheet.Cells[i, 7].Text.Trim();

                    HEA_JEPSU list = heaJepsuService.GetItembyPtNoBdate(strPTNO1, strBDATE);

                    strSname = list.SNAME.Trim();
                    strSEX = list.SEX.Trim();
                    nAge = list.AGE;
                    nWRTNO = list.WRTNO;
                    strPtNo = list.PTNO.Trim();

                    strSuCode = "A-POL12";
                    nEntQty1 = 20;

                    ///TODO : 이상훈 (2019-09-23) strDept 에 변수 Set 하는 곳이 없음.
                    strDept = SS1.ActiveSheet.Cells[i, 4].Text.Trim();

                    //승인일자를 읽음
                    List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetItembyWrtNoBDate(nWRTNO, strBDATE, strDept, strSuCode);

                    strSDate = list2[0].APPROVETIME.ToString();
                    strJuso = list2[0].JUSO.Trim();
                    strJumin = list2[0].JUMIN.Trim();

                    strSDate = dtpDate.Text;
                    strNRSABUN = string.Format("{0:#00000}", clsType.User.IdNumber);

                    //주소를 읽음
                    if (strJuso == "")
                    {
                        COMHPC lstJuso = comHpcLibBService.GetJusobyWrtnoPtnoSname(nWRTNO, strPtNo, strSname, strDept);
                        strJuso = lstJuso.JUSO1 + lstJuso.JUSO2;
                    }

                    //수량
                    strQty = "(=" + string.Format("{0:#0.0}", nEntQty2) + "ml)";
                    if (VB.InStr(strQty, ".0ml") > 0)
                    {
                        strQty = strQty.Replace(".0ml", "ml");
                        if (nEntQty1 != 0 && nEntQty2 != 0)
                        {
                            strQty += "," + string.Format("0:0.00", nEntQty2 / nEntQty1) + "A";
                        }
                    }

                    //------------------------------------
                    //  인쇄폼에 넘겨줄 변수를 설정함
                    //------------------------------------
                    //(1) 처방전명
                    if (VB.Left(strSuCode, 2) == "N-")
                    {
                        strPRT = "마  약{$}";
                    }
                    else
                    {
                        strPRT = "향정주사{$}";
                    }
                    strPRT += strDept + "{$}";  //(2) 병동/외래
                    strPRT += strSname + "{$}"; //(3) 성명
                    strPRT += strPano + "{$}";  //(4) 등록번호

                    if (strDept == "TO")
                    {
                        strPRT += "종합건진{$}"; //(5) 진료과
                    }
                    else
                    {
                        strPRT += "일반건진{$}"; //(5) 진료과
                    }

                    strPRT += "{$}"; //(6) 오더번호
                    strPRT += strSEX + "/" + string.Format("{0:#0}", nAge) + "{$}"; //(7) 성별/나이
                    strPRT += VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******" + "{$}"; //(8) 주민등록번호
                    strPRT += strJuso + "{$}"; //(9) 주소
                    strPRT += "검사용{$}"; //(10) 상병명
                    strPRT += "Pain{$}"; //(11) 주요증상
                    strPRT += strSDate + "{$}"; //(12) 처방일,검사일
                    strPRT += strBDATE + "{$}"; //(13) 처방일,검사일
                    strPRT += strSuCode + "{$}"; //(14) 약품코드
                    strPRT += strDRSABUN + "{$}"; //(15) 처방승인의사
                    strPRT += strQty + "{$}"; //(16) 사용수량

                    //GstrHelpCode = strPRT;
                    //향정마약처방전인쇄
                    frmNarcoticPrint Print = new frmNarcoticPrint(strPRT);
                    Print.StartPosition = FormStartPosition.CenterParent;
                    Print.ShowDialog(this);
                    Print.Dispose();
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {

        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {

        }

        private void SS1_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            Pen cPen = new Pen(Color.Black, 1);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 1;
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;
            Font font = new Font("맑은 고딕", 10, FontStyle.Regular);

            int strX = 0;
            int strY = 0;

            if (e.IsHeader == true)
            {
                #region 헤더
                //e.Graphics.DrawString(" 테스트 ", new Font("맑은 고딕", 15, FontStyle.Bold), Brushes.Black, 575, 20, drawFormat);
                #endregion

                #region 칸 그리기
                e.Graphics.DrawRectangle(cPen, 450, 50, 320, 130);

                e.Graphics.DrawLine(cPen, 770, 90, 450, 90);

                e.Graphics.DrawLine(cPen, 770, 130, 450, 130);

                //e.Graphics.DrawLine(cPen, 900, 115, 950, 115);

                //결재
                e.Graphics.DrawLine(cPen, 530, 90, 530, 180);
                ////담당
                e.Graphics.DrawLine(cPen, 610, 50, 610, 180);
                ////계장
                e.Graphics.DrawLine(cPen, 690, 90, 690, 180);

                //행정처장
                //e.Graphics.DrawLine(cPen, 1005, 90, 1005, 180);
                //전결처리
                //e.Graphics.DrawLine(cPen, 1005, 115, 935, 180);
                //e.Graphics.DrawLine(cPen, 1080, 115, 1005, 180);

                #endregion


                #region 칸안에 글
                //e.Graphics.DrawString("결", font, Brushes.Black, strX, 102, drawFormat);
                //e.Graphics.DrawString("재", font, Brushes.Black, strX, 102 + 47, drawFormat);
                //e.Graphics.DrawString("담    당", font, Brushes.Black, strX + 64, strY, drawFormat);
                //e.Graphics.DrawString("원무행정", font, Brushes.Black, strX + 137, strY, drawFormat);
                //e.Graphics.DrawString("팀    장", font, Brushes.Black, strX + 204, strY, drawFormat);
                //e.Graphics.DrawString("행정처장", font, Brushes.Black, strX + 278, strY, drawFormat);
                //e.Graphics.DrawString("병 원 장", font, Brushes.Black, strX + 348, strY, drawFormat);

                //e.Graphics.DrawString(" 전 결 ", font, Brushes.Black, strX + 198, strY + 42, drawFormat);

                e.Graphics.DrawString(" 신   청   인 ", font, Brushes.Black, strX + 570, strY + 60, drawFormat);
                e.Graphics.DrawString(" 수  령  확  인 ", font, Brushes.Black, strX + 730, strY + 60, drawFormat);
                e.Graphics.DrawString(" 종합검진 ", font, Brushes.Black, strX + 520, strY + 95, drawFormat);
                e.Graphics.DrawString(" 전 문 의 ", font, Brushes.Black, strX + 520, strY + 110, drawFormat);
                e.Graphics.DrawString(" 종합검진 ", font, Brushes.Black, strX + 600, strY + 95, drawFormat);
                e.Graphics.DrawString(" 간호팀장 ", font, Brushes.Black, strX + 600, strY + 110, drawFormat);
                e.Graphics.DrawString(" 종합검진 ", font, Brushes.Black, strX + 680, strY + 100, drawFormat);
                e.Graphics.DrawString(" 약 제 팀 ", font, Brushes.Black, strX + 755, strY + 100, drawFormat);

                #endregion

                #region 업무일지 / 작업일
                drawFormat.Alignment = StringAlignment.Near;
                //e.Graphics.DrawString("작성자   : " + clsType.User.UserName, font, Brushes.Black, 30, 85, drawFormat);
                //e.Graphics.DrawString("작업일자 : ", font, Brushes.Black, 30, 105, drawFormat);
                //e.Graphics.DrawString("출력시간 : " + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("yyyy-MM-dd HH:mm"), font, Brushes.Black, 30, 125, drawFormat);
                #endregion
            }
        }
    }
}
