using System;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComEmrBase
{
    public class clsScan
    {
        private static string mstrScanFold = "\\scandata";

        public static bool ChkNetDrive(string sFileName)
        {
            bool rtnVal = false;
            //이미지서버 연결이 끊어 지는 경우가 있어 한번더 체크한다.
            FileInfo patfile = new FileInfo(sFileName);
            if (patfile.Exists == false)
            {
                if (NetScanDrive() == true)
                {
                    rtnVal = true;
                }
            }
            rtnVal = true;
            return rtnVal;
        }

        private static bool NetScanDrive()
        {
            try
            {
                bool blnConYn = false;
                string sFileName = clsScanPublic.gEmrScanSvrInfo.strPath1 + mstrScanFold + "\\hsplog.png";

                FileInfo patfile = new FileInfo(sFileName);
                if (patfile.Exists == true)
                {
                    return true;
                }

                blnConYn = clsNetDrive.ConnectRemoteServer(clsScanPublic.gEmrScanSvrInfo.strPath1, clsScanPublic.gEmrScanSvrInfo.strNETUSER, clsScanPublic.gEmrScanSvrInfo.strNETPASSWORD);

                if (blnConYn == false)
                {
                    ComFunc.MsgBox("파일서버 연결에 실패했습니다.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        public static bool DeleteFoldAll(string strPath)
        {
            try
            {
                DirectoryInfo diSource = new DirectoryInfo(strPath);
                foreach (FileInfo fi in diSource.GetFiles())
                {
                    fi.Delete();
                }
                diSource = null;
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("프로세스") == -1)
                {
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                }
                return false;
            }
        }
    }
}
