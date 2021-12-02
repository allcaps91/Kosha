namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using HC.Core.Service;
    using static HC.Core.Service.LogService;
    using ComBase.Controls;
    using HC.OSHA.Model;


    /// <summary>
    /// 보건관리 계약 서비스
    /// </summary>
    public class HcOshaContractService
    {
        private HcOshaContractRepository hcOshaContractRepository;

        private CardPage1Service cardPage1Service;
        private HcOshaCard4_2Service hcOshaCard4_2Service;
        private HcOshaCard4_1Service hcOshaCard4_1Service;
        private HcOshaCard5Service hcOshaCard5Service;
        private HcOshaCard6Service hcOshaCard6Service;
        private HcOshaCard7Service hcOshaCard7Service;
        private HcOshaCard91Service hcOshaCard91Service;
        private HcOshaCard10Service hcOshaCard10Service;
        private HcOshaCard11_1Service hcOshaCard11_1Service;
        private HcOshaCard13Service hcOshaCard13Service;
        private HcOshaCard15Service hcOshaCard15Service;
        private HcOshaCard16Service hcOshaCard16Service;
        private HcOshaCard20Service hcOshaCard20Service;
        private HcOshaCard21Service hcOshaCard21Service;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaContractService()
        {
			this.hcOshaContractRepository = new HcOshaContractRepository();

            this.cardPage1Service = new CardPage1Service();
            this.hcOshaCard4_2Service = new HcOshaCard4_2Service();
            this.hcOshaCard4_1Service = new HcOshaCard4_1Service();
            this.hcOshaCard5Service = new HcOshaCard5Service();
            this.hcOshaCard6Service = new HcOshaCard6Service();
            this.hcOshaCard7Service = new HcOshaCard7Service();
            this.hcOshaCard91Service = new HcOshaCard91Service();
            this.hcOshaCard10Service = new HcOshaCard10Service();
            this.hcOshaCard11_1Service = new HcOshaCard11_1Service();
            this.hcOshaCard13Service = new HcOshaCard13Service();
            this.hcOshaCard15Service = new HcOshaCard15Service();
            this.hcOshaCard16Service = new HcOshaCard16Service();
            this.hcOshaCard20Service = new HcOshaCard20Service();
            this.hcOshaCard21Service = new HcOshaCard21Service();
        }

        public bool Copy(long siteId, long newEstimateId)
        {
            List<HC_OSHA_CONTRACT> list = hcOshaContractRepository.FindAllByEstimateId(siteId);
            HC_OSHA_CONTRACT newContract = null;
            long lastEstimateId = 0;
            if (list.Count == 1)
            {
                newContract = list[0];
                lastEstimateId = list[0].ESTIMATE_ID;
                newContract.ESTIMATE_ID = newEstimateId;                
             
            }
            else if (list.Count > 1)
            {
                newContract = list[1];
                lastEstimateId = list[1].ESTIMATE_ID;
                newContract.ESTIMATE_ID = newEstimateId;
            }
            else
            {
                return false;
            }

            if(lastEstimateId == 0)
            {
                return false;
            }
            if (lastEstimateId == newContract.ESTIMATE_ID)
            {
                return false;
            }

            int year = newContract.CONTRACTDATE.Substring(0, 4).To<int>() + 1;
            
            newContract.CONTRACTDATE = year + newContract.CONTRACTDATE.Substring(4, 6);
            newContract.CONTRACTSTARTDATE = year + newContract.CONTRACTSTARTDATE.Substring(4, 6);
            newContract.CONTRACTENDDATE = year + newContract.CONTRACTENDDATE.Substring(4, 6);
            newContract.TERMINATEDATE = null;
            Save(newContract);

            HcOshaContractManagerService hcOshaContractManagerService =  new HcOshaContractManagerService();
            List<HC_OSHA_CONTRACT_MANAGER_MODEL> maangerList = hcOshaContractManagerService.FindContractManager(lastEstimateId);
            foreach(HC_OSHA_CONTRACT_MANAGER_MODEL model in maangerList)
            {
                model.ESTIMATE_ID = newContract.ESTIMATE_ID;
                model.ID = 0;
                model.RowStatus = ComBase.Mvc.RowStatus.Insert;
            }
            hcOshaContractManagerService.Save(maangerList);


            //  관리카드 1 관리책임자, 선임 현황
            cardPage1Service.COPY_HIC_OSHA_CARD3(year, newEstimateId, lastEstimateId);
            //  관리카드 업무개요 (1)
            hcOshaCard4_2Service.COPY_HIC_OSHA_CARD4_2(year, newEstimateId, lastEstimateId);
            //  관리카드 업무개요 (2)
            hcOshaCard4_1Service.COPY_HIC_OSHA_CARD4_1(year, newEstimateId, lastEstimateId);
            //  입퇴사자 현황.. 제외 
            //  방문 스케쥴과 열결이 되어 있으므로 생성을 하지 못한다.
            hcOshaCard5Service.COPY_HIC_OSHA_CARD5(year, newEstimateId, lastEstimateId);
            //  재해현황
            hcOshaCard6Service.COPY_HIC_OSHA_CARD6(year, newEstimateId, lastEstimateId);
            //  안전보건관리 규정, 산업안전보건 위원회
            hcOshaCard7Service.COPY_HIC_OSHA_CARD7(year, newEstimateId, lastEstimateId);
            //  무재해 운동추진
            hcOshaCard10Service.COPY_HIC_OSHA_CARD10(year, newEstimateId, lastEstimateId, siteId);
            //  근로자건장증진운동
            hcOshaCard11_1Service.COPY_HIC_OSHA_CARD11_1(year, newEstimateId, lastEstimateId, siteId);
            //  보호구
            hcOshaCard13Service.COPY_HIC_OSHA_CARD13(year, newEstimateId, lastEstimateId, siteId);
            //  위험물질
            hcOshaCard15Service.COPY_HIC_OSHA_CARD15(year, newEstimateId, lastEstimateId, siteId);
            //  유해물질
            hcOshaCard16Service.COPY_HIC_OSHA_CARD16(year, newEstimateId, lastEstimateId, siteId);
            //  안전 · 보건관리전문기관에 대한 사업장의 만족도
            hcOshaCard20Service.COPY_HIC_OSHA_CARD20(year, newEstimateId, lastEstimateId, siteId);
            //  응급의료, 사업장 약도
            hcOshaCard21Service.COPY_HIC_OSHA_CARD21(year, newEstimateId, lastEstimateId, siteId);

            return true;
        }
        public HC_OSHA_CONTRACT FindByEstimateId(long estimateId)
        {
            return hcOshaContractRepository.FindByEstimateId(estimateId);
        }
        public bool Delete(long estimateId)
        {
            return hcOshaContractRepository.Delete(estimateId);
        }
        public HC_OSHA_CONTRACT Save(HC_OSHA_CONTRACT dto)
        {
            dto.Validate();

            LogService.Instance.Task(dto.OSHA_SITE_ID, TaskName.CONTRACT);


            HC_OSHA_CONTRACT saved = hcOshaContractRepository.FindByEstimateId(dto.ESTIMATE_ID);

            if (!dto.TERMINATEDATE.IsNullOrEmpty())
            {
                dto.ISCONTRACT = "N";
            }
            else
            {
                dto.ISCONTRACT = "Y";
            }

            if(saved == null)
            {
                return hcOshaContractRepository.Insert(dto);
            }
            else
            {
                if(dto.ISDELETED == ComBase.Mvc.Enums.IsDeleted.Y)
                {
                    dto.ISDELETED = ComBase.Mvc.Enums.IsDeleted.N;
                }
                return hcOshaContractRepository.Update(dto);
            }
            
        }
    }
}
