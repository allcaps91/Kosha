namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuExjongPatientService
    {
        
        private HicJepsuExjongPatientRepository hicJepsuExjongPatientRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuExjongPatientService()
        {
			this.hicJepsuExjongPatientRepository = new HicJepsuExjongPatientRepository();
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyJepDateLtdCode(string strFrDate, string strToDate, long nLtdCode, string strTong, string strJob)
        {
            return hicJepsuExjongPatientRepository.GetItembyJepDateLtdCode(strFrDate, strToDate, nLtdCode, strTong, strJob);
        }

        public string GetGbMunjinbyWrtNo(long argWrtNo)
        {
            return hicJepsuExjongPatientRepository.GetGbMunjinbyWrtNo(argWrtNo);
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyWrtNoJepDateGjJongLtdCode(string strFrDate, string strToDate, string strJong, long nLtdCode, List<long> str2ChaWrtno)
        {
            return hicJepsuExjongPatientRepository.GetItembyWrtNoJepDateGjJongLtdCode(strFrDate, strToDate, strJong, nLtdCode, str2ChaWrtno);
        }

        public HIC_JEPSU_EXJONG_PATIENT ReadJepMunjinSatus(long argWRTNO)
        {
            return hicJepsuExjongPatientRepository.ReadJepMunjinSatus(argWRTNO);
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyWrtNoJepDateJongLtdCode(string strFrDate, string strToDate, string strJob, string strJong, long nLtdCode)
        {
            return hicJepsuExjongPatientRepository.GetItembyWrtNoJepDateJongLtdCode(strFrDate, strToDate, strJob, strJong, nLtdCode);
        }
    }
}
