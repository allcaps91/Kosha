namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class XRAY_PACSSEND : BaseDto
    {

        /// <summary>
        /// 등록일자 및 시각
        /// </summary>
        public DateTime? ENTDATE { get; set; }

        /// <summary>
        /// Accession_Number
        /// </summary>
        public string PACSNO { get; set; }

        /// <summary>
        /// 전송구분(아래참조)
        /// </summary>
        public string SENDGBN { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PANO { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 영문이름
        /// </summary>
        public string ENAME { get; set; }

        /// <summary>
        /// 성별(M.남자 F.여자)
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public long AGE { get; set; }

        /// <summary>
        /// 진료과
        /// </summary>
        public string DEPTCODE { get; set; }

        /// <summary>
        /// 의사코드
        /// </summary>
        public string DRCODE { get; set; }

        /// <summary>
        /// 외래,입원(I/O)
        /// </summary>
        public string IPDOPD { get; set; }

        /// <summary>
        /// 병동코드
        /// </summary>
        public string WARDCODE { get; set; }

        /// <summary>
        /// 병실코드
        /// </summary>
        public string ROOMCODE { get; set; }

        /// <summary>
        /// 촬영종류
        /// </summary>
        public string XJONG { get; set; }

        /// <summary>
        /// 부위코드
        /// </summary>
        public string XSUBCODE { get; set; }

        /// <summary>
        /// 방사선코드
        /// </summary>
        public string XCODE { get; set; }

        /// <summary>
        /// 오더코드
        /// </summary>
        public string ORDERCODE { get; set; }

        /// <summary>
        /// 촬영예약일자 및 시각
        /// </summary>
        public DateTime? SEEKDATE { get; set; }

        /// <summary>
        /// 촬영방법(PA,AP등)
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 촬영실
        /// </summary>
        public string XRAYROOM { get; set; }

        /// <summary>
        /// 전송일자 및 시각
        /// </summary>
        public DateTime? SENDTIME { get; set; }

        /// <summary>
        /// 촬영명칭
        /// </summary>
        public string XRAYNAME { get; set; }

        /// <summary>
        /// 판독번호
        /// </summary>
        public long READNO { get; set; }

        /// <summary>
        /// 의사REMARK
        /// </summary>
        public string DRREMARK { get; set; }

        /// <summary>
        /// SENDTIME2
        /// </summary>
        public DateTime? SENDTIME2 { get; set; }

        /// <summary>
        /// 촬영자성명(EN)
        /// </summary>
        public string OPERATOR { get; set; }

        /// <summary>
        /// 오더 gbinfo - 2014-05-13
        /// </summary>
        public string GBINFO { get; set; }

        /// <summary>
        /// 입력자
        /// </summary>
        public string INPS { get; set; }

        /// <summary>
        /// 최초시간
        /// </summary>
        public DateTime? INPT_DT { get; set; }

        /// <summary>
        /// 변경자
        /// </summary>
        public string UPPS { get; set; }

        /// <summary>
        /// 변경시간
        /// </summary>
        public DateTime? UP_DT { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public XRAY_PACSSEND()
        {
        }
    }
}
