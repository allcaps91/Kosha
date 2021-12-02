using ComBase;
using ComLibB;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmPickUp : Form, MainFormMessage
    {
        ContextMenu PopupMenu = null;  //  폼 class 에 넣기

        frmPickupR frmPickupRX = null;
        string[] mstrPickupR = null;

        string FstrPtno = "";
        string FstrInDate = "";

        int mintRow1 = 0;
        //int mintCol1 = 0;

        bool mbolOPMAIN = false;
        string mstrWardCode = "";

        bool bolLoad = false;

        string[] mstrIpdno = null;

        #region MainFormMessage InterFace

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

        #endregion

        public frmPickUp()
        {
            InitializeComponent();
        }

        public frmPickUp(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        public frmPickUp(string strWardCode, bool bolOPMAIN)
        {
            InitializeComponent();

            mbolOPMAIN = bolOPMAIN;
            mstrWardCode = strWardCode;  
        }

        private void frmPickUp_Load(object sender, EventArgs e)
        {

            bolLoad = true;

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            // 임시 사용
            pSubFormToControl(frmPatientInfoX, panPwInfo);

            conPatInfo1.Height = 81;

            lblMsg.BackColor = Color.FromArgb(0, 0, 192);
            lblMsg.Text = "";
            lblBSA.Text = "";

            chkERPICKUP.Visible = false;

            ssView1_Sheet1.Columns.Get(17, 18).Visible = false; //'오더 no //row id
            ssView1_Sheet1.Columns.Get(23, 24).Visible = false; //'suga //'prn

            ssView2_Sheet1.Columns.Get(19, 20).Visible = false; //'오더 no //'row id 
            ssView2_Sheet1.Columns.Get(24).Visible = false; //'suga

            ssViewM_Sheet1.Columns.Get(20, 21).Visible = false;

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpFDate.Value = dtpDate.Value.AddDays(-60);
            dtpTDate.Value = dtpDate.Value;

            if (mstrWardCode == "" && VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                mstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            cboWard_SET();

            if (ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun)) == true || clsType.User.JobGroup == "JOB018005" || 
                cboWard.Text.Trim() == "OP" || cboWard.Text.Trim() == "AG" || clsType.User.JobGroup == "JOB013007" || clsType.User.JobGroup == "JOB013047")
            {
                cboWard.Enabled = true;
            }

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.Items.Add("지정");
            cboTeam.SelectedIndex = 0;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원자명단");
            cboJob.Items.Add("2.당일입원자");
            cboJob.Items.Add("3.퇴원예고자");
            cboJob.Items.Add("4.당일퇴원자");
            cboJob.Items.Add("5.중증도미분류");
            cboJob.Items.Add("6.수술예정자");
            cboJob.Items.Add("7.진단명 누락자");
            cboJob.Items.Add("A.응급실경유입원(1-3일전)");
            cboJob.Items.Add("B.재원기간 7-14일 환자");
            cboJob.Items.Add("C.재원기간 3-7일 환자");
            cboJob.Items.Add("D.어제퇴원자");
            cboJob.SelectedIndex = 0;

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체");
            cboDrCode.SelectedIndex = 0;

            //2018-11-23 안정수, 내시경실에서 사용시 병동세팅 되도록 추가함
            if (clsType.User.JobGroup == "JOB006001" || clsType.User.JobGroup == "JOB006002" || clsType.User.JobGroup == "JOB013002" || clsType.User.JobGroup == "JOB013055")
            {
                cboWard.Text = "ENDO";
            }

            bolLoad = false;

            btnSearch_Click(null, null);

            txtRemark.Text = "";
            txtRemarkM.Text = "";

        }

        private void SCREEN_CLEAR()
        {
            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            panActing.Enabled = false;
            FstrPtno = "";
        }

        private void cboWard_SET()
        {
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            chkERPICKUP.Visible = false;
            cboWard.Items.Clear();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }

                    cboWard.Items.Add("SICU");
                    cboWard.Items.Add("MICU");
                    cboWard.Items.Add("HD");
                    cboWard.Items.Add("OP");
                    cboWard.Items.Add("AG");
                    cboWard.Items.Add("ER");
                    cboWard.Items.Add("ENDO");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            ComFunc.ComboFind(cboWard, "L", 10, mstrWardCode);

            if (cboWard.SelectedIndex != -1 && cboWard.SelectedIndex != 0)
            {
                cboWard.Enabled = false;
            }

            if (clsType.User.DeptCode == "ER")
            {
                chkERPICKUP.Visible = true;
                cboWard.Enabled = true;
            }

        }

        private void Read_Orders_PICKUP_New(string argPTNO, string argBDate)
        {
            //DATA_READ:
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView2_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE1, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.DRORDERVIEW,'YYYY-MM-DD HH24:MI') DRORDERVIEW1, ";
                SQL = SQL + ComNum.VBLF + "       A.ROWID, ";
                SQL = SQL + ComNum.VBLF + " (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST B WHERE A.DRCODE =  B.SABUN) DRNAME, ";
                //SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN2) PICKUPNAME";
                SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN) PICKUPNAME";
                SQL = SQL + ComNum.VBLF + " , (SELECT  MAX(X.ROWID) FROM KOSMOS_OCS.OCS_IORDER  X";
                SQL = SQL + ComNum.VBLF + " WHERE X.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND X.BDATE      = TO_DATE('" + Convert.ToDateTime(argBDate).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND X.GBSTATUS  IN  (' ','D+','D','D-') ";
                SQL = SQL + ComNum.VBLF + "    AND X.ORDERCODE = A.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "    AND X.QTY = A.QTY ";
                SQL = SQL + ComNum.VBLF + "    AND X.REALQTY = A.REALQTY ";
                SQL = SQL + ComNum.VBLF + "    AND X.CONTENTS = A.CONTENTS ";
                SQL = SQL + ComNum.VBLF + "    AND X.REMARK = A.REMARK ";
                
                if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND X.GBIOE IN ('E','EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( X.GBIOE NOT IN ('E','EI') OR X.GBIOE IS NULL) ";
                }
                SQL = SQL + ComNum.VBLF + " ) AS COMPARE_RTN , ";

                SQL = SQL + ComNum.VBLF + "    O.ORDERCODE AS O_ORDERCODE, O.DISPHEADER CDISPHEADER, O.ORDERNAME CORDERNAME, O.DISPRGB CDISPRGB,  ";
                SQL = SQL + ComNum.VBLF + "    O.GBBOTH CGBBOTH, O.GBINFO CGBINFO,  ";
                SQL = SQL + ComNum.VBLF + "    O.GBQTY  CGBQTY, O.GBDOSAGE CGBDOSAGE, O.NEXTCODE CNEXTCODE,  ";
                SQL = SQL + ComNum.VBLF + "    O.ORDERNAMES CORDERNAMES, O.GBIMIV CGBIMIV, B.SUNAMEK, O.BUN AS O_BUN  ";
                SQL = SQL + ComNum.VBLF + "    , (SELECT MAX(X.JEPCODE)  ";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_ADM.DRUG_SETCODE X ";
                SQL = SQL + ComNum.VBLF + "        WHERE X.GUBUN = '13'   ";
                SQL = SQL + ComNum.VBLF + "            AND (X.DELDATE IS NULL or X.DelDate ='')     ";
                SQL = SQL + ComNum.VBLF + "            AND TRIM(X.JepCode) = TRIM(O.ORDERCODE)) AS ANTIBLOOD  ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT COUNT(X.GRPNAME) CNT  ";
                SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT X ";
                SQL = SQL + ComNum.VBLF + "            WHERE X.GRPNAME in (SELECT X1.GRPNAME FROM KOSMOS_OCS.OCS_DRUGINFO_COMPONENT X1 WHERE TRIM(X1.SUNEXT) = TRIM(O.ORDERCODE))  ";
                SQL = SQL + ComNum.VBLF + "            HAVING COUNT(X.GRPNAME) > 0) AS COMPONENT    ";
                SQL = SQL + ComNum.VBLF + "     , (SELECT X.SEQNO  ";
                SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_ADM.DRUG_SPECIAL_JEPCODE X ";
                SQL = SQL + ComNum.VBLF + "            WHERE X.SEQNO = 7  ";
                SQL = SQL + ComNum.VBLF + "                AND TRIM(JEPCODE) = TRIM(O.ORDERCODE)) AS JEPCODE7   ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER A   ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER  JOIN  KOSMOS_OCS.OCS_ORDERCODE O ";
                SQL = SQL + ComNum.VBLF + "    ON A.ORDERCODE = O.ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "    AND A.SLIPNO = O.SLIPNO ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER  JOIN KOSMOS_PMPA.BAS_SUN B  ";
                SQL = SQL + ComNum.VBLF + "    ON O.SUCODE = B.SUNEXT     ";

                SQL = SQL + ComNum.VBLF + " WHERE A.PTNO     = '" + argPTNO + "'  ";

                if (cboWard.Text.Trim() == "ER")
                {
                    if (chkERPICKUP.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE      >= TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE      <= TO_DATE('" + Convert.ToDateTime(argBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE      = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE      = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.GBPICKUP = '*' ";
                SQL = SQL + ComNum.VBLF + "    AND ( A.ORDERSITE NOT IN ('CAN','NDC') OR A.ORDERSITE IS NULL) ";

                if (cboWard.Text.Trim() == "HD")
                {

                }
                else if (cboWard.Text.Trim() == "ENDO")
                {
                    SQL = SQL + ComNum.VBLF + "AND ( (A.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ('EN')) OR (TRIM(A.PICKUPSABUN) IN ( SELECT TRIM(IDNUMBER) FROM KOSMOS_PMPA.BAS_PASS WHERE PROGRAMID ='ENDO' AND IDNUMBER >0  )) )    ) ";
                }
                else if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.GBIOE IN ('E','EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( A.GBIOE NOT IN ('E','EI') OR A.GBIOE IS NULL) ";
                }

                if (chkNC.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.SLIPNO NOT IN ('A7') ";
                }

                //''    2016 - 07 - 13 계장 김현욱 구두처방 의사 DC는 픽업 받도록 변경
                if (chkVerval.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND (((A.GBVERB IS NULL OR A.GBVERB <> 'Y') AND A.GBSTATUS IN (' ', 'D+', 'D', 'D-'))  OR";
                    SQL = SQL + ComNum.VBLF + "      ((A.GBVERB IS NOT NULL OR A.GBVERB = 'Y') AND A.GBSTATUS IN ('D-')))";
                }
                else
                {
                    //''    '========================================================================================
                    //''    '구두확정오더제외 2015-09-02
                    SQL = SQL + ComNum.VBLF + "   AND ( A.GBVERB IS NULL OR A.GBVERB <>'Y' ) ";  //'2015-11-22 간호구두처방 제외;
                    SQL = SQL + ComNum.VBLF + "   AND A.GBSTATUS  IN  (' ','D+','D','D-') ";
                    //''    '========================================================================================
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY  A.ORDERNO , A.SEQNO, A.ENTDATE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Move_Orders_PICKUP(i, dt, argPTNO);

                        if (dt.Rows[i]["COMPARE_RTN"].ToString().Trim() != "")
                        {
                            ssView2_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 200, 200);
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Read_Orders_PICKUP(string argPTNO, string argBDate)
        {
            //DATA_READ:
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView2_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE1, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.DRORDERVIEW,'YYYY-MM-DD HH24:MI') DRORDERVIEW1, ";
                SQL = SQL + ComNum.VBLF + "       A.ROWID, ";
                SQL = SQL + ComNum.VBLF + " (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST B WHERE A.DRCODE =  B.SABUN) DRNAME, ";
                //SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN2) PICKUPNAME";
                SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN) PICKUPNAME";
                SQL = SQL + ComNum.VBLF + " , A.VER ";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_OCS.OCS_IORDER A ";       //'당일 처방 ORDER SEARCH;
                SQL = SQL + ComNum.VBLF + " WHERE PTNO     = '" + argPTNO + "'  ";


                if (cboWard.Text.Trim() == "ER")
                {
                    if (chkERPICKUP.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND BDATE      >= TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE      <= TO_DATE('" + Convert.ToDateTime(argBDate).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "   AND BDATE      = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND BDATE      = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "   AND GBPICKUP = '*' ";
                SQL = SQL + ComNum.VBLF + "    AND ( ORDERSITE NOT IN ('CAN','NDC') OR ORDERSITE IS NULL) ";

                if (cboWard.Text.Trim() == "HD")
                {

                }
                else if (cboWard.Text.Trim() == "ENDO")
                {
                    SQL = SQL + ComNum.VBLF + "AND ( (A.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ('EN')) OR (TRIM(A.PICKUPSABUN) IN ( SELECT TRIM(IDNUMBER) FROM KOSMOS_PMPA.BAS_PASS WHERE PROGRAMID ='ENDO' AND IDNUMBER >0  )) )    ) ";
                }
                else if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('E','EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
                }

                if (chkNC.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND SLIPNO NOT IN ('A7') ";
                }

                //''    2016 - 07 - 13 계장 김현욱 구두처방 의사 DC는 픽업 받도록 변경
                if (chkVerval.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND (((GBVERB IS NULL OR GBVERB <> 'Y') AND GBSTATUS IN (' ', 'D+', 'D', 'D-'))  OR";
                    SQL = SQL + ComNum.VBLF + "      ((GBVERB IS NOT NULL OR GBVERB = 'Y') AND GBSTATUS IN ('D-')))";
                }
                else
                {
                    //''    '========================================================================================
                    //''    '구두확정오더제외 2015-09-02
                    SQL = SQL + "   AND ( GBVERB IS NULL OR GBVERB <>'Y' ) ";  //'2015-11-22 간호구두처방 제외;
                    SQL = SQL + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";
                    //''    '========================================================================================
                }

                SQL = SQL + " ORDER BY  ORDERNO , SEQNO, ENTDATE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Move_Orders_PICKUP(i, dt, argPTNO);
                        COMPARE_RTN(ssView2_Sheet1, i, dt, argPTNO, argBDate);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void COMPARE_RTN(FarPoint.Win.Spread.SheetView argView, int intRow, DataTable dt, string argPTNO, string argBDate)
        {
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT  ROWID FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE      = TO_DATE('" + Convert.ToDateTime(argBDate).AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND ORDERCODE = '" + dt.Rows[intRow]["ORDERCODE"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND QTY = '" + dt.Rows[intRow]["QTY"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND REALQTY = '" + dt.Rows[intRow]["REALQTY"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CONTENTS = '" + dt.Rows[intRow]["CONTENTS"].ToString().Trim() + "'  ";

                if (dt.Rows[intRow]["REMARK"].ToString().Trim() == "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND ( REMARK = '" + dt.Rows[intRow]["REMARK"].ToString().Trim() + "'  OR REMARK IS NULL ) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND REMARK = '" + dt.Rows[intRow]["REMARK"].ToString().Trim() + "' ";
                }

                if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('E','EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    argView.Rows.Get(intRow).BackColor = Color.FromArgb(255, 200, 200);
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Move_Orders_PICKUP(int intRow, DataTable dt, string argPTNO)
        {

            if (dt.Rows[intRow]["TUYEOPOINT"].ToString().Trim() != "" && btnCancerWorklist.Visible != true)
            {
                btnCancerWorklist.Visible = true;
            }

            switch (dt.Rows[intRow]["GBSEND"].ToString().Trim())
            {
                case "*":
                    ssView2_Sheet1.Cells[intRow, 0].Text = "미전송";
                    break;
                case "Z":
                    ssView2_Sheet1.Cells[intRow, 0].Text = "전송중";
                    break;
                default:
                    ssView2_Sheet1.Cells[intRow, 0].Text = "";
                    break;
            }

            //'2013-03-20
            if (dt.Rows[intRow]["ORDERSITE"].ToString().Trim() == "OPDX")
            {
                ssView2_Sheet1.Cells[intRow, 0].Text = "★외래오더★";
            }

            if (dt.Rows[intRow]["AIRSHT"].ToString().Trim() == "1")
            {
                ssView2_Sheet1.Cells[intRow, 0].Text = ssView2_Sheet1.Cells[intRow, 0].Text.Trim() + "(긴급약)";
            }

            ssView2_Sheet1.Cells[intRow, 1].Text = dt.Rows[intRow]["GBSTATUS"].ToString().Trim();


            if (dt.Rows[intRow]["GBINFO"].ToString().Trim() == "CONSULT")
            {
                ssView2_Sheet1.Cells[intRow, 2].Text = "C/O";
            }
            else
            {
                ssView2_Sheet1.Cells[intRow, 2].Text = ComFunc.LeftH(clsIpdNr.SlipNo_Gubun(dt.Rows[intRow]["SLIPNO"].ToString().Trim(),
                 ComFunc.LeftH(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), 2), dt.Rows[intRow]["BUN"].ToString().Trim()), 7);
                ;
            }

            switch (dt.Rows[intRow]["GBORDER"].ToString().Trim())
            {
                case "F":
                    ssView2_Sheet1.Cells[intRow, 2].Text = "Pre";
                    break;
                case "T":
                    ssView2_Sheet1.Cells[intRow, 2].Text = "Post";
                    break;
                case "M":
                    ssView2_Sheet1.Cells[intRow, 2].Text = "Adm";
                    break;
            }

            ssView2_Sheet1.Cells[intRow, 3].Text = dt.Rows[intRow]["ORDERCODE"].ToString().Trim();
            ssView2_Sheet1.Cells[intRow, 26].Text = dt.Rows[intRow]["ORDERCODE"].ToString().Trim();

            Order_Read_Move_PICKUP(intRow, dt);

            if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                    && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
            {
                ssView2_Sheet1.Cells[intRow, 1, intRow, ssView2_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
            }

            if ((String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "11") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "20") <= 0)
            || String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "23") == 0)
            {
                if (dt.Rows[intRow]["CONTENTS"].ToString().Trim().IndexOf(".") == -1)
                {
                    ssView2_Sheet1.Cells[intRow, 6].Text = (dt.Rows[intRow]["CONTENTS"].ToString().Trim() == "0" ? " " : dt.Rows[intRow]["CONTENTS"].ToString().Trim());
                }
                else
                {
                    ssView2_Sheet1.Cells[intRow, 6].Text = VB.Val(dt.Rows[intRow]["CONTENTS"].ToString().Trim()).ToString("###0.##");
                }

            }

            ssView2_Sheet1.Cells[intRow, 7].Text = dt.Rows[intRow]["REALQTY"].ToString().Trim();

            if (VB.Val(dt.Rows[intRow]["GBDIV"].ToString().Trim()) == 0)
            {
                ssView2_Sheet1.Cells[intRow, 8].Text = "";
            }
            else
            {
                ssView2_Sheet1.Cells[intRow, 8].Text = dt.Rows[intRow]["GBDIV"].ToString().Trim();
            }

            ssView2_Sheet1.Cells[intRow, 9].Text = dt.Rows[intRow]["NAL"].ToString().Trim();

            ssView2_Sheet1.Cells[intRow, 10].Text = (dt.Rows[intRow]["POWDER"].ToString().Trim() == "1" ? "◎" : "");


            if (String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "16") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "21") <= 0)
            {
                if (dt.Rows[intRow]["GBNGT"].ToString().Trim() != "")
                {
                    ssView2_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBNGT"].ToString().Trim();
                }

                if (dt.Rows[intRow]["GBGROUP"].ToString().Trim() != "")
                {
                    ssView2_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
                }
            }
            else if (String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "28") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "39") <= 0)
            {
                if (VB.IsNumeric(dt.Rows[intRow]["GBGROUP"].ToString().Trim()) == true
                || dt.Rows[intRow]["GBGROUP"].ToString().Trim() == "")
                {
                    ssView2_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBNGT"].ToString().Trim();
                }
                else
                {
                    ssView2_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
                }
            }
            else
            {
                ssView2_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
            }

            ssView2_Sheet1.Cells[intRow, 12].Text = dt.Rows[intRow]["GBER"].ToString().Trim();

            ssView2_Sheet1.Cells[intRow, 13].Text = dt.Rows[intRow]["GBSELF"].ToString().Trim();

            if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                    && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
            {

            }
            else
            {
                if (dt.Rows[intRow]["REMARK"].ToString().Trim() != "")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "#";
                }

                if (dt.Rows[intRow]["GBPRN"].ToString().Trim() != "")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = dt.Rows[intRow]["GBPRN"].ToString().Trim();
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "T")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "T";
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "O")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "O";
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "A")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "A";
                }

                if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "S" || ssView2_Sheet1.Cells[intRow, 14].Text == "S")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "";
                    //ssView2_Sheet1.Cells[intRow, 4].Text = "(선)";
                    ssView2_Sheet1.Cells[intRow, 4].Text = "(선수납)";
                }
                else if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "B")
                {
                    ssView2_Sheet1.Cells[intRow, 14].Text = "";
                    //ssView2_Sheet1.Cells[intRow, 4].Text = "(수)";
                    ssView2_Sheet1.Cells[intRow, 4].Text = "(수납완료)";
                }

                if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "A")
                {
                    //ssView2_Sheet1.Cells[intRow, 4].Text = "(A)" + ssView2_Sheet1.Cells[intRow, 4].Text.Trim();
                    ssView2_Sheet1.Cells[intRow, 4].Text = "(선수납)" + ssView2_Sheet1.Cells[intRow, 4].Text.Trim();
                }
            }

            if (dt.Rows[intRow]["VER"].ToString().Trim() == "CPORDER" && ssView2_Sheet1.Cells[intRow, 4].Text.IndexOf("[CP]") == -1)
            {
                ssView2_Sheet1.Cells[intRow, 4].Text = "[CP]" + ssView2_Sheet1.Cells[intRow, 4].Text.Trim();
            }

            if (dt.Rows[intRow]["GBPORT"].ToString().Trim() == "M")
            {
                ssView2_Sheet1.Cells[intRow, 15].Text = "M";
            }

            //'2015-09-02 구두의사확정
            if (dt.Rows[intRow]["VERBC"].ToString().Trim() == "C")
            {
                ssView2_Sheet1.Cells[intRow, 16].BackColor = Color.FromArgb(0, 255, 0);
            }

            //2019-06-25 분류가 49는 초음파로 변경 됨.
            //if (dt.Rows[intRow]["BUN"].ToString().Trim() == "48"            || dt.Rows[intRow]["BUN"].ToString().Trim() == "49")
            if (dt.Rows[intRow]["BUN"].ToString().Trim() == "48" )
            {
                ssView2_Sheet1.Cells[intRow, 16].Text = ssView2_Sheet1.Cells[intRow, 16].Text.Trim()
                + READ_ENDO_REMARK(dt.Rows[intRow]["ORDERNO"].ToString().Trim(), argPTNO);
            }
            else
            {
                ssView2_Sheet1.Cells[intRow, 16].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
            }

            if (dt.Rows[intRow]["ASA"].ToString().Trim() != "")
            {
                ssView2_Sheet1.Cells[intRow, 16].Text = "ASA : " + dt.Rows[intRow]["ASA"].ToString().Trim()
                + " " + ComNum.VBLF + ssView2_Sheet1.Cells[intRow, 16].Text.Trim();
            }

            //'2015-09-02

            if (dt.Rows[intRow]["PRN_REMARK"].ToString().Trim() != "")
            {
                ssView2_Sheet1.Cells[intRow, 16].Text = dt.Rows[intRow]["PRN_REMARK"].ToString().Trim();
                ssView2_Sheet1.Cells[intRow, 25].Text = "#";
            }

            ssView2_Sheet1.Cells[intRow, 17].Text = dt.Rows[intRow]["PICKUPNAME"].ToString().Trim();
            ssView2_Sheet1.Cells[intRow, 18].Text = dt.Rows[intRow]["PICKUPDATE1"].ToString().Trim();

            ssView2_Sheet1.Cells[intRow, 19].Text = dt.Rows[intRow]["ORDERNO"].ToString().Trim();
            ssView2_Sheet1.Cells[intRow, 20].Text = dt.Rows[intRow]["ROWID"].ToString().Trim();

            //'처방의
            ssView2_Sheet1.Cells[intRow, 21].Text = "";
            //'처방시간 2015-10-27
            if (dt.Rows[intRow]["GBVERB"].ToString().Trim() == "Y")
            {
                if (dt.Rows[intRow]["VERBC"].ToString().Trim() == "C")
                {
                    ssView2_Sheet1.Cells[intRow, 21].Text = dt.Rows[intRow]["DRNAME"].ToString().Trim();
                    ssView2_Sheet1.Cells[intRow, 22].Text = dt.Rows[intRow]["DRORDERVIEW1"].ToString().Trim();
                }
            }
            else
            {
                ssView2_Sheet1.Cells[intRow, 21].Text = dt.Rows[intRow]["DRNAME"].ToString().Trim();
                ssView2_Sheet1.Cells[intRow, 22].Text = dt.Rows[intRow]["ENTDATE1"].ToString().Trim();
            }

            ssView2_Sheet1.Cells[intRow, 23].Text = dt.Rows[intRow]["PICKUPREMARK"].ToString().Trim();
            ssView2_Sheet1.Cells[intRow, 24].Text = dt.Rows[intRow]["SUCODE"].ToString().Trim();

            ssView2_Sheet1.Cells[intRow, 27].Text = clsBagage.READ_ORDER_SUBUL_BUSE(clsDB.DbCon, dt.Rows[intRow]["SUBUL_WARD"].ToString().Trim(), "");
        }

        private string READ_ENDO_REMARK(string argOrderNo, string argPTNO)
        {
            string RtnValue = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT REMARKC, REMARKD ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_REMARK B";
                SQL = SQL + ComNum.VBLF + " WHERE A.JDATE = A.JDATE";
                SQL = SQL + ComNum.VBLF + "      AND A.PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "      AND A.ORDERNO = " + argOrderNo;
                SQL = SQL + ComNum.VBLF + "      AND A.ORDERCODE = B.ORDERCODE";
                SQL = SQL + ComNum.VBLF + "      AND A.PTNO = B.PTNO";
                SQL = SQL + ComNum.VBLF + "      AND ROWNUM <= 1";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnValue;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnValue = "증상 : " + dt.Rows[0]["REMARKC"].ToString().Trim() + ComNum.VBLF + "진단 : " + dt.Rows[0]["REMARKD"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }




            return RtnValue;
        }

        /// <summary>
        ///  Read_Orders_PICKUP의 GoSub Order_Read, GoSub Order_Move 합침
        /// </summary>
        /// <param name="intRow"></param>
        /// <param name="dt"></param>
        private void Order_Read_Move_PICKUP(int intRow, DataTable dt)
        {
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = " SELECT    DISPHEADER CDISPHEADER, ORDERNAME CORDERNAME, DISPRGB CDISPRGB, ";
                SQL = SQL + " GBBOTH CGBBOTH, GBINFO CGBINFO, ";
                SQL = SQL + " GBQTY  CGBQTY, GBDOSAGE CGBDOSAGE, NEXTCODE CNEXTCODE, ";
                SQL = SQL + " ORDERNAMES CORDERNAMES, GBIMIV CGBIMIV, B.SUNAMEK, A.BUN  ";
                SQL = SQL + " FROM KOSMOS_OCS.OCS_ORDERCODE A, KOSMOS_PMPA.BAS_SUN B ";
                SQL = SQL + "WHERE A.ORDERCODE = '" + dt.Rows[intRow]["ORDERCODE"].ToString().Trim() + "' ";
                SQL = SQL + "  AND A.SLIPNO    = '" + dt.Rows[intRow]["SLIPNO"].ToString().Trim() + "'          ";
                SQL = SQL + "  AND A.SUCODE = B.SUNEXT ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {

                    switch (dt1.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "20":
                        case "23":
                            if (dt1.Rows[0]["SUNAMEK"].ToString().Trim().IndexOf("자가약") != -1)
                            {
                                ssView2_Sheet1.Cells[intRow, 4].Text = ssView2_Sheet1.Cells[intRow, 4].Text.Trim()
                                + dt1.Rows[0]["CORDERNAME"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else
                            {
                                ssView2_Sheet1.Cells[intRow, 4].Text = dt1.Rows[0]["SUNAMEK"].ToString().Trim()
                                + clsIpdNr.GetDrugInfoSnaem(clsDB.DbCon, dt.Rows[intRow]["ORDERCODE"].ToString().Trim());
                            }
                            break;
                        default:
                            if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                            {
                                ssView2_Sheet1.Cells[intRow, 4].Text = ssView2_Sheet1.Cells[intRow, 4].Text.Trim()
                                + dt1.Rows[0]["CORDERNAME"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                            {
                                ssView2_Sheet1.Cells[intRow, 4].Text = ssView2_Sheet1.Cells[intRow, 4].Text
                                + dt1.Rows[0]["CDISPHEADER"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssView2_Sheet1.Cells[intRow, 4].Text = ssView2_Sheet1.Cells[intRow, 4].Text
                                 + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }
                            break;

                    }

                    ssView2_Sheet1.Cells[intRow, 4].Text = clsIpdNr.READ_ATTENTION(clsDB.DbCon, dt.Rows[intRow]["ORDERCODE"].ToString().Trim())
                                                            + ssView2_Sheet1.Cells[intRow, 4].Text.Trim();

                    if (dt.Rows[intRow]["SLIPNO"].ToString().Trim() == "A4")
                    {
                        ssView2_Sheet1.Cells[intRow, 4].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }

                    if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[intRow, 5].Text = dt.Rows[intRow]["GBINFO"].ToString().Trim();
                    }
                    else if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1") //    '용법
                    {
                        ssView2_Sheet1.Cells[intRow, 5].Text = Read_Dosage(dt.Rows[intRow]["DOSCODE"].ToString().Trim());
                    }
                    else
                    {
                        ssView2_Sheet1.Cells[intRow, 5].Text =
                        Read_Specman(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), dt.Rows[intRow]["SLIPNO"].ToString().Trim());
                    }

                    if (dt1.Rows[0]["CGBBOTH"].ToString().Trim() == "1")
                    {
                        ssView2_Sheet1.Cells[intRow, 4].Text =
                        VB.Left(ComFunc.RPAD(ssView2_Sheet1.Cells[intRow, 4].Text.Trim(), 30, " "), 30) + dt.Rows[intRow]["GBINFO"].ToString().Trim();
                    }

                    ssView2_Sheet1.Cells[intRow, 1, intRow, ssView2_Sheet1.ColumnCount - 1].ForeColor =
                     ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
                else
                {
                    if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                    && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                    {
                        ssView2_Sheet1.Cells[intRow, 4].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;

                //'손동현 선수납 항목 체크
                SQL = " SELECT    SUGBN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN  ";
                SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + dt.Rows[intRow]["SUCODE"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUGBN  = '1' ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssView2_Sheet1.Cells[intRow, 4].Text = "(A)" + ssView2_Sheet1.Cells[intRow, 4].Text.Trim();
                }

                dt1.Dispose();
                dt1 = null;

            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string Read_Specman(string strDosCode, string strSlipno)
        {
            string RtnValue = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT SPECNAME FROM KOSMOS_OCS.OCS_OSPECIMAN ";
                SQL = SQL + "WHERE SPECCODE = '" + strDosCode + "' ";
                SQL = SQL + "  AND SLIPNO   = '" + strSlipno + "'   ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnValue;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnValue = dt.Rows[0]["SPECNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnValue;
        }

        private string Read_Dosage(string strDosCode)
        {
            string RtnValue = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT DOSNAME FROM KOSMOS_OCS.OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "WHERE DOSCODE = '" + strDosCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnValue;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnValue = dt.Rows[0]["DOSNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnValue;
        }

        private void Read_Orders_Multi()
        {
            //'당일 이전 해당병동의 미pickup 오더 조회

            int i = 0;

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strPtNo = "";

            txtRemark.Text = "";

            //'당일이전 처방 Read
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssViewM_Sheet1.RowCount = 0;


            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, ";
                SQL = SQL + ComNum.VBLF + "B.SNAME, B.GBSTS , A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_PMPA.IPD_NEW_MASTER B ";      //'당일 처방 ORDER SEARCH;
                SQL = SQL + ComNum.VBLF + "WHERE B.WARDCODE     = '" + cboWard.Text.Trim() + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";
                SQL = SQL + ComNum.VBLF + "   AND (GBPICKUP ='' OR GBPICKUP IS NULL) ";
                SQL = SQL + ComNum.VBLF + "   AND GBSEND ='*' ";
                SQL = SQL + ComNum.VBLF + "   AND ACCSEND IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "   AND B.ACTDATE IS NULL ";

                if (cboWard.Text.Trim() == "HD")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'HD') ) ";
                }
                else if (cboWard.Text.Trim() == "ENDO")
                {
                    SQL = SQL + ComNum.VBLF + "AND A.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ";
                }
                else if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('E','EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( GBIOE NOT IN ('E','EI')  OR GBIOE IS NULL) ";
                }

                SQL = SQL + ComNum.VBLF + "    AND A.BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND A.BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                SQL = SQL + ComNum.VBLF + "   AND ( A.GBVERB IS NULL OR A.GBVERB <>'Y' ) ";  //'2015-11-22 간호구두처방 제외;

                if (chkNC.Checked == true)
                {
                    //SQL = SQL + ComNum.VBLF + "  AND GBSLIP NOT IN ('A7') ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY A.ROOMCODE,  A.PTNO, A.BDATE,   A.ORDERNO , A.SEQNO, A.ENTDATE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }



                if (dt.Rows.Count > 0)
                {
                    ssViewM_Sheet1.RowCount = dt.Rows.Count;
                    ssViewM_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssViewM_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssViewM_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssViewM_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssViewM_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssViewM_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBSTATUS"].ToString().Trim();

                        if (dt.Rows[i]["GBINFO"].ToString().Trim() == "CONSULT")
                        {
                            ssViewM_Sheet1.Cells[i, 6].Text = "C/O";
                        }
                        else
                        {
                            ssViewM_Sheet1.Cells[i, 6].Text =
                            ComFunc.LeftH(
                            clsIpdNr.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim())
                            , 7);
                        }

                        switch (dt.Rows[i]["GBORDER"].ToString().Trim())
                        {
                            case "F":
                                ssViewM_Sheet1.Cells[i, 2].Text = "Pre";
                                break;
                            case "T":
                                ssViewM_Sheet1.Cells[i, 2].Text = "Post";
                                break;
                            case "M":
                                ssViewM_Sheet1.Cells[i, 2].Text = "Adm";
                                break;
                        }

                        ssViewM_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        Order_Read_Move_Multi(dt, i, strSysDate);

                        if (String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0
                            && String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                        {
                            ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                        }

                        if (VB.IsNumeric(dt.Rows[i]["BUN"].ToString().Trim()) == true)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) >= 11
                            && Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) <= 20
                            || Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) == 23)
                            {
                                if (dt.Rows[i]["CONTENTS"].ToString().Trim().IndexOf(".") == -1)
                                {
                                    ssViewM_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["CONTENTS"].ToString().Trim() == "0" ? " " : dt.Rows[i]["CONTENTS"].ToString().Trim());
                                }
                                else
                                {
                                    ssViewM_Sheet1.Cells[i, 10].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()).ToString("###0.##");
                                }
                            }
                        }

                        ssViewM_Sheet1.Cells[i, 11].Text = dt.Rows[i]["REALQTY"].ToString().Trim();

                        if (dt.Rows[i]["GBDIV"].ToString().Trim() == "0")
                        {
                            ssViewM_Sheet1.Cells[i, 12].Text = "";
                        }
                        else
                        {
                            ssViewM_Sheet1.Cells[i, 12].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        }

                        ssViewM_Sheet1.Cells[i, 13].Text = dt.Rows[i]["NAL"].ToString().Trim();

                        if (VB.IsNumeric(dt.Rows[i]["BUN"].ToString().Trim()) == true)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) >= 16
                            && Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) <= 21)
                            {
                                if (dt.Rows[i]["GBNGT"].ToString().Trim() != "")
                                {
                                    ssViewM_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                                }

                                if (dt.Rows[i]["GBGROUP"].ToString().Trim() != "")
                                {
                                    ssViewM_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                                }
                            }
                            else if (Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) >= 28
                            && Convert.ToInt32(dt.Rows[i]["BUN"].ToString().Trim()) <= 39)  //        '손동현 위를 아래로 한다.  //        '처치/재료는 GbNgt    나머지는 Group
                            {
                                if (VB.IsNumeric(dt.Rows[i]["GBGROUP"].ToString().Trim()) == true
                                || dt.Rows[i]["GBGROUP"].ToString().Trim() == "")
                                {
                                    ssViewM_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                                }
                                else
                                {
                                    ssViewM_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                                }

                            }
                            else
                            {
                                ssViewM_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }

                        ssViewM_Sheet1.Cells[i, 15].Text = dt.Rows[i]["GBER"].ToString().Trim();
                        ssViewM_Sheet1.Cells[i, 16].Text = dt.Rows[i]["GBSELF"].ToString().Trim();

                        if (String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0
                            && String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                        {

                        }
                        else
                        {
                            //    SSM.Col = 18
                            if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                            {
                                ssViewM_Sheet1.Cells[i, 17].Text = "#";
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() != "")
                            {
                                ssViewM_Sheet1.Cells[i, 17].Text = dt.Rows[i]["GBPRN"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBTFLAG"].ToString().Trim() == "T"
                            || dt.Rows[i]["GBTFLAG"].ToString().Trim() == "O"
                            || dt.Rows[i]["GBTFLAG"].ToString().Trim() == "A")
                            {
                                ssViewM_Sheet1.Cells[i, 17].Text = dt.Rows[i]["GBTFLAG"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "S"
                            || ssViewM_Sheet1.Cells[i, 17].Text == "S")
                            {
                                ssViewM_Sheet1.Cells[i, 17].Text = "";//'손동현 추가
                                //ssViewM_Sheet1.Cells[i, 8].Text = "(선)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                                ssViewM_Sheet1.Cells[i, 8].Text = "(선수납)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                            {
                                ssViewM_Sheet1.Cells[i, 17].Text = "";//'손동현 추가
                                //ssViewM_Sheet1.Cells[i, 8].Text = "(수)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                                ssViewM_Sheet1.Cells[i, 8].Text = "(수납완료)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                            {
                                //ssViewM_Sheet1.Cells[i, 8].Text = "(A)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                                ssViewM_Sheet1.Cells[i, 8].Text = "(선수납)" + ssViewM_Sheet1.Cells[i, 8].Text.Trim();
                            }
                        }

                        if (dt.Rows[i]["GBPORT"].ToString().Trim() == "M")
                        {
                            ssViewM_Sheet1.Cells[i, 18].Text = "M";
                        }

                        //'remark
                        ssViewM_Sheet1.Cells[i, 19].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                        //'orderno
                        ssViewM_Sheet1.Cells[i, 20].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();

                        //'rowid
                        ssViewM_Sheet1.Cells[i, 21].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        //'처방의
                        ssViewM_Sheet1.Cells[i, 22].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());

                        switch (dt.Rows[i]["GBSTS"].ToString().Trim())
                        {

                            case "1":
                                ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 100);
                                break;
                            case "2":
                                ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 100);
                                break;
                            case "4":

                                break;
                            case "5":
                                ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 255);
                                break;
                            case "6":
                                ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 100);
                                break;
                            case "7":
                                ssViewM_Sheet1.Cells[i, 0, i, ssViewM_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 100, 100);
                                break;
                        }

                        if (dt.Rows[i]["PTNO"].ToString().Trim() != strPtNo)
                        {
                            ssViewM_Sheet1.Rows.Get(i).Border = new FarPoint.Win.LineBorder(Color.Red, 1, false, true, false, false);
                            strPtNo = dt.Rows[i]["PTNO"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Order_Read_Move_Multi(DataTable dt, int intRow, string strSysDate)
        {
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT DISPHEADER CDISPHEADER, ORDERNAME CORDERNAME, DISPRGB CDISPRGB, ";
                SQL = SQL + "GBBOTH CGBBOTH, GBINFO CGBINFO, ";
                SQL = SQL + "GBQTY  CGBQTY, GBDOSAGE CGBDOSAGE, NEXTCODE CNEXTCODE, ";
                SQL = SQL + "ORDERNAMES CORDERNAMES, GBIMIV CGBIMIV  ";
                SQL = SQL + "FROM KOSMOS_OCS.OCS_ORDERCODE  ";
                SQL = SQL + "WHERE ORDERCODE = '" + dt.Rows[intRow]["ORDERCODE"].ToString().Trim() + "' ";
                SQL = SQL + "   AND SLIPNO    = '" + dt.Rows[intRow]["SLIPNO"].ToString().Trim() + "'          ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = ssViewM_Sheet1.Cells[intRow, 8].Text.Trim()
                        + dt1.Rows[0]["CORDERNAME"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                    }
                    else if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = ssViewM_Sheet1.Cells[intRow, 8].Text.Trim()
                        + dt1.Rows[0]["CDISPHEADER"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                    }
                    else
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = ssViewM_Sheet1.Cells[intRow, 8].Text.Trim() + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                    }

                    if (dt.Rows[intRow]["SLIPNO"].ToString().Trim() == "A4")
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }

                    if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")
                    {
                        ssViewM_Sheet1.Cells[intRow, 9].Text = dt.Rows[intRow]["GBINFO"].ToString().Trim();
                    }
                    else if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")  //'용법
                    {
                        ssViewM_Sheet1.Cells[intRow, 9].Text = Read_Dosage(dt.Rows[intRow]["DOSCODE"].ToString().Trim());
                    }
                    else
                    {
                        ssViewM_Sheet1.Cells[intRow, 9].Text =
                        Read_Specman(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), dt.Rows[intRow]["SLIPNO"].ToString().Trim());
                    }

                    //    '2013-09-02 제한항생제 감염내과 과장 요청 + 감염관리실 신청서 없이 픽업불가!!


                    if (Convert.ToDateTime(strSysDate) >= Convert.ToDateTime("2013-09-03")
                    && READ_제한항생제_신청확인(
                    dt.Rows[intRow]["PTNO"].ToString().Trim(),
                    dt.Rows[intRow]["BDATE"].ToString().Trim(),
                    dt.Rows[intRow]["SUCODE"].ToString().Trim()) != true)
                    {
                        ComFunc.MsgBox("제한항생제 [" + dt.Rows[intRow]["SUCODE"].ToString().Trim() + "] 승인신청서가 없거나,"
                            + ComNum.VBLF + ComNum.VBLF + "승인자가 승인불가 처리한 경우입니다.."
                            + ComNum.VBLF + ComNum.VBLF + "미승인,보류시 3일경과시 신청서를 재작성하셔야합니다.."
                            + ComNum.VBLF + ComNum.VBLF + "항생제 처방이 필요한 경우라면 주치의와 상의후 조정해 주세요"
                            + ComNum.VBLF + ComNum.VBLF + "문의사항 :감염내과(8224) 감염관리(8019) 전산실(8333)", "제한항생제");

                        //'Define cells as type STATIC
                        ssViewM_Sheet1.Cells[intRow, 0].LockBackColor = Color.Gray;
                        ssViewM_Sheet1.Cells[intRow, 0].Locked = true;
                        ssViewM_Sheet1.Cells[intRow, 0].Value = false;
                    }

                    if (dt1.Rows[0]["CGBBOTH"].ToString().Trim() == "1")
                    {
                        ssViewM_Sheet1.Cells[intRow, 9].Text
                        = VB.Left(ComFunc.RPAD(ssViewM_Sheet1.Cells[mintRow1, 9].Text.Trim(), 30, " "), 30) + dt.Rows[intRow]["GBINFO"].ToString().Trim();
                    }

                    ssViewM_Sheet1.Cells[intRow, 1, intRow, ssViewM_Sheet1.ColumnCount - 1].ForeColor =
                    ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));

                    dt1.Dispose();
                    dt1 = null;

                    //손동현 선수납 항목 체크

                    SQL = "SELECT    SUGBN ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN  ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + dt.Rows[intRow]["SUCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUGBN  = '1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = "(A)" + ssViewM_Sheet1.Cells[intRow, 8].Text.Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
                else
                {
                    if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                        && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                    {
                        ssViewM_Sheet1.Cells[intRow, 8].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool READ_제한항생제_신청확인(string ArgPano, string ArgBDate, string ArgSuCode)
        {

            bool RtnVal = false;
            bool bolOK = false;

            string strSDate = "";
            string strExDate = "";
            string strOKDate = "";
            string strStat = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                bolOK = false;

                RtnVal = true;

                SQL = "SELECT ROWID    ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_SUN        ";
                SQL = SQL + ComNum.VBLF + "WHERE GBANTI ='Y'        ";
                SQL = SQL + ComNum.VBLF + "     AND SUNEXT ='" + ArgSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    bolOK = true;
                }

                dt.Dispose();
                dt = null;


                if (bolOK == true && RtnVal == true)
                {
                    SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,STATE,TRUNC(SYSDATE - SDATE)   DAY ,";
                    SQL = SQL + "  TO_CHAR(OKDATE,'YYYY-MM-DD') OKDATE ,TO_CHAR(EXDATE,'YYYY-MM-DD') EXDATE";
                    SQL = SQL + " FROM KOSMOS_OCS.OCS_ANTI_MST ";
                    SQL = SQL + "  WHERE PANO ='" + ArgPano + "' ";
                    SQL = SQL + "   AND SUCODE ='" + ArgSuCode + "' ";
                    SQL = SQL + "   AND(EXDATE >= TO_DATE('"+ Convert.ToDateTime(ArgBDate).ToShortDateString() + "', 'YYYY-MM-DD') OR EXDATE IS NULL) ";
                    SQL = SQL + "                         AND(STATE IN('1', '2', '3')  OR STATE IS NULL) ";
                    SQL = SQL + " ORDER BY SDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return RtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strStat = dt.Rows[0]["STATE"].ToString().Trim();
                        strSDate = dt.Rows[0]["SDATE"].ToString().Trim();
                        strOKDate = dt.Rows[0]["OKDATE"].ToString().Trim();
                        strExDate = dt.Rows[0]["EXDATE"].ToString().Trim();

                        //'미처리 및 보류 5일기간
                        if (strStat == "" || strStat == "2")
                        {
                            if (VB.Val(dt.Rows[0]["DAY"].ToString().Trim()) > 4)
                            {
                                RtnVal = false;
                            }
                        }
                        else if (strStat == "1") //'승인
                        {
                            if (Convert.ToDateTime(strExDate) < Convert.ToDateTime(ArgBDate))
                            {
                                RtnVal = false;
                            }
                        }
                        else if (strStat == "3") //'불가
                        {
                            RtnVal = false;
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void Read_Orders(string argPTNO, string argBDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");


            txtRemark.Text = "";
            ssView1_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }


                SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE1,";
                SQL = SQL + "       A.ROWID, ";
                SQL = SQL + ComNum.VBLF + " (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST B WHERE A.DRCODE =  B.SABUN) DRNAME, ";
                SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN) PICKUPNAME, ";
                //SQL = SQL + ComNum.VBLF + "          (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST C WHERE A.PICKUPSABUN = C.SABUN2) PICKUPNAME, ";
                SQL = SQL + ComNum.VBLF + "          ( SELECT SUBSTR(CAUTION_STRING, 4, LENGTH(CAUTION_STRING)) CAUTION_STRING";
                SQL = SQL + ComNum.VBLF + "               FROM KOSMOS_OCS.OCS_DRUGINFO_NEW D ";
                SQL = SQL + ComNum.VBLF + "            WHERE A.SUCODE = D.SUNEXT AND ROWNUM=1 ) CAUTION_STRING, A.TUYEOPOINT ";  //'P.KEY 없어 한번씩 중복 생성 오류 방지;
                SQL = SQL + ComNum.VBLF + "           , A.VER ";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_OCS.OCS_IORDER A ";       //'당일 처방 ORDER SEARCH;
                SQL = SQL + ComNum.VBLF + "WHERE PTNO     = '" + argPTNO + "'  ";
                SQL = SQL + ComNum.VBLF + "   AND (GBPICKUP ='' OR GBPICKUP IS NULL) ";


                if (cboWard.Text.Trim() == "HD" || cboWard.Text.Trim() == "ENDO")
                {
                    SQL = SQL + ComNum.VBLF + "   AND BDATE    = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }
                else if (cboWard.Text.Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "   AND GBIOE IN ('E','EI') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSEND ='*' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACCSEND IS NULL ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND ( GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE    = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                }

                if (chkNC.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND SLIPNO NOT IN ('A7') ";

                }

                //'2016-07-13 계장 김현욱 구두처방 의사 D/C건은 픽업 받도록 함

                if (chkVerval.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND (((GBVERB IS NULL OR GBVERB <> 'Y') AND GBSTATUS IN (' ', 'D+', 'D', 'D-'))  OR";
                    SQL = SQL + ComNum.VBLF + "      ((GBVERB IS NOT NULL OR GBVERB = 'Y') AND GBSTATUS IN ('D-')))";
                }
                else
                {
                    //'======================================================================================
                    SQL = SQL + ComNum.VBLF + "   AND ( GBVERB IS NULL OR GBVERB <>'Y' ) ";  //'2015-11-22 간호구두처방 제외;
                    SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";
                    //'======================================================================================
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY  ORDERNO , SEQNO, ENTDATE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Move_Orders(dt, i, strSysDate, argPTNO); //Move_Orders_PICKUP 과 같은거 같음. 수정할 일 있을때 같이 하거나 하나로 만들어서 수정하기.
                        COMPARE_RTN(ssView1_Sheet1, i, dt, argPTNO, argBDate);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void Move_Orders(DataTable dt, int intRow, string strSysDate, string argPTNO)
        {
            ssView1_Sheet1.Cells[intRow, 1].Text = dt.Rows[intRow]["GBSTATUS"].ToString().Trim();

            if (dt.Rows[intRow]["TUYEOPOINT"].ToString().Trim() != "" && btnCancerWorklist.Visible != true)
            {
                btnCancerWorklist.Visible = true;
            }

            if (dt.Rows[intRow]["GBINFO"].ToString().Trim() == "CONSULT")
            {
                ssView1_Sheet1.Cells[intRow, 2].Text = "C/O";
            }
            else
            {
                ssView1_Sheet1.Cells[intRow, 2].Text = ComFunc.LeftH(clsIpdNr.SlipNo_Gubun(dt.Rows[intRow]["SLIPNO"].ToString().Trim(),
                                                                            ComFunc.LeftH(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), 2),
                                                                             dt.Rows[intRow]["BUN"].ToString().Trim()), 7);
            }

            switch (dt.Rows[intRow]["GBORDER"].ToString().Trim())
            {
                case "F":
                    ssView1_Sheet1.Cells[intRow, 2].Text = "Pre";
                    break;
                case "T":
                    ssView1_Sheet1.Cells[intRow, 2].Text = "Post";
                    break;
                case "M":
                    ssView1_Sheet1.Cells[intRow, 2].Text = "Adm";
                    break;
            }

            ssView1_Sheet1.Cells[intRow, 3].Text = dt.Rows[intRow]["ORDERCODE"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 25].Text = dt.Rows[intRow]["ORDERCODE"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 26].Text = dt.Rows[intRow]["BDATE1"].ToString().Trim();


            Order_Read_Move(dt, intRow, strSysDate);

            if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
            {
                ssView1_Sheet1.Cells[intRow, 1, intRow, ssView1_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
            }


            if ((String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "11") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "20") <= 0)
            || String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "23") == 0)
            {
                if (dt.Rows[intRow]["CONTENTS"].ToString().Trim().IndexOf(".") == -1)
                {
                    ssView1_Sheet1.Cells[intRow, 6].Text = (dt.Rows[intRow]["CONTENTS"].ToString().Trim() == "0" ? " " : dt.Rows[intRow]["CONTENTS"].ToString().Trim());
                }
                else
                {
                    ssView1_Sheet1.Cells[intRow, 6].Text = VB.Val(dt.Rows[intRow]["CONTENTS"].ToString().Trim()).ToString("###0.##");
                }

            }

            ssView1_Sheet1.Cells[intRow, 7].Text = dt.Rows[intRow]["REALQTY"].ToString().Trim();

            if (VB.Val(dt.Rows[intRow]["GBDIV"].ToString().Trim()) == 0)
            {
                ssView1_Sheet1.Cells[intRow, 8].Text = "";
            }
            else
            {
                ssView1_Sheet1.Cells[intRow, 8].Text = dt.Rows[intRow]["GBDIV"].ToString().Trim();
            }

            ssView1_Sheet1.Cells[intRow, 9].Text = dt.Rows[intRow]["NAL"].ToString().Trim();

            ssView1_Sheet1.Cells[intRow, 10].Text = (dt.Rows[intRow]["POWDER"].ToString().Trim() == "1" ? "◎" : "");


            if (String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "16") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "21") <= 0)
            {
                if (dt.Rows[intRow]["GBNGT"].ToString().Trim() != "")
                {
                    ssView1_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBNGT"].ToString().Trim();
                }

                if (dt.Rows[intRow]["GBGROUP"].ToString().Trim() != "")
                {
                    ssView1_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
                }
            }
            else if (String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "28") >= 0 && String.Compare(dt.Rows[intRow]["BUN"].ToString().Trim(), "39") <= 0)
            {
                if (VB.IsNumeric(dt.Rows[intRow]["GBGROUP"].ToString().Trim()) == true
                || dt.Rows[intRow]["GBGROUP"].ToString().Trim() == "")
                {
                    ssView1_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBNGT"].ToString().Trim();
                }
                else
                {
                    ssView1_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
                }
            }
            else
            {
                ssView1_Sheet1.Cells[intRow, 11].Text = dt.Rows[intRow]["GBGROUP"].ToString().Trim();
            }

            ssView1_Sheet1.Cells[intRow, 12].Text = dt.Rows[intRow]["GBER"].ToString().Trim();

            ssView1_Sheet1.Cells[intRow, 13].Text = dt.Rows[intRow]["GBSELF"].ToString().Trim();

            if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                    && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
            {

            }
            else
            {
                if (dt.Rows[intRow]["REMARK"].ToString().Trim() != "")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "#";
                }

                if (dt.Rows[intRow]["GBPRN"].ToString().Trim() != "")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = dt.Rows[intRow]["GBPRN"].ToString().Trim();
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "T")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "T";
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "O")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "O";
                }

                if (dt.Rows[intRow]["GBTFLAG"].ToString().Trim() == "A")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "A";
                }

                if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "S" || ssView1_Sheet1.Cells[intRow, 14].Text == "S")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "";
                    //ssView1_Sheet1.Cells[intRow, 4].Text = "(선)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                    ssView1_Sheet1.Cells[intRow, 4].Text = "(선수납)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                }
                else if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "B")
                {
                    ssView1_Sheet1.Cells[intRow, 14].Text = "";
                    //ssView1_Sheet1.Cells[intRow, 4].Text = "(수)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                    ssView1_Sheet1.Cells[intRow, 4].Text = "(수납완료)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                }
                else if (dt.Rows[intRow]["GBPRN"].ToString().Trim() == "A")
                {
                    //ssView1_Sheet1.Cells[intRow, 4].Text = "(A)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                    ssView1_Sheet1.Cells[intRow, 4].Text = "(선수납)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                }

            }

            if (dt.Rows[intRow]["VER"].ToString().Trim() == "CPORDER" && ssView1_Sheet1.Cells[intRow, 4].Text.IndexOf("[CP]") == -1)
            {
                ssView1_Sheet1.Cells[intRow, 4].Text = "[CP]" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
            }


            if (dt.Rows[intRow]["GBPORT"].ToString().Trim() == "M")
            {
                ssView1_Sheet1.Cells[intRow, 15].Text = "M";
            }

            //'remark
            switch (dt.Rows[intRow]["BUN"].ToString().Trim())
            {
                case "48":
                //case "49":    2019-06-25 분류 49는 초음파로 변경됨
                    ssView1_Sheet1.Cells[intRow, 16].Text = READ_ENDO_REMARK(dt.Rows[intRow]["ORDERNO"].ToString().Trim(), argPTNO);
                    break;
                default:
                    ssView1_Sheet1.Cells[intRow, 16].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    break;
            }

            if (dt.Rows[intRow]["ASA"].ToString().Trim() != "")
            {
                ssView1_Sheet1.Cells[intRow, 16].Text = "ASA : " + dt.Rows[intRow]["ASA"].ToString().Trim()
                + ComNum.VBLF + ssView1_Sheet1.Cells[intRow, 16].Text.Trim();
            }

            //'2015-09-02

            if (dt.Rows[intRow]["PRN_REMARK"].ToString().Trim() != "")
            {
                ssView1_Sheet1.Cells[intRow, 16].Text = dt.Rows[intRow]["PRN_REMARK"].ToString().Trim();
            }

            //'orderno
            ssView1_Sheet1.Cells[intRow, 17].Text = dt.Rows[intRow]["ORDERNO"].ToString().Trim();
            //'rowid
            ssView1_Sheet1.Cells[intRow, 18].Text = dt.Rows[intRow]["ROWID"].ToString().Trim();



            //처방의
            ssView1_Sheet1.Cells[intRow, 19].Text = dt.Rows[intRow]["DRNAME"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 20].Text = dt.Rows[intRow]["CAUTION_STRING"].ToString().Trim();

            if (ssView1_Sheet1.Cells[intRow, 20].Text.Trim() == "")
            {
                ssView1_Sheet1.Cells[intRow, 20].BackColor = Color.FromArgb(255, 255, 255);
                ssView1_Sheet1.Cells[intRow, 20].Font = new Font("맑은 고딕", 10, FontStyle.Regular);
            }
            else
            {
                ssView1_Sheet1.Cells[intRow, 20].BackColor = Color.FromArgb(255, 0, 0);
                ssView1_Sheet1.Cells[intRow, 20].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            }

            ssView1_Sheet1.Cells[intRow, 21].Text = dt.Rows[intRow]["PICKUPREMARK"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 22].Text = dt.Rows[intRow]["BUN"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 23].Text = dt.Rows[intRow]["SUCODE"].ToString().Trim();
            ssView1_Sheet1.Cells[intRow, 24].Text = dt.Rows[intRow]["GBPRN"].ToString().Trim();

            if (clsBagage.CHECK_SUBUL_ORDERCODE(clsDB.DbCon, dt.Rows[intRow]["ORDERCODE"].ToString().Trim()))
            {
                clsBagage.SET_COMBO_ORDER_SUBUL_DEPT(clsDB.DbCon, ssView1, 27, intRow);
            }
            else
            {
                clsBagage.SET_RETURN_LABEL(clsDB.DbCon, ssView1, 27, intRow);
            }

            ssView1_Sheet1.Cells[intRow, 27].Text = clsBagage.READ_ORDER_SUBUL_BUSE(clsDB.DbCon, dt.Rows[intRow]["SUBUL_WARD"].ToString().Trim(), "OCS");
        }

        private void Order_Read_Move(DataTable dt, int intRow, string strSysDate)
        {

            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = " SELECT    DISPHEADER CDISPHEADER, ORDERNAME CORDERNAME, DISPRGB CDISPRGB, ";
                SQL = SQL + " GBBOTH CGBBOTH, GBINFO CGBINFO, ";
                SQL = SQL + " GBQTY  CGBQTY, GBDOSAGE CGBDOSAGE, NEXTCODE CNEXTCODE, ";
                SQL = SQL + " ORDERNAMES CORDERNAMES, GBIMIV CGBIMIV, B.SUNAMEK, A.BUN  ";
                SQL = SQL + " FROM KOSMOS_OCS.OCS_ORDERCODE A, KOSMOS_PMPA.BAS_SUN B ";
                SQL = SQL + "WHERE A.ORDERCODE = '" + dt.Rows[intRow]["ORDERCODE"].ToString().Trim() + "' ";
                SQL = SQL + "  AND A.SLIPNO    = '" + dt.Rows[intRow]["SLIPNO"].ToString().Trim() + "'          ";
                SQL = SQL + "  AND A.SUCODE = B.SUNEXT(+)";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    switch (dt1.Rows[0]["BUN"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "20":
                        case "23":

                            if (dt1.Rows[0]["SUNAMEK"].ToString().Trim().IndexOf("자가약") != -1)
                            {
                                ssView1_Sheet1.Cells[intRow, 4].Text = ssView1_Sheet1.Cells[intRow, 4].Text
                                + dt1.Rows[0]["CORDERNAME"].ToString().Trim()
                                + " "
                                + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[intRow, 4].Text = dt1.Rows[0]["SUNAMEK"].ToString().Trim()
                                + clsIpdNr.GetDrugInfoSnaem(clsDB.DbCon, dt.Rows[intRow]["SUCODE"].ToString().Trim());
                            }

                            break;
                        default:
                            if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                            {
                                ssView1_Sheet1.Cells[intRow, 4].Text = ssView1_Sheet1.Cells[intRow, 4].Text
                                                                                + dt1.Rows[0]["CORDERNAME"].ToString().Trim()
                                                                                + " "
                                                                                + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                            {
                                ssView1_Sheet1.Cells[intRow, 4].Text = ssView1_Sheet1.Cells[intRow, 4].Text
                                                                        + dt1.Rows[0]["CDISPHEADER"].ToString().Trim()
                                                                        + " "
                                                                        + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[intRow, 4].Text = dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }

                            break;
                    }

                    ssView1_Sheet1.Cells[intRow, 4].Text = clsIpdNr.READ_ATTENTION(clsDB.DbCon, dt.Rows[intRow]["ORDERCODE"].ToString().Trim())
                                                            + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();

                    if (dt.Rows[intRow]["SLIPNO"].ToString().Trim() == "A4")
                    {
                        ssView1_Sheet1.Cells[intRow, 4].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }

                    if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")
                    {
                        ssView1_Sheet1.Cells[intRow, 5].Text = dt.Rows[intRow]["GBINFO"].ToString().Trim();
                    }
                    else if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")
                    {
                        ssView1_Sheet1.Cells[intRow, 5].Text = Read_Dosage(dt.Rows[intRow]["DOSCODE"].ToString().Trim());
                    }
                    else
                    {
                        ssView1_Sheet1.Cells[intRow, 5].Text = Read_Specman(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), dt.Rows[intRow]["SLIPNO"].ToString().Trim());
                    }

                    if (Convert.ToDateTime(strSysDate) >= Convert.ToDateTime("2013-09-03")
                    && READ_제한항생제_신청확인(dt.Rows[intRow]["PTNO"].ToString().Trim()
                                                , dt.Rows[intRow]["BDATE"].ToString().Trim()
                                                , dt.Rows[intRow]["SUCODE"].ToString().Trim()) == false)
                    {
                        ComFunc.MsgBox("제한항생제 [" + dt.Rows[intRow]["SUCODE"].ToString().Trim() + "] 승인신청서가 없거나,"
                                    + ComNum.VBLF + ComNum.VBLF + "승인자가 승인불가 처리한 경우입니다.."
                                    + ComNum.VBLF + ComNum.VBLF + "미승인,보류시 3일경과시 신청서를 재작성하셔야합니다.."
                                    + ComNum.VBLF + ComNum.VBLF + "항생제 처방이 필요한 경우라면 주치의와 상의후 조정해 주세요"
                                    + ComNum.VBLF + ComNum.VBLF + "문의사항 :감염내과(8224) 감염관리(8019) 전산실(8333)", "제한항생제");

                        ssView1_Sheet1.Cells[intRow, 0].LockBackColor = Color.Gray;
                        ssView1_Sheet1.Cells[intRow, 0].Locked = true;
                        ssView1_Sheet1.Cells[intRow, 0].Value = false;
                    }


                    if (Convert.ToBoolean(ssView1_Sheet1.Cells[intRow, 0].Value) == false)
                    {
                        ssView1_Sheet1.Cells[intRow, 1, intRow, ssView1_Sheet1.ColumnCount - 1].ForeColor =
                        ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }

                    dt1.Dispose();
                    dt1 = null;

                    SQL = " SELECT    SUGBN ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN  ";
                    SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + dt.Rows[intRow]["SUCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SUGBN  = '1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 4].Text = "(A)" + ssView1_Sheet1.Cells[intRow, 4].Text.Trim();
                    }
                }
                else
                {
                    if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                        && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                    {
                        ssView1_Sheet1.Cells[intRow, 4].Text = dt.Rows[intRow]["REMARK"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            string strORDERNO = "";
            int i = 0;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                ssView1_Sheet1.Cells[i, 0].Value = true;

                strORDERNO = ssView1_Sheet1.Cells[i, 17].Text.Trim();

                ChkssView1(i, strORDERNO);
            }
        }

        private void btnSelectAll1_Click(object sender, EventArgs e)
        {
            ssViewM_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, 0].Value = true;
        }

        private void btnSelectCancel_Click(object sender, EventArgs e)
        {
            ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, 0].Value = false;
        }

        private void btnSelectCancel1_Click(object sender, EventArgs e)
        {
            ssViewM_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, 0].Value = false;
        }

        /// <summary>
        /// cmdAirShooter_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] strCODE = new string[0];

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                {
                    Array.Resize<string>(ref strCODE, strCODE.Length + 1);

                    strCODE[strCODE.Length - 1] = ssView1_Sheet1.Cells[i, 3].Text.Trim();

                    switch (ssView1_Sheet1.Cells[i, 22].Text.Trim())
                    {
                        case "11":
                        case "12":
                        case "20":
                        case "23":
                            break;
                        default:

                            ComFunc.MsgBox("긴급약 픽업은 약종류(경구, 외용, 주사제)만 가능합니다.", "확인");
                            return;
                    }
                }
            }

            if (strCODE.Length > 0)
            {
                clsBagage.Not_AirSht(clsDB.DbCon, strCODE, "");
            }
            RUN_PICKUP("1");
        }

        private bool VERIFY_PICKUP()
        {
            bool RtnVal = false;

            //'2014-10-31  주임 김현욱
            //'픽업하기 전 오더에 대한 검증을 위한 함수임.
            //'현재는 AST 반응만을 체크함
            //'추가 검증이 필요할 경우 해당 함수에 넣어주세요.

            int i = 0;
            string strSucode = "";

            RtnVal = true;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                {
                    strSucode = ssView1_Sheet1.Cells[i, 3].Text.Trim();

                    if (clsIpdNr.READ_AST_REACTION(clsDB.DbCon, strSucode, FstrPtno, FstrInDate) == "Positive")
                    {
                        if (ComFunc.MsgBoxQ("★오더코드 : " + strSucode + " ★" + ComNum.VBLF + "해당 약은 알러지 반응 양성으로 등록되어있습니다."
                            + ComNum.VBLF + "처방을 받으시겠습니까?", "알러지 양성 반응!", MessageBoxDefaultButton.Button1) == DialogResult.No)
                        {
                            RtnVal = false;
                            return RtnVal;
                        }
                    }
                }
            }
            return RtnVal;
        }


        private void RUN_PICKUP(string argGubun)
        {
            //'================================================
            //'2014-08-08 김현욱 수정
            //'기송관 요청용 PICKUP 관련하여 구분 추가
            //'기송관요청 버튼을 클릭하면 OCS_IORDER.AIRSHT에 '1'이 들어감
            //'해당 정보가 OCS_PHARMACY.AIRSHT에 동일에게 전송
            //'해당 정보를 이용하여 별도의 처방전 처리 프로세스 만듬
            //'기송관 제외용 약코드 관리 프로그램 별도 만들예정
            // '===============================================

            int i = 0;
            string strROWID = "";
            string strOrderName = "";
            string strORDERCODE = "";  //'2014-04-02
            string strBun = "";
            string strDC = "";
            string strR = "";
            DataTable dt = null;
            DataTable dt1 = null;

            string strOrderNo = ""; //nOrderNo Double
            string strSucode = "";
            string strBDATE = "";
            string strDrCd = "";
            string strSWard = "";
            string[] strGuDong = null;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (VERIFY_PICKUP() == false)
            {
                return;
            }

            //미리 메세지 체크 함 (setBeginTran 중에 메세지를 안 띄우기 위해 수정)

            mstrPickupR = null;
            mstrPickupR = new string[ssView1_Sheet1.RowCount];
            strGuDong = null;
            strGuDong = new string[ssView1_Sheet1.RowCount];

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {

                strDC = ssView1_Sheet1.Cells[i, 1].Text.Trim();
                strORDERCODE = ssView1_Sheet1.Cells[i, 3].Text.Trim();
                strOrderName = ssView1_Sheet1.Cells[i, 4].Text.Trim();
                strR = ssView1_Sheet1.Cells[i, 14].Text.Trim();

                if (strR == "T")
                {
                    if (ssView1_Sheet1.Cells[i, 24].Text.Trim() == "P")
                    {
                        strR = "P";
                    }
                }

                strOrderNo = Convert.ToString(Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i, 17].Text.Trim())));
                strROWID = ssView1_Sheet1.Cells[i, 18].Text.Trim();
                mstrPickupR[i] = ssView1_Sheet1.Cells[i, 21].Text.Trim();
                strBun = ssView1_Sheet1.Cells[i, 22].Text.Trim();
                strSucode = ssView1_Sheet1.Cells[i, 23].Text.Trim();
                strBDATE = ssView1_Sheet1.Cells[i, 26].Text.Trim();
                strSWard = VB.Right(ssView1_Sheet1.Cells[i, 27].Text.Trim(), 2);

                strGuDong[i] = "";

                if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                {

                    if (MSG_고가피하주사(strSucode, "") != "")
                    {
                        ComFunc.MsgBox("※오더코드 : " + strSucode
                                        + ComNum.VBLF + "프리필시린지로 개봉 후 그대로 주사하시면 됩니다"
                                        + ComNum.VBLF + "<<공기를 빼지 말고 그대로 주사하십시오.>>", "확인");
                    }

                    if (MSG_휴미라(strSucode, "") != "")
                    {
                        ComFunc.MsgBox("※오더코드 : " + strSucode
                                        + ComNum.VBLF + "개봉 즉시 압력에 의해 주사약이 나오므로 외래주시실에서 투약합니다."
                                        + ComNum.VBLF + "<<환자분을 외래 주사기실로 안내해 주십시오.>>", "확인");
                    }

                    if (strORDERCODE == "C/O" && VB.Val(strOrderNo) > 0)
                    {
                        strDrCd = "";

                        SQL = "";
                        SQL = " SELECT TODRCODE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ITRANSFER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND ORDERNO =" + strOrderNo + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strDrCd = dt.Rows[0]["TODRCODE"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;

                        if (ComFunc.READ_SELECT_DOCTOR_CHK(clsDB.DbCon, "I", strDrCd) == "OK")
                        {
                            ComFunc.MsgBox(clsVbfunc.GetBASDoctorName(clsDB.DbCon, strDrCd) + "과장님은 선택의사 입니다.", "확인");
                        }

                        if (ComFunc.MsgBoxQ("컨설트 픽업 거동상태 확인!!"
                            + ComNum.VBLF + ComNum.VBLF + "거동가능 하면 Yes(예:거동가능), 아니면  No(아니요:거동불가) 를 선택하세요!!"
                            , "거동상태", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            strGuDong[i] = "0";  //'0:거동가능,1:불가능;
                        }
                        else
                        {
                            strGuDong[i] = "1";  //'0:거동가능,1:불가능;
                        }

                    }

                    if ((strBun == "72" || strBun == "71") && mbolOPMAIN == false) //'UCase(App.EXEName) <> "OPMAIN" 마취과 픽업시 선택안되게 수정 add kyo 2017.03.15
                    {

                        if (frmPickupRX != null)
                        {
                            frmPickupRX.Close();
                            frmPickupRX = null;
                        }

                        frmPickupRX = new frmPickupR(strOrderName, i);
                        frmPickupRX.rEventClosed += FrmPickupRX_rEventClosed;
                        frmPickupRX.rSetRemark += FrmPickupRX_rSetRemark;
                        frmPickupRX.TopMost = true;

                        frmPickupRX.ShowDialog();
                    }
                }
            }


            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            //비급여 체크용
            StringBuilder SuCodeList = new StringBuilder();

            try
            {

                for (i = 0; i < ssView1_Sheet1.RowCount; i++)
                {
                    strDC = ssView1_Sheet1.Cells[i, 1].Text.Trim();
                    strORDERCODE = ssView1_Sheet1.Cells[i, 3].Text.Trim();
                    strOrderName = ssView1_Sheet1.Cells[i, 4].Text.Trim();
                    strR = ssView1_Sheet1.Cells[i, 14].Text.Trim();

                    if (strR == "T")
                    {
                        if (ssView1_Sheet1.Cells[i, 24].Text.Trim() == "P")
                        {
                            strR = "P";
                        }
                    }

                    strOrderNo = Convert.ToString(Convert.ToInt32(VB.Val(ssView1_Sheet1.Cells[i, 17].Text.Trim())));
                    strROWID = ssView1_Sheet1.Cells[i, 18].Text.Trim();
                    //mstrPickupR[i] = ssView1_Sheet1.Cells[i, 21].Text.Trim(); 체크 부분에서 값이 들어 옴.
                    strBun = ssView1_Sheet1.Cells[i, 22].Text.Trim();
                    strSucode = ssView1_Sheet1.Cells[i, 23].Text.Trim();
                    strBDATE = ssView1_Sheet1.Cells[i, 26].Text.Trim();
                    strSWard = VB.Right(ssView1_Sheet1.Cells[i, 27].Text.Trim(), 2);

                    if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        //2019-04-09 기준 변경되어 처방 가능함. 심사팀 이민주주임 제외 처리 요청함(10:30)
                        //2014-04-02
                        //if (strBun == "11" && (strORDERCODE == "PANBU" || strORDERCODE == "MCP" || strORDERCODE == "MCPA"))
                        //{
                        //    if (READ_CHK_DRUG_CNTCHK2(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"), strORDERCODE) == false)
                        //    {
                        //        clsDB.setRollbackTran(clsDB.DbCon);
                        //        Cursor.Current = Cursors.Default;
                        //        ComFunc.MsgBox(strORDERCODE + " 수가는 재원중 5일이상 사용할수 없습니다.."
                        //        + ComNum.VBLF + ComNum.VBLF + "반환처리후 처리하십시오!!", "확인");
                        //        return;
                        //    }
                        //}


                        //'2014-07-10
                        if (strBun == "11" || strBun == "12" || strBun == "20")
                        {
                            //    '삭제된 코드 체크
                            SQL = " SELECT TRIM(SUCODE) ";
                            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_SUT ";
                            SQL = SQL + ComNum.VBLF + "WHERE DELDATE < TRUNC(SYSDATE) ";
                            SQL = SQL + ComNum.VBLF + "    AND DELDATE >=TRUNC(SYSDATE-365) ";
                            SQL = SQL + ComNum.VBLF + "    AND BUN IN ('11','12','20') ";
                            SQL = SQL + ComNum.VBLF + " AND SUNEXT ='" + strSucode + "' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt.Rows.Count > 0)
                            {

                                dt.Dispose();
                                dt = null;
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(strSucode
                                    + ComNum.VBLF + strOrderName
                                    + ComNum.VBLF + "삭제처리된 수가입니다... 오더 DC를 하십시오!!", "삭제수가");
                                return;
                            }

                            dt.Dispose();
                            dt = null;


                            //    '2014-08-05 약 DC만전송건 체크
                            if (strDC == "D-")
                            {
                                //'PRN ,AS 제외
                                if (strSucode.ToUpper().Trim() == "JAGA"
                                || strR == "P"
                                || strR == "A"
                                || strR == "S"
                                || VB.Left(strOrderName, 3) == "(선수"
                                || VB.Left(strOrderName, 3) == "(수납")
                                //|| VB.Left(strOrderName, 3) == "(선)"
                                //|| VB.Left(strOrderName, 3) == "(수)"
                                //|| VB.Left(strOrderName, 3) == "(A)")
                                {

                                }
                                else
                                {
                                    //'픽업금액체크 예외수가
                                    if (READ_BCODE_NRINFO("NUR_픽업체크제외수가", strSucode) == true)
                                    {

                                    }
                                    else
                                    {
                                        SQL = " SELECT ROWID FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
                                        SQL = SQL + " WHERE PANO ='" + FstrPtno + "' ";
                                        SQL = SQL + "  AND ORDERNO =" + strOrderNo + " ";
                                        //'============================================================================
                                        //' 2016-05-19 김현욱 수정
                                        if (strBDATE != "")
                                        {
                                            SQL = SQL + "  AND BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                                        }
                                        else
                                        {
                                            SQL = SQL + "  AND BDATE >= TO_DATE('" + FstrInDate + "','YYYY-MM-DD')";
                                        }
                                        //'============================================================================

                                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            return;
                                        }

                                        if (dt.Rows.Count == 0)
                                        {
                                            if (strSucode != "OM-31G") //'금액없는수가
                                            {
                                                SQL = "";
                                                SQL = " SELECT PTNO ";
                                                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_PHARMACY ";
                                                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "' ";
                                                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                                                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + strOrderNo;

                                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                                if (SqlErr != "")
                                                {
                                                    dt.Dispose();
                                                    dt = null;
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                    Cursor.Current = Cursors.Default;
                                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                                    return;
                                                }

                                                if (dt1.Rows.Count > 0)
                                                {
                                                    dt1.Dispose();
                                                    dt1 = null;
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    Cursor.Current = Cursors.Default;
                                                    ComFunc.MsgBox(strORDERCODE + ComNum.VBLF + strOrderName + ComNum.VBLF + "DC 처리전 해당오더 미전송 있습니다.. 오더 전송후 픽업하십시오!!", "확인");
                                                    return;
                                                }
                                                else
                                                {
                                                    SQL = "";
                                                    SQL = " INSERT INTO KOSMOS_OCS.OCS_IORDER_AUTOPICKUP(";
                                                    SQL = SQL + ComNum.VBLF + " AUTOPICKUPDATE,";
                                                    SQL = SQL + ComNum.VBLF + " PTNO,BDATE,SEQNO,DEPTCODE,";
                                                    SQL = SQL + ComNum.VBLF + " DRCODE,STAFFID,SLIPNO,ORDERCODE,";
                                                    SQL = SQL + ComNum.VBLF + " SUCODE,BUN,GBORDER,CONTENTS,";
                                                    SQL = SQL + ComNum.VBLF + " BCONTENTS,REALQTY,QTY,REALNAL,";
                                                    SQL = SQL + ComNum.VBLF + " NAL,DOSCODE,GBINFO,GBSELF,";
                                                    SQL = SQL + ComNum.VBLF + " GBSPC,GBNGT,GBER,GBPRN,";
                                                    SQL = SQL + ComNum.VBLF + " GBDIV,GBBOTH,GBACT,GBTFLAG,";
                                                    SQL = SQL + ComNum.VBLF + " GBSEND,GBPOSITION,GBSTATUS,NURSEID,";
                                                    SQL = SQL + ComNum.VBLF + " ENTDATE,WARDCODE,ROOMCODE,BI,";
                                                    SQL = SQL + ComNum.VBLF + " ORDERNO,REMARK,ACTDATE,GBGROUP,";
                                                    SQL = SQL + ComNum.VBLF + " GBPORT,ORDERSITE,MULTI,MULTIREMARK,";
                                                    SQL = SQL + ComNum.VBLF + " DUR,LABELPRINT,ACTDIV,GBSEND_OORDER,";
                                                    SQL = SQL + ComNum.VBLF + " GBPICKUP , PICKUPSABUN, PICKUPDATE)";
                                                    SQL = SQL + ComNum.VBLF + " SELECT SYSDATE, ";
                                                    SQL = SQL + ComNum.VBLF + " PTNO,BDATE,SEQNO,DEPTCODE,";
                                                    SQL = SQL + ComNum.VBLF + " DRCODE,STAFFID,SLIPNO,ORDERCODE,";
                                                    SQL = SQL + ComNum.VBLF + " SUCODE,BUN,GBORDER,CONTENTS,";
                                                    SQL = SQL + ComNum.VBLF + " BCONTENTS,REALQTY,QTY,REALNAL,";
                                                    SQL = SQL + ComNum.VBLF + " NAL,DOSCODE,GBINFO,GBSELF,";
                                                    SQL = SQL + ComNum.VBLF + " GBSPC,GBNGT,GBER,GBPRN,";
                                                    SQL = SQL + ComNum.VBLF + " GBDIV,GBBOTH,GBACT,GBTFLAG,";
                                                    SQL = SQL + ComNum.VBLF + " GBSEND,GBPOSITION,GBSTATUS,NURSEID,";
                                                    SQL = SQL + ComNum.VBLF + " ENTDATE,WARDCODE,ROOMCODE,BI,";
                                                    SQL = SQL + ComNum.VBLF + " ORDERNO,REMARK,ACTDATE,GBGROUP,";
                                                    SQL = SQL + ComNum.VBLF + " GBPORT,ORDERSITE,MULTI,MULTIREMARK,";
                                                    SQL = SQL + ComNum.VBLF + " DUR,LABELPRINT,ACTDIV,GBSEND_OORDER,";
                                                    SQL = SQL + ComNum.VBLF + " GBPICKUP , PICKUPSABUN, PICKUPDATE";
                                                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                                                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                                    if (SqlErr != "")
                                                    {
                                                        dt.Dispose();
                                                        dt = null;
                                                        dt1.Dispose();
                                                        dt1 = null;
                                                        clsDB.setRollbackTran(clsDB.DbCon);
                                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                                        Cursor.Current = Cursors.Default;
                                                        return;
                                                    }

                                                    SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET GBSEND = ' ' ";
                                                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                                    if (SqlErr != "")
                                                    {
                                                        dt.Dispose();
                                                        dt = null;
                                                        dt1.Dispose();
                                                        dt1 = null;
                                                        clsDB.setRollbackTran(clsDB.DbCon);
                                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                                        Cursor.Current = Cursors.Default;
                                                        return;
                                                    }

                                                    dt1.Dispose();
                                                    dt1 = null;
                                                }                                                
                                            }
                                        }

                                        dt.Dispose();
                                        dt = null;

                                    }
                                }
                            }
                            //ER에서 낸 원외약은 체크안되도록 보완 작업(2021-01-18)
                            if (READ_CHK_원외약구분(strORDERCODE) == true && cboWard.Text.Trim() != "ER")
                            {
                                ComFunc.MsgBox(strORDERCODE + " 수가는 원외처방전용약입니다.. 사용할수 없습니다.."
                                    + ComNum.VBLF + ComNum.VBLF + "반환처리후 처리하십시오!!", "확인");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        //2014 - 06 - 10 컨설트오더이면 거동유무체크
                        //setBeginTran 중에 메세지를 안 띄우기 위해 수정 

                        if (strORDERCODE == "C/O" && VB.Val(strOrderNo) > 0)
                        {
                            SQL = " UPDATE KOSMOS_OCS.OCS_ITRANSFER SET GBSTS = '" + strGuDong[i] + "' ";  //'0:거동가능,1:불가능;
                            SQL = SQL + " WHERE PTNO = '" + FstrPtno + "' ";
                            SQL = SQL + "  AND ORDERNO =" + strOrderNo + " ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        if ((strBun == "72" || strBun == "71") && mbolOPMAIN == false) //'UCase(App.EXEName) <> "OPMAIN" 마취과 픽업시 선택안되게 수정 add kyo 2017.03.15
                        {
                            if (mstrPickupR[i] != "")
                            {
                                SQL = "";
                                SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET ";
                                SQL = SQL + ComNum.VBLF + " GBPICKUP = '*' , ";
                                SQL = SQL + ComNum.VBLF + " SUBUL_WARD = '" + strSWard + "', ";
                                SQL = SQL + ComNum.VBLF + " PICKUPSABUN = '" + Convert.ToInt32(clsType.User.Sabun) + "' , ";
                                SQL = SQL + ComNum.VBLF + " PICKUPDATE = SYSDATE, ";
                                SQL = SQL + ComNum.VBLF + " PICKUPREMARK = '" + mstrPickupR[i] + "'";
                                SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE KOSMOS_OCS.OCS_IORDER SET";

                            if (argGubun == "1")
                            {
                                SQL = SQL + ComNum.VBLF + " AIRSHT = '" + argGubun + "', ";
                            }

                            SQL = SQL + ComNum.VBLF + " SUBUL_WARD = '" + strSWard + "', ";
                            SQL = SQL + ComNum.VBLF + " GBPICKUP = '*' , ";
                            SQL = SQL + ComNum.VBLF + " PICKUPSABUN = '" + Convert.ToInt32(clsType.User.Sabun) + "' , ";
                            SQL = SQL + ComNum.VBLF + " PICKUPDATE = SYSDATE";
                            SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        #region 전산업무 의뢰서 처리 - 2021-396
                        string GBSELF = ssView1_Sheet1.Cells[i, 13].Text.Trim();
                        if (strBun != "48" && strBun != "75" && (GBSELF.Equals("1") || GBSELF.Equals("2")))
                        {
                            SuCodeList.Append("'" + strSucode + "', ");
                        }
                        #endregion
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                #region 전산업무 의뢰서 처리 - 2021-396
                strSucode = SuCodeList.ToString();

                if (strSucode.Length > 0)
                {
                    if (strSucode.Length > 3 && strSucode.Substring(strSucode.Length - 3).Equals("', "))
                    {
                        strSucode = strSucode.Substring(0, strSucode.Length - 2);
                    }

                    OracleDataReader reader = null;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT LISTAGG(B.SUNEXT || '(' || B.SUNAMEK || ')', '\r\n') WITHIN GROUP(ORDER BY B.SUNAMEK) AS NAMES ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUT A";
                    SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_SUN B";
                    SQL = SQL + ComNum.VBLF + "    ON A.SUNEXT = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + " WHERE ((A.BUN IN ('71','73') AND A.SUGBF = '1') OR B.GBSELFHANG = 'Y')";
                    SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT IN (" + strSucode + ")";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        ComFunc.MsgBoxEx(this, "비급여 목록 " + "\r\n" + reader.GetValue(0).ToString().Trim());
                    }

                    reader.Dispose();
                    reader = null;
                }

                #endregion

                SCREEN_CLEAR();
                if (ssList_Sheet1.ActiveRowIndex != -1)
                {
                    ssListCellClick(ssList_Sheet1.ActiveRowIndex, 0);
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void FrmPickupRX_rSetRemark(string strRemark, int intRow)
        {
            mstrPickupR[intRow] = strRemark;
        }

        private void FrmPickupRX_rEventClosed()
        {
            frmPickupRX.Close();
            frmPickupRX = null;
        }

        private bool READ_CHK_원외약구분(string argSuCode)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT A.SUNEXT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUT A, KOSMOS_PMPA.BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.SUNEXT=B.SUNEXT  ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUCODE ='" + argSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUGBJ='1' ";  //'원외전용;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private bool READ_BCODE_NRINFO(string argGubun, string argCode)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = "SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN='" + argGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE='" + argCode.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='')";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private bool READ_CHK_DRUG_CNTCHK2(string strPtno, string argBDate, string argSuCode)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strInDate = "";

            try
            {
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                RtnVal = true;

                SQL = " SELECT SUM(NAL) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PTNO ='" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND SUCODE ='" + argSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "  GROUP BY SUCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) >= 5)
                    {
                        RtnVal = false;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private string MSG_휴미라(string strSucode, string argGbn)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_PICKUP_휴미라'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strSucode + "'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (argGbn == "주사티켓")
                    {
                        RtnVal = "<<외래 주시실에서 주사>>";
                    }
                    else
                    {
                        RtnVal = "※오더코드 : " + strSucode;
                        RtnVal = RtnVal + ComNum.VBLF + "개봉 즉시 압력에 의해 주사약이 나오므로 외래주시실에서 투약합니다.";
                        RtnVal = RtnVal + ComNum.VBLF + "<<환자분을 외래 주사기실로 안내해 주십시오.>>";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private string MSG_고가피하주사(string strSucode, string argGbn)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_PICKUP_고가피하주사'";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + strSucode + "'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (argGbn == "주사티켓")
                    {
                        RtnVal = "<<공기를 빼지 말고 그대로 주사>>";
                    }
                    else
                    {
                        RtnVal = "※오더코드 : " + strSucode;
                        RtnVal = RtnVal + ComNum.VBLF + "프리필시린지로 개봉 후 그대로 주사하시면 됩니다";
                        RtnVal = RtnVal + ComNum.VBLF + "<<공기를 빼지 말고 그대로 주사하십시오.>>";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            RUN_PICKUP("");
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            string strROWID = "";
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                    return; //권한 확인



                for (i = 0; i < ssViewM_Sheet1.RowCount; i++)
                {
                    strROWID = ssViewM_Sheet1.Cells[i, 21].Text.Trim();

                    if (Convert.ToBoolean(ssViewM_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = " UPDATE KOSMOS_OCS.OCS_IORDER ";
                        SQL = SQL + ComNum.VBLF + "SET GBPICKUP = '*' ,";
                        SQL = SQL + ComNum.VBLF + "PICKUPSABUN = '" + clsType.User.Sabun + "' ,";
                        SQL = SQL + ComNum.VBLF + "PICKUPDATE = SYSDATE";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(" PICKUP 작업중 오류 발생 하였습니다. "
                                    + ComNum.VBLF + ComNum.VBLF + "해당 환자를 다시 한번 확인 후 PICKUP 하세요 ", "오류");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);

                Read_Orders_Multi();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            Read_Orders_Multi();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            panel8.Enabled = false;

            if (bolLoad == true)
            {
                panel8.Enabled = true;
                return;
            }

            int i = 0;
            string strJob = "";
            string strPriDate = "";
            string strToDate = "";
            string strNextDate = "";
            string strROOM_OLD = "";
            string strPreTOI = "";

            ssList_Sheet1.RowCount = 0;

            strJob = VB.Left(cboJob.Text, 1);

            strPriDate = dtpDate.Value.AddDays(-1).ToString("yyyy-MM-dd");
            strToDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strNextDate = dtpDate.Value.AddDays(1).ToString("yyyy-MM-dd");

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    panel8.Enabled = true;
                    return; //권한 확인
                }


                SQL = "SELECT M.WARDCODE,M.ROOMCODE,M.PANO,M.SNAME,M.SEX,M.AGE,M.BI,M.PNAME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE,M.ILSU,M.IPDNO,M.GBSTS,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OUTDATE,'YYYY-MM-DD') OUTDATE, M.GBDRG, ";
                SQL = SQL + ComNum.VBLF + " M.DEPTCODE,M.DRCODE,D.DRNAME,M.AMSET1,M.AMSET4,M.AMSET6,M.AMSET7,  KOSMOS_PMPA.READ_CP_PROGRESS_NUR(M.IPDNO) CP_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_DOCTOR  D ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.ROOMCODE='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + " WHERE M.ROOMCODE='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE IN ('ND','IQ','NR') ";
                        break;
                    case "HD":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM  TONG_HD_DAILY WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') )";
                        break;
                    case "OP":
                    case "AG":
                        if (cboWard.Text == "AG")
                        {
                            SQL = SQL + ComNum.VBLF + " WHERE (M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD')  ";
                            SQL = SQL + ComNum.VBLF + "     AND GbAngio ='Y' )) ";
                            
                        }   
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " WHERE (M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER WHERE OPDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND GbAngio ='N' )  ";
                            SQL = SQL + ComNum.VBLF + "  OR M.PANO IN (SELECT PTNO FROM KOSMOS_OCS.OCS_ITRANSFER WHERE TODEPTCODE = 'PC' AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')) )";
                        }
                        break;
                    case "ENDO":
                        //'내시경+당일 내시경 처방 추가
                        SQL = SQL + ComNum.VBLF + " WHERE  ( M.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST WHERE TRUNC(RDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD') )  OR ";
                        SQL = SQL + ComNum.VBLF + "          ( M.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.OCS_IORDER WHERE BDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') AND BUN IN ('48','49') AND GBSEND ='*' AND ACCSEND IS NULL AND GBPICKUP IS NULL)   )";
                        SQL = SQL + ComNum.VBLF + "        )";
                        break;
                    case "ER":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.OPD_MASTER  WHERE BDATE =TO_DATE('" + strToDate + "','YYYY-MM-DD') AND DEPTCODE ='ER' ) ";
                        break;
                    case "RA":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PTNO   FROM KOSMOS_OCS.OCS_ITRANSFER  WHERE TODRCODE ='1107' AND GBDEL <>'*'  AND TRUNC(EDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD' ))  ";
                        break;
                    case "TTE":
                        SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN ( SELECT PTNO   FROM KOSMOS_OCS.OCS_ITRANSFER  WHERE TODRCODE ='1120' AND GBDEL <>'*'  AND TRUNC(EDATE) =TO_DATE('" + strToDate + "','YYYY-MM-DD' ))  ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + " WHERE M.WARDCODE='" + cboWard.Text.Trim() + "' ";
                        break;
                }

                if (ComQuery.CurrentDateTime(clsDB.DbCon, "D") == "20120920")
                {
                    SQL = SQL + ComNum.VBLF + "     AND (M.REMARK IS NULL OR M.REMARK <>'EDPS') ";  //'2012-09-20 같은날퇴원,입원있어서 제외함;
                }

                if (VB.Left(cboDept.Text.Trim(), 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DEPTCODE = '" + VB.Left(cboDept.Text.Trim(), 2) + "' ";
                }

                if (VB.Left(cboDrCode.Text.Trim(), 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.DRCODE = '" + VB.Left(cboDrCode.Text.Trim(), 4) + "' ";
                }

                //    '작업분류
                if (strJob == "1") //'재원자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL)";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    if (cboWard.Text.Trim() != "ER")
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    }
                    SQL = SQL + ComNum.VBLF + " AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "2") //'당일입원자
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.INDATE >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IPWONTIME >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IPWONTIME < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.PANO <> '81000004' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.GBSTS <> '9' ";
                }
                else if (strJob == "3") //'퇴원예고
                {
                    SQL = SQL + ComNum.VBLF + " AND EXISTS ( SELECT * FROM KOSMOS_PMPA.NUR_MASTER SUB ";
                    SQL = SQL + ComNum.VBLF + "  WHERE M.PANO = SUB.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND SUB.ROUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND M.IPDNO = SUB.IPDNO) ";
                }
                else if (strJob == "4") //'당일퇴원
                {
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS = '7' "; //'퇴원수납완료;
                }
                else if (strJob == "6") //'수술예정자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS IN ('0','2')  ";
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                }
                else if (strJob == "A") //'응급실경유입원 1-3일전
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL)";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU >= 1 AND M.ILSU<=3) ";
                    SQL = SQL + ComNum.VBLF + " AND M.AMSET7 IN ('3','4','5') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROUTDATE>=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "B") //'재원기간 7-14일 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL)";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU>=7 AND M.ILSU<=14) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "C") //'재원기간 3-7일 환자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL)";
                    SQL = SQL + ComNum.VBLF + " OR M.OUTDATE>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ILSU>=3 AND M.ILSU<=7) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS <> '9' ";
                }
                else if (strJob == "D") //'어제퇴원자
                {
                    SQL = SQL + ComNum.VBLF + " AND M.OUTDATE=TRUNC(SYSDATE-1) ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND M.GBSTS IN ('0','2')  ";
                }

                SQL = SQL + "  AND M.PANO=P.PANO(+) ";
                SQL = SQL + "  AND M.DRCODE=D.DRCODE(+) ";


                if (cboTeam.Text.Trim() == "지정")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.PANO IN (SELECT PTNO FROM KOSMOS_PMPA.NUR_SABUN_PTNO WHERE SABUN = '" + clsType.User.Sabun + "')       ";
                }
                else if (cboTeam.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                    SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_TEAM_ROOMCODE T";
                    SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                }

                if (chkTransfor.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND EXISTS (";
                    SQL = SQL + ComNum.VBLF + "  SELECT SUB1.PANO";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_TRANSFOR SUB1";
                    SQL = SQL + ComNum.VBLF + "  WHERE SUB1.PANO = M.PANO";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TOWARD = M.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.FRWARD <> SUB1.TOWARD";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.IPDNO = M.IPDNO";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TRSDATE >= TO_DATE('" + strToDate + " 00:00','YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "  AND SUB1.TRSDATE <= TO_DATE('" + strToDate + " 23:59','YYYY-MM-DD HH24:MI'))";
                }

                //'SORT
                if (cboWard.Text.Trim() == "32" || cboWard.Text.Trim() == "33" || cboWard.Text.Trim() == "35")
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.ROOMCODE, M.BEDNUM ASC, M.SNAME, M.INDATE DESC  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY M.ROOMCODE, M.SNAME, M.INDATE DESC  ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    panel8.Enabled = true;
                    return;
                }

                mstrIpdno = null;

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    mstrIpdno = new string[dt.Rows.Count];

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strROOM_OLD != dt.Rows[i]["ROOMCODE"].ToString().Trim())
                        {
                            ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            strROOM_OLD = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 8 + 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 9 + 1].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = clsIpdNr.READ_AGE_GESAN(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()
                                                                                    , dt.Rows[i]["AGE"].ToString().Trim())
                                                                                    + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]" : "");

                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CP_PROGRESS"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        strPreTOI = "";

                        if (clsVbfunc.GetPreToi(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dtpDate.Value.ToString("yyyy-MM-dd"), "") == "1")
                        {
                            ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(100, 100, 255);
                            //ssList_Sheet1.Cells[i, 0, i, 5].BackColor = Color.FromArgb(100, 100, 255);
                            strPreTOI = "Y";
                            ssList_Sheet1.Cells[i, 7 + 1].Text = "Y";
                        }

                        ssList_Sheet1.Cells[i, 6 + 1].Text = "";

                        //        ssList.Col = 7 + 1


                        if (strPreTOI == "Y")
                        {
                            SQL = " SELECT A, B - (B - A) B FROM (";
                            SQL = SQL + ComNum.VBLF + " SELECT";
                            SQL = SQL + ComNum.VBLF + " (SELECT COUNT (*) CNT";
                            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER";
                            SQL = SQL + ComNum.VBLF + "  WHERE     BDATE = TO_DATE ('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "        AND PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "        AND (GBPICKUP = '' OR GBPICKUP IS NULL)";
                            SQL = SQL + ComNum.VBLF + "        AND GBSTATUS IN (' ', 'D+', 'D', 'D-')";

                            if (chkNC.Checked == true)
                            {
                                //SQL = SQL + "  AND GBSLIP NOT IN ('A7') ";
                            }

                            SQL = SQL + ComNum.VBLF + "        AND (GBIOE NOT IN ('E', 'EI') OR GBIOE IS NULL)) A,";
                            SQL = SQL + ComNum.VBLF + " (SELECT COUNT (*) CNT";
                            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IORDER";
                            SQL = SQL + ComNum.VBLF + "  WHERE     BDATE = TO_DATE ('" + dtpDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "        AND PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "        AND GBSTATUS IN (' ', 'D+', 'D', 'D-')";

                            if (chkNC.Checked == true)
                            {
                                //SQL = SQL + "  AND GBSLIP NOT IN ('A7') ";
                            }

                            SQL = SQL + ComNum.VBLF + "        AND (GBIOE NOT IN ('E', 'EI') OR GBIOE IS NULL)) B";
                            SQL = SQL + ComNum.VBLF + "        FROM DUAL)";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                panel8.Enabled = true;
                                return;
                            }

                            if (VB.Val(dt1.Rows[0]["B"].ToString().Trim()) > 0
                            && dt1.Rows[0]["A"].ToString().Trim() != dt1.Rows[0]["B"].ToString().Trim())
                            {
                                ssList_Sheet1.Cells[i, 6 + 1].Text = "◆";
                                ssList_Sheet1.Cells[i, 6 + 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else if (VB.Val(dt1.Rows[0]["A"].ToString().Trim()) > 0)
                            {
                                ssList_Sheet1.Cells[i, 6 + 1].Text = "◆";
                                ssList_Sheet1.Cells[i, 6 + 1].ForeColor = Color.FromArgb(0, 0, 0);
                            }
                            else
                            {
                                if (READ_DUTY_PICKUP(dt.Rows[i]["PANO"].ToString().Trim(), ComFunc.LPAD(clsType.User.Sabun, 5, "0"), dtpDate.Value.ToString("yyyy-MM-dd")) == true)
                                {
                                    ssList_Sheet1.Cells[i, 6 + 1].Text = "◇";
                                }
                            }
                        }
                        else
                        {
                            //'PICKUP 할것이 남아 있는지 점검

                            SQL = "";
                            SQL = " SELECT COUNT(*) CNT FROM KOSMOS_OCS.OCS_IORDER ";
                            SQL = SQL + ComNum.VBLF + " WHERE BDATE =TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (GBPICKUP ='' OR GBPICKUP IS NULL) ";
                            SQL = SQL + ComNum.VBLF + "   AND GBSTATUS  IN  (' ','D+','D','D-') ";

                            if (cboWard.Text.Trim() == "ER")
                            {
                                SQL = SQL + ComNum.VBLF + " AND GBIOE IN ('E','EI') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + " AND (GBIOE NOT IN ('E','EI')  OR GBIOE IS NULL) ";
                            }

                            if (chkNC.Checked == true)
                            {
                                //SQL = SQL + "  AND GBSLIP NOT IN ('A7') ";
                            }

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                panel8.Enabled = true;
                                return;
                            }

                            if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 0)
                            {
                                ssList_Sheet1.Cells[i, 6 + 1].Text = "◆";
                            }
                            else
                            {
                                if (READ_DUTY_PICKUP(dt.Rows[i]["PANO"].ToString().Trim(), ComFunc.LPAD(clsType.User.Sabun, 5, "0"), dtpDate.Value.ToString("yyyy-MM-dd")) == true)
                                {
                                    ssList_Sheet1.Cells[i, 6 + 1].Text = "◇";
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;


                        switch (dt.Rows[i]["GBSTS"].ToString().Trim())
                        {
                            case "1": //"가퇴원"
                            case "2"://"퇴원접수"
                            case "6"://"계산발부"
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 255, 100);
                                break;
                            case "4"://"심 사 중"
                                break;
                            case "5"://"심사완료"
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(128, 255, 255);
                                break;
                            case "7"://"퇴원완료"
                                ssList_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 100, 100);
                                break;
                            default:
                                break;
                        }


                        ssList_Sheet1.Cells[i, 10 + 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();


                        mstrIpdno[i] = dt.Rows[i]["IPDNO"].ToString().Trim();

                        Application.DoEvents();
                    }
                }

                dt.Dispose();
                dt = null;

                panel8.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool READ_DUTY_PICKUP(string argPTNO, string argSabun, string argDate)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDUTY = "";
            string strDate = "";

            string strDay = "";

            string strSTIME = "";
            string strETime = "";

            try
            {
                SQL = "";
                SQL = " SELECT * FROM KOSMOS_PMPA.NUR_SCHEDULE1";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + argSabun + "'";
                SQL = SQL + ComNum.VBLF + " AND YYMM = '" + VB.Left(argDate, 7).Replace("-", "") + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strDUTY = dt.Rows[0]["SCHEDULE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                strDay = VB.Right(argDate, 2);

                strDUTY = ComFunc.MidH(strDUTY, Convert.ToInt32(strDay) * 4 - 3, 4).Trim();

                switch (strDUTY)
                {
                    case "D1":
                        strDate = argDate;
                        strSTIME = strDate + " 07:00:00";
                        strETime = strDate + " 14:59:00";
                        break;
                    case "E1":
                        strDate = argDate;
                        strSTIME = strDate + " 15:00:00";
                        strETime = strDate + " 22:59:00";
                        break;
                    case "N1":
                        strDate = Convert.ToDateTime(argDate).AddDays(1).ToString("yyyy-MM-dd");
                        strSTIME = argDate + " 23:00:00";
                        strETime = strDate + " 06:59:00";
                        break;
                    default:
                        RtnVal = false;
                        return RtnVal;
                }

                SQL = " SELECT ORDERNO FROM KOSMOS_OCS.OCS_IORDER";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PICKUPDATE >= TO_DATE('" + strSTIME + "','YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "   AND PICKUPDATE <= TO_DATE('" + strETime + "','YYYY-MM-DD HH24:MI:SS')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = true;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDrCode, VB.Left(cboDept.Text.Trim(), 2), "", 1, "");
            cboDrCode.SelectedIndex = 0;

            btnSearch.Enabled = true;
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWard.Text.Trim() == "ER")
            {
                chkERPICKUP.Visible = true;
            }
            else
            {
                chkERPICKUP.Visible = false;
            }

            btnSearch_Click(null, null);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strORDERNO = "";

            if (ssView1_Sheet1.RowCount < 2)
            {
                return;
            }

            if (e.Column == 0)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[e.Row, 0].Value) == true)
                {
                    strORDERNO = ssView1_Sheet1.Cells[e.Row, 17].Text.Trim();

                    ChkssView1(e.Row, strORDERNO);
                }
            }
        }

        private void ssView1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ssView1_Sheet1.RowCount == 0)
            {
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                ssView1.ContextMenu = null; //  초기화
                PopupMenu = null;
                PopupMenu = new ContextMenu();
                PopupMenu.Name = ssView1_Sheet1.Cells[e.Row, 23].Text.Trim() + "^^" + ssView1_Sheet1.Cells[e.Row, 3].Text.Trim();
                PopupMenu.MenuItems.Add("수가정보", new System.EventHandler(mnuSuInfo_Click));
                ssView1.ContextMenu = PopupMenu; // 입력
                return;
            }

            if (e.Column == 0)
            {
                return;
            }

            string strORDERNO = ssView1_Sheet1.Cells[e.Row, 17].Text.Trim();
            string strXray_Remark4 = Xray_Remark4_Conv(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"), strORDERNO);

            if (strXray_Remark4 != "")
            {
                ComFunc.MsgBox(strXray_Remark4, "확인");
            }

            if (ssView1_Sheet1.Cells[e.Row, 0].Locked == false)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[e.Row, 0].Value) == true)
                {
                    ssView1_Sheet1.Cells[e.Row, 0].Value = false;
                }
                else
                {
                    ssView1_Sheet1.Cells[e.Row, 0].Value = true;
                    ChkssView1(e.Row, strORDERNO);
                }
            }

            if (ssView1_Sheet1.Cells[e.Row, 13].Text.Trim() == "#")
            {
                txtRemark.Text = ssView1_Sheet1.Cells[e.Row, 15].Text.Trim();
            }

        }
        private void ChkssView1(int intRow, string strORDERNO)
        {
            if (chkDuplicate.Checked == false)
            {
                return;
            }

            for (int i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                if (i != intRow)
                {
                    if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (ssView1_Sheet1.Cells[i, 17].Text.Trim() == strORDERNO)
                        {
                            ComFunc.MsgBox("해당 처방에 대한 변경 내역(DC 또는 추가처방)이 존재합니다."
                                + ComNum.VBLF + "기존 선택된 처방을 먼저 픽업하신 후 다시 픽업하십시요.", "확인");

                            ssView1_Sheet1.Cells[intRow, 0].Value = false;
                        }
                    }
                }
            }
        }

        private void mnuSuInfo_Click(object sender, EventArgs e)
        {
            if (VB.Split(((MenuItem)sender).Parent.Name, "^^").Length != 2)
            {
                ComFunc.MsgBox("오더를 다시 선택해 주세요.");
                return;
            }

            // 프로젝트 권환 안 만들어 줘서 그냥 붙쳐 둠.....
            FrmOrdSugaInfo FrmOrdSugaInfoX = new FrmOrdSugaInfo(VB.Split(((MenuItem)sender).Parent.Name, "^^")[0], VB.Split(((MenuItem)sender).Parent.Name, "^^")[1]);
            FrmOrdSugaInfoX.StartPosition = FormStartPosition.CenterParent;
            FrmOrdSugaInfoX.ShowDialog();

        }

        private string Xray_Remark4_Conv(string argPano, string argDate, string argOrderNo)
        {
            string RtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return RtnVal; //권한 확인

                SQL = "";
                SQL = " SELECT A.REMARK4 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_CODE A , KOSMOS_PMPA.XRAY_DETAIL B";
                SQL = SQL + ComNum.VBLF + "   WHERE PANO ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE  =TO_DATE('" + argDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO  ='" + argOrderNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.XCODE =B.XCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["REMARK4"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void ssView2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }


            if (e.Button == MouseButtons.Right)
            {
                ssView2.ContextMenu = null; //  초기화
                PopupMenu = null;
                PopupMenu = new ContextMenu();
                PopupMenu.Name = ssView2_Sheet1.Cells[e.Row, 24].Text.Trim() + "^^" + ssView2_Sheet1.Cells[e.Row, 3].Text.Trim();
                PopupMenu.MenuItems.Add("수가정보", new System.EventHandler(mnuSuInfo_Click));
                ssView2.ContextMenu = PopupMenu; // 입력
                return;
            }

            string strXray_Remark4 = Xray_Remark4_Conv(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"), ssView2_Sheet1.Cells[e.Row, 19].Text.Trim());

            if (strXray_Remark4 != "")
            {
                ComFunc.MsgBox(strXray_Remark4, "확인");
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            if (e.Column == 14)
            {
                //    FrmPRN상세정보.Show 1
                frmMedPRNDetail frmMedPRNDetailX = new frmMedPRNDetail(ssView1_Sheet1.Cells[e.Row, 18].Text.Trim());
                frmMedPRNDetailX.StartPosition = FormStartPosition.CenterParent;
                frmMedPRNDetailX.ShowDialog();
                frmMedPRNDetailX = null;
            }
            else if (e.Column == 16)
            {
                if (ssView1_Sheet1.Cells[e.Row, 16].Text.Trim() != "")
                {
                    ComFunc.MsgBox(ssView1_Sheet1.Cells[e.Row, 16].Text.Trim(), "확인");
                }
            }
            else
            {
                if (ssView1_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper() == "MED")
                {
                    if (clsBagage.USE_DIF() == true)
                    {
                        //frmSupDrstDifDown frmSupDrstDifDownX = new frmSupDrstDifDown("NRINFO", ssView1_Sheet1.Cells[e.Row, 3].Text.Trim());
                        //frmSupDrstDifDownX.StartPosition = FormStartPosition.CenterParent;
                        //frmSupDrstDifDownX.ShowDialog();
                        //frmSupDrstDifDownX = null;

                        //2019-07-12 전산업무 의뢰서 2019-822
                        frmSupDrstInfoEntryNew f = new frmSupDrstInfoEntryNew(ssView1_Sheet1.Cells[e.Row, 3].Text.Trim(), "NRINFO", "VIEW");
                        f.ShowDialog(this);
                        f = null;
                    }
                }
            }

        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 14 || e.Column == 25)
            {
                if (ssView2_Sheet1.Cells[e.Row, 14].Text.Trim() == "P")
                {
                    //FrmPRN상세정보.Show 1
                    frmMedPRNDetail frmMedPRNDetailX = new frmMedPRNDetail(ssView2_Sheet1.Cells[e.Row, 20].Text.Trim());
                    frmMedPRNDetailX.StartPosition = FormStartPosition.CenterParent;
                    frmMedPRNDetailX.ShowDialog();
                    frmMedPRNDetailX.Dispose();
                    frmMedPRNDetailX = null;
                }
            }
            else if (e.Column == 16)
            {
                if (ssView2_Sheet1.Cells[e.Row, 16].Text.Trim() != "")
                {
                    ComFunc.MsgBox(ssView2_Sheet1.Cells[e.Row, 16].Text.Trim(), "확인");
                }
            }
            else
            {
                #region 전산업무 의뢰서 처리 비급여 팝업(2021-369)
                //24 수가코드
                //3 오더네임
                string GBSELF = ssView2_Sheet1.Cells[e.Row, 13].Text.Trim();
                if (e.Column == 4 && (GBSELF.Equals("1") || GBSELF.Equals("2")))
                {
                    string strSuCode = ssView2_Sheet1.Cells[e.Row, 24].Text.Trim();
                    if (strSuCode != "")
                    {
                        strSuCode = "'" + strSuCode + "'";

                        OracleDataReader reader = null;
                        string SQL = string.Empty;
                        string SqlErr = string.Empty;

                        SQL = " SELECT  1";
                        SQL = SQL + ComNum.VBLF + "  FROM DUAL";
                        SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
                        SQL = SQL + ComNum.VBLF + " (";
                        SQL = SQL + ComNum.VBLF + " SELECT 1";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SUT A";
                        SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_SUN B";
                        SQL = SQL + ComNum.VBLF + "    ON A.SUNEXT = B.SUNEXT";
                        SQL = SQL + ComNum.VBLF + " WHERE ((A.BUN IN ('71','73') AND A.SUGBF = '1') OR B.GBSELFHANG = 'Y')";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT IN (" + strSuCode + ")";
                        SQL = SQL + ComNum.VBLF + " )";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (reader.HasRows)
                        {
                            using (frmUnPaidPopup f = new frmUnPaidPopup(strSuCode))
                            {
                                f.StartPosition = FormStartPosition.CenterParent;
                                f.ShowDialog(this);
                            }
                        }

                        reader.Dispose();
                        reader = null;
                    }
                }
                #endregion

                if (ssView2_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper() == "MED")
                {
                    if (clsBagage.USE_DIF() == true)
                    {
                        frmSupDrstDifDown frmSupDrstDifDownX = new frmSupDrstDifDown("NRINFO", ssView2_Sheet1.Cells[e.Row, 3].Text.Trim());
                        frmSupDrstDifDownX.StartPosition = FormStartPosition.CenterParent;
                        frmSupDrstDifDownX.ShowDialog();
                        frmSupDrstDifDownX.Dispose();
                        frmSupDrstDifDownX = null;
                    }
                }
            }
        }

        private void ssView1_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            string strNowDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strORDERNO = "";
            string strGubun = "";
            string strMessage = "";

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            if (ssView1_Sheet1.RowCount == 0)
            {
                return;
            }

            strGubun = ssView1_Sheet1.Cells[e.Row, 2].Text.Trim();
            strORDERNO = ssView1_Sheet1.Cells[e.Row, 17].Text.Trim();

            if (strGubun != "C/O")
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT FRREMARK, TODRCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ITRANSFER";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + strORDERNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMessage = READ_DR_SCHEDULE(dt.Rows[0]["TODRCODE"].ToString().Trim(), strNowDate)
                    + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["FRREMARK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                e.TipText = strMessage;
                e.WrapText = true;
                e.TipWidth = 800;
                e.ShowTip = true;
                ssView1.TextTipDelay = 10;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string READ_DR_SCHEDULE(string argDrCode, string argDate)
        {
            string RtnVal = "";
            string strAM = "";
            string strPM = "";


            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT GBJIN, GBJIN2, GBJIN3";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_SCHEDULE";
                SQL = SQL + ComNum.VBLF + "  WHERE DRCODE = '" + argDrCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND SCHDATE = TO_DATE('" + argDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strAM = READ_SCHGBN(dt.Rows[0]["GBJIN"].ToString().Trim());
                    strPM = READ_SCHGBN(dt.Rows[0]["GBJIN2"].ToString().Trim());

                    if (strAM != "")
                    {
                        RtnVal = RtnVal + " 오전:" + strAM;
                    }

                    if (strPM != "")
                    {
                        RtnVal = RtnVal + ", 오후:" + strPM;
                    }

                    if (strAM != "" || strPM != "")
                    {
                        RtnVal = ComNum.VBLF + "※ " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, argDrCode) + "과장님 당일 진료 스케쥴 :" + RtnVal;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;

        }

        private string READ_SCHGBN(string strGBJIN)
        {
            string RtnVal = "";

            switch (strGBJIN)
            {
                case "1":
                    RtnVal = "진료";
                    break;
                case "2":
                    RtnVal = "수술";
                    break;
                case "3":
                    RtnVal = "특수검사";
                    break;
                case "4":
                    RtnVal = "휴진";
                    break;
                case "5":
                    RtnVal = "학회";
                    break;
                case "6":
                    RtnVal = "휴가";
                    break;
                case "7":
                    RtnVal = "출장";
                    break;
                case "8":
                    RtnVal = "기타";
                    break;
                case "9":
                    RtnVal = "OFF";
                    break;
                case "A":
                    RtnVal = "협진";
                    break;
                case "B":
                    RtnVal = "출장검진";
                    break;
                case "C":
                    RtnVal = "채용상담";
                    break;
                default:
                    RtnVal = "";
                    break;
            }
            return RtnVal;
        }

        private void ssView2_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            string strNowDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strORDERNO = "";
            string strGubun = "";
            string strMessage = "";

            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }

            strGubun = ssView2_Sheet1.Cells[e.Row, 2].Text.Trim();
            strORDERNO = ssView2_Sheet1.Cells[e.Row, 19].Text.Trim();

            if (strGubun != "C/O")
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT FRREMARK, TODRCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ITRANSFER";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + FstrPtno + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = " + strORDERNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMessage = READ_DR_SCHEDULE(dt.Rows[0]["TODRCODE"].ToString().Trim(), strNowDate)
                    + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["FRREMARK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                e.TipText = strMessage;
                e.WrapText = true;
                e.TipWidth = 800;
                e.ShowTip = true;
                ssView2.TextTipDelay = 10;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            ssView1.SetViewportLeftColumn(0, 0);
            ssView2.SetViewportLeftColumn(0, 0);

            ssListCellClick(e.Row, e.Column);

            

        }

        void READ_VERVAL_DC_ORDER(string argPANO, string argBDATE)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMag = "";

            try
            {
                SQL = " SELECT PTNO ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER A ";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + argBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.PTNO = '" + argPANO + "' ";
                SQL += ComNum.VBLF + "   AND A.GBPICKUP = '*' ";
                SQL += ComNum.VBLF + "   AND (A.ORDERSITE NOT IN('CAN', 'NDC') OR A.ORDERSITE IS NULL) ";
                SQL += ComNum.VBLF + "   AND A.PICKUPSABUN IS NULL ";
                SQL += ComNum.VBLF + "   AND (A.GBIOE NOT IN('E', 'EI') OR A.GBIOE IS NULL) ";
                SQL += ComNum.VBLF + "   AND (((A.GBVERB IS NULL OR A.GBVERB <> 'Y') ";
                SQL += ComNum.VBLF + "          AND A.GBSTATUS IN(' ', 'D+', 'D', 'D-')) ";
                SQL += ComNum.VBLF + "       OR ((A.GBVERB IS NOT NULL OR A.GBVERB = 'Y') ";
                SQL += ComNum.VBLF + "          AND A.GBSTATUS IN('D-'))) ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strMag = "";

                if (dt.Rows.Count > 0)
                {
                    strMag = "OK";
                }

                dt.Dispose();
                dt = null;

                if (strMag != "")
                {
                    ComFunc.MsgBox("============================================= " + ComNum.VBLF + ComNum.VBLF +
                                   "  구두처방 DC 건이 있습니다. " + ComNum.VBLF +
                                   "  화면 왼쪽 환자명단 위에 있는 체크 박스인   " + ComNum.VBLF + ComNum.VBLF + 
                                   "  구두처방 D/C Pickup " + ComNum.VBLF + ComNum.VBLF +
                                   "  을 체크 후 환자를 다시 선택하시면 " + ComNum.VBLF +
                                   "  PICKUP 할 목록이 표시가 됩니다." + ComNum.VBLF +
                                   "============================================= " + ComNum.VBLF + ComNum.VBLF , "구두처방 D/C건 있음");
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        private void ssListCellClick(int intRow, int intCol)
        {
            if (intCol == 13)
            {
                return;
            }

            string strName = "";

            string strHeight = "";
            string strWeight = "";
            string strBSA = "";
            string[] strBSA_array;

            string strCP = "";

            FstrPtno = ssList_Sheet1.Cells[intRow, 1].Text.Trim();
            strName = ssList_Sheet1.Cells[intRow, 2].Text.Trim();
            FstrInDate = ssList_Sheet1.Cells[intRow, 9 + 1].Text.Trim();

            conPatInfo1.SetDisPlay(clsType.User.IdNumber, "I", dtpDate.Value.ToString("yyyy-MM-dd"), FstrPtno, ssList_Sheet1.Cells[intRow, 10 + 1].Text);

            frmPatientInfoX.rGetDate(FstrPtno, mstrIpdno[intRow]);

            lblMsg.BackColor = Color.FromArgb(0, 0, 192);
            lblMsg.Text = "";

            lblBSA.Text = "";
            lblCPNameDay.Text = "";


            #region ComLibB 함수 사용 KMC 2020-12-24

            ////키몸무게 읽어와서 BSA 표시, 키 몸무게는 챠트에서 최신 자료로 가져오기. (2020-12-18)
            //strHeight = clsIpdNr.READ_IPD_HEIGHT(clsDB.DbCon, FstrPtno, FstrInDate);
            //strWeight = clsIpdNr.READ_IPD_WEIGHT(clsDB.DbCon, FstrPtno, FstrInDate);

            //lblBSA.Text = " ■ BSA : " + clsIpdNr.GetBSA(strHeight, strWeight) + "   (키:" + strHeight + "/몸무게:" + strWeight + ")";

            btnCancerWorklist.Visible = false;

            strHeight = ComFunc.READ_IPD_HEIGHT(clsDB.DbCon, FstrPtno, FstrInDate);
            strWeight = ComFunc.READ_IPD_WEIGHT(clsDB.DbCon, FstrPtno, FstrInDate);
            strBSA = ComFunc.GetBSA(strHeight, strWeight);
            if (strBSA != "Error")
            {
                strBSA_array = strBSA.Split('.');
                //2021-01-18 안정수 임시추가
                //02060948 이희태, BSA READ시 strBSA_array length가 1이라서 오류발생하여 임시로 예외처리함
                if (strBSA_array.Length > 1)
                {
                    strBSA = strBSA_array[0] + "." + VB.Left(strBSA_array[1], 2);
                }
            }
            lblBSA.Text = " ■ BSA:" + strBSA + " (키:" + strHeight + "/몸무게:" + strWeight + ")";

            #endregion

            strCP = clsIpdNr.ReadCP_NameDay(clsDB.DbCon, FstrPtno, mstrIpdno[intRow], "1");
            if (strCP != "")
            {
                lblCPNameDay.Text = " ■ CP:" + strCP;
            }

            if (clsVbfunc.GetPreToi(clsDB.DbCon, FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"), "") == "1")
            {
                lblMsg.Text = "퇴원예고자입니다.";
            }
            Read_Orders(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));

            Read_Orders_PICKUP(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));
            //Read_Orders_PICKUP_New(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));

            Read_NoPickup_Chk(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));
            Read_NST_Chk(FstrPtno);
            lblNoSend.Text = CHECK_NOSEND(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));
            READ_ALLERGY_POPUP(FstrPtno);

            lblMsg.Text = (lblMsg.Text != "" ? lblMsg.Text.Trim() + ComNum.VBLF : "") + READ_ALLERGY(FstrPtno);
            if (lblMsg.Text != "")
            {
                lblMsg.Enabled = true;
            }
            panActing.Enabled = true;

            READ_VERVAL_DC_ORDER(FstrPtno, dtpDate.Value.ToString("yyyy-MM-dd"));
            //ssView1_Sheet1.Columns.Get(53).Visible = true;
        }
        private void Read_NST_Chk(string argPano)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMag = "";

            try
            {
                SQL = " SELECT IPDNO, PANO, SNAME, SEX ";
                SQL = SQL + ComNum.VBLF + " AGE, INDATE, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIAGNOSIS, DRSABUN, ";
                SQL = SQL + ComNum.VBLF + " NRSABUN, PMSABUN, DTSABUN, RDATE, ";
                SQL = SQL + ComNum.VBLF + " BDATE, STATUS, COMPLITE, ORDERNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + " AND TRUNC(RDATE)=TRUNC(SYSDATE-6) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMag = dt.Rows[0]["RDATE"].ToString().Trim() + "일자에 NST를 의뢰하였습니다." + ComNum.VBLF;
                }

                dt.Dispose();
                dt = null;

                ComFunc.MsgBox("============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 "  NST 재 의뢰 대상자 입니다.  몸무게를 측정 하십시오 !! " + ComNum.VBLF + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 strMag + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF, "NST 재의뢰 확인");
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Read_NoPickup_Chk(string argPano, string argDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMag = "";

            try
            {
                SQL = "";
                SQL = "SELECT A.ORDERCODE,A.BDATE, A.SUCODE,  A.QTY , A.NAL, B.SUNAMEK   ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER A , KOSMOS_PMPA.BAS_SUN B  ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.BDATE    BETWEEN TO_DATE('" + argDate + "','YYYY-MM-DD')-7";
                SQL = SQL + ComNum.VBLF + "     AND TO_DATE('" + argDate + "','YYYY-MM-DD') -1";
                //2019-02-26
                //SQL = SQL + ComNum.VBLF + "     AND A.GBSEND IN ('*','Z')         ";
                SQL = SQL + ComNum.VBLF + "      AND (  (A.SUCODE NOT IN ('C-OT','C-EN','C-PC') AND A.GBSEND IN ('*','Z')) ";
                SQL = SQL + ComNum.VBLF + "          OR (A.SUCODE IN ('C-OT','C-EN','C-PC') AND A.DRCODE2 IS NOT NULL AND A.GBSEND IN ('*','Z')) )";
                
                
                ////SQL = SQL + ComNum.VBLF + "     AND A.ACCSEND IN (NULL,'Z')         ";
                SQL = SQL + ComNum.VBLF + "     AND A.SUCODE = B.SUNEXT(+) ";
                SQL = SQL + ComNum.VBLF + "     AND A.SLIPNO NOT IN ('A7')  ";
                SQL = SQL + ComNum.VBLF + "     AND (A.GBIOE ='I' OR A.GBIOE IS NULL )             ";
                SQL = SQL + ComNum.VBLF + "ORDER BY  ORDERNO    ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMag = strMag + dt.Rows[i]["BDATE"].ToString().Trim()
                                    + " [" + dt.Rows[i]["ORDERCODE"].ToString().Trim() + "] "
                                    + dt.Rows[i]["SUCODE"].ToString().Trim() + " "
                                    + dt.Rows[i]["QTY"].ToString().Trim() + "*"
                                    + dt.Rows[i]["NAL"].ToString().Trim() + " "
                                    + dt.Rows[i]["SUNAMEK"].ToString().Trim() + ComNum.VBLF;
                    }
                }

                dt.Dispose();
                dt = null;

                ComFunc.MsgBox("============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 "▷OCS 미전송 오더가 있습니다!!" + ComNum.VBLF + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 strMag + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF, "미픽업 지난오더 확인요청");
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string READ_ALLERGY(string argPTNO)
        {
            string RtnVal = "";
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        RtnVal = RtnVal + dt.Rows[i]["REMARK"].ToString().Trim() + ",";
                    }

                    RtnVal = VB.Mid(RtnVal, 1, RtnVal.Length - 1);
                    RtnVal = "해당환자는 " + RtnVal + "에 알러지 반응이 있습니다. *** Order PICK UP시 꼭 확인하십시오***";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void READ_ALLERGY_POPUP(string argPano)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMag = "";

            try
            {
                SQL = "";
                SQL = "SELECT REMARK ,B.NAME INF, A.ENTDATE ,C.KORNAME SANAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ETC_ALLERGY_MST A,KOSMOS_PMPA.BAS_BCODE B, KOSMOS_ADM.INSA_MST C  ";
                SQL = SQL + ComNum.VBLF + "WHERE A.PANO = '" + argPano + "'";
                SQL = SQL + ComNum.VBLF + "     AND B.GUBUN='환자정보_알러지종류'";
                SQL = SQL + ComNum.VBLF + "     AND A.CODE=B.CODE    ";
                SQL = SQL + ComNum.VBLF + "     AND A.SABUN=C.SABUN";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMag = strMag + " " + dt.Rows[i]["REMARK"].ToString().Trim() + dt.Rows[i]["INF"].ToString().Trim() + ComNum.VBLF;
                    }
                }

                dt.Dispose();
                dt = null;

                ComFunc.MsgBox("============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 "▷환자의 알러지 정보가 있습니다." + ComNum.VBLF + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF + ComNum.VBLF +
                                 strMag + ComNum.VBLF +
                                "============================================= " + ComNum.VBLF, "환자의 알러지 정보");
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string CHECK_NOSEND(string argPano, string argBDate)
        {
            string RtnVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT REMARK, GBIOE, BDATE, PICKUPDATE, ORDERCODE, NAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + argBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GBSEND = '*' ";
                SQL = SQL + ComNum.VBLF + "   AND ACCSEND IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["REMARK"].ToString().Trim().IndexOf("구두") > -1)
                    {
                        if (VB.Val(dt.Rows[0]["NAL"].ToString().Trim()) < 0)
                        {
                            RtnVal = "★구두처방 D/C건이 있습니다." + ComNum.VBLF + "★'구두처방 D/C PICKUP' 체크 후 조회하시기 바랍니다.";
                        }
                        else
                        {
                            RtnVal = "★의사가 확인하지 않은 구두처방이 있습니다."
                            + ComNum.VBLF + "★" + dt.Rows[0]["REMARK"].ToString().Trim()
                            + " : " + Read_OrderName(dt.Rows[0]["ORDERCODE"].ToString().Trim());
                        }

                    }
                    else if (dt.Rows[0]["GBIOE"].ToString().Trim() == "EI" && dt.Rows[0]["PICKUPDATE"].ToString().Trim() == "")
                    {
                        RtnVal = "★ 응급실 입원 처방 중 픽업하지 않은 처방이 있습니다.";
                    }
                    else if (dt.Rows[0]["PICKUPDATE"].ToString().Trim() == "")
                    {
                        RtnVal = "★ 픽업하지 않은 처방이 있습니다.";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private string Read_OrderName(string argCode)
        {
            string RtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT ORDERNAME FROM KOSMOS_OCS.OCS_ORDERCODE ";
                SQL = SQL + " WHERE ORDERCODE = '" + argCode.Trim() + "'   ";
                SQL = SQL + "   AND (SENDDEPT <> 'N' OR SENDDEPT IS NULL) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return RtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    RtnVal = dt.Rows[0]["ORDERNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return RtnVal;
        }

        private void lblMsg_TextChanged(object sender, EventArgs e)
        {
            if (lblMsg.Text.IndexOf(ComNum.VBLF) > -1)
            {
                lblMsg.Font = new Font("맑은 고딕", 9, FontStyle.Regular);
            }
            else
            {
                lblMsg.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            }
        }

        private void ssViewM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ssViewM_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Column != 0)
            {

                if (ssViewM_Sheet1.Cells[e.Row, 0].Locked == false)
                {
                    if (Convert.ToBoolean(ssViewM_Sheet1.Cells[e.Row, 0].Value) == true)
                    {
                        ssViewM_Sheet1.Cells[e.Row, 0].Value = false;
                    }
                    else
                    {
                        ssViewM_Sheet1.Cells[e.Row, 0].Value = true;
                    }
                }

            }

            if (ssViewM_Sheet1.Cells[e.Row, 13].Text.Trim() == "#")
            {

                txtRemarkM.Text = ssViewM_Sheet1.Cells[e.Row, 15].Text.Trim();
            }

        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            ComFunc.MsgBox(clsBagage.readAllergyInfo(clsDB.DbCon, FstrPtno), "알러지 등록 정보");
        }

        private void frmPickUp_Activated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void frmPickUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void cboJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void chkVerval_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void chkTransfor_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void chkERPICKUP_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void cboDrCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        private void chkVerval_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ssList_Sheet1.ActiveRowIndex != -1)
            {
                ssListCellClick(ssList_Sheet1.ActiveRowIndex, 0);
            }
        }

        private void chkNC_CheckedChanged(object sender, EventArgs e)
        {
            if (ssList_Sheet1.ActiveRowIndex != -1)
            {
                ssListCellClick(ssList_Sheet1.ActiveRowIndex, 0);
            }
        }

        //TODO 환자 정보창에 추가 되기 전까지 이용함 - 한곳에서 지우기 위해 여기서 다 선언 함.. - 영록
        //로드 부분과 환자 선택 부분에 각각 frmPatientInfoX.rGetDate() 사용
        //pSubFormToControl는 로드 부분에서 사용

        frmPatientInfo frmPatientInfoX = new frmPatientInfo();


        private void pSubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();

        }

        private void btnAntiView_Click(object sender, EventArgs e)
        {
            FrmAntiView FrmAntiViewX = new FrmAntiView(cboWard.Text.Trim());
            FrmAntiViewX.StartPosition = FormStartPosition.CenterParent;
            FrmAntiViewX.WindowState = FormWindowState.Maximized;
            FrmAntiViewX.ShowDialog();
            FrmAntiViewX = null;
        }

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (tabControl1.SelectedTabIndex == 2)
            {
                tabControl1.SelectedTabIndex = 0;

                FrmAntiView FrmAntiViewX = new FrmAntiView(cboWard.Text.Trim());
                FrmAntiViewX.StartPosition = FormStartPosition.CenterParent;
                FrmAntiViewX.WindowState = FormWindowState.Maximized;
                FrmAntiViewX.ShowDialog();
                FrmAntiViewX = null;
            }
        }

        private void btnCancerWorklist_Click(object sender, EventArgs e)
        {
            frmWorkListAntiCancer frmWorkListAntiCancerX = new frmWorkListAntiCancer(FstrPtno, "I");
            frmWorkListAntiCancerX.StartPosition = FormStartPosition.CenterParent;
            frmWorkListAntiCancerX.ShowDialog();
            frmWorkListAntiCancerX.Dispose();
            frmWorkListAntiCancerX = null;
            
        }
    }
}
