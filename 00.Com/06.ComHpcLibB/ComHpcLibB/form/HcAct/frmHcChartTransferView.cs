using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcChartTransferView.cs
/// Description     : 차트인계 조회
/// Author          : 이상훈
/// Create Date     : 2020-09-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm차트인계조회.frm(Frm차트인계조회)" />

namespace ComHpcLibB
{
    public partial class frmHcChartTransferView : Form
    {
        List<HIC_BCODE> lstJindan = new List<HIC_BCODE>();    //진단서구분

        HicCharttransService hicCharttransService = null;
        HicJepsuService hicJepsuService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResultService hicResultService = null;
        HicConsentService hicConsentService = null;
        HicBcodeService hicBcodeService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();


        long FnWrtNo;
        string FstrROWID;
        string FstrTrList;
        bool boolSort = false;

        public frmHcChartTransferView()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCharttransService = new HicCharttransService();
            hicJepsuService = new HicJepsuService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResultService = new HicResultService();
            hicConsentService = new HicConsentService();
            hicBcodeService = new HicBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.txtWrtNo.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;
            txtSName.Text = "";
            txtWrtNo.Text = "";

            lstJindan = hicBcodeService.GetCodebyGubun("HIC_진단서구분등록");

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
                int nREAD = 0;
                int nRow = 0;
                int nCNT1 = 0;
                int nCNT2 = 0;
                string strTrList = "";
                long nWRTNO = 0;
                bool bOK = false;
                string strSname = "";
                string strGjJong = "";
                string str인터넷문진 = "";
                string str특수 = "";
                string str청력 = "";
                string str내시경 = "";
                string str종이문진표 = "";
                string str1차 = "";
                string str종검 = "";
                string strROWID = "";
                string strRemark = "";
                string str인계무관 = "";
                string strTrListName = "";
                string strBDate = "";
                string strCHK1 = "";
                string strJEPDATE = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strOut = "";
                string strAmPm = "";
                string strNoTrans = "";

                Cursor.Current = Cursors.WaitCursor;

                sp.Spread_All_Clear(SS1);

                Application.DoEvents();

                strBDate = VB.Left(dtpFDate.Text, 4) + "-01-01";

                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;

                if (rdoJob0.Checked == true)
                {
                    strJob = "0";
                }
                else if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }
                else if (rdoJob3.Checked == true)
                {
                    strJob = "3";
                }

                if (rdoOut0.Checked == true)
                {
                    strOut = "0";
                }
                else if (rdoOut1.Checked == true)
                {
                    strOut = "1";
                }

                if (rdoAmPm0.Checked == true)
                {
                    strAmPm = "0";
                }
                else if (rdoAmPm1.Checked == true)
                {
                    strAmPm = "1";
                }

                if (chkNoTrans.Checked == true)
                {
                    strNoTrans = "1";
                }

                if (chkTrans.Checked == true)
                {
                    strNoTrans = "2";
                }

                strSname = txtSName.Text;
                nWRTNO = txtWrtNo.Text.To<long>();
                sp.SetfpsRowHeight(SS1, 22);

