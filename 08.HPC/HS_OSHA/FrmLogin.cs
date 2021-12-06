using ComBase;
using ComDbB;
using HC_OSHA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HS_OSHA
{
    public partial class FrmLogin : Form
    {
        public static string FstrLicno = "";
        public static string FstrSangho = "";
        public static string FstrPassword = "";
        public static string FstrEndDate = "";

        public FrmLogin()
        {
            InitializeComponent();
            if (READ_Licno_Disk() == false) this.Close();
            if (READ_Licno_Server() == false) this.Close();

            GuideMsg_Display();

            clsDB.GetDbInfo();
            clsDB.DbCon = clsDB.DBConnect_Cloud();
        }

        private void txtIdNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool READ_Licno_Disk()
        {
            string strPcData = "";
            string strNewData = "";

            clsType.HosInfo.SwLicense = "";
            clsType.HosInfo.SwLicInfo = "";

            //C:\Windows\System32\acledit392io87.dll
            //파일형식: 라이선스번호{}회사명{}종료일자{}관리자비번{}
            string strLicFile = @"C:\Windows\System32\acledit392io87.dll";
            if (System.IO.File.Exists(strLicFile) == true)
            {
                strPcData = System.IO.File.ReadAllText(strLicFile);
                strNewData = clsAES.DeAES(strPcData);
                if (VB.L(strNewData, "{}") != 5)
                {
                    ComFunc.MsgBox("라이선스 정보가 손상되어 종료됩니다.");
                    return false;
                }

                clsType.HosInfo.SwLicense = VB.Pstr(strNewData, "{}", 1);
                clsType.HosInfo.SwLicInfo = strNewData;
                return true;
            }
            else
            {
                ComFunc.MsgBox("라이선스 정보가 손상되어 종료됩니다.");
                return false;
            }
        }

        // 라이선스 서버에서 상세정보를 읽음 //
        private bool READ_Licno_Server()
        {
            string SQL = "";
            string strNewData = "";
            string strEndDate = "";
            string strPcData = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                Cursor.Current = Cursors.Default;
                return true;
            }

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + clsType.HosInfo.SwLicense + "' ";
                dt = clsDbMySql.GetDataTable(SQL);

                strNewData = "";
                if (dt.Rows.Count > 0)
                {
                    strNewData = clsType.HosInfo.SwLicense + "{}";
                    strNewData += dt.Rows[0]["Sangho"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["EDate"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["AdminPass"].ToString().Trim() + "{}";
                }

                dt.Dispose();
                dt = null;

                if (strNewData != "")
                {
                    if (clsType.HosInfo.SwLicInfo != strNewData)
                    {
                        clsType.HosInfo.SwLicInfo = strNewData;
                        strPcData = clsAES.AES(strNewData);
                        System.IO.File.WriteAllText(@"C:\Windows\System32\acledit392io87.dll", strPcData);
                    }
                }

                if (VB.Pstr(strNewData, "{}", 3) != "")
                {
                    strEndDate = VB.Pstr(strNewData, "{}", 3);
                    strEndDate = VB.Pstr(strEndDate, "-", 1) + VB.Pstr(strEndDate, "-", 2) + VB.Pstr(strEndDate, "-", 3);

                    if (VB.Val(strEndDate) < VB.Val(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        ComFunc.MsgBox("라이선스 만기일이 경과되어 종료됩니다.");
                        return false;
                    }
                }

                lblLicno.Text = clsType.HosInfo.SwLicense;
                lblSangho.Text = VB.Pstr(strNewData, "{}", 2);

                Cursor.Current = Cursors.Default;
                return true;
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
                return false;
            }
        }

        private void GuideMsg_Display()
        {
            string SQL = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMSG ";

                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {
                    txtGuide.Text = dt.Rows[0]["Remark"].ToString();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int nSabun = 0;

            if (txtIdNumber.Text.Trim() == "admin")
            {
                if (txtPassword.Text.Trim() == VB.Pstr(clsType.HosInfo.SwLicInfo, "{}", 4))
                {
                    clsType.User.Sabun = "admin";
                    clsType.User.IdNumber = "1";
                    clsType.User.JobName = "관리자";
                    clsType.User.BuseName = "관리자";
                } else {
                    ComFunc.MsgBox("관리자 비밀번호 오류 입니다", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

            } else {
                if (Set_UserInfo(txtIdNumber.Text.Trim(), txtPassword.Text.Trim()) == false)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            // 프로그램 실행
            Dashboard form = new Dashboard();
            form.Show();
        }

        private bool Set_UserInfo(string ArgSabun, string argPassword)
        {
            string SQL = "";
            string SqlErr = "";
            string strPassword = "";
            DataTable dt = null;
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT Name,DEPT,PASSHASH256 FROM HIC_USERS ";
                SQL = SQL + ComNum.VBLF + "Where SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  And USERID = '" + ArgSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    ComFunc.MsgBox("등록 안된 사용자입니다.", "알림");
                    return false;
                }

                strPassword = clsAES.DeAES(dt.Rows[i]["PASSHASH256"].ToString().Trim());
                if (strPassword != argPassword)
                {
                    dt.Dispose();
                    dt = null;

                    ComFunc.MsgBox("비밀번호가 틀립니다.", "알림");
                    return false;
                }

                clsType.User.Sabun = ArgSabun;
                clsType.User.IdNumber = ArgSabun;
                clsType.User.JobName = dt.Rows[i]["Name"].ToString().Trim();
                clsType.User.BuseName = dt.Rows[i]["DEPT"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void txtGuide_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
