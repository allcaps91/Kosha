namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard4_2Service
    {
        
        public HcOshaCard4_2Repository hcOshaCard4_2Repository { get;  }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard4_2Service()
        {
			this.hcOshaCard4_2Repository = new HcOshaCard4_2Repository();
        }

        public HC_OSHA_CARD4_2 Save(HC_OSHA_CARD4_2 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard4_2Repository.Update(dto);
            }
            else
            {
                return  hcOshaCard4_2Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD4_2 dto)
        {
            hcOshaCard4_2Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD4_2(int year, long newEstimateId, long lastEstimateId)
        {
            List<HC_OSHA_CARD4_2> list = hcOshaCard4_2Repository.FindByEstimateId(lastEstimateId, (year - 1).ToString());
            foreach(HC_OSHA_CARD4_2 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();

                hcOshaCard4_2Repository.Insert(dto);
            }
        }
    }
}
