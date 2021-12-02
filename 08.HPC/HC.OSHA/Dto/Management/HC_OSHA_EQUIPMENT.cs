namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HC_OSHA_EQUIPMENT : BaseDto
    {
        public long ID { get; set; }
        public long SITE_ID { get; set; }
        public DateTime? REGDATE { get; set; }
        public string NAME { get; set; }
        public string MODELNAME { get; set; }
        public string SERIALNUMBER { get; set; }
        public string REMARK { get; set; }

        public IsDeleted ISDELETED { get; set; }
        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
     
    }
}

