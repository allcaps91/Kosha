namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard94Service
    {
        
        public HcOshaCard94Repository hcOshaCard94Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard94Service()
        {
			this.hcOshaCard94Repository = new HcOshaCard94Repository();
        }
        public List<HC_OSHA_CARD9_4> FindAll(long siteId, string year)
        {
            year += "-12-31";

            List < HC_OSHA_CARD9_4 >  list = this.hcOshaCard94Repository.FindAll(siteId, year);
            foreach(HC_OSHA_CARD9_4 dto in list)
            {
                dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }
            return list;
        }
        public HC_OSHA_CARD9_4 Save(HC_OSHA_CARD9_4 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard94Repository.Update(dto);
            }
            else
            {
                return hcOshaCard94Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD9_4 dto)
        {
            hcOshaCard94Repository.Delete(dto.ID);
        }
    }
}
