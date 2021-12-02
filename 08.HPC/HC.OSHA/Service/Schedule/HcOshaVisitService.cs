namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using HC.OSHA.Repository.Schedule;
    using HC.OSHA.Service.Schedule;
    using static HC.Core.Service.LogService;
    using HC.Core.Service;
    using ComBase;
    using System;
    using ComBase.Mvc.Utils;


    /// <summary>
    /// 방문등록
    /// </summary>
    public class HcOshaVisitService
    {

        public HcOshaVisitRepository hcOshaVisitRepository { get; }
        public OshaVisitPriceService oshaVisitPriceService { get; }

        /// <summary>
        /// 
        /// </summary>
        public HcOshaVisitService()
        {
            this.hcOshaVisitRepository = new HcOshaVisitRepository();
            oshaVisitPriceService = new OshaVisitPriceService();
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
            HC_OSHA_VISIT saved = null;
            if (dto.ID == 0)
            {
                if (hcOshaVisitRepository.FindByScheduleId(dto.SCHEDULE_ID) == null)
                {
                    saved = hcOshaVisitRepository.Insert(dto);
                }
                else
                {
                    saved = hcOshaVisitRepository.Update(dto);
                }

            }
            else
            {
                saved = hcOshaVisitRepository.Update(dto);
            }

            LogService.Instance.Task(dto.SITE_ID, TaskName.VISIT);

            return saved;
        }



        /// <summary>
        /// 일자가 다르면 집계로 인해 RAWA 데이타는 삭제하지 않고(방문등록) 마이너스 수수료를 당일 날짜로 발생시킨다.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="priceDto"></param>
        /// <returns></returns>
        public int Delete(HC_OSHA_VISIT dto, OSHA_VISIT_PRICE priceDto)
        {
            int result = 0;
            try
            {
                
                clsDB.setBeginTran(clsDB.DbCon);

                ComFunc.ReadSysDate(clsDB.DbCon);
                string today = clsPublic.GstrSysDate;
                HC_OSHA_VISIT saved = hcOshaVisitRepository.FindById(dto.ID);
                string created = DateUtil.DateTimeToStrig(saved.CREATED, ComBase.Controls.DateTimeType.YYYY_MM_DD);

                if (created == today)
                {
                    DeleteVisit(dto);

                    result = 0;
                }
                else
                {
                    //  2020.12.17 무조건 마이너스 금액으로 변경
                    //  기존데이터는 수정을 하지 않고 새로운 데이터를 생성해서 마이너스금액으로 발생한다.

                    //if (saved.ISFEE == "N")
                    //{
                    //    DeleteVisit(dto);
                    //    result = 0;
                    //}
                    //else if (saved.ISFEE == "Y")
                    {
                        //DeleteVisit(dto);
                        NewVisitAndMinusPrice(saved);

                        result = 1;
                    }

                }


                 clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);

                Log.Error(ex);
                result = 99;
            }

            return result;

        }

        // 마이너스  발생
        private long NewVisitAndMinusPrice(HC_OSHA_VISIT saved)
        {

            OSHA_VISIT_PRICE visitPriceDto = oshaVisitPriceService.oshaVisitPriceRepository.FindMaxId(saved.ID);

            //saved.ID = 0;
            //HC_OSHA_VISIT newSaved = hcOshaVisitRepository.Insert(saved);

            //방문금액 마이너스 발생
            if (visitPriceDto != null)
            {
                //방문등록  삭제시 이미 마이너스 금액 발생되어 있음
                if (visitPriceDto.TOTALPRICE >0)
                {
                    visitPriceDto.ID = 0;
                    visitPriceDto.VISIT_ID = saved.ID;
                    visitPriceDto.TOTALPRICE = -visitPriceDto.TOTALPRICE;

                    this.oshaVisitPriceService.Save(visitPriceDto);
                }
                
            }
            
            return saved.ID;

        }
        /// <summary>
        /// 방문등록 및 수수료 삭제
        /// </summary>
        /// <param name="dto"></param>
        private void DeleteVisit(HC_OSHA_VISIT dto)
        {
            dto.ISDELETED = "Y";
            hcOshaVisitRepository.Delete(dto);
            oshaVisitPriceService.oshaVisitPriceRepository.Delete(dto.ID);
        }

        public HC_OSHA_VISIT Save(HC_OSHA_VISIT dto, OSHA_VISIT_PRICE priceDto)
        {
            HC_OSHA_VISIT saved = hcOshaVisitRepository.FindById(dto.ID);
            HC_OSHA_VISIT newSaved = null;

            //신규
            if (saved == null)
            {
                if (hcOshaVisitRepository.FindByScheduleId(dto.SCHEDULE_ID) == null)
                {
                    newSaved = hcOshaVisitRepository.Insert(dto);
                }
                else
                {
                 
                    newSaved = hcOshaVisitRepository.Update(dto);
                }

            }
            else
            {
                newSaved = hcOshaVisitRepository.Update(dto);
            }

              if(saved == null)
            {
                if(newSaved.ISFEE == "Y")
                {
                    priceDto.VISIT_ID = newSaved.ID;
                    oshaVisitPriceService.Save(priceDto);
                }
                
            }
            else
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                string today = clsPublic.GstrSysDate;
                string created = DateUtil.DateTimeToStrig(saved.CREATED, ComBase.Controls.DateTimeType.YYYY_MM_DD);
                
                
                if(saved.ISPRECHARGE == "Y")
                {
                    //선청구는 집계대상이 아니므로 바로 삭제하고 새로 생성한다.
                    priceDto.ISPRECHARGE = "N";
                    DeleteVisitPrice(priceDto);
                }
                else
                {
                    if (priceDto != null)
                    {
                        if (saved.ISFEE == "Y" && newSaved.ISFEE == "Y")
                        {
                            if (today == created)
                            {
                                priceDto.VISIT_ID = newSaved.ID;
                                DeleteVisitPrice(priceDto);
                            }
                            else
                            {
                                NewVisitAndMinusPrice(saved);
                                priceDto.VISIT_ID = saved.ID;
                                oshaVisitPriceService.Save(priceDto); // 수정된 금액 발생
                            }
                        }
                        else if (saved.ISFEE == "Y" && newSaved.ISFEE == "N")
                        {
                            if (today == created)
                            {
                                priceDto.VISIT_ID = newSaved.ID;
                                // DeleteVisitPrice(priceDto);
                                oshaVisitPriceService.Delete(priceDto);
                            }
                            else
                            {
                                NewVisitAndMinusPrice(saved);
                                oshaVisitPriceService.Save(priceDto);
                            }
                        }
                        else if (saved.ISFEE == "N" && newSaved.ISFEE == "Y")
                        {
                            priceDto.VISIT_ID = newSaved.ID;
                            DeleteVisitPrice(priceDto);
                        }
                    }
              
                }

           

            }
            return newSaved;


            //if (today != created)
            //{
            //    if (saved.ISFEE == "N" && newSaved.ISFEE == "N")
            //    {
            //        DeleteVisit(dto);

            //    }
            //    else if (saved.ISFEE == "Y" && priceDto != null)
            //    {

            //    }
            //}
            //else
            //{
            //    priceDto.VISIT_ID = saved.ID;
            //    this.oshaVisitPriceService.DeleteAndSave(priceDto);
            //}



        }
        /// <summary>
        /// 방문수수료 삭제하고 새로 발생
        /// </summary>
        /// <param name="priceDto"></param>
        private void DeleteVisitPrice(OSHA_VISIT_PRICE priceDto)
        {
         
            this.oshaVisitPriceService.DeleteAndSave(priceDto);

        }

    }
 }
