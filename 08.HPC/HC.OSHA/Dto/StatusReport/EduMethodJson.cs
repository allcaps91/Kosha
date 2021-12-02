using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Dto
{ /// <summary>
  /// 상태보고사 산업위생 보건교육 - 교육종류
  /// </summary>
    public class EduMethodJson : BaseDto
    {
        public string ChkEduMethod1 { get; set; } //다수
        public string ChkEduMethod2 { get; set; } //소그룹
        public string ChkEduMethod3 { get; set; } //상담식
    }
}
