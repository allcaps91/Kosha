namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_XRAY_RESULT : BaseDto
    {
        
        /// <summary>
        /// 접수일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 촬영번호(YYYYMMDD+일련번호5자리)
        /// </summary>
		public string XRAYNO { get; set; } 

        /// <summary>
        /// 건진 환자마스타 번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 이름
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// rowid
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 성별(M/F)
        /// </summary>
        public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 검진종류
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 출장검진여부(Y.출장 N.내원검진)
        /// </summary>
		public string GBCHUL { get; set; } 

        /// <summary>
        /// 회사코드
        /// </summary>
		public long LTDCODE { get; set; } 

        /// <summary>
        /// 방사선 검사코드
        /// </summary>
		public string XCODE { get; set; } 

        /// <summary>
        /// 판독구분(1.1명판독 2.2명판독)
        /// </summary>
		public string GBREAD { get; set; } 

        /// <summary>
        /// 판독상태(0.미판독 1.판독중 2.판독완료 P:보류)
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 판독코드1(자료사전:XRAY_검진판정분류2019)
        /// </summary>
		public string RESULT1 { get; set; } 

        /// <summary>
        /// 판독분류1(A,B,C,D-A,D-B,...)
        /// </summary>
		public string RESULT2 { get; set; } 

        /// <summary>
        /// 판독분류명1(정상,사진불량,...)
        /// </summary>
		public string RESULT3 { get; set; } 

        /// <summary>
        /// 판독소견1
        /// </summary>
		public string RESULT4 { get; set; } 

        /// <summary>
        /// 1차 판독일자 및 시각
        /// </summary>
		public DateTime? READTIME1 { get; set; } 

        /// <summary>
        /// 1차 판독의사 사번
        /// </summary>
		public long READDOCT1 { get; set; } 

        /// <summary>
        /// 2차 판독일자 및 시각
        /// </summary>
		public DateTime? READTIME2 { get; set; } 

        /// <summary>
        /// 2차 판독의사 사번
        /// </summary>
		public long READDOCT2 { get; set; } 

        /// <summary>
        /// 판독결과지 인쇄여부(Y.인쇄 NULL:미인쇄)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 판독결과 전송여부(Y.전송 NULL.미전송)
        /// </summary>
		public string GBRESULTSEND { get; set; } 

        /// <summary>
        /// 방사선검사 취소일자
        /// </summary>
		public DateTime? DELDATE { get; set; } 

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 최종 작업 일자 및 시각
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 판독완료일자
        /// </summary>
		public DateTime? READDATE { get; set; }
        public DateTime? READDATE1 { get; set; }

        /// <summary>
        /// 판독코드2(자료사전:XRAY_검진판정분류)
        /// </summary>
        public string RESULT1_1 { get; set; } 

        /// <summary>
        /// 판독분류2(A,B,C,D-A,D-B,...)
        /// </summary>
		public string RESULT2_1 { get; set; } 

        /// <summary>
        /// 판독분류명2(정상,사진불량,...)
        /// </summary>
		public string RESULT3_1 { get; set; } 

        /// <summary>
        /// 판독소견2
        /// </summary>
		public string RESULT4_1 { get; set; } 

        /// <summary>
        /// PACS에 오더전송 여부(Y/N) - PACS 변환용
        /// </summary>
		public string GBORDER_SEND { get; set; } 

        /// <summary>
        /// PACS에 이미지 존재 여부(Y/N) - PACS 변환용
        /// </summary>
		public string GBPACS { get; set; } 

        /// <summary>
        /// 병원 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// PACS 등록번호 변환여부(Y.변경완료)
        /// </summary>
		public string GBCONV { get; set; } 

        /// <summary>
        /// 이전 xrayno
        /// </summary>
		public string XRAYNO2 { get; set; } 

        /// <summary>
        /// 회사코드2
        /// </summary>
		public string LTDCODE2 { get; set; } 

        /// <summary>
        /// 입력자
        /// </summary>
		public string INPS { get; set; } 

        /// <summary>
        /// 최초시간
        /// </summary>
		public DateTime? INPT_DT { get; set; } 

        /// <summary>
        /// 변경자
        /// </summary>
		public string UPPS { get; set; } 

        /// <summary>
        /// 변경시간
        /// </summary>
		public DateTime? UP_DT { get; set; } 

        /// <summary>
        /// 촬영자
        /// </summary>
		public string EXID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string PASTCT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string PASTCTYYYY { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string PASTCTMM { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOYN_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOICON_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOSITE_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE1_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long NUDOSIZE2_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPOSITIVE_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOTRACECHK2_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string INDICATIOCHK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string INDICATIOETC { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SISACHK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SISAETC { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN2_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN7 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN8 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN9 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOMEAN9_9 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOUNACTIVE { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPANGU { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDOPANGU2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE1_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE1_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE1_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE1_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE2_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE2_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE2_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE2_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE3_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE3_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE3_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE3_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY3 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE4_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE4_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE4_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE4_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY4 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE5_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE5_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE5_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE5_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY5 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE6_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SIZE6_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE6_1 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long GOSIZE6_2 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string IMAGENO_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4X_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string NUDO4XETC_6 { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string CATEGORY6 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CTDOSE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NUDOMAXRESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long CNT { get; set; }

        public string MINDATE { get; set; }
        public string MAXDATE { get; set; }
        public string MINXRAY { get; set; }
        public string MAXXRAY { get; set; }

        /// <summary>
        /// 일반검진 방사선촬영 판독관리
        /// </summary>
        public HIC_XRAY_RESULT()
        {
        }
    }
}
