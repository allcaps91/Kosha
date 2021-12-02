namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc.Spread;
    using HC_Measurement.Dto;
    using HC_Measurement.Model;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukDtlPlanService
    {
        private HicChukDtlPlanRepository hicChukDtlPlanRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukDtlPlanService()
        {
            this.hicChukDtlPlanRepository = new HicChukDtlPlanRepository();
        }

        public bool Save(IList<HIC_CHUKDTL_PLAN> list, long argWRTNO)
        {
            try
            {
                foreach (HIC_CHUKDTL_PLAN code in list)
                {
                    code.ENTSABUN = clsType.User.IdNumber.To<long>(0);

                    if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (code.WRTNO > 0)
                        {
                            hicChukDtlPlanRepository.Delete(code);
                        }
                    }
                    else
                    {
                        if (code.RID.IsNullOrEmpty())
                        {
                            code.WRTNO = argWRTNO;
                            hicChukDtlPlanRepository.InSert(code);
                        }
                        else
                        {
                            hicChukDtlPlanRepository.UpDate(code);
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

        public List<HIC_CHUKDTL_PLAN> GetListByWrtno(long argWRTNO, bool bDel = false)
        {
            return hicChukDtlPlanRepository.GetListByWrtno(argWRTNO, bDel);
        }

        public List<HIC_CHUKDTL_PLAN_SUGA> GetAccountByPlan(long nWRTNO, long nYear)
        {
            return hicChukDtlPlanRepository.GetAccountByPlan(nWRTNO, nYear);
        }

        public bool DeleteAll(long nWRTNO)
        {
            try
            {
                hicChukDtlPlanRepository.DeleteAll(nWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
