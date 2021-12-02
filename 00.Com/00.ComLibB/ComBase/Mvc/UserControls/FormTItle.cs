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
    public partial class FormTItle : UserControl
    {
        [Description("폼제목 목록 ")]
        public FormTItle()
        {
            InitializeComponent();
        }
        [Category("A-MTS-Framework-Properties")]
        [Description("폼 타이틀")]
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
