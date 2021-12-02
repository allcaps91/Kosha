namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicCresvService
    {
        private HicCresvRepository hicCresvRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicCresvService()
        {
            this.hicCresvRepository = new HicCresvRepository();
        }

        public List<HIC_CRESV> GetEnvironment(string dtpFDate, string strNextDate)
        {
            return hicCresvRepository.GetEnvironment(dtpFDate, strNextDate);
        }
    }
}