                //전체 또는 인계누락
                if (rdoJob0.Checked == true || rdoJob2.Checked == true)
                {
                    List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateTrDate(strFrDate, strToDate, strSname, nWRTNO, strOut, strAmPm, strNoTrans, strJob);

                    nREAD = list.Count;
                    SS1.ActiveSheet.RowCount = nREAD;
                    nRow = 0;
                    for (int i = 0; i < nREAD; i++)
                    {
                        str인터넷문진 = "N";
                        str특수 = "N";
                        str청력 = "N";
                        str내시경 = "N";
                        str종이문진표 = "N";
                        str1차 = "N";
                        strCHK1 = "";

                        nWRTNO = list[i].WRTNO;
                        strSname = list[i].SNAME;
                        strGjJong = list[i].GJJONG;
                        strJEPDATE = list[i].JEPDATE;
                        if (list[i].JONGGUMYN == "1")
                        {
                            str종검 = "Y";
                        }
                        else
                        {
                            str종검 = "N";
                        }

                        //전체
                        strROWID = "";
                        strRemark = "";

                        //기존 차트 인계,인수내역을 읽음
                        HIC_CHARTTRANS list2 = hicCharttransService.GetAllbyWrtno(nWRTNO);

                        strROWID = "";
                        strRemark = "";

                        if (!list2.IsNullOrEmpty())
                        {
                            strROWID = list2.ROWID;
                            strTrList = list2.TRLIST;
                            strRemark = list2.REMARK;
                            if (!strRemark.IsNullOrEmpty())
                            {
                                strRemark += "(" + list2.REMARKTIME + ")";
                            }
                        }

                        //자료가 있으면 기존의 내용을 표시함
                        if (!strROWID.IsNullOrEmpty())
                        {
                            if (VB.Mid(strTrList, 1, 1) == "Y") { str인터넷문진 = "Y"; }
                            if (VB.Mid(strTrList, 2, 1) == "Y") { str특수 = "Y"; }
                            if (VB.Mid(strTrList, 3, 1) == "Y") { str청력 = "Y"; }
                            if (VB.Mid(strTrList, 4, 1) == "Y") { str내시경 = "Y"; }
                            if (VB.Mid(strTrList, 5, 1) == "Y") { str종이문진표 = "Y"; }
                            if (VB.Mid(strTrList, 6, 1) == "Y") { str1차 = "Y"; }
                            if (VB.Mid(strTrList, 7, 1) == "Y") { str종검 = "Y"; }
                        }
                        else
                        {
                            //신규는 인계할 내용을 찾음
                            if (list[i].IEMUNNO > 0)
                            {
                                str인터넷문진 = "Y";
                            }
                            else
                            {
                                if (hicIeMunjinNewService.GetCountbyPtNo(list[i].PTNO) > 0)
                                {
                                    str인터넷문진 = "Y";
                                }
                            }
                            if (!list[i].UCODES.IsNullOrEmpty())
                            {
                                str특수 = "Y";
                            }

                            //순음청력이 있는지 점검
                            if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "J231") > 0)
                            {
                                str청력 = "Y";
                            }

                            //내시경이 있는지 점검
                            if (str종검 == "N")
                            {
                                if (hicResultService.GetCountbyWrtNoCode(nWRTNO, "TX23") > 0)
                                {
                                    str내시경 = "Y";
                                }
                            }

                            //종이문진표가 필요한지 점검
                            switch (strGjJong)
                            {
                                case "51":  //방사선종사자
                                    str종이문진표 = "Y";
                                    break;
                                case "16":  //2차검진
                                case "17":
                                case "18":
                                case "28":
                                case "29":
                                case "44":
                                case "45":
                                case "46":
                                    //당뇨,혈압만 있는지 점검
                                    string[] strCode = { "1601", "1634", "1665", "1701", "1734", "1765", "1801", "1865", "1834", "1117" };
                                    if (hicSunapdtlService.GetCountbyWrtNoInCode(nWRTNO, strCode) > 0)
                                    {
                                        str종이문진표 = "Y";
                                        str1차 = "Y";
                                    }
                                    break;
                                case "32":  //건강진단서
                                    //일부 항목만 문진표 있음
                                    string[] strCode1 = { "9503", "9542", "9504" };
                                    if (hicSunapdtlService.GetCountbyWrtNoInCode(nWRTNO, strCode1) > 0)
                                    {
                                        str종이문진표 = "Y";
                                    }
                                    break;
                                case "69":  //회사추가검진
                                    if (hicJepsuService.GetCountbyPtNoGjJongJepDate(list[i].PTNO, "69", strJEPDATE) == 0)
                                    {
                                        str종이문진표 = "Y";
                                    }
                                    break;
                                case "55":
                                case "57":
                                case "58":
                                case "61":
                                case "62":
                                case "63":
                                case "70":
                                    break;
                                default:
                                    if (str인터넷문진 == "N")
                                    {
                                        str종이문진표 = "Y";
                                    }
                                    break;
                            }
                        }

                        //인계무관 여부를 설정
                        str인계무관 = "Y";
                        if (str특수 == "Y") { str인계무관 = "N"; }
                        if (str청력 == "Y") { str인계무관 = "N"; }
                        //if (str내시경 == "Y") { str인계무관 = "N"; }
                        if(chkNoTrans.Checked == true)
                        {
                            if (str종이문진표 == "Y") { str인계무관 = "N"; }
                        }
                        if (str1차 == "Y") { str인계무관 = "N"; }

                        if (str종검 == "Y" && str인계무관 == "Y")
                        {
                            if (hicIeMunjinNewService.GetCountbyPtNoGjJong(list[i].PTNO, strGjJong) == 0)
                            {
                                str인계무관 = "N";
                            }
                        }

