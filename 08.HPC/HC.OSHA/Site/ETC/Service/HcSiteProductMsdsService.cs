namespace HC.OSHA.Site.ETC.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.ETC.Repository;
    using HC.OSHA.Site.ETC.Dto;
    using ComBase;
    using System;
    using HC.OSHA.Site.ETC.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductMsdsService
    {
        
        private HcSiteProductMsdsRepository hcSiteProductMsdsRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcSiteProductMsdsService()
        {
			this.hcSiteProductMsdsRepository = new HcSiteProductMsdsRepository();
        }

        public List<HC_SITE_PRODUCT_MSDS_MODEL> FindAll(long SITE_PRODUCT_ID)
        {
            return hcSiteProductMsdsRepository.FindAll(SITE_PRODUCT_ID);
        }
        public void Delete(long SITE_PRODUCT_ID, long MSDS_ID)
        {
            hcSiteProductMsdsRepository.Delete(SITE_PRODUCT_ID, MSDS_ID);
        }
        public bool Save(IList<HC_SITE_PRODUCT_MSDS_MODEL> modelList)
        {
            try
            {
                List<HC_SITE_PRODUCT_MSDS> list = new List<HC_SITE_PRODUCT_MSDS>();
                foreach(HC_SITE_PRODUCT_MSDS_MODEL model in modelList)
                {
                    HC_SITE_PRODUCT_MSDS dto = new HC_SITE_PRODUCT_MSDS()
                    {
                        SITE_PRODUCT_ID = model.SITE_PRODUCT_ID,
                        MSDS_ID = model.MSDS_ID,
                        QTY = model.QTY,
                        RowStatus = model.RowStatus
                    };
                    list.Add(dto);
                }

                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_SITE_PRODUCT_MSDS dto in list)
                {

                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hcSiteProductMsdsRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcSiteProductMsdsRepository.Update(dto);
                    }
                    else
                    {
                        hcSiteProductMsdsRepository.Delete(dto.SITE_PRODUCT_ID, dto.MSDS_ID);
                    }
                 
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }
    }
}
