using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaHyangjoengApproval.cs
/// Description     : 향정/마약 처방전 전송
/// Author          : 이상훈
/// Create Date     : 2019-10-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm향정처방승인.frm(Frm향정처방승인)" />


namespace ComHpcLibB
{
    public partial class frmHaHyangjoengApproval : Form
    {
        HicHyangApproveService hicHyangApproveService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicHyangApproveJepsuService hicHyangApproveJepsuService = null;
        BasSunService basSunService = null;
        EndoJupmstService endoJupmstService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        clsOrdFunction OF = new clsOrdFunction();

        string FstrDate;
        string fstrGubun = "";

        public frmHaHyangjoengApproval()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicHyangApproveService = new HicHyangApproveService();
            comHpcLibBService = new ComHpcLibBService();
            hicHyangApproveJepsuService = new HicHyangApproveJepsuService();
            basSunService = new BasSunService();
            endoJupmstService = new EndoJupmstService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnAllSelect.Click += new EventHandler(eBtnClick);
            this.btnCancleSelect.Click += new EventHandler(eBtnClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            sp.Spread_All_Clear(SS1);

            dtpDate.Text = clsPublic.GstrSysDate;
            FstrDate = "";
            txtSName.Text = "";
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
                int nREAD = 0;
                int nRow = 0;
                string strSuCode = "";
                string strDrname = "";
                string strDeptCode = "";
                string strOK = "";
                string strJob = "";


                sp.Spread_All_Clear(SS1);

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "2";
                }

                if (rdoGubun1.Checked == true)
                {
                    fstrGubun = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    fstrGubun = "2";
                }

                List<HIC_HYANG_APPROVE_JEPSU> list = hicHyangApproveJepsuService.GetItembyBDate(dtpDate.Text, txtSName.Text.Trim(), strJob, fstrGubun);

                nREAD = list.Count;
                nRow = 0;
                //SS1.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    strDeptCode = list[i].DEPTCODE;
                    strOK = "OK";
                    if (list[i].ENTQTY == 0)
                    {
                        strOK = "";
                    }

                    //내시경 검사완료 구분
                    //if (fstrGubun =="2")
                    //{
                        List<ENDO_JUPMST> list1 = endoJupmstService.GetItembyPtNoBDateDept(list[i].PTNO, list[i].BDATE);
                        if(list1.Count == 0)
                        {
                            strOK = "";
                        }
                    //}

