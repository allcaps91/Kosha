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
    public class HcOshaCard10Service
    {
        
        private HcOshaCard10Repository hcOshaCard10Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard10Service()
        {
			this.hcOshaCard10Repository = new HcOshaCard10Repository();
        }

        public List<HC_OSHA_CARD10> FindAll(long siteID, string year)
        {
            return hcOshaCard10Repository.FindAll(siteID, year);
        }
        public bool Save(IList<HC_OSHA_CARD10> list, long estimateId, long siteId, string year)
        {
            try
            {
                foreach (HC_OSHA_CARD10 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.YEAR = year;
                        dto.ESTIMATE_ID = estimateId;
                        dto.SITE_ID = siteId;
                        hcOshaCard10Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard10Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard10Repository.Delete(dto.ID);
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

        internal void COPY_HIC_OSHA_CARD10(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD10> list = hcOshaCard10Repository.FindAll(siteId, (year - 1).ToString());
            foreach (HC_OSHA_CARD10 dto in list)
            {
                dto.YEAR = year.ToString();
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard10Repository.Insert(dto);
            }
        }
    }
}
