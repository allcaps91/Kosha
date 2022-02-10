using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;
using System.IO;

namespace ComEmrBase
{
    public class clsScanPublic
    {

        public static string gClintPath = @"C:\HealthSoft";
        public static int gPosition = 0;
        public static string gstrOCRNO= string.Empty;

        //FTP
        public static string gFtpServer = "ftp://21";
        public static string gFtpServerIP= string.Empty;
        public static string gFtpServerPort = "21";
        public static string gServerId= string.Empty;
        public static string gServerPass= string.Empty;
        public static string gSvr = "PATH1";

        //string mServerScanPath = "/scandata/";
        public static string gScanServerCode = "01";
        public static string gScanServer= string.Empty;
        public static string gServerScanPath = @"\scandata\";
        public static string gstrScanFold= string.Empty;
        public static string gServerPathbackup = @"backupdata\";

        public static string gClientMoveLLPath = @"C:\HealthSoft\ScanData\MoveLL\";
        public static string gClientMoveRRPath = @"C:\HealthSoft\ScanData\MoveRR\";
        public static string gClientOcrPath = @"\C:\HealthSoft\ScanData\Ocr\";
        public static string gClientScanJobPath = @"C:\HealthSoft\ScanData\ScanJob\";
        public static string gClientScanPath = @"C:\HealthSoft\ScanData\Scan\";

        //public static string gClientMoveLLPath = @"\ScanData\MoveLL\";
        //public static string gClientMoveRRPath = @"\ScanData\MoveRR\";
        //public static string gClientOcrPath = @"\ScanData\Ocr\";
        //public static string gClientScanJobPath = @"\ScanData\ScanJob\";
        //public static string gClientScanPath = @"\ScanData\Scan\";

        public static string gstrServerOs = "FTP"; //운영체제(NT:NetDriver, UNIX:FTP)

        public struct gEmrScanServerInformation
        {
            public string strServerOs;
            public string strFtpIp1;
            public string strFtpPort1;
            public string strFtpId1;
            public string strFtpPasswd1;
            public string strPath1;
            public string strPath2;
            public string strNETSERVERIP;
            public string strNETUSER;
            public string strNETPASSWORD;
        }
        public static gEmrScanServerInformation gEmrScanSvrInfo;


        //Scan 스프래드 Enum
        public enum ScanSp
        {
            sPTNO = 0,
            sACPNO,
            sCLSNO,
            sEMRNO,
            sSCANNO,
            sSEQNO,
            sFORMNO,
            sFORMNAME,
            sIMGPATH,
            sIMGNAME,
            sFORMNOCHG,
            sFLIPX,
            sFLIPY,
            sROTATE,
            sDEL,
            sIMGSVR,
            sSEVERPATH,
            sIMGEXTENSION,
            sSVRIP,
            sSVRID,
            sSVRPW,
            sSVRHOME
        }

        //OCR영역
        public struct typeOCR
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }
        public static typeOCR gOCR;

        public static string gOcrImgPath;

        #region Nt File Down
        public static bool NtFileCopy(string strSourceFilePath, string strDestFilePath)
        {
            bool returnValue = false;
            try
            {
                File.Copy(strSourceFilePath, strDestFilePath, true);
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return returnValue;
            }

        }
        #endregion

        public static void getOcrRect()
        {
            gOCR.X = (int)(VB.Val(ComFunc.ReadINI(@"C:\HealthSoft\ocr.ini", "RECT", "X")));
            gOCR.Y = (int)(VB.Val(ComFunc.ReadINI(@"C:\HealthSoft\ocr.ini", "RECT", "Y")));
            gOCR.Width = (int)(VB.Val(ComFunc.ReadINI(@"C:\HealthSoft\ocr.ini", "RECT", "Width")));
            gOCR.Height = (int)(VB.Val(ComFunc.ReadINI(@"C:\HealthSoft\ocr.ini", "RECT", "Height")));
        }

        //서버정보 가져오기
        public static void GetEmrScanServerInfo(PsmhDb pDbCon)
        {
            short i;
            DataTable dt = null;

            gEmrScanSvrInfo.strServerOs = "NT";

            //서버정보
            dt = clsEmrQuery.GetBBasCd(pDbCon, "SCAN기초", "SERVER");
            if (dt == null)
            {
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    switch (dt.Rows[i]["BASCD"].ToString().Trim().ToUpper())
                    {
                        case "OS":
                            gEmrScanSvrInfo.strServerOs = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "FTPSERVERIP1":
                            gEmrScanSvrInfo.strFtpIp1 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "FTPSERVERPORT1":
                            gEmrScanSvrInfo.strFtpPort1 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "FTPSERVERID1":
                            gEmrScanSvrInfo.strFtpId1 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "FTPSERVERPW1":
                            gEmrScanSvrInfo.strFtpPasswd1 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "PATH1":
                            gEmrScanSvrInfo.strPath1 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "PATH2":
                            gEmrScanSvrInfo.strPath2 = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "NETSERVERIP":
                            gEmrScanSvrInfo.strNETSERVERIP = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "NETUSER":
                            gEmrScanSvrInfo.strNETUSER = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                        case "NETPASSWORD":
                            gEmrScanSvrInfo.strNETPASSWORD = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                            break;
                    }
                }
            }
            dt.Dispose();
            dt = null;

            //OS 정보
            gstrServerOs = gEmrScanSvrInfo.strServerOs;
        }

    }
}
