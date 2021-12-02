using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 병동코드 선택
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 병동코드 등록 </summary>
    public partial class frmEntryWard : Form
    {
        /// <summary> 병동코드 등록 </summary>
        public frmEntryWard()
        {
            InitializeComponent();
        }

        void frmWardEntry_Load(object sender, EventArgs e)
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
                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "SELECT WardCode, WardName, ROWID , USED ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

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
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WardName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = "";
                    ssView_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["USED"].ToString().Trim() == "Y" ? "true" : "false");

                    if (dt.Rows[i]["USED"].ToString().Trim() == "Y")
                    {
                        ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 200, 200);
                    }
                }

                dt.Dispose();
                dt = null;

                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strCode = "";
            string strName = "";
            string strROWID = "";
            string strChange = "";
            string strUsed = "";
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
                    strCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strUsed = (ssView_Sheet1.Cells[i, 4].Text.Trim() == "true" ? "Y" : "");

                    if (strDel == true)
                    {
                        if (strROWID != "")
                        {
                            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                            {
                                return; //권한 확인
                            }

                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_WARD ";
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
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_WARD (WardCode,WardName, Used) VALUES ('";
                            SQL = SQL + ComNum.VBLF + strCode + "','" + strName + "', '" + strUsed + "') ";

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
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_WARD set ";
                            SQL = SQL + ComNum.VBLF + "  WardCode ='" + strCode + "',";
                            SQL = SQL + ComNum.VBLF + "  WardName ='" + strName + "',  ";
                            SQL = SQL + ComNum.VBLF + "  Used     = '" + strUsed + "' ";
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
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"15\" ";
            strFont2 = "/fn\"굴림체\" /fz\"10\" ";
            strHead1 = "/n" + "/l/f1" + "병동 코드집" + "/n";
            strHead1 = "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "PAGE: " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 300;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 180;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssView_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 4].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssView_Sheet1.Cells[e.Row, 4].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
