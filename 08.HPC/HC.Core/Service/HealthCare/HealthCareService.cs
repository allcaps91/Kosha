using ComHpcLibB.Dto;
using ComHpcLibB.Repository;
using ComHpcLibB.Service;
using HC.Core.Model;
using HC.Core.Repository;
using System.Collections.Generic;

namespace HC.Core.Service
{
    /// <summary>
    /// 건강검진
    /// </summary>
    public class HealthCareService
    {
        public HealthCarePatientRepository HealthCarePatientRepository { get; }
        public ExResultRepository ExResultRepository { get; }
        /// <summary>
        /// 일반검진
        /// </summary>
        public HicResBohum1Repository HicResBohum1Repository { get;  }

        /// <summary>
        /// 특수검진
        /// </summary>
        public HicResSpecialService HicResSpecialService { get; }
        
        public HealthCareService()
        {
            this.HealthCarePatientRepository = new HealthCarePatientRepository();
            this.ExResultRepository = new ExResultRepository();
            this.HicResBohum1Repository = new HicResBohum1Repository();
            this.HicResSpecialService = new HicResSpecialService();
        }

        
        /// <summary>
        /// 1차 문진표
        /// </summary>
        /// <param name="wrtNo"></param>
        /// <returns></returns>
        public HIC_RES_BOHUM1 GetFirstExaminationQuestionnaire(long wrtNo)
        {
            HIC_RES_BOHUM1 dto = HicResBohum1Repository.GetItemByWrtno(wrtNo);
            return dto;
        }
        /// <summary>
        /// 특수검진 문진표
        /// </summary>
        /// <param name="wrtNo"></param>
        /// <returns></returns>
        public HIC_RES_SPECIAL GetSpecialExaminationQuestionnaire(long wrtNo)
        {
            HIC_RES_SPECIAL dto;
            return dto = HicResSpecialService.GetItemByWrtno(wrtNo);
        }
        /// <summary>
        /// 일반검진 검사결과
        /// </summary>
        /// <param name="wrtNo"></param>
        /// <returns></returns>
        public ExResultModel GetExResult(long wrtNo)
        {
            List<ExResult_WRTNO_Model> exResultList  =  ExResultRepository.FindByWrtNo( wrtNo);
          
            return GetResult(exResultList);
        }
        public ExResultModel GetResult(List<ExResult_WRTNO_Model> exResultList)
        {
            ExResultModel result = new ExResultModel();
            foreach (ExResult_WRTNO_Model model in exResultList)
            {
                switch (model.EXCODE)
                {
                    case "A101":
                        result.HEIGHT = model.STATUS;
                        break;
                    case "A102":
                        result.WEIGHT = model.STATUS;
                        break;
                    case "A117":
                        result.BMI = model.STATUS;
                        break;
                    case "A115":
                        result.WAITST = model.STATUS;
                        break;
                    case "A108":
                        result.SBP = model.STATUS;
                        break;
                    case "A109":
                        result.DBP = model.STATUS;
                        break;
                    case "A122":
                        result.BST = model.STATUS;
                        break;
                    case "ZE22":
                        result.HBA1C = model.STATUS;
                        break;
                    case "A123":
                        result.TDL = model.STATUS;
                        break;
                    case "A242":
                        result.HDL = model.STATUS;
                        break;
                    case "C404":
                        result.LDL = model.STATUS;
                        break;
                    case "A241":
                        result.TRI = model.STATUS;
                        break;
                    case "A112":
                        result.PROTEIN = model.STATUS;
                        result.PROTEIN_RESULT = model.RESULT;
                        result.PROTEIN_RESCODE = model.RESCODE;
                        break;
                    case "A274":
                        result.CREATININE = model.STATUS;
                        break;
                    case "A116":
                        result.GFR = model.STATUS;
                        break;
                    case "A142":
                    case "TX11":
                        result.CT = model.STATUS;
                        break;
                    case "LU48":
                        result.LU48 = model.STATUS;
                        break;
                    case "TH11":
                        result.TH11 = model.STATUS;
                        break;
                    case "TH12":
                        result.TH12 = model.STATUS;
                        break;
                    case "TH13":
                        result.TH13 = model.STATUS;
                        break;
                    case "TH14":
                        result.TH14 = model.STATUS;
                        break;
                    case "TH15":
                        result.TH15 = model.STATUS;
                        break;
                    case "TH16":
                        result.TH16 = model.STATUS;
                        break;
                    case "TH21":
                        result.TH21 = model.STATUS;
                        break;
                    case "TH22":
                        result.TH22 = model.STATUS;
                        break;
                    case "TH23":
                        result.TH23 = model.STATUS;
                        break;
                    case "TH24":
                        result.TH24 = model.STATUS;
                        break;
                    case "TH25":
                        result.TH25 = model.STATUS;
                        break;
                    case "TH26":
                        result.TH26 = model.STATUS;
                        break;
                    default:
                        break;

                }

                if(model.EXCODE.CompareTo("TE15") == 1 && model.EXCODE.CompareTo("TE24")== -1 )
                {
                    if(result.TE15 == "")
                    {
                        result.TE15 = "정상";
                    }
                    if(result.TE15 != "정상" && result.TE15 != "" && result.TE15 != ".")
                    {
                        
                    }
                }
            }//for
            return result;
        }
        /// <summary>
        /// 종합검진 검사결과
        /// </summary>
        /// <param name="wrtNo"></param>
        /// <returns></returns>
        public ExResultModel GetTotalExResult(long wrtNo)
        {
            List<ExResult_WRTNO_Model> exResultList = ExResultRepository.FindTotalByWrtNo(wrtNo);
            return GetResult(exResultList);
        }
    }
}
