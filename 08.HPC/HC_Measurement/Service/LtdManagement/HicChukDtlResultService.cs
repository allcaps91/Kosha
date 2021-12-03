namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc.Spread;
    using ComHpcLibB.Dto;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlResultService
    {
        private HicChukDtlResultRepository hicChukDtlResultRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlResultService()
        {
            this.hicChukDtlResultRepository = new HicChukDtlResultRepository();
        }

        public bool Save(List<HIC_CHUKDTL_RESULT> lstUCD, long argWRTNO, string argGBN = "1")
        {
            try
            {
                foreach (HIC_CHUKDTL_RESULT code in lstUCD)
                {
                    if (code.RID.IsNullOrEmpty())
                    {
                        code.WRTNO = argWRTNO;
                        code.GUBUN = argGBN;
                        code.ENTSABUN = clsType.User.IdNumber.To<long>(0);

                        hicChukDtlResultRepository.InSert(code);
                    }
                    else
                    {
                        if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                        {
                            hicChukDtlResultRepository.Delete(code);
                        }
                        else
                        {
                            code.ENTSABUN = clsType.User.IdNumber.To<long>(0);

                            hicChukDtlResultRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_RESULT> GetListByWrtno(long argWRTNO, string argGubun)
        {
            return hicChukDtlResultRepository.GetListByWrtno(argWRTNO, argGubun);
        }

        public List<HIC_CHUKDTL_RESULT> GetListMCodeByWrtno(long nWRTNO, long nYear)
        {
            return hicChukDtlResultRepository.GetListMCodeByWrtno(nWRTNO, nYear);
        }

        public SpreadComboBoxData GetSpreadComboBoxData(long nWRTNO)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CHUKDTL_RESULT> list = hicChukDtlResultRepository.GetMCodeListByWrtno(nWRTNO);

            foreach (HIC_CHUKDTL_RESULT code in list)
            {
                data.Put(code.CHMCLS_CD, code.CHMCLS_NM);
            }
            return data;
        }

        public List<HIC_CHUKDTL_RESULT> GetPlanListByWrtno(long wRTNO, string argGubun)
        {
            return hicChukDtlResultRepository.GetPlanListByWrtno(wRTNO, argGubun);
        }
    }
}
