namespace HC.Core.Service
{
    using System.Collections.Generic;
    using HC.Core.Repository;
    using HC.Core.Dto;
    using ComBase.Mvc.Utils;
    using System;
    using ComHpcLibB.Model;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteViewService
    {
        
        private HcSiteViewRepository hcSiteViewRepository;
        private HcUsersRepository hcUsersRepository;
        /// <summary>
        /// 
        /// </summary>
        public HcSiteViewService()
        {
			this.hcSiteViewRepository = new HcSiteViewRepository();
            hcUsersRepository = new HcUsersRepository();
        }
        public HC_SITE_VIEW FindById(long id)
        {
            return hcSiteViewRepository.FindById(id);
        }

        public List<HC_SITE_VIEW> Search(string idOrName, string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                userId = CommonService.Instance.Session.UserId;
            }
            HC_USER user = hcUsersRepository.FindOne(userId);
            Role role = Role.ENGINEER;

            if (user != null)
            {
                role = (Role)Enum.Parse(typeof(Role), user.Role);
            }

            if (idOrName.IsNumeric())
            {

                return hcSiteViewRepository.FindAllById(idOrName, userId, role);
            }
            else
            {
                return hcSiteViewRepository.FindAllByName(idOrName, userId, role);
            }
        }

        
    }
}
