using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmWonCodeEntry : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmWonCodeEntry.cs
        /// Description     : 수입항목대 수가코드 변환정보 조회, 수정 및 출력
        /// Author          : 김효성
        /// Create Date     : 2017-06-19
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busuga\BuSuga20.frm => frmWonCodeEntry.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\busuga\BuSuga20.frm(FrmWonCodeEntry)
        /// </seealso>
        /// <vbp>
        /// default : VBy
        /// </vbp>
        /// 
        public frmWonCodeEntry ()
        {
            InitializeComponent ();
        }
        
        private void frmWonCodeEntry_Load (object sender , EventArgs e)
        {
            int i = 0;
            string strList = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            try
            {                
                cboWongaList.Items.Clear ();
                cboWongaList.Items.Add ("0000.분류누락분");
                cboWongaList.Items.Add ("9999.전체코드");

                SQL = "SELECT Hang,HangName FROM ADMIN.WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE Hang >= '1000' AND Hang <= '1999' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SORTKEY ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strList = VB.Left ((dt.Rows [i] ["Hang"]).ToString ().Trim () + VB.Space (5) , 5);
                    strList = strList + dt.Rows [i] ["HangName"].ToString ().Trim ();
                    cboWongaList.Items.Add (strList);
                }
                dt.Dispose ();
                dt = null;
                cboWongaList.SelectedIndex = 2;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ScreenClear ()
        {
            if (ssView_Sheet1.Rows.Count > 0)
            {
                ssView_Sheet1.Rows.Count = 0;
            }
        }

        private string READ_WonName (string ArgCode)
        {
            string argreturn = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (ArgCode == "")
                {
                    return "";
                }
                SQL = "SELECT HangName FROM ADMIN.WON_HANG ";
                SQL = SQL + ComNum.VBLF + "WHERE Hang='" + ArgCode.Trim () + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    argreturn = dt.Rows [0] ["HangName"].ToString ().Trim ();
                }
                else
                {
                    argreturn = "** ERROR **";
                }
                dt.Dispose ();
                dt = null;

                return argreturn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnPrint_Click (object sender , EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssView_Sheet1.RowCount == 0) return;

            if (ComFunc.MsgBoxQ ("인쇄 하시겠습니까?" , "확인" , MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fk0 /fs2";
            strHead1 = "/c/fu1" + "수가코드대 수입항목코드 변환 정보" + "/fu0/n/n/n";
            strHead2 = "/r" + "출력일자:" + ComFunc.FormatStrToDate (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D") + "   Page: /p";

            ssView_Sheet1.PrintInfo.AbortMessage = "수가코드대 수입항목코드 변환 정보  인쇄중 !!!";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 40;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.UseSmartPrint = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet (0);
        }

        private void btnSearch_Click (object sender , EventArgs e)
        {
            int i = 0;
            int j = 0;
            int nRow = 0;
            string strOldData = "";
            string strNewData = "";
            string strWonName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT SuCode,SuNext,Bun,WonCode,SuNameK,GBWON1,GBWON2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE DelDate IS NULL ";

                switch (VB.Left (cboWongaList.Text , 4))
                {
                    case "0000":
                        SQL = SQL + ComNum.VBLF + " AND (WonCode IS NULL OR WonCode='    ') ";
                        break;
                    case "9999":
                        SQL = SQL + ComNum.VBLF + " AND WonCode IS NOT NULL ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + " AND WonCode = '" + VB.Left (cboWongaList.Text , 4) + "' ";
                        break;
                }

                if (optSugaCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY SuCode,SuNext ";
                }
                else if (optWongaCode.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY WonCode,SuCode,SuNext ";
                }

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow + 10;
                    }
                    ssView_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["SuCode"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SuNext"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["Bun"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["SuNameK"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["WonCode"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["GBWON1"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["GBWON2"].ToString ().Trim ();

                    strNewData = dt.Rows [i] ["WonCode"].ToString ().Trim ();

                    if (strOldData != strNewData)
                    {
                        strWonName = READ_WonName (strNewData);
                        strOldData = strNewData;
                    }
                    ssView_Sheet1.Cells [i , 5].Text = strWonName.ToString ().Trim ();
                }

                Cursor.Current = Cursors.Default;

                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void optSCheckedChanged (object sender , EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                ssView_Sheet1.Rows.Count = 0;
            }
        }

        private void ssView_EditModeOff (object sender , EventArgs e)
        {
            int i = 0;
            int intCol = ssView_Sheet1.ActiveColumnIndex;
            int intRow = ssView_Sheet1.ActiveRowIndex;
            string strData = "";
            string strSuNext = "";
            string strWonName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (intCol != 4 && intCol != 6 && intCol != 7)
                {
                    clsDB.setRollbackTran (clsDB.DbCon);
                    return;
                }

                strSuNext = ssView_Sheet1.Cells [intRow , 1].Text.ToString ().Trim ();

                if (intCol == 4)
                {
                    strData = ssView_Sheet1.Cells [intRow , intCol].Text.ToString ().Trim ();
                }
                else
                {
                    strData = (Convert.ToBoolean (ssView_Sheet1.Cells [intRow , intCol].Value) == true ? "1" : "0");
                }

                if (intCol == 4)
                {
                    strWonName = READ_WonName (strData);
                    if (strWonName == "** ERROR **")
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        MessageBox.Show ("원가항목분류가 오류입니다." , "오류");
                        ssView_Sheet1.Cells [intRow , 3].Text = "";
                        ssView_Sheet1.Cells [intRow , 5].Text = "";
                        return;
                    }

                    ssView_Sheet1.Cells [intRow , 5].Text = strWonName.ToString ().Trim ();

                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_SUN SET WonCode = '" + strData + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strSuNext + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                    {
                        if (ssView_Sheet1.Cells [i , 1].Text == strSuNext)
                        {
                            ssView_Sheet1.Cells [i , 4].Text = strData;
                            ssView_Sheet1.Cells [i , 5].Text = strWonName;
                        }
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        ComFunc.MsgBox (SqlErr);
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }
                }
                else if (intCol == 6)
                {
                    if (strData != "1")
                    {
                        strData = "0";
                    }
                    //자료를 DB 업데이트
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_SUN SET GBWON1 = '" + strData + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strSuNext + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                    {
                        if (ssView_Sheet1.Cells [i , 1].Text == strSuNext.ToString ().Trim ())
                        {
                            ssView_Sheet1.Cells [i , 6].Text = strData.ToString ().Trim ();
                        }
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        ComFunc.MsgBox (SqlErr);
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }
                }
                else
                {
                    if (strData != "1")
                    {
                        strData = "0";
                    }
                    SQL = "UPDATE "+ ComNum.DB_PMPA +"BAS_SUN SET GBWON2 = '" + strData + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + strSuNext + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    for (i = 0; i < ssView_Sheet1.Rows.Count; i++)
                    {
                        if (ssView_Sheet1.Cells [i , 1].Text == strSuNext.ToString ().Trim ())
                        {
                            ssView_Sheet1.Cells [i , 7].Text = strData.ToString ().Trim ();
                        }
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        ComFunc.MsgBox (SqlErr);
                        clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return ;
                    }
                }
                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboWongaList_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send ("{Tab}");
            }
        }
    }
}
