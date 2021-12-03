namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ACC_CLO_MGT : BaseDto
    {

        /// <summary>
        /// 업무 구분
        /// </summary>
        public string CLO_BSNS_GB { get; set; }

        /// <summary>
        /// 마감 일자
        /// </summary>
        public string CLO_YMD { get; set; }

        /// <summary>
        /// 일 마감 여부
        /// </summary>
        public string DD_CLO_YN { get; set; }

        /// <summary>
        /// 월 마감 여부
        /// </summary>
        public string MM_CLO_YN { get; set; }

        /// <summary>
        /// 년 마감 여부
        /// </summary>
        public string YY_CLO_YN { get; set; }

        /// <summary>
        /// 입력일시
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// 입력ID
        /// </summary>
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 입력IP
        /// </summary>
        public string CREATEDIP { get; set; }

        /// <summary>
        /// 수정일시
        /// </summary>
        public DateTime? MODIFIED { get; set; }

        /// <summary>
        /// 수정ID
        /// </summary>
        public string MODIFIEDUSER { get; set; }

        /// <summary>
        /// 수정IP
        /// </summary>
        public string MODIFIEDIP { get; set; }



        public ACC_CLO_MGT()
        {
        }
    }
}
