namespace ComHpcLibB.Model
{
    using ComBase.Mvc;


    /// <summary>
    /// 
    /// </summary>
    public class OSHA_HEALTH_CHECK_MODEL : BaseDto
    {
        public string YEAR { get; set; }
        public string SOGEN { get; set; }
        public string PAN { get; set; }
        public string JOB { get; set; }
    }

}
