namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard20Service
    {
        
        public HcOshaCard20Repository hcOshaCard20Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard20Service()
        {
			this.hcOshaCard20Repository = new HcOshaCard20Repository();
        }

        public HC_OSHA_CARD20 Save(HC_OSHA_CARD20 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard20Repository.Update(dto);
            }
            else
            {
                return hcOshaCard20Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD20 dto)
        {
            hcOshaCard20Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD20(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD20> list = hcOshaCard20Repository.FindAllByYear((year - 1).ToString(), siteId);
        }
    }
}
