using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class FrmVital_D : Form
    {
        string mstrPano = string.Empty;
        EmrPatient AcpEmr = null;

        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        public FrmVital_D()
        {
            InitializeComponent();
        }


        public FrmVital_D(string Pano)
        {
            mstrPano = Pano;
            InitializeComponent();
        }

        public FrmVital_D(string Pano, EmrPatient emrPatient)
        {
            mstrPano = Pano;
            AcpEmr = emrPatient;
            InitializeComponent();
        }

        private void FrmVital_D_Load(object sender, EventArgs e)
        {
            SSPatientInfo_Sheet1.Cells[0, 0].Text = mstrPano;
            SSPatientInfo_Sheet1.Cells[0, 1].Text = clsVbfunc.GetPatientName(clsDB.DbCon, mstrPano);

            dtpEDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            if (AcpEmr != null && AcpEmr.medFrDate.NotEmpty() && clsEmrPublic.gUserGrade.Equals("SIMSA") && clsEmrPublic.gDateSET == true)
            {
                dtpSDATE.Value = DateTime.ParseExact(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, AcpEmr.inOutCls, AcpEmr.medDeptCd, clsType.User.IdNumber, AcpEmr.medFrDate).Replace("-", ""), "yyyyMMdd", null);
            }
            else
            {
                dtpSDATE.Value = dtpEDATE.Value.AddDays(-5);
            }
            

            btnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");

            btnSearch.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            READ_VITAL();
        }


        void READ_VITAL()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssVital_Sheet1.RowCount = 0;
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3150");
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region XML
                SQL = " SELECT CHARTDATE, CHARTTIME,";
                SQL += ComNum.VBLF + "   extractValue(chartxml, '//it1') AS IT1,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it2') AS IT2,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it3') AS IT3,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it4') AS IT4,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it5') AS IT5,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it6') AS IT6,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it7') AS IT7,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it8') AS IT8,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it9') AS IT9,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it10') AS IT10,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it11') AS IT11,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it12') AS IT12,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it13') AS IT13,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it14') AS IT14,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it121') AS IT121,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it150') AS IT150,";
                SQL += ComNum.VBLF + "              extractValue(chartxml, '//it274') AS IT274";
                SQL += ComNum.VBLF + "    From ADMIN.EMRXML";
                SQL += ComNum.VBLF + " WHERE EMRNO IN (";
                SQL += ComNum.VBLF + "  SELECT EMRNO FROM ADMIN.EMRXMLMST WHERE FORMNO = 1562";
                SQL += ComNum.VBLF + "      AND PTNO = '" + mstrPano + "'";
                SQL += ComNum.VBLF + "      AND CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "')";
                #endregion

                #region 신규
                if (pForm.FmOLDGB != 1)
                {
                    #region 신규
                    SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    SQL = SQL + ComNum.VBLF + "     SELECT ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, ";
                    SQL = SQL + ComNum.VBLF + "            '' AS IT1, B0.ITEMVALUE AS IT2,   B1.ITEMVALUE AS IT3,   B2.ITEMVALUE AS IT4,";
                    SQL = SQL + ComNum.VBLF + "            B00.ITEMVALUE AS IT5,  B4.ITEMVALUE AS IT6,   B41.ITEMVALUE AS IT7,";
                    SQL = SQL + ComNum.VBLF + "            B5.ITEMVALUE AS IT8,   B51.ITEMVALUE AS IT9,  B8.ITEMVALUE AS IT10,";
                    SQL = SQL + ComNum.VBLF + "            B9.ITEMVALUE AS IT11,  B7.ITEMVALUE AS IT12,  B71.ITEMVALUE AS IT13,";
                    SQL = SQL + ComNum.VBLF + "            B6.ITEMVALUE AS IT14,  B10.ITEMVALUE AS IT121,  B11.ITEMVALUE AS IT150,";
                    SQL = SQL + ComNum.VBLF + "            B12.ITEMVALUE AS IT274";
                    SQL = SQL + ComNum.VBLF + "     FROM ADMIN.AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "       INNER JOIN ADMIN.AEMRFORM F";
                    SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = F.FORMNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPDATENO = F.UPDATENO";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B0";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B0.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B0.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B0.ITEMCD IN ('I0000024733') --구분";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B1";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B1.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B1.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B1.ITEMCD IN ('I0000002018') --SBP";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B2";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B2.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B2.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B2.ITEMCD IN ('I0000001765') --DBP";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B00";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B00.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B00.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B00.ITEMCD IN ('I0000037575') --혈압측정위치";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B4";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B4.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B4.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B4.ITEMCD IN ('I0000014815') --PR";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B41";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B41.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B41.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B41.ITEMCD IN ('I0000002009') --RR";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B5";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B5.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B5.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B5.ITEMCD IN ('I0000001811') --BT(℃)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B51";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B51.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B51.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B51.ITEMCD IN ('I0000035464') --체온측정위치";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B6";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B6.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B6.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B6.ITEMCD IN ('I0000008708') --SpO2 (%)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B7";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B7.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B7.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B7.ITEMCD IN ('I0000018853') --배둘레(복위)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B8";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B8.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B8.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B8.ITEMCD IN ('I0000000418') --체중(BWT)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B9";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B9.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B9.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B9.ITEMCD IN ('I0000000002') --신장";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B71";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B71.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B71.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B71.ITEMCD IN ('I0000029454') --FHR";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B10";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B10.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B10.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B10.ITEMCD IN ('I0000017712') --머리둘레";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B11";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B11.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B11.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B11.ITEMCD IN ('I0000010747') --가슴둘레";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN ADMIN.AEMRCHARTROW B12";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B12.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B12.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B12.ITEMCD IN ('I0000037854') --비고";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + mstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE >= '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE <= '" + dtpEDATE.Value.ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.FORMNO IN(1562, 3150)";
                    SQL = SQL + ComNum.VBLF + "		  AND (B0.ITEMVALUE  > CHR(0)  OR B1.ITEMVALUE  > CHR(0) OR  B2.ITEMVALUE  > CHR(0) OR       ";
                    SQL = SQL + ComNum.VBLF + "           B00.ITEMVALUE  > CHR(0)  OR B4.ITEMVALUE	> CHR(0) OR  B41.ITEMVALUE > CHR(0) OR  ";
                    SQL = SQL + ComNum.VBLF + "           B5.ITEMVALUE   > CHR(0)  OR B51.ITEMVALUE > CHR(0) OR  B8.ITEMVALUE  > CHR(0) OR    ";
                    SQL = SQL + ComNum.VBLF + "           B9.ITEMVALUE   > CHR(0)  OR B7.ITEMVALUE  > CHR(0) OR  B71.ITEMVALUE > CHR(0) OR    ";
                    SQL = SQL + ComNum.VBLF + "           B6.ITEMVALUE   > CHR(0)  OR B10.ITEMVALUE > CHR(0) OR  B11.ITEMVALUE > CHR(0) OR    ";
                    SQL = SQL + ComNum.VBLF + "           B12.ITEMVALUE  > CHR(0))                                                   ";
                    #endregion
                }
                #endregion

                if (chkAsc.Checked == true)
                {
                    SQL += ComNum.VBLF + ("ORDER BY CHARTDATE ASC, CHARTTIME ASC");
                }
                else
                {
                    SQL += ComNum.VBLF + ("ORDER BY CHARTDATE DESC, CHARTTIME DESC");
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, " EMRSYSMP 조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssVital_Sheet1.RowCount = dt.Rows.Count;
                ssVital_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssVital_Sheet1.Cells[i, 0].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    ssVital_Sheet1.Cells[i, 1].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                    ssVital_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IT1"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IT2"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IT3"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 6].Text = dt.Rows[i]["IT4"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 7].Text = dt.Rows[i]["IT5"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IT6"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 9].Text = dt.Rows[i]["IT7"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 10].Text = dt.Rows[i]["IT8"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 11].Text = dt.Rows[i]["IT9"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 12].Text = dt.Rows[i]["IT14"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 13].Text = dt.Rows[i]["IT10"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 14].Text = dt.Rows[i]["IT11"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 15].Text = dt.Rows[i]["IT12"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 16].Text = dt.Rows[i]["IT13"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 17].Text = dt.Rows[i]["IT121"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 18].Text = dt.Rows[i]["IT150"].ToString().Trim();
                    ssVital_Sheet1.Cells[i, 19].Text = dt.Rows[i]["IT274"].ToString().Trim();

                    if (ssVital_Sheet1.Cells[i, 19].Text.Length > 0)
                    {

                        ssVital_Sheet1.Rows[i].Height = ssVital_Sheet1.Rows[i].GetPreferredHeight() + 16;
                    }

                    double rtnVal = VB.Val(dt.Rows[i]["IT3"].ToString().Trim());
                    if (rtnVal >= 140 || rtnVal < 80)  //혈압 it3
                    {
                        ssVital_Sheet1.Cells[i, 5].ForeColor = Color.Red;
                    }

                    rtnVal = VB.Val(dt.Rows[i]["IT4"].ToString().Trim());
                    if (rtnVal >= 90 || rtnVal < 60) //혈압 it4
                    {
                        ssVital_Sheet1.Cells[i, 6].ForeColor = Color.Red;
                    }

                    rtnVal = VB.Val(dt.Rows[i]["IT6"].ToString().Trim());
                    if (rtnVal >= 100 || rtnVal < 60) //맥박 it6
                    {
                        ssVital_Sheet1.Cells[i, 8].ForeColor = Color.Red;
                    }

                    rtnVal = VB.Val(dt.Rows[i]["IT7"].ToString().Trim());
                    if (rtnVal >= 21 || rtnVal < 12)  //호흡 it7
                    {
                        ssVital_Sheet1.Cells[i, 9].ForeColor = Color.Red;
                    }

                    rtnVal = VB.Val(dt.Rows[i]["IT8"].ToString().Trim());
                    if (rtnVal >= 37.5 || rtnVal < 36.5) //체온 it8
                    {
                        ssVital_Sheet1.Cells[i, 10].ForeColor = Color.Red;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return;

            //Print Head 지정
            string strFont1 = "/fn\"굴림체\"/fz\"14\"/fb1/fi0/fu0/fk0/fs1";
            string strFont2 = "/fn\"굴림체\"/fz\"10\"/fb0/fi0/fu0/fk0/fs2";
            string strHead1 = string.Empty;
            string strHead2 = string.Empty;

            strHead1 = "/c/f1 Vital Record History /f1/n/n";

            string strAge = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, mstrPano);
            string strSex = clsVbfunc.READ_SEX(clsDB.DbCon, mstrPano).Equals("F") ? "여자" : "남자";

            strHead2 = "/l/f2" + "     성명 : " + clsVbfunc.GetPatientName(clsDB.DbCon, mstrPano) + " [" + mstrPano + "]   (" + strAge + "/" + strSex + ")" + "/f2/n";
            strHead2 = strHead2 + "/l/f2" + "     출력자(출력일자) : " + clsType.User.UserName + "(" + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + ")" + "/f2/n/n";

            ssVital.ActiveSheet.PrintInfo.AbortMessage = "현재 출력중입니다..";
            ssVital.ActiveSheet.PrintInfo.HeaderHeight = 70;
            ssVital.ActiveSheet.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssVital.ActiveSheet.PrintInfo.Margin.Top = 20;
            ssVital.ActiveSheet.PrintInfo.Margin.Left = 20;
            ssVital.ActiveSheet.PrintInfo.Orientation = PrintOrientation.Landscape;
            //ssVital.ActiveSheet.PrintInfo.Centering = Centering.Horizontal;
            ssVital.ActiveSheet.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssVital.ActiveSheet.PrintInfo.ShowRowHeader = PrintHeader.Show;
            ssVital.ActiveSheet.PrintInfo.ShowBorder = true;
            ssVital.ActiveSheet.PrintInfo.ShowColor = true;
            ssVital.ActiveSheet.PrintInfo.ShowGrid = true;
            ssVital.ActiveSheet.PrintInfo.ShowShadows = true;
            ssVital.ActiveSheet.PrintInfo.UseMax = false;
            ssVital.ActiveSheet.PrintInfo.PrintType = PrintType.All;
            ssVital.PrintSheet(0);
        }
    }
}
