using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using ComPmpaLibB;
using System.Windows.Forms;
using FarPoint.Win.Spread;


/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcCharttrans_Insert.cs
/// Description     : 차트인계등록화면/// Author          : 김경동
/// Create Date     : 2020-07-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm차트인계등록.frm(Frm차트인계등록)" />
namespace HC_Main
{
    public partial class frmHcCharttrans_Insert : Form
    {
        long FnWRTNO = 0;
        string FstrROWID = "";
        string FstrTrList = "";

        HicCharttransService hicCharttransService = null;
        HicCharttransHisService hicCharttransHisService = null;
        HicJepsuService hicJepsuService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResultService hicResultService = null;

        ComFunc CF = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsPublic cpublic = new clsPublic();

        public frmHcCharttrans_Insert()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicCharttransService = new HicCharttransService();
            hicCharttransHisService = new HicCharttransHisService();
            hicJepsuService = new HicJepsuService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResultService = new HicResultService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            read_sysdate();
            Screen_Display();
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnDelete)
            {
                Data_Delete();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                string strSname = "";
                string strGjjong = "";
                string strTrList = "";
                string strRemark = "";

                FnWRTNO = Convert.ToInt32(SSList.ActiveSheet.Cells[e.Row, 0].Text);
                strSname = SSList.ActiveSheet.Cells[e.Row, 1].Text;
                strGjjong = SSList.ActiveSheet.Cells[e.Row, 2].Text;
                strTrList = SSList.ActiveSheet.Cells[e.Row, 11].Text;

                SSPainfo.ActiveSheet.Cells[0, 0].Text = strSname;
                SSPainfo.ActiveSheet.Cells[0, 1].Text = strGjjong;
                SSPainfo.ActiveSheet.Cells[0, 2].Text = VB.IIf(VB.Mid(strTrList, 1,1) == "Y", "▦", "").ToString();
                SSPainfo.ActiveSheet.Cells[0, 3].Text = VB.IIf(VB.Mid(strTrList, 7,1) == "Y", "◎", " ").ToString();
                SSPainfo.ActiveSheet.Cells[0, 4].Text = VB.IIf(VB.Mid(strTrList, 2,1) == "Y", 1, 0).ToString();
                SSPainfo.ActiveSheet.Cells[0, 5].Text = VB.IIf(VB.Mid(strTrList, 3,1) == "Y", 1, 0).ToString();
                SSPainfo.ActiveSheet.Cells[0, 6].Text = VB.IIf(VB.Mid(strTrList, 4,1) == "Y", 1, 0).ToString();
                SSPainfo.ActiveSheet.Cells[0, 7].Text = VB.IIf(VB.Mid(strTrList, 5,1) == "Y", 1, 0).ToString();
                SSPainfo.ActiveSheet.Cells[0, 8].Text = VB.IIf(VB.Mid(strTrList, 6,1) == "Y", 1, 0).ToString();
                SSPainfo.ActiveSheet.Cells[0, 9].Text = FnWRTNO.ToString();
                SSPainfo.ActiveSheet.Cells[0, 10].Text = strRemark;

                FstrTrList = strTrList;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                txtWrtNo.Focus();
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                long nWRTNO = 0;

                string strPtNo = "";
                string strSname = "";
                string strGjjong = "";
                string strIeMunjin = ""; //인터넷문진
                string strSpecial = ""; //특수
                string strAudio = ""; //청력
                string strEndo = ""; //내시경
                string strPaperMunjin = ""; //종이문진
                string strFirst = ""; //1차
                string strHea = ""; //종검
                string strMsg = "";
                string strTrList = "";
                string strRemark= "";
                string strMUNDATE = "";

                List<string> strCodes = new List<string>();
                List<string> sCodes = new List<string>();

                sCodes.Clear();
                sCodes.Add("J231");

                strCodes.Add ("1601','1634','1665','1701','1734','1765','1801','1865','1834','1117");

                //if (long.Parse(txtWrtNo.Text) == 0)
                //{
                //    return;
                //}

                if (e.KeyChar == 13)
                {
                    nWRTNO = Convert.ToInt32(txtWrtNo.Text);

                    if (nWRTNO.Trim().Length >= 0)
                    {
                        HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(nWRTNO);
                        if (item.IsNullOrEmpty())
                        {
                            {
                                MessageBox.Show( nWRTNO + " 접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        FnWRTNO = nWRTNO;
                        strSname = item.SNAME;
                        strGjjong = item.GJJONG;
                        strPtNo = item.PTNO;
                        if ( item.JONGGUMYN == "1" )
                        {
                            strHea = "Y";
                        }
                        else
                        {
                            strHea = "N";
                        }
                        if( item.JEPDATE != clsPublic.GstrSysDate)
                        {
                            strMsg = "접수일자가" + item.JEPDATE + "입니다.";
                            strMsg = strMsg + "접수번호가 정확합니까?";
                            if (ComFunc.MsgBoxQ(strMsg, "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        strIeMunjin = "N";
                        strSpecial = "N";
                        strAudio = "N";
                        strEndo = "N";
                        strPaperMunjin = "N";
                        strFirst = "N";

                        HIC_CHARTTRANS item2 = hicCharttransService.GetAllbyWrtno(nWRTNO);
                        FstrROWID = ""; FstrTrList = ""; strRemark = "";
                        if (!item2.IsNullOrEmpty())
                        {
                            FstrROWID = item2.ROWID;
                            FstrTrList = item2.TRLIST;
                            strRemark = item2.REMARK;

                            if (item2.RECVSABUN > 0)
                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                MessageBox.Show("계측에서 인수한 차트 입니다.","확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else if (item2.ENTSABUN > 0)
                            {
                                btnSave.Enabled = false;
                                btnDelete.Enabled = false;
                                MessageBox.Show("입력이 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        //인터넷문진체크
                        if ( item.IEMUNNO >0)
                        {
                            strIeMunjin = "Y";
                        }
                        else
                        {
                            strMUNDATE = CF.DATE_ADD(clsDB.DbCon, cpublic.strSysDate, -150);
                            HIC_IE_MUNJIN_NEW item3 = hicIeMunjinNewService.GetItembyPtnoMundate(strPtNo, strMUNDATE);
                            if (!item3.IsNullOrEmpty())
                            {
                                strIeMunjin = "Y";
                            }
                        }

                        if (!item.UCODES.IsNullOrEmpty()) { strSpecial = "Y"; }

                        HIC_SUNAPDTL item4 = hicSunapdtlService.GetSunapDtlbyWrtNo(nWRTNO, sCodes);
                        if (!item4.IsNullOrEmpty()) { strAudio = "Y"; }

                        if (hicResultService.GetEndoCountbyWrtNoExCode(nWRTNO, "TX23") > 0) { strEndo = "Y"; }

                        switch (strGjjong)
                        {
                            case "51":
                                strPaperMunjin = "Y";
                                break;
                            //2차검진 (당뇨, 혈압만 있는지 점검)
                            case "16":
                            case "17":
                            case "18":
                            case "28":
                            case "29":
                                //HIC_SUNAPDTL item6 = hicSunapdtlService.GetCountbyWrtNoNotInCode(nWRTNO, strCodes);
                                if (hicSunapdtlService.GetCountbyWrtNoNotInCode(nWRTNO, strCodes) > 0)
                                {
                                    strPaperMunjin = "Y"; strFirst = "Y";
                                }
                                break;
                            case "44":
                            case "45":
                            case "46":
                            //성인병 2차검진 (당뇨, 혈압만 있는지 점검)
                               // HIC_SUNAPDTL item7 = hicSunapdtlService.GetCountbyWrtNoNotInCode(nWRTNO, strCodes);
                                if (hicSunapdtlService.GetCountbyWrtNoNotInCode(nWRTNO, strCodes) > 0)
                                {
                                    strPaperMunjin = "Y";
                                }
                                break;
                            case "21":
                                break;
                            default:
                                if (strIeMunjin == "N") { strPaperMunjin = "Y";}
                                break;
                        }

                        SSPainfo.ActiveSheet.Cells[0, 0].Text = strSname;
                        SSPainfo.ActiveSheet.Cells[0, 1].Text = strGjjong;
                        SSPainfo.ActiveSheet.Cells[0, 2].Text = VB.IIf(strIeMunjin == "Y", "▦", "").ToString();
                        SSPainfo.ActiveSheet.Cells[0, 3].Text = VB.IIf(strHea == "Y", "◎", " ").ToString();
                        SSPainfo.ActiveSheet.Cells[0, 4].Text = VB.IIf(strSpecial == "Y", 1, 0).ToString();
                        SSPainfo.ActiveSheet.Cells[0, 5].Text = VB.IIf(strAudio == "Y", 1, 0).ToString();
                        SSPainfo.ActiveSheet.Cells[0, 6].Text = VB.IIf(strEndo == "Y", 1, 0).ToString();
                        SSPainfo.ActiveSheet.Cells[0, 7].Text = VB.IIf(strPaperMunjin == "Y", 1, 0).ToString();
                        SSPainfo.ActiveSheet.Cells[0, 8].Text = VB.IIf(strFirst == "Y", 1, 0).ToString();
                        SSPainfo.ActiveSheet.Cells[0, 9].Text = FnWRTNO.ToString();
                        SSPainfo.ActiveSheet.Cells[0, 10].Text = strRemark;

                        //DB저장
                        strTrList = strIeMunjin + strSpecial + strAudio + strEndo + strPaperMunjin + strFirst + strHea;

                        ComFunc.ReadSysDate(clsDB.DbCon);
                        //SSList.ActiveSheet.RowCount = SSList.ActiveSheet.RowCount + 1;

                        SSList.ActiveSheet.AddRows(0, 1);
                        SSList.ActiveSheet.Cells[0, 0].Text = FnWRTNO.ToString();
                        SSList.ActiveSheet.Cells[0, 1].Text = strSname;
                        SSList.ActiveSheet.Cells[0, 2].Text = strGjjong;
                        SSList.ActiveSheet.Cells[0, 3].Text = clsPublic.GstrSysTime;
                        SSList.ActiveSheet.Cells[0, 4].Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, clsType.User.Sabun.ToString());
                        SSList.ActiveSheet.Cells[0, 5].Text = hb.GET_TrList_Name(strTrList);
                        SSList.ActiveSheet.Cells[0, 11].Text = strTrList;

                        //신규등록이면 자동으로 INSERT
                        if (FstrROWID == "")
                        {
                            btnSave.Enabled = true;
                            btnDelete.Enabled = false;

                            HIC_CHARTTRANS eChart = new HIC_CHARTTRANS
                            {
                                WRTNO = FnWRTNO,
                                TRDATE = DateTime.Today.ToString(),
                                SNAME = strSname,
                                GJJONG = strGjjong,
                                TRLIST = strTrList,
                                //ENTTIME = DateTime.Now.ToString(),
                                ENTTIME = clsPublic.GstrSysTime,
                                ENTSABUN = clsType.User.IdNumber.To<long>()
                            };
                            //INSERT
                            hicCharttransService.Insert(eChart);
                        }
                        else
                        {
                            btnSave.Enabled = true;
                            btnDelete.Enabled = true;
                            hicCharttransService.UpdatebyWrtno(FnWRTNO, clsPublic.GstrSysDate, strTrList, clsPublic.GstrSysTime, clsType.User.Sabun,"");
                        }
                        
                    }

                    FstrTrList = strTrList;
                    txtWrtNo.Text = "";

                }
            }
        }

        private void Data_Save()
        {
            string strIeMunjin = "";
            string strSpecial = "";
            string strAudio = "";
            string strEndo = "";
            string strPaperMunjin = "";
            string strFirst = "";
            string strHea = "";

            string strMsg = "";
            string strTrList = "";
            string strRemark = "";

            strIeMunjin = "N";
            strSpecial = "N";
            strAudio = "N";
            strEndo = "N";
            strPaperMunjin = "N";
            strFirst = "N";
            strHea = "N";


            if (SSPainfo.ActiveSheet.Cells[0, 2].Text == "▦") { strIeMunjin = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 3].Text == "◎") { strHea = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 4].Text == "1") { strSpecial = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 5].Text == "1") { strAudio = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 6].Text == "1") { strEndo = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 7].Text == "") { strPaperMunjin = "Y"; }
            if (SSPainfo.ActiveSheet.Cells[0, 8].Text == "1") { strFirst = "Y"; }
            FnWRTNO = Convert.ToInt32(SSPainfo.ActiveSheet.Cells[0, 9].Text);
            strRemark = SSPainfo.ActiveSheet.Cells[0, 10].Text;

            strTrList = strIeMunjin + strSpecial + strAudio + strEndo + strPaperMunjin + strFirst + strHea;


            HIC_CHARTTRANS_HIS eChart = new HIC_CHARTTRANS_HIS
            {
                WRTNO = FnWRTNO
            };

            //HIS_INSERT
            hicCharttransHisService.Insert(eChart);

            //UPDATE

            //DateTime.Today.ToString()
            hicCharttransService.UpdatebyWrtno(FnWRTNO, clsPublic.GstrSysDate, strTrList, DateTime.Now.ToString(), clsType.User.Sabun, strRemark);


            for ( int i =0; i < SSList.ActiveSheet.RowCount; i++)
            {
                if (SSList.ActiveSheet.Cells[i,0].Text == FnWRTNO.ToString())
                {
                    SSList.ActiveSheet.Cells[i, 5].Text = hb.GET_TrList_Name(strTrList);
                    SSList.ActiveSheet.Cells[i, 9].Text = strTrList;
                    SSList.ActiveSheet.Cells[i, 10].Text = strRemark;
                }
            }

            txtWrtNo.Focus();

        }

        private void Data_Delete()
        {
            if (ComFunc.MsgBoxQ("차트인계 내역을 삭제를 하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            HIC_CHARTTRANS_HIS eChart = new HIC_CHARTTRANS_HIS
            {
                WRTNO = FnWRTNO
            };

            //HIS_INSERT
            hicCharttransHisService.Insert(eChart);

            hicCharttransHisService.InsertDel(eChart);

            hicCharttransService.DeleteData(FnWRTNO);

            //for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            //{
            //    if (SSList.ActiveSheet.Cells[i, 0].Text == FnWRTNO.ToString())
            //    {
            //        //SList.DeleteRow(i);
            //    }
            //}

            Screen_Display();
            txtWrtNo.Focus();

        }

        private void Screen_Display()
        {
            string strTrList = "";

            txtWrtNo.Text = "";
            SSList.ActiveSheet.RowCount = 0;

            List<HIC_CHARTTRANS> list = hicCharttransService.GetAllbyTrDate(cpublic.strSysDate);
            SSList.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                strTrList = list[i].TRLIST;
                SSList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();
                SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                SSList.ActiveSheet.Cells[i, 2].Text = list[i].GJJONG;
                SSList.ActiveSheet.Cells[i, 3].Text = list[i].ENTTIME;
                SSList.ActiveSheet.Cells[i, 4].Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list[i].ENTSABUN.ToString());
                SSList.ActiveSheet.Cells[i, 5].Text = " "+ hb.GET_TrList_Name(list[i].TRLIST);
                SSList.ActiveSheet.Cells[i, 7].Text = list[i].RECVTIME;
                SSList.ActiveSheet.Cells[i, 8].Text = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, list[i].RECVSABUN.ToString());
                SSList.ActiveSheet.Cells[i, 10].Text = list[i].REMARK;
                SSList.ActiveSheet.Cells[i, 11].Text = list[i].TRLIST;
            }

            //txtWrtNo.Focus();
            this.ActiveControl = txtWrtNo;
        }
    }
}