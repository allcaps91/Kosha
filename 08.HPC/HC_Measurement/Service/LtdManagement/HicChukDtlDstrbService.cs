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
    public class HicChukDtlDstrbService
    {
        private HicChukDtlDstrbRepository hicChukDtlDstrbRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlDstrbService()
        {
            this.hicChukDtlDstrbRepository = new HicChukDtlDstrbRepository();
        }

        public bool Save(List<HIC_CHUKDTL_DSTRB> lstHCL, long argWRTNO)
        {
            try
            {
                foreach (HIC_CHUKDTL_DSTRB code in lstHCL)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (!code.RID.IsNullOrEmpty())
                        {
                            hicChukDtlDstrbRepository.Delete(code);
                        }
                    }
                    else
                    {
                        if (code.RID.IsNullOrEmpty())
                        {
                            code.WRTNO = argWRTNO;
                            hicChukDtlDstrbRepository.InSert(code);
                        }
                        else
                        {
                            hicChukDtlDstrbRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_DSTRB> GetListByWrtno(long argWRTNO)
        {
            return hicChukDtlDstrbRepository.GetListByWrtno(argWRTNO);
        }

        public HIC_CHUKDTL_DSTRB FindImageByRowid(string strRid)
        {
            return hicChukDtlDstrbRepository.FindImageByRowid(strRid);
        }

        public HIC_CHUKDTL_DSTRB GetItemByWrtno(long nWRTNO)
        {
            return hicChukDtlDstrbRepository.GetItemByWrtno(nWRTNO);
        }
    }
}
