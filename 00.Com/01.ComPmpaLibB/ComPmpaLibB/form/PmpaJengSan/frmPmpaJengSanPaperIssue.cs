using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanPaperIssue.cs
    /// Description     : 기타서류 발급 내역
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 기타서류 발급 내역
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm기타서류발급.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanPaperIssue : Form
    {
        private string GstrSysDate = "";
        clsPmpaFunc CPF = null;

        private frmPmpaViewSname frmPmpaViewSnameX = null;


        public frmPmpaJengSanPaperIssue()
        { 
            InitializeComponent();
        }

        private void frmPmpaJengSanPaperIssue_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            CPF = new clsPmpaFunc();

            GstrSysDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            txtPano.Text = "";
            lblSName.Text = "";

            ssView_Sheet1.RowCount = 0;

            dtpSDate.Value = Convert.ToDateTime(Convert.ToDateTime(GstrSysDate).ToString("yyyy-MM-01"));
            dtpEDate.Value = Convert.ToDateTime(GstrSysDate);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ACTDATE, A.PANO, A.USE1, A.USE2, A.REMARK, A.ENTPART, A.ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "     A.PJUMIN1, A.PJUMIN2, A.PSNAME, A.PTEL, A.PJUMIN3, A.GUBUN ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_PRINT_HIS A ";
                SQL = SQL + ComNum.VBLF + "     Where A.ACTDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (txtPano.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + txtPano.Text + "' ";
                }

                SQL = SQL + ComNum.VBLF + "         AND A.ENTPART <> '4349' ";

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(A.REMARK,1,2) = '입원' ";
                }
                else if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBSTR(A.REMARK,1,2) = '외래' ";
                }

                if (rdoGb1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.GUBUN = '01' ";
                }
                else if (rdoGb2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.GUBUN = '02' ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY A.ACTDATE";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim() == "01" ? "진료상세내역" : "수납내역서";
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ENTPART"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["USE2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PSNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PTEL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["USE1"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) { return; }
            if (txtPano.Text.Trim() == "") { return; }
            

            if (rdoBun0.Checked == true)
            {
                if (CPF.READ_BAS_PATIENT2(clsDB.DbCon, txtPano.Text, "1") == "NO")
                {
                    ComFunc.MsgBox("[" + txtPano.Text + "]해당하는 환자는 없습니다.");
                    txtPano.Text = "";
                }

                lblSName.Text = clsPmpaType.TBP.Sname;
            }
            else
            {
                if (String.Compare(VB.Left(txtPano.Text.Trim(), 1), "ㄱ") < 0)
                {
                    return;
                }

                clsPmpaPb.GstrName = txtPano.Text.Trim();
                clsPmpaPb.GstrFal = "1";

                frmPmpaViewSnameX = new frmPmpaViewSname();
                frmPmpaViewSnameX.rSendText += new frmPmpaViewSname.SendText(GetText);
                frmPmpaViewSnameX.rEventExit += new frmPmpaViewSname.EventExit(frmPmpaViewSnameX_rEventExit);
                frmPmpaViewSnameX.ShowDialog(this);
                
                if (CPF.READ_BAS_PATIENT2(clsDB.DbCon, txtPano.Text, "1") == "NO")
                {
                    ComFunc.MsgBox("[" + txtPano.Text + "]해당하는 환자는 없습니다.");
                    txtPano.Text = "";
                }

                lblSName.Text = clsPmpaType.TBP.Sname;
            }
        }

        void GetText(string ArgROWID)
        {
            CPF.READ_BAS_PATIENT2(clsDB.DbCon, ArgROWID, "2");
            txtPano.Text = clsPmpaType.TBP.Ptno;
        }

        void frmPmpaViewSnameX_rEventExit()
        {
            frmPmpaViewSnameX.Dispose();
            frmPmpaViewSnameX = null;
        }

    }
}
