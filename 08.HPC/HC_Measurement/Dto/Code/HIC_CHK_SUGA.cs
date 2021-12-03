namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHK_SUGA : BaseDto
    {
        /// <summary>
        /// 년도
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 수가코드
        /// </summary>
        public string SUCODE { get; set; }

        /// <summary>
        /// 항목명
        /// </summary>
        public string SUNAME { get; set; }

        /// <summary>
        /// 물질코드
        /// </summary>
        public string MCODE { get; set; }

        /// <summary>
        /// 물질명
        /// </summary>
        public string MCODE_NM { get; set; }

        /// <summary>
        /// 측정방법명
        /// </summary>
        public string CHKNAME { get; set; }

        /// <summary>
        /// 산정내역명
        /// </summary>
        public string CALNAME { get; set; }

        /// <summary>
        /// 계산서용
        /// </summary>
        public string ACCNAME { get; set; }

        /// <summary>
        /// 수수료
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 공단수수료
        /// </summary>
        public long GAMT { get; set; }

        /// <summary>
        /// 측정코드
        /// </summary>
        public string CHKCODE { get; set; }

        /// <summary>
        /// 공단코드
        /// </summary>
        public string GCODE { get; set; }

        /// <summary>
        /// 수가항목분류
        /// </summary>
        public string HANG1 { get; set; }

        /// <summary>
        /// 측정방법명
        /// </summary>
        public string HANG2 { get; set; }

        /// <summary>
        /// 분석방법명
        /// </summary>
        public string HANG3 { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string GBUSE { get; set; }

        /// <summary>
        /// 입력사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 입력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHK_SUGA()
        {
        }
    }
}
