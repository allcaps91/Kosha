using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상태보고서 사업장 현황(의사, 간호사)
    /// </summary>
    public class SiteStatusDto : BaseDto
    {
        public SiteStatusDto()
        {
            
        }
        public string SITENAME { get; set; }
        /// <summary>
        /// 타병원검진
        /// </summary>
        public string EXTDATA { get; set; }
        
        /// <summary>
        ///현재인원
        /// <summary>
        public long CURRENTWORKERCOUNT { get; set; }
     
        /// <summary>
        ///채용인원
        /// <summary>
        public long NEWWORKERCOUNT { get; set; }
        /// <summary>
        ///퇴직인원
        /// <summary>
        public long RETIREWORKERCOUNT { get; set; }
        /// <summary>
        ///작업변경
        /// <summary>
        public long CHANGEWORKERCOUNT { get; set; }
        /// <summary>
        ///재해자수
        /// <summary>
        public long ACCIDENTWORKERCOUNT { get; set; }
        /// <summary>
        ///사망자수
        /// <summary>
        public long DEADWORKERCOUNT { get; set; }
        /// <summary>
        ///부상자수
        /// <summary>
        public long INJURYWORKERCOUNT { get; set; }
        /// <summary>
        ///업무상질병자수
        /// <summary>
        public long BIZDISEASEWORKERCOUNT { get; set; }
        /// <summary>
        ///일반검진예정일
        /// <summary>
        public string GENERALHEALTHCHECKDATE { get; set; }
        /// <summary>
        ///특수검진에정일
        /// <summary>
        public string SPECIALHEALTHCHECKDATE { get; set; }

        public long GENERALTOTALCOUNT { get; set; }
        public long SPECIALTOTALCOUNT { get; set; }

        /// <summary>
        ///일반검진D2
        /// <summary>
        public long GENERALD2COUNT { get; set; }
        /// <summary>
        ///일반검진C2
        /// <summary>
        public long GENERALC2COUNT { get; set; }
        /// <summary>
        ///특수검진D1
        /// <summary>
        public long SPECIALD1COUNT { get; set; }
        /// <summary>
        ///특수검진C1
        /// <summary>
        public long SPECIALC1COUNT { get; set; }
        /// <summary>
        ///특수검진D2
        /// <summary>
        public long SPECIALD2COUNT { get; set; }
        /// <summary>
        ///특수검진C2
        /// <summary>
        public long SPECIALC2COUNT { get; set; }
        /// <summary>
        ///특수검진DN
        /// <summary>
        public long SPECIALDNCOUNT { get; set; }
        /// <summary>
        ///특수검진CN
        /// <summary>
        public long SPECIALCNCOUNT { get; set; }
        /// <summary>
        ///자연환경측정예정일
        /// <summary>
        public string WEMDATE { get; set; }
        /// <summary>
        ///노출기준 미만
        /// <summary>
        public string WEMEXPORSURE { get; set; }

        /// <summary>
        ///노출기준 초과
        /// <summary>
        public string WEMEXPORSURE2 { get; set; }
        /// <summary>
        ///노출기준초과
        /// <summary>
        public string WEMEXPORSUREREMARK { get; set; }
        /// <summary>
        ///주요유해인자
        /// <summary>
        public string WEMHARMFULFACTORS { get; set; }
  
        /// <summary>
        /// 부서명
        /// </summary>
        public string DEPTNAME { get; set; }

    }
}
