using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    /// <summary>
    /// 외래진료검사의뢰
    /// </summary>
    public class SangDamOutExamCountModel : BaseDto
    {
        public string Name { get; set; }
        public string Exam { get; set; }
    }
}
