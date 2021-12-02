using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupXRYSET02.cs
    /// Description     : 영상의학과 판독관련 판독상용구 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 XuRead07.frm(FrmResultSet) 폼 frmComSupXRYSET02.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\xray\xuread\XuRead07.frm >> frmComSupXRYSET02.cs 폼이름 재정의" />
    public partial class frmComSupXRYSET02 : Form
    {
        public frmComSupXRYSET02()
        {
            InitializeComponent();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData))
            {
                // 여기에 처리코드를 넣는다.
                if (keyData.Equals(Keys.F1))
                {                    
                    return true;
                }
                else if (keyData.Equals(Keys.F2))
                {                 
                    return false;
                }
                else if (keyData.Equals(Keys.F3))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F4))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F5))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F6))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F7))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F8))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F9))
                {
                    return false;
                }
                else if (keyData.Equals(Keys.F10))
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

    }
}
