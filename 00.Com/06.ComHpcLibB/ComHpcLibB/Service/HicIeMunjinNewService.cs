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
    public class HicIeMunjinNewService
    {
        
        private HicIeMunjinNewRepository hicIeMunjinNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicIeMunjinNewService()
        {
			this.hicIeMunjinNewRepository = new HicIeMunjinNewRepository();
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtNoJepDateGjJong(string fstrPtno, string strFrDate, string strToDate, string fstrGjJong)
        {
            return hicIeMunjinNewRepository.GetItembyPtNoJepDateGjJong(fstrPtno, strFrDate, strToDate, fstrGjJong);
        }

        public int GetCountbyWrtNo(long fnWRTNO)
        {
            return hicIeMunjinNewRepository.GetCountbyWrtNo(fnWRTNO);
        }

        public int GetCountbyJumin(string strJumin, string strJepDate)
        {
            return hicIeMunjinNewRepository.GetCountbyJumin(strJumin, strJepDate);
        }

        public int GetwebHtmlbyWrtNo(long fnWRTNO)
        {
            return hicIeMunjinNewRepository.GetwebHtmlbyWrtNo(fnWRTNO);
        }

        public HIC_IE_MUNJIN_NEW GetMunjinResbyWrtNo1(long argWrtNo)
        {
            return hicIeMunjinNewRepository.GetMunjinResbyWrtNo1(argWrtNo);
        }

        public string GetRecvFormbyWrtNo(long iEMUNNO)
        {
            return hicIeMunjinNewRepository.GetRecvFormbyWrtNo(iEMUNNO);
        }

        public HIC_IE_MUNJIN_NEW GetCountbyPtNoMunDate(string fstrPtno, string gstrSysDate, string fstrJepDate)
        {
            return hicIeMunjinNewRepository.GetCountbyPtNoMunDate(fstrPtno, gstrSysDate, fstrJepDate);
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtnoMundate(string strPtno, string strMundate)
        {
            return hicIeMunjinNewRepository.GetItembyPtnoMundate(strPtno, strMundate);
        }

        public HIC_IE_MUNJIN_NEW GetItembyRowId(string strROWID)
        {
            return hicIeMunjinNewRepository.GetItembyRowId(strROWID);
        }

        public List<HIC_IE_MUNJIN_NEW> GetItembyWrtNoMunDate(long fnWrtNo, string strFrDate, string strToDate, string fstrPtno, string strSname, string strLtdCode, string fstrGjJong)
        {
            return hicIeMunjinNewRepository.GetItembyWrtNoMunDate(fnWrtNo, strFrDate, strToDate, fstrPtno, strSname, strLtdCode, fstrGjJong);
        }

        public int UpdatePtNobyRowId(string strPtNo, string rOWID)
        {
            return hicIeMunjinNewRepository.UpdatePtNobyRowId(strPtNo, rOWID);
        }

        public HIC_IE_MUNJIN_NEW GetWrtNoRecvFormbyRowId(string strROWID)
        {
            return hicIeMunjinNewRepository.GetWrtNoRecvFormbyRowId(strROWID);
        }

        public string GetMunDatebyPtNo(string fstrPtno)
        {
            return hicIeMunjinNewRepository.GetMunDatebyPtNo(fstrPtno);
        }

        public bool UpDateReset(long argIEMunno)
        {
            try
            {
                hicIeMunjinNewRepository.UpDateReset(argIEMunno);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtnoMundate2(string argPtno, string strDate)
        {
            return hicIeMunjinNewRepository.GetItembyPtnoMundate2(argPtno, strDate);
        }

        public HIC_IE_MUNJIN_NEW GetRecvFormbyMunDatePtNo(string strMunDate, string pTNO)
        {
            return hicIeMunjinNewRepository.GetRecvFormbyMunDatePtNo(strMunDate, pTNO);
        }

        public HIC_IE_MUNJIN_NEW GetItembyWrtNoMunDateLike(long iEMUNNO, string argMunDate)
        {
            return hicIeMunjinNewRepository.GetItembyWrtNoMunDateLike(iEMUNNO, argMunDate);
        }

        public HIC_IE_MUNJIN_NEW GetItembySNamePtnoMunDate(string sNAME, string pTNO, string jEPDATE)
        {
            return hicIeMunjinNewRepository.GetItembySNamePtnoMunDate(sNAME, pTNO, jEPDATE);
        }

        public HIC_IE_MUNJIN_NEW GetItembyPtNoMunDate(string pTNO, string strDate, string argJob = "")
        {
            return hicIeMunjinNewRepository.GetItembyPtNoMunDate(pTNO, strDate, argJob);
        }

        public string GetResvFormByWrtno(long wRTNO)
        {
            return hicIeMunjinNewRepository.GetResvFormByWrtno(wRTNO);
        }

        public int GetCountbyPtNo(string pTNO)
        {
            return hicIeMunjinNewRepository.GetCountbyPtNo(pTNO);
        }

        public int GetCountbyPtNoGjJong(string pTNO, string strGjJong)
        {
            return hicIeMunjinNewRepository.GetCountbyPtNoGjJong(pTNO, strGjJong);
        }

        public string GetResvFormByPtno(string argPtno, string argDate)
        {
            return hicIeMunjinNewRepository.GetResvFormByPtno(argPtno, argDate);
        }

        public HIC_IE_MUNJIN_NEW GetMunjinResbyWrtNo2(long nWRTNO)
        {
            return hicIeMunjinNewRepository.GetMunjinResbyWrtNo2(nWRTNO);
        }
    }
}
