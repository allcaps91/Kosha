namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcScodeService
    {
        
        private HicSpcScodeRepository hicSpcScodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcScodeService()
        {
			this.hicSpcScodeRepository = new HicSpcScodeRepository();
        }

        public string Read_Spc_Scode_Name(string argCode)
        {
            return hicSpcScodeRepository.Read_Spc_Scode_Name(argCode);
        }

        public HIC_SPC_SCODE GetItembyCode(string argSogen)
        {
            return hicSpcScodeRepository.GetItembyCode(argSogen);
        }

        public List<HIC_SPC_SCODE> GetCodeNameby()
        {
            return hicSpcScodeRepository.GetCodeNameby();
        }

        public List<HIC_SPC_SCODE> FindAll(string strPan, bool bDel)
        {
            return hicSpcScodeRepository.FindAll(strPan, bDel);
        }

        public int Insert(HIC_SPC_SCODE item)
        {
            return hicSpcScodeRepository.Insert(item);
        }

        public int Update(HIC_SPC_SCODE item)
        {
            return hicSpcScodeRepository.Update(item);
        }

        public int Delete(string argRid, DateTime? argDelDate)
        {
            return hicSpcScodeRepository.Delete(argRid, argDelDate);
        }

        public List<HIC_SPC_SCODE> GetListByCodeName(string strName, string strRetValue)
        {
            return hicSpcScodeRepository.GetListByCodeName(strName, strRetValue);
        }

        public HIC_SPC_SCODE GetItemByMCodePanjeng(string strPan, string fstrMCode)
        {
            return hicSpcScodeRepository.GetItemByMCodePanjeng(strPan, fstrMCode);
        }

        public HIC_SPC_SCODE GetItemByMCodeSogen(string fstrCode, string fstrMCode)
        {
            return hicSpcScodeRepository.GetItemByMCodeSogen(fstrCode, fstrMCode);
        }
    }
}
