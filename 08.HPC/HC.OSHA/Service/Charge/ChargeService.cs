namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC.OSHA.Repository.Schedule;
    using ComBase;
    using System;
    using ComBase.Mvc.Exceptions;
    using HC.OSHA.Service.Schedule;
    using ComBase.Mvc.Utils;
    using HC.Core.Common.Extension;
    using HC.Core.Repository;
    using HC.Core.Dto;
    using HC.Core.Service;

    public class ChargeService
    {
        public ChargeRepository ChargeRepository { get; }
        private HcOshaScheduleRepository hcOshaScheduleRepository;
        private HcOshaVisitService hcOshaVisitService;
        private OshaPriceService oshaPriceService;
        private OshaVisitPriceService oshaVisitPriceService;
        private HcOshaEstimateRepository hcOshaEstimateRepository;
        private HcUsersRepository hcUsersRepository;
        public ChargeService()
        {
            this.ChargeRepository = new ChargeRepository();
            this.hcOshaScheduleRepository = new HcOshaScheduleRepository();
            this.hcOshaVisitService = new HcOshaVisitService();
            this.oshaPriceService = new OshaPriceService();
            this.oshaVisitPriceService = new OshaVisitPriceService();
            hcOshaEstimateRepository = new HcOshaEstimateRepository();
            hcUsersRepository = new HcUsersRepository();
        }


        /// <summary>
        /// 하청이 있는 원청의 경우는 원청만 수수료를 발생한다
        /// 하청은 스케쥴이 없기 때문에 선청구 수수료 발생이 되지 않음.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="siteId"></param>
        public bool PreCharge(long siteId, long chargePrice, string startDate, string endDate)
        {
            bool isPreCharge = false;
        
            long visitCount = ChargeRepository.GetVisitCount(startDate, endDate, siteId);
            if(visitCount == 0)
            {
                List<HC_OSHA_SCHEDULE> scheduleList = hcOshaScheduleRepository.FindAll(startDate, endDate, siteId);
                if(scheduleList.Count > 0)
                {
                    HC_OSHA_VISIT visit = new HC_OSHA_VISIT();

                    foreach (HC_OSHA_SCHEDULE scheddule in scheduleList)
                    {
                        HC_USER user = hcUsersRepository.FindOne(scheddule.VISITUSERID);
                        Role role = role = (Role)Enum.Parse(typeof(Role), user.Role);
                        if(role == Role.NURSE)
                        {
                            HC_OSHA_VISIT saved =  hcOshaVisitService.FindByScheduleId(scheddule.ID);
                            if(saved == null)
                            {
                                visit.SCHEDULE_ID = scheddule.ID;
                                visit.VISITDATETIME = scheddule.VISITRESERVEDATE;
                                visit.VISITUSER = scheddule.VISITUSERID;
                                visit.VISITUSERNAME = scheddule.VISITUSERNAME;
                                visit.ISFEE = "Y";
                                visit.ISDELETED = "N";
                                visit.ISPRECHARGE = "Y";
                                visit.SITE_ID = scheddule.SITE_ID;
                                visit.ESTIMATE_ID = scheddule.ESTIMATE_ID;

                                visit = hcOshaVisitService.Save(visit);
                                break;
                            }
                            
                        }
                    }
                  
                    if(visit.ID>0)
                    {
                        long estimateId = hcOshaEstimateRepository.GetEstimateId(siteId);
                        //OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(estimateId);
                        OSHA_PRICE dto = oshaPriceService.OshaPriceRepository.FindMaxIdBySite(siteId);

                        List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(siteId);
                        foreach (OSHA_PRICE child in childPrice)
                        {
                            dto.WORKERTOTALCOUNT += child.WORKERTOTALCOUNT;
                            dto.TOTALPRICE += child.TOTALPRICE;
                        }

                        // OSHA_PRICE lastPrice = oshaPriceService.OshaPriceRepository.findm.FindMaxIdByEstimate(scheddule.SITE_ID );
                        if (dto.TOTALPRICE == 0)
                        {
                            throw new MTSException(siteId + " 단가가 0입니다. 선청구를 할 수 없습니다");
                        }

                        OSHA_VISIT_PRICE priceDto = new OSHA_VISIT_PRICE();
                        priceDto.VISIT_ID = visit.ID;
                        priceDto.ISDELETED = "N";
                        priceDto.UNITTOTALPRICE = dto.UNITTOTALPRICE;
                        priceDto.TOTALPRICE = dto.TOTALPRICE;
                        priceDto.UNITPRICE = dto.UNITPRICE;
                        priceDto.WORKERCOUNT = dto.WORKERTOTALCOUNT;
                        priceDto.ISPRECHARGE =  "Y";
                        if (childPrice.Count > 0)
                        {
                            priceDto.CHARGEPRICE = dto.TOTALPRICE;
                        }
                        else
                        {
                            priceDto.CHARGEPRICE = chargePrice;
                        }
                        
                        oshaVisitPriceService.Save(priceDto);

                        isPreCharge = true;
                    }
                  
                }
            
            }

            return isPreCharge;
        }
        public bool Save(List<HC_MISU_MST> list)
        {
            clsDB.setBeginTran(clsDB.DbCon);
 
            try
            {
                foreach(HC_MISU_MST mst in list)
                {
                    string startDate = DateUtil.DateTimeToStrig(mst.BDATE.GetFirstDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);
                    string endDate = DateUtil.DateTimeToStrig(mst.BDATE.GetLastDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);

                    //선청구 원청만 수수료 발생
                    bool isPrecharge = PreCharge( mst.LTDCODE, mst.MISUAMT, startDate, endDate);
                    int year = mst.BDATE.Year;
                    long cno = ChargeRepository.FindMaxCno(year + "-" + "01-01", year + "-" + "12-31");
                    mst.GIRONO = (cno + 1).ToString() ;
                    mst.CNO = cno + 1;

                    long wrtNo = ChargeRepository.Save(mst);

                    if (!isPrecharge)
                    {
                        //선청구가 아니면 청구금액만 업데이트
                        ChargeRepository.UpdateChargePrice(startDate, endDate, mst.LTDCODE, mst.MISUAMT);
                    }

                    HC_MISU_SLIP slip = new HC_MISU_SLIP();
                    slip.WRTNO = wrtNo;
                    slip.BDATE = mst.BDATE;
                    slip.LTDCODE = mst.LTDCODE;
                    slip.GEACODE = "11";
                    slip.SLIPAMT = mst.MISUAMT;
                    slip.REMARK = mst.REMARK;
                    slip.ENTSABUN = mst.ENTSABUN;

                    ChargeRepository.SaveSlip(slip);
                    ChargeRepository.SaveHC_MISU_HISTORY(wrtNo, "1");

                   
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return true;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex);
                return false;
            }
        }

        public bool Delete(List<HC_MISU_MST> list)
        {

         
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                foreach (HC_MISU_MST mst in list)
                {
                    // long wrtNo = ChargeRepository.Save(mst);
                   

                    ChargeRepository.SaveHC_MISU_HISTORY(mst.WRTNO, "4");
                    ChargeRepository.DeleteMisuMst(mst.WRTNO);
                    ChargeRepository.DeleteMisuSlip(mst.WRTNO);

                    string startDate = DateUtil.DateTimeToStrig(mst.BDATE.GetFirstDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);
                    string endDate = DateUtil.DateTimeToStrig(mst.BDATE.GetLastDate(), ComBase.Controls.DateTimeType.YYYY_MM_DD);

                    //선청구 발생 데이타 삭제
                    ChargeRepository. DeleteVisitPreCharge(startDate, endDate, mst.LTDCODE);
                }

                clsDB.setCommitTran(clsDB.DbCon);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }
      
       
    }
}
