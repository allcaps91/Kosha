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
    public class HicMcodeService
    {

        private HicMcodeRepository hicMcodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicMcodeService()
        {
            this.hicMcodeRepository = new HicMcodeRepository();
        }

        public IList<HIC_MCODE> SelMCode_Many(string argSql)
        {
            return hicMcodeRepository.SelMCode_Many(argSql);
        }

        public string Read_MCode_Name(string argCode)
        {
            return hicMcodeRepository.Read_MCode_Name(argCode);
        }

        public string Read_UCode(string argCode)
        {
            return hicMcodeRepository.Read_UCode(argCode);
        }

        public string Read_MCode_Jugi(string argCode)
        {
            return hicMcodeRepository.Read_JUGI(argCode);
        }

        public string Read_GbDent(string argMCode)
        {
            return hicMcodeRepository.Read_GbDent(argMCode);
        }

        public List<HIC_MCODE> FindAll()
        {
            return hicMcodeRepository.FindAll();
        }

        public List<HIC_MCODE> GetCodeNamebyCode(List<string> strUSQL)
        {
            return hicMcodeRepository.GetCodeNamebyCode(strUSQL);
        }

        public List<HIC_MCODE> GetCodeNamebyNotInCode(List<string> strUSQL)
        {
            return hicMcodeRepository.GetCodeNamebyNotInCode(strUSQL);
        }

        public bool Save(IList<HIC_MCODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HIC_MCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicMcodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicMcodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hicMcodeRepository.Delete(code);
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

        public List<HIC_MCODE> GetCodeNamebyNotInNight(List<string> strCode)
        {
            return hicMcodeRepository.GetCodeNamebyNotInNight(strCode);
        }

        public HIC_MCODE GetItemByCode(string strCode)
        {
            return hicMcodeRepository.GetItemByCode(strCode);
        }

        public int UpDateSpcExamInfoByCode(HIC_MCODE item)
        {
            return hicMcodeRepository.UpDateSpcExamInfoByCode(item);
        }

        public List<HIC_MCODE> GetTongBunCountbyCode(List<string> argSQL)
        {
            return hicMcodeRepository.GetTongBunCountbyCode(argSQL);
        }

        public int GetTongBunbyCode(string strUCodes)
        {
            return hicMcodeRepository.GetTongBunbyCode(strUCodes);
        }

        public List<HIC_MCODE> GetItemByLikeName(string strName = "")
        {
            return hicMcodeRepository.GetItemByLikeName(strName);
        }

        public string GetNameByCode(string argCode)
        {
            return hicMcodeRepository.GetNameByCode(argCode);
        }

        public List<HIC_MCODE> GetCodeListByCodeIn(List<string> strTemp1)
        {
            return hicMcodeRepository.GetCodeListByCodeIn(strTemp1);
        }
        public List<HIC_MCODE> GetCodeListByCodeNotIn(List<string> strTemp1)
        {
            return hicMcodeRepository.GetCodeListByCodeNotIn(strTemp1);
        }
        public string GetUCodebyCode(string fstrMCode)
        {
            return hicMcodeRepository.GetUCodebyCode(fstrMCode);
        }

        public List<HIC_MCODE> GetListByCodeName(string strGubun, string strName)
        {
            return hicMcodeRepository.GetListByCodeName(strGubun, strName);
        }
    }
}
