using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;
using FarPoint.Win.Spread;

namespace ComNurLibB
{

    public partial class frmNurseNightTong2 : Form, MainFormMessage
    {

        #region //MainFormMessage

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }

        public void MsgUnloadForm(Form frm)
        {
        }

        public void MsgFormClear()
        {
        }

        public void MsgSendPara(string strPara)
        {
        }

        #endregion

        public frmNurseNightTong2()
        {
            InitializeComponent();
        }

        public frmNurseNightTong2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            Search();
        }

         void Search()
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            ClearSpread();

            int i = 0;
            int j = 0;

            int nCol = 0;

            string strSYYMM1 = "";
            string strEYYMM1 = "";
            string strSYYMM2 = "";
            string strEYYMM2 = "";
            string strSYYMM3 = "";
            string strEYYMM3 = "";

            string strWard = "";

            //FarPoint.Win.Spread.Column col;

            ComFunc cf = new ComFunc();

            if (cboYear.Text.Trim() == "" || cbobun.Text.Trim() == "")
            {
                //MessageBox.Show("조회기간을 확인하시기 바랍니다.", "오류", MessageBoxIcon.Information);
                return;
            }

            strSYYMM1 = cboYear.Text.Replace("년도", "");
            strEYYMM1 = cboYear.Text.Replace("년도", "");
            strSYYMM2 = cboYear.Text.Replace("년도", "");
            strEYYMM2 = cboYear.Text.Replace("년도", "");
            strSYYMM3 = cboYear.Text.Replace("년도", "");
            strEYYMM3 = cboYear.Text.Replace("년도", "");

            #region 조회기간 설정
            switch (cbobun.Text.Replace("분기", ""))
            {
                case "1/4":
                    strSYYMM1 = (Int32.Parse(strSYYMM1) - 1).ToString();
                    strSYYMM1 += "-12-15";
                    strEYYMM1 += "-01-14";
                    strSYYMM2 += "-01-15";
                    strEYYMM2 += "-02-14";
                    strSYYMM3 += "-02-15";
                    strEYYMM3 += "-03-14";
                    break;
                case "2/4":
                    strSYYMM1 += "-03-15";
                    strEYYMM1 += "-04-14";
                    strSYYMM2 += "-04-15";
                    strEYYMM2 += "-05-14";
                    strSYYMM3 += "-05-15";
                    strEYYMM3 += "-06-14";
                    break;
                case "3/4":
                    strSYYMM1 += "-06-15";
                    strEYYMM1 += "-07-14";
                    strSYYMM2 += "-07-15";
                    strEYYMM2 += "-08-14";
                    strSYYMM3 += "-08-15";
                    strEYYMM3 += "-09-14";
                    break;
                case "4/4":
                    strSYYMM1 += "-09-15";
                    strEYYMM1 += "-10-14";
                    strSYYMM2 += "-10-15";
                    strEYYMM2 += "-11-14";
                    strSYYMM3 += "-11-15";
                    strEYYMM3 += "-12-14";
                    break;
            }
            #endregion

            for (i = 1; i <= 3; i++)
            {
                for (j = 4; j <= 14; j++)
                {
                    strWard = SS1_Sheet1.Cells[j, 2].Text.Trim();
                    switch (i)
                    {
                        case 1:
                            nCol = 5;
                            SS1_Sheet1.Cells[1, nCol-1].Text = VB.Left(strEYYMM1, 4) + "년 " + VB.Mid(strEYYMM1, 6, 2) + "월";
                            SS1_Sheet1.Cells[2, nCol - 1].Text = strSYYMM1 + " ~ " + strEYYMM1;
                            SS1_Sheet1.Cells[j, nCol-1].Text = ReadScheduleTong_inwon(strSYYMM1, strEYYMM1, strWard);
                            SS1_Sheet1.Cells[j, nCol].Text = ReadScheduleTong(strSYYMM1, strEYYMM1, strWard);
                            break;
                        case 2:
                            nCol = 8;
                            SS1_Sheet1.Cells[1, nCol - 1].Text = VB.Left(strEYYMM2, 4) + "년 " + VB.Mid(strEYYMM2, 6, 2) + "월";
                            SS1_Sheet1.Cells[2, nCol - 1].Text = strSYYMM2 + " ~ " + strEYYMM2;
                            SS1_Sheet1.Cells[j, nCol-1].Text = ReadScheduleTong_inwon(strSYYMM2, strEYYMM2, strWard);
                            SS1_Sheet1.Cells[j, nCol].Text = ReadScheduleTong(strSYYMM2, strEYYMM2, strWard);
                            break;
                        case 3:
                            nCol = 11;
                            SS1_Sheet1.Cells[1, nCol - 1].Text = VB.Left(strEYYMM3, 4) + "년 " + VB.Mid(strEYYMM3, 6, 2) + "월";
                            SS1_Sheet1.Cells[2, nCol - 1].Text = strSYYMM3 + " ~ " + strEYYMM3;
                            SS1_Sheet1.Cells[j, nCol-1].Text = ReadScheduleTong_inwon(strSYYMM3, strEYYMM3, strWard);
                            SS1_Sheet1.Cells[j, nCol].Text = ReadScheduleTong(strSYYMM3, strEYYMM3, strWard);
                            break;
                    }

                }

            }

            ViewMemo(cboYear.Text, cbobun.Text);
            calc();
        }


        private string ReadScheduleTong(string argSDate, string argEDate, string argWard)
        {
            string strSche = "";    //스케쥴
            int nCnt = 0;
            int i = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            MatchCollection matches;

            ComFunc cf = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = " SELECT SUBSTRB(A.SCHEDULE, 57, LENGTH(SCHEDULE)) SCH1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(argSDate.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + argWard + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSche = dt.Rows[i]["SCH1"].ToString();
                    matches = Regex.Matches(strSche, "N1");
                    nCnt += matches.Count;
                }
                dt.Dispose();
                dt = null;

                SQL = " SELECT SUBSTRB(A.SCHEDULE, 1, 56) SCH1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(argEDate.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + argWard + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSche = dt.Rows[i]["SCH1"].ToString();
                    matches = Regex.Matches(strSche, "N1");
                    nCnt += matches.Count;
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return nCnt.ToString();
                
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
                return "";
            }

        }
        private string ReadScheduleTong_inwon(string argSDate, string argEDate, string argWard)
        {
            string strSche = "";    //스케쥴
            int nCnt = 0;
            int i = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            MatchCollection matches;

            ComFunc cf = new ComFunc();

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = " SELECT SUBSTRB(A.SCHEDULE, 57, LENGTH(SCHEDULE)) SCH1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(argSDate.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + argWard + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSche = dt.Rows[i]["SCH1"].ToString();
                    matches = Regex.Matches(strSche, "N1");
                    if ( matches.Count > 0 )
                    {
                        nCnt += 1;
                    }
                    
                }
                dt.Dispose();
                dt = null;

                SQL = " SELECT SUBSTRB(A.SCHEDULE, 1, 56) SCH1";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(argEDate.Replace("-", ""), 6) + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + argWard + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSche = dt.Rows[i]["SCH1"].ToString();
                    matches = Regex.Matches(strSche, "N1");
                    if (matches.Count > 0)
                    {
                        nCnt += 1;
                    }
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                return nCnt.ToString();

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
                return "";
            }

        }

        private void frmNurseNightTong_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            cboYear.Items.Clear();
            cboYear.Items.Add("2021년도");
            cboYear.Items.Add("2020년도");
            cboYear.Items.Add("2019년도");
            cboYear.Items.Add("2018년도");
            cboYear.Items.Add("2017년도");
            cboYear.SelectedIndex = 0;

            cbobun.Items.Clear();
            cbobun.Items.Add("1/4분기");
            cbobun.Items.Add("2/4분기");
            cbobun.Items.Add("3/4분기");
            cbobun.Items.Add("4/4분기");
            cbobun.SelectedIndex = -1;

            ClearSpread();
        }

        private void ClearSpread()
        {
            SS1_Sheet1.Cells[4, 3, 16, 9].Text = "";
            SS1_Sheet1.Cells[19, 2].Text = "";
            //TxtMemo.Text = "";
        }


        private void ViewMemo(string argYear, string argBungi)
        {
            int i = 0;
            //int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strYear = "";
            string strBungi = "";

            strYear = argYear.Replace("년도", "").Trim();
            strBungi = argBungi.Replace("분기", "").Trim();

            SQL = " SELECT MEMO ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_NIGHTTONG_MEMO ";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + strYear + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BUNGI = '" + strBungi + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                SS1_Sheet1.Cells[19,2].Text = dt.Rows[i]["MEMO"].ToString();
                //TxtMemo.Text = dt.Rows[i]["MEMO"].ToString();
            }

            dt.Dispose();
            dt = null;
        }

        private void save()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            
            string strYear = "";
            string strBungi = "";
            string strMemo = "";

            strYear = cboYear.Text.Replace("년도", "").Trim();
            strBungi = cbobun.Text.Replace("분기", "").Trim();
            //strMemo = TxtMemo.Text.Trim();
            strMemo = SS1_Sheet1.Cells[19, 2].Text.Trim();

            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE KOSMOS_PMPA.NUR_NIGHTTONG_MEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + strYear + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUNGI = '" + strBungi + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " INSERT INTO KOSMOS_PMPA.NUR_NIGHTTONG_MEMO(YEAR, BUNGI, MEMO) VALUES(";
                SQL = SQL + ComNum.VBLF + "'" + strYear + "','" + strBungi + "','" + strMemo + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }
        private void calc()
        {
            int i = 0;
            int j = 0;
            int nTot = 0;

            for (i = 4; i < 15; i++)
            {
                nTot = 0;
                for (j = 4; j < SS1.ActiveSheet.ColumnCount - 2; j++)
                {
                    if (SS1.ActiveSheet.Cells[i, j].Text.Trim() != "")
                    {
                        nTot = nTot + Int32.Parse(SS1.ActiveSheet.Cells[i, j].Text.Trim());
                    }
                }
                SS1.ActiveSheet.Cells[i, 3].Text = nTot.ToString();
            }

            for (i = 3; i < SS1.ActiveSheet.ColumnCount - 1 ; i++)
            {
                nTot = 0;
                for (j = 4; j < 15; j++)
                {
                    if (SS1.ActiveSheet.Cells[j, i].Text.Trim() != "")
                    {
                        nTot = nTot + Int32.Parse(SS1.ActiveSheet.Cells[j, i].Text.Trim());
                    }

                }
                SS1.ActiveSheet.Cells[15, i].Text = nTot.ToString();
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";

            bool PrePrint = false;


            strTitle = cboYear.Text + " " + cbobun.Text + " 월별 간호사 야간근무 통계" + "\r\n";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 80, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            SPR.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

            btnPrint.Enabled = true;

        }

        private void cbobun_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }
    }
}
