using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComDbB;

namespace ComNurLibB
{
    public partial class frmDC_new2 : Form, MainFormMessage
    {
        string mstrPtNo = "";

        string mstrROWID = "";
        bool mbolD = false;
        string mstrFlag = "";
        string mstrQty = "";
        string mstrDQty = "";
        string mstrBun = "";
        string mstrOrderSite = "";
        string mstrTO = "";
        string mstrORDERCODE = "";
        string mstrBDATE = "";
        string mstrNurseID = "";

        double mdblQty = 0;
        double mdblDQty = 0;
        double mdblSQty = 0;
        double mdblGBDIV = 0;
        double mdblActDiv = 0;

        string mstrWardCode = "";
        string mstrEXEName = "";

        string strInDate = "";

        FarPoint.Win.Spread.CellType.CheckBoxCellType CellchkBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

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

        public frmDC_new2(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
        }

        public frmDC_new2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="strWardCode"></param>
        /// <param name="strEXEName">"DRPHAR", "PHARNOR" 약제과에서는 D/C전달이 불가능합니다.</param>
        public frmDC_new2(string strWardCode, string strEXEName)
        {
            InitializeComponent();
            mstrWardCode = strWardCode;
            mstrEXEName = strEXEName;
        }

        /// <summary>
        /// </summary>
        /// <param name="strWardCode"></param>
        /// <param name="strEXEName">"DRPHAR", "PHARNOR" 약제과에서는 D/C전달이 불가능합니다.</param>
        public frmDC_new2(MainFormMessage pform, string strWardCode, string strEXEName)
        {
            InitializeComponent();
            this.mCallForm = pform;
            mstrWardCode = strWardCode;
            mstrEXEName = strEXEName;
        }


        private void frmDC_new2_Load(object sender, EventArgs e)
        {
            dtpDateTo.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpDateFr.Value = dtpDateTo.Value.AddDays(-2);

            dtpDateTo.ValueChanged += DtpDateTo_ValueChanged;
            dtpDateFr.ValueChanged += DtpDateFr_ValueChanged;

            //lock 로직 글로벌 사용
            clsPublic.GnJobSabun = Convert.ToInt64(clsType.User.Sabun);
            clsPublic.GstrIpAddress = clsCompuInfo.gstrCOMIP;

            if (mstrWardCode == "ER")
            {
                this.Text = this.Text + "ER";
            }

            ssOrder_Sheet1.Columns.Get(13, 19).Visible = false;

            if (mstrEXEName != "DRPHAR" && mstrEXEName != "PHARNOR" && mstrWardCode == "")
            {
                mstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            ComboWard_SET();

            ComFunc.ComboFind(cboWard, "L", 6, mstrWardCode);

            // '2014-02-24 응급실은 NDC 전체과 풀어줌
            if (cboWard.Text.Trim() == "ER")
            {
                cboWard.Enabled = true;
            }
            else
            {
                cboWard.Enabled = false;
            }

            if (clsType.User.Sabun == "4349" || clsType.User.Sabun == "15291" || clsType.User.Sabun == "23758"
            || mstrEXEName.ToUpper() == "DRPHAR" || mstrEXEName.ToUpper() == "PHARNOR")
            {
                cboWard.Enabled = true;
            }

            //opt4.Checked = true;      //2019-05-10 BST 제외 전체 안보이도록 막음!
            opt6.Checked = true;

            btnCancel_Click(null, null);
            READ_PATIENT();
        }

        private void DtpDateFr_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDateTo.Value >= dtpDateFr.Value)
            {
                READ_PATIENT();

            }
        }

