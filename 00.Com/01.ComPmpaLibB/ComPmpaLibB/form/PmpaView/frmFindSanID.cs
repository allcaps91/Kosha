using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmFindSanID : Form
    {
        public frmFindSanID()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void frmFindSanID_Load(object sender, EventArgs e)
        {
            TxtPano.Text = "";
            TxtName.Text = "";
        }

        private void TxtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)

            {
                CmdView.Focus();
            }
      
   
        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)

            {
                CmdView.Focus();
            }
        }

        private void CmdView_Click(object sender, EventArgs e)
        {

            Screen_Display(clsDB.DbCon);


        }


        private void Data_Delete(PsmhDb pDbCon, string StrROWID  )
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            switch (clsType.User.Sabun)
            {
                //'김종은 함종현  박시철 김보미 전산실 김정형
                case "32364":
                case "20175":
                case "19684":
                case "12576":
                case "4349":
                case "33478":
                case "11701":
                case "36550":
                    break;
                default:
                    ComFunc.MsgBox("산재작업 권한자가 아닙니다..!!");
                    return;
            }


            SQL = "";
            SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "BAS_SANID_HIS ";
            SQL += ComNum.VBLF + "  WHERE ROWID = '" + StrROWID + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }


        }
        private void Screen_Display(PsmhDb pDbCon)
        {
            int i = 0, nREAD = 0;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            //btnSearch.Enabled = false;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, BI, SNAME, JUMIN1, JUMIN2, TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, COPRNAME, ";
            SQL += ComNum.VBLF + " TO_CHAR(DATE2,'YYYY-MM-DD') DATE2, TO_CHAR(DATE3,'YYYY-MM-DD') DATE3, BENHO, JobSabun, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SANID_HIS ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
            if (TxtPano.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND PANO = '" + TxtPano.Text.Trim() + "' ";
            }
            if (TxtName.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND SNAME LIKE '" + TxtName.Text.Trim() + "%' ";
            }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nREAD = Dt.Rows.Count;

            SS1_Sheet1.Rows.Count = nREAD;

            if (nREAD > 0)
            {
                for (i = 0; i < nREAD; i++)
                {
                    SS1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DATE1"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DATE2"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DATE3"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["COPRNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["BENHO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["JobSabun"].ToString().Trim();

                }
            }

            Dt.Dispose();
            Dt = null;
        }


        private void CmdDel_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strDel = string.Empty;
            string strChk = string.Empty;
            string StrROWID = string.Empty;
            
          //  CmdDel.Enabled = false;

            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                strDel = SS1_Sheet1.Cells[i, 0].Text.Trim();
                StrROWID = SS1_Sheet1.Cells[i, 8].Text.Trim();
               

                if (strDel == "True" && StrROWID != string.Empty)
                {
                    Data_Delete(clsDB.DbCon, StrROWID);
                }
             

            }

            clsDB.setCommitTran(clsDB.DbCon);

            Screen_Display(clsDB.DbCon);
        }

        private void SS1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0, j = 0;
            int nREAD = 0;
            //int nYY = 0, nMM = 0;

            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strROWID = "";
            string strPano = "";
            //string strData = "";
            //string strBi = "";
            //btnSearch.Enabled = false;

          //  SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
          //  SS1_Sheet1.Rows.Count = 0;

            strPano = SS1.ActiveSheet.Cells[e.Row, 1].Text;
            strROWID = SS1.ActiveSheet.Cells[e.Row, 8].Text;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT pano,bi,sname,jumin1,jumin2, JUMIN3, coprname,coprno, ";
            SQL += ComNum.VBLF + " dept1,dept2,to_char(date1,'yyyy-mm-dd') date1, ";
            SQL += ComNum.VBLF + " to_char(date2,'yyyy-mm-dd') date2,to_char(date3,'yyyy-mm-dd') date3,to_char(daterequest,'yyyy-mm-dd') daterequest, ";
            SQL += ComNum.VBLF + "  gbresult,gbill,illcode1,illcode2,illcode3,illcode4,illcode5, ";
            SQL += ComNum.VBLF + "  dept3, memo, Count, Dname, BENHO, JONG, BoNo , DEPT4, DEPT5 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SANID_HIS ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1  ";
            SQL += ComNum.VBLF + "  AND PANO = '" + strPano.Trim() + "' ";
            SQL += ComNum.VBLF + "  AND ROWID = '" + strROWID.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nREAD = dt.Rows.Count;

            SS1_Sheet1.Rows.Count = nREAD;

            if (nREAD > 0)
            {
                clsType.TBS.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                clsType.TBS.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                clsType.TBS.Sname = dt.Rows[0]["Sname"].ToString().Trim();
                clsType.TBS.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                {
                    clsType.TBS.Jumin2 = clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                }
                else
                {
                    clsType.TBS.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();
                }
                clsType.TBS.CoprName = dt.Rows[0]["CoprName"].ToString().Trim();
                clsType.TBS.CoprNo = dt.Rows[0]["CoprNo"].ToString().Trim();
                clsType.TBS.Dept1 = dt.Rows[0]["Dept1"].ToString().Trim();
                clsType.TBS.Dept2 = dt.Rows[0]["Dept2"].ToString().Trim();
                clsType.TBS.Dept3 = dt.Rows[0]["Dept3"].ToString().Trim();
                clsType.TBS.Dept4 = dt.Rows[0]["Dept4"].ToString().Trim();
                clsType.TBS.Dept5 = dt.Rows[0]["Dept5"].ToString().Trim();
                clsType.TBS.Date1 = dt.Rows[0]["Date1"].ToString().Trim();
                clsType.TBS.Date2 = dt.Rows[0]["Date2"].ToString().Trim();
                clsType.TBS.Date3 = dt.Rows[0]["Date3"].ToString().Trim();
                clsType.TBS.DateRequest = dt.Rows[0]["DateRequest"].ToString().Trim();
                clsType.TBS.GbResult = dt.Rows[0]["GbResult"].ToString().Trim();
                clsType.TBS.GbIll = dt.Rows[0]["GbIll"].ToString().Trim();
                clsType.TBS.IllCode1 = dt.Rows[0]["IllCode1"].ToString().Trim();
                clsType.TBS.IllCode2 = dt.Rows[0]["IllCode2"].ToString().Trim();
                clsType.TBS.IllCode3 = dt.Rows[0]["IllCode3"].ToString().Trim();
                clsType.TBS.IllCode4 = dt.Rows[0]["IllCode4"].ToString().Trim();
                clsType.TBS.IllCode5 = dt.Rows[0]["IllCode5"].ToString().Trim();
                clsType.TBS.Memo = dt.Rows[0]["Memo"].ToString().Trim();
                clsType.TBS.Count = (int)VB.Val(dt.Rows[0]["Count"].ToString().Trim());
                clsType.TBS.Dname = dt.Rows[0]["Dname"].ToString().Trim();
                clsType.TBS.BenHo = dt.Rows[0]["BENHO"].ToString().Trim();
                clsType.TBS.JONG = dt.Rows[0]["JONG"].ToString().Trim();
                clsType.TBS.BoNo = dt.Rows[0]["BoNo"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            Application.DoEvents();
            this.Close();
            return;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
