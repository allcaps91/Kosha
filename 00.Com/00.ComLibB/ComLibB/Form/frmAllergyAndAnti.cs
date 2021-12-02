using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public interface IAllergyInterface
    {
        void setAllergyData(string strRmk, string[] strDamKor, string[] strDamEng, string[] strDamCd, string[] strDamTypeNm, string[] strDamTypeCd);
    }

    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAllergyAndAnti.cs
    /// Description     : 알러지 등록 및 항생반응 등록
    /// Author          : 박창욱
    /// Create Date     : 2017-07-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\ojumst\Frm외래정보입력.frm(Frm외래정보입력.frm) >> frmAllergyAndAnti.cs 폼이름 재정의" />	
    public partial class frmAllergyAndAnti : Form, IAllergyInterface
    {
        private string GstrPtNo = "";
        private string GstrDate = "";

        public frmAllergyAndAnti()
        {
            InitializeComponent();
        }

        public frmAllergyAndAnti(string strPtNo, string strDate)
        {
            InitializeComponent();
            GstrPtNo = strPtNo;
            GstrDate = strDate;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DelData() == true)
            {
                Read_AST();
            }
        }

        private bool DelData()
        {
            int i = 0;
            string strROWID = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;

            if (lblSabun.Text == "")
            {
                ComFunc.MsgBox("등록자 사번을 입력 후 작업해주세요.");
                txtSabun.Focus();
                return rtnVal;
            }


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 5].Text;

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true && strROWID != "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_AST_HISTORY";
                        SQL = SQL + ComNum.VBLF + "     (SDATE, EDATE, PTNO, INDATE, GBN, SABUN, DELSABUN, ORDERCODE, JOBNAME) ";
                        SQL = SQL + ComNum.VBLF + "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SDATE, EDATE, PTNO, INDATE, GBN, SABUN, '" + txtSabun.Text + "', ORDERCODE, JOBNAME ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_AST ";
                        SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        SQL = "";
                        SQL = " DELETE " + ComNum.DB_PMPA + "NUR_AST ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                this.Close();
            }
        }

        private bool SaveData()
        {
            string strPtNo = "";
            string strSname = "";
            string strInDate = "";
            string strORDERCODE = "";
            string strGbn = "";
            string strJOBNAME = "";
            //string strChange = "";
            //string strCheck = "";
            string strRemark = "";
            string strGBIOE = "";
            string strBDATE = string.Empty;

            bool rtnVal = false;

            int i = 0;
            string strROWID = "";
            DataTable dt = null;

            if (lblSabun.Text == "")
            {
                ComFunc.MsgBox("등록자 사번을 입력 후 작업해주세요.");
                txtSabun.Focus();
                return rtnVal;
            }

            strPtNo = txtPano.Text.Trim();
            strSname = lblPano.Text;
            strInDate = dtpDate.Value.ToString("yyyy-MM-dd");

            if (strPtNo.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 입력 후 작업해주세요.");
                txtSabun.Focus();
                return rtnVal;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            //삭제여부 확인
            int nDelCnt = 0;
            for (int index = 0; index < ssAllergy.ActiveSheet.Rows.Count; index++)
            {
                if (ssAllergy.ActiveSheet.Cells[index, 0].Text == "True")
                {
                    nDelCnt++;
                }
            }

            if (nDelCnt > 0)
            {
                if (MessageBox.Show("삭제 체크 건수가 " + nDelCnt + "건 입니다. 삭제하시겠습니까?","삭제 확인",MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return rtnVal;
                }                
            }



            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //알러지 등록
                if (setAllergy() == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return rtnVal;
                }

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strGbn = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strORDERCODE = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strJOBNAME = lblSabun.Text.Trim();
                    strGBIOE = "00";
                    strROWID = ssView_Sheet1.Cells[i, 5].Text.Trim();

                    strBDATE = ssView_Sheet1.Cells[i, 9].Text.Trim();


                    if (ssView_Sheet1.Cells[i, 6].Text.Trim() == "Y")
                    {
                        if (strJOBNAME == "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("등록자명을 반드시 입력해야 합니다.");
                            return rtnVal;
                        }

                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_AST";
                            SQL = SQL + ComNum.VBLF + "     SET ";
                            SQL = SQL + ComNum.VBLF + "         GBN     = '" + strGbn + "', ";
                            SQL = SQL + ComNum.VBLF + "         SDATE   = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ,";
                            SQL = SQL + ComNum.VBLF + "         JOBNAME = '" + strJOBNAME + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }
                        else if (strROWID == "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_AST";
                            SQL = SQL + ComNum.VBLF + "     (SDATE, PTNO, INDATE, GBN, SABUN, ORDERCODE, JOBNAME, GBIOE)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE)   ,";
                            SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "',";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strInDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + "         '" + strGbn + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + txtSabun.Text + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + strJOBNAME + "',";
                            SQL = SQL + ComNum.VBLF + "         '" + strGBIOE + "'";
                            SQL = SQL + ComNum.VBLF + "     ) ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (strGbn == "Positive")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     REMARK, ROWID ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE UPPER(REMARK) LIKE '%" + strORDERCODE.ToUpper() + "%' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPtNo + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }
                            if (dt.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = " INSERT INTO " + ComNum.DB_PMPA + "ETC_ALLERGY_MST";
                                SQL = SQL + ComNum.VBLF + "     (PANO, SNAME, CODE, ENTDATE, REMARK)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + strPtNo + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + strSname + "', ";
                                SQL = SQL + ComNum.VBLF + "         '002',";
                                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "'";
                                SQL = SQL + ComNum.VBLF + "     )";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                            else
                            {
                                strRemark = dt.Rows[0]["REMARK"].ToString().Trim();

                                if (strRemark.ToUpper().IndexOf(strORDERCODE.ToUpper()) == -1)
                                {
                                    if (strRemark != "")
                                    {
                                        strRemark += "," + strORDERCODE;
                                    }
                                    else
                                    {
                                        strRemark = strORDERCODE;
                                    }

                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_ALLERGY_MST";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         Remark = '" + strRemark + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox(SqlErr);
                                        Cursor.Current = Cursors.Default;
                                        return rtnVal;
                                    }
                                }
                            }
                            dt.Dispose();
                            dt = null;
                        }
                        else
                        {
                            //2019-06-04 Negative 일때 삭제추가
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                            SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN) ";
                            SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text;
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST";
                            SQL = SQL + ComNum.VBLF + "     WHERE UPPER(REMARK) LIKE '%" + strORDERCODE.ToUpper() + "%' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPtNo + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }


                            SQL = "";                                                       
                            SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE UPPER(REMARK) LIKE '%" + strORDERCODE.ToUpper() + "%' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPtNo + "' "; 

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("정상 처리되었습니다.");
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

        void Read_AST()
        {
            int i = 0;
            int nREAD = 0;
            string strPtno = "";
            string strInDate = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strPtno = txtPano.Text.Trim();
            strInDate = dtpDate.Value.ToString("yyyy-MM-dd");

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.SUCODE, B.GBN, B.SDATE, B.ROWID, B.JOBNAME, B.GBIOE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_oORDER A, KOSMOS_PMPA.NUR_AST B";
                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = B.PTNO(+)";
                SQL = SQL + ComNum.VBLF + " AND A.SUCODE = B.ORDERCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.BDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + " AND (A.SUCODE LIKE 'W-%' OR A.SUCODE IN ('KT-TIAM1','KT-AXON2','KT-AXON1'))";
                SQL = SQL + ComNum.VBLF + " AND A.BUN IN ('12','20') ";
                SQL = SQL + ComNum.VBLF + " AND A.PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY A.BDATE, A.SUCODE, B.GBN, B.SDATE,  B.ROWID, B.JOBNAME, B.GBIOE";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(B.SDATE, 'YYYY-MM-DD') AS BDATE, A.SUCODE, B.GBN, B.SDATE, B.ROWID, B.JOBNAME, B.GBIOE    ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.NUR_AST B    ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = B.PTNO(+)    ";
                SQL = SQL + ComNum.VBLF + "     AND A.SUCODE = B.ORDERCODE(+)    ";
                SQL = SQL + ComNum.VBLF + "     AND A.BDATE >= TO_DATE('" + strInDate + "', 'YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "     AND(A.BDATE <= TRUNC(SYSDATE) OR A.BDATE <= B.EDATE)    ";
                SQL = SQL + ComNum.VBLF + "     AND B.INDATE(+) <= TO_DATE('" + strInDate + "', 'YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "     AND B.INDATE(+) >= TO_DATE('" + strInDate + "', 'YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "     AND(A.SUCODE LIKE 'W-%' OR A.SUCODE IN('KT-TIAM1', 'KT-AXON2', 'KT-AXON1'))    ";
                SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('12', '20')     ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + strPtno + "'    ";
                SQL = SQL + ComNum.VBLF + "GROUP BY B.SDATE, A.SUCODE, B.GBN, B.SDATE,  B.ROWID, B.JOBNAME, B.GBIOE, A.PTNO    ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssView_Sheet1.RowCount = nREAD;
                    for (i = 0; i < nREAD; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = READ_DRUGNAME(dt.Rows[i]["SUCODE"].ToString().Trim()); 
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GBN"].ToString().Trim();

                        if (dt.Rows[i]["SDATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 4].Text = Convert.ToDateTime(dt.Rows[i]["SDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["JOBNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GBIOE"].ToString().Trim();
                        if (ssView_Sheet1.Cells[i, 8].Text.Trim() == "")
                        {
                            ssView_Sheet1.Cells[i, 8].Text = clsType.User.JobName;
                        }
                        if (dt.Rows[i]["GBIOE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 230);
                        }
                        else
                        {
                            ssView_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 0);
                        }

                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    }
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

        void READ_ALLERGY()
        {
            int i = 0;
            int j = 0;
            string strCODE = "";
            string strRemark = "";
            string strPano = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strPano = txtPano.Text.Trim();

            chkAllergy00.Text = "";
            chkAllergy01.Text = "";
            chkAllergy02.Text = "";

            try
            {
                //알러지 Setting
                SQL = "";
                SQL = " SELECT Code || '.' || Name CodeName";
                SQL = SQL + ComNum.VBLF + " FROM  " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '환자정보_알러지종류' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < 7; i++)
                    {
                        switch (VB.Left(dt.Rows[i]["CodeName"].ToString().Trim(), 3))
                        {
                            case "001":
                                chkAllergy00.Text = dt.Rows[i]["CodeName"].ToString().Trim();
                                break;
                            case "002":
                                chkAllergy01.Text = dt.Rows[i]["CodeName"].ToString().Trim();
                                break;
                            case "003":
                                chkAllergy02.Text = dt.Rows[i]["CodeName"].ToString().Trim();
                                break;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                chkAllergy00.Checked = false;
                chkAllergy01.Checked = false;
                chkAllergy02.Checked = false;
                txtAllergy00.Text = "";
                txtAllergy01.Text = "";
                txtAllergy02.Text = "";
                txtAllergySabun00.Text = "";
                txtAllergySabun01.Text = "";
                txtAllergySabun02.Text = "";
                dtpAllergy00.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                dtpAllergy01.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                dtpAllergy02.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

                SQL = "";
                SQL = " SELECT Code, Remark, SABUN, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_PMPA.ETC_ALLERGY_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Code ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < 3; i++)
                    {
                        strCODE = "";
                        strRemark = "";

                        if (dt.Rows.Count > i)
                        {
                            strCODE = dt.Rows[i]["CODE"].ToString().Trim();
                            strRemark = dt.Rows[i]["REMARK"].ToString().Trim();
                        }

                        for (j = 0; j < 3; j++)
                        {
                            if (strCODE == VB.Pstr(chkAllergy00.Text.Trim(), ".", 1))
                            {
                                chkAllergy00.Checked = true;
                                txtAllergy00.Text = strRemark;
                                txtAllergySabun00.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                                dtpAllergy00.Value = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim());
                            }
                            else if (strCODE == VB.Pstr(chkAllergy01.Text.Trim(), ".", 1))
                            {
                                chkAllergy01.Checked = true;
                                txtAllergy01.Text = strRemark;
                                txtAllergySabun01.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                                dtpAllergy01.Value = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim());
                            }
                            else if (strCODE == VB.Pstr(chkAllergy02.Text.Trim(), ".", 1))
                            {
                                chkAllergy02.Checked = true;
                                txtAllergy02.Text = strRemark;
                                txtAllergySabun02.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                                dtpAllergy02.Value = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim());
                            }
                        }
                    }

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {

                        if (dt.Rows[k]["CODE"].ToString().Trim().Equals("004"))
                        {
                            this.txtDRUG.Text = dt.Rows[k]["REMARK"].ToString().Trim();
                        }
                        else if (dt.Rows[k]["CODE"].ToString().Trim().Equals("005"))
                        {
                            this.txtORDER.Text = dt.Rows[k]["REMARK"].ToString().Trim();
                        }
                        //2017-09-06 박창욱 : 요구사항 반영
                        //ADR입력정보를 Display
                        else if (dt.Rows[k]["CODE"].ToString().Trim().Equals("006"))
                        {
                            this.txtADR.Text = dt.Rows[k]["REMARK"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;


                READ_ALLERGYOLD();  //CODE 001, 002, 006
                READ_ALLERGYNEW();  //CODE 100

                //기존 패널창 데이터값 없으면 안보여줌 2019-07 이전 등록 내역 패널
                if (ssViewOld_Sheet1.NonEmptyRowCount == 0)
                {
                    panOld.Visible = false;
                }
                else
                {
                    panOld.Visible = true;
                }

                if (ssViewOld2_Sheet1.NonEmptyRowCount == 0)
                {
                    panOld2.Visible = false;
                }
                else
                {
                    panOld2.Visible = true;
                }


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// CODE 001, 002, 006 스프레드로
        /// </summary>
        private void READ_ALLERGYOLD()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //알러지 Setting
            SQL = "";
            SQL = " SELECT A.GUBUN, A.CODE, A.NAME, TO_CHAR(B.ENTDATE) ENTDATE, B.REMARK, B.SABUN ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE A, KOSMOS_PMPA.ETC_ALLERGY_MST B ";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '환자정보_알러지종류' ";
            SQL = SQL + ComNum.VBLF + "  AND a.CODE = b.CODE ";
            SQL = SQL + ComNum.VBLF + "  AND b.CODE IN ('001','002','006') ";
            SQL = SQL + ComNum.VBLF + "  AND b.PANO = '" + txtPano.Text + "' ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssViewOld_Sheet1.Rows.Count = 0;
            ssViewOld_Sheet1.Rows.Count = dt.Rows.Count;
            ssViewOld_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                ssViewOld_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString();
                ssViewOld_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString();
                ssViewOld_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                ssViewOld_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString();
            }

            for (int i = 0; i < ssViewOld_Sheet1.Columns.Count; i++)
            {
                ssViewOld_Sheet1.Columns[i].Width = ssViewOld_Sheet1.Columns[i].GetPreferredWidth() + 1;
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = " SELECT A.GUBUN, A.CODE, A.NAME, TO_CHAR(B.ENTDATE) ENTDATE, B.REMARK, B.DELSABUN ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE A, KOSMOS_PMPA.ETC_ALLERGY_MST_HIS B ";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '환자정보_알러지종류' ";
            SQL = SQL + ComNum.VBLF + "  AND a.CODE = b.CODE ";
            SQL = SQL + ComNum.VBLF + "  AND b.CODE IN ('001','002','006') ";
            SQL = SQL + ComNum.VBLF + "  AND b.PANO = '" + txtPano.Text + "' ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY CODE ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssViewOld2_Sheet1.Rows.Count = 0;
            ssViewOld2_Sheet1.Rows.Count = dt.Rows.Count;
            ssViewOld2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssViewOld2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["NAME"].ToString();
                ssViewOld2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString();
                ssViewOld2_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DELSABUN"].ToString().Trim());
                ssViewOld2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ENTDATE"].ToString();
            }

            for (int i = 0; i < ssViewOld2_Sheet1.Columns.Count; i++)
            {
                ssViewOld2_Sheet1.Columns[i].Width = ssViewOld2_Sheet1.Columns[i].GetPreferredWidth() + 1;
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 알러지(NEW) 조회 함수
        /// </summary>
        private void READ_ALLERGYNEW()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT PANO, SNAME, CODE, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI:SS') ENTDATE, REMARK, SABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR,  '' WRITEDATE, NULL DELSABUN, '' SAYU";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_ALLERGY_MST";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text + "' ";
            SQL = SQL + ComNum.VBLF + "AND CODE = '100' ";
            SQL = SQL + ComNum.VBLF + "UNION ALL ";
            SQL = SQL + ComNum.VBLF + "SELECT PANO, SNAME, CODE, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI:SS') ENTDATE, REMARK, SABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR, TO_CHAR(WRITEDATE, 'YYYY-MM-DD HH24:MI:SS') WRITEDATE, DELSABUN, SAYU ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_ALLERGY_MST_HIS";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + txtPano.Text + "' ";
            SQL = SQL + ComNum.VBLF + "AND CODE = '100' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY WRITEDATE DESC, ENTDATE DESC  ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            ssAllergy_Sheet1.Rows.Count = 0;
            ssAllergy_Sheet1.Rows.Count = dt.Rows.Count;
            ssAllergy_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            Font f = new Font("맑은 고딕", 10, FontStyle.Strikeout);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssAllergy_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DAMDESC"].ToString();
                ssAllergy_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DAMDESCKR"].ToString();
                ssAllergy_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DAMTYPENM"].ToString();
                ssAllergy_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RMK"].ToString();
                ssAllergy_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ENTDATE"].ToString();
                ssAllergy_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SABUN"].ToString();
                ssAllergy_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DAMTYPE"].ToString();
                ssAllergy_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DAMCD"].ToString();
                ssAllergy_Sheet1.Cells[i, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                ssAllergy_Sheet1.Cells[i, 10].Text = dt.Rows[i]["WRITEDATE"].ToString();
                ssAllergy_Sheet1.Cells[i, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DELSABUN"].ToString().Trim());
                ssAllergy_Sheet1.Cells[i, 12].Text = dt.Rows[i]["DELSABUN"].ToString();
                ssAllergy_Sheet1.Cells[i, 13].Text = dt.Rows[i]["SAYU"].ToString();

                //삭제된 히스토리내역은 정보 수정 불가능
                if (ssAllergy_Sheet1.Cells[i, 10].Text != "")
                {
                    ssAllergy_Sheet1.Rows[i].Locked = true;
                    ssAllergy_Sheet1.Rows[i].Font = f;      //취소선
                    //ssAllergy_Sheet1.Cells[i, 0].Locked = true;
                }
            }

            dt.Dispose();
            dt = null;

            for (int i = 0; i < ssAllergy_Sheet1.Columns.Count; i++)
            {
                ssAllergy_Sheet1.Columns[i].Width = ssAllergy_Sheet1.GetPreferredColumnWidth(i, true, true, true, true);
            }
            //증상, 변경사유는 고정
            ssAllergy_Sheet1.Columns[4].Width = 150;
            ssAllergy_Sheet1.Columns[13].Width = 150;
        }

        bool setAllergy()
        {
            int i = 0;
            string strROWID5 = "";
            string strRemark = "";
            string strChange = "";
            string strPano = "";
            string strSname = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strPano = txtPano.Text.Trim();
            strSname = lblPano.Text.Trim();

            CheckBox[] chk = new CheckBox[3];
            TextBox[] txt1 = new TextBox[3];
            TextBox[] txt2 = new TextBox[3];
            DateTimePicker[] dtp = new DateTimePicker[3];

            chk[0] = chkAllergy00;
            chk[1] = chkAllergy01;
            chk[2] = chkAllergy02;

            txt1[0] = txtAllergy00;
            txt1[1] = txtAllergy01;
            txt1[2] = txtAllergy02;

            txt2[0] = txtAllergySabun00;
            txt2[1] = txtAllergySabun01;
            txt2[2] = txtAllergySabun02;

            dtp[0] = dtpAllergy00;
            dtp[1] = dtpAllergy01;
            dtp[2] = dtpAllergy02;
            //Control[] controls = ComFunc.GetAllControls(this);


            //foreach (Control ctl in controls)
            //{
            //    if (ctl is TextBox)
            //    {
            //        if (VB.Left(((TextBox)ctl).Name, 15) == "txtAllergySabun")
            //        {
            //            Array.Resize<TextBox>(ref txt2, txt2.Length + 1);
            //            txt2[txt2.Length - 1] = (TextBox)ctl;
            //        }
            //        else if (VB.Left(((TextBox)ctl).Name, 10) == "txtAllergy")
            //        {
            //            Array.Resize<TextBox>(ref txt1, txt1.Length + 1);
            //            txt1[txt1.Length - 1] = (TextBox)ctl;
            //        }
            //    }
            //    else if (ctl is CheckBox)
            //    {
            //        if (VB.Left(((CheckBox)ctl).Name, 10) == "chkAllergy")
            //        {
            //            Array.Resize<CheckBox>(ref chk, chk.Length + 1);
            //            chk[chk.Length - 1] = (CheckBox)ctl;
            //        }
            //    }
            //    else if (ctl is DateTimePicker)
            //    {
            //        if (VB.Left(((DateTimePicker)ctl).Name, 10) == "dtpAllergy")
            //        {
            //            Array.Resize<DateTimePicker>(ref dtp, dtp.Length + 1);
            //            dtp[dtp.Length - 1] = (DateTimePicker)ctl;
            //        }
            //    }
            //}

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (i = 0; i < 3; i++)
                {
                    strRemark = txt1[i].Text.Trim();
                    SQL = "";
                    SQL = " SELECT ROWID, REMARK FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND Code ='" + VB.Pstr(chk[i].Text, ".", 1).Trim() + "' ";
                    strROWID5 = "";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strROWID5 = dt.Rows[0]["ROWID"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;

                    strChange = "";
                    if (chk[i].Checked == true)
                    {
                        SQL = "";
                        SQL = " SELECT ROWID FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND Code ='" + VB.Pstr(chk[i].Text, ".", 1).Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND REMARK = '" + strRemark + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return false;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            strChange = "Y";
                        }
                        dt.Dispose();
                        dt = null;

                        if (strROWID5 != "" && strChange == "Y")
                        {
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                            SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR) ";
                            SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text + ", DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "', ";
                            SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + "         SABUN = " + txtSabun.Text;
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + strROWID5 + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else if (strROWID5 == "")
                        {
                            //추가
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, REMARK, ";
                            SQL = SQL + ComNum.VBLF + " ENTDATE, SABUN) VALUES (  ";
                            SQL = SQL + ComNum.VBLF + " '" + strPano + "', '" + strSname + "', ";
                            SQL = SQL + ComNum.VBLF + " '" + VB.Pstr(chk[i].Text, ".", 1).Trim() + "','" + txt1[i].Text + "', ";
                            SQL = SQL + ComNum.VBLF + " SYSDATE," + txtSabun.Text + " ) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                    else if (strROWID5 != "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                        SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR) ";
                        SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text + ", DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        SQL = "";
                        SQL = "  DELETE FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID5 + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                }


                //알러지 새로운것 등록
                for (int j = 0; j < ssAllergy_Sheet1.NonEmptyRowCount; j++)
                {
                    //삭제
                    if (ssAllergy_Sheet1.Cells[j, 0].Text == "True")
                    {
                        //삭제 체크 해놓고 사유 안쓰면 메세지창 띄움
                        if (string.IsNullOrEmpty(ssAllergy_Sheet1.Cells[j, 13].Text))
                        {
                            MessageBox.Show("입력된 코드를 삭제하려면 변경사유(마지막열)에 입력하셔야 합니다.");
                            return false;
                        }

                        SQL = "";
                        SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                        SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR, SAYU) ";
                        SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text + ", DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR, '" + ssAllergy_Sheet1.Cells[j, 13].Text + "'";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + " AND CODE = '100' ";
                        SQL = SQL + ComNum.VBLF + " AND DAMCD = '" + ssAllergy_Sheet1.Cells[j, 8].Text + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        SQL = "";
                        SQL = " DELETE FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + " AND CODE = '100' ";
                        SQL = SQL + ComNum.VBLF + " AND DAMCD = '" + ssAllergy_Sheet1.Cells[j, 8].Text + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }
                    //추가
                    else
                    {
                        //히스토리 내역은 체크 및 프로세스 스킵
                        if (ssAllergy_Sheet1.Rows[j].Locked == true)
                        {
                            continue;
                        }

                        strRemark = ssAllergy_Sheet1.Cells[j, 4].Text;
                        if (string.IsNullOrEmpty(strRemark))
                        {
                            MessageBox.Show("등록하려는 약품 중 증상을 확인 입력 후 다시 저장해주십시오.");
                            return false;
                        }

                        SQL = "";
                        SQL = " SELECT ROWID, REMARK FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                        SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND Code ='100' ";
                        SQL = SQL + ComNum.VBLF + "  AND DAMCD =  '" + ssAllergy_Sheet1.Cells[j, 8].Text + "'";
                        strROWID5 = "";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return false;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strROWID5 = dt.Rows[0]["ROWID"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                        strChange = "";
                        if (ssAllergy_Sheet1.Cells[j, 0].Text != "True")
                        {
                            SQL = "";
                            SQL = " SELECT ROWID FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Code ='100' ";
                            SQL = SQL + ComNum.VBLF + "  AND RMK = '" + strRemark + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return false;
                            }
                            if (dt.Rows.Count == 0)
                            {
                                strChange = "Y";
                            }
                            dt.Dispose();
                            dt = null;

                            if (strROWID5 != "" && strChange == "Y")
                            {
                                //증상 수정시 사유 안쓰면 메세지창 띄움
                                if (string.IsNullOrEmpty(ssAllergy_Sheet1.Cells[j, 13].Text))
                                {
                                    MessageBox.Show("기존 코드의 증상을 수정하려면 변경사유를 입력하여주십시오.(마지막열)");
                                    return false;
                                }


                                SQL = "";
                                SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                                SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR, SAYU) ";
                                SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text + ", DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR, '" + ssAllergy_Sheet1.Cells[j, 13].Text + "'";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }

                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_ALLERGY_MST ";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                //SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "', ";
                                SQL = SQL + ComNum.VBLF + "         RMK = '" + strRemark + "', ";
                                SQL = SQL + ComNum.VBLF + "         ENTDATE = SYSDATE, ";
                                SQL = SQL + ComNum.VBLF + "         SABUN = " + txtSabun.Text;
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + strROWID5 + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            else if (strROWID5 == "")
                            {
                                //추가
                                SQL = "";
                                SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST ";
                                SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE,  ";
                                SQL = SQL + ComNum.VBLF + " ENTDATE, SABUN,DAMCD,DAMTYPE,RMK,REMARK,DAMTYPENM,DAMDESC,DAMDESCKR) ";
                                SQL = SQL + ComNum.VBLF + "     VALUES('" + txtPano.Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '" + lblPano.Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '100',";
                                SQL = SQL + ComNum.VBLF + "     SYSDATE,";
                                SQL = SQL + ComNum.VBLF + "     '" + txtSabun.Text + "',";                                
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 8].Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 7].Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 4].Text + "',";
                                if (ssAllergy_Sheet1.Cells[j,1].Text == "계열 모름")
                                {
                                    SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 4].Text + "',";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 1].Text + "(" + ssAllergy_Sheet1.Cells[j, 2].Text + ")" + "',";
                                }                              
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 3].Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 1].Text + "',";
                                SQL = SQL + ComNum.VBLF + "     '" + ssAllergy_Sheet1.Cells[j, 2].Text + "')";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                        else if (strROWID5 != "")
                        {
                            SQL = "";
                            SQL = " INSERT INTO KOSMOS_PMPA.ETC_ALLERGY_MST_HIS ";
                            SQL = SQL + ComNum.VBLF + " (PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, WRITEDATE, DELSABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR) ";
                            SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, CODE, ENTDATE, REMARK, SABUN, SYSDATE, " + txtSabun.Text + ", DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID5 + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            SQL = "";
                            SQL = "  DELETE FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID5 + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            Read_AST();
        }

        private void frmAllergyAndAnti_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            if (GstrPtNo != "")
            {
                txtPano.Text = GstrPtNo;
            }

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            if (GstrDate != "" && VB.IsDate(GstrDate))
            {
                dtpDate.Value = Convert.ToDateTime(GstrDate);
            }

            SCREEN_CLEAR();

            READ_BAS_PATIENT(txtPano.Text);

            READ_ALLERGY();

            Read_AST();
        }

        void SCREEN_CLEAR()
        {
            txtSabun.Text = "";
            lblPano.Text = "";
            lblSabun.Text = "";

            this.txtORDER.Text = string.Empty;
            this.txtDRUG.Text = string.Empty;

            //등록사번 필요 없음
            ssAllergy.ActiveSheet.Columns[6].Visible = false;
            ssAllergy.ActiveSheet.Columns[7].Visible = false;
            ssAllergy.ActiveSheet.Columns[8].Visible = false;
            //변경사번 필요 없음
            ssAllergy.ActiveSheet.Columns[12].Visible = false;
        }

        void READ_BAS_PATIENT(string argPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO ='" + argPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }


                lblPano.Text = "";

                if (dt.Rows.Count > 0)
                {
                    lblPano.Text = dt.Rows[0]["SName"].ToString().Trim();
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

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Column == 3)
            {
                ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() != "")
                {
                    txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                }

                READ_BAS_PATIENT(txtPano.Text);

                READ_ALLERGY();

                Read_AST();

                btnSearch.Focus();
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPano.Text) == true)
            {
                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

                READ_BAS_PATIENT(txtPano.Text);
            }

            //READ_ALLERGY();

            //Read_AST();
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSabun.Text = ComFunc.LPAD(txtSabun.Text, 5, "0");

                READ_SNAME(txtSabun.Text);

                txtPano.Focus();
            }
        }

        private void txtSabun_Leave(object sender, EventArgs e)
        {
            if (txtSabun.Text.Trim() != "")
            {
                txtSabun.Text = ComFunc.LPAD(txtSabun.Text, 5, "0");
                READ_SNAME(txtSabun.Text);
            }
        }

        void READ_SNAME(string argSabun)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN ='" + argSabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }


                lblSabun.Text = "";

                if (dt.Rows.Count > 0)
                {
                    lblSabun.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
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

        private string READ_DRUGNAME(string strDrugCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {                                
                SQL = " SELECT HNAME FROM KOSMOS_OCS.OCS_DRUGINFO_NEW WHERE SUNEXT = '"+ strDrugCode + "'";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["HNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }            
        }

        private void btnDrugSearch_Click(object sender, EventArgs e)
        {
            string strDamgb;
            if (optGb1.Checked == true)
            {
                strDamgb = "1";
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    MessageBox.Show("약품명, 성분명은 검색어를 입력해 주십시오.");
                    return;
                }
            }
            else if (optGb2.Checked == true)
            {
                strDamgb = "2";                
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    MessageBox.Show("약품명, 성분명은 검색어를 입력해 주십시오.");
                    return;
                }
            }
            else if (optGb3.Checked == true)
            {
                strDamgb = "3";
            }
            else
            {
                strDamgb = "4";
            }
            string strSearch = txtSearch.Text;


            using (frmAllergySearchDIF frm = new frmAllergySearchDIF(this as IAllergyInterface, strDamgb, strSearch))
            {
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 의약품 검색창에서 가져온 데이터 스프레드에 셋팅
        /// </summary>
        /// <param name="strRmk"></param>
        /// <param name="strDamKor"></param>
        /// <param name="strDamEng"></param>
        /// <param name="strDamCd"></param>
        /// <param name="strDamTypeNm"></param>
        /// <param name="strDamTypeCd"></param>
        public void setAllergyData(string strRmk, string[] strDamKor, string[] strDamEng, string[] strDamCd, string[] strDamTypeNm, string[] strDamTypeCd)
        {
            int nRow = ssAllergy_Sheet1.Rows.Count;
            ssAllergy_Sheet1.AddRows(ssAllergy_Sheet1.Rows.Count, strDamCd.Length);

            for (int i = 0; i < strDamCd.Length; i++)
            {
                ssAllergy_Sheet1.Cells[nRow, 1].Text = strDamEng[i];
                ssAllergy_Sheet1.Cells[nRow, 2].Text = strDamKor[i];
                ssAllergy_Sheet1.Cells[nRow, 3].Text = strDamTypeNm[i];
                ssAllergy_Sheet1.Cells[nRow, 4].Text = strRmk;                
                ssAllergy_Sheet1.Cells[nRow, 7].Text = strDamTypeCd[i];
                ssAllergy_Sheet1.Cells[nRow, 8].Text = strDamCd[i];
                nRow++;
            }

            for (int i = 0; i < ssAllergy_Sheet1.Columns.Count; i++)
            {
                ssAllergy_Sheet1.Columns[i].Width = ssAllergy_Sheet1.GetPreferredColumnWidth(i, true, true, true, true);
            }

            //증상, 변경사유는 고정
            ssAllergy_Sheet1.Columns[4].Width = 150;
            ssAllergy_Sheet1.Columns[13].Width = 150;

            ssAllergy_Sheet1.SetActiveCell(ssAllergy_Sheet1.Rows.Count - 1, 4);
            ssAllergy.ShowRow(0, ssAllergy_Sheet1.Rows.Count - 1, FarPoint.Win.Spread.VerticalPosition.Nearest);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnDrugSearch.PerformClick();
            }
        }

        private void btnADR_Click(object sender, EventArgs e)
        {
            using (frmComSupADR1 f = new frmComSupADR1(txtPano.Text, "", "", "", ""))
            {
                f.ShowDialog();
            }                
        }
    }
}
