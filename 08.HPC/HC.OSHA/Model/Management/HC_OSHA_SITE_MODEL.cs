namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.Core.Model;
    using System;
    using System.Collections;


    /// <summary>
    /// ���ǰ��� �����
    /// </summary>
    public class HC_OSHA_SITE_MODEL : BaseDto, ISiteModel
    {
        /// <summary>
        /// ������ �۾� ����
        /// </summary>
        public string LASTMODIFIED { get; set; }

        /// <summary>
        /// ��û�� ������ �ִ��� ���� (Y,N)
        /// </summary>
        public string HASCHILD { get; set; }

        /// <summary>
        /// ��û����� ���̵�
        /// </summary>
        public long PARENTSITE_ID { get; set; }
        /// <summary>
        /// 
        /// ��û����ڸ�
        /// </summary>
        public string PARENTSITE_NAME { get; set; }

        /// <summary>
        /// ����ھ��̵�
        /// </summary>
		public long ID { get; set; }
        public long SITE_ID { get; set; }


        /// <summary>
        /// ����ڸ�
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TEL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string FAX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string EMAIL { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ADDRESS { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZNUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZTYPE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZJONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZKIHO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? BIZCREATEDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string INSURANCE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string INDUSTRIALNUMBER { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BIZJIDOWON { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string LABOR { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CEONAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ISACTIVE { get; set; }

        public long WORKERTOTALCOUNT { get; set; }

        public long VISITWEEK { get; set; }
        public long VISITDAY { get; set; }
        public long MANAGEDOCTORCOUNT { get; set; }
        public long MANAGENURSECOUNT { get; set; }
        public long MANAGEENGINEERCOUNT { get; set; }
        
        public string MANAGEENGINEERSTARTDATE { get; set; }
        public string MANAGEDOCTORSTARTDATE{ get; set; }
        public string MANAGENURSESTARTDATE{ get; set; }

        public HC_OSHA_SITE_MODEL()
        {
        }

        public HC_OSHA_SITE_MODEL(long id , string name)
        {
            this.ID = id;
            this.NAME = name;
        }
    }
}
