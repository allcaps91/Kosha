using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using HC.Core.Model;
using HC.Core.Service;
using HC_Core.Service;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HC_Core
{
    public partial class CommonForm : BaseForm, MainFormMessage
    {

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage
        protected HcCodeService codeService;
        protected HicCodeService hiccodeService;
        protected HcUserService userService;
        protected Session session;

       
        [Browsable(false)]
        public bool IsDesignMode
        {

            get { return (LicenseManager.UsageMode == LicenseUsageMode.Designtime); }
        }

        /// <summary>
        /// 선택된 사업장
        /// </summary>
        public ISiteModel SelectedSite { get; set; }

        /// <summary>
        /// 선택된 계약정보
        /// </summary>
        public IEstimateModel SelectedEstimate{ get; set; }
        public CommonForm()
        {
            InitializeComponent();

            Init();
        }
        public CommonForm(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            Init();
        }

        private void Init()
        {
            if (!IsInDesigner)
            {
                codeService = CommonService.Instance.CodeService;
                hiccodeService = CommonService.Instance.HicCodeService;
                userService = CommonService.Instance.UserService;
                session = CommonService.Instance.Session;
            }
        }
        protected static bool IsInDesigner
        {
            get { return (System.Reflection.Assembly.GetEntryAssembly() == null); }
        }
        public void SetSelectedSite(long siteId)
        {
            SelectedSite model = new SelectedSite
            {
                ID = siteId
            };
            this.SelectedSite = model;
        }

        public void SetSelectedEstimate(long estimateId)
        {
            SelectedEstimate model = new SelectedEstimate
            {
                ID = estimateId
            };
            this.SelectedEstimate = model;
        }
        public CommonForm AddForm(CommonForm form, Control target)
        {
            foreach(Control control in target.Controls)
            {
                target.Controls.Remove(control);
            }

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            
            target.Controls.Add(form);
            form.Show();
            return form;
        }
        
        //계약 기준 금년도
        public string GetCurrentYear()
        {
            string year = "";
            if (SelectedEstimate == null)
            {
                return "";
            }
            if (SelectedEstimate.CONTRACTENDDATE == null)
            {
                //year = DateTime.Now.Year.ToString();
                MessageUtil.Alert("계약종료일이 없습니다");
            }
            else
            {
                if (DateTime.Now.Year > DateUtil.stringToDateTime(SelectedEstimate.CONTRACTENDDATE, DateTimeType.YYYY_MM_DD).Year)
                {
                    year = SelectedEstimate.CONTRACTENDDATE.Substring(0, 4);
                }
                else
                {
                    year = DateTime.Now.Year.ToString().Substring(0, 4);
                }

            }
            return year;
            //string lastYear = (int.Parse(year) - 1).ToString();
        }
        /// <summary>
        /// 계약 기준년도 작년
        /// </summary>
        /// <returns></returns>
        public string GetLastYear(string year)
        {
            if(year == "")
            {
                return "";
            }
            return (int.Parse(year) - 1).ToString();

        }

        private void CommonForm_Load(object sender, EventArgs e)
        {
         
            if (DataSyncService.Instance.IsLocalDB == true)
            {
                this.Text = "[오프라인] " + this.Text + "  출장프로그램";
          

            }
        
        }
 
        private void CommonForm_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void CommonForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
          
        }

        /*

        [Browsable(false)]
        public string CurrentDateString
        {
            get
            {
                if (!IsDesignMode && clsDB.DbCon!=null)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return clsPublic.GstrSysDate;
                }

                return string.Empty;
            }
        }
        [Browsable(false)]
        public string CurrentTimeString
        {
            get
            {
                if (!IsDesignMode && clsDB.DbCon != null)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return clsPublic.GstrSysTime;
                }

                return string.Empty;
            }
        }
        [Browsable(false)]
        public string CurrentYearString
        {
            get
            {
                if (!IsDesignMode && clsDB.DbCon != null)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return clsPublic.GstrSysDate.Left(4);
                }
                return string.Empty;
            }
        }
       
        [Browsable(false)]
        public DateTime CurrentYear
        {
            get
            {
                if (!IsDesignMode && clsDB.DbCon != null)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return DateTime.ParseExact(string.Concat(clsPublic.GstrSysDate.Left(4), "0101"), "yyyyMMdd", null);
                }
                return DateTime.MinValue;
            }
        }
        [Browsable(false)]
        public DateTime CurrentTime
        {
            get
            {
                if (!IsDesignMode && clsDB.DbCon != null)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    return DateTime.ParseExact(clsPublic.GstrSysTime, "HH:mm", null);
                }
                return DateTime.MinValue;
            }
        }
        */
     
      
    }
}
