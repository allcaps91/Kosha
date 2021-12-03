namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class CardApprovCenterService
    {
        
        private CardApprovCenterRepository cardApprovCenterRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public CardApprovCenterService()
        {
			this.cardApprovCenterRepository = new CardApprovCenterRepository();
        }

        public List<CARD_APPROV_CENTER> GetItembyActDate(CARD_APPROV_CENTER item, string strGubun, string strCompany, string strPart, string strIO, string strCard, string strBun, string strChk)
        {
            return cardApprovCenterRepository.GetItembyActDate(item, strGubun, strCompany, strPart, strIO, strCard, strBun, strChk);
        }

        public int UpdateHWrtNobyCardSeqNo(long Idx, long nWRTNO, string argPtno)
        {
            return cardApprovCenterRepository.UpdateHWrtNobyCardSeqNo(Idx, nWRTNO, argPtno);
        }

        public int UpdateHWrtNobyPano(long argWrtno, string argPtno, long argCardSeqno)
        {
            return cardApprovCenterRepository.UpdateHWrtNobyPano(argWrtno, argPtno, argCardSeqno);
        }

        public CARD_APPROV_CENTER GetDataByPanoOrigin(string argPano, string argOrigin)
        {
            return cardApprovCenterRepository.GetDataByPanoOrigin(argPano, argOrigin);
        }

        public List<CARD_APPROV_CENTER> GetCardReportData(string argDate, int rdoCheck, string strSaBun)
        {
            return cardApprovCenterRepository.GetCardReportData(argDate, rdoCheck, strSaBun);
        }
    }
}
