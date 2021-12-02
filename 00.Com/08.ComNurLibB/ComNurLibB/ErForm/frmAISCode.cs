using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedErNr
    /// File Name       : frmAISCode.cs
    /// Description     : AIS CODE
    /// Author          : 이현종
    /// Create Date     : 2018-04-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\frmAISCode.frm(FrmAIS코드.frm) >> frmAISCode.cs 폼이름 재정의" />
    /// 

    public partial class frmAISCode : Form
    {
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        //Messgae Send
        public delegate void SendMsg(string strAISSCODE, string strAISSCOMM, string strAISSSCOR);
        public event SendMsg rSendMsg;

        string strPLAC = "";
        string mstrAISSREGI = ""; //GstrAISSREGI
  
        public frmAISCode()
        {
            InitializeComponent();
        }

        public frmAISCode(string strAISSREGI)
        {
            InitializeComponent();
            mstrAISSREGI = strAISSREGI;
        }



        private void frmAISCode_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssLIST_Sheet1.RowCount = 0;
            ssCODE_Sheet1.RowCount = 0;

            SETCOMBOBOX();

        }

        void SETCOMBOBOX()
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            cboGubun.Items.Clear();
            cboGubun.Items.Add("코드명");
            cboGubun.Items.Add("코드");
            cboGubun.SelectedIndex = 0;

            cboRegi.Items.Clear();

            if (mstrAISSREGI != "")
            {
                switch (mstrAISSREGI)
                {
                    case "HEAD|NECK":
                        cboRegi.Items.Add("HEAD");
                        cboRegi.Items.Add("NECK");
                        break;
                    case "LOWER EXTREMITY|UPPER EXTREMITY":
                        cboRegi.Items.Add("LOWER EXTREMITY");
                        cboRegi.Items.Add("UPPER EXTREMITY");
                        break;
                    default:
                        cboRegi.Items.Add(mstrAISSREGI);
                        break;
                }
            }
            else
            {
                cboRegi.Items.Add("ABDOMEN");
                cboRegi.Items.Add("EXTERNAL");
                cboRegi.Items.Add("FACE");
                cboRegi.Items.Add("HEAD");
                cboRegi.Items.Add("LOWER EXTREMITY");
                cboRegi.Items.Add("NECK");
                cboRegi.Items.Add("THORAX");
                cboRegi.Items.Add("UPPER EXTREMITY");
            }

            cboRegi.SelectedIndex = 0;

            if (cboRegi.Text == "") return;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = " SELECT AISSPLAC FROM KOSMOS_PMPA.BAS_AISCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE AISSREGI = '" + cboRegi.Text + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY AISSPLAC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    return;
                }

                ssLIST_Sheet1.RowCount = dt.Rows.Count;
                ssLIST_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssLIST_Sheet1.Cells[i, 0].Text = dt.Rows[i]["AISSPLAC"].ToString().Trim();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            ssCODE_Sheet1.RowCount = 0;
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string strSPACE = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = " SELECT AISSCODE, AISSSTEP, AISSCOMM, AISSSCOR ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_AISCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE AISSREGI = '" + cboRegi.Text + "' ";
                if (strPLAC != "") SQL = SQL + ComNum.VBLF + "   AND AISSPLAC = '" + strPLAC + "' ";
                if (cboGubun.Text == "코드명")
                {
                    SQL = SQL + ComNum.VBLF + "   AND AISSCOMM LIKE '%" + txtSearch.Text.Trim() + "%' ";
                } 
                else if (cboGubun.Text == "코드")
                {
                    SQL = SQL + ComNum.VBLF + "   AND AISSCODE LIKE '%" + txtSearch.Text.Trim() + "%' ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY AISSNUMB ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    return;
                }

                ssCODE_Sheet1.RowCount = dt.Rows.Count;
                ssCODE_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                int intSTEP = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSPACE = "";
                    intSTEP = Convert.ToInt32(dt.Rows[i]["AISSSTEP"].ToString().Trim());
                    if (intSTEP > 1) strSPACE = VB.Space(3 * intSTEP);

                    ssCODE_Sheet1.Cells[i, 0].Text = dt.Rows[i]["AISSSTEP"].ToString().Trim();
                    ssCODE_Sheet1.Cells[i, 1].Text = strSPACE +  dt.Rows[i]["AISSCOMM"].ToString().Trim();
                    if(intSTEP == 1)
                    {
                        ssCODE_Sheet1.Cells[i, 1].Font = new System.Drawing.Font("맑은고딕", 12, System.Drawing.FontStyle.Bold);
                    }
                    else
                    {
                        ssCODE_Sheet1.Cells[i, 1].Font = new System.Drawing.Font("맑은고딕", 9, System.Drawing.FontStyle.Regular);
                    }
                    
                    ssCODE_Sheet1.Cells[i, 2].Text = dt.Rows[i]["AISSSCOR"].ToString().Trim();
                    if (dt.Rows[i]["AISSSCOR"].ToString().Trim() == "0") ssCODE_Sheet1.Cells[i, 2].Text = "";
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

        private void ssCODE_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;
            if (e.Column < 0) return;

            if (ssCODE_Sheet1.Cells[e.Row, 0].Text == "") return;

            if (rSendMsg == null) return;

            rSendMsg(ssCODE_Sheet1.Cells[e.Row, 0].Text,
                     ssCODE_Sheet1.Cells[e.Row, 1].Text,
                     ssCODE_Sheet1.Cells[e.Row, 2].Text);
            rEventClosed();

        }

        private void ssLIST_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;
            if (ssLIST_Sheet1.Cells[e.Row, e.Column].Text == "") return;

            strPLAC = ssLIST_Sheet1.Cells[e.Row, e.Column].Text;

            ssCODE_Sheet1.RowCount = 0;

            GetAISSData();
        }

        void GetAISSData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strSPACE = "";

            try
            {
                SQL = " SELECT AISSCODE, AISSSTEP, AISSCOMM, AISSSCOR ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_AISCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE AISSREGI = '" + cboRegi.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND AISSPLAC = '" + strPLAC + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY AISSNUMB ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    return;
                }

                ssCODE_Sheet1.RowCount = dt.Rows.Count;
                ssCODE_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                int intSTEP = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSPACE = "";
                    intSTEP = Convert.ToInt32(dt.Rows[i]["AISSSTEP"].ToString().Trim());
                    if (intSTEP > 1) strSPACE = VB.Space(3 * intSTEP);

                    ssCODE_Sheet1.Cells[i, 0].Text = dt.Rows[i]["AISSSTEP"].ToString().Trim();
                    ssCODE_Sheet1.Cells[i, 1].Text = strSPACE + dt.Rows[i]["AISSCOMM"].ToString().Trim();
                    if (intSTEP == 1)
                    {
                        ssCODE_Sheet1.Cells[i, 1].Font = new System.Drawing.Font("맑은고딕", 12, System.Drawing.FontStyle.Bold);
                    }
                    else
                    {
                        ssCODE_Sheet1.Cells[i, 1].Font = new System.Drawing.Font("맑은고딕", 9, System.Drawing.FontStyle.Regular);
                    }

                    ssCODE_Sheet1.Cells[i, 2].Text = dt.Rows[i]["AISSSCOR"].ToString().Trim();
                    if (dt.Rows[i]["AISSSCOR"].ToString().Trim() == "0") ssCODE_Sheet1.Cells[i, 2].Text = "";
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
    }
}
