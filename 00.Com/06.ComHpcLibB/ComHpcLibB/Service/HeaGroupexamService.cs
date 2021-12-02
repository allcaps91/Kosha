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
    public class HeaGroupexamService
    {
        
        private HeaGroupexamRepository heaGroupexamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupexamService()
        {
			this.heaGroupexamRepository = new HeaGroupexamRepository();
        }

        public bool Save(IList<HEA_GROUPEXAM> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_GROUPEXAM code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        heaGroupexamRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        heaGroupexamRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        heaGroupexamRepository.Delete(code);
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

        public bool DeleteAll(string fstrCode)
        {
            try
            {
                heaGroupexamRepository.DeleteAll(fstrCode);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public string ExistByGrpCodeExCode(string strGrpCode, string strExCode)
        {
            return heaGroupexamRepository.ExistByGrpCodeExCode(strGrpCode, strExCode);
        }
    }
}
