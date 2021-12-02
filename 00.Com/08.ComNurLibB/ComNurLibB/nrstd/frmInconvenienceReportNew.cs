using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmInconvenienceReportNew.cs
    /// Description     : 고객불편상황보고서
    /// Author          : 박창욱
    /// Create Date     : 2018-02-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm불편상황보고서_new.frm(Frm불편상황보고서_new.frm) >> frmInconvenienceReportNew.cs 폼이름 재정의" />	
    public partial class frmInconvenienceReportNew : Form
    {
        string FstrROWID = "";
        string FstrBuCode = "";
        string FstrUse_All = "";
        string GstrHelpCode = "";

        public frmInconvenienceReportNew()
        {
            InitializeComponent();
        }

        public frmInconvenienceReportNew(string strHelpCode)
        {
            InitializeComponent();

            this.GstrHelpCode = strHelpCode;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if (Delete_Data() == true)
            {
                Screen_Clear();
                Search_Data();
            }
        }

        bool Delete_Data()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까?") == DialogResult.No)
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "";
                SQL = "DELETE " + ComNum.DB_PMPA + "NUR_STD_INCONV";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Screen_Clear();

            grb1.Enabled = true;
            grb2.Enabled = true;
            grb3.Enabled = true;
            grb4.Enabled = true;
            grb5.Enabled = true;
            grb6.Enabled = true;

            btnSave.Enabled = true;

            txtBoName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

            txtSName.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Data();
        }

        void Print_Data()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false, 1.1f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strSName = "";
            string strAge = "";
            string strSex = "";
            string strTel = "";
            string strJuso = "";
            string strJepDate = "";
            string strCase1 = "";
            string strCase2 = "";
            string strCase3 = "";
            string strBDate = "";
            string strDtlbun = "";
            string strBun = "";
            string strSabun = "";
            string strBuse = "";
            string strJikJong = "";
            string strYoin1 = "";
            string strYoin2 = "";
            string strYoin3 = "";
            string strYoin4 = "";
            string strApprove1 = "";
            string strApprove2 = "";
            string strActs = "";
            string strBoDate = "";
            string strBoName = "";
            string strPano = "";
            string strWardCode = "";
            string strROOMCODE = "";
            string strCaseTemp = "";

            string strRowid2 = "";

            strSName = txtSName.Text.Trim();
            strAge = txtAge.Text.Trim();
            strSex = txtSex.Text.Trim();
            strJuso = txtJuso.Text.Trim();
            strTel = txtTel.Text.Trim();
            strJepDate = dtpJepDate.Value.ToString("yyyy-MM-dd");
            strCase1 = clsVbfunc.QuotationChange(txtCase1.Text.Trim());
            strCase2 = clsVbfunc.QuotationChange(txtCase2.Text.Trim());
            strCase3 = clsVbfunc.QuotationChange(txtCase3.Text.Trim());

            if (strCase1 != "")
            {
                strCaseTemp = strCase1 + " ";
            }
            if (strCase2 != "")
            {
                strCaseTemp += strCase2 + " ";
            }
            if (strCase3 != "")
            {
                strCaseTemp += strCase3;
            }

            strBDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strDtlbun = VB.Left(cboDtlBun.Text, 1);
            strBun = VB.Left(cboBun.Text, 1);
            strBuse = txtBuse.Text.Trim();
            strJikJong = VB.Pstr(lblName.Text, "/", 2);
            strSabun = txtSabun.Text.Trim();
            strYoin1 = clsVbfunc.QuotationChange(txtYoin1.Text.Trim());
            strYoin2 = clsVbfunc.QuotationChange(txtYoin2.Text.Trim());
            strYoin3 = clsVbfunc.QuotationChange(txtYoin3.Text.Trim());
            strYoin4 = clsVbfunc.QuotationChange(txtYoin4.Text.Trim());
            strBoDate = dtpBoDate.Value.ToString("yyyy-MM-dd");
            strBoName = txtBoName.Text.Trim();
            strPano = txtPano.Text.Trim();
            strWardCode = txtWardCode.Text.Trim();
            strROOMCODE = txtRoomCode.Text.Trim();

            if (chkApprove0.Checked == true)
            {
                strApprove1 += "1";
            }
            else
            {
                strApprove1 += "0";
            }

            if (chkApprove1.Checked == true)
            {
                strApprove1 += "1";
            }
            else
            {
                strApprove1 += "0";
            }

            if (chkApprove2.Checked == true)
            {
                strApprove1 += "1";
            }
            else
            {
                strApprove1 += "0";
            }

            strApprove2 = txtApprove.Text.Trim();
            strActs = clsVbfunc.QuotationChange(txtActs.Text.Trim());

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_STD_INCONV ( ";
                    SQL = SQL + ComNum.VBLF + " SNAME,SEX,AGE,TEL,JUSO,SABUN,BUCODE,JikJong,BDATE,JEPDATE,HARD_REMARK, ";
                    SQL = SQL + ComNum.VBLF + " SYSTEM_REMARK,HUMAN_REMARK,DTLBUN,BUN,YOIN_1,YOIN_2,YOIN_3,YOIN_4,APPROVE,APPROVE_REMARK,";
                    SQL = SQL + ComNum.VBLF + " ACTS,Pano,WardCode,RoomCode,BoDate,BoName,EntDate,ENTSABUN) VALUES ('" + strSName + "','" + strSex + "' , ";
                    SQL = SQL + ComNum.VBLF + " " + VB.Val(strAge) + " , '" + strTel + "', '" + strJuso + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSabun + "' , '" + strBuse + "','" + strJikJong + "',  ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strBDate + "','YYYY-MM-DD'),TO_DATE('" + strJepDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strCase1 + "','" + strCase2 + "', '" + strCase3 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDtlbun + "','" + strBun + "','" + strYoin1 + "', '" + strYoin2 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strYoin3 + "','" + strYoin4 + "','" + strApprove1 + "','" + strApprove2 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strActs + "','" + strPano + "', '" + strWardCode + "', " + VB.Val(strROOMCODE) + " , ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strBoDate + "','YYYY-MM-DD'),'" + strBoName + "',  ";
                    SQL = SQL + ComNum.VBLF + " SYSDATE, " + clsType.User.Sabun + " ) ";
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_STD_INCONV  SET ";
                    SQL = SQL + ComNum.VBLF + " SNAME ='" + strSName + "', ";
                    SQL = SQL + ComNum.VBLF + " SEX ='" + strSex + "', ";
                    SQL = SQL + ComNum.VBLF + " AGE =" + VB.Val(strAge) + ", ";
                    SQL = SQL + ComNum.VBLF + " TEL ='" + strTel + "', ";
                    SQL = SQL + ComNum.VBLF + " JUSO ='" + strJuso + "', ";
                    SQL = SQL + ComNum.VBLF + " SABUN ='" + strSabun + "', ";
                    SQL = SQL + ComNum.VBLF + " BuCode ='" + strBuse + "', ";
                    SQL = SQL + ComNum.VBLF + " JikJong ='" + strJikJong + "', ";
                    SQL = SQL + ComNum.VBLF + " BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD') ,  ";
                    SQL = SQL + ComNum.VBLF + " JepDATE =TO_DATE('" + strJepDate + "','YYYY-MM-DD') ,  ";
                    SQL = SQL + ComNum.VBLF + " HARD_REMARK ='" + strCase1 + "', ";
                    SQL = SQL + ComNum.VBLF + " SYSTEM_REMARK ='" + strCase2 + "', ";
                    SQL = SQL + ComNum.VBLF + " HUMAN_REMARK ='" + strCase3 + "', ";
                    SQL = SQL + ComNum.VBLF + " DTLBUN ='" + strDtlbun + "', ";
                    SQL = SQL + ComNum.VBLF + " BUN ='" + strBun + "', ";
                    SQL = SQL + ComNum.VBLF + " YOIN_1 ='" + strYoin1 + "', ";
                    SQL = SQL + ComNum.VBLF + " YOIN_2 ='" + strYoin2 + "', ";
                    SQL = SQL + ComNum.VBLF + " YOIN_3 ='" + strYoin3 + "', ";
                    SQL = SQL + ComNum.VBLF + " YOIN_4 ='" + strYoin4 + "', ";
                    SQL = SQL + ComNum.VBLF + " APPROVE ='" + strApprove1 + "', ";
                    SQL = SQL + ComNum.VBLF + " APPROVE_REMARK ='" + strApprove2 + "', ";
                    SQL = SQL + ComNum.VBLF + " ACTS ='" + strActs + "', ";
                    SQL = SQL + ComNum.VBLF + " BoDate =TO_DATE('" + strBoDate + "','YYYY-MM-DD') ,  ";
                    SQL = SQL + ComNum.VBLF + " BoName ='" + strBoName + "', ";
                    SQL = SQL + ComNum.VBLF + " Pano ='" + strPano + "', ";
                    SQL = SQL + ComNum.VBLF + " WardCode ='" + strWardCode + "', ";
                    SQL = SQL + ComNum.VBLF + " RoomCode =" + VB.Val(strROOMCODE) + ", ";
                    SQL = SQL + ComNum.VBLF + " EntDate =SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
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
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CS_RECOMEND ";
                SQL = SQL + ComNum.VBLF + " WHERE Sabun ='" + strSabun + "' ";
                SQL = SQL + ComNum.VBLF + "  AND RDate =TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strRowid2 = "";

                if (dt.Rows.Count > 0)
                {
                    strRowid2 = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //TEST로 웹으로 전송 안 함 99999
                if (strRowid2 == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "NUR_CS_RECOMEND (RDate,Sabun,BuCode,Gubun,DtlBun,Sname,Age,";
                    SQL = SQL + ComNum.VBLF + " Sex,Tel,Juso,Remark1,SendSeqno,EntTime,EntSabun) ";
                    SQL = SQL + ComNum.VBLF + " VALUES (TO_DATE('" + strBDate + "','YYYY-MM-DD'),'";
                    SQL = SQL + ComNum.VBLF + strSabun + "','" + strBuse + "','" + strBun + "','" + strDtlbun + "','";
                    SQL = SQL + ComNum.VBLF + strSName + "'," + VB.Val(strAge) + ",'" + strSex + "','";
                    SQL = SQL + ComNum.VBLF + strTel + "','" + strJuso + "','";
                    SQL = SQL + ComNum.VBLF + strCaseTemp + "',99999,SYSDATE,";
                    SQL = SQL + ComNum.VBLF + clsType.User.Sabun + ") ";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_CS_RECOMEND SET ";
                    SQL = SQL + ComNum.VBLF + " RDate=TO_DATE('" + strBDate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " Sabun='" + strSabun + "',";
                    SQL = SQL + ComNum.VBLF + " BuCode='" + strBuse + "',";
                    SQL = SQL + ComNum.VBLF + " Gubun='" + strBun + "',";
                    SQL = SQL + ComNum.VBLF + " DtlBun='" + strDtlbun + "',";
                    SQL = SQL + ComNum.VBLF + " SName='" + strSName + "',";
                    SQL = SQL + ComNum.VBLF + " Age=" + VB.Val(strAge) + ",";
                    SQL = SQL + ComNum.VBLF + " Sex='" + strSex + "',";
                    SQL = SQL + ComNum.VBLF + " Tel='" + strTel + "',";
                    SQL = SQL + ComNum.VBLF + " Juso='" + strJuso + "',";
                    SQL = SQL + ComNum.VBLF + " Remark1='" + strCaseTemp + "',";
                    SQL = SQL + ComNum.VBLF + " EntTime=SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " EntSabun=" + clsType.User.Sabun + " ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strRowid2 + "' ";
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

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (ComFunc.MsgBoxQ("저장 완료. 저장한 자료를 인쇄하시겠습니까?") == DialogResult.Yes)
                {
                    Print_Data();
                }

                Screen_Clear();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            string strFDate = "";
            string strTDate = "";
            string strSabun = "";

            Screen_Clear();

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            if (VB.Val(clsType.User.Sabun) <= 99999)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            FstrUse_All = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='1'";
                SQL = SQL + ComNum.VBLF + "   AND Remark ='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrUse_All = "OK";
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT a.SNAME,a.SEX,a.AGE,a.TEL,a.JUSO,a.SABUN,a.BUCODE,a.BTIME,a.WARDCODE,a.HARD_REMARK, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate, ";
                SQL = SQL + ComNum.VBLF + " a.SYSTEM_REMARK,a.HUMAN_REMARK,a.DTLBUN,a.BUN,a.YOIN_1,a.YOIN_2,a.YOIN_3,a.YOIN_4,a.APPROVE,a.APPROVE_REMARK, ";
                SQL = SQL + ComNum.VBLF + " a.ACTS , a.ENTSABUN, a.ROWID, b.Name AS BuName  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_STD_INCONV a, " + ComNum.DB_PMPA + "BAS_BUSE b ";
                SQL = SQL + ComNum.VBLF + "  WHERE JepDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JepDate <=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.BuCode=b.BuCode(+) ";
                if (FstrUse_All != "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND EntSabun = " + clsType.User.Sabun + " ";
                }
                switch (GstrHelpCode)
                {
                    case "전체":
                    case "":
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
                        SQL = SQL + ComNum.VBLF + " AND a.WardCode='" + GstrHelpCode.Trim() + "' ";
                        break;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow += 1;
                    if (nRow > ssView2_Sheet1.RowCount)
                    {
                        ssView2_Sheet1.RowCount = nRow;
                    }

                    ssView2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["JepDate"].ToString().Trim();
                    switch (dt.Rows[i]["Bun"].ToString().Trim())
                    {
                        case "1":
                            ssView2_Sheet1.Cells[nRow - 1, 1].Text = "개인친절";
                            break;
                        case "2":
                            ssView2_Sheet1.Cells[nRow - 1, 1].Text = "개인불친절";
                            break;
                        case "3":
                            ssView2_Sheet1.Cells[nRow - 1, 1].Text = "부서친절";
                            break;
                        case "4":
                            ssView2_Sheet1.Cells[nRow - 1, 1].Text = "부서불친절";
                            break;
                    }

                    if (dt.Rows[i]["Sabun"].ToString().Trim() != "")
                    {
                        SQL = "";
                        SQL = " SELECT a.KorName KorName, b.Name JikName ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_ERP + "INSA_CODE b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Sabun='" + dt.Rows[i]["Sabun"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + " And a.Jik=b.Code ";
                        SQL = SQL + ComNum.VBLF + " And b.Gubun = '2' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 5].Text = dt1.Rows[0]["KorName"].ToString().Trim();
                            ssView2_Sheet1.Cells[nRow - 1, 7].Text = dt1.Rows[0]["JikName"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssView2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                    if (dt.Rows[i]["Sex"].ToString().Trim() != "")
                    {
                        ssView2_Sheet1.Cells[nRow - 1, 3].Text += "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    }
                    ssView2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["BuName"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                ssView2_Sheet1.RowCount = nRow;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmInconvenienceReportNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Screen_Clear();

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-10);

            cboBun.Items.Clear();
            cboBun.Items.Add("2.개인 불편불만");
            cboBun.Items.Add("4.부서 불편불만");

            cboDtlBun.Items.Clear();
            cboDtlBun.Items.Add("1.Hard Ware");
            cboDtlBun.Items.Add("2.System Ware");
            cboDtlBun.Items.Add("3.Human Ware");
            cboDtlBun.Items.Add("            ");
        }

        private void ssView2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            Screen_Clear();

            FstrROWID = ssView2_Sheet1.Cells[e.Row, 8].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT a.Sabun,a.BuCode,b.KorName,a.Sex,a.Age,a.Tel,a.Juso,a.Bun,a.DtlBun, ";
                SQL = SQL + ComNum.VBLF + " a.Hard_Remark,a.System_Remark,a.Human_Remark,a.BoName,a.Yoin_1,a.Yoin_2,a.Yoin_3,a.Yoin_4, ";
                SQL = SQL + ComNum.VBLF + " a.Approve,a.Approve_Remark,a.Acts,a.Pano,a.WardCode,a.RoomCode, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(BDate,'YYYY-MM-DD') BDate,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(BoDate,'YYYY-MM-DD') BoDate, ";
                SQL = SQL + ComNum.VBLF + " c.Name BuName,d.Name JikName,a.SName,a.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_STD_INCONV a, " + ComNum.DB_ERP + "INSA_MST b,";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_BUSE c, " + ComNum.DB_ERP + "INSA_CODE d ";
                SQL = SQL + ComNum.VBLF + "WHERE a.ROWID='" + FstrROWID + "' ";
                if (ssView2_Sheet1.Cells[e.Row, 6].Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND a.Sabun=b.Sabun(+) ";
                }
                SQL = SQL + ComNum.VBLF + "  AND a.BuCode=c.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND b.Jik=d.Code ";
                SQL = SQL + ComNum.VBLF + "  AND d.Gubun='2' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                FstrBuCode = dt.Rows[0]["BuCode"].ToString().Trim();
                txtSName.Text = dt.Rows[0]["Age"].ToString().Trim();
                txtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
                txtJuso.Text = dt.Rows[0]["Juso"].ToString().Trim();
                txtSabun.Text = dt.Rows[0]["Sabun"].ToString().Trim();
                txtBuse.Text = dt.Rows[0]["BuCode"].ToString().Trim();
                if (txtSabun.Text.Trim() != "")
                {
                    lblName.Text = dt.Rows[0]["KorName"].ToString().Trim() + "/";
                    lblName.Text += dt.Rows[0]["BuName"].ToString().Trim();
                    lblBuse.Text = dt.Rows[0]["JikName"].ToString().Trim();
                }
                dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["BDate"].ToString().Trim());
                dtpJepDate.Value = Convert.ToDateTime(dt.Rows[0]["JepDate"].ToString().Trim());
                cboBun.Text = "";
                switch (dt.Rows[0]["Bun"].ToString().Trim())
                {
                    case "2":
                        cboBun.SelectedIndex = 0;
                        break;
                    case "4":
                        cboBun.SelectedIndex = 1;
                        break;
                }
                cboDtlBun.Text = "";
                switch (dt.Rows[0]["Dtlbun"].ToString().Trim())
                {
                    case "1":
                        cboDtlBun.SelectedIndex = 0;
                        break;
                    case "2":
                        cboDtlBun.SelectedIndex = 1;
                        break;
                    case "3":
                        cboDtlBun.SelectedIndex = 2;
                        break;
                }

                txtCase1.Text = dt.Rows[0]["Hard_Remark"].ToString().Trim();
                txtCase2.Text = dt.Rows[0]["System_Remark"].ToString().Trim();
                txtCase3.Text = dt.Rows[0]["Human_Remark"].ToString().Trim();
                dtpBoDate.Value = Convert.ToDateTime(dt.Rows[0]["BoDate"].ToString().Trim());
                txtBoName.Text = dt.Rows[0]["BoName"].ToString().Trim();
                txtYoin1.Text = dt.Rows[0]["Yoin_1"].ToString().Trim();
                txtYoin2.Text = dt.Rows[0]["Yoin_2"].ToString().Trim();
                txtYoin3.Text = dt.Rows[0]["Yoin_3"].ToString().Trim();
                txtYoin4.Text = dt.Rows[0]["Yoin_4"].ToString().Trim();

                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 1, 1) == "1")
                {
                    chkApprove0.Checked = true;
                }
                else
                {
                    chkApprove0.Checked = false;
                }

                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 2, 1) == "1")
                {
                    chkApprove1.Checked = true;
                }
                else
                {
                    chkApprove1.Checked = false;
                }

                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 3, 1) == "1")
                {
                    chkApprove2.Checked = true;
                }
                else
                {
                    chkApprove2.Checked = false;
                }

                txtApprove.Text = dt.Rows[0]["Approve_Remark"].ToString().Trim();
                txtActs.Text = dt.Rows[0]["Acts"].ToString().Trim();
                txtPano.Text = dt.Rows[0]["Pano"].ToString().Trim();
                txtWardCode.Text = dt.Rows[0]["WardCode"].ToString().Trim();
                txtRoomCode.Text = dt.Rows[0]["RoomCode"].ToString().Trim();

                //인쇄용 시트
                ssView_Sheet1.Cells[5, 2].Text = txtSName.Text.Trim();
                ssView_Sheet1.Cells[5, 4].Text = txtAge.Text.Trim() + "/" + txtSex.Text.Trim();
                ssView_Sheet1.Cells[6, 2].Text = txtJuso.Text.Trim();
                ssView_Sheet1.Cells[7, 2].Text = txtTel.Text.Trim();
                ssView_Sheet1.Cells[7, 4].Text = dtpDate.Value.ToString("yyyy-MM-dd");
                ssView_Sheet1.Cells[8, 2].Text = VB.Space(5) + dtpJepDate.Value.ToString("yyyy-MM-dd");
                ssView_Sheet1.Cells[12, 3].Text = VB.Space(2) + txtCase1.Text.Trim();
                ssView_Sheet1.Cells[13, 3].Text = VB.Space(2) + txtCase2.Text.Trim();
                ssView_Sheet1.Cells[14, 3].Text = VB.Space(2) + txtCase3.Text.Trim();
                ssView_Sheet1.Cells[17, 2].Text = VB.Pstr(lblName.Text, "/", 1) + "/" + txtSabun.Text.Trim();
                ssView_Sheet1.Cells[17, 4].Text = lblBuse.Text;
                strTemp = "";
                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 1, 1) == "1")
                {
                    strTemp = "▣면담  ";
                }
                else
                {
                    strTemp = "□면담  ";
                }

                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 2, 1) == "1")
                {
                    strTemp += "▣전화  ";
                }
                else
                {
                    strTemp += "□전화  ";
                }

                if (VB.Mid(dt.Rows[0]["Approve"].ToString().Trim(), 3, 1) == "1")
                {
                    strTemp += "▣이메일  ";
                }
                else
                {
                    strTemp += "□이메일  ";
                }

                ssView_Sheet1.Cells[20, 1].Text = strTemp;
                ssView_Sheet1.Cells[21, 2].Text = VB.Space(2) + txtApprove.Text.Trim();
                ssView_Sheet1.Cells[24, 1].Text = VB.Space(2) + txtActs.Text.Trim();
                ssView_Sheet1.Cells[26, 4].Text = dtpBoDate.Value.ToString("yyyy-MM-dd");
                ssView_Sheet1.Cells[26, 7].Text = txtBoName.Text.Trim();

                dt.Dispose();
                dt = null;

                grb1.Enabled = true;
                grb2.Enabled = true;
                grb3.Enabled = true;
                grb4.Enabled = true;
                grb5.Enabled = true;
                grb6.Enabled = true;

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            lblName.Text = "";
            lblBuse.Text = "";

            if (txtSabun.Text.Trim() == "")
            {
                txtBuse.Focus();
                return;
            }

            txtSabun.Text = VB.Val(txtSabun.Text).ToString("00000");

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //명단을 SELECT
                SQL = "";
                SQL = "SELECT a.Sabun,a.KorName,TO_CHAR(a.BalDay,'YYYY-MM-DD') BalDay,";
                SQL = SQL + ComNum.VBLF + " a.Buse,a.Jik,b.Name BuseName,c.Name JikName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c ";
                SQL = SQL + ComNum.VBLF + "WHERE a.Sabun='" + txtSabun.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.Buse=b.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Jik=c.Code(+) ";
                SQL = SQL + ComNum.VBLF + "  AND c.Gubun='2' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    lblName.Text = "";
                    lblBuse.Text = "";
                }
                else
                {
                    lblName.Text = dt.Rows[0]["KorName"].ToString().Trim() + "/";
                    lblName.Text += dt.Rows[0]["JikName"].ToString().Trim();
                    txtBuse.Text = dt.Rows[0]["Buse"].ToString().Trim();
                    lblBuse.Text += dt.Rows[0]["BuseName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                txtBuse.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Screen_Clear()
        {
            txtSName.Text = "";
            txtAge.Text = "";
            txtTel.Text = "";
            txtJuso.Text = "";
            txtSabun.Text = "";
            txtSex.Text = "";
            lblName.Text = "";
            lblBuse.Text = "";
            txtBuse.Text = "";
            cboBun.Text = "";
            cboDtlBun.Text = "";
            txtBoName.Text = "";
            txtPano.Text = "";
            txtRoomCode.Text = "";
            txtWardCode.Text = "";

            txtCase1.Text = "";
            txtCase2.Text = "";
            txtCase3.Text = "";
            txtYoin1.Text = "";
            txtYoin2.Text = "";
            txtYoin3.Text = "";
            txtYoin4.Text = "";
            txtApprove.Text = "";
            txtActs.Text = "";

            chkApprove0.Checked = false;
            chkApprove1.Checked = false;
            chkApprove2.Checked = false;

            grb1.Enabled = false;
            grb2.Enabled = false;
            grb3.Enabled = false;
            grb4.Enabled = false;
            grb5.Enabled = false;
            grb6.Enabled = false;

            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = false;

            FstrROWID = "";
        }

        private void txtSex_Enter(object sender, EventArgs e)
        {
            txtSex.ImeMode = ImeMode.Alpha;
        }

        private void txtSex_Leave(object sender, EventArgs e)
        {
            txtSex.Text = txtSex.Text.ToUpper();
            if (ComFunc.LenH(txtSex.Text) > 1)
            {
                ComFunc.MsgBox("성별은 M, F로 표기하세요.");
                txtSex.Focus();
            }
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAge.Focus();
            }
        }

        private void txtAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSex.Focus();
            }
        }

        private void txtSex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTel.Focus();
            }
        }

        private void txtTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtJuso.Focus();
            }
        }

        private void txtJuso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpJepDate.Focus();
            }
        }

        private void dtpJepDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Focus();
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtWardCode.Focus();
            }
        }

        private void txtWardCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRoomCode.Focus();
            }
        }

        private void txtRoomCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCase1.Focus();
            }
        }

        private void txtCase1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                txtCase2.Focus();
            }
        }

        private void txtCase2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                txtCase3.Focus();
            }
        }

        private void txtCase3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                dtpDate.Focus();
            }
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDtlBun.Focus();
            }
        }

        private void cboDtlBun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboBun.Focus();
            }
        }

        private void cboBun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSabun.Focus();
            }
        }

        private void txtBuse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtYoin1.Focus();
            }
        }

        private void txtYoin1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtYoin2.Focus();
            }
        }

        private void txtYoin2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtYoin3.Focus();
            }
        }

        private void txtYoin3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtYoin4.Focus();
            }
        }

        private void txtYoin4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtApprove.Focus();
            }
        }

        private void txtApprove_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                txtActs.Focus();
            }
        }

        private void txtActs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                dtpBoDate.Focus();
            }
        }

        private void dtpBoDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBoName.Focus();
            }
        }
    }
}
