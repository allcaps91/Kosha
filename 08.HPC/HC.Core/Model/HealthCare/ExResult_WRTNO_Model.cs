namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 일반건짐 검사결과 / 접수번호별
    /// </summary>
    public class ExResult_WRTNO_Model : BaseDto
    {
        public string EXCODE { get; set; }
        public string RESULT { get; set; }
        public string RESCODE { get; set; }
        public string RESULT_NAME { get; set; }
        public string STATUS { get; set; }
    }
}
