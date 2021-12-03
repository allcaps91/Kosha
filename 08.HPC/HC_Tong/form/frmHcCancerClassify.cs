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
/// Class Name      : HC_Tong
/// File Name       : frmHcPersonnelTotal.cs
/// Description     : 암검사분류별 인원통계
/// Author          : 심명섭
/// Create Date     : 2021-06-02
/// Update History  : 
/// </summary>
/// <seealso cref= "Hc_Tong > Frm암검사분류인원 (Frm암검사분류인원.frm)" />

namespace HC_Tong
{
    public partial class frmHcCancerClassify :BaseForm
    {
        // Spread
        clsSpread cSpd = null;
        ComFunc cf = null;
        clsHaBase cHB = null;
        HicJepsuService hicJepsuService = null;
        HicTongamService hicTongamService = null;
        clsHaBase hb = null;

        public frmHcCancerClassify()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            // Service 선언 
            // Service -> Repository
            cSpd = new clsSpread();
            cf = new ComFunc();
            cHB = new clsHaBase();
            hicJepsuService = new HicJepsuService();
            hicTongamService = new HicTongamService();
            hb = new clsHaBase();
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
            cSpd.Spread_All_Clear(SS1);
            cSpd.Spread_All_Clear(SS2);
            cSpd.Spread_All_Clear(SS3);
            long [] nAmCnt = new long[8];
            
            for(int i = 0; i <= 7; i++)
            {
                nAmCnt[i] = 0;
            }

            List<HIC_JEPSU> list_S1 = hicJepsuService.GetJepsuGunsu(dtpFDate.Text, dtpTDate.Text, VB.Left(cboJONG.Text, 2));
            SS1.ActiveSheet.RowCount = list_S1.Count + 1;

