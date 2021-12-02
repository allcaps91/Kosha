using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmNrCode : Form
    {
        /// <summary>
        /// Class Name : frmNrCode
        /// File Name : frmNrCode.cs
        /// Title or Description : 간호부 각종 기초코드 등록 페이지
        /// Author : 박성완
        /// Create Date : 2017-06-05
        /// <history> 
        /// </history>
        /// </summary>
        public frmNrCode()
        {
            InitializeComponent();
        }


        private void frmNrCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //TODO: Call FormInfo_History(Me.Name, Me.Caption) 폼로딩 사용빈도 
            ss1_Sheet1.Columns[5].Visible = false;
            ss1_Sheet1.Columns[6].Visible = false;
            ss1_Sheet1.Columns[7].Visible = false;
            ss1_Sheet1.Columns[8].Visible = false;

            cboJong.Items.Clear();
            cboJong.Items.Add("1.직책코드");
            cboJong.Items.Add("2.병동코드");
            cboJong.Items.Add("3.주사분류");
            cboJong.Items.Add("4.근무형태");
            cboJong.Items.Add("5.외래부서");
            cboJong.Items.Add("6.특수검사");
            cboJong.Items.Add("7.가동병상수");
            cboJong.Items.Add("Z.특수,주사(파트별)");

            //TODO:임시사용한 Combo_BCode_SET 함수
            cboBCodeSET(cboJong, "NUR_검사항목", false, 1);

            cboJong.SelectedIndex = 0;

            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            ss1.Enabled = false;

        }

        //TODO:임시사용한 Combo_BCode_SET 함수
        #region Combo_BCode_SET -> cboBCodeSET 함수
        /// <summary>
        /// 자료사전을 이용하여 콤보박스 셋팅
        /// </summary>
        /// <param name="ArgCombobox"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgClear">True=Combobox를 Clear후 다른 자료를 Additem, False=Clear 안함</param>
        /// <param name="ArgTYPE">1=(코드) + "." + (명칭)형식 2: (코드) 3.(명칭)</param>
        /// <param name="ArgNULL">N = " " 제외</param>
        private void cboBCodeSET(ComboBox ArgCombobox, string ArgGubun, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ArgClear == true) { ArgCombobox.Items.Clear(); }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Sort,Code,Name FROM KOSMOS_PMPA.BAS_BCODE ";
            SQL += ComNum.VBLF + "WHERE Gubun='" + ArgGubun + "' ";
            SQL += ComNum.VBLF + "AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE))";
            SQL += ComNum.VBLF + "ORDER BY Sort,Code ";

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

            if (ArgNULL != "N") { ArgCombobox.Items.Add(" "); }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ArgTYPE == 1)
                { ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim()); }
                else if (ArgTYPE == 2)
                { ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim()); }
                else if (ArgTYPE == 3)
                { ArgCombobox.Items.Add(dt.Rows[i]["NAME"].ToString().Trim()); }
            }

            dt.Dispose();
            dt = null;
        }
        #endregion

        /// <summary>
        /// 화면 정리 함수
        /// </summary>
        private void ScreenClear()
        {
            ss1_Sheet1.Rows.Count = 0;
            ss1.Enabled = false;
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            grbJong.Enabled = true;
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
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
        /// 조회 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            if (ViewData() == false) return;
        }

        /// <summary>
        /// 조회하는 함수
        /// </summary>
        /// <returns></returns>
        private bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ScreenClear();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Code,Name,PrintRanking,ROWID ";
            SQL += ComNum.VBLF + " FROM NUR_CODE ";
            SQL += ComNum.VBLF + " WHERE Gubun = '" + VB.Left(cboJong.Text, 1) + "' ";
            SQL += ComNum.VBLF + " ORDER BY PrintRanking,Code ";

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
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Code"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                grbJong.Enabled = false;
                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;

                ss1_Sheet1.Rows.Count += 30;
                ss1.Enabled = true;
                ss1.Focus();
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
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
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCode = "";
            string strName = "";
            string strRowid = "";
            string strOldCode = "";
            string strOldName = "";
            string strDel = "";
            int nSeq = 0;
            int nOldSeq = 0;

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
                    //TODO:PrintRanking 열에 값이 Null값도 들어갈 수 있는지 확인필요. int형으로 하여 넣지 않으면 0으로 들어감
                    nSeq = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 3].Text));
                    strRowid = ss1_Sheet1.Cells[i, 5].Text.Trim();
                    strOldCode = ss1_Sheet1.Cells[i, 6].Text.Trim();
                    strOldName = ss1_Sheet1.Cells[i, 7].Text.Trim();
                    nOldSeq = Convert.ToInt32(VB.Val(ss1_Sheet1.Cells[i, 8].Text));

                    if (strDel == "True" && strRowid != "")
                    {
                        //GoSub DELETE_RTN 삭제작업
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
                        if ((strCode != strOldCode) || (strName != strOldName) || (nSeq != nOldSeq))
                        {
                            if (strRowid == "")
                            {
                                //GoSub INSERT_RTN 신규등록
                                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "INSERT INTO NUR_CODE (Gubun,Code,Name,PrintRanking) VALUES ";
                                SQL = SQL + ComNum.VBLF + "('" + VB.Left(cboJong.Text, 1) + "','" + strCode;
                                SQL = SQL + ComNum.VBLF + "','" + strName + "'," + nSeq + ") ";
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
                                //GoSub Update_RTN 수정등록
                                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                                SQL = "UPDATE NUR_CODE SET Code='" + strCode + "',";
                                SQL += ComNum.VBLF + "Name='" + strName + "', ";
                                SQL += ComNum.VBLF + "PrintRanking =" + nSeq + " ";
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
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                ScreenClear();
                grbJong.Enabled = true;
                btnView.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnExit.Enabled = true;
                cboJong.Focus();
                return true;
            }
            catch (Exception ex)
            {
                //TODO: 글로벌 변수 에러로그 저장 및 플레그. 검토 필요
                //gstrERROR_SQL = SQL;
                //GstrERROR_FLAG = "*";
                //Call ERROR_CALL (nrcode00.bas 에 있는 함수)
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 사용자가 셀에서 나올 때 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ss1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strSuNext = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            //해당셀이 2번째 열 아니면 return
            if (e.Column != 1)
            {
                return;
            }

            strSuNext = ss1_Sheet1.Cells[e.Row, e.Column].Text;

            if (strSuNext.Trim() == "")
            {
                return;
            }

            switch (VB.Left(cboJong.Text, 1))
            {
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                    SQL = "SELECT SUNAMEK FROM BAS_SUN WHERE SUNEXT = '" + strSuNext + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (dt.Rows.Count == 0)
                    {
                        ss1_Sheet1.Cells[e.Row, 1].Text = "";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[e.Row, 2].Text = dt.Rows[0]["SUNAMEK"].ToString();
                    }

                    dt.Dispose();
                    dt = null;
                    break;
                default:
                    break;
            }
        }
    }
}
