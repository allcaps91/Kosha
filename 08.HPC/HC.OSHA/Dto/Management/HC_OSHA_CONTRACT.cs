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
        /// 계약일
        /// </summary>
        [MTSNotEmpty]
		public string CONTRACTDATE { get; set; }

        public string TERMINATEDATE { get; set; }
        /// <summary>
        /// 특약사항
        /// </summary>
        public string SPECIALCONTRACT { get; set; }

        /// <summary>
        /// 총근로자수
        /// </summary>
        [MTSZero]
        public long WORKERTOTALCOUNT { get; set; } 

        /// <summary>
        /// 사무직남
        /// </summary>
		public long WORKERWHITEMALECOUNT { get; set; } 

        /// <summary>
        /// 사무직여
        /// </summary>
		public long WORKERWHITEFEMALECOUNT { get; set; } 

        /// <summary>
        /// 비사무직남
        /// </summary>
		public long WORKERBLUEMALECOUNT { get; set; } 

        /// <summary>
        /// 비사무직여
        /// </summary>
		public long WORKERBLUEFEMALECOUNT { get; set; } 

        /// <summary>
        /// 담당요원 총인원
        /// </summary>
		public long MANAGEWORKERCOUNT { get; set; } 

        /// <summary>
        /// 담당의사
        /// </summary>
		public string MANAGEDOCTOR { get; set; } 

        /// <summary>
        /// 담당의사시작년월
        /// </summary>
		public string MANAGEDOCTORSTARTDATE { get; set; } 

        /// <summary>
        /// 담당의사방문주기
        /// </summary>
		public long MANAGEDOCTORCOUNT { get; set; } 

        /// <summary>
        /// 담당간호사
        /// </summary>
		public string MANAGENURSE { get; set; } 

        /// <summary>
        /// 담당간호사시작년월
        /// </summary>
		public string MANAGENURSESTARTDATE { get; set; } 

        /// <summary>
        /// 담당간호사방문주기
        /// </summary>
		public long MANAGENURSECOUNT { get; set; } 

        /// <summary>
        /// 산업위생기사
        /// </summary>
		public string MANAGEENGINEER { get; set; } 

        /// <summary>
        /// 산업위생기사시작년월
        /// </summary>
		public string MANAGEENGINEERSTARTDATE { get; set; } 

        /// <summary>
        /// 산업기사방문주기
        /// </summary>
		public long MANAGEENGINEERCOUNT { get; set; } 

        /// <summary>
        /// 근무일(주)
        /// </summary>
		public string VISITWEEK { get; set; }

        /// <summary>
        /// 방문장소
        /// </summary>
        public string VISITPLACE { get; set; }

        /// <summary>
        /// 근무일
        /// </summary>
        public string VISITDAY { get; set; } 

        /// <summary>
        /// 대행수수료
        /// </summary>
		public long COMMISSION { get; set; } 

        /// <summary>
        /// 선임일
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
        /// 위치 공단=0, 농공단지=1, 도심내=2, 기타=3
        /// </summary>
		public string POSITION { get; set; } 

        /// <summary>
        /// 교대제 유=0, 무=1
        /// </summary>
		public string ISROTATION { get; set; } 

        /// <summary>
        /// 생산방식 독릭=0, 하청=1
        /// </summary>
		public string ISPRODUCTTYPE { get; set; } 

        /// <summary>
        /// 노동조합유무
        /// </summary>
		public string ISLABOR { get; set; }
        /// <summary>
        /// 건물소유  자가=0, 임대 = 1
        /// </summary>
        public string BUILDINGTYPE { get; set; }



        /// <summary>
        /// 사업장 시작시간
        /// </summary>
        public string WORKSTARTTIME { get; set; } 

        /// <summary>
        /// 사업장죵료시간
        /// </summary>
		public string WORKENDTIME { get; set; } 

        /// <summary>
        /// 사업장조회시간
        /// </summary>
		public string WORKMEETTIME { get; set; } 

        /// <summary>
        /// 교대시간
        /// </summary>
		public string WORKROTATIONTIME { get; set; } 

        /// <summary>
        /// 점심시간
        /// </summary>
		public string WORKLUANCHTIME { get; set; } 

        /// <summary>
        /// 휴식시간
        /// </summary>
		public string WORKRESTTIME { get; set; } 

        /// <summary>
        /// 교육시간
        /// </summary>
		public string WORKEDUTIME { get; set; } 

        /// <summary>
        /// .기타시간
        /// </summary>
		public string WORKETCTIME { get; set; } 

        /// <summary>
        /// 계약여부
        /// </summary>
		public string ISCONTRACT { get; set; } 

        /// <summary>
        /// 작업환경측정대상 여부
        /// </summary>
		public string ISWEM { get; set; } 

        /// <summary>
        /// 작업환경측정대상 데이타
        /// </summary>
		public string ISWEMDATA { get; set; } 

        /// <summary>
        /// 산업안전보건위원회설치 여부
        /// </summary>
		public string ISCOMMITTEE { get; set; } 

        /// <summary>
        /// 근골격계 유해요인조사 대상 여부
        /// </summary>
		public string ISSKELETON { get; set; }

        /// <summary>
        /// 근골격계 유해요인조사 대상 실시일
        /// </summary>
        public string ISSKELETONDATE { get; set; } 

        /// <summary>
        /// 밀폐공간보거느로그램 대상
        /// </summary>
		public string ISSPACEPROGRAM { get; set; }

        /// <summary>
        /// 밀폐공간보거느로그램 대상 실시일
        /// </summary>
        public string ISSPACEPROGRAMDATE { get; set; } 

        /// <summary>
        /// 청력보존프로그램 대상 여부
        /// </summary>
		public string ISEARPROGRAM { get; set; }

        /// <summary>
        /// 청력보존프로그램 실시일
        /// </summary>
        public string ISEARPROGRAMDATE { get; set; } 

        /// <summary>
        /// 직무스트레스평가대상 여부
        /// </summary>
		public string ISSTRESS { get; set; }

        /// <summary>
        /// 직무스트레스평가대상 실시일
        /// </summary>
        public string ISSTRESSDATE { get; set; } 

        /// <summary>
        /// 뇌심혈관 평가대상 여부
        /// </summary>
		public string ISBRAINTEST { get; set; }

        /// <summary>
        /// 뇌심혈관 평가대상 실시일
        /// </summary>
        public string ISBRAINTESTDATE { get; set; } 

        /// <summary>
        /// 특별관리물질취급 여부
        /// </summary>
		public string ISSPECIAL { get; set; }
        /// <summary>
        /// 참고사항
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 특별관리물질취급 데이타
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
