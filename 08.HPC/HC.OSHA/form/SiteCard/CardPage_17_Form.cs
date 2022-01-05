using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_17_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        HcOshaCard21Service hcOshaCard21Service;
        List<CheckBox> Clean1CheckboxList = null;
        List<CheckBox> Clean2CheckboxList = null;
        List<CheckBox> Clean3CheckboxList = null;
        public CardPage_17_Form()
        {
            InitializeComponent();
            hcOshaCard21Service = new HcOshaCard21Service();
            Clean1CheckboxList = new List<CheckBox>();
            Clean2CheckboxList = new List<CheckBox>();
            Clean3CheckboxList = new List<CheckBox>();
        }


        private void CardPage_17_Form_Load(object sender, EventArgs e)
        {
            Init();

            Clean1CheckboxList.Add(Clean1_Chk1);
            Clean1CheckboxList.Add(Clean1_Chk2);
            Clean1CheckboxList.Add(Clean1_Chk3);
            Clean1CheckboxList.Add(Clean1_Chk4);
            Clean1CheckboxList.Add(Clean1_Chk5);
            Clean1CheckboxList.Add(Clean1_Chk6);
        }


        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            
            Clear();

            Search();


        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

        private void Clear()
        {
            pan21.Initialize();
            pictureBox1.Image = null;
            SSCard.ActiveSheet.Cells[15, 0].Value = null;

            TxtFileName.Text = string.Empty;

            SSCard.ActiveSheet.Cells[1, 4].Value = false;
            SSCard.ActiveSheet.Cells[1, 5].Value = false;

            SSCard.ActiveSheet.Cells[2, 2].Value = "(의료기관명:        )";
            SSCard.ActiveSheet.Cells[1, 11].Value = string.Empty;
            SSCard.ActiveSheet.Cells[2, 11].Value = string.Empty;

            SSCard.ActiveSheet.Cells[3, 3].Value = false;
            SSCard.ActiveSheet.Cells[3, 4].Value = false;
            
            SSCard.ActiveSheet.Cells[4, 3].Value = string.Empty;
      
            SSCard.ActiveSheet.Cells[3, 9].Value = false;
            SSCard.ActiveSheet.Cells[3, 10].Value = false;
                     
            SSCard.ActiveSheet.Cells[3, 15].Value = false;
            SSCard.ActiveSheet.Cells[3, 18].Value = false;


            //위생기구
            SSCard.ActiveSheet.Cells[5, 4].Value = "( )";//체온계
            SSCard.ActiveSheet.Cells[5, 7].Value =  "( )";//가위
            SSCard.ActiveSheet.Cells[5, 10].Value ="( )";//핀셋
            SSCard.ActiveSheet.Cells[5, 13].Value = "( )";//설압자
            SSCard.ActiveSheet.Cells[5, 16].Value =  "( )";//브먹
            SSCard.ActiveSheet.Cells[5, 19].Value =  "( )";//삼각건
            SSCard.ActiveSheet.Cells[6, 4].Value =  "( )";//혈압켸
            SSCard.ActiveSheet.Cells[6, 7].Value = "";

            //위생믈픔
            SSCard.ActiveSheet.Cells[7, 4].Value =  "( )";//탄력붕대
            SSCard.ActiveSheet.Cells[7, 7].Value = "( )";//
            SSCard.ActiveSheet.Cells[7, 10].Value = "( )";//
            SSCard.ActiveSheet.Cells[7, 13].Value = "( )";//
            SSCard.ActiveSheet.Cells[7, 16].Value = "( )";//
            SSCard.ActiveSheet.Cells[7, 19].Value = "( )";//
            SSCard.ActiveSheet.Cells[8, 4].Value = "";

            //;외용약
            SSCard.ActiveSheet.Cells[9, 5].Value =  "( )";//
            SSCard.ActiveSheet.Cells[9, 8].Value =  "( )";//
            SSCard.ActiveSheet.Cells[9, 11].Value ="( )";//
            SSCard.ActiveSheet.Cells[9, 14].Value = "( )";//
            SSCard.ActiveSheet.Cells[9, 18].Value =  "( )";//
            SSCard.ActiveSheet.Cells[10, 4].Value ="";

            SSCard.ActiveSheet.Cells[11, 13].Value ="";

        }

          private void Init()
        {

            RdoISHOSPITAL_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISHOSPITAL), CheckValue = "0", });
            RdoISHOSPITAL_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISHOSPITAL), CheckValue = "1",  });
            TxtHOSPITALNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.HOSPITALNAME) });
            TxtADDRESS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.ADDRESS) });
            TxtTel.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.TEL) });

            RdoISMANAGER_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISMANAGER), CheckValue = "0",  });
            RdoISMANAGER_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISMANAGER), CheckValue = "1", });
            TxtMANAGERNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.MANAGERNAME) });

            RdoISAIDKIT_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISAIDKIT), CheckValue = "0",  });
            RdoISAIDKIT_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.ISAIDKIT), CheckValue = "1",  });

            RdoAIDKITTYPE_0.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.AIDKITTYPE), CheckValue = "0",  });
            RdoAIDKITTYPE_1.SetOptions(new RadioButtonOption { DataField = nameof(HC_OSHA_CARD21.AIDKITTYPE), CheckValue = "1",  });
            TxtREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.REMARK) });

            TxtCLEAN1ETC.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.CLEAN1ETC) });
            TxtCLEAN2ETC.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.CLEAN2ETC) });
            TxtCLEAN3ETC.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD21.CLEAN3ETC) });



        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null || base.SelectedSite == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan21.Validate<HC_OSHA_CARD21>())
                {
                    HC_OSHA_CARD21 dto = pan21.GetData<HC_OSHA_CARD21>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;

                    dto.CLEAN1CHECKBOX = GetCheckedValue(CleanBox1);
                    dto.CLEAN2CHECKBOX = GetCheckedValue(CleanBox2);
                    dto.CLEAN3CHECKBOX = GetCheckedValue(CleanBox3);

                    //dto.YEAR = base.GetCurrentYear();
                    dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                    HC_OSHA_CARD21 saved = hcOshaCard21Service.Save(dto);

                    MessageUtil.Info("저장하였습니다.");
                    //    pan21.SetData(saved);
                    Search();
                    
                }
            }
        }

        private string GetCheckedValue(GroupBox groupbox)
        {
            List<CheckBox> checkboxList = new List<CheckBox>();
            string value = string.Empty;
            foreach(Control control in groupbox.Controls)
            {
                if(control is CheckBox)
                {
                    CheckBox chk = control as CheckBox;
                    checkboxList.Add(chk);
                }
            }
            checkboxList = checkboxList.OrderBy(c => c.Name).ToList();

            foreach(CheckBox chk in checkboxList)
            {
                if (chk.Checked)
                {
                    value += "1";
                }
                else
                {
                    value += "0";
                }
            }


            return value;
        }

        private void SetCheckedValue(string value, GroupBox groupbox)
        {
            if (value != null)
            {
                List<CheckBox> checkboxList = new List<CheckBox>();
                foreach (Control control in groupbox.Controls)
                {
                    if (control is CheckBox)
                    {
                        CheckBox chk = control as CheckBox;
                        checkboxList.Add(chk);
                    }
                }
                checkboxList = checkboxList.OrderBy(c => c.Name).ToList();

                for (int i = 0; i < checkboxList.Count; i++)
                {
                    if (value.Substring(i, 1) == "1")
                    {
                        checkboxList[i].Checked = true;
                    }
                    else
                    {
                        checkboxList[i].Checked = false;
                    }
                }


            }
         
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD21 dto = pan21.GetData<HC_OSHA_CARD21>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard21Service.Delete(dto);
                    pan21.Initialize();

                }
            }
        }


        private void Search()
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            HC_OSHA_CARD21 dto = hcOshaCard21Service.hcOshaCard21Repository.FindByEstimateId(base.SelectedEstimate.ID, base.GetCurrentYear());
            if (dto != null)
            {
                pan21.SetData(dto);

                SetCheckedValue(dto.CLEAN1CHECKBOX, CleanBox1);
                SetCheckedValue(dto.CLEAN2CHECKBOX, CleanBox2);
                SetCheckedValue(dto.CLEAN3CHECKBOX, CleanBox3);

                if (dto.ISHOSPITAL == "0")
                {
                    SSCard.ActiveSheet.Cells[1, 4].Value = true;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[1, 5].Value = true;
                }

                SSCard.ActiveSheet.Cells[2, 2].Value = "(의료기관명:" + dto.HOSPITALNAME + ")";
                SSCard.ActiveSheet.Cells[1, 11].Value = dto.ADDRESS;
                SSCard.ActiveSheet.Cells[2, 11].Value = dto.TEL;

                //응급처치담당자 지정
                if (dto.ISMANAGER == "0")
                {
                    SSCard.ActiveSheet.Cells[3, 3].Value = true;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[3, 4].Value = true;
                }
                SSCard.ActiveSheet.Cells[4, 3].Value = dto.MANAGERNAME;

                //구급함
                if (dto.ISAIDKIT == "0")
                {
                    SSCard.ActiveSheet.Cells[3, 9].Value = true;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[3, 10].Value = true;
                }
                if (dto.AIDKITTYPE == "0")
                {
                    SSCard.ActiveSheet.Cells[3, 15].Value = true;
                }
                else
                {
                    SSCard.ActiveSheet.Cells[3, 18].Value = true;
                }

                //위생기구
                SSCard.ActiveSheet.Cells[5, 4].Value = dto.CLEAN1CHECKBOX[0].Equals('1') ? "(○)" : "( )";//체온계
                SSCard.ActiveSheet.Cells[5, 7].Value = dto.CLEAN1CHECKBOX[1].Equals('1') ? "(○)" : "( )";//가위
                SSCard.ActiveSheet.Cells[5, 10].Value = dto.CLEAN1CHECKBOX[2].Equals('1') ? "(○)" : "( )";//핀셋
                SSCard.ActiveSheet.Cells[5, 13].Value = dto.CLEAN1CHECKBOX[3].Equals('1') ? "(○)" : "( )";//설압자
                SSCard.ActiveSheet.Cells[5, 16].Value = dto.CLEAN1CHECKBOX[4].Equals('1') ? "(○)" : "( )";//브먹
                SSCard.ActiveSheet.Cells[5, 19].Value = dto.CLEAN1CHECKBOX[5].Equals('1') ? "(○)" : "( )";//삼각건
                SSCard.ActiveSheet.Cells[6, 4].Value = dto.CLEAN1CHECKBOX[6].Equals('1') ? "(○)" : "( )";//혈압켸
                SSCard.ActiveSheet.Cells[6, 7].Value = dto.CLEAN1ETC;
                
                //위생믈픔
                SSCard.ActiveSheet.Cells[7, 4].Value = dto.CLEAN2CHECKBOX[0].Equals('1') ? "(○)" : "( )";//탄력붕대
                SSCard.ActiveSheet.Cells[7, 7].Value = dto.CLEAN2CHECKBOX[1].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[7, 10].Value = dto.CLEAN2CHECKBOX[2].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[7, 13].Value = dto.CLEAN2CHECKBOX[3].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[7, 16].Value = dto.CLEAN2CHECKBOX[4].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[7, 19].Value = dto.CLEAN2CHECKBOX[5].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[8, 4].Value = dto.CLEAN2ETC;


                //;외용약
                SSCard.ActiveSheet.Cells[9, 5].Value = dto.CLEAN3CHECKBOX[0].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[9, 8].Value = dto.CLEAN3CHECKBOX[1].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[9, 11].Value = dto.CLEAN3CHECKBOX[2].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[9, 14].Value = dto.CLEAN3CHECKBOX[3].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[9, 18].Value = dto.CLEAN3CHECKBOX[4].Equals('1') ? "(○)" : "( )";//
                SSCard.ActiveSheet.Cells[10, 4].Value = dto.CLEAN3ETC;

                SSCard.ActiveSheet.Cells[11, 13].Value = dto.REMARK;

            }

            HC_OSHA_CARD22 saved = hcOshaCard21Service.FindImage(base.SelectedEstimate.ID);
            if (saved != null)
            {
                pictureBox1.Image = byteArrayToImage(saved.ImageData);
                SSCard.ActiveSheet.Cells[15, 0].Value = saved.ImageData;
            }
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Write(byteArrayIn, 0, byteArrayIn.Length);
            return  Image.FromStream(ms, true);
           
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result =  dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                TxtFileName.Text = dialog.FileName;

                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length - 1];
                fs.Read(b, 0, b.Length);
                fs.Close();
                pictureBox1.Image = byteArrayToImage(b);

                SSCard.ActiveSheet.Cells[15, 0].Value = pictureBox1.Image;
            }
            

        }

        private void BtnDeleteImage_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                HC_OSHA_CARD22 dto = new HC_OSHA_CARD22();
                dto.ID = base.SelectedSite.ID;
                dto.ESTIMATE_ID = base.SelectedEstimate.ID;

                hcOshaCard21Service.DeleteImage(dto);

                pictureBox1.Image = null;

                Search();
            }
                
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                return;
            }
            if (!TxtFileName.Text.IsNullOrEmpty())
            {
                HC_OSHA_CARD22 dto = new HC_OSHA_CARD22();
                dto.SITE_ID = base.SelectedSite.ID;
                dto.ESTIMATE_ID = base.SelectedEstimate.ID;

                FileStream fs = new FileStream(TxtFileName.Text, FileMode.Open, FileAccess.Read);
                byte[] b = new byte[fs.Length - 1];
                fs.Read(b, 0, b.Length);
                fs.Close();
                dto.ImageData = b;
                //dto.YEAR = base.GetCurrentYear();
                dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                this.hcOshaCard21Service.SaveImage(dto);

                MessageUtil.Info("저장되었습니다");
            }
        
        }
        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void RdoISHOSPITAL_1_Click(object sender, EventArgs e)
        {
            TxtHOSPITALNAME.Text = "";
            TxtADDRESS.Text = "";
            TxtTel.Text = "";
        }

        private void RdoISHOSPITAL_0_Click(object sender, EventArgs e)
        {
            TxtHOSPITALNAME.Text = "(재)포항성모병원";
            TxtADDRESS.Text = "포항시 남구 대잠동길 17";
            TxtTel.Text = "(☏289-4658)";
        }
    }
}
