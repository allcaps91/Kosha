using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcGaJepsuVIew_Print : Form
    {
        ComFunc CF = null;
        clsHaBase cHB = null;
        clsHcMain cHM = null;
        HIC_LTD LtdHelpItem = null;


        HicJepsuWorkService hicJepsuWorkService = null;



        public frmHcGaJepsuVIew_Print()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {

            hicJepsuWorkService = new HicJepsuWorkService();


            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cHM = new clsHcMain();
            LtdHelpItem = new HIC_LTD();

            int nYY = DateTime.Now.ToShortDateString().Substring(0, 4).To<int>();

            cboYYMM.Items.Clear();
            for (int i = 1; i < 4; i++)
            {
                cboYYMM.Items.Add(VB.Format(nYY, "0000"));
                nYY += 1;
            }

            cboYYMM.SelectedIndex = 0;

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong_AddItem(cboJong, true);
            cboJong.SelectedIndex = 0;

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), -10);
            dtpTDate.Text = DateTime.Now.ToShortDateString();
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                Screen_Display();
            }

        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void Screen_Display()
        {
            string strPano = "";
            string strSudate = "";
            string strGjjong = "";

            string strUcodes = "";
            string strJumin = string.Empty;
            string strYear = cboYYMM.Text;
            string strFDate = dtpFDate.Text;
            string strTDate = CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1);
            string strGbChul = rdoGbn1.Checked == true ? "Y" : rdoGbn2.Checked == true ? "N" : "";
            string strGjJong = VB.Pstr(cboJong.Text, ".", 1);
            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();


            IList<HIC_JEPSU_WORK> list = hicJepsuWorkService.GetListByItem(strYear, strFDate, strTDate, strGbChul, strGjJong, nLtdCode);

            SS1.ActiveSheet.RowCount = 0;

            if (list.Count > 0)
            {
                SS1.ActiveSheet.RowCount = list.Count;

                for (int i = 0;  i < list.Count; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].JUMINNO;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].LTDNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].BUSENAME;
                    SS1.ActiveSheet.Cells[i, 4].Text = cHM.SExam_Names_Display(list[i].SEXAMS);
                    SS1.ActiveSheet.Cells[i, 5].Text = cHM.UCode_Names_Display(list[i].UCODES);
                    SS1.ActiveSheet.Cells[i, 6].Text = cHB.READ_GjJong_Name(list[i].GJJONG);
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].HPHONE;
                }

                
            }
            else
            {
                MessageBox.Show("접수된 자료가 없습니다.", "확인");
                return;
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }


    }
}
