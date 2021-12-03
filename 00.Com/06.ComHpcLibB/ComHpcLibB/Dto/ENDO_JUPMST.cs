namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ENDO_JUPMST : BaseDto
    {
        
        /// <summary>
        /// 등록번호
        /// </summary>
		public string PTNO { get; set; } 

        /// <summary>
        /// 접수일자
        /// </summary>
		public string JDATE { get; set; } 

        /// <summary>
        /// Order코드
        /// </summary>
		public string ORDERCODE { get; set; } 

        /// <summary>
        /// Order번호
        /// </summary>
		public long ORDERNO { get; set; } 

        /// <summary>
        /// 구분 (1:기관지, 2:위, 3:장, 4:ERCP)
        /// </summary>
		public string GBJOB { get; set; } 

        /// <summary>
        /// 예약일자
        /// </summary>
		public DateTime? RDATE { get; set; } 

        /// <summary>
        /// 과코드
        /// </summary>
		public string DEPTCODE { get; set; } 

        /// <summary>
        /// 의사코드
        /// </summary>
		public string DRCODE { get; set; } 

        /// <summary>
        /// 결과일자
        /// </summary>
		public string RESULTDATE { get; set; } 

        /// <summary>
        /// 결과의사
        /// </summary>
		public string RESULTDRCODE { get; set; } 

        /// <summary>
        /// 병동코드
        /// </summary>
		public string WARDCODE { get; set; } 

        /// <summary>
        /// 호실코드
        /// </summary>
		public string ROOMCODE { get; set; } 

        /// <summary>
        /// 입원외래구분(I.입원 O.외래)
        /// </summary>
		public string GBIO { get; set; } 

        /// <summary>
        /// 수납여부 (1:접수, 2:미접수,7:완료 *:취소)
        /// </summary>
		public string GBSUNAP { get; set; } 

        /// <summary>
        /// 금액
        /// </summary>
		public long AMT { get; set; } 

        /// <summary>
        /// 결과번호
        /// </summary>
		public long SEQNO { get; set; } 

        /// <summary>
        /// 접수자명
        /// </summary>
		public string JUPSUNAME { get; set; } 

        /// <summary>
        /// 비고
        /// </summary>
		public string REMARK { get; set; } 

        /// <summary>
        /// 입력일자 + 시간 / 접수일시
        /// </summary>
		public DateTime? ENTDATE { get; set; } 

        /// <summary>
        /// 상담일자(사용안함)
        /// </summary>
		public string VDATE { get; set; } 

        /// <summary>
        /// 성명
        /// </summary>
		public string SNAME { get; set; } 

        /// <summary>
        /// 성별(M/F)
        /// </summary>
		public string SEX { get; set; } 

        /// <summary>
        /// 생년월일
        /// </summary>
		public string BIRTHDATE { get; set; } 

        /// <summary>
        /// 차트유무
        /// </summary>
		public string GBCHART { get; set; } 

        /// <summary>
        /// PACS 고유번호(YYYYMMDD-nnnn)
        /// </summary>
		public string PACSNO { get; set; } 

        /// <summary>
        /// PACS Study UID
        /// </summary>
		public string PACSUID { get; set; } 

        /// <summary>
        /// PACS 전송구분(*.미전송 Y.전송)
        /// </summary>
		public string PACSSEND { get; set; } 

        /// <summary>
        /// 촬영결과 전송구분(*.미전송 Y.전송)
        /// </summary>
		public string RESULTSEND { get; set; } 

        /// <summary>
        /// 구분의 일려번호
        /// </summary>
		public string SEQNUM { get; set; } 

        /// <summary>
        /// 오더시간(2005/08/25부터)
        /// </summary>
		public DateTime? ORDERDATE { get; set; } 

        /// <summary>
        /// 전송시간(2005/08/25부터)
        /// </summary>
		public DateTime? SENDDATE { get; set; } 

        /// <summary>
        /// 영상전송시간(2005/08/25부터)
        /// </summary>
		public DateTime? XSENDDATE { get; set; } 

        /// <summary>
        /// 수가코드(조직검사여부사용)
        /// </summary>
		public string SUCODE { get; set; } 

        /// <summary>
        /// 예약자통보일자
        /// </summary>
		public DateTime? TDATE { get; set; } 

        /// <summary>
        /// 예약통보자사번
        /// </summary>
		public long SABUN { get; set; } 

        /// <summary>
        /// 건진내시경->종검chk
        /// </summary>
		public string ENDOCHK { get; set; } 

        /// <summary>
        /// 예약검사 WRTNO (ADMIN.ETC_EXAM_RESERVED_MST)
        /// </summary>
		public long EXAM_WRTNO { get; set; } 

        /// <summary>
        /// 시행부서코드
        /// </summary>
		public string BUSE { get; set; } 

        /// <summary>
        /// 예약의사명-스케쥴사용
        /// </summary>
		public string RDRNAME { get; set; } 

        /// <summary>
        /// 내시경 영상 다운로드 여부(NULL.미작업)
        /// </summary>
		public long STUDY_REF { get; set; } 

        /// <summary>
        /// 전자서명(2010-01-01 부터)
        /// </summary>
		public long CERTNO { get; set; } 

        /// <summary>
        /// 카덱스 간호사 수동작업 (삭제 : D, 수동확인 : 1 =>하단 확인일자에 추가)
        /// </summary>
		public string CADEX_DEL { get; set; } 

        /// <summary>
        /// 1:예약
        /// </summary>
		public string RES { get; set; } 

        /// <summary>
        /// 처방일자
        /// </summary>
		public string BDATE { get; set; } 

        /// <summary>
        /// 1:부도(환불)
        /// </summary>
		public string GBREFUND { get; set; } 

        /// <summary>
        /// 환불처리요청일자
        /// </summary>
		public DateTime? GBREFUND_DATE { get; set; } 

        /// <summary>
        /// 환불처리요청 사번
        /// </summary>
		public long GBREFUND_SABUN { get; set; } 

        /// <summary>
        /// 확인일자시간
        /// </summary>
		public DateTime? CDATE { get; set; } 

        /// <summary>
        /// 확인자
        /// </summary>
		public long CSABUN { get; set; } 

        /// <summary>
        /// Premedication None
        /// </summary>
		public string GBPRE_1 { get; set; } 

        /// <summary>
        /// Premedication Aigiron, 2019-05-17부로 사용 x
        /// </summary>
		public string GBPRE_2 { get; set; } 

        /// <summary>
        /// Premedication Aigiron mg
        /// </summary>
		public string GBPRE_21 { get; set; } 

        /// <summary>
        /// Premedication Aigiron IV
        /// </summary>
		public string GBPRE_22 { get; set; } 

        /// <summary>
        /// Premedication Others
        /// </summary>
		public string GBPRE_3 { get; set; } 

        /// <summary>
        /// Conscious None
        /// </summary>
		public string GBCON_1 { get; set; } 

        /// <summary>
        /// Conscious Midazolam
        /// </summary>
		public string GBCON_2 { get; set; } 

        /// <summary>
        /// Conscious Midazolam mg
        /// </summary>
		public string GBCON_21 { get; set; } 

        /// <summary>
        /// Conscious Midazolam IV
        /// </summary>
		public string GBCON_22 { get; set; } 

        /// <summary>
        /// Conscious Propofol
        /// </summary>
		public string GBCON_3 { get; set; } 

        /// <summary>
        /// Conscious Propofol mg
        /// </summary>
		public string GBCON_31 { get; set; } 

        /// <summary>
        /// Conscious Propofol IV
        /// </summary>
		public string GBCON_32 { get; set; } 

        /// <summary>
        /// Conscious Pethidine -> 기관지경에도 사용 pre에서
        /// </summary>
		public string GBCON_4 { get; set; } 

        /// <summary>
        /// Conscious Pethidine mg
        /// </summary>
		public string GBCON_41 { get; set; } 

        /// <summary>
        /// Conscious Pethidine IV
        /// </summary>
		public string GBCON_42 { get; set; } 

        /// <summary>
        /// Procedure Biopsy
        /// </summary>
		public string GBPRO_1 { get; set; } 

        /// <summary>
        /// Procedure CLO 2019-05-17부로 사용 x
        /// </summary>
		public string GBPRO_2 { get; set; } 

        /// <summary>
        /// Premedication Others remark
        /// </summary>
		public string GBPRE_31 { get; set; } 

        /// <summary>
        /// 대장-장정결도
        /// </summary>
		public string GB_CLEAN { get; set; } 

        /// <summary>
        /// 구분(신규서식) Y
        /// </summary>
		public string GBNEW { get; set; } 

        /// <summary>
        /// 도착처리대상
        /// </summary>
		public string SGUBUN { get; set; } 

        /// <summary>
        /// Premedication Atropine
        /// </summary>
		public string GBPRE_4 { get; set; } 

        /// <summary>
        /// Premedication Atropine mg
        /// </summary>
		public string GBPRE_41 { get; set; } 

        /// <summary>
        /// Premedication Atropine IV
        /// </summary>
		public string GBPRE_42 { get; set; } 

        /// <summary>
        /// Conscious others remark
        /// </summary>
		public string GBCON_5 { get; set; } 

        /// <summary>
        /// Conscious others remark
        /// </summary>
		public string GBCON_51 { get; set; } 

        /// <summary>
        /// 기관지경 Medication None
        /// </summary>
		public string GBMED_1 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Epinephrine
        /// </summary>
		public string GBMED_2 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Epinephrine mg
        /// </summary>
		public string GBMED_21 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Epinephrine IV
        /// </summary>
		public string GBMED_22 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Botrooase
        /// </summary>
		public string GBMED_3 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Botrooase KU
        /// </summary>
		public string GBMED_31 { get; set; } 

        /// <summary>
        /// 기관지경 Medication Botrooase IV
        /// </summary>
		public string GBMED_32 { get; set; } 

        /// <summary>
        /// 기관지경 Medication others
        /// </summary>
		public string GBMED_4 { get; set; } 

        /// <summary>
        /// 기관지경 Medication others remark
        /// </summary>
		public string GBMED_41 { get; set; } 

        /// <summary>
        /// 장부구분 (아래참조)
        /// </summary>
		public string GUBUN { get; set; } 

        /// <summary>
        /// 장부시간(13:00~13:50)
        /// </summary>
		public string GUBUN_TIME { get; set; } 

        /// <summary>
        /// 장부간호사
        /// </summary>
		public string GUBUN_NURSE { get; set; } 

        /// <summary>
        /// 궤양여부
        /// </summary>
		public string GUBUN_GUE { get; set; } 

        /// <summary>
        /// MOAAS scale
        /// </summary>
		public string MOAAS { get; set; } 

        /// <summary>
        /// 내시경 삽입분
        /// </summary>
		public string D_INTIME1 { get; set; } 

        /// <summary>
        /// 내시경 삽입초
        /// </summary>
		public string D_INTIME2 { get; set; } 

        /// <summary>
        /// 검사 소요분
        /// </summary>
		public string D_EXTIME1 { get; set; } 

        /// <summary>
        /// 검사 소요초
        /// </summary>
		public string D_EXTIME2 { get; set; } 

        /// <summary>
        /// procedure Bx
        /// </summary>
		public string PRO_BX1 { get; set; } 

        /// <summary>
        /// procedure Bx()
        /// </summary>
		public string PRO_BX2 { get; set; } 

        /// <summary>
        /// procedure PP
        /// </summary>
		public string PRO_PP1 { get; set; } 

        /// <summary>
        /// procedure PP()
        /// </summary>
		public string PRO_PP2 { get; set; } 

        /// <summary>
        /// procedure ESD
        /// </summary>
		public string PRO_ESD1 { get; set; } 

        /// <summary>
        /// procedure ESD en-bloc
        /// </summary>
		public string PRO_ESD2 { get; set; } 

        /// <summary>
        /// procedure ESD piecemeal
        /// </summary>
		public string PRO_ESD3_1 { get; set; } 

        /// <summary>
        /// procedure ESD piecemeal()
        /// </summary>
		public string PRO_ESD3_2 { get; set; } 

        /// <summary>
        /// procedure EMR
        /// </summary>
		public string PRO_EMR1 { get; set; } 

        /// <summary>
        /// procedure EMR en-bloc
        /// </summary>
		public string PRO_EMR2 { get; set; } 

        /// <summary>
        /// procedure EMR piecemeal
        /// </summary>
		public string PRO_EMR3_1 { get; set; } 

        /// <summary>
        /// procedure EMR piecemeal()
        /// </summary>
		public string PRO_EMR3_2 { get; set; } 

        /// <summary>
        /// procedure APC
        /// </summary>
		public string PRO_APC { get; set; } 

        /// <summary>
        /// procedure Electrocauterization
        /// </summary>
		public string PRO_ELEC { get; set; } 

        /// <summary>
        /// procedure Hemoclip
        /// </summary>
		public string PRO_HEMO1 { get; set; } 

        /// <summary>
        /// procedure Hemoclip()
        /// </summary>
		public string PRO_HEMO2 { get; set; } 

        /// <summary>
        /// procedure EPNA
        /// </summary>
		public string PRO_EPNA1 { get; set; } 

        /// <summary>
        /// procedure EPNA()
        /// </summary>
		public string PRO_EPNA2 { get; set; } 

        /// <summary>
        /// procedure band
        /// </summary>
		public string PRO_BAND1 { get; set; } 

        /// <summary>
        /// procedure band()
        /// </summary>
		public string PRO_BAND2 { get; set; } 

        /// <summary>
        /// procedure multi-band
        /// </summary>
		public string PRO_MBAND { get; set; } 

        /// <summary>
        /// procedure Histoacyl
        /// </summary>
		public string PRO_HIST1 { get; set; } 

        /// <summary>
        /// procedure Histoacyl()
        /// </summary>
		public string PRO_HIST2 { get; set; } 

        /// <summary>
        /// procedure Detachable snare
        /// </summary>
		public string PRO_DETA { get; set; } 

        /// <summary>
        /// procedure EST
        /// </summary>
		public string PRO_EST { get; set; } 

        /// <summary>
        /// procedure Ballooon
        /// </summary>
		public string PRO_BALL { get; set; } 

        /// <summary>
        /// procedure Basket
        /// </summary>
		public string PRO_BASKET { get; set; } 

        /// <summary>
        /// procedure EPBD
        /// </summary>
		public string PRO_EPBD1 { get; set; } 

        /// <summary>
        /// procedure EPBD()mm
        /// </summary>
		public string PRO_EPBD2 { get; set; } 

        /// <summary>
        /// procedure EPBD()atm
        /// </summary>
		public string PRO_EPBD3 { get; set; } 

        /// <summary>
        /// procedure EPBD()sec
        /// </summary>
		public string PRO_EPBD4 { get; set; } 

        /// <summary>
        /// procedure ENBD
        /// </summary>
		public string PRO_ENBD1 { get; set; } 

        /// <summary>
        /// procedure ENBD()fr
        /// </summary>
		public string PRO_ENBD2 { get; set; } 

        /// <summary>
        /// procedure ENBD()type
        /// </summary>
		public string PRO_ENBD3 { get; set; } 

        /// <summary>
        /// procedure ERBD
        /// </summary>
		public string PRO_ERBD1 { get; set; } 

        /// <summary>
        /// procedure ERBD()fr
        /// </summary>
		public string PRO_ERBD2 { get; set; } 

        /// <summary>
        /// procedure ERBD()cm
        /// </summary>
		public string PRO_ERBD3 { get; set; } 

        /// <summary>
        /// procedure ERBD()type
        /// </summary>
		public string PRO_ERBD4 { get; set; } 

        /// <summary>
        /// procedure EST (small/...)
        /// </summary>
		public string PRO_EST_STS { get; set; } 

        /// <summary>
        /// 
        /// </summary>
		public string ASA { get; set; } 

        /// <summary>
        /// 예약부도 구분
        /// </summary>
		public string BANKRUPTCY { get; set; } 

        /// <summary>
        /// 예약부도 개인사정 메모
        /// </summary>
		public string BANKRUPTCY_MEMO { get; set; } 

        /// <summary>
        /// 예약대기 여부
        /// </summary>
		public string YWAIT { get; set; } 

        /// <summary>
        /// 예약대기 설정 일자
        /// </summary>
		public DateTime? YWAIT_DATE { get; set; } 

        /// <summary>
        /// 예약대기 설정 사번
        /// </summary>
		public long YWAIT_SABUN { get; set; } 

        /// <summary>
        /// 예약대기 일자
        /// </summary>
		public DateTime? YWAIT_TODATE { get; set; } 

        /// <summary>
        /// 예약대기 메모
        /// </summary>
		public string YWAIT_MEMO { get; set; } 

        /// <summary>
        /// 도착구분2
        /// </summary>
		public string SGUBUN2 { get; set; } 

        /// <summary>
        /// procedure Rapid Urease Test
        /// </summary>
		public string PRO_RUT { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }


        /// <summary>
        /// 내시경 접수자 관리
        /// </summary>
        public ENDO_JUPMST()
        {
        }
    }
}
