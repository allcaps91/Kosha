namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HIC_HYANG_APPROVE_JEPSU : BaseDto
    {
        
        /// <summary>
        /// 
        /// </summary>
		public string PANO { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long AGE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SEX { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SUCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public double ENTQTY { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public double ENTQTY2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PRINT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DRSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DRNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GBSITE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string DEPTCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string BDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? OCSSENDTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public DateTime? APPROVETIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string RID { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PTNO { get; set; }

        public HIC_HYANG_APPROVE_JEPSU()
        {
        }
    }
}
