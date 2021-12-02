using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcLtdMCode.cs
/// Description     : 사업장별 취급물질 관리
/// Author          : 김민철
/// Create Date     : 2020-02-27
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm사업장별취급물질관리(Frm사업장별취급물질관리.frm)" />

namespace ComHpcLibB
{
    public partial class frmHcLtdMCode : Form
    {
        clsSpread cSpd = null;
        HicMcodeService hicMcodeService = null;
        HicGroupcodeService hicGroupcodeService = null;
        HicLtdService hicLtdService = null;
        HicLtdUcodeService hicLtdUcodeService = null;
        HicCodeService hicCodeService = null;
        HicJepsuService hicJepsuService = null;

        string FstrROWID = string.Empty;
        string FstrJong = string.Empty;
        long FnLtdCode = 0;

        public frmHcLtdMCode()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);

            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSearch2.Click       += new EventHandler(eBtnClick);
            this.btnSearch3.Click       += new EventHandler(eBtnClick);
            this.btnBuild.Click         += new EventHandler(eBtnClick);

            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnDelete.Click        += new EventHandler(eBtnClick);

            this.btnAdd1.Click          += new EventHandler(eBtnClick);
            this.btnAdd2.Click          += new EventHandler(eBtnClick);
            this.btnDel1.Click          += new EventHandler(eBtnClick);
            this.btnDel2.Click          += new EventHandler(eBtnClick);
            
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader) { return; }

            if (sender == SSList)
            {
                FstrJong = VB.Left(cboViewJong.Text, 1).Trim();
                FnLtdCode = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                if (FnLtdCode <= 0) { return; }

                SS1.DataSource = hicLtdUcodeService.GetListByCodeJong(FnLtdCode, FstrJong);
                txtRemark.Text = hicLtdService.GetSpcRemarkByCode(FnLtdCode);
            }
            else if (sender == SS1)
            {
                FstrROWID = SS1.ActiveSheet.Cells[e.Row, 6].Text;
                DisPlay_LtdInfo(FstrROWID);
            }
        }

        private void DisPlay_LtdInfo(string fstrROWID)
        {
            int nCNT = 0;
            int nRow = 0;
            string strUCodes = string.Empty;
            string strSCodes = string.Empty;
            string strCode = string.Empty;

            HIC_LTD_UCODE item = hicLtdUcodeService.GetItemByRowid(FstrROWID);

            if (!item.IsNullOrEmpty())
            {
                panJob1.SetData(item);

                strUCodes = item.UCODES.IsNullThen<string>("");
                strSCodes = item.SCODES.IsNullThen<string>("");

                nCNT = VB.L(strUCodes, ",").To<int>();
                nRow = 0;

                SS2.ActiveSheet.RowCount = 20;
                for (int i = 0; i < nCNT; i++)
                {
                    strCode = VB.Pstr(strUCodes, ",", i + 1);
                    if (strCode != "")
                    {
                        nRow = nRow + 1;
                        if (nRow > SS2.ActiveSheet.RowCount)
                        {
                            SS2.ActiveSheet.RowCount = nRow;
                            SS2.ActiveSheet.Cells[nRow - 1, 0].Value = "False";
                            SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strCode;
                            SS2.ActiveSheet.Cells[nRow - 1, 2].Text = hicMcodeService.GetNameByCode(strCode);
                        }
                    }
                }

                nCNT = VB.L(strSCodes, ",").To<int>();
                nRow = 0;

                SS4.ActiveSheet.RowCount = 20;
                for (int i = 0; i < nCNT; i++)
                {
                    strCode = VB.Pstr(strSCodes, ",", i + 1);
                    if (strCode != "")
                    {
                        nRow = nRow + 1;
                        if (nRow > SS4.ActiveSheet.RowCount)
                        {
                            SS4.ActiveSheet.RowCount = nRow;
                            SS4.ActiveSheet.Cells[nRow - 1, 0].Value = "False";
                            SS4.ActiveSheet.Cells[nRow - 1, 1].Text = strCode;
                            SS4.ActiveSheet.Cells[nRow - 1, 2].Text = hicGroupcodeService.GetNameByCode(strCode);
                        }
                    }
                }
            }
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            hicMcodeService     = new HicMcodeService();
            hicGroupcodeService = new HicGroupcodeService();
            hicLtdService       = new HicLtdService();
            hicLtdUcodeService  = new HicLtdUcodeService();
            hicCodeService      = new HicCodeService();
            hicJepsuService     = new HicJepsuService();

            #region Spread Set
            SSList.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SSList.AddColumn("코드",        nameof(HIC_LTD.CODE),     50, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("사업장명칭",  nameof(HIC_LTD.NAME),    160, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("대표자명",    nameof(HIC_LTD.DAEPYO),   42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SSList.AddColumn("ROWID",       nameof(HIC_LTD.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            
            SS1.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SS1.AddColumn("담당업무",               nameof(HIC_LTD_UCODE.JOBNAME),  86, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false,  IsSort = true });
            SS1.AddColumn("취급물질 및 선택검사",   nameof(HIC_LTD_UCODE.VUCODE),  280, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false,  IsSort = true });
            SS1.AddColumn("종류",                   nameof(HIC_LTD_UCODE.JONG),     48, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false, IsSort = true });            
            SS1.AddColumn("직종",                   nameof(HIC_LTD_UCODE.JONG_NM),  64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true });            
            SS1.AddColumn("직종명",                 nameof(HIC_LTD_UCODE.JIKJONG),  64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("공정",                   nameof(HIC_LTD_UCODE.GONGJENG), 68, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("ROWID",                  nameof(HIC_LTD_UCODE.RID),      42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            SS2.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SS2.AddColumnCheckBox("선택", nameof(HIC_MCODE.IsDelete), 45, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false });
            SS2.AddColumn("코드",        nameof(HIC_MCODE.CODE),     64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS2.AddColumn("취급물질명",  nameof(HIC_MCODE.NAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            
            SS3.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SS3.AddColumnCheckBox("선택", nameof(HIC_MCODE.IsDelete), 45, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false });
            SS3.AddColumn("코드",        nameof(HIC_MCODE.CODE),     64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS3.AddColumn("취급물질명",  nameof(HIC_MCODE.NAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            
            SS4.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SS4.AddColumnCheckBox("선택", nameof(HIC_GROUPCODE.IsDelete), 45, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false });
            SS4.AddColumn("코드",        nameof(HIC_GROUPCODE.CODE),     64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS4.AddColumn("선택검사명",  nameof(HIC_GROUPCODE.NAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            
            SS5.Initialize(new SpreadOption() { ColumnHeaderHeight = 22, IsRowSelectColor = false });
            SS5.AddColumnCheckBox("선택", nameof(HIC_GROUPCODE.IsDelete), 45, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false });
            SS5.AddColumn("코드",        nameof(HIC_GROUPCODE.CODE),     64, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            SS5.AddColumn("선택검사명",  nameof(HIC_GROUPCODE.NAME),    180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsSort = true });
            #endregion

            cboViewJong.Items.Clear();
            cboViewJong.Items.Add("*.전체");
            cboViewJong.Items.Add("1.배치전");
            cboViewJong.Items.Add("2.특수");
            cboViewJong.Items.Add("3.일특");
            cboViewJong.Items.Add("9.기타");

            cboJong.Items.Clear();
            cboJong.Items.Add("배치전");
            cboJong.Items.Add("특수");
            cboJong.Items.Add("일특");
            cboJong.Items.Add("기타");

            List<HIC_CODE> list = null;

            list = hicCodeService.FindOne("05", "NAME");
            cboJikjong.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            list = hicCodeService.FindOne("A2", "NAME");
            cboGongjeng.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Data_Search(); 
            }
            else if (sender == btnBuild)
            {
                Data_Build();
            }
            else if (sender == btnSearch2)
            {
                Data_Search_MCode();
            }
            else if (sender == btnSearch3)
            {
                Data_Search_GroupCode();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnAdd1)
            {
                Spread_Add(SS3, SS2);
            }
            else if (sender == btnAdd2)
            {
                Spread_Add(SS5, SS4);
            }
            else if (sender == btnDel1)
            {
                Spread_Remove(SS2);
            }
            else if (sender == btnDel2)
            {
                Spread_Remove(SS4);
            }
            else if (sender == btnDelete)
            {
                Delete_Data();
            }
            else if (sender == btnSave)
            {
                Save_Data();
            }
        }

        private void Save_Data()
        {
            int result = 0;
            string strJobName   = string.Empty;
            string strJikJong   = string.Empty;
            string strGongjeng  = string.Empty;
            string strJong      = string.Empty;
            string strRemark    = string.Empty;
            string strUCodes    = string.Empty;
            string strSCodes    = string.Empty;

            strJobName = txtJobName.Text.Trim();
            strJikJong = VB.Pstr(cboJikjong.Text, ".", 1);
            strGongjeng = VB.Pstr(cboGongjeng.Text, ".", 1);

            switch (cboJong.Text.Trim())
            { 
                case "배치전": strJong = "1"; break;
                case "특수":   strJong = "2"; break;
                case "일특":   strJong = "3"; break;
                case "기타":   strJong = "9"; break;
                default:       strJong = "" ; break;
            }

            if (strJobName == "") { MessageBox.Show("담당업무가 공란입니다.", "오류"); return; }
            if (strJobName.Length > 30) { MessageBox.Show("담당업무가 50자 초과입니다.", "오류"); return; }
            if (strJong == "") { MessageBox.Show("건진종류가 오류입니다.", "오류"); return; }

            //채용,배치전,특수검진 전달사항
            //strRemark = VB.Quot(txtRemark.Text.Trim());
            if (strRemark.Length > 2000)
            {
                MessageBox.Show("전달사항이 2000자를 초과함", "오류");
                return;
            }

            strUCodes = "";
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (SS2.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (SS2.ActiveSheet.Cells[i, 1].Text != "")
                    {
                        strUCodes = strUCodes + SS2.ActiveSheet.Cells[i, 1].Text + ",";
                    }
                }
            }

            strSCodes = "";
            for (int i = 0; i < SS4.ActiveSheet.RowCount; i++)
            {
                if (SS4.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (SS4.ActiveSheet.Cells[i, 1].Text != "")
                    {
                        strSCodes = strSCodes + SS4.ActiveSheet.Cells[i, 1].Text + ",";
                    }
                }
            }

            HIC_LTD_UCODE item = new HIC_LTD_UCODE();
            item.LTDCODE = FnLtdCode;
            item.JONG = strJong;
            item.JOBNAME = strJobName;
            item.UCODES = strUCodes;
            item.SCODES = strSCodes;
            item.JOBNAME = clsType.User.IdNumber;
            item.JIKJONG = strJikJong;
            item.GONGJENG = strGongjeng;

            if (FstrROWID != "" && chkNew.Checked == false)
            {
                result = hicLtdUcodeService.UpDateDateByItem(item);
            }
            else
            {
                HIC_LTD_UCODE item2 = hicLtdUcodeService.GetItemByLtdCode(FnLtdCode, strJobName);

                if (item2.IsNullOrEmpty())
                {
                    result = hicLtdUcodeService.InsertData(item);
                }
                else
                {
                    result = hicLtdUcodeService.UpDateDateByItem(item);
                }
            }

            if (result <= 0)
            {
                MessageBox.Show("취급물질 저장 실패", "확인");
                return;
            }

            //채용,배치전,특수검진 전달사항
            result = hicLtdService.UpdateSpcRemarkByLtdCode(strRemark, FnLtdCode);
            if (result <= 0)
            {
                MessageBox.Show("취급물질 저장 실패", "확인");
                return;
            }

            SS1.DataSource = hicLtdUcodeService.GetListByCodeJong(FnLtdCode, FstrJong);
            txtRemark.Text = hicLtdService.GetSpcRemarkByCode(FnLtdCode);

            Screen_Clear();
        }

        private void Delete_Data()
        {
            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            int result = hicLtdUcodeService.DeleteByRowid(FstrROWID);
            if (result <= 0)
            {
                MessageBox.Show("삭제오류", "확인");
                return;
            }

            SS1.DataSource = hicLtdUcodeService.GetListByCodeJong(FnLtdCode, FstrJong);
            txtRemark.Text = hicLtdService.GetSpcRemarkByCode(FnLtdCode);

            Screen_Clear();

        }

        private void Spread_Remove(FpSpread SS)
        {
            for (int i = SS.ActiveSheet.RowCount; i <= 1; i--)
            {
                if (SS.ActiveSheet.Cells[i - 1, 0].Text == "True")
                {
                    SS.ActiveSheet.RemoveRows(i - 1, 1);
                }
            }
        }

        private void Spread_Add(FpSpread SS_R, FpSpread SS_L)
        {
            string strCode = string.Empty;
            string strName = string.Empty;
            bool bOK = false;

            for (int i = 1; i <= SS_R.ActiveSheet.RowCount; i++)
            {
                if (SS_R.ActiveSheet.Cells[i - 1, 0].Text == "True")
                {
                    strCode = SS_R.ActiveSheet.Cells[i - 1, 1].Text.Trim();
                    strName = SS_R.ActiveSheet.Cells[i - 1, 2].Text.Trim();
                    //자료가 있으면 추가를 안함
                    bOK = true;
                    for (int j = 1; j <= SS_L.ActiveSheet.RowCount; j++)
                    {
                        if (SS_L.ActiveSheet.Cells[j - 1, 1].Text.Trim() == strCode)
                        {
                            bOK = false; break;
                        }
                    }

                    if (bOK)
                    {
                        int nRow = SS_L.ActiveSheet.RowCount + 1;
                        SS_L.ActiveSheet.RowCount = nRow;
                        SS_L.ActiveSheet.Cells[nRow - 1, 1].Text = strCode;
                        SS_L.ActiveSheet.Cells[nRow - 1, 2].Text = strName;
                    }

                    SS_R.ActiveSheet.Cells[i - 1, 0].Text = "False";
                }
            }
        }

        private void Data_Build()
        {
            HIC_LTD_UCODE item = hicLtdUcodeService.GetItemByLtdCode(FnLtdCode);

            if (!item.IsNullOrEmpty())
            {
                MessageBox.Show("이미 회사의 자료가 있어 작업이 불가능함.", "오류");
                return;
            }

            string strUCodes = string.Empty;
            List<string> strTemp1 = null;
            string strTemp2 = string.Empty;
            string strTemp3 = string.Empty;
            string strJong = string.Empty;
            string strBuse = string.Empty;
            string strOK = string.Empty;
            int nBuseNo = 0;

            //회사별 2014.2.1일부터 접수내역을 읽음
            List<HIC_JEPSU> list = hicJepsuService.GetGroupByListByLtdCode(FnLtdCode);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strUCodes = list[i].UCODES.Trim();
                    strTemp3 = list[i].BUSENAME.Trim();
                    if (strTemp3 == "") { strTemp3 = "공통"; }
                    switch (list[i].GJJONG)
                    {
                        case "11":
                        case "12":
                        case "14": strJong = "3"; strBuse = "(일특)" + strTemp3; break;
                        case "21": 
                        case "22": strJong = "1"; strBuse = "(배치)" + strTemp3; break;
                        case "23": strJong = "2"; strBuse = "(특수)" + strTemp3; break;
                        case "24": strJong = "1"; strBuse = "(배치)" + strTemp3; break;
                        default: strJong = ""; break;
                    }

                    strOK = "";
                    if (strJong != "")
                    {
                        //취급물질을 취급물질명 순으로 SORT
                        strTemp1.Clear();
                        for (int j = 1; j <= VB.L(strUCodes, ","); j++)
                        {
                            if (VB.Pstr(strUCodes, ",", j) != "")
                            {
                                strTemp1.Add(VB.Pstr(strUCodes, ",", j));
                            }
                        }

                        List<HIC_MCODE> list1 = hicMcodeService.GetCodeListByCodeIn(strTemp1);
                        if (list1.Count > 0)
                        {
                            for (int j = 0; j < list1.Count; j++)
                            {
                                strUCodes = strUCodes + list1[j].CODE.Trim() + ",";
                            }
                        }

                        //취급물질이 동일한것이 있으면 등록 안함
                        strOK = "OK";
                        HIC_LTD_UCODE item2 = hicLtdUcodeService.GetRowidByLtdCodeLikeName(FnLtdCode, strBuse, strUCodes);
                        if (!item2.IsNullOrEmpty())
                        {
                            strOK = "";
                        }

                        if (strOK == "OK")
                        {
                            HIC_LTD_UCODE item3 = hicLtdUcodeService.GetMaxNameByLtdCodeLikeName(FnLtdCode, strBuse);

                            if (!item3.IsNullOrEmpty())
                            {
                                strTemp1.Add(item3.JOBNAME.Trim());
                            }

                            if (strTemp1 == null)
                            {
                                nBuseNo = 0;
                            }
                            else
                            {
                                if (VB.InStr(strTemp1.ToString(), "_") == 0)
                                {
                                    nBuseNo = 1;
                                }
                                else
                                {
                                    nBuseNo = VB.Pstr(strTemp1.ToString(), "_", 2).To<int>() + 1;
                                }
                                strBuse = strBuse + "_" + VB.Format(nBuseNo, "000");
                            }

                            HIC_LTD_UCODE item4 = new HIC_LTD_UCODE();
                            item4.LTDCODE = FnLtdCode;
                            item4.JONG = strJong;
                            item4.JOBNAME = strBuse;
                            item4.UCODES = strUCodes;
                            item4.JOBNAME = clsType.User.IdNumber;
                            item4.JIKJONG = "";
                            item4.GONGJENG = "";

                            int result = hicLtdUcodeService.InsertData(item4);
                            if (result <= 0)
                            {
                                MessageBox.Show("작업실패", "확인");
                            }
                        }
                    }

                }
            }

            SS1.DataSource = hicLtdUcodeService.GetListByCodeJong(FnLtdCode, FstrJong);
            txtRemark.Text = hicLtdService.GetSpcRemarkByCode(FnLtdCode);

            MessageBox.Show("작업완료", "확인");
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            Data_Search_MCode();
            Data_Search_GroupCode();
        }

        private void Data_Search()
        {
            string strLtdCode = txtLtdCode.Text.Trim();

            if (strLtdCode.IsNullOrEmpty()) { return; }

            SSList.DataSource = hicLtdService.ViewLtd(strLtdCode);
        }

        private void Data_Search_MCode()
        {
            string strName = txtView1.Text.Trim();
            SS3.DataSource = hicMcodeService.GetItemByLikeName(strName);
        }

        private void Data_Search_GroupCode()
        {
            string strName = txtView2.Text.Trim();
            SS5.DataSource = hicGroupcodeService.GetItemByLikeName(strName);
        }

        private void Screen_Clear()
        {
            txtJobName.Text = "";
            cboJikjong.SelectedIndex = 0;
            txtRemark.Text = "";
            cboJong.SelectedIndex = 0;
            cboGongjeng.SelectedIndex = 0;
            chkNew.Checked = false;
            chkNew.Visible = false;
            FstrROWID = "";

            cSpd.Spread_Clear_Simple(SS2);
            cSpd.Spread_Clear_Simple(SS4);
        }
    }
}
