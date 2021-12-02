using FarPoint.Win.Spread;
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
/// Description : 외래 보험제외 상병 저장 인쇄
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 외래 보험제외 상병 </summary>
    public partial class frmMiSelf : Form
    {
        /// <summary> 외래 보험제외 상병 </summary>
        public frmMiSelf()
        {
            InitializeComponent();
        }

        void frmMiSelf_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[5].Visible = false;
            ssView_Sheet1.Columns[6].Visible = false;

            ScreenDisplay();
        }

        void ScreenDisplay()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = "SELECT a.IllCode,a.DeptCode,TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate,";
                SQL = SQL + ComNum.VBLF + "      a.ROWID,b.IllNameK ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_MISELF a, " + ComNum.DB_PMPA + "BAS_ILLS b ";
                SQL = SQL + ComNum.VBLF + " WHERE a.IllCode = b.IllCode(+) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.IllCode ";

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
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IllNameK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = "";
                }

                dt.Dispose();
                dt = null;
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
            string strChange = "";
            string strROWID = "";

            string strIllCode = "";
            string strDeptCode = "";
            string strDate = "";
            bool strChk = false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strChk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    strIllCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strDeptCode = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strDate = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 6].Text.Trim();

                    SQL = "";

                    if (strChk == true)
                    {
                        if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "DELETE BAS_MISELF WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strIllCode != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "INSERT INTO BAS_MISELF (IllCode,DeptCode,EntDate) ";
                            SQL = SQL + ComNum.VBLF + "VALUES ('" + strIllCode + "','" + strDeptCode + "',";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
                        }
                        else if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "UPDATE BAS_MISELF SET IllCode='" + strIllCode + "',";
                            SQL = SQL + ComNum.VBLF + "DeptCode='" + strDeptCode + "',";
                            SQL = SQL + ComNum.VBLF + "EntDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }
                    }

                    if (SQL != "")
                    {
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
                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
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

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/n" + "/c/f1" + "외래 보험제외 상병(BAS_MISELF)" + "/n";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead2 = "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "/r" + "PAGE : " + "/p" + VB.Space(15);

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

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void ssView_Change(object sender, ChangeEventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strData = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                if (e.Column > 0)
                {
                    ssView_Sheet1.Cells[e.Row, 6].Text = "Y";
                }

                if (e.Column == 1)
                {
                    strData = VB.UCase(ssView_Sheet1.Cells[e.Row, e.Column].Text).Trim();

                    SQL = "";
                    SQL = "SELECT IllNameK FROM BAS_ILLS ";
                    SQL = SQL + ComNum.VBLF + "WHERE IllCode = '" + strData + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        ssView_Sheet1.Cells[e.Row, 1].Text = "";
                        ssView_Sheet1.Cells[e.Row, 4].Text = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[e.Row, 4].Text = dt.Rows[0]["IllNameK"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }
    }
}
