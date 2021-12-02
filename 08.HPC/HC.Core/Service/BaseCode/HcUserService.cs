namespace HC.Core.Service
{
    using System.Collections.Generic;
    using HC.Core.Repository;
    using HC.Core.Dto;
    using ComBase.Mvc.Spread;
    using ComBase;
    using System;


    /// <summary>
    /// �ý��� ����� ����
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
        /// ���ǰ������� ���� ���
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetOsha()
        {
            return hcUsersRepository.FindOSHA();
        }

        /// <summary>
        /// �۾�ȯ������ ����
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetWem()
        {
            return hcUsersRepository.FindWEM();
        }
        /// <summary>
        /// �۾�ȯ������ �м���
        /// </summary>
        /// <returns></returns>
        public List<HC_USER> GetAnalyst()
        {
            return hcUsersRepository.FindAnaylist();
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
                if(viewUser.Dept == "DOCTOR" || viewUser.Dept == "�۾�ȯ�����а�")
                {
                    viewUser.Dept = "OSHA";
                    hcUsersRepository.Insert(viewUser, "DOCTOR");
                }
                else if (viewUser.Dept == "NURSE" || viewUser.Dept == "���ǰ�������")
                {
                    viewUser.Dept = "OSHA";
                    hcUsersRepository.Insert(viewUser, "NURSE");
                }
                else
                {
                    viewUser.Dept = "WEM";
                    hcUsersRepository.Insert(viewUser, "ENGINEER");
                }
          
            }
            else
            {
                dto.ISDELETED = ComBase.Mvc.Enums.IsDeleted.N;
                hcUsersRepository.Update(dto);
            }
        }

        public HC_USER FindByName(string strSName)
        {
            return hcUsersRepository.FindByName(strSName);
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

        public HC_USER FindByUserId(string userId)
        {
            return hcUsersRepository.FindOne(userId);
        }

        public bool IsValid(string userId, string password)
        {
            
            HC_USER user = hcUsersRepository.FindOne(userId);
            if(user == null)
            {
                return false;
            }
            else
            {
                if(user.PASSHASH256 == clsSHA.SHA256(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
