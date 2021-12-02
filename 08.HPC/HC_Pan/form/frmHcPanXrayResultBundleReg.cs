using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanXrayResultBundleReg.cs
/// Description     : 방사선 검사 일괄 등록작업
/// Author          : 이상훈
/// Create Date     : 2019-12-24
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmXResult.frm(HcPan98)" />

namespace HC_Pan
{
    public partial class frmHcPanXrayResultBundleReg : Form
    {
        HicResultService hicResultService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HicExjongService hicExjongService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrPartExam;

        public frmHcPanXrayResultBundleReg()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            hicJepsuResultService = new HicJepsuResultService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnUpdate.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtSName.Text = "";

            txtSName.Text = "";
            txtEnd.Text = "";
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME);
                }
            }

            //hb.ComboJong_Set(cboJong);

            txtLtdCode.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.To<string>() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnUpdate)
            {
                string strOK = "";
                long nWrtNo = 0;
                string sMsg = "";
                string strJong = "";

                if (rdoAll1.Checked == true)
                {
                    sMsg = "선택한것만 저장합니다. 정말로 하시겠습니까?";
                }
                else if (rdoAll2.Checked == true)
                {
                    sMsg = "선택하지 않은것만 저장 합니다. 정말로 하시겠습니까?";
                }
                else if (rdoAll3.Checked == true)
                {
                    sMsg = "조회한 전체를 저장합니다. 정말로 하시겠습니까?";
                }

                if (MessageBox.Show(sMsg, "확인", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (rdoJong1.Checked == true)
                {
                    strJong = "1";
                }
                else if (rdoJong1.Checked == true)
                {
                    strJong = "2";
                }
                else if (rdoJong1.Checked == true)
                {
                    strJong = "3";
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    strOK = SSList.ActiveSheet.Cells[i, 0].Text;
                    nWrtNo = long.Parse(SSList.ActiveSheet.Cells[i, 5].Text);
                    if (rdoAll1.Checked == true && strOK == "True")
                    {
                        fn_One_Update(nWrtNo, strJong);
                        fn_One_Update_Result(nWrtNo);
                        //접수마스타의 상태를 변경
                        hm.Result_EntryEnd_Check(nWrtNo);
                    }
                    else if (rdoAll2.Checked == true && strOK.IsNullOrEmpty())
                    {
                        fn_One_Update(nWrtNo, strJong);
                        fn_One_Update_Result(nWrtNo);
                        //접수마스타의 상태를 변경
                        hm.Result_EntryEnd_Check(nWrtNo);
                    }
                    else if (rdoAll3.Checked == true)
                    {
                        fn_One_Update(nWrtNo, strJong);
                        fn_One_Update_Result(nWrtNo);
                        //접수마스타의 상태를 변경
                        hm.Result_EntryEnd_Check(nWrtNo);
                    }
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strOK = "";
                string strJong = "";
                List<string> strTemp = new List<string>();
                long nLen1 = 0;
                long nLen2 = 0;
                long nS = 0;
                long nE = 0;

                string strFrDate = "";
                string strToDate = "";
                string strSName = "";
                string strGubun = "";
                string strSort = "";
                long nLtdCode = 0;
                string strGjJong = "";
                string strGbStart = "";

                string strResult = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                txtSName.Text = txtSName.Text.Trim();
                strSName = txtSName.Text;
                if (rdoGubun11.Checked == true)
                {
                    strGubun = "1";
                }
                else if (rdoGubun12.Checked == true)
                {
                    strGubun = "2";
                }

                if (chkSort.Checked == true)
                {
                    strSort = "1";
                }

                if (rdoJong1.Checked == true)
                {
                    strJong = "간촬";
                }
                else if (rdoJong2.Checked == true)
                {
                    strJong = "직촬";
                }
                else if (rdoJong3.Checked == true)
                {
                    strJong = "분진+";
                }

                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                if (VB.Left(cboJong.Text, 2) != "**")
                {
                    strGjJong = VB.Left(cboJong.Text, 2);
                }

                if (!txtStart.Text.IsNullOrEmpty())
                {
                    nS = txtStart.Text.To<long>();
                    nE = txtEnd.Text.To<long>();
                    for (long j = nS; j <= nE; j++)
                    {
                        strTemp.Add(j.To<string>());
                    }
                }

                strGbStart = txtStart.Text.Trim();

                fn_Screen_Clear();

                nLen1 = 0;
                nLen2 = 0;

                sp.Spread_All_Clear(SSList);
                SSList.ActiveSheet.RowCount = 50;

                
                if (!txtStart.Text.IsNullOrEmpty())
                {
                    if (txtEnd.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("끝번호 누락", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (!txtStart.Text.IsNullOrEmpty())
                {
                    nLen1 = txtStart.Text.Length;
                    nLen2 = txtEnd.Text.Length;
                }

                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItembyJepDateJong(strFrDate, strToDate, strSName, strGubun, nLtdCode, strJong, strGjJong, strTemp, strSort, strGbStart);

                nREAD = list.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    strResult = hicResultService.GetResultbyExCode(list[i].WRTNO, strJong);
                    strOK = "OK";

                    if (nLen1 > 0)
                    {
                        if (nLen1 > strResult.Length)
                        {
                            strOK = "NO";
                        }
                    }
                    if (nLen2 > 0)
                    {
                        if (nLen2 < strResult.Length)
                        {
                            strOK = "NO";
                        }
                    }

                    if (rdoJong2.Checked == true)   //직촬
                    {
                        if (!list[i].UCODES.IsNullOrEmpty())
                        {
                            for (int j = 0; j < VB.I(list[i].UCODES, ","); j++)
                            {
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H01")
                                {
                                    strOK = "NO";
                                }
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H08")
                                {
                                    strOK = "NO";
                                }
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H11")
                                {
                                    strOK = "NO";
                                }
                            }
                        }
                    }
                    else if (rdoJong3.Checked == true)  //분진+
                    {
                        if (!list[i].UCODES.IsNullOrEmpty())
                        {
                            for (int j = 0; j < VB.I(list[i].UCODES, ","); j++)
                            {
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H01")
                                {
                                    strOK = "OK";
                                }
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H08")
                                {
                                    strOK = "OK";
                                }
                                if (VB.Pstr(list[i].UCODES, ",", j + 1) == "H11")
                                {
                                    strOK = "OK";
                                }
                            }
                        }
                        else
                        {
                            strOK = "NO";
                        }
                    }
                    if (!strResult.IsNullOrEmpty())
                    {
                        strOK = "NO";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE;
                        SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                        SSList.ActiveSheet.Cells[i, 4].Text = list[i].GJJONG;
                        SSList.ActiveSheet.Cells[i, 5].Text = list[i].WRTNO.To<string>();
                        SSList.ActiveSheet.Cells[i, 6].Text = strJong;
                        SSList.ActiveSheet.Cells[i, 7].Text = list[i].RESULT;
                    }
                }
            }
        }

        void fn_One_Update(long argWrtNo, string argJong)
        {
            int result = 0;

            result = hicResultService.Update_ResultbyWrtNoExCode(argWrtNo, argJong);

            if (result < 0)
            {
                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        void fn_One_Update_Result(long argWrtNo)
        {
            int result = 0;

            result = hicResultService.Update_ResultbyWrtNoExCode(argWrtNo, "");

            if (result < 0)
            {
                MessageBox.Show("자료를 등록중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        void fn_Screen_Clear()
        {

        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            long nWrtNo = 0;

            clsPublic.GstrHelpCode = SSList.ActiveSheet.Cells[e.Row, 5].Text;
            nWrtNo = long.Parse(SSList.ActiveSheet.Cells[e.Row, 5].Text);

            FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(nWrtNo, "", "");
            FrmHcPanExamResultRegChg.ShowDialog(this);
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }
    }
}
