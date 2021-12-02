using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using System.Text;

namespace ComLibB
{
    public partial class frmOcsCpTong : Form, MainFormMessage
    {
        /// <summary>
        /// EMR 뷰어
        /// </summary>
        frmEmrViewer frmEmrViewerX = null;

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmOcsCpTong(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmOcsCpTong()
        {
            InitializeComponent();
        }

        private void frmOcsCpTong_Load(object sender, EventArgs e)
        {
            Set_Combo();
            GetDataCpCode();
        }

        /// <summary>
        /// 콤보박스 설정
        /// </summary>
        void Set_Combo()
        {
            cboSex.Items.Clear();
            cboSex.Items.Add("남자");
            cboSex.Items.Add("여자");

            cboDropEnable.Items.Clear();
            cboDropEnable.Items.Add("00.제외안함");
            cboDropEnable.Items.Add("01.제외");

            cboCancerEnable.Items.Clear();
            cboCancerEnable.Items.Add("00.중단안함");
            cboCancerEnable.Items.Add("01.중단");

            cboLeaveState.Items.Clear();
            cboLeaveState.Items.Add("1.퇴원지시후");
            cboLeaveState.Items.Add("2.자의퇴원");
            cboLeaveState.Items.Add("3.전송");
            cboLeaveState.Items.Add("4.탈원");
            cboLeaveState.Items.Add("5.사망");
            cboLeaveState.Items.Add("6.강제퇴원");

            cboNOTAPPLYEnable.Items.Clear();
            cboNOTAPPLYEnable.Items.Add("00.적용");
            cboNOTAPPLYEnable.Items.Add("01.미적용");

            cboDept.Items.Clear();
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT DeptCode FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL += ComNum.VBLF + "ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// CP기초코드 정보 가져오기
        /// </summary>
        private void GetDataCpCode()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboCP1.Items.Clear();
            cboCP2.Items.Clear();
            cboCP3.Items.Clear();
            cboCancer.Items.Clear();

            cboCP1.Items.Add("**.전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " GRPCD, BASCD, BASNAME, BASNAME1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "  AND GRPCD IN('CP코드관리', 'CP중단사유', 'CP제외기준')";
                SQL = SQL + ComNum.VBLF + "ORDER BY BASCD";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
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
                        switch(dt.Rows[i]["GRPCD"].ToString().Trim())
                        {
                            case "CP코드관리":
                                if(dt.Rows[i]["BASNAME1"].ToString().Trim() == "IPD") // 입원 CP만
                                {
                                    cboCP1.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(50) + "." + dt.Rows[i]["BASCD"].ToString().Trim());
                                    cboCP2.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(50) + "." + dt.Rows[i]["BASCD"].ToString().Trim());
                                    cboCP3.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(50) + "." + dt.Rows[i]["BASCD"].ToString().Trim());
                                }
                                break;
                            case "CP중단사유":
                                cboCancer.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(80) + "." + dt.Rows[i]["BASCD"].ToString().Trim());
                                break;
                            case "CP제외기준":
                                cboDrop.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(80) + "." + dt.Rows[i]["BASCD"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                cboCP1.SelectedIndex = 0;
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

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(txtPtNo.Text.Length == 0 || e.KeyCode != Keys.Enter)
            {
                return;
            }

            txtPtNo.Text = VB.Val(txtPtNo.Text.Trim()).ToString("00000000");
            lblName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPtNo.Text.Trim());
        }

