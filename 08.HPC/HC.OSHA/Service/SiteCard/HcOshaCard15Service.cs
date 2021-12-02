namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard15Service
    {

        public  HcOshaCard15Repository hcOshaCard15Repository {get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard15Service()
        {
			this.hcOshaCard15Repository = new HcOshaCard15Repository();
        }


        public HC_OSHA_CARD15 Save(HC_OSHA_CARD15 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard15Repository.Update(dto);
            }
            else
            {
                return hcOshaCard15Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD15 dto)
        {
            hcOshaCard15Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD15(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD15> list = hcOshaCard15Repository.FindAll(siteId, (year - 1).ToString());
            foreach(HC_OSHA_CARD15 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();

                hcOshaCard15Repository.Insert(dto);
            }
        }
    }
}
