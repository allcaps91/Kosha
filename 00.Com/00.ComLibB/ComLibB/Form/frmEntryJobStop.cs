using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : OCS, 원무행정 업무 중지 시간 설정
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// </history>
namespace ComLibB
{
    /// <summary> OCS, 원무행정 업무 중지 시간 설정 </summary>
    public partial class frmEntryJobStop : Form
    {
        string fstrRowId = "";

        /// <summary> OCS, 원무행정 업무 중지 시간 설정 </summary>
        public frmEntryJobStop()
        {
            InitializeComponent();
        }

        void frmEntryJobStop_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            fstrRowId = "";


            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(JobStop_From,'YYYY-MM-DD HH24:MI') JobStop_From,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(JobStop_TO,'YYYY-MM-DD HH24:MI') JobStop_TO,Remark,ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_JOBSTOP_TIME ";

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

                txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                fstrRowId = dt.Rows[0]["ROWID"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "DELETE ETC_JOBSTOP_TIME WHERE ROWID='" + fstrRowId + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C") == false || ComQuery.IsJobAuth(this, "U") == false)
            //{
            //    return; //권한 확인
            //}

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (txtRemark.Text == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("업무중지 안내멘트가 공란입니다.", "오류");
                    return;
                }

                if (fstrRowId == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO ETC_JOBSTOP_TIME (JobStop_From,JobStop_To,Remark) VALUES (";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " " + dtpFTime.Value.ToString("hh:mm") + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + " " + dtpTTime.Value.ToString("hh:mm") + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "'" + txtRemark.Text + "') ";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE ETC_JOBSTOP_TIME ";
                    SQL = SQL + ComNum.VBLF + "SET ";
                    SQL = SQL + ComNum.VBLF + "JobStop_From=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " " + dtpFTime.Value.ToString("hh:mm") + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "JobStop_TO  =TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + " " + dtpTTime.Value.ToString("hh:mm") + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + "Remark='" + txtRemark.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + fstrRowId + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void dtpFTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void dtpTTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
