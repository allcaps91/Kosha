using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmAccountConv2 : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmAccountConv2.cs
        /// Description     : 감액계정 변환 Table
        /// Author          : 김형범
        /// Create Date     : 2017-06-30
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busanid\Bucode46.frm => frmAccountConv2.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\bucode\Bucode46.frm(frmAccountConv2.frm)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\bucode\bucode.vbp
        /// </vbp>

        public frmAccountConv2 ()
        {
            InitializeComponent ();
        }

        void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        void frmAccountConv2_Load (object sender , EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ScreenClear ();
            ssView_Sheet1.Columns [4].Visible = false;
            //작업종류를 ComboBox에 SET
            cboJob.Items.Clear ();
            cboJob.Items.Add ("2.감액계정 변환 Table");
            cboJob.SelectedIndex = 0;
        }

        void ScreenClear ()
        {
            cboJob.Enabled = true;
            btnPrint.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancle.Enabled = false;
            ssView.Enabled = false;
            ssView_Sheet1.RowCount = 20;
            ssClear (ssView);
        }

        /// <summary>
        /// 모듈 Spread Clear
        /// </summary>
        /// <param name="ssSpread"></param>
        void ssClear (FpSpread ssSpread)
        {
            ssSpread.ActiveSheet.Cells [0 , 0 , ssSpread.ActiveSheet.Rows.Count - 1 , ssSpread.ActiveSheet.Columns.Count - 1].Text = "";
            ssSpread.ActiveSheet.SetActiveCell (0 , 0);
        }

        void btnSearch_Click (object sender , EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string strGubun = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView_Sheet1.RowCount = 20;
            ssClear (ssView);

            strGubun = VB.Left (cboJob.Text , 1);
            if (strGubun == "")
            {
                return;
            }

            try
            {
                SQL = "SELECT Code,SuNext,Remark,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_ACCOUNT_CONV ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

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

                ssView_Sheet1.RowCount = dt.Rows.Count + 10;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells [i , 0].Text = "";
                    ssView_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["Code"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["SuNext"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["Remark"].ToString ().Trim ();
                    ssView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["ROWID"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.GetLastNonEmptyRow (FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 20;

                btnPrint.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                btnCancle.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnSave_Click (object sender , EventArgs e)
        {
            int i = 0;
            bool strDel = false;
            string strGubun = "";
            string strCode = "";
            string strSuNext = "";
            string strRemark = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow (FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strDel = Convert.ToBoolean(ssView_Sheet1.Cells [i , 0].Value);
                    strCode = ssView_Sheet1.Cells [i , 1].Text;
                    strSuNext = ssView_Sheet1.Cells [i , 2].Text;
                    strRemark = ssView_Sheet1.Cells [i , 3].Text;
                    strROWID = ssView_Sheet1.Cells [i , 4].Text;

                    SQL = "";

                    if (strDel == true)
                    {
                        if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this , "D", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                        }
                    }
                    else
                    {
                        if (strROWID == "")
                        {
                            if (strCode != "" && strSuNext != "")
                            {
                                //if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false)
                                //{
                                //    return; //권한 확인
                                //}

                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV (Gubun,Code,SuNext,";
                                SQL = SQL + ComNum.VBLF + "Remark) VALUES ('" + strGubun + "','";
                                SQL = SQL + ComNum.VBLF + strCode + "','" + strSuNext + "','" + strRemark.Trim () + "') ";
                            }
                        }
                        else
                        {
                            //if (ComQuery.IsJobAuth(this , "U", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV SET Code='" + strCode + "',";
                            SQL = SQL + ComNum.VBLF + "SuNext='" + strSuNext + "',";
                            SQL = SQL + ComNum.VBLF + "Remark = '" + strRemark.Trim () + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                        }
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran (clsDB.DbCon);
                            ComFunc.MsgBox (SqlErr);
                            clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                    }
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran (clsDB.DbCon);
                ssClear (ssView);
                cboJob.Focus ();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnCancle_Click (object sender , EventArgs e)
        {
            ScreenClear ();
            cboJob.Focus ();
        }

        private void btnPrint_Click (object sender , EventArgs e)
        {
            //if (ComQuery.IsJobAuth (this , "R") == false || ComQuery.IsJobAuth (this , "P") == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            int intRow = 0;
            string strOldData = "";
            string strNewData = "";
            string strGubuns = "";

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                strGubuns = "(";

                for (i = 0; i < cboJob.Items.Count; i++)
                {
                    strGubuns = strGubuns + "'" + VB.Left (cboJob.Items [i].ToString () , 1) + "',";
                }

                strGubuns = VB.Left (strGubuns , VB.Len (strGubuns) - 1) + ")";

                Cursor.Current = Cursors.WaitCursor;

                //ssView2_Sheet1.RowCount = 20;
                ssClear (ssView2);

                SQL = "";
                SQL = "SELECT Gubun,TO_CHAR(SDate,'YYYY-MM-DD') SDate,";
                SQL = SQL + ComNum.VBLF + "      Code,SuNext,Remark ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun IN " + strGubuns + " ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Gubun,SDate DESC,Code ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                ssView2_Sheet1.RowCount = dt.Rows.Count;

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

                strOldData = "";
                intRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = VB.Left (dt.Rows [i] ["Gubun"].ToString ().Trim () + VB.Space (12) , 12);
                    strNewData = strNewData + dt.Rows [i] ["SDate"].ToString ().Trim ();

                    if (strNewData != strOldData)
                    {
                        ssView2_Sheet1.Cells [i , 0].Text = clsVbfunc.READ_ConvGbn_Name (dt.Rows [i] ["Gubun"].ToString ().Trim ());
                        ssView2_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SDate"].ToString ().Trim ();
                        strOldData = strNewData;
                    }

                    if (VB.Right (strOldData , 10) != VB.Right (strNewData , 10))
                    {
                        ssView2_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["SDate"].ToString ().Trim ();
                        strOldData = strNewData;
                    }

                    ssView2_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["Code"].ToString ().Trim ();
                    ssView2_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["SuNext"].ToString ().Trim ();
                    ssView2_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["Remark"].ToString ().Trim ();

                    intRow = i;
                }

                dt.Dispose ();
                dt = null;

                ssView2_Sheet1.RowCount = intRow;

                strFont1 = "/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
                strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
                strHead1 = "/n" + "/c/f1" + "감액계정 변환정보(BAS_ACCOUNT_CONV)" + "/f1/n";
                strHead1 = "/l/f2" + "     출력일자 : " + Convert.ToDateTime (ComFunc.FormatStrToDateEx (ComQuery.CurrentDateTime (clsDB.DbCon, "D") , "D" , "-")) + "/r" + "PAGE: " + "/p" + VB.Space(5);

                ssView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssView2_Sheet1.PrintInfo.Margin.Top = 50;
                ssView2_Sheet1.PrintInfo.Margin.Bottom = 50;
                ssView2_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
                ssView2_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
                ssView2_Sheet1.PrintInfo.ShowBorder = true;
                ssView2_Sheet1.PrintInfo.ShowColor = false;
                ssView2_Sheet1.PrintInfo.ShowGrid = true;
                ssView2_Sheet1.PrintInfo.ShowShadows = false;
                ssView2_Sheet1.PrintInfo.UseMax = false;
                ssView2_Sheet1.PrintInfo.PrintType = PrintType.All;
                ssView2.PrintSheet (0);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox (ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        //TODO: 모듈 READ_SugaName
        void ssView_LeaveCell (object sender , LeaveCellEventArgs e)
        {
            string strSuNext = "";
            string strRemark = "";

            if (ssView_Sheet1.ColumnCount != 3)
            {
                return;
            }

            strSuNext = VB.UCase (ssView_Sheet1.Cells [e.Row , 2].Text);
            strRemark = VB.UCase (ssView_Sheet1.Cells [e.Row , 3].Text);
            ssView_Sheet1.Cells [e.Row , 3].Text = strSuNext;

            if (strSuNext == "")
            {
                ssView_Sheet1.Cells [e.Row , 3].Text = "";
                return;
            }
            ssView_Sheet1.Cells [e.Row , 3].Text = clsVbfunc.READ_SugaName (clsDB.DbCon, strSuNext);
        }
    }
}
