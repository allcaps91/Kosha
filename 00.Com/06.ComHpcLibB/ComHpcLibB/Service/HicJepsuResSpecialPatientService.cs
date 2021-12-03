

namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicJepsuResSpecialPatientService 
    {

        private HicJepsuResSpecialPatientRepository hicJepsuResSpecialPatientRepository;

        /// <summary>
        /// 생성자 
        /// </summary>
        public HicJepsuResSpecialPatientService()
        {
            this.hicJepsuResSpecialPatientRepository = new HicJepsuResSpecialPatientRepository();
        }

        public List<HIC_JEPSU_RES_SPECIAL_PATIENT> GetItemByJepdate(string strFdate, string strTdate)
        {
            return hicJepsuResSpecialPatientRepository.GetItemByJepdate(strFdate, strTdate);
        }

        public List<HIC_JEPSU_RES_SPECIAL_PATIENT> GetItemByJepdateGbspc(string strFdate, string strTdate, long nWrtno = 0)
        {
            return hicJepsuResSpecialPatientRepository.GetItemByJepdateGbspc(strFdate, strTdate, nWrtno);
        }
        public HIC_JEPSU_RES_SPECIAL_PATIENT GetItembyWrtno(long nWrtno)
        {
            return hicJepsuResSpecialPatientRepository.GetItembyWrtno(nWrtno);
        }
    }
}
