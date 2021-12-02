using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ComNurLibB
{
    public class clsBXLLib
    {
        /////////////////////////////////////////////////////////////////
        //  Constant List

        //	Rotation
        public const int ROTATE_0 = 0;
        public const int ROTATE_90 = 1;
        public const int ROTATE_180 = 2;
        public const int ROTATE_270 = 3;

        //	Bar-code Type
        public const int CODE39 = 0;
        public const int CODE128 = 1;
        public const int I2OF5 = 2;
        public const int CODEBAR = 3;
        public const int CODE93 = 4;
        public const int UPC_A = 5;
        public const int UPC_E = 6;
        public const int EAN13 = 7;
        public const int EAN8 = 8;
        public const int UCC_EAN128 = 9;

        //	Device Fonts
        public const int ENG_9X15 = 0;	//	9 x 15
        public const int ENG_12X20 = 1;	//	12 x 20
        public const int ENG_16X25 = 2;	//	16 x 25
        public const int ENG_19X30 = 3;	//	19 x 30
        public const int ENG_24X38 = 4;	//	24 x 38
        public const int ENG_32X50 = 5;	//	32 x 50
        public const int ENG_48X76 = 6;	//	48 x 76
        public const int ENG_22X34 = 7;    //  22 x 34
        public const int ENG_28X44 = 8;    //  28 x 44
        public const int ENG_37X58 = 9;    //  37 x 58

        public const int KOR_16X16 = 0x61;	//	16 x 16
        public const int KOR_24X24 = 0x62;	//	24 x 24	
        public const int KOR_20X20 = 0x63; //  20 x 20
        public const int KOR_26X26 = 0x64; //  26 x 26
        public const int KOR_20X26 = 0x65; //  20 x 26
        public const int KOR_38X38 = 0x66; //  38 x 38

        public const int CHN_GB2312 = 0x6D;
        public const int CHN_BIG5 = 0x6E;

        //	Speed
        public const int SPEED_25 = 0;
        public const int SPEED_30 = 1;
        public const int SPEED_40 = 2;
        public const int SPEED_50 = 3;
        public const int SPEED_60 = 4;
        public const int SPEED_70 = 5;

        //	Orientation
        public const int TOP = 0;
        public const int BOTTOM = 1;

        //	Media Type
        public const int GAP = 0;
        public const int CONTINUOUS = 1;
        public const int BLACKMARK = 2;

        //	Block Option
        public const int LINE_OVER_WRITING = 0;
        public const int LINE_EXCLUSIVE_OR = 1;
        public const int LINE_DELETE = 2;
        public const int SLOPE = 3;
        public const int BOX = 4;

        // Font Selection
        public const string ASCII = "U";
        public const string KS5601 = "K";
        public const string BIG5 = "B";
        public const string GB2312 = "G";
        public const string ShiftJIS = "J";


        // Font Alignment
        public const string LEFTALIGN = "L";
        public const string RIGHTALIGN = "R";
        public const string CENTERALIGN = "C";

        // Font Direction
        public const int LEFTTORIGHT = 0;
        public const int RIGHTTOLEFT = 1;

        // QRCode MODEL
        public const int QRMODEL_1 = 1;
        public const int QRMODEL_2 = 2;

        // QRCode ECC Level
        public const int QRECCLEVEL_L = 1;   // 7%
        public const int QRECCLEVEL_M = 2;   // 15%
        public const int QRECCLEVEL_Q = 3;   // 25%
        public const int QRECCLEVEL_H = 4;   // 30%

        // QRCode size
        public const int QRSIZE_1 = 1;
        public const int QRSIZE_2 = 2;
        public const int QRSIZE_3 = 3;
        public const int QRSIZE_4 = 4;

        // Dither option 
        public const int DITHER_NONE = -1;
        public const int DITHER_1 = 0;
        public const int DITHER_2 = 1;
        public const int DITHER_3 = 6;
        public const int DITHER_4 = 7;

        //////////////////////////////////////////////////////////////////////
        //  Function List
        [DllImport("bxllib.dll")]
        public static extern bool ConnectPrinter(string szPrinterName);

        [DllImport("bxllib.dll")]
        public static extern bool DisconnectPrinter();

        [DllImport("bxllib.dll")]
        public static extern int GetBIXOLON_PrinterList(StringBuilder strBxlPrtList);

        [DllImport("bxllib.dll")]
        public static extern bool GetDllVersion(StringBuilder strDllVersion);

        [DllImport("bxllib.dll")]
        public static extern int GetPrinterResolution();

        [DllImport("bxllib.dll")]
        public static extern bool Print1DBarcode(int nHorizontalPos,
                                                 int nVerticalPos,
                                                 int nBarcodeType,
                                                 int nNarrowBarWidth,
                                                 int nWideBarWidth,
                                                 int nBarcodeHeight,
                                                 int nRotation,
                                                 bool bHRI,
                                                 string pData);

        [DllImport("bxllib.dll")]
        public static extern bool PrintDeviceFont(int nHorizontalPos,
                                                  int nVerticalPos,
                                                  int nFontName,
                                                  int nHorizontalMulti,
                                                  int nVerticalMulti,
                                                  int nRotation,
                                                  bool bBold,
                                                  string szText);

        [DllImport("bxllib.dll")]
        public static extern bool SetConfigOfPrinter(int nSpeed,
                                                     int nDensity,
                                                     int nOrientation,
                                                     bool bAutoCut,
                                                     int nCuttingPeriod,
                                                     bool bBackFeeding);

        [DllImport("bxllib.dll")]
        public static extern bool Prints(int nLabelSet,
                                         int nCopiesOfEachLabel);

        [DllImport("bxllib.dll")]
        public static extern bool SetPaper(int nHorizontalMagin,
                                           int nVerticalMargin,
                                           int nPaperWidth,
                                           int nPaperLength,
                                           int nMediaType,
                                           int nOffSet,
                                           int nGapLengthORThicknessOfBlackLine);

        [DllImport("bxllib.dll")]
        public static extern bool ClearBuffer();

        [DllImport("bxllib.dll")]
        public static extern bool PrintBlock(int nHorizontalStartPos,
                                             int nVerticalStartPos,
                                             int nHorizontalEndPos,
                                             int nVerticalEndPos,
                                             int nOption,
                                             int nThickness);

        ///******************************************************************************/
        // Circle draw 
        // int nHorizontalStartPos	: X position
        // int nVerticalStartPos	: Y position
        // int nDiameter			: 원 Size(반지름) 1~6
        // int nMulti				: 확대(1~4)
        /******************************************************************************/
        [DllImport("bxllib.dll")]
        public static extern bool PrintCircle(int nHorizontalStartPos,
                                             int nVerticalStartPos,
                                             int nDiameter,
                                             int nMulti);

        [DllImport("bxllib.dll")]
        public static extern int PrintDirect(string pDirectData);


        [DllImport("bxllib.dll")]
        public static extern bool StartLabel();

        [DllImport("bxllib.dll")]
        public static extern void EndLabel();


        [DllImport("bxllib.dll")]
        public static extern bool PrintTrueFontLib(int nXPos,
                                        int nYPos,
                                        string strFontName,
                                        int nFontSize,
                                        int nRotaion,
                                        bool bItalic,
                                        bool bBold,
                                        bool bUnderline,
                                        string strText);

        [DllImport("bxllib.dll")]
        public static extern bool PrintTrueFontLibWithAlign(int nXPos,
                                        int nYPos,
                                        string strFontName,
                                        int nFontSize,
                                        int nRotaion,
                                        bool bItalic,
                                        bool bBold,
                                        bool bUnderline,
                                        string strText,
                                        int nPrintWidth,
                                        int nAlignment);

        [DllImport("bxllib.dll")]
        public static extern bool PrintVectorFont(
                                                int nXPos,
                                                int nYPos,
                                                string FontSelection,
                                                int FontWidth,
                                                int FontHeight,
                                                string RightSideCharSpacing,
                                                bool bBold,
                                                bool ReversePrinting,
                                                bool TextStyle,
                                                int Rotation,
                                                string TextAlignment,
                                                int TextDirection,
                                                string pData);

        [DllImport("bxllib.dll")]
        public static extern bool PrintQRCode(int nXPos,
                           int nYPos,
                           int nModel,
                           int nECCLevel,
                           int nSize,
                           int nRotation,
                           string pData);

        [DllImport("bxllib.dll")]
        public static extern bool SetShowMsgBox(bool bShowMsgBox);


        [DllImport("bxllib.dll")]
        public static extern bool PrintImageLib(int nHorizontalStartPos,
                            int nVerticalStartPos,
                            string pBitmapFilename,
                            int nDither,
                            bool bDataCompression);

        [DllImport("bxllib.dll")]
        public static extern bool PrintImageLibWithSize(int nHorizontalStartPos,
                            int nVerticalStartPos,
                            int nNewWidth,
                            int nNewHeight,
                            string pBitmapFilename,
                            int nDither,
                            bool bDataCompression);
    }
}
