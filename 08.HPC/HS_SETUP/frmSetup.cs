using ComBase;
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

namespace HS_SETUP
{
    public partial class frmSetup : Form
    {
        public frmSetup()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string strNewData = "";
            string strPcData = "";
            string strEndDate = "";

            DataTable dt = null;

            if (txtLicense.Text.Trim() == "") { ComFunc.MsgBox("라이선스번호가 공란입니다."); return; }

            Cursor.Current = Cursors.WaitCursor;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("라이선스 서비 접속이 불가능합니다."); 
                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + txtLicense.Text.Trim() + "' ";
                dt = clsDbMySql.GetDataTable(SQL);

                strNewData = "";
                if (dt.Rows.Count > 0)
                {
                    strNewData = txtLicense.Text.Trim() + "{}";
                    strNewData += dt.Rows[0]["Sangho"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["EDate"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["AdminPass"].ToString().Trim() + "{}";
                }

                dt.Dispose();
                dt = null;

                if (strNewData == "")
                {
                    ComFunc.MsgBox("라이선스 정보가 없습니다.");
                    return;
                }

                if (VB.Pstr(strNewData, "{}", 3) != "")
                {
                    strEndDate = VB.Pstr(strNewData, "{}", 3);
                    strEndDate = VB.Pstr(strEndDate, "-", 1) + VB.Pstr(strEndDate, "-", 2) + VB.Pstr(strEndDate, "-", 3);

                    if (VB.Val(strEndDate) < VB.Val(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        ComFunc.MsgBox("라이선스 만기일이 경과되어 종료됩니다.");
                        return;
                    }
                }

                strPcData = clsAES.AES(strNewData);
                System.IO.File.WriteAllText(@"C:\Windows\System32\acledit392io87.dll", strPcData);

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("라이선스 등록 완료");

                this.Close();

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

        private void HS_SETUP_Load(object sender, EventArgs e)
        {

        }
    }
}
