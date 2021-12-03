using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmEmergencyRoomDuty : Form
    {
        public frmEmergencyRoomDuty()
        {
            InitializeComponent();
        }

        private void frmEmergencyRoomDuty_Load(object sender, EventArgs e)
        {            
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate).AddMonths( + 1);

            cboYYMM.Items.Clear();
            for(int i = 0; i < 26; i++)
            {
                cboYYMM.Items.Add(DT.Year + "년 " + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "월");
                DT = DT.AddMonths(-1);
            }
            cboYYMM.SelectedIndex = 0;
        }

        void SS1_Clear()
        {
            ss1_Sheet1.Cells[0, 0, 24, 6].Text = "";
        }

        void YYMM_DISPLAYER()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            DateTime DT = Convert.ToDateTime(VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01");
            string strDate = DT.ToShortDateString();
            int nDD = DateTime.DaysInMonth(DT.Year, DT.Month);

            int j = 1;
            int nCol = 0;
            string strWeek = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for(int i = 1; i < nDD + 1; i++)
                {
                    SQL = "Select TO_CHAR(TO_DATE('" + VB.Left(strDate, 8) + ComFunc.SetAutoZero(i.ToString(), 2) + "', 'YYYY-MM-DD'),'DY') cWeek ";
                    SQL += ComNum.VBLF + " FROM DUAL ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strWeek = dt.Rows[0]["cWeek"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    switch(strWeek)
                    {
                        case "MON":
                        case  "월": nCol = 0;          //  '월
                                break;
                        case "TUE":
                        case "화": nCol = 1;          // '화
                            break;
                        case "WED":
                        case "수": nCol = 2;          // '수
                            break;
                        case "THU":
                        case "목": nCol = 3;          // '목
                            break;
                        case "FRI":
                        case "금": nCol = 4;          //  '금"
                            break;
                        case "SAT":
                        case "토": nCol = 5;          // '토
                            break;
                        case "SUN":
                        case "일": nCol = 6;          // '일
                            break;
                    }

                    ss1_Sheet1.Cells[j, nCol].Text = i.ToString();
                    if (nCol == 6) j = j + 4;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            Save_Data();
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            DateTime DT = Convert.ToDateTime(VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2));
            string strTDate = DT.Year + "-" + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "-01";
            string strYYMM = VB.Left(strTDate, 8);
            string strFDate = DT.Year + "-" + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "-" +  DateTime.DaysInMonth(DT.Year, DT.Month);
            int n = 0;
            int m = 0;

            string[] strName = new string[4];

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " DELETE KOSMOS_PMPA.ETC_DANGJIK ";
                SQL += ComNum.VBLF + " WHERE TDATE >= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND TDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND GUBUN = '11' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                n = 23;
                if (ss1_Sheet1.RowCount == 20) n = 20;

                if(ComFunc.MsgBoxQ("당직표를 등록 하시겠습니까?") == DialogResult.Yes)
                {
                    for(int j = 0; j < 7; j++)
                    {
                        for(int k = 1; k < n; k = k + 3)
                        {
                            m = (int) VB.Val(ss1_Sheet1.Cells[k, j].Text);
                            if (m > 0)
                            {
                                strName[0] = ss1_Sheet1.Cells[k + 1, j].Text.Trim();
                                strName[1] = ss1_Sheet1.Cells[k + 2, j].Text.Trim();
                                strName[2] = ss1_Sheet1.Cells[k + 3, j].Text.Trim();

                                SQL = "INSERT INTO KOSMOS_PMPA.ETC_DANGJIK(TDATE,GUBUN, DNAME1, ";
                                SQL += ComNum.VBLF + " DNAME2, DNAME3, DNAME4) VALUES ";
                                SQL += ComNum.VBLF + " (TO_DATE('" + strYYMM + ComFunc.SetAutoZero(m.ToString(), 2) + "','YYYY-MM-DD'), ";
                                SQL += ComNum.VBLF + " '11', '" + strName[0] + "', '" + strName[1] + "', ";
                                SQL += ComNum.VBLF + " '" + strName[2] + "', '') ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {            
            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SS1_Clear();
            YYMM_DISPLAYER();

            DateTime DT = Convert.ToDateTime(VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2));
            string strTDate = "";
            string strFDate = "";

            int j = 0;
            int k = 0;
            int m = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (DT >= Convert.ToDateTime("2013-08-09"))
                {
                    ss1_Sheet1.Cells[25, 0].Text = "D 근무 : 09:01 ∼ 17:00     E 근무 : 17:01 ∼ 23:00     N 근무 : 23:01 ∼ 09:00";
                }
                else if (DT <= Convert.ToDateTime("2006-03"))
                {
                    ss1_Sheet1.Cells[25, 0].Text = "D 근무 : 08:01 ∼ 18:00     N 근무 : 18:01 ∼ 08:00";
                }
                else if (DT >= Convert.ToDateTime("2009-01"))
                {
                    ss1_Sheet1.Cells[25, 0].Text = "D 근무 : 09:01 ∼ 19:00     N 근무 : 19:01 ∼ 09:00";
                }
                else if (DT >= Convert.ToDateTime("2006-10"))
                {
                    ss1_Sheet1.Cells[25, 0].Text = "D 근무 : 08:01 ∼ 18:00     N 근무 : 18:01 ∼ 08:00";
                }
                else
                {
                    ss1_Sheet1.Cells[25, 0].Text = "D 근무 : 07:01 ∼ 15:00     E 근무 : 15:01 ∼ 23:00     N 근무 : 23:01 ∼ 07:00";
                }

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;

                strTDate = DT.Year + "-" + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "-01";    //'시작일자
                strFDate = DT.Year + "-" + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "-" + DateTime.DaysInMonth(DT.Year, DT.Month);  //'마지막일자

                //'Data select함
                SQL = " SELECT TO_CHAR(TDATE,'DD') TDATE, DNAME1, DNAME2, DNAME3, DNAME4, ROWID FROM ETC_DANGJIK ";
                SQL += ComNum.VBLF + " WHERE TDATE >= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND TDATE <= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND GUBUN = '11' ";  //'응급실 당직
                SQL += ComNum.VBLF + " ORDER BY TDATE ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

                for(j = 0; j < 7; j++)
                {
                    for(k = 1; k < 23; k = k + 4)
                    {
                        m = (int)VB.Val(ss1_Sheet1.Cells[k, j].Text);
                        if(m > 0)
                        {
                            ss1_Sheet1.Cells[k + 1, j].Text = dt.Rows[m - 1]["DNAME1"].ToString().Trim();
                            ss1_Sheet1.Cells[k + 2, j].Text = dt.Rows[m - 1]["DNAME2"].ToString().Trim();
                            ss1_Sheet1.Cells[k + 3, j].Text = dt.Rows[m - 1]["DNAME3"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {            
            SETPRINT();
        }

        void SETPRINT()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";

            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboYYMM.Text + " 응급실 당직표";
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 20, 90, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
