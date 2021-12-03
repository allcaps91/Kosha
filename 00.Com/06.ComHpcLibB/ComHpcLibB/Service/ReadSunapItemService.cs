namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ReadSunapItemService 
    {
        private ReadSunapItemRepository readSunapItemRepository;

        /// <summary>
        /// 
        /// </summary>
        public ReadSunapItemService()
        {
            this.readSunapItemRepository = new ReadSunapItemRepository();
        }

        public List<READ_SUNAP_ITEM> GetListByGWrtno(long nGWRTNO, string argIdx = "")
        {
            return readSunapItemRepository.GetListByGWrtno(nGWRTNO, argIdx);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapInfoByWrtno(long argWRTNO)
        {
            return readSunapItemRepository.GetHicSunapInfoByWrtno(argWRTNO);
        }

        public List<READ_SUNAP_ITEM> GetHeaSunapInfoByWrtno(long argWRTNO)
        {
            return readSunapItemRepository.GetHeaSunapInfoByWrtno(argWRTNO);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapWorkInfoByWrtno(long argPano, string argJong)
        {
            return readSunapItemRepository.GetHicSunapWorkInfoByWrtno(argPano, argJong);
        }

        public List<READ_SUNAP_ITEM> GetListHicGrpCodeByJong(string argJong, string argDate)
        {
            return readSunapItemRepository.GetListHicGrpCodeByJong(argJong, argDate);
        }

        public List<READ_SUNAP_ITEM> GetListHeaGrpCodeByJong(string argJong, string argDate, long nLtdCode, string argSex)
        {
            return readSunapItemRepository.GetListHeaGrpCodeByJong(argJong, argDate, nLtdCode, argSex);
        }

        public READ_SUNAP_ITEM GetItemByCode(string argCode, string argGbn = "")
        {
            return readSunapItemRepository.GetHicItemByCode(argCode, argGbn);
        }

        public List<READ_SUNAP_ITEM> GetHicSunapHisInfoByWrtno(long argWRTNO)
        {
            return readSunapItemRepository.GetHicSunapHisInfoByWrtno(argWRTNO);
        }

        public READ_SUNAP_ITEM GetHeaItemByCode(string argCode)
        {
            return readSunapItemRepository.GetHeaItemByCode(argCode);
        }

        public List<READ_SUNAP_ITEM> GetHeaGrpCodeListByNameLike(string argWard, string argName)
        {
            return readSunapItemRepository.GetHeaGrpCodeListByNameLike(argWard, argName);
        }
    }
}
