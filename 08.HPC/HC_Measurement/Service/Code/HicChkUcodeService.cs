namespace HC_Measurement.Service
{
    using HC_Measurement.Repository;
    using System.Collections.Generic;
    using HC_Measurement.Dto;
    using System;
    using System.Collections;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc.Spread;

    /// <summary>
    /// 
    /// </summary>
    public class HicChkUcodeService
    {
        private HicChkUcodeRepository hicChkUcodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicChkUcodeService()
        {
            this.hicChkUcodeRepository = new HicChkUcodeRepository();
        }

        public List<HIC_CHK_UCODE> GetItemAll()
        {
            return hicChkUcodeRepository.GetItemAll();
        }

        public bool Save(IList<HIC_CHK_UCODE> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HIC_CHK_UCODE code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        if (code.CODE.To<string>("").Trim() != "")
                        {
                            code.ENTDATE = DateTime.Now;
                            code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                            hicChkUcodeRepository.Insert(code);
                        }
                    }
                    else 
                    {
                        if (code.RID.To<string>("").Trim() != "")
                        {
                            code.ENTDATE = DateTime.Now;
                            code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                            hicChkUcodeRepository.UpdatebyRowId(code);
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        public SpreadComboBoxData GetListAllComboBoxData()
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CHK_UCODE> list = hicChkUcodeRepository.GetItemAll();

            foreach (HIC_CHK_UCODE code in list)
            {
                data.Put(code.CODE.Trim(), code.NAME.Trim());
            }
            return data;
        }
    }
}
