namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResultwardService
    {
        
        private HeaResultwardRepository heaResultwardRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResultwardService()
        {
			this.heaResultwardRepository = new HeaResultwardRepository();
        }

        public HEA_RESULTWARD GetWardNameByCode(string argGbn, long nSabun, string strGubun)
        {
            return heaResultwardRepository.GetWardNameByCode(argGbn, nSabun, strGubun);
        }

        public List<HEA_RESULTWARD> GetCodeNameBySabunGubun(string idNumber, string strGubun)
        {
            return heaResultwardRepository.GetCodeNameBySabunGubun(idNumber, strGubun);
        }

        public List<HEA_RESULTWARD> GetCodeNameBySabunGubunJong(string idNumber, string strGubun, string strJong)
        {
            return heaResultwardRepository.GetCodeNameBySabunGubunJong(idNumber, strGubun, strJong);
        }

        public int DeleteCode(string strROWID)
        {
            return heaResultwardRepository.DeleteCode(strROWID);
        }

        public int InsertCode(string idNumber, string strCODE, string strWard, string strGubun)
        {
            return heaResultwardRepository.InsertCode(idNumber, strCODE, strWard, strGubun);
        }

        public int UpdateWardNamebyRowId(string strWard, string strROWID)
        {
            return heaResultwardRepository.UpdateWardNamebyRowId(strWard, strROWID);
        }

        public List<HEA_RESULTWARD> GetItembySabuncodeGubun(string idNumber, string strCODE, string strGubun, string strStep = "", string strKeyWord = "")
        {
            return heaResultwardRepository.GetItembySabuncodeGubun(idNumber, strCODE, strGubun, strStep, strKeyWord);
        }

        public List<HEA_RESULTWARD> GetItembySabunCodeStep(long fnJobSabun, string strSts, string strGubun, string strStep)
        {
            return heaResultwardRepository.GetItembySabunCodeStep(fnJobSabun, strSts, strGubun, strStep);
        }

        public int DeletebyRowId(string strROWID)
        {
            return heaResultwardRepository.DeletebyRowId(strROWID);
        }

        public int InsertItem(string idNumber, int nSeqNo, string strCode, string strStep, string strGubun, string strWard)
        {
            return heaResultwardRepository.InsertItem(idNumber, nSeqNo, strCode, strStep, strGubun, strWard);
        }

        public int UpdateWardNameStepSeqNobyRowId(string strROWID, string strWard, string strStep, int nSeqNo)
        {
            return heaResultwardRepository.UpdateWardNameStepSeqNobyRowId(strROWID, strWard, strStep, nSeqNo);
        }
    }
}