                    if (strOK == "OK")
                    {
                        strSuCode = list[i].SUCODE;
                        nRow += 1;
                        if (nRow > SS1.ActiveSheet.RowCount)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        if (rdoJob1.Checked == true)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "True";  //미승인
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].BDATE;
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].PANO;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].AGE + "/" + list[i].SEX;
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = " " + strSuCode;
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = string.Format("{0:#0.00}", list[i].ENTQTY);
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = string.Format("{0:#0.00}", list[i].ENTQTY2);
                        //투약용량은 RealQTY에 환산계수량으로 입력함.
                        BAS_SUN list2 = basSunService.GetSuNameKbySuNext(strSuCode);

                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = " " + list2.UNITNEW2;
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = " " + list2.SUNAMEK;

                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].PRINT;
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                        if (list[i].OCSSENDTIME.ToString() != "")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = "Y";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                        if (list[i].GBSITE == "2")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "◎";
                        }
                        if (!list[i].DRNAME.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = list[i].DRNAME.Trim();
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].APPROVETIME.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list[i].RID;
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Text = list[i].PTNO;
                    }
                }

                if (fn_Magam_Check() == true)
                {
                    btnOK.Enabled = false;
                    btnCancel.Enabled = false;
                    MessageBox.Show("마감이 되어 처방전 전송/취소가 불가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (rdoJob1.Checked == true)
                {
                    btnOK.Enabled = true;
                    btnCancel.Enabled = false;
                }
                else
                {
                    btnOK.Enabled = true;
                    btnCancel.Enabled = true;
                }
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
                string strBDate = "";
                string strGbSite = "";
                string strMsg = "";
                long nPano = 0;
                int result = 0;

                if (rdoGubun1.Checked == true)
                {
                    if (fn_Magam_Check() == true)
                    {
                        MessageBox.Show("마감이 되어 처방전 전송/취소가 불가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                // 처방전 발행일에 의사가 휴일인지 점검
                if (hb.Check_Sabun_Huil(clsType.User.IdNumber, dtpDate.Text) == true)
                {
                    strMsg = strBDate + "일은 근무일이 아님, 전송이 불가능합니다.";
                    if (MessageBox.Show(strMsg, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.No)
                    {
                        return;
                    }
                }

                strList = "{}";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    if (strChk == "True" && strSuCode != "" && strApproveTime == "")
                    {
                        strBDate = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        if (VB.InStr(strList, "{}" + strBDate + "{}") == 0)
                        {
                            strList += strBDate + "{}";
                        }
                    }
                }

                strList = "{}";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strGbSite = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 15].Text.Trim();
                    if (strChk == "True" && strSuCode != "")
                    {
                        result = hicHyangApproveService.UpdateDrSabunPproveTimebyRowId(clsType.User.IdNumber, strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("향정/마약 의사 승인 내역 Update중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //내시경실 OCS에 전송할 명단을 보관함
                        strBDate = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        nPano = SS1.ActiveSheet.Cells[i, 2].Text.Trim().To<long>();
                        //if (strList.IndexOf("{}" + strBDate + "," + nPano + "{}") == 0)
                        if(VB.InStr(strList, "{}" +strBDate + ","+ nPano + "{}") ==0)
                        {
                            strList += strBDate + "," + nPano + "{}";
                        }
                    }
                }

                //--------------------------------------------
                // 본관 내시경실 OCS에 향정/마약 처방 전송
                //--------------------------------------------
                nCNT = VB.I(strList, "{}") - 1;
                for (int i = 2; i <= nCNT; i++)
                {
                    strBDate = VB.Pstr(VB.Pstr(strList, "{}", i), ",", 1);
                    nPano = long.Parse(VB.Pstr(VB.Pstr(strList, "{}", i), ",", 2));
                    if (strBDate != "" && nPano != 0)
                    {
                        fn_Send_Endo_Hyang_Slip(strBDate, nPano); //향정,마약 OCS에 전송
                    }
                }

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnCancel)
            {
                int nCNT = 0;
                string strChk = "";
                string strSuCode = "";
                string strPrint = "";
                string strApproveTime = "";
                string strGbSite = "";
                string strROWID = "";
                long nPano = 0;
                string strPtNo = "";
                string strBDate = "";
                string strList = "";
                int result = 0;

                if (fn_Magam_Check() == true)
                {
                    MessageBox.Show("마감이 되어 처방전 전송/취소가 불가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //인쇄한것은 전송취소 불가
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strPrint = SS1.ActiveSheet.Cells[i, 9].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 14].Text.Trim();

                    if (strChk == "True" && strSuCode != "" && strApproveTime != "")
                    {
                        if (strPrint == "Y")
                        {
                            MessageBox.Show(i + " 번줄 인쇄한 전송내역은 취소 불가능함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                strList = "{}";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    strBDate = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    nPano = long.Parse(SS1.ActiveSheet.Cells[i, 2].Text.Trim());
                    strSuCode = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    strGbSite = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                    strApproveTime = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 15].Text.Trim();
                    strPtNo = SS1.ActiveSheet.Cells[i, 16].Text.Trim();

                    if (strChk == "True" && strSuCode != "" && strApproveTime != "")
                    {
                        result = hicHyangApproveService.UpdateDrSabunbyRowId("", "", "", strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show("향정/마약 의사 승인 내역 Update중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        result = comHpcLibBService.DeleteOcsHyang(strPtNo, strSuCode, dtpDate.Text);

                        if (result < 0)
                        {
                            MessageBox.Show("향정 내역 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        result = comHpcLibBService.DeleteOcsMayak(strPtNo, strSuCode, dtpDate.Text);

                        if (result < 0)
                        {
                            MessageBox.Show("마약 내역 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        result = comHpcLibBService.DeleteOcsOrder(strPtNo, strSuCode, dtpDate.Text);

                        if (result < 0)
                        {
                            MessageBox.Show("외래처방 내역 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnAllSelect)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "True";
            }
            else if (sender == btnCancleSelect)
            {
                SS1.ActiveSheet.Cells[0, 0, SS1.ActiveSheet.RowCount - 1, 0].Text = "";
            }
        }

        void fn_Send_Endo_Hyang_Slip(string strBDate, long nPano)
        {
            int nCNT = 0;
            int nREAD = 0;
            int result = 0;
            double[] nHyangQty = new double[5];
            long nWRTNO;
            double nQty = 0;
            double nTotQty = 0;
            double nRealQty = 0;
            double nMiMayak = 0;
            double nOrderNo = 0;
            string strDrCode = "";
            string strDosCode = "";
            string strORDERCODE = "";
            string strSlipNo = "";

            string strPtNo = "";
            string strDRSABUN = "";
            string strNRSABUN = "";
            string strSex = "";
            long nAge = 0;
            string strJob = "";
            string strJong = "";
            string strDrug = "";
            string strDept = "";
            string strGbSite = "";
            string strSuCode = "";
            string strSname = "";
            string strJuso = "";
            string strROWID = "";
            string strROWID2 = "";
            string strROWID3 = "";
            string strGbJob = "";
            string strGubun = "";

            string strDeptCode = "";



            //누적할 배열을 Clear
            for (int i = 0; i < 5; i++)
            {
                nHyangQty[i] = 0;
            }

            nWRTNO = 0;
            strPtNo = "";
            strNRSABUN = "";
            nAge = 0;
            strDRSABUN = "";
            strSname = "";
            nMiMayak = 0;
            strGbJob = "";

            //전송할 검진자의 정보를 읽음
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyBdatePaNo(strBDate, nPano);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                //승인이 된것만 수량에 누적함
                if (!list[i].APPROVETIME.IsNullOrEmpty())
                {
                    if (clsHcVariable.GstrUSE == "OK")
                    {
                        switch (list[i].SUCODE.Trim())
                        {
                            case "A-POL2":
                                nHyangQty[0] += list[i].ENTQTY;
                                break;
                            case "A-ANE12G":
                                nHyangQty[1] += list[i].ENTQTY;
                                break;
                            case "A-POL8G":
                                nHyangQty[2] += list[i].ENTQTY;
                                break;
                            case "A-BASCAM":
                                nHyangQty[3] += list[i].ENTQTY;
                                break;
                            case "N-PTD25":
                                nHyangQty[4] += list[i].ENTQTY;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (list[i].SUCODE.Trim())
                        {
                            case "A-POL2":
                                nHyangQty[0] += list[i].ENTQTY;
                                break;
                            case "A-ANE12G":
                                nHyangQty[1] += list[i].ENTQTY;
                                break;
                            case "A-POL8G":
                                nHyangQty[2] += list[i].ENTQTY;
                                break;
                            case "A-BASCAM":
                                nHyangQty[3] += list[i].ENTQTY;
                                break;
                            case "N-PTD25":
                                nHyangQty[4] += list[i].ENTQTY;
                                break;
                            default:
                                break;
                        }
                    }
                    strDRSABUN = list[i].DRSABUN;
                }
                else
                {
                    //미승인 마약이 있으면 취소
                    switch (list[i].SUCODE.Trim())
                    {
                        case "N-PTD25":
                            nMiMayak += list[i].ENTQTY;
                            break;
                        default:
                            break;
                    }
                }

                if (nWRTNO == 0)
                {
                    nWRTNO = list[i].WRTNO;
                    strPtNo = list[i].PTNO;
                    strDRSABUN = list[i].DRSABUN;
                    strNRSABUN = list[i].NRSABUN;
                    strSname = list[i].SNAME;
                    strSex = list[i].SEX;
                    nAge = list[i].AGE;
                    strGbSite = list[i].GBSITE;
                }
            }

            //향정/마약을 OCS에 UPDATE
            for (int i = 0; i < 5; i++)
            {
                if (clsHcVariable.GstrUSE == "OK")
                {
                    switch (i)
                    {
                        case 0:
                            strDrug = "A-POL2"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 1:
                            strDrug = "A-ANE12G"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 2:
                            strDrug = "A-POL8G"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 3:
                            strDrug = "A-BASCAM"; strJong = "1";
                            strGbJob = "3";
                            break;
                        case 4:
                            strDrug = "N-PTD25"; strJong = "2";
                            strGbJob = "3";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            strDrug = "A-POL2"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 1:
                            strDrug = "A-ANE12G"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 2:
                            strDrug = "A-POL8G"; strJong = "1";
                            strGbJob = "2";
                            break;
                        case 3:
                            strDrug = "A-BASCAM"; strJong = "1";
                            strGbJob = "3";
                            break;
                        case 4:
                            strDrug = "N-PTD25"; strJong = "2";
                            strGbJob = "3";
                            break;
                        default:
                            break;
                    }
                }

                //위,대장검사 결과등록의사 확인(2:위, 3:대장) (2021-11-12)
                COMHPC item0 = comHpcLibBService.GetEndoJupmstByRdateDeptCodeGbSunap(strBDate, strGbJob, strPtNo);
                if (!item0.IsNullOrEmpty())
                {
                    if (!item0.RESULTDRCODE.IsNullOrEmpty()) { strDRSABUN = item0.RESULTDRCODE.Trim(); }
                    strDeptCode = item0.DEPTCODE.Trim();
                }

                //대장검사시 A-ANE12G 사용시 체크로직
                if (item0.IsNullOrEmpty() && strDrug =="A-ANE12G" && strGbJob =="2")
                {
                    COMHPC item1 = comHpcLibBService.GetEndoJupmstByRdateDeptCodeGbSunap(strBDate, "3", strPtNo);
                    if (!item1.IsNullOrEmpty())
                    {
                        if (!item1.RESULTDRCODE.IsNullOrEmpty()) { strDRSABUN = item1.RESULTDRCODE.Trim(); }
                        strDeptCode = item1.DEPTCODE.Trim();
                    }
                }

                //본관 내시경실만 전송함
                if (strGbSite == "2" && nHyangQty[i] > 0)
                {

                    if (strJong == "1")
                    {
                        clsHcVariable.GnWRTNO = nWRTNO;
                        hm.Insert_Ocs_Hyang(nWRTNO, strPtNo, strBDate, strDrug, "", nHyangQty[i], strDRSABUN, fstrGubun, strDeptCode);
                    }
                    else if (strJong == "2")
                    {
                        clsHcVariable.GnWRTNO = nWRTNO;
                        hm.Insert_OCS_Mayak(nWRTNO, strPtNo, strBDate, strDrug, "", nHyangQty[i], strDRSABUN, fstrGubun, strDeptCode);
                    }
                }

                ///TODO : 이상훈(2019.10.22) 검진처방 테이블 설계 완료 후 진행;
                if(nHyangQty[i] > 0 )
                {
                    strSuCode = strDrug;
                    nQty = 1;

                    //외래접수
                    COMHPC item = comHpcLibBService.GetOpdMasterbyPaNoDeptCodeBDate(strPtNo, "TO", strBDate);
                    if (item.IsNullOrEmpty())
                    {
                        COMHPC item1 = new COMHPC();

                        item1.PTNO = strPtNo;
                        item1.DEPTCODE = "TO";
                        item1.BI = "51";
                        item1.SNAME = strSname;
                        item1.SEX = strSex;
                        item1.AGE = nAge;
                        item1.DRCODE = "7102";
                        item1.RESERVED = "0";
                        item1.CHOJAE = "1";
                        item1.GBGAMEK = "00";
                        item1.GBSPC = "0";
                        item1.JIN = "D";
                        item1.SINGU = "0";
                        item1.PART = "111";
                        item1.BDATE = strBDate;
                        item1.EMR = "0";
                        item1.GBUSE = "Y";
                        item1.MKSJIN = "D";

                        result = comHpcLibBService.InsertOpdMaster(item1);

                        if (result < 0)
                        {
                            MessageBox.Show("외래접수 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }


                    //(2021 - 11 - 12)

                    //OCS_OORDER전송
                    COMHPC item2 = comHpcLibBService.GetRowIdQtyOcsOrderbyPtNoBDateSuCode(strPtNo, strBDate, strSuCode, strDeptCode);
                    if ( item2.IsNullOrEmpty())
                    {
                        strDosCode = "920103";
                        strDrCode = hb.READ_HIC_OcsDrcode(strDRSABUN.To<long>(0));
                        //strDrCode = hb.READ_HIC_OcsDrcode(clsType.User.IdNumber.To<long>(0));

                        switch (strSuCode)
                        {
                            case "A-ANE12G":
                                strORDERCODE = "A-ANE12G"; strSlipNo = "0005";
                                break;
                            case "A-BASCAM":
                                strORDERCODE = "A-BASCAM"; strSlipNo = "0005";
                                break;
                            case "N-PTD25":
                                strORDERCODE = "N-PTD25"; strSlipNo = "0005";
                                break;
                            case "A-POL8G":
                                strORDERCODE = "00440411"; strSlipNo = "0044";
                                break;
                            case "A-POL2":
                                strORDERCODE = "00440410"; strSlipNo = "0044";
                                break;
                            default:
                                strORDERCODE = strSuCode; strSlipNo = "0044";
                                break;
                        }


                        COMHPC item3 = new COMHPC();

                        item3.PTNO = strPtNo;
                        item3.BDATE = strBDate;
                        item3.DEPTCODE = strDeptCode;
                        item3.SEQNO = 99;
                        item3.ORDERCODE = strORDERCODE;
                        item3.SUCODE = strSuCode;
                        item3.BUN = "20";
                        item3.SLIPNO = strSlipNo;
                        item3.REALQTY = nQty;
                        item3.QTY = nQty;
                        item3.NAL = 1;
                        item3.GBDIV = "1";
                        item3.DOSCODE = strDosCode;
                        item3.GBBOTH = "0";
                        item3.GBINFO = "";
                        item3.GBER = "";
                        item3.GBSELF = "";
                        item3.GBSPC = "";
                        item3.BI = "51";
                        item3.DRCODE = strDrCode;
                        item3.REMARK = "검사용";
                        item3.GBSUNAP = "1";
                        item3.TUYAKNO = 0;
                        item3.MULTI = "";
                        item3.MULTIREMARK = "";
                        item3.DUR = "";
                        item3.RESV = "";
                        item3.SCODESAYU = "";
                        item3.SCODEREMARK = "";
                        item3.GBSEND = "Y";
                        //item3.SABUN = clsType.User.IdNumber;
                        item3.SABUN = strDRSABUN;
                        item3.CORDERCODE = strORDERCODE;
                        item3.CSUCODE = strSuCode;
                        item3.CBUN = "20";
                        item3.IP = clsPublic.GstrIpAddress;

                        //2021-08-16
                        item3.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item3.ORDERCODE, item3.SUCODE, "", item3.BDATE, item3.DEPTCODE);

                        result = comHpcLibBService.InsertOcsOrder(item3);
                        if (result < 0)
                        {
                            MessageBox.Show("향정처방등록중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //OILLS(상병)등록
                        if (comHpcLibBService.GetOcsIllsbyPtNoDeptCodeIllCode(strPtNo, strDeptCode, "Z018") == 0) 
                        {
                            COMHPC item4 = new COMHPC();

                            item4.PTNO = strPtNo;
                            item4.BDATE = strBDate;
                            item4.DEPTCODE = "TO";
                            item4.SEQNO = 1;
                            item4.ILLCODE = "Z018";

                            result = comHpcLibBService.InsertOIlls(item4);
                            if (result < 0)
                            {
                                MessageBox.Show("상병코드등록중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else if (item2.QTY != nQty)
                    {
                        comHpcLibBService.UpdateOcsOrder(nQty.To<long>(0), item2.RID);
                    }

                    nOrderNo = 0;
                    //OCS오더번호찾음
                    nOrderNo = comHpcLibBService.GetOrderNoOcsOrderbyPtno(strPtNo, strBDate, strSuCode, strDeptCode);

                    strROWID2 = "";
                    //OCS_HYANG_UPDATE
                    strROWID2 = comHpcLibBService.GetRidOcsHyang(strBDate, strPtNo, strSuCode);
                    if (!strROWID2.IsNullOrEmpty())
                    {
                        result = comHpcLibBService.UpdateHyangOrderNo1(nOrderNo.ToString(),strROWID2);

                    }
                    strROWID3 = "";
                    //OCS_MAYAK_UPDATE
                    strROWID3 = comHpcLibBService.GetRidOcsMayak(strBDate, strPtNo, strSuCode);
                    if (!strROWID3.IsNullOrEmpty())
                    {
                        result = comHpcLibBService.UpdateMayakOrderNo1(nOrderNo.ToString(), strROWID3);
                    }
                }



                /*
                    If nHyangQty(i) > 0 Then
                    strSuCode = strDrug
                    nQty = 1
            
                    '2014-12-15 외래접수
                    SQL = " SELECT ROWID FROM OPD_MASTER "
                    SQL = SQL & " WHERE PANO = '" & strPtNo & "' "
                    SQL = SQL & "   AND BDate = TO_Date('" & ArgBDate & "','YYYY-MM-DD') "
                    SQL = SQL & "   AND DEPTCODE='TO' "
                    Call AdoOpenSet(rs1, SQL)
                    If RowIndicator = 0 Then
                        SQL = "INSERT INTO OPD_MASTER " & vbLf
                        SQL = SQL & "(ActDate, Pano, DeptCode, Bi, Sname, Sex,Age, JiCode, DrCode, "
                        SQL = SQL & " Reserved, Chojae, GbGamek,GbSpc, Jin, Singu,Part,Jtime,BDate, "
                        SQL = SQL & " EMR,GbUse,MksJin) VALUES (" & vbLf
                        SQL = SQL & " TRUNC(SYSDATE),'" & strPtNo & "','TO','51','" & strSname & "','"
                        SQL = SQL & strSex & "'," & nAge & ",'','7102','0','1','00','"
                        SQL = SQL & "0','D','0','111',SYSDATE,TO_DATE('" & ArgBDate & "','YYYY-MM-DD'),'0','Y','D') "
                        result = AdoExecute(SQL)
                    End If
                    Call AdoCloseSet(rs1)
            
                    '2014-12-15 OCS_OORDER 전송
                    SQL = "SELECT ROWID,Qty FROM ADMIN.OCS_OORDER "
                    SQL = SQL & "WHERE Ptno='" & strPtNo & "' "
                    SQL = SQL & "  AND BDate=TO_DATE('" & ArgBDate & "','YYYY-MM-DD') "
                    SQL = SQL & "  AND DeptCode='TO' "
                    SQL = SQL & "  AND SuCode='" & strSuCode & "' "
                    Call AdoOpenSet(rs1, SQL)
                    If RowIndicator = 0 Then
                        strDosCode = "920103"
                        If strSuCode = "N-PTD25" Then strDosCode = "999903"
                        StrDrCode = READ_HIC_OcsDrcode(GnJobSabun)
            
                        '오더코드,SlipNo 설정
                        Select Case Trim(strSuCode)
                            Case "A-PO12GA": strORDERCODE = "A-PO12GA": strSlipNo = "0005"
                            'Case "A-POL12G": strORDERCODE = "00440325": strSlipNo = "0044"
                            'Case "A-BASCA":  strOrderCode = "A-BASCA":  strSlipNo = "0005"
                            Case "A-BASCAM":  strORDERCODE = "A-BASCAM":  strSlipNo = "0005"
                            Case "N-PTD25":  strORDERCODE = "N-PTD25":  strSlipNo = "0005"
                            Case "A-POL8G":  strORDERCODE = "00440411": strSlipNo = "0044"
                            Case "A-POL2":   strORDERCODE = "00440410": strSlipNo = "0044"
                            Case Else:       strORDERCODE = strSuCode:  strSlipNo = "0044"
                        End Select
                
                        '2019-07-08
                        'Call OCS_OORDER_INSERT(strPtNo, strORDERCODE, strSuCode)
                
                
                        SQL = " INSERT INTO ADMIN.OCS_OORDER " & vbLf
                        SQL = SQL & "  (PTNO, BDATE, DEPTCODE, SEQNO, ORDERCODE, SUCODE, BUN, SLIPNO, REALQTY, QTY, NAL, GBDIV, "
                        SQL = SQL & "   DOSCODE, GBBOTH, GBINFO, GBER, GBSELF, GBSPC, BI, DRCODE, REMARK, ENTDATE, GBSUNAP, TUYAKNO, "
                        SQL = SQL & "   ORDERNO, MULTI, MULTIREMARK, DUR, RESV, SCODESAYU, SCODEREMARK, GBSEND,SABUN, "
                        SQL = SQL & "   CORDERCODE, CSUCODE, CBUN ) VALUES "
                        SQL = SQL & "   ( '" & strPtNo & "' , TO_DATE('" & ArgBDate & "','YYYY-MM-DD'), 'TO',"
                        SQL = SQL & "     99, '" & strORDERCODE & "', '" & strSuCode & "','20','" & strSlipNo & "'," & nQty & "," & nQty & ",'1','1', "
                        SQL = SQL & "     '" & strDosCode & "','0','','','','','51', '" & StrDrCode & "','검사용' ,"
                        SQL = SQL & "     SYSDATE, '1','0', ADMIN.SEQ_ORDERNO.NEXTVAL,'','','','','','','Y','" & GnJobSabun & "',"
                        SQL = SQL & "    '" & strORDERCODE & "','" & strSuCode & "','20' ) " & vbLf
                        result = AdoExecute(SQL)
                         
                
                    ElseIf AdoGetNumber(rs1, "Qty", 0) <> nQty Then
                        SQL = "UPDATE ADMIN.OCS_OORDER SET "
                        SQL = SQL & " Qty=" & nQty & ", "
                        SQL = SQL & " RealQty=" & nQty & "  "
                        SQL = SQL & "WHERE ROWID='" & Trim(AdoGetString(rs1, "ROWID", 0)) & "' "
                        result = AdoExecute(SQL)
                    End If
                    Call AdoCloseSet(rs1)

                    'OCS 오더번호를 찾음
                    SQL = "SELECT ORDERNO FROM ADMIN.OCS_OORDER "
                    SQL = SQL & "WHERE Ptno='" & strPtNo & "' "
                    SQL = SQL & "  AND BDate=TO_DATE('" & ArgBDate & "','YYYY-MM-DD') "
                    SQL = SQL & "  AND DeptCode='TO' "
                    SQL = SQL & "  AND SuCode='" & strSuCode & "' "
                    Call AdoOpenSet(rs1, SQL)
                    nOrderNo = AdoGetNumber(rs1, "OrderNo", 0)
                    Call AdoCloseSet(rs1)
            
                    'OCS_HYANG Update
                    SQL = "SELECT ROWID FROM ADMIN.OCS_HYANG "
                    SQL = SQL & "WHERE Ptno='" & strPtNo & "' "
                    SQL = SQL & "  AND BDate=TO_DATE('" & ArgBDate & "','YYYY-MM-DD') "
                    SQL = SQL & "  AND DeptCode IN ('HR','TO') "
                    SQL = SQL & "  AND SuCode='" & strSuCode & "' "
                    Call AdoOpenSet(rs1, SQL)
                    If RowIndicator > 0 Then
                        SQL = "UPDATE ADMIN.OCS_HYANG SET "
                        SQL = SQL & " DeptCode='TO', "
                        SQL = SQL & " WardCode='EN', "
                        SQL = SQL & " OrderNo=" & nOrderNo & ", "
                        SQL = SQL & " Certno='' "
                        SQL = SQL & "WHERE ROWID='" & AdoGetString(rs1, "ROWID", 0) & "' "
                        result = AdoExecute(SQL)
                    End If
            
                    'OCS_MAYAK Update
                    SQL = "SELECT ROWID FROM ADMIN.OCS_MAYAK "
                    SQL = SQL & "WHERE Ptno='" & strPtNo & "' "
                    SQL = SQL & "  AND BDate=TO_DATE('" & ArgBDate & "','YYYY-MM-DD') "
                    SQL = SQL & "  AND DeptCode IN ('HR','TO') "
                    SQL = SQL & "  AND SuCode='" & strSuCode & "' "
                    Call AdoOpenSet(rs1, SQL)
                    If RowIndicator > 0 Then
                        SQL = "UPDATE ADMIN.OCS_MAYAK SET "
                        SQL = SQL & " DeptCode='TO', "
                        SQL = SQL & " WardCode='EN', "
                        SQL = SQL & " OrderNo=" & nOrderNo & ", "
                        SQL = SQL & " Certno='' "
                        SQL = SQL & "WHERE ROWID='" & AdoGetString(rs1, "ROWID", 0) & "' "
                        result = AdoExecute(SQL)
                    End If
            
                End If
                  
                */
            }
        }

        /// <summary>
        /// 마감여부 점검
        /// </summary>
        /// <returns></returns>
        bool fn_Magam_Check()
        {
            bool rtnVal = false;

            if (hicHyangApproveService.GetCountbyBDate(dtpDate.Text) > 0)
            {
                rtnVal = true;
            }

            ///TODO : 이상훈(2019.10.22) 마감체크 확인
            rtnVal = false;

            return rtnVal;
        }

        void eRdoClick(object sender, EventArgs e)
        {
            eBtnClick(btnSearch, new EventArgs());
        }
    }
}
