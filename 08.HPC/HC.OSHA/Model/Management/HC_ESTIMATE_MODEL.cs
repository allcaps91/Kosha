namespace HC.OSHA.Model
{
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using HC.Core.Model;
    using System;
    
    
    /// <summary>
    /// ���� + ���
    /// </summary>
    public class HC_ESTIMATE_MODEL : BaseDto, IEstimateModel
    {
        
        /// <summary>
        /// 
        /// </summary>
		public long ID { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
		public string ESTIMATEDATE { get; set; }

        /// <summary>
        /// ��࿩��
        /// </summary>
        public string ISCONTRACT { get; set; }

        /// <summary>
        /// �����
        /// </summary>
		public string CONTRACTDATE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string CONTRACTSTARTDATE { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        public string CONTRACTENDDATE { get; set; }

        /// <summary>
        /// ���Ⱓ
        /// </summary>
        public string ContractPeriod { get; set; }
      

       
    }
}
