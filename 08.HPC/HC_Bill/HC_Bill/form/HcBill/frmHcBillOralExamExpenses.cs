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
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcBillOralExamExpenses.cs
/// Description     : 구강비용청구서 [2020]
/// Author          : 이상훈
/// Create Date     : 2020-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm구강청구서_2020.frm(Frm구강청구서_2020)" />

namespace HC_Bill
{
    public partial class frmHcBillOralExamExpenses : Form
    {
        HicMirDentalService hicMirDentalService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        PrintDocument pd;

        string FstrMirNo;

        public frmHcBillOralExamExpenses()
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        public frmHcBillOralExamExpenses(long strMirNo)
        {
            InitializeComponent();

            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicMirDentalService = new HicMirDentalService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtMirno.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void SetControl()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            txtLtdCode.Text = "";
            txtMirno.Text = "";

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>();
            cboYear.Items.Clear();
            for (int i = 0; i <= 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;
            //cboYear.Text = "2020";

            if (!FstrMirNo.IsNullOrEmpty())
            {
                txtMirno.Text = FstrMirNo;
                eBtnClick(btnSearch, new EventArgs());
            }
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
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "";
                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                if (rdoCnt1.Checked == true)
                {
                    for (int i = 0; i < 2; i++) //인쇄매수
                    {
                        sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                    }
                }
                else if (rdoCnt0.Checked == true)
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
            }
            else if (sender == btnSearch)
            {
                string strYear = "";
                string strkiho = "";
                string strLtdCode = "";
                string strLtdName = "";
                string strSDate = "";
                long nREAD = 0;
                long nJepCnt = 0;
                long nDentAmt = 0;
                long nHuCnt = 0;
                long nHuAmt = 0;

                nDentAmt = 0;
                nJepCnt = 0;

                fn_Spread_Clear();

                strYear = cboYear.Text;
                if (txtMirno.Text.Trim() == "")
                {
                    return;
                }

                chb.READ_HIC_MIR_DENTAL(txtMirno.Text.To<long>());
                if (clsHcType.TMD.Life_Gbn == "Y")
                {
                    lbl_Life.Visible = true;
                }
                else
                {
                    lbl_Life.Visible = false;
                }

                List<HIC_MIR_DENTAL> list = hicMirDentalService.GetItembyMirnoYear(txtMirno.Text, strYear);
                
                if (list.Count > 0)
                {
                    strkiho = list[0].KIHO;
                    //strLtdCode = string.Format("{0:#######}", list[0].LTDCODE);
                    //strLtdName = hb.READ_Ltd_Name(string.Format("{0:#######}", list[0].LTDCODE));
                    strSDate = list[0].FRDATE + " ~ " + list[0].TODATE;
                    nJepCnt = list[0].JEPQTY;
                    nHuCnt = list[0].HUQTY;
                }

                SS1.ActiveSheet.Cells[2, 1].Text = "  출력일자 : " + clsPublic.GstrSysDate;
                SS1.ActiveSheet.Cells[6, 7].Text = "  " + strLtdName;                     //사업장명칭
                SS1.ActiveSheet.Cells[6, 10].Text = "    " + strkiho;                     //사업장명칭

                nDentAmt = SS1.ActiveSheet.Cells[11, 4].Text.Replace(",", "").To<long>();
                SS1.ActiveSheet.Cells[11, 5].Text = nJepCnt.To<string>();
                SS1.ActiveSheet.Cells[11, 6].Text = (nDentAmt * nJepCnt).To<string>();

                nHuAmt = SS1.ActiveSheet.Cells[12, 4].Text.Replace(",", "").To<long>();
                SS1.ActiveSheet.Cells[12, 5].Text = nHuAmt.To<string>();
                SS1.ActiveSheet.Cells[12, 6].Text = (nHuAmt * nHuCnt).To<string>();

                SS1.ActiveSheet.Cells[35, 5].Text = nJepCnt.To<string>();
                SS1.ActiveSheet.Cells[35, 6].Text = string.Format("{0:###,###,##0}", (nDentAmt * nJepCnt) + (nHuAmt * nHuCnt));

                SS1.ActiveSheet.Cells[37, 1].Text = "검진기간 : " + strSDate + "  파일명 : " + list[0].FILENAME + "  [ 청구번호 : " + txtMirno.Text.Trim() + " ]";

                for (int i = 10; i <= 35; i++)
                {
                    SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:###,###,###,##0}", SS1.ActiveSheet.Cells[i, 6].Text);
                    SS1.ActiveSheet.Cells[i, 11].Text = string.Format("{0:###,###,###,##0}", SS1.ActiveSheet.Cells[i, 6].Text);
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
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
            else if (sender == txtMirno)
            {
                if (e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                }
            }
        }

        void fn_Spread_Clear()
        {
            SS1.ActiveSheet.Cells[2, 1].Text = "";  //출력일자
            SS1.ActiveSheet.Cells[6, 7].Text = "";  //사업장 명칭
            SS1.ActiveSheet.Cells[6, 10].Text = ""; //기호                                                    
            SS1.ActiveSheet.Cells[7, 9].Text = "";  //총청구금액

            //검사항목 및 검사료
            for (int i = 10; i <= 35; i++)
            {
                SS1.ActiveSheet.Cells[i, 5].Text = "";  //1차실시인원
                SS1.ActiveSheet.Cells[i, 6].Text = "";  //1차청구금액
                SS1.ActiveSheet.Cells[i, 10].Text = ""; //2차실시인원
                SS1.ActiveSheet.Cells[i, 11].Text = ""; //2차청구금액
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
