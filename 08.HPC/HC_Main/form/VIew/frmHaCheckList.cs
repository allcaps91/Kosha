using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaCheckList.cs
/// Description     : 종검 수검자 Check List
/// Author          : 김민철
/// Create Date     : 2019-09-19
/// Update History  : 
/// </summary>
/// <seealso cref= "FrmCheckList(FrmCheckList.frm)" />
namespace HC_Main
{
    public partial class frmHaCheckList : BaseForm
    {
        bool bolSort = false;

        clsHaBase cHB = null;
        clsHcFunc cHF = null;
        HIC_LTD LtdHelpItem = null;
        HicLtdService hicLtdService = null;
        HeaJepsuService heaJepsuService = null;
        HeaMailSendJepsuService heaMailSendJepsuService = null;

        public frmHaCheckList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cHB = new clsHaBase();
            cHF = new clsHcFunc();
            LtdHelpItem = new HIC_LTD();
            hicLtdService = new HicLtdService();
            heaJepsuService = new HeaJepsuService();
            heaMailSendJepsuService = new HeaMailSendJepsuService();

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong2_AddItem(cboJong);

            SSList.Initialize(new SpreadOption { RowHeaderVisible = true });
            SSList.ActiveSheet.ColumnHeader.Rows[0].Height = 38;
            SSList.AddColumn("수검일자",   nameof(HEA_JEPSU.SDATE),       88, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { });
            SSList.AddColumn("등록번호",   nameof(HEA_JEPSU.PTNO),        84, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { });
            SSList.AddColumn("종류",       nameof(HEA_JEPSU.JONG_GB),     44, FpSpreadCellType.TextCellType);
            SSList.AddColumn("수검자명",   nameof(HEA_JEPSU.SNAME),       64, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { });
            SSList.AddColumn("나이",       nameof(HEA_JEPSU.AGESEX),      44, FpSpreadCellType.TextCellType);
            SSList.AddColumn("회사명",     nameof(HEA_JEPSU.LTDNAME),    140, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("수검상태",   nameof(HEA_JEPSU.GBSTS_NM),    92, IsReadOnly.Y,     new SpreadCellTypeOption { ICustomCellType = new GbStsCellType() });
            SSList.AddColumn("가판정정보", nameof(HEA_JEPSU.GAPAN_INFO),  90, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsMulti = true });
            SSList.AddColumn("판정정보",   nameof(HEA_JEPSU.PAN_INFO),    90, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsMulti = true });
            SSList.AddColumn("출력정보",   nameof(HEA_JEPSU.PRT_INFO),    90, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsMulti = true });
            SSList.AddColumn("수령일자",   nameof(HEA_JEPSU.RECV_INFO),   90, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsMulti = true });
            SSList.AddColumn("발송일자",   "",                            90, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsMulti = true });
            SSList.AddColumn("경과일수",   "",                            44, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { });
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click   += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.dtpFDate.TextChanged += new EventHandler(eDtpChanged);
            this.SSList.CellClick += new CellClickEventHandler(eSpdCellClick);
        }

        private void eSpdCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SSList, e.Column, ref bolSort, true);
            }
        }

        private void eDtpChanged(object sender, EventArgs e)
        {
            if (sender == dtpFDate)
            {
                dtpTDate.Value = dtpFDate.Value;
                dtpTDate.Text = dtpFDate.Text;
            }
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                Search_Data();
            }
        }

        private void Search_Data()
        {
            string strDate = string.Empty;

            SSList.ActiveSheet.ClearRange(0, 0, SSList.ActiveSheet.Rows.Count, SSList.ActiveSheet.ColumnCount, true);
            SSList.ActiveSheet.Rows.Count = 5;

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strJong = VB.Left(cboJong.Text, 2);
            string strName = txtSName.Text;
            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            bool bGaPan = chkSel1.Checked;
            bool bPan = chkSel2.Checked;
            bool bPrt = chkSel3.Checked;
            bool bBal = chkSel4.Checked;

            List<HEA_JEPSU> list = heaJepsuService.GetListByItems(strFDate, strTDate, "접수", strJong, strName, nLtdCode, false, bGaPan, bPan, bPrt, bBal);

            if (list.Count > 0)
            {
                SSList.ActiveSheet.RowCount = list.Count;

                HEA_MAILSEND hMS = new HEA_MAILSEND();

                for (int i = 0; i < list.Count; i++)
                {
                    strDate = clsPublic.GstrSysDate;

                    SSList.ActiveSheet.Cells[i,  0].Text = list[i].SDATE.ToString();
                    SSList.ActiveSheet.Cells[i,  1].Text = list[i].PTNO;
                    SSList.ActiveSheet.Cells[i,  2].Text = list[i].JONG_GB;
                    SSList.ActiveSheet.Cells[i,  3].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[i,  4].Text = list[i].AGESEX;
                    SSList.ActiveSheet.Cells[i,  5].Text = list[i].LTDNAME;
                    SSList.ActiveSheet.Cells[i,  6].Text = list[i].GBSTS_NM;
                    SSList.ActiveSheet.Cells[i,  7].Text = list[i].GAPAN_INFO;
                    SSList.ActiveSheet.Cells[i,  8].Text = list[i].PAN_INFO;
                    SSList.ActiveSheet.Cells[i,  9].Text = list[i].PRT_INFO;
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].RECV_INFO;
                    SSList.ActiveSheet.Cells[i, 11].Text = "";

                    hMS = heaMailSendJepsuService.GetSendDateByWrtno(list[i].WRTNO);

                    if (hMS.IsNullOrEmpty())
                    {
                        if (!list[i].RECVDATE.IsNullOrEmpty())
                        {
                            strDate = list[i].RECVDATE.To<string>("");
                        }
                    }
                    else
                    {
                        if (!hMS.SENDDATE.IsNullOrEmpty())
                        {
                            strDate = hMS.SENDDATE;
                            SSList.ActiveSheet.Cells[i, 11].Text = hMS.SEND_INFO;
                        }
                        if (!list[i].RECVDATE.IsNullOrEmpty()) {strDate = list[i].RECVDATE;}
                    }
                    
                    //SSList.ActiveSheet.Cells[i, 11].Text = VB.Format(list[i].MAILDATE, "yyyy-MM-dd");
                    SSList.ActiveSheet.Cells[i, 12].Text = VB.Format(cHF.DATE_ILSU_YOIL(list[i].SDATE, strDate),"00");

                    Size size = SSList.ActiveSheet.GetPreferredCellSize(i, 7);
                    SSList.ActiveSheet.Rows[i].Height = size.Height;

                    if (!list[i].WEBPRINTSEND.IsNullOrEmpty() && !list[i].WEBPRINTREQ.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[i,  9].Text = list[i].WEBPRINTSEND + ComNum.VBLF + "전자출력";
                        SSList.ActiveSheet.Cells[i, 11].Text = "";
                        //SSList.ActiveSheet.Cells[i, 12].Text = "";
                        SSList.ActiveSheet.Cells[i, 12].Text = VB.Format(cHF.DATE_ILSU_YOIL(list[i].SDATE, list[i].WEBPRINTSEND), "00");
                    }
                }
            }

            Cursor.Current = Cursors.Default;

        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cboJong.SelectedIndex = 0;
            SSList.ActiveSheet.RowCount = 5;
        }
    }
}
