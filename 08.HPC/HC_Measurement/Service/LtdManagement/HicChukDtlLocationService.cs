namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using ComBase.Controls;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlLocationService
    {
        private HicChukDtlLocationRepository hicChukDtlLocationRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlLocationService()
        {
            this.hicChukDtlLocationRepository = new HicChukDtlLocationRepository();
        }

        public bool Save(List<HIC_CHUKDTL_LOCATION> lstHCL, long argWRTNO)
        {
            try
            {
                foreach (HIC_CHUKDTL_LOCATION code in lstHCL)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (!code.RID.IsNullOrEmpty())
                        {
                            hicChukDtlLocationRepository.Delete(code);
                        }
                    }
                    else
                    {
                        if (code.RID.IsNullOrEmpty())
                        {
                            code.WRTNO = argWRTNO;
                            hicChukDtlLocationRepository.InSert(code);
                        }
                        else
                        {
                            hicChukDtlLocationRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_LOCATION> GetListByWrtno(long argWRTNO)
        {
            return hicChukDtlLocationRepository.GetListByWrtno(argWRTNO);
        }

        public HIC_CHUKDTL_LOCATION FindImageByRowid(string strRid)
        {
            return hicChukDtlLocationRepository.FindImageByRowid(strRid);
        }
    }
}
