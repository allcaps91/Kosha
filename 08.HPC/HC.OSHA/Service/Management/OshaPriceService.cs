using ComBase.Controls;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service
{
    public class OshaPriceService
    {
        public OshaPriceRepository OshaPriceRepository { get; }
        public OshaPriceService()
        {
            this.OshaPriceRepository = new OshaPriceRepository();
        }
        public List<OSHA_PRICE> FindAll(string nameOrCode, string gbGukgo, bool isParent)
        {
            return OshaPriceRepository.FindAllBySite(nameOrCode, gbGukgo , isParent);
        }
        public OSHA_PRICE Save(OSHA_PRICE dto)
        {
            if (dto.ISFIX.IsNullOrEmpty()){
                dto.ISFIX = "N";
            }
            if (dto.ISBILL.IsNullOrEmpty()){
                dto.ISBILL = "N";
            }
            return this.OshaPriceRepository.Insert(dto);

        }

        public void Delete(long id)
        {
            this.OshaPriceRepository.Delete(id);
        }


        /// <summary>
        /// 단가금액으로 단가 조절
        /// </summary>
        /// <param name="workerTotalCount"></param>
        /// <param name="unitTotalPrice"></param>
        public double GetUnitPrice(object workerCountValue, object unitTotalPriceValue)
        {

            int workerCount = Convert.ToInt32(workerCountValue);
            double unitTotalPrice = Convert.ToDouble(unitTotalPriceValue);

            double unitPrice = unitTotalPrice / workerCount;
            return unitPrice;
        }
        /// <summary>
        /// 계약금액 계싼
        /// </summary>
        /// <param name="workerCountValue"></param>
        /// <param name="unitPriceValue"></param>
        public double GetTotalPrice(object workerCountValue, object unitPriceValue)
        {
            int workerCount = 0;
            double unitPrice = 0;
            if (!workerCountValue.Equals(Decimal.MinValue))
            {
                workerCount = Convert.ToInt32(workerCountValue);
            }
          
            if(!unitPriceValue.Equals( Decimal.MinValue))
            {
                unitPrice = Convert.ToDouble(unitPriceValue);
            }

            double total = workerCount * unitPrice;
            return total;

        }
    }
}
