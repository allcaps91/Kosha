namespace HC.Core.BaseCode.Management.Service
{
    using System.Collections.Generic;
    using HC.Core.BaseCode.Management.Repository;
    using HC.Core.BaseCode.Management.Dto;
    using ComBase.Mvc.Spread;


    /// <summary>
    /// 시스템 사용자 관리
    /// </summary>
    public class HcUserService
    {
        
        private HcUsersRepository hcUsersRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcUserService()
        {
			this.hcUsersRepository = new HcUsersRepository();
        }
        /// <summary>
        /// 의사 목록 
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetDoctors()
        {
            return hcUsersRepository.FindDoctors();
        }
        /// <summary>
        /// 간호사 목록 
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetNurse()
        {
            return hcUsersRepository.FindNurse();
        }
        /// <summary>
        /// 보건관리전문 직원 목록
        /// </summary>
        /// <returns></returns>
        public SpreadComboBoxData GetSpreadByOsha()
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HC_USER> list = hcUsersRepository.FindOSHA();

            foreach (HC_USER dto in list)
            {
                data.Put(dto.UserId, dto.Name);
            }
            return data;
        }

        /// <summary>
        /// 보건관리전문 직원 목록
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetOsha()
        {
            return hcUsersRepository.FindOSHA();
        }
        /// <summary>
        /// 보건관리 산업위생기사 목록
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetEngineerByOsha()
        {
            return hcUsersRepository.FinnEngineerByOSHA();
        }
        public void Create(HC_VIEW_USER viewUser)
        {
            HC_USER dto = hcUsersRepository.FindOne(viewUser.UserId);
            if (dto == null)
            {
                if(viewUser.Dept == "DOCTOR")
                {
                    hcUsersRepository.Insert(viewUser, viewUser.Dept);
                }
                else
                {
                    hcUsersRepository.Insert(viewUser, "");
                }
          
            }
            else
            {
                dto.ISDELETED = ComBase.Mvc.Enums.IsDeleted.N;
                hcUsersRepository.Update(dto);
            }
        }
        public void Save(HC_USER user)
        {
            if (user.RowStatus == ComBase.Mvc.RowStatus.Update)
            {
                hcUsersRepository.Update(user);
            }
            else
            {
                hcUsersRepository.Delete(user);
            }
             
        }

        public int Delete(HC_USER user)
        {
            //사용자가 사용되는지 확인 루틴 필요함
            return hcUsersRepository.Delete(user);
        }
    
        public List<HC_USER> FindAll()
        {
            return hcUsersRepository.FindAll();
        }
    }
}
