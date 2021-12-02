namespace ComHpcLibB.Service
{
    using ComHpcLibB.Repository;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasPcconfigService
    {
        
        private BasPcconfigRepository basPcconfigRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasPcconfigService()
        {
			this.basPcconfigRepository = new BasPcconfigRepository();
        }

        public List<BAS_PCCONFIG> GetConfig(string IPADDRESS)
        {
            return basPcconfigRepository.GetConfig(IPADDRESS);
        }

        public BAS_PCCONFIG GetConfig_PFTSN(string IPADDRESS)
        {
            return basPcconfigRepository.GetConfig_PFTSN(IPADDRESS);
        }

        public string GetConfig_CardGubun(string IPADDRESS)
        {
            return basPcconfigRepository.GetConfig_CardGubun(IPADDRESS);
        }

        public string GetConfig_Check(string IPADDRESS, string sGubun)
        {
            return basPcconfigRepository.GetConfig_Check(IPADDRESS, sGubun);
        }

        public int Save_PcConfig(BAS_PCCONFIG item)
        {
            return basPcconfigRepository.Save_PcConfig(item);
        }

        public int Save_PcConfig_Test(BAS_PCCONFIG item)
        {
            return basPcconfigRepository.Save_PcConfig_Test(item);
        }

        public string GetConfig_Code(string gstrCOMIP, string sGubun)
        {
            return basPcconfigRepository.GetConfig_Code(gstrCOMIP, sGubun);
        }

        public int Delete_PcConfig(string sIpAddress)
        {
            return basPcconfigRepository.GetConfig_Code(sIpAddress);
        }
    }
}
