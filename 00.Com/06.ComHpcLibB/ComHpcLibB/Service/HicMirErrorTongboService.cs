namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicMirErrorTongboService
    {
        
        private HicMirErrorTongboRepository hicMirErrorTongboRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicMirErrorTongboService()
        {
			this.hicMirErrorTongboRepository = new HicMirErrorTongboRepository();
        }

        public List <HIC_MIR_ERROR_TONGBO> GetListByItems(long ArgWRTNO, string ArgFDATE, string ArgTDATE, string ArgGUBUN, string ArgSname)
        {
            return hicMirErrorTongboRepository.GetListByItems(ArgWRTNO, ArgFDATE, ArgTDATE, ArgGUBUN, ArgSname);
        }


        public int UPDATE_OKDATE(string strCHK, string strMEMO1, string strROWID, string strJOBNAME)
        {
            return hicMirErrorTongboRepository.UPDATE_OKDATE(strCHK, strMEMO1, strROWID, strJOBNAME);
        }

        public int GetCountbyMirNo(long fnMirNo)
        {
            return hicMirErrorTongboRepository.GetCountbyMirNo(fnMirNo);
        }

        public int GetCountbyWrtNoGubunRemark(long nWRTNO, string strGubun, string strRemark)
        {
            return hicMirErrorTongboRepository.GetCountbyWrtNoGubunRemark(nWRTNO, strGubun, strRemark);
        }

        public int Insert(long nWRTNO, long nMirno, string strSName, string strSex, long nAge, string strGubun, string strRemark)
        {
            return hicMirErrorTongboRepository.Insert(nWRTNO, nMirno, strSName, strSex, nAge, strGubun, strRemark);
        }
    }
}
