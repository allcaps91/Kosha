namespace ComHpcLibB.Service
{
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMstSlipService
    {
        
        private HicMisuMstSlipRepository hicMisuMstSlipRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMstSlipService()
        {
			this.hicMisuMstSlipRepository = new HicMisuMstSlipRepository();
        }

        public List<HIC_MISU_MST_SLIP> GetMisuCashSum(string argFDate, string argTDate)
        {
            return hicMisuMstSlipRepository.GetMisuCashSum(argFDate, argTDate);

        }

        public List<HIC_MISU_MST_SLIP> GetMisuMaster(long txtWrtNo)
        {
            return hicMisuMstSlipRepository.GetMisuMaster(txtWrtNo);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuSlip(long txtWrtNo)
        {
            return hicMisuMstSlipRepository.GetMisuSlip(txtWrtNo);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuDate(long txtWrtNo)
        {
            return hicMisuMstSlipRepository.GetMisuDate(txtWrtNo);
        }

        public int GetinsertDelHistory(string strROWID, long GnJobSabun)
        {
            return hicMisuMstSlipRepository.GetinsertDelHistory(strROWID, GnJobSabun);
        }

        public int GetDelHistory(string strROWID)
        {
            return hicMisuMstSlipRepository.GetDelHistory(strROWID);
        }

        public int HistoryIn(string strROWID, long GnJobSabun)
        {
            return hicMisuMstSlipRepository.HistoryIn(strROWID, GnJobSabun);
        }

        public int HistoryUpdate(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.HistoryUpdate(item);
        }

        public int HistoryAfter(string strROWID, long GnJobSabun)
        {
            return hicMisuMstSlipRepository.HistoryAfter(strROWID, GnJobSabun);
        }

        public int HistoryInsert(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.HistoryInsert(item);
        }

        public int HistoryInsert_New(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.HistoryInsert_New(item);
        }

        public int LtdCodeUpdate(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.LtdCodeUpdate(item);
        }

        public int GetMisuMaster_Update(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.GetMisuMaster_Update(item);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuCheck(long nWrtno)
        {
            return hicMisuMstSlipRepository.GetMisuCheck(nWrtno);
        }

        public int GetMisuSlipUpdate(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.GetMisuSlipUpdate(item);
        }

        public int insertNew(HIC_MISU_MST_SLIP hmm, long nWrtno, string TxtDate, long fnMisuAmt, long gnJobSabun)
        {
            return hicMisuMstSlipRepository.insertNew(hmm, nWrtno, TxtDate, fnMisuAmt, gnJobSabun);
        }

        public int insertOld(HIC_MISU_MST_SLIP item, long gnJobSabun, string strROWID)
        {
            return hicMisuMstSlipRepository.insertOld(item, strROWID, gnJobSabun);
        }

        public int DelListHistory(HIC_MISU_MST_SLIP item, long nWrtno, long GnJobSabun)
        {
            return hicMisuMstSlipRepository.DelListHistory(item, nWrtno, GnJobSabun);
        }

        public int MisuUpdate(HIC_MISU_MST_SLIP item2, long gnJobSabun, string strROWID, string TxtDate, double TxtLtdCode, string TxtRemark, long FnMisuAmt)
        {
            return hicMisuMstSlipRepository.MisuUpdate(item2, gnJobSabun, strROWID, TxtDate, TxtLtdCode, TxtRemark, FnMisuAmt);
        }

        public List<HIC_MISU_MST_SLIP> GetJisaName(string strFdate, string strTdate, string strJong, string strDtlJong, string strJisaCode, bool rdoJob1, bool rdoJob2, string strView, bool rdoSort1)
        {
            return hicMisuMstSlipRepository.GetJisaName(strFdate, strTdate, strJong, strDtlJong, strJisaCode, rdoJob1, rdoJob2, strView, rdoSort1);
        }

        public int DelMisuMaster(HIC_MISU_MST_SLIP item, long nWrtno)
        {
            return hicMisuMstSlipRepository.DelMisuMaster(item, nWrtno);
        }

        public int DelMisuSlip(HIC_MISU_MST_SLIP item, long nWrtno)
        {
            return hicMisuMstSlipRepository.DelMisuSlip(item, nWrtno);
        }

        public List<HIC_MISU_MST_SLIP> Getitem(string strFdate, string strTdate, string strJong, string strDtlJong, string strLtdCode, bool rdoJob1, bool rdoJob2, long TxtMisuAmt, string CboName, bool rdoSort, long LtdCode)
        {
            return hicMisuMstSlipRepository.Getitem(strFdate, strTdate, strJong, strDtlJong, strLtdCode, rdoJob1, rdoJob2, TxtMisuAmt, CboName, rdoSort, LtdCode);
        }

        public int MisuUpdate_New(HIC_MISU_MST_SLIP item3, long gnJobSabun, string strROWID)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
            return hicMisuMstSlipRepository.MisuUpdate_New(item3, strROWID, gnJobSabun);
        }
                      
        public int GetMisuMaster_Update_1(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.GetMisuMaster_Update_1(item);
        }

        public int GetMisuMaster_Update_2(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.GetMisuMaster_Update_2(item);
        }

        public List<HIC_MISU_MST_SLIP> GetchunguNum(string strInput)
        {
            return hicMisuMstSlipRepository.GetchunguNum(strInput);
        }

        public int MisuMasterNew(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.MisuMasterNew(item);
        }

        public int MisuSlipNew(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.MisuSlipNew(item);
        }

        public int NewHistoryInsert(long gnJobSabun, long nMisuNo)
        {
            return hicMisuMstSlipRepository.NewHistoryInsert(gnJobSabun, nMisuNo);
        }

        public List<HIC_MISU_MST_SLIP> getMisuNum(string strltdcode, string strJong, string strTDate, double nAmt)
        {
            return hicMisuMstSlipRepository.getMisuNum(strltdcode, strJong ,strTDate, nAmt);
        }

        public int UpdateRowid(string strROWID)
        {
            return hicMisuMstSlipRepository.UpdateRowid(strROWID);
        }

        public int GongDanMisuMasterNew(HIC_MISU_MST_SLIP item, string fstrJong)
        {
            return hicMisuMstSlipRepository.GongDanMisuMasterNew(item, fstrJong);
        }

        public int GongDanMisuSlipNew(HIC_MISU_MST_SLIP item)
        {
            return hicMisuMstSlipRepository.GongDanMisuSlipNew(item);
        }

        public int GongDanNewHistoryInsert(long gnJobSabun, long nMisuNo)
        {
            return hicMisuMstSlipRepository.GongDanNewHistoryInsert(gnJobSabun, nMisuNo);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuMaster2(long nWrtno)
        {
            return hicMisuMstSlipRepository.GetMisuMaster2(nWrtno);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuSlipDisplay(long nWrtno)
        {
            return hicMisuMstSlipRepository.GetMisuSlipDisplay(nWrtno);
        }
    }
}
