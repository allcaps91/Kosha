namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class EXAM_ANATMST : BaseDto
    {
        
        /// <summary>
        /// 병록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 발생일자=>(2005-07-22) 처방일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// OrderCode
        /// </summary>
		public string ORDERCODE { get; set; } 

        /// <summary>
        /// OrderNo
        /// </summary>
		public long ORDERNO { get; set; } 

        /// <summary>
        /// I:입원, O:외래 구분
        /// </summary>
		public string GBIO { get; set; } 

        /// <summary>
        /// Nature + of Specimen
        /// </summary>
		public string REMARK1 { get; set; } 

        /// <summary>
        /// Clinical Information +
        /// </summary>
		public string REMARK2 { get; set; } 

        /// <summary>
        /// Information on preview Cytoloy Examination
        /// </summary>
		public string REMARK3 { get; set; } 

        /// <summary>
        /// 작업구분(Y/N/V)
        /// </summary>
		public string GBJOB { get; set; } 

        /// <summary>
        /// 결과일자
        /// </summary>
		public DateTime? RESULTDATE { get; set; } 

        /// <summary>
        /// 결과소견--E
        /// </summary>
		public string RESULT1 { get; set; } 

        /// <summary>
        /// 결과소견2--E
        /// </summary>
		public string RESULT2 { get; set; } 

        /// <summary>
        /// 과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 검체번호
        /// </summary>
		public string SPECNO { get; set; } 

        /// <summary>
        /// 병리번호
        /// </summary>
		public string ANATNO { get; set; } 

        /// <summary>
        /// 검사코드
        /// </summary>
		public string MASTERCODE { get; set; } 

        /// <summary>
        /// 결과입력자---E
        /// </summary>
		public string RESULTSABUN { get; set; } 

        /// <summary>
        /// 검체코드
        /// </summary>
		public string SPECCODE { get; set; } 

        /// <summary>
        /// 프린트 장수
        /// </summary>
		public string GBPRINT { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public DateTime? JDATE { get; set; } 

        /// <summary>
        /// 적출일자시간
        /// </summary>
		public DateTime? OUTDATE { get; set; } 

        /// <summary>
        /// 적출자
        /// </summary>
		public long OUTSABUN { get; set; } 

        /// <summary>
        /// Clinical Diagnosis(050718추가)
        /// </summary>
		public string REMARK4 { get; set; } 

        /// <summary>
        /// 탈회여부
        /// </summary>
		public string TALCHK { get; set; } 

        /// <summary>
        /// 세표 조직 소견
        /// </summary>
		public string REMARK5 { get; set; } 

        /// <summary>
        /// 일반건진 세포 조직 소견1
        /// </summary>
		public string HRREMARK1 { get; set; } 

        /// <summary>
        /// 일반건진 세포 조직 소견2
        /// </summary>
		public string HRREMARK2 { get; set; } 

        /// <summary>
        /// 일반건진 세포 조직 소견3---E
        /// </summary>
		public string HRREMARK3 { get; set; } 

        /// <summary>
        /// 일반건진 세포 조직 소견4---E
        /// </summary>
		public string HRREMARK4 { get; set; } 

        /// <summary>
        /// 일반건진 세포 조직 소견5---E
        /// </summary>
		public string HRREMARK5 { get; set; } 

        /// <summary>
        /// 의뢰의사를 종검으로 표시(Y)
        /// </summary>
		public string GBTO { get; set; } 

        /// <summary>
        /// 결과 입력자 성명
        /// </summary>
		public string GBSNAME { get; set; } 

        /// <summary>
        /// 아래참조
        /// </summary>
		public string GBTATBUN { get; set; } 

        /// <summary>
        /// 지연이유
        /// </summary>
		public string GBTATSAU { get; set; } 

        /// <summary>
        /// 아래참조
        /// </summary>
		public string GBTATBUN2 { get; set; } 

        /// <summary>
        /// 아래참조
        /// </summary>
		public string GBTATBUN3 { get; set; } 

        /// <summary>
        /// 종검,건진 접수번호
        /// </summary>
		public long HICNO { get; set; } 

        /// <summary>
        /// 전자서명일시
        /// </summary>
		public DateTime? CERTDATE { get; set; } 

        /// <summary>
        /// 해시데이타
        /// </summary>
		public string HASHDATA { get; set; } 

        /// <summary>
        /// 전자서명값
        /// </summary>
		public string CERTDATA { get; set; } 

        /// <summary>
        /// EMR여부
        /// </summary>
		public string SENDEMR { get; set; } 

        /// <summary>
        /// EMR 번호
        /// </summary>
		public long SENDEMRNO { get; set; } 

        /// <summary>
        /// 접수자사번
        /// </summary>
		public string JSABUN { get; set; } 

        /// <summary>
        /// *:외부의뢰
        /// </summary>
		public string GBOUT { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string RESULT_IMG { get; set; } 

        /// <summary>
        /// CVR 등록일자 2015-05-26
        /// </summary>
		public DateTime? CVR_ENTDATE { get; set; } 

        /// <summary>
        /// CVR 등록사번 2015-05-26
        /// </summary>
		public long CVR_ENTSABUN { get; set; } 

        /// <summary>
        /// CVR 등록소견 2015-05-26
        /// </summary>
		public string CREMARK1 { get; set; } 

        /// <summary>
        /// CVR 등록 조치일자 2015-05-26
        /// </summary>
		public DateTime? CVR_CONFIRM_DATE { get; set; } 

        /// <summary>
        /// CVR 등록 조치사번 2015-05-26
        /// </summary>
		public long CVR_CONFIRM_SABUN { get; set; } 

        /// <summary>
        /// CVR 등록 조치소견 2015-05-26
        /// </summary>
		public string CREMARK2 { get; set; } 

        /// <summary>
        /// CVR 등록 확인일자 2015-11-02
        /// </summary>
		public DateTime? CVR_CONFIRM_DATE2 { get; set; } 

        /// <summary>
        /// CVR 등록 확인사번 2015-11-02
        /// </summary>
		public long CVR_CONFIRM_SABUN2 { get; set; } 

        /// <summary>
        /// 동결절편여부
        /// </summary>
		public string FROZEN { get; set; } 

        /// <summary>
        /// 조직슬라이드적정코드
        /// </summary>
		public string SLIDE_S_CD { get; set; } 

        /// <summary>
        /// 조직슬라이드적정기타
        /// </summary>
		public string SLIDE_S_ETC { get; set; } 

        /// <summary>
        /// 세포슬라이드적정코드
        /// </summary>
		public string SLIDE_C_CD { get; set; } 

        /// <summary>
        /// 세포슬라이드적정기타
        /// </summary>
		public string SLIDE_C_ETC { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SPEC_S_CD { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SPEC_C_CD { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SPEC_C_ETC { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string SPEC_S_ETC { get; set; } 

        /// <summary>
        /// C# 결과입력 폼에서 전자인증할 경우 Y, 아니라면 null
        /// </summary>
		public string CERTNEW { get; set; }


        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }

        /// <summary>
        /// 해부병리 Master
        /// </summary>
        public EXAM_ANATMST()
        {
        }
    }
}
