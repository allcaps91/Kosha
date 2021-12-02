namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuResBohum2Service
    {
        
        private HicJepsuResBohum2Repository hicJepsuResBohum2Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResBohum2Service()
        {
			this.hicJepsuResBohum2Repository = new HicJepsuResBohum2Repository();
        }

        public HIC_JEPSU_RES_BOHUM2 GetItembyPanoGjYearGjBangiJepsuDate(string strPANO, string strGjYear, string strGJBANGI, string strJEPDATE)
        {
            return hicJepsuResBohum2Repository.GetItembyPanoGjYearGjBangiJepsuDate(strPANO, strGjYear, strGJBANGI, strJEPDATE);
        }

        public HIC_JEPSU_RES_BOHUM2 GetItembyJepDateMirNo(string argFrDate, string argToDate, long argMirno)
        {
            return hicJepsuResBohum2Repository.GetItembyJepDateMirNo(argFrDate, argToDate, argMirno);
        }
    }
}
