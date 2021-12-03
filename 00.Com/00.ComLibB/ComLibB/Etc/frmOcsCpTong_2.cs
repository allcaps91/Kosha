using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using System.Text;
using FarPoint.Win.Spread;

namespace ComLibB
{
    public partial class frmOcsCpTong_2 : Form, MainFormMessage

    {
       
        /// <summary>
        /// EMR 뷰어
        /// </summary>
        ///
        frmEmrViewer frmEmrViewerX = null;

        ComFunc cf = new ComFunc();

        string fstrWard = "";

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


        public frmOcsCpTong_2(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }



        public frmOcsCpTong_2()
        {
            InitializeComponent();
        }

        public frmOcsCpTong_2(string argWard)
        {
            InitializeComponent();
            fstrWard = argWard;
        }

        private void frmOcsCpTong_2_Load(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            ComFunc.ReadSysDate(clsDB.DbCon);
            
            Set_Combo();
            ComboWard_SET();
            GetDataCpCode();

            if (clsType.User.JobGroup != "JOB016002" && clsType.User.JobGroup != "JOB000001")
            {
                ssList_Sheet1.SetColumnAllowFilter(-1, true);
                if (fstrWard == "")
                {
                    fstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
                }
                panel3.Visible = false;
                //panContent.Visible = false;
                chkExcept.Visible = false;
                btnSave.Visible = false;
                btnRegist.Visible = false;
                cboWard.Enabled = false;
                cboWard.Items.Clear();
                cboWard.Items.Add(fstrWard);
                cboWard.SelectedIndex = 0;

                rbtnGubun1.Enabled = false;
                rbtnGubun2.Enabled = false;
                dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 7) + "-01";
                dtpToDate.Text = cf.READ_LASTDAY(clsDB.DbCon, dtpFrDate.Text.Trim());
            }
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

            cboNOTAPPLYEnable.Items.Clear();
            cboNOTAPPLYEnable.Items.Add("00.적용");
            cboNOTAPPLYEnable.Items.Add("01.미적용");


            cboLeaveState.Items.Clear();
            cboLeaveState.Items.Add("1.퇴원지시후");
            cboLeaveState.Items.Add("2.자의퇴원");
            cboLeaveState.Items.Add("3.전송");
            cboLeaveState.Items.Add("4.탈원");
            cboLeaveState.Items.Add("5.사망");
            cboLeaveState.Items.Add("6.강제퇴원");

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

        private void ComboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboWard.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Add("**");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                    cboWard.SelectedIndex = 0;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
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

        private void GetDataCpCode_Gubun()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string strCPCODE = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            strCPCODE = VB.Right(cboCP2.Text.Trim(),10);
            cboCancer.Items.Clear();
            cboDrop.Items.Clear();
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + " A.GRPCD, A.BASCD, BB.name BASNAME, A.BASNAME1 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD A,";
                SQL = SQL + ComNum.VBLF + "( select B.CODE, B.NAME from " + ComNum.DB_MED + "OCS_CP_SUB B WHERE GUBUN  in ( '02','03') ";
                SQL = SQL + ComNum.VBLF + " AND B.SDATE  = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = B.CPCODE ) ";
                SQL = SQL + ComNum.VBLF + " AND B.CPCODE=  '" + strCPCODE + "'    ) BB";

                SQL = SQL + ComNum.VBLF + "WHERE A.GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GRPCD IN('CP중단사유', 'CP제외기준')";
                SQL = SQL + ComNum.VBLF + "  AND A.BASCD = BB.CODE";
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
                        switch (dt.Rows[i]["GRPCD"].ToString().Trim())
                        {
                            
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

        void GetSearchData(string ViewOption = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            //if (cboCP1.SelectedIndex == 0)
            //{
            //    ComFunc.MsgBox("CP를 선택해주세요.");
            //    cboCP1.Focus();
            //    return;
            //}

            

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            int i = 0;
            int j = 0;
            int nCPNum = 0;
            int nROW = 0;

            string strSUCODE1 = "";     //수가코드
            string strSUCODE2 = "";     //처방일자

            string strCPCODE = "";
            string strGubun = "";
            string strIllCode = "";
            string strSuCode = "";

            string strSDATE;
            string strEDATE;

            if (ViewOption != "")
            {

                strSDATE = dtpFrDate.Value.ToShortDateString();
                strSDATE = VB.Left(strSDATE, 7) + "-01";
                strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);
                //퇴원일 기준으로 강제 설정
                rbtnGubun1.Checked = true;

                strCPCODE = "전체";
                
            }
            else
            {
                strSDATE = dtpFrDate.Value.ToShortDateString();
                strEDATE = dtpToDate.Value.ToShortDateString();

                strCPCODE = cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1);
            }
            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                

