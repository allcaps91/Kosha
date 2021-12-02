namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BasGamfService
    {        
        private BasGamfRepository basGamfRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasGamfService()
        {
			this.basGamfRepository = new BasGamfRepository();
        }
        
        public BAS_GAMF Read_Gam_Opd(string strJumin)
        {
            return basGamfRepository.Read_Gam_Opd(strJumin);
        }
    }
}
