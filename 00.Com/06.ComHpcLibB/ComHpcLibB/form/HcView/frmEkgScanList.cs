using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Add.cs
/// Description     : EKG명단 조회
/// Author          : 김경동
/// Create Date     : 2021-06-17
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Hcbill106.frm(FrmAddPrint)" />
/// 

namespace ComHpcLibB
{
    public partial class frmEkgScanList : Form
    {

        #region Declare Variable Area
        HIC_LTD LtdHelpItem = null;
        #endregion

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();

        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        HicJepsuResultService hicJepsuResultService = null;



        public frmEkgScanList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

        }
        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();

            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicJepsuResultService = new HicJepsuResultService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            //dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            txtSname.Text = "";
            btnPrint.Enabled = false;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
                btnPrint.Enabled = true;
            }
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
            }
            #endregion
            else if (sender == btnPrint)
            {
                Spread_Print();
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {
            int nRow = 0;
            long nCount = 0;
            long nLtdCode = 0;
            string strGbChul = "";
            string[] strExCode = { "A151","A153" };

            SSList.ActiveSheet.RowCount = 0;

            if (txtLtdCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
            }

            if (rdoGubun2.Checked) { strGbChul = "1"; }
            if (rdoGubun3.Checked) { strGbChul = "2"; }

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepdate1(dtpFDate.Text, dtpTDate.Text, txtSname.Text, strGbChul, nLtdCode);
            if (!list.IsNullOrEmpty())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    nCount = 0;
                    nCount = hicResultService.GetCntbyWrtNoExCode(list[i].WRTNO, strExCode);
                    
                    if (nCount > 0)
                    {
                        nRow = nRow + 1;
                        SSList.ActiveSheet.RowCount = nRow;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].JEPDATE;
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].PTNO;
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].AGE.ToString() + "/" + list[i].SEX ;
                        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = " ";
                    }
                }
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdCode.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdCode.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdCode.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtLtdCode.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                if (VB.Pstr(txtLtdCode.Text, ",", 1).Trim() == "")
                {
                    txtLtdCode.Text = "";
                }
            }
        }

        private void Spread_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "건진 EKG EMR스캔 명단";
            strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 18, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true) + "\r\n";
            strHeader += sp.setSpdPrint_String("인쇄시각: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("맑은 고딕", 10), clsSpread.enmSpdHAlign.Left, false, true) + "\r\n"; ;
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
