namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard11_1Service
    {
        
        private HcOshaCard11_1Repository hcOshaCard111Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard11_1Service()
        {
			this.hcOshaCard111Repository = new HcOshaCard11_1Repository();
        }


        public List<HC_OSHA_CARD11_1> FindAll(long siteId, string year)
        {
        //    year += "-12-31";

            List<HC_OSHA_CARD11_1> list = this.hcOshaCard111Repository.FindAll(siteId, year);
            foreach (HC_OSHA_CARD11_1 dto in list)
            {
              //  dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }
            return list;
        }
        public HC_OSHA_CARD11_1 Save(HC_OSHA_CARD11_1 dto, string year)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard111Repository.Update(dto);
            }
            else
            {
                dto.YEAR = year;
                return hcOshaCard111Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD11_1 dto)
        {
            hcOshaCard111Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD11_1(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD11_1> list = hcOshaCard111Repository.FindAll(siteId, (year - 1).ToString());
            foreach(HC_OSHA_CARD11_1 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();

                hcOshaCard111Repository.Insert(dto);
            }
        }
    }
}
