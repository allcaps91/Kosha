namespace HC.OSHA.Site.ETC.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Site.ETC.Repository;
    using HC.OSHA.Site.ETC.Dto;
    using ComBase;
    using System;
    using System.Drawing;
    using HC.Core.BaseCode.MSDS.Service;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteProductService
    {
        
        private HcSiteProductRepository hcSiteProductRepository;

        private HcMsdsService hcMsdsService;
        /// <summary>
        /// 
        /// </summary>
        public HcSiteProductService()
        {
			this.hcSiteProductRepository = new HcSiteProductRepository();
            this.hcMsdsService = new HcMsdsService();
        }

        public List<HC_SITE_PRODUCT> FindAll(long estimateId)
        {
            //개당그림크기 원본의 3%로 크기 축소하여 30*30
            //가로 최대 9개 = 3*90 = 270
            
            List<HC_SITE_PRODUCT> list = hcSiteProductRepository.FindAll(estimateId);
            foreach(HC_SITE_PRODUCT dto in list)
            {
                if (!dto.GHSPICTURE.IsNullOrEmpty())
                {
                    dto.GhsImage = GetProductGHSPictures(dto.GHSPICTURE);
                }
                
              
            }
            return list;
        }

        /// <summary>
        /// MSDS 제품 ghs 사진 합성
        /// </summary>
        public Bitmap GetProductGHSPictures(string ghs_pictures)
        {
            int width = 270;
            int height = 30;

            string[] pictures = ghs_pictures.Split('|');
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            for (int i = 0; i < pictures.Length; i++)
            {
                if (!pictures[i].IsNullOrEmpty())
                {
                    g.DrawImageUnscaled(hcMsdsService.GetGhsPicture(pictures[i]), i * 30, 0);
                }
                
            }
            g.Save();
            g.Dispose();
            return bitmap;
        }

        public void Delete(long id)
        {
            this.hcSiteProductRepository.Delete(id);
        }
        public bool Save(long estimateId, IList<HC_SITE_PRODUCT> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_SITE_PRODUCT dto in list)
                {
                    dto.ESTIMATE_ID = estimateId;
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hcSiteProductRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcSiteProductRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcSiteProductRepository.Delete(dto.ID);
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
