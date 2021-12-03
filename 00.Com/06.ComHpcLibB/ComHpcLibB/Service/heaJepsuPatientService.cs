namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    public class HeaJepsuPatientService
    {
        private HeaJepsuPatientRepository heaJepsuPatientRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuPatientService()
        {
            this.heaJepsuPatientRepository = new HeaJepsuPatientRepository();
        }

        public List<HEA_JEPSU_PATIENT> GetItembyLtdCode(string strFDate, string strTDate, string strLtdCode)
        {
            return heaJepsuPatientRepository.GetItembyLtdCode(strFDate, strTDate, strLtdCode);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySName(string strFrDate, string strToDate, long nLtdCode, string strSName, string strSend, string strSort)
        {
            return heaJepsuPatientRepository.GetItembySName(strFrDate, strToDate, nLtdCode, strSName, strSend, strSort);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySDate(string strBDate, long nSabun)
        {
            return heaJepsuPatientRepository.GetItembySDate(strBDate, nSabun);
        }

        public HEA_JEPSU_PATIENT GetItembyPano(long nPano)
        {
            return heaJepsuPatientRepository.GetItembyPano(nPano);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyIDate(string strFrDate, string strToDate, string strName, string strGbn, string strLtdCode, string strJong)
        {
            return heaJepsuPatientRepository.GetItembyIDate(strFrDate, strToDate, strName, strGbn, strLtdCode, strJong);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySDateLtdCode(string strSDate, string strGbSts, string strLtdCode, string strSort)
        {
            return heaJepsuPatientRepository.GetItembySDateLtdCode(strSDate, strGbSts, strLtdCode, strSort);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyJepDate(string strFDate, string strTDate)
        {
            return heaJepsuPatientRepository.GetItembyJepDate(strFDate, strTDate);
        }

        public List<HEA_JEPSU_PATIENT> GetItembyJepDateGjJong(string strFDate, string strTDate, List<string> strjong)
        {
            return heaJepsuPatientRepository.GetItembyJepDateGjJong(strFDate, strTDate, strjong);
        }

        public List<HEA_JEPSU_PATIENT> GetItembySdateSname(string strFDate, string strTDate, string strSname)
        {
            return heaJepsuPatientRepository.GetItembySdateSname(strFDate, strTDate, strSname);
        }

        public List<HEA_JEPSU_PATIENT> GetListBySDateAMPM2(string strSDate, string strAmPm2)
        {
            return heaJepsuPatientRepository.GetListBySDateAMPM2(strSDate, strAmPm2);
        }

        public HEA_JEPSU GetWrtnoBySDateSNameJumin2(string strFDate, string strTDate, string strSName, string strAESJumin, string strJumin1)
        {
            return heaJepsuPatientRepository.GetWrtnoBySDateSNameJumin2(strFDate, strTDate, strSName, strAESJumin, strJumin1);
        }

        public HEA_JEPSU_PATIENT GetJepsuListbyToDay(long nPano)
        {
            return heaJepsuPatientRepository.GetJepsuListbyToDay(nPano);
        }

        public HEA_JEPSU_PATIENT GetItemByJumin2(string strJumin)
        {
            return heaJepsuPatientRepository.GetItemByJumin2(strJumin);
        }

        public List<HEA_JEPSU_PATIENT> GetJepsuListbyToDayAll()
        {
            return heaJepsuPatientRepository.GetJepsuListbyToDayAll();
        }

        public HEA_JEPSU_PATIENT GetItembyWrtNo(long argWrtNo)
        {
            return heaJepsuPatientRepository.GetItembyWrtNo(argWrtNo);
        }

        public List<HEA_JEPSU_PATIENT> GetItemByHanwol(string strFDate, string strTDate, long nLtdcode)
        {
            return heaJepsuPatientRepository.GetItemByHanwol(strFDate, strTDate, nLtdcode);
        }


    }
}
