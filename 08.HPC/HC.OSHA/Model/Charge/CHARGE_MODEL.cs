namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using HC.Core.Model;
    using System;
    using System.Collections.Generic;

    public class CHARGE_MODEL : BaseDto
    {
        //계약, 방문
      
        public long SCHEDULE_ID { get; set; }
        public string VISITRESERVEDATE { get; set; }
        public long VISIT_ID { get; set; }
        public long SITE_ID { get; set; }

        public long PARENT_SITE_ID { get; set; }
        public long ESTIMATE_ID { get; set; }
        public string SITE_NAME { get; set; }
        public string VISITDATETIME { get; set; }
        public string VISITUSERNAME { get; set; }
        public string ISPRECHARGE { get; set; }
        
        public long WORKERCOUNT { get; set; }
        public long UNITPRICE { get; set; }
        public long TOTALPRICE { get; set; }


        //청구
        public long WRTNO { get; set; }
        public long ID { get; set; }
        public long VISIT_PRICE_ID { get; set; }
        public string CHARGEDATE { get; set; }
        public long E_WORKERCOUNT { get; set; }
        public long E_UNITPRICE { get; set; }
        public long E_TOTALPRICE { get; set; }
        public string ISCOMPLETE { get; set; }
        public string ISVISIT { get; set; }

        public string REMARK { get; set; }

        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
    }
}
