namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard16Service
    {

        public HcOshaCard16Repository hcOshaCard16Repository {get;}
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard16Service()
        {
			this.hcOshaCard16Repository = new HcOshaCard16Repository();
        }
        public List<HC_OSHA_CARD16> FindAll(long siteId, string year)
        {
            List<HC_OSHA_CARD16> list = hcOshaCard16Repository.FindAll(siteId, year);
            foreach(HC_OSHA_CARD16 dto in list)
            {
                if(dto.ISMSDSEDUCATION == "1")
                {
                    dto.ISMSDSEDUCATION = "¡Û";
                }
                else if (dto.ISMSDSEDUCATION == "0")
                {
                    dto.ISMSDSEDUCATION = "";
                }

                if (dto.ISALET == "1")
                {
                    dto.ISALET = "¡Û";
                }
                else if (dto.ISALET == "0")
                {
                    dto.ISALET = "";
                }

                if (dto.ISMSDSPUBLISH == "1")
                {
                    dto.ISMSDSPUBLISH = "¡Û";
                }
                else if (dto.ISMSDSPUBLISH == "0")
                {
                    dto.ISMSDSPUBLISH = "";
                }
            }
            return list;
        }

        public HC_OSHA_CARD16 Save(HC_OSHA_CARD16 dto)
        {
           
            if (dto.ID > 0)
            {
                return hcOshaCard16Repository.Update(dto);
            }
            else
            {
                return hcOshaCard16Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD16 dto)
        {
            hcOshaCard16Repository.Delete(dto);
        }

        internal void COPY_HIC_OSHA_CARD16(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            List<HC_OSHA_CARD16> list = hcOshaCard16Repository.FindAll(siteId, (year - 1).ToString());
            foreach(HC_OSHA_CARD16 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                dto.YEAR = year.ToString();

                hcOshaCard16Repository.Insert(dto);
            }
        }
    }
}
