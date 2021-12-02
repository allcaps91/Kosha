namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class EtcAlimTalkService
    {
        private EtcAlimTalkRepository etcAlimTalkRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public EtcAlimTalkService()
        {
            etcAlimTalkRepository = new EtcAlimTalkRepository();
        }

        public int Insert(ETC_ALIMTALK item)
        {
            return etcAlimTalkRepository.Insert(item);
        }

        public ETC_ALIMTALK GetRDatebyJobDatePaNoHPhoneTempCd(string strFDate, string strTDate, string strPANO, string strTel, string strTempCD)
        {
            return etcAlimTalkRepository.GetRDatebyJobDatePaNoHPhoneTempCd(strFDate, strTDate, strPANO, strTel, strTempCD);
        }

        public List<ETC_ALIMTALK> GetItembySendFlag(string strSendFlag)
        {
            return etcAlimTalkRepository.GetItembySendFlag(strSendFlag);
        }

        public List<ETC_ALIMTALK> GetItembyJobDate(string strFrDate, string strToDate, string strDeptName, string strTempCd, string strReportCode, string strSName, string strPhone)
        {
            return etcAlimTalkRepository.GetItembyJobDate(strFrDate, strToDate, strDeptName, strTempCd, strReportCode, strSName, strPhone);
        }

        public bool UpDateDelDateByPanoRDateTmpCD(long argPano, string argSDate, string argTmpCD)
        {
            try
            {
                etcAlimTalkRepository.UpDateDelDateByPanoRDateTmpCD(argPano, argSDate, argTmpCD);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
