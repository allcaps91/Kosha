using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Model
{
    public class DataSyncModel : BaseDto
    {
        public long ID { get; set; }
        public string TABLENAME { get; set; }
        /// <summary>
        /// 테이블의 키 복합키의 경우 , 로 구분
        /// </summary>
        public string TABLEKEY { get; set; }
        /// <summary>
        /// DML 종류 I (insert), U (update), D (delete), N(ISDELETED =Y)
        /// </summary>
        public string DMLTYPE { get; set; }

        /// <summary>
        /// 싱크 완료 여부
        /// </summary>
        public string ISSYNC { get; set; }

        public string COMMENTS { get; set; }
        public string MESSAGE { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
        public string CREATEDUSERNAME { get; set; }
    }
}
