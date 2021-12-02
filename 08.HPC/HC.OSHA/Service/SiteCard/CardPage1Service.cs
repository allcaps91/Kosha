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
    public class CardPage1Service
    {
        
        private HcOshaCard3Repository hcOshaCard3Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public CardPage1Service()
        {
			this.hcOshaCard3Repository = new HcOshaCard3Repository();
        }
        public List<HC_OSHA_CARD3> FindAll(long estimateId, string year)
        {
            return hcOshaCard3Repository.FindAll(estimateId, year);
        }
        public bool Save(IList<HC_OSHA_CARD3> list, long siteId, long estimateId, string year)
        {
            try
            {
                foreach (HC_OSHA_CARD3 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.YEAR = year;
                        dto.ESTIMATE_ID = estimateId;
                        dto.SITE_ID = siteId;
                        hcOshaCard3Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard3Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard3Repository.Delete(dto.ID);
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

        internal void COPY_HIC_OSHA_CARD3(int year, long newEstimateId, long lastEstimateId)
        {
            List<HC_OSHA_CARD3> list = FindAll(lastEstimateId, (year - 1).ToString());
            
            foreach(HC_OSHA_CARD3 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard3Repository.Insert(dto);
            }
        }
    }
}
