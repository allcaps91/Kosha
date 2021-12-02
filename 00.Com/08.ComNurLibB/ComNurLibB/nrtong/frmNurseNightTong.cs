using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComNurLibB
{

    public partial class frmNurseNightTong : Form, MainFormMessage
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

        public frmNurseNightTong()
        {
            InitializeComponent();
        }

        public frmNurseNightTong(MainFormMessage pform)
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

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strSDATE = "";   //시작일자
            string strEDATE = "";   //종료일자
            string strNDate = "";   //조회일자
            int i = 0;
            int j = 0;
            int k = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            int nDay = 0;

            int nToday = 0;

            //int nTot = 0;

            string strSYYMM = "";
            string strEYYMM = "";

            FarPoint.Win.Spread.Column col;

            ComFunc cf = new ComFunc();

            strSDATE = dtpSdate.Text.Trim();
            strEDATE = dtpEdate.Text.Trim();

            strSYYMM = strSDATE.Substring(0, 7).Replace("-", "");
            strEYYMM = strEDATE.Substring(0, 7).Replace("-", "");

            nDay = cf.DATE_ILSU(clsDB.DbCon, strEDATE, strSDATE);

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                #region 부서명 세팅
                SQL = " SELECT WARDCODE ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 ";
                SQL += ComNum.VBLF + " WHERE YYMM >= '" + strSYYMM + "' ";
                SQL += ComNum.VBLF + "   AND YYMM <= '" + strEYYMM + "' ";
                SQL += ComNum.VBLF + " GROUP BY WARDCODE ";
                SQL += ComNum.VBLF + " ORDER BY WARDCODE ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1.ActiveSheet.RowCount = dt.Rows.Count;

                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        SS1.ActiveSheet.Cells[j, 0].Text = dt.Rows[j]["WARDCODE"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

                SS1.ActiveSheet.ColumnCount = nDay + 2;

                for (i = 0; i <= nDay; i++)
                {
                    strNDate = cf.DATE_ADD(clsDB.DbCon, strSDATE, i);
                    nToday = Convert.ToInt32(VB.Right(strNDate, 2));
                    //기간별 조회 가능하도록 보완
                    SQL = " SELECT WARDCODE, SUM(1) CNT FROM (";
                    SQL = SQL + ComNum.VBLF + " SELECT A.WARDCODE, SUBSTRB(A.SCHEDULE, " + ((1 + ((nToday) * 4)) - 4).ToString() + ", 4) ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                    SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(strNDate.Replace("-", ""), 6) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                    SQL = SQL + ComNum.VBLF + "   AND TRIM(SUBSTRB(A.SCHEDULE, " + ((1 + ((nToday) * 4)) - 4).ToString() + ", 4)) = 'N1'";
                    SQL = SQL + ComNum.VBLF + " ) GROUP BY WARDCODE ORDER BY WARDCODE ";
                    //}
                    //else
                    //{
                    //    SQL = " SELECT WARDCODE, SUM(1) CNT FROM (";
                    //    SQL = SQL + ComNum.VBLF + " SELECT A.WARDCODE, SUBSTRB(A.SCHEDULE, " + ((1 + ((i + 1) * 4)) - 4).ToString() + ", 4) ";
                    //    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
                    //    SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + VB.Left(strNDate.Replace("-", ""), 6) + "'";
                    //    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
                    //    SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
                    //    SQL = SQL + ComNum.VBLF + "   AND TRIM(SUBSTRB(A.SCHEDULE, " + ((1 + ((i + 1) * 4)) - 4).ToString() + ", 4)) = 'N1'";
                    //    SQL = SQL + ComNum.VBLF + " ) GROUP BY WARDCODE ORDER BY WARDCODE ";
                    //}

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt.Rows.Count; j++)
                    {
                        for (k = 0; k < SS1.ActiveSheet.RowCount; k++)
                        {
                            if (SS1.ActiveSheet.Cells[k, 0].Text.Trim() == dt.Rows[j]["WARDCODE"].ToString().Trim())
                            {
                                break;
                            }
                        }

                        SS1.ActiveSheet.Cells[k, i + 1].Text = dt.Rows[j]["CNT"].ToString();
                    }
                    col = SS1.ActiveSheet.Columns[i + 1];
                    col.Label = strNDate;

                    SS1.ActiveSheet.SetColumnWidth(i + 1, 44);

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
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

            calc();
        }

        private void frmNurseNightTong_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            dtpSdate.Text = clsPublic.GstrSysDate;
            dtpEdate.Text = clsPublic.GstrSysDate;

            //dtpSdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            //dtpEdate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }


        private void ViewJikwon(string argBuse, string argDate)
        {
            int i = 0;
            int j = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strYYMM = "";

            ComFunc cf = new ComFunc();

            if (argDate == "부서별합계")
                return;

            strYYMM = argDate.Substring(0, 7).Replace("-", "");
            i = Int32.Parse(argDate.Substring(8, 2)) - 1;

            SS2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT A.WARDCODE, C.SABUN, C.KORNAME, TO_CHAR(C.KUNDAY,'YYYY-MM-DD') KUNDAY, TO_CHAR(C.TOIDAY,'YYYY-MM-DD') TOIDAY ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE1 A, KOSMOS_ADM.INSA_MST C";
            SQL = SQL + ComNum.VBLF + " WHERE A.YYMM = '" + strYYMM + "'";
            if (argBuse == "일자별합계")
            { }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND A.WARDCODE = '" + argBuse + "' ";
            }
            SQL = SQL + ComNum.VBLF + "   AND A.SABUN = C.SABUN";
            SQL = SQL + ComNum.VBLF + "   AND C.MYEN_CODE = '31'";
            SQL = SQL + ComNum.VBLF + "   AND TRIM(SUBSTRB(A.SCHEDULE, " + ((1 + ((i + 1) * 4)) - 4).ToString() + ", 4)) = 'N1'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY WARDCODE ASC, SABUN ASC ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return ;
            }
            if (dt.Rows.Count > 0)
            {
                SS2.ActiveSheet.RowCount = dt.Rows.Count;

                for (j = 0; j < dt.Rows.Count; j++)
                {
                    SS2.ActiveSheet.Cells[j, 0].Text = dt.Rows[j]["WARDCODE"].ToString();
                    SS2.ActiveSheet.Cells[j, 1].Text = dt.Rows[j]["SABUN"].ToString();
                    SS2.ActiveSheet.Cells[j, 2].Text = dt.Rows[j]["KORNAME"].ToString();
                    SS2.ActiveSheet.Cells[j, 3].Text = dt.Rows[j]["KUNDAY"].ToString();
                    SS2.ActiveSheet.Cells[j, 4].Text = dt.Rows[j]["TOIDAY"].ToString();
                }
            }

            dt.Dispose();
            dt = null;
        }


        private void calc()
        {
            int i = 0;
            int j = 0;
            int nTot = 0;
            //int nValue = 0;
            FarPoint.Win.Spread.Column col;
            //FarPoint.Win.Spread.Row row;

            SS1.ActiveSheet.RowCount = SS1.ActiveSheet.RowCount + 1;
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 0].Text = "일자별합계";

            for ( i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
            {
                nTot = 0;
                for (j = 0; j < SS1.ActiveSheet.RowCount - 1; j++)
                {
                    if (SS1.ActiveSheet.Cells[j, i].Text.Trim() != "")
                    {
                        nTot = nTot + Int32.Parse(SS1.ActiveSheet.Cells[j, i].Text.Trim());
                    }
                    
                }
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].Text = nTot.ToString();
            }

            
            SS1.ActiveSheet.ColumnCount = SS1.ActiveSheet.ColumnCount + 1;
            col = SS1.ActiveSheet.Columns[SS1.ActiveSheet.ColumnCount - 1];
            col.Label = "부서별합계";

            for (i = 0; i < SS1.ActiveSheet.RowCount ; i++)
            {
                nTot = 0;
                for (j = 1; j < SS1.ActiveSheet.ColumnCount - 1; j++)
                {
                    if (SS1.ActiveSheet.Cells[i, j].Text.Trim() != "")
                    {
                        nTot = nTot + Int32.Parse(SS1.ActiveSheet.Cells[i, j].Text.Trim());
                    }
                }
                SS1.ActiveSheet.Cells[i, SS1.ActiveSheet.ColumnCount - 1].Text = nTot.ToString();
            }

        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBuse = "";
            string strDate = "";
            FarPoint.Win.Spread.Column col;

            if (e.Row < 0 || e.Column < 1)
                return;

            strBuse = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();

            col = SS1.ActiveSheet.Columns[e.Column];

            strDate = col.Label.ToString();

            ViewJikwon(strBuse, strDate);
        }
    }
}
