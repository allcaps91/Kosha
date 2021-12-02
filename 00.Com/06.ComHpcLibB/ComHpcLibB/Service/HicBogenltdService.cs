namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HicBogenltdService
    {

        private HicBogenltdRepository HicBogenltdRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicBogenltdService()
        {
            this.HicBogenltdRepository = new HicBogenltdRepository();
        }

        public List<HIC_BOGENLTD> GetDaeHang(long LTDCODE)
        {
            return HicBogenltdRepository.GetDaeHang(LTDCODE);
        }
    }
}
