namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using HC.Core.Model;
    using System;

    /// <summary>
    /// 청구공문
    /// </summary>
    public class ChargeDocumentModel : BaseDto
    {
        public string BDATE { get; set; }
        public long WRTNO { get; set; }
        public string GJONG { get; set; }
        public long LTDCODE { get; set; }
        public string SITENAME { get; set; }
        public string GEACODE { get; set; }
        public string SLIPAMT { get; set; }
        public string REMARK { get; set; }
        public string USERID { get; set; }
        public string NAME { get; set; }
        public string MJJONG { get; set; }

        public string EMAIL { get; set; }
        public string DAMNAME { get; set; }
        public string EMAIL2 { get; set; }

        public string SENDDATE { get; set; }
        public string SENDNAME { get; set; }
    }
}
