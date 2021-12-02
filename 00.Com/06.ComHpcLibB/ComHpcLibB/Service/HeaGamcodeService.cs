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
    public class HeaGamcodeService
    {        
        private HeaGamcodeRepository heaGamcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGamcodeService()
        {
			this.heaGamcodeRepository = new HeaGamcodeRepository();
        }

        public string Read_Hea_GamName(string strCode)
        {
            return heaGamcodeRepository.Read_Hea_GamName(strCode);
        }

        public List<HEA_GAMCODE> GetListItems(bool bDel)
        {
            return heaGamcodeRepository.GetListItems(bDel);
        }

        public bool Save(IList<HEA_GAMCODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_GAMCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        heaGamcodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        heaGamcodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        heaGamcodeRepository.Delete(code);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        public HEA_GAMCODE GetItemByCode(string strGamCode)
        {
            return heaGamcodeRepository.GetItemByCode(strGamCode);
        }
    }
}
