using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWAllRoom.cs
    /// Description     : 전실전과조회 기록실
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "D:\psmh\mid\midout\midout.vbp\frm전실전과조회_기록실.frm(frm전실전과조회_기록실.frm)  >> frmPmPaVIEWAllRoom.cs 폼이름 재정의" />	

    public partial class frmPmPaVIEWAllRoom : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaMisu CPM = new clsPmpaMisu();

        string GstrPANO = "";

        string FstrPANO = "";
        int FnIPDNO = 0;
        string FstrInDate = "";
        string FstrOutDate = "";
        int fnRow = 0;

        public frmPmPaVIEWAllRoom(string strPANO)
        {
            GstrPANO = strPANO;

            InitializeComponent();
        }

        public frmPmPaVIEWAllRoom()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWAllRoom_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.RowCount = 13;

            ssList_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView_Sheet1.Columns[0].Visible = false;
            ssView_Sheet1.Columns[1].Visible = false;
            ssView_Sheet1.Columns[2].Visible = false;
            ssView_Sheet1.Columns[12].Visible = false;

            if (GstrPANO != "")
            {
                txtPano.Text = GstrPANO;
                View();
            }
            else
            {
                txtPano.Text = "";
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            View();
        }

        private void View()   //CmdSearch_Click
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            txtPano.Text = txtPano.Text;

            if (rdoTewon2.Checked == true && txtPano.Text == "")
            {
                ComFunc.MsgBox("퇴원환자는 반드시 등록번호 또는 성명을 입력하셔야 됩니다.", "오류");
                return;
            }

            if (rdoTewon0.Checked == true && txtPano.Text == "")
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = Convert.ToInt32(txtPano.Text).ToString("00000000");
                }
            }
            try
            {
                //'환자명단을 SELECT
                SQL = SQL + ComNum.VBLF + "SELECT IPDNO,Pano,SName,DeptCode,RoomCode,GbSTS,Bi,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate,TO_CHAR(OutDate,'YYYY-MM-DD') OutDate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";

                if (rdoTewon0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE Pano='" + txtPano.Text + "' ";
                }
                else if (rdoTewon1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  AND GbSTS = '1' ";
                    SQL = SQL + ComNum.VBLF + " AND Pano='" + txtPano.Text + "' ";

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtPano.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ActDate IS NOT NULL ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY OUTDATE, SName,Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                //스프레드 출력문
                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                if (GstrPANO == "")
                {
                    ssList.Focus();
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

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            ssView_Sheet1.RowCount = 14;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            FstrPANO = ssList_Sheet1.Cells[e.Row, 0].Text;
            FstrInDate = ssList_Sheet1.Cells[e.Row, 3].Text;
            FstrOutDate = ssList_Sheet1.Cells[e.Row, 4].Text;
            FnIPDNO = (int)VB.Val(ssList_Sheet1.Cells[e.Row, 5].Text);

            READ_HISTORY();
        }

        private void READ_HISTORY()
        {
            int i = 0;
            int nRow = 0;
            string strOldData = "";
            string strNewData = "";
            string strViewData = "";

            READ_BM(ref nRow, ref strOldData, ref strNewData, strViewData);
            READ_TRANSFOR_Dept(ref nRow);
            READ_TRANSFOR_Room(ref nRow);
        }

        private void READ_BM(ref int nRow, ref string strOldData, ref string strNewData, string strViewData)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(JobDate,'YYYY-MM-DD') JobDate, Bi ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_BM            ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + txtPano.Text + "            ";
                SQL = SQL + ComNum.VBLF + "    AND GbBackUp = 'J'                            ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY JobDate,Bi                             ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                strOldData = dt.Rows[0]["Bi"].ToString().Trim();
                nRow = 0;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["Bi"].ToString().Trim();
                    if (strOldData != strNewData)
                    {
                        nRow = nRow + 1;
                    }
                    ssView_Sheet1.Cells[i, 0].Text = nRow.ToString();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JobDate"].ToString().Trim();
                    strViewData = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strOldData) + "->";
                    strViewData = strViewData + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strNewData);
                    ssView_Sheet1.Cells[i, 2].Text = strViewData;

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

        private void READ_TRANSFOR_Dept(ref int nRow)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                SQL = SQL + ComNum.VBLF + " SELECT FrDept, FRDOCTOR,  FrRoom, ToDept, TODOCTOR, ToRoom,        ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(TrsDate,'YYYY-MM-DD HH24:MI') TrsDate1, PASSWORD, ROWID, 'A' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano       = '" + txtPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(FstrInDate, 10) + "' ";

                if (FstrOutDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + FstrOutDate + "' ";
                }
                SQL = SQL + ComNum.VBLF + "    AND FrDept <> ToDept ";
                SQL = SQL + ComNum.VBLF + " UNION ALL    ";
                SQL = SQL + ComNum.VBLF + " SELECT  SUBSTR(B.REMARK, INSTR(B.REMARK, '(') - 3, 2) FRDEPT, SUBSTR(B.REMARK, INSTR(B.REMARK, '(') + 1, 4) FRDOCTOR, 0 FRROOM,";
                SQL = SQL + ComNum.VBLF + "         SUBSTR(B.REMARK, INSTR(B.REMARK, '(') - 3, 2) TODEPT, SUBSTR(B.REMARK, INSTR(B.REMARK, '=>') + 2, 4) TODOCTOR, 0 TOROOM,";
                SQL = SQL + ComNum.VBLF + "         TO_CHAR(JOBTIME,' YYYY-MM-DD HH24:MI') TRSDATE, ENTNAME, A.ROWID, 'B' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_JOBHISTORY B, " + ComNum.DB_PMPA + "IPD_NEW_MASTER A";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = '" + txtPano.Text + "'";



                if (FstrOutDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.OUTDATE = TO_DATE('" + FstrOutDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY TrsDate1 ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                nRow = 0;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = nRow.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FrDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["FRDOCTOR"].ToString().Trim()) + ")->" + dt.Rows[i]["ToDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["TODOCTOR"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = CPM.READ_TRANS_JOBNAME(dt.Rows[i]["PASSWORD"].ToString().Trim());

                    if (ssView_Sheet1.Cells[nRow - 1, 7].Text == "")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["PASSWORD"].ToString().Trim();
                    }

                    if (dt.Rows[i]["GUBUN"].ToString().Trim() == "A")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void READ_TRANSFOR_Room(ref int nRow)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT FrDept, FrRoom, ToDept, ToRoom,        ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate1, PASSWORD ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_TRANSFOR  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano       = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(FstrInDate, 10) + "' ";

                if (FstrOutDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + FstrOutDate + "' ";
                }
                SQL = SQL + ComNum.VBLF + "    AND FrRoom <> ToRoom ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY TrsDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nRow.ToString();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["FrRoom"].ToString().Trim() + " -> " + dt.Rows[i]["ToRoom"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = CPM.READ_TRANS_JOBNAME(dt.Rows[i]["PASSWORD"].ToString().Trim());
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

        private void rdoTewon0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTewon2.Checked == true)
            {
                lblList.Text = "퇴원환자";
            }
            else if (rdoTewon0.Checked == true)
            {
                lblList.Text = "재원환자";
            }
            else
            {
                lblList.Text = "가퇴원";
            }
        }

        private void btnDelte_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string[] strROWID = null;
            int eRow = ssView_Sheet1.ActiveRowIndex;
            int eCol = ssView_Sheet1.ActiveColumnIndex;
            string strTEXT = "";
            int nCnt = 0;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                strROWID = new string[ssView_Sheet1.RowCount];

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 3].Value) == true)
                    {
                        strROWID[i] = ssView_Sheet1.Cells[i, 12].Text;
                    }
                }

                for (i = 0; i < strROWID.Length; i++)
                {
                    if (strROWID[i] != "")
                    {
                        nCnt += 1;
                    }
                }

                if (nCnt == 0)
                {
                    ComFunc.MsgBox("삭제할 데이터가 없습니다.", "확인");
                    return;
                }
                else
                {
                    strTEXT = "선택한 내역을 삭제하시겠습니까?";

                    if (ComFunc.MsgBoxQ(strTEXT, "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    for (i = 0; i < strROWID.Length; i++)
                    {
                        if (strROWID[i] != "")
                        {
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "IPD_TRANSFOR_DEL ";
                            SQL = SQL + ComNum.VBLF + " SELECT * FROM KOSMOS_PMPA.IPD_TRANSFOR ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID[i] + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }


                            SQL = " DELETE " + ComNum.DB_PMPA + "IPD_TRANSFOR ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID[i] + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                    //clsDB . setRollbackTran (clsDB.DbCon);
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("삭제 하였습니다.");

                    ssView_Sheet1.RowCount = 0;

                    View();

                    Cursor.Current = Cursors.Default;
                }
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

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
