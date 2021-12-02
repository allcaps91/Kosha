using HC.Core.BaseCode.MSDS.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Spread.CustomCellType;

namespace HC_OSHA
{
    public partial class GHS_PictureForm : CommonForm
    {
        /// <summary>
        /// 선택된 이미지 문자열
        /// GHS01.gif|GHS02.gif
        /// </summary>
        public string PhictureString { get; set; }

        private HcMsdsService hcMsdsService;
        public GHS_PictureForm()
        {
            InitializeComponent();
            this.hcMsdsService = new HcMsdsService();
        }

        private void GHS_PictureForm_Load(object sender, EventArgs e)
        {

            ChkGHS01.SetOptions(new CheckBoxOption { CheckValue = "GHS01.gif", UnCheckValue = "" });
            ChkGHS02.SetOptions(new CheckBoxOption { CheckValue = "GHS02.gif", UnCheckValue = "" });
            ChkGHS03.SetOptions(new CheckBoxOption { CheckValue = "GHS03.gif", UnCheckValue = "" });
            ChkGHS04.SetOptions(new CheckBoxOption { CheckValue = "GHS04.gif", UnCheckValue = "" });
            ChkGHS05.SetOptions(new CheckBoxOption { CheckValue = "GHS05.gif", UnCheckValue = "" });
            ChkGHS06.SetOptions(new CheckBoxOption { CheckValue = "GHS06.gif", UnCheckValue = "" });
            ChkGHS07.SetOptions(new CheckBoxOption { CheckValue = "GHS07.gif", UnCheckValue = "" });
            ChkGHS08.SetOptions(new CheckBoxOption { CheckValue = "GHS08.gif", UnCheckValue = "" });
            ChkGHS09.SetOptions(new CheckBoxOption { CheckValue = "GHS09.gif", UnCheckValue = "" });

             int index = 0;
            foreach (KeyValuePair<string, Image> item in hcMsdsService.GhsPictures)
            {
                PictureBox pic = new PictureBox();
                pic.Image = item.Value;
                pic.Size = new Size(65, 65);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                // pic.Margin = new Padding(110, 0, 130, 0);
                pic.Location = new Point(75 * index, 35);
                picturePanel.Controls.Add(pic);
                ++index;
            }
            if (PhictureString.NotEmpty())
            {
                string[] fileNames = PhictureString.Split('|');
                foreach(string fileName in fileNames)
                {
                    for (int i = 0; i < CheckboxPanel.Controls.Count; i++)
                    {
                        CheckBox chk = CheckboxPanel.Controls[i] as CheckBox;
                        CheckBoxOption option = chk.Tag as CheckBoxOption;
                        if (option.CheckValue.Equals(fileName))
                        {
                            chk.Checked = true;
                        }
                    }
                        
                }
            }
            //panPictures
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string ghs_pictures = string.Empty;
            for(int i= 0; i < CheckboxPanel.Controls.Count;  i++) 
            {
                CheckBox chk = CheckboxPanel.Controls[i] as CheckBox;
                if(chk.GetValue().IsNullOrEmpty() == false)
                {
                    ghs_pictures += chk.GetValue() + "|";
                }
                
            }

            this.PhictureString = ghs_pictures;

            this.DialogResult = DialogResult.OK;

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
