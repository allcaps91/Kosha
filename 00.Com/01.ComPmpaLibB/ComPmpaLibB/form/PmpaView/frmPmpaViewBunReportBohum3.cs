using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewBunReportBohum3.cs
    /// Description     : 보험 차상위 2종F 대상 장애의료비 총괄표 (개인별)
    /// Author          : 박창욱
    /// Create Date     : 2017-09-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM104.FRM(FrmBunReportBohum3.frm) >> frmPmpaViewBunReportBohum3.cs 폼이름 재정의" />	
    public partial class frmPmpaViewBunReportBohum3 : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        frmPmpaViewGelSearch frm = null;

        double nSubAmt = 0;
        double nSubTotAmt = 0;
        double nTotAmt = 0;
        string strYY = ""; 
        string strMM = "";
        string GstrJob = "";
        string GstrMiaCode = "";
        string GstrMiaName = "";

        public frmPmpaViewBunReportBohum3()
        {
            InitializeComponent();
        }

        string READ_DTNO(string argDtno)
        {
            string rtnVal = "";

            switch (argDtno)
            {
                case "11":
                    rtnVal = "내과분야";
                    break;
                case "12":
                    rtnVal = "인공신장";
                    break;
                case "13":
                    rtnVal = "신경정신과";
                    break;
                case "14":
                    rtnVal = "신경과";
                    break;
                case "21":
                    rtnVal = "일반외과";
                    break;
                case "22":
                    rtnVal = "흉부외과";
                    break;
                case "23":
                    rtnVal = "신경외과";
                    break;
                case "24":
                    rtnVal = "정형외과";
                    break;
                case "26":
                    rtnVal = "통증치료과";
                    break;
                case "27":
                    rtnVal = "응급실";
                    break;
                case "28":
                    rtnVal = "재활의학과";
                    break;
                case "31":
                    rtnVal = "산부인과";
                    break;
                case "32":
                    rtnVal = "소아과";
                    break;
                case "41":
                    rtnVal = "안과";
                    break;
                case "42":
                    rtnVal = "이비인후과";
                    break;
                case "51":
                    rtnVal = "피부과";
                    break;
                case "52":
                    rtnVal = "비뇨기과";
                    break;
                case "61":
                    rtnVal = "치과";
                    break;
            }
            return rtnVal;
        }

        int DEPT_SET(string argDept)
        {
            int rtnVal = 0;

            switch (argDept)
            {
                case "11":
                    rtnVal = 1;
                    break;  //내과
                case "12":
                    rtnVal = 2;
                    break;  //인공신장
                case "13":
                    rtnVal = 3;
                    break;  //신경정신과
                case "14":
                    rtnVal = 4;
                    break;  //신경과
                case "21":
                    rtnVal = 8;
                    break;  //일반외과
                case "22":
                    rtnVal = 9;
                    break;  //흉부외과
                case "23":
                    rtnVal = 10;
                    break;  //신경외과
                case "24":
                    rtnVal = 11;
                    break;  //정형외과
                case "25":
                    rtnVal = 12;
                    break;  //성형외과
                case "26":
                    rtnVal = 13;
                    break;  //마취과
                case "27":
                    rtnVal = 6;
                    break;  //응급실
                case "31":
                    rtnVal = 16;
                    break;  //산과
                case "32":
                    rtnVal = 17;
                    break;  //소아과
                case "41":
                    rtnVal = 19;
                    break;  //안과
                case "42":
                    rtnVal = 20;
                    break;  //이비인후과
                case "51":
                    rtnVal = 22;
                    break;  //피부과
                case "52":
                    rtnVal = 23;
                    break;  //비뇨기과
                case "61":
                    rtnVal = 26;
                    break;  //치과
                default:
                    rtnVal = 27;
                    break; //기타과
            }
            return rtnVal;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            cboBi.Focus();
            txtJepNo.Text = "";
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
           // {
           //     return; //권한 확인
           // }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strYYMM = "";
            string strROWID = "";
            string strMisuID = "";
            string strSname = "";
            string strDept = "";
            string strJiDate = "";
            string strJikiho = "";
            double nJiamt = 0;
            string strJiRemark = "";
            string strJiRemarkold = "";
            string strOK = "";

            strYY = VB.Mid(cboFYYMM.Text, 1, 4);
            strMM = VB.Mid(cboFYYMM.Text, 7, 2);
            strYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strMisuID = VB.Val(ssView_Sheet1.Cells[i , 0].Text).ToString("00000000").Trim();
                    strSname = ssView_Sheet1.Cells[i , 4].Text.Trim();
                    strDept = ssView_Sheet1.Cells[i , 7].Text.Trim();
                    strJiDate = ssView_Sheet1.Cells[i , 10].Text.Trim();
                    strJikiho = VB.Pstr(ssView_Sheet1.Cells[i , 11].Text.Trim(), ".", 1);
                    nJiamt = VB.Val(ssView_Sheet1.Cells[i , 12].Text.Trim());
                    strJiRemark = ssView_Sheet1.Cells[i, 13].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i , 14].Text.Trim();
                    strJiRemarkold = ssView_Sheet1.Cells[i, 15].Text.Trim();
                    strOK = "OK";

                    if (strJiRemark != strJiRemarkold)
                    {
                        //청구 UPDATE
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MIR_INSID SET ";
                        SQL = SQL + ComNum.VBLF + "       JIREMARK = '" + strJiRemark + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            strOK = "NO";
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    if (nJiamt != 0 || strJikiho != "")
                    {
                        //청구 UPDATE
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MIR_INSID SET ";
                        SQL = SQL + ComNum.VBLF + "       JIDATE= TO_DATE('" + strJiDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + "       JIAMT = '" + nJiamt + "' ,";
                        SQL = SQL + ComNum.VBLF + "       JIKIHO = '" + strJikiho + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            strOK = "NO";
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                }

                SearchData();

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strIO = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            if (rdoIO0.Checked == true)
            {
                strIO = "구분 : 입원 " + "[ " + cboBi.Text.Trim() + " ]";
            }
            if (rdoIO1.Checked == true)
            {
                strIO = "구분 : 외래 " + "[ " + cboBi.Text.Trim() + " ]";
            }
            if (rdoIO2.Checked == true)
            {
                strIO = "구분 : 전체 " + "[ " + cboBi.Text.Trim() + " ]";
            }

            strTitle = "보험 차상위2종 F대상 장애인의료비 총괄표(개인별)";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(strIO + "출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboFYYMM.Text + "~" + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSave_Click(object sender, EventArgs e) 
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nJiamt = 0;
            string strYYMM = "";
            string strROWID = "";
            string strMisuID = "";
            string strSname = "";
            string strDept = "";
            string strJiDate = "";
            string strJikiho = "";
            string strJiKihoName = "";
            string strJiRemark = "";
            string strFlag = "";
            string strKiho = "";
            string strKihoName = "";
            string strOK = "";

            strYY = VB.Mid(cboFYYMM.Text, 1, 4);
            strMM = VB.Mid(cboFYYMM.Text, 7, 2);
            strYYMM = VB.Left(cboFYYMM.Text, 4) + VB.Mid(cboFYYMM.Text, 7, 2);

            if ((lblDate.Visible == true && txtDate.Visible == true) && txtDate.Text == "")
            {
                ComFunc.MsgBox("이중 접수번호 발생대상은 일자를 넣고 입금작업하세요.");
                return;
            }

            strJiDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strJikiho = VB.Pstr(cboKiho.Text, ".", 1);
            strJiKihoName = VB.Pstr(cboKiho.Text, ".", 2);

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strMisuID = VB.Val(ssView_Sheet1.Cells[i, 0].Text).ToString("00000000").Trim();
                    strKiho = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strKihoName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strSname = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strDept = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strFlag = ssView_Sheet1.Cells[i, 10].Text.Trim();
                    nJiamt = VB.Val(ssView_Sheet1.Cells[i, 12].Text.Trim());
                    strJiRemark = ssView_Sheet1.Cells[i, 13].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 14].Text.Trim();

                    if (strFlag == "" && nJiamt != 0)
                    {
                        strOK = "OK";

                        //차상위 F장애인
                        if (VB.Left(cboBi.Text, 1) == "1")
                        {
                            strKiho = "";
                            strKihoName = "";

                            if (txtJepNo.Text.Trim() != "" && (lblDate.Visible == true && txtDate.Visible == true) && txtDate.Text != "")
                            {
                                strOK = CREATE_MISU_AMT(strMisuID, nJiamt, strJiDate, strSname + " " + strDept + ":" + strJiKihoName, strKiho, txtDate.Text);
                            }
                            else
                            {
                                strOK = CREATE_MISU_AMT(strMisuID, nJiamt, strJiDate, strSname + " " + strDept + ":" + strJiKihoName, strKiho, ""); 
                            }
                        }
                        else
                        {
                            strJikiho = strKiho;
                            strJiKihoName = strKihoName;
                            if (txtJepNo.Text.Trim() != "" && (lblDate.Visible == true && txtDate.Visible == true) && txtDate.Text != "")
                            {
                                strOK = CREATE_MISU_AMT(strMisuID, nJiamt, strJiDate, strSname + " " + strDept, strKiho, txtDate.Text);
                            }
                            else
                            {
                                strOK = CREATE_MISU_AMT(strMisuID, nJiamt, strJiDate, strSname + " " + strDept, strKiho, "");
                            }
                        }

                        if (strOK == "OK")
                        {
                            //청구 UPDATE
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MIR_INSID SET ";
                            SQL = SQL + ComNum.VBLF + "        JIDATE= TO_DATE('" + strJiDate + "','YYYY-MM-DD') ,";
                            SQL = SQL + ComNum.VBLF + "        JIAMT = '" + nJiamt + "' ,";
                            SQL = SQL + ComNum.VBLF + "        JIKIHO = '" + strJikiho + "' ,";
                            SQL = SQL + ComNum.VBLF + "        JIREMARK = '" + strJiRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                strOK = "NO";
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 13].BackColor = Color.FromArgb(255, 255, 100);
                        }
                    }
                }
                if (strOK != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                }

                Cursor.Current = Cursors.Default;
                SearchData();
                lblDate.Visible = false;
                txtDate.Visible = false;
                txtDate.Text = "";
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            SearchData();
        }

        string CREATE_MISU_AMT(string argMisuid, double argJiAmt, string argJiDate, string argRemark, string argKiho = "", string argBDate = "")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nAmt3 = 0;
            string strWrtno = "";
            string strGelCode = "";
            string strIpdOpd = "";
            string strClass = "";
            string strROWID = "";
            string rtnVal = "";
            long nJamt = 0;

            rtnVal = "OK";



            //Cursor.Current = Cursors.WaitCursor;


            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT WRTNO, GELCODE, IPDOPD,";
                SQL = SQL + ComNum.VBLF + "       CLASS, AMT3, ROWID,";
                SQL = SQL + ComNum.VBLF + "       Amt2 MAmt, Amt3+Amt6+Amt7 IAMT, Amt4 SAMT,";
                SQL = SQL + ComNum.VBLF + "       Amt5 BAMT, Amt8 SAmt2";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MISUID = '" + argMisuid + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUN ='08' ";
                if (argBDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND BDate =TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }
                if (argKiho != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GELCODE ='" + argKiho + "' ";
                }
                if (VB.Left(cboBi.Text, 1) == "1")
                {
                    SQL = SQL + ComNum.VBLF + "   AND CLASS = '01' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND CLASS = '04' ";   
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    rtnVal = "NO";
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnVal = "NO";
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("미수 MST 대상자가 없습니다...입금작업실패!!");
                    return rtnVal;
                }

                strWrtno = dt.Rows[0]["WRTNO"].ToString().Trim();
                nAmt3 = VB.Val(dt.Rows[0]["amt3"].ToString().Trim());
                strGelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                strIpdOpd = dt.Rows[0]["IpdOpd"].ToString().Trim();
                strClass = dt.Rows[0]["Class"].ToString().Trim();
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                nJamt = (long)(VB.Val(dt.Rows[0]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[0]["IAmt"].ToString().Trim()) - VB.Val(dt.Rows[0]["SAmt"].ToString().Trim()) -
                        VB.Val(dt.Rows[0]["Bamt"].ToString().Trim()) - VB.Val(dt.Rows[0]["SAmt2"].ToString().Trim()) - argJiAmt);
                dt.Dispose();
                dt = null;



                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + "        (WRTNO , MISUID, BDATE, GELCODE, IPDOPD, CLASS,";
                SQL = SQL + ComNum.VBLF + "        GUBUN,QTY, TAMT, AMT, REMARK, ENTDATE, ENTPART)";
                SQL = SQL + ComNum.VBLF + " VALUES (";
                SQL = SQL + ComNum.VBLF + "               '" + strWrtno + "',";
                SQL = SQL + ComNum.VBLF + "               '" + argMisuid + "',";
                SQL = SQL + ComNum.VBLF + "               TO_DATE('" + argJiDate + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "               '" + strGelCode + "',";
                SQL = SQL + ComNum.VBLF + "               '" + strIpdOpd + "',";
                SQL = SQL + ComNum.VBLF + "               '" + strClass + "',";
                SQL = SQL + ComNum.VBLF + "               '21', '0', '0',";
                SQL = SQL + ComNum.VBLF + "               '" + argJiAmt + "',";
                SQL = SQL + ComNum.VBLF + "               '" + argRemark + "',";
                SQL = SQL + ComNum.VBLF + "               sysdate,";
                SQL = SQL + ComNum.VBLF + "               '" + clsType.User.Sabun + "'";
                SQL = SQL + ComNum.VBLF + "        )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    rtnVal = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_IDMST SET ";
                SQL = SQL + ComNum.VBLF + "       AMT3 = '" + (nAmt3 + argJiAmt) + "' ";
                if (nJamt <= 0)
                {
                    SQL = SQL + ComNum.VBLF + " , GBEND = '0' ";
                }
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    rtnVal = "NO";
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                //Cursor.Current = Cursors.Default;
                rtnVal = "OK";
                return rtnVal;
            }
            catch (Exception ex)
            {
                rtnVal = "NO";
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            SearchData();

            Cursor.Current = Cursors.Default;
        }

        void SearchData()
        {
           // if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
           // {
           //     return; //권한 확인
           // }

            int i = 0;
            int j = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            string strJepNo = "";
            string strKiho = "";
            string strYYMMF = "";
            string strYYMMT = "";
            string strDTno = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            ssView_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            ssView_Sheet1.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);

            nSubAmt = 0;
            nSubTotAmt = 0;
            nTotAmt = 0;
            nRow = 0;
            strJepNo = "";
            strKiho = "";

            strYY = VB.Mid(cboFYYMM.Text, 1, 4);
            strMM = VB.Mid(cboFYYMM.Text, 7, 2);
            strYYMMF = strYY + strMM;


            strYY = VB.Mid(cboTYYMM.Text, 1, 4);
            strMM = VB.Mid(cboTYYMM.Text, 7, 2);
            strYYMMT = strYY + strMM;

            try
            {
                //2020-11-03 심경순 요청으로 조건 X 
                //if (VB.Left(cboBi.Text, 1) == "1")
                //{
                if (VB.Left(cboBi.Text, 1) == "1")
                {
                    //접수번호 이중체크
                    SQL = "";
                    SQL = SQL + "SELECT WRTNO";
                    SQL = SQL + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL = SQL + " WHERE 1 = 1";
                    SQL = SQL + "   AND MISUID ='" + VB.Val(txtJepNo.Text).ToString("00000000") + "' ";
                    SQL = SQL + "   AND BUN ='08' ";
                    SQL = SQL + "   AND CLASS ='01' ";              
                }
                else
                {
                    SQL = "";
                    SQL = SQL + "SELECT BDATE, MISUID";
                    SQL = SQL + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL = SQL + " WHERE 1 = 1";
                    SQL = SQL + "   AND MISUID ='" + VB.Val(txtJepNo.Text).ToString("00000000") + "' ";
                    SQL = SQL + "   AND BUN ='08' ";
                    SQL = SQL + "   AND CLASS ='04' ";
                    SQL = SQL + "   GROUP BY BDATE, MISUID ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                lblDate.Visible = false;
                txtDate.Visible = false;
                txtDate.Text = "";

                //이중발생시
                if (dt.Rows.Count > 1)
                {
                    ComFunc.MsgBox("이미 같은 접수번호가 있습니다. 일자조건을 넣어주세요.");

                    lblDate.Visible = true;
                    txtDate.Visible = true;
                    txtDate.Text = "";
                }
                dt.Dispose();
                dt = null;
                //}



                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.PANO, A.SNAME, A.SEQNO,";
                SQL = SQL + ComNum.VBLF + "        A.DTNO,  A.KIHO,  a.JINDATE1,";
                SQL = SQL + ComNum.VBLF + "        A.DEPTCODE1, B.MIANAME, b.MIANAME CMIANAME,";
                SQL = SQL + ComNum.VBLF + "        A.EdiTAmt, A.EdiJAmt, A.EdiBAmt,";
                SQL = SQL + ComNum.VBLF + "        A.EdiBOAMT, C.JEPNO, c.Year,";
                SQL = SQL + ComNum.VBLF + "        A.JIDATE, A.JIKIHO, A.JIAMT,";
                SQL = SQL + ComNum.VBLF + "        A.JIREMARK, A.ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_MIA B, " + ComNum.DB_PMPA + "EDI_JEPSU C ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1 ";
                if (VB.Left(cboBi.Text, 1) == "1")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.JOHAP ='1'"; //보험
                    SQL = SQL + ComNum.VBLF + "    AND A.GBGS ='F'  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.JOHAP ='5'"; //의료급여
                    SQL = SQL + ComNum.VBLF + "   AND A.BohoJong IN ('0','1','2','3','4')";
                    SQL = SQL + ComNum.VBLF + "   AND A.EDIBOAMT <>'0'";
                }
                if (VB.Left(cboFYYMM.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + " AND A.YYMM >= '" + strYYMMF + "'";
                    SQL = SQL + ComNum.VBLF + " AND A.YYMM <= '" + strYYMMT + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND A.YYMM >= '201301'";
                }
                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IpdOpd = 'I' ";
                }
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.IpdOpd = 'O' ";
                }

                if (VB.Left(cboGb.Text, 1) == "1")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.JIDATE IS NULL ";
                }
                if (VB.Left(cboGb.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.JIDATE IS NOT NULL ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.JIKIHO = b.MIACODE(+) ";
                SQL = SQL + ComNum.VBLF + "   AND A.KIHO = B.MIACODE(+) ";
                SQL = SQL + ComNum.VBLF + "   AND A.EDIMIRNO = C.MIRNO ";

                //'접수번호로 조회
                if (txtJepNo.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND C.JEPNO = '" + txtJepNo.Text + "' ";
                }
                if (txtPano.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.PANO = '" + txtPano.Text + "' ";
                }
                if (txtKiho.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.KIHO = '" + txtKiho.Text + "'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY C.JEPNO, A.DTNO, A.KIHO, A.PANO  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                strJepNo = dt.Rows[0]["Jepno"].ToString().Trim();
                strKiho = dt.Rows[0]["Kiho"].ToString().Trim();

                for (i = 0; i < nRead; i++)
                {
                    if (dt.Rows[i]["DTNO"].ToString().Trim() == "11")
                    {
                        strDTno = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DEPTCODE1"].ToString().Trim());
                    }
                    else
                    {
                        strDTno = READ_DTNO(dt.Rows[i]["DTNO"].ToString().Trim());
                    }

                    if (strJepNo != dt.Rows[i]["Jepno"].ToString().Trim())
                    {
                        strJepNo = dt.Rows[i]["JepNo"].ToString().Trim();
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strJepNo;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["kiho"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["MIANAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["JINDATE1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["DEPTCODE1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim()).ToString("###,###,###,###,##0");
                    nSubAmt += VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim());
                    nSubTotAmt += VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim());
                    nTotAmt += VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim());

                    if (VB.Left(cboBi.Text, 1) == "1")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SAMT13";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0202";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND JEPNO = '" + strJepNo + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MIRSEQ ='" + dt.Rows[i]["SEQNO"].ToString().Trim() + "'"; 
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            int EDIBOTAMT = 0;
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                EDIBOTAMT = Convert.ToInt32(VB.Val(dt1.Rows[j]["SAMT13"].ToString().Trim()));
                            }
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = EDIBOTAMT.ToString("###,###,###,##0");
                            if (VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim()) != EDIBOTAMT)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 9].BackColor = Color.FromArgb(200, 255, 255);
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT SUM(SAMT7) SAMT7";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0602";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND JEPNO = '" + strJepNo + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND MIRSEQ ='" + dt.Rows[i]["SEQNO"].ToString().Trim() + "'";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = VB.Val(dt1.Rows[0]["SAMT7"].ToString().Trim()).ToString("###,###,###,##0");
                            if (VB.Val(dt.Rows[i]["EdiBOAmt"].ToString().Trim()) != VB.Val(dt1.Rows[0]["SAMT7"].ToString().Trim()))
                            {
                                ssView_Sheet1.Cells[nRow - 1, 9].BackColor = Color.FromArgb(200, 255, 255);
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["JIDATE"].ToString().Trim();
                    if (dt.Rows[i]["Jikiho"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["JIKIHO"].ToString().Trim() + "." + dt.Rows[i]["CMIANAME"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["JIAMT"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["JIREMARK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["JIREMARK"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboFYYMM_Leave(object sender, EventArgs e)
        {
            cboTYYMM.Text = cboFYYMM.Text;
        }

        private void frmPmpaViewBunReportBohum3_Load(object sender, EventArgs e)
        {
           // if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
           //  {
           //     this.Close(); //폼 권한 조회
           //     return;
           // }
           // ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboBi.Items.Clear();
            cboBi.Items.Add("1.차상위F장애인");
            cboBi.Items.Add("2.의료급여장애인");

            if (GstrJob == "1")
            {
                cboBi.SelectedIndex = 0;
                this.Text = "1.차상위F장애인 총괄표 개인별";
            }
            else
            {
                cboBi.SelectedIndex = 1;
                this.Text = "2.의료급여장애인 총괄표 개인별";
            }


            cboFYYMM.Items.Clear();
            cboTYYMM.Items.Clear();

           

            int j = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));

            cboFYYMM.Items.Add("****.전체년월");
            cboTYYMM.Items.Add("****.전체년월");

            for (j = 1; j <= 60; j++)
            {
                cboFYYMM.Items.Add(ArgYY + "년 " + ComFunc.SetAutoZero(ArgMM.ToString(), 2) + "월분");
                cboTYYMM.Items.Add(ArgYY + "년 " + ComFunc.SetAutoZero(ArgMM.ToString(), 2) + "월분");

                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }
            cboFYYMM.SelectedIndex = 0;
            cboTYYMM.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            txtJepNo.Text = "";
            txtPano.Text = "";
            txtKiho.Text = "";

            cboGb.Items.Clear();
            cboGb.Items.Add("*.전체");
            cboGb.Items.Add("1.미처리");
            cboGb.Items.Add("2.처리완료");
            cboGb.SelectedIndex = 0;

            txtDate.Text = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT MIACODE, MIANAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_MIA ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND GBCHA ='*'";
                SQL = SQL + ComNum.VBLF + " ORDER BY DECODE(SUBSTR(MIANAME,1,2), '경북', 'ㄱ', MIANAME),  MIACODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboKiho.Items.Clear();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboKiho.Items.Add(dt.Rows[i]["MIACODE"].ToString().Trim() + "." + dt.Rows[i]["MIANAME"].ToString().Trim());
                }

                cboKiho.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                lblDate.Visible = false;
                txtDate.Visible = false;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            string strJiamt = "";
            string strROWID = "";

            if (e.Column == 11)
            {
                strROWID = ssView_Sheet1.Cells[e.Row, 14].Text;

                //TODO : 폼 호출
                //FrmGelSearch.Show 1
                frm = new frmPmpaViewGelSearch();

                frm.rGetData += frm_rGetData;
                frm.rEventClose += frm_rEventClose;
                frm.ShowDialog();

                ssView_Sheet1.Cells[e.Row, 11].Text = GstrMiaCode.Trim() + "." + GstrMiaName;
            }
            else if (e.Column == 9)
            {
                //strJiamt = VB.Val(ssView_Sheet1.Cells[e.Row, 9].Text).ToString("##########");
                //ssView_Sheet1.Cells[e.Row, 12].Text = strJiamt;

                //2020-07-30 프로그램 수정작업
                ssView_Sheet1.Cells[e.Row, 12].Text = VB.Replace(ssView_Sheet1.Cells[e.Row, 9].Text, ",", "");
                ssView_Sheet1.Rows[e.Row].ForeColor = Color.Blue;
            }
        }

        private void txtJepNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void frm_rGetData(string strMiaCode, string strMiaName)
        {
            GstrMiaCode = strMiaCode;
            GstrMiaName = strMiaName;

            frm.Dispose();
            frm = null;
        }

        private void frm_rEventClose()
        {
            if (frm != null)
            {
                frm.Dispose();
                frm = null;
            }
        }
    }
}
