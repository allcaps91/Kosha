using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseVitalSign
    /// Description     : 통합 Vital Sign 조회
    /// Author          : 전상원
    /// Create Date     : 2018-05-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= \mtsEmr\EMRWARD\MHEMRWARD.vbp(FrmVitalSign)" >> frmEmrBaseVitalSign.cs 폼이름 재정의" />
    public partial class frmEmrBaseVitalSign : Form
    {
        public frmEmrBaseVitalSign()
        {
            InitializeComponent();
        }

        private void frmEmrBaseVitalSign_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            ComFunc CF = new ComFunc();

            btnAccept.Visible = false;

            if (clsPublic.GstrHelpCode == "수혈기록지")
            {
                dtpSDATE.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, -2));
                btnAccept.Visible = true;
            }
            else
            {
                dtpSDATE.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, -15));
            }

            dtpEDATE.Value = Convert.ToDateTime(strSysDate);

            txtPtno.Text = "";
            txtPtno.Text = clsPublic.GstrHelpCode;
            lblSname.Text = READ_PatientName(clsPublic.GstrHelpCode);

            READ_VITAL(VB.Trim(txtPtno.Text), dtpSDATE.Text, dtpEDATE.Text);
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            READ_VITAL(VB.Trim(txtPtno.Text), dtpSDATE.Text, dtpEDATE.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strVITAL = "";
            string strTEMP = "";

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    strVITAL = ssView_Sheet1.Cells[i, 4].Text;
                    strVITAL = strVITAL + "/" + ssView_Sheet1.Cells[i, 5].Text;
                    strVITAL = strVITAL + "-" + ssView_Sheet1.Cells[i, 6].Text;
                    strVITAL = strVITAL + "-" + ssView_Sheet1.Cells[i, 7].Text;
                    strVITAL = strVITAL + "-" + ssView_Sheet1.Cells[i, 8].Text;
                    strVITAL = strVITAL + ssView_Sheet1.Cells[i, 2].Text;
                    strTEMP = strTEMP + strVITAL + "|";
                }
            }

            if (strTEMP != "")
            {
                strTEMP = VB.Mid(strTEMP, 1, VB.Len(strTEMP) - 1);
                clsPublic.GstrHelpCode = strTEMP;
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtno.Text = VB.Val(txtPtno.Text).ToString("00000000");
                lblSname.Text = READ_PatientName(txtPtno.Text);
                GetData();
            }
        }

        private void READ_VITAL(string argPTNO, string argSDATE, string argEDATE)
        {
            int i = 0;
            int nRead = 0;
            string strA = "";
            string strB = "";
            string strC = "";
            string strD = "";
            string strE = "";
            string strF = "";
            string strG = "";
            string strH = "";
            string strI = "";
            string strGUBUN = "";
            string strChartDate = "";
            string strChartTime = "";
            string strUseId = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            argSDATE = argSDATE.Replace("-", "");
            argEDATE = argEDATE.Replace("-", "");

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3150");

            Cursor.Current = Cursors.WaitCursor;

            if (panCheck.Controls.OfType<CheckBox>().Where(d => d.Checked).Count() == 0)
            {
                ComFunc.MsgBoxEx(this, "1개 항목이라도 선택 해야 조회 가능합니다.");
                return;
            }
            try
            {

                #region RRS Score
                //int Score = 0;
                //string Warring = string.Empty;
                //Color WarringCol = Color.Green;
                //FormPatInfoFunc.Set_FormPatInfo_RRSSCORE(clsDB.DbCon, p, ref Score, ref Warring, ref WarringCol);
                //lblRRS.BackColor = WarringCol;
                //lblRRS.Text = "RRS(" + Score + "점/" + Warring + ")";
                //lblRRS.Visible = clsEmrFunc.GetMenuAuth(this, clsDB.DbCon);
                #endregion

                SQL = "";
                SQL = " SELECT A, B, C, D, E, F, G, H, I, GUBUN, CHARTDATE, CHARTTIME, USEID";
                SQL = SQL + ComNum.VBLF + " FROM (";
                #region 이전 기록지(XML)

                if (chkVital1.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//ta2') A, extractValue(chartxml, '//ta3') B, extractValue(chartxml, '//ta7') C, extractValue(chartxml, '//ta8') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//ta9') E, extractValue(chartxml, '//ta10') F, extractValue(chartxml, '//ta11') G, 'ICU Vital2'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//ta34') AS H, extractValue(chartxml, '//ta36') AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 2599";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";

                    SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it3') A, extractValue(chartxml, '//it4') B, extractValue(chartxml, '//it6') C, extractValue(chartxml, '//it7') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it7') E, extractValue(chartxml, '//it8') F, extractValue(chartxml, '//it14') G, '활력측정표'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it10') AS H, extractValue(chartxml, '//it12') AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 1562";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }

                if (chkVital3.Checked)
                {
                    if (chkVital2.Checked == true || chkVital1.Checked == true || chkVital4.Checked == true &&
                         chkVital5.Checked == true || chkVital6.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    }

                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it1') A, extractValue(chartxml, '//it2') B, extractValue(chartxml, '//it3') C, extractValue(chartxml, '//it4') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it4') E, extractValue(chartxml, '//it5') F, extractValue(chartxml, '//it6') G, '회복실 Vital Sheet'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            '' AS H, '' AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 2135";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }

                if (chkVital4.Checked)
                {
                    if (chkVital2.Checked == true || chkVital1.Checked == true || chkVital3.Checked == true &&
                        chkVital5.Checked == true || chkVital6.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    }

                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it2') A, extractValue(chartxml, '//it3') B, extractValue(chartxml, '//it5') C, extractValue(chartxml, '//it6') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it6') E, extractValue(chartxml, '//it7') F, extractValue(chartxml, '//it9') G, '환자모니터 및 진정 환자 평가'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            '' AS H, '' AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 2431";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }

                if (chkVital5.Checked)
                {
                    if (chkVital1.Checked == true || chkVital2.Checked == true || chkVital3.Checked == true &&
                        chkVital4.Checked == true || chkVital6.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    }

                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it7') A, extractValue(chartxml, '//it8') B, extractValue(chartxml, '//it11') C, extractValue(chartxml, '//it12') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it13') E, extractValue(chartxml, '//it14') F, extractValue(chartxml, '//it16') G, 'SPECIAL WATCH RECORD'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            '' AS H, '' AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 1969";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }

                if (chkVital6.Checked)
                {
                    if (chkVital1.Checked == true || chkVital2.Checked == true || chkVital3.Checked == true &&
                        chkVital4.Checked == true || chkVital5.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    }
                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it3') A, extractValue(chartxml, '//it4') B, extractValue(chartxml, '//it14') C, extractValue(chartxml, '//it7') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it8') E, '' F, extractValue(chartxml, '//it2') G, '활력측정표(조영실)'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            '' AS H, '' AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 1935";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }
     
                if (chkVital2.Checked)
                {
                    if (chkVital1.Checked == true || chkVital6.Checked == true || chkVital3.Checked == true &&
                        chkVital4.Checked == true || chkVital5.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    }
                    SQL = SQL + ComNum.VBLF + "     SELECT extractValue(chartxml, '//it3') A, extractValue(chartxml, '//it4') B, extractValue(chartxml, '//it6') C, extractValue(chartxml, '//it7') D, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it7') E, extractValue(chartxml, '//it8') F, extractValue(chartxml, '//it14') G, '인공신장실 V/S Sheet'  GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            extractValue(chartxml, '//it10') AS H, '' AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                    SQL = SQL + ComNum.VBLF + "     WHERE B.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND B.FORMNO = 2201";
                    SQL = SQL + ComNum.VBLF + "       AND A.EMRNO = B.EMRNO";
                }
                #endregion

                #region 신규 기록지
                if (pForm.FmOLDGB != 1)
                {
                    #region 바이탈 폼 번호 저장
                    string strFormNo = string.Empty;
                    if (chkVital1.Checked)
                    {
                        strFormNo += "1562 ";
                        strFormNo += "3150 ";
                    }

                    if (chkVital2.Checked)
                    {
                        strFormNo += "2201 ";
                    }

                    if (chkVital3.Checked)
                    {
                        strFormNo += "2135 ";
                    }

                    if (chkVital4.Checked)
                    {
                        strFormNo += "2431 ";
                    }

                    if (chkVital5.Checked)
                    {
                        strFormNo += "1969 ";
                    }

                    if (chkVital6.Checked)
                    {
                        strFormNo += "1935 ";
                    }
                    #endregion

                    strFormNo = strFormNo.Replace(" ", ",");
                    strFormNo = VB.Left(strFormNo, strFormNo.Length - 1);
                    #endregion

                    SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    SQL = SQL + ComNum.VBLF + "     SELECT B1.ITEMVALUE AS A, B2.ITEMVALUE AS B, B3.ITEMVALUE AS C, B4.ITEMVALUE AS D, ";
                    SQL = SQL + ComNum.VBLF + "            B4.ITEMVALUE AS E, B5.ITEMVALUE AS F, B6.ITEMVALUE AS G, F.FORMNAME AS GUBUN, ";
                    SQL = SQL + ComNum.VBLF + "            B7.ITEMVALUE AS H, B8.ITEMVALUE AS I, ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, A.CHARTUSEID AS USEID";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "       INNER JOIN ADMIN.AEMRFORM F";
                    SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = F.FORMNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPDATENO = F.UPDATENO";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B1";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B1.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B1.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B1.ITEMCD IN ('I0000002018') --SBP";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B2";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B2.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B2.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B2.ITEMCD IN ('I0000001765') --DBP";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B3";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B3.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B3.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B3.ITEMCD IN ('I0000014815') --PR";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B4";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B4.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B4.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B4.ITEMCD IN ('I0000002009') --RR";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B5";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B5.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B5.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B5.ITEMCD IN ('I0000001811') --BT(℃)";
                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B6";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B6.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B6.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B6.ITEMCD IN ('I0000008708') --SpO2 (%)";

                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B7";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B7.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B7.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B7.ITEMCD IN ('I0000000418') --체중";

                    SQL = SQL + ComNum.VBLF + "        LEFT OUTER JOIN ADMIN.AEMRCHARTROW B8";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B8.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B8.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B8.ITEMCD IN ('I0000018853') --복위";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.FORMNO IN( " + strFormNo + " )";
                }

                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "WHERE (A > CHR(0) OR B > CHR(0) OR ";
                SQL = SQL + ComNum.VBLF + "       C > CHR(0) OR D > CHR(0) OR ";
                SQL = SQL + ComNum.VBLF + "       E > CHR(0) OR F > CHR(0) OR ";
                SQL = SQL + ComNum.VBLF + "       G > CHR(0) OR H > CHR(0) OR ";
                SQL = SQL + ComNum.VBLF + "       I > CHR(0))";
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strA = dt.Rows[i]["A"].ToString().Trim();
                        strB = dt.Rows[i]["B"].ToString().Trim();
                        strC = dt.Rows[i]["C"].ToString().Trim();
                        strD = dt.Rows[i]["D"].ToString().Trim();
                        strE = dt.Rows[i]["E"].ToString().Trim();
                        strF = dt.Rows[i]["F"].ToString().Trim();
                        strG = dt.Rows[i]["G"].ToString().Trim();
                        strH = dt.Rows[i]["H"].ToString().Trim();
                        strI = dt.Rows[i]["I"].ToString().Trim();
                        strGUBUN = dt.Rows[i]["GUBUN"].ToString().Trim();
                        strChartDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        strChartTime = dt.Rows[i]["CHARTTIME"].ToString().Trim();
                        strUseId = dt.Rows[i]["USEID"].ToString().Trim();



                        ssView_Sheet1.Cells[i, 1].Text = VB.Left(strChartDate, 4) + "-" + VB.Mid(strChartDate, 5, 2) + "-" + VB.Right(strChartDate, 2);
                        ssView_Sheet1.Cells[i, 2].Text = VB.Left(strChartTime, 2) + ":" + VB.Mid(strChartTime, 3, 2);
                        ssView_Sheet1.Cells[i, 4].Text = strA;
                        ssView_Sheet1.Cells[i, 5].Text = strB;
                        ssView_Sheet1.Cells[i, 6].Text = strC;

                        if (strD != "")
                        {
                            ssView_Sheet1.Cells[i, 7].Text = strD;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 7].Text = strE;
                        }
                        ssView_Sheet1.Cells[i, 8].Text = strF;
                        ssView_Sheet1.Cells[i, 9].Text = strG;
                        ssView_Sheet1.Cells[i, 11].Text = strH;
                        ssView_Sheet1.Cells[i, 12].Text = strI;
                        ssView_Sheet1.Cells[i, 13].Text = strGUBUN;
                        ssView_Sheet1.Cells[i, 14].Text = clsVbfunc.GetInSaName(clsDB.DbCon, VB.Val(strUseId).ToString("00000"));


                        if (i > 0 && i % 5 == 0)
                        {
                            DRAW_LINE_COLOR(i);
                        }

                        double rtnVal = VB.Val(strA);
                        if (rtnVal >= 140 || rtnVal < 80)  //혈압 it3
                        {
                            ssView_Sheet1.Cells[i, 4].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(strB);
                        if (rtnVal >= 90 || rtnVal < 60) //혈압 it4
                        {
                            ssView_Sheet1.Cells[i, 5].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(strC);
                        if (rtnVal >= 100 || rtnVal < 60) //맥박 it6
                        {
                            ssView_Sheet1.Cells[i, 6].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(strD);
                        if (rtnVal >= 21 || rtnVal < 12)  //호흡 it7
                        {
                            ssView_Sheet1.Cells[i, 7].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(strF);
                        if (rtnVal >= 37.5 || rtnVal < 36.5) //체온 it8
                        {
                            ssView_Sheet1.Cells[i, 8].ForeColor = Color.Red;
                        }


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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_PatientName(string ArgPano)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (VB.Val(ArgPano) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SName FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano='" + ArgPano + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void DRAW_LINE_COLOR(int argRow)
        {
            //ssView_Sheet1.Cells[argRow - 1, 1, argRow - 1, 2].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 1, argRow - 1, 2].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 4, argRow - 1, 5].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 4, argRow - 1, 5].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 6].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 6].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 7].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 7].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 8].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 8].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 9].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 9].BackColor = Color.FromArgb(239, 239, 239);

            //ssView_Sheet1.Cells[argRow - 1, 11, argRow - 1, 14].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[argRow - 1, 11, argRow - 1, 14].BackColor = Color.FromArgb(239, 239, 239);
        }
    }
}
