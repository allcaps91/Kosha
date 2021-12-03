using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB.form.PmpaView
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMLREPB01.cs
    /// Description     : 전산실 각종 마감 명세서 (Ver2002-10-04))
    /// Author          : 김효성
    /// Create Date     : 2017-11-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// 사용하지 않음
    /// <seealso cref= D:\psmh\magam\mlrepb\mlrepb.vbp\MLREPB01.FRM >> frmPmPaMISUM405STS.cs 폼이름 재정의" />	

    public partial class frmPmpaViewMLREPB01 : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        string FstrPassGrade = "";

        #region 전역변수

        int nDbflag = 0;
        int nSub = 0;

        string strYY = "";
        string strMM = "";
        int nCount = 0;
        int nSelect = 0;
        int nSelect1 = 0;
        int nSelect2 = 0;
        int nSelect3 = 0;
        int nSelect4 = 0;
        string Title = "";

        string SaveSel = "";
        string strGubun = "";
        string StrSelect = "";
        string StrGwaName = "";
        string StrDrname = "";
        string strFlag = "";
        double TOTAmt = 0;
        double AmtTOT = 0;
        double nJubso = 0;
        double nJinso = 0;
        double nJubSuTot = 0;
        double nJinRuTot = 0;
        string StrBiGubun = "";
        string StrBiName = "";
        double nSubTot = 0;
        double nTot = 0;
        double nSubTot1 = 0;
        double nSubTot2 = 0;
        string StrLDate = "";

        //'===================================== SS1

        string StrGamRoom = "";
        string StrGamInDate = "";
        string StrGamOutdate = "";
        double nGamAmt1 = 0;
        double nGamAmt2 = 0;
        string strBigo = "";
        string[] StrData1 = new string[8];

        string[] strGamGwaName = new string[21];
        string strGwa = "";
        int[] nGamCntA = new int[28];
        double[] nGamAmtA = new double[28];

        string[] strGamGubun = new string[28];
        int[] nGamCntB = new int[28];
        double[] nGamAmtB = new double[28];

        //'===================================== SS2

        string strGamPano = "";
        string strGamName = "";
        string strGamGwa = "";
        string StrGamDr = "";
        double nGamAmt = 0;
        double nGamTotal = 0;
        double nGamAmt3 = 0;

        double nGamSo = 0;
        double nTottotGam = 0;
        string[] StrData2 = new string[7];

        //'===================================== SS3

        string StrMiRoom = "";
        string StrMiInDate = "";
        double nMiAmt = 0;
        string[] StrData3 = new string[8];

        //'===================================== SS4

        string StrMiPano = "";
        string StrMiName = "";
        string StrMiGwa = "";
        string StrMiDr = "";
        double nMiAmt1 = 0;
        double nMiAmt2 = 0;

        double nMiSo = 0;
        double nTottotMi = 0;
        string[] StrData4 = new string[6];

        //'===================================== SS5

        //Dim ILSUMonth(12)       As Integer
        //Dim nNal                As Integer
        //Dim StrGelCode          As String
        //Dim StrTPano            As String
        //Dim StrTName            As String
        //Dim StrTGwa             As String
        //Dim StrDate1            As String
        //Dim StrDate2            As String
        //Dim StrTNal             As String
        //Dim nTAmt1              As Double
        //Dim nTAmt2              As Double
        //Dim nTAmt3              As Double
        //Dim nTAmt4              As Double
        //Dim nTAmt5              As Double
        //Dim nTAmt6              As Double
        //Dim nTAmt7              As Double
        //Dim nTAmt8              As Double

        //Dim nSub1               As Double
        //Dim nSub2               As Double
        //Dim nSub3               As Double
        //Dim nSub4               As Double
        //Dim nSub5               As Double
        //Dim nSub6               As Double
        //Dim nSub7               As Double
        //Dim nSub8               As Double

        //Dim nTot1               As Double
        //Dim nTot2               As Double
        //Dim nTot3               As Double
        //Dim nTot4               As Double
        //Dim nTot5               As Double
        //Dim nTot6               As Double
        //Dim nTot7               As Double
        //Dim nTot8               As Double

        //Dim StrData5(8)         As String
        #endregion
        public frmPmpaViewMLREPB01()
        {
            InitializeComponent();
        }

        private void frmPmpaViewMLREPB01_Load(object sender, EventArgs e)
        {
            FstrPassGrade = clsPublic.FstrPassGrade;

            txtIntText11.Visible = false;
            txtIntText21.Visible = false;

            if ((FstrPassGrade == "MANAGER") || (FstrPassGrade == "CASHER") || ((VB.RTrim(FstrPassGrade) == "TOP") || (FstrPassGrade.Trim() == "TOP") || (FstrPassGrade == "EDPS")))
            {
                dtpDateText1.Visible = false;
                panDTP.Visible = false;
                panIntText1.Visible = false;
                panOptGamSelect.Visible = false;
                panrdoOptSelect.Visible = false;
            }
            else
            {
                ComFunc.MsgBox("당신은 이 프로그램을 사용할 수 없습니다.");
            }
            Gwa_Define_Load();

            dtpIntText1.Value = Convert.ToDateTime(strDTP);
            dtpIntText2.Value = Convert.ToDateTime(strDTP).AddMonths(-1);
        }

        #region 함수모음

        /// <summary>
        /// Gwa_Define_Load
        /// </summary>
        private void Gwa_Define_Load()
        {
            strGamGwaName[1] = "내       과";
            strGamGwaName[2] = "인 공 신 장";
            strGamGwaName[3] = "외       과";
            strGamGwaName[4] = "산 부 인 과";
            strGamGwaName[5] = "소  아   과";
            strGamGwaName[6] = "정 형 외 과";
            strGamGwaName[7] = "신 경 외 과";
            strGamGwaName[8] = "흉 부 외 과";
            strGamGwaName[9] = "신경 정신과";
            strGamGwaName[10] = "이비인후 과";
            strGamGwaName[11] = "안       과";
            strGamGwaName[12] = "비 뇨 기 과";
            strGamGwaName[13] = "피  부  과";
            strGamGwaName[14] = "치       과";
            strGamGwaName[15] = "통증 치료과";
            strGamGwaName[16] = "응  급   실";
            strGamGwaName[17] = "기       타";

            strGamGubun[1] = "타재단  성직자";
            strGamGubun[2] = "성직자  부  모";
            strGamGubun[3] = "성직자  친형제";
            strGamGubun[4] = "성직자  친  척";
            strGamGubun[5] = "재단  산하기관";
            strGamGubun[6] = "직원의  친  척";
            strGamGubun[7] = "승          려";
            strGamGubun[8] = "직  원  본  인";
            strGamGubun[9] = "직  원  배우자";
            strGamGubun[10] = "직원직계,존비속";
            strGamGubun[11] = "직원 장인,장모";
            strGamGubun[12] = "시    용    자";
            strGamGubun[13] = "금  액  할  인";
            strGamGubun[14] = "계    약    처";
            strGamGubun[15] = "실    업    자";
            strGamGubun[16] = "영  일  직  원";
            strGamGubun[17] = "영  일  가  족";
            strGamGubun[18] = "나자렛집 환 자";
            strGamGubun[19] = "마리아집 직 원";
            strGamGubun[20] = "마리아집 환 자";
            strGamGubun[21] = "햇빛마을 직 원";
            strGamGubun[22] = "햇빛마을 환 자";
            strGamGubun[23] = "자 원 봉 사 자";
            strGamGubun[24] = "퇴    직    자";
            strGamGubun[25] = "종합건진 접 수";
            strGamGubun[26] = "재 단 성 직 자";
            strGamGubun[27] = "감액(OGUR협진)";
        }

        private void ClearProcess()
        {
            //TODO 패널 깨짐
            //PanelTitle1 = ""
            //PanelTitle2 = ""
            //PanelTitle3 = ""

            dtpDateText1.Visible = false;
            dtpIntText1.Visible = false;
            dtpIntText2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;

            rdoOptGamSelect0.Checked = false;
            rdoOptGamSelect1.Checked = false;
            rdoOptSelect20.Checked = false;
            rdoOptSelect21.Checked = false;
            rdoOptSelect0.Checked = false;
            rdoOptSelect1.Checked = false;

            switch (nSelect1)
            {
                case 1:
                    btnMenuJobInGam1.Enabled = false;
                    break;
                case 2:
                    btnMenuJobInJib2.Enabled = false;
                    break;
                case 3:
                    btnMenuJobOutGam3.Enabled = false;
                    break;
                case 4:
                    btnMenuJobOutJib4.Enabled = false;
                    break;
                    //case 5: MenuJobInMisu.Checked = False
                    //case 6: MenuJobOutMIsu.Checked = False
                    //case 7: MenuJobTMisu.Checked = False
            }

            nSelect = 0;
            nSelect1 = 0;
            nSelect2 = 0;
            nSelect3 = 0;
        }

        private void Title1Show()
        {
            switch (nSelect1)
            {
                case 1:
                    this.Text = "입원 감액 명세서";
                    break;
                case 2:
                    this.Text = "입원 감액 집계표";
                    break;
                case 3:
                    this.Text = "외래 감액 명세서";
                    break;
                case 4:
                    this.Text = "외래 감액 집계표";
                    break;
                case 5:
                    this.Text = "입원 미수금 명세서";
                    break;
                case 6:
                    this.Text = "외래 미수금 명세서";
                    break;
                case 7:
                    this.Text = "퇴원자 미수금 명세서";
                    break;
            }

            rdoOptSelect0.Select();
        }

        private void SSPrintProcess()
        {
            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";
            string strHead1 = "";
            string strHead2 = "";
            string StrHead3 = "";
            string Headtitle = "";
            string StrLine1 = "";
            string StrLine2 = "";
            string StrLine3 = "";
            string StrLine4 = "";
            string StrLine5 = "";
            string StrLine6 = "";
            string strFoot = "";
            string JobDate = "";
            string PrintDate = "";
            string JobMan = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " Select TO_Char(SysDate,'YYYY-MM-DD  HH24:MM') SDate ";
                SQL = SQL + ComNum.VBLF + " From Dual ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                PrintDate = dt.Rows[0]["SDate"].ToString().Trim();

                if (nSelect == 1)
                {
                    //TODO GDate
                    //JobDate = clsPmpaType.G.GDate;
                }
                else
                {
                    JobDate = strYY + "-" + strMM;
                }
                JobMan = clsPublic.GstrJobName;

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        #endregion

        private void btnMenuJobInGam1_Click(object sender, EventArgs e)
        {
            btnMenuJobInGam1.Visible = true;
            btnMenuJobInJib2.Visible = false;
            btnMenuJobOutGam3.Visible = false;
            btnMenuJobOutJib4.Visible = false;

            nSelect1 = 1;

            rdoOptGamSelect0.Enabled = true;
            rdoOptGamSelect1.Enabled = true;
            rdoOptGamSelect2.Enabled = true;
            rdoOptGamSelect0.Checked = false;
            rdoOptGamSelect1.Checked = false;
            rdoOptGamSelect2.Checked = false;

            rdoOptSelect20.Enabled = true;
            rdoOptSelect21.Enabled = true;
            rdoOptSelect20.Checked = false;
            rdoOptSelect21.Checked = false;
            rdoOptSelect20.Enabled = false;
            rdoOptSelect21.Enabled = false;

            SS1.Visible = true;
            SS2.Visible = false;
            SS3.Visible = false;
            SS4.Visible = false;

            Title1Show();
        }

        private void btnMenuJobInJib2_Click(object sender, EventArgs e)
        {
            btnMenuJobInGam1.Visible = false;
            btnMenuJobInJib2.Visible = true;
            btnMenuJobOutGam3.Visible = false;
            btnMenuJobOutJib4.Visible = false;

            nSelect1 = 2;

            rdoOptGamSelect0.Enabled = true;
            rdoOptGamSelect1.Enabled = true;
            rdoOptGamSelect0.Checked = false;
            rdoOptGamSelect1.Checked = false;


            rdoOptSelect20.Enabled = true;
            rdoOptSelect21.Enabled = true;
            rdoOptSelect20.Checked = false;
            rdoOptSelect21.Checked = false;
            rdoOptSelect20.Enabled = false;
            rdoOptSelect21.Enabled = false;

            SS1.Visible = true;
            SS2.Visible = false;
            SS3.Visible = false;
            SS4.Visible = false;

            Title1Show();
        }

        private void btnMenuJobOutGam3_Click(object sender, EventArgs e)
        {
            btnMenuJobInGam1.Visible = false;
            btnMenuJobInJib2.Visible = false;
            btnMenuJobOutGam3.Visible = true;
            btnMenuJobOutJib4.Visible = false;

            rdoOptGamSelect0.Enabled = true;
            rdoOptGamSelect1.Enabled = true;
            rdoOptGamSelect0.Checked = false;
            rdoOptGamSelect1.Checked = false;


            rdoOptSelect20.Enabled = true;
            rdoOptSelect21.Enabled = true;
            rdoOptSelect20.Checked = false;
            rdoOptSelect21.Checked = false;
            rdoOptSelect20.Enabled = false;
            rdoOptSelect21.Enabled = false;

            SS1.Visible = false;
            SS2.Visible = true;
            SS3.Visible = false;
            SS4.Visible = false;

            Title1Show();
        }

        private void btnMenuJobOutJib4_Click(object sender, EventArgs e)
        {
            btnMenuJobInGam1.Visible = false;
            btnMenuJobInJib2.Visible = false;
            btnMenuJobOutGam3.Visible = false;
            btnMenuJobOutJib4.Visible = true;

            rdoOptGamSelect0.Enabled = true;
            rdoOptGamSelect1.Enabled = true;
            rdoOptGamSelect0.Checked = false;
            rdoOptGamSelect1.Checked = false;


            rdoOptSelect20.Enabled = true;
            rdoOptSelect21.Enabled = true;
            rdoOptSelect20.Checked = false;
            rdoOptSelect21.Checked = false;
            rdoOptSelect20.Enabled = false;
            rdoOptSelect21.Enabled = false;

            SS1.Visible = false;
            SS2.Visible = true;
            SS3.Visible = false;
            SS4.Visible = false;

            Title1Show();
        }

        private void btncensel_Click(object sender, EventArgs e)
        {
            ClearProcess();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            strMM = dtpIntText2.Value.Year.ToString();
            strYY = dtpIntText1.Value.Month.ToString();

            if (nSelect == 0)
            {
                ComFunc.MsgBox("작업구분이 정해지지 않았습니다.", "주의");
                rdoOptSelect0.Select();
                return;
            }
            //        ElseIf nSelect = 1 Then
            //    If(Trim$(GDate) = "" Or IsNull(GDate)) Then
            //       MsgBox "일자를 다시 입력해 주세요.", , "주의"
            //        DateText1.SetFocus
            //        Exit Sub
            //    ElseIf GDate > GstrSysDate Then
            //        MsgBox "오늘이후의 데이타는 볼 수 없습니다.", , "주의"
            //        DateText1.Text = Date
            //        DateText1.SetFocus
            //        Exit Sub
            //    Else
            //        PanelTitle2 = Mid$(GDate, 1, 4) &"년 " & Mid$(GDate, 6, 2) &"월 " & Mid$(GDate, 9, 2) &"일"
            //        PanelTitle3 = "일     보"
            //    End If

            //ElseIf nSelect = 2 Then
            //    If Trim$(IntText1.Text) = "0" Then
            //        MsgBox "년도를 다시 입력해 주세요.", , "주의"
            //        IntText1.SetFocus
            //        Exit Sub
            //    ElseIf Trim$(IntText2.Text) = "0" Then
            //        MsgBox "월을 다시 입력해 주세요.", , "주의"
            //        IntText2.SetFocus
            //        Exit Sub
            //    ElseIf IntText1.Text > Mid$(GstrSysDate, 1, 4) Then
            //        MsgBox "현재의 년도 보다 큽니다.", , "주의"
            //        IntText1.SetFocus
            //        Exit Sub
            //    Else
            //        PanelTitle2 = strYY & "년 " & strMM & "월"
            //        PanelTitle3 = "월     보"
            //    End If
            //ElseIf nSelect1 = 0 Then
            //        MsgBox "작업선택이 정해지지 않았습니다.", , "주의"
            //        SendKeys "%" & "J"
            //        Exit Sub
            //ElseIf nSelect1 = 3 Or nSelect1 = 4 Or nSelect1 = 5 Then
            //    If nSelect2 = 0 Then
            //        MsgBox "미수선택이 정해지지 않았습니다.", , "주의"
            //        OptSelect2(0).Enabled = True
            //        OptSelect2(1).Enabled = True
            //        OptSelect2(0).SetFocus
            //        Exit Sub
            //    End If
            //End If

            if (this.Text != "퇴원자 미수금 명세서")
            {
                if (nSelect3 == 0 && nSelect1 != 5 && nSelect1 != 6)
                {
                    ComFunc.MsgBox("감액구분을 선택해주세요!", "주의");
                    rdoOptGamSelect0.Select();
                }
            }

            switch (nSelect1)
            {
                //TODO
                //case 1:
                //    SS1BuildMain(); //'입원감액내역
                //    break;
                //case 2:
                //    SS11PrintBuild(); //'입원감액집계내역
                //    break;
                //case 3:
                //    SS2BuildMain(); //'외래감액내역
                //    break;
                //case 4:
                //    SS21PrintBuild(); //'외래감액집계내역
                //    break;
                //case 5: Call SS3BuildMain     '입원미수금내역
                //case 6: Call SS4BuildMain     '외래미수금내역
                //case 7: Call SS5BuildMain     '퇴원미수금내역
            }
            if (nSelect4 == 1 && nSelect1 != 2 && nSelect1 != 4)
            {
                SSPrintProcess();
            }

            ClearProcess();
        }

        private void dtpIntText1_Enter(object sender, EventArgs e)
        {

        }
    }
}
