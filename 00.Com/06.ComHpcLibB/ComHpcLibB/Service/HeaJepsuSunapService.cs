namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    public class HeaJepsuSunapService 
    {
        private HeaJepsuSunapRepository heaJepsuSunapRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuSunapService()
        {
            this.heaJepsuSunapRepository = new HeaJepsuSunapRepository();
        }

        public List<HEA_JEPSU_SUNAP> GetItembyPaNo(long nPano)
        {
            return heaJepsuSunapRepository.GetItembyPaNo(nPano);
        }

        public List<HEA_JEPSU_SUNAP> GetListAll(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero)
        {
            return heaJepsuSunapRepository.GetListAll(fstrFDate, fstrTDate, fstrJong, fstrJob, fnLtdCode, fstrSName, fbZero);
        }

        public List<HEA_JEPSU_SUNAP> GetListSum(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, List<long> AllWrtNo)
        {
            return heaJepsuSunapRepository.GetListSum(fstrFDate, fstrTDate, fstrJong, fstrJob, fnLtdCode, fstrSName, fbZero, AllWrtNo);
        }

        public List<HEA_JEPSU_SUNAP> GetListSum2(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero)
        {
            return heaJepsuSunapRepository.GetListSum2(fstrFDate, fstrTDate, fstrJong, fstrJob, fnLtdCode, fstrSName, fbZero);
        }
    }
}
