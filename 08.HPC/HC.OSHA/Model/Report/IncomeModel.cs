namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 달력 이벤트 모델
    /// </summary>
    public class IncomeModel : BaseDto
    {
        public string Created { get; set; }
        public string Modified { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string VisitDate { get; set; }
        public string VisitUserName { get; set; }
        public long WorkerCount { get; set; }
        public long UnitPrice { get; set; }
        public long TotalPrice { get; set; }
        public string IsDeleted { get; set; }

        public string inwon { get; set; }
        public string danga { get; set; }
        public string income { get; set; }
    }
}
