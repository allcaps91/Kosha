using System.Drawing;
using System.Windows.Forms;


namespace ComEmrBase
{
    public class clsEmrType
    {
        //EMR 서버, 클라이언트 정보
        public struct EmrSvrInfo
        {
            public static string EmrClient;                         //프로그램 기본 위치
            public static string EmrFtpSvrIp;                       //
            public static string EmrFtpPort;                        //
            public static string EmrFtpUser;                        //
            public static string EmrFtpPasswd;                      //
            public static string EmrFtpHomePath;                    //
            public static string EmrFtpPath;                        //
            public static string EmrNetServer;                      //네트워크 드라이브 접근용
            public static string EmrNetDrivePath;                   //네트워크 드라이브 접근용
            public static string EmrNetDriveID;                     //네트워크 드라이브 접근용
            public static string EmrNetDrivePW;                     //네트워크 드라이브 접근용
            public static string EmrManualUrl;                      //
        }


        public static void InitEmrSvrInfo()
        {
            EmrSvrInfo.EmrClient = @"C:\HealthSoft\TextEmr";           //프로그램 기본 위치
            EmrSvrInfo.EmrFtpSvrIp = "192.168.100.33";              //
            EmrSvrInfo.EmrFtpPort = "21";                           //
            EmrSvrInfo.EmrFtpUser = "emr";                          //
            EmrSvrInfo.EmrFtpPasswd = "emr1234";                    //
            EmrSvrInfo.EmrFtpHomePath = "/emr1";                    //
            EmrSvrInfo.EmrFtpPath = "/emr1/mtsemr/EmrImage";        //
            EmrSvrInfo.EmrNetServer = "";                           //네트워크 드라이브 접근용
            EmrSvrInfo.EmrNetDrivePath = "";                        //네트워크 드라이브 접근용
            EmrSvrInfo.EmrNetDriveID = "";                          //네트워크 드라이브 접근용
            EmrSvrInfo.EmrNetDrivePW = "";                          //네트워크 드라이브 접근용
            EmrSvrInfo.EmrManualUrl = "";                           //
        }

        /// <summary>
        /// 기록지의 이미지 내용을 저장하기 위해서 선언함:Old 서식지
        /// </summary>
        public struct EmrXmlImage
        {
            public Control Pic;
            public string ContNm;
            public string ImageNo;
        }

        /// <summary>
        /// 차트작성시 이미지 초기값 저장
        /// </summary>
        public struct EmrXmlImageInit
        {
            public Control Pic;
            public Image Img;
            public string ContNm;
            public string ImageNo;
        }

        public enum OrdAct
        {
            Dsp_Chk = 0,                        //0
            Dsp_ROOM_NO,                        //1
            PATIENT_NAME,                       //2
            Dsp_PATIENT_NO,                     //3
            DspORDER_DATE,                      //4
            ITEM_NAME,                          //5
            REMARK,                             //6
            QTY_OF_ORDER,                       //7
            DOSAGE_OF_ORDER,                    //8
            FREQUENCY_OF_ORDER,                 //9
            EXAM_SPECIMEN,                      //10
            VALUE_OF_FREQUENCY,                 //11
            DURATION_OF_ORDER,                  //12
            Dsp_ACTINFO,                        //13
            MEDDEPTCD,                          //14
            DR_NAME,                            //15
            ORDER_KEY,                          //16
            DRUG_CLASS,                         //17
            DOSAGE_CODE,                        //18
            PATIENT_NO,                         //19
            ROOM_NO,                            //20
            ORDER_DATE,                         //21
            INPUT_STATUS,                       //22
            ITEM_CODE,                          //23
            PMPA_CODE,                          //24
            ORDER_CHECK,                        //25
            ORDER_COUNT,                        //26
            PRNOldKey,                          //27
            PRN_Remark,                         //28
            WRITE_DATE                          //29
        }

        public enum ChartView
        {
            GB = 0,         //구분
            CHART ,         //차트 내용
            C_UPDATE,       //차트 수정
            C_IMPORT,       //차트 중요차트 등록 해제
            C_IMP_REMARK ,  //차트 주석
            CHARTUSENAME ,  //차트작성자명
            PTNO,           //등록번호
            ACPNO,          //접수번호
            INOUTCLS,       //외래 입원
            MEDFRDATE,      //내원 입원일자
            MEDDEPTCD,      //진료과 코드
            MEDDEPTNAME,    //진료과 명
            MEDDRCD,        //의사코드    
            MEDDRNAME,      //의사명
            EMRNO,          //차트 작성 시퀀스
            CHARTUSEID,     //차트작성자id
            FORMNO,         //기록지번호
            UPDATENO,       //기록지 변경 순번
            CHARTDATE,      //차트작성일자
            CHARTTIME,      //차트 작성시간
            ORDDATE,        //처방일자
            ORDTIME,        //처방시간
            ORDNO,          //처방번호
            ORDSEQNO,       //처방일련번호
            ORDGB,          //처방구분
            GB_HIDDEN,      //구분 숨김
            HEADYN          //Head 여부
        }
    }
}
