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

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmReservePaperTellMng.cs
    /// Description     : 예약증전화번호등록
    /// Author          : 유진호
    /// Create Date     : 2018-01-16
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm예약증전화번호
    /// </history>
    /// </summary>
    public partial class frmReservePaperTellMng : Form
    {
        ComFunc CF = new ComFunc();

        public frmReservePaperTellMng()
        {
            InitializeComponent();
        }

        private void frmReservePaperTellMng_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF.COMBO_DEPT_SET(clsDB.DbCon, cboDept, "1", "1");
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDEPT = "";

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strDEPT = VB.Left(cboDept.Text, 2);
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE,DRDEPT1,DRNAME,TELNO,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE TOUR = 'N' ";
                if (strDEPT != "**") SQL = SQL + ComNum.VBLF + "  AND DRDEPT1 = '" + strDEPT + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING,DRCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DRDEPT1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TELNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";
            string strTel = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; ; //권한 확인

                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 4].Text;
                    strTel = ssView_Sheet1.Cells[i, 3].Text;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.BAS_DOCTOR SET ";
                    SQL = SQL + ComNum.VBLF + " TELNO = '" + strTel + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
