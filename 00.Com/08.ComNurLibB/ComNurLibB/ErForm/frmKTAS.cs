using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComEmrBase;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmKTAS.cs
    /// Description     : KTAS 코드 선택
    /// Author          : 유진호
    /// Create Date     : 2018-04-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\FrmKTAS.frm(FrmKTAS) >> frmKTAS.cs" />
    public partial class frmKTAS : Form
    {
        private string FstrIDNO = "";
        private string FstrINDT = "";
        private string FstrINTM = "";
        private string FstrKTID = "";
        private string FstrAge = "";
        private string FstrROWID = "";
        private string FstrJDATE = "";

        private string FstrKTAS = "";
        private string FstrINFECT = "";

        private string FstrReKTAS = "";
        
        public frmKTAS()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIDNO">환자번호</param>
        /// <param name="strINDT">내원일자</param>
        /// <param name="strINTM">내원시간</param>
        /// <param name="strKTID">PTMIKTID</param>
        /// <param name="strAge">나이</param>
        /// <param name="strROWID">ROWID</param>
        /// <param name="strJDATE">접수일자</param>
        public frmKTAS(string strIDNO, string strINDT, string strINTM, string strKTID, string strAge, string strROWID, string strJDATE)
        {
            InitializeComponent();
            FstrIDNO = strIDNO;
            FstrINDT = strINDT;
            FstrINTM = strINTM;
            FstrKTID = strKTID;
            FstrAge = strAge;
            FstrROWID = strROWID;
            FstrJDATE = strJDATE;
        }

        private void frmKTAS_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            
            tab_1.SelectedIndex = 1;
            SCREEN_CLEAR();
            if (VB.Val(FstrAge) < 15)
            {
                rdoCodeB.Checked = true;
                rdoDiagB.Checked = true;
            }
            else
            {
                rdoCodeA.Checked = true;
                rdoDiagA.Checked = true;
            }
            
            cboAORT.Items.Clear();
            cboAORT.Items.Add("");
            cboAORT.Items.Add("A");
            cboAORT.Items.Add("O");
            cboAORT.Items.Add("R");
            cboAORT.Items.Add("T");
            cboAORT.SelectedIndex = 0;
            
            txtDisease.Text = "";
            txtHCnt.Text = "";
            txtHiBp.Text = "";
            txtLoBp.Text = "";
            txtPuLs.Text = "";
            txtBrTh.Text = "";
            txtBdHt.Text = "";
            txtVoxs.Text = "";

            cboReSp.Items.Clear();
            cboReSp.Items.Add(" ");
            cboReSp.Items.Add("A.Alert");
            cboReSp.Items.Add("V.Verbal response");
            cboReSp.Items.Add("P.Painful response");
            cboReSp.Items.Add("U.Unresponsive");
            cboReSp.SelectedIndex = 1;


            FstrReKTAS = clsErNr.READ_KTAS(clsDB.DbCon, FstrIDNO, FstrINDT, FstrINTM);
            if (FstrReKTAS != "")
            {
                btnKtasRepeat.Enabled = true;                
            }
        }

        private bool SAVE_DATA()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strPTMIIDNO = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";
            string strPTMIKTID = "";
            string strPTMIKPR = "";
            string strPTMIKTS = "";
            string strPTMIKTDT = "";
            string strPTMIKTTM = "";
            string strPTMIKJOB = "";
            string strPTMIKIDN = "";
            //string strWRITEDATE = "";
            string strWRITESABUN = "";

            string strKTAS = "";
            string strFIRST = "";
            //string strKTASLEVL = "";


            try
            {
                strFIRST = "";
                strPTMIKTID = txtKtid.Text.Trim();
                strPTMIIDNO = FstrIDNO;
                strPTMIINDT = FstrINDT;
                strPTMIINTM = FstrINTM;

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + " * ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI A ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI B ";
                SQL = SQL + ComNum.VBLF + "               WHERE B.PTMIIDNO  = A.PTMIIDNO ";
                SQL = SQL + ComNum.VBLF + "                    AND B.PTMIINDT  = A.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "                    AND B.PTMIINTM = A.PTMIINTM ) ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIIDNO = '" + strPTMIIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINDT = '" + strPTMIINDT + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINTM = '" + strPTMIINTM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(A.PTMIOTDT) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIEMCD= 'C24C0083' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("퇴실일자를 네디스서버에 전송후에는 더이상 KTAS를 추가 입력할수 없습니다. ");
                    dt.Dispose();
                    dt = null;
                    return rtVal;
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
            }


            try
            {
                if (strPTMIKTID == "")
                {
                    SQL = " SELECT * FROM KOSMOS_PMPA.NUR_ER_KTAS ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPTMIIDNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strPTMIINDT + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ComFunc.MsgBox("환자 정보가 갱신되었습니다. " + ComNum.VBLF +
                                       "환자를 다시 선택하여 입력하시기 바랍니다." + ComNum.VBLF +
                                       "(ex. 다른 PC에서 이미 분류를 하였는 경우)");
                        dt.Dispose();
                        dt = null;
                        return rtVal;
                    }
                }                
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            if (strPTMIIDNO == "")
            {
                if (ComFunc.MsgBoxQ("환자 정보가 존재하지 않습니다. 아래와 같이 대체됩니다." + ComNum.VBLF +
                                    "★ 환자등록번호 <= 선별번호" + ComNum.VBLF +
                                    "★ 내원일시 <= 분류일시" + ComNum.VBLF +
                                    "계속 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return rtVal;
                }
            }


            if (strPTMIKTID == "")
            {
                if (ComFunc.MsgBoxQ("내원 후 처음하는 중증도 분류입니다. 계속하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return rtVal;
                }

                strFIRST = "OK";                
                strPTMIKTID = "999999999999";     //'2018-04-01 add 네디스 오류 롱 인한 수정
            }

            if (chkDate.Checked == true)
            {
                strPTMIKTDT = VB.Replace(txtKtdt.Text, "-", "");
                strPTMIKTTM = VB.Replace(txtKttm.Text, ":", "");
            }
            else
            {
                strPTMIKTDT = VB.Replace(clsPublic.GstrSysDate, "-", "");
                strPTMIKTTM = VB.Replace(clsPublic.GstrSysTime, ":", "");
            }

            if (strPTMIIDNO == "")
            {
                strPTMIIDNO = strPTMIKTID;
                strPTMIINDT = strPTMIKTDT;
                strPTMIINTM = strPTMIKTTM;
            }

            strWRITESABUN = VB.Trim(txtSabun.Text);
            strPTMIKIDN = VB.Trim(txtKidn.Text);
            strPTMIKJOB = VB.Left(cboKjob.Text, 1);
            strPTMIKPR = VB.Left(FstrKTAS, 5) + FstrINFECT;
            strPTMIKTS = VB.Right(FstrKTAS, 1);

            // 예외체크
            if (strPTMIKIDN == "")
            {
                ComFunc.MsgBox("면허,자격번호가 공란입니다.");
                return rtVal;
            }

            if (strPTMIKJOB == "")
            {
                ComFunc.MsgBox("직종이 공란입니다.");
                return rtVal;
            }

            if (strPTMIKPR == "")
            {
                ComFunc.MsgBox("중증도분류 과정이 선택되지 않았습니다.");
                return rtVal;
            }

            if (VB.Len(strPTMIKPR) != 6)
            {
                ComFunc.MsgBox("중증도분류 과정이 완성되지 않았습니다.");
                return rtVal;
            }

            if (strPTMIKTS == "")
            {
                ComFunc.MsgBox("중증도분류 결과가 선택되지 않았습니다.");
                return rtVal;
            }

            if (VB.Len(strPTMIKTS) != 1)
            {
                ComFunc.MsgBox("중증도분류 결과값이 오류입니다.");
                return rtVal;
            }

            if (!(VB.IsNumeric(strPTMIKTS)))
            {
                ComFunc.MsgBox("중증도분류 결과값이 숫자가 아닙니다. 다시 선택하여 주시기 바랍니다.");
                return rtVal;
            }

            if (string.Compare(strPTMIINDT + strPTMIINTM, strPTMIKTDT + strPTMIKTTM) > 0)
            {
                ComFunc.MsgBox("중증도분류 일시가 접수시간보다 빠를수 없습니다. ");
                return rtVal;
            }



            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_KTAS( ";
                SQL = SQL + ComNum.VBLF + "  PTMIIDNO, PTMIINDT, PTMIINTM, PTMIKTID,";
                SQL = SQL + ComNum.VBLF + "  PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + "  PTMIKJOB, PTMIKIDN, WRITEDATE, WRITESABUN, REALSABUN, ";
                SQL = SQL + ComNum.VBLF + "  SEQNO ";
                SQL = SQL + ComNum.VBLF + "  ) VALUES (";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIIDNO + "','" + strPTMIINDT + "','" + strPTMIINTM + "','" + strPTMIKTID + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIKPR + "','" + strPTMIKTS + "','" + strPTMIKTDT + "','" + strPTMIKTTM + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIKJOB + "','" + strPTMIKIDN + "', SYSDATE, " + strWRITESABUN + "," + clsType.User.Sabun + ",";
                SQL = SQL + ComNum.VBLF + (strFIRST == "OK" ? "1" : "2") + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsErNr.GstrHelpCode = strPTMIKTID;
                strKTAS = VB.Left(strPTMIKPR, 5) + VB.Trim(strPTMIKTS) + VB.Right(strPTMIKPR, 1);

                if (SaveXML(clsDB.DbCon, strPTMIIDNO, strPTMIINDT, strPTMIKPR, strPTMIKTS, strPTMIKTDT, strPTMIKTTM, strKTAS) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    //clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                }

                if (VB.Trim(txtDisease.Text) != "" || VB.Trim(txtHiBp.Text) != "" || VB.Trim(txtLoBp.Text) != "" ||
                   VB.Trim(txtPuLs.Text) != "" || VB.Trim(txtBrTh.Text) != "" || VB.Trim(txtBdHt.Text) != "" ||
                   VB.Trim(cboAORT.Text) != "" || VB.Trim(txtVoxs.Text) != "" || chkDisable.Checked == true || chkDisable2.Checked == true)
                {
                    clsErNr.GstrHelpName = VB.Trim(txtDisease.Text) + "||" + VB.Trim(txtHiBp.Text) + "||" + VB.Trim(txtLoBp.Text) + "||" +
                                   VB.Trim(txtPuLs.Text) + "||" + VB.Trim(txtBrTh.Text) + "||" + VB.Trim(txtBdHt.Text) + "||" +
                                   VB.Trim(cboAORT.Text) + "||" + VB.Trim(txtVoxs.Text) + "||" + VB.Trim(txtHCnt.Text) + "||" +
                                   VB.Left(cboReSp.Text, 1) + "||" +
                                   (chkDisable.Checked == true ? "true" : "false") + "||" +
                                   (chkDisable2.Checked == true ? "true" : "false");  //'2017-11-09 chkDisable2 add
                }
                else
                {
                    clsErNr.GstrHelpName = "";
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void cboAORT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtVoxs.Focus();
            }
        }

        private void chkDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisable.Checked == true)
            {
                //'TxtHCnt.Text = "-1"
                txtHiBp.Text = "-1";
                txtLoBp.Text = "-1";
                txtPuLs.Text = "-1";
                txtBrTh.Text = "-1";
                txtBdHt.Text = "-1";

                //'TxtHCnt.Enabled = False
                txtHiBp.Enabled = false;
                txtLoBp.Enabled = false;
                txtPuLs.Enabled = false;
                txtBrTh.Enabled = false;
                txtBdHt.Enabled = false;
            }
            else
            {
                //'TxtHCnt.Enabled = True
                txtHiBp.Enabled = true;
                txtLoBp.Enabled = true;
                txtPuLs.Enabled = true;
                txtBrTh.Enabled = true;
                txtBdHt.Enabled = true;
            }
        }

        private void chkDisable2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisable2.Checked == true)
            {
                //'TxtHCnt.Text = "-1"
                txtHiBp.Text = "-1";
                txtLoBp.Text = "-1";
                txtPuLs.Text = "-1";
                txtBrTh.Text = "-1";
                txtBdHt.Text = "-1";

                //'TxtHCnt.Enabled = False
                txtHiBp.Enabled = false;
                txtLoBp.Enabled = false;
                txtPuLs.Enabled = false;
                txtBrTh.Enabled = false;
                txtBdHt.Enabled = false;
            }
            else
            {
                //'TxtHCnt.Enabled = True
                txtHiBp.Enabled = true;
                txtLoBp.Enabled = true;
                txtPuLs.Enabled = true;
                txtBrTh.Enabled = true;
                txtBdHt.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (txtSabun.Text.Trim() == "")
            {
                ComFunc.MsgBox("사번을 입력해 주세요.");
                return;
            }

            if (txtKidn.Text.Trim() == "")
            {
                ComFunc.MsgBox("자격번호를 입력해 주세요.");
                return;
            }

            FstrINFECT = "";

            if (rdoCodeInfect_1.Checked == true)
            {
                FstrINFECT = "0";
            }
            else if (rdoCodeInfect_2.Checked == true)
            {
                FstrINFECT = "1";
            }
            else if (rdoCodeInfect_3.Checked == true)
            {
                FstrINFECT = "2";
            }
            else if (rdoCodeInfect_5.Checked == true)
            {
                FstrINFECT = "3";
            }
            else if (rdoCodeInfect_4.Checked == true)
            {
                FstrINFECT = "9";
            }

            if (FstrKTAS == "")
            {
                ComFunc.MsgBox("KTAS 코드를 선택하시기 바랍니다.");
                return;
            }

            if (FstrINFECT == "")
            {
                ComFunc.MsgBox("감염여부를 선택하시기 바랍니다.");
                return;
            }

            clsErNr.GstrHelpCode = "";
            clsErNr.GstrHelpName = "";

            if (SAVE_DATA() == true)
            {                
                this.Close();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            clsErNr.GstrHelpCode = "";
            clsErNr.GstrHelpName = "";
            this.Close();
        }

        private void btnSearchDiag_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strLV1 = "";
            string strDIAG = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            FstrINFECT = "";
            FstrKTAS = "";
            ssDiag_Sheet1.RowCount = 0;

            if (rdoDiagA.Checked == true)
            {
                strLV1 = "A";
            }
            else if (rdoDiagB.Checked == true)
            {
                strLV1 = "B";
            }

            strDIAG = VB.Trim(txtDiag.Text);

            if (strLV1 == "")
            {
                ComFunc.MsgBox("'1단계-환자나이분류'를 선택하시기 바랍니다.");
                return;
            }

            if (strDIAG == "")
            {
                ComFunc.MsgBox("질병명이 공란입니다.");
                return;
            }

            try
            {
                SQL = " SELECT KTASCD, KTASLVLCD, KTASCD1, KTASCATEGORY FROM ";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "                 T1.*,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND lvl ='1')";
                SQL = SQL + ComNum.VBLF + " ktasAge,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND lvl ='2') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND lvl";
                SQL = SQL + ComNum.VBLF + " ='3') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND";
                SQL = SQL + ComNum.VBLF + " ktascd4 = T1.ktascd4 AND lvl ='4')";
                SQL = SQL + ComNum.VBLF + "                 ktasCategory";
                SQL = SQL + ComNum.VBLF + "         FROM (";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                         ktascd, ktascd1, ktascd2,";
                SQL = SQL + ComNum.VBLF + "                         ktascd3, ktascd4, ktaslvlcd,";
                SQL = SQL + ComNum.VBLF + "                         ktasname";
                SQL = SQL + ComNum.VBLF + "                 From KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + "                WHERE lvl='4'";
                SQL = SQL + ComNum.VBLF + "                AND ktascd1 = '" + strLV1 + "'";
                SQL = SQL + ComNum.VBLF + "                order by ktascd, ktaslvlcd";
                SQL = SQL + ComNum.VBLF + "                 ) T1";
                SQL = SQL + ComNum.VBLF + "         ) T2";
                SQL = SQL + ComNum.VBLF + " WHERE ktasCategory LIKE '%" + strDIAG + "%'";
                SQL = SQL + ComNum.VBLF + " ORDER BY ktascd";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssDiag_Sheet1.RowCount = dt.Rows.Count;
                    ssDiag_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDiag_Sheet1.Cells[i, 0].Text = dt.Rows[i]["KTASCD"].ToString().Trim();
                        ssDiag_Sheet1.Cells[i, 1].Text = dt.Rows[i]["KTASLVLCD"].ToString().Trim();
                        ssDiag_Sheet1.Cells[i, 2].Text = dt.Rows[i]["KTASCD1"].ToString().Trim();
                        ssDiag_Sheet1.Cells[i, 3].Text = " " + dt.Rows[i]["KTASCATEGORY"].ToString().Trim();
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
            }
        }

        private void SCREEN_CLEAR()
        {
            //int i = 0;
            //DataTable dt = null;
            //string SQL = "";
            //string SqlErr = ""; //에러문 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);

            rdoDiagA.Checked = false;
            rdoDiagB.Checked = false;

            btnKtasRepeat.Enabled = false;

            txtDiag.Text = "";

            rdoDiagInfect_1.Checked = false;
            rdoDiagInfect_2.Checked = false;
            rdoDiagInfect_3.Checked = false;
            rdoDiagInfect_4.Checked = false;

            ssDiag_Sheet1.RowCount = 0;

            rdoCodeA.Checked = false;
            rdoCodeB.Checked = false;
            ssCode_1_Sheet1.RowCount = 0;
            ssCode_2_Sheet1.RowCount = 0;
            ssCode_3_Sheet1.RowCount = 0;

            rdoCodeInfect_1.Checked = false;
            rdoCodeInfect_2.Checked = false;
            rdoCodeInfect_3.Checked = false;
            rdoCodeInfect_4.Checked = false;

            txtKtid.Text = FstrKTID;
            txtSabun.Text = "";
            txtKidn.Text = "";
            cboKjob.Items.Clear();
            cboKjob.Items.Add("1.전문의");
            cboKjob.Items.Add("2.전공의");
            cboKjob.Items.Add("3.인턴");
            cboKjob.Items.Add("4.일반의");
            cboKjob.Items.Add("5.간호사");
            cboKjob.Items.Add("6.1급 응급구조사");
            cboKjob.Items.Add("8.기타");
            cboKjob.Items.Add("9.미상");
            cboKjob.SelectedIndex = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            lblpinTime.Text = VB.Left(FstrINDT, 4) + "-" + VB.Mid(FstrINDT, 5, 2) + "-" + VB.Right(FstrINDT, 2) +
                              " " + VB.Left(FstrINTM, 2) + ":" + VB.Right(FstrINTM, 2);

            chkDate.Checked = true;
            //KTAS 분류일시 현재 시간으로 보완 요청
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtKtdt.Text = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            txtKttm.Text = Convert.ToDateTime(clsPublic.GstrSysTime).ToString("HH:mm");
            //txtKtdt.Text = VB.Left(FstrINDT, 4) + "-" + VB.Mid(FstrINDT, 5, 2) + "-" + VB.Right(FstrINDT, 2);
            //txtKttm.Text = VB.Left(FstrINTM, 2) + ":" + VB.Right(FstrINTM, 2);

            chkDate.Enabled = true;

            #region // 2018-09-05 김아린 요청사항으로 디폴트 제거
            //try
            //{
            //    SQL = " SELECT WRITESABUN, PTMIKIDN, PTMIKJOB, PTMIKTDT, PTMIKTTM ";
            //    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_KTAS ";
            //    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + FstrIDNO + "' ";
            //    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + FstrINDT + "' ";
            //    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + FstrINTM + "' ";
            //    SQL = SQL + ComNum.VBLF + "   AND SEQNO = 1";

            //    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        return;
            //    }

            //    if (dt.Rows.Count > 0)
            //    {
            //        txtSabun.Text = dt.Rows[0]["WRITESABUN"].ToString().Trim();
            //        txtKidn.Text = dt.Rows[0]["PTMIKIDN"].ToString().Trim();

            //        for (i = 0; i < cboKjob.Items.Count; i++)
            //        {
            //            cboKjob.SelectedIndex = i;
            //            if (VB.Left(cboKjob.Text, 1) == dt.Rows[0]["PTMIKJOB"].ToString().Trim())
            //            {
            //                break;
            //            }
            //        }
            //    }
            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    ComFunc.MsgBox(ex.Message);
            //}
            #endregion
        }

        private void btnSearchhHelp_Click(object sender, EventArgs e)
        {
            string GstrPANO = FstrIDNO;
            string GstrJDate = FstrJDATE;
            string GstrInDate = VB.Left(FstrINDT, 4) + "-" + VB.Mid(FstrINDT, 5, 2) + "-" + VB.Right(FstrINDT, 2) +
                                " " + VB.Left(FstrINTM, 2) + ":" + VB.Right(FstrINTM, 2);
            string GstrRetName = "KTAS";


            clsPublic.GstrHelpCode = VB.Trim(txtDisease.Text);

            frmROFind frmROFindX = new frmROFind(GstrPANO, GstrJDate, GstrInDate, GstrRetName);
            frmROFindX.rSendDisease += frmROFindX_rSendDisease;
            frmROFindX.StartPosition = FormStartPosition.CenterParent;
            frmROFindX.ShowDialog();
        }

        private void frmROFindX_rSendDisease(string strValue)
        {
            txtDisease.Text = strValue;
        }

        private void btnEmr_vscall_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT EXTRACTVALUE(CHARTXML, '//it7') it7,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it8') it8,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it11') it11,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it12') it12,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it14') it14,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it15') it15,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//it16') it16 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + VB.Trim(FstrIDNO) + "' ";
                //'SQL = SQL + vbCr + " AND MEDFRTIME = '" + Trim(FstrINTM) + "00' "
                SQL = SQL + ComNum.VBLF + " AND writeTime >= '" + VB.Trim(FstrINTM) + "00' ";
                SQL = SQL + ComNum.VBLF + " AND MEDFRDATE = '" + VB.Trim(FstrINDT) + "' ";
                SQL = SQL + ComNum.VBLF + " AND FORMNO = '1969' ORDER BY CHARTDATE, CHARTTIME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtHiBp.Text = dt.Rows[0]["it7"].ToString().Trim();
                    txtLoBp.Text = dt.Rows[0]["it8"].ToString().Trim();
                    txtPuLs.Text = dt.Rows[0]["it11"].ToString().Trim();
                    txtBrTh.Text = dt.Rows[0]["it12"].ToString().Trim();
                    txtBdHt.Text = dt.Rows[0]["it14"].ToString().Trim();
                    cboAORT.Text = dt.Rows[0]["it15"].ToString().Trim();
                    txtVoxs.Text = dt.Rows[0]["it16"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;


                SQL = " SELECT EXTRACTVALUE(CHARTXML, '//ik1') ik1,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//ik2') ik2,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//ik3') ik3,";
                SQL = SQL + ComNum.VBLF + " EXTRACTVALUE(CHARTXML, '//ik4') ik4";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + VB.Trim(FstrIDNO) + "' ";
                SQL = SQL + ComNum.VBLF + " AND MEDFRTIME = '" + VB.Trim(FstrINTM) + "00' ";
                SQL = SQL + ComNum.VBLF + " AND MEDFRDATE = '" + VB.Trim(FstrINDT) + "' ";
                SQL = SQL + ComNum.VBLF + " AND FORMNO = '2506' ORDER BY CHARTDATE, CHARTTIME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ik1"].ToString().Trim() == "true") cboReSp.SelectedIndex = 1;
                    if (dt.Rows[0]["ik2"].ToString().Trim() == "true") cboReSp.SelectedIndex = 2;
                    if (dt.Rows[0]["ik3"].ToString().Trim() == "true") cboReSp.SelectedIndex = 3;
                    if (dt.Rows[0]["ik4"].ToString().Trim() == "true") cboReSp.SelectedIndex = 4;
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnKtasTime_Click(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            txtKtdt.Text = Convert.ToDateTime(clsPublic.GstrSysDate).ToString("yyyy-MM-dd");
            txtKttm.Text = Convert.ToDateTime(clsPublic.GstrSysTime).ToString("HH:mm"); //' FstrINDT = str(1)   FstrINTM = str(2)
        }

        private void btnKtasTmSet_Click(object sender, EventArgs e)
        {
            txtKtdt.Text = VB.Left(FstrINDT, 4) + "-" + VB.Mid(FstrINDT, 5, 2) + "-" + VB.Right(FstrINDT, 2);
            txtKttm.Text = VB.Left(FstrINTM, 2) + ":" + VB.Right(FstrINTM, 2);
        }

        private void rdoCodeA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCodeA.Checked == true)
            {
                LV1("A");
            }
        }

        private void rdoCodeB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCodeB.Checked == true)
            {
                LV1("B");
            }
        }

        private void ssCode_1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;
            LV2(ssCode_1_Sheet1.Cells[e.Row, 0].Text);
        }

        private void ssCode_2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;
            LV3(ssCode_2_Sheet1.Cells[e.Row, 0].Text);
        }

        private void LV1(string arg)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            FstrKTAS = "";

            ssCode_1_Sheet1.RowCount = 0;
            ssCode_2_Sheet1.RowCount = 0;
            ssCode_3_Sheet1.RowCount = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT KTASCD, KTASNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTASCD ";
                SQL = SQL + ComNum.VBLF + " WHERE Length(KTASCD) = 2 ";
                SQL = SQL + ComNum.VBLF + "   AND KTASCD1 = '" + arg + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY KTASCD ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssCode_1_Sheet1.RowCount = dt.Rows.Count;
                    ssCode_1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCode_1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["KTASCD"].ToString().Trim();
                        ssCode_1_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["KTASNAME"].ToString().Trim();

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
            }
        }

        private void LV2(string arg)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            FstrKTAS = "";

            ssCode_2_Sheet1.RowCount = 0;
            ssCode_3_Sheet1.RowCount = 0;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = " SELECT KTASCD, KTASNAME ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTASCD ";
                SQL = SQL + ComNum.VBLF + " WHERE Length(KTASCD) = 3 ";
                SQL = SQL + ComNum.VBLF + "   AND KTASCD LIKE '" + arg + "%' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY KTASCD ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssCode_2_Sheet1.RowCount = dt.Rows.Count;
                    ssCode_2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCode_2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["KTASCD"].ToString().Trim();
                        ssCode_2_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["KTASNAME"].ToString().Trim();

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
            }
        }

        private void LV3(string arg)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            FstrKTAS = "";

            ssCode_3_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT KTASCD, KTASNAME, KTASLVLCD ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTASCD ";
                SQL = SQL + ComNum.VBLF + " WHERE Length(KTASCD) = 5 ";
                SQL = SQL + ComNum.VBLF + "   AND KTASCD LIKE '" + arg + "%' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY KTASCD ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssCode_3_Sheet1.RowCount = dt.Rows.Count;
                    ssCode_3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCode_3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["KTASCD"].ToString().Trim();
                        ssCode_3_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["KTASNAME"].ToString().Trim();
                        ssCode_3_Sheet1.Cells[i, 2].Text = " " + dt.Rows[i]["KTASLVLCD"].ToString().Trim();
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
            }
        }

        private void ssCode_3_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;
            FstrKTAS = ssCode_3_Sheet1.Cells[e.Row, 0].Text;
            FstrKTAS = FstrKTAS + READ_KTAS_LEVEL(FstrKTAS);
        }

        private string READ_KTAS_LEVEL(string arg)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT KTASLVLCD";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_KTASCD";
                SQL = SQL + ComNum.VBLF + " WHERE KTASCD = '" + VB.Left(arg, 5) + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[i]["KTASLVLCD"].ToString().Trim();
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
            }

            return rtnVal;
        }

        private void ssDiag_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;
            FstrKTAS = ssDiag_Sheet1.Cells[e.Row, 0].Text;
            FstrKTAS = FstrKTAS + ssDiag_Sheet1.Cells[e.Row, 1].Text;
        }

        private void ssDiag_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;
            FstrKTAS = ssDiag_Sheet1.Cells[e.Row, 0].Text;
        }

        private void txtHiBp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtLoBp.Focus();
            }

        }

        private void txtLoBp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPuLs.Focus();
            }
        }

        private void txtPuLs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBrTh.Focus();
            }
        }

        private void txtBrTh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBdHt.Focus();
            }
        }

        private void txtBdHt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboAORT.Focus();
            }
        }

        private void txtVoxs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtHCnt.Focus();
            }
        }

        private void txtKtdt_DoubleClick(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "")
            {
                clsPublic.GstrCalDate = ((TextBox)sender).Text;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.StartPosition = FormStartPosition.CenterParent;
            frmCalendarX.ShowDialog();

            ((TextBox)sender).Text = clsPublic.GstrCalDate;
        }

        private void txtDisease_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtHiBp.Focus();
            }
        }

        private void txtHCnt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboReSp.Focus();
            }
        }

        private void cboReSp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtKtdt.Focus();
            }
        }

        private void txtKtdt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtKttm.Focus();
            }
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.KeyCode != Keys.Enter) return;

            try
            {

                SQL = " SELECT MYEN_BUNHO, JIKJONG ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ComFunc.SetAutoZero(VB.Trim(txtSabun.Text), 5) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtKidn.Text = dt.Rows[0]["MYEN_BUNHO"].ToString().Trim();
                    if (dt.Rows[0]["JIKJONG"].ToString().Trim() == "41")
                    {
                        //간호사
                        cboKjob.SelectedIndex = 4;
                    }
                    else if (dt.Rows[0]["JIKJONG"].ToString().Trim() == "86")
                    {
                        //응급구조사
                        cboKjob.SelectedIndex = 5;
                    }
                }
                else
                {
                    txtKidn.Text = "";
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
            }
        }

        private string READ_KTAS_NAME(string arg)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT KTASCD, KTASLVLCD, KTASCD1, KTASCATEGORY FROM";
                SQL = SQL + ComNum.VBLF + "   (";
                SQL = SQL + ComNum.VBLF + "         SELECT";
                SQL = SQL + ComNum.VBLF + "                 T1.*,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND lvl ='1')";
                SQL = SQL + ComNum.VBLF + " ktasAge,";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND lvl ='2') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND lvl";
                SQL = SQL + ComNum.VBLF + " ='3') || ' > ' ||";
                SQL = SQL + ComNum.VBLF + "                 (SELECT ktasname FROM KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + " WHERE ktascd1 = T1.ktascd1 AND ktascd2 =";
                SQL = SQL + ComNum.VBLF + " T1.ktascd2 AND ktascd3 = T1.ktascd3 AND";
                SQL = SQL + ComNum.VBLF + " ktascd4 = T1.ktascd4 AND lvl ='4')";
                SQL = SQL + ComNum.VBLF + "                 ktasCategory";
                SQL = SQL + ComNum.VBLF + "         FROM (";
                SQL = SQL + ComNum.VBLF + "                 SELECT";
                SQL = SQL + ComNum.VBLF + "                         ktascd, ktascd1, ktascd2,";
                SQL = SQL + ComNum.VBLF + "                         ktascd3, ktascd4, ktaslvlcd,";
                SQL = SQL + ComNum.VBLF + "                         ktasname";
                SQL = SQL + ComNum.VBLF + "                 From KOSMOS_PMPA.NUR_ER_ktascd";
                SQL = SQL + ComNum.VBLF + "                WHERE lvl='4'";
                SQL = SQL + ComNum.VBLF + "                AND ktascd1 = '" + VB.Left(arg, 1) + "'";
                SQL = SQL + ComNum.VBLF + "                order by ktascd, ktaslvlcd";
                SQL = SQL + ComNum.VBLF + "                 ) T1";
                SQL = SQL + ComNum.VBLF + "         ) T2";
                SQL = SQL + ComNum.VBLF + " WHERE KTASCD = '" + VB.Left(arg, 5) + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY ktascd";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[i]["KTASCATEGORY"].ToString().Trim();
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
            }

            return rtnVal;
        }

        private string READ_MAX_KTAS(string argIDNO, string argINDT, string argINTM)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            argINDT = VB.Replace(argINDT, "-", "");
            argINTM = VB.Replace(argINTM, ":", "");

            if (argINDT == "" || argINTM == "") return rtnVal;

            try
            {
                SQL = " SELECT MIN(PTMIKTS) KTASLEVL";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + argIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + argINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + argINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO = 1";
                SQL = SQL + ComNum.VBLF + "  UNION ALL";
                SQL = SQL + ComNum.VBLF + "      SELECT PTMIKTS";
                SQL = SQL + ComNum.VBLF + "        FROM KOSMOS_PMPA.NUR_ER_KTAS";
                SQL = SQL + ComNum.VBLF + "       WHERE PTMIIDNO = '" + argIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINDT = '" + argINDT + "' ";
                SQL = SQL + ComNum.VBLF + "         AND PTMIINTM = '" + argINTM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SEQNO > 1)";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[i]["KTASLEVL"].ToString().Trim();
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
            }

            return rtnVal;
        }

        private bool SaveXML(PsmhDb pDbCon, string strPTMIIDNO, string strPTMIINDT, string strPTMIKPR, string strPTMIPTS, string strPTMIKTDT, string strPTMIKTTM, string strKTAS)
        {
            bool rtVal = false;
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //int result = 0;
            string strDrCd = "";


            try
            {
                SQL = " SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + strPTMIINDT + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPTMIIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = 'ER' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();

                }

                if (strDrCd == "")
                {
                    strDrCd = "7403";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtVal;
            }


            EmrPatient pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPTMIIDNO, "O", strPTMIINDT, "ER");
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2501");
            Dictionary<string, string> strDataNew = new Dictionary<string, string>();

            if (pForm.FmOLDGB == 1)
            {
                #region XML 문장 생성  quote = Chr(34)

                double dblEmrNo = 0;
                string strHead = "";
                string strChartX1 = "";
                string strChartX2 = "";
                string strXML = "";
                string strXMLCert = "";
                string strTagHead = "";
                string strTagTail = "";
                string strTagVal = "";

                char quote = (char)34;
                strXML = "";

                strHead = "<?xml version=" + quote + "1.0" + quote + " encoding=" + quote + "UTF-8" + quote + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";


                strXML = strHead + strChartX1;

                strTagHead = "<it1 type=" + quote + "inputText" + quote + " label=" + quote + "분류과정" + quote + "><![CDATA[";
                strTagVal = strPTMIKPR;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it2 type=" + quote + "inputText" + quote + " label=" + quote + "분류결과" + quote + "><![CDATA[";
                strTagVal = strPTMIPTS;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it3 type=" + quote + "inputText" + quote + " label=" + quote + "분류내용" + quote + "><![CDATA[";
                strTagVal = READ_KTAS_NAME(strKTAS);
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it4 type=" + quote + "inputText" + quote + " label=" + quote + "분류자직종" + quote + "><![CDATA[";
                strTagVal = VB.Mid(cboKjob.Text, 3, VB.Len(cboKjob.Text));
                strTagTail = "]]></it4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it5 type=" + quote + "inputText" + quote + " label=" + quote + "분류자번호" + quote + "><![CDATA[";
                strTagVal = VB.Trim(txtKidn.Text);
                strTagTail = "]]></it5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strXML = strXML + strChartX2;

                strXMLCert = strXML;
                #endregion

                #region // EMR 시퀀스 생성
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KOSMOS_EMR.GetEmrXmlNo() FunSeqNo FROM Dual";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                #endregion

                // EMR 생성                
                if (clsNurse.CREATE_EMR_XMLINSRT3(dblEmrNo, "2501", txtSabun.Text.Trim(),
                    strPTMIKTDT, strPTMIKTTM + "00", 0, strPTMIIDNO, "O", strPTMIINDT, strPTMIKTTM + "00",
                    "", "", "ER", strDrCd, "0", 1, strXML) == false)
                {
                    return rtVal;
                }
            }
            else
            {
                strDataNew.Clear();
                strDataNew.Add("I0000031179", strPTMIKPR);
                strDataNew.Add("I0000037706", strPTMIPTS);
                strDataNew.Add("I0000037707", READ_KTAS_NAME(strKTAS));
                strDataNew.Add("I0000037708", VB.Mid(cboKjob.Text, 3, VB.Len(cboKjob.Text)));
                strDataNew.Add("I0000026295", txtKidn.Text.Trim());
                if (clsEmrQuery.SaveNurChartFlowEx(clsDB.DbCon, txtSabun.Text.Trim(), this, pAcp, pForm,  strPTMIINDT, strPTMIIDNO, strDrCd, strPTMIKTDT, strPTMIKTTM + "00", strDataNew, false) == 0)
                {
                    ComFunc.MsgBoxEx(this, "중증도 분류내역 저장도중 오류가 발생했습니다.");
                    return rtVal;
                }
            }
         

            SQL = " UPDATE KOSMOS_PMPA.OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + " SET TEXTEMR = '1' ";
            SQL = SQL + ComNum.VBLF + " WHERE BDATE = TO_DATE ('" + strPTMIINDT + "', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "      AND PANO = '" + strPTMIIDNO + "' ";
            SQL = SQL + ComNum.VBLF + "      AND DEPTCODE IN ('ER', 'EM') ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                return rtVal;
            }

            rtVal = true;
            return rtVal;
        }

        private void txtKttm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.Focus();
            }
        }

        private void btnKtasRepeat_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (txtSabun.Text.Trim() == "")
            {
                ComFunc.MsgBox("사번을 입력해 주세요.");
                return;
            }

            if (txtKidn.Text.Trim() == "")
            {
                ComFunc.MsgBox("자격번호를 입력해 주세요.");
                return;
            }

            if (ComFunc.MsgBoxQ("최초분류 : " + READ_KTAS_LEVEL(FstrReKTAS) + " " + READ_KTAS_NAME(FstrReKTAS) + ComNum.VBLF + "재분류 전송하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (btnKtasRepeatClick() == true)
            {
                this.Close();
            }
        }

        private bool btnKtasRepeatClick()
        {
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strPTMIIDNO = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";
            string strPTMIKTID = "";
            string strPTMIKPR = "";
            string strPTMIKTS = "";
            string strPTMIKTDT = "";
            string strPTMIKTTM = "";
            string strPTMIKJOB = "";
            string strPTMIKIDN = "";
            //string strWRITEDATE = "";
            string strWRITESABUN = "";

            string strKTAS = "";
            string strFIRST = "";
            //string strKTASLEVL = "";

            

            try
            {
                strFIRST = "";
                strPTMIKTID = txtKtid.Text.Trim();
                strPTMIIDNO = FstrIDNO;
                strPTMIINDT = FstrINDT;
                strPTMIINTM = FstrINTM;

                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + " * ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_ER_EMIHPTMI A ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI B ";
                SQL = SQL + ComNum.VBLF + "               WHERE B.PTMIIDNO  = A.PTMIIDNO ";
                SQL = SQL + ComNum.VBLF + "                    AND B.PTMIINDT  = A.PTMIINDT";
                SQL = SQL + ComNum.VBLF + "                    AND B.PTMIINTM = A.PTMIINTM ) ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIIDNO = '" + strPTMIIDNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINDT = '" + strPTMIINDT + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTMIINTM = '" + strPTMIINTM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(A.PTMIOTDT) IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIEMCD= 'C24C0083' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("퇴실일자를 네디스서버에 전송후에는 더이상 KTAS를 추가 입력할수 없습니다. ");
                    dt.Dispose();
                    dt = null;
                    return rtVal;
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
            }


            try
            {
                if (strPTMIKTID == "")
                {
                    SQL = " SELECT * FROM KOSMOS_PMPA.NUR_ER_KTAS ";
                    SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + strPTMIIDNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + strPTMIINDT + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + strPTMIINTM + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return rtVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ComFunc.MsgBox("환자 정보가 갱신되었습니다. " + ComNum.VBLF +
                                       "환자를 다시 선택하여 입력하시기 바랍니다." + ComNum.VBLF +
                                       "(ex. 다른 PC에서 이미 분류를 하였는 경우)");
                        dt.Dispose();
                        dt = null;
                        return rtVal;
                    }
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            if (strPTMIIDNO == "")
            {
                if (ComFunc.MsgBoxQ("환자 정보가 존재하지 않습니다. 아래와 같이 대체됩니다." + ComNum.VBLF +
                                    "★ 환자등록번호 <= 선별번호" + ComNum.VBLF +
                                    "★ 내원일시 <= 분류일시" + ComNum.VBLF +
                                    "계속 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return rtVal;
                }
            }

            if (chkDate.Checked == true)
            {
                strPTMIKTDT = VB.Replace(txtKtdt.Text, "-", "");
                strPTMIKTTM = VB.Replace(txtKttm.Text, ":", "");
            }
            else
            {
                strPTMIKTDT = VB.Replace(clsPublic.GstrSysDate, "-", "");
                strPTMIKTTM = VB.Replace(clsPublic.GstrSysTime, ":", "");
            }

            if (VB.Len(strPTMIKTDT) != 8)
            {
                ComFunc.MsgBox("날짜형식이 맞지 않습니다. 다시 확인해 주세요.");
                return rtVal;
            }
            if (VB.Len(strPTMIKTTM) != 4)
            {
                ComFunc.MsgBox("시간형식이 맞지 않습니다. 다시 확인해 주세요.");
                return rtVal;
            }

            if (strPTMIIDNO == "")
            {
                strPTMIIDNO = strPTMIKTID;
                strPTMIINDT = strPTMIKTDT;
                strPTMIINTM = strPTMIKTTM;
            }

            strWRITESABUN = VB.Trim(txtSabun.Text);
            strPTMIKIDN = VB.Trim(txtKidn.Text);
            strPTMIKJOB = VB.Left(cboKjob.Text, 1);
            strPTMIKPR = FstrReKTAS;
            strPTMIKTS = READ_KTAS_LEVEL(FstrReKTAS);

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_PMPA.NUR_ER_KTAS( ";
                SQL = SQL + ComNum.VBLF + "  PTMIIDNO, PTMIINDT, PTMIINTM, PTMIKTID,";
                SQL = SQL + ComNum.VBLF + "  PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + "  PTMIKJOB, PTMIKIDN, WRITEDATE, WRITESABUN, REALSABUN, ";
                SQL = SQL + ComNum.VBLF + "  SEQNO ";
                SQL = SQL + ComNum.VBLF + "  ) VALUES (";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIIDNO + "','" + strPTMIINDT + "','" + strPTMIINTM + "','" + strPTMIKTID + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIKPR + "','" + strPTMIKTS + "','" + strPTMIKTDT + "','" + strPTMIKTTM + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strPTMIKJOB + "','" + strPTMIKIDN + "', SYSDATE, " + strWRITESABUN + "," + clsType.User.Sabun + ",";
                SQL = SQL + ComNum.VBLF + (strFIRST == "OK" ? "1" : "2") + ")";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsErNr.GstrHelpCode = strPTMIKTID;
                strKTAS = VB.Left(strPTMIKPR, 5) + VB.Trim(strPTMIKTS) + VB.Right(strPTMIKPR, 1);

                if (SaveXML(clsDB.DbCon, strPTMIIDNO, strPTMIINDT, strPTMIKPR, strPTMIKTS, strPTMIKTDT, strPTMIKTTM, strKTAS) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    //clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }        
    }
}
