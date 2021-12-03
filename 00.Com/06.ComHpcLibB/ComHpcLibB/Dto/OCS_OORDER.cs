
namespace ComHpcLibB.Dto
{

    using ComBase.Mvc;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class OCS_OORDER : BaseDto
    {

    /// <summary>
    /// 병록번호
    /// </summary>
    public string PTNO { get; set; }

    /// <summary>
    /// 발생일
    /// </summary>
    public string BDATE { get; set; }

    /// <summary>
    /// 과목
    /// </summary>
    public string DEPTCODE { get; set; }

    /// <summary>
    /// 일련번호
    /// </summary>
    public long SEQNO { get; set; }

    /// <summary>
    /// Order Code
    /// </summary>
    public string ORDERCODE { get; set; }

    /// <summary>
    /// 수가코드
    /// </summary>
    public string SUCODE { get; set; }

    /// <summary>
    /// 수가 분류
    /// </summary>
    public string BUN { get; set; }

    /// <summary>
    /// 처방 분류
    /// </summary>
    public string SLIPNO { get; set; }

    /// <summary>
    /// 실수량
    /// </summary>
    public string REALQTY { get; set; }

    /// <summary>
    /// 수량(원무금액계산)
    /// </summary>
    public long QTY { get; set; }

    /// <summary>
    /// 일수
    /// </summary>
    public long NAL { get; set; }

    /// <summary>
    /// 횟수
    /// </summary>
    public long GBDIV { get; set; }

    /// <summary>
    /// 용법코드
    /// </summary>
    public string DOSCODE { get; set; }

    /// <summary>
    /// 주사,기능검사등 일부 항목 접수 Flag 사용(J-주사실에서 삭제)
    /// </summary>
    public string GBBOTH { get; set; }

    /// <summary>
    /// 검사방법
    /// </summary>
    public string GBINFO { get; set; }

    /// <summary>
    /// 응급구분
    /// </summary>
    public string GBER { get; set; }

    /// <summary>
    /// 급여구분
    /// </summary>
    public string GBSELF { get; set; }

    /// <summary>
    /// 특진구분
    /// </summary>
    public string GBSPC { get; set; }

    /// <summary>
    /// 환자구분
    /// </summary>
    public string BI { get; set; }

    /// <summary>
    /// 의사코드
    /// </summary>
    public string DRCODE { get; set; }

    /// <summary>
    /// 비고
    /// </summary>
    public string REMARK { get; set; }

    /// <summary>
    /// 입력일
    /// </summary>
    public string ENTDATE { get; set; }

    /// <summary>
    /// 수납여부(0.수납않함 1.수납)
    /// </summary>
    public string GBSUNAP { get; set; }

    /// <summary>
    /// 투약번호
    /// </summary>
    public long TUYAKNO { get; set; }

    /// <summary>
    /// Order 고유번호
    /// </summary>
    public long ORDERNO { get; set; }

    /// <summary>
    /// 저함량 처방사유코드(A.B,C,E)
    /// </summary>
    public string MULTI { get; set; }

    /// <summary>
    /// 저함량 처방사유코드 참고사항
    /// </summary>
    public string MULTIREMARK { get; set; }

    /// <summary>
    /// DUR 구분
    /// </summary>
    public string DUR { get; set; }

    /// <summary>
    /// 통합예약구분( @:통합예약)
    /// </summary>
    public string RESV { get; set; }

    /// <summary>
    /// 중복처방 사유코드
    /// </summary>
    public string SCODESAYU { get; set; }

    /// <summary>
    /// 중복처방 사유-참고사항
    /// </summary>
    public string SCODEREMARK { get; set; }

    /// <summary>
    /// 중복 전송 방지 flag (Y: 전송완료)
    /// </summary>
    public string GBSEND { get; set; }

    /// <summary>
    /// 입원수속시 - 전송Flag (1=외래-> O,2=ER->E)
    /// </summary>
    public string AUTO_SEND { get; set; }

    /// <summary>
    /// 예약구분 (1:예약)
    /// </summary>
    public string RES { get; set; }

    /// <summary>
    /// 선택진료비 수가 발생 제외(1:제외)
    /// </summary>
    public string GBSPC_NO { get; set; }

    /// <summary>
    /// 예약검사 번호
    /// </summary>
    public long WRTNO { get; set; }

    /// <summary>
    /// 전자서명 관리번호
    /// </summary>
    public long CERTNO { get; set; }

    /// <summary>
    /// 자동전송(Y:자동전송) 내시경실 오더만 적용됨
    /// </summary>
    public string GBAUTOSEND { get; set; }

    /// <summary>
    /// 향정신의약품 장기처방 사유
    /// </summary>
    public string OCSDRUG { get; set; }

    /// <summary>
    /// 예약검사-금액 (사용안함)
    /// </summary>
    public long RESAMT { get; set; }

    /// <summary>
    /// 예약오더 복사일자
    /// </summary>
    public string GBCOPY { get; set; }

    /// <summary>
    /// 외래 후불대상자 자동전송 (1:외래후불대상->2:전송됨) 아래참조
    /// </summary>
    public string GBAUTOSEND2 { get; set; }

    /// <summary>
    /// 구분(01:내시경복사 02:내시경복사2)
    /// </summary>
    public string GUBUN { get; set; }

    /// <summary>
    /// FM가정의학 보험약 : 1
    /// </summary>
    public string GBFM { get; set; }

    /// <summary>
    /// 사번 2013-09-03
    /// </summary>
    public string SABUN { get; set; }

    /// <summary>
    /// 비과세 1
    /// </summary>
    public string GBTAX { get; set; }

    /// <summary>
    /// 구매전송(*)
    /// </summary>
    public string GBGUME_ACT { get; set; }

    /// <summary>
    /// IP주소 2015-01-26
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 미국마취 신체등급 BAS_BCODE GUBUN=`마취_신체등급(ASA)`
    /// </summary>
    public string ASA { get; set; }

    /// <summary>
    /// 임신차수
    /// </summary>
    public string PCHASU { get; set; }

    /// <summary>
    /// 실수령부서
    /// </summary>
    public string SUBUL_WARD { get; set; }

    /// <summary>
    /// 지원부서 시행처
    /// </summary>
    public string SENDDEPT { get; set; }

    /// <summary>
    /// 지원부서 중 보다 세부 부서존재시
    /// </summary>
    public string SENDDEPT_SUB { get; set; }

    /// <summary>
    /// 지원부서 상태
    /// </summary>
    public string SENDDEPT_STAT { get; set; }

    /// <summary>
    /// 입력자
    /// </summary>
    public string SENDDEPT_INPS { get; set; }

    /// <summary>
    /// 입력일시
    /// </summary>
    public string SENDDEPT_INPT_DT { get; set; }

    /// <summary>
    /// 수정자
    /// </summary>
    public string SENDDEPT_UPPS { get; set; }

    /// <summary>
    /// 수정일시
    /// </summary>
    public string SENDDEPT_UPDT { get; set; }

    /// <summary>
    /// CORDERCODE
    /// </summary>
    public string CORDERCODE { get; set; }

    /// <summary>
    /// CSUCODE
    /// </summary>
    public string CSUCODE { get; set; }

    /// <summary>
    /// CBUN
    /// </summary>
    public string CBUN { get; set; }

    /// <summary>
    /// 접수번호
    /// </summary>
    public long OPDNO { get; set; }

    /// <summary>
    /// 화상가산
    /// </summary>
    public string BURNADD { get; set; }

    /// <summary>
    /// 수술구분(0.주수술, 1.부수술, 2.제2수술)
    /// </summary>
    public string OPGUBUN { get; set; }

    /// <summary>
    /// 파우더조제(1:파우더)
    /// </summary>
    public string POWDER { get; set; }

    /// <summary>
    /// 진정(기본null,0, 대상:1)
    /// </summary>
    public string SEDATION { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string CERTDATE { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long CONTENTS { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long BCONTENTS { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string GBGROUP { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long TUYEOPOINT { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string TUYEOTIME { get; set; }

    }
}
