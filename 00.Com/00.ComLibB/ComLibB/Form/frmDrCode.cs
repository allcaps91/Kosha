using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmDrCode
    /// File Name : frmDrCode.cs
    /// Title or Description : 의사코드 등록 페이지
    /// Author : 박성완    
    /// Create Date : 2017-06-08
    /// <history> 
    /// </history>
    /// 2017-06-15 트랜잭션 부분 수정 
    /// </summary>
    public partial class frmDrCode : Form
    {
        string fstrROWID = "";

        public frmDrCode()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            txtCode.Text = "";
            txtRank.Text = "";
            txtName.Text = "";
            cboDept1.Text = "";
            txtEName.Text = "";
            txtTelNo.Text = "";
            cboDept2.Text = "";
            txtJin.Text = "";
            chkTour.Checked = false;

            fstrROWID = "";
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        void Screen_Display()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            txtCode.Text = txtCode.Text.Trim().ToUpper();

            if (txtCode.Text == "")
            {
                Screen_Clear();
                return;
            }

            if (txtCode.Text.Length != 4)
            {
                MessageBox.Show("의사코드 4자리를 입력하세요", "오류");
                txtCode.Focus();
                return;
            }

            //자료 READ
            SQL = "SELECT ROWID,DrCode,PrintRanking,DrName,DrEname,DrDept1,DrDept2,Tour,Telno, Jin, EMRPRTS, GBEXAM , GBCHOICE , GBGRADE ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_DOCTOR ";
            SQL = SQL + ComNum.VBLF + "WHERE DrCode = '" + txtCode.Text + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                txtRank.Text = "";
                txtName.Text = "";
                txtTelNo.Text = "";
                chkTour.Checked = false;
                cboDept1.SelectedIndex = -1;
                cboDept2.SelectedIndex = -1;
                fstrROWID = "";
                txtJin.Text = "";
                txtEmrSort.Text = "";
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnDelete.Enabled = false;
                txtRank.Focus();
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            fstrROWID = dt.Rows[0]["ROWID"].ToString();
            txtRank.Text = dt.Rows[0]["PrintRanking"].ToString();
            txtName.Text = dt.Rows[0]["DrName"].ToString();
            txtEName.Text = dt.Rows[0]["DrEName"].ToString();
            cboDept1.Text = dt.Rows[0]["DrDept1"].ToString();
            cboDept2.Text = dt.Rows[0]["DrDept2"].ToString();
            txtTelNo.Text = dt.Rows[0]["TelNo"].ToString();
            txtJin.Text = dt.Rows[0]["Jin"].ToString();
            txtEmrSort.Text = dt.Rows[0]["EMRPRTS"].ToString();
            chkTour.Checked = false;

            if (dt.Rows[0]["Tour"].ToString() == "Y")
            {
                chkTour.Checked = true;
            }
            else
            {
                chkTour.Checked = false;
            }

            if (dt.Rows[0]["GBEXAM"].ToString() == "Y")
            {
                chkGBEXAM.Checked = true;
            }
            else
            {
                chkGBEXAM.Checked = false;
            }

            if (dt.Rows[0]["GBCHOICE"].ToString() == "Y")
            {
                chkGBEXAM.Checked = true;
            }
            else
            {
                chkGBEXAM.Checked = false;
            }

            if (dt.Rows[0]["GBGRADE"].ToString() == "1")
            {
                cboGrade.Text = "1.전문의";
            }
            else
            {
                cboGrade.Text = "2.전공의";
            }

            dt.Dispose();
            dt = null;

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;

            txtRank.Focus();
        }

        bool SaveData()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (txtCode.Text.Trim().Length != 4) { MessageBox.Show("의사코드가 4자리가 아님", "오류"); return false; }
            if (VB.Val(txtRank.Text) == 0) { MessageBox.Show("출력시 서열이 공란입니다.", "오류"); return false; }
            if (txtName.Text.Trim() == "") { MessageBox.Show("의사성명이 공란입니다.", "오류"); return false; }
            if (cboDept1.Text.Trim() == "") { MessageBox.Show("진료과1이 공란입니다.", "오류"); return false; }
            if (VB.Right(txtCode.Text, 2) != "99")
            {
                if (txtEName.Text.Trim() == "") { MessageBox.Show("진료과장은 반드시 영문성명이 있어야 합니다.", "오류"); return false; }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (fstrROWID == "") // 신규등록
                {
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                    SQL = "INSERT INTO BAS_DOCTOR (DrCode,PrintRanking,DrName,DrEname,DrDept1,DrDept2,Tour,TelNo, Jin, EMRPRTS, GBEXAM, GBCHOICE, GBGRADE ) ";
                    SQL = SQL + ComNum.VBLF + "VALUES ('" + txtCode.Text + "'," + VB.Val(txtRank.Text) + ",";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtName.Text) + "' ,";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(txtEName.Text) + "',";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(cboDept1.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "'" + VB.Trim(cboDept2.Text) + "', ";
                    if (chkTour.Checked == true) { SQL = SQL + ComNum.VBLF + "'Y',"; }
                    if (chkTour.Checked == false) { SQL = SQL + ComNum.VBLF + "'N',"; }
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(txtTelNo.Text) + "' , '" + txtJin.Text + "' , '" + VB.Val(txtEmrSort.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + "  '" + (chkGBEXAM.Checked == true ? "Y" : "N") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  '" + (chkSelect.Checked == true ? "Y" : "N") + "'  , '" + VB.Left(cboGrade.Text, 1) + "'  ";
                    SQL = SQL + ComNum.VBLF + ") ";
                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                    SQL = "UPDATE BAS_DOCTOR SET PrintRanking=" + VB.Val(txtRank.Text) + ",";
                    SQL = SQL + ComNum.VBLF + "DrName='" + VB.Trim(txtName.Text) + "',";
                    SQL = SQL + ComNum.VBLF + "DrEName='" + VB.Trim(txtEName.Text) + "',";
                    SQL = SQL + ComNum.VBLF + "DrDept1='" + VB.Trim(cboDept1.Text) + "',";
                    SQL = SQL + ComNum.VBLF + "DrDept2='" + VB.Trim(cboDept2.Text) + "',";
                    if (chkTour.Checked == true) { SQL = SQL + ComNum.VBLF + "TOUR='Y'," + ","; }
                    if (chkTour.Checked == false) { SQL = SQL + ComNum.VBLF + "TOUR='N'," + ","; }
                    SQL = SQL + ComNum.VBLF + " TelNo='" + VB.Trim(txtTelNo.Text) + "', ";
                    SQL = SQL + ComNum.VBLF + " Jin ='" + VB.Trim(txtJin.Text) + "',  ";
                    SQL = SQL + ComNum.VBLF + " EMRPRTS = '" + VB.Val(txtEmrSort.Text) + "',  ";
                    SQL = SQL + ComNum.VBLF + "  GBEXAM = '" + (chkGBEXAM.Checked == true ? "Y" : "N") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  GBCHOICE = '" + (chkSelect.Checked == true ? "Y" : "N") + "' , ";
                    SQL = SQL + ComNum.VBLF + "  GBGRADE = '" + VB.Left(cboGrade.Text, 1) + "'  ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + fstrROWID + "' ";

                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                btnView.PerformClick();
                Screen_Clear();
                txtCode.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인
            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "삭제여부", MessageBoxButtons.YesNo) == DialogResult.No) return false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "DELETE BAS_DOCTOR WHERE DrCode='" + txtCode.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                btnView.PerformClick();
                Screen_Clear();
                txtCode.Focus();
                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strDept = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strDept = cboDept3.Text.Substring(0, 2);

                SQL = "SELECT DrCode,DrName,PrintRanking,DrDept1,DrDept2,Tour,TelNo, EMRPRTS";
                SQL = SQL + ComNum.VBLF + " FROM BAS_DOCTOR ";

                if (strDept != "**")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE (DrDept1='" + strDept + "' OR DrDept2='" + strDept + "') ";
                    if (optTour0.Checked == true) SQL = SQL + ComNum.VBLF + "";
                }
                else
                {
                    if (optTour0.Checked == true) SQL = SQL + ComNum.VBLF + "";
                }

                if (optSelect1.Checked == true) SQL = SQL + ComNum.VBLF + " AND  GBCHOICE = 'Y'";
                if (optSelect2.Checked == true) SQL = SQL + ComNum.VBLF + " AND ( GBCHOICE = 'N' OR GBCHOICE IS NULL) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DrCode,PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DrCode"].ToString();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DrName"].ToString();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PrintRanking"].ToString();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DrDept1"].ToString();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DrDept2"].ToString();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Tour"].ToString();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["TelNo"].ToString();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["EMRPRTS"].ToString();
                }
                ss1_Sheet1.Rows.Count++;
                dt.Dispose();
                dt = null;
                btnPrint.Enabled = true;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void frmDrCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strData = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            //진료과를 Combo에 Store
            SQL = "SELECT DeptCode,DeptNameK FROM BAS_CLINICDEPT ";
            SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking,DeptCode ";
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

            cboDept1.Items.Clear();
            cboDept2.Items.Clear();
            cboDept3.Items.Clear();
            cboDept2.Items.Add("  ");
            cboDept3.Items.Add("**.전체과");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strData = dt.Rows[i]["DeptCode"].ToString() + ".";
                strData += dt.Rows[i]["DeptNameK"].ToString().Trim();
                cboDept1.Items.Add(strData.Substring(0, 2));
                cboDept2.Items.Add(strData.Substring(0, 2));
                cboDept3.Items.Add(strData);
            }
            dt.Dispose();
            dt = null;
            cboDept3.SelectedIndex = 0;

            cboGrade.Items.Clear();

            cboGrade.Items.Add("1.전문의");
            cboGrade.Items.Add("2.전공의");
            cboGrade.SelectedIndex = 1;

            Screen_Clear();
            btnView.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData() == false) return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            txtCode.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "의 사 코 드 집";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void ss1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            txtCode.Text = ss1_Sheet1.Cells[e.Row, 0].Text;
            Screen_Display();
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