        private void txtPtNo_Leave(object sender, EventArgs e)
        {
            if(txtPtNo.Text.Length == 0)
            {
                return;
            }

            txtPtNo.Text = VB.Val(txtPtNo.Text.Trim()).ToString("00000000");
            lblName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPtNo.Text.Trim());
        }

        void Control_Clear()
        {
            for (int i = 0; i < panContent.Controls.Count; i++)
            {
                if (panContent.Controls[i] is ComboBox)
                {
                    ((ComboBox)panContent.Controls[i]).SelectedIndex = -1;
                }
                else if (panContent.Controls[i] is TextBox)
                {
                    panContent.Controls[i].Text = string.Empty;
                }
                else if (panContent.Controls[i] is DateTimePicker)
                {
                    ((DateTimePicker)panContent.Controls[i]).Value = DateTime.Now;
                    ((DateTimePicker)panContent.Controls[i]).Checked = panContent.Controls[i] != dtpOpDate;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if(cboCP1.SelectedIndex == 0)
            {
                ComFunc.MsgBox("CP를 선택해주세요.");
                cboCP1.Focus();
                return;
            }

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string strIllCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "04")); // 진단 적용코드
            string strSuCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "05")); // 수술 적용코드

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (rbtnGubun1.Checked == true)
                {
                    SQL = "SELECT";
                    SQL += ComNum.VBLF + "A.IPDNO, C.ACTDATE, TO_CHAR(B.BIRTH, 'YYYY-MM-DD') BIRTH,  E.DRNAME,";
                    SQL += ComNum.VBLF + "F.BASNAME AS CPNAME, ";
                    SQL += ComNum.VBLF + "C.PANO, C.SNAME, C.SEX, C.DEPTCODE, C.ILLCODE, C.SUCODE, C.IPDNO CIPDNO, ";
                    SQL += ComNum.VBLF + "D.IPDNO IPDNO2, D.DROPGB, D.CANCERGB";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_IPD_LIST C";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                    SQL += ComNum.VBLF + "     ON A.IPDNO = C.IPDNO ";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT B";
                    SQL += ComNum.VBLF + "     ON B.PANO = C.PANO ";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_DOCTOR E";
                    SQL += ComNum.VBLF + "     ON E.DRCODE = C.DRCODE";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BASCD F";
                    SQL += ComNum.VBLF + "     ON F.BASCD = C.CPCODE";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_RECORD D";
                    SQL += ComNum.VBLF + "     ON D.IPDNO = C.IPDNO ";
                    SQL += ComNum.VBLF + " WHERE C.ACTDATE BETWEEN TO_DATE('" + dtpFrDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') AND TO_DATE('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                    SQL += (cboCP1.SelectedIndex == 0 ? "" : ComNum.VBLF + "   AND C.CPCODE = '" + cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1) + "'");

                    if (chkSearch.Checked == true)
                    {
                        SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "   AND C.PANO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    }
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "P.IPDNO, C.ACTDATE, TO_CHAR(B.BIRTH, 'YYYY-MM-DD') BIRTH, E.DRNAME,";
                    SQL += ComNum.VBLF + "F.BASNAME AS CPNAME, ";
                    SQL += ComNum.VBLF + "C.PANO, C.SNAME, C.SEX, C.DEPTCODE, '', '', C.IPDNO CIPDNO, ";
                    SQL += ComNum.VBLF + "A.IPDNO IPDNO2, A.DROPGB, A.CANCERGB";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_RECORD A";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT B";
                    SQL += ComNum.VBLF + "     ON B.PANO = A.PTNO ";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER C";
                    SQL += ComNum.VBLF + "     ON C.IPDNO = A.IPDNO ";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_DOCTOR E";
                    SQL += ComNum.VBLF + "     ON E.DRCODE = C.DRCODE";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BASCD F";
                    SQL += ComNum.VBLF + "     ON F.BASCD = A.CPCODE";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_RECORD_P P";
                    SQL += ComNum.VBLF + "     ON P.IPDNO = A.IPDNO";
                    SQL += ComNum.VBLF + " WHERE C.ACTDATE BETWEEN TO_DATE('" + dtpFrDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') AND TO_DATE('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND NOT EXISTS";
                    SQL += ComNum.VBLF + "   (";
                    SQL += ComNum.VBLF + "      SELECT";
                    SQL += ComNum.VBLF + "      IPDNO";
                    SQL += ComNum.VBLF + "      FROM " + ComNum.DB_MED + "OCS_CP_IPD_LIST";
                    SQL += ComNum.VBLF + "      WHERE ACTDATE BETWEEN TO_DATE('" + dtpFrDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') AND TO_DATE('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND IPDNO = A.IPDNO";
                    SQL += ComNum.VBLF + "      UNION ALL";
                    SQL += ComNum.VBLF + "      SELECT";
                    SQL += ComNum.VBLF + "      IPDNO";
                    SQL += ComNum.VBLF + "      FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                    SQL += ComNum.VBLF + "      WHERE OUTDATE BETWEEN TO_DATE('" + dtpFrDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') AND TO_DATE('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND IPDNO = A.IPDNO";
                    SQL += (cboCP1.SelectedIndex == 0 ? "" : ComNum.VBLF + "       AND CPCODE = '" + cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1) + "'");
                    SQL += ComNum.VBLF + "   )";

                    SQL += (cboCP1.SelectedIndex == 0 ? "" : ComNum.VBLF + "   AND A.CPCODE = '" + cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1) + "'");

                    if (chkSearch.Checked == true)
                    {
                        SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "   AND A.PTNO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    }

                    SQL += ComNum.VBLF + " ORDER BY ACTDATE";
                }
                else
                {
                    SQL = "  SELECT IPDNO,         ACTDATE,         BIRTH,         DRNAME,        PANO,  ";
                    SQL += ComNum.VBLF + " ( SELECT BASNAME FROM KOSMOS_PMPA.BAS_BASCD ";
                    SQL += ComNum.VBLF + "      WHERE GRPCDB = 'CP관리' ";
                    SQL += ComNum.VBLF + "        AND GRPCD = 'CP코드관리' ";
                    SQL += ComNum.VBLF + "        AND BASCD = CPCODE) CPNAME, ";
                    SQL += ComNum.VBLF + "          SNAME,         SEX,         DEPTCODE,         ILLCODE,         SUCODE,         CIPDNO, IPDNO2,DROPGB,CANCERGB, CPCODE ";
                    SQL += ComNum.VBLF + "    FROM (SELECT C.IPDNO, ";
                    SQL += ComNum.VBLF + "                 C.ACTDATE, ";
                    SQL += ComNum.VBLF + "                 TO_CHAR (B.BIRTH, 'YYYY-MM-DD') BIRTH, ";
                    SQL += ComNum.VBLF + "                 E.DRNAME, ";
                    SQL += ComNum.VBLF + "                 C.PANO, ";
                    SQL += ComNum.VBLF + "                 C.SNAME, ";
                    SQL += ComNum.VBLF + "                 C.SEX, ";
                    SQL += ComNum.VBLF + "                 C.DEPTCODE, ";
                    SQL += ComNum.VBLF + "                 '' ILLCODE, ";
                    SQL += ComNum.VBLF + "                 '' SUCODE, ";
                    SQL += ComNum.VBLF + "                 C.IPDNO CIPDNO, ";
                    SQL += ComNum.VBLF + "                 A.IPDNO IPDNO2, ";
                    SQL += ComNum.VBLF + "                 A.DROPGB, ";
                    SQL += ComNum.VBLF + "                 A.CANCERGB, ";
                    SQL += ComNum.VBLF + "                 A.CPCODE ";
                    SQL += ComNum.VBLF + "            FROM KOSMOS_PMPA.IPD_NEW_MASTER C,            KOSMOS_OCS.OCS_IILLS Q,                 KOSMOS_PMPA.IPD_NEW_SLIP S, ";
                    SQL += ComNum.VBLF + "                 KOSMOS_OCS.OCS_DOCTOR E,                 KOSMOS_PMPA.BAS_PATIENT B,              KOSMOS_OCS.OCS_CP_RECORD A ";
                    SQL += ComNum.VBLF + "           WHERE C.IPDNO = Q.IPDNO(+) ";
                    SQL += ComNum.VBLF + "                 AND C.IPDNO = S.IPDNO(+) ";
                    SQL += ComNum.VBLF + "                 AND C.DRCODE = E.DRCODE ";
                    SQL += ComNum.VBLF + "                 AND C.PANO = B.PANO ";
                    SQL += ComNum.VBLF + "                 AND C.IPDNO = A.IPDNO(+) ";
                    SQL += ComNum.VBLF + "                 AND ( (C.INDATE >= ";
                    SQL += ComNum.VBLF + "                           TO_DATE ('" + dtpFrDate.Value.ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "                        AND C.INDATE <= ";
                    SQL += ComNum.VBLF + "                               TO_DATE ('" + dtpToDate.Value.ToShortDateString() + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                    SQL += ComNum.VBLF + "                      OR (C.OUTDATE >= TO_DATE ('" + dtpFrDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "                          AND C.OUTDATE <= TO_DATE ('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')) ";
                    SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + dtpToDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "                          AND C.OUTDATE IS NULL)) ";
                    if (strIllCode.Trim() != "" && strSuCode.Trim() != "")
                    {
                        SQL += ComNum.VBLF + " AND (" + strIllCode.Replace("AND", "").Replace("B.ILLCODE", "Q.ILLCODE");
                        SQL += ComNum.VBLF + "        OR " + strSuCode.Replace("AND", "").Replace("C.SUCODE", "S.SUCODE") + ")";

                    }
                    else if(strIllCode.Trim() != "")
                    {
                        SQL += ComNum.VBLF + " AND (" + strIllCode.Replace("AND", "").Replace("B.ILLCODE", "Q.ILLCODE") + ")";

                    }
                    else if(strSuCode.Trim() != "")
                    {
                        SQL += ComNum.VBLF + " AND (" + strSuCode.Replace("AND", "").Replace("C.SUCODE", "S.SUCODE") + ")";

                    }


                    if (chkSearch.Checked == true)
                    {
                        SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "   AND A.PTNO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    }
                    SQL += ComNum.VBLF + "  ) GROUP BY IPDNO,         ACTDATE,         BIRTH,         DRNAME,         PANO,           CPCODE, ";
                    SQL += ComNum.VBLF + "             SNAME,         SEX,         DEPTCODE,         ILLCODE,         SUCODE,         CIPDNO, IPDNO2,DROPGB,CANCERGB ";
                    SQL += ComNum.VBLF + " ORDER BY ACTDATE ";

                    #region 사용안함
                    //SQL = " SELECT IPDNO, ACTDATE, BIRTH, DRNAME, CPNAME, PANO, SNAME, SEX, DEPTCODE, ILLCODE, SUCODE, CIPDNO, IPDNO2, DROPGB, CANCERGB, CPCODE ";
                    //SQL += ComNum.VBLF + " FROM ( ";
                    //SQL += ComNum.VBLF + "SELECT";
                    //SQL += ComNum.VBLF + "P.IPDNO, C.ACTDATE, TO_CHAR(B.BIRTH, 'YYYY-MM-DD') BIRTH, E.DRNAME,";
                    //SQL += ComNum.VBLF + "F.BASNAME AS CPNAME, ";
                    //SQL += ComNum.VBLF + "C.PANO, C.SNAME, C.SEX, C.DEPTCODE, '' ILLCODE, '' SUCODE, C.IPDNO CIPDNO, ";
                    //SQL += ComNum.VBLF + "A.IPDNO IPDNO2, A.DROPGB, A.CANCERGB, A.CPCODE";
                    //SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_RECORD A";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT B";
                    //SQL += ComNum.VBLF + "     ON B.PANO = A.PTNO ";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER C";
                    //SQL += ComNum.VBLF + "     ON C.IPDNO = A.IPDNO ";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_DOCTOR E";
                    //SQL += ComNum.VBLF + "     ON E.DRCODE = C.DRCODE";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BASCD F";
                    //SQL += ComNum.VBLF + "     ON F.BASCD = A.CPCODE";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_RECORD_P P";
                    //SQL += ComNum.VBLF + "     ON P.IPDNO = A.IPDNO";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_IILLS Q";
                    //SQL += ComNum.VBLF + "     ON Q.IPDNO = A.IPDNO";
                    //SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "IPD_NEW_SLIP S";
                    //SQL += ComNum.VBLF + "     ON S.IPDNO = A.IPDNO";
                    //SQL += ComNum.VBLF + " WHERE ((C.INDATE >= TO_DATE('" + dtpFrDate.Value.ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI') AND C.INDATE <= TO_DATE('" + dtpToDate.Value.ToShortDateString() + " 23:59','YYYY-MM-DD HH24:MI'))";
                    //SQL += ComNum.VBLF + "   OR (C.OUTDATE >= TO_DATE('" + dtpFrDate.Value.ToShortDateString() + "','YYYY-MM-DD') AND C.OUTDATE <= TO_DATE('" + dtpToDate.Value.ToShortDateString() + "','YYYY-MM-DD'))";
                    //SQL += ComNum.VBLF + "   OR (TRUNC(SYSDATE) <= TO_DATE('" + dtpToDate.Value.ToShortDateString() + "','YYYY-MM-DD') AND C.OUTDATE IS NULL)) ";
                    ////SQL += (cboCP1.SelectedIndex == 0 ? "" : ComNum.VBLF + "   AND A.CPCODE = '" + cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1) + "'");
                    //SQL += ComNum.VBLF + " AND (" + strIllCode.Replace("AND","").Replace("B.ILLCODE","Q.ILLCODE");
                    //SQL += ComNum.VBLF + "        OR " + strSuCode.Replace("AND", "").Replace("C.SUCODE", "S.SUCODE") + ")";


                    //if (chkSearch.Checked == true)
                    //{
                    //    SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "   AND A.PTNO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    //}
                    //SQL += ComNum.VBLF + " ) GROUP BY IPDNO, ACTDATE, BIRTH, DRNAME, CPNAME, PANO, SNAME, SEX, DEPTCODE, ILLCODE, SUCODE, CIPDNO, IPDNO2, DROPGB, CANCERGB, CPCODE ";
                    //SQL += ComNum.VBLF + " ORDER BY ACTDATE"; 
                    #endregion
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CPNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CIPDNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BIRTH"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ILLCODE"].ToString().Trim().Replace(",", ComNum.VBLF).Replace(" ", "");
                    ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim().Replace(",", ComNum.VBLF).Replace(" ", "");

                    if (dt.Rows[i]["IPDNO"].ToString().Trim().Length > 0) // QI실에서 핸들 했을경우
                    {
                        ssList_Sheet1.Cells[i, 12].Text = "검";
                    }
                    else if(dt.Rows[i]["IPDNO2"].ToString().Trim().Length > 0) // 의사 등록
                    {
                        ssList_Sheet1.Cells[i, 12].Text = "등" ;
                    }

                    if (dt.Rows[i]["DROPGB"].ToString().Trim() == "01") // 제외 등록
                    {
                        ssList_Sheet1.Cells[i, 12].Text = "제";
                    }

                    if (dt.Rows[i]["CANCERGB"].ToString().Trim() == "01") // 중단 등록
                    {
                        ssList_Sheet1.Cells[i, 12].Text = "중";
                    }

                    if (rbtnGubun2.Checked == true)
                    {
                        //

                        ssList_Sheet1.Cells[i, 10].Text = ReadCPIllCode(dt.Rows[i]["CIPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "ILLCODE");
                        ssList_Sheet1.Cells[i, 11].Text = ReadCPIllCode(dt.Rows[i]["CIPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "SUCODE");

                        //ssList_Sheet1.Cells[i, 10].Text = ReadCPIllCode(dt.Rows[i]["CIPDNO"].ToString().Trim(), dt.Rows[i]["CPCODE"].ToString().Trim(), "ILLCODE");
                        //ssList_Sheet1.Cells[i, 11].Text = ReadCPIllCode(dt.Rows[i]["CIPDNO"].ToString().Trim(), dt.Rows[i]["CPCODE"].ToString().Trim(), "SUCODE");
                    }
                    else
                    {

                        //중단 제외 상관없이 표시해달라고 함
                        //switch (ssList_Sheet1.Cells[i, 12].Text.Trim())
                        //{
                            //case "제":
                            //case "중":

                                //SQL = "SELECT B.ILLCODE, C.SUCODE, C.BDATE"; //진단코드, 수술코드, 수술날짜
                                //SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IILLS B,";
                                //SQL += ComNum.VBLF + "     " + ComNum.DB_PMPA + "IPD_NEW_SLIP C";
                                //SQL += ComNum.VBLF + "     WHERE B.IPDNO = '" + dt.Rows[i]["IPDNO2"].ToString().Trim() + "'";
                                //SQL += ComNum.VBLF + "       AND C.IPDNO = B.IPDNO";

                                //SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                                //if (SqlErr != "")
                                //{
                                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return;
                                //}

                                //if (dt2.Rows.Count > 0)
                                //{
                                //    ssList_Sheet1.Cells[i, 10].Text = dt2.Rows[0]["ILLCODE"].ToString().Trim();
                                //    ssList_Sheet1.Cells[i, 11].Text = dt2.Rows[0]["SUCODE"].ToString().Trim();
                                //    ssList_Sheet1.Cells[i, 11].Tag = dt2.Rows[0]["BDATE"].ToString().Trim();
                                //}

                                //dt2.Dispose();
                                //dt2 = null;

                                //break;
                        //}
                    }

                    ssList_Sheet1.Rows[i].Height = ssList_Sheet1.Rows[i].GetPreferredHeight() + 5;

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Delete_Data() == true)
            {
                Control_Clear();
                GetSearchData();
            }
            return;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        bool Delete_Data()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인
            if (ssList_Sheet1.RowCount == 0) return false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for(int i = 0; i< ssList_Sheet1.NonEmptyRowCount; i++)
                {
                    if(ssList_Sheet1.Cells[i, 0].Text.Trim() == "True" && ssList_Sheet1.Rows[i].ForeColor == Color.RoyalBlue) //KOSMOS_OCS.OCS_CP_RECORD_P 등록된 환자 일때만
                    {
                        SQL = "DELETE " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                        SQL += ComNum.VBLF + "WHERE PTNO = '" + ssList_Sheet1.Cells[i, 3].Text.Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(Save_Data() == true)
            {
                GetSearchData();
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        bool Save_Data()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            bool bUpdate = false;

            string strCpCode = cboCP2.Text.Substring(cboCP2.Text.LastIndexOf(".") + 1);
            string strDrCode = cboDr.Text.Substring(cboDr.Text.LastIndexOf(".") + 1);

            string strDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            string strIpdNo = txtCPtNo.Tag.ToString();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = "SELECT ";
                SQL += ComNum.VBLF + " PTNO ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                SQL += ComNum.VBLF + "  WHERE IPDNO = '" + strIpdNo + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                bUpdate = dt.Rows.Count > 0;

                dt.Dispose();
                dt = null;

                 if(bUpdate)
                {
                    SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                    SQL += ComNum.VBLF + "SET ";
                    if (cboDropEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "DROPGB     = '01',         ";
                        SQL += ComNum.VBLF + "DROPCD     = '" + cboDrop.SelectedIndex.ToString("00") + "' ,         ";
                        SQL += ComNum.VBLF + "DROPDATE   = '" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "DROPTIME   = '" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "DROPSABUN  = '" + clsType.User.Sabun + "' ,      ";
                        SQL += ComNum.VBLF + "DROPREMARK = '" + txtDropRemark.Text.Trim().Replace("'", "`") + "', ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "DROPGB     = '',";
                        SQL += ComNum.VBLF + "DROPCD     = '',";
                        SQL += ComNum.VBLF + "DROPDATE   = '',";
                        SQL += ComNum.VBLF + "DROPTIME   = '',";
                        SQL += ComNum.VBLF + "DROPSABUN  = '',";
                        SQL += ComNum.VBLF + "DROPREMARK = '',";
                    }

                    if (cboCancerEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "CANCERGB     = '01',         ";
                        SQL += ComNum.VBLF + "CANCERCD     = '" + cboCancer.SelectedIndex.ToString("00") + "' ,         ";
                        SQL += ComNum.VBLF + "CANCERDATE   = '" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "CANCERTIME   = '" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "CANCERSABUN  = '" + clsType.User.Sabun + "' ,      ";
                        SQL += ComNum.VBLF + "CANCERREMARK = '" + txtCancerRemark.Text.Trim().Replace("'", "`") + ",";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "CANCERGB     = '',";
                        SQL += ComNum.VBLF + "CANCERCD     = '',";
                        SQL += ComNum.VBLF + "CANCERDATE   = '',";
                        SQL += ComNum.VBLF + "CANCERTIME   = '',";
                        SQL += ComNum.VBLF + "CANCERSABUN  = '',";
                        SQL += ComNum.VBLF + "CANCERREMARK = '',";
                    }
                    if (cboNOTAPPLYEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "NOTAPPLYGB     = '01',         ";
                        SQL += ComNum.VBLF + "NOTAPPLYCD     = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYDATE   = '" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "NOTAPPLYTIME   = '" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "NOTAPPLYSABUN  = '" + clsType.User.Sabun + "' ,      ";
                        SQL += ComNum.VBLF + "NOTAPPLYREMARK = '" + txtNOTAPPLYRemark.Text.Trim().Replace("'", "`") + ",";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "CANCERGB     = '',";
                        SQL += ComNum.VBLF + "CANCERCD     = '',";
                        SQL += ComNum.VBLF + "CANCERDATE   = '',";
                        SQL += ComNum.VBLF + "CANCERTIME   = '',";
                        SQL += ComNum.VBLF + "CANCERSABUN  = '',";
                        SQL += ComNum.VBLF + "CANCERREMARK = '',";
                    }

                    SQL += ComNum.VBLF + "ILLCODE = '" + txtDiagCode.Text.Trim().Replace("'", "`'") + "', ";
                    SQL += ComNum.VBLF + "OPDATE = '" + (dtpOpDate.Checked ? dtpOpDate.Value.ToString("yyyyMMdd") : "") + "', ";
                    SQL += ComNum.VBLF + "SUCODE = '" + txtOpCode.Text.Trim().Replace("'", "`'") + "', ";
                    SQL += ComNum.VBLF + "LEAVESTATE = '" + cboLeaveState.Text.Trim().Substring(0, 1) + "'";
                    SQL += ComNum.VBLF + "WHERE IPDNO = '" + strIpdNo + "'";
                }
                else
                {
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "CPCODE ,         ";
                    SQL += ComNum.VBLF + "PTNO ,           ";
                    SQL += ComNum.VBLF + "PTNAME ,         ";
                    SQL += ComNum.VBLF + "AGE ,            ";
                    SQL += ComNum.VBLF + "SEX ,            ";
                    SQL += ComNum.VBLF + "DEPTCODE ,       ";
                    SQL += ComNum.VBLF + "GBIO ,           ";
                    SQL += ComNum.VBLF + "BDATE ,          ";
                    SQL += ComNum.VBLF + "INTIME ,         ";
                    SQL += ComNum.VBLF + "OUTDATE,         ";
                    SQL += ComNum.VBLF + "BI ,             ";
                    SQL += ComNum.VBLF + "IPDNO ,          ";
                    SQL += ComNum.VBLF + "STARTDATE ,      ";
                    SQL += ComNum.VBLF + "STARTTIME ,      ";
                    SQL += ComNum.VBLF + "STARTSABUN ,     ";
                    SQL += ComNum.VBLF + "DROPGB ,         ";
                    SQL += ComNum.VBLF + "DROPCD ,         ";
                    SQL += ComNum.VBLF + "DROPDATE ,       ";
                    SQL += ComNum.VBLF + "DROPTIME ,       ";
                    SQL += ComNum.VBLF + "DROPSABUN ,      ";
                    SQL += ComNum.VBLF + "CANCERGB ,       ";
                    SQL += ComNum.VBLF + "CANCERCD ,       ";
                    SQL += ComNum.VBLF + "CANCERDATE ,     ";
                    SQL += ComNum.VBLF + "CANCERTIME ,     ";
                    SQL += ComNum.VBLF + "CANCERSABUN,     ";
                    SQL += ComNum.VBLF + "NOTAPPLYGB ,       ";
                    SQL += ComNum.VBLF + "NOTAPPLYCD ,       ";
                    SQL += ComNum.VBLF + "NOTAPPLYDATE ,     ";
                    SQL += ComNum.VBLF + "NOTAPPLYTIME ,     ";
                    SQL += ComNum.VBLF + "NOTAPPLYSABUN,     ";
                    SQL += ComNum.VBLF + "ILLCODE ,   ";
                    SQL += ComNum.VBLF + "SUCODE ,   ";
                    SQL += ComNum.VBLF + "OPDATE ,   ";
                    SQL += ComNum.VBLF + "DROPREMARK ,   ";
                    SQL += ComNum.VBLF + "CANCERREMARK ,   ";
                    SQL += ComNum.VBLF + "NOTAPPLYREMARK ,   ";
                    SQL += ComNum.VBLF + "LEAVESTATE)   ";
                    SQL += ComNum.VBLF + "SELECT";
                    SQL += ComNum.VBLF + "'" + strCpCode + "' ,         ";
                    SQL += ComNum.VBLF + "PANO ,           ";
                    SQL += ComNum.VBLF + "SNAME ,          ";
                    SQL += ComNum.VBLF + "AGE ,            ";
                    SQL += ComNum.VBLF + "SEX ,            ";
                    SQL += ComNum.VBLF + "DEPTCODE ,       ";
                    SQL += ComNum.VBLF + "'I'  ,           ";
                    SQL += ComNum.VBLF + "TO_CHAR(INDATE, 'YYYY-MM-DD') ,         ";
                    SQL += ComNum.VBLF + "INDATE ,         ";
                    SQL += ComNum.VBLF + "OUTDATE ,        ";
                    SQL += ComNum.VBLF + "BI ,             ";
                    SQL += ComNum.VBLF + "IPDNO ,          ";
                    SQL += ComNum.VBLF + "'" + strDate + "' ,      ";
                    SQL += ComNum.VBLF + "'" + strTime + "' ,      ";
                    SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "',     ";

                    if (cboDropEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "'01',         ";
                        SQL += ComNum.VBLF + "'" + cboDrop.SelectedIndex.ToString("00") + "' ,         ";
                        SQL += ComNum.VBLF + "'" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "'" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "' ,      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                    }

                    if (cboCancerEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "'01',         ";
                        SQL += ComNum.VBLF + "'" + cboCancer.SelectedIndex.ToString("00") + "' ,         ";
                        SQL += ComNum.VBLF + "'" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "'" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "' ,      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                    }
                    if (cboNOTAPPLYEnable.SelectedIndex == 1)
                    {
                        SQL += ComNum.VBLF + "'01',         ";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'" + strDate + "' ,       ";
                        SQL += ComNum.VBLF + "'" + strTime + "' ,       ";
                        SQL += ComNum.VBLF + "'" + clsType.User.Sabun + "' ,      ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                        SQL += ComNum.VBLF + "'',";
                    }
                    SQL += ComNum.VBLF + "'";


                    SQL += ComNum.VBLF + "'" + txtDiagCode.Text.Trim().Replace("'", "`'") + "' AS DIAGNOSISCODE, ";
                    SQL += ComNum.VBLF + "'" + txtOpCode.Text.Trim().Replace("'", "`'") + "' AS OPERATIONCODE, ";
                    SQL += ComNum.VBLF + "'" + (dtpOpDate.Checked ? dtpOpDate.Value.ToShortDateString() : "") + "' AS OPERATIONDATE, ";
                    SQL += ComNum.VBLF + "'" + txtDropRemark.Text.Trim() + "' AS DROPREMARK, ";
                    SQL += ComNum.VBLF + "'" + txtCancerRemark.Text.Trim() + "' AS CANCERREMARK, ";
                    SQL += ComNum.VBLF + "'" + txtNOTAPPLYRemark.Text.Trim() + "' AS NOTAPPLYREMARK, ";
                    SQL += ComNum.VBLF + "'" + (cboLeaveState.SelectedIndex == -1 ? "" : cboLeaveState.Text.Trim().Substring(0, 1)) + "' AS LEAVESTATE";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                    SQL += ComNum.VBLF + "WHERE IPDNO = '" + strIpdNo + "'";

                }

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        string Get_Age(string strBirth)
        {
            if (string.IsNullOrEmpty(strBirth)) return strBirth;
            try
            {
                DateTime today = DateTime.Today;
                DateTime birthDate = Convert.ToDateTime(strBirth);
                int age = today.Year - birthDate.Year;
                if (birthDate > today.AddYears(-age))
                {
                    age--;
                }
                return age.ToString();
            }
            catch
            {
                return strBirth;
            }
        }


        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }

            Control_Clear();
            GetPatientData(e.Row, ssList_Sheet1.Cells[e.Row, 3].Text.Trim());
        }

        void GetPatientData(int row, string strPano)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strOpDate = "";

            #region 기본 내용 보여주기
            cboCP2.SelectedIndex = cboCP2.FindString(ssList_Sheet1.Cells[row, 1].Text.Trim());
            txtCPtNo.Text = ssList_Sheet1.Cells[row, 3].Text.Trim();
            txtCPtNo.Tag  = ssList_Sheet1.Cells[row, 4].Text.Trim(); //IPDNO
            txtCName.Text = ssList_Sheet1.Cells[row, 5].Text.Trim();
            dtpBirth.Value = Convert.ToDateTime(ssList_Sheet1.Cells[row, 6].Text.Trim());
            cboSex.SelectedIndex = ssList_Sheet1.Cells[row, 7].Text.Trim() == "M" ? 0 : 1;

            cboDept.SelectedIndex = -1;
            cboDept.SelectedIndex = cboDept.FindString(ssList_Sheet1.Cells[row, 8].Text.Trim());
            cboDr.SelectedIndex = cboDr.FindString(ssList_Sheet1.Cells[row, 9].Text.Trim());
            #endregion

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                switch(ssList_Sheet1.Cells[row, 12].Text.Trim())
                {
                    case "검": //QI실에서 핸들
                        SQL = "SELECT";
                        SQL += ComNum.VBLF + "A.ILLCODE,";
                        SQL += ComNum.VBLF + "A.SUCODE, A.OPDATE, ";
                        SQL += ComNum.VBLF + "A.DROPGB, A.DROPCD, A.DROPREMARK,";
                        SQL += ComNum.VBLF + "A.CANCERGB, A.CANCERCD, A.CANCERREMARK, ";
                        SQL += ComNum.VBLF + "A.NOTAPPLYGB, A.NOTAPPLYCD, A.NOTAPPLYREMARK, ";
                        SQL += ComNum.VBLF + "TO_CHAR(B.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(B.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                        SQL += ComNum.VBLF + "C.TMODEL";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                        SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_IPD_LIST B";
                        SQL += ComNum.VBLF + "     ON B.IPDNO = A.IPDNO ";
                        SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_SUMMARY C";
                        SQL += ComNum.VBLF + "     ON C.PANO = A.PTNO ";
                        SQL += ComNum.VBLF + "   WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                        SQL += ComNum.VBLF + "     AND B.OUTDATE = C.OUTDATE";
                        break;
                    case "등": // 의사가 핸들
                    case "중": // 의사가 핸들
                    case "제": // 의사가 핸들
                    case "완": // 의사가 핸들
                        SQL = "SELECT";
                        SQL += ComNum.VBLF + "'" + ssList_Sheet1.Cells[row, 10].Text.Trim() + "' ILLCODE, ";
                        SQL += ComNum.VBLF + "'" + ssList_Sheet1.Cells[row, 11].Text.Trim() + "' SUCODE, ";
                        SQL += ComNum.VBLF + "'" + (ssList_Sheet1.Cells[row, 11].Tag != null ? ssList_Sheet1.Cells[row, 11].Tag.ToString() : "") + "' OPDATE, ";
                        SQL += ComNum.VBLF + "A.DROPGB, A.DROPCD, '' DROPREMARK,";
                        SQL += ComNum.VBLF + "A.CANCERGB, A.CANCERCD, '' CANCERREMARK, ";
                        SQL += ComNum.VBLF + "A.NOTAPPLYGB, A.NOTAPPLYCD, '' NOTAPPLYREMARK, ";
                        SQL += ComNum.VBLF + "TO_CHAR(B.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(B.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                        SQL += ComNum.VBLF + "C.TMODEL";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_RECORD A";
                        SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                        SQL += ComNum.VBLF + "     ON B.IPDNO = A.IPDNO ";
                        SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_SUMMARY C";
                        SQL += ComNum.VBLF + "     ON C.PANO = A.PTNO ";
                        SQL += ComNum.VBLF + "   WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                        SQL += ComNum.VBLF + "     AND C.OUTDATE = B.OUTDATE";
                        break;
                    default: //해당 CP대상자 이며 핸들 되지 않은 상태
                        SQL = "SELECT";
                        SQL += ComNum.VBLF + "TO_CHAR(A.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(A.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                        SQL += ComNum.VBLF + "A.ILLCODE, A.SUCODE, ";
                        SQL += ComNum.VBLF + "A.OPDATE ,";
                        SQL += ComNum.VBLF + "'' DROPGB, '' DROPCD, '' DROPREMARK,";
                        SQL += ComNum.VBLF + "'' CANCERGB, '' CANCERCD, '' CANCERREMARK, ";
                        SQL += ComNum.VBLF + "'' NOTAPPLYGB, '' NOTAPPLYCD, '' NOTAPPLYREMARK, ";
                        SQL += ComNum.VBLF + "C.TMODEL";
                        SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_IPD_LIST A";
                        SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_SUMMARY C";
                        SQL += ComNum.VBLF + "     ON C.PANO = A.PANO ";
                        SQL += ComNum.VBLF + " WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                        SQL += ComNum.VBLF + "   AND C.OUTDATE = A.OUTDATE";
                        break;
                }

                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0) // QI실에서 저장한 데이터 없음.
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtDiagCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                txtOpCode.Text   = dt.Rows[0]["SUCODE"].ToString().Trim();

                strOpDate = dt.Rows[0]["OPDATE"].ToString().Trim();
                strOpDate = strOpDate.IndexOf("-") == -1 ? ComFunc.FormatStrToDate(strOpDate, "D") : strOpDate;
                dtpOpDate.Value   = strOpDate.Length == 0 ? DateTime.Now : Convert.ToDateTime(strOpDate);
                dtpOpDate.Checked = strOpDate.Length > 0;

                dtpInDate.Value  = Convert.ToDateTime(dt.Rows[0]["INDATE"].ToString().Trim());
                dtpOutDate.Value = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim());
                txtOpOutDay.Text = dtpOpDate.Checked ? Math.Round((dtpOutDate.Value - dtpOpDate.Value).TotalDays + 1, 2).ToString() : "";

                txtHospitalDay.Text = Math.Round((dtpOutDate.Value - dtpInDate.Value).TotalDays + 1, 2).ToString();

                cboDropEnable.SelectedIndex = dt.Rows[0]["DROPGB"].ToString().Trim() == "01" ? 1 : 0;
                if (cboDropEnable.SelectedIndex == 1)
                {
                    cboDrop.SelectedIndex = dt.Rows[0]["DROPCD"].ToString().Trim().Length == 0 ? -1 : Convert.ToInt32(dt.Rows[0]["DROPCD"].ToString().Trim());
                    txtDropRemark.Text = dt.Rows[0]["DROPREMARK"].ToString().Trim();
                }

                cboCancerEnable.SelectedIndex = dt.Rows[0]["CANCERGB"].ToString().Trim() == "01" ? 1 : 0;
                if (cboCancerEnable.SelectedIndex == 1)
                {
                    cboCancer.SelectedIndex = dt.Rows[0]["CANCERCD"].ToString().Trim().Length == 0 ? -1 : Convert.ToInt32(dt.Rows[0]["CANCERCD"].ToString().Trim());
                    txtCancerRemark.Text = dt.Rows[0]["CANCERREMARK"].ToString().Trim();
                }
                cboNOTAPPLYEnable.SelectedIndex = dt.Rows[0]["NOTAPPLYGB"].ToString().Trim() == "01" ? 1 : 0;
                if (cboNOTAPPLYEnable.SelectedIndex == 1)
                {
                    cboNOTAPPLY.SelectedIndex = dt.Rows[0]["NOTAPPLYCD"].ToString().Trim().Length == 0 ? -1 : Convert.ToInt32(dt.Rows[0]["NOTAPPLYCD"].ToString().Trim());
                    txtNOTAPPLYRemark.Text = dt.Rows[0]["NOTAPPLYREMARK"].ToString().Trim();
                }

                cboLeaveState.SelectedIndex = cboLeaveState.FindString(dt.Rows[0]["TMODEL"].ToString().Trim());

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDrList(cboDept.Text);
        }

        void GetDrList(string strDept)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboDr.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT DRNAME, DRCODE FROM KOSMOS_OCS.OCS_DOCTOR";
                SQL += ComNum.VBLF + "WHERE GBOUT = 'N'";
                SQL += ComNum.VBLF + "  AND DEPTCODE = '" + strDept + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDr.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(50) + "." + dt.Rows[i]["DRCODE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnTongSearch_Click(object sender, EventArgs e)
        {
            GetCpStatistics();
        }

        void GetCpStatistics()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (cboCP3.SelectedIndex == -1)
            {
                ComFunc.MsgBox("CP를 선택해주세요.");
                cboCP3.Select();
                cboCP3.DroppedDown = true;
                return;
            }

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            //DataTable dt2 = null;

            string strCpName = cboCP3.Text.Substring(0, cboCP3.Text.LastIndexOf(".") - 1);
            string strCpCode = cboCP3.Text.Substring(cboCP3.Text.LastIndexOf(".") + 1);

            string strDate = dtpFrDate2.Value.ToShortDateString();
            string strDate2 = dtpToDate2.Value.ToShortDateString();
            string strMonth = string.Empty;
            string strYear = strDate.Substring(0, 4);

            double dAllCnt = 0;
            double dCompleteCnt = 0;
            double dDropCnt = 0;
            double dCancerCnt = 0;
            double dApplyCnt = 0;
            
            ssStatistics_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
 
                SQL = "SELECT TO_CHAR(OUTDATE, 'YYYYMM') YYYYMM, COUNT(*) CNT";
                #region 각종 건 수
                SQL += ComNum.VBLF + ", (";
                SQL += ComNum.VBLF + "    SELECT COUNT(*) MONTHCNT"; // 해당 월 전체 대상 건수
                SQL += ComNum.VBLF + "       FROM " + ComNum.DB_MED + "OCS_CP_IPD_LIST ";
                SQL += ComNum.VBLF + "       WHERE TO_CHAR(ACTDATE, 'YYYYMM') = TO_CHAR(A.OUTDATE, 'YYYYMM')";
                SQL += ComNum.VBLF + "         AND CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "  ) AS MONTHCNT";

                SQL += ComNum.VBLF + ", (";
                SQL += ComNum.VBLF + "      SELECT COUNT(*) APPLYCNT"; // 적용 건수(QI실에서 적용 한 수)
                SQL += ComNum.VBLF + "       FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P";
                SQL += ComNum.VBLF + "       WHERE CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "         AND GBIO = 'I'";
                SQL += ComNum.VBLF + "         AND TO_CHAR(OUTDATE, 'YYYYMM') = TO_CHAR(A.OUTDATE, 'YYYYMM')";
                SQL += ComNum.VBLF + "  ) AS APPLYCNT";

                SQL += ComNum.VBLF + ", (   ";
                SQL += ComNum.VBLF + "      SELECT COUNT(*) DROPCNT"; // 제외 건수
                SQL += ComNum.VBLF + "       FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                SQL += ComNum.VBLF + "         INNER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + "            ON B.IPDNO = A.IPDNO ";
                SQL += ComNum.VBLF + "       WHERE A.CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "         AND A.GBIO = 'I'";
                SQL += ComNum.VBLF + "         AND TO_CHAR(B.OUTDATE, 'YYYYMM') = TO_CHAR(A.OUTDATE, 'YYYYMM')";
                SQL += ComNum.VBLF + "         AND DROPGB = '01'";
                SQL += ComNum.VBLF + " ) DROPCNT";

                SQL += ComNum.VBLF + ", (   ";
                SQL += ComNum.VBLF + "      SELECT COUNT(*) CANCERCNT"; // 중단 건수
                SQL += ComNum.VBLF + "       FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                SQL += ComNum.VBLF + "         INNER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + "            ON B.IPDNO = A.IPDNO ";
                SQL += ComNum.VBLF + "       WHERE A.CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "         AND A.GBIO = 'I'";
                SQL += ComNum.VBLF + "         AND TO_CHAR(B.OUTDATE, 'YYYYMM') = TO_CHAR(A.OUTDATE, 'YYYYMM')";
                SQL += ComNum.VBLF + "         AND CANCERGB = '01'";
                SQL += ComNum.VBLF + " ) CANCERCNT";

                SQL += ComNum.VBLF + ", (   ";
                SQL += ComNum.VBLF + "      SELECT COUNT(*) COMPLETECNT"; // 완결 건수
                SQL += ComNum.VBLF + "       FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                SQL += ComNum.VBLF + "         INNER JOIN " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL += ComNum.VBLF + "            ON B.IPDNO = A.IPDNO ";
                SQL += ComNum.VBLF + "       WHERE A.CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "         AND A.GBIO = 'I'";
                SQL += ComNum.VBLF + "         AND TO_CHAR(B.OUTDATE, 'YYYYMM') = TO_CHAR(A.OUTDATE, 'YYYYMM')";
                SQL += ComNum.VBLF + "         AND DROPGB NOT IN ('01')";
                SQL += ComNum.VBLF + "         AND CANCERGB NOT IN ('01')";
                SQL += ComNum.VBLF + " ) COMPLETECNT";

                #endregion
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_IPD_LIST A";
                SQL += ComNum.VBLF + " WHERE CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "   AND OUTDATE BETWEEN TO_DATE('" + strDate + "', 'YYYY-MM-DD') AND TO_DATE('" + strDate2 + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + " GROUP BY TO_CHAR(OUTDATE, 'YYYYMM') ";
                SQL += ComNum.VBLF + " ORDER BY YYYYMM";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssStatistics_Sheet1.RowCount = dt.Rows.Count;
                ssStatistics_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strYear  = dt.Rows[i]["YYYYMM"].ToString().ToString().Substring(0, 4);
                    strMonth = dt.Rows[i]["YYYYMM"].ToString().ToString();
                    dAllCnt      = double.Parse(dt.Rows[0]["MONTHCNT"].ToString().Trim()) + double.Parse(dt.Rows[0]["CANCERCNT"].ToString().Trim());
                    dApplyCnt    = double.Parse(dt.Rows[0]["APPLYCNT"].ToString().Trim());
                    dDropCnt     = double.Parse(dt.Rows[0]["DROPCNT"].ToString().Trim());
                    dCancerCnt   = double.Parse(dt.Rows[0]["CANCERCNT"].ToString().Trim());
                    dCompleteCnt = double.Parse(dt.Rows[0]["COMPLETECNT"].ToString().Trim());

                    ssStatistics_Sheet1.Cells[i, 0].Text = strCpName;
                    ssStatistics_Sheet1.Cells[i, 1].Text = strYear;
                    ssStatistics_Sheet1.Cells[i, 2].Text = strMonth.Substring(strMonth.Length - 2);
                    ssStatistics_Sheet1.Cells[i, 3].Text = dAllCnt.ToString("#,##0");      //해당 월 전체 대상
                    ssStatistics_Sheet1.Cells[i, 4].Text = dApplyCnt.ToString("#,##0");    // 해당 월 QI실에서 적용한 대상
                    ssStatistics_Sheet1.Cells[i, 5].Text = dDropCnt.ToString("#,##0");     // 의사가 제외한 대상
                    ssStatistics_Sheet1.Cells[i, 6].Text = dCancerCnt.ToString("#,##0");   // 의사가 중단한 대상
                    ssStatistics_Sheet1.Cells[i, 7].Text = dCompleteCnt.ToString("#,##0"); // 의사가 적용 한 후 완결된 대상
                    ssStatistics_Sheet1.Cells[i, 8].Text = Math.Round((dApplyCnt / dAllCnt) * 100, 2).ToString(); //적용률
                    ssStatistics_Sheet1.Cells[i, 9].Text = Math.Round((dCompleteCnt / dAllCnt) * 100, 2).ToString(); //완결률
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }           

        string GetCodeList(string strCpCode, string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            StringBuilder strVal = new StringBuilder();
            string strCompre = strGubun == "04" ? "B.ILLCODE IN(" : "C.SUCODE IN("; //진단일 경우 진단코드 아닐경우 수가코드(수술)

            try
            {
                SQL = "SELECT CODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_SUB";
                SQL += ComNum.VBLF + "WHERE CPCODE = '" + strCpCode + "'";
                SQL += ComNum.VBLF + "  AND GUBUN  = '" + strGubun + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal.ToString().Trim();
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal.ToString().Trim();
                }

                strVal.AppendLine("AND " + strCompre);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strVal.AppendLine( "	'" + dt.Rows[i]["Code"].ToString().Trim() + "' " + (i < dt.Rows.Count - 1 ? "," : ")"));
                }

                dt.Dispose();
                dt = null;


                return strVal.ToString().Trim();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal.ToString().Trim();
            }
        }

        void strGetAge(string strCpCode, ref string strFrAge, ref string strToAge)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL.AppendLine("SELECT FRAGE, TOAGE");
                SQL.AppendLine("FROM " + ComNum.DB_MED + "OCS_CP_MAIN");
                SQL.AppendLine("WHERE CPCODE = '" + strCpCode + "'");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strFrAge = dt.Rows[0]["FRAGE"].ToString().Trim();
                strToAge = dt.Rows[0]["TOAGE"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
            }
        }
              
        private void btnRegist_Click(object sender, EventArgs e)
        {
            Cp_Statistics_Build();
        }

        private string ReadCPIllCode(string strIPDNO, string strCpCode, string strGubun)
        {
            string strRtn = "";
            string strIllCode = string.Concat("       ", GetCodeList(strCpCode, "04")); // 진단 적용코드
            string strSuCode = string.Concat("       ", GetCodeList(strCpCode, "05")); // 수술 적용코드

            string SQL = "";
            string SqlErr = "";

            if (string.IsNullOrEmpty(strSuCode.Trim()) == true && strGubun == "SUCODE")
            {
                return strRtn;
            }

            if (string.IsNullOrEmpty(strIllCode.Trim()) == true && strGubun == "ILLCODE")
            {
                return strRtn;
            }

            DataTable dt = null;

            if (strGubun == "ILLCODE")
            {
                SQL = "	 SELECT B.ILLCODE rtn, ";
            }
            else
            {
                SQL = "	 SELECT C.SUCODE rtn, ";
            }
            SQL += ComNum.VBLF + "	 TO_CHAR(C.BDATE, 'YYYY-MM-DD') OPDATE";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A,";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_IILLS B,";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_SLIP C";
            SQL += ComNum.VBLF + "     WHERE A.IPDNO = " + strIPDNO;
            SQL += ComNum.VBLF + "       AND B.IPDNO = A.IPDNO";
            SQL += ComNum.VBLF + "       AND C.IPDNO = A.IPDNO";
            if (strGubun == "ILLCODE")
            {
                //SQL += ComNum.VBLF + "       AND B.BDATE BETWEEN TO_DATE(A.INDATE, 'YYYY-MM-DD') AND A.ACTDATE";
                SQL += ComNum.VBLF + strIllCode;
            }
            else
            {
                //SQL += ComNum.VBLF + "       AND C.BDATE BETWEEN TO_DATE(A.INDATE, 'YYYY-MM-DD') AND A.ACTDATE";
                SQL += ComNum.VBLF + strSuCode;
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strRtn;
            }

            if (dt.Rows.Count > 0)
            {
                strRtn = dt.Rows[0]["rtn"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return strRtn;
        }

        bool Cp_Statistics_Build()
        {
            if(cboCP1.SelectedIndex == -1 || cboCP1.SelectedIndex == 0)
            {
                ComFunc.MsgBox("전체는 빌드 할 수 없습니다." + ComNum.VBLF +
                               "다시 선택해주세요.");
                cboCP1.Select();
                cboCP1.DroppedDown = true;
                return false;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            DataTable dt = null;

            string strOldIpdNo = string.Empty;
            string strCpName = cboCP1.Text.Substring(0, cboCP1.Text.LastIndexOf(".") - 1);
            string strCpCode = cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1);

            string strFrAge = string.Empty;
            string strToAge = string.Empty;

            string strIllCode = string.Concat("       ", GetCodeList(strCpCode, "04")); // 진단 적용코드
            string strSuCode  = string.Concat("       ", GetCodeList(strCpCode, "05")); // 수술 적용코드

            strGetAge(strCpCode, ref strFrAge, ref strToAge);


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region 해당월 대상자 삭제
                SQL = "DELETE " + ComNum.DB_MED + "OCS_CP_IPD_LIST";
                SQL += ComNum.VBLF + "WHERE TO_CHAR(ACTDATE, 'YYYY-MM') = '" + dtpBuild.Text + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                #endregion

                #region 해당월 대상자 빌드

                SQL = "	 SELECT A.IPDNO, A.PANO, A.SNAME , A.SEX, A.AGE, A.BI, ";
                SQL += ComNum.VBLF + "	 TO_CHAR(A.INDATE, 'YYYY-MM-DD HH24:mi') INDATE, TO_CHAR(A.OUTDATE, 'YYYY-MM-DD') OUTDATE, TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ACTDATE, A.ILSU, ";
                SQL += ComNum.VBLF + "	 A.GBSTS, A.DEPTCODE, A.DRCODE, A.WARDCODE, A.ROOMCODE, ";
                SQL += ComNum.VBLF + "	 B.ILLCODE, C.SUCODE, ";
                SQL += ComNum.VBLF + "	 TO_CHAR(C.BDATE, 'YYYY-MM-DD') OPDATE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_MED  + "OCS_IILLS B,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_SLIP C";
                SQL += ComNum.VBLF + "     WHERE A.OUTDATE BETWEEN TO_DATE('" + dtpBuild.Value.ToString("yyyy-MM-01") + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "       AND TO_DATE('" + dtpBuild.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpBuild.Value.Year, dtpBuild.Value.Month) + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "       AND B.IPDNO = A.IPDNO";
                SQL += ComNum.VBLF + "       AND C.IPDNO = A.IPDNO";
                if (string.IsNullOrEmpty(strFrAge) == false) //최소 나이
                {
                    SQL += ComNum.VBLF + "       AND A.AGE >= " + strFrAge;
                }
                if (string.IsNullOrEmpty(strToAge) == false) //최대 나이
                {
                    SQL += ComNum.VBLF + "       AND A.AGE <= " + strToAge;
                }

                if (string.IsNullOrEmpty(strIllCode) == false)
                {
                    SQL += ComNum.VBLF + "       AND B.BDATE BETWEEN TO_DATE(A.INDATE, 'YYYY-MM-DD') AND A.ACTDATE";
                    SQL += ComNum.VBLF + strIllCode;
                }

                if (string.IsNullOrEmpty(strSuCode) == false)
                {
                    SQL += ComNum.VBLF + "       AND C.BDATE BETWEEN TO_DATE(A.INDATE, 'YYYY-MM-DD') AND A.ACTDATE";
                    SQL += ComNum.VBLF + strSuCode;
                }

                SQL += ComNum.VBLF + "     ORDER BY IPDNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if(dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당월에 해당하는 대상자가 없습니다.");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                //strOldIpdNo = dt.Rows[0]["IPDNO"].ToString().Trim();

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["IPDNO"].ToString().Trim() != strOldIpdNo)
                    {
                        //IPDNO 다를때 추가
                        strOldIpdNo = dt.Rows[i]["IPDNO"].ToString().Trim();

                        SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_IPD_LIST";
                        SQL += ComNum.VBLF + "(IPDNO,           ";
                        SQL += ComNum.VBLF + "PANO,             ";
                        SQL += ComNum.VBLF + "SNAME,            ";
                        SQL += ComNum.VBLF + "SEX,              ";
                        SQL += ComNum.VBLF + "AGE ,             ";
                        SQL += ComNum.VBLF + "BI ,              ";
                        SQL += ComNum.VBLF + "INDATE ,          ";
                        SQL += ComNum.VBLF + "OUTDATE ,         ";
                        SQL += ComNum.VBLF + "ACTDATE ,         ";
                        SQL += ComNum.VBLF + "ILSU,             ";
                        SQL += ComNum.VBLF + "GBSTS ,           ";
                        SQL += ComNum.VBLF + "DEPTCODE,         ";
                        SQL += ComNum.VBLF + "DRCODE ,          ";
                        SQL += ComNum.VBLF + "WARDCODE,         ";
                        SQL += ComNum.VBLF + "ROOMCODE,         ";
                        SQL += ComNum.VBLF + "ILLCODE,    ";
                        SQL += ComNum.VBLF + "SUCODE,           ";
                        SQL += ComNum.VBLF + "CPCODE,           ";
                        SQL += ComNum.VBLF + "OPDATE            ";
                        SQL += ComNum.VBLF + ")             ";
                        SQL += ComNum.VBLF + "VALUES            ";
                        SQL += ComNum.VBLF + "(                 ";
                        SQL += ComNum.VBLF + "'" + strOldIpdNo + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["SEX"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["AGE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "TO_DATE('" + dt.Rows[i]["INDATE"].ToString().Trim() + "', 'YYYY-MM-DD HH24:mi') ,";
                        SQL += ComNum.VBLF + "TO_DATE('" + dt.Rows[i]["OUTDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "TO_DATE('" + dt.Rows[i]["ACTDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["ILSU"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["GBSTS"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["DRCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["ILLCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["SUCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "'" + strCpCode + "',";
                        SQL += ComNum.VBLF + "'" + dt.Rows[i]["OPDATE"].ToString().Trim() + "'";
                        SQL += ComNum.VBLF + ")";

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
                }

                dt.Dispose();
                dt = null;

                #endregion

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

        private void btnPrintExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssList_Sheet1.RowCount == 0) return;

            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                mDlg.ShowDialog();

                if (mDlg.FileName.Length == 0) return;

                ssList.ActiveSheet.Protect = false;
                ssList.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.NoFormulas | FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            }

            ssList.ActiveSheet.Protect = true;
        }

        private void ssStatistics_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.Column == 3 || e.Column == 5)
            {
                if (frmEmrViewerX != null)
                {
                    frmEmrViewerX.SetNewPatient(ssList_Sheet1.Cells[e.Row, 3].Text.Trim());
                    return;
                }
                else
                {
                    frmEmrViewerX = new frmEmrViewer(ssList_Sheet1.Cells[e.Row, 3].Text.Trim());
                    frmEmrViewerX.rEventClosed += FrmEmrViewerX_rEventClosed;
                    frmEmrViewerX.Show();
                }
                //clsVbEmr.EXECUTE_TextEmrViewEx(ssList_Sheet1.Cells[e.Row, 3].Text.Trim(), clsType.User.IdNumber);
            }

            return;
        }

        private void FrmEmrViewerX_rEventClosed()
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Dispose();
                frmEmrViewerX = null;
            }
        }

        private void ssStatistics_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            if(e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            e.ShowTip = true;

            switch(e.Column)
            {
                case 3:
                    e.TipText = "해당 CP 전체 대상자";
                    break;
                case 4:
                    e.TipText = "QI실에서 적용한 대상자";
                    break;
                case 5:
                    e.TipText = "제외 한 수";
                    break;
                case 6:
                    e.TipText = "중단한 수";
                    break;
                case 7:
                    e.TipText = "CP가 적용 된 후 제외/중단 없이 퇴원한 환자 ";
                    break;
            }
        }

        private void frmOcsCpTong_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Close();
                frmEmrViewerX = null;
            }
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }

        }

        private void frmOcsCpTong_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }
    }
}
