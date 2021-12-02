using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmAccount
    /// File Name : frmAccount.cs
    /// Title or Description : 산정기준 등록
    /// Author : 박창욱
    /// Create Date : 2017-06-07
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>  
    /// <history>  
    /// VB\BuCode41.frm(FrmAccount) -> frmAccount.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\BuCode41.frm(FrmAccount)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\\bucode.vbp
    /// </vbp>
    public partial class frmAccount : Form
    {
        private string gstrMsgList = "";

        public frmAccount()
        {
            InitializeComponent();
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR ();
            ComFunc.ReadSysDate(clsDB.DbCon);

            //작업종류를 ComboBox에 Set
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboJob, "BAS_ACCOUNT", 1, true, "N");
            cboJob.SelectedIndex = 0;

            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            cboDate.Visible = false;
            dtpDate.Visible = true;
        }

        private void SCREEN_CLEAR()
        {
            rdoInsert.Enabled = true;
            rdoUpdate.Enabled = true;
            dtpDate.Enabled = true;
            cboDate.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            ssView.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (searchData() == true)
            {
                cboDate.Enabled = false;
                dtpDate.Enabled = false;
                btnDelete.Enabled = true;
                rdoInsert.Enabled = false;
                rdoUpdate.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private bool searchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            int i = 0;
            string strIDName = "";
            string strStartDate = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ssView.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;

            strIDName = Combo_To_IDname();
            if (strIDName == "")
            {
                return false;
            }

            try
            {
                if (rdoInsert.Checked == true)
                {
                    //GoSub CmdView_INSERT
                    #region btnSearch_INSERT

                    //신규자료
                    //자료를 READ
                    SQL = "";
                    SQL = "SELECT TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,COUNT(*) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT ";
                    SQL = SQL + ComNum.VBLF + "WHERE IDname = '" + strIDName + "' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY StartDate ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY StartDate DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    strStartDate = "";
                    if (dt.Rows.Count > 0)
                    {
                        strStartDate = dt.Rows[0]["StartDate"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;

                    cboDate.Enabled = true;
                    dtpDate.Enabled = true;

                    if (strStartDate == "") { return false; }

                    //최종자료를 Sheet에 Display
                    SQL = "";
                    SQL = "SELECT ArrayClass,RateValue,RateText,ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT ";
                    SQL = SQL + ComNum.VBLF + "WHERE IDName = '" + strIDName + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND StartDate = TO_DATE('" + strStartDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count + 10;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ArrayClass"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RateValue"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RateText"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = "";
                    }
                    dt.Dispose();
                    dt = null;

                    #endregion
                }
                if (rdoUpdate.Checked == true)
                {
                    //GoSub CmdView_UPDATE
                    #region btnSearch_UPDATE

                    //수정작업

                    SQL = "";
                    SQL = "SELECT ArrayClass,RateValue,RateText,ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT ";
                    SQL = SQL + ComNum.VBLF + "WHERE IDName = '" + strIDName + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND StartDate = TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    ssView_Sheet1.RowCount = dt.Rows.Count + 10;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ArrayClass"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RateValue"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RateText"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                    #endregion
                }
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private string Combo_To_IDname()
        {
            string strIDName = "";

            switch (VB.Left(cboJob.Text, 1))
            {
                case "1": strIDName = "OPD_BON"; break;
                case "2": strIDName = "OPD_BON2"; break;
                case "3": strIDName = "IPD_BON"; break;
                case "4": strIDName = "GISUL"; break;
                case "5": strIDName = "NIGHT"; break;
                case "6": strIDName = "NIGHT_22"; break;
                case "7": strIDName = "JOJE"; break;
                case "8": strIDName = "PEDADD"; break;
                case "9": strIDName = "JUNG_BON"; break;
                case "A": strIDName = "SPC_BON"; break;
                case "B": strIDName = "DRUG_MIR"; break;
                case "C": strIDName = "GAMEK_JIN"; break;
                case "D": strIDName = "GAM_OPD"; break;
                case "E": strIDName = "GAM_IPD"; break;
                case "F": strIDName = "GAM_BOHUM"; break;
                case "G": strIDName = "GAM_ILBAN"; break;
                case "H": strIDName = "BON_TAX"; break;
                case "I": strIDName = "IPD_SANG"; break;
                default: strIDName = ""; break;
            }

            return strIDName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveData() == true)
            {
                SCREEN_CLEAR();
                cboJob.Focus();
            }
        }

        private bool saveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            int i = 0;
            int nClass = 0;
            int nValue = 0;
            string strIDName = "";
            string strText = "";
            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strIDName = Combo_To_IDname();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    nClass = (int)(VB.Val(ssView_Sheet1.Cells[i - 1, 0].Text));
                    nValue = (int)(VB.Val(ssView_Sheet1.Cells[i - 1, 1].Text));
                    strText = ssView_Sheet1.Cells[i - 1, 2].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i - 1, 3].Text.Trim();

                    if (rdoInsert.Checked == true)
                    {
                        SQL = "";
                        SQL = "INSERT INTO BAS_ACCOUNT (IDName,StartDate,ArrayClass,RateValue,";
                        SQL = SQL + ComNum.VBLF + "RateText) VALUES ('" + strIDName;
                        SQL = SQL + ComNum.VBLF + "',TO_DATE('" + dtpDate.Value.ToString() + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + nClass + "," + nValue + ",'" + strText + "') ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE BAS_ACCOUNT SET ArrayClass=" + nClass + ",";
                        SQL = SQL + ComNum.VBLF + "RateValue=" + nValue + ",";
                        SQL = SQL + ComNum.VBLF + "RateText = '" + strText + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                    }
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
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (deleteData() == true)
            {
                SCREEN_CLEAR();
                ComboDate_SET();

                cboJob.Focus();
            }
        }

        private bool deleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string strIDName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            gstrMsgList = "화면의 자료를 정말 삭제하시겠습니까?";
            if (ComFunc.MsgBoxQ(gstrMsgList, "삭제여부") == DialogResult.No)
            {
                return false;
            }

            strIDName = Combo_To_IDname();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //자료 삭제
                SQL = "";
                SQL = "DELETE BAS_ACCOUNT ";
                SQL = SQL + ComNum.VBLF + "WHERE IDname = '" + strIDName + "' ";
                SQL = SQL + ComNum.VBLF + "  AND StartDate=TO_DATE('" + cboDate.Text + "','YYYY-MM-DD') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        private void ComboDate_SET()
        {
            int i = 0;
            string strIDName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (cboJob.Text == "") { return; }

            cboDate.Items.Clear();
            strIDName = Combo_To_IDname();
            if (strIDName == "") { return; }

            //자료를 READ
            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT ";
                SQL = SQL + ComNum.VBLF + "WHERE IDname = '" + strIDName + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY StartDate ";
                SQL = SQL + ComNum.VBLF + "ORDER BY StartDate DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["StartDate"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            cboJob.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            if(printDataSelect() == true)
            {
                string strFont1 = "";
                string strFont2 = "";
                string strHead1 = "";
                string strHead2 = "";


                //자료를 인쇄
                strFont1 = "/fn\"굴림체\" /fz\"15\"";
                strFont2 = "/fn\"굴림체\" /fz\"10\"";
                strHead1 = "/n" + "/l/f1" + VB.Space(16) + "산정기준 코드집(BAS_ACCOUNT)" + "/n";
                strHead2 = "/l/f2" + "출력일자 : " + clsPublic.GstrSysDate + VB.Space(65) + "PAGE:" + "/p";

                ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
                ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssView2_Sheet1.PrintInfo.Margin.Top = 30;
                ssView2_Sheet1.PrintInfo.Margin.Bottom = 180;
                ssView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssView2_Sheet1.PrintInfo.ShowBorder = true;
                ssView2_Sheet1.PrintInfo.ShowColor = false;
                ssView2_Sheet1.PrintInfo.ShowGrid = true;
                ssView2_Sheet1.PrintInfo.ShowShadows = false;
                ssView2_Sheet1.PrintInfo.UseMax = false;
                ssView2_Sheet1.PrintInfo.PrintType = PrintType.All;
                ssView2.PrintSheet(0);

                Cursor.Current = Cursors.Default;

            }
        }

        private bool printDataSelect()
        {
            int i = 0;
            int inx = 0;
            int nRow = 0;
            int nCnt = 0;
            int nRead = 0;
            string strIDName = "";
            string strDate1 = "";
            string strDate2 = "";
            string strOldData = "";
            string strNewData = "";
            string strJobList = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            nRow = 0;
            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 20;

            strJobList = "OPD_BON,OPD_BON2,IPD_BON,GISUL,NIGHT,NIGHT_22,JOJE,PEDADD,"
                         + "JUNG_BON,SPC_BON,DRUG_MIR,GAMEK_JIN,GAM_OPD,GAM_IPD,GAM_BOHUM,GAM_ILBAN";
            nCnt = VB.I(strJobList, ",");

            try
            {
                for (inx = 1; inx <= nCnt; inx++)
                {
                    strIDName = VB.Pstr(strJobList, ",", inx);

                    //최근 적용일자 두 건만 SELECT
                    SQL = "";
                    SQL = "SELECT TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,COUNT(*) CNT ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_ACCOUNT ";
                    SQL = SQL + ComNum.VBLF + " WHERE IDname='" + strIDName + "' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY StartDate ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY StartDate DESC ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    strDate1 = dt.Rows[0]["StartDate"].ToString().Trim();
                    strDate2 = dt.Rows[1]["StartDate"].ToString().Trim();
                    dt.Dispose();
                    dt = null;

                    //자료를 SELECT
                    SQL = "";
                    SQL = "SELECT IDname,TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,";
                    SQL = SQL + ComNum.VBLF + "      ArrayClass,RateValue,RateText ";
                    SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT ";
                    SQL = SQL + ComNum.VBLF + "WHERE IDname='" + strIDName + "' ";
                    if (strDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND StartDate>=TO_DATE('" + strDate2 + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND StartDate<=TO_DATE('" + strDate1 + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND StartDate=TO_DATE('" + strDate1 + "','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + "ORDER BY IDname,StartDate DESC,ArrayClass ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    strOldData = "";
                    nRead = dt.Rows.Count;
                    for (i = 0; i < nRead; i++)
                    {
                        strNewData = VB.Left(dt.Rows[i]["IDname"].ToString().Trim() + VB.Space(12), 12);
                        strNewData = strNewData + dt.Rows[i]["StartDate"].ToString().Trim();
                        nRow = nRow + 1;
                        if (nRow > ssView2_Sheet1.RowCount) { ssView2_Sheet1.RowCount = nRow; }
                        if (strNewData != strOldData)
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["IDname"].ToString().Trim();
                            switch (dt.Rows[i]["IDname"].ToString().Trim())
                            {
                                case "OPD_BON":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "외래본인부담율(25000이하)";
                                    break;
                                case "OPD_BON2":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "외래본인부담율(25000초과)";
                                    break;
                                case "IPD_BON":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "입원본인부담율";
                                    break;
                                case "GISUL":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "기술료가산";
                                    break;
                                case "NIGHT":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "야간가산율";
                                    break;
                                case "NIGHT_22":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "마취야간가산율";
                                    break;
                                case "JOJE":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "내복약조제료";
                                    break;
                                case "PEDADD":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "6세미만진찰료가산액";
                                    break;
                                case "JUNG_BON":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "중증질환자 본인부담율";
                                    break;
                                case "SPC_BON":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "산정특례 본인부담율";
                                    break;
                                case "DRUG_MIR":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "약제상한차액 부담율";
                                    break;
                                case "GAMEK_JIN":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "진찰료감액율";
                                    break;
                                case "GAM_OPD":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "외래감액율";
                                    break;
                                case "GAM_IPD":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "입원감액율";
                                    break;
                                case "GAM_BOHUM":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "보험감액율";
                                    break;
                                case "GAM_ILBAN":
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "일반감액율";
                                    break;
                                default:
                                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = "**ERROR**";
                                    break;
                            }
                            ssView2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["StartDate"].ToString().Trim();
                            strOldData = strNewData;
                        }
                        if (VB.Right(strOldData, 10) != VB.Right(strNewData, 10))
                        {
                            ssView2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["StartDate"].ToString().Trim();
                            strOldData = strNewData;
                        }
                        ssView2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["ArrayClass"].ToString().Trim();
                        ssView2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["RateValue"].ToString().Trim();
                        ssView2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["RateText"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }
                ssView2_Sheet1.RowCount = nRow;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboJob_Click(object sender, EventArgs e)
        {
            if (rdoUpdate.Checked == true) { ComboDate_SET(); }
        }

        private void cboJob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SendKeys.Send("{Tab}"); }
        }

        private void rdoInsert_Click(object sender, EventArgs e)
        {
            if (rdoInsert.Checked == true)
            {
                cboDate.Visible = false;
                dtpDate.Visible = true;
            }
            else
            {
                cboDate.Visible = true;
                dtpDate.Visible = false;
                ComboDate_SET();
            }
        }
  


        private void rdoUpdate_Click(object sender, EventArgs e)
        {
            if (rdoUpdate.Checked == true)
            {
                cboDate.Visible = true;
                dtpDate.Visible = false;
                ComboDate_SET();
            }
            else
            {
                cboDate.Visible = false;
                dtpDate.Visible = true;
            }
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { SendKeys.Send("{Tab}"); }
        }
    }
}
