namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_PACS_ORDER : BaseDto
    {


        /// <summary>
        /// 
        /// </summary>
        public string QUEUEID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FALG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? WORKTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ACDESSIONNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EVENTTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ORDERDOC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ORDERFROM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATBIRTHDAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATSEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATDEPT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXAMROOM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HISORDERID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WARD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ROOM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string REQUESTMEMO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EMERGENCY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OPERATOR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DISEASE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string INPS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? INPT_DT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UPPS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UP_DT { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public XRAY_PACS_ORDER()
        {
        }
    }
}
