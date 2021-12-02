using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSupDrstADRSimple.cs
    /// Description     : 약물이상반응(ADR) 발생 보고서(간편서식)
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응(ADR) 발생 보고서(간편서식)
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADR_Simple.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADRSimple : Form
    {
        private frmComSupADRContent frmComSupADRContentEvent = null;

        private string GstrSEQNO = "";
        private bool GbolDRUG = false;

        public frmComSupADRSimple()
        {
            InitializeComponent();
        }

        public frmComSupADRSimple(string strSEQNO)
        {
            InitializeComponent();

            GstrSEQNO = strSEQNO;
        }

        private void frmComSupADRSimple_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            GbolDRUG = true;

            READ_LIST();

            if (GstrSEQNO != "")
            {
                READ_SS1(GstrSEQNO);
            }
            else
            {
                GetNew();
            }
        }

        private void READ_LIST()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(WDATE,'YYYY-MM-DD') AS WDATE, SEQNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE";

                if (GbolDRUG == false)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE WSABUN = " + clsType.User.Sabun;
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY WDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void SCREEN_CLEAR()
        {
            ssView_Sheet1.Cells[2, 3].Text = "";            //등록번호
            ssView_Sheet1.Cells[2, 5].Text = "";            //성명
            ssView_Sheet1.Cells[2, 7].Text = "";            //성별/나이

            ssView_Sheet1.Cells[3, 3].Text = "";            //환자구분

            ssView_Sheet1.Cells[4, 3].Text = "";            //특이사항

            ssView_Sheet1.Cells[7, 3].Text = "";            //유해사례명

            ssView_Sheet1.Cells[8, 3].Text = "";            //증상 발현일
            ssView_Sheet1.Cells[8, 4].Text = "";            //증상 발현시

            ssView_Sheet1.Cells[9, 3].Text = "";            //증상 종료일
            ssView_Sheet1.Cells[9, 4].Text = "";            //증상 종료시

            ssView_Sheet1.Cells[10, 3].Text = "";           //증상 지속 일
            ssView_Sheet1.Cells[10, 4].Text = "";           //증상 지속 시
            ssView_Sheet1.Cells[10, 5].Text = "";           //증상 지속 분

            ssView_Sheet1.Cells[12, 3].Text = "";           //투약관의 연관성

            ssView_Sheet1.Cells[13, 3].Text = "";           //유해사례 경과

            ssView_Sheet1.Cells[14, 3].Text = "";           //증상에 대한 조치

            ssView_Sheet1.Cells[15, 3].Text = "";           //참고사항

            for (int i = 0; i <= 2; i++)
            {
                ssView_Sheet1.Cells[18 + (i * 7), 3].Text = "";     //약품명
                ssView_Sheet1.Cells[18 + (i * 7), 7].Text = "";     //약품코드

                ssView_Sheet1.Cells[19 + (i * 7), 3].Text = "";     //1회 투여량

                ssView_Sheet1.Cells[20 + (i * 7), 3].Text = "";     //1일 투여 횟수

                ssView_Sheet1.Cells[21 + (i * 7), 3].Text = "";     //투여기간
                ssView_Sheet1.Cells[21 + (i * 7), 5].Text = "";     //투여기간
                ssView_Sheet1.Cells[21 + (i * 7), 6].Text = "(총       일)";

                ssView_Sheet1.Cells[22 + (i * 7), 3].Text = "";     //투여경로

                ssView_Sheet1.Cells[23 + (i * 7), 3].Text = "";     //재투여시 유해사례 여부
            }
        }

        private void READ_SS1(string strSEQNO = "0")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            double dblNo = 0;

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO, WSABUN, WDATE, PTNO, SNAME, AGESEX, PATIENT_BUN, PATIENT_MEMO, ";
                SQL = SQL + ComNum.VBLF + "     INSTANCE, TO_CHAR(SDATE,'YYYY-MM-DD') AS SDATE_DAY, TO_CHAR(SDATE,'HH24:MI') AS SDATE_SECOND,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(EDATE,'YYYY-MM-DD') AS EDATE_DAY, TO_CHAR(EDATE,'HH24:MI') AS EDATE_SECOND, ";
                SQL = SQL + ComNum.VBLF + "     DURATION_DAY, DURATION_TIME, DURATION_SECOND, RELATION, PROGRESS, MEASURE, MEMO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.Cells[2, 3].Text = dt.Rows[0]["PTNO"].ToString().Trim();
                    ssView_Sheet1.Cells[2, 5].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[2, 7].Text = dt.Rows[0]["AGESEX"].ToString().Trim();

                    ssView_Sheet1.Cells[3, 3].Text = dt.Rows[0]["PATIENT_BUN"].ToString().Trim();

                    ssView_Sheet1.Cells[4, 3].Text = dt.Rows[0]["PATIENT_MEMO"].ToString().Trim();

                    ssView_Sheet1.Cells[7, 3].Text = dt.Rows[0]["INSTANCE"].ToString().Trim();

                    ssView_Sheet1.Cells[8, 3].Text = dt.Rows[0]["SDATE_DAY"].ToString().Trim();
                    ssView_Sheet1.Cells[8, 4].Text = dt.Rows[0]["SDATE_SECOND"].ToString().Trim();

                    ssView_Sheet1.Cells[9, 3].Text = dt.Rows[0]["EDATE_DAY"].ToString().Trim();
                    ssView_Sheet1.Cells[9, 4].Text = dt.Rows[0]["EDATE_SECOND"].ToString().Trim();

                    ssView_Sheet1.Cells[10, 3].Text = dt.Rows[0]["DURATION_DAY"].ToString().Trim();
                    ssView_Sheet1.Cells[10, 4].Text = dt.Rows[0]["DURATION_TIME"].ToString().Trim();
                    ssView_Sheet1.Cells[10, 5].Text = dt.Rows[0]["DURATION_SECOND"].ToString().Trim();

                    ssView_Sheet1.Cells[12, 3].Text = dt.Rows[0]["RELATION"].ToString().Trim();

                    ssView_Sheet1.Cells[13, 3].Text = dt.Rows[0]["PROGRESS"].ToString().Trim();

                    ssView_Sheet1.Cells[14, 3].Text = dt.Rows[0]["MEASURE"].ToString().Trim();

                    ssView_Sheet1.Cells[15, 3].Text = dt.Rows[0]["MEMO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SEQNO, NO, JEPCODE, QTY, DIV, TO_CHAR(SDATE,'YYYY-MM-DD') AS SDATE, TO_CHAR(EDATE,'YYYY-MM-DD') AS EDATE, DATE_CNT, PATH, HARZARD, JEPNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE_ORDER ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY NO ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        dblNo = VB.Val(dt.Rows[i]["NO"].ToString().Trim());

                        ssView_Sheet1.Cells[18 + (i * 7), 3].Text = dt.Rows[i]["JEPNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[18 + (i * 7), 7].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();

                        ssView_Sheet1.Cells[19 + (i * 7), 3].Text = dt.Rows[i]["QTY"].ToString().Trim();

                        ssView_Sheet1.Cells[20 + (i * 7), 3].Text = dt.Rows[i]["DIV"].ToString().Trim();

                        ssView_Sheet1.Cells[21 + (i * 7), 3].Text = dt.Rows[i]["SDATE"].ToString().Trim();

                        ssView_Sheet1.Cells[21 + (i * 7), 5].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[21 + (i * 7), 6].Text = dt.Rows[i]["DATE_CNT"].ToString().Trim();

                        ssView_Sheet1.Cells[22 + (i * 7), 3].Text = dt.Rows[i]["PATH"].ToString().Trim();

                        ssView_Sheet1.Cells[23 + (i * 7), 3].Text = dt.Rows[i]["HARZARD"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            READ_SS1();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            GetNew();
        }

        private void GetNew()
        {
            ComFunc.MsgBox("신규 작성입니다.");

            GstrSEQNO = "";

            SCREEN_CLEAR();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string strPANO = ssView_Sheet1.Cells[2, 3].Text.Trim();

            if (strPANO == "") { ComFunc.MsgBox("환자번호를 입력하십시오."); return; }

            if (SAVE_SS1(GstrSEQNO) == true)
            {
                this.Close();
            }
        }

        private bool SAVE_SS1(string strSEQNO = "")
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            
            string strWSABUN = "";
            string strWDATE = "";
            string strPTNO = "";
            string strSNAME = "";
            string strAGESEX = "";
            string strPATIENT_BUN = "";
            string strPATIENT_MEMO = "";
            string strINSTANCE = "";
            string strSDATE = "";
            string strEdate = "";
            string strDURATION_DAY = "";
            string strDURATION_TIME = "";
            string strDURATION_SECOND = "";
            string strRELATION = "";
            string strPROGRESS = "";
            string strMEASURE = "";
            string strMEMO = "";
            string strNO = "";
            string strJEPCODE = "";
            string strQTY = "";
            string strDIV = "";
            string strDATE_CNT = "";
            string strPATH = "";
            string strHARZARD = "";
            string strJEPNAME = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strSEQNO != "")
                {
                    if (ComFunc.MsgBoxQ("보고서를 수정하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE_ORDER ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + strSEQNO;

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
                else
                {
                    if (ComFunc.MsgBoxQ("보고서를 저장하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    strSEQNO = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_ADR").ToString();
                }

                strWSABUN = clsType.User.Sabun;
                strWDATE = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

                strPTNO = ssView_Sheet1.Cells[2, 3].Text.Trim();
                strSNAME = ssView_Sheet1.Cells[2, 5].Text.Trim();
                strAGESEX = ssView_Sheet1.Cells[2, 7].Text.Trim();

                strPATIENT_BUN = ssView_Sheet1.Cells[3, 3].Text.Trim();

                strPATIENT_MEMO = ssView_Sheet1.Cells[4, 3].Text.Trim();

                strINSTANCE = ssView_Sheet1.Cells[7, 3].Text.Trim();

                strSDATE = ssView_Sheet1.Cells[8, 3].Text.Trim();
                strSDATE = strSDATE + " " + ssView_Sheet1.Cells[8, 4].Text.Trim();

                strEdate = ssView_Sheet1.Cells[9, 3].Text.Trim();
                strEdate = strEdate + " " + ssView_Sheet1.Cells[9, 4].Text.Trim();

                strDURATION_DAY = ssView_Sheet1.Cells[10, 3].Text.Trim();
                strDURATION_TIME = ssView_Sheet1.Cells[10, 4].Text.Trim();
                strDURATION_SECOND = ssView_Sheet1.Cells[10, 5].Text.Trim();

                strRELATION = ssView_Sheet1.Cells[12, 3].Text.Trim();

                strPROGRESS = ssView_Sheet1.Cells[13, 3].Text.Trim();

                strMEASURE = ssView_Sheet1.Cells[14, 3].Text.Trim();

                strMEASURE = ssView_Sheet1.Cells[15, 3].Text.Trim();

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, WSABUN, WDATE, PTNO, SNAME, AGESEX, PATIENT_BUN, PATIENT_MEMO,";
                SQL = SQL + ComNum.VBLF + "     INSTANCE, SDATE, EDATE, DURATION_DAY, DURATION_TIME, DURATION_SECOND, ";
                SQL = SQL + ComNum.VBLF + "     RELATION, PROGRESS, MEASURE, MEMO)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + strSEQNO + ", ";
                SQL = SQL + ComNum.VBLF + "         " + strWSABUN + ", ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strWDATE + "', 'YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "         '" + strPTNO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strSNAME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strAGESEX + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strPATIENT_BUN + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strPATIENT_MEMO + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strINSTANCE + "', ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strSDATE + "', 'YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strEdate + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + "         '" + strDURATION_DAY + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDURATION_TIME + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strDURATION_SECOND + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strRELATION + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strPROGRESS + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMEASURE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strMEMO + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (i = 0; i <= 2; i++)
                {
                    strNO = (i + 1).ToString();

                    strJEPNAME = ssView_Sheet1.Cells[18 + (i * 7), 3].Text.ToString().Trim();
                    strJEPCODE = ssView_Sheet1.Cells[18 + (i * 7), 7].Text.ToString().Trim();
                    
                    strQTY = ssView_Sheet1.Cells[19 + (i * 7), 3].Text.ToString().Trim();
                    
                    strDIV = ssView_Sheet1.Cells[20 + (i * 7), 3].Text.ToString().Trim();
                    
                    strSDATE = ssView_Sheet1.Cells[21 + (i * 7), 3].Text.ToString().Trim();
                    strEdate = ssView_Sheet1.Cells[21 + (i * 7), 5].Text.ToString().Trim();
                    strDATE_CNT = ssView_Sheet1.Cells[21 + (i * 7), 6].Text.ToString().Trim();
                    
                    strPATH = ssView_Sheet1.Cells[22 + (i * 7), 3].Text.ToString().Trim();
                    
                    strHARZARD = ssView_Sheet1.Cells[23 + (i * 7), 3].Text.ToString().Trim();

                    if (strJEPCODE != "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE_ORDER";
                        SQL = SQL + ComNum.VBLF + "     (SEQNO, NO, JEPCODE, QTY, DIV, SDATE, EDATE, DATE_CNT, PATH, HARZARD, JEPNAME)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         " + strSEQNO + ", ";
                        SQL = SQL + ComNum.VBLF + "         " + strNO + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + strJEPCODE + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strQTY + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDIV + "', ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strSDATE + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strEdate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "         '" + strDATE_CNT + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPATH + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strHARZARD + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strJEPNAME + "' ";
                        SQL = SQL + ComNum.VBLF + "     )";

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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("삭제할 보고서를 선택하십시오.");
                return;
            }

            if (ComFunc.MsgBoxQ("보고서를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (GstrSEQNO != "")
                {
                    if (DelData() == true)
                    {
                        GetNew();
                    }
                }
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_ADR_SIMPLE_ORDER ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = " + GstrSEQNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
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

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) { return; }

            GstrSEQNO = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();

            READ_SS1(GstrSEQNO);
        }

        private void ssView_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row == 7 && e.Column == 6)
            {
                frmComSupADRContentEvent = new frmComSupADRContent();
                frmComSupADRContentEvent.SendEvent += FrmComSupADRContentEvent_SendEvent;
                frmComSupADRContentEvent.rEventClosed += FrmComSupADRContentEvent_rEventClosed;
                frmComSupADRContentEvent.StartPosition = FormStartPosition.CenterParent;
                frmComSupADRContentEvent.ShowDialog();
            }
        }

        private void FrmComSupADRContentEvent_SendEvent(string SendRetValue)
        {
            ssView_Sheet1.Cells[7, 3].Text = SendRetValue;

            frmComSupADRContentEvent.Dispose();
            frmComSupADRContentEvent = null;
        }

        private void FrmComSupADRContentEvent_rEventClosed()
        {
            frmComSupADRContentEvent.Dispose();
            frmComSupADRContentEvent = null;
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strPANO = "";
            string strSDATE = "";
            string strEdate = "";
            string strJEPCODE = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ssView_Sheet1.ActiveRowIndex == 2 && ssView_Sheet1.ActiveColumnIndex == 3)
                {
                    strPANO = ssView_Sheet1.Cells[2, 3].Text.Trim();


                    if (strPANO == "") { return; }

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO, SNAME, SEX ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPANO + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[2, 5].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[2, 7].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPANO);
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if ((ssView_Sheet1.ActiveRowIndex == 21 || ssView_Sheet1.ActiveRowIndex == 28 || ssView_Sheet1.ActiveRowIndex == 35)
                    && (ssView_Sheet1.ActiveColumnIndex == 3 || ssView_Sheet1.ActiveColumnIndex == 5))
                {
                    strSDATE = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text.Trim();
                    strEdate = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text.Trim();

                    if (strSDATE != "" && strEdate != "")
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     (TO_DATE('" + strEdate + "','YYYY-MM-DD') - TO_DATE('" + strSDATE + "','YYYY-MM-DD') + 1) AS SINCEDATE";
                        SQL = SQL + ComNum.VBLF + "FROM DUAL ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 6].Text = "(총 " + dt.Rows[0]["SINCEDATE"].ToString().Trim() + " 일)";
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
                else if ((ssView_Sheet1.ActiveRowIndex == 32 || ssView_Sheet1.ActiveRowIndex == 25 || ssView_Sheet1.ActiveRowIndex == 18)
                    && ssView_Sheet1.ActiveColumnIndex == 7)
                {
                    strJEPCODE = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 7].Text.Trim();

                    if (strJEPCODE != "")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, strJEPCODE);
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
