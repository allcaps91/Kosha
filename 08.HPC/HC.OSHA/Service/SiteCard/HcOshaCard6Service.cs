namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 산업재해 서비스
    /// </summary>
    public class HcOshaCard6Service
    {
        
        private HcOshaCard6Repository hcOshaCard6Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard6Service()
        {
			this.hcOshaCard6Repository = new HcOshaCard6Repository();
        }

        public List<HC_OSHA_CARD6> FindAll(long estimateId)
        {
            List<HC_OSHA_CARD6> list = hcOshaCard6Repository.FindAll(estimateId);
            foreach(HC_OSHA_CARD6 card in list)
            {
                card.JUMIN_NO = clsAES.DeAES(card.JUMIN_NO);

                if (card.JUMIN_NO.Length>6)
                {
                    card.JUMIN_NO = card.JUMIN_NO.Substring(0, 6) +"-"+ card.JUMIN_NO.Substring(6, 1) +"*****";
                }
             
            }
            return list;
            
        }
        public void DeleteByReportid(long report_id)
        {
            hcOshaCard6Repository.DeleteByReportid(report_id);
        }

        public void Save(HC_OSHA_CARD6 dto)
        {
            hcOshaCard6Repository.Insert(dto);
        }
        public bool Save(IList<HC_OSHA_CARD6> list, long estimateId)
        {
            try
            {
                foreach (HC_OSHA_CARD6 dto in list)
                {
                    if (dto.JUMIN_NO!=null)
                    {
                        dto.JUMIN_NO = clsAES.AES(dto.JUMIN_NO);
                    }
                    
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.ESTIMATE_ID = estimateId;
                        hcOshaCard6Repository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaCard6Repository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaCard6Repository.Delete(dto.ID);
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

        internal void COPY_HIC_OSHA_CARD6(int year, long newEstimateId, long lastEstimateId)
        {
            string lastStartYear = string.Concat(year - 1, "-01-01 00:00:00");
            string lastEndYear = string.Concat(year - 1, "-12-31 23:59:59");
            List<HC_OSHA_CARD6> list = hcOshaCard6Repository.FindAllByYear(lastEstimateId, lastStartYear, lastEndYear);

            foreach(HC_OSHA_CARD6 dto in list)
            {
                dto.ESTIMATE_ID = newEstimateId;
                hcOshaCard6Repository.Insert(dto);
            }
        }
    }
}
