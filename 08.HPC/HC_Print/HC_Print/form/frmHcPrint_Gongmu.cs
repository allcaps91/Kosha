using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Gongmu.cs
/// Description     : 공무원 채용검진 검사서 출력
/// Author          : 김경동
/// Create Date     : 2021-02-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Frm채용신체검사_new.frm(Frm채용신체검사_new)" />
/// 


namespace HC_Print
{
    public partial class frmHcPrint_Gongmu : Form
    {

        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();

        HicJepsuResSpecialPatientService hicJepsuResSpecialPatientService = null;

        public frmHcPrint_Gongmu()
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

            this.menuExit.Click += new EventHandler(eMenuClick);

        }

        private void SetControl()
        {
            hicJepsuResSpecialPatientService = new HicJepsuResSpecialPatientService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-3).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

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
                long nWrtno = 0;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {

                        HIC_JEPSU_RES_SPECIAL_PATIENT nHJRSP = new HIC_JEPSU_RES_SPECIAL_PATIENT
                        {
                            WRTNO = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 1].Text),
                            JUMIN = SSList.ActiveSheet.Cells[i, 2].Text,
                            SNAME = SSList.ActiveSheet.Cells[i, 3].Text,
                            HNAME = SSList.ActiveSheet.Cells[i, 4].Text,
                            JEPDATE = SSList.ActiveSheet.Cells[i, 5].Text,
                            PANO = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 7].Text),
                            AGE = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 8].Text),
                            LTDCODE = hb.READ_Ltd_Name(SSList.ActiveSheet.Cells[i, 9].Text),
                            GJJONG = SSList.ActiveSheet.Cells[i, 10].Text
                        };

                        nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 10].Text);
                        Spread_Print(nHJRSP);
                    }
                }
            }
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {

            string strCHK3 = "";

            List<HIC_JEPSU_RES_SPECIAL_PATIENT> list = hicJepsuResSpecialPatientService.GetItemByJepdateGbspc(dtpFDate.Text, dtpTDate.Text);

            
            if(!list.IsNullOrEmpty())
            {
                SSList.ActiveSheet.RowCount = list.Count;

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].WRTNO.ToString();
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JUMIN;
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i, 4].Text = list[i].HNAME;
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].JEPDATE;

                    switch (list[i].GBSPC)
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
                        default:
                            break;
                    }

                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].PANO.ToString();
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].AGE + "/" +list[i].SEX;
                    SSList.ActiveSheet.Cells[i, 9].Text = VB.Format(list[i].LTDCODE,"#");
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].GJJONG;
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].GBSPC;

                    strCHK3 = cHcMain.READ_JEPSU_GBCHK3(list[i].WRTNO);
                    if (strCHK3 =="OK")
                    {
                        
                    }

                }
            } 
        }

        private void Spread_Print(HIC_JEPSU_RES_SPECIAL_PATIENT nHJRSP)
        {

            //frmHcPrint_Gongmu_Sub fHP = new frmHcPrint_Gongmu_Sub(argWrtno);
            //fHP.ShowDialog();
        }

    }

}
