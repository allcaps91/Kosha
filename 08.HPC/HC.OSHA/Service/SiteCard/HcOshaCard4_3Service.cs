namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard4_3Service
    {
        private HcOshaCard4_3Repository hcOshaCard4_3Repository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard4_3Service()
        {
            this.hcOshaCard4_3Repository = new HcOshaCard4_3Repository();
        }

        public List<HC_OSHA_CARD4_3> FindAll(long Estimate_Id)
        {
            return hcOshaCard4_3Repository.FindAll(Estimate_Id);
        }
        public bool Save(IList<HC_OSHA_CARD4_3> list, long estimateId, long siteId)
        {
            try
            {
                foreach (HC_OSHA_CARD4_3 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.ESTIMATE_ID = estimateId;
                        hcOshaCard4_3Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard4_3Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard4_3Repository.Delete(dto.ID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

    }
}
