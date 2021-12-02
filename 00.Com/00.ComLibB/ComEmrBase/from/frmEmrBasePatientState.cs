using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBasePatientState
    /// Description     : 환자상태 Vital 조회
    /// Author          : 전상원
    /// Create Date     : 2018-05-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO: strTEMP 선언안되어있음
    /// </history>
    /// <seealso cref= \mtsEmr\EMRWARD\MHEMRWARD.vbp(FrmPatientVital)" >> frmEmrBasePatientState.cs 폼이름 재정의" />
    public partial class frmEmrBasePatientState : Form
    {
        public frmEmrBasePatientState()
        {
            InitializeComponent();
        }

        private void frmEmrBasePatientState_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboDate.Items.Clear();
            cboDate.Items.Add("1주 이내");
            cboDate.Items.Add("2주 이내");
            cboDate.Items.Add("3주 이내");
            cboDate.Items.Add("4주 이내");
            cboDate.Items.Add("5주 이내");
            cboDate.SelectedIndex = 1;

            //If GetBMedDept(Rs)Then
            //    For i = 1 To Rs.RecordCount
            //        cboDept.AddItem Trim(Rs("MEDDEPTCD") & "")
            //        Rs.MoveNext
            //    Next i

            //    Rs.Close: Set Rs = Nothing
            //    cboDept.ListIndex = 0
            //End If


            //cboDept.Text = "MP"
            //Call cboDept_Click

            //Call cmdView_Click
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int nRead = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            lblTitle1.Text = "";

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " (SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DRCODE = A.DRCODE) DRNAME, ";
                SQL = SQL + ComNum.VBLF + "  DRCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, IPDNO, WARDCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A";
                SQL = SQL + ComNum.VBLF + "  WHERE JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + VB.Trim(cboDept.Text) + "'";
                if (VB.Trim(VB.Right(cboDr.Text, 10)) != "0")
                {
                    SQL = SQL + ComNum.VBLF + "    AND DRCODE = '" + VB.Trim(VB.Right(cboDr.Text, 10)) + "' ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE, SNAME";

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
                    ssList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_VITAL(string argPTNO, string argSDATE, string argEDATE)
        {
            int i = 0;
            int nRead = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssVital_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.CHARTDATE, A.CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it3') AS IT3,";    //혈압(Sys)
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it4') AS IT4,";    //혈압(Dia)
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it6') AS IT6,";    //맥박
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it8') AS IT8,";    //체온
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it10') AS IT10,";  //BWT
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it14') AS IT14,";  //SpO2
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it274') AS IT274"; //SpO2
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRXMLMST B";
                SQL = SQL + ComNum.VBLF + "   WHERE B.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.CHARTDATE >= '" + argSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.CHARTDATE <= '" + argEDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.FORMNO = '1562'";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO ";
                SQL = SQL + ComNum.VBLF + "      ORDER BY B.CHARTDATE DESC, B.CHARTTIME DESC ";
                SQL = SQL + ComNum.VBLF + "";

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
                    ssVital_Sheet1.RowCount = dt.Rows.Count + 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssVital_Sheet1.Cells[0, i + 1].Text = VB.Right(VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000/00/00"), 5) + ComNum.VBLF + VB.Val(dt.Rows[i]["CHARTTIME"].ToString().Trim()).ToString("00:00");
                        ssVital_Sheet1.Cells[2, i + 1].Text = dt.Rows[i]["IT3"].ToString().Trim();
                        ssVital_Sheet1.Cells[3, i + 1].Text = dt.Rows[i]["IT4"].ToString().Trim();
                        ssVital_Sheet1.Cells[4, i + 1].Text = dt.Rows[i]["IT6"].ToString().Trim();
                        ssVital_Sheet1.Cells[5, i + 1].Text = dt.Rows[i]["IT8"].ToString().Trim();
                        ssVital_Sheet1.Cells[6, i + 1].Text = dt.Rows[i]["IT14"].ToString().Trim();
                        ssVital_Sheet1.Cells[7, i + 1].Text = dt.Rows[i]["IT10"].ToString().Trim();
                        ssVital_Sheet1.Cells[8, i + 1].Text = dt.Rows[i]["IT274"].ToString().Trim();
                        ssVital_Sheet1.SetColumnWidth(i + 1, 7);
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

        private void READ_DATA(string argPTNO, string ArgInDate, string ArgIPDNO, string argAge, string argWARD)
        {
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            string strSDate = "";
            string strEDATE = "";
            string strWeek = "";

            ComFunc CF = new ComFunc();

            strWeek = VB.Left(cboDate.Text, 1);

            strEDATE = strSysDate.Replace("-", "");

            if (strWeek == "")
            {
                strSDate = strEDATE;
            }
            else
            {
                strSDate = CF.DATE_ADD(clsDB.DbCon, strSysDate, (int)(VB.Val(strWeek) * 7) * -1);
            }

            strSDate = strSDate.Replace("-", "");
            strEDATE = strEDATE.Replace("-", "");

            READ_VITAL(argPTNO, strSDate, strEDATE);
            READ_IO(argPTNO, strSDate, strEDATE);
            READ_BST(argPTNO, strSDate, strEDATE);

            READ_DETAIL_BRADEN(argPTNO, strSysDate, ArgIPDNO, argAge, argWARD);
            READ_DETAIL_PAIN(ArgIPDNO, argPTNO);
            READ_NURPRO(argPTNO, ArgInDate);
        }

        private void READ_BST(string argPTNO, string argSDATE, string argEDATE)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssBST_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL = " SELECT CHARTDATE, CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "   extractValue(chartxml, '//ta1') AS TA1,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta2') AS TA2,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta3') AS TA3,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta4') AS TA4,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta5') AS TA5,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta6') AS TA6,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta7') AS TA7,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta8') AS TA8,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta9') AS TA9";
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "  SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = '1572'";
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + argSDATE + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + argEDATE + "')";
                SQL = SQL + ComNum.VBLF + "      ORDER BY CHARTDATE DESC, CHARTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssBST_Sheet1.Cells[0, i + 1].Text = VB.Val(VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4)).ToString("00/00") + ComNum.VBLF + VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                        ssBST_Sheet1.Cells[2, i + 1].Text = dt.Rows[i]["TA2"].ToString().Trim();
                        ssBST_Sheet1.Cells[3, i + 1].Text = dt.Rows[i]["TA4"].ToString().Trim() + " " + dt.Rows[i]["TA5"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["TA9"].ToString().Trim();
                        ssBST_Sheet1.SetColumnWidth(i + 1, 7);
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

        private void READ_IO(string argPTNO, string argSDATE, string argEDATE)
        {
            int i = 0;
            //int j = 0;
            int nRead = 0;
            int nItotal = 0;
            int nOtotal = 0;
            int nCOL = 0;
            int nINFUSION = 0;
            int nEXCRETION = 0;
            //int nSTOOL = 0;
            //int nURINE = 0;
            //int nBLOOD = 0;
            //int nEPIGRAM = 0;

            string strChartDate = "";
            string strChartTime = "";
            string strOLDCHARTDATE = "";
            string strOLDCHARTTIME = "";
            //string strTEMP = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssIO_Sheet1.RowCount = 3;

            try
            {
                SQL = "";
                SQL = " SELECT 'WARD' GBN, CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it64') AS DUTY,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it2') 구강1,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it5') 구강2 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it8') 구강3,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it11') 수액1,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it14') 수액2 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it17') 수액3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it20') 수액4 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it23') 수액5 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it66') 수액6,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it26') 혈액1 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it29') 혈액2,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it32') 혈액3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it35') 혈액4,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it38') 섭취총량,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it41') URINE,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it44') STOOL,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it47') 기타배설1 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it50') 기타배설2,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it53') 기타배설3 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it56') 기타배설4 ,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it59') 기타배설5,";
                SQL = SQL + ComNum.VBLF + "               extractValue(chartxml, '//it62') 배설총량";
                SQL = SQL + ComNum.VBLF + "     From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = '1970'";
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + argSDATE + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + argEDATE + "')";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "   SELECT 'ICU' GBN, CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta1') AS DUTY,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta3') 구강1,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta6') 구강2,";
                SQL = SQL + ComNum.VBLF + "   '0' 구강3,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액1,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액2,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액3,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액4,";
                SQL = SQL + ComNum.VBLF + "   '0' 수액5,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta9') 수액6,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta12') 혈액1,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta15') 혈액2 ,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta18') 혈액3 ,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta21') 혈액4,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta24') 섭취총량,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta27') URINE,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta223') STOOL,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설1,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설2,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설3,";
                SQL = SQL + ComNum.VBLF + "   '0' 기타배설4,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta502') 기타배설5,";
                SQL = SQL + ComNum.VBLF + "   EXTRACTVALUE(CHARTXML, '//ta724') 배설총량";
                SQL = SQL + ComNum.VBLF + "     From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "   WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT EMRNO";
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE >= '" + argSDATE + "' ";
                SQL = SQL + ComNum.VBLF + " AND CHARTDATE <= '" + argEDATE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND FORMNO = '1795')";
                SQL = SQL + ComNum.VBLF + "   ORDER BY CHARTDATE DESC, CHARTTIME DESC";

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
                    nCOL = 0;
                    ssIO_Sheet1.RowCount = dt.Rows.Count + 3;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            nCOL = nCOL + 3;
                        }

                        nCOL = nCOL + 1;

                        strChartDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        strChartTime = dt.Rows[i]["CHARTTIME"].ToString().Trim();

                        if (strOLDCHARTDATE == strChartDate && VB.Val(strChartTime) <= 0700 && strOLDCHARTDATE != "" && strChartDate != "")
                        {
                            ssIO_Sheet1.RowCount = ssIO_Sheet1.RowCount + 1;

                            ssIO_Sheet1.Cells[0, nCOL - 1].Text = "총량";
                            ssIO_Sheet1.AddSpanCell(2, nCOL - 1, 3, 1);
                            ssIO_Sheet1.Cells[2, nCOL - 1].Text = nItotal.ToString().Trim();
                            ssIO_Sheet1.AddSpanCell(5, nCOL - 1, 3, 1);
                            ssIO_Sheet1.Cells[5, nCOL - 1].Text = nOtotal.ToString().Trim();
                            nItotal = 0;
                            nOtotal = 0;
                            ssIO_Sheet1.SetColumnWidth(nCOL - 1, 7);
                            nCOL = nCOL + 1;
                        }

                        ssIO_Sheet1.Cells[0, nCOL - 1].Text = VB.Left(VB.Right(strChartDate, 4), 2) + "/" + VB.Right(strChartDate, 2) + ComNum.VBLF + VB.Left(strChartTime, 2) + ":" + VB.Right(strChartTime, 2);
                        ssIO_Sheet1.Cells[8, nCOL - 1].Text = dt.Rows[i]["DUTY"].ToString().Trim();

                        ssIO_Sheet1.Cells[2, nCOL - 1].Text = dt.Rows[i]["구강1"].ToString().Trim() + dt.Rows[i]["구강2"].ToString().Trim() + dt.Rows[i]["구강3"].ToString().Trim();
                        nItotal = nItotal + (int)VB.Val(ssIO_Sheet1.Cells[2, nCOL - 1].Text);

                        //TODO: strTEMP 선언안되어있음
                        //strTEMP = dt.Rows[i]["수액6"].ToString().Trim();
                        //strTEMP = dt.Rows[i]["수액6"].ToString().Trim().Split("\n");
                        //strTEMP = Split(Trim(AdoGetString(RSI, "수액6", ii)), vbLf)
                        //nINFUSION = 0;
                        //for (j = 0; j < VB.UBound(strTEMP); j++)
                        //{
                        //    nINFUSION = nINFUSION + VB.Val(strTEMP[j]);

                        //}

                        //strTEMP = Split(Trim(AdoGetString(RSI, "기타배설5", ii)), vbLf)
                        //n기타배설 = 0
                        //For jj = 0 To UBound(strTEMP)
                        //    n기타배설 = n기타배설 + Val(strTEMP(jj))
                        //Next jj

                        ssIO_Sheet1.Cells[3, nCOL - 1].Text = dt.Rows[i]["수액1"].ToString().Trim() + dt.Rows[i]["수액2"].ToString().Trim() + dt.Rows[i]["수액3"].ToString().Trim() + dt.Rows[i]["수액4"].ToString().Trim() + dt.Rows[i]["수액5"].ToString().Trim() + nINFUSION;
                        nItotal = nItotal + (int)VB.Val(ssIO_Sheet1.Cells[3, nCOL - 1].Text);

                        ssIO_Sheet1.Cells[4, nCOL - 1].Text = dt.Rows[i]["수액1"].ToString().Trim() + dt.Rows[i]["수액2"].ToString().Trim() + dt.Rows[i]["수액3"].ToString().Trim() + dt.Rows[i]["수액4"].ToString().Trim();
                        nItotal = nItotal + (int)VB.Val(ssIO_Sheet1.Cells[4, nCOL - 1].Text);

                        ssIO_Sheet1.Cells[5, nCOL - 1].Text = dt.Rows[i]["URINE"].ToString().Trim();
                        nOtotal = nOtotal + (int)VB.Val(ssIO_Sheet1.Cells[5, nCOL - 1].ToString().Trim());

                        ssIO_Sheet1.Cells[6, nCOL - 1].Text = dt.Rows[i]["STOOL"].ToString().Trim();
                        nOtotal = nOtotal + (int)VB.Val(ssIO_Sheet1.Cells[6, nCOL - 1].ToString().Trim());

                        ssIO_Sheet1.Cells[7, nCOL - 1].Text = dt.Rows[i]["기타배설1"].ToString().Trim() + dt.Rows[i]["기타배설2"].ToString().Trim() + dt.Rows[i]["기타배설3"].ToString().Trim() + dt.Rows[i]["기타배설4"].ToString().Trim() + nEXCRETION;
                        nOtotal = nOtotal + (int)VB.Val(ssIO_Sheet1.Cells[7, nCOL - 1].Text);

                        if (dt.Rows[i]["GBN"].ToString().Trim() == "ICU")
                        {
                            ssIO_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 255);
                        }

                        strOLDCHARTDATE = strChartDate;
                        strOLDCHARTTIME = strChartTime;

                        ssIO_Sheet1.SetColumnWidth(nCOL - 1, 7);
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

        private void READ_DETAIL_FALL(string argPTNO, string argDATE, string ArgIPDNO, string ArgAge)
        {
            // 여기꺼 바꾸면 CARE PLAN 것도 바꿔야 함.
            string strFall = "";
            string strTOTAL = "";
            string strCAUSE = "";
            string strDrug = "";
            string strTEMP = "";
            string strTOOL = "";
            string strWARD_C = "";
            string strAGE_C = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            strWARD_C = "중환자실 재원 환자";
                            break;
                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            strWARD_C = "신생아실 재원 환자";
                            break;
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 70)
                    {
                        strFall = "OK";
                        strAGE_C = "70세 이상 환자";
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 7)
                    {
                        strFall = "OK";
                        strAGE_C = "7세 미만 환자";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                SQL = "";
                SQL = "  SELECT PANO, TOTAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                strTOOL = "The Morse Fall Scale";

                if (VB.Val(ArgAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                    strTOOL = "The Humpty Dumpty Scale";
                }

                //신생아의 경우 도구표 사용하지 않음
                if (strWARD_C == "신생아실 재원 환자")
                {
                    strTOOL = "";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    if (VB.Val(ArgAge) < 18 && VB.Val(strTOTAL) >= 12 || VB.Val(ArgAge) >= 18 && VB.Val(strTOTAL) >= 51)
                    {
                        strFall = "OK";
                    }
                }

                dt.Dispose();
                dt = null;

                strDrug = "";
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_FALL_WARNING";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND (WARNING1 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING2 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING3 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING4 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_01 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_02 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_03 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_04 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_05 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_06 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_07 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_08 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";

                    strCAUSE = "";
                    if (strAGE_C == "")
                    {
                        if (dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "70세이상 ";
                        }
                        if (dt.Rows[0]["WARNING2"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "보행장애 ";
                        }
                        if (dt.Rows[0]["WARNING3"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "혼미 ";
                        }
                        if (dt.Rows[0]["WARNING4"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "어지럼증 ";
                        }
                        strDrug = "";

                        if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "진정제 ";
                        }
                        if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "수면제 ";
                        }
                        if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "향정신성약물 ";
                        }
                        if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "항우울제 ";
                        }
                        if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "완하제 ";
                        }
                        if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "이뇨제 ";
                        }
                        if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "진정약물 ";
                        }
                        if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + dt.Rows[0]["DRUB_08_ETC"].ToString().Trim();
                        }
                    }
                }

                ssIndicator_Sheet1.Cells[1, 1].Text = strTOTAL;

                if (strFall == "OK")
                {
                    ssIndicator_Sheet1.Cells[2, 1].Text = "고위험";
                }

                strTEMP = "";
                if (strWARD_C != "")
                {
                    strTEMP = strTEMP + strWARD_C;
                    strTEMP = strTEMP + ComNum.VBLF;
                }

                if (strAGE_C != "")
                {
                    strTEMP = strTEMP + strAGE_C;
                    strTEMP = strTEMP + ComNum.VBLF;
                }

                if (strCAUSE != "")
                {
                    strTEMP = strTEMP + strCAUSE;
                    strTEMP = strTEMP + ComNum.VBLF;
                }

                if (strDrug != "")
                {
                    strTEMP = strTEMP +  "-위험약물:" + strDrug;
                    strTEMP = strTEMP + ComNum.VBLF;
                }

                ssIndicator_Sheet1.Cells[3, 1].Text = strTEMP;
                ssIndicator_Sheet1.Cells[4, 1].Text = strTOOL;
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

        private void READ_DETAIL_BRADEN(string argPTNO, string argDATE, string ArgIPDNO, string ArgAge, string argWard, string ArgDate2 = "")
        {
            string strBraden = "";
            string strGUBUN = "";
            string strTOTAL = "";
            string strBun   = "";
            string strTOOL  = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //if (argPTNO == "09315922")
            //{

            //}

            if (argWard == "NR" || argWard == "ND" || argWard == "IQ")
            {
                strGUBUN = "신생아";
                strTOOL = "신생아욕창사정 도구표";
            }
            else if (VB.Val(ArgAge) < 5)
            {
                strGUBUN = "소아";
                strTOOL = "소아욕창사정 도구표";
            }   
            else
            {
                strGUBUN = "";
                strTOOL = "욕창사정 도구표";
            }

            //if (argPTNO == "08619351")
            //{
            //    argPTNO = argPTNO;
            //}

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strGUBUN == "")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";
                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                        if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) > 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18 || VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                else if (strGUBUN == "신생아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                        if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 20)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = " SELECT *";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( ";
                SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                SQL = SQL + ComNum.VBLF + "      )";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strBraden = "OK";
                    strBun = "";

                    if (dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "중환자실 ";
                    }
                    if (dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "중증도 분류 3군 이상 ";
                    }
                    if (dt.Rows[0]["PARAL"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "뇌, 척추 관련 마비 ";
                    }
                    if (dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "부종 ";
                    }
                    if (dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "영양불량 ";
                    }
                    if (dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "단백질 불량 ";
                    }
                    if (dt.Rows[0]["EDEMA"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "부동 ";
                    }
                    if (dt.Rows[0]["BRADEN"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "현재 욕창이 있는 환자 ";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                ssIndicator_Sheet1.Cells[1, 3].Text = strTOTAL;

                if (strBraden == "OK")
                {
                    ssIndicator_Sheet1.Cells[2, 3].Text = "고위험";
                }

                ssIndicator_Sheet1.Cells[3, 3].Text = strBun;
                ssIndicator_Sheet1.Cells[4, 3].Text = strTOOL;
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

        private void READ_DETAIL_PAIN(string ArgIPDNO, string ArgPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL = SQL + ComNum.VBLF + "  MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL = SQL + ComNum.VBLF + "  DRUG, NODRUG, TIMES";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE)    ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssIndicator_Sheet1.Cells[1, 5].Text = dt.Rows[0]["SCORE"].ToString().Trim();
                    ssIndicator_Sheet1.Cells[2, 5].Text = dt.Rows[0]["REGION"].ToString().Trim();
                    ssIndicator_Sheet1.Cells[4, 5].Text = dt.Rows[0]["TOOLS"].ToString().Trim();
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

        private void READ_NURPRO(string argPTNO, string argMEDFRDATE)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssNurPro_Sheet1.RowCount = 1;

            argMEDFRDATE = argMEDFRDATE.Replace("-", "");

            try
            {
                SQL = "";
                SQL = " SELECT A.RANKING, A.SEQNO, B.NURPROBLEM, A.GOAL, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.EDATE,'YYYY-MM-DD') EDATE, A.BIGO, A.ROWID, '1' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_CADEX_NURPROBLEM A, KOSMOS_EMR.EMR_CADEX_NURPROBLEM_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.MEDFRDATE = '" + argMEDFRDATE + "' ";
                if (chkNurDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.EDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT RANKING, 0, PROBLEM, GOAL, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, '' BIGO, A.ROWID, '2' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_CARE_GOAL A";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.INDATE = TO_DATE('" + argMEDFRDATE + "','YYYY-MM-DD')  ";
                if (chkNurDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssNurPro_Sheet1.RowCount = dt.Rows.Count + 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssNurPro_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["NURPROBLEM"].ToString().Trim();
                        ssNurPro_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["GOAL"].ToString().Trim();
                        ssNurPro_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssNurPro_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssNurPro_Sheet1.SetRowHeight(i + 1, 17);
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
    }
}