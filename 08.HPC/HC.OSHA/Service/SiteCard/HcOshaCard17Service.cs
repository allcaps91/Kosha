namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard17Service
    {
        
        public HcOshaCard17Repository hcOshaCard17Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard17Service()
        {
			this.hcOshaCard17Repository = new HcOshaCard17Repository();
        }

        public HC_OSHA_CARD17 Save(HC_OSHA_CARD17 dto , string year)
        {
            dto.YEAR = year;
            if (dto.ID > 0)
            {
                return hcOshaCard17Repository.Update(dto);
            }
            else
            {
                return hcOshaCard17Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD17 dto)
        {
            hcOshaCard17Repository.Delete(dto.ID);
        }
    }
}
