namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_MST_LTD : BaseDto
    {

        /// <summary>
        /// 미수 일련번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 회사코드
        /// </summary>
        public long LTDCODE { get; set; }

        /// <summary>
        /// 건강보험 지사코드(회사미수는 NULL)
        /// </summary>
        public string JISA { get; set; }

        /// <summary>
        /// 회사기호(회사미수는 NULL)
        /// </summary>
        public string KIHO { get; set; }

        /// <summary>
        /// 미수종류(1.회사미수 2.건강보험 3.국고미수 4.개인미수 5.보건소)
        /// </summary>
        public string MISUJONG { get; set; }

        /// <summary>
        /// 청구(발생)일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 검진종류(기초코드의 검진코드 참조)
        /// </summary>
        public string GJONG { get; set; }

        /// <summary>
        /// 입금완료여부(Y.완료 N.미완료)
        /// </summary>
        public string GBEND { get; set; }

        /// <summary>
        /// 미수구분(아래참조)
        /// </summary>
        public string MISUGBN { get; set; }

        /// <summary>
        /// 미수금액
        /// </summary>
        public long MISUAMT { get; set; }

        /// <summary>
        /// 입금금액
        /// </summary>
        public long IPGUMAMT { get; set; }

        /// <summary>
        /// 감액금액
        /// </summary>
        public long GAMAMT { get; set; }

        /// <summary>
        /// 삭감금액
        /// </summary>
        public long SAKAMT { get; set; }

        /// <summary>
        /// 반송액
        /// </summary>
        public long BANAMT { get; set; }

        /// <summary>
        /// 미수잔액
        /// </summary>
        public long JANAMT { get; set; }

        /// <summary>
        /// 수작업 장부의 미수번호
        /// </summary>
        public string GIRONO { get; set; }

        /// <summary>
        /// 담당자명
        /// </summary>
        public string DAMNAME { get; set; }

        /// <summary>
        /// 미수내역
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 최종 변경일시
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// 최종 작업자 사번
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 지로영수증 발급여부(Y/N)
        /// </summary>
        public string GBGIRO { get; set; }

        /// <summary>
        /// 계산서 발행여부(Y/N)
        /// </summary>
        public string GBTAX { get; set; }

        /// <summary>
        /// 품목
        /// </summary>
        public string PUMMOK { get; set; }

        /// <summary>
        /// 청구구분(아래참조)
        /// </summary>
        public string MIRGBN { get; set; }

        /// <summary>
        /// 청구차액 금액
        /// </summary>
        public long MIRCHAAMT { get; set; }

        /// <summary>
        /// 청구차액 발생사유
        /// </summary>
        public string MIRCHAREMARK { get; set; }

        /// <summary>
        /// 청구차액발생일자
        /// </summary>
        public string MIRCHADATE { get; set; }

        /// <summary>
        /// 회사 미수발생구분(Y:자동형성 ,NULL or N:수동입력)
        /// </summary>
        public string GBMISUBUILD { get; set; }

        /// <summary>
        /// 미수조치사항에 사용 (Y: 임시제외)
        /// </summary>
        public string CHK { get; set; }

        /// <summary>
        /// 공단 미수발생구분(Y:자동형성 ,NULL or N:수동입력)
        /// </summary>
        public string GBMISUBUILD2 { get; set; }

        /// <summary>
        /// 공단 구강미수발생구분(Y:자동형성 ,NULL or N:수동입력)
        /// </summary>
        public string GBMISUBUILD3 { get; set; }

        /// <summary>
        /// 청구년월
        /// </summary>
        public string YYMM_JIN { get; set; }

        /// <summary>
        /// 청구번호
        /// </summary>
        public long CNO { get; set; }

        /// <summary>
        /// 보건소 미수발생구분(Y:자동형성, Null or N: 수동)
        /// </summary>
        public string GBMISUBUILD4 { get; set; }

        /// <summary>
        /// 보건소지사코드(회사미수는 NULL)
        /// </summary>
        public string BOGUNSO { get; set; }

        /// <summary>
        /// 검진 사업년도
        /// </summary>
        public string GJYEAR { get; set; }

        /// <summary>
        /// 전자계산서 미발행 사유코드(BCode참조)
        /// </summary>
        public string TAXSAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCODE2 { get; set; }

        /// <summary>
        /// ////////////////////////////////////////
        /// </summary>
        /// 


        /// <summary>
        /// 업체코드
        /// </summary>
        public long CODE { get; set; }

        /// <summary>
        /// 상호
        /// </summary>
        public string SANGHO { get; set; }

        /// <summary>
        /// 약칭 상호
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string TEL { get; set; }

        /// <summary>
        /// FAX번호
        /// </summary>
        public string FAX { get; set; }

        /// <summary>
        /// E-MAIL번호
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string MAILCODE { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// 사업자등록번호
        /// </summary>
        public string SAUPNO { get; set; }

        /// <summary>
        /// 업태
        /// </summary>
        public string UPTAE { get; set; }

        /// <summary>
        /// 종목
        /// </summary>
        public string JONGMOK { get; set; }

        /// <summary>
        /// 대표자성명
        /// </summary>
        public string DAEPYO { get; set; }

        /// <summary>
        /// 대표자 주민번호
        /// </summary>
        public string JUMIN { get; set; }


        /// <summary>
        /// 업종코드
        /// </summary>
        public string UPJONG { get; set; }

        /// <summary>
        /// 산재성립번호
        /// </summary>
        public string SANKIHO { get; set; }

        /// <summary>
        /// 지방관서코드
        /// </summary>
        public string GWANSE { get; set; }

        /// <summary>
        /// 지도원코드
        /// </summary>
        public string JIDOWON { get; set; }

        /// <summary>
        /// 보건담당자
        /// </summary>
        public string BONAME { get; set; }

        /// <summary>
        /// 보건담당자 직위
        /// </summary>
        public string BOJIK { get; set; }

        /// <summary>
        /// 규모구분(1.50인미만 2.300미만 3.1000미만 4.1000이상)
        /// </summary>
        public string GYUMOGBN { get; set; }

        /// <summary>
        /// 개시번호
        /// </summary>
        public string GESINO { get; set; }

        /// <summary>
        /// 설립일자
        /// </summary>
        public DateTime? SELDATE { get; set; }

        /// <summary>
        /// 계약일자
        /// </summary>
        public string GYEDATE { get; set; }

        /// <summary>
        /// 제품코드1
        /// </summary>
        public string JEPUM1 { get; set; }

        /// <summary>
        /// 제품코드2
        /// </summary>
        public string JEPUM2 { get; set; }

        /// <summary>
        /// 제품코드3
        /// </summary>
        public string JEPUM3 { get; set; }

        /// <summary>
        /// 제품코드4
        /// </summary>
        public string JEPUM4 { get; set; }

        /// <summary>
        /// 제품코드5
        /// </summary>
        public string JEPUM5 { get; set; }

        /// <summary>
        /// 검진유무(Y/N)
        /// </summary>
        public string GBGEMJIN { get; set; }

        /// <summary>
        /// 측정유무(Y/N)
        /// </summary>
        public string GBCHUKJENG { get; set; }

        /// <summary>
        /// 대행유무(Y/N)
        /// </summary>
        public string GBDAEHANG { get; set; }

        /// <summary>
        /// 종검유무(Y/N)
        /// </summary>
        public string GBJONGGUM { get; set; }

        /// <summary>
        /// 국고유무(Y/N)
        /// </summary>
        public string GBGUKGO { get; set; }

        /// <summary>
        /// 삭제일자
        /// </summary>
        public DateTime? DELDATE { get; set; }

        /// <summary>
        /// 주요생산품
        /// </summary>
        public string JEPUMLIST { get; set; }

        /// <summary>
        /// 군병원
        /// </summary>
        public string ARMY_HSP { get; set; }

        /// <summary>
        /// 영업소기호
        /// </summary>
        public string YOUNGUPSO { get; set; }

        /// <summary>
        /// 영업소명
        /// </summary>
        public string UPSONAME { get; set; }

        /// <summary>
        /// 상세주소
        /// </summary>
        public string JUSODETAIL { get; set; }

        /// <summary>
        /// 종검계약일자
        /// </summary>
        public DateTime? NEGODATE { get; set; }

        /// <summary>
        /// 종검 남자 금액
        /// </summary>
        public long MAMT { get; set; }

        /// <summary>
        /// 종검 여자 금액
        /// </summary>
        public long FAMT { get; set; }

        /// <summary>
        /// 회사인원
        /// </summary>
        public long INWON { get; set; }

        /// <summary>
        /// 0.해당없음  1.초등학교   2.중/고등
        /// </summary>
        public string GBSCHOOL { get; set; }

        /// <summary>
        /// 특수청구유무(Y/N)
        /// </summary>
        public string SPCHUNGGU { get; set; }

        /// <summary>
        /// HEMS 전송용 사업장 기호
        /// </summary>
        public string HEMSNO { get; set; }

        /// <summary>
        /// 종검 참고사항
        /// </summary>
        public string HAREMARK { get; set; }

        /// <summary>
        /// 휴대전화 번호(출장문자메시지 전송용)
        /// </summary>
        public string HTEL { get; set; }

        /// <summary>
        /// 종검 가예약 가능 사업장 여부(Y.가능)
        /// </summary>
        public string GBGARESV { get; set; }

        /// <summary>
        /// 직원 종검가접수(공단1차) 여부(Y/N)
        /// </summary>
        public string HEAGAJEPSU1 { get; set; }

        /// <summary>
        /// 직원 종검가접수(암검진) 여부(Y/N)
        /// </summary>
        public string HEAGAJEPSU2 { get; set; }

        /// <summary>
        /// 가족 종검가접수(공단1차) 여부(Y/N)
        /// </summary>
        public string HEAGAJEPSU3 { get; set; }

        /// <summary>
        /// 가족 종검가접수(암검진) 여부(Y/N)
        /// </summary>
        public string HEAGAJEPSU4 { get; set; }

        /// <summary>
        /// 채용,배치전,특검 전달사항
        /// </summary>
        public string SPC_REMARK { get; set; }

        /// <summary>
        /// 전자세금계산서 관련 참고사항
        /// </summary>
        public string TAX_REMARK { get; set; }

        /// <summary>
        /// 종사업장번호(2015-07-01 신설)
        /// </summary>
        public string JSAUPNO { get; set; }

        /// <summary>
        /// 우편번호_건물번호
        /// </summary>
        public string BUILDNO { get; set; }

        /// <summary>
        /// 전자계산서용 우편번호
        /// </summary>
        public string TAX_MAILCODE { get; set; }

        /// <summary>
        /// 전자계산서용 주소
        /// </summary>
        public string TAX_JUSO { get; set; }

        /// <summary>
        /// 전자계산서용 상세주소
        /// </summary>
        public string TAX_JUSODETAIL { get; set; }

        /// <summary>
        /// 대행회사 코드(온라인메드 등)
        /// </summary>
        public long DLTD { get; set; }

        /// <summary>
        /// 변경전코드(2017-02-18)
        /// </summary>
        public string CODE2 { get; set; }

        /// <summary>
        /// <사용안함>
        /// </summary>
        public string DLTD2 { get; set; }

        /// <summary>
        /// 출장검진 불가능 사유
        /// </summary>
        public string CHULNOTSAYU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CHREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BOREMARK { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDCHARGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDGRADE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDHPHONE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LTDEMAIL { get; set; }

public HIC_MISU_MST_LTD()
        {
        }
    }
}