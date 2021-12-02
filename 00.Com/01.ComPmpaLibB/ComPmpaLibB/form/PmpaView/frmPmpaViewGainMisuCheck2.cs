using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGainMisuCheck2.cs
    /// Description     : 월별 기타 미수금 점검표 (상세 내역)
    /// Author          : 박창욱
    /// Create Date     : 2017-08-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs43.frm(FrmGainMisuCheck2.frm) >> frmPmpaViewGainMisuCheck2.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGainMisuCheck2 : Form
    {
        string FstrYYMM = "";
        string FstrJob = "";
        string FstrIO = "";
        string GnJobSabun = clsType.User.JobMan;

        private string GstrHelpCode = "";

        public frmPmpaViewGainMisuCheck2()
        {
            InitializeComponent();
        }

        public frmPmpaViewGainMisuCheck2(string strHelpCode)
        {
            InitializeComponent();
            GstrHelpCode = strHelpCode;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            string strPano = "";
            string strSname = "";
            string strIpdOpd = "";
            long nSunapAmt = 0;
            long nMisuAmt = 0;
            long nChaAmt = 0;
            string strRemark = "";
            string strROWID = "";
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 2; i < ssView_Sheet1.RowCount; i++)
                {
                    strPano = ssView_Sheet1.Cells[i - 1, 0].Text;
                    strSname = ssView_Sheet1.Cells[i - 1, 1].Text;
                    strIpdOpd = ssView_Sheet1.Cells[i - 1, 2].Text;
                    nSunapAmt = Convert.ToInt32((VB.Val(VB.Replace(ssView_Sheet1.Cells[i - 1, 3].Text, ",", ""))));
                    nMisuAmt = Convert.ToInt32((VB.Val(VB.Replace(ssView_Sheet1.Cells[i - 1, 4].Text, ",", ""))));
                    nChaAmt = Convert.ToInt32((VB.Val(VB.Replace(ssView_Sheet1.Cells[i - 1, 5].Text, ",", ""))));
                    strRemark = ssView_Sheet1.Cells[i - 1, 6].Text;
                    strROWID = ssView_Sheet1.Cells[i - 1, 7].Text;

                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_ETC SET ";
                        SQL = SQL + ComNum.VBLF + " PANO = '" + strPano + "' , ";
                        SQL = SQL + ComNum.VBLF + " SNAME = '" + strSname + "', ";
                        SQL = SQL + ComNum.VBLF + " IPDOPD = '" + strIpdOpd + "', ";
                        SQL = SQL + ComNum.VBLF + " SUNAPAMT = '" + nSunapAmt + "' ,";
                        SQL = SQL + ComNum.VBLF + " MISUAMT = '" + nMisuAmt + "', ";
                        SQL = SQL + ComNum.VBLF + " CHAAMT = '" + nChaAmt + "' , ";
                        SQL = SQL + ComNum.VBLF + " REMARK = '" + strRemark + "' ,";
                        SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + " ENTSABUN = '" + GnJobSabun + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_ETC(YYMM , Pano, SName, JONG, IPDOPD, SUNAPAMT, MISUAMT, CHAAMT, REMARK, ENTDATE, ENTSABUN ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES ( '" + FstrYYMM + "',  '" + strPano + "', '" + strSname + "', '" + FstrJob + "',  ";
                        SQL = SQL + ComNum.VBLF + "          '" + strIpdOpd + "','" + nSunapAmt + "', '" + nMisuAmt + "', '" + nChaAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "          '" + strRemark + "', SYSDATE, '" + GnJobSabun + "') ";  
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }

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

        private void frmPmpaViewGainMisuCheck2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView.Dock = DockStyle.Fill;

            lblInfo.Text = GstrHelpCode;

            FstrYYMM = VB.Pstr(GstrHelpCode, "/", 1);
            FstrJob = VB.Pstr(GstrHelpCode, "/", 2);
            FstrIO = VB.Pstr(GstrHelpCode, "/", 4);

            lblInfo.Text = "작업년월: " + FstrYYMM + " " + "구분: " + VB.Pstr(GstrHelpCode, "/", 3);
            View_Data();
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            View_Data();
        }

        void View_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            int i = 0;
            int nRead = 0;
            int nRow = 0;
            string strPano = "";
            string strSname = "";
            string strIO = "";

            string strOldData = "";
            double nAmt1 = 0;
            double nAmt2 = 0;

            double nTotAmt1 = 0;
            double nTotAmt2 = 0;

            string strFDate = "";
            string strTDate = "";

            strFDate = VB.Left(FstrYYMM, 4) + "-" + VB.Right(FstrYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(FstrYYMM, 4)), (int)VB.Val(VB.Right(FstrYYMM, 2)));

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrJob == "1")     //개인미수
                {
                    SQL = "";
                    SQL = " CREATE OR REPLACE VIEW VIEW_MISU_ETC_COM (GBN, PANO, IPDOPD, AMT)  AS      ";
                    SQL = SQL + ComNum.VBLF + " SELECT '1', PANO, DECODE( SUBSTR(MISUDTL,1,1) ,'O', 'O','I'), SUM(AMT)  ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND GUBUN1 ='1'    ";
                    if (FstrIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  SUBSTR(MISUDTL,1,1) = 'O'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND SUBSTR(MISUDTL,1,1) = 'I' ";
                    }
                    SQL = SQL + ComNum.VBLF + " GROUP BY PANO, DECODE( SUBSTR(MISUDTL,1,1) ,'O', 'O','I') ";
                    SQL = SQL + ComNum.VBLF + " UNION   ";

                    if (FstrIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'O'IPDOPD,  SUM(AMT1) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND SUCODE IN ('Y96','Y96B')";
                        SQL = SQL + ComNum.VBLF + "    AND BUN ='96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                        SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'O'IPDOPD,  SUM(AMT) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96')";    //기타수납 개인미수
                        SQL = SQL + ComNum.VBLF + "    AND BUN = '96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'I' IPDOPD, SUM(AMT) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96','Y96B')";
                        SQL = SQL + ComNum.VBLF + "    AND BUN ='96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                    }
                }
                else
                {
                    SQL = "";
                    SQL = " CREATE OR REPLACE VIEW VIEW_MISU_ETC_COM (GBN, PANO, IPDOPD, AMT)  AS      ";
                    SQL = SQL + ComNum.VBLF + " SELECT '1', MISUID,  IPDOPD, SUM(AMT2)  ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND MIRYYMM = '" + FstrYYMM + "' ";

                    switch (FstrJob)
                    {
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '11' ";
                            break; //보훈청미수
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '08' ";
                            break; //계약처
                        case "4":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '13' ";
                            break; //심신장애
                        case "5":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '09' ";
                            break; //헌혈미수
                        case "6":
                            SQL = SQL + ComNum.VBLF + " AND CLASS IN ( '12','14','15')  ";
                            break; //기타미수
                        case "7":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '16' ";
                            break; //요양소견미수
                        case "8":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '17' ";
                            break; //가정간호소견서
                        case "9":
                            SQL = SQL + ComNum.VBLF + " AND CLASS = '18' ";
                            break; //치매검사
                    }


                    if (FstrIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  IPDOPD = 'O'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND  IPDOPD = 'I'";
                    }
                    SQL = SQL + ComNum.VBLF + " GROUP BY MISUID, IPDOPD ";
                    SQL = SQL + ComNum.VBLF + " UNION   ";
                    if (FstrIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'O'IPDOPD,  SUM(AMT1) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                        switch (FstrJob)
                        {
                            case "2":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96H')";
                                break;        //보휸청 미수
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96J','Y96M')";
                                break;      //계약처
                            case "4":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96K')";
                                break;        //심신장애
                            case "5":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96S')";
                                break;        //헌혈미수
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96I')";
                                break;        //기타미수
                            case "7":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96C')";
                                break;        //요양소견미수
                            case "8":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96F')";
                                break;        //가정간호소견서
                            case "9":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96Y')";
                                break;        //치매검사
                        }

                        SQL = SQL + ComNum.VBLF + "    AND BUN ='96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                        SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'O'IPDOPD,  SUM(AMT) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";

                        switch (FstrJob)
                        {
                            case "2":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96H')";
                                break;        //보훈청 미수
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96J','Y96M')";
                                break; //계약처
                            case "4":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96K')";
                                break;        //심신장애
                            case "5":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96S')";
                                break;        //헌혈미수
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96I')";
                                break;        //기타미수
                            case "7":
                                if (Convert.ToDateTime(strFDate) >= Convert.ToDateTime("2016-12-01"))
                                {
                                    SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96A','Y96C')";        //요양소견미수
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96C')";        //요양소견미수
                                }
                                break;
                            case "8":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96F')";
                                break;        //가정간호소견서
                            case "9":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96Y')";
                                break;        //치매검사
                        }

                        SQL = SQL + ComNum.VBLF + "    AND BUN ='96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " SELECT '2', PANO, 'I' IPDOPD, SUM(AMT) ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";

                        switch (FstrJob)
                        {
                            case "2":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96H')";
                                break;        //보휸청 미수
                            case "3":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96J','Y96M')";
                                break;       //계약처
                            case "4":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96K')";
                                break;        //심신장애
                            case "5":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96S')";
                                break;        //헌혈미수
                            case "6":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96I')";
                                break;        //기타미수
                            case "7":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96C')";
                                break;        //요양소견미수
                            case "8":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96F')";
                                break;        //가정간호소견서
                            case "9":
                                SQL = SQL + ComNum.VBLF + "    AND SUNEXT IN ('Y96Y')";
                                break;        //치매검사
                        }
                        SQL = SQL + ComNum.VBLF + "    AND BUN ='96'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO ";
                    }
                }

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
                SQL = " SELECT A.GBN, A.PANO, B.SNAME ,A.IPDOPD, A.AMT ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_MISU_ETC_COM A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "    AND A.AMT <> 0";
                SQL = SQL + ComNum.VBLF + " ORDER BY 2,1 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (chkGb.Checked == true)
                {
                    nRow = nRow + 1;
                }
                else
                {
                    nRow = 0;
                }
                
                strOldData = "";
                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;

                for (i = 0; i < nRead; i++)
                {
                    if (strOldData != dt.Rows[i]["Pano"].ToString().Trim() + dt.Rows[i]["IPDOPD"].ToString().Trim())
                    {
                        Display_SUB(nAmt1, nAmt2, ref nRow, strPano, strSname, strIO, ref nTotAmt1, ref nTotAmt2);
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        strIO = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        strSname = dt.Rows[i]["SName"].ToString().Trim();
                        strOldData = strPano + strIO;
                        nAmt1 = 0;
                        nAmt2 = 0;
                    }
                    if (dt.Rows[i]["GBN"].ToString().Trim() == "2")     //미수발생액
                    {
                        nAmt1 += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }
                    else
                    {
                        nAmt2 += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }
                }
                Display_SUB(nAmt1, nAmt2, ref nRow, strPano, strSname, strIO, ref nTotAmt1, ref nTotAmt2);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
               
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[0, 3].Text = nTotAmt1.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[0, 4].Text = nTotAmt2.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[0, 5].Text = (nTotAmt2 - nTotAmt1).ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[0, 0].Text = " ** 합 계 **";

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

        void Display_SUB(double nAmt1, double nAmt2, ref int nRow, string strPano, string strSname, string strIO, ref double nTotAmt1, ref double nTotAmt2)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (chkGb.Checked == true)
                {
                    if (nAmt2 - nAmt1 != 0)
                    {
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = strSname;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = strIO;
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = nAmt1.ToString("###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = nAmt2.ToString("###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = (nAmt2 - nAmt1).ToString("###,###,##0 ");

                        nTotAmt1 += nAmt1;
                        nTotAmt2 += nAmt2;

                        //사유 READ
                        SQL = "";
                        SQL = " SELECT REMARK, ROWID ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_ETC ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM = '" + FstrYYMM + "'";
                        SQL = SQL + ComNum.VBLF + "   AND JONG = '" + FstrJob + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND IPDOPD = '" + strIO + "'";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[0]["remark"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }
                else
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strSname;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = strIO;
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = nAmt1.ToString("###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = nAmt2.ToString("###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = (nAmt2 - nAmt1).ToString("###,###,##0 ");

                    nTotAmt1 += nAmt1;
                    nTotAmt2 += nAmt2;

                    //사유 READ
                    SQL = "";
                    SQL = " SELECT REMARK, ROWID ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_ETC ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND YYMM = '" + FstrYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "   AND JONG = '" + FstrJob + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND IPDOPD = '" + strIO + "'";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[0]["remark"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssPrint_Sheet1.RowCount = 6;

            string strFont1 = "";
            string strHead1 = "";

            //Print Head
            strFont1 = "/c/fn\"굴림체\" /fz\"22\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/c/f1" + "월별 기타 미수금 청구액 점검표" + "/f1/n";

            ssPrint_Sheet1.Cells[3, 0].Text = lblInfo.Text;
            ssPrint_Sheet1.Cells[4, 0].Text = "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].ColumnSpan = 2;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].ColumnSpan = 5;
            ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);

            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = "등록번호";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "성명";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = "I/O";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = "미수액";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].BackColor = System.Drawing.Color.FromArgb(218, 217, 255);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = "발생미수";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].BackColor = System.Drawing.Color.FromArgb(218, 217, 255);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = "차액";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].BackColor = System.Drawing.Color.FromArgb(255, 216, 216);
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = "사유";
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.FromArgb(250, 244, 192);
            ssPrint_Sheet1.Rows[ssPrint_Sheet1.RowCount - 1].Height = 25;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].ColumnSpan = 5;
                ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].BackColor = System.Drawing.Color.FromArgb(228, 247, 186);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].BackColor = System.Drawing.Color.FromArgb(218, 217, 255);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].BackColor = System.Drawing.Color.FromArgb(218, 217, 255);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].BackColor = System.Drawing.Color.FromArgb(255, 216, 216);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.FromArgb(250, 244, 192);
            }

            //Print Body
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssPrint_Sheet1.PrintInfo.Margin.Left = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Right = 0;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 60;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowColor = true;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = true;
            ssPrint_Sheet1.PrintInfo.UseMax = false;
            if (chkPrintW.Checked == false)
            {
                ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            }
            else
            {
                ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;   //2010-11-07
            }
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint.PrintSheet(0);
        }


    }
}
