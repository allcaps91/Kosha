using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.BaseCode.Management.Model;
using HC.Core.BaseCode.Management.Service;
using HC.Core.BaseCode.MSDS.Dto;
using HC.Core.BaseCode.MSDS.Service;
using HC.Core.Common.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace HC.Core.BaseCode.Management.UI
{
    public partial class MsdsForm : CommonForm
    {
        private KoshaMsdsService koshaMsdsService;
        private HcMsdsService hcMsdsService;
   
        public MsdsForm()
        {
            InitializeComponent();
            koshaMsdsService = new KoshaMsdsService();
            hcMsdsService = new HcMsdsService();
          
        }

        private void BtnSearchKosha_Click(object sender, EventArgs e)
        {

           List<KoshaMsds> list =  koshaMsdsService.SearchKoshaMsds(RdoName.Checked, TxtSearchKoshaWord.Text);
           SSKoshaList.SetDataSource(list);

            
        }

        private void MsdsForm_Load(object sender, EventArgs e)
        {
         

            TxtSearchKoshaWord.SetExecuteButton(BtnSearchKosha);
            TxtSearchMsdsWord.SetExecuteButton(BtnSearchMsds);

            
            SSKoshaList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSKoshaList.AddColumnText("CHEMID", nameof(KoshaMsds.ChemId), 60,IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSKoshaList.AddColumnText("물질명", nameof(KoshaMsds.ChemNameKor), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSKoshaList.AddColumnText("CasNo", nameof(KoshaMsds.CasNo), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSMSDSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSMSDSList.AddColumnText("ID", nameof(HC_MSDS.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("CHEMID", nameof(HC_MSDS.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("물질명", nameof(HC_MSDS.NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSMSDSList.AddColumnText("CasNo", nameof(HC_MSDS.CASNO), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnDateTime("수정일시", nameof(HC_MSDS.MODIFIED), 120, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSMSDSList.AddColumnText("수정자", nameof(HC_MSDS.MODIFIEDUSER), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });


            TxtChemId.SetOptions(new TextBoxOption { DataField = nameof(HC_MSDS.CHEMID) });
            TxtName.SetOptions(new TextBoxOption { DataField = nameof(HC_MSDS.NAME) });
            TxtCasNo.SetOptions(new TextBoxOption { DataField = nameof(HC_MSDS.CASNO) });
        }

        private void SSKoshaList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
           KoshaMsds model =  SSKoshaList.GetRowData(e.Row) as KoshaMsds;
           
           HC_MSDS dto = koshaMsdsService.GetKoshaRule(model.ChemId);
           dto.CASNO = model.CasNo;
           dto.CHEMID = model.ChemId;
           dto.NAME = model.ChemNameKor;
           dto.GHS_PICTURE = koshaMsdsService.GetGHSPicture(model.ChemId);

           SetData(dto);
            


        }
        private void SetData(HC_MSDS dto)
        {
            panMsds.SetData(dto);
            
            SetDataCheckOrTextBox(dto.EXPOSURE_MATERIAL, ChkEXPOSURE_MATERIAL, TxtEXPOSURE_MATERIAL);

            SetDataCheckOrTextBox(dto.WEM_MATERIAL, ChkWEM_MATERIAL, TxtWEM_MATERIAL);
      
            SetDataCheckOrTextBox(dto.SPECIALHEALTH_MATERIAL, ChkSPECIALHEALTH_MATERIAL, TxtSPECIALHEALTH_MATERIAL);

            SetDataCheckOrTextBox(dto.MANAGETARGET_MATERIAL, ChkMANAGETARGET_MATERIAL, TxtMANAGETARGET_MATERIAL);

            SetDataCheckOrTextBox(dto.SPECIALMANAGE_MATERIAL, ChkSPECIALMANAGE_MATERIAL, TxtSPECIALMANAGE_MATERIAL);
          
            SetDataCheckOrTextBox(dto.STANDARD_MATERIAL, ChkSTANDARD_MATERIAL, TxtSTANDARD_MATERIAL);

            SetDataCheckOrTextBox(dto.PERMISSION_MATERIAL, ChkPERMISSION_MATERIAL, TxtPERMISSION_MATERIAL);

            SetDataCheckOrTextBox(dto.PSM_MATERIAL, ChkPSM_MATERIAL, TxtPSM_MATERIAL);

        
            GhsPicture.Controls.Clear();
            if (dto.GHS_PICTURE != "자료없음")
            {
                string[] pictures = dto.GHS_PICTURE.Split('|');
                foreach (string image in pictures)
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = hcMsdsService.GetGhsPicture(image);
                    pic.Size = new Size(46, 46);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    GhsPicture.Controls.Add(pic);
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "자료없음";
                GhsPicture.Controls.Add(label);
            }

        }
        private void SetDataCheckOrTextBox(string value, CheckBox checkbox, TextBox textbox)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.Equals("N"))
                {
                    checkbox.Checked = false;
                }
                else
                {
                    checkbox.Checked = true;
                    if (value.Length > 1)
                    {
                        textbox.Text = value;
                    }
                }
                
          
            }
            else
            {
                checkbox.Checked = false;
            }
        }
        private void BtnSearchMsds_Click(object sender, EventArgs e)
        {
            List<HC_MSDS> list = hcMsdsService.Search(RdoMsdsName.Checked, TxtSearchMsdsWord.Text);
            SSMSDSList.SetDataSource(list);
        }

      

        private void SSMSDSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_MSDS dto = SSMSDSList.GetRowData(e.Row) as HC_MSDS;
            SetData(dto);

        }

        private void BtnSaveMsds_Click(object sender, EventArgs e)
        {
            HC_MSDS dto = panMsds.GetData<HC_MSDS>();

            dto.EXPOSURE_MATERIAL = GetCheckedString(ChkEXPOSURE_MATERIAL, TxtEXPOSURE_MATERIAL);
            dto.WEM_MATERIAL = GetCheckedString(ChkWEM_MATERIAL, TxtWEM_MATERIAL);
            dto.SPECIALHEALTH_MATERIAL = GetCheckedString(ChkSPECIALHEALTH_MATERIAL, TxtSPECIALHEALTH_MATERIAL);
            dto.MANAGETARGET_MATERIAL = GetCheckedString(ChkMANAGETARGET_MATERIAL, TxtMANAGETARGET_MATERIAL);
            dto.SPECIALMANAGE_MATERIAL = GetCheckedString(ChkSPECIALMANAGE_MATERIAL, TxtSPECIALMANAGE_MATERIAL);
            dto.STANDARD_MATERIAL = GetCheckedString(ChkSTANDARD_MATERIAL, TxtSTANDARD_MATERIAL);
            dto.PERMISSION_MATERIAL = GetCheckedString(ChkPERMISSION_MATERIAL, TxtSTANDARD_MATERIAL);
            dto.PSM_MATERIAL = GetCheckedString(ChkPSM_MATERIAL, TxtPSM_MATERIAL);

            hcMsdsService.Save(dto);
            MessageUtil.Info(dto.NAME + " 저장하였습니다");
            BtnSearchMsds.PerformClick();
        }
       
        private string GetCheckedString(CheckBox checkbox, TextBox textbox)
        {
            string value = string.Empty;
            if (checkbox.Checked == false)
            {
                value = "N";
            }
            else
            {
                if (textbox.Text.IsNullOrEmpty())
                {
                    value = "Y";
                }
                else
                {
                    value = textbox.Text;
                }
            }
            return value;
        }

        private void BtnDeleteMsds_Click(object sender, EventArgs e)
        {
            HC_MSDS dto = panMsds.GetData<HC_MSDS>();
            if(dto != null)
            {
                if (dto.ID > 0)
                {
                    hcMsdsService.Delete(dto.ID);
                    MessageUtil.Info(dto.NAME + " 삭제하였습니다");
                    panMsds.Initialize();
                    BtnSearchMsds.PerformClick();
                }
            }
            
            
        }

        private void RdoMsdsCasNo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TxtSearchMsdsWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void RdoMsdsName_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    
}
