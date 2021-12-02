using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmSanIlls : Form
    {
        public frmSanIlls()
        {
            InitializeComponent();
        }

        private void frmSanIlls_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            ss2.Sheets[0].Columns[6].Visible = false; //변경
            ss2.Sheets[0].Columns[7].Visible = false; //ROWID
            Screen_Clear();
            lst1.Items.Clear();
            lst1.Enabled = false;
        }

        private void Screen_Clear()
        {
            lst1.Enabled = true;
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            ss1_Sheet1.ClearRange(0, 0, ss1_Sheet1.Rows.Count, ss1_Sheet1.Columns.Count, true);
            ss1_Sheet1.SetActiveCell(0, 0);

            ss2_Sheet1.ClearRange(0, 0, ss2_Sheet1.Rows.Count, ss2_Sheet1.Columns.Count, true);
            ss2_Sheet1.SetActiveCell(ss2_Sheet1.Rows.Count - 1, ss2_Sheet1.Columns.Count - 1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            //TODO:모달리스? 모달?
            frmMsymHelp frm = new frmMsymHelp();
            frm.Show();
        }

        private void btnIlls_Click(object sender, EventArgs e)
        {
            //TODO:모달리스? 모달?
            frmKCD6_MsymEntry frm = new frmKCD6_MsymEntry();
            frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            lst1.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        private bool SaveData()
        {
            int nRank = 0;
            string strROWID = "";
            string strChk = "";
            string strDel = "";
            string strPano = "";
            string strCode = "";
            string strName = "";
            string strSDate = "";
            string strEDate = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strPano = ss1_Sheet1.Cells[0, 0].Text.Trim();
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            //자료의 오류여부 먼저 체크 한다.
            for (int i = 0; i < ss2_Sheet1.Rows.Count; i++)
            {
                strDel = ss2_Sheet1.Cells[0, 0].Text;
                nRank = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[0, 1].Text));
                strCode = ss2_Sheet1.Cells[0, 2].Text.Trim();
                strName = ss2_Sheet1.Cells[0, 3].Text.Trim();

                if ( strDel != "True" && (strCode != "" || strName != ""))
                {
                    if (nRank == 0) { MessageBox.Show(i + "번줄 순위가 ZERO입니다.", "확인"); return false; }
                    if (strCode == "") { MessageBox.Show(i + "번줄 순위가 상병코드가 공란입니다.", "확인"); return false; }
                    if (strName == "") { MessageBox.Show(i + "번줄 순위가 상병명칭이 공란입니다.", "확인"); return false; }
                }
            }

            try
            {
                for (int i = 0; i < ss2_Sheet1.Rows.Count; i++)
                {
                    strDel = ss2_Sheet1.Cells[i, 0].Text;
                    nRank = Convert.ToInt32(VB.Val(ss2_Sheet1.Cells[i, 1].Text));
                    strCode = ss2_Sheet1.Cells[i, 2].Text.Trim();
                    strName = ss2_Sheet1.Cells[i, 3].Text.Trim();
                    strSDate = ss2_Sheet1.Cells[i, 4].Text.Trim();
                    strEDate = ss2_Sheet1.Cells[i, 5].Text.Trim();
                    strChk = ss2_Sheet1.Cells[i, 6].Text.Trim();
                    strROWID = ss2_Sheet1.Cells[i, 7].Text.Trim();

                    if (strCode != "" || strName != "" || strROWID != "")
                    {
                        SQL = "";
                        if(strDel == "True") //체크박스 체크 된 것
                        {
                            if(strROWID != "")
                            {
                                //삭제 조건 만족
                                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "DELETE MIR_SANILLS WHERE ROWID = '" + strROWID + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                        else //체크박스 체크 안된 것
                        {
                            if (strROWID == "")
                            {
                                //Insert 조건 만족
                                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "INSERT INTO MIR_SANILLS (Pano,Rank,IllCode,IllName,";
                                SQL = SQL + ComNum.VBLF + "SDate,EDate) VALUES ('" + strPano + "',";
                                SQL = SQL + ComNum.VBLF + nRank + ",'" + strCode + "','" + strName + "',";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strSDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "TO_DATE('" + strEDate + "','YYYY-MM-DD')) ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            else if (strChk == "Y")
                            {
                                //Update 조건 만족
                                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "UPDATE MIR_SANILLS SET Rank=" + nRank + ",";
                                SQL = SQL + ComNum.VBLF + "IllCode='" + strCode + "',";
                                SQL = SQL + ComNum.VBLF + "IllName='" + strName + "',";
                                SQL = SQL + ComNum.VBLF + "SDate=TO_DATE('" + strSDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "EDate=TO_DATE('" + strEDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Screen_Clear();
                lst1.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                Screen_Clear();
                lst1.Focus();
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <returns></returns>
        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            lst1.Enabled = true;
            lst1.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "  SELECT Pano,Sname FROM BAS_SANID ";
                SQL = SQL + ComNum.VBLF + "WHERE Bi = '31' ";
                if (optEnd0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND DATE3 IS NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND DATE3 >= TRUNC(SYSDATE-360) ";
                }
                if (optSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Sname,Pano ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Pano ";
                }
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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lst1.Items.Add(dt.Rows[i]["Pano"].ToString() + " " + dt.Rows[i]["Sname"].ToString().Trim());
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return true;
            }

            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }

        private void lst1_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            strPano = lst1.SelectedItem.ToString().Substring(0, 8);

            SQL = "    SELECT Pano,Sname,CoprNo,CoprName,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(Date1,'YYYY-MM-DD') Date1, ";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(Date2,'YYYY-MM-DD') Date2, ";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(Date3,'YYYY-MM-DD') Date3  ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_SANID ";
            SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPano + "' ";
            SQL = SQL + ComNum.VBLF + "  AND Bi = '31' ";

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

            ss1_Sheet1.Cells[0, 0].Text = strPano;
            ss1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["Sname"].ToString().Trim(); 
            ss1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Date1"].ToString();
            ss1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Date2"].ToString();
            ss1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["Date3"].ToString();
            ss1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["CoprNo"].ToString().Trim();
            ss1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["CoprName"].ToString().Trim();

            btnView.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = false;
            lst1.Enabled = false;
            ss2.Enabled = true;
            //기존의 상병 READ
            SQL = "  SELECT Rank,IllCode,IllName,ROWID,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(SDate,'YYYY-MM-DD') SDate,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(EDate,'YYYY-MM-DD') EDate ";
            SQL = SQL + ComNum.VBLF + " FROM MIR_SANILLS ";
            SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPano + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Rank,IllCode ";
            dt = null;
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
              //  ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            ss2_Sheet1.Rows.Count = dt.Rows.Count + 10;
            ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss2_Sheet1.Cells[i, 1].Text = String.Format("{0}", (i * 10) + 10);
                ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IllName"].ToString().Trim();
                ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SDate"].ToString().Trim();
                ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EDate"].ToString().Trim();
                ss2_Sheet1.Cells[i, 6].Text = "";
                ss2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            ss2.Focus();
        }

        private void ss2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strCode = "";
            string strOldName = "";
            string strNewName = "";
            string strMsg = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss2_Sheet1.Cells[e.Row, 6].Text = "Y";
            if (e.Column != 2)
            {
                return;
            }

            strCode = ss2_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper();
            if (strCode == "")
            {
                MessageBox.Show("상병코드가 공란입니다", "확인");
                return;
            }

            //표준상병에서 상병명칭을 READ
            SQL = "  SELECT Name FROM EDI_MSYM ";
            SQL = SQL + ComNum.VBLF + "WHERE Code = '" + strCode + "' ";
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
            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ss2_Sheet1.Cells[e.Row, 2].Text = "";
                ComFunc.MsgBox("자료가 없습니다.");
                return;
            }

            strOldName = ss2_Sheet1.Cells[e.Row, 3].Text.Trim();
            strNewName = dt.Rows[0]["Name"].ToString().Trim();

            if ( strOldName == "")
            {
                ss2_Sheet1.Cells[e.Row, 3].Text = strNewName;
            }
            else if ( strOldName != strNewName)
            {
                strMsg = "[" + strNewName + "]" + VB.Chr(13);
                strMsg = strMsg + "상병명을 변경하시겠습니까?";
                if ( MessageBox.Show(strMsg, "변경?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ss2_Sheet1.Cells[e.Row, 3].Text = strNewName;
                }
            }
            dt.Dispose();
            dt = null;
        }
    }
}
