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
    /// Class Name : frmPassEntry
    /// File Name : frmPassEntry.cs
    /// Title or Description : 작업자 사번 등록 페이지
    /// Author : 박성완
    /// Create Date : 2017-06-07
    /// <history> 
    /// 2017-06-20 사번 찾는 폼의 데이터 가져오는 이벤트 추가
    /// </history>
    /// </summary>
    public partial class frmPassEntry : Form
    {

        private frmSearchSabun frmSearchSabunX;

        string FstrKorName = "";
        string FstrBuseName = "";
        string FstrJikName = "";
        string FstrIpsaDay = "";
        string FstrBalDay = "";
        string FstrToiDay = "";
        string FstrPassWord = "";

        public frmPassEntry()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            txtSabun.Enabled = true;
            txtSabun.Text = "";
            txtSname.Text = "";
            txtPart.Text = "";
            cboClass.Text = "";
            txtCharge.Text = "";
            cboGrade.Text = "";
            //인사사항 Clear
            ss1_Sheet1.ClearRange(0, 1, ss1_Sheet1.Rows.Count, 1, true);
            //업무권한 가능여부 Clear
            for (int i = 0; i < ss2_Sheet1.Rows.Count; i++)
            {
                ss2_Sheet1.Cells[i, 0].Text = " ";
                ss2_Sheet1.Cells[i, 1].Text = " ";
                ss2_Sheet1.Cells[i, 2].Text = " ";
                ss2_Sheet1.Cells[i, 3].Text = " ";
                //숨겨진 열
                ss2_Sheet1.Cells[i, 6].Text = " ";
                ss2_Sheet1.Cells[i, 7].Text = " ";
                ss2_Sheet1.Cells[i, 8].Text = " ";
                ss2_Sheet1.Cells[i, 9].Text = " ";
                ss2_Sheet1.Cells[i, 10].Text = " ";
            }

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnRoll.Enabled = true;
        }

        void Roll_Setting()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //업무권한별 목록 SELECT
            SQL = "SELECT ProgramID,Name FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID > ' '";
            SQL = SQL + ComNum.VBLF + "  AND IdNumber = 0 ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ProgramID ";
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
            ss2_Sheet1.Rows.Count = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss2_Sheet1.Cells[i, 0].Text = " ";
                ss2_Sheet1.Cells[i, 1].Text = " ";
                ss2_Sheet1.Cells[i, 2].Text = " ";
                ss2_Sheet1.Cells[i, 3].Text = " ";
                ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ProgramID"].ToString().Trim();
                ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Name"].ToString().Trim();
                ss2_Sheet1.Cells[i, 6].Text = " ";
                ss2_Sheet1.Cells[i, 7].Text = " ";
                ss2_Sheet1.Cells[i, 8].Text = " ";
                ss2_Sheet1.Cells[i, 9].Text = " ";
            }

        }

        void Password_Display()
        {
            long nSabun;
            string strProgID;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nSabun = (long)VB.Val(txtSabun.Text);
            if (nSabun == 0)
            {
                return;
            }

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            txtSabun.Enabled = false;
            btnRoll.Enabled = false;

            //BAS_PASS READ
            SQL = "SELECT * FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
            SQL = SQL + ComNum.VBLF + "  AND IdNumber = " + nSabun + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if ( dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                INSA_READ(nSabun);
                ss1_Sheet1.Cells[0, 1].Text = FstrIpsaDay;
                ss1_Sheet1.Cells[1, 1].Text = FstrBalDay;
                ss1_Sheet1.Cells[2, 1].Text = FstrToiDay;
                ss1_Sheet1.Cells[3, 1].Text = FstrBuseName;
                ss1_Sheet1.Cells[4, 1].Text = FstrJikName;
                cboClass.SelectedIndex = 2;
                cboGrade.SelectedIndex = 3;
                txtSname.Text = FstrKorName;
                MessageBox.Show("신규코드 입니다", "확인");
                return;
            }

            txtSname.Text = dt.Rows[0]["Name"].ToString().Trim();
            FstrPassWord = dt.Rows[0]["PassWard"].ToString().Trim();
            txtPart.Text = dt.Rows[0]["Part"].ToString().Trim();
            txtCharge.Text = dt.Rows[0]["Charge"].ToString().Trim();
            cboClass.Text = dt.Rows[0]["Class"].ToString().Trim();
            cboGrade.Text = dt.Rows[0]["Grade"].ToString().Trim();
            dt.Dispose();
            dt = null;

            INSA_READ(nSabun);
            ss1_Sheet1.Cells[0, 1].Text = FstrIpsaDay;
            ss1_Sheet1.Cells[1, 1].Text = FstrBalDay;
            ss1_Sheet1.Cells[2, 1].Text = FstrToiDay;
            ss1_Sheet1.Cells[3, 1].Text = FstrBuseName;
            ss1_Sheet1.Cells[4, 1].Text = FstrJikName;

            //업무권한별 가능여부 SET
            SQL = "SELECT ProgramID, privilege1, privilege2, privilege3, privilege4, rowid  FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE IdNumber = " + nSabun + " ";
            SQL = SQL + ComNum.VBLF + "  AND ProgramID > ' ' ";
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
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strProgID = dt.Rows[i]["ProgramID"].ToString().Trim();
                    for (int j = 0; j < ss2_Sheet1.Rows.Count; j++)
                    {
                        if (strProgID == ss2_Sheet1.Cells[j, 4].ToString())
                        {
                            ss2_Sheet1.Cells[j, 0].Text = "Y"; //데이터가 있으면 조회 권한 있는것으로 됨
                            ss2_Sheet1.Cells[j, 6].Text = "Y";

                            if (dt.Rows[i]["privilege1"].ToString() == "Y")
                            {
                                ss2_Sheet1.Cells[j, 0].Text = "Y";
                                ss2_Sheet1.Cells[j, 6].Text = "Y";
                            }

                            if (dt.Rows[i]["privilege2"].ToString() == "Y")
                            {
                                ss2_Sheet1.Cells[j, 1].Text = "Y";
                                ss2_Sheet1.Cells[j, 7].Text = "Y";
                            }

                            if (dt.Rows[i]["privilege3"].ToString() == "Y")
                            {
                                ss2_Sheet1.Cells[j, 2].Text = "Y";
                                ss2_Sheet1.Cells[j, 8].Text = "Y";
                            }

                            if (dt.Rows[i]["privilege4"].ToString() == "Y")
                            {
                                ss2_Sheet1.Cells[j, 3].Text = "Y";
                                ss2_Sheet1.Cells[j, 9].Text = "Y";
                            }
                        }
                        ss2_Sheet1.Cells[j, 10].Text = dt.Rows[i]["rowid"].ToString().Trim();
                    }
                }

                //SS2를 SORT하여 권한이 있는것을 상단에 표시함
            }
            dt.Dispose();
            dt = null;

            btnDelete.Enabled = true;
        }

        void INSA_READ(long argSabun)
        {
            string strBuseCode = "";
            string strJikCode = "";
            string strKorName = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

             FstrKorName = "";
             FstrBuseName = "";
             FstrJikName = "";
             FstrIpsaDay = "";
             FstrBalDay = "";
             FstrToiDay = "";
             FstrPassWord = "";

            //인사 개인정보READ
            SQL = "SELECT KorName,Buse,Jik,TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay,";
            SQL = SQL + ComNum.VBLF + "TO_CHAR(BalDay,'YYYY-MM-DD') BalDay,";
            SQL = SQL + ComNum.VBLF + "TO_CHAR(ToiDay,'YYYY-MM-DD') ToiDay ";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.INSA_MST ";
            SQL = SQL + ComNum.VBLF + "WHERE Sabun = '" + string.Format("{0:00000}",argSabun) + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                strKorName = dt.Rows[0]["KorName"].ToString().Trim() as string;
                strBuseCode = dt.Rows[0]["Buse"].ToString().Trim() as string;
                strJikCode = dt.Rows[0]["Jik"].ToString().Trim() as string;
                FstrIpsaDay = dt.Rows[0]["IpsaDay"].ToString().Trim() as string;
                FstrBalDay = dt.Rows[0]["BalDay"].ToString().Trim() as string;
                FstrToiDay = dt.Rows[0]["ToiDay"].ToString().Trim() as string;
            }

            dt.Dispose();
            dt = null;

            //부서명 READ
            SQL = "SELECT Name FROM ADMIN.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + "WHERE BuCode = '" + strBuseCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)  { FstrBuseName = dt.Rows[0]["Name"].ToString().Trim() as string; }
            dt.Dispose();
            dt = null;

            //직책명 READ
            SQL = "SELECT Name FROM ADMIN.INSA_CODE ";
            SQL = SQL + ComNum.VBLF + "WHERE Gubun = '2' ";
            SQL = SQL + ComNum.VBLF + "  AND Code = '" + strJikCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)  { FstrJikName = dt.Rows[0]["Name"].ToString().Trim() as string; }
            dt.Dispose();
            dt = null;

            //성명 중간의 공란 제거

            FstrKorName = strKorName.Replace(" ", "");
            

            //for (int i = 0; i < strKorName.Length; i++)
            //{
            //    if (strKorName.Substring(i, 1) != " ") 
            //    {
            //        FstrKorName = FstrKorName + strKorName.Substring(i, 1) as string;
            //    }
            //}
        }

        void frmPassEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            string[] cboClassItem = 
            {
                      "ALL"
                     ,"PMPA_ALL"
                     ,"PMPA_IPD"
                     ,"PMPA_OPD"
                     ,"PMPA_MIR"
                     ,"PMPA_MID"
                     ,"PMPA_XRY"
                     ,"PMPA_DIE"
                     ,"ADM_ALL"
                     ,"ADM_PAY"
                     ,"ADM_DRG"
                     ,"ADM_ORD"
                     ,"ADM_AIS"
                     ,"OCS"
            };
            string[] cboGradeItem = 
            {
                      "DIRECTOR"
                     ,"TOP"
                     ,"MANAGER"
                     ,"CASHER"
                     ,"ENTRY"
                     ,"EMPLOYEE"
                     ,"EDPS"
            };

            txtViewData.Text = "";
            Screen_Clear();

            cboClass.Items.Clear();
            cboGrade.Items.Clear();
            cboClass.Items.AddRange(cboClassItem);
            cboGrade.Items.AddRange(cboGradeItem);

            Roll_Setting();
        }

        private void btnSabun_Click(object sender, EventArgs e)
        {
            //닫는 이벤트 내용
            if (frmSearchSabunX != null)
            {
                frmSearchSabunX.Dispose();
                frmSearchSabunX = null;
            }
            frmSearchSabunX = new frmSearchSabun();
            frmSearchSabunX.rGetSabunData += FrmSearchSabunX_GetSabunData;
            frmSearchSabunX.rEventClose += FrmSearchSabunX_EventClose;
            frmSearchSabunX.Show();
        }

        private void FrmSearchSabunX_EventClose()
        {
            //닫는 이벤트 내용
            if (frmSearchSabunX != null)
            {
                frmSearchSabunX.Dispose();
                frmSearchSabunX = null;
            }
        }

        private void FrmSearchSabunX_GetSabunData(string strSabun)
        {
            if (strSabun != "")
            {
                txtSabun.Text = strSabun;
                frmSearchSabunX.Leave += txtSabun_Leave;
            }
        }

        private void txtSabun_Leave(object sender, EventArgs e)
        {
            if (VB.Val(txtSabun.Text) == 0)
            {
                return;
            }

            Password_Display();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            txtSabun.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteData() == false) return;
        }

        bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            int nSabun = 0;
            string strNewData = "";
            string strRoll = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            nSabun = (int)VB.Val(txtSabun.Text);

            clsPublic.GstrMsgList = txtSabun.Text + " " + txtSname.Text + " 비밀번호와 관련된 모든 자료를";
            clsPublic.GstrMsgList += "정말로 삭제 하시겠습니까?";
            if (MessageBox.Show(clsPublic.GstrMsgList, "삭제여부", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //자료 삭제
                SQL = "DELETE BAS_PASS WHERE IdNumber=" + VB.Val(txtSabun.Text) + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                //업무권한 DB에 저장
                for (int i = 0; i < ss2_Sheet1.Rows.Count; i++)
                {
                    strNewData = ss2_Sheet1.Cells[i, 0].ToString().Trim();
                    strRoll = ss2_Sheet1.Cells[i, 1].ToString().Trim();

                    SQL = "INSERT INTO BAS_ROLL_LOG (JOBDATE, GUBUN, ProgramID,IdNumber,Name, Entdate, CLASS, GRADE ) VALUES ( TRUNC(SYSDATE) , 'D' , '";
                    SQL = SQL + ComNum.VBLF + "' , " + nSabun + ",'" + (txtSname.Text).Trim() + "', SYSDATE, '" + cboClass.Text + "', '" + cboGrade.Text + "'  )  ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtSabun.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
        }

        bool SaveData()
        {
            long nSabun = 0;
            string strROWID = "";
            string strROLL = "";
            string strSabun = "";
            string strPrivilege1 = "";
            string strPrivilege2 = "";
            string strPrivilege3 = "";
            string strPrivilege4 = "";

            string strPrivilege1_old = "";
            string strPrivilege2_old = "";
            string strPrivilege3_old = "";
            string strPrivilege4_old = "";

            string strGubun = "";
            string strJobDate = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            nSabun = (long)VB.Val(txtSabun.Text);
            //GoSub Data_Error_Check
            txtSname.Text = txtSname.Text.Trim();

            if (txtSname.Text == "") 
            {
                MessageBox.Show("성명이 공란입니다.", "성명오류");
                return false;
            }

            if (cboClass.Text.Trim() == "")
            {
                MessageBox.Show("작업범위가 공란입니다.", "오류");
            }

            if (cboGrade.Text.Trim() == "")
            {
                MessageBox.Show("관리구분이 공란입니다.", "오류");
            }

            //책임구분 체크
            switch (txtCharge.Text.Trim())
            {
                case "":
                case "0":
                case "1":
                case "R":
                case "I":
                case "X":
                case "S":
                    break;
                default:
                    MessageBox.Show("책임구분은 '','0','1','R','I','X','S'만 가능함", "책임구분 오류");
                    return false;
            }

            //조가 공란이면 체크 않함
            if(txtPart.Text.Trim() == "") { goto Return; }

            //작업조 다른사람이 사용여부 체크
            SQL = "SELECT IdNumber,Name FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
            SQL = SQL + ComNum.VBLF + "  AND IdNumber <> " + nSabun + " ";
            SQL = SQL + ComNum.VBLF + "  AND Part = '" + txtPart.Text + "' ";
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
            if (dt.Rows.Count > 0)
            {
                clsPublic.GstrMsgList = "(" + txtPart.Text + ")조는 이미 사번:";
                clsPublic.GstrMsgList += dt.Rows[0]["IdNumber"].ToString() + " 성명:";
                clsPublic.GstrMsgList += dt.Rows[0]["Name"].ToString().Trim() + ComNum.VBLF;
                clsPublic.GstrMsgList += "사용중입니다. 확인 하십시오.";
                MessageBox.Show(clsPublic.GstrMsgList, "작업조 오류");
                dt.Dispose();
                dt = null;
                return false;
            }
            dt.Dispose();
            dt = null;

        //Data_Error_Check에서의 리턴
        Return:

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //INSERT,UPDATE여부
                SQL = "SELECT ROWID FROM ADMIN.BAS_PASS ";
                SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
                SQL = SQL + ComNum.VBLF + "  AND IdNumber = " + nSabun + " ";
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
                strROWID = "";
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString();
                }
                dt.Dispose();
                dt = null;

                if (FstrPassWord == "")
                {
                    FstrPassWord = nSabun.ToString();
                }
                
                //자료가 없으면 INSERT
                if (strROWID == "")
                {
                    SQL = "INSERT INTO BAS_PASS (ProgramID,IdNumber,Name,Class,";
                    SQL = SQL + ComNum.VBLF + "Grade,Charge,Part,PassWard) VALUES (' '," + nSabun + ",'";
                    SQL = SQL + txtSname.Text.Trim() + "','";
                    SQL = SQL + cboClass.Text.Trim() + "','" + cboGrade.Text.Trim() + "','";
                    SQL = SQL + txtCharge.Text.Trim() + "','" + txtPart.Text.Trim() + "','" + FstrPassWord + "') ";
                }
                else
                {
                    SQL = "UPDATE BAS_PASS SET ";
                    SQL = SQL + ComNum.VBLF + "Name= '" + txtSname.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "Class= '" + cboClass.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "Grade= '" + cboGrade.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "Charge= '" + txtCharge.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "Part= '" + txtPart.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                //업무권한 DB에 저장
                for (int i = 0; i < ss2_Sheet1.Rows.Count; i++)
                {
                    strPrivilege1 = ss2_Sheet1.Cells[i, 0].Text.Trim();
                    strPrivilege2 = ss2_Sheet1.Cells[i, 1].Text.Trim();
                    strPrivilege3 = ss2_Sheet1.Cells[i, 2].Text.Trim();
                    strPrivilege4 = ss2_Sheet1.Cells[i, 3].Text.Trim();

                    strROLL = ss2_Sheet1.Cells[i, 4].Text.Trim();
                    strPrivilege1_old = ss2_Sheet1.Cells[i, 6].Text.Trim();
                    strPrivilege2_old = ss2_Sheet1.Cells[i, 7].Text.Trim();
                    strPrivilege3_old = ss2_Sheet1.Cells[i, 8].Text.Trim();
                    strPrivilege4_old = ss2_Sheet1.Cells[i, 9].Text.Trim();
                    strROWID = ss2_Sheet1.Cells[i, 10].Text.Trim();

                    if ( strPrivilege1 + strPrivilege2 + strPrivilege3 + strPrivilege4 != strPrivilege1_old + strPrivilege2_old + strPrivilege3_old + strPrivilege4_old)
                    {
                        if ( strROWID != "")
                        {
                            //삭제
                            if (strPrivilege1 == "" && strPrivilege2 == "" && strPrivilege3 == "" && strPrivilege4 == "")
                            {
                                strGubun = "D";
                                SQL = "DELETE BAS_PASS ";
                                SQL = SQL + ComNum.VBLF + "WHERE IdNumber=" + nSabun + " ";
                                SQL = SQL + ComNum.VBLF + "  AND ProgramID = '" + strROLL + "' ";
                            }
                            else
                            {
                                strGubun = "U";
                                SQL = "UPDATE  BAS_PASS SET ";
                                SQL = SQL + ComNum.VBLF + " Privilege1 = '" + strPrivilege1 + "' ,";
                                SQL = SQL + ComNum.VBLF + " Privilege2 = '" + strPrivilege2 + "' ,";
                                SQL = SQL + ComNum.VBLF + " Privilege3 = '" + strPrivilege3 + "' , ";
                                SQL = SQL + ComNum.VBLF + " Privilege4 = '" + strPrivilege4 + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE IdNumber=" + nSabun + " ";
                                SQL = SQL + ComNum.VBLF + "  AND ProgramID = '" + strROLL + "' ";
                            }
                        }
                        else
                        {
                            strGubun = "I";
                            SQL = "INSERT INTO BAS_PASS (ProgramID,IdNumber,Name,Passward, Privilege1, Privilege2, Privilege3, Privilege4 ) VALUES ('";
                            SQL = SQL + ComNum.VBLF + strROLL + "'," + nSabun + ",'" + txtSname.Text.Trim() + "','" + FstrPassWord + "', '" + strPrivilege1 + "'  , '" + strPrivilege2 + "', '" + strPrivilege3 + "', '" + strPrivilege4 + "'";
                            SQL = SQL + ComNum.VBLF + ") ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        strJobDate = ss1_Sheet1.Cells[2, 1].Text;

                        //신규
                        if (strGubun == "I")
                        {
                            SQL = "INSERT INTO BAS_ROLL_LOG (JOBDATE, GUBUN, ProgramID,IdNumber,Name, Entdate, CLASS, GRADE, Privilege1, Privilege2, Privilege3, Privilege4  ) VALUES ( TRUNC(SYSDATE) ,  'I' , '";
                            SQL = SQL + ComNum.VBLF + strROLL + "' , " + nSabun + ",'" + txtSname.Text.Trim() + "', SYSDATE , '" + cboClass.Text + "', '" + cboGrade.Text + "', '" + strPrivilege1_old + "'  , '" + strPrivilege2_old + "', '" + strPrivilege3_old + "', '" + strPrivilege4_old + "'  )  ";
                        }
                        //삭제
                        else if (strGubun == "D")
                        {
                            SQL = "INSERT INTO BAS_ROLL_LOG (JOBDATE, GUBUN, ProgramID,IdNumber,Name, Entdate, CLASS, GRADE, Privilege1, Privilege2, Privilege3, Privilege4  ) VALUES ( TRUNC(SYSDATE) , 'D' , '";
                            SQL = SQL + ComNum.VBLF + strROLL + "' , " + nSabun + ",'" + txtSname.Text.Trim() + "', SYSDATE, '" + cboClass.Text + "', '" + cboGrade.Text + "', '" + strPrivilege1_old + "'  , '" + strPrivilege2_old + "', '" + strPrivilege3_old + "', '" + strPrivilege4_old + "'  )  ";
                        }
                        else
                        {
                            SQL = "INSERT INTO BAS_ROLL_LOG (JOBDATE, GUBUN, ProgramID,IdNumber,Name, Entdate, CLASS, GRADE, Privilege1, Privilege2, Privilege3, Privilege4  ) VALUES ( TRUNC(SYSDATE) ,  'U' , '";
                            SQL = SQL + ComNum.VBLF + strROLL + "' , " + nSabun + ",'" + txtSname.Text.Trim() + "', SYSDATE,'" + cboClass.Text + "', '" + cboGrade.Text + "', '" + strPrivilege1_old + "'  , '" + strPrivilege2_old + "', '" + strPrivilege3_old + "' , '" + strPrivilege4_old + "'  )  ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                }

                strSabun = String.Format("{0:00000}", nSabun);
                if (nSabun > 99999)
                {
                    strSabun = String.Format("{0:000000}", nSabun);
                }
                //WEB 서버로 전송하기 위하여 INSA_MST에 업데이트
                SQL = " UPDATE ADMIN.INSA_MST SET WebSend='*' ";
                SQL = SQL + ComNum.VBLF + " WHERE Sabun='" + strSabun + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();
                txtSabun.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            //TODO:frmPassRollEntry.show 1
            Roll_Setting();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            //TODO:모달인지 그냥인지
            frmPassAuto frm = new frmPassAuto();
            frm.Show();
        }

        private void btnPart_Click(object sender, EventArgs e)
        {
            //TODO:frmPart.Show
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            long nSabun = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            txtViewData.Text = txtViewData.Text.Trim();

            ss3.Enabled = true;
            ss3_Sheet1.Rows.Count = 30;
            ss3_Sheet1.ClearRange(0, 0, ss3_Sheet1.Rows.Count, ss3_Sheet1.Columns.Count, true);

            //BAS_PASS를 READ
            SQL = "SELECT * FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
            if (txtViewData.Text != "")
            {
                SQL = SQL + ComNum.VBLF + " AND Name LIKE '" + txtViewData.Text + "%' ";
            }
            if(optSort0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY Name ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "ORDER BY IdNumber ";
            }
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
            if (dt.Rows.Count > 0)
            {
                ss3_Sheet1.Rows.Count = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    nSabun = long.Parse(dt.Rows[i]["IdNumber"].ToString() as string);
                    ss3_Sheet1.Cells[i, 0].Text = nSabun.ToString();
                    ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Passward"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Class"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Grade"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Charge"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Part"].ToString().Trim();
                    INSA_READ(nSabun);
                    ss3_Sheet1.Cells[i, 7].Text = FstrBuseName;
                    ss3_Sheet1.Cells[i, 8].Text = FstrJikName;
                    ss3_Sheet1.Cells[i, 9].Text = FstrIpsaDay;
                    ss3_Sheet1.Cells[i, 10].Text = FstrToiDay;
                    ss3_Sheet1.Cells[i, 11].Text = FstrBalDay;
                }
            }
            dt.Dispose();
            dt = null;
            btnPrint.Enabled = true;
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

            strTitle = "작업자 등록 현황(BAS_PASS)";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ss3, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnPartView_Click(object sender, EventArgs e)
        {
            long nSabun = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss3.Enabled = true;
            ss3_Sheet1.Rows.Count = 20;
            ss3_Sheet1.ClearRange(0, 0, ss3_Sheet1.Rows.Count, ss3_Sheet1.Columns.Count, true);

            SQL = "SELECT * FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
            SQL = SQL + ComNum.VBLF + "  AND Part > ' ' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Part,IdNumber ";
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

            if ( dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nSabun = long.Parse(dt.Rows[i]["IdNumber"].ToString());
                    ss3_Sheet1.Cells[i, 0].Text = nSabun.ToString();
                    ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Passward"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Class"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Grade"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Charge"].ToString().Trim();
                    ss3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Part"].ToString().Trim();
                    INSA_READ(nSabun);
                    ss3_Sheet1.Cells[i, 7].Text = FstrBuseName;
                    ss3_Sheet1.Cells[i, 8].Text = FstrJikName;
                    ss3_Sheet1.Cells[i, 9].Text = FstrIpsaDay;
                    ss3_Sheet1.Cells[i, 10].Text = FstrToiDay;
                    ss3_Sheet1.Cells[i, 11].Text = FstrBalDay;
                }
            }

            dt.Dispose();
            dt = null;
            btnPrint.Enabled = true;

        }

        private void btnToiView_Click(object sender, EventArgs e)
        {
            long nSabun = 0;
            string strOK = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            ss3.Enabled = true;
            ss3_Sheet1.Rows.Count = 30;
            ss3_Sheet1.ClearRange(0, 0, ss3_Sheet1.Rows.Count, ss3_Sheet1.Columns.Count, true);

            SQL = "SELECT * FROM ADMIN.BAS_PASS ";
            SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
            SQL = SQL + ComNum.VBLF + "  AND IDNUMBER IN ( SELECT SABUN FROM ADMIN.INSA_MST WHERE TOIDAY IS NOT NULL )";
            SQL = SQL + ComNum.VBLF + "ORDER BY Name ";

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
            int nRow = 0;
            if ( dt.Rows.Count >0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "NO";
                    nSabun = long.Parse(dt.Rows[i]["IdNumber"].ToString());

                    SQL = "SELECT * FROM ADMIN.BAS_PASS ";
                    SQL = SQL + ComNum.VBLF + "WHERE ProgramID = ' ' ";
                    SQL = SQL + ComNum.VBLF + "  AND IDNUMBER ='" + nSabun + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND (PRIVIlEGE1 ='Y' OR PRIVIlEGE2 ='Y' OR PRIVIlEGE3 ='Y' OR PRIVIlEGE4 ='Y'  ) ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

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

                    if (dt2.Rows.Count > 0)
                    {
                        strOK = "OK";
                    }

                    dt2.Dispose();
                    dt2 = null;

                    if (strOK == "OK")
                    {
                        INSA_READ(nSabun);
                        if (FstrToiDay != "")
                        {
                            strOK = "OK";
                        }
                    }


                    if ( strOK == "OK")
                    {
                        nRow += 1;
                        if (nRow > ss3_Sheet1.Rows.Count)
                        {
                            ss3_Sheet1.Rows.Count = nRow;
                        }
                        ss3_Sheet1.Cells[nRow - 1, 0].Text = nSabun.ToString();
                        ss3_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Passward"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Class"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Grade"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Charge"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Part"].ToString().Trim();
                        ss3_Sheet1.Cells[nRow - 1, 7].Text = FstrBuseName;
                        ss3_Sheet1.Cells[nRow - 1, 8].Text = FstrJikName;
                        ss3_Sheet1.Cells[nRow - 1, 9].Text = FstrIpsaDay;
                        ss3_Sheet1.Cells[nRow - 1, 10].Text = FstrToiDay;
                        ss3_Sheet1.Cells[nRow - 1, 11].Text = FstrBalDay;
                    }
                }
                ss3_Sheet1.Rows.Count = nRow;
            }
            dt.Dispose();
            dt = null;
            btnPrint.Enabled = true;
        }

        private void ss2_Change(object sender, ChangeEventArgs e)
        {
            if (e.Column != 0 || e.Column != 1 || e.Column != 2 || e.Column != 3)
            {
                return;
            }

            ss2_Sheet1.Cells[e.Row, e.Column].Text = ss2_Sheet1.Cells[e.Row, e.Column].Text.Trim().ToUpper();

            if (ss2_Sheet1.Cells[e.Row, e.Column].Text != "Y" && ss2_Sheet1.Cells[e.Row, e.Column].Text != "")
            {
                MessageBox.Show("가능여부: Y=권한있음 공란=권한없음", "오류");
                ss2_Sheet1.Cells[e.Row, e.Column].Text = "";
            }
        }

        private void ss3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if(ss3_Sheet1.Cells[e.Row, 0].Text.Trim() != "")
            {
                Screen_Clear();
                txtSabun.Text = ss3_Sheet1.Cells[e.Row, 0].Text;
                Password_Display();
                txtSname.Focus();
            }
        }

        /// <summary>
        /// 콘트롤들의 KeyPress 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtCharge_Leave(object sender, EventArgs e)
        {
            txtCharge.Text = txtCharge.Text.Trim().ToUpper();
        }

        private void txtPart_Leave(object sender, EventArgs e)
        {
            txtPart.Text = txtPart.Text.Trim().ToUpper();           
        }
    }
}
