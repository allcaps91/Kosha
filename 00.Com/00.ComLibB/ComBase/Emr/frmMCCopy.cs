using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace ComBase
{ 
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\frmMCCopy" >> frmMCCopy.cs 폼이름 재정의" />

    public partial class frmMCCopy : Form
    {
        string FstrROWID = "";
        string FstrPANO = "";
        string FstrDept = "";
        string FstrDRCD = "";
        bool FbDOCTOR = false;
        string FstrVIEW = "";
        string FstrDrCode = "";

        string GSTRDRCODE = "";
        string GstrGPano = "";
        string GstrGDept = "";
        string GstrROWID = "";
        string strEXEName = "";
        string GstrHelpCode = "";

        public frmMCCopy(string GstrGPanoX, string GstrGDeptX, string GSTRDRCODEX, string GstrROWIDX, string strEXENameX, string GstrHelpCodeX)
        {
            GstrGPano = GstrGPanoX;
            GstrGDept = GstrGDeptX;
            GSTRDRCODE = GSTRDRCODEX;
            GstrROWID = GstrROWIDX;
            strEXEName = strEXENameX;
            GstrHelpCode = GstrHelpCodeX;

            InitializeComponent();
        }

        public frmMCCopy()
        {
            InitializeComponent();
        }

        private void frmMCCopy_Load(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;

            string SqlErr = "";
            SSList_Sheet1.Columns[3].Visible = false;
            SSList2_Sheet1.Columns[3].Visible = false;

            ComFunc CF = new ComFunc();

            txtSDATE.Text = CF.DATE_ADD(clsDB.DbCon, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), -60);
            txtEDATE.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            txtDATE.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            FstrVIEW = "";

            if (GstrGPano == "00000000")
            {
                FstrVIEW = "OK";
                GstrGPano = "";
            }

            txtPANO.Text = GstrGPano;
            FstrPANO = GstrGPano;
            txtNAME.Text = READ_PatientName(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            btnPrint.Enabled = false;
            FstrDept = GstrGDept;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (GSTRDRCODE == "")
                {
                    SQL = "";
                    SQL = " SELECT DRCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + " WHERE DOCCODE = " + clsType.User.Sabun;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        FstrDRCD = dt.Rows[0]["DRCODE"].ToString().Trim();
                    }
                    else
                    {
                        FstrDRCD = GSTRDRCODE;
                    }

                    dt.Dispose();
                    dt = null;
                }

                FstrROWID = GstrROWID;

                FbDOCTOR = false;

                SQL = "";
                SQL = " SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DOCCODE = " + clsType.User.Sabun;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    FbDOCTOR = true;
                }
                dt.Dispose();
                dt = null;

                cboDEPT.Text = GstrGDept;
                cboDRCD.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, FstrDRCD) + "            " + FstrDRCD;

                SET_COMBO_SMS(GstrGDept);

                SetVari();

                //TODO
                //mnuView_Click;

                if (FstrROWID == "")
                {
                    //mnuNew_Click
                }
                else
                {
                    //Call ReadDATA(FstrROWID)
                }



                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_PatientName(string argPano)
        {
            string SQL = "";
            string rtnVal = "";
            string SqlErr = "";
            DataTable dt = null;
            try
            {
                if (VB.Val(argPano) == 0)
                {
                    rtnVal = "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + argPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        private void SetVari()
        {
            FstrPANO = txtPANO.Text.Trim();
            FstrDept = cboDEPT.Text.Trim();
            FstrDRCD = VB.Right(cboDRCD.Text, 4);
        }

        private void SET_COMBO_SMS(string GstrGDept)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE, KORNAME, HTEL";
                SQL = SQL + ComNum.VBLF + " FROM (";
                SQL = SQL + ComNum.VBLF + "   SELECT B.DEPTCODE, A.KORNAME, NVL(A.MSTEL, A.HTEL) HTEL";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "    WHERE A.Sabun = b.Sabun";
                SQL = SQL + ComNum.VBLF + "      AND B.GBOUT = 'N'";
                SQL = SQL + ComNum.VBLF + "      AND A.BUSE >= '010000'";
                SQL = SQL + ComNum.VBLF + "      AND   A.BUSE < '030000'";
                SQL = SQL + ComNum.VBLF + "      AND A.TOIDAY IS NULL";
                SQL = SQL + ComNum.VBLF + "   Union All";
                SQL = SQL + ComNum.VBLF + "   SELECT A.DEPTCODE, A.SNAME, A.HTEL";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_PMPA.NUR_CHARGE_NURSE A, KOSMOS_ADM.INSA_MST B";
                SQL = SQL + ComNum.VBLF + "    WHERE a.Sabun = b.Sabun";
                SQL = SQL + ComNum.VBLF + "      AND B.TOIDAY IS NULL)";
                SQL = SQL + ComNum.VBLF + " WHERE HTEL Is Not Null";

                switch (cboDEPT.Text.Trim())
                {
                    case "MD":
                    case "MN":
                    case "MI":
                    case "MP":
                    case "MG":
                    case "MR":
                    case "MC":
                    case "ME":
                        SQL = SQL + ComNum.VBLF + "   AND (DEPTCODE = 'MD' OR DEPTCODE = '" + (cboDEPT.Text).Trim() + "')";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + (cboDEPT.Text).Trim() + "'";
                        break;
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY KORNAME";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ComboSMS.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboSMS.Items.Add(dt.Rows[i]["KORNAME"].ToString().Trim() + VB.Space(10) + (dt.Rows[i]["HTEL"].ToString().Trim()).Replace("-", ""));
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 0; i < ComboSMS.Items.Count; i++)
                {
                    ComboSMS.SelectedIndex = i;

                    if (VB.Left(cboDRCD.Text, 5) == VB.Left(ComboSMS.Text, 6))
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SSClear()
        {
            //◆ 기본정보


            //병원등록번호               
            ss1_Sheet1.Cells[2, 8].Text = FstrPANO;
            //환자성명
            ss1_Sheet1.Cells[2, 33].Text = READ_PatientName(FstrPANO);

            //신청인 성명        
            ss1_Sheet1.Cells[3, 8].Text = "";
            //환자와의 관계
            ss1_Sheet1.Cells[3, 33].Text = "";


            //◆ 제출서류 확인

            //환자본인 
            ss1_Sheet1.Cells[7, 0].Text = "";
            //본인 신분증
            ss1_Sheet1.Cells[7, 22].Text = "";


            //환자의 친족                             
            ss1_Sheet1.Cells[8, 0].Text = "";
            //신청인 신분증 
            ss1_Sheet1.Cells[8, 22].Text = "";
            //환자신분증 사본
            ss1_Sheet1.Cells[8, 37].Text = "";

            //가족관계 증명서 또는 주민등록등본
            ss1_Sheet1.Cells[9, 22].Text = "";

            //환자자필 동의서
            ss1_Sheet1.Cells[10, 22].Text = "";


            //대리인                  
            ss1_Sheet1.Cells[12, 0].Text = "";
            //신청인 신분증       
            ss1_Sheet1.Cells[12, 22].Text = "";
            //환자 신분증 사본
            ss1_Sheet1.Cells[12, 37].Text = "";

            //환자자필 위임장           
            ss1_Sheet1.Cells[13, 22].Text = "";
            //환자자필 동의서
            ss1_Sheet1.Cells[13, 37].Text = "";


            //◆ 사본발급용도
            //타 병원 진료용        
            ss1_Sheet1.Cells[16, 8].Text = "";
            //보험회사 제출용      
            ss1_Sheet1.Cells[16, 19].Text = "";
            //관계기관 제출용     
            ss1_Sheet1.Cells[16, 30].Text = "";
            //개인 보관용
            ss1_Sheet1.Cells[16, 41].Text = "";

            //병무청 제출용           
            ss1_Sheet1.Cells[17, 8].Text = "";
            //법원 제출용           
            ss1_Sheet1.Cells[17, 19].Text = "";
            //직장 제출용    
            ss1_Sheet1.Cells[17, 30].Text = "";
            //학교 제출용
            ss1_Sheet1.Cells[17, 41].Text = "";

            //기타                      
            ss1_Sheet1.Cells[18, 8].Text = "";
            //기타 내용
            ss1_Sheet1.Cells[18, 13].Text = "";

            //증명서 서식

            ss1_Sheet1.Cells[22, 0].Text = "";
            ss1_Sheet1.Cells[23, 0].Text = "";
            ss1_Sheet1.Cells[24, 0].Text = "";
            ss1_Sheet1.Cells[25, 0].Text = "";
            ss1_Sheet1.Cells[26, 0].Text = "";
            ss1_Sheet1.Cells[27, 0].Text = "";
            ss1_Sheet1.Cells[28, 0].Text = "";
            ss1_Sheet1.Cells[39, 0].Text = "";
            ss1_Sheet1.Cells[30, 0].Text = "";
            ss1_Sheet1.Cells[31, 0].Text = "";
            ss1_Sheet1.Cells[32, 0].Text = "";

            ss1_Sheet1.Cells[30, 0].Text = "";
            ss1_Sheet1.Cells[30, 5].Text = "";

            ss1_Sheet1.Cells[30, 0].Text = "";
            ss1_Sheet1.Cells[30, 5].Text = "";

            ss1_Sheet1.Cells[31, 0].Text = "";
            ss1_Sheet1.Cells[31, 4].Text = "";

            ss1_Sheet1.Cells[32, 0].Text = "";
            ss1_Sheet1.Cells[32, 4].Text = "";

            ss1_Sheet1.Cells[33, 0].Text = "";
            ss1_Sheet1.Cells[33, 4].Text = "";

            ss1_Sheet1.Cells[34, 0].Text = "";
            ss1_Sheet1.Cells[34, 4].Text = "";

            //외래

            ss1_Sheet1.Cells[22, 16].Text = "";
            ss1_Sheet1.Cells[23, 16].Text = "";
            ss1_Sheet1.Cells[24, 16].Text = "";
            ss1_Sheet1.Cells[25, 16].Text = "";
            ss1_Sheet1.Cells[26, 16].Text = "";
            ss1_Sheet1.Cells[27, 16].Text = "";
            ss1_Sheet1.Cells[28, 16].Text = "";

            ss1_Sheet1.Cells[29, 16].Text = "";
            ss1_Sheet1.Cells[30, 16].Text = "";

            ss1_Sheet1.Cells[30, 16].Text = "";
            ss1_Sheet1.Cells[30, 21].Text = "";
            ss1_Sheet1.Cells[30, 27].Text = "";

            ss1_Sheet1.Cells[31, 16].Text = "";

            ss1_Sheet1.Cells[32, 16].Text = "";
            ss1_Sheet1.Cells[32, 21].Text = "";
            ss1_Sheet1.Cells[32, 27].Text = "";

            ss1_Sheet1.Cells[33, 16].Text = "";
            ss1_Sheet1.Cells[33, 20].Text = "";

            ss1_Sheet1.Cells[34, 16].Text = "";
            ss1_Sheet1.Cells[34, 20].Text = "";

            //입원
            ss1_Sheet1.Cells[22, 33].Text = "";
            ss1_Sheet1.Cells[23, 33].Text = "";
            ss1_Sheet1.Cells[24, 33].Text = "";
            ss1_Sheet1.Cells[25, 33].Text = "";
            ss1_Sheet1.Cells[26, 33].Text = "";
            ss1_Sheet1.Cells[27, 33].Text = "";
            ss1_Sheet1.Cells[28, 33].Text = "";

            ss1_Sheet1.Cells[29, 33].Text = "";
            ss1_Sheet1.Cells[30, 33].Text = "";

            ss1_Sheet1.Cells[30, 33].Text = "";
            ss1_Sheet1.Cells[30, 38].Text = "";
            ss1_Sheet1.Cells[30, 44].Text = "";

            ss1_Sheet1.Cells[31, 33].Text = "";

            ss1_Sheet1.Cells[32, 33].Text = "";
            ss1_Sheet1.Cells[32, 38].Text = "";
            ss1_Sheet1.Cells[32, 44].Text = "";

            ss1_Sheet1.Cells[33, 33].Text = "";
            ss1_Sheet1.Cells[33, 37].Text = "";

            ss1_Sheet1.Cells[34, 33].Text = "";
            ss1_Sheet1.Cells[34, 37].Text = "";

            //작성일
            ss1_Sheet1.Cells[38, 34].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            ss1_Sheet1.Cells[39, 34].Text = "";
            ss1_Sheet1.Cells[40, 34].Text = "";

            //서명
            ss1_Sheet1.Cells[40, 45].Text = "";
        }

        private void SSList2_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0)
            {
                return;
            }

            SSClear();
            FstrROWID = ss1_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (FstrROWID != "")
            {
                ReadDATA(FstrROWID, "Y");
            }
        }

        private void txtPANO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPANO.Text = txtPANO.Text.PadLeft(8, '0');	//0 숫자 포맷형식 8자리 채우기
                txtNAME.Text = READ_PatientName(txtPANO.Text);
            }


        }

        private void cboDEPT_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strData = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT DrCode,DrName FROM " + ComNum.DB_PMPA + "BAS_Doctor ";
                SQL = SQL + ComNum.VBLF + " WHERE (Tour IS NULL OR Tour='N') ";
                SQL = SQL + ComNum.VBLF + "   AND DrDept1='" + (cboDEPT.Text).Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cboDRCD.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDRCD.Items.Add(clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["Drcodr"].ToString().Trim()
                            + "            " +
                            dt.Rows[i]["Drcode"].ToString().Trim()));
                    }
                }

                SET_COMBO_SMS(cboDEPT.Text.Trim());

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            string strDocCode = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            if (FstrROWID == "")
            {
                if (ComFunc.MsgBoxQ("신청서를 선택하십시요.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (VB.UCase(strEXEName) == "EMRPRT")
                {
                    SQL = "";
                    SQL = " SELECT DocCode FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + FstrDrCode + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strDocCode = dt.Rows[0]["DocCode"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }


                SQL = "";
                SQL = " UPDATE " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST SET ";

                if (VB.UCase(strEXEName) == "EMRPRT")
                {
                    SQL = SQL + ComNum.VBLF + " SIGNSABUN = " + strDocCode + ", ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SIGNSABUN = " + clsType.User.Sabun + ", ";
                }

                SQL = SQL + ComNum.VBLF + " SIGNDATE = SYSDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("승인되었습니다.");
                Cursor.Current = Cursors.Default;

                if (GstrHelpCode == "ORD")
                {
                    SSClear();
                    Search();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void comboSet()
        {
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            if (string.Compare(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"), "2010-10-01") < 0)
            {
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN') ";//  '2005-08-09 ER제외
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN','MD') ";
            }
            SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                cboDEPT.Items.Clear();

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDEPT.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboDEPT.Text = GstrGDept;
            cboDRCD.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, FstrDRCD) + "            " + FstrDRCD;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            if (FstrROWID == "")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("삭제 시 복원 불가능, 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO KOSMOS_OCS" + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST_HIS ";
                SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                SQL = "";
                SQL = " DELETE KOSMOS_OCS.OCS_MCCERTIFI_REQUEST";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FstrROWID = "";
            if (FstrPANO == "")
            {
                MessageBox.Show("환자를 선택하시기 바랍니다");
                return;
            }

            MessageBox.Show("신규 작성입니다.", "사본발급신청서");
            SSClear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }


            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT PRTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL = " UPDATE " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
                    SQL = SQL + ComNum.VBLF + " SET PRTDATE = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "  PRTSABUN = " + clsType.User.Sabun;
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);

                strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;

                this.Close();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetVari();
            if (setData() == false)
            {
                MessageBox.Show("setData 오류");
                return;

            }

            if (panSMS.Visible == true)
            {
                SEND_SMS(VB.Right(ComboSMS.Text, 20));
            }
        }

        private void SEND_SMS(string arg)
        {
            string strMsg = "";
            string strDateTime = "";
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            strDateTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            strMsg = "※" + FstrDept + "/" + READ_PatientName(FstrPANO) + "(" + FstrPANO + ")님 사본발급신청서가 작성되었습니다. 승인하여 주시기 바랍니다.";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "insert into " + ComNum.DB_PMPA + "ETC_SMS(";
                SQL = SQL + ComNum.VBLF + "JOBDATE, SNAME, HPHONE, RETTEL, GUBUN, SENDMSG,GBPUSH) VALUES(";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + strDateTime + "', 'YYYY-MM-DD HH24:MI'),";
                SQL = SQL + ComNum.VBLF + "'사본발급', '" + arg + "', '054-260-8041', '67', '" + strMsg + "','N')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private bool setData()
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            DataTable dt = null;

            string strSEQNO = "";
            string strPtNo = "";
            string StrDrCode = "";
            string strDeptCode = "";
            string strLSDATE = "";
            string strSName = "";
            string strSNAMER = "";
            string strWRITEDATE = "";
            string strWRITESABUN = "";
            string strBUN1_01 = "";
            string strBUN1_02 = "";
            string strBUN1_03 = "";
            string strBUN1_04 = "";
            string strBUN1_05 = "";
            string strBUN1_06 = "";
            string strBUN1_07 = "";
            string strBUN1_08 = "";
            string strBUN1_09 = "";
            string strBUN1_10 = "";
            string strBUN1_11 = "";
            string strBUN1_12 = "";
            string strBUN2_01 = "";
            string strBUN2_02 = "";
            string strBUN2_03 = "";
            string strBUN2_04 = "";
            string strBUN2_05 = "";
            string strBUN2_06 = "";
            string strBUN2_07 = "";
            string strBUN2_08 = "";
            string strBUN2_09 = "";
            string strBUN2_09_MEMO = "";


            string strBUN3_01 = "";
            string strBUN3_02 = "";
            string strBUN3_03 = "";
            string strBUN3_04 = "";
            string strBUN3_05 = "";
            string strBUN3_06 = "";
            string strBUN3_07 = "";
            string strBUN3_08 = "";
            string strBUN3_09 = "";
            string strBUN3_10 = "";
            string strBUN3_11 = "";
            string strBUN3_12 = "";
            string strBUN3_12_MEMO = "";
            string strBUN3_13 = "";
            string strBUN3_13_MEMO = "";
            string strBUN3_14 = "";
            string strBUN3_14_MEMO = "";
            string strBUN3_15 = "";
            string strBUN3_16 = "";
            string strBUN3_17 = "";
            string strBUN3_18 = "";
            string strBUN3_19 = "";


            string strBUN4_01 = "";
            string strBUN4_02 = "";
            string strBUN4_03 = "";
            string strBUN4_04 = "";
            string strBUN4_05 = "";
            string strBUN4_06 = "";
            string strBUN4_07 = "";
            string strBUN4_08 = "";
            string strBUN4_09 = "";
            string strBUN4_10 = "";
            string strBUN4_11 = "";
            string strBUN4_12 = "";
            string strBUN4_13 = "";
            string strBUN4_13_MEMO = "";
            string strBUN4_14 = "";
            string strBUN4_14_MEMO = "";
            string strBUN4_15 = "";
            string strBUN4_16 = "";
            string strBUN4_17 = "";
            string strBUN4_18 = "";


            string strBUN5_01 = "";
            string strBUN_02 = "";
            string strBUN_03 = "";
            string strBUN_04 = "";
            string strBUN_05 = "";
            string strBUN_06 = "";
            string strBUN_07 = "";
            string strBUN_08 = "";
            string strBUN_09 = "";

            string strBUN5_02 = "";
            string strBUN5_03 = "";
            string strBUN5_04 = "";
            string strBUN5_05 = "";
            string strBUN5_06 = "";
            string strBUN5_07 = "";
            string strBUN5_08 = "";
            string strBUN5_09 = "";
            string strBUN5_10 = "";
            string strBUN5_10_MEMO = "";
            string strBUN5_11 = "";
            string strBUN5_11_MEMO = "";
            string strBUN5_12 = "";
            string strBUN5_12_MEMO = "";
            string strBUN5_13 = "";
            string strBUN5_13_MEMO = "";
            string strBUN5_14 = "";
            string strBUN5_14_MEMO = "";

            Cursor.Current = Cursors.WaitCursor;

            if (FstrROWID != "")
            {
                if (ComFunc.MsgBoxQ("기존 내용을 변경합니다. 계속 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return rtnVal;
                }

                return rtnVal;
            }

            //'◆ 기본정보
            //'신청인 성명                
            strSNAMER = ss1_Sheet1.Cells[3, 8].Text.Trim();
            //'환자와의 관계
            strSNAMER = ss1_Sheet1.Cells[3, 33].Text.Trim();

            //◆ 제출서류 확인
            strBUN1_01 = ss1_Sheet1.Cells[7, 0].Text.Trim();
            strBUN1_02 = ss1_Sheet1.Cells[7, 22].Text.Trim();

            //환자의 친족
            strBUN1_03 = ss1_Sheet1.Cells[8, 0].Text.Trim();
            strBUN1_04 = ss1_Sheet1.Cells[8, 22].Text.Trim();
            strBUN1_05 = ss1_Sheet1.Cells[8, 37].Text.Trim();

            //가족관계 증명서 또는 주민등록등본
            strBUN1_06 = ss1_Sheet1.Cells[9, 22].Text.Trim();
            //환자자필 동의서
            strBUN1_07 = ss1_Sheet1.Cells[10, 22].Text.Trim();

            //대리인 
            strBUN1_08 = ss1_Sheet1.Cells[12, 0].Text.Trim();

            //신청인 신분증 
            strBUN1_09 = ss1_Sheet1.Cells[12, 22].Text.Trim();

            //환자 신분증 사본
            strBUN1_10 = ss1_Sheet1.Cells[12, 37].Text.Trim();

            //환자자필 위임장  
            strBUN1_11 = ss1_Sheet1.Cells[13, 22].Text.Trim();

            //환자자필 동의서
            strBUN1_12 = ss1_Sheet1.Cells[13, 37].Text.Trim();

            //◆ 사본발급용도
            //타 병원 진료용
            strBUN2_01 = ss1_Sheet1.Cells[16, 8].Text.Trim();

            //보험회사 제출용 
            strBUN2_02 = ss1_Sheet1.Cells[16, 19].Text.Trim();

            //관계기관 제출용 
            strBUN2_03 = ss1_Sheet1.Cells[16, 30].Text.Trim();

            //개인 보관용
            strBUN2_04 = ss1_Sheet1.Cells[16, 41].Text.Trim();

            //병무청 제출용
            strBUN2_05 = ss1_Sheet1.Cells[17, 8].Text.Trim();

            //법원 제출용  
            strBUN2_06 = ss1_Sheet1.Cells[17, 19].Text.Trim();

            //직장 제출용
            strBUN2_07 = ss1_Sheet1.Cells[17, 30].Text.Trim();

            //학교 제출용
            strBUN2_08 = ss1_Sheet1.Cells[17, 41].Text.Trim();


            //기타
            strBUN2_09 = ss1_Sheet1.Cells[18, 8].Text.Trim();

            //기타 내용
            strBUN2_09_MEMO = ss1_Sheet1.Cells[18, 13].Text.Trim();

            //증명서 서식
            strBUN5_02 = ss1_Sheet1.Cells[22, 0].Text.Trim();
            strBUN5_03 = ss1_Sheet1.Cells[23, 0].Text.Trim();
            strBUN5_04 = ss1_Sheet1.Cells[24, 0].Text.Trim();
            strBUN5_05 = ss1_Sheet1.Cells[25, 0].Text.Trim();
            strBUN5_06 = ss1_Sheet1.Cells[26, 0].Text.Trim();
            strBUN5_07 = ss1_Sheet1.Cells[27, 0].Text.Trim();
            strBUN5_08 = ss1_Sheet1.Cells[28, 0].Text.Trim();
            strBUN5_09 = ss1_Sheet1.Cells[29, 0].Text.Trim();

            strBUN5_10 = ss1_Sheet1.Cells[30, 0].Text.Trim();
            strBUN5_10_MEMO = ss1_Sheet1.Cells[30, 5].Text.Trim();

            strBUN5_11 = ss1_Sheet1.Cells[31, 0].Text.Trim();
            strBUN5_11_MEMO = ss1_Sheet1.Cells[31, 4].Text.Trim();

            strBUN5_12 = ss1_Sheet1.Cells[32, 0].Text.Trim();
            strBUN5_12_MEMO = ss1_Sheet1.Cells[32, 4].Text.Trim();

            strBUN5_13 = ss1_Sheet1.Cells[33, 0].Text.Trim();
            strBUN5_13_MEMO = ss1_Sheet1.Cells[33, 4].Text.Trim();

            strBUN5_14 = ss1_Sheet1.Cells[34, 0].Text.Trim();
            strBUN5_14_MEMO = ss1_Sheet1.Cells[34, 4].Text.Trim();

            //외래
            strBUN3_02 = ss1_Sheet1.Cells[22, 16].Text.Trim();
            strBUN3_03 = ss1_Sheet1.Cells[23, 16].Text.Trim();
            strBUN3_15 = ss1_Sheet1.Cells[24, 16].Text.Trim();
            strBUN3_04 = ss1_Sheet1.Cells[25, 16].Text.Trim();
            strBUN3_05 = ss1_Sheet1.Cells[26, 16].Text.Trim();
            strBUN3_06 = ss1_Sheet1.Cells[27, 16].Text.Trim();
            strBUN3_07 = ss1_Sheet1.Cells[28, 16].Text.Trim();

            strBUN3_08 = ss1_Sheet1.Cells[29, 16].Text.Trim();

            strBUN3_09 = ss1_Sheet1.Cells[30, 16].Text.Trim();
            strBUN3_10 = ss1_Sheet1.Cells[30, 21].Text.Trim();
            strBUN3_11 = ss1_Sheet1.Cells[30, 27].Text.Trim();

            strBUN3_16 = ss1_Sheet1.Cells[31, 16].Text.Trim();

            strBUN3_17 = ss1_Sheet1.Cells[32, 16].Text.Trim();
            strBUN3_18 = ss1_Sheet1.Cells[32, 21].Text.Trim();
            strBUN3_19 = ss1_Sheet1.Cells[32, 27].Text.Trim();

            strBUN3_12 = ss1_Sheet1.Cells[33, 16].Text.Trim();
            strBUN3_12_MEMO = ss1_Sheet1.Cells[33, 20].Text.Trim();

            strBUN3_13 = ss1_Sheet1.Cells[34, 16].Text.Trim();
            strBUN3_13_MEMO = ss1_Sheet1.Cells[34, 20].Text.Trim();

            //입원
            strBUN4_02 = ss1_Sheet1.Cells[22, 33].Text.Trim();
            strBUN4_03 = ss1_Sheet1.Cells[23, 33].Text.Trim();
            strBUN4_04 = ss1_Sheet1.Cells[24, 33].Text.Trim();
            strBUN4_05 = ss1_Sheet1.Cells[25, 33].Text.Trim();
            strBUN4_06 = ss1_Sheet1.Cells[26, 33].Text.Trim();
            strBUN4_07 = ss1_Sheet1.Cells[27, 33].Text.Trim();
            strBUN4_08 = ss1_Sheet1.Cells[28, 33].Text.Trim();

            strBUN4_09 = ss1_Sheet1.Cells[29, 33].Text.Trim();

            strBUN4_10 = ss1_Sheet1.Cells[30, 33].Text.Trim();
            strBUN4_11 = ss1_Sheet1.Cells[30, 38].Text.Trim();
            strBUN4_12 = ss1_Sheet1.Cells[30, 44].Text.Trim();

            strBUN4_15 = ss1_Sheet1.Cells[31, 33].Text.Trim();

            strBUN4_16 = ss1_Sheet1.Cells[32, 33].Text.Trim();
            strBUN4_17 = ss1_Sheet1.Cells[32, 38].Text.Trim();
            strBUN4_18 = ss1_Sheet1.Cells[32, 44].Text.Trim();

            strBUN4_13 = ss1_Sheet1.Cells[33, 33].Text.Trim();
            strBUN4_13_MEMO = ss1_Sheet1.Cells[33, 37].Text.Trim();

            strBUN4_14 = ss1_Sheet1.Cells[34, 33].Text.Trim();
            strBUN4_14_MEMO = ss1_Sheet1.Cells[34, 37].Text.Trim();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID != "")
                {
                    SQL = "";
                    SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST_DEL ";
                    SQL = SQL + ComNum.VBLF + " SELECT * FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = " DELETE " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                SQL = "";
                SQL = "SELECT " + ComNum.DB_MED + "SEQ_MCCERTIFIREQUEST.NEXTVAL SEQ FROM DUAL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strSEQNO = dt.Rows[0]["SEQ"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                strPtNo = txtPANO.Text;
                StrDrCode = VB.Right(cboDRCD.Text, 4);
                strDeptCode = (cboDEPT.Text).Trim();
                strLSDATE = (txtDATE.Text).Trim();


                SQL = "";

                SQL = " INSERT INTO " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST(";
                SQL = SQL + ComNum.VBLF + "  SEQNO, PTNO, DRCODE, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + "  LSDATE, SNAME, SNAMER, ";
                SQL = SQL + ComNum.VBLF + " WRITEDATE, WRITESABUN, BUN1_01, BUN1_02,";
                SQL = SQL + ComNum.VBLF + " BUN1_03, BUN1_04, BUN1_05, BUN1_06,";
                SQL = SQL + ComNum.VBLF + " BUN1_07, BUN1_08, BUN1_09, BUN1_10,";
                SQL = SQL + ComNum.VBLF + " BUN1_11, BUN1_12, BUN2_01, BUN2_02,";
                SQL = SQL + ComNum.VBLF + " BUN2_03, BUN2_04, BUN2_05, BUN2_06,";
                SQL = SQL + ComNum.VBLF + " BUN2_07, BUN2_08, BUN2_09, BUN2_09_MEMO,";
                SQL = SQL + ComNum.VBLF + " BUN3_01, BUN3_02, BUN3_03, BUN3_04,";
                SQL = SQL + ComNum.VBLF + " BUN3_05, BUN3_06, BUN3_07, BUN3_08,";
                SQL = SQL + ComNum.VBLF + " BUN3_09, BUN3_10, BUN3_11, BUN3_12,";
                SQL = SQL + ComNum.VBLF + " BUN3_12_MEMO, BUN3_13, BUN3_13_MEMO, BUN3_14,";
                SQL = SQL + ComNum.VBLF + " BUN3_14_MEMO, BUN4_01, BUN4_02, BUN4_03,";
                SQL = SQL + ComNum.VBLF + " BUN4_04, BUN4_05, BUN4_06, BUN4_07,";
                SQL = SQL + ComNum.VBLF + " BUN4_08, BUN4_09, BUN4_10, BUN4_11,";
                SQL = SQL + ComNum.VBLF + " BUN4_12, BUN4_13, BUN4_13_MEMO, BUN4_14,";
                SQL = SQL + ComNum.VBLF + " BUN4_14_MEMO, ";
                SQL = SQL + ComNum.VBLF + " BUN3_15, BUN3_16, BUN3_17, BUN3_18,";
                SQL = SQL + ComNum.VBLF + " BUN3_19, BUN4_15, BUN4_16, BUN4_17, ";
                SQL = SQL + ComNum.VBLF + " BUN4_18, ";
                SQL = SQL + ComNum.VBLF + " BUN5_01, BUN5_02, BUN5_03, BUN5_04, ";
                SQL = SQL + ComNum.VBLF + " BUN5_05, BUN5_06, BUN5_07, BUN5_08, ";
                SQL = SQL + ComNum.VBLF + " BUN5_09, BUN5_10, BUN5_10_MEMO, BUN5_11, ";
                SQL = SQL + ComNum.VBLF + " BUN5_11_MEMO, BUN5_12, BUN5_12_MEMO, BUN5_13, ";
                SQL = SQL + ComNum.VBLF + " BUN5_13_MEMO, BUN5_14, BUN5_14_MEMO     ";
                SQL = SQL + ComNum.VBLF + " ) VALUES (";
                SQL = SQL + ComNum.VBLF + "'" + strSEQNO + "','" + strPtNo + "','" + StrDrCode + "','" + strDeptCode + "',";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + strLSDATE + "','YYYY-MM-DD'),'" + strSName + "','" + strSNAMER + "',";
                SQL = SQL + ComNum.VBLF + "SYSDATE, " + clsType.User.Sabun + ", '" + strBUN1_01 + "','" + strBUN1_02 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN1_03 + "','" + strBUN1_04 + "','" + strBUN1_05 + "','" + strBUN1_06 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN1_07 + "','" + strBUN1_08 + "','" + strBUN1_09 + "','" + strBUN1_10 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN1_11 + "','" + strBUN1_12 + "','" + strBUN2_01 + "','" + strBUN2_02 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN2_03 + "','" + strBUN2_04 + "','" + strBUN2_05 + "','" + strBUN2_06 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN2_07 + "','" + strBUN2_08 + "','" + strBUN2_09 + "','" + strBUN2_09_MEMO + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_01 + "','" + strBUN3_02 + "','" + strBUN3_03 + "','" + strBUN3_04 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_05 + "','" + strBUN3_06 + "','" + strBUN3_07 + "','" + strBUN3_08 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_09 + "','" + strBUN3_10 + "','" + strBUN3_11 + "','" + strBUN3_12 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_12_MEMO + "','" + strBUN3_13 + "','" + strBUN3_13_MEMO + "','" + strBUN3_14 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_14_MEMO + "','" + strBUN4_01 + "','" + strBUN4_02 + "','" + strBUN4_03 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN4_04 + "','" + strBUN4_05 + "','" + strBUN4_06 + "','" + strBUN4_07 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN4_08 + "','" + strBUN4_09 + "','" + strBUN4_10 + "','" + strBUN4_11 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN4_12 + "','" + strBUN4_13 + "','" + strBUN4_13_MEMO + "','" + strBUN4_14 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN4_14_MEMO + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_15 + "','" + strBUN3_16 + "','" + strBUN3_17 + "','" + strBUN3_18 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN3_19 + "','" + strBUN4_15 + "','" + strBUN4_16 + "','" + strBUN4_17 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN4_18 + "',";
                SQL = SQL + ComNum.VBLF + "'" + strBUN5_01 + "','" + strBUN5_02 + "','" + strBUN5_03 + "','" + strBUN5_04 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN5_05 + "','" + strBUN5_06 + "','" + strBUN5_07 + "','" + strBUN5_08 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN5_09 + "','" + strBUN5_10 + "','" + strBUN5_10_MEMO + "','" + strBUN5_11 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN5_11_MEMO + "','" + strBUN5_12 + "','" + strBUN5_12_MEMO + "','" + strBUN5_13 + "', ";
                SQL = SQL + ComNum.VBLF + "'" + strBUN5_13_MEMO + "','" + strBUN5_14 + "','" + strBUN5_14_MEMO + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                if (GstrHelpCode == "ORD")
                {
                    SSClear();
                    Search();
                }
                else
                {
                    this.Close();
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string strDRDEPT = "";
            string strDeptCode = "";

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE LIKE 'M%' or DEPTCODE in ('HU') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDeptCode = strDeptCode + "'" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "',";
                    }

                    strDeptCode = VB.Mid(strDeptCode, 1, VB.Len(strDeptCode) - 1);
                }

                dt.Dispose();
                dt = null;

                SSList_Sheet1.RowCount = 0;
                SSList2_Sheet1.RowCount = 0;


                SQL = "";
                SQL = " SELECT DEPTCODE, GRADE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DOCCODE = " + clsType.User.Sabun;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GRADE"].ToString().Trim() == "1")
                    {
                        strDRDEPT = "";
                    }
                    else
                    {
                        strDRDEPT = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    }
                }
                else
                {

                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT LSDATE, PTNO, DEPTCODE, DRCODE, ROWID";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST";

                if (FstrVIEW == "OK" && (txtPANO.Text).Trim() == "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE SIGNDATE IS NULL";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + (txtPANO.Text).Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND SIGNDATE IS NULL";
                }

                if (strDRDEPT != "")
                {
                    switch (strDRDEPT)
                    {
                        case "MD":
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IN (" + strDeptCode + ")";
                            break;

                        case "OS":
                        case "GS":
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDRDEPT + "' ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + "   AND 1 = 2";
                            break;

                    }
                }
                else if (FstrDRCD != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + FstrDRCD + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND WRITEDATE >= TO_DATE('" + txtSDATE.Text + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND WRITEDATE <= TO_DATE('" + txtEDATE.Text + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY LSDATE DESC, DEPTCODE, DRCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["SIGNSABUN"].ToString().Trim());
                        SSList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["PRTDATE"].ToString().Trim() == "")
                        {
                            SSList_Sheet1.Cells[i, 4].Text = "";
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 4].Text = "Y";
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0)
            {
                return;
            }

            SSClear();

            FstrROWID = SSList_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (FstrROWID != "")
            {
                ReadDATA(FstrROWID, "N");
            }
        }

        private void ReadDATA(string argROWID, string argPRT = "")
        {
            string strLSDATE = "";
            string strSIGNSABUN = "";

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            ComFunc CF = new ComFunc();


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + argROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count < 1)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                panSMS.Visible = true;
                panSMS.Visible = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;

                if (dt.Rows[0]["SIGNDATE"].ToString().Trim() == "")
                {
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSign.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSign.Enabled = false;
                    panSMS.Visible = false;
                    btnPrint.Enabled = true;
                }

                if (FbDOCTOR == false)
                {
                    btnSign.Enabled = false;
                }
                else
                {
                    btnSign.Enabled = true;
                }

                //◆ 기본정보

                //'병원등록번호              
                ss1_Sheet1.Cells[2, 8].Text = dt.Rows[0]["PTNO"].ToString().Trim();
                //'환자성명
                ss1_Sheet1.Cells[2, 33].Text = dt.Rows[0]["PTNO"].ToString().Trim();

                //'신청인 성명                '환자와의 관계
                ss1_Sheet1.Cells[3, 8].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                ss1_Sheet1.Cells[3, 33].Text = dt.Rows[0]["SNAMER"].ToString().Trim();

                //'◆ 제출서류 확인
                //'환자본인                   '본인 신분증
                ss1_Sheet1.Cells[7, 0].Text = dt.Rows[0]["BUN1_01"].ToString().Trim();
                ss1_Sheet1.Cells[7, 22].Text = dt.Rows[0]["BUN1_02"].ToString().Trim();

                //'환자의 친족                '신청인 신분증              '환자신분증 사본
                ss1_Sheet1.Cells[8, 0].Text = dt.Rows[0]["BUN1_03"].ToString().Trim();
                ss1_Sheet1.Cells[8, 22].Text = dt.Rows[0]["BUN1_04"].ToString().Trim();
                ss1_Sheet1.Cells[8, 37].Text = dt.Rows[0]["BUN1_05"].ToString().Trim();

                //'가족관계 증명서 또는 주민등록등본
                ss1_Sheet1.Cells[9, 22].Text = dt.Rows[0]["BUN1_06"].ToString().Trim();

                //'환자자필 동의서
                ss1_Sheet1.Cells[10, 22].Text = dt.Rows[0]["BUN1_07"].ToString().Trim();

                //'대리인                     '신청인 신분증              '환자 신분증 사본
                ss1_Sheet1.Cells[12, 0].Text = dt.Rows[0]["BUN1_08"].ToString().Trim();
                ss1_Sheet1.Cells[12, 22].Text = dt.Rows[0]["BUN1_09"].ToString().Trim();
                ss1_Sheet1.Cells[12, 37].Text = dt.Rows[0]["BUN1_10"].ToString().Trim();

                //'환자자필 위임장            '환자자필 동의서
                ss1_Sheet1.Cells[13, 22].Text = dt.Rows[0]["BUN1_11"].ToString().Trim();
                ss1_Sheet1.Cells[13, 37].Text = dt.Rows[0]["BUN1_12"].ToString().Trim();

                //'◆ 사본발급용도
                //'타 병원 진료용             '보험회사 제출용            '관계기관 제출용            '개인 보관용
                ss1_Sheet1.Cells[16, 8].Text = dt.Rows[0]["BUN2_01"].ToString().Trim();
                ss1_Sheet1.Cells[16, 19].Text = dt.Rows[0]["BUN2_02"].ToString().Trim();
                ss1_Sheet1.Cells[16, 30].Text = dt.Rows[0]["BUN2_03"].ToString().Trim();
                ss1_Sheet1.Cells[16, 41].Text = dt.Rows[0]["BUN2_04"].ToString().Trim();

                //'병무청 제출용              '법원 제출용                '직장 제출용                '학교 제출용
                ss1_Sheet1.Cells[17, 8].Text = dt.Rows[0]["BUN2_05"].ToString().Trim();
                ss1_Sheet1.Cells[17, 19].Text = dt.Rows[0]["BUN2_06"].ToString().Trim();
                ss1_Sheet1.Cells[17, 30].Text = dt.Rows[0]["BUN2_07"].ToString().Trim();
                ss1_Sheet1.Cells[17, 41].Text = dt.Rows[0]["BUN2_08"].ToString().Trim();

                //'기타                       '기타 내용
                ss1_Sheet1.Cells[18, 8].Text = dt.Rows[0]["BUN2_09"].ToString().Trim();
                ss1_Sheet1.Cells[18, 13].Text = dt.Rows[0]["BUN2_09_MEMO"].ToString().Trim();


                //'증명서 서식
                //.Col = 1:
                //  '.Row = 22:  SS1.Text = Trim(AdoGetString(Rs, "BUN5_01", 0))
                //ss1_Sheet1.Cells[21, 0].Text = dt.Rows[0]["BUN5_01"].ToString().Trim();
                ss1_Sheet1.Cells[22, 0].Text = dt.Rows[0]["BUN5_02"].ToString().Trim();
                ss1_Sheet1.Cells[23, 0].Text = dt.Rows[0]["BUN5_03"].ToString().Trim();
                ss1_Sheet1.Cells[24, 0].Text = dt.Rows[0]["BUN5_04"].ToString().Trim();
                ss1_Sheet1.Cells[25, 0].Text = dt.Rows[0]["BUN5_05"].ToString().Trim();
                ss1_Sheet1.Cells[26, 0].Text = dt.Rows[0]["BUN5_06"].ToString().Trim();
                ss1_Sheet1.Cells[27, 0].Text = dt.Rows[0]["BUN5_07"].ToString().Trim();
                ss1_Sheet1.Cells[28, 0].Text = dt.Rows[0]["BUN5_08"].ToString().Trim();
                ss1_Sheet1.Cells[29, 0].Text = dt.Rows[0]["BUN5_09"].ToString().Trim();
                ss1_Sheet1.Cells[30, 0].Text = dt.Rows[0]["BUN5_10"].ToString().Trim();
                ss1_Sheet1.Cells[31, 0].Text = dt.Rows[0]["BUN5_09"].ToString().Trim();
                ss1_Sheet1.Cells[32, 0].Text = dt.Rows[0]["BUN5_09"].ToString().Trim();

                ss1_Sheet1.Cells[31, 0].Text = dt.Rows[0]["BUN5_10"].ToString().Trim();
                ss1_Sheet1.Cells[31, 5].Text = dt.Rows[0]["BUN5_10_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[31, 0].Text = dt.Rows[0]["BUN5_11"].ToString().Trim();
                ss1_Sheet1.Cells[31, 4].Text = dt.Rows[0]["BUN5_11_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[32, 0].Text = dt.Rows[0]["BUN5_12"].ToString().Trim();
                ss1_Sheet1.Cells[32, 4].Text = dt.Rows[0]["BUN5_12_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[33, 0].Text = dt.Rows[0]["BUN5_13"].ToString().Trim();
                ss1_Sheet1.Cells[33, 4].Text = dt.Rows[0]["BUN5_13_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[34, 0].Text = dt.Rows[0]["BUN5_14"].ToString().Trim();
                ss1_Sheet1.Cells[34, 4].Text = dt.Rows[0]["BUN5_14_MEMO"].ToString().Trim();

                //'외래

                ss1_Sheet1.Cells[22, 16].Text = dt.Rows[0]["BUN3_02"].ToString().Trim();
                ss1_Sheet1.Cells[23, 16].Text = dt.Rows[0]["BUN3_03"].ToString().Trim();
                ss1_Sheet1.Cells[24, 16].Text = dt.Rows[0]["BUN3_15"].ToString().Trim();
                ss1_Sheet1.Cells[25, 16].Text = dt.Rows[0]["BUN3_04"].ToString().Trim();
                ss1_Sheet1.Cells[26, 16].Text = dt.Rows[0]["BUN3_05"].ToString().Trim();
                ss1_Sheet1.Cells[27, 16].Text = dt.Rows[0]["BUN3_06"].ToString().Trim();
                ss1_Sheet1.Cells[28, 16].Text = dt.Rows[0]["BUN3_07"].ToString().Trim();

                ss1_Sheet1.Cells[29, 16].Text = dt.Rows[0]["BUN3_08"].ToString().Trim();

                ss1_Sheet1.Cells[30, 16].Text = dt.Rows[0]["BUN3_09"].ToString().Trim();
                ss1_Sheet1.Cells[30, 16].Text = dt.Rows[0]["BUN3_10"].ToString().Trim();
                ss1_Sheet1.Cells[30, 16].Text = dt.Rows[0]["BUN3_11"].ToString().Trim();

                ss1_Sheet1.Cells[31, 16].Text = dt.Rows[0]["BUN3_16"].ToString().Trim();

                ss1_Sheet1.Cells[32, 16].Text = dt.Rows[0]["BUN3_17"].ToString().Trim();
                ss1_Sheet1.Cells[32, 21].Text = dt.Rows[0]["BUN3_18"].ToString().Trim();
                ss1_Sheet1.Cells[32, 27].Text = dt.Rows[0]["BUN3_19"].ToString().Trim();

                ss1_Sheet1.Cells[33, 16].Text = dt.Rows[0]["BUN3_12"].ToString().Trim();
                ss1_Sheet1.Cells[33, 20].Text = dt.Rows[0]["BUN3_12_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[34, 16].Text = dt.Rows[0]["BUN3_13"].ToString().Trim();
                ss1_Sheet1.Cells[34, 20].Text = dt.Rows[0]["BUN3_13_MEMO"].ToString().Trim();

                //         '입원
                ss1_Sheet1.Cells[22, 33].Text = dt.Rows[0]["BUN4_02"].ToString().Trim();
                ss1_Sheet1.Cells[23, 33].Text = dt.Rows[0]["BUN4_03"].ToString().Trim();
                ss1_Sheet1.Cells[24, 33].Text = dt.Rows[0]["BUN4_04"].ToString().Trim();
                ss1_Sheet1.Cells[25, 33].Text = dt.Rows[0]["BUN4_05"].ToString().Trim();
                ss1_Sheet1.Cells[26, 33].Text = dt.Rows[0]["BUN4_06"].ToString().Trim();
                ss1_Sheet1.Cells[27, 33].Text = dt.Rows[0]["BUN4_07"].ToString().Trim();
                ss1_Sheet1.Cells[28, 33].Text = dt.Rows[0]["BUN4_08"].ToString().Trim();

                ss1_Sheet1.Cells[29, 33].Text = dt.Rows[0]["BUN4_09"].ToString().Trim();

                ss1_Sheet1.Cells[30, 33].Text = dt.Rows[0]["BUN4_10"].ToString().Trim();
                ss1_Sheet1.Cells[30, 38].Text = dt.Rows[0]["BUN4_11"].ToString().Trim();
                ss1_Sheet1.Cells[30, 44].Text = dt.Rows[0]["BUN4_12"].ToString().Trim();

                ss1_Sheet1.Cells[31, 33].Text = dt.Rows[0]["BUN4_15"].ToString().Trim();

                ss1_Sheet1.Cells[32, 33].Text = dt.Rows[0]["BUN4_16"].ToString().Trim();
                ss1_Sheet1.Cells[32, 38].Text = dt.Rows[0]["BUN4_17"].ToString().Trim();
                ss1_Sheet1.Cells[32, 44].Text = dt.Rows[0]["BUN4_18"].ToString().Trim();

                ss1_Sheet1.Cells[33, 33].Text = dt.Rows[0]["BUN4_13"].ToString().Trim();
                ss1_Sheet1.Cells[33, 37].Text = dt.Rows[0]["BUN4_13_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[34, 33].Text = dt.Rows[0]["BUN4_14"].ToString().Trim();
                ss1_Sheet1.Cells[34, 37].Text = dt.Rows[0]["BUN4_14_MEMO"].ToString().Trim();

                ss1_Sheet1.Cells[38, 34].Text = dt.Rows[0]["LSDATE"].ToString().Trim();//작성일
                ss1_Sheet1.Cells[39, 34].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, dt.Rows[0]["BUN4_14_MEMO"].ToString().Trim());

                FstrDrCode = dt.Rows[0]["DrCode"].ToString().Trim();

                strLSDATE = dt.Rows[0]["LSDATE"].ToString().Trim();
                strSIGNSABUN = dt.Rows[0]["SIGNSABUN"].ToString().Trim();


                dt.Dispose();
                dt = null;

                if (FbDOCTOR == false)
                {

                    ss1_Sheet1.Cells[38, 34].Text = VB.Left(strLSDATE, 4) + "년 " + VB.Mid(strLSDATE, 6, 2) + "월 " + VB.Right(strLSDATE, 2) + "일";
                    ss1_Sheet1.Cells[40, 34].Text = clsVbfunc.GetInSaName(clsDB.DbCon, strSIGNSABUN);
                    SetSign(ss1, 41, 41, ComFunc.LPAD(strSIGNSABUN, 5, "0"));
                    return;
                }
                if (argPRT == "Y" || strSIGNSABUN != "")
                {
                    SQL = "";
                    SQL = " SELECT SABUN, DEPTCODE, DRNAME FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                    SQL = SQL + ComNum.VBLF + " WHERE DOCCODE = " + strSIGNSABUN;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[40, 34].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;

                    ss1_Sheet1.Cells[38, 34].Text = VB.Left(strLSDATE, 4) + "년 " + VB.Mid(strLSDATE, 6, 2) + "월 " + VB.Right(strLSDATE, 2) + "일";
                    SetSign(ss1, 41, 41, ComFunc.LPAD(strSIGNSABUN, 5, "0"));
                }
                else
                {
                    if (VB.UCase(strEXEName) == "EMRPRT")
                    {
                        SQL = "";
                        SQL = " SELECT DRNAME,DocCode FROM KOSMOS_OCS.OCS_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + " WHERE DrCode = '" + FstrDrCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            ss1_Sheet1.Cells[40, 34].Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                        ss1_Sheet1.Cells[38, 34].Text = VB.Left(txtDATE.Text, 4) + "년 " + VB.Mid(txtDATE.Text, 6, 2) + "월 " + VB.Right(txtDATE.Text, 2) + "일";
                        SetSign(ss1, 41, 41, ComFunc.LPAD(strSIGNSABUN, 5, "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        //TODO 
        private void SetSign(FarPoint.Win.Spread.FpSpread ss, int argROW, int ArgCol, string ArgSabun)
        {
            //TODO 안에 함수 마저 해야함
            string strFile = "";

            if (ArgSabun == "")
            {
                MessageBox.Show("저장된 서명이미지가 없습니다. 의무기록실에 연락을 하여 이미지서명을 등록하시거나 인쇄된 증명서에 직접 의사 서명을 하시기 바랍니다.");
                return;
            }

            ImageCellType imgCell = new ImageCellType();
            //imgCell.Style = 


            ss1_Sheet1.Cells[argROW - 1, ArgCol - 1].CellType = imgCell;

            ss1_Sheet1.Cells[argROW - 1, ArgCol - 1].Value = null;

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.InitialDirectory = @"C:\CMC\" + ComFunc.LPAD(ArgSabun, 5, "0") + ".jpg";

            //TODO 디비에 이미지로 저장된 데이터 값을 불러와야함
            //If SIGNATUREFILE_DBToFile(strFile, ArgSabun) = True Then


            //'해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업


            //If Dir(strFile, vbNormal) <> "" Then

            //ss.TypePictMaintainScale = True

            //ss.TypePictStretch = True

            //ss.TypePictPicture = LoadPicture(strFile)

            //End If



            //Dim fs

            //Set fs = CreateObject("Scripting.FileSystemObject")

            //fs.DeleteFile strFile

            //End If
        }


    }
}
