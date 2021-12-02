namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class HicResultActiveService
    {        
        private HicResultActiveRepository hicResultActiveRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultActiveService()
        {
			this.hicResultActiveRepository = new HicResultActiveRepository();
        }

        public List<HIC_RESULT_ACTIVE> Read_Result(long WRTNO, string HEAPART)
        {
            return hicResultActiveRepository.Read_Result(WRTNO, HEAPART);
        }

        public List<HIC_RESULT_ACTIVE> Read_Active(long WRTNO, string HEAPART = "")
        {
            return hicResultActiveRepository.Read_Active(WRTNO, HEAPART);
        }

        public List<HIC_RESULT_ACTIVE> Read_Active_Hic(long WRTNO, long PANO, string ArgDate, string strJong, string HEAPART = "")
        {
            return hicResultActiveRepository.Read_Active_Hic(WRTNO, PANO, ArgDate, strJong, HEAPART);
        }

        public List<HIC_RESULT_ACTIVE> Read_Result2(long WRTNO, string HEAPART)
        {
            return hicResultActiveRepository.Read_Result2(WRTNO, HEAPART);
        }

        public List<HIC_RESULT_ACTIVE> GetActivebyWrtno(long nWrtNo, string strBuse)
        {
            return hicResultActiveRepository.GetActivebyWrtno(nWrtNo, strBuse);
        }

        public string GetActiveByPanoEntPart(long nPANO, string argEntPart)
        {
            return hicResultActiveRepository.GetActiveByPanoEntPart(nPANO, argEntPart);
        }
    }
}
