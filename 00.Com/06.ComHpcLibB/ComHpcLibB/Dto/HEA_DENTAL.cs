namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_DENTAL : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 판정:Y , 미판정:Null or N
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 수검일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 판정일자
        /// </summary>
		public DateTime? PANJENGDATE { get; set; } 

        /// <summary>
        /// 입력일자
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 우식증치아 번호(1,2,3, ...)
        /// </summary>
		public string USIK { get; set; } 

        /// <summary>
        /// 결손치아 번호(1,2,3, ...)
        /// </summary>
		public string GYEOLSON { get; set; } 

        /// <summary>
        /// 치은염 유무(Y/N)
        /// </summary>
		public string CHIEUN { get; set; } 

        /// <summary>
        /// 치주염 유무(Y/N)
        /// </summary>
		public string CHIJU { get; set; } 

        /// <summary>
        /// 치근막염 유무(Y/N)
        /// </summary>
		public string CHIGUNMAK { get; set; } 

        /// <summary>
        /// 의치 (0.없음 1.양호 2.불량)
        /// </summary>
		public string DENTURE { get; set; } 

        /// <summary>
        /// 의치 기타소견
        /// </summary>
		public string DENTURE_ETC { get; set; } 

        /// <summary>
        /// 정상
        /// </summary>
		public string PANJENG1 { get; set; } 

        /// <summary>
        /// 스케일링 필요함
        /// </summary>
		public string PANJENG2 { get; set; } 

        /// <summary>
        /// 풍치인 경우 치주과 전문의와 상담후 치료
        /// </summary>
		public string PANJENG3 { get; set; } 

        /// <summary>
        /// 산종류에 의한 부식된 부분 치료
        /// </summary>
		public string PANJENG4 { get; set; } 

        /// <summary>
        /// 치경부 마모증(칫솔질 잘못)
        /// </summary>
		public string PANJENG5 { get; set; } 

        /// <summary>
        /// 외상으로 인한 치아손상
        /// </summary>
		public string PANJENG6 { get; set; } 

        /// <summary>
        /// 턱관절에 이상 전문의와 상담
        /// </summary>
		public string PANJENG7 { get; set; } 

        /// <summary>
        /// 구강내 혀.잇몸에 염증시 정밀 검사
        /// </summary>
		public string PANJENG8 { get; set; } 

        /// <summary>
        /// 기타소견
        /// </summary>
		public string PANJENG9 { get; set; } 

        /// <summary>
        /// 기타소견 상세
        /// </summary>
		public string PANJENG10 { get; set; } 

        /// <summary>
        /// 결손치(보철요망)
        /// </summary>
		public string PANJENG11 { get; set; } 

        /// <summary>
        /// 우식증
        /// </summary>
		public string PANJENG12 { get; set; } 

        /// <summary>
        /// 결손치(보철필요없음)
        /// </summary>
		public string PANJENG13 { get; set; } 

        /// <summary>
        /// 이를 빼야합니다
        /// </summary>
		public string PANJENG14 { get; set; } 

        /// <summary>
        /// 교합면 마모
        /// </summary>
		public string PANJENG15 { get; set; } 

        /// <summary>
        /// 검사방 접수로 선택시 - Y 2013-06-26
        /// </summary>
		public string GBCHK { get; set; }

        /// <summary>
        /// 의사명
        /// </summary>
        public string DRNAME { get; set; }


        /// <summary>
        /// 종합검진 구강상담 테이블
        /// </summary>
        public HEA_DENTAL()
        {
        }
    }
}