                        bOK = true;
                        if (chkNoTrans.Checked == true) //인계무관
                        {
                            //2020-04-27(내시경 동의서 전산화로 추가)
                            if (str내시경 == "Y")
                            {
                                if (hicConsentService.GetCountbyPtNoSDateFormCode(list[i].PTNO, strJEPDATE, "D10") > 0)
                                {
                                    str인계무관 = "Y";
                                }

                                if (str인계무관 == "Y" && str인터넷문진 == "Y")
                                {
                                    bOK = false;
                                }
                            }

                            if (str인계무관 == "Y" && strGjJong != "69")
                            {
                                bOK = false;
                            }


                            //2021-09-27
                            int nJepCount = 0;
                            nJepCount = hicJepsuService.GetJepsuCountbyPaNo(list[i].PANO, strJEPDATE);

                            if (list[i].GJJONG == "32")
                            {
                                if (nJepCount == 1)
                                {
                                    for (int j = 0; j < lstJindan.Count; j++)
                                    {
                                        if (list[i].SEXAMS.Trim().Contains(lstJindan[j].CODE.To<string>("").Trim()))
                                        {
                                            bOK = true; 
                                        }
                                    }
                                }
                            }
                            if (list[i].GJJONG == "69")
                            {
                                if (nJepCount == 1) { bOK = true; }
                            }

                            if(list[i].GJJONG == "11")
                            {
                                if(list[i].UCODES.IsNullOrEmpty()) { bOK = false; }
                            }
                            if (list[i].GJJONG == "31")
                            {
                                bOK = false; 
                            }
                        }

                        if (chkTrans.Checked == true)
                        {
                            if (str인계무관 =="N")
                            {
                                bOK = false;
                            }


                            //if (strRemark.IsNullOrEmpty())
                            //{
                                //bOK = false;
                            //}
                        }

                        if (strGjJong == "31")
                        {
                            List<HIC_JEPSU> listJepsu = hicJepsuService.GetCountbyPtNoJepDate(list[i].PTNO, strBDate, dtpTDate.Text);
                            if (listJepsu.Count > 1)
                            {
                                strCHK1 = "OK";
                            }
                        }

                        if(strGjJong =="21" || strGjJong =="51" || strGjJong == "69")
                        {
                            str인계무관 = "N";
                        }



