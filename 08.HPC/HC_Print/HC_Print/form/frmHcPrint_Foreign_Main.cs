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
/// File Name       : frmHcPrint_Foreign_Main.cs
/// Description     : 추가검진 결과지출력
/// Author          : 김경동
/// Create Date     : 2020-07-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " HcPrint12.frm(Frm채용신체검사_일반)" />
/// 
namespace HC_Print.form
{
    public partial class frmHcPrint_Foreign_Main : Form
    {

        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();

        HicJepsuResSpecialPatientService hicJepsuResSpecialPatientService = null;

        public frmHcPrint_Foreign_Main()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

        }

        private void SetControl()
        {
            hicJepsuResSpecialPatientService = new HicJepsuResSpecialPatientService();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();


            btnPrint.Enabled = false;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
                btnPrint.Enabled = true;
            }
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

            int nRead = 0;
            string strSname = "";
            string strEname = "";
            string strSExams = "";
            string strCHK3 = "";
            string strGBSPC = "";


            if (txtSname.Text.Trim() != "")
            {
                strSname = txtSname.Text.Trim();
            }

            if (ChkEname.Checked == true)
            {
                strEname = "Y";
            }

            SSList.ActiveSheet.RowCount = 0;
            List<HIC_JEPSU_RES_SPECIAL_PATIENT> list = hicJepsuResSpecialPatientService.GetItemByJepdate(dtpFDate.Text, dtpTDate.Text);

            nRead = list.Count;
            SSList.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].JUMIN.Trim();
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].SNAME.Trim();
                SSList.ActiveSheet.Cells[i, 4].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].JEPDATE.Trim();
                strGBSPC = list[i].GBSPC.Trim();
                switch (strGBSPC)
                {
                    case "1":
                    case "01":
                        SSList.ActiveSheet.Cells[i, 6].Text = "일반+특수";
                        break;
                    case "2":
                    case "02":
                        SSList.ActiveSheet.Cells[i, 6].Text = "특수";
                        break;
                    case "3":
                    case "03":
                        SSList.ActiveSheet.Cells[i, 6].Text = "배치전";
                        break;
                    case "4":
                    case "04":
                        SSList.ActiveSheet.Cells[i, 6].Text = "채용+배치전";
                        break;
                    case "5":
                    case "05":
                        SSList.ActiveSheet.Cells[i, 6].Text = "수시";
                        break;
                    case "6":
                    case "06":
                        SSList.ActiveSheet.Cells[i, 6].Text = "임시";
                        break;
                    case "7":
                    case "07":
                        SSList.ActiveSheet.Cells[i, 6].Text = "채용";
                        break;
                    case "8":
                    case "08":
                        SSList.ActiveSheet.Cells[i, 6].Text = "일반";
                        break;
                    case "9":
                    case "09":
                        SSList.ActiveSheet.Cells[i, 6].Text = "일반+배치전";
                        break;
                    case "10":
                        SSList.ActiveSheet.Cells[i, 6].Text = "공무원채용";
                        break;
                }

                //8-ROWID, 9-전화번호 10-검진종류
                if (!list[i].TEL.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].TEL.Trim();
                }
                
                if (VB.InStr(list[i].SEXAMS, "J134") > 0) { strSExams = "E-2"; }
                if (VB.InStr(list[i].SEXAMS, "J225") > 0) { strSExams = "H-2"; }
                if (VB.InStr(list[i].SEXAMS, "J223") > 0) { strSExams = "E-9"; }

                SSList.ActiveSheet.Cells[i, 8].Text = strSExams;
                SSList.ActiveSheet.Cells[i, 9].Text = "";

                strCHK3 = "";
                strCHK3 = hm.READ_JEPSU_GBCHK3(list[i].WRTNO);
                if (strCHK3 == "OK")
                {
                    SSList.ActiveSheet.Rows[i].BackColor = Color.Aqua;
                }

            }
        }

        private void Spread_Print()
        {





        }
    }
}
