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
    public class HicScodeService
    {

        private HicScodeRepository hicScodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicScodeService()
        {
            this.hicScodeRepository = new HicScodeRepository();
        }

        public List<HIC_SCODE> FindAll()
        {
            return hicScodeRepository.FindAll();
        }

        public bool Save(IList<HIC_SCODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HIC_SCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicScodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicScodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hicScodeRepository.Delete(code);
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
