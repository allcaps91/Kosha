using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HS_SETUP
{
    public partial class frmSetup : Form
    {
        string FstrMac = "";
        string FstrIP = "";
        string FstrUser = "";

        public frmSetup()
        {
            InitializeComponent();

            //MAC번호 보관용 파일이 있으면 삭제함
            if (System.IO.File.Exists(@"C:\HealthSoft\MacAddress.txt") == true)
            {
                System.IO.File.Delete(@"C:\HealthSoft\MacAddress.txt");
            }
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
            string strExcelVer = "";

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
                strExcelVer = "";
                if (dt.Rows.Count > 0)
                {
                    strNewData = txtLicense.Text.Trim() + "{}";
                    strNewData += dt.Rows[0]["Sangho"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["EDate"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["AdminPass"].ToString().Trim() + "{}";

                    strExcelVer = dt.Rows[0]["ExcelVer"].ToString().Trim();
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
                System.IO.File.WriteAllText(@"C:\HealthSoft\acledit392io87.dll", strPcData);

                WRITE_ETC_PCMST(); //설치내역

                //엑셀서식 다운로드
                if (strExcelVer!="")
                {
                    if (Download_ExcelFiles() == true)
                    {
                        System.IO.File.WriteAllText(@"C:\HealthSoft\엑셀서식\excelVer.dat", strExcelVer);
                    }
                }

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

        private void WRITE_ETC_PCMST()
        {
            string strWinVer = "";
            string strNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string SQL = "";
            bool SqlErr = false;

            DataTable dt = null;

            SetComputerInfo();
            strWinVer = Get_Window_Version();

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false) return;

            try
            {
                SQL = "SELECT MacAddr FROM ETC_PCMST ";
                SQL = SQL + "WHERE MacAddr='" + FstrMac + "' ";
                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count == 0)
                {
                    SQL = "INSERT INTO ETC_PCMST (MacAddr,IpAddr,PcUser,WinVer,SWLICENSE,FirstTime,LastTime) ";
                    SQL = SQL + "VALUES ('" + FstrMac + "','" + FstrIP + "','" + FstrUser + "','" + strWinVer + "','";
                    SQL = SQL + txtLicense.Text.Trim() + "','" + strNow + "','" + strNow + "') ";
                }
                else
                {
                    SQL = "UPDATE ETC_PCMST SET ";
                    SQL = SQL + " IpAddr='" + FstrIP + "',";
                    SQL = SQL + " PcUser='" + FstrUser + "',";
                    SQL = SQL + " WinVer='" + strWinVer + "',";
                    SQL = SQL + " SWLICENSE='" + txtLicense.Text.Trim() + "',";
                    SQL = SQL + " LastTime='" + strNow + "' ";
                    SQL = SQL + "WHERE MacAddr='" + FstrMac + "' ";
                }
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        private void HS_SETUP_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 컴퓨터 정보를 변수에 할당한다
        /// </summary>
        private void SetComputerInfo()
        {
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            for (int i = 0; i < ip.AddressList.Length; i++)
            {
                if (ip.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    FstrIP = ip.AddressList[i].ToString();
                }
            }
            FstrUser = SystemInformation.ComputerName;
            FstrMac = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        }

        private string Get_Window_Version()
        {
            OperatingSystem os = Environment.OSVersion;
            Version v = os.Version;

            if (5 == v.Major && v.Minor > 0)
            {
                return "Windows XP";
            }

            else if (6 == v.Major && v.Minor == 0)
            {
                return "Windows VISTA";
            }

            else if (6 == v.Major && v.Minor == 1)
            {
                return "Windows 7";
            }
            else if (6 == v.Major && v.Minor == 2)
            {
                return "Windows 10";
            }
            else
            {
                return Environment.OSVersion.ToString();
            }
        }

        private bool Download_ExcelFiles()
        {
            string strList = "견적서.xlsx{}기업건강증진지수.xls{}업무적합성평가양식.xlsx{}일정공문양식.xlsx";
            string strPcPath = @"C:\HealthSoft\엑셀서식\";
            string strFile = "";
            string strServerPath = "";

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                strServerPath = "/excelFiles/" + txtLicense.Text.Trim() + "/";

                using (Ftpedt FtpedtX = new Ftpedt())
                {
                    FtpedtX.FtpConBatchEx = FtpedtX.FtpConnetBatchEx("115.68.23.223", "dhson", "@thsehdgml#");
                    if (FtpedtX.FtpConBatchEx == null)
                    {
                        FtpedtX.Dispose();
                        return false;
                    }

                    for (int i = 1; i <= 4; i++)
                    {
                        strFile = VB.Pstr(strList, "{}", i).Trim();
                        if (strFile != "")
                        {
                            //파일이 있으면 삭제 후 다운로드
                            if (File.Exists(strPcPath + strFile) == true) File.Delete(strPcPath + strFile);
                            bool blnUp = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx, strPcPath + strFile, strFile, strServerPath); //파일업로드
                            if (blnUp == false)
                            {
                                ComFunc.MsgBox(strFile + " 다운로드 실패", "오류");
                                FtpedtX.Dispose();
                                return false;
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

    }
}
