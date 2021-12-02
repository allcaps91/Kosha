namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsOrderService
    {
        
        private XrayPacsOrderRepository xrayPacsOrderRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacsOrderService()
        {
			this.xrayPacsOrderRepository = new XrayPacsOrderRepository();
        }

        public int Insert(XRAY_PACS_ORDER item)
        {
            return xrayPacsOrderRepository.Insert(item);
        }

        public int GetCountbyPatIdAcdessionNoExamDate(string strPtNo, string strXrayno, string strExamDate)
        {
            return xrayPacsOrderRepository.GetCountbyPatIdAcdessionNoExamDate(strPtNo, strXrayno, strExamDate);
        }
    }
}
