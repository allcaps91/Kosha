namespace HC_Measurement.Service
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc.Spread;
    using HC_Measurement.Dto;
    using HC_Measurement.Repository;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicChkSugaService
    {
        private HicChkSugaRepository hicChkSugaRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChkSugaService()
        {
            this.hicChkSugaRepository = new HicChkSugaRepository();
        }

        public List<HIC_CHK_SUGA> GetItemAll(long nYEAR, string argKeyWord = "")
        {
            return hicChkSugaRepository.GetItemAll(nYEAR, argKeyWord);
        }

        public bool Save(HIC_CHK_SUGA item)
        {
            try
            {
                if (item.RID.IsNullOrEmpty())
                {
                    hicChkSugaRepository.Insert(item);
                }
                else
                {
                    hicChkSugaRepository.Update(item);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public HIC_CHK_SUGA GetItemByRid(string argRid)
        {
            return hicChkSugaRepository.GetItemByRid(argRid);
        }

        public HIC_CHK_SUGA GetItemByCode(string argCode)
        {
            return hicChkSugaRepository.GetItemByCode(argCode);
        }

        public SpreadComboBoxData GetSpreadComboBoxData(long nYEAR)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CHK_SUGA> list = hicChkSugaRepository.GetItemAll(nYEAR, "");

            foreach (HIC_CHK_SUGA code in list)
            {
                data.Put(code.SUCODE, code.SUNAME);
            }
            return data;
        }
    }
}
