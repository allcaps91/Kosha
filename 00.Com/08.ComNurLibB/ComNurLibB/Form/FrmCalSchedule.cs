using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class FrmCalSchedule : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string FstrCurDate;//현재일자
        string FstrGetSDate;//시작일자
        string FstrGetLDate;
        string FstrSelDate;

        int FnCheckRow;
        int FnCheckCol;

        //string FstrROWID = "";

        ComFunc CF = new ComFunc();
        clsSpread SP = new clsSpread();

        public FrmCalSchedule()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            SP.Spread_Clear(ssSchedule, 6, 7);
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            
            fn_Schedule_View(Convert.ToInt32(VB.Right(FstrGetLDate, 2)), Convert.ToInt32(Convert.ToDateTime(FstrGetSDate).DayOfWeek), VB.Right(clsPublic.GstrSysDate, 2));

           

       
        }

        private void FrmMedDeptSchedule_Load(object sender, EventArgs e)
        {
           

        }

        
        /// <summary>
        /// 달력 그리기
        /// </summary>
        /// <param name="sLastDD" 해당월 말일></param>
        /// <param name="sStartCol" 요일></param>
        /// <param name="sSysDate" 현재일자></param>
        void fn_Schedule_View(int sLastDD, int sStartCol, string sSysDate)
        {
            int nCol, nRow;
            string cmmm;

            ssSchedule_Sheet1.RowCount = 6;

            nRow = 0;
            nCol = sStartCol;

            for (int i = 1; i <= sLastDD; i++)
            {
                ssSchedule.ActiveSheet.Cells[nRow, nCol].Text = string.Format("{0:00}", i);

                switch (nCol)
                {
                    case 0:
                        ssSchedule.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.Red;
                        break;
                    case 6:
                        ssSchedule.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.FromArgb(40, 30, 230);
                        break;
                    default:
                        if (VB.IsNumeric(ssSchedule.ActiveSheet.Cells[nRow, nCol].Text))
                        {
                            ssSchedule.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.FromArgb(0, 0, 0);
                        }
                        break;
                }

                if (int.Parse(ssSchedule.ActiveSheet.Cells[nRow, nCol].Text) == int.Parse(sSysDate))
                {
                    FnCheckCol = nCol;
                    FnCheckRow = nRow;
                }

                cmmm = VB.Mid(FstrGetSDate, 6, 2);

                try
                {
                    SQL = "";
                    SQL += " Select * From KOSMOS_PMPA.BAS_JOB        \r";
                    SQL += "  Where JOBdate = to_date('" + lblYear.Text.Trim() + "-" + cmmm + "-" + string.Format("{0:00}", ssSchedule.ActiveSheet.Cells[nRow, nCol].Text) + "','yyyy-MM-dd')   \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["HOLYDAY"].ToString() == "*")
                        {
                            ssSchedule.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.Red;
                        }

                        nCol += 1;
                        if (nCol > 6)
                        {
                            nRow += 1;
                            nCol = 0;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (OracleException ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
                       
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            string strGetSdate;
            string strGetLdate;
            int nLastDD;
            int nWeek;

            //'CmdReservedSearch.Enabled = True
          
            strGetLdate = DateTime.Parse(FstrGetSDate).AddMonths(-1).ToShortDateString();
            strGetSdate = VB.Mid(strGetLdate, 1, 8) + "01";
            nLastDD = LastDD_Gesan(strGetSdate);
            strGetLdate = VB.Mid(strGetSdate, 1, 8) + string.Format("{0:00}", nLastDD);
            //nWeek = clsVbfunc.GetYoIl(strGetSdate);
            nWeek = Convert.ToInt32(Convert.ToDateTime(strGetSdate).DayOfWeek);
            lblYear.Text = VB.Left(strGetSdate, 4); //Year(Now)
            lblMonth.Text = VB.Mid(strGetSdate, 6, 2); //Month(Now)
            FstrGetSDate = strGetSdate;
            FstrGetLDate = strGetLdate;

            SP.Spread_Clear(ssSchedule, 6, 7);
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.White;

            //fn_Schedule_View(VB.Right(FstrGetLDate, 2), Convert.ToInt32(Convert.ToDateTime(FstrGetSDate).DayOfWeek), VB.Right(clsPublic.GstrSysDate, 2));
            fn_Schedule_View(nLastDD, nWeek, VB.Right(clsPublic.GstrSysDate, 2));

           
        }

        int LastDD_Gesan(string strSDate)
        {
            string c_day;

            if (int.Parse(VB.Left(strSDate, 4)) % 4 == 0)
            {
                c_day = "312931303130313130313031";
            }
            else
            {
                c_day = "312831303130313130313031";
            }

            return int.Parse(string.Format("{0:00}", VB.Mid(c_day, int.Parse(VB.Mid(strSDate, 6, 2)) * 2 - 1, 2)));
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            string strGetSdate;
            string strGetLdate;
            int nLastDD;
            int nWeek;

            //'CmdReservedSearch.Enabled = True
           
            strGetSdate = DateTime.Parse(FstrGetSDate).AddMonths(1).ToShortDateString();
            nLastDD = LastDD_Gesan(strGetSdate);
            strGetLdate = VB.Mid(strGetSdate, 1, 8) + string.Format("{0:00}", nLastDD);
            //nWeek = clsVbfunc.GetYoIl(strGetSdate);
            nWeek = Convert.ToInt32(Convert.ToDateTime(strGetSdate).DayOfWeek);
            lblYear.Text = VB.Left(strGetSdate, 4); //Year(Now)
            lblMonth.Text = VB.Mid(strGetSdate, 6, 2); //Month(Now)
            FstrGetSDate = strGetSdate;
            FstrGetLDate = strGetLdate;

            SP.Spread_Clear(ssSchedule, 6, 7);
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.White;

            //fn_Schedule_View(VB.Right(FstrGetLDate, 2), Convert.ToInt32(Convert.ToDateTime(FstrGetSDate).DayOfWeek), VB.Right(clsPublic.GstrSysDate, 2));
            fn_Schedule_View(nLastDD, nWeek, VB.Right(clsPublic.GstrSysDate, 2));

           
        }

       

        private void ssSchedule_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            string strGetSdate = "";
            int nDay;

            if (e.ColumnHeader == true) return;

            if (VB.IsNull(ssSchedule.ActiveSheet.Cells[e.Row, e.Column].Text) || ssSchedule.ActiveSheet.Cells[e.Row, e.Column].Text.Trim() == "")
            {
                return;
            }

            strGetSdate = VB.Mid(string.Format("{0:yyyy-mm-dd}", FstrGetSDate), 1, 8) + "01";

            nDay = Convert.ToInt32(VB.Mid(ssSchedule.ActiveSheet.Cells[e.Row, e.Column].Text, 0, 2));
            FstrSelDate = VB.Mid(strGetSdate, 1, 8) + string.Format("{0:00}", nDay);

            clsPublic.GstrCalDate = FstrSelDate;

            this.Dispose();

            //try
            //{
            //    SQL = "";
            //    SQL += " SELECT '' chk, STIME, REMARK, ROWID                                \r";
            //    SQL += "   FROM KOSMOS_PMPA.BAS_SCHEDULE_DEPT  A                            \r";
            //    SQL += "  WHERE A.SCHDate = TO_DATE('" + FstrSelDate + "','YYYY-MM-DD')     \r";
            //    SQL += "    AND A.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'             \r";
            //    SQL += "  ORDER BY STIME                                                    \r";
            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        return;
            //    }

            //    SP.Spread_All_Clear(ssDtl);

            //    ssDtl.ActiveSheet.RowCount = dt.Rows.Count + 10;

            //    if (dt.Rows.Count > 0)
            //    {
            //        clsDB.DataTableToSpdRow(dt, ssDtl, 0, true);
            //    }
            //    else
            //    {
            //        pnlDtl.Visible = true;
            //        dt.Dispose();
            //        dt = null;
            //        return;
            //    }

            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //}
        }

        
        private void FrmCalSchedule_Load(object sender, EventArgs e)
        {
            string strCurDate;
            string strGetSdate;
            string strGetLdate;



            this.Location = new Point(1, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);
            FstrGetSDate = clsPublic.GstrSysDate;
            
            

            strCurDate = string.Format("{0:yyyy-MM-dd}", clsPublic.GstrSysDate);
            strGetSdate = VB.Mid(strCurDate, 1, 8) + "01";
            strGetLdate = CF.READ_LASTDAY(clsDB.DbCon, strGetSdate);

            FstrCurDate = strCurDate;            //현재일자
            FstrGetSDate = strGetSdate;          //시작일자
            FstrGetLDate = strGetLdate;          //종료일자

            lblYear.Text = VB.Left(clsPublic.GstrSysDate, 4); //Year(Now)
            lblMonth.Text = VB.Mid(clsPublic.GstrSysDate, 6, 2); //Month(Now)

            SP.Spread_Clear(ssSchedule, 6, 7);
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
            ssSchedule.ActiveSheet.Cells[0, 0, ssSchedule.ActiveSheet.RowCount - 1, ssSchedule.ActiveSheet.ColumnCount - 1].BackColor = Color.White;

            fn_Schedule_View(Convert.ToInt32(VB.Right(FstrGetLDate, 2)), Convert.ToInt32(Convert.ToDateTime(FstrGetSDate).DayOfWeek), VB.Right(clsPublic.GstrSysDate, 2));

           
        }

        private void btnColse_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
