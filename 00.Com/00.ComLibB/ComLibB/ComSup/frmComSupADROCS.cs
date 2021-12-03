using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADROCS
    /// Description     : 약물이상반응 모니터링
    /// Author          : 전상원
    /// Create Date     : 2019-04-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " >> frmComSupADROCS.cs 폼이름 재정의" />

    public partial class frmComSupADROCS : Form
    {
        string GstrSEQNO = "";
        string mstrPANO = "";
        string mstrDeptCode = "";
        string mstrStrIO = "";
        string mstrOPTION = "";

        frmComSupADR1 frmComSupADR1X;

        public frmComSupADROCS()
        {
            InitializeComponent();
        }

        public frmComSupADROCS(string strPano, string strDeptCode, string strIO, string strSeqno, string strIPDNO)
        {
            InitializeComponent();
            mstrPANO = strPano;
            mstrDeptCode = strDeptCode;
            mstrStrIO = strIO;
            GstrSEQNO = strSeqno;
            mstrOPTION = strIPDNO;
        }

        private void frmComSupADROCS_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (GstrSEQNO == "" && mstrPANO == "")
            {
                mstrPANO = ComFunc.LPAD(VB.InputBox("환자 정보가 없습니다. 환자 등록번호를 입력하여 주십시요", "등록번호입력"), 8, "0");
            }

            clsPublic.GnLogOutCNT = 0;

            ReadPatient(mstrPANO);
            GetData(GstrSEQNO);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            clsPublic.GnLogOutCNT = 0;

            ReadPatient(mstrPANO);
        }

        private void ReadPatient(string argPTNO)
        {
            int i = 0;
            string strSEQNO = "";

            StringBuilder Sql = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Sql.Append("SELECT TO_CHAR(WDATE,'YYYY-MM-DD') AS WDATE, WNAME, SEQNO     ").Append("\n");
                Sql.Append("  FROM KOSMOS_ADM.DRUG_ADR1    ").Append("\n");
                Sql.Append(" WHERE PTNO = '" + argPTNO + "'").Append("\n");
                Sql.Append("ORDER BY WDATE DESC            ").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, Sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = ReadProgress(dt.Rows[i]["SEQNO"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    }

                    strSEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSEQNO != "")
                {
                    clsPublic.GstrHelpCode = strSEQNO;
                    frmComSupPrtADR frmComSupPrtADRX = new frmComSupPrtADR(strSEQNO);
                    frmComSupPrtADRX.StartPosition = FormStartPosition.CenterParent;
                    frmComSupPrtADRX.ShowDialog();
                    frmComSupPrtADRX = null;
                    clsPublic.GstrHelpCode = "";
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

                clsDB.SaveSqlErrLog(ex.Message, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string ReadProgress(string ArgSeqno)
        {
            string rtnVal = "";

            StringBuilder Sql = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt1 = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Sql.Clear();
                Sql.Append("SELECT TO_CHAR(A.WDATE,'YYYY-MM-DD') ADR1,                  ").Append("\n");
                Sql.Append("       TO_CHAR(B.WDATE,'YYYY-MM-DD') ADR2,                  ").Append("\n");
                Sql.Append("       TO_CHAR(C.WDATE,'YYYY-MM-DD') ADR3,                   ").Append("\n");
                Sql.Append("       DECODE(TRIM(D.REPORT1 || D.REPORT2), '00', '', '01', ").Append("\n");
                Sql.Append("       '식약처 보고', '10', '위원회보고', '') ADR4          ").Append("\n");
                Sql.Append("  FROM KOSMOS_ADM.DRUG_ADR1 A,                              ").Append("\n");
                Sql.Append("       KOSMOS_ADM.DRUG_ADR2 B,                              ").Append("\n");
                Sql.Append("       KOSMOS_ADM.DRUG_ADR3 C,                              ").Append("\n");
                Sql.Append("       KOSMOS_ADM.DRUG_ADR4 D                               ").Append("\n");
                Sql.Append(" WHERE A.SEQNO = B.SEQNO(+)                                 ").Append("\n");
                Sql.Append("   AND A.SEQNO = C.SEQNO(+)                                 ").Append("\n");
                Sql.Append("   AND A.SEQNO = D.SEQNO(+)                                 ").Append("\n");
                Sql.Append("   AND A.SEQNO = '" + ArgSeqno + "'").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt1, Sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["ADR4"].ToString().Trim() != "")
                    {
                        rtnVal = "완료";
                    }
                    else if (dt1.Rows[0]["ADR3"].ToString().Trim() != "")
                    {
                        rtnVal = "진행중(2)";
                    }
                    else if (dt1.Rows[0]["ADR2"].ToString().Trim() != "")
                    {
                        rtnVal = "진행중(1)";
                    }
                    else
                    {
                        rtnVal = "보고";
                    }
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void btnADR_Click(object sender, EventArgs e)
        {
            GetADR();
        }

        private void GetADR()
        {
            clsPublic.GnLogOutCNT = 0;

            GstrSEQNO = "";

            GetData(GstrSEQNO);
        }

        private void btnADR2_Click(object sender, EventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            //if (fForm1 == null)
            //{

            //}
            //else
            //{
            //    Unload fForm1
            //    Set fForm1 = Nothing
            //}

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("작성 내역 조회에서 보고서를 선택하십시요");
                return;
            }

            StringBuilder Sql = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Sql.Append("SELECT ROWID                      ").Append("\n");
                Sql.Append("  FROM KOSMOS_ADM.DRUG_ADR4       ").Append("\n");
                Sql.Append(" WHERE SEQNO = '" + GstrSEQNO + "'").Append("\n");

                SqlErr = clsDB.GetDataTable(ref dt, Sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                }
                else
                {
                    ComFunc.MsgBox("완료된 보고서가 없습니다.");
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                frmComSupADR4 frmComSupADR4X = new frmComSupADR4();
                frmComSupADR4X.StartPosition = FormStartPosition.CenterParent;
                frmComSupADR4X.ShowDialog();
                frmComSupADR4X = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.SaveSqlErrLog(ex.Message, Sql.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetData(string strSEQNO)
        {
            //if (fForm1 == null)
            //{

            //}
            //else
            //{
            //    Unload fForm1
            //    Set fForm1 = Nothing
            //}
            if (frmComSupADR1X != null)
            {
                frmComSupADR1X.Dispose();
                frmComSupADR1X = null;
            }

            frmComSupADR1X = new frmComSupADR1(mstrPANO, mstrDeptCode, mstrStrIO, GstrSEQNO, mstrOPTION);

            pSubFormToControl(frmComSupADR1X, panADR);
            frmComSupADR1X.Width = panADR.Width - 50;
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            clsPublic.GnLogOutCNT = 0;

            if (e.Row < 0)
            {
                return;
            }

            GstrSEQNO = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
            GetData(GstrSEQNO);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            pControl.Dock = DockStyle.Fill;
            frm.Show();

        }
    }
}
