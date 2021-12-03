namespace HC.OSHA.Site.Management.Service
{
    using ComBase.Mvc.Utils;
    using ComHpcLibB.Model;
    using HC.OSHA.Site.Management.Model;
    using HC.OSHA.Site.Management.Repository;
    using System.Collections.Generic;
    using HC_OSHA_SITE = ComHpcLibB.Model.HC_OSHA_SITE;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaSiteService
    {
        
        private HcOshaSiteRepository hcOshaSiteRepository;
        private HcOshaSiteModelRepository hcOshaSiteModelRepository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaSiteService()
        {
			this.hcOshaSiteRepository = new HcOshaSiteRepository();
            this. hcOshaSiteModelRepository = new HcOshaSiteModelRepository();
        }

        /// <summary>
        /// 보건관리 사업장 등록
        /// </summary>
        /// <param name="view"></param>
        public bool Save(HC_SITE_VIEW view)
        {
            bool result = false;
            HC_OSHA_SITE site = hcOshaSiteRepository.FindById(view.ID);
            if(site == null)
            {
                hcOshaSiteRepository.Save(view);
                result = true;
            }
            return result;            
        }
        public HC_OSHA_SITE_MODEL FindById(long id)
        {
           return hcOshaSiteModelRepository.FindById(id);
           
        }
        /// <summary>
        /// 아이디 또는 이름으로 검색
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<HC_OSHA_SITE_MODEL> Search(string name)
        {
            if (name.IsNumeric())
            {
                return hcOshaSiteModelRepository.FindById(name);
            }
            else
            {
                return hcOshaSiteModelRepository.FindByName(name);
            }
        }
    }
}