            for(int i = 0; i < list_S1.Count; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list_S1[i].JEPDATE;
                SS1.ActiveSheet.Cells[i, 1].Text = list_S1[i].PANO.To<string>("0");
                SS1.ActiveSheet.Cells[i, 2].Text = list_S1[i].WRTNO.To<string>("0");
                SS1.ActiveSheet.Cells[i, 3].Text = list_S1[i].SNAME;

                SS1.ActiveSheet.Cells[i, 4].Text = (VB.Val(SS1.Text) + list_S1[i].CAN1).To<string>("0");
                nAmCnt[1] += list_S1[i].CAN1; // 위1

                SS1.ActiveSheet.Cells[i, 5].Text = (VB.Val(SS1.Text) + list_S1[i].CAN2).To<string>("0");
                nAmCnt[2] += list_S1[i].CAN2; // 위2

                SS1.ActiveSheet.Cells[i, 6].Text = (VB.Val(SS1.Text) + list_S1[i].CAN3).To<string>("0");
                nAmCnt[3] += list_S1[i].CAN3; // 대장

                SS1.ActiveSheet.Cells[i, 7].Text = (VB.Val(SS1.Text) + list_S1[i].CAN4).To<string>("0");
                nAmCnt[4] += list_S1[i].CAN4; // 간

                SS1.ActiveSheet.Cells[i, 8].Text = (VB.Val(SS1.Text) + list_S1[i].CAN5).To<string>("0");
                nAmCnt[5] += list_S1[i].CAN5; // 유방

                SS1.ActiveSheet.Cells[i, 9].Text = (VB.Val(SS1.Text) + list_S1[i].CAN6).To<string>("0");
                nAmCnt[6] += list_S1[i].CAN6; // 자궁
            }

            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 2].Text = "계";
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 4].Text = nAmCnt[1].To<string>("0");
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 5].Text = nAmCnt[2].To<string>("0");
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 6].Text = nAmCnt[3].To<string>("0");
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 7].Text = nAmCnt[4].To<string>("0");
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 8].Text = nAmCnt[5].To<string>("0");
            SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, 9].Text = nAmCnt[6].To<string>("0");


            // 접수기준 월별

            for (int i = 0; i <= 7; i++)
            {
                nAmCnt[i] = 0;
            }

            int result = hicJepsuService.GetTongAm(dtpFDate.Text, dtpTDate.Text, VB.Left(cboJONG.Text, 2));

            List<HIC_JEPSU> list_View = hicJepsuService.GetTongAm_View(dtpFDate.Text, dtpTDate.Text, VB.Left(cboJONG.Text, 2));

            SS2.ActiveSheet.RowCount = list_View.Count + 1;

            for (int i = 0; i < list_View.Count; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = list_View[i].JEPDATE + "월";

                SS2.ActiveSheet.Cells[i, 1].Text = list_View[i].CNT1.To<string>("0");
                nAmCnt[1] += list_View[i].CNT1;

                SS2.ActiveSheet.Cells[i, 2].Text = list_View[i].CNT2.To<string>("0");
                nAmCnt[2] += list_View[i].CNT2;

                SS2.ActiveSheet.Cells[i, 3].Text = list_View[i].CNT3.To<string>("0");
                nAmCnt[3] += list_View[i].CNT3;

                SS2.ActiveSheet.Cells[i, 4].Text = list_View[i].CNT4.To<string>("0");
                nAmCnt[4] += list_View[i].CNT4;

                SS2.ActiveSheet.Cells[i, 5].Text = list_View[i].CNT5.To<string>("0");
                nAmCnt[5] += list_View[i].CNT5;

                SS2.ActiveSheet.Cells[i, 6].Text = list_View[i].CNT6.To<string>("0");
                nAmCnt[6] += list_View[i].CNT6;
            }

            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 0].Text = "계";
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 1].Text = nAmCnt[1].To<string>("0");
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 2].Text = nAmCnt[2].To<string>("0");
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 3].Text = nAmCnt[3].To<string>("0");
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 4].Text = nAmCnt[4].To<string>("0");
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 5].Text = nAmCnt[5].To<string>("0");
            SS2.ActiveSheet.Cells[SS2.ActiveSheet.RowCount - 1, 6].Text = nAmCnt[6].To<string>("0");

            // 통계기준 

            for (int i = 0; i <= 7; i++)
            {
                nAmCnt[i] = 0;
            }

            List<HIC_TONGAM> list_S2 = hicTongamService.GetCancerTong(dtpFDate.Text, dtpTDate.Text, VB.Left(cboJONG.Text, 2));

            SS3.ActiveSheet.RowCount = list_S2.Count + 1;

            for (int i = 0; i < list_S2.Count; i++)
            {
                SS3.ActiveSheet.Cells[i, 0].Text = hb.READ_GjJong_Name(list_S2[i].GJJONG);

                SS3.ActiveSheet.Cells[i, 1].Text = list_S2[i].JEPCNT.To<string>("0");
                nAmCnt[7] += list_S2[i].JEPCNT;

                SS3.ActiveSheet.Cells[i, 2].Text = list_S2[i].SUBTOT.To<string>("0");
                nAmCnt[0] += list_S2[i].SUBTOT.To<long>(0);
                
                SS3.ActiveSheet.Cells[i, 3].Text = (VB.Val(SS3.Text) + list_S2[i].CNT1).To<string>("0"); 
                nAmCnt[1] += list_S2[i].CNT1; // 위1

                SS3.ActiveSheet.Cells[i, 4].Text = (VB.Val(SS3.Text) + list_S2[i].CNT2).To<string>("0"); 
                nAmCnt[2] += list_S2[i].CNT2; // 위2

                SS3.ActiveSheet.Cells[i, 5].Text = (VB.Val(SS3.Text) + list_S2[i].CNT3).To<string>("0");
                nAmCnt[3] += list_S2[i].CNT3; // 대장

                SS3.ActiveSheet.Cells[i, 6].Text = (VB.Val(SS3.Text) + list_S2[i].CNT4).To<string>("0");
                nAmCnt[4] += list_S2[i].CNT4; // 간

                SS3.ActiveSheet.Cells[i, 7].Text = (VB.Val(SS3.Text) + list_S2[i].CNT5).To<string>("0");
                nAmCnt[5] += list_S2[i].CNT5; // 유방

                SS3.ActiveSheet.Cells[i, 8].Text = (VB.Val(SS3.Text) + list_S2[i].CNT6).To<string>("0");
                nAmCnt[6] += list_S2[i].CNT6; // 자궁
            }

            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 0].Text = "계";
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 1].Text = nAmCnt[7].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 2].Text = nAmCnt[0].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 3].Text = nAmCnt[1].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 4].Text = nAmCnt[2].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 5].Text = nAmCnt[3].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 6].Text = nAmCnt[4].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 7].Text = nAmCnt[5].To<string>("0");
            SS3.ActiveSheet.Cells[SS3.ActiveSheet.RowCount - 1, 8].Text = nAmCnt[6].To<string>("0");
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
            strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, false, false, true, 1f);
            cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;

            // 검진종류 SET
            cboJONG.Items.Clear();
            cboJONG.Items.Add("00. 전체");
            cHB.ComboJong_AddItem(cboJONG, false);
        }
    }
}
