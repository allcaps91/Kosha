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
    public partial class FrmExcelUpload : Form
    {
        private string FstrFiles = "";
        private int FnFiles = 0;
        private string FstrPath = "";
        private string FstrLic = "";

        public FrmExcelUpload()
        {
            InitializeComponent();

            FstrFiles = "견적서.xlsx{}기업건강증진지수.xls{}업무적합성평가양식.xlsx{}일정공문양식.xlsx";
            FnFiles = 4;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                ComFunc.MsgBox("라이선스 서버에 접속할 수 없습니다");
                Application.Exit();
            }

            List_Search();
            Folder_Combo_Set();
        }

        // 라이선스 발급 목록을 표시
        private void List_Search()
        {
            string SQL = "";
            DataTable dt = null;
            int i = 0;
            string strList = "";

            listLic.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "      LicNo, Sangho, SDATE ";
                SQL = SQL + ComNum.VBLF + " FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where EDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Sangho ";
                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strList = dt.Rows[i]["LicNo"].ToString().Trim() + " ";
                        strList += dt.Rows[i]["Sangho"].ToString().Trim();
                        listLic.Items.Add(strList);
                    }
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

        // C:\헬스소프트\0.SETUP\3.회사별엑셀서식 폴더 목록를 설정
        private void Folder_Combo_Set()
        {
            cboFolder.Items.Clear();

            string path = @"C:\\헬스소프트\\0.SETUP\\3.회사별엑셀서식";
            DirectoryInfo Info = new DirectoryInfo(path);
            if (Info.Exists)
            {
                DirectoryInfo[] CInfo = Info.GetDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo info in CInfo)
                {
                    cboFolder.Items.Add(info.Name);
                }
                cboFolder.SelectedIndex = 0;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string strFile = "";
            string strServerPath = "";

            if (listLic.Text.Trim() == "")
            {
                ComFunc.MsgBox("서버로 전송할 업체를 선택 안함", "오류");
                return;
            }

            if (cboFolder.Text.Trim() == "")
            {
                ComFunc.MsgBox("PC의 폴더를 선택 안함", "오류");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                FstrLic = VB.Pstr(listLic.Text.Trim(), " ", 1); //서버의 폴더명(라이선스 번호)
                FstrPath = @"C:\\헬스소프트\\0.SETUP\\3.회사별엑셀서식\\" + cboFolder.Text.Trim() + "\\"; //PC의 폴더명
                strServerPath = "/excelFiles/" + FstrLic + "/";

                using (Ftpedt FtpedtX = new Ftpedt())
                {
                    FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx("115.68.23.223", "dhson", "@thsehdgml#");
                    if (FtpedtX.FtpConBatchEx == null)
                    {
                        FtpedtX.Dispose();
                        return;
                    }
                    string directoryX = FtpedtX.FtpConBatchEx.ServerDirectory;

                    //서버에 폴더가 없으면 생성
                    if (FtpedtX.FtpConBatchEx.DirectoryExists("/excelFiles/") == false) FtpedtX.FtpConBatchEx.CreateDirectory("/excelFiles/");
                    if (FtpedtX.FtpConBatchEx.DirectoryExists(strServerPath) == false) FtpedtX.FtpConBatchEx.CreateDirectory(strServerPath);

                    for (int i = 1; i <= FnFiles; i++)
                    {
                        strFile = VB.Pstr(FstrFiles, "{}", i).Trim();
                        if (strFile != "")
                        {
                            //파일이 있는지 점검
                            FileInfo fileInfo = new FileInfo(FstrPath + strFile);
                            if (fileInfo.Exists == true)
                            {
                                bool blnUp = FtpedtX.FtpUploadBatchEx(FtpedtX.FtpConBatchEx, FstrPath + strFile, strFile, strServerPath); //파일업로드
                                if (blnUp == false)
                                {
                                    ComFunc.MsgBox(strFile + " 서버에 전송 실패", "오류");
                                    return;
                                }
                            }
                        }
                    }
                    update_excel_version(FstrLic);
                    ComFunc.MsgBox("서버에 전송 완료", "성공");
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void update_excel_version(string strLic)
        {
            string SQL = string.Empty;
            string strVer = DateTime.Now.ToString("yyyyMMddmmss");

            bool SqlErr;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE LICMST ";
                SQL += ComNum.VBLF + "    SET ExcelVer = '" + strVer + "' ";
                SQL += ComNum.VBLF + "  WHERE LicNo = '" + strLic + "' ";
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("버전 정보 저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }
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
