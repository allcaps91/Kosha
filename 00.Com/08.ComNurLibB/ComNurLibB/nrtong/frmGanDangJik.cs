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
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrtong\nrtong02.frm >> frmGanDangJik.cs 폼이름 재정의" />

    public partial class frmGanDangJik : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmGanDangJik()
        {
            InitializeComponent();
        }

        private void Clear()
        {

            SS1_Sheet1.RowCount = 0;

        }

        private string READ_Sname(string Arg1)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";
            if (Arg1 == "")
            {
                return strVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT KorName ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE Sabun = '" + Arg1.PadLeft(5, '0') + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return Arg1;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return Arg1;
                }

                strVal = dt.Rows[0]["Korname"].ToString().Trim();

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            btnCancel.Enabled = false;
            btnSearch.Enabled = true;
            btnExit.Enabled = true;
            btnPrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strTitle = "간호부 당직 Schedule";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : " + ComboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            //int J = 0;
            //int K = 0;
            string strDate = "";           //'날짜 비교
            string strDay = "";           //'월의 처음 날짜
            string strHDay = "";           //'월의 처음 날짜 (휴일 Check)
            string strDay1 = "";           //'월의 처음 요일
            int nHDay = 0;       //'휴일인 날짜
            string strYY = "";
            string strMM = "";
            string strDateH1 = "";
            string strDateH2 = "";
            string nDateH1 = "";
            string nDateH2 = "";
            int nRow2 = 0;
            int nRow3 = 0;

            strYY = VB.Left(ComboYYMM.Text, 4);
            strMM = VB.Mid(ComboYYMM.Text, 6, 2);
            strDay = strYY + "-" + strMM + "01";

            strHDay = strYY + "-" + strMM + "-01";
            strDate = strYY + "-" + strMM + "-01";
            strDateH1 = strDate;
            strDate = strYY + "-" + strMM + "-01";

            strDateH2 = CF.READ_LASTDAY(clsDB.DbCon, strDate);

            nDateH1 = VB.Val(VB.Right(strDateH1, 2)).ToString();
            nDateH2 = VB.Val(VB.Right(strDateH2, 2)).ToString();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // '자료를 DisPlay
                SQL = "";
                SQL = "SELECT to_char(ActDate,'yyyy-mm-dd') actdate,DDuty,EDuty,An,Op1,OP2,OP3,HD ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_SCHEDULE2 ";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate >= TO_DATE('" + strDateH1 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ANd ActDate <= TO_DATE('" + strDateH2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ActDate,DDuty,EDuty,An,Op1,Op2,Op3,HD ";

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

                SS1_Sheet1.Rows.Count = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 2].Text = READ_Sname(dt.Rows[i]["DDuty"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 3].Text = READ_Sname(dt.Rows[i]["EDuty"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 5].Text = READ_Sname(dt.Rows[i]["An"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 6].Text = READ_Sname(dt.Rows[i]["Op1"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 7].Text = READ_Sname(dt.Rows[i]["Op2"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 8].Text = READ_Sname(dt.Rows[i]["Op3"].ToString().Trim());
                    SS1_Sheet1.Cells[Convert.ToInt32(VB.Right(dt.Rows[i]["Actdate"].ToString().Trim(), 2)) - 1, 10].Text = READ_Sname(dt.Rows[i]["HD"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                if (nDateH2 == "28")
                {
                    SS1_Sheet1.Rows[27].Visible = true;
                    //SS1_Sheet1.Rows[28].Visible = false;
                    //SS1_Sheet1.Rows[29].Visible = false;
                    //SS1_Sheet1.Rows[30].Visible = false;
                }
                else if (nDateH2 == "29")
                {
                    SS1_Sheet1.Rows[27].Visible = true;
                    SS1_Sheet1.Rows[28].Visible = true;
                    //SS1_Sheet1.Rows[29].Visible = false;
                    //SS1_Sheet1.Rows[30].Visible = false;
                }
                else if (nDateH2 == "30")
                {
                    SS1_Sheet1.Rows[27].Visible = true;
                    SS1_Sheet1.Rows[28].Visible = true;
                    SS1_Sheet1.Rows[29].Visible = true;
                    //SS1_Sheet1.Rows[30].Visible = false;
                }
                else if (nDateH2 == "31")
                {
                    SS1_Sheet1.Rows[27].Visible = true;
                    SS1_Sheet1.Rows[28].Visible = true;
                    SS1_Sheet1.Rows[29].Visible = true;
                    SS1_Sheet1.Rows[30].Visible = true;
                }
                //월의 요일 Check
                nRow2 = 1;

                SQL = "";
                SQL = "SELECT TO_CHAR(TO_DATE('" + strDay + "', 'YYYY-MM-DD'),'DY') cWeek FROM DUAL ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    strDay1 = dt1.Rows[0]["cWeek"].ToString().Trim();
                    switch (strDay1.ToUpper())
                    {
                        case "SUN":
                            strDay1 = "1";
                            break;
                        case "MON":
                            strDay1 = "2";
                            break;
                        case "TUE":
                            strDay1 = "3";
                            break;
                        case "WED":
                            strDay1 = "4";
                            break;
                        case "THU":
                            strDay1 = "5";
                            break;
                        case "FRI":
                            strDay1 = "6";
                            break;
                        case "SAT":
                            strDay1 = "7";
                            break;
                    }
                }

                dt1.Dispose();
                dt1 = null;

                for (i = Convert.ToInt32(nDateH1); i <= Convert.ToInt32(nDateH2); i++)
                {
                    SS1_Sheet1.Cells[nRow2 - 1, 0].Text = i.ToString();

                    switch (strDay1)
                    {
                        case "1":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "일";
                            break;
                        case "2":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "월";
                            break;
                        case "3":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "화";
                            break;
                        case "4":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "수";
                            break;
                        case "5":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "목";
                            break;
                        case "6":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "금";
                            break;
                        case "7":
                            SS1_Sheet1.Cells[nRow2 - 1, 1].Text = "토";
                            break;
                    }

                    nRow2 = nRow2 + 1;
                    strDay1 = (VB.Val(strDay1) + 1).ToString();

                    if (VB.Val(strDay1) > 7)
                    {
                        strDay1 = "1";
                    }
                }

                nRow3 = 0;
                SQL = "";
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
                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        nHDay = (int)VB.Val(VB.Right(dt1.Rows[i]["JobDate"].ToString().Trim(), 2));
                        //SS1_Sheet1.Cells[(nRow3 + nHDay) - 1, 0, (nRow3 + nHDay) - 1, SS1_Sheet1.ColumnCount - 1].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        SS1_Sheet1.Rows[(nRow3 + nHDay) - 1].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                }



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

        private void frmGanDangJik_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }
            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");


            clsVbfunc.SetCboDate(clsDB.DbCon, ComboYYMM, 12, "", "0");
        }
    }


}
