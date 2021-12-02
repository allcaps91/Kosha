namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard91Service
    {
        
        public HcOshaCard91Repository hcOshaCard91Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard91Service()
        {
			this.hcOshaCard91Repository = new HcOshaCard91Repository();
        }
        public List<HC_OSHA_CARD9_1> FindAll(long siteId, string year)
        {
            year += "-12-31";
            List<HC_OSHA_CARD9_1> list = this.hcOshaCard91Repository.FindAll(siteId, year);
            foreach(HC_OSHA_CARD9_1 dto in list)
            {
                dto.PERIOD = dto.STARTDATE + "~" + dto.ENDDATE;
            }

            return list;
        }
        public HC_OSHA_CARD9_1 Save(HC_OSHA_CARD9_1 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard91Repository.Update(dto);
            }
            else
            {
                return hcOshaCard91Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD9_1 dto)
        {
            hcOshaCard91Repository.Delete(dto.ID);
        }
    }
}