                        if (bOK == true)
                        {
                            strTrList = str인터넷문진 + str특수 + str청력 + str내시경 + str종이문진표 + str1차 + str종검;

                            nRow += 1;
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].WRTNO.To<string>();
                            if (strCHK1 == "OK")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(0, 255, 255);
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].GBCHUL;
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                            if (str종검 == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "◎";
                            }
                            if (str인계무관 == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "◎";
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = " " + hb.GET_TrList_Name(strTrList);
                            if (!strROWID.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list2.ENTTIME;
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = hb.READ_HIC_InsaName(list2.ENTSABUN.To<string>()).Trim();
                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list2.RECVTIME;
                                SS1.ActiveSheet.Cells[nRow - 1, 11].Text = hb.READ_HIC_InsaName(list2.RECVSABUN.To<string>()).Trim();
                            }
                            if (strROWID.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 14].Text = "";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 12].Text = strRemark;
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = strROWID;
                                SS1.ActiveSheet.Cells[nRow - 1, 14].Text = strRemark;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 15].Text = strTrList;
                            if (!list2.IsNullOrEmpty())
                            {
                                if (!list2.ENTTIME.IsNullOrEmpty())
                                {
                                    nCNT1 += 1;
                                }
                            }
                            if (!list2.IsNullOrEmpty())
                            {
                                if (!list2.RECVTIME.IsNullOrEmpty())
                                {
                                    nCNT2 += 1;
                                }
                            }
                        }
                    }

                    nRow += 1;
                    SS1.ActiveSheet.RowCount = nRow;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "인계건수";
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = nCNT1 + "건";
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "인수건수";
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = nCNT2 + "건";
                }
                else
                {
                    List<HIC_CHARTTRANS> list = hicCharttransService.GetItembyTrDate(strFrDate, strToDate, strSname, nWRTNO, strOut, strAmPm, strNoTrans, strJob);

                    nREAD = list.Count;
                    SS1.ActiveSheet.RowCount = nREAD;
                    nRow = 0;
                    for (int i = 0; i < nREAD; i++)
                    {
                        strTrList = list[i].TRLIST;
                        strTrListName = hb.GET_TrList_Name(strTrList);

                        //인계무관 여부를 설정
                        str인계무관 = "N";
                        if (strTrListName.IsNullOrEmpty())
                        {
                            str인계무관 = "Y";
                        }

                        bOK = true;
                        if (chkNoTrans.Checked == true)
                        {
                            if (str인계무관 == "Y")
                            {
                                bOK = false;
                            }
                        }

                        if (chkTrans.Checked == true)
                        {
                            if(list[i].REMARK.IsNullOrEmpty())
                            {
                                bOK = false;
                            }
                        }
                        

                        if (bOK == true)
                        {
                            nRow += 1;
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].WRTNO.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].GBCHUL;
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                            if (VB.Mid(strTrList, 7, 1) == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "◎";
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "";
                            if (str인계무관 == "Y")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "◎";
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = " " + hb.GET_TrList_Name(strTrList);
                            SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].ENTTIME;
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = hb.READ_HIC_InsaName(list[i].ENTSABUN.To<string>()).Trim();
                            SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].RECVTIME;
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = hb.READ_HIC_InsaName(list[i].RECVSABUN.To<string>()).Trim();
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].REMARK;
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].ROWID;
                            SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].REMARK;
                            SS1.ActiveSheet.Cells[nRow - 1, 15].Text = strTrList;
                            if (!list[i].ENTTIME.IsNullOrEmpty())
                            {
                                nCNT1 += 1;
                            }
                            if (!list[i].RECVTIME.IsNullOrEmpty())
                            {
                                nCNT2 += 1;
                            }
                        }
                    }

                    nRow += 1;
                    SS1.ActiveSheet.RowCount = nRow;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "인계건수";
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = nCNT1 + "건";
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "인수건수";
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = nCNT2 + "건";
                }
                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "차트 인계 대장";

                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text + VB.Space(70) + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String("/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSave)
            {
                string strROWID = "";
                string strRemark = "";
                string strOLD_Remark = "";
                string strTrList = "";
                string strTrDate = "";
                long nWRTNO = 0;
                string strSname = "";
                string strGjJong = "";
                string strEntTime = "";
                string strRecvTime = "";
                int result = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strRemark = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                    strOLD_Remark = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    if (strRemark != strOLD_Remark)
                    {
                        strTrDate = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                        nWRTNO = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                        strSname = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        strGjJong = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        strEntTime = SS1.ActiveSheet.Cells[i, 8].Text.Trim();
                        strRecvTime = SS1.ActiveSheet.Cells[i, 10].Text.Trim();
                        strROWID = SS1.ActiveSheet.Cells[i, 13].Text.Trim();
                        strTrList = SS1.ActiveSheet.Cells[i, 15].Text.Trim();

                        if (!strROWID.IsNullOrEmpty())
                        {
                            if (strRemark.IsNullOrEmpty() && strEntTime.IsNullOrEmpty() && strRecvTime.IsNullOrEmpty())
                            {
                                clsDB.setBeginTran(clsDB.DbCon);

                                result = hicCharttransService.DeletebyRowId(strROWID);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    return;
                                }
                                clsDB.setCommitTran(clsDB.DbCon);

                                SS1.ActiveSheet.Cells[i, 13].Text = "";
                                SS1.ActiveSheet.Cells[i, 14].Text = "";
                            }
                            else
                            {
                                clsDB.setBeginTran(clsDB.DbCon);
                                if (!strRemark.IsNullOrEmpty())
                                {
                                    result = hicCharttransService.UpdatebyRowId(strROWID, "Y");
                                }
                                else
                                {
                                    result = hicCharttransService.UpdatebyRowId(strROWID, "");
                                }

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    return;
                                }
                                clsDB.setCommitTran(clsDB.DbCon);
                                SS1.ActiveSheet.Cells[i, 14].Text = strRemark;
                            }                        
                        }
                        else
                        {
                            HIC_CHARTTRANS item = new HIC_CHARTTRANS();

                            item.TRDATE = strTrDate;
                            item.WRTNO = nWRTNO;
                            item.SNAME = strSname;
                            item.GJJONG = strGjJong;
                            item.TRLIST = strTrList;
                            item.REMARK = strRemark;

                            clsDB.setBeginTran(clsDB.DbCon);
                            if (!strRemark.IsNullOrEmpty())
                            {
                                item.REMARKTIME = "SYSTIME";
                                result = hicCharttransService.InsertAll(item);
                            }
                            else
                            {
                                item.REMARKTIME = "";
                                result = hicCharttransService.InsertAll(item);
                            }

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            clsDB.setCommitTran(clsDB.DbCon);

                            strROWID = hicCharttransService.GetRowIdbyWrtNo(nWRTNO);

                            SS1.ActiveSheet.Cells[i, 13].Text = strROWID;
                            SS1.ActiveSheet.Cells[i, 14].Text = strRemark;
                        }
                    }
                }
                MessageBox.Show("저장 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(SS1, e.Column, ref boolSort, true);
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            long nWrtNo = 0;

            if (sender == txtWrtNo)
            {
                if (e.KeyChar == 13)
                {
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            txtWrtNo.SelectAll();
        }
    }
}
