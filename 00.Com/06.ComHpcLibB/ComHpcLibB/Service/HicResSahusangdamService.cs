namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class HicResSahusangdamService
    {
        
        private HicResSahusangdamRepository hicResSahusangdamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResSahusangdamService()
        {
			this.hicResSahusangdamRepository = new HicResSahusangdamRepository();
        }

        public int GetCountbyWrtNo(long wRTNO)
        {
            return hicResSahusangdamRepository.GetCountbyWrtNo(wRTNO);
        }

        public int DeletebyRowId(string fstrROWID)
        {
            return hicResSahusangdamRepository.DeletebyRowId(fstrROWID);
        }

        public int Insert(long fnWRTNO, string fstrJepDate, string strSDate, string idNumber, string fstrSogen, string fstrPanjengGbn, string strGbn, string strRemark)
        {
            return hicResSahusangdamRepository.Insert(fnWRTNO, fstrJepDate, strSDate, idNumber, fstrSogen, fstrPanjengGbn, strGbn, strRemark);
        }

        public int Update(string fstrJepDate, string strSDate, string fstrSogen, string fstrPanjengGbn, string strGbn, string argSabun, string strRemark, string fstrROWID)
        {
            return hicResSahusangdamRepository.Update(fstrJepDate, strSDate, fstrSogen, fstrPanjengGbn, strGbn, argSabun, strRemark, fstrROWID);
        }

        public HIC_RES_SAHUSANGDAM GetItembyWrtNo(long fnWRTNO)
        {
            return hicResSahusangdamRepository.GetItembyWrtNo(fnWRTNO);
        }

        public long GetWrtNobyWrtNo(long nWrtNo)
        {
            return hicResSahusangdamRepository.GetWrtNobyWrtNo(nWrtNo);
        }
    }
}
