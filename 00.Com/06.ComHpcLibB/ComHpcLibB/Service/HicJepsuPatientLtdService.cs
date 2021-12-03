
namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicJepsuPatientLtdService
    {
        private HicJepsuPatientLtdRepository hicJepsuPatientLtdRepository;
        /// <summary>
        /// 생성자 
        /// </summary>

        public HicJepsuPatientLtdService()
        {
            this.hicJepsuPatientLtdRepository = new HicJepsuPatientLtdRepository();
        }
        public HIC_JEPSU_PATIENT_LTD GetItemByWrtno(long nWRTNO)
        {
            return hicJepsuPatientLtdRepository.GetItemByWrtno(nWRTNO);
        }
    }
}
