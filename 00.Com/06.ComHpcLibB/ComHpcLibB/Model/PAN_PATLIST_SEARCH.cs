namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    public class BTN_PANSET :BaseDto
    {
        public int INDEX { get; set; }
        public string JONG { get; set; }
        public string NAME { get; set; }
        public long WRTNO { get; set; }
        public BTN_PANSET()
        {

        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class PAN_PATLIST_SEARCH : BaseDto
    {
        /// <summary>
        /// ��ȸ ���� (���� ��) (HIC, SPC, REC, CAN, SCHOOL, ETC, TOTAL)
        /// </summary>
		public string JOBGUBUN { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public string FRDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string TODATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string GJJONG { get; set; }

        /// <summary>
        /// ����Viewer ���࿩��
        /// </summary>
		public bool MUNJINVIEW { get; set; }

        /// <summary>
        /// (����) : ��ü || 0 : ����� || 1 : ������ || 2 : ����
        /// </summary>
		public string JOB { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public long SABUN { get; set; }        

        /// <summary>
        /// 
        /// </summary>
		public long IDNUMBER { get; set; }

        /// <summary>
        /// �α��� �� �����ǻ� LICENCE
        /// </summary>
		public long LICENSE { get; set; }

        /// <summary>
        /// ��ȸ �� �����ǻ� LICENCE 
        /// </summary>
		public string LICENSE_VIEW { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string SORT { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string PANJENGDRNO { get; set; }

        

        /// <summary>
        /// 
        /// </summary>
		public long LTDCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string JONG { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string CHUL { get; set; }

        public string GBAUTOPAN { get; set; }

        public string B02_PANJENG_DRNO { get; set; }

        public long WRTNO { get; set; }

        public PAN_PATLIST_SEARCH()
        {
        }
    }
}
