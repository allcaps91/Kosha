namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCancerResv2Service
    {
        
        private HicCancerResv2Repository hicCancerResv2Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCancerResv2Service()
        {
			this.hicCancerResv2Repository = new HicCancerResv2Repository();
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime(string strFDate, string strTDate, string strJong, string strName)
        {
            return hicCancerResv2Repository.GetItembyRTime(strFDate, strTDate, strJong, strName);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime2(string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyRTime2(strFDate, strTDate);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime3(string strAMPM, string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyRTime3(strAMPM, strFDate, strTDate);
        }

        public HIC_CANCER_RESV2 GetItembyJumin(string strJumin, string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyJumin(strJumin, strFDate, strTDate);
        }

        public int UpdateRTimeEntTimeEntSabunbyRowId(string strDelDate, string idNumber, string strRowId)
        {
            return hicCancerResv2Repository.UpdateRTimeEntTimeEntSabunbyRowId(strDelDate, idNumber, strRowId);
        }

        public List<HIC_CANCER_RESV2> GetItembyEntTime(string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyEntTime(strFDate, strTDate);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTimeHPhone(string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyRTimeHPhone(strFDate, strTDate);
        }

        public List<HIC_CANCER_RESV2> GetRTimebyJumin2RTime(string strJumin, string strSDate, string strEDate)
        {
            return hicCancerResv2Repository.GetRTimebyJumin2RTime(strJumin, strSDate, strEDate);
        }

        public HIC_CANCER_RESV2 GetItembyRowId(string strROWID)
        {
            return hicCancerResv2Repository.GetItembyRowId(strROWID);
        }

        public List<HIC_CANCER_RESV2> GetItembyLtdCodeRTime(string strLtdCode, string strFDate, string strTDate, string strYYMM)
        {
            return hicCancerResv2Repository.GetItembyLtdCodeRTime(strLtdCode, strFDate, strTDate, strYYMM);
        }

        public string GetRowIdbyJumin2RTime(string strJumin, string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetRowIdbyJumin2RTime(strJumin, strFDate, strTDate);
        }

        public int Insert(HIC_CANCER_RESV2 item)
        {
            return hicCancerResv2Repository.Insert(item);
        }

        public int Update(HIC_CANCER_RESV2 item)
        {
            return hicCancerResv2Repository.Update(item);
        }

        public int UpdateRemark(string strRemark, string strRowId)
        {
            return hicCancerResv2Repository.UpdateRemark(strRemark, strRowId);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime4(string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyRTime4(strFDate, strTDate);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime5(string strTempFDate, string strTempTDate, string strSort)
        {
            return hicCancerResv2Repository.GetItembyRTime5(strTempFDate, strTempTDate, strSort);
        }

        public List<HIC_CANCER_RESV2> GetItembyRTime6(string strAmPm, string strFDate, string strTDate)
        {
            return hicCancerResv2Repository.GetItembyRTime6(strAmPm, strFDate, strTDate);
        }

        public HIC_CANCER_RESV2 GetItemByPtnoRTime(string strPtno, string strDate)
        {
            return hicCancerResv2Repository.GetItemByPtnoRTime(strPtno, strDate);
        }

        public List<HIC_CANCER_RESV2> GetListByRTimeJumin(string argDate, string argJumin)
        {
            return hicCancerResv2Repository.GetListByRTimeJumin(argDate, argJumin);
        }
    }
}
