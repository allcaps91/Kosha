namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// �����ī�� �����Ƿ�ü��
    /// </summary>
    public class HC_OSHA_CARD21 : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long SITE_ID { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        public string YEAR { get; set; }

        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        public string ISHOSPITAL { get; set; } 

        /// <summary>
        /// �����Ƿ���
        /// </summary>
		public string HOSPITALNAME { get; set; } 

        /// <summary>
        /// ������
        /// </summary>
		public string ADDRESS { get; set; } 

        /// <summary>
        /// ����ó
        /// </summary>
		public string TEL { get; set; } 

        /// <summary>
        /// ����óġ��������� ����
        /// </summary>
		public string ISMANAGER { get; set; } 

        /// <summary>
        /// ����óġ�����
        /// </summary>
		public string MANAGERNAME { get; set; } 

        /// <summary>
        /// ������ ����
        /// </summary>
		public string ISAIDKIT { get; set; } 

        /// <summary>
        /// ������  ����
        /// </summary>
		public string AIDKITTYPE { get; set; } 

        /// <summary>
        /// ���
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// �����ⱸ üũ�ڽ�
        /// </summary>
		public string CLEAN1CHECKBOX { get; set; } 

        /// <summary>
        /// �����ⱸ ��Ÿ
        /// </summary>
		public string CLEAN1ETC { get; set; } 

        /// <summary>
        /// ������ǰüũ�ڽ�
        /// </summary>
		public string CLEAN2CHECKBOX { get; set; } 

        /// <summary>
        /// ������ǰ ��Ÿ
        /// </summary>
		public string CLEAN2ETC { get; set; } 

        /// <summary>
        /// �ܿ�� üũ�ڽ�
        /// </summary>
		public string CLEAN3CHECKBOX { get; set; } 

        /// <summary>
        /// �ܿ�� ��Ÿ
        /// </summary>
		public string CLEAN3ETC { get; set; } 

        
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CARD21()
        {
        }
    }
}
