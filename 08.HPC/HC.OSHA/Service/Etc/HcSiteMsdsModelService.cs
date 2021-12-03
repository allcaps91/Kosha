namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Model;
    using System.Drawing;
    using ComBase.Controls;
    using HC.Core.BaseCode.MSDS.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteMsdsModelService
    {
        
        private HcSiteMsdsModelRepository hcSiteMsdsModelRepository;

        private HcMsdsService hcMsdsService;
        /// <summary>
        /// 
        /// </summary>
        public HcSiteMsdsModelService()
        {
			this.hcSiteMsdsModelRepository = new HcSiteMsdsModelRepository();
            hcMsdsService = new HcMsdsService();
        }

        public List<HC_SITE_MSDS_MODEL> FindAll(long siteId)
        {
            List<HC_SITE_MSDS_MODEL> list = hcSiteMsdsModelRepository.FindAll(siteId);

            foreach (HC_SITE_MSDS_MODEL dto in list)
            {
                //자동 머지가 되지 않아 빈물자열로 교체
                if (dto.MANUFACTURER.Empty())
                {
                    dto.MANUFACTURER = " ";
                }
                if (dto.USAGE.Empty())
                {
                    dto.USAGE = " ";
                }
                if (dto.REVISIONDATE.Empty())
                {
                    dto.REVISIONDATE = " ";
                }
                if (dto.QTY.Empty())
                {
                    dto.QTY = " ";
                }
                if (dto.MONTHLYAMOUNT.Empty())
                {
                    dto.MONTHLYAMOUNT = " ";
                }
                if (dto.UNIT.Empty())
                {
                    dto.UNIT = " ";
                }
                //if (dto.GHSPICTURE.NotEmpty())
                //{
                //    //dto.GHS_IMAGE = GetProductGHSPictures(dto.GHSPICTURE);
                //    dto.GHS_IMAGE = GetProductGHSPictures(dto.BASE_GHSPICTURE);
                //}
                if (dto.BASE_GHSPICTURE.NotEmpty() && dto.BASE_GHSPICTURE !="자료없음")
                {
                    //dto.GHS_IMAGE = GetProductGHSPictures(dto.GHSPICTURE);
                    dto.GHS_IMAGE = GetProductGHSPictures(dto.BASE_GHSPICTURE);
                }
              

            }
            return list;

        }
        /// <summary>
        /// MSDS 제품 ghs 사진 합성
        ///   
        /// 개당그림크기 원본의 3%로 크기 축소하여 30*30
        /// 가로 3개 세로 3개 최대 9개 = 90*90
        /// </summary>
        public Bitmap GetProductGHSPictures(string ghs_pictures)
        {
            //개당그림크기 원본의 3%로 크기 축소하여 30*30
            //가로 3개 세로 3개 최대 9개 = 90*90
            int width = 90;
            int height = 90;

            string[] pictures = ghs_pictures.Split('|');

            if(pictures.Length <= 3)
            {
                height = 30;
            }
            else if (pictures.Length >3 && pictures.Length <=6)
            {
                height = 60;
            }
            
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            int second = 0;
            int third = 0;
            for (int i = 0; i < pictures.Length; i++)
            {
                if (!pictures[i].IsNullOrEmpty())
                {
            
                    if (i>=0 && i <= 2)
                    {
                        g.DrawImageUnscaled(hcMsdsService.GetGhsPicture(pictures[i]), i * 30, 0);
                    }
                    else if (i >= 3 && i <= 5)
                    {
                       
                        g.DrawImageUnscaled(hcMsdsService.GetGhsPicture(pictures[i]), second * 30, 30);
                        second += 1;
                    }
                    else 
                    {
                        g.DrawImageUnscaled(hcMsdsService.GetGhsPicture(pictures[i]), third * 30, 60);
                        third += 1;
                    }
                }
            }
            g.Save();
            g.Dispose();
            return bitmap;
        }
    }
}
