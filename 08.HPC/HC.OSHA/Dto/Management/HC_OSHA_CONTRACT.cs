namespace HC.OSHA.Dto
{
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using ComBase.Mvc.Validation;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HC_OSHA_CONTRACT : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ESTIMATE_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
    
        public long OSHA_SITE_ID { get; set; } 

        /// <summary>
        /// �����
        /// </summary>
        [MTSNotEmpty]
		public string CONTRACTDATE { get; set; }

        public string TERMINATEDATE { get; set; }
        /// <summary>
        /// Ư�����
        /// </summary>
        public string SPECIALCONTRACT { get; set; }

        /// <summary>
        /// �ѱٷ��ڼ�
        /// </summary>
        [MTSZero]
        public long WORKERTOTALCOUNT { get; set; } 

        /// <summary>
        /// �繫����
        /// </summary>
		public long WORKERWHITEMALECOUNT { get; set; } 

        /// <summary>
        /// �繫����
        /// </summary>
		public long WORKERWHITEFEMALECOUNT { get; set; } 

        /// <summary>
        /// ��繫����
        /// </summary>
		public long WORKERBLUEMALECOUNT { get; set; } 

        /// <summary>
        /// ��繫����
        /// </summary>
		public long WORKERBLUEFEMALECOUNT { get; set; } 

        /// <summary>
        /// ����� ���ο�
        /// </summary>
		public long MANAGEWORKERCOUNT { get; set; } 

        /// <summary>
        /// ����ǻ�
        /// </summary>
		public string MANAGEDOCTOR { get; set; } 

        /// <summary>
        /// ����ǻ���۳��
        /// </summary>
		public string MANAGEDOCTORSTARTDATE { get; set; } 

        /// <summary>
        /// ����ǻ�湮�ֱ�
        /// </summary>
		public long MANAGEDOCTORCOUNT { get; set; } 

        /// <summary>
        /// ��簣ȣ��
        /// </summary>
		public string MANAGENURSE { get; set; } 

        /// <summary>
        /// ��簣ȣ����۳��
        /// </summary>
		public string MANAGENURSESTARTDATE { get; set; } 

        /// <summary>
        /// ��簣ȣ��湮�ֱ�
        /// </summary>
		public long MANAGENURSECOUNT { get; set; } 

        /// <summary>
        /// ����������
        /// </summary>
		public string MANAGEENGINEER { get; set; } 

        /// <summary>
        /// ������������۳��
        /// </summary>
		public string MANAGEENGINEERSTARTDATE { get; set; } 

        /// <summary>
        /// ������湮�ֱ�
        /// </summary>
		public long MANAGEENGINEERCOUNT { get; set; } 

        /// <summary>
        /// �ٹ���(��)
        /// </summary>
		public string VISITWEEK { get; set; }

        /// <summary>
        /// �湮���
        /// </summary>
        public string VISITPLACE { get; set; }

        /// <summary>
        /// �ٹ���
        /// </summary>
        public string VISITDAY { get; set; } 

        /// <summary>
        /// ���������
        /// </summary>
		public long COMMISSION { get; set; } 

        /// <summary>
        /// ������
        /// </summary>
		public string DECLAREDAY { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
		public string CONTRACTSTARTDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MTSNotEmpty]
        public string CONTRACTENDDATE { get; set; } 

        /// <summary>
        /// ��ġ ����=0, �������=1, ���ɳ�=2, ��Ÿ=3
        /// </summary>
		public string POSITION { get; set; } 

        /// <summary>
        /// ������ ��=0, ��=1
        /// </summary>
		public string ISROTATION { get; set; } 

        /// <summary>
        /// ������ ����=0, ��û=1
        /// </summary>
		public string ISPRODUCTTYPE { get; set; } 

        /// <summary>
        /// �뵿��������
        /// </summary>
		public string ISLABOR { get; set; }
        /// <summary>
        /// �ǹ�����  �ڰ�=0, �Ӵ� = 1
        /// </summary>
        public string BUILDINGTYPE { get; set; }



        /// <summary>
        /// ����� ���۽ð�
        /// </summary>
        public string WORKSTARTTIME { get; set; } 

        /// <summary>
        /// ������շ�ð�
        /// </summary>
		public string WORKENDTIME { get; set; } 

        /// <summary>
        /// �������ȸ�ð�
        /// </summary>
		public string WORKMEETTIME { get; set; } 

        /// <summary>
        /// ����ð�
        /// </summary>
		public string WORKROTATIONTIME { get; set; } 

        /// <summary>
        /// ���ɽð�
        /// </summary>
		public string WORKLUANCHTIME { get; set; } 

        /// <summary>
        /// �޽Ľð�
        /// </summary>
		public string WORKRESTTIME { get; set; } 

        /// <summary>
        /// �����ð�
        /// </summary>
		public string WORKEDUTIME { get; set; } 

        /// <summary>
        /// .��Ÿ�ð�
        /// </summary>
		public string WORKETCTIME { get; set; } 

        /// <summary>
        /// ��࿩��
        /// </summary>
		public string ISCONTRACT { get; set; } 

        /// <summary>
        /// �۾�ȯ��������� ����
        /// </summary>
		public string ISWEM { get; set; } 

        /// <summary>
        /// �۾�ȯ��������� ����Ÿ
        /// </summary>
		public string ISWEMDATA { get; set; } 

        /// <summary>
        /// ���������������ȸ��ġ ����
        /// </summary>
		public string ISCOMMITTEE { get; set; } 

        /// <summary>
        /// �ٰ�ݰ� ���ؿ������� ��� ����
        /// </summary>
		public string ISSKELETON { get; set; }

        /// <summary>
        /// �ٰ�ݰ� ���ؿ������� ��� �ǽ���
        /// </summary>
        public string ISSKELETONDATE { get; set; } 

        /// <summary>
        /// ����������Ŵ��α׷� ���
        /// </summary>
		public string ISSPACEPROGRAM { get; set; }

        /// <summary>
        /// ����������Ŵ��α׷� ��� �ǽ���
        /// </summary>
        public string ISSPACEPROGRAMDATE { get; set; } 

        /// <summary>
        /// û�º������α׷� ��� ����
        /// </summary>
		public string ISEARPROGRAM { get; set; }

        /// <summary>
        /// û�º������α׷� �ǽ���
        /// </summary>
        public string ISEARPROGRAMDATE { get; set; } 

        /// <summary>
        /// ������Ʈ�����򰡴�� ����
        /// </summary>
		public string ISSTRESS { get; set; }

        /// <summary>
        /// ������Ʈ�����򰡴�� �ǽ���
        /// </summary>
        public string ISSTRESSDATE { get; set; } 

        /// <summary>
        /// �������� �򰡴�� ����
        /// </summary>
		public string ISBRAINTEST { get; set; }

        /// <summary>
        /// �������� �򰡴�� �ǽ���
        /// </summary>
        public string ISBRAINTESTDATE { get; set; } 

        /// <summary>
        /// Ư������������� ����
        /// </summary>
		public string ISSPECIAL { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// Ư������������� ����Ÿ
        /// </summary>
        public string ISSPECIALDATA { get; set; }
        public IsDeleted ISDELETED { get; set; }
        public DateTime? MODIFIED { get; set; }
        public string MODIFIEDUSER { get; set; }
        public DateTime? CREATED { get; set; }
        public string CREATEDUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HC_OSHA_CONTRACT()
        {
            this.ISCONTRACT = "N";
            
        }
    }
}
