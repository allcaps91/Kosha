using System.Drawing;
using System.Drawing.Printing;

namespace ComSupLibB.Com
{

    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : clsParam.cs
    /// Description     : 진료지원 공통 변수
    /// Author          : 김홍록
    /// Create Date     : 2017-08-18
    /// Update History  : 
    /// </summary>
    /// <history>       
    /// </history>
    /// <seealso cref= "신규" />
    public class clsParam
    {

        /// <summary>요양기관코드</summary>
        public const string ComSupHOSP_ID = "37100068";
        /// <summary>요양기관명</summary>
        public const string ComSupHOSP_NAME = "포항성모병원";
        /// <summary>주사실비상마약관리대장 인터페이스용 ward 코드</summary>
        public const string ComSupIjrm = "JS";
        /// <summary>정도관리환자정보</summary>
        public const string QC_PTNO = "81000013";

        public const string BARCODE_PRINTER_NAME = "혈액환자정보";

        public const string EXAM_OUT_PATH = @"C:\삼광의료재단\";
        public const string EXAM_OUT_PATH_BACK = @"C:\삼광의료재단\작업완료\";
        public const string EXAM_OUT_IMG_FILE_TYPE = ".jpg";
        public const string EXAM_OUT_TXT_FILE_TYPE = ".txt";

        public const string EXAM_IMG_TMP_PATH = @"C:\삼광의료재단\EXAM\";

        public const string EXAM_WORK_PATH = @"C:\EXAM\";


        public const string WINDOW_IMG_DLL_VIEW_PATH = @"C:\WINDOWS\SYSTEM32\SHIMGVW.DLL";

        public const string EXAM_OUT_DR = "DR";

        public const string EXAM_DEPT = "EXAM_DEPT";

        public const string EXAM_DEPT_LIS = "LIS";

        public const string EXAM_DEPT_ENDO = "ENDO";


        /// <summary>Save옵션</summary>
        public enum enmComParamSave
        {
            Create, Retrive,Update,Delete
        }

        /// <summary> 콤버박스에서 All을 삽입여부</summary>
        public enum enmComParamComboType
        {
            /// <summary> ****.전체 형태로 삽입</summary>
            ALL,
            /// <summary> Null.형태로 삽입</summary>
            NULL,
            /// <summary> 데이터만 삽입</summary>
            None
        }

        /// <summary>환자입원외래구분</summary>
        public enum enmComParamGBIO
        {
            /// <summary>전체</summary>
            ALL,
            /// <summary>입원</summary>
            I,
            /// <summary>외래</summary>
            O,
            /// <summary>응급</summary>
            E

        }

        public PaperSize barCodeSize = new PaperSize("BARCODESIZE", 50, 30);

        public Brush _textColor = Brushes.Black;
        public Brush _lineColor = Brushes.Black;

        public const int nCol_SCHK          =  16;
        public const int nCol_PANO          =  60 + 15;
        public const int nCol_DATE          =  60 + 15;
        public const int nCol_JUMIN1        =  60;
        public const int nCol_TIME          = 110 + 15;
        public const int nCol_SNAME         =  60 + 15;
        public const int nCol_CHEK          =  50 + 15;
        public const int nCol_AGE           =  30 + 15;
        public const int nCol_SEX           =  30 + 15;
        public const int nCol_DPNM          =  60 + 15;
        public const int nCol_DPCD          =  30 + 15;
        public const int nCol_WARD          =  40 + 15;
        public const int nCol_SPNO          =  60 + 15;
        public const int nCol_IOPD          =  30 + 15;
        public const int nCol_NAME          = 150 + 15;
        public const int nCol_JUSO          = 300 + 15;
        public const int nCol_STAT          =  60 + 15;
        public const int nCol_EXCD          =  60 + 15;
        public const int nCol_JUMN1         =  70 + 15;
        public const int nCol_TEL           = 100 + 15;
        public const int nCol_LNAME         = 460;
        public const int nCol_ORDERNAME     = 180;

        public const string gPB             = "HR10";

        public Color cSpdRowSubTotalColor   = Color.FromArgb(255, 220, 220);
        public Color cSpdRowSumColor        = Color.FromArgb(255, 100, 100);
        public Color cSpdCellClick          = Color.FromArgb(240, 250, 250);
        public Color cSpdCellClick_LIS      = Color.PaleGreen;

        public Color cSpdCellImpact_Back    = Color.FromArgb(220, 55, 55);
        public Color cSpdCellImpact_Fore    = Color.FromArgb(255, 250, 250);

        public Color cQC1                   = Color.FromArgb(0, 0, 0);
        public Color cQC2                   = Color.FromArgb(0, 162, 232);
        public Color cQC3                   = Color.FromArgb(232, 28, 36);
        public Color cQC4                   = Color.FromArgb(34, 177, 76);
        public Color cQC5                   = Color.FromArgb(255, 242, 0);
        public Color cQC6                   = Color.FromArgb(163, 73, 164);
        public Color cQC7                   = Color.FromArgb(63, 72, 204);
        public Color cQC8                   = Color.FromArgb(255, 127, 39);
        public Color cQC9                   = Color.FromArgb(136, 0, 21);
        public Color cQC10                   = Color.FromArgb(127, 127, 127);


        public Color cPalePurple = Color.MediumPurple;
        public Color cPaleGreen = Color.PaleGreen;
        

        /// <summary>검체바코드 사이즈</summary>
        public const int nBarCodeLength = 10;

        public const int nDateGap = 10;

        /// <summary>메시지의 아이콘 설정</summary>
        public enum enmMsgIcon
        {
            /// <summary>아이콘 없음</summary>
            none,
            /// <summary>정보아이콘모양 아이콘 </summary>
            Info,
            /// <summary>에러아이콘모양 아이콘 </summary>
            Error,
            /// <summary>물음표아이콘모양 아이콘 </summary>
            question,
            /// <summary>경고아이콘모양 아이콘 </summary>
            waring
        };

        /// <summary> 바코드변수 2017.03.08</summary>
        public struct BarCodeParam
        {

            /// <summary> 환자명</summary>
            public string ptName;
            /// <summary> 환자번호</summary>
            public string ptNo;
            /// <summary> 나이</summary>
            public string age;
            /// <summary> 성별</summary>
            public string sex;
            /// <summary> 진료구분</summary>
            public string gbio;
            /// <summary> 병동</summary>
            public string ward;
            /// <summary> 병실</summary>
            public string room;
            /// <summary> 진료과</summary>
            public string dept;
            /// <summary> 워크센터</summary>
            public string wc;
            /// <summary> 검체번호</summary>
            public string specNo;
            /// <summary> 바코드명</summary>
            public string barCodeName;
            /// <summary> 검체</summary>
            public string speciMan;
            /// <summary> 응급여부</summary>
            public string er;
            /// <summary> 의사명</summary>
            public string drName;
            /// <summary> 발행일시</summary>
            public string receipTime;
            /// <summary> 감염여부</summary>
            public string inFect;

        }





    }
}
