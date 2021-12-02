using ComBase; //기본 클래스

namespace ComEmrBase
{
    public class clsScanSet
    {
        public struct ScanSetType //로그인시 사용자정보
        {
            public static string SourceName; //스캐너이름
            public static string SelectFeeder; //급지방식(자동급지) : 고속,평판
            public static string SetHideUI; //사용자 정의 인터페이스 숨기기
            public static string EnableDuplex; //양면/단면
            public static string SetPixelType; //흑백black/gray : 1 , 칼라 color : 2
            //    SetBitDepth As String       'black 1, gray 8,16 / color 24(세팅안함) , 48
            public static string SetResolution; //DPI 기본 0
            public static string SetPaperSize; //용지 사이즈 세팅 : 기본은 -1, 사용자 설정은 0, 나머지는 index - 1
            public static string SetRegionLeft; //용지 사이즈
            public static string SetRegionTop; //용지 사이즈
            public static string SetRegionWidth; //용지 사이즈
            public static string SetRegionHeight; //용지 사이즈
            public static string SetFileFormat; //저장시 파일 포맷(png)
            public static string SetFileAppendFlag; //TIF,JPG,PDF: 0 // BMP,PNG,GIF 설정안함
            public static string SetJpegQuality; //TIF,JPG,PDF: 76 // BMP,PNG,GIF 설정안함
            public static string SetIndicators; //Progress Hide : 0
            public static string SetAutoDeskew; //Deskew :1
            public static string SetAutoCrop; //Crop : 1
            public static string SetBlankSize; //빈용지 사이즈 : 1000
            /// <summary>
            /// 앞면 각도(0: 0, 1: 90, 2: 180, 3: 270)
            /// </summary>
            public static string SetFront;
            /// <summary>
            /// 뒷면 각도(0: 0, 1: 90, 2: 180, 3: 270)
            /// </summary>
            public static string SetRear;
        }

        public static void InitScanSet()
        {
            ScanSetType.SourceName = ""; //스캐너이름
            ScanSetType.SelectFeeder = "1"; //급지방식(자동급지) : 고속,평판
            ScanSetType.EnableDuplex = "0"; //사용자 정의 인터페이스 숨기기
            ScanSetType.SetPixelType = "1"; //양면/단면
            //        .SetBitDepth = ReadIni("SCANSET", "SetBitDepth")
            ScanSetType.SetResolution = "150"; //DPI 기본 0
            ScanSetType.SetPaperSize = "A4"; //용지 사이즈 세팅 : 기본은 -1, 사용자 설정은 0, 나머지는 index - 1

            ScanSetType.SetRegionLeft = "0"; //용지 사이즈
            ScanSetType.SetRegionTop = "0"; //용지 사이즈
            ScanSetType.SetRegionWidth = "0"; //용지 사이즈
            ScanSetType.SetRegionHeight = "0"; //용지 사이즈
            ScanSetType.SetFileFormat = "0"; //저장시 파일 포맷(png)
            ScanSetType.SetFileAppendFlag = "0"; //TIF,JPG,PDF: 0 // BMP,PNG,GIF 설정안함
            ScanSetType.SetJpegQuality = "0"; //TIF,JPG,PDF: 76 // BMP,PNG,GIF 설정안함
            ScanSetType.SetHideUI = "0"; //Progress Hide : 0
            ScanSetType.SetIndicators = "0"; //Progress Hide : 0
            ScanSetType.SetAutoDeskew = "1"; //Deskew :1
            ScanSetType.SetBlankSize = "1000"; //빈용지 사이즈 : 1
            ScanSetType.SetFront = "0"; //0도
            ScanSetType.SetRear = "0"; //0도
        }

        /// <summary>
        /// 스캐너 설정을 불러온다
        /// </summary>
        public static void ReadScanSet()
        {
            ScanSetType.SourceName = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SourceName");
            ScanSetType.SelectFeeder = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SelectFeeder");
            ScanSetType.EnableDuplex = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "EnableDuplex");
            ScanSetType.SetPixelType = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetPixelType");
            //        .SetBitDepth = ReadIni("SCANSET", "SetBitDepth")
            ScanSetType.SetResolution = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetResolution");
            ScanSetType.SetPaperSize = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetPaperSize");
            ScanSetType.SetBlankSize = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetBlankSize");
            ScanSetType.SetFront = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetFront");
            ScanSetType.SetRear = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRear");


            //ScanSetType.SetRegionLeft = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRegionLeft");
            //ScanSetType.SetRegionTop = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRegionTop");
            //ScanSetType.SetRegionWidth = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRegionWidth");
            //ScanSetType.SetRegionHeight = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRegionHeight");
            //ScanSetType.SetFileFormat = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetFileFormat");
            //ScanSetType.SetFileAppendFlag = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetFileAppendFlag");
            //ScanSetType.SetJpegQuality = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetJpegQuality");
            //ScanSetType.SetHideUI = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetHideUI");
            //ScanSetType.SetIndicators = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetIndicators");
            //ScanSetType.SetAutoDeskew = ComFunc.ReadINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetAutoDeskew");
        }

        /// <summary>
        /// 스캐너 설정을 저장한다
        /// </summary>
        public static void WriteScanSet()
        {
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SourceName", clsScanSet.ScanSetType.SourceName);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SelectFeeder", clsScanSet.ScanSetType.SelectFeeder);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "EnableDuplex", clsScanSet.ScanSetType.EnableDuplex);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetPixelType", clsScanSet.ScanSetType.SetPixelType);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetResolution", clsScanSet.ScanSetType.SetResolution);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetPaperSize", clsScanSet.ScanSetType.SetPaperSize);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetBlankSize", clsScanSet.ScanSetType.SetBlankSize);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetFront", clsScanSet.ScanSetType.SetFront);
            ComFunc.WriteINI(@"C:\PSMHEXE\SCANEZTWAINSET.INI", "SCANSET", "SetRear", clsScanSet.ScanSetType.SetRear);
        }
    }
}
