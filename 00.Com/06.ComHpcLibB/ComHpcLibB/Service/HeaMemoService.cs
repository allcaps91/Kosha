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
    public class HeaMemoService
    {
        
        private HeaMemoRepository heaMemoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaMemoService()
        {
			this.heaMemoRepository = new HeaMemoRepository();
        }

        public int Insert(HEA_MEMO item)
        {
            return heaMemoRepository.Insert(item);
        }

        public int Delete(string strROWID)
        {
            return heaMemoRepository.Update(strROWID);
        }

        public bool Save(IList<HEA_MEMO> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_MEMO code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        heaMemoRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        heaMemoRepository.Update(code.RID);
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

        public List<HEA_MEMO> GetItembyPaNo(string argPano)
        {
            return heaMemoRepository.GetItembyPaNo(argPano);
        }
    }
}
