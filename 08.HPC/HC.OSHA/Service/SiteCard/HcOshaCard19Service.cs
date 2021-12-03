namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    
    public class HcOshaCard19Service
    {

        public HcOshaCard19Repository hcOshaCard19Repository { get; }

        /// <summary>
        /// 
        /// </summary>
        public HcOshaCard19Service()
        {
            this.hcOshaCard19Repository = new HcOshaCard19Repository();
        }

        public HC_OSHA_CARD19 Save(HC_OSHA_CARD19 dto)
        {
            if (dto.ID > 0)
            {
                return hcOshaCard19Repository.Update(dto);
            }
            else
            {
                return hcOshaCard19Repository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_CARD19 dto)
        {
            hcOshaCard19Repository.Delete(dto.ID);
        }
    }
}
