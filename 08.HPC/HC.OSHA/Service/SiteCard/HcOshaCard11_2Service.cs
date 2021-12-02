namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard11_2Service
    {
        
        private HcOshaCard11_2Repository hcOshaCard112Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard11_2Service()
        {
			this.hcOshaCard112Repository = new HcOshaCard11_2Repository();
        }

        public List<HC_OSHA_CARD11_2> FindAll(long siteId, string year)
        {
            year += "-12-31";

            List<HC_OSHA_CARD11_2> list = this.hcOshaCard112Repository.FindAll(siteId, year);
            foreach (HC_OSHA_CARD11_2 dto in list)
            {
                dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }
            return list;
        }
        public HC_OSHA_CARD11_2 Save(HC_OSHA_CARD11_2 dto, string year)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard112Repository.Update(dto);
            }
            else
            {
                dto.YEAR = year;
                return hcOshaCard112Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD11_2 dto)
        {
            hcOshaCard112Repository.Delete(dto.ID);
        }
    }
}
