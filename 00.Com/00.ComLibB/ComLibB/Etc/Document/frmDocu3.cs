using ComBase;
using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocu3 : Form, MainFormMessage
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

        string cCode = "";
        string cName = "";

        string cYear = "";
        string cSeqNo = "";
        string cGubun = "";
        string cBuse = "";
        string cDocuNo = "";
        string cDocuName = "";
        string cPlaceName = "";
        string cPlaceName2 = "";
        string cWorkday = "";
        string cOUTMAN = "";
        string cPAGE = "";
        string strBuseGbn = "";

        public frmDocu3()
        {
            InitializeComponent();
        }

        public frmDocu3(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocu3(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmDocu3_Load(object sender, EventArgs e)
        {
            string strBuse = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            Combo_Add();
            Data_Clear();
            TxtYear.Text = DateTime.Parse(clsPublic.GstrSysDate).ToString("yyyy");
            DtpWorkDay.Value = DateTime.Parse(clsPublic.GstrSysDate);
            BtnDelete.Enabled = false;
            OptGB0.Checked = true;

            //ComboDept_Load();  //'(원외공문 담당부서 목록조회)

            if (VB.Right(CboBuse.Text, 6) == "055201")
            {
                CboBuse.Items.Add("병리과               .055201");
            }
            else if (VB.Right(CboBuse.Text, 6) == "077201")
            {
                CboBuse.Items.Add("어린이집               .101730");
            }

            TxtOutMan.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun.PadLeft(5, '0'));
        }

        private void Data_Clear()
        {
            TxtDocuNo.Text = "";
            DtpWorkDay.Value = DateTime.Parse(clsPublic.GstrSysDate);
            CboPlaceName.Text = "";
            CboPlaceName2.Text = "";
            CboDocuName.Text = "";
            TxtSeqNo.Text = "";
            TxtPage.Text = "";
        }

        private void Combo_Add()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //'---< 부서코드 (반단위) >----------------------------------------
            string strBuse = "";

            SQL = "SELECT BUSE FROM KOSMOS_ADM.INSA_DOCU_BUSE ";
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

            SQL = "SELECT BUCODE, NAME FROM KOSMOS_PMPA.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BUCODE IN (" + strBuse + ") ";
            SQL = SQL + ComNum.VBLF + " ORDER BY BUCODE ";

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
                    CboBuse.Items.Add(cName + VB.Space(20) + cCode);
                    if (clsType.User.BuseCode.Equals(cCode)) CboBuse.SelectedIndex = i;
                }
            }
        }

        private void Data_Display()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = "SELECT YEAR, SEQNO, DOCUNO, PLACENAME, PLACENAME2, DOCUNAME, BUSE, OUTMAN, PAGE, ";
            SQL = SQL + ComNum.VBLF + "       TO_CHAR(WORKDAY, 'YYYY-MM-DD') WORKDAY";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU2 ";

            if (OptGB0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '0'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
            }
            SQL = SQL + ComNum.VBLF + "   AND YEAR = '" + TxtYear.Text + "'";
            SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + TxtSeqNo.Text + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + VB.Right(CboBuse.Text, 6) + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                TxtYear.Text = dt.Rows[0]["YEAR"].ToString().Trim();
                TxtSeqNo.Text = dt.Rows[0]["SEQNO"].ToString().Trim().PadLeft(4, '0');
                TxtDocuNo.Text = dt.Rows[0]["DOCUNO"].ToString().Trim();
                TxtPage.Text = dt.Rows[0]["PAGE"].ToString().Trim();

                DtpWorkDay.Value = DateTime.Parse(dt.Rows[0]["WORKDAY"].ToString().Trim());
                CboPlaceName.Text = dt.Rows[0]["PLACENAME"].ToString().Trim();
                CboPlaceName2.Text = dt.Rows[0]["PLACENAME2"].ToString().Trim();
                CboDocuName.Text = dt.Rows[0]["DOCUNAME"].ToString().Trim();

                for (int i = 0; i < CboBuse.Items.Count; i++)
                {
                    if (dt.Rows[0]["Buse"].ToString().Trim() == VB.Right(CboBuse.Items[i].ToString(), 6))
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SsList.ActiveSheet.RowCount = 0;

            SQL = "SELECT SEQNO, DOCUNO, DOCUNAME, PLACENAME, PLACENAME2, OUTMAN, TO_CHAR(WORKDAY, 'YYYY-MM-DD') WORKDAY, PAGE";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU2";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + TxtYear.Text + "'";

            if (OptGB0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   And GUBUN = '0'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '1'";
            }

            SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + VB.Right(CboBuse.Text, 6) + "'";
            SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO DESC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                SsList.ActiveSheet.RowCount = 0;
                SsList.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cSeqNo = dt.Rows[i]["SEQNO"].ToString().Trim();
                    cDocuNo = dt.Rows[i]["DOCUNO"].ToString().Trim();
                    cDocuName = dt.Rows[i]["DOCUNAME"].ToString().Trim();
                    cPlaceName = dt.Rows[i]["PLACENAME"].ToString().Trim();
                    cPlaceName2 = dt.Rows[i]["PLACENAME2"].ToString().Trim();
                    cWorkday = dt.Rows[i]["WORKDAY"].ToString().Trim();
                    cOUTMAN = dt.Rows[i]["OUTMAN"].ToString().Trim();
                    cPAGE = dt.Rows[i]["PAGE"].ToString().Trim();

                    SsList.ActiveSheet.Cells[i, 0].Text = cSeqNo.PadLeft(4, '0');
                    SsList.ActiveSheet.Cells[i, 1].Text = cDocuNo;
                    SsList.ActiveSheet.Cells[i, 2].Text = cDocuName;
                    SsList.ActiveSheet.Cells[i, 3].Text = cPlaceName;
                    SsList.ActiveSheet.Cells[i, 4].Text = cPlaceName2;
                    SsList.ActiveSheet.Cells[i, 5].Text = cOUTMAN;
                    SsList.ActiveSheet.Cells[i, 6].Text = cWorkday;
                    SsList.ActiveSheet.Cells[i, 7].Text = cPAGE;
                }
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
            TxtSeqNo.Focus();
            TxtOutMan.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun.PadLeft(5, '0'));
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

            if (VB.Trim(TxtDocuNo.Text) == "")
            {
                ComFunc.MsgBox("문서번호를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboPlaceName.Text) == "")
            {
                ComFunc.MsgBox("발신처를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboPlaceName2.Text) == "")
            {
                ComFunc.MsgBox("수신처를 입력하세요.!!");
                return;
            }

            if (VB.Trim(CboDocuName.Text) == "")
            {
                ComFunc.MsgBox("공문명을 입력하세요.!!");
                return;
            }

            if (VB.Trim(TxtOutMan.Text) == "")
            {
                ComFunc.MsgBox("담당자를 입력하세요.!!");
                return;
            }

            if (VB.Trim(DtpWorkDay.Text) == "")
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

            if (OptGB0.Checked == true)
            {
                cGubun = "0";
            }

            if (OptGB1.Checked == true)
            {
                cGubun = "1";
            }

            SQL = "SELECT * FROM KOSMOS_ADM.INSA_DOCU2 ";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + cYear + "' ";
            SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BUSE  = '" + VB.Right(CboBuse.Text, 6) + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                SQL = "INSERT INTO KOSMOS_ADM.INSA_DOCU2 ";
                SQL = SQL + ComNum.VBLF + " (YEAR, SEQNO, GUBUN, DOCUNO,";
                SQL = SQL + ComNum.VBLF + "  WORKDAY, PLACENAME, PLACENAME2, DOCUNAME, BUSE, OUTMAN, PAGE, BUSEGBN)";
                SQL = SQL + ComNum.VBLF + "  VALUES (";
                SQL = SQL + ComNum.VBLF + "  '" + cYear + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(TxtSeqNo.Text) + "', ";

                if (OptGB0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  '0',";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  '1',";
                }

                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(TxtDocuNo.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  TO_DATE('" + DtpWorkDay.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(CboPlaceName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(CboPlaceName2.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "   '" + VB.Trim(CboDocuName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + cBuse + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + VB.Trim(TxtOutMan.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + cPAGE + "', ";
                SQL = SQL + ComNum.VBLF + "  '" + strBuseGbn + "') ";

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
            else
            {
                SQL = "UPDATE KOSMOS_ADM.INSA_DOCU2 SET";
                SQL = SQL + ComNum.VBLF + "       DOCUNO = '" + VB.Trim(TxtDocuNo.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "       WORKDAY = TO_DATE('" + DtpWorkDay.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "       PLACENAME = '" + VB.Trim(CboPlaceName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "       PLACENAME2 = '" + VB.Trim(CboPlaceName2.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "       DOCUNAME = '" + VB.Trim(CboDocuName.Text) + "', ";
                SQL = SQL + ComNum.VBLF + "       BUSE = '" + cBuse + "', ";
                SQL = SQL + ComNum.VBLF + "       PAGE = '" + cPAGE + "', ";
                SQL = SQL + ComNum.VBLF + "       OUTMAN = '" + VB.Trim(TxtOutMan.Text) + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE YEAR =  '" + cYear + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + VB.Right(CboBuse.Text, 6) + "' ";

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

            TxtYear.Enabled = true;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;

            BtnSearch.PerformClick();
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

            SQL = "DELETE FROM KOSMOS_ADM.INSA_DOCU2 ";
            SQL = SQL + ComNum.VBLF + " WHERE YEAR = '" + cYear + "'";
            SQL = SQL + ComNum.VBLF + "   AND SEQNO = '" + cSeqNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND GUBUN = '" + cGubun + "'";
            SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + VB.Right(CboBuse.Text, 6) + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return;
            }

            TxtSeqNo.Text = "";
            Data_Clear();

            TxtYear.Enabled = true;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;
            BtnDelete.Enabled = false;
            TxtSeqNo.Focus();

            BtnSearch.PerformClick();
        }

        private void BtnSEQ_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nMaxNo = 0;

            SQL = "SELECT NVL(MAX(SEQNO), 0) AS MAXNO";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU2";

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
            SQL = SQL + ComNum.VBLF + "   AND BUSE = '" + VB.Right(CboBuse.Text, 6) + "'";

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
                nMaxNo = (int)VB.Val(dt.Rows[0]["MaxNo"].ToString().Trim());
            }

            Data_Clear();

            TxtSeqNo.Text = (nMaxNo + 1).ToString("0000");

            if (OptGB0.Checked == true)
            {
                TxtDocuNo.Focus();
            }
            else if (OptGB1.Checked == true)
            {
                TxtDocuNo.Text = CONV_BUSE(VB.Right(CboBuse.Text, 6));

                if (VB.Trim(TxtDocuNo.Text) == "")
                {
                    ComFunc.MsgBox("지정되지 않은 부서의 사용자입니다.");
                    return;
                }

                TxtDocuNo.Text = TxtDocuNo.Text + " " + VB.Trim(TxtYear.Text) + "-" + (nMaxNo + 1).ToString("0000");
                TxtDocuNo.Focus();
            }
            else
            {
                DtpWorkDay.Focus();
            }

            TxtYear.Enabled = true;
            OptGB0.Enabled = true;
            OptGB1.Enabled = true;
            TxtSeqNo.Enabled = true;
            BtnDelete.Enabled = false;
        }

        private string CONV_BUSE(string strBUSE)
        {
            string CONV_BUSE = "";

            switch (strBUSE)
            {
                case "033101": CONV_BUSE = "포성간내"; break;
                case "044501":
                case "011128":
                case "100790":
                case "100800":
                    CONV_BUSE = "포성건내";   //'2011-04-05 김현욱 수정, 건강증진센터, 보건대행, 작업환경 부서는 건진에 포함
                    break;
                case "077301": CONV_BUSE = "포성경내"; break;
                case "066101": CONV_BUSE = "포성관내"; break;
                case "070101": CONV_BUSE = "포성기내"; break;
                case "099101": CONV_BUSE = "포성노내"; break;
                case "077201": CONV_BUSE = "포성총내"; break;
                case "055301": CONV_BUSE = "포성물내"; break;
                case "077901": CONV_BUSE = "포성사내"; break;
                case "044401": CONV_BUSE = "포성정내"; break;
                case "078201": CONV_BUSE = "포성심내"; break;
                case "044101": CONV_BUSE = "포성약내"; break;
                case "055101": CONV_BUSE = "포성영상내"; break;
                case "044301": CONV_BUSE = "포성영내"; break;
                case "077401": CONV_BUSE = "포성원내"; break;
                case "088101": CONV_BUSE = "포성원목내"; break;
                case "044201": CONV_BUSE = "포성의내"; break;
                case "077501": CONV_BUSE = "포성의정내"; break;
                case "077601": CONV_BUSE = "포성의학내"; break;
                case "088201": CONV_BUSE = "포성장내"; break;
                case "076001": CONV_BUSE = "포성의료내"; break;
                case "044411": CONV_BUSE = "포성임내"; break;
                case "055201": CONV_BUSE = "포성진내"; break;
                case "055200": CONV_BUSE = "포성병내"; break;
                case "101730": CONV_BUSE = "포성어내"; break;
                case "076010": CONV_BUSE = "포성구내"; break;
                case "101770": CONV_BUSE = "포성사내"; break;
                case "101768": CONV_BUSE = "포성이념내"; break;
                case "078001": CONV_BUSE = "포성감외내"; break;
            }

            return CONV_BUSE;
        }

        private void SsList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Data_Clear();
            TxtSeqNo.Text = SsList.ActiveSheet.Cells[e.Row, 0].Text;

            Data_Display();
            TxtYear.Enabled = false;
            TxtSeqNo.Enabled = false;
            BtnDelete.Enabled = true;
        }

        private void OptGB0_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                label10.Text = "발신처";
            }
        }

        private void OptGB1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                label10.Text = "수신처";
            }
        }

        private void frmDocu3_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocu3_FormClosed(object sender, FormClosedEventArgs e)
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


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void DtpWorkDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
