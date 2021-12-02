namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard13Service
    {
        
        public HcOshaCard13Repository hcOshaCard13Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard13Service()
        {
			this.hcOshaCard13Repository = new HcOshaCard13Repository();
        }

        public HC_OSHA_CARD13 Save(HC_OSHA_CARD13 dto, string year)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard13Repository.Update(dto);
            }
            else
            {
                dto.YEAR = year;
                return hcOshaCard13Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD13 dto)
        {
            hcOshaCard13Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD13(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD13> list = hcOshaCard13Repository.FindAll(siteId, (year - 1).ToString());
            foreach(HC_OSHA_CARD13 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();

                hcOshaCard13Repository.Insert(dto);
            }
        }
    }
}
