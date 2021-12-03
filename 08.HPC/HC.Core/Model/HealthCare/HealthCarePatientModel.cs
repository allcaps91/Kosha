namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 일반건짐 수검자 모델
    /// </summary>
    public class HealthCarePatientModel : BaseDto
    {
        public long WRTNO { get; set; }
      
        public string SNAME { get; set; }
        public string JUMIN2 { get; set; }
        public string JEPDATE { get; set; }
        public string SEX { get; set; }
        public int AGE { get; set; }
    }
}

