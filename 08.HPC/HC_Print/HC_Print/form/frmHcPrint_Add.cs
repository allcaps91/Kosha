using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Add.cs
/// Description     : 추가검진 결과지출력
/// Author          : 김경동
/// Create Date     : 2020-07-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Hcbill106.frm(FrmAddPrint)" />
/// 
namespace HC_Print
{
    public partial class frmHcPrint_Add : Form
    {

        #region Declare Variable Area

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        #endregion

        HIC_JEPSU_RES_ETC_PATIENT nHJREP = null;
        long fnWrtno = 0;

        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();

        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;
        HicJepsuService hicJepsuService = null;
        HeaWebprtLogService heaWebprtLogService = null;
        HicResEtcService hicResEtcService = null;

        public frmHcPrint_Add()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnWebSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnWebPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnAllOk.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);

            this.ChkAll.CheckedChanged += new EventHandler(chkChanged);
        }

        private void SetControl()
        {
            LtdHelpItem = new HIC_LTD();

            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
            hicJepsuService = new HicJepsuService();
            heaWebprtLogService = new HeaWebprtLogService();
            hicResEtcService = new HicResEtcService();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            txtSname.Text = "";
            btnPrint.Enabled = false;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
                btnPrint.Enabled = true;
                btnWebPrint.Enabled = false;
            }
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
            }
            #endregion
            else if (sender == btnPrint)
            {
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {

                        HIC_JEPSU_RES_ETC_PATIENT nHJREP = new HIC_JEPSU_RES_ETC_PATIENT
                        {

                            SNAME = SSList.ActiveSheet.Cells[i, 1].Text,
                            GJJONG = SSList.ActiveSheet.Cells[i, 2].Text,
                            LTDCODE = SSList.ActiveSheet.Cells[i, 3].Text.To<long>(0),
                            JEPDATE = SSList.ActiveSheet.Cells[i, 4].Text,
                            JUMIN = SSList.ActiveSheet.Cells[i, 5].Text,
                            TONGBODATE = SSList.ActiveSheet.Cells[i, 6].Text,
                            WRTNO = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 7].Text),
                            UCODES = SSList.ActiveSheet.Cells[i, 8].Text
                        };

                        Spread_Print(nHJREP);
                    }
                }

            }

            else if (sender == btnWebSearch)
            {
                Screen_Display_Web(SSList);
                btnPrint.Enabled = false;
                btnWebPrint.Enabled = true;
            }
            else if (sender == btnWebPrint)
            {
                
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnAllOk)
            {

                long nWrtno = 0;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 10].Text);

                        //접수
                        HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(nWrtno, "4");

                        if(!item.IsNullOrEmpty())
                        {
                            hicResEtcService.UpdateByWrtnoGubun(fnWrtno, clsType.User.IdNumber.To<long>(), clsPublic.GstrSysDate, "4");
                        }

                        //접수테이블에 통보일자 세팅최초값-갱신안됨
                        HIC_JEPSU item1 = hicJepsuService.GetItemByWrtnoGjjong(fnWrtno, "54");
                        if (!item1.IsNullOrEmpty())
                        {
                            if (item1.TONGBODATE.IsNullOrEmpty())
                            {
                                hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(fnWrtno, clsType.User.IdNumber.To<long>());
                            }
                        }
                    }

                    SSList.ActiveSheet.Cells[i, 0].Text = "False";
                }

                btnSearch.PerformClick();
            }
        }

        private void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                if (e.Column == 1)
                {
                    MessageBox.Show(SSList.ActiveSheet.Cells[e.Row, 9].Text + " 종이 접수되어있습니다.");
                }
            }
        }

        private void Screen_Display(FpSpread Spd)
        {

            int nRead = 0;
            long nWrtno = 0;
            long nLtdCode = 0;
            string strSname = "";
            string strCHK3 = "";
            string strGJJONG = "";
            string strGbRe = "";

            if (txtLtdCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
            }
            if (txtSname.Text.Trim() != "")
            {
                strSname = txtSname.Text.Trim();
            }

            nWrtno = txtWrtno.Text.To<long>(0);

            strGbRe = "";
            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }

            SSList.ActiveSheet.RowCount = 0;
            List<HIC_JEPSU_RES_ETC_PATIENT> list = hicJepsuResEtcPatientService.GetItembyJepDate(dtpFDate.Text, dtpTDate.Text, nLtdCode, strSname, strGbRe,"69", nWrtno, "");

            nRead = list.Count;
            SSList.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE.Trim();
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN.Trim();
                if (!list[i].TONGBODATE.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 6].Text = list[i].TONGBODATE.Trim(); }
                SSList.ActiveSheet.Cells[i, 7].Text = list[i].WRTNO.ToString();
                if (!list[i].UCODES.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 8].Text = hm.UCode_Names_Display(list[i].UCODES.Trim()); }

                strCHK3 = "";
                strCHK3 = hm.READ_JEPSU_GBCHK3(list[i].WRTNO);
                if (strCHK3 == "OK")
                {

                    SSList.ActiveSheet.Rows[i].BackColor = Color.Aqua;
                    strGJJONG = hm.READ_JEPSU_COUNT(list[i].PTNO, list[i].JEPDATE);
                    SSList.ActiveSheet.Cells[i, 9].Text = strGJJONG;
                    //SSList.ActiveSheet.Columns.
                    if (strGJJONG != "")
                    {
                        SSList.ActiveSheet.Cells[i, 0].BackColor = Color.Red;
                    }
                }
                if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 10].Text = list[i].WEBPRINTREQ.Trim(); }
            }
        }

        private void Screen_Display_Web(FpSpread Spd)
        {

            int nRead = 0;
            long nWrtno = 0;
            long nLtdCode = 0;
            string strSname = "";
            string strCHK3 = "";
            string strGJJONG = "";
            string strGbRe = "";

            if (txtLtdCode.Text.Trim() != "")
            {
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
            }
            if (txtSname.Text.Trim() != "")
            {
                strSname = txtSname.Text.Trim();
            }

            nWrtno = txtWrtno.Text.To<long>(0);

            strGbRe = "";
            if (ChkGbRe.Checked == true)
            {
                strGbRe = "Y";
            }

            SSList.ActiveSheet.RowCount = 0;
            List<HIC_JEPSU_RES_ETC_PATIENT> list = hicJepsuResEtcPatientService.GetItembyJepDate(dtpFDate.Text, dtpTDate.Text, nLtdCode, strSname, strGbRe, "69", nWrtno, "CERT");

            nRead = list.Count;
            SSList.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                SSList.ActiveSheet.Cells[i, 2].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                SSList.ActiveSheet.Cells[i, 4].Text = list[i].JEPDATE.Trim();
                SSList.ActiveSheet.Cells[i, 5].Text = list[i].JUMIN.Trim();
                if (!list[i].TONGBODATE.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 6].Text = list[i].TONGBODATE.Trim(); }
                SSList.ActiveSheet.Cells[i, 7].Text = list[i].WRTNO.ToString();
                if (!list[i].UCODES.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 8].Text = hm.UCode_Names_Display(list[i].UCODES.Trim()); }

                strCHK3 = "";
                strCHK3 = hm.READ_JEPSU_GBCHK3(list[i].WRTNO);
                if (strCHK3 == "OK")
                {

                    SSList.ActiveSheet.Rows[i].BackColor = Color.Aqua;
                    strGJJONG = hm.READ_JEPSU_COUNT(list[i].PTNO, list[i].JEPDATE);
                    SSList.ActiveSheet.Cells[i, 9].Text = strGJJONG;
                    //SSList.ActiveSheet.Columns.
                    if (strGJJONG != "")
                    {
                        SSList.ActiveSheet.Cells[i, 0].BackColor = Color.Red;
                    }
                }
                if (!list[i].WEBPRINTREQ.IsNullOrEmpty()) { SSList.ActiveSheet.Cells[i, 10].Text = list[i].WEBPRINTREQ.Trim(); }
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdCode.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdCode.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdCode.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtLtdCode.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                if (VB.Pstr(txtLtdCode.Text, ",", 1).Trim() == "")
                {
                    txtLtdCode.Text = "";
                } 
            }
        }

        private void Spread_Print(HIC_JEPSU_RES_ETC_PATIENT nHJREP)
        {
            long nWrtno = 0;
            long nPrtCNT = 0;
            string strChk = "";
            string strCERT = "";
            string strSname = "";
            string strGJJONG = "";

            for (int i = 0; i <= SSList.ActiveSheet.RowCount; i++)
            {
                strChk = SSList.ActiveSheet.Cells[i, 0].Text;
                nWrtno = Convert.ToInt32(SSList.ActiveSheet.Cells[i, 7].Text);
                strCERT = SSList.ActiveSheet.Cells[i, 10].Text;

                fnWrtno = nWrtno;


                frmHcPrint_Add_Sub fHP = new frmHcPrint_Add_Sub(nHJREP);
                fHP.ShowDialog();

                //if (strChk == "True")
                //{
                //    if (rdoGubun1.Checked = true)
                //    {
                //        nPrtCNT = nPrtCNT + 1;
                //        Result_Print_Main();

                //    }
                //    else if (rdoGubun2.Checked = true && strChk == "True")
                //    {
                //        nPrtCNT = nPrtCNT + 1;
                //        Result_Print_Main();
                //        SSList.ActiveSheet.Cells[i, 0].Text = "";
                //    }
                //    else if (rdoGubun3.Checked = true && strChk == "")
                //    {
                //        nPrtCNT = nPrtCNT + 1;
                //        Result_Print_Main();
                //        SSList.ActiveSheet.Cells[i, 0].Text = "True";
                //    }
                //}

                if (strCERT != "")
                {
                    //접수마스터 업데이트
                    hicJepsuService.UpdateWebPrintSendByWrtNo(nWrtno);

                    HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(nWrtno);
                    strSname = item.SNAME;
                    strGJJONG = item.GJJONG;

                    HEA_WEBPRT_LOG item2 = heaWebprtLogService.GetItemByWrtnoGjjong(nWrtno, strGJJONG);
                    if (item2.IsNullOrEmpty())
                    {
                        HEA_WEBPRT_LOG item3 = new HEA_WEBPRT_LOG();

                        item3.REQDATE = clsPublic.GstrSysDate;
                        item3.WRTNO = nWrtno;
                        item3.SNAME = strSname;
                        item3.GJJONG = strGJJONG;
                        item3.ENTSABUN = long.Parse(clsType.User.IdNumber);

                        int result = heaWebprtLogService.Insert(item3);

                        if (result < 0)
                        {
                            MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }
        private void Result_Print_Main()
        {



            







            //string strBangi = "";
            //string strGbn = "";
            //string strGjjong = "";
            //string strJepdate = "";
            //string strUpdateOK = "";



            //HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(fnWrtno, "2");
            //if(item.IsNullOrEmpty())
            //{
            //    strUpdateOK = "OK";
            //}
            //else
            //{
            //    if(item.GBPRINT != "Y") { strUpdateOK = "OK"; }
            //}


            //if(strUpdateOK =="OK")
            //{
            //    hicResEtcService.UpdateByWrtnoGubun(fnWrtno, long.Parse(clsType.User.IdNumber), clsPublic.GstrSysDate ,"2");
            //}

            ////접수테이블에 통보일자 세팅최초값




        }

        private void chkChanged(object sender, EventArgs e)
        {
            int nCount = 0;

            if (sender == ChkAll)
            {
                nCount = SSList.ActiveSheet.RowCount;
                if (nCount > 0)
                {
                    SSList.ActiveSheet.Cells[0, 0, nCount - 1, 0].Text = ChkAll.Checked.ToString();
                }
            }
        }


    }
}
