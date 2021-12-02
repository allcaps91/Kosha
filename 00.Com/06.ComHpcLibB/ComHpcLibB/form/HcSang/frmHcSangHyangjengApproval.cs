using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSangHyangjengApproval.cs
/// Description     : 향정/마약 처방전 전송
/// Author          : 이상훈
/// Create Date     : 2020-01-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm향정처방승인.frm(Frm향정처방승인)" />

namespace ComHpcLibB
{
    public partial class frmHcSangHyangjengApproval : Form
    {
        HicHyangApproveService hicHyangApproveService = null;
        BasSunService basSunService = null;
        HicHyangService hicHyangService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcFunc hc = new clsHcFunc();
        clsOrdFunction OF = new clsOrdFunction();

        string FstrDate;

        public frmHcSangHyangjengApproval()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicHyangApproveService = new HicHyangApproveService();
            basSunService = new BasSunService();
            hicHyangService = new HicHyangService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnSendCancel.Click += new EventHandler(eBtnClick);
            this.btnMenuAllSelect.Click += new EventHandler(eBtnClick);
            this.btnMenuAllCancel.Click += new EventHandler(eBtnClick);

            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            hb.READ_HIC_Doctor(long.Parse(clsType.User.IdNumber));  //판정의사 여부를 읽음

            dtpDate.Text = clsPublic.GstrSysDate;
            FstrDate = "";
            eBtnClick(btnSearch, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnOK)
            {
                int nCNT = 0;
                string strDate = "";
                string strChk = "";
                string strSuCode = "";
                string strApproveTime = "";
                string strROWID = "";
                string strList = "";
                string strBDATE = "";
                string strGbSite = "";
                string strMSG = "";
                string strWRTNO = "";
                long nWRTNO = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    if (SS1.ActiveSheet.Cells[i, 12].Text.IsNullOrEmpty())
                    {
                        nCNT += 1;
                    }
                }

                if (nCNT > 0)
                {
                    if (fn_Magam_Check() == true)
                    {
                        MessageBox.Show("마감이 되어 처방전 전송/취소가 불가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //처방전 발행일에 의사가 휴일인지 점검
                if (hf.Check_Sabun_Huil(clsType.User.IdNumber, dtpDate.Text) == true)
                {
                    strMSG = strBDATE + "일은 근무일이 아님, 전송이 불가능합니다.";
                    ///TODO : 이상훈(2020.01.21) 왜 Question 하지?
                    ///전송불가 메시지 후 종료 해야 할듯한데...
                    //if (MessageBox.Show(strMSG, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    //{
                    //    return;
                    //}
                    MessageBox.Show(strMSG, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                strList = "{}";
                nCNT = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    if (strChk == "True" && !strSuCode.IsNullOrEmpty() && strApproveTime.IsNullOrEmpty())
                    {
                        strWRTNO = SS1.ActiveSheet.Cells[i, 2].Text;
                        if (VB.InStr(strList, "{}" + strWRTNO + "{}") == 0)
                        {
                            strList += strWRTNO + "{}";
                            nCNT += 1;
                        }
                    }
                }

                //접수번호별로 처방을 전달함
                for (int i = 1; i <= nCNT; i++)
                {
                    strWRTNO = VB.Pstr(strList, "{}", i + 1);
                    if (!strWRTNO.IsNullOrEmpty())
                    {
                        nWRTNO = strWRTNO.To<long>();
                        fn_HIC_HYANG_Approve(nWRTNO);
                    }
                }

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                string strSuCode = "";
                string strDrname = "";
                string strDeptCode = "";
                string strOK = "";
                string strBDate = "";
                string strJob = "";

                strBDate = dtpDate.Text;

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                //자료를 Select
                List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyBDateDeptCode(strBDate, strJob, "HR");

                nREAD = list.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                nRow = 0;

                for (int i = 0; i < nREAD; i++)
                {
                    strDeptCode = list[i].DEPTCODE;
                    strOK = "OK";
                    if (list[i].QTY == "0")
                    {
                        strOK = "";
                    }

                    if (strOK == "OK")
                    {
                        strSuCode = list[i].SUCODE.Trim();
                        //nRow += 1;
                        //if (nRow > SS1.ActiveSheet.RowCount)
                        //{
                        //    SS1.ActiveSheet.RowCount = nRow;
                        //}
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                        if (rdoJob1.Checked == true)
                        {
                            SS1.ActiveSheet.Cells[i, 0].Text = "True";  //미승인
                        }
                        SS1.ActiveSheet.Cells[i, 1].Text = list[i].BDATE.To<string>();
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                        SS1.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].AGE + "/" + list[i].SEX;
                        SS1.ActiveSheet.Cells[i, 5].Text = " " + strSuCode;
                        SS1.ActiveSheet.Cells[i, 6].Text = string.Format("{0:0.00}", list[i].QTY.Trim());
                        SS1.ActiveSheet.Cells[i, 8].Text = string.Format("{0:0.00}", list[i].REALQTY);

                        //투약용량은 RealQTY에 환산계수량으로 입력함.
                        BAS_SUN list2 = basSunService.GetSuNamekUnitbySuCode(strSuCode);

                        if (!list2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 7].Text = " " + list2.UNIT;
                            SS1.ActiveSheet.Cells[i, 9].Text = " " + list2.SUNAMEK;
                        }

                        SS1.ActiveSheet.Cells[i, 10].Text = list[i].PRINT;
                        SS1.ActiveSheet.Cells[i, 11].Text = "";
                        if (!list[i].OCSSENDTIME.ToString().IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 11].Text = "Y";
                        }

                        if (!list[i].OCSSENDTIME.ToString().IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = "";
                        }

                        if (list[i].GBSITE == "2")
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = "◎";
                        }
                        strDrname = hb.READ_Sabun_Name(list[i].DRSABUN).Trim();
                        if (strDrname.IsNullOrEmpty())
                        {
                            strDrname = list[i].DRSABUN;
                        }
                        SS1.ActiveSheet.Cells[i, 13].Text = strDrname;
                        SS1.ActiveSheet.Cells[i, 14].Text = list[i].APPROVETIME.To<string>();
                        SS1.ActiveSheet.Cells[i, 15].Text = list[i].RID;
                        SS1.ActiveSheet.Cells[i, 16].Text = list[i].PTNO;
                    }
                }

