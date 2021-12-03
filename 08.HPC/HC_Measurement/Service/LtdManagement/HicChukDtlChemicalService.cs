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
    public class HicChukDtlChemicalService
    {
        private HicChukDtlChemicalRepository hicChukDtlChemicalRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlChemicalService()
        {
            this.hicChukDtlChemicalRepository = new HicChukDtlChemicalRepository();
        }

        public bool Save(IList<HIC_CHUKDTL_CHEMICAL> list3, long argWRTNO)
        {
            try
            {
                foreach (HIC_CHUKDTL_CHEMICAL code in list3)
                {
                    code.JOBSABUN = clsType.User.IdNumber.To<long>(0);

                    if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (code.WRTNO > 0)
                        {
                            hicChukDtlChemicalRepository.Delete(code);
                        }
                    }
                    else
                    {
                        if (code.RID.IsNullOrEmpty())
                        {
                            code.WRTNO = argWRTNO;
                            hicChukDtlChemicalRepository.InSert(code);
                        }
                        else
                        {
                            hicChukDtlChemicalRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_CHEMICAL> GetListByWrtno(long argWRTNO, bool bDel = false)
        {
            return hicChukDtlChemicalRepository.GetListByWrtno(argWRTNO, bDel);
        }

        public bool DeleteAll(long nWRTNO)
        {
            try
            {
                hicChukDtlChemicalRepository.DeleteAll(nWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
