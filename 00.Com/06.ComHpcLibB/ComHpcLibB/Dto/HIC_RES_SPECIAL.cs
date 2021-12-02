namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_RES_SPECIAL : BaseDto
    {   
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public string JEPDATE { get; set; } 

        /// <summary>
        /// 문진일자(NULL:미등록)
        /// </summary>
		public string MUNJINDATE { get; set; } 

        /// <summary>
        /// 판정일자(NULL:미판정)
        /// </summary>
		public string PANJENGDATE { get; set; } 

        /// <summary>
        /// 판정의사 면허번호
        /// </summary>
		public long PANJENGDRNO { get; set; } 

        /// <summary>
        /// 취급물질 건수
        /// </summary>
		public long UCODECNT { get; set; } 

        /// <summary>
        /// 취급물질 (컴마로 분리됨)
        /// </summary>
		public string UCODENAME { get; set; } 

        /// <summary>
        /// 선택검사
        /// </summary>
		public string SEXAMNAME { get; set; } 

        /// <summary>
        /// 결과지 인쇄(Y:인쇄 기타:미인쇄)
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// R(2차검진)판정 여부(Y:2차대상,N:2차대상 아님)
        /// </summary>
		public string GBPANJENGR { get; set; } 

        /// <summary>
        /// 작업상태(아래참조)
        /// </summary>
		public string GBSTS { get; set; } 

        /// <summary>
        /// 특수검진 구분(아래 참조)
        /// </summary>
		public string GBSPC { get; set; } 

        /// <summary>
        /// 안전공단 전송여부(Y/N)
        /// </summary>
		public string GBOHMS { get; set; } 

        /// <summary>
        /// 근무회사의 사원번호
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 작업부서
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 한자이름(공무원채용에서 사용)
        /// </summary>
		public string HNAME { get; set; } 

        /// <summary>
        /// 입사일자
        /// </summary>
		public string IPSADATE { get; set; } 

        /// <summary>
        /// 수첩소지여부(1.소지 0.소지안함)
        /// </summary>
		public string SUCHUPYN { get; set; } 

        /// <summary>
        /// 현작업공정
        /// </summary>
		public string GONGJENG { get; set; } 

        /// <summary>
        /// 현직종 - 사용안함
        /// </summary>
		public string JIKJONG { get; set; } 

        /// <summary>
        /// 전입일자
        /// </summary>
		public string JENIPDATE { get; set; } 

        /// <summary>
        /// 현직근무년수
        /// </summary>
		public long PGIGAN_YY { get; set; } 

        /// <summary>
        /// 현직근무월수
        /// </summary>
		public long PGIGAN_MM { get; set; } 

        /// <summary>
        /// 1일 노출시간 - 사용안함
        /// </summary>
		public long PTIME { get; set; } 

        /// <summary>
        /// 과거직력①:작업공정
        /// </summary>
		public string OLDGONG1 { get; set; } 

        /// <summary>
        /// 과거직력①:취급물질코드
        /// </summary>
		public string OLDMCODE1 { get; set; } 

        /// <summary>
        /// 과거직력①:근무년수
        /// </summary>
		public long OLDYEAR1 { get; set; } 

        /// <summary>
        /// 과거직력①:1일폭로시간
        /// </summary>
		public long OLDDAYTIME1 { get; set; } 

        /// <summary>
        /// 과거직력②:작업공정
        /// </summary>
		public string OLDGONG2 { get; set; } 

        /// <summary>
        /// 과거직력②:취급물질코드
        /// </summary>
		public string OLDMCODE2 { get; set; } 

        /// <summary>
        /// 과거직력②:근무년수
        /// </summary>
		public long OLDYEAR2 { get; set; } 

        /// <summary>
        /// 과거직력②:1일폭로시간
        /// </summary>
		public long OLDDAYTIME2 { get; set; } 

        /// <summary>
        /// 과거직력③:작업공정
        /// </summary>
		public string OLDGONG3 { get; set; } 

        /// <summary>
        /// 과거직력③:취급물질코드
        /// </summary>
		public string OLDMCODE3 { get; set; } 

        /// <summary>
        /// 과거직력③:근무년수
        /// </summary>
		public long OLDYEAR3 { get; set; } 

        /// <summary>
        /// 과거직력③:1일폭로시간
        /// </summary>
		public long OLDDAYTIME3 { get; set; } 

        /// <summary>
        /// 생활습관:음주
        /// </summary>
		public string HABIT1 { get; set; } 

        /// <summary>
        /// 생활습관:흡연
        /// </summary>
		public string HABIT2 { get; set; } 

        /// <summary>
        /// 생활습관:운동
        /// </summary>
		public string HABIT3 { get; set; } 

        /// <summary>
        /// 생활습관:체중
        /// </summary>
		public string HABIT4 { get; set; } 

        /// <summary>
        /// 생활습관:식사
        /// </summary>
		public string HABIT5 { get; set; } 

        /// <summary>
        /// 외상및휴유증여부(Y/N)
        /// </summary>
		public string GBHUYU { get; set; } 

        /// <summary>
        /// 일반상태(1.양호 2.보통 3.불량)
        /// </summary>
		public string GBSANGTAE { get; set; } 

        /// <summary>
        /// 과거병력:간장   발병년도
        /// </summary>
		public string OLDMYEAR1 { get; set; } 

        /// <summary>
        /// 과거병력:고혈압 발병년도
        /// </summary>
		public string OLDMYEAR2 { get; set; } 

        /// <summary>
        /// 과거병력:뇌졸증 발병년도
        /// </summary>
		public string OLDMYEAR3 { get; set; } 

        /// <summary>
        /// 과거병력:당뇨   발병년도
        /// </summary>
		public string OLDMYEAR4 { get; set; } 

        /// <summary>
        /// 과거병력:암     발병년도
        /// </summary>
		public string OLDMYEAR5 { get; set; } 

        /// <summary>
        /// 과거병력:기타질환명
        /// </summary>
		public string OLDETCMSYM { get; set; } 

        /// <summary>
        /// 문진: 가족력
        /// </summary>
		public string MUN_GAJOK { get; set; } 

        /// <summary>
        /// 문진: 업무기인성
        /// </summary>
		public string MUN_GIINSUNG { get; set; } 

        /// <summary>
        /// 임상진찰: 안과(2009)
        /// </summary>
		public string JIN_NEURO { get; set; } 

        /// <summary>
        /// 임상진찰: 이비인후(2009)
        /// </summary>
		public string JIN_HEAD { get; set; } 

        /// <summary>
        /// 임상진찰: 피부(2009)
        /// </summary>
		public string JIN_SKIN { get; set; } 

        /// <summary>
        /// 임상진찰: 치아(2009)
        /// </summary>
		public string JIN_CHEST { get; set; } 

        /// <summary>
        /// 현재증상 및 자타각 증상 코드
        /// </summary>
		public string JENGSANG { get; set; } 

        /// <summary>
        /// 문진표(소음)
        /// </summary>
		public string MUNJIN_A { get; set; } 

        /// <summary>
        /// 문진표(분진)
        /// </summary>
		public string MUNJIN_B { get; set; } 

        /// <summary>
        /// 문진표(진동)
        /// </summary>
		public string MUNJIN_C { get; set; } 

        /// <summary>
        /// 문진표(유기용제)
        /// </summary>
		public string MUNJIN_D { get; set; } 

        /// <summary>
        /// 문진표(연)
        /// </summary>
		public string MUNJIN_E { get; set; } 

        /// <summary>
        /// 문진표(수은)
        /// </summary>
		public string MUNJIN_F { get; set; } 

        /// <summary>
        /// 문진표(비전리방사선)
        /// </summary>
		public string MUNJIN_G { get; set; } 

        /// <summary>
        /// 문진표(크롬)
        /// </summary>
		public string MUNJIN_H { get; set; } 

        /// <summary>
        /// 문진표(특정화학물질I류)
        /// </summary>
		public string MUNJIN_I { get; set; } 

        /// <summary>
        /// 문진표(카드뮴)
        /// </summary>
		public string MUNJIN_J { get; set; } 

        /// <summary>
        /// 문진표(특정화학물질2,3류)
        /// </summary>
		public string MUNJIN_K { get; set; } 

        /// <summary>
        /// 문진표(망간)
        /// </summary>
		public string MUNJIN_L { get; set; } 

        /// <summary>
        /// 문진표(이상기압)
        /// </summary>
		public string MUNJIN_M { get; set; } 

        /// <summary>
        /// 문진표(전리방사선)
        /// </summary>
		public string MUNJIN_N { get; set; } 

        /// <summary>
        /// 신장
        /// </summary>
		public decimal HEIGHT { get; set; } 

        /// <summary>
        /// 몸무게
        /// </summary>
		public decimal WEIGHT { get; set; } 

        /// <summary>
        /// 비만도
        /// </summary>
		public long BIMAN { get; set; } 

        /// <summary>
        /// 혈액형(AboRH)
        /// </summary>
		public string ABORH { get; set; } 

        /// <summary>
        /// 검진구분
        /// </summary>
		public string GUNJINGBN { get; set; } 

        /// <summary>
        /// 치과소견
        /// </summary>
		public string DENTSOGEN { get; set; } 

        /// <summary>
        /// 치과의사 면허번호
        /// </summary>
		public long DENTDOCT { get; set; } 

        /// <summary>
        /// 흉부방사선구분(1.직접,2.간접)
        /// </summary>
		public string XRAYGBN { get; set; } 

        /// <summary>
        /// 흉부방사선1차 촬영번호
        /// </summary>
		public string XRAYNO { get; set; } 

        /// <summary>
        /// 흉부방사선2차 촬영번호
        /// </summary>
		public string XRAYNO2 { get; set; } 

        /// <summary>
        /// 임상진찰 의사 면허번호
        /// </summary>
		public long JINDRNO { get; set; } 

        /// <summary>
        /// 임상진찰 참고사항
        /// </summary>
		public string JINREMARK { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public string TONGBODATE { get; set; } 

        /// <summary>
        /// 특검공통문진-일반
        /// </summary>
		public string MUNJINT_A { get; set; } 

        /// <summary>
        /// 특검공통문진-피부
        /// </summary>
		public string MUNJINT_B { get; set; } 

        /// <summary>
        /// 특검공통문진-눈
        /// </summary>
		public string MUNJINT_C { get; set; } 

        /// <summary>
        /// 특검공통문진-귀
        /// </summary>
		public string MUNJINT_D { get; set; } 

        /// <summary>
        /// 특검공통문진-코
        /// </summary>
		public string MUNJINT_E { get; set; } 

        /// <summary>
        /// 특검공통문진-입
        /// </summary>
		public string MUNJINT_F { get; set; } 

        /// <summary>
        /// 특검공통문진-소화기
        /// </summary>
		public string MUNJINT_G { get; set; } 

        /// <summary>
        /// 특검공통문진-심혈/호흡
        /// </summary>
		public string MUNJINT_H { get; set; } 

        /// <summary>
        /// 특검공통문진-척추사지
        /// </summary>
		public string MUNJINT_I { get; set; } 

        /// <summary>
        /// 특검공통문진-정신신경
        /// </summary>
		public string MUNJINT_J { get; set; } 

        /// <summary>
        /// 특검공통문진-비뇨생식
        /// </summary>
		public string MUNJINT_K { get; set; } 

        /// <summary>
        /// 작업중 건강상의문제?
        /// </summary>
		public string HSTAT { get; set; } 

        /// <summary>
        /// 취급하는 물질로 건강문제?
        /// </summary>
		public string MCODE_STAT { get; set; } 

        /// <summary>
        /// 폐활량검사 경험(0:무, 1:유)
        /// </summary>
		public string GBCAPACITY { get; set; } 

        /// <summary>
        /// 과거또는현재앓고있는질환(0:무,8:기타)
        /// </summary>
		public string OLDJILHWAN { get; set; } 

        /// <summary>
        /// 과거또는현재앓고있는질환기타사항
        /// </summary>
		public string OLDJILHWAN_ETC { get; set; } 

        /// <summary>
        /// 수술경험(0:무,6:기타)
        /// </summary>
		public string GBSUSUL { get; set; } 

        /// <summary>
        /// 수술경험기타사항
        /// </summary>
		public string GBSUSUL_ETC { get; set; } 

        /// <summary>
        /// 현재복용약물(0:무,2:기타)
        /// </summary>
		public string GBDRUG { get; set; } 

        /// <summary>
        /// 현재복용약물기타사항
        /// </summary>
		public string GBDRUG_ETC { get; set; } 

        /// <summary>
        /// 흡연력(ex: 00,00,0000 -> 00년 00개월 0000개비)
        /// </summary>
		public string SMOKEYEAR { get; set; } 

        /// <summary>
        /// 금일흡연여부
        /// </summary>
		public string GBSMOKE { get; set; } 

        /// <summary>
        /// 의치착용여부
        /// </summary>
		public string GBDENTY { get; set; } 

        /// <summary>
        /// 호흡곤란정도
        /// </summary>
		public string DYSPNEA { get; set; } 

        /// <summary>
        /// 의사상담내역
        /// </summary>
		public string SANGDAM { get; set; } 

        /// <summary>
        /// 검진일에 식사여부(Y: 했음, N or NULL : 안했음)
        /// </summary>
		public string GBSIKSA { get; set; } 

        /// <summary>
        /// 추가(선택)검사 판정소견
        /// </summary>
		public string ADDSOGEN { get; set; } 

        /// <summary>
        /// 과거직력①:노출기간
        /// </summary>
		public string OLDPGIGAN1 { get; set; } 

        /// <summary>
        /// 과거직력②:노출기간
        /// </summary>
		public string OLDPGIGAN2 { get; set; } 

        /// <summary>
        /// 과거직력③:노출기간
        /// </summary>
		public string OLDPGIGAN3 { get; set; } 

        /// <summary>
        /// 프린트 작업사번
        /// </summary>
		public long PRTSABUN { get; set; } 

        /// <summary>
        /// 국가코드(2자리 영문)
        /// </summary>
		public string NATIONAL { get; set; } 

        /// <summary>
        /// 문진-과거병력
        /// </summary>
		public string MUN_OLDMSYM { get; set; } 

        /// <summary>
        /// 금기사항확인(0:무, 1:유)
        /// </summary>
		public string GBGEUMGI { get; set; } 

        /// <summary>
        /// 흡연력(0:무, 1:유)
        /// </summary>
		public string GBSMOKE1 { get; set; } 

        /// <summary>
        /// 폐활량 검사자세 (0: 선자세, 1:앉은자세)
        /// </summary>
		public string POSTURE { get; set; } 

        /// <summary>
        /// 폐활량 검사장비 Serial No
        /// </summary>
		public string SERIALNO { get; set; } 

        /// <summary>
        /// 폐활량 검사협조 (0: 협조, 1:보통, 2:비협조)
        /// </summary>
		public string COOPERATE { get; set; } 

        /// <summary>
        /// 폐활량 검사 소견항목 (구분자 {} )
        /// </summary>
		public string PSOGEN { get; set; } 

        /// <summary>
        /// 폐활량 검사 기타소견
        /// </summary>
		public string PETCSOGEN { get; set; } 

        /// <summary>
        /// 폐활량 검사일자
        /// </summary>
		public string PFTDATE { get; set; } 

        /// <summary>
        /// 폐활량 검사사번
        /// </summary>
		public long PFTSABUN { get; set; } 

        /// <summary>
        /// 폐활량 복부비만 허리둘레(cm)
        /// </summary>
		public string BMI { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 특수검진 문진 및 판정결과(변경후)
        /// </summary>
        public HIC_RES_SPECIAL()
        {
        }
    }
}
