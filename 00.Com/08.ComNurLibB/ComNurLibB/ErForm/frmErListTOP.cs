using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmErList.cs
    /// Description     : 응급실대장
    /// Author          : 박창욱
    /// Create Date     : 2018-03-20
    /// Update History  : 
    /// </summary>
    /// <history>    
    /// </history>
    /// <seealso cref= "\nurse\nrer\nrer11.frm(FrmErList.frm) >> frmErList.cs 폼이름 재정의" />
    public partial class frmErListTOP : Form, MainFormMessage
    {
        string FstrJob = "";
        string FstrPrtHead = "";
        string strSysDate = "";
        string FstrViewMode = "";
        bool FbViewOK = false;
        
        public frmErListTOP()
        {
            InitializeComponent();
        }

        public frmErListTOP(string strViewMode)
        {
            InitializeComponent();
            FstrViewMode = strViewMode;
        }

        public frmErListTOP(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

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
        #endregion //MainFormMessage


        private void btnExcel_Click(object sender, EventArgs e)
        {
            clsSpread CS = new clsSpread();
            CS.ExportToXLS(ssView);            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strGubun = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strSysDateTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            ssPrint_Sheet1.RowCount = 0;
            ssPrint_Sheet1.RowCount = ssView_Sheet1.RowCount;

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                for (int k = 1; k < ssView_Sheet1.ColumnCount; k++)
                {
                    ssPrint_Sheet1.Cells[i, k - 1].Text = ssView_Sheet1.Cells[i, k].Text;
                }

                ssPrint_Sheet1.SetRowHeight(i, (int)ssPrint_Sheet1.GetPreferredRowHeight(i) + 2);
            }

            if (chkCancle.Checked == true)
            {
                ssView_Sheet1.Columns[0].Visible = false;

                strTitle = "응  급  실    대  장";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("Ktas 최초전송후 취소", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDateTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("응급실장: ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                if (FstrPrtHead != "")
                {
                    strHeader += CS.setSpdPrint_String("인쇄방법 : " + FstrPrtHead, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.9f);

                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                if (VB.Left(cboOutGbn.Text, 1) == "0")
                {
                    strGubun = "";
                    if (FstrPrtHead == "")
                    {
                        ssPrint_Sheet1.Columns[12].Width = 128;
                        ssPrint_Sheet1.Columns[13].Width = 142;                        
                        ssPrint_Sheet1.Columns[14].Width = 80;
                        ssPrint_Sheet1.Columns[16].Visible = true;
                        ssPrint_Sheet1.Columns[17].Visible = true;
                        ssPrint_Sheet1.Columns[18].Visible = true;
                        ssPrint_Sheet1.Columns[19].Visible = true;
                    }
                }
                else
                {
                    strGubun = " ( " + VB.Pstr(cboOutGbn.Text, ".", 2) + " )";

                    if (FstrPrtHead == "")
                    {
                        ssPrint_Sheet1.Columns[12].Width = 128;
                        ssPrint_Sheet1.Columns[13].Width = 250;
                        ssPrint_Sheet1.Columns[14].Width = 250;
                        ssPrint_Sheet1.Columns[15].Visible = false;
                        ssPrint_Sheet1.Columns[16].Visible = false;
                        ssPrint_Sheet1.Columns[17].Visible = false;
                        ssPrint_Sheet1.Columns[18].Visible = false;
                    }
                }

                strTitle = "응  급  실    대  장" + strGubun;

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("내원사유 : " + cboSayu.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDateTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("응급실장: ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                if (FstrPrtHead != "")
                {
                    strHeader += CS.setSpdPrint_String("인쇄방법 : " + FstrPrtHead, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.8f);

                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

            }

            ssView_Sheet1.Columns[0].Visible = true;
        }

        void Spread_Click(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            //clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.ssView)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssView, e.Column, true);
                    return;
                }


            }
        }

        private void btnSend1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strBDATE = "";
            string strWardCode = "";
            string strDUTY = "";
            string strRoomCode = "";
            string strPtno = "";
            string strSName = "";
            string strDeptCode = "";
            string strAge = "";
            string strSex = "";
            string strDIAG = "";
            string strBigo = "";
            string strINPUTGubun = "";
            string strINTIME = "";

            strBDATE = dtpTDate.Value.ToString("yyyy-MM-dd").ToString().Trim();
            strWardCode = "ER";
            strRoomCode = "100";
            strINPUTGubun = cboIpwon.Text.Trim();

            if (rdoDay.Checked == true)
            {
                strDUTY = "Day";
            }
            else if (rdoEvening.Checked == true)
            {
                strDUTY = "Eve";
            }
            else if (rdoNight.Checked == true)
            {
                strDUTY = "Nit1";
            }
            else if (rdoNight2.Checked == true)
            {
                strDUTY = "Nit2";
            }

            if (strINPUTGubun == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.");
                return;
            }

            if (strDUTY == "")
            {
                ComFunc.MsgBox("DUTY가 선택되지 않았습니다.");
                return;
            }



            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strINTIME = ssView_Sheet1.Cells[i, 23].Text.Trim();
                        strPtno = ssView_Sheet1.Cells[i, 3].Text.Trim();
                        strSName = ssView_Sheet1.Cells[i, 4].Text.Trim();
                        strDeptCode = ssView_Sheet1.Cells[i, 5].Text.Trim();
                        strAge = VB.Mid(ssView_Sheet1.Cells[i, 7].Text.Trim(), 1, ssView_Sheet1.Cells[i, 7].Text.Trim().Length - 2);
                        strSex = VB.Right(ssView_Sheet1.Cells[i, 7].Text.Trim(), 1);
                        strRoomCode = ssView_Sheet1.Cells[i, 12].Text.Trim();
                        strDIAG = ssView_Sheet1.Cells[i, 14].Text.Trim();

                        SQL = "";
                        SQL = "SELECT * ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_24H_ETC ";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "      AND WARDCODE = '" + strWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND INPUTGUBUN = '" + strINPUTGubun + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + strPtno + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ComFunc.MsgBox(strSName + "해당 내역은 이미 등록되어 있습니다. (" + strPtno + ")" + ComNum.VBLF + "※구분 : " + strINPUTGubun);
                        }
                        else
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_24H_ETC (";
                            SQL = SQL + ComNum.VBLF + " BDATE, WARDCODE, GUBUN, DUTY, ";
                            SQL = SQL + ComNum.VBLF + " ROOMCODE, PTNO, SNAME, DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + " AGE, SEX, DIAG, BIGO, ";
                            SQL = SQL + ComNum.VBLF + " INPUTGUBUN, INTIME) VALUES (";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strWardCode + "','입원','" + strDUTY + "', ";
                            SQL = SQL + ComNum.VBLF + "'" + strRoomCode + "','" + strPtno + "','" + strSName + "','" + strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "'" + strAge + "','" + strSex + "','" + strDIAG + "','" + strBigo + "',";
                            SQL = SQL + ComNum.VBLF + "'" + strINPUTGubun + "', TO_DATE('" + strINTIME + "','YYYY-MM-DD HH24:MI'))";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                if (dt != null)
                                {
                                    dt.Dispose();
                                    dt = null;
                                }
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (strINPUTGubun == "수술/시술 후 입원")
                            {
                                SQL = "";
                                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_24H_ETC (";
                                SQL = SQL + ComNum.VBLF + " BDATE, WARDCODE, GUBUN, DUTY, ";
                                SQL = SQL + ComNum.VBLF + " ROOMCODE, PTNO, SNAME, DEPTCODE, ";
                                SQL = SQL + ComNum.VBLF + " AGE, SEX, DIAG, BIGO, ";
                                SQL = SQL + ComNum.VBLF + " INPUTGUBUN, INTIME) VALUES (";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strWardCode + "','수술','" + strDUTY + "', ";
                                SQL = SQL + ComNum.VBLF + "'" + strRoomCode + "','" + strPtno + "','" + strSName + "','" + strDeptCode + "', ";
                                SQL = SQL + ComNum.VBLF + "'" + strAge + "','" + strSex + "','" + strDIAG + "','" + strBigo + "',";
                                SQL = SQL + ComNum.VBLF + "'" + strINPUTGubun + "', TO_DATE('" + strINTIME + "','YYYY-MM-DD HH24:MI'))";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    if (dt != null)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Value = false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("전송하였습니다.");
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

        private void btnSend2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strBDATE = "";
            string strWardCode = "";
            string strDUTY = "";
            string strRoomCode = "";
            string strPtno = "";
            string strSName = "";
            string strDeptCode = "";
            string strAge = "";
            string strSex = "";
            string strDIAG = "";
            string strBigo = "";
            string strINPUTGubun = "";
            string strINTIME = "";

            strBDATE = dtpTDate.Value.ToString("yyyy-MM-dd").ToString().Trim();
            strWardCode = "ER";
            strRoomCode = "100";
            strINPUTGubun = cboTewon.Text.Trim();

            if (rdoDay.Checked == true)
            {
                strDUTY = "Day";
            }
            else if (rdoEvening.Checked == true)
            {
                strDUTY = "Eve";
            }
            else if (rdoNight.Checked == true)
            {
                strDUTY = "Nit1";
            }
            else if (rdoNight2.Checked == true)
            {
                strDUTY = "Nit2";
            }

            if (strINPUTGubun == "")
            {
                ComFunc.MsgBox("구분이 공란입니다.");
                return;
            }

            if (strDUTY == "")
            {
                ComFunc.MsgBox("DUTY가 선택되지 않았습니다.");
                return;
            }



            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strINTIME = ssView_Sheet1.Cells[i, 23].Text.Trim();
                        strPtno = ssView_Sheet1.Cells[i, 3].Text.Trim();
                        strSName = ssView_Sheet1.Cells[i, 4].Text.Trim();
                        strDeptCode = ssView_Sheet1.Cells[i, 5].Text.Trim();
                        strAge = VB.Mid(ssView_Sheet1.Cells[i, 7].Text.Trim(), 1, ssView_Sheet1.Cells[i, 7].Text.Trim().Length - 2);
                        strSex = VB.Right(ssView_Sheet1.Cells[i, 7].Text.Trim(), 1);
                        strRoomCode = ssView_Sheet1.Cells[i, 12].Text.Trim();
                        strDIAG = ssView_Sheet1.Cells[i, 14].Text.Trim();

                        SQL = "";
                        SQL = "SELECT * ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_24H_ETC ";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "      AND WARDCODE = '" + strWardCode + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND INPUTGUBUN = '" + strINPUTGubun + "' ";
                        SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + strPtno + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ComFunc.MsgBox(strSName + "해당 내역은 이미 등록되어 있습니다. (" + strPtno + ")" + ComNum.VBLF + "※구분 : " + strINPUTGubun);
                        }
                        else
                        {
                            SQL = "";
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_24H_ETC (";
                            SQL = SQL + ComNum.VBLF + " BDATE, WARDCODE, GUBUN, DUTY, ";
                            SQL = SQL + ComNum.VBLF + " ROOMCODE, PTNO, SNAME, DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + " AGE, SEX, DIAG, BIGO, ";
                            SQL = SQL + ComNum.VBLF + " INPUTGUBUN, INTIME) VALUES (";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strWardCode + "','퇴원','" + strDUTY + "', ";
                            SQL = SQL + ComNum.VBLF + "'" + strRoomCode + "','" + strPtno + "','" + strSName + "','" + strDeptCode + "', ";
                            SQL = SQL + ComNum.VBLF + "'" + strAge + "','" + strSex + "','" + strDIAG + "','" + strBigo + "',";
                            SQL = SQL + ComNum.VBLF + "'" + strINPUTGubun + "', TO_DATE('" + strINTIME + "','YYYY-MM-DD HH24:MI'))";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                if (dt != null)
                                {
                                    dt.Dispose();
                                    dt = null;
                                }
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox(SqlErr);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            if (strINPUTGubun == "사망")
                            {
                                SQL = "";
                                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_24H_ETC (";
                                SQL = SQL + ComNum.VBLF + " BDATE, WARDCODE, GUBUN, DUTY, ";
                                SQL = SQL + ComNum.VBLF + " ROOMCODE, PTNO, SNAME, DEPTCODE, ";
                                SQL = SQL + ComNum.VBLF + " AGE, SEX, DIAG, BIGO, ";
                                SQL = SQL + ComNum.VBLF + " INPUTGUBUN, INTIME) VALUES (";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strWardCode + "','사망','" + strDUTY + "', ";
                                SQL = SQL + ComNum.VBLF + "'" + strRoomCode + "','" + strPtno + "','" + strSName + "','" + strDeptCode + "', ";
                                SQL = SQL + ComNum.VBLF + "'" + strAge + "','" + strSex + "','" + strDIAG + "','" + strBigo + "',";
                                SQL = SQL + ComNum.VBLF + "'" + strINPUTGubun + "', TO_DATE('" + strINTIME + "','YYYY-MM-DD HH24:MI'))";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    if (dt != null)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox(SqlErr);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Value = false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("전송하였습니다.");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }


            int j = 0;
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SQL1 = "";    //Query문
            
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            string strDate = "";
            string strDate1 = "";
            string strINTIME = "";
            string strOutTime = "";
            string strOutGbn = "";

            ComFunc cf = new ComFunc();

            FbViewOK = true;

            ssView_Sheet1.RowCount = 0;

            strDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strDate1 = dtpTDate.Value.ToString("yyyy-MM-dd");

            nRow = 0;

            strOutGbn = VB.Left(cboOutGbn.Text, 1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                do
                {
                    //1일분의 자료2를 Dispay
                    #region View_One_Day

                    string strDate_1 = "";
                    string strDate_5 = "";
                    string strDispDate = "";

                    strDate_1 = cf.DATE_ADD(clsDB.DbCon, strDate, -1);
                    strDate_5 = cf.DATE_ADD(clsDB.DbCon, strDate, -5);
                    strDispDate = VB.Val(VB.Mid(strDate, 6, 2)).ToString("#0") + "/" + VB.Val(VB.Right(strDate, 2)).ToString("#0");
                    cboDept.Text = cboDept.Text.Trim();

                    if (chkCancle.Checked == true || chk_time.Checked == true)
                    {

                    }
                    else
                    {
                        //Keep환자의 자료를 Display
                        SQL = "";
                        SQL = "SELECT TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime,a.Pano,b.SName,";
                        SQL = SQL + ComNum.VBLF + " a.DeptCode,a.Age,a.Sex,a.WardCode,a.Grade,a.SinGu,";
                        SQL = SQL + ComNum.VBLF + " a.Singu,a.Bi,a.Room,a.WardCode,a.Study,a.Disease,a.OutGbn,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime,";
                        SQL = SQL + ComNum.VBLF + " a.Nurse,a.Juso,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate,";
                        SQL = SQL + ComNum.VBLF + " A.HODRNAME1, HODEPT1,HODEPT2,HODEPT3,HODEPT4,HODEPT5,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(A.HOTIME1,'HH24:MI') HOTIME1,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(A.HODATE1,'HH24:MI') HODATE1,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(a.DrPDate1,'HH24:MI') DrPDate1, B.ZIPCODE1, A.KTASLEVL, B.JICODE";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
                        SQL = SQL + ComNum.VBLF + "WHERE a.JDate>=TO_DATE('" + strDate_5 + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  AND a.JDate<TO_DATE('" + strDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  AND (a.OutTime IS NULL OR";
                        SQL = SQL + ComNum.VBLF + "      a.OutTime > TO_DATE('" + strDate_1 + " 23:59','YYYY-MM-DD HH24:MI'))";
                        SQL = SQL + ComNum.VBLF + "  AND (A.DGKD IS NULL OR A.DGKD NOT IN ('4'))";

                        if (strOutGbn != "0")
                        {
                            SQL = SQL + ComNum.VBLF + " AND A.OUTGBN = '" + strOutGbn + "'";
                        }
                        if (cboDept2.Text != "전체")
                        {
                            SQL = SQL + ComNum.VBLF + "AND  ( a.HODEPT1='" + cboDept2.Text + "' OR a.HODEPT2='" + cboDept2.Text + "' OR a.HODEPT3='" + cboDept2.Text + "' OR a.HODEPT4='" + cboDept2.Text + "' OR a.HODEPT5='" + cboDept2.Text + "') ";
                        }
                        if (cboDept.Text != "전체")
                        {
                            SQL = SQL + ComNum.VBLF + " AND a.DeptCode='" + cboDept.Text + "' ";
                        }
                        if (cboWard.Text != "전체")
                        {
                            SQL = SQL + ComNum.VBLF + " AND a.WardCode='" + cboWard.Text + "' ";
                        }
                        if (chk_NGT.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " and  (A.HODATE1 < decode(TO_CHAR(A.HODATE1,'D'),'1',trunc(A.HODATE1 + 1 ),trunc(A.HODATE1) + 08/24 ) ";
                            SQL = SQL + ComNum.VBLF + "     or  A.HODATE1 > decode(TO_CHAR(A.HODATE1,'D'),'7',trunc(A.HODATE1)+ 13/24,'1',trunc(A.HODATE1),trunc(A.HODATE1)+ 18/24 ))";
                        }
                        
                        SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+)";

                        if (VB.Left(cboSayu.Text, 1) != "**")
                        {
                            switch (VB.Left(cboSayu.Text, 1).Trim())
                            {
                                case "1":
                                    SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "' AND PTMIDGKD ='1' )";
                                    break;
                                case "2":
                                    SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='2' )";
                                    break;
                                case "3":
                                    SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "'   AND PTMIDGKD ='3' )";
                                    break;
                            }

                            if (cboArCf.Text.Trim() != "")//EMIHPTMI.PTMIARCF  VB.Left(cboArCf.Text, 1).Trim()
                            {
                                SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "' AND PTMIARCF ='"+ VB.Left(cboArCf.Text.Trim(), 1) +"' ) ";
                            }
                            if (cboArCs.Text.Trim() != "")//EMIHPTMI.PTMIARCS
                            {
                                SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "' AND PTMIARCS ='" + VB.Left(cboArCs.Text.Trim(), 2) + "' ) ";
                            }

                        }
                        SQL = SQL + ComNum.VBLF + "  AND NOT (A.DGKD = '3' AND OUTTIME IS NULL) ";

                        SQL = SQL + ComNum.VBLF + "ORDER BY a.InTime";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                #region View_OneLine_Display

                                strOutTime = dt.Rows[i]["OutTime"].ToString().Trim();
                                strINTIME = dt.Rows[i]["InTime"].ToString().Trim();
                                nRow++;
                                if (nRow > ssView_Sheet1.RowCount)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }

                                ssView_Sheet1.Cells[nRow - 1, 0].Value = false;

                                if (string.Compare(VB.Left(strINTIME, 10), strDate) < 0)
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Mid(strINTIME, 6, 2) + "/" + VB.Mid(strINTIME, 9, 2);
                                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "Kp" + " " + VB.Right(strINTIME, 5);
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 1].Text = strDispDate;
                                    ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Right(dt.Rows[i]["InTime"].ToString().Trim(), 5);
                                }

                                ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["InTime"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = Read_JiName(dt.Rows[i]["JICODE"].ToString().Trim());
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["SinGu"].ToString().Trim() == "1" ? "신환" : "구환";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "51":
                                    case "53":
                                    case "54":
                                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "일반";
                                        break;
                                    case "52":
                                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "교통";
                                        break;
                                    case "31":
                                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "산재";
                                        break;
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                    case "25":
                                    case "26":
                                    case "27":
                                    case "28":
                                    case "29":
                                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "보호";
                                        break;
                                    default:
                                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "보험";
                                        break;
                                }

                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["WardCode"].ToString().Trim();

                                if (VB.Val(dt.Rows[i]["Room"].ToString().Trim()) > 0)
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["Room"].ToString().Trim();
                                }

                                ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["Study"].ToString().Trim();

                                if (chkCancle.Checked == true)
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 14].Text = Read_Motive(dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(strINTIME, 10), VB.Right(strINTIME, 5), dt.Rows[i]["Disease"].ToString().Trim());
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 14].Text = Read_FinalDiagnosis(dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(strINTIME, 10), VB.Right(strINTIME, 5), dt.Rows[i]["Disease"].ToString().Trim());
                                }

                                if (strOutTime == "" || string.Compare(VB.Left(strOutTime, 10), strDate) > 0 && FstrJob == "")
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "Keep";
                                }
                                else
                                {
                                    switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                                    {
                                        case "1":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "입원";
                                            break;
                                        case "2":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "귀가";
                                            break;
                                        case "3":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "DOA";
                                            break;
                                        case "4":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "사망";
                                            break;
                                        case "5":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "취소";
                                            break;
                                        case "6":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "이송";
                                            break;
                                        case "7":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "DAMA";
                                            break;
                                        case "8":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "OPD";
                                            break;
                                        case "9":
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "OR입원";
                                            break;
                                        default:
                                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "**";
                                            break;
                                    }

                                    ssView_Sheet1.Cells[nRow - 1, 19].Text = VB.Right(strOutTime, 5);
                                }

                                ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["DrPDate1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["HOTIME1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["HODATE1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["HODRNAME1"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["Nurse"].ToString().Trim().Replace(" ", "");
                                ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["JDate"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 24].Text = dt.Rows[i]["HODEPT2"].ToString().Trim() + " " + dt.Rows[i]["HODEPT3"].ToString().Trim() + " " + dt.Rows[i]["HODEPT4"].ToString().Trim() + " " + dt.Rows[i]["HODEPT5"].ToString().Trim();

                                SQL1 = "";
                                SQL1 = "SELECT GBINFO";
                                SQL1 = SQL1 + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_IORDER ";
                                SQL1 = SQL1 + ComNum.VBLF + " WHERE PTNO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                                SQL1 = SQL1 + ComNum.VBLF + " AND BDATE BETWEEN TO_DATE('" + VB.Left(strINTIME, 10) + "','YYYY-MM-DD') ";
                                SQL1 = SQL1 + ComNum.VBLF + " AND TO_DATE('" + VB.Left(strOutTime, 10) + "','YYYY-MM-DD') ";
                                SQL1 = SQL1 + ComNum.VBLF + " AND SUCODE ='$$39' ";
                                SQL1 = SQL1 + ComNum.VBLF + " AND GBSTATUS =' ' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL1, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL1, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    for (j = 0; j < dt1.Rows.Count; j++)
                                    {
                                        ssView_Sheet1.Cells[nRow - 1, 25].Text += dt1.Rows[j]["GBINFO"].ToString().Trim() + ComNum.VBLF;

                                    }


                                }

                                dt1.Dispose();
                                dt1 = null;
                                ssView_Sheet1.SetRowHeight(nRow - 1, (int)ssView_Sheet1.GetPreferredRowHeight(nRow - 1) + 2);

                                #endregion
                            }
                        }


                        dt.Dispose();
                        dt = null;
                    }

                    //당일 내원환자 Display
                    SQL = "";
                    SQL = "SELECT TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime,a.Pano,b.SName,a.DeptCode,a.Age,a.Sex,a.WardCode,a.Grade, ";
                    SQL = SQL + ComNum.VBLF + " a.Singu,a.Bi,a.Room,a.Study,a.Disease,a.OutGbn,TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime,";
                    SQL = SQL + ComNum.VBLF + " a.Nurse,a.Juso,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate, ";
                    SQL = SQL + ComNum.VBLF + " A.HODRNAME1, HODEPT1,HODEPT2,HODEPT3,HODEPT4,HODEPT5, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(A.HOTIME1,'HH24:MI') HOTIME1, TO_CHAR(A.HODATE1,'HH24:MI') HODATE1, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(a.DrPDate1,'HH24:MI') DrPDate1, B.ZIPCODE1, A.KTASLEVL, B.JICODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "BAS_PATIENT b ";

                    if (chk_time.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE a.InTime between TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + txt1.Text + "','YYYY-MM-DDHH24MI') ";
                        SQL = SQL + ComNum.VBLF + " and  TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + txt2.Text + "','YYYY-MM-DDHH24MI') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE a.JDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    }

                    if (chkCancle.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.DGKD = '4'";
                        ssView_Sheet1.Columns[5].Visible = false;
                        ssView_Sheet1.Columns[11].Visible = false;
                        ssView_Sheet1.Columns[12].Visible = false;
                        ssView_Sheet1.Columns[13].Visible = false;
                        ssView_Sheet1.Columns[14].Width = 600;
                        ssView_Sheet1.Columns[15].Visible = false;
                        ssView_Sheet1.Columns[16].Visible = false;
                        ssView_Sheet1.Columns[17].Visible = false;
                        ssView_Sheet1.Columns[18].Visible = false;
                        ssView_Sheet1.Columns[19].Visible = false;
                        ssView_Sheet1.Columns[21].Visible = false;
                        ssView_Sheet1.Columns[24].Visible = false;
                    }
                    else
                    {
                        ssView_Sheet1.Columns[5].Visible = true;
                        ssView_Sheet1.Columns[11].Visible = true;
                        ssView_Sheet1.Columns[12].Visible = true;
                        ssView_Sheet1.Columns[13].Visible = true;
                        ssView_Sheet1.Columns[14].Width = 133;
                        ssView_Sheet1.Columns[15].Visible = true;
                        ssView_Sheet1.Columns[16].Visible = true;
                        ssView_Sheet1.Columns[17].Visible = true;
                        ssView_Sheet1.Columns[18].Visible = true;
                        ssView_Sheet1.Columns[19].Visible = true;
                        ssView_Sheet1.Columns[21].Visible = true;
                        ssView_Sheet1.Columns[24].Visible = true;

                        SQL = SQL + ComNum.VBLF + "  AND (A.DGKD IS NULL OR A.DGKD NOT IN ('4'))";
                    }

                    if (strOutGbn != "0")
                    {
                        SQL = SQL + " AND A.OUTGBN = '" + strOutGbn + "' ";
                    }

                    if (cboDept2.Text != "전체")
                    {
                        SQL = SQL + "AND  ( a.HODEPT1='" + cboDept2.Text + "' OR a.HODEPT2='" + cboDept2.Text + "' OR a.HODEPT3='" + cboDept2.Text + "' OR a.HODEPT4='" + cboDept2.Text + "' OR a.HODEPT5='" + cboDept2.Text + "') ";
                    }
                    if (cboDept.Text != "전체")
                    {
                        SQL = SQL + " AND a.DeptCode='" + cboDept.Text + "' ";
                    }
                    if (cboWard.Text != "전체")
                    {
                        SQL = SQL + " AND a.WardCode='" + cboWard.Text + "' ";
                    }
                    if (chk_NGT.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " and  (A.HODATE1 < decode(TO_CHAR(A.HODATE1,'D'),'1',trunc(A.HODATE1 + 1 ),trunc(A.HODATE1) + 08/24 ) ";
                        SQL = SQL + ComNum.VBLF + "     or  A.HODATE1 > decode(TO_CHAR(A.HODATE1,'D'),'7',trunc(A.HODATE1)+ 13/24,'1',trunc(A.HODATE1),trunc(A.HODATE1)+ 18/24 ))";
                    }

                        SQL = SQL + "  AND a.Pano=b.Pano(+) ";

                    if (VB.Left(cboSayu.Text, 1) != "**")
                    {
                        switch (VB.Left(cboSayu.Text, 1).Trim())
                        {
                            case "1":
                                SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( ";
                                SQL = SQL + ComNum.VBLF + " SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='1' AND SEQNO = ";
                                break;
                            case "2":
                                SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='2' )";
                                break;
                            case "3":
                                SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='3' )";
                                break;
                        }
                        if (cboArCf.Text.Trim() != "")//EMIHPTMI.PTMIARCF  VB.Left(cboArCf.Text, 1).Trim()
                        {
                            SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "' AND PTMIARCF ='" + VB.Left(cboArCf.Text.Trim(), 1) + "' ) ";
                        }
                        if (cboArCs.Text.Trim() != "")//EMIHPTMI.PTMIARCS
                        {
                            SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT >= '" + strDate_5.Replace("-", "") + "' AND PTMIINDT <= '" + strDate.Replace("-", "") + "' AND PTMIARCS ='" + VB.Left(cboArCs.Text.Trim(), 2) + "' ) ";
                        }
                    }
                    //SELECT to_char(jobdate,'DY') FROM BAS_JOB b where


                    SQL = SQL + ComNum.VBLF + "ORDER BY a.InTime ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        #region View_OneLine_Display

                        strOutTime = dt.Rows[i]["OutTime"].ToString().Trim();
                        strINTIME = dt.Rows[i]["InTime"].ToString().Trim();
                        nRow++;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 0].Value = false;

                        if (string.Compare(VB.Left(strINTIME, 10), strDate) < 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = VB.Mid(strINTIME, 6, 2) + "/" + VB.Mid(strINTIME, 9, 2);
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "Kp" + " " + VB.Right(strINTIME, 5);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = strDispDate;
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = VB.Right(dt.Rows[i]["InTime"].ToString().Trim(), 5);
                        }

                        ssView_Sheet1.Cells[nRow - 1, 23].Text = dt.Rows[i]["InTime"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = Read_JiName(dt.Rows[i]["JICODE"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["SinGu"].ToString().Trim() == "1" ? "신환" : "구환";

                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "51":
                            case "53":
                            case "54":
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "일반";
                                break;
                            case "52":
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "교통";
                                break;
                            case "31":
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "산재";
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "보호";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "보험";
                                break;
                        }

                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["WardCode"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["Room"].ToString().Trim()) > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["Room"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["Study"].ToString().Trim();

                        if (chkCancle.Checked == true)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = Read_Motive(dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(strINTIME, 10), VB.Right(strINTIME, 5), dt.Rows[i]["Disease"].ToString().Trim());
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = Read_FinalDiagnosis(dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(strINTIME, 10), VB.Right(strINTIME, 5), dt.Rows[i]["Disease"].ToString().Trim());
                        }

                        if (strOutTime == "" || string.Compare(VB.Left(strOutTime, 10), strDate) > 0 && FstrJob == "")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = "Keep";
                        }
                        else
                        {
                            switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                            {
                                case "1":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "입원";
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "귀가";
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "DOA";
                                    break;
                                case "4":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "사망";
                                    break;
                                case "5":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "취소";
                                    break;
                                case "6":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "이송";
                                    break;
                                case "7":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "DAMA";
                                    break;
                                case "8":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "OPD";
                                    break;
                                case "9":
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "OR입원";
                                    break;
                                default:
                                    ssView_Sheet1.Cells[nRow - 1, 15].Text = "**";
                                    break;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 19].Text = VB.Right(strOutTime, 5);
                        }

                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["DrPDate1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["HOTIME1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["HODATE1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["HODRNAME1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["Nurse"].ToString().Trim().Replace(" ", "");
                        ssView_Sheet1.Cells[nRow - 1, 22].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 24].Text = dt.Rows[i]["HODEPT2"].ToString().Trim() + " " + dt.Rows[i]["HODEPT3"].ToString().Trim() + " " + dt.Rows[i]["HODEPT4"].ToString().Trim() + " " + dt.Rows[i]["HODEPT5"].ToString().Trim();

                        SQL1 = "";
                        SQL1 = "SELECT GBINFO";
                        SQL1 = SQL1 + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_IORDER ";
                        SQL1 = SQL1 + ComNum.VBLF + " WHERE PTNO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL1 = SQL1 + ComNum.VBLF + " AND BDATE BETWEEN TO_DATE('" + VB.Left(strINTIME, 10) + "','YYYY-MM-DD') ";
                        SQL1 = SQL1 + ComNum.VBLF + " AND TO_DATE('" + VB.Left(strOutTime, 10) + "','YYYY-MM-DD') ";
                        SQL1 = SQL1 + ComNum.VBLF + " AND SUCODE ='$$39' ";
                        SQL1 = SQL1 + ComNum.VBLF + " AND GBSTATUS =' ' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL1, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL1, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (j = 0; j < dt1.Rows.Count; j++)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 25].Text += dt1.Rows[j]["GBINFO"].ToString().Trim() + ComNum.VBLF;
                               
                            }

                           
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //ssView_Sheet1.Cells[nRow - 1, 24].Text = dt.Rows[i]["HODEPT2"].ToString().Trim() + " " + dt.Rows[i]["HODEPT3"].ToString().Trim() + " " + dt.Rows[i]["HODEPT4"].ToString().Trim() + " " + dt.Rows[i]["HODEPT5"].ToString().Trim();

                        ssView_Sheet1.SetRowHeight(nRow - 1, (int)ssView_Sheet1.GetPreferredRowHeight(nRow - 1) + 2);

                        #endregion
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion

                    strDate = cf.DATE_ADD(clsDB.DbCon, strDate, 1);

                } while (string.Compare(strDate, dtpTDate.Value.ToShortDateString()) <= 0);

                ssView_Sheet1.RowCount = nRow;
                FbViewOK = false;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ViewFormMonitor2()
        {
            Screen[] screens = Screen.AllScreens;
            Screen secondary_Screen = null;

            if (screens.Length == 1)
            {
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                foreach (Screen screen in screens)
                {
                    if (screen.Primary == false)
                    {
                        secondary_Screen = screen;
                        this.Bounds = secondary_Screen.Bounds;
                        this.Show();
                        this.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }

        private void frmErList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            // 
            if (FstrViewMode != "")
            {
                ViewFormMonitor2();
            }
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string GstrJob = "";
            int GnMinIlsu = 0;
            int GnMaxIlsu = 0;
            string GstrCaption = "";

            ssView.Dock = DockStyle.Fill;

            //ssView_Sheet1.Columns[13].Width = 40;
            //ssView_Sheet1.Columns[14].Width = 40;

            //ssPrint_Sheet1.Columns[12].Width = 40;
            //ssPrint_Sheet1.Columns[13].Width = 40;

            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            dtpFDate.Value = Convert.ToDateTime(strSysDate);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            FbViewOK = true;

            cboOutGbn.Items.Clear();
            cboOutGbn.Items.Add("0.전체");
            cboOutGbn.Items.Add("1.입원");
            cboOutGbn.Items.Add("2.귀가");
            cboOutGbn.Items.Add("3.DOA");             //사망후 응급실도착
            cboOutGbn.Items.Add("4.사망");             //응급실에서 사망
            cboOutGbn.Items.Add("5.취소");
            cboOutGbn.Items.Add("6.이송");
            cboOutGbn.Items.Add("7.DAMA");
            cboOutGbn.Items.Add("8.OPD");
            cboOutGbn.Items.Add("9.수술후입원(응급수술)");
            cboOutGbn.SelectedIndex = 0;

            cboIpwon.Items.Clear();
            cboIpwon.Items.Add("단순 입원");
            cboIpwon.Items.Add("수술/시술 후 입원");
            cboIpwon.SelectedIndex = -1;

            cboTewon.Items.Clear();
            cboTewon.Items.Add("전원");
            cboTewon.Items.Add("DAMA");
            cboTewon.Items.Add("탈원");
            cboTewon.Items.Add("사망");
            cboTewon.Items.Add("기타");
            cboTewon.SelectedIndex = -1;

            grb24Hour.Text = "24시간 보고서 ( " + dtpTDate.Value.ToString("yyyy-MM-dd") + " )";

            cboSayu.Items.Clear();
            cboSayu.Items.Add("*.전체");
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboSayu, "EMI_내원사유(질병여부)", 1, false, "N");
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboArCf, "EMI_내원사유(의도성여부)", 1, true, "");
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboArCs, "EMI_내원사유(손상기전)", 1, true, "");
            cboSayu.SelectedIndex = 0;

            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            cboDept2.Items.Clear();
            cboDept2.Items.Add("전체");

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            cboWard.Items.Add("33");
            //cboWard.Items.Add("40");
            cboWard.Items.Add("65");
            cboWard.Items.Add("83");

            cboWard.SelectedIndex = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //진료과를 READ
                SQL = "";
                SQL = "SELECT DeptCode";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II','R6','PT','OC','HC') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking,DeptCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDept2.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }

                    cboDept.SelectedIndex = 0;
                    cboDept2.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                FstrJob = "";
                FstrPrtHead = "";

                
                if (clsPublic.GstrHelpCode != "")
                {
                    GstrJob = VB.Pstr(clsPublic.GstrHelpCode, "{@}", 1);
                    GnMinIlsu = (int)VB.Val(VB.Pstr(clsPublic.GstrHelpCode, "{@}", 2)); //일수 기간시작
                    GnMaxIlsu = (int)VB.Val(VB.Pstr(clsPublic.GstrHelpCode, "{@}", 3)); //일수 기간종료
                    GstrCaption = VB.Pstr(clsPublic.GstrHelpCode, "{@}", 4);    //서비스 명칭
                    lblTitle.Text = GstrCaption;

                    if (GstrCaption == "2.4.2 타병원 이송서비스")
                    {
                        //dtpFDate.Value = Convert.ToDateTime("2008-03-01");
                        //dtpTDate.Value = Convert.ToDateTime("2008-08-31");

                        //의사일경우 시간 안 보임(응급실, 기록사는 보임)
                        ssView_Sheet1.Columns[15].Visible = false;
                        ssView_Sheet1.Columns[16].Visible = false;
                        ssView_Sheet1.Columns[17].Visible = false;
                        ssView_Sheet1.Columns[18].Visible = false;

                        //ssView_Sheet1.Columns[12].Width = 20;
                        //ssView_Sheet1.Columns[13].Width = 40;

                        ssPrint_Sheet1.Columns[14].Visible = false;
                        ssPrint_Sheet1.Columns[15].Visible = false;
                        ssPrint_Sheet1.Columns[16].Visible = false;
                        ssPrint_Sheet1.Columns[17].Visible = false;

                        //ssPrint_Sheet1.Columns[11].Width = 20;
                        //ssPrint_Sheet1.Columns[12].Width = 40;

                        cboOutGbn.SelectedIndex = 6;
                        Search_Data();
                    }
                    else if (GstrCaption == "2.4.3 응급환자 응급실 체류시간")
                    {
                        //dtpFDate.Value = Convert.ToDateTime("2008-06-01");
                        //dtpTDate.Value = Convert.ToDateTime("2008-08-31");

                        //의사일경우 시간 안 보임(응급실, 기록사는 보임)
                        ssView_Sheet1.Columns[15].Visible = false;
                        ssView_Sheet1.Columns[16].Visible = false;
                        ssView_Sheet1.Columns[17].Visible = false;
                        ssView_Sheet1.Columns[18].Visible = false;

                        //ssView_Sheet1.Columns[12].Width = 20;
                        //ssView_Sheet1.Columns[13].Width = 40;

                        ssPrint_Sheet1.Columns[14].Visible = false;
                        ssPrint_Sheet1.Columns[15].Visible = false;
                        ssPrint_Sheet1.Columns[16].Visible = false;
                        ssPrint_Sheet1.Columns[17].Visible = false;

                        //ssPrint_Sheet1.Columns[11].Width = 20;
                        //ssPrint_Sheet1.Columns[12].Width = 40;

                        switch (GstrJob)
                        {
                            case "01":
                                cboOutGbn.SelectedIndex = 2;
                                break;
                            case "02":
                                cboOutGbn.SelectedIndex = 6;
                                break;
                            case "03":
                                cboOutGbn.SelectedIndex = 1;
                                break;
                            case "04":
                                cboOutGbn.SelectedIndex = 9;
                                break;
                        }

                        Search_Data();
                    }
                    else
                    {
                        //dtpFDate.Value = Convert.ToDateTime("2008-03-01");
                        //dtpTDate.Value = Convert.ToDateTime("2008-08-31");

                        //의사일경우 시간 안 보임(응급실, 기록사는 보임)
                        ssView_Sheet1.Columns[15].Visible = false;
                        ssView_Sheet1.Columns[16].Visible = false;
                        ssView_Sheet1.Columns[17].Visible = false;
                        ssView_Sheet1.Columns[18].Visible = false;

                        //ssView_Sheet1.Columns[12].Width = 20;
                        //ssView_Sheet1.Columns[13].Width = 40;

                        ssPrint_Sheet1.Columns[14].Visible = false;
                        ssPrint_Sheet1.Columns[15].Visible = false;
                        ssPrint_Sheet1.Columns[16].Visible = false;
                        ssPrint_Sheet1.Columns[17].Visible = false;

                        //ssPrint_Sheet1.Columns[11].Width = 20;
                        //ssPrint_Sheet1.Columns[12].Width = 40;
                    }
                }

                clsPublic.GstrHelpCode = "";
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;

                //의료기관평가 관련
                if (clsPublic.GstrHelpCode != "")
                {
                    ssView_Sheet1.ColumnHeader.Cells[0, 7].Text = "EMR";
                    dtpFDate.Value = Convert.ToDateTime(VB.Pstr(clsPublic.GstrHelpCode, "{}", 1));
                    dtpTDate.Value = Convert.ToDateTime(VB.Pstr(clsPublic.GstrHelpCode, "{}", 2));
                    FstrJob = VB.Pstr(clsPublic.GstrHelpCode, "{}", 3);
                    lblTitle.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 4);
                    FstrPrtHead = VB.Pstr(clsPublic.GstrHelpCode, "{}", 4);
                    cboOutGbn.SelectedIndex = (int)VB.Val(FstrJob);
                    dtpFDate.Enabled = false;
                    dtpTDate.Enabled = false;
                    cboOutGbn.Enabled = false;
                    clsPublic.GstrHelpCode = "";
                    Search_Data();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int nCnt = 0;

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (FbViewOK == true)
            {
                return;
            }

            if (e.Column != 0)
            {
                return;
            }

            if (Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, e.Column].Value) == true)
            {
                ssView_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(128, 128, 255);
            }
            else
            {
                ssView_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
            }

            nCnt = 0;
            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    nCnt++;
                }
            }

            lblCnt.Text = nCnt + " 건";
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string strPano = "";

            if (e.Column == 8)
            {
                if (ssView_Sheet1.Cells[e.Row, e.Column].Text == "▦")
                {
                    strPano = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
                    clsVbEmr.ExecuteEmr(clsDB.DbCon, strPano);
                }
                return;
            }

            if (FstrPrtHead != "")
            {
                return;
            }

            
            clsPublic.GstrHelpCode = ssView_Sheet1.Cells[e.Row, 3].Text.Trim() + "@@";
            clsPublic.GstrHelpCode += ssView_Sheet1.Cells[e.Row, 2].Text.Trim() + "@@";
            clsPublic.GstrHelpCode += ssView_Sheet1.Cells[e.Row, 20].Text.Trim() + "@@";

            Search_Data();
        }

        private void dtpTDate_ValueChanged(object sender, EventArgs e)
        {
            grb24Hour.Text = "24시간 보고서 ( " + dtpTDate.Value.ToString("yyyy-MM-dd") + " )";
        }

        string Read_JiName(string argVar)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.JICODE, A.GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '포항' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE <= '64'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '경주' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE >= '71' AND JICODE <= '76'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '영천' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '77'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '영덕' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '78'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '울진' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '79'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '그외' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE >= '80') A";
                SQL = SQL + ComNum.VBLF + "  Where a.JICODE = '" + argVar + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["Gubun"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        string Read_FinalDiagnosis(string argPano, string argINDT, string argINTM, string argDisease)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            rtnVar = argDisease;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CHARTDATE, CHARTTIME, decode(FORMNO,'2605',EXTRACTVALUE(CHARTXML, '//ta7'),EXTRACTVALUE(CHARTXML, '//ta4'))  Disease ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " AND writetime >= '" + argINTM.Trim().Replace(":", "") + "00'";
                SQL = SQL + ComNum.VBLF + " AND MEDFRDATE = '" + argINDT.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " AND FORMNO in ('2074','2224','2276','2605')";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT  CHARTDATE, CHARTTIME,  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000014279') AS Disease ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND writetime >= '" + argINTM.Trim().Replace(":", "") + "00'";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + argINDT.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 2605";
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE, CHARTTIME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                if (dt.Rows[0]["Disease"].ToString().Trim() != "")
                {
                    rtnVar = dt.Rows[0]["Disease"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        string Read_Motive(string argPano, string argINDT, string argINTM, string argDisease)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT INSTS";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENTADD  ";
                SQL = SQL + ComNum.VBLF + " WHERE JDATE = TO_DATE('" + argINDT + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INTIME = '" + argINTM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPano + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["InSts"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        private void cboSayu_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (VB.Left(cboSayu.Text, 1) == "2")
            {
                cboArCf.Enabled = true;
                cboArCs.Enabled = true;
               

            }
            else
            {
                cboArCf.Enabled = false;
                cboArCs.Enabled = false;
                cboArCf.SelectedIndex = 0;
                cboArCs.SelectedIndex = 0;


            }
        }

    }
}
