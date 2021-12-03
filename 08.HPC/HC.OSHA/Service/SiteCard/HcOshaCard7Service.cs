namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard7Service
    {
        
        private HcOshaCard7Repository hcOshaCard7Repository;
        private HcOshaCard7_1Repository hcOshaCard7_1Repository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard7Service()
        {
			this.hcOshaCard7Repository = new HcOshaCard7Repository();
            hcOshaCard7_1Repository = new HcOshaCard7_1Repository();
        }

        public List<HC_OSHA_CARD7> FindAll(long Estimate_Id)
        {
            return hcOshaCard7Repository.FindAll(Estimate_Id);
        }
        public bool Save(IList<HC_OSHA_CARD7> list, long estimateId, long siteId)
        {
            try
            {
                foreach (HC_OSHA_CARD7 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.SITE_ID = siteId;
                        dto.ESTIMATE_ID = estimateId;
                        hcOshaCard7Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard7Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard7Repository.Delete(dto.ID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }




        public List<HC_OSHA_CARD7_1> FindAllByMeet(long estimateId)
        {
            return hcOshaCard7_1Repository.FindAll(estimateId);
        }

        public List<HC_OSHA_CARD7_1> FindAllByMeet(long estimateId, string startYear, string endYear)
        {
            return hcOshaCard7_1Repository.FindAll(estimateId, startYear, endYear);
        }
        public bool SaveMeet(IList<HC_OSHA_CARD7_1> list, long estimateId, long siteId, string year)
        {
            try
            {
                foreach (HC_OSHA_CARD7_1 dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.SITE_ID = siteId;
                        dto.ESTIMATE_ID = estimateId;
                        dto.YEAR = year;
                        hcOshaCard7_1Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard7_1Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard7_1Repository.Delete(dto.ID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        internal void COPY_HIC_OSHA_CARD7(int year, long newEstimateId, long lastEstimateId)
        {
            List<HC_OSHA_CARD7>  list = hcOshaCard7Repository.FindAll(lastEstimateId);
            foreach(HC_OSHA_CARD7 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard7Repository.Insert(dto);
            }

            List<HC_OSHA_CARD7_1> list2 = hcOshaCard7_1Repository.FindAll(lastEstimateId);
            foreach (HC_OSHA_CARD7_1 dto in list2)
            {
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard7_1Repository.Insert(dto);
            }
        }
    }
}
