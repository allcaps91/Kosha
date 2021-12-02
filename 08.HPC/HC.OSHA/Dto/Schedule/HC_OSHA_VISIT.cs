namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 방문내역
    /// </summary>
    public class HC_OSHA_VISIT : BaseDto
    {
        public HC_OSHA_VISIT()
        {
            this.VISITDATETIME = DateTime.Now;
            this.VISITTYPE ="1";
            this.ISPRECHARGE = "N";
            this.ISFEE = "N";
            this.ISKUKGO = "N";
        }

        /// <summary>
        ///아이디
        /// <summary>
        public long ID { get; set; }
        /// <summary>
        ///방문일정아이디
        /// <summary>
        public long SCHEDULE_ID { get; set; }
        /// <summary>
        ///사업장아이디
        /// <summary>
        public long SITE_ID { get; set; }
        /// <summary>
        ///견적아이디
        /// <summary>
        public long ESTIMATE_ID { get; set; }
        /// <summary>
        ///방문일시
        /// <summary>
        public DateTime? VISITDATETIME { get; set; }
        /// <summary>
        ///방문구분
        /// <summary>
        public string VISITTYPE { get; set; }
        /// <summary>
        ///시작시간
        /// <summary>
        public string STARTTIME { get; set; }
        /// <summary>
        ///종료시간
        /// <summary>
        public string ENDTIME { get; set; }
        /// <summary>
        /// 소요시간,분 문자열
        /// </summary>
        public string TakeHourAndMinute { get; set; }

        /// <summary>
        ///소요시간
        /// <summary>
        public long TAKEHOUR { get; set; }
        /// <summary>
        ///소요분
        /// <summary>
        public long TAKEMINUTE { get; set; }
        /// <summary>
        ///방문유저이름
        /// <summary>
        public string VISITUSERNAME { get; set; }
        /// <summary>
        ///방문유저
        /// <summary>
        [MTSNotEmpty]
        public string VISITUSER { get; set; }
        /// <summary>
        ///방문의사이름
        /// <summary>
        public string VISITDOCTORNAME { get; set; }
        /// <summary>
        ///방문의사
        /// <summary>
        public string VISITDOCTOR { get; set; }
        /// <summary>
        ///수수료발생여부
        /// <summary>
        public string ISFEE { get; set; }
        /// <summary>
        ///국고여부
        /// <summary>
        public string ISKUKGO { get; set; }

        /// <summary>
        /// 선청구 여부
        /// </summary>
        public string ISPRECHARGE { get; set; }
        /// <summary>
        ///방문일지 기타
        /// <summary>
        public string REMARK { get; set; }


        /// <summary>
        ///삭제여부
        /// <summary>
        public string ISDELETED { get; set; }
        /// <summary>
        ///
        /// <summary>
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
