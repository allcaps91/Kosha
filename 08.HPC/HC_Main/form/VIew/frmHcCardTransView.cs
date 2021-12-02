using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcCardTransView.cs
/// Description     : 카드거래내역조회 
/// Author          : 이상훈
/// Create Date     : 2020-06-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm카드거래내역.frm(Frm카드거래내역)" />

namespace HC_Main
{
    public partial class frmHcCardTransView : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        CardApprovCenterService cardApprovCenterService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrChk;

        public frmHcCardTransView()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            cardApprovCenterService = new CardApprovCenterService();
            hicJepsuService = new HicJepsuService();
            heaJepsuService = new HeaJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnWork.Click += new EventHandler(eBtnClick);
            this.btnCardInternet.Click += new EventHandler(eBtnClick);
            //this.btnView.Click += new EventHandler(eBtnClick);
            //this.btnOK.Click += new EventHandler(eBtnClick);
            //this.btnSearch.Click += new EventHandler(eBtnClick);
            //this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtPtno.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPart.LostFocus += new EventHandler(eTxtLostFocus);
            this.cboCompany.KeyPress += new KeyPressEventHandler(eComboKeyPress);
            this.cboGubun.Click += new EventHandler(eCboClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            ssDetail_Sheet1.Columns.Get(12).Visible = false;
            ssDetail_Sheet1.Columns.Get(13).Visible = false;

            cboGubun.Items.Clear();
            cboGubun.Items.Add("매출일자");
            cboGubun.Items.Add("등록번호");
            cboGubun.Items.Add("카드번호");

            cboGubun.SelectedIndex = 0;

            cboCompany.Items.Clear();
            cboCompany.Items.Add("전체" + VB.Space(10) + "00");

            List<COMHPC> list = comHpcLibBService.GetBaseCode("CARDCODE");

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cboCompany.Items.Add(list[i].CODENAME_EX + VB.Space(10) + list[i].CODE);
                }
            }

            cboCompany.SelectedIndex = 0;

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            txtPtno.Text = "";
            FstrChk = "";

            if (clsType.User.IdNumber != "23515")
            {
                grpEDPS.Visible = false;
            }
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
                string strCardNo = "";
                double nTotCard = 0;
                double nCard = 0;
                string strGubun = "";
                string strCompany = "";
                string strPart = "";
                string strIO = "";
                string strCard = "";
                string strBun = "";
                int nRead = 0;

                Cursor.Current = Cursors.WaitCursor;

                sp.Spread_All_Clear(ssDetail);
                Application.DoEvents();

                if (cboGubun.SelectedIndex == 1)
                {
                    txtPtno.Text = string.Format("{0:00000000}", txtPtno.Text);
                }
                nTotCard = 0;

                if (cboGubun.SelectedIndex == 0)
                {
                    strGubun = "0";
                }
                else if (cboGubun.SelectedIndex == 0)
                {
                    strGubun = "1";
                }
                else
                {
                    strGubun = "";
                }

                strCompany = VB.Right(cboCompany.Text, 2);
                strPart = txtPart.Text.Trim();

                if (rdoIO0.Checked == true)
                {
                    strIO = "0";
                }
                else if (rdoIO1.Checked == true)
                {
                    strIO = "1";
                }
                else if (rdoIO2.Checked == true)
                {
                    strIO = "2";
                }

                if (rdoCard0.Checked == true)
                {
                    strCard = "0";
                }
                else if (rdoCard0.Checked == true)
                {
                    strCard = "1";
                }

                if (rdoBun1.Checked == true)
                {
                    strBun = "1";
                }
                else if (rdoBun2.Checked == true)
                {
                    strBun = "2";
                }

                CARD_APPROV_CENTER item = new CARD_APPROV_CENTER();

                item.FRDATE = dtpFrDate.Text;
                item.TODATE = dtpToDate.Text;
                item.PANO = txtPtno.Text;
                item.CARDNO = txtPtno.Text;
                item.FICODE = strCompany;
                item.PART = strPart;

                List<CARD_APPROV_CENTER> list = cardApprovCenterService.GetItembyActDate(item, strGubun, strCompany, strPart, strIO, strCard, strBun, FstrChk);

                nRead = list.Count;
                ssDetail.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    strCardNo = clsAES.DeAES(list[i].CARDNO);
                    ssDetail.ActiveSheet.Cells[i, 0].Text = list[i].ACTDATE.Trim();
                    ssDetail.ActiveSheet.Cells[i, 1].Text = list[i].PANO;
                    ssDetail.ActiveSheet.Cells[i, 2].Text = list[i].DEPTCODE.Trim();
                    ssDetail.ActiveSheet.Cells[i, 3].Text = list[i].HPANO.ToString();
                    ssDetail.ActiveSheet.Cells[i, 4].Text = list[i].SNAME;
                    ssDetail.ActiveSheet.Cells[i, 5].Text = list[i].ACCEPTERNAME;
                    ssDetail.ActiveSheet.Cells[i, 6].Text = VB.Left(strCardNo, 8) + "******";
                    ssDetail.ActiveSheet.Cells[i, 7].Text = list[i].GBRECORD;
                    ssDetail.ActiveSheet.Cells[i, 8].Text = list[i].INSTPERIOD;
                    ssDetail.ActiveSheet.Cells[i, 9].Text = list[i].ORIGINDATE;
                    nCard = list[i].TRADEAMT;
                    if (list[i].GBRECORD == "일반취소" || list[i].GBRECORD == "할부취소")
                    {
                        nCard = nCard * -1;
                    }
                    ssDetail.ActiveSheet.Cells[i, 10].Text = string.Format("{0:###,###,###,###}", nCard);
                    ssDetail.ActiveSheet.Cells[i, 11].Text = list[i].ORIGINNO;
                    ssDetail.ActiveSheet.Cells[i, 12].Text = list[i].CARDSEQNO.ToString();
                    ssDetail.ActiveSheet.Cells[i, 13].Text = list[i].HWRTNO.ToString();
                    nTotCard = nTotCard + nCard;
                }

                ssDetail.ActiveSheet.RowCount += 1;
                ssDetail.ActiveSheet.Cells[ssDetail.ActiveSheet.RowCount - 1, 9].Text = "합 계";
                ssDetail.ActiveSheet.Cells[ssDetail.ActiveSheet.RowCount - 1, 10].Text = string.Format("{0:###,###,###,###}", nTotCard);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;
                string strBun = "";

                Cursor.Current = Cursors.WaitCursor;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                if (rdoBun0.Checked == true)
                {
                    strBun = "(전체)";
                }
                else if (rdoBun1.Checked == true)
                {
                    strBun = "(PC)";
                }
                else if (rdoBun2.Checked == true)
                {
                    strBun = "(단말)";
                }

                strTitle = "카드거래내역조회" + strBun;
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("매출일자 : " + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("맑은 고딕", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(ssDetail, PrePrint, setMargin, setOption, strHeader, strFooter);

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnCancel)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                if (cboGubun.SelectedIndex == 0)
                {
                    txtPtno.Visible = false;
                    dtpFrDate.Visible = true;
                    lblHiphone.Visible = true;
                    dtpToDate.Visible = true;
                    dtpFrDate.Text = clsPublic.GstrSysDate;
                    dtpToDate.Text = clsPublic.GstrSysDate;
                }
                else
                {
                    txtPtno.Visible = true;
                    txtPtno.Text = "";
                    txtPtno.Focus();
                    dtpFrDate.Visible = true;
                    lblHiphone.Visible = true;
                    dtpToDate.Visible = true;
                }

                txtPart.Text = "";
                sp.Spread_All_Clear(ssDetail);
                ssDetail.ActiveSheet.RowCount = 30;
            }
            else if (sender == btnView)
            {
                FstrChk = "OK";
                eBtnClick(btnSearch, new EventArgs());
                FstrChk = "";
            }
            else if (sender == btnWork)
            {
                string strBDATE = "";
                string strDept = "";
                string strPano = "";
                long nCardSeq = 0;
                long nHWrtno = 0;
                long nWRTNO = 0;
                int result = 0;                     

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssDetail.ActiveSheet.RowCount; i++)
                {
                    strBDATE = ssDetail.ActiveSheet.Cells[i, 0].Text.Trim();
                    strPano = ssDetail.ActiveSheet.Cells[i, 1].Text.Trim();
                    strDept = ssDetail.ActiveSheet.Cells[i, 2].Text.Trim();
                    nCardSeq = long.Parse(ssDetail.ActiveSheet.Cells[i, 12].Text);
                    nHWrtno = long.Parse(ssDetail.ActiveSheet.Cells[i, 13].Text);
                    nWRTNO = 0;
                    if (nHWrtno == 0)
                    {
                        if (strDept == "HR")
                        {
                            nWRTNO = hicJepsuService.GetWrtNobyJepDateCardSeqNo(strBDATE, nCardSeq);
                        }
                        else if (strDept == "TO")
                        {
                            nWRTNO = heaJepsuService.GetWrtNobyJepDateCardSeqNo(strBDATE, nCardSeq);
                        }

                        if (nWRTNO != 0)
                        {
                            result = cardApprovCenterService.UpdateHWrtNobyCardSeqNo(nWRTNO, nCardSeq, strPano);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("CARD_APPROV_CENTER에 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnCardInternet)
            {
                string strPath = @"C:\Program Files\Internet Explorer\";

                if (hf.Dir_Check_YN(strPath) == true)
                {
                    VB.Shell(@"C:\Program Files\Internet Explorer\iexplore.exe" + " " + "http://store.ikoces.com/" + "", "NormalFocus");
                }
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtPart)
            {
                if (txtPart.Text.IsNullOrEmpty())
                {
                    return;
                }
                txtPart.Text = txtPart.Text.ToUpper();
                //SendKeys.Send("{ TAB}");
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtPtno)
                {
                    if (cboGubun.SelectedIndex == 1)
                    {
                        txtPtno.Text = string.Format("{0:00000000}", txtPtno.Text);
                    }
                    btnSearch.Focus();
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
        }

        void eComboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == cboCompany)
                {
                    txtPtno.Focus();
                }
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboGubun)
            {
                if (cboGubun.SelectedIndex == 0)
                {
                    grpNo.Enabled = false;
                    dtpFrDate.Enabled = true;
                    lblHiphone.Enabled = true;
                    dtpToDate.Enabled = true;
                }
                else
                {
                    dtpFrDate.Enabled = false;
                    lblHiphone.Enabled = false;
                    dtpToDate.Enabled = false;
                    grpNo.Enabled = true;
                    if (cboGubun.Text == "등록번호")
                    {
                        txtPtno.MaxLength = 8;
                    }
                    else
                    {
                        txtPtno.MaxLength = 16;
                    }
                    txtPtno.Text = "";
                }
            }
        }
    }
}
