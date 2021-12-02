namespace HC_Measurement.Service
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukWorkerService
    {
        private HicChukWorkerRepository hicChukWorkerRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChukWorkerService()
        {
            this.hicChukWorkerRepository = new HicChukWorkerRepository();
        }

        public List<HIC_CHUK_WORKER> GetListByWrtno(long fnWRTNO)
        {
            return hicChukWorkerRepository.GetListByWrtno(fnWRTNO);
        }

        public bool Save(IList<HIC_CHUK_WORKER> list, long argWRTNO)
        {
            foreach (HIC_CHUK_WORKER code in list)
            {
                if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                {
                    if (code.WORKER_SABUN > 0)
                    {
                        code.WRTNO = argWRTNO;
                        code.ENTDATE = DateTime.Now;
                        code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                        hicChukWorkerRepository.Insert(code);
                    }
                }
                else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                {
                    hicChukWorkerRepository.Delete(code);
                }
                else
                {
                    if (code.RID.To<string>("").Trim() != "")
                    {
                        code.ENTDATE = DateTime.Now;
                        code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                        hicChukWorkerRepository.UpdatebyRowId(code);
                    }
                }
            }

            return true;
        }

        public bool DeleteAll(long nWRTNO)
        {
            try
            {
                hicChukWorkerRepository.DeleteAll(nWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
