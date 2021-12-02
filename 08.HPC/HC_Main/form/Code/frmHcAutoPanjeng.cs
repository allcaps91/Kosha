using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcAutoPanjeng.cs
/// Description     : 가판정 문구 설정 화면
/// Author          : 이상훈
/// Create Date     : 2019-09-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAutoPan.frm(FrmAutoPan)" />

namespace HC_Main
{
    public partial class frmHcAutoPanjeng : Form
    {
        HicExcodeService hicExcodeService = null;
        HeaAutopanService heaAutopanService = null;
        HeaAutoPanMatchExcodeService heaAutoPanMatchExcodeService = null;
        HeaAutopanMatchService heaAutopanMatchService = null;
        HeaAutoPanMatchResultService heaAutoPanMatchResultService = null;

        clsSpread sp = new clsSpread();

        string FstrWrtNo;
        string[] FstrExCode = new string[100];

        public frmHcAutoPanjeng()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicExcodeService = new HicExcodeService();
            heaAutopanService = new HeaAutopanService();
            heaAutoPanMatchExcodeService = new HeaAutoPanMatchExcodeService();
            heaAutopanMatchService = new HeaAutopanMatchService();
            heaAutoPanMatchResultService = new HeaAutoPanMatchResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnFind.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnListChange.Click += new EventHandler(eBtnClick);
            this.txtSearch.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SSResult.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            List<HIC_EXCODE> list = hicExcodeService.GetCodeAll();

