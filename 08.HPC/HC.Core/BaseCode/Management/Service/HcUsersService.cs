namespace HC.Core.BaseCode.Management.Service
{
    using System.Collections.Generic;
    using HC.Core.BaseCode.Management.Repository;
    using HC.Core.BaseCode.Management.Dto;
    
    
    /// <summary>
    /// �ý��� ����� ����
    /// </summary>
    public class HcUsersService
    {
        
        private HcUsersRepository hcUsersRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HcUsersService()
        {
			this.hcUsersRepository = new HcUsersRepository();
        }
        /// <summary>
        /// �ǻ� ��� 
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetDoctors()
        {
            return hcUsersRepository.FindDoctors();
        }
        /// <summary>
        /// ��ȣ�� ��� 
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetNurse()
        {
            return hcUsersRepository.FindNurse();
        }
        /// <summary>
        /// ���ǰ������� ���� ���
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetOsha()
        {
            return hcUsersRepository.FindOSHA();
        }
        /// <summary>
        /// ���ǰ��� ���������� ���
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
            //����ڰ� ���Ǵ��� Ȯ�� ��ƾ �ʿ���
            return hcUsersRepository.Delete(user);
        }
    
        public List<HC_USER> FindAll()
        {
            return hcUsersRepository.FindAll();
        }
    }
}
