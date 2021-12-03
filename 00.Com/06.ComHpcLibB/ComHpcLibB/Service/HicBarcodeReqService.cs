namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HicBarcodeReqService
    {   
        private HicBarcodeReqRepository hicBarcodeReqRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicBarcodeReqService()
        {
			this.hicBarcodeReqRepository = new HicBarcodeReqRepository();
        }

        public int HIC_BARCODE_REQ_Insert(long nWrtNo, string strBuse)
        {
            return hicBarcodeReqRepository.HIC_BARCODE_REQ_Insert(nWrtNo, strBuse);
        }

        public int HIC_BARCODE_REQ_Insert_HIC(long fnPano, string strJepDate)
        {
            return hicBarcodeReqRepository.HIC_BARCODE_REQ_Insert_HIC(fnPano, strJepDate);
        }

        public int HIC_BARCODE_REQ_Insert_HIC_His(long fnPano, string strJepDate, string idNumber, string gstrCOMIP)
        {
            return hicBarcodeReqRepository.HIC_BARCODE_REQ_Insert_HIC_His(fnPano, strJepDate, idNumber, gstrCOMIP);
        }

        public int HIC_BARCODE_REQ_Insert_His(long FnWRTNO, string strGbUse, string idNumber, string gstrCOMIP)
        {
            return hicBarcodeReqRepository.HIC_BARCODE_REQ_Insert_His(FnWRTNO, strGbUse, idNumber, gstrCOMIP);
        }
    }
}
