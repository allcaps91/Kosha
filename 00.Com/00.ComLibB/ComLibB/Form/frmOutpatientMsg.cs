using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmOutpatientMsg
    /// File Name : frmOutpatientMsg.cs
    /// Title or Description : 외래 안부문자 예약
    /// Author : 박창욱
    /// Create Date : 2017-06-15
    /// Update Histroy :
    /// </summary>
    /// <history>  
    /// VB\Frm외래안부문자예약.frm(Frm외래안부문자예약) -> frmOutpatientMsg.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\Frm외래안부문자예약.frm(Frm외래안부문자예약)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\Ocs\OpdOcs\Oorder\mtsoorder.vbp
    /// </vbp>
    public partial class frmOutpatientMsg : Form
    {
        string FstrPano = "";
        string FstrDeptCode = "";

        public frmOutpatientMsg()
        {
            InitializeComponent();
        }

        public frmOutpatientMsg(string sPano, string sDeptCode)
        {
            InitializeComponent();
            FstrPano = sPano;
            FstrDeptCode = sDeptCode;
        }

        private void screenClear()
        {
            txtSendTime.Text = "";

            btnMonth01.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth02.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth03.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth04.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth05.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth06.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth07.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth08.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth09.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth10.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth11.ForeColor = Color.FromArgb(0, 0, 0);
            btnMonth12.ForeColor = Color.FromArgb(0, 0, 0);
        }

        private void getSearch()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int nRead = 0;
            string strRegdate = "";
            string strYYYY = "";
            string strMM = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Control[] conVal = null;

            screenClear();

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(REGDATE, 'YYYY-MM-DD') BREGDATE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_CANCER_SERVICE";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                SQL = SQL + ComNum.VBLF + "  AND DEPT_CODE = '" + FstrDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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
                nRead = dt.Rows.Count;
                if (nRead > 0)
                {
                    txtSendTime.Text = dt.Rows[0]["BREGDATE"].ToString().Trim();
                    strRegdate = dt.Rows[0]["BREGDATE"].ToString().Trim();
                    strYYYY = VB.Left(strRegdate, 4);
                    strMM = VB.Mid(strRegdate, 6, 2);

                    if (cboYYYY.Text == strYYYY)
                    {
                        conVal = ComFunc.GetAllControls(grb2);

                        if (conVal != null)
                        {
                            if (conVal.Length > 0)
                            {
                                foreach (Control conItem in conVal)
                                {
                                    if (conItem is Button)
                                    {
                                        if ((((Button)conItem).Name).Replace("btnMonth", "") == strMM)
                                        {
                                            ((Button)conItem).BackColor = Color.FromArgb(255, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboYYYY_Click(object sender, EventArgs e)
        {
            screenClear();
            getSearch();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {

                SQL = "";
                SQL = "UPDATE KOSMOS_PMPA.ETC_CANCER_SERVICE SET";
                SQL = SQL + ComNum.VBLF + " DELDATE = TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                SQL = SQL + ComNum.VBLF + "  AND DEPT_CODE = '" + FstrDeptCode + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("예약취소 완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            screenClear();
            getSearch();
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            string strSendDate = "";
            string strMsg = "";
            string strMonth = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strMonth = VB.Right(((Button)sender).Name, 2);
            strSendDate = cboYYYY.Text + "-" + strMonth + "-01";

            try
            {
                //환자마스터에서 SMS동의여부 확인
                SQL = "";
                SQL = "SELECT PANO, SNAME, HPHONE, GBSMS";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                if (dt.Rows[0]["GBSMS"].ToString().Trim() != "Y")
                {
                    strMsg = "[SMS동의안함]";
                    strMsg = strMsg + ComNum.VBLF + "등록번호 : " + dt.Rows[0]["PANO"].ToString().Trim();
                    strMsg = strMsg + ComNum.VBLF + "성명 : " + dt.Rows[0]["SNAME"].ToString().Trim();
                    strMsg = strMsg + ComNum.VBLF + "전화번호 : " + dt.Rows[0]["HPHONE"].ToString().Trim();
                    ComFunc.MsgBox(strMsg, "확인사항");
                }
                dt.Dispose();
                dt = null;

                //SMS_ETC에서 문자전송이 되었고, 안부문자에서 삭제가 안된 환자
                if (saveSend() == true)
                {
                    //SMS_ETC에서 전송안된 목록
                    saveFail();
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //SMS_ETC에서 문자전송이 되었고, 안부문자에서 삭제가 안된 환자
        private bool saveSend()
        {
            int nREAD = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "SELECT A.JOBDATE, A.PANO, A.DEPTCODE, A.SENDTIME";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SMS A, KOSMOS_PMPA.ETC_CANCER_SERVICE B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN = '36'";
                SQL = SQL + ComNum.VBLF + "  AND A.SENDTIME IS  NOT NULL";
                SQL = SQL + ComNum.VBLF + "  AND B.DELDATE IS NULL";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_PMPA.ETC_CANCER_SERVICE SET";
                    SQL = SQL + ComNum.VBLF + " DELDATE = TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "  AND DEPT_CODE = '" + FstrDeptCode + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                
                getSearch();

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
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

        //SMS+ETC에서 전송되지 않은 목록
        private bool saveFail()
        {
            string strSendDate = "";
            int nREAD = 0;
            string strSendGubun = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = "SELECT A.JOBDATE, A.PANO, A.DEPTCODE, A.SENDTIME";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SMS A, KOSMOS_PMPA.ETC_CANCER_SERVICE B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "  AND A.GUBUN = '36'";
                SQL = SQL + ComNum.VBLF + "  AND A.SENDTIME IS NULL";
                SQL = SQL + ComNum.VBLF + "";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }
                nREAD = dt.Rows.Count;
                dt.Dispose();
                dt = null;

                strSendGubun = txtSendTime.Text.Trim();
                if (strSendGubun == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_PMPA.ETC_CANCER_SERVICE(";
                    SQL = SQL + ComNum.VBLF + "PANO, REGDATE, DEPT_CODE) VALUES(";
                    SQL = SQL + ComNum.VBLF + "'" + FstrPano + "', TO_DATE('" + strSendDate + "', 'YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "'" + FstrDeptCode + "')";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_PMPA.ETC_CANCER_SERVICE SET";
                    SQL = SQL + ComNum.VBLF + " REGDATE = TO_DATE('" + strSendDate + "', 'YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                    SQL = SQL + ComNum.VBLF + "  AND DEPT_CODE = '" + FstrDeptCode + "'";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                getSearch();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int nRead = 0;
            string strRegdate = "";
            string strYYYY = "";
            string strMM = "";
            Control[] conVal = null;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            screenClear();

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(REGDATE, 'YYYY-MM-DD') BREGDATE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_CANCER_SERVICE";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + FstrPano + "'";
                SQL = SQL + ComNum.VBLF + "  AND DEPT_CODE = '" + FstrDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                nRead = dt.Rows.Count;

                if (nRead > 0)
                {
                    txtSendTime.Text = dt.Rows[0]["BREGDATE"].ToString().Trim();
                    strRegdate = dt.Rows[0]["BREGDATE"].ToString().Trim();
                    strYYYY = VB.Left(strRegdate, 4);
                    strMM = VB.Mid(strRegdate, 6, 2);

                    if (cboYYYY.Text == strYYYY)
                    {
                        conVal = ComFunc.GetAllControls(grb2);

                        if (conVal != null)
                        {
                            if (conVal.Length > 0)
                            {
                                foreach (Control conItem in conVal)
                                {
                                    if (conItem is Button)
                                    {
                                        if ((((Button)conItem).Name).Replace("btnMonth", "") == strMM)
                                        {
                                            ((Button)conItem).BackColor = Color.FromArgb(255, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmOutpatientMsg_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            for (i = 0; i < 3; i++)
            {
                cboYYYY.Items.Add(VB.Mid(clsPublic.GstrSysDate, 1, 4) + i);
            }
            getSearch();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
