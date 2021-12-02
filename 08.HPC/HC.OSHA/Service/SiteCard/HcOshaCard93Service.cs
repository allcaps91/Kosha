namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaCard93Service
    {
        
        public HcOshaCard93Repository hcOshaCard93Repository { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard93Service()
        {
			this.hcOshaCard93Repository = new HcOshaCard93Repository();
        }


        public List<HC_OSHA_CARD9_3> FindAll(long siteId)
        {
            List< HC_OSHA_CARD9_3 > list = this.hcOshaCard93Repository.FindAll(siteId);
            foreach(HC_OSHA_CARD9_3 dto in list)
            {
                dto.SITE_PERIOD = dto.SITESTARTDATE + "~" + dto.SITEENDDATE;
                dto.SITE_GRADE_NAME = dto.SITEGRADE + "/" + dto.SITENAME;

                dto.TEST_PERIOD = dto.TESTSTARTDATE + "~" + dto.TESTENDDATE;
                dto.TEST_GRADE_NAME = dto.TESTGRADE + "/" + dto.TESTNAME;
            }

            return list;
        }

        public HC_OSHA_CARD9_3 Save(HC_OSHA_CARD9_3 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard93Repository.Update(dto);
            }
            else
            {
                return hcOshaCard93Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD9_3 dto)
        {
            hcOshaCard93Repository.Delete(dto.ID);
        }
    }
}
