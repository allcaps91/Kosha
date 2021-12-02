namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class HC_MISU_MST : BaseDto
    {
        public long WRTNO { get; set; }
        public long LTDCODE { get; set; }
        public string MISUJONG { get; set; }
        public DateTime BDATE { get; set; }
        public string GJONG { get; set; }
        public string GBEND { get; set; }
        public string MISUGBN { get; set; }
        public long MISUAMT { get; set; }
        public long IPGUMAMT { get; set; }
        public long GAMAMT { get; set; }
        public long SAKAMT { get; set; }
        public long BANAMT { get; set; }
        public long JANAMT { get; set; }
        public string GIRONO { get; set; }
        public string DANNAME { get; set; }
        
        public string REMARK { get; set; }
        public string MIRGBN { get; set; }
        public long MIRCHAAMT { get; set; }
        public string GBMISUBUILD { get; set; }
        public string YYMM_JIN { get; set; }
        public string PUMMOK { get; set; }
        public long CNO { get; set; }
        public DateTime ENDDATE { get; set; }

        public long ENTSABUN { get; set; }

    }
}
