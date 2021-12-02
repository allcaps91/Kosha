namespace HC.OSHA.Service
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using HC.OSHA.Dto;
    using HC.OSHA.Repository;
    using HC.OSHA.Repository.Schedule;
    using HC_OSHA.Model.Visit;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard5Service
    {
        
        public HcOshaCard5Repository hcOshaCard5Repository { get; }
        private OshaVisitPriceRepository oshaVisitPriceRepository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard5Service()
        {
			this.hcOshaCard5Repository = new HcOshaCard5Repository();
            
            this.oshaVisitPriceRepository = new OshaVisitPriceRepository();
        }
      
        public List<HC_OSHA_CARD5> FindAll(long estimateId)
        {
            return hcOshaCard5Repository.FindAll(estimateId);
        }
        public bool Save(IList<HC_OSHA_CARD5> list, long estimateId, long siteId)
        {
            try
            {
                foreach (HC_OSHA_CARD5 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.ESTIMATE_ID = estimateId;
                        dto.SITE_ID = siteId;
                        hcOshaCard5Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard5Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard5Repository.Delete(dto.ID);
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

        internal void InsertOrUpdate(HC_OSHA_CARD5 dto)
        {
            List<HC_OSHA_CARD5> list = hcOshaCard5Repository.Find(dto);
            if(list.Count > 0)
            {
                dto.ID = list[0].ID;
                hcOshaCard5Repository.Update(dto);
            }
            else
            {
                hcOshaCard5Repository.Insert(dto);
            }
        }

        internal void COPY_HIC_OSHA_CARD5(int year, long newEstimateId, long lastEstimateId)
        {
            //  금년도 조회
        }
    }
}
