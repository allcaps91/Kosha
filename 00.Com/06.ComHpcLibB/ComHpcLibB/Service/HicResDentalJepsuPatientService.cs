namespace ComHpcLibB.Service
{

    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicResDentalJepsuPatientService
    {
        private HicResDentalJepsuPatientRepository hicResDentalJepsuPatientRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicResDentalJepsuPatientService()
        {
            this.hicResDentalJepsuPatientRepository = new HicResDentalJepsuPatientRepository();
        }

        public HIC_RES_DENTAL_JEPSU_PATEINT GetItemByWrtno(long argWRTNO)
        {
            return hicResDentalJepsuPatientRepository.GetItemByWrtno(argWRTNO);
        }

        public HIC_RES_DENTAL_JEPSU_PATEINT GetItembyMirNoMirNo(long argMirno, long argWRTNO, string argJepDate)
        {
            return hicResDentalJepsuPatientRepository.GetItembyMirNoMirNo(argMirno, argWRTNO, argJepDate);
        }

        public List<HIC_RES_DENTAL_JEPSU_PATEINT> GetItemByPandateSnameLtdGubun( string argFDate, string argTDate, string argSname, string argLtdCode, string argRePrint, string argSort)
        {
            return hicResDentalJepsuPatientRepository.GetItemByPandateSnameLtdGubun(argFDate, argTDate, argSname, argLtdCode, argRePrint, argSort);
        }



    }
}
