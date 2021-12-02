namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 사업장 원하청
    /// </summary>
    public class HC_OSHA_RELATION : BaseDto
    {
        
        public long ID { get; set; }
        public long PARENT_ID { get; set; }
        public long CHILD_ID { get; set; }

        public string NAME { get; set; }
    }
}
