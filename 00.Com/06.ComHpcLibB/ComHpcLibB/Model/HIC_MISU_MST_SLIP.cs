namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HIC_MISU_MST_SLIP : BaseDto
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
        /// 회사코드
        /// </summary>
        public string LTDNAME { get; set; }

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
        /// 미수계정코드
        /// </summary>
        public string GEACODE { get; set; }


        /// <summary>
        /// 미수계정코드
        /// </summary>
        public string JISACODE { get; set; }
        /// <summary>
        /// 미수계정코드
        /// </summary>
        public string KIHOCODE { get; set; }



        /// <summary>
        /// 검진종류(기초코드의 검진코드 참조)
        /// </summary>
        public string GJONG { get; set; }

        /// <summary>
        /// 검진종류(기초코드의 검진코드 참조)
        /// </summary>
        public string GJNAME { get; set; }

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
        /// DAMT
        /// </summary>
        public long DAMT { get; set; }


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
        /// 미수 Slip 금액
        /// </summary>
        public long SLIPAMT { get; set; }
        public string ROWID { get; set; }
        public HIC_MISU_MST_SLIP()
        {
        }
    }
}