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
    public class HeaResultService
    {

        private HeaResultRepository heaResultRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaResultService()
        {
            this.heaResultRepository = new HeaResultRepository();
        }

        public int UpdateResultbyWrtNoExCode(string argResult, string strResCode, string idNumber, long argWrtNo, string argExCode)
        {
            return heaResultRepository.UpdateResultbyWrtNoExCode(argResult, strResCode, idNumber, argWrtNo, argExCode);
        }

        public int GetCountbyWrtNoExCode(long nWRTNO, List<string> strACT_SET_Data)
        {
            return heaResultRepository.GetCountbyWrtNoExCode(nWRTNO, strACT_SET_Data);
        }

        public string ChkExamByWrtnoExCode(long nWRTNO, string argExCode)
        {
            return heaResultRepository.ChkExamByWrtnoExCode(nWRTNO, argExCode);
        }

        public long GetCountByWrtnoInExcodeIn(List<long> lstHeaWrtno, List<string> lstExCode)
        {
            return heaResultRepository.GetCountByWrtnoInExcodeIn(lstHeaWrtno, lstExCode);
        }

        public List<HEA_RESULT> GetExCodeResult(long fnHeaWRTNO)
        {
            return heaResultRepository.GetExCodeResult(fnHeaWRTNO);
        }

        public bool UpDatePartResCodeByExCodeWrtno(HEA_RESULT hRES)
        {
            try
            {
                heaResultRepository.UpDatePartResCodeByExCodeWrtno(hRES);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetRowidByOneExcodeWrtno(string argExcode, long argwRTNO)
        {
            return heaResultRepository.GetRowidByOneExcodeWrtno(argExcode, argwRTNO);
        }

        public bool InSert(HEA_RESULT item)
        {
            try
            {
                heaResultRepository.InSert(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCountbyPtNo(string strPtNo)
        {
            return heaResultRepository.GetCountbyPtNo(strPtNo);
        }

        public int UpdateResultActivebyWrtNoExCode(string strResult, string strActive, string idNumber, long fnWRTNO, string strExCode)
        {
            return heaResultRepository.UpdateResultActivebyWrtNoExCode(strResult, strActive, idNumber, fnWRTNO, strExCode);
        }

        public bool UpdateResultActivebyWrtNoExCode(string strResult, string strActive, long fnWRTNO, string strExCode)
        {
            try
            {
                heaResultRepository.UpdateResultActivebyWrtNoExCode(strResult, strActive, fnWRTNO, strExCode);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<HEA_RESULT> GetExCodebyWrtNo_All(long wRTNO, List<string> lstInstr)
        {
            return heaResultRepository.GetExCodebyWrtNo_All(wRTNO, lstInstr);
        }

        public List<HEA_RESULT> GetActiveExCodebyWrtNo(long nWrtNo)
        {
            return heaResultRepository.GetActiveExCodebyWrtNo(nWrtNo);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNoExCode(long argHcWRTNO)
        {
            return heaResultRepository.GetExCodeResultbyWrtNoExCode(argHcWRTNO);
        }

        public List<HEA_RESULT> Read_Result(long fnWRTNO, string hEAPART)
        {
            return heaResultRepository.Read_Result2(fnWRTNO, hEAPART);
        }

        public List<HEA_RESULT> Read_Active(long fnWRTNO, string hEAPART)
        {
            return heaResultRepository.Read_Active(fnWRTNO, hEAPART);
        }

        public List<HEA_RESULT> Get_Results(long wRTNO)
        {
            return heaResultRepository.Get_Results(wRTNO);
        }

        public List<HEA_RESULT> GetItembyWrtNo(long nWrtNo)
        {
            return heaResultRepository.GetItembyWrtNo(nWrtNo);
        }

        public int Result_History_Insert(string idNumber, string strResult, string strROWID, string strExCode)
        {
            return heaResultRepository.Result_History_Insert(idNumber, strResult, strROWID, strExCode);
        }

        public int Result_Update(string strResult, string strPanjeng, string strResCode, string idNumber, string strRowId, string strExCode = "")
        {
            return heaResultRepository.Result_Update(strResult, strPanjeng, strResCode, idNumber, strRowId, strExCode);
        }

        public List<HIC_RESULT> Read_Result_Acitve(long nWrtNo, List<string> fstrPartG)
        {
            return heaResultRepository.Read_Result_Acitve(nWrtNo, fstrPartG);
        }

        public List<HEA_RESULT> Read_Result(long fnWRTNO)
        {
            return heaResultRepository.Read_Result(fnWRTNO);
        }

        public string GetRowidByWrtnoExCodeIN(long wRTNO, List<string> lstExCodes)
        {
            return heaResultRepository.GetRowidByWrtnoExCodeIN(wRTNO, lstExCodes);
        }

        public List<HEA_RESULT> GetAllByWrtNo(long argWrtno)
        {
            return heaResultRepository.GetAllByWrtNo(argWrtno);
        }

        public void InsertDelSelectbyRowid(string argRid)
        {
            heaResultRepository.InsertDelSelectbyRowid(argRid);
        }

        public void DeleteByRowid(string argRid)
        {
            heaResultRepository.DeleteByRowid(argRid);
        }

        public HEA_RESULT GetOneItemByExCodeWrtno(string argExCode, long argWrtno)
        {
            return heaResultRepository.GetOneItemByExCodeWrtno(argExCode, argWrtno);
        }

        public List<HEA_RESULT> GetListByWrtnoExCodeIN(long fnWRTNO, List<string> lstExCD)
        {
            return heaResultRepository.GetListByWrtnoExCodeIN(fnWRTNO, lstExCD);
        }

        public string IsActiveByWrtno(long argWRTNO)
        {
            return heaResultRepository.IsActiveByWrtno(argWRTNO);
        }

        public List<HEA_RESULT> GetListByWrtnoGubun(long wRTNO)
        {
            return heaResultRepository.GetListByWrtnoGubun(wRTNO);
        }

        public int GetCountbyWrtNo(long argWrtNo, string argRoom)
        {
            return heaResultRepository.GetCountbyWrtNo(argWrtNo, argRoom);
        }

        public int GetCountbyWrtNoHaRoom(long argWrtNo, string strRoom)
        {
            return heaResultRepository.GetCountbyWrtNoHaRoom(argWrtNo, strRoom);
        }

        public List<HEA_RESULT> Read_ResultAct(long fnWRTNO, string hEAPART)
        {
            return heaResultRepository.Read_ResultAct(fnWRTNO, hEAPART);
        }

        public HEA_RESULT GetEntSabunbyWrtNo(long wRTNO)
        {
            return heaResultRepository.GetEntSabunbyWrtNo(wRTNO);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNo(long fnWRTNO)
        {
            return heaResultRepository.GetExCodeResultbyWrtNo(fnWRTNO);
        }

        public List<HEA_RESULT> GetExCodeResultbyWrtNo1(long fnWRTNO)
        {
            return heaResultRepository.GetExCodeResultbyWrtNo1(fnWRTNO);
        }

        public List<HEA_RESULT> GetListByWrtno(long argWRTNO)
        {
            return heaResultRepository.GetListByWrtno(argWRTNO);
        }

        public int UpDateResultPanjengByRid(string strNewPan, string argRid)
        {
            return heaResultRepository.UpDateResultPanjengByRid(strNewPan, argRid);
        }

        public string GetActiveByWrtnoExCode(long wRTNO, string eXCODE)
        {
            return heaResultRepository.GetActiveByWrtnoExCode(wRTNO, eXCODE);
        }

        public HEA_RESULT Read_Result_YN(long WRTNO, string EXCODE)
        {
            return heaResultRepository.Read_Result_YN(WRTNO, EXCODE);
        }

        public HEA_RESULT GetResultBYWrtnoExCodeIN(long nWRTNO, List<string> strExcode)
        {
            return heaResultRepository.GetResultBYWrtnoExCodeIN(nWRTNO, strExcode);
        }

        public int UpDateResultReadTimeByItemExCodeIN(HEA_RESULT item, List<string> strExcode)
        {
            return heaResultRepository.UpDateResultReadTimeByItemExCodeIN(item, strExcode);
        }

        public int UpdateSangdambyWrtnoExCode(long fnWRTNO, string strExam, string strSangDam)
        {
            return heaResultRepository.UpdateSangdambyWrtnoExCode(fnWRTNO, strExam, strSangDam);
        }
        public int Result_Update2(double RESULT, long WRTNO, string EXCODE)
        {
            return heaResultRepository.Result_Update2(RESULT, WRTNO, EXCODE);
        }

        public int UpdateResCode(string strResCode, string strResult, long fnWRTNO)
        {
            return heaResultRepository.UpdateResCode(strResCode, strResult, fnWRTNO);
        }
    }
}
