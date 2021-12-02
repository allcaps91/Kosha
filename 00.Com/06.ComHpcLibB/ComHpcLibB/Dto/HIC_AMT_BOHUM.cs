namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_AMT_BOHUM : BaseDto
    {
        
        /// <summary>
        /// 적용일자
        /// </summary>
		public string SDATE { get; set; } 

        /// <summary>
        /// 1차 진찰및상담
        /// </summary>
		public long ONE_AMT01 { get; set; } 

        /// <summary>
        /// 1차 흉부방사선검사
        /// </summary>
		public long ONE_AMT02 { get; set; } 

        /// <summary>
        /// 1차 요검사
        /// </summary>
		public long ONE_AMT03 { get; set; } 

        /// <summary>
        /// 1차 혈색소
        /// </summary>
		public long ONE_AMT04 { get; set; } 

        /// <summary>
        /// 1차 식전혈당
        /// </summary>
		public long ONE_AMT05 { get; set; } 

        /// <summary>
        /// 1차 총콜레스테롤
        /// </summary>
		public long ONE_AMT06 { get; set; } 

        /// <summary>
        /// 1차 HDL콜레스테롤
        /// </summary>
		public long ONE_AMT07 { get; set; } 

        /// <summary>
        /// 1차 트리글리세라이드
        /// </summary>
		public long ONE_AMT08 { get; set; } 

        /// <summary>
        /// 1차 AST(SGOT)
        /// </summary>
		public long ONE_AMT09 { get; set; } 

        /// <summary>
        /// 1차 ALT(SGPT)
        /// </summary>
		public long ONE_AMT10 { get; set; } 

        /// <summary>
        /// 1차 감마지티피
        /// </summary>
		public long ONE_AMT11 { get; set; } 

        /// <summary>
        /// 1차 혈청크레아티닌
        /// </summary>
		public long ONE_AMT12 { get; set; } 

        /// <summary>
        /// 1차 간염항원일반
        /// </summary>
		public long ONE_AMT13 { get; set; } 

        /// <summary>
        /// 1차 간염항원정밀
        /// </summary>
		public long ONE_AMT14 { get; set; } 

        /// <summary>
        /// 1차 간염항체일반
        /// </summary>
		public long ONE_AMT15 { get; set; } 

        /// <summary>
        /// 1차 간염항체정밀
        /// </summary>
		public long ONE_AMT16 { get; set; } 

        /// <summary>
        /// 1차 양방사선골밀도
        /// </summary>
		public long ONE_AMT17 { get; set; } 

        /// <summary>
        /// 1차 양방사선말단골밀도
        /// </summary>
		public long ONE_AMT18 { get; set; } 

        /// <summary>
        /// 1차 적량적전산화단층
        /// </summary>
		public long ONE_AMT19 { get; set; } 

        /// <summary>
        /// 1차 초음파골밀도
        /// </summary>
		public long ONE_AMT20 { get; set; } 

        /// <summary>
        /// 1차 노인신체기능검사
        /// </summary>
		public long ONE_AMT21 { get; set; } 

        /// <summary>
        /// 1차 LDL콜레스테롤(추가분)
        /// </summary>
		public long ONE_AMT22 { get; set; } 

        /// <summary>
        /// 1차 토.공휴일가산료
        /// </summary>
		public long ONE_AMT23 { get; set; } 

        /// <summary>
        /// <여유분>
        /// </summary>
		public long ONE_AMT24 { get; set; } 

        /// <summary>
        /// <여유분>
        /// </summary>
		public long ONE_AMT25 { get; set; } 

        /// <summary>
        /// 1차 구강진찰료
        /// </summary>
		public long ONE_DENT1 { get; set; } 

        /// <summary>
        /// 1차 구강진찰료(휴일가산)
        /// </summary>
		public long ONE_DENT2 { get; set; } 

        /// <summary>
        /// 2차 건강진단 결과 사후상담
        /// </summary>
		public long TWO_AMT01 { get; set; } 

        /// <summary>
        /// 2차 식전혈당(혈액화학분석기)
        /// </summary>
		public long TWO_AMT02 { get; set; } 

        /// <summary>
        /// 2차 식전혈당(자가혈당측정기)
        /// </summary>
		public long TWO_AMT03 { get; set; } 

        /// <summary>
        /// 2차 기본(상담)
        /// </summary>
		public long TWO_AMT04 { get; set; } 

        /// <summary>
        /// 2차 흡연
        /// </summary>
		public long TWO_AMT05 { get; set; } 

        /// <summary>
        /// 2차 음주
        /// </summary>
		public long TWO_AMT06 { get; set; } 

        /// <summary>
        /// 2차 운동
        /// </summary>
		public long TWO_AMT07 { get; set; } 

        /// <summary>
        /// 2차 영양
        /// </summary>
		public long TWO_AMT08 { get; set; } 

        /// <summary>
        /// 2차 비만
        /// </summary>
		public long TWO_AMT09 { get; set; } 

        /// <summary>
        /// 2차 우울증(CES-D)
        /// </summary>
		public long TWO_AMT10 { get; set; } 

        /// <summary>
        /// 2차 우울증(GDS)
        /// </summary>
		public long TWO_AMT11 { get; set; } 

        /// <summary>
        /// 2차 인지기능검사(치매)(KDSQ-C)
        /// </summary>
		public long TWO_AMT12 { get; set; } 

        /// <summary>
        /// 2차 토.공휴일가산
        /// </summary>
		public long TWO_AMT13 { get; set; } 

        /// <summary>
        /// <여유분>
        /// </summary>
		public long TWO_AMT14 { get; set; } 

        /// <summary>
        /// <여유분>
        /// </summary>
		public long TWO_AMT15 { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 공단청구 기준금액
        /// </summary>
        public HIC_AMT_BOHUM()
        {
        }
    }
}
