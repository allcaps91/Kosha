namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard4_1Service
    {
        
        public HcOshaCard4_1Repository hcOshaCard41Repository { get;  }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard4_1Service()
        {
			this.hcOshaCard41Repository = new HcOshaCard4_1Repository();
        }
    

        public HC_OSHA_CARD4_1 Save(HC_OSHA_CARD4_1 dto, string year)
        {
            if (dto.ID > 0)
            {
               return hcOshaCard41Repository.Update(dto);
            }
            else
            {
                dto.YEAR = year;
                return hcOshaCard41Repository.Insert(dto);
            }
            
        }

        public void Delete(HC_OSHA_CARD4_1 dto)
        {
            hcOshaCard41Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD4_1(int year, long newEstimateId, long lastEstimateId)
        {
            HC_OSHA_CARD4_1 dto = hcOshaCard41Repository.FindByEstimate(lastEstimateId, (year - 1).ToString());

            if(dto != null)
            {
                dto.YEAR = year.ToString();
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard41Repository.Insert(dto);
            }
        }

        internal void COPY_HIC_OSHA_CARD5(int year, long newEstimateId, long lastEstimateId)
        {
            
        }
    }
}
