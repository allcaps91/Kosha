namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard21Service
    {
        
        public HcOshaCard21Repository hcOshaCard21Repository { get; }
        public HcOshaCard22Repository hcOshaCard22Repository { get; }

        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard21Service()
        {
			this.hcOshaCard21Repository = new HcOshaCard21Repository();
            this.hcOshaCard22Repository = new HcOshaCard22Repository();
        }
        public HC_OSHA_CARD22 FindImage(long estimateId) 
        {
           return hcOshaCard22Repository.FindByEstimateId(estimateId);
        }

        public void SaveImage(HC_OSHA_CARD22 dto)
        {
            if(hcOshaCard22Repository.FindByEstimateId(dto.ESTIMATE_ID) == null)
            {
                hcOshaCard22Repository.Insert(dto);
            }
            else
            {
                hcOshaCard22Repository.Update(dto);
            }
     
        }

        public void DeleteImage(HC_OSHA_CARD22 dto)
        {
            HC_OSHA_CARD22 saved = hcOshaCard22Repository.FindByEstimateId(dto.ESTIMATE_ID);

            if (saved != null)
            {
                hcOshaCard22Repository.Delete(saved.ID);
            }
        }

        public HC_OSHA_CARD21 Save(HC_OSHA_CARD21 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard21Repository.Update(dto);
            }
            else
            {
                return hcOshaCard21Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD21 dto)
        {
            hcOshaCard21Repository.Delete(dto.ID);
        }

        internal void COPY_HIC_OSHA_CARD21(int year, long newEstimateId, long lastEstimateId, long siteId)
        {
            HC_OSHA_CARD21 dto = hcOshaCard21Repository.FindByEstimateId(lastEstimateId, (year - 1).ToString());

            dto.ESTIMATE_ID = newEstimateId;
            dto.YEAR = year.ToString();

            hcOshaCard21Repository.Insert(dto);


            HC_OSHA_CARD22 item = hcOshaCard22Repository.FindByEstimateId(lastEstimateId);
            item.ESTIMATE_ID = newEstimateId;
            item.YEAR = year.ToString();

            hcOshaCard22Repository.Insert(item);
        }
    }
}
