namespace HC.Core.Service
{
    using ComBase;
    using ComBase.Mvc.Spread;
    using HC.Core.Dto;
    using HC.Core.Repository;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 기초코드 서비스
    /// </summary>
    public class HcCodeService
    {
        
        private HcCodeRepository hcCodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcCodeService()
        {
			this.hcCodeRepository = new HcCodeRepository();
        }

        public DateTime CurrentDate
        {
            get
            {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return DateTime.ParseExact(clsPublic.GstrSysDate, "yyyy-MM-dd", null);
            }
        }
        /// <summary>
        /// 삭제여부 상관없이 모든 그룹코드를 가져옵니다
        /// </summary>
        /// <returns></returns>
        public List<HC_CODE> FindGroupCodeAll(string program)
        {
            List<HC_CODE> list = hcCodeRepository.FindGroupCodeAll(program);
            return list;
        }
        /// <summary>
        /// 삭제여부와 관계없이 그룹코드에 해당하는 기초코드 목록을 가져옵니다
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindAllByGroupCode(string groupCode, string program)
        {
            return hcCodeRepository.FindCodeByGroupCode(groupCode, program);
        }

        /// <summary>
        ///삭제되지않은 코드목록 가져오기
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindActiveCodeByGroupCode(string groupCode, string program)
        {
            return hcCodeRepository.FindActiveCodeByGroupCode(groupCode, program);
        }
        /// <summary>
        /// 그룹, 코드로 기초코드 가져오기
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public HC_CODE FindActiveCodeByGroupAndCode(string groupCode, string code, string program)
        {
            return hcCodeRepository.FindActiveCodeByGroupAndCode(groupCode, code, program);
        }

        public HC_CODE FindActiveCodeByGroupAndCodeAndCodeName(string groupCode, string code, string codeName, string program)
        {
            return hcCodeRepository.FindActiveCodeByGroupAndCodeAndCodeName(groupCode, code, codeName, program);
        }
        public bool SaveGroupCode(IList<HC_CODE> list, string program)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_CODE code in list)
                {
                   
                    code.GroupCode = "GROUP";
                    code.Program = program;
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hcCodeRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcCodeRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcCodeRepository.Delete(code);
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

        public List<HC_CODE> GetComboBoxData(string argGrpCD, string argPgrCD)
        {
            return hcCodeRepository.GetComboBoxData(argGrpCD, argPgrCD);
        }

        public int SaveCode(string groupCode, string program, IList<HC_CODE> list)
        {
            try
            {
                int result = 0;
                HcCodeService codeService = new HcCodeService();
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_CODE code in list)
                {
                    code.GroupCode = groupCode;
               
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        code.Program = program;
                        if (codeService.FindActiveCodeByGroupAndCode(code.GroupCode, code.Code, code.Program) == null)
                        {
                            hcCodeRepository.Insert(code);
                        }
                        else
                        {
                            result = 1;
                        }
                      
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        if (codeService.FindActiveCodeByGroupAndCodeAndCodeName(code.GroupCode, code.Code, code.CodeName, code.Program) == null)
                        {
                            hcCodeRepository.Update(code);
                        }
                        else
                        {
                            result = 1;
                        }
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcCodeRepository.Delete(code);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return 2;
            }
        }
        public void Delete(HC_CODE code)
        {
            hcCodeRepository.Delete(code);
        }
        /// <summary>
        /// 공통코드 콤보박스 데이타 가져오기
        /// 스프레드에서만 사용
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public SpreadComboBoxData GetSpreadComboBoxData(string groupCode, string program)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HC_CODE> list = hcCodeRepository.FindActiveCodeByGroupCode(groupCode, program);
            
            foreach(HC_CODE code in list)
            {
                data.Put(code.Code, code.CodeName);
            }
            return data;
        }

    }
}
