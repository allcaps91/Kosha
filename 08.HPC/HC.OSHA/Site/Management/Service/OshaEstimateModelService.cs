using ComBase.Controls;
using HC.OSHA.Site.Management.Model;
using HC.OSHA.Site.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Site.Management.Service
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

        public List<HC_ESTIMATE_MODEL> FindBySiteId(long id)
        {
            List<HC_ESTIMATE_MODEL> list = hcEstimateModelRepository.FindBySiteId(id);
            foreach (HC_ESTIMATE_MODEL model in list)
            {
                if (model.CONTRACTSTARTDATE.IsNullOrEmpty() || model.CONTRACTENDDATE.IsNullOrEmpty())
                {
                    model.ContractPeriod = string.Empty;
                }
                else
                {
                    model.ContractPeriod = model.CONTRACTSTARTDATE.Substring(0, 7) + "-" + model.CONTRACTENDDATE.Substring(0, 7);
                }
                
               
            }
            return list;
        }
    }
}
