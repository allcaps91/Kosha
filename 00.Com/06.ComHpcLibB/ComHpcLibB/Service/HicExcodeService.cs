namespace ComHpcLibB.Service
{
    using ComBase;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicExcodeService
    {
        
        private HicExcodeRepository hicExcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicExcodeService()
        {
			this.hicExcodeRepository = new HicExcodeRepository();
        }

        public List<HIC_EXCODE> FindAll(bool fbDel, bool fbSpc, bool fbSend, string fstrKey, string fstrGbsuga = "")
        {
            return hicExcodeRepository.FindAll(fbDel, fbSpc, fbSend, fstrKey, fstrGbsuga);
        }

        public int Insert(HIC_EXCODE list)
        {
            return hicExcodeRepository.Insert(list);
        }

        public int Update(HIC_EXCODE list)
        {
            return hicExcodeRepository.Update(list);
        }

        public int UpdateAmt(HIC_EXCODE list)
        {
            return hicExcodeRepository.UpdateAmt(list);

        }
        public int Delete(HIC_EXCODE list)
        {
            return hicExcodeRepository.Delete(list);
        }

        public HIC_EXCODE Read_HaRoom(string HEAPART)
        {
            return hicExcodeRepository.Read_HaRoom(HEAPART);
        }

        public HIC_EXCODE FindOne(string fCode)
        {
            return hicExcodeRepository.FindOne(fCode);
        }

        public List<HIC_EXCODE> GetItembyFind(string strTemp, string strGubun)
        {
            return hicExcodeRepository.GetItembyFind(strTemp, strGubun);
        }

        public string Read_XrayCode(string argCode)
        {
            return hicExcodeRepository.Read_XrayCode(argCode);
        }

        public List<HIC_EXCODE> GetReferencebyCodeList(List<string> rEFER_CHANGE_CODELIST)
        {
            return hicExcodeRepository.GetReferencebyCodeList(rEFER_CHANGE_CODELIST);
        }

        public string GetResultTypebyCode(string strCODE)
        {
            return hicExcodeRepository.GetResultTypebyCode(strCODE);
        }

        public List<HIC_EXCODE> GetCodeHNamebyPartHname(string strPart, string strName)
        {
            return hicExcodeRepository.GetCodeHNamebyPartHname(strPart, strName);
        }

        public List<HIC_EXCODE> GetCodeHNamebyPartHName(string strGubun, string strView)
        {
            return hicExcodeRepository.GetCodeHNamebyPartHName(strGubun, strView);
        }

        public List<HIC_EXCODE> GetCodeAll()
        {
            return hicExcodeRepository.GetCodeAll();
        }

        public List<HIC_EXCODE> GetCodeEndoScope()
        {
            return hicExcodeRepository.GetCodeEndoScope();
        }

        public List<HIC_EXCODE> GetEntPartbyWrtNo(long nWRTNO)
        {
            return hicExcodeRepository.GetEntPartbyWrtNo(nWRTNO);
        }

        public List<HIC_EXCODE> GetListByExCode(string argExamCode)
        {
            return hicExcodeRepository.GetListByExCode(argExamCode);
        }

        public string GetResCodebyCode(string strHicCode)
        {
            return hicExcodeRepository.GetResCodebyCode(strHicCode);
        }

        public string GetHNmaebyCode(string argCode)
        {
            return hicExcodeRepository.GetHNmaebyCode(argCode);
        }

        public List<HIC_EXCODE> GetCodebyEntPart(string strPart)
        {
            return hicExcodeRepository.GetCodebyEntPart(strPart);
        }

        public List<HIC_EXCODE> GetCodebyPart(string argPart = "")
        {
            return hicExcodeRepository.GetCodebyPart(argPart);
        }

        public HIC_EXCODE GetHNameYNamebyCode(string strExCode)
        {
            return hicExcodeRepository.GetHNameYNamebyCode(strExCode);
        }

        public bool Save(IList<HIC_EXCODE> list)
        {

            string strOK = string.Empty;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HIC_EXCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicExcodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicExcodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hicExcodeRepository.Delete(code);
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

        public int UpDateGbSuga(string argExCode, string argGbSuga)
        {
            return hicExcodeRepository.UpDateGbSuga(argExCode, argGbSuga);
        }

        public HIC_EXCODE GetCodeEndoGubun3byExCode(string strExCode)
        {
            return hicExcodeRepository.GetCodeEndoGubun3byExCode(strExCode);
        }

        public List<HIC_EXCODE> GetCodebyHeaSort(string argCode = "")
        {
            return hicExcodeRepository.GetCodebyHeaSort(argCode);
        }

        public List<HIC_EXCODE> GetEndoGbnCodeList()
        {
            return hicExcodeRepository.GetEndoGbnCodeList();
        }

        public HIC_EXCODE GetEndoGubunbyCode(string argCode)
        {
            return hicExcodeRepository.GetEndoGubunbyCode(argCode);
        }

        public HIC_EXCODE GetGotobyCode(string argCode)
        {
            return hicExcodeRepository.GetGotobyCode(argCode);
        }

        public HIC_EXCODE Read_HcRoom(string strEntPart)
        {
            return hicExcodeRepository.Read_HcRoom(strEntPart);
        }

        public string GetRoombyHeaPart(string strEntPart)
        {
            return hicExcodeRepository.GetRoombyHeaPart(strEntPart);
        }

        public bool SelectInsertHicExCodeByRid(string fstrRowid)
        {
            try
            {
                hicExcodeRepository.SelectInsertHicExCodeByRid(fstrRowid);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public List<HIC_EXCODE> GetHistoryByCode(string argCode)
        {
            return hicExcodeRepository.GetHistoryByCode(argCode);
        }

        public List<HIC_EXCODE> GetSugaListByChkSuga(string strChkSuga, string strSuCode, string strExamCode, string strSuDateCode, bool bDel, string argSearch = "")
        {
            return hicExcodeRepository.GetSugaListByChkSuga(strChkSuga, strSuCode, strExamCode, strSuDateCode, bDel, argSearch);
        }
    }
}
