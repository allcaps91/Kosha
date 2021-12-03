namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// �湮����
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
        ///���̵�
        /// <summary>
        public long ID { get; set; }
        /// <summary>
        ///�湮�������̵�
        /// <summary>
        public long SCHEDULE_ID { get; set; }
        /// <summary>
        ///�������̵�
        /// <summary>
        public long SITE_ID { get; set; }
        /// <summary>
        ///�������̵�
        /// <summary>
        public long ESTIMATE_ID { get; set; }
        /// <summary>
        ///�湮�Ͻ�
        /// <summary>
        public DateTime? VISITDATETIME { get; set; }
        /// <summary>
        ///�湮����
        /// <summary>
        public string VISITTYPE { get; set; }
        /// <summary>
        ///���۽ð�
        /// <summary>
        public string STARTTIME { get; set; }
        /// <summary>
        ///����ð�
        /// <summary>
        public string ENDTIME { get; set; }
        /// <summary>
        /// �ҿ�ð�,�� ���ڿ�
        /// </summary>
        public string TakeHourAndMinute { get; set; }

        /// <summary>
        ///�ҿ�ð�
        /// <summary>
        public long TAKEHOUR { get; set; }
        /// <summary>
        ///�ҿ��
        /// <summary>
        public long TAKEMINUTE { get; set; }
        /// <summary>
        ///�湮�����̸�
        /// <summary>
        public string VISITUSERNAME { get; set; }
        /// <summary>
        ///�湮����
        /// <summary>
        [MTSNotEmpty]
        public string VISITUSER { get; set; }
        /// <summary>
        ///�湮�ǻ��̸�
        /// <summary>
        public string VISITDOCTORNAME { get; set; }
        /// <summary>
        ///�湮�ǻ�
        /// <summary>
        public string VISITDOCTOR { get; set; }
        /// <summary>
        ///������߻�����
        /// <summary>
        public string ISFEE { get; set; }
        /// <summary>
        ///������
        /// <summary>
        public string ISKUKGO { get; set; }

        /// <summary>
        /// ��û�� ����
        /// </summary>
        public string ISPRECHARGE { get; set; }
        /// <summary>
        ///�湮���� ��Ÿ
        /// <summary>
        public string REMARK { get; set; }


        /// <summary>
        ///��������
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
