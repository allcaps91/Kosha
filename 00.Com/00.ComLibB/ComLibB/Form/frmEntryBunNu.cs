using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 분류누적코드 조회 저장
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 분류누적코드 </summary>
    public partial class frmEntryBunNu : Form
    {
        /// <summary> 분류누적코드 </summary>
        public frmEntryBunNu()
        {
            InitializeComponent();
        }

        private void frmBunNuEntry_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            //ssView_Sheet1.Columns[5].Visible = false;
            //ssView_Sheet1.Columns[6].Visible = false;

            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.분류");
            cboGubun.Items.Add("2.누적");
            cboGubun.Items.Add("3.EDI항");
            cboGubun.Items.Add("4.EDI목");
            cboGubun.SelectedIndex = 0;

            ScreenClear();
        }

        void ScreenClear()
        {
            cboGubun.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = true;
            ssClear(ssView);
            ssView.Enabled = false;
        }

        void ssClear(FpSpread SpreadName)
        {
            SpreadName.ActiveSheet.Cells[0, 0, SpreadName.ActiveSheet.Rows.Count - 1, SpreadName.ActiveSheet.Columns.Count - 1].Text = "";
            SpreadName.ActiveSheet.SetActiveCell(0, 0);
            ssView_Sheet1.RowCount = 0;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string strJong = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView.Enabled = true;

                if (cboGubun.Text.Trim() == "")
                {
                    ComFunc.MsgBox("구분이 공란입니다.", "오류");
                    return;
                }

                strJong = VB.Left(cboGubun.Text, 1);

                SQL = "";
                SQL = "SELECT Code,Name,TO_CHAR(JDate,'YYYY-MM-DD') JDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE Jong='" + strJong + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = "";
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = "";
                }

                dt.Dispose();
                dt = null;

                cboGubun.Enabled = false;
                btnSearch.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strJong = "";
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strDeldate = "";
            string strROWID = "";
            string strChange = "";
            bool strDel = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strJong = VB.Left(cboGubun.Text, 1);

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strDel = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    strCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strJDate = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strDeldate = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 6].Text.Trim();

                    switch (strJong)
                    {
                        case "6":
                        case "8":
                        case "9":
                            strCode = VB.Format(strCode, "0");
                            break;
                        default:
                            strCode = VB.Format(strCode, "00");
                            break;
                    }

                    if (strDel == true)
                    {
                        if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_BUN ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "")
                        {
                            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BUN SET Code='" + strCode + "',";
                            SQL = SQL + ComNum.VBLF + "      Name='" + strName + "',";
                            SQL = SQL + ComNum.VBLF + "      JDate=TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + "      DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }

                        if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BUN (Jong,Code,Name,JDate,DelDate) VALUES ('" + strJong + "','";
                            SQL = SQL + ComNum.VBLF + strCode + "','" + strName + "',";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strJDate + "','YYYY-MM-DD'),";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD')) ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                        }
                    }
                }
                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);

                ScreenClear();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            cboGubun.Focus();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ssView_Sheet1.RowCount = ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data);

            strFont1 = "/fn\" 굴림체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\" 굴림체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1" + VB.Right(cboGubun.Text, VB.Len(cboGubun.Text) - 2) + " 코드집" + "/n";
            strHead2 = "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "/r" + "PAGE: " + "/p" + VB.Space(25);

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = PrintType.All;
            ssView.PrintSheet(0);
        }

        void ssView_ButtonClicked_1(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        void ssView_Change_1(object sender, ChangeEventArgs e)
        {
            ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
