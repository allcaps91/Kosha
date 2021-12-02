using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Spread;
using HC.Core.BaseCode.Management.Dto;
using HC.Core.BaseCode.Management.Repository;
using HC.Core.BaseCode.Management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC.Core.Common.Service
{
    /// <summary>
    /// 기초코드 및 자주사용하는 서비스 싱글톤
    /// </summary>
    public class CommonService
    {
        private readonly static object Lock_Object = new object();
        private static CommonService instance;
        private readonly HcCodeService hcCodeService;
        private readonly HcUserService userService;
        private Dictionary<String, Form> formList;
        public Session Session { get; set; } 
        public CommonService()
        {
            this.Session = new Session("41783");


            hcCodeService = new HcCodeService();
            userService = new HcUserService();
            formList = new Dictionary<string, Form>();
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
                    return instance;
                }
               
            }
        }

        public HcCodeService CodeService
        {
            get
            {
                return this.hcCodeService;
            }
        }
        public HcUserService UserService
        {
            get
            {
                return this.userService;
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
