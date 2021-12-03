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
    public class BasBcodeService
    {
        
        private BasBcodeRepository basBcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasBcodeService()
        {
			this.basBcodeRepository = new BasBcodeRepository();
        }

        public BAS_BCODE GetAllByGubunCode(string argGubun, string argCode)
        {
            return basBcodeRepository.GetAllByGubunCode(argGubun, argCode);
        }

        public BAS_BCODE GetAllByGubunCode1(string argGubun, string argCode)
        {
            return basBcodeRepository.GetAllByGubunCode1(argGubun, argCode);
        }

        public List<BAS_BCODE> GetCodeNamebyBCode(string strGubun, string strJobSabun = "")
        {
            return basBcodeRepository.GetCodeNamebyBCode(strGubun, strJobSabun);
        }

        public List<BAS_BCODE> GetItembyGubun(string strGubun)
        {
            return basBcodeRepository.GetItembyGubun(strGubun);
        }

        public BAS_GAMF Read_Gam_Opd(string JUMIN)
        {
            return basBcodeRepository.Read_Gam_Opd(JUMIN);
        }

        public List<BAS_BCODE> GetListByCodeIn(string argGubun, string[] strCodes)
        {
            return basBcodeRepository.GetListByCodeIn(argGubun, strCodes);
        }

        public List<BAS_BCODE> FindAllByGubun(string strGbn)
        {
            return basBcodeRepository.FindAllByGubun(strGbn);
        }

        public bool Save(IList<BAS_BCODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (BAS_BCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        basBcodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        basBcodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        basBcodeRepository.Delete(code);
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

        public List<BAS_BCODE> GetCodeNamebyBGubun(string strGubun)
        {
            return basBcodeRepository.GetCodeNamebyBGubun(strGubun);
        }

        public List<BAS_BCODE> GetListHicExamMstByGubun(string argGbn, string argSex, int argAge, string argJepDate)
        {
            return basBcodeRepository.GetListHicExamMstByGubun(argGbn, argSex, argAge, argJepDate);
        }
    }
}
