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
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcHemsListDrawup.cs
/// Description     : HEMS용 명단작성
/// Author          : 이상훈
/// Create Date     : 2021-02-09
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm명단작성.frm(FrmHEMS_명단작성)" />

namespace HC_Bill
{
    public partial class frmHcHemsListDrawup : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicJepsuService hicJepsuService = null;
        HicSpcPanhisService hicSpcPanhisService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string FstrViewJob;
        string FstrCommit;

        public frmHcHemsListDrawup()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicJepsuService = new HicJepsuService();
            hicSpcPanhisService = new HicSpcPanhisService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMake.Click += new EventHandler(eBtnClick);

            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            //this.cboYear.DropDownClosed += new EventHandler(eCboDropDownClosed);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboChanged);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS3);

            txtCnt.Text = "";

            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<int>() + 1;

            for (int i = 1; i <= 3; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 1;

            dtpFDate.Text = VB.Left(cboYear.Text, 4) + "-01-01";
            dtpFDate.Text = VB.Left(cboYear.Text, 4) + "-12-31";

            txtLtdCode.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strMinDate = "";
                string strMaxDate = "";
                string strOldData = "";
                string strNewData = "";
                string strDate = "";
                string strDate_2 = "";
                string strYear = "";
                string strOHMS = "";
                string str수첩 = "";
                int nPan_R = 0;
                int nPan_Not = 0;
                long nWRTNO = 0;
                long nWRTNO_2 = 0;
                int nCnt1 = 0;
                int nCNT2 = 0;
                int nCnt3 = 0;
                int nLtdCNT = 0;
                int nInwon1 = 0;
                int nInwon2 = 0;
                string strGubun = "";   //배치전구분
                string strFrDate = "";
                string strToDate = "";

                string strRdoChk = "";
                string strRdoBook = "";
                long nLtdCode = 0;

                strFrDate = dtpFDate.Text;
                strToDate = dtpTDate.Text;
                strYear = cboYear.Text;

                if (rdoChk0.Checked == true)
                {
                    strRdoChk = "0";
                }
                else if (rdoChk0.Checked == true)
                {
                    strRdoChk = "1";
                }
                else if (rdoChk1.Checked == true)
                {
                    strRdoChk = "2";
                }

                if (rdoBook0.Checked == true)
                {
                    strRdoBook = "0";
                }
                else if (rdoBook1.Checked == true)
                {
                    strRdoBook = "1";
                }

                if (!txtLtdCode.Text.IsNullOrEmpty() && txtLtdCode.Text.IndexOf(".") > 0)
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }
                else
                {
                    nLtdCode = 0;
                }

                sp.Spread_All_Clear(SS1);
                sp.Spread_All_Clear(SS3);

                strOldData = ""; strNewData = "";

                nLtdCNT = 0;

                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItembyJepDateGjYear(strFrDate, strToDate, strRdoChk, strRdoBook, strYear, nLtdCode, "");

                nRow = 0;
                nREAD = list.Count;

                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list[i].WRTNO;
                    strYear = list[i].GJYEAR;
                    strDate = list[i].JEPDATE;
                    strNewData = list[i].LTDCODE.To<string>();
                    strOHMS = list[i].GBGUKGO;
                    str수첩 = list[i].SUCHUPYN;

                    if (str수첩.IsNullOrEmpty())
                    {
                        str수첩 = "N";
                    }

                    if (strOldData.IsNullOrEmpty())
                    {
                        strOldData = strNewData;
                    }

