namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacssendService
    {
        
        private XrayPacssendRepository xrayPacssendRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacssendService()
        {
			this.xrayPacssendRepository = new XrayPacssendRepository();
        }

        public void InsertData(XRAY_PACSSEND xPCS)
        {
            xrayPacssendRepository.InsertData(xPCS);
        }
    }
}
