namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    using ComBase.Mvc;

    public class HeaJepsuMemoService 
    {
        private HeaJepsuMemoRepository heaJepsuMemoRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuMemoService()
        {
            this.heaJepsuMemoRepository = new HeaJepsuMemoRepository();
        }

        public List<HEA_JEPSU_MEMO> GetItembyPaNo(long nPaNo)
        {
            return heaJepsuMemoRepository.GetItembyPaNo(nPaNo);
        }

        public List<HEA_JEPSU_MEMO> GetItembyPaNo(string Ptno)
        {
            return heaJepsuMemoRepository.GetItembyPtno(Ptno);
        }
    }
}
