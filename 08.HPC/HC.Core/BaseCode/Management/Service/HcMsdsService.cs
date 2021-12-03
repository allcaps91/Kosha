namespace HC.Core.BaseCode.MSDS.Service
{
    using System.Collections.Generic;
    using HC.Core.BaseCode.MSDS.Repository;
    using HC.Core.BaseCode.MSDS.Dto;
    using System.Drawing;
    using ComBase.Mvc.Exceptions;


    /// <summary>
    /// MSDS 기초코드 
    /// </summary>
    public class HcMsdsService
    {
        public Dictionary<string, Image> GhsPictures { get; }

        private HcMsdsRepository hcMsdsRepository;
        
        public HcMsdsService()
        {
			this.hcMsdsRepository = new HcMsdsRepository();
            GhsPictures = new Dictionary<string, Image>();

            GhsPictures.Add("GHS01.gif", HC.Core.Properties.Resources.GHS01);
            GhsPictures.Add("GHS02.gif", HC.Core.Properties.Resources.GHS02);
            GhsPictures.Add("GHS03.gif", HC.Core.Properties.Resources.GHS03);
            GhsPictures.Add("GHS04.gif", HC.Core.Properties.Resources.GHS04);
            GhsPictures.Add("GHS05.gif", HC.Core.Properties.Resources.GHS05);
            GhsPictures.Add("GHS06.gif", HC.Core.Properties.Resources.GHS06);
            GhsPictures.Add("GHS07.gif", HC.Core.Properties.Resources.GHS07);
            GhsPictures.Add("GHS08.gif", HC.Core.Properties.Resources.GHS08);
            GhsPictures.Add("GHS09.gif", HC.Core.Properties.Resources.GHS09);
        }
        
        public Image GetGhsPicture(string fileName)
        {
            Image image;
            if(GhsPictures.TryGetValue(fileName, out image))
            {
                return image;
            }
            else
            {
                throw new MTSException(fileName + " 이 없습니다 ");
            }
            
        }
        public void Delete(long id)
        {
            hcMsdsRepository.Delete(id);
        }
        public HC_MSDS Save(HC_MSDS dto)
        {
            HC_MSDS saved = hcMsdsRepository.FindByCasNo(dto.CASNO);
            if ( saved == null)
            {
                return hcMsdsRepository.Save(dto);
            }
            else
            {
                dto.ID = saved.ID;
                return hcMsdsRepository.Update(dto);
            }
        }

        public List<HC_MSDS> Search(bool isName, string searchWord)
        {
            if (isName)
            {
                return hcMsdsRepository.FindByName(searchWord);
            }
            List<HC_MSDS> list = new List<HC_MSDS>();
            list.Add(hcMsdsRepository.FindByCasNo(searchWord));
            return list;

        }

    }
}
