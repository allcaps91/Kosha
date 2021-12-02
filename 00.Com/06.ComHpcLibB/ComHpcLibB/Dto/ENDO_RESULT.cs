namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ENDO_RESULT : BaseDto
    {

        /// <summary>
        /// 접수(결과) 번호
        /// </summary>
        public long SEQNO { get; set; }

        /// <summary>
        /// VocalCord(Esophagus, Endo Find)
        /// </summary>
        public string REMARK1 { get; set; }

        /// <summary>
        /// Carina(Stomach, Endo Diagnosis)
        /// </summary>
        public string REMARK2 { get; set; }

        /// <summary>
        /// Bronchi(Duodenum, Biospy)
        /// </summary>
        public string REMARK3 { get; set; }

        /// <summary>
        /// Endoscopic Diagnosis(식도)
        /// </summary>
        public string REMARK4 { get; set; }

        /// <summary>
        /// Endoscopic Procedure
        /// </summary>
        public string REMARK5 { get; set; }

        /// <summary>
        /// 내시경 그림 좌표
        /// </summary>
        public string PICXY { get; set; }

        /// <summary>
        /// Endoscopic Biopsy
        /// </summary>
        public string REMARK6 { get; set; }

        /// <summary>
        /// Endoscopic Biopsy2 2013-01-09
        /// </summary>
        public string REMARK6_2 { get; set; }

        /// <summary>
        /// Endoscopic Biopsy3 2013-01-09
        /// </summary>
        public string REMARK6_3 { get; set; }

        /// <summary>
        /// 참고사항 2013-01-09
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// 내시경 결과 관리
        /// </summary>
        public ENDO_RESULT()
        {
        }
    }
}
