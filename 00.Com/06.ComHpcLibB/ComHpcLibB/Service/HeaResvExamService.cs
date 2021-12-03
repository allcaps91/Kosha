namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;
    using System.Windows.Forms;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvExamService
    {
        
        private HeaResvExamRepository heaResvExamRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvExamService()
        {
			this.heaResvExamRepository = new HeaResvExamRepository();
        }

        
        public string Read_Hes_Resv_Exam(string strSDate, long nPano, string strCode)
        {
            return heaResvExamRepository.Read_Hes_Resv_Exam(strSDate, nPano, strCode);
        }

        public HEA_RESV_EXAM GetCountbyPaNo(long pANO, string strExCode)
        {
            return heaResvExamRepository.GetCountbyPaNo(pANO, strExCode);
        }

        public HEA_RESV_EXAM GetCountbyPaNo1(long pANO, string strGbExam, string strSDate)
        {
            return heaResvExamRepository.GetCountbyPaNo1(pANO, strGbExam, strSDate);
        }

        public List<HEA_RESV_EXAM> GetItembyRTime(string sFrDate, string sToDate, string sGbExam)
        {
            return heaResvExamRepository.GetItembyRTime(sFrDate, sToDate, sGbExam);
        }

        public int GetCountbyPano(string sFrDate, string sToDate, string sGbExam, long nPano = 0)
        {
            return heaResvExamRepository.GetCountbyPano(sFrDate, sToDate, sGbExam, nPano);
        }

        public List<HEA_RESV_EXAM> GetCNTAMPMbyRTime(string argFDate, string argTDate, string argGubun)
        {
            return heaResvExamRepository.GetCNTAMPMbyRTime(argFDate, argTDate, argGubun);
        }

        public HEA_RESV_EXAM GetCountAMPMbyRTime(string argFDate, string argTDate, string argGubun)
        {
            return heaResvExamRepository.GetCountAMPMbyRTime(argFDate, argTDate, argGubun);
        }

        public int GetCountbyPanoSDate(string strPano, string strJepDate)
        {
            return heaResvExamRepository.GetCountbyPanoSDate(strPano, strJepDate);
        }

        public List<HEA_RESV_EXAM> GetItembyRTimeGbExam(string[] strGbExam)
        {
            return heaResvExamRepository.GetItembyRTimeGbExam(strGbExam);
        }

        public bool Save(IList<HEA_RESV_EXAM> list)
        {
            try
            {   
                foreach (HEA_RESV_EXAM code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        heaResvExamRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        heaResvExamRepository.Update(code);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public string GetRowidByPanoExcode(long nPano, string argExcode, string argCurDate)
        {
            return heaResvExamRepository.GetRowidByPanoExcode(nPano, argExcode, argCurDate);
        }

        public int GetCountbyPaNoGbExam(long argPano)
        {
            return heaResvExamRepository.GetCountbyPaNoGbExam(argPano);
        }

        public string GetRTimeByPanoExCodeSDate(long nPano, string argExCode, string argCurDate, string argType = "")
        {
            return heaResvExamRepository.GetRTimeByPanoExCodeSDate(nPano, argExCode, argCurDate, argType);
        }

        public List<HEA_RESV_EXAM> GetListByPanoRDate(long nPano, string argCurDate)
        {
            return heaResvExamRepository.GetListByPanoRDate(nPano, argCurDate);
        }

        public HEA_RESV_EXAM GetRTimeGbExamByPanoExCodeSDate(long pANO, string jEPDATE, string argExcode)
        {
            return heaResvExamRepository.GetRTimeGbExamByPanoExCodeSDate(pANO, jEPDATE, argExcode);
        }

        public string GetSDateByPanoRTimeExCode(long pANO, string argFDate, string argTDate, string argExcode)
        {
            return heaResvExamRepository.GetSDateByPanoRTimeExCode(pANO, argFDate, argTDate, argExcode);
        }

        public long GetExistCountbyPanoGbExam(string strRDate, string strTDate, long FnPano, string strGb, string strAMPM, List<long> lstLtdCodes = null)
        {
            return heaResvExamRepository.GetExistCountbyPanoGbExam(strRDate, strTDate, FnPano, strGb, strAMPM, lstLtdCodes);
        }

        public HEA_RESV_EXAM GetRTimebyPaNoGbExamSDate(long nPANO, string strSDate, string argGbExam)
        {
            return heaResvExamRepository.GetRTimebyPaNoGbExamSDate(nPANO, strSDate, argGbExam);
        }

        public string GetRTimeByPanoSDateGubun1(long nPano, string strSDate, string strGubn1)
        {
            return heaResvExamRepository.GetRTimeByPanoSDateGubun1(nPano, strSDate, strGubn1);
        }

        public string GetNotEqualResvExamByPanoSDate(long argPano, string argSDate)
        {
            return heaResvExamRepository.GetNotEqualResvExamByPanoSDate(argPano, argSDate);
        }

        public bool UpDateDelDateByPanoSDate(long argPano, string argSDate)
        {
            try
            {
                heaResvExamRepository.UpDateDelDateByPanoSDate(argPano, argSDate);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public HEA_RESV_EXAM GetRTimebySdatePanoExcode(string argSdate, string argPano,  string argCode)
        {
            return heaResvExamRepository.GetRTimebySdatePanoExcode(argSdate, argPano, argCode);
        }

        public List<HEA_RESV_EXAM> GetListByPanoSDate(long argPano, string argSDate)
        {
            return heaResvExamRepository.GetListByPanoSDate(argPano, argSDate);
        }

        public bool UpDateDelDateByPanoNotSDate(long argPano, string argSDate)
        {
            try
            {
                heaResvExamRepository.UpDateDelDateByPanoNotSDate(argPano, argSDate);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateDelDateByPanoNotExam(long argPano, List<string> varGED)
        {
            try
            {
                heaResvExamRepository.UpDateDelDateByPanoNotExam(argPano, varGED);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public List<HEA_RESV_EXAM> GetEndoResvListByPanoSDate(long argPano, string argSDate)
        {
            return heaResvExamRepository.GetEndoResvListByPanoSDate(argPano, argSDate);
        }

        public string GetRowidByPanoRTimeGbExam(long nPano, string sFrDate, string sToDate, string argGbExam)
        {
            return heaResvExamRepository.GetRowidByPanoRTimeGbExam(nPano, sFrDate, sToDate, argGbExam);
        }

        public List<HEA_RESV_EXAM> GetListByRTimeExCodeIN(string strDate, List<string> strExCode)
        {
            return heaResvExamRepository.GetListByRTimeExCodeIN(strDate, strExCode);
        }

        public HEA_RESV_EXAM GetCountBySdateGbexam(string argSdate, string argGbExam, string argSTime)
        {
            return heaResvExamRepository.GetCountBySdateGbexam(argSdate, argGbExam, argSTime);
        }
    }
}
