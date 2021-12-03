using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmCertPoolMstMain : Form, MainFormMessage
    {
        /// <summary>
        /// 일괄검증
        /// </summary>
        Form AllVerify = null;

        /// <summary>
        /// 전체인증서 점검
        /// </summary>
        Form AutoVerify = null;

        #region //MainFormMessage
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

        public frmCertPoolMstMain(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        public frmCertPoolMstMain(MainFormMessage pform, string strPara)
        {
            mCallForm = pform;
            InitializeComponent();
        }

        public frmCertPoolMstMain()
        {
            InitializeComponent();
        }

        private void frmCertPoolMstMain_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            FrmClear();
            ComFunc.ReadSysDate(clsDB.DbCon);
            ssView.ActiveSheet.RowCount = 0;

            DtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7);
            DtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            TxtSabun.Focus();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchListSabun();
        }

        private void ssListSabun_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            TxtSabun.Text = ssListSabun.ActiveSheet.Cells[e.Row, 0].Text;
            SearchEmp();
        }

        private void FrmClear()
        {
            LblCert.Text = "";
            LblCert.BackColor = Color.White;

            TxtSabun.Text = "";
            TxtPass.Text = "";

            ChkPass.Checked = true;

            TxtKorName.Text = "";
            TxtJumin1.Text = "";
            TxtJumin2.Text = "";

            TxtKunday.Text = "";
            TxtIpsaday.Text = "";
            TxtBalday.Text = "";
            TxtCertDate.Text = "";
            TxtNameSearch.Text = "";

            picPHOTO.Image = null;
        }

        private void SearchListSabun()
        {            
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (string.IsNullOrEmpty(TxtNameSearch.Text.Trim()))
            {
                ComFunc.MsgBox("이름을 넣고 검색하세요!!");
                TxtNameSearch.Focus();
                return;
            }

            ssListSabun.ActiveSheet.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT M.Sabun, M.KorName, M.Buse, B.Name ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_MST M, ADMIN.BAS_BUSE  B ";
                SQL = SQL + ComNum.VBLF + " WHERE M.Buse = Bucode ";

                if (RdoJaeGu_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.ToiDay is null ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.ToiDay is not null ";
                }

                //'직원,영일,알바 구분
                if (RdoJikwon_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun < '600001' ";
                }
                else if (RdoJikwon_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun > '800000' ";
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun < '899999' ";
                    SQL = SQL + ComNum.VBLF + " AND   Substr(M.Buse, 1, 4) <> '0992' ";     //'용역직원 제외
                }
                else if (RdoJikwon_2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun > '900000' ";
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun < '999999' ";
                }
                else if (RdoJikwon_3.Checked == true) //'파견
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun > '600000' ";
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun < '699999' ";
                }
                else if (RdoJikwon_4.Checked == true) //'성모어린이집
                {
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun > '700000' ";
                    SQL = SQL + ComNum.VBLF + " AND   M.Sabun < '799999' ";
                }

                if (string.IsNullOrEmpty(TxtNameSearch.Text.Trim()) == false)
                {
                    SQL = SQL + ComNum.VBLF + " AND KORNAME LIKE '%" + TxtNameSearch.Text.Trim() + "%' ";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY  M.Sabun ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssListSabun.ActiveSheet.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssListSabun.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssListSabun.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ssListSabun.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
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

        private void TxtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchEmp();
            }
        }

        private void TxtSabun_Leave(object sender, EventArgs e)
        {

        }

        private void SearchEmp()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strSabun = string.Empty;

            if (string.IsNullOrEmpty(TxtSabun.Text.Trim()))
            {
                return;
            }

            if (RdoJikwon_0.Checked == true && VB.Val(TxtSabun.Text) > 60000)
            {
                ComFunc.MsgBox("직원의 사번은 1-60000까지만 가능합니다.");
                return;
            }
            if (RdoJikwon_1.Checked == true && VB.Val(TxtSabun.Text) < 800001)
            {
                ComFunc.MsgBox("영일의 사번은 800001부터만 가능합니다.");
                return;
            }

            if (string.IsNullOrEmpty(TxtSabun.Text) == false)
            {
                TxtSabun.Text = ComFunc.SetAutoZero(TxtSabun.Text, 5);
                strSabun = TxtSabun.Text;

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SABUN,BUSE,JUMIN3,KORNAME,TO_CHAR(TOIDAY,'YYYY-MM-DD') TOIDAY,CERTPASS, ";
                    SQL = SQL + ComNum.VBLF + "       TO_CHAR(KUNDAY,'YYYY-MM-DD') KUNDAY, TO_CHAR(IPSADAY,'YYYY-MM-DD') IPSADAY, ";
                    SQL = SQL + ComNum.VBLF + "       TO_CHAR(BALDAY,'YYYY-MM-DD') BALDAY  ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + strSabun + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND   SUBSTR(BUSE, 1, 4) <> '0992' ";     //'용역직원 제외

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        TxtKorName.Text = dt.Rows[0]["KORNAME"].ToString().Trim();
                        TxtJumin1.Text = VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), 6);
                        TxtJumin2.Text = VB.Mid(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), 7, 7);
                        TxtPass.Text = clsAES.DeAES(dt.Rows[0]["CERTPASS"].ToString().Trim());
                        TxtKunday.Text = dt.Rows[0]["KUNDAY"].ToString().Trim();
                        TxtIpsaday.Text = dt.Rows[0]["IPSADAY"].ToString().Trim();
                        TxtBalday.Text = dt.Rows[0]["BALDAY"].ToString().Trim();

                        ImageView(strSabun);

                        SQL = "SELECT TO_CHAR(CERDATE,'YYYY-MM-DD') CERTDATE FROM ADMIN.INSA_MSTS WHERE SABUN = '" + strSabun + "' ORDER BY CERDATE DESC ";
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            TxtCertDate.Text = dt1.Rows[0]["CERTDATE"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[0]["TOIDAY"].ToString().Trim() != "")
                        {
                            ComFunc.MsgBox("퇴사한 사번입니다..!! 퇴직일 [" + dt.Rows[0]["TOIDAY"].ToString().Trim() + "]");
                        }
                        else
                        {
                            BtnCertTest.PerformClick();
                        }
                    }
                    else
                    {
                        FrmClear();
                        TxtKorName.Focus();
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
        }

        private void BtnCertTest_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;

            string strJumin = string.Empty;
            string strPass = string.Empty;

            strJumin = TxtJumin1.Text.Trim() + TxtJumin2.Text.Trim();

            if (strJumin.Length != 13 || string.IsNullOrEmpty(strJumin) == true)
            {
                ComFunc.MsgBox("주민번호 에러!!");
                return;
            }

            strPass = TxtPass.Text.Trim();

            if (ChkPass.Checked == false)
            {
                strPass = strJumin;
            }

            if (string.IsNullOrEmpty(strPass))
            {
                ComFunc.MsgBox("공인인증 패스워드 에러!");
                TxtPass.Focus();
                return;
            }

            //1.API 초기화 : API_INIT
            if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
            {
                return;
            }

            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            if (clsCertWork.ROAMING_NOVIEW_FORM(strJumin, strPass) == false)
            {
                LblCert.Text = "인증실패";
                LblCert.BackColor = Color.Red;                
            }
            else
            {
                LblCert.Text = "인증성공";
                LblCert.BackColor = Color.Yellow;
            }

            clsCertWork.API_RELEASE();
            return;
        }

        private void BtnCert_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strJumin = string.Empty;
            string strPass = string.Empty;
            string strSabun = string.Empty;

            strSabun = ComFunc.SetAutoZero(TxtSabun.Text, 5);

            strJumin = TxtJumin1.Text.Trim() + TxtJumin2.Text.Trim();
            if (string.IsNullOrEmpty(strJumin) || strJumin.Length != 13)
            {
                ComFunc.MsgBox("주민번호 에러");
                return;
            }

            strPass = TxtPass.Text.Trim();
            if (string.IsNullOrEmpty(strPass))
            {
                ComFunc.MsgBox("공인인증 패스워드 에러!");
                TxtPass.Focus();
                return;
            }

            //1.API 초기화 : API_INIT
            if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
            {
                return;
            }

            // 인증서 등록
            clsCertWork.REGISTERCERTTERM(strJumin);

            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            if (clsCertWork.ROAMING_NOVIEW_FORM(strJumin, strPass) == true)
            {

                ComFunc.ReadSysDate(clsDB.DbCon);
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {

                    SQL = " INSERT INTO ADMIN.INSA_MSTS(SABUN, UDATE, CERDATE) VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + TxtSabun.Text + "', TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI'),";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI') )";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("인증서 내역 등록 중 에러 발생 - 전산정보팀 연락 요망", "확인");
                        clsCertWork.API_RELEASE();
                        return;
                    }

                    SQL = " UPDATE ADMIN.INSA_MST SET ";
                    SQL = SQL + ComNum.VBLF + "  CertPass = '" + clsAES.AES(strPass) + "' ";
                    SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + strSabun + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("인증서 내역 등록 중 에러 발생 - 전산정보팀 연락 요망", "확인");
                        clsCertWork.API_RELEASE();
                        return;
                    }

                    ComFunc.MsgBox("인증서 등록 내역 업데이트 완료", "확인");
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                catch (Exception ex)
                {                    
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("인증서 내역 등록 중 에러 발생 - 전산정보팀 연락 요망", "확인");
                    clsCertWork.API_RELEASE();
                    return;
                }
            }
            else
            {
                ComFunc.MsgBox("인증서 검증 실패 - 재등록 하세요", "확인");
            }

            clsCertWork.API_RELEASE();
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;


            int i = 0;
            DataTable dt = null;            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView.ActiveSheet.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.SABUN, TO_CHAR(A.UDATE, 'YYYY-MM-DD') UDATE, A.CERTIOK,b.CertPass, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.CERDATE, 'YYYY-MM-DD') CERDATE, B.BUSE, B.KORNAME, A.ROWID, A.USE, B.JUMIN3 BJUMIN3 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.INSA_MSTS A, ADMIN.INSA_MST B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SABUN = B.SABUN ";
                SQL = SQL + ComNum.VBLF + "   AND A.UDATE >= TO_DATE('" + DtpSDate.Value.ToShortDateString() + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND trunc(A.UDATE) <= TO_DATE('" + DtpEDate.Value.ToShortDateString() + "','YYYY-MM-DD')  ";                
                if (TxtName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.KorName LIKE '%" + TxtName.Text.Trim() + "%' ";
                }
                SQL = SQL + ComNum.VBLF + "   ORDER BY BUSE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView.ActiveSheet.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView.ActiveSheet.Cells[i, 0].Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, dt.Rows[i]["BUSE"].ToString().Trim());
                        ssView.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["BJUMIN3"].ToString().Trim());
                        ssView.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["UDATE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["CERDATE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["USE"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["CertPass"].ToString().Trim();
                        if (dt.Rows[i]["CERTIOK"].ToString().Trim() == "1")
                        {
                            ssView.ActiveSheet.Cells[i, 6].Text = "성공";
                        }
                        else
                        {
                            ssView.ActiveSheet.Cells[i, 6].Text = "실패";
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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return;

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            //string strOK = "OK";
            string strJUMIN = string.Empty;
            string strSABUN = string.Empty;
            string strUPDATE = string.Empty;
            string strCERDATE = string.Empty;
            string strCHECK = string.Empty;
            string strCERTPASS = string.Empty;

            //1.API 초기화 : API_INIT
            if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView.ActiveSheet.RowCount; i++)
                {
                    strJUMIN = ssView.ActiveSheet.Cells[i, 3].Text.Trim();
                    strSABUN = ssView.ActiveSheet.Cells[i, 1].Text.Trim();
                    strUPDATE = ssView.ActiveSheet.Cells[i, 4].Text.Trim();
                    strCERDATE = ssView.ActiveSheet.Cells[i, 5].Text.Trim();
                    strCHECK = ssView.ActiveSheet.Cells[i, 7].Text.Trim();
                    strCERTPASS = clsAES.DeAES(ssView.ActiveSheet.Cells[i, 10].Text.Trim());

                    if (string.IsNullOrEmpty(strJUMIN) || strJUMIN.Length != 13)
                    {
                        ComFunc.MsgBox("주민번호 에러");
                        return;
                    }

                    if (clsCertWork.ROAMING_NOVIEW_FORM(strJUMIN, strCERTPASS) == true)
                    {
                        SQL = " UPDATE ADMIN.INSA_MSTS SET ";
                        SQL = SQL + ComNum.VBLF + " CERDATE = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + " CERTIOK = '1' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + strSABUN + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRUNC(UDATE) = TO_DATE('" + strUPDATE + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("처리 중 에러 발생", "확인");
                            clsCertWork.API_RELEASE();
                            return;
                        }

                        ssView.ActiveSheet.Cells[i, 6].Text = "성공";
                    }
                    else
                    {
                        SQL = " UPDATE ADMIN.INSA_MSTS SET ";
                        SQL = SQL + ComNum.VBLF + " CERDATE = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI'), ";
                        SQL = SQL + ComNum.VBLF + " CERTIOK = '0' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SABUN = '" + strSABUN + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND TRUNC(UDATE) = TO_DATE('" + strUPDATE + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("처리 중 에러 발생", "확인");
                            clsCertWork.API_RELEASE();
                            return;
                        }

                        ssView.ActiveSheet.Cells[i, 6].Text = "실패";
                    }                    
                }

                ComFunc.MsgBox("정상적으로 처리되었습니다.", "확인");
                clsDB.setCommitTran(clsDB.DbCon);
                clsCertWork.API_RELEASE();
                BtnView.PerformClick();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox("처리 중 에러 발생", "확인");
                clsCertWork.API_RELEASE();
                return;
            }
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            TxtPass.Text = "psmh" + TxtJumin1.Text.Trim() + "!";
        }

        private void TxtNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch.PerformClick();
            }
        }

        private void ImageView(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT IMAGE FROM ADMIN.INSA_IMAGE ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + ComFunc.SetAutoZero(strSABUN, 6) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    byte[] b = (byte[])(dt.Rows[0]["IMAGE"]);
                    picPHOTO.SizeMode = PictureBoxSizeMode.StretchImage;
                    picPHOTO.Image = ConvertByteToImage(b);                    
                }
                else
                {
                    picPHOTO.Image = null;
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

        public Image ConvertByteToImage(byte[] imgByte)
        {
            ImageConverter imageConverter = new ImageConverter();
            Image img = (Image)imageConverter.ConvertFrom(imgByte);
            return img;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnView.PerformClick();
            }
        }

        private void btnCertPoolVerify_Click(object sender, EventArgs e)
        {
            if (AllVerify != null)
            {
                AllVerify.Dispose();
                AllVerify = null;
            }

            AllVerify = new frmCertPoolVerify();
            AllVerify.StartPosition = FormStartPosition.CenterScreen;
            AllVerify.Show();
        }

        private void btnCertPoolAutoVerify_Click(object sender, EventArgs e)
        {
            if (AutoVerify != null)
            {
                AutoVerify.Dispose();
                AutoVerify = null;
            }

            AutoVerify = new frmCertPoolAutoVerify();
            AutoVerify.StartPosition = FormStartPosition.CenterScreen;
            AutoVerify.Show();
        }

        private void frmCertPoolMstMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AllVerify  != null )
            {
                AllVerify.Dispose();
                AllVerify = null;
            }

            if (AutoVerify != null)
            {
                AutoVerify.Dispose();
                AutoVerify = null;
            }

            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmCertPoolMstMain_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }
    }
}
