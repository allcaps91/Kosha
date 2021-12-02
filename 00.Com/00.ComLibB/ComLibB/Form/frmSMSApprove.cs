using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSMSApprove.cs
    /// Description     : 휴대전화, E-Mail, 전화번호 변경하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-07
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\OPD\oumsad\OUMSAD35.FRM(FrmSMSApprove) => frmSMSApprove.cs 으로 변경함
    /// TODO로 처리했던 함수들 구현 후 오류 발생하여 소스 수정 하였음
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\OPD\oumsad\OUMSAD35.FRM(FrmSMSApprove)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\OPD\oumsad\oumsad.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSMSApprove : Form
    {
        public string fstrTel = "";
        public string fstrHPhone = "";
        public string fstrEmail = "";
        public string fstrGbSMS = "";
        public int fnErrorCnt = 0;
        //TODO: OumSad2(OUMSAD.BAS)에 있는 변수 사용. 처리 어떻게 할지 ..               
        string strDataFLAG = "";

        void PressKey(int keyChar)
        {

        }
    
        public frmSMSApprove()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 화면정리 함수
        /// </summary>
        void Screen_Clear()
        {
            txtPano.Text = ""; lblSName.Text = ""; lblApprov.Text = "";
            txtTel.Text = ""; txtHphone.Text = ""; txtEmail.Text = "";
            optApprove0.Checked = false; optApprove1.Checked = false;

            fstrTel = ""; fstrHPhone = ""; fstrEmail = ""; fstrGbSMS = "";
            fnErrorCnt = 0;

            txtTel.Enabled = false;
            txtHphone.Enabled = false;
            txtEmail.Enabled = false;
            grbApprove.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        //TODO: OumSad2(OUMSAD.BAS)에 있는 함수 사용.
        /// <summary>
        /// 전화번호 체크 함수
        /// </summary>
        /// <param name="argTel"></param>
        /// <param name="argMail"></param>
        /// <param name="argBuildNo"></param>
        /// <returns></returns>
        string TelNo_Check(string argTel, string argMail, string argBuildNo = "")
        {
            int Inx = 0;
            string strTelNo = "";
            string rtnVal = "";

            if (argTel.Trim() == "")
            {
                return "";
            }

            //유효숫자만 정리
            strTelNo = "";
            for (Inx = 0; Inx < VB.Len(argTel); Inx++)
            {
                if(String.Compare(VB.Mid(argTel, Inx + 1, 1), "0") >= 0 && String.Compare(VB.Mid(argTel, Inx + 1, 1), "9") <= 0)
                {
                    strTelNo += VB.Mid(argTel, Inx + 1, 1);
                }
            }
                        
            if (VB.Len(strTelNo) < 7)
            {
                rtnVal = "전화번호 오류: 국번호가 2자리입니다.";
            }

            else if (VB.Left(argTel, 3) == "054")
            {
                rtnVal = "경북은 지역번호를 입력하지 마세요";
            }

            else
            {
                if (VB.Left(strTelNo, 3) != "070") //인터넷 전화
                {
                    if (argBuildNo != "")
                    {
                        if (!(String.Compare(argMail, "360") >=0 && !(String.Compare(argMail, "402") <= 0)))
                        {
                            if (VB.Left(strTelNo, 1) != "0" && argMail != "" && argMail != "000")
                            {
                                rtnVal = "타지역은 반드시 DDD번호를 입력하세요.";
                            }
                        }
                    }

                    else
                    {
                        if (!(String.Compare(argMail, "712") >= 0 && !(String.Compare(argMail, "799") <= 0)))
                        {
                            if(VB.Left(strTelNo, 1) != "0" && argMail != "" && argMail != "000")
                            {
                                rtnVal = "타지역은 반드시 DDD번호를 입력하세요.";
                            }
                        }
                    }
                }
            }

            return rtnVal;
        }

        //TODO: OumSad2(OUMSAD.BAS)에 있는 함수 임시 만들어 사용
        void BAS_SMS_Approve_Insert(string strPano, string strTel, string strHPhone, string strEmail, string strGbSMS, string strFlag)
        {
            if (strFlag != "OK")
            {
                return;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region 환자인적사항 변경 내역 백업
                ComFunc CF1 = new ComFunc();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("TEL", strTel);
                dict.Add("HPHONE", strHPhone);
                CF1.INSERT_BAS_PATIENT_HIS(strPano, dict);
                #endregion

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "   SET Tel='" + strTel + "',";
                SQL = SQL + ComNum.VBLF + "       HPhone='" + strHPhone + "',";
                SQL = SQL + ComNum.VBLF + "       HPhone2='" + strHPhone + "',";    //2012-03-30
                switch (clsType.User.JobGroup)
                {
                    case "JOB002001":
                    case "JOB002002":
                    case "JOB002003":
                    case "JOB002007":
                    case "JOB002008":
                        SQL = SQL + ComNum.VBLF + "       GBSMS='" + strGbSMS + "', ";
                        CF1.FUNC_GBSMS_HISTORY(clsDB.DbCon, strPano, strGbSMS);
                        break;
                }
                SQL = SQL + ComNum.VBLF + "       EMail='" + strEmail + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano='" + strPano + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    strDataFLAG = "NO";
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
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
         
        bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            string strTel = "";
            string strHPhone = "";
            string strEmail = "";
            string strGbSMS = "";
            string strMsg = "";
            string SQL = "";    //Query문
            clsDB.setBeginTran(clsDB.DbCon);

            strTel = txtTel.Text.Trim();
            strHPhone = txtHphone.Text.Trim();
            strEmail = txtEmail.Text.Trim();

            if (strHPhone == "")
            {
                MessageBox.Show("휴대폰번호가 공란입니다.", "오류");
                return false;
            }
            if (strTel == "")
            {
                MessageBox.Show("전화번호가 공란입니다.", "오류");
                return false;
            }

            if(HandPhoneNumber_Check(strHPhone) != "OK")
            {
                MessageBox.Show("핸드폰번호를 확인하세요. 정확한 번호를 넣어주세요. 핸드폰이 없으면 빈공란으로 입력해주세요", "확인");
            }

            strMsg = TelNo_Check(strTel, "");
            if (strMsg != "")
            {
                MessageBox.Show(strMsg, "전화번호 오류");
                return false;
            }

            try
            {
                //TODO: OumSad2(OUMSAD.BAS)에 있는 변수 사용. 처리 어떻게 할지 ..               
                string strDataFLAG = "OK";

                if (optApprove0.Checked == true) //동의
                {
                    strGbSMS = "Y";
                }
                else if (optApprove1.Checked == true) //동의안함
                {
                    strGbSMS = "X";
                }
                else if (optApprove2.Checked == true) //요청
                {
                    strGbSMS = "N";
                }

                BAS_SMS_Approve_Insert(txtPano.Text, strTel, strHPhone, strEmail, strGbSMS, strDataFLAG);

                if (strDataFLAG == "OK")
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("수정하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    strDataFLAG = "NO";
                    ComFunc.MsgBox("해당 작업중 ERROR");
                }

                clsPublic.GstrChoicePano = "";

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

        string HandPhoneNumber_Check(string ArgNumber)
        {
            string strHandPhone = "";
            string rtnVal = "OK";
            int i = 0;

            if(ArgNumber.Trim() == "")
            {
                return rtnVal;
            }

            for(i = 0; i < VB.Len(ArgNumber.Trim()); i++)
            {
                switch(VB.Asc(VB.Mid(ArgNumber, i + 1 , 1)))
                {
                    case 48:
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                    case 56:
                    case 57:                    
                        strHandPhone += VB.Mid(ArgNumber, i + 1, 1);
                        break;                 
                }
            }

            //번호 Check
            switch(VB.Left(strHandPhone, 3))
            {
                case "010":
                case "011":
                case "016":
                case "017":
                case "018":
                case "019":
                case "070":
                    break;

                default:
                    rtnVal = "NO";
                    break;
            }

            //016-813-4394번호길이 check
            //3  + 3 +   4  = 10(최소한 10자리 이상이여야 합니다.)
            
            if(VB.Len(strHandPhone) < 10)
            {
                rtnVal = "NO";
            }
            return rtnVal;
        }

        void frmSMSApprove_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            Screen_Clear();

            if (clsPublic.GstrChoicePano.Trim() != "")
            {
                txtPano.Text = clsPublic.GstrChoicePano;
                txtPano.Enabled = false;
                txtPano_KeyPress(sender, new KeyPressEventArgs('\r')); //carriage return 값 전달
            }
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13 || txtPano.Text.Trim() == "")
            {
                return;
            }

            string strMsg = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            //환자마스타에서 자료를 읽어 Display
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SName,Tel,Hphone,Email,GbSMS ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT ";
            SQL = SQL + ComNum.VBLF + " WHERE Pano='" + txtPano.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            lblSName.Text = dt.Rows[0]["Sname"].ToString().Trim();
            txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
            txtHphone.Text = dt.Rows[0]["HPhone"].ToString().Trim();
            txtEmail.Text = dt.Rows[0]["EMail"].ToString().Trim();

            fstrTel = dt.Rows[0]["Tel"].ToString().Trim();
            fstrHPhone = dt.Rows[0]["HPhone"].ToString().Trim();
            fstrEmail = dt.Rows[0]["EMail"].ToString().Trim();

            if(dt.Rows[0]["GbSMS"].ToString().Trim() == "Y")
            {
                optApprove0.Checked = true;
            }
            else if (dt.Rows[0]["GbSMS"].ToString().Trim() == "N" || dt.Rows[0]["GbSMS"].ToString().Trim() == "")
            {
                optApprove2.Checked = true;
            }
            else if (dt.Rows[0]["GbSMS"].ToString().Trim() == "X")
            {
                optApprove1.Checked = true;
            }

            dt.Dispose();
            dt = null;
            SqlErr = "";

            //최종 승인 취소내역 Display
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT AppGbn,TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate,EntSabun ";
            SQL = SQL + ComNum.VBLF + "  FROM BAS_SMS_APPROVE ";
            SQL = SQL + ComNum.VBLF + " WHERE Pano='" + txtPano.Text + "' ";
            SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            strMsg = "";
            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["AppGbn"].ToString())
                {
                    case "Y": strMsg = dt.Rows[0]["EntDate"].ToString() + " <승인>"; break;
                    case "N": strMsg = dt.Rows[0]["EntDate"].ToString() + " <미승인>"; break;
                    case "X": strMsg = dt.Rows[0]["EntDate"].ToString() + " <승인>"; break;
                    default: strMsg = dt.Rows[0]["EntDate"].ToString() + " <미승인>"; break;
                }
            }

            dt.Dispose();
            dt = null;

            lblApprov.Text = strMsg;

            txtTel.Enabled = true;
            txtHphone.Enabled = true;
            txtEmail.Enabled = true;
            grbApprove.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == false) return;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }
    }
}
