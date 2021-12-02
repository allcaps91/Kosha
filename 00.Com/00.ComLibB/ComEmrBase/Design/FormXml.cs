using System.Drawing;

namespace ComEmrBase
{
    /// <summary>
    /// 서식지 컨트롤 속성 값
    /// </summary>
    public class FormXml
    {
        public string strCONTROLNAME = string.Empty;
        public string strCONTROTYPE = string.Empty;
        public string strCONTROLPARENT = string.Empty;
        public string strLOCATIONX = string.Empty;
        public string strLOCATIONY = string.Empty;
        public string strSIZEWIDTH = string.Empty;
        public string strSIZEHEIGHT = string.Empty;
        public string strUSERFUNC = string.Empty;
        public string strFRONTBACK = string.Empty; //사용안함
        public string strCHILDINDEX = string.Empty;
        public string strBACKCOLOR = string.Empty;
        public string strFORECOLOR = string.Empty;
        public string strBOARDSTYLE = string.Empty;
        public string strDOCK = string.Empty;
        public string strENABLED = string.Empty;
        public string strVISIBLED = string.Empty;
        public string strTABORDER = string.Empty;
        public string strTEXT = string.Empty;
        public string strFONTS = string.Empty;
        public string strMULTILINE = string.Empty;
        public string strSCROLLBARS = string.Empty;
        public string strMAXTEXTLENGTH = string.Empty;
        public string strTEXTREADONLY = string.Empty;
        public string strTEXTALIGN = string.Empty;
        public string strITEMIMAGE = string.Empty; //PictureBox 이미지 경로
        public Image imgIMAGE = null; //PictureBox 이미지 이미지
        public string strIMAGESIZEMODE = string.Empty; //PictureBox 이미지
        public string strCHECKALIGN = string.Empty; //CheckBox, RadioButton : 값
        public string strCHECKED = string.Empty; //CheckBox, RadioButton : 값
        public string strAPPEARANCS = string.Empty; //CheckBox, RadioButton : 버튼 모양
        public string strFLATSTYLE = string.Empty;
        public string strFLATBORDERCOLOR = string.Empty; //사용안함
        public string strFLATBORDERSIZE = string.Empty;
        public string strAUTOSIZE = string.Empty;
        public string strAUTOHEIGH = string.Empty; //TextBox 높이 자동 조절
        public string strPARENTNAME = string.Empty;
        public string strPARENTTYPE = string.Empty;
        public string strMIBI = string.Empty;
        public string strINITVALUE = string.Empty;
    }
}
