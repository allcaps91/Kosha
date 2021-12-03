namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuWorkPatientHeaJepsuService
    {
        
        private HicJepsuWorkPatientHeaJepsuRepository hicJepsuWorkPatientHeaJepsuRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuWorkPatientHeaJepsuService()
        {
			this.hicJepsuWorkPatientHeaJepsuRepository = new HicJepsuWorkPatientHeaJepsuRepository();
        }

        public List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> GetItembyGjJong(string strGjJong)
        {
            return hicJepsuWorkPatientHeaJepsuRepository.GetItembyGjJong(strGjJong);
        }

        public List<HIC_JEPSU_WORK_PATIENT_HEA_JEPSU> GetItembyLtdCodeSNameGjJong(string strLtdCode, string strSname, string strGjJong)
        {
            return hicJepsuWorkPatientHeaJepsuRepository.GetItembyLtdCodeSNameGjJong(strLtdCode, strSname, strGjJong);
        }

        public int GetPaNoPtNobyJumin2GjYear(string strJumin, string strGjYear)
        {
            return hicJepsuWorkPatientHeaJepsuRepository.GetPaNoPtNobyJumin2GjYear(strJumin, strGjYear);
        }

        public int GetCountbyJuminGjYearGjJong(string strJumin, string strGjYear, string strGjJong)
        {
            return hicJepsuWorkPatientHeaJepsuRepository.GetCountbyJuminGjYearGjJong(strJumin, strGjYear, strGjJong);
        }
    }
}
