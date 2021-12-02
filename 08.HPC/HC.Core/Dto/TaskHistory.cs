using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Dto
{
    public class TaskHistory : BaseDto
    {
        /// <summary>
        ///사업장아이디
        /// <summary>
        public long SITE_ID { get; set; }
        /// <summary>
        ///업무명
        /// <summary>
        public string TASKNAME { get; set; }
        /// <summary>
        ///생성일시
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///생성자
        /// <summary>
        public string CREATEDUSER { get; set; }

    }
}
