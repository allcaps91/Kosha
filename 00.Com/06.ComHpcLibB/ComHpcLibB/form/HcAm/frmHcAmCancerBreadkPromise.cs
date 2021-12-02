using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmCancerBreadkPromise.cs
/// Description     : 암검진 부도자 관리
/// Author          : 이상훈
/// Create Date     : 2020-01-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm암부도점검.frm(Frm암부도점검)" />

namespace ComHpcLibB
{
    public partial class frmHcAmCancerBreadkPromise : Form
    {
        HicCancerResv2Service hicCancerResv2Service = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcAmCancerBreadkPromise()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicCancerResv2Service = new HicCancerResv2Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;
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
                int nRead = 0;
                string strJumin = "";
                string strFDate = "";
                string strTDate = "";

                strFDate = dtpFDate.Text;
                strTDate = DateTime.Parse(dtpFDate.Text).AddDays(1).ToShortDateString();

                sp.Spread_All_Clear(SS1);

                List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime2(strFDate, strTDate);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);

                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].RTIME.To<string>();
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].PANO;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "**";
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].TEL;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].HPHONE;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].GBUGI == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].GBGFS == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].GBMAMMO == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 9].Text = list[i].GBRECUTM == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 10].Text = list[i].GBSONO == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 11].Text = list[i].GBWOMB == "Y" ? "◎" : "";
                    SS1.ActiveSheet.Cells[i, 12].Text = list[i].REMARK;
                }

                //RowHeight 자동 조정
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (!SS1.ActiveSheet.Cells[i, 12].Text.IsNullOrEmpty())
                    {
                        Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 12);
                        SS1.ActiveSheet.Rows[i].Height = size.Height;
                    }
                    else
                    {
                        SS1.ActiveSheet.Rows[i].Height = 20;
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strPrintName = "";
                string strHeader = "";
                string strTitle = "";
                string strFooter = "";
                bool PrePrint = true;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                clsPrint CP = new clsPrint();

                strTitle = "암검진 부도자 : " + "[" + dtpFDate.Text + " ~ " + dtpTDate.Text + "]";
                
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
