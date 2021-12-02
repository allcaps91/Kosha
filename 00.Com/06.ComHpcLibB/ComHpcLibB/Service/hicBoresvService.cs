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
    public class HicBoresvService
    {
        private HicBoresvRepository hicBoresvRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicBoresvService()
        {
            this.hicBoresvRepository = new HicBoresvRepository();
        }

        public List<HIC_BORESV> GetHealth(string dtpFDate, string strNextDate)
        {
            return hicBoresvRepository.GetHealth(dtpFDate, strNextDate);
        }
    }
}