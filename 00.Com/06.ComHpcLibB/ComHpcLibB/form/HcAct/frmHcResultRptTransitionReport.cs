using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using System.Drawing;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcResultRptTransitionReport.cs
/// Description     : 결과지 본인수령 인수인계 대장
/// Author          : 이상훈
/// Create Date     : 2020-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm결과지인수인계대장.frm(Frm결과지인수인계대장)" />

namespace ComHpcLibB
{
    public partial class frmHcResultRptTransitionReport : Form
    {
        HicCharttransPrintService hicCharttransPrintService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcResultRptTransitionReport()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCharttransPrintService = new HicCharttransPrintService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtSName.Text = "";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            long nWrtNo = 0;
            string strDate = "";
            int result = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            nWrtNo = ssList.ActiveSheet.Cells[e.Row, 1].Text.To<long>();

            clsDB.setBeginTran(clsDB.DbCon);

            if (e.Column == 8)
            {
                strDate = ssList.ActiveSheet.Cells[e.Row, 8].Text;
                //인계저장
                result = hicCharttransPrintService.UpdatebyWrtNo(strDate, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, clsType.User.IdNumber, nWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인수인계(인계) 저장시 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (e.Column == 9)
            {
                strDate = ssList.ActiveSheet.Cells[e.Row, 9].Text;
                //인수저장
                result = hicCharttransPrintService.UpdateRecvbyWrtNo(strDate, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, clsType.User.IdNumber, nWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인수인계(인수) 저장시 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (e.Column == 10)
            {
                strDate = ssList.ActiveSheet.Cells[e.Row, 10].Text;
                //결과지전달저장
                result = hicCharttransPrintService.UpdateJobbyWrtNo(strDate, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, clsType.User.IdNumber, nWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("인수인계저장(결과지전달)시 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (e.Column == 11)
            {
                ssList.ActiveSheet.Cells[e.Row, 0].Text = "True";
            }

            clsDB.setCommitTran(clsDB.DbCon);
            eBtnClick(btnSearch, new EventArgs());
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
                string strFrDate = "";
                string strToDate = "";
                string strSName = "";
                string strJob = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strSName = txtSName.Text;
                if (rdoJob2.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob3.Checked == true)
                {
                    strJob = "2";
                }
                else if (rdoJob4.Checked == true)
                {
                    strJob = "3";
                }
                else if (rdoJob5.Checked == true)
                {
                    strJob = "4";
                }
                else
                {
                    strJob = "";
                }

                sp.Spread_All_Clear(ssList);
                ssList_Sheet1.SetRowHeight(-1, 50);

                List<HIC_CHARTTRANS_PRINT> list = hicCharttransPrintService.GetItembyJepDateJob(strFrDate, strToDate, strSName, strJob);

                ssList.ActiveSheet.RowCount = list.Count;

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].WRTNO.To<string>();
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE;
                        ssList.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[i, 5].Text = list[i].BIRTH;
                        ssList.ActiveSheet.Cells[i, 6].Text = hf.Read_HPhone(list[i].WRTNO);
                        ssList.ActiveSheet.Cells[i, 7].Text = list[i].LTDNAME;
                        if (!list[i].ENTSABUN.IsNullOrEmpty() && list[i].ENTSABUN != 0)
                        {
                            ssList.ActiveSheet.Cells[i, 8].Text = hb.READ_HIC_InsaName(list[i].ENTSABUN.To<string>()) + "\r\n" + list[i].ENTTIME; ;
                        }
                        if (!list[i].RECVSABUN.IsNullOrEmpty() && list[i].RECVSABUN != 0)
                        {
                            ssList.ActiveSheet.Cells[i, 9].Text = hb.READ_HIC_InsaName(list[i].RECVSABUN.To<string>()) + "\r\n" + list[i].RECVTIME; ;
                        }
                        if (!list[i].JOBSABUN.IsNullOrEmpty() && list[i].JOBSABUN != 0)
                        {
                            ssList.ActiveSheet.Cells[i, 10].Text = hb.READ_HIC_InsaName(list[i].JOBSABUN.To<string>()) + "\r\n" + list[i].JOBTIME; ;
                        }
                        ssList.ActiveSheet.Cells[i, 11].Text = list[i].REMARK;
                    }

                    //RowHeight 설정
                    //for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                    //{
                    //    if (!ssList.ActiveSheet.Cells[i, 8].Text.IsNullOrEmpty())
                    //    {
                    //        Size size = ssList.ActiveSheet.GetPreferredCellSize(i, 0);
                    //        ssList.ActiveSheet.Rows[i].Height = size.Height;
                    //    }
                    //    else
                    //    {
                    //        ssList.ActiveSheet.Rows[i].Height = 20;
                    //    }

                    //    if (!ssList.ActiveSheet.Cells[i, 9].Text.IsNullOrEmpty())
                    //    {
                    //        Size size = ssList.ActiveSheet.GetPreferredCellSize(i, 0);
                    //        ssList.ActiveSheet.Rows[i].Height = size.Height;
                    //    }
                    //    else
                    //    {
                    //        ssList.ActiveSheet.Rows[i].Height = 20;
                    //    }

                    //    if (!ssList.ActiveSheet.Cells[i, 10].Text.IsNullOrEmpty())
                    //    {
                    //        Size size = ssList.ActiveSheet.GetPreferredCellSize(i, 0);
                    //        ssList.ActiveSheet.Rows[i].Height = size.Height;
                    //    }
                    //    else
                    //    {
                    //        ssList.ActiveSheet.Rows[i].Height = 20;
                    //    }

                    //    if (!ssList.ActiveSheet.Cells[i, 11].Text.IsNullOrEmpty())
                    //    {
                    //        Size size = ssList.ActiveSheet.GetPreferredCellSize(i, 0);
                    //        ssList.ActiveSheet.Rows[i].Height = size.Height;
                    //    }
                    //    else
                    //    {
                    //        ssList.ActiveSheet.Rows[i].Height = 20;
                    //    }
                    //}
                }
                txtSName.Text = "";
            }
            else if (sender == btnSave)
            {
                long nWrtNo = 0;
                string strRemark = "";
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtNo = ssList.ActiveSheet.Cells[i, 1].Text.To<long>();
                        strRemark = ssList.ActiveSheet.Cells[i, 11].Text;

                        result = hicCharttransPrintService.UpdateRemarkbyWrtNo(strRemark, nWrtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("인수인계 저장시 오류발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                eBtnClick(btnSearch, new EventArgs());

            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ssList_Sheet1.Columns.Get(0).Visible = false;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "결과지 본인수령 인수/인계 대장";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("조회기간:" + dtpFrDate.Text + "~" + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

                ssList_Sheet1.Columns.Get(0).Visible = true;
            }
        }
    }
}
