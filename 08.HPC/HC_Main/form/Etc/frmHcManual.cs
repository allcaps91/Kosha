using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcManual.cs
/// Description     : 종합건진 수동호출
/// Author          : 이상훈
/// Create Date     : 2019-09-10
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm수동호출.frm(Frm수동호출)" />

namespace HC_Main
{
    public partial class frmHcManual : Form
    {
        HicWaitService hicWaitService = null;
        HeaJepsuService heaJepsuService = null;
        HeaGroupcodeService heaGroupcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();


        public frmHcManual()
        {
            InitializeComponent();

            SetEvents();
        }

        private void SetEvents()
        {
            hicWaitService = new HicWaitService();
            heaJepsuService = new HeaJepsuService();
            heaGroupcodeService = new HeaGroupcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            int nREAD = 0;
            string strDate = "";
            string strJumin = "";
            string strLtdCode = "";
            long nWRTNO = 0;

            List<HIC_WAIT> list = hicWaitService.GetItembyJobDate(clsPublic.GstrSysDate, false, "1");

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].SEQNO.ToString();
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.ToString();
                SS1.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H80000012"));
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].JEPTIME.ToString();

                //회사명 표시
                strJumin = list[i].JUMIN2;
                HEA_JEPSU list2 = heaJepsuService.GetItembyJumin(strJumin);

                strLtdCode = list2.LTDCODE.ToString();

                if (strLtdCode != "")
                {
                    SS1.ActiveSheet.Cells[i, 3].Text = " " + hb.READ_Ltd_Name(strLtdCode);
                }

                //명품검진,골드검진,VIP검진 표시
                if (list2.GJJONG == "11" || list2.GJJONG == "12")
                {
                    HEA_GROUPCODE list3 = heaGroupcodeService.GetYNamebyWrtNo(nWRTNO);

                    if (list3.YNAME.Trim() != "")
                    {
                        if (VB.InStr(list3.YNAME, "골드검진") > 0)
                        {
                            SS1.ActiveSheet.Cells[i, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFF0000"));
                        }

                        if (VB.InStr(list3.YNAME, "VIP검진") > 0)
                        {
                            SS1.ActiveSheet.Cells[i, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFF0000"));
                        }
                    }
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true) return;

            ///TODO : 이상훈(2019.09.10) 호출 화면 찾아서 Delegate 작성할것
            this.Close();
        }
    }
}
