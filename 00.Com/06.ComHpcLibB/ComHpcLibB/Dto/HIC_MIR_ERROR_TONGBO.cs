namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_MIR_ERROR_TONGBO : BaseDto
    {
        
        /// <summary>
        /// 접수번호
        /// </summary>
		public long WRTNO { get; set; } 

        /// <summary>
        /// 통보일자
        /// </summary>
		public string TONGBODATE { get; set; } 

        /// <summary>
        /// 청구번호
        /// </summary>
		public long MIRNO { get; set; } 

        /// <summary>
        /// 수진자명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 나이
        /// </summary>
		public long AGE { get; set; } 

        /// <summary>
        /// 청구구분
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 오류 상세내역
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 담당자명
        /// </summary>
		public string DAMNAME { get; set; } 

        /// <summary>
        /// 수정완료일자
        /// </summary>
		public string OKDATE { get; set; } 

        /// <summary>
        /// 계측 메모창
        /// </summary>
		public string ACTMEMO { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string ROWID { get; set; }


        public long JOBSABUN { get; set; }
        public string JEPDATE { get; set; }
        public string PRTSABUN { get; set; }
        public string GJJONG { get; set; }
        public string PTNO { get; set; }
        public string GBDEL { get; set; }

        /// <summary>
        /// 건진청구 오류 통보 상세내역
        /// </summary>
        public HIC_MIR_ERROR_TONGBO()
        {
        }
    }
}
