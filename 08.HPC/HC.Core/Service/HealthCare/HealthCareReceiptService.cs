using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Repository;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.Service
{
    public class HealthCareReceiptService
    {
        private HealthCareReceiptRepository healthCareReceiptRepository;
        public HealthCareReceiptService()
        {
            this.healthCareReceiptRepository = new HealthCareReceiptRepository();
        }
        /// <summary>
        /// 일반검진 접수내역
        /// </summary>
        /// <param name="pano"></param>
        /// <returns></returns>
        public List<HealthCareReciptModel> FindHealthCareByPano(string pano)
        {
            List<HealthCareReciptModel>  list = healthCareReceiptRepository.FindAll(pano);
            foreach(HealthCareReciptModel model in list)
            {
                if (model.UCodes.IsNullOrEmpty() == false)
                {
                    model.EXNAME = "일반+특수";
                }
            }
            return list;
        }
    }
}
