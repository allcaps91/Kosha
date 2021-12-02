using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocu_01 : Form, MainFormMessage
    {
        #region //MainFormMessage
        public string mPara1 = "";
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
            mPara1 = strPara;
        }
        #endregion //MainFormMessage

        string cYear = "";
        string cSeqNo = "";
        string cGubun = "";
        string cBuse = "";
        string cDocuNo = "";
        string cDocuName = "";
        string cPlaceName = "";
        string cWorkday = "";
        string cPAGE = "";
        string strBuseGbn = "";

        public frmDocu_01()
        {
            InitializeComponent();
        }

        public frmDocu_01(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocu_01(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmDocu_01_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);

            Combo_Add();
            Data_Clear();
            TxtYear.Text = clsPublic.GstrSysDate.Substring(0, 4);
            TxtWorkDay.Text = clsPublic.GstrSysDate;
            BtnDelete.Enabled = false;
            OptGB0.Checked = true;

            TxtOutMan.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun.PadLeft(5, '0'));

            ss1.ActiveSheet.RowCount = 0;

            if (clsType.User.Sabun.Equals("04349"))
            {
                strBuseGbn = "1";
            }
            else
            {
                SQL = "";
                SQL = "SELECT BUSE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["BUSE"].ToString().Trim())
                    {
                        case "077201":
                            strBuseGbn = "1"; //'기획행정과
                            break;
                        case "077401":
                        case "077402":
                        case "077404":
                            strBuseGbn = "2"; //'원무과
                            break;
                        case "044520":
                        case "101772":
                        case "100800":
                        case "100790":
                            strBuseGbn = "3"; //'일반건진
                            break;
                        case "078301":
                            strBuseGbn = "4"; //'안전보건팀
                            break;
                    }
                }

                dt.Dispose();
                dt = null;
            }
        }

        private void Combo_Add()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strBuse = "";
            string cCode = "";
            string cName = "";

            SQL = "";
            SQL = "SELECT BUSE";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE DELDATE IS NULL ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strBuse = strBuse + "'" + dt.Rows[i]["BUSE"].ToString().Trim() + "',";
                }

                strBuse = VB.Mid(strBuse, 1, VB.Len(strBuse) - 1);
            }

            dt.Dispose();
            dt = null;

            if (strBuse == "")
            {
                ComFunc.MsgBox("세팅 된 부서가 없습니다");
                return;
            }

            //'수정(允2006-01-10) 심사계 => 심사과
            //
            //'    strSql = " SELECT  Bucode, Name  FROM  KOSMOS_PMPA.BAS_BUSE "
            //'    strSql = strSql & " WHERE  Bucode IN ('033101','044101','044201','044301','044501','055100','055200', '066101','077101', '070101',"
            //'                                          '간호부 약제과   기록실 영양실  건강관리 방사선   임상병리 관리과   비서실 기획행정과
            //'    strSql = strSql & "                   '077501','078201','077601','077201','077301','077401','088100', '077701','076010',         "
            //'                                          '전산실 심사과   도서실 총무과   경리과 원무과   원목실 QI실      구매과
            //'    strSql = strSql & "                    '044401','044411','055301','077901','076001', '078101','078001', '088201', '076010', '101730') "
            //'                                          '정신의료 임상심리                  적정관리실 QI실   감염관리실 장례식장 구매과, 어린이집

            SQL = "";
            SQL = "SELECT A.BUCODE, A.NAME";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BUSE A, KOSMOS_ADM.INSA_DOCU_BUSE B";
            SQL = SQL + ComNum.VBLF + " WHERE A.BUCODE IN (" + strBuse + ") ";
            SQL = SQL + ComNum.VBLF + "   AND A.BUCODE = B.BUSE ";
            SQL = SQL + ComNum.VBLF + " Order by B.RANKING ASC, A.NAME ASC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            CboBuse.Items.Clear();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cCode = dt.Rows[i]["BUCODE"].ToString().Trim();
                    cName = dt.Rows[i]["NAME"].ToString().Trim();

                    //'2019-06-13 안정수, 총무팀 요청으로 40H(033121) 추가, 40H -> 호스피스완화센터로 표기하도록 추가
                    if (cCode.Equals("033121"))
                    {
                        CboBuse.Items.Add("호스피스완화센터" + VB.Space(20) + cCode);
                    }
                    else
                    {
                        CboBuse.Items.Add(cName + VB.Space(20) + cCode);
                    }
                }
            }

            dt.Dispose();
            dt = null;

            //'문서번호
            CboDocuNo.Items.Clear();
            CboDocuNo.Items.Add("");

            SQL = "";
            SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
            SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CboDocuNo.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;
            CboDocuNo.SelectedIndex = 0;

            //'기관명
            CboPlaceName.Items.Clear();
            CboPlaceName.Items.Add("");

            SQL = "";
            SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1 ";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2' ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
            SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CboPlaceName.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;
            CboPlaceName.SelectedIndex = 0;

            //'공문명
            CboDocuName.Items.Clear();
            CboDocuName.Items.Add("");

            SQL = "";
            SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '3'";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
            SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CboDocuName.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;
            CboDocuName.SelectedIndex = 0;
        }

        private void Data_Clear()
        {
            TxtWorkDay.Text = clsPublic.GstrSysDate;
            CboDocuNo.SelectedIndex = -1;
            CboPlaceName.SelectedIndex = -1;
            CboDocuName.SelectedIndex = -1;
            CboBuse.SelectedIndex = -1;
            TxtYear.Enabled = true;
            TxtOutMan.Text = "";
            TxtPage.Text = "";
        }

        private void Data_Display()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strYear = "";
            string strSeqno = "";

            strYear = VB.Trim(TxtYear.Text);
            strSeqno = VB.Trim(TxtSeqNo.Text);

            SQL = "";
            SQL = "SELECT YEAR, SEQNO, DOCUNO, PLACENAME, DOCUNAME, BUSE, OUTMAN, PAGE, ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(WORKDAY, 'YYYY-MM-DD') WORKDAY ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU ";

            if (OptGB0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '0'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
            }

            SQL = SQL + ComNum.VBLF + "   AND YEAR = '" + strYear + "'";
            SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + strSeqno + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSEGBN = '" + strBuseGbn + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                CboDocuNo.Text = dt.Rows[0]["DOCUNO"].ToString().Trim();
                TxtWorkDay.Text = dt.Rows[0]["WORKDAY"].ToString().Trim();
                CboPlaceName.Text = dt.Rows[0]["PLACENAME"].ToString().Trim();
                CboDocuName.Text = dt.Rows[0]["DOCUNAME"].ToString().Trim();
                TxtPage.Text = dt.Rows[0]["PAGE"].ToString().Trim();

                for (int i = 0; i < CboBuse.Items.Count; i++)
                {
                    if (dt.Rows[0]["BUSE"].ToString().Trim() == VB.Right(CboBuse.Items[i].ToString(), 6))
                    {
                        CboBuse.SelectedIndex = i;
                        break;
                    }
                }

                TxtOutMan.Text = dt.Rows[0]["OUTMAN"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TxtSeqNo.Text = "";
            Data_Clear();

            TxtYear.Enabled = true;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;
            BtnDelete.Enabled = false;
            TxtYear.Focus();

            TxtOutMan.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun.PadLeft(5, '0'));
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            if (ComFunc.MsgBoxQ("정말 삭제하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            cYear = TxtYear.Text;
            cSeqNo = TxtSeqNo.Text;

            if (OptGB0.Checked == true) cGubun = "0";
            if (OptGB1.Checked == true) cGubun = "1";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM KOSMOS_ADM.INSA_DOCU";
                SQL = SQL + ComNum.VBLF + " WHERE YEAR =  '" + cYear + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUSEGBN  = '" + strBuseGbn + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;

                TxtSeqNo.Text = "";
                Data_Clear();

                TxtYear.Enabled = true;
                OptGB0.Enabled = true;
                OptGB1.Enabled = true;
                TxtSeqNo.Enabled = true;
                BtnDelete.Enabled = false;
                TxtSeqNo.Focus();
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void BtnDocuName_ADD_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strCode = "";
            string strName = "";

            if (ComFunc.MsgBoxQ("현재 공문명을 상용구에 새롭게 추가하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT MAX(CODE) SEQNO";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strCode = dt.Rows[0]["SEQNO"].ToString().Trim().PadLeft(4, '0');
                }

                dt.Dispose();
                dt = null;

                strName = VB.Trim(CboDocuName.Text);

                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU_CODE1 (CODE, CODENAME, ENTDATE, GUBUN) ";
                SQL = SQL + ComNum.VBLF + "VALUES ('" + strCode + "', '" + strName + "', SYSDATE, '3') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                CboDocuName.Items.Clear();
                CboDocuName.Items.Add("");

                SQL = "";
                SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '3'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CboDocuName.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
                CboDocuName.Text = strName;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void BtnDocuName_Click(object sender, EventArgs e)
        {
            string GstrRetValue = "3";

            frmDocuRemark frmDocuRemarkX = new frmDocuRemark(GstrRetValue);
            frmDocuRemarkX.StartPosition = FormStartPosition.CenterParent;
            frmDocuRemarkX.ShowDialog();
            frmDocuRemarkX = null;

            GstrRetValue = "";
        }

        private void BtnDocuNo_Click(object sender, EventArgs e)
        {
            string GstrRetValue = "1";

            frmDocuRemark frmDocuRemarkX = new frmDocuRemark(GstrRetValue);
            frmDocuRemarkX.StartPosition = FormStartPosition.CenterParent;
            frmDocuRemarkX.ShowDialog();
            frmDocuRemarkX = null;

            GstrRetValue = "";
        }

        private void BtnPlaceName_ADD_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strCode = "";
            string strName = "";

            if (ComFunc.MsgBoxQ("현재 기관명을 상용구에 새롭게 추가하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT MAX(CODE) SEQNO";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strCode = dt.Rows[0]["SEQNO"].ToString().Trim().PadLeft(4, '0');
                }

                dt.Dispose();
                dt = null;

                strName = VB.Trim(CboPlaceName.Text);

                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU_CODE1 (CODE,CODENAME,ENTDATE,GUBUN) ";
                SQL = SQL + ComNum.VBLF + "VALUES ('" + strCode + "', '" + strName + "', SYSDATE,'2') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                CboPlaceName.Items.Clear();
                CboPlaceName.Items.Add("");

                SQL = "";
                SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CboDocuName.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
                CboPlaceName.Text = strName;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void BtnPlaceName_Click(object sender, EventArgs e)
        {
            string GstrRetValue = "2";

            frmDocuRemark frmDocuRemarkX = new frmDocuRemark(GstrRetValue);
            frmDocuRemarkX.StartPosition = FormStartPosition.CenterParent;
            frmDocuRemarkX.ShowDialog();
            frmDocuRemarkX = null;

            GstrRetValue = "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            if (VB.Trim(TxtYear.Text) == "")
            {
                ComFunc.MsgBox("년도를 입력하세요.!!");
                return;
            }

            if (VB.Trim(TxtSeqNo.Text) == "")
            {
                ComFunc.MsgBox("일련번호를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboDocuNo.Text) == "")
            {
                ComFunc.MsgBox("문서번호를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboPlaceName.Text) == "")
            {
                ComFunc.MsgBox("기관명을 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboDocuName.Text) == "")
            {
                ComFunc.MsgBox("공문명을 입력하세요.!!");
                return;
            }

            if (VB.Trim(TxtWorkDay.Text) == "")
            {
                ComFunc.MsgBox("작업일자를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboBuse.Text) == "")
            {
                ComFunc.MsgBox("부서를 입력하세요.!!");
                return;
            }

            if (ComFunc.MsgBoxQ("저장 하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            cBuse = VB.Right(CboBuse.Text, 6);
            cYear = TxtYear.Text;
            cSeqNo = TxtSeqNo.Text;
            cPAGE = TxtPage.Text;

            if (OptGB0.Checked == true) cGubun = "0";
            if (OptGB1.Checked == true) cGubun = "1";

            SQL = "";
            SQL = "SELECT * FROM KOSMOS_ADM.INSA_DOCU";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + cYear + "'";
            SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSEGBN  = '" + strBuseGbn + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU ";
                SQL = SQL + ComNum.VBLF + " (YEAR, SEQNO, GUBUN, DOCUNO,";
                SQL = SQL + ComNum.VBLF + "  WORKDAY, PLACENAME, DOCUNAME, BUSE, OUTMAN, PAGE, BUSEGBN)";
                SQL = SQL + ComNum.VBLF + "  VALUES (";
                SQL = SQL + ComNum.VBLF + "  '" + cYear + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(TxtSeqNo.Text) + "', ";

                if (OptGB0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  '0', ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  '1', ";
                }

                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(CboDocuNo.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  TO_DATE('" + VB.Trim(TxtWorkDay.Text) + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(CboPlaceName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "   '" + VB.Trim(CboDocuName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + cBuse + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(TxtOutMan.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + cPAGE + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + strBuseGbn + "') ";
            }
            else
            {
                SQL = "";
                SQL = "UPDATE KOSMOS_ADM.INSA_DOCU SET";
                SQL = SQL + ComNum.VBLF + "       DOCUNO = '" + VB.Trim(CboDocuNo.Text) + "',";
                SQL = SQL + ComNum.VBLF + "       WORKDAY = TO_DATE('" + VB.Trim(TxtWorkDay.Text) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "       PLACENAME = '" + VB.Trim(CboPlaceName.Text) + "',";
                SQL = SQL + ComNum.VBLF + "       DOCUNAME = '" + VB.Trim(CboDocuName.Text) + "',";
                SQL = SQL + ComNum.VBLF + "       BUSE = '" + cBuse + "', ";
                SQL = SQL + ComNum.VBLF + "       PAGE = '" + cPAGE + "', ";
                SQL = SQL + ComNum.VBLF + "       OUTMAN = '" + VB.Trim(TxtOutMan.Text) + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + cYear + "'";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "'";
                SQL = SQL + ComNum.VBLF + "   AND BUSEGBN = '" + strBuseGbn + "'";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            TxtYear.Enabled = true;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;
        }

        private void BtnSeqNoShow_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nMaxNo = 0;

            SQL = "";
            SQL = "SELECT NVL(MAX(SEQNO), 0) AS MAXNO";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU";

            if (OptGB0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '0'";
            }
            else if (OptGB1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            }

            SQL = SQL + ComNum.VBLF + "   AND YEAR = '" + TxtYear.Text + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSEGBN = '" + strBuseGbn + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            nMaxNo = 0;

            if (dt.Rows.Count > 0)
            {
                nMaxNo = Convert.ToInt32(dt.Rows[0]["MaxNo"].ToString().Trim());
            }

            TxtSeqNo.Text = string.Format("{0:0000}", nMaxNo + 1);

            if (OptGB0.Checked == true)
            {
                CboDocuNo.Focus();
            }
            else if (OptGB1.Checked == true)
            {
                CboDocuNo.Text = VB.Trim(TxtYear.Text) + "-" + string.Format("{0:0000}", nMaxNo + 1);
                CboDocuNo.Focus();
            }
            else
            {
                TxtWorkDay.Focus();
            }

            TxtYear.Enabled = false;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;
            BtnDelete.Enabled = false;
        }

        private void TxtSeqNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            BtnCancel.PerformClick();

            SQL = "";
            SQL = "SELECT SEQNO, DOCUNO, DOCUNAME, PLACENAME, TO_CHAR(WORKDAY, 'YYYY-MM-DD') WORKDAY";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + VB.Trim(TxtYear.Text) + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSEGBN = '" + strBuseGbn + "'";

            if (OptGB0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '0'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '1'";
            }

            SQL = SQL + ComNum.VBLF + "   ORDER BY SEQNO DESC";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ss1.ActiveSheet.RowCount = 0;
                ss1.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["seqno"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["WorkDay"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["docuno"].ToString().Trim();
                    ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DocuName"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void BtnDocuNo_ADD_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strCode = "";
            string strName = "";

            if (ComFunc.MsgBoxQ("현재 문서번호를 상용구에 새롭게 추가하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT MAX(CODE) SEQNO";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strCode = dt.Rows[0]["SEQNO"].ToString().Trim().PadLeft(4, '0');
                }

                dt.Dispose();
                dt = null;

                strName = VB.Trim(CboDocuName.Text);

                SQL = "";
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU_CODE1 (CODE,CODENAME,ENTDATE,GUBUN) ";
                SQL = SQL + ComNum.VBLF + "VALUES ('" + strCode + "', '" + strName + "', SYSDATE,'1') ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                CboDocuNo.Items.Clear();
                CboDocuNo.Items.Add("");

                SQL = "";
                SQL = "SELECT CODE, CODENAME, ENTDATE, DELDATE, GUBUN ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU_CODE1 ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODENAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CboDocuNo.Items.Add(dt.Rows[i]["CODENAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
                CboDocuNo.Text = strName;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void CboDocuNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        private void CboPlaceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        private void CboBuse_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            TxtSeqNo.Text = ss1.ActiveSheet.Cells[e.Row, 0].Text;

            Data_Clear();
            Data_Display();

            TxtYear.Enabled = false;
            TxtSeqNo.Enabled = false;
            BtnDelete.Enabled = true;
        }

        private void TxtOutMan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.SendWait("{TAB}");
            }
        }

        private void TxtSeqNo_Leave(object sender, EventArgs e)
        {
            Data_Display();
            TxtYear.Enabled = false;
            BtnDelete.Enabled = true;
        }

        private void frmDocu_01_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocu_01_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
