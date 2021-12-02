using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmLtdHelp.cs
/// Description     : 거래처코드 찾기
/// Author          : 심명섭
/// Create Date     : 2021-06-21
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > frmLtdHelp (HcLtd00.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmLtdHelp :BaseForm
    {
        clsSpread cSpd                                  = null;
        ComFunc CF                                      = null;
        clsHaBase cHB                                   = null;
        HicLtdService hicLtdService                     = null;

        public frmLtdHelp()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd            =       new clsSpread();
            CF              =       new ComFunc();
            cHB             =       new clsHaBase();
            hicLtdService   =       new HicLtdService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                DisPlay_Screen();
            }
            else if (sender == btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Spread_Print();
            }
        }

        private void DisPlay_Screen()
        {
            if(TxtViewCode.Text.Trim() == "")
            {
                MessageBox.Show("찾으실 자료가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<HIC_LTD> item = hicLtdService.GetItemData(rdoSort1.Checked, rdoSort3.Checked, TxtViewCode.Text);
            SS1.ActiveSheet.RowCount = item.Count;

            if (!item.IsNullOrEmpty())
            {
                for (int i = 0; i < item.Count; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = VB.Format(item[i].CODE, "00000");
                    SS1.ActiveSheet.Cells[i, 1].Text = item[i].NAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = item[i].SAUPNO;
                    SS1.ActiveSheet.Cells[i, 3].Text = item[i].DAEPYO;
                    SS1.ActiveSheet.Cells[i, 4].Text = cHB.Jisa2_Name(item[i].JISA);
                    SS1.ActiveSheet.Cells[i, 5].Text = item[i].KIHO;
                    SS1.ActiveSheet.Cells[i, 6].Text = item[i].UPTAE;
                    SS1.ActiveSheet.Cells[i, 7].Text = item[i].JONGMOK;
                    SS1.ActiveSheet.Cells[i, 8].Text = item[i].TEL;
                    SS1.ActiveSheet.Cells[i, 9].Text = item[i].FAX;
                    SS1.ActiveSheet.Cells[i, 10].Text = item[i].BONAME;
                    SS1.ActiveSheet.Cells[i, 11].Text = item[i].GYEDATE.ToString();
                }
                btnPrint.Enabled = true;
            }
        }

        private void Spread_Print()
        {
            string strTitle = "";
            string strSign = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "암검사 상세명단";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
            //strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            TxtViewCode.Text = "";
            btnPrint.Enabled = false;
            clsPublic.GstrRetValue = "";
        }
    }
}
