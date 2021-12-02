namespace ComHpcLibB.Service
{
    using System;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class BasSunService
    {
        
        private BasSunRepository basSunRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasSunService()
        {
			this.basSunRepository = new BasSunRepository();
        }

        public BAS_SUN FindSugaName(string gstrRetValue)
        {
            return basSunRepository.FindSugaName(gstrRetValue);
        }

        public BAS_SUN GetItembySuCode(string strSuCode)
        {
            return basSunRepository.GetItembySuCode(strSuCode);
        }

        public BAS_SUN GetSuNameKbySuNext(string strSuCode)
        {
            return basSunRepository.GetSuNameKbySuNext(strSuCode);
        }

        public BAS_SUN GetSuNamekUnitbySuCode(string strSuCode)
        {
            return basSunRepository.GetSuNamekUnitbySuCode(strSuCode);
        }
    }
}
