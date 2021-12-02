using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcChartAcceptReg.cs
/// Description     : 차트인수 등록
/// Author          : 이상훈
/// Create Date     : 2020-09-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm차트인수등록.frm(Frm차트인수등록)" />

namespace ComHpcLibB
{
    public partial class frmHcChartAcceptReg : Form
    {
        HicCharttransService hicCharttransService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();


        long FnWrtNo;
        string FstrROWID;
        string FstrTrList;

        public frmHcChartAcceptReg()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCharttransService = new HicCharttransService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpreadDblClick);
            this.txtWrtNo.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRead = 0;
            int nRow = 0;
            string strTrList = "";
            string strTemp = "";

            txtWrtNo.Text = "";
            sp.Spread_All_Clear(SS1);

            List<HIC_CHARTTRANS> list = hicCharttransService.GetItembySysDate();

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strTrList = list[i].TRLIST;
                strTemp = hb.GET_TrList_Name(strTrList);

                SS1.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].ENTTIME;
                SS1.ActiveSheet.Cells[i, 4].Text = hb.READ_HIC_InsaName(list[i].ENTSABUN.To<string>());
                SS1.ActiveSheet.Cells[i, 5].Text = " " + strTemp;
                SS1.ActiveSheet.Cells[i, 7].Text = list[i].RECVTIME;
                SS1.ActiveSheet.Cells[i, 8].Text = list[i].RECVSABUN.To<string>();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnDelete)
            {
                int result = 0;

                if (MessageBox.Show("차트인수 내역을 삭제 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //HISTORY에 로그를 복사함
                result = hicCharttransService.InsertbySelect(FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                result = hicCharttransService.UpdaerecvSabunRecvTimebyWrtNo(FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text.To<long>() == FnWrtNo)
                    {
                        SS1_Sheet1.Rows[i].Remove();
                        SS1.ActiveSheet.RowCount -= 1;
                        break;
                    }
                }

                txtWrtNo.Focus();
            }
        }

        void eSpreadDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strSName = "";
                string strGjJong = "";
                string strTrList = "";

                FnWrtNo = SS1.ActiveSheet.Cells[e.Row, 0].Text.To<long>();
                txtWrtNo.Text = FnWrtNo.To<string>();
                strSName = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                strGjJong = SS1.ActiveSheet.Cells[e.Row, 2].Text;
                strTrList = SS1.ActiveSheet.Cells[e.Row, 5].Text;

                ssPatInfo.ActiveSheet.Cells[0, 0].Text = strSName;
                ssPatInfo.ActiveSheet.Cells[0, 1].Text = strGjJong;
                ssPatInfo.ActiveSheet.Cells[0, 2].Text = strTrList;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = FnWrtNo.To<string>();

                FstrTrList = strTrList;
                btnDelete.Enabled = true;
                txtWrtNo.Focus();
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            txtWrtNo.SelectAll();
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            long nWrtNo = 0;

            if (sender == txtWrtNo)
            {
                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty())
                    {
                        return;
                    }
                    nWrtNo = txtWrtNo.Text.To<long>();
                    FnWrtNo = nWrtNo;

                    fn_Screen_Display();
                }
            }
        }

        void fn_Screen_Display()
        {
            string strSName = "";
            string strGjJong = "";
            string strMsg = "";
            string strTrList = "";
            int result = 0;

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show(FnWrtNo + " 접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strSName = list.SNAME;
            strGjJong = list.GJJONG;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = strSName;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = strGjJong;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = FnWrtNo.To<string>();

            //기존 차트 인계,인수내역을 읽음
            HIC_CHARTTRANS list2 = hicCharttransService.GetItembyWrtNo(FnWrtNo);

            if (list2.IsNullOrEmpty())
            {
                MessageBox.Show("접수에서 인계하지 않은 접수번호입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strTrList = list2.TRLIST;

            ssPatInfo.ActiveSheet.Cells[0, 2].Text = hb.GET_TrList_Name(strTrList);

            btnDelete.Enabled = false;
            if (list2.RECVSABUN > 0)
            {
                btnDelete.Enabled = true;
            }

            //신규등록이면 자동으로 INSERT함
            if (list2.RECVSABUN == 0)
            {
                SS1.ActiveSheet.RowCount += 1;

                SS1_Sheet1.AddRows(0, 1);
                SS1.ActiveSheet.Cells[0, 0].Text = FnWrtNo.To<string>();
                SS1.ActiveSheet.Cells[0, 1].Text = strSName;
                SS1.ActiveSheet.Cells[0, 2].Text = strGjJong;
                SS1.ActiveSheet.Cells[0, 3].Text = list2.ENTTIME;
                SS1.ActiveSheet.Cells[0, 4].Text = list2.ENTSABUN.To<string>();
                SS1.ActiveSheet.Cells[0, 5].Text = hb.GET_TrList_Name(strTrList);
                SS1.ActiveSheet.Cells[0, 7].Text = clsPublic.GstrSysTime + ":00";
                SS1.ActiveSheet.Cells[0, 8].Text = clsType.User.UserName;

                btnDelete.Enabled = false;

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicCharttransService.UpdaterevbTimeRecvSabunbyWrtno(FnWrtNo, clsType.User.IdNumber);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            else
            {
                btnDelete.Enabled = true;
            }

            txtWrtNo.Text = "";
        }
    }
}
