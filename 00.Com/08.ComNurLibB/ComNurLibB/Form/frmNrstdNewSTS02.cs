using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComLibB;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-31
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm통계2New.frm >> frmNrstdNewSTS02.cs 폼이름 재정의" />

    public partial class frmNrstdNewSTS02 : Form
    {

        FarPoint.Win.ComplexBorder border3 = new FarPoint.Win.ComplexBorder(    //ㅡㅡ
new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, Color.White),
new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, Color.White),
new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, Color.White),
new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmNrstdNewSTS02()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strYYMM = "";
            int nRow = 0;
            int nCol = 0;
            string strDEPT_OLD = "";

            Cursor.Current = Cursors.WaitCursor;

            strYYMM = VB.Left(ComboYYMM.Text, 4) + VB.Mid(ComboYYMM.Text, 6, 2);

            DATE_SET(CF.READ_LASTDAY(clsDB.DbCon, strYYMM + "-01"));

            try
            {
                if (OptTong0.Checked == true)
                {
                    SQL = "";
                    SQL = "  SELECT E.DEPTCODE2, E.ACTDATE, E.SIL, A.CNT CNT1, B.CNT CNT2, C.CNT CNT3, D.CNT CNT4";
                    SQL = SQL + ComNum.VBLF + "   FROM (";
                    SQL = SQL + ComNum.VBLF + "     SELECT   TO_CHAR(TIME2, 'YYYY-MM-DD') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '1'";

                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text) + ") ";


                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, (VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2)) + "-01") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TIME2) A,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYY-MM-DD') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '2'";

                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text) + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, (VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2)) + "-01") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TIME2) B,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYY-MM-DD') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '3'";

                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text) + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, (VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2)) + "-01") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TIME2) C,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYY-MM-DD') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '4'";


                    if ((ComboWard.Text).Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text) + ") ";


                    SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text) + ") ";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, (VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2)) + "-01") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TIME2) D,";
                    SQL = SQL + ComNum.VBLF + "   ( SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE, WARDCODE DEPTCODE2, SUM(CNT51 + CNT52) SIL";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_JEWON";
                    SQL = SQL + ComNum.VBLF + "   WHERE ACTDATE >= TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + CF.READ_LASTDAY(clsDB.DbCon, (VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2)) + "-01") + "','YYYY-MM-DD')";


                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";

                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, ACTDATE) E";
                    SQL = SQL + ComNum.VBLF + "   WHERE E.ACTDATE = A.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = A.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = B.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = B.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = C.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = C.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = D.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = D.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "    ORDER BY DEPTCODE2, ACTDATE";

                }
                else if (OptTong1.Checked == true)
                {
                    SQL = " SELECT E.DEPTCODE2, E.ACTDATE, E.SIL, A.CNT CNT1, B.CNT CNT2, C.CNT CNT3, D.CNT CNT4";
                    SQL = SQL + ComNum.VBLF + "   FROM (";
                    SQL = SQL + ComNum.VBLF + "     SELECT   TO_CHAR(TIME2, 'YYYYMM') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '1'";


                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + ComboYear.Text + "-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + ComboYear.Text + "-12-31','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TO_CHAR(TIME2, 'YYYYMM')) A,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYYMM') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '2'";



                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text.Trim()) + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + ComboYear.Text + "-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + ComboYear.Text + "-12-31','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TO_CHAR(TIME2, 'YYYYMM')) B,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYYMM') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '3'";


                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + ComboYear.Text + "-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + ComboYear.Text + "-12-31','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TO_CHAR(TIME2, 'YYYYMM')) C,";
                    SQL = SQL + ComNum.VBLF + "     (SELECT   TO_CHAR(TIME2, 'YYYYMM') TIME1, WARDCODE DEPTCODE1, COUNT(PANO) CNT";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_QI_DATA_LIST";
                    SQL = SQL + ComNum.VBLF + "   WHERE CODE = '52001'";
                    SQL = SQL + ComNum.VBLF + "     AND ITEM = '4'";

                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";

                    SQL = SQL + ComNum.VBLF + "     AND TIME2 >= TO_DATE('" + ComboYear.Text + "-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND TIME2 <= TO_DATE('" + ComboYear.Text + "-12-31','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE, TO_CHAR(TIME2, 'YYYYMM')) D,";
                    SQL = SQL + ComNum.VBLF + "   ( SELECT TO_CHAR(ACTDATE, 'YYYYMM') ACTDATE, WARDCODE DEPTCODE2, SUM(CNT51 + CNT52) SIL";
                    SQL = SQL + ComNum.VBLF + "    From KOSMOS_PMPA.NUR_JEWON";
                    SQL = SQL + ComNum.VBLF + "   WHERE ACTDATE >= TO_DATE('" + ComboYear.Text + "-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ComboYear.Text + "-12-31','YYYY-MM-DD')";

                    if (ComboWard.Text.Trim() != "전체")
                        SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN (" + ReadInWard(ComboWard.Text).Trim() + ") ";

                    SQL = SQL + ComNum.VBLF + "   GROUP BY WARDCODE,  TO_CHAR(ACTDATE, 'YYYYMM')) E";
                    SQL = SQL + ComNum.VBLF + "   WHERE E.ACTDATE = A.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = A.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = B.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = B.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = C.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = C.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.ACTDATE = D.TIME1(+)";
                    SQL = SQL + ComNum.VBLF + "     AND E.DEPTCODE2 = D.DEPTCODE1(+)";
                    SQL = SQL + ComNum.VBLF + "    ORDER BY DEPTCODE2, ACTDATE";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strDEPT_OLD != dt.Rows[i]["DEPTCODE2"].ToString().Trim())
                        {
                            nRow = nRow + 1;
                            strDEPT_OLD = dt.Rows[i]["DEPTCODE2"].ToString().Trim();

                            SS1_Sheet0.RowCount = (nRow * 5 - 2) + 2;

                            SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, 0].Text = strDEPT_OLD;
                        }

                        for (j = 5; j < SS1_Sheet0.ColumnCount; j++)
                        {
                            if (VB.Val(SS1_Sheet0.ColumnHeader.Cells[0, j - 1].Text) == VB.Val(VB.Right(dt.Rows[i]["ACTDATE"].ToString().Trim(), 2)))
                            {
                                nCol = j;
                                break;
                            }
                        }

                        SS1_Sheet0.Cells[(nRow * 5) - 1, nCol - 1].Text =
                        (VB.Val(SS1_Sheet0.Cells[(nRow * 5) - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["SIL"].ToString().Replace(",", ""))).ToString();

                        SS1_Sheet0.Cells[(nRow * 5 - 1) - 1, nCol - 1].Text =
                        (VB.Val(SS1_Sheet0.Cells[(nRow * 5 - 1) - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT4"].ToString().Replace(",", ""))).ToString();

                        SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, nCol - 1].Text =
                        (VB.Val(SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT3"].ToString().Replace(",", ""))).ToString();

                        SS1_Sheet0.Cells[(nRow * 5 - 3) - 1, nCol - 1].Text =
                        (VB.Val(SS1_Sheet0.Cells[(nRow * 5 - 3) - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT2"].ToString().Replace(",", ""))).ToString();

                        SS1_Sheet0.Cells[(nRow * 5 - 4) - 1, nCol - 1].Text =
                        (VB.Val(SS1_Sheet0.Cells[(nRow * 5 - 4) - 1, nCol - 1].Text) + VB.Val(dt.Rows[i]["CNT1"].ToString().Replace(",", ""))).ToString();

                        //SS1.SetCellBorder - 1, -1, -1, -1, SS_BORDER_TYPE_RIGHT, &H0 &, SS_BORDER_STYLE_SOLID

                        //SS1.SetCellBorder - 1, nRow * 5, -1, nRow * 5, SS_BORDER_TYPE_LEFT, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                        //SS1.SetCellBorder - 1, nRow * 5, -1, nRow * 5, SS_BORDER_TYPE_TOP, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                        //SS1.SetCellBorder - 1, nRow * 5, -1, nRow * 5, SS_BORDER_TYPE_RIGHT, &HFFFFFFFF, SS_BORDER_STYLE_DEFAULT
                        //SS1.SetCellBorder - 1, nRow * 5, -1, nRow * 5, SS_BORDER_TYPE_BOTTOM, &H0 &, SS_BORDER_STYLE_SOLID


                        SS1_Sheet0.Rows[(nRow * 5) - 1].BackColor = System.Drawing.Color.FromArgb(202, 255, 202);
                        SS1_Sheet0.Rows[(nRow * 5) - 1].Border = border3;

                        SS1_Sheet0.Cells[(nRow * 5) - 1, 3].Text = "실인원";
                        SS1_Sheet0.Cells[(nRow * 5) - 1, 2].Text = "5";
                        SS1_Sheet0.Cells[(nRow * 5) - 1, 1].Text = strDEPT_OLD;




                        SS1_Sheet0.Cells[(nRow * 5 - 1) - 1, 3].Text = "4군";
                        SS1_Sheet0.Cells[(nRow * 5 - 1) - 1, 2].Text = "4";
                        SS1_Sheet0.Cells[(nRow * 5 - 1) - 1, 1].Text = strDEPT_OLD;

                        SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, 3].Text = "3군";
                        SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, 2].Text = "3";
                        SS1_Sheet0.Cells[(nRow * 5 - 2) - 1, 1].Text = strDEPT_OLD;

                        SS1_Sheet0.Cells[(nRow * 5 - 3) - 1, 3].Text = "2군";
                        SS1_Sheet0.Cells[(nRow * 5 - 3) - 1, 2].Text = "2";
                        SS1_Sheet0.Cells[(nRow * 5 - 3) - 1, 1].Text = strDEPT_OLD;

                        SS1_Sheet0.Cells[(nRow * 5 - 4) - 1, 3].Text = "1군";
                        SS1_Sheet0.Cells[(nRow * 5 - 4) - 1, 2].Text = "1";
                        SS1_Sheet0.Cells[(nRow * 5 - 4) - 1, 1].Text = strDEPT_OLD;

                    }

                }

                dt.Dispose();
                dt = null;

                CALC_SUM();

                for (i = 1; i <= SS1_Sheet0.RowCount; i++)
                {
                    SS1_Sheet0.SetRowHeight(i, ComNum.SPDROWHT);
                }

                for (i = 5; i < SS1_Sheet0.ColumnCount; i++)
                {
                    SS1_Sheet0.SetColumnWidth(i, 30);
                }

                SS1_Sheet0.SetColumnWidth(SS1_Sheet0.ColumnCount - 1, 40);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void DATE_SET(string argLASTDAY)
        {
            int i = 0;
            string strLastDATE = "";


            strLastDATE = VB.Right(argLASTDAY, 2);
            SS1_Sheet0.RowCount = 0;

            if (OptTong0.Checked == true)
            {
                SS1_Sheet0.ColumnCount = (int)VB.Val(strLastDATE) + 5;
                SS1_Sheet0.RowCount = 0;

                for (i = 5; i <= (int)VB.Val(strLastDATE) + 4; i++)
                {
                    SS1_Sheet0.ColumnHeader.Cells[0, i - 1].Text = (i - 4).ToString();
                    Application.DoEvents();
                }
            }
            else
            {
                SS1_Sheet0.ColumnCount = 17;
                for (i = 5; i <= 17; i++)
                {
                    SS1_Sheet0.ColumnHeader.Cells[0, i - 1].Text = (i - 4).ToString();
                }
            }

            SS1_Sheet0.ColumnCount = SS1_Sheet0.ColumnCount;
            SS1_Sheet0.ColumnHeader.Cells[0, SS1_Sheet0.ColumnCount - 1].Text = "합 계";

        }

        private void frmNrstdNewSTS02_Load(object sender, EventArgs e)
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;
            double nYYMM = 0;

            //FstrWard = GstrHelpCode 현재 없음 

            panYear.Visible = false;
            SS1_Sheet0.Columns[2].Visible = false;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));

            for (i = 1; i <= 5; i++)
            {
                ComboYear.Items.Add(nYY.ToString("0000"));
                nYY = nYY - 1;
            }
            ComboYear.SelectedIndex = 0;

            nYY = (int)VB.Val(VB.Left(strDTP, 4));
            nMM = (int)VB.Val(VB.Mid(strDTP, 6, 2));
            nYYMM = Convert.ToDouble(VB.Left(strDTP, 4) + VB.Mid(strDTP, 6, 2));

            ComboYYMM.Items.Clear();

            for (i = 1; i <= 24; i++)
            {
                ComboYYMM.Items.Add(nYY.ToString("0000") + "년" + nMM.ToString("00") + "월");
                nMM = nMM - 1;
                if (nMM == 0)
                {
                    nYY = nYY - 1;
                    nMM = 12;
                }
            }

            ComboYYMM.SelectedIndex = 0;
            lblSts.Text = "";

            ComboWard_SET();

        }

        private void ComboWard_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "   AND USED = 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                ComboWard.Items.Clear();
                ComboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["Wardcode"].ToString().Trim());
                }
                ComboWard.Items.Add("SICU");
                ComboWard.Items.Add("MICU");

                ComboWard.SelectedIndex = 0;

                for (i = 0; i < ComboWard.Items.Count; i++)
                {
                    if (ComboWard.Items.IndexOf(clsNurse.gsWard) == i)
                    {
                        ComboWard.SelectedIndex = i;
                        ComboWard.Enabled = false;
                        return;
                    }
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void OptTong_CheckedChanged(object sender, EventArgs e)
        {
            if (OptTong0.Checked == true)
            {
                panYYMM.Visible = true;
                panYear.Visible = false;
            }
            else
            {
                panYYMM.Visible = false;
                panYear.Visible = true;

            }
        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strWARD = "";
            string strGUBUN = "";
            string strGBN1 = "";
            string strYYMM = "";
            string strDate = "";

            if (e.Row < 1)
            {
                return;
            }
            if (e.Column < 5)
            {
                return;
            }

            strWARD = SS1_Sheet0.Cells[e.Row, 1].Text;
            strGUBUN = SS1_Sheet0.Cells[e.Row, 2].Text;

            strDate = SS1_Sheet0.ColumnHeader.Cells[0, e.Column].Text;

            strYYMM = VB.Left(ComboYYMM.Text, 4) + VB.Mid(ComboYYMM.Text, 6, 2);
            strDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + strDate; ////       VB.Format(strYYMM, "@@@@-@@") + "-" + strDate;

            strGBN1 = "52001";

            //FrmDetailList
            frmDetailListEtc frmDetailListEtcX = new frmDetailListEtc();

            switch (strGUBUN)
            {
                case "1":
                case "2":
                case "3":
                case "4":

                    if (OptTong0.Checked == true)
                    {
                        Display_Detail_List(strGBN1, strYYMM, strDate, strGUBUN, frmDetailListEtcX.ssList, strWARD, "1");
                        frmDetailListEtcX.ShowDialog();
                    }
                    else
                    {
                        Display_Detail_List(strGBN1, strYYMM, strDate, strGUBUN, frmDetailListEtcX.ssList, strWARD, "2");
                        frmDetailListEtcX.ShowDialog();
                    }

                    break;
            }

            frmDetailListEtcX = null;

        }

        private string Display_Detail_List(string ArgCode, string argYYMM, string argDATE, string argGBN, FarPoint.Win.Spread.FpSpread ArgSheet, string argWard, string argGbn2)
        {
            string strVal = "";
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE,YYMM,ITEM,PANO,SNAME,AGE,SEX,DEPTCODE,WARDCODE,ROOMCODE,DRCODE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(TIME1,'YYYY-MM-DD HH24:MI') TIME1,  TO_CHAR(TIME2,'YYYY-MM-DD HH24:MI') TIME2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_QI_DATA_LIST ";
                SQL = SQL + ComNum.VBLF + " WHERE CODE ='" + ArgCode + "' ";

                if (argGbn2 == "2")
                {
                    SQL = SQL + ComNum.VBLF + "   AND YYMM ='" + VB.Left(argYYMM, 4) + VB.Right(argDATE, 2) + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND YYMM ='" + argYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND TIME2 = TO_DATE('" + argDATE + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "   AND ITEM ='" + argGBN + "' ";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + argWard + "' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY TIME1, TIME2 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    ArgSheet.ActiveSheet.RowCount = 0;
                    ArgSheet.ActiveSheet.RowCount = dt.Rows.Count;
                    ArgSheet.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ArgSheet.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["sex"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["wardcode"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["roomcode"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Drcode"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["TIME1"].ToString().Trim();
                        ArgSheet.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["TIME2"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return strVal;
        }

        private void CALC_SUM()
        {
            int i = 0;
            int j = 0;
            double nTOT1 = 0;
            double nTOT2 = 0;
            double nTOT3 = 0;
            double nTOT4 = 0;
            double nTOT5 = 0;

            for (i = 1; i < SS1_Sheet0.RowCount; i++)
            {
                nTOT1 = 0;
                for (j = 5; j < SS1_Sheet0.ColumnCount; j++)
                {
                    nTOT1 = nTOT1 + VB.Val(SS1_Sheet0.Cells[i - 1, j - 1].Text);
                }
                SS1_Sheet0.Cells[i - 1, SS1_Sheet0.ColumnCount - 1].Text = nTOT1.ToString();
            }

            SS1_Sheet0.RowCount = SS1_Sheet0.RowCount + 5;

            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 4) - 1, 3].Text = "1군";
            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 3) - 1, 3].Text = "2군";
            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 2) - 1, 3].Text = "3군";
            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 1) - 1, 3].Text = "4군";

            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount) - 1, 3].Text = "실인원";
            SS1_Sheet0.Rows[(SS1_Sheet0.RowCount - 1) - 1].BackColor = Color.FromArgb(202, 255, 202);

            SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 2) - 1, 0].Text = "합 계";

            for (i = 5; i <= SS1_Sheet0.ColumnCount; i++)
            {
                for (j = 1; j < SS1_Sheet0.RowCount - 5; j = j + 5)
                {
                    nTOT1 = nTOT1 + VB.Val(SS1_Sheet0.Cells[j - 1, i - 1].Text);
                    nTOT2 = nTOT2 + VB.Val(SS1_Sheet0.Cells[(j + 1) - 1, i - 1].Text);
                    nTOT3 = nTOT3 + VB.Val(SS1_Sheet0.Cells[(j + 2) - 1, i - 1].Text);
                    nTOT4 = nTOT4 + VB.Val(SS1_Sheet0.Cells[(j + 3) - 1, i - 1].Text);
                    nTOT5 = nTOT5 + VB.Val(SS1_Sheet0.Cells[(j + 4) - 1, i - 1].Text);
                }

                SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 4) - 1, i - 1].Text = nTOT1.ToString();
                nTOT1 = 0;
                SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 3) - 1, i - 1].Text = nTOT2.ToString();
                nTOT2 = 0;
                SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 2) - 1, i - 1].Text = nTOT3.ToString();
                nTOT3 = 0;
                SS1_Sheet0.Cells[(SS1_Sheet0.RowCount - 1) - 1, i - 1].Text = nTOT4.ToString();
                nTOT4 = 0;
                SS1_Sheet0.Cells[(SS1_Sheet0.RowCount) - 1, i - 1].Text = nTOT5.ToString();
                nTOT5 = 0;

            }

        }

        /// <summary>
        /// 과거 병동 데이터 조회 되도록 프로그램
        /// 쿼리 사용시 IN으로 조회해야함.
        /// </summary>
        /// <param name="argWard"></param>
        /// <returns></returns>
        private string ReadInWard(string argWard)
        {
            string rtnval = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_과거병동조회' ";
                SQL = SQL + ComNum.VBLF + "    AND NAME = '" + argWard + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnval = "'" + argWard + "'";
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnval = rtnval + dt.Rows[i]["CODE"].ToString().Trim() + "','";
                    }
                    rtnval = "'" + rtnval;
                    rtnval = VB.Mid(rtnval, 1, VB.Len(rtnval) - 2);

                }
                dt.Dispose();
                dt = null;

                return rtnval;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnval;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return rtnval;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
