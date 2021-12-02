using ComBase.Mvc;
using ComBase.Mvc.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Dto
{
    /// <summary>
    /// 상용구
    /// </summary>
    public class MacrowordDto :  BaseDto
    {

        public long ID { get; set; }
        /// <summary>
        ///폼명
        /// <summary>
        public string FORMNAME { get; set; }
        /// <summary>
        ///컨트롤명
        /// <summary>
        public string CONTROL { get; set; }
        /// <summary>
        ///제목
        /// <summary>
        [MTSNotEmpty]
        public string TITLE { get; set; }

        public string SUBTITLE { get; set; }
        /// <summary>
        ///내용
        /// <summary>
        [MTSNotEmpty]
        public string CONTENT { get; set; }
        public string CONTENT2 { get; set; }
        public double DISPSEQ { get; set; }

        public DateTime? MODIFIED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string MODIFIEDUSER { get; set; }
        /// <summary>
        ///
        /// <summary>
        public DateTime? CREATED { get; set; }
        /// <summary>
        ///
        /// <summary>
        public string CREATEDUSER { get; set; }
    }
}
