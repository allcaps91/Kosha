using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Spread;
using ComHpcLibB.Service;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.Service
{
    /// <summary>
    /// 기초코드 및 자주사용하는 서비스 싱글톤
    /// </summary>
    public class CommonService
    {
        private readonly static object Lock_Object = new object();
        private static CommonService instance;
        private HcCodeService hcCodeService;
        private HicCodeService hicCodeService;
        private HcUserService userService;
        private Dictionary<String, Form> formList;
        public Session Session { get; set; }

      
        public CommonService()
        {

            //this.Session = new Session(clsType.User.Sabun, clsType.User.UserName);
            this.Session = new Session(clsType.User.Sabun);

            hcCodeService = new HcCodeService();
            hicCodeService = new HicCodeService();
            userService = new HcUserService();
            formList = new Dictionary<string, Form>();
            if( userService!=null)
            {
                HC_USER user = userService.FindByUserId(Session.UserId);
                if (user != null)
                {
                    this.Session.Program = user.Dept;
                    Role r = (Role)Enum.Parse(typeof(Role), user.Role);

                    this.Session.Role = r;

                }
            }
       
        }
        public static CommonService Instance
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        instance = new CommonService();
                    }
                    if(instance.Session.UserId != clsType.User.Sabun)
                    {
                        instance = new CommonService();
                    }
                    return instance;
                }
               
            }
        }

        public HcCodeService CodeService
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        hcCodeService = new HcCodeService();
                    }
                }
                return hcCodeService;
            }
        }
        public HcUserService UserService
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        userService = new HcUserService();
                    }
                }
                return userService;
            }
        }

        public HicCodeService HicCodeService
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        hicCodeService = new HicCodeService();
                    }
                }
                return hicCodeService;
            }
        }

        public void AddForm(Control control, Form form)
        {
            Form activeForm;
            string fullNamespace = form.GetType().FullName;
            if(formList.TryGetValue(fullNamespace, out activeForm))
            {
                activeForm.Close();
                activeForm.Dispose();
                formList.Remove(fullNamespace);
                
            }

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
            control.Controls.Add(form);

            formList.Add(fullNamespace, form);
        }

  
    }
}