                    if (strOldData != strNewData)
                    {
                        //GoSub CmdView_OneLtd
                        nLtdCNT += 1;

                        nRow += 1;
                        if (nRow > SS1.ActiveSheet.RowCount)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = strOldData;
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = " " + hb.READ_Ltd_Name(strOldData);
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = nCnt1.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = strMinDate;
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = strMaxDate;
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nCNT2.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nCnt3.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "";          //남인원
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = "";          //여인원
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";         //작업자
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = "";         //작업일시
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = strYear;    //검진년도
                        SS1.ActiveSheet.Cells[nRow - 1, 13].Text = strOHMS;    //비용지원
                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = str수첩;    //수첩소지여부
                        if (rdoChk2.Checked == true)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 15].Text = "Y";    //배치전 구분
                        }

                        //남여 총근로자수
                        COMHPC list2 = comHpcLibBService.GetLtdInwon2byLtdCodeYear(strOldData, strYear);

                        nInwon1 = 0;
                        nInwon2 = 0;
                        if (!list2.IsNullOrEmpty())
                        {
                            nInwon1 = list2.INWON011;
                            nInwon2 = list2.INWON012;
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = nInwon1.To<string>(); //남인원
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = nInwon2.To<string>(); //여인원

                        //미판정이 있거나 2차재검자만 있을 경우
                        if (nCnt3 > 0 || (nCNT2 == nCnt1))
                        {
                            SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
                        }

                        strOldData = strNewData;
                        nCnt1 = 0;  nCNT2 = 0;  nCnt3 = 0;
                        strMinDate = ""; strMaxDate = "";
                    }

                    //검진시작일/종료일
                    if (strMinDate.IsNullOrEmpty() || strMinDate.To<DateTime>() > strDate.To<DateTime>())
                    {
                        strMinDate = strDate;
                    }

                    if (strMaxDate.IsNullOrEmpty() || strMaxDate.To<DateTime>() < strDate.To<DateTime>())
                    {
                        strMaxDate = strDate;
                    }

                    //2차 접수번호를 읽음
                    HIC_JEPSU_RES_SPECIAL list3 = hicJepsuResSpecialService.GetItembyJepDatePaNoGjYear(strDate, list[i].PANO, cboYear.Text);

                    nWRTNO_2 = 0;
                    strDate_2 = "";

                    if (!list3.IsNullOrEmpty())
                    {
                        nWRTNO_2 = list3.WRTNO;
                        strDate_2 = list3.JEPDATE;
                    }

                    //검진종료일
                    if (!strDate_2.IsNullOrEmpty())
                    {
                        if (strMaxDate.IsNullOrEmpty() || strMaxDate.To<DateTime>() < strDate_2.To<DateTime>())
                        {
                            strMaxDate = strDate_2;
                        }
                    }

                    nCnt1 += 1; //건수

                    //1차,2차의 R판정,미판정 건수를 읽음
                    List<HIC_SPC_PANJENG> list4 = hicSpcPanjengService.GetPanjengbyWrtNoMCodeNo(nWRTNO);

                    nPan_R = 0;
                    nPan_Not = 0;
                    for (int j = 0; j < list4.Count; j++)
                    {
                        switch (list4[j].PANJENG)
                        {
                            case "":
                                nPan_Not += 1;
                                break;
                            case "7":
                                nPan_R += 1;    //R판정
                                break;
                            default:
                                break;
                        }
                    }

                    if (nPan_R > 0)
                    {
                        nCNT2 += 1; //2차미실시 (미실시 있더라도 명단작성가능)
                    }

                    if (nPan_Not > 0)
                    {
                        nCnt3 += 1; //미판정
                    }

                    btnMake.Enabled = true;

                    progressBar1.Value = i + 1;

                    if (txtCnt.Text.To<long>() > 0 && nLtdCNT >= txtCnt.Text.To<long>())
                    {
                        break;
                    }
                }

                progressBar1.Value = 0;

                if (txtCnt.Text.To<long>() == 0)
                {
                    //GoSub CmdView_OneLtd
                    nLtdCNT += 1;

                    nRow += 1;
                    if (nRow > SS1.ActiveSheet.RowCount)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = strOldData;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = " " + hb.READ_Ltd_Name(strOldData);
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = nCnt1.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 4].Text = strMinDate;
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = strMaxDate;
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = nCNT2.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = nCnt3.To<string>();
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = "";          //남인원
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = "";          //여인원
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = "";         //작업자
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = "";         //작업일시
                    SS1.ActiveSheet.Cells[nRow - 1, 12].Text = strYear;    //검진년도
                    SS1.ActiveSheet.Cells[nRow - 1, 13].Text = strOHMS;    //비용지원
                    SS1.ActiveSheet.Cells[nRow - 1, 14].Text = str수첩;    //수첩소지여부
                    if (rdoChk2.Checked == true)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = "Y";    //배치전 구분
                    }

                    //남여 총근로자수
                    COMHPC list2 = comHpcLibBService.GetLtdInwon2byLtdCodeYear(strOldData, strYear);

                    nInwon1 = 0;
                    nInwon2 = 0;
                    if (!list2.IsNullOrEmpty())
                    {
                        nInwon1 = list2.INWON011;
                        nInwon2 = list2.INWON012;
                    }

                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = nInwon1.To<string>(); //남인원
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = nInwon2.To<string>(); //여인원

                    //미판정이 있거나 2차재검자만 있을 경우
                    if (nCnt3 > 0 || (nCNT2 == nCnt1))
                    {
                        SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
                    }

                    strOldData = strNewData;
                    nCnt1 = 0; nCNT2 = 0; nCnt3 = 0;
                    strMinDate = ""; strMaxDate = "";
                }

                SS1.ActiveSheet.RowCount = nRow;
            }
            else if (sender == btnMake)
            {
                int nREAD = 0;
                long nMirno = 0;
                int nCnt1 = 0;
                int nCNT2 = 0;
                int nCnt3 = 0;
                string strChk = "";
                string strFrDate = "";
                string strToDate = "";
                string strLtdCode = "";
                string strYear = "";
                string strDate = "";
                string strMinDate = "";
                string strMaxDate = "";
                string strTemp = "";
                string strOHMS = "";
                string str수첩 = "";
                bool bOK = false;
                string strGubun1 = ""; //배치전구분
                string strRdoChk = "";
                string strRdoBook = "";
                long nLtdCode = 0;
                int result = 0;

                if (rdoChk0.Checked == true)
                {
                    strRdoChk = "0";
                }
                else if (rdoChk1.Checked == true)
                {
                    strRdoChk = "1";
                }
                else if (rdoChk2.Checked == true)
                {
                    strRdoChk = "2";
                }

                if (rdoBook0.Checked  == true)
                {
                    strRdoBook = "0";
                }
                else if (rdoBook1.Checked == true)
                {
                    strRdoBook = "1";
                }

                if (!txtLtdCode.Text.IsNullOrEmpty() && txtLtdCode.Text.IndexOf(".") > 0)
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }
                else
                {
                    nLtdCode = 0;
                }

                sp.Spread_All_Clear(SS3);

                clsDB.setBeginTran(clsDB.DbCon);

                for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                {
                    strChk = SS1.ActiveSheet.Cells[j, 0].Text;

                    strMinDate = ""; strMaxDate = "";
                    nCnt1 = 0; nCNT2 = 0; nCnt3 = 0;

                    strLtdCode = SS1.ActiveSheet.Cells[j, 1].Text;
                    strFrDate = SS1.ActiveSheet.Cells[j, 4].Text;
                    strToDate = SS1.ActiveSheet.Cells[j, 5].Text;
                    strYear = SS1.ActiveSheet.Cells[j, 12].Text;
                    strOHMS = SS1.ActiveSheet.Cells[j, 13].Text;
                    str수첩 = SS1.ActiveSheet.Cells[j, 14].Text;
                    strGubun1 = SS1.ActiveSheet.Cells[j, 15].Text;
                    //새로운 헴스 신규번호 생성
                    nMirno = fn_READ_New_SpcMirNo();

                    List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItembyJepDateGjYear(strFrDate, strToDate, strRdoChk, strRdoBook, strYear, nLtdCode, "");

                    bOK = true;
                    nREAD = list.Count;
                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        //2차 재검을 하지 않았으면 명단에서 제외함
                        if (hicSpcPanjengService.GetPanRbyWrtNo(list[i].WRTNO) > 0)
                        {
                            bOK = false;
                        }

                        if (bOK == true)
                        {
                            strTemp = "";
                            strDate = list[i].JEPDATE;

                            //검진시작일/종료일
                            if (strMinDate.IsNullOrEmpty() || strMinDate.To<DateTime>() > strDate.To<DateTime>())
                            {
                                strMinDate = strDate;
                            }

                            if (strMinDate.IsNullOrEmpty() || strMinDate.To<DateTime>() < strDate.To<DateTime>())
                            {
                                strMaxDate = strDate;
                            }

                            //접수마스타에 청구형성 완료 SET
                            result = hicJepsuService.UpdateHemsNobyWrtNo(list[i].WRTNO, nMirno);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("HIC_JEPSU에 HEMS번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            nCnt1 += 1;

                            //2차 접수번호를 읽음
                            HIC_JEPSU_RES_SPECIAL list2 = hicJepsuResSpecialService.GetWrtNoJepDatebyJepDatePaNoGjYear(strDate, list[i].PANO, cboYear.Text);

                            if (!list2.IsNullOrEmpty())
                            {
                                strTemp = "OK";
                                strDate = list2.JEPDATE;

                                //검진시작일/종료일
                                if (strMinDate.IsNullOrEmpty() || strMinDate.To<DateTime>() > strDate.To<DateTime>())
                                {
                                    strMinDate = strDate;
                                }

                                if (strMinDate.IsNullOrEmpty() || strMinDate.To<DateTime>() < strDate.To<DateTime>())
                                {
                                    strMaxDate = strDate;
                                }

                                //접수마스타에 청구형성 완료 SET
                                result = hicJepsuService.UpdateHemsNobyWrtNo(list2.WRTNO, nMirno);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_JEPSU에 HEMS번호 UPDATE시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                nCnt1 += 1;
                            }

                            if (nCnt1 > 99)
                            {
                                //GoSub Build_SPC_INSERT
                                FstrCommit = "NO";
                                fn_Build_SPC_INSERT(nMirno, strYear, nLtdCode, strMinDate, strMaxDate, nCnt1, clsType.User.IdNumber.To<long>(), nCNT2, strOHMS, str수첩, nCnt3, strGubun1);
                                if (FstrCommit == "NO")
                                {
                                    return;
                                }

                                strMinDate = "";
                                strMaxDate = "";
                                nMirno = fn_READ_New_SpcMirNo();
                                nCnt3 += 1;
                                nCnt1 = 0;
                            }
                        }
                        progressBar1.Value = i + 1;
                    }

                    if (nCnt1 > 0)
                    {
                        FstrCommit = "NO";
                        fn_Build_SPC_INSERT(nMirno, strYear, nLtdCode, strMinDate, strMaxDate, nCnt1, clsType.User.IdNumber.To<long>(), nCNT2, strOHMS, str수첩, nCnt3, strGubun1);
                        if (FstrCommit == "NO")
                        {
                            return;
                        }
                    }

                    SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
                }
                progressBar1.Value = 0;

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        /// <summary>
        /// 새로운 건강검진 비용청구서 INSERT
        /// </summary>
        void fn_Build_SPC_INSERT(long argMirNo, string argYear, long argLtdCode, string argMinDate, string argMaxDate, int argCnt1, long argSabun, int argCnt2, string argOHMS, string argBook, int argCnt3, string argGubun1)
        {            
            string strFDate = "";
            string strTDate = "";
            int result = 0;

            strFDate = clsPublic.GstrSysDate;
            strTDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();

            COMHPC list = comHpcLibBService.GetMirHemsSeqNobyBuildDate(strFDate, strTDate);

            if (!list.IsNullOrEmpty())
            {
                if (list.SEQNO > 99)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("하루에 100건 이상 작업할 수 없습니다.(공단연계문제)", "작업중지!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    progressBar1.Value = 0;
                    FstrCommit = "NO";
                    return;
                }
            }

            result = comHpcLibBService.InsertMirHems(argMirNo, argYear, argLtdCode, argMinDate, argMaxDate, argCnt1, argSabun, argCnt2, argOHMS, argBook, argCnt3, argGubun1);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("HIC_MIR_HEMS에 INSERT시 오류가 발생함", "확인!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                FstrCommit = "NO";
                return;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            string strName = "";

            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        if (txtLtdCode.Text.Trim().IndexOf(".") > 0)
                        {
                            strName = hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text.Trim(), ".", 1));
                        }
                        else
                        {
                            strName = hb.READ_Ltd_Name(txtLtdCode.Text.Trim());
                        }

                        if (strName.IsNullOrEmpty())
                        {
                            eBtnClick(btnLtdCode, new EventArgs());
                        }
                        else
                        {
                            txtLtdCode.Text += "." + strName;
                        }
                    }
                    SendKeys.Send("{Tab}");
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                int nRow = 0;
                long nREAD = 0;
                string strLtdCode = "";
                long nMirno = 0;
                string strFrDate = "";
                string strToDate = "";
                long nWRTNO = 0;

                string strRdoChk = "";
                string strRdoBook = "";
                long nLtdCode = 0;

                string strYear = "";

                if (e.RowHeader == true && e.Column == 0)
                {
                    SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "True";
                }

                strYear = cboYear.Text;

                if (rdoChk0.Checked == true)
                {
                    strRdoChk = "0";
                }
                else if (rdoChk1.Checked == true)
                {
                    strRdoChk = "1";
                }
                else if (rdoChk2.Checked == true)
                {
                    strRdoChk = "2";
                }

                if (rdoBook0.Checked == true)
                {
                    strRdoBook = "0";
                }
                else if (rdoBook1.Checked == true)
                {
                    strRdoBook = "1";
                }

                if (!txtLtdCode.Text.IsNullOrEmpty() && txtLtdCode.Text.IndexOf(".") > 0)
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }
                else
                {
                    nLtdCode = 0;
                }

                sp.Spread_All_Clear(SS3);

                strLtdCode = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                strFrDate = SS1.ActiveSheet.Cells[e.Row, 4].Text;
                strToDate = SS1.ActiveSheet.Cells[e.Row, 5].Text;

                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItembyJepDateGjYear(strFrDate, strToDate, strRdoChk, strRdoBook, strYear, nLtdCode, "SORT");

                nRow = 0;
                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > SS3.ActiveSheet.RowCount)
                    {
                        SS3.ActiveSheet.RowCount = nRow;
                    }

                    SS3.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;  //접수일자
                    SS3.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].WRTNO.To<string>();  //접수번호
                    SS3.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME;  //성명
                    SS3.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].AGE.To<string>();  //나이
                    SS3.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SS3.ActiveSheet.Cells[nRow - 1, 5].Text = "1";  //기본은 1차로

                    if (hicSpcPanhisService.GetCountbyWrtNo(list[i].WRTNO) > 0)
                    {
                        SS3.ActiveSheet.Cells[nRow - 1, 5].Text = "2";  //2차
                    }

                    if (hicSpcPanhisService.GetPanRbyWrtNo(list[i].WRTNO) > 0)
                    {
                        SS3.ActiveSheet.Cells[nRow - 1, 6].Text = "R"; 
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[nRow - 1, 6].Text = "";
                    }

                    SS3.ActiveSheet.Cells[nRow - 1, 7].Text = hm.SExam_Names_Display(list[i].SEXAMS);   //선택검사
                    SS3.ActiveSheet.Cells[nRow - 1, 8].Text = hm.UCode_Names_Display(list[i].UCODES);   //유해인자
                    SS3.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].ROWID;
                }
            }
        }

        //void eCboDropDownClosed(object sender, EventArgs e)
        //{
        //    dtpFDate.Text = cboYear.Text + "-01-01";
        //    dtpTDate.Text = cboYear.Text + "-12-31";
        //}

        void eCboChanged(object sender, EventArgs e)
        {
            dtpFDate.Text = cboYear.Text + "-01-01";
            dtpTDate.Text = cboYear.Text + "-12-31";
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        /// <summary>
        /// 특수건강진단 자료전송 청구번호 부여 생성
        /// </summary>
        /// <returns></returns>
        long fn_READ_New_SpcMirNo()
        {
            long rtnVal = 0;

            rtnVal = comHpcLibBService.GetSpcMirNobyDual();

            return rtnVal;
        }
    }
}
