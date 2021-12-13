namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Model;
    using ComBase;
    using ComBase.Mvc.Utils;
    using HC.Core.Dto;
    using HC.Core.Repository;
    using HC.Core.Service;
    using System;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;

    /// <summary>
    /// 
    /// </summary>
    public class HcOshaSiteService
    {
        
        public  HcOshaSiteRepository HcOshaSiteRepository { get; }
        private HcOshaSiteModelRepository hcOshaSiteModelRepository;
        private HcUsersRepository hcUsersRepository;
        /// <summary>
        /// 
        /// </summary>
        public HcOshaSiteService()
        {
			this.HcOshaSiteRepository = new HcOshaSiteRepository();
            this. hcOshaSiteModelRepository = new HcOshaSiteModelRepository();
            hcUsersRepository = new HcUsersRepository();
        }

        /// <summary>
        /// 보건관리 사업장 등록
        /// </summary>
        /// <param name="view"></param>
        public bool Save(HC_SITE_VIEW view)
        {
            bool result = false;
            HC_OSHA_SITE site = HcOshaSiteRepository.FindById(view.ID);
            if(site == null)
            {
                HcOshaSiteRepository.Insert(view);
                result = true;
            }
            return result;            
        }

        
        public HC_OSHA_SITE_MODEL FindById(long id, string userId ="")
        {
           return hcOshaSiteModelRepository.FindById(id, userId);
           
        }

        public List<HC_OSHA_SITE_MODEL> FindChild(long id)
        {
            return hcOshaSiteModelRepository.FindChild(id);
        }
        /// <summary>
        /// 아이디 또는 이름으로 검색
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<HC_OSHA_SITE_MODEL> Search(string name, string userId, bool isOsha, bool isSchedule)
        {
            List<HC_OSHA_SITE_MODEL> list = null;
            HC_USER user = hcUsersRepository.FindOne(userId);
            Role role = Role.ENGINEER;

            if (user!= null)
            {
                role = (Role)Enum.Parse(typeof(Role), user.Role);
            }
            if (clsType.User.LtdUser != "") //고객사 사용자
            {
                list = hcOshaSiteModelRepository.FindByLtduser();
            }
            else if (name.IsNumeric())
            {
                list = hcOshaSiteModelRepository.FindById(name, userId, role, isOsha, isSchedule);
            }
            else
            {
                list = hcOshaSiteModelRepository.FindByName(name, userId, role, isOsha, isSchedule);
            }
            if (list.Count == 0)
            {
                if (name.IsNumeric())
                {
                    list = hcOshaSiteModelRepository.FindById(name, "", role, isOsha, isSchedule);
                }
                else
                {
                    list = hcOshaSiteModelRepository.FindByName(name, "", role, isOsha, isSchedule);
                }
            }

            //하청 트리 만들기
            if (isOsha == false)
            {
                if (list.Count == 1)
                {
                    //검색결과가 하나이고 부모가 있을경우
                    if (list[0].PARENTSITE_ID > 0)
                    {
                        name = list[0].PARENTSITE_ID.ToString();
                        List<HC_OSHA_SITE_MODEL> tmp = hcOshaSiteModelRepository.FindById(name, userId, role, true, false);
                        tmp.Add(list[0]);
                        return tmp;
                    }
                    else
                    {
                        if (list[0].HASCHILD == "Y")
                        {
                            AddChild(ref list);
                        }
                    }
                }
                if (list.Count > 1)
                {
                    AddChild(ref list);
                }
            }
      
            return list;
        }
        private void AddChild(ref List<HC_OSHA_SITE_MODEL> list)
        {
            List<HC_OSHA_SITE_MODEL> tmp = new List<HC_OSHA_SITE_MODEL>();
            foreach (HC_OSHA_SITE_MODEL model in list)
            {
                if (model.HASCHILD == "Y")
                {
                    foreach (HC_OSHA_SITE_MODEL childModel in hcOshaSiteModelRepository.FindChild(model.ID))
                    {
                        tmp.Add(childModel);
                    }
                }
            }


            foreach (HC_OSHA_SITE_MODEL childModel in tmp)
            {

                if (HasChild(list, childModel) == false)
                {
                    list.Add(childModel);
                }
            }
        }
        private bool HasChild(List<HC_OSHA_SITE_MODEL> list, HC_OSHA_SITE_MODEL child)
        {
            foreach(HC_OSHA_SITE_MODEL model in list)
            {
                if(model.ID == child.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
