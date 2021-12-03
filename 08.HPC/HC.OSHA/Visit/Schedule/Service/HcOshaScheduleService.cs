namespace HC.OSHA.Visit.Schedule.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Visit.Schedule.Repository;
    using HC.OSHA.Visit.Schedule.Dto;
    using ComBase.Mvc.Utils;
    using ComBase.Mvc.Exceptions;


    /// <summary>
    /// 일정관리 
    /// </summary>
    public class HcOshaScheduleService
    {
        
        private HcOshaScheduleRepository hcOshaScheduleRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcOshaScheduleService()
        {
			this.hcOshaScheduleRepository = new HcOshaScheduleRepository();
        }

        public HC_OSHA_SCHEDULE FindOne(long id)
        {
            return hcOshaScheduleRepository.FindById(id);

        }
        public HC_OSHA_SCHEDULE Save(HC_OSHA_SCHEDULE dto)
        {
            string startTime = "00:00";
            if (dto.VISITSTARTTIME != null)
            {
                startTime = dto.VISITSTARTTIME;
            }
            dto.EVENTSTARTDATETIME = DateUtil.stringToDateTime(dto.VISITRESERVEDATE +" "+ startTime, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM);
            if (dto.ID > 0)
            {
                return hcOshaScheduleRepository.Update(dto);
            }
            else
            {
                return hcOshaScheduleRepository.Insert(dto);
            }
        }

        public void Delete(HC_OSHA_SCHEDULE dto)
        {
            hcOshaScheduleRepository.Delete(dto.ID);
        } 

        public void UpdateStartDate(long id, string startDate)
        {
            HC_OSHA_SCHEDULE dto = hcOshaScheduleRepository.FindById(id);
            if(dto == null)
            {
                throw new MTSException("일자변경에 실패하였습니다");
            }
            else
            {
                string[] tmp = startDate.Split(' '); 

                dto.VISITRESERVEDATE = tmp[0];
                dto.VISITSTARTTIME = tmp[1];

                Save(dto);
            }
          
        }
    }
}
