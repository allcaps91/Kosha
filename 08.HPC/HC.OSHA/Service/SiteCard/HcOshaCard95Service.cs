namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard95Service
    {
        
        private HcOshaCard95Repository hcOshaCard95Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard95Service()
        {
			this.hcOshaCard95Repository = new HcOshaCard95Repository();
        }

        public List<HC_OSHA_CARD9_5> FindAll(long siteId, string year)
        {
            year += "-12-31";

            List<HC_OSHA_CARD9_5> list = this.hcOshaCard95Repository.FindAll(siteId, year);
            foreach (HC_OSHA_CARD9_5 dto in list)
            {
                dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }
            return list;
        }
        public HC_OSHA_CARD9_5 Save(HC_OSHA_CARD9_5 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard95Repository.Update(dto);
            }
            else
            {
                return hcOshaCard95Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD9_5 dto)
        {
            hcOshaCard95Repository.Delete(dto.ID);
        }
    }
}