        private void DtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDateTo.Value >= dtpDateFr.Value)
            {
                READ_PATIENT();

            }
        }

        private void ComboWard_SET()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            cboWard.Items.Add("SICU");
            cboWard.Items.Add("MICU");
            cboWard.Items.Add("HD");

            try
            {
                //'병동마스타에서 코드를 Select
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT WARDCODE,WARDNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ') ";
                SQL = SQL + ComNum.VBLF + " AND USED = 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        private void READ_PATIENT()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "SELECT PANO, SNAME, AGE, SEX, DEPTCODE, WARDCODE, ROOMCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER  ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND  GBSTS IN ('0','2','3','4') "; //'允가퇴원 제외 심사계요청(2005-08-24);

                if
                 (cboWard.Text.Trim() == "MICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND WARDCODE ='IU'";
                    SQL = SQL + ComNum.VBLF + " AND ROOMCODE ='234'";
                }
                else if (cboWard.Text.Trim() == "SICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND WARDCODE ='IU'";
                    SQL = SQL + ComNum.VBLF + " AND ROOMCODE ='233'";
                }
                else if (cboWard.Text.Trim() == "IQ" || cboWard.Text.Trim() == "ND" || cboWard.Text.Trim() == "NR")
                {
                    SQL = SQL + ComNum.VBLF + " AND  WARDCODE IN ('ND','IQ','NR') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND  WARDCODE = '" + cboWard.Text.Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY ROOMCODE,SNAME,PANO ";

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
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panSearch.Enabled = true;
            ssOrder_Sheet1.RowCount = 0;
            ssView1.Focus();
            ssOrder.Enabled = false;
            btnSave1.Enabled = false;
            btnCancel.Enabled = false;

            //lock 관리
            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
            }

            READ_PATIENT();
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            //DataTable dt = null;
            string SqlErr = "";
            string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            if (mstrEXEName.ToUpper() == "DRPHAR" || mstrEXEName.ToUpper() == "PHARNOR")
            {
                ComFunc.MsgBox("약제과에서는 D/C전달이 불가능합니다.", "확인");
                return;
            }

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인

            if (ssOrder_Sheet1.RowCount == 0)
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                    return; //권한 확인
                }


                for (i = 0; i < ssOrder_Sheet1.RowCount; i++)
                {

                    mstrROWID = "";
                    mbolD = false;
                    mstrFlag = "";
                    mstrQty = "";
                    mstrDQty = "";
                    mstrBun = "";
                    mstrOrderSite = "";
                    mstrTO = "";
                    mstrORDERCODE = "";
                    mstrBDATE = "";
                    mstrNurseID = "";

                    mdblQty = 0;
                    mdblDQty = 0;
                    mdblSQty = 0;
                    mdblGBDIV = 0;
                    mdblActDiv = 0;

                    mbolD = Convert.ToBoolean(ssOrder_Sheet1.Cells[i, 0].Value);
                    mstrORDERCODE = ssOrder_Sheet1.Cells[i, 3].Text.Trim();
                    mdblGBDIV = VB.Val(ssOrder_Sheet1.Cells[i, 7].Text.Trim());
                    mstrQty = ssOrder_Sheet1.Cells[i, 8].Text.Trim();
                    mdblQty = VB.Val(ssOrder_Sheet1.Cells[i, 8].Text.Trim());
                    mstrDQty = ssOrder_Sheet1.Cells[i, 9].Text.Trim();
                    mdblDQty = VB.Val(ssOrder_Sheet1.Cells[i, 9].Text.Trim());
                    mstrROWID = ssOrder_Sheet1.Cells[i, 13].Text.Trim();
                    mstrFlag = ssOrder_Sheet1.Cells[i, 14].Text.Trim();
                    mstrBun = ssOrder_Sheet1.Cells[i, 15].Text.Trim();
                    mstrTO = ssOrder_Sheet1.Cells[i, 18].Text.Trim();

                    mdblActDiv = VB.Val(ssOrder_Sheet1.Cells[i, 19].Text.Trim());

                    mstrNurseID = ssOrder_Sheet1.Cells[i, 20].Text.Trim();

                    mdblSQty = VB.Val((mdblQty - mdblDQty).ToString("###0.##"));


                    if (mstrROWID == "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("D/C 하지 못하였습니다.", "오류");
                        return;
                    }

                    //SSOrder.Col = 11:

                    if (ssOrder_Sheet1.Cells[i, 10].CellType == CellchkBox
                    && Convert.ToBoolean(ssOrder_Sheet1.Cells[i, 10].Value) == true)
                    {
                        if (CANCEL_RTN(ref SQL, ref SqlErr, strDate) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("기존자료를 D/C 하는데 오류가 발생되었습니다", "처방취소");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        if (mstrFlag == "Y")
                        {
                            if (DELETE_RTN(ref SQL, ref SqlErr, strDate) == false) //'반환
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("기존자료를 D/C 하는데 오류가 발생되었습니다", "처방취소");
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                btnCancel_Click(null, null);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// '수량반납
        /// </summary>
        /// <param name="sQL"></param>
        /// <param name="sqlErr"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        private bool DELETE_RTN(ref string SQL, ref string SqlErr, string strDate)
        {
            int intRowAffected = 0;

            if (mbolD == false) //'의사가 처방취소 "않"한 경우
            {
                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " SET GBSTATUS = ' ', ORDERSITE = ' ' WHERE ROWID = '" + mstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER  (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR , GBPICKUP, PICKUPSABUN, PICKUPDATE , MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2) ";
                SQL = SQL + ComNum.VBLF + "(SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, '*',      GBPOSITION,";
                SQL = SQL + ComNum.VBLF + "       'D-', ";
                SQL = SQL + ComNum.VBLF + " '" + Convert.ToInt32(clsType.User.Sabun) + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'DC1' ,MULTI, MULTIREMARK , DUR, '*', '" + Convert.ToInt32(clsType.User.Sabun) + "', SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','N1', ";

                //      '2015-08-10
                if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20")
                {
                    if (mstrNurseID == "")
                    {
                        SQL = SQL + ComNum.VBLF + "        'N', ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "        'Y', ";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        'N', ";
                }

                SQL = SQL + ComNum.VBLF + "        VERBAL,SYSDATE   ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + mstrROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    return false;
                }
            }

            SQL = "";
            SQL = "INSERT INTO ";
            SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 ) ";
            SQL = SQL + ComNum.VBLF + "(SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
            SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, BCONTENTS, BCONTENTS, '" + mdblSQty.ToString("###0.##") + "', '" + mdblSQty.ToString("###0.##") + "',"; //'간호사 반환은 무조건 수량으로 반환 됩니다.;
            SQL = SQL + ComNum.VBLF + "        REALNAL ,      NAL , DOSCODE,   ";
            SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";


            //'2012-03-14

            if (mstrBun == "11")
            {
                SQL = SQL + ComNum.VBLF + "        GBDIV,    ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        GBDIV,    ";
            }

            SQL = SQL + ComNum.VBLF + "        GBBOTH, GBACT,    GBTFLAG, ";

            if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20" || mstrBun == "21" || mstrBun == "23") //'주사,약 SEND 해줌
            {
                if (VB.Val(mstrQty) - VB.Val(mstrDQty) <= 0)
                {
                    SQL = SQL + ComNum.VBLF + "  ' ', ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  '*',";
                }
            }
            else
            {
                if (mstrORDERCODE == "C3710" || mstrORDERCODE == "B2522" || mstrORDERCODE == "B2521" || mstrORDERCODE == "A27" || mstrORDERCODE == "A26") //'BST 는전송함
                {
                    SQL = SQL + ComNum.VBLF + "  '*',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ' ',"; //'약주사가 아니면 SEND 않해줌;
                }
            }

            SQL = SQL + ComNum.VBLF + "        GBPOSITION, 'D+', ";
            SQL = SQL + ComNum.VBLF + " '" + Convert.ToInt32(clsType.User.Sabun) + "', ";
            SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
            SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";

            if (mbolD == false)
            {
                SQL = SQL + ComNum.VBLF + "        GBPORT,     'DC1', MULTI, MULTIREMARK, DUR ";  //'의사가취소않한경우;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        GBPORT,     'DC0', MULTI, MULTIREMARK, DUR ";  //'의사가 취소한경우;
            }

            SQL = SQL + ComNum.VBLF + "  , '*', '" + Convert.ToInt32(clsType.User.Sabun) + "', SYSDATE , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','N2',";

            //'2015-08-10

            if (mstrBun == "11" || mstrBun == "12" || mstrBun == "20")
            {
                if (mstrNurseID == "")
                {
                    SQL = SQL + ComNum.VBLF + "        'N', ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        'Y', ";
                }
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "        'N', ";
            }

            SQL = SQL + ComNum.VBLF + "  VERBAL,SYSDATE ";
            SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
            SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + mstrROWID + "') ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// 'ORDERSITE DC0:의사DC , DC1:간호사DC , DC2:내복약간호사 DC
        /// </summary>
        /// <param name="dbCon"></param>
        private bool CANCEL_RTN(ref string SQL, ref string SqlErr, string strDate)
        {
            DataTable dt = null;
            int intRowAffected = 0;

            SQL = "";
            SQL = "SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ORDERCODE, SEQNO, QTY,ORDERSITE, SLIPNO ";
            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER  ";
            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + mstrROWID + "'"; //'ROWID 로 읽어온이유는 모든내용이동일하기때문;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                mstrBDATE = dt.Rows[0]["BDATE"].ToString().Trim();
                mstrORDERCODE = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                mstrQty = dt.Rows[0]["QTY"].ToString().Trim();
                mstrOrderSite = dt.Rows[0]["ORDERSITE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            if (mstrOrderSite == "DC1")
            {
                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " SET  ORDERSITE ='CAN' WHERE ROWID ='" + mstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER ( PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP , PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 )  ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG,";

                if (VB.Val(mstrQty) == 0)
                {
                    SQL = SQL + ComNum.VBLF + "' ',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '*',";
                }

                SQL = SQL + ComNum.VBLF + "        GBPOSITION ,'D-', ";
                SQL = SQL + ComNum.VBLF + "        '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI, MULTIREMARK , DUR, '*','" + clsType.User.Sabun + "' , SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A1',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + mstrROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = " INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_OCS.OCS_IORDER (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE , MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 )  ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";
                SQL = SQL + ComNum.VBLF + "        '*',";
                SQL = SQL + ComNum.VBLF + "        GBPOSITION, ' ', ";
                SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI , MULTIREMARK, DUR , '*','" + clsType.User.Sabun + "' , SYSDATE, MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A2',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + mstrBDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + mstrORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC1'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-'";
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " SET ORDERSITE='CAN' ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + mstrBDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + mstrORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC1'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-'";
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
            }
            else //의사 DC
            {
                if (mstrBun == "11" && (mstrTO != "T" && mstrTO != "O")) //'내복약 DC일경우(퇴원약,외출약제외) 무조건 금액반영해줌
                {
                    SQL = "";
                    SQL = " INSERT INTO ";
                    SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 )  ";
                    SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                    SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                    SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                    SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";
                    SQL = SQL + ComNum.VBLF + "        '*',";
                    SQL = SQL + ComNum.VBLF + "        GBPOSITION, ' ', ";
                    SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                    SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                    SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                    SQL = SQL + ComNum.VBLF + "        GBPORT,      'DC2', MULTI, MULTIREMARK, DUR, '*', '" + clsType.User.Sabun + "', SYSDATE  , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A5',GBVERB,VERBAL,SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + mstrBDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + mstrORDERCODE + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERSITE ='DC2'";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTATUS ='D-')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        return false;
                    }
                }

                SQL = "";
                SQL = "INSERT INTO ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER  (PTNO, BDATE, SEQNO, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE, BUN, GBORDER, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBTFLAG, GBSEND, GBPOSITION, GBSTATUS, NURSEID, ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, DUR, GBPICKUP, PICKUPSABUN, PICKUPDATE, MAYAK, POWDER,GBIOE,IP,GBCHK,GBVERB,VERBAL,ENTDATE2 ) ";
                SQL = SQL + ComNum.VBLF + " (SELECT PTNO, BDATE, SEQNO,  DEPTCODE, DRCODE,  STAFFID,  SLIPNO,    ";
                SQL = SQL + ComNum.VBLF + "        ORDERCODE,   SUCODE, BUN,      GBORDER, CONTENTS, BCONTENTS, ";
                SQL = SQL + ComNum.VBLF + "        REALQTY,     QTY,    REALNAL * -1,      NAL * -1, DOSCODE,   ";
                SQL = SQL + ComNum.VBLF + "        GBINFO,      GBSELF, GBSPC,    GBNGT,   GBER,     GBPRN,     ";
                SQL = SQL + ComNum.VBLF + "        GBDIV,       GBBOTH, GBACT,    GBTFLAG, ";

                if (VB.Val(mstrQty) == 0)
                {
                    SQL = SQL + ComNum.VBLF + "' ',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '*',";
                }

                SQL = SQL + ComNum.VBLF + "        GBPOSITION , 'D-', ";
                SQL = SQL + ComNum.VBLF + " '" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "        SYSDATE,     WARDCODE,ROOMCODE, BI,     ORDERNO,  REMARK,    ";
                SQL = SQL + ComNum.VBLF + "        TO_DATE('" + strDate + "','YYYY-MM-DD'),      GBGROUP,   ";
                SQL = SQL + ComNum.VBLF + "        GBPORT,      'CAN' , MULTI, MULTIREMARK, DUR, '*', '" + clsType.User.Sabun + "', SYSDATE , MAYAK, POWDER,GBIOE,'" + clsCompuInfo.gstrCOMIP + "','A6',GBVERB,VERBAL,SYSDATE ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  ";
                SQL = SQL + ComNum.VBLF + " WHERE  ROWID = '" + mstrROWID + "') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }

                SQL = "";
                SQL = "UPDATE ";
                SQL = SQL + ComNum.VBLF + "   KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + " SET ORDERSITE='CAN' ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + mstrBDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ORDERCODE ='" + mstrORDERCODE + "'";
                SQL = SQL + ComNum.VBLF + "   AND (ORDERSITE  ='DC0'  OR ORDERSITE='DC2')";
                SQL = SQL + ComNum.VBLF + "   AND (DCDIV = 0 OR DCDIV IS NULL) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
            }

            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDC_new2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
            }

            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_PATIENT();
        }

        private void opt_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (mstrEXEName == "DRPHAR" || mstrEXEName == "PHARNOR")
                {
                    switch (((RadioButton)sender).Name)
                    {
                        case "opt0":
                        case "opt1":
                        case "opt2":

                            break;
                        default:
                            ComFunc.MsgBox("약제과에서는 약,주사 조회만 가능합니다.", "확인");
                            opt4.Checked = true;
                            return;
                    }
                }
                READ_PATIENT();
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strImFor = "";

            ssOrder_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssOrder.Enabled = false;
            btnSave1.Enabled = false;
            btnCancel.Enabled = false;

            strImFor = ssView1_Sheet1.Cells[e.Row, 0].Text.Trim() + " ";
            strImFor = strImFor + ssView1_Sheet1.Cells[e.Row, 1].Text.Trim() + " ";
            mstrPtNo = ssView1_Sheet1.Cells[e.Row, 2].Text.Trim();

            clsLockCheck.GstrLockPtno = mstrPtNo;

            strImFor = strImFor + " " + mstrPtNo;
            lblImfor.Text = strImFor;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strMsg = "";

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'해당 오더 전송 중 여부 확인
                if (mstrEXEName.ToUpper() == "DRPHAR" || mstrEXEName.ToUpper() == "PHARNOR")
                {
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT A.*, TO_CHAR(A.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE1, SLIPNO, A.ROWID , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE1  ";
                    SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  A";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO     = '" + mstrPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE >= TO_DATE('" + dtpDateFr.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND A.BDATE <= TO_DATE('" + dtpDateTo.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND A.GBSEND ='*' ";

                    //    '2014-02-27
                    if (mstrWardCode == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('EI') ";
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strMsg = dt.Rows[0]["BDATE1"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox(strMsg + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE1\r";
                    SQL += "   FROM KOSMOS_OCS.OCS_IORDER\r";
                    SQL += "  WHERE Ptno       = '" + mstrPtNo + "'\r";
                    SQL += "    AND BDATE >= TO_DATE('" + dtpDateFr.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')\r";
                    SQL += "    AND BDATE <= TO_DATE('" + dtpDateTo.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')\r";
                    SQL += "    AND DEPTCODE  != 'ER'\r";
                    SQL += "    AND (ACCSEND IS NULL or ACCSEND = 'Z')\r";
                    SQL += "    AND ORDERSITE not in ('NDC', 'ERO')\r";
                    SQL += "    AND GBPRN != 'P'\r";
                    SQL += "    AND GBSEND = ' '\r";
                    SQL += "    AND GBSTATUS != 'D'\r";
                    SQL += "    AND SUCODE IS NOT NULL\r";
                    SQL += "    AND SLIPNO != '0106'\r";    //자가약

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

                        clsDB.setRollbackTran(clsDB.DbCon);
                        strMsg = dt.Rows[0]["BDATE1"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox(strMsg + "일자에 미 전송된 오더가 있습니다.잠시후에 작업 해주세요..", "확인");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                }

                SQL = "";
                SQL = "SELECT GBSTS, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + mstrPtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                if (DATA_READ(ref SqlErr, ref SQL) == true)
                {
                    ssOrder.Enabled = true;
                    btnSave1.Enabled = true;
                    btnCancel.Enabled = true;

                    panSearch.Enabled = false;
                    ssOrder.Focus();

                }
                else
                {
                    ssOrder_Sheet1.RowCount = 0;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool DATA_READ(ref string SqlErr, ref string SQL)
        {
            bool RtnVal = false;
            int i = 0;
            int h = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;

            string strSpecSQL = "";
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            if (dtpDateTo.Value < dtpDateFr.Value)
            {
                dtpDateTo.Value = Convert.ToDateTime(strSysDate);
            }

            SQL = "";
            SQL = "SELECT A.*, TO_CHAR(A.ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE1, SLIPNO, A.ROWID , ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE1  ";
            SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_OCS.OCS_IORDER  A";
            SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO     = '" + mstrPtNo + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.BDATE >= TO_DATE('" + dtpDateFr.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND A.BDATE <= TO_DATE('" + dtpDateTo.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND A.SLIPNO NOT IN ('A1','A2','A4') "; //'V/S, BED REST,DIET    ORDER,SPECIAL ORDER,제외;
            SQL = SQL + ComNum.VBLF + "  AND A.GBPRN NOT IN ('P','S')";

            if (opt0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND A.BUN IN ('11','12')   "; //'약;
            }

            if (opt1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND  A.BUN = '20'  ";      // '주사;
            }

            if (opt4.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND A.BUN NOT IN ('11','12','20')   "; //'약;
            }


            if (opt5.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND  A.BUN IN ('41','42','43','44','45','46','47','48','49','65','66','67','68','69','70','71','72','73')  ";  //'내시경, 방사선, 기능검사;
            }

            if (opt3.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND ((A.BUN >= '41' AND A.BUN <= '70') OR A.BUN ='37')  "; //'검사;
                SQL = SQL + ComNum.VBLF + " AND A.GBSTATUS IN (' ','D+')  ";       //'검사일때는 의사취소는 나타내지 않기위해서;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND A.GBSTATUS IN (' ','D','D+')  ";
            }

            if (opt6.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND A.SUCODE IN ('C3710','C3711') ";
                SQL = SQL + ComNum.VBLF + " AND A.ORDERSITE NOT IN ('CAN','DC2','OPDX') ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND (A.ORDERSITE NOT IN ('CAN','DC2','OPDX') OR A.ORDERSITE IS NULL) "; //'간호사가 취소한 ORDER에 대해서는 보여주지않는다;

                if (mstrWardCode == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIOE IN ('EI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.GBACT <>'*'";
                }
            }

            //    '2014-07-08 약제과 의뢰서 마약,향정주사 간호사DC불가
            if (Convert.ToDateTime(strSysDate) >= Convert.ToDateTime("2014-07-08"))
            {
                SQL = SQL + ComNum.VBLF + "  AND TRIM(A.SUCODE) NOT IN ( SELECT TRIM(JEPCODE) FROM KOSMOS_ADM.DRUG_JEP WHERE (CHENGGU ='09' OR (SUB='16' AND BUN='2') )  ) ";
            }

            SQL = SQL + ComNum.VBLF + " AND GBSEND =' ' "; //'전송완료된것만 처리되도록함;

            //    '2015-09-02 의사구두확정 제외
            SQL = SQL + ComNum.VBLF + " AND (A.VERBC IS NULL OR A.VERBC <> 'Y') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.BDATE, A.SEQNO ,A.ORDERCODE ,A.ORDERNO,  A.GBSTATUS DESC";


            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return RtnVal;
            }

            if (dt.Rows.Count > 0)
            {

                clsLockCheck.GstrLockPtno = mstrPtNo;
                clsLockCheck.GstrLockRemark = clsType.User.JobName + " 님이 간호창에서 작업중 !! ";

                //'LOCKING

                if (mstrEXEName.ToUpper() == "DRPHAR" || mstrEXEName.ToUpper() == "PHARNOR")
                {
                }
                else
                {
                    if (clsLockCheck.IpdOcs_Lock_Insert_NEW() != "OK")
                    {
                        btnCancel_Click(null, null);
                        return RtnVal;
                    }
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                // strFlag = "OK"

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        if (dt.Rows[i]["BDATE1"].ToString().Trim() != dt.Rows[i - 1]["BDATE1"].ToString().Trim())
                        {
                            ssOrder_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[i, 2].Text = ComFunc.LeftH(clsIpdNr.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(),
                                                                    ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2),
                                                                    dt.Rows[i]["BUN"].ToString().Trim()), 7).Trim();
                    ssOrder_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                    if ((VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 11 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 20)
                    || VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) == 23)
                    {
                        if (VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0)
                        {
                            ssOrder_Sheet1.Cells[i, 6].Text = " ";

                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        }
                    }

                    ssOrder_Sheet1.Cells[i, 8].Text = dt.Rows[i]["QTY"].ToString().Trim();


                    // SSOrder.Col = 8:
                    if (VB.Val(dt.Rows[i]["GBDIV"].ToString().Trim()) == 0)
                    {
                        if ((VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 41 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 70)
                        || dt.Rows[i]["BUN"].ToString().Trim() == "37" || dt.Rows[i]["GBSTATUS"].ToString().Trim() != "D+")
                        {
                            ssOrder_Sheet1.Cells[i, 7].Text = "1"; //'XRAY,LAB 은 무조건 1로 SETTING
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[i, 11].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 12].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                    ssOrder_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, 15].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 17].Text = dt.Rows[i]["GBSTATUS"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 18].Text = dt.Rows[i]["GBTFLAG"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, 19].Text = dt.Rows[i]["ACTDIV"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, 20].Text = dt.Rows[i]["NURSEID"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 21].Text = dt.Rows[i]["GBVERB"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 22].Text = dt.Rows[i]["VERBC"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, 23].Text = dt.Rows[i]["POWDER"].ToString().Trim();

                    if (dt.Rows[i]["VERBC"].ToString().Trim() == "C")
                    {
                        ssOrder_Sheet1.Cells[i, 20].BackColor = Color.FromArgb(0, 255, 0);
                    }

                    switch (dt.Rows[i]["GBORDER"].ToString().Trim())
                    {
                        case "F":
                            ssOrder_Sheet1.Cells[i, 2].Text = "Pre";
                            break;
                        case "T":
                            ssOrder_Sheet1.Cells[i, 2].Text = "Post";
                            break;
                        case "M":
                            ssOrder_Sheet1.Cells[i, 2].Text = "Adm";
                            break;
                    }

                    if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D") //'의사가처방취소
                    {
                        ssOrder_Sheet1.Cells[i, 2].Text = "D/C";
                        ssOrder_Sheet1.Cells[i, 0].Value = true;
                        ssOrder_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(230, 230, 255);
                    }
                    else if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D+") //'간호사가 처방취소
                    {
                        ssOrder_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(255, 208, 208); //'다시반환 못하게 막음
                        ssOrder_Sheet1.Cells[i, 9].Locked = true;

                        ssOrder_Sheet1.Rows.Get(i + 1).BackColor = Color.FromArgb(255, 208, 208); //'다시반환 못하게 막음
                        ssOrder_Sheet1.Cells[i + 1, 9].Locked = true;

                        CellchkBox.Caption = "취소";
                        CellchkBox.TextAlign = FarPoint.Win.ButtonTextAlign.TextRightPictLeft;

                        ssOrder_Sheet1.Cells[i, 10].CellType = CellchkBox;

                        //간호부요청으로 어제꺼가지 가능하도록 함
                        if (Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()) < Convert.ToDateTime(strSysDate).AddDays(-1))
                        {
                            ssOrder_Sheet1.Cells[i, 10].Locked = true;
                        }
                    }

                    // '선수납 품목중에서 수납 한내용에 대해서 반환못하게 LOCK
                    if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                    {
                        ssOrder_Sheet1.Cells[i, 9].Locked = true;
                    }

                    //  '입원일자보다 큰자료는 dc불가능
                    if (Convert.ToDateTime(strInDate) > Convert.ToDateTime(dt.Rows[i]["BDATE1"].ToString().Trim()))
                    {
                        ssOrder_Sheet1.Rows.Get(i).Locked = true;
                    }


                    // SSOrder.Col = 5

                    if ((VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 41 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 64) || dt.Rows[i]["BUN"].ToString().Trim() == "37") //'검사일경우 37: 수혈
                    {
                        SQL = "";
                        SQL = "SELECT SPECNO FROM KOSMOS_OCS.EXAM_ORDER ";
                        SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE1"].ToString().Trim() + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + dt.Rows[i]["PTNO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "   AND IPDOPD = 'I'";
                        SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt.Rows[i]["ORDERNO"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return RtnVal;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssOrder_Sheet1.Cells[i, 4].Text = dt1.Rows[0]["SPECNO"].ToString().Trim();
                            //         '검체번호를 분리
                            strSpecSQL = "";
                            for (h = 1; h < dt1.Rows[0]["SPECNO"].ToString().Trim().Length + 1; h = h + 10)
                            {
                                strSpecSQL = strSpecSQL + ", " + VB.Mid(dt1.Rows[0]["SPECNO"].ToString().Trim(), h, 10);
                            }

                            strSpecSQL = VB.Left(strSpecSQL, strSpecSQL.Length - 1);

                            SQL = "";
                            SQL = "SELECT COUNT(*) CNT FROM KOSMOS_OCS.EXAM_SPECMST ";
                            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mstrPtNo.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND SPECNO IN ('" + strSpecSQL + "') ";
                            SQL = SQL + ComNum.VBLF + "  AND STATUS IN ('01','02','03','04','05') ";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                dt.Dispose();
                                dt = null;
                                dt1.Dispose();
                                dt1 = null;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return RtnVal;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                if (VB.Val(dt2.Rows[0]["CNT"].ToString().Trim()) > 0)
                                {
                                    ssOrder_Sheet1.Rows.Get(i).Locked = true;
                                }
                            }


                            dt2.Dispose();
                            dt2 = null;
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    if (Order_Read_Move(dt, i, ref SQL, ref SqlErr) == false)
                    {
                        dt.Dispose();
                        dt = null;
                        return RtnVal;
                    }

                    if (String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0
                            && String.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A9") <= 0)
                    {
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[i, 3].Text = "";
                            ssOrder_Sheet1.Cells[i, 4].Text = ssOrder_Sheet1.Cells[i, 4].Text.Trim()
                            + " " + dt.Rows[i]["REMARK"].ToString().Trim();

                            ssOrder_Sheet1.Cells[i, 1, i, ssOrder_Sheet1.ColumnCount - 1].ForeColor
                            = Color.FromArgb(128, 0, 0);
                        }
                    }

                    if (dt.Rows[i]["ORDERSITE"].ToString().Trim() == "DRUG")
                    {
                        ssOrder_Sheet1.Cells[i, 1, i, ssOrder_Sheet1.ColumnCount - 1].BackColor
                                = Color.FromArgb(234, 234, 255);
                    }
                }

            }

            dt.Dispose();
            dt = null;

            RtnVal = true;

            return RtnVal;
        }

        private bool Order_Read_Move(DataTable dt, int intRow, ref string SQL, ref string SqlErr)
        {
            bool RtnVal = false;
            DataTable dt1 = null;

            SQL = "";
            SQL = " SELECT    DISPHEADER CDISPHEADER, ORDERNAME CORDERNAME,         ";
            SQL = SQL + ComNum.VBLF + " DISPRGB CDISPRGB, GBBOTH CGBBOTH, GBINFO CGBINFO,     ";
            SQL = SQL + ComNum.VBLF + " GBQTY CGBQTY, GBDOSAGE CGBDOSAGE, NEXTCODE CNEXTCODE, ";
            SQL = SQL + ComNum.VBLF + " ORDERNAMES CORDERNAMES, GBIMIV CGBIMIV ,HNAME";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_ORDERCODE A,KOSMOS_OCS.OCS_DRUGINFO_NEW B";
            SQL = SQL + ComNum.VBLF + " WHERE ORDERCODE = '" + dt.Rows[intRow]["ORDERCODE"].ToString().Trim() + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SLIPNO    = '" + dt.Rows[intRow]["SLIPNO"].ToString().Trim() + "' ";
            SQL = SQL + ComNum.VBLF + " AND A.ORDERCODE=B.SUNEXT(+) ";

            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return RtnVal;
            }

            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {


                    ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim()
                    + dt1.Rows[0]["HNAME"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();

                }
                else
                {
                    if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                    {
                        ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim()
                        + " " + dt1.Rows[0]["CDISPHEADER"].ToString().Trim()
                        + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim()
                        + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                    }

                    switch (dt.Rows[intRow]["GBPRN"].ToString().Trim())
                    {
                        case "S":
                            //ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(A)";
                            ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(선수납)";
                            break;
                        case "A":
                            //ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(선)";
                            ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(선수납)";
                            break;
                        case "B":
                            //ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(수)";
                            ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + "(수납완료)";
                            break;
                    }
                }

                if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")//'용법
                {
                    ssOrder_Sheet1.Cells[intRow, 5].Text = dt.Rows[intRow]["GBINFO"].ToString().Trim();
                }
                else
                {
                    if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[intRow, 5].Text = Read_Dosage(dt.Rows[intRow]["DOSCODE"].ToString().Trim());
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[intRow, 5].Text = Read_Specman(dt.Rows[intRow]["DOSCODE"].ToString().Trim(), dt.Rows[intRow]["SLIPNO"].ToString().Trim());
                    }
                }

                //SSOrder.Col = 5

                if (dt1.Rows[0]["CGBBOTH"].ToString().Trim() == "1")
                {
                    ssOrder_Sheet1.Cells[intRow, 4].Text = ComFunc.LeftH(ssOrder_Sheet1.Cells[intRow, 4].Text.Trim(), 30) + dt.Rows[intRow]["GBINFO"].ToString().Trim();
                }

                if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                    && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A9") <= 0)
                {
                    ssOrder_Sheet1.Cells[intRow, 1, intRow, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                }
                else
                {
                    if (ssOrder_Sheet1.Cells[intRow, 2].Text == "D/C")
                    {
                        ssOrder_Sheet1.Cells[intRow, 2].ForeColor = Color.FromArgb(255, 0, 0);

                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[intRow, 2].ForeColor
                            = ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                }

            }
            else
            {
                if (String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A1") >= 0
                        && String.Compare(dt.Rows[intRow]["SLIPNO"].ToString().Trim(), "A9") <= 0)
                {
                    ssOrder_Sheet1.Cells[intRow, 4].Text = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim() + " " + dt.Rows[intRow]["REMARK"].ToString().Trim();
                }
                else
                {
                    ssOrder_Sheet1.Cells[intRow, 4].Text = "삭제된 코드입니다.. 변경요망";
                }

            }
            dt1.Dispose();
            dt1 = null;

            RtnVal = true;

            return RtnVal;
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
                SQL = SQL + ComNum.VBLF + "WHERE SPECCODE = '" + strDosCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SLIPNO   = '" + strSlipno + "'   ";

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

        private void ssOrder_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {


        }

        private void ssOrder_EditModeOff(object sender, EventArgs e)
        {
            if (ssOrder_Sheet1.ActiveColumnIndex == 9)
            {
                string strQty = "";
                string strDQty = "";
                string strBun = "";
                string strDel = "";
                string strCODE = "";
                string strPowder = "";
                string strORDERCODE = "";  //'약코드

                int intRow = ssOrder_Sheet1.ActiveRowIndex;

                string strOK = "OK";

                strDel = ssOrder_Sheet1.Cells[intRow, 0].Text.Trim();
                strORDERCODE = ssOrder_Sheet1.Cells[intRow, 3].Text.Trim();
                strCODE = ssOrder_Sheet1.Cells[intRow, 4].Text.Trim();
                strQty = ssOrder_Sheet1.Cells[intRow, 8].Text.Trim();
                strDQty = ssOrder_Sheet1.Cells[intRow, 9].Text.Trim();
                strBun = ssOrder_Sheet1.Cells[intRow, 15].Text.Trim();
                strPowder = ssOrder_Sheet1.Cells[intRow, 23].Text.Trim();

                if (VB.Val(strDQty) <= 0)
                {
                    ComFunc.MsgBox("<< 0 >> 이상의 값은 넣어주세요", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder_Sheet1.Cells[intRow, 13].Text = "";
                    ssOrder.Focus();
                    return;
                }

                if (VB.Val(strDQty) <= 0)
                {
                    ComFunc.MsgBox("<< 0 >> 이상의 값은 넣어주세요", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder_Sheet1.Cells[intRow, 13].Text = "";
                    ssOrder.Focus();
                    return;
                }

                //    '2012-04-04 김현욱 작업(약제과 의뢰서 작업 2012년 접수번호 181번)
                if (READ_NOT_PART_DC(strORDERCODE) == true && VB.Val(strQty) != VB.Val(strDQty))
                {
                    ComFunc.MsgBox("지정된 약제는 부분반환이 불가능합니다. 문의사항은 약제과로 연락 바랍니다.", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder_Sheet1.Cells[intRow, 13].Text = "";
                    ssOrder.Focus();
                    return;
                }

                //    '2015-11-14
                if (strPowder == "1")
                {
                    ComFunc.MsgBox("산제(POWDER) 약제는 부분반환이 불가능합니다..", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder_Sheet1.Cells[intRow, 13].Text = "";
                    ssOrder.Focus();
                    return;
                }

                // ''允(2006 - 10 - 12) 심사과 요청 주사 의사오더 반환 후 간호사 다시 전체 반환 않되도록 처리함
                if (strDel == "1" && strBun == "20" && VB.Val(strQty) == VB.Val(strDQty))
                {
                    ComFunc.MsgBox("주사약제 [" + strCODE + "] 는 의사창에서 전체 반환되었습니다", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder.Focus();
                    return;
                }

                //    '2012-03-12
                if (strDel == "1" && strBun == "11" && VB.Val(strQty) == VB.Val(strDQty))
                {
                    ComFunc.MsgBox("경구약제 [" + strCODE + "] 는 의사창에서 전체 반환되었습니다", "확인");
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder.Focus();
                    return;
                }

                DATA_CHECK(ref strOK, intRow);

                if (strOK == "NO")
                {
                    ssOrder_Sheet1.Cells[intRow, 9].Text = "";
                    ssOrder_Sheet1.Cells[intRow, 13].Text = "";
                    ssOrder.Focus();
                }

                ssOrder_Sheet1.Cells[intRow, 14].Text = "Y";
            }
        }

        private void DATA_CHECK(ref string strOK, int intRow)
        {
            string strMsg = "";
            if (strOK == "OK")
            {
                Delete_Lab_Check(ref strOK, intRow);
            }
            if (strOK == "OK")
            {
                Delete_Endo_Check(ref strOK, intRow);
            }
            if (strOK == "OK")
            {
                Delete_Xray_Check(ref strOK, intRow);
            }
            if (strOK == "OK")
            {
                Delete_Ekg_Check(ref strOK, intRow);
            }
            if (strOK == "OK")
            {
                Delete_Pharmacy_Check(ref strOK, intRow);
            }

            if (strOK != "OK")
            {
                switch (strOK)
                {
                    case "EXAM":
                        strMsg = "검사실에 접수되어 취소할수 없습니다.";
                        break;
                    case "ENDO":
                        strMsg = "내시경실에 접수되어 취소할수 없습니다.";
                        break;
                    case "XRAY":
                        strMsg = "방사선과에 접수되어 취소할수 없습니다.";
                        break;
                    case "PTEX":
                        strMsg = "물리치료실에 접수되어 취소할수 없습니다.";
                        break;
                    case "EKG":
                        strMsg = "기능검사가 접수되어 취소할수 없습니다.";
                        break;
                    case "PHAR":
                        strMsg = "취소할수 없는 오더입니다.";
                        break;
                }
                strOK = "NO";
                ComFunc.MsgBox(strMsg, "취소불가");
            }
        }

        private void Delete_Lab_Check(ref string strOK, int intRow)
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strSpecSQL = "";

            try
            {
                if (VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) >= 41 && VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) < 65)
                {
                    SQL = "";
                    SQL = " SELECT SPECNO FROM KOSMOS_OCS.EXAM_ORDER ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO       = '" + mstrPtNo.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO    = " + VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim());

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssOrder_Sheet1.Cells[intRow, 4].Text = dt.Rows[0]["SPECNO"].ToString().Trim();
                        //         '검체번호를 분리
                        strSpecSQL = "";
                        for (i = 1; i < dt.Rows[0]["SPECNO"].ToString().Trim().Length + 1; i = i + 10)
                        {
                            strSpecSQL = strSpecSQL + ", " + ComFunc.MidH(dt.Rows[0]["SPECNO"].ToString().Trim(), i, 10);
                        }

                        strSpecSQL = VB.Left(strSpecSQL, strSpecSQL.Length - 1);

                        SQL = "";
                        SQL = "SELECT COUNT(*) CNT FROM KOSMOS_OCS.EXAM_SPECMST ";
                        SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + mstrPtNo.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SPECNO IN ('" + strSpecSQL + "') ";
                        SQL = SQL + ComNum.VBLF + "  AND STATUS IN ('01','02','03','04','05') ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            dt.Dispose();
                            dt = null;
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 0)
                            {
                                strOK = "EXAM";
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
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

        }

        private void Delete_Endo_Check(ref string strOK, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ssOrder_Sheet1.Cells[intRow, 15].Text.Trim() == "48" && ssOrder_Sheet1.Cells[intRow, 15].Text.Trim() == "49")
                {
                    SQL = "";
                    SQL = " SELECT GBSUNAP FROM KOSMOS_OCS.ENDO_JUPMST ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO       = '" + mstrPtNo.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO    = " + VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim());
                    SQL = SQL + ComNum.VBLF + "  AND GBSUNAP    = '1' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOK = "ENDO";
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
        }

        private void Delete_Xray_Check(ref string strOK, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) >= 65 && VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) < 74)
                {
                    if (VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim()) == 0)
                    {
                        return;
                    }

                    SQL = "";
                    SQL = " SELECT GBEND FROM KOSMOS_PMPA.XRAY_DETAIL ";
                    SQL = SQL + ComNum.VBLF + "WHERE PANO       = '" + mstrPtNo.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO    = " + VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim());
                    SQL = SQL + ComNum.VBLF + "  AND GBEND      = '1'  ";
                    SQL = SQL + ComNum.VBLF + "  AND GBRESERVED NOT IN ('1','2')  ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOK = "XRAY";
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
        }

        private void Delete_Ekg_Check(ref string strOK, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) >= 43 && VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim()) <= 71)
                {
                    switch (Convert.ToInt32(VB.Val(ssOrder_Sheet1.Cells[intRow, 15].Text.Trim())))
                    {
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 50:
                        case 71:
                            break;
                        default:
                            return;
                    }

                    SQL = "";
                    SQL = " SELECT GBJOB   FROM KOSMOS_OCS.ETC_JUPMST   ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO       = '" + mstrPtNo.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO    = " + VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim());
                    SQL = SQL + ComNum.VBLF + "  AND GBJOB      NOT IN ('1','9','2') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOK = "EKG";
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
        }

        private void Delete_Pharmacy_Check(ref string strOK, int intRow)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim()) != 0)
                {
                    SQL = "";
                    SQL = "SELECT GBOUT FROM KOSMOS_OCS.OCS_PHARMACY";
                    SQL = SQL + ComNum.VBLF + " WHERE PTNO       = '" + mstrPtNo.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ORDERNO    = '" + VB.Val(ssOrder_Sheet1.Cells[intRow, 16].Text.Trim()) + "'";
                    SQL = SQL + ComNum.VBLF + "   AND GBOUT ='N' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strOK = "PHAR";
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


        }

        private bool READ_NOT_PART_DC(string argCode)
        {
            bool RtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT JEPCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_SETCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = '07'";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND JEPCODE = '" + argCode.Trim() + "' ";

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

        private void frmDC_new2_Activated(object sender, EventArgs e)
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
    }
}
