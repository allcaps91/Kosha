using ComBase.Controls;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    /// <summary>
    /// 견적 및 계약 모델 서비스
    /// </summary>
    public class OshaEstimateModelService
    {
        HcEstimateModelRepository hcEstimateModelRepository;

        public OshaEstimateModelService()
        {
            this.hcEstimateModelRepository = new HcEstimateModelRepository();
        }

        public List<HC_ESTIMATE_MODEL> FindBySiteId(long id, bool isALL)
        {
            List<HC_ESTIMATE_MODEL> list = null;
            if (isALL)
            {
                list= hcEstimateModelRepository.FindBySiteId(id);
            }
            else
            {
                list = hcEstimateModelRepository.FindBySiteIdAndContract(id);
            }
            
            foreach (HC_ESTIMATE_MODEL model in list)
            {
                if (model.CONTRACTSTARTDATE.IsNullOrEmpty() || model.CONTRACTENDDATE.IsNullOrEmpty())
                {
                    model.ContractPeriod = model.ESTIMATEDATE;
                    model.ISCONTRACT = "견적";
                }
                else
                {
                    model.ContractPeriod = model.CONTRACTSTARTDATE.Substring(0, 7) + " ~ " + model.CONTRACTENDDATE.Substring(0, 7);
                }
                
               
            }
            return list;
        }
    }
}
