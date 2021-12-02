using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmJusaLabel_ER.cs
    /// Description     : 응급실용 주사라벨 인쇄
    /// Author          : 유진호
    /// Create Date     : 2018-05-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\FrmJusaLabel_ER.frm(FrmJusaLabel_ER.frm) >> frmJusaLabel_ER.cs 폼이름 재정의" />
    /// 
    public partial class frmJusaLabel_ER : Form, MainFormMessage
    {
        private bool bolSort = false;
        private string GstrWardCode = "";

        private string strFlag = "";
        private string strRoom = "";
        private string strPano = "";
        private string strSName = "";
        private string strJusaName = "";
        private string strDosage = "";
        private string strIMIV = "";
        private string strORDERCODE = "";
        private string strUnit = "";
        private string strDivQty = "";
        //private string strBDATE = "";
        private string strROWID = "";

        private string strUnitNew1 = "";
        private string strUnitNew2 = "";
        private string strUnitNew3 = "";
        private string strUnitNew4 = "";

        private string strTEMP1 = "";
        private string strTEMP2 = "";
        private string strREMARK = "";
        private string strGBGROUP = "";
        private string strJung = "";

        string strInfo1 = "";
        string strInfo2 = "";

        #region //MainFormMessage
        //string mPara1 = "";
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

        public frmJusaLabel_ER()
        {
            InitializeComponent();
        }

        public frmJusaLabel_ER(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmJusaLabel_ER(string strWardCode)
        {
            InitializeComponent();
            GstrWardCode = strWardCode;
        }

        public frmJusaLabel_ER(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            GstrWardCode = sPara1;
        }
       
        private void frmJusaLabel_ER_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");


            ss1_Sheet1.Columns[20].Visible = false;
            ss1_Sheet1.Columns[21].Visible = false;
            ss1_Sheet1.Columns[22].Visible = false;
            ss1_Sheet1.Columns[23].Visible = false;
            ss1_Sheet1.Columns[24].Visible = false;
            ss1_Sheet1.Columns[28].Visible = false;
            ss1_Sheet1.Columns[29].Visible = false;
            ss1_Sheet1.Columns[30].Visible = false;
            if(clsType.User.Sabun == "21403")
            {
                ss1_Sheet1.Columns[20].Visible = true;
                ss1_Sheet1.Columns[21].Visible = true;
                ss1_Sheet1.Columns[22].Visible = true;
                ss1_Sheet1.Columns[23].Visible = true;
                ss1_Sheet1.Columns[24].Visible = true;
                ss1_Sheet1.Columns[28].Visible = true;
                ss1_Sheet1.Columns[29].Visible = true;
                ss1_Sheet1.Columns[30].Visible = true;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            Get_Ward();

            txtPano.Text = "";
            lblSname.Text = "";

            if (clsType.User.Sabun != "4349")
            {
                cboWard.Enabled = false;
            }

            lblX.Text = "마진(X): " + clsType.PC_CONFIG.nx;
            lblY.Text = "마진(Y): " + clsType.PC_CONFIG.nY;

            if (cboWard.Text.Trim() == "EM" || cboWard.Text.Trim() == "ER")
            {
                chkER.Checked = true;
            }
        }

        private void frmJusaLabel_ER_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmJusaLabel_ER_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void Get_Ward()
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT WardCode, WardName  ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_WARD ";
                SQL += ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                        cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    }

                    cboWard.Items.Add("SICU");
                    cboWard.Items.Add("MICU");
                    cboWard.Items.Add("ER");
                }
                dt.Dispose();
                dt = null;

                for (i = 0; i < cboWard.Items.Count; i++)
                {
                    if (GstrWardCode == cboWard.Items[i].ToString().Trim())
                    {
                        cboWard.SelectedIndex = i;
                        break;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetSearchData();
        }

        private void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strUnit = "";
            int nL = 0;
            Single nBContents = 0;
            Single nContents = 0;
            int i = 0;
            int nDiv = 0;
            double nDivQty = 0;

            double nUnit1 = 0;
            double nUnit2 = 0;
            double nUnit3 = 0;

            double nUnitNew1 = 0;
            string strUnitNew2 = "";
            string strUnitNew3 = "";
            double nUnitNew4 = 0;

            string strTEMP1 = "";
            string strTEMP2 = "";
            string strTEMP3 = "";
            string strTEMP4 = "";

            string strBUN = "";     //2018-12-28

            string strTemp3 = "";

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (cboWard.Text.Trim() == "EM" || cboWard.Text.Trim() == "ER") //~1
                {
                    SQL = " SELECT 'ER' WARDCODE,  '100' RoomCode,   O.PTno PANO,       P.SName,      P.Sex,       ";
                    SQL += ComNum.VBLF + "        O.DeptCode,   O.SlipNo,     O.OrderCode, ";
                    SQL += ComNum.VBLF + "        C.OrderName,  C.OrderNames, C.DispHeader, O.Contents,  ";
                    SQL += ComNum.VBLF + "        O.BContents,  O.RealQty,    O.DosCode,    D.DosName,  D.DosFullCode,   D.GBDIV,  ";
                    SQL += ComNum.VBLF + "        O.GbGroup,    O.Remark,     O.GbInfo,     O.GbTFlag,   ";
                    SQL += ComNum.VBLF + "        O.GbPRN,      O.GbOrder,    O.OrderSite,  O.OrderNo,   ";
                    SQL += ComNum.VBLF + "        O.GbPort,     N.DrName,     O.SUCODE,     O.Qty,  O.BUN ,O.nal, O.ROWID, ";
                    SQL += ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4,    TO_CHAR(O.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE ";
                    SQL += ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
                    SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                    SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S                            ";
                    SQL += ComNum.VBLF + " WHERE  O.BDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                    //'등록번호 조회
                    if (lblSname.Text != "") SQL += ComNum.VBLF + " AND O.PTNO = '" + txtPano.Text + "' ";


                    if (rdoIMIV3.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND  O.Bun IN ('20','23','12') ";
                        SQL += ComNum.VBLF + " AND  D.DosfullCode = 'Inhal' ";           //'INHAL
                    }
                    else if (rdoIMIV6.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND  O.Bun IN ('11','12') ";              //'경구약도 뽑길 원한다....2017-09-19 add
                        SQL += ComNum.VBLF + " AND  O.GBTFLAG NOT IN ('T') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND  O.Bun IN ('20','23') ";
                    }
                    SQL += ComNum.VBLF + " AND O.GBIOE IN ('E','EI')";
                    if (VB.Trim(cboWard.Text) == "EM")
                    {
                        SQL += ComNum.VBLF + " AND  O.OrderSite  Not In ('TEL','CAN','DC0','DC1','DC2')  ";
                    }
                    SQL += ComNum.VBLF + " AND  (O.GbPRN IN  NULL OR O.GbPRN <> 'P') ";


                    if (chkER.Checked == false)
                    {
                        if (rdoIMIV3.Checked == true)
                        {
                        }
                        else
                        {
                            if (rdoGB0.Checked == true)
                            {
                                SQL += ComNum.VBLF + "  AND D.GBDIV IN ('1','2','3','4') ";  //'AM
                                SQL += ComNum.VBLF + "   AND D.DOSCODE NOT IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%1time%')";
                            }
                            else if (rdoGB1.Checked == true)
                            {
                                SQL += ComNum.VBLF + "  AND D.GBDIV IN ('3','4')  "; //' PM
                            }
                            else if (rdoGB2.Checked == true)
                            {
                                SQL += ComNum.VBLF + "  AND D.GBDIV IN ('2','3','4')  "; //'HS
                            }
                            else if (rdoGB3.Checked == true)
                            {
                                SQL += ComNum.VBLF + "  AND D.GBDIV ='4' ";           //'4AM(6AM)
                            }
                            else if (rdoGB4.Checked == true)              //'1TIME
                            {
                                SQL += ComNum.VBLF + "  AND D.GBDIV IN ('1','2','3','4') ";  //'AM
                                SQL += ComNum.VBLF + "   AND D.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%1time%')";
                            }
                            else if (rdoGB5.Checked == true)    //'BID add
                            {
                                SQL += ComNum.VBLF + "  AND D.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%bid%')";
                            }
                            else if (rdoGB6.Checked == true)    //'TID add
                            {
                                SQL += ComNum.VBLF + "  AND D.DOSCODE  IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%tid%')";
                            }
                            else if (rdoGB7.Checked == true)    //'QID add
                            {
                                SQL += ComNum.VBLF + "  AND D.DOSCODE  IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%qid%')";
                            }
                            else if (rdoGB8.Checked == true)    //'QD add
                            {
                                SQL += ComNum.VBLF + "  AND D.DOSCODE  IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%qd%')";
                            }
                        }

                        if (rdoIMIV0.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTr(O.DosCode,1,2) IN ('91')  ";       //'IM'IM
                        if (rdoIMIV1.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTR(O.DosCode,1,2) IN ('92')  ";       //'IV
                        if (rdoIMIV2.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTr(O.DosCode,1,2) IN ('95')  ";       //'SC
                        if (rdoIMIV3.Checked == true) SQL += ComNum.VBLF + " AND  D.DosfullCode = 'Inhal' ";                //'INHAL
                        if (rdoIMIV4.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTR(O.DosCode,1,2) IN ('93')  ";       //'IV(m) add
                        if (rdoIMIV5.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTR(LOWER(DOSNAME),1,6) ='others'  ";  //'Others add
                    }


                    //'인쇄여부
                    if (rdoPrt0.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND (O.LABELPRINT IS NULL OR  O.LABELPRINT = '') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND O.LABELPRINT = '*' ";
                    }
                    SQL += ComNum.VBLF + " AND    O.SUCODE NOT IN ('NSA-CT') ";
                    SQL += ComNum.VBLF + " AND    O.GbPRN <>'S' ";  //'jjy 추가(2000/05/22 'S는 선수납(선불)
                    SQL += ComNum.VBLF + " AND   (O.GbStatus    = ' ' OR O.GbStatus IS NULL)    ";
                    SQL += ComNum.VBLF + " AND   (O.GbStatus  <> 'D' AND O.GbStatus <> 'D-')    ";
                    SQL += ComNum.VBLF + " AND    O.Ptno       =  P.Pano(+)        ";
                    SQL += ComNum.VBLF + " AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL += ComNum.VBLF + " AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL += ComNum.VBLF + " AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)   ";
                    SQL += ComNum.VBLF + " AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL += ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL += ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL += ComNum.VBLF + " ORDER  BY  P.SName,O.PTno,  O.DOSCODE,O.SlipNo, O.SeqNo     ";
                }
                else
                {
                    SQL = " ";
                    SQL = SQL + " SELECT M.WARDCODE,   M.RoomCode,   M.Pano,       M.SName,      M.Sex,       ";
                    SQL += ComNum.VBLF + "        M.Age,        M.DeptCode,   O.SlipNo,     O.OrderCode, ";
                    SQL += ComNum.VBLF + "        C.OrderName,  C.OrderNames, C.DispHeader, O.Contents,  ";
                    SQL += ComNum.VBLF + "        O.BContents,  O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  ";
                    SQL += ComNum.VBLF + "        O.GbGroup,    O.Remark,     O.GbInfo,     O.GbTFlag,   ";
                    SQL += ComNum.VBLF + "        O.GbPRN,      O.GbOrder,    O.OrderSite,  O.OrderNo,   ";
                    SQL += ComNum.VBLF + "        O.GbPort,     N.DrName,     O.SUCODE,     O.Qty,  O.BUN ,O.nal, O.ROWID, ";
                    SQL += ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4,    TO_CHAR(O.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE ";
                    SQL += ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
                    SQL += ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER  M,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
                    SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
                    SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S                            ";
                    SQL += ComNum.VBLF + " WHERE  O.BDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";

                    //'등록번호 조회
                    if (lblSname.Text != "") SQL += ComNum.VBLF + " AND O.PTNO = '" + txtPano.Text + "' ";

                    if (rdoIMIV0.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTr(O.DosCode,1,2) IN ('91')  ";   //'IM'IM
                    if (rdoIMIV1.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTR(O.DosCode,1,2) IN ('92')  ";   //'IV
                    if (rdoIMIV2.Checked == true) SQL += ComNum.VBLF + " AND  SUBSTr(O.DosCode,1,2) IN ('95')  ";   //'SC
                    if (rdoIMIV3.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND  D.DosfullCode = 'Inhal' ";           // 'INHAL
                        SQL += ComNum.VBLF + " AND  O.Bun IN ('12') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND  O.Bun IN ('20','23') "; //'ICU 로엘수녀님 요청 사항

                    }
                    if (cboWard.Text.Trim() == "EM") SQL += ComNum.VBLF + " AND  O.OrderSite  Not In ('TEL','CAN','DC0','DC1','DC2')  ";
                    SQL += ComNum.VBLF + " AND  (O.GbPRN IN  NULL OR O.GbPRN <> 'P') ";



                    if (rdoGB0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND D.GBDIV IN ('1','2','3','4','5','6') "; // 'AM
                        SQL += ComNum.VBLF + "   AND D.DOSCODE NOT IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%1time%')";
                    }
                    else if (rdoGB1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND D.GBDIV IN ('3','4')  "; //' PM
                    }
                    else if (rdoGB2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND D.GBDIV IN ('2','3','4')  "; //'HS
                    }
                    else if (rdoGB3.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND D.GBDIV ='4' ";           //'4AM(6AM)
                    }
                    else if (rdoGB4.Checked == true)              //'1TIME
                    {
                        SQL += ComNum.VBLF + "  AND D.GBDIV IN ('1','2','3','4') ";  //'AM
                        SQL += ComNum.VBLF + "   AND D.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE LOWER(DOSNAME) LIKE '%1time% ')";
                    }



                    //'인쇄여부
                    if (rdoPrt0.Checked == true)
                    {
                        SQL += ComNum.VBLF + " AND (O.LABELPRINT IS NULL OR  O.LABELPRINT = '') ";

                    }
                    else
                    {
                        SQL += ComNum.VBLF + " AND O.LABELPRINT = '*' ";

                    }

                    SQL += ComNum.VBLF + " AND    O.GbPRN <>'S' ";  //'jjy 추가(2000/05/22 /'S는 선수납(선불)
                    SQL += ComNum.VBLF + " AND   (O.GbStatus    = ' ' OR O.GbStatus IS NULL)    ";
                    SQL += ComNum.VBLF + " AND   (O.GbStatus  <> 'D' AND O.GbStatus <> 'D-')    ";
                    SQL += ComNum.VBLF + " AND    O.Ptno       =  M.Pano           ";

                    if (cboWard.Text.Trim() == "MICU")
                    {
                        SQL += ComNum.VBLF + " AND M.WARDCODE ='IU'";
                        SQL += ComNum.VBLF + " AND M.ROOMCODE ='234'";
                    }
                    else
                    {
                        if (cboWard.Text.Trim() == "SICU")
                        {
                            SQL += ComNum.VBLF + " AND M.WARDCODE ='IU'";
                            SQL += ComNum.VBLF + " AND M.ROOMCODE ='233'";
                        }
                        else if (cboWard.Text.Trim() != "ER")
                        {
                            if (cboWard.Text.Trim() == "IQ" || cboWard.Text.Trim() == "ND" || cboWard.Text.Trim() == "NR")
                            {
                                SQL += ComNum.VBLF + " AND  M.WardCode IN ('IQ','ND','NR')";
                            }
                            else
                            {
                                SQL += ComNum.VBLF + " AND  M.WardCode = '" + cboWard.Text.Trim() + "' ";
                            }
                        }
                    }

                    SQL += ComNum.VBLF + " AND    O.SUCODE NOT IN ('NSA-CT') ";
                    SQL += ComNum.VBLF + " AND    M.GBSTS IN ('0','2')              ";
                    SQL += ComNum.VBLF + " AND    M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.OUTDATE IS NULL";
                    SQL += ComNum.VBLF + " AND    O.Ptno       =  P.Pano(+)        ";
                    SQL += ComNum.VBLF + " AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL += ComNum.VBLF + " AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL += ComNum.VBLF + " AND    (C.SendDept  !=  'N' OR C.SendDept IS NULL)   ";
                    SQL += ComNum.VBLF + " AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL += ComNum.VBLF + " AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL += ComNum.VBLF + " AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL += ComNum.VBLF + " ORDER  BY  M.RoomCode, M.Pano,  O.DOSCODE,O.SlipNo, O.SeqNo     ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nUnitNew1 = VB.Val(dt.Rows[i]["UNITNEW1"].ToString().Trim());
                        nBContents = (float)VB.Val(dt.Rows[i]["BContents"].ToString().Trim());
                        strUnitNew2 = (dt.Rows[i]["UNITNEW2"].ToString().Trim());
                        strUnitNew3 = (dt.Rows[i]["UNITNEW3"].ToString().Trim());
                        nUnitNew4 = VB.Val(dt.Rows[i]["UNITNEW4"].ToString().Trim());
                        
                        if (nUnitNew4 == 0)
                        {
                            nUnitNew4 = nUnitNew1;
                        }
                        nContents = (float)VB.Val(dt.Rows[i]["Contents"].ToString().Trim());

                        strBUN = dt.Rows[i]["BUN"].ToString().Trim();       //2018-12-28

                        nUnit1 = 0;
                        nUnit2 = 0;
                        nUnit3 = 0;

                        if (dt.Rows[i]["Pano"].ToString().Trim().Equals("11587213"))
                        {
                            i = i;
                        }

                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = READ_DRINFO_NAME(dt.Rows[i]["SUCODE"].ToString().Trim(), "1");

                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();

                        strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        nL = VB.I(strUnit.Trim(), "/");
                        strUnit = VB.Pstr(strUnit.Trim(), "/", nL).Trim();

                        switch (strUnit)
                        {
                            case "A":
                                strUnit = "ⓐ";
                                break;
                            case "T":
                                strUnit = "ⓣ";
                                break;
                            case "V":
                                strUnit = "ⓥ";
                                break;
                            case "BT":
                                strUnit = "ⓑ";
                                break;
                        }

                        nDiv = (int)VB.Val(dt.Rows[i]["GBDIV"].ToString().Trim());
                        nDiv = (int)VB.IIf(nDiv == 0, 1, nDiv);

                        if (nBContents == nContents)
                        {
                            nDivQty = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) / nDiv * nUnitNew1;
                        }
                        else
                        {
                            nDivQty = VB.Val(dt.Rows[i]["Contents"].ToString().Trim()) / nDiv;
                        }

                        nDivQty = VB.Fix((int)(nDivQty + 0.05) * 10) / 10;

                        ss1_Sheet1.Cells[i, 8].Text = nDivQty + " " + dt.Rows[i]["UNITNEW2"].ToString().Trim() + " /" +
                            VB.Left(strUnit.Trim() + VB.Space(3), 3);

                        if (dt.Rows[i]["REALQTY"].ToString().Trim() != "1" &&
                            dt.Rows[i]["BContents"].ToString().Trim() != dt.Rows[i]["Contents"].ToString().Trim())
                        {
                            ss1_Sheet1.Cells[i, 8].Text = "오류";
                        }

                        switch (VB.Left(dt.Rows[i]["DosCode"].ToString().Trim(), 2))
                        {
                            case "91":
                                ss1_Sheet1.Cells[i, 9].Text = "IM";
                                break;
                            case "95":
                                ss1_Sheet1.Cells[i, 9].Text = "SC";
                                break;
                            case "92":
                                ss1_Sheet1.Cells[i, 9].Text = "IV";
                                break;
                            case "55":
                                ss1_Sheet1.Cells[i, 9].Text = "흡입";
                                break;
                        }

                        ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BContents"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Contents"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["REALQTY"].ToString().Trim();

                        ss1_Sheet1.Cells[i, 14].Text = nDiv.ToString();

                        ss1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["UNITNEW1"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["UNITNEW2"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 17].Text = dt.Rows[i]["UNITNEW3"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 18].Text = dt.Rows[i]["UNITNEW4"].ToString().Trim();

                        if (nBContents == nContents)
                        {
                            nUnit1 = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) / nDiv * nUnitNew1; // 1회투여량
                        }
                        else
                        {
                            nUnit1 = nContents / nDiv; //1회투여량
                        }


                        if (rdoIMIV6.Checked == false)
                        {
                            nUnit2 = nUnit1 / nUnitNew1; //단위부피
                            nUnit3 = nUnit2 * nUnitNew4; //단위부피

                            #region Nan 발생시 값 강제 처리 2021-09-01
                            //if (double.IsNaN(nUnit2))
                            //{
                            //    nUnit2 = nUnitNew1;
                            //}

                            //if (double.IsNaN(nUnit3))
                            //{
                            //    nUnit3 = nUnitNew4;
                            //}
                            #endregion

                            nUnit1 = VB.Fix((int)((nUnit1 + 0.005) * 100)) / 100.0;
                            nUnit2 = VB.Fix((int)((nUnit2 + 0.005) * 100)) / 100.0;
                            nUnit3 = VB.Fix((int)((nUnit3 + 0.005) * 100)) / 100.0;

                            if (VB.Val(dt.Rows[i]["UNITNEW4"].ToString().Trim()) == 0)
                            {
                                ss1_Sheet1.Cells[i, 19].Text = nUnit1 + strUnitNew2 + "/ (" + nUnit2 + strUnitNew3 + ")";
                            }
                            else
                            {
                                ss1_Sheet1.Cells[i, 19].Text = nUnit1 + strUnitNew2 + " /" + nUnit3 + "ml(" + nUnit2 + strUnitNew3 + ")";
                            }
                        }
                        else
                        {
                            if (strBUN == "11")     //2018-12-28 내복약의 경우 실수량에 div 로 나눠서 1회분만 표기, 나머지 분류는 일단 제외
                            {
                                ss1_Sheet1.Cells[i, 19].Text = nUnit1 + strUnitNew2 + " / (" + ((double)VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) / nDiv) + strUnitNew3 + ") " + READ_DIV_NAME(dt.Rows[i]["DosCode"].ToString().Trim());
                            }
                            else
                            {
                                ss1_Sheet1.Cells[i, 19].Text = nUnit1 + strUnitNew2 + " / (" + dt.Rows[i]["REALQTY"].ToString().Trim() + strUnitNew3 + ") " + READ_DIV_NAME(dt.Rows[i]["DosCode"].ToString().Trim()); ;
                            }
                          
                            
                        }

                        ss1_Sheet1.Cells[i, 20].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 21].Text = dt.Rows[i]["DosCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 22].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 23].Text = dt.Rows[i]["DosFullCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 24].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        strTEMP1 = dt.Rows[i]["DosCode"].ToString().Trim();
                        strTEMP2 = dt.Rows[i]["DOSNAME"].ToString().Trim();
                        strTEMP3 = dt.Rows[i]["DosFullCode"].ToString().Trim();
                        strTEMP4 = dt.Rows[i]["GBDIV"].ToString().Trim();

                        switch (strTEMP4)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                                if (VB.L(VB.LCase(strTEMP2), "1time") > 1)
                                {
                                    strInfo1 = "1Time";
                                }
                                else if (VB.L(VB.LCase(strTEMP2), "bid") > 1)
                                {
                                    strInfo1 = "BID";
                                }
                                else if (VB.L(VB.LCase(strTEMP2), "tid") > 1)
                                {
                                    strInfo1 = "TID";
                                }
                                else if (VB.L(VB.LCase(strTEMP2), "qid") > 1)
                                {
                                    strInfo1 = "QID";
                                }
                                else if (VB.L(VB.LCase(strTEMP2), "qd") > 1)
                                {
                                    strInfo1 = "QD";
                                }
                                else
                                {
                                    strInfo1 = "AM";
                                }
                                break;
                            default:
                                break;
                        }

                        ss1_Sheet1.Cells[i, 25].Text = strInfo1;

                        switch (VB.Left(strTEMP1, 2))
                        {
                            case "91":
                                strInfo2 = "IM";
                                break;
                            case "92":
                                strInfo2 = "IV";
                                break;
                            case "93":
                                strInfo2 = "IV(m)";
                                break;
                            case "95":
                                strInfo2 = "SC";
                                break;
                        }

                        if (strTemp3 == "Inhal") strInfo2 = "Inhal";
                        if (VB.Mid(strTEMP2, 1, 6) == "others") strInfo2 = "others";

                        ss1_Sheet1.Cells[i, 26].Text = strInfo2;
                        ss1_Sheet1.Cells[i, 27].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 28].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 29].Text = dt.Rows[i]["BUN"].ToString().Trim();     //2018-12-28
                        ss1_Sheet1.Cells[i, 31].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();     //2018-12-28
                    }
                }
                Cursor.Current = Cursors.Default;
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


        string READ_DRINFO_NAME(string argSUNEXT, string Gbn = "")
        {
            if (argSUNEXT.Trim() == "JAGA") return "";

            string returnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT  HNAME, ENAME, SNAME ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_NEW ";
                SQL += ComNum.VBLF + "WHERE SUNEXT ='" + argSUNEXT.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                if (Gbn == "1")
                {
                    returnVal = dt.Rows[0]["hName"].ToString().Trim();
                }
                else if (Gbn == "2")
                {
                    returnVal = dt.Rows[0]["eName"].ToString().Trim();
                }
                else if (Gbn == "3")
                {
                    returnVal = dt.Rows[0]["sName"].ToString().Trim();
                }

                returnVal = READ_ATTENTION(argSUNEXT) + returnVal;

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
            return returnVal;
        }

        private String READ_DIV_NAME(string argDOSCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "SELECT DECODE(GBDIV, '1', 'QD', '2', 'BID', '3', 'TID', '4', 'QID') DIV ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ODOSAGE ";
                SQL += ComNum.VBLF + " WHERE DOSCODE = '" + argDOSCODE + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DIV"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(ex.Message);
                }
                return rtnVal;
            }

        }

        private string READ_ATTENTION(string strSuCode)
        {
            string strTemp = "";

            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, strSuCode) == "OK")
            {
                strTemp = strTemp + "★항혈";
            }

            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, strSuCode) == "OK")
            {
                strTemp = strTemp + "<!>";
            }

            if (READ_SUGA_IMMUNITY_INHIBITOR(strSuCode) == "OK")
            {
                strTemp = strTemp + "Ω면역";
            }

            return strTemp;
        }

        /// <summary>
        /// READ_SUGA_면역억제제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSuCode"></param>
        /// <returns></returns>
        private string READ_SUGA_IMMUNITY_INHIBITOR(string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "NO";

            try
            {
                SQL = "";
                SQL = "SELECT * FROM " + ComNum.DB_ERP + "DRUG_SPECIAL_JEPCODE  ";
                SQL = SQL + ComNum.VBLF + "     WHERE SEQNO = 7 ";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (chkSungbun.Checked == true)
            {
                SET_PRINT_NEW();
            }
            else
            {
                SET_PRINT_OLD();
            }
        }

        private void SET_PRINT_NEW()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string mstrPrintName = "혈액환자정보";

            //if (clsPrint.gGetPrinterFind(mstrPrintName) == false)
            //{
            //    ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.", "혈액환자정보");
            //    return;
            //}

            for (i = 0; i < ss1_Sheet1.NonEmptyRowCount; i++)
            {
                strFlag = ss1_Sheet1.Cells[i, 0].Text;
                strRoom = ss1_Sheet1.Cells[i, 2].Text;
                strPano = ss1_Sheet1.Cells[i, 3].Text;
                strSName = ss1_Sheet1.Cells[i, 4].Text;
                strORDERCODE = ss1_Sheet1.Cells[i, 5].Text;
                strJusaName = ss1_Sheet1.Cells[i, 6].Text;
                strDosage = ss1_Sheet1.Cells[i, 7].Text;
                strIMIV = ss1_Sheet1.Cells[i, 9].Text;
                strUnit = ss1_Sheet1.Cells[i, 10].Text;

                strUnitNew1 = ss1_Sheet1.Cells[i, 15].Text;
                strUnitNew2 = ss1_Sheet1.Cells[i, 16].Text;
                strUnitNew3 = ss1_Sheet1.Cells[i, 17].Text;
                strUnitNew4 = ss1_Sheet1.Cells[i, 18].Text;
                strDivQty = ss1_Sheet1.Cells[i, 19].Text;
                strROWID = ss1_Sheet1.Cells[i, 20].Text;

                strTEMP1 = ss1_Sheet1.Cells[i, 25].Text;
                strTEMP2 = ss1_Sheet1.Cells[i, 26].Text;
                strREMARK = ss1_Sheet1.Cells[i, 27].Text;
                strGBGROUP = ss1_Sheet1.Cells[i, 28].Text;

                try
                {
                    SQL = " SELECT ERPATIENT as ERPAT ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='ER' ";
                    SQL = SQL + ComNum.VBLF + "   AND BDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        switch (dt.Rows[0]["ERPAT"].ToString().Trim())
                        {
                            case "T":
                                strJung = "TRA";
                                break;
                            case "C":
                                strJung = "CVA";
                                break;
                            case "A":
                                strJung = "ACS";
                                break;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    return;
                }



                if (strFlag == "True")
                {
                    BARCODE_PRINT_NEW(mstrPrintName);
                    Ocs_Update(strROWID);
                }
            }
        }

        private void BARCODE_PRINT_NEW(string strPrintName)
        {            
            string strPrintName1 = "";
            string strPrintName2 = "";
            clsPrint CP = new clsPrint();

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(strPrintName.ToUpper());

                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 의료정보과에 연락바랍니다.");
                    return;
                }

                PrintDocument pd = new PrintDocument();
                PrintController pc = new StandardPrintController();
                pd.PrintController = pc;
                pd.PrinterSettings.PrinterName = strPrintName2;
                if (chkNew.Checked == true)
                {
                    pd.PrintPage += new PrintPageEventHandler(ePrintPage1_202105);
                }
                else
                {
                    if (chkBackColor.Checked == true)
                    {
                        pd.PrintPage += new PrintPageEventHandler(ePrintPage1_NEW);
                    }
                    else
                    {
                        pd.PrintPage += new PrintPageEventHandler(ePrintPage1);
                    }
                }
                pd.Print();    //프린트             
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }
        }

        void ePrintPage1(object sender, PrintPageEventArgs ev)
        {
            clsVbfunc CV = new clsVbfunc();
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            Font printFont;
            string sFont = "굴림체";
            //int nSize = 12;
            int nX = 0;
            int nY = 0;
            string s;


            printFont = new Font(sFont, 12, FontStyle.Regular);
            if (strJung != "")
            {
                s = "ER " + "★" + strPano + " " + strSName + "★";
            }
            else
            {
                s = "ER " + strPano + " " + strSName;   //'2017-09-19 add 출력형식변경)
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 5);

            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = READ_WARRING(strORDERCODE);
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 25);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            if (strUnitNew4 == "")
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew3;
            }
            else
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew4 + "ml" + "/" + strUnitNew3;
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 115, nY + 25);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            //s = "약품명:" + strJusaName;
            s = strJusaName;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 40);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            //s = "성분:" + READ_SUNGBUN(strORDERCODE); //'2017-09-19 add 출력형식변경
            s = READ_SUNGBUN(strORDERCODE); //'2017-09-19 add 출력형식변경
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 55);

            //printFont = new Font(sFont, 10, FontStyle.Regular);
            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "1회:" + strDivQty;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 70);

            //printFont = new Font(sFont, 10, FontStyle.Regular);
            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = strDosage + "(" + strREMARK + ")";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 85);

            printFont = new Font(sFont, 18, FontStyle.Bold);
            s = strGBGROUP;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 180, nY + 40);

            ComFunc.ReadSysDate(clsDB.DbCon);
            string strDate = VB.Replace(VB.Mid(clsPublic.GstrSysDate, 3, clsPublic.GstrSysDate.Length - 1), "-", "/");

            printFont = new Font(sFont, 12, FontStyle.Bold);
            if (strIMIV == "흡입")
            {
                if (chkER.Checked == false)
                {
                    if (rdoIMIV3.Checked == true)
                    { }
                    else
                    {
                        if (rdoGB0.Checked == true) s = "AM";
                        else if (rdoGB1.Checked == true) s = "PM";
                        else if (rdoGB2.Checked == true) s = "HS";
                        else if (rdoGB3.Checked == true)
                        {
                            if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)") s = "4AM";
                            else s = "5AM";
                        }
                        //'2013-09-10
                        else if (rdoGB5.Checked == true) s = "BID";
                        else if (rdoGB6.Checked == true) s = "TID";
                        else if (rdoGB7.Checked == true) s = "QID";
                        else if (rdoGB8.Checked == true) s = "QD";
                    }
                }
                else
                {
                    if (strInfo1 == "AM") s = "AM";
                    else if (strInfo1 == "PM") s = "PM";
                    else if (strInfo1 == "HS") s = "HS";
                    else if (strInfo1 == "4AM") s = "4AM";
                    else if (strInfo1 == "5AM") s = "5AM";
                    else if (strInfo1 == "BID") s = "BID";
                    else if (strInfo1 == "TID") s = "TID";
                    else if (strInfo1 == "QID") s = "QID";
                    else if (strInfo1 == "QD") s = "QD";
                }
            }
            else
            {
                if (chkER.Checked == false)
                {

                    if (rdoGB0.Checked == true) s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB1.Checked == true) s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB2.Checked == true) s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB3.Checked == true)
                    {
                        if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)")
                        {
                            s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                        else
                        {
                            s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                    }
                    //'2013-09-10
                    else if (rdoGB5.Checked == true) s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB6.Checked == true) s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB7.Checked == true) s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB8.Checked == true) s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add 
                }
                else
                {
                    if (strInfo1 == "AM") s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "PM") s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "HS") s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "4AM") s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "5AM") s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "BID") s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "TID") s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QID") s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QD") s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                }
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 85, nY + 1300);
        }

        void ePrintPage1_NEW(object sender, PrintPageEventArgs ev)
        {
            clsVbfunc CV = new clsVbfunc();
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            Font printFont;
            string sFont = "굴림체";
            //int nSize = 12;
            int nX = 0;
            int nY = 0;
            string s;

            #region 최상단 진료과 등록번호 이름
            printFont = new Font(sFont, 12, FontStyle.Regular);

            ev.Graphics.DrawString("ER", printFont, Brushes.Black, nX + 5, nY + 5);

            printFont = new Font(sFont, 12, FontStyle.Underline);
            if (strJung != "")
            {
                s = "★" + strPano + " " + strSName + "★";
            }
            else
            {
                s = strPano + " " + strSName;   //'2017-09-19 add 출력형식변경)
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 25, nY + 5); 
            #endregion


            #region 주의사항 및 단위 표시
            printFont = new Font(sFont, 8, FontStyle.Bold);
            s = READ_WARRING(strORDERCODE);
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 23);

            
            printFont = new Font(sFont, 8, FontStyle.Regular);
            if (strUnitNew4 == "")
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew3;
            }
            else
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew4 + "ml" + "/" + strUnitNew3;
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 115, nY + 23);
            #endregion

            var rect = new RectangleF(nX + 5, nY + 35, nX + 185, nY + 36);
            ev.Graphics.FillRectangle(Brushes.Gray, rect);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            //s = "약품명:" + strJusaName;
            s = strJusaName;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 40);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            //s = "성분:" + READ_SUNGBUN(strORDERCODE); //'2017-09-19 add 출력형식변경
            s = READ_SUNGBUN(strORDERCODE); //'2017-09-19 add 출력형식변경
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 55);

            //printFont = new Font(sFont, 10, FontStyle.Regular);
            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "1회:" + strDivQty;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 70);

            //printFont = new Font(sFont, 10, FontStyle.Regular);
            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = strDosage + "(" + strREMARK + ")";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 85);

            printFont = new Font(sFont, 18, FontStyle.Bold);
            s = strGBGROUP;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 180, nY + 70);

            ComFunc.ReadSysDate(clsDB.DbCon);
            string strDate = VB.Replace(VB.Mid(clsPublic.GstrSysDate, 3, clsPublic.GstrSysDate.Length - 1), "-", "/");

            printFont = new Font(sFont, 12, FontStyle.Bold);
            if (strIMIV == "흡입")
            {
                if (chkER.Checked == false)
                {
                    if (rdoIMIV3.Checked == true)
                    { }
                    else
                    {
                        if (rdoGB0.Checked == true) s = "AM";
                        else if (rdoGB1.Checked == true) s = "PM";
                        else if (rdoGB2.Checked == true) s = "HS";
                        else if (rdoGB3.Checked == true)
                        {
                            if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)") s = "4AM";
                            else s = "5AM";
                        }
                        //'2013-09-10
                        else if (rdoGB5.Checked == true) s = "BID";
                        else if (rdoGB6.Checked == true) s = "TID";
                        else if (rdoGB7.Checked == true) s = "QID";
                        else if (rdoGB8.Checked == true) s = "QD";
                    }
                }
                else
                {
                    if (strInfo1 == "AM") s = "AM";
                    else if (strInfo1 == "PM") s = "PM";
                    else if (strInfo1 == "HS") s = "HS";
                    else if (strInfo1 == "4AM") s = "4AM";
                    else if (strInfo1 == "5AM") s = "5AM";
                    else if (strInfo1 == "BID") s = "BID";
                    else if (strInfo1 == "TID") s = "TID";
                    else if (strInfo1 == "QID") s = "QID";
                    else if (strInfo1 == "QD") s = "QD";
                }
            }
            else
            {
                if (chkER.Checked == false)
                {

                    if (rdoGB0.Checked == true) s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB1.Checked == true) s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB2.Checked == true) s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB3.Checked == true)
                    {
                        if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)")
                        {
                            s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                        else
                        {
                            s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                    }
                    //'2013-09-10
                    else if (rdoGB5.Checked == true) s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB6.Checked == true) s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB7.Checked == true) s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB8.Checked == true) s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add 
                }
                else
                {
                    if (strInfo1 == "AM") s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "PM") s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "HS") s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "4AM") s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "5AM") s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "BID") s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "TID") s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QID") s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QD") s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                }
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 85, nY + 1300);
        }


        void ePrintPage1_202105(object sender, PrintPageEventArgs ev)
        {
            //2021-05-12의뢰건으로 서식 변경 적용
            clsVbfunc CV = new clsVbfunc();
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            string strTuPath = ""; //투여경로

            Font printFont;
            string sFont = "굴림체";
            //int nSize = 12;
            int nX = 0;
            int nY = 0;
            string s;

            string strGubun = ""; //한글 영문 구분

            //8.경구약을 선택하고 출력하엿을 경우[경구]로 출력되도록 해주세요
            if (rdoIMIV6.Checked == true)
            {
                strTuPath = "[경구]";
                strGubun = "1";
            }
            //2.IV(m)인 경우  IV(m)로 출력되도록 해주세요
            else if (VB.UCase(strDosage).Contains("IV(M)"))
            {
                strTuPath = "IV(m)";
            }
            else if (VB.UCase(strDosage).Contains("IV/"))
            {
                strTuPath = "IV";
            }
            else if (VB.UCase(strDosage).Contains("IM/"))
            {
                strTuPath = "IM";
            }
            //3.Inhal을 선택하여 출력하거나 용법이 Inhal인 경우[흡입]으로 출력되도록 해 주세요
            else if (VB.UCase(strDosage).Contains("INHAL"))
            {
                strTuPath = "[흡입]";
                strGubun = "1";
            }
            //4.용법이 SC인 경우[SC]로 출력되도록 해 주세요
            else if (VB.UCase(strDosage).Contains("SC"))
            {
                strTuPath = "[SC]";
            }
            //5.용법이 Skin인 경우[피부]로 출력되도록 해 주세요
            else if (VB.UCase(strDosage).Contains("SKIN"))
            {
                strTuPath = "[피부]";
                strGubun = "1";
            }
            //6.용법이 Rectum인 경우[직장]로 출력되도록 해 주세요
            else if (VB.UCase(strDosage).Contains("RECTUM"))
            {
                strTuPath = "[직장]";
                strGubun = "1";
            }
            //7.용법이 Eye인 경우[안약]로 출력되도록 해 주세요
            else if (VB.UCase(strDosage).Contains("EYE"))
            {
                strTuPath = "[안약]";
                strGubun = "1";
            }
            else
            {
                strTuPath = "";
            }
            

            #region 최상단 진료과 등록번호 이름
            printFont = new Font(sFont, 11, FontStyle.Regular);

            //ev.Graphics.DrawString("ER", printFont, Brushes.Black, nX + 5, nY + 5);
            
            printFont = new Font(sFont, 11, FontStyle.Bold);
            //if (strJung != "")
            //{
            //    s = "★" + strPano + " " + strSName + "★";
            //}
            //else
            //{
            //    s = strPano + " " + strSName;   //'2017-09-19 add 출력형식변경)
            //}

            s = strPano + " " + strSName;   //'2021-07-02 add 출력형식변경)

            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 10, nY + 5);
            #endregion

            #region 약명칭 표시
            printFont = new Font(sFont, 9, FontStyle.Regular);

            s = strJusaName.Split('(')[0];
            //s = VB.Left(s,9);
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 10, nY + 23);
            #endregion
           
            #region 약용량 표시
            printFont = new Font(sFont, 9, FontStyle.Underline);
            if (strUnitNew4 == "")
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew3;
            }
            else
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew4 + "ml" + "/" + strUnitNew3;
            }
            //s = s + " " + strDosage;
            s = s + " "; //용법표시 안함

            if (strUnitNew4 == "")
            {
                ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 75, nY + 38);
            }
            else
            {
                ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 55, nY + 38);
            }
                
            #endregion

            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "________________________________________";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 10, nY + 42);


            printFont = new Font(sFont, 9, FontStyle.Bold);   
            s = "1회:" + strDivQty.Replace("A)", "ⓐ)").Replace("V)", "ⓥ)");
            s = VB.Replace(s," ","");

            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 10, nY + 57);
            s = strDosage; //용법표시
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 35, nY + 72);

            printFont = new Font(sFont, 8, FontStyle.Bold);
            if (strREMARK != "")
            {
                if (VB.Len(strREMARK.Trim()) > 18)
                {
                    s = "[REMARK:" + VB.Left(strREMARK.Trim(), 18)  + "]";
                }
                else
                {
                    s = "[REMARK:" + strREMARK + "]";
                }
            }
            else
            {
                s = "[REMARK:             ]";
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 10, nY + 85);


            printFont = new Font(sFont, 14, FontStyle.Bold);
            s = strGBGROUP;
            if (s !="")
            {
                ev.Graphics.DrawString("■", printFont, Brushes.Black, nX + 180, nY + 5);
                ev.Graphics.DrawString(s, printFont, Brushes.White, nX + 185, nY + 5);
            }
            else
            {
                ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 185, nY + 5);
            }
            

            

            


            ComFunc.ReadSysDate(clsDB.DbCon);
            string strDate = VB.Replace(VB.Mid(clsPublic.GstrSysDate, 3, clsPublic.GstrSysDate.Length - 1), "-", "/");

            printFont = new Font(sFont, 12, FontStyle.Bold);

            s = strTuPath;

            if (strGubun == "1")
            {
                ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 150, nY + 63);
            }
            else
            {
                ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 155, nY + 63);
            }

            //if (strTuPath == "IM")
            //{
            //    s = "M";
            //}
            //else if (strTuPath == "IV")
            //{
            //    s = "V";
            //}
            //ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 175, nY + 73);

        }

        bool Ocs_Update(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (cboWard.Text.Trim() == "EM" || cboWard.Text.Trim() == "ER")
                {
                    SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET LABELPRINT = '*' WHERE ROWID = '" + strRowid + "' ";
                }
                else
                {
                    SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET LABELPRINT = '*' WHERE ROWID = '" + strRowid + "' ";
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

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
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

        private void SET_PRINT_OLD()
        {
            int i = 0;            
            string mstrPrintName = "혈액환자정보";

            //if (clsPrint.gGetPrinterFind2(mstrPrintName) == false)
            if (clsPrint.gGetPrinterFind(mstrPrintName) == false)
            {
                ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.", "혈액환자정보");
                return;
            }

            for (i = 0; i < ss1_Sheet1.NonEmptyRowCount; i++)
            {
                strFlag = ss1_Sheet1.Cells[i, 0].Text;
                strRoom = ss1_Sheet1.Cells[i, 2].Text;
                strPano = ss1_Sheet1.Cells[i, 3].Text;
                strSName = ss1_Sheet1.Cells[i, 4].Text;
                strORDERCODE = ss1_Sheet1.Cells[i, 5].Text;
                strJusaName = ss1_Sheet1.Cells[i, 6].Text;
                strDosage = ss1_Sheet1.Cells[i, 7].Text;
                strIMIV = ss1_Sheet1.Cells[i, 9].Text;
                strUnit = ss1_Sheet1.Cells[i, 10].Text;

                strUnitNew1 = ss1_Sheet1.Cells[i, 15].Text;
                strUnitNew2 = ss1_Sheet1.Cells[i, 16].Text;
                strUnitNew3 = ss1_Sheet1.Cells[i, 17].Text;
                strUnitNew4 = ss1_Sheet1.Cells[i, 18].Text;
                strDivQty = ss1_Sheet1.Cells[i, 19].Text;
                strROWID = ss1_Sheet1.Cells[i, 20].Text;

                strTEMP1 = ss1_Sheet1.Cells[i, 25].Text;
                strTEMP2 = ss1_Sheet1.Cells[i, 26].Text;
                strREMARK = ss1_Sheet1.Cells[i, 27].Text;

                if (strFlag == "True")
                {
                    BARCODE_PRINT_OLD(mstrPrintName);
                    Ocs_Update(strROWID);
                }
            }
        }

        private void BARCODE_PRINT_OLD(string strPrintName)
        {
            string strPrintName1 = "";
            string strPrintName2 = "";
            clsPrint CP = new clsPrint();

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk(strPrintName.ToUpper());

                if (strPrintName2 == "")
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 의료정보과에 연락바랍니다.");
                    return;
                }

                PrintDocument pd = new PrintDocument();
                PrintController pc = new StandardPrintController();
                pd.PrintController = pc;
                pd.PrinterSettings.PrinterName = strPrintName2;
                pd.PrintPage += new PrintPageEventHandler(ePrintPage2);
                pd.Print();    //프린트             
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return;
            }            
        }

        void ePrintPage2(object sender, PrintPageEventArgs ev)
        {
            clsVbfunc CV = new clsVbfunc();
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;

            Font printFont;
            string sFont = "굴림체";
            //int nSize = 12;
            int nX = 0;
            int nY = 0;
            string s;


            printFont = new Font(sFont, 12, FontStyle.Regular);
            s = strRoom + "호 " + strPano + " " + strSName;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 5);

            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "[" + strORDERCODE + "] ";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 25);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            if (strUnitNew4 == "")
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew3;
            }
            else
            {
                s = strUnitNew1 + strUnitNew2 + "/" + strUnitNew4 + "ml" + "/" + strUnitNew3;
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 115, nY + 25);

            printFont = new Font(sFont, 10, FontStyle.Regular);
            s = "용법:" + strDosage;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 40);

            printFont = new Font(sFont, 10, FontStyle.Bold);
            s = "1회:" + strDivQty;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 5, nY + 55);

            printFont = new Font(sFont, 18, FontStyle.Bold);
            if (strIMIV == "흡입")
            {
                s = "흡";
            }
            else
            {
                s = VB.Left(strIMIV, 1);
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 2435, nY + 300);

            printFont = new Font(sFont, 10, FontStyle.Bold);
            if (strIMIV == "흡입")
            {
                s = "입";
            }
            else
            {
                s = VB.Right(strIMIV, 1);
            }
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 2435, nY + 700);


            ComFunc.ReadSysDate(clsDB.DbCon);
            string strDate = VB.Replace(VB.Mid(clsPublic.GstrSysDate, 3, clsPublic.GstrSysDate.Length - 1), "-", "/");
            
            if (strIMIV == "흡입")
            {
                if (chkER.Checked == false)
                {

                    if (rdoGB0.Checked == true) s = "AM";
                    else if (rdoGB1.Checked == true) s = "PM";
                    else if (rdoGB2.Checked == true) s = "HS";
                    else if (rdoGB3.Checked == true)
                    {
                        if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)") s = "4AM";
                        else s = "5AM";
                    }
                    //'2013-09-10
                    else if (rdoGB5.Checked == true) s = "BID";
                    else if (rdoGB6.Checked == true) s = "TID";
                    else if (rdoGB7.Checked == true) s = "QID";
                    else if (rdoGB8.Checked == true) s = "QD";

                }
                else
                {
                    if (strInfo1 == "AM") s = "AM";
                    else if (strInfo1 == "PM") s = "PM";
                    else if (strInfo1 == "HS") s = "HS";
                    else if (strInfo1 == "4AM") s = "4AM";
                    else if (strInfo1 == "5AM") s = "5AM";
                    else if (strInfo1 == "BID") s = "BID";
                    else if (strInfo1 == "TID") s = "TID";
                    else if (strInfo1 == "QID") s = "QID";
                    else if (strInfo1 == "QD") s = "QD";
                }

                printFont = new Font(sFont, 12, FontStyle.Bold);                
                ev.Graphics.DrawString("★먹지마세요!★", printFont, Brushes.Black, nX + 900, nY + 1100);
            }
            else
            {
                if (chkER.Checked == false)
                {

                    if (rdoGB0.Checked == true) s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB1.Checked == true) s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB2.Checked == true) s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (rdoGB3.Checked == true)
                    {
                        if (cboWard.Text == "IU(MISU)" || cboWard.Text == "IU(SICU)")
                        {
                            s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                        else
                        {
                            s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                        }
                    }
                    //'2013-09-10
                    else if (rdoGB5.Checked == true) s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB6.Checked == true) s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB7.Checked == true) s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add
                    else if (rdoGB8.Checked == true) s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")"; //'add 
                }
                else
                {
                    if (strInfo1 == "AM") s = "AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "PM") s = "PM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "HS") s = "HS" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "4AM") s = "4AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "5AM") s = "5AM" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "BID") s = "BID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "TID") s = "TID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QID") s = "QID" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                    else if (strInfo1 == "QD") s = "QD" + "  (" + strDate + " " + clsPublic.GstrSysTime + ")";
                }
            }
            printFont = new Font(sFont, 12, FontStyle.Bold);
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 85, nY + 1100);
        }

        private string READ_WARRING(string arg)
        {
            string rtnVal = "";
           // int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            
            Cursor.Current = Cursors.WaitCursor;
            //2019-10-04 약코드 안나오도록 보완 요청
            try
            {
                SQL = " SELECT DECODE(GUBUN,'01','고위험','02','고위험','03','고위험','04','고위험','11','고주의','10','고주의','05','고주의') GUBUN ";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_BUN_LIST ";
                SQL += ComNum.VBLF + " WHERE JEPCODE = '" + arg + "' ";
                SQL += ComNum.VBLF + " union all SELECT '항혈전' GUBUN ";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SETCODE ";
                SQL += ComNum.VBLF + " WHERE JEPCODE = '" + arg + "'  AND GUBUN = '13' AND (DELDATE IS NULL or DelDate ='')  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GUBUN"].ToString().Trim() != "")
                    {
                        //rtnVal = "[" + arg + " ★" + dt.Rows[0]["GUBUN"].ToString().Trim() + "]";
                        rtnVal = "[★" + dt.Rows[0]["GUBUN"].ToString().Trim() + "]";
                    }
                    else
                    {
                        //rtnVal = "[" + arg + "]";
                        rtnVal = "";
                    }
                }
                else
                {
                    //rtnVal = "[" + arg + "]";
                    rtnVal = "";
                }                    
                                
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private string READ_SUNGBUN(string arg)
        {
            string rtnVal = "";
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {                
                SQL = " SELECT SNAME ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DRUGINFO_NEW ";
                SQL += ComNum.VBLF + " WHERE SUNEXT = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SNAME"].ToString().Trim();                    
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, 0].Value = true;
        }

        private void btnSelectCancel_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, 0].Value = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpDate_Enter(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            GetPatient();
        }

        void GetPatient()
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            lblSname.Text = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                lblSname.Text = dt.Rows[0]["SNAME"].ToString();

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

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column, ref bolSort, true);
                return;
            }

            if (e.RowHeader == true) return;

            if (Convert.ToBoolean(ss1_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ss1_Sheet1.Cells[e.Row, 0].Value = false;
                ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(0, 0, 0);
            }
            else
            {
                ss1_Sheet1.Cells[e.Row, 0].Value = true;
                ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(255, 0, 0);
            }

            //if (e.Column == 0)
            //{
            //    if (Convert.ToBoolean(ss1_Sheet1.Cells[e.Row, 0].Value) == true)
            //    {
            //        ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(0, 0, 0);
            //    }
            //    else
            //    {
            //        ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(255, 0, 0);
            //    }
            //}
            //else
            //{
            //    if (Convert.ToBoolean(ss1_Sheet1.Cells[e.Row, 0].Value) == true)
            //    {
            //        ss1_Sheet1.Cells[e.Row, 0].Value = false;
            //        ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(0, 0, 0);
            //    }
            //    else
            //    {
            //        ss1_Sheet1.Cells[e.Row, 0].Value = true;
            //        ss1_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(255, 0, 0);
            //    }
            //}            
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            if(VB.Trim(txtPano.Text) == "" || VB.Len(txtPano.Text) != 8)
            {
                ComFunc.MsgBox("등록번호를 넣고 제외처리하십시오!!");
                return;
            }

            if (VB.Trim(cboWard.Text) != "ER")
            {
                ComFunc.MsgBox("응급실만 사용가능합니다!!");
                return;
            }
            
            Cursor.Current = Cursors.WaitCursor;            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET LABELPRINT = '*' ";
                SQL += ComNum.VBLF + " WHERE Ptno ='" + VB.Trim(txtPano.Text) + "' ";
                SQL += ComNum.VBLF + "   AND BDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND GbIoe IN ('E','EI') ";
                SQL += ComNum.VBLF + "   AND (LABELPRINT IS NULL OR LABELPRINT <> '*' ) ";
                SQL += ComNum.VBLF + "   AND  Bun IN ('11', '12', '20') ";
                SQL += ComNum.VBLF + "   AND  (GbPRN IN  NULL OR GbPRN <> 'P') ";
                SQL += ComNum.VBLF + "   AND  GbPRN <>'S' ";  //'jjy 추가(2000/05/22 'S는 선수납(선불)
                SQL += ComNum.VBLF + "   AND  (GbStatus    = ' ' OR GbStatus IS NULL)    ";
                SQL += ComNum.VBLF + "   AND  (GbStatus  <> 'D' AND GbStatus <> 'D-')    ";
                
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
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void Print_WardFormat(object sender, string argBdate, string argPtno, string argSex, string argOrderno, PrintPageEventArgs e)
        {
            //2019-03-11
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            //int intRowAffected = 0;

            string strRoomCode = ""; //'약  (수액은 별도 사용)
            string strPtNo = "";
            string strSName = "";
            string strSex = "";
            string strAge = "";
            string strDeptCode = "";

            string strUnit = "";  //     '기본용량
            int nL = 0; //         '기본단위자르때 사용
            string strQty = ""; //'총용량
            string strDos = ""; //'용법
            string strSlipNo = ""; //'과처방인지를 구분

            string oldPtno = "";  //'환자번호

            string oldDos = "";    //   '주사 사용
            string oldDosCode = "";
            string newDosCode = "";

            int nPtCnt = 0;      //    '약 & 주사 사용
            int CurLeft = 0;      //    '출력시 현재 Left
            int CurTop = 0;       // '출력시 현재 Top
            string sDeptOrder = ""; //    As String *20
            int Cnt = 0;      //    As Integer        '15건 이상일 경우 다음장으로 연결 출력

            string newDos = "";
            string strCaution = "";
            string strJepName = ""; //'약국 제품명

            int intCurrentX = 0;
            int intCurrentY = 0;

            string strPrintDate = "";
            int intPrt = 0;

            DataTable dt1 = null;

            SQL = " SELECT  ";
            SQL += ComNum.VBLF + "        O.ROOMCODE,    O.PTNO PANO,    P.SNAME,   '" + argSex + "' SEX, P.JUMIN1, P.JUMIN2,      ";
            SQL += ComNum.VBLF + "        O.DEPTCODE,    O.SLIPNO,       O.ORDERCODE, ";
            SQL += ComNum.VBLF + "        C.ORDERNAME,   C.ORDERNAMES,   O.CONTENTS,   O.BCONTENTS, ";
            SQL += ComNum.VBLF + "        O.REALQTY,     O.GBGROUP,      O.DOSCODE,    D.DOSNAME,     D.DOSFULLCODE,  ";
            SQL += ComNum.VBLF + "        O.REMARK,      O.GBORDER,      O.GBDIV,      O.SUCODE,      O.QTY , O.WARDCODE, ";
            SQL += ComNum.VBLF + "        N.UNITNEW1,    N.UNITNEW2,     N.UNITNEW3,   N.UNITNEW4,    ";
            SQL += ComNum.VBLF + "        N.UNITNEW1 ||  N.UNITNEW2 ||  DECODE( N.UNITNEW4 ,'', '', '/(' ||  N.UNITNEW4 || 'ML)' )  || '/' ||  N.UNITNEW3 UNITNEW,  ";
            SQL += ComNum.VBLF + "        DR.CAUTION_STRING     ";
            SQL += ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
            SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE C, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE D, ";
            SQL += ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     N, ";
            SQL += ComNum.VBLF + " KOSMOS_OCS.OCS_DRUGINFO_NEW DR ";
            SQL += ComNum.VBLF + "  WHERE    O.BDATE = TO_DATE('" + argBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND    O.PTNO IN ( '" + argPtno + "' ) ";
            SQL += ComNum.VBLF + "    AND    O.ORDERNO = " + argOrderno;
            SQL += ComNum.VBLF + "    AND    O.PTNO      =  P.PANO ";
            SQL += ComNum.VBLF + "    AND    O.SLIPNO    =  C.SLIPNO(+)  ";
            SQL += ComNum.VBLF + "    AND    O.ORDERCODE =  C.ORDERCODE(+)  ";
            SQL += ComNum.VBLF + "    AND    O.DOSCODE   =  D.DOSCODE(+) ";
            SQL += ComNum.VBLF + "    AND    O.SUCODE =  N.SUNEXT ";
            SQL += ComNum.VBLF + "    AND    O.SUCODE = DR.SUNEXT(+) ";
            SQL += ComNum.VBLF + " ORDER  BY O.ROOMCODE, P.PANO, O.DOSCODE DESC, O.SLIPNO, O.SEQNO ";
            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt1.Rows.Count == 0)
            {
                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i]["DOSFULLCODE"].ToString().Trim() == "HS")
                {
                    newDos = VB.Left(dt1.Rows[i]["DOSCODE"].ToString().Trim(), 2) + "HS";
                }
                else if (dt1.Rows[i]["DOSFULLCODE"].ToString().Trim() == "3time(BF,SP,HS)"
                         || dt1.Rows[i]["DOSFULLCODE"].ToString().Trim() == "BF,SP & HS/PC 30min"
                         || dt1.Rows[i]["DOSFULLCODE"].ToString().Trim() == "BF,SP & HS")
                {
                    newDos = "BSH";
                }
                else
                {
                    newDos = VB.Left(dt1.Rows[i]["DOSCODE"].ToString().Trim(), 2);

                    if (dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("AC", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "AC";
                    }

                    if (dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("SP", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "SP";
                    }

                    if (dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("LC", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "LC";
                    }

                    //'간호부요청 bid 인경우 Lunch & HS 경우 1pm  9pm 인쇄 요청(2012-11-20) ADD BF/Lunch  경우 8Am  1pm 인쇄 요청(2016-04-19)


                    if (newDos == "02" && dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("Lunch/HS", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "Lunch/HS";
                    }


                    if (newDos == "02" && dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("BF & L", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "BF/Lunch";
                    }

                    if (newDos == "02" && dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("BF & HS", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "BF/HS";
                    }

                    //'간호부요청 tid 인경우 "BF,L & HS경우 ADD

                    if (newDos == "03" && dt1.Rows[i]["DOSNAME"].ToString().Trim().Replace("BF,L & HS", "") != dt1.Rows[i]["DOSNAME"].ToString().Trim())
                    {
                        newDos = newDos + "BF,L & HS";
                    }

                    newDosCode = dt1.Rows[i]["DOSCODE"].ToString().Trim();
                }

                if ((oldPtno != dt1.Rows[i]["PANO"].ToString().Trim() || oldPtno == dt1.Rows[i]["PANO"].ToString().Trim() && Cnt > 14 || oldDos != newDos) && intPrt == 0)
                {
                    if (oldPtno == dt1.Rows[i]["PANO"].ToString().Trim() && Cnt > 15)
                    {
                        intCurrentX = CurLeft;
                        intCurrentY = CurTop + 13;
                        CurTop = intCurrentY;
                        e.Graphics.DrawString(VB.Space(10) + "*** 연 결 ***", new Font("궁서체", 10), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                    }

                    if (i != 0) //'투약카드 꼬리말 달기
                    {
                        switch ((nPtCnt - 1) % 4)
                        {
                            case 0:
                                intCurrentX = 0;
                                break;
                            case 1:
                                intCurrentX = 200;
                                break;
                            case 2:
                                intCurrentX = 400;
                                break;
                            case 3:
                                intCurrentX = 600;
                                break;
                        }

                        switch ((nPtCnt - 1) % 16)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                intCurrentY = 290 * 1 - 50;
                                break;
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                intCurrentY = 290 * 2 - 50;
                                break;
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                intCurrentY = 290 * 3 - 50;
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                intCurrentY = 290 * 4 - 50;
                                break;
                        }

                        switch (oldDos)
                        {

                            case "01":
                            case "01HS":
                            case "01AC":
                            case "01SP":
                            case "01LC":
                            case "01ACSP":
                                if (oldDos == "01HS")
                                {
                                    e.Graphics.DrawString(VB.Space(5) + (dt1.Rows[i]["WARDCODE"].ToString().Trim() == "3A" ? "8PM" : "9PM"), new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "01ACSP")
                                {
                                    e.Graphics.DrawString(VB.Space(5) + "5PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (VB.Left(oldDos, 4) == "01AC")
                                {
                                    e.Graphics.DrawString(VB.Space(5) + "7AM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "01SP")
                                {
                                    e.Graphics.DrawString(VB.Space(5) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "01LC")
                                {
                                    e.Graphics.DrawString(VB.Space(5) + "1PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else
                                {
                                    e.Graphics.DrawString(VB.Space(5) + "8AM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                break;

                            case "02":
                            case "02AC":
                            case "02SP":
                            case "02ACSP":
                            case "02Lunch/HS":
                                if (VB.Left(oldDos, 4) == "02AC")
                                {
                                    e.Graphics.DrawString("7AM" + VB.Space(12) + "5PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "02Lunch/HS")
                                {
                                    e.Graphics.DrawString("1PM" + VB.Space(12) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "02BF/Lunch")
                                {
                                    e.Graphics.DrawString("8AM" + VB.Space(12) + "1PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "02BF/HS")
                                {
                                    e.Graphics.DrawString("8AM" + VB.Space(12) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else
                                {
                                    e.Graphics.DrawString("8AM" + VB.Space(12) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                break;
                            case "03":
                            case "03AC":
                            case "03SP":
                            case "03ACSP":
                                if (VB.Left(oldDos, 4) == "03AC")
                                {
                                    e.Graphics.DrawString("7AM" + VB.Space(5) + "12AM" + VB.Space(5) + "5PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else if (oldDos == "03BF,L & HS")
                                {

                                    e.Graphics.DrawString("8AM" + VB.Space(5) + "1PM" + VB.Space(5) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else
                                {
                                    e.Graphics.DrawString("8AM" + VB.Space(5) + "1PM" + VB.Space(5) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                break;

                            case "04":
                            case "04AC":
                            case "04SP":
                                if (VB.Left(oldDos, 4) == "04AC")
                                {
                                    e.Graphics.DrawString("7AM" + VB.Space(2) + "12PM" + VB.Space(2) + "5PM" + VB.Space(2) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                else
                                {
                                    e.Graphics.DrawString("8AM" + VB.Space(2) + "1PM" + VB.Space(2) + "6PM" + VB.Space(2) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                }
                                break;
                            case "BSH":
                                e.Graphics.DrawString("8AM" + VB.Space(5) + "6PM" + VB.Space(5) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                break;
                            default:
                                e.Graphics.DrawString(READ_NEW_DOS_TIME(oldDosCode), new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                                break;
                        }

                        switch ((nPtCnt - 1) % 4)
                        {
                            case 0:
                                intCurrentX = 0;
                                break;
                            case 1:
                                intCurrentX = 200;
                                break;
                            case 2:
                                intCurrentX = 400;
                                break;
                            case 3:
                                intCurrentX = 600;
                                break;
                        }

                        intCurrentY = intCurrentY - 10;
                        e.Graphics.DrawString("인쇄일시:" + strPrintDate, new Font("궁서체", 8), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                    }

                    Cnt = 0;

                    strRoomCode = dt1.Rows[i]["ROOMCODE"].ToString().Trim();
                    strPtNo = dt1.Rows[i]["PANO"].ToString().Trim();
                    strSName = dt1.Rows[i]["SNAME"].ToString().Trim();
                    strSex = dt1.Rows[i]["SEX"].ToString().Trim();
                    strAge = Convert.ToString(ComFunc.AgeCalcEx(dt1.Rows[i]["JUMIN1"].ToString().Trim() + dt1.Rows[i]["JUMIN2"].ToString().Trim(), Convert.ToDateTime(strPrintDate).ToString("yyyy-MM-dd")));
                    strDeptCode = dt1.Rows[i]["DeptCode"].ToString().Trim();

                    oldPtno = strPtNo.Trim();
                    nPtCnt = nPtCnt + 1;
                    oldDos = newDos.Trim();
                    oldDosCode = newDosCode.Trim();

                    switch ((nPtCnt - 1) % 4)
                    {
                        case 0:
                            intCurrentX = 0;
                            break;
                        case 1:
                            intCurrentX = 200;
                            break;
                        case 2:
                            intCurrentX = 400;
                            break;
                        case 3:
                            intCurrentX = 600;
                            break;
                    }

                    switch ((nPtCnt - 1) % 16)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            intCurrentY = 290 * 0;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            intCurrentY = 290 * 1;
                            break;
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            intCurrentY = 290 * 2;
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            intCurrentY = 290 * 3;
                            break;
                    }

                    CurLeft = intCurrentX;
                    CurTop = intCurrentY;
                    if ((nPtCnt - 1) % 16 == 0 && nPtCnt != 1)
                    {
                        e.HasMorePages = true;

                        intPrt = i;
                        return;
                    }

                    e.Graphics.DrawString(strRoomCode + ComFunc.LeftH(strSName + VB.Space(8), 8) + strPtNo + " " + strSex + "/" + strAge.Trim() + " " + strDeptCode.Trim()
                            , new Font("궁서체", 9, FontStyle.Bold), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                }


                if (intPrt != 0)
                {
                    e.Graphics.DrawString(strRoomCode + ComFunc.LeftH(strSName + VB.Space(8), 8) + strPtNo + " " + strSex + "/" + strAge.Trim() + " " + strDeptCode.Trim()
                            , new Font("궁서체", 9, FontStyle.Bold), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());

                    intPrt = 0;
                }

                //           Printer.Font.Size = 9
                //            '제품명
                Cnt = Cnt + 1;
                strJepName = READ_DRINFO_NAME(dt1.Rows[i]["SUCODE"].ToString().Trim(), "1");
                intCurrentX = CurLeft;
                intCurrentY = CurTop + 13;
                CurTop = intCurrentY;

                e.Graphics.DrawString("⊙" + ComFunc.LeftH(strJepName, 25)
                                , new Font("궁서체", 9), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());

                Cnt = Cnt + 1;

                strSlipNo = dt1.Rows[i]["SLIPNO"].ToString().Trim();
                strUnit = dt1.Rows[i]["ORDERNAME"].ToString().Trim();
                sDeptOrder = dt1.Rows[i]["ORDERNAME"].ToString().Trim();



                nL = VB.I(strUnit, "/");

                strUnit = VB.Pstr(strUnit, "/", nL);

                switch (strUnit.Trim())
                {
                    case "A":
                        strUnit = "ⓐ";
                        break;
                    case "T":
                        strUnit = "ⓣ";
                        break;
                    case "V":
                        strUnit = "ⓥ";
                        break;
                    case "BT":
                        strUnit = "ⓑ";
                        break;
                }


                switch (Convert.ToInt32(VB.Val(dt1.Rows[i]["BCONTENTS"].ToString().Trim())))
                {
                    case 0:
                        strQty = dt1.Rows[i]["Qty"].ToString().Trim() + " /" + strUnit.Trim();
                        break;
                    default:
                        strQty = Get_Format(Convert.ToString(VB.Val(dt1.Rows[i]["Contents"].ToString().Trim()) / VB.Val(dt1.Rows[i]["BContents"].ToString().Trim())
                            * VB.Val(dt1.Rows[i]["RealQty"].ToString().Trim())), 2) + " /" + strUnit.Trim();
                        
                        break;
                }

                strDos = GET_DRUG(dt1.Rows[i]["DOSCODE"].ToString().Trim());

                intCurrentX = CurLeft;
                intCurrentY = CurTop + 13;
                CurTop = intCurrentY;

                e.Graphics.DrawString("  " + sDeptOrder + VB.Space(1) + VB.Left(strQty.Trim() + VB.Space(5), 5)
                                , new Font("궁서체", 9), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                Cnt = Cnt + 1;


                intCurrentX = CurLeft;
                intCurrentY = CurTop + 13;
                CurTop = intCurrentY;

                e.Graphics.DrawString(ComFunc.LeftH(VB.Space(7) + strDos, 24) + (dt1.Rows[i]["GBORDER"].ToString().Trim() == "F" ? "-전" : "")
                    + (dt1.Rows[i]["GBORDER"].ToString().Trim() == "T" ? "-후" : "")
                                , new Font("궁서체", 9), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());

                strCaution = dt1.Rows[i]["CAUTION_STRING"].ToString().Trim();

                if (strCaution != "")
                {
                    intCurrentX = CurLeft;
                    intCurrentY = CurTop + 10;
                    CurTop = intCurrentY;

                    e.Graphics.DrawString(VB.Space(1) + "★" + VB.Mid(strCaution, 4, strCaution.Length)
                                , new Font("궁서체", 8, FontStyle.Bold), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                }

                if (i == dt1.Rows.Count - 1)
                {
                    switch ((nPtCnt - 1) % 4)
                    {
                        case 0:
                            intCurrentX = 0;
                            break;
                        case 1:
                            intCurrentX = 200;
                            break;
                        case 2:
                            intCurrentX = 400;
                            break;
                        case 3:
                            intCurrentX = 600;
                            break;
                    }


                    switch ((nPtCnt - 1) % 16)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            intCurrentY = 290 * 1 - 50;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            intCurrentY = 290 * 2 - 50;
                            break;
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            intCurrentY = 290 * 3 - 50;
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            intCurrentY = 290 * 4 - 50;
                            break;
                    }

                    //                Printer.Font.Size = 12

                    switch (oldDos)
                    {
                        case "01":
                        case "01HS":
                        case "01AC":
                        case "01SP":
                        case "01ACSP":
                            if (oldDos == "01HS")
                            {
                                e.Graphics.DrawString(VB.Space(5) + (dt1.Rows[i]["WARDCODE"].ToString().Trim() == "3A" ? "8PM" : "9PM"), new Font("궁서체", 10), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else if (VB.Left(oldDos, 4) == "01AC")
                            {
                                e.Graphics.DrawString(VB.Space(5) + "7AM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else if (oldDos == "01SP")
                            {
                                e.Graphics.DrawString(VB.Space(5) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else
                            {
                                e.Graphics.DrawString(VB.Space(5) + "8AM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            break;

                        case "02":
                        case "02AC":
                        case "02SP":
                        case "02ACSP":
                        case "02Lunch/HS":

                            if (VB.Left(oldDos, 4) == "02AC")
                            {
                                e.Graphics.DrawString("7AM" + VB.Space(12) + "5PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else if (oldDos == "02Lunch/HS")
                            {
                                e.Graphics.DrawString("1PM" + VB.Space(12) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else
                            {
                                e.Graphics.DrawString("8AM" + VB.Space(12) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            break;
                        case "03":
                        case "03AC":
                        case "03SP":
                        case "03ACSP":
                            if (VB.Left(oldDos, 4) == "03AC")
                            {
                                e.Graphics.DrawString("7AM" + VB.Space(5) + "12AM" + VB.Space(5) + "5PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else
                            {
                                e.Graphics.DrawString("8AM" + VB.Space(5) + "1PM" + VB.Space(5) + "6PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            break;
                        case "04":
                        case "04AC":
                        case "04SP":
                            if (VB.Left(oldDos, 4) == "04AC")
                            {
                                e.Graphics.DrawString("7AM" + VB.Space(2) + "12PM" + VB.Space(2) + "5PM" + VB.Space(2) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            else
                            {
                                e.Graphics.DrawString("8AM" + VB.Space(2) + "1PM" + VB.Space(2) + "6PM" + VB.Space(2) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            }
                            break;
                        case "BSH":
                            e.Graphics.DrawString("8AM" + VB.Space(5) + "6PM" + VB.Space(5) + "9PM", new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            break;
                        default:
                            e.Graphics.DrawString(READ_NEW_DOS_TIME(oldDosCode), new Font("궁서체", 12), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());
                            break;

                    }

                    switch ((nPtCnt - 1) % 4)
                    {
                        case 0:
                            intCurrentX = 0;
                            break;
                        case 1:
                            intCurrentX = 200;
                            break;
                        case 2:
                            intCurrentX = 400;
                            break;
                        case 3:
                            intCurrentX = 600;
                            break;
                    }


                    intCurrentY = intCurrentY - 10;
                    e.Graphics.DrawString("인쇄일시:" + strPrintDate, new Font("궁서체", 8), Brushes.Black, intCurrentX, intCurrentY, new StringFormat());

                    e.HasMorePages = false;
                }
            }

            dt1.Dispose();
            dt1 = null;
            return;
        }

        public string Get_Format(string str, int n)
        {
            string rtnVal = "";
            string strVal = "";
            int i = 0;

            if (n < 0)
            {
                rtnVal = str;
                return rtnVal;
            }

            for (i = 0; i < n; i++)
            {
                strVal = strVal = "#";
            }

            rtnVal = VB.Val(str).ToString("0." + strVal);
            rtnVal = str;

            return rtnVal;
        }

        private string GET_DRUG(string slipNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";
            string strSlipNo = "";

            strSlipNo = VB.Left(slipNo.Trim(), 2) + "0000";
            rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT DOSFULLCODE";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE DOSCODE = '" + strSlipNo + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVar = dt.Rows[0]["DOSFULLCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                //SQL = "SELECT DOSFULLCODE";
                SQL = "SELECT DOSNAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE DOSCODE = '" + slipNo + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }

                if (dt.Rows.Count > 0)
                {
                    //rtnVar = rtnVar + " " + dt.Rows[0]["DOSFULLCODE"].ToString().Trim();
                    rtnVar = dt.Rows[0]["DOSNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        private string READ_NEW_DOS_TIME(string arg)
        {
            string rtnVal = "";

            switch (VB.Left(arg, 4))
            {
                case "0101":
                    rtnVal = VB.Space(5) + "8AM";
                    break;
                case "0102":
                    rtnVal = VB.Space(5) + "6PM";
                    break;
                case "0103":
                    rtnVal = VB.Space(5) + "9PM";
                    break;
                case "0106":
                    rtnVal = VB.Space(5) + "1PM";
                    break;
                case "0201":
                    rtnVal = "8AM" + VB.Space(12) + "6PM";
                    break;
                case "0202":
                    rtnVal = "8AM" + VB.Space(12) + "1PM";
                    break;
                case "0203":
                    rtnVal = "8AM" + VB.Space(12) + "9PM";
                    break;
                case "0207":
                    rtnVal = "1PM" + VB.Space(12) + "6PM";
                    break;
                case "0208":
                    rtnVal = "1PM" + VB.Space(12) + "9PM";
                    break;
                case "0301":
                    rtnVal = "8AM" + VB.Space(5) + "1PM" + VB.Space(5) + "6PM";
                    break;
                case "0302":
                    rtnVal = "8AM" + VB.Space(5) + "1PM" + VB.Space(5) + "9PM";
                    break;
                case "0303":
                    rtnVal = "1PM" + VB.Space(5) + "6PM" + VB.Space(5) + "9PM";
                    break;
                case "0306":
                    rtnVal = "1PM" + VB.Space(5) + "6PM" + VB.Space(5) + "9PM";
                    break;
                case "0401":
                    rtnVal = "1PM" + VB.Space(2) + "1PM" + VB.Space(3) + "6PM" + VB.Space(2) + "9PM";
                    break;
                default:
                    rtnVal = "";
                    break;
            }


            return rtnVal;
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strPano = "";
            string strROWID = "";

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ss1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strPano = ss1_Sheet1.Cells[i, 3].Text;
                        strROWID = ss1_Sheet1.Cells[i, 20].Text;

                        if (strPano != "" && strROWID != "")
                        {
                            SQL = " UPDATE KOSMOS_OCS.OCS_IORDER SET LABELPRINT = '*' ";
                            SQL += ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                            SQL += ComNum.VBLF + "   AND Ptno ='" + strPano + "' ";
                            SQL += ComNum.VBLF + "   AND BDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "   AND GbIoe IN ('E','EI') ";
                            SQL += ComNum.VBLF + "   AND (LABELPRINT IS NULL OR LABELPRINT <> '*' ) ";
                            SQL += ComNum.VBLF + "   AND  Bun IN ('11', '12', '20') ";
                            SQL += ComNum.VBLF + "   AND  (GbPRN IN  NULL OR GbPRN <> 'P') ";
                            SQL += ComNum.VBLF + "   AND  GbPRN <>'S' ";  //'jjy 추가(2000/05/22 'S는 선수납(선불)
                            SQL += ComNum.VBLF + "   AND  (GbStatus    = ' ' OR GbStatus IS NULL)    ";
                            SQL += ComNum.VBLF + "   AND  (GbStatus  <> 'D' AND GbStatus <> 'D-')    ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                btnSearch.PerformClick();
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
