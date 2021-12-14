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
using MySql.Data.MySqlClient;
using System.IO;
using static System.IO.Directory;
using System.Diagnostics;

namespace HcAdmin
{
    public partial class FrmUpload : Form
    {
        public FrmUpload()
        {
            InitializeComponent();
            lblMsg.Text = "";
            txtNewVer.Text = "";
            READ_VerInfo_Server();

            string strVerPath = @"C:\Kosha\08.HPC\HS_OSHA\bin\Release\VerInfo.txt";
            if (System.IO.File.Exists(strVerPath) == true) txtNewVer.Text = System.IO.File.ReadAllText(strVerPath);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (txtOldVer.Text==txtNewVer.Text)
            {
                ComFunc.MsgBox("버전정보가 변경되지 않았습니다.", "오류");
                return;
            }
            if (txtNewVer.Text.Trim()=="")
            {
                ComFunc.MsgBox("버전정보가 공란입니다.", "오류");
                return;
            }

            //버전정보를 PC에 저장
            string strVerPath = @"C:\Kosha\08.HPC\HS_OSHA\bin\Release\VerInfo.txt";
            System.IO.File.WriteAllText(strVerPath,txtNewVer.Text.Trim());

            //업데이트 파일생성
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\Kosha\Setup\InstallFactory 2.70\InstFact.exe";
            startInfo.Arguments = null;
            Process.Start(startInfo);

            btnFileSend.Enabled = true;

        }
        // 서버에서 버전정보를 읽음 //
        private void READ_VerInfo_Server()
        {
            string SQL = "";
            string strVerInfo = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                Cursor.Current = Cursors.Default;
                return;
            }

            try
            {
                SQL = "SELECT Remark FROM LICMSG ";
                SQL += " WHERE Gubun='1' ";
                dt = clsDbMySql.GetDataTable(SQL);

                strVerInfo = "";
                if (dt.Rows.Count > 0) strVerInfo = dt.Rows[0]["Remark"].ToString().Trim();

                dt.Dispose();
                dt = null;

                txtOldVer.Text = "Ver=1.0.0;";
                if (strVerInfo != "") txtOldVer.Text = strVerInfo;
                Cursor.Current = Cursors.Default;
                return;
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
                return;
            }
        }

        // 서버에 버전정보를 저장 //
        private bool UPDATE_VerInfo()
        {
            string SQL = string.Empty;
            bool SqlErr;

            Cursor.Current = Cursors.WaitCursor;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                Cursor.Current = Cursors.Default;
                return false;
            }

            try
            {
                SQL =  "UPDATE LICMSG SET Remark='" + txtNewVer.Text.Trim() + "' ";
                SQL += " WHERE Gubun='1' ";
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);
                if (SqlErr == false)
                {
                    ComFunc.MsgBox("저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void FrmUpload_Load(object sender, EventArgs e)
        {

        }

        private void btnFileSend_Click(object sender, EventArgs e)
        {
            string strLocalFile = @"C:\Kosha\Setup\Update\HsMainUpdate.exe";

            //파일이 있는지 점검
            FileInfo fileInfo = new FileInfo(strLocalFile);
            if (fileInfo.Exists==false)
            {
                ComFunc.MsgBox("업데이트 파일이 없습니다.", "오류");
                return;
            }

            lblMsg.Text = "업데이트 서버로 파일 전송";

            Ftpedt ftp = new Ftpedt();
            if (ftp.FtpConnetBatch("115.68.23.223", "dhson", "@thsehdgml#") == false)
            {
                ComFunc.MsgBox("서버에 접속이 불가능합니다.", "알림");
                return;
            }

            string strLocalPath = @"C:\Kosha\Setup\Update";
            string strFileNm = "HsMainUpdate.exe";
            string strServerPath = "/update";
            if (ftp.FtpUploadBatch(strLocalPath, strFileNm, strServerPath, true) == false)
            {
                ComFunc.MsgBox("업데이트 파일 전송 실패", "알림");
                return;
            }
            ftp.FtpDisConnetBatch();

            // 버전정보를 서버에 저장
            if (UPDATE_VerInfo() == false) return;

            ComFunc.MsgBox("전송 완료", "알림");
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {

        }
    }
}
