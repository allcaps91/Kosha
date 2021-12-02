namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using ComBase;
    using System;
    using HC.OSHA.Model;


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
        public void Delete(long id)
        {
            hcSiteProductMsdsRepository.Delete(id);
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
                        ID = model.ID,
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
                        hcSiteProductMsdsRepository.Delete(dto.ID);
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
