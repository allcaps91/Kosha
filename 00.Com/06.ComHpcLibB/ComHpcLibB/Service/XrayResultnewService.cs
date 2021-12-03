namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewService
    {
        
        private XrayResultnewRepository xrayResultnewRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewService()
        {
			this.xrayResultnewRepository = new XrayResultnewRepository();
        }

        public List<XRAY_RESULTNEW> GetResultNewbyPano(string strPtNo, string argDate)
        {
            return xrayResultnewRepository.GetResultNewbyPano(strPtNo, argDate);
        }

        public List<XRAY_RESULTNEW> GetItembyXCode(string strXCode, string strDeptCode)
        {
            return xrayResultnewRepository.GetItembyXCode(strXCode, strDeptCode);
        }

        public List<XRAY_RESULTNEW> GetItembySeekDate(string strDate)
        {
            return xrayResultnewRepository.GetItembySeekDate(strDate);
        }

        public List<XRAY_RESULTNEW> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return xrayResultnewRepository.GetItembyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public XRAY_RESULTNEW GetXDrCode1byPaNoSeekDate(string fstrPano, string fstrJepDate, string[] strXCode)
        {
            return xrayResultnewRepository.GetXDrCode1byPaNoSeekDate(fstrPano, fstrJepDate, strXCode);
        }

        public XRAY_RESULTNEW GetXDrCode1byPaNoSeekDateGroup(string fstrPano, string fstrJepDate, string[] strXCode)
        {
            return xrayResultnewRepository.GetXDrCode1byPaNoSeekDateGroup(fstrPano, fstrJepDate, strXCode);
        }

        public List<XRAY_RESULTNEW> GetItembyPaNoSeekDateXCode(string strPtNo, string argBDate, string strXCode)
        {
            return xrayResultnewRepository.GetItembyPaNoSeekDateXCode(strPtNo, argBDate, strXCode);
        }
        public List<XRAY_RESULTNEW> GetItembyPaNoSeekDateXCode1(string strPtNo, string argBDate, string strXCode)
        {
            return xrayResultnewRepository.GetItembyPaNoSeekDateXCode1(strPtNo, argBDate, strXCode);
        }
    }
}
