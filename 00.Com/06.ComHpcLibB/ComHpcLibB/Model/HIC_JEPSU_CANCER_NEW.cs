namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_JEPSU_CANCER_NEW : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long WRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string S_SOGEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string S_SOGEN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string C_SOGEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string C_SOGEN2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string C_SOGEN3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string L_SOGEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string B_SOGEN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string W_SOGEN { get; set; }

        public string LUNG_RESULT070 { get; set; }

        public string LUNG_RESULT071 { get; set; }

        public long LTDCODE { get; set; }

        public string GKIHO { get; set; }

        public string PTNO { get; set; }

        public string GUNDATE { get; set; }

        public string JEPDATE { get; set; }

        public string TONGBODATE { get; set; }

        public string JUMIN2 { get; set; }

        public string MURYOAM { get; set; }

        public string BOGUNSO { get; set; }

        public string JUSOAA { get; set; }

        public string SEX { get; set; }

        public long AGE { get; set; }

        public long PANO { get; set; }

        public long PANJENGDRNO { get; set; }

        public string PANJENGDATE { get; set; }

        public string GJCHASU { get; set; }

        public string GBCHUL { get; set; }

        public string GBAM { get; set; }

        public string GBSTOMACH { get; set; }

        public string GBRECTUM { get; set; }

        public string GBLIVER { get; set; }

        public string GBBREAST { get; set; }

        public string GBWOMB { get; set; }

        public string S_ANATGBN { get; set; }

        public string S_ANAT { get; set; }

        public string C_ANATGBN { get; set; }

        public string C_ANAT { get; set; }

        public string JINCHALGBN { get; set; }

        public HIC_JEPSU_CANCER_NEW()
        {
        }
    }
}
