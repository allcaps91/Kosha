using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{
    /// <summary>
    /// 상태보고사 산업위생 보건교육 - 교육종류
    /// </summary>
    public class EduTypeJson : BaseDto
    {
        public string ChkEduType1 { get; set; } //정기교육
        public string ChkEduType2 { get; set; } //채용시
        public string ChkEduType3 { get; set; } //변경시
        public string ChkEduType4 { get; set; } //특별교육
    }
}
