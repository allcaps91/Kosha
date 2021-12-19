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
using System.Diagnostics;

namespace HcAdmin
{
    public partial class FrmUpload : Form
    {
        bool FbNew = false;

        public FrmUpload()
        {
            InitializeComponent();
            lblMsg.Text = "";
            txtNewVer.Text = "";
            READ_VerInfo_Server();

            string strVerPath = @"C:\Kosha\08.HPC\HS_OSHA\bin\Release\VerInfo.txt";
            if (System.IO.File.Exists(strVerPath) == true) txtNewVer.Text = System.IO.File.ReadAllText(strVerPath);
            txtNewVer.Text = VB.Pstr(txtNewVer.Text, ";", 1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {

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

                if (dt.Rows.Count == 0)
                {
                    FbNew = true;
                }
                else
                {
                    strVerInfo = dt.Rows[0]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                txtOldVer.Text = "Ver=1.0.0";
                if (strVerInfo != "") txtOldVer.Text = VB.Pstr(strVerInfo, ";", 1);
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

        private void FrmUpload_Load(object sender, EventArgs e)
        {

        }

        private void lblMsg_Click(object sender, EventArgs e)
        {

        }

        private void btnFileCopy_Click(object sender, EventArgs e)
        {
            if (txtNewVer.Text.Trim() == "")
            {
                ComFunc.MsgBox("버전정보가 공란입니다.", "오류");
                return;
            }
            if (VB.L(txtNewVer.Text.Trim(),".")!=3 || VB.Left(txtNewVer.Text,4)!="Ver=")
            {
                ComFunc.MsgBox("신규 버전정보 형식 오류입니다.", "오류");
                return;
            }

            lblMsg.Text = "파일을 복사 중";

            // 1. 업데이트 목록 폴더의 기존 내용은 삭제
            DirectoryInfo d1 = new DirectoryInfo(@"C:\헬스소프트\UpdateFiles");
            FileInfo[] files = d1.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            // 2. 최근 1주일이내 변경된 파일만 Update Files 목록에 복사
            DateTime strGdate = DateTime.Now.AddDays(-7);
            string strCopyPath = @"C:\헬스소프트\UpdateFiles\";
            DirectoryInfo d2 = new DirectoryInfo(@"C:\Kosha\08.HPC\HS_OSHA\bin\Release");
            FileInfo[] files2 = d2.GetFiles();
            foreach (FileInfo file in files2)
            {
                if (file.LastWriteTime >= strGdate)
                {
                    file.CopyTo(strCopyPath + file.Name, true);
                }
            }

            // 3. 버전정보 파일을 생성
            string strVerPath = @"C:\헬스소프트\UpdateFiles\VerInfo.txt";
            System.IO.File.WriteAllText(strVerPath, txtNewVer.Text.Trim());

            strVerPath = @"C:\Kosha\08.HPC\HS_OSHA\bin\Release\VerInfo.txt";
            System.IO.File.WriteAllText(strVerPath, txtNewVer.Text.Trim());

            lblMsg.Text = "파일을 복사 완료";
            ComFunc.MsgBox("파일 복사 완료", "확인");
        }

        private void btnSetupFile_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "업데이트 설치파일 생성 중";

            //업데이트 파일생성
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\헬스소프트\InstallFactory 2.70\InstFact.exe";
            startInfo.Arguments = null;
            Process.Start(startInfo);

            lblMsg.Text = "업데이트 설치파일 생성 완료";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string strLocalFile = @"C:\헬스소프트\HsMainUpdate.exe";

            //파일이 있는지 점검
            FileInfo fileInfo = new FileInfo(strLocalFile);
            if (fileInfo.Exists == false)
            {
                ComFunc.MsgBox("업데이트 파일이 없습니다.", "오류");
                return;
            }

            // 버전정보를 서버에 저장
            if (UPDATE_VerInfo() == false) return;

            lblMsg.Text = "업데이트 설치파일 전송 중";

            // 설치파일을 서버로 전송
            if (Server_Send() == false)
            {
                lblMsg.Text = "";
                ComFunc.MsgBox("파일 서버로 전송 오류", "오류");
                return;
            }

            lblMsg.Text = "업데이트 설치파일 전송 완료";
            ComFunc.MsgBox("서버 전송 완료", "확인");
        }

        // 서버에 버전정보를 저장 //
        private bool UPDATE_VerInfo()
        {
            string SQL = string.Empty;
            bool SqlErr;
            string strTime = DateTime.Now.ToString("yyyy-MM-dd mm:ss");

            Cursor.Current = Cursors.WaitCursor;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                Cursor.Current = Cursors.Default;
                return false;
            }

            try
            {
                if (FbNew == true)
                {
                    SQL = "INSERT INTO LICMSG (Gubun,Remark,EntTime) ";
                    SQL += " VALUES ('1','" + txtNewVer.Text.Trim() + "','" + strTime + "') ";
                }
                else
                {
                    SQL = "UPDATE LICMSG SET Remark='" + txtNewVer.Text.Trim() + "', ";
                    SQL = SQL + "       EntTime='" + strTime + "' ";
                    SQL = SQL + " WHERE Gubun='1' ";
                }
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

        // 업데이트 설치파일을 서버로 전송
        private bool Server_Send()
        {
            try
            {
                Ftpedt ftpedt = new Ftpedt();
                string strLocalPath = @"C:\헬스소프트\";
                string strFileNm = "HsMainUpdate.exe";
                string strServerPath = "/update";
                ftpedt.FtpUpload("115.68.23.223", "dhson", "@thsehdgml#", strLocalPath + strFileNm, strFileNm, strServerPath); //TODO 윤조연 FTP 계정 정리
                ftpedt.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
