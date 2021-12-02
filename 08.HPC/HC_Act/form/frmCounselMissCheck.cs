using ComBase;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmCounselMissCheck.cs
/// Description     : 상담누락체크
/// Author          : 이상훈
/// Create Date     : 2019-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAct01.frm(Frm상담누락체크)" />

namespace HC_Act
{

    public partial class frmCounselMissCheck : Form
    {
        HicResultService hicResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnWRTNO;
        int FnRowNo;
        int FnColNo;

        public frmCounselMissCheck()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);            
            this.btnHelp.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS1.KeyDown += new KeyEventHandler(eSpreadKeyDown);
            this.SS1.LeaveCell += new LeaveCellEventHandler(eSpreadLeaveCell);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();
        }

        void fn_Form_Load()
        {
            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
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
                SS1.ActiveSheet.RowCount = 0;

                //strJob, strLtdCode, strGjJong

                List<HIC_JEPSU> list = hicJepsuService.GetItembyWrtNo(dtpFrDate.Text, dtpToDate.Text);

                if (list.Count == 0) return;
                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    FnWRTNO = list[i].WRTNO;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].WRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].GJJONG;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].GBCHUL;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].GBJINCHAL;
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].SANGDAMDRNO.ToString();
                    SS1.ActiveSheet.Cells[i, 8].Text = list[i].GBJINCHAL2;
                }
            }
            else if (sender == btnClose)
            {
                pnlHelp.Visible = false;
                return;
            }
            else if (sender == btnHelp)
            {
                if (pnlHelp.Visible == false)
                {
                    pnlHelp.Visible = true;
                }
                else if (pnlHelp.Visible == true)
                {
                    pnlHelp.Visible = false;
                }
            }
            else if (sender == btnSave)
            {
                long nWrtNo = 0;
                long nDrNo = 0;

                string[] strCodes = new string[] { "A999", "ZD99", "A136", "A135" };

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtNo = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();

                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = "01";
                        item.ACTIVE = "Y";
                        item.ENTSABUN = clsType.User.IdNumber.To<long>();
                        item.WRTNO = nWrtNo;

                        int result = hicResultService.Update_Result_ChulAutoFlag(item, strCodes);

                        if (result < 0)
                        {
                            MessageBox.Show(i + "번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (SS1.ActiveSheet.Cells[i, 3].Text == "11")
                        {
                            nDrNo = SS1.ActiveSheet.Cells[i, 9].Text.To<long>();

                            if (nWrtNo == 0 || nDrNo == 0)
                            {
                                result = comHpcLibBService.UpdateHicResBohum1(nDrNo, nWrtNo);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("건강검진1차 문진 및 판정결과 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                result = comHpcLibBService.UpdateHicXMunjin(nDrNo, nWrtNo);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("방사선 작업종사자 문진 및 판정 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                result = comHpcLibBService.UpdateHicSangdamNew(nDrNo, nWrtNo);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("건강검진1차 문진 및 판정결과 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("누락건 처리 완료!! 결과입력창에서 저장 버튼을 한번더 눌러주시길 바랍니다.", "확인창", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            int nRow = 0;
            FpSpread s = (FpSpread)sender;

            if (sender == this.SS1)
            {
                if (e.ColumnHeader == true && e.RowHeader == true)
                {
                    return;
                }

                if (e.Column != 7) return;

                FnColNo = e.Column;
                lblMsg.Text = "F4.전혜리 F5.김중구 F6.김주호 F7.이홍주";
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            long nAge = 0;
            long nWRTNO = 0;
            long nWait = 0;
            string strPart = "";
            string strSName = "";
            string strSex = "";
            string strGjJong = "";
            string strRoom = "";

            FpSpread o = (FpSpread)sender;
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (sender == this.SS1)
            {
                if (e.ColumnHeader == false && e.RowHeader == false)
                {
                    return;
                }

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "True";
                    }
                }
            }
        }

        void eSpreadKeyDown(object sender, KeyEventArgs e)
        {
            int nRow = this.SS1.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS1.ActiveSheet.ActiveColumn.Index;

            long nDrNO = 0;
            string strCODE = "";
            string strResult = "";
            string strResCode = "";
            string strResType = "";

            if (FnRowNo < 1 || FnRowNo > SS1.ActiveSheet.RowCount)
            {
                return;
            }

            FnRowNo = nRow;
            strCODE = SS1.ActiveSheet.Cells[FnRowNo, 0].Text.Trim();

            strResult = "";
            switch (e.KeyCode)
            {
                case Keys.F4:
                    strResult = "전 혜 리";
                    nDrNO = 48054;
                    break;
                case Keys.F5:
                    strResult = "김 중 구";
                    nDrNO = 19516;
                    break;
                case Keys.F6:
                    strResult = "김 주 호";
                    nDrNO = 22977;
                    break;
                case Keys.F7:
                    strResult = "이 홍 주";
                    nDrNO = 1809;
                    break;
                default:
                    break;
            }

            if (!strResult.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[FnRowNo, FnColNo].Text = strResult;
                if (FnColNo == 7)
                {
                    SS1.ActiveSheet.Cells[FnRowNo, 10].Text = nDrNO.ToString();
                }

                FnRowNo += 1;
                if (FnRowNo > SS1.ActiveSheet.RowCount)
                {
                    FnRowNo = SS1.ActiveSheet.RowCount;
                }

                SS1.ActiveSheet.SetActiveCell(FnRowNo, FnColNo);
            }
        }

        void eSpreadLeaveCell(object sender, LeaveCellEventArgs e)
        {
            FnRowNo = e.Row;
        }
    }
}
