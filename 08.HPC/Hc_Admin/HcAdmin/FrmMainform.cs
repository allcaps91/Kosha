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
using System.IO;
using System.Diagnostics;

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

                clsDB.GetDbInfo();
                clsDB.DbCon = clsDB.DBConnect_Cloud();

                clsType.User.Sabun = "1";
                clsType.User.IdNumber = "1";
                clsType.User.JobName = "관리자";
                clsType.User.UserName = "관리자";
                clsType.User.BuseName = "OSHA";
                clsType.User.Jikmu = "YYYYYYNNNNNNNNN";
                clsType.User.LtdUser = "";
                clsType.User.PassWord = "";

                READ_Licno_Disk();
            }
            else
            {
                ComFunc.MsgBox("관리자 비빌번호를 확인하세요");
            }

        }

        private bool READ_Licno_Disk()
        {
            string strPcData = "";
            string strNewData = "";

            clsType.HosInfo.SwLicense = "";
            clsType.HosInfo.SwLicInfo = "";

            //파일형식: 라이선스번호{}회사명{}종료일자{}관리자비번{}
            string strLicFile = @"C:\HealthSoft\acledit392io87.dll";
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

        private void CmdExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 안내문등록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm안내문등록 form = new Frm안내문등록();
            form.Show();
        }

        private void 서버업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 설치파일만들기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //필요없는 파일 삭제
            DirectoryInfo d1 = new DirectoryInfo(@"C:\헬스소프트\Debug");
            FileInfo[] files = d1.GetFiles();
            foreach (FileInfo file in files)
            {
                if (VB.Right(file.Name.ToLower(), 11) == ".dll.config") file.Delete();
                if (VB.Right(file.Name.ToLower(), 4) == ".pdb") file.Delete();
            }

            //설치파일 만들기
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\헬스소프트\0.SETUP\2.InstallFactory 2.70\InstFact.exe";
            startInfo.Arguments = null;
            Process.Start(startInfo);
        }

        private void 엑셀업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmExcelTest1 form = new FrmExcelTest1();
            form.Show();
        }

        private void 특정폴더삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strPath = @"C:\PSMHEXE";
            DirectoryInfo di = new DirectoryInfo(strPath);
            if (di.Exists == true)
            {
                di.Delete(true);
                ComFunc.MsgBox("삭제 완료");
            }
            else
            {
                ComFunc.MsgBox(strPath +" 폴더가 없습니다.");
            }
        }

        private void 싸인복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSignCopy form = new FrmSignCopy();
            form.Show();
        }

        private void 서버업로드ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //필요없는 파일 삭제
            DirectoryInfo d1 = new DirectoryInfo(@"C:\헬스소프트\Debug");
            FileInfo[] files = d1.GetFiles();
            foreach (FileInfo file in files)
            {
                //if (VB.Right(file.Name.ToLower(), 11) == ".exe.config") file.Delete();
                //if (VB.Right(file.Name.ToLower(), 13) == ".exe.manifest") file.Delete();
                if (VB.Right(file.Name.ToLower(), 11) == ".dll.config") file.Delete();
                if (VB.Right(file.Name.ToLower(), 4) == ".pdb") file.Delete();
            }

            FrmUpload form = new FrmUpload();
            form.Show();

        }

        private void 엑셀파일업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmExcelUpload form = new FrmExcelUpload();
            form.Show();
        }
    }
}
