using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcIndustrialMisuAutoFormation.cs
/// Description     : 공단 미수 자동형성
/// Author          : 심명섭
/// Create Date     : 2021-06-28
/// Update History  : 
/// </summary>
/// <seealso cref= "hcmisu > Frm공단미수자동형성 (Frm공단미수자동형성.frm)" />
/// 

namespace HC_Bill
{
    public partial class frmHcIndustrialMisuAutoFormation :BaseForm
    {
        // 약한참조
        clsSpread cSpd = null;
        clsPublic cpublic = null;
        ComFunc cF = null;
        clsHaBase cHb = null;
        clsHcMisu cHM = null;
        clsHcMain hM = null;
        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;
        frmHcCodeHelp FrmHcCodeHelp = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicMirCancerService hicMirCancerService = null;
        HicCodeService hicCodeService = null;
        HicJepsuService hicJepsuService = null;
        HicSunapService hicSunapService = null;
        HicMisuDetailService hicMisuDetailService = null;
        HicMisuMstSlipService hicMisuMstSlipService = null;
        HicLtdService hicLtdService = null;
        HicComHpcService hicComHpcService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicExjongService hicExjongService = null;

        //const string Jong_1 = "'11','12','16','17','14','19','23','28'";      // 특수
        //const string Jong_2 = "'21','22','24','27','29','30'";      // 채용, 배치전
        //const string Jong_3 = "'50','51'";      // 방사선, 기타검진
        //const string Jong_4 = "'31','35'";      // 암검진
        //const string Jong_6 = "'11','12','13'";      // 구강검진
        string[] Jong_1 = { "11","12","16","17","14","19","23","28" };
        string[] Jong_2 = { "21","22","24","27","29","30" };
        string[] Jong_3 = { "50","51" };
        string[] Jong_4 = { "31","35" };
        string[] Jong_6 = { "11","12","13" };

        List<string> JongSQL = new List<string>();
        List<string> Wrtno = new List<string>();
        bool Gubun = true;

        int result;
        int ninwon;

        int nRow;
        long nCnt;
        long nWrtno;
        long nMisuNo;
        long nTempAmt;
        long nMirNo;
        long nDentCnt;
        long nDentAmt1;

        double nTotAmt;
        double nAmt;

        string FstrCode = "";
        string FstrName = "";
        string FstrLtdName;
        string FstrJong;
        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;
        string FstrKiho;
        string strMinDate;
        string strMaxDate;
        string strOldData;
        string strNewData = "";
        string strDate;
        string strOK;
        string FstrChaDate;
        string strChk;
        string strBDate;
        string strGJong;
        string strRemark;
        string strPumMok;
        string strGiroNo;
        string strROWID;
        string strltdcode;
        string strFDate;
        string strTDate;
        string strYEAR;
        string FstrCOLNM;
        string FstrJongSQL;

        public frmHcIndustrialMisuAutoFormation()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnKihoHelp.Click += new EventHandler(eBtnClick);
            this.btnBogunHelp.Click += new EventHandler(eBtnClick);
            this.btnHelp2.Click += new EventHandler(eBtnClick);
            this.menuPrint1.Click += new EventHandler(eBtnClick);
            this.menuPrint2.Click += new EventHandler(eBtnClick);
            this.menuEtc.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            long nREAD;
            int nRow;
            long nRow3;
            long nMirNo;
            long nTotAmt;
            string strOldData;
            string strNewData;
            long nAmt1;
            long nAmt2;
            long nJohapAmt;
            long nTotHicAmt;
            long nCnt;
            long nTotAmt1;
            long nTotAmt2;

            long nDentAmt1;
            long nDentCnt;

            string Temp1;
            string Temp2;
            long nCnt1;
            long nCnt2;

            long nTempAmt;
            long nWrtno;
            string strDENTAL;

            if (e.Row < 0)
            {
                return;
            }

            FstrJong = VB.Left(cboJong.Text, 1);

            FstrLtdCode = SS1.ActiveSheet.Cells[e.Row, 0].Text;
            FstrLtdName = SS1.ActiveSheet.Cells[e.Row, 1].Text;
            FstrFDate = SS1.ActiveSheet.Cells[e.Row, 2].Text;
            FstrTDate = SS1.ActiveSheet.Cells[e.Row, 3].Text;
            FstrKiho = SS1.ActiveSheet.Cells[e.Row, 8].Text;
            nMirNo = SS1.ActiveSheet.Cells[e.Row, 9].Text.To<long>(0);

            TxtLtdCode.Text = "";
            TxtLtdName.Text = "";
            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtPummok.Text = "";
            TxtChaAmt.Text = "";
            TxtChaRemark.Text = "";

            // 해당 회사의 청구명단을 표시함
            SS2.ActiveSheet.RowCount = 50;
            SS_Clear(SS2_Sheet1);

