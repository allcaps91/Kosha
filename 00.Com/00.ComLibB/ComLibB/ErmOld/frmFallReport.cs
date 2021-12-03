using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmFallReport.cs
    /// Description     : 낙상 상황 보고서
    /// Author          : 박창욱
    /// Create Date     : 2017-08-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-08-25 박창욱 : 완료
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\Frm낙상보고서.frm(Frm낙상보고서.frm) >> frmFallReport.cs 폼이름 재정의" />	
    public partial class frmFallReport : Form
    {
        //string FstrFlag = "";
        string FstrNEW = "";
        string FstrROWID = "";
        string GstrWardCodes = "";
        string GstrHelpCode = "";
        string gsWard = "";

        string mstrIpdno = "";
        string mstrBDate = "";
        string mstrDeptCode = "";
        string mstrROWID = "";

        /// <summary>
        /// EMR VIWER
        /// </summary>
        frmEmrViewer frmEmrViewer = null;

        public frmFallReport()
        {
            InitializeComponent();
        }

        public frmFallReport(string sWard)
        {
            InitializeComponent();
            gsWard = sWard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWard"></param>
        /// <param name="strIpdno"> 외래는 ptno 입원은 ipdno  기존 루틴인듯</param>
        /// <param name="strBDate"></param>
        /// <param name=""></param>
        public frmFallReport(string sWard, string strIpdno, string strBDate, string strDeptCode, string strROWID = "")
        {
            InitializeComponent();
            gsWard = sWard;
            mstrIpdno = strIpdno;
            mstrBDate = strBDate;
            mstrDeptCode = strDeptCode;
            mstrROWID = strROWID;
        }

        private void chkToiwon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkToiwon.Checked == true)
            {
                //grbDate.Visible = true;
                dtpDate.Enabled = true;
                dtpEDate.Enabled = true;
                chkWrite.Enabled = true;
                //grbDept.Visible = true;
                //cboOPDDept.Enabled = true;
                //dtpOPDDate.Enabled = true;
            }
            else
            {
                //grbDate.Visible = true;
                dtpDate.Enabled = false;
                dtpEDate.Enabled = false;
                chkWrite.Enabled = false;
                //grbDept.Visible = true;
                //cboOPDDept.Enabled = true;
                //dtpOPDDate.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 보고서를 선택하여 주십시오.");
                return;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                SQL = "";

                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_REPORT_HIS";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

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
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_FALL_REPORT";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

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

            Search_Data();
        }

        void SCREEN_CLEAR()
        {
            int i = 0;

            
            FarPoint.Win.Spread.CellType.ComboBoxCellType cboCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            cboCell.Items = (new String[] { "근접오류", "위해사건", "적신호사건" });

            cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
            cboCell.Editable = false;
            cboCell.MaxDrop = 3;
            ssView_Sheet1.Cells[8, 2].CellType = cboCell;
            ssView_Sheet1.Cells[8, 2].Text = "";        //오류구분
            ssView_Sheet1.Cells[8, 4].Text = "";        //오류등급

            for (i = 5; i < 9; i++)
            {
                ssView_Sheet1.Cells[i - 1, 2].Text = "";
                ssView_Sheet1.Cells[i - 1, 4].Text = "";
                ssView_Sheet1.Cells[i - 1, 6].Text = "";
                ssView_Sheet1.Cells[i - 1, 9].Text = "";
            }

            ssView_Sheet1.Cells[8, 2].Text = "";
            ssView_Sheet1.Cells[8, 4].Text = "";

            ssView_Sheet1.Cells[9 + 1, 2].Text = "";
            ssView_Sheet1.Cells[9 + 1, 4].Text = "";

            ssView_Sheet1.Cells[10 + 1, 2].Text = "";
            ssView_Sheet1.Cells[10 + 1, 3].Text = "";
            ssView_Sheet1.Cells[10 + 1, 5].Text = "";
            ssView_Sheet1.Cells[10 + 1, 6].Text = "";
            ssView_Sheet1.Cells[10 + 1, 8].Text = "";

            ssView_Sheet1.Cells[11 + 1, 2].Text = "";
            ssView_Sheet1.Cells[11 + 1, 3].Text = "";
            ssView_Sheet1.Cells[11 + 1, 6].Text = "";
            ssView_Sheet1.Cells[11 + 1, 8].Text = "";

            ssView_Sheet1.Cells[12 + 1, 3].Text = "";
            ssView_Sheet1.Cells[12 + 1, 5].Text = "";
            ssView_Sheet1.Cells[12 + 1, 7].Text = "";

            ssView_Sheet1.Cells[13 + 1, 3].Text = "";
            ssView_Sheet1.Cells[13 + 1, 4].Text = "";
            ssView_Sheet1.Cells[13 + 1, 5].Text = "";
            ssView_Sheet1.Cells[13 + 1, 6].Text = "";
            ssView_Sheet1.Cells[13 + 1, 7].Text = "";
            ssView_Sheet1.Cells[13 + 1, 9].Text = "";

            ssView_Sheet1.Cells[14 + 1, 3].Text = "";
            ssView_Sheet1.Cells[14 + 1, 4].Text = "";
            ssView_Sheet1.Cells[14 + 1, 5].Text = "";
            ssView_Sheet1.Cells[14 + 1, 6].Text = "";
            ssView_Sheet1.Cells[14 + 1, 9].Text = "";

            ssView_Sheet1.Cells[15 + 1, 4].Text = "";

            ssView_Sheet1.Cells[17 + 1, 2].Text = "";

            ssView_Sheet1.Cells[19 + 1, 1].Text = "";
            ssView_Sheet1.Cells[19 + 1, 3].Text = "";
            ssView_Sheet1.Cells[19 + 1, 8].Text = "";
            ssView_Sheet1.Cells[19 + 1, 9].Text = "";

            setSpread(false);

            ssView_Sheet1.Cells[20 + 1, 1].Text = "";
            ssView_Sheet1.Cells[20 + 1, 3].Text = "";
            ssView_Sheet1.Cells[20 + 1, 7].Text = "";

            ssView_Sheet1.Cells[21 + 1, 1].Text = "";
            ssView_Sheet1.Cells[21 + 1, 2].Text = "";
            ssView_Sheet1.Cells[21 + 1, 7].Text = "";

            //낙상장소
            ssView_Sheet1.Cells[23 + 1, 1].Text = "";
            ssView_Sheet1.Cells[23 + 1, 2].Text = "";
            ssView_Sheet1.Cells[23 + 1, 3].Text = "";
            ssView_Sheet1.Cells[23 + 1, 4].Text = "";
            ssView_Sheet1.Cells[23 + 1, 5].Text = "";

            ssView_Sheet1.Cells[24 + 1, 1].Text = "";
            ssView_Sheet1.Cells[24 + 1, 2].Text = "";
            ssView_Sheet1.Cells[24 + 1, 3].Text = "";
            ssView_Sheet1.Cells[24 + 1, 4].Text = "";
            ssView_Sheet1.Cells[24 + 1, 6].Text = "";

            ssView_Sheet1.Cells[25 + 1, 8].Text = "";
            ssView_Sheet1.Cells[25 + 1, 9].Text = "";

            //침대 낙상시
            ssView_Sheet1.Cells[26 + 1, 3].Text = "";
            ssView_Sheet1.Cells[26 + 1, 4].Text = "";
            ssView_Sheet1.Cells[26 + 1, 8].Text = "";
            ssView_Sheet1.Cells[26 + 1, 9].Text = "";

            ssView_Sheet1.Cells[27 + 1, 3].Text = "";
            ssView_Sheet1.Cells[27 + 1, 4].Text = "";
            ssView_Sheet1.Cells[27 + 1, 8].Text = "";
            ssView_Sheet1.Cells[27 + 1, 9].Text = "";

            ssView_Sheet1.Cells[28 + 1, 3].Text = "";
            ssView_Sheet1.Cells[28 + 1, 4].Text = "";

            ssView_Sheet1.Cells[29 + 1, 3].Text = "";
            ssView_Sheet1.Cells[29 + 1, 4].Text = "";
            ssView_Sheet1.Cells[29 + 1, 5].Text = "";

            ssView_Sheet1.Cells[30 + 1, 2].Text = "";

            //미끄러지거나 넘어진 경우
            ssView_Sheet1.Cells[32 + 1, 3].Text = "";
            ssView_Sheet1.Cells[32 + 1, 4].Text = "";
            ssView_Sheet1.Cells[32 + 1, 5].Text = "";
            ssView_Sheet1.Cells[32 + 1, 7].Text = "";

            ssView_Sheet1.Cells[33 + 1, 3].Text = "";
            ssView_Sheet1.Cells[33 + 1, 4].Text = "";
            ssView_Sheet1.Cells[33 + 1, 5].Text = "";
            ssView_Sheet1.Cells[33 + 1, 7].Text = "";

            ssView_Sheet1.Cells[34 + 1, 3].Text = "";
            ssView_Sheet1.Cells[34 + 1, 4].Text = "";

            ssView_Sheet1.Cells[36 + 1, 1].Text = "";
            ssView_Sheet1.Cells[36 + 1, 4].Text = "";

            for (i = 38 + 1; i < 45 + 1; i++)
            {
                ssView_Sheet1.Cells[i - 1 + 1, 5].Text = "";
                ssView_Sheet1.Cells[i - 1 + 1, 7].Text = "";
            }

            ssView_Sheet1.Cells[45 + 1, 1].Text = "";   //IPDNO
            ssView_Sheet1.Cells[45 + 1, 2].Text = "";   //발생일자
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            if (ComFunc.MsgBoxQ("보고서를 저장하지 않고 출력하는 버튼입니다." + ComNum.VBLF + "저장하지 않은 경우 '저장 및 출력' 버튼을 사용하십시오. 계속하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            ssView_Sheet1.PrintInfo.ShowBorder = false;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssView_Sheet1.PrintInfo.Margin.Left = 40;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            int nRead = 0;
            string strWard = "";
            string strToDate = "";
            string strNextDate = "";
            string strRemark = "";
            string strOK = "";
            string strFlag = "";

            ssList_Sheet1.RowCount = 0;
            ssHistory_Sheet1.RowCount = 0;

            SCREEN_CLEAR();

            strWard = cboWard.Text.Trim();
            strToDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strNextDate = dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd");

            try
            {
                if (GstrWardCodes == "OPD" || cboWard.Text.Trim() == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                {
                    SQL = "";
                    SQL = " SELECT TO_NUMBER(PANO) IPDNO, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE, 0 ILSU, ";
                    SQL = SQL + ComNum.VBLF + "   BI, '' RELIGION, GBSPC, 'OPD' WARDCODE, 0 ROOMCODE, ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD')  INDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  '' OUTDATE ,  '' JIYUK, 0 TBED ";
                    SQL = SQL + ComNum.VBLF + "  From " + ComNum.DB_PMPA + "OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + "WHERE GBUSE = 'Y' ";
                    SQL = SQL + ComNum.VBLF + "  AND ACTDATE = TO_DATE('" + dtpOPDDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    if (GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                    {
                        SQL = SQL + ComNum.VBLF + " AND PANO IN (SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE = TO_DATE('" + dtpOPDDate.Value.ToString("yyyy-MM-dd") + "','YYYY--MM-DD') ) ";
                    }
                    else
                    {
                        if (cboOPDDept.Text != "전체")
                        {
                            SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + cboOPDDept.Text + "' ";
                        }
                    }
                    SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE, PANO, SNAME     ";
                }
                else if (GstrWardCodes != "OPD")
                {
                    SQL = "";
                    SQL = " SELECT a.IPDNO, a.Pano, a.SName, a.Sex, a.Age, a.DeptCode,a.DrCode,a.ILSU, ";
                    SQL = SQL + ComNum.VBLF + "  a.Bi, a.ReliGion, a.GbSpc, a.WardCode, a.RoomCode, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(A.INDATE,'YYYY-MM-DD')  INDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, a.JiYuk, b.TBed ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "Bas_Room b ";
                    if (chkToiwon.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE a.IpwonTime >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + " 00:01','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE ((A.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND a.OutDate IS NULL) OR a.OutDate>=TO_DATE('" + strNextDate + "','YYYY-MM-DD')) ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND a.Amset4 <> '3' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.RoomCode = b.RoomCode(+) ";

                    if (chkWrite.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND A.IPDNO IN (SELECT IPDNO FROM NUR_FALL_REPORT ";
                        SQL += ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                    }
                    else
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "전체":
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '233' ";
                                break;
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '234' ";
                                break;
                            case "ND":
                            case "NR":
                                SQL = SQL + ComNum.VBLF + " AND a.RoomCode IN ('369','358','368','640','641','642')  ";
                                break;
                            default:
                                // SQL = SQL + ComNum.VBLF + " AND a.WardCode = ('" + cboWard.Text.Trim() + "') ";
                                SQL = SQL + ComNum.VBLF + " AND (( a.WardCode IN ('" + cboWard.Text.Trim() + "') ) ";
                                SQL = SQL + ComNum.VBLF + " OR  ( a.IPDNO IN (SELECT IPDNO FROM NUR_FALL_REPORT ";
                                SQL = SQL + ComNum.VBLF + "                   WHERE ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "                     AND ( WardCode IN ('" + cboWard.Text.Trim() + "') ) or ( entsabun IN ('" + clsType.User.IdNumber + "') )  ) ) ) ";
                                break;
                        }
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY a.RoomCode,a.SName,a.Indate DESC   ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strOK = "";
                    strFlag = "";
                    if (GstrWardCodes == "OPD" || cboWard.Text == "OPD" ||  GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                    {
                        strFlag = "OK";
                    }
                    else
                    {
                        SQL = "";
                        SQL = " SELECT Remark FROM " + ComNum.VBLF + "NUR_MASTER WHERE Ipdno ='" + dt.Rows[i]["Ipdno"].ToString().Trim() + "' ";
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strRemark = dt1.Rows[0]["Remark"].ToString().Trim();
                            if (strRemark != "")
                            {
                                if (VB.I(strRemark, "▶낙상,") > 1)
                                {
                                    strFlag = "OK";
                                }
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    if (chkDaesang.Checked == true)
                    {
                        if (strFlag == "OK")
                        {
                            nRow++;
                            strOK = "OK";
                        }
                    }
                    else
                    {
                        nRow += 1;
                        strOK = "OK";
                    }

                    if ((strFlag == "OK" && chkDaesang.Checked == true) || (strOK == "OK" && chkDaesang.Checked == false))
                    {
                        ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Indate"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Ipdno"].ToString().Trim();

                        if (strFlag == "OK" || GstrWardCodes == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                        {
                            ssList_Sheet1.Cells[nRow - 1, 6].Text = "OK";
                        }

                        if (GstrWardCodes != "OPD" && GstrWardCodes != "OP" && cboWard.Text.Trim() != "OP")
                        {
                            SQL = "";
                            SQL = " SELECT ROWID ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT WHERE IPDNO = " + dt.Rows[i]["Ipdno"].ToString().Trim() + " ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " SELECT ROWID ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALL_REPORT ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND TRUNC(ACTDATE) = TO_DATE('" + dt.Rows[i]["Indate"].ToString().Trim() + "','YYYY-MM-DD')";
                        }
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 6].ForeColor = Color.FromArgb(0, 0, 0);
                            ssList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 6].BackColor = Color.FromArgb(128, 255, 128);
                        }
                        dt1.Dispose();
                        dt1 = null;

                        strOK = "";
                        strFlag = "";
                    }
                }
                dt.Dispose();
                dt = null;

                ssList_Sheet1.RowCount = nRow;

                SCREEN_CLEAR();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            int i = 0;
            string strPano = "";
            string strName = "";
            string strAge = "";
            string strSex = "";
            string strDiag = "";
            string strACTDATE = "";
            string strBDATE = "";
            string strSeekDate = "";
            string strReturnDate = "";
            string strRoom = "";
            int nIPDNO = 0;
            string strDeptCode = "";
            string strWard = "";
            string strEtcName = "";
            string strWEIGHT = "";
            string strHEIGHT = "";
            string strTemp = "";
            string[] strFall = new string[28];
            string[] strFallEtc = new string[28];

            string strEntDate = "";
            string strEntSabun = "";

            string strErrGubun = "";
            string strErrGrade = "";

            for (i = 0; i < strFall.Length; i++)
            {
                strFall[i] = "";
            }

            if (ssView_Sheet1.Cells[45 + 1, 4].Text == "Y")
            {
                if (ComFunc.MsgBoxQ("이미 인쇄한 보고서입니다. 수정하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
            }

            //기본정보 세팅
            strName = ssView_Sheet1.Cells[4, 2].Text;
            strSex = VB.Pstr(ssView_Sheet1.Cells[4, 4].Text, "/", 1);
            strAge = VB.Pstr(ssView_Sheet1.Cells[4, 4].Text, "/", 2);
            strACTDATE = ssView_Sheet1.Cells[4, 6].Text;
            strPano = ssView_Sheet1.Cells[5, 2].Text;
            strSeekDate = ssView_Sheet1.Cells[5, 6].Text;
            strWard = ssView_Sheet1.Cells[6, 2].Text;
            strDeptCode = ssView_Sheet1.Cells[6, 4].Text;
            strReturnDate = ssView_Sheet1.Cells[6, 6].Text;
            strDiag = ssView_Sheet1.Cells[7, 2].Text;
            strEtcName = ssView_Sheet1.Cells[7, 9].Text;
            
            strErrGubun = ssView_Sheet1.Cells[8, 2].Text;
            strErrGrade = ssView_Sheet1.Cells[8, 4].Text;

            strWEIGHT = ssView_Sheet1.Cells[9 + 1, 2].Text;
            strHEIGHT = ssView_Sheet1.Cells[9 + 1, 4].Text;

            //의식상태
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[10 + 1, 2].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[10 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[10 + 1, 5].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[10 + 1, 6].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[10 + 1, 8].Value) == true, "1", "0").ToString();
            strFall[1] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[1]) == 0)
            {
                ComFunc.MsgBox("의식상태를 선택하세요");
                return;
            }

            //활동 및 기능
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[11 + 1, 2].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[11 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[11 + 1, 6].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[11 + 1, 8].Value) == true, "1", "0").ToString();
            strFall[2] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[2]) == 0)
            {
                ComFunc.MsgBox("활동 및 기능을 선택하세요");
                return;
            }

            //휠체어나 보행보조기구 사용 여부(2019-07-31 추가)
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[12 + 1, 3].Value) == true, "1", "0").ToString();
            strFall[3] = strTemp;
            strFallEtc[3] = ssView_Sheet1.Cells[12 + 1, 5].Text.Trim();
            strTemp = "";
            

            //환자의 위험요인
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 4].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 5].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 6].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 7].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[13 + 1, 9].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[14 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[14 + 1, 4].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[14 + 1, 5].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[14 + 1, 6].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[14 + 1, 9].Value) == true, "1", "0").ToString();
            strFallEtc[4] = ssView_Sheet1.Cells[15 + 1, 4].Text.Trim();
            strFall[4] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[4]) == 0)
            {
                ComFunc.MsgBox("환자관련 위험요인을 선택하세요");
                return;
            }

            //투약관련 위험요인
            strFall[5] = ssView_Sheet1.Cells[17 + 1, 2].Text;

            //낙상유형
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 1].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[20 + 1, 1].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[20 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[21 + 1, 1].Value) == true, "1", "0").ToString();
            strFallEtc[6] = ssView_Sheet1.Cells[21 + 1, 2].Text;
            strFall[6] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[6]) == 0)
            {
                ComFunc.MsgBox("낙상유형을 선택하세요");
                return;
            }

            //낙상장소
            strTemp = "";
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[23 + 1, 1].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[23 + 1, 2].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[23 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[23 + 1, 4].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[24 + 1, 1].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[24 + 1, 2].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[24 + 1, 3].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[24 + 1, 4].Value) == true, "1", "0").ToString();
            strFall[7] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[7]) == 0)
            {
                ComFunc.MsgBox("낙상장소를 선택하세요");
                return;
            }

            //침대낙상시
            if (Convert.ToBoolean(ssView_Sheet1.Cells[26 + 1, 3].Value) == true)
            {
                strFall[8] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[26 + 1, 4].Value) == true)
            {
                strFall[8] = "0";
                strTemp = "1";
            }
            if (strTemp != "1" && Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 1].Value) == true)
            {
                ComFunc.MsgBox("침대낙상 시에 빠진 항목이 있습니다.");
                return;
            }


            if (Convert.ToBoolean(ssView_Sheet1.Cells[27 + 1, 3].Value) == true)
            {
                strFall[9] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[27 + 1, 4].Value) == true)
            {
                strFall[9] = "0";
                strTemp = "1";
            }
            if (strTemp != "1" && Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 1].Value) == true)
            {
                ComFunc.MsgBox("침대낙상 시에 빠진 항목이 있습니다.");
                return;
            }


            if (Convert.ToBoolean(ssView_Sheet1.Cells[28 + 1, 3].Value) == true)
            {
                strFall[10] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[28 + 1, 4].Value) == true)
            {
                strFall[10] = "0";
                strTemp = "1";
            }
            if (strTemp != "1" && Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 1].Value) == true)
            {
                ComFunc.MsgBox("침대낙상 시에 빠진 항목이 있습니다.");
                return;
            }

            if (Convert.ToBoolean(ssView_Sheet1.Cells[29 + 1, 3].Value) == true)
            {
                strFall[11] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[29 + 1, 4].Value) == true)
            {
                strFall[11] = "0";
                strTemp = "1";
            }
            if (strTemp != "1" && Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 1].Value) == true)
            {
                ComFunc.MsgBox("침대낙상 시에 빠진 항목이 있습니다.");
                return;
            }


            strFallEtc[11] = ssView_Sheet1.Cells[30 + 1, 2].Text.Trim();

            //미끄러지거나 넘어진 경우
            if (Convert.ToBoolean(ssView_Sheet1.Cells[32 + 1, 3].Value) == true)
            {
                strFall[12] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[32 + 1, 4].Value) == true)
            {
                strFall[12] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("미끄러지거나 넘어진 경우에 빠진 항목이 있습니다.");
                return;
            }

            if (Convert.ToBoolean(ssView_Sheet1.Cells[33 + 1, 3].Value) == true)
            {
                strFall[13] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[33 + 1, 4].Value) == true)
            {
                strFall[13] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("미끄러지거나 넘어진 경우에 빠진 항목이 있습니다.");
                return;
            }

            if (Convert.ToBoolean(ssView_Sheet1.Cells[34 + 1, 3].Value) == true)
            {
                strFall[14] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[34 + 1, 4].Value) == true)
            {
                strFall[14] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("미끄러지거나 넘어진 경우에 빠진 항목이 있습니다.");
                return;
            }

            //간호중재
            strTemp = "";
            if (Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 8].Value) == true)
            {
                strFall[15] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[19 + 1, 9].Value) == true)
            {
                strFall[15] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("간호중재에 빠진 항목이 있습니다.");
                return;
            }

            strFall[16] = ssView_Sheet1.Cells[20 + 1, 7].Text;
            strFall[17] = ssView_Sheet1.Cells[21 + 1, 7].Text;
            strFall[18] = ssView_Sheet1.Cells[23 + 1, 5].Text;
            strFall[19] = ssView_Sheet1.Cells[24 + 1, 6].Text;

            strTemp = "";
            if (Convert.ToBoolean(ssView_Sheet1.Cells[25 + 1, 8].Value) == true)
            {
                strFall[20] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[25 + 1, 9].Value) == true)
            {
                strFall[20] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("간호중재에 빠진 항목이 있습니다.");
                return;
            }

            strTemp = "";
            if (Convert.ToBoolean(ssView_Sheet1.Cells[26 + 1, 8].Value) == true)
            {
                strFall[21] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[26 + 1, 9].Value) == true)
            {
                strFall[21] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("간호중재에 빠진 항목이 있습니다.");
                return;
            }

            strTemp = "";
            if (Convert.ToBoolean(ssView_Sheet1.Cells[27 + 1, 8].Value) == true)
            {
                strFall[22] = "1";
                strTemp = "1";
            }
            else if (Convert.ToBoolean(ssView_Sheet1.Cells[27 + 1, 9].Value) == true)
            {
                strFall[22] = "0";
                strTemp = "1";
            }
            if (strTemp != "1")
            {
                ComFunc.MsgBox("간호중재에 빠진 항목이 있습니다.");
                return;
            }

            strFall[23] = ssView_Sheet1.Cells[29 + 1, 5].Text;

            //낙상결과
            strTemp = "";
            strTemp = VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[32 + 1, 5].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[32 + 1, 7].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[33 + 1, 5].Value) == true, "1", "0").ToString();
            strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[33 + 1, 7].Value) == true, "1", "0").ToString();
            strFall[24] = strTemp;
            strTemp = "";
            if (VB.Val(strFall[24]) == 0)
            {
                ComFunc.MsgBox("낙상결과를 선택하세요");
                return;
            }

            //환자의 신체적 손상 및 치료
            for (i = 38; i < 45; i++)
            {
                strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[i - 1 + 1, 5].Value) == true, "1", "0").ToString();
            }
            strFall[25] = strTemp;
            if (VB.Val(strFall[25]) == 0)
            {
                ComFunc.MsgBox("신체적 손상을 선택하세요");
                return;
            }

            strTemp = "";
            for (i = 38; i < 44; i++)
            {
                strTemp += VB.IIf(Convert.ToBoolean(ssView_Sheet1.Cells[i - 1 + 1, 7].Value) == true, "1", "0").ToString();
            }
            strFall[26] = strTemp;
            if (VB.Val(strFall[26]) == 0)
            {
                ComFunc.MsgBox("치료내용을 선택하세요");
                return;
            }
            strTemp = "";

            //낙상발생 상황
            strFall[27] = ssView_Sheet1.Cells[36 + 1, 1].Text;
            nIPDNO = (int)VB.Val(ssView_Sheet1.Cells[45 + 1, 1].Text);
            strBDATE = ssView_Sheet1.Cells[45 + 1, 2].Text;
            strEntDate = ssView_Sheet1.Cells[45 + 1, 3].Text;



            if (strPano == "")
            {
                ComFunc.MsgBox("등록번호가 공백입니다.");
                return;
            }
            if (strName == "")
            {
                ComFunc.MsgBox("성명이 공백입니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (cboWard.Text.Trim() == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                {
                    SQL = "";
                    SQL = " SELECT ENTDATE, ENTSABUN, ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO =  '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ENTDATE = TO_DATE('" + strEntDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT ENTDATE, ENTSABUN,  ROWID FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO =  " + nIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND ENTDATE = TO_DATE('" + strEntDate + "','YYYY-MM-DD') ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    strEntSabun = dt.Rows[0]["ENTSABUN"].ToString().Trim();
                    strEntDate = dt.Rows[0]["ENTDATE"].ToString().Trim();
                }
                else
                {
                    FstrROWID = "";
                }
                dt.Dispose();
                dt = null;


                if (FstrROWID == "" || FstrNEW == "OK")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_REPORT ( ";
                    SQL = SQL + ComNum.VBLF + " PANO,SNAME,SEX,AGE,ACTDATE,SeekDATE,ReturnDate, DIAGNOSYS,DeptCode,WardCode,RoomCode, ";
                    SQL = SQL + ComNum.VBLF + " EtcName, Weight, Height, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall1, Nur_Fall2, Nur_Fall3, Nur_Fall3_Etc, Nur_Fall4, Nur_Fall4_Etc, Nur_Fall5, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall6, Nur_Fall6_Etc, Nur_Fall7, Nur_Fall8, Nur_Fall9, Nur_Fall10, Nur_Fall11, Nur_Fall11_Etc, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall12, Nur_Fall13, Nur_Fall14, Nur_Fall15, Nur_Fall16, Nur_Fall17, Nur_Fall18, Nur_Fall19, Nur_Fall20, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall21, Nur_Fall22, Nur_Fall23, Nur_Fall24, Nur_Fall25, Nur_Fall26, Nur_Fall27, ";
                    SQL = SQL + ComNum.VBLF + " Ipdno, Gubun, ENTDATE, ENTSABUN, ERRGUBUN, ERRGRADE ) VALUES ('" + strPano + "','" + strName + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + strSex + "' , '" + strAge + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strACTDATE + "','YYYY-MM-DD HH24:MI'),TO_DATE('" + strSeekDate + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strReturnDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strDiag + "' , '" + strDeptCode + "', '" + strWard + "', " + VB.Val(strRoom) + ", ";
                    SQL = SQL + ComNum.VBLF + " '" + strEtcName + "', '" + strWEIGHT + "', '" + strHEIGHT + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[1] + "', '" + strFall[2] + "', '" + strFall[3] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFallEtc[3] + "', '" + strFall[4] + "', '" + strFallEtc[4] + "','" + strFall[5] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[6] + "', '" + strFallEtc[6] + "', '" + strFall[7] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[8] + "', '" + strFall[9] + "', '" + strFall[10] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[11] + "', '" + strFallEtc[11] + "', '" + strFall[12] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[13] + "', '" + strFall[14] + "', '" + strFall[15] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[16] + "', '" + strFall[17] + "', '" + strFall[18] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[19] + "', '" + strFall[20] + "', '" + strFall[21] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[22] + "', '" + strFall[23] + "', '" + strFall[24] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[25] + "', '" + strFall[26] + "', '" + strFall[27] + "', ";
                    SQL = SQL + ComNum.VBLF + "  " + nIPDNO + ", ";
                    if (nIPDNO > 0)
                    {
                        SQL = SQL + ComNum.VBLF + "  '1', ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  '2', ";
                    }
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD'), " + clsType.User.IdNumber + ", ";
                    SQL = SQL + ComNum.VBLF + "'" + strErrGubun + "','" + strErrGrade + "' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_REPORT_HIS ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";
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
                    SQL = " DELETE KOSMOS_PMPA.NUR_FALL_REPORT ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";
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
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_FALL_REPORT ( ";
                    SQL = SQL + ComNum.VBLF + " PANO,SNAME,SEX,AGE,ACTDATE,SeekDATE,ReturnDate, DIAGNOSYS,DeptCode,WardCode,RoomCode, ";
                    SQL = SQL + ComNum.VBLF + " EtcName, Weight, Height, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall1, Nur_Fall2, Nur_Fall3, Nur_Fall3_Etc, Nur_Fall4, Nur_Fall4_Etc, Nur_Fall5, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall6, Nur_Fall6_Etc, Nur_Fall7, Nur_Fall8, Nur_Fall9, Nur_Fall10, Nur_Fall11, Nur_Fall11_Etc, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall12, Nur_Fall13, Nur_Fall14, Nur_Fall15, Nur_Fall16, Nur_Fall17, Nur_Fall18, Nur_Fall19, Nur_Fall20, ";
                    SQL = SQL + ComNum.VBLF + " Nur_Fall21, Nur_Fall22, Nur_Fall23, Nur_Fall24, Nur_Fall25, Nur_Fall26, Nur_Fall27, ";
                    SQL = SQL + ComNum.VBLF + " Ipdno, Gubun, ENTDATE, ENTSABUN, ERRGUBUN, ERRGRADE ) VALUES ('" + strPano + "','" + strName + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + strSex + "' , '" + strAge + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strACTDATE + "','YYYY-MM-DD HH24:MI'),TO_DATE('" + strSeekDate + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strReturnDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strDiag + "' , '" + strDeptCode + "', '" + strWard + "', " + VB.Val(strRoom) + ", ";
                    SQL = SQL + ComNum.VBLF + " '" + strEtcName + "', '" + strWEIGHT + "', '" + strHEIGHT + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[1] + "', '" + strFall[2] + "', '" + strFall[3] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFallEtc[3] + "', '" + strFall[4] + "', '" +  strFallEtc[4] + "','" + strFall[5] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[6] + "', '" + strFallEtc[6] + "', '" + strFall[7] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[8] + "', '" + strFall[9] + "', '" + strFall[10] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[11] + "', '" + strFallEtc[11] + "', '" + strFall[12] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[13] + "', '" + strFall[14] + "', '" + strFall[15] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[16] + "', '" + strFall[17] + "', '" + strFall[18] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[19] + "', '" + strFall[20] + "', '" + strFall[21] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[22] + "', '" + strFall[23] + "', '" + strFall[24] + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strFall[25] + "', '" + strFall[26] + "', '" + strFall[27] + "', ";
                    SQL = SQL + ComNum.VBLF + "  " + nIPDNO + ", ";
                    if (nIPDNO > 0)
                    {
                        SQL = SQL + ComNum.VBLF + "  '1', ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  '2', ";
                    }
                    //SQL = SQL + ComNum.VBLF + " TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD'), '" + clsType.User.IdNumber + "',  ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD'), '" + strEntSabun + "',  ";
                    SQL = SQL + ComNum.VBLF + "'" + strErrGubun + "','" + strErrGrade + "'  ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                if (ComFunc.MsgBoxQ("자료저장완료! 저장한 자료를 인쇄하시겠습니까?") == DialogResult.Yes)
                {
                    Do_Print();
                }

                FstrNEW = "";
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

        void Do_Print()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strIPDNO = "";
            string strBDATE = "";

            if (ssView_Sheet1.Cells[45 + 1, 4].Text == "Y")
            {
                if (ComFunc.MsgBoxQ("이미 인쇄한 보고서입니다. 인쇄하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.PrintInfo.ShowBorder = false;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssView_Sheet1.PrintInfo.Margin.Left = 40;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView.PrintSheet(0);

            strIPDNO = ssView_Sheet1.Cells[45 + 1, 1].Text;
            strBDATE = ssView_Sheet1.Cells[45 + 1, 2].Text;


            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                SQL = "";
                SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_FALL_REPORT SET";
                SQL = SQL + ComNum.VBLF + " PRTYN = 'Y' ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(ACTDATE) = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";

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

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;

            if (txtPano.Text == "" && txtSName.Text == "")
            {
                ComFunc.MsgBox("등록번호 및 성명을 확인하세요");
                return;
            }

            try
            {
                SQL = "";
                SQL = " SELECT Pano,DeptCode,SName,Sex,Age,DrCode,Bi, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ActDate  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE =TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (txtPano.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND PANO='" + txtPano.Text.Trim() + "' ";
                }
                if (txtSName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND SNAME LIKE '%" + txtSName.Text.Trim() + "%' ";
                }
                if (cboDept.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND DeptCode ='" + cboDept.Text.Trim() + "' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                nRead = dt.Rows.Count;
                ssList2_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    ssList2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim() + "/" + VB.Val(dt.Rows[i]["Age"].ToString().Trim());

                    SQL = "SELECT ROWID AS ROWIDC";
                    SQL = SQL + " FROM NUR_FALL_REPORT ";
                    SQL = SQL + " WHERE Pano = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + "  AND ActDate >=TO_DATE('" + dt.Rows[i]["ActDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + "  AND ActDate <TO_DATE('" + Convert.ToDateTime(dt.Rows[i]["ActDate"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssList2_Sheet1.Cells[i, 0, i, 1].ForeColor = Color.FromArgb(0, 0, 0);
                        ssList2_Sheet1.Cells[i, 0, i, 1].BackColor = Color.FromArgb(128, 255, 128);
                    }
                    dt1.Dispose();
                    dt1 = null;
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            GstrWardCodes = cboWard.Text.Trim();

            if (cboWard.Text == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
            {
                grbDept.Visible = true;
                grbDate.Visible = false;
            }
            else
            {
                grbDept.Visible = false;
                grbDate.Visible = true;
            }
        }

        private void frmFallReport_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SCREEN_CLEAR();

            //dtpDate.Enabled = false;
            //dtpEDate.Enabled = false;

            if (gsWard == "")
            {
                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            GstrWardCodes = gsWard;
            if (GstrWardCodes == "")
            {
                GstrWardCodes = "OPD";
            }

            if (clsType.User.JobGroup == "JOB013053")
            {
                chkWrite.Visible = true;
            }


            //진료과 Combo SET
            try
            {
                SQL = "";
                SQL = "SELECT DeptCode FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + " WHERE DeptCode NOT IN ('II','HR','TO','R6','PT','AN') ";
                SQL = SQL + " ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboDept.Items.Clear();
                cboDept.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    cboOPDDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                cboDept.Items.Add("OPD");
                cboDept.SelectedIndex = 0;
                cboOPDDept.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            if (GstrWardCodes == "HD")
            {
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                tabControl1.SelectedIndex = 0;
            }

            cboWard_SET();

            if (GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("OP");
                cboWard.SelectedIndex = 0;
                grbDept.Visible = true;
                chkDaesang.Checked = false;
            }
            else if (GstrWardCodes == "OPD" )
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("OPD");
                cboWard.SelectedIndex = 0;
                grbDept.Visible = true;
                chkDaesang.Checked = false;
            }

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            txtPano.Text = "";
            txtSName.Text = "";
            dtpDate1.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpOPDDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            SCREEN_CLEAR();

            //2019-05-16
            btnNew.Visible = false;
            if (cboWard.Text == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
            {
                btnNew.Visible = true;
            }

            if (mstrIpdno != "")
            {
                if ( GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                {
                    SS_Display("1", mstrIpdno, mstrBDate, mstrDeptCode);
                }
                else if (gsWard != "OPD")
                {
                    SS_Display("1", mstrIpdno, "", "", mstrROWID);
                }
                else
                {
                    SS_Display("1", mstrIpdno, mstrBDate, mstrDeptCode);
                }
            }
        }

        void SS_Display(string argGbn, string argIpdNo, string argDate = "", string argDept = "", string argROWID = "")
        {
            int k = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strInDate = "";
            string strEMRNO = "";
            string strFormNo = "";

            SCREEN_CLEAR();

            try
            {
                SQL = "";
                SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate,TO_CHAR(ActDate,'YYYY-MM-DD') BDate,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(SeekDate,'YYYY-MM-DD HH24:MI') SeekDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ReturnDate,'YYYY-MM-DD HH24:MI') ReturnDate, SNAME,SEX,AGE,DIAGNOSYS,ROOMCODE,DEPTCODE,EntSabun, ";
                SQL = SQL + ComNum.VBLF + " WardCode,DIAGNOSYS,ENTDATE, ";
                SQL = SQL + ComNum.VBLF + " EtcName, Weight,Height, Nur_Fall1,Nur_Fall2,Nur_Fall3,Nur_Fall3_Etc, ";
                SQL = SQL + ComNum.VBLF + " Nur_Fall4,Nur_Fall4_Etc,Nur_Fall5,Nur_Fall6,Nur_Fall6_Etc,Nur_Fall7,Nur_Fall8,Nur_Fall9,Nur_Fall10, ";
                SQL = SQL + ComNum.VBLF + " Nur_Fall11,Nur_Fall11_Etc,Nur_Fall12,Nur_Fall13,Nur_Fall14,Nur_Fall15,Nur_Fall16,Nur_Fall17,Nur_Fall18, ";
                SQL = SQL + ComNum.VBLF + " Nur_Fall19,Nur_Fall20,Nur_Fall21,Nur_Fall22,Nur_Fall23,Nur_Fall24,Nur_Fall25,Nur_Fall26,Nur_Fall27,ROWID, PRTYN, ";
                SQL = SQL + ComNum.VBLF + " ERRGUBUN, ERRGRADE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "NUR_FALL_REPORT ";
                if (GstrWardCodes =="OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + VB.Val(argIpdNo).ToString("00000000") + "' ";
                }
                else
                { 
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO =" + VB.Val(argIpdNo) + " ";
                }

                if (argROWID != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND ROWID = '" + argROWID + "' ";
                }
                if (argDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + argDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + argDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE DESC        ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE DESC        ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0 && argGbn == "1")
                {
                    //FstrFlag = "Y";
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    //기본사항
                    ssView_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[4, 4].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[4, 6].Text = dt.Rows[0]["ActDate"].ToString().Trim();

                    ssView_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[5, 6].Text = dt.Rows[0]["SeekDate"].ToString().Trim();

                    ssView_Sheet1.Cells[6, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[6, 4].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[6, 6].Text = dt.Rows[0]["ReturnDate"].ToString().Trim();

                    ssView_Sheet1.Cells[7, 2].Text = dt.Rows[0]["DIAGNOSYS"].ToString().Trim();
                    ssView_Sheet1.Cells[7, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["EntSabun"].ToString().Trim());
                    ssView_Sheet1.Cells[7, 9].Text = dt.Rows[0]["EtcName"].ToString().Trim();

                    ssView_Sheet1.Cells[8, 2].Text = dt.Rows[0]["ERRGUBUN"].ToString().Trim();
                    ssView_Sheet1.Cells[8, 4].Text = dt.Rows[0]["ERRGRADE"].ToString().Trim();

                    //환자관련사항
                    ssView_Sheet1.Cells[9 + 1, 2].Text = dt.Rows[0]["Weight"].ToString().Trim();
                    ssView_Sheet1.Cells[9 + 1, 4].Text = dt.Rows[0]["Height"].ToString().Trim();

                    ssView_Sheet1.Cells[10 + 1, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[10 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 2, 1);
                    ssView_Sheet1.Cells[10 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[10 + 1, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 4, 1);
                    ssView_Sheet1.Cells[10 + 1, 8].Text = VB.Mid(dt.Rows[0]["Nur_Fall1"].ToString().Trim(), 5, 1);

                    ssView_Sheet1.Cells[11 + 1, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[11 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 2, 1);
                    ssView_Sheet1.Cells[11 + 1, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[11 + 1, 8].Text = VB.Mid(dt.Rows[0]["Nur_Fall2"].ToString().Trim(), 4, 1);

                    if (dt.Rows[0]["Nur_Fall3"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[12 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[12 + 1, 7].Value = true;
                    }

                    ssView_Sheet1.Cells[12 + 1, 5].Text = dt.Rows[0]["Nur_Fall3_Etc"].ToString().Trim();

                    ssView_Sheet1.Cells[13 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[13 + 1, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 2, 1);
                    ssView_Sheet1.Cells[13 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[13 + 1, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 4, 1);
                    ssView_Sheet1.Cells[13 + 1, 7].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 5, 1);
                    ssView_Sheet1.Cells[13 + 1, 9].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 6, 1);

                    ssView_Sheet1.Cells[14 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 7, 1);
                    ssView_Sheet1.Cells[14 + 1, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 8, 1);
                    ssView_Sheet1.Cells[14 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 9, 1);
                    ssView_Sheet1.Cells[14 + 1, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 10, 1);
                    ssView_Sheet1.Cells[14 + 1, 9].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 11, 1);

                    //ssView_Sheet1.Cells[14 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 1, 1);
                    //ssView_Sheet1.Cells[14 + 1, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 2, 1);
                    //ssView_Sheet1.Cells[14 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 3, 1);
                    //ssView_Sheet1.Cells[14 + 1, 6].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 4, 1);
                    //ssView_Sheet1.Cells[14 + 1, 9].Text = VB.Mid(dt.Rows[0]["Nur_Fall4"].ToString().Trim(), 5, 1);

                    ssView_Sheet1.Cells[15 + 1, 4].Text = dt.Rows[0]["Nur_Fall4_Etc"].ToString().Trim();

                    ssView_Sheet1.Cells[17 + 1, 2].Text = dt.Rows[0]["Nur_Fall5"].ToString().Trim();

                    //낙상유형
                    ssView_Sheet1.Cells[19 + 1, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[19 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 2, 1);

                    ssView_Sheet1.Cells[20 + 1, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[20 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 4, 1);

                    ssView_Sheet1.Cells[21 + 1, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 5, 1);
                    ssView_Sheet1.Cells[21 + 1, 2].Text = dt.Rows[0]["Nur_Fall6_Etc"].ToString().Trim();

                    if (VB.Mid(dt.Rows[0]["Nur_Fall6"].ToString().Trim(), 1, 1) == "1")
                    {
                        setSpread(true);
                    }
                    else
                    {
                        setSpread(false);
                    }

                    //낙상장소
                    ssView_Sheet1.Cells[23 + 1, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[23 + 1, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 2, 1);
                    ssView_Sheet1.Cells[23 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[23 + 1, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 4, 1);

                    ssView_Sheet1.Cells[24 + 1, 1].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 5, 1);
                    ssView_Sheet1.Cells[24 + 1, 2].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 6, 1);
                    ssView_Sheet1.Cells[24 + 1, 3].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 7, 1);
                    ssView_Sheet1.Cells[24 + 1, 4].Text = VB.Mid(dt.Rows[0]["Nur_Fall7"].ToString().Trim(), 8, 1);

                    //침대낙상시
                    if (dt.Rows[0]["Nur_Fall8"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[26 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[26 + 1, 4].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall9"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[27 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[27 + 1, 4].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall10"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[28 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[28 + 1, 4].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall11"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[29 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[29 + 1, 4].Value = true;
                    }
                    ssView_Sheet1.Cells[30 + 1, 2].Text = dt.Rows[0]["Nur_Fall11_Etc"].ToString().Trim();

                    //미끄러지거나 넘어진 경우
                    if (dt.Rows[0]["Nur_Fall12"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[32 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[32 + 1, 4].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall13"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[33 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[33 + 1, 4].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall14"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[34 + 1, 3].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[34 + 1, 4].Value = true;
                    }

                    //간호중재
                    if (dt.Rows[0]["Nur_Fall15"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[19 + 1, 8].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[19 + 1, 9].Value = true;
                    }
                    ssView_Sheet1.Cells[20 + 1, 7].Text = dt.Rows[0]["Nur_Fall16"].ToString().Trim();
                    ssView_Sheet1.Cells[21 + 1, 7].Text = dt.Rows[0]["Nur_Fall17"].ToString().Trim();
                    ssView_Sheet1.Cells[23 + 1, 5].Text = dt.Rows[0]["Nur_Fall18"].ToString().Trim();
                    ssView_Sheet1.Cells[24 + 1, 6].Text = dt.Rows[0]["Nur_Fall19"].ToString().Trim();

                    if (dt.Rows[0]["Nur_Fall20"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[25 + 1, 8].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[25 + 1, 9].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall21"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[26 + 1, 8].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[26 + 1, 9].Value = true;
                    }

                    if (dt.Rows[0]["Nur_Fall22"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[27 + 1, 8].Value = true;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[27 + 1, 9].Value = true;
                    }
                    ssView_Sheet1.Cells[29 + 1, 5].Text = dt.Rows[0]["Nur_Fall23"].ToString().Trim();

                    //낙상결과
                    ssView_Sheet1.Cells[32 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 1, 1);
                    ssView_Sheet1.Cells[32 + 1, 7].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 2, 1);

                    ssView_Sheet1.Cells[33 + 1, 5].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 3, 1);
                    ssView_Sheet1.Cells[33 + 1, 7].Text = VB.Mid(dt.Rows[0]["Nur_Fall24"].ToString().Trim(), 4, 1);

                    //환자의 신체적 손상 및 치료
                    for (k = 38; k < 45; k++)
                    {
                        if (VB.Mid(dt.Rows[0]["Nur_Fall25"].ToString().Trim(), k - 37, 1) == "1")
                        {
                            ssView_Sheet1.Cells[k - 1 + 1, 5].Value = true;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[k - 1 + 1, 5].Value = false;
                        }
                    }

                    for (k = 38; k < 44; k++)
                    {
                        if (VB.Mid(dt.Rows[0]["Nur_Fall26"].ToString().Trim(), k - 37, 1) == "1")
                        {
                            ssView_Sheet1.Cells[k - 1 + 1, 7].Value = true;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[k - 1 + 1, 7].Value = false;
                        }
                    }

                    //낙상발생 상황
                    ssView_Sheet1.Cells[36 + 1, 1].Text = dt.Rows[0]["Nur_Fall27"].ToString().Trim();

                    ssView_Sheet1.Cells[45 + 1, 1].Text = dt.Rows[0]["IpdNo"].ToString().Trim();
                    ssView_Sheet1.Cells[45 + 1, 2].Text = dt.Rows[0]["BDate"].ToString().Trim();
                    ssView_Sheet1.Cells[45 + 1, 3].Text = Convert.ToDateTime(dt.Rows[0]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[45 + 1, 4].Text = dt.Rows[0]["PRTYN"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    //FstrFlag = "";
                    FstrROWID = "";

                    if (GstrWardCodes != "OPD" && GstrWardCodes != "OP" && cboWard.Text.Trim() != "OP")
                    {
                        SQL = "";
                        SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate, DECODE(b.RoomCode,233,'SICU',234,'MICU',WardCode) WardCode, ";
                        SQL = SQL + ComNum.VBLF + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_MASTER a,  " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Ipdno=b.Ipdno(+) ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpdNo =" + VB.Val(argIpdNo) + " ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = " SELECT A.Pano,A.DeptCode,A.SName,A.Sex,A.Age,A.DrCode,Bi,'' WardCode, C.ILLNAmek Diagnosis, 0 Ipdno,";
                        SQL = SQL + ComNum.VBLF + "   TO_CHAR(ACTDATE,'YYYY-MM-DD') ActDate, C.ILLNAmek";
                        if (argDept == "ER")
                        {
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_MED + "OCS_EILLS B, " + ComNum.DB_PMPA + "BAS_ILLS C";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_MED + "OCS_OILLS B, " + ComNum.DB_PMPA + "BAS_ILLS C";
                        }
                        SQL = SQL + ComNum.VBLF + "  WHERE ActDate =TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND A.Pano ='" + VB.Val(argIpdNo).ToString("00000000") + "'";
                        if (GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                        {
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND A.DeptCode ='" + argDept + "'";
                        }
                        SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PTNO(+)";
                        SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE = B.DEPTCODE(+)";
                        SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = B.BDATE(+)";
                        SQL = SQL + ComNum.VBLF + "    AND B.ILLCODE = C.ILLCODE(+)";
                        if (argDept == "ER")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND B.DGOTDGGB  = 1";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND B.SEQNO = 1";
                        }
                    }
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[4, 4].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[4, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
                        ssView_Sheet1.Cells[5, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
                        ssView_Sheet1.Cells[6, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");

                        ssView_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();

                        ssView_Sheet1.Cells[6, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                        ssView_Sheet1.Cells[6, 4].Text = dt.Rows[0]["DeptCode"].ToString().Trim();

                        ssView_Sheet1.Cells[7, 2].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                        ssView_Sheet1.Cells[7, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                        ssView_Sheet1.Cells[45 + 1, 1].Text = dt.Rows[0]["IpdNo"].ToString().Trim();
                        ssView_Sheet1.Cells[45 + 1, 4].Text = "N";
                    }
                    dt.Dispose();
                    dt = null;



                    SQL = "";
                    SQL = " SELECT WEIGHT, HEIGHT, TO_CHAR(INDATE,'YYYYMMDD') AS  INDATE";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + VB.Val(ssView_Sheet1.Cells[45 + 1, 1].Text.Trim()) + " ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[9 + 1, 4].Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                        ssView_Sheet1.Cells[9 + 1, 2].Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                        strInDate = dt.Rows[0]["INDATE"].ToString().Trim();

                    }
                    dt.Dispose();
                    dt = null;

                    if (ssView_Sheet1.Cells[9 + 1, 4].Text.Trim() == "0" || ssView_Sheet1.Cells[9 + 1, 2].Text.Trim() == "0"
                        || ssView_Sheet1.Cells[9 + 1, 4].Text.Trim() == "" || ssView_Sheet1.Cells[9 + 1, 2].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL = " SELECT EMRNO, FORMNO";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.EMRXMLMST ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + ssView_Sheet1.Cells[5, 2].Text.Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                        SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-").Replace("-", "") + "'";
                        SQL = SQL + ComNum.VBLF + "      AND FORMNO IN ('1545','1553','1554','1556','1558','2285','2294','2295','2305','2311','2356')";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strEMRNO = dt.Rows[0]["EMRNO"].ToString().Trim();
                            strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;

                        if (strEMRNO != "" && strFormNo != "")
                        {
                            SQL = "";
                            switch (strFormNo)
                            {
                                case "1545":
                                case "1553":
                                    SQL = " SELECT extractValue(chartxml, '//it14') HEIGHT, extractValue(chartxml, '//it15') WEIGHT";
                                    break;
                                case "1554":
                                    SQL = " SELECT extractValue(chartxml, '//it47') HEIGHT, extractValue(chartxml, '//it48') WEIGHT";
                                    break;
                                case "1556":
                                    SQL = " SELECT extractValue(chartxml, '//it2') HEIGHT, extractValue(chartxml, '//it1') WEIGHT";
                                    break;
                                case "1558":
                                    SQL = " SELECT extractValue(chartxml, '//it13') HEIGHT, extractValue(chartxml, '//it14') WEIGHT";
                                    break;
                                case "2285":
                                    SQL = " SELECT extractValue(chartxml, '//it1271') HEIGHT, extractValue(chartxml, '//it83') WEIGHT";
                                    break;
                                case "2294":
                                    SQL = " SELECT extractValue(chartxml, '//it51') HEIGHT, extractValue(chartxml, '//it52') WEIGHT";
                                    break;
                                case "2295":
                                    SQL = " SELECT extractValue(chartxml, '//it96') HEIGHT, extractValue(chartxml, '//it45') WEIGHT";
                                    break;
                                case "2305":
                                    SQL = " SELECT extractValue(chartxml, '//it51') HEIGHT, extractValue(chartxml, '//it49') WEIGHT";
                                    break;
                                case "2311":
                                    SQL = " SELECT extractValue(chartxml, '//it45') HEIGHT, extractValue(chartxml, '//it46') WEIGHT";
                                    break;
                                case "2356":
                                    SQL = " SELECT extractValue(chartxml, '//it50') HEIGHT, extractValue(chartxml, '//it49') WEIGHT";
                                    break;
                            }
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEMRNO;                     

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                if (VB.IsNumeric(dt.Rows[0]["HEIGHT"].ToString().Trim()))
                                {
                                    ssView_Sheet1.Cells[9 + 1, 4].Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[9 + 1, 4].Text = "0";
                                }
                                if (VB.IsNumeric(dt.Rows[0]["WEIGHT"].ToString().Trim()))
                                {
                                    ssView_Sheet1.Cells[9 + 1, 2].Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[9 + 1, 2].Text = "0";
                                }
                            }

                            dt.Dispose();
                            dt = null;
                        }
                        else
                        {
                            #region 신규
                            //신규기록지에서 값을 읽어 표시한다
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT ";
                            SQL = SQL + ComNum.VBLF + "   (SELECT R.ITEMVALUE ";
                            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTROW R ";
                            SQL = SQL + ComNum.VBLF + "    WHERE R.EMRNO =  ";
                            SQL = SQL + ComNum.VBLF + "                (SELECT  ";
                            SQL = SQL + ComNum.VBLF + "                    MAX(A.EMRNO) ";
                            SQL = SQL + ComNum.VBLF + "                FROM  KOSMOS_EMR.AEMRCHARTMST A ";
                            SQL = SQL + ComNum.VBLF + "                INNER JOIN KOSMOS_EMR.AEMRCHARTROW B ";
                            SQL = SQL + ComNum.VBLF + "                    ON A.EMRNO = B.EMRNO ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.EMRNOHIS = B.EMRNOHIS ";
                            SQL = SQL + ComNum.VBLF + "                    AND B.ITEMNO  = 'I0000000002' ";
                            SQL = SQL + ComNum.VBLF + "                WHERE A.PTNO = '" + ssView_Sheet1.Cells[5, 2].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.FORMNO IN(2294, 2295, 2311, 2356, 3150) ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE >= '" + strInDate.Replace("-", "") + "' ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ";
                            SQL = SQL + ComNum.VBLF + "                ) ";
                            SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO  = 'I0000000002' ";
                            SQL = SQL + ComNum.VBLF + "    ) AS HEIGHT, ";
                            SQL = SQL + ComNum.VBLF + "    (SELECT R.ITEMVALUE ";
                            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTROW R ";
                            SQL = SQL + ComNum.VBLF + "    WHERE R.EMRNO =  ";
                            SQL = SQL + ComNum.VBLF + "                (SELECT  ";
                            SQL = SQL + ComNum.VBLF + "                    MAX(A.EMRNO) ";
                            SQL = SQL + ComNum.VBLF + "                FROM  KOSMOS_EMR.AEMRCHARTMST A ";
                            SQL = SQL + ComNum.VBLF + "                INNER JOIN KOSMOS_EMR.AEMRCHARTROW B ";
                            SQL = SQL + ComNum.VBLF + "                    ON A.EMRNO = B.EMRNO ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.EMRNOHIS = B.EMRNOHIS ";
                            SQL = SQL + ComNum.VBLF + "                    AND B.ITEMNO  = 'I0000000418' ";
                            SQL = SQL + ComNum.VBLF + "                WHERE A.PTNO = '" + ssView_Sheet1.Cells[5, 2].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.FORMNO IN(2294, 2295, 2311, 2356, 3150) ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE >= '" + strInDate.Replace("-", "") + "' ";
                            SQL = SQL + ComNum.VBLF + "                    AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ";
                            SQL = SQL + ComNum.VBLF + "                ) ";
                            SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO  = 'I0000000418' ";
                            SQL = SQL + ComNum.VBLF + "    ) AS WEIGHT ";
                            SQL = SQL + ComNum.VBLF + "FROM DUAL ";
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                if (VB.IsNumeric(dt.Rows[0]["HEIGHT"].ToString().Trim()))
                                {
                                    ssView_Sheet1.Cells[9 + 1, 4].Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                                }

                                if (VB.IsNumeric(dt.Rows[0]["WEIGHT"].ToString().Trim()))
                                {
                                    ssView_Sheet1.Cells[9 + 1, 2].Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                                }
                            }
                            dt.Dispose();
                            dt = null;
                            #endregion
                        }
                    }
                }

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                GstrHelpCode = "";
                //FstrFlag = "";

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void cboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','NR','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboWard.Items.Clear();
                cboWard.Items.Add("전체");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }
                //cboWard.Items.Add("SICU");
                //cboWard.Items.Add("MICU");
                cboWard.Items.Add("OPD");
                cboWard.Items.Add("OP");

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = -1;
                for (i = 0; i < cboWard.Items.Count; i++)
                {
                    if (cboWard.Items.IndexOf(gsWard) == i)
                    {
                        cboWard.SelectedIndex = i;
                        cboWard.Enabled = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssHistory_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssHistory_Sheet1.RowCount == 0)
            {
                return;
            }
            
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssHistory_Sheet1.Cells[0, 0, ssHistory_Sheet1.RowCount - 1, ssHistory_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssHistory_Sheet1.Cells[e.Row, 0, e.Row, ssHistory_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strIPDNO = "";
            string strDate = "";
            string strROWID = "";

            strDate = ssHistory_Sheet1.Cells[e.Row, 1].Text;
            if (cboWard.Text.Trim() == "OPD")
            {
                strIPDNO = ssHistory_Sheet1.Cells[e.Row, 2].Text;
            }
            else
            {
                strIPDNO = ssHistory_Sheet1.Cells[e.Row, 6].Text;
            }
            strROWID = ssHistory_Sheet1.Cells[e.Row, 9].Text;

            SS_Display("1", strIPDNO, strDate, "",strROWID);
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strIPDNO = "";
            string strBDATE = "";
            string strDeptCode = "";

            strBDATE = ssList_Sheet1.Cells[e.Row, 0].Text;
            strPano = ssList_Sheet1.Cells[e.Row, 1].Text.PadLeft(8, '0');
            strDeptCode = ssList_Sheet1.Cells[e.Row, 4].Text;
            strIPDNO = ssList_Sheet1.Cells[e.Row, 5].Text;

            SCREEN_CLEAR();

            try
            {
                if (ssList_Sheet1.Cells[e.Row, 6].Text == "OK")
                {
                    if (ComFunc.MsgBoxQ("신규로 작성하려면 'YES', 기존자료를 보려면 'NO'를 선택하세요.") == DialogResult.No)
                    {
                        FstrNEW = "";
                        SQL = "";
                        SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD') InDate,  ";
                        SQL = SQL + ComNum.VBLF + " SNAME, ROOMCODE,DEPTCODE,ENTDATE,WardCode,ROWID, PRTYN ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ssHistory_Sheet1.RowCount = dt.Rows.Count;
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                                //ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PRTYN"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            }
                            dt.Dispose();
                            dt = null;
                        }
                        else
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("기존에 작성된 자료가 없습니다. 새로운 자료를 작성합니다.");
                            if (cboWard.Text == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                            {
                                SS_Display("1", strIPDNO, strBDATE, strDeptCode);
                            }
                            else
                            {
                                SS_Display("1", strIPDNO, strBDATE);
                            }
                        }
                    }
                    else
                    {
                        FstrNEW = "OK";
                        if (cboWard.Text == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                        {
                            SS_Display("1", strIPDNO, strBDATE, strDeptCode);
                        }
                        else
                        {
                            SS_Display("1", strIPDNO, strBDATE);
                        }
                    }
                }
                else
                {
                    if (ssList_Sheet1.Cells[e.Row, 6].BackColor == Color.FromArgb(128, 255, 128))
                    {
                        FstrNEW = "";
                        SQL = "";
                        SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD') InDate,  ";
                        SQL = SQL + ComNum.VBLF + " SNAME, ROOMCODE,DEPTCODE,ENTDATE,WardCode,ROWID, PRTYN ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssHistory_Sheet1.RowCount = dt.Rows.Count;
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                                //ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                                ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                                ssHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PRTYN"].ToString().Trim();
                            }
                            dt.Dispose();
                            dt = null;
                        }
                    }
                    else
                    {
                        SCREEN_CLEAR();
                    }
                }

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void Pano_history()
        {
          
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strIPDNO = "";
            string strBDATE = "";
            string strDeptCode = "";

                      
            strPano = ssView_Sheet1.Cells[5, 2].Text;
            strDeptCode = ssView_Sheet1.Cells[6, 4].Text;
          

            SCREEN_CLEAR();

            try
            {
                FstrNEW = "";
                SQL = "";
                SQL = "  SELECT PANO,IPDNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(InDate,'YYYY-MM-DD') InDate,  ";
                SQL = SQL + ComNum.VBLF + " SNAME, ROOMCODE,DEPTCODE,ENTDATE,WardCode,ROWID, PRTYN ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_FALL_REPORT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssHistory_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssHistory_Sheet1.Cells[i, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        //ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssHistory_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PRTYN"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("기존에 작성된 자료가 없습니다. 새로운 자료를 작성합니다.");
                    if (cboWard.Text == "OPD" || GstrWardCodes == "OP" || cboWard.Text.Trim() == "OP")
                    {
                        SS_Display("1", strIPDNO, strBDATE, strDeptCode);
                    }
                    else
                    {
                        SS_Display("1", strIPDNO, strBDATE);
                    }
                }

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssList2_Sheet1.Cells[0, 0, ssList2_Sheet1.RowCount - 1, ssList2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList2_Sheet1.Cells[e.Row, 0, e.Row, ssList2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            string strPano = "";
            string strDeptCode = "";
            string strDRCD = "";

            strPano = ssList2_Sheet1.Cells[e.Row, 0].Text;  //번호
            strDeptCode = ssList2_Sheet1.Cells[e.Row, 2].Text;  //과별
            strDRCD = ssList2_Sheet1.Cells[e.Row, 3].Text;  //의사

            GstrHelpCode = ssList2_Sheet1.Cells[e.Row, 5].Text; //진료일자
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == 20 && e.Column == 7)
            {
                clsPublic.GstrCalDate = "";
                frmCalendar2 frmCalendar2X = new frmCalendar2();
                frmCalendar2X.StartPosition = FormStartPosition.CenterParent;
                frmCalendar2X.ShowDialog();
                frmCalendar2X.Dispose();
                frmCalendar2X = null;

                ssView_Sheet1.Cells[e.Row, e.Column].Text = clsPublic.GstrCalDate;

                clsPublic.GstrCalDate = "";
            }




        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.Cells[5, 2].Text.Trim().Length > 0)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name.Equals("frmEmrViewer"))
                    {
                        if (frmEmrViewer == null)
                        {
                            frm.Dispose();
                            break;
                        }
                        else
                        {
                            frmEmrViewer.SetNewPatient(ssView_Sheet1.Cells[5, 2].Text.Trim());
                            return;
                        }
                    }
                }

                frmEmrViewer = new frmEmrViewer(ssView_Sheet1.Cells[5, 2].Text.Trim());
                frmEmrViewer.StartPosition = FormStartPosition.CenterParent;
                frmEmrViewer.Show(this);
                //clsVbEmr.EXECUTE_TextEmrViewEx(ssView_Sheet1.Cells[5, 2].Text.Trim(), clsType.User.Sabun);
                return;
            }

            //if (ssList_Sheet1.RowCount == 0) return;

            ////clsVbEmr.EXECUTE_TextEmrViewEx(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim(), clsType.User.Sabun);

            //foreach (Form frm in Application.OpenForms)
            //{
            //    if (frm.Name.Equals("frmEmrViewer"))
            //    {
            //        if (frmEmrViewer == null)
            //        {
            //            frm.Dispose();
            //            break;
            //        }
            //        else
            //        {
            //            frmEmrViewer.SetNewPatient(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim());
            //            return;
            //        }
            //    }
            //}

            //frmEmrViewer = new frmEmrViewer(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim());
            //frmEmrViewer.StartPosition = FormStartPosition.CenterParent;
            //frmEmrViewer.Show(this);
            //return;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            string strPano = "";
            
            DataTable dt = new DataTable();

            strPano = VB.InputBox("입원환자의 등록번호를 입력하시기 바랍니다.", "입원환자 외래 낙상");

            strPano = VB.Right("00000000" + strPano, 8);

            SQL = "";
            SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate, DECODE(b.RoomCode,233,'SICU',234,'MICU',WardCode) WardCode, ";
            SQL = SQL + ComNum.VBLF + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_MASTER a,  " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
            SQL = SQL + ComNum.VBLF + " WHERE a.Ipdno=b.Ipdno(+) ";
            SQL = SQL + ComNum.VBLF + "  AND a.PANO ='" + strPano + "' ";
            SQL = SQL + ComNum.VBLF + "  AND b.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssView_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                ssView_Sheet1.Cells[4, 4].Text = dt.Rows[0]["Sex"].ToString().Trim() + "/" + dt.Rows[0]["Age"].ToString().Trim();
                ssView_Sheet1.Cells[4, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
                ssView_Sheet1.Cells[5, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
                ssView_Sheet1.Cells[6, 6].Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");

                ssView_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Pano"].ToString().Trim();

                ssView_Sheet1.Cells[6, 2].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                ssView_Sheet1.Cells[6, 4].Text = dt.Rows[0]["DeptCode"].ToString().Trim();

                ssView_Sheet1.Cells[7, 2].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                ssView_Sheet1.Cells[7, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                ssView_Sheet1.Cells[45 + 1, 1].Text = "0";
                ssView_Sheet1.Cells[45 + 1, 4].Text = "N";
            }

            dt.Dispose();
            dt = null;
            Pano_history();
        }


        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {


            if (e.Column == 1 && e.Row == 20)
            {

                setSpread(bool.Parse(ssView_Sheet1.Cells[e.Row, e.Column].Text));
                
            }

            if (e.Row == 8 && e.Column == 2)
            {

                string strErrGubun = "";

                FarPoint.Win.Spread.CellType.ComboBoxCellType cboCell = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                strErrGubun = ssView_Sheet1.Cells[8, 2].Text.Trim();

                if (strErrGubun == "근접오류")
                {
                    cboCell.Items = (new String[] { "등급1. 오류가 발생할 위험이 있는 상황", "등급2. 오류가 발생하였으나 환자에게 도달하지 않음" });

                }
                else if (strErrGubun == "위해사건")
                {
                    cboCell.Items = (new String[] { "등급3. 환자에게 투여/적용되었으나 해가 없음", "등급4. 환자에게 투여/적용되었으며 추가적인 관찰이 필요함",
                                                    "등급5. 일시적 손상으로 중재가 필요함", "등급6. 일시적 손상으로 입원기간이 연장됨",
                                                    "등급7. 생명을 유지하기 위해 필수적인 중재가 필요함" });
                }
                else if (strErrGubun == "적신호사건")
                {
                    cboCell.Items = (new String[] { "등급8. 영구적 손상", "등급9. 환자사망" });
                }

                if (strErrGubun == "근접오류" || strErrGubun == "위해사건" || strErrGubun == "적신호사건")
                {
                    cboCell.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    cboCell.Editable = true ;
                    cboCell.MaxDrop = 3;
                    ssView_Sheet1.Cells[8, 4].Text = "";
                    ssView_Sheet1.Cells[8, 4].CellType = cboCell;
                }


            }
        }

        private void frmFallReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewer != null)
            {
                frmEmrViewer.Dispose();
                frmEmrViewer = null;
            }
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            //int i = 0;
            //int j = 0;
            int intCol = ssView_Sheet1.ActiveColumnIndex;
            int intRow = ssView_Sheet1.ActiveRowIndex;
            //string strData = "";

            //bool bValue = false;
            
            if (intCol == 1 && intRow == 20)
            {

                //setSpread(bool.Parse(ssView_Sheet1.Cells[intRow, intCol].Text));
            }
                
        }

        private void setSpread(bool bChecked)
        {
            int i = 0;
            int j = 0;
            //string strData = "";

            bool bValue = false;

            if (bChecked)
            {
                bValue = false;
            }
            else
            {
                bValue = true;
            }
            for (i = 27; i <= 30; i++)
            {
                for (j = 3; j <= 4; j++)
                {
                    ssView_Sheet1.Cells[i, j].Locked = bValue;
                    ssView_Sheet1.Cells[i, j].Value = false;
                }
            }
        }
    }
}
