namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class HicResSpecialService
    {
        
        private HicResSpecialRepository hicResSpecialRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicResSpecialService()
        {
			this.hicResSpecialRepository = new HicResSpecialRepository();
        }

        public string Read_Res_Special(long nWrtNo)
        {
            return hicResSpecialRepository.Read_Res_Special(nWrtNo);
        }

        public int Delete(long argWRTNO)
        {
            return hicResSpecialRepository.Delete(argWRTNO);
        }

        public int UCodeName_UpDate(string strRowid, string uCODES)
        {
            return hicResSpecialRepository.UCodeName_UpDate(strRowid, uCODES);
        }

        public int UCodeCount_UpDate(long argWRTNO, int nUCodeCNT)
        {
            return hicResSpecialRepository.UCodeCount_UpDate(argWRTNO, nUCodeCNT);
        }

        public HIC_RES_SPECIAL GetItemByWrtno(long wRTNO)
        {
            return hicResSpecialRepository.GetItemByWrtno(wRTNO);
        }

        public string GetTmunAllbyWrtNo(long nWrtNo)
        {
            return hicResSpecialRepository.GetTmunAllbyWrtNo(nWrtNo);
        }

        public int UpdateAllbyWrtNo(HIC_RES_SPECIAL item, string argGbn)
        {
            return hicResSpecialRepository.UpdateAllbyWrtNo(item, argGbn);
        }

        public int UpdateMunjinbyWrtNo(long nWrtNo, string strMunjin1, string strMunjin2, string strMunjin3, string strMunjin4, string strMunjin5, string strMunjin6, string strMunjin7
                                       , string strMunjin8, string strMunjin9, string strMunjin10, string strMunjin11, string strMunjin12, string strMunjin13, string strMunjin14)
        {
            return hicResSpecialRepository.UpdateMunjinbyWrtNo(nWrtNo, strMunjin1, strMunjin2, strMunjin3, strMunjin4, strMunjin5, strMunjin6, strMunjin7, strMunjin8
                                                               , strMunjin9, strMunjin10, strMunjin11, strMunjin12, strMunjin13, strMunjin14);
        }

        public int UpdateMunjinToKbyWrtNo(long nWrtNo, string strMunjinT1, string strMunjinT2, string strMunjinT3, string strMunjinT4, string strMunjinT5, string strMunjinT6, string strMunjinT7
                                       , string strMunjinT8, string strMunjinT9, string strMunjinT10, string strMunjinT11)
        {
            return hicResSpecialRepository.UpdateMunjinToKbyWrtNo(nWrtNo, strMunjinT1, strMunjinT2, strMunjinT3, strMunjinT4, strMunjinT5, strMunjinT6, strMunjinT7, strMunjinT8
                                                               , strMunjinT9, strMunjinT10, strMunjinT11);
        }

        public int UpdateLungbyWrtNo(HIC_RES_SPECIAL item)
        {
            return hicResSpecialRepository.UpdateLungbyWrtNo(item);
        }

        public HIC_RES_SPECIAL GetItembyWrtNo(long nWrtNo)
        {
            return hicResSpecialRepository.GetItembyWrtNo(nWrtNo);
        }

        public int UpdatebyWrtNo(long nWrtNo)
        {
            return hicResSpecialRepository.UpdatebyWrtNo(nWrtNo);
        }

        public string GetGbOhmsbyWrtNo(long fnWRTNO)
        {
            return hicResSpecialRepository.GetGbOhmsbyWrtNo(fnWRTNO);
        }

        public HIC_RES_SPECIAL GetPanjengDatebyWrtNo(long nWRTNO)
        {
            return hicResSpecialRepository.GetPanjengDatebyWrtNo(nWRTNO);
        }

        public int UpdateGbOhmabyWrtNo(long nWrtNo)
        {
            return hicResSpecialRepository.UpdateGbOhmabyWrtNo(nWrtNo);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            return hicResSpecialRepository.GetRowIdbyWrtNo(fnWRTNO);
        }

        public bool UpDaterJinDrnoByWrtno(long nDrNO, long fnWRTNO)
        {
            try
            {
                hicResSpecialRepository.UpDaterJinDrnoByWrtno(nDrNO, fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Insert(HIC_RES_SPECIAL nHRS)
        {
            return hicResSpecialRepository.Insert(nHRS);
        }

        public HIC_RES_SPECIAL GetDentSogenbyWrtNo(long fnWRTNO)
        {
            return hicResSpecialRepository.GetDentSogenbyWrtNo(fnWRTNO);
        }

        public int UpdateAllbyWrtNo(HIC_RES_SPECIAL item2)
        {
            return hicResSpecialRepository.UpdateAllbyWrtNo(item2);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            return hicResSpecialRepository.GetCountbyWrtNo(argWrtNo);
        }

        public int UPdateSpecialbyWrtNo(HIC_RES_SPECIAL item)
        {
            return hicResSpecialRepository.UPdateSpecialbyWrtNo(item);
        }

        public HIC_RES_SPECIAL GetPanjengDrNoDatebyWrtNo(long fnWrtNo)
        {
            return hicResSpecialRepository.GetPanjengDrNoDatebyWrtNo(fnWrtNo);
        }

        public int UpdatePanjengDrNoDatebyWrtNo(long fnWrtNo, long nPanDrNo1)
        {
            return hicResSpecialRepository.UpdatePanjengDrNoDatebyWrtNo(fnWrtNo, nPanDrNo1);
        }

        public int UpdatePanjengDatebyWrtNo(long fnWrtNo)
        {
            return hicResSpecialRepository.UpdatePanjengDatebyWrtNo(fnWrtNo);
        }

        public int UpdateUcodesbyRowId(string strRowId, string strUCodes)
        {
            return hicResSpecialRepository.UpdateUcodesbyRowId(strRowId, strUCodes);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicResSpecialRepository.UpdateWrtNobyFWrtNo(nWrtNo, fnWrtNo);
        }

        public bool UpDateMunjinbyItem(HIC_RES_SPECIAL nHRS)
        {
            try
            {
                hicResSpecialRepository.UpDateMunjinbyItem(nHRS);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertWrtNo(long fnWRTNO)
        {
            return hicResSpecialRepository.InsertWrtNo(fnWRTNO);
        }

        public bool UpDateGbSpcByWrtno(string argGbSpc, long nWRTNO)
        {
            try
            {
                hicResSpecialRepository.UpDateGbSpcByWrtno(argGbSpc, nWRTNO);

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
                hicResSpecialRepository.UpdatePanjengInfobyWrtNo(fnWRTNO, strDate, nDrNO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int UpdatePRINTbyWrtNo(long nWRTNO, long nSABUN)
        {
            return hicResSpecialRepository.UpdatePRINTbyWrtNo(nWRTNO, nSABUN);
        }

        public int UpdateDentSogenbyWrtNo(string strSpcPanjeng, long nPanDrNo, long fnWRTNO)
        {
            return hicResSpecialRepository.UpdateDentSogenbyWrtNo(strSpcPanjeng, nPanDrNo, fnWRTNO);
        }

        public HIC_RES_SPECIAL GetPanTongDateByWrtno(long fnWrtNo)
        {
            return hicResSpecialRepository.GetPanTongDateByWrtno(fnWrtNo);
        } 
    }
}
