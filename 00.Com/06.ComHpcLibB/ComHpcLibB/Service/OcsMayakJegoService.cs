namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class OcsMayakJegoService
    {
        
        private OcsMayakJegoRepository ocsMayakJegoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public OcsMayakJegoService()
        {
			this.ocsMayakJegoRepository = new OcsMayakJegoRepository();
        }

        public List<OCS_MAYAK_JEGO> GetListByDate(string argDate, string argGbn)
        {
            return ocsMayakJegoRepository.GetListByDate(argDate, argGbn);
        }

        public OCS_MAYAK_JEGO GetCodeInfoItemBySucode(string argSucode, string argDate)
        {
            return ocsMayakJegoRepository.GetCodeInfoItemBySucode(argSucode, argDate);
        }

        public List<OCS_MAYAK_JEGO> GetDayJegoBySucodeDate(string argSucode, string argDate, string argGbn)
        {
            return ocsMayakJegoRepository.GetDayJegoBySucodeDate(argSucode, argDate, argGbn);
        }

        public string GetDrugQtyBySucode(string argSucde, string argDate)
        {
            return ocsMayakJegoRepository.GetDrugQtyBySucode(argSucde, argDate);
        }

        public List<OCS_MAYAK_JEGO> GetDayUsedBySucodeDate(string argSucode, string argDate, string argGbn, int nChasu, string argJob)
        {
            return ocsMayakJegoRepository.GetDayUsedBySucodeDate(argSucode, argDate, argGbn, nChasu, argJob);
        }

        public string GetOcsDrugSetRowidBySucode(string argSuCode, string argDate)
        {
            return ocsMayakJegoRepository.GetOcsDrugSetRowidBySucode(argSuCode, argDate);
        }

        public int UpdateOcsDrugSetQtyByRowid(string argQty, string argBQty, string argRowid)
        {
            return ocsMayakJegoRepository.UpdateOcsDrugSetQtyByRowid(argQty, argBQty, argRowid);
        }

        public int InsertOcsDrugSet(OCS_MAYAK_JEGO item)
        {
            return ocsMayakJegoRepository.InsertOcsDrugSet(item);
        }

        public int UpdateOcsDrugSetQty(string argQty, string argBuildDate, string  argSuCode)
        {
            return ocsMayakJegoRepository.UpdateOcsDrugSetQty(argQty, argBuildDate, argSuCode);
        }
    }
}
