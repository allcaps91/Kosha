using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmQiNrCode
    /// File Name : frmQiNrCode.cs
    /// Title or Description : 각종 기초코드 등록 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-02
    /// <history> 
    /// </history>
    /// </summary>
    public partial class frmQiNrCode : Form
    {
        public frmQiNrCode()
        {
            InitializeComponent();
        }
        
        //폼 사용하는지 확인 필요. 쿼리를 날려도 데이터 조회가 되지않음.

        /// <summary>
        /// 폼 로드 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmQiNrCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등
            //TODO: Call FormInfo_History(Me.Name, Me.Caption) 폼로딩 사용빈도 
            ss1_Sheet1.Columns[4].Visible = false;
            ss1_Sheet1.Columns[5].Visible = false;
            ss1_Sheet1.Columns[6].Visible = false;
            ss1_Sheet1.Columns[7].Visible = false;

            cboJong.Items.Clear();
            cboJong.Items.Add("1.부서(팀)명");
            cboJong.SelectedIndex = 0;

            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            ss1.Enabled = false;
        }

        /// <summary>
        /// 화면 정리 함수
        /// </summary>
        private void ScreenClear()
        {
            ss1_Sheet1.Rows.Count = 0;
            ss1.Enabled = false;
        }

        /// <summary>
        /// 조회 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            string SqlErr = "";
            DataTable dt = null;

            ScreenClear();

            SQL = "";
            SQL += "SELECT Code,Name,SORT,ROWID ";
            SQL += ComNum.VBLF + " FROM NUR_QI_CODE ";
            SQL += ComNum.VBLF + " WHERE Gubun = '" + VB.Left(cboJong.Text, 1) + "' ";
            SQL += ComNum.VBLF + " ORDER BY Code ";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
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
                    ss1_Sheet1.Cells[i, 0].Text = "";
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SORT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SORT"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;
                grbJong.Enabled = false;

                ss1_Sheet1.Rows.Count += 30;
                ss1.Enabled = true;
                ss1.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            grbJong.Enabled = true;
            cboJong.Focus();
        }

        /// <summary>
        /// 닫기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 등록 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        /// <summary>
        /// 등록 함수
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCode = "";
            string strName = "";
            string strRowid = "";
            string strOldCode = "";
            string strOldName = "";
            string strDel = "";
            int nSort = 0;
            int nOldSort = 0;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    strDel = ss1_Sheet1.Cells[i, 0].Text.Trim();
                    strCode = ss1_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ss1_Sheet1.Cells[i, 2].Text.Trim();
                    nSort = Convert.ToInt32(ss1_Sheet1.Cells[i, 3]);
                    strRowid = ss1_Sheet1.Cells[i, 4].Text.Trim();
                    strOldCode = ss1_Sheet1.Cells[i, 5].Text.Trim();
                    strOldName = ss1_Sheet1.Cells[i, 6].Text.Trim();
                    nOldSort = Convert.ToInt32(ss1_Sheet1.Cells[i, 7]);

                    if (strDel == "1" && strRowid != "")
                    {
                        //GoSub DELETE_RTN 삭제작업
                        SQL = "DELETE NUR_QI_CODE WHERE ROWID = '" + strRowid + "' ";
                    }
                    else if (strDel != "1" && strCode != "")
                    {
                        if (strCode != strOldCode || strName != strOldName || nSort != nOldSort)
                        {
                            if (strRowid == "")
                            {
                                //GoSub INSERT_RTN 신규등록
                                SQL = "INSERT INTO NUR_QI_CODE (Gubun,Code,Name,SORT) VALUES ";
                                SQL = SQL + ComNum.VBLF + "('" + VB.Left(cboJong.Text, 1) + "','" + strCode;
                                SQL = SQL + ComNum.VBLF + "','" + strName + "'," + nSort + ") ";
                            }
                            else if (strRowid != "")
                            {
                                //GoSub Update_RTN 수정등록
                                SQL = "UPDATE NUR_QI_CODE SET Code='" + strCode + "',";
                                SQL += ComNum.VBLF + "Name='" + strName + "', ";
                                SQL += ComNum.VBLF + "SORT='" + nSort + "', ";
                                SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                            }
                        }
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 사용자가 셀에서 떠났을 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ss1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strCode = "";
            string strName = "";

            //수술재료대여업체
            if (e.Column != 2 || VB.Left(cboJong.Text, 1) != "7")
            {
                return;
            }

            strCode = ss1_Sheet1.Cells[e.Row, e.Column].Text.Trim();

            if(strCode == "")
            {
                return;
            }

            //TODO: READ_AIS_LTD 함수 임시 사용
            strName = ReadAISLTD(strCode);
            if (strName == "")
            {
                ComFunc.MsgBox(strCode + " 코드가 거래처코드에 등록이 안됐습니다.", "확인");
                ss1_Sheet1.Cells[e.Row, e.Column].Text = "";
                return;
            }
            ss1_Sheet1.Cells[e.Row, 2].Text = strName;
        }

        //TODO: READ_AIS_LTD 함수 임시 사용
        /// <summary>
        /// 거래처명칭
        /// </summary>
        /// <param name="argCode">LTD Code</param>
        /// <returns></returns>
        private string ReadAISLTD(string argCode)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;
            

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Name FROM ADMIN.AIS_LTD ";
            SQL += ComNum.VBLF + "WHERE LtdCode='" + argCode.Trim() + "' ";
            try
            {
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }
                if (dt.Rows.Count >0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString();
                }
                else
                {
                    rtnVal = "";
                }

                return rtnVal;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
    }
}
