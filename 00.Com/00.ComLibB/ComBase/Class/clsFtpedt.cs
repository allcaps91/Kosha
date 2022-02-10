using ComDbB; //DB연결
using EnterpriseDT.Net.Ftp;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace ComBase
{
    /// <summary>
    /// Description : FTP 
    /// Author : 박웅규
    /// Create Date : 2017.07.19
    /// 참조 : C:\HealthSoft\exenet\edtFTPnet.dll
    /// N:\차세대 의료정보시스템\4-0 개발단계\4-5 개발 툴\HealthSoft\exenet\edtFTPnet.dll
    /// </summary>
    /// <history>
    /// </history>
    public class Ftpedt : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            //  관리되는 리소스 해지
            if (disposing)
            {
                if(FtpConBatch != null)
                {
                    FtpConBatch.Dispose();
                    FtpConBatch = null;
                }

                if(FtpConBatchEx != null)
                {
                    FtpConBatchEx.Dispose();
                    FtpConBatchEx = null;
                }
            }

            base.Dispose(disposing);
        }

        #region //기타 프로그램 업로드 다운로드 삭제

        //Ftpedt FtpedtX = new Ftpedt();
        //bool ftpCon = FtpedtX.FtpConnetBatch("192.168.100.35", "pcnfs", "pcnfs1");
        //if (ftpCon == true)
        //{
        //    FtpedtX.FtpUploadBatch(@"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet"); //파일업로드
        //    FtpedtX.FtpDownloadBatch(@"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet"); //파일다운로드
        //    FtpedtX.FtpDisConnetBatch();
        //}
        //FtpedtX = null;

        public FTPConnection FtpConBatch = null;
        public FTPConnection FtpConBatchEx = null;

        /// <summary>
        /// 파일서버연결(기타)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="intPort">서버 Port</param>
        /// <returns>true/false</returns>
        public bool FtpConnetBatch(string strServerIp, string strUser, string strPasswd, int intPort = 21)
        {
            FtpConBatch = new FTPConnection();

            try
            {
                FtpConBatch.ServerAddress = strServerIp;
                FtpConBatch.ServerPort = intPort;
                FtpConBatch.UserName = strUser;
                FtpConBatch.Password = strPasswd;
                FtpConBatch.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                FtpConBatch.Connect();

                return true;
            }
            catch (Exception ex)
            {
                if (FtpConBatch.IsConnected) FtpConBatch.Close();
                FtpConBatch = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일서버연결(다수의 파일을 핸들링할 경우 접속)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="strServerIp"></param>
        /// <param name="strUser"></param>
        /// <param name="strPasswd"></param>
        /// <param name="intPort"></param>
        /// <returns></returns>
        public FTPConnection FtpConnetBatchEx(string strServerIp, string strUser, string strPasswd, int intPort = 21)
        {
            FTPConnection rtnVal = new FTPConnection();

            try
            {
                rtnVal.ServerAddress = strServerIp;
                rtnVal.ServerPort = intPort;
                rtnVal.UserName = strUser;
                rtnVal.Password = strPasswd;
                rtnVal.CommandEncoding = Encoding.GetEncoding("EUC-KR");

                //rtnVal.TransferBufferSize = 524288;
                //rtnVal.TransferBufferSize = 131072;

                rtnVal.Connect();

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (rtnVal.IsConnected) rtnVal.Close();
                rtnVal = null;
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 파일서버연결해제(기타)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        public void FtpDisConnetBatch()
        {
            if (FtpConBatch.IsConnected) FtpConBatch.Close();
            FtpConBatch = null;
        }

        /// <summary>
        /// /// <summary>
        /// 파일서버 접속 해제(다수의 파일을 핸들링할 경우 접속)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// </summary>
        /// <param name="pFtpCont"></param>
        public void FtpDisConnetBatchEx(FTPConnection pFtpCont)
        {
            if (pFtpCont.IsConnected) pFtpCont.Close();
            pFtpCont = null;
        }

        /// <summary>
        /// 파일업로드(기타)
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpUploadBatch(string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            try
            {
                string directoryX = FtpConBatch.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    FtpConBatch.ChangeWorkingDirectory(directory);
                    directoryX = FtpConBatch.ServerDirectory;

                    if (blnReplace == true)
                    {
                        FtpConBatch.UploadFile(strLocalFilePath, strRemoteFileNm);
                    }
                    else
                    {
                        if (FtpConBatch.Exists(strRemoteFileNm) == true)
                        {
                            ComFunc.MsgBox("파일이 존재합니다.");
                            return false;
                        }
                        else
                        {
                            FtpConBatch.UploadFile(strLocalFilePath, strRemoteFileNm);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //if (FtpConEtc.IsConnected) FtpConEtc.Close();
                //FtpConEtc = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일 다운로드 배치작업(다수의 파일을 핸들링할 경우 접속)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="pFtpCont"></param>
        /// <param name="strLocalFilePath"></param>
        /// <param name="strRemoteFileNm"></param>
        /// <param name="strServerPath"></param>
        /// <param name="blnReplace"></param>
        /// <returns></returns>
        public bool FtpUploadBatchEx(FTPConnection pFtpCont, string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            try
            {
                string directoryX = pFtpCont.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    pFtpCont.ChangeWorkingDirectory(directory);
                    directoryX = pFtpCont.ServerDirectory;

                    if (blnReplace == true)
                    {
                        pFtpCont.UploadFile(strLocalFilePath, strRemoteFileNm);
                    }
                    else
                    {
                        if (pFtpCont.Exists(strRemoteFileNm) == true)
                        {
                            ComFunc.MsgBox("파일이 존재합니다.");
                            return false;
                        }
                        else
                        {
                            pFtpCont.UploadFile(strLocalFilePath, strRemoteFileNm);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //if (FtpConEtc.IsConnected) FtpConEtc.Close();
                //FtpConEtc = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일업로드 - 경로 없을시 생성
        /// Author : 박웅규
        /// Create Date : 2020.09.08
        /// </summary>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpUploadBatchEx2(string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            try
            {
                string directoryX = FtpConBatch.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    if (directory.Length > 0)
                    {
                        string DayDir = directory.IndexOf("/data/ocs_etc/") != -1 ? directory.Substring(0, 22) : directory.Substring(0, 20);
                        if (FtpConBatch.DirectoryExists(DayDir) == false) FtpConBatch.CreateDirectory(DayDir);
                    }
                    
                    if (FtpConBatch.DirectoryExists(directory) == false) FtpConBatch.CreateDirectory(directory);

                    FtpConBatch.ChangeWorkingDirectory(directory);
                    directoryX = FtpConBatch.ServerDirectory;

                    if (blnReplace == true)
                    {
                        FtpConBatch.UploadFile(strLocalFilePath, strRemoteFileNm);
                    }
                    else
                    {
                        if (FtpConBatch.Exists(strRemoteFileNm) == true)
                        {
                            //ComFunc.MsgBox("파일이 존재합니다.");
                            return false;
                        }
                        else
                        {
                            FtpConBatch.UploadFile(strLocalFilePath, strRemoteFileNm);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //if (FtpConEtc.IsConnected) FtpConEtc.Close();
                //FtpConEtc = null;
                //ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일다운로드(기타)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpDownloadBatch(string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            if (blnReplace == false)
            {
                if (File.Exists(strLocalFilePath) == true)
                {
                    ComFunc.MsgBox("파일이 존재합니다.");
                    return false;
                }
            }

            if (FtpConBatch.IsConnected == false)
            {
                ComFunc.MsgBox("FTP서버에 접속되어 있지 않습니다.");
                return false;
            }

            try
            {
                string directoryX = FtpConBatch.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    FtpConBatch.ChangeWorkingDirectory(directory);
                    directoryX = FtpConBatch.ServerDirectory;

                    FtpConBatch.DownloadFile(strLocalFilePath, strRemoteFileNm);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (FtpConBatch.IsConnected) FtpConBatch.Close();
                FtpConBatch = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일다운로드(기타) - 오류 메시지 안뛰우게
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpDownloadBatch2(string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            if (blnReplace == false)
            {
                if (File.Exists(strLocalFilePath) == true)
                {
                    ComFunc.MsgBox("파일이 존재합니다.");
                    return false;
                }
            }

            if (FtpConBatch.IsConnected == false)
            {
                ComFunc.MsgBox("FTP서버에 접속되어 있지 않습니다.");
                return false;
            }

            try
            {
                string directoryX = FtpConBatch.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    FtpConBatch.ChangeWorkingDirectory(directory);
                    directoryX = FtpConBatch.ServerDirectory;

                    FtpConBatch.DownloadFile(strLocalFilePath, strRemoteFileNm);
                }
                return true;
            }
            catch (Exception ex)
            {
                //if (FtpConBatch.IsConnected) FtpConBatch.Close();
                //FtpConBatch = null;
                //ComFunc.MsgBox(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 파일다운로드(기타)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="pFtpCont"></param>
        /// <param name="strLocalFilePath"></param>
        /// <param name="strRemoteFileNm"></param>
        /// <param name="strServerPath"></param>
        /// <param name="blnReplace"></param>
        /// <returns></returns>
        public bool FtpDownloadBatchEx(FTPConnection pFtpCont, string strLocalFilePath, string strRemoteFileNm, string strServerPath, bool blnReplace = true)
        {
            if (blnReplace == false)
            {
                if (File.Exists(strLocalFilePath) == true)
                {
                    ComFunc.MsgBox("파일이 존재합니다.");
                    return false;
                }
            }

            if (pFtpCont.IsConnected == false)
            {
                ComFunc.MsgBox("FTP서버에 접속되어 있지 않습니다.");
                return false;
            }

            try
            {
                string directoryX = pFtpCont.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    pFtpCont.ChangeWorkingDirectory(directory);
                    directoryX = pFtpCont.ServerDirectory;

                    //if (pFtpCont.FileInfoParser.ParsingCulture)

                    pFtpCont.DownloadFile(strLocalFilePath, strRemoteFileNm);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("(code=550)") == -1 && ex.Message.IndexOf("프로세스") == -1)
                {
                    if (pFtpCont.IsConnected) pFtpCont.Close();
                    pFtpCont = null;
                    ComFunc.MsgBox(ex.Message);
                }
                return false;
            }
        }

        /// <summary>
        /// 파일삭제(기타)
        /// Author : 박웅규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <param name="pFtpCont"></param>
        /// <param name="strLocalFilePath"></param>
        /// <param name="strRemoteFileNm"></param>
        /// <param name="strServerPath"></param>
        /// <param name="blnReplace"></param>
        /// <returns></returns>
        public bool FtpFileDeleteBatch(FTPConnection pFtpCont, string strRemoteFileNm, string strServerPath)
        {
            if (pFtpCont.IsConnected == false)
            {
                ComFunc.MsgBox("FTP서버에 접속되어 있지 않습니다.");
                return false;
            }

            try
            {
                string directoryX = pFtpCont.ServerDirectory;
                string directory = strServerPath;

                pFtpCont.ChangeWorkingDirectory(directory);
                directoryX = pFtpCont.ServerDirectory;

                if (pFtpCont.Exists(strRemoteFileNm) == true)
                {
                    if (pFtpCont.DeleteFile(strRemoteFileNm) == false)
                    {
                        //return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("(code=550)") == -1)
                {
                    if (pFtpCont.IsConnected) pFtpCont.Close();
                    pFtpCont = null;
                    ComFunc.MsgBox(ex.Message);
                }
                return false;
            }
        }


        #endregion //기타 프로그램 업로드 다운로드 삭제

        #region //파일 하나만 업로드 다운로드 할 경우
        /// <summary>
        /// 파일업로드
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="intPort">서버 Port</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpUpload(string strServerIp, string strUser, string strPasswd,
                                string strLocalFilePath, string strRemoteFileNm, string strServerPath,
                                int intPort = 21, bool blnReplace = true)
        {
            //Ftpedt FtpedtX = new Ftpedt();
            //FtpedtX.FtpUpload("192.168.100.35", "pcnfs", "pcnfs1", @"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet");
            //FtpedtX = null;
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    ftpConnection1.ChangeWorkingDirectory(directory);
                    directoryX = ftpConnection1.ServerDirectory;

                    if (blnReplace == true)
                    {
                        ftpConnection1.UploadFile(strLocalFilePath, strRemoteFileNm);
                    }
                    else
                    {
                        if (ftpConnection1.Exists(strRemoteFileNm) == true)
                        {
                            ComFunc.MsgBox("파일이 존재합니다.");
                        }
                        else
                        {
                            ftpConnection1.UploadFile(strLocalFilePath, strRemoteFileNm);
                        }
                    }
                }
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        public bool FtpUploadEx(string strServerIp, string strUser, string strPasswd,
                                string strLocalFilePath, string strRemoteFileNm, string strServerPath,
                                string strIMGPATH, string strITEMVALUE, int intPort = 21, bool blnReplace = true)
        {
            //Ftpedt FtpedtX = new Ftpedt();
            //FtpedtX.FtpUpload("192.168.100.35", "pcnfs", "pcnfs1", @"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet");
            //FtpedtX = null;
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                ftpConnection1.ChangeWorkingDirectory(directory);
                //폴더를 만들고
                if (ftpConnection1.DirectoryExists(strIMGPATH) == false) ftpConnection1.CreateDirectory(strIMGPATH);

                ftpConnection1.ChangeWorkingDirectory(strIMGPATH);

                //if (ftpConnection1.DirectoryExists(strIMGPATH2) == false) ftpConnection1.CreateDirectory(strIMGPATH2);
                //ftpConnection1.ChangeWorkingDirectory(strIMGPATH2);

                //파일 업로드
                ftpConnection1.UploadFile(strLocalFilePath + "\\" + strITEMVALUE + ".jpg", strITEMVALUE + ".jpg");
                if (File.Exists(strLocalFilePath + "\\" + strITEMVALUE + ".dgr") == true)
                {
                    ftpConnection1.UploadFile(strLocalFilePath + "\\" + strITEMVALUE + ".dgr", strITEMVALUE + ".dgr");
                }

                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        /// <summary>
        /// 파일다운로드
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="intPort">서버 Port</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpDownload(string strServerIp, string strUser, string strPasswd,
                                string strLocalFilePath, string strRemoteFileNm, string strServerPath,
                                int intPort = 21, bool blnReplace = true)
        {
            //Ftpedt FtpedtX = new Ftpedt();
            //FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", @"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet");
            //FtpedtX = null;
            if (blnReplace == false)
            {
                if (File.Exists(strLocalFilePath) == true)
                {
                    ComFunc.MsgBox("파일이 존재합니다.");
                    return false;
                }
            }
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    ftpConnection1.ChangeWorkingDirectory(directory);
                    directoryX = ftpConnection1.ServerDirectory;

                    if (ftpConnection1.Exists(strRemoteFileNm) == false)
                    {
                        ftpConnection1 = null;
                        return false;
                    }

                    ftpConnection1.DownloadFile(strLocalFilePath, strRemoteFileNm);
                }
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        /// <summary>
        /// 파일다운로드
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="strLocalFilePath">파일전체 경로</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="intPort">서버 Port</param>
        /// <param name="blnReplace">덮어쓰기여부</param>
        /// <returns></returns>
        public bool FtpDownloadEx(string strServerIp, string strUser, string strPasswd,
                                string strLocalFilePath, string strRemoteFileNm, string strServerPath,
                                int intPort = 21, bool blnReplace = true)
        {
            //Ftpedt FtpedtX = new Ftpedt();
            //FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", @"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet");
            //FtpedtX = null;
            if (blnReplace == false)
            {
                if (File.Exists(strLocalFilePath) == true)
                {
                    ComFunc.MsgBox("파일이 존재합니다.");
                    return false;
                }
            }
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                if (strLocalFilePath.Trim() != "")
                {
                    ftpConnection1.ChangeWorkingDirectory(directory);
                    directoryX = ftpConnection1.ServerDirectory;

                    ftpConnection1.DownloadFile(strLocalFilePath, strRemoteFileNm);
                }
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return false;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        /// <summary>
        /// 파일다운로드
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="intPort">서버 Port</param>
        /// <returns></returns>
        public bool FtpDeleteFile(string strServerIp, string strUser, string strPasswd,
                                string strRemoteFileNm, string strServerPath,
                                int intPort = 21)
        {
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                ftpConnection1.ChangeWorkingDirectory(directory);
                directoryX = ftpConnection1.ServerDirectory;

                if (ftpConnection1.Exists(strRemoteFileNm) == true)
                {
                    ftpConnection1.DeleteFile(strRemoteFileNm);
                }

                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        /// <summary>
        /// 파일다운로드
        /// Author : 박웅규
        /// Create Date : 2017.07.19
        /// </summary>
        /// <param name="strServerIp">서버 IP</param>
        /// <param name="strUser">사용자</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <param name="strRemoteFileNm">서버에 저장할 파일명</param>
        /// <param name="strServerPath">서버경로</param>
        /// <param name="intPort">서버 Port</param>
        /// <returns></returns>
        public Stream DownloadStream(string strServerIp, string strUser, string strPasswd,
                                string strRemoteFileNm, string strServerPath,
                                int intPort = 21)
        {
            //Ftpedt FtpedtX = new Ftpedt();
            //FtpedtX.FtpDownload("192.168.100.35", "pcnfs", "pcnfs1", @"C:\HealthSoft\exenet\edtFTPnet.dll",  "edtFTPnet.dll", "/psnfs/HealthSoft/exenet");
            //FtpedtX = null;

            Stream strFileStream = null;
            FTPConnection ftpConnection1 = new FTPConnection();

            try
            {
                ftpConnection1.ServerAddress = strServerIp;
                ftpConnection1.ServerPort = intPort;
                ftpConnection1.UserName = strUser;
                ftpConnection1.Password = strPasswd;
                ftpConnection1.CommandEncoding = Encoding.GetEncoding("EUC-KR");
                ftpConnection1.Connect();
                string directoryX = ftpConnection1.ServerDirectory;
                string directory = strServerPath;

                ftpConnection1.ChangeWorkingDirectory(directory);
                directoryX = ftpConnection1.ServerDirectory;

                ftpConnection1.DownloadStream(strFileStream, strRemoteFileNm);
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                return strFileStream;
            }
            catch (Exception ex)
            {
                if (ftpConnection1.IsConnected) ftpConnection1.Close();
                ftpConnection1 = null;
                ComFunc.MsgBox(ex.Message);
                return null;
            }
            finally
            {
                if (ftpConnection1 != null)
                {
                    if (ftpConnection1.IsConnected) ftpConnection1.Close();
                    ftpConnection1 = null;
                }
            }
        }

        #endregion //파일 하나만 업로드 다운로드 할 경우


        /// <summary>
        /// 서버 경로를 받아온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strIP"></param>
        /// <param name="strUser"></param>
        /// <returns></returns>
        public string READ_FTP(PsmhDb pDbCon, string strIP, string strUser)
        {
            string strVal = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT USERPASS FROM ADMIN.BAS_ACCOUNT_SERVER      ";
                SQL = SQL + ComNum.VBLF + "WHERE IP = '"+ strIP + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND USERID = '"+ strUser + "'        ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE DESC      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strVal = clsAES.DeAES(dt.Rows[0]["USERPASS"].ToString().Trim());
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            return strVal ;
        }


    }
}
