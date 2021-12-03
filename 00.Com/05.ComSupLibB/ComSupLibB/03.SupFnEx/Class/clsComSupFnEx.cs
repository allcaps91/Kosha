using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB;
using System.IO;
using System.Data;
using ComSupLibB.Com;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXray.cs
    /// Description     : 진료지원 공통 기능검사 메인 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupFnEx
    {

        public const string NVC_PATH = @"d:\interface\nvc\nvc_temp\";
        public const string NVC_PATHB = @"d:\interface\nvc\nvc_backup\";
        public const string NVC_PATH_VIEW = @"c:\cmc\nvc_temp\";
        public const string NVC_HOST = "/data/NVC/";
        public const int NVC_IMG_CNT = 100;


        public const string STRESS_PATH = @"X:\";
        public const string STRESS_PATHB = @"X:\backup_image\";
        public const string STRESS_HOST = "/data/ocs_etc/";

        public const string WINDOW_IMG_DLL_VIEW_PATH = @"C:\WINDOWS\SYSTEM32\SHIMGVW.DLL";
        public const string TEMP_VIEW_IMG_PATH = @"c:\cmc\temp_img\";

        public const string ECG_EXE_VIEW_PATH = @"C:\Program Files\NKC\ECG Viewer3\FileViewer.exe";
        public const string ECG_EXE_VIEW_PATHx64 = @"C:\Program Files (x86)\NKC\ECG Viewer3\FileViewer.exe";        
        public const string TEMP_VIEW_ECG_PATH = @"c:\cmc\temp_ecg\";

        public const string TEMP_VIEW_EMG_PATH = @"c:\cmc\temp_emg\";


        public enum enmRDateTime { Part, STS, OK, RDate, RTime, RDateTime }; //EKG 예약변경시 사용될 델리게이트에 사용
        public enum enmfrmComSupFnExPOP01 { Part, STS, RDate, RTime }; //frmComSupFnExPOP01 사용

        public enum enm_FnExPart { ALL,PART1,PART2,PART3,PART4, PART5 }; //심전도 관리 기본파트 1.심전도, 2.심초음파, 3.근전도, 4.청력실


        public void FnEx_display_img_file(string Job, string argYYMM, string argFile,  string argViewGbn = "")
        {
            string strFile = "";
            string strHost = "";
            string strHostFile = "";
            

        
            if (Job == "STRESS")
            {

                if (!System.IO.File.Exists(WINDOW_IMG_DLL_VIEW_PATH))
                {
                    ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 않았습니다", "오류");
                    return;
                }

                strFile = TEMP_VIEW_IMG_PATH + "ETC.JPG";
                strHost = STRESS_HOST;

                //폴더체크 및 폴더생성 및 파일삭제
                folder_sts_chk(TEMP_VIEW_IMG_PATH);

            }
            else if (Job == "ECG")
            {
                if (!System.IO.File.Exists(ECG_EXE_VIEW_PATH))
                {                    
                    ComFunc.MsgBox("ECG IMAGE Viewer가 설치되지 않았습니다", "오류");
                    return;                                        
                }
                
                //strFile = TEMP_VIEW_ECG_PATH + "ETC.ecg";
                strFile = TEMP_VIEW_ECG_PATH + argFile  ;
                strHost = STRESS_HOST;

                //폴더체크 및 폴더생성 및 파일삭제                
                folder_sts_chk(TEMP_VIEW_ECG_PATH,"*.ecg");               
                
            }
            else
            {
                if(!System.IO.File.Exists(WINDOW_IMG_DLL_VIEW_PATH))
                {
                    ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 않았습니다", "오류");
                    return;
                }

                strFile = clsComSupFnEx.TEMP_VIEW_IMG_PATH + "ETC.JPG";
                strHost = clsComSupFnEx.STRESS_HOST;

                //폴더체크 및 폴더생성 및 파일삭제
                folder_sts_chk(TEMP_VIEW_IMG_PATH);

            }
                        
            strHostFile = strHost + argYYMM + "/" + argFile;

            Ftpedt FtpedtX = new Ftpedt(); //FTP

            FtpedtX.FtpDownload("192.168.100.31", "oracle", "oracle", strFile, strHostFile, strHost); //TODO 윤조연 FTP 계정 정리

            FtpedtX = null;


            if (argViewGbn == "1")
            {
                if (Job == "STRESS")
                {
                    VB.Shell("rundll32.exe " + clsComSupFnEx.WINDOW_IMG_DLL_VIEW_PATH + ", ImageView_Fullscreen " + strFile);
                }
                else if (Job == "ECG")
                {
                    VB.Shell(ECG_EXE_VIEW_PATH + "  " + strFile + " ");

                    ComFunc.Delay(1000);
                    //폴더체크 및 폴더생성 및 파일삭제                
                    folder_sts_chk(TEMP_VIEW_ECG_PATH, "*.ecg");
                }
                else
                {
                    VB.Shell("rundll32.exe " + clsComSupFnEx.WINDOW_IMG_DLL_VIEW_PATH + ", ImageView_Fullscreen " + strFile);
                }
                    
            }
           
        }

        public void FnEx_display_EMG_file(DataTable dt, string argViewGbn = "")
        {
            string strFile = "";
            string strHost = "";
            string strHostFile = "";
            string strFile1 = "";
            
                        
            if (!System.IO.File.Exists(WINDOW_IMG_DLL_VIEW_PATH))
            {
                ComFunc.MsgBox("WINDOWS IMAGE Viewer가 설치되지 않았습니다", "오류");
                return;
            }
                        
            strHost = clsComSupFnEx.STRESS_HOST;

            //폴더체크 및 폴더생성 및 파일삭제
            folder_sts_chk(TEMP_VIEW_EMG_PATH);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strFile = clsComSupFnEx.TEMP_VIEW_EMG_PATH + "EMG" + dt.Rows[i]["SEQNO"].ToString().Trim() + ".JPG";
                if (i==0 && strFile1 =="")
                {
                    strFile1 = strFile;
                }

                strHostFile = strHost + dt.Rows[i]["RDate"].ToString().Trim() + "/" + dt.Rows[i]["FileName"].ToString().Trim(); 

                Ftpedt FtpedtX = new Ftpedt(); //FTP

                FtpedtX.FtpDownload("192.168.100.31", "oracle", "oracle", strFile, strHostFile, strHost); //TODO 윤조연 FTP 계정 정리

                FtpedtX = null;

            }            

            if (argViewGbn == "1" && strFile1 !="")
            {                
                VB.Shell("rundll32.exe " + clsComSupFnEx.WINDOW_IMG_DLL_VIEW_PATH + ", ImageView_Fullscreen " + strFile1);
               
            }

        }

        public void folder_sts_chk(string folderpath,string argExe="*.jpg")
        {
            //디렉토리 체크후 없으면 생성 있으면 폴더내 파일 삭제후 작업
            DirectoryInfo di = new DirectoryInfo(folderpath);
            if (di.Exists == false)
            {
                di.Create();
            }
            else
            {

                System.IO.FileInfo[] files = di.GetFiles(argExe, SearchOption.AllDirectories);

                foreach (System.IO.FileInfo file in files)
                {
                    //file.Attributes = FileAttributes.Normal; 
                    file.Delete();
                }

                //Directory.Delete(folderpath, true);

            }
        }

        #region 닷넷바의 메트로 콘트롤 이용하여 이미지 뷰어 구현

        /// <summary>
        /// 류마티스 현미경 검사 결과 이미지 뷰어 관련
        /// </summary>
        /// <param name="Item1"></param>
        /// <param name="folderpath"></param>
        /// <param name="argFileName"></param>
        public int display_nvc_img_file_dotnetbar(DevComponents.DotNetBar.Metro.MetroTileItem[] Item1, string folderpath, string argFileName, string argNVC_HOST, bool bDown = false)
        {
            string[] sFile = argFileName.Split('|');


            #region 폴더유무 체크 및 생성, 폴더내 파일 삭제...
            if (bDown)
            {
                folder_sts_chk(folderpath);
            }

            #endregion


            //이미지 뷰
            string strFile = "";
            string strYYMM = clsComSup.setP(clsComSup.setP(sFile[0].Trim(), argNVC_HOST, 2), "/", 1);
            string strHost = argNVC_HOST + strYYMM + "/"; //ftp 저장경로


            Ftpedt FtpedtX = new Ftpedt(); //FTP
            FileStream fs = null;

            //clear
            for (int i = 0; i < Item1.Length; i++)
            {
                Item1[i].TileStyle.BackgroundImage = null;
                Item1[i].Text = "";
            }

            for (int i = 0; i < sFile.Length - 1; i++)
            {
                strFile = clsComSup.setP(sFile[i], "/", 5).Trim();

                FtpedtX.FtpDownload("192.168.100.31", "oracle", "oracle", folderpath + strFile, strHost + strFile, strHost); //TODO 윤조연 FTP 계정 정리

                fs = new System.IO.FileStream(folderpath + strFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                Item1[i].TileStyle.BackgroundImage = System.Drawing.Image.FromStream(fs);
                //Item1.TileStyle.BackgroundImage = Image.FromFile(folderpath + strFile);
                Item1[i].TileStyle.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Stretch;
                Item1[i].TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                Item1[i].Text = (i + 1).ToString() + "." + strFile;
                Item1[i].Name = (i + 1).ToString() + "." + strFile;

                fs.Close();

            }

            FtpedtX = null;

            return sFile.Length - 1;

        }

        public int folder_display_img_file_dotnetbar(DevComponents.DotNetBar.Metro.MetroTileItem[] Item1, string argFolder)
        {
            string strFile = "";

            //clear
            for (int i = 0; i < Item1.Length; i++)
            {
                Item1[i].TileStyle.BackgroundImage = null;
                Item1[i].Text = "";
            }

            FileStream fs = null;

            DirectoryInfo di = new System.IO.DirectoryInfo(argFolder);
            System.IO.FileInfo[] fi = di.GetFiles("*.jpg");

            if (fi.Length != 0)
            {

                for (int i = 0; i < fi.Length; i++)
                {
                    strFile = fi[i].Name.ToString();
                    fs = new System.IO.FileStream(argFolder + strFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    Item1[i].TileStyle.BackgroundImage = System.Drawing.Image.FromStream(fs);
                    Item1[i].TileStyle.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Stretch;
                    Item1[i].TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    Item1[i].Text = (i + 1).ToString() + "." + strFile;
                    Item1[i].Name = (i + 1).ToString() + "." + strFile;

                    fs.Close();

                }
            }

            return fi.Length;
        }



        #endregion

    }
}
