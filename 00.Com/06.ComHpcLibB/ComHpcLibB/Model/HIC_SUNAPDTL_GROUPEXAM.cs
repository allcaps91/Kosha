
namespace ComHpcLibB
{

    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_SUNAPDTL_GROUPEXAM : BaseDto
    {

        /// <summary>
        /// 검진접수 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 묶음코드
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 유해물질
        /// </summary>
        public string UCODE { get; set; }

        /// <summary>
        /// 금액
        /// </summary>
        public long AMT { get; set; }

        /// <summary>
        /// 부담율
        /// </summary>
        public string GBSELF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long GWRTNO { get; set; }


        /// <summary>
        /// 금액코드
        /// </summary>
        public string GROUPCODE { get; set; }

        /// <summary>
        /// 검사코드
        /// </summary>
        public string EXCODE { get; set; }

        /// <summary>
        /// 수가구분(아래참조)
        /// </summary>
        public string SUGAGBN { get; set; }

        /// <summary>
        /// 순위
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 최종작업사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 최종작업일자
        /// </summary>
        public string ENTTIME { get; set; }


}
}

