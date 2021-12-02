namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstOrdercodeService
    {
        
        private EndoJupmstOrdercodeRepository endoJupmstOrdercodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstOrdercodeService()
        {
			this.endoJupmstOrdercodeRepository = new EndoJupmstOrdercodeRepository();
        }

        public List<ENDO_JUPMST_ORDERCODE> GetListByRDate(string argFDate, string argTDate, bool chkHc)
        {
            return endoJupmstOrdercodeRepository.GetListByRDate(argFDate, argTDate, chkHc);
        }
    }
}
