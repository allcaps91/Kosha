namespace HC_Main.Service
{
    using System.Collections.Generic;
    using HC_Main.Repository;
    using HC_Main.Dto;
    using System;
    using ComBase;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvMemoService
    {
        
        private HeaResvMemoRepository heaResvMemoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvMemoService()
        {
			this.heaResvMemoRepository = new HeaResvMemoRepository();
        }

        public List<HEA_RESV_MEMO> GetListByAll()
        {
            return heaResvMemoRepository.GetListByAll();
        }

        public bool Save(IList<HEA_RESV_MEMO> list2)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_RESV_MEMO code in list2)
                {
                    if(!code.EXAMENAME.IsNullOrEmpty())
                    {
                        if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                        {
                            heaResvMemoRepository.Insert(code);
                        }
                        else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                        {
                            heaResvMemoRepository.Update(code);
                        }
                        else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                        {
                            heaResvMemoRepository.Delete(code);
                        }
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
    }
}
