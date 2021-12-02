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
    /// <seealso cref= "D:\psmh\nurse\nrtong\nrtong.vbp\nrtong19.frm >> frmBogosea.cs 폼이름 재정의" />

    public partial class frmBogosea : Form
    {
        //int nRow = 0;
        //int nCol = 0;
        //string strBackDate = "";
        int[] nMaxTotal = new int[13];
        string strWardCode = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmBogosea()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            dtpDate.Enabled = false;
            dtpDate.Select();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)

                if (OptView0.Checked == true)
                {
                    strTitle = "당일입원자(" + dtpDate.Text + ")";
                }

            if (OptView2.Checked == true)
            {
                strTitle = "당일퇴원자(" + dtpDate.Text + ")";
            }
            if (OptView1.Checked == true)
            {
                strTitle = "당일전실전과(" + dtpDate.Text + ")";
            }



            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            string strNowDate = "";
            string strNextDate = "";
            //string strTranData = "";
            //int strRow = 0;

            strNowDate = dtpDate.Text;
            strNextDate = CF.DATE_ADD(clsDB.DbCon, strNowDate, 1);

            SS1_Sheet1.RowCount = 0;
            //strRow = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (OptView3.Checked == true)
                {

                    #region Jewon_Display

                    //'재원자
                    //'NEW_IPD
                    if (strWardCode == "NR")
                    {
                        SQL = "";
                        SQL = "SELECT PANO , DEPTCODE, SEX, AGE, SNAME, ROOMCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE WARDCODE  IN ('IQ','NR')";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE <= '" + strNowDate + "'";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE < '" + strNextDate + "'";
                        SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSTS = '0' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY  ROOMCODE,PANO";
                    }
                    else if (strWardCode == "MICU")
                    {
                        SQL = "";
                        SQL = "SELECT A.PANO , A.DEPTCODE, A.SEX, A.AGE, A.SNAME, A.ROOMCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER A, NUR_JINDAN B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND A.INDATE <  TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBSTS  = '0' ";
                        SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE ='MICU'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY  A.ROOMCODE,A.PANO";
                    }
                    else if (strWardCode == "SICU")
                    {
                        SQL = "";
                        SQL = "SELECT A.PANO , A.DEPTCODE, A.SEX, A.AGE, A.SNAME, A.ROOMCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER A, NUR_JINDAN B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO(+)";
                        SQL = SQL + ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND A.INDATE <  TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBSTS = '0' ";
                        SQL = SQL + ComNum.VBLF + "   AND B.WARDCODE = 'SICU'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY  A.ROOMCODE,A.PANO";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "SELECT PANO , DEPTCODE, SEX, AGE, SNAME, ROOMCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE WARDCODE = '" + strWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + strNowDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND INDATE <  TO_DATE('" + strNextDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND GBSTS = '0' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY  ROOMCODE,PANO";
                    }

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

                    SS1_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        Application.DoEvents();
                    }

                    dt.Dispose();
                    dt = null;
                    #endregion

                    btnCancel.Enabled = true;
                    btnPrint.Enabled = true;
                    return;
                }

                SQL = "";
                SQL = "SELECT A.WARDCODE, A.PANO, B.SNAME, B.SEX, B.JUMIN1,B.JUMIN2, A.DEPT, A.ROOM,A.REMARK, A.DIAGNOSYS, A.ICUGBN,A.ROWID";
                SQL = SQL + " FROM NUR_JINDAN A, BAS_PATIENT B";
                SQL = SQL + " WHERE A.ACTDATE =TO_DATE('" + strNowDate + "','YYYY-MM-DD')";


                if (OptView0.Checked == true)
                    SQL = SQL + "   AND A.GUBUN ='I'";
                if (OptView1.Checked == true)
                    SQL = SQL + "   AND A.GUBUN ='T'";
                if (OptView2.Checked == true)

                    SQL = SQL + "   AND A.GUBUN ='O'";
                SQL = SQL + "   AND A.PANO = B.PANO";
                SQL = SQL + " ORDER BY  A.WARDCODE,A.ROOM,A.PANO";

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

                SS1_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = ComFunc.AgeCalcEx(dt.Rows[i]["Jumin1"].ToString().Trim() + dt.Rows[i]["Jumin2"].ToString().Trim(), strDTP).ToString();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPT"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Room"].ToString().Trim();

                    Application.DoEvents();

                    SQL = "";
                    SQL = " SELECT PATID FROM KOSMOS_EMR.EMR_TREATT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PATID = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND CLASS ='O'";
                    SQL = SQL + ComNum.VBLF + "   AND CHECKED ='1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[i, 9].Text = "SCAN";
                    }
                    dt1.Dispose();
                    dt1 = null;
                }

                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
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

        private void frmBogosea_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnPrint.Enabled = false;
            dtpDate.Value = Convert.ToDateTime(strDTP);

        }
    }
}
