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
    public class HicXrayResultService
    {
        
        private HicXrayResultRepository hicXrayResultRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicXrayResultService()
        {
			this.hicXrayResultRepository = new HicXrayResultRepository();
        }

        public HIC_XRAY_RESULT GetItemByPtnoJepDate(string argPTNO, string argJepDate1, string argJepDate2)
        {
            return hicXrayResultRepository.GetItemByPtnoJepDate(argPTNO, argJepDate1, argJepDate2);
        }

        public List<HIC_XRAY_RESULT> GetItemByPtnoJepDateList(string argPTNO, string argJepDate1, string argJepDate2)
        {
            return hicXrayResultRepository.GetItemByPtnoJepDateList(argPTNO, argJepDate1, argJepDate2);
        }

        public string GetXrayNobyXrayNo(string strXrayno)
        {
            return hicXrayResultRepository.GetXrayNobyXrayNo(strXrayno);
        }
        
        public int GetXrayCountByPtnoJepDate(string argPTNO, string argJepDate1, string argJepDate2, string argExCode)
        {
            return hicXrayResultRepository.GetXrayCountByPtnoJepDate(argPTNO, argJepDate1, argJepDate2, argExCode);
        }

        public int GetEndoCountByPtnoJepDate(string strPtNo, string strJepDate1, string strJepDate2, string strGbJob)
        {
            return hicXrayResultRepository.GetEndoCountByPtnoJepDate(strPtNo, strJepDate1, strJepDate2, strGbJob);
        }

        public string GetEndoResultDateByPtno(string pTNO)
        {
            return hicXrayResultRepository.GetEndoResultDateByPtno(pTNO);
        }

        public int GetCountbyPaNo(long fnPano)
        {
            return hicXrayResultRepository.GetCountbyPaNo(fnPano);
        }

        public HIC_XRAY_RESULT GetItembyPaNo(long nPano, string jEPDATE, string strXrayNo = "")
        {
            return hicXrayResultRepository.GetItembyPaNo(nPano, jEPDATE, strXrayNo);
        }

        public int GetCountbyPtNoPaNo(string text, string Date, string v)
        {
            return hicXrayResultRepository.GetCountbyPtNoPaNo(text, Date, v);
        }

        public int Insert(HIC_XRAY_RESULT item)
        {
            return hicXrayResultRepository.Insert(item);
        }

        public HIC_XRAY_RESULT GetXrayNobyPtNo(string pTNO, string text)
        {
            return hicXrayResultRepository.GetXrayNobyPtNo(pTNO, text);
        }

        public HIC_XRAY_RESULT GetItembyRowId(string strROWID)
        {
            return hicXrayResultRepository.GetItembyRowId(strROWID);
        }

        public List<HIC_XRAY_RESULT> GetListItemByPtnoJepDate(string fstrPtno, string fstrJepDate, string strExCode = "")
        {
            return hicXrayResultRepository.GetListItemByPtnoJepDate(fstrPtno, fstrJepDate, strExCode);
        }

        public List<HIC_XRAY_RESULT> GetItembyReadTime1(string strGTime)
        {
            return hicXrayResultRepository.GetItembyReadTime1(strGTime);
        }

        public int UpdateGbResultSendbyRowId(string rOWID)
        {
            return hicXrayResultRepository.UpdateGbResultSendbyRowId(rOWID);
        }

        public string GetXrayNobyPtNoJepDate(string pTNO, string jEPDATE)
        {
            return hicXrayResultRepository.GetXrayNobyPtNoJepDate(pTNO, jEPDATE);
        }

        public HIC_XRAY_RESULT GetItembyJepDatePtNo(string strJepDate, string fstrPano)
        {
            return hicXrayResultRepository.GetItembyJepDatePtNo(strJepDate, fstrPano);
        }

        public int Update_Patient_Exid(string strSabun, string strRowId)
        {
            return hicXrayResultRepository.Update_Patient_Exid(strSabun, strRowId);
        }

        public int GetCountbyPaNoXrayNoJepDate(long nPano, string strXrayno, string strJobDate)
        {
            return hicXrayResultRepository.GetCountbyPaNoXrayNoJepDate(nPano, strXrayno, strJobDate);
        }

        public int GetCountbyPtNoXCode(string strPtNo, string strXCode)
        {
            return hicXrayResultRepository.GetCountbyPtNoXCode(strPtNo, strXCode);
        }

        public HIC_XRAY_RESULT GetAllbyPtNoJepDate(string fstrPano, string fstrJepDate, string strXCode)
        {
            return hicXrayResultRepository.GetAllbyPtNoJepDate(fstrPano, fstrJepDate, strXCode);
        }
        public List<HIC_XRAY_RESULT> GetItemByJepDate( string argJepDate1, string argJepDate2, string argGubun)
        {
            return hicXrayResultRepository.GetItemByJepDate(argJepDate1, argJepDate2, argGubun);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicXrayResultRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public string GetRowidByXrayNo(string argXrayNo)
        {
            return hicXrayResultRepository.GetRowidByXrayNo(argXrayNo);
        }

        public bool UpDateDelDateByXrayNo(string argXrayNo, long argPano)
        {
            try
            {
                hicXrayResultRepository.UpDateDelDateByXrayNo(argXrayNo, argPano);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetXrayNoByJepDatePtno(string argPano, string argDate, string argXCode)
        {
            return hicXrayResultRepository.GetXrayNoByJepDatePtno(argPano, argDate, argXCode);
        }

        public bool UpDateDelDateNullByJepDatePtnoXrayNo(string argPano, string argDate, string argXrayNo)
        {
            try
            {
                hicXrayResultRepository.UpDateDelDateNullByJepDatePtnoXrayNo(argPano, argDate, argXrayNo);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void UpDateGjJongPanoByRowid(string argGjJong, long argPano, string argRowid)
        {
            try
            {
                hicXrayResultRepository.UpDateGjJongPanoByRowid(argGjJong, argPano, argRowid);

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public string GetRowidByPtnoJepDate(string pTNO, string jEPDATE)
        {
            return hicXrayResultRepository.GetRowidByPtnoJepDate(pTNO, jEPDATE);
        }

        public HIC_XRAY_RESULT GetItemByPanoXrayno(long argPano, string argXrayno)
        {
            return hicXrayResultRepository.GetItemByPanoXrayno(argPano, argXrayno);
        }

        public HIC_XRAY_RESULT GetItemByPtnoXrayno(string argPtno, string argXrayno)
        {
            return hicXrayResultRepository.GetItemByPtnoXrayno(argPtno, argXrayno);
        }


        public void UpDateGbOrderSendByPanoXrayNo(long argPano, string argXrayNo)
        {
            hicXrayResultRepository.UpDateGbOrderSendByPanoXrayNo(argPano, argXrayNo);
            return;
        }

        public string GetXrayNoByJepDate(string strDate, string strPtNo)
        {
            return hicXrayResultRepository.GetXrayNoByJepDate(strDate, strPtNo);
        }

        public int GetCountbyJepDateXrayno(string strDate, string strXrayno)
        {
            return hicXrayResultRepository.GetCountbyJepDateXrayno(strDate, strXrayno);
        }

        public int SaveHicXrayResultWork(HIC_XRAY_RESULT item)
        {
            return hicXrayResultRepository.SaveHicXrayResultWork(item);
        }

        public HIC_XRAY_RESULT GetAllbyPtNoJepDateXCode(string fstrPano, string fstrJepDate, string strXCode)
        {
            return hicXrayResultRepository.GetAllbyPtNoJepDateXCode(fstrPano, fstrJepDate, strXCode);
        }

        public List<HIC_XRAY_RESULT> GetListItemByHeaPtno(string strDate)
        {
            return hicXrayResultRepository.GetListItemByHeaPtno(strDate);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepDateLtdCodeGubun(string argFdate, string argTDate, long argLtdCode, string argGubun, string argSname)
        {
            return hicXrayResultRepository.GetItemByJepDateLtdCodeGubun(argFdate, argTDate, argLtdCode, argGubun, argSname);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepDateLtdCodeDocJongGubun(string argFdate, string argTDate, long argLtdCode, string argGbRead, string argJong, long argReadDoc1, long argReadDoc2, string argGubun, string argReadDate)
        {
            return hicXrayResultRepository.GetItemByJepDateLtdCodeDocJongGubun(argFdate, argTDate, argLtdCode, argGbRead,  argJong,  argReadDoc1,  argReadDoc2,  argGubun, argReadDate);
        }

        public int UpdateGbPrintByXrayNo(string argXrayNo, string argGbPrint)
        {
            return hicXrayResultRepository.UpdateGbPrintByXrayNo(argXrayNo, argGbPrint);
        }
        public int UpdateGbPrintByXrayNo1(List<string> argXrayNo, string argGbPrint)
        {
            return hicXrayResultRepository.UpdateGbPrintByXrayNo1(argXrayNo, argGbPrint);
        }

        public HIC_XRAY_RESULT GetMaxMinXrayNoByXrayNo(List<string> argXrayNo)
        {
            return hicXrayResultRepository.GetMaxMinXrayNoByXrayNo(argXrayNo);
        }

        public HIC_XRAY_RESULT GetReadDoctByXrayNo(List<string> argXrayNo)
        {
            return hicXrayResultRepository.GetReadDoctByXrayNo(argXrayNo);
        }

        public List<HIC_XRAY_RESULT> GetItemByXrayNo(List<string> argXrayNo)
        {
            return hicXrayResultRepository.GetItemByXrayNo(argXrayNo);
        }

        public List<HIC_XRAY_RESULT> GetItemByJepdateJongLtd(string argFDate, string argTDate, string argGjJong, long argLtdCode, string argGbsts, string argGbChul)
        {
            return hicXrayResultRepository.GetItemByJepdateJongLtd(argFDate, argTDate, argGjJong, argLtdCode, argGbsts, argGbChul);
        }
        public int UpdateGbReadByXrayNo(string argGbRead, string argXrayNo)
        {
            return hicXrayResultRepository.UpdateGbReadByXrayNo(argGbRead, argXrayNo);
        }

        public int UpdateGbChulByXrayNo(string argGbChul, string argXrayNo)
        {
            return hicXrayResultRepository.UpdateGbChulByXrayNo(argGbChul, argXrayNo);
        }


    }
}
