namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class BAS_SUN : BaseDto
    {
        
        /// <summary>
        /// 품명코드
        /// </summary>
		public string SUNEXT { get; set; } 

        /// <summary>
        /// 한글명
        /// </summary>
		public string SUNAMEK { get; set; } 

        /// <summary>
        /// 영문명
        /// </summary>
		public string SUNAMEE { get; set; } 

        /// <summary>
        /// 약품성분명
        /// </summary>
		public string SUNAMEG { get; set; } 

        /// <summary>
        /// 표준코드와 환산계수
        /// </summary>
		public double SUHAM { get; set; } 

        /// <summary>
        /// 단위,ex) ml/Bottle
        /// </summary>
		public string UNIT { get; set; } 

        /// <summary>
        /// 대분류(약품 Only)
        /// </summary>
		public string DAICODE { get; set; } 

        /// <summary>
        /// 한글수가코드
        /// </summary>
		public string HCODE { get; set; } 

        /// <summary>
        /// 보험코드
        /// </summary>
		public string BCODE { get; set; } 

        /// <summary>
        /// 수가종류
        /// </summary>
		public string EDIJONG { get; set; } 

        /// <summary>
        /// EDI변경일자
        /// </summary>
		public string EDIDATE { get; set; } 

        /// <summary>
        /// OLD수가종류
        /// </summary>
		public string OLDJONG { get; set; } 

        /// <summary>
        /// OLD표준코드
        /// </summary>
		public string OLDBCODE { get; set; } 

        /// <summary>
        /// OLD환산계수
        /// </summary>
		public double OLDGESU { get; set; } 

        /// <summary>
        /// EDI용 기본검체 또는 확인코드
        /// </summary>
		public string EDILCODE { get; set; } 

        /// <summary>
        /// 원가분석 항목분류코드
        /// </summary>
		public string WONCODE { get; set; } 

        /// <summary>
        /// 원가분석 구입원가
        /// </summary>
		public long WONAMT { get; set; } 

        /// <summary>
        /// 1.퇴장방지의약품10%가산 0.아님
        /// </summary>
		public string SUGBM { get; set; } 

        /// <summary>
        /// 1.선수납물품 0.아님
        /// </summary>
		public string SUGBN { get; set; } 

        /// <summary>
        /// 의약분업 구분(아래참조)
        /// </summary>
		public string SUGBO { get; set; } 

        /// <summary>
        /// 원가분류1(0.행위료 1.재료대)
        /// </summary>
		public string GBWON1 { get; set; } 

        /// <summary>
        /// 원가분류2(0.일반 1.방사선조영제)
        /// </summary>
		public string GBWON2 { get; set; } 

        /// <summary>
        /// 원가분류3(차후사용예정)
        /// </summary>
		public string GBWON3 { get; set; } 

        /// <summary>
        /// 비급여분류(1.인정 2.임의 9.제외)
        /// </summary>
		public string SUGBP { get; set; } 

        /// <summary>
        /// EDI 변경일자3
        /// </summary>
		public string EDIDATE3 { get; set; } 

        /// <summary>
        /// 수가종류3
        /// </summary>
		public string EDIJONG3 { get; set; } 

        /// <summary>
        /// 표준코드3
        /// </summary>
		public string BCODE3 { get; set; } 

        /// <summary>
        /// 환산계수3
        /// </summary>
		public double GESU3 { get; set; } 

        /// <summary>
        /// EDI 변경일자4
        /// </summary>
		public string EDIDATE4 { get; set; } 

        /// <summary>
        /// 수가종류4
        /// </summary>
		public string EDIJONG4 { get; set; } 

        /// <summary>
        /// 표준코드4
        /// </summary>
		public string BCODE4 { get; set; } 

        /// <summary>
        /// 환산계수
        /// </summary>
		public double GESU4 { get; set; } 

        /// <summary>
        /// EDI 변경일자5
        /// </summary>
		public string EDIDATE5 { get; set; } 

        /// <summary>
        /// 수가종류5
        /// </summary>
		public string EDIJONG5 { get; set; } 

        /// <summary>
        /// 표준코드5
        /// </summary>
		public string BCODE5 { get; set; } 

        /// <summary>
        /// 환산계수5
        /// </summary>
		public double GESU5 { get; set; } 

        /// <summary>
        /// 예방접종여부(Y.예방접종)
        /// </summary>
		public string GBYEBANG { get; set; } 

        /// <summary>
        /// 고객정보에 특수검사로 보관 여부(Y.보관)
        /// </summary>
		public string GBCSINFO { get; set; } 

        /// <summary>
        /// 산재급여(0.적용않함 1.급여처리)
        /// </summary>
		public string SUGBQ { get; set; } 

        /// <summary>
        /// 임상심리실 오더전달 여부(Y.전달)
        /// </summary>
		public string GBSIMLI { get; set; } 

        /// <summary>
        /// 자보비급여수가중 급여여부(0.급여, 1.비급여)
        /// </summary>
		public string SUGBR { get; set; } 

        /// <summary>
        /// 1.100/100
        /// </summary>
		public string SUGBS { get; set; } 

        /// <summary>
        /// 1.DRG비급여
        /// </summary>
		public string SUGBT { get; set; } 

        /// <summary>
        /// 0.기본, 1.고가, 2.저가
        /// </summary>
		public string SUGBU { get; set; } 

        /// <summary>
        /// 수가 상세분류(자료사전:BAS_수가상세분류)
        /// </summary>
		public string DTLBUN { get; set; } 

        /// <summary>
        /// 수불구분(1.약제, 2.제료, 3.약제(제제약), 4.방사선조영제, 5.마취(분),6.
        /// </summary>
		public string SUGBV { get; set; } 

        /// <summary>
        /// 간호활동코드(ADMIN.abc_nurse_code)
        /// </summary>
		public long NURCODE { get; set; } 

        /// <summary>
        /// 예약검사 분류(자료사전: ETC_통합예약분류)
        /// </summary>
		public string GBYEYAK { get; set; } 

        /// <summary>
        /// 비급여구분
        /// </summary>
		public string GBSUGBF { get; set; } 

        /// <summary>
        /// 항생제구분(제한항생제용)
        /// </summary>
		public string GBANTI { get; set; } 

        /// <summary>
        /// 항생제 계열(약제과용)
        /// </summary>
		public string ANTICLASS { get; set; } 

        /// <summary>
        /// 고지혈증관리약제
        /// </summary>
		public string GBGOJI { get; set; } 

        /// <summary>
        /// 간장용관리약제
        /// </summary>
		public string GBGANJANG { get; set; } 

        /// <summary>
        /// 희귀난치성 질환 VCODE : Y
        /// </summary>
		public string GBRARE { get; set; } 

        /// <summary>
        /// 골다공증
        /// </summary>
		public string GBBONE { get; set; } 

        /// <summary>
        /// 단위1(약제용량)
        /// </summary>
		public double UNITNEW1 { get; set; } 

        /// <summary>
        /// 단위2(용량단위)
        /// </summary>
		public string UNITNEW2 { get; set; } 

        /// <summary>
        /// 단위3(제형)
        /// </summary>
		public string UNITNEW3 { get; set; } 

        /// <summary>
        /// 단위4(부피)
        /// </summary>
		public double UNITNEW4 { get; set; } 

        /// <summary>
        /// 수가참고사항
        /// </summary>
		public string MEMO { get; set; } 

        /// <summary>
        /// 항암제 (Y)
        /// </summary>
		public string GBANTICAN { get; set; } 

        /// <summary>
        /// 치매 미수구분
        /// </summary>
		public string SUGBW { get; set; } 

        /// <summary>
        /// PPI제제(원외처방 처방일수누적)
        /// </summary>
		public string GBPPI { get; set; } 

        /// <summary>
        /// 치매 약제
        /// </summary>
		public string GBDEMENTIA { get; set; } 

        /// <summary>
        /// 당뇨약제관리(ocs 3종이상 점검)
        /// </summary>
		public string GBDIA { get; set; } 

        /// <summary>
        /// 약제상한 관리(Y)
        /// </summary>
		public string GBDRUG { get; set; } 

        /// <summary>
        /// OCS 비급여 급여가능
        /// </summary>
		public string GBOCSF { get; set; } 

        /// <summary>
        /// 원무과 급여전환 메세지 표시
        /// </summary>
		public string GBWONF { get; set; } 

        /// <summary>
        /// GABAPENTIN 계열
        /// </summary>
		public string GBGABA { get; set; } 

        /// <summary>
        /// 약제상한 구분
        /// </summary>
		public string GBDRUGNO { get; set; } 

        /// <summary>
        /// 신경차단술(마취 노인 소아가산 않함)
        /// </summary>
		public string GBNS { get; set; } 

        /// <summary>
        /// 선택진료구분 아래참조
        /// </summary>
		public string GBSELECT { get; set; } 

        /// <summary>
        /// 의약품관리 여부
        /// </summary>
		public string GBOCSDRUG { get; set; } 

        /// <summary>
        /// 방사선단순 촬영매수
        /// </summary>
		public long XRAYQTY { get; set; } 

        /// <summary>
        /// 방사선코드
        /// </summary>
		public string XRAYCODE { get; set; } 

        /// <summary>
        /// DRG 코드(기본 부여코드)
        /// </summary>
		public string DRGCODE { get; set; } 

        /// <summary>
        /// DRG 100/100(Y)
        /// </summary>
		public string DRG100 { get; set; } 

        /// <summary>
        /// DRG 비급여(Y)
        /// </summary>
		public string DRGF { get; set; } 

        /// <summary>
        /// DRG 외가가산(Y)
        /// </summary>
		public string DRGADD { get; set; } 

        /// <summary>
        /// DRG 복강경 개복(Y)
        /// </summary>
		public string DRGOPEN { get; set; } 

        /// <summary>
        /// DRG 산부인가 30%가산여부(Y)
        /// </summary>
		public string DRGOGADD { get; set; } 

        /// <summary>
        /// 산재/자보 기술료가산(0:재료,1:행위)
        /// </summary>
		public string SUGBX { get; set; } 

        /// <summary>
        /// 흉부외과
        /// </summary>
		public string GBCS { get; set; } 

        /// <summary>
        /// Y:수술예방적대상(항생제)
        /// </summary>
		public string GBOPROOM { get; set; } 

        /// <summary>
        /// Y:부과세대상
        /// </summary>
		public string GBTAX { get; set; } 

        /// <summary>
        /// Y:항결핵약제비-입원명령결핵
        /// </summary>
		public string GBTB { get; set; } 

        /// <summary>
        /// 외과가산 1:20% 2:30%
        /// </summary>
		public string SUGBY { get; set; } 

        /// <summary>
        /// 휴부외과가산 1:100% 2:70%
        /// </summary>
		public string SUGBZ { get; set; } 

        /// <summary>
        /// Y:혈우약제
        /// </summary>
		public string GBBLOOD { get; set; } 

        /// <summary>
        /// 청구 특정내역 MT004 여부
        /// </summary>
		public string GBMT004 { get; set; } 

        /// <summary>
        /// 1.응급가산 50%, 2. 중증응급가산 50%,  3.권역가산 50%
        /// </summary>
		public string SUGBAA { get; set; } 

        /// <summary>
        /// 1.판독가산 10%
        /// </summary>
		public string SUGBAB { get; set; } 

        /// <summary>
        /// 1.개두마취 50%, 2.일측폐환기 50%, 3.개흉적 심장수술 마취 50%
        /// </summary>
		public string SUGBAC { get; set; } 

        /// <summary>
        /// 1.화상가산 30%
        /// </summary>
		public string SUGBAD { get; set; } 

        /// <summary>
        /// 수가코드 new
        /// </summary>
		public string CSUNEXT { get; set; } 

        /// <summary>
        /// 신경외과 가산 0. 가산 무, 1.5%, 2.10%, 3.15%
        /// </summary>
		public string SUGBAE { get; set; } 

        /// <summary>
        /// 호스피스행수가 대상 0.비대상, 1.정액수가대상
        /// </summary>
		public string SUGBAF { get; set; } 

        
        /// <summary>
        /// 수가 품명 Table
        /// </summary>
        public BAS_SUN()
        {
        }
    }
}
