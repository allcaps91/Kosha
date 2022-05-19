namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using ComBase;
    using System;
    using System.Drawing;
    using HC.Core.BaseCode.MSDS.Service;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HcMsdsProductService
    {

        private HcMsdsProductRepository hcMsdsProductRepository;

        private HcMsdsService hcMsdsService;
        /// <summary>
        /// 
        /// </summary>
        public HcMsdsProductService()
        {
            this.hcMsdsProductRepository = new HcMsdsProductRepository();
            this.hcMsdsService = new HcMsdsService();
        }

        public List<HC_MSDS_PRODUCT> FindAll(string viewData)
        {
            List<HC_MSDS_PRODUCT> list = hcMsdsProductRepository.FindAll(viewData);
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
        public void Update_Msdslist(long id,string msdslist)
        {
            this.hcMsdsProductRepository.Update_Msdslist(id, msdslist);
        }

        public void Delete(long id)
        {
            this.hcMsdsProductRepository.Delete(id);
        }
        public bool Save(IList<HC_MSDS_PRODUCT> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_MSDS_PRODUCT dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hcMsdsProductRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcMsdsProductRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcMsdsProductRepository.Delete(dto.ID);
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
