namespace ComHpcLibB.Model
{

    using ComBase.Mvc;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_RESULT_SUNAPDTL : BaseDto
    {

        /// <summary>
        /// 접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 묶음코드
        /// </summary>
        public string GROUPCODE { get; set; }

        /// <summary>
        /// 검사부서(1.신체계측 2.임상병리과 3.방사선과 4.기타검사)
        /// </summary>
        public string PART { get; set; }

        /// <summary>
        /// 검사코드
        /// </summary>
        public string EXCODE { get; set; }

        /// <summary>
        /// 검사결과분류코드(기타는 NULL)
        /// </summary>
        public string RESCODE { get; set; }

        /// <summary>
        /// 검사결과
        /// </summary>
        public string RESULT { get; set; }

        /// <summary>
        /// 검사결과판정(B.정상B,R.질환의심,NULL:정상)
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// ABC시행부서
        /// </summary>
        public string OPER_DEPT { get; set; }

        /// <summary>
        /// ABC시행의사(사번)
        /// </summary>
        public string OPER_DCT { get; set; }

        /// <summary>
        /// Acting 여부 (Y: 액팀함, Null: 안함)
        /// </summary>
        public string ACTIVE { get; set; }

        /// <summary>
        /// 작업자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 작업날짜
        /// </summary>
        public string ENTTIME { get; set; }

        /// <summary>
        /// 묶음코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 유해물질
        /// </summary>
        public string UCODE { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 부담율
        /// </summary>
        public string GBSELF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GWRTNO { get; set; }

    }
}

