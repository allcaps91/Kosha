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

namespace HcAdmin
{
    public partial class FrmMainform : Form
    {
        public FrmMainform()
        {
            InitializeComponent();
            닫기ToolStripMenuItem.Enabled = false;
            라이선스ToolStripMenuItem.Enabled = false;
            안내문등록ToolStripMenuItem.Enabled = false;
            if (READ_Licno_Disk() == false) { Application.Exit(); return; }
            if (READ_Licno_Server()==false) { Application.Exit(); return; }
            DoLogin_Cloud();
        }

        private void DoLogin_Cloud()
        {
            //TODO : 커넥션 교체하기전까지 막음
            clsDB.GetDbInfo();
            clsDB.DbCon = clsDB.DBConnect_Cloud();
            clsCompuInfo.SetComputerInfo();

            if (clsDB.DbCon == null)
            {
                Application.Exit();
                return;
            }

        }

        private bool READ_Licno_Disk()
        {
            string strPcData = "";
            string strNewData = "";

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
            string strEDate = "";
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

                strEDate = VB.Pstr(strNewData, "{}", 3);
                if (strEDate != "")
                {
                    if (VB.Val(VB.Format(strEDate,"yyyyMMdd")) < VB.Val(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        ComFunc.MsgBox("라이선스 만기일이 경과되어 종료됩니다.");
                        return false;
                    }
                }

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

        private void FrmMainform_Load(object sender, EventArgs e)
        {

        }

        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 라이선스ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLicense form = new FrmLicense();
            form.Show();
        }

        private void CmdLogin_Click_1(object sender, EventArgs e)
        {
            if (TxtPass.Text.Trim() == "0542894349")
            {
                닫기ToolStripMenuItem.Enabled = true;
                라이선스ToolStripMenuItem.Enabled = true;
                안내문등록ToolStripMenuItem.Enabled = true;
                panLogin.Visible = false;
            }
            else
            {
                ComFunc.MsgBox("관리자 비빌번호를 확인하세요");
            }

        }

        private void CmdExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 안내문등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm안내문등록 form = new Frm안내문등록();
            form.Show();
        }
    }
}
