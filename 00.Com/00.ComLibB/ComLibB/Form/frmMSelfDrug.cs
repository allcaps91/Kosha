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

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmMSelfDrug.cs
    /// Description     : 제한사항 등록하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : 2018-05-11 이정현 폼 전면수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\BuCode10.frm(FrmMSelf_Drug) => frmMSelfDrug.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\BuCode10.frm(FrmMSelf_Drug)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// </vbp>
    /// </summary>
    public partial class frmMSelfDrug : Form
    {
        public frmMSelfDrug()
        {
            InitializeComponent();
        }

        private void frmMSelfDrug_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;

            cboJob.Text = "";
            cboJob.Items.Clear();
            cboJob.Items.Add("86.외래OCS 분할처방 금지");

            cboJob.SelectedIndex = 0;

            GetData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strGubunA = "";
            string strGubunB = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strGubunA = VB.Left(cboJob.Text, 1);
            strGubunB = VB.Mid(cboJob.Text, 2, 1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.SuCode,a.FieldA,a.FieldB,TO_CHAR(a.EntDate,'YYYY-MM-DD') EntDate,";
                SQL = SQL + ComNum.VBLF + "     a.ROWID,b.SuNameK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MSELF a, " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.GubunA = '" + strGubunA + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.GubunB = '" + strGubunB + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.SuCode = b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.SuCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = 0;
                    ssView_Sheet1.RowCount = dt.Rows.Count + 20;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FIELDA"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FIELDB"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = "";
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            if (ssView_Sheet1.NonEmptyRowCount < 1) { return; }

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strChange = "";
            string strROWID = "";
            string strSucode = "";
            string strGubunA = "";
            string strGubunB = "";
            string strFieldA = "";
            string strFieldB = "";
            string strDate = "";

            strGubunA = VB.Left(cboJob.Text, 1);
            strGubunB = VB.Mid(cboJob.Text, 2, 1);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strSucode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strFieldA = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    strFieldB = ssView_Sheet1.Cells[i, 3].Text.Trim();

                    strDate = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    if (strDate == "") { strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"); }

                    strROWID = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 7].Text.Trim();

                    SQL = "";

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (strROWID != "")
                        {
                            SQL = "DELETE " + ComNum.DB_PMPA + "BAS_MSELF";
                            SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";
                        }
                    }
                    else if (strChange == "Y")
                    {
                        if (strROWID == "" && strSucode != "")
                        {
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_MSELF";
                            SQL = SQL + ComNum.VBLF + "     (SuCode, GubunA, GubunB, FieldA, FieldB, EntDate)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + strSucode + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strGubunA + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strGubunB + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strFieldA + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strFieldB + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else if (strROWID != "")
                        {
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_MSELF";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         FieldA = '" + strFieldA + "', ";
                            SQL = SQL + ComNum.VBLF + "         FieldB = '" + strFieldB + "', ";
                            SQL = SQL + ComNum.VBLF + "         EntDate = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
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
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 20;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strHead3 = "";

            strFont1 = "/fn\"맑은 고딕\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "제한사항 코드집(BAS_MSELF)" + "/f1/n";
            strHead2 = "/l/f2" + cboJob.Text + "/f2/n";
            strHead3 = "/l/f2" + "출력일자 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + VB.Space(10) + "PAGE:/p" + "/f2/n";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.9f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + strFont2 + strHead3;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strData = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ssView_Sheet1.ActiveColumnIndex > 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 7].Text = "Y";
                }

                if (ssView_Sheet1.ActiveColumnIndex == 1)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text.ToUpper().Trim();
                    strData = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ActiveColumnIndex].Text;
                    
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SuNameK";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                    SQL = SQL + ComNum.VBLF + "     WHERE SuNext = '" + strData + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}

