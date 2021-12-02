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
/// Class Name      : ComHpcLibB
/// File Name       : frmHcChestXrayCheckcs.cs
/// Description     : 흉부촬영 점검
/// Author          : 이상훈
/// Create Date     : 2019-09-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm흉부촬영점검.frm(Frm흉부촬영점검)" />

namespace ComHpcLibB
{
    public partial class frmHcChestXrayCheckcs : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HicJepsuService hicJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;
        XrayResultnewService xrayResultnewService = null;
        XrayDetailService xrayDetailService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHcOrderSend Hord = new clsHcOrderSend();

        public frmHcChestXrayCheckcs()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            hicXrayResultService = new HicXrayResultService();
            hicJepsuService = new HicJepsuService();
            comHpcLibBService = new ComHpcLibBService();
            xrayResultnewService = new XrayResultnewService();
            xrayDetailService = new XrayDetailService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string strRoom = "";
            string strRoom1 = "";
            string strRoom2 = "";
            string strRoom3 = "";
            string strRoom4 = "";
            string strNextRoom = "";
            string strRemark = "";
            int nWait = 0;
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            string strGjJong = "";
            long nWRTNO = 0;
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                bool bOK = false;
                string strPtNo = "";
                string strXrayno = "";
                long nPano = 0;
                string strGbPacs = "";
                string strGubun = "";

                Cursor.Current = Cursors.WaitCursor;

                if (rdoChul1.Checked == true)
                {
                    strGubun = "";
                }
                else if (rdoChul2.Checked == true)
                {
                    strGubun = "1";
                }
                else if (rdoChul3.Checked == true)
                {
                    strGubun = "2";
                }

                clsDB.setBeginTran(clsDB.DbCon);

                List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetItembyJepDate(dtpFrDate.Text, dtpToDate.Text, strGubun);

                nREAD = list.Count;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    nPano = list[i].PANO;
                    strPtNo = list[i].PTNO;
                    strXrayno = list[i].XRAYNO;
                    strGbPacs = "";

                    //촬영번호가 없으면 삭제 Flag를 점검함
                    if (strXrayno.IsNullOrEmpty())
                    {
                        HIC_XRAY_RESULT lst = hicXrayResultService.GetItembyPaNo(nPano, list[i].JEPDATE, "");

                        if (!lst.IsNullOrEmpty())
                        {
                            strXrayno = lst.XRAYNO;
                            if (!strXrayno.IsNullOrEmpty())
                            {
                                result = hicJepsuService.UpdateXrayNobyWrtNo(strXrayno, list[i].WRTNO);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("촬영번호 갱신시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                int result1 = hicJepsuService.UpdateXrayResultbyWrtNo(nPano, strXrayno);

                                if (result1 < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("촬영번호 갱신시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    //PACS의 취소 Flag를 '0'으로 설정
                    if (!strXrayno.IsNullOrEmpty())
                    {
                        COMHPC list3 = comHpcLibBService.GetCancerFlagbyPatIdSerialNo(strPtNo, strXrayno);

                        if (list3.IsNullOrEmpty())
                        {
                            strGbPacs = "N";
                            //PACS에 오더를 전송함
                            if (comHpcLibBService.GetPacsOrderCountbyPatIdAcdessionNoExamDate(strPtNo, strXrayno, VB.Left(strXrayno, 8)) == 0)  //중복전송 방지
                            {
                                Hord.HIC_PACS_SEND(nPano, strXrayno, "NW", strPtNo);
                            }
                        }
                        else
                        {
                            if (list3.CANCELFLAG == "1")
                            {
                                result = comHpcLibBService.UpDatePacsDOrderCancelFlagbyRowId("0", list3.ROWID);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("PacsDOrder 갱신시 오류발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    HIC_XRAY_RESULT list2 = hicXrayResultService.GetItembyPaNo(nPano, list[i].JEPDATE, strXrayno);

                    if (!list2.IsNullOrEmpty())
                    {
                        bOK = true;
                        if (rdoJob2.Checked == true)    //미촬영
                        {
                            bOK = false;
                            if (list2.GBPACS != "Y")
                            {
                                bOK = true;
                            }

                            if (list2.READDOCT1 > 0)
                            {
                                bOK = false;
                            }

                            if (list2.DELDATE != null)
                            {
                                bOK = true;
                            }

                            if (bOK == true)
                            {
                                if (xrayDetailService.GetCountbyPtNo(strPtNo, list[i].JEPDATE) > 0)
                                {
                                    bOK = false;
                                }
                            }

                            if (bOK == true)
                            {
                                strGbPacs = "";
                                if (comHpcLibBService.GetFlagbyXrayNo(strXrayno) > 0)
                                {
                                    strGbPacs = "N";
                                }
                            }
                        }
                        else if (rdoJob3.Checked == true)   //미판독
                        {
                            bOK = false;
                            if (list2.READDOCT1 == 0)
                            {
                                bOK = true;
                            }

                            if (bOK == true)
                            {
                                if (list2.GBPACS != "Y")
                                {
                                    bOK = false;
                                }
                            }
                        }

                        if (bOK == true)
                        {
                            nRow += 1;
                            if (nRow > SS1.ActiveSheet.RowCount)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].WRTNO.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GJJONG;
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].LTDCODE.To<string>();
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].PTNO;
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = " " + list[i].XRAYNO;
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].GBCHUL;
                            SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].JONGGUMYN;
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = "";

                            if (strGbPacs == "N")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = "★";
                            }

                            SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";

                            if (!list2.IsNullOrEmpty())
                            {
                                if (list2.GBPACS == "Y")
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "▦";
                                }

                                if (list2.READDOCT1 > 0)
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "◎";
                                }
                            }
                        }
                    }
                }

                Cursor.Current = Cursors.Default;

                SS1.ActiveSheet.RowCount = nRow;

                clsDB.setCommitTran(clsDB.DbCon);
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "흉부촬영 점검";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간:" + dtpFrDate.Text + " ~ " + dtpToDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

                ///TODO : 이상훈(2019.08.26) 출력 Log 여부 확인
                //SQL_LOG("", SS1.PrintHeader);
            }
        }
    }
}
