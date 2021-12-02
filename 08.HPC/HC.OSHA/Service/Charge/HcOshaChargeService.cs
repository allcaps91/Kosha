namespace HC.OSHA.Service
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc.Exceptions;
    using HC.Core.Service;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC.OSHA.Repository;
    using HC.OSHA.Service.Schedule;
    using System;
    using System.Collections.Generic;

    public class HcOshaChargeService
    {
        public HcOshaChargeRepository HcOshaChargeRepository { get; }
        private OshaVisitPriceService oshaVisitPriceService;
        private HcOshaScheduleService hcOshaScheduleService;
        private OshaPriceService oshaPriceService;
        private HcOshaVisitService hcOshaVisitService;
        private ChargeRepository chargeRepository;
        public HcOshaChargeService()
        {
            HcOshaChargeRepository = new HcOshaChargeRepository();
            oshaVisitPriceService = new OshaVisitPriceService();
            oshaPriceService = new OshaPriceService();
            hcOshaScheduleService = new HcOshaScheduleService();
            hcOshaVisitService = new HcOshaVisitService();
            chargeRepository = new ChargeRepository();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<CHARGE_MODEL> FindAll(long siteId, string startDate, string endDate)
        {
            return HcOshaChargeRepository.FindAll(siteId, startDate, endDate);
        }
        public List<CHARGE_MODEL> FindSite(string startDate, string endDate)
        {
            return HcOshaChargeRepository.FindSite(startDate, endDate);
        }

        public void Delete(List<CHARGE_MODEL> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (CHARGE_MODEL model in list)
                {
                    HcOshaChargeRepository.Delete(model.ID);
                    chargeRepository.SaveHC_MISU_HISTORY(model.WRTNO, "1");
                    chargeRepository.DeleteMisuMst(model.WRTNO);
                    chargeRepository.DeleteMisuSlip(model.WRTNO);

                }//for

                clsDB.setCommitTran(clsDB.DbCon);
              
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
               
            }
        }
        /// <summary>
        /// 미방문의 경우 방문 및 수수료를 선발생 (방문및수수료발생일자는 미수일자) 시킨 후 청구
        /// </summary>
        /// <param name="list"></param>
        public bool Build(List<CHARGE_MODEL> list, DateTime chargeDate, string executeDate)
        {
            try
            {

                string year = executeDate.Substring(0, 4);
                string month = executeDate.Substring(5, 2);
                if (month.Substring(0, 1) == "0")
                {
                    month = month.Substring(1, 1);
                }
                string pummok = "보건관리 위탁수수료(" + month + "월)";

                clsDB.setBeginTran(clsDB.DbCon);
                foreach (CHARGE_MODEL model in list)
                {
                 
                    //미방문
                    if (model.VISIT_ID == 0)
                    {
                        HC_OSHA_SCHEDULE schedule = hcOshaScheduleService.FindOne(model.SCHEDULE_ID);
                        HC_OSHA_VISIT visit = new HC_OSHA_VISIT();
                        visit.SCHEDULE_ID = model.SCHEDULE_ID;
                        visit.VISITDATETIME = schedule.VISITRESERVEDATE;
                        visit.VISITUSER = schedule.VISITUSERID;
                        visit.VISITUSERNAME = schedule.VISITUSERNAME;
                        visit.ISFEE = "Y";
                        visit.ISDELETED = "N";
                        visit.ISPRECHARGE = "Y";
                        visit.SITE_ID = model.SITE_ID;
                        visit.ESTIMATE_ID = model.ESTIMATE_ID;

                        visit = hcOshaVisitService.Save(visit);
                        model.VISIT_ID = visit.ID;
                       
                        SaveVisitPrice(model);
                        

                    }
                    else
                    {
                        //방문 수수료 미발생
                        if (model.VISIT_PRICE_ID == 0)
                        {
                            SaveVisitPrice(model);
                        }
                    }
                    
                    long id = Save(model, chargeDate);
                    long wrtNo =   SaveMisu(model, chargeDate, year, month, pummok);
                   
                    HcOshaChargeRepository.UpdateWrtNo(id, wrtNo);

                }//for

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if(ex is MTSException)
                {
                    throw ex;
                }
                return false;
            }
          
           
        }
        private long SaveMisu(CHARGE_MODEL model, DateTime chargeDate, string year, string month, string pummok)
        {

            HC_MISU_MST dto = new HC_MISU_MST();
            dto.LTDCODE = model.SITE_ID;
            dto.MISUJONG = "1";
            dto.BDATE = chargeDate;
            dto.GJONG = "82";
            dto.GBEND = "N";
            dto.MISUGBN = "1";
            dto.MISUAMT = model.TOTALPRICE;
            dto.IPGUMAMT = 0;
            dto.GAMAMT = 0;
            dto.SAKAMT = 0;
            dto.BANAMT = 0;
            dto.JANAMT = model.TOTALPRICE;
        //    CNOMODEL cno = chargeRepository.FindMaxCno(year + "-" + "01-01", year + "-" + "12-31");
          //  dto.GIRONO = cno.MaxCNO + 1 + "";
            dto.DANNAME = model.VISITUSERNAME;
            dto.REMARK = model.REMARK;
            dto.MIRGBN = "1";
            dto.MIRCHAAMT = 0;
            dto.GBMISUBUILD = "Y";
            dto.YYMM_JIN = year + month;
            dto.PUMMOK = pummok;
          // dto.CNO = cno.MaxCNO + 1;
            dto.ENTSABUN = CommonService.Instance.Session.UserId.To<long>(0);

            long wrtNo = chargeRepository.Save(dto);

            HC_MISU_SLIP slip = new HC_MISU_SLIP();
            slip.WRTNO = wrtNo;
            slip.BDATE = dto.BDATE;
            slip.LTDCODE = dto.LTDCODE;
            slip.GEACODE = "11";
            slip.SLIPAMT = dto.MISUAMT;
            slip.REMARK = dto.REMARK;
            slip.ENTSABUN = dto.ENTSABUN;

            chargeRepository.SaveSlip(slip);
            chargeRepository.SaveHC_MISU_HISTORY(wrtNo, "1");

            return wrtNo;
        }
        private long Save(CHARGE_MODEL model, DateTime chargeDate)
        {
            HC_OSHA_CHARGE dto = new HC_OSHA_CHARGE();
            dto.SITE_ID = model.SITE_ID;
            dto.ESTIMATE_ID = model.ESTIMATE_ID;
            dto.VISIT_ID = model.VISIT_ID;
            OSHA_VISIT_PRICE VISIT_PRICE = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(dto.VISIT_ID);
            dto.VISIT_PRICE_ID = VISIT_PRICE.ID;
            dto.CHARGEDATE = chargeDate;
            dto.WORKERCOUNT = VISIT_PRICE.WORKERCOUNT;
            dto.UNITPRICE = VISIT_PRICE.UNITPRICE;
            dto.TOTALPRICE = VISIT_PRICE.TOTALPRICE;
            dto.ISCOMPLETE = "N";
            dto.REMARK = model.REMARK;
            
            dto.ISDELETED = ComBase.Mvc.Enums.IsDeleted.N;

            return HcOshaChargeRepository.Insert(dto);
        }
        private void SaveVisitPrice(CHARGE_MODEL model)
        {
            //단가
            OSHA_PRICE lastPrice = oshaPriceService.OshaPriceRepository.FindMaxIdByEstimate(model.ESTIMATE_ID);
            if (lastPrice == null || lastPrice.TOTALPRICE == 0)
            {
                throw new MTSException(model.SITE_NAME + " 단가가 0입니다");
            }

            OSHA_VISIT_PRICE priceDto = new OSHA_VISIT_PRICE();
            priceDto.VISIT_ID = model.VISIT_ID;
            priceDto.ISDELETED = "N";
            priceDto.UNITTOTALPRICE = lastPrice.UNITTOTALPRICE;
            priceDto.TOTALPRICE = lastPrice.TOTALPRICE;
            priceDto.UNITPRICE = lastPrice.UNITPRICE;
            priceDto.WORKERCOUNT = lastPrice.WORKERTOTALCOUNT;
            oshaVisitPriceService.Save(priceDto);
        }
    }
}
