namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EndoJupmstResultService
    {
        
        private EndoJupmstResultRepository endoJupmstResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EndoJupmstResultService()
        {
			this.endoJupmstResultRepository = new EndoJupmstResultRepository();
        }

        public List<ENDO_JUPMST_RESULT> GetItembyPtNoJDate(string argPTNO, string argJepDate)
        {
            return endoJupmstResultRepository.GetItembyPtNoJDate(argPTNO, argJepDate);
        }
    }
}
