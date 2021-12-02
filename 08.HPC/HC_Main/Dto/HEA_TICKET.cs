namespace HC_Main.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HEA_TICKET : BaseDto
    {

        /// <summary>
        /// 초대권번호
        /// </summary>
        public long NO { get; set; }

        /// <summary>
        /// 배부일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 배부자
        /// </summary>
        public string BSAWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BGUBUN { get; set; }

        /// <summary>
        /// 배부처
        /// </summary>
        public string BNAME { get; set; }

        /// <summary>
        /// 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 종검예약 SMS전송 여부(Y/N)
        /// </summary>
        public string GBSMS1 { get; set; }

        /// <summary>
        /// 종검수검 SMS전송 여부(Y/N)
        /// </summary>
        public string GBSMS2 { get; set; }

        /// <summary>
        /// 종검 접수일자
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// 종검 수검일자
        /// </summary>
        public string SDATE { get; set; }

        /// <summary>
        /// 종검 접수번호
        /// </summary>
        public long JEPSUNO { get; set; }

        /// <summary>
        /// 종검 수검자명(나이/성별)
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SMS_SEND1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SMS_SEND2 { get; set; }

        /// <summary>
        /// 배부내역 등록 일시
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 배부내역 등록자 사번
        /// </summary>
        public long JOBSABUN { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 종합검진 초대권 관리
        /// </summary>
        public HEA_TICKET()
        {
        }
    }
}
