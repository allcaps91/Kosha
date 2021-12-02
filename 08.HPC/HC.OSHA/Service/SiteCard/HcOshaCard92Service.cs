namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard92Service
    {
        
        public HcOshaCard92Repository hcOshaCard92Repository { get; }
        public List<HC_OSHA_CARD9_2> FindAll(long siteId)
        {
         //   year += "-12-31";
            List<HC_OSHA_CARD9_2> list = this.hcOshaCard92Repository.FindAll(siteId);
            foreach (HC_OSHA_CARD9_2 dto in list)
            {
                dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }

            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard92Service()
        {
			this.hcOshaCard92Repository = new HcOshaCard92Repository();
        }
        public HC_OSHA_CARD9_2 Save(HC_OSHA_CARD9_2 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard92Repository.Update(dto);
            }
            else
            {
                return hcOshaCard92Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD9_2 dto)
        {
            hcOshaCard92Repository.Delete(dto.ID);
        }
    }
}
