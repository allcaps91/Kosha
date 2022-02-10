using System.Collections.Generic;

namespace ComEmrBase
{
    public class clsEmrPublic
    {
        /// <summary>
        /// 스프래드 Col Width
        /// </summary>
        public const int FLOWWT = 80;  //
        /// <summary>
        /// 스프래드 Row 높이를 공통으로
        /// </summary>
        public const int FLOWHT = 22;  //

        /// <summary>
        /// EMR 사용구 옵션 설정
        /// </summary>
        public static string gstrMcrAllFlag = "1";
        public static string gstrMcrAddFlag = "2";

        /// <summary>
        /// 프로그래스 상용구
        /// </summary>
        public static string gstrMcrProgFlag = string.Empty;

        /// <summary>
        /// 스캔관련
        /// </summary>
        public static string gstrSERVERPATH1 = string.Empty;
        public static string gstrSERVERPATH2 = string.Empty;

        public static string gCltImgPathS = @"C:\HealthSoft\scanimage";
        public static string gCltImgPathD = @"C:\HealthSoft\downimage";

        public static string AdmFormNo = string.Empty; //입원기록지 코드
        public static string ProgFormNo = string.Empty; //경과기록 코드
        public static string OpFormNo = string.Empty; //수술기록 코드
        public static string DishFormNo = string.Empty; //퇴원기록 코드
        public static string TrsDptFormNo = string.Empty; //전과기록 코드

        /// <summary>
        /// 폐렴저장 코드
        /// </summary>
        public static bool PuSaveYn; //

        public static frmEmrImageDrawingNew DrawingImage = null;
        //public static frmItemValueSpdReg ItemValueSpdReg = null;
        public static frmEmrProgMacro frmEmrProgMacroX = null;
        public static frmEmrChartToImage frmEmrChartToImageX = null;

        /// <summary>
        /// 기록지 확대 축소 여부
        /// </summary>
        public static string isEMRFORMZOOM = "0"; //
        /// <summary>
        /// 확대 축소시에 이미지로 변환할 경우 문제가 발생함으로....
        /// </summary>
        public static string isEMRFORMZOOMINIT = "0";   //

        /// <summary>
        /// 퇴원처방(Discharge)시 미비 체크
        /// </summary>
        public static bool CheckMibiDisch = false; //

        public static string strUserGrade = string.Empty;
        public static string strUserJik = string.Empty;

        public static List<string> GstrNurData = new List<string>();
        public static List<string> GstrNurAction = new List<string>();
        public static List<string> GstrNurResult = new List<string>();
        
        public static string GstrNurCodeDAR = string.Empty;
        public static string GstrNurResult1 = string.Empty;

        /// <summary>
        /// 챠트내역 없는 것 보지않기/보기 (값 : 보지않기 0 또는 없음,  보기 : 1)
        /// </summary>
        public static string GstrView01 = string.Empty;

        /// <summary>
        /// 전체목록 조회일때 기간 적용 여부 판단용으로 변경(2012-01-20)
        /// </summary>
        public static bool gDateSET = false;

        /// <summary>
        /// 수정 권한 여부
        /// </summary>
        public static bool ModifyCert = false;

        /// <summary>
        /// 수정권한 활성화 
        /// </summary>
        public static string GstrModify = string.Empty;

        public static string gUserGrade = string.Empty;

        /// <summary>
        /// 차트대출 사유 코드
        /// </summary>
        public static string mstrSayu = string.Empty;

        /// <summary>
        /// 차트대출 사유 내용(기타)
        /// </summary>
        public static string mstrSayuRemark = string.Empty;

        /// <summary>
        /// False: 최신순, True: 순정렬
        /// </summary>
        public static bool bOrderAsc = false;

        /// <summary>
        /// 스캔 연속보기
        /// </summary>
        public static bool bScanContinuView = false;

        /// <summary>
        /// 임상관찰 탭 인덱스
        /// </summary>
        public static int VitalTabIdx = 0;


        /// <summary>
        /// 혈액투석 탭 인덱스
        /// </summary>
        public static int HemodialysisTabIdx = 0;

        /// <summary>
        /// 임상관찰 작업하고 있는 Duty
        /// </summary>
        public static string VitalWorkDutySet = "ALL"; //ALL, NAM, DAY, EVE, NPM

        /// <summary>
        /// 수가연동 
        /// </summary>
        public static string GstrHighRisk = string.Empty;

        /// <summary>
        /// 수가연동 
        /// </summary>
        public static string GstrOP = string.Empty;
    }
}
