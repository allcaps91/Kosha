namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComBase.Controls;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukEstimateService
    {
        private HicChukEstimateRepository hicChukEstimateRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukEstimateService()
        {
            this.hicChukEstimateRepository = new HicChukEstimateRepository();
        }

        public List<HIC_CHUK_ESTIMATE> GetItemByWrtno(long nWRTNO)
        {
            return hicChukEstimateRepository.GetItemByWrtno(nWRTNO);
        }


        public HIC_CHUK_ESTIMATE GetSumEstAmtByWrtno(long nWRTNO)
        {
            return hicChukEstimateRepository.GetSumEstAmtByWrtno(nWRTNO);
        }

        public bool Save(IList<HIC_CHUK_ESTIMATE> list, long argWRTNO)
        {
            try
            {
                foreach (HIC_CHUK_ESTIMATE code in list)
                {
                    code.JOBSABUN = clsType.User.IdNumber.To<long>(0);

                    if (code.RID.IsNullOrEmpty())
                    {
                        code.WRTNO = argWRTNO;
                        hicChukEstimateRepository.InSert(code);
                    }
                    else
                    {
                        if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                        {
                            hicChukEstimateRepository.Delete(code);
                        }
                        else
                        {
                            hicChukEstimateRepository.UpDate(code);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool InsertAmt(HIC_CHUK_ESTIMATE hCEST, long fnWRTNO)
        {
            try
            {
                hCEST.WRTNO = fnWRTNO;
                hCEST.JOBSABUN = clsType.User.IdNumber.To<long>(0);
               
                hicChukEstimateRepository.InsertAmt(hCEST);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool UpDateAmt(HIC_CHUK_ESTIMATE hCEST, long fnWRTNO)
        {
            try
            {
                hicChukEstimateRepository.MinusAmt(hCEST);

                hCEST.WRTNO = fnWRTNO;
                hCEST.JOBSABUN = clsType.User.IdNumber.To<long>(0);
                hCEST.SEQNO += 2;

                hicChukEstimateRepository.InsertAmt(hCEST);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetRowidEstAmtByWrtno(long fnWRTNO, long nSeqno)
        {
            return hicChukEstimateRepository.GetRowidEstAmtByWrtno(fnWRTNO, nSeqno);
        }

        public long GetMaxSeqNoByWrtno(long fnWRTNO)
        {
            return hicChukEstimateRepository.GetMaxSeqNoByWrtno(fnWRTNO);
        }

        public long GetEstAmtSumByWrtno(long fnWRTNO)
        {
            return hicChukEstimateRepository.GetEstAmtSumByWrtno(fnWRTNO);
        }

        public bool DeleteAll(long fnWRTNO)
        {
            try
            {
                hicChukEstimateRepository.DeleteAll(fnWRTNO);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool DeleteAmt(HIC_CHUK_ESTIMATE hCE, long fnWRTNO)
        {
            try
            {
                hCE.WRTNO = fnWRTNO;
                hCE.SEQNO += 1;
                hCE.JOBSABUN = clsType.User.IdNumber.To<long>();
                hCE.TOTAMT *= -1;
                hCE.BASEAMT *= -1;
                hCE.CHARGEAMT *= -1;
                hCE.HALINAMT *= -1;
                hCE.AMT *= -1;

                hicChukEstimateRepository.MinusSumAmt(hCE);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public HIC_CHUK_ESTIMATE GetSendInfoByWrtno(long nWRTNO)
        {
            return hicChukEstimateRepository.GetSendInfoByWrtno(nWRTNO);
        }

        public void UpdateSendMail(string strLtdMgr, string strLtdAddr, string strRid)
        {
            hicChukEstimateRepository.UpdateSendMail(strLtdMgr, strLtdAddr, strRid);
        }
    }
}
