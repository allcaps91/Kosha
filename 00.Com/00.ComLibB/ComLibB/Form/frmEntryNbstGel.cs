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
/// Description : 기타수납 거래처 선택
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> 기타수납 거래처 등록 </summary>
    public partial class frmEntryNbstGel : Form
    {
        /// <summary> 기타수납 거래처 등록 </summary>
        public frmEntryNbstGel()
        {
            InitializeComponent();
        }

        void frmNbstGelEntry_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            //ssView_Sheet1.Columns[7].Visible = false;
            //ssView_Sheet1.Columns[8].Visible = false;

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
                ssClear(ssView);

                SQL = "";
                SQL = "SELECT GelCode,Sangho,Tel,Damdang,Mail,Juso,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_NBSTGEL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY GelCode ";

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
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Tel"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Damdang"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Mail"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Juso"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = "";
                }

                dt.Dispose();
                dt = null;

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
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
            string strCode = "";
            string strSangho = "";
            string strTel = "";
            string strDamDang = "";
            string strMail = "";
            string strJuso = "";
            string strROWID = "";
            bool strDel = false;
            string strChange = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strDel = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value);
                    strCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strSangho = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strTel = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strDamDang = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strMail = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strJuso = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 8].Text.Trim();

                    if (strDel == true)
                    {
                        if (strROWID != "")
                        {
                            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                            //{
                            //    return; //권한 확인
                            //}

                            SQL = "";
                            SQL = "DELETE " + ComNum.DB_PMPA + "ETC_NBSTGEL ";
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
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_NBSTGEL (GelCode,Sangho,Tel,Damdang,Mail,Juso) VALUES ('";
                            SQL = SQL + ComNum.VBLF + strCode + "','" + strSangho + "','" + strTel + "','" + strDamDang;
                            SQL = SQL + ComNum.VBLF + "','" + strMail + "','" + strJuso + "') ";

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
                            SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_NBSTGEL SET Sangho='" + strSangho + "',Tel='" + strTel + "',";
                            SQL = SQL + ComNum.VBLF + " Damdang='" + strDamDang + "',Mail='" + strMail + "',";
                            SQL = SQL + ComNum.VBLF + " Juso='" + strJuso + "' ";
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

            strFont1 = "/fn\"굴림체\" /fz\"15\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/n" + "/c/f1" + "기타수납 거래처 코드집" + "/n";
            strHead2 = "/n" + "/l/f2" + "출력일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "/r" + "PAGE: " + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
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

        void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 8].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        void ssView_Change(object sender, ChangeEventArgs e)
        {
            ssView_Sheet1.Cells[e.Row, 8].Text = "Y";
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
