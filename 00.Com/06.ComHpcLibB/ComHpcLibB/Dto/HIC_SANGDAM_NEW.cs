namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_SANGDAM_NEW : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 검진구분
        /// </summary>
		public string GJJONG { get; set; } 

        /// <summary>
        /// 검진차수
        /// </summary>
		public string GJCHASU { get; set; } 

        /// <summary>
        /// 상담상태(Y: 완료, N or NULL : 미완료)
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 검진번호
        /// </summary>
		public long PANO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 생활습관 개선(음주)
        /// </summary>
		public string HABIT1 { get; set; } 

        /// <summary>
        /// 생활습관 개선(흡연)
        /// </summary>
		public string HABIT2 { get; set; } 

        /// <summary>
        /// 생활습관 개선(운동)
        /// </summary>
		public string HABIT3 { get; set; } 

        /// <summary>
        /// 생활습관 개선(체중)
        /// </summary>
		public string HABIT4 { get; set; } 

        /// <summary>
        /// 생활습관 개선(음식)
        /// </summary>
		public string HABIT5 { get; set; } 

        /// <summary>
        /// 외상후유증(1.무, 2.유)
        /// </summary>
		public string JINCHAL1 { get; set; } 

        /// <summary>
        /// 일반상태(1.양호, 2.보통, 3.불량)
        /// </summary>
		public string JINCHAL2 { get; set; } 

        /// <summary>
        /// 식사여부(Y/ N or NULL)
        /// </summary>
		public string GBSIKSA { get; set; } 

        /// <summary>
        /// 현재상태(중풍) - 진단여부
        /// </summary>
		public string T_STAT01 { get; set; } 

        /// <summary>
        /// 현재상태(중풍) - 약물치료여부
        /// </summary>
		public string T_STAT02 { get; set; } 

        /// <summary>
        /// 현재상태(심장병) - 진단여부
        /// </summary>
		public string T_STAT11 { get; set; } 

        /// <summary>
        /// 현재상태(심장병) - 약물치료여부
        /// </summary>
		public string T_STAT12 { get; set; } 

        /// <summary>
        /// 현재상태(고혈압) - 진단여부
        /// </summary>
		public string T_STAT21 { get; set; } 

        /// <summary>
        /// 현재상태(고혈압) - 약물치료여부
        /// </summary>
		public string T_STAT22 { get; set; } 

        /// <summary>
        /// 현재상태(당뇨병) - 진단여부
        /// </summary>
		public string T_STAT31 { get; set; } 

        /// <summary>
        /// 현재상태(당뇨병) - 약물치료여부
        /// </summary>
		public string T_STAT32 { get; set; } 

        /// <summary>
        /// 현재상태(이상지질) - 진단여부
        /// </summary>
		public string T_STAT41 { get; set; } 

        /// <summary>
        /// 현재상태(이상지질) - 약물치료여부
        /// </summary>
		public string T_STAT42 { get; set; } 

        /// <summary>
        /// 현재상태(기타) - 진단여부
        /// </summary>
		public string T_STAT51 { get; set; } 

        /// <summary>
        /// 현재상태(기타) - 약물치료여부
        /// </summary>
		public string T_STAT52 { get; set; } 

        /// <summary>
        /// 현재상태(기타) - 기타여부
        /// </summary>
		public string T_STAT52_TEC { get; set; } 

        /// <summary>
        /// 문진-가족력
        /// </summary>
		public string MUN_GAJOK { get; set; } 

        /// <summary>
        /// 문진-업무기인성
        /// </summary>
		public string MUN_GIINSUNG { get; set; } 

        /// <summary>
        /// 현재증상 및 자타각 증상
        /// </summary>
		public string JENGSANG { get; set; } 

        /// <summary>
        /// 임상진찰 - 안과
        /// </summary>
		public string JIN_01 { get; set; } 

        /// <summary>
        /// 임상진찰 - 이비인후과
        /// </summary>
		public string JIN_02 { get; set; } 

        /// <summary>
        /// 임상진찰 - 피부
        /// </summary>
		public string JIN_03 { get; set; } 

        /// <summary>
        /// 임상진찰 - 치아
        /// </summary>
		public string JIN_04 { get; set; } 

        /// <summary>
        /// 상담내용
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 병록등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 상담의사면허번호
        /// </summary>
		public long SANGDAMDRNO { get; set; } 

        /// <summary>
        /// 작업사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 작업일자
        /// </summary>
		public DateTime? ENTTIME { get; set; } 

        /// <summary>
        /// 현재상태(간장질환) - 진단여부
        /// </summary>
		public string T_STAT61 { get; set; } 

        /// <summary>
        /// 현재상태(간장질환) - 약물치료여부
        /// </summary>
		public string T_STAT62 { get; set; } 

        /// <summary>
        /// 2차 당뇨병-결과상태(0:정상 1:장애 2:당뇨)
        /// </summary>
		public string DIABETES_1 { get; set; } 

        /// <summary>
        /// 2차 당뇨병-치료계획
        /// </summary>
		public string DIABETES_2 { get; set; } 

        /// <summary>
        /// 2차 당뇨병-결과상태(0:정상 1:장애 2:고혈압)
        /// </summary>
		public string CYCLE_1 { get; set; } 

        /// <summary>
        /// 2차 당뇨병-치료계획
        /// </summary>
		public string CYCLE_2 { get; set; } 

        /// <summary>
        /// 학생-근골격및척추
        /// </summary>
		public string SCHPAN1 { get; set; } 

        /// <summary>
        /// 학생-근골격및척추(기타)
        /// </summary>
		public string SCHPAN2 { get; set; } 

        /// <summary>
        /// 학생-안질환
        /// </summary>
		public string SCHPAN3 { get; set; } 

        /// <summary>
        /// 학생-콧병
        /// </summary>
		public string SCHPAN4 { get; set; } 

        /// <summary>
        /// 학생-목병
        /// </summary>
		public string SCHPAN5 { get; set; } 

        /// <summary>
        /// 학생-피부병
        /// </summary>
		public string SCHPAN6 { get; set; } 

        /// <summary>
        /// 학생-귓병
        /// </summary>
		public string SCHPAN7 { get; set; } 

        /// <summary>
        /// 학생-과거병력
        /// </summary>
		public string SCHPAN8 { get; set; } 

        /// <summary>
        /// 학생-생활습관
        /// </summary>
		public string SCHPAN9 { get; set; } 

        /// <summary>
        /// 학생-외상및후유증
        /// </summary>
		public string SCHPAN10 { get; set; } 

        /// <summary>
        /// 학생-일반상태
        /// </summary>
		public string SCHPAN11 { get; set; } 

        /// <summary>
        /// 현재상태(폐결핵) - 진단여부
        /// </summary>
		public string T_STAT71 { get; set; } 

        /// <summary>
        /// 현재상태(폐결핵) - 약물치료여부
        /// </summary>
		public string T_STAT72 { get; set; } 

        /// <summary>
        /// 암검진 상담내역(신체증상{}암병력{}위대장{}유방자궁)
        /// </summary>
		public string AMSANGDAM { get; set; } 

        /// <summary>
        /// 문진-과거병력
        /// </summary>
		public string MUN_OLDMSYM { get; set; } 

        /// <summary>
        /// 특수검진 표적장기별 상담내역
        /// </summary>
		public string PJSANGDAM { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string GBCHK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RID { get; set; }

        public string NEXTROOM { get; set; }
        

        /// <summary>
        /// 
        /// </summary>
        public HIC_SANGDAM_NEW()
        {
        }
    }
}
