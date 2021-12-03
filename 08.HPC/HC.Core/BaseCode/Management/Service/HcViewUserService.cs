namespace HC.Core.BaseCode.Management.Service
{
    using System.Collections.Generic;
    using HC.Core.BaseCode.Management.Repository;
    using HC.Core.BaseCode.Management.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcViewUserService
    {
        
        private HcViewUserRepository viewUserRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcViewUserService()
        {
			this.viewUserRepository = new HcViewUserRepository();
        }

        public List<HC_VIEW_USER> FinAll()
        {
            return viewUserRepository.FindAll();
        }
    }
}
