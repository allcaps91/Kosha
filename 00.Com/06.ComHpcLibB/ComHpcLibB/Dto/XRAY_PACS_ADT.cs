namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_PACS_ADT : BaseDto
    {

        /// <summary>
        /// 
        /// </summary>
        public string QUEUEID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FLAG { get; set; }

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
        public string EVENTTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BIRTHDAY { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DEPT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ATTENDDOCT1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PATTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PERSONALID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WARD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ROOMNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EPATNAME { get; set; }

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
        public XRAY_PACS_ADT()
        {
        }
    }
}
