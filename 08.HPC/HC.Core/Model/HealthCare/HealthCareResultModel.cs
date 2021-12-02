namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 1차검진 결과 , 유소견자 사후관리(대행)
    /// </summary>
    public class HealthCareResultModel : BaseDto
    {
        
        public string PANJENG { get; set; }
        public string SOGEN { get; set; }

        public HIC_RES_BOHUM1 HIC_RES_BOHUM1 { get; set; }
    }

}