            nTotAmt = 0;
            nAmt1 = 0;
            nAmt2 = 0;
            nTotAmt1 = 0;
            nTotAmt2 = 0;
            nTotHicAmt = 0;
            nDentAmt1 = 0;
            nDentCnt = 0;

            List<COMHPC> list = hicComHpcService.GetGongDanListData(FstrCOLNM, nMirNo, FstrLtdCode, FstrJong);

            if(!list.IsNullOrEmpty() || list.Count > 0)
            {
                nRow = 0;
                strOldData = "";
                for (int i = 0; i < list.Count; i++)
                {
                    nWrtno = list[i].WRTNO;
                    cHb.READ_SUNAPDTL_Calculator(list[i].WRTNO);

                    // 암검진
                    if (FstrJong == "4")
                    {
                        cHb.READ_SUNAPDTL_Calculator(list[i].WRTNO);
                        nTempAmt = clsHcVariable.GnAmt_Misu_JohapAmt1.To<long>(0);
                    }
                    else
                    {
                        nTempAmt = list[i].JOHAPAMT;
                    }

                    if (nTempAmt != 0)
                    {
                        nRow += 1;
                        if (nRow > SS2.ActiveSheet.RowCount)
                        {
                            SS2.ActiveSheet.RowCount = nRow;
                        }

                        // 일반건진
                        nJohapAmt = nTempAmt;
                        SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PANO.To<string>("");
                        if (list[i].MURYOAM == "Y")
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.LightPink;
                        }

                        SS2.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].SNAME.Trim();
                        SS2.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE.Trim();
                        SS2.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].WRTNO.ToString();
                        SS2.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE.ToString() + " / " + list[i].SEX;
                        SS2.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Left(list[i].JUMIN, 2);
                        SS2.ActiveSheet.Cells[nRow - 1, 7].Text = cHb.READ_GjJong_Name(list[i].GJJONG.Trim());
                        if (clsHcVariable.GnAmt_Misu_BogenAmt2 > 0)
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Format(clsHcVariable.GnAmt_Misu_LtdAmt2, "###,###,##0");
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Format(list[i].LTDAMT, "###,###,##0");
                        }

                        // 구강
                        strDENTAL = "";
                        List<HIC_SUNAPDTL> DentalList = hicSunapdtlService.GetDentalList(nWrtno);
                        if (DentalList.Count > 0)
                        {
                            strDENTAL = "OK";
                        }

