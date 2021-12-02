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
    public class HicSpcPanjengService
    {
        
        private HicSpcPanjengRepository hicSpcPanjengRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengService()
        {
			this.hicSpcPanjengRepository = new HicSpcPanjengRepository();
        }

        
        public string Read_HicCode2_SCode(long argWrtNo, string argSogenCode)
        {
            return hicSpcPanjengRepository.Read_HicCode2_SCode(argWrtNo, argSogenCode);
        }

        public int All_Del_UpDate(long argWRTNO)
        {
            return hicSpcPanjengRepository.All_Del_UpDate(argWRTNO);
        }

        public IList<HIC_SPC_PANJENG> Read_Spc_Panjeng(long argWRTNO)
        {
            return hicSpcPanjengRepository.Read_Spc_Panjeng(argWRTNO);
        }

        public int OneDelUpDate(string gstrSysDate, string rOWID)
        {
            return hicSpcPanjengRepository.OneDelUpDate(gstrSysDate, rOWID);
        }

        public string FindRid(long argWRTNO, string strMCode = "")
        {
            return hicSpcPanjengRepository.FindRid(argWRTNO, strMCode);
        }

        public int Insert(HIC_SPC_PANJENG item2)
        {
            return hicSpcPanjengRepository.Insert(item2);
        }

        public string Read_Spc_Panjeng_YN(long argWrtNo)
        {
            return hicSpcPanjengRepository.Read_Spc_Panjeng_YN(argWrtNo);
        }

        public int ChasuUpDate(long argWRTNO)
        {
            return hicSpcPanjengRepository.ChasuUpDate(argWRTNO);
        }

        public int GetCountbyWrtNo(long fnWRTNO)
        {
            return hicSpcPanjengRepository.GetCountbyWrtNo(fnWRTNO);
        }

        public int GetCountbyWrtNo2(long fnWRTNO)
        {
            return hicSpcPanjengRepository.GetCountbyWrtNo2(fnWRTNO);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoWrtNo2(long fnWRTNO, long fnWrtno2, string strGubun)
        {
            return hicSpcPanjengRepository.GetItembyWrtNoWrtNo2(fnWRTNO, fnWrtno2, strGubun);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long fnWrtno, long fnWrtno2, string strGubun)
        {
            return hicSpcPanjengRepository.GetItembyWrtNo(fnWrtno, fnWrtno2, strGubun);
        }

        public long GetPanjengDrNobyWrtNo2(long fnWrtno2)
        {
            return hicSpcPanjengRepository.GetPanjengDrNobyWrtNo2(fnWrtno2);
        }

        public List<HIC_SPC_PANJENG> GetPanjengbyWrtNo(long fnWRTNO)
        {
            return hicSpcPanjengRepository.GetPanjengbyWrtNo(fnWRTNO);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo2(long nWrtno2)
        {
            return hicSpcPanjengRepository.GetItembyWrtNo2(nWrtno2);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long nWRTNO)
        {
            return hicSpcPanjengRepository.GetItembyWrtNo(nWRTNO);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtno1Wrtno2(long fnWrtno1, long fnWrtno2)
        {
            return hicSpcPanjengRepository.GetItembyWrtno1Wrtno2(fnWrtno1, fnWrtno2);
        }

        public HIC_SPC_PANJENG GetItembyRowId(string fstrPROWID)
        {
            return hicSpcPanjengRepository.GetItembyRowId(fstrPROWID);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo1WrtNo2(long fnWRTNO, long fnWrtno1, long fnWrtno2)
        {
            return hicSpcPanjengRepository.GetItembyWrtNo1WrtNo2(fnWRTNO, fnWrtno1, fnWrtno2);
        }

        public int UpdateDelDatebyRowId(string strRowId)
        {
            return hicSpcPanjengRepository.UpdateDelDatebyRowId(strRowId);
        }

        public int SelectInsert(string strCode, long gnWRTNO)
        {
            return hicSpcPanjengRepository.SelectInsert(strCode, gnWRTNO);
        }

        public long GetPanjengDrNobyWrtNo(long fnWRTNO)
        {
            return hicSpcPanjengRepository.GetPanjengDrNobyWrtNo(fnWRTNO);
        }

        public List<HIC_SPC_PANJENG> GetPanjengSahuCodebyWrtNo(long fnWRTNO)
        {
            return hicSpcPanjengRepository.GetPanjengSahuCodebyWrtNo(fnWRTNO);
        }

        public List<HIC_SPC_PANJENG> GeResulttItembyWrtNoWrtNo2(long fnWRTNO, long fnWrtno2, string strTemp)
        {
            return hicSpcPanjengRepository.GeResulttItembyWrtNoWrtNo2(fnWRTNO, fnWrtno2, strTemp);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoWrtNo3(long fnWRTNO, long fnWrtno2)
        {
            return hicSpcPanjengRepository.GetItembyWrtNoWrtNo3(fnWRTNO, fnWrtno2);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoUnionWrtNo2(long fnWRTNO, long fnWrtno2, string strTemp)
        {
            return hicSpcPanjengRepository.GetItembyWrtNoUnionWrtNo2(fnWRTNO, fnWrtno2, strTemp);
        }

        public HIC_SPC_PANJENG GetAllbyWrtNo(long fnWrtNo)
        {
            return hicSpcPanjengRepository.GetAllbyWrtNo(fnWrtNo);
        }

        public int UpdateDelDatebyWrtNo(long fnWrtNo)
        {
            return hicSpcPanjengRepository.UpdateDelDatebyWrtNo(fnWrtNo);
        }

        public List<HIC_SPC_PANJENG> GetMCodebyWrtNo(long fnWrtNo)
        {
            return hicSpcPanjengRepository.GetMCodebyWrtNo(fnWrtNo);
        }

        public List<HIC_SPC_PANJENG> GetAllbyWrtNoList(long fnWrtNo)
        {
            return hicSpcPanjengRepository.GetAllbyWrtNoList(fnWrtNo);
        }

        public int InsertPanjeng(long fnWrtNo, string strCode, string strUCode)
        {
            return hicSpcPanjengRepository.InsertPanjeng(fnWrtNo, strCode, strUCode);
        }

        public int UpdateAllbyWrtNoMCodePyojanggi(string fstrSaveGbn, HIC_SPC_PANJENG item)
        {
            return hicSpcPanjengRepository.UpdateAllbyWrtNoMCodePyojanggi(fstrSaveGbn, item);
        }
        public int UpdateAllbyWrtNoMCodePyojanggi1(string fstrSaveGbn, HIC_SPC_PANJENG item)
        {
            return hicSpcPanjengRepository.UpdateAllbyWrtNoMCodePyojanggi1(fstrSaveGbn, item);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicSpcPanjengRepository.UpdateWrtNobyFWrtNo(nWrtNo, fnWrtNo);
        }

        public bool UpDateDelDateByWrtnoMCodeIN(long wRTNO, List<string> lstUCodes_Del)
        {
            try
            {
                hicSpcPanjengRepository.UpDateDelDateByWrtnoMCodeIN(wRTNO, lstUCodes_Del);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetRowidByWrtnoMCode(long argWrtno, string argMCode)
        {
            return hicSpcPanjengRepository.GetRowidByWrtnoMCode(argWrtno, argMCode);
        }

        public int UpdatePanjengSogenRemarkbyJepDate(string strFrDate, string strToDate)
        {
            return hicSpcPanjengRepository.UpdatePanjengSogenRemarkbyJepDate(strFrDate, strToDate);
        }

        public int GetPanRbyWrtNo(long wRTNO)
        {
            return hicSpcPanjengRepository.GetPanRbyWrtNo(wRTNO);
        }

        public bool UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            try
            {
                hicSpcPanjengRepository.UpdatePanjengInfobyWrtNo(fnWRTNO, strDate, nDrNO);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_SPC_PANJENG GetD1D2bywrtNo(long nWrtNo)
        {
            return hicSpcPanjengRepository.GetD1D2bywrtNo(nWrtNo);
        }

        public List<HIC_SPC_PANJENG> GetPanjengbyWrtNoMCodeNo(long nWRTNO)
        {
            return hicSpcPanjengRepository.GetPanjengbyWrtNoMCodeNo(nWRTNO);
        }

        public string GetPanjengDatebyWrtNo(long fnWrtNo)
        {
            return hicSpcPanjengRepository.GetPanjengDatebyWrtNo(fnWrtNo);
        }
    }
}
