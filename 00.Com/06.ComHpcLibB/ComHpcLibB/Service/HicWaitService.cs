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
    public class HicWaitService
    {
        
        private HicWaitRepository hicWaitRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicWaitService()
        {
			this.hicWaitRepository = new HicWaitRepository();
        }

        public List<HIC_WAIT> GetReExambyDate()
        {
            return hicWaitRepository.GetReExambyDate();
        }

        public int Update_SecondPrint(string sRid)
        {
            return hicWaitRepository.Update_SecondPrint(sRid);
        }

        public List<HIC_WAIT> GetItembyJobDate(string gstrSysDate, bool bChk, string argBuse)
        {
            return hicWaitRepository.GetItembyJobDate(gstrSysDate, bChk, argBuse);
        }

        /// <summary>
        /// 일반검진 대기자 인원 쿼리
        /// </summary>
        public List<HIC_WAIT> GetItembyJobDate1(string gstrSysDate, string strMundate)
        {
            return hicWaitRepository.GetItembyJobDate1(gstrSysDate, strMundate);
        }

        public HIC_WAIT GetItemByJobDateGbDisplay(string argDate, string argDisplay)
        {
            return hicWaitRepository.GetItemByJobDateGbDisplay(argDate, argDisplay);
        }

        public void DeleteByJobDate(string strDate)
        {
            try
            {
                hicWaitRepository.DeleteByJobDate(strDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 종합검진 대기자 총인원 쿼리
        /// </summary>
        public HIC_WAIT GetItembyJobDate2(string gstrSysDate, string argGbBuse)
        {
            return hicWaitRepository.GetItembyJobDate2(gstrSysDate, argGbBuse);
        }


        /// <summary>
        /// 대기순번 대상자 삭제
        /// </summary>
        public int DeleteBySeqNo(long nSeqno, string strJOBDATE)
        {
            return hicWaitRepository.DeleteBySeqNo(nSeqno, strJOBDATE);
        }

        public bool UpDateDisplay(string argDisplay, string argJobDate, string argPcNo)
        {
            try
            {
                hicWaitRepository.UpDateDisplay(argDisplay, argJobDate, argPcNo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 대기순번 대상자 주민번호조회
        /// </summary>
        public List<HIC_WAIT> GetListbyJobdateJumin(string strJOBDATE, string strJUMIN)
        {
            return hicWaitRepository.GetListbyJobdateJumin(strJOBDATE, strJUMIN);
        }

        public bool UpDateCompleteOK(string argTime, string jUMINNO2, string argDate)
        {
            try
            {
                hicWaitRepository.UpDateCompleteOK(argTime, jUMINNO2, argDate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_WAIT> GetItembyJobDate(string argBuse)
        {
            return hicWaitRepository.GetItembyJobDate(argBuse);
        }

        public HIC_WAIT GetCountItemByJobDateGbJob(string argDate, string argJob)
        {
            return hicWaitRepository.GetCountItemByJobDateGbJob(argDate, argJob);
        }

        public int GetCountbyJobDate(string argGbBuse)
        {
            return hicWaitRepository.GetCountbyJobDate(argGbBuse);
        }

        public bool UpDateCall(string strPcNo, string argJob, int nSeqNo)
        {
            try
            {
                hicWaitRepository.UpDateCall(strPcNo, argJob, nSeqNo);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public long GetMaxSeqnoByJobDate(string argDate, string argJob)
        {
            return hicWaitRepository.GetMaxSeqnoByJobDate(argDate, argJob);
        }

        public bool UpDateCallWaitPC(string strPcNo, string argJob, string argBuse, string strName, int nSeqNo)
        {
            try
            {
                hicWaitRepository.UpDateCallWaitPC(strPcNo, argJob, argBuse, strName, nSeqNo);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateReCallWaitPC(string strPcNo, string argJob, string argBuse)
        {
            try
            {
                hicWaitRepository.UpDateReCallWaitPC(strPcNo, argJob, argBuse);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetRowidByJobDatePcNo(string strWaitPcNo)
        {
            return hicWaitRepository.GetRowidByJobDatePcNo(strWaitPcNo);
        }

        public bool InsertHicWaitPcRow(string strWaitPcNo)
        {
            try
            {
                hicWaitRepository.InsertHicWaitPcRow(strWaitPcNo);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetRowidByJobDateJumin2(string strDate, string strJumin2)
        {
            return hicWaitRepository.GetRowidByJobDateJumin2(strDate, strJumin2);
        }

        public long GetSeqNoByJobDateJumin(string strCurDate, string argJumin2)
        {
            return hicWaitRepository.GetSeqNoByJobDateJumin(strCurDate, argJumin2);
        }

        public long GetMaxSeqNoByGbYeyakAge(string strDate, string strGbYeyak, int nAge, string strGBGFS, string strGBCT, string strJONG)
        {
            return hicWaitRepository.GetMaxSeqNoByGbYeyakAge(strDate, strGbYeyak, nAge, strGBGFS, strGBCT, strJONG);
        }

        public bool InsertData(HIC_WAIT nHW)
        {
            try
            {
                hicWaitRepository.InsertData(nHW);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public long GetMaxSeqnoByJobDateGbBuse(string strCurDate, string argBuse, string argEndo, long argCount)
        {
            return hicWaitRepository.GetMaxSeqnoByJobDateGbBuse(strCurDate, argBuse, argEndo, argCount);
        }
    }
}
