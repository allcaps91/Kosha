
namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Dto;
    using System;
    using ComBase;

    public class HcOshaEquipmentService
    {
        private HcOshaEquipmentRepository hcOshaEquipmentRepository;

        /// <summary>
        /// 
        /// </summary>
        public HcOshaEquipmentService()
        {
            this.hcOshaEquipmentRepository = new HcOshaEquipmentRepository();
        }

        public List<HC_OSHA_EQUIPMENT> FindAll(long siteId)
        {
            return hcOshaEquipmentRepository.FindAll(siteId);
        }
        public bool Save(IList<HC_OSHA_EQUIPMENT> list, long siteId)
        {
            try
            {
                foreach (HC_OSHA_EQUIPMENT dto in list)
                {
                    if (dto.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        dto.SITE_ID = siteId;
                        hcOshaEquipmentRepository.Insert(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hcOshaEquipmentRepository.Update(dto);
                    }
                    else if (dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hcOshaEquipmentRepository.Delete(dto.ID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
