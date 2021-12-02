namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultwardService
    {
        
        private HicResultwardRepository hicResultwardRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResultwardService()
        {
			this.hicResultwardRepository = new HicResultwardRepository();
        }
        
        public string Read_WardName(string strCode)
        {
            return hicResultwardRepository.Read_WardName(strCode);
        }

        public List<HIC_RESULTWARD> GetItembyGubun(string fstrGubun)
        {
            return hicResultwardRepository.GetItembyGubun(fstrGubun);
        }

        public int DeletebyRowId(string strROWID)
        {
            return hicResultwardRepository.DeletebyRowId(strROWID);
        }

        public int Insert(string idNumber, string strCODE, string strName, string fstrGbn)
        {
            return hicResultwardRepository.Insert(idNumber, strCODE, strName, fstrGbn);
        }

        public int Update(string strROWID, string strCODE, string strName)
        {
            return hicResultwardRepository.Update(strROWID, strCODE, strName);
        }

        public List<HIC_RESULTWARD> GetItembyGubunExamWardGubun2(string fstrGubun, string fstrExam, string strGubun)
        {
            return hicResultwardRepository.GetItembyGubunExamWardGubun2(fstrGubun, fstrExam, strGubun);
        }
    }
}
