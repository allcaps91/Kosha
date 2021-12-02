namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HIC_HEA_JEPSU_CONSENT : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>

        public string PTNO { get; set; }

        public string SNAME { get; set; }

        public long WRTNO { get; set; }

        public string SEX { get; set; }

        public long AGE { get; set; }        

        public string ASA { get; set; }

		public string SDATE { get; set; }

        public long D10 { get; set; }

        public long D20 { get; set; }

        public long D30 { get; set; }

        public long D40 { get; set; }

        public string DEPTCODE { get; set; }

        public string FORMCODE { get; set; }

        public int PAGECNT { get; set; }
        

        public HIC_HEA_JEPSU_CONSENT()
        {
        }
    }
}

