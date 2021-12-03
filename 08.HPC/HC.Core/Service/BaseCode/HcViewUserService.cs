namespace HC.Core.Service
{
    using System.Collections.Generic;
    using HC.Core.Repository;
    using HC.Core.Dto;
    
    
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