            if (list.Count > 0)
            {
                Array.Resize(ref FstrExCode, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    FstrExCode[i] = list[i].CODE.Trim();
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                fn_ScreenClear();
                fn_ViewAutoPan("");
            }
            else if (sender == btnNew)
            {
                fn_ScreenClear();
                txtRanking.Text = "0";
                txtSyntex.Focus();
            }
            else if (sender == btnFind)
            {
                if (txtSearch.Text.Trim() == "")
                {
                    MessageBox.Show("검색하실 단어를 입력 해 주세요!", "검색어 누락", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSearch.Focus();
                    return;
                }
                fn_ViewAutoPan(txtSearch.Text.Trim());
            }
            else if (sender == btnListChange)
            {
                string strWrtNo = "";
                string strGrpNo = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strWrtNo = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strGrpNo = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                    if (SS1.ActiveSheet.Cells[i, 3].Text == "Y" && strGrpNo != "" && strWrtNo != "")
                    {
                        int result = heaAutopanService.UpdateGrpNobyWrtNo(strWrtNo, strGrpNo);

                        if (result < 0)
                        {
                            MessageBox.Show("종검가판정 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else if (sender == btnSave)
            {
                string strWrtNo = "";
                string strRanking = "";
                string strText = "";
                string strMCode = "";
                string strExcode = "";
                string strROWID = "";
                int nREAD = 0;
                int result = 0;

                strWrtNo = txtWrtNo.Text.Trim();
                strRanking = txtRanking.Text.Trim();
                strText = txtSyntex.Text.Trim();

                if (strText == "")
                {
                    MessageBox.Show("내용이 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fn_ReadTextExCode(strText);

                if (strWrtNo == "")
                {
                    strWrtNo = heaAutopanService.GetSeqHeaAutoPan();

                    HEA_AUTOPAN item = new HEA_AUTOPAN();

                    item.WRTNO = long.Parse(strWrtNo);
                    item.RANKING = long.Parse(strRanking);
                    item.WRITESABUN = long.Parse(clsType.User.IdNumber);
                    item.TEXT = strText;

                    result = heaAutopanService.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("종검가판정 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < SSResult.ActiveSheet.RowCount; i++)
                    {
                        strMCode = SSResult.ActiveSheet.Cells[i, 1].Text.Trim();
                        strExcode = SSResult.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (strMCode != "" && strExcode != "")
                        {
                            result = heaAutopanMatchService.Insert(strWrtNo, strMCode, strExcode);

                            if (result < 0)
                            {
                                MessageBox.Show("종검가판정 계산 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    HEA_AUTOPAN item = new HEA_AUTOPAN();

                    item.WRTNO = long.Parse(strWrtNo);
                    item.WRITESABUN = long.Parse(clsType.User.IdNumber);
                    item.TEXT = strText;

                    result = heaAutopanService.Update(item);

                    if (result < 0)
                    {
                        MessageBox.Show("종검가판정 갱신 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < SSResult.ActiveSheet.RowCount; i++)
                    {
                        strMCode = SSResult.ActiveSheet.Cells[i, 1].Text.Trim();
                        strExcode = SSResult.ActiveSheet.Cells[i, 2].Text.Trim();

                        strROWID = "";
                        if (strMCode != "" && strExcode != "")
                        {
                            strROWID = heaAutopanMatchService.GetRowIdbyWrtNo(strWrtNo, strMCode);
                        }

                        if (strROWID != "")
                        {
                            int result1 = heaAutopanMatchService.Update(strExcode, strROWID);

                            if (result1 < 0)
                            {
                                MessageBox.Show("종검가판정 갱신 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            result = heaAutopanMatchService.Insert(strWrtNo, strMCode, strExcode);

                            if (result < 0)
                            {
                                MessageBox.Show("종검가판정 저장 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("저장 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnDelete)
            {
                if (txtWrtNo.Text.Trim() == "")
                {
                    MessageBox.Show("삭제할 문구를 선택해주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("삭제하시면 해당 문구의 조건도 함께 삭제됩니다." + "\r\n\r\n" + "삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //아래 모두 삭제(히스토리 기능 추가)
                Dictionary<string, object> dic = heaAutopanService.Delete(long.Parse(txtWrtNo.Text.Trim()));

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in dic)
                {
                    stringBuilder.AppendLine(item.Key + " : " + item.Value);
                }

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void fn_ReadTextExCode(string strText)
        {
            if (SSResult.ActiveSheet.RowCount > 0)
            {
                return;
            }

            for (int i = 0; i < VB.UBound(FstrExCode); i++)
            {
                if (VB.InStr(strText, FstrExCode[i]) > 0 && FstrExCode[i].Trim() != "")
                {
                    SSResult.ActiveSheet.RowCount += 1;
                    SSResult.ActiveSheet.Cells[i, 1].Text = FstrExCode[i];
                    SSResult.ActiveSheet.Cells[i, 2].Text = FstrExCode[i];
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtSearch && e.KeyChar == (char)13)
            {

            }
            else if (sender == txtJepNo && e.KeyChar == (char)13)
            {
                if (txtJepNo.Text.Trim() == "")
                {
                    return;
                }

                if (SSResult.ActiveSheet.RowCount == 0)
                {
                    return;
                }
                txtTest.Text = fn_ViewResultSyntex(txtWrtNo.Text, txtJepNo.Text);
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column >= 0 && e.Row >= 0)
            {
                if (sender == SSResult)
                {

                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strWrtNo = "";
                string strRanking = "";
                string strText = "";

                if (e.ColumnHeader == true)
                {
                    return;
                }

                if (strWrtNo == "")
                {
                    return;
                }

                txtWrtNo.Text = strWrtNo.Trim();
                txtRanking.Text = strRanking.Trim();
                txtSyntex.Text = strText.Trim();

                fn_ReadMatchExCode(strWrtNo);
            }
        }

        void fn_ReadMatchExCode(string argWrtNo)
        {
            SSResult.ActiveSheet.RowCount = 0;

            SSResult.DataSource = heaAutoPanMatchExcodeService.GetItembyWrtNo(argWrtNo);
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.Column == 1 || e.Column == 4)
                {
                    SS1.ActiveSheet.Cells[e.Row, 3].Text = "Y";
                }
            }
        }

        void fn_ScreenClear()
        {
            txtWrtNo.Text = "";
            txtRanking.Text = "";
            txtSyntex.Text = "";
            SSResult.ActiveSheet.RowCount = 0;
            txtJepNo.Text = "";
            txtTest.Text = "";
        }

        void fn_ViewAutoPan(string argText)
        {
            int nREAD = 0;

            SS1.ActiveSheet.RowCount = 0;
            SS1.ActiveSheet.RowCount = nREAD;
            SS1.DataSource = heaAutopanService.GetItembyText(argText);
        }

        string fn_ViewResultSyntex(string argWrtNo, string argJepNo)
        {
            string rtnVal = "";

            string strSyntex = "";
            string[] strMCode = new string[10];
            string[] strExCode = new string[10];
            string[] strResult = new string[10];

            strSyntex = heaAutopanService.GetTextbyWrtNo(long.Parse(argWrtNo));

            if (strSyntex == "")
            {
                return rtnVal;
            }

            List<HEA_AUTOPAN_MATCH_RESULT> list = heaAutoPanMatchResultService.GetItembyWrtno(argWrtNo, argJepNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strSyntex = strSyntex.Replace(list[i].MCODE.Trim(), list[i].RESULT.Trim());
                }
            }

            if (list.Count < 1)
            {
                return "";
            }

            rtnVal = strSyntex;
            return rtnVal;
        }
    }
}
