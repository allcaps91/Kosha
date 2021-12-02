using ComLibB;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>환자공통정보</summary>
    /// <remarks>2017.03.28.김홍록:현재는 진료지원공통이지만 차후 전역 공통으로 이관 예정</remarks>
    public partial class FrmComSupLib_PtInfo : Form
    {
        /// <summary>환자공통정보</summary>
        public FrmComSupLib_PtInfo()
        {
            InitializeComponent();
        }

        clsMethod method = new clsMethod();

        //ResourceManager rm = Properties.Resources.ResourceManager;
        Bitmap b;

        enum enmOPD_MASTER_MAIN {BDATE, PANO,    SNAME,   AGE, SEX, BI,  BI_NM,   AUTO_CHK,    SECRET,  GBDRG,   GBIO,    DEPT,    WRCODE,  DRNM,    INDATE,  VIP, BLACK,   GAMEK,   SINGA,   INFECT,  GBSMS,   OPDATE,  ILLS,    INFE_IMG,    ABO, CVR, FALL,    BRADEN,  FIRE,    PNEUMONIA,   BRAIN,   AMI, OP_JIPYO,    CONTRAST,    AST, ALLEGY,  ADR, NST, HEIGHT,  WEIGHT,  PREGNANT};

        /// <summary>환자공통정보</summary>
        /// <param name="empNo">이용자사번</param>
        /// <param name="gbIo">진료구분(입원, 외래, 응급)</param>
        /// <param name="bDate">발생일자(조회하고자 하는 일자)</param>
        /// <param name="Ptno">환자번호</param>
        /// <param name="DeptCode">진료과</param>
        public void set_DisPlay(string empNo, string gbIo, string bDate, string Ptno, string DeptCode)
        {


            DataTable dt = sel_DB(gbIo, bDate, Ptno, DeptCode);
         
            if (dt != null && dt.Rows.Count > 0)
            {
                set_Spd(dt);

                //TODO : 2017.04.04.김홍록 : 권한에 의한 화면 조절
                //set_Restriction(empNo);
            }


        }

        void set_Spd(DataTable dt)
        {

            if (dt  == null || dt.Rows.Count < 1)
            {
                this.spd_PtInfo.Sheets[0].Cells[0, 1].Value = string.Empty;  // 환자번호
                this.spd_PtInfo.Sheets[0].Cells[0, 4].Value = string.Empty;  // 환자명
                this.spd_PtInfo.Sheets[0].Cells[0, 7].Value = string.Empty;  // 나이
                this.spd_PtInfo.Sheets[0].Cells[0, 8].Value = string.Empty;  // 보험유형
                this.spd_PtInfo.Sheets[0].Cells[1, 7].Value = string.Empty;  // 성별
                this.spd_PtInfo.Sheets[0].Cells[1, 8].Value = string.Empty;  // 중증
                this.spd_PtInfo.Sheets[0].Cells[1, 10].Value = string.Empty; // 후불
                this.spd_PtInfo.Sheets[0].Cells[1, 11].Value = string.Empty; // 개인정보여부
                this.spd_PtInfo.Sheets[0].Cells[1, 12].Value = string.Empty; // 사생활
                this.spd_PtInfo.Sheets[0].Cells[1, 13].Value = string.Empty; // DRG
                this.spd_PtInfo.Sheets[0].Cells[1, 14].Value = string.Empty; // 혈액형
                this.spd_PtInfo.Sheets[0].Cells[1, 15].Value = string.Empty; // 키
                this.spd_PtInfo.Sheets[0].Cells[1, 16].Value = string.Empty; // 체중
                this.spd_PtInfo.Sheets[0].Cells[1, 17].Value = string.Empty; // VIP
                this.spd_PtInfo.Sheets[0].Cells[1, 18].Value = string.Empty; // BLACK
                this.spd_PtInfo.Sheets[0].Cells[1, 19].Value = string.Empty; // Family
                this.spd_PtInfo.Sheets[0].Cells[1, 20].Value = string.Empty; // 가톨릭
                this.spd_PtInfo.Sheets[0].Cells[1, 21].Value = string.Empty; // 산모여부
                this.spd_PtInfo.Sheets[0].Cells[2, 1].Value = null;          // 감염이미지
                this.spd_PtInfo.Sheets[0].Cells[3, 9].Value = string.Empty;  // 낙상
                this.spd_PtInfo.Sheets[0].Cells[3, 10].Value = string.Empty; // 욕창
                this.spd_PtInfo.Sheets[0].Cells[3, 11].Value = string.Empty; // 화재
                this.spd_PtInfo.Sheets[0].Cells[3, 12].Value = string.Empty; // 폐렴
                this.spd_PtInfo.Sheets[0].Cells[3, 13].Value = string.Empty; // 뇌졸증
                this.spd_PtInfo.Sheets[0].Cells[3, 14].Value = string.Empty; // AMI
                this.spd_PtInfo.Sheets[0].Cells[3, 15].Value = string.Empty; // 예방항생제
                this.spd_PtInfo.Sheets[0].Cells[3, 16].Value = string.Empty; // 조영제
                this.spd_PtInfo.Sheets[0].Cells[3, 17].Value = string.Empty; // 항생제반응
                this.spd_PtInfo.Sheets[0].Cells[3, 18].Value = string.Empty; // 알러지
                this.spd_PtInfo.Sheets[0].Cells[3, 19].Value = string.Empty; // ADR
                this.spd_PtInfo.Sheets[0].Cells[3, 20].Value = string.Empty; // NST
                this.spd_PtInfo.Sheets[0].Cells[3, 21].Value = string.Empty; // CVR
                this.spd_PtInfo.Sheets[0].Cells[4, 1].Value = string.Empty;  // 외래/입원/응급
                this.spd_PtInfo.Sheets[0].Cells[4, 3].Value = string.Empty;  // 과
                this.spd_PtInfo.Sheets[0].Cells[4, 4].Value = string.Empty;  // 의사
                this.spd_PtInfo.Sheets[0].Cells[4, 9].Value = string.Empty;  // 진단명
                this.spd_PtInfo.Sheets[0].Cells[4, 21].Value = string.Empty; // 입원일
                this.spd_PtInfo.Sheets[0].Cells[5, 1].Value = string.Empty;  // 병동/병실/병상
                this.spd_PtInfo.Sheets[0].Cells[5, 4].Value = string.Empty;  // HOD / POD
                this.spd_PtInfo.Sheets[0].Cells[5, 9].Value = string.Empty;  // 수술명
                this.spd_PtInfo.Sheets[0].Cells[5, 21].Value = string.Empty; // 수술일자
            }
            else
            {

                this.spd_PtInfo.Sheets[0].Cells[0, 1].Value = dt.Rows[0][enmOPD_MASTER_MAIN.PANO.ToString()].ToString();            // 환자번호
                this.spd_PtInfo.Sheets[0].Cells[0, 4].Value = dt.Rows[0][enmOPD_MASTER_MAIN.SNAME.ToString()].ToString();           // 환자명
                this.spd_PtInfo.Sheets[0].Cells[0, 7].Value = dt.Rows[0][enmOPD_MASTER_MAIN.AGE.ToString()].ToString();             // 나이
                this.spd_PtInfo.Sheets[0].Cells[0, 8].Value = dt.Rows[0][enmOPD_MASTER_MAIN.BI_NM.ToString()].ToString();           // 보험유형
                this.spd_PtInfo.Sheets[0].Cells[1, 7].Value = dt.Rows[0][enmOPD_MASTER_MAIN.SEX.ToString()].ToString();             // 성별
                this.spd_PtInfo.Sheets[0].Cells[1, 8].Value = string.Empty;                                                         // TODO : 중증 
                this.spd_PtInfo.Sheets[0].Cells[1, 10].Value = dt.Rows[0][enmOPD_MASTER_MAIN.AUTO_CHK.ToString()].ToString();       // 후불
                this.spd_PtInfo.Sheets[0].Cells[1, 11].Value = dt.Rows[0][enmOPD_MASTER_MAIN.GBSMS.ToString()].ToString();          // 개인정보여부
                this.spd_PtInfo.Sheets[0].Cells[1, 12].Value = dt.Rows[0][enmOPD_MASTER_MAIN.SECRET.ToString()].ToString();         // 사생활
                this.spd_PtInfo.Sheets[0].Cells[1, 13].Value = dt.Rows[0][enmOPD_MASTER_MAIN.GBDRG.ToString()].ToString();          // DRG
                this.spd_PtInfo.Sheets[0].Cells[1, 14].Value = dt.Rows[0][enmOPD_MASTER_MAIN.ABO.ToString()].ToString();            // 혈액형
                this.spd_PtInfo.Sheets[0].Cells[1, 15].Value = dt.Rows[0][enmOPD_MASTER_MAIN.HEIGHT.ToString()].ToString();         // 키
                this.spd_PtInfo.Sheets[0].Cells[1, 16].Value = dt.Rows[0][enmOPD_MASTER_MAIN.WEIGHT.ToString()].ToString();         // 체중
                this.spd_PtInfo.Sheets[0].Cells[1, 17].Value = dt.Rows[0][enmOPD_MASTER_MAIN.VIP.ToString()].ToString();            // VIP
                this.spd_PtInfo.Sheets[0].Cells[1, 18].Value = dt.Rows[0][enmOPD_MASTER_MAIN.BLACK.ToString()].ToString();          // BLACK
                this.spd_PtInfo.Sheets[0].Cells[1, 19].Value = dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString();          // Family
                this.spd_PtInfo.Sheets[0].Cells[1, 20].Value = dt.Rows[0][enmOPD_MASTER_MAIN.SINGA.ToString()].ToString();          // 가톨릭
                this.spd_PtInfo.Sheets[0].Cells[1, 21].Value = dt.Rows[0][enmOPD_MASTER_MAIN.PREGNANT.ToString()].ToString();       // 산모여부
                

                string img = "_" + dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString();
                b = (Bitmap)rm.GetObject(img);                                                                                                  
                this.spd_PtInfo.Sheets[0].Cells[2, 1].Value = b;                                                                    // 감염이미지
                this.spd_PtInfo.Sheets[0].Cells[3, 9].Value = dt.Rows[0][enmOPD_MASTER_MAIN.FALL.ToString()].ToString();            // 낙상
                this.spd_PtInfo.Sheets[0].Cells[3, 10].Value = dt.Rows[0][enmOPD_MASTER_MAIN.BRADEN.ToString()].ToString();         // 욕창
                this.spd_PtInfo.Sheets[0].Cells[3, 11].Value = dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString();           // 화재
                this.spd_PtInfo.Sheets[0].Cells[3, 12].Value = dt.Rows[0][enmOPD_MASTER_MAIN.PNEUMONIA.ToString()].ToString();      // 폐렴
                this.spd_PtInfo.Sheets[0].Cells[3, 13].Value = dt.Rows[0][enmOPD_MASTER_MAIN.BRAIN.ToString()].ToString();          // 뇌졸증
                this.spd_PtInfo.Sheets[0].Cells[3, 14].Value = dt.Rows[0][enmOPD_MASTER_MAIN.AMI.ToString()].ToString();            // AMI
                this.spd_PtInfo.Sheets[0].Cells[3, 15].Value = dt.Rows[0][enmOPD_MASTER_MAIN.OP_JIPYO.ToString()].ToString();       // 예방항생제
                this.spd_PtInfo.Sheets[0].Cells[3, 16].Value = dt.Rows[0][enmOPD_MASTER_MAIN.CONTRAST.ToString()].ToString();       // 조영제
                this.spd_PtInfo.Sheets[0].Cells[3, 17].Value = dt.Rows[0][enmOPD_MASTER_MAIN.AST.ToString()].ToString();            // 항생제반응
                this.spd_PtInfo.Sheets[0].Cells[3, 18].Value = dt.Rows[0][enmOPD_MASTER_MAIN.ALLEGY.ToString()].ToString();         // 알러지
                this.spd_PtInfo.Sheets[0].Cells[3, 19].Value = dt.Rows[0][enmOPD_MASTER_MAIN.ADR.ToString()].ToString();            // ADR
                this.spd_PtInfo.Sheets[0].Cells[3, 20].Value = dt.Rows[0][enmOPD_MASTER_MAIN.NST.ToString()].ToString();            // NST
                this.spd_PtInfo.Sheets[0].Cells[3, 21].Value = dt.Rows[0][enmOPD_MASTER_MAIN.CVR.ToString()].ToString();            // CVR
                this.spd_PtInfo.Sheets[0].Cells[4, 1].Value = dt.Rows[0][enmOPD_MASTER_MAIN.GBIO.ToString()].ToString();            // 외래/입원/응급
                this.spd_PtInfo.Sheets[0].Cells[4, 3].Value = dt.Rows[0][enmOPD_MASTER_MAIN.DEPT.ToString()].ToString();            // 과
                this.spd_PtInfo.Sheets[0].Cells[4, 4].Value = dt.Rows[0][enmOPD_MASTER_MAIN.DRNM.ToString()].ToString();            // 의사
                this.spd_PtInfo.Sheets[0].Cells[4, 9].Value = dt.Rows[0][enmOPD_MASTER_MAIN.ILLS.ToString()].ToString();            // 진단명
                this.spd_PtInfo.Sheets[0].Cells[4, 21].Value = dt.Rows[0][enmOPD_MASTER_MAIN.INDATE.ToString()].ToString();         // 입원일
                this.spd_PtInfo.Sheets[0].Cells[5, 1].Value = dt.Rows[0][enmOPD_MASTER_MAIN.WRCODE.ToString()].ToString();          // 병동/병실/병상
                this.spd_PtInfo.Sheets[0].Cells[5, 4].Value = string.Empty;                                                         // TODO :  HOD / POD
                this.spd_PtInfo.Sheets[0].Cells[5, 9].Value = string.Empty;                                                         // TODO :  수술명
                this.spd_PtInfo.Sheets[0].Cells[5, 21].Value = dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString();         // 수술일자
            }

            this.spd_PtInfo.Sheets[0].Cells[2, 1].Value = b;
            this.spd_PtInfo.Sheets[1].Cells[1, 1].Value = b;

        }        

        DataTable sel_DB(string gbIo, string bDate, string Ptno, string DeptCode)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;

            sql = "";
            sql =sql + "  SELECT                                                                                                                                        \r\n";
            sql =sql + "         OPD.PANO                                                                                  AS PANO     -- 01 환자번호                   \r\n";
            sql =sql + "       , OPD.SNAME                                                                                 AS SNAME    -- 02 환자성명                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(OPD.PANO), TRUNC(SYSDATE))          AS AGE      -- 03 나이                       \r\n";
            sql =sql + "       , OPD.SEX                                                                                   AS SEX      -- 04 성별                       \r\n";
            sql =sql + "       , OPD.BI                                                                                    AS BI       -- 05 BI                         \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BI_NM(OPD.BI)                                                               AS BI_NM    -- 06 보험유형                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(OPD.PANO, TRUNC(OPD.BDATE))                                AS AUTO_CHK -- 07 후불여부                   \r\n";
            sql =sql + "       , ''                                                                                        AS SECRET   -- 08 사생활                     \r\n";
            sql =sql + "       , ''                                                                                        AS GBDRG    -- 09 DRG여부                    \r\n";
            sql =sql + "       , '외래'                                                                                    AS GBIO     -- 10 진료구분                   \r\n";
            sql =sql + "       , OPD.DEPTCODE                                                                              AS DEPT     -- 11 진료과                     \r\n";
            sql =sql + "       , ''                                                                                        AS WRCODE   -- 12 병동병실                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(OPD.DRCODE)                                               AS DRNM     -- 13 지정의                     \r\n";
            sql =sql + "       , OPD.BDATE                                                                                 AS INDATE   -- 14 입원일자, 외래는 발생일자  \r\n";
            sql =sql + "       , BAS.GB_VIP                                                                                AS VIP      -- 15 VIP                        \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BAS_OCSMEMO_CHK(OPD.PANO, TRUNC(SYSDATE))                                   AS BLACK    -- 16 문제환자                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BAS_GAMCODE_NM(OPD.GBGAMEK)                                                 AS GAMEK    -- 17 감액정보                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_BAS_GAMFSINGA_CHK(OPD.PANO)                                                 AS SINGA    -- 18 가톨릭                     \r\n";
            sql =sql + "       , ''                                                                                        AS INFECT   -- 19 보호(격리)                 \r\n";
            sql =sql + "       , BAS.GBSMS                                                                                 AS GBSMS    -- 20 개인정보동의서             \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(OPD.PANO)                                                AS OPDATE   -- 21 수술일자                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_OCS_ILLS(OPD.PANO, 'I', TRUNC(OPD.BDATE), OPD.DEPTCODE, NULL)               AS ILLS     -- 22 ILL                        \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(OPD.PANO, TRUNC(OPD.BDATE))                          AS INFE_IMG -- 23 감염이미지                 \r\n";
            sql =sql + "	   , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(OPD.PANO)                                             AS ABO      -- 24 ABO                        \r\n";
            sql =sql + "       , ''                                                                                        AS CVR      -- 25 CVR                        \r\n";  // TODO : CVR
            sql =sql + "       , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(OPD.PANO, TRUNC(OPD.BDATE), 0, '0')                     AS FALL     -- 26 낙상                       \r\n";
            sql =sql + "       , ''                                                                                        AS BRADEN   -- 27 욕창                       \r\n";
            sql =sql + "       , ''                                                                                        AS FIRE     -- 28 화재                       \r\n";
            sql =sql + "       , ''                                                                                        AS PNEUMONIA-- 29 폐렴                       \r\n";  // TODO : 폐렴
            sql =sql + "       , ''                                                                                        AS BRAIN    -- 30 뇌졸증                     \r\n";  // TODO : 뇌졸증
            sql =sql + "       , ''                                                                                        AS AMI      -- 31 AMI                        \r\n";  // TODO : AMI
            sql =sql + "       , ''                                                                                        AS OP_JIPYO -- 32 수술예방항생제             \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_XRAY_CONTRAST_CHK(OPD.PANO)                                                 AS CONTRAST -- 33 조영제부작용환자           \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_NUR_AST_CHK(OPD.PANO)                                                       AS AST      -- 34 항생반응                   \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(OPD.PANO)                                               AS ALLEGY   -- 35 알러지                     \r\n";
            sql =sql + "       , KOSMOS_OCS.FC_DRUG_ADR_CHK(OPD.PANO)                                                      AS ADR      -- 36 ADR                        \r\n";
            sql =sql + "       , ''                                                                                        AS NST      -- 37 NST                        \r\n";       
            sql =sql + "       , ''                                                                                        AS HEIGHT   -- 38 키                         \r\n";  // TODO : Height
            sql =sql + "       , ''                                                                                        AS WEIGHT   -- 39 몸무게                     \r\n";  // TODO : Weight
            sql =sql + "       , OPD.PREGNANT                                                                              AS PREGNANT -- 40 산모여부                   \r\n";
            sql =sql + "  FROM KOSMOS_PMPA.OPD_MASTER  OPD                                                                                                              \r\n";
            sql =sql + "     , KOSMOS_PMPA.BAS_PATIENT BAS                                                                                                              \r\n";
            sql =sql + " WHERE 1 = 1                                                                                                                                    \r\n";
            sql =sql + "   AND '" + gbIo + "' IN ('O', 'E')                                                                                                             \r\n";
            sql = sql + "  AND OPD.PANO     = " + ComFunc.covSqlstr(Ptno, false);
            sql =sql + "   AND OPD.PANO     = BAS.PANO                                                                                                                  \r\n ";
            sql = sql + "  AND OPD.BDATE    = " + ComFunc.covSqlDate(bDate, false);
            sql = sql + "  AND OPD.DEPTCODE = " + ComFunc.covSqlstr(DeptCode, false);
            sql =sql + " UNION ALL                                                                                                                                      \r\n ";
            sql =sql + " SELECT                                                                                                                                         \r\n ";
            sql =sql + "        IPD.PANO                                                                                  AS PANO     -- 01 환자번호         \r\n ";
            sql =sql + "      , IPD.SNAME                                                                                 AS SNAME    -- 02 환자성명         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(IPD.PANO), TRUNC(SYSDATE))          AS AGE      -- 03 나이             \r\n ";
            sql =sql + "      , IPD.SEX                                                                                   AS SEX      -- 04 성별             \r\n ";
            sql =sql + "      , IPD.BI                                                                                    AS BI       -- 05 BI               \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BI_NM(IPD.BI)                                                               AS BI_NM    -- 06 보험유형         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(IPD.PANO, TRUNC(IPD.INDATE))                               AS AUTO_CHK -- 07 후불여부         \r\n ";
            sql =sql + "      , IPD.SECRET                                                                                AS SECRET   -- 08 사생활           \r\n ";
            sql =sql + "      , IPD.GBDRG                                                                                 AS GBDRG    -- 09 DRG여부          \r\n ";
            sql =sql + "      , '입원'                                                                                    AS GBIO     -- 10 진료구분         \r\n ";
            sql =sql + "      , IPD.DEPTCODE                                                                              AS DEPT     -- 11 진료과           \r\n ";
            sql =sql + "      , IPD.WARDCODE || '/' || IPD.ROOMCODE || DECODE(IPD.BEDNUM, NULL, '', '/' || IPD.BEDNUM)    AS WRCODE   -- 12 병동병실         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(IPD.DRCODE)                                               AS DRNM     -- 13 지정의           \r\n ";
            sql =sql + "      , IPD.INDATE                                                                                AS INDATE   -- 14 입원일자         \r\n ";
            sql =sql + "      , BAS.GB_VIP                                                                                AS VIP      -- 15 VIP              \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BAS_OCSMEMO_CHK(IPD.PANO, TRUNC(IPD.INDATE))                                AS BLACK    -- 16 문제환자         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BAS_GAMCODE_NM(IPD.GBGAMEK)                                                 AS GAMEK    -- 17 감액정보         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_BAS_GAMFSINGA_CHK(IPD.PANO)                                                 AS SINGA    -- 18 가톨릭           \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_IPD_TRANSFOR_INFECT_CHK(IPD.PANO, IPD.IPDNO)                                AS INFECT   -- 19 보호(격리)       \r\n ";
            sql =sql + "      , BAS.GBSMS                                                                                 AS GBSMS    -- 20 개인정보동의서   \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(IPD.PANO)                                                AS OPDATE   -- 21 수술일자         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_OCS_ILLS(IPD.PANO, 'I', TRUNC(IPD.INDATE), IPD.DEPTCODE, NULL)              AS ILLS     -- 22 ILL              \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(IPD.PANO, TRUNC(IPD.INDATE))                         AS INFE_IMG -- 23 감염이미지       \r\n ";
            sql =sql + "	  , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(IPD.PANO)                                             AS ABO      -- 24 ABO              \r\n ";
            sql =sql + "      , ''                                                                                        AS CVR      -- 25 CVR              \r\n "; // TODO : CVR
            sql =sql + "      , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(IPD.PANO, TRUNC(IPD.INDATE), IPD.IPDNO, 'I')            AS FALL     -- 26 낙상             \r\n ";
            sql =sql + "      , ''                                                                                        AS BRADEN   -- 27 욕창             \r\n "; // TODO : 욕창
            sql =sql + "      , KOSMOS_OCS.FC_NUR_MASTER_FIRE(IPD.IPDNO)                                                  AS FIRE     -- 28 화재             \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_NUR_MASTER_pneumonia(IPD.IPDNO)                                             AS PNEUMONIA-- 29 폐렴             \r\n "; // TODO : 폐렴
            sql =sql + "      , KOSMOS_OCS.FC_IPD_TRANS_BRAIN(IPD.IPDNO)                                                  AS BRAIN    -- 30 뇌졸증           \r\n "; // TODO : 뇌졸증
            sql =sql + "      , KOSMOS_OCS.FC_NUR_MASTER_AMI(IPD.IPDNO)                                                   AS AMI      -- 31 AMI              \r\n "; // TODO : AMI
            sql =sql + "      , IPD.OP_JIPYO                                                                              AS OP_JIPYO -- 32 수술예방항생제   \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_XRAY_CONTRAST_CHK(IPD.PANO)                                                 AS CONTRAST -- 33 조영제부작용환자 \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_NUR_AST_CHK(IPD.PANO)                                                       AS AST      -- 34 항생반응         \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(IPD.PANO)                                               AS ALLEGY   -- 35 알러지           \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_DRUG_ADR_CHK(IPD.PANO)                                                      AS ADR      -- 36 ADR              \r\n ";
            sql =sql + "      , KOSMOS_OCS.FC_DIET_NST_PROGRESS_CHK(IPD.IPDNO)                                            AS NST      -- 37 NST              \r\n ";
            sql =sql + "      , TO_CHAR(IPD.HEIGHT)                                                                       AS HEIGHT   -- 38 키               \r\n ";
            sql =sql + "      , TO_CHAR(IPD.WEIGHT)                                                                       AS WEIGHT   -- 39 몸무게           \r\n ";
            sql =sql + "      , IPD.PREGNANT                                                                              AS PREGNANT -- 40 산모여부         \r\n ";
            sql =sql + " FROM KOSMOS_PMPA.IPD_NEW_MASTER    IPD                                                                                              \r\n ";
            sql =sql + "    , KOSMOS_PMPA.BAS_PATIENT       BAS                                                                                              \r\n ";
            sql =sql + "WHERE 1    = 1                                                                                                                       \r\n ";
            sql = sql + "  AND '" + gbIo + "' IN ('I')                                                                                                       \r\n";
            sql = sql + " AND IPD.PANO = " + ComFunc.covSqlstr(Ptno, false);
            sql =sql + "  AND IPD.PANO = BAS.PANO                                   \r\n";
            sql =sql + "  AND IPD.OUTDATE IS NULL                                   \r\n";
            sql =sql + "  AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD')       \r\n";

            dt = clsDB.GetDataTable(sql);

            return dt;
        }

        void set_Restriction(string empNo)
        {
            // TODO : 2017.03.31.김홍록 : 사용자에 대한 보이는 옵션 제한
        }

    }
}
