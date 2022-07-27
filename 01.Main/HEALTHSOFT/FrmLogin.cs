using ComBase;
using ComBase.Mvc.Utils;
using HC_OSHA;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// 프로그램 시작 폼
/// 추후 작업환경측정 등 다른 프로젝트와 연결의 시작점
/// </summary>
namespace HEALTHSOFT
{
    public partial class FrmLogin :Form
    {
        public static string FstrLicno = "";
        public static string FstrSangho = "";
        public static string FstrPassword = "";
        public static string FstrEndDate = "";
        public static string FstrNewVer = "";
        public static string FstrOldVer = "";
        public int intDelay = 0;

        private string FstrExcelVer = "";
        private string FstrLicExcelVer = "";
        private string FstrExcelPath = @"C:\HealthSoft\엑셀서식\excelVer.dat";

        private string FstrMac = "";
        private string FstrIp = "";
        private string FstrWinVer = "";
        public FrmLogin()
        {
            InitializeComponent();
            SetEvent();
            
            //생성자에서 폼로드시 필요한 이벤트만 정의
            //폼로드 후 라이선스 및 사용자세팅 정의 eFormload 이후 ...
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLogin.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.txtIdNumber.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtPassword.KeyDown += new KeyEventHandler(eTxtKeyDown);

            //PC정보를 읽음
            clsCompuInfo.SetComputerInfo();
            FstrIp = clsCompuInfo.gstrCOMIP;

            //MAC번호가 임의로 변경이 되어 PC에 저장 후 사용함
            FstrMac = "";
            if (System.IO.File.Exists(@"C:\HealthSoft\MacAddress.txt") == true)
            {
                FstrMac = System.IO.File.ReadAllText(@"C:\HealthSoft\MacAddress.txt");
            }
            if (FstrMac == "")
            {
                FstrMac = clsCompuInfo.GetMACAddress();
                System.IO.File.WriteAllText(@"C:\HealthSoft\MacAddress.txt", FstrMac);
            }
            FstrWinVer = GetOSFriendlyName();
        }

        private string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }

