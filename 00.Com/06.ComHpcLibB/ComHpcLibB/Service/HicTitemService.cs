namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicTitemService
    {

        private HicTitemRepository hicTitemRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicTitemService()
        {
            this.hicTitemRepository = new HicTitemRepository();
        }

        public List<HIC_TITEM> GetJumsuCodebyWrtNo(long fnWrtNo)
        {
            return hicTitemRepository.GetJumsuCodebyWrtNo(fnWrtNo);
        }

        public string GetCodebyWrtNo(long fnWrtNo)
        {
            return hicTitemRepository.GetCodebyWrtNo(fnWrtNo);
        }

        public long GetTotalbyGubunWrtNo(int i, long nWRTNO)
        {
            return hicTitemRepository.GetTotalbyGubunWrtNo(i, nWRTNO);
        }

        public int GetCountbyWrtNo(long nWRTNO)
        {
            return hicTitemRepository.GetCountbyWrtNo(nWRTNO);
        }

        public int GetCountbyWrtNoGubunCode(long fnWrtNo, string strGubun, string[] strCodes)
        {
            return hicTitemRepository.GetCountbyWrtNoGubunCode(fnWrtNo, strGubun, strCodes);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicTitemRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public string GetItembyWrtNo(long nWrtNo)
        {
            return hicTitemRepository.GetItembyWrtNo(nWrtNo);
        }

        public List<HIC_TITEM> GetGubunCodeJumsubyWrtNo(long argWrtNo, string argGubun, long argJumsu, string sGbn)
        {
            return hicTitemRepository.GetGubunCodeJumsubyWrtNo(argWrtNo, argGubun, argJumsu, sGbn);
        }

        public List<HIC_TITEM> GetGubunCodeJumsubyWrtNo(long argWrtno)
        {
            return hicTitemRepository.GetGubunCodeJumsubyWrtNo(argWrtno);
        }

        public int DeletebyWrtNoGuybun(long nWRTNO, int nGubun)
        {
            return hicTitemRepository.DeletebyWrtNoGuybun(nWRTNO, nGubun);
        }

        public HIC_TITEM GetRowIdbyGubun(int strGubun, string strCODE, long nWRTNO, long fnPano)
        {
            return hicTitemRepository.GetRowIdbyGubun(strGubun, strCODE, nWRTNO, fnPano);
        }

        public int Insert(long nWRTNO, string strGubun, string strCODE, long nJumsu, long fnPano)
        {
            return hicTitemRepository.Insert(nWRTNO, strGubun, strCODE, nJumsu, fnPano);
        }

        public int Delete(string strROWID)
        {
            return hicTitemRepository.Delete(strROWID);
        }

        public int Update(string strCODE, long nWRTNO, long fnPano, long nJumsu, string strROWID)
        {
            return hicTitemRepository.Update(strCODE, nWRTNO, fnPano, nJumsu, strROWID);
        }

        public int UpdateCodePaNobyWrtNoPaNo(string strCode, long fnPano, long nWRTNO, string strGubun, int nJumsu)
        {
            return hicTitemRepository.UpdateCodePaNobyWrtNoPaNo(strCode, fnPano, nWRTNO, strGubun, nJumsu);
                                                        
        } 

        public List<HIC_TITEM> GetRowId(long nWRTNO, long FnPano)
        {
            return hicTitemRepository.GetRowId(nWRTNO, FnPano);
        }

        public int DeletebyWrtNoPaNo(long nWRTNO, long fnPano, string strGubun, int nJumsu)
        {
            return hicTitemRepository.DeletebyWrtNoPaNo(nWRTNO, fnPano, strGubun, nJumsu);
        }

        public List<HIC_TITEM> GetRowIdbyGubunWrtNoPaNo(string strGubun, int nJumsu, long nWRTNO, long fnPano)
        {
            return hicTitemRepository.GetRowIdbyGubunWrtNoPaNo(strGubun, nJumsu, nWRTNO, fnPano);
        }

        public string GetRowIdRowIdbyWrtNo(long fnWRTNO)
        {
            return hicTitemRepository.GetRowIdRowIdbyWrtNo(fnWRTNO);
        }

        public HIC_TITEM GetJumsubyGubunWrtNo(int i, long nWRTNO)
        {
            return hicTitemRepository.GetJumsubyGubunWrtNo(i, nWRTNO);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunJumsu(long nWRTNO)
        {
            return hicTitemRepository.GetItembyWrtNoGubunJumsu(nWRTNO);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunCode(long fnWrtNo, string strGubun)
        {
            return hicTitemRepository.GetItembyWrtNoGubunCode(fnWrtNo, strGubun);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubun(long argWRTNO, string argGubun)
        {
            return hicTitemRepository.GetItembyWrtNoGubun(argWRTNO, argGubun);
        }

        public List<HIC_TITEM> GetItembyWrtNoGubunJumsu2(long argWRTNO, string argGubun)
        {
            return hicTitemRepository.GetItembyWrtNoGubunJumsu2(argWRTNO, argGubun);
        }
    }
}
