using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanDrNoChk.cs
/// Description     : 1차 판정의사 번호 누락 명단 리스트 및 작업
/// Author          : 이상훈
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmDrNoChk.frm(HcPan97)" />

namespace HC_Pan
{
    public partial class frmHcPanDrNoChk : Form
    {
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcPanDrNoChk()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnWrok.Click += new EventHandler(eBtnClick); 
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            
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

                sp.Spread_All_Clear(SS1);

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateGjChasu();

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].GJYEAR;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].PANO.ToString();
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_GjJong_Name(list[i].GJJONG); 
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].GJCHASU;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].GJBANGI;
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].GBSTS;
                }
            }
            else if (sender == btnWrok)
            {
                //Call UPDATE_FirstPanjeng_DrNo_ALL
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
