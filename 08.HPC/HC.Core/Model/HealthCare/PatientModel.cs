namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 검진 접수 환자
    /// </summary>
    public class PatientModel : BaseDto
    {
        public string PTNO { get; set; }
        public string PANO { get; set; }
        public string NAME { get; set; }
        public string JUMIN { get; set; }
        public string TEL { get; set; }
        public string IPSADATE { get; set; }
        public string DEPT { get; set; }
    }
}
