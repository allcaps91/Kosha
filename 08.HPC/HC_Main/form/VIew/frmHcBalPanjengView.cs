using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComHpcLibB.form.HcView;
using ComBase.Controls;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcBalPanjengView.cs
/// Description     : 발레오판정결과
/// Author          : 이상훈
/// Create Date     : 2019-09-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPanjengView.frm(FrmPanjengView)" />

namespace HC_Main
{
    public partial class frmHcBalPanjengView : Form
    {
        HeaJepsuPatientService heaJepsuPatientService = null;

        frmHcLtdHelp frmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmHcBalPanjengView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuPatientService = new HeaJepsuPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);
            sp.Spread_All_Clear(SS1);
            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
            SS1_Sheet1.Rows.Get(-1).Height = 20;

            txtLtdCode.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnHelp)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                string strFDate = "";
                string strTDate = "";
                string strPan = "";
                int nChk = 0;
                string nCNT = "";
                long count = 0;
                string strLtdCode = "";
                string strJumin = "";
                int num = 0;

                strFDate = dtpFrDate.Text;
                strTDate = dtpToDate.Text;
                txtLtdCode.Text = txtLtdCode.Text.Trim();

                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).Trim();

                List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembyLtdCode(strFDate, strTDate, strLtdCode);

                if (list.Count > 0)
                {
                    return;
                }

                SS1.ActiveSheet.RowCount = list.Count;

                SS1.DataSource = list;

                for (int i = 0; i < list.Count; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);
                    count = 0;
                    strPan = "";
                    strPan += list[i].PANREMARK;
                    strPan += VB.Right(strPan.Trim(), strPan.Trim().Length - 1);
                    strPan = "→" + strPan;
                    //특정문자갯수 파악
                    nChk = VB.InStr(strPan, "\n");
                    do
                    {
                        count += 1;
                        nChk = VB.InStr(nChk + 1, strPan, "\n");
                    } while (num <= nChk);

                    if (strPan != "")
                    {
                        strPan = VB.TR(strPan, "\n", VB.Space(4));
                        SS1.ActiveSheet.Cells[i, 7].Text = strPan;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 7].Text = "";
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

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            string strTemp = "";

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    strTemp = hb.READ_Ltd_One_Name(txtLtdCode.Text.Trim());
                    if (strTemp == "")
                    {
                        txtLtdCode.Text = hb.READ_Ltd_One_Name(txtLtdCode.Text.Trim());
                    }
                }
            }
        }
    }
}
