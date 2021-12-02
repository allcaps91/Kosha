namespace ComHpcLibB.Service
{
    using ComHpcLibB.Repository;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicBcodeService
    {
        private HicBcodeRepository hicBcodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicBcodeService()
        {
            this.hicBcodeRepository = new HicBcodeRepository();
        }

        public List<HIC_BCODE> BarCodeAutoPrintSet(string strIpAddress)
        {
            return hicBcodeRepository.BarCodeAutoPrintSet(strIpAddress);
        }

        public string Read_Check_Center_Buse(string argBuCode)
        {
            return hicBcodeRepository.Read_Check_Center_Buse(argBuCode);
        }

        public string Order_Insert(long argSabun)
        {
            return hicBcodeRepository.Order_Insert(argSabun);
        }

        public string Read_Code(string argGubun, string argName)
        {
            return hicBcodeRepository.Read_Code(argGubun, argName);
        }

        public string Read_Code2(string argGubun, string argCode)
        {
            return hicBcodeRepository.Read_Code2(argGubun, argCode);
        }

        public List<HIC_BCODE> Read_Code_All(string argGubun, string argCode)
        {
            return hicBcodeRepository.Read_Code_All(argGubun, argCode);
        }

        public string Read_Code_One(string argGubun, string argCode)
        {
            return hicBcodeRepository.Read_Code_One(argGubun, argCode);
        }

        public string GetRowIdbySabun(long gnJobSabun)
        {
            return hicBcodeRepository.GetRowIdbySabun(gnJobSabun);
        }

        public string GetRowIdbySabunJupsu(long gnJobSabun)
        {
            return hicBcodeRepository.GetRowIdbySabunJupsu(gnJobSabun);
        }

        public List<HIC_BCODE> GetCodeNamebyBcode(string strGubun, string strName = "")
        {
            return hicBcodeRepository.GetCodeNamebyBcode(strGubun, strName);
        }

        public List<HIC_BCODE> GetCodeNamebyBcode1(string strGubun, string strCode = "")
        {
            return hicBcodeRepository.GetCodeNamebyBcode1(strGubun, strCode);
        }

        public long GetCountbyName(string strName, string strGubun)
        {
            return hicBcodeRepository.GetCountbyName(strName, strGubun);
        }

        public int GetMaxCodebyGubun(string strGubun)
        {
            return hicBcodeRepository.GetMaxCodebyGubun(strGubun);
        }

        public int InsertCode(string strGubun, int nMaxCode, string strName)
        {
            return hicBcodeRepository.InsertCode(strGubun, nMaxCode, strName);
        }

        public int UpdateCode(string strGubun, int nMaxCode, string strName, string strSort)
        {
            return hicBcodeRepository.UpdateCode(strGubun, nMaxCode, strName, strSort);
        }


        public List<HIC_BCODE> GetCodeNamebyGubun(string strGubun)
        {
            return hicBcodeRepository.GetCodeNamebyGubun(strGubun);
        }

        public int DeletebyCode(string strGubun, string strCode)
        {
            return hicBcodeRepository.DeletebyCode(strGubun, strCode);
        }

        public string GetCodeNamebyGubunCode(string strGubun, string strCode)
        {
            return hicBcodeRepository.GetCodeNamebyGubunCode(strGubun, strCode);
        }

        public List<HIC_BCODE> GetItembyGubun(string strGubun)
        {
            return hicBcodeRepository.GetItembyGubun(strGubun);
        }

        public List<HIC_BCODE> GetCodebyGubun(string strGubun)
        {
            return hicBcodeRepository.GetCodebyGubun(strGubun);
        }

        public HIC_BCODE Read_Hic_BCode(string strGubun, string strCode)
        {
            return hicBcodeRepository.Read_Hic_BCode(strGubun, strCode);
        }

        public string GetHeaRowIdbySabun(long gnJobSabun)
        {
            return hicBcodeRepository.GetHeaRowIdbySabun(gnJobSabun);
        }
        public int GetCountbyGubunCode(string strGubun, string strCode)
        {
            return hicBcodeRepository.GetCountbyGubunCode(strGubun, strCode);
        }

        public int GetCountbyGuybunName(string strGubun, string gstrCOMIP)
        {
            return hicBcodeRepository.GetCountbyGuybunName(strGubun, gstrCOMIP);
        }

        public int SaveBcode(HIC_BCODE item)
        {
            return hicBcodeRepository.SaveBcode(item);
        }

        public int DeletebyGubun(string argGubun)
        {
            return hicBcodeRepository.DeletebyGubun(argGubun);
        }

        public int GetHeaResultAutoSendPCbyGubun(string strGubun, string strName)
        {
            return hicBcodeRepository.GetHeaResultAutoSendPCbyGubun(strGubun, strName);
        }

        public int GetMenuSetAuthoritybyIdNumber(string strGubun, string idNumber)
        {
            return hicBcodeRepository.GetMenuSetAuthoritybyIdNumber(strGubun, idNumber);
        }

        public int GetCountbyGubunCodeName(string strGubun, string strCode)
        {
            return hicBcodeRepository.GetCountbyGubunCodeName(strGubun, strCode);
        }

        public List<HIC_BCODE> GetRowid(string Sabun)
        {
            return hicBcodeRepository.GetRowid(Sabun);
        }
    }
}
