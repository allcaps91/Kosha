using ComBase;
using ComDbB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class FrmPassChange : Form
    {
        public int FnID = 0;
        public string FstrLicense = "";
        public FrmPassChange()
        {
            InitializeComponent();

            FstrLicense = clsType.HosInfo.SwLicense;
            lblSabunInfo.Text = "";
            READ_UserInfo();
        }

        private void READ_UserInfo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            FnID = Int32.Parse(clsType.User.IdNumber);


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_USERMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + FstrLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  And Sabun = " + FnID + " ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    lblSabunInfo.Text = FnID.ToString() + " ";
                    lblSabunInfo.Text += dt.Rows[i]["Name"].ToString().Trim();
                    lblSabunInfo.Text += " 비밀번호 변경";
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

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private void FrmPassChange_Load(object sender, EventArgs e)
        {

        }

        private void FrmPassChange_Load_1(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string strPass1 = "";
            string strPass2 = "";
            char strChar;
            int nCnt1 = 0;
            int nCnt2 = 0;
            int nCnt3 = 0;
            int nCnt4 = 0;
            string SQL = string.Empty;
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            strPass1 = txtPass1.Text.Trim();
            strPass2 = txtPass2.Text.Trim();

            if (strPass1.Length < 6) { ComFunc.MsgBox("비밀번호가 6자리 이상만 가능합니다."); return; }
            if (strPass1 != strPass2) { ComFunc.MsgBox("확인 비밀번호가 틀립니다."); return; }
            for (int i = 0; i < strPass1.Length; i++)
            {
                strChar = Convert.ToChar(strPass1.Substring(i, 1));
                if (strChar >= '0' && strChar <= '9') nCnt1++;
                if (strChar >= 'a' && strChar <= 'z') nCnt2++;
                if (strChar >= 'A' && strChar <= 'Z') nCnt3++;
                if (strChar == '!' || strChar == '@') nCnt4++;
                if (strChar == '#' || strChar == '$') nCnt4++;
                if (strChar == '%' || strChar == '^') nCnt4++;
                if (strChar == '&' || strChar == '*') nCnt4++;
                if (strChar == '(' || strChar == ')') nCnt4++;
            }
            if (nCnt1 == 0) { ComFunc.MsgBox("0~9 숫자가 1글자도 포함이 안됨"); return; }
            if (nCnt2 == 0) { ComFunc.MsgBox("a~z 영어 소문자 1글자도 포함이 안됨"); return; }
            if (nCnt3 == 0) { ComFunc.MsgBox("A~Z 영어 대문자 1글자도 포함이 안됨"); return; }
            if (nCnt4 == 0) { ComFunc.MsgBox("특수문자 !@#$%^&*() 1글자도 포함이 안됨"); return; }

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE HIC_USERMST SET ";
                SQL += ComNum.VBLF + "        Password      = '" + clsAES.DeAES(strPass1) + "', ";
                SQL += ComNum.VBLF + "        PassChange    = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                SQL += ComNum.VBLF + "  WHERE LicNo         = '" + FstrLicense + "'";
                SQL += ComNum.VBLF + "    AND Sabun         = "  + FnID + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("변경 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("변경되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }
    }
}