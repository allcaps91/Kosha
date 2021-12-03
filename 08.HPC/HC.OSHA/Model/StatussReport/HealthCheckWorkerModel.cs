using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Model
{
    /// <summary>
    /// 근로자 상담관리
    /// 근로자 목록 모델
    /// </summary>
    public class HealthCheckWorkerModel : BaseDto
    {
        public string Worker_ID { get; set; }
        public string Pano { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public string Jumin { get; set; }

        public string Dept { get; set; }
        public string AgeAndGender { get; set; }

        public string Year { get; set; }
        public string Panjeong { get; set; }

        //특수 Y        
        public string IsSpecial { get; set; }
        public string IsManageOsha { get; set; }

        public string REMARK { get; set; }

        public DateTime? END_DATE { get; set; }

        public long SITEID { get; set; }



        public string PANJENGR1 { get; set; }
        public string PANJENGR2 { get; set; }
        public string PANJENGR3 { get; set; }
        public string PANJENGR4 { get; set; }
        public string PANJENGR5 { get; set; }
        public string PANJENGR6 { get; set; }
        public string PANJENGR7 { get; set; }
        public string PANJENGR8 { get; set; }
        public string PANJENGR9 { get; set; }
        public string PANJENGR10 { get; set; }
        public string PANJENGR11 { get; set; }
        public string PANJENGR12 { get; set; }

        public string PANJENGD11 { get; set; }
        public string PANJENGD12 { get; set; }
        public string PANJENGD13 { get; set; }
        public string PANJENGD21 { get; set; }
        public string PANJENGD22 { get; set; }
        public string PANJENGD23 { get; set; }

        public string PANJENGU1 { get; set; }
        public string PANJENGU2 { get; set; }
        public string PANJENGU3 { get; set; }
        public string PANJENGU4 { get; set; }

        public string GJJONG { get; set; }
        public string BROWID { get; set; }
        public string CROWID { get; set; }
        public string IPSADATE { get; set; }
        public string BUSENAME { get; set; }
        public string PANJENG { get; set; }

        public long PANDRNO { get; set; }
        public long WRTNO { get; set; }
        public long CWRTNO { get; set; }

        public string JEPDATE { get; set; }

        public string GJCHASU { get; set; }
        public string UCODES { get; set; }

        public string PanName { get; set; }
        public string Flag { get; set; }
        public string SOGEN { get; set; }
    }
}