                        if (FstrJong != "6")
                        {
                            if (list[i].DENTAMT > 0 && strDENTAL == "OK")
                            {
                                nDentCnt += 1;
                                SS2.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Format(list[i].TOTAMT - nDentAmt1, "###,###,###,##0");
                                SS2.ActiveSheet.Cells[nRow - 1, 10].Text = VB.Format(list[i].JOHAPAMT, "###,###,###,##0");
                                SS2.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(list[i].JOHAPAMT - list[i].DENTAMT, "###,###,###,##0");
                                if (SS2.ActiveSheet.Cells[nRow - 1, 11].Text.Trim() != "26,550" && SS2.ActiveSheet.Cells[nRow - 1, 11].Text.Trim() != "37,850")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 11].BackColor = Color.LightPink;
                                }
                            }
                            else
                            {
                                SS2.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Format(list[i].TOTAMT, "###,###,###,##0");
                                SS2.ActiveSheet.Cells[nRow - 1, 10].Text = VB.Format(list[i].JOHAPAMT, "###,###,###,##0");
                                SS2.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(nJohapAmt, "###,###,###,##0");
                                if (SS2.ActiveSheet.Cells[nRow - 1, 11].Text.Trim() != "26,550" && SS2.ActiveSheet.Cells[nRow - 1, 11].Text.Trim() != "37,850")
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, 11].BackColor = Color.LightPink;
                                }
                            }
                        }
                        else
                        {
                            // 구강검진
                            if (list[i].DENTAMT > 0)
                            {
                                nDentCnt += 1;
                                SS2.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Format(list[i].DENTAMT, "###,###,###,##0");
                                SS2.ActiveSheet.Cells[nRow - 1, 9].Text = "0";
                                SS2.ActiveSheet.Cells[nRow - 1, 10].Text = "0";
                                SS2.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(list[i].DENTAMT, "###,###,###,##0");
                            }
                        }

                        SS2.ActiveSheet.Cells[nRow - 1, 12].Text = hM.SExam_Names_Display(list[i].SEXAMS);

                        if (list[i].UCODES.IsNullOrEmpty())
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 13].Text = "";
                        }
                        else
                        {
                            if (list[i].UCODES.Trim() != "ZZZ")
                            {
                                SS2.ActiveSheet.Cells[nRow - 1, 13].Text = cHM.Ucode_Name_Display(list[i].UCODES.Trim());
                            }
                        }

                        SS2.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].GJCHASU;
                        SS2.ActiveSheet.Cells[nRow - 1, 15].Text = list[i].GJJONG;

                        if (FstrJong == "4")  // 암검진
                        {
                            nTotAmt += nJohapAmt;
                        }
                        else if (FstrJong == "6")
                        {
                            nTotAmt += list[i].DENTAMT;
                        }
                        else
                        {
                            if (strDENTAL == "OK")
                            {
                                nTotAmt += list[i].JOHAPAMT - list[i].DENTAMT;
                            }
                            else
                            {
                                nTotAmt += list[i].JOHAPAMT;
                            }
                        }
                    }
                }

                nRow += 1;
                SS2.ActiveSheet.RowCount = nRow;
                SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "** 합계 **";
                if (FstrJong != "6")
                {
                    nTotAmt -= (nDentCnt * nDentAmt1);
                }
                SS2.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(nTotAmt, "###,###,##0");

                if (FstrJong == "5")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, 11].Text = VB.Format(nTotHicAmt, "###,###,##0");
                }

                Inwon_Amt_Gesan();
            }
            else
            {
                MessageBox.Show("해당 회사의 청구명단 READ 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for (int i = 0; i < Spd.RowCount; i++)
            {
                for (int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                DisPlay_Screen();

            }
            else if (sender == btnSave)
            {
                DataSave();
            }
            else if (sender == btnKihoHelp)
            {
                KihoHelp();
            }
            else if (sender == btnBogunHelp)
            {
                BogunHelp();
            }
            else if (sender == btnHelp2)
            {
                LtdHelp();
            }
            else if (sender == menuPrint1)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                Gubun = true;
                menuPrint(Gubun);
            }
            else if (sender == menuPrint2)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                Gubun = false;
                menuPrint(Gubun);
            }
            else if (sender == menuEtc)
            {
                SelfChungGu();
            }
        }

        private void DataSave()
        {
           if(TxtGiroNo.Text.Trim() == "")
            {
                MessageBox.Show("청구(지로)번호를 입력하세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtRemark.Text.Trim() == "")
            {
                MessageBox.Show("적요가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtPummok.Text.Trim() == "")
            {
                MessageBox.Show("품목이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtYYMM.Text.Trim() == "")
            {
                MessageBox.Show("검진년월이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(VB.Len(TxtYYMM.Text) != 6)
            {
                MessageBox.Show("검진년월형식을 YYYYMM(202106)으로 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FstrJong = VB.Left(cboJong.Text, 1);    // 미수청구구분
            strGiroNo = TxtGiroNo.Text.Trim();

            FstrChaDate = "";
            if(TxtChaAmt.Text.To<int>(0) != 0)
            {
                FstrChaDate = clsPublic.GstrSysDate;
            }

            clsPublic.GstrMsgList = "공단미수내역을 자동형성 하시겠습니까 ?";
            if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            strBDate = TxtDate.Text.Trim();
            if (strBDate == "")
            {
                MessageBox.Show("공단미수일자가 오류입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Wrtno.Clear();

            for (int i = 0; i < SS2.ActiveSheet.RowCount - 1; i++)
            {
                strChk = SS2.ActiveSheet.Cells[i, 0].Text;
                strOK = "";
                strChk = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                nWrtno = SS2.ActiveSheet.Cells[i, 4].Text.To<long>(0);
                strOK = "";
                if (rdoJob1.Checked == true && strChk == "True") { strOK = "OK"; }
                if (rdoJob2.Checked == true && strChk != "True") { strOK = "OK"; }

                if (strOK == "OK")
                {
                    if (cboYear.Text.Trim() != cHM.READ_GJYEAR(nWrtno))
                    {
                        MessageBox.Show("검진년도와 미수검진년도가 서로 다릅니다.", "미수발생불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Wrtno.Add(VB.Format(nWrtno, "########0"));
                }
            }

            if (Wrtno.Count > 0)
            {   // 일반건진
                if (!Save_Sub1())
                {
                    return;
                }
            }

            DisPlay_Screen();

            MessageBox.Show("공단미수형성이 완료되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.None);

            Screen_Clear();

        }

        private bool Save_Sub1()
        {
            nMisuNo = cHb.READ_New_MisuNo();
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                // 공단청구 완료 SET             
                if (!GongDanChungGu())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return false;
                }

                if (!GuGangGongDan())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return false;
                }

                for(int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    nWrtno = SS2.ActiveSheet.Cells[i, 4].Text.To<long>(0);
                    nTempAmt = SS2.ActiveSheet.Cells[i, 11].Text.To<long>(0);

                    if(SS2.ActiveSheet.Cells[i, 15].Text != "")
                    {
                        strGJong = SS2.ActiveSheet.Cells[i, 15].Text;
                    }

                    // HIC_MISU_DETAIL에 상세내역을 입력
                    if (!MisuDetail())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }

                ninwon = TxtInwon.Text.To<int>(0);
                nAmt = TxtMisuAmt.Text.To<long>(0);

                if(nAmt != 0)
                {
                    // 2004-06-11 적요 및 품목을 별도 입력하도록 수정함
                    strRemark = TxtRemark.Text.Trim();
                    strPumMok = TxtPummok.Text.Trim();
                    strYEAR = cboYear.Text.Trim();

                    // 미수마스타에 신규등록
                    if (!MisuMasterNew())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                    // 미수SLIP에 신규등록
                    if (!MisuSlipNew())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }

                    // 신규입력 내역을 History에 INSERT
                    if (!NewHistoryInsert())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                FstrKiho = "";

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        private bool NewHistoryInsert()
        {
            result = hicMisuMstSlipService.GongDanNewHistoryInsert(clsPublic.GnJobSabun, nMisuNo);
            if (result < 0)
            {
                MessageBox.Show("신규등록 내역을 HISTORY에 INSERT시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuSlipNew()
        {
            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
            {
                WRTNO =            nMisuNo,
                BDATE =            strBDate,
                LTDCODE =          FstrLtdCode.To<long>(0),
                GEACODE =          "11",
                SLIPAMT =          nAmt.To<long>(0),
                REMARK =           strRemark,
                ENTSABUN =         clsType.User.IdNumber.To<long>(0)
            };

            result = hicMisuMstSlipService.GongDanMisuSlipNew(item);
            if (result < 0)
            {
                MessageBox.Show("미수SLIP에 등록중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuMasterNew()
        {
            HIC_MISU_MST_SLIP item = new HIC_MISU_MST_SLIP
            {
                WRTNO =            nMisuNo,
                LTDCODE =          TxtLtdCode.Text.To<long>(0),
                MISUJONG =         "2",
                JISA =             "0719",
                KIHO =             FstrKiho,
                BDATE =            strBDate,
                GJONG =            strGJong,
                GBEND =            "N",
                MISUGBN =          "1",
                MISUAMT =          nAmt.To<long>(0),
                IPGUMAMT =         0,
                GAMAMT =           0,
                SAKAMT =           0,
                BANAMT =           0,
                JANAMT =           nAmt.To<long>(0),
                MIRCHAAMT =        TxtChaAmt.Text.To<long>(0),
                MIRCHAREMARK =     TxtChaRemark.Text.Trim(),
                MIRCHADATE =       FstrChaDate,
                DAMNAME =          clsPublic.GstrJobName,
                REMARK =           strRemark,
                PUMMOK =           strPumMok,
                GIRONO =           strGiroNo,
                MIRGBN =           "1",
                GBMISUBUILD4 =     "Y",
                YYMM_JIN =         TxtYYMM.Text.Trim(),
                ENTSABUN =         clsType.User.IdNumber.To<long>(0),
                GJYEAR =           strYEAR
            };

            result = hicMisuMstSlipService.GongDanMisuMasterNew(item, FstrJong);
            if (result < 0)
            {
                MessageBox.Show("HIC_MISU_MST에 자료를 신규등록시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool MisuDetail()
        {
            HIC_MISU_DETAIL item = new HIC_MISU_DETAIL
            {
                WRTNO =             nWrtno,
                MISUNO =            nMisuNo,
                GUBUN =             "H",
                GJJONG =            strGJong,
                LTDCODE =           strltdcode.To<long>(0),
                MISUJONG =          "2",
                BDATE =             strBDate,
                ENTJOBSABUN =       clsType.User.IdNumber.To<long>(0),
                MISUAMT =           nTempAmt - nDentAmt1,
                IPGUMAMT =          0,
                GAMAMT =            0,
                BANAMT =            0,
                SAKAMT =            0,
                HALAMT =            0,
                ETCAMT =            0,
                JANAMT =            0,
                DAMNAME =           clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, clsType.User.IdNumber.Trim()),
                REMARK =            ""
            };

            result = hicMisuDetailService.GongDanInsert(item);

            if (result < 0)
            {
                MessageBox.Show("HIC_MISU_DETAIL에 자료를 신규등록시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool GuGangGongDan()
        {
            result = hicSunapService.GuGangGongDan(nMisuNo, Wrtno, FstrJong);

            if (result < 0)
            {
                MessageBox.Show("수납마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool GongDanChungGu()
        {
            result = hicJepsuService.GongDanChungGu(nMisuNo, Wrtno, FstrJong);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 회사청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Screen_Clear()
        {
            TxtGiroNo.Text = "";
            TxtLtdCode.Text = "";
            TxtLtdName.Text = "";
            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtPummok.Text = "";
            PanelKihoName.Text = "";
            TxtChaRemark.Text = "";
            PanelBogun.Text = "";
            TxtChaAmt.Text = "";

            SS_Clear(SS2_Sheet1);
        }

        private void DisPlay_Screen()
        {
            
            long nWrtno;
            string strDENTAL;

            nRow = 0;
            JongSQL.Clear();

            if (VB.Left(cboJong.Text, 1) == "6" && cboGbn.Text.Trim() == "")
            {
                MessageBox.Show("구강미수생성시 구분을 선택하세요.", "작업선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FstrJong = VB.Left(cboJong.Text, 1);
            if(FstrJong == "6")
            {
                for (int i = 0; i < Jong_6.Length; i++)
                {
                    JongSQL.Add(Jong_6[i]);
                }
            }

            //SS2.ActiveSheet.Cells[0, 11].Text = "구강뺀 조합부담";
            if((FstrJong != "1" && FstrJong != "2" && FstrJong != "3") && VB.Left(cboDtl.Text, 1) != "*")
            {
                MessageBox.Show("사업장미수만 상세분류 선택이 가능합니다.", "작업선택", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            switch(VB.Left(cboDtl.Text, 1))
            {
                case "1":
                    for(int i = 0; i < Jong_1.Length; i++)
                    {
                        JongSQL.Add(Jong_1[i]);
                        FstrJongSQL += Jong_1[i];
                    }
                    break;
                case "2":
                    for (int i = 0; i < Jong_2.Length; i++)
                    {
                        JongSQL.Add(Jong_2[i]);
                        FstrJongSQL += Jong_2[i];
                    }
                    break;
                case "3":
                    for (int i = 0; i < Jong_3.Length; i++)
                    {
                        JongSQL.Add(Jong_3[i]);
                        FstrJongSQL += Jong_3[i];
                    }
                    break;
                case "4":
                    for (int i = 0; i < Jong_4.Length; i++)
                    {
                        JongSQL.Add(Jong_4[i]);
                        FstrJongSQL += Jong_4[i];
                    }
                    break;
                case "5":
                    List<HIC_EXJONG> item = hicExjongService.GetCode(FstrJong, Jong_1, Jong_2, Jong_3, Jong_4);
                    FstrJongSQL = "";
                    for(int i = 0; i < item.Count; i++)
                    {
                        FstrJongSQL += item[i].CODE;
                        JongSQL.Add(FstrJongSQL);
                    }
                    if(FstrJongSQL != "")
                    {
                        FstrJongSQL = VB.Left(FstrJongSQL, VB.Len(FstrJongSQL) - 1);
                        JongSQL.Add(FstrJongSQL);
                    }
                    else
                    {
                        FstrJongSQL = "XX"; // 해당 검진종류 없음
                        JongSQL.Add(FstrJongSQL);
                    }
                    break;
                default:
                    FstrJongSQL = "";   // 전체
                    JongSQL.Add(FstrJongSQL);
                    break;
            }

            SS1.ActiveSheet.RowCount = 50;
            SS_Clear(SS1_Sheet1);

            // 공단청구번호 칼럼명 설정

            if(FstrJong == "4") { FstrCOLNM = "Mirno3"; } // 암검진
            else if(FstrJong == "6") { FstrCOLNM = "Mirno2"; } // 구강
            else { FstrCOLNM = "Mirno1"; }

            List<HIC_JEPSU_SUNAP> list = hicJepsuSunapService.GetGongDanItem(JongSQL, FstrCOLNM, FstrJongSQL, dtpFDate.Text, dtpTDate.Text, FstrJong, VB.Left(cboJong.Text, 1), TxtBogun.Text, TxtKiho.Text.Trim(), ChkW_Am.Checked, cboGbn.Text.Trim(), VB.Left(cboGbn.Text, 1), cboDent.Text.Trim(), rdoChasu2.Checked, rdoChasu3.Checked);

            strOldData = "";
            nCnt = 0;
            nTotAmt = 0;
            strMinDate = "";
            strMaxDate = "";

            for(int i = 0; i < list.Count; i++)
            {
                if (FstrJong == "4")
                {
                    strNewData = list[i].MIRNO3.ToString();
                }
                else if (FstrJong == "6")
                {
                    strNewData = list[i].MIRNO2.ToString();
                }
                else
                {
                    strNewData = list[i].MIRNO1.ToString();
                }
                
                nWrtno = list[i].WRTNO;
                if(list[i].WRTNO == 538317)
                {
                    i = i;
                }

                strDENTAL = "";

                List<HIC_SUNAPDTL> AllItem = hicSunapdtlService.GetAll(nWrtno);
                if(AllItem.Count > 0)
                {
                    strDENTAL = "OK";
                }

                // 암검진
                if(FstrJong == "4")
                {
                    cHb.READ_SUNAPDTL_Calculator(list[i].WRTNO);
                    nAmt = clsHcVariable.GnAmt_Misu_JohapAmt1.To<long>(0);
                }
                else
                {
                    nAmt = list[i].JOHAPAMT;
                }

                if(nAmt != 0)
                {
                    if(strOldData == "") { strOldData = strNewData; }
                    if(strOldData != strNewData) { nDentCnt = 0; View_display(); }
                    //회사별 인원 및 금액을 누적
                    nCnt += 1;

                    // 구강일경우
                    if (FstrJong == "6")
                    {
                        if (list[i].DENTAMT > 0)
                        {
                            nDentCnt += 1;
                            nTotAmt += list[i].DENTAMT;
                        }
                    }
                    else
                    {
                        // 일반
                        if (list[i].DENTAMT > 0 && strDENTAL == "OK")
                        {
                            nDentCnt += 1;
                            nTotAmt += (nAmt - list[i].DENTAMT);
                        }
                        else
                        {
                            nTotAmt += nAmt;
                        }
                    }

                    // 검진시작일, 종료일
                    strDate = list[i].MINDATE;
                    if (strMinDate == "") { strMinDate = strDate; }
                    if (string.Compare(strDate, strMinDate) < 0) { strMinDate = strDate; }
                    strDate = list[i].MAXDATE;
                    if (strMaxDate == "") { strMaxDate = strDate; }
                    if (string.Compare(strDate, strMaxDate) > 0) { strMaxDate = strDate; }
                }
            }

            View_display();
            SS1.ActiveSheet.RowCount = nRow.To<int>(0);
        }

        private void View_display()
        {
            nRow += 1;
            if (nRow > SS1.ActiveSheet.RowCount)
            {
                SS1.ActiveSheet.RowCount = nRow;
            }

            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = "0174";
            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = "국민건강보험공단";
            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = strMinDate;
            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = strMaxDate;
            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = nCnt.ToString();
            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = VB.Format(nTotAmt, "###,###,###,##0");
            SS1.ActiveSheet.Cells[nRow - 1, 8].Text = clsHcVariable.GstrKiho;
            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strOldData;

            List<HIC_JEPSU> item = hicJepsuService.GetCountGongDan(strMinDate, strMaxDate, strOldData);

            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = item[0].CNT.ToString();

            strOldData = strNewData;
            strMinDate = "";
            strMaxDate = "";
            nCnt = 0;
            nTotAmt = 0;

            return;
        }

        private void LtdHelp()
        {   
             string strLtdCode = "";

             if (TxtLtdCode.Text.IndexOf(".") > 0)
             {
                 strLtdCode = VB.Pstr(TxtLtdCode.Text, ".", 2);
             }
             else
             {
                 strLtdCode = TxtLtdCode.Text;
             }

             FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
             FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
             FrmHcLtdHelp.ShowDialog();
             FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

             if (!LtdHelpItem.IsNullOrEmpty())
             {
                TxtLtdCode.Text = LtdHelpItem.CODE.ToString();
                TxtLtdName.Text = LtdHelpItem.SANGHO;
            }
             else
             {
                 TxtLtdCode.Text = "";
                 TxtLtdName.Text = "";
             }
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void KihoHelp()
        {
            clsPublic.GstrRetValue = "18";      //사업장 기호
            FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
            FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
            FrmHcCodeHelp.ShowDialog();

            if (!FstrCode.IsNullOrEmpty())
            {
                TxtKiho.Text = FstrCode.Trim();
                PanelKihoName.Text = FstrName.Trim();
            }
            else
            {
                TxtKiho.Text = "";
                PanelKihoName.Text = "";
            }

            FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
        }

        private void BogunHelp()
        {
            clsPublic.GstrRetValue = "25";      //보건소 기호
            FrmHcCodeHelp = new frmHcCodeHelp(clsPublic.GstrRetValue);
            FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
            FrmHcCodeHelp.ShowDialog();

            if (!FstrCode.IsNullOrEmpty())
            {
                TxtBogun.Text = FstrCode.Trim();
                PanelBogun.Text = FstrName.Trim();
            }
            else
            {
                TxtBogun.Text = "";
                PanelBogun.Text = "";
            }

            FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
        }

        private void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        private void SelfChungGu()
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strltdcode = SS1.ActiveSheet.Cells[i, 1].Text;
                    strFDate = SS1.ActiveSheet.Cells[i, 3].Text;
                    strTDate = SS1.ActiveSheet.Cells[i, 4].Text;
                    nAmt = SS1.ActiveSheet.Cells[i, 6].Text.To<double>(0);

                    // 미수번호 찾기
                    List<HIC_MISU_MST_SLIP> MisuNumSearch = hicMisuMstSlipService.getMisuNum(strltdcode, FstrJong, strTDate, nAmt);
                    nMisuNo = 0;
                    strBDate = "";
                    if (MisuNumSearch.Count > 0)
                    {
                        nMisuNo = MisuNumSearch[0].WRTNO;
                        strBDate = MisuNumSearch[0].BDATE.Trim();
                        strROWID = MisuNumSearch[0].ROWID.Trim();
                    }

                    if (nMisuNo > 0)
                    {
                        if (!RowidUpdate())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (FstrJong.To<int>(0) <= 4)
                        {
                            SelfChungGu_Sub();
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void SelfChungGu_Sub()
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (!CompanyChunguUpdate())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!CompanySuNapUpdate())
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private bool CompanySuNapUpdate()
        {
            result = hicSunapService.CompanySuNapUpdate(nMisuNo, strFDate, strTDate, strltdcode);

            if (result < 0)
            {
                MessageBox.Show("수납마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool CompanyChunguUpdate()
        {
            result = hicJepsuService.CompanyChunguUpdate(nMisuNo, strFDate, strTDate, strltdcode, FstrJong);

            if (result < 0)
            {
                MessageBox.Show("접수마스타에 공단청구 완료처리 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool RowidUpdate()
        {
            result = hicMisuMstSlipService.UpdateRowid(strROWID);

            if (result < 0)
            {
                MessageBox.Show("ROWID UPDATA 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void menuPrint(bool Gubun)
        {
            if (Gubun == true)
            {
                string strTitle = "";
                string strSign = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "공단부담액 청구대상 공단명단";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += cSpd.setSpdPrint_String("인쇄시각 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                string strTitle = "";
                string strSign = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건강진단비 청구내역";
                strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += cSpd.setSpdPrint_String("작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text + VB.Space(10) + "회사명 : " + TxtLtdName.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = cSpd.setSpdPrint_String(strSign, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 40, 40);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, false, false, false, true, 1f);
                cSpd.setSpdPrint(SS2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            long nYY;
            SS2.ActiveSheet.Columns.Get(15).Visible = false;
            read_sysdate();

            dtpFDate.Text = cF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -90);
            dtpTDate.Text = clsPublic.GstrSysDate;
            TxtDate.Text = clsPublic.GstrSysDate;

            cboDtl.Clear();
            cboDtl.Items.Add("*. 전체");
            cboDtl.Items.Add("1.특수검진");
            cboDtl.Items.Add("2.채용배치전");
            cboDtl.Items.Add("3.방사선");
            cboDtl.Items.Add("4.특수기타");
            cboDtl.SelectedIndex = 1;

            cboJong.Clear();
            cboJong.Items.Add("1.성인병");
            cboJong.Items.Add("2.공무원");
            cboJong.Items.Add("3.사업장");
            cboJong.Items.Add("4.암검진");
            cboJong.Items.Add("5.기타검진");
            cboJong.Items.Add("6.구강검진");
            cboJong.SelectedIndex = 2;

            cboDent.Clear();
            cboDent.Items.Add("전체");
            cboDent.Items.Add("직장");
            cboDent.Items.Add("공교");
            cboDent.Items.Add("지역");
            cboDent.Items.Add("급여");
            cboDent.SelectedIndex = 0;

            cboGbn.Clear();
            cboGbn.Items.Add("1.성인병");
            cboGbn.Items.Add("2.공무원");
            cboGbn.Items.Add("3.사업장");
            cboGbn.SelectedIndex = 0;
            
            TxtGiroNo.Text = "";
            TxtInwon.Text = "";
            TxtMisuAmt.Text = "";
            TxtGiroNo.Text = "";
            TxtRemark.Text = "";
            TxtPummok.Text = "";
            TxtLtdCode.Text = "";
            TxtLtdName.Text = "";

            PanelKihoName.Text = "";
            PanelBogun.Text = "";
            TxtChaAmt.Text = "";
            TxtChaRemark.Text = "";
            TxtYYMM.Text = "";

            menuEtc.Enabled = false;
            nYY = VB.Left(clsPublic.GstrSysDate, 4).To<long>(0);
            cboYear.Clear();

            for (int i = 0; i < 6; i++)
            {
                cboYear.Items.Add(VB.Format(nYY, "0000"));
                nYY -= 1;
            }
            cboYear.SelectedIndex = 0;
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            cpublic = new clsPublic();
            cF = new ComFunc();
            cHb = new clsHaBase();
            cHM = new clsHcMisu();
            hM = new clsHcMain();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicMirCancerService = new HicMirCancerService();
            hicCodeService = new HicCodeService();
            hicJepsuService = new HicJepsuService();
            hicSunapService = new HicSunapService();
            hicMisuDetailService = new HicMisuDetailService();
            hicMisuMstSlipService = new HicMisuMstSlipService();
            hicLtdService = new HicLtdService();
            hicComHpcService = new HicComHpcService();
            hicSunapdtlService = new HicSunapdtlService();
            hicExjongService = new HicExjongService();
        }

        private void rdoJob2_CheckedChanged(object sender, EventArgs e)
        {
            Inwon_Amt_Gesan();
        }

        private void rdoJob1_CheckedChanged(object sender, EventArgs e)
        {
            Inwon_Amt_Gesan();
        }

        private void Inwon_Amt_Gesan()
        {
            long nCnt, nMisuAmt;
            long nMisuAmt1;
            string strChk = "";
            string strOldData;
            string strNewData;

            string strFlag;
            string strChasu;
            long nChaCnt1;
            long nChaCnt2;
            long nChaCnt3;
            long nChaSUM1;
            long nChaSUM2;
            long nChaSUM3;

            string strOK;
            string strJong;
            long nWrtno;


            strFlag = "";
            strJong = "";
            nChaCnt1 = 0;
            nChaCnt2 = 0;
            nChaCnt3 = 0;
            nChaSUM1 = 0;
            nChaSUM2 = 0;
            nChaSUM3 = 0;
            strOldData = "";
            nCnt = 0;
            nMisuAmt = 0;

            for (int i = 0; i < SS2.ActiveSheet.RowCount - 1; i++)
            {
                if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "")
                {
                    strChk = "False";
                }
                else if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "True")
                {
                    strChk = "True";
                }
                else if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "False")
                {
                    strChk = "False";
                }
                
                nWrtno = SS2.ActiveSheet.Cells[i, 1].Text.Trim().To<long>(0);
                nMisuAmt1 = VB.Replace(SS2.ActiveSheet.Cells[i, 11].Text, ",", "").To<long>(0);
                strChasu = SS2.ActiveSheet.Cells[i, 13].Text.Trim();

                strOK = "";
                if (rdoJob1.Checked == true && strChk == "True")
                {
                    strOK = "OK";
                }
                if (rdoJob2.Checked == true && strChk == "False")
                {
                    strOK = "OK";
                }

                if (strOK == "OK" && nMisuAmt1 != 0)
                {
                    // 인원카운트
                    strNewData = SS2.ActiveSheet.Cells[i, 4].Text.Trim();
                    if (strOldData != strNewData) { nCnt += 1; strOldData = strNewData; }

                    // 차수별 카운트
                    if (strChasu == "1")
                    {
                        nChaCnt1 += 1;
                        nChaSUM1 += nMisuAmt1;
                    }
                    else if (strChasu == "2")
                    {
                        nChaCnt2 += 1;
                        nChaSUM2 += nMisuAmt1;
                    }
                    else
                    {
                        nChaCnt3 += 1;
                        nChaSUM3 += nMisuAmt1;
                    }

                    // 총금액
                    nMisuAmt += nMisuAmt1;
                }
            }

            TxtInwon.Text = VB.Format(nCnt, "###,##0");
            TxtMisuAmt.Text = VB.Format(nMisuAmt, "###,###,###,##0");

            switch (FstrJong)
            {
                case "1":
                    strJong = "성인병";
                    break;
                case "2":
                    strJong = "공무원";
                    break;
                case "3":
                    strJong = "사업장";
                    break;
                case "4":
                    strJong = "암검진";
                    break;
                case "5":
                    strJong = "기타검진";
                    break;
            }

            if (nChaCnt3 > 0) { strFlag = "3"; }
            if (FstrJong == "6") { strFlag = "3"; }
            if (strFlag != "3")
            {
                // 2010-11-08 윤조연 수정
                if (nChaSUM2 == 0)
                {
                    TxtRemark.Text = strJong + "1차 금액 :" + VB.Format(nChaSUM1, "###,###,##0") + "원";
                }
                else
                {
                    TxtRemark.Text = strJong + "1차 금액 :" + VB.Format(nChaSUM1, "###,###,##0") + "원, 2차 금액 : " + VB.Format(nChaSUM2, "###,###,##0") + "원";
                }

                if (nChaCnt2 == 0)
                {
                    TxtPummok.Text = "1차 인원 : " + VB.Format(nChaCnt1, "###,###,##0") + "명";
                }
                else
                {
                    TxtPummok.Text = "1차 인원 : " + VB.Format(nChaCnt1, "###,###,##0") + "명, 2차 인원 : " + VB.Format(nChaCnt2, "###,###,##0") + "명";
                }
            }
            else
            {
                if (FstrJong != "6")
                {
                    TxtRemark.Text = strJong + "금액 : " + VB.Format(nMisuAmt, "###,###,##0") + "원";
                    TxtPummok.Text = "인원 : " + VB.Format(nCnt, "###,###,##0") + "명";
                }
                else
                {
                    TxtRemark.Text = strJong + "구강금액 : " + VB.Format(nMisuAmt, "###,###,##0") + "원";
                    TxtPummok.Text = "인원 : " + VB.Format(nCnt, "###,###,##0") + "명";
                }
            }
        }

        private void SS2_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            Inwon_Amt_Gesan();
        }
    }
}