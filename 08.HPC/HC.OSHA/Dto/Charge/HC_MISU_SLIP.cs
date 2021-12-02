namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class HC_MISU_SLIP : BaseDto
    {
        public long WRTNO { get; set; }
        public DateTime BDATE { get; set; }
        public long LTDCODE { get; set; }
        public string GEACODE { get; set; }
        public long SLIPAMT { get; set; }
        public string REMARK { get; set; }
        public DateTime ENTDATE { get; set; }
        public long ENTSABUN { get; set; }
    }
}
