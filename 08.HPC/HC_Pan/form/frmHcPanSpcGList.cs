using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanSpcGList.cs
/// Description     : 특수검진 공단 대상자 체크
/// Author          : 이상훈
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSGList.frm(HcPan55)" />

namespace HC_Pan
{
    public partial class frmHcPanSpcGList : Form
    {
        HicResSpecialService hicResSpecialService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicExjongService hicExjongService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcPanSpcGList()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicResSpecialService = new HicResSpecialService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicExjongService = new HicExjongService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-10).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

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
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnDelete)
            {
                long nWrtNo = 0;
                string strOK = "";

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strOK = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    nWrtNo = SS1.ActiveSheet.Cells[i, 4].Text.To<long>();

                    if (strOK == "True" && nWrtNo > 0)
                    {
                        //변경한 자료를 DB에 UPDATE
                        result = hicResSpecialService.UpdatebyWrtNo(nWrtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("DB에 UPDATE시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnSave)
            {
                long nWrtNo = 0;
                string strOK = "OK";

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strOK = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    nWrtNo = SS1.ActiveSheet.Cells[i, 4].Text.To<long>();

                    if (strOK == "True" && nWrtNo > 0)
                    {
                        //변경한 자료를 DB에 UPDATE
                        result = hicResSpecialService.UpdateGbOhmabyWrtNo(nWrtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("DB에 UPDATE시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnSearch)
            {
                int nRow = 0;
                int nREAD = 0;
                int nHEIGHT = 0;
                string strOK = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                long nLtdCode = 0;
                string strJong = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else
                {
                    strJob = "2";
                }

                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

                if (VB.Left(cboJong.Text, 2) != "**")
                {
                    strJong = VB.Left(cboJong.Text, 2);
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 50;

                //신규접수 및 접수수정 자료를 SELECT
                List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItemsbyJepDate(strFrDate, strToDate, strJob, nLtdCode, strJong);

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;                            //성명
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;                           //건진종류
                    SS1.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());    
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].WRTNO.To<string>();                        //접수번호
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].JEPDATE.To<string>();                      //접수일자
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].GBOHMS;                           //대상구분
                    progressBar1.Value = i + 1;
                }
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

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
