namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicGroupcodeService
    {
        
        private HicGroupcodeRepository hicGroupcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicGroupcodeService()
        {
			this.hicGroupcodeRepository = new HicGroupcodeRepository();
        }

        public IList<HIC_GROUPCODE> FindCodeIn(string argSql)
        {
            return hicGroupcodeRepository.FindCodeIn(argSql);
        }

        public string Read_Group_Name(string argCode)
        {
            return hicGroupcodeRepository.Read_Group_Name(argCode);
        }

        public HIC_GROUPCODE GetItemByCode(string argCode)
        {
            return hicGroupcodeRepository.GetItemByCode(argCode);
        }

        public IList<HIC_GROUPCODE> GetListByAll(string argJong)
        {
            return hicGroupcodeRepository.GetListByAll(argJong);
        }

        public int Insert(HIC_GROUPCODE item)
        {
            return hicGroupcodeRepository.Insert(item);
        }

        public int UpDate(HIC_GROUPCODE item)
        {
            return hicGroupcodeRepository.UpDate(item);
        }

        public int Delete(string argRowid)
        {
            return hicGroupcodeRepository.Delete(argRowid);
        }

        public IList<HIC_GROUPCODE> GetItemByLikeName(string argName = "")
        {
            return hicGroupcodeRepository.GetItemByLikeName(argName);
        }

        public string GetNameByCode(string argCode)
        {
            return hicGroupcodeRepository.GetNameByCode(argCode);
        }

        public List<HIC_GROUPCODE> GetHalinListByItems(string argSDate, string argJong, List<string> lstCode, string argTECGubun, int[] fnAm, string argTemp, string arghCan4)
        {
            return hicGroupcodeRepository.GetHalinListByItems(argSDate, argJong, lstCode, argTECGubun, fnAm, argTemp, arghCan4);
        }

        public List<HIC_GROUPCODE> GetListByCode(string argCode)
        {
            return hicGroupcodeRepository.GetListByCode(argCode);
        }

        public List<HIC_GROUPCODE> GetHangCode(List<string> strJong, string strGbSelectYN)
        {
            return hicGroupcodeRepository.GetHangCode(strJong, strGbSelectYN);
        }

        public List<HIC_GROUPCODE> GetItembySDate(string argGJepDate, List<string> strUCodeSQL)
        {
            return hicGroupcodeRepository.GetItembySDate(argGJepDate, strUCodeSQL);
        }

        public List<HIC_GROUPCODE> GetItembyJepDateGjJong(string argGJepDate, string argGjJong,  string argGubu, List<string> argUCodesSql, List<string> argCodesSql, string strGjJong_Gubun1, string strGjJong_Gubun2)
        {
            return hicGroupcodeRepository.GetItembyJepDateGjJong(argGJepDate, argGjJong, argGubu, argUCodesSql, argCodesSql, strGjJong_Gubun1, strGjJong_Gubun2);
        }

        public string GetGbAmByCode(string argGroupCode)
        {
            return hicGroupcodeRepository.GetGbAmByCode(argGroupCode);
        }

        public List<HIC_GROUPCODE> GetItemByUCodes(List<string> argUCode)
        {
            return hicGroupcodeRepository.GetHicItemByUCodes(argUCode);
        }

        public List<HIC_GROUPCODE> GetItembyJong(string strJong)
        {
            return hicGroupcodeRepository.GetItembyJong(strJong);
        }

        public int DeletebyRowId(string fstrROWID)
        {
            return hicGroupcodeRepository.DeletebyRowId(fstrROWID);
        }

    }
}
