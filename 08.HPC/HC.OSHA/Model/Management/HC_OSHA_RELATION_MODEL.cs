namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.Core.Model;
    using System;
    using System.Collections;


    public class HC_OSHA_RELATION_MODEL : BaseDto
    {

        public long ID { get; set; }

        public long PARENT_ID { get; set; }

        public string PARENT_NAME { get; set; }

        /// <summary>
        /// 하청 사업자아이디
        /// </summary>
        public long CHILD_ID { get; set; }


        /// <summary>
        /// 사업자명
        /// </summary>
        public string CHILD_NAME { get; set; }
    }
}
