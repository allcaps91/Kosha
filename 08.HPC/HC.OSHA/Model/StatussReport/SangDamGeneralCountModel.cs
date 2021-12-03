using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Model.StatussReport
{
    /// <summary>
    /// 건강상담 일반질병 건수
    /// SELECT SUM(DECODE(PANJENGB1,'1','1','0')) B비만, SUM(DECODE(PANJENGB2,'1','1','0')) B고혈압, SUM(DECODE(PANJENGB3,'1','1','0')) B이상지질혈증, SUM(DECODE(PANJENGB4,'1','1','0')) B간기능,
//SM(DECODE(PANJENGB5,'1','1','0')) B당뇨, SUM(DECODE(PANJENGB6,'1','1','0')) B신장기능, SUM(DECODE(PANJENGB7,'1','1','0')) B빈혈, SUM(DECODE(PANJENGB8,'1','1','0')) B골다공증, 
//SUM(DECODE(PANJENGB9,'1','1','0')) B기타질환,SUM(DECODE(PANJENGB10,'1','1','0')) B비활동성폐결핵,
//SUM(DECODE(PANJENGR1,'1','1','0')) R폐결핵, SUM(DECODE(PANJENGR2,'1','1','0')) R기타흉부질환, SUM(DECODE(PANJENGR4,'1','1','0')) R이상지질혈,SUM(DECODE(PANJENGR5,'1','1','0')) R간장질환, 
//SUM(DECODE(PANJENGR7,'1','1','0')) R신장질환 ,SUM(DECODE(PANJENGR8,'1','1','0')) R빈혈, SUM(DECODE(PANJENGR9,'1','1','0')) R골다공증, SUM(DECODE(PANJENGR10,'1','1','0')) R기타,
//SUM(DECODE(PANJENGR11,'1','1','0')) R비만, SUM(DECODE(PANJENGR12,'1','1','0')) R난청,
//SUM(DECODE(PANJENGR3,'1','1','0')) U고혈압, SUM(DECODE(PANJENGR6,'1','1','0')) U당뇨,
//SUM(DECODE(PANJENGU1,'1','1','0')) D고혈압, SUM(DECODE(PANJENGU2,'1','1','0')) D당뇨병 ,SUM(DECODE(PANJENGU3,'1','1','0')) D이상지질혈증, SUM(DECODE(PANJENGU4,'1','1','0')) D폐결핵
//FROM ADMIN.HIC_RES_BOHUM1 WHERE WRTNO in ('995732') ;

    /// </summary>
    public class SangDamGeneralCountModel
    {
        /// <summary>
        /// 상담 인원
        /// </summary>
        public long SANGDAMCOUNT { get; set; }
        /// <summary>
        /// 고혈압D
        /// </summary>
        public long PANJENGU1 { get; set; }
        /// <summary>
        /// 고혈압C
        /// </summary>
        public long PANJENGR3 { get; set; }
        /// <summary>
        /// 당뇨D
        /// </summary>
        public long PANJENGU2 { get; set; }
        /// <summary>
        /// 당뇨C
        /// </summary>
        public long PANJENGR6 { get; set; }

        /// <summary>
        /// 간장질환 C
        /// </summary>
        public long PANJENGR5 { get; set; }

        
        /// <summary>
        /// 이상지질D
        /// </summary>
        public long PANJENGU3 { get; set; }
        /// <summary>
        /// 이상지질C
        /// </summary>
        public long PANJENGR4 { get; set; }
        

        public long ETC_C1 { get; set; }
        public long ETC_C2 { get; set; }
        /// <summary>
        /// R간장질환 
        /// </summary>
        public long ETC_C3 { get; set; }
        public long ETC_C4 { get; set; }
        public long ETC_C5 { get; set; }
        public long ETC_C6 { get; set; }
        public long ETC_C7 { get; set; }
        public long ETC_C8 { get; set; }
        public long ETC_C9 { get; set; }
        public long ETC_D1 { get; set; }

        //R난청, 빈혈은 일반질병 기타 D2로 표시

        public long PANJENGR12 { get; set; }//R난청
        public long PANJENGR8 { get; set; } // R빈혈
    }
}
