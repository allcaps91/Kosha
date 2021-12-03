namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HicMemoService
    {
        
        private HicMemoRepository hicMemoRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMemoService()
        {
			this.hicMemoRepository = new HicMemoRepository();
        }
                              
        public List<HIC_MEMO> GetItembyPaNo(long nPano)
        {
            return hicMemoRepository.GetItembyPaNo(nPano);
        }

        public int UpdatebyRowId(string strROWID, string argJob = "")
        {
            HIC_MEMO item = new HIC_MEMO
            {
                RID = strROWID,
                JOBGBN = argJob
            };

            return hicMemoRepository.UpdatebyRowId(item);
        }

        public int Insert(HIC_MEMO item)
        {
            return hicMemoRepository.Insert(item);
        }

        public int DeleteData(string argRowid, string strGbn = "")
        {
            return hicMemoRepository.DeleteData(argRowid, strGbn);
        }

        public int InsertPanoMemoJobsabun(HIC_MEMO item)
        {
            return hicMemoRepository.InsertPanoMemoJobsabun(item);
        }

        public bool Save(IList<HIC_MEMO> list, string strJob, string strGubun)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HIC_MEMO code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        if (code.MEMO.To<string>("").Trim() != "")
                        {
                            code.JOBGBN = strJob;
                            code.GUBUN1 = strGubun;
                            hicMemoRepository.Insert(code);
                        }
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (code.RID.To<string>("").Trim() != "")
                        {
                            code.JOBGBN = strJob;
                            hicMemoRepository.UpdatebyRowId(code);
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

        public List<HIC_MEMO> GetItembyPaNo(string argPano, string argGubun)
        {
            return hicMemoRepository.GetItembyPaNo(argPano, argGubun);
        }

        public List<HIC_MEMO> GetHicItembyPaNo(string argPtno)
        {
            return hicMemoRepository.GetHicItembyPaNo(argPtno);
        }

        public List<HIC_MEMO> GetHeaItembyPaNo(string argPtno)
        {
            return hicMemoRepository.GetHeaItembyPaNo(argPtno);
        }
    }
}
