namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class OCS_DOCTOR : BaseDto
    {
        
        /// <summary>
        /// 사원번호
        /// </summary>
		public string SABUN { get; set; } 

        /// <summary>
        /// 직급 1.전문의 2.전공의 3.인턴
        /// </summary>
		public string GRADE { get; set; } 

        /// <summary>
        /// 의사명
        /// </summary>
		public string DRNAME { get; set; } 

        /// <summary>
        /// 진료과
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사코드(수입금)
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 퇴사여부
        /// </summary>
		public string GBOUT { get; set; } 

        /// <summary>
        /// 의사면허번호
        /// </summary>
		public long DRBUNHO { get; set; } 

        /// <summary>
        /// 시작일자
        /// </summary>
		public DateTime? FDATE { get; set; } 

        /// <summary>
        /// 종료일자
        /// </summary>
		public DateTime? TDATE { get; set; } 

        /// <summary>
        /// 순서
        /// </summary>
		public long SORT { get; set; } 

        /// <summary>
        /// EMR_TREATT와 매칭용
        /// </summary>
		public string DOCCODE { get; set; } 

        /// <summary>
        /// 이미지 사인
        /// </summary>
		public string SIGNATURE { get; set; } 

        /// <summary>
        /// 진료과(OLD)
        /// </summary>
		public string DEPTCODE_OLD { get; set; } 

        /// <summary>
        /// 의사코드(OLD)
        /// </summary>
		public string DRCODE_OLD { get; set; } 

        /// <summary>
        /// NST 상담의사 여부(Y)
        /// </summary>
		public string GBNST { get; set; } 

        /// <summary>
        /// SMS 전송여부
        /// </summary>
		public string SMS_EXAMOK { get; set; } 

        /// <summary>
        /// WBC
        /// </summary>
		public string SMS_EXAM_H1 { get; set; } 

        /// <summary>
        /// RBC
        /// </summary>
		public string SMS_EXAM_H2 { get; set; } 

        /// <summary>
        /// Platelet
        /// </summary>
		public string SMS_EXAM_H3 { get; set; } 

        /// <summary>
        /// PT
        /// </summary>
		public string SMS_EXAM_H4 { get; set; } 

        /// <summary>
        /// aPTT
        /// </summary>
		public string SMS_EXAM_H5 { get; set; } 

        /// <summary>
        /// HGB
        /// </summary>
		public string SMS_EXAM_H6 { get; set; } 

        /// <summary>
        /// Ca
        /// </summary>
		public string SMS_EXAM_C1 { get; set; } 

        /// <summary>
        /// Gluecose
        /// </summary>
		public string SMS_EXAM_C2 { get; set; } 

        /// <summary>
        /// P
        /// </summary>
		public string SMS_EXAM_C3 { get; set; } 

        /// <summary>
        /// Na
        /// </summary>
		public string SMS_EXAM_C4 { get; set; } 

        /// <summary>
        /// K
        /// </summary>
		public string SMS_EXAM_C5 { get; set; } 

        /// <summary>
        /// T.Bilirubin
        /// </summary>
		public string SMS_EXAM_C6 { get; set; } 

        /// <summary>
        /// Creatinine
        /// </summary>
		public string SMS_EXAM_C7 { get; set; } 

        /// <summary>
        /// 입원외래 구분(I:외래, O:입원, A:전체)
        /// </summary>
		public string SMS_EXAM_IO { get; set; } 

        
        /// <summary>
        /// OCS 의사코드 관리
        /// </summary>
        public OCS_DOCTOR()
        {
        }
    }
}
