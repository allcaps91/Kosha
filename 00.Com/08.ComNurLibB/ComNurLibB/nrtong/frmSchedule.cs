using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-03
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\nurse\nrtong\nrtong.vbp\FrmSchedule.frm >> frmSchedule.cs 폼이름 재정의" />
    /// 
    public partial class frmSchedule : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmSchedule()
        {
            InitializeComponent();
        }

        private string READ_JikName(string Arg1)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT Name cJikName FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND Code = '" + VB.Val(Arg1).ToString("00") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["cJikName"].ToString().Trim();

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


            return rtnVal;
        }

        private string READ_BuseCode(string Arg1)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (Arg1 == "치료사(ICU)")
            {
                Arg1 = "간호부";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Code cCode FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '2' ";
                SQL = SQL + ComNum.VBLF + "  AND Name = '" + Arg1.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["cCode"].ToString().Trim();

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


            return rtnVal;
        }

        private string READ_BuseName(string Arg1)
        {
            string rtnVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT Name cName FROM NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '2' ";
                SQL = SQL + ComNum.VBLF + "  AND Code = '" + Arg1.Trim() + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["cCode"].ToString().Trim();

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


            return rtnVal;
        }

        private string ComboDept_Code()
        {
            string rtnVal = "";

            switch (Combodept.Text)
            {
                case "간호부":
                    rtnVal = "033101";
                    break;
                case "OR":
                    rtnVal = "033102";
                    break;
                case "마취과":
                    rtnVal = "033103";
                    break;
                case "NR":
                    rtnVal = "033104";
                    break;
                case "DR":
                    rtnVal = "033105";
                    break;
                case "SICU":
                    rtnVal = "033106";
                    break;
                case "공급실":
                    rtnVal = "033107";
                    break;
                case "HD":
                    rtnVal = "033108";
                    break;
                case "ER":
                    rtnVal = "033109";
                    break;
                case "외래":
                    rtnVal = "033110";
                    break;
                case "정신과":
                    rtnVal = "033111";
                    break;
                case "3A":
                    rtnVal = "033112";
                    break;
                case "3B":
                    rtnVal = "033113";
                    break;
                case "4A":
                    rtnVal = "033114";
                    break;
                case "5W":
                    rtnVal = "033116";
                    break;
                case "6W":
                    rtnVal = "033117";
                    break;
                case "7W":
                    rtnVal = "033118";
                    break;
                case "8W":
                    rtnVal = "033119";
                    break;
                case "MICU":
                    rtnVal = "033120";
                    break;
            }

            return rtnVal;
        }

        private void frmSchedule_Load(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strData = "";

            SS1_Sheet1.Columns[33].Visible = false;
            SS1_Sheet1.Columns[34].Visible = false;
            SS1_Sheet1.Columns[35].Visible = false;
            SS1_Sheet1.Columns[36].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "0");
                cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                cboYYMM.SelectedIndex = 0;

                Combodept.Items.Add("치료사(ICU)");

                SQL = "";
                SQL = "SELECT Code,Name ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun = '2' ";
                SQL = SQL + ComNum.VBLF + "  AND Code Not IN ('JU','SI') ";
                SQL = SQL + ComNum.VBLF + "  AND GBUSE = 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER By PrintRanking ";

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
                    ComFunc.MsgBox("부서가 설정되어 있지 않습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = dt.Rows[i]["Name"].ToString().Trim();
                    Combodept.Items.Add(strData);
                }

                Combodept.SelectedIndex = 0;

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

        private void Clear()
        {
            SS1_Sheet1.RowCount = 50;
            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";

            SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 232);

            SS1_Sheet1.Cells[1, 0, 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(0, 0, 0);

            SS1_Sheet1.Cells[2, 2, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            cboYYMM.Enabled = true;
            Combodept.Enabled = true;
            btnSearch.Enabled = true;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            btnPrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int J = 0;
            string strYY = "";            //'콤보 박스 YY
            string strMM = "";            //'콤보 박스 MM
            string strDate = "";            //'날짜 비교
            string strDateH1 = "";            //'월의 처음 날짜 (YYYY-MM-DD)
            string strDateH2 = "";            //'월의 마지막 날짜 (YYYY-MM-DD)
            string strDay = "";            //'월의 처음 날짜 (YYYY-MM-DD)
            string strHDay = "";            //'월의 처음 날짜 (휴일 Check)
            string strDay1 = "";            //'월의 처음 요일
            int nCol = 0;
            int nCol2 = 0;
            int nCol3 = 0;
            int nTEMP = 0;          //'익일의 데이타 check용
            int nRow = 0;          //'SS1.MaxRows 비교용
            //string nDTot = "";            //'주간근무일 합계
            //string nETot = "";            //'저녁근무일 합계
            //string nNTot = "";            //'야간근무일 합계
            //string nOFFTot = "";            //'휴무 합계
            string strBUSE = "";
            int nHDay = 0;           //'휴일인 날짜
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int nDateH1 = 0;
            int nDateH2 = 0;

            cboYYMM.Enabled = false;
            Combodept.Enabled = false;

            Clear();

            strYY = VB.Left(cboYYMM.Text, 4);
            strMM = VB.Mid(cboYYMM.Text, 6, 2);

            strDay = strYY + "-" + strMM + "-01";
            strHDay = strYY + "-" + strMM + "-01";
            strDate = strYY + "-" + strMM + "-01";
            strDateH1 = strDate;

            strDateH2 = CF.READ_LASTDAY(clsDB.DbCon, strDate);

            nDateH1 = (int)VB.Val(VB.Right(strDateH1, 2));
            nDateH2 = (int)VB.Val(VB.Right(strDateH2, 2));

            nCol = 3;

            if (nDateH2 == 28)
            {
                SS1_Sheet1.Columns[(nCol + 28 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 29 - 1) - 1].Visible = false;
                SS1_Sheet1.Columns[(nCol + 30 - 1) - 1].Visible = false;
                SS1_Sheet1.Columns[(nCol + 31 - 1) - 1].Visible = false;
            }
            else if (nDateH2 == 29)
            {
                SS1_Sheet1.Columns[(nCol + 28 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 29 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 30 - 1) - 1].Visible = false;
                SS1_Sheet1.Columns[(nCol + 31 - 1) - 1].Visible = false;
            }
            else if (nDateH2 == 30)
            {
                SS1_Sheet1.Columns[(nCol + 28 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 29 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 30 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 31 - 1) - 1].Visible = false;
            }
            else if (nDateH2 == 31)
            {
                SS1_Sheet1.Columns[(nCol + 28 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 29 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 30 - 1) - 1].Visible = true;
                SS1_Sheet1.Columns[(nCol + 31 - 1) - 1].Visible = true;
            }



            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //월의 요일  Check
                nCol2 = 3;
                SQL = "";
                SQL = "Select TO_CHAR(TO_DATE('" + strDay + "', 'YYYY-MM-DD'),'DY') cWeek FROM DUAL ";

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
                    btnCancel.Enabled = true;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDay1 = dt.Rows[i]["cWeek"].ToString().Trim();

                    switch (VB.UCase(strDay1))
                    {
                        case "일":
                            strDay1 = "1";
                            break;
                        case "월":
                            strDay1 = "2";
                            break;
                        case "화":
                            strDay1 = "3";
                            break;
                        case "수":
                            strDay1 = "4";
                            break;
                        case "목":
                            strDay1 = "5";
                            break;
                        case "금":
                            strDay1 = "6";
                            break;
                        case "토":
                            strDay1 = "7";
                            break;
                    }
                }
                for (i = nDateH1; i <= nDateH2; i++)
                {
                    switch (strDay1)
                    {
                        case "1":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "일";
                            break;
                        case "2":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "월";
                            break;
                        case "3":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "화";
                            break;
                        case "4":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "수";
                            break;
                        case "5":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "목";
                            break;
                        case "6":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "금";
                            break;
                        case "7":
                            SS1_Sheet1.Cells[0, nCol2 - 1].Text = "토";
                            break;
                    }
                    nCol2 = nCol2 + 1;
                    strDay1 = (VB.Val(strDay1) + 1).ToString();

                    if (VB.Val(strDay1) > 7)
                    {
                        strDay1 = "1";
                    }
                }

                dt.Dispose();
                dt = null;

                
                //'해당 부서에 해당하는 직원을 DisPlay
                strBUSE = READ_BuseCode(Combodept.Text);
                SQL = "";
                SQL = "SELECT YYMM,WARDCODE, A.SABUN, SNAME,JIKCODE,SCHEDULE,DTOT,";
                SQL = SQL + ComNum.VBLF + " ETOT,NTOT,OFFTOT, A.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_SCHEDULE1 A";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_ADM.INSA_MST B";
                SQL = SQL + ComNum.VBLF + "      ON A.SABUN = B.SABUN";
                SQL = SQL + ComNum.VBLF + " WHERE WardCode = '" + strBUSE + "'  ";
                //SQL = SQL + ComNum.VBLF + "   AND B.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND (B.TOIDAY IS NULL OR TOIDAY >= TO_DATE('"+ VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2) + "01','YYYY-MM-DD')) ";
                //SQL = SQL + ComNum.VBLF + "  AND A.SABUN = '43376'  ";
                if (Combodept.Text == "치료사(ICU)")
                {
                    SQL = SQL + ComNum.VBLF + "   AND JIKCODE IN ('91') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN NOT IN ('14015','18515') ";
                }
                else if (Combodept.Text == "ER응급구조사")
                {
                    SQL = SQL + ComNum.VBLF + "   AND JIKCODE IN ('59') ";
                    //SQL = SQL + ComNum.VBLF + "   AND A.SABUN NOT IN ('14015','18515') ";
                }

                SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2) + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY JikCode, Sabun ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = i + 3;
                    if (nRow > SS1_Sheet1.RowCount)
                    {
                        SS1_Sheet1.RowCount = nRow;
                    }

                    SS1_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 1].Text = READ_JikName(dt.Rows[i]["JikCode"].ToString().Trim());

                    nCol = 3;
                    nTEMP = 1;
                    for (J = nDateH1; J <= nDateH2; J++)
                    {
                        //SS1_Sheet1.Cells[nRow - 1, nCol - 1].Text = ComFunc.MidH(dt.Rows[i]["Schedule"].ToString().Trim(), nTEMP, 4).Trim();
                        SS1_Sheet1.Cells[nRow - 1, nCol - 1].Text = ComFunc.MidH(dt.Rows[i]["Schedule"].ToString(), nTEMP, 4).Trim();
                        nCol = nCol + 1;
                        nTEMP = nTEMP + 4;
                    }

                    //'월이 30일인 경우
                    if (nDateH2 == 30)
                    {
                        SS1_Sheet1.Cells[nRow - 1, 32].Text = VB.Space(4);
                        nCol = nCol + 1;
                    }
                }

                SS1_Sheet1.RowCount = nRow;
                SS1_Sheet1.SetRowHeight(1, 2);

                // '휴일인 경우에 Color DisPlay
                SQL = "";
                nCol3 = 2;

                SQL = "Select TO_CHAR(JobDate,'YYYY-MM-DD') JOBDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_JOB ";
                SQL = SQL + ComNum.VBLF + " WHERE HolyDay = '*' ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate >= TO_DATE('" + strDateH1 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JobDate <= TO_DATE('" + strDateH2 + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    nHDay = Convert.ToInt32(VB.Right(dt1.Rows[i]["JOBDATE"].ToString().Trim(), 2));

                    SS1_Sheet1.Cells[0, (nCol3 + nHDay) - 1, 0, (nCol3 + nHDay) - 1].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    SS1_Sheet1.Cells[2, (nCol3 + nHDay) - 1, SS1_Sheet1.RowCount - 1, (nCol3 + nHDay) - 1].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }

                dt1.Dispose();
                dt1 = null;

                btnCancel.Enabled = true;
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

            if (chkChange.Checked == true)
            {
                for (i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    for (J = 0; J < SS1_Sheet1.ColumnCount; J++)
                    {
                        switch (SS1_Sheet1.Cells[i, J].Text.Trim())
                        {
                            case "D1":
                                SS1_Sheet1.Cells[i, J].Text = "D";
                                break;
                            case "E1":
                            case "EM":
                                SS1_Sheet1.Cells[i, J].Text = "E";
                                break;
                            case "N1":
                                SS1_Sheet1.Cells[i, J].Text = "N";
                                break;
                            case "SH":
                                SS1_Sheet1.Cells[i, J].Text = "S";
                                break;
                            case "D/L1":
                                SS1_Sheet1.Cells[i, J].Text = "D/L";
                                break;
                            case "E/L1":
                                SS1_Sheet1.Cells[i, J].Text = "E/L";
                                break;
                            case "N/L1":
                                SS1_Sheet1.Cells[i, J].Text = "N/L";
                                break;
                            case "S1":
                                SS1_Sheet1.Cells[i, J].Text = "S";
                                break;
                            case "SP":
                                SS1_Sheet1.Cells[i, J].Text = "D";
                                break;
                            case "SP1":
                                SS1_Sheet1.Cells[i, J].Text = "H/D";
                                break;
                            case "SHD":
                                SS1_Sheet1.Cells[i, J].Text = "E";
                                break;
                            case "SCSR":
                                SS1_Sheet1.Cells[i, J].Text = "E";
                                break;
                        }
                    }
                }
            }

            btnPrint.Enabled = true;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strTitle = Combodept.Text + " 번표";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
    }
}
