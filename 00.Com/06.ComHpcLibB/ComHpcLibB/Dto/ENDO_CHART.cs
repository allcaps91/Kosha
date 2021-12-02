namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ENDO_CHART : BaseDto
    {
        
        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 처방일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// 검사일자
        /// </summary>
		public string RDATE { get; set; } 

        /// <summary>
        /// 검사자
        /// </summary>
		public string R_DRNAME { get; set; } 

        /// <summary>
        /// 구분(1.외래,2.병실,3.신검,4.종검 5.응급실)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// EGD
        /// </summary>
		public string GB_EGD1 { get; set; } 

        /// <summary>
        /// EGD 수면
        /// </summary>
		public string GB_EGD2 { get; set; } 

        /// <summary>
        /// CFS
        /// </summary>
		public string GB_CFS1 { get; set; } 

        /// <summary>
        /// CFS 수면
        /// </summary>
		public string GB_CFS2 { get; set; } 

        /// <summary>
        /// Sigmodioscopy
        /// </summary>
		public string GB_SIG1 { get; set; } 

        /// <summary>
        /// Sigmodioscopy 수면
        /// </summary>
		public string GB_SIG2 { get; set; } 

        /// <summary>
        /// Bronchoscopy
        /// </summary>
		public string GB_BRO1 { get; set; } 

        /// <summary>
        /// Bronchoscopy 수면
        /// </summary>
		public string GB_BRO2 { get; set; } 

        /// <summary>
        /// ERCP
        /// </summary>
		public string GB_ERCP1 { get; set; } 

        /// <summary>
        /// ERCP 수면
        /// </summary>
		public string GB_ERCP2 { get; set; } 

        /// <summary>
        /// 금식여부(1.예)
        /// </summary>
		public string GB_DIET { get; set; } 

        /// <summary>
        /// 전신상태(1.양호,2.만성병색,3.급성병색,4.열)
        /// </summary>
		public string GB_STS { get; set; } 

        /// <summary>
        /// 장정결(1.시행함,2.시행안함,3,관장)
        /// </summary>
		public string GB_STS1 { get; set; } 

        /// <summary>
        /// 장정결도(1.좋음,2.보통,3.나쁨)
        /// </summary>
		public string GB_STS2 { get; set; } 

        /// <summary>
        /// 병력(1.없음)
        /// </summary>
		public string GB_OLD { get; set; } 

        /// <summary>
        /// 병력-간경변 (1.예)
        /// </summary>
		public string GB_OLD1 { get; set; } 

        /// <summary>
        /// 병력-불안정한 심폐질환
        /// </summary>
		public string GB_OLD2 { get; set; } 

        /// <summary>
        /// 병력-출혈경향질환
        /// </summary>
		public string GB_OLD3 { get; set; } 

        /// <summary>
        /// 병력- 신장기능부전
        /// </summary>
		public string GB_OLD4 { get; set; } 

        /// <summary>
        /// 병력- 심장판막질환
        /// </summary>
		public string GB_OLD5 { get; set; } 

        /// <summary>
        /// 병력- 심내막염의병력
        /// </summary>
		public string GB_OLD6 { get; set; } 

        /// <summary>
        /// 병력- 류마티스의 병력
        /// </summary>
		public string GB_OLD7 { get; set; } 

        /// <summary>
        /// 병력- 고혈압
        /// </summary>
		public string GB_OLD8 { get; set; } 

        /// <summary>
        /// 병력- 당뇨
        /// </summary>
		public string GB_OLD9 { get; set; } 

        /// <summary>
        /// 병력- 노혈관계 질환
        /// </summary>
		public string GB_OLD10 { get; set; } 

        /// <summary>
        /// 병력- 녹내장
        /// </summary>
		public string GB_OLD11 { get; set; } 

        /// <summary>
        /// 병력- 전립선 비대
        /// </summary>
		public string GB_OLD12 { get; set; } 

        /// <summary>
        /// 병력- 알레르기
        /// </summary>
		public string GB_OLD13 { get; set; } 

        /// <summary>
        /// 알레르기?()
        /// </summary>
		public string GB_OLD13_1 { get; set; } 

        /// <summary>
        /// 병력- 기존위암병력
        /// </summary>
		public string GB_OLD14 { get; set; } 

        /// <summary>
        /// 병력- 기타()
        /// </summary>
		public string GB_OLD15_1 { get; set; } 

        /// <summary>
        /// 현재복용약물 (1.없음)
        /// </summary>
		public string GB_DRUG { get; set; } 

        /// <summary>
        /// 현재복용약물 - 아스피린 (1.예)
        /// </summary>
		public string GB_DRUG1 { get; set; } 

        /// <summary>
        /// 현재복용약물 - 와파린
        /// </summary>
		public string GB_DRUG2 { get; set; } 

        /// <summary>
        /// 현재복용약물 - 항혈소판제재
        /// </summary>
		public string GB_DRUG3 { get; set; } 

        /// <summary>
        /// 현재복용약물 -항응고제
        /// </summary>
		public string GB_DRUG4 { get; set; } 

        /// <summary>
        /// 현재복용약물 - 항우울제/진정제
        /// </summary>
		public string GB_DRUG5 { get; set; } 

        /// <summary>
        /// 현재복용약물 - 인슐린/경구혈당강하제
        /// </summary>
		public string GB_DRUG6 { get; set; } 

        /// <summary>
        /// 현재복용약물 - 항고혈압제재
        /// </summary>
		public string GB_DRUG7 { get; set; } 

        /// <summary>
        /// 기타약제 : ()
        /// </summary>
		public string GB_DRUG8_1 { get; set; } 

        /// <summary>
        /// 검사전 복용중단 몇일 - 아스피린
        /// </summary>
		public string GB_DRUG_STOP1 { get; set; } 

        /// <summary>
        /// 검사전 복용중단 몇일 - 항응고제
        /// </summary>
		public string GB_DRUG_STOP2 { get; set; } 

        /// <summary>
        /// 전처치 약제 (1.없음)
        /// </summary>
		public string GB_B_DRUG { get; set; } 

        /// <summary>
        /// 전처치 약제 (1.진경제 사용)
        /// </summary>
		public string GB_B_DRUG1 { get; set; } 

        /// <summary>
        /// 전처치 약제 진경제 내역 ()
        /// </summary>
		public string GB_B_DRUG1_1 { get; set; } 

        /// <summary>
        /// 비고사항
        /// </summary>
		public string GB_BIGO { get; set; } 

        /// <summary>
        /// 수면평가 투여약물 Midazolam ()
        /// </summary>
		public string GB_SLEEP_DRUG1 { get; set; } 

        /// <summary>
        /// 수면평가 투여약물 Propofol ()
        /// </summary>
		public string GB_SLEEP_DRUG2 { get; set; } 

        /// <summary>
        /// 수면평가 투여약물 Pethidine ()
        /// </summary>
		public string GB_SLEEP_DRUG3 { get; set; } 

        /// <summary>
        /// 기타약제 ()
        /// </summary>
		public string GB_SLEEP_DRUG_ETC { get; set; } 

        /// <summary>
        /// 수면회복약제 (1.없음)
        /// </summary>
		public string GB_SLEEP_RE_DRUG { get; set; } 

        /// <summary>
        /// 수면회복약제 (1.Flumazenil)
        /// </summary>
		public string GB_SLEEP_RE_DRUG1 { get; set; } 

        /// <summary>
        /// 수면회복약제 Flumazenil ()
        /// </summary>
		public string GB_SLEEP_RE_DRUG1_1 { get; set; } 

        /// <summary>
        /// 활력징후 검사전 회/분1
        /// </summary>
		public string GB_SP0_11 { get; set; } 

        /// <summary>
        /// 활력징후 검사전  회/분2
        /// </summary>
		public string GB_SP0_12 { get; set; } 

        /// <summary>
        /// 활력징후 검사전  %1
        /// </summary>
		public string GB_SP0_13 { get; set; } 

        /// <summary>
        /// 활력징후 검사전  %2
        /// </summary>
		public string GB_SP0_14 { get; set; } 

        /// <summary>
        /// 활력징후 검사중  회/분1
        /// </summary>
		public string GB_SP0_21 { get; set; } 

        /// <summary>
        /// 활력징후 검사중  회/분2
        /// </summary>
		public string GB_SP0_22 { get; set; } 

        /// <summary>
        /// 활력징후 검사중  %1
        /// </summary>
		public string GB_SP0_23 { get; set; } 

        /// <summary>
        /// 활력징후 검사중  %2
        /// </summary>
		public string GB_SP0_24 { get; set; } 

        /// <summary>
        /// 활력징후 검사후  회/분1
        /// </summary>
		public string GB_SP0_31 { get; set; } 

        /// <summary>
        /// 활력징후 검사후  회/분2
        /// </summary>
		public string GB_SP0_32 { get; set; } 

        /// <summary>
        /// 활력징후 검사후  %1
        /// </summary>
		public string GB_SP0_33 { get; set; } 

        /// <summary>
        /// 활력징후 검사후  %2
        /// </summary>
		public string GB_SP0_34 { get; set; } 

        /// <summary>
        /// 활력징후 O2공급1
        /// </summary>
		public string GB_SP0_41 { get; set; } 

        /// <summary>
        /// 활력징후 O2공급2
        /// </summary>
		public string GB_SP0_42 { get; set; } 

        /// <summary>
        /// 수면평가 (1.잘함)
        /// </summary>
		public string GB_SLEEP_STS1 { get; set; } 

        /// <summary>
        /// 수면평가 (1.기침)
        /// </summary>
		public string GB_SLEEP_STS2 { get; set; } 

        /// <summary>
        /// 수면평가 (1.보통)
        /// </summary>
		public string GB_SLEEP_STS3 { get; set; } 

        /// <summary>
        /// 수면평가 (1.많이 움직임)
        /// </summary>
		public string GB_SLEEP_STS4 { get; set; } 

        /// <summary>
        /// 수면평가 (1.수면유도안됨)
        /// </summary>
		public string GB_SLEEP_STS5 { get; set; } 

        /// <summary>
        /// 수면평가 (1.구역질)
        /// </summary>
		public string GB_SLEEP_STS6 { get; set; } 

        /// <summary>
        /// 수면평가 (1.기타)
        /// </summary>
		public string GB_SLEEP_STS7 { get; set; } 

        /// <summary>
        /// 수면평가 기타 ()
        /// </summary>
		public string GB_SLEEP_STS7_1 { get; set; } 

        /// <summary>
        /// 생검유무(1.유)
        /// </summary>
		public string GB_EXAM { get; set; } 

        /// <summary>
        /// 퇴실기준 ( 1.외래, 2.병동)
        /// </summary>
		public string GB_OUT_GUBUN { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.활력징후는 안정되었는가?)
        /// </summary>
		public string GB_OUT_GUBUN1 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.호흡저하는 없는가?)
        /// </summary>
		public string GB_OUT_GUBUN2 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.보조없이 보행이 가능한가?)
        /// </summary>
		public string GB_OUT_GUBUN3 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.의사전달이 제대로 가능한가?)
        /// </summary>
		public string GB_OUT_GUBUN4 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.오심이나 구토등 이상증상이 없는가?)
        /// </summary>
		public string GB_OUT_GUBUN5 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.스스로 음료수를 섭취할수 있는가?)
        /// </summary>
		public string GB_OUT_GUBUN6 { get; set; } 

        /// <summary>
        /// 퇴실기준 (1.심한 통증은 없는가?)
        /// </summary>
		public string GB_OUT_GUBUN7 { get; set; } 

        /// <summary>
        /// 특이사항 유무( 1.유)
        /// </summary>
		public string GB_NUR_CHART { get; set; } 

        /// <summary>
        /// 특이사항 내역
        /// </summary>
		public string GB_NUR_CHART_REMARK { get; set; } 

        /// <summary>
        /// 간호사
        /// </summary>
		public string GB_NUR_NAME { get; set; } 

        /// <summary>
        /// 최종등록시간
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 최종등록사번
        /// </summary>
		public long ENTSABUN { get; set; } 

        /// <summary>
        /// 검사시작시간
        /// </summary>
		public string GB_STIME { get; set; } 

        /// <summary>
        /// 검사종료시간
        /// </summary>
		public string GB_ETIME { get; set; } 

        /// <summary>
        /// 생검-CLO (1.유)
        /// </summary>
		public string GB_CLO { get; set; } 

        /// <summary>
        /// emrno
        /// </summary>
		public long EMRNO { get; set; } 

        /// <summary>
        /// temp
        /// </summary>
		public string CHK_EDIT { get; set; } 

        /// <summary>
        /// 종검신규에서 저장했을경우 Y
        /// </summary>
		public string TO_OK { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ENTSABUN2 { get; set; }

        public string ROWID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ENDO_CHART()
        {
        }
    }
}
