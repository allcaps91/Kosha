using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmGamF.cs
    /// Description     : 감액 대상자 관리
    /// Author          : 박성완
    /// Create Date     : 2017-06-20
    /// </summary>
    /// <history>  
    /// 2017-06-29 빈약한 함수 및 이벤트 정리 -박성완
    /// </history>
    /// <seealso> 
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bugamf\bugamf.frm
    /// seealso : 
    /// </vbp>
    public partial class frmGamF : Form
    {
        #region 필드,enum
        string strRowid = "";
        string strSabun = "";
        string strJumin1 = "";
        string strJumin2 = "";


        enum ss1_Col { Delete, Jumin, Gamsabun, Gamcode, Gamenter, Gamout, Gamend, Gammessage, Gamname, Bucode, Rowid }

        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        public frmGamF()
        {
            InitializeComponent();

            setEvent();
        }

        #region 메소드

        void BAS_GAMF_JuminNo_Select()
        {
            string GamJumin = "";
            string SQL = "";
            string SqlError = "";
            DataTable dt = null;

            GamJumin = txtJumin1.Text + txtJumin2.Text;

            try
            {
                SQL = "   SELECT   gamjumin, gamsabun,   gamcode, ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(gamenter,'YYYY-MM-DD')gamenter, ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(gamout,'YYYY-MM-DD')gamout, ";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(gamend,'YYYY-MM-DD')gamend, ";
                SQL = SQL + ComNum.VBLF + "             gammessage, gamname,  gamsosok, rowid ";
                SQL = SQL + ComNum.VBLF + " FROM        BAS_GAMF";
                SQL = SQL + ComNum.VBLF + " WHERE      GAMJUMIN = '" + GamJumin + "'";

                SqlError = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    lblMsg0.Text = "삽입";
                    dt.Dispose();
                    dt = null;
                    return;
                }

                lblMsg0.Text = "수정";
                txtJumin1.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                txtJumin2.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 7, 7);
                txtSabun.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                dtpEnter.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                dtpOut.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                dtpEnd.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                txtMessage.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                txtName.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                txtSosok.Text = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                strRowid = VB.Mid(dt.Rows[0]["gamjumin"].ToString(), 1, 6);
                lblSosok.Text = READ_BuseName(txtSosok.Text);

                for (int i = 0; i < cboGubun.Items.Count; i++)
                {
                    if (dt.Rows[0]["gamcode"].ToString() == VB.Left(cboGubun.Items[i].ToString(), 2))
                    {
                        cboGubun.SelectedIndex = i;
                        return;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void Sosok_Select()
        {
            string SQL = "";
            string SqlError = "";
            DataTable dt = null;

            SQL = "     SELECT Name FROM BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + "  WHERE Bucode = '" + txtSosok.Text + "' ";

            SqlError = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                lblSosok.Text = dt.Rows[0]["Name"].ToString();
            }
            else
            {
                lblSosok.Text = "";
            }
            dt.Dispose();
            dt = null;
        }

        void Screen_Clear()
        {
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            txtSabun.Text = "";
            txtMessage.Text = "";
            txtName.Text = "";
            txtSosok.Text = "";
            dtpEnter.Text = "";
            dtpOut.Text = "";
            dtpEnd.Text = "";
            lblMsg0.Text = "";
            lblMsg1.Text = "";
            lblSosok.Text = "";

            cboGubun.SelectedIndex = -1;
        }

        void Gubun_Fill()
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            cboGubun.Items.Clear();
            cboGubun2.Items.Clear();

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT CODE, NAME FROM BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'BAS_감액코드명' ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
            SQL = SQL + ComNum.VBLF + "   AND (CODE >= 11 AND CODE <= 44 OR CODE = 52) ";
            SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboGubun.Items.Add(dt.Rows[i]["CODE"].ToString() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                cboGubun2.Items.Add(dt.Rows[i]["CODE"].ToString() + "." + dt.Rows[i]["NAME"].ToString().Trim());
            }
            cboGubun.Text = "";
            cboGubun2.Text = "";
            dt.Dispose();
            dt = null;
        }

        bool Check_JuminNo()
        {
            int Hap = 0;
            int Remainder = 0;
            int[] j = new int[5];
            bool rtnVal = false;
            
            j[0] = 8; j[1] = 9; j[2] = 2;
            j[3] = 3; j[4] = 4; j[5] = 5;
            
            for (int i = 1; i <= 6; i++)
            {
                Hap = Hap + (int)VB.Val(VB.Mid(txtJumin1.Text, i, 1)) * (i + 1);
                Hap = Hap + (int)VB.Val(VB.Mid(txtJumin2.Text, i, 1)) * j[i];
            }

            rtnVal = false;
            Remainder = Hap % 11;
            switch (Remainder)
            {
                case 0:
                    {
                        if (VB.Mid(txtJumin2.Text, 7, 1) == "1") { rtnVal = true; }
                        break;
                    }
                case 1:
                    {
                        if (VB.Mid(txtJumin2.Text, 7, 1) == "0") { rtnVal = true; }
                        break;
                    }
                default:
                    {
                        if (VB.Mid(txtJumin2.Text, 7, 1) == (11 - Remainder).ToString()) { rtnVal = true; }
                        break;
                    }
            }
             

            return rtnVal;
        }

        /// <summary>
        /// 저장 명령의 조건 체크
        /// </summary>
        /// <returns></returns>
        bool BAS_GAMF_Save_Check()
        {
            if (txtJumin1.Text.Length != 6) { txtJumin1.Focus(); return false; }
            if (VB.Val(txtJumin2.Text) < 1000000) { txtJumin2.Focus(); return false; }
            if (txtMessage.Text == "") { txtMessage.Focus(); return false; }
            if (txtName.Text == "") { txtName.Focus(); return false; }

            return true;
        }

        bool BAS_GAMF_Insert()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = " INSERT INTO BAS_GAMF (GAMJUMIN,GAMSABUN,GAMCODE,GAMENTER,GAMOUT,GAMEND,GAMMESSAGE,GAMNAME,GAMSOSOK,GAMGUBUN) ";
                SQL = SQL + ComNum.VBLF + " VALUES ( '" + txtJumin1.Text + txtJumin2.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + txtSabun.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + cboGubun.Text.Substring(0, 2) + "', ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnter.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpOut.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnd.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "'" + txtMessage.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + txtName.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtSosok.Text + "','0')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool BAS_GAMF_Update()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = " UPDATE BAS_GAMF SET Gamsabun    = '" + txtSabun.Text + "' ,";
                SQL = SQL + ComNum.VBLF + "               Gamcode    = '" + cboGubun.Text.Substring(0, 2) + "',";
                SQL = SQL + ComNum.VBLF + "   GAMENTER = TO_DATE('" + dtpEnter.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "   GAMOUT=    TO_DATE('" + dtpOut.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "   GAMEND=    TO_DATE('" + dtpEnd.Text + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "               Gammessage  = '" + txtMessage.Text + "', ";
                SQL = SQL + ComNum.VBLF + "               Gamsosok    = '" + txtSosok.Text + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE         ROWID = '" + strRowid + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                btnView.PerformClick();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool BAS_GAMF_Delete()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            


            try
            {
                SQL = "DELETE FROM BAS_GAMF WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                btnDelete.Enabled = false;
                Screen_Clear();
                btnView.PerformClick();
                txtJumin1.Focus();
                return true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool BAS_GAMF_Delete2()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            


            try
            {
                for (int i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    if (ss1_Sheet1.Cells[i, (int)ss1_Col.Delete].Text == "True")
                    {
                        strRowid = ss1_Sheet1.Cells[i, (int)ss1_Col.Rowid].Text;
                        SQL = "DELETE FROM BAS_GAMF WHERE ROWID = '" + strRowid + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ss1_Sheet1.Rows.Count = 0;
                btnDelete.Enabled = false;
                Screen_Clear();
                btnView.PerformClick();
                txtJumin1.Focus();
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strBucode = "";
            string strGubun = "";
            string strJumin1 = "";
            string strJumin2 = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ss1_Sheet1.Rows.Count = 0;

                SQL = "    SELECT      A.Gamjumin, A.Gamsabun,   A.Gamcode, ";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(A.Gamenter, 'YYYY-MM-DD') Gamenter, ";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(A.Gamout  , 'YYYY-MM-DD') Gamout,";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(A.Gamend  , 'YYYY-MM-DD') Gamend,";
                SQL = SQL + ComNum.VBLF + "             A.Gammessage, A.Gamname,  A.Gamsosok, A.GamJumin3 ,a.rowid,  ";
                SQL = SQL + ComNum.VBLF + "         B.NAME ";
                SQL = SQL + ComNum.VBLF + " FROM        BAS_GAMF A, BAS_BUSE B  ";
                SQL = SQL + ComNum.VBLF + "   WHERE A.GAMSOSOK = B.BUCODE(+)";
                if (optSel0.Checked == true)
                {
                    if (txtData.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  A.GAMSABUN = '" + String.Format("{0}:000000", txtData.Text.Trim()) + "'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.Gamsabun ";
                    }
                }
                else if (optSel1.Checked == true)
                {
                    if (txtData.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  A.Gamname LIKE '" + txtData.Text.Trim() + "%'";
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.Gamname ";
                    }
                }
                else if (optSel2.Checked == true)
                {
                    if (strGubun != "*")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  A.Gamcode = '" + strGubun + "' ";
                    }
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.Gamcode, A.Gamname ";
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

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strBucode = dt.Rows[i]["Gamsosok"].ToString();

                    if (dt.Rows[i]["Gamjumin3"].ToString() != "")
                    {
                        strJumin1 = clsAES.DeAES(dt.Rows[i]["Gamjumin3"].ToString()).Substring(0, 6);
                        strJumin2 = VB.Right(clsAES.DeAES(dt.Rows[i]["Gamjumin3"].ToString()), 7);
                    }
                    else
                    {
                        strJumin1 = dt.Rows[i]["Gamjumin"].ToString().Trim().Substring(0, 6);
                        strJumin2 = VB.Right(dt.Rows[i]["Gamjumin"].ToString().Trim(), 7);
                    }

                    ss1_Sheet1.Cells[i, (int)ss1_Col.Jumin].Text = strJumin1 + strJumin2;
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamsabun].Text = dt.Rows[i]["Gamsabun"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamcode].Text = dt.Rows[i]["Gamcode"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamenter].Text = dt.Rows[i]["Gamenter"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamout].Text = dt.Rows[i]["Gamout"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamend].Text = dt.Rows[i]["Gamend"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gammessage].Text = " " + dt.Rows[i]["Gammessage"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Gamname].Text = dt.Rows[i]["Gamname"].ToString();
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Bucode].Text = strBucode;
                    ss1_Sheet1.Cells[i, (int)ss1_Col.Rowid].Text = dt.Rows[i]["Rowid"].ToString();
                }

                dt.Dispose();
                dt = null;
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

        string READ_BuseName(string ArgCode)
        {
            string strVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT Sname FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE BuCode = '" + VB.Format(ArgCode, "000000") + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }
                else
                {
                    return strVal;
                }
                dt.Dispose();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strVal;
            }
        }

        /// <summary>
        /// 부서명으로 부서코드 찾기
        /// </summary>
        /// <param name="ArgName">부서명</param>
        /// <returns></returns>
        string BuseName2Code(string ArgName)
        {
            string SQL = "";
            string SqlError = "";
            string rtnVal = "";
            DataTable dt = null;

            if (ArgName.Length < 2) { rtnVal = ArgName; }
            if (Information.IsNumeric(ArgName) == true) { rtnVal = ArgName; }

            SQL = "SELECT BuCode FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + "WHERE SName LIKE '" + ArgName + "%' ";
            SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";
            SQL = SQL + ComNum.VBLF + "  AND Jas='*' ";

            SqlError = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["BuCode"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        #endregion

        #region 이벤트
        void setEvent()
        {
            this.Load           += (sender, e) =>
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                Screen_Clear();
                Gubun_Fill();
                btnDelete.Enabled = false;

                txtData.Text = "";
                ss1_Sheet1.Columns[(int)ss1_Col.Rowid].Visible = false;
            };
            ss1.CellDoubleClick += (sender, e) =>
            {
                if (e.Row < 0 || e.Column < 0) { return; }

                txtJumin1.Text = VB.Mid(ss1_Sheet1.Cells[e.Row, 1].Text, 1, 6);
                txtJumin2.Text = VB.Mid(ss1_Sheet1.Cells[e.Row, 1].Text, 7, 7);

                txtSabun.Text = ss1_Sheet1.Cells[e.Row, 2].Text;

                for(int i =0; i< cboGubun.Items.Count; i++)
                {
                    if (ss1_Sheet1.Cells[e.Row, 3].Text == VB.Left(cboGubun.Items[i].ToString(), 2))
                    {
                        cboGubun.SelectedIndex = i;
                        break;
                    }                       
                }

                dtpEnter.Text = ss1_Sheet1.Cells[e.Row, 4].Text;
                dtpOut.Text = ss1_Sheet1.Cells[e.Row, 5].Text;
                dtpEnd.Text = ss1_Sheet1.Cells[e.Row, 6].Text;
                txtMessage.Text = ss1_Sheet1.Cells[e.Row, 7].Text;
                txtName.Text = ss1_Sheet1.Cells[e.Row, 8].Text;
                txtSosok.Text = ss1_Sheet1.Cells[e.Row, 9].Text;
                Sosok_Select();

                strRowid = ss1_Sheet1.Cells[e.Row, 10].Text;

                lblMsg0.Text = "수정";

                txtSabun.Focus();

                btnDelete.Enabled = true;
            };
            txtJumin1.Enter     += (sender, e) => { strJumin1 = txtJumin1.Text; };
            txtJumin2.Enter     += (sender, e) => { strJumin2 = txtJumin2.Text; };

            #region Leave

            cboGubun.Leave  += (sender, e) =>
            {
                if (dtpEnd.Text != "" && (dtpEnd.Value < dtpEnter.Value || dtpEnd.Value > Convert.ToDateTime(clsPublic.GstrSysDate)))
                {
                    if (this.ActiveControl == txtSosok) { dtpEnd.Focus(); }
                    lblMsg1.Text = "종료일자 확인요망 !!.";
                }
            };

            dtpEnter.Leave  += (sender, e) =>
            {
                if ( dtpEnter.Value > DateTime.Parse(clsPublic.GstrSysDate))
                {
                    dtpEnter.Text = "";
                    lblMsg1.Text = "오늘보다 큰 날짜입니다.";
                }
            };

            dtpOut.Leave    += (sender, e) => 
            {
                if (dtpOut.Text != "" && (dtpOut.Value < dtpEnter.Value || dtpOut.Value > DateTime.Parse(clsPublic.GstrSysDate)))
                {
                    if (this.ActiveControl == dtpEnd)
                    {
                        dtpOut.Focus();
                        lblMsg1.Text = "퇴사일자 확인요망 !!.";
                    }
                }
            };

            txtJumin1.Leave += (sender, e) => 
            {
                DateTime Result = new DateTime();

                if (strJumin1 == txtJumin1.Text && strJumin2 == txtJumin2.Text) { return; }
                if (txtJumin1.Text == "") { return; }
                if (DateTime.TryParse(VB.Format(txtJumin1.Text, "0000-00-00"), out Result) == false) { txtJumin1.Focus(); return; }

                BAS_GAMF_JuminNo_Select();
            };

            txtJumin2.Leave += (sender, e) => 
            {
                if (strJumin1 == txtJumin1.Text && strJumin2 == txtJumin2.Text) { BAS_GAMF_JuminNo_Select(); }

                if (Check_JuminNo() == false)
                {
                    lblMsg1.Text = "";
                }
                else
                {
                    lblMsg1.Text = "";
                }
                BAS_GAMF_JuminNo_Select();
            };

            txtSabun.Leave  += (sender, e) => 
            {
                string strGamSabun = "";
                string SQL = "";
                string SqlError = "";
                DataTable dt = null;

                if (strSabun == txtSabun.Text) { return; }

                strGamSabun = txtSabun.Text;

                SQL = "    SELECT  GamJumin,GamSabun,Gamcode, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(GamEnter,'YYYY-MM-DD') GamEnter, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(GamOut,'YYYY-MM-DD') GamOut, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(GamEnd,'YYYY-MM-DD') GamEnd, ";
                SQL = SQL + ComNum.VBLF + "      GamMessage, GamName, GamSosok, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_GAMF ";
                SQL = SQL + ComNum.VBLF + "WHERE GamSabun = '" + strGamSabun + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Gamcode ";

                SqlError = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                ss1_Sheet1.Rows.Count = 0;
                if (dt.Rows.Count == 0) { dt.Dispose(); dt = null; return; }

                dtpEnter.Text = dt.Rows[0]["GamEnter"].ToString();
                dtpOut.Text = dt.Rows[0]["GamOut"].ToString();
                dtpEnd.Text = dt.Rows[0]["GamEnd"].ToString();
                txtSosok.Text = dt.Rows[0]["GamSosok"].ToString();

                Sosok_Select();

                txtMessage.Text = "< " + dt.Rows[0]["GamName"].ToString() + " >";

            };

            txtSosok.Leave  += (sender, e) =>
            {
                txtSosok.Text = txtSosok.Text.Trim();

                if (txtSosok.Text == "") { lblSosok.Text = ""; return; }

                txtSosok.Text = BuseName2Code(txtSosok.Text.Trim());
                lblSosok.Text = READ_BuseName(txtSosok.Text);

                if ( lblSosok.Text == "")
                {
                    MessageBox.Show("사용부서 코드가 오류 입니다.", "오류");
                    return;
                }
            };

            #endregion

            #region Click

            btnGamF1.Click   += (sender, e) => { };     //TODO:FrmGamf1.Show 1
            btnSave.Click    += (sender, e) => 
            {
                if (BAS_GAMF_Save_Check() == false) { return; }

                if (lblMsg0.Text == "삽입") { if (BAS_GAMF_Insert() == false) return; }
                if (lblMsg0.Text == "수정") { if (BAS_GAMF_Update() == false) return; }

                Screen_Clear();

                txtJumin1.Focus();
            };
            btnExit.Click    += (sender, e) => { this.Close(); };
            btnCancel.Click  += (sender, e) => { Screen_Clear(); txtJumin1.Focus(); };
            btnDelete.Click  += (sender, e) => { if (BAS_GAMF_Delete() == false) return; };
            btnDelete2.Click += (sender, e) => { if (BAS_GAMF_Delete2() == false) return; };
            btnView.Click    += (sender, e) => { if (ViewData() == false) return; };
            cboGubun.Click   += (sender, e) => { if (lblMsg1.Text != "") lblMsg1.Text = ""; };

            #endregion
        }
        #endregion

        /// <summary>
        /// 컨트롤 Carriage Return -> 다음 컨트롤 넘어감
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlAutoTab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == (char)Keys.Enter) { SendKeys.Send("{Tab}"); }
        }
    }
}
