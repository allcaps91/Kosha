namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BasSutService
    {
        
        private BasSutRepository basSutRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasSutService()
        {
			this.basSutRepository = new BasSutRepository();
        }

        public BAS_SUT GetItembySuCode(string strSuCode)
        {
            return basSutRepository.GetItembySuCode(strSuCode);
        }

    }
}
