namespace HC.Core.BaseCode.Management.Service
{
    using System.Collections.Generic;
    using HC.Core.BaseCode.Management.Repository;
    using HC.Core.BaseCode.Management.Dto;
    using ComBase.Mvc.Spread;
    using ComBase;
    using System;


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
        /// <summary>
        /// 삭제여부 상관없이 모든 그룹코드를 가져옵니다
        /// </summary>
        /// <returns></returns>
        public List<HC_CODE> FindGroupCodeAll()
        {
            return hcCodeRepository.FindGroupCodeAll();
        }
        /// <summary>
        /// 삭제여부와 관계없이 그룹코드에 해당하는 기초코드 목록을 가져옵니다
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindAllByGroupCode(string groupCode)
        {
            return hcCodeRepository.FindCodeByGroupCode(groupCode);
        }

        /// <summary>
        ///삭제되지않은 코드목록 가져오기
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<HC_CODE> FindActiveCodeByGroupCode(string groupCode)
        {
            return hcCodeRepository.FindActiveCodeByGroupCode(groupCode);
        }


        public bool SaveGroupCode(IList<HC_CODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_CODE code in list)
                {
                   
                    code.GroupCode = "GROUP";
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
        public bool SaveCode(string groupCode, IList<HC_CODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HC_CODE code in list)
                {
                    code.GroupCode = groupCode;
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
        public SpreadComboBoxData GetSpreadComboBoxData(string groupCode)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HC_CODE> list = hcCodeRepository.FindActiveCodeByGroupCode(groupCode);
            
            foreach(HC_CODE code in list)
            {
                data.Put(code.Code, code.CodeName);
            }
            return data;
        }

    }
}
