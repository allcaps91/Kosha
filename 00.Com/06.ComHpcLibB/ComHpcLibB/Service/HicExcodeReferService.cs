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
    public class HicExcodeReferService
    {
        
        private HicExcodeReferRepository hicExcodeReferRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicExcodeReferService()
        {
			this.hicExcodeReferRepository = new HicExcodeReferRepository();
        }

        public List<HIC_EXCODE_REFER> GetItemByCode(string argCode, string argFDate, string argGbn)
        {
            return hicExcodeReferRepository.GetItemByCode(argCode, argFDate, argGbn);
        }

        public List<HIC_EXCODE_REFER> FindAll(string strResultType)
        {
            return hicExcodeReferRepository.FindAll(strResultType);
        }

        public bool SaveData(string cODE, IList<HIC_EXCODE_REFER> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HIC_EXCODE_REFER code in list)
                {
                    code.CODE = cODE;

                    //자료 저장하기 전 Clear -> 적용기간 날짜관리
                    //hicExcodeReferRepository.Delete(code);

                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicExcodeReferRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicExcodeReferRepository.Update(code);
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
