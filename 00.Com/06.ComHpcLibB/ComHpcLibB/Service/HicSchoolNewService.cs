namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSchoolNewService
    {
        
        private HicSchoolNewRepository hicSchoolNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSchoolNewService()
        {
			this.hicSchoolNewRepository = new HicSchoolNewRepository();
        }

        public int UpdatebyWrtNo(HIC_SCHOOL_NEW item)
        {
            return hicSchoolNewRepository.UpdatebyWrtNo(item);
        }

        public List<HIC_SCHOOL_NEW> GetItembyWrtNo(long fnWRTNO)
        {
            return hicSchoolNewRepository.GetItembyWrtNo(fnWRTNO);
        }

        public int UpdateGbDntPrtbyWrtNo(string idNumber, long nWrtNo, string strGbGubun)
        {
            return hicSchoolNewRepository.UpdateGbDntPrtbyWrtNo(idNumber, nWrtNo, strGbGubun);
        }

        public int UpdateGbMirPrintbyWrtNo(List<string> fstrWRTNO)
        {
            return hicSchoolNewRepository.UpdateGbMirPrintbyWrtNo(fstrWRTNO);
        }

        public HIC_SCHOOL_NEW GetDPanDrNobySDate(string strFrDate, string strToDate, long nLtdCode1, string strClass)
        {
            return hicSchoolNewRepository.GetDPanDrNobySDate(strFrDate, strToDate, nLtdCode1, strClass);
        }

        public string GetRowIdbyWrtNo(long argWrtNo)
        {
            return hicSchoolNewRepository.GetRowIdbyWrtNo(argWrtNo);
        }

        public int Insert(HIC_SCHOOL_NEW item)
        {
            return hicSchoolNewRepository.Insert(item);
        }

        public int UpdatebyRowId(HIC_SCHOOL_NEW item)
        {
            return hicSchoolNewRepository.UpdatebyRowId(item);
        }

        public int UpdatebySchPanWrtNo(HIC_SCHOOL_NEW item)
        {
            return hicSchoolNewRepository.UpdatebySchPanWrtNo(item);
        }

        public int UpdatebyPpanBWrtNo(HIC_SCHOOL_NEW item)
        {
            return hicSchoolNewRepository.UpdatebyPpanBWrtNo(item);
        }

        public int InsertWrtNo(long argWrtNo)
        {
            return hicSchoolNewRepository.InsertWrtNo(argWrtNo);
        }

        public HIC_SCHOOL_NEW GetItembyWrtNoSingle(long nWRTNO)
        {
            return hicSchoolNewRepository.GetItembyWrtNoSingle(nWRTNO);
        }

        public HIC_SCHOOL_NEW GetDPPanDrNoPPanDrNobyWrtNo(long nWRTNO)
        {
            return hicSchoolNewRepository.GetDPPanDrNoPPanDrNobyWrtNo(nWRTNO);
        }

        public bool UpDateItembyItem(HIC_SCHOOL_NEW nHSN)
        {
            try
            {
                hicSchoolNewRepository.UpDateItembyItem(nHSN);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_SCHOOL_NEW GetPanjengDateByWrtno(long argWrtno)
        {
            return hicSchoolNewRepository.GetPanjengDateByWrtno(argWrtno);
        }
    }
}
