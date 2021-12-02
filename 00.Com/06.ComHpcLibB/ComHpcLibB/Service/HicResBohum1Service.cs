namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum1Service
    {
        
        private HicResBohum1Repository hicResBohum1Repository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum1Service()
        {
			this.hicResBohum1Repository = new HicResBohum1Repository();
        }

        public HIC_RES_BOHUM1 GetItemByWrtno(long argWRTNO)
        {
            return hicResBohum1Repository.GetItemByWrtno(argWRTNO);
        }

        public int DeletebyWrtNo(long fnWRTNO)
        {
            return hicResBohum1Repository.DeletebyWrtNo(fnWRTNO);
        }

        public int Insert(long fnWRTNO)
        {
            return hicResBohum1Repository.Insert(fnWRTNO);
        }

        public int UpdateResultbyWrtNo(HIC_RES_BOHUM1 item, string argGbn)
        {
            return hicResBohum1Repository.UpdateResultbyWrtNo(item, argGbn);
        }

        public int UpdateOldByengbyWrtNo(long wRTNO, string strOldByeng1)
        {
            return hicResBohum1Repository.UpdateOldByengbyWrtNo(wRTNO, strOldByeng1);
        }

        public int GetCountbyWrtNo(long argWRTNO)
        {
            return hicResBohum1Repository.GetCountbyWrtNo(argWRTNO);
        }

        public string GetCountLife1thbyWrtNo(long argWRTNO)
        {
            return hicResBohum1Repository.GetCountLife1thbyWrtNo(argWRTNO);
        }

        public int UpdateMunjinDrNobyWrtNo(long gnHicLicense, long nWRTNO, string argMunDate = "")
        {
            return hicResBohum1Repository.UpdateMunjinDrNobyWrtNo(gnHicLicense, nWRTNO, argMunDate);
        }

        public HIC_RES_BOHUM1 GetMunjinDrNobyWrtNo(long fnWRTNO)
        {
            return hicResBohum1Repository.GetMunjinDrNobyWrtNo(fnWRTNO);
        }

        public HIC_RES_BOHUM1 GetPanjengDatebyWrtNo(long nWRTNO)
        {
            return hicResBohum1Repository.GetPanjengDatebyWrtNo(nWRTNO);
        }

        public HIC_RES_BOHUM1 GetItemByWrtnoPanjeng(long nWrtNo, List<string> strDAT, string strChkFirst1, string strChkFirst2)
        {
            return hicResBohum1Repository.GetItemByWrtnoPanjeng(nWrtNo, strDAT, strChkFirst1, strChkFirst2);
        }

        public HIC_RES_BOHUM1 GetIetmbyWrtNo(long fnWrtNo)
        {
            return hicResBohum1Repository.GetIetmbyWrtNo(fnWrtNo);
        }

        public int UpdateSlipbyWrtNo(string strSLIP1, string strSLIP2, string strSLIP3, string strSLIP4, string strSLIP5, string strSLIP6, string strSLIP7, long fnWrtNo)
        {
            return hicResBohum1Repository.UpdateSlipbyWrtNo(strSLIP1, strSLIP2, strSLIP3, strSLIP4, strSLIP5, strSLIP6, strSLIP7, fnWrtNo);
        }

        public int UpdateHabitbyWrtNo(HIC_RES_BOHUM1 item)
        {
            return hicResBohum1Repository.UpdateHabitbyWrtNo(item);
        }

        public int UpdateAllbyWrtNo(HIC_RES_BOHUM1 item1)
        {
            return hicResBohum1Repository.UpdateAllbyWrtNo(item1);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return hicResBohum1Repository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public int UpdateGbPanjengbyWrtNo(long fnWrtNo, string strOK)
        {
            return hicResBohum1Repository.UpdateGbPanjengbyWrtNo(fnWrtNo, strOK);
        }

        public List<HIC_RES_BOHUM1> GetAllByWrtno(long fnWrtNo)
        {
            return hicResBohum1Repository.GetAllByWrtno(fnWrtNo);
        }

        public int UpdateLifebyWrtNo(HIC_RES_BOHUM1 item)
        {
            return hicResBohum1Repository.UpdateLifebyWrtNo(item);
        }

        public int UpdateHicPanjengbyWrtNo(HIC_RES_BOHUM1 item)
        {
            return hicResBohum1Repository.UpdateHicPanjengbyWrtNo(item);
        }

        public int UpdateOnlyuHabitbyWrtNo(HIC_RES_BOHUM1 item1)
        {
            return hicResBohum1Repository.UpdateOnlyuHabitbyWrtNo(item1);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicResBohum1Repository.UpdateWrtNobyFWrtNo(nWrtNo, fnWrtNo);
        }

        public int UpdateLiverCbyWrtNo(HIC_RES_BOHUM1 item )
        {
            return hicResBohum1Repository.UpdateLiverCbyWrtNo(item);
        }

        public string GetItemBywrtno(long fnWrtNo)
        {
            return hicResBohum1Repository.GetItemBywrtno(fnWrtNo);
        }

        public bool UpdateNotPanjengbyWrtNo(long fnWRTNO)
        {
            try
            {
                hicResBohum1Repository.UpdateNotPanjengbyWrtNo(fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            try
            {
                hicResBohum1Repository.UpdatePanjengInfobyWrtNo(fnWRTNO, strDate, nDrNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateTongBoInfobyWrtNo(long fnWRTNO, string strDate, string strGbn, string strSabun)
        {
            try
            {
                hicResBohum1Repository.UpdateTongBoInfobyWrtNo(fnWRTNO, strDate, strGbn, strSabun);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public long GetPanjengDrNobyWrtNo(long fnWrtNo)
        {
            return hicResBohum1Repository.GetPanjengDrNobyWrtNo(fnWrtNo);
        }

        public HIC_RES_BOHUM1 GetPanjengDatebyWrtno(long wRTNO, string strChasu)
        {
            return hicResBohum1Repository.GetPanjengDatebyWrtno(wRTNO, strChasu);
        }

        public int UpdateAll(HIC_RES_BOHUM1 item)
        {
            return hicResBohum1Repository.UpdateAll(item);
        }
        public HIC_RES_BOHUM1 GetTongBoPanjengDateByWrtno(long argWRTNO)
        {
            return hicResBohum1Repository.GetTongBoPanjengDateByWrtno(argWRTNO);
        }

    }
}
