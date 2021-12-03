using ComBase; //기본 클래스
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupFnExVIEW05.cs
    /// Description     : 류마티스 현미경 검사 사진 보기 -> 상단으로 이동함 기존 frmComSupFnExVIEW03
    /// Author          : 윤조연
    /// Create Date     : 2018-10-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\nvc\Frm사진보기.frm(frm사진보기) >> frmComSupFnExVIEW03.cs >> frmComSupFnExVIEW05 c#에서 폼 추가함" />
    public partial class frmComSupFnExVIEW05 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = null; //공용함수                
        ComFunc fun = null;
        

        DevComponents.DotNetBar.Metro.MetroTileItem[] Item1 = null;

        string gFiles = "";
        string gTitle = "NVC Images";

        #region //단독실행 상단의 변수 
        string NVC_PATH = @"d:\interface\nvc\nvc_temp\";
        string NVC_PATHB = @"d:\interface\nvc\nvc_backup\";
        string NVC_PATH_VIEW = @"c:\cmc\nvc_temp\";
        string NVC_HOST = "/data/NVC/";
        int NVC_IMG_CNT = 10;
        #endregion

        #endregion
        public frmComSupFnExVIEW05(string argFiles)
        {
            InitializeComponent();

            gFiles = argFiles;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCtl(NVC_IMG_CNT);

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);


        }

        void setCtl(int cnt)
        {
            if (Item1 != null)
            {
                itemContainer1.SubItems.RemoveRange(Item1);
            }

            Item1 = new DevComponents.DotNetBar.Metro.MetroTileItem[cnt];

            for (int i = 0; i < cnt; i++)
            {
                Item1[i] = new DevComponents.DotNetBar.Metro.MetroTileItem();
                Item1[i].ImageTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
                Item1[i].Name = "";
                Item1[i].SymbolColor = System.Drawing.Color.Empty;
                Item1[i].TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.Default;
                Item1[i].TileSize = new System.Drawing.Size(520, 480);
                Item1[i].Visible = true;
                Item1[i].DoubleClick += new EventHandler(ePicDClick);

            }

            itemContainer1.SubItems.AddRange(Item1);

        }

        void setCtl_Visible(int cnt, int cnt2)
        {
            if (Item1 == null)
            {
                return;
            }

            for (int i = 0; i < cnt; i++)
            {
                Item1[i].Visible = false;
            }

            for (int i = 0; i < cnt2; i++)
            {
                Item1[i].Visible = true;
            }

        }

        void ePicDClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Metro.MetroTileItem p = (DevComponents.DotNetBar.Metro.MetroTileItem)sender;

            if (p.Text == "")
            {
                p.Text = p.Name;
                p.TileSize = new Size(520, 480);
            }
            else
            {
                p.Text = "";
                p.TileSize = new Size(1000, 800);
            }


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                cpublic = new clsPublic(); //공용함수                        
                fun = new ComFunc();                

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData();

                if (gFiles != "")
                {
                    itemContainer1.TitleText = gTitle + "(" + display_nvc_img_file_dotnetbar(Item1, NVC_PATH_VIEW, gFiles, NVC_HOST) + ")";
                }
                else
                {
                    this.Close();
                    return;
                }
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                folder_sts_chk(NVC_PATH_VIEW);
                this.Close();
            }


        }

        void screen_clear()
        {
            itemContainer1.TitleText = gTitle;
        }


        #region //단독실행관련
        void folder_sts_chk(string folderpath, string argExe = "*.jpg")
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

        int display_nvc_img_file_dotnetbar(DevComponents.DotNetBar.Metro.MetroTileItem[] Item1, string folderpath, string argFileName, string argNVC_HOST, bool bDown = false)
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
            string strYYMM = setP(setP(sFile[0].Trim(), argNVC_HOST, 2), "/", 1);
            string strHost = argNVC_HOST + strYYMM + "/"; //ftp 저장경로


            Ftpedt FtpedtX = new Ftpedt(); //FTP
            FileStream fs = null;

            setCtl_Visible(NVC_IMG_CNT, sFile.Length - 1);

            //clear
            for (int i = 0; i < Item1.Length; i++)
            {
                Item1[i].TileStyle.BackgroundImage = null;
                Item1[i].Text = "";
            }

            for (int i = 0; i < sFile.Length - 1; i++)
            {
                strFile = setP(sFile[i], "/", 5).Trim();

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


        string setP(string str, string ch, int n)
        {
            string[] c = VB.Split(str, ch);

            if (c.Length == 0 || c.Length < n) return "";

            try
            {
                return c[n - 1];
            }
            catch
            {
                return "";
            }

        }

        #endregion

    }
}
