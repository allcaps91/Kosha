using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcLtdCode.cs
/// Description     : 사업장코드 관리
/// Author          : 김민철
/// Create Date     : 2019-07-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcCode01.frm(FrmLtdCode)" />
namespace ComHpcLibB
{
    public partial class frmHcLtdCode : BaseForm
    {
        HicLtdService hicLtdService = null;
        HicCodeService hcCodeservice = null;
        HicLtdTaxService hicLtdTaxService = null;

        public frmHcLtdCode()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetControl()
        {
            hicLtdService = new HicLtdService();
            hcCodeservice = new HicCodeService();
            hicLtdTaxService = new HicLtdTaxService();

            #region SSList Set
            SSList.Initialize();
            SSList.AddColumn("코드",        nameof(HIC_LTD.CODE),     50, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("사업장명칭",  nameof(HIC_LTD.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("대표자명",    nameof(HIC_LTD.DAEPYO),   48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("ROWID",       nameof(HIC_LTD.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region SSTax Set
            SSTax.Initialize();
            SSTax.AddColumn("D",            "",                           50, FpSpreadCellType.CheckBoxCellType);
            Column column = SSTax.AddColumn("구분", nameof(HIC_LTD_TAX.BUSE), 72, FpSpreadCellType.ComboBoxCellType);
            ComboBoxCellType comboBoxCell = column.CellType as ComboBoxCellType;
            comboBoxCell.Items = new string[] { "종검", "일반", "측정", "대행" };
            comboBoxCell.ItemData = new string[] { "1", "2", "3", "4" };

            SSTax.AddColumn("담당자",       nameof(HIC_LTD_TAX.DAMNAME),  78, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("직책",         nameof(HIC_LTD_TAX.DAMJIK),   84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("전화번호",     nameof(HIC_LTD_TAX.TEL),      88, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("휴대폰",       nameof(HIC_LTD_TAX.HPHONE),   96, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("이메일",       nameof(HIC_LTD_TAX.EMAIL),   180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("참고사항",     nameof(HIC_LTD_TAX.REMARK),  210, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSTax.AddColumn("ROWID",        nameof(HIC_LTD_TAX.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SSTax.AddColumn("회사코드",     nameof(HIC_LTD_TAX.LTDCODE),  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            chkUseGbn1.SetOptions(new CheckBoxOption { DataField = nameof(HIC_LTD.GBGEMJIN),    CheckValue = "Y", UnCheckValue = "" });
            chkUseGbn2.SetOptions(new CheckBoxOption { DataField = nameof(HIC_LTD.GBCHUKJENG),  CheckValue = "Y", UnCheckValue = "" });
            chkUseGbn3.SetOptions(new CheckBoxOption { DataField = nameof(HIC_LTD.GBDAEHANG),   CheckValue = "Y", UnCheckValue = "" });
            chkUseGbn4.SetOptions(new CheckBoxOption { DataField = nameof(HIC_LTD.GBJONGGUM),   CheckValue = "Y", UnCheckValue = "" });
            chkUseGbn5.SetOptions(new CheckBoxOption { DataField = nameof(HIC_LTD.GBGUKGO),     CheckValue = "Y", UnCheckValue = "" });

            rdoSchool1.SetOptions(new RadioButtonOption { DataField = nameof(HIC_LTD.GBSCHOOL), CheckValue = "0" });
            rdoSchool2.SetOptions(new RadioButtonOption { DataField = nameof(HIC_LTD.GBSCHOOL), CheckValue = "1" });
            rdoSchool3.SetOptions(new RadioButtonOption { DataField = nameof(HIC_LTD.GBSCHOOL), CheckValue = "2" });

            List<HIC_CODE> list = null;

            list = hcCodeservice.FindOne("01", "NAME");
            cboUpjong.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hcCodeservice.FindOne("07", "NAME");
            cboNodong.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hcCodeservice.FindOne("21", "NAME");
            cboJisa.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hcCodeservice.FindOne("03", "NAME");
            cboJido.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hcCodeservice.FindOne("26", "NAME");
            cboArmy.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hcCodeservice.FindOne("A6", "NAME");
            cboJepum1.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJepum2.SetItems(list, "NAME", "CODE");
            cboJepum3.SetItems(list, "NAME", "CODE");
            cboJepum4.SetItems(list, "NAME", "CODE");
            cboJepum5.SetItems(list, "NAME", "CODE");

            cboGyumo.Items.Clear();
            cboGyumo.Items.Add("1.5인미만");
            cboGyumo.Items.Add("2.5~49인");
            cboGyumo.Items.Add("3.50~299인");
            cboGyumo.Items.Add("4.300~999인");
            cboGyumo.Items.Add("5.1000인이상");
            cboGyumo.SelectedIndex = 0;

            dtpSelDate.SetOptions( new DateTimePickerOption { DataField = nameof(HIC_LTD.SELDATE),  DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpGyeDate.SetOptions( new DateTimePickerOption { DataField = nameof(HIC_LTD.GYEDATE),  DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpNegoDate.SetOptions(new DateTimePickerOption { DataField = nameof(HIC_LTD.NEGODATE), DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });
            dtpDelDate.SetOptions( new DateTimePickerOption { DataField = nameof(HIC_LTD.DELDATE),  DisplayFormat = DateTimeType.YYYY_MM_DD, DataBaseFormat = DateTimeType.None });

            panMain.SetEnterKey();
            //txtCode.Initialize();
            //txtCode.HideUpDownButton();
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnDelete.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnRowAdd.Click        += new EventHandler(eBtnClick);
            this.txtViewCode.KeyPress   += new KeyPressEventHandler(eKeyPress);
            this.txtCode.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.txtDLtd.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SSList)
            {
                HIC_LTD item = SSList.GetRowData(e.Row) as HIC_LTD;
                if (!item.JUMIN.IsNullOrEmpty())
                {
                    item.JUMIN = clsAES.DeAES(item.JUMIN);
                }
                
                panMain.SetData(item);

                //세금계산서 정보
                SSTax.DataSource = hicLtdTaxService.ViewData(item.CODE.ToString().Trim());
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtViewCode && e.KeyChar == (char)13)
            {
                Search_List(txtViewCode.Text.Trim());
            }
            else if (sender == txtCode && e.KeyChar == (char)13)
            {
                if (txtCode.Text.Trim() == "")
                {
                    return;
                }

                HIC_LTD item = hicLtdService.FindOne(txtCode.Text.Trim());

                if (item == null)
                {
                    Screen_Clear();
                    MessageBox.Show("조회 된 Data가 없음");
                    return;
                }

                if (!item.JUMIN.IsNullOrEmpty())
                {
                    item.JUMIN = clsAES.DeAES(item.JUMIN);
                }

                panMain.SetData(item);
                //세금계산서 정보
                SSTax.DataSource = hicLtdTaxService.ViewData(txtCode.Text.Trim());
            }
            else if (sender == txtDLtd && e.KeyChar == (char)13)
            {
                if (txtDLtd.Text.Trim() == "")
                {
                    return;
                }

                HIC_LTD list = hicLtdService.FindOne(txtDLtd.Text.Trim());
                if (!list.IsNullOrEmpty()) { lblDLtdName.Text = list.NAME; }
            }
        }

        private void Search_List(string strKeyWord)
        {
            if (strKeyWord == "")
            {
                if (ComFunc.MsgBoxQ("찾는 단어가 없을 경우 조회시간이 오래걸립니다. 그래도 조회하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            SSList.DataSource = hicLtdService.ViewLtd(strKeyWord);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Search_List(txtViewCode.Text.Trim());
            }
            else if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnRowAdd)
            {
                SSTax.AddRows(1);
            }
            else if (sender == btnNew)
            {
                Screen_Clear();
                txtCode.Text = ComQuery.GetSequencesNoEx(clsDB.DbCon, "HC_LTD_SEQ").ToString();
                txtCode.Enabled = false;
            }
            else if (sender == btnDelete)
            {
                //HIC_LTD item = panMain.GetData<HIC_LTD>();
                HIC_LTD item = null;

                if (panMain.Tag is PanelOption)
                {
                    PanelOption panelOption = panMain.Tag as PanelOption;

                    if(panelOption.Data != null)
                    {
                        item = panelOption.Data as HIC_LTD;
                    }
                }

                if (item.RID == "" || item.RID == null)
                {
                    return;
                }

                if (ComFunc.MsgBoxQ("사업장 정보를 삭제하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                #region 단순쿼리 예제
                ////일반건진에 자료가 발생되었는지 점검
                //Dictionary<string, object> D1 = hicLtdService.SelHicCount(item.CODE);
                //object nCNT1 = D1["CNT"];

                ////종합검진에 자료가 발생되었는지 점검
                //Dictionary<string, object> D2 = hicLtdService.SelHeaCount(item.CODE);
                //object nCNT2 = D2["CNT"];

                ////미수Data에 자료가 있는지 점검
                //Dictionary<string, object> D3 = hicLtdService.SelMisuCount(item.CODE);
                //object nCNT3 = D3["CNT"];

                //int nTOT = Convert.ToInt16(nCNT1) + Convert.ToInt16(nCNT2) + Convert.ToInt16(nCNT3); 

                ////형변환
                //string strCount = "1234";
                //int count = strCount.To<int>();

                //string strEmpty = string.Empty;
                //int count2 = strEmpty.To<int>(0);
                #endregion

                //GetCount 형식 SingleQuery 예제
                //일반건진에 자료가 발생되었는지 점검
                long nCNT1 = hicLtdService.GetHicCount(item.CODE);
                //종합검진에 자료가 발생되었는지 점검
                long nCNT2 = hicLtdService.GetHeaCount(item.CODE);
                //미수Data에 자료가 있는지 점검
                long nCNT3 = hicLtdService.GetMisuCount(item.CODE);
                
                long nTOT = nCNT1 + nCNT2 + nCNT3;

                if (nTOT > 0)
                {
                    StringBuilder strMst = new StringBuilder();

                    strMst.AppendLine("코드를 삭제하면 삭제한 회사와 관련된 자료를 조회 시");
                    strMst.AppendLine("오류가 발생합니다.");
                    strMst.AppendLine("그래도 삭제 하시겠습니까?");

                    if (ComFunc.MsgBoxQ(strMst.ToString(), "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }

                item.DELDATE = DateTime.Now;

                int result = hicLtdService.Delete(item);
                int result2 = hicLtdService.Delete_Tax_All(item.CODE);

                if (result > 0 && result > 0)
                {
                    MessageBox.Show("삭제 하였습니다.");
                    panMain.Initialize();
                    Screen_Clear();
                    return;
                }
            }
            else if (sender == btnSave)
            {
                txtCode.Enabled = true;
                HIC_LTD item = panMain.GetData<HIC_LTD>();
                IList<HIC_LTD_TAX> item2 = SSTax.GetEditbleData<HIC_LTD_TAX>();
                int result = 0;

                //Data Error Check
                if (!hicLtdService.DataCheck(item))
                {
                    return;
                }

                if (item != null)
                {
                    if (!panMain.RequiredValidate())
                    {
                        MessageBox.Show("필수 입력항목이 누락되었습니다.");
                        return;
                    }

                    if (!item.JUMIN.IsNullOrEmpty())
                    {
                        item.JUMIN = clsAES.AES(item.JUMIN.Trim());
                    }

                    if (item.RID == null || item.RID == "")
                    {
                        result = hicLtdService.Insert(item);
                    }
                    else
                    {
                        result = hicLtdService.Update(item);
                    }

                    if (chkUseGbn3.Checked)
                    {
                        hicLtdService.SaveOsha(item.CODE);
                    }
                    else
                    {
                        hicLtdService.InactiveOsha(item.CODE);
                    }
                       

                    //세금계산서 정보 입력
                    for (int i = 0; i < item2.Count; i++)
                    {
                        //작업자 사번입력
                        item2[i].JOBSABUN = clsType.User.IdNumber.To<long>();

                        if (!item2[i].RID.IsNullOrEmpty())
                        {
                            result = hicLtdTaxService.UpDate(item2[i]);
                        }
                        else
                        {
                            item2[i].LTDCODE = txtCode.Text.To<long>();
                            result = hicLtdTaxService.Insert(item2[i]);
                        }
                    }

                    HIC_LTD_TAX item3 = new HIC_LTD_TAX();

                    for (int i = 0; i < SSTax.ActiveSheet.RowCount; i++)
                    {
                        if (SSTax.ActiveSheet.Cells[i, 0].Text == "True")
                        {                            
                            item3 = SSTax.GetRowData(i) as HIC_LTD_TAX;
                            result = hicLtdTaxService.Delete_Tax_One(item3.RID);
                        }
                    }                 

                    if (result > 0)
                    {
                        MessageBox.Show("저장 하였습니다.");
                        panMain.Initialize();
                        Screen_Clear();
                        return;
                    }

                }
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            panMain.AddRequiredControl(txtCode);
            panMain.AddRequiredControl(txtSangho);
            panMain.AddRequiredControl(txtName);
        }

        private void Screen_Clear()
        {
            panMain.Initialize();
            clsSpread cSpd = new clsSpread();
            cSpd.Spread_All_Clear(SSTax);
            cSpd.Dispose();
            txtCode.Enabled = true;
        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
