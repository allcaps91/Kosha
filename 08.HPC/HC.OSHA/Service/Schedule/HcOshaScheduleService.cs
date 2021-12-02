namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using ComBase.Mvc.Utils;
    using ComBase.Mvc.Exceptions;
    using ComBase.Controls;


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
        public List<HC_OSHA_SCHEDULE> FindAll(long siteId, string startDate, string endDate, string visitUserId, string visituserId2)
        {
            return hcOshaScheduleRepository.FindAll(siteId, startDate, endDate, visitUserId, visituserId2);
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
            
            dto.EVENTSTARTDATETIME = DateUtil.stringToDateTime(DateUtil.DateTimeToStrig(dto.VISITRESERVEDATE, DateTimeType.YYYY_MM_DD) + " "+ startTime, ComBase.Controls.DateTimeType.YYYY_MM_DD_HH_MM);

            string vvv = dto.VISITUSERID;
          
            if (dto.ID > 0)
            {
                if (dto.GBCHANGE.IsNullOrEmpty())
                {
                    dto.GBCHANGE = "1";
                }
                else if (dto.GBCHANGE.Equals("1"))
                {
                    dto.GBCHANGE = "Y";
                }
                return hcOshaScheduleRepository.Update(dto);
            }
            else
            {
                if (dto.GBCHANGE.IsNullOrEmpty())
                {
                    dto.GBCHANGE = "";
                }
                
                HC_OSHA_SCHEDULE saved = hcOshaScheduleRepository.Insert(dto);

                if (dto.VISITMANAGERID.NotEmpty())
                {
                    string VISITUSERID = dto.VISITUSERID;
                    string VISITUSERNAME = dto.VISITUSERNAME;
                    dto.VISITUSERID = dto.VISITMANAGERID;
                    dto.VISITUSERNAME = dto.VISITMANAGERNAME;
                    dto.VISITMANAGERID = VISITUSERID;
                    dto.VISITMANAGERNAME = VISITUSERNAME;
                    hcOshaScheduleRepository.Insert(dto);
                }

                return saved;
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

                dto.VISITRESERVEDATE = DateUtil.stringToDateTime(tmp[0].Substring(0, 10), DateTimeType.YYYY_MM_DD);  
                dto.VISITSTARTTIME = tmp[1];

                Save(dto);
            }
          
        }
    }
}
