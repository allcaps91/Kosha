namespace HC.OSHA.Visit.Schedule.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Visit.Schedule.Repository;
    using HC.OSHA.Visit.Schedule.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HcOshaVisitService
    {
        
        private HcOshaVisitRepository hcOshaVisitRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitService()
        {
			this.hcOshaVisitRepository = new HcOshaVisitRepository();
        }

        
        /// <summary>
        /// 방문일정, 방문내역은 1:1
        /// 1:n변경시 방문내역 목록을 방문내역등록에 나타내어 선택하도록할것.
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public HC_OSHA_VISIT FindByScheduleId(long scheduleId)
        {
            return hcOshaVisitRepository.FindByScheduleId(scheduleId);
        }
        public HC_OSHA_VISIT FindById(long id)
        {
            return hcOshaVisitRepository.FindById(id);
        }
        public HC_OSHA_VISIT Save(HC_OSHA_VISIT dto)
        {
           
            if(dto.ID == 0)
            {
                if (hcOshaVisitRepository.FindByScheduleId(dto.SCHEDULE_ID) == null)
                {
                    return hcOshaVisitRepository.Insert(dto);
                }
                else
                {
                    return this.hcOshaVisitRepository.Update(dto);
                }
                
            }
            else
            {
                return this.hcOshaVisitRepository.Update(dto);
            }
            
        }
    }
}
