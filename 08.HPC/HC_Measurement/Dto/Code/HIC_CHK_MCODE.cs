namespace HC_Measurement.Dto
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_CHK_MCODE : BaseDto
    {

        /// <summary>
        /// 물질코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 공단코드
        /// </summary>
        public string K2BCODE { get; set; }

        /// <summary>
        /// 물질명
        /// </summary>
        public string FULLNAME { get; set; }

        /// <summary>
        /// 상용명
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 상용명2
        /// </summary>
        public string NAME2 { get; set; }

        /// <summary>
        /// 영문명
        /// </summary>
        public string ENAME { get; set; }

        /// <summary>
        /// CAS 번호
        /// </summary>
        public string CAS_NO { get; set; }

        /// <summary>
        /// EC 번호
        /// </summary>
        public string EC_NO { get; set; }

        /// <summary>
        /// 분류
        /// </summary>
        public string BUN { get; set; }

        /// <summary>
        /// 분석분류
        /// </summary>
        public string ANAL_BUN { get; set; }

        /// <summary>
        /// 주기
        /// </summary>
        public long CYCLE { get; set; }

        /// <summary>
        /// TWA
        /// </summary>
        public string TWA_PPM { get; set; }

        /// <summary>
        /// TWA 단위
        /// </summary>
        public string TWA_MG { get; set; }

        /// <summary>
        /// STEL
        /// </summary>
        public string STEL_PPM { get; set; }

        /// <summary>
        /// STEL 단위
        /// </summary>
        public string STEL_MG { get; set; }

        /// <summary>
        /// Ceiling
        /// </summary>
        public string CEILING { get; set; }

        /// <summary>
        /// Ceiling 단위
        /// </summary>
        public string CEILING_UNIT { get; set; }

        /// <summary>
        /// 회수율
        /// </summary>
        public string RET_RATE { get; set; }

        /// <summary>
        /// PAS 회수율
        /// </summary>
        public string PAS_RATE { get; set; }

        /// <summary>
        /// FACTOR 값
        /// </summary>
        public string FACTOR_VAL { get; set; }

        /// <summary>
        /// 측정방법
        /// </summary>
        public string CHK_WAY { get; set; }

        /// <summary>
        /// 측정단위
        /// </summary>
        public string CHK_UNIT { get; set; }

        /// <summary>
        /// 분자량
        /// </summary>
        public string MOLECULE { get; set; }

        /// <summary>
        /// 계산상수 B(ppm)
        /// </summary>
        public long CAL_CONST { get; set; }

        /// <summary>
        /// 적용값1
        /// </summary>
        public string APPLY_VALUE1 { get; set; }

        /// <summary>
        /// 적용값2
        /// </summary>
        public string APPLY_VALUE2 { get; set; }

        /// <summary>
        /// 측정 여부
        /// </summary>
        public string GBCHK { get; set; }

        /// <summary>
        /// 특검 여부
        /// </summary>
        public string GBSPC { get; set; }

        /// <summary>
        /// 혼합물 여부
        /// </summary>
        public string GBMIX { get; set; }

        /// <summary>
        /// 발암성 여부
        /// </summary>
        public string GBCAN { get; set; }

        /// <summary>
        /// 사용 여부
        /// </summary>
        public string GBUSE { get; set; }

        /// <summary>
        /// 순번
        /// </summary>
        public long SORT { get; set; }

        /// <summary>
        /// 입력사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 입력일자
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 적용값3
        /// </summary>
        public string APPLY_VALUE3 { get; set; }

        /// <summary>
        /// Rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 건강검진 자료사전 Table
        /// </summary>
        public HIC_CHK_MCODE()
        {
        }
    }
}
