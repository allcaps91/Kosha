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
using ComBase.Controls;
using ComLibB;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmReserve.cs
/// Description     : 예약선택 : 암검진 예약 등록시 기 예약 선택 Display
/// Author          : 이상훈
/// Create Date     : 2020-07-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResv_new.frm(HcAm05)" />

namespace ComHpcLibB
{
    public partial class frmHcAmRsvSelect : Form
    {
        HicCancerResv2Service hicCancerResv2Service = null;

        clsSpread sp = null;

        List<string> FstrRsvRowIdList = new List<string>();
        List<string> FstrRsvTRimeList = new List<string>();
        string FstrRsvName;
        //string FUGI;                //UGI
        //string FGFS;                //GFS
        //string FGFSH;               //GFSH
        //string FMAMMO;              //맘모
        //string FCT;                 //CT
        //string FRECTUM;             //분변
        //string FCOLON;              //COLON
        //string FSONO;               //SONO
        //string FWOMB;               //자궁경부
        //string FBOHUM;              //건진1차
        //string FLUNG_SANGDAM;       //CT사후상담

        List<string> FGBUGI = new List<string>();               //UGI
        List<string> FGBGFS = new List<string>();               //GFS
        List<string> FGBGFSH = new List<string>();              //GFSH
        List<string> FGBMAMMO = new List<string>();             //맘모
        List<string> FGBCT = new List<string>();                //CT
        List<string> FGBRECUTM = new List<string>();            //분변
        List<string> FGBCOLON = new List<string>();             //COLON
        List<string> FGBSONO = new List<string>();              //SONO
        List<string> FGBWOMB = new List<string>();              //자궁경부
        List<string> FGBBOHUM = new List<string>();             //건진1차
        List<string> FGBLUNG_SANGDAM = new List<string>();      //CT사후상담


        public delegate void SetGstrValue(string GstrValue1, string GstrValue2);
        public event SetGstrValue rSetGstrValue;

        public frmHcAmRsvSelect()
        {
            InitializeComponent();
            SetEvents();
            SetConTrol();
        }

        public frmHcAmRsvSelect(List<string> strRsvRowIdList, string strRsvName, List<string> strRsvTRimeList,
                                List<string> strGBUGI, List<string> strGBGFS, List<string> strGBGFSH, List<string> strGBMAMMO,
                                List<string> strGBCT, List<string> strGBRECUTM, List<string> strGBCOLON, List<string> strGBSONO,
                                List<string> strGBWOMB, List<string> strGBBOHUM, List<string> strGBLUNG_SANGDAM)
        {
            InitializeComponent();
            FstrRsvRowIdList = strRsvRowIdList;
            FstrRsvName = strRsvName;
            FstrRsvTRimeList = strRsvTRimeList;

            FGBUGI.Clear();
            FGBGFS.Clear();
            FGBGFSH.Clear();
            FGBMAMMO.Clear();
            FGBCT.Clear();
            FGBRECUTM.Clear();
            FGBCOLON.Clear();
            FGBSONO.Clear();
            FGBWOMB.Clear();
            FGBBOHUM.Clear();
            FGBLUNG_SANGDAM.Clear();

            FGBUGI = strGBUGI;
            FGBGFS = strGBGFS;
            FGBGFSH = strGBGFSH;
            FGBMAMMO = strGBMAMMO;
            FGBCT = strGBCT;
            FGBRECUTM = strGBRECUTM;
            FGBCOLON = strGBCOLON;
            FGBSONO = strGBSONO;
            FGBWOMB = strGBWOMB;
            FGBBOHUM = strGBBOHUM;
            FGBLUNG_SANGDAM = strGBLUNG_SANGDAM;

            SetEvents();
        }

        void SetEvents()
        {
            hicCancerResv2Service = new HicCancerResv2Service();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.ssRsv.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        void SetConTrol()
        {
            sp = new clsSpread();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //sp.Spread_All_Clear(ssRsv);

            if (FstrRsvRowIdList.Count > 0)
            {
                ssRsv.ActiveSheet.RowCount = FstrRsvRowIdList.Count;
                for (int i = 0; i < ssRsv.ActiveSheet.RowCount; i++)
                {
                    ssRsv.ActiveSheet.Cells[i, 0].Text = FstrRsvName;
                    ssRsv.ActiveSheet.Cells[i, 1].Text = FstrRsvTRimeList[i];
                    ssRsv.ActiveSheet.Cells[i, 2].Text = FstrRsvRowIdList[i];
                    ssRsv.ActiveSheet.Cells[i, 3].Text = FGBUGI[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 4].Text = FGBGFS[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 5].Text = FGBGFSH[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 6].Text = FGBMAMMO[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 7].Text = FGBCT[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 8].Text = FGBRECUTM[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 9].Text = FGBCOLON[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 10].Text = FGBSONO[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 11].Text = FGBWOMB[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 12].Text = FGBBOHUM[i] == "Y" ? "●" : "";
                    ssRsv.ActiveSheet.Cells[i, 13].Text = FGBLUNG_SANGDAM[i] == "Y" ? "●" : "";
                }
            }
            else
            {
                MessageBox.Show("예약 내역이 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                rSetGstrValue("", "");
                this.Close();
                return;
            }
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            string strRTime = "";
            string strRowId = "";

            strRTime = ssRsv.ActiveSheet.Cells[e.Row, 1].Text;
            strRowId = ssRsv.ActiveSheet.Cells[e.Row, 2].Text;
            rSetGstrValue(strRTime, strRowId);
            this.Close();
        }
    }
}
