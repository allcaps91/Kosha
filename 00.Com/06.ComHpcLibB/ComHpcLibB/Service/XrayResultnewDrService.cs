namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewDrService
    {
        
        private XrayResultnewDrRepository xrayResultnewDrRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewDrService()
        {
			this.xrayResultnewDrRepository = new XrayResultnewDrRepository();
        }

        public List<XRAY_RESULTNEW_DR> GetItembyXCodeDeptCode(string strXCode, string strDeptCode, int nDay)
        {
            return xrayResultnewDrRepository.GetItembyXCodeDeptCode(strXCode, strDeptCode, nDay);
        }

        public List<XRAY_RESULTNEW_DR> GetItembyPaNoReadDate(string fstrPtno, string fstrJepDate)
        {
            return xrayResultnewDrRepository.GetItembyPaNoReadDate(fstrPtno, fstrJepDate);
        }
    }
}
