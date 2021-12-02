namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsAdtService
    {
        
        private XrayPacsAdtRepository xrayPacsAdtRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacsAdtService()
        {
			this.xrayPacsAdtRepository = new XrayPacsAdtRepository();
        }

        public int Insert(XRAY_PACS_ADT item)
        {
            return xrayPacsAdtRepository.Insert(item);
        }

        public XRAY_PACS_ADT GetItembyPATID(string strPANO)
        {
            return xrayPacsAdtRepository.GetItembyPATID(strPANO);
        }

    }
}
