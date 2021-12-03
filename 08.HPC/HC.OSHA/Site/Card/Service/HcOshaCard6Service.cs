namespace HC.OSHA.Site.Card.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.Card.Repository;
    using HC.OSHA.Site.Card.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard6Service
    {
        
        private HcOshaCard6Repository hcOshaCard6Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard6Service()
        {
			this.hcOshaCard6Repository = new HcOshaCard6Repository();
        }

        public List<HC_OSHA_CARD6> FindAll(long estimateId)
        {
            return hcOshaCard6Repository.FindAll(estimateId);
        }
        public bool Save(IList<HC_OSHA_CARD6> list, long estimateId)
        {
            try
            {
                foreach (HC_OSHA_CARD6 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.ESTIMATE_ID = estimateId;
                        hcOshaCard6Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard6Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard6Repository.Delete(dto);
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
