namespace ComHpcLibB.Service
{
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMonthlyService
    {
        private HicMisuMonthlyRepository hicMisuMonthlyRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicMisuMonthlyService()
        {
            this.hicMisuMonthlyRepository = new HicMisuMonthlyRepository();
        }

        public List<HIC_MISU_MONTHLY> GetmisuSummary(long txtWrtNo)
        {
            return hicMisuMonthlyRepository.GetmisuSummary(txtWrtNo);
        }

        public List<HIC_MISU_MONTHLY> GetYYMM()
        {
            return hicMisuMonthlyRepository.GetYYMM();
        }

        public List<HIC_MISU_MONTHLY> GetmisuTong(long txtWrtNo)
        {
            return hicMisuMonthlyRepository.GetmisuTong(txtWrtNo);
        }
        public List<HIC_MISU_MONTHLY> GetmisuTong_2()
        {
            return hicMisuMonthlyRepository.GetmisuTong_2();
        }
    }
}