            //결과에서 "microsoft"를 제거
            if (VB.Left(result, 9) == "Microsoft") result = VB.Right(result, VB.Len(result) - 9).Trim();
            return result;
        }
        private void eTimerTick(object sender, EventArgs e)
        {
            intDelay++;

            if (intDelay >= 5)
            {
                timer1.Enabled = false;

                string strFileName = @"c:\temp\HsMainUpdate.exe";
                System.Diagnostics.Process.Start(strFileName);

                //2초 대기
                Thread.Sleep(2000);

                this.Close();

            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLogin)
            {
                DoLogin();
            }
        }

        private void DoLogin()
        {
            string strUpdate = "최신 업데이트 파일이 있습니다." + "\n" + "지금 업데이트를 진행하시겠습니까?";

            FstrLicno = clsType.HosInfo.SwLicense;

            //버전정보가 틀리면 자동 업데이트
            if (FstrOldVer != FstrNewVer)
            {
                if (MessageUtil.Confirm(strUpdate) == DialogResult.Yes)
                {
                    Log_Send(FstrNewVer + " 버전으로 업데이트");
                    btnUpdate_Click();
                    return;
                }
            }


            //엑셀서식파일 버전을 읽음
            FstrExcelVer = "";
            if (System.IO.File.Exists(FstrExcelPath) == true)
            {
                FstrExcelVer = System.IO.File.ReadAllText(FstrExcelPath);
            }

            //PC와 라이선스 DB 엑셀파일 버전이 틀리면 다운로드
            if (FstrLicExcelVer != FstrExcelVer)
            {
                if (Download_ExcelFiles() == true)
                {
                    Log_Send(FstrLicExcelVer + " 버전 엑셀파일 다운로드");
                    System.IO.File.WriteAllText(FstrExcelPath, FstrLicExcelVer);
                }
            }

            if (txtIdNumber.Text.Trim() == "admin")
            {
                if (txtPassword.Text.Trim() == VB.Pstr(clsType.HosInfo.SwLicInfo, "{}", 4))
                {
                    clsType.User.Sabun = "1";
                    clsType.User.IdNumber = "1";
                    clsType.User.JobName = "관리자";
                    clsType.User.UserName = "관리자";
                    clsType.User.BuseName = "OSHA";
                    clsType.User.Jikmu = "YYYYYYNNNNNNNNN";
                    clsType.User.LtdUser = "";
                    clsType.User.PassWord = "";
                }
                else if (txtPassword.Text.Trim() == "0542894349")
                {
                    clsType.User.Sabun = "1";
                    clsType.User.IdNumber = "1";
                    clsType.User.JobName = "관리자";
                    clsType.User.UserName = "관리자";
                    clsType.User.BuseName = "OSHA";
                    clsType.User.Jikmu = "YYYYYYNNNNNNNNN";
                    clsType.User.LtdUser = "";
                    clsType.User.PassWord = "";
                }

                else
                {
                    ComFunc.MsgBox("관리자 비밀번호 오류 입니다", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            else
            {
                if (Set_UserInfo(txtIdNumber.Text.Trim(), txtPassword.Text.Trim()) == false)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            // OLD버전 작업폴더 삭제
            DirectoryInfo di = new DirectoryInfo(@"C:\PSMHEXE");
            if (di.Exists == true)
            {
                di.Delete(true);
            }

            // 최근 로그인 ID를 저장함
            Save_LastLoginID();

            //라이선스 서버에 로그정보를 전송함
            Log_Send(txtIdNumber.Text.Trim() + " 사용자 로그인");

            //라이선스 서버에 pc정보를 업데이트
            Update_PcMaster();

            // 프로그램 실행
            Dashboard form = new Dashboard();
            form.Show();
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtIdNumber || sender == txtPassword)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            if (READ_Licno_Disk() == false)
            {
                this.Close();
                return;
            }

            if (READ_Licno_Server() == false)
            {
                this.Close();
                return;
            }

            clsType.ClearUser(); //사용자 정보 초기화
            GuideMsg_Display();

            Set_LastLoginID();  //최종 로그인 ID를 표시

            clsDB.GetDbInfo();
            clsDB.DbCon = clsDB.DBConnect_Cloud();
            timer1.Enabled = false;

            this.ActiveControl = txtIdNumber;
            txtIdNumber.Focus();
        }

        private bool READ_Licno_Disk()
        {
            string strPcData = "";
            string strNewData = "";

            clsType.HosInfo.SwLicense = "";
            clsType.HosInfo.SwLicInfo = "";
            clsType.HosInfo.SMTP_Info = "";

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

                // 버전정보를 읽음
                if (System.IO.File.Exists(@"C:\HealthSoft\VerInfo.txt") == true)
                    FstrOldVer = System.IO.File.ReadAllText(@"C:\HealthSoft\VerInfo.txt");

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

            clsType.HosInfo.SMTP_Info = "";
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
                SQL = "SELECT * FROM LICMST ";
                SQL = SQL + "Where Licno = '" + clsType.HosInfo.SwLicense + "' ";
                dt = clsDbMySql.GetDataTable(SQL);

                strNewData = "";
                FstrLicExcelVer = "";
                if (dt.Rows.Count > 0)
                {
                    strNewData = clsType.HosInfo.SwLicense + "{}";
                    strNewData += dt.Rows[0]["Sangho"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["EDate"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[0]["AdminPass"].ToString().Trim() + "{}";

                    clsType.HosInfo.strNameKor = dt.Rows[0]["Sangho"].ToString().Trim();
                    clsType.HosInfo.SMTP_Info = dt.Rows[0]["SMTP_Setup"].ToString().Trim();

                    FstrLicExcelVer = dt.Rows[0]["ExcelVer"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strNewData != "")
                {
                    if (clsType.HosInfo.SwLicInfo != strNewData)
                    {
                        clsType.HosInfo.SwLicInfo = strNewData;
                        strPcData = clsAES.AES(strNewData);
                        System.IO.File.WriteAllText(@"C:\HealthSoft\acledit392io87.dll", strPcData);
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Gubun"].ToString() == "1") FstrNewVer = dt.Rows[i]["Remark"].ToString();
                    if (dt.Rows[i]["Gubun"].ToString() == "2") txtGuide.Text = dt.Rows[i]["Remark"].ToString();
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

        //라이선스 서버에 로그정보를 전송
        private void Log_Send(string argLog)
        {
            string SQL = "";
            bool SqlErr;
            string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //라이선스 서버에 로그를 전송함
                SQL = "INSERT INTO pc_log (SENDTIME,MAC,LICNO,IP,SENDLOG) ";
                SQL = SQL + "VALUES ('" + strNow + "','" + FstrMac + "','";
                SQL = SQL + FstrLicno + "','" + FstrIp + "','" + argLog + "') ";
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);
                if (SqlErr == false)
                {
                    ComFunc.MsgBox("pc_log에 로그전송 실패", "알림");
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        //라이선스 서버에 pc정보를 Update
        private void Update_PcMaster()
        {
            string SQL = "";
            bool SqlErr;
            DataTable dt = null;
            string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT * FROM pc_master ";
                SQL = SQL + "WHERE MAC='" + FstrMac + "' ";
                dt = clsDbMySql.GetDataTable(SQL);
                if (dt.Rows.Count == 0)
                {
                    SQL = "INSERT INTO pc_master (MAC,LICNO,VER,IP,STARTDATE,LASTDATE,";
                    SQL = SQL + "WINVER,REMARK,PCINFO) VALUES ('" + FstrMac + "','";
                    SQL = SQL + FstrLicno + "','" + FstrOldVer + "','" + FstrIp + "','";
                    SQL = SQL + strNow + "','" + strNow + "','" + FstrWinVer + "','','') ";
                }
                else
                {
                    SQL = "UPDATE pc_master SET ";
                    SQL = SQL + " LICNO='" + FstrLicno + "',";
                    SQL = SQL + " VER='" + FstrOldVer + "',";
                    SQL = SQL + " IP='" + FstrIp + "',";
                    SQL = SQL + " LASTDATE='" + strNow + "',";
                    SQL = SQL + " WINVER='" + FstrWinVer + "' ";
                    SQL = SQL + "WHERE MAC='" + FstrMac + "' ";
                }
                dt.Dispose();
                dt = null;

                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);
                if (SqlErr == false)
                {
                    ComFunc.MsgBox("pc_master 업데이트 실패", "알림");
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        // 최근 로그인 성공한 ID를 표시함
        private void Set_LastLoginID()
        {
            string strData = "";
            string strNewData = "";
            String strFile = @"C:\HealthSoft\HSLastLogin.dat";

            //파일형식: 사원번호{}
            if (System.IO.File.Exists(strFile) == true)
            {
                strData = System.IO.File.ReadAllText(strFile);
                strNewData = clsAES.DeAES(strData);
                if (VB.L(strNewData, "{}") == 2)
                {
                    txtIdNumber.Text = VB.Pstr(strNewData, "{}", 1);
                }
            }
        }

        // 최근 로그인 성공한 ID를 저장함
        private void Save_LastLoginID()
        {
            string strData = "";
            string strNewData = "";

            strNewData = txtIdNumber.Text.Trim() + "{}";

            strData = clsAES.AES(strNewData);
            System.IO.File.WriteAllText(@"C:\HealthSoft\HSLastLogin.dat", strData);
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
                SQL = "SELECT Name,DEPT,JIKMU,LTDUSER,PASSHASH256 FROM HIC_USERS ";
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

                if (argPassword != "0542894349!") //슈퍼비번
                {
                    strPassword = clsAES.DeAES(dt.Rows[i]["PASSHASH256"].ToString().Trim());
                    if (strPassword != argPassword)
                    {
                        dt.Dispose();
                        dt = null;

                        ComFunc.MsgBox("비밀번호가 틀립니다.", "알림");
                        return false;
                    }
                }
                clsType.User.Sabun = ArgSabun;
                clsType.User.IdNumber = ArgSabun;
                clsType.User.JobName = dt.Rows[i]["Name"].ToString().Trim();
                clsType.User.UserName = dt.Rows[i]["Name"].ToString().Trim();
                clsType.User.BuseName = dt.Rows[i]["DEPT"].ToString().Trim();
                clsType.User.Jikmu = dt.Rows[i]["JIKMU"].ToString().Trim();
                clsType.User.LtdUser = dt.Rows[i]["LTDUSER"].ToString().Trim();
                clsType.User.PassWord = strPassword;

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

        private void btnUpdate_Click()
        {
            string folderPath = @"C:\temp";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            timer1.Enabled = false;
            Ftpedt ftpedt = new Ftpedt();
            string strMsgBackup = txtGuide.Text.Trim();

            txtGuide.Text = ComNum.VBLF + "     업데이트 서버에서 파일을 다운로드 중 입니다.";
            txtGuide.Text += ComNum.VBLF + "        ( 소요시간: 약1~2분 입니다. )";

            string strLocalPath = @"c:\temp\HsMainUpdate.exe";
            string strFileNm = "HsMainUpdate.exe";
            string strServerPath = "/update";

            Ftpedt FtpedtX = new Ftpedt();
            FtpedtX.FtpDownload("115.68.23.223", "dhson", "@thsehdgml#", strLocalPath, strFileNm, strServerPath);
            FtpedtX = null;

            //테스트
            Thread.Sleep(20000);

            intDelay = 0;
            timer1.Enabled = true;

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

                strServerPath = "/excelFiles/" + clsType.HosInfo.SwLicense + "/";

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
                            bool blnUp = FtpedtX.FtpDownloadBatchEx(FtpedtX.FtpConBatchEx,strPcPath + strFile, strFile, strServerPath); //파일업로드
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

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        // 엑셀서식을 재설치
        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = false;

            //엑셀서식파일 버전을 읽음
            FstrExcelVer = "";
            if (File.Exists(FstrExcelPath) == true) File.Delete(FstrExcelPath);

            bOk = false;
            if (FstrLicExcelVer != "")
            {
                if (Download_ExcelFiles() == true)
                {
                    bOk = true;
                    System.IO.File.WriteAllText(FstrExcelPath, FstrLicExcelVer);
                    ComFunc.MsgBox("엑셀서식 파일 다운로드 완료", "성공");
                }
            }
            if (bOk==false) ComFunc.MsgBox("엑셀서식 파일 다운로드 실패", "실패");
        }
    }
}
