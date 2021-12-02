using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupPrtADR.cs
    /// Description     : 약물이상반응 평가 보고서 인쇄 미리보기
    /// Author          : 이정현
    /// Create Date     : 2018-01-12
    /// <history> 
    /// 약물이상반응 평가 보고서 인쇄 미리보기
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmPrtADR.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupPrtADR : Form
    {
        private string GstrSEQNO = "";
        
        public frmComSupPrtADR(string strSEQNO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
        }

        private void frmComSupPrtADR_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssPage1.Visible = true;
            ssPage1.Dock = DockStyle.Fill;

            ssPage2.Visible = false;
            ssPage2.Dock = DockStyle.None;

            SetSS1(GstrSEQNO);
            SetSS2(GstrSEQNO);
        }

        private void ClearSS1()
        {
            ssPage1_Sheet1.Cells[3, 5].Text = "";
            ssPage1_Sheet1.Cells[3, 11].Text = "";
            ssPage1_Sheet1.Cells[3, 17].Text = "";
            ssPage1_Sheet1.Cells[3, 23].Text = "";

            ssPage1_Sheet1.Cells[4, 5].Text = "";
            ssPage1_Sheet1.Cells[4, 11].Text = "";

            ssPage1_Sheet1.Cells[6, 5].Text = "";
            ssPage1_Sheet1.Cells[6, 10].Text = "";
            ssPage1_Sheet1.Cells[6, 15].Text = "";
            ssPage1_Sheet1.Cells[6, 20].Text = "";
            ssPage1_Sheet1.Cells[6, 24].Text = "";

            ssPage1_Sheet1.Cells[7, 5].Text = "";
            ssPage1_Sheet1.Cells[7, 15].Text = "";

            ssPage1_Sheet1.Cells[8, 5].Text = "";
            ssPage1_Sheet1.Cells[8, 9].Text = "";
            ssPage1_Sheet1.Cells[8, 13].Text = "";
            ssPage1_Sheet1.Cells[8, 17].Text = "";

            ssPage1_Sheet1.Cells[11, 2].Text = "";
            ssPage1_Sheet1.Cells[11, 4].Text = "";
            ssPage1_Sheet1.Cells[11, 7].Text = "";
            ssPage1_Sheet1.Cells[11, 16].Text = "";
            ssPage1_Sheet1.Cells[11, 19].Text = "";
            ssPage1_Sheet1.Cells[11, 23].Text = "";

            ssPage1_Sheet1.Cells[12, 2].Text = "";
            ssPage1_Sheet1.Cells[12, 4].Text = "";
            ssPage1_Sheet1.Cells[12, 7].Text = "";
            ssPage1_Sheet1.Cells[12, 16].Text = "";
            ssPage1_Sheet1.Cells[12, 19].Text = "";
            ssPage1_Sheet1.Cells[12, 23].Text = "";

            ssPage1_Sheet1.Cells[13, 2].Text = "";
            ssPage1_Sheet1.Cells[13, 4].Text = "";
            ssPage1_Sheet1.Cells[13, 7].Text = "";
            ssPage1_Sheet1.Cells[13, 16].Text = "";
            ssPage1_Sheet1.Cells[13, 19].Text = "";
            ssPage1_Sheet1.Cells[13, 23].Text = "";

            ssPage1_Sheet1.Cells[14, 2].Text = "";
            ssPage1_Sheet1.Cells[14, 4].Text = "";
            ssPage1_Sheet1.Cells[14, 7].Text = "";
            ssPage1_Sheet1.Cells[14, 16].Text = "";
            ssPage1_Sheet1.Cells[14, 19].Text = "";
            ssPage1_Sheet1.Cells[14, 23].Text = "";

            ssPage1_Sheet1.Cells[16, 7].Text = "";
            ssPage1_Sheet1.Cells[17, 7].Text = "";
            ssPage1_Sheet1.Cells[18, 7].Text = "";
            ssPage1_Sheet1.Cells[19, 7].Text = "";
        }

        private void ClearSS2()
        {
            ssPage2_Sheet1.Cells[3, 7].Text = "";

            ssPage2_Sheet1.Cells[4, 7].Text = "";

            ssPage2_Sheet1.Cells[6, 7].Text = "";
            ssPage2_Sheet1.Cells[6, 17].Text = "";

            ssPage2_Sheet1.Cells[7, 7].Text = "";

            ssPage2_Sheet1.Cells[8, 7].Text = "";

            ssPage2_Sheet1.Cells[11, 2].Text = "";
            ssPage2_Sheet1.Cells[11, 4].Text = "";
            ssPage2_Sheet1.Cells[11, 7].Text = "";
            ssPage2_Sheet1.Cells[11, 16].Text = "";
            ssPage2_Sheet1.Cells[11, 19].Text = "";
            ssPage2_Sheet1.Cells[11, 23].Text = "";

            ssPage2_Sheet1.Cells[12, 2].Text = "";
            ssPage2_Sheet1.Cells[12, 4].Text = "";
            ssPage2_Sheet1.Cells[12, 7].Text = "";
            ssPage2_Sheet1.Cells[12, 16].Text = "";
            ssPage2_Sheet1.Cells[12, 19].Text = "";
            ssPage2_Sheet1.Cells[12, 23].Text = "";

            ssPage2_Sheet1.Cells[13, 2].Text = "";
            ssPage2_Sheet1.Cells[13, 4].Text = "";
            ssPage2_Sheet1.Cells[13, 7].Text = "";
            ssPage2_Sheet1.Cells[13, 16].Text = "";
            ssPage2_Sheet1.Cells[13, 19].Text = "";
            ssPage2_Sheet1.Cells[13, 23].Text = "";

            ssPage2_Sheet1.Cells[14, 2].Text = "";
            ssPage2_Sheet1.Cells[14, 4].Text = "";
            ssPage2_Sheet1.Cells[14, 7].Text = "";
            ssPage2_Sheet1.Cells[14, 16].Text = "";
            ssPage2_Sheet1.Cells[14, 19].Text = "";
            ssPage2_Sheet1.Cells[14, 23].Text = "";

            ssPage2_Sheet1.Cells[16, 7].Text = "";
            ssPage2_Sheet1.Cells[17, 7].Text = "";
            ssPage2_Sheet1.Cells[18, 7].Text = "";
            ssPage2_Sheet1.Cells[20, 7].Text = "";
            ssPage2_Sheet1.Cells[21, 7].Text = "";
            ssPage2_Sheet1.Cells[23, 7].Text = "";
        }

        private void SetSS1(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string str01 = "";
            string str02 = "";
            string str03 = "";
            string str04 = "";
            string str05 = "";
            string str06 = "";
            string str07 = "";
            string str08 = "";
            string str09 = "";
            string str10 = "";
            string str11 = "";
            string str12 = "";
            string str13 = "";
            string str14 = "";
            string str15 = "";
            string str16 = "";
            string str17 = "";
            string str18 = "";
            string str19 = "";
            string str20 = "";
            string str21 = "";

            string strTEMP1 = "";
            string strTEMP2 = "";
            string strTEMP3 = "";
            string strTEMP4 = "";

            string str04_1 = "";
            string str04_2 = "";

            ClearSS1();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.SEQNO,";
                SQL = SQL + ComNum.VBLF + "     C.WDATE, ";              //최종평가일,"
                SQL = SQL + ComNum.VBLF + "     B.WNAME AS WNAME1, ";       // 평가자1차,"
                SQL = SQL + ComNum.VBLF + "     C.WNAME AS WNAME2, ";       // 평가자2차,"
                SQL = SQL + ComNum.VBLF + "     D.REPORT1, ";           // 위원회보고,"
                SQL = SQL + ComNum.VBLF + "     D.REPORT2, ";            // 식약처보고,"
                SQL = SQL + ComNum.VBLF + "     A.WDATE AS WDATE2, ";       // 이상반응보고일,"
                SQL = SQL + ComNum.VBLF + "     A.WNAME AS WNAME3, ";       // 이상반응보고자,"
                SQL = SQL + ComNum.VBLF + "     A.WBUSE, A.WRITESABUN, A.PTNO, A.SNAME, A.AGESEX, A.PATIENT_BUN, A.ROOMCODE, A.DEPTCODE, A.DIAGNAME,";
                SQL = SQL + ComNum.VBLF + "     DECODE(a.IMSIN1, '1', '유', '') || DECODE(a.IMSIN2, '1', '무', '') || DECODE(a.IMSIN3, '1', '모름', '') AS IMSIN, ";   //임신여부"
                SQL = SQL + ComNum.VBLF + "     DECODE(a.SMOKE1, '1', '유', '') || DECODE(a.SMOKE2, '1', '무', '') || DECODE(a.SMOKE3, '1', '모름', '') AS SMOKE, ";   //흡연여부"
                SQL = SQL + ComNum.VBLF + "     DECODE(a.DRUNK1, '1', '유', '') || DECODE(a.DRUNK2, '1', '무', '') || DECODE(a.DRUNK3, '1', '모름', '') AS DRUNK, ";   //음주여부"
                SQL = SQL + ComNum.VBLF + "     a.ALLERGY , ";          //알러지"
                SQL = SQL + ComNum.VBLF + "     a.BDATE, ";            //증상발현일"
                SQL = SQL + ComNum.VBLF + "     A.BTIME1, ";            //투약중 발현"
                SQL = SQL + ComNum.VBLF + "     a.BTIME1TIME, ";       //시간"
                SQL = SQL + ComNum.VBLF + "     a.BTIME1SECOND, ";     //분"
                SQL = SQL + ComNum.VBLF + "     A.BTIME2, ";           //투약종료 후 발현"
                SQL = SQL + ComNum.VBLF + "     a.BTIME2INSTANCE, ";   //즉시"
                SQL = SQL + ComNum.VBLF + "     a.BTIME2SECOND, ";     //분"
                SQL = SQL + ComNum.VBLF + "     a.BTIME2TIME, ";       //시간"
                SQL = SQL + ComNum.VBLF + "     a.BTIME2DAY, ";        // 일"
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_A1, '1', '발열, ', '') || DECODE(A.RACT_A2, '1', '식욕감소, ', '') || DECODE(A.RACT_A3, '1', '전신부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_A4, '1', '전신쇠약, ', '') || DECODE(A.RACT_A5, '1', '체중감소, ', '') || DECODE(A.RACT_A6, '1', '체중증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B1, '1', '가려움증, ', '') || DECODE(A.RACT_B2, '1', '가려운 발진, ', '') || DECODE(A.RACT_B3, '1', '농포성 발진, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B4, '1', '두드러기, ', '') || DECODE(A.RACT_B5, '1', '여드름성 발진, ', '') || DECODE(A.RACT_B6, '1', '피부작리, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_B7, '1', '피부변색, ', '') || DECODE(A.RACT_B8, '1', '혈관부종, ', '') || DECODE(A.RACT_B9, '1', '탈모, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C1, '1', '구강칸디다증, ', '') || DECODE(A.RACT_C2, '1', '구강건조증, ', '') || DECODE(A.RACT_C3, '1', '귀울림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C4, '1', '급성청각이상, ', '') || DECODE(A.RACT_C5, '1', '미각이상, ', '') || DECODE(A.RACT_C6, '1', '시각장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_C7, '1', '안압상승, ', '') || DECODE(A.RACT_C8, '1', '음성변화, ', '') || DECODE(A.RACT_D1, '1', '가슴불편함, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_D2, '1', '부정맥, ', '') || DECODE(A.RACT_D3, '1', '빈맥, ', '') || DECODE(A.RACT_D4, '1', '실신, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_D5, '1', '심부전, ', '') || DECODE(A.RACT_D6, '1', '저혈압, ', '') || DECODE(A.RACT_D7, '1', '고혈압, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E1, '1', '오심/구토, ', '') || DECODE(A.RACT_E2, '1', '변비, ', '') || DECODE(A.RACT_E3, '1', '복통, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E4, '1', '설사, ', '') || DECODE(A.RACT_E5, '1', '소화불량, ', '') || DECODE(A.RACT_E6, '1', '위장관통증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_E7, '1', '위출혈, ', '') || DECODE(A.RACT_F1, '1', '빌리루빈증가, ', '') || DECODE(A.RACT_F2, '1', 'AST/ALT 증가, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_G1, '1', '기침, ', '') || DECODE(A.RACT_G2, '1', '호흡곤란, ', '') || DECODE(A.RACT_G3, '1', '폐부종, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_H1, '1', '백혈구감소증, ', '') || DECODE(A.RACT_H2, '1', '빈혈, ', '') || DECODE(A.RACT_H3, '1', '응고장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_H4, '1', '혈소판감소증, ', '') || DECODE(A.RACT_I1, '1', '단백뇨, ', '') || DECODE(A.RACT_I2, '1', '신기능장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_I3, '1', '혈뇨, ', '') || DECODE(A.RACT_I4, '1', '혈중 Creatinine 증가, ', '') || DECODE(A.RACT_J1, '1', '기억력장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J2, '1', '두통, ', '') || DECODE(A.RACT_J3, '1', '보행곤란, ', '') || DECODE(A.RACT_J4, '1', '사지떨림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J5, '1', '수면장애, ', '') || DECODE(A.RACT_J6, '1', '어지러움, ', '') || DECODE(A.RACT_J7, '1', '언어장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J8, '1', '의식저하, ', '') || DECODE(A.RACT_J9, '1', '운동이상증, ', '') || DECODE(A.RACT_J10, '1', '졸림, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J11, '1', '피부저림, ', '') || DECODE(A.RACT_J12, '1', '불안, ', '') || DECODE(A.RACT_J13, '1', '섬망, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_J14, '1', '신경과민, ', '') || DECODE(A.RACT_J15, '1', '우울, ', '') || DECODE(A.RACT_J16, '1', '과행동, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K1, '1', '고혈당증, ', '') || DECODE(A.RACT_K2, '1', '저혈당증, ', '') || DECODE(A.RACT_K3, '1', '배뇨장애, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K4, '1', '성기능장애, ', '') || DECODE(A.RACT_K5, '1', '성욕감소, ', '') || DECODE(A.RACT_K6, '1', '여성형유방, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_K7, '1', '월경불순, ', '') || DECODE(A.RACT_L1, '1', '관절통, ', '') || DECODE(A.RACT_L2, '1', '골다공증, ', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(A.RACT_L3, '1', '근육통, ', '') AS RACT,";
                SQL = SQL + ComNum.VBLF + "     A.RACTMEMO,";
                SQL = SQL + ComNum.VBLF + "     DECODE(a.CLASS1, '1', 'mild', '') || DECODE(a.CLASS2, '1', 'moderate', '') || DECODE(a.CLASS3, '1', 'severe', '') || DECODE(a.CLASS4, '1', 'serious', '') AS CLASS";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A, " + ComNum.DB_ERP + "DRUG_ADR2 B, " + ComNum.DB_ERP + "DRUG_ADR3 C, " + ComNum.DB_ERP + "DRUG_ADR4 D";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = D.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["WDATE"].ToString().Trim() != "")
                    {
                        str01 = Convert.ToDateTime(dt.Rows[0]["WDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }

                    str02 = dt.Rows[0]["WNAME1"].ToString().Trim();
                    str03 = dt.Rows[0]["WNAME2"].ToString().Trim();

                    if (dt.Rows[0]["REPORT1"].ToString().Trim() == "1") { str04_1 = "약물이상반응 모니터링 위원회 보고"; }
                    if (dt.Rows[0]["REPORT2"].ToString().Trim() == "1") { str04_2 = "식약처 보고"; }

                    if (str04_1 != "" && str04_2 != "")
                    {
                        str04 = str04_1 + ", " + str04_2;
                    }
                    else
                    {
                        str04 = str04_1 + str04_2;
                    }

                    if (dt.Rows[0]["WDATE2"].ToString().Trim() != "")
                    {
                        str05 = Convert.ToDateTime(dt.Rows[0]["WDATE2"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }

                    str06 = "     (성명)  " + dt.Rows[0]["WNAME3"].ToString().Trim() + "     (부서)  " + dt.Rows[0]["WBUSE"].ToString().Trim();
                    str07 = dt.Rows[0]["PTNO"].ToString().Trim();
                    str08 = dt.Rows[0]["SNAME"].ToString().Trim();
                    str09 = dt.Rows[0]["AGESEX"].ToString().Trim();
                    str10 = dt.Rows[0]["PATIENT_BUN"].ToString().Trim();
                    str11 = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    str12 = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    str13 = dt.Rows[0]["DIAGNAME"].ToString().Trim();
                    str14 = dt.Rows[0]["IMSIN"].ToString().Trim();
                    str15 = dt.Rows[0]["SMOKE"].ToString().Trim();
                    str16 = dt.Rows[0]["DRUNK"].ToString().Trim();
                    str17 = dt.Rows[0]["ALLERGY"].ToString().Trim();

                    if (dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    {
                        str18 = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }

                    if (dt.Rows[0]["BTIME1"].ToString().Trim() == "1")
                    {
                        strTEMP1 = dt.Rows[0]["BTIME1TIME"].ToString().Trim();
                        if (strTEMP1 != "") { strTEMP1 = strTEMP1 + " 시 "; }
                        strTEMP2 = dt.Rows[0]["BTIME1SECOND"].ToString().Trim();
                        if (strTEMP2 != "") { strTEMP2 = strTEMP2 + " 분 "; }
                        str19 = " ★) 투약 중 발현";
                        str19 = str19 + ComNum.VBLF + " ※ IV infusion인 경우 : 투약 개시 " + strTEMP1 + strTEMP2 + " 경과 후 발현";
                    }
                    else if (dt.Rows[0]["BTIME2"].ToString().Trim() == "1")
                    {
                        strTEMP1 = dt.Rows[0]["BTIME2INSTANCE"].ToString().Trim();
                        if (strTEMP1 == "1") { strTEMP1 = " 즉시(1분 이내) "; }
                        strTEMP2 = dt.Rows[0]["BTIME2SECOND"].ToString().Trim();
                        if (strTEMP2 != "") { strTEMP2 = strTEMP2 + " 분 "; }
                        strTEMP3 = dt.Rows[0]["BTIME2TIME"].ToString().Trim();
                        if (strTEMP3 != "") { strTEMP3 = strTEMP3 + " 시간 "; }
                        strTEMP4 = dt.Rows[0]["BTIME2DAY"].ToString().Trim();
                        if (strTEMP4 != "") { strTEMP4 = strTEMP4 + " 일 "; }

                        str19 = " ★) 투약 종료 후 발현";
                        if (strTEMP1 != "0")
                        {
                            str19 = str19 + "    " + strTEMP1;
                        }
                        else
                        {
                            str19 = str19 + "    " + strTEMP2 + strTEMP3 + strTEMP4 + " 경과 후 발현 ";
                        }
                    }

                    str20 = dt.Rows[0]["RACT"].ToString().Trim();
                    if (str20 != "")
                    {
                        str20 = VB.Mid(str20, 1, str20.Length - 1);
                    }

                    strTEMP1 = dt.Rows[0]["RACTMEMO"].ToString().Trim();
                    if (strTEMP1 != "")
                    {
                        str20 = str20 + ComNum.VBLF + " * 참고사항 : " + strTEMP1;
                    }

                    str21 = dt.Rows[0]["CLASS"].ToString().Trim();

                    switch (str21)
                    {
                        case "mild":
                            str21 = " - mild (경증)";
                            str21 = str21 + ComNum.VBLF + " - 증상 또는 징후를 자각할 수 있으나 불편감을 주지 않고 참을 수 있음";
                            str21 = str21 + ComNum.VBLF + " - 행동이나 기능에 영향을 미치치 않음";
                            str21 = str21 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요하지 않음";
                            break;
                        case "moderate":
                            str21 = " - moderate (중등증)";
                            str21 = str21 + ComNum.VBLF + " - 증상이 일상의 활동을 방해할 만큼 불편함";
                            str21 = str21 + ComNum.VBLF + " - 행동에 영향을 미침";
                            str21 = str21 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요할 수 있음";
                            break;
                        case "severe":
                            str21 = " - severe (중증)";
                            str21 = str21 + ComNum.VBLF + " - 증상이 일이나 일상의 활동을 수행할 수  없을 만큼 불편감을 야기함";
                            str21 = str21 + ComNum.VBLF + " - 의심약물을 중단할 만큼 불편감이 있음";
                            str21 = str21 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요함";
                            break;
                        case "serious":
                            str21 = " - serious (중대함)";
                            str21 = str21 + ComNum.VBLF + " - 사망을 초래하거나 생명의 위협을 연장하는 경우";
                            str21 = str21 + ComNum.VBLF + " - 입원 또는 입원기간의 연장이 필요한 경우";
                            str21 = str21 + ComNum.VBLF + " - 지속적 또는 중대한 불구나 기능저하를 초래하는 경우";
                            str21 = str21 + ComNum.VBLF + " - 선천적 기형 또는 이상을 초래하는 경우";
                            str21 = str21 + ComNum.VBLF + " - 기타 의학적으로 중요한 상황";
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                ssPage1_Sheet1.Cells[3, 5].Text = str01;
                ssPage1_Sheet1.Cells[3, 11].Text = str02;
                ssPage1_Sheet1.Cells[3, 17].Text = str03;
                ssPage1_Sheet1.Cells[3, 23].Text = str04;

                ssPage1_Sheet1.Cells[4, 5].Text = str05;
                ssPage1_Sheet1.Cells[4, 11].Text = str06;

                ssPage1_Sheet1.Cells[6, 5].Text = str07;
                ssPage1_Sheet1.Cells[6, 10].Text = str08;
                ssPage1_Sheet1.Cells[6, 15].Text = str09;
                ssPage1_Sheet1.Cells[6, 20].Text = str10;
                ssPage1_Sheet1.Cells[6, 24].Text = str11;

                ssPage1_Sheet1.Cells[7, 5].Text = str12;
                ssPage1_Sheet1.Cells[7, 15].Text = str13;

                ssPage1_Sheet1.Cells[8, 5].Text = str14;
                ssPage1_Sheet1.Cells[8, 9].Text = str15;
                ssPage1_Sheet1.Cells[8, 13].Text = str16;
                ssPage1_Sheet1.Cells[8, 17].Text = str17;

                ssPage1_Sheet1.Cells[16, 7].Text = str18;
                ssPage1_Sheet1.Cells[17, 7].Text = str19;
                ssPage1_Sheet1.Cells[18, 7].Text = str20;
                ssPage1_Sheet1.Cells[19, 7].Text = str21;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, QTY, DOSNAME, DECODE(CHECK1, '1', '유', '') || DECODE(CHECK2, '1', '무', '') || DECODE(CHECK3, '1', '모름', '') AS USED";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "         AND ROWNUM < 3 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPage1_Sheet1.Cells[i + 11, 2].Text = (i + 1).ToString();
                        ssPage1_Sheet1.Cells[i + 11, 4].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 7].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 16].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 19].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 23].Text = dt.Rows[i]["USED"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, INSPEED, QTY, DECODE(CHECK1, '1', '유', '') || DECODE(CHECK2, '1', '무', '') || DECODE(CHECK3, '1', '모름', '') AS USED";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "         AND ROWNUM < 3 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPage1_Sheet1.Cells[10, 16].Text = "주입속도(ml/sec)";
                    ssPage1_Sheet1.Cells[10, 19].Text = "주입량(ml)";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPage1_Sheet1.Cells[i + 11, 2].Text = (i + 1).ToString();
                        ssPage1_Sheet1.Cells[i + 11, 4].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 7].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 16].Text = dt.Rows[i]["INSPEED"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 19].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssPage1_Sheet1.Cells[i + 11, 23].Text = dt.Rows[i]["USED"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetSS2(string strSEQNO)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string str01 = "";
            string str02 = "";
            string str03 = "";
            string str04 = "";
            string str05 = "";
            string str06 = "";
            string str07 = "";
            string str08 = "";
            string str09 = "";
            string str10 = "";
            string str11 = "";

            string str08_1 = "";
            string str08_2 = "";

            string strTEMP1 = "";
            string strTEMP2 = "";
            string strTEMP3 = "";

            ClearSS2();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT1, '1', '관찰 (투약 종료 또는 투약 유지)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT2, '1', '투약 중지', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3, '1', '투약변경', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3_1, '1', ' (용량 변경)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3_2, '1', ' (용법 변경)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECEPT3_3, '1', ' (약물 변경)', '') AS RECEPT,";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER1, '1', '없음', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER2, '1', '항히스타민제-IV', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER3, '1', '항히스타민제-PO', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER4, '1', '스테로이드제-IV', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER5, '1', '스테로이드제-PO', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER6, '1', '에피네프린', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER7, '1', 'Hydration', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER8, '1', '기관지 확장제', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(EMER9, '1', '기타 : ', '') ||";
                SQL = SQL + ComNum.VBLF + "     EMER9MEMO AS EMER,";
                SQL = SQL + ComNum.VBLF + "     RESULTDATE,";
                SQL = SQL + ComNum.VBLF + "     RESULTTIME,";
                SQL = SQL + ComNum.VBLF + "     RESULTSEC,";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER1, '1', 'ER 방문', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER2, '1', '주사실 방문', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER3, '1', '입원', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER4, '1', '입원 연장', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER5, '1', '회복되지 않음', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RECOVER6, '1', '기타 : ', '') ||";
                SQL = SQL + ComNum.VBLF + "     RECOVER6MEMO AS RECOVE,";
                SQL = SQL + ComNum.VBLF + "     DECODE(RETUYAK1, '1', '재 투여하지 않음', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RETUYAK2, '1', '재 투여시 유사증상 재 발현', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RETUYAK3, '1', '재 투여 시 유사증상 없음', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(RETUYAK4, '1', '재 투약 정보 없음', '') AS RETUYAK,";
                SQL = SQL + ComNum.VBLF + "     PROGRESSMEMO,";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.RELATION1, '1', '확실한(Certain)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.RELATION2, '1', '상당히 확실함(Probable)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.RELATION3, '1', '가능함(Possible)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.RELATION4, '1', '가능성 적음(Unlikely)', '') AS BRELATION,";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.RELATION1, '1', '확실한(Certain)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.RELATION2, '1', '상당히 확실함(Probable)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.RELATION3, '1', '가능함(Possible)', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.RELATION4, '1', '가능성 적음(Unlikely)', '') AS CRELATION,";
                SQL = SQL + ComNum.VBLF + "     B.RELATIONMEMO AS RELATIONMEMO1,";
                SQL = SQL + ComNum.VBLF + "     C.RELATIONMEMO,";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.CLASS1, '1', 'mild', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.CLASS2, '1', 'moderate', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.CLASS3, '1', 'severe', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(B.CLASS4, '1', 'serious', '') AS BCLASS,";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.CLASS1, '1', 'mild', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.CLASS2, '1', 'moderate', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.CLASS3, '1', 'severe', '') ||";
                SQL = SQL + ComNum.VBLF + "     DECODE(C.CLASS4, '1', 'serious', '') AS CCLASS";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A, " + ComNum.DB_ERP + "DRUG_ADR2 B, " + ComNum.DB_ERP + "DRUG_ADR3 C, " + ComNum.DB_ERP + "DRUG_ADR4 D";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = D.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    str01 = dt.Rows[0]["RECEPT"].ToString().Trim();
                    str02 = dt.Rows[0]["EMER"].ToString().Trim();
                    strTEMP1 = dt.Rows[0]["RESULTDATE"].ToString().Trim();
                    if (strTEMP1 != "") { strTEMP1 = strTEMP1 + " 일 "; }
                    strTEMP2 = dt.Rows[0]["RESULTTIME"].ToString().Trim();
                    if (strTEMP2 != "") { strTEMP2 = strTEMP2 + " 시간 "; }
                    strTEMP3 = dt.Rows[0]["RESULTSEC"].ToString().Trim();
                    if (strTEMP3 != "") { strTEMP3 = strTEMP3 + " 분 "; }

                    if (strTEMP1 != "" || strTEMP2 != "" || strTEMP3 != "")
                    {
                        str03 = " ＊ 자연회복인 경우 ";
                        str03 = str03 + ComNum.VBLF + strTEMP1 + strTEMP2 + strTEMP3 + " 경과 후 회복됨";
                    }
                    else if (dt.Rows[0]["RECOVE"].ToString().Trim() != "")
                    {
                        str03 = " * 응급 처치 후 회복인 경우 ";
                        str03 = str03 + ComNum.VBLF + dt.Rows[0]["RECOVE"].ToString().Trim();
                    }

                    str04 = dt.Rows[0]["RETUYAK"].ToString().Trim();
                    str05 = dt.Rows[0]["PROGRESSMEMO"].ToString().Trim();
                    str06 = dt.Rows[0]["BRELATION"].ToString().Trim();

                    switch (str06)
                    {
                        case "확실한 (Certain)":
                            str06 = " - " + str06;
                            str06 = str06 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 전후 관계가 타당하고 다른 의약품이나 화학물질 또는 수반하는 질환으로 설명되지 않음";
                            str06 = str06 + ComNum.VBLF + " - 그 의약품 등의 투여중단 시 임상적으로 타당한 반응을 보이고, 필요에 따른 그 의약품 등의 재 투여 시, 약물학적 또는 현상학적으로 결정적인 경우";
                            break;
                        case "상당히 확실함(Probable)":
                            str06 = " - " + str06;
                            str06 = str06 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 시간적 관계가 합당하고 다른 의약품이나 화학물질 또는 수반하는 질환에 의한 것으로 보이지 아니하며, 그 의약품 등의 투여중단 시 임상적으로 합당한 반응을 보이는 경우(재 투여 정보 없음)";
                            break;
                        case "가능함(Possible)":
                            str06 = " - " + str06;
                            str06 = str06 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 시간적 관계가 합당하나 다른 의약품이나 화학물질 또는 수반하는 질환에 의한 것으로도 설명되며, 그 의약품 등의 투여중단에 관한 정보가 부족하거나 불명확한 경우";
                            break;
                        case "가능성 적음(Unlikely)":
                            str06 = " - " + str06;
                            str06 = str06 + ComNum.VBLF + " - 의약품 등의 투여, 사용과 인과 관계가 있을 것 같지 않은 일시적 사례이고, 다른 의약품이나 화학물질 또는 잠재된 질환에 의한 것으로도 타당한 설명이 가능한 경우";
                            break;
                    }

                    str07 = dt.Rows[0]["CRELATION"].ToString().Trim();

                    switch (str07)
                    {
                        case "확실한 (Certain)":
                            str07 = " - " + str07;
                            str07 = str07 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 전후 관계가 타당하고 다른 의약품이나 화학물질 또는 수반하는 질환으로 설명되지 않음";
                            str07 = str07 + ComNum.VBLF + " - 그 의약품 등의 투여중단 시 임상적으로 타당한 반응을 보이고, 필요에 따른 그 의약품 등의 재 투여 시, 약물학적 또는 현상학적으로 결정적인 경우";
                            break;
                        case "상당히 확실함(Probable)":
                            str07 = " - " + str07;
                            str07 = str07 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 시간적 관계가 합당하고 다른 의약품이나 화학물질 또는 수반하는 질환에 의한 것으로 보이지 아니하며, 그 의약품 등의 투여중단 시 임상적으로 합당한 반응을 보이는 경우(재 투여 정보 없음)";
                            break;
                        case "가능함(Possible)":
                            str07 = " - " + str07;
                            str07 = str07 + ComNum.VBLF + " - 의약품 등의 투여, 사용과의 시간적 관계가 합당하나 다른 의약품이나 화학물질 또는 수반하는 질환에 의한 것으로도 설명되며, 그 의약품 등의 투여중단에 관한 정보가 부족하거나 불명확한 경우";
                            break;
                        case "가능성 적음(Unlikely)":
                            str07 = " - " + str07;
                            str07 = str07 + ComNum.VBLF + " - 의약품 등의 투여, 사용과 인과 관계가 있을 것 같지 않은 일시적 사례이고, 다른 의약품이나 화학물질 또는 잠재된 질환에 의한 것으로도 타당한 설명이 가능한 경우";
                            break;
                    }

                    str08 = "";

                    str08_1 = dt.Rows[0]["RELATIONMEMO1"].ToString().Trim();
                    str08_2 = dt.Rows[0]["RELATIONMEMO"].ToString().Trim();

                    if (str08_1 != "") { str08_1 = str08_1; }
                    if (str08_2 != "") { str08_2 = str08_2; }

                    //str08 = str08_1 + ComNum.VBLF + str09_1;

                    switch (str09)
                    {
                        case "mild":
                            str09 = " - mild (경증)";
                            str09 = str09 + ComNum.VBLF + " - 증상 또는 징후를 자각할 수 있으나 불편감을 주지 않고 참을 수 있음";
                            str09 = str09 + ComNum.VBLF + " - 행동이나 기능에 영향을 미치치 않음";
                            str09 = str09 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요하지 않음";
                            break;
                        case "moderate":
                            str09 = " - moderate (중등증)";
                            str09 = str09 + ComNum.VBLF + " - 증상이 일상의 활동을 방해할 만큼 불편함";
                            str09 = str09 + ComNum.VBLF + " - 행동에 영향을 미침";
                            str09 = str09 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요할 수 있음";
                            break;
                        case "severe":
                            str09 = " - severe (중증)";
                            str09 = str09 + ComNum.VBLF + " - 증상이 일이나 일상의 활동을 수행할 수  없을 만큼 불편감을 야기함";
                            str09 = str09 + ComNum.VBLF + " - 의심약물을 중단할 만큼 불편감이 있음";
                            str09 = str09 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요함";
                            break;
                        case "serious":
                            str09 = " - serious (중대함)";
                            str09 = str09 + ComNum.VBLF + " - 사망을 초래하거나 생명의 위협을 연장하는 경우";
                            str09 = str09 + ComNum.VBLF + " - 입원 또는 입원기간의 연장이 필요한 경우";
                            str09 = str09 + ComNum.VBLF + " - 지속적 또는 중대한 불구나 기능저하를 초래하는 경우";
                            str09 = str09 + ComNum.VBLF + " - 선천적 기형 또는 이상을 초래하는 경우";
                            str09 = str09 + ComNum.VBLF + " - 기타 의학적으로 중요한 상황";
                            break;
                    }

                    str10 = dt.Rows[0]["CCLASS"].ToString().Trim();

                    switch (str10)
                    {
                        case "mild":
                            str10 = " - mild (경증)";
                            str10 = str10 + ComNum.VBLF + " - 증상 또는 징후를 자각할 수 있으나 불편감을 주지 않고 참을 수 있음";
                            str10 = str10 + ComNum.VBLF + " - 행동이나 기능에 영향을 미치치 않음";
                            str10 = str10 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요하지 않음";
                            break;
                        case "moderate":
                            str10 = " - moderate (중등증)";
                            str10 = str10 + ComNum.VBLF + " - 증상이 일상의 활동을 방해할 만큼 불편함";
                            str10 = str10 + ComNum.VBLF + " - 행동에 영향을 미침";
                            str10 = str10 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요할 수 있음";
                            break;
                        case "severe":
                            str10 = " - severe (중증)";
                            str10 = str10 + ComNum.VBLF + " - 증상이 일이나 일상의 활동을 수행할 수  없을 만큼 불편감을 야기함";
                            str10 = str10 + ComNum.VBLF + " - 의심약물을 중단할 만큼 불편감이 있음";
                            str10 = str10 + ComNum.VBLF + " - 증상을 경감하기 위한 치료가 필요함";
                            break;
                        case "serious":
                            str10 = " - serious (중대함)";
                            str10 = str10 + ComNum.VBLF + " - 사망을 초래하거나 생명의 위협을 연장하는 경우";
                            str10 = str10 + ComNum.VBLF + " - 입원 또는 입원기간의 연장이 필요한 경우";
                            str10 = str10 + ComNum.VBLF + " - 지속적 또는 중대한 불구나 기능저하를 초래하는 경우";
                            str10 = str10 + ComNum.VBLF + " - 선천적 기형 또는 이상을 초래하는 경우";
                            str10 = str10 + ComNum.VBLF + " - 기타 의학적으로 중요한 상황";
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                ssPage2_Sheet1.Cells[3, 7].Text = str01;

                ssPage2_Sheet1.Cells[4, 7].Text = str02;

                ssPage2_Sheet1.Cells[6, 7].Text = str03;

                ssPage2_Sheet1.Cells[7, 7].Text = str04;

                ssPage2_Sheet1.Cells[8, 7].Text = str05;

                ssPage2_Sheet1.Cells[16, 7].Text = str06;
                ssPage2_Sheet1.Cells[17, 7].Text = str07;
                ssPage2_Sheet1.Cells[18, 7].Text = str08;
                ssPage2_Sheet1.Cells[20, 7].Text = str09;
                ssPage2_Sheet1.Cells[21, 7].Text = str10;
                ssPage2_Sheet1.Cells[23, 7].Text = str11;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUCODE, SUNAMEK, QTY, DOSNAME, DECODE(CHECK1, '1', '유', '') || DECODE(CHECK2, '1', '무', '') || DECODE(CHECK3, '1', '모름', '') AS USED";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2_ORDER";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "         AND ROWNUM < 5 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPage2_Sheet1.Cells[i + 11, 2].Text = (i + 1).ToString();
                        ssPage2_Sheet1.Cells[i + 11, 4].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssPage2_Sheet1.Cells[i + 11, 7].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssPage2_Sheet1.Cells[i + 11, 16].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssPage2_Sheet1.Cells[i + 11, 19].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ssPage2_Sheet1.Cells[i + 11, 23].Text = dt.Rows[i]["USED"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                ssPage2_Sheet1.Cells[23, 7].Text = "  No  ";

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR2";
                SQL = SQL + ComNum.VBLF + "     WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "Union All";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR3";
                SQL = SQL + ComNum.VBLF + "     WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPage2_Sheet1.Cells[23, 7].Text = "  Yes  ";
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPage1_Click(object sender, EventArgs e)
        {
            ssPage1.Visible = true;
            ssPage1.Dock = DockStyle.Fill;

            ssPage2.Visible = false;
            ssPage2.Dock = DockStyle.None;
        }

        private void btnPage2_Click(object sender, EventArgs e)
        {
            ssPage1.Visible = false;
            ssPage1.Dock = DockStyle.None;

            ssPage2.Visible = true;
            ssPage2.Dock = DockStyle.Fill;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            
            ssPage1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPage1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPage1_Sheet1.PrintInfo.Margin.Top = 20;
            ssPage1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPage1_Sheet1.PrintInfo.Margin.Header = 10;
            ssPage1_Sheet1.PrintInfo.ShowColor = false;
            ssPage1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPage1_Sheet1.PrintInfo.ShowBorder = false;
            ssPage1_Sheet1.PrintInfo.ShowGrid = false;
            ssPage1_Sheet1.PrintInfo.ShowShadows = false;
            ssPage1_Sheet1.PrintInfo.UseMax = true;
            ssPage1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPage1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPage1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPage1_Sheet1.PrintInfo.Preview = false;
            ssPage1.PrintSheet(0);

            ssPage2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPage2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPage2_Sheet1.PrintInfo.Margin.Top = 20;
            ssPage2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPage2_Sheet1.PrintInfo.Margin.Header = 10;
            ssPage2_Sheet1.PrintInfo.ShowColor = false;
            ssPage2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPage2_Sheet1.PrintInfo.ShowBorder = false;
            ssPage2_Sheet1.PrintInfo.ShowGrid = false;
            ssPage2_Sheet1.PrintInfo.ShowShadows = false;
            ssPage2_Sheet1.PrintInfo.UseMax = true;
            ssPage2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPage2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPage2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPage2_Sheet1.PrintInfo.Preview = false;
            ssPage2.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
