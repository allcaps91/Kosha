namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResDentalService
    {
        
        private HicResDentalRepository hicResDentalRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResDentalService()
        {
			this.hicResDentalRepository = new HicResDentalRepository();
        }

        internal static HIC_RES_DENTAL GetItemByWRTNO(long argWRTNO)
        {
            throw new NotImplementedException();
        }

        public HIC_RES_DENTAL GetItemByWrtno(long argWRTNO)
        {
            return hicResDentalRepository.GetItemByWrtno(argWRTNO);
        }

        public int MunjinResultUpDate(HIC_RES_DENTAL item2)
        {
            return hicResDentalRepository.MunjinResultUpDate(item2);
        }

        public long GetPanjengDrNobyWrtNo(long wRTNO)
        {
            return hicResDentalRepository.GetPanjengDrNobyWrtNo(wRTNO);
        }

        public HIC_RES_DENTAL GetPanjengDrnoDate(long wRTNO)
        {
            return hicResDentalRepository.GetPanjengDrnoDate(wRTNO);
        }

        public int DeletebyWrtNo(long fnWRTNO)
        {
            return hicResDentalRepository.DeletebyWrtNo(fnWRTNO);
        }

        public int UpdatebyWrtNo(long fnWRTNO)
        {
            return hicResDentalRepository.UpdatebyWrtNo(fnWRTNO);
        }

        public int Insert(HIC_RES_DENTAL item)
        {
            return hicResDentalRepository.Insert(item);
        }

        public int UpdateAll(HIC_RES_DENTAL item)
        {
            return hicResDentalRepository.UpdateAll(item);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            return hicResDentalRepository.GetCountbyWrtNo(argWRTNO);
        }

        public HIC_RES_DENTAL GetItemByWrtnoPanjenGDrNo(long argWRTNO)
        {
            return hicResDentalRepository.GetItemByWrtnoPanjenGDrNo(argWRTNO);
        }

        public HIC_RES_DENTAL GetPanjengDatebyWrtNo(long nWRTNO)
        {
            return hicResDentalRepository.GetPanjengDatebyWrtNo(nWRTNO);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return hicResDentalRepository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public int UpdateBusikGyomobyWrtNo(string strGbn, long fnWRTNO)
        {
            return hicResDentalRepository.UpdateBusikGyomobyWrtNo(strGbn, fnWRTNO);
        }

        public int UpdateResultbyWrtNo(string strGbn, long fnWRTNO, string strResult, string strBusik, string strGyomo)
        {
            return hicResDentalRepository.UpdateResultbyWrtNo(strGbn, fnWRTNO, strResult, strBusik, strGyomo);
        }

        public int UpdateAllbyWrtNo(HIC_RES_DENTAL item)
        {
            return hicResDentalRepository.UpdateAllbyWrtNo(item);
        }

        public int UpdateDentSogenbyWrtNo(string strSpcPanjeng, long nPanDrNo, long fnWRTNO)
        {
            return hicResDentalRepository.UpdateDentSogenbyWrtNo(strSpcPanjeng, nPanDrNo, fnWRTNO);
        }

        public int UPdatePanjengDatebyWrtNo(string strPanjengDate, long fnWRTNO)
        {
            return hicResDentalRepository.UPdatePanjengDatebyWrtNo(strPanjengDate, fnWRTNO);
        }

        public int InsertWrtNo(long argWrtNo)
        {
            return hicResDentalRepository.InsertWrtNo(argWrtNo);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicResDentalRepository.UpdateWrtNobyFWrtNo(nWrtNo, fnWrtNo);
        }

        public List<HIC_RES_DENTAL> GetItemsbyWrtNo(long nWRTNO)
        {
            return hicResDentalRepository.GetItemsbyWrtNo(nWRTNO);
        }

        public bool UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            try
            {
                hicResDentalRepository.UpdatePanjengInfobyWrtNo(fnWRTNO, strDate, nDrNO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn)
        {
            try
            {
                hicResDentalRepository.UpdateTongBoInfobyWrtNo(fnWRTNO, strDate, strGbn);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_RES_DENTAL GetPanjengDateOpdDntDntStatusbyWrtNo(long wRTNO)
        {
            return hicResDentalRepository.GetPanjengDateOpdDntDntStatusbyWrtNo(wRTNO);
        }

        public int UpdateTongboByWrtno(long argWrtno, string argTongGbn)
        {
            return hicResDentalRepository.UpdateTongboByWrtno(argWrtno, argTongGbn);
        }
    }
}
