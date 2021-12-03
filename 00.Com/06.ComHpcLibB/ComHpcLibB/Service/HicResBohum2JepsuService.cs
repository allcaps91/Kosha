namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum2JepsuService
    {
        
        private HicResBohum2JepsuRepository hicResBohum2JepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum2JepsuService()
        {
			this.hicResBohum2JepsuRepository = new HicResBohum2JepsuRepository();
        }

        public HIC_RES_BOHUM2_JEPSU GetItembyLtdCodeJepDate(long nPaNo, string fstrJepDate, string strYear, string strBangi)
        {
            return hicResBohum2JepsuRepository.GetItembyLtdCodeJepDate(nPaNo, fstrJepDate, strYear, strBangi);
        }
    }
}
