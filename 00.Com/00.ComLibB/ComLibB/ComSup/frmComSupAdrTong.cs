using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupAdrTong.cs
    /// Description     : 약물이상반응(ADR) 모니터링 통계
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 모니터링 통계
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADRTong.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupAdrTong : Form
    {
        public frmComSupAdrTong()
        {
            InitializeComponent();
        }

        private void frmComSupAdrTong_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddMonths(-1);
            dtpEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            CLEAR_SS();
        }

        private void CLEAR_SS()
        {
            ssView_Sheet1.Cells[5, 5].Text = "";
            ssView_Sheet1.Cells[6, 5].Text = "";
            ssView_Sheet1.Cells[7, 5].Text = "";
            ssView_Sheet1.Cells[8, 5].Text = "";
            ssView_Sheet1.Cells[9, 5].Text = "";
            ssView_Sheet1.Cells[10, 5].Text = "";
            ssView_Sheet1.Cells[11, 5].Text = "";
            ssView_Sheet1.Cells[15, 5].Text = "";

            ssView_Sheet1.Cells[19, 7].Text = "";
            ssView_Sheet1.Cells[20, 7].Text = "";
            ssView_Sheet1.Cells[22, 7].Text = "";
            ssView_Sheet1.Cells[23, 7].Text = "";
            ssView_Sheet1.Cells[24, 7].Text = "";
            ssView_Sheet1.Cells[25, 7].Text = "";
            ssView_Sheet1.Cells[26, 7].Text = "";

            ssView_Sheet1.Cells[27, 7].Text = "";
            ssView_Sheet1.Cells[28, 7].Text = "";
            ssView_Sheet1.Cells[29, 7].Text = "";
            ssView_Sheet1.Cells[30, 7].Text = "";

            ssView_Sheet1.Cells[31, 7].Text = "";
            ssView_Sheet1.Cells[32, 7].Text = "";
            ssView_Sheet1.Cells[33, 7].Text = "";
            ssView_Sheet1.Cells[34, 7].Text = "";

            ssView_Sheet1.Cells[35, 5].Text = "";

            ssView_Sheet1.Cells[36, 10].Text = "";

            ssView_Sheet1.Cells[37, 7].Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            
            CLEAR_SS();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.PATIENT_BUN, '입원', 1, 0)) AS IPD,";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.PATIENT_BUN, '외래', 1, 0)) AS OPD,";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.PATIENT_BUN, '입원', 0, DECODE(A.PATIENT_BUN, '외래', 0, 1))) AS ETC,";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(DECODE(A.RESULTDATE,NULL, 0, 1) + DECODE(A.RESULTTIME, NULL, 0, 1) + DECODE(A.RESULTSEC, NULL, 0, 1), 0, 0, 1)) AS REC0,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER1) AS REC1,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER4) AS REC2,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER2) AS REC3,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER3) AS REC4,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER5) AS REC5,";
                SQL = SQL + ComNum.VBLF + "     SUM(A.RECOVER6) AS REC6,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.RELATION1) AS REL1,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.RELATION2) AS REL2,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.RELATION3) AS REL3,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.RELATION4) AS REL4,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.CLASS1) AS CLS1,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.CLASS2) AS CLS2,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.CLASS3) AS CLS3,";
                SQL = SQL + ComNum.VBLF + "     SUM(C.CLASS4) AS CLS4,";
                SQL = SQL + ComNum.VBLF + "     SUM(B.REPORT1) AS REP1,";
                SQL = SQL + ComNum.VBLF + "     SUM(B.REPORT2) AS REP2, ";
                SQL = SQL + ComNum.VBLF + "     SUM(1) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR1 A, " + ComNum.DB_ERP + "DRUG_ADR2 B, " + ComNum.DB_ERP + "DRUG_ADR3 C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.SEQNO = B.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.WDATE >= TO_DATE('" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.WDATE <= TO_DATE('" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";

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
                    ssView_Sheet1.Cells[5, 5].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                    ssView_Sheet1.Cells[6, 5].Text = dtpSDATE.Value.ToString("yyyy-MM-dd") + " ~ " + dtpEDATE.Value.ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[7, 5].Text = dt.Rows[0]["CNT"].ToString().Trim();
                    ssView_Sheet1.Cells[8, 5].Text = dt.Rows[0]["IPD"].ToString().Trim() == "0" ? "" : dt.Rows[0]["IPD"].ToString().Trim();
                    ssView_Sheet1.Cells[9, 5].Text = dt.Rows[0]["OPD"].ToString().Trim() == "0" ? "" : dt.Rows[0]["OPD"].ToString().Trim();
                    ssView_Sheet1.Cells[10, 5].Text = dt.Rows[0]["ETC"].ToString().Trim() == "0" ? "" : dt.Rows[0]["ETC"].ToString().Trim();
                    ssView_Sheet1.Cells[11, 5].Text = READ_DRBUN(dtpSDATE.Value.ToString("yyyy-MM-dd"), dtpEDATE.Value.ToString("yyyy-MM-dd"));
                    ssView_Sheet1.Cells[15, 5].Text = READ_RACT(dtpSDATE.Value.ToString("yyyy-MM-dd"), dtpEDATE.Value.ToString("yyyy-MM-dd"));

                    ssView_Sheet1.Cells[19, 7].Text = dt.Rows[0]["REC0"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC0"].ToString().Trim();
                    ssView_Sheet1.Cells[20, 7].Text = dt.Rows[0]["REC1"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC1"].ToString().Trim();
                    ssView_Sheet1.Cells[22, 7].Text = dt.Rows[0]["REC2"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC2"].ToString().Trim();
                    ssView_Sheet1.Cells[23, 7].Text = dt.Rows[0]["REC3"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC3"].ToString().Trim();
                    ssView_Sheet1.Cells[24, 7].Text = dt.Rows[0]["REC4"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC4"].ToString().Trim();
                    ssView_Sheet1.Cells[25, 7].Text = dt.Rows[0]["REC5"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC5"].ToString().Trim();
                    ssView_Sheet1.Cells[26, 7].Text = dt.Rows[0]["REC6"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REC6"].ToString().Trim();

                    ssView_Sheet1.Cells[27, 7].Text = dt.Rows[0]["REL1"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REL1"].ToString().Trim();
                    ssView_Sheet1.Cells[28, 7].Text = dt.Rows[0]["REL2"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REL2"].ToString().Trim();
                    ssView_Sheet1.Cells[29, 7].Text = dt.Rows[0]["REL3"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REL3"].ToString().Trim();
                    ssView_Sheet1.Cells[30, 7].Text = dt.Rows[0]["REL4"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REL4"].ToString().Trim();

                    ssView_Sheet1.Cells[31, 7].Text = dt.Rows[0]["CLS1"].ToString().Trim() == "0" ? "" : dt.Rows[0]["CLS1"].ToString().Trim();
                    ssView_Sheet1.Cells[32, 7].Text = dt.Rows[0]["CLS2"].ToString().Trim() == "0" ? "" : dt.Rows[0]["CLS2"].ToString().Trim();
                    ssView_Sheet1.Cells[33, 7].Text = dt.Rows[0]["CLS3"].ToString().Trim() == "0" ? "" : dt.Rows[0]["CLS3"].ToString().Trim();
                    ssView_Sheet1.Cells[34, 7].Text = dt.Rows[0]["CLS4"].ToString().Trim() == "0" ? "" : dt.Rows[0]["CLS4"].ToString().Trim();

                    ssView_Sheet1.Cells[35, 5].Text = READ_ALERT(dtpSDATE.Value.ToString("yyyy-MM-dd"), dtpEDATE.Value.ToString("yyyy-MM-dd"));
                    if (ssView_Sheet1.Cells[35, 5].Text.Trim() == "0") { ssView_Sheet1.Cells[35, 5].Text = ""; }

                    ssView_Sheet1.Cells[36, 10].Text = dt.Rows[0]["REP1"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REP1"].ToString().Trim();
                    ssView_Sheet1.Cells[37, 7].Text = dt.Rows[0]["REP2"].ToString().Trim() == "0" ? "" : dt.Rows[0]["REP2"].ToString().Trim();
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

        private string READ_DRBUN(string strSDATE, string strEDATE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     NVL(A.DRBUN, '분류없음') AS DRBUN, SUM(1) AS CNT";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         SEQNO, DRBUN";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER";
                SQL = SQL + ComNum.VBLF + "     UNION ALL";
                SQL = SQL + ComNum.VBLF + "     SELECT";
                SQL = SQL + ComNum.VBLF + "         SEQNO, '조영제' DRBUN";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_ERP + "DRUG_ADR1_ORDER_JO) A, ";
                SQL = SQL + ComNum.VBLF + "     " + ComNum.DB_ERP + "DRUG_ADR1 B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + "     AND B.WDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND B.WDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY DRBUN";
                SQL = SQL + ComNum.VBLF + "ORDER BY CNT DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0) { rtnVal = rtnVal + ComNum.VBLF; }
                        rtnVal = rtnVal + VB.Left(dt.Rows[i]["DRBUN"].ToString().Trim() + VB.Space(50), 25) + " " + dt.Rows[i]["CNT"].ToString().Trim() + "건";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        private string READ_RACT(string strSDATE, string strEDATE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     RACT, SUM(1) AS CNT";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SELECT BDATE, DECODE(RACT_A1, '1', '발열', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_A2, '1', '식욕감소', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_A3, '1', '전신부종', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_A4, '1', '전신쇠약', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_A5, '1', '체중감소', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_A6, '1', '체중증가', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B1, '1', '가려움증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B2, '1', '가려운 발진', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B3, '1', '농포성 발진', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B4, '1', '두드러기', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B5, '1', '여드름성 발진', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B6, '1', '피부작리', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B7, '1', '피부변색', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B8, '1', '혈관부종', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_B9, '1', '탈모', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C1, '1', '구강칸디다증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C2, '1', '구강건조증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C3, '1', '귀울림', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C4, '1', '급성청각이상', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C5, '1', '미각이상', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C6, '1', '시각장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C7, '1', '안압상승', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_C8, '1', '음성변화', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D1, '1', '가슴불편함', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D2, '1', '부정맥', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D3, '1', '빈맥', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D4, '1', '실신', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D5, '1', '심부전', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D6, '1', '저혈압', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_D7, '1', '고혈압', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E1, '1', '오심/구토', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E2, '1', '변비', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E3, '1', '복통', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E4, '1', '설사', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E5, '1', '소화불량', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E6, '1', '위장관통증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_E7, '1', '위출혈', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_F1, '1', '빌리루빈증가', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_F2, '1', 'AST/ALT 증가', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_G1, '1', '기침', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_G2, '1', '호흡곤란', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_G3, '1', '폐부종', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_H1, '1', '백혈구감소증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_H2, '1', '빈혈', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_H3, '1', '응고장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_H4, '1', '혈소판감소증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_I1, '1', '단백뇨', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_I2, '1', '신기능장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_I3, '1', '혈뇨', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_I4, '1', '혈중 Creatinine 증가', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J1, '1', '기억력장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J2, '1', '두통', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J3, '1', '보행곤란', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J4, '1', '사지떨림', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J5, '1', '수면장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J6, '1', '어지러움', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J7, '1', '언어장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J8, '1', '의식저하', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J9, '1', '운동이상증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J10, '1', '졸림', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J11, '1', '피부저림', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J12, '1', '불안', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J13, '1', '섬망', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J14, '1', '신경과민', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J15, '1', '우울', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_J16, '1', '과행동', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K1, '1', '고혈당증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K2, '1', '저혈당증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K3, '1', '배뇨장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K4, '1', '성기능장애', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K5, '1', '성욕감소', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K6, '1', '여성형유방', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_K7, '1', '월경불순', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_L1, '1', '관절통', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_L2, '1', '골다공증', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "         UNION ALL SELECT BDATE, DECODE(RACT_L3, '1', '근육통', '') AS RACT FROM " + ComNum.DB_ERP + "DRUG_ADR1";
                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "  Where RACT Is Not Null";
                SQL = SQL + ComNum.VBLF + "     AND BDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND BDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "GROUP BY RACT";
                SQL = SQL + ComNum.VBLF + "ORDER BY CNT DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0) { rtnVal = rtnVal + ComNum.VBLF; }
                        rtnVal = rtnVal + VB.Left(dt.Rows[i]["RACT"].ToString().Trim() + VB.Space(30), 25) + " " + dt.Rows[i]["CNT"].ToString().Trim() + "건";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        private string READ_ALERT(string strSDATE, string strEDATE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "             A.SEQNO";
                SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_ERP + "DRUG_ADR2 A, " + ComNum.DB_ERP + "DRUG_ADR1 B";
                SQL = SQL + ComNum.VBLF + "             WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "                 AND A.SEQNO = B.SEQNO ";
                SQL = SQL + ComNum.VBLF + "                 AND B.BDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                 AND B.BDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         Union All";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "             A.SEQNO";
                SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_ERP + "DRUG_ADR3 A, " + ComNum.DB_ERP + "DRUG_ADR1 B";
                SQL = SQL + ComNum.VBLF + "             WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')";
                SQL = SQL + ComNum.VBLF + "                 AND A.SEQNO = B.SEQNO ";
                SQL = SQL + ComNum.VBLF + "                 AND B.BDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                 AND B.BDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "GROUP BY SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows.Count.ToString();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
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
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
