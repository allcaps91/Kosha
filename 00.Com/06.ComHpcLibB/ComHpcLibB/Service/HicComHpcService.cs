namespace ComHpcLibB.Service
{
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public class HicComHpcService
    {

        private HicComHpcRepository hicComHpcRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicComHpcService()
        {
            this.hicComHpcRepository = new HicComHpcRepository();
        }

        public List<COMHPC> GetStatistics(string strSdate, string strEdate)
        {
            return hicComHpcRepository.GetStatistics(strSdate, strEdate);
        }

        public List<COMHPC> GetDepartment(string strFDate, string strTDate)
        {
            return hicComHpcRepository.GetDepartment(strFDate, strTDate);
        }

        public List<COMHPC> GetMonthData(string strFDate, string strTDate)
        {
            return hicComHpcRepository.GetMonthData(strFDate, strTDate);
        }

        public List<COMHPC> GetIndividual_A(string strFDate, string strTDate)
        {
            return hicComHpcRepository.GetIndividual_A(strFDate, strTDate);
        }
        public List<COMHPC> GetIndividual_B(string strFDate, string strTDate)
        {
            return hicComHpcRepository.GetIndividual_B(strFDate, strTDate);
        }

        public List<COMHPC> GetItemByDate(string argFDate, string argTDate)
        {
            return hicComHpcRepository.GetItemByDate(argFDate, argTDate);
        }

        public List<COMHPC> GetBogunListData(string fstrFDate, string fstrTDate, string strBogenso, string TxtKiho, long nMirNo, bool ChkW_Am, bool ChkGub, bool ChkBogen)
        {
            return hicComHpcRepository.GetBogunListData(fstrFDate, fstrTDate, strBogenso, TxtKiho, nMirNo, ChkW_Am, ChkGub, ChkBogen);
        }

        public List<COMHPC> GetGongDanListData(string fstrCOLNM, long nMirNo, string fstrLtdCode, string fstrJong)
        {
            return hicComHpcRepository.GetGongDanListData(fstrCOLNM, nMirNo, fstrLtdCode, fstrJong);
        }

        public List<COMHPC> GetBillNo()
        {
            return hicComHpcRepository.GetBillNo();
        }
    }
}
