namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicCharttransService
    {
        
        private HicCharttransRepository hicCharttransRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicCharttransService()
        {
			this.hicCharttransRepository = new HicCharttransRepository();
        }
        public List<HIC_CHARTTRANS> GetAllbyTrDate(string strTRDAE)
        {
            return hicCharttransRepository.GetAllbyTrDate(strTRDAE);
        }

        public HIC_CHARTTRANS GetAllbyWrtno(long nWRTNO)
        {
            return hicCharttransRepository.GetAllbyWrtno(nWRTNO);
        }

        public int Insert(HIC_CHARTTRANS item)
        {
            return hicCharttransRepository.Insert(item);
        }
        public int UpdatebyWrtno(long nWRTNO, string strTRDATE, string strTRLIST, string strENTTIME, string strENTSABUN, string strREMARK)
        {
            return hicCharttransRepository.UpdatebyWrtno(nWRTNO, strTRDATE, strTRLIST, strENTTIME, strENTSABUN, strREMARK);
        }

        public int DeleteData(long nWRTNO)
        {
            return hicCharttransRepository.DeleteData(nWRTNO);
        }

        public int InsertbySelect(long fnWRTNO)
        {
            return hicCharttransRepository.InsertbySelect(fnWRTNO);
        }

        public int UpdaerecvSabunRecvTimebyWrtNo(long fnWRTNO)
        {
            return hicCharttransRepository.UpdaerecvSabunRecvTimebyWrtNo(fnWRTNO);
        }

        public List<HIC_CHARTTRANS> GetItembySysDate()
        {
            return hicCharttransRepository.GetItembySysDate();
        }

        public HIC_CHARTTRANS GetItembyWrtNo(long fnWrtNo)
        {
            return hicCharttransRepository.GetItembyWrtNo(fnWrtNo);
        }

        public int UpdaterevbTimeRecvSabunbyWrtno(long fnWrtNo, string idNumber)
        {
            return hicCharttransRepository.UpdaterevbTimeRecvSabunbyWrtno(fnWrtNo, idNumber);
        }

        public int DeletebyRowId(string strROWID)
        {
            return hicCharttransRepository.DeletebyRowId(strROWID);
        }

        public int UpdatebyRowId(string strROWID, string strGubun)
        {
            return hicCharttransRepository.UpdatebyRowId(strROWID, strGubun);
        }

        public int InsertAll(HIC_CHARTTRANS item)
        {
            return hicCharttransRepository.InsertAll(item);
        }

        public string GetRowIdbyWrtNo(long nWRTNO)
        {
            return hicCharttransRepository.GetRowIdbyWrtNo(nWRTNO);
        }

        public List<HIC_CHARTTRANS> GetItembyTrDate(string strFrDate, string strToDate, string strSname, long nWRTNO, string strOut, string strAmPm, string strNoTrans, string strJob)
        {
            return hicCharttransRepository.GetItembyTrDate(strFrDate, strToDate, strSname, nWRTNO, strOut, strAmPm, strNoTrans, strJob);
        }
    }
}
