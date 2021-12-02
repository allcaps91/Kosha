using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : OCS 오더 메세지
    /// Description     : frmOcsOrderMessage
    /// Author          : 전상원
    /// Create Date     : 2018-04-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 칼럼 Bohun없음, TEST
    /// </history>
    /// <seealso cref= " \basic\buppat\buppat.vbp(FrmOCSMessage) >> frmOcsOrderMessage.cs 폼이름 재정의" />
    public partial class frmOcsOrderMessage : Form
    {
        string strSysDate = "";
        string strFlagBp = "";
        string strGbMsg = "";
        string strCancelFlag = "";

        public frmOcsOrderMessage()
        {
            InitializeComponent();
        }

        private void frmOcsOrderMessage_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtPano.Text = "";
            txtName.Text = "";
            dtpSDate.Value = Convert.ToDateTime(strSysDate);

            dtpEDate.Value = Convert.ToDateTime(strSysDate);
            txtRowid.Text = "";

            cboSGbn.Items.Clear();
            cboGbn.Items.Clear();
            cboSGbn.Items.Add("***.전체");

            Combo_BCode_SET(cboSGbn, "BAS_OCSMEMO", false, 1);
            Combo_BCode_SET(cboGbn, "BAS_OCSMEMO", false, 1);

            cboSGbn.SelectedIndex = 0;
            cboGbn.SelectedIndex = 0;

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체과");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE, DEPTNAMEK FROM BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                    }
                }

                cboDept.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
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

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PTNO, SNAME, DEPTCODE, CODE, MEMO, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_OCSMEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO <> '99999999'";
                SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL";
                if (VB.Left(cboSGbn.Text, 3) != "***")
                {
                    SQL = SQL + ComNum.VBLF + " AND CODE = '" + VB.Left(cboSGbn.Text, 3) + "' ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView4_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView4_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssView4_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                        ssView4_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    }
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void SCREEN_CLEAR()
        {
            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            txtPano.Text = "";
            txtName.Text = "";
            dtpSDate.Text = strSysDate;
            dtpEDate.Text = "";
            txtRowid.Text = "";
            ssView4_Sheet1.RowCount = 0;
            ssView5_Sheet1.RowCount = 0;
            cboDept.Text = "";
            btnDelete.Enabled = false;

            ssView1_Sheet1.Cells[0, 2].Text = "";
            ssView1_Sheet1.Cells[0, 3].Text = "";
            ssView1_Sheet1.Cells[1, 2].Text = "";
            ssView1_Sheet1.Cells[1, 3].Text = "";
            ssView1_Sheet1.Cells[2, 2].Text = "";
            ssView1_Sheet1.Cells[2, 3].Text = "";
            ssView1_Sheet1.Cells[3, 2].Text = "";
            ssView1_Sheet1.Cells[3, 3].Text = "";

            ssView2_Sheet1.Cells[0, 2].Text = "";
            ssView2_Sheet1.Cells[0, 3].Text = "";
            ssView2_Sheet1.Cells[1, 2].Text = "";
            ssView2_Sheet1.Cells[1, 3].Text = "";
            ssView2_Sheet1.Cells[2, 2].Text = "";
            ssView2_Sheet1.Cells[2, 3].Text = "";
            ssView2_Sheet1.Cells[3, 2].Text = "";
            ssView2_Sheet1.Cells[3, 3].Text = "";

            ssView3_Sheet1.Cells[0, 2].Text = "";
            ssView3_Sheet1.Cells[0, 3].Text = "";
            ssView3_Sheet1.Cells[1, 2].Text = "";
            ssView3_Sheet1.Cells[1, 3].Text = "";
            ssView3_Sheet1.Cells[2, 2].Text = "";
            ssView3_Sheet1.Cells[2, 3].Text = "";
            ssView3_Sheet1.Cells[3, 2].Text = "";
            ssView3_Sheet1.Cells[3, 3].Text = "";
            ssView3_Sheet1.Cells[4, 2].Text = "";
            ssView3_Sheet1.Cells[4, 3].Text = "";
            ssView3_Sheet1.Cells[5, 2].Text = "";
            ssView3_Sheet1.Cells[5, 3].Text = "";
            ssView3_Sheet1.Cells[6, 2].Text = "";
            ssView3_Sheet1.Cells[6, 3].Text = "";
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            int i = 0;
            string strPano = "";
            string strKiho = "";
            string strDrCode = "";
            string strDeptCode = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            FarPoint.Win.Spread.CellType.TextCellType CellBo = new FarPoint.Win.Spread.CellType.TextCellType();

            strPano = VB.Val(txtPano.Text).ToString("00000000");

            if (VB.IsNull(txtPano.Text) || txtPano.Text == "")
            {
                return;
            }

            if (strCancelFlag == "OK")
            {
                return;
            }

            if (VB.IsNumeric(txtPano.Text) == false)
            {
                System.Media.SystemSounds.Beep.Play();

                txtPano.Focus();
                return;
            }

            #region Sql_Bas_Patient
            

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "        SELECT  Pano, Sname, Jumin1, Jumin2, to_char(StartDate,'yyyy-mm-dd') Sdate, ";
                SQL = SQL + ComNum.VBLF + "      to_char(Lastdate,'yyyy-mm-dd') Ldate, JiCode, Tel, Bi, Pname, ";
                SQL = SQL + ComNum.VBLF + "      Gwange, Kiho, Gkiho, DeptCode, Drcode, GbMsg, gbInfor, Remark,Jumin3 ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Sname"].ToString().Trim();
                    txtName.Text = dt.Rows[0]["Sname"].ToString().Trim();
                    ssView1_Sheet1.Cells[1, 2].Text = dt.Rows[0]["Jumin1"].ToString().Trim();
                    ssView1_Sheet1.Cells[1, 3].Text = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    ssView1_Sheet1.Cells[2, 2].Text = dt.Rows[0]["Tel"].ToString().Trim();
                    ssView1_Sheet1.Cells[3, 2].Text = dt.Rows[0]["JiCode"].ToString().Trim();

                    ssView2_Sheet1.Cells[0, 2].Text = dt.Rows[0]["SDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[1, 2].Text = dt.Rows[0]["LDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[2, 2].Text = dt.Rows[0]["DeptCode"].ToString().Trim();

                    strDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    if (VB.Trim(strDeptCode) != "")
                    {
                        SQL = "";
                        SQL = " SELECT DeptNameK FROM BAS_CLINICDEPT ";
                        SQL = SQL + ComNum.VBLF + " WHERE DeptCode = '" + strDeptCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView2_Sheet1.Cells[2, 3].Text = dt1.Rows[0]["DeptNameK"].ToString().Trim();
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[2, 3].Text = "Error !!!";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssView2_Sheet1.Cells[3, 2].Text = dt.Rows[0]["DrCode"].ToString().Trim();

                    strDrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                    if (VB.Trim(strDrCode) != "0000")
                    {
                        SQL = "";
                        SQL = " SELECT DrName FROM BAS_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + strDrCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView2_Sheet1.Cells[3, 3].Text = dt1.Rows[0]["DrName"].ToString().Trim();
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[3, 3].Text = "Error !!!";
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssView3_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Bi"].ToString().Trim();
                    ssView3_Sheet1.Cells[1, 2].Text = dt.Rows[0]["Gwange"].ToString().Trim();
                    ssView3_Sheet1.Cells[2, 2].Text = dt.Rows[0]["Pname"].ToString().Trim();
                    ssView3_Sheet1.Cells[3, 2].Text = dt.Rows[0]["Kiho"].ToString().Trim();

                    switch (VB.Mid(dt.Rows[0]["Bi"].ToString().Trim(), 1, 1))
                    {
                        case "2":
                            ssView3_Sheet1.Cells[3, 1].Text = "기관 기호";
                            ssView3_Sheet1.Cells[4, 1].Text = "관리 변호";
                            ssView3_Sheet1.Cells[5, 1].Text = "장애 여부";
                            ssView3_Sheet1.Cells[6, 1].Text = "";
                            ssView3_Sheet1.Cells[6, 2].Text = "";
                            ssView3_Sheet1.Cells[6, 3].Text = "";

                            ssView3_Sheet1.Cells[4, 2].Locked = false;
                            CellBo.MaxLength = 14;
                            ssView3_Sheet1.Cells[5, 2].Locked = false;
                            CellBo.MaxLength = 1;
                            ssView3_Sheet1.Cells[6, 2].Locked = true;
                            ssView3_Sheet1.Cells[6, 3].Locked = true;

                            ssView3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();
                            //TODO: 칼럼 Bohun없음
                            //ssView3_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Bohun"].ToString().Trim();

                            break;

                        case "3":
                            ssView3_Sheet1.Cells[3, 1].Text = "계약처 Co";
                            ssView3_Sheet1.Cells[4, 1].Text = "사고발생일";
                            ssView3_Sheet1.Cells[5, 1].Text = "진료시작일";
                            ssView3_Sheet1.Cells[6, 1].Text = "진료종료일";

                            ssView3_Sheet1.Cells[4, 2].Locked = false;
                            CellBo.MaxLength = 6;
                            ssView3_Sheet1.Cells[5, 2].Locked = false;
                            CellBo.MaxLength = 6;
                            ssView3_Sheet1.Cells[6, 2].Locked = false;
                            CellBo.MaxLength = 6;
                            ssView3_Sheet1.Cells[6, 3].Locked = true;

                            ssView3_Sheet1.Cells[4, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);
                            ssView3_Sheet1.Cells[5, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);
                            ssView3_Sheet1.Cells[6, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);

                            break;

                        case "5":
                            ssView3_Sheet1.Cells[3, 1].Text = "계약처 Co";
                            ssView3_Sheet1.Cells[4, 1].Text = "차량 번호";
                            ssView3_Sheet1.Cells[5, 1].Text = "";
                            ssView3_Sheet1.Cells[5, 2].Text = "";
                            ssView3_Sheet1.Cells[5, 3].Text = "";
                            ssView3_Sheet1.Cells[6, 1].Text = "";
                            ssView3_Sheet1.Cells[6, 2].Text = "";
                            ssView3_Sheet1.Cells[6, 3].Text = "";

                            ssView3_Sheet1.Cells[4, 2].Locked = false;
                            CellBo.MaxLength = 18;
                            ssView3_Sheet1.Cells[5, 2].Locked = true;
                            ssView3_Sheet1.Cells[6, 2].Locked = true;
                            ssView3_Sheet1.Cells[6, 3].Locked = true;

                            ssView3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();

                            break;

                        default:
                            ssView3_Sheet1.Cells[3, 1].Text = "기관 기호";
                            ssView3_Sheet1.Cells[4, 1].Text = "증    번  호";
                            ssView3_Sheet1.Cells[5, 1].Text = "승인 신청";
                            ssView3_Sheet1.Cells[6, 1].Text = "";
                            ssView3_Sheet1.Cells[6, 2].Text = "";
                            ssView3_Sheet1.Cells[6, 3].Text = "";

                            ssView3_Sheet1.Cells[4, 2].Locked = false;
                            CellBo.MaxLength = 14;
                            ssView3_Sheet1.Cells[5, 2].Locked = false;
                            CellBo.MaxLength = 20;
                            ssView3_Sheet1.Cells[6, 2].Locked = false;
                            CellBo.MaxLength = 2;

                            ssView3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();
                            ssView3_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Remark"].ToString().Trim();

                            break;
                            
                    }

                    strKiho = dt.Rows[0]["Kiho"].ToString().Trim();

                    SQL = "";
                    SQL = " SELECT * FROM BAS_MIA WHERE MiaCode  = '" + strKiho + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView3_Sheet1.Cells[3, 3].Text = dt1.Rows[0]["MiaName"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #region GbMsg_Set
                    strGbMsg = dt.Rows[0]["GbMsg"].ToString().Trim();
                    #endregion

                    strFlagBp = "OK";
                }
                else
                {
                    strFlagBp = "NO";
                }

                //BAS_OCSMSG

                SQL = "";
                SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, ";
                SQL = SQL + ComNum.VBLF + " DEPTCODE, MEMO, CODE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_OCSMEMO ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssView5_Sheet1.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView5_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssView5_Sheet1.Cells[i, 1].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssView5_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView5_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                        ssView5_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView5_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
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

                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            #endregion

            if (strFlagBp == "NO")
            {
                System.Media.SystemSounds.Beep.Play();

                SCREEN_CLEAR();
                txtPano.Focus();
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            //SS4의 정보를 레코드를 DB에 저장
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (cboDept.Text == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다.", "확인");
                cboDept.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (txtRowid.Text == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO BAS_OCSMEMO (  PTNO ,SNAME, DEPTCODE, CODE, Memo, SDATE, EDATE)";
                    SQL = SQL + ComNum.VBLF + " VALUES( '" + txtPano.Text + "', '" + txtName.Text + "', '" + VB.Left(cboDept.Text, 2) + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Left(cboGbn.Text, 3) + "' , '" + VB.Mid(cboGbn.Text, 5, cboGbn.Text.Length) + "', ";
                    SQL = SQL + ComNum.VBLF + "  TO_DATE('" + VB.Val(dtpSDate.Text).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') , TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') )";
                    
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE BAS_OCSMEMO SET ";
                    SQL = SQL + ComNum.VBLF + " CODE = '" + VB.Left(cboGbn.Text, 3) + "' ,";
                    SQL = SQL + ComNum.VBLF + " DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ,";
                    SQL = SQL + ComNum.VBLF + " SDATE = TO_DATE('" + VB.Val(dtpSDate.Text).ToString("YYYY-MM-DD") + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + " EDATE = TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "  MEMO = '" + VB.Mid(cboGbn.Text, 5, cboGbn.Text.Length) + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + txtRowid.Text + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            txtPano.Focus();
            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
            SendKeys.Send("{Tab}");
            SCREEN_CLEAR();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (txtRowid.Text == "")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말 삭제 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {

                    SQL = "";
                    SQL = " DELETE BAS_OCSMEMO WHERE ROWID ='" + txtRowid.Text + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SCREEN_CLEAR();
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                SendKeys.Send("{Tab}");
            }
        }

        private void ssView5_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView5_Sheet1.RowCount == 0)
            {
                return;
            }

            dtpSDate.Value = Convert.ToDateTime(ssView5_Sheet1.Cells[e.Row, 0].Text);
        }

        private void Combo_BCode_SET(ComboBox ArgCombobox, string ArgGubun, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Sort,Code,Name FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun='" + ArgGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY Sort,Code ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (ArgNULL != "N")
                {
                    ArgCombobox.Items.Add(" ");
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (ArgTYPE == 1)
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                        }
                        else if (ArgTYPE == 2)
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                        }
                        else if (ArgTYPE == 3)
                        {
                            ArgCombobox.Items.Add(dt.Rows[i]["NAME"].ToString().Trim());
                        }
                    }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView4_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView4_Sheet1.RowCount == 0)
            {
                return;
            }

            txtPano.Text = VB.Val(ssView4_Sheet1.Cells[e.Row, 0].Text).ToString("00000000");

            txtPano.Focus();

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
            SendKeys.Send("{Tab}");
        }
    }
}
