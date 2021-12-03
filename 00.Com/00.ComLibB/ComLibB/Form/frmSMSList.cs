using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using System.Text;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSMSList.cs
    /// Description     : 문자메시지 발송 내역 조회
    /// Author          : 이현종
    /// Create Date     : 2018-12-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Etc\kms\kms.vbp(FrmSMSList.frm) >> frmSMSList.cs 폼이름 재정의" />
    public partial class frmSMSList : Form
    {
        string GstrSabun = string.Empty;

        public frmSMSList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 의료정보팀용 
        /// </summary>
        /// <param name="strSabun"></param>
        public frmSMSList(string strSabun)
        {
            InitializeComponent();
            GstrSabun = strSabun;
        }

        void frmSMSManager_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }

        void btnView_Click(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            ssSMS_Sheet1.RowCount = 0;

            int i = 0;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL.AppendLine(" SELECT ENTDATE, JOBDATE, SNAME, HPHONE, SENDMSG, RETTEL, SENDTIME, ROWID");
                SQL.AppendLine("  FROM " + ComNum.DB_PMPA + "ETC_SMS");
                SQL.AppendLine("   WHERE GUBUN = '10' ");
                SQL.AppendLine("     AND ENTSABUN = '" + (string.IsNullOrEmpty(GstrSabun) ? clsType.User.Sabun : GstrSabun) + "'");
                SQL.AppendLine("     AND ENTDATE >= TO_DATE('" + dtpSdate.Value.ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI') ");
                SQL.AppendLine("     AND ENTDATE <= TO_DATE('" + dtpEdate.Value.ToShortDateString() + " 23:59','YYYY-MM-DD HH24:MI') ");
                if(checkBox1.Checked)
                {
                    SQL.AppendLine("     AND SENDTIME IS NULL  ");
                }
                SQL.AppendLine(" ORDER BY ENTDATE ");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssSMS_Sheet1.Rows.Count = dt.Rows.Count;
                ssSMS_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSMS_Sheet1.Cells[i, 1].Text = string.IsNullOrEmpty(dt.Rows[i]["ENTDATE"].ToString().Trim()) ? "" : Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
                    switch (clsType.User.Sabun)
                    {
                        case "26562":
                        case "45316":
                        case "16109":
                        case "25500":
                        case "37225":
                            ssSMS_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HPHONE"].ToString().Trim(); 
                            ssSMS_Sheet1.Cells[i, 2].Text = ssSMS_Sheet1.Cells[i, 2].Text + " " + dt.Rows[i]["SNAME"].ToString().Trim();
                            break;
                        default:
                            ssSMS_Sheet1.Cells[i, 2].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                            break;
                    }

                    ssSMS_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SENDMSG"].ToString().Trim(); 
                    ssSMS_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RETTEL"].ToString().Trim();
                    ssSMS_Sheet1.Cells[i, 5].Text = string.IsNullOrEmpty(dt.Rows[i]["SENDTIME"].ToString().Trim()) ? "" : Convert.ToDateTime(dt.Rows[i]["SENDTIME"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm");
                    ssSMS_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    //Row 높이 설정 2020-09-23 
                    FarPoint.Win.Spread.Row row;
                    row = ssSMS.ActiveSheet.Rows[i];
                    float rowSize = row.GetPreferredHeight();
                    row.Height = rowSize;
                } 

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if(checkBox1.Checked == false)
            {
                ComFunc.MsgBox("미발송 문자만 조회하신 후에 삭제하시기 바랍니다.");
                return;
            }

            if(ssSMS_Sheet1.NonEmptyRowCount == 0)
            {
                return;
            }

            if(Delete_Data() == true)
            {
                GetData();
            }
            return;
        }

        bool Delete_Data()
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            string strRowid = string.Empty;
            int intRowAffected = 0;

            bool rtnVal = false;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < ssSMS_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssSMS_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strRowid = ssSMS_Sheet1.Cells[i, 6].Text.Trim();
                        SQL.Clear();
                        SQL.AppendLine(" DELETE " + ComNum.DB_PMPA + "ETC_SMS");
                        SQL.AppendLine("WHERE ROWID = '" + strRowid + "'");

                        SqlErr = clsDB.ExecuteNonQuery(SQL.ToString().Trim(), ref intRowAffected, clsDB.DbCon);

                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
