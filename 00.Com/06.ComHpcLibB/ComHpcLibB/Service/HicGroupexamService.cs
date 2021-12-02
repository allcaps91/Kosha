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
    public class HicGroupexamService
    {
        
        private HicGroupexamRepository hicGroupexamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupexamService()
        {
			this.hicGroupexamRepository = new HicGroupexamRepository();
        }

        public bool Save(IList<HIC_GROUPEXAM> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HIC_GROUPEXAM code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicGroupexamRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicGroupexamRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hicGroupexamRepository.Delete(code);
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
                hicGroupexamRepository.DeleteAll(fstrCode);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public List<string> GetListExcodeByGrpCode(string strHcGroupCode)
        {
            return hicGroupexamRepository.GetExcodeByGrpCode(strHcGroupCode);
        }

        public List<HIC_GROUPEXAM> GetExCodeByGrCDExCodeIN(string cODE)
        {
            return hicGroupexamRepository.GetExCodeByGrCDExCodeIN(cODE);
        }

        public List<HIC_GROUPEXAM> GetExcodeByGroupCode(List<string> argExcode, List<string> argGroupcode)
        {
            return hicGroupexamRepository.GetExcodeByGroupCode(argExcode, argGroupcode);
        }
        
    }
}
