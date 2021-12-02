namespace HC.Core.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    /// <summary>
    /// 
    /// </summary>
    public class DataSyncDto : BaseDto
    {
        public long ID { get; set; }
        public string TABLENAME { get; set; }
        /// <summary>
        /// 테이블의 키 복합키의 경우 , 로 구분
        /// </summary>
        public string TABLEKEY { get; set; }
        /// <summary>
        /// INSERT시 새로발생된 키
        /// </summary>
        public string NEWTABLEKEY { get; set; }
        /// <summary>
        /// DML 종류 I (insert), U (update), D (delete), N(ISDELETED =Y)
        /// </summary>
        public string DMLTYPE { get; set; }

        /// <summary>
        /// 싱크 완료 여부
        /// </summary>
        public string ISSYNC { get; set; }

        public string MESSAGE { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
    }
}
