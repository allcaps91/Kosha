namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HcPanjengPatlistSchoolService
    {
        
        private HcPanjengPatlistSchoolRepository hcPanjengPatlistSchoolRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcPanjengPatlistSchoolService()
        {
			this.hcPanjengPatlistSchoolRepository = new HcPanjengPatlistSchoolRepository();
        }

        public List<HC_PANJENG_PATLIST_SCHOOL> GetPanjengPatListbyJepDate(PAN_PATLIST_SEARCH sItem)
        {
            return hcPanjengPatlistSchoolRepository.GetPanjengPatListbyJepDate(sItem);
        }
    }
}
