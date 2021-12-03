using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 약품분류코드 등록
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 약품분류코드 등록 </summary>
    public partial class frmEntryYakClass : Form
    {
        /// <summary> 약품분류코드 등록 </summary>
        public frmEntryYakClass()
        {
            InitializeComponent();
        }

        void frmYakClassEntry_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.Columns[3].Visible = false;
            ssView_Sheet1.Columns[4].Visible = false;

            ScreenDisplay();
        }

        void ScreenDisplay()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssClear(ssView);

                SQL = "";
                SQL = "SELECT ClassCode,ClassName,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_Class ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ClassCode ";

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
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ClassCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ClassName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = "";
                }

                dt.Dispose();
                dt = null;

                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void ssClear(FpSpread SpreadName)
        {
            SpreadName.ActiveSheet.Cells[0, 0, SpreadName.ActiveSheet.Rows.Count - 1, SpreadName.ActiveSheet.Columns.Count - 1].Text = "";
            SpreadName.ActiveSheet.SetActiveCell(0, 0);
            SpreadName.ActiveSheet.RowCount = 0;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            int intCode = 0;
            string strName = "";
            string strROWID = "";
            string strChange = "";
            bool strDel = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strDel = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    intCode = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 1].Text.Trim()));
                    strName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 4].Text.Trim();

                    if (strDel == true)
                    {
                        if (strROWID != "")
                        {
                            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                            {
                                return; //권한 확인
                            }

                            SQL = "";
                            SQL = "DELETE BAS_CLASS ";
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
                            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                            {
                                return; //권한 확인
                            }

                            SQL = "";
                            SQL = "INSERT INTO BAS_CLASS (ClassCode,ClassName) VALUES (";
                            SQL = SQL + ComNum.VBLF + intCode + ",'" + strName + "') ";

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
                            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                            {
                                return; //권한 확인
                            }

                            SQL = "";
                            SQL = "UPDATE BAS_CLASS SET ClassCode=" + intCode + ",";
                            SQL = SQL + ComNum.VBLF + "      ClassName='" + strName + "' ";
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
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                ScreenDisplay();
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
            ScreenDisplay();
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

            strFont1 = "/fn\"굴림체\" /fz\"15\" ";
            strFont2 = "/fn\"굴림체\" /fz\"10\" ";
            strHead1 = "/n" + "/l/f1" + "약품 성분분류 코드집" + "/n";
            strHead2 = "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "PAGE: " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 300;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 180;
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

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 4].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        void ssView_Change(object sender, ChangeEventArgs e)
        {
            ssView_Sheet1.Cells[e.Row, 4].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
