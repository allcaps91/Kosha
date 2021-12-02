using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmReligionChange : Form
    {
        string FstrROWID = "";
        string FstrJong = "";
        string[] strBis = new string[56];

        public frmReligionChange()
        {
            InitializeComponent();
        }

        private void frmReligionChange_Load(object sender, EventArgs e)
        {
            ssView_Sheet1.Columns[11].Visible = false;

            strBis[11] = "공단";
            strBis[12] = "직장";
            strBis[13] = "지역";
            strBis[14] = "";
            strBis[15] = "";
            strBis[21] = "보호1";
            strBis[22] = "보호2";
            strBis[23] = "보호3";
            strBis[24] = "행려";
            strBis[25] = "";
            strBis[31] = "산재";
            strBis[32] = "공상";
            strBis[33] = "산재공상";
            strBis[34] = "";
            strBis[35] = "";
            strBis[41] = "공단100%";
            strBis[42] = "직장100%";
            strBis[43] = "지역100%";
            strBis[44] = "가족계획";
            strBis[45] = "보험계약";
            strBis[51] = "일반";
            strBis[52] = "TA 보험";
            strBis[53] = "계약";
            strBis[54] = "미확인";
            strBis[55] = "TA 일반";

            cboReligion.Items.Clear();
            cboReligion.Items.Add("*.전체");
            cboReligion.Items.Add("1.천주교");
            cboReligion.Items.Add("2.개신교");
            cboReligion.Items.Add("3.불  교");
            cboReligion.Items.Add("4.천도교");
            cboReligion.Items.Add("5.유  교");
            cboReligion.Items.Add("6.무  교");
            cboReligion.Items.Add("9.기  타");
            cboReligion.SelectedIndex = 0;


            cboJong.Items.Add(" ");
            cboJong.Items.Add("1.천주교");
            cboJong.Items.Add("2.개신교");
            cboJong.Items.Add("3.불  교");
            cboJong.Items.Add("4.천도교");
            cboJong.Items.Add("5.유  교");
            cboJong.Items.Add("6.무  교");
            cboJong.Items.Add("9.기  타");
            cboJong.SelectedIndex = 0;

            Screen_Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            string strBi = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WardCode,RoomCode,Pano,Bi,Sname,Pname,Sex,Age,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate, 'yyyy-mm-dd') InDate,DeptCode,DrName,Religion,i.ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER i,BAS_DOCTOR k ";
                SQL = SQL + ComNum.VBLF + " WHERE i.DrCode = k.DrCode";
                SQL = SQL + ComNum.VBLF + "   AND i.OUTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND i.GBSTS  = '0' ";

                if (VB.Left(cboReligion.Text, 1) != "*")
                {
                    SQL = SQL + ComNum.VBLF + "   AND RELIGION = '" + VB.Left(cboReligion.Text, 1) + "' ";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY ROOMCODE,SNAME";
                }

                if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY INDATE, SNAME";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count < 20)
                {
                    ssView_Sheet1.RowCount = 20;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = strBis[Convert.ToInt32(VB.Val(strBi))];

                    switch (dt.Rows[i]["Religion"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 5].Text = "1.천주교";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 5].Text = "2.개신교";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 5].Text = "3.불  교";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[i, 5].Text = "4.천도교";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[i, 5].Text = "5.유  교";
                            break;
                        case "6":
                            ssView_Sheet1.Cells[i, 5].Text = "6.무  교";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 5].Text = "9.기  타";
                            break;
                    }

                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cboReligion.Focus();
            ssView_Sheet1.RowCount = 0;
            Screen_Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("변경할 환자를 먼저 선택하세요");
                return;
            }

            if (cboJong.Text == "")
            {
                ComFunc.MsgBox("변경할 종교를 선택하세요");
                return;
            }

            if (ComFunc.MsgBoxQ("기존 " + FstrJong + "종교를 ->> " + cboJong.Text.Trim() + " 종교로 변경하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE KOSMOS_PMPA.IPD_NEW_MASTER SET ";
                SQL = SQL + ComNum.VBLF + "  Religion ='" + VB.Left(cboJong.Text.Trim(), 1) + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("정상적으로 종교를 변경하였습니다..");
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                GetData();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            txtPano.Text = "";
            txtSName.Text = "";
            txtWardCode.Text = "";
            txtRoomCode.Text = "";
            txtDeptCode.Text = "";
            txtSex.Text = "";
            txtAge.Text = "";
            FstrROWID = "";
            FstrJong = "";
            cboJong.Text = "";
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtPano.Text = ssView_Sheet1.Cells[e.Row, 0].Text;
            txtSName.Text = ssView_Sheet1.Cells[e.Row, 1].Text;
            txtWardCode.Text = ssView_Sheet1.Cells[e.Row, 2].Text;
            txtRoomCode.Text = ssView_Sheet1.Cells[e.Row, 3].Text;
            cboJong.Text = ssView_Sheet1.Cells[e.Row, 5].Text;
            FstrJong = ssView_Sheet1.Cells[e.Row, 5].Text;
            txtDeptCode.Text = ssView_Sheet1.Cells[e.Row, 9].Text;
            txtSex.Text = ssView_Sheet1.Cells[e.Row, 6].Text;
            txtAge.Text = ssView_Sheet1.Cells[e.Row, 7].Text;
            FstrROWID = ssView_Sheet1.Cells[e.Row, 11].Text;
        }
    }
}
