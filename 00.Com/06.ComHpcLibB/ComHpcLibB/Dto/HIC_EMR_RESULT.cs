
namespace ComHpcLibB.Dto
{

    using ComBase.Mvc;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HIC_EMR_RESULT : BaseDto
    {

        /// <summary>
        /// 작업일자
        /// </summary>
        public string JOBDATE { get; set; }

        /// <summary>
        /// 테이블명
        /// </summary>
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// 구분(자료사전:HIC_건진결과지종류 참조)
        /// </summary>
        public string GUBUN { get; set; }

        /// <summary>
        /// 관리번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 인쇄순번
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// 면허번호
        /// </summary>
        public long DRNO { get; set; }

        /// <summary>
        /// 인쇄한 서식지
        /// </summary>
        public string PRTRESULT { get; set; }

        /// <summary>
        /// 해쉬 결과
        /// </summary>
        public string HASHDATA { get; set; }

        /// <summary>
        /// 전자서명결과
        /// </summary>
        public string CERTDATA { get; set; }

        /// <summary>
        /// 작업자
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 작업일시
        /// </summary>
        public string ENTDATE { get; set; }

        /// <summary>
        /// 화일위치(서버)
        /// </summary>
        public string FILENAME { get; set; }

        /// <summary>
        /// 접수일자
        /// </summary>
        public string JEPDATE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// pdf파일 서버의 폴더 및 파일명
        /// </summary>
        public string PDFFILE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WEBSEND { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WEBSENDDATE { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }
    } 
}
