namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMstLtdService
    {
        
        private HicMisuMstLtdRepository hicMisuMstLtdRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMstLtdService()
        {
			this.hicMisuMstLtdRepository = new HicMisuMstLtdRepository();
        }
        

        public List<HIC_MISU_MST_LTD> GetBillList(string strFDate, string strTDate, string strMJong)
        {
            return hicMisuMstLtdRepository.GetBillList(strFDate, strTDate, strMJong);
        }

        public List<HIC_MISU_MST_LTD> GetActingItem(long DLTD)
        {
            return hicMisuMstLtdRepository.GetActingItem(DLTD);
        }

        public int UpdateBill(long nWrtno)
        {
            return hicMisuMstLtdRepository.UpdateBill(nWrtno);
        }
    }
}
