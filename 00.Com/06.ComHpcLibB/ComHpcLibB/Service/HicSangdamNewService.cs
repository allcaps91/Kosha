namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSangdamNewService
    {
        
        private HicSangdamNewRepository hicSangdamNewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamNewService()
        {
			this.hicSangdamNewRepository = new HicSangdamNewRepository();
        }

        public HIC_SANGDAM_NEW SelOneData(long argWrtNo)
        {
            return hicSangdamNewRepository.SelOneData(argWrtNo);
        }

        public List<HIC_SANGDAM_NEW> GetPanoCntbyDrNo(string strDrNo)
        {
            return hicSangdamNewRepository.GetPanoCntbyDrNo(strDrNo);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public int UpdateHabitbyWrtNo(HIC_RES_BOHUM1 item)
        {
            return hicSangdamNewRepository.UpdateHabitbyWrtNo(item);
        }

        public HIC_SANGDAM_NEW GetGbstsRowIdbyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetGbstsRowIdbyWrtNo(fnWRTNO);
        }

        public HIC_SANGDAM_NEW GetHabitbyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetHabitbyWrtNo(fnWRTNO);
        }

        public string GetRemarkbyWrtNo(long nWrtNo)
        {
            return hicSangdamNewRepository.GetRemarkbyWrtNo(nWrtNo);
        }

        public HIC_SANGDAM_NEW GetGbStsRowIdbyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetGbStsRowIdbyWrtNo(fnWRTNO);
        }

        public int Insert(HIC_SANGDAM_NEW item)
        {
            return hicSangdamNewRepository.Insert(item);
        }

        public int UpdatebyWrtNo(HIC_SANGDAM_NEW item)
        {
            return hicSangdamNewRepository.UpdatebyWrtNo(item);
        }

        public int UpdateDiabetesbyWrtNo(HIC_SANGDAM_NEW item)
        {
            return hicSangdamNewRepository.UpdateDiabetesbyWrtNo(item);
        }

        public int UpdateJinchal2byWrtNo(HIC_SANGDAM_NEW item)
        {
            return hicSangdamNewRepository.UpdateJinchal2byWrtNo(item);
        }

        public int UpdatSchPanbyWrtNo(HIC_SANGDAM_NEW item2)
        {
            return hicSangdamNewRepository.UpdatSchPanbyWrtNo(item2);
        }

        public int UpdateRemarkbyWrtNo(HIC_SANGDAM_NEW item2)
        {
            return hicSangdamNewRepository.UpdateRemarkbyWrtNo(item2);
        }

        public HIC_SANGDAM_NEW GetItembyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetItembyWrtNo(fnWRTNO);
        }

        public HIC_SANGDAM_NEW GetAllbyWrtNo(long argWrtNo)
        {
            return hicSangdamNewRepository.GetAllbyWrtNo(argWrtNo);
        }

        public long GetSangdamDrNobyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetSangdamDrNobyWrtNo(fnWRTNO);
        }

        public HIC_SANGDAM_NEW GetSangdamDrNoAmSangdambyWrtNo(long fnWRTNO)
        {
            return hicSangdamNewRepository.GetSangdamDrNoAmSangdambyWrtNo(fnWRTNO);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            return hicSangdamNewRepository.GetCountbyWrtNo(argWrtNo);
        }

        public int UpdateHabitGbChkbyWrtNo(HIC_SANGDAM_NEW item)
        {
            return hicSangdamNewRepository.UpdateHabitGbChkbyWrtNo(item);
        }

        public bool UpDateSangdamDrno(long nDrno, long fnWRTNO)
        {
            try
            {
                hicSangdamNewRepository.UpDateSangdamDrno(nDrno, fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int UpdateWrtNoGjJongbyWrtNo(long nWrtNo, string strJong, long fnWrtNo)
        {
            return hicSangdamNewRepository.UpdateWrtNoGjJongbyWrtNo(nWrtNo, strJong, fnWrtNo);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicSangdamNewRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public HIC_SANGDAM_NEW GetPjSangdambyWrtNo(long nWRTNO)
        {
            return hicSangdamNewRepository.GetPjSangdambyWrtNo(nWRTNO);
        }

        public int GetCountbyWrtNoSangdamDrNo(long argWrtNo)
        {
            return hicSangdamNewRepository.GetCountbyWrtNoSangdamDrNo(argWrtNo);
        }
    }
}
