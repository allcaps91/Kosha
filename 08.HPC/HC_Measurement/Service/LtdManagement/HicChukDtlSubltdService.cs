namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase.Controls;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlSubltdService
    {
        private HicChukDtlSubltdRepository hicChukDtlSubltdRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlSubltdService()
        {
            this.hicChukDtlSubltdRepository = new HicChukDtlSubltdRepository();
        }

        public bool Save(IList<HIC_CHUKDTL_SUBLTD> list4, long argWRTNO, long argLtdCode)
        {
            try
            {
                foreach (HIC_CHUKDTL_SUBLTD code in list4)
                {
                    if (code.RID.IsNullOrEmpty() && code.SUB_LTDCODE > 0)
                    {
                        code.WRTNO = argWRTNO;
                        code.LTDCODE = argLtdCode;
                        hicChukDtlSubltdRepository.InSert(code);
                    }
                    else
                    {
                        if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                        {
                            hicChukDtlSubltdRepository.Delete(code);
                        }
                        else
                        {
                            hicChukDtlSubltdRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_SUBLTD> GetListByWrtno(long argWRTNO)
        {
            return hicChukDtlSubltdRepository.GetListByWrtno(argWRTNO);
        }

        public bool DeleteAll(long nWRTNO)
        {
            try
            {
                hicChukDtlSubltdRepository.DeleteAll(nWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
