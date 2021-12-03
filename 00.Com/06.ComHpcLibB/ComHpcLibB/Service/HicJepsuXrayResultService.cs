namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuXrayResultService
    {
        
        private HicJepsuXrayResultRepository hicJepsuXrayResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuXrayResultService()
        {
			this.hicJepsuXrayResultRepository = new HicJepsuXrayResultRepository();
        }

        public List<HIC_JEPSU_XRAY_RESULT> GetListbyPaNo(long nPano)
        {
            return hicJepsuXrayResultRepository.GetListbyPaNo(nPano);
        }

        public List<HIC_JEPSU_XRAY_RESULT> GetListbyXrayNo(string strXRAYNO)
        {
            return hicJepsuXrayResultRepository.GetListbyXrayNo(strXRAYNO);
        }
    }
}
