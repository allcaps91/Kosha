using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmMedTreatmentReservation.cs
    /// Description     : 진료 예약
    /// Author          : 이정현
    /// Create Date     : 2018-07-12
    /// <history> 
    /// 진료 예약
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmReserved.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmMedTreatmentReservation : Form
    {
        private string GstrBDate = "";
        private string GstrPtNo = "";
        private string GstrPtName = "";
        private string GstrDeptCode = "";
        private string GstrDrCode = "";

        private string FstrReserved = "";
        private string FstrRDoct = "";
        private string FstrRDept = "";
        private string FstrYDate = "";
        private string FstrROWID = "";
        private string FstrDate3 = "";

        int FnMorningNo = 0;
        int FnResvInwon = 0;
        int FnResvInwon2 = 0;
        int FnSelRow = 0;
        int FnSelCol = 0;

        string[] FstrRTime = new string[0];
        string[] FstrRDate = new string[0];

        public FrmMedTreatmentReservation()
        {
            InitializeComponent();
        }

        public FrmMedTreatmentReservation(string strBDate, string strPtNo, string strPtName, string strDeptCode, string strDrCode)
        {
            InitializeComponent();

            GstrBDate = strBDate;
            GstrPtNo = strPtNo;
            GstrPtName = strPtName;
            GstrDeptCode = strDeptCode;
            GstrDrCode = strDrCode;
        }

        private void FrmMedTreatmentReservation_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            InitForm();
        }

        private void InitForm()
        {
            ssView1_Sheet1.ColumnCount = 0;

            ssView2_Sheet1.RowCount = 0;

            btnOk.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Enabled = false;

            lblYeyak0.Text = "";
            lblYeyak1.Text = "";
            lblYeyak2.Text = "";
            lblYeyak3.Text = "";
            lblYeyak4.Text = "";
            lblYeyak5.Text = "";
            lblYeyak6.Text = "";

            lblPtno.Text = GstrPtNo;
            lblName.Text = GstrPtName;
            lblRTime.Text = "";
            lblStat.Text = "";

            FstrReserved = "";
            FstrRDoct = GstrDrCode;
            FstrYDate = "";
            FstrROWID = "";
            FstrDate3 = "";

            cboDept.Items.Clear();

            SetCboDept();
            Reserved_DisPlay();
        }

        private void SetCboDept()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE DeptCode NOT IN ('AN','HR','TO','II','R6','PT','EM','HD','ER') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }

                    if (GstrDeptCode != "")
                    {
                        ComFunc.ComboFind(cboDept, "L", 2, GstrDeptCode);
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Reserved_DisPlay()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strData = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode, DrCode, TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') AS RDate ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                SQL = SQL + ComNum.VBLF + "         AND BDate >= TRUNC(SYSDATE-5) ";
                SQL = SQL + ComNum.VBLF + "         AND GbSunap ='0' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = dt.Rows[i]["RDATE"].ToString().Trim() + " " + dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        switch (i)
                        {
                            case 0: lblYeyak0.Text = strData; break;
                            case 1: lblYeyak1.Text = strData; break;
                            case 2: lblYeyak2.Text = strData; break;
                            case 3: lblYeyak3.Text = strData; break;
                            case 4: lblYeyak4.Text = strData; break;
                            case 5: lblYeyak5.Text = strData; break;
                            case 6: lblYeyak6.Text = strData; break;
                        }
                    }

                    if (lblYeyak0.Text.Trim() != "")
                    {
                        lblYeyak_Click(lblYeyak0, new EventArgs());
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                InitForm();
                GetData();
            }
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strRDate = "";
            string strRTime = "";

            if (VB.Len(lblRTime.Text.Trim()) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI)형태로 입력하세요");
                return false;
            }

            if (cboDoct.Text.Trim() == "")
            {
                ComFunc.MsgBox("예약의사가 공란입니다.");
                return false;
            }

            strRDate = VB.Left(lblRTime.Text, 10);
            strRTime = VB.Right(lblRTime.Text, 5);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_RESERVED";
                    SQL = SQL + ComNum.VBLF + "     (BDate, Pano, SName, DeptCode, DrCode, IpdOpd, RDate, GbSunap, EntSabun)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         TRUNC(SYSDATE), ";
                    SQL = SQL + ComNum.VBLF + "         '" + lblPtno.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + lblName.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + cboDept.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + VB.Left(cboDoct.Text, 4) + "', ";
                    SQL = SQL + ComNum.VBLF + "         'I', ";
                    SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + "         '0', ";
                    SQL = SQL + ComNum.VBLF + "         '" + GstrDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_RESERVED";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         DrCode = '" + VB.Left(cboDoct.Text, 4) + "', ";
                    SQL = SQL + ComNum.VBLF + "         RDate = TO_DATE('" + strRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + "         EntSabun = " + GstrDrCode;
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + FstrROWID + "' ";
                }

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
                ComFunc.MsgBox(strRTime + "에 예약을 하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {
                InitForm();
                GetData();
            }
        }

        private bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + cboDept.Text.Trim() + "' ";

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
                ComFunc.MsgBox(cboDept.Text + "과 예약을 취소 하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateData() == true)
            {
                InitForm();
                GetData();
            }
        }

        private bool UpdateData()
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (VB.Len(lblRTime.Text.Trim()) != 16)
            {
                ComFunc.MsgBox("예약일시를 (YYYY-MM-DD HH:MI)형태로 입력하세요");
                return false;
            }

            if (cboDoct.Text.Trim() == "")
            {
                ComFunc.MsgBox("예약의사가 공란입니다.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            GstrDeptCode = cboDept.Text.Trim();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + "     SET ";
                SQL = SQL + ComNum.VBLF + "         Date3 = TO_DATE('" + lblRTime.Text.Trim() + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + "         DrCode = '" + ComFunc.LeftH(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + lblPtno.Text + "' ";
                SQL = SQL + ComNum.VBLF + "     AND DeptCode = '" + GstrDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND DATE3 = TO_DATE('" + FstrDate3 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "     AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND RETDATE IS NULL ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("예약일자를 정상적으로 변경하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strDeptCode = cboDept.Text.Trim();

            btnOk.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Enabled = false;

            FstrReserved = "";
            FstrRDept = strDeptCode;
            FstrRDoct = GstrDrCode;
            FstrYDate = "";
            FstrROWID = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //원무과 예약 Table에 자료가 있는지 Check
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode, DrCode, TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') AS RTime, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Date3 >= TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrReserved = "OK";
                    FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                    FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    FstrDate3 = FstrYDate;
                }

                dt.Dispose();
                dt = null;

                if (FstrReserved == "OK")
                {
                    btnOk.Enabled = false;
                    btnCancel.Enabled = false;
                    btnUpdate.Enabled = true;
                    lblStat.Text = "수납";
                }
                else
                {
                    //입원예약이 있는지 Check
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') AS RTime, DrCode, GbSunap, ROWID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                    SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BDate >= TRUNC(SYSDATE-1) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrReserved = "YEYAK";
                        FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                        FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                        FstrDate3 = FstrYDate;
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        lblStat.Text = "미수납";
                    }

                    dt.Dispose();
                    dt = null;
                }

                lblRTime.Text = FstrYDate;

                cboDoct.Items.Clear();

                //재원과의 의사를 Display
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DrCode, DrName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrDept1 = '" + strDeptCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                    }

                    ComFunc.ComboFind(cboDoct, "L", 4, FstrRDoct);
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboDoct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetData();
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int j = 0;
            int k = 0;

            string strRDate = "";
            string strRTime = "";
            string strETime = "";
            string strGDate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(59).ToString("yyyy-MM-dd");
            string strAmTime = "";
            string strAMTime2 = "";
            string strPmTime = "";
            string strPMTime2 = "";
            string strDrCode = VB.Left(cboDoct.Text, 4);

            string FstrDrCode = "";

            int[,,] nCNT = new int[60, 50, 2];

            int inx1 = 0;
            int inx2 = 0;
            int nRow = 0;
            int nCol = 0;
            int nTime = 0;
            int nTimeCNT = 0;
            int FnResvTimeGbn = 0;

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            strAmTime = "";
            strPmTime = "";
            lblYinwon.Text = "";

            ssView1_Sheet1.RowCount = 5;
            ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssView1_Sheet1.ColumnCount = 0;
            ssView2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DrCode, DrName, YTimeGbn, YInwon, AmTime, PmTime, YInwon2, AmTime2, PmTime2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrCode = '" + strDrCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FnResvTimeGbn = Convert.ToInt32(VB.Val(dt.Rows[0]["YTimeGbn"].ToString().Trim()));
                    FnResvInwon = Convert.ToInt32(VB.Val(dt.Rows[0]["YInwon"].ToString().Trim()));
                    strAmTime = dt.Rows[0]["AmTime"].ToString().Trim();
                    strPmTime = dt.Rows[0]["PmTime"].ToString().Trim();
                    FnResvInwon2 = Convert.ToInt32(VB.Val(dt.Rows[0]["YInwon2"].ToString().Trim()));
                    strAMTime2 = dt.Rows[0]["AmTime2"].ToString().Trim();
                    strPMTime2 = dt.Rows[0]["PmTime2"].ToString().Trim();
                    lblYinwon.Text = "예약인원(오전:" + FnResvInwon + "명 오후:" + FnResvInwon2 + "명)";
                }

                dt.Dispose();
                dt = null;

                FstrDrCode = strDrCode;
                //예약구분이 없으면 기본으로 30분단위 예약
                if (FnResvTimeGbn == 0) { FnResvTimeGbn = 4; }
                //예약인원이 없으면 인원제한 않함
                if (FnResvInwon == 0) { FnResvInwon = 999; }
                //예약 시작시간 설정
                if (strAmTime == "") { strAmTime = "09:30"; }
                if (strPmTime == "") { strPmTime = "14:00"; }

                nRow = 5;
                FnMorningNo = 0;

                switch (FnResvTimeGbn)
                {
                    case 1: nTime = 10; break;
                    case 2: nTime = 15; break;
                    case 3: nTime = 20; break;
                    case 4: nTime = 30; break;
                }

                for (i = clsVbfunc.TimeToMi(strAmTime); i <= clsVbfunc.TimeToMi(strAMTime2); i = i + nTime)
                {
                    Array.Resize<string>(ref FstrRTime, FstrRTime.Length + 1);
                    FstrRTime[j] = clsVbfunc.MiToTime(i);

                    if (nRow >= ssView1_Sheet1.RowCount)
                    {
                        ssView1_Sheet1.RowCount = nRow + 1;
                    }

                    ssView1_Sheet1.RowHeader.Cells[nRow, 0].Text = FstrRTime[j];
                    FnMorningNo = nRow;
                    j = j + 1;
                    nRow++;
                }

                for (i = clsVbfunc.TimeToMi(strPmTime); i <= clsVbfunc.TimeToMi(strPMTime2); i = i + nTime)
                {
                    Array.Resize<string>(ref FstrRTime, FstrRTime.Length + 1);
                    FstrRTime[j] = clsVbfunc.MiToTime(i);

                    if (nRow >= ssView1_Sheet1.RowCount)
                    {
                        ssView1_Sheet1.RowCount = nRow + 1;
                    }

                    ssView1_Sheet1.RowHeader.Cells[nRow, 0].Text = FstrRTime[j];
                    j = j + 1;
                    nRow++;
                }

                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                nTimeCNT = nRow - 4;

                Array.Resize<string>(ref FstrRTime, FstrRTime.Length + 1);
                FstrRTime[nTimeCNT - 1] = "23:59";

                //예약 스케쥴을 읽어 SHEET의 상단에 Display
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SchDate,'YYYY-MM-DD') AS SchDate, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SchDate,'DY') AS Yoil, GbDay, GbJin, GbJin2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SchDate > TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND SchDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.ColumnCount = dt.Rows.Count;
                    ssView1_Sheet1.SetColumnWidth(-1, 50);

                    ssView1_Sheet1.Columns[0, ssView1_Sheet1.ColumnCount - 1].CellType = txt;
                    ssView1_Sheet1.Columns[0, ssView1_Sheet1.ColumnCount - 1].Locked = true;
                    ssView1_Sheet1.Columns[0, ssView1_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    ssView1_Sheet1.Columns[0, ssView1_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                    nCol = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["YOIL"].ToString().ToUpper().Trim() != "SUN")
                        {
                            Array.Resize<string>(ref FstrRDate, FstrRDate.Length + 1);
                            FstrRDate[nCol] = dt.Rows[i]["SchDate"].ToString().Trim();
                            strRDate = dt.Rows[i]["SchDate"].ToString().Trim();

                            ssView1_Sheet1.ColumnHeader.Cells[0, nCol].Text = VB.Right(strRDate, 5);
                            ssView1_Sheet1.Cells[0, nCol].Text = strRDate;
                            ssView1_Sheet1.Cells[1, nCol].Text = dt.Rows[i]["GbJin"].ToString().Trim();

                            switch (dt.Rows[i]["Yoil"].ToString().ToUpper().Trim())
                            {
                                case "SUN": case "일": ssView1_Sheet1.Cells[2, nCol].Text = "일"; break;
                                case "MON": case "월": ssView1_Sheet1.Cells[2, nCol].Text = "월"; break;
                                case "TUE": case "화": ssView1_Sheet1.Cells[2, nCol].Text = "화"; break;
                                case "WED": case "수": ssView1_Sheet1.Cells[2, nCol].Text = "수"; break;
                                case "THU": case "목": ssView1_Sheet1.Cells[2, nCol].Text = "목"; break;
                                case "FRI": case "금": ssView1_Sheet1.Cells[2, nCol].Text = "금"; break;
                                case "SAT": case "토": ssView1_Sheet1.Cells[2, nCol].Text = "토"; break;
                                default: ssView1_Sheet1.Cells[2, nCol].Text = ""; break;
                            }

                            if (FnMorningNo != 0)
                            {
                                switch (dt.Rows[i]["GBJIN"].ToString().Trim())
                                {
                                    case "1":
                                        ssView1_Sheet1.Cells[3, nCol].Text = "진료";
                                        ssView1_Sheet1.Cells[5, nCol, FnMorningNo, nCol].BackColor = Color.FromArgb(192, 255, 192);
                                        break;
                                    case "2":
                                        ssView1_Sheet1.Cells[3, nCol].Text = "수술";
                                        ssView1_Sheet1.Cells[5, nCol, FnMorningNo, nCol].BackColor = Color.FromArgb(255, 224, 192);
                                        break;
                                    case "3":
                                        ssView1_Sheet1.Cells[3, nCol].Text = "특검";
                                        ssView1_Sheet1.Cells[5, nCol, FnMorningNo, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                        break;
                                    case "9":
                                        ssView1_Sheet1.Cells[3, nCol].Text = "OFF";
                                        ssView1_Sheet1.Cells[5, nCol, FnMorningNo, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                        break;
                                    default:
                                        ssView1_Sheet1.Cells[3, nCol].Text = "휴진";
                                        ssView1_Sheet1.Cells[5, nCol, FnMorningNo, nCol].BackColor = Color.FromArgb(255, 255, 255);
                                        break;
                                }

                                switch (dt.Rows[i]["GBJIN2"].ToString().Trim())
                                {
                                    case "1":
                                        ssView1_Sheet1.Cells[4, nCol].Text = "진료";
                                        ssView1_Sheet1.Cells[FnMorningNo + 1, nCol, ssView1_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(192, 255, 192);
                                        break;
                                    case "2":
                                        ssView1_Sheet1.Cells[4, nCol].Text = "수술";
                                        ssView1_Sheet1.Cells[FnMorningNo + 1, nCol, ssView1_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 224, 192);
                                        break;
                                    case "3":
                                        ssView1_Sheet1.Cells[4, nCol].Text = "특검";
                                        ssView1_Sheet1.Cells[FnMorningNo + 1, nCol, ssView1_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 128, 0);
                                        break;
                                    case "9":
                                        ssView1_Sheet1.Cells[4, nCol].Text = "OFF";
                                        ssView1_Sheet1.Cells[FnMorningNo + 1, nCol, ssView1_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 192, 192);
                                        break;
                                    default:
                                        ssView1_Sheet1.Cells[4, nCol].Text = "휴진";
                                        ssView1_Sheet1.Cells[FnMorningNo + 1, nCol, ssView1_Sheet1.RowCount - 1, nCol].BackColor = Color.FromArgb(255, 255, 255);
                                        break;
                                }
                            }

                            nCol++;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //의사별 기타 스케쥴을 읽어 Sheet에 표시함
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SchDate,'YYYY-MM-DD') AS SchDate, STime, ETime ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SchDate > TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "         AND SchDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SchDate, STime ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRDate = dt.Rows[i]["SCHDATE"].ToString().Trim();
                        strRTime = dt.Rows[i]["STIME"].ToString().Trim();
                        strETime = dt.Rows[i]["ETIME"].ToString().Trim();

                        //예약일자 Col 찾기
                        inx1 = 0;

                        for (j = 1; j <= FstrRDate.Length; j++)
                        {
                            if (strRDate == FstrRDate[j - 1])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        if (inx1 > 0)
                        {
                            for (j = 1; j <= nTimeCNT; j++)
                            {
                                if (Convert.ToDateTime(FstrRTime[j - 1]) >= Convert.ToDateTime(strRTime)
                                    && Convert.ToDateTime(FstrRTime[j - 1]) <= Convert.ToDateTime(strETime))
                                {
                                    ssView1_Sheet1.Cells[j + 4, inx1].BackColor = Color.FromArgb(192, 192, 192);
                                }
                            }
                        }
                    }
                }


                dt.Dispose();
                dt = null;

                //예약자 인원을 COUNT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') AS RTime, COUNT(*) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Date3 > TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND Date3 <= TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "GROUP BY DATE3 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DATE3 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRDate = VB.Left(dt.Rows[i]["RTime"].ToString().Trim(), 10);
                        strRTime = VB.Right(dt.Rows[i]["RTime"].ToString().Trim(), 5);

                        //예약일자
                        inx1 = 0;
                        for (j = 1; j <= FstrRDate.Length; j++)
                        {
                            if (strRDate == FstrRDate[j - 1])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //예약시간 ROW
                        inx2 = 0;
                        for (j = 1; j <= nTimeCNT; j++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[j - 1]))
                            {
                                inx2 = j;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCNT[inx1 - 1, inx2 - 1, 0] = nCNT[inx1 - 1, inx2 - 1, 0] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //병동의 퇴원자예약 미수납자를 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') AS RTime, COUNT(*) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                SQL = SQL + ComNum.VBLF + "     WHERE BDate = TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "         AND RDate > TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND RDate <= TO_DATE('" + strGDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {

                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRDate = VB.Left(dt.Rows[i]["RTime"].ToString().Trim(), 10);
                        strRTime = VB.Right(dt.Rows[i]["RTime"].ToString().Trim(), 5);

                        //예약일자
                        inx1 = 0;
                        for (j = 1; j <= FstrRDate.Length; j++)
                        {
                            if (strRDate == FstrRDate[j - 1])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //예약시간 ROW
                        inx2 = 0;
                        for (j = 1; j <= nTimeCNT; j++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[j - 1]))
                            {
                                inx2 = j;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCNT[inx1 - 1, inx2 - 1, 0] = nCNT[inx1 - 1, inx2 - 1, 0] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //전화접수 인원을 COUNT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDate,'YYYY-MM-DD') AS RDate, RTime, COUNT(*) AS CNT ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "     WHERE RDate > TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND RDate <= TO_DATE('" + strGDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY RDate, RTime ";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDate, RTime ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRDate = VB.Left(dt.Rows[i]["RDATE"].ToString().Trim(), 10);
                        strRTime = VB.Right(dt.Rows[i]["RTime"].ToString().Trim(), 5);

                        //예약일자
                        inx1 = 0;
                        for (j = 1; j <= FstrRDate.Length; j++)
                        {
                            if (strRDate == FstrRDate[j - 1])
                            {
                                inx1 = j;
                                break;
                            }
                        }

                        //예약시간 ROW
                        inx2 = 0;
                        for (j = 1; j <= nTimeCNT; j++)
                        {
                            if (Convert.ToDateTime(strRTime) < Convert.ToDateTime(FstrRTime[j - 1]))
                            {
                                inx2 = j;
                                break;
                            }
                        }

                        if (inx1 > 0 && inx2 > 0)
                        {
                            nCNT[inx1 - 1, inx2 - 1, 1] = nCNT[inx1 - 1, inx2 - 1, 1] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //자료를 SHEET에 Display
                for (i = 0; i < ssView1_Sheet1.ColumnCount; i++)
                {
                    for (j = 0; j <= nTimeCNT; j++)
                    {
                        if (nCNT[i, j, 0] != 0 || nCNT[i, j, 1] != 0)
                        {
                            ssView1_Sheet1.Cells[j + 4, i].Text = (nCNT[i, j, 0] + nCNT[i, j, 1]).ToString("##0") + "(" + nCNT[i, j, 1].ToString("##0") + ")";  //당일예약+전화예약
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void lblYeyak_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strDeptCode = VB.Right(((Label)sender).Text, 2);
            string strDrCode = "";
            
            if (strDeptCode.Trim() == "") { return; }

            btnOk.Enabled = true;
            btnCancel.Enabled = true;
            btnUpdate.Enabled = false;

            ComFunc.ComboFind(cboDept, "L", 2, strDeptCode);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                FstrReserved = "";
                FstrRDept = strDeptCode;
                FstrRDoct = GstrDrCode;
                FstrYDate = "";
                FstrROWID = "";

                //원무과 예약 Table에 자료가 있는지 Check
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode, DrCode, TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') AS RTime, ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strDeptCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Date3 >= TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "         AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND RETDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FstrReserved = "OK";
                    FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                    FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                //기수납한 예약자료 수정 못하게
                if (FstrReserved == "OK")
                {
                    btnOk.Enabled = false;
                    btnCancel.Enabled = false;
                    btnUpdate.Enabled = true;
                    lblStat.Text = "수납";
                }
                else
                {
                    //입원예약이 있는지 Check
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(RDate,'YYYY-MM-DD HH24:MI') AS RTime, DrCode, GbSunap, ROWID ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED ";
                    SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + strDeptCode + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND Gbsunap = '0' ";  //2013-10-04
                    SQL = SQL + ComNum.VBLF + "         AND BDate >= TRUNC(SYSDATE-5) ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrReserved = "YEYAK";
                        FstrRDoct = dt.Rows[0]["DrCode"].ToString().Trim();
                        FstrYDate = dt.Rows[0]["RTime"].ToString().Trim();
                        FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                        lblStat.Text = "미수납";
                    }

                    dt.Dispose();
                    dt = null;
                }

                lblRTime.Text = FstrYDate;
                FstrDate3 = FstrYDate;

                //재원과의 의사를 Display
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DrCode, DrName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "     WHERE DrDept1 = '" + strDeptCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDoct.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                    }

                    ComFunc.ComboFind(cboDoct, "L", 4, FstrRDoct);
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string strRTime = "";
            string strTime1 = "";
            string strTime2 = "";
            int nYeyakInwon = 0;

            ssView2_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //예약자 조회할 시각을 설정
                //strRTime = Convert.ToDateTime(GstrBDate).ToString("yyyy-") + ssView1_Sheet1.ColumnHeader.Cells[0, e.Column].Text;
                strRTime = ssView1_Sheet1.Cells[0, e.Column].Text;

                if (e.Row < 5)
                {
                    strTime1 = VB.Left(strRTime, 10) + " 00:00";
                    strTime2 = VB.Left(strRTime, 10) + " 23:59";
                }
                else if (e.Row == 4 || e.Row == 3)
                {
                    strTime1 = VB.Left(strRTime, 10) + " 00:00";
                    strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
                }
                else if (e.Row == ssView1_Sheet1.RowCount - 1)
                {
                    strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                    strTime2 = VB.Left(strRTime, 10) + " 23:59";
                }
                else
                {
                    strTime1 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 5];
                    strTime2 = VB.Left(strRTime, 10) + " " + FstrRTime[e.Row - 4];
                }

                if (ComFunc.LeftH(cboDoct.Text, 4) == "0107"
                    && clsVbfunc.GetYoIl(strRTime) == "화요일"
                    && Convert.ToDateTime(VB.Right(strTime1, 5)) <= Convert.ToDateTime("10:30"))
                {
                    ComFunc.MsgBox("박준모과장 화요일은 10시30전 예약 안됨!!", "내과문의");
                    return;
                }

                //해당 시각대 예약자를 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.Pano, b.SName, b.Tel, a.DeptCode, c.DrName,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.Date3, 'MM/DD HH24:MI') AS RDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.Date3, 'YYYY-MM-DD HH24:MI') AS RDate2,";
                SQL = SQL + ComNum.VBLF + "     b.Juso, b.Sex, b.ZipCode1, b.ZipCode2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW a, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.Date3 >= TO_DATE('" + strTime1 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND a.Date3 < TO_DATE('" + strTime2 + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND a.DrCode = '" + ComFunc.LeftH(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND A.RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND a.Pano = b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND a.DrCode = c.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.Date3, a.Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 5].Text = "예약";
                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["TEL"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SEX"].ToString().Trim();

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     MailJuso";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                        SQL = SQL + ComNum.VBLF + "     WHERE MailCode = '" + dt.Rows[i]["ZipCode1"].ToString().Trim() + dt.Rows[i]["ZipCode2"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            ssView2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[i, 8].Text = dt1.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["RDATE2"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //해당 시각대 전화접수자를 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.Pano, b.SName, b.Tel, a.DeptCode, c.DrName,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.RDate, 'YYYY-MM-DD') AS RDate, RTime,";
                SQL = SQL + ComNum.VBLF + "     b.Juso, b.Sex, b.ZipCode1, b.ZipCode2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_TELRESV a, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT b,";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_DOCTOR c ";
                SQL = SQL + ComNum.VBLF + "     WHERE a.RDate = TO_DATE('" + VB.Left(strTime1, 10) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND a.RTime >= '" + VB.Right(strTime1, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.RTime < '" + VB.Right(strTime2, 5) + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.DrCode = '" + ComFunc.LeftH(cboDoct.Text, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "         AND a.Pano = b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "         AND a.DrCode = c.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.RDate, a.RTime, a.Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;
                        ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 4].Text = VB.Right(dt.Rows[i]["RDATE"].ToString().Trim(), 5) + " " + dt.Rows[i]["RTIME"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 5].Text = "전화";
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["TEL"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["RDATE"].ToString().Trim() + " " + dt.Rows[i]["RTIME"].ToString().Trim();

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     MailJuso";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                        SQL = SQL + ComNum.VBLF + "     WHERE MailCode = '" + dt.Rows[i]["ZipCode1"].ToString().Trim() + dt.Rows[i]["ZipCode2"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["JUSO"].ToString().Trim();
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = dt1.Rows[0]["MAILJUSO"].ToString().Trim() + " " + dt.Rows[i]["JUSO"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                //예약이 가능한 시각인지 Check
                if (e.Row >= 5)
                {
                    //strRTime = Convert.ToDateTime(GstrBDate).ToString("yyyy-") + ssView1_Sheet1.ColumnHeader.Cells[0, e.Column].Text;
                    strRTime = ssView1_Sheet1.Cells[0, e.Column].Text;
                    strRTime += " " + ssView1_Sheet1.RowHeader.Cells[e.Row, 0].Text;
                    nYeyakInwon = (int)VB.Val(ssView1_Sheet1.Cells[e.Row, e.Column].Text) + 1;

                    //예약 불가능 시간대
                    if (ssView1_Sheet1.Cells[e.Row, e.Column].BackColor != Color.FromArgb(192, 255, 192))
                    {
                        ComFunc.MsgBox(strRTime + "은 예약이 불가능한 시간 입니다."
                            + ComNum.VBLF + "스케쥴에 오류가 있으면 심전도실(☏534)에"
                            + ComNum.VBLF + "통보하여 스케쥴을 수정 바랍니다.", "오류");
                    }
                    else if (e.Row <= FnMorningNo && nYeyakInwon <= FnResvInwon)
                    {
                        lblRTime.Text = strRTime;
                        //해당 예약일자의 Cell Backcolor를 변경
                        if (e.Row != 0)
                        {
                            ssView1_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(192, 255, 192);
                        }

                        ssView1_Sheet1.Cells[FnSelRow, FnSelCol].BackColor = Color.FromArgb(255, 255, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else if (e.Row > FnMorningNo && nYeyakInwon <= FnResvInwon2)
                    {
                        lblRTime.Text = strRTime;
                        //해당 예약일자의 Cell Backcolor를 변경
                        if (FnSelRow != 0)
                        {
                            ssView1_Sheet1.Cells[e.Row, e.Column].BackColor = Color.FromArgb(192, 255, 192);
                        }

                        ssView1_Sheet1.Cells[FnSelRow, FnSelCol].BackColor = Color.FromArgb(255, 255, 0);

                        FnSelRow = e.Row;
                        FnSelCol = e.Column;
                    }
                    else
                    {
                        if (e.Row <= FnMorningNo)
                        {
                            ComFunc.MsgBox(cboDoct.Text + " 과장님은 예약단위당(오전) " + FnResvInwon + "명이 가능합니다"
                                + ComNum.VBLF + strRTime + "은 예약인원 초과입니다."
                                + ComNum.VBLF + "다른 예약일시를 선택하십시오.");
                        }
                        else
                        {
                            ComFunc.MsgBox(cboDoct.Text + " 과장님은 예약단위당(오후) " + FnResvInwon2 + "명이 가능합니다"
                                + ComNum.VBLF + strRTime + "은 예약인원 초과입니다."
                                + ComNum.VBLF + "다른 예약일시를 선택하십시오.");
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = "";
            string strSname = "";
            string strRTime = "";

            strPano = ssView2_Sheet1.Cells[e.Row, 0].Text.Trim();
            strSname = ssView2_Sheet1.Cells[e.Row, 1].Text.Trim();
            strRTime = ssView2_Sheet1.Cells[e.Row, 9].Text.Trim();

            if (ssView2_Sheet1.Cells[e.Row, 5].Text == "전화")
            {
                ComFunc.MsgBox("전화예약은 변경이 불가능 합니다.");
                return;
            }

            if (ComFunc.MsgBoxQ(strPano + " " + strSname + "님의 예약을 변경하시겠습니까?", "예약변경", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            FstrDate3 = "2003-" + VB.Left(ssView2_Sheet1.Cells[e.Row, 4].Text, 2) + "-" + VB.Mid(ssView2_Sheet1.Cells[e.Row, 4].Text, 4, 2);

            lblPtno.Text = strPano;
            lblName.Text = strSname;
            lblStat.Text = "수납";
            btnOk.Enabled = false;
            btnCancel.Enabled = false;
            btnUpdate.Enabled = true;
            lblRTime.Text = strRTime;
        }
    }
}

