using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase.Mvc.UserControls
{
    public partial class ContentTitle : UserControl
    {
        [Description("컨텐트 제목")]
        public ContentTitle()
        {
            InitializeComponent();
        }
        [Category("A-MTS-Framework-Properties")]
        [Description("컨텐츠 타이틀")]
        public string TitleText
        {
            get { return lblTitle.Text; }
            set
            {
                lblTitle.Text = value;

            }
        }
    }
}
