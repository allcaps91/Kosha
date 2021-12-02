using ComBase;
using ComBase.Mvc.Exceptions;
using HC.OSHA.Dto;
using HC.OSHA.Repository.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.Schedule
{
    public class OshaVisitPriceService
    {
        public OshaVisitPriceRepository oshaVisitPriceRepository { get; }
        public OshaVisitPriceService()
        {
            oshaVisitPriceRepository = new OshaVisitPriceRepository();
        }
        public OSHA_VISIT_PRICE FindOne(long visitId)
        {
            return oshaVisitPriceRepository.FindOne(visitId);
        }
        /// <summary>
        /// 수수료 발생시 기존 발생된 내역은 모두 삭제한다.
        /// </summary>
        /// <param name="dto"></param>
        public void DeleteAndSave(OSHA_VISIT_PRICE dto)
        {

            this.oshaVisitPriceRepository.Delete(dto.VISIT_ID);
            this.oshaVisitPriceRepository.Insert(dto);
            
        }
        public void Delete(OSHA_VISIT_PRICE dto)
        {

            this.oshaVisitPriceRepository.Delete(dto.VISIT_ID);
    

        }
        public void Save(OSHA_VISIT_PRICE dto)
        {

            this.oshaVisitPriceRepository.Insert(dto);

        }


    }
}
