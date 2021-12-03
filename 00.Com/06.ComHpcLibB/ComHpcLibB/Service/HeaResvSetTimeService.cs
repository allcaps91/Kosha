
namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;
    using ComBase.Controls;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HeaResvSetTimeService
    {
        private HeaResvSetTimeRepository heaResvSetTimeRepository;
        /// <summary>
        /// 생성자 
        /// </summary>

        public HeaResvSetTimeService()
        {
            this.heaResvSetTimeRepository = new HeaResvSetTimeRepository();
        }

        public HEA_RESV_SET_TIME GetItemByGubun(string argDate, string argGubun)
        {
            return heaResvSetTimeRepository.GetItemByGubun(argDate, argGubun);
        }
        public HEA_RESV_SET_TIME GetItemBySTimeGubun(string argDate, string argSTime, string argGubun )
        {
            return heaResvSetTimeRepository.GetItemBySTimeGubun(argDate, argSTime, argGubun);
        }

        public List <HEA_RESV_SET_TIME> GetSumInwonAMPMByGubun(string argDate, string argGubun )
        {
            return heaResvSetTimeRepository.GetSumInwonAMPMByGubun(argDate, argGubun);
        }
        public bool DeleteBySDate(string argGetSDate, string argGetLDate)
        {
            try
            {
                heaResvSetTimeRepository.DeleteBySDate(argGetSDate, argGetLDate);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<HEA_RESV_SET_TIME> GetAmPmInwonByGubun(string argDate, string argGubun)
        {
            return heaResvSetTimeRepository.GetAmPmInwonByGubun(argDate, argGubun);
        }

        public int Insert(HEA_RESV_SET_TIME dto)
        {
            return heaResvSetTimeRepository.Insert(dto);
        }

        public int UpDate(HEA_RESV_SET_TIME dto)
        {
            return heaResvSetTimeRepository.UpDate(dto);
        }


    }
}
