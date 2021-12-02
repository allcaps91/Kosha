namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HeaResvSetService
    {
        
        private HeaResvSetRepository heaResvSetRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaResvSetService()
        {
			this.heaResvSetRepository = new HeaResvSetRepository();
        }

        public HEA_RESV_SET GetItemByGubun(string argDate, string argGubun)
        {
            return heaResvSetRepository.GetItemByGubun(argDate, argGubun);
        }

        public HEA_RESV_SET GetSumGaInwonAMPMByGubun(string argDate, string argGubun)
        {
            return heaResvSetRepository.GetSumGaInwonAMPMByGubun(argDate, argGubun);
        }

        public List<HEA_RESV_SET> GetListByInwonSet(string argDate, string argResv, string argGubun)
        {
            return heaResvSetRepository.GetListByInwonSet(argDate, argResv, argGubun);
        }

        public HEA_RESV_SET GetCountByGubunYoil(string argDate, string argResv, string argGubun, string argYoil)
        {
            return heaResvSetRepository.GetCountByGubunYoil(argDate, argResv, argGubun, argYoil);
        }

        public int Insert(HEA_RESV_SET dto)
        {
            return heaResvSetRepository.Insert(dto);
        }

        public int UpDate(HEA_RESV_SET dto)
        {
            return heaResvSetRepository.UpDate(dto);
        }

        public HEA_RESV_SET GetAmPmInwonByGubun(string argDate, string argResv, string argGubun, string argYoil = "")
        {
            return heaResvSetRepository.GetAmPmInwonByGubun(argDate, argResv, argGubun, argYoil);
        }

        public HEA_RESV_SET GetSumInwonAMPMByGubun(string argDate, string argGubun)
        {
            return heaResvSetRepository.GetSumInwonAMPMByGubun(argDate, argGubun);
        }

        public List<HEA_RESV_SET> GetListGaInwonBySDate(string argDate, string argGbn, int argRow)
        {
            return heaResvSetRepository.GetListGaInwonBySDate(argDate, argGbn, argRow);
        }

        public IList<HEA_RESV_SET> GetSumGaInwonSDateBySDateGbResv(string argGetSDate, string argGetLDate, string argGubun)
        {
            return heaResvSetRepository.GetSumGaInwonSDateBySDateGbResv(argGetSDate, argGetLDate, argGubun);
        }

        public long GetSumAmPmInwonBySDate(string argDate, string argGubun)
        {
            return heaResvSetRepository.GetSumAmPmInwonBySDate(argDate, argGubun);
        }

        public bool Save(IList<HEA_RESV_SET> list, string fstrCurDate, string fstrGbExam)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HEA_RESV_SET code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        if (!code.EXAMNAME.IsNullOrEmpty())
                        {
                            code.SDATE = fstrCurDate;
                            code.GUBUN = fstrGbExam;
                            code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                            code.GBRESV = "2";
                            heaResvSetRepository.Insert(code);
                        }
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        if (!code.RID.IsNullOrEmpty())
                        {
                            code.GBRESV = "2";
                            code.ENTSABUN = clsType.User.IdNumber.To<long>(0);
                            heaResvSetRepository.UpDate(code);
                        }
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        if (!code.RID.IsNullOrEmpty())
                        {
                            heaResvSetRepository.Delete(code);
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        public List<HEA_RESV_SET> GetListBySDateGubun(string strRDate, string strGb)
        {
            return heaResvSetRepository.GetListBySDateGubun(strRDate, strGb);
        }

        public bool DeleteBySDate(string argGetSDate, string argGetLDate)
        {
            try
            {
                heaResvSetRepository.DeleteBySDate(argGetSDate, argGetLDate);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HEA_RESV_SET> GetItembyYDateLtdCode(string strFDate, string strTDate, string strLtdCode)
        {
            return heaResvSetRepository.GetItembyYDateLtdCode(strFDate, strTDate, strLtdCode);
        }
    }
}
