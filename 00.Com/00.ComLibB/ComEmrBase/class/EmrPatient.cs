namespace ComEmrBase
{
    public class EmrPatient
    {
        /// <summary>
        /// 등록번호
        /// </summary>
        public string ptNo;         
        /// <summary>
        /// 성명
        /// </summary>
        public string ptName;       
        /// <summary>
        /// 주민번호 앞
        /// </summary>
        public string ssno1;        
        /// <summary>
        /// 주민번호 뒤
        /// </summary>
        public string ssno2;        
        /// <summary>
        /// 생일
        /// </summary>
        public string birthdate;    

        /// <summary>
        /// 성별
        /// </summary>
        public string sex;          
        /// <summary>
        /// 나이
        /// </summary>
        public string age;

        /// <summary>
        /// 전화번호
        /// </summary>
        public string tel;          //
        /// <summary>
        /// 핸드폰
        /// </summary>
        public string celphno;      //
        /// <summary>
        /// 우편번호(Old)
        /// </summary>
        public string zipcd;        //
        /// <summary>
        /// 주소(Old)
        /// </summary>
        public string addr;         //
        /// <summary>
        /// 상세주소(Old)
        /// </summary>
        public string address;      //
        /// <summary>
        /// 도로명 우편번호
        /// </summary>
        public string zipcdLoad;    //
        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string addrLoad;     //
        /// <summary>
        /// 도로명 상세주소
        /// </summary>
        public string addressLoad;  //


        // 2017-06-09 법정감염병신고땜에 추가함
        /// <summary>
        /// 도로명주소
        /// </summary>
        public string rdnmAddr;         // 
        /// <summary>
        /// 도로명주소 상세
        /// </summary>
        public string rdnmAddr_dtl;     // 
        /// <summary>
        /// 피보험자 이름
        /// </summary>
        public string pibo_name;        // 

        /// <summary>
        /// 접수번호(KOSMOS_EMR.EMR_TREATT) - TREATNO
        /// </summary>
        public string acpNo;            //
        /// <summary>
        /// 외래(O), 입원(I)
        /// </summary>
        public string inOutCls;         //
        /// <summary>
        /// 내원(입원)일자
        /// </summary>
        public string medFrDate;        //

        /// <summary>
        /// 동국대 경주병원만 사용함
        /// </summary>
        public string medIP_INDATE;     //

        /// <summary>
        /// 내원(입원)시간
        /// </summary>
        public string medFrTime;        //
        /// <summary>
        /// 퇴원일자
        /// </summary>
        public string medEndDate;       //
        /// <summary>
        /// 퇴원시간
        /// </summary>
        public string medEndTime;       //
        /// <summary>
        /// 퇴원예고시간
        /// </summary>
        public string medEndexDate;     //
        /// <summary>
        /// 현재 진료과
        /// </summary>
        public string medDeptCd;        //
        /// <summary>
        /// 현재 진료의
        /// </summary>
        public string medDrCd;          //
        /// <summary>
        /// 진료과 명
        /// </summary>
        public string medDeptKorName;   //
        /// <summary>
        /// 의사명
        /// </summary>
        public string medDrName;        //
        /// <summary>
        /// 병동
        /// </summary>
        public string ward;             //
        /// <summary>
        /// 병실
        /// </summary>
        public string room;             //
        /// <summary>
        /// 비고
        /// </summary>
        public string remark;           //

        /// <summary>
        /// 환자(보험)구분
        /// </summary>
        public string bi;               //
        /// <summary>
        /// 환자(보험)구분명
        /// </summary>
        public string biname;           //
        /// <summary>
        /// 증번호
        /// </summary>
        public string g_Kiho;           //

        /// <summary>
        /// 서식지번호
        /// </summary>
        public long formNo;             //
        /// <summary>
        /// 서식지 버전 번호
        /// </summary>
        public int updateNo;            //
        /// <summary>
        /// 출력여부
        /// </summary>
        public string printyn;          //
        /// <summary>
        /// 차트 작성날짜
        /// </summary>
        public string chartDate;        //
        /// <summary>
        /// 차트 작성시간
        /// </summary>
        public string chartTime;        //
        /// <summary>
        /// 작성자 사번
        /// </summary>
        public string writeSabun;        //
        /// <summary>
        /// 작성자 이름
        /// </summary>
        public string writeName;        //
        /// <summary>
        /// 작성자 날짜
        /// </summary>
        public string writeDate;        //
        /// <summary>
        /// 작성자 시간
        /// </summary>
        public string writeTime;        //
        /// <summary>
        /// 산모(엄마) 명
        /// </summary>
        public string MomName; //
        /// <summary>
        /// 아빠 이름
        /// </summary>
        public string DadName;  //
        /// <summary>
        /// XML형태의 이전 기록지(1:Old, 0:New)
        /// </summary>
        public string oldYn;            //

        /// <summary>
        /// 수술기록지관련해서 수술번호 전달
        /// </summary>
        public string opno;             //
        /// <summary>
        /// 수술일자
        /// </summary>
        public string opdate;           //
        /// <summary>
        /// 동국대경주병원사용(당일 두개 수술일 경우)
        /// </summary>
        public string opdegree;         //
        /// <summary>
        /// 수술과
        /// </summary>
        public string opdept;           //

        /// <summary>
        /// EMR차트 작성당시 환자 진료과
        /// </summary>
        public string nowdeptcd; //
        /// <summary>
        /// EMR차트 작성당시 환자 진료의사
        /// </summary>
        public string nowdrcd;   // 
        /// <summary>
        /// EMR차트 작성당시 환자 병실
        /// </summary>
        public string nowroomno;    // 

        /// <summary>
        /// 병동 입실일자
        /// </summary>
        public string wardDate; //
        /// <summary>
        /// /병동 입실시간
        /// </summary>
        public string wardTime; //

        /// <summary>
        /// EMR차트 작성당시 사용자 작성자 과
        /// </summary>
        public string cur_Dept;     // 
        /// <summary>
        /// EMR차트 작성당시 사용자 작성자 Grade
        /// </summary>
        public string cur_Grade;    // 

        //포항성모병원 외래/입원 접수 번호
        /// <summary>
        /// 외래접수번호
        /// </summary>
        public string acpNoOut;            //
        /// <summary>
        /// 입원접수번호(KOSMOS_PMPA.IPD_NEW_MASTER - IPDNO)
        /// </summary>
        public string acpNoIn;            //

        /// <summary>
        /// 확인자 이름
        /// </summary>
        public string compuseName;        //

        /// <summary>
        /// 확인자 사번
        /// </summary>
        public string compuseSabun;        //
    }
}
