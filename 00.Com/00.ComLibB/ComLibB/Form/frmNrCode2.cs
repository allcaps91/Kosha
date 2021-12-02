using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmNrCode2
    /// File Name : frmNrCode2.cs
    /// Title or Description : 간호부 각종 기초코드 등록 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-06
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmNrCode2 : Form
    {
        public frmNrCode2()
        {
            InitializeComponent();
        }

        private void frmNrCode2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            //TODO: Call FormInfo_History(Me.Name, Me.Caption) 폼 로딩 사용 빈도
            ss1_Sheet1.Columns[6].Visible = false;
            ss1_Sheet1.Columns[7].Visible = false;
            ss1_Sheet1.Columns[8].Visible = false;
            ss1_Sheet1.Columns[9].Visible = false;
            ss1_Sheet1.Columns[10].Visible = false;

            cboJong.Items.Clear();
            cboJong.Items.Add("A.주간당직");
            cboJong.Items.Add("B.저녁당직");
            cboJong.SelectedIndex = 0;

            lst1.Items.Clear();

            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            ss1.Enabled = false;
            lst1.Enabled = false;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strData = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            Screen_Clear();
            lst1.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //리스트 박스 데이터 셋팅
                SQL = "";
                SQL += "SELECT Sabun,KorName,Jik " + ComNum.VBLF;
                SQL += " FROM " + ComNum.DB_ERP + "INSA_MST " + ComNum.VBLF;
                SQL += " WHERE JIK IN ('04','31','32','33') " + ComNum.VBLF;
                SQL += "  AND Buse IN (" + ComNum.VBLF;
                SQL += " SELECT MATCH_CODE " + ComNum.VBLF;
                SQL += " FROM " + ComNum.DB_PMPA + "NUR_CODE " + ComNum.VBLF;
                SQL += " WHERE GUBUN = '2') " + ComNum.VBLF;
                SQL += "  AND ToiDay is NULL " + ComNum.VBLF;
                SQL += "ORDER BY KorName " + ComNum.VBLF;
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
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strData = VB.Left(dt.Rows[i]["Sabun"].ToString().Trim() + VB.Space(8), 8);
                    strData += VB.Left(dt.Rows[i]["KorName"].ToString().Trim() + VB.Space(10), 10);
                    strData += VB.Left(dt.Rows[i]["Jik"].ToString().Trim() + VB.Space(2), 2);
                    lst1.Items.Add(strData);
                }


                //스프레드
                SQL = "";
                SQL += "SELECT Code,Name,Jik,PrintRanking,ROWID " + ComNum.VBLF;
                SQL += " FROM NUR_CODE " + ComNum.VBLF;
                SQL += "WHERE Gubun = '" + VB.Left(cboJong.Text, 1) + "' " + ComNum.VBLF;
                SQL += "ORDER BY PrintRanking,Jik,Name " + ComNum.VBLF;
                dt = null;
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
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count + 20;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = "";
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Jik"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = Read_JikName(dt.Rows[i]["Jik"].ToString().Trim());
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Jik"].ToString().Trim();
                }
                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;
                lst1.Enabled = true;
                ss1.Enabled = true;
                ss1.Focus();
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// Jik을 읽어 cJikName 반환
        /// </summary>
        /// <param name="argJik">Jik</param>
        /// <returns></returns>
        private string Read_JikName(string argJik)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Name cJikName FROM NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL += ComNum.VBLF + "  AND Code = '" + String.Format("{0:00}", argJik) + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                if (dt.Rows[0]["cJikName"].ToString().Trim() != "")
                {
                    rtnVal = dt.Rows[0]["cJikName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch
            {
                dt.Dispose();
                dt = null;
                rtnVal = "";
                return rtnVal;
            }
        }

        /// <summary>
        /// 화면정리 함수
        /// </summary>
        private void Screen_Clear()
        {
            ss1_Sheet1.Rows.Count = 0;
            ss1.Enabled = false;
            lst1.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            cboJong.Focus();
            lst1.Items.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        /// <summary>
        /// Insert, Update, 체크박스 체크시 삭제 함수
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            string strOK = "";
            int nSeq, nOldSeq = 0;
            string strCode, strOldCode = "";
            string strName, strOldName = "";
            string strJik, strOldJik = "";
            string strRowid = "";
            string strDel = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            lst1.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    strDel = ss1_Sheet1.Cells[i, 0].Text.Trim();
                    nSeq = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 1].Text.Trim()));
                    strCode = ss1_Sheet1.Cells[i, 2].Text.Trim();
                    strName = ss1_Sheet1.Cells[i, 3].Text.Trim();
                    strJik = String.Format("{0:00}", ss1_Sheet1.Cells[i, 4].Text.Trim());
                    strRowid = ss1_Sheet1.Cells[i, 6].Text.Trim();
                    nOldSeq = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 7].Text.Trim()));
                    strOldCode = ss1_Sheet1.Cells[i, 8].Text.Trim();
                    strOldName = ss1_Sheet1.Cells[i, 9].Text.Trim();
                    strOldJik = String.Format("{0:00}", ss1_Sheet1.Cells[i, 10].Text.Trim());

                    if (strDel == "True" && strRowid != "")
                    {
                        //GoSub DELETE_RTN 체크박스 체크 and strRowid Null 아니면 데이터 삭제
                        if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인
                        SQL = "DELETE NUR_CODE WHERE ROWID = '" + strRowid + "' ";
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
                    else if (strDel != "True" && strCode != "")
                    {
                        strOK = "NO";
                        if (strCode != strOldCode) strOK = "OK";
                        if (strName != strOldName) strOK = "OK";
                        if (nSeq != nOldSeq) strOK = "OK";
                        if (strJik != strOldJik) strOK = "OK";
                        if (strOK == "OK")
                        {
                            if (strRowid == "")
                            {
                                //GoSub INSERT_Rtn Rowid 없으면 insert
                                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "INSERT INTO NUR_CODE (Gubun,Code,Name,Jik,PrintRanking) VALUES ";
                                SQL += ComNum.VBLF + "('" + VB.Left(cboJong.Text, 1) + "','" + strCode;
                                SQL += ComNum.VBLF + "','" + strName + "','" + strJik + "'," + nSeq + ") ";
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
                            if (strRowid != "")
                            {
                                //GoSub Update_Rtn Update문
                                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "UPDATE NUR_CODE SET Code='" + strCode + "',";
                                SQL += ComNum.VBLF + "Name='" + strName + "', ";
                                SQL += ComNum.VBLF + "Jik = '" + strJik + "', ";
                                SQL += ComNum.VBLF + "PrintRanking = " + nSeq + " ";
                                SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
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
                Screen_Clear();
                btnView.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnExit.Enabled = true;
                cboJong.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void cboJong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void lst1_DoubleClick(object sender, EventArgs e)
        {
            int nRow = 0;
            string strDel = "";
            string strSabun1 = "";
            string strSabun2 = "";
            string strName = "";
            string strJik = "";

            strSabun1 = lst1.Text.Substring(0, 5);
            strName = lst1.Text.Substring(8, 10).Trim();
            strJik = VB.Right(lst1.Text, 2);

            for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
            {
                strDel = ss1_Sheet1.Cells[i, 0].Text.Trim();
                strSabun2 = ss1_Sheet1.Cells[i, 2].Text.Trim();
                if (strDel != "True" && strSabun1 == strSabun2)
                {
                    MessageBox.Show("이미 해당 사번이 등록됨", "확인");
                    return;
                }
                if ( strSabun2 == "")
                {
                    nRow = i;
                    break;
                }
            }

            if ( nRow == 0)
            {
                ss1_Sheet1.Rows.Count++;
                nRow = ss1_Sheet1.Rows.Count;
            }

            ss1_Sheet1.Cells[nRow, 2].Text = strSabun1;
            ss1_Sheet1.Cells[nRow, 3].Text = strName;
            ss1_Sheet1.Cells[nRow, 4].Text = strJik;
            ss1_Sheet1.Cells[nRow, 5].Text = Read_JikName(strJik);
            ss1_Sheet1.Cells[nRow, 6].Text = "";

        }
    }
}
