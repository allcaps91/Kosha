using ComBase;
using System;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDrugMaills.cs
    /// Description     : 약품사진 다운로드
    /// Author          : 이정현
    /// Create Date     : 2017-11-29
    /// <history> 
    /// 약품사진 다운로드
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm약사진다운로드.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstMedPictureDown : Form
    {
        public delegate void SendDataHandler(string SendRetValue);
        public event SendDataHandler SendEvent;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        private string GstrBCode = "";

        public frmSupDrstMedPictureDown()
        {
            InitializeComponent();
        }

        public frmSupDrstMedPictureDown(string strBCode)
        {
            InitializeComponent();

            GstrBCode = strBCode;
        }

        private void frmSupDrstMedPictureDown_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            webBrowser1.Navigate("http://www.druginfo.co.kr");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strHTML = "";
            string strTemp = "";
            string strNo = "";
            string strYakName = "";
            string strURL = "";
            string strLocal = "";

            strHTML = webBrowser1.Document.ActiveElement.InnerHtml;
            strTemp = STRCUT2(strHTML, "<A class=p href=" + (char)34 + "/detail/product.aspx?pid=", "<TR>");

            if (VB.InStr(strTemp, GstrBCode) == 0)
            {
                ComFunc.MsgBox("검색정보내에 원하시는 표준코드가 없습니다.", "오류");
                return;
            }

            //STRCUT 함수를 자체 폼으로 옮겨사 용함
            strNo = STRCUT2(strTemp, "href=" + (char)34 + "/drugimg/", ((char)34).ToString()).Trim();

            if (strNo == "")
            {
                ComFunc.MsgBox("검색정보내에 원하시는 사진정보가 없습니다.", "오류");
                return;
            }

            //약품명을 읽음
            strYakName = STRCUT2(strTemp, (char)34 + ">", "</A>").Trim();
            strYakName = strYakName.Replace("<B>", "");
            strYakName = strYakName.Replace("</B>", "");
            strYakName = strYakName.Replace(" ", "").Trim();
            strYakName = strYakName.Replace(".", "").Trim();
            strYakName = strYakName.Replace("/", "_").Trim();
            strYakName = VB.Left(strYakName, 10);

            strURL = "http://www.druginfo.co.kr/drugimg/" + strNo;
            strLocal = "c:\\Pharm_Image\\" + GstrBCode + "_" + strYakName + ".jpg";

            Dir_Check("c:\\Pharm_Image");

            if (DownloadFile(strURL, strLocal) == false)
            {
                ComFunc.MsgBox("약품사진 다운로드 실패");
                return;
            }

            SendEvent(strLocal);
        }

        private bool DownloadFile(string strURL, string strLocalFile)
        {
            //TODO : URLDOWNLOAD DLL 필요
            bool rtnVal = false;

            //'무조건 다시 다운로드를 위해 캐쉬를 삭제
            //Call DeleteUrlCacheEntry(URL)
            //'자료를 다운로드함
            //If Dir(LocalFile) <> "" Then Kill LocalFile
            //lRet = URLDownloadToFile(ByVal 0, URL, LocalFile, ByVal 0, ByVal 0)
            //If lRet = 0 Then DownloadFile = True
            //Exit Function

            return rtnVal;
        }

        private void Dir_Check(string sDirPath)
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sDirPath);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }
        
        private string STRCUT2(string strString, string strStart, string strEnd)
        {
            int intStartPos = 0;
            int intEndPos = 0;
            string rtnVal = "";

            //문자열이 공란이면 공란을 Return
            if (strString == "") { return rtnVal; }

            //짜를 시작문자,종료문자가 모두 NULL이면 전체문장을 Return
            if (strStart == "" && strEnd == "")
            {
                rtnVal = strString;
                return rtnVal;
            }

            //비교대상,비교할 단어를 소문자로 변경함
            strString = strString.ToLower();
            strStart = strStart.ToLower();
            strEnd = strEnd.ToLower();

            //-----------------------------------
            // 문자열중 짜를 시작위치를 찾음
            //-----------------------------------
            if (strStart == "")
            {
                intStartPos = 1;
            }
            else
            {
                intStartPos = VB.InStr(strString, strStart);

                if (intStartPos > 0)
                {
                    intStartPos = intStartPos + strStart.Length;
                }
                else
                {
                    rtnVal = "";
                    return rtnVal;
                }
            }

            //-----------------------------------
            // 문자열중 짜를 종료위치를 찾음
            //-----------------------------------
            if (strEnd == "")
            {
                intEndPos = strString.Length;
            }
            else
            {
                intEndPos = VB.InStr(intStartPos, strString, strEnd);

                if (intEndPos > 0)
                {
                    intEndPos = intEndPos - 1;
                }
                else
                {
                    rtnVal = "";
                    return rtnVal;
                }
            }

            if (intEndPos >= intStartPos)
            {
                rtnVal = VB.Mid(strString, intStartPos, (intEndPos - intStartPos + 1));
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed == null)
            {
                this.Close();
            }
            else
            {
                rEventClosed();
            }
        }
    }
}