                if (rdoJob1.Checked == true)
                {
                    btnOK.Enabled = true;
                    btnSendCancel.Enabled = false;
                }
                else
                {
                    btnOK.Enabled = true;
                    btnSendCancel.Enabled = true;
                }

                //의사가 아니면 전송 불가
                if (clsHcVariable.GnHicLicense == 0)
                {
                    btnOK.Enabled = false;
                }
            }
            else if (sender == btnSendCancel)
            {

            }
            else if (sender == btnMenuAllSelect)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "True";
            }
            else if (sender == btnMenuAllCancel)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "";
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }

        void fn_HIC_HYANG_Approve(long argWrtNo)
        {
            int nREAD = 0;
            string strPTNO = "";
            string strBDATE = "";
            string strSuCode = "";
            string strORDERCODE = "";
            string strDeldate = "";
            string strROWID = "";
            double nQty = 0;
            double nOldQty = 0;
            string strSname = "";
            string strJuso = "";
            string strList = "";
            long nSeqNo = 0;
            string strGbSite = "";
            string strSex = "";
            long nAge = 0;
            string strDosCode = "";
            string strDrCode = "";
            string strSlipNo = "";
            long nWRTNO = 0;
            double nOrderno = 0;
            int result = 0;
            string strSysDate = "";
            string sRowId = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strSysDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";

            //승인한것 수가코드 목록을 만듬
            strList = "";
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 2].Text.To<long>() == argWrtNo)
                {
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strList += strSuCode + ",";
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //HIC_HYANG_Approve에 삭제된것 등록
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItemListbyWrtNo(argWrtNo, "HR");

            nREAD = list.Count;
            //SS1.ActiveSheet.RowCount = nREAD;
            if (nREAD == 0 && SS1.ActiveSheet.RowCount == 0)
            {
                clsDB.setCommitTran(clsDB.DbCon);
                return;
            }

            for (int i = 0; i < nREAD; i++)
            {
                strSuCode = list[i].SUCODE;
                if (!strSuCode.IsNullOrEmpty())
                {
                    strSuCode = strSuCode.Trim();
                }
                //승인한 수가코드 목록에 없으면 삭제함
                if (strList.IndexOf(strSuCode) < 0)
                {
                    strBDATE = list[i].BDATE;
                    strPTNO = list[i].PTNO;

                    result = hicHyangApproveService.UpdatebyRowId(list[i].RID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("삭제일자 update 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    result = hicHyangService.UpdatebyRowId(list[i].RID, argWrtNo, strSuCode);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("삭제일자 update 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (VB.Left(strSuCode, 2) == "N-")
                    {
                        result = comHpcLibBService.DeleteOcsMayak(strPTNO, strSuCode, strBDATE);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("마약 삭제 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        result = comHpcLibBService.DeleteOcsHyang(strPTNO, strSuCode, strBDATE);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("향정 삭제 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //OCS_OORDER 전송
                    result = comHpcLibBService.DeleteOcsOrder(strPTNO, strSuCode, strBDATE);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("처방 삭제 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            strJuso = hicJepsuService.GetJusobyWrtNo(argWrtNo);

            //승인한것 업데이트
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                nWRTNO = SS1.ActiveSheet.Cells[i, 2].Text.To<long>();
                strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                nQty = SS1.ActiveSheet.Cells[i, 6].Text.To<double>();

                if (nWRTNO == argWrtNo && !strSuCode.IsNullOrEmpty())
                {
                    HIC_HYANG_APPROVE list3 = hicHyangApproveService.GetItembyWrtNoSucode(argWrtNo, strSuCode);

                    if (!list3.IsNullOrEmpty())
                    {
                        strROWID = list3.RID;
                        strPTNO = list3.PTNO;
                        strSname = list3.SNAME;
                        strGbSite = list3.GBSITE;
                        strSex = list3.SEX;
                        nOldQty = list3.QTY.To<double>();
                        nAge = list3.AGE;
                        strBDATE = list3.BDATE;

                        //승인한 약품의 수량이 변경되었으면 History를 저장함
                        if (nQty != nOldQty && !list3.APPROVETIME.To<string>().IsNullOrEmpty())
                        {
                            result = hicHyangApproveService.InsertSelect(strROWID);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("승인약품 수량 History를 저장 중 오류발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        //HIC_HYANG_APPROVE Update                    
                        result = hicHyangApproveService.UpdateItembyRowId(nQty, nOldQty, clsType.User.IdNumber, strROWID, list3.APPROVETIME);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_HYANG_APPROVE Update중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //마약
                        //SEQNO를 읽음
                        nSeqNo = hicHyangService.GetSeqNobyBDate(strSysDate);

                        //HIC_HYANG Update
                        HIC_HYANG listHyang = hicHyangService.GetRowIdbyWrtNoSuCode(argWrtNo, strSuCode);

                        if (!listHyang.IsNullOrEmpty())
                        {
                            sRowId = listHyang.RID;
                        }
                        else
                        {
                            sRowId = "";
                        }

                        if (!sRowId.IsNullOrEmpty())
                        {
                            result = hicHyangService.UpdateQtybyRowId(nQty, sRowId);
                        }
                        else
                        {
                            HIC_HYANG item = new HIC_HYANG();

                            item.IO = "O";
                            item.NAL = 1;
                            item.DOSCODE = "920103";
                            item.REMARK1 = "검사용";
                            item.REMARK2 = "Pain";
                            item.SEQNO = nSeqNo;
                            item.ORDERNO = 0;
                            item.JUSO = strJuso;
                            item.RID = strROWID;
                            

                            result = hicHyangService.InsertSelectbyWorId(item);
                        }

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("향정약품 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //외래접수
                        sRowId = comHpcLibBService.GetRowIdOpdMasterbyPanoBDate(strPTNO, strBDATE);

                        if (sRowId.IsNullOrEmpty())
                        {
                            COMHPC item = new COMHPC();

                            item.PTNO = strPTNO;
                            item.DEPTCODE = "HR";
                            item.BI = "51";
                            item.SNAME = strSname;
                            item.SEX = strSex;
                            item.AGE = nAge;
                            item.JICODE = "";
                            item.DRCODE = "7101";
                            item.RESERVED = "0";
                            item.CHOJAE = "1";
                            item.GBGAMEK = "00";
                            item.GBSPC = "0";
                            item.JIN = "D";
                            item.SINGU = "0";
                            item.PART = "111";
                            item.BDATE = strBDATE;
                            item.EMR = "0";
                            item.GBUSE = "Y";
                            item.MKSJIN = "D";

                            result = comHpcLibBService.InsertOpdMaster(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("외래접수 생성 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        //OCS_OORDER 전송
                        COMHPC list4 = comHpcLibBService.GetRowIdQtyOcsOrderbyPtNoBDateSuCode(strPTNO, strBDATE, strSuCode,"HR");

                        if (list4.IsNullOrEmpty())
                        {
                            strDosCode = "920103";
                            //HD실 요청으로 용법코드를 920103을 사용함
                            strDrCode = hb.READ_HIC_OcsDrcode(clsType.User.IdNumber.To<long>());

                            //오더코드,SlipNo 설정
                            switch (strSuCode.Trim())
                            {
                                case "A-ANE12G":
                                    strORDERCODE = "A-ANE12G";
                                    strSlipNo = "0005";
                                    break;
                                case "A-PO12GA":
                                    strORDERCODE = "A-PO12GA";
                                    strSlipNo = "0005"; //ORDERCODE변경(2019-10-10)
                                    break;
                                case "A-BASCAM":
                                    strORDERCODE = "A-BASCAM";
                                    strSlipNo = "0005";
                                    break;
                                case "N-PTD25":
                                    strORDERCODE = "N-PTD25";
                                    strSlipNo = "0005";
                                    break;
                                case "A-POL8G":
                                    strORDERCODE = "00440411";
                                    strSlipNo = "0044";
                                    break;
                                case "A-POL2":
                                    strORDERCODE = "00440410";
                                    strSlipNo = "0044";
                                    break;
                                default:
                                    strORDERCODE = strSuCode;
                                    strSlipNo = "0044";
                                    break;
                            }

                            COMHPC item = new COMHPC();

                            item.PTNO = strPTNO;
                            item.BDATE = strBDATE;
                            item.DEPTCODE = "HR";
                            item.SEQNO = 99;
                            item.ORDERCODE = strORDERCODE;
                            item.SUCODE = strSuCode;
                            item.BUN = "20";
                            item.SLIPNO = strSlipNo;
                            item.REALQTY = nQty;
                            item.QTY = nQty;
                            item.NAL = 1;
                            item.GBDIV = "1";
                            item.DOSCODE = strDosCode;
                            item.GBBOTH = "0";
                            item.GBINFO = "";
                            item.GBER = "";
                            item.GBSELF = "";
                            item.GBSPC = "";
                            item.BI = "51";
                            item.DRCODE = strDrCode;
                            item.REMARK = "검사용";
                            item.GBSUNAP = "1";
                            item.TUYAKNO = 0;
                            item.MULTI = "";
                            item.MULTIREMARK = "";
                            item.DUR = "";
                            item.RESV = "";
                            item.SCODESAYU = "";
                            item.SCODEREMARK = "";
                            item.GBSEND = "Y";
                            item.SABUN = clsType.User.IdNumber;
                            item.CORDERCODE = strORDERCODE;
                            item.CSUCODE = strDosCode;
                            item.CBUN = "200";
                            item.IP = clsPublic.GstrIpAddress;

                            //2021-08-16
                            item.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item.ORDERCODE, item.SUCODE, "", item.BDATE, item.DEPTCODE);

                            result = comHpcLibBService.InsertOcsOrder(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("처방 전송 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else if (list4.QTY != nQty)
                        {
                            result = comHpcLibBService.UpdateQtybyRowId(nQty, list3.RID);
                        }

                        //OCS 오더번호를 찾음
                        nOrderno = comHpcLibBService.GetOrderNoOcsOrderbyPtno(strPTNO, strBDATE, strSuCode,"HR");

                        //OCS_HYANG Update
                        //향정약품 종검에서 Keep함, 본관내시경실만 전송함
                        if (strGbSite == "2")
                        {
                            if (VB.Left(strSuCode, 2) == "N-")
                            {
                                COMHPC list5 = comHpcLibBService.GetRowIdOcsMayakbyPtNo(strPTNO, strBDATE, strSuCode);

                                COMHPC item = new COMHPC();

                                item.BI = "51";
                                item.WARDCODE = "EN";
                                item.IO = "O";
                                item.QTY = nQty;
                                item.REALQTY = nQty;
                                item.NAL = 1;
                                item.DOSCODE = "920103";
                                item.ORDERNO = nOrderno;
                                item.REMARK1 = "검사용";
                                item.REMARK2 = "Pain";
                                item.JUSO = strJuso;
                                item.DRSABUN = clsType.User.IdNumber.To<long>();
                                item.CERTNO = "";
                                item.ROWID = strROWID;

                                if (list5.IsNullOrEmpty())
                                {
                                    result = comHpcLibBService.InsertSelectOcsMayak(item);
                                }
                                else if (nQty != nOldQty)
                                {
                                    result = comHpcLibBService.UpdateOcsMayak(item);
                                }

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("마약 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else  //향정
                            {
                                COMHPC list6 = comHpcLibBService.GetRowIdOcsHyangbyPtNo(strPTNO, strBDATE, strSuCode);

                                COMHPC item = new COMHPC();

                                item.BI = "51";
                                item.WARDCODE = "EN";
                                item.IO = "O";
                                item.QTY = nQty;
                                item.REALQTY = nQty;
                                item.NAL = 1;
                                item.DOSCODE = "920103";
                                item.ORDERNO = nOrderno;
                                item.REMARK1 = "검사용";
                                item.REMARK2 = "Pain";
                                item.JUSO = strJuso;
                                item.DRSABUN = clsType.User.IdNumber.To<long>();
                                item.CERTNO = "";
                                item.RID = strROWID;

                                if (list6.IsNullOrEmpty())
                                {
                                    result = comHpcLibBService.InsertSelectOcsHyang(item);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("향정 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else if (nQty != nOldQty)
                                {
                                    result = comHpcLibBService.UpdateOcsHyang(item);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("향정 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 마감여부 점검
        /// </summary>
        bool fn_Magam_Check()
        {
            bool rtnVal = false;

            if (hicHyangApproveService.GetCountbyBDate(dtpDate.Text, "N") > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }
    }
}
