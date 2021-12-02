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
    public class HicChkMcodeService
    {
        private HicChkMcodeRepository hicChkMcodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChkMcodeService()
        {
            this.hicChkMcodeRepository = new HicChkMcodeRepository();
        }

        public List<HIC_CHK_MCODE> GetItemAll(string argKeyWord = "")
        {
            return hicChkMcodeRepository.GetItemAll(argKeyWord);
        }

        public HIC_CHK_MCODE GetItemByRid(string argRid)
        {
            return hicChkMcodeRepository.GetItemByRid(argRid);
        }

        public HIC_CHK_MCODE GetItemByCode(string argCode)
        {
            return hicChkMcodeRepository.GetItemByCode(argCode);
        }

        public bool Save(HIC_CHK_MCODE item)
        {
            try
            {
                if (item.RID.IsNullOrEmpty())
                {
                    hicChkMcodeRepository.Insert(item);
                }
                else
                {
                    hicChkMcodeRepository.Update(item);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public SpreadComboBoxData GetSpreadComboBoxData()
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CHK_MCODE> list = hicChkMcodeRepository.GetItemAll("");

            foreach (HIC_CHK_MCODE code in list)
            {
                data.Put(code.CODE, code.NAME);
            }
            return data;
        }
    }
}
