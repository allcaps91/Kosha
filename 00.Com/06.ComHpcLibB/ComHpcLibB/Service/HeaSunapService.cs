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
    public class HeaSunapService
    {

        private HeaSunapRepository heaSunapRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaSunapService()
        {
            this.heaSunapRepository = new HeaSunapRepository();
        }

        public List<HEA_SUNAP> GetSumbySuDate(string strFrDate, string strToDate)
        {
            return heaSunapRepository.GetSumbySuDate(strFrDate, strToDate);
        }

        public HEA_SUNAP GetAmtbyWrtNo(long wRTNO)
        {
            return heaSunapRepository.GetAmtbyWrtNo(wRTNO);
        }

        public long GetSunapAmtByWrtno(long nWRTNO)
        {
            return heaSunapRepository.GetSunapAmtByWrtno(nWRTNO);
        }

        public string GetRowidByWrtno(long nWRTNO)
        {
            return heaSunapRepository.GetRowidByWrtno(nWRTNO);
        }
        public List<HEA_SUNAP> GetRowidByWrtno1(long nWRTNO)
        {
            return heaSunapRepository.GetRowidByWrtno1(nWRTNO);
        }

        public bool Insert(HEA_SUNAP haSUNAP)
        {
            try
            {
                heaSunapRepository.Insert(haSUNAP);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public long GetMisuAmtByWrtno(long fnWRTNO)
        {
            return heaSunapRepository.GetMisuAmtByWrtno(fnWRTNO);
        }

        public long GetSumTotAmtByWrtno(long wRTNO)
        {
            return heaSunapRepository.GetSumTotAmtByWrtno(wRTNO);
        }

        public long GetSumBoninAmtByWrtno(long wRTNO1)
        {
            return heaSunapRepository.GetSumBoninAmtByWrtno(wRTNO1);
        }

        public void DeleteByRowid(string argRowid)
        {
            heaSunapRepository.DeleteByRowid(argRowid);
        }
    }
}
