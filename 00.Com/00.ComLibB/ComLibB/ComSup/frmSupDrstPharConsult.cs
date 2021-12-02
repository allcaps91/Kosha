using ComBase;
using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstPharConsult.cs
    /// Description     : 복약상담 의뢰 및 결과확인
    /// Author          : 이정현
    /// Create Date     : 2017-12-18
    /// <history> 
    /// 복약상담 의뢰 및 결과확인
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\FrmPharConsult.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstPharConsult : Form
    {
        private string GstrGubun   = "NEW";
        private string GstrCode    = string.Empty;
        private string GstrName    = string.Empty;
        private string GstrTreatno = string.Empty;

        public frmSupDrstPharConsult()
        {
            InitializeComponent();
        }

        public frmSupDrstPharConsult(string strCode, string strName)
        {
            InitializeComponent();

            GstrCode = strCode;
            GstrName = strName;
        }


        public frmSupDrstPharConsult(string strCode, string strName, string strTreatno = "")
        {
            InitializeComponent();

            GstrCode    = strCode;
            GstrName    = strName;
            GstrTreatno = strTreatno;
        }

        private void frmSupDrstPharConsult_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            frmLoad();
        }

        private void frmLoad()
        {
            if (GstrGubun == "OLD")
            {
                panDrugOld.Visible = true;                
                panDrugOld.Height = 322;
                panDrugNew.Visible = false;
                panDrugNew.Height = 172;
                panSayuTitle.Visible = false;
                panSayuValue.Visible = false;          
            }
            else
            {
                panDrugOld.Visible = false;
                panDrugOld.Height = 322;
                panDrugNew.Visible = true;
                panDrugNew.Height = 172;
                panSayuTitle.Visible = true;
                panSayuValue.Visible = true;
            }

            initCtrl();
            initPatient();
            readCode();
            setWard();
            setDept();
            GetData();

            if (GstrCode != "")
            {
                string SQL = "";
                DataTable dt = null;
                string SqlErr = "";

                string strPano = GstrCode;
                string strIPDNO = GstrName;
                string strSEQNO = "";

                try
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     SEQNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_PCONSULT ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                    if (GstrTreatno.NotEmpty())
                    {
                        SQL = SQL + ComNum.VBLF + "  AND TREATNO = " + GstrTreatno;
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO = " + strIPDNO;
                    }
                    SQL = SQL + ComNum.VBLF + "  AND WRITESABUN = " + clsType.User.Sabun;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strSEQNO = dt.Rows[0]["SEQNO"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if (strSEQNO != "")
                    {
                        setPatient(strPano, strIPDNO, GstrTreatno, strSEQNO);
                    }
                    else
                    {
                        setPatient(strPano, strIPDNO, GstrTreatno);
                    }
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
                }
            }
        }

        private void initCtrl()
        {
            panButton.Enabled = true;
            panLock.Enabled = true;
            txtCode0.Text = "";
            txtCode1.Text = "";
            txtCode2.Text = "";
            lblCodeNAme0.Text = "";
            lblCodeNAme1.Text = "";
            lblCodeNAme2.Text = "";
            ssDrugNew_Sheet1.RowCount = 0;
            rdoUsed1.Checked = false;
            rdoUsed2.Checked = false;
            optSayu1.Checked = false;
            optSayu2.Checked = false;
            optSayu3.Checked = false;
            optSayu4.Checked = false;
            txtSayu.Text = "";
            txtBigo.Text = "";
            txtReturn.Text = "";
        }

        private void initPatient()
        {
            ssPatient_Sheet1.Cells[0, 0, 0, ssPatient_Sheet1.ColumnCount - 1].Text = "";
        }

        private void readCode()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssDrug_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PART, A.COMMENTS AS GUBUN, B.JEPCODE, B.COMMENTS";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE = '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY PART) A,";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM " + ComNum.DB_ERP + "DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE <> '분류명칭'";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY JEPCODE) B";
                SQL = SQL + ComNum.VBLF + "WHERE A.PART = B.PART";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssDrug_Sheet1.RowCount = dt.Rows.Count;
                    ssDrug_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDrug_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PART"].ToString().Trim() + "." + dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssDrug_Sheet1.Cells[i, 2].Text = dt.Rows[i]["COMMENTS"].ToString().Trim();
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void setWard()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboWard.Text = "";
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WardCode, WardName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "     WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode ";

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
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("RA");
                cboWard.Items.Add("TTE"); //심장초음파
                cboWard.Items.Add("OPD"); //외래 추가 2021-06-30

                cboWard.SelectedIndex = 0;
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
            }
        }

        private void setDept()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept.Text = "";
            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
                SQL = SQL + ComNum.VBLF + "     WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN') ";  //2005-08-09 ER제외
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

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
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;
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
            }
        }

        private void setPatient(string strPTNO, string strIPDNO, string strTreatNo, string strSeqno = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') AS INDATE, A.DRCODE, NVL(A.IPDNO, TREATNO) AS IPDNO, B.ROWID AS ROWID_2, B.SEQNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ";
                SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN " + ComNum.DB_ERP + "DRUG_PCONSULT B";
                SQL = SQL + ComNum.VBLF + "   ON A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  AND A.PANO  = B.PANO   ";
                if (strSeqno != "")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.SEQNO = " + strSeqno;
                    SQL = SQL + ComNum.VBLF + "  AND B.WRITESABUN = " + clsType.User.Sabun;
                }

                SQL = SQL + ComNum.VBLF + "WHERE A.PANO  = '" + strPTNO + "' ";
                if (GstrTreatno.NotEmpty())
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.TREATNO = " + GstrTreatno;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = " + strIPDNO;
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY IPDNO DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPatient_Sheet1.Cells[0, 0].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 1].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 3].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 4].Text = dt.Rows[0]["INDATE"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 5].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    ssPatient_Sheet1.Cells[0, 8].Text = dt.Rows[0]["ROWID_2"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 9].Text = dt.Rows[0]["IPDNO"].ToString().Trim();
                    ssPatient_Sheet1.Cells[0, 10].Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSeqno == "") { return; }

                readProgress(strPTNO, GstrTreatno.NotEmpty() ? GstrTreatno : strIPDNO, strSeqno);
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
            }
        }

        private void readProgress(string strPTNO, string strIPDNO, string strSeqno)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            btnDetail.Enabled = false;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PROGRESS, A.WRITESABUN, A.ROWID AS ROWID2, A.BIGO, A.USED, A.ORDERCODE, A.RETURN_TEXT, C.MEMO, C.WRITESABUN AS RSABUN, C.WRITEDATE AS RDATE, A.ORDERCODE2, A.ORDERCODE3, A.CONSULTSAYU, A.CONSULTSAYU_ETC  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_PCONSULT A, " + ComNum.DB_ERP + "DRUG_PCONSULT_RETURN C";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = " + strIPDNO;
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = " + strSeqno;
                SQL = SQL + ComNum.VBLF + "         AND A.SEQNO = C.SEQNO(+)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPatient_Sheet1.Cells[0, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["WRITESABUN"].ToString().Trim());
                    ssPatient_Sheet1.Cells[0, 7].Text = codeProgress(dt.Rows[0]["PROGRESS"].ToString().Trim());
                    ssPatient_Sheet1.Cells[0, 8].Text = dt.Rows[0]["ROWID2"].ToString().Trim();

                    if (dt.Rows[0]["PROGRESS"].ToString().Trim() == "C")
                    {
                        btnDetail.Enabled = true;
                        panLock.Enabled = false;
                        panButton.Enabled = false;

                        txtReturn.Text = dt.Rows[0]["RETURN_TEXT"].ToString().Trim();

                        if (txtReturn.Text.Trim() == "")
                        {
                            txtReturn.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                        }

                        txtReturn.Text = txtReturn.Text + ComNum.VBLF + ComNum.VBLF + " - 작성자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["RSABUN"].ToString().Trim());
                    }

                    if (GstrGubun == "OLD")
                    {
                        txtCode0.Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                        lblCodeNAme0.Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim());

                        txtCode1.Text = dt.Rows[0]["ORDERCODE2"].ToString().Trim();
                        lblCodeNAme1.Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE2"].ToString().Trim());

                        txtCode2.Text = dt.Rows[0]["ORDERCODE3"].ToString().Trim();
                        lblCodeNAme2.Text = clsVbfunc.READ_SugaName(clsDB.DbCon, dt.Rows[0]["ORDERCODE3"].ToString().Trim());
                    }
                    else
                    {
                        List<string> strDx = new List<string>();

                        strDx.Add(dt.Rows[0]["ORDERCODE"].ToString().Trim());
                        strDx.Add(dt.Rows[0]["ORDERCODE2"].ToString().Trim());
                        strDx.Add(dt.Rows[0]["ORDERCODE3"].ToString().Trim());

                        SET_DRUGNEW(strDx);


                        switch (dt.Rows[0]["CONSULTSAYU"].ToString().Trim())
                        {
                            case "0":
                                optSayu1.Checked = true;
                                break;
                            case "1":
                                optSayu2.Checked = true;
                                break;
                            case "2":
                                optSayu3.Checked = true;
                                break;
                            case "3":
                                optSayu4.Checked = true;
                                txtSayu.Text = dt.Rows[0]["CONSULTSAYU_ETC"].ToString().Trim();
                                break;
                        }
                    }

                    switch (dt.Rows[0]["USED"].ToString().Trim())
                    {
                        case "0": rdoUsed1.Checked = true; break;
                        case "1": rdoUsed2.Checked = true; break;
                    }

                    txtBigo.Text = dt.Rows[0]["BIGO"].ToString().Trim();

                    READ_RETURN(strSeqno);
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();            
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            initCtrl();
            initPatient();

            ssList_Sheet1.RowCount = 0;
            
            try
            {
                SQL = "";

                if (cboWard.Text.Trim().Equals("OPD"))
                {
                    #region 외래 2021-06-30 추가
                    SQL = "SELECT                                                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "     '' AS ROOMCODE, A.PATID AS PANO, (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE TRIM(PANO) = A.PATID) AS SNAME, A.CLINCODE AS DEPTCODE, SUBSTR(INDATE, 0, 4) || '-' || SUBSTR(INDATE, 5, 2) || '-' ||  SUBSTR(INDATE, 7, 2) AS INDATE, '' AS WARDCODE,                   ";
                    SQL = SQL + ComNum.VBLF + "     0 AS IPDNO, A.TREATNO, B.ROWID, B.SEQNO, (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE TRIM(DOCCODE) = A.DOCCODE) AS DRNAME                               ";
                    SQL = SQL + ComNum.VBLF + "  ,  CASE WHEN B.PROGRESS = '1' THEN  '○'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B.PROGRESS = '2' THEN  '◎'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B.PROGRESS = 'C' THEN  '●'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "      END PROGRESS                                                                                               ";

                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_TREATT A, " + ComNum.DB_ERP + "DRUG_PCONSULT B                               ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";

                    if (cboDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.CLINCODE = '" + cboDept.Text.Trim() + "'                                                                 ";
                    }

                    if (rdoGbn1.Checked == true)
                    {
                        //SQL = SQL + ComNum.VBLF + "  AND A.PATID         = TRIM(B.PANO(+))                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "  AND A.TREATNO       = B.TREATNO(+)                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "  AND B.WRITESABUN(+) = " + clsType.User.Sabun;
                        SQL = SQL + ComNum.VBLF + "  AND B.TREATNO IS NOT NULL          ";   
                    }
                    else if (rdoGbn2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.PATID         = TRIM(B.PANO)                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "  AND A.TREATNO       = B.TREATNO                                                                                       ";
                        SQL = SQL + ComNum.VBLF + "  AND B.WRITESABUN    = " + clsType.User.Sabun;
                    }
                    #endregion

                }
                else
                {
                    #region 입원
                    SQL = "SELECT                                                                                                                               ";
                    SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.INDATE, 'YYYY-MM-DD') AS INDATE, WARDCODE,                ";
                    SQL = SQL + ComNum.VBLF + "     A.DRCODE, A.IPDNO, B.TREATNO, B.ROWID, B.SEQNO, FC_BAS_DOCTOR_DRNAME(A.DRCODE) AS DRNAME                               ";
                    SQL = SQL + ComNum.VBLF + "  ,  CASE WHEN B.PROGRESS = '1' THEN  '○'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B.PROGRESS = '2' THEN  '◎'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "          WHEN B.PROGRESS = 'C' THEN  '●'                                                                        ";
                    SQL = SQL + ComNum.VBLF + "      END PROGRESS                                                                                               ";

                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_ERP + "DRUG_PCONSULT B                               ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.OUTDATE IS NULL ";

                    if (cboWard.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.WARDCODE = '" + cboWard.Text.Trim() + "'                                                             ";
                    }

                    if (cboDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = '" + cboDept.Text.Trim() + "'                                                                 ";
                    }

                    if (rdoGbn1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PANO(+)                                                                                     ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO(+)                                                                                   ";
                        SQL = SQL + ComNum.VBLF + "  AND B.WRITESABUN(+) = " + clsType.User.Sabun;
                    }
                    else if (rdoGbn2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PANO                                                                                        ";
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO                                                                                      ";
                        SQL = SQL + ComNum.VBLF + "  AND B.WRITESABUN = " + clsType.User.Sabun;
                    }
                    #endregion
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ASC, ROOMCODE ASC                                                                          ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PROGRESS"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Tag  = dt.Rows[i]["TREATNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string codeProgress(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "1": rtnVal = ""; break;
                case "2": rtnVal = "진행중"; break;
                case "3": rtnVal = "취소"; break;
                case "C": rtnVal = "완료"; break;
            }

            return rtnVal;
        }

        private string codeProgressToText(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "1": rtnVal = "○"; break;
                case "2": rtnVal = "◎"; break;
                case "C": rtnVal = "●"; break;
            }

            return rtnVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (GstrGubun == "OLD")
            {
                if (SaveData() == true)
                {
                    GetData();
                }
            }
            else
            {
                if (SaveDataNew() == true)
                {
                }
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strIPDNO = "";
            string strPano = "";
            string strORDERCODE = "";
            string strORDERCODE2 = "";
            string strORDERCODE3 = "";
            string strUSED = "";
            string strBigo = "";
            string strPROGRESS = "";
            string strROWID = "";

            double dblSeqno = 0;

            strPano = ssPatient_Sheet1.Cells[0, 1].Text.Trim();
            strIPDNO = ssPatient_Sheet1.Cells[0, 9].Text.Trim();
            strROWID = ssPatient_Sheet1.Cells[0, 8].Text.Trim();
            strPROGRESS = ssPatient_Sheet1.Cells[0, 6].Text.Trim();

            strORDERCODE = txtCode0.Text.Trim();
            strORDERCODE2 = txtCode1.Text.Trim();
            strORDERCODE3 = txtCode2.Text.Trim();
            if (rdoUsed1.Checked == true) { strUSED = "0"; }
            if (rdoUsed2.Checked == true) { strUSED = "1"; }

            strBigo = txtBigo.Text.Trim();

            if (strIPDNO == "")
            {
                ComFunc.MsgBox("자료가 부족합니다.");
                return rtnVal;
            }

            if (strUSED == "") { return rtnVal; }

            if (strPROGRESS == "" || strPROGRESS == "진행중") { }
            else
            {
                ComFunc.MsgBox("약제팀에서 접수 또는 회신이 완료된 의뢰입니다. 수정은 불가능합니다.");
                return rtnVal;
            }

            if (strORDERCODE == "")
            {
                ComFunc.MsgBox("약품을 선택하시기 바랍니다.");
                return rtnVal;
            }
            
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT_HIS                                                       ";
                    SQL = SQL + ComNum.VBLF + "     SELECT                                                                          "; 
                    SQL = SQL + ComNum.VBLF + "         SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, WRITEDATE, WRITESABUN, ";
                    SQL = SQL + ComNum.VBLF + "         ORDERCODE2, ORDERCODE3, CONSULTSAYU, CONSULTSAYU_ETC                        ";
                    SQL = SQL + ComNum.VBLF + "      FROM " + ComNum.DB_ERP + "DRUG_PCONSULT                                        ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "'                                                ";

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
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

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

                dblSeqno = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_PCONSULT");

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, ";
                SQL = SQL + ComNum.VBLF + "     WRITEDATE, WRITESABUN, ORDERCODE2, ORDERCODE3)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + dblSeqno + ", ";
                SQL = SQL + ComNum.VBLF + "         " + strIPDNO + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strUSED + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strBigo + "', ";
                SQL = SQL + ComNum.VBLF + "         '1', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE2 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE3 + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

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

        private bool SaveDataNew()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strIPDNO = "";
            string strPano = "";
            string strORDERCODE = "";
            string strORDERCODE2 = "";
            string strORDERCODE3 = "";
            string strUSED = "";
            string strBigo = "";
            string strPROGRESS = "";
            string strROWID = "";
            string strConsultSayu = "";
            string strConsultSayuText = "";

            double dblSeqno = 0;

            strPano = ssPatient_Sheet1.Cells[0, 1].Text.Trim();
            strIPDNO = ssPatient_Sheet1.Cells[0, 9].Text.Trim();
            strROWID = ssPatient_Sheet1.Cells[0, 8].Text.Trim();
            strPROGRESS = ssPatient_Sheet1.Cells[0, 6].Text.Trim();

            if (ssDrugNew_Sheet1.RowCount >= 1)
            {
                strORDERCODE = ssDrugNew_Sheet1.Cells[0, 1].Text;
            }

            if (ssDrugNew_Sheet1.RowCount >= 2)
            {
                strORDERCODE2 = ssDrugNew_Sheet1.Cells[1, 1].Text;
            }

            if (ssDrugNew_Sheet1.RowCount >= 3)
            {
                strORDERCODE3 = ssDrugNew_Sheet1.Cells[2, 1].Text;
            }

            if (rdoUsed1.Checked == true) { strUSED = "0"; }
            if (rdoUsed2.Checked == true) { strUSED = "1"; }
            
            if (optSayu1.Checked == true) { strConsultSayu = "0"; }
            if (optSayu2.Checked == true) { strConsultSayu = "1"; }
            if (optSayu3.Checked == true) { strConsultSayu = "2"; }
            if (optSayu4.Checked == true)
            {
                strConsultSayu = "3";
                strConsultSayuText = txtSayu.Text;
            }

            strBigo = txtBigo.Text.Trim();

            if (strIPDNO == "" && ssPatient_Sheet1.Cells[0, 3].Text.Trim().Equals("MO") == false)
            {
                ComFunc.MsgBox("자료가 부족합니다.");
                return rtnVal;
            }

            if (strUSED == "") { return rtnVal; }

            if (strPROGRESS == "" || strPROGRESS == "진행중") { }
            else
            {
                ComFunc.MsgBox("약제팀에서 접수 또는 회신이 완료된 의뢰입니다. 수정은 불가능합니다.");
                return rtnVal;
            }

            if (strORDERCODE == "")
            {
                ComFunc.MsgBox("약품을 선택하시기 바랍니다.");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID != "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT_HIS                                                       ";
                    SQL = SQL + ComNum.VBLF + "     SELECT                                                                          ";
                    SQL = SQL + ComNum.VBLF + "         SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, WRITEDATE, WRITESABUN, ";
                    SQL = SQL + ComNum.VBLF + "         ORDERCODE2, ORDERCODE3, CONSULTSAYU, CONSULTSAYU_ETC                        ";
                    SQL = SQL + ComNum.VBLF + "      FROM " + ComNum.DB_ERP + "DRUG_PCONSULT                                        ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "'                                                ";

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
                    SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

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

                dblSeqno = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_ERP.Replace(".", ""), "SEQ_PCONSULT");

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     (SEQNO, IPDNO, PANO, ORDERCODE, USED, BIGO, PROGRESS, ";
                SQL = SQL + ComNum.VBLF + "     WRITEDATE, WRITESABUN, ORDERCODE2, ORDERCODE3, ";
                SQL = SQL + ComNum.VBLF + "     CONSULTSAYU, CONSULTSAYU_ETC)";
                SQL = SQL + ComNum.VBLF + "VALUES";
                SQL = SQL + ComNum.VBLF + "     (";
                SQL = SQL + ComNum.VBLF + "         " + dblSeqno + ", ";
                SQL = SQL + ComNum.VBLF + "         " + (strIPDNO.IsNullOrEmpty() ?  "NULL" : strIPDNO) + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strUSED + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strBigo + "', ";
                SQL = SQL + ComNum.VBLF + "         '1', ";
                SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE2 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strORDERCODE3 + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strConsultSayu + "', ";
                SQL = SQL + ComNum.VBLF + "         '" + strConsultSayuText + "' ";
                SQL = SQL + ComNum.VBLF + "     )";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if (DelData() == true)
            {
                GetData();
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strROWID = "";

            strROWID = ssPatient_Sheet1.Cells[0, 8].Text.Trim();

            if (strROWID == "")
            {
                ComFunc.MsgBox("삭제할 자료를 선택하여 주십시오.");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_PCONSULT_HIS ";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_PCONSULT ";
                SQL = SQL + ComNum.VBLF + "         WHERE ROWID = '" + strROWID + "' ";

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
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_PCONSULT";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID = '" + strROWID + "' ";

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
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            initPatient();
            initCtrl();

            if (e.ColumnHeader == true) { return; }

            string strIPDNO = ssList_Sheet1.Cells[e.Row, 7].Text.Trim();
            string strTREATNO = ssList_Sheet1.Cells[e.Row, 7].Tag.ToString().Trim();
            string strPano = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            string strSEQNO = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();

            if (strIPDNO == "") { return; }

            if (strSEQNO != "")
            {
                setPatient(strPano, strIPDNO, strTREATNO, strSEQNO);
            }
            else
            {
                setPatient(strPano, strIPDNO, strTREATNO);
            }
        }

        private void tabStrip_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabStrip.SelectedIndex)
            {
                case 1:
                case 2:
                    ComFunc.MsgBox("작업 중입니다.");
                    tabStrip.SelectedIndex = 0;
                    break;
            }
        }

        private void ssDrug_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) { return; }

            string strCode = "";
            string strName = "";

            strCode = ssDrug_Sheet1.Cells[e.Row, 1].Text.Trim();
            strName = ssDrug_Sheet1.Cells[e.Row, 2].Text.Trim();

            if (txtCode0.Text.Trim() == "")
            {
                txtCode0.Text = strCode;
                lblCodeNAme0.Text = strName;
            }
            else if (txtCode1.Text.Trim() == "")
            {
                txtCode1.Text = strCode;
                lblCodeNAme1.Text = strName;
            }
            else
            {
                txtCode2.Text = strCode;
                lblCodeNAme2.Text = strName;
            }
        }

        private void btnDelCode0_Click(object sender, EventArgs e)
        {
            txtCode0.Text = "";
            lblCodeNAme0.Text = "";
        }

        private void btnDelCode1_Click(object sender, EventArgs e)
        {
            txtCode1.Text = "";
            lblCodeNAme1.Text = "";
        }

        private void btnDelCode2_Click(object sender, EventArgs e)
        {
            txtCode2.Text = "";
            lblCodeNAme2.Text = "";
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            string strSEQNO = ssPatient_Sheet1.Cells[0, 10].Text.Trim();

            if (strSEQNO != "")
            {
                using (frmComSupPharConsultReturnDetail frm = new frmComSupPharConsultReturnDetail(strSEQNO))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog();
                }
            }
        }

        private void btnSearchDrug_Click(object sender, EventArgs e)
        {
            using (frmSupDrstPharConsultSearch frm = new frmSupDrstPharConsultSearch())
            {
                frm.rGetInfo += frm_rGetInfo;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
        }

        private void frm_rGetInfo(List<string> strDx)
        {
            SET_DRUGNEW(strDx);
        }

        private void SET_DRUGNEW(List<string> strDx)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ssDrugNew_Sheet1.RowCount = 0;

            for (int i = 0; i < strDx.Count; i++)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                       ";
                SQL = SQL + ComNum.VBLF + "     A.COMMENTS PART, B.JEPCODE, C.SNAME, B.COMMENTS  ";
                SQL = SQL + ComNum.VBLF + " FROM                                        ";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM KOSMOS_ADM.DRUG_SETCODE  ";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'                  ";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE = '분류명칭'        ";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL             ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY PART) A,                       ";
                SQL = SQL + ComNum.VBLF + "     (SELECT * FROM KOSMOS_ADM.DRUG_SETCODE  ";
                SQL = SQL + ComNum.VBLF + "         WHERE GUBUN = '11'                  ";
                SQL = SQL + ComNum.VBLF + "             AND JEPCODE <> '분류명칭'       ";
                SQL = SQL + ComNum.VBLF + "             AND DELDATE IS NULL             ";
                SQL = SQL + ComNum.VBLF + "     ORDER BY JEPCODE) B,                    ";
                SQL = SQL + ComNum.VBLF + "     KOSMOS_OCS.OCS_DRUGINFO C               ";
                SQL = SQL + ComNum.VBLF + " WHERE A.PART = B.PART                       ";
                SQL = SQL + ComNum.VBLF + "   AND B.JEPCODE = C.SUNEXT                  ";
                SQL = SQL + ComNum.VBLF + "   AND B.JEPCODE = '" + strDx[i] + "'        ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PART, JEPCODE                      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDrugNew_Sheet1.RowCount = ssDrugNew_Sheet1.RowCount + 1;
                    ssDrugNew_Sheet1.Cells[i, 0].Text = dt.Rows[0]["PART"].ToString().Trim();
                    ssDrugNew_Sheet1.Cells[i, 1].Text = dt.Rows[0]["JEPCODE"].ToString().Trim();
                    ssDrugNew_Sheet1.Cells[i, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssDrugNew_Sheet1.Cells[i, 3].Text = dt.Rows[0]["COMMENTS"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
        }

        private void ssDrugNew_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssDrugNew_Sheet1.RemoveRows(e.Row, 1);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ssList_Sheet1.ActiveRowIndex < 0) return;

            int iRow = ssList_Sheet1.ActiveRowIndex;
            string strIPDNO = ssList_Sheet1.Cells[iRow, 7].Text.Trim();
            string strTREATNO = ssList_Sheet1.Cells[iRow, 7].Tag.ToString().Trim();
            string strPano = ssList_Sheet1.Cells[iRow, 2].Text.Trim();
            string strSEQNO = ssList_Sheet1.Cells[iRow, 8].Text.Trim();

            using (frmSupDrstPharConsultDr frm = new frmSupDrstPharConsultDr(strPano, strIPDNO, strTREATNO))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void READ_RETURN(string strSeqno)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMsg = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                                       ";
                SQL = SQL + ComNum.VBLF + "    SEQNO,                                                   ";
                SQL = SQL + ComNum.VBLF + "    PROGRESS,                                                ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M1   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M1.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000031') AS M1,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M2   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M2.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000032') AS M2,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M3   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M3.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000033') AS M3,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M4   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M4.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000034') AS M4,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M5   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M5.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000035') AS M5,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M6   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M6.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000036') AS M6,                     ";
                SQL = SQL + ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_ADM.DRUG_PCONSULT_ROW M7   ";
                SQL = SQL + ComNum.VBLF + "      WHERE M7.SEQNO = A.SEQNO                               ";
                SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I00000060') AS M7                      ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_PCONSULT A                             ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SEQNO = " + strSeqno + "                             ";
                SQL = SQL + ComNum.VBLF + "  AND A.PROGRESS IN ('2', 'C')                               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strMsg += "#회신일 : " + dt.Rows[0]["M1"].ToString().Trim() + VB.Space(5);
                    strMsg += "#상담일 : " + dt.Rows[0]["M2"].ToString().Trim() + VB.Space(5);
                    strMsg += "#담당약사 : " + dt.Rows[0]["M3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                    strMsg += "#상담대상 : ";
                    strMsg += (dt.Rows[0]["M4"].ToString().Trim() == "1" ? "■" : "□") + " 환자" + VB.Space(5);
                    strMsg += (dt.Rows[0]["M5"].ToString().Trim() == "1" ? "■" : "□") + " 보호자" + VB.Space(5);
                    strMsg += (dt.Rows[0]["M6"].ToString().Trim() == "1" ? "■" : "□") + " 환자와 보호자" + ComNum.VBLF + ComNum.VBLF;

                    strMsg += "#약사 COMMENT : " + ComNum.VBLF;
                    strMsg += dt.Rows[0]["M7"].ToString().Trim();

                    txtReturn.Text = strMsg;
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
