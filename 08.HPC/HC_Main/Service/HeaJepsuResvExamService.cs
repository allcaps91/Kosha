namespace HC_Main.Service
{
    using System.Collections.Generic;
    using HC_Main.Repository;
    using HC_Main.Model;
    using System;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuResvExamService
    {
        
        private HeaJepsuResvExamRepository heaJepsuResvExamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuResvExamService()
        {
			this.heaJepsuResvExamRepository = new HeaJepsuResvExamRepository();
        }

        public List<HEA_JEPSU_RESV_EXAM> GetListBySDateCfmYN(string argFDate, string argTDate, int argCFM, string argSName, int nSort)
        {
            return heaJepsuResvExamRepository.GetListBySDateCfmYN(argFDate, argTDate, argCFM, argSName, nSort);
        }

        public bool Save(IList<HEA_JEPSU_RESV_EXAM> list)
        {
            try
            {
                foreach (HEA_JEPSU_RESV_EXAM code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        heaJepsuResvExamRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        heaJepsuResvExamRepository.Update(code);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }


    }
}