                SQL = "";
                SQL += ComNum.VBLF + " SELECT BASCD ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                SQL += ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
                SQL += ComNum.VBLF + "    AND BASNAME1 = 'IPD' ";
                SQL += ComNum.VBLF + "    AND APLENDDATE >= '" + VB.Replace(strSDATE,"-","") + "' ";
                if (strCPCODE != "전체")
                {
                    SQL += ComNum.VBLF + "    AND BASCD = '" + strCPCODE + "' ";
                }
                SQL += ComNum.VBLF + "  ORDER BY BASCD ";
                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt2.Rows.Count == 0)
                {
                    dt2.Dispose();
                    dt2 = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                nCPNum = dt2.Rows.Count;

                for (j = 0; j < nCPNum; j++)
                {

                    strCPCODE = dt2.Rows[j]["BASCD"].ToString().Trim();

                    strGubun = GetCPGUBUN(strCPCODE); ;       //조건 구분 01: 상병만, 02: 수가만, 03:상병+수가
                    strIllCode = string.Concat("       ", GetCodeList(strCPCODE, "04", "1")).Trim(); ; // 진단 적용코드
                    strSuCode = string.Concat("       ", GetCodeList(strCPCODE, "05", "1")).Trim(); // 수술 적용코드
                    //strIllCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "04", "1")).Trim(); ; // 진단 적용코드
                    //strSuCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "05", "1")).Trim(); // 수술 적용코드
                     
                    SQL = "";
                    SQL += ComNum.VBLF + "WITH CP AS ( ";
                    SQL += ComNum.VBLF + "            SELECT IPDNO, PTNO FROM ( ";
                    if (strGubun == "01" || strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "              SELECT IPDNO, PTNO FROM ( ";
                        SQL += ComNum.VBLF + "                        SELECT A.IPDNO, A.PTNO, B.OUTDATE, B.INDATE  ";
                        SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_IILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "                         WHERE ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                           AND A.IPDNO = B.IPDNO ";
                        SQL += ComNum.VBLF + "                        UNION ALL ";
                        SQL += ComNum.VBLF + "                        SELECT B.IPDNO, B.PANO, B.OUTDATE, B.INDATE ";
                        SQL += ComNum.VBLF + "                         FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                        SQL += ComNum.VBLF + "                        WHERE A.BDATE = TRUNC(B.INDATE) ";
                        SQL += ComNum.VBLF + "                          AND A.DEPTCODE = B.DEPTCODE ";
                        SQL += ComNum.VBLF + "                          AND A.PTNO = B.PANO ";
                        SQL += ComNum.VBLF + "                          AND ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                        UNION ALL ";
                        SQL += ComNum.VBLF + "                        SELECT B.IPDNO, B.PANO , B.OUTDATE, B.INDATE ";
                        SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                        SQL += ComNum.VBLF + "                         WHERE A.BDATE = TRUNC(B.INDATE) ";
                        SQL += ComNum.VBLF + "                           AND A.PTNO = B.PANO ";
                        SQL += ComNum.VBLF + "                           AND ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                      )";
                        if (rbtnGubun1.Checked == true)
                        {
                            SQL += ComNum.VBLF + "       WHERE OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "       WHERE ( (INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "          AND INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                            SQL += ComNum.VBLF + "                      OR (OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                            SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND OUTDATE IS NULL)) ";

                        }
                        if (chkSearch.Checked == true)
                        {
                            SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND PTNO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                        }


                    }

                    if (strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "                INTERSECT ";
                    }
                    if (strGubun == "02" || strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "                SELECT A.IPDNO, A.PANO ";
                        SQL += ComNum.VBLF + "                  FROM KOSMOS_PMPA.IPD_NEW_SLIP A, KOSMOS_PMPA.IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "                 WHERE SUCODE IN ( ";
                        SQL += ComNum.VBLF + strSuCode;
                        SQL += ComNum.VBLF + "                   AND A.IPDNO = B.IPDNO ";
                        if (chkSearch.Checked == true)
                        {
                            SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND B.PANO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                        }

                        if (rbtnGubun1.Checked == true)
                        {
                            SQL += ComNum.VBLF + "       AND B.OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "       AND ( (B.INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "          AND B.INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                            SQL += ComNum.VBLF + "                      OR (B.OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND B.OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                            SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND B.OUTDATE IS NULL)) ";
                        }
                        SQL += ComNum.VBLF + "                          group by A.IPDNO,A.PANO,SUCODE having   SUM(NAL * QTY) > 0 ";
                    }


                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + " SELECT A.IPDNO, A.PTNO ";
                    SQL += ComNum.VBLF + "  FROM ( ";
                    SQL += ComNum.VBLF + "        SELECT IPDNO, PTNO, CPCODE FROM KOSMOS_OCS.OCS_CP_RECORD ";
                    SQL += ComNum.VBLF + "        UNION ALL ";
                    SQL += ComNum.VBLF + "        SELECT IPDNO, PTNO, CPCODE FROM KOSMOS_OCS.OCS_CP_RECORD_P S ";
                    SQL += ComNum.VBLF + "         WHERE NOT EXISTS (SELECT * FROM KOSMOS_OCS.OCS_CP_RECORD WHERE IPDNO = S.IPDNO AND CPCODE = S.CPCODE) ";
                    SQL += ComNum.VBLF + "       ) A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                    SQL += ComNum.VBLF + " WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "   AND A.CPCODE = '" + strCPCODE + "' ";
                    if (chkSearch.Checked == true)
                    {
                        SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND B.PANO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    }

                    if (rbtnGubun1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND B.OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "       AND ( (B.INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                        SQL += ComNum.VBLF + "          AND B.INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                        SQL += ComNum.VBLF + "                      OR (B.OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND B.OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND B.OUTDATE IS NULL)) ";
                        SQL += ComNum.VBLF + "     GROUP BY A.IPDNO, A.PTNO ";
                    }
                    SQL += ComNum.VBLF + "                                    ) ";
                    SQL += ComNum.VBLF + "            GROUP BY IPDNO, PTNO ";
                    SQL += ComNum.VBLF + "        ) ";
                    //====================여기 까지 WITH CP======================================================================
                    SQL += ComNum.VBLF + "        ( ";
                    SQL += ComNum.VBLF + "    SELECT C.CPCODE,  ";
                    SQL += ComNum.VBLF + "            CASE WHEN CANCERDATE IS NOT NULL THEN '중단' ";
                    SQL += ComNum.VBLF + "                 WHEN DROPDATE IS NOT NULL THEN '제외' ";
                    SQL += ComNum.VBLF + "                 WHEN NOTAPPLYDATE IS NOT NULL THEN '미적용' ";
                    SQL += ComNum.VBLF + "                 WHEN STARTDATE IS NOT NULL THEN '적용' ELSE '' END CP_PROGRESS, ";
                    SQL += ComNum.VBLF + "           A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.IPWONTIME,'YYYY-MM-DD') IPWONTIME,    ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.STARTDATE,'YYYY-MM-DD'),'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD') CDATE, C.CANCERCD, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD') DROPDATE,  ";
                    SQL += ComNum.VBLF + "           '' ILLCODE, '' SUCODE, A.ILSU, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "           A.AMSET7, D.INWARD, D.OUTWARD, C.ROWID CP_ROWID, A.IPDNO, '' GUBUN          ";
                    SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_NEW_MASTER A, CP B, KOSMOS_OCS.OCS_CP_RECORD C, KOSMOS_PMPA.NUR_MASTER D  ";
                    SQL += ComNum.VBLF + "     WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = D.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = C.IPDNO(+) ";
                    SQL += ComNum.VBLF + "       AND B.IPDNO not in (SELECT IPDNO FROM KOSMOS_OCS.OCS_CP_RECORD  UNION ALL  SELECT IPDNO FROM KOSMOS_OCS.OCS_CP_RECORD_P S )";
                    if (chkER.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND A.AMSET7 NOT IN ('3','4','5') ";
                    }

                    SQL += ComNum.VBLF + "       AND  A.DEPTCODE =  ( SELECT VFLAG1 FROM KOSMOS_PMPA.BAS_BASCD WHERE GRPCDB = 'CP관리'   AND GRPCD = 'CP코드관리' AND BASCD = '" + strCPCODE + "' AND ROWNUM = 1) ";

                    if (cboWard.Text.Trim() != "**")
                    {
                        SQL += ComNum.VBLF + "       AND A.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + "    SELECT C.CPCODE,  ";
                    SQL += ComNum.VBLF + "            CASE WHEN CANCERDATE IS NOT NULL THEN '중단' ";
                    SQL += ComNum.VBLF + "                 WHEN DROPDATE IS NOT NULL THEN '제외' ";
                    SQL += ComNum.VBLF + "                 WHEN NOTAPPLYDATE IS NOT NULL THEN '미적용' ";
                    SQL += ComNum.VBLF + "                 WHEN STARTDATE IS NOT NULL THEN '적용' ELSE '' END CP_PROGRESS, ";
                    SQL += ComNum.VBLF + "           A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.IPWONTIME,'YYYY-MM-DD') IPWONTIME,    ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.STARTDATE,'YYYY-MM-DD'),'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD') CDATE, C.CANCERCD, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD') DROPDATE,  ";
                    SQL += ComNum.VBLF + "           '' ILLCODE, '' SUCODE, A.ILSU, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "           A.AMSET7, D.INWARD, D.OUTWARD, C.ROWID CP_ROWID, A.IPDNO, '' GUBUN          ";
                    SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_NEW_MASTER A, CP B, KOSMOS_OCS.OCS_CP_RECORD C, KOSMOS_PMPA.NUR_MASTER D  ";
                    SQL += ComNum.VBLF + "     WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = D.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = C.IPDNO ";
                    SQL += ComNum.VBLF + "       AND B.IPDNO not in ( SELECT IPDNO FROM KOSMOS_OCS.OCS_CP_RECORD_P S )";
                    SQL += ComNum.VBLF + "       AND '" + strCPCODE + "' = C.CPCODE ";
                    if (chkER.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND A.AMSET7 NOT IN ('3','4','5') ";
                    }

                    if (cboWard.Text.Trim() != "**")
                    {
                        SQL += ComNum.VBLF + "       AND A.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + "    SELECT C.CPCODE,  ";
                    SQL += ComNum.VBLF + "            CASE WHEN CANCERDATE IS NOT NULL THEN '중단' ";
                    SQL += ComNum.VBLF + "                 WHEN DROPDATE IS NOT NULL THEN '제외' ";
                    SQL += ComNum.VBLF + "                 WHEN NOTAPPLYDATE IS NOT NULL THEN '미적용' ";
                    SQL += ComNum.VBLF + "                 WHEN STARTDATE IS NOT NULL THEN '적용' ELSE '' END CP_PROGRESS, ";
                    SQL += ComNum.VBLF + "           A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.IPWONTIME,'YYYY-MM-DD') IPWONTIME,    ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.STARTDATE,'YYYY-MM-DD'),'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD') CDATE, C.CANCERCD, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD') DROPDATE,  ";
                    SQL += ComNum.VBLF + "           '' ILLCODE, '' SUCODE, A.ILSU, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "           A.AMSET7, D.INWARD, D.OUTWARD, C.ROWID CP_ROWID, A.IPDNO, 'QI' GUBUN     ";
                    SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_NEW_MASTER A, CP B, KOSMOS_OCS.OCS_CP_RECORD_P C, KOSMOS_PMPA.NUR_MASTER D  ";
                    SQL += ComNum.VBLF + "     WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = D.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = C.IPDNO ";
                    SQL += ComNum.VBLF + "       AND '" + strCPCODE + "' = C.CPCODE ";
                    if (chkER.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND A.AMSET7 NOT IN ('3','4','5') ";
                    }
                    if (cboWard.Text.Trim() != "**")
                    {
                        SQL += ComNum.VBLF + "       AND A.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    SQL += ComNum.VBLF + "        ) ";
                    SQL += ComNum.VBLF + " ORDER BY IPWONTIME, PANO, SNAME ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    Cursor.Current = Cursors.Default;
                    //    return;
                    //}

                    //ssList_Sheet1.RowCount = dt.Rows.Count;

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList_Sheet1.RowCount += 1;
                            nROW = ssList_Sheet1.RowCount - 1;

                            ssList_Sheet1.Cells[nROW, 0].Text = ReadCPBASNAME("CP코드관리", dt.Rows[i]["CPCODE"].ToString().Trim());
                            if(ssList_Sheet1.Cells[nROW, 0].Text.Trim() == "")
                            {
                                ssList_Sheet1.Cells[nROW, 0].Text = ReadCPBASNAME("CP코드관리", strCPCODE);
                            }
                            ssList_Sheet1.Cells[nROW, 1].Text = dt.Rows[i]["CP_PROGRESS"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 7].Text = cf.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                            ssList_Sheet1.Cells[nROW, 8].Text = dt.Rows[i]["IPWONTIME"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 9].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 10].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 11].Text = GetCPSAYU(dt.Rows[i]["CANCERCD"].ToString().Trim(), "중단");
                            //ssList_Sheet1.Cells[nROW, 12].Text = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "ILLCODE");
                            //strSUCODE1 = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "SUCODE");
                            ssList_Sheet1.Cells[nROW, 12].Text = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), strCPCODE, "ILLCODE");
                            strSUCODE1 = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), strCPCODE, "SUCODE");
                            strSUCODE2 = VB.Right(strSUCODE1, 10);
                            strSUCODE1 = VB.Mid(strSUCODE1, 1, VB.Len(strSUCODE1) - 11).Trim();
                            ssList_Sheet1.Cells[nROW, 13].Text = strSUCODE1;
                            ssList_Sheet1.Cells[nROW, 13].Tag = strSUCODE2;
                            ssList_Sheet1.Cells[nROW, 14].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                            //if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "중단")
                            if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "중단")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = (cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["CDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "적용")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = (cf.DATE_ILSU(clsDB.DbCon, clsPublic.GstrSysDate, dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                                if  (VB.Val(ssList_Sheet1.Cells[nROW, 15].Text ) > VB.Val(GetCPDay(strCPCODE))   )
                                {
                                    ssList_Sheet1.Cells[nROW, 15].Text = GetCPDay(strCPCODE);
                                }
                                if (VB.Val(ssList_Sheet1.Cells[nROW, 15].Text) > VB.Val(dt.Rows[i]["ILSU"].ToString().Trim()))
                                {
                                    ssList_Sheet1.Cells[nROW, 15].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                                }
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "제외")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = "";//(cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["DROPDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "미적용")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = "";//(cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["DROPDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                            }

                            //ssList_Sheet1.Cells[nROW, 15].Text = GetCPDay(dt.Rows[i]["CPCODE"].ToString().Trim());
                            ssList_Sheet1.Cells[nROW, 16].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                            switch (dt.Rows[i]["AMSET7"].ToString().Trim())
                            {
                                case "3":
                                case "4":
                                case "5":
                                    ssList_Sheet1.Cells[nROW, 17].Text = "Y";
                                    break;
                            }
                            ssList_Sheet1.Cells[nROW, 18].Text = dt.Rows[i]["INWARD"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 19].Text = dt.Rows[i]["OUTWARD"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 20].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 21].Text = dt.Rows[i]["CP_ROWID"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 22].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 23].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                            ssList_Sheet1.Cells[nROW, 24].Text = strCPCODE;
                            ssList_Sheet1.Cells[nROW, 25].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 26].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            //ssList_Sheet1.Rows[i].Height = ssList_Sheet1.Rows[i].GetPreferredHeight() + 5;

                        }
                    }
                    dt.Dispose();
                    dt = null;

                }

                dt2.Dispose();
                dt = null;

                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
        void GetSearchData_Bak(string ViewOption = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            //if (cboCP1.SelectedIndex == 0)
            //{
            //    ComFunc.MsgBox("CP를 선택해주세요.");
            //    cboCP1.Focus();
            //    return;
            //}



            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            int i = 0;
            int j = 0;
            int nCPNum = 0;
            int nROW = 0;

            string strSUCODE1 = "";     //수가코드
            string strSUCODE2 = "";     //처방일자

            string strCPCODE = "";
            string strGubun = "";
            string strIllCode = "";
            string strSuCode = "";

            string strSDATE;
            string strEDATE;

            if (ViewOption != "")
            {

                strSDATE = dtpFrDate.Value.ToShortDateString();
                strSDATE = VB.Left(strSDATE, 7) + "-01";
                strEDATE = cf.READ_LASTDAY(clsDB.DbCon, strSDATE);
                //퇴원일 기준으로 강제 설정
                rbtnGubun1.Checked = true;

                strCPCODE = "전체";

            }
            else
            {
                strSDATE = dtpFrDate.Value.ToShortDateString();
                strEDATE = dtpToDate.Value.ToShortDateString();

                strCPCODE = cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1);
            }
            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {


                SQL = "";
                SQL += ComNum.VBLF + " SELECT BASCD ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                SQL += ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
                SQL += ComNum.VBLF + "    AND BASNAME1 = 'IPD' ";
                SQL += ComNum.VBLF + "    AND APLENDDATE >= '" + VB.Replace(strSDATE, "-", "") + "' ";
                if (strCPCODE != "전체")
                {
                    SQL += ComNum.VBLF + "    AND BASCD = '" + strCPCODE + "' ";
                }
                SQL += ComNum.VBLF + "  ORDER BY BASCD ";
                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt2.Rows.Count == 0)
                {
                    dt2.Dispose();
                    dt2 = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                nCPNum = dt2.Rows.Count;

                for (j = 0; j < nCPNum; j++)
                {

                    strCPCODE = dt2.Rows[j]["BASCD"].ToString().Trim();

                    strGubun = GetCPGUBUN(strCPCODE); ;       //조건 구분 01: 상병만, 02: 수가만, 03:상병+수가
                    strIllCode = string.Concat("       ", GetCodeList(strCPCODE, "04", "1")).Trim(); ; // 진단 적용코드
                    strSuCode = string.Concat("       ", GetCodeList(strCPCODE, "05", "1")).Trim(); // 수술 적용코드
                    //strIllCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "04", "1")).Trim(); ; // 진단 적용코드
                    //strSuCode = string.Concat("       ", GetCodeList(cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "05", "1")).Trim(); // 수술 적용코드

                    SQL = "";
                    SQL += ComNum.VBLF + "WITH CP AS ( ";
                    SQL += ComNum.VBLF + "            SELECT IPDNO, PTNO FROM ( ";
                    if (strGubun == "01" || strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "              SELECT IPDNO, PTNO FROM ( ";
                        SQL += ComNum.VBLF + "                        SELECT A.IPDNO, A.PTNO, B.OUTDATE, B.INDATE  ";
                        SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_IILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "                         WHERE ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                           AND A.IPDNO = B.IPDNO ";
                        SQL += ComNum.VBLF + "                        UNION ALL ";
                        SQL += ComNum.VBLF + "                        SELECT B.IPDNO, B.PANO, B.OUTDATE, B.INDATE ";
                        SQL += ComNum.VBLF + "                         FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                        SQL += ComNum.VBLF + "                        WHERE A.BDATE = TRUNC(B.INDATE) ";
                        SQL += ComNum.VBLF + "                          AND A.DEPTCODE = B.DEPTCODE ";
                        SQL += ComNum.VBLF + "                          AND A.PTNO = B.PANO ";
                        SQL += ComNum.VBLF + "                          AND ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                        UNION ALL ";
                        SQL += ComNum.VBLF + "                        SELECT B.IPDNO, B.PANO , B.OUTDATE, B.INDATE ";
                        SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                        SQL += ComNum.VBLF + "                         WHERE A.BDATE = TRUNC(B.INDATE) ";
                        SQL += ComNum.VBLF + "                           AND A.PTNO = B.PANO ";
                        SQL += ComNum.VBLF + "                           AND ILLCODE IN (" + strIllCode;
                        SQL += ComNum.VBLF + "                      )";
                        if (rbtnGubun1.Checked == true)
                        {
                            SQL += ComNum.VBLF + "       WHERE OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "       WHERE ( (INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "          AND INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                            SQL += ComNum.VBLF + "                      OR (OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                            SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND OUTDATE IS NULL)) ";

                        }
                        if (chkSearch.Checked == true)
                        {
                            SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND PTNO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                        }


                    }

                    if (strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "                INTERSECT ";
                    }
                    if (strGubun == "02" || strGubun == "03")
                    {
                        SQL += ComNum.VBLF + "                SELECT A.IPDNO, A.PANO ";
                        SQL += ComNum.VBLF + "                  FROM KOSMOS_PMPA.IPD_NEW_SLIP A, KOSMOS_PMPA.IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "                 WHERE SUCODE IN ( ";
                        SQL += ComNum.VBLF + strSuCode;
                        SQL += ComNum.VBLF + "                   AND A.IPDNO = B.IPDNO ";
                        if (chkSearch.Checked == true)
                        {
                            SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND B.PANO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                        }

                        if (rbtnGubun1.Checked == true)
                        {
                            SQL += ComNum.VBLF + "       AND B.OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "       AND ( (B.INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                            SQL += ComNum.VBLF + "          AND B.INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                            SQL += ComNum.VBLF + "                      OR (B.OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND B.OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                            SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "                          AND B.OUTDATE IS NULL)) ";
                        }
                    }


                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + " SELECT A.IPDNO, A.PTNO ";
                    SQL += ComNum.VBLF + "  FROM ( ";
                    SQL += ComNum.VBLF + "        SELECT IPDNO, PTNO, CPCODE FROM KOSMOS_OCS.OCS_CP_RECORD ";
                    SQL += ComNum.VBLF + "        UNION ALL ";
                    SQL += ComNum.VBLF + "        SELECT IPDNO, PTNO, CPCODE FROM KOSMOS_OCS.OCS_CP_RECORD_P S ";
                    SQL += ComNum.VBLF + "         WHERE NOT EXISTS (SELECT * FROM KOSMOS_OCS.OCS_CP_RECORD WHERE IPDNO = S.IPDNO AND CPCODE = S.CPCODE) ";
                    SQL += ComNum.VBLF + "       ) A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                    SQL += ComNum.VBLF + " WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "   AND A.CPCODE = '" + strCPCODE + "' ";
                    if (chkSearch.Checked == true)
                    {
                        SQL += (txtPtNo.Text.Length == 0 ? "" : ComNum.VBLF + "       AND B.PANO = '" + VB.Val(txtPtNo.Text.Trim()).ToString("00000000") + "'");
                    }

                    if (rbtnGubun1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND B.OUTDATE BETWEEN TO_DATE('" + strSDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "       AND TO_DATE('" + strEDATE + "', 'YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "       AND ( (B.INDATE >=  TO_DATE ('" + strSDATE + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                        SQL += ComNum.VBLF + "          AND B.INDATE <=  TO_DATE ('" + strEDATE + " 23:59', 'YYYY-MM-DD HH24:MI')) ";
                        SQL += ComNum.VBLF + "                      OR (B.OUTDATE >= TO_DATE ('" + strSDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND B.OUTDATE <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD')) ";
                        SQL += ComNum.VBLF + "                      OR (TRUNC (SYSDATE) <= TO_DATE ('" + strEDATE + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "                          AND B.OUTDATE IS NULL)) ";
                        SQL += ComNum.VBLF + "     GROUP BY A.IPDNO, A.PTNO ";
                    }
                    SQL += ComNum.VBLF + "                                    ) ";
                    SQL += ComNum.VBLF + "            GROUP BY IPDNO, PTNO ";
                    SQL += ComNum.VBLF + "        ) ";
                    //====================여기 까지 WITH CP======================================================================
                    SQL += ComNum.VBLF + "        ( ";
                    SQL += ComNum.VBLF + "    SELECT C.CPCODE,  ";
                    SQL += ComNum.VBLF + "            CASE WHEN CANCERDATE IS NOT NULL THEN '중단' ";
                    SQL += ComNum.VBLF + "                 WHEN DROPDATE IS NOT NULL THEN '제외' ";
                    SQL += ComNum.VBLF + "                 WHEN NOTAPPLYDATE IS NOT NULL THEN '미적용' ";
                    SQL += ComNum.VBLF + "                 WHEN STARTDATE IS NOT NULL THEN '적용' ELSE '' END CP_PROGRESS, ";
                    SQL += ComNum.VBLF + "           A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.IPWONTIME,'YYYY-MM-DD') IPWONTIME,    ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.STARTDATE,'YYYY-MM-DD'),'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD') CDATE, C.CANCERCD, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD') DROPDATE,  ";
                    SQL += ComNum.VBLF + "           '' ILLCODE, '' SUCODE, A.ILSU, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "           A.AMSET7, D.INWARD, D.OUTWARD, C.ROWID CP_ROWID, A.IPDNO, '' GUBUN          ";
                    SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_NEW_MASTER A, CP B, KOSMOS_OCS.OCS_CP_RECORD C, KOSMOS_PMPA.NUR_MASTER D  ";
                    SQL += ComNum.VBLF + "     WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = D.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = C.IPDNO(+) ";
                    if (chkER.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND A.AMSET7 NOT IN ('3','4','5') ";
                    }

                    SQL += ComNum.VBLF + "       AND  A.DEPTCODE =  ( SELECT VFLAG1 FROM KOSMOS_PMPA.BAS_BASCD WHERE GRPCDB = 'CP관리'   AND GRPCD = 'CP코드관리' AND BASCD = '" + strCPCODE + "' AND ROWNUM = 1) ";
                    SQL += ComNum.VBLF + "      AND '" + strCPCODE + "' = C.CPCODE(+) ";
                    if (cboWard.Text.Trim() != "**")
                    {
                        SQL += ComNum.VBLF + "       AND A.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + "    SELECT C.CPCODE,  ";
                    SQL += ComNum.VBLF + "            CASE WHEN CANCERDATE IS NOT NULL THEN '중단' ";
                    SQL += ComNum.VBLF + "                 WHEN DROPDATE IS NOT NULL THEN '제외' ";
                    SQL += ComNum.VBLF + "                 WHEN NOTAPPLYDATE IS NOT NULL THEN '미적용' ";
                    SQL += ComNum.VBLF + "                 WHEN STARTDATE IS NOT NULL THEN '적용' ELSE '' END CP_PROGRESS, ";
                    SQL += ComNum.VBLF + "           A.PANO, A.SNAME, A.SEX, A.AGE, A.DEPTCODE, A.DRCODE, TO_CHAR(A.IPWONTIME,'YYYY-MM-DD') IPWONTIME,    ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.STARTDATE,'YYYY-MM-DD'),'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.CANCERDATE,'YYYY-MM-DD'),'YYYY-MM-DD') CDATE, C.CANCERCD, ";
                    SQL += ComNum.VBLF + "           TO_CHAR(TO_DATE(C.DROPDATE,'YYYY-MM-DD'),'YYYY-MM-DD') DROPDATE,  ";
                    SQL += ComNum.VBLF + "           '' ILLCODE, '' SUCODE, A.ILSU, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "           A.AMSET7, D.INWARD, D.OUTWARD, C.ROWID CP_ROWID, A.IPDNO, 'QI' GUBUN     ";
                    SQL += ComNum.VBLF + "      FROM KOSMOS_PMPA.IPD_NEW_MASTER A, CP B, KOSMOS_OCS.OCS_CP_RECORD_P C, KOSMOS_PMPA.NUR_MASTER D  ";
                    SQL += ComNum.VBLF + "     WHERE A.IPDNO = B.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = D.IPDNO ";
                    SQL += ComNum.VBLF + "       AND A.IPDNO = C.IPDNO ";
                    if (chkER.Checked == true)
                    {
                        SQL += ComNum.VBLF + "       AND A.AMSET7 NOT IN ('3','4','5') ";
                    }
                    if (cboWard.Text.Trim() != "**")
                    {
                        SQL += ComNum.VBLF + "       AND A.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                    SQL += ComNum.VBLF + "        ) ";
                    SQL += ComNum.VBLF + " ORDER BY IPWONTIME, PANO, SNAME ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    Cursor.Current = Cursors.Default;
                    //    return;
                    //}

                    //ssList_Sheet1.RowCount = dt.Rows.Count;

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssList_Sheet1.RowCount += 1;
                            nROW = ssList_Sheet1.RowCount - 1;

                            ssList_Sheet1.Cells[nROW, 0].Text = ReadCPBASNAME("CP코드관리", dt.Rows[i]["CPCODE"].ToString().Trim());
                            if (ssList_Sheet1.Cells[nROW, 0].Text.Trim() == "")
                            {
                                ssList_Sheet1.Cells[nROW, 0].Text = ReadCPBASNAME("CP코드관리", strCPCODE);
                            }
                            ssList_Sheet1.Cells[nROW, 1].Text = dt.Rows[i]["CP_PROGRESS"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 7].Text = cf.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                            ssList_Sheet1.Cells[nROW, 8].Text = dt.Rows[i]["IPWONTIME"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 9].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 10].Text = dt.Rows[i]["CDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 11].Text = GetCPSAYU(dt.Rows[i]["CANCERCD"].ToString().Trim(), "중단");
                            //ssList_Sheet1.Cells[nROW, 12].Text = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "ILLCODE");
                            //strSUCODE1 = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), cboCP1.Text.Substring(cboCP1.Text.LastIndexOf(".") + 1), "SUCODE");
                            ssList_Sheet1.Cells[nROW, 12].Text = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), strCPCODE, "ILLCODE");
                            strSUCODE1 = ReadCPIllCode(dt.Rows[i]["IPDNO"].ToString().Trim(), strCPCODE, "SUCODE");
                            strSUCODE2 = VB.Right(strSUCODE1, 10);
                            strSUCODE1 = VB.Mid(strSUCODE1, 1, VB.Len(strSUCODE1) - 11).Trim();
                            ssList_Sheet1.Cells[nROW, 13].Text = strSUCODE1;
                            ssList_Sheet1.Cells[nROW, 13].Tag = strSUCODE2;
                            ssList_Sheet1.Cells[nROW, 14].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                            //if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "중단")
                            if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "중단")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = (cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["CDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString();
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "적용")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = (cf.DATE_ILSU(clsDB.DbCon, clsPublic.GstrSysDate, dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString();
                                if (VB.Val(ssList_Sheet1.Cells[nROW, 15].Text) > VB.Val(GetCPDay(strCPCODE)))
                                {
                                    ssList_Sheet1.Cells[nROW, 15].Text = GetCPDay(strCPCODE);
                                }
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "제외")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = "";//(cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["DROPDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                            }
                            else if (ssList_Sheet1.Cells[nROW, 1].Text.Trim() == "미적용")
                            {
                                ssList_Sheet1.Cells[nROW, 15].Text = "";//(cf.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["DROPDATE"].ToString().Trim(), dt.Rows[i]["SDATE"].ToString().Trim()) + 1).ToString(); 
                            }

                            //ssList_Sheet1.Cells[nROW, 15].Text = GetCPDay(dt.Rows[i]["CPCODE"].ToString().Trim());
                            ssList_Sheet1.Cells[nROW, 16].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                            switch (dt.Rows[i]["AMSET7"].ToString().Trim())
                            {
                                case "3":
                                case "4":
                                case "5":
                                    ssList_Sheet1.Cells[nROW, 17].Text = "Y";
                                    break;
                            }
                            ssList_Sheet1.Cells[nROW, 18].Text = dt.Rows[i]["INWARD"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 19].Text = dt.Rows[i]["OUTWARD"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 20].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 21].Text = dt.Rows[i]["CP_ROWID"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 22].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 23].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                            ssList_Sheet1.Cells[nROW, 24].Text = strCPCODE;
                            ssList_Sheet1.Cells[nROW, 25].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                            ssList_Sheet1.Cells[nROW, 26].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                            //ssList_Sheet1.Rows[i].Height = ssList_Sheet1.Rows[i].GetPreferredHeight() + 5;

                        }
                    }
                    dt.Dispose();
                    dt = null;

                }

                dt2.Dispose();
                dt = null;

                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

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
                        SQL += ComNum.VBLF + "CANCERREMARK = '" + txtCancerRemark.Text.Trim().Replace("'", "`") + "',";
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
                        SQL += ComNum.VBLF + "NOTAPPLYREMARK = '" + txtNOTAPPLYRemark.Text.Trim().Replace("'", "`") + "',";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "NOTAPPLYGB     = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYCD     = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYDATE   = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYTIME   = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYSABUN  = '',";
                        SQL += ComNum.VBLF + "NOTAPPLYREMARK = '',";
                    }

                    SQL += ComNum.VBLF + "CPCODE = '" + strCpCode + "', ";
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
                ComFunc.MsgBox("저장후 다시 재조회 진행");
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
            GetPatientData(e.Row, ssList_Sheet1.Cells[e.Row, 2].Text.Trim());
        }

        void GetPatientData(int row, string strPano)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            

            string strOpDate = "";
            string strROWID = ssList_Sheet1.Cells[row, 21].Text.Trim();

            #region 기본 내용 보여주기
            if (ssList_Sheet1.Cells[row, 0].Text.Trim() != "")
            {
                //cboCP2.SelectedIndex = cboCP2.FindString(ssList_Sheet1.Cells[row, 0].Text.Trim());
                cboCP2.SelectedIndex = cboCP2.FindString(ReadCPBASCodelist(ssList_Sheet1.Cells[row, 0].Text.Trim()));
                


            }
            else
            {
                cboCP2.SelectedIndex = cboCP2.FindString(cboCP1.Text.Trim());
               
               
            }
            txtCPtNo.Text = ssList_Sheet1.Cells[row, 2].Text.Trim();
            txtCPtNo.Tag  = ssList_Sheet1.Cells[row, 22].Text.Trim(); //IPDNO
            txtCName.Text = ssList_Sheet1.Cells[row, 3].Text.Trim();
            dtpBirth.Value = Convert.ToDateTime(GetBirthDay(strPano));
            //dtpBirth.Value = Convert.ToDateTime(ssList_Sheet1.Cells[row, 6].Text.Trim()); 
            cboSex.SelectedIndex = ssList_Sheet1.Cells[row, 2].Text.Trim() == "M" ? 0 : 1;

            cboDept.SelectedIndex = -1;
            cboDept.SelectedIndex = cboDept.FindString(ssList_Sheet1.Cells[row, 6].Text.Trim());
            cboDr.SelectedIndex = cboDr.FindString(ssList_Sheet1.Cells[row, 7].Text.Trim());
            //CP코드에따른 사유 선택
            if (btnSave.Visible ==false)
            {
                GetDataCpCode_Gubun();
            }

            

            #endregion

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (ssList_Sheet1.Cells[row, 23].Text.Trim() == "QI")
                {
                    SQL = "SELECT";
                    SQL += ComNum.VBLF + "A.ILLCODE,";
                    SQL += ComNum.VBLF + "A.SUCODE, A.OPDATE, ";
                    SQL += ComNum.VBLF + "A.DROPGB, A.DROPCD, A.DROPREMARK,";
                    SQL += ComNum.VBLF + "A.CANCERGB, A.CANCERCD, A.CANCERREMARK, ";
                    SQL += ComNum.VBLF + "A.NOTAPPLYGB, A.NOTAPPLYCD, A.NOTAPPLYREMARK, ";
                    SQL += ComNum.VBLF + "TO_CHAR(C.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(C.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                    SQL += ComNum.VBLF + "C.TMODEL";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_RECORD_P A";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_CP_IPD_LIST2 B";
                    SQL += ComNum.VBLF + "     ON B.IPDNO = A.IPDNO ";
                    SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_SUMMARY C";
                    SQL += ComNum.VBLF + "     ON C.PANO = A.PTNO ";
                    SQL += ComNum.VBLF + "   WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                    SQL += ComNum.VBLF + "     AND B.OUTDATE = C.OUTDATE";
                }
                else
                {
                    switch (ssList_Sheet1.Cells[row, 1].Text.Trim())
                    {
                        case "적용": // 의사가 핸들
                        case "중단": // 의사가 핸들
                        case "제거": // 의사가 핸들
                        case "완료": // 의사가 핸들
                            SQL = "SELECT";
                            SQL += ComNum.VBLF + "'" + ssList_Sheet1.Cells[row, 12].Text.Trim() + "' ILLCODE, ";
                            SQL += ComNum.VBLF + "'" + ssList_Sheet1.Cells[row, 13].Text.Trim() + "' SUCODE, ";
                            SQL += ComNum.VBLF + "'" + (ssList_Sheet1.Cells[row, 13].Tag != null ? ssList_Sheet1.Cells[row, 13].Tag.ToString() : "") + "' OPDATE, ";
                            SQL += ComNum.VBLF + "A.DROPGB, A.DROPCD, '' DROPREMARK,";
                            SQL += ComNum.VBLF + "A.CANCERGB, A.CANCERCD, '' CANCERREMARK, ";
                            SQL += ComNum.VBLF + "A.NOTAPPLYGB, A.NOTAPPLYCD, '' NOTAPPLYREMARK, ";

                            SQL += ComNum.VBLF + "TO_CHAR(B.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(B.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                            SQL += ComNum.VBLF + "  C.TMODEL";
                            SQL += ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_CP_RECORD A, KOSMOS_PMPA.IPD_NEW_MASTER B, KOSMOS_PMPA.MID_SUMMARY C ";
                            SQL += ComNum.VBLF + "   WHERE A.IPDNO = B.IPDNO ";
                            SQL += ComNum.VBLF + "     AND B.PANO = C.PANO(+) ";
                            SQL += ComNum.VBLF + "     AND B.OUTDATE = C.OUTDATE(+) ";
                            //SQL += ComNum.VBLF + "   WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                            SQL += ComNum.VBLF + "     AND A.ROWID = '" + strROWID + "'";
                            break;
                        default: //해당 CP대상자 이며 핸들 되지 않은 상태
                            SQL = "SELECT";
                            SQL += ComNum.VBLF + "TO_CHAR(A.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(A.OUTDATE, 'YYYY-MM-DD') OUTDATE, ";
                            SQL += ComNum.VBLF + "'" + ssList_Sheet1.Cells[row, 12].Text.Trim() + "' ILLCODE, '" + ssList_Sheet1.Cells[row, 13].Text.Trim() + "' SUCODE, ";
                            SQL += ComNum.VBLF + "'' OPDATE ,";
                            SQL += ComNum.VBLF + "'' DROPGB, '' DROPCD, '' DROPREMARK,";
                            SQL += ComNum.VBLF + "'' CANCERGB, '' CANCERCD, '' CANCERREMARK, ";
                            SQL += ComNum.VBLF + "'' NOTAPPLYGB, '' NOTAPPLYCD, '' NOTAPPLYREMARK, ";
                            SQL += ComNum.VBLF + "C.TMODEL";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A";
                            SQL += ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_PMPA + "MID_SUMMARY C";
                            SQL += ComNum.VBLF + "     ON C.PANO = A.PANO ";
                            SQL += ComNum.VBLF + " WHERE A.IPDNO = '" + txtCPtNo.Tag.ToString() + "'";
                            SQL += ComNum.VBLF + "   AND C.OUTDATE = A.OUTDATE";
                            break;
                    }
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

                //txtDiagCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                //txtOpCode.Text   = dt.Rows[0]["SUCODE"].ToString().Trim();
                txtDiagCode.Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                txtOpCode.Text = dt.Rows[0]["SUCODE"].ToString().Trim();

                strOpDate = dt.Rows[0]["OPDATE"].ToString().Trim();
                strOpDate = strOpDate.IndexOf("-") == -1 ? ComFunc.FormatStrToDate(strOpDate, "D") : strOpDate;
                dtpOpDate.Value   = strOpDate.Length == 0 ? DateTime.Now : Convert.ToDateTime(strOpDate);
                dtpOpDate.Checked = strOpDate.Length > 0;

                dtpInDate.Text = dt.Rows[0]["INDATE"].ToString().Trim();
                if (dt.Rows[0]["OUTDATE"].ToString().Trim() == "")
                {
                    dtpOutDate.Text = "2999-12-12";
                    dtpOutDate.Enabled = false;
                }
                else
                {
                    dtpOutDate.Text = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    dtpOutDate.Enabled = true;
                }
                
                
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

        private string GetBirthDay(string strPANO)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            try
            {
                SQL = "SELECT BIRTH FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL += ComNum.VBLF + "WHERE PANO = '" + strPANO + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["BIRTH"].ToString().Trim();
                
                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string GetCPDay(string strCPCODE)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            try
            {
                SQL = "  SELECT CPDAY ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_CP_MAIN A ";
                SQL += ComNum.VBLF + "  WHERE SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE) ";
                SQL += ComNum.VBLF + "    AND CPCODE = '" + strCPCODE + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["CPDAY"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string GetCPSAYU(string strCODE, string strGUBUN)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            if (strCODE == "")
            {
                return strRTN;
            }
            else
            {
                strCODE = (VB.Val(strCODE) + 1).ToString();
            }

            try
            {
                SQL = " SELECT BASNAME FROM (";
                SQL += ComNum.VBLF + " SELECT BASNAME, ROWNUM RNUM ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                SQL += ComNum.VBLF + "   WHERE GRPCDB = 'CP관리' ";
                if (strGUBUN == "중단")
                {
                    SQL += ComNum.VBLF + "     AND GRPCD = 'CP중단사유' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "     AND GRPCD = 'CP제외기준' ";
                }
                SQL += ComNum.VBLF + " ORDER BY BASCD ASC ";
                SQL += ComNum.VBLF + ") WHERE RNUM = '" +  strCODE + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["BASNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string GetCPGUBUN(string strCODE)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            if (strCODE == "")
            {
                return strRTN;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GUBUN ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_CP_MAIN A ";
                SQL += ComNum.VBLF + " WHERE SDATE = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE) ";
                SQL += ComNum.VBLF + "   AND CPCODE = '" + strCODE + "'";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["GUBUN"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string ReadCPBASNAME(string strGRPCD, string strBASCD)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            try
            {
                SQL = "  SELECT BASNAME ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                SQL += ComNum.VBLF + "    AND GRPCD = '" + strGRPCD + "' ";
                SQL += ComNum.VBLF + "    AND BASCD = '" + strBASCD + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["BASNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string ReadCPBASCodelist(string strBASName)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strRTN = "";

            try
            {
                SQL = "  SELECT GRPCD, BASCD, BASNAME, BASNAME1 ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BASCD ";
                SQL += ComNum.VBLF + "  WHERE GRPCDB = 'CP관리' ";
                SQL += ComNum.VBLF + "    AND GRPCD IN('CP코드관리') ";
                SQL += ComNum.VBLF + "    AND BASNAME = '" + strBASName + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return "";
                }

                strRTN = dt.Rows[0]["BASNAME"].ToString().Trim() + VB.Space(50) + "." + dt.Rows[0]["BASCD"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return strRTN;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }


        string GetCodeList(string strCpCode, string strGubun, string strGubun2 = "")
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

                if (strGubun2 == "")
                {
                    strVal.AppendLine("AND " + strCompre);
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    strVal.AppendLine(" '')");
                    return strVal.ToString().Trim();
                }

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
            string strIllCode = string.Concat("       ", GetCodeList(strCpCode, "04", "1")); // 진단 적용코드
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
                SQL = "";
                SQL += ComNum.VBLF + "                        SELECT A.ILLCODE rtn, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_IILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B  ";
                SQL += ComNum.VBLF + "                         WHERE ILLCODE IN (" + strIllCode;
                SQL += ComNum.VBLF + "                           AND A.IPDNO = B.IPDNO ";
                SQL += ComNum.VBLF + "                           AND B.IPDNO = " + strIPDNO;
                SQL += ComNum.VBLF + "                        UNION ALL ";
                SQL += ComNum.VBLF + "                        SELECT A.ILLCODE rtn, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "                         FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + "                        WHERE A.BDATE = TRUNC(B.INDATE) ";
                SQL += ComNum.VBLF + "                          AND A.DEPTCODE = B.DEPTCODE ";
                SQL += ComNum.VBLF + "                          AND A.PTNO = B.PANO ";
                SQL += ComNum.VBLF + "                           AND B.IPDNO = " + strIPDNO;
                SQL += ComNum.VBLF + "                          AND ILLCODE IN (" + strIllCode;
                SQL += ComNum.VBLF + "                        UNION ALL ";
                SQL += ComNum.VBLF + "                        SELECT A.ILLCODE rtn, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "                          FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + "                         WHERE A.BDATE = TRUNC(B.INDATE) ";
                SQL += ComNum.VBLF + "                           AND A.PTNO = B.PANO ";
                SQL += ComNum.VBLF + "                           AND B.IPDNO = " + strIPDNO;
                SQL += ComNum.VBLF + "                           AND ILLCODE IN (" + strIllCode;
            }
            else
            {
                SQL = "	 SELECT C.SUCODE rtn, TO_CHAR(C.BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.IPD_NEW_SLIP C";
                SQL += ComNum.VBLF + "     WHERE A.IPDNO = " + strIPDNO;
                SQL += ComNum.VBLF + "       AND C.IPDNO = A.IPDNO";
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
                if (strGubun == "ILLCODE")
                {
                    strRtn = dt.Rows[0]["rtn"].ToString().Trim();
                }
                else
                {
                    strRtn = dt.Rows[0]["rtn"].ToString().Trim() + "        " + dt.Rows[0]["BDATE"].ToString().Trim();
                }
                
            }

            dt.Dispose();
            dt = null;

            return strRtn;
        }

        bool Cp_Statistics_Build()
        {
    
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인

            GetSearchData("1");

            string strYYMM = VB.Left(dtpFrDate.Value.ToShortDateString(), 7).Replace("-","");

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strIPDNO = "";
            string strPANO = "";
            string strCPCODE_T = "";
            string strCPCODE = "";
            string strCP_STATUS = "";
            string strWRITEGUBUN = "";
            string strDEPTCODE = "";
            string strDRCODE = "";
            string strILLCODE = "";
            string strSUCODE = "";
            string strAGE = "";
            string strOPDATE = "";
            string strOUTDATE = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region 해당월 대상자 삭제
                SQL = "DELETE " + ComNum.DB_MED + "OCS_CP_IPD_LIST2";
                SQL += ComNum.VBLF + "WHERE YYMM = '" + strYYMM + "'";

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


                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strIPDNO = ssList_Sheet1.Cells[i, 25].Text.Trim();
                    strPANO = ssList_Sheet1.Cells[i, 2].Text.Trim();
                    strCPCODE_T = ssList_Sheet1.Cells[i, 24].Text.Trim();
                    strCPCODE = ssList_Sheet1.Cells[i, 20].Text.Trim();
                    strCP_STATUS = ssList_Sheet1.Cells[i, 1].Text.Trim();
                    strWRITEGUBUN = ssList_Sheet1.Cells[i, 23].Text.Trim();
                    strDEPTCODE = ssList_Sheet1.Cells[i, 6].Text.Trim();
                    strDRCODE = ssList_Sheet1.Cells[i, 26].Text.Trim();
                    strILLCODE = ssList_Sheet1.Cells[i, 12].Text.Trim();
                    strSUCODE = ssList_Sheet1.Cells[i, 13].Text.Trim();
                    strAGE = ssList_Sheet1.Cells[i, 5].Text.Trim();
                    strOPDATE = ssList_Sheet1.Cells[i, 13].Tag.ToString().Trim();
                    strOUTDATE = ssList_Sheet1.Cells[i, 16].Text.Trim();

                    if (strWRITEGUBUN != "")
                    {
                        strWRITEGUBUN = "Y";
                    }
                    if (strIPDNO == "1201685")
                    {
                        strIPDNO = strIPDNO;
                    }

                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_IPD_LIST2 (";
                    SQL += ComNum.VBLF + " YYMM, IPDNO, PANO, CPCODE_T, ";
                    SQL += ComNum.VBLF + " CPCODE, CP_STATUS, WRITEGUBUN, DEPTCODE,  ";
                    SQL += ComNum.VBLF + " DRCODE, ILLCODE, SUCODE, AGE,  ";
                    SQL += ComNum.VBLF + " OPDATE, OUTDATE  ";
                    SQL += ComNum.VBLF + ") VALUES (                 ";
                    SQL += ComNum.VBLF + "'" + strYYMM + "'," + strIPDNO + ",'" + strPANO + "','" + strCPCODE_T + "',";
                    SQL += ComNum.VBLF + "'" + strCPCODE + "','" + strCP_STATUS + "','" + strWRITEGUBUN + "','" + strDEPTCODE + "',";
                    SQL += ComNum.VBLF + "'" + strDRCODE + "','" + strILLCODE + "','" + strSUCODE + "'," + strAGE + ",";
                    SQL += ComNum.VBLF + " TO_DATE('" + strOPDATE + "','YYYY-MM-DD'), TO_DATE('" + strOUTDATE + "','YYYY-MM-DD') ";
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

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.Column == 0 || e.Column == 2 || e.Column == 3)
            {
                if (frmEmrViewerX != null)
                {
                    frmEmrViewerX.SetNewPatient(ssList_Sheet1.Cells[e.Row, 2].Text.Trim());
                    return;
                }
                else
                {
                    frmEmrViewerX = new frmEmrViewer(ssList_Sheet1.Cells[e.Row, 2].Text.Trim());
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

        private void frmOcsCpTong_2_FormClosed(object sender, FormClosedEventArgs e)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = dtpFrDate.Text + "~" + dtpToDate.Text + "Critical Pathway 대상자 목록";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, true, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnPrint_CP_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strCPCODE = "";
            string strSDATE = "";

            FarPoint.Win.Spread.FpSpread ssSpread = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                DataTable dt2 = null;
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME,APLFRDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + cboCP2.Text.Substring(cboCP2.Text.LastIndexOf(".") + 1) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt2.Rows.Count > 0)
                {
                    txtCPNAME.Text = dt2.Rows[0]["BASNAME"].ToString().Trim();
                    
                    strCPCODE = dt2.Rows[0]["BASCD"].ToString().Trim();
                }
                else
                {
                    txtCPNAME.Text = "";
                    //dtpSDate.Text = "";
                }
                dt2.Dispose();
                dt2 = null;

                

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    CPCODE, SDATE, EDATE, GBIO, GUBUN, SCALE, SCALERMK, FRAGE, TOAGE, CPDAY, GBINDICATOR, GBAGREE, OCSEDUFILE,OCSEDUFILE1, PATEDUFILE,PATEDUFILE1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_MAIN ";
                SQL = SQL + ComNum.VBLF + "     WHERE CPCODE = '" + cboCP2.Text.Substring(cboCP2.Text.LastIndexOf(".") + 1) + "'   order by sdate desc ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
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
                    ComFunc.MsgBox("데이타가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }
                strSDATE = dt.Rows[0]["SDATE"].ToString().Trim();
                //dtpSDate.Text = ComFunc.FormatStrToDate(dt.Rows[0]["SDATE"].ToString().Trim(), "D");
                

                if (dt.Rows[0]["GBIO"].ToString().Trim() == "E") { rdoIO0.Checked = true; }
                else if (dt.Rows[0]["GBIO"].ToString().Trim() == "I") { rdoIO1.Checked = true; }

                if (dt.Rows[0]["GUBUN"].ToString().Trim() == "01") { rdoGUBUN0.Checked = true; }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "02") { rdoGUBUN1.Checked = true; }
                else if (dt.Rows[0]["GUBUN"].ToString().Trim() == "03") { rdoGUBUN2.Checked = true; }

                if (dt.Rows[0]["SCALE"].ToString().Trim() == "01") { chkSCALE0.Checked = true; }
                else if (dt.Rows[0]["SCALE"].ToString().Trim() == "02") { chkSCALE1.Checked = true; }
                else if (dt.Rows[0]["SCALE"].ToString().Trim() == "03")
                {
                    chkSCALE2.Checked = true;
                   // txtSCALE.Text = dt.Rows[0]["SCALERMK"].ToString().Trim();
                }

                txtFRAGE.Text = dt.Rows[0]["FRAGE"].ToString().Trim();
                txtTOAGE.Text = dt.Rows[0]["TOAGE"].ToString().Trim();
                txtDay.Text = dt.Rows[0]["CPDAY"].ToString().Trim();

               

                if (dt.Rows[0]["GBINDICATOR"].ToString().Trim() == "Y") { rdoINDICATOR0.Checked = true; }
                else { rdoINDICATOR1.Checked = true; }

                if (dt.Rows[0]["GBAGREE"].ToString().Trim() == "Y") { rdoAGREE0.Checked = true; }
                else { rdoAGREE1.Checked = true; }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= 7; i++)
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     CODE, NAME, REMARK, TYPE, SCODE, CPVALUE, INPUTGBC, INPUTGBS, DSPSEQ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_SUB";
                    SQL = SQL + ComNum.VBLF + "     WHERE CPCODE = '" + strCPCODE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND SDATE = '" + strSDATE + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '" + i.ToString("00") + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY  DSPSEQ";

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
                        if (i == 1)
                        {
                            for (k = 0; k < dt.Rows.Count; k++)
                            {
                                if (dt.Rows[k]["CODE"].ToString().Trim() == "01") { chkBI0.Checked = true; }
                                if (dt.Rows[k]["CODE"].ToString().Trim() == "02") { chkBI1.Checked = true; }
                            }

                            continue;
                        }
                        else if (i == 2) { ssSpread = ssStop; }
                        else if (i == 3) { ssSpread = ssExcept; }
                        else if (i == 4) { ssSpread = ssILLCode; }
                        else if (i == 5) { ssSpread = ssOpCode; }
                        else if (i == 6) { ssSpread = ssINDICATOR; }
                        else if (i == 7) { ssSpread = ssAGREE; }

                        if (ssSpread == ssILLCode || ssSpread == ssOpCode)
                        {
                            ssSpread.ActiveSheet.RowCount = dt.Rows.Count + 1;
                        }
                        else
                        {
                            ssSpread.ActiveSheet.RowCount = dt.Rows.Count;
                        }
                        ssSpread.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            ssSpread.ActiveSheet.Cells[k, 0].Text = dt.Rows[k]["CODE"].ToString().Trim();
                            ssSpread.ActiveSheet.Cells[k, 1].Text = dt.Rows[k]["NAME"].ToString().Trim();
                            if (ssSpread == ssINDICATOR)
                            {
                                ssSpread.ActiveSheet.Cells[k, 2].Text = dt.Rows[k]["TYPE"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 3].Text = dt.Rows[k]["INPUTGBC"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 4].Text = dt.Rows[k]["SCODE"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 6].Text = dt.Rows[k]["INPUTGBS"].ToString().Trim();
                                ssSpread.ActiveSheet.Cells[k, 8].Text = dt.Rows[k]["DSPSEQ"].ToString().Trim();
                                if (VB.Val(dt.Rows[k]["CPVALUE"].ToString().Trim()) != 0)
                                {
                                    ssSpread.ActiveSheet.Cells[k, 7].Text = dt.Rows[k]["CPVALUE"].ToString().Trim();
                                }

                                if (dt.Rows[k]["SCODE"].ToString().Trim() != "")
                                {
                                    DataTable dt1 = null;
                                    SQL = "";
                                    SQL = "SELECT";
                                    SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME";
                                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                                    SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리' ";
                                    SQL = SQL + ComNum.VBLF + "    AND GRPCD = 'CP지표참조' ";
                                    SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + dt.Rows[k]["SCODE"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return;
                                    }
                                    if (dt1.Rows.Count > 0)
                                    {
                                        ssSpread.ActiveSheet.Cells[k, 5].Text = dt1.Rows[0]["BASNAME"].ToString().Trim();
                                    }
                                    else
                                    {
                                        ssSpread.ActiveSheet.Cells[k, 5].Text = "";
                                    }
                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;


               
                Set_Spread_Clear();
                Set_Spread();
                clsSpread SPR = new clsSpread();


                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 0, 0);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

                //SPR.setSpdPrint(ss10, true, setMargin, setOption, "", "");

                //ss10_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
                ss10_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                ss10_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ss10_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ss10_Sheet1.PrintInfo.Margin.Top = 60;
                ss10_Sheet1.PrintInfo.Margin.Bottom = 20;
                ss10_Sheet1.PrintInfo.ShowColor = true;
                ss10_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ss10_Sheet1.PrintInfo.ShowBorder = false;
                ss10_Sheet1.PrintInfo.ShowGrid = false;
                ss10_Sheet1.PrintInfo.ShowShadows = false;
                ss10_Sheet1.PrintInfo.UseMax = true;
                ss10_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ss10_Sheet1.PrintInfo.UseSmartPrint = false;
                ss10_Sheet1.PrintInfo.ShowPrintDialog = false;
                ss10_Sheet1.PrintInfo.Preview = true;
                ss10.PrintSheet(0);

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

        void Set_Spread()
        {
            int i = 0;
            int k = 0;
            ss10_Sheet1.Cells[2, 1].Text = txtCPNAME.Text;
           // ss10_Sheet1.Cells[2, 8].Text = dtpSDate.Text + " 등록";

            if (rdoIO0.Checked == true)
            {
                ss10_Sheet1.Cells[3, 1].Text = "■ 응급     □ 입원  ";
            }
            else if (rdoIO1.Checked == true)
            {
                ss10_Sheet1.Cells[3, 1].Text = "□ 응급     ■ 입원  ";
            }
            else
            {
                ss10_Sheet1.Cells[3, 1].Text = "□ 응급     □ 입원  ";
            }
            if (rdoGUBUN0.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "■ 진단  □ 수술  □ 진단+수술 ";
            }
            else if (rdoGUBUN1.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  ■ 수술  □ 진단+수술 ";
            }
            else if (rdoGUBUN2.Checked == true)
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  □ 수술  ■ 진단+수술 ";
            }
            else
            {
                ss10_Sheet1.Cells[4, 1].Text = "□ 진단  □ 수술  □ 진단+수술 ";
            }

            if (chkSCALE0.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "■ 입퇴원  □  POST-OP 퇴원  □ 기타 ";
            }
            else if (chkSCALE1.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  ■  POST-OP 퇴원  □ 기타";
            }
            else if (chkSCALE2.Checked == true)
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  □  POST-OP 퇴원  ■ 기타";
            }
            else
            {
                ss10_Sheet1.Cells[5, 1].Text = "□ 입퇴원  □  POST-OP 퇴원  □ 기타";
            }


            for (i = 0; i < ssILLCode_Sheet1.RowCount; i++)
            {

                ss10_Sheet1.Cells[i + 7, 2].Text = ssILLCode_Sheet1.Cells[i, 0].Text;
                ss10_Sheet1.Cells[i + 7, 3].Text = ssILLCode_Sheet1.Cells[i, 1].Text;

            }
            for (i = 0; i < ssOpCode_Sheet1.RowCount; i++)
            {

                ss10_Sheet1.Cells[i + 7, 5].Text = ssOpCode_Sheet1.Cells[i, 0].Text;
                ss10_Sheet1.Cells[i + 7, 6].Text = ssOpCode_Sheet1.Cells[i, 1].Text;

            }
            ss10_Sheet1.Cells[27, 2].Text = "(  " + txtFRAGE.Text + "   )세이상 " + "(  " + txtTOAGE.Text + "   )세이하 ";


            if (chkBI0.Checked == true)
            {
                ss10_Sheet1.Cells[28, 2].Text = "■ 보험     □ 보호  ";
            }
            else
            {
                ss10_Sheet1.Cells[28, 2].Text = "□ 보험     ■ 보호  ";
            }


            for (i = 0; i < ssExcept_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 29, 1].Text = ssExcept_Sheet1.Cells[i, 1].Text;
            }

            for (i = 0; i < ssStop_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 29, 5].Text = ssStop_Sheet1.Cells[i, 1].Text;
            }


            ss10_Sheet1.Cells[44, 1].Text = "(  " + txtDay.Text + "   )일";

            if (rdoINDICATOR0.Checked == true)
            {
                ss10_Sheet1.Cells[45, 1].Text = "■ 유    □ 무 ";
            }
            else
            {
                ss10_Sheet1.Cells[45, 1].Text = "□ 유    ■ 무 ";
            }


            for (i = 0; i < ssINDICATOR_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[i + 47, 1].Text = ssINDICATOR_Sheet1.Cells[i, 1].Text;
                ss10_Sheet1.Cells[i + 47, 3].Text = ssINDICATOR_Sheet1.Cells[i, 5].Text;
                ss10_Sheet1.Cells[i + 47, 7].Text = ssINDICATOR_Sheet1.Cells[i, 7].Text;
                ss10_Sheet1.Cells[i + 47, 9].Text = ssINDICATOR_Sheet1.Cells[i, 8].Text;

            }
            if (rdoAGREE0.Checked == true)
            {
                ss10_Sheet1.Cells[53, 1].Text = "■ 유    □ 무 ";
            }
            else
            {
                ss10_Sheet1.Cells[53, 1].Text = "□ 유    ■ 무 ";
            }


            for (i = 0; i < ssAGREE_Sheet1.RowCount; i++)
            {
                ss10_Sheet1.Cells[54, 1].Text += ssAGREE_Sheet1.Cells[i, 1].Text + ComNum.VBLF;

            }

        }

        void Set_Spread_Clear()
        {
            int i = 0;
            int k = 0;
            ss10_Sheet1.Cells[2, 1].Text = "";
            ss10_Sheet1.Cells[2, 8].Text = "";

            ss10_Sheet1.Cells[3, 1].Text = "";
            ss10_Sheet1.Cells[4, 1].Text = "";

            ss10_Sheet1.Cells[5, 1].Text = "";


            for (i = 0; i < 20; i++)
            {

                ss10_Sheet1.Cells[i + 7, 2].Text = "";
                ss10_Sheet1.Cells[i + 7, 3].Text = "";

            }
            for (i = 0; i < 20; i++)
            {

                ss10_Sheet1.Cells[i + 7, 5].Text = "";
                ss10_Sheet1.Cells[i + 7, 6].Text = "";

            }
            ss10_Sheet1.Cells[27, 2].Text = "";


            ss10_Sheet1.Cells[28, 2].Text = "";


            for (i = 0; i < 15; i++)
            {
                ss10_Sheet1.Cells[i + 29, 1].Text = "";
            }

            for (i = 0; i < 15; i++)
            {
                ss10_Sheet1.Cells[i + 29, 5].Text = "";
            }


            ss10_Sheet1.Cells[44, 1].Text = "";

            ss10_Sheet1.Cells[45, 1].Text = "";


            for (i = 0; i < 6; i++)
            {
                ss10_Sheet1.Cells[i + 47, 1].Text = "";
                ss10_Sheet1.Cells[i + 47, 3].Text = "";
                ss10_Sheet1.Cells[i + 47, 7].Text = "";
                ss10_Sheet1.Cells[i + 47, 9].Text = "";

            }
            ss10_Sheet1.Cells[53, 1].Text = "";

            ss10_Sheet1.Cells[54, 1].Text = "";

        }

        private void frmOcsCpTong_2_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }
    }
